Imports MAntMonitor.Extensions

Public Class frmMain

    'handles the attempt to run this program when it's already running
    Public Event StartupNextInstance As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventHandler

    'what it sounds like
    Private ToldUserRunningInNotificationTray As Boolean

    'web browser control and busy indicators
    Private wb(0 To 2) As WebBrowser
    Private wbData(0 To 2) As clsWBData

    Private Class clsWBData
        Public IsBusy As Boolean
        Public StartTime As Date
    End Class

    'tracks last time emails and reboots occurred
    Private RebootInfo, EMailAlertInfo As System.Collections.Generic.Dictionary(Of String, Date)

    'the dataset that holds the grid data
    Private ds, dsAntConfig As DataSet

    'location of the configuration settings in the registry
    Private Const csRegKey As String = "Software\MAntMonitor"

    'version
    Private Const csVersion As String = "M's Ant Monitor v3.4"

    'alert string   
    Private sAlerts As String

    'how many seconds until next refresh
    Private iCountDown As Integer

    'how often we refresh
    Private iRefreshRate As Integer

    'class to make managing configuration settings easy
    Private ctlsByKey As ControlsByRegistry

    'used to prevent controls from firing before the app is fully started
    Private bStarted As Boolean

    'whether or not we're using the API instead of webscraping
    'Private bUseAPI As Boolean

    'log queue from other threads
    Private Shared logQueue As System.Collections.Generic.Queue(Of String)
    Private Shared logQueueLock As Object

    'queue of ants to check
    Private Shared antsToCheckQueue As System.Collections.Generic.Queue(Of stAntConfig)
    Private Shared antsToCheckLock As Object

    'data coming back from the worker threads with Ant refresh data
    Private Shared AntRefreshDataQueue As System.Collections.Generic.Queue(Of clsAntRefreshData)
    Private Shared AntRefreshLock As Object

    'display refresh period after ant refresh is initiated
    Private iDisplayRefreshPeriod As Integer

    'object populated by the worker threads that is passed back to the UI thread for grid population
    Private Class clsAntRefreshData
        Public AntType As enAntType
        Public ID As Integer
        Public sStats As String
        Public sSummary As String
        Public sPools As String
        Public sAntIP As String
        Public bError As Boolean
        Public ex As Exception
    End Class

    'ant data from the config
    Private Structure stAntConfig
        Dim sName As String
        Dim sIP As String
        Dim AntType As enAntType
        Dim sAPIPort As String
        Dim sHTTPPort As String
        Dim sSSHPort As String
        Dim ID As Integer
        Dim sSSHPassword As String
        Dim sSSHUsername As String
        Dim sWebUsername As String
        Dim sWebPassword As String
    End Structure

    '# of ants enabled in the config
    Private iAntsEnabled As Integer

    'worker threads and data
    Private workerThread() As System.Threading.Thread
    Private ThreadHandlers() As clsThreadHandler

    Private Class clsThreadHandler
        Public bBusy As Boolean
        Public AntToCheck As stAntConfig
        Public bGotWork As Boolean
    End Class

    'set to true when shutting down
    Private bShutDown As Boolean

    'ant types
    Private Enum enAntType
        S1 = 1
        S2 = 2
        S3 = 3
    End Enum

    'pool data for pushing pool data to ants
    Private Class clsPoolData
        Public URL As String
        Public UID As String
        Public PW As String

        Public Sub New()
            PW = ""
        End Sub
    End Class

    'for Ant scanning on another thread
    Private sIPDataResponse As String
    Private sIPToCheck As String

#If DEBUG Then
    Private Const bErrorHandle As Boolean = False
#Else
    Private Const bErrorHandle As Boolean = True
#End If

    Private Sub Form1_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim host As System.Net.IPHostEntry
        Dim x As Integer
        'Dim sTemp As String
        Dim s() As String
        Dim dr As DataRow
        Dim AntType As enAntType

        bStarted = True

        Me.Text = csVersion

        'initialize objects
        logQueue = New System.Collections.Generic.Queue(Of String)
        logQueueLock = New Object

        antsToCheckQueue = New System.Collections.Generic.Queue(Of stAntConfig)
        antsToCheckLock = New Object

        AntRefreshDataQueue = New System.Collections.Generic.Queue(Of clsAntRefreshData)
        AntRefreshLock = New Object

        wbData(0) = New clsWBData
        wbData(1) = New clsWBData
        wbData(2) = New clsWBData

        AddToLogQueue(csVersion & " starting")

        host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName)

        For Each IP As System.Net.IPAddress In host.AddressList
            If IP.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                s = IP.ToString.Split(".")

                Me.txtIPRangeToScan.Text = s(0) & "." & s(1) & "." & s(2)
            End If
        Next

        For x = 1 To 254
            Me.cmbAntScanStart.Items.Add(x)
            Me.cmbAntScanStop.Items.Add(x)
        Next

        RebootInfo = New System.Collections.Generic.Dictionary(Of String, Date)
        EMailAlertInfo = New System.Collections.Generic.Dictionary(Of String, Date)

        'ant output
        ds = New DataSet

        With ds
            .Tables.Add()
            Me.dataAnts.DataSource = .Tables(0)

            With .Tables(0).Columns
                .Add("Name")
                .Add("Uptime")
                .Add("GH/s(5s)", GetType(Double))
                .Add("GH/s(avg)", GetType(Double))
                .Add("Blocks")
                .Add("HWE%")
                .Add("BestShare")
                .Add("Diff")
                .Add("Pools")
                .Add("PoolData")
                .Add("PoolData2", GetType(Object))
                .Add("Rej%")
                .Add("Stale%")
                .Add("HFan", GetType(Integer))
                .Add("Fans")
                .Add("HTemp", GetType(Integer))
                .Add("Temps")
                .Add("Freq", GetType(Double))
                .Add("XCount")
                .Add("Status")
                .Add("ACount", GetType(Integer))
                '.Add("IPAddress")
                .Add("ID")
                .Add("Type")
            End With
        End With

        Me.dataAnts.Columns("PoolData").Visible = False
        Me.dataAnts.Columns("PoolData2").Visible = False
        'Me.dataAnts.Columns("IPAddress").Visible = False
        Me.dataAnts.Columns("Type").Visible = False

        Me.dataAnts.Columns("HWE%").ToolTipText = "Hardware Error Percentage"
        Me.dataAnts.Columns("Diff").ToolTipText = "Difficulty Ant is using.  For web scraping, it's the value from all 3 pools."
        Me.dataAnts.Columns("HFan").ToolTipText = "Highest Fan Speed"
        Me.dataAnts.Columns("Rej%").ToolTipText = "Reject Percentage"
        Me.dataAnts.Columns("HTemp").ToolTipText = "Highest Temperature across all blades"
        Me.dataAnts.Columns("Freq").ToolTipText = "Frequency Ant is running at"
        Me.dataAnts.Columns("XCount").ToolTipText = "Number of Xs this Ant has"
        Me.dataAnts.Columns("ACount").ToolTipText = "Alert count for this Ant"

        With Me.dataAnts
            .Columns("ACount").Width = 74
            .Columns("Blocks").Width = 65
            .Columns("Diff").Width = 48
            .Columns("Freq").Width = 62
            .Columns("GH/s(5s)").Width = 87
            .Columns("GH/s(avg)").Width = 92
            .Columns("HFan").Width = 63
            .Columns("HTemp").Width = 73
            .Columns("HWE%").Width = 76
            .Columns("Name").Width = 69
            .Columns("Pools").Width = 60
            .Columns("Rej%").Width = 65
            .Columns("Stale%").Width = 71
            .Columns("Status").Width = 266
            .Columns("Temps").Width = 220
            .Columns("XCount").Width = 71
            .Columns("ID").Width = 49
        End With

        ctlsByKey = New ControlsByRegistry(csRegKey)

        Call SetGridSizes("\Columns\dataAnts", Me.dataAnts)
        Call SetGridColumnPositions("\Columns\" & Me.dataAnts.Name & "_DisplayIndex", Me.dataAnts)

        'handles saving of column widths and column locations
        AddHandler Me.dataAnts.ColumnWidthChanged, AddressOf Me.dataGrid_ColumnWidthChanged
        AddHandler Me.dataAnts.ColumnDisplayIndexChanged, AddressOf Me.dataAnts_ColumnDisplayIndexChanged

        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey)
            If key Is Nothing Then
                My.Computer.Registry.CurrentUser.CreateSubKey(csRegKey)
            End If
        End Using

        'load configuration
        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey)
            If key.GetValue("Width") > 100 Then
                Me.Width = key.GetValue("Width")
            End If

            If key.GetValue("Height") > 100 Then
                Me.Height = key.GetValue("Height")
            End If

            If key.GetValue("ToldUserAboutNotification") = "Y" Then
                ToldUserRunningInNotificationTray = True
            End If
        End Using

        With ctlsByKey
            'options
            '.AddControl(Me.chklstAnts, "AntList")
            .AddControl(Me.txtRefreshRate, "RefreshRateValue")
            .AddControl(Me.cmbRefreshRate, "RefreshRateVolume")
            .AddControl(Me.chkShowBestShare, "ShowBestShare")
            .AddControl(Me.chkShowBlocks, "ShowBlocks")
            .AddControl(Me.chkShowFans, "ShowFans")
            .AddControl(Me.chkShowGHs5s, "ShowGHs5s")
            .AddControl(Me.chkShowGHsAvg, "ShowGHsAvg")
            .AddControl(Me.chkShowHWE, "ShowHWE")
            .AddControl(Me.chkShowPools, "ShowPools")
            .AddControl(Me.chkShowStatus, "ShowStatus")
            .AddControl(Me.chkShowTemps, "ShowTemps")
            .AddControl(Me.chkShowUptime, "ShowUptime")
            .AddControl(Me.chkShowFreqs, "ShowFreqs")
            .AddControl(Me.chkShowHighFan, "ShowHighFan")
            .AddControl(Me.chkShowHighTemp, "ShowHighTemp")
            .AddControl(Me.chkShowXCount, "ShowXCount")
            .AddControl(Me.chkShowRej, "ShowReject")
            .AddControl(Me.chkShowStale, "ShowStale")
            .AddControl(Me.chkShowDifficulty, "ShowDifficulty")
            .AddControl(Me.chkShowACount, "ShowACount")

            .AddControl(Me.chkShowSelectionColumn, "ShowSelectionColumn")

            '.AddControl(Me.chkUseAPI, "UseAPI")

            .AddControl(Me.trackThreadCount, "WorkerThreadCount")
            .AddControl(Me.txtDisplayRefreshInSecs, "DisplayRefreshPeriod")

            'alerts
            .AddControl(Me.chkAlertIfS1Temp, "AlertIfS1Temp")
            .AddControl(Me.chkAlertIfS2Temp, "AlertIfS2Temp")
            .AddControl(Me.chkAlertIfS3Temp, "AlertIfS3Temp")
            .AddControl(Me.txtAlertS1Temp, "AlertValueS1Temp")
            .AddControl(Me.txtAlertS2Temp, "AlertValueS2Temp")
            .AddControl(Me.txtAlertS3Temp, "AlertValueS3Temp")

            .AddControl(Me.chkAlertIfS1FanHigh, "AlertIfS1Fan")
            .AddControl(Me.chkAlertIfS2FanHigh, "AlertIfS2Fan")
            .AddControl(Me.chkAlertIfS3FanHigh, "AlertIfS3Fan")
            .AddControl(Me.txtAlertS1FanHigh, "AlertValueS1Fan")
            .AddControl(Me.txtAlertS2FanHigh, "AlertValueS2Fan")
            .AddControl(Me.txtAlertS3FanHigh, "AlertValueS3Fan")

            .AddControl(Me.chkAlertIfS1FanLow, "AlertIfS1FanLow")
            .AddControl(Me.chkAlertIfS2FanLow, "AlertIfS2FanLow")
            .AddControl(Me.chkAlertIfS3FanLow, "AlertIfS3FanLow")
            .AddControl(Me.txtAlertS1FanLow, "AlertValueS1FanLow")
            .AddControl(Me.txtAlertS2FanLow, "AlertValueS2FanLow")
            .AddControl(Me.txtAlertS3FanLow, "AlertValueS3FanLow")

            .AddControl(Me.chkAlertIfS1Hash, "AlertIfS1Hash")
            .AddControl(Me.chkAlertIfS2Hash, "AlertIFS2Hash")
            .AddControl(Me.chkAlertIfS3Hash, "AlertIFS3Hash")
            .AddControl(Me.txtAlertS1Hash, "AlertValueS1Hash")
            .AddControl(Me.txtAlertS2Hash, "AlertValueS2Hash")
            .AddControl(Me.txtAlertS3Hash, "AlertValueS3Hash")

            .AddControl(Me.chkAlertIfS1XCount, "AlertIfS1XCount")
            .AddControl(Me.chkAlertIfS2XCount, "AlertIFS2XCount")
            .AddControl(Me.chkAlertIfS3XCount, "AlertIFS3XCount")
            .AddControl(Me.txtAlertS1XCount, "AlertValueS1XCount")
            .AddControl(Me.txtAlertS2XCount, "AlertValueS2XCount")
            .AddControl(Me.txtAlertS3XCount, "AlertValueS3XCount")

            .AddControl(Me.chkAlertHighlightField, "AlertHighlightField")
            .AddControl(Me.chkAlertShowAnnoyingPopup, "AlertShowAnnoyingPopup")
            .AddControl(Me.chkAlertShowNotifyPopup, "AlertShowNotifyPopup")
            .AddControl(Me.chkAlertStartProcess, "AlertStartProcess")
            .AddControl(Me.txtAlertStartProcessName, "AlertProcessName")
            .AddControl(Me.txtAlertStartProcessParms, "AlertProcessParms")
            .AddControl(Me.txtAlertEMailGovernor, "AlertEMailGovernorSize")
            .AddControl(Me.cmbAlertEMailGovernor, "AlertEMailGovernorValue")

            .AddControl(Me.chkAlertSendEMail, "AlertSendEMail")

            'email settings
            .AddControl(Me.txtSMTPServer, "SMTPServerName")
            .AddControl(Me.txtSMTPPort, "SMTPServerPort")
            .AddControl(Me.txtSMTPUserName, "SMTPUserName")
            .AddControl(Me.txtSMTPPassword, "SMTPUserPassword")
            .AddControl(Me.txtSMTPAlertName, "SMTPAlertName")
            .AddControl(Me.txtSMTPAlertAddress, "SMTPAlertAddress")
            .AddControl(Me.txtSMTPAlertSubject, "SMTPAlertSubject")
            .AddControl(Me.txtSMTPFromName, "SMTPFromName")
            .AddControl(Me.txtSMTPFromAddress, "SMTPFromAddress")
            .AddControl(Me.chkSMTPSSL, "SMTPUseSSL")

            .AddControl(Me.chkAlertRebootIfXd, "RebootAntIfXd")
            .AddControl(Me.chkAlertRebootAntsOnHashAlert, "RebootAntIfHashAlert")
            .AddControl(Me.chkRebootAntOnError, "RebootAntIfError")
            .AddControl(Me.txtAlertRebootGovernor, "AlertRebootGovernor")
            .AddControl(Me.cmbAlertRebootGovernor, "AlertRebootGovernorValue")

            'upgrade code
            '.SetControlByRegKeyAnt(Me.chklstAnts)

            'establish credentials for existing Ants when they aren't already there
            'For x = 0 To Me.chklstAnts.Items.Count - 1
            '    Dim sAnt, sDefaultUN, sDefaultPW As String

            '    sAnt = Me.chklstAnts.Items(x)

            '    sDefaultUN = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey, "Username", "root")
            '    sDefaultPW = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey, "Password", "root")

            '    If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sAnt, "WebUsername", "") Is Nothing Then
            '        If sAnt.Substring(0, 2) = "S1" OrElse sAnt.Substring(0, 2) = "S3" Then
            '            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sAnt, "WebUsername", sDefaultUN, Microsoft.Win32.RegistryValueKind.String)
            '            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sAnt, "WebPassword", sDefaultPW, Microsoft.Win32.RegistryValueKind.String)
            '            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sAnt, "SSHUsername", sDefaultUN, Microsoft.Win32.RegistryValueKind.String)
            '            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sAnt, "SSHPassword", sDefaultPW, Microsoft.Win32.RegistryValueKind.String)
            '        End If

            '        If sAnt.Substring(0, 2) = "S2" Then
            '            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sAnt, "WebUsername", sDefaultUN, Microsoft.Win32.RegistryValueKind.String)
            '            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sAnt, "WebPassword", sDefaultPW, Microsoft.Win32.RegistryValueKind.String)
            '            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sAnt, "SSHUsername", "root", Microsoft.Win32.RegistryValueKind.String)
            '            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sAnt, "SSHPassword", "admin", Microsoft.Win32.RegistryValueKind.String)
            '        End If
            '    End If

            '    'add the port to the end of the Ants if they aren't already there
            '    sTemp = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sAnt, "Port", "")

            '    If sTemp.IsNullOrEmpty Then
            '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sAnt, "Port", 80, Microsoft.Win32.RegistryValueKind.String)

            '        sTemp = "80"
            '    End If

            '    Me.chklstAnts.Items(x) = Me.chklstAnts.Items(x) & ":" & sTemp
            'Next

            .SetControlByRegKey(Me.txtRefreshRate, "300")
            .SetControlByRegKey(Me.cmbRefreshRate, "Seconds")
            .SetControlByRegKey(Me.chkShowBestShare, True)
            .SetControlByRegKey(Me.chkShowBlocks, True)
            .SetControlByRegKey(Me.chkShowFans, True)
            .SetControlByRegKey(Me.chkShowGHs5s, True)
            .SetControlByRegKey(Me.chkShowGHsAvg, True)
            .SetControlByRegKey(Me.chkShowHWE, True)
            .SetControlByRegKey(Me.chkShowPools, True)
            .SetControlByRegKey(Me.chkShowStatus, True)
            .SetControlByRegKey(Me.chkShowTemps, True)
            .SetControlByRegKey(Me.chkShowUptime, True)
            .SetControlByRegKey(Me.chkShowFreqs, True)
            .SetControlByRegKey(Me.chkShowHighTemp, True)
            .SetControlByRegKey(Me.chkShowHighFan, True)
            .SetControlByRegKey(Me.chkShowXCount, True)
            .SetControlByRegKey(Me.chkShowRej, True)
            .SetControlByRegKey(Me.chkShowStale, True)
            .SetControlByRegKey(Me.chkShowDifficulty, True)
            .SetControlByRegKey(Me.chkShowACount, True)

            .SetControlByRegKey(Me.chkShowSelectionColumn)

            '.SetControlByRegKey(Me.chkUseAPI, True)

            .SetControlByRegKey(Me.trackThreadCount, 5)
            .SetControlByRegKey(Me.txtDisplayRefreshInSecs, "1")

            Call txtDisplayRefreshInSecs_Leave(sender, e)

            'alerts
            .SetControlByRegKey(Me.chkAlertIfS1Temp)
            .SetControlByRegKey(Me.chkAlertIfS2Temp)
            .SetControlByRegKey(Me.chkAlertIfS3Temp)
            .SetControlByRegKey(Me.txtAlertS1Temp)
            .SetControlByRegKey(Me.txtAlertS2Temp)
            .SetControlByRegKey(Me.txtAlertS3Temp)

            .SetControlByRegKey(Me.chkAlertIfS1FanHigh)
            .SetControlByRegKey(Me.chkAlertIfS2FanHigh)
            .SetControlByRegKey(Me.chkAlertIfS3FanHigh)
            .SetControlByRegKey(Me.txtAlertS1FanHigh)
            .SetControlByRegKey(Me.txtAlertS2FanHigh)
            .SetControlByRegKey(Me.txtAlertS3FanHigh)

            .SetControlByRegKey(Me.chkAlertIfS1FanLow)
            .SetControlByRegKey(Me.chkAlertIfS2FanLow)
            .SetControlByRegKey(Me.chkAlertIfS3FanLow)
            .SetControlByRegKey(Me.txtAlertS1FanLow)
            .SetControlByRegKey(Me.txtAlertS2FanLow)
            .SetControlByRegKey(Me.txtAlertS3FanLow)

            .SetControlByRegKey(Me.chkAlertIfS1Hash)
            .SetControlByRegKey(Me.chkAlertIfS2Hash)
            .SetControlByRegKey(Me.chkAlertIfS3Hash)
            .SetControlByRegKey(Me.txtAlertS1Hash)
            .SetControlByRegKey(Me.txtAlertS2Hash)
            .SetControlByRegKey(Me.txtAlertS3Hash)

            .SetControlByRegKey(Me.chkAlertIfS1XCount)
            .SetControlByRegKey(Me.chkAlertIfS2XCount)
            .SetControlByRegKey(Me.chkAlertIfS3XCount)
            .SetControlByRegKey(Me.txtAlertS1XCount)
            .SetControlByRegKey(Me.txtAlertS2XCount)
            .SetControlByRegKey(Me.txtAlertS3XCount)

            .SetControlByRegKey(Me.chkAlertHighlightField, True)
            .SetControlByRegKey(Me.chkAlertShowNotifyPopup, True)
            .SetControlByRegKey(Me.chkAlertShowAnnoyingPopup)
            .SetControlByRegKey(Me.chkAlertStartProcess)
            .SetControlByRegKey(Me.txtAlertStartProcessName)
            .SetControlByRegKey(Me.txtAlertStartProcessParms)
            .SetControlByRegKey(Me.chkAlertSendEMail)

            .SetControlByRegKey(Me.chkAlertRebootIfXd, True)
            .SetControlByRegKey(Me.chkAlertRebootAntsOnHashAlert)
            .SetControlByRegKey(Me.chkRebootAntOnError)
            .SetControlByRegKey(Me.txtAlertRebootGovernor, 30)
            .SetControlByRegKey(Me.cmbAlertRebootGovernor, "Minutes")

            'email settings
            Call ctlsByKey.SetControlByRegKey(Me.txtSMTPServer)
            Call ctlsByKey.SetControlByRegKey(Me.txtSMTPPort)
            Call ctlsByKey.SetControlByRegKey(Me.txtSMTPUserName)
            Call ctlsByKey.SetControlByRegKey(Me.txtSMTPPassword)
            Call ctlsByKey.SetControlByRegKey(Me.txtSMTPAlertName)
            Call ctlsByKey.SetControlByRegKey(Me.txtSMTPAlertAddress)
            Call ctlsByKey.SetControlByRegKey(Me.txtSMTPAlertSubject)
            Call ctlsByKey.SetControlByRegKey(Me.txtSMTPFromName)
            Call ctlsByKey.SetControlByRegKey(Me.txtSMTPFromAddress)
            Call ctlsByKey.SetControlByRegKey(Me.chkSMTPSSL)

            Call ctlsByKey.SetControlByRegKey(Me.txtAlertEMailGovernor, "10")
            Call ctlsByKey.SetControlByRegKey(Me.cmbAlertEMailGovernor, "Minutes")
        End With

        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\Pools")
            If key Is Nothing Then
                My.Computer.Registry.CurrentUser.CreateSubKey(csRegKey & "\Pools")
            End If
        End Using

        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\Pools")
            For Each sKey As String In key.GetSubKeyNames
                Me.lstPools.AddItem(My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & sKey, "Description", ""), sKey)
            Next
        End Using

        'config
        dsAntConfig = New DataSet

        With dsAntConfig
            .Tables.Add()
            Me.dataAntConfig.DataSource = .Tables(0)

            With .Tables(0).Columns
                .Add("Name")
                .Add("Type")
                .Add("IPAddress")
                .Add("Active")
                .Add("APIPort")
                .Add("HTTPPort")
                .Add("SSHPort")
                .Add("ID", GetType(Integer))
                .Add("SSHPassword")
                .Add("SSHUsername")
                .Add("WebUsername")
                .Add("WebPassword")
                .Add("UseAPI")
                .Add("RebootViaSSH")
            End With
        End With

        Me.dataAntConfig.Columns("APIPort").Visible = False
        Me.dataAntConfig.Columns("HTTPPort").Visible = False
        Me.dataAntConfig.Columns("SSHPort").Visible = False
        Me.dataAntConfig.Columns("SSHPassword").Visible = False
        Me.dataAntConfig.Columns("SSHUsername").Visible = False
        Me.dataAntConfig.Columns("WebUsername").Visible = False
        Me.dataAntConfig.Columns("WebPassword").Visible = False
        Me.dataAntConfig.Columns("UseAPI").Visible = False
        Me.dataAntConfig.Columns("RebootViaSSH").Visible = False

        With Me.dataAntConfig
            .Columns("Active").Width = 62
            .Columns("ID").Width = 39
            .Columns("IPAddress").Width = 115
            .Columns("Name").Width = 76
            .Columns("Type").Width = 46
        End With

        'load Ants into the config
        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\AntsV2")
            If key Is Nothing Then
                My.Computer.Registry.CurrentUser.CreateSubKey(csRegKey & "\AntsV2")

                'convert Ants from prior version over
                Using key2 As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\Ants")
                    If key2 IsNot Nothing Then
                        x = 0

                        For Each sKey As String In key2.GetSubKeyNames
                            'do some validation before converting them over
                            If sKey.Split(":").Count = 2 AndAlso sKey.Split(".").Count = 4 Then
                                If sKey.Substring(0, 4) = "S1: " OrElse sKey.Substring(0, 4) = "S2: " OrElse sKey.Substring(0, 4) = "S3: " Then
                                    Select Case sKey.Substring(0, 2)
                                        Case "S1"
                                            AntType = enAntType.S1

                                        Case "S2"
                                            AntType = enAntType.S2

                                        Case "S3"
                                            AntType = enAntType.S3

                                    End Select

                                    s = sKey.Split(".")

                                    Call AddOrSaveAnt(-1, sKey.Substring(0, 3) & s(2) & "." & s(3), AntType, sKey.Substring(4), , My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sKey, "Port", "80"), _
                                                    My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sKey, "WebUsername", "root"), _
                                                    My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sKey, "WebPassword", "root"), _
                                                    , , , , My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey, "UseAPI", "Y"), _
                                                    My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey, "UseAPI", "Y"))

                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "Name", sKey, Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "Type", sKey.Substring(0, 2), Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "IPAddress", sKey.Substring(4), Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "HTTPPort", My.Computer.Registry.GetValue(csRegKey & "Ants\" & sKey, "Port", "80"), Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "WebUsername", My.Computer.Registry.GetValue(csRegKey & "Ants\" & sKey, "WebUsername", "root"), Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "WebPassword", My.Computer.Registry.GetValue(csRegKey & "Ants\" & sKey, "WebPassword", "root"), Microsoft.Win32.RegistryValueKind.String)

                                    'If sKey.Substring(0, 2) = "S2" Then
                                    '    sTemp = "admin"
                                    'Else
                                    '    sTemp = "root"
                                    'End If

                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "SSHUsername", My.Computer.Registry.GetValue(csRegKey & "Ants\" & sKey, "SSHUsername", "root"), Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "SSHPassword", My.Computer.Registry.GetValue(csRegKey & "Ants\" & sKey, "SSHPassword", sTemp), Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "Active", "Y", Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "APIPort", "4028", Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "SSHPort", "22", Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "ID", x.ToString, Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "UseAPI", "Y", Microsoft.Win32.RegistryValueKind.String)
                                    'My.Computer.Registry.SetValue(csRegKey & "\AntsV2\" & x, "RebootViaSSH", "Y", Microsoft.Win32.RegistryValueKind.String)
                                End If
                            End If
                        Next
                    End If
                End Using
            End If
        End Using

        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\AntsV2")
            For Each sKey As String In key.GetSubKeyNames
                dr = Me.dsAntConfig.Tables(0).NewRow

                dr("Name") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "Name", "")
                dr("Type") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "Type", "")
                dr("IPAddress") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "IPAddress", "")
                dr("Active") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "Active", "")

                If dr("Active") = "Y" Then
                    iAntsEnabled += 1
                End If

                dr("APIPort") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "APIPort", "")
                dr("HTTPPort") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "HTTPPort", "")
                dr("SSHPort") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "SSHPort", "")
                dr("ID") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "ID", "")
                dr("SSHPassword") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "SSHPassword", "")
                dr("SSHUsername") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "SSHUsername", "")
                dr("WebUsername") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "WebUsername", "")
                dr("WebPassword") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "WebPassword", "")
                dr("UseAPI") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "UseAPI", "")
                dr("RebootViaSSH") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "RebootViaSSH", "")

                dsAntConfig.Tables(0).Rows.Add(dr)
            Next
        End Using

        Call SetGridSizes("\Columns\dataAntConfig", Me.dataAntConfig)
        Call SetGridColumnPositions("\Columns\" & Me.dataAntConfig.Name & "_DisplayIndex", Me.dataAntConfig)

        AddHandler Me.dataAntConfig.ColumnWidthChanged, AddressOf Me.dataGrid_ColumnWidthChanged
        AddHandler Me.dataAntConfig.ColumnDisplayIndexChanged, AddressOf Me.dataAnts_ColumnDisplayIndexChanged

        Call CalcRefreshRate()

        'start up web browser controls
        wb(0) = New WebBrowser
        AddHandler wb(0).DocumentCompleted, AddressOf Me.wb_completed
        wb(0).Name = "0"

        wb(1) = New WebBrowser
        AddHandler wb(1).DocumentCompleted, AddressOf Me.wb_completed
        wb(1).Name = "1"

        wb(2) = New WebBrowser
        AddHandler wb(2).DocumentCompleted, AddressOf Me.wb_completed
        wb(2).Name = "2"

        'initialize workers
        Array.Resize(ThreadHandlers, Me.trackThreadCount.Value)
        Array.Resize(workerThread, Me.trackThreadCount.Value)

        For x = 0 To ThreadHandlers.Length - 2
            ThreadHandlers(x) = New clsThreadHandler

            workerThread(x) = New System.Threading.Thread(AddressOf HandleWork)
            workerThread(x).Name = "WorkerThread" & x
            workerThread(x).Start(x)
        Next

        ThreadHandlers(ThreadHandlers.Length - 1) = New clsThreadHandler
        workerThread(ThreadHandlers.Length - 1) = New System.Threading.Thread(AddressOf CheckForWork)
        workerThread(ThreadHandlers.Length - 1).Name = "ThreadDispatcher" & x
        workerThread(ThreadHandlers.Length - 1).Start()

        Me.timerDoStuff.Enabled = True

        'startup if ready
        If iAntsEnabled = 0 Then
            MsgBox("Please add some active Ant addresses first." & vbCrLf & vbCrLf & "You can also use the scan feature to auto detect your Ants.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

            Me.TabControl1.SelectTab(1)

            Exit Sub
        Else
            Call cmdRefresh_Click(sender, e)

            Me.TimerRefresh.Enabled = True
        End If

    End Sub

    Private Function AddOrSaveAnt(ByVal ID As Integer, ByVal sAntName As String, ByVal AntType As enAntType, ByVal sIPAddress As String, _
                          Optional ByVal sActive As String = "Y", Optional ByVal sHTTPPort As String = "80", _
                          Optional ByVal sWebUserName As String = "root", Optional ByVal sWebPassword As String = "root", _
                          Optional ByVal sSSHUserName As String = "root", Optional ByVal sSSHPassword As String = "", _
                          Optional ByVal sAPIPort As String = "4028", Optional ByVal sSSHPort As String = "22", _
                          Optional ByVal sUseAPI As String = "Y", Optional ByVal sRebootViaSSH As String = "Y") As Integer

        Dim x As Integer

        Try
            If ID = -1 Then 'adding a new one
                x = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2", "Count", 0)
            Else
                x = ID
            End If

            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "Name", sAntName, Microsoft.Win32.RegistryValueKind.String)

            Select Case AntType
                Case enAntType.S1
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "Type", "S1", Microsoft.Win32.RegistryValueKind.String)

                Case enAntType.S2
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "Type", "S2", Microsoft.Win32.RegistryValueKind.String)

                Case enAntType.S3
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "Type", "S3", Microsoft.Win32.RegistryValueKind.String)

            End Select

            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "IPAddress", sIPAddress, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "HTTPPort", sHTTPPort, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "WebUsername", sWebUserName, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "WebPassword", sWebPassword, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "SSHUsername", sSSHUserName, Microsoft.Win32.RegistryValueKind.String)

            If sSSHPassword.IsNullOrEmpty = True Then
                If AntType = enAntType.S2 Then
                    sSSHPassword = "admin"
                Else
                    sSSHPassword = "root"
                End If
            End If

            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "SSHPassword", sSSHPassword, Microsoft.Win32.RegistryValueKind.String)

            If sActive = "Y" Then
                If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "Active", "") <> "Y" Then
                    iAntsEnabled += 1
                End If
            ElseIf sActive = "N" And ID <> -1 Then
                iAntsEnabled -= 1
            End If

            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "Active", sActive, Microsoft.Win32.RegistryValueKind.String)

            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "APIPort", sAPIPort, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "SSHPort", sSSHPort, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "ID", x.ToString, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "UseAPI", sUseAPI, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "RebootViaSSH", sRebootViaSSH, Microsoft.Win32.RegistryValueKind.String)

            If ID = -1 Then 'adding an ant, need to increment the counter
                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2", "Count", x + 1)

                Return x
            Else
                Return ID
            End If
        Catch ex As Exception When bErrorHandle = True
            AddToLogQueue("Error occurred when trying to add Ant " & sAntName & ": " & ex.Message)
        End Try

    End Function

    'this is the thread dispatcher
    'it runs on its own thread, waiting for Ants to check
    'then it hands the work out to the worker threads as they are available
    Private Sub CheckForWork()

        While bShutDown = False
            While antsToCheckQueue.Count = 0 AndAlso bShutDown = False
                System.Threading.Thread.Sleep(10)
            End While

            If bShutDown = True Then
                Exit While
            End If

            SyncLock antsToCheckLock
                For x As Integer = 0 To workerThread.Count - 2
                    If ThreadHandlers(x).bBusy = False AndAlso ThreadHandlers(x).bGotWork = False Then
                        ThreadHandlers(x).bBusy = True
                        ThreadHandlers(x).AntToCheck = antsToCheckQueue.Dequeue
                        ThreadHandlers(x).bGotWork = True

                        If antsToCheckQueue.Count = 0 Then
                            Exit For
                        End If
                    End If
                Next
            End SyncLock
        End While

    End Sub

    'this is the worker code.  each worker thread runs here, waiting for work to do
    Private Sub HandleWork(ByVal iThread As Integer)

        While bShutDown = False
            While ThreadHandlers(iThread).bGotWork = False AndAlso bShutDown = False
                System.Threading.Thread.Sleep(10)
            End While

            ThreadHandlers(iThread).bBusy = True

#If DEBUG Then
            AddToLogQueue("Thread " & iThread & ": " & ThreadHandlers(iThread).AntToCheck.sName)
#End If

            Call GetAntDataViaAPI(ThreadHandlers(iThread).AntToCheck)

            ThreadHandlers(iThread).bGotWork = False
            ThreadHandlers(iThread).bBusy = False
        End While

    End Sub

    Private Function FindAntConfig(ByVal ID As Integer) As DataRow

        For Each dr As DataRow In Me.dsAntConfig.Tables(0).Rows
            If dr("ID").ToString = ID Then
                Return dr
            End If
        Next

        'should never happen
        AddToLogQueue("Ant " & ID & " not found in config!")

        Return Nothing

    End Function

    Private Function FindDisplayAntByID(ByVal ID As Integer) As DataRow

        Dim dr As DataRow

        For Each dr In ds.Tables(0).Rows
            If dr.Item("ID") = ID Then
                Return dr

                Exit For
            End If
        Next

        'not found in the list of ants in the output, make a new one
        dr = ds.Tables(0).NewRow
        dr("ID") = -1

        Return dr

    End Function

    'triggered by the browser control when it's done loading a web page
    Private Sub wb_completed(sender As Object, e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs)

        Dim dr As DataRow
        Dim x, y, z As Integer
        'Dim sAnt As String
        'Dim bAntFound As Boolean
        Dim wb As WebBrowser
        Dim sbTemp As System.Text.StringBuilder
        Dim count(0 To 9) As Integer
        'Dim sWebUN, sWebPW As String
        Dim sIP As String
        Dim s(), p() As String
        'Dim sFullAntName, sTemp As String
        Dim antConfigRow As DataRow

        Try
            wb = sender

            sbTemp = New System.Text.StringBuilder

            'first slash slash
            x = InStr(wb.Url.AbsoluteUri, "/")

            'second slash, should be after address
            x = InStr(x + 2, wb.Url.AbsoluteUri, "/")

            sIP = wb.Url.AbsoluteUri.Substring(7, x - 8)

            While y < x
                z = InStr(y + 1, wb.Url.AbsoluteUri.Substring(0, x), ".")

                If z = 0 Then
                    If y = 0 Then
                        y = InStr(wb.Url.AbsoluteUri, "//") + 1
                    End If

                    Exit While
                Else
                    y = z
                End If
            End While

            s = wb.Url.AbsoluteUri.Split(".")
            p = wb.Url.AbsoluteUri.Substring(6).Split(":")

            antConfigRow = wb.Tag

            'sFullAntName = antConfigRow("Name")

            'If p.Count = 2 Then
            '    sAnt = s(2) & "." & s(3).Substring(0, InStr(s(3), "/") - 1) & ":" & p(1)
            'Else
            '    sAnt = s(2) & "." & s(3).Substring(0, InStr(s(3), "/") - 1) & ":" & "80"
            'End If

            If wb.Document.All(1).OuterHtml.ToLower.Contains("authorization") Then
                AddToLogQueue(antConfigRow("Type") & ":" & antConfigRow("Name") & " responded with login page")

                'Call GetWebCredentials(sFullAntName, sWebUN, sWebPW)

                wb.Document.All("username").SetAttribute("value", antConfigRow("WebUsername"))
                wb.Document.All("password").SetAttribute("value", antConfigRow("WebPassword"))
                wb.Document.All(48).InvokeMember("click")

                Exit Sub
            Else
#If DEBUG Then
                For x = 0 To wb.Document.All.Count - 1
                    Debug.Print(x & " -- " & wb.Document.All(x).OuterText)
                    Debug.Print(x & " -- " & wb.Document.All(x).OuterHtml)
                Next
#End If
                If wb.Url.AbsoluteUri.Contains("minerStatus.cgi") Then
                    'sAnt = "S2:" & sAnt

                    dr = FindDisplayAntByID(antConfigRow("ID"))

                    'For Each dr In ds.Tables(0).Rows
                    '    If dr.Item("ID") = antConfigRow("ID") Then
                    '        bAntFound = True

                    '        Exit For
                    '    End If
                    'Next

                    'If bAntFound = False Then
                    '    dr = ds.Tables(0).NewRow
                    'End If

                    dr.Item("Name") = antConfigRow("Name")
                    'dr.Item("IPAddress") = "S2: " & sIP

                    'S2 status code
                    AddToLogQueue(antConfigRow("Type") & ":" & antConfigRow("Name") & " responded with status page")

                    dr.Item("Uptime") = wb.Document.All(88).OuterText
                    dr.Item("GH/s(5s)") = wb.Document.All(91).OuterText
                    dr.Item("GH/s(avg)") = wb.Document.All(94).OuterText
                    dr.Item("Blocks") = wb.Document.All(97).OuterText
                    dr.Item("HWE%") = Format(UInt64.Parse(wb.Document.All(109).OuterText) / _
                            (UInt64.Parse(wb.Document.All(127).OuterText) + UInt64.Parse(wb.Document.All(130).OuterText) + UInt64.Parse(wb.Document.All(109).OuterText)), "##0.###%")
                    dr.Item("BestShare") = Format(UInt64.Parse(wb.Document.All(137).OuterText), "###,###,###,###,###,##0")

                    Select Case wb.Document.All(193).OuterText
                        Case "Alive"
                            sbTemp.Append("U")

                        Case "Dead"
                            sbTemp.Append("D")

                    End Select

                    If wb.Document.All.Count > 224 Then
                        Select Case wb.Document.All(245).OuterText
                            Case "Alive"
                                sbTemp.Append("U")

                            Case "Dead"
                                sbTemp.Append("D")

                        End Select

                        Select Case wb.Document.All(297).OuterText
                            Case "Alive"
                                sbTemp.Append("U")

                            Case "Dead"
                                sbTemp.Append("D")

                        End Select

                        dr.Item("Pools") = sbTemp.ToString

                        sbTemp.Clear()

                        dr.Item("Diff") = wb.Document.All(215).OuterText & " " & wb.Document.All(267).OuterText & " " & wb.Document.All(319).OuterText

                        dr.Item("Rej%") = Format(Val(wb.Document.All(107).OuterText) / (Val(wb.Document.All(104).OuterText) + Val(wb.Document.All(107).OuterText)) * 100, "##0.###")

                        dr.Item("Stale%") = Format(Val(wb.Document.All(119).OuterText) / (Val(wb.Document.All(104).OuterText) + Val(wb.Document.All(119).OuterText)) * 100, "##0.###")

                        dr.Item("HFan") = GetHighValue(wb.Document.All(530).OuterText, wb.Document.All(531).OuterText, wb.Document.All(532).OuterText, wb.Document.All(533).OuterText)

                        dr.Item("Fans") = wb.Document.All(530).OuterText & " " & wb.Document.All(531).OuterText & " " & wb.Document.All(532).OuterText & " " & wb.Document.All(533).OuterText

                        dr.Item("Freq") = Val(wb.Document.All(366).OuterText)

                        dr.Item("HTemp") = GetHighValue(wb.Document.All(369).OuterText, wb.Document.All(385).OuterText, wb.Document.All(401).OuterText, wb.Document.All(417).OuterText, _
                                                        wb.Document.All(433).OuterText, wb.Document.All(449).OuterText, wb.Document.All(465).OuterText, wb.Document.All(481).OuterText, _
                                                        wb.Document.All(497).OuterText, wb.Document.All(513).OuterText)

                        dr.Item("Temps") = wb.Document.All(369).OuterText & " " & wb.Document.All(385).OuterText & " " & wb.Document.All(401).OuterText & " " & wb.Document.All(417).OuterText & " " & _
                                           wb.Document.All(433).OuterText & " " & wb.Document.All(449).OuterText & " " & wb.Document.All(465).OuterText & " " & wb.Document.All(481).OuterText & " " & _
                                           wb.Document.All(497).OuterText & " " & wb.Document.All(513).OuterText

                        count(0) = HowManyInString(wb.Document.All(372).OuterText, "x") + HowManyInString(wb.Document.All(372).OuterText, "-")
                        count(1) = HowManyInString(wb.Document.All(388).OuterText, "x") + HowManyInString(wb.Document.All(388).OuterText, "-")
                        count(2) = HowManyInString(wb.Document.All(404).OuterText, "x") + HowManyInString(wb.Document.All(404).OuterText, "-")
                        count(3) = HowManyInString(wb.Document.All(420).OuterText, "x") + HowManyInString(wb.Document.All(420).OuterText, "-")
                        count(4) = HowManyInString(wb.Document.All(436).OuterText, "x") + HowManyInString(wb.Document.All(436).OuterText, "-")
                        count(5) = HowManyInString(wb.Document.All(452).OuterText, "x") + HowManyInString(wb.Document.All(452).OuterText, "-")
                        count(6) = HowManyInString(wb.Document.All(468).OuterText, "x") + HowManyInString(wb.Document.All(468).OuterText, "-")
                        count(7) = HowManyInString(wb.Document.All(484).OuterText, "x") + HowManyInString(wb.Document.All(484).OuterText, "-")
                        count(8) = HowManyInString(wb.Document.All(500).OuterText, "x") + HowManyInString(wb.Document.All(500).OuterText, "-")
                        count(9) = HowManyInString(wb.Document.All(516).OuterText, "x") + HowManyInString(wb.Document.All(516).OuterText, "-")

                        dr.Item("XCount") = count(0) + count(1) + count(2) + count(3) + count(4) + count(5) + count(6) + count(7) + count(8) + count(9) & "X"

                        dr.Item("Status") = count(0) & "X " & count(1) & "X " & count(2) & "X " & count(3) & "X " & count(4) & "X " & count(5) & "X " & _
                                            count(6) & "X " & count(7) & "X " & count(8) & "X " & count(9) & "X"

                        If (count(0) <> 0 OrElse count(1) <> 0 OrElse count(2) <> 0 OrElse count(3) <> 0 OrElse count(4) <> 0 OrElse count(5) <> 0 _
                            OrElse count(6) <> 0 OrElse count(7) <> 0 OrElse count(8) <> 0 OrElse count(9) <> 0) AndAlso Me.chkAlertRebootIfXd.Checked = True Then
                            'only reboot once every x minutes
                            Call RebootAnt(antConfigRow, False, False, wb)
                        End If
                    End If
                ElseIf wb.Url.AbsoluteUri.Contains("/reboot.html") = True Then
                    'S2 reboot
                    wb.Document.All(66).InvokeMember("click")

                    Call wbFinished(wb)

                    Exit Sub
                ElseIf wb.Url.AbsoluteUri.Contains("/admin/status/minerstatus/") = True Then
                    'S1/S3 status code    
                    'sAnt = sFullAntName.Substring(0, 3) & sAnt

                    AddToLogQueue(antConfigRow("Type") & ":" & antConfigRow("Name") & " responded with status page")

                    dr = FindDisplayAntByID(antConfigRow("ID"))

                    'For Each dr In ds.Tables(0).Rows
                    '    If dr.Item("ID") = antConfigRow("ID") Then
                    '        bAntFound = True

                    '        Exit For
                    '    End If
                    'Next

                    'If bAntFound = False Then
                    '    dr = ds.Tables(0).NewRow
                    'End If

                    dr.Item("Name") = antConfigRow("Name")
                    'dr.Item("IPAddress") = antConfigRow("IP")

                    If wb.Url.AbsoluteUri.Contains("minerstatus") AndAlso wb.Document.All.Count > 75 Then
                        dr.Item("Uptime") = wb.Document.All(122).OuterText.TrimEnd

                        If wb.Document.All(84).Children(2).Children.Count <> 1 Then
                            dr.Item("GH/s(5s)") = wb.Document.All(126).OuterText.TrimEnd
                            dr.Item("GH/s(avg)") = wb.Document.All(130).OuterText.TrimEnd
                            dr.Item("Blocks") = wb.Document.All(134).OuterText.TrimEnd
                            dr.Item("HWE%") = Format(UInt64.Parse(wb.Document.All(150).OuterText.TrimEnd.Replace(",", "")) / _
                                             (UInt64.Parse(wb.Document.All(174).OuterText.TrimEnd.Replace(",", "")) + _
                                              UInt64.Parse(wb.Document.All(178).OuterText.TrimEnd.Replace(",", "")) + _
                                              UInt64.Parse(wb.Document.All(150).OuterText.TrimEnd.Replace(",", ""))), "##0.###%")
                            dr.Item("BestShare") = wb.Document.All(186).OuterText.TrimEnd

                            Select Case wb.Document.All(247).OuterText.TrimEnd
                                Case "Alive"
                                    sbTemp.Append("U")

                                Case "Dead"
                                    sbTemp.Append("D")

                            End Select

                            If wb.Document.All(192).Children(2).Children(0).Children(0).Children.Count > 3 Then
                                Select Case wb.Document.All(192).Children(2).Children(0).Children(0).Children(3).Children(3).Children(0).OuterText.TrimEnd
                                    Case "Alive"
                                        sbTemp.Append("U")

                                    Case "Dead"
                                        sbTemp.Append("D")

                                End Select

                                If wb.Document.All(192).Children(2).Children(0).Children(0).Children.Count > 4 Then
                                    '3 pools
                                    Select Case wb.Document.All(192).Children(2).Children(0).Children(0).Children(4).Children(3).Children(0).OuterText.TrimEnd
                                        Case "Alive"
                                            sbTemp.Append("U")

                                        Case "Dead"
                                            sbTemp.Append("D")

                                    End Select

                                    x = 443

                                    dr.Item("Diff") = wb.Document.All(276).OuterText.Trim & " " & wb.Document.All(345).OuterText.Trim & " " & wb.Document.All(414).OuterText.Trim
                                Else
                                    'two pools
                                    sbTemp.Append("N")

                                    dr.Item("Diff") = wb.Document.All(276).OuterText.Trim & " " & wb.Document.All(345).OuterText.Trim

                                    x = 374
                                End If
                            Else
                                'one pool
                                sbTemp.Append("NN")

                                dr.Item("Diff") = wb.Document.All(276).OuterText.Trim

                                x = 305
                            End If
                            dr.Item("Pools") = sbTemp.ToString

                            sbTemp.Clear()



                            dr.Item("Rej%") = Format(Val(wb.Document.All(147).OuterText.Replace(",", "")) / (Val(wb.Document.All(143).OuterText.Replace(",", "")) + Val(wb.Document.All(147).OuterText.Replace(",", ""))) * 100, "##0.###")

                            dr.Item("Stale%") = Format(Val(wb.Document.All(163).OuterText.Replace(",", "")) / (Val(wb.Document.All(143).OuterText.Replace(",", "")) + Val(wb.Document.All(163).OuterText.Replace(",", ""))) * 100, "##0.###")

                            dr.Item("HFan") = GetHighValue(wb.Document.All(x + 33).OuterText.TrimEnd, wb.Document.All(x + 58).OuterText.TrimEnd)

                            dr.Item("Fans") = wb.Document.All(x + 33).OuterText.TrimEnd & " " & _
                                              wb.Document.All(x + 58).OuterText.TrimEnd

                            dr.Item("HTemp") = GetHighValue(wb.Document.All(x + 37).OuterText.TrimEnd, wb.Document.All(x + 62).OuterText.TrimEnd)

                            dr.Item("Temps") = wb.Document.All(x + 37).OuterText.TrimEnd & " " & _
                                               wb.Document.All(x + 62).OuterText.TrimEnd

                            dr.Item("Freq") = Val(wb.Document.All(x + 29).OuterText.TrimEnd)

                            count(0) = HowManyInString(wb.Document.All(x + 41).OuterText.TrimEnd, "x") + HowManyInString(wb.Document.All(x + 41).OuterText.TrimEnd, "-")
                            count(1) = HowManyInString(wb.Document.All(x + 66).OuterText.TrimEnd, "x") + HowManyInString(wb.Document.All(x + 66).OuterText.TrimEnd, "-")

                            dr.Item("XCount") = count(0) + count(1) & "X"

                            dr.Item("Status") = count(0) & "X " & count(1) & "X"
                        End If

                        If (count(0) <> 0 OrElse count(1) <> 0) AndAlso Me.chkAlertRebootIfXd.Checked = True Then
                            Call RebootAnt(antConfigRow, False, False, wb)
                        End If
                    End If
                Else
                    Exit Sub
                End If

                If dr("ID") = -1 Then
                    dr("ID") = antConfigRow("ID")
                    ds.Tables(0).Rows.Add(dr)
                End If

                Call HandleAlerts(dr, antConfigRow, wb)

                Call wbFinished(wb)

                Call RefreshTitle()
            End If
        Catch ex As Exception When bErrorHandle = True
            If dr IsNot Nothing Then
                dr.Item("Name") = "ERR"
                dr.Item("Uptime") = "SEE LOG"
            End If

            AddToLogQueue("An error occurred when parsing the web output for " & wb.Url.AbsoluteUri & ": " & ex.Message)

            Call wbFinished(wb)
        End Try

    End Sub

    Private Sub RebootViaReboot(ByRef antConfigRow As DataRow, ByRef wb As WebBrowser)

        'Dim sWebUN, sWebPW As String

        If TryGovernor(RebootInfo, antConfigRow("ID"), Me.cmbAlertRebootGovernor, Me.txtAlertRebootGovernor, 60 * 30) = True Then
            AddToLogQueue("REBOOTING " & antConfigRow("Name"))

            Select Case antConfigRow("Type")
                Case "S1", "S3"
                    wb.Navigate("http://" & antConfigRow("IP") & ":" & antConfigRow("HTTPPort") & "/cgi-bin/luci/;stok=/admin/system/reboot?reboot=1")

                Case "S2"
                    wb.Navigate(String.Format("http://{0}:{1}@" & antConfigRow("IP") & ":" & antConfigRow("HTTPPort") & "/reboot.html", antConfigRow("WebUsername"), antConfigRow("WebPassword"), Nothing, Nothing, GetHeader))

            End Select
        Else
            AddToLogQueue("Need to reboot " & antConfigRow("Name") & " but it hasn't been long enough since last reboot")
        End If

    End Sub

    Private Sub wbFinished(ByRef wb As WebBrowser)

        Select Case wb.Name
            Case "0"
                wbData(0).IsBusy = False
                AddToLogQueue("Browser instance 0 done")

            Case "1"
                wbData(1).IsBusy = False
                AddToLogQueue("Browser instance 1 done")

            Case "2"
                wbData(2).IsBusy = False
                AddToLogQueue("Browser instance 2 done")

        End Select
    End Sub

    Private Function GetHighValue(ByVal s1 As String, ByVal s2 As String, Optional ByVal s3 As String = "", Optional ByVal s4 As String = "", Optional ByVal s5 As String = "", _
                                  Optional ByVal s6 As String = "", Optional ByVal s7 As String = "", Optional ByVal s8 As String = "", Optional ByVal s9 As String = "", _
                                  Optional ByVal s10 As String = "") As Integer

        Dim h As Integer

        If Val(s1) > Val(s2) Then
            h = Val(s1)
        Else
            h = Val(s2)
        End If

        If s3.IsNullOrEmpty = False Then
            If Val(s3) > h Then
                h = Val(s3)
            End If

            If s4.IsNullOrEmpty = False Then
                If Val(s4) > h Then
                    h = Val(s4)
                End If

                If s5.IsNullOrEmpty = False Then
                    If Val(s5) > h Then
                        h = Val(s5)
                    End If

                    If s6.IsNullOrEmpty = False Then
                        If Val(s6) > h Then
                            h = Val(s6)
                        End If

                        If s7.IsNullOrEmpty = False Then
                            If Val(s7) > h Then
                                h = Val(s7)
                            End If

                            If s8.IsNullOrEmpty = False Then
                                If Val(s8) > h Then
                                    h = Val(s8)
                                End If

                                If s9.IsNullOrEmpty = False Then
                                    If Val(s9) > h Then
                                        h = Val(s9)
                                    End If

                                    If s10.IsNullOrEmpty = False Then
                                        If Val(s10) > h Then
                                            h = Val(s10)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Return h

    End Function

    Private Function HowManyInString(ByVal sString As String, sSearch As String) As Integer

        Dim i, x As Integer

        For x = 0 To sString.Length - 1
            If sString.Substring(x, 1).ToLower = sSearch.ToLower Then
                i += 1
            End If
        Next

        Return i

    End Function

    Private Sub TimerRefresh_Tick(sender As Object, e As System.EventArgs) Handles TimerRefresh.Tick

        'Dim AntData As clsAntRefreshData
        Dim antConfig As stAntConfig
        Dim x As Integer

        Static dRefreshTime As Date
        Static bRefresh As Boolean
        Static bStarted As Boolean

        iCountDown -= 1

        If iCountDown < 0 Then
            iCountDown = iRefreshRate
        End If

        If bRefresh = True AndAlso dRefreshTime.AddSeconds(iDisplayRefreshPeriod) < Now Then
            bRefresh = False
            Me.dataAnts.Refreshing = False

            Me.dataAnts.Refresh()
        ElseIf bRefresh = False Then
            Me.dataAnts.Refresh()
        End If

        'watchdog timer of sorts of web browsers
        'they have 2 minutes to finish up, otherwise they're marked as available
        If wbData(0).IsBusy = True AndAlso wbData(0).StartTime.AddMinutes(2) < Now Then
            wbData(0).IsBusy = False
        End If

        If wbData(1).IsBusy = True AndAlso wbData(1).StartTime.AddMinutes(2) < Now Then
            wbData(1).IsBusy = False
        End If

        If wbData(2).IsBusy = True AndAlso wbData(2).StartTime.AddMinutes(2) < Now Then
            wbData(2).IsBusy = False
        End If

        If iCountDown = 0 Then
            Me.cmdPause.Enabled = False

            'clear the uptime column to indicate we're refreshing
            For Each dr As DataRow In Me.ds.Tables(0).Rows
                dr.Item("UpTime") = "???"
            Next

            If Me.txtLog.TextLength > 1500000 Then
                Me.txtLog.Text = Me.txtLog.Text.Substring(InStr(Me.txtLog.TextLength - 1000000, Me.txtLog.Text, vbCrLf) + 1)
            End If

            AddToLogQueue("Initiated Ant refresh")

            dRefreshTime = Now
            bRefresh = True

            If bStarted = True Then
                Me.dataAnts.Refreshing = True
            Else
                bStarted = True
            End If

            For Each dr As DataRow In Me.dsAntConfig.Tables(0).Rows
                If dr("Active") = "Y" Then
                    If dr("UseAPI") = "Y" Then
                        antConfig = GetAntConfigByConfigRow(dr)

                        SyncLock antsToCheckLock
                            antsToCheckQueue.Enqueue(antConfig)
                        End SyncLock
                    Else
                        'wait for one of the 3 browsers to become available
                        While wbData(0).IsBusy = True AndAlso wbData(1).IsBusy = True AndAlso wbData(2).IsBusy = True
                            My.Application.DoEvents()
                        End While

                        'browser logic
                        'Call GetWebCredentials(AntData.sAnt, sWebUN, sWebPW)

                        If wbData(0).IsBusy = False Then
                            x = 0
                        ElseIf wbData(1).IsBusy = False Then
                            x = 1
                        ElseIf wbData(2).IsBusy = False Then
                            x = 2
                        Else
                            x = 100
                        End If

                        If x <> 100 Then
                            wbData(x).IsBusy = True
                            wbData(x).StartTime = Now

                            wb(x).Tag = dr

                            Select Case dr("Type")
                                Case "S1", "S3"
                                    wb(x).Navigate("http://" & dr("IPAddress") & ":" & dr("HTTPPort") & "/cgi-bin/luci/;stok=/admin/status/minerstatus/", False)

                                Case "S2"
                                    wb(x).Navigate(String.Format("http://{0}:{1}@" & dr("IPAddress") & ":" & dr("HTTPPort") & "/cgi-bin/minerStatus.cgi", dr("WebUsername"), dr("WebPassword")), Nothing, Nothing, GetHeader)

                            End Select

                            AddToLogQueue("Submitted " & dr("Name") & " on web browser instance " & x)
                        End If

                        'Call RefreshGrid(dr)
                    End If
                End If
            Next

            iCountDown = iRefreshRate

            Me.cmdPause.Enabled = True
        End If

        Me.cmdRefresh.Text = "Refreshing in " & iCountDown

    End Sub

    Private Function GetAntConfigByConfigRow(ByRef dr As DataRow) As stAntConfig

        Dim antConfig As stAntConfig

        antConfig = New stAntConfig

        Select Case dr("Type")
            Case "S1"
                antConfig.AntType = enAntType.S1

            Case "S2"
                antConfig.AntType = enAntType.S2

            Case "S3"
                antConfig.AntType = enAntType.S3

        End Select

        antConfig.sName = dr("Name")
        antConfig.ID = dr("ID")
        antConfig.sAPIPort = dr("APIPort")
        antConfig.sHTTPPort = dr("HTTPPort")
        antConfig.sIP = dr("IPAddress")
        antConfig.sSSHPassword = dr("SSHPassword")
        antConfig.sSSHPort = dr("SSHPort")
        antConfig.sSSHUsername = dr("SSHUsername")
        antConfig.sWebPassword = dr("WebPassword")
        antConfig.sWebUsername = dr("WebUsername")

        Return antConfig

    End Function

    Private Sub cmdRefresh_Click(sender As System.Object, e As System.EventArgs) Handles cmdRefresh.Click

        If Me.dataAntConfig.Rows.Count = 0 Then
            MsgBox("Please add some Ant addresses first.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

            Me.TabControl1.SelectTab(1)

            Exit Sub
        Else
            Me.timerDoStuff.Enabled = True

            iCountDown = 1

            Call TimerRefresh_Tick(sender, e)

            Me.TimerRefresh.Enabled = True

            Me.TabControl1.SelectTab(0)

        End If

    End Sub

    Private Function GetHeader() As String

        Return "Authorization: Basic " & Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(Me.txtAntWebUsername.Text & ":" & Me.txtAntWebPassword.Text)) & System.Environment.NewLine

    End Function

    'refresh by API
    'this runs on the UI thread
    Private Sub RefreshGrid(ByRef AntData As clsAntRefreshData)

        'Dim bAntFound As Boolean
        Dim AntConfig As DataRow
        Dim dr As DataRow
        Dim j, jp1 As Newtonsoft.Json.Linq.JObject
        Dim ja As Newtonsoft.Json.Linq.JArray
        Dim ts As TimeSpan
        Dim sbTemp, sbTemp2 As System.Text.StringBuilder
        Dim count(0 To 9), iTemp As Integer
        Dim dBestShare As Double
        Dim x As Integer
        Dim bStep As Byte
        Dim pd As clsPoolData
        Dim pdl As System.Collections.Generic.List(Of clsPoolData)
        Dim Lock As New Object

        sbTemp = New System.Text.StringBuilder

        Debug.Print("Refreshing " & AntData.sAntIP)

        Try
            dr = FindDisplayAntByID(AntData.ID)

            AntConfig = FindAntConfig(AntData.ID)

            'For Each dr In ds.Tables(0).Rows
            '    If dr.Item("Name") = AntData.sAnt Then
            '        bAntFound = True

            '        Exit For
            '    End If
            'Next

            'If bAntFound = False Then
            '    dr = ds.Tables(0).NewRow
            'End If

            dr.Item("Name") = AntConfig("Name")
            'dr.Item("IPAddress") = AntData.sAntIP

            bStep = 1

            j = Newtonsoft.Json.Linq.JObject.Parse(AntData.sStats)

            For Each ja In j.Property("STATS")
                If AntData.AntType = enAntType.S3 Then
                    If ja.Count = 4 Then
                        jp1 = ja(0)
                    Else
                        jp1 = ja(1)
                    End If
                Else
                    jp1 = ja(0)
                End If

                ts = New TimeSpan(0, 0, jp1.Value(Of Integer)("Elapsed"))

                dr.Item("Uptime") = Format(ts.Days, "0d") & " " & Format(ts.Hours, "0h") & " " & Format(ts.Minutes, "0m") & " " & Format(ts.Seconds, "0s")
                dr.Item("HWE%") = jp1.Value(Of String)("Device Hardware%") & "%"

                dr.Item("HFan") = GetHighValue(jp1.Value(Of Integer)("fan1"), jp1.Value(Of Integer)("fan2"), jp1.Value(Of Integer)("fan3"), jp1.Value(Of Integer)("fan4"))

                sbTemp.Clear()

                For x = 1 To jp1.Value(Of Integer)("fan_num")
                    sbTemp.Append(jp1.Value(Of Integer)("fan" & x))

                    If x <> jp1.Value(Of Integer)("fan_num") Then
                        sbTemp.Append(" ")
                    End If
                Next

                dr.Item("Fans") = sbTemp.ToString

                sbTemp.Clear()

                iTemp = 0

                For x = 1 To jp1.Value(Of Integer)("temp_num")
                    sbTemp.Append(jp1.Value(Of Integer)("temp" & x))

                    If jp1.Value(Of Integer)("temp" & x) > iTemp Then
                        iTemp = jp1.Value(Of Integer)("temp" & x)
                    End If

                    If x <> jp1.Value(Of Integer)("temp_num") Then
                        sbTemp.Append(" ")
                    End If
                Next

                dr.Item("HTemp") = iTemp

                dr.Item("Temps") = sbTemp.ToString

                dr.Item("Freq") = Val(jp1.Value(Of String)("frequency"))

                count(0) = HowManyInString(jp1.Value(Of String)("chain_acs1"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs1"), "x")
                count(1) = HowManyInString(jp1.Value(Of String)("chain_acs2"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs2"), "x")

                If AntData.AntType = enAntType.S2 Then
                    count(2) = HowManyInString(jp1.Value(Of String)("chain_acs3"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs3"), "x")
                    count(3) = HowManyInString(jp1.Value(Of String)("chain_acs4"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs4"), "x")
                    count(4) = HowManyInString(jp1.Value(Of String)("chain_acs5"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs5"), "x")
                    count(5) = HowManyInString(jp1.Value(Of String)("chain_acs6"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs6"), "x")
                    count(6) = HowManyInString(jp1.Value(Of String)("chain_acs7"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs7"), "x")
                    count(7) = HowManyInString(jp1.Value(Of String)("chain_acs8"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs8"), "x")
                    count(8) = HowManyInString(jp1.Value(Of String)("chain_acs9"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs9"), "x")
                    count(9) = HowManyInString(jp1.Value(Of String)("chain_acs10"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs10"), "x")
                Else
                    count(2) = 0
                    count(3) = 0
                    count(4) = 0
                    count(5) = 0
                    count(6) = 0
                    count(7) = 0
                    count(8) = 0
                    count(9) = 0
                End If

                dr.Item("XCount") = count(0) + count(1) + count(2) + count(3) + count(4) + count(5) + count(6) + count(7) + count(8) + count(9) & "X"

                If AntData.AntType = enAntType.S2 Then
                    dr.Item("Status") = count(0) & "X " & count(1) & "X " & count(2) & "X " & count(3) & "X " & count(4) & "X " & count(5) & "X " & _
                                        count(6) & "X " & count(7) & "X " & count(8) & "X " & count(9) & "X"
                Else
                    dr.Item("Status") = count(0) & "X " & count(1) & "X "
                End If

                Exit For
            Next

            bStep = 2

            j = Newtonsoft.Json.Linq.JObject.Parse(AntData.sSummary)

            For Each ja In j.Property("SUMMARY")
                For Each jp1 In ja
                    dr.Item("GH/s(5s)") = Val(jp1.Value(Of String)("GHS 5s"))
                    dr.Item("GH/s(avg)") = Val(jp1.Value(Of String)("GHS av"))

                    dr.Item("Rej%") = jp1.Value(Of String)("Pool Rejected%")
                    dr.Item("Stale%") = Format(jp1.Value(Of Integer)("Stale") / (jp1.Value(Of Integer)("Accepted") + jp1.Value(Of Integer)("Stale")) * 100, "##0.###")

                    dr.Item("Blocks") = jp1.Value(Of String)("Found Blocks")
                Next
            Next

            bStep = 3

            j = Newtonsoft.Json.Linq.JObject.Parse(AntData.sPools)

            dBestShare = 0

            sbTemp.Clear()

            sbTemp2 = New System.Text.StringBuilder

            If IsDBNull(dr.Item("PoolData2")) = True Then
                dr.Item("PoolData2") = New System.Collections.Generic.List(Of clsPoolData)
            End If

            pdl = dr.Item("PoolData2")
            pd = New clsPoolData

            For Each ja In j.Property("POOLS")
                For Each jp1 In ja
                    If jp1.Value(Of Double)("Best Share") > dBestShare Then
                        dBestShare = jp1.Value(Of Double)("Best Share")
                    End If

                    Select Case jp1.Value(Of String)("Status")
                        Case "Alive"
                            If sbTemp.ToString.Contains("U") = False Then
                                dr.Item("Diff") = Format(jp1.Value(Of Double)("Last Share Difficulty"), "#,###,###")
                            End If

                            sbTemp.Append("U")

                        Case "Dead"
                            sbTemp.Append("D")

                        Case Else
                            sbTemp.Append("U")

                    End Select

                    If sbTemp2.Length <> 0 Then
                        sbTemp2.Append(vbCrLf)
                    End If

                    sbTemp2.Append(jp1.Value(Of String)("POOL") & ": " & jp1.Value(Of String)("URL") & " (" & jp1.Value(Of String)("User") & ") " & jp1.Value(Of String)("Status"))

                    pd.URL = jp1.Value(Of String)("URL")
                    pd.UID = jp1.Value(Of String)("User")

                    pdl.Add(pd)
                Next

                Exit For
            Next

            dr.Item("BestShare") = Format(dBestShare, "###,###,###,###,###,##0")
            dr.Item("Pools") = sbTemp.ToString
            dr.Item("PoolData") = sbTemp2.ToString

            If dr("ID") = -1 Then
                dr("ID") = AntConfig("ID")

                ds.Tables(0).Rows.Add(dr)
            End If

            Call HandleAlerts(dr, AntConfig, Nothing)

            Call RefreshTitle()
        Catch ex As Exception When bErrorHandle = True
            dr.Item("Uptime") = "ERROR"

            AddToLogQueue("ERROR when querying " & AntConfig("Name") & " (step " & bStep & "): " & ex.Message)

            If Me.chkRebootAntOnError.Checked = True Then
                AddToLogQueue("Attempting to reboot " & AntConfig("Name") & " via the web because the API query errored out.")

                Call RebootAnt(AntConfig, False, YNtoBoolean(AntConfig("RebootViaSSH")), Nothing)
            End If
        End Try

    End Sub

    ''initiates refresh via the browser controls
    'Private Sub RefreshGrid(ByRef AntConfigRow As DataRow)


    'End Sub

    'this is what does the work to get the data from the Ants via the API
    'it then passes it back to the UI thread for display
    Private Sub GetAntDataViaAPI(ByRef AntToCheck As stAntConfig)

        Dim sTemp As String
        Dim x As Integer
        Dim bStep As Byte
        'Dim s() As String
        Dim AntData As clsAntRefreshData

        AntData = New clsAntRefreshData

        If bShutDown = True Then Exit Sub

        Try
            AntData.AntType = AntToCheck.AntType
            'Select Case AntToCheck.AntType
            '    Case
            '        AntData.AntType = enAntType.S3

            '    Case "S2"
            '        AntData.AntType = enAntType.S2

            '    Case "S1"
            '        AntData.AntType = enAntType.S1

            '    Case Else
            '        Throw New Exception("Unknown ant type.")

            'End Select

            'sAntIP = sAntToCheck.Substring(4)

            'If sAntIP.Contains(".") Then
            '    s = sAntIP.Split(".")

            '    sAnt = sAntToCheck.Substring(0, 2) & ":" & s(2) & "." & s(3)
            'Else
            '    sAnt = sAntIP
            'End If

            'AntData.sAnt = sAnt
            'AntData.sAntIP = sAntToCheck

            bStep = 1

            sTemp = GetIPData(AntToCheck.sIP, AntToCheck.sAPIPort, "stats")

            If AntData.AntType = enAntType.S3 OrElse AntData.AntType = enAntType.S1 Then
                'fix mangled JSON
                x = InStr(sTemp, "}{")

                If x <> 0 Then
                    AntData.sStats = sTemp.Insert(x, ",")
                Else
                    AntData.sStats = sTemp
                End If
            Else
                AntData.sStats = sTemp
            End If

            bStep = 2

            AntData.sSummary = GetIPData(AntToCheck.sIP, AntToCheck.sAPIPort, "summary")

            bStep = 3

            AntData.sPools = GetIPData(AntToCheck.sIP, AntToCheck.sAPIPort, "pools")

            AntData.ID = AntToCheck.ID
        Catch ex As Exception
            AntData.bError = True
            AntData.ex = ex

            AddToLogQueue("ERROR when querying " & AntToCheck.sIP & " (step " & bStep & "): " & ex.Message)
        End Try

        SyncLock AntRefreshLock
            AntRefreshDataQueue.Enqueue(AntData)
        End SyncLock

    End Sub

    'Private Sub GetWebCredentials(ByVal sAnt As String, ByRef sUsername As String, ByRef sPassword As String)

    '    sAnt = RemoveAntPort(sAnt)

    '    'Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\Ants\" & sAnt)
    '    '    If key Is Nothing Then
    '    '        My.Computer.Registry.CurrentUser.CreateSubKey(csRegKey & "\Ants\" & sAnt)
    '    '    End If
    '    'End Using

    '    Try
    '        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\Ants\" & sAnt, True)
    '            sUsername = key.GetValue("WebUsername")

    '            If sUsername.IsNullOrEmpty = True Then
    '                key.SetValue("WebUsername", "root")
    '                sUsername = "root"
    '            End If

    '            sPassword = key.GetValue("WebPassword")

    '            If sPassword.IsNullOrEmpty = True Then
    '                key.SetValue("WebPassword", "root")
    '                sPassword = "root"
    '            End If
    '        End Using
    '    Catch ex As Exception When bErrorHandle = True
    '        sPassword = "root"
    '        sUsername = "root"

    '        AddToLogQueue("ERROR in GetWebCredentials (assuming default credentials): " & ex.Message)
    '    End Try

    'End Sub

    'Private Sub GetSSHCredentials(ByVal sAnt As String, ByRef sUsername As String, ByRef sPassword As String)

    '    sAnt = RemoveAntPort(sAnt)

    '    'Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\Ants\" & sAnt)
    '    '    If key Is Nothing Then
    '    '        My.Computer.Registry.CurrentUser.CreateSubKey(csRegKey & "\Ants\" & sAnt)
    '    '    End If
    '    'End Using

    '    Try
    '        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\Ants\" & sAnt, True)
    '            sUsername = key.GetValue("SSHUsername")

    '            If sUsername.IsNullOrEmpty = True Then
    '                key.SetValue("SSHUsername", "root")
    '                sUsername = "root"
    '            End If

    '            sPassword = key.GetValue("SSHPassword")

    '            If sPassword.IsNullOrEmpty = True Then
    '                Select Case sAnt.Substring(0, 2)
    '                    Case "S1", "S3"
    '                        key.SetValue("SSHPassword", "root")
    '                        sPassword = "root"

    '                    Case "S2"
    '                        key.SetValue("SSHPassword", "admin")
    '                        sPassword = "admin"

    '                End Select
    '            End If

    '        End Using
    '    Catch ex As Exception When bErrorHandle = True
    '        Select Case sAnt.Substring(0, 2)
    '            Case "S1", "S3"
    '                sPassword = "root"

    '            Case "S2"
    '                sPassword = "admin"

    '        End Select

    '        sUsername = "root"

    '        AddToLogQueue("ERROR in GetSSHCredentials (assuming default credentials): " & ex.Message)
    '    End Try

    'End Sub

    Private Sub HandleAlerts(ByRef drAnt As DataRow, ByRef drAntConfig As DataRow, ByRef wb As WebBrowser)

        Dim x As Integer
        Dim dr As DataGridViewRow
        Dim iAlertCount, iAntAlertCount As Integer
        Dim bStep As Byte
        Dim colHighlightColumns As System.Collections.Generic.List(Of Integer)
        Dim bFound As Boolean

        'alert logic
        Try
            For Each dr In Me.dataAnts.Rows
                If dr.Cells("ID").Value.ToString = drAnt.Item("ID").ToString Then
                    bFound = True

                    Exit For
                End If
            Next

            If bFound = False Then
                AddToLogQueue("Ant " & drAnt("ID") & " not found in HandleAlerts!")

                Exit Sub
            End If

            If IsDBNull(dr.Cells("Uptime").Value) = False AndAlso dr.Cells("Uptime").Value <> "ERROR" AndAlso dr.Cells("Uptime").Value <> "???" Then
                iAntAlertCount = 0

                If dr.Tag Is Nothing Then
                    dr.Tag = New System.Collections.Generic.List(Of Integer)
                End If

                colHighlightColumns = dr.Tag
                colHighlightColumns.Clear()

                Select Case drAntConfig("Type")
                    Case "S1"
                        If Me.chkAlertIfS1Temp.Checked = True Then
                            bStep = 1

                            x = Val(Me.txtAlertS1Temp.Text)

                            If x > 0 Then
                                If Integer.Parse(dr.Cells("HTemp").Value) >= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("HTemp").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " celcius", "S1 Temp Alert")
                                End If
                            End If
                        End If

                        If Me.chkAlertIfS1FanHigh.Checked = True Then
                            bStep = 2

                            x = Val(Me.txtAlertS1FanHigh.Text)

                            If x > 0 Then
                                If Integer.Parse(dr.Cells("HFan").Value) >= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("HFan").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " RPM", "S1 Fan Alert")
                                End If
                            End If
                        End If

                        If Me.chkAlertIfS1FanLow.Checked = True Then
                            bStep = 3

                            x = Val(Me.txtAlertS1FanLow.Text)

                            If Integer.Parse(dr.Cells("HFan").Value) <= x Then
                                iAntAlertCount += 1

                                colHighlightColumns.Add(dr.Cells("HFan").ColumnIndex)

                                Call ProcessAlerts(dr, dr.Cells("Name").Value & " is below " & x & " RPM", "S1 Fan Alert")
                            End If
                        End If

                        If Me.chkAlertIfS1Hash.Checked = True Then
                            bStep = 4

                            x = Val(Me.txtAlertS1Hash.Text)

                            If x > 0 Then
                                If Val(dr.Cells("GH/s(avg)").Value) <= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("GH/s(avg)").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " less than " & x & " GH/s", "S1 Hash Alert")

                                    If Me.chkAlertRebootAntsOnHashAlert.Checked = True Then
                                        If drAntConfig("RebootViaSSH") = "Y" Then
                                            Call RebootAnt(drAntConfig, False, True, Nothing)
                                        Else
                                            Call RebootAnt(drAntConfig, False, False, wb)
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        If Me.chkAlertIfS1XCount.Checked = True Then
                            bStep = 5

                            x = Val(Me.txtAlertS1XCount.Text)

                            If x > 0 Then
                                If Integer.Parse(dr.Cells("XCount").Value.ToString.LeftMost(1)) >= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("XCount").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " X count", "S1 XCount Alert")

                                    'use SSH only if using the API, as the web code has its own reboot logic
                                    If Me.chkAlertRebootIfXd.Checked = True Then
                                        If drAntConfig("RebootViaSSH") = "Y" Then
                                            Call RebootAnt(drAntConfig, False, True, Nothing)
                                        Else
                                            Call RebootAnt(drAntConfig, False, False, wb)
                                        End If
                                    End If
                                End If
                            End If
                        End If

                    Case "S2"
                        If Me.chkAlertIfS2Temp.Checked = True Then
                            bStep = 6

                            x = Val(Me.txtAlertS2Temp.Text)

                            If x > 0 Then
                                If Integer.Parse(dr.Cells("HTemp").Value) >= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("HTemp").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " celcius", "S2 Temp Alert")
                                End If
                            End If
                        End If

                        If Me.chkAlertIfS2FanHigh.Checked = True Then
                            bStep = 7

                            x = Val(Me.txtAlertS2FanHigh.Text)

                            If x > 0 Then
                                If Integer.Parse(dr.Cells("HFan").Value) >= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("HFan").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " RPM", "S2 Fan Alert")
                                End If
                            End If
                        End If

                        If Me.chkAlertIfS2FanLow.Checked = True Then
                            bStep = 8

                            x = Val(Me.txtAlertS2FanLow.Text)

                            If Integer.Parse(dr.Cells("HFan").Value) <= x Then
                                iAntAlertCount += 1

                                colHighlightColumns.Add(dr.Cells("HFan").ColumnIndex)

                                Call ProcessAlerts(dr, dr.Cells("Name").Value & " is below " & x & " RPM", "S2 Fan Alert")
                            End If

                        End If

                        If Me.chkAlertIfS2Hash.Checked = True Then
                            bStep = 9

                            x = Val(Me.txtAlertS2Hash.Text)

                            If x > 0 Then
                                If Val(dr.Cells("GH/s(avg)").Value) <= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("GH/s(avg)").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " less than " & x & " GH/s", "S2 Hash Alert")

                                    If Me.chkAlertRebootAntsOnHashAlert.Checked = True Then
                                        If drAntConfig("RebootViaSSH") = "Y" Then
                                            Call RebootAnt(drAntConfig, False, True, Nothing)
                                        Else
                                            Call RebootAnt(drAntConfig, False, False, wb)
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        If Me.chkAlertIfS2XCount.Checked = True Then
                            bStep = 10

                            x = Val(Me.txtAlertS2XCount.Text)

                            If x > 0 Then
                                If Integer.Parse(dr.Cells("XCount").Value.ToString.LeftMost(1)) >= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("XCount").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " X count", "S2 XCount Alert")

                                    'use SSH only if using the API, as the web code has its own reboot logic
                                    If Me.chkAlertRebootIfXd.Checked = True Then
                                        If drAntConfig("RebootViaSSH") = "Y" Then
                                            Call RebootAnt(drAntConfig, False, True, Nothing)
                                        Else
                                            Call RebootAnt(drAntConfig, False, False, wb)
                                        End If
                                    End If
                                End If
                            End If
                        End If

                    Case "S3"
                        If Me.chkAlertIfS3Temp.Checked = True Then
                            bStep = 1

                            x = Val(Me.txtAlertS3Temp.Text)

                            If x > 0 Then
                                If Integer.Parse(dr.Cells("HTemp").Value) >= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("HTemp").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " celcius", "S3 Temp Alert")
                                End If
                            End If
                        End If

                        If Me.chkAlertIfS3FanHigh.Checked = True Then
                            bStep = 2

                            x = Val(Me.txtAlertS3FanHigh.Text)

                            If x > 0 Then
                                If Integer.Parse(dr.Cells("HFan").Value) >= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("HFan").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " RPM", "S3 Fan Alert")
                                End If
                            End If
                        End If

                        If Me.chkAlertIfS3FanLow.Checked = True Then
                            bStep = 3

                            x = Val(Me.txtAlertS3FanLow.Text)

                            If Integer.Parse(dr.Cells("HFan").Value) <= x Then
                                iAntAlertCount += 1

                                colHighlightColumns.Add(dr.Cells("HFan").ColumnIndex)

                                Call ProcessAlerts(dr, dr.Cells("Name").Value & " is below " & x & " RPM", "S3 Fan Alert")
                            End If
                        End If

                        If Me.chkAlertIfS3Hash.Checked = True Then
                            bStep = 4

                            x = Val(Me.txtAlertS3Hash.Text)

                            If x > 0 Then
                                If Val(dr.Cells("GH/s(avg)").Value) <= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("GH/s(avg)").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " less than " & x & " GH/s", "S3 Hash Alert")

                                    If Me.chkAlertRebootAntsOnHashAlert.Checked = True Then
                                        If drAntConfig("RebootViaSSH") = "Y" Then
                                            Call RebootAnt(drAntConfig, False, True, Nothing)
                                        Else
                                            Call RebootAnt(drAntConfig, False, False, wb)
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        If Me.chkAlertIfS3XCount.Checked = True Then
                            bStep = 5

                            x = Val(Me.txtAlertS3XCount.Text)

                            If x > 0 Then
                                If Integer.Parse(dr.Cells("XCount").Value.ToString.LeftMost(1)) >= x Then
                                    iAntAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("XCount").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " X count", "S3 XCount Alert")

                                    'use SSH only if using the API, as the web code has its own reboot logic
                                    If Me.chkAlertRebootIfXd.Checked = True Then
                                        If drAntConfig("RebootViaSSH") = "Y" Then
                                            Call RebootAnt(drAntConfig, False, True, Nothing)
                                        Else
                                            Call RebootAnt(drAntConfig, False, False, wb)
                                        End If
                                    End If
                                End If
                            End If
                        End If

                End Select

                dr.Cells("ACount").Value = iAntAlertCount

                If dr.Tag IsNot Nothing Then
                    colHighlightColumns = dr.Tag

                    dr.Cells("HTemp").Style.BackColor = New Color
                    dr.Cells("HFan").Style.BackColor = New Color
                    dr.Cells("GH/s(avg)").Style.BackColor = New Color
                    dr.Cells("XCount").Style.BackColor = New Color

                    For Each x In colHighlightColumns
                        dr.Cells(x).Style.BackColor = Color.Red
                    Next
                End If
            End If

            iAlertCount = 0

            For Each dr In Me.dataAnts.Rows
                If IsDBNull(dr.Cells("Uptime").Value) = False AndAlso dr.Cells("Uptime").Value <> "ERROR" AndAlso dr.Cells("Uptime").Value <> "???" Then
                    iAlertCount += dr.Cells("ACount").Value
                End If
            Next

            If iAlertCount <> 0 Then
                sAlerts = " !!! " & iAlertCount & " ALERTS !!!"
            Else
                sAlerts = ""
            End If
        Catch ex As Exception
            AddToLogQueue("ERROR when checking alerts on " & dr.Cells("Name").Value & " (step " & bStep & "): " & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Returns the seconds value of a drop down/text box combo for a governor rate
    ''' </summary>
    ''' <param name="dictList">The dictionary the data is in</param>
    ''' <param name="sKey">The key to search for in the dictionary</param>
    ''' <param name="cmbValueType">Seconds, Minutes, Hours, Days</param>
    ''' <param name="txtValue">Actual value, ie, 60</param>
    ''' <param name="iDefault">Which value to use if the end result is zero</param>
    ''' <returns>True if the action needs to be performed</returns>
    ''' <remarks>Will update the dictionary as necessary</remarks>
    Private Function TryGovernor(ByRef dictList As Dictionary(Of String, Date), ByVal sKey As String, ByVal cmbValueType As ComboBox, ByVal txtValue As TextBox, ByVal iDefault As Integer) As Boolean

        Dim iResult As Integer
        Dim dLastDate As Date

        If dictList.TryGetValue(sKey, dLastDate) = True Then
            Select Case cmbValueType.Text
                Case "Seconds"
                    iResult = Val(txtValue.Text)

                Case "Minutes"
                    iResult = Val(txtValue.Text) * 60

                Case "Hours"
                    iResult = Val(txtValue.Text) * 60 * 60

                Case "Days"
                    iResult = Val(txtValue.Text) * 60 * 60 * 24

            End Select

            If iResult = 0 Then
                iResult = iDefault
            End If

            If dLastDate.AddSeconds(iResult) < Now Then
                dictList(sKey) = Now

                Return True
            Else
                Return False
            End If
        Else
            dictList.Add(sKey, Now)

            Return True
        End If

    End Function

    'called by any routine to attempt to reboot an Ant, based on the governor
    Private Sub RebootAnt(ByRef AntConfigRow As DataRow, ByVal bRebootNow As Boolean, ByVal bUseSSH As Boolean, ByRef wbToUse As WebBrowser)

        Dim t As Threading.Thread
        Dim x As Integer

        If TryGovernor(RebootInfo, AntConfigRow("ID"), Me.cmbAlertRebootGovernor, Me.txtAlertRebootGovernor, 30 * 60) = True Then
            bRebootNow = True
        Else
            If bRebootNow = False Then
                AddToLogQueue("Need to reboot " & AntConfigRow("Name") & " but it hasn't been long enough since last reboot")
            End If
        End If

        If bRebootNow = True Then
            If bUseSSH = True Then
                t = New Threading.Thread(AddressOf Me._RebootAntBySSH)

                AddToLogQueue("REBOOTING " & AntConfigRow("Name") & " via SSH")

                t.Start(GetAntConfigByConfigRow(AntConfigRow))
            Else
                AddToLogQueue("REBOOTING " & AntConfigRow("Name") & " via Web")

                If wbToUse Is Nothing Then
                    While wbData(0).IsBusy = True AndAlso wbData(1).IsBusy = True AndAlso wbData(2).IsBusy = True
                        My.Application.DoEvents()
                    End While

                    'browser logic
                    'Call GetWebCredentials(AntData.sAnt, sWebUN, sWebPW)

                    If wbData(0).IsBusy = False Then
                        x = 0
                    ElseIf wbData(1).IsBusy = False Then
                        x = 1
                    ElseIf wbData(2).IsBusy = False Then
                        x = 2
                    Else
                        x = 100
                    End If

                    If x <> 100 Then
                        wbData(x).IsBusy = True
                        wbData(x).StartTime = Now

                        wbToUse = wb(x)
                        wbToUse.Tag = AntConfigRow
                    End If
                End If

                Select Case AntConfigRow("Type")
                    Case "S1", "S3"
                        wbToUse.Navigate("http://" & AntConfigRow("IPAddress") & ":" & AntConfigRow("HTTPPort") & "/cgi-bin/luci/;stok=/admin/system/reboot?reboot=1")

                    Case "S2"
                        wbToUse.Navigate(String.Format("http://{0}:{1}@" & AntConfigRow("IPAddress") & ":" & AntConfigRow("HTTPPort") & "/reboot.html", AntConfigRow("WebUsername"), AntConfigRow("WebPassword"), Nothing, Nothing, GetHeader))

                End Select
            End If
        End If

    End Sub

    'does the actual rebooting on a separate thread
    Private Sub _RebootAntBySSH(ByVal AntConfig As stAntConfig)

        Dim ssh As Renci.SshNet.SshClient
        Dim sshCommand As Renci.SshNet.SshCommand
        'Dim sUN, sPW As String

        Try
            'sAnt = RemoveAntPort(sAnt)

            'Call GetSSHCredentials(sAnt, sUN, sPW)

            ssh = New Renci.SshNet.SshClient(AntConfig.sIP, AntConfig.sSSHPort, AntConfig.sSSHUsername, AntConfig.sSSHPassword)
            ssh.Connect()

            sshCommand = ssh.CreateCommand("/sbin/reboot")
            sshCommand.Execute()

            If sshCommand.Error.IsNullOrEmpty = False Then
                AddToLogQueue("Reboot of " & AntConfig.sName & " appears to have failed: " & sshCommand.Error)
            Else
                AddToLogQueue("Reboot of " & AntConfig.sName & " appears to have succeeded")
            End If

            ssh.Disconnect()
            ssh.Dispose()

            sshCommand.Dispose()
        Catch ex As Exception
            AddToLogQueue("Reboot of " & AntConfig.sName & " FAILED: " & ex.Message)
        End Try

    End Sub

    Private Sub ProcessAlerts(ByRef dr As DataGridViewRow, ByVal sAlertMsg As String, ByVal sAlertTitle As String)

        Dim ap As frmAnnoyingPopup
        Dim bStep As Byte

        Try
            bStep = 9

            'notify icon
            If Me.chkAlertShowNotifyPopup.Checked = True Then
                Me.NotifyIcon1.ShowBalloonTip(0, sAlertTitle, Now.ToString & vbCrLf & sAlertMsg, ToolTipIcon.Warning)
            End If

            'annoying popup
            If Me.chkAlertShowAnnoyingPopup.Checked = True Then
                ap = New frmAnnoyingPopup
                ap.Text = sAlertTitle
                ap.lblAlert.Text = Now.ToString & vbCrLf & sAlertMsg
                ap.Show()
            End If

            'launch process
            If Me.chkAlertStartProcess.Checked = True Then
                bStep = 10

                If Me.txtAlertStartProcessName.Text.IsNullOrEmpty = False Then
                    Try
                        If My.Computer.FileSystem.FileExists(Me.txtAlertStartProcessName.Text) = False Then
                            Me.NotifyIcon1.ShowBalloonTip(30000, "Error launching alert process!", "Error launching alert process!  The specified file to start does not seem to exist.", ToolTipIcon.Error)
                        Else
                            Process.Start(Me.txtAlertStartProcessName.Text, Replace(Me.txtAlertStartProcessParms.Text, "%A", dr.Cells("Name").Value))
                        End If
                    Catch ex As Exception
                        Me.NotifyIcon1.ShowBalloonTip(30000, "Error starting idle worker process!", "Error starting idle worker process!" & vbCrLf & vbCrLf & ex.Message, ToolTipIcon.Error)
                    End Try
                End If
            End If

            'email
            If Me.chkAlertSendEMail.Checked = True Then
                bStep = 11

                If TryGovernor(EMailAlertInfo, dr.Cells("ID").Value, Me.cmbAlertEMailGovernor, Me.txtAlertEMailGovernor, 10 * 30) = True Then
                    If Me.txtSMTPAlertSubject.Text.IsNullOrEmpty = True Then
                        Call SendEMail(sAlertMsg, sAlertTitle)
                    Else
                        Call SendEMail(sAlertMsg, Me.txtSMTPAlertSubject.Text)
                    End If
                End If
            End If

            Call AddToLogQueue("ALERT: " & sAlertMsg)
        Catch ex As Exception When bErrorHandle = True
            AddToLogQueue("ERROR when processing alerts on " & dr.Cells("Name").Value & " (step " & bStep & "): " & ex.Message)
        End Try

    End Sub

    Private Sub GetIPDataOnOtherThread()

        sIPDataResponse = GetIPData(sIPToCheck, "4028", "stats")

    End Sub

    'network code, used by the API querying routine
    Private Function GetIPData(ByVal sIP As String, ByVal sPort As String, ByVal sCommand As String) As String

        Dim socket As System.Net.Sockets.TcpClient
        Dim s As System.IO.Stream
        Dim b() As Byte
        Dim sbTemp As System.Text.StringBuilder
        Dim d As Date
        'Dim p() As String
        Dim a() As String

        Try
            socket = New System.Net.Sockets.TcpClient

            ' = sIP.Split(":")
            a = sIP.Split(".")

            sbTemp = New System.Text.StringBuilder

            'strip out leading zeros from addresses.  for some reason 192.168.0.90 works, but 192.168.000.090 doesn't
            For Each sTemp As String In a
                If sbTemp.Length <> 0 Then
                    sbTemp.Append(".")
                End If

                sbTemp.Append(Byte.Parse(sTemp).ToString)
            Next

            socket.Connect(sbTemp.ToString, sPort)
            s = socket.GetStream

            b = System.Text.Encoding.ASCII.GetBytes("{""command"":""" & sCommand & """}" & vbCrLf)

            s.Write(b, 0, b.Length)

            sbTemp.Clear()

            d = Now

            While (sbTemp.Length < 2 OrElse sbTemp.ToString.Substring(sbTemp.Length - 2, 1) <> "}") AndAlso d.AddMinutes(1) > Now
                My.Application.DoEvents()
                System.Threading.Thread.Sleep(100)

                If socket.Available <> 0 Then
                    Array.Resize(b, socket.Available)
                    s.Read(b, 0, b.Length)

                    sbTemp.Append(System.Text.Encoding.ASCII.GetString(b))
                End If
            End While

            s.Close()
            socket.Close()

            Return sbTemp.ToString
        Catch ex As Exception
            AddToLogQueue("ERROR when accessing API on " & sIP & ": " & ex.Message)
        End Try

    End Function

    'saves new column widths 
    Private Sub dataGrid_ColumnWidthChanged(sender As Object, e As System.Windows.Forms.DataGridViewColumnEventArgs)

        Dim dt As DataGridView

        dt = DirectCast(sender, DataGridView)

        With My.Computer.Registry
            .CurrentUser.CreateSubKey(csRegKey & "\Columns\" & dt.Name)
            .SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Columns\" & dt.Name, e.Column.Name, e.Column.Width, Microsoft.Win32.RegistryValueKind.DWord)
        End With

    End Sub

    Private Sub SetGridSizes(ByVal sKey As String, ByRef dataGrid As DataGridView)

        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & sKey)
            If key IsNot Nothing Then
                For Each colAny As DataGridViewColumn In dataGrid.Columns
                    If key.GetValue(colAny.Name) <> 0 Then
                        colAny.Width = key.GetValue(colAny.Name)
                    End If
                Next

                key.Close()
            End If
        End Using

    End Sub

    Private Sub SetGridColumnPositions(ByVal sKey As String, ByRef datagrid As DataGridView)

        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & sKey)
            If key IsNot Nothing Then
                For Each colAny As DataGridViewColumn In datagrid.Columns
                    If key.GetValue(colAny.Name) <> 0 Then
                        colAny.DisplayIndex = key.GetValue(colAny.Name)
                    End If
                Next

                key.Close()
            End If
        End Using

    End Sub

    Private Sub frmMain_ResizeEnd(sender As Object, e As System.EventArgs) Handles Me.ResizeEnd

        With My.Computer.Registry
            .CurrentUser.CreateSubKey(csRegKey)
            .SetValue("HKEY_CURRENT_USER\" & csRegKey, "Width", Me.Width, Microsoft.Win32.RegistryValueKind.DWord)
            .SetValue("HKEY_CURRENT_USER\" & csRegKey, "Height", Me.Height, Microsoft.Win32.RegistryValueKind.DWord)
        End With

    End Sub

    Private Sub cmdScan_Click(sender As System.Object, e As System.EventArgs) Handles cmdScan.Click

        'Dim sResponse, sLocalNet As String
        Dim x, y As Integer
        'Dim wc As eWebClient
        'Dim sResponse As String
        Dim t As Threading.Thread
        Dim dStart As Date
        Dim j, jp1 As Newtonsoft.Json.Linq.JObject
        Dim ja As Newtonsoft.Json.Linq.JArray
        Dim AntType As enAntType
        Dim sAnt As String
        Dim bFound As Boolean

        Static bStopRequested As Boolean

        Try
            If Me.cmdScan.Text = "Scan" Then
                If Me.txtIPRangeToScan.Text.IsNullOrEmpty Then
                    MsgBox("Please enter the IP range to scan first.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                    Me.txtIPRangeToScan.Focus()

                    Exit Sub
                End If

                Me.cmdScan.Text = "STOP!"

                Me.Cursor = Cursors.WaitCursor

                Me.ProgressBar1.Visible = True

                For x = Val(Me.cmbAntScanStart.Text) To Val(Me.cmbAntScanStop.Text)
                    If bStopRequested = True Then Exit For

                    sIPDataResponse = ""

                    Me.ProgressBar1.Value = x

                    Me.ToolTip1.SetToolTip(Me.ProgressBar1, Me.txtIPRangeToScan.Text & "." & x)

                    t = New Threading.Thread(AddressOf Me.GetIPDataOnOtherThread)

                    sIPToCheck = Me.txtIPRangeToScan.Text & "." & x

                    t.Start()

                    dStart = Now

                    While sIPDataResponse.IsNullOrEmpty = True AndAlso dStart.AddSeconds(5) > Now
                        My.Application.DoEvents()
                    End While

                    If sIPDataResponse.IsNullOrEmpty = True Then
                        If t.IsAlive Then t.Abort()

                        Debug.Print("Scan connect failed: " & x)
                    Else
                        Try
                            y = InStr(sIPDataResponse, """Type"":""S3""}{""STATS")

                            If y <> 0 AndAlso sIPDataResponse.Substring(y + 11, 1) = "{" Then
                                sIPDataResponse = sIPDataResponse.Insert(y + 11, ",")
                            End If

                            j = Newtonsoft.Json.Linq.JObject.Parse(sIPDataResponse)

                            AntType = 0

                            For Each ja In j.Property("STATS")
                                jp1 = ja(0)

                                Select Case jp1.Value(Of String)("ID")
                                    Case "BTM0"
                                        'probably S2
                                        If HowManyInString(jp1.Value(Of String)("chain_acs1"), " ") = 8 Then
                                            AntType = enAntType.S2

                                            sAnt = "S2: " & sIPToCheck
                                        End If

                                    Case "BMM0"
                                        'probably S3
                                        If HowManyInString(jp1.Value(Of String)("chain_acs1"), " ") = 1 Then
                                            AntType = enAntType.S3

                                            sAnt = "S3: " & sIPToCheck
                                        End If

                                    Case "ANT0"
                                        'probably S1
                                        If HowManyInString(jp1.Value(Of String)("chain_acs1"), " ") = 3 Then
                                            AntType = enAntType.S1

                                            sAnt = "S1: " & sIPToCheck
                                        End If

                                    Case ""
                                        'might be an S3 on second node of the array
                                        jp1 = ja(1)

                                        Select Case jp1.Value(Of String)("ID")
                                            Case "BTM0"
                                                'probably S2
                                                If HowManyInString(jp1.Value(Of String)("chain_acs1"), " ") = 8 Then
                                                    AntType = enAntType.S2

                                                    sAnt = "S2: " & sIPToCheck
                                                End If

                                            Case "BMM0"
                                                'probably S3
                                                If HowManyInString(jp1.Value(Of String)("chain_acs1"), " ") = 2 Then
                                                    AntType = enAntType.S3

                                                    sAnt = "S3: " & sIPToCheck
                                                End If

                                            Case "ANT0"
                                                'probably S1
                                                If HowManyInString(jp1.Value(Of String)("chain_acs1"), " ") = 3 Then
                                                    AntType = enAntType.S1

                                                    sAnt = "S1: " & sIPToCheck
                                                End If

                                        End Select
                                End Select
                            Next

                            If AntType <> 0 Then
                                For Each dr As DataRow In Me.dsAntConfig.Tables(0).Rows
                                    If dr("IPAddress") = sIPToCheck Then
                                        bFound = True

                                        Exit For
                                    End If
                                Next

                                If bFound = False Then
                                    Me.txtAntAddress.Text = sIPToCheck
                                    Me.txtAntName.Text = ""

                                    Select Case AntType
                                        Case enAntType.S1
                                            Me.optAntS1.Checked = True

                                            AddToLogQueue("S1 found at " & sIPToCheck & "!")

                                        Case enAntType.S2
                                            Me.optAntS2.Checked = True

                                            AddToLogQueue("S2 found at " & sIPToCheck & "!")

                                        Case enAntType.S3
                                            Me.optAntS3.Checked = True

                                            AddToLogQueue("S3 found at " & sIPToCheck & "!")

                                    End Select

                                    Call AddOrSaveAntLogic(True, -1)
                                End If
                            End If
                        Catch ex As Exception
                            Debug.Print("Scan failed: " & x)
                        End Try
                    End If
                Next

                'sLocalNet = Me.cmbLocalIPs.Text.Substring(0, Microsoft.VisualBasic.InStrRev(Me.cmbLocalIPs.Text, "."))

                '    wc = New eWebClient
                '    wc.Credentials = New System.Net.NetworkCredential(Me.txtWebUsername.Text, Me.txtWebPassword.Text)

                '    Me.cmdScan.Text = "STOP!"

                '    Me.ProgressBar1.Minimum = 1
                '    Me.ProgressBar1.Maximum = 254
                '    Me.ProgressBar1.Visible = True
                '    My.Application.DoEvents()

                '    For x = 1 To 255
                '        If bStopRequested = True Then
                '            bStopRequested = False

                '            Me.cmdScan.Text = "Scan"

                '            Exit For
                '        End If

                '        Me.ProgressBar1.Value = x
                '        Me.ToolTip1.SetToolTip(Me.ProgressBar1, sLocalNet & x.ToString)

                '        If sLocalNet & x.ToString <> Me.cmbLocalIPs.Text Then
                '            Try
                '                Debug.Print(x)

                '                My.Application.DoEvents()

                '                sResponse = wc.DownloadString("http://" & sLocalNet & x.ToString)

                '                If sResponse.Contains("href=""/cgi-bin/luci"">LuCI - Lua Configuration Interface</a>") = True Then
                '                    wc.DownloadFile("http://" & sLocalNet & x.ToString & "/luci-static/resources/icons/antminer_logo.png", My.Computer.FileSystem.SpecialDirectories.Temp & "\ant.png")

                '                    My.Computer.FileSystem.DeleteFile(My.Computer.FileSystem.SpecialDirectories.Temp & "\ant.png")

                '                    Me.chklstAnts.SetItemChecked("S1: " & Me.chklstAnts.Items.Add(sLocalNet & x.ToString), True)

                '                    AddToLogQueue("S1 found at " & sLocalNet & x.ToString & "!")

                '                    My.Application.DoEvents()
                '                End If

                '                If sResponse.Contains("<tr><td width=""33%"">Miner Type</td><td id=""ant_minertype""></td></tr>") Then
                '                    wc.DownloadFile("http://" & sLocalNet & x.ToString & "/images/antminer_logo.png", My.Computer.FileSystem.SpecialDirectories.Temp & "\ant.png")

                '                    My.Computer.FileSystem.DeleteFile(My.Computer.FileSystem.SpecialDirectories.Temp & "\ant.png")

                '                    Me.chklstAnts.SetItemChecked("S2: " & Me.chklstAnts.Items.Add(sLocalNet & x.ToString), True)

                '                    Call AddToLogQueue("S2 found at " & sLocalNet & x.ToString & "!")

                '                    My.Application.DoEvents()
                '                End If

                '                Debug.Print(sResponse)
                '            Catch ex As Exception
                '            End Try
                '        End If
                '    Next
            Else
                bStopRequested = True
            End If
        Catch ex As Exception When bErrorHandle = True
            MsgBox("The following error has occurred:" & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly)
        Finally
            Me.ToolTip1.SetToolTip(Me.ProgressBar1, "")
            Me.ProgressBar1.Visible = False
            Me.cmdScan.Enabled = True
            Me.Cursor = Cursors.Default
            Me.cmdScan.Text = "Scan"
            'Me.lblScanning.Visible = False
        End Try

    End Sub

    'Private Class eWebClient

    '    Inherits System.Net.WebClient

    '    Protected Overrides Function GetWebRequest(address As System.Uri) As System.Net.WebRequest
    '        Dim w As System.Net.WebRequest

    '        w = MyBase.GetWebRequest(address)
    '        w.Timeout = 5000

    '        Return w
    '    End Function

    'End Class

    Private Sub cmdPause_Click(sender As System.Object, e As System.EventArgs) Handles cmdPause.Click

        Me.TimerRefresh.Enabled = Not Me.TimerRefresh.Enabled

        If Me.TimerRefresh.Enabled = True Then
            Me.cmdPause.Text = "Pause"
        Else
            Me.cmdPause.Text = "Resume"
        End If

    End Sub

    Private Sub cmdSaveConfig_Click(sender As System.Object, e As System.EventArgs) Handles cmdSaveConfig.Click

        With ctlsByKey
            '.SetRegKeyByControl(Me.chklstAnts)

            .SetRegKeyByControl(Me.txtRefreshRate)
            .SetRegKeyByControl(Me.cmbRefreshRate)

            .SetRegKeyByControl(Me.chkShowBestShare)
            .SetRegKeyByControl(Me.chkShowBlocks)
            .SetRegKeyByControl(Me.chkShowFans)
            .SetRegKeyByControl(Me.chkShowGHs5s)
            .SetRegKeyByControl(Me.chkShowGHsAvg)
            .SetRegKeyByControl(Me.chkShowHWE)
            .SetRegKeyByControl(Me.chkShowPools)
            .SetRegKeyByControl(Me.chkShowStatus)
            .SetRegKeyByControl(Me.chkShowTemps)
            .SetRegKeyByControl(Me.chkShowUptime)
            .SetRegKeyByControl(Me.chkShowFreqs)
            .SetRegKeyByControl(Me.chkShowHighTemp)
            .SetRegKeyByControl(Me.chkShowHighFan)
            .SetRegKeyByControl(Me.chkShowXCount)
            .SetRegKeyByControl(Me.chkShowRej)
            .SetRegKeyByControl(Me.chkShowStale)
            .SetRegKeyByControl(Me.chkShowDifficulty)
            .SetRegKeyByControl(Me.chkShowACount)

            .SetRegKeyByControl(Me.chkShowSelectionColumn)

            '.SetRegKeyByControl(Me.chkUseAPI)

            .SetRegKeyByControl(Me.trackThreadCount)
            .SetRegKeyByControl(Me.txtDisplayRefreshInSecs)
        End With

    End Sub

    Private Sub cmdAddAnt_Click(sender As Object, e As System.EventArgs) Handles cmdAddAnt.Click

        Call AddOrSaveAntLogic(True, -1)

    End Sub

    Private Function chkBoxToYN(ByRef chkAny As CheckBox) As String

        If chkAny.Checked = True Then
            Return "Y"
        Else
            Return "N"
        End If

    End Function

    Private Function YNtoBoolean(ByVal sValue As String) As Boolean

        If sValue = "Y" Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub AddOrSaveAntLogic(ByVal bAddNewAnt As Boolean, ByVal ID As Integer)

        'Dim sTemp As String
        Dim bAntFound As Boolean
        Dim AntType As enAntType
        Dim AntConfigRow As DataRow

        Try
            If Me.optAntS1.Checked = False AndAlso Me.optAntS2.Checked = False AndAlso Me.optAntS3.Checked = False Then
                MsgBox("Please specify if this is an S1, S2, or and S3.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                Exit Sub
            End If

            If Me.txtAntAddress.Text.IsNullOrEmpty = False Then
                Me.txtAntAddress.Text = Me.txtAntAddress.Text.ToLower.Replace("http://", "")

                If bAddNewAnt = True Then
                    For Each dr As DataRow In Me.dsAntConfig.Tables(0).Rows
                        If dr("IPAddress") = Me.txtAntAddress.Text Then
                            If dr("HTTPPort") = Me.txtAntWebPort.Text Then
                                If MsgBox("This address/port combination seems to already exist.  Are you sure you want to add it?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                                    bAntFound = True

                                    Exit For
                                End If
                            End If
                        End If
                    Next
                End If

                If bAntFound = False Then
                    If Me.txtAntName.Text.IsNullOrEmpty = True Then
                        If Me.optAntS1.Checked = True Then
                            Me.txtAntName.Text = "S1: " & Me.txtAntAddress.Text
                        ElseIf Me.optAntS2.Checked = True Then
                            Me.txtAntName.Text = "S2: " & Me.txtAntAddress.Text
                        ElseIf Me.optAntS3.Checked = True Then
                            Me.txtAntName.Text = "S3: " & Me.txtAntAddress.Text
                        End If
                    End If

                    If bAddNewAnt = True Then
                        AntConfigRow = dsAntConfig.Tables(0).NewRow
                    Else
                        AntConfigRow = FindAntConfig(ID)
                    End If

                    If Me.optAntS1.Checked = True Then
                        AntType = enAntType.S1
                        AntConfigRow("Type") = "S1"
                    ElseIf Me.optAntS2.Checked = True Then
                        AntType = enAntType.S2
                        AntConfigRow("Type") = "S2"
                    ElseIf Me.optAntS3.Checked = True Then
                        AntType = enAntType.S3
                        AntConfigRow("Type") = "S3"
                    End If

                    ID = AddOrSaveAnt(ID, Me.txtAntName.Text, AntType, Me.txtAntAddress.Text, chkBoxToYN(Me.chkAntActive), _
                                   Me.txtAntWebPort.Text, Me.txtAntWebUsername.Text, Me.txtAntWebPassword.Text, _
                                   Me.txtAntSSHUsername.Text, Me.txtAntSSHPassword.Text, Me.txtAntAPIPort.Text, _
                                   Me.txtAntSSHPort.Text, chkBoxToYN(Me.chkAntUseAPI), chkBoxToYN(Me.chkAntRebootViaSSH))

                    AntConfigRow("Name") = Me.txtAntName.Text
                    AntConfigRow("IPAddress") = Me.txtAntAddress.Text
                    AntConfigRow("Active") = chkBoxToYN(Me.chkAntActive)
                    AntConfigRow("HTTPPort") = Me.txtAntWebPort.Text
                    AntConfigRow("WebUsername") = Me.txtAntWebUsername.Text
                    AntConfigRow("WebPassword") = Me.txtAntWebPassword.Text
                    AntConfigRow("SSHPort") = Me.txtAntSSHPort.Text
                    AntConfigRow("SSHUsername") = Me.txtAntSSHUsername.Text
                    AntConfigRow("SSHPassword") = Me.txtAntSSHPassword.Text
                    AntConfigRow("UseAPI") = chkBoxToYN(Me.chkAntUseAPI)
                    AntConfigRow("APIPort") = Me.txtAntAPIPort.Text
                    AntConfigRow("RebootViaSSH") = chkBoxToYN(Me.chkAntRebootViaSSH)
                    AntConfigRow("ID") = ID

                    If bAddNewAnt = True Then
                        dsAntConfig.Tables(0).Rows.Add(AntConfigRow)
                    End If

                    'My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "Port", sPort, Microsoft.Win32.RegistryValueKind.String)

                    'If sTemp.Substring(0, 2) = "S1" OrElse sTemp.Substring(0, 2) = "S3" Then
                    '    If Me.txtAntWebUsername.Text.IsNullOrEmpty = True Then
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "WebUsername", "root", Microsoft.Win32.RegistryValueKind.String)
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "WebPassword", "root", Microsoft.Win32.RegistryValueKind.String)
                    '    Else
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "WebUsername", Me.txtAntWebUsername.Text, Microsoft.Win32.RegistryValueKind.String)
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "WebPassword", Me.txtAntWebPassword.Text, Microsoft.Win32.RegistryValueKind.String)
                    '    End If

                    '    If Me.txtAntSSHUsername.Text.IsNullOrEmpty = True Then
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "SSHUsername", "root", Microsoft.Win32.RegistryValueKind.String)
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "SSHPassword", "root", Microsoft.Win32.RegistryValueKind.String)
                    '    Else
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "SSHUsername", Me.txtAntSSHUsername.Text, Microsoft.Win32.RegistryValueKind.String)
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "SSHPassword", Me.txtAntSSHPassword.Text, Microsoft.Win32.RegistryValueKind.String)
                    '    End If
                    'End If

                    'If sTemp.Substring(0, 2) = "S2" Then
                    '    If Me.txtAntWebUsername.Text.IsNullOrEmpty = True Then
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "WebUsername", "root", Microsoft.Win32.RegistryValueKind.String)
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "WebPassword", "root", Microsoft.Win32.RegistryValueKind.String)
                    '    Else
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "WebUsername", Me.txtAntWebUsername.Text, Microsoft.Win32.RegistryValueKind.String)
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "WebPassword", Me.txtAntWebPassword.Text, Microsoft.Win32.RegistryValueKind.String)
                    '    End If

                    '    If Me.txtAntSSHUsername.Text.IsNullOrEmpty = True Then
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "SSHUsername", "root", Microsoft.Win32.RegistryValueKind.String)
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "SSHPassword", "admin", Microsoft.Win32.RegistryValueKind.String)
                    '    Else
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "SSHUsername", Me.txtAntSSHUsername.Text, Microsoft.Win32.RegistryValueKind.String)
                    '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & Me.txtAntAddress.Text, "SSHPassword", Me.txtAntSSHPassword.Text, Microsoft.Win32.RegistryValueKind.String)
                    '    End If
                    'End If

                    'Me.chklstAnts.SetItemChecked(Me.chklstAnts.Items.Add(Me.txtAntAddress.Text & ":" & sPort), True)
                    'Me.txtAntAddress.Text = ""
                Else
                    MsgBox("This address appears to already be in the list.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)
                End If
            End If
        Catch ex As Exception When bErrorHandle = True
            AddToLogQueue("An error occurred when trying to add or save an Ant config: " & ex.Message)
            MsgBox("An error occurred when trying to add or save an Ant config:" & vbCrLf & vbCrLf & ex.Message)
        End Try

    End Sub

    Private Sub cmdDelAnt_Click(sender As System.Object, e As System.EventArgs) Handles cmdDelAnt.Click

        Try
            If Me.dataAntConfig.SelectedRows.Count = 0 Then
                MsgBox("Please select one or more Ants to remove first.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                Exit Sub
            End If

            Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\AntsV2", True)
                For Each dr As DataGridViewRow In Me.dataAntConfig.SelectedRows
                    key.DeleteSubKey(dr.Cells("ID").Value)

                    Me.dataAntConfig.Rows.Remove(dr)
                Next
            End Using
        Catch ex As Exception When bErrorHandle = True
            MsgBox("An error occurred when trying to delete an Ant:" & vbCrLf & vbCrLf & ex.Message)
        End Try

    End Sub

    Private Sub cmbRefreshRate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cmbRefreshRate.KeyPress, cmbAlertEMailGovernor.KeyPress

        e.Handled = True

    End Sub

    Private Sub NumericOnlyKeyPressHandler(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtRefreshRate.KeyPress, _
        txtAlertEMailGovernor.KeyPress, txtAntAPIPort.KeyPress, txtAntSSHPort.KeyPress, txtAntWebPort.KeyPress, txtAlertS1Temp.KeyPress, txtAlertS2Temp.KeyPress, _
        txtAlertS3Temp.KeyPress, cmbAntScanStart.KeyPress, cmbAntScanStop.KeyPress

        Select Case e.KeyChar
            Case "0" To "9", vbBack
                'okay

            Case Else
                e.Handled = True

        End Select

    End Sub

    Private Sub CalcRefreshRate()

        Select Case Me.cmbRefreshRate.Text
            Case "Seconds"
                iRefreshRate = Val(Me.txtRefreshRate.Text)

            Case "Minutes"
                iRefreshRate = Val(Me.txtRefreshRate.Text) * 60

            Case "Hours"
                iRefreshRate = Val(Me.txtRefreshRate.Text) * 60 * 60

        End Select

        If iRefreshRate = 0 Then
            iRefreshRate = 300
        End If

    End Sub

    Private Sub txtRefreshRate_LostFocus(sender As Object, e As System.EventArgs) Handles txtRefreshRate.LostFocus, cmbRefreshRate.LostFocus

        Call CalcRefreshRate()

    End Sub

    Private Sub chkShow_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkShowBestShare.CheckedChanged, chkShowBlocks.CheckedChanged, _
        chkShowFans.CheckedChanged, chkShowGHs5s.CheckedChanged, chkShowGHsAvg.CheckedChanged, chkShowHWE.CheckedChanged, chkShowPools.CheckedChanged, _
        chkShowStatus.CheckedChanged, chkShowTemps.CheckedChanged, chkShowUptime.CheckedChanged, chkShowFreqs.CheckedChanged, chkShowHighFan.CheckedChanged, _
        chkShowHighTemp.CheckedChanged, chkShowXCount.CheckedChanged, chkShowRej.CheckedChanged, chkShowStale.CheckedChanged, chkShowDifficulty.CheckedChanged, _
        chkShowACount.CheckedChanged

        Dim chkAny As CheckBox

        If bStarted = False Then Exit Sub

        chkAny = DirectCast(sender, CheckBox)

        Select Case chkAny.Name
            Case "chkShowUptime"
                Me.dataAnts.Columns("Uptime").Visible = chkAny.Checked

            Case "chkShowGHs5s"
                Me.dataAnts.Columns("GH/s(5s)").Visible = chkAny.Checked

            Case "chkShowGHsAvg"
                Me.dataAnts.Columns("GH/s(avg)").Visible = chkAny.Checked

            Case "chkShowBlocks"
                Me.dataAnts.Columns("Blocks").Visible = chkAny.Checked

            Case "chkShowHWE"
                Me.dataAnts.Columns("HWE%").Visible = chkAny.Checked

            Case "chkShowBestShare"
                Me.dataAnts.Columns("BestShare").Visible = chkAny.Checked

            Case "chkShowPools"
                Me.dataAnts.Columns("Pools").Visible = chkAny.Checked

            Case "chkShowFans"
                Me.dataAnts.Columns("Fans").Visible = chkAny.Checked

            Case "chkShowTemps"
                Me.dataAnts.Columns("Temps").Visible = chkAny.Checked

            Case "chkShowStatus"
                Me.dataAnts.Columns("Status").Visible = chkAny.Checked

            Case "chkShowFreqs"
                Me.dataAnts.Columns("Freq").Visible = chkAny.Checked

            Case "chkShowHighFan"
                Me.dataAnts.Columns("HFan").Visible = chkAny.Checked

            Case "chkShowHighTemp"
                Me.dataAnts.Columns("HTemp").Visible = chkAny.Checked

            Case "chkShowXCount"
                Me.dataAnts.Columns("XCount").Visible = chkAny.Checked

            Case "chkShowRej"
                Me.dataAnts.Columns("Rej%").Visible = chkAny.Checked

            Case "chkShowStale"
                Me.dataAnts.Columns("Stale%").Visible = chkAny.Checked

            Case "chkShowDifficulty"
                Me.dataAnts.Columns("Diff").Visible = chkAny.Checked

            Case "chkShowACount"
                Me.dataAnts.Columns("ACount").Visible = chkAny.Checked

            Case Else
                MsgBox(chkAny.Name & " not found!", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly)

        End Select

    End Sub

    'Private Sub optAddS1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optAntS1.CheckedChanged, optAntS2.CheckedChanged

    '    Dim opt As RadioButton

    '    opt = sender

    '    If opt.Checked = True Then
    '        If opt.Name = "optAddS1" Then
    '            optAntS2.Checked = False
    '        Else
    '            optAntS1.Checked = False
    '        End If
    '    End If

    'End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As System.EventArgs) Handles NotifyIcon1.DoubleClick

        Me.Show()
        Me.Focus()

    End Sub

    Private Sub frmAntMonitor_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.NotifyIcon1.Visible = False
        My.Application.DoEvents()

    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Hide()
        e.Cancel = True

        If ToldUserRunningInNotificationTray = False Then
            Me.NotifyIcon1.ShowBalloonTip(30000, "Still running!", "Still running in notification tray!  If you want to close me, right click me and click Exit.", ToolTipIcon.Info)
            ToldUserRunningInNotificationTray = True

            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, "ToldUserAboutNotification", "Y", Microsoft.Win32.RegistryValueKind.String)
        End If

    End Sub

    'Private Sub txtAlertS1Temp_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtAlertS1Temp.KeyPress, txtAlertS2Temp.KeyPress

    '    Select Case e.KeyChar
    '        Case "0" To "9", vbBack
    '            'good

    '        Case Else
    '            e.Handled = True

    '    End Select

    'End Sub

    Private Sub mnuShow_Click(sender As Object, e As System.EventArgs) Handles mnuShow.Click

        Me.Show()
        Me.Focus()

    End Sub

    Private Sub cmdSaveAlerts_Click(sender As System.Object, e As System.EventArgs) Handles cmdSaveAlerts1.Click, cmdSaveAlerts2.Click, cmdSaveAlerts3.Click, cmdSaveAlerts4.Click, cmdSaveAlerts5.Click

        With ctlsByKey
            .SetRegKeyByControl(Me.chkAlertHighlightField)
            .SetRegKeyByControl(Me.chkAlertShowAnnoyingPopup)
            .SetRegKeyByControl(Me.chkAlertShowNotifyPopup)

            If Me.chkAlertStartProcess.Checked = True Then
                If Me.txtAlertStartProcessName.Text.IsNullOrEmpty = True Then
                    MsgBox("Please specify a file to launch.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertStartProcess.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertStartProcess)
            .SetRegKeyByControl(Me.txtAlertStartProcessName)
            .SetRegKeyByControl(Me.txtAlertStartProcessParms)

            's1
            If Me.chkAlertIfS1Temp.Checked = True Then
                If Me.txtAlertS1Temp.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S1 temp alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS1Temp.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS1Temp)
            .SetRegKeyByControl(Me.txtAlertS1Temp)

            's2
            If Me.chkAlertIfS2Temp.Checked = True Then
                If Me.txtAlertS2Temp.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S2 temp alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS2Temp.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS2Temp)
            .SetRegKeyByControl(Me.txtAlertS2Temp)

            's3
            If Me.chkAlertIfS3Temp.Checked = True Then
                If Me.txtAlertS3Temp.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S3 temp alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS3Temp.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS3Temp)
            .SetRegKeyByControl(Me.txtAlertS3Temp)

            's1
            If Me.chkAlertIfS1FanHigh.Checked = True Then
                If Me.txtAlertS1FanHigh.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S1 high fan alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS1FanHigh.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS1FanHigh)
            .SetRegKeyByControl(Me.txtAlertS1FanHigh)

            's2
            If Me.chkAlertIfS2FanHigh.Checked = True Then
                If Me.txtAlertS2FanHigh.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S2 high fan alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS2FanHigh.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS2FanHigh)
            .SetRegKeyByControl(Me.txtAlertS2FanHigh)

            'S3
            If Me.chkAlertIfS3FanHigh.Checked = True Then
                If Me.txtAlertS3FanHigh.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S3 high fan alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS3FanHigh.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS3FanHigh)
            .SetRegKeyByControl(Me.txtAlertS3FanHigh)

            'S1
            If Me.chkAlertIfS1FanLow.Checked = True Then
                If Me.txtAlertS1FanLow.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S1 low fan alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS1FanLow.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS1FanLow)
            .SetRegKeyByControl(Me.txtAlertS1FanLow)

            'S2
            If Me.chkAlertIfS2FanLow.Checked = True Then
                If Me.txtAlertS2FanLow.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S2 low fan alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS2FanLow.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS2FanLow)
            .SetRegKeyByControl(Me.txtAlertS2FanLow)

            'S3
            If Me.chkAlertIfS3FanLow.Checked = True Then
                If Me.txtAlertS3FanLow.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S3 low fan alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS3FanLow.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS3FanLow)
            .SetRegKeyByControl(Me.txtAlertS3FanLow)

            's1
            If Me.chkAlertIfS1Hash.Checked = True Then
                If Me.txtAlertS1Hash.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S1 hash alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS1Hash.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS1Hash)
            .SetRegKeyByControl(Me.txtAlertS1Hash)

            's2
            If Me.chkAlertIfS2Hash.Checked = True Then
                If Me.txtAlertS2Hash.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S2 hash alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS2Hash.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS2Hash)
            .SetRegKeyByControl(Me.txtAlertS2Hash)

            'S3
            If Me.chkAlertIfS3Hash.Checked = True Then
                If Me.txtAlertS3Hash.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S3 hash alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS3Hash.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS3Hash)
            .SetRegKeyByControl(Me.txtAlertS3Hash)

            'S1
            If Me.chkAlertIfS1XCount.Checked = True Then
                If Me.txtAlertS1XCount.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S1 XCount alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS1XCount.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS1XCount)
            .SetRegKeyByControl(Me.txtAlertS1XCount)

            'S2
            If Me.chkAlertIfS2XCount.Checked = True Then
                If Me.txtAlertS2XCount.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S2 XCount alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS2XCount.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS2XCount)
            .SetRegKeyByControl(Me.txtAlertS2XCount)

            'S3
            If Me.chkAlertIfS3XCount.Checked = True Then
                If Me.txtAlertS3XCount.Text.IsNullOrEmpty Then
                    MsgBox("Please specify an S3 XCount alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertIfS3XCount.Checked = False
                End If
            End If

            .SetRegKeyByControl(Me.chkAlertIfS3XCount)
            .SetRegKeyByControl(Me.txtAlertS3XCount)

            'email notifications
            Call ctlsByKey.SetRegKeyByControl(Me.txtSMTPServer)
            Call ctlsByKey.SetRegKeyByControl(Me.txtSMTPPort)
            Call ctlsByKey.SetRegKeyByControl(Me.txtSMTPUserName)
            Call ctlsByKey.SetRegKeyByControl(Me.txtSMTPPassword)
            Call ctlsByKey.SetRegKeyByControl(Me.txtSMTPAlertName)
            Call ctlsByKey.SetRegKeyByControl(Me.txtSMTPAlertAddress)
            Call ctlsByKey.SetRegKeyByControl(Me.txtSMTPAlertSubject)
            Call ctlsByKey.SetRegKeyByControl(Me.txtSMTPFromAddress)
            Call ctlsByKey.SetRegKeyByControl(Me.txtSMTPFromName)
            Call ctlsByKey.SetRegKeyByControl(Me.chkSMTPSSL)

            Call ctlsByKey.SetRegKeyByControl(Me.txtAlertEMailGovernor)
            Call ctlsByKey.SetRegKeyByControl(Me.cmbAlertEMailGovernor)

            If Me.chkAlertSendEMail.Checked = True Then
                If String.IsNullOrEmpty(Me.txtSMTPServer.Text) = True OrElse String.IsNullOrEmpty(Me.txtSMTPPort.Text) = True OrElse String.IsNullOrEmpty(Me.txtSMTPUserName.Text) = True _
                    OrElse String.IsNullOrEmpty(Me.txtSMTPPassword.Text) = True OrElse String.IsNullOrEmpty(Me.txtSMTPAlertAddress.Text) = True OrElse _
                    String.IsNullOrEmpty(Me.txtSMTPFromAddress.Text) = True Then

                    Me.chkAlertSendEMail.Checked = False

                    MsgBox("EMail alerts are enabled, but one of more required SMTP fields are not specified.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")
                End If

                If Val(Me.txtAlertEMailGovernor.Text) = 0 Then
                    MsgBox("EMail alerts are enabled, but the EMail governor field appears to be zero.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertSendEMail.Checked = False
                End If
            End If

            Call ctlsByKey.SetRegKeyByControl(Me.chkAlertSendEMail)

            If Me.chkAlertRebootIfXd.Checked = True Then
                If Val(Me.txtAlertRebootGovernor.Text) = 0 Then
                    MsgBox("Reboot if XCount is enabled, but the reboot governor field appears to be zero.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

                    Me.chkAlertRebootIfXd.Checked = False
                End If
            End If

            Call .SetRegKeyByControl(Me.chkAlertRebootIfXd)
            Call .SetRegKeyByControl(Me.chkAlertRebootAntsOnHashAlert)
            Call .SetRegKeyByControl(Me.chkRebootAntOnError)
            Call .SetRegKeyByControl(Me.txtAlertRebootGovernor)
            Call .SetRegKeyByControl(Me.cmbAlertRebootGovernor)
        End With

    End Sub

    Private Sub mnuMainExit_Click(sender As System.Object, e As System.EventArgs) Handles mnuMainExit.Click, mnuExit.Click

        Me.NotifyIcon1.Visible = False

        bShutDown = True

        For Each tThread As System.Threading.Thread In Me.workerThread
            tThread.Abort()
        Next

        My.Application.DoEvents()

        End

    End Sub

    Private Sub cmdAlertStartFileFinder_Click(sender As System.Object, e As System.EventArgs) Handles cmdAlertProcessFileFinder.Click

        Dim dlg As OpenFileDialog

        dlg = New OpenFileDialog

        dlg.InitialDirectory = "c:\"
        dlg.ShowDialog()

        If String.IsNullOrEmpty(dlg.FileName) = True Then Exit Sub

        Me.txtAlertStartProcessName.Text = dlg.FileName

    End Sub

    Private Sub cmdSendTestEMail_Click(sender As System.Object, e As System.EventArgs) Handles cmdSendTestEMail.Click

        If String.IsNullOrEmpty(Me.txtSMTPAlertSubject.Text) = True Then
            Call SendEMail("Ant TEST alert!", "Ant TEST alert!")
        Else
            Call SendEMail("Ant TEST alert!", Me.txtSMTPAlertSubject.Text)
        End If

    End Sub

    Private Sub SendEMail(ByVal sBody As String, ByVal sSubject As String)

        Dim SMTP As System.Net.Mail.SmtpClient
        Dim MSGfrom, MSGto As System.Net.Mail.MailAddress
        Dim MSG As System.Net.Mail.MailMessage

        SMTP = New System.Net.Mail.SmtpClient(Me.txtSMTPServer.Text, Me.txtSMTPPort.Text)

        SMTP.UseDefaultCredentials = False
        SMTP.Credentials = New System.Net.NetworkCredential(Me.txtSMTPUserName.Text, Me.txtSMTPPassword.Text)
        SMTP.EnableSsl = Me.chkSMTPSSL.Checked
        SMTP.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network

        If String.IsNullOrEmpty(Me.txtSMTPAlertName.Text) = True Then
            MSGto = New System.Net.Mail.MailAddress(Me.txtSMTPAlertAddress.Text, Me.txtSMTPAlertAddress.Text, System.Text.Encoding.UTF8)
        Else
            MSGto = New System.Net.Mail.MailAddress(Me.txtSMTPAlertAddress.Text, Me.txtSMTPAlertName.Text, System.Text.Encoding.UTF8)
        End If

        If String.IsNullOrEmpty(Me.txtSMTPFromName.Text) = True Then
            MSGfrom = New System.Net.Mail.MailAddress(Me.txtSMTPFromAddress.Text, Me.txtSMTPFromAddress.Text, System.Text.Encoding.UTF8)
        Else
            MSGfrom = New System.Net.Mail.MailAddress(Me.txtSMTPFromAddress.Text, Me.txtSMTPFromName.Text, System.Text.Encoding.UTF8)
        End If

        MSG = New System.Net.Mail.MailMessage(MSGfrom, MSGto)
        MSG.Body = sBody
        MSG.BodyEncoding = System.Text.Encoding.UTF8

        MSG.Subject = sSubject
        MSG.SubjectEncoding = System.Text.Encoding.UTF8

        AddHandler SMTP.SendCompleted, AddressOf HandleEMailResponse

        SMTP.SendAsync(MSG, SMTP)

    End Sub

    Private Sub HandleEMailResponse(ByVal Sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs)

        If e.Error Is Nothing Then
            Me.NotifyIcon1.ShowBalloonTip(5000, "Alert email sent", "Alert email sent", ToolTipIcon.Info)
            AddToLogQueue("Alert email sent.")
        Else
            Me.NotifyIcon1.ShowBalloonTip(5000, "Alert email failed", "Alert email failed", ToolTipIcon.Warning)
            AddToLogQueue("Alert email failed!")
        End If

        With DirectCast(e.UserState, System.Net.Mail.SmtpClient)
            RemoveHandler .SendCompleted, AddressOf HandleEMailResponse
            .Dispose()
        End With

    End Sub

    'if already running, forces the other one to come to the foreground
    Public Sub HandlesAlreadyRunning(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance

        e.BringToForeground = True

    End Sub

    Private Sub dataAnts_ColumnDisplayIndexChanged(sender As Object, e As System.Windows.Forms.DataGridViewColumnEventArgs)

        Dim dt As DataGridView

        dt = DirectCast(sender, DataGridView)

        With My.Computer.Registry
            .CurrentUser.CreateSubKey(csRegKey & "\Columns\" & dt.Name & "_DisplayIndex")
            .SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Columns\" & dt.Name & "_DisplayIndex", e.Column.Name, e.Column.DisplayIndex, Microsoft.Win32.RegistryValueKind.DWord)
        End With

    End Sub

    Private Sub dataAnts_CellContextMenuStripNeeded(sender As Object, e As System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventArgs) Handles dataAnts.CellContextMenuStripNeeded

        Dim colAnts As System.Collections.Generic.List(Of Integer)
        Dim x As Integer
        Dim AntConfigRow As DataRow

        If e.RowIndex = -1 Then Exit Sub

        AntConfigRow = FindAntConfig(Me.dataAnts.Rows(e.RowIndex).Cells("ID").Value)

        If AntConfigRow("UseAPI") <> "Y" Then Exit Sub

        '0 - reboot one
        '1 - reboot multiple
        '2 - remove from list
        '3 - shutdown s2
        '4 - update pools
        mnuAntMenu.Items(0).Text = "Reboot " & Me.dataAnts.Rows(e.RowIndex).Cells("Name").Value
        mnuAntMenu.Items(0).Tag = Me.dataAnts.Rows(e.RowIndex).Cells("ID").Value

        'reboot multiple
        If Me.dataAnts.SelectedRows.Count = 0 Then
            mnuAntMenu.Items(1).Visible = False
        Else
            mnuAntMenu.Items(1).Visible = True

            mnuAntMenu.Items(1).Tag = New System.Collections.Generic.List(Of Integer)
            colAnts = mnuAntMenu.Items(1).Tag

            x = 0

            For Each dr As DataGridViewRow In Me.dataAnts.SelectedRows
                colAnts.Add(dr.Cells("ID").Value)

                x += 1
            Next

            If x > 1 Then
                mnuAntMenu.Items(1).Text = "Reboot Multiple (" & x & " Ants)"
            Else
                mnuAntMenu.Items(1).Text = "Reboot Multiple (" & x & " Ant)"
            End If
        End If

        'remove from list
        mnuAntMenu.Items(2).Text = "Remove " & Me.dataAnts.Rows(e.RowIndex).Cells("Name").Value
        mnuAntMenu.Items(2).Tag = Me.dataAnts.Rows(e.RowIndex).Cells("ID").Value
        mnuAntMenu.Items(2).Visible = True

        'shutdown s2
        If AntConfigRow("Type") = "S2" Then
            mnuAntMenu.Items(3).Text = "Shutdown " & Me.dataAnts.Rows(e.RowIndex).Cells("Name").Value
            mnuAntMenu.Items(3).Tag = Me.dataAnts.Rows(e.RowIndex).Cells("ID").Value
            mnuAntMenu.Items(3).Visible = True
        End If

        'update pools
        If Me.lblPools1.Tag IsNot Nothing Then
            mnuAntMenu.Items(4).Tag = New System.Collections.Generic.List(Of Integer)
            colAnts = mnuAntMenu.Items(4).Tag

            x = 0

            If Me.dataAnts.SelectedRows.Count = 0 Then
                colAnts.Add(Me.dataAnts.Rows(e.RowIndex).Cells("ID").Value)

                x = 1
            Else
                For Each dr As DataGridViewRow In Me.dataAnts.SelectedRows
                    colAnts.Add(dr.Cells("ID").Value)

                    x += 1
                Next
            End If

            If x > 1 Then
                mnuAntMenu.Items(4).Text = "Update Pools (" & x & " Ants)"
            Else
                mnuAntMenu.Items(4).Text = "Update Pools (" & x & " Ant)"
            End If

            mnuAntMenu.Items(4).Visible = True
        End If

        e.ContextMenuStrip = mnuAntMenu

    End Sub

    Private Sub dataAnts_CellToolTipTextNeeded(sender As Object, e As System.Windows.Forms.DataGridViewCellToolTipTextNeededEventArgs) Handles dataAnts.CellToolTipTextNeeded

        If e.ColumnIndex = Me.dataAnts.Columns("Pools").Index AndAlso e.RowIndex <> -1 Then
            e.ToolTipText = Me.dataAnts.Rows(e.RowIndex).Cells("PoolData").Value.ToString
        End If

    End Sub

    Private Sub chkShowSelectionColumn_Click(sender As Object, e As System.EventArgs) Handles chkShowSelectionColumn.Click

        Me.dataAnts.RowHeadersVisible = Me.chkShowSelectionColumn.Checked

    End Sub

    'Private Sub chklstAnts_SelectedValueChanged(sender As Object, e As System.EventArgs)

    '    If Me.chklstAnts.SelectedItems.Count <> 1 Then
    '        Me.cmdSaveAnt.Enabled = False
    '    Else
    '        Me.cmdSaveAnt.Enabled = True

    '        Me.txtAntAddress.Text = Me.chklstAnts.SelectedItem.ToString.Substring(4)

    '        Call GetWebCredentials(RemoveAntPort(Me.chklstAnts.SelectedItem.ToString), Me.txtWebUsername.Text, Me.txtWebPassword.Text)
    '        Call GetSSHCredentials(RemoveAntPort(Me.chklstAnts.SelectedItem.ToString), Me.txtSSHUsername.Text, Me.txtSSHPassword.Text)
    '    End If

    'End Sub

    'Private Function RemoveAntPort(ByVal sAnt As String) As String

    '    Dim p() As String

    '    If sAnt.Contains(":") = True Then
    '        p = sAnt.Split(":")

    '        Return p(0) & ":" & p(1)
    '    Else
    '        Return sAnt
    '    End If

    'End Function

    Private Sub cmdSaveAnt_Click(sender As Object, e As System.EventArgs) Handles cmdSaveAnt.Click

        Call AddOrSaveAntLogic(False, Me.lblAntID.Tag)

        'Dim sTemp As String

        'sTemp = RemoveAntPort(Me.chklstAnts.SelectedItem.ToString)

        'If sTemp.Substring(0, 2) = "S1" OrElse sTemp.Substring(0, 2) = "S3" Then
        '    If Me.txtAntWebUsername.Text.IsNullOrEmpty = True Then
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "WebUsername", "root", Microsoft.Win32.RegistryValueKind.String)
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "WebPassword", "root", Microsoft.Win32.RegistryValueKind.String)
        '    Else
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "WebUsername", Me.txtAntWebUsername.Text, Microsoft.Win32.RegistryValueKind.String)
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "WebPassword", Me.txtAntWebPassword.Text, Microsoft.Win32.RegistryValueKind.String)
        '    End If

        '    If Me.txtAntSSHUsername.Text.IsNullOrEmpty = True Then
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "SSHUsername", "root", Microsoft.Win32.RegistryValueKind.String)
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "SSHPassword", "root", Microsoft.Win32.RegistryValueKind.String)
        '    Else
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "SSHUsername", Me.txtAntSSHUsername.Text, Microsoft.Win32.RegistryValueKind.String)
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "SSHPassword", Me.txtAntSSHPassword.Text, Microsoft.Win32.RegistryValueKind.String)
        '    End If
        'End If

        'If sTemp.Substring(0, 2) = "S2" Then
        '    If Me.txtAntWebUsername.Text.IsNullOrEmpty = True Then
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "WebUsername", "root", Microsoft.Win32.RegistryValueKind.String)
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "WebPassword", "root", Microsoft.Win32.RegistryValueKind.String)
        '    Else
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "WebUsername", Me.txtAntWebUsername.Text, Microsoft.Win32.RegistryValueKind.String)
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "WebPassword", Me.txtAntWebPassword.Text, Microsoft.Win32.RegistryValueKind.String)
        '    End If

        '    If Me.txtAntSSHUsername.Text.IsNullOrEmpty = True Then
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "SSHUsername", "root", Microsoft.Win32.RegistryValueKind.String)
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "SSHPassword", "admin", Microsoft.Win32.RegistryValueKind.String)
        '    Else
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "SSHUsername", Me.txtAntSSHUsername.Text, Microsoft.Win32.RegistryValueKind.String)
        '        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sTemp, "SSHPassword", Me.txtAntSSHPassword.Text, Microsoft.Win32.RegistryValueKind.String)
        '    End If
        'End If

    End Sub

    Private Sub mnuRebootAnt_Click(sender As Object, e As System.EventArgs) Handles mnuRebootAnt.Click

        Dim t As ToolStripMenuItem
        Dim AntConfigRow As DataRow

        t = sender
        AntConfigRow = FindAntConfig(t.Tag)

        Call AddToLogQueue("Reboot of " & t.Tag.ToString & " requested")
        Call RebootAnt(AntConfigRow, True, YNtoBoolean(AntConfigRow("RebootViaSSH")), Nothing)

    End Sub

    Private Function FormatHashRate(ByVal dHashRate As Double) As String

        Dim sTemp As String

        Select Case dHashRate
            Case 0
                sTemp = "ZERO"

            Case Is < 0.001
                sTemp = Format(dHashRate * 1000000, "###.##") & " H/s"

            Case Is < 1
                sTemp = Format(dHashRate * 1000, "###.##") & " KH/s"

            Case Is < 1000
                sTemp = Format(dHashRate, "###.##") & " MH/s"

            Case Is < 1000000
                sTemp = Format(dHashRate / 1000, "###.##") & " GH/s"

            Case Is < 1000000000
                sTemp = Format(dHashRate / 1000000, "###.##") & " TH/s"

            Case Is < 1000000000000
                sTemp = Format(dHashRate / 1000000000, "###.##") & " PH/s"

            Case Is < 1000000000000000
                sTemp = Format(dHashRate / 1000000000000, "###.##") & " EH/s"

            Case Is < 1000000000000000000
                sTemp = Format(dHashRate / 1000000000000000, "###.##") & " ZH/s"

            Case Else
                sTemp = "UNKNOWN (BFH?)"

        End Select

        Return sTemp

    End Function

    Private Sub mnuRebootMultiple_Click(sender As Object, e As System.EventArgs) Handles mnuRebootMultiple.Click

        Dim t As ToolStripMenuItem
        Dim c As System.Collections.Generic.List(Of Integer)
        Dim AntConfigRow As DataRow

        t = sender
        c = t.Tag

        For Each ID As Integer In c
            AntConfigRow = FindAntConfig(ID)
            Call AddToLogQueue("Reboot of " & AntConfigRow("Name") & " requested")
            Call RebootAnt(AntConfigRow, True, YNtoBoolean(AntConfigRow("RebootViaSSH")), Nothing)
        Next

    End Sub

    Private Sub mnuShutdownS2_Click(sender As Object, e As System.EventArgs) Handles mnuShutdownS2.Click

        Dim th As Threading.Thread
        Dim t As ToolStripMenuItem
        Dim ID As Integer

        t = sender
        ID = t.Tag

        th = New Threading.Thread(AddressOf Me._ShutdownAnt)

        th.Start(GetAntConfigByConfigRow(FindAntConfig(ID)))

    End Sub

    Private Sub _ShutdownAnt(ByVal AntConfig As stAntConfig)

        Dim ssh As Renci.SshNet.SshClient
        Dim sshCommand As Renci.SshNet.SshCommand

        Try
            AddToLogQueue("SHUTTING DOWN " & AntConfig.sName)

            ssh = New Renci.SshNet.SshClient(AntConfig.sIP, AntConfig.sSSHPort, AntConfig.sSSHUsername, AntConfig.sSSHPassword)
            ssh.Connect()

            sshCommand = ssh.CreateCommand("/sbin/shutdown -h -P now")
            sshCommand.Execute()

            If sshCommand.Error.IsNullOrEmpty = False Then
                AddToLogQueue("Shutdown of " & AntConfig.sName & " appears to have failed: " & sshCommand.Error)
            Else
                AddToLogQueue("Shutdown of " & AntConfig.sName & " appears to have succeeded")
            End If

            ssh.Disconnect()
            ssh.Dispose()

            sshCommand.Dispose()

        Catch ex As Exception
            AddToLogQueue("Shutdown of " & AntConfig.sName & " FAILED: " & ex.Message)
        End Try

    End Sub

    Private Function ValidatePool() As Boolean

        If Me.txtPoolDesc.Text.IsNullOrEmpty Then
            MsgBox("Please enter a pool description first.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

            Me.txtPoolDesc.Focus()

            Return False
        End If

        If Me.txtPoolURL.Text.IsNullOrEmpty Then
            MsgBox("Please enter a pool URL first.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

            Me.txtPoolURL.Focus()

            Return False
        End If

        If Me.txtPoolUsername.Text.IsNullOrEmpty Then
            MsgBox("Please enter a pool username first.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")

            Me.txtPoolUsername.Focus()

            Return False
        End If

        Return True

    End Function

    Private Sub cmdPoolAdd_Click(sender As System.Object, e As System.EventArgs) Handles cmdPoolAdd.Click

        Dim c As Integer

        If ValidatePool() = False Then Exit Sub

        c = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools", "Count", 0) + 1

        Me.lstPools.AddItem(Me.txtPoolDesc.Text, c)

        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & c, "Description", Me.txtPoolDesc.Text, Microsoft.Win32.RegistryValueKind.String)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & c, "URL", Me.txtPoolURL.Text, Microsoft.Win32.RegistryValueKind.String)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & c, "Username", Me.txtPoolUsername.Text, Microsoft.Win32.RegistryValueKind.String)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & c, "Password", Me.txtPoolPassword.Text, Microsoft.Win32.RegistryValueKind.String)

        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools", "Count", c, Microsoft.Win32.RegistryValueKind.String)

        Me.txtPoolDesc.Text = ""
        Me.txtPoolPassword.Text = ""
        Me.txtPoolURL.Text = ""
        Me.txtPoolUsername.Text = ""

    End Sub

    Private Sub lstPools_Click(sender As Object, e As System.EventArgs) Handles lstPools.Click

        Dim i As Integer

        i = Me.lstPools.ItemTag(Me.lstPools.SelectedIndex)

        Me.txtPoolDesc.Text = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & i, "Description", "")
        Me.txtPoolURL.Text = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & i, "URL", "")
        Me.txtPoolUsername.Text = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & i, "Username", "")
        Me.txtPoolPassword.Text = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & i, "Password", "")

    End Sub

    Private Sub cmdPoolChange_Click(sender As System.Object, e As System.EventArgs) Handles cmdPoolChange.Click

        Dim i As Integer

        If ValidatePool() = False Then Exit Sub

        i = Me.lstPools.ItemTag(Me.lstPools.SelectedIndex)

        Me.lstPools.Items(Me.lstPools.SelectedIndex) = Me.txtPoolDesc.Text

        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & i, "Description", Me.txtPoolDesc.Text, Microsoft.Win32.RegistryValueKind.String)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & i, "URL", Me.txtPoolURL.Text, Microsoft.Win32.RegistryValueKind.String)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & i, "Username", Me.txtPoolUsername.Text, Microsoft.Win32.RegistryValueKind.String)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & i, "Password", Me.txtPoolPassword.Text, Microsoft.Win32.RegistryValueKind.String)

    End Sub

    Private Sub cmdPoolDelete_Click(sender As Object, e As System.EventArgs) Handles cmdPoolDelete.Click

        Dim i As Integer

        If MsgBox("Are you sure you want to delete the selected pool: " & vbCrLf & vbCrLf & Me.lstPools.SelectedItem, MsgBoxStyle.Question Or MsgBoxStyle.YesNo) <> MsgBoxResult.Yes Then Exit Sub

        i = Me.lstPools.ItemTag(Me.lstPools.SelectedIndex)

        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\Pools", True)
            key.DeleteSubKey(i, False)
        End Using

        Me.lstPools.RemoveSelectedItem()

        Me.txtPoolDesc.Text = ""
        Me.txtPoolPassword.Text = ""
        Me.txtPoolURL.Text = ""
        Me.txtPoolUsername.Text = ""

    End Sub

    Private Sub cmdPoolsImportFromAnts_Click(sender As System.Object, e As System.EventArgs) Handles cmdPoolsImportFromAnts.Click

        Dim pd, pd2 As clsPoolData
        Dim pdl, pdl2 As System.Collections.Generic.List(Of clsPoolData)
        Dim i, c, x As Integer
        Dim bCheckAgain, bFound As Boolean

        c = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools", "Count", 0) + 1

        pdl = New System.Collections.Generic.List(Of clsPoolData)

        'create a list of all the poolds we know about already
        For x = 0 To Me.lstPools.Items.Count - 1
            pd = New clsPoolData

            i = Me.lstPools.ItemTag(x)

            pd.UID = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & i, "Username", "")
            pd.URL = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & i, "URL", "")

            pdl.Add(pd)
        Next

        'go through each ant
        For Each dr As DataGridViewRow In Me.dataAnts.Rows
            pdl2 = dr.Cells("PoolData2").Value

            'go through each pool for each ant
            For x = 0 To pdl2.Count - 1
                pd = pdl2(x)

                Do
                    bCheckAgain = False
                    bFound = False

                    For Each pd2 In pdl 'see if pool is in list we know about
                        If pd2.URL = pd.URL AndAlso pd2.UID = pd.UID Then
                            bFound = True

                            Exit For
                        End If
                    Next

                    If bFound = False Then
                        'add pool
                        pdl.Add(pd)

                        Me.lstPools.AddItem(dr.Cells("IPAddress").Value & " #" & x + 1, c)

                        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & c, "Description", dr.Cells("IPAddress").Value & " #" & x + 1, Microsoft.Win32.RegistryValueKind.String)
                        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & c, "URL", pd.URL, Microsoft.Win32.RegistryValueKind.String)
                        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & c, "Username", pd.UID, Microsoft.Win32.RegistryValueKind.String)

                        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools", "Count", c, Microsoft.Win32.RegistryValueKind.String)

                        c += 1

                        bCheckAgain = True
                    End If
                Loop While bCheckAgain = True   'if the list is changed, the enumeration can not continue, so we start over again
            Next
        Next

    End Sub

    Private Sub cmdPoolMake1_Click(sender As System.Object, e As System.EventArgs) Handles cmdPoolMake1.Click, cmdPoolMake2.Click, cmdPoolMake3.Click

        Dim pd As clsPoolData
        Dim lblPool As Label

        If ValidatePool() = False Then Exit Sub

        Select Case DirectCast(sender, Button).Text
            Case "Make Pool #1"
                lblPool = Me.lblPools1

            Case "Make Pool #2"
                lblPool = Me.lblPools2

            Case "Make Pool #3"
                lblPool = Me.lblPools3

        End Select

        lblPool.Text = Me.txtPoolDesc.Text

        If lblPool.Tag Is Nothing Then
            lblPool.Tag = New clsPoolData
        End If

        pd = lblPool.Tag

        pd.URL = Me.txtPoolURL.Text
        pd.UID = Me.txtPoolUsername.Text
        pd.PW = Me.txtPoolPassword.Text

    End Sub

    Private Sub cmdPoolClear2_Click(sender As System.Object, e As System.EventArgs) Handles cmdPoolClear2.Click, cmdPoolClear3.Click

        Dim pd As clsPoolData
        Dim lblPool As Label

        Select Case DirectCast(sender, Button).Text
            Case "Clear Pool #2"
                lblPool = Me.lblPools2

            Case "Clear Pool #3"
                lblPool = Me.lblPools3

        End Select

        lblPool.Text = "<Blank>"

        If lblPool.Tag Is Nothing Then
            lblPool.Tag = New clsPoolData
        End If

        pd = lblPool.Tag

        pd.URL = ""
        pd.UID = ""
        pd.PW = ""

    End Sub

    Private Sub mnuUpdatePools_Click(sender As Object, e As System.EventArgs) Handles mnuUpdatePools.Click

        Dim th As Threading.Thread
        Dim t As ToolStripMenuItem
        Dim c As System.Collections.Generic.List(Of Integer)

        t = sender
        c = t.Tag

        For Each ID As Integer In c
            th = New Threading.Thread(AddressOf _UpdatePools)

            th.Start(GetAntConfigByConfigRow(FindAntConfig(ID)))
        Next

    End Sub

    Private Sub _UpdatePools(ByVal AntConfig As stAntConfig)

        Dim ssh As Renci.SshNet.SshClient
        Dim sshCommand As Renci.SshNet.SshCommand
        Dim pd1, pd2, pd3 As clsPoolData

        Try
            AddToLogQueue("Update of pool info on " & AntConfig.sName & " requested")

            pd1 = Me.lblPools1.Tag
            pd2 = Me.lblPools2.Tag
            pd3 = Me.lblPools3.Tag

            If pd1.PW = "" Then pd1.PW = "abc"

            If pd2 IsNot Nothing Then
                If pd2.PW = "" Then pd2.PW = "abc"
            End If

            If pd3 IsNot Nothing Then
                If pd3.PW = "" Then pd3.PW = "abc"
            End If

            ssh = New Renci.SshNet.SshClient(AntConfig.sIP, AntConfig.sSSHPort, AntConfig.sSSHUsername, AntConfig.sSSHPassword)
            ssh.Connect()

            sshCommand = ssh.CreateCommand("/usr/bin/cgminer-api ""removepool|1""")
            sshCommand.Execute()

            System.Threading.Thread.Sleep(1000)

            sshCommand.Execute()

            System.Threading.Thread.Sleep(1000)

            sshCommand = ssh.CreateCommand("/usr/bin/cgminer-api ""addpool|" & pd1.URL.Replace("\", "\\") & "," & pd1.UID.Replace("\", "\\").Replace(",", "\,") & "," & pd1.PW.Replace("\", "\\").Replace(",", "\,") & """")
            sshCommand.Execute()

            System.Threading.Thread.Sleep(1000)

            sshCommand = ssh.CreateCommand("/usr/bin/cgminer-api ""switchpool|1""")
            sshCommand.Execute()

            System.Threading.Thread.Sleep(1000)

            sshCommand = ssh.CreateCommand("/usr/bin/cgminer-api ""removepool|0""")
            sshCommand.Execute()

            System.Threading.Thread.Sleep(1000)

            If pd2 IsNot Nothing Then
                sshCommand = ssh.CreateCommand("/usr/bin/cgminer-api ""addpool|" & pd2.URL.Replace("\", "\\") & "," & pd2.UID.Replace("\", "\\").Replace(",", "\,") & "," & pd2.PW.Replace("\", "\\").Replace(",", "\,") & """")
                sshCommand.Execute()

                System.Threading.Thread.Sleep(1000)
            End If

            If pd3 IsNot Nothing Then
                sshCommand = ssh.CreateCommand("/usr/bin/cgminer-api ""addpool|" & pd3.URL.Replace("\", "\\") & "," & pd3.UID.Replace("\", "\\").Replace(",", "\,") & "," & pd3.PW.Replace("\", "\\").Replace(",", "\,") & """")
                sshCommand.Execute()

                System.Threading.Thread.Sleep(1000)
            End If

            sshCommand = ssh.CreateCommand("/usr/bin/cgminer-api save")
            sshCommand.Execute()

            AddToLogQueue("Update of pool info on " & AntConfig.sName & " appears to have succeeded")

            ssh.Disconnect()
            ssh.Dispose()

            sshCommand.Dispose()
        Catch ex As Exception
            AddToLogQueue("Update of pool info on " & AntConfig.sName & " FAILED: " & ex.Message)
        End Try

    End Sub

    Private Sub txtLog_VisibleChanged(sender As Object, e As System.EventArgs) Handles txtLog.VisibleChanged

        Me.txtLog.SelectionStart = Me.txtLog.TextLength
        Me.txtLog.ScrollToCaret()

    End Sub

    'runs all the time (~every 10ms) to pass Ant refresh data to the refresh routine
    'and updates the log
    Private Sub timerDoStuff_Tick(sender As System.Object, e As System.EventArgs) Handles timerDoStuff.Tick

        Me.timerDoStuff.Enabled = False

        Try
            While AntRefreshDataQueue.Count <> 0
                SyncLock AntRefreshLock
                    Call RefreshGrid(AntRefreshDataQueue.Dequeue)
                End SyncLock

                My.Application.DoEvents()
            End While

            While logQueue.Count <> 0
                SyncLock logQueueLock
                    Me.txtLog.AppendText(Now.ToLocalTime & ": " & logQueue.Dequeue & vbCrLf)
                    Me.txtLog.SelectionStart = Me.txtLog.TextLength
                    Me.txtLog.ScrollToCaret()
                End SyncLock

                My.Application.DoEvents()
            End While
        Catch ex As Exception When bErrorHandle = True
            AddToLogQueue("ERROR in DoStuff routine: " & ex.Message)
        Finally
            Me.timerDoStuff.Enabled = True
        End Try

    End Sub

    Private Sub RefreshTitle()

        Dim x As Integer
        Dim dbTemp As Double

        x = 0

        Try
            For Each dg As DataGridViewRow In Me.dataAnts.Rows
                If IsDBNull(dg.Cells("Uptime").Value) = False AndAlso dg.Cells("Uptime").Value <> "ERROR" AndAlso dg.Cells("Uptime").Value <> "???" Then
                    x += 1

                    dbTemp += dg.Cells("GH/s(avg)").Value
                End If
            Next

            Me.Text = csVersion & " - " & Now.ToString & " - " & x & " of " & iAntsEnabled & " responded - " & FormatHashRate(dbTemp * 1000) & " " & sAlerts
        Catch ex As Exception When bErrorHandle = True
            AddToLogQueue("ERROR in RefreshTitle routine: " & ex.Message)
        End Try

    End Sub

    Public Sub AddToLogQueue(ByVal sMsg As String)

        SyncLock logQueueLock
            logQueue.Enqueue(sMsg)
        End SyncLock

    End Sub

    Private Sub cmdChangeThreads_Click(sender As System.Object, e As System.EventArgs) Handles cmdChangeThreads.Click

        Dim x As Integer

        For Each tThread As System.Threading.Thread In Me.workerThread
            tThread.Abort()
        Next

        Array.Resize(ThreadHandlers, Me.trackThreadCount.Value)
        Array.Resize(workerThread, Me.trackThreadCount.Value)

        For x = 0 To ThreadHandlers.Length - 2
            ThreadHandlers(x) = New clsThreadHandler

            workerThread(x) = New System.Threading.Thread(AddressOf HandleWork)
            workerThread(x).Name = "WorkerThread" & x
            workerThread(x).Start(x)
        Next

        ThreadHandlers(ThreadHandlers.Length - 1) = New clsThreadHandler
        workerThread(ThreadHandlers.Length - 1) = New System.Threading.Thread(AddressOf CheckForWork)
        workerThread(ThreadHandlers.Length - 1).Name = "ThreadDispatcher" & x
        workerThread(ThreadHandlers.Length - 1).Start()

    End Sub

    Private Sub trackThreadCount_ValueChanged(sender As Object, e As System.EventArgs) Handles trackThreadCount.ValueChanged

        Me.ToolTip1.SetToolTip(Me.trackThreadCount, Me.trackThreadCount.Value)

    End Sub

    Private Sub txtDisplayRefreshInSecs_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtDisplayRefreshInSecs.KeyPress

        Select Case e.KeyChar
            Case "0" To "9", vbBack
                'okay

            Case Else
                e.Handled = True

        End Select

    End Sub

    Private Sub txtDisplayRefreshInSecs_Leave(sender As Object, e As System.EventArgs) Handles txtDisplayRefreshInSecs.Leave

        Dim i As Integer

        If Integer.TryParse(Me.txtDisplayRefreshInSecs.TextLength, i) = False Then
            iDisplayRefreshPeriod = 1
        Else
            iDisplayRefreshPeriod = i
        End If

    End Sub

    Private Sub txtAntAddress_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtAntAddress.KeyPress

        Select Case e.KeyChar
            Case "0" To "9", vbBack, "."
                'okay

            Case Else
                e.Handled = True

        End Select

    End Sub

    Private Sub txtIPRangeToScan_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtIPRangeToScan.KeyPress

        Select Case e.KeyChar
            Case "0" To "9", vbBack, "."
                'okay

            Case Else
                e.Handled = True

        End Select

    End Sub

    Private Sub txtIPRangeToScan_Leave(sender As Object, e As System.EventArgs) Handles txtIPRangeToScan.Leave

        If HowManyInString(Me.txtIPRangeToScan.Text, ".") > 2 Then
            MsgBox("Please enter an address in the format of '192.168.0' (no quotes).", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

            Me.txtIPRangeToScan.Focus()
        End If

    End Sub

    Private Sub dataAntConfig_SelectionChanged(sender As Object, e As System.EventArgs) Handles dataAntConfig.SelectionChanged

        Dim dr As DataGridViewRow

        Try
            If Me.dataAntConfig.SelectedRows.Count = 1 Then
                dr = Me.dataAntConfig.SelectedRows(0)

                Me.txtAntAddress.Text = dr.Cells("IPAddress").Value
                Me.txtAntName.Text = dr.Cells("Name").Value

                Select Case dr.Cells("Type").Value
                    Case "S1"
                        Me.optAntS1.Checked = True

                    Case "S2"
                        Me.optAntS2.Checked = True

                    Case "S3"
                        Me.optAntS3.Checked = True

                End Select

                Me.txtAntSSHUsername.Text = dr.Cells("SSHUsername").Value
                Me.txtAntSSHPassword.Text = dr.Cells("SSHPassword").Value
                Me.txtAntSSHPort.Text = dr.Cells("SSHPort").Value

                Me.txtAntWebUsername.Text = dr.Cells("WebUsername").Value
                Me.txtAntWebPassword.Text = dr.Cells("WebPassword").Value
                Me.txtAntWebPort.Text = dr.Cells("HTTPPort").Value

                Me.chkAntUseAPI.Checked = YNtoBoolean(dr.Cells("UseAPI").Value)
                Me.txtAntAPIPort.Text = dr.Cells("APIPort").Value
                Me.chkRebootAntOnError.Checked = YNtoBoolean(dr.Cells("RebootViaSSH").Value)
                Me.chkAntActive.Checked = YNtoBoolean(dr.Cells("Active").Value)

                Me.lblAntID.Text = "ID #" & dr.Cells("ID").Value
                Me.lblAntID.Tag = dr.Cells("ID").Value
            Else
                Me.lblAntID.Tag = -1
            End If
        Catch ex As Exception When bErrorHandle = True
            AddToLogQueue("An error occurred when displaying the config for an Ant: " & ex.Message)
        End Try

    End Sub

    Private Sub optAntS1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optAntS1.CheckedChanged, optAntS2.CheckedChanged, optAntS3.CheckedChanged

        Dim sTemp As String
        Dim s() As String

        Try
            If Me.optAntS1.Checked = True Then
                sTemp = "S1"
            ElseIf Me.optAntS2.Checked = True Then
                sTemp = "S2"
            ElseIf Me.optAntS3.Checked = True Then
                sTemp = "S3"
            Else
                Exit Sub
            End If

            s = Me.txtAntAddress.Text.Split(".")

            If Me.txtAntName.Text.IsNullOrEmpty Then
                Me.txtAntName.Text = sTemp & ":" & s(2) & "." & s(3)
            ElseIf Me.txtAntName.Text.Substring(0, 3) = "S1:" OrElse Me.txtAntName.Text.Substring(0, 3) = "S2:" OrElse Me.txtAntName.Text.Substring(0, 3) = "S3:" Then
                Me.txtAntName.Text = sTemp & ":" & Me.txtAntName.Text.Substring(3)
            End If
        Catch ex As Exception When bErrorHandle = True
        End Try

    End Sub

    Private Sub cmdAntClear_Click(sender As System.Object, e As System.EventArgs) Handles cmdAntClear.Click

        Me.txtAntName.Text = ""
        Me.txtAntAddress.Text = ""
        Me.optAntS1.Checked = False
        Me.optAntS2.Checked = False
        Me.optAntS3.Checked = False

        Me.txtAntAddress.Focus()

    End Sub

    Private Sub cmbAntScanStart_Leave(sender As Object, e As System.EventArgs) Handles cmbAntScanStart.Leave, cmbAntScanStop.Leave

        Dim cmbAny As ComboBox

        cmbAny = sender

        If cmbAny.Text.IsNullOrEmpty = True OrElse Val(cmbAny.Text) > 254 Then
            Me.cmbAntScanStart.Text = "1"
        End If

        If Val(Me.cmbAntScanStart.Text) > Val(Me.cmbAntScanStop.Text) Then
            Me.cmbAntScanStart.Text = "1"
            Me.cmbAntScanStop.Text = "254"
        End If

    End Sub

    Private Sub mnuRemoveAnt_Click(sender As System.Object, e As System.EventArgs) Handles mnuRemoveAnt.Click

        Dim t As ToolStripMenuItem
        
        t = sender

        For Each dr As DataRow In Me.ds.Tables(0).Rows
            If dr("ID") = t.Tag Then
                ds.Tables(0).Rows.Remove(dr)

                Exit For
            End If
        Next

    End Sub
End Class

'wrapper around the datagridview to allow disabling the paint event
'this way you can populate data on the grid without it having to render (paint)
'when done, set Refreshing back to false and next paint call will clear it up
Public Class dgvWrapper

    Inherits DataGridView

    Public Refreshing As Boolean

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)

        If Me.Refreshing = False Then
            MyBase.OnPaint(e)
        End If

    End Sub

End Class