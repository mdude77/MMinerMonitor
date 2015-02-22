Imports MMinerMonitor.Extensions
Imports MMinerMonitor.MobileMinerApi.Helpers

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
    Private ds, dsMinerConfig As DataSet

    'location of the configuration settings in the registry
    Private Const csRegKey As String = "Software\MAntMonitor"

    'version
    Private Const csVersion As String = "M's Miner Monitor v5.0"

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

    Private Class clsSupportedMinerInfo

        'supported miner types
        Public Enum enSupportedMinerTypes
            AntMinerS1 = 1
            AntMinerS2 = 2
            AntMinerS3 = 3
            AntMinerS4 = 4
            AntMinerC1 = 5
            SpondooliesSP10 = 6
            SpondooliesSP20 = 7
            SpondooliesSP30 = 8
            SpondooliesSP31 = 9
            SpondooliesSP35 = 10
            AntminerS5 = 11
            GridseedGBlack = 12
            InnosiliconA288MH = 13
        End Enum

        Public Enum enSpeedType
            MHs = 1
            GHs = 2
            THs = 3
        End Enum

        Public Enum enHashType
            SHA256 = 1
            Scrypt = 2
        End Enum

        Public Structure stAlertTypes
            Public Fans As Boolean
            Public Hash As Boolean
            Public Temps As Boolean
            Public XCount As Boolean
        End Structure

        Public Structure stAlertInfoStringRegistry
            Public Key As String
            Public Value As String
        End Structure

        Public Structure stAlertInfoBooleanRegistry
            Public Key As String
            Public Value As Boolean
        End Structure

        Public Class clsAlertInfo
            Public Item As stAlertInfoStringRegistry
            Public Enabled As stAlertInfoBooleanRegistry

            Public Sub Initialize(ByVal sEnabledKey As String, ByVal sValueKey As String, ByVal sDefault As String, ByVal bDefaultEnabled As Boolean)
                Dim sReturn As String

                Me.Item.Key = sValueKey
                Me.Enabled.Key = sEnabledKey

                Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey)
                    If key Is Nothing Then
                        My.Computer.Registry.CurrentUser.CreateSubKey(csRegKey)
                    End If
                End Using

                Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey)
                    'is enabled
                    sReturn = key.GetValue(sEnabledKey)

                    If String.IsNullOrEmpty(sReturn) = True Then
                        Me.Enabled.Value = bDefaultEnabled
                    Else
                        If sReturn = "Y" Then
                            Me.Enabled.Value = True
                        Else
                            Me.Enabled.Value = False
                        End If
                    End If

                    'value  
                    sReturn = key.GetValue(sValueKey)

                    If String.IsNullOrEmpty(sReturn) = False Then
                        Me.Item.Value = sReturn
                    Else
                        Me.Item.Value = sDefault
                    End If
                End Using
            End Sub

            Public Sub SaveSettings(ByVal bEnabled As Boolean, ByVal sValue As String)
                If Me.Enabled.Key Is Nothing Then
                    'unsupported value
                    Exit Sub
                End If

                Me.Enabled.Value = bEnabled
                Me.Item.Value = sValue

                If bEnabled = True Then
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, Me.Enabled.Key, "Y")
                Else
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, Me.Enabled.Key, "N")
                End If

                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, Me.Item.Key, sValue)
            End Sub

        End Class

        Public Class clsAlerts
            Public FanHigh As clsAlertInfo
            Public FanLow As clsAlertInfo
            Public HashLow As clsAlertInfo
            Public TempHigh As clsAlertInfo
            Public XCount As clsAlertInfo

            Public Sub New()
                FanHigh = New clsAlertInfo
                FanLow = New clsAlertInfo
                HashLow = New clsAlertInfo
                TempHigh = New clsAlertInfo
                XCount = New clsAlertInfo
            End Sub
        End Class

        Public Class clsMinerInfo
            Private sShortName As String
            Private sLongName As String
            Private xMinerType As clsSupportedMinerInfo.enSupportedMinerTypes
            Private xAlertTypes As stAlertTypes
            Private xAlerts As clsAlerts

            Private xSpeedType As enSpeedType
            Private xSpeedTypeString As String

            Private xHashType As enHashType

            Private ctlsByKey As ControlsByRegistry

            Public ReadOnly Property SpeedTypeString As String
                Get
                    Return xSpeedTypeString
                End Get
            End Property

            Public ReadOnly Property HashType As enHashType
                Get
                    Return xHashType
                End Get
            End Property

            Public ReadOnly Property SpeedType As enSpeedType
                Get
                    Return xSpeedType
                End Get
            End Property

            Public ReadOnly Property ShortName As String
                Get
                    Return Me.sShortName
                End Get
            End Property

            Public ReadOnly Property LongName As String
                Get
                    Return Me.sLongName
                End Get
            End Property

            Public ReadOnly Property MinerType As clsSupportedMinerInfo.enSupportedMinerTypes
                Get
                    Return Me.xMinerType
                End Get
            End Property

            Public ReadOnly Property AlertTypes As stAlertTypes
                Get
                    Return xAlertTypes
                End Get
            End Property

            Public ReadOnly Property AlertValues As clsAlerts
                Get
                    Return xAlerts
                End Get
            End Property

            Public Sub New(ByVal MinerType As clsSupportedMinerInfo.enSupportedMinerTypes)

                Me.xMinerType = MinerType
                Me.xAlerts = New clsAlerts

                Select Case Me.xSpeedType
                    Case enSpeedType.GHs
                        Me.xSpeedTypeString = "GH/s"

                    Case enSpeedType.MHs
                        Me.xSpeedTypeString = "MH/s"

                    Case enSpeedType.THs
                        Me.xSpeedTypeString = "TH/s"

                End Select

                Select Case MinerType
                    Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1
                        Me.sShortName = "C1"
                        Me.sLongName = "Antminer C1"

                        Me.xAlertTypes.Fans = True
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = True

                        With Me.xAlerts
                            .FanHigh.Initialize("AlertIfC1Fan", "AlertValueC1Fan", "", False)
                            .FanLow.Initialize("AlertIfC1FanLow", "AlertValueC1FanLow", "", False)
                            .TempHigh.Initialize("AlertIfC1Temp", "AlertValueC1Temp", "", False)
                            .HashLow.Initialize("AlertIfC1Hash", "AlertValueC1Hash", "", False)
                            .XCount.Initialize("AlertIfC1XCount", "AlertValueC1XCount", "", False)
                        End With

                        Me.xSpeedType = enSpeedType.GHs

                        Me.xHashType = enHashType.SHA256

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS1
                        Me.sShortName = "S1"
                        Me.sLongName = "Antminer S1"

                        Me.xAlertTypes.Fans = True
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = True

                        With Me.xAlerts
                            .FanHigh.Initialize("AlertIfS1Fan", "AlertValueS1Fan", "", False)
                            .FanLow.Initialize("AlertIfS1FanLow", "AlertValueS1FanLow", "", False)
                            .TempHigh.Initialize("AlertIfS1Temp", "AlertValueS1Temp", "", False)
                            .HashLow.Initialize("AlertIfS1Hash", "AlertValueS1Hash", "", False)
                            .XCount.Initialize("AlertIfS1XCount", "AlertValueS1XCount", "", False)
                        End With

                        Me.xSpeedType = enSpeedType.GHs

                        Me.xHashType = enHashType.SHA256

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2
                        Me.sShortName = "S2"
                        Me.sLongName = "Antminer S2"

                        Me.xAlertTypes.Fans = True
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = True

                        With Me.xAlerts
                            .FanHigh.Initialize("AlertIfS2Fan", "AlertValueS2Fan", "", False)
                            .FanLow.Initialize("AlertIfS2FanLow", "AlertValueS2FanLow", "", False)
                            .TempHigh.Initialize("AlertIfS2Temp", "AlertValueS2Temp", "", False)
                            .HashLow.Initialize("AlertIfS2Hash", "AlertValueS2Hash", "", False)
                            .XCount.Initialize("AlertIfS2XCount", "AlertValueS2XCount", "", False)
                        End With

                        Me.xSpeedType = enSpeedType.GHs

                        Me.xHashType = enHashType.SHA256

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS3
                        Me.sShortName = "S3"
                        Me.sLongName = "Antminer S3"

                        Me.xAlertTypes.Fans = True
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = True

                        With Me.xAlerts
                            .FanHigh.Initialize("AlertIfS3Fan", "AlertValueS3Fan", "", False)
                            .FanLow.Initialize("AlertIfS3FanLow", "AlertValueS3FanLow", "", False)
                            .TempHigh.Initialize("AlertIfS3Temp", "AlertValueS3Temp", "", False)
                            .HashLow.Initialize("AlertIfS3Hash", "AlertValueS3Hash", "", False)
                            .XCount.Initialize("AlertIfS3XCount", "AlertValueS3XCount", "", False)
                        End With

                        Me.xSpeedType = enSpeedType.GHs

                        Me.xHashType = enHashType.SHA256

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4
                        Me.sShortName = "S4"
                        Me.sLongName = "Antminer S4"

                        Me.xAlertTypes.Fans = True
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = True

                        With Me.xAlerts
                            .FanHigh.Initialize("AlertIfS4Fan", "AlertValueS4Fan", "", False)
                            .FanLow.Initialize("AlertIfS4FanLow", "AlertValueS4FanLow", "", False)
                            .TempHigh.Initialize("AlertIfS4Temp", "AlertValueS4Temp", "", False)
                            .HashLow.Initialize("AlertIfS4Hash", "AlertValueS4Hash", "", False)
                            .XCount.Initialize("AlertIfS4XCount", "AlertValueS4XCount", "", False)
                        End With

                        Me.xSpeedType = enSpeedType.GHs

                        Me.xHashType = enHashType.SHA256

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.AntminerS5
                        Me.sShortName = "S5"
                        Me.sLongName = "Antminer S5"

                        Me.xAlertTypes.Fans = True
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = True

                        With Me.xAlerts
                            .FanHigh.Initialize("AlertIfS5Fan", "AlertValueS5Fan", "", False)
                            .FanLow.Initialize("AlertIfS5FanLow", "AlertValueS5FanLow", "", False)
                            .TempHigh.Initialize("AlertIfS5Temp", "AlertValueS5Temp", "", False)
                            .HashLow.Initialize("AlertIfS5Hash", "AlertValueS5Hash", "", False)
                            .XCount.Initialize("AlertIfS5XCount", "AlertValueS5XCount", "", False)
                        End With

                        Me.xSpeedType = enSpeedType.GHs

                        Me.xHashType = enHashType.SHA256

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP10
                        Me.sShortName = "SP10"
                        Me.sLongName = "Spondoolies SP10"

                        Me.xAlertTypes.Fans = False
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = True

                        With Me.xAlerts
                            .TempHigh.Initialize("AlertIfSP10Temp", "AlertValueSP10Temp", "", False)
                            .HashLow.Initialize("AlertIfSP10Hash", "AlertValueSP10Hash", "", False)
                        End With

                        Me.xSpeedType = enSpeedType.GHs

                        Me.xHashType = enHashType.SHA256

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP20
                        Me.sShortName = "SP20"
                        Me.sLongName = "Spondoolies SP20"

                        Me.xAlertTypes.Fans = False
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = True

                        With Me.xAlerts
                            .TempHigh.Initialize("AlertIfSP20Temp", "AlertValueSP20Temp", "", False)
                            .HashLow.Initialize("AlertIfSP20Hash", "AlertValueSP20Hash", "", False)
                        End With

                        Me.xSpeedType = enSpeedType.GHs

                        Me.xHashType = enHashType.SHA256

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP30
                        Me.sShortName = "SP30"
                        Me.sLongName = "Spondoolies SP30"

                        Me.xAlertTypes.Fans = False
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = True

                        With Me.xAlerts
                            .TempHigh.Initialize("AlertIfSP30Temp", "AlertValueSP30Temp", "", False)
                            .HashLow.Initialize("AlertIfSP30Hash", "AlertValueSP30Hash", "", False)
                        End With

                        Me.xSpeedType = enSpeedType.GHs

                        Me.xHashType = enHashType.SHA256

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP31
                        Me.sShortName = "SP31"
                        Me.sLongName = "Spondoolies SP31"

                        Me.xAlertTypes.Fans = False
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = True

                        With Me.xAlerts
                            .TempHigh.Initialize("AlertIfSP31Temp", "AlertValueSP31Temp", "", False)
                            .HashLow.Initialize("AlertIfSP31Hash", "AlertValueSP31Hash", "", False)
                        End With

                        Me.xSpeedType = enSpeedType.GHs

                        Me.xHashType = enHashType.SHA256

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP35
                        Me.sShortName = "SP35"
                        Me.sLongName = "Spondoolies SP35"

                        Me.xAlertTypes.Fans = False
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = True

                        With Me.xAlerts
                            .TempHigh.Initialize("AlertIfSP35Temp", "AlertValueSP35Temp", "", False)
                            .HashLow.Initialize("AlertIfSP35Hash", "AlertValueSP35Hash", "", False)
                        End With

                        Me.xSpeedType = enSpeedType.GHs

                        Me.xHashType = enHashType.SHA256

                    Case enSupportedMinerTypes.GridseedGBlack
                        Me.sShortName = "GDSDGB"
                        Me.sLongName = "Gridseed G-Black"

                        Me.xAlertTypes.Fans = False
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = False

                        Me.xSpeedType = enSpeedType.MHs

                        Me.xHashType = enHashType.Scrypt

                    Case enSupportedMinerTypes.InnosiliconA288MH
                        Me.sShortName = "INSA288"
                        Me.sLongName = "Innosilicon A2 88MH"

                        Me.xAlertTypes.Fans = False
                        Me.xAlertTypes.Hash = True
                        Me.xAlertTypes.Temps = True
                        Me.xAlertTypes.XCount = False

                        Me.xSpeedType = enSpeedType.MHs

                        Me.xHashType = enHashType.Scrypt

                    Case Else
                        Throw New Exception("Internal error: miner type not found")

                End Select

            End Sub
        End Class

        Public AntMinerS1 As clsMinerInfo
        Public AntMinerS2 As clsMinerInfo
        Public AntMinerS3 As clsMinerInfo
        Public AntMinerS4 As clsMinerInfo
        Public AntMinerS5 As clsMinerInfo
        Public AntMinerC1 As clsMinerInfo
        Public SpondoolieSP10 As clsMinerInfo
        Public SpondoolieSP20 As clsMinerInfo
        Public SpondoolieSP30 As clsMinerInfo
        Public SpondoolieSP31 As clsMinerInfo
        Public SpondoolieSP35 As clsMinerInfo
        Public GridSeedGBlack As clsMinerInfo
        Public InnosiloconA288 As clsMinerInfo

        Public SupportedMinerCollection As System.Collections.Generic.List(Of clsMinerInfo)

        Public Sub New()

            AntMinerC1 = New clsMinerInfo(enSupportedMinerTypes.AntMinerC1)
            AntMinerS1 = New clsMinerInfo(enSupportedMinerTypes.AntMinerS1)
            AntMinerS2 = New clsMinerInfo(enSupportedMinerTypes.AntMinerS2)
            AntMinerS3 = New clsMinerInfo(enSupportedMinerTypes.AntMinerS3)
            AntMinerS4 = New clsMinerInfo(enSupportedMinerTypes.AntMinerS4)
            AntMinerS5 = New clsMinerInfo(enSupportedMinerTypes.AntminerS5)
            SpondoolieSP10 = New clsMinerInfo(enSupportedMinerTypes.SpondooliesSP10)
            SpondoolieSP20 = New clsMinerInfo(enSupportedMinerTypes.SpondooliesSP20)
            SpondoolieSP30 = New clsMinerInfo(enSupportedMinerTypes.SpondooliesSP30)
            SpondoolieSP31 = New clsMinerInfo(enSupportedMinerTypes.SpondooliesSP31)
            SpondoolieSP35 = New clsMinerInfo(enSupportedMinerTypes.SpondooliesSP35)
            GridSeedGBlack = New clsMinerInfo(enSupportedMinerTypes.GridseedGBlack)
            InnosiloconA288 = New clsMinerInfo(enSupportedMinerTypes.InnosiliconA288MH)

            SupportedMinerCollection = New System.Collections.Generic.List(Of clsMinerInfo)
            SupportedMinerCollection.Add(Me.AntMinerS1)
            SupportedMinerCollection.Add(Me.AntMinerS2)
            SupportedMinerCollection.Add(Me.AntMinerS3)
            SupportedMinerCollection.Add(Me.AntMinerS4)
            SupportedMinerCollection.Add(Me.AntMinerS5)
            SupportedMinerCollection.Add(Me.AntMinerC1)
            SupportedMinerCollection.Add(Me.SpondoolieSP10)
            SupportedMinerCollection.Add(Me.SpondoolieSP20)
            SupportedMinerCollection.Add(Me.SpondoolieSP30)
            SupportedMinerCollection.Add(Me.SpondoolieSP31)
            SupportedMinerCollection.Add(Me.SpondoolieSP35)
            SupportedMinerCollection.Add(Me.GridSeedGBlack)
            SupportedMinerCollection.Add(Me.InnosiloconA288)

        End Sub

        Public Function GetMinerObjectByShortName(ByVal sShortName As String) As clsMinerInfo

            Dim mi As clsMinerInfo

            For Each mi In Me.SupportedMinerCollection
                If mi.ShortName.ToLower = sShortName.ToLower Then
                    Return mi
                End If
            Next

            Throw New Exception("Internal error: No match on ShortName in GetMinerObjectByShortName: " & sShortName)

        End Function

        Public Function GetMinerObjectByLongName(ByVal sLongName As String) As clsMinerInfo

            Dim mi As clsMinerInfo

            For Each mi In Me.SupportedMinerCollection
                If mi.LongName.ToLower = sLongName.ToLower Then
                    Return mi
                End If
            Next

            Throw New Exception("Internal error: No match on LongName in GetMinerObjectByLongName: " & sLongName)

        End Function

        Public Function GetMinerObjectByType(ByVal minerType As clsSupportedMinerInfo.enSupportedMinerTypes) As clsMinerInfo

            Dim mi As clsMinerInfo

            For Each mi In Me.SupportedMinerCollection
                If mi.MinerType = minerType Then
                    Return mi
                End If
            Next

            Throw New Exception("Internal error: No match on MinerType in GetMinerObjectByType: " & minerType)

        End Function

    End Class

    Private SupportedMinerInfo As clsSupportedMinerInfo

    'log queue from other threads
    Private Shared logQueue As System.Collections.Generic.Queue(Of String)
    Private Shared logQueueLock As Object

    'queue of ants to check
    Private Shared minersToCheckQueue As System.Collections.Generic.Queue(Of stMinerConfig)
    Private Shared minersToCheckLock As Object

    'data coming back from the worker threads with Miner refresh data
    Private Shared MinerRefreshDataQueue As System.Collections.Generic.Queue(Of clsMinerRefreshData)
    Private Shared MinerRefreshLock As Object

    'display refresh period after miner refresh is initiated
    Private iDisplayRefreshPeriod As Integer

    'object populated by the worker threads that is passed back to the UI thread for grid population
    Private Class clsMinerRefreshData
        'Public MinerType As clsSupportedMinerInfo.enSupportedMinerTypes
        Public MinerInfo As clsSupportedMinerInfo.clsMinerInfo
        Public ID As Integer
        Public sStats As String
        Public sSummary As String
        Public sPools As String
        Public sMinerIP As String
        Public sConfig As String
        Public bError As Boolean
        Public ex As Exception
    End Class

    'miner data from the config
    Private Structure stMinerConfig
        Dim sName As String
        Dim sIP As String
        Dim MinerInfo As clsSupportedMinerInfo.clsMinerInfo
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
    Private iMinersEnabled As Integer

    'worker threads and data
    Private workerThread() As System.Threading.Thread
    Private ThreadHandlers() As clsThreadHandler

    Private Class clsThreadHandler
        Public bBusy As Boolean
        Public MinerToCheck As stMinerConfig
        Public bGotWork As Boolean
    End Class

    'set to true when shutting down
    Private bShutDown As Boolean

    'pool data for pushing pool data to ants
    Private Class clsPoolData
        Public URL As String
        Public UID As String
        Public PW As String
        Public PoolIsUp As Boolean

        Public Sub New()
            PW = ""
        End Sub
    End Class

    'for miner scanning on another thread
    Private sIPDataResponse As String
    Private sIPToCheck As String

    'for get miner info on another thread
    Private sIPToGetInfo As String
    Private Shared listOfGetInfoResponse As System.Collections.Generic.List(Of String)

    'for tracking scheduled reboots
    Private dictScheduledReboots As System.Collections.Generic.Dictionary(Of Integer, Date)

    'for tracking reboot at
    Private dictRebootAt As System.Collections.Generic.Dictionary(Of Integer, Date)

    'for tracking reboot at also
    Private dictRebootAtAlso As System.Collections.Generic.Dictionary(Of Integer, Date)

    'reference to display grid, prevents typos from entering a column name wrong
    Private Class clsDisplayColumns

        Private xDatarow As DataRow

        Public Sub New(ByRef dr As DataRow)

            Me.xDatarow = dr

        End Sub

        Public Property Uptime As String
            Set(value As String)
                Me.xDatarow.Item("Uptime") = value
            End Set
            Get
                Return Me.xDatarow.Item("Uptime")
            End Get
        End Property

        Public Property Speed5s As String
            Set(value As String)
                Me.xDatarow.Item("Speed(5s)") = value
            End Set
            Get
                Return Me.xDatarow.Item("Speed(5s)")
            End Get
        End Property

        Public Property SpeedAvg As String
            Set(value As String)
                Me.xDatarow.Item("Speed(avg)") = value
            End Set
            Get
                Return Me.xDatarow.Item("Speed(avg)")
            End Get
        End Property

        Public Property Blocks As String
            Set(value As String)
                Me.xDatarow.Item("Blocks") = value
            End Set
            Get
                Return Me.xDatarow.Item("Blocks")
            End Get
        End Property

        Public Property HWE As String
            Set(value As String)
                Me.xDatarow.Item("HWE%") = value
            End Set
            Get
                Return Me.xDatarow.Item("HWE%")
            End Get
        End Property

        Public Property BestShare As String
            Set(value As String)
                Me.xDatarow.Item("BestShare") = value
            End Set
            Get
                Return Me.xDatarow.Item("BestShare")
            End Get
        End Property

        Public Property Diff As String
            Set(value As String)
                Me.xDatarow.Item("Diff") = value
            End Set
            Get
                Return Me.xDatarow.Item("Diff")
            End Get
        End Property

        Public Property Pools As String
            Set(value As String)
                Me.xDatarow.Item("Pools") = value
            End Set
            Get
                Return Me.xDatarow.Item("Pools")
            End Get
        End Property

        Public Property PoolData As String
            Set(value As String)
                Me.xDatarow.Item("PoolData") = value
            End Set
            Get
                Return Me.xDatarow.Item("PoolData")
            End Get
        End Property

        Public Property PoolData2 As Object
            Set(value As Object)
                Me.xDatarow.Item("PoolData2") = value
            End Set
            Get
                Return Me.xDatarow.Item("PoolData2")
            End Get
        End Property

        Public Property Rej As String
            Set(value As String)
                Me.xDatarow.Item("Rej%") = value
            End Set
            Get
                Return Me.xDatarow.Item("Rej%")
            End Get
        End Property

        Public Property Stale As String
            Set(value As String)
                Me.xDatarow.Item("Stale%") = value
            End Set
            Get
                Return Me.xDatarow.Item("Stale%")
            End Get
        End Property

        Public Property HFan As Object
            Set(value As Object)
                Me.xDatarow.Item("HFan") = value
            End Set
            Get
                Return Me.xDatarow.Item("HFan")
            End Get
        End Property

        Public Property Fans As String
            Set(value As String)
                Me.xDatarow.Item("Fans") = value
            End Set
            Get
                Return Me.xDatarow.Item("Fans")
            End Get
        End Property

        Public Property HTemp As Integer
            Set(value As Integer)
                Me.xDatarow.Item("HTemp") = value
            End Set
            Get
                Return Me.xDatarow.Item("HTemp")
            End Get
        End Property

        Public Property ATemp As Integer
            Set(value As Integer)
                Me.xDatarow.Item("ATemp") = value
            End Set
            Get
                Return Me.xDatarow.Item("ATemp")
            End Get
        End Property

        Public Property Temps As String
            Set(value As String)
                Me.xDatarow.Item("Temps") = value
            End Set
            Get
                Return Me.xDatarow.Item("Temps")
            End Get
        End Property

        Public Property Freq As Object
            Set(value As Object)
                Me.xDatarow.Item("Freq") = value
            End Set
            Get
                Return Me.xDatarow.Item("Freq")
            End Get
        End Property

        Public Property XCount As String
            Set(value As String)
                Me.xDatarow.Item("XCount") = value
            End Set
            Get
                Return Me.xDatarow.Item("XCount")
            End Get
        End Property

        Public Property Status As String
            Set(value As String)
                Me.xDatarow.Item("Status") = value
            End Set
            Get
                Return Me.xDatarow.Item("Status")
            End Get
        End Property

        Public Property ACount As Integer
            Set(value As Integer)
                Me.xDatarow.Item("ACount") = value
            End Set
            Get
                Return Me.xDatarow.Item("ACount")
            End Get
        End Property

        Public Property ID As String
            Set(value As String)
                Me.xDatarow.Item("ID") = value
            End Set
            Get
                Return Me.xDatarow.Item("ID")
            End Get
        End Property

        Public Property Type As String
            Set(value As String)
                Me.xDatarow.Item("Type") = value
            End Set
            Get
                Return Me.xDatarow.Item("Type")
            End Get
        End Property

        Public Property HashSHA256 As Double
            Set(value As Double)
                Me.xDatarow.Item("HashSHA256") = value
            End Set
            Get
                Return Me.xDatarow.Item("HashSHA256")
            End Get
        End Property

        Public Property HashScrypt As Double
            Set(value As Double)
                Me.xDatarow.Item("HashScrypt") = value
            End Set
            Get
                Return Me.xDatarow.Item("HashScrypt")
            End Get
        End Property

        Public Property Data As Object
            Set(value As Object)
                Me.xDatarow.Item("Data") = value
            End Set
            Get
                Return Me.xDatarow.Item("Data")
            End Get
        End Property

        Public Property Queue As String
            Set(value As String)
                Me.xDatarow.Item("Queue") = value
            End Set
            Get
                Return Me.xDatarow.Item("Queue")
            End Get
        End Property

        Public Property Name As String
            Set(value As String)
                Me.xDatarow.Item("Name") = value
            End Set
            Get
                Return Me.xDatarow.Item("Name")
            End Get
        End Property

        Public Property MobileMinerHash As Double
            Set(value As Double)
                Me.xDatarow.Item("CurHash") = value
            End Set
            Get
                Return Me.xDatarow.Item("CurHash")
            End Get
        End Property

    End Class

#If DEBUG Then
    Private bErrorHandle As Boolean = False
#Else
    Private bErrorHandle As Boolean = True
#End If

    Private Sub Form1_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim host As System.Net.IPHostEntry
        Dim x As Integer
        Dim s() As String
        Dim dr As DataRow
        Dim MinerInfo As clsSupportedMinerInfo.clsMinerInfo
        Dim bTemp As Boolean

        bStarted = True

        Me.Text = csVersion

        'initialize objects
        logQueue = New System.Collections.Generic.Queue(Of String)
        logQueueLock = New Object

        SupportedMinerInfo = New clsSupportedMinerInfo

        minersToCheckQueue = New System.Collections.Generic.Queue(Of stMinerConfig)
        minersToCheckLock = New Object

        MinerRefreshDataQueue = New System.Collections.Generic.Queue(Of clsMinerRefreshData)
        MinerRefreshLock = New Object

        wbData(0) = New clsWBData
        wbData(1) = New clsWBData
        wbData(2) = New clsWBData

        dictScheduledReboots = New System.Collections.Generic.Dictionary(Of Integer, Date)
        dictRebootAt = New System.Collections.Generic.Dictionary(Of Integer, Date)
        dictRebootAtAlso = New System.Collections.Generic.Dictionary(Of Integer, Date)

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

        For Each SupportMiner As clsSupportedMinerInfo.clsMinerInfo In SupportedMinerInfo.SupportedMinerCollection
            Me.cmbMinerType.Items.Add(SupportMiner.LongName)
            Me.cmbAlertMinerType.Items.Add(SupportMiner.LongName)
        Next

        RebootInfo = New System.Collections.Generic.Dictionary(Of String, Date)
        EMailAlertInfo = New System.Collections.Generic.Dictionary(Of String, Date)

        'ant output
        ds = New DataSet

        With ds
            .Tables.Add()
            Me.dataMiners.DataSource = .Tables(0)

            With .Tables(0).Columns
                .Add("Name")
                .Add("Uptime")
                .Add("Speed(5s)")
                .Add("Speed(avg)")
                .Add("Blocks")
                .Add("HWE%")
                .Add("BestShare", GetType(String))
                .Add("Diff")
                .Add("Pools")
                .Add("PoolData")
                .Add("PoolData2", GetType(Object))
                .Add("Rej%")
                .Add("Stale%")
                .Add("Queue")
                .Add("HFan", GetType(Integer))
                .Add("Fans")
                .Add("HTemp", GetType(Integer))
                .Add("ATemp", GetType(Integer))
                .Add("Temps")
                .Add("Freq", GetType(Double))
                .Add("XCount")
                .Add("Status")
                .Add("ACount", GetType(Integer))
                .Add("ID")
                .Add("Type")
                .Add("CurHash", GetType(Double))
                .Add("HashSHA256", GetType(Double))
                .Add("HashScrypt", GetType(Double))
                .Add("Data", GetType(Object))
            End With
        End With

        Me.dataMiners.Columns("PoolData").Visible = False
        Me.dataMiners.Columns("PoolData2").Visible = False
        Me.dataMiners.Columns("Type").Visible = False
        Me.dataMiners.Columns("CurHash").Visible = False
        Me.dataMiners.Columns("HashSHA256").Visible = False
        Me.dataMiners.Columns("HashScrypt").Visible = False
        Me.dataMiners.Columns("Data").Visible = False

        Me.dataMiners.Columns("HWE%").ToolTipText = "Hardware Error Percentage"
        Me.dataMiners.Columns("Diff").ToolTipText = "Difficulty Miner is using.  For web scraping, it's the value from all 3 pools."
        Me.dataMiners.Columns("HFan").ToolTipText = "Highest Fan Speed"
        Me.dataMiners.Columns("Rej%").ToolTipText = "Reject Percentage"
        Me.dataMiners.Columns("HTemp").ToolTipText = "Highest Temperature across all blades"
        Me.dataMiners.Columns("Freq").ToolTipText = "Frequency Miner is running at"
        Me.dataMiners.Columns("XCount").ToolTipText = "Number of Xs this Miner has"
        Me.dataMiners.Columns("ACount").ToolTipText = "Alert count for this Miner"
        Me.dataMiners.Columns("ATemp").ToolTipText = "Average temperature for all sensors for this miner"

        With Me.dataMiners
            .Columns("ACount").Width = 74
            .Columns("Blocks").Width = 65
            .Columns("Diff").Width = 48
            .Columns("Freq").Width = 62
            .Columns("Speed(5s)").Width = 87
            .Columns("Speed(avg)").Width = 92
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

        SetupMobileMinerTabs()

        ctlsByKey = New ControlsByRegistry(csRegKey)

        Call SetGridSizes("\Columns\dataMiners", Me.dataMiners)
        Call SetGridColumnPositions("\Columns\" & Me.dataMiners.Name & "_DisplayIndex", Me.dataMiners)

        'handles saving of column widths and column locations
        AddHandler Me.dataMiners.ColumnWidthChanged, AddressOf Me.dataGrid_ColumnWidthChanged
        AddHandler Me.dataMiners.ColumnDisplayIndexChanged, AddressOf Me.dataMiners_ColumnDisplayIndexChanged

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
            .AddControl(Me.chkShowQueue, "ShowQueue")
            .AddControl(Me.chkShowATemp, "ShowATemp")

            .AddControl(Me.chkShowSelectionColumn, "ShowSelectionColumn")

            .AddControl(Me.trackThreadCount, "WorkerThreadCount")
            .AddControl(Me.txtDisplayRefreshInSecs, "DisplayRefreshPeriod")

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

            'reboots
            .AddControl(Me.chkAlertRebootIfXd, "RebootAntIfXd")
            .AddControl(Me.chkAlertRebootAntsOnHashAlert, "RebootAntIfHashAlert")
            .AddControl(Me.chkRebootAntOnError, "RebootAntIfError")
            .AddControl(Me.txtAlertRebootGovernor, "AlertRebootGovernor")
            .AddControl(Me.cmbAlertRebootGovernor, "AlertRebootGovernorValue")

            .AddControl(Me.chkRebootAllAntsAt, "RebootAllAntsAtOnOff")
            .AddControl(Me.txtRebootAllAntsAt, "RebootAllAntsAtValue")
            .AddControl(Me.cmbRebootAllAntsAt, "RebootAllAntsAtAMPM")

            .AddControl(Me.chkRebootAllAntsAtAlso, "RebootAllAntsAtAlsoOnOff")
            .AddControl(Me.txtRebootAllAntsAtAlso, "RebootAllAntsAtAlsoValue")
            .AddControl(Me.cmbRebootAllAntsAtAlso, "RebootAllAntsAtAlsoAMPM")

            .AddControl(Me.chkRebootAntsByUptime, "RebootAntsByUptimeOnOff")
            .AddControl(Me.txtRebootAntsByUptime, "RebootAntsByUptimeValue")
            .AddControl(Me.cmbRebootAntsByUptime, "RebootAntsByUptimeSecMinHour")

            ' MobileMiner settings
            .AddControl(Me.txtMMEmail, "MobileMinerEmail")
            .AddControl(Me.txtMMAppKey, "MobileMinerAppKey")
            .AddControl(Me.chkMMDashHistCheck, "MobileMinerDashHist")

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
            .SetControlByRegKey(Me.chkShowQueue, True)
            .SetControlByRegKey(Me.chkShowATemp, True)

            .SetControlByRegKey(Me.chkShowSelectionColumn)

            .SetControlByRegKey(Me.trackThreadCount, 5)
            .SetControlByRegKey(Me.txtDisplayRefreshInSecs, "1")

            Call txtDisplayRefreshInSecs_Leave(sender, e)

            .SetControlByRegKey(Me.chkAlertHighlightField, True)
            .SetControlByRegKey(Me.chkAlertShowNotifyPopup, True)
            .SetControlByRegKey(Me.chkAlertShowAnnoyingPopup)
            .SetControlByRegKey(Me.chkAlertStartProcess)
            .SetControlByRegKey(Me.txtAlertStartProcessName)
            .SetControlByRegKey(Me.txtAlertStartProcessParms)
            .SetControlByRegKey(Me.chkAlertSendEMail)

            'reboots
            .SetControlByRegKey(Me.chkAlertRebootIfXd, True)
            .SetControlByRegKey(Me.chkAlertRebootAntsOnHashAlert)
            .SetControlByRegKey(Me.chkRebootAntOnError)
            .SetControlByRegKey(Me.txtAlertRebootGovernor, 30)
            .SetControlByRegKey(Me.cmbAlertRebootGovernor, "Minutes")

            .SetControlByRegKey(Me.chkRebootAllAntsAt)
            .SetControlByRegKey(Me.txtRebootAllAntsAt)
            .SetControlByRegKey(Me.cmbRebootAllAntsAt)

            .SetControlByRegKey(Me.chkRebootAllAntsAtAlso)
            .SetControlByRegKey(Me.txtRebootAllAntsAtAlso)
            .SetControlByRegKey(Me.cmbRebootAllAntsAtAlso)

            .SetControlByRegKey(Me.chkRebootAntsByUptime)
            .SetControlByRegKey(Me.txtRebootAntsByUptime)
            .SetControlByRegKey(Me.cmbRebootAntsByUptime)

            'MobileMiner settings
            .SetControlByRegKey(Me.txtMMEmail)
            .SetControlByRegKey(Me.txtMMAppKey)
            .SetControlByRegKey(Me.chkMMDashHistCheck)

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

        'convert SP20 values that were generic under SP to SP20
        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey)
            For Each sKey As String In key.GetValueNames
                If sKey = "AlertIfSP20Hash" Then
                    bTemp = True

                    Exit For
                End If
            Next

            If bTemp = False Then
                If key.GetValue("AlertIfSPHash") IsNot Nothing Then
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, "AlertIfSP20Hash", key.GetValue("AlertIfSPHash"))
                Else
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, "AlertIfSP20Hash", "N")
                End If

                If key.GetValue("AlertIfSPTemp") IsNot Nothing Then
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, "AlertIfSP20Temp", key.GetValue("AlertIfSPTemp"))
                Else
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, "AlertIfSP20Temp", "")
                End If

                If key.GetValue("AlertValueSPHash") IsNot Nothing Then
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, "AlertValueSP20Hash", key.GetValue("AlertValueSPHash"))
                Else
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, "AlertValueSP20Hash", "N")
                End If

                If key.GetValue("AlertValueSPTemp") IsNot Nothing Then
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, "AlertValueSP20Temp", key.GetValue("AlertValueSPTemp"))
                Else
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey, "AlertValueSP20Temp", "")
                End If

                Using key2 As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\AntsV2")
                    If key2 Is Nothing Then
                        My.Computer.Registry.CurrentUser.CreateSubKey(csRegKey & "\AntsV2")
                    End If
                End Using

                'iterate through miner entries and change those with type SP to SP20
                Using key2 As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\AntsV2")
                    For Each sKey As String In key2.GetSubKeyNames
                        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "Type", "") = "SP" Then
                            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "Type", "SP20")
                        End If
                    Next
                End Using
            End If
        End Using

        'config
        dsMinerConfig = New DataSet

        With dsMinerConfig
            .Tables.Add()
            Me.dataMinerConfig.DataSource = .Tables(0)

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
                .Add("DoNotReboot")
                .Add("HTTPS")
            End With
        End With

        Me.dataMinerConfig.Columns("APIPort").Visible = False
        Me.dataMinerConfig.Columns("HTTPPort").Visible = False
        Me.dataMinerConfig.Columns("SSHPort").Visible = False
        Me.dataMinerConfig.Columns("SSHPassword").Visible = False
        Me.dataMinerConfig.Columns("SSHUsername").Visible = False
        Me.dataMinerConfig.Columns("WebUsername").Visible = False
        Me.dataMinerConfig.Columns("WebPassword").Visible = False
        Me.dataMinerConfig.Columns("UseAPI").Visible = False
        Me.dataMinerConfig.Columns("RebootViaSSH").Visible = False
        Me.dataMinerConfig.Columns("DoNotReboot").Visible = False
        Me.dataMinerConfig.Columns("HTTPS").Visible = False

        With Me.dataMinerConfig
            .Columns("Active").Width = 62
            .Columns("ID").Width = 39
            .Columns("IPAddress").Width = 115
            .Columns("Name").Width = 76
            .Columns("Type").Width = 46
        End With

        'load Miners into the config
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
                                If sKey.Substring(0, 4) = "S1: " OrElse sKey.Substring(0, 4) = "S2: " OrElse sKey.Substring(0, 4) = "S3: " OrElse _
                                    sKey.Substring(0, 4) = "C1: " OrElse sKey.Substring(0, 4) = "SP: " Then

                                    MinerInfo = SupportedMinerInfo.GetMinerObjectByShortName(sKey.Substring(0, 2))

                                    s = sKey.Split(".")

                                    Call AddOrSaveMiner(-1, sKey.Substring(0, 3) & s(2) & "." & s(3), MinerInfo, sKey.Substring(4), , My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sKey, "Port", "80"), _
                                                    My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sKey, "WebUsername", "root"), _
                                                    My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\Ants\" & sKey, "WebPassword", "root"), _
                                                    , , , , My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey, "UseAPI", "Y"), _
                                                    My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey, "UseAPI", "Y"))
                                End If
                            End If
                        Next
                    End If
                End Using
            End If
        End Using

        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\AntsV2")
            For Each sKey As String In key.GetSubKeyNames
                dr = Me.dsMinerConfig.Tables(0).NewRow

                dr("Name") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "Name", "")
                dr("Type") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "Type", "")
                dr("IPAddress") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "IPAddress", "")
                dr("Active") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "Active", "")

                If dr("Active") = "Y" Then
                    iMinersEnabled += 1
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
                dr("DoNotReboot") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "DoNotReboot", "")
                dr("HTTPS") = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & sKey, "HTTPS", "")

                dsMinerConfig.Tables(0).Rows.Add(dr)
            Next
        End Using

        Call SetGridSizes("\Columns\dataAntConfig", Me.dataMinerConfig)
        Call SetGridColumnPositions("\Columns\" & Me.dataMinerConfig.Name & "_DisplayIndex", Me.dataMinerConfig)

        AddHandler Me.dataMinerConfig.ColumnWidthChanged, AddressOf Me.dataGrid_ColumnWidthChanged
        AddHandler Me.dataMinerConfig.ColumnDisplayIndexChanged, AddressOf Me.dataMiners_ColumnDisplayIndexChanged

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
        If iMinersEnabled = 0 Then
            MsgBox("Please add some active miner addresses first." & vbCrLf & vbCrLf & "You can also use the scan feature to auto detect your supported miners.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

            Me.TabControl1.SelectTab(1)

            Exit Sub
        Else
            Call cmdRefresh_Click(sender, e)

            Me.TimerRefresh.Enabled = True
        End If

        SetupMobileMinerTimer()

    End Sub

    Private momDashTabPage As TabPage
    Private momHistTabPage As TabPage
    Private momTabsSetup As Boolean

    Private Sub SetupMobileMinerTabs()
        momDashTabPage = New TabPage("Dashboard")
        momHistTabPage = New TabPage("History")
        momTabsSetup = True

        SetupMobileMinerTabVisibility()
    End Sub

    Private Sub SetupMobileMinerTabVisibility()
        If momTabsSetup Then
            If chkMMDashHistCheck.Checked Then
                Me.TabControl1.TabPages.Add(momDashTabPage)
                Me.TabControl1.TabPages.Add(momHistTabPage)
            Else
                Me.TabControl1.TabPages.Remove(momDashTabPage)
                Me.TabControl1.TabPages.Remove(momHistTabPage)
            End If
        End If
    End Sub

    Private Sub SetupMobileMinerTimer()
        Me.mobileMinerTimer.Interval = 35000
        RemoveHandler Me.mobileMinerTimer.Tick, AddressOf mobileMinerTimer_Tick
        AddHandler Me.mobileMinerTimer.Tick, AddressOf mobileMinerTimer_Tick
        Me.mobileMinerTimer.Enabled = True
    End Sub

    Private Function AddOrSaveMiner(ByVal ID As Integer, ByVal sMinerName As String, ByVal MinerInfo As clsSupportedMinerInfo.clsMinerInfo, ByVal sIPAddress As String, _
                          Optional ByVal sActive As String = "Y", Optional ByVal sHTTPPort As String = "80", _
                          Optional ByVal sWebUserName As String = "root", Optional ByVal sWebPassword As String = "root", _
                          Optional ByVal sSSHUserName As String = "root", Optional ByVal sSSHPassword As String = "", _
                          Optional ByVal sAPIPort As String = "4028", Optional ByVal sSSHPort As String = "22", _
                          Optional ByVal sUseAPI As String = "Y", Optional ByVal sRebootViaSSH As String = "Y", _
                          Optional ByVal sDoNotReboot As String = "N", Optional ByVal sHTTPS As String = "N") As Integer

        Dim x As Integer

        Try
            If ID = -1 Then 'adding a new one
                x = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2", "Count", 0)
            Else
                x = ID
            End If

            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "Name", sMinerName, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "Type", MinerInfo.ShortName, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "IPAddress", sIPAddress, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "HTTPPort", sHTTPPort, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "WebUsername", sWebUserName, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "WebPassword", sWebPassword, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "SSHUsername", sSSHUserName, Microsoft.Win32.RegistryValueKind.String)

            If sSSHPassword.IsNullOrEmpty = True Then
                If MinerInfo.MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2 OrElse _
                   MinerInfo.MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1 OrElse _
                   MinerInfo.MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4 Then

                    sSSHPassword = "admin"
                Else
                    sSSHPassword = "root"
                End If
            End If

            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "SSHPassword", sSSHPassword, Microsoft.Win32.RegistryValueKind.String)

            If sActive = "Y" Then
                If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "Active", "") <> "Y" Then
                    iMinersEnabled += 1
                End If
            ElseIf sActive = "N" And ID <> -1 Then
                iMinersEnabled -= 1
            End If

            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "Active", sActive, Microsoft.Win32.RegistryValueKind.String)

            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "APIPort", sAPIPort, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "SSHPort", sSSHPort, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "ID", x.ToString, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "UseAPI", sUseAPI, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "RebootViaSSH", sRebootViaSSH, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "DoNotReboot", sRebootViaSSH, Microsoft.Win32.RegistryValueKind.String)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2\" & x, "HTTPS", sHTTPS, Microsoft.Win32.RegistryValueKind.String)

            If ID = -1 Then 'adding an ant, need to increment the counter
                My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\AntsV2", "Count", x + 1)

                Return x
            Else
                Return ID
            End If
        Catch ex As Exception When bErrorHandle = True
            AddToLogQueue("Error occurred when trying to add Miner " & sMinerName & ": " & ex.Message)
        End Try

    End Function

    'this is the thread dispatcher
    'it runs on its own thread, waiting for Ants to check
    'then it hands the work out to the worker threads as they are available
    Private Sub CheckForWork()

        While bShutDown = False
            While minersToCheckQueue.Count = 0 AndAlso bShutDown = False
                System.Threading.Thread.Sleep(10)
            End While

            If bShutDown = True Then
                Exit While
            End If

            SyncLock minersToCheckLock
                For x As Integer = 0 To workerThread.Count - 2
                    If ThreadHandlers(x).bBusy = False AndAlso ThreadHandlers(x).bGotWork = False Then
                        ThreadHandlers(x).bBusy = True
                        ThreadHandlers(x).MinerToCheck = minersToCheckQueue.Dequeue
                        ThreadHandlers(x).bGotWork = True

                        If minersToCheckQueue.Count = 0 Then
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
            AddToLogQueue("Thread " & iThread & ": " & ThreadHandlers(iThread).MinerToCheck.sName)
#End If

            Call GetMinerDataViaAPI(ThreadHandlers(iThread).MinerToCheck)

            ThreadHandlers(iThread).bGotWork = False
            ThreadHandlers(iThread).bBusy = False
        End While

    End Sub

    Private Function FindMinerConfig(ByVal ID As Integer) As DataRow

        For Each dr As DataRow In Me.dsMinerConfig.Tables(0).Rows
            If dr("ID").ToString = ID Then
                Return dr
            End If
        Next

        'should never happen
        AddToLogQueue("Miner " & ID & " not found in config!")

        Return Nothing

    End Function

    Private Function FindDisplayMinerByID(ByVal ID As Integer) As DataRow

        Dim dr As DataRow

        For Each dr In ds.Tables(0).Rows
            If dr.Item("ID") = ID Then
                Return dr

                Exit For
            End If
        Next

        'not found in the list of miners in the output, make a new one
        dr = ds.Tables(0).NewRow
        dr("ID") = -1

        Return dr

    End Function

    'triggered by the browser control when it's done loading a web page
    Private Sub wb_completed(sender As Object, e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs)

        Dim dr As DataRow
        Dim x, y, z As Integer
        Dim wb As WebBrowser
        Dim sbTemp, sbTemp2, sbTemp3, sbTemp4 As System.Text.StringBuilder
        Dim count(0 To 9) As Integer
        Dim sIP As String
        Dim s(), p() As String
        Dim antConfigRow As DataRow
        Dim avg As clsAverage
        Dim pdl As System.Collections.Generic.List(Of clsPoolData)
        Dim pd As clsPoolData

        Try
            wb = sender

            sbTemp = New System.Text.StringBuilder
            sbTemp2 = New System.Text.StringBuilder
            sbTemp3 = New System.Text.StringBuilder
            sbTemp4 = New System.Text.StringBuilder

            avg = New clsAverage

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

            If wb.Document.All(1).OuterHtml.ToLower.Contains("authorization") Then
                AddToLogQueue(antConfigRow("Type") & ":" & antConfigRow("Name") & " responded with login page")

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
                dr = FindDisplayMinerByID(antConfigRow("ID"))

                dr.Item("Name") = antConfigRow("Name")

                If wb.Url.AbsoluteUri.Contains("minerStatus.cgi") Then
                    'S2/S4 status code
                    AddToLogQueue(antConfigRow("Type") & ":" & antConfigRow("Name") & " responded with status page")

                    Select Case antConfigRow("Type")
                        Case "S2"
                            dr.Item("Uptime") = wb.Document.All(88).OuterText
                            dr.Item("Speed(5s)") = FormatHashRate(Val(wb.Document.All(91).OuterText) * 1000)
                            dr.Item("Speed(avg)") = FormatHashRate(Val(wb.Document.All(94).OuterText) * 1000)

                            dr.Item("CurHash") = Val(wb.Document.All(94).OuterText) * 1000000
                            dr.Item("HashSHA256") = Val(wb.Document.All(94).OuterText.Replace(",", "")) * 1000
                            dr.Item("HashScrypt") = 0

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

                                avg.AddNumber(Val(wb.Document.All(369).OuterText))
                                avg.AddNumber(Val(wb.Document.All(385).OuterText))
                                avg.AddNumber(Val(wb.Document.All(401).OuterText))
                                avg.AddNumber(Val(wb.Document.All(417).OuterText))
                                avg.AddNumber(Val(wb.Document.All(433).OuterText))
                                avg.AddNumber(Val(wb.Document.All(449).OuterText))
                                avg.AddNumber(Val(wb.Document.All(465).OuterText))
                                avg.AddNumber(Val(wb.Document.All(481).OuterText))
                                avg.AddNumber(Val(wb.Document.All(497).OuterText))
                                avg.AddNumber(Val(wb.Document.All(513).OuterText))

                                dr.Item("ATemp") = avg.GetAverage

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
                                    Call RebootMiner(antConfigRow, False, False, wb)
                                End If
                            End If

                        Case "S4"
                            dr.Item("Uptime") = wb.Document.All(70).OuterText
                            dr.Item("Speed(5s)") = FormatHashRate(Val(wb.Document.All(74).OuterText * 1000))
                            dr.Item("Speed(avg)") = FormatHashRate(Val(wb.Document.All(77).OuterText * 1000))

                            dr.Item("CurHash") = Val(wb.Document.All(77).OuterText.Replace(",", "")) * 1000000
                            dr.Item("HashSHA256") = Val(wb.Document.All(77).OuterText.Replace(",", "")) * 1000
                            dr.Item("HashScrypt") = 0

                            dr.Item("Blocks") = wb.Document.All(80).OuterText

                            'HW / (diffA + diffR + HW) * 100
                            dr.Item("HWE%") = Format(Val(wb.Document.All(350).OuterText.Replace(",", "")) / (Val(wb.Document.All(350).OuterText.Replace(",", "")) + _
                                              Val(wb.Document.All(322).OuterText.Replace(",", "")) + Val(wb.Document.All(325).OuterText.Replace(",", ""))), "##0.000%")

                            dr.Item("BestShare") = Format(UInt64.Parse(wb.Document.All(91).OuterText), "###,###,###,###,###,##0")

                            If IsDBNull(dr.Item("PoolData2")) = True Then
                                dr.Item("PoolData2") = New System.Collections.Generic.List(Of clsPoolData)
                            End If

                            pdl = dr.Item("PoolData2")
                            pdl.Clear()

                            pd = New clsPoolData

                            Select Case wb.Document.All(148).OuterText
                                Case "Alive"
                                    sbTemp.Append("U")

                                    pd.PoolIsUp = True

                                Case "Dead"
                                    sbTemp.Append("D")

                                    pd.PoolIsUp = False

                                Case Else
                                    sbTemp.Append("?")

                                    pd.PoolIsUp = False

                            End Select

                            sbTemp2.Append("0: " & wb.Document.All(142).OuterText & " (" & wb.Document.All(145).OuterText & ") " & wb.Document.All(148).OuterText)

                            pd.URL = wb.Document.All(142).OuterText
                            pd.UID = wb.Document.All(145).OuterText

                            pdl.Add(pd)

                            'reject
                            sbTemp4.Append(Format(Val(wb.Document.All(175).OuterText.Replace(",", "")) / (Val(wb.Document.All(160).OuterText.Replace(",", "")) + Val(wb.Document.All(175).OuterText.Replace(",", ""))) * 100, "##0.0"))
                            'stale
                            sbTemp3.Append(Format(Val(wb.Document.All(181).OuterText.Replace(",", "")) / (Val(wb.Document.All(160).OuterText.Replace(",", "")) + Val(wb.Document.All(181).OuterText.Replace(",", ""))) * 100, "##0.0"))

                            pd = New clsPoolData

                            Select Case wb.Document.All(200).OuterText
                                Case "Alive"
                                    sbTemp.Append("U")

                                    pd.PoolIsUp = True

                                Case "Dead"
                                    sbTemp.Append("D")

                                    pd.PoolIsUp = False

                                Case Else
                                    sbTemp.Append("?")

                                    pd.PoolIsUp = False

                            End Select

                            sbTemp2.Append(vbCrLf)

                            sbTemp2.Append("1: " & wb.Document.All(194).OuterText & " (" & wb.Document.All(197).OuterText & ") " & wb.Document.All(200).OuterText)

                            pd.URL = wb.Document.All(194).OuterText
                            pd.UID = wb.Document.All(197).OuterText

                            pdl.Add(pd)

                            'reject
                            sbTemp4.Append(" " & Format(Val(wb.Document.All(227).OuterText.Replace(",", "")) / (Val(wb.Document.All(212).OuterText.Replace(",", "")) + Val(wb.Document.All(227).OuterText.Replace(",", ""))) * 100, "##0.0"))
                            'stale
                            sbTemp3.Append(" " & Format(Val(wb.Document.All(233).OuterText.Replace(",", "")) / (Val(wb.Document.All(212).OuterText.Replace(",", "")) + Val(wb.Document.All(233).OuterText.Replace(",", ""))) * 100, "##0.0"))

                            pd = New clsPoolData

                            Select Case wb.Document.All(252).OuterText
                                Case "Alive"
                                    sbTemp.Append("U")

                                    pd.PoolIsUp = True

                                Case "Dead"
                                    sbTemp.Append("D")

                                    pd.PoolIsUp = False

                                Case Else
                                    sbTemp.Append("?")

                                    pd.PoolIsUp = False

                            End Select

                            sbTemp2.Append(vbCrLf)

                            sbTemp2.Append("2: " & wb.Document.All(246).OuterText & " (" & wb.Document.All(249).OuterText & ") " & wb.Document.All(252).OuterText)

                            pd.URL = wb.Document.All(246).OuterText
                            pd.UID = wb.Document.All(249).OuterText

                            pdl.Add(pd)

                            dr.Item("PoolData") = sbTemp2.ToString

                            'reject
                            sbTemp4.Append(" " & Format(Val(wb.Document.All(279).OuterText.Replace(",", "")) / (Val(wb.Document.All(264).OuterText.Replace(",", "")) + Val(wb.Document.All(279).OuterText.Replace(",", ""))) * 100, "##0.0"))
                            'stale
                            sbTemp3.Append(" " & Format(Val(wb.Document.All(285).OuterText.Replace(",", "")) / (Val(wb.Document.All(264).OuterText.Replace(",", "")) + Val(wb.Document.All(285).OuterText.Replace(",", ""))) * 100, "##0.0"))

                            dr.Item("Pools") = sbTemp.ToString

                            sbTemp.Clear()

                            dr.Item("Diff") = wb.Document.All(184).OuterText & " " & wb.Document.All(236).OuterText & " " & wb.Document.All(288).OuterText

                            dr.Item("Rej%") = sbTemp4.ToString.Replace("NaN", "").Trim

                            dr.Item("Stale%") = sbTemp3.ToString.Replace("NaN", "").Trim

                            dr.Item("HFan") = GetHighValue(wb.Document.All(494).OuterText.Replace(",", ""), wb.Document.All(495).OuterText.Replace(",", ""), wb.Document.All(496).OuterText.Replace(",", ""), wb.Document.All(497).OuterText.Replace(",", ""))

                            dr.Item("Fans") = wb.Document.All(494).OuterText & " " & wb.Document.All(495).OuterText & " " & wb.Document.All(496).OuterText & " " & wb.Document.All(497).OuterText

                            dr.Item("Freq") = Val(wb.Document.All(425).OuterText)

                            dr.Item("HTemp") = GetHighValue(wb.Document.All(428).OuterText, wb.Document.All(444).OuterText, wb.Document.All(460).OuterText, wb.Document.All(476).OuterText)

                            dr.Item("Temps") = wb.Document.All(428).OuterText & " " & wb.Document.All(444).OuterText & " " & wb.Document.All(460).OuterText & " " & wb.Document.All(476).OuterText

                            avg.AddNumber(Val(wb.Document.All(428).OuterText))
                            avg.AddNumber(Val(wb.Document.All(444).OuterText))
                            avg.AddNumber(Val(wb.Document.All(460).OuterText))
                            avg.AddNumber(Val(wb.Document.All(476).OuterText))

                            dr.Item("ATemp") = avg.GetAverage

                            count(0) = HowManyInString(wb.Document.All(431).OuterText, "x") + HowManyInString(wb.Document.All(431).OuterText, "-")
                            count(1) = HowManyInString(wb.Document.All(447).OuterText, "x") + HowManyInString(wb.Document.All(447).OuterText, "-")
                            count(2) = HowManyInString(wb.Document.All(463).OuterText, "x") + HowManyInString(wb.Document.All(463).OuterText, "-")
                            count(3) = HowManyInString(wb.Document.All(479).OuterText, "x") + HowManyInString(wb.Document.All(479).OuterText, "-")

                            dr.Item("XCount") = count(0) + count(1) + count(2) + count(3) & "X"

                            dr.Item("Status") = count(0) & "X " & count(1) & "X " & count(2) & "X " & count(3) & "X "

                            If (count(0) <> 0 OrElse count(1) <> 0 OrElse count(2) <> 0 OrElse count(3) <> 0) AndAlso Me.chkAlertRebootIfXd.Checked = True Then
                                Call RebootMiner(antConfigRow, False, False, wb)
                            End If

                    End Select
                ElseIf wb.Url.AbsoluteUri.Contains("/reboot.html") = True Then
                    'S2/S4 reboot
                    wb.Document.All(66).InvokeMember("click")

                    Call wbFinished(wb)

                    Exit Sub
                ElseIf wb.Url.AbsoluteUri.Contains("/admin/status/minerstatus/") = True Then
                    'S1/S3 status code    
                    'sAnt = sFullAntName.Substring(0, 3) & sAnt

                    AddToLogQueue(antConfigRow("Type") & ":" & antConfigRow("Name") & " responded with status page")

                    If wb.Url.AbsoluteUri.Contains("minerstatus") AndAlso wb.Document.All.Count > 75 Then
                        dr.Item("Uptime") = wb.Document.All(122).OuterText.TrimEnd

                        If wb.Document.All(84).Children(2).Children.Count <> 1 Then
                            dr.Item("Speed(5s)") = FormatHashRate(wb.Document.All(126).OuterText.TrimEnd * 1000)
                            dr.Item("Speed(avg)") = FormatHashRate(wb.Document.All(130).OuterText.TrimEnd * 1000)

                            dr.Item("CurHash") = Val(wb.Document.All(130).OuterText) * 1000000
                            dr.Item("HashSHA256") = Val(wb.Document.All(130).OuterText.Replace(",", "")) * 1000
                            dr.Item("HashScrypt") = 0

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

                            avg.AddNumber(Val(wb.Document.All(x + 37).OuterText))
                            avg.AddNumber(Val(wb.Document.All(x + 62).OuterText))

                            dr.Item("ATemp") = avg.GetAverage

                            dr.Item("Freq") = Val(wb.Document.All(x + 29).OuterText.TrimEnd)

                            count(0) = HowManyInString(wb.Document.All(x + 41).OuterText.TrimEnd, "x") + HowManyInString(wb.Document.All(x + 41).OuterText.TrimEnd, "-")
                            count(1) = HowManyInString(wb.Document.All(x + 66).OuterText.TrimEnd, "x") + HowManyInString(wb.Document.All(x + 66).OuterText.TrimEnd, "-")

                            dr.Item("XCount") = count(0) + count(1) & "X"

                            dr.Item("Status") = count(0) & "X " & count(1) & "X"
                        End If

                        If (count(0) <> 0 OrElse count(1) <> 0) AndAlso Me.chkAlertRebootIfXd.Checked = True Then
                            Call RebootMiner(antConfigRow, False, False, wb)
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

        Dim antConfig As stMinerConfig
        Dim antConfigRow As DataRow
        Dim x As Integer
        Dim tsRebootTime, tsCompare As TimeSpan
        Dim dRebootAt, dRebootAtAlso As Date
        Dim s() As String
        Dim dTemp As Date
        Dim bGoodToCheckReboot, bRebootAnt, bValidRebootTime, bValidRebootTimeAlso As Boolean
        Dim wc As eWebClient
        Dim sTemp As String

        Static dRefreshTime As Date
        Static bRefresh As Boolean
        Static bStarted As Boolean

        Try
            iCountDown -= 1

            If iCountDown < 0 Then
                iCountDown = iRefreshRate
            End If

            If bRefresh = True AndAlso dRefreshTime.AddSeconds(iDisplayRefreshPeriod) < Now Then
                bRefresh = False
                Me.dataMiners.Refreshing = False

                Me.dataMiners.Refresh()
            ElseIf bRefresh = False Then
                Me.dataMiners.Refresh()
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

            'reboot checks
            Try
                If (Me.chkRebootAntsByUptime.Checked = True AndAlso Me.txtRebootAntsByUptime.Text.IsNullOrEmpty = False) _
                    OrElse (Me.chkRebootAllAntsAt.Checked = True AndAlso Me.txtRebootAllAntsAt.Text.IsNullOrEmpty = False) _
                    OrElse (Me.chkRebootAllAntsAtAlso.Checked = True AndAlso Me.txtRebootAllAntsAtAlso.Text.IsNullOrEmpty = False) Then

                    If Me.chkRebootAntsByUptime.Checked = True AndAlso Me.txtRebootAntsByUptime.Text.IsNullOrEmpty = False Then
                        Select Case Me.cmbRebootAntsByUptime.Text
                            Case "Hours"
                                tsRebootTime = TimeSpan.FromHours(Val(Me.txtRebootAntsByUptime.Text))

                            Case "Minutes"
                                tsRebootTime = TimeSpan.FromMinutes(Val(Me.txtRebootAntsByUptime.Text))

                            Case "Seconds"
                                tsRebootTime = TimeSpan.FromSeconds(Val(Me.txtRebootAntsByUptime.Text))

                            Case "Days"
                                tsRebootTime = TimeSpan.FromDays(Val(Me.txtRebootAntsByUptime.Text))

                        End Select
                    End If

                    If Me.chkRebootAllAntsAt.Checked = True AndAlso Me.txtRebootAllAntsAt.Text.IsNullOrEmpty = False Then
                        If Date.TryParse(Me.txtRebootAllAntsAt.Text & " " & Me.cmbRebootAllAntsAt.Text, dTemp) = True Then
                            dRebootAt = dTemp

                            bValidRebootTime = True
                        End If
                    End If

                    If Me.chkRebootAllAntsAtAlso.Checked = True AndAlso Me.txtRebootAllAntsAtAlso.Text.IsNullOrEmpty = False Then
                        If Date.TryParse(Me.txtRebootAllAntsAtAlso.Text & " " & Me.cmbRebootAllAntsAtAlso.Text, dTemp) = True Then
                            dRebootAtAlso = dTemp

                            bValidRebootTimeAlso = True
                        End If
                    End If

                    'check each responding Ant 
                    For Each dr As DataGridViewRow In Me.dataMiners.Rows
                        bGoodToCheckReboot = False
                        bRebootAnt = False

                        If dr.Cells("Uptime").Value <> "???" AndAlso dr.Cells("Uptime").Value <> "ERROR" Then
                            'get antconfigrow
                            antConfigRow = FindMinerConfig(dr.Cells("ID").Value)

                            s = dr.Cells("Uptime").Value.ToString.Split(" ")

                            'days
                            tsCompare = TimeSpan.FromDays(Val(s(0).LeftMost(1)))

                            'hours
                            tsCompare = tsCompare.Add(TimeSpan.FromHours(Val(s(1).LeftMost(1))))

                            'minutes
                            tsCompare = tsCompare.Add(TimeSpan.FromMinutes(Val(s(2).LeftMost(1))))

                            'seconds
                            tsCompare = tsCompare.Add(TimeSpan.FromSeconds(Val(s(3).LeftMost(1))))

                            'reboot ants by uptime
                            If Me.chkRebootAntsByUptime.Checked = True AndAlso Me.txtRebootAntsByUptime.Text.IsNullOrEmpty = False Then
                                If dictScheduledReboots.TryGetValue(dr.Cells("ID").Value, dTemp) = True Then
                                    'stop looking for 10 minutes after a reboot
                                    If dTemp.AddMinutes(10) < Now Then
                                        bGoodToCheckReboot = True

                                        dictScheduledReboots.Remove(dr.Cells("ID").Value)
                                    End If
                                Else
                                    bGoodToCheckReboot = True
                                End If
                            End If

                            If bGoodToCheckReboot = True Then
                                If tsCompare > tsRebootTime Then
                                    bRebootAnt = True

                                    AddToLogQueue("Rebooting " & antConfigRow("Name") & " because it's been up more than the specified uptime before rebooting.")

                                    dictScheduledReboots.Add(dr.Cells("ID").Value, Now)
                                End If
                            End If

                            'reboot all ants at a certain time
                            If Me.chkRebootAllAntsAt.Checked = True AndAlso Me.txtRebootAllAntsAt.Text.IsNullOrEmpty = False AndAlso bValidRebootTime = True Then
                                'only after the reboot time
                                If dRebootAt < Now Then
                                    'but within 15 minutes of the time
                                    If dRebootAt.AddMinutes(15) > Now Then
                                        'check to see if we've already rebooted this way today
                                        If dictRebootAt.TryGetValue(dr.Cells("ID").Value, dTemp) = True Then
                                            'if so, within the last 15 mins?
                                            If dTemp.AddMinutes(15) < Now Then
                                                'reboot!
                                                bRebootAnt = True

                                                dictRebootAt(dr.Cells("ID").Value) = Now

                                                AddToLogQueue("Rebooting " & antConfigRow("Name") & " because it's the scheduled reboot time.")
                                            End If
                                        Else
                                            'reboot!
                                            bRebootAnt = True

                                            dictRebootAt.Add(dr.Cells("ID").Value, Now)

                                            AddToLogQueue("Rebooting " & antConfigRow("Name") & " because it's the scheduled reboot time.")
                                        End If
                                    End If
                                End If
                            End If

                            'reboot all ants at a certain time also
                            If Me.chkRebootAllAntsAtAlso.Checked = True AndAlso Me.txtRebootAllAntsAtAlso.Text.IsNullOrEmpty = False AndAlso bValidRebootTimeAlso = True Then
                                'only after the reboot time
                                If dRebootAtAlso < Now Then
                                    'but within 15 minutes of the time
                                    If dRebootAtAlso.AddMinutes(15) > Now Then
                                        'check to see if we've already rebooted this way today
                                        If dictRebootAtAlso.TryGetValue(dr.Cells("ID").Value, dTemp) = True Then
                                            'if so, within the last 15 mins?
                                            If dTemp.AddMinutes(15) < Now Then
                                                'reboot!
                                                bRebootAnt = True

                                                dictRebootAtAlso(dr.Cells("ID").Value) = Now

                                                AddToLogQueue("Rebooting " & antConfigRow("Name") & " because it's the scheduled reboot time (also).")
                                            End If
                                        Else
                                            'reboot!
                                            bRebootAnt = True

                                            dictRebootAtAlso.Add(dr.Cells("ID").Value, Now)

                                            AddToLogQueue("Rebooting " & antConfigRow("Name") & " because it's the scheduled reboot time (also).")
                                        End If
                                    End If
                                End If
                            End If

                            If bRebootAnt = True Then
                                Call RebootMiner(antConfigRow, False, YNtoBoolean(antConfigRow("RebootViaSSH")), Nothing)
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception When bErrorHandle = True
                AddToLogQueue("Internal error occurred when processing the reboot on schedule/uptime logic: " & ex.Message)
            End Try

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
                    Me.dataMiners.Refreshing = True
                Else
                    bStarted = True
                End If

                For Each dr As DataRow In Me.dsMinerConfig.Tables(0).Rows
                    If dr("Active") = "Y" Then
                        If dr("UseAPI") = "Y" Then
                            antConfig = GetMinerConfigByConfigRow(dr)

                            SyncLock minersToCheckLock
                                minersToCheckQueue.Enqueue(antConfig)
                            End SyncLock
                        Else
                            Select Case dr("Type")
                                Case "SP10", "SP20", "SP30", "SP31", "SP35"
                                    wc = New eWebClient
                                    wc.Credentials = New System.Net.NetworkCredential(dr("WebUsername").ToString, dr("WebPassword").ToString)

                                    System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf WCIgnoreInvalidCertCheck

                                    AddHandler wc.DownloadStringCompleted, AddressOf Me.WebClientDownloadStringCompleted

                                    If dr("HTTPS").ToString = "Y" Then
                                        sTemp = "https://"
                                    Else
                                        sTemp = "http://"
                                    End If

                                    If dr("HTTPPort") <> "80" Then
                                        wc.DownloadStringAsync(New System.Uri(sTemp & dr("IPAddress").ToString & ":" & dr("HTTPPort").ToString & "/monitor.php"), dr)
                                    Else
                                        wc.DownloadStringAsync(New System.Uri(sTemp & dr("IPAddress").ToString & "/monitor.php"), dr)
                                    End If

                                Case Else

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

                                            Case "S2", "S4"
                                                wb(x).Navigate(String.Format("http://{0}:{1}@" & dr("IPAddress") & ":" & dr("HTTPPort") & "/cgi-bin/minerStatus.cgi", dr("WebUsername"), dr("WebPassword")), Nothing, Nothing, GetHeader)

                                        End Select

                                        AddToLogQueue("Submitted " & dr("Name") & " on web browser instance " & x)
                                    End If

                            End Select
                        End If
                    End If
                Next

                iCountDown = iRefreshRate

                Me.cmdPause.Enabled = True
            End If

            Me.cmdRefresh.Text = "Refreshing in " & iCountDown

            If Me.cmdPause.Text = "Resume" Then
                Me.TimerRefresh.Enabled = False
            End If
        Catch ex As Exception When bErrorHandle = True
            Call AddToLogQueue("Internal error on TimerRefresh: " & ex.Message)
        End Try

    End Sub

    Private Function WCIgnoreInvalidCertCheck(sender As Object, cert As System.Security.Cryptography.X509Certificates.X509Certificate, chain As System.Security.Cryptography.X509Certificates.X509Chain, sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean

        Debug.Print("Ignoring cert error on HTTPS")

        Return True

    End Function

    Private Sub WebClientDownloadStringCompleted(ByVal sender As Object, ByVal e As System.Net.DownloadStringCompletedEventArgs)

        Dim configRow As DataRow
        Dim dr As DataRow
        Dim j, jp1 As Newtonsoft.Json.Linq.JObject
        Dim ja As Newtonsoft.Json.Linq.JArray
        Dim ts As TimeSpan
        Dim avg As clsAverage
        Dim count(0 To 2) As Integer
        Dim DisplayColumns As clsDisplayColumns
        Dim sbTemp, sbTemp2 As System.Text.StringBuilder
        Dim dBestShare As Double
        Dim pd As clsPoolData
        Dim pdl As System.Collections.Generic.List(Of clsPoolData)
        Dim bStep As Byte
        Dim s() As String

        Try
            configRow = e.UserState

            If e.Cancelled = False AndAlso e.Error Is Nothing Then
                dr = FindDisplayMinerByID(configRow("ID"))

                DisplayColumns = New clsDisplayColumns(dr)

                j = Newtonsoft.Json.Linq.JObject.Parse(e.Result)

#If DEBUG Then
                'j = Newtonsoft.Json.Linq.JObject.Parse("{""stats"":{""STATS"":0,""ID"":""SPN0"",""Elapsed"":1064271,""Calls"":0,""Wait"":0,""Max"":0,""Min"":99999999,""ASICstotalrate"":1410975,""Temparaturefront"":29,""Temparaturereartop"":65,""Temparaturerearbot"":77},""pools"":[{""POOL"":0,""URL"":""stratum+tcp:\/\/us1.ghash.io:3333"",""Status"":""Alive"",""Priority"":0,""Quota"":1,""LongPoll"":""N"",""Getworks"":27338,""Accepted"":433670,""Rejected"":639,""Works"":17579759,""Discarded"":636545,""Stale"":59,""GetFailures"":1,""RemoteFailures"":0,""User"":""MichaelDiggs.SP10"",""LastShareTime"":1422188852,""Diff1Shares"":55661888,""ProxyType"":"""",""Proxy"":"""",""DifficultyAccepted"":346986496,""DifficultyRejected"":488960,""DifficultyStale"":0,""LastShareDifficulty"":512,""HasStratum"":true,""StratumActive"":true,""StratumURL"":""us1.ghash.io"",""HasGBT"":false,""BestShare"":670139018,""PoolRejected%"":0.1407,""PoolStale%"":0}],""notify"":[{""NOTIFY"":0,""Name"":""SPN"",""ID"":0,""LastWell"":1422188852,""LastNotWell"":0,""ReasonNotWell"":""None"",""*ThreadFailInit"":0,""*ThreadZeroHash"":0,""*ThreadFailQueue"":0,""*DevSickIdle60s"":0,""*DevDeadIdle600s"":0,""*DevNostart"":0,""*DevOverHeat"":0,""*DevThermalCutoff"":0,""*DevCommsError"":0,""*DevThrottle"":0}],""summary"":[{""Elapsed"":1064271,""MHSav"":1309615.15,""MHS5s"":1313744.36,""MHS1m"":1305496.07,""MHS5m"":1307374.55,""MHS15m"":1308144.31,""FoundBlocks"":0,""Getworks"":27338,""Accepted"":433670,""Rejected"":639,""HardwareErrors"":2912,""Utility"":24.45,""Discarded"":636546,""Stale"":59,""GetFailures"":1,""LocalWork"":18218002,""RemoteFailures"":0,""NetworkBlocks"":1693,""TotalMH"":1393785480000,""WorkUtility"":3138.03,""DifficultyAccepted"":346986496,""DifficultyRejected"":488960,""DifficultyStale"":0,""BestShare"":670139018,""DeviceHardware%"":0.0052,""DeviceRejected%"":0.8784,""PoolRejected%"":0.1407,""PoolStale%"":0,""Lastgetwork"":1422188852}],""conf"":{""api-listen"":true,""api-allow"":""W:127.0.0.1"",""pools"":[{""url"":""us1.ghash.io:3333"",""user"":""MichaelDiggs.SP10"",""pass"":""123""}]},""miner"":{""model_id"":""SP10"",""model_class"":""SP1x"",""board_ver"":""FL1421003541"",""fw_ver"":""1.5.8"",""mac"":""7C:66:9D:36:3E:4C"",""uptime"":2089383,""free_mem"":""254""},""mg_events"":"""",""mg_status"":""DCRTOP 1 DCRBOT 0"",""workmode"":{""fan_speed"":""80"",""start_voltage_top"":""664"",""start_voltage_bot"":""664"",""max_voltage"":""750"",""max_watts"":""1260"",""dc2dc_current"":""62""}}")
#End If

                avg = New clsAverage
                sbTemp = New System.Text.StringBuilder
                sbTemp2 = New System.Text.StringBuilder

                dr.Item("Name") = configRow("Name")
                DisplayColumns.Type = configRow("Type")

                bStep = 1

                For Each jp1 In j.Property("stats")
                    ts = New TimeSpan(0, 0, jp1.Value(Of Integer)("Elapsed"))

                    DisplayColumns.Uptime = Format(ts.Days, "0d") & " " & Format(ts.Hours, "0h") & " " & Format(ts.Minutes, "0m") & " " & Format(ts.Seconds, "0s")

                    If configRow("Type") = "SP10" Then
                        count(0) = jp1.Value(Of Integer)("Temparaturefront")
                        count(1) = jp1.Value(Of Integer)("Temparaturereartop")
                        count(2) = jp1.Value(Of Integer)("Temparaturerearbot")
                    Else
                        count(0) = jp1.Value(Of Integer)("Temperaturefront")
                        count(1) = jp1.Value(Of Integer)("Temperaturereartop")
                        count(2) = jp1.Value(Of Integer)("Temperaturerearbot")
                    End If

                    avg.AddNumber(count(0))
                    avg.AddNumber(count(1))
                    avg.AddNumber(count(2))

                    DisplayColumns.ATemp = avg.GetAverage

                    DisplayColumns.HTemp = GetHighValue(count(0), count(1), count(2))

                    DisplayColumns.Temps = count(0).ToString & " " & count(1).ToString & " " & count(2).ToString
                Next

                DisplayColumns.Freq = DBNull.Value
                DisplayColumns.HFan = DBNull.Value

                bStep = 2

                For Each ja In j.Property("summary")
                    For Each jp1 In ja
                        DisplayColumns.HWE = jp1.Value(Of String)("DeviceHardware%") & "%"

                        DisplayColumns.Speed5s = FormatHashRate(jp1.Value(Of String)("MHS5s"))
                        DisplayColumns.SpeedAvg = FormatHashRate(jp1.Value(Of String)("MHSav"))

                        DisplayColumns.MobileMinerHash = Val(jp1.Value(Of String)("MHSav")) * 1000
                        DisplayColumns.HashSHA256 = Val(jp1.Value(Of String)("MHSav"))
                        DisplayColumns.HashScrypt = 0

                        DisplayColumns.Rej = jp1.Value(Of String)("PoolRejected%")
                        DisplayColumns.Stale = jp1.Value(Of String)("PoolStale%")

                        DisplayColumns.Blocks = jp1.Value(Of String)("FoundBlocks")
                    Next
                Next

                bStep = 3

                For Each ja In j.Property("notify")
                    For Each jp1 In ja
                        If jp1.Value(Of String)("ReasonNotWell") = "None" Then
                            DisplayColumns.Status = "Good"
                            DisplayColumns.XCount = 0
                        Else
                            DisplayColumns.XCount = WCHelper(jp1, "*ThreadFailInit", sbTemp)
                            DisplayColumns.XCount += WCHelper(jp1, "*ThreadZeroHash", sbTemp)
                            DisplayColumns.XCount += WCHelper(jp1, "*ThreadFailQueue", sbTemp)
                            DisplayColumns.XCount += WCHelper(jp1, "*DevSickIdle60s", sbTemp)
                            DisplayColumns.XCount += WCHelper(jp1, "*DevDeadIdle600s", sbTemp)
                            DisplayColumns.XCount += WCHelper(jp1, "*DevNostart", sbTemp)
                            DisplayColumns.XCount += WCHelper(jp1, "*DevOverHeat", sbTemp)
                            DisplayColumns.XCount += WCHelper(jp1, "*DevThermalCutoff", sbTemp)
                            DisplayColumns.XCount += WCHelper(jp1, "*DevCommsError", sbTemp)
                            DisplayColumns.XCount += WCHelper(jp1, "*DevThrottle", sbTemp)

                            DisplayColumns.Status = jp1.Value(Of String)("ReasonNotWell") & " -- " & sbTemp.ToString
                        End If
                    Next
                Next

                bStep = 3

                sbTemp.Clear()

                If IsDBNull(dr.Item("PoolData2")) = True Then
                    dr.Item("PoolData2") = New System.Collections.Generic.List(Of clsPoolData)
                End If

                pdl = dr.Item("PoolData2")
                pdl.Clear()

                For Each ja In j.Property("pools")
                    For Each jp1 In ja
                        If jp1.Value(Of Double)("BestShare") > dBestShare Then
                            dBestShare = jp1.Value(Of Double)("BestShare")
                        End If

                        pd = New clsPoolData

                        Select Case jp1.Value(Of String)("Status")
                            Case "Alive"
                                If sbTemp.ToString.Contains("U") = False Then
                                    DisplayColumns.Diff = Format(jp1.Value(Of Double)("LastShareDifficulty"), "#,###,###")
                                End If

                                sbTemp.Append("U")

                                pd.PoolIsUp = True

                            Case "Dead"
                                sbTemp.Append("D")

                                pd.PoolIsUp = False

                            Case Else
                                sbTemp.Append("?")

                                pd.PoolIsUp = False

                        End Select

                        If sbTemp2.Length <> 0 Then
                            sbTemp2.Append(vbCrLf)
                        End If

                        sbTemp2.Append(jp1.Value(Of String)("POOL") & ": " & jp1.Value(Of String)("URL") & " (" & jp1.Value(Of String)("User") & ") " & jp1.Value(Of String)("Status"))

                        pd.URL = jp1.Value(Of String)("URL")
                        pd.UID = jp1.Value(Of String)("User")

                        pdl.Add(pd)
                    Next
                Next

                bStep = 4

                DisplayColumns.BestShare = Format(dBestShare, "###,###,###,###,###,##0")
                DisplayColumns.Pools = sbTemp.ToString
                DisplayColumns.PoolData = sbTemp2.ToString

                If configRow("Type") = "SP10" Then
                    For Each jp1 In j.Property("workmode")
                        If jp1.Value(Of String)("fan_speed") = "0" Then
                            DisplayColumns.Fans = "Auto"
                        Else
                            DisplayColumns.Fans = jp1.Value(Of String)("fan_speed")
                        End If
                    Next
                Else
                    For Each jp1 In j.Property("miner")
                        s = jp1.Value(Of String)("mg_custom_mode").Split(" ")

                        s = s(0).Split(":")

                        If s(1) = "0" Then
                            DisplayColumns.Fans = "Auto"
                        Else
                            DisplayColumns.Fans = s(1)
                        End If
                    Next
                End If

                If dr("ID") = -1 Then
                    dr("ID") = configRow("ID")

                    ds.Tables(0).Rows.Add(dr)
                End If

                Call HandleAlerts(dr, configRow, Nothing)

                Call RefreshTitle()
            Else
                AddToLogQueue("HTTP request for " & configRow("Name") & " failed: " & e.Error.Message)
            End If
        Catch ex As Exception When bErrorHandle = True
            AddToLogQueue("ERROR when processing download string from " & configRow("Name") & ": " & ex.Message)
        End Try

        RemoveHandler DirectCast(sender, eWebClient).DownloadStringCompleted, AddressOf Me.WebClientDownloadStringCompleted

    End Sub

    Private Function WCHelper(ByRef jp1 As Newtonsoft.Json.Linq.JObject, ByVal sValue As String, ByRef sbTemp As System.Text.StringBuilder) As Integer

        If jp1.Value(Of Integer)(sValue) <> 0 Then
            If sbTemp.Length <> 0 Then
                sbTemp.Append("; ")
            End If

            sbTemp.Append(sValue & ": " & jp1.Value(Of Integer)(sValue))
        End If

        Return jp1.Value(Of Integer)(sValue)

    End Function

    Private Function GetMinerConfigByConfigRow(ByRef dr As DataRow) As stMinerConfig

        Dim MinerConfig As stMinerConfig

        MinerConfig = New stMinerConfig

        MinerConfig.MinerInfo = Me.SupportedMinerInfo.GetMinerObjectByShortName(dr("Type"))
        MinerConfig.sName = dr("Name")
        MinerConfig.ID = dr("ID")
        MinerConfig.sAPIPort = dr("APIPort")
        MinerConfig.sHTTPPort = dr("HTTPPort")
        MinerConfig.sIP = dr("IPAddress")
        MinerConfig.sSSHPassword = dr("SSHPassword")
        MinerConfig.sSSHPort = dr("SSHPort")
        MinerConfig.sSSHUsername = dr("SSHUsername")
        MinerConfig.sWebPassword = dr("WebPassword")
        MinerConfig.sWebUsername = dr("WebUsername")

        Return MinerConfig

    End Function

    Private Sub cmdRefresh_Click(sender As System.Object, e As System.EventArgs) Handles cmdRefresh.Click

        If Me.dataMinerConfig.Rows.Count = 0 Then
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

        Return "Authorization: Basic " & Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(Me.txtMinerWebUsername.Text & ":" & Me.txtMinerWebPassword.Text)) & System.Environment.NewLine

    End Function

    'refresh by API
    'this runs on the UI thread
    Private Sub RefreshGrid(ByRef MinerData As clsMinerRefreshData)

        Dim MinerConfig As DataRow
        Dim dr As DataRow
        Dim j, jp1 As Newtonsoft.Json.Linq.JObject
        Dim ja As Newtonsoft.Json.Linq.JArray
        Dim ts As TimeSpan
        Dim sbTemp, sbTemp2, sbTemp3, sbTemp4 As System.Text.StringBuilder
        Dim count(0 To 9), iTemp As Integer
        Dim dBestShare As Double
        Dim x, y, z As Integer
        Dim sbStep As System.Text.StringBuilder
        Dim pd As clsPoolData
        Dim pdl As System.Collections.Generic.List(Of clsPoolData)
        Dim Lock As New Object
        Dim DisplayColumns As clsDisplayColumns
        Dim s() As String
        Dim avg As clsAverage

        sbTemp = New System.Text.StringBuilder
        sbTemp2 = New System.Text.StringBuilder
        sbTemp3 = New System.Text.StringBuilder
        sbTemp4 = New System.Text.StringBuilder

        avg = New clsAverage

        Debug.Print("Refreshing " & MinerData.sMinerIP)

        Try
            sbStep = New System.Text.StringBuilder(3)

            dr = FindDisplayMinerByID(MinerData.ID)

            DisplayColumns = New clsDisplayColumns(dr)

            MinerConfig = FindMinerConfig(MinerData.ID)

            dr.Item("Name") = MinerConfig("Name")
            dr.Item("Type") = MinerConfig("Type")

            sbStep.Append("1.0")

#If DEBUG Then
            'MinerData.sStats = Replace("{""STATUS"":[{""STATUS"":""S"",""When"":1419209685,""Code"":70,""Msg"":""CGMiner stats"",""Description"":""cgminer 4.6.1""}],""STATS"":[{""CGMiner"":""4.6.1"",""Miner"":""7.0.0.3"",""CompileTime"":""Fri Oct 24 19:46:59 CST 2014"",""Type"":""S3""}{""STATS"":0,""ID"":""BMM0"",""Elapsed"":88,""Calls"":0,""Wait"":0.000000,""Max"":0.000000,""Min"":99999999.000000,""GHS 5s"":465.19,""GHS av"":490.97,""baud"":115200,""miner_count"":2,""asic_count"":8,""timeout"":17,""frequency"":""231.25"",""voltage"":"""",""hwv1"":7,""hwv2"":0,""hwv3"":0,""hwv4"":3,""fan_num"":2,""fan1"":900,""fan2"":720,""fan3"":0,""fan4"":0,""fan5"":0,""fan6"":0,""fan7"":0,""fan8"":0,""fan9"":0,""fan10"":0,""fan11"":0,""fan12"":0,""fan13"":0,""fan14"":0,""fan15"":0,""fan16"":0,""temp_num"":2,""temp1"":33,""temp2"":35,""temp3"":0,""temp4"":0,""temp5"":0,""temp6"":0,""temp7"":0,""temp8"":0,""temp9"":0,""temp10"":0,""temp11"":0,""temp12"":0,""temp13"":0,""temp14"":0,""temp15"":0,""temp16"":0,""temp_avg"":34,""temp_max"":35,""Device Hardware%"":0.0000,""no_matching_work"":0,""chain_acn1"":16,""chain_acn2"":16,""chain_acn3"":0,""chain_acn4"":0,""chain_acn5"":0,""chain_acn6"":0,""chain_acn7"":0,""chain_acn8"":0,""chain_acn9"":65535,""chain_acn10"":0,""chain_acn11"":0,""chain_acn12"":0,""chain_acn13"":0,""chain_acn14"":0,""chain_acn15"":0,""chain_acn16"":0,""chain_acs1"":""oooooooo oooooooo "",""chain_acs2"":""oooooooo oooooooo "",""chain_acs3"":"""",""chain_acs4"":"""",""chain_acs5"":"""",""chain_acs6"":"""",""chain_acs7"":"""",""chain_acs8"":"""",""chain_acs9"":"""",""chain_acs10"":"""",""chain_acs11"":"""",""chain_acs12"":"""",""chain_acs13"":"""",""chain_acs14"":"""",""chain_acs15"":"""",""chain_acs16"":"""",""USB Pipe"":""0""}],""id"":1}", "}{", "},{")
#End If

            j = Newtonsoft.Json.Linq.JObject.Parse(MinerData.sStats)

            sbStep.Clear()
            sbStep.Append("1.1")

            For Each ja In j.Property("STATS")
                Select Case MinerData.MinerInfo.MinerType
                    Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS3, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.AntminerS5
                        If ja.Count = 4 Then
                            jp1 = ja(0)
                        Else
                            jp1 = ja(1)
                        End If

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS1, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP10, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP20, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP30, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP31, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP35
                        jp1 = ja(0)

                End Select

                sbStep.Clear()
                sbStep.Append("1.2")

                Select Case MinerData.MinerInfo.MinerType
                    Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS1, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS3, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4, clsSupportedMinerInfo.enSupportedMinerTypes.AntminerS5

                        ts = New TimeSpan(0, 0, jp1.Value(Of Integer)("Elapsed"))

                        DisplayColumns.Uptime = Format(ts.Days, "0d") & " " & Format(ts.Hours, "0h") & " " & Format(ts.Minutes, "0m") & " " & Format(ts.Seconds, "0s")
                        dr.Item("HWE%") = jp1.Value(Of String)("Device Hardware%") & "%"

                        dr.Item("HFan") = GetHighValue(jp1.Value(Of Integer)("fan1"), jp1.Value(Of Integer)("fan2"), jp1.Value(Of Integer)("fan3"), jp1.Value(Of Integer)("fan4"))

                        sbTemp.Clear()

                        sbStep.Clear()
                        sbStep.Append("1.3")

                        For x = 1 To jp1.Value(Of Integer)("fan_num")
                            sbTemp.Append(jp1.Value(Of Integer)("fan" & x))

                            If x <> jp1.Value(Of Integer)("fan_num") Then
                                sbTemp.Append(" ")
                            End If
                        Next

                        dr.Item("Fans") = sbTemp.ToString

                        sbTemp.Clear()

                        iTemp = 0

                        sbStep.Clear()
                        sbStep.Append("1.4")

                        avg.Clear()

                        For x = 1 To jp1.Value(Of Integer)("temp_num")
                            sbTemp.Append(jp1.Value(Of Integer)("temp" & x))

                            avg.AddNumber(jp1.Value(Of Integer)("temp" & x))

                            If jp1.Value(Of Integer)("temp" & x) > iTemp Then
                                iTemp = jp1.Value(Of Integer)("temp" & x)
                            End If

                            If x <> jp1.Value(Of Integer)("temp_num") Then
                                sbTemp.Append(" ")
                            End If
                        Next

                        dr.Item("HTemp") = iTemp
                        dr.Item("ATemp") = avg.GetAverage

                        dr.Item("Temps") = sbTemp.ToString

                        dr.Item("Freq") = Val(jp1.Value(Of String)("frequency"))

                        sbStep.Clear()
                        sbStep.Append("1.5")

                        count(0) = HowManyInString(jp1.Value(Of String)("chain_acs1"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs1"), "x")
                        count(1) = HowManyInString(jp1.Value(Of String)("chain_acs2"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs2"), "x")

                        If MinerData.MinerInfo.MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2 OrElse _
                           MinerData.MinerInfo.MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1 OrElse _
                           MinerData.MinerInfo.MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4 Then

                            count(2) = HowManyInString(jp1.Value(Of String)("chain_acs3"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs3"), "x")
                            count(3) = HowManyInString(jp1.Value(Of String)("chain_acs4"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs4"), "x")

                            If MinerData.MinerInfo.MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2 Then
                                count(4) = HowManyInString(jp1.Value(Of String)("chain_acs5"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs5"), "x")
                                count(5) = HowManyInString(jp1.Value(Of String)("chain_acs6"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs6"), "x")
                                count(6) = HowManyInString(jp1.Value(Of String)("chain_acs7"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs7"), "x")
                                count(7) = HowManyInString(jp1.Value(Of String)("chain_acs8"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs8"), "x")
                                count(8) = HowManyInString(jp1.Value(Of String)("chain_acs9"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs9"), "x")
                                count(9) = HowManyInString(jp1.Value(Of String)("chain_acs10"), "-") + HowManyInString(jp1.Value(Of String)("chain_acs10"), "x")
                            Else
                                count(4) = 0
                                count(5) = 0
                                count(6) = 0
                                count(7) = 0
                                count(8) = 0
                                count(9) = 0
                            End If
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

                        sbStep.Clear()
                        sbStep.Append("1.6")

                        dr.Item("XCount") = count(0) + count(1) + count(2) + count(3) + count(4) + count(5) + count(6) + count(7) + count(8) + count(9) & "X"

                        Select Case MinerData.MinerInfo.MinerType
                            Case MinerData.MinerInfo.MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2
                                dr.Item("Status") = count(0) & "X " & count(1) & "X " & count(2) & "X " & count(3) & "X " & count(4) & "X " & count(5) & "X " & _
                                                count(6) & "X " & count(7) & "X " & count(8) & "X " & count(9) & "X"

                            Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4
                                dr.Item("Status") = count(0) & "X " & count(1) & "X " & count(2) & "X " & count(3) & "X "

                            Case Else
                                dr.Item("Status") = count(0) & "X " & count(1) & "X "

                        End Select

                        sbStep.Clear()
                        sbStep.Append("1.7")

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP10, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP20, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP30, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP31, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP35

                        sbStep.Clear()
                        sbStep.Append("1.8")

                        ts = New TimeSpan(0, 0, jp1.Value(Of Integer)("Elapsed"))

                        dr.Item("Uptime") = Format(ts.Days, "0d") & " " & Format(ts.Hours, "0h") & " " & Format(ts.Minutes, "0m") & " " & Format(ts.Seconds, "0s")

                        iTemp = 0

                        count(0) = jp1.Value(Of Integer)("Temperature front")
                        count(1) = jp1.Value(Of Integer)("Temperature rear top")
                        count(2) = jp1.Value(Of Integer)("Temperature rear bot")

                        avg.Clear()

                        avg.AddNumber(count(0))
                        avg.AddNumber(count(1))
                        avg.AddNumber(count(2))

                        dr.Item("ATemp") = avg.GetAverage

                        If count(1) > count(0) Then
                            If count(2) > count(1) Then
                                iTemp = count(2)
                            Else
                                iTemp = count(1)
                            End If
                        Else
                            iTemp = count(0)
                        End If

                        sbStep.Clear()
                        sbStep.Append("1.9")

                        dr.Item("HTemp") = iTemp

                        dr.Item("Temps") = count(0).ToString & " " & count(1).ToString & " " & count(2).ToString

                        dr.Item("Freq") = DBNull.Value
                        dr.Item("HFan") = DBNull.Value
                        dr.Item("Fans") = ""
                        dr.Item("XCount") = ""
                        dr.Item("Status") = ""

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.InnosiliconA288MH
                        sbStep.Clear()
                        sbStep.Append("2.01")

                        'dTemp = 0

                        'cores
                        sbTemp.Clear()

                        'temperature readings
                        sbTemp2.Clear()

                        'highest temperature
                        y = 0

                        avg.Clear()

                        For Each jp1 In ja
                            Select Case jp1.Value(Of String)("ID")
                                Case "BA10", "BA11", "BA12", "BA13", "BA14", "BA15"
                                    'uptime
                                    ts = New TimeSpan(0, 0, jp1.Value(Of Integer)("Elapsed"))

                                    DisplayColumns.Uptime = Format(ts.Days, "0d") & " " & Format(ts.Hours, "0h") & " " & Format(ts.Minutes, "0m") & " " & Format(ts.Seconds, "0s")

                                    'cores solo
                                    If sbTemp.Length <> 0 Then
                                        sbTemp.Append(" ")
                                    End If

                                    sbTemp.Append(jp1.Value(Of String)("ID") & ":" & jp1.Value(Of String)("CORES(TOTAL)"))

                                    'get highest temp
                                    s = jp1.Value(Of String)("TEMP(SOLO)").Split("-")

                                    z = 0

                                    For x = 0 To s.Count - 1
                                        If Val(s(x)) > y Then
                                            y = Val(s(x))   'highest across all
                                        End If

                                        If Val(s(x)) > z Then
                                            z = Val(s(x))   'highest just for this one
                                        End If
                                    Next

                                    'temps solo
                                    If sbTemp2.Length <> 0 Then
                                        sbTemp2.Append(vbCrLf)
                                    End If

                                    sbTemp2.Append(jp1.Value(Of String)("ID") & ": H(" & z.ToString & ")" & " " & jp1.Value(Of String)("TEMP(SOLO)"))

                                    'average temperature
                                    avg.AddNumber(jp1.Value(Of Double)("TEMP(AVG)"))

                                    If sbTemp3.Length <> 0 Then
                                        sbTemp3.Append(" ")
                                    End If

                                    sbTemp3.Append(jp1.Value(Of String)("TEMP(AVG)"))

                                    'cores total
                                    If sbTemp4.Length <> 0 Then
                                        sbTemp4.Append(vbCrLf)
                                    End If

                                    sbTemp4.Append(jp1.Value(Of String)("ID") & ":" & jp1.Value(Of String)("CORES(SOLO)"))
                            End Select
                        Next

                        DisplayColumns.HTemp = y
                        DisplayColumns.Temps = sbTemp3.ToString
                        DisplayColumns.ATemp = avg.GetAverage
                        DisplayColumns.Status = sbTemp.ToString

                        DisplayColumns.Data = New System.Collections.Generic.Dictionary(Of String, String)

                        With DirectCast(DisplayColumns.Data, Dictionary(Of String, String))
                            .Add("Temps", sbTemp2.ToString)
                            .Add("ATemps", sbTemp3.ToString)
                            .Add("Cores", sbTemp4.ToString)
                        End With

                End Select

                Exit For
            Next

            sbStep.Clear()
            sbStep.Append("2.0")

#If DEBUG Then
            'MinerData.sSummary = "{""STATUS"":[{""STATUS"":""S"",""When"":1420194081,""Code"":11,""Msg"":""Summary"",""Description"":""cgminer 3.9.0""}],""SUMMARY"":[{""Elapsed"":4855,""MHS av"":91.87,""MHS 5s"":92.68,""Found Blocks"":1,""Getworks"":1201,""Accepted"":6718,""Rejected"":1119,""Hardware Errors"":361,""Utility"":83.03,""Discarded"":2290,""Stale"":12,""Get Failures"":1,""Local Work"":33046,""Remote Failures"":2,""Network Blocks"":281,""Total MH"":446027.8491,""Work Utility"":76652.61,""Difficulty Accepted"":11684812.00000000,""Difficulty Rejected"":754364.00000000,""Difficulty Stale"":120.00000000,""Best Share"":9179304,""Device Hardware%"":0.0058,""Device Rejected%"":12.1630,""Pool Rejected%"":6.0644,""Pool Stale%"":0.0010}],""id"":1}"
#End If

            j = Newtonsoft.Json.Linq.JObject.Parse(MinerData.sSummary)

            sbStep.Clear()
            sbStep.Append("2.1")

            For Each ja In j.Property("SUMMARY")
                Select Case MinerData.MinerInfo.MinerType
                    Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS1, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS3, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4, clsSupportedMinerInfo.enSupportedMinerTypes.AntminerS5

                        sbStep.Clear()
                        sbStep.Append("2.2")

                        For Each jp1 In ja
                            dr.Item("Speed(5s)") = FormatHashRate(Val(jp1.Value(Of String)("GHS 5s")) * 1000)
                            dr.Item("Speed(avg)") = FormatHashRate(Val(jp1.Value(Of String)("GHS av")) * 1000)

                            dr.Item("CurHash") = Val(jp1.Value(Of String)("GHS av")) * 1000000
                            dr.Item("HashSHA256") = Val(jp1.Value(Of String)("GHS av")) * 1000
                            dr.Item("HashScrypt") = 0

                            dr.Item("Rej%") = jp1.Value(Of String)("Pool Rejected%")
                            dr.Item("Stale%") = Format(jp1.Value(Of Integer)("Stale") / (jp1.Value(Of Integer)("Accepted") + jp1.Value(Of Integer)("Stale")) * 100, "##0.###")

                            dr.Item("Blocks") = jp1.Value(Of String)("Found Blocks")
                        Next

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP10, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP20, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP30, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP31, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP35

                        sbStep.Clear()
                        sbStep.Append("2.3")

                        For Each jp1 In ja
                            dr.Item("HWE%") = jp1.Value(Of String)("Device Hardware%") & "%"

                            dr.Item("Speed(5s)") = FormatHashRate(jp1.Value(Of String)("MHS 5s"))
                            dr.Item("Speed(avg)") = FormatHashRate(jp1.Value(Of String)("MHS av"))

                            dr.Item("CurHash") = Val(jp1.Value(Of String)("MHS av")) * 1000
                            dr.Item("HashSHA256") = Val(jp1.Value(Of String)("MHS av"))
                            dr.Item("HashScrypt") = 0

                            dr.Item("Rej%") = jp1.Value(Of String)("Pool Rejected%")
                            dr.Item("Stale%") = jp1.Value(Of String)("Pool Stale%")

                            dr.Item("Blocks") = jp1.Value(Of String)("Found Blocks")
                        Next

                    Case clsSupportedMinerInfo.enSupportedMinerTypes.InnosiliconA288MH
                        sbStep.Clear()
                        sbStep.Append("2.4")

                        For Each jp1 In ja
                            dr.Item("HWE%") = jp1.Value(Of String)("Device Hardware%") & "%"

                            dr.Item("Speed(5s)") = FormatHashRate(jp1.Value(Of String)("MHS 5s"))
                            dr.Item("Speed(avg)") = FormatHashRate(jp1.Value(Of String)("MHS av"))

                            dr.Item("CurHash") = Val(jp1.Value(Of String)("MHS av"))
                            dr.Item("HashSHA256") = 0
                            dr.Item("HashScrypt") = Val(jp1.Value(Of String)("MHS av"))

                            dr.Item("Rej%") = jp1.Value(Of String)("Device Rejected%")
                            dr.Item("Stale%") = jp1.Value(Of String)("Pool Stale%")

                            dr.Item("Blocks") = jp1.Value(Of String)("Found Blocks")
                        Next
                End Select
            Next

            sbStep.Clear()
            sbStep.Append("3.0")

#If DEBUG Then
            'MinerData.sPools = "{""STATUS"":[{""STATUS"":""S"",""When"":1420194081,""Code"":7,""Msg"":""3 Pool(s)"",""Description"":""cgminer 3.9.0""}],""POOLS"":[{""POOL"":0,""URL"":""pool1"",""Status"":""Alive"",""Priority"":0,""Quota"":1,""Long Poll"":""N"",""Getworks"":1180,""Accepted"":6584,""Rejected"":1116,""Works"":29780,""Discarded"":2280,""Stale"":0,""Get Failures"":0,""Remote Failures"":0,""User"":""worker"",""Last Share Time"":1420194079,""Diff1 Shares"":6176064,""Proxy Type"":"""",""Proxy"":"""",""Difficulty Accepted"":11659040.00000000,""Difficulty Rejected"":754304.00000000,""Difficulty Stale"":0.00000000,""Last Share Difficulty"":2048.00000000,""Has Stratum"":true,""Stratum Active"":true,""Stratum URL"":""pool1"",""Has GBT"":false,""Best Share"":9179304,""Pool Rejected%"":6.0766,""Pool Stale%"":0.0000},{""POOL"":1,""URL"":""pool2"",""Status"":""Alive"",""Priority"":1,""Quota"":1,""Long Poll"":""N"",""Getworks"":19,""Accepted"":134,""Rejected"":3,""Works"":723,""Discarded"":10,""Stale"":12,""Get Failures"":1,""Remote Failures"":2,""User"":""worker"",""Last Share Time"":1420189269,""Diff1 Shares"":26072,""Proxy Type"":"""",""Proxy"":"""",""Difficulty Accepted"":25772.00000000,""Difficulty Rejected"":60.00000000,""Difficulty Stale"":120.00000000,""Last Share Difficulty"":1024.00000000,""Has Stratum"":true,""Stratum Active"":false,""Stratum URL"":"""",""Has GBT"":false,""Best Share"":278930,""Pool Rejected%"":0.2312,""Pool Stale%"":0.4624},{""POOL"":2,""URL"":""pool3"",""Status"":""Alive"",""Priority"":2,""Quota"":1,""Long Poll"":""N"",""Getworks"":2,""Accepted"":0,""Rejected"":0,""Works"":0,""Discarded"":0,""Stale"":0,""Get Failures"":0,""Remote Failures"":0,""User"":""worker"",""Last Share Time"":0,""Diff1 Shares"":0,""Proxy Type"":"""",""Proxy"":"""",""Difficulty Accepted"":0.00000000,""Difficulty Rejected"":0.00000000,""Difficulty Stale"":0.00000000,""Last Share Difficulty"":0.00000000,""Has Stratum"":true,""Stratum Active"":false,""Stratum URL"":"""",""Has GBT"":false,""Best Share"":0,""Pool Rejected%"":0.0000,""Pool Stale%"":0.0000}],""id"":1}"
#End If

            j = Newtonsoft.Json.Linq.JObject.Parse(MinerData.sPools)

            sbStep.Clear()
            sbStep.Append("3.1")

            dBestShare = 0

            sbTemp.Clear()

            sbTemp2 = New System.Text.StringBuilder

            If IsDBNull(dr.Item("PoolData2")) = True Then
                dr.Item("PoolData2") = New System.Collections.Generic.List(Of clsPoolData)
            End If

            pdl = dr.Item("PoolData2")
            pdl.Clear()

            sbStep.Clear()
            sbStep.Append("3.2")

            For Each ja In j.Property("POOLS")
                For Each jp1 In ja
                    Select Case MinerData.MinerInfo.MinerType
                        Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS1, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2, _
                             clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS3, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1, _
                             clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4, clsSupportedMinerInfo.enSupportedMinerTypes.AntminerS5
                            sbStep.Clear()
                            sbStep.Append("3.3")

                            If jp1.Value(Of Double)("Best Share") > dBestShare Then
                                dBestShare = jp1.Value(Of Double)("Best Share")
                            End If

                            pd = New clsPoolData

                            Select Case jp1.Value(Of String)("Status")
                                Case "Alive"
                                    If sbTemp.ToString.Contains("U") = False Then
                                        dr.Item("Diff") = Format(jp1.Value(Of Double)("Last Share Difficulty"), "#,###,###")
                                    End If

                                    sbTemp.Append("U")

                                    pd.PoolIsUp = True

                                Case "Dead"
                                    sbTemp.Append("D")

                                    pd.PoolIsUp = False

                                Case Else
                                    sbTemp.Append("?")

                                    pd.PoolIsUp = False

                            End Select

                            sbStep.Clear()
                            sbStep.Append("3.4")

                            If sbTemp2.Length <> 0 Then
                                sbTemp2.Append(vbCrLf)
                            End If

                            sbTemp2.Append(jp1.Value(Of String)("POOL") & ": " & jp1.Value(Of String)("URL") & " (" & jp1.Value(Of String)("User") & ") " & jp1.Value(Of String)("Status"))

                            pd.URL = jp1.Value(Of String)("URL")
                            pd.UID = jp1.Value(Of String)("User")

                            pdl.Add(pd)

                            sbStep.Clear()
                            sbStep.Append("3.5")

                        Case clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP10, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP20, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP30, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP31, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP35

                            sbStep.Clear()
                            sbStep.Append("3.6")

                            If jp1.Value(Of Double)("Best Share") > dBestShare Then
                                dBestShare = jp1.Value(Of Double)("Best Share")
                            End If

                            pd = New clsPoolData

                            Select Case jp1.Value(Of String)("Status")
                                Case "Alive"
                                    If sbTemp.ToString.Contains("U") = False Then
                                        dr.Item("Diff") = Format(jp1.Value(Of Double)("Last Share Difficulty"), "#,###,###")
                                    End If

                                    sbTemp.Append("U")

                                    pd.PoolIsUp = True

                                Case "Dead"
                                    sbTemp.Append("D")

                                    pd.PoolIsUp = False

                                Case Else
                                    sbTemp.Append("?")

                                    pd.PoolIsUp = False

                            End Select

                            sbStep.Clear()
                            sbStep.Append("3.7")

                            If sbTemp2.Length <> 0 Then
                                sbTemp2.Append(vbCrLf)
                            End If

                            sbTemp2.Append(jp1.Value(Of String)("POOL") & ": " & jp1.Value(Of String)("URL") & " (" & jp1.Value(Of String)("User") & ") " & jp1.Value(Of String)("Status"))

                            sbStep.Clear()
                            sbStep.Append("3.8")

                            pd.URL = jp1.Value(Of String)("URL")
                            pd.UID = jp1.Value(Of String)("User")

                            pdl.Add(pd)

                        Case clsSupportedMinerInfo.enSupportedMinerTypes.InnosiliconA288MH
                            sbStep.Clear()
                            sbStep.Append("3.9")

                            If jp1.Value(Of Double)("Best Share") > dBestShare Then
                                dBestShare = jp1.Value(Of Double)("Best Share")
                            End If

                            pd = New clsPoolData

                            Select Case jp1.Value(Of String)("Status")
                                Case "Alive"
                                    If sbTemp.ToString.Contains("U") = False Then
                                        dr.Item("Diff") = Format(jp1.Value(Of Double)("Last Share Difficulty"), "#,###,###")
                                    End If

                                    sbTemp.Append("U")

                                    pd.PoolIsUp = True

                                Case "Dead"
                                    sbTemp.Append("D")

                                    pd.PoolIsUp = False

                                Case Else
                                    sbTemp.Append("?")

                                    pd.PoolIsUp = False

                            End Select

                            sbStep.Clear()
                            sbStep.Append("3.91")

                            If sbTemp2.Length <> 0 Then
                                sbTemp2.Append(vbCrLf)
                            End If

                            sbTemp2.Append(jp1.Value(Of String)("POOL") & ": " & jp1.Value(Of String)("URL") & " (" & jp1.Value(Of String)("User") & ") " & jp1.Value(Of String)("Status"))

                            sbStep.Clear()
                            sbStep.Append("3.92")

                            pd.URL = jp1.Value(Of String)("URL")
                            pd.UID = jp1.Value(Of String)("User")

                            pdl.Add(pd)

                    End Select
                Next

                Exit For
            Next

            sbStep.Clear()
            sbStep.Append("3.9")

            dr.Item("BestShare") = Format(dBestShare, "###,###,###,###,###,##0")
            dr.Item("Pools") = sbTemp.ToString
            dr.Item("PoolData") = sbTemp2.ToString

            sbTemp.Append("4.0")

            j = Newtonsoft.Json.Linq.JObject.Parse(MinerData.sConfig)

            sbStep.Clear()
            sbStep.Append("2.1")

            For Each ja In j.Property("CONFIG")
                Select Case MinerData.MinerInfo.MinerType
                    Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS1, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2,
                         clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS3, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4, clsSupportedMinerInfo.enSupportedMinerTypes.AntminerS5,
                         clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP10, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP20, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP30, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP31, _
                         clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP35

                        DisplayColumns.Queue = ja(0).Value(Of String)("Queue")

                End Select
            Next

            If dr("ID") = -1 Then
                dr("ID") = MinerConfig("ID")

                ds.Tables(0).Rows.Add(dr)
            End If

            sbStep.Clear()
            sbStep.Append("4.0")

            Call HandleAlerts(dr, MinerConfig, Nothing)

            Call RefreshTitle()
        Catch ex As Exception When bErrorHandle = True
            dr.Item("Uptime") = "ERROR"

            AddToLogQueue("ERROR when querying " & MinerConfig("Name") & " (step " & sbStep.ToString & "): " & ex.Message)

            If Me.chkRebootAntOnError.Checked = True Then
                AddToLogQueue("Attempting to reboot " & MinerConfig("Name") & " via the web because the API query errored out.")

                Call RebootMiner(MinerConfig, False, YNtoBoolean(MinerConfig("RebootViaSSH")), Nothing)
            End If
        End Try

    End Sub

    'this is what does the work to get the data from the Ants via the API
    'it then passes it back to the UI thread for display
    Private Sub GetMinerDataViaAPI(ByRef MinerToCheck As stMinerConfig)

        'Dim sTemp As String
        'Dim x As Integer
        Dim bStep As Byte
        'Dim s() As String
        Dim MinerData As clsMinerRefreshData

        MinerData = New clsMinerRefreshData

        If bShutDown = True Then Exit Sub

        Try
            MinerData.MinerInfo = MinerToCheck.MinerInfo

            bStep = 1

            MinerData.sStats = Replace(GetIPData(MinerToCheck.sIP, MinerToCheck.sAPIPort, "stats"), "}{", "},{")

            'fix mangled JSON
            'x = InStr(sTemp, "}{")

            'If x <> 0 Then
            '    MinerData.sStats = sTemp.Insert(x, ",")
            'Else
            '    MinerData.sStats = sTemp
            'End If

            bStep = 2

            MinerData.sSummary = GetIPData(MinerToCheck.sIP, MinerToCheck.sAPIPort, "summary")

            bStep = 3

            MinerData.sPools = GetIPData(MinerToCheck.sIP, MinerToCheck.sAPIPort, "pools")

            bStep = 4

            MinerData.sConfig = GetIPData(MinerToCheck.sIP, MinerToCheck.sAPIPort, "config")

            MinerData.ID = MinerToCheck.ID

            SyncLock MinerRefreshLock
                MinerRefreshDataQueue.Enqueue(MinerData)
            End SyncLock
        Catch ex As Exception
            MinerData.bError = True
            MinerData.ex = ex

            AddToLogQueue("ERROR when querying " & MinerToCheck.sIP & " (step " & bStep & "): " & ex.Message)
        End Try

    End Sub

    Private Sub HandleAlerts(ByRef drMiner As DataRow, ByRef drMinerConfig As DataRow, ByRef wb As WebBrowser)

        Dim x, iHash As Integer
        Dim dr As DataGridViewRow
        Dim iAlertCount, iMinerAlertCount As Integer
        Dim bStep As Byte
        Dim colHighlightColumns As System.Collections.Generic.List(Of Integer)
        Dim bFound As Boolean

        'alert logic
        Try
            For Each dr In Me.dataMiners.Rows
                If dr.Cells("ID").Value.ToString = drMiner.Item("ID").ToString Then
                    bFound = True

                    Exit For
                End If
            Next

            If bFound = False Then
                AddToLogQueue("Miner " & drMiner("ID") & " not found in HandleAlerts!")

                Exit Sub
            End If

            If IsDBNull(dr.Cells("Uptime").Value) = False AndAlso dr.Cells("Uptime").Value <> "ERROR" AndAlso dr.Cells("Uptime").Value <> "???" Then
                iMinerAlertCount = 0

                If dr.Tag Is Nothing Then
                    dr.Tag = New System.Collections.Generic.List(Of Integer)
                End If

                colHighlightColumns = dr.Tag
                colHighlightColumns.Clear()

                For Each MinerInfo As clsSupportedMinerInfo.clsMinerInfo In SupportedMinerInfo.SupportedMinerCollection
                    If MinerInfo.ShortName = drMinerConfig("Type") Then
                        'temp check available for this miner type?
                        bStep = 1

                        If MinerInfo.AlertTypes.Temps = True Then
                            'temp check enabled for this miner type?
                            If MinerInfo.AlertValues.TempHigh.Enabled.Value = True Then
                                'compare
                                x = Val(MinerInfo.AlertValues.TempHigh.Item.Value)

                                If x > 0 Then
                                    If Integer.Parse(dr.Cells("HTemp").Value) >= x Then
                                        iMinerAlertCount += 1

                                        colHighlightColumns.Add(dr.Cells("HTemp").ColumnIndex)

                                        Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " celcius", MinerInfo.ShortName & " Temp Alert (> " & MinerInfo.AlertValues.TempHigh.Item.Value & ")")
                                    End If
                                End If
                            End If
                        End If

                        'available?
                        bStep = 2

                        If MinerInfo.AlertTypes.Fans = True Then
                            'enabled for this miner type?
                            If MinerInfo.AlertValues.FanHigh.Enabled.Value = True Then
                                'compare
                                x = Val(MinerInfo.AlertValues.FanHigh.Item.Value)

                                If x > 0 Then
                                    If Integer.Parse(dr.Cells("HFan").Value) >= x Then
                                        iMinerAlertCount += 1

                                        colHighlightColumns.Add(dr.Cells("HFan").ColumnIndex)

                                        Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " RPM", MinerInfo.ShortName & " Fan Alert (> " & MinerInfo.AlertValues.FanHigh.Item.Value & ")")
                                    End If
                                End If
                            End If

                            'available?
                            bStep = 3

                            'enabled for this miner type?
                            If MinerInfo.AlertValues.FanLow.Enabled.Value = True Then
                                'compare
                                x = Val(MinerInfo.AlertValues.FanLow.Item.Value)

                                If x > 0 Then
                                    If Integer.Parse(dr.Cells("HFan").Value) <= x Then
                                        iMinerAlertCount += 1

                                        colHighlightColumns.Add(dr.Cells("HFan").ColumnIndex)

                                        Call ProcessAlerts(dr, dr.Cells("Name").Value & " is below " & x & " RPM", MinerInfo.ShortName & " Fan Alert (< " & MinerInfo.AlertValues.FanHigh.Item.Value & ")")
                                    End If
                                End If
                            End If
                        End If

                        bStep = 4

                        If MinerInfo.AlertTypes.Hash = True Then
                            'enabled for this miner type?
                            If MinerInfo.AlertValues.HashLow.Enabled.Value = True Then
                                'compare
                                x = Val(MinerInfo.AlertValues.HashLow.Item.Value)

                                Select Case MinerInfo.HashType
                                    Case clsSupportedMinerInfo.enHashType.SHA256
                                        iHash = dr.Cells("HashSHA256").Value

                                    Case clsSupportedMinerInfo.enHashType.Scrypt
                                        iHash = dr.Cells("HashScrypt").Value

                                End Select

                                If iHash <= x Then
                                    iMinerAlertCount += 1

                                    colHighlightColumns.Add(dr.Cells("Speed(avg)").ColumnIndex)

                                    Call ProcessAlerts(dr, dr.Cells("Name").Value & " less than " & x & " " & MinerInfo.SpeedTypeString, MinerInfo.ShortName & " Hash Alert (< " & MinerInfo.AlertValues.HashLow.Item.Value & ")")

                                    If Me.chkAlertRebootAntsOnHashAlert.Checked = True Then
                                        If drMinerConfig("RebootViaSSH") = "Y" Then
                                            Call RebootMiner(drMinerConfig, False, True, Nothing)
                                        Else
                                            Call RebootMiner(drMinerConfig, False, False, wb)
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        bStep = 5

                        If MinerInfo.AlertTypes.XCount = True Then
                            'enabled for this miner type?
                            If MinerInfo.AlertValues.XCount.Enabled.Value = True Then
                                'compare
                                x = Val(MinerInfo.AlertValues.XCount.Item.Value)

                                If x > 0 Then
                                    If Integer.Parse(dr.Cells("XCount").Value.ToString.LeftMost(1)) >= x Then
                                        iMinerAlertCount += 1

                                        colHighlightColumns.Add(dr.Cells("XCount").ColumnIndex)

                                        Call ProcessAlerts(dr, dr.Cells("Name").Value & " exceeded " & x & " X count", MinerInfo.ShortName & " XCount Alert (< " & MinerInfo.AlertValues.XCount.Item.Value & ")")

                                        'use SSH only if using the API, as the web code has its own reboot logic
                                        If Me.chkAlertRebootIfXd.Checked = True Then
                                            If drMinerConfig("RebootViaSSH") = "Y" Then
                                                Call RebootMiner(drMinerConfig, False, True, Nothing)
                                            Else
                                                Call RebootMiner(drMinerConfig, False, False, wb)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        Exit For
                    End If
                Next

                dr.Cells("ACount").Value = iMinerAlertCount

                If dr.Tag IsNot Nothing Then
                    colHighlightColumns = dr.Tag

                    dr.Cells("HTemp").Style.BackColor = New Color
                    dr.Cells("HFan").Style.BackColor = New Color
                    dr.Cells("Speed(avg)").Style.BackColor = New Color
                    dr.Cells("XCount").Style.BackColor = New Color

                    For Each x In colHighlightColumns
                        dr.Cells(x).Style.BackColor = Color.Red
                    Next
                End If
            End If

            iAlertCount = 0

            For Each dr In Me.dataMiners.Rows
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
    ''' <Returns>True if the action needs to be performed</Returns>
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
    Private Sub RebootMiner(ByRef MinerConfigRow As DataRow, ByVal bRebootNow As Boolean, ByVal bUseSSH As Boolean, ByRef wbToUse As WebBrowser)

        Dim t As Threading.Thread
        Dim x As Integer

        If MinerConfigRow("DoNotReboot") = "Y" Then
            AddToLogQueue("Not rebooting " & MinerConfigRow("Name") & " because it is marked as Do Not Reboot")

            Exit Sub
        End If

        If TryGovernor(RebootInfo, MinerConfigRow("ID"), Me.cmbAlertRebootGovernor, Me.txtAlertRebootGovernor, 30 * 60) = True Then
            bRebootNow = True
        Else
            If bRebootNow = False Then
                AddToLogQueue("Need to reboot " & MinerConfigRow("Name") & " but it hasn't been long enough since last reboot")
            End If
        End If

        If bRebootNow = True Then
            If bUseSSH = True Then
                t = New Threading.Thread(AddressOf Me._RebootMinerBySSH)

                AddToLogQueue("REBOOTING " & MinerConfigRow("Name") & " via SSH")

                t.Start(GetMinerConfigByConfigRow(MinerConfigRow))
            Else
                AddToLogQueue("REBOOTING " & MinerConfigRow("Name") & " via Web")

                If wbToUse Is Nothing Then
                    While wbData(0).IsBusy = True AndAlso wbData(1).IsBusy = True AndAlso wbData(2).IsBusy = True
                        My.Application.DoEvents()
                    End While

                    'browser logic
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
                        wbToUse.Tag = MinerConfigRow
                    End If
                End If

                Select Case MinerConfigRow("Type")
                    Case "S1", "S3"
                        wbToUse.Navigate("http://" & MinerConfigRow("IPAddress") & ":" & MinerConfigRow("HTTPPort") & "/cgi-bin/luci/;stok=/admin/system/reboot?reboot=1")

                    Case "S2", "S4"
                        wbToUse.Navigate(String.Format("http://{0}:{1}@" & MinerConfigRow("IPAddress") & ":" & MinerConfigRow("HTTPPort") & "/reboot.html", MinerConfigRow("WebUsername"), MinerConfigRow("WebPassword"), Nothing, Nothing, GetHeader))

                End Select
            End If
        End If

    End Sub

    'does the actual rebooting on a separate thread
    Private Sub _RebootMinerBySSH(ByVal MinerConfig As stMinerConfig)

        Dim ssh As Renci.SshNet.SshClient
        Dim sshCommand As Renci.SshNet.SshCommand
        'Dim sUN, sPW As String

        Try
            'sAnt = RemoveAntPort(sAnt)

            'Call GetSSHCredentials(sAnt, sUN, sPW)

            ssh = New Renci.SshNet.SshClient(MinerConfig.sIP, MinerConfig.sSSHPort, MinerConfig.sSSHUsername, MinerConfig.sSSHPassword)
            ssh.Connect()

            sshCommand = ssh.CreateCommand("/sbin/reboot")
            sshCommand.Execute()

            If sshCommand.Error.IsNullOrEmpty = False Then
                AddToLogQueue("Reboot of " & MinerConfig.sName & " appears to have failed: " & sshCommand.Error)
            Else
                AddToLogQueue("Reboot of " & MinerConfig.sName & " appears to have succeeded")
            End If

            ssh.Disconnect()
            ssh.Dispose()

            sshCommand.Dispose()
        Catch ex As Exception
            AddToLogQueue("Reboot of " & MinerConfig.sName & " FAILED: " & ex.Message)
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

    Private Sub GetInfoDataOnOtherThread()

        Dim s, s1, s2, s3 As String

        s = GetIPData(sIPToGetInfo, "4028", "stats")
        s1 = GetIPData(sIPToGetInfo, "4028", "pools")
        s2 = GetIPData(sIPToGetInfo, "4028", "summary")
        s3 = GetIPData(sIPToGetInfo, "4028", "config")

        listOfGetInfoResponse.Add(s)
        listOfGetInfoResponse.Add(s1)
        listOfGetInfoResponse.Add(s2)
        listOfGetInfoResponse.Add(s3)

    End Sub

    'network code, used by the API querying routine
    Private Function GetIPData(ByVal sIP As String, ByVal sPort As String, ByVal sCommand As String, Optional ByVal sParm As String = "") As String

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

            If sParm.IsNullOrEmpty = True Then
                b = System.Text.Encoding.ASCII.GetBytes("{""command"":""" & sCommand & """}" & vbCrLf)
            Else
                b = System.Text.Encoding.ASCII.GetBytes("{""command"":""" & sCommand & """,""parameter"":""" & sParm & """}" & vbCrLf)
            End If

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

        Dim x, y As Integer
        Dim t As Threading.Thread
        Dim dStart As Date
        Dim j, jp1 As Newtonsoft.Json.Linq.JObject
        Dim ja As Newtonsoft.Json.Linq.JArray
        Dim MinerType As clsSupportedMinerInfo.enSupportedMinerTypes
        Dim sMiner As String
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
                            'y = InStr(sIPDataResponse, """Type"":""S3""}{""STATS")

#If DEBUG Then
                            'sIPDataResponse = "{""STATUS"":[{""STATUS"":""S"",""When"":1420194081,""Code"":70,""Msg"":""CGMiner stats"",""Description"":""cgminer 3.9.0""}],""STATS"":[{""STATS"":0,""ID"":""BA10"",""Elapsed"":4855,""CS"":0,""ASIC"":8,""CORES(TOTAL)"":432,""CORES(SOLO)"":""54-54-54-54-54-54-54-54"",""TEMP(AVG)"":31.25,""TEMP(SOLO)"":""30-34-29-29-34-28-46-50""},{""STATS"":1,""ID"":""BA11"",""Elapsed"":4855,""CS"":1,""ASIC"":8,""CORES(TOTAL)"":432,""CORES(SOLO)"":""54-54-54-54-54-54-54-54"",""TEMP(AVG)"":31.50,""TEMP(SOLO)"":""29-34-29-34-34-34-44-43""},{""STATS"":2,""ID"":""BA12"",""Elapsed"":4855,""CS"":2,""ASIC"":8,""CORES(TOTAL)"":389,""CORES(SOLO)"":""53-53-37-53-49-40-53-51"",""TEMP(AVG)"":31.25,""TEMP(SOLO)"":""29-29-29-34-29-34-47-48""},{""STATS"":3,""ID"":""BA13"",""Elapsed"":4855,""CS"":3,""ASIC"":8,""CORES(TOTAL)"":432,""CORES(SOLO)"":""54-54-54-54-54-54-54-54"",""TEMP(AVG)"":32.00,""TEMP(SOLO)"":""29-34-29-34-34-34-43-48""},{""STATS"":4,""ID"":""BA14"",""Elapsed"":4855,""CS"":4,""ASIC"":8,""CORES(TOTAL)"":432,""CORES(SOLO)"":""54-54-54-54-54-54-54-54"",""TEMP(AVG)"":29.62,""TEMP(SOLO)"":""29-29-29-34-34-29-36-46""},{""STATS"":5,""ID"":""BA15"",""Elapsed"":4855,""CS"":5,""ASIC"":8,""CORES(TOTAL)"":432,""CORES(SOLO)"":""54-54-54-54-54-54-54-54"",""TEMP(AVG)"":29.88,""TEMP(SOLO)"":""29-29-29-29-29-34-43-46""},{""STATS"":6,""ID"":""POOL0"",""Elapsed"":4855,""Pool Calls"":0,""Pool Attempts"":0,""Pool Wait"":0.000000,""Pool Max"":0.000000,""Pool Min"":99999999.000000,""Pool Av"":0.000000,""Work Had Roll Time"":false,""Work Can Roll"":false,""Work Had Expire"":false,""Work Roll Time"":0,""Work Diff"":2048.00000000,""Min Diff"":16.00000000,""Max Diff"":2048.00000000,""Min Diff Count"":313,""Max Diff Count"":28835,""Times Sent"":3852,""Bytes Sent"":426659,""Times Recv"":8891,""Bytes Recv"":883114,""Net Bytes Sent"":426659,""Net Bytes Recv"":883114},{""STATS"":7,""ID"":""POOL1"",""Elapsed"":4855,""Pool Calls"":0,""Pool Attempts"":0,""Pool Wait"":0.000000,""Pool Max"":0.000000,""Pool Min"":99999999.000000,""Pool Av"":0.000000,""Work Had Roll Time"":false,""Work Can Roll"":false,""Work Had Expire"":false,""Work Roll Time"":0,""Work Diff"":1024.00000000,""Min Diff"":20.00000000,""Max Diff"":1024.00000000,""Min Diff Count"":533,""Max Diff Count"":218,""Times Sent"":159,""Bytes Sent"":19658,""Times Recv"":187,""Bytes Recv"":17716,""Net Bytes Sent"":19658,""Net Bytes Recv"":17716},{""STATS"":8,""ID"":""POOL2"",""Elapsed"":4855,""Pool Calls"":0,""Pool Attempts"":0,""Pool Wait"":0.000000,""Pool Max"":0.000000,""Pool Min"":99999999.000000,""Pool Av"":0.000000,""Work Had Roll Time"":false,""Work Can Roll"":false,""Work Had Expire"":false,""Work Roll Time"":0,""Work Diff"":4096.00000000,""Min Diff"":4096.00000000,""Max Diff"":4096.00000000,""Min Diff Count"":1,""Max Diff Count"":1,""Times Sent"":2,""Bytes Sent"":150,""Times Recv"":5,""Bytes Recv"":1385,""Net Bytes Sent"":150,""Net Bytes Recv"":1385}],""id"":1}"
#End If

                            y = InStr(sIPDataResponse, "}{")

                            If y <> 0 Then
                                sIPDataResponse = sIPDataResponse.Insert(y, ",")
                            End If

                            j = Newtonsoft.Json.Linq.JObject.Parse(sIPDataResponse)

                            MinerType = 0

                            For Each ja In j.Property("STATS")
                                jp1 = ja(0)

                                Select Case jp1.Value(Of String)("ID")
                                    Case "BA10"
                                        MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.InnosiliconA288MH

                                        sMiner = SupportedMinerInfo.InnosiloconA288.ShortName & ": " & sIPToCheck

                                    Case "BTM0"
                                        'could be S2, C1, S4, or S5
                                        Select Case HowManyInString(jp1.Value(Of String)("chain_acs1"), " ")
                                            Case 8
                                                MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2

                                                sMiner = SupportedMinerInfo.AntMinerS2.ShortName & ": " & sIPToCheck

                                            Case 2
                                                MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1

                                                sMiner = SupportedMinerInfo.AntMinerC1.ShortName & ": " & sIPToCheck

                                            Case 6
                                                MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4

                                                sMiner = SupportedMinerInfo.AntMinerS4.ShortName & ": " & sIPToCheck

                                            Case 4
                                                MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntminerS5

                                                sMiner = SupportedMinerInfo.AntMinerS5.ShortName & ": " & sIPToCheck

                                        End Select

                                    Case "BMM0"
                                        'probably S3
                                        If HowManyInString(jp1.Value(Of String)("chain_acs1"), " ") = 1 Then
                                            MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS3

                                            sMiner = SupportedMinerInfo.AntMinerS3.ShortName & ": " & sIPToCheck
                                        End If

                                    Case "ANT0"
                                        'probably S1
                                        If HowManyInString(jp1.Value(Of String)("chain_acs1"), " ") = 3 Then
                                            MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS1

                                            sMiner = SupportedMinerInfo.AntMinerS4.ShortName & ": " & sIPToCheck
                                        End If

                                    Case "S300"
                                        'Spondoolie of some sort
                                        MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP20

                                        sMiner = SupportedMinerInfo.SpondoolieSP20.ShortName & ": " & sIPToCheck

                                    Case ""
                                        'might be on second node of the array
                                        jp1 = ja(1)

                                        Select Case jp1.Value(Of String)("ID")
                                            Case "BTM0"
                                                'could be S2, S4, C1, or S5
                                                Select Case HowManyInString(jp1.Value(Of String)("chain_acs1"), " ")
                                                    Case 8
                                                        MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2

                                                        sMiner = SupportedMinerInfo.AntMinerS2.ShortName & ": " & sIPToCheck

                                                    Case 2
                                                        MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1

                                                        sMiner = SupportedMinerInfo.AntMinerC1.ShortName & ": " & sIPToCheck

                                                    Case 6
                                                        MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4

                                                        sMiner = SupportedMinerInfo.AntMinerS4.ShortName & ": " & sIPToCheck

                                                    Case 4
                                                        MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntminerS5

                                                        sMiner = SupportedMinerInfo.AntMinerS5.ShortName & ": " & sIPToCheck

                                                End Select

                                            Case "BMM0"
                                                'probably S3
                                                If HowManyInString(jp1.Value(Of String)("chain_acs1"), " ") = 2 Then
                                                    MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS3

                                                    sMiner = SupportedMinerInfo.AntMinerS3.ShortName & ": " & sIPToCheck
                                                End If

                                            Case "ANT0"
                                                'probably S1
                                                If HowManyInString(jp1.Value(Of String)("chain_acs1"), " ") = 3 Then
                                                    MinerType = clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS1

                                                    sMiner = SupportedMinerInfo.AntMinerS4.ShortName & ": " & sIPToCheck
                                                End If

                                        End Select
                                End Select
                            Next

                            If MinerType <> 0 Then
                                For Each dr As DataRow In Me.dsMinerConfig.Tables(0).Rows
                                    If dr("IPAddress") = sIPToCheck Then
                                        bFound = True

                                        Exit For
                                    End If
                                Next

                                If bFound = False Then
                                    Me.txtMinerAddress.Text = sIPToCheck
                                    Me.txtMinerName.Text = ""

                                    Me.txtMinerSSHUsername.Text = "root"

                                    Select Case MinerType
                                        Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS1, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS3, _
                                             clsSupportedMinerInfo.enSupportedMinerTypes.AntminerS5
                                            Me.txtMinerSSHPassword.Text = "root"
                                            Me.txtMinerWebPassword.Text = "root"
                                            Me.txtMinerWebUsername.Text = "root"

                                        Case clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS2, clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerC1, _
                                             clsSupportedMinerInfo.enSupportedMinerTypes.AntMinerS4
                                            Me.txtMinerSSHPassword.Text = "admin"
                                            Me.txtMinerWebPassword.Text = "root"
                                            Me.txtMinerWebUsername.Text = "root"

                                        Case clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP10, _
                                             clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP20, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP30, _
                                             clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP31, clsSupportedMinerInfo.enSupportedMinerTypes.SpondooliesSP35
                                            Me.txtMinerWebPassword.Text = "admin"
                                            Me.txtMinerWebUsername.Text = "admin"
                                            Me.txtMinerSSHUsername.Text = ""
                                            Me.txtMinerSSHPassword.Text = ""

                                        Case clsSupportedMinerInfo.enSupportedMinerTypes.InnosiliconA288MH
                                            Me.txtMinerWebPassword.Text = ""
                                            Me.txtMinerWebUsername.Text = ""
                                            Me.txtMinerSSHUsername.Text = ""
                                            Me.txtMinerSSHPassword.Text = ""

                                    End Select

                                    Me.cmbMinerType.Text = SupportedMinerInfo.GetMinerObjectByType(MinerType).LongName

                                    AddToLogQueue(Me.cmbMinerType.Text & " found at " & sIPToCheck & "!")

                                    Call AddOrSaveMinerLogic(True, -1)
                                End If
                            End If
                        Catch ex As Exception
                            Debug.Print("Scan failed: " & x)
                        End Try
                    End If
                Next

                bStopRequested = False
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
        End Try

    End Sub

    Private Class eWebClient

        Inherits System.Net.WebClient

        Protected Overrides Function GetWebRequest(address As System.Uri) As System.Net.WebRequest
            Dim w As System.Net.WebRequest

            w = MyBase.GetWebRequest(address)
            w.Timeout = 5000

            Return w
        End Function

    End Class

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
            .SetRegKeyByControl(Me.chkShowQueue)
            .SetRegKeyByControl(Me.chkShowATemp)

            .SetRegKeyByControl(Me.chkShowSelectionColumn)

            '.SetRegKeyByControl(Me.chkUseAPI)

            .SetRegKeyByControl(Me.trackThreadCount)
            .SetRegKeyByControl(Me.txtDisplayRefreshInSecs)

            .SetRegKeyByControl(Me.txtMMEmail)
            .SetRegKeyByControl(Me.txtMMAppKey)
            .SetRegKeyByControl(Me.chkMMDashHistCheck)
        End With

    End Sub

    Private Sub cmdAddAnt_Click(sender As Object, e As System.EventArgs) Handles cmdAddAnt.Click

        Call AddOrSaveMinerLogic(True, -1)

    End Sub

    Private Function chkBoxToYN(ByRef chkAny As CheckBox) As String

        If chkAny.Checked = True Then
            Return "Y"
        Else
            Return "N"
        End If

    End Function

    Private Function YNtoBoolean(ByVal sValue As Object) As Boolean

        If IsDBNull(sValue) = True Then
            Return False
        End If

        If sValue = "Y" Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Sub AddOrSaveMinerLogic(ByVal bAddNewAnt As Boolean, ByVal ID As Integer)

        'Dim sTemp As String
        Dim bAntFound As Boolean
        'Dim MinerType As clsSupportedMinerInfo.enSupportedMinerTypes
        Dim MinerConfigRow As DataRow
        Dim MinerInfo As clsSupportedMinerInfo.clsMinerInfo

        Try
            If Me.cmbMinerType.Text.IsNullOrEmpty = True Then
                MsgBox("Please specify the miner type.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                Me.cmbMinerType.DroppedDown = True

                Me.cmbMinerType.Focus()

                Exit Sub
            End If

            If Me.txtMinerAddress.Text.IsNullOrEmpty = False Then
                Me.txtMinerAddress.Text = Me.txtMinerAddress.Text.ToLower.Replace("http://", "")

                If bAddNewAnt = True Then
                    For Each dr As DataRow In Me.dsMinerConfig.Tables(0).Rows
                        If dr("IPAddress") = Me.txtMinerAddress.Text Then
                            If dr("HTTPPort") = Me.txtMinerWebPort.Text Then
                                If MsgBox("This address/port combination seems to already exist.  Are you sure you want to add it?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                                    bAntFound = True

                                    Exit For
                                End If
                            End If
                        End If
                    Next
                End If

                If bAntFound = False Then
                    MinerInfo = SupportedMinerInfo.GetMinerObjectByLongName(Me.cmbMinerType.Text)

                    If Me.txtMinerName.Text.IsNullOrEmpty = True Then
                        Me.txtMinerName.Text = MinerInfo.ShortName & ": " & Me.txtMinerAddress.Text
                    End If

                    If bAddNewAnt = True Then
                        MinerConfigRow = dsMinerConfig.Tables(0).NewRow
                    Else
                        MinerConfigRow = FindMinerConfig(ID)

                        If MinerConfigRow Is Nothing Then
                            If MsgBox("Did you intend to add a new miner?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo) = vbYes Then
                                MinerConfigRow = dsMinerConfig.Tables(0).NewRow
                                bAddNewAnt = True
                            Else
                                MsgBox("There doesn't seem to be an existing record to save.  Aborting.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                                Exit Sub
                            End If
                        End If
                    End If

                    MinerConfigRow("Type") = MinerInfo.ShortName

                    ID = AddOrSaveMiner(ID, Me.txtMinerName.Text, MinerInfo, Me.txtMinerAddress.Text, chkBoxToYN(Me.chkMinerActive), _
                                    Me.txtMinerWebPort.Text, Me.txtMinerWebUsername.Text, Me.txtMinerWebPassword.Text, _
                                    Me.txtMinerSSHUsername.Text, Me.txtMinerSSHPassword.Text, Me.txtMinerAPIPort.Text, _
                                    Me.txtMinerSSHPort.Text, chkBoxToYN(Me.chkMinerUseAPI), chkBoxToYN(Me.chkMinerRebootViaSSH), _
                                    chkBoxToYN(Me.chkMinerDoNotReboot), chkBoxToYN(Me.chkMinerUseHTTPS))

                    MinerConfigRow("Name") = Me.txtMinerName.Text
                    MinerConfigRow("IPAddress") = Me.txtMinerAddress.Text
                    MinerConfigRow("Active") = chkBoxToYN(Me.chkMinerActive)
                    MinerConfigRow("HTTPPort") = Me.txtMinerWebPort.Text
                    MinerConfigRow("WebUsername") = Me.txtMinerWebUsername.Text
                    MinerConfigRow("WebPassword") = Me.txtMinerWebPassword.Text
                    MinerConfigRow("SSHPort") = Me.txtMinerSSHPort.Text
                    MinerConfigRow("SSHUsername") = Me.txtMinerSSHUsername.Text
                    MinerConfigRow("SSHPassword") = Me.txtMinerSSHPassword.Text
                    MinerConfigRow("UseAPI") = chkBoxToYN(Me.chkMinerUseAPI)
                    MinerConfigRow("APIPort") = Me.txtMinerAPIPort.Text
                    MinerConfigRow("RebootViaSSH") = chkBoxToYN(Me.chkMinerRebootViaSSH)
                    MinerConfigRow("DoNotReboot") = chkBoxToYN(Me.chkMinerDoNotReboot)
                    MinerConfigRow("HTTPS") = chkBoxToYN(Me.chkMinerUseHTTPS)
                    MinerConfigRow("ID") = ID

                    If bAddNewAnt = True Then
                        dsMinerConfig.Tables(0).Rows.Add(MinerConfigRow)
                    End If
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
            If Me.dataMinerConfig.SelectedRows.Count = 0 Then
                MsgBox("Please select one or more Ants to remove first.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                Exit Sub
            End If

            Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey & "\AntsV2", True)
                For Each dr As DataGridViewRow In Me.dataMinerConfig.SelectedRows
                    key.DeleteSubKey(dr.Cells("ID").Value)

                    Me.dataMinerConfig.Rows.Remove(dr)
                Next
            End Using
        Catch ex As Exception When bErrorHandle = True
            MsgBox("An error occurred when trying to delete an Ant:" & vbCrLf & vbCrLf & ex.Message)
        End Try

    End Sub

    Private Sub cmbRefreshRate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cmbRefreshRate.KeyPress, _
        cmbAlertEMailGovernor.KeyPress, cmbAlertRebootGovernor.KeyPress, cmbRebootAllAntsAt.KeyPress, cmbRebootAllAntsAtAlso.KeyPress, _
        cmbRebootAntsByUptime.KeyPress

        e.Handled = True

    End Sub

    Private Sub NumericOnlyKeyPressHandler(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtRefreshRate.KeyPress, _
        txtAlertEMailGovernor.KeyPress, txtMinerAPIPort.KeyPress, txtMinerSSHPort.KeyPress, txtMinerWebPort.KeyPress, _
         cmbAntScanStart.KeyPress, _
        cmbAntScanStop.KeyPress, txtRebootAntsByUptime.KeyPress

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
        chkShowACount.CheckedChanged, chkShowQueue.CheckedChanged, chkShowATemp.CheckedChanged

        Dim chkAny As CheckBox

        If bStarted = False Then Exit Sub

        chkAny = DirectCast(sender, CheckBox)

        Select Case chkAny.Name
            Case "chkShowUptime"
                Me.dataMiners.Columns("Uptime").Visible = chkAny.Checked

            Case "chkShowGHs5s"
                Me.dataMiners.Columns("Speed(5s)").Visible = chkAny.Checked

            Case "chkShowGHsAvg"
                Me.dataMiners.Columns("Speed(avg)").Visible = chkAny.Checked

            Case "chkShowBlocks"
                Me.dataMiners.Columns("Blocks").Visible = chkAny.Checked

            Case "chkShowHWE"
                Me.dataMiners.Columns("HWE%").Visible = chkAny.Checked

            Case "chkShowBestShare"
                Me.dataMiners.Columns("BestShare").Visible = chkAny.Checked

            Case "chkShowPools"
                Me.dataMiners.Columns("Pools").Visible = chkAny.Checked

            Case "chkShowFans"
                Me.dataMiners.Columns("Fans").Visible = chkAny.Checked

            Case "chkShowTemps"
                Me.dataMiners.Columns("Temps").Visible = chkAny.Checked

            Case "chkShowStatus"
                Me.dataMiners.Columns("Status").Visible = chkAny.Checked

            Case "chkShowFreqs"
                Me.dataMiners.Columns("Freq").Visible = chkAny.Checked

            Case "chkShowHighFan"
                Me.dataMiners.Columns("HFan").Visible = chkAny.Checked

            Case "chkShowHighTemp"
                Me.dataMiners.Columns("HTemp").Visible = chkAny.Checked

            Case "chkShowXCount"
                Me.dataMiners.Columns("XCount").Visible = chkAny.Checked

            Case "chkShowRej"
                Me.dataMiners.Columns("Rej%").Visible = chkAny.Checked

            Case "chkShowStale"
                Me.dataMiners.Columns("Stale%").Visible = chkAny.Checked

            Case "chkShowDifficulty"
                Me.dataMiners.Columns("Diff").Visible = chkAny.Checked

            Case "chkShowACount"
                Me.dataMiners.Columns("ACount").Visible = chkAny.Checked

            Case "chkShowQueue"
                Me.dataMiners.Columns("Queue").Visible = chkAny.Checked

            Case "chkShowATemp"
                Me.dataMiners.Columns("ATemp").Visible = chkAny.Checked

            Case Else
                MsgBox(chkAny.Name & " not found!", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly)

        End Select

    End Sub

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

    Private Sub mnuShow_Click(sender As Object, e As System.EventArgs) Handles mnuShow.Click

        Me.Show()
        Me.Focus()

    End Sub

    Private Sub cmdSaveAlerts_Click(sender As System.Object, e As System.EventArgs) Handles _
        cmdSaveAlerts3.Click, cmdSaveAlerts4.Click, cmdSaveReboots.Click, tabAlertsAndReboots.Click, cmdSaveAlerts6.Click

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

            For Each MinerInfo As clsSupportedMinerInfo.clsMinerInfo In SupportedMinerInfo.SupportedMinerCollection
                If Me.cmbAlertMinerType.Text.ToLower = MinerInfo.LongName.ToLower Then
                    If Me.chkAlertHighFan.Checked = True AndAlso Me.txtAlertHighFanValue.Text.IsNullOrEmpty = True Then
                        Me.chkAlertHighFan.Checked = False

                        MsgBox("Please specify a high fan alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")
                    End If

                    If Me.chkAlertLowFan.Checked = True AndAlso Me.txtAlertLowFanValue.Text.IsNullOrEmpty = True Then
                        Me.chkAlertLowFan.Checked = False

                        MsgBox("Please specify a low fan alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")
                    End If

                    If Me.chkAlertTempHigh.Checked = True AndAlso Me.txtAlertTempHighValue.Text.IsNullOrEmpty = True Then
                        Me.chkAlertTempHigh.Checked = False

                        MsgBox("Please specify a high temp alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")
                    End If

                    If Me.chkAlertHashLow.Checked = True AndAlso Me.txtAlertHashLowValue.Text.IsNullOrEmpty = True Then
                        Me.chkAlertHashLow.Checked = False

                        MsgBox("Please specify a low hash alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")
                    End If

                    If Me.chkAlertXCount.Checked = True AndAlso Me.txtAlertXCountValue.Text.IsNullOrEmpty = True Then
                        Me.chkAlertXCount.Checked = False

                        MsgBox("Please specify a X count alert value.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Oops!")
                    End If

                    MinerInfo.AlertValues.FanHigh.SaveSettings(Me.chkAlertHighFan.Checked, Me.txtAlertHighFanValue.Text)
                    MinerInfo.AlertValues.FanLow.SaveSettings(Me.chkAlertLowFan.Checked, Me.txtAlertLowFanValue.Text)
                    MinerInfo.AlertValues.TempHigh.SaveSettings(Me.chkAlertTempHigh.Checked, Me.txtAlertTempHighValue.Text)
                    MinerInfo.AlertValues.HashLow.SaveSettings(Me.chkAlertHashLow.Checked, Me.txtAlertHashLowValue.Text)
                    MinerInfo.AlertValues.XCount.SaveSettings(Me.chkAlertXCount.Checked, Me.txtAlertXCountValue.Text)

                    Exit For
                End If
            Next

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

            'reboots
            Call .SetRegKeyByControl(Me.chkAlertRebootIfXd)
            Call .SetRegKeyByControl(Me.chkAlertRebootAntsOnHashAlert)
            Call .SetRegKeyByControl(Me.chkRebootAntOnError)
            Call .SetRegKeyByControl(Me.txtAlertRebootGovernor)
            Call .SetRegKeyByControl(Me.cmbAlertRebootGovernor)

            If Me.chkRebootAllAntsAt.Checked = True AndAlso Me.txtRebootAllAntsAt.Text.IsNullOrEmpty = True Then
                MsgBox("Reboot all Ants at X is enabled, but the related timeframe appears to be zero.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                Me.chkRebootAllAntsAt.Checked = False
            End If

            Call .SetRegKeyByControl(Me.chkRebootAllAntsAt)
            Call .SetRegKeyByControl(Me.txtRebootAllAntsAt)
            Call .SetRegKeyByControl(Me.cmbRebootAllAntsAt)

            If Me.chkRebootAllAntsAtAlso.Checked = True AndAlso Me.txtRebootAllAntsAtAlso.Text.IsNullOrEmpty = True Then
                MsgBox("Reboot all Ants at also X is enabled, but the related timeframe appears to be zero.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                Me.chkRebootAllAntsAtAlso.Checked = False
            End If

            Call .SetRegKeyByControl(Me.chkRebootAllAntsAtAlso)
            Call .SetRegKeyByControl(Me.txtRebootAllAntsAtAlso)
            Call .SetRegKeyByControl(Me.cmbRebootAllAntsAtAlso)

            If Me.chkRebootAntsByUptime.Checked = True AndAlso Me.txtRebootAntsByUptime.Text.IsNullOrEmpty = True Then
                MsgBox("Reboot all Ants by Uptime is enabled, but the related timeframe appears to be zero.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                Me.chkRebootAntsByUptime.Checked = False
            End If

            Call .SetRegKeyByControl(Me.chkRebootAntsByUptime)
            Call .SetRegKeyByControl(Me.txtRebootAntsByUptime)
            Call .SetRegKeyByControl(Me.cmbRebootAntsByUptime)
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

    Private Sub dataMiners_ColumnDisplayIndexChanged(sender As Object, e As System.Windows.Forms.DataGridViewColumnEventArgs)

        Dim dt As DataGridView

        dt = DirectCast(sender, DataGridView)

        With My.Computer.Registry
            .CurrentUser.CreateSubKey(csRegKey & "\Columns\" & dt.Name & "_DisplayIndex")
            .SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Columns\" & dt.Name & "_DisplayIndex", e.Column.Name, e.Column.DisplayIndex, Microsoft.Win32.RegistryValueKind.DWord)
        End With

    End Sub

    Private Sub dataMiners_CellContextMenuStripNeeded(sender As Object, e As System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventArgs) Handles dataMiners.CellContextMenuStripNeeded

        Dim colAnts As System.Collections.Generic.List(Of Integer)
        Dim x As Integer
        Dim AntConfigRow As DataRow

        If e.RowIndex = -1 Then Exit Sub

        AntConfigRow = FindMinerConfig(Me.dataMiners.Rows(e.RowIndex).Cells("ID").Value)

        'remove from list
        mnuAntMenu.Items(2).Text = "Remove " & Me.dataMiners.Rows(e.RowIndex).Cells("Name").Value
        mnuAntMenu.Items(2).Tag = Me.dataMiners.Rows(e.RowIndex).Cells("ID").Value
        mnuAntMenu.Items(2).Visible = True

        If AntConfigRow Is Nothing Then
            mnuAntMenu.Items(0).Visible = False
            mnuAntMenu.Items(1).Visible = False
            mnuAntMenu.Items(3).Visible = False
        Else
            If AntConfigRow("UseAPI") <> "Y" Then Exit Sub

            mnuAntMenu.Items(0).Visible = True

            '0 - reboot one
            '1 - reboot multiple
            '2 - remove from list
            '3 - shutdown s2/s4
            '4 - update pools
            mnuAntMenu.Items(0).Text = "Reboot " & Me.dataMiners.Rows(e.RowIndex).Cells("Name").Value
            mnuAntMenu.Items(0).Tag = Me.dataMiners.Rows(e.RowIndex).Cells("ID").Value

            'reboot multiple
            If Me.dataMiners.SelectedRows.Count = 0 Then
                mnuAntMenu.Items(1).Visible = False
            Else
                mnuAntMenu.Items(1).Visible = True

                mnuAntMenu.Items(1).Tag = New System.Collections.Generic.List(Of Integer)
                colAnts = mnuAntMenu.Items(1).Tag

                x = 0

                For Each dr As DataGridViewRow In Me.dataMiners.SelectedRows
                    colAnts.Add(dr.Cells("ID").Value)

                    x += 1
                Next

                If x > 1 Then
                    mnuAntMenu.Items(1).Text = "Reboot Multiple (" & x & " Miners)"
                Else
                    mnuAntMenu.Items(1).Text = "Reboot Multiple (" & x & " Miner)"
                End If
            End If

            'shutdown s2/s4
            If AntConfigRow("Type") = "S2" OrElse AntConfigRow("Type") = "S4" Then
                mnuAntMenu.Items(3).Text = "Shutdown " & Me.dataMiners.Rows(e.RowIndex).Cells("Name").Value
                mnuAntMenu.Items(3).Tag = Me.dataMiners.Rows(e.RowIndex).Cells("ID").Value
                mnuAntMenu.Items(3).Visible = True
            Else
                mnuAntMenu.Items(3).Visible = False
            End If

            'update pools
            If Me.lblPools1.Tag IsNot Nothing Then
                mnuAntMenu.Items(4).Tag = New System.Collections.Generic.List(Of Integer)
                colAnts = mnuAntMenu.Items(4).Tag

                x = 0

                If Me.dataMiners.SelectedRows.Count = 0 Then
                    colAnts.Add(Me.dataMiners.Rows(e.RowIndex).Cells("ID").Value)

                    x = 1
                Else
                    For Each dr As DataGridViewRow In Me.dataMiners.SelectedRows
                        colAnts.Add(dr.Cells("ID").Value)

                        x += 1
                    Next
                End If

                If x > 1 Then
                    mnuAntMenu.Items(4).Text = "Update Pools (" & x & " Miner)"
                Else
                    mnuAntMenu.Items(4).Text = "Update Pools (" & x & " Miner)"
                End If

                mnuAntMenu.Items(4).Visible = True
            End If
        End If

        e.ContextMenuStrip = mnuAntMenu

    End Sub

    Private Sub dataMiners_CellToolTipTextNeeded(sender As Object, e As System.Windows.Forms.DataGridViewCellToolTipTextNeededEventArgs) Handles dataMiners.CellToolTipTextNeeded

        If e.RowIndex = -1 Then Exit Sub

        Select Case e.ColumnIndex
            Case Me.dataMiners.Columns("Pools").Index
                e.ToolTipText = Me.dataMiners.Rows(e.RowIndex).Cells("PoolData").Value.ToString

            Case Me.dataMiners.Columns("Speed(avg)").Index
                If Me.dataMiners.Rows(e.RowIndex).Cells("HashSHA256").Value <> 0 Then
                    e.ToolTipText = Format(Me.dataMiners.Rows(e.RowIndex).Cells("HashSHA256").Value, "#,###,###,###,###.##") & " SHA256"
                ElseIf Me.dataMiners.Rows(e.RowIndex).Cells("HashScrypt").Value <> 0 Then
                    e.ToolTipText = Format(Me.dataMiners.Rows(e.RowIndex).Cells("HashScrypt").Value, "#,###,###,###,###.##") & " Scrypt"
                Else
                    e.ToolTipText = "NO HASH!"
                End If

            Case Me.dataMiners.Columns("ATemp").Index
                If Me.dataMiners.Rows(e.RowIndex).Cells("Type").Value.ToString = SupportedMinerInfo.InnosiloconA288.ShortName Then
                    e.ToolTipText = TryCast(Me.dataMiners.Rows(e.RowIndex).Cells("Data").Value, Dictionary(Of String, String)).Item("ATemps")
                End If

            Case Me.dataMiners.Columns("Temps").Index
                If Me.dataMiners.Rows(e.RowIndex).Cells("Type").Value.ToString = SupportedMinerInfo.InnosiloconA288.ShortName Then
                    e.ToolTipText = TryCast(Me.dataMiners.Rows(e.RowIndex).Cells("Data").Value, Dictionary(Of String, String)).Item("Temps")
                End If

            Case Me.dataMiners.Columns("Status").Index
                If Me.dataMiners.Rows(e.RowIndex).Cells("Type").Value.ToString = SupportedMinerInfo.InnosiloconA288.ShortName Then
                    e.ToolTipText = TryCast(Me.dataMiners.Rows(e.RowIndex).Cells("Data").Value, Dictionary(Of String, String)).Item("Cores")
                End If

        End Select

    End Sub

    Private Sub chkShowSelectionColumn_Click(sender As Object, e As System.EventArgs) Handles chkShowSelectionColumn.Click

        Me.dataMiners.RowHeadersVisible = Me.chkShowSelectionColumn.Checked

    End Sub

    Private Sub cmdSaveAnt_Click(sender As Object, e As System.EventArgs) Handles cmdSaveAnt.Click

        Call AddOrSaveMinerLogic(False, Me.lblMinerID.Tag)

    End Sub

    Private Sub mnuRebootAnt_Click(sender As Object, e As System.EventArgs) Handles mnuRebootAnt.Click

        Dim t As ToolStripMenuItem
        Dim AntConfigRow As DataRow

        t = sender
        AntConfigRow = FindMinerConfig(t.Tag)

        Call AddToLogQueue("Reboot of " & AntConfigRow("Name") & " requested")
        Call RebootMiner(AntConfigRow, True, YNtoBoolean(AntConfigRow("RebootViaSSH")), Nothing)

    End Sub

    Private Function FormatHashRate(ByVal sHashRate As String) As String

        Return FormatHashRate(Val(sHashRate))

    End Function

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
            AntConfigRow = FindMinerConfig(ID)
            Call AddToLogQueue("Reboot of " & AntConfigRow("Name") & " requested")
            Call RebootMiner(AntConfigRow, True, YNtoBoolean(AntConfigRow("RebootViaSSH")), Nothing)
        Next

    End Sub

    Private Sub mnuShutdownS2_Click(sender As Object, e As System.EventArgs) Handles mnuShutdownS2.Click

        Dim th As Threading.Thread
        Dim t As ToolStripMenuItem
        Dim ID As Integer

        t = sender
        ID = t.Tag

        th = New Threading.Thread(AddressOf Me._ShutdownMiner)

        th.Start(GetMinerConfigByConfigRow(FindMinerConfig(ID)))

    End Sub

    Private Sub _ShutdownMiner(ByVal MinerConfig As stMinerConfig)

        Dim ssh As Renci.SshNet.SshClient
        Dim sshCommand As Renci.SshNet.SshCommand

        Try
            AddToLogQueue("SHUTTING DOWN " & MinerConfig.sName)

            ssh = New Renci.SshNet.SshClient(MinerConfig.sIP, MinerConfig.sSSHPort, MinerConfig.sSSHUsername, MinerConfig.sSSHPassword)
            ssh.Connect()

            sshCommand = ssh.CreateCommand("/sbin/shutdown -h -P now")
            sshCommand.Execute()

            If sshCommand.Error.IsNullOrEmpty = False Then
                AddToLogQueue("Shutdown of " & MinerConfig.sName & " appears to have failed: " & sshCommand.Error)
            Else
                AddToLogQueue("Shutdown of " & MinerConfig.sName & " appears to have succeeded")
            End If

            ssh.Disconnect()
            ssh.Dispose()

            sshCommand.Dispose()

        Catch ex As Exception
            AddToLogQueue("Shutdown of " & MinerConfig.sName & " FAILED: " & ex.Message)
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

    Private Sub cmdPoolsImportFromminers_Click(sender As System.Object, e As System.EventArgs) Handles cmdPoolsImportFromMiners.Click

        Dim pd, pd2 As clsPoolData
        Dim pdl, pdl2 As System.Collections.Generic.List(Of clsPoolData)
        Dim i, c, x As Integer
        Dim bCheckAgain, bFound As Boolean
        Dim drMinerConfig As DataRow

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

        'go through each miner
        For Each dr As DataGridViewRow In Me.dataMiners.Rows
            pdl2 = dr.Cells("PoolData2").Value
            drMinerConfig = FindMinerConfig(dr.Cells("ID").Value)

            'go through each pool for each miner
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

                        Me.lstPools.AddItem(drMinerConfig("IPAddress") & " #" & x + 1, c)

                        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\" & csRegKey & "\Pools\" & c, "Description", drMinerConfig("IPAddress") & " #" & x + 1, Microsoft.Win32.RegistryValueKind.String)
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

            th.Start(GetMinerConfigByConfigRow(FindMinerConfig(ID)))
        Next

    End Sub

    Private Sub _UpdatePools(ByVal MinerConfig As stMinerConfig)

        Dim pd1, pd2, pd3 As clsPoolData
        Dim sPools As String
        Dim j As Newtonsoft.Json.Linq.JObject
        Dim ja As Newtonsoft.Json.Linq.JArray

        Try
            AddToLogQueue("Update of pool info on " & MinerConfig.sName & " requested")

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

            'plan of attack:
            'get the # of pools
            'add all of the pools to the end
            'switch to the first new pool that is active
            'remove all the original ones

            'get pool count
            sPools = GetIPData(MinerConfig.sIP, MinerConfig.sAPIPort, "pools")

            j = Newtonsoft.Json.Linq.JObject.Parse(sPools)

            For Each ja In j.Property("POOLS")
                'try to remove every pool
                For x As Integer = ja.Count - 1 To 0 Step -1
                    AddToLogQueue(GetIPData(MinerConfig.sIP, MinerConfig.sAPIPort, "removepool", x))
                Next

                Exit For
            Next

            'add all the pools
            AddToLogQueue(GetIPData(MinerConfig.sIP, MinerConfig.sAPIPort, "addpool", pd1.URL.Replace("\", "\\").Replace(",", "\,") & "," & pd1.UID.Replace("\", "\\").Replace(",", "\,") & "," & pd1.PW.Replace("\", "\\").Replace(",", "\,")))

            If pd2 IsNot Nothing Then
                AddToLogQueue(GetIPData(MinerConfig.sIP, MinerConfig.sAPIPort, "addpool", pd2.URL.Replace("\", "\\").Replace(",", "\,") & "," & pd2.UID.Replace("\", "\\").Replace(",", "\,") & "," & pd2.PW.Replace("\", "\\").Replace(",", "\,")))
            End If

            If pd3 IsNot Nothing Then
                AddToLogQueue(GetIPData(MinerConfig.sIP, MinerConfig.sAPIPort, "addpool", pd3.URL.Replace("\", "\\").Replace(",", "\,") & "," & pd3.UID.Replace("\", "\\").Replace(",", "\,") & "," & pd3.PW.Replace("\", "\\").Replace(",", "\,")))
            End If

            'give cgminer 5 seconds 
            System.Threading.Thread.Sleep(5000)

            'get the pools status
            sPools = GetIPData(MinerConfig.sIP, MinerConfig.sAPIPort, "pools")

            j = Newtonsoft.Json.Linq.JObject.Parse(sPools)

            For Each ja In j.Property("POOLS")
                'switch to each of the new pools, from bottom to top, to set the cgminer priority
                For x As Integer = ja.Count - 1 To 1 Step -1
                    AddToLogQueue(GetIPData(MinerConfig.sIP, MinerConfig.sAPIPort, "switchpool", x))
                Next

                'remove the old pool 0
                AddToLogQueue(GetIPData(MinerConfig.sIP, MinerConfig.sAPIPort, "removepool", 0))

                Exit For
            Next

            AddToLogQueue("Update request of pool info on " & MinerConfig.sName & " is complete")
        Catch ex As Exception
            AddToLogQueue("Update of pool info on " & MinerConfig.sName & " FAILED: " & ex.Message)
        End Try

    End Sub

    Private Sub txtLog_VisibleChanged(sender As Object, e As System.EventArgs) Handles txtLog.VisibleChanged

        Me.txtLog.SelectionStart = Me.txtLog.TextLength
        Me.txtLog.ScrollToCaret()

    End Sub

    'runs all the time (~every 10ms) to pass miner refresh data to the refresh routine
    'and updates the log
    Private Sub timerDoStuff_Tick(sender As System.Object, e As System.EventArgs) Handles timerDoStuff.Tick

        Me.timerDoStuff.Enabled = False

        Try
            While MinerRefreshDataQueue.Count <> 0
                SyncLock MinerRefreshLock
                    Call RefreshGrid(MinerRefreshDataQueue.Dequeue)
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
        Dim dbSHA256, dbScrypt As Double
        Dim sTemp As String

        x = 0

        Try
            For Each dg As DataGridViewRow In Me.dataMiners.Rows
                If IsDBNull(dg.Cells("Uptime").Value) = False AndAlso dg.Cells("Uptime").Value <> "ERROR" AndAlso dg.Cells("Uptime").Value <> "???" Then
                    x += 1

                    dbSHA256 += dg.Cells("HashSHA256").Value
                    dbScrypt += dg.Cells("HashScrypt").Value
                End If
            Next

            If dbSHA256 = 0 AndAlso dbScrypt = 0 Then
                sTemp = "NO HASHING!"
            ElseIf dbSHA256 <> 0 AndAlso dbScrypt = 0 Then
                sTemp = FormatHashRate(dbSHA256) & " SHA256"
            ElseIf dbSHA256 = 0 AndAlso dbScrypt <> 0 Then
                sTemp = FormatHashRate(dbScrypt) & " Scrypt"
            ElseIf dbSHA256 <> 0 AndAlso dbScrypt <> 0 Then
                sTemp = FormatHashRate(dbSHA256) & " SHA256 " & FormatHashRate(dbScrypt) & " Scrypt"
            End If

            Me.Text = csVersion & " - " & Now.ToString & " - " & x & " of " & iMinersEnabled & " responded - " & sTemp & " " & sAlerts
        Catch ex As Exception When bErrorHandle = True
            AddToLogQueue("ERROR in RefreshTitle routine: " & ex.Message)
        End Try

    End Sub

    Public Sub AddToLogQueue(ByVal sMsg As String)

        Debug.Print(sMsg)

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

    Private Sub txtAntAddress_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtMinerAddress.KeyPress

        Select Case e.KeyChar
            Case "0" To "9", vbBack, "."
                'okay

            Case Else
                e.Handled = True

        End Select

    End Sub

    Private Sub txtIPRangeToScan_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtIPRangeToScan.KeyPress, txtIPToGetInfo.KeyPress

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

    Private Sub dataAntConfig_SelectionChanged(sender As Object, e As System.EventArgs) Handles dataMinerConfig.SelectionChanged

        Dim dr As DataGridViewRow

        Try
            If Me.dataMinerConfig.SelectedRows.Count = 1 Then
                dr = Me.dataMinerConfig.SelectedRows(0)

                Me.txtMinerAddress.Text = dr.Cells("IPAddress").Value
                Me.txtMinerName.Text = dr.Cells("Name").Value

                Me.cmbMinerType.Text = SupportedMinerInfo.GetMinerObjectByShortName(dr.Cells("Type").Value).LongName

                Me.txtMinerSSHUsername.Text = dr.Cells("SSHUsername").Value
                Me.txtMinerSSHPassword.Text = dr.Cells("SSHPassword").Value
                Me.txtMinerSSHPort.Text = dr.Cells("SSHPort").Value

                Me.txtMinerWebUsername.Text = dr.Cells("WebUsername").Value
                Me.txtMinerWebPassword.Text = dr.Cells("WebPassword").Value
                Me.txtMinerWebPort.Text = dr.Cells("HTTPPort").Value

                Me.chkMinerUseAPI.Checked = YNtoBoolean(dr.Cells("UseAPI").Value)
                Me.txtMinerAPIPort.Text = dr.Cells("APIPort").Value
                Me.chkMinerRebootViaSSH.Checked = YNtoBoolean(dr.Cells("RebootViaSSH").Value)
                Me.chkMinerActive.Checked = YNtoBoolean(dr.Cells("Active").Value)
                Me.chkMinerDoNotReboot.Checked = YNtoBoolean(dr.Cells("DoNotReboot").Value)
                Me.chkMinerUseHTTPS.Checked = YNtoBoolean(dr.Cells("HTTPS").Value)

                Me.lblMinerID.Text = "ID #" & dr.Cells("ID").Value
                Me.lblMinerID.Tag = dr.Cells("ID").Value
            Else
                Me.lblMinerID.Tag = -1
            End If
        Catch ex As Exception When bErrorHandle = True
            AddToLogQueue("An error occurred when displaying the config for an Ant: " & ex.Message)
        End Try

    End Sub

    Private Sub cmbMinerType_LostFocus(sender As Object, e As System.EventArgs) Handles cmbMinerType.LostFocus, cmbAlertMinerType.LostFocus

        Dim cmbAny As ComboBox

        cmbAny = sender

        If cmbAny.Text.IsNullOrEmpty = True Then Exit Sub

        For Each MinerInfo As clsSupportedMinerInfo.clsMinerInfo In SupportedMinerInfo.SupportedMinerCollection
            If cmbAny.Text.ToLower = MinerInfo.LongName.ToLower Then
                Exit Sub
            End If
        Next

        MsgBox("Invalid miner type entered.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

        cmbAny.Text = ""

    End Sub

    Private Sub cmbMinerType_SelectedValueChanged(sender As Object, e As System.EventArgs) Handles cmbMinerType.SelectedValueChanged

        Dim s() As String
        Dim sTemp As String
        Dim MinerInfo As clsSupportedMinerInfo.clsMinerInfo

        Try
            sTemp = SupportedMinerInfo.GetMinerObjectByLongName(Me.cmbMinerType.Text).ShortName

            s = Me.txtMinerAddress.Text.Split(".")

            If Me.txtMinerName.Text.IsNullOrEmpty Then
                Me.txtMinerName.Text = sTemp & ":" & s(2) & "." & s(3)
            Else
                For Each MinerInfo In SupportedMinerInfo.SupportedMinerCollection
                    If Me.txtMinerName.Text.Substring(0, 3).ToLower = MinerInfo.ShortName.ToLower & ":" Then
                        Me.txtMinerName.Text = sTemp & ":" & Me.txtMinerName.Text.Substring(3)
                    End If
                Next
            End If
        Catch ex As Exception When bErrorHandle = True
        End Try

    End Sub

    Private Sub cmdAntClear_Click(sender As System.Object, e As System.EventArgs) Handles cmdAntClear.Click

        Me.txtMinerName.Text = ""
        Me.txtMinerAddress.Text = ""
        Me.cmbMinerType.Text = ""

        Me.txtMinerAddress.Focus()

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

    Private Sub txtRebootAllAntsAt_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtRebootAllAntsAt.KeyPress, _
        txtRebootAllAntsAtAlso.KeyPress

        Select Case e.KeyChar
            Case "0" To "9", vbBack, ":"
                'okay

            Case Else
                e.Handled = True

        End Select


    End Sub

    Private Sub txtRebootAllAntsAt_LostFocus(sender As Object, e As System.EventArgs) Handles txtRebootAllAntsAt.LostFocus, txtRebootAllAntsAtAlso.LostFocus

        Dim txtAny As TextBox
        Dim d As Date

        txtAny = sender

        If txtAny.Text.IsNullOrEmpty = False Then
            Select Case txtAny.Name
                Case "txtRebootAllAntsAt"
                    If Date.TryParse(txtAny.Text & " " & Me.cmbRebootAllAntsAt.Text, d) = False Then
                        MsgBox("Invalid value entered:" & vbCrLf & vbCrLf & txtAny.Text, MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                        Me.chkRebootAllAntsAt.Checked = False

                        txtAny.Text = ""
                    End If

                Case "txtRebootAllAntsAtAlso"
                    If Date.TryParse(txtAny.Text & " " & Me.cmbRebootAllAntsAtAlso.Text, d) = False Then
                        MsgBox("Invalid value entered." & vbCrLf & vbCrLf & txtAny.Text, MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                        Me.chkRebootAllAntsAtAlso.Checked = False

                        txtAny.Text = ""
                    End If

            End Select
        End If

    End Sub

    Private Sub txtAlertEMailGovernor_LostFocus(sender As Object, e As System.EventArgs) Handles txtRefreshRate.LostFocus, _
        txtAlertEMailGovernor.LostFocus, txtMinerAPIPort.LostFocus, txtMinerSSHPort.LostFocus, txtMinerWebPort.LostFocus, _
         txtRebootAntsByUptime.LostFocus

        Dim txtAny As TextBox

        txtAny = sender

        If txtAny.Text.IsNullOrEmpty = True Then Exit Sub

        If Val(txtAny.Text) = 0 Then
            MsgBox("Invalid value entered:" & vbCrLf & vbCrLf & txtAny.Text)

            txtAny.Text = ""
        End If

    End Sub

    Private Sub cmbText_LostFocus(sender As Object, e As System.EventArgs) Handles cmbAntScanStart.LostFocus, cmbAntScanStop.LostFocus

        Dim cmbAny As ComboBox

        cmbAny = sender

        If cmbAny.Text.IsNullOrEmpty = True Then Exit Sub

        If Val(cmbAny.Text) = 0 Then
            MsgBox("Invalid value entered:" & vbCrLf & vbCrLf & cmbAny.Text)

            cmbAny.Text = ""
        End If

    End Sub

    Private Sub chkRebootAntsByUptime_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkRebootAntsByUptime.CheckedChanged, chkRebootAllAntsAt.CheckedChanged, _
        chkRebootAllAntsAtAlso.CheckedChanged

        Dim chkAny As CheckBox
        Dim txtAny As TextBox

        chkAny = sender

        If chkAny.Checked = False Then Exit Sub

        Select Case chkAny.Name
            Case "chkRebootAntsByUptime"
                txtAny = Me.txtRebootAntsByUptime

            Case "chkRebootAllAntsAt"
                txtAny = Me.txtRebootAllAntsAt

            Case "chkRebootAllAntsAtAlso"
                txtAny = Me.txtRebootAllAntsAtAlso

        End Select

        If txtAny.Text.IsNullOrEmpty = True Then
            MsgBox("You can not enable this until a valid value is entered.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

            txtAny.Focus()

            chkAny.Checked = False
        End If

    End Sub

    Private Sub cmdGetMinerInfo_Click(sender As System.Object, e As System.EventArgs) Handles cmdGetMinerInfo.Click

        Dim t As Threading.Thread
        Dim dStart As Date
        Dim frmInfo As frmGetMinerInfo

        Try
            If Me.txtIPToGetInfo.Text.IsNullOrEmpty = True Then
                MsgBox("Please enter an exact IP address to get information on.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                Me.txtIPToGetInfo.Focus()

                Exit Sub
            End If

            Me.cmdGetMinerInfo.Enabled = False

            listOfGetInfoResponse = New System.Collections.Generic.List(Of String)

            sIPToGetInfo = Me.txtIPToGetInfo.Text

            t = New Threading.Thread(AddressOf Me.GetInfoDataOnOtherThread)

            t.Start()

            dStart = Now

            'wait 30 seconds max
            While listOfGetInfoResponse.Count <> 4 AndAlso dStart.AddSeconds(30) > Now
                My.Application.DoEvents()
            End While

            Debug.Print(listOfGetInfoResponse.Count)

            If listOfGetInfoResponse.Count <> 4 Then
                If t.IsAlive Then t.Abort()

                MsgBox("Unable to get a response from " & Me.txtIPToGetInfo.Text & ".", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)
            Else
                frmInfo = New frmGetMinerInfo
                frmInfo.Show()

                frmInfo.txt1.Text = "STATS:" & vbCrLf & listOfGetInfoResponse(0)
                frmInfo.txt2.Text = "POOLS:" & vbCrLf & listOfGetInfoResponse(1)
                frmInfo.txt3.Text = "SUMMARY:" & vbCrLf & listOfGetInfoResponse(2)
                frmInfo.txt4.Text = "CONFIG:" & vbCrLf & listOfGetInfoResponse(3)

                Clipboard.SetText(frmInfo.txt1.Text & vbCrLf & vbCrLf & frmInfo.txt2.Text & vbCrLf & vbCrLf & frmInfo.txt3.Text & vbCrLf & frmInfo.txt4.Text)
            End If
        Catch ex As Exception
            MsgBox("An error occurred: " & ex.Message)
        Finally
            Me.cmdGetMinerInfo.Enabled = True
        End Try

    End Sub

    Private Sub mobileMinerTimer_Tick(sender As Object, e As EventArgs) Handles mobileMinerTimer.Tick
        Dim emailAddress As String = txtMMEmail.Text
        Dim applicationKey As String = txtMMAppKey.Text
        Dim isConfigured As Boolean = InputValidation.IsValidEmailAddress(emailAddress) And InputValidation.IsValidApplicationKey(applicationKey)

        If isConfigured Then

            mobileMinerTimer.Enabled = False
            Try
                SubmitMobileMinerStatistics(emailAddress, applicationKey)
            Finally
                mobileMinerTimer.Enabled = True
            End Try

        End If
    End Sub

    Private Sub SubmitMobileMinerStatistics(emailAddress As String, applicationKey As String)

        Dim statistics As MobileMinerApi.Data.MiningStatistics
        Dim statisticsList As List(Of MobileMinerApi.Data.MiningStatistics) = New List(Of MobileMinerApi.Data.MiningStatistics)
        Dim pd As clsPoolData
        Dim pdl As System.Collections.Generic.List(Of clsPoolData)
        Dim s() As String

        Try
            For Each dr As DataRow In Me.ds.Tables(0).Rows
                statistics = New MobileMinerApi.Data.MiningStatistics

                Dim isScryptAsic As Object = dr.Item("HashScrypt") > 0

                statistics.Algorithm = If(isScryptAsic, "Scrypt", "SHA-256")
                statistics.Appliance = True
                statistics.AverageHashrate = If(isScryptAsic, dr.Item("HashScrypt"), dr.Item("HashSHA256"))

                If IsDBNull(dr.Item("CurHash")) = False Then
                    statistics.CurrentHashrate = dr.Item("CurHash")
                    statistics.CoinSymbol = If(isScryptAsic, "LTC", "BTC")
                    statistics.CoinName = If(isScryptAsic, "Litecoin", "Bitcoin")
                    statistics.Enabled = True

                    If IsDBNull(dr.Item("HFan")) = False Then
                        statistics.FanSpeed = dr.Item("HFan")
                    Else
                        statistics.FanSpeed = 0
                    End If

                    statistics.FullName = dr.Item("Name")
                    statistics.HardwareErrorsPercent = dr.Item("HWE%").ToString().TrimEnd("%")
                    statistics.Kind = "ASC"
                    statistics.MachineName = dr.Item("Name")
                    statistics.MinerName = "MMinerMonitor"
                    statistics.Name = dr.Item("Name")
                    statistics.Temperature = dr.Item("HTemp")

                    'all miner types may not have this ... especially for web scraping
                    pdl = TryCast(dr.Item("PoolData2"), System.Collections.Generic.List(Of clsPoolData))

                    If pdl IsNot Nothing Then
                        'if using web scraping, there could be multiple values here
                        'if there is only one value, assume that's the right one
                        'otherwise, iterate through the pools object and assume the first one that is up is the one being used
                        s = dr.Item("Rej%").ToString.Split(" ")

                        If s.Count = 1 Then
                            statistics.RejectedSharesPercent = Val(dr.Item("Rej%"))
                        Else
                            For x = 0 To pdl.Count - 1
                                pd = pdl(x)

                                If pd.PoolIsUp = True Then
                                    If s.Count >= x Then
                                        statistics.RejectedSharesPercent = Val(s(x))
                                    Else
                                        'unable to determine
                                        statistics.RejectedSharesPercent = 0
                                    End If

                                    Exit For
                                End If
                            Next
                        End If

                        For Each pd In pdl
                            If pd.PoolIsUp = True Then
                                statistics.PoolName = pd.URL

                                Exit For
                            End If
                        Next
                    Else
                        If dr.Item("Rej%").ToString.Contains(" ") = False Then
                            statistics.RejectedSharesPercent = dr.Item("Rej%")
                        Else
                            'unable to determine
                            statistics.RejectedSharesPercent = 0
                        End If

                        statistics.PoolName = "Unknown"
                    End If

                    statisticsList.Add(statistics)
                End If
            Next

            Dim commandList As List(Of MobileMinerApi.Data.RemoteCommand)
            ' no remote commands just yet - flip to True and process the commandList to enable
            Dim processCommands As Boolean = False
            Const MobileMinerUrl As String = "https://api.mobileminerapp.com"
            Const MobileMinerApiKey As String = "mCJMjV2iKtdOLT"

            commandList = MobileMinerApi.ApiContext.SubmitMiningStatistics(MobileMinerUrl, MobileMinerApiKey, emailAddress, applicationKey, statisticsList, processCommands)
        Catch ex As Exception When bErrorHandle = True
            AddToLogQueue("Error occurred submitting MobileMiner stats: " & ex.Message)
        End Try
    End Sub

    Private Sub momAppKeyLabel_Click(sender As Object, e As EventArgs) Handles lblMMAppKey.Click
        Process.Start("http://web.mobileminerapp.com/")
    End Sub

    Private Sub cmbAlertMinerType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbAlertMinerType.SelectedIndexChanged

        If Me.cmbAlertMinerType.Text.IsNullOrEmpty = True Then
            Me.chkAlertHashLow.Enabled = False
            Me.chkAlertHighFan.Enabled = False
            Me.chkAlertLowFan.Enabled = False
            Me.chkAlertXCount.Enabled = False
            Me.chkAlertTempHigh.Enabled = False

            Me.txtAlertHashLowValue.Enabled = False
            Me.txtAlertHighFanValue.Enabled = False
            Me.txtAlertLowFanValue.Enabled = False
            Me.txtAlertXCountValue.Enabled = False
            Me.txtAlertTempHighValue.Enabled = False

            Exit Sub
        End If

        For Each MinerInfo As clsSupportedMinerInfo.clsMinerInfo In SupportedMinerInfo.SupportedMinerCollection
            If Me.cmbAlertMinerType.Text.ToLower = MinerInfo.LongName.ToLower Then
                Me.chkAlertHashLow.Enabled = MinerInfo.AlertTypes.Hash
                Me.chkAlertHashLow.Checked = MinerInfo.AlertValues.HashLow.Enabled.Value
                Me.txtAlertHashLowValue.Enabled = MinerInfo.AlertTypes.Hash
                Me.txtAlertHashLowValue.Text = MinerInfo.AlertValues.HashLow.Item.Value

                Me.chkAlertHighFan.Enabled = MinerInfo.AlertTypes.Fans
                Me.chkAlertHighFan.Checked = MinerInfo.AlertValues.FanHigh.Enabled.Value
                Me.txtAlertHighFanValue.Enabled = MinerInfo.AlertTypes.Fans
                Me.txtAlertHighFanValue.Text = MinerInfo.AlertValues.FanHigh.Item.Value

                Me.chkAlertLowFan.Enabled = MinerInfo.AlertTypes.Fans
                Me.chkAlertLowFan.Checked = MinerInfo.AlertValues.FanLow.Enabled.Value
                Me.txtAlertLowFanValue.Enabled = MinerInfo.AlertTypes.Fans
                Me.txtAlertLowFanValue.Text = MinerInfo.AlertValues.FanLow.Item.Value

                Me.chkAlertXCount.Enabled = MinerInfo.AlertTypes.XCount
                Me.chkAlertXCount.Checked = MinerInfo.AlertValues.XCount.Enabled.Value
                Me.txtAlertXCountValue.Enabled = MinerInfo.AlertTypes.XCount
                Me.txtAlertXCountValue.Text = MinerInfo.AlertValues.XCount.Item.Value

                Me.chkAlertTempHigh.Enabled = MinerInfo.AlertTypes.Temps
                Me.chkAlertTempHigh.Checked = MinerInfo.AlertValues.TempHigh.Enabled.Value
                Me.txtAlertTempHighValue.Enabled = MinerInfo.AlertTypes.Temps
                Me.txtAlertTempHighValue.Text = MinerInfo.AlertValues.TempHigh.Item.Value
            End If
        Next

    End Sub

    Private Sub chkMinerUseAPI_Click(sender As Object, e As System.EventArgs) Handles chkMinerUseAPI.Click

        If Me.chkMinerUseAPI.Checked = False Then
            Select Case Me.cmbMinerType.Text
                Case SupportedMinerInfo.SpondoolieSP10.LongName, SupportedMinerInfo.SpondoolieSP20.LongName, SupportedMinerInfo.SpondoolieSP30.LongName, _
                     SupportedMinerInfo.SpondoolieSP31.LongName, SupportedMinerInfo.SpondoolieSP35.LongName

                    If Me.chkMinerUseHTTPS.Checked = False Then
                        MsgBox("If your Spondoolie miner is remote, try enabling the HTTPS option to secure your web credentials." & vbCrLf & vbCrLf & "Note this may not work with all hosting configurations.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)
                    End If

            End Select
        End If

    End Sub

    Private Sub momDashHistCheck_CheckedChanged(sender As Object, e As EventArgs) Handles chkMMDashHistCheck.CheckedChanged
        SetupMobileMinerTabVisibility()
    End Sub

    Private Sub TabControl1_Selected(sender As Object, e As TabControlEventArgs) Handles TabControl1.Selected

        Dim controller As String = Nothing

        If e.TabPage.Equals(Me.momDashTabPage) Then
            controller = MobileMinerApi.WebBrowserProvider.DashboardController
        ElseIf e.TabPage.Equals(Me.momHistTabPage) Then
            controller = MobileMinerApi.WebBrowserProvider.HistoryController
        End If

        If controller IsNot Nothing Then
            Dim webBrowser As WebBrowser = MobileMinerApi.WebBrowserProvider.GetWebBrowser(controller, txtMMEmail.Text, txtMMAppKey.Text)
            webBrowser.Dock = DockStyle.Fill
            webBrowser.Parent = e.TabPage
        End If

    End Sub

    Private Sub txtMMAppKey_LostFocus(sender As Object, e As System.EventArgs) Handles txtMMAppKey.LostFocus

        Me.txtMMAppKey.Text = Me.txtMMAppKey.Text.Trim

    End Sub

    Private Sub txtMMEmail_LostFocus(sender As Object, e As System.EventArgs) Handles txtMMEmail.LostFocus

        Me.txtMMEmail.Text = Me.txtMMEmail.Text.Trim

    End Sub
End Class

'wrapper around the datagridview to allow disabling the paint event
'this way you can populate data on the grid without it having to render (paint)
'when done, set Refreshing back to false and the next paint call will clear it up
Public Class dgvWrapper

    Inherits DataGridView

    Public Refreshing As Boolean

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)

        If Me.Refreshing = False Then
            MyBase.OnPaint(e)
        End If

    End Sub

End Class

Public Class clsAverage

    Private colNumbers As System.Collections.Generic.List(Of Integer)

    Public Sub New()
        colNumbers = New System.Collections.Generic.List(Of Integer)
    End Sub

    Public Sub AddNumber(ByVal N As Integer)
        colNumbers.Add(N)
    End Sub

    Public Function GetAverage() As Integer
        Dim N As Integer

        For Each X As Integer In colNumbers
            N += X
        Next

        Return N / colNumbers.Count

    End Function

    Public Sub Clear()
        colNumbers.Clear()
    End Sub

End Class