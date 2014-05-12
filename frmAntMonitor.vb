Imports MAntMonitor.Extensions

Public Class frmAntMonitor

    Private wb(0 To 9) As WebBrowser

    Private ds As DataSet
    Private bBrowserCount As Byte

    Private Const csRegKey As String = "Software\MAntMonitor"

    Private Const csVersion As String = "M's Ant Monitor v1.3"

    Private iCountDown, iWatchDog, bAnt As Integer

    Private iRefreshRate As Integer

    Private ctlsByKey As ControlsByRegistry

#If DEBUG Then
    Private Const bErrorHandle As Boolean = False
#Else
    Private Const bErrorHandle As Boolean = True
#End If

    Private Sub Form1_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim host As System.Net.IPHostEntry

        AddToLog(csVersion & " starting")

        host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName)

        For Each IP As System.Net.IPAddress In host.AddressList
            If IP.AddressFamily = Net.Sockets.AddressFamily.InterNetwork Then
                Me.cmbLocalIPs.Items.Add(IP.ToString)
            End If
        Next

        Me.cmbLocalIPs.Text = Me.cmbLocalIPs.Items(0)

        ds = New DataSet

        With ds
            .Tables.Add()
            Me.dataAnts.DataSource = .Tables(0)

            With .Tables(0).Columns
                .Add("Name")
                .Add("Uptime")
                .Add("GH/s(5s)")
                .Add("GH/s(avg)")
                .Add("Blocks")
                .Add("HWE%")
                .Add("BestShare")
                .Add("P0Status")
                .Add("P1Status")
                .Add("P2Status")
                .Add("Fan1")
                .Add("Temp1")
                .Add("Status1")
                .Add("Fan2")
                .Add("Temp2")
                .Add("Status2")
            End With
        End With

        ctlsByKey = New ControlsByRegistry(csRegKey)

        Call SetGridSizes("\Columns\dataAnts", Me.dataAnts)

        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey)
            If key Is Nothing Then
                My.Computer.Registry.CurrentUser.CreateSubKey(csRegKey)
            End If
        End Using

        Using key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(csRegKey)
            If key.GetValue("Width") > 100 Then
                Me.Width = key.GetValue("Width")
            End If

            If key.GetValue("Height") > 100 Then
                Me.Height = key.GetValue("Height")
            End If
        End Using

        ctlsByKey.AddControl(Me.chkRebootIfXd, "RebootAntIfXd")
        ctlsByKey.AddControl(Me.txtPassword, "Password")
        ctlsByKey.AddControl(Me.txtUserName, "Username")
        ctlsByKey.AddControl(Me.chkSavePassword, "SavePassword")
        ctlsByKey.AddControl(Me.chklstAnts, "AntList")
        ctlsByKey.AddControl(Me.txtRefreshRate, "RefreshRateValue")
        ctlsByKey.AddControl(Me.cmbRefreshRate, "RefreshRateVolume")
        ctlsByKey.AddControl(Me.cmbBrowserInstances, "BrowserInstances")
        ctlsByKey.AddControl(Me.cmbFontSize, "FontSize")

        Call ctlsByKey.SetControlByRegKey(Me.chkRebootIfXd, True)
        Call ctlsByKey.SetControlByRegKey(Me.txtPassword, "root")
        Call ctlsByKey.SetControlByRegKey(Me.txtUserName, "root")
        Call ctlsByKey.SetControlByRegKey(Me.chkSavePassword, True)
        Call ctlsByKey.SetControlByRegKey(Me.chklstAnts)
        Call ctlsByKey.SetControlByRegKey(Me.txtRefreshRate, "300")
        Call ctlsByKey.SetControlByRegKey(Me.cmbRefreshRate, "Seconds")
        Call ctlsByKey.SetControlByRegKey(Me.cmbBrowserInstances, "3")
        Call ctlsByKey.SetControlByRegKey(Me.cmbFontSize, "10")

        'check each of the boxes
        For x As Integer = 0 To Me.chklstAnts.Items.Count - 1
            Me.chklstAnts.SetItemChecked(x, True)
        Next

        Call CalcRefreshRate()

        Call RefreshGrid()

    End Sub

    Private Sub wb_completed(sender As Object, e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs)

        Dim dr As DataRow
        Dim x, y, z As Integer
        Dim sAnt As String
        Dim bAntFound As Boolean
        Dim wb As WebBrowser

        wb = sender

        'Select Case wb.Name
        '    Case "WebBrowser1"
        '        Me.lblWB1.Text = wb.Url.AbsoluteUri

        '    Case "WebBrowser2"
        '        Me.lblWB2.Text = wb.Url.AbsoluteUri

        '    Case "WebBrowser3"
        '        Me.lblWB3.Text = wb.Url.AbsoluteUri

        'End Select

        If wb.Document.All(1).OuterHtml.ToLower.Contains("authorization") Then
            wb.Document.All("username").SetAttribute("value", Me.txtUserName.Text)
            wb.Document.All("password").SetAttribute("value", Me.txtPassword.Text)
            wb.Document.All(48).InvokeMember("click")
        Else
            x = InStr(wb.Url.AbsoluteUri, "/")

            x = InStr(x + 2, wb.Url.AbsoluteUri, "/")

            While y < x
                z = InStr(y + 1, wb.Url.AbsoluteUri, ".")

                If z = 0 Then
                    If y = 0 Then
                        y = InStr(wb.Url.AbsoluteUri, "//") + 1
                    End If

                    Exit While
                Else
                    y = z
                End If
            End While

            sAnt = wb.Url.AbsoluteUri.Substring(y, x - y - 1)

            For Each dr In ds.Tables(0).Rows
                If dr.Item("Name") = sAnt Then
                    bAntFound = True

                    Exit For
                End If
            Next

            If bAntFound = False Then
                dr = ds.Tables(0).NewRow
            End If

            dr.Item("Name") = wb.Url.AbsoluteUri.Substring(y, x - y - 1)

            AddToLog(dr.Item("Name") & " responded")

            If wb.Url.AbsoluteUri.Contains("minerstatus") AndAlso wb.Document.All.Count > 75 Then
                dr.Item("Uptime") = wb.Document.All(84).Children(2).Children(0).Children(0).OuterText.TrimEnd

                If wb.Document.All(84).Children(2).Children.Count <> 1 Then
                    dr.Item("GH/s(5s)") = wb.Document.All(84).Children(2).Children(1).Children(0).OuterText.TrimEnd
                    dr.Item("GH/s(avg)") = wb.Document.All(84).Children(2).Children(2).Children(0).OuterText.TrimEnd
                    dr.Item("Blocks") = wb.Document.All(84).Children(2).Children(3).Children(0).OuterText.TrimEnd
                    dr.Item("HWE%") = Format(UInt64.Parse(wb.Document.All(84).Children(2).Children(7).Children(0).OuterText.TrimEnd.Replace(",", "")) / _
                                     (UInt64.Parse(wb.Document.All(84).Children(2).Children(13).Children(0).OuterText.TrimEnd.Replace(",", "")) + _
                                      UInt64.Parse(wb.Document.All(84).Children(2).Children(14).Children(0).OuterText.TrimEnd.Replace(",", "")) + _
                                      UInt64.Parse(wb.Document.All(84).Children(2).Children(7).Children(0).OuterText.TrimEnd.Replace(",", ""))), "##0.###%")
                    dr.Item("BestShare") = wb.Document.All(84).Children(2).Children(16).Children(0).OuterText.TrimEnd
                    dr.Item("P0Status") = wb.Document.All(192).Children(2).Children(0).Children(0).Children(2).Children(3).Children(0).OuterText.TrimEnd

                    If wb.Document.All(192).Children(2).Children(0).Children(0).Children.Count > 3 Then
                        dr.Item("P1Status") = wb.Document.All(192).Children(2).Children(0).Children(0).Children(3).Children(3).Children(0).OuterText.TrimEnd

                        If wb.Document.All(192).Children(2).Children(0).Children(0).Children.Count > 4 Then
                            dr.Item("P2Status") = wb.Document.All(192).Children(2).Children(0).Children(0).Children(4).Children(3).Children(0).OuterText.TrimEnd
                            x = 443
                        Else
                            dr.Item("P2Status") = "N/A"
                            x = 374
                        End If
                    Else
                        dr.Item("P1Status") = "N/A"
                        x = 305
                    End If

                    dr.Item("Fan1") = wb.Document.All(x).Children(2).Children(0).Children(0).Children(2).Children(3).OuterText.TrimEnd
                    dr.Item("Temp1") = wb.Document.All(x).Children(2).Children(0).Children(0).Children(2).Children(4).OuterText.TrimEnd
                    dr.Item("Status1") = wb.Document.All(x).Children(2).Children(0).Children(0).Children(2).Children(5).OuterText.TrimEnd
                    dr.Item("Fan2") = wb.Document.All(x).Children(2).Children(0).Children(0).Children(3).Children(3).OuterText.TrimEnd
                    dr.Item("Temp2") = wb.Document.All(x).Children(2).Children(0).Children(0).Children(3).Children(4).OuterText.TrimEnd
                    dr.Item("Status2") = wb.Document.All(x).Children(2).Children(0).Children(0).Children(3).Children(5).OuterText.TrimEnd
                End If

                If (dr.Item("Status1").ToString.Contains("x") OrElse dr.Item("Status2").ToString.Contains("x")) AndAlso Me.chkRebootIfXd.Checked = True Then
                    AddToLog("REBOOTING " & dr.Item("Name"))

                    wb.Navigate("http://192.168.0." & dr.Item("Name") & "/cgi-bin/luci/;stok=/admin/system/reboot?reboot=1")
                Else
                    If Me.TimerRefresh.Enabled = False Then
                        Call RefreshGrid()
                    End If
                End If
            End If

            If bAntFound = False Then
                ds.Tables(0).Rows.Add(dr)
            End If

            Me.dataAnts.Refresh()
        End If

    End Sub

    Private Sub TimerRefresh_Tick(sender As Object, e As System.EventArgs) Handles TimerRefresh.Tick

        iCountDown -= 1

        If iCountDown < 0 Then
            iCountDown = iRefreshRate
        End If

        If iCountDown = 0 Then
            Me.TimerRefresh.Enabled = False
            Me.cmdPause.Enabled = False
            Me.cmbBrowserInstances.Enabled = False

            iWatchDog = 300 '5 minutes
            Me.TimerWatchdog.Enabled = True

            'clear the uptime column to indicate we're refreshing
            For Each dr As DataRow In Me.ds.Tables(0).Rows
                dr.Item("UpTime") = "???"
            Next

            Me.dataAnts.Refresh()

            AddToLog("Initiated refresh")

            Call RefreshGrid()

            iCountDown = iRefreshRate
        End If

        Me.cmdRefresh.Text = "Refreshing in " & iCountDown

    End Sub

    Private Sub cmdRefresh_Click(sender As System.Object, e As System.EventArgs) Handles cmdRefresh.Click

        iCountDown = 1

        Call TimerRefresh_Tick(sender, e)

    End Sub

    Private Sub RefreshGrid()

        If Me.chklstAnts.Items.Count = 0 Then
            MsgBox("Please add some Ant addresses first.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

            Me.TabControl1.SelectTab(1)

            Exit Sub
        End If

        For x As Byte = 0 To bBrowserCount - 1
            If bAnt <> Me.chklstAnts.CheckedItems.Count Then
                If wb(x).IsBusy = False Then
                    AddToLog("Submitting " & Me.chklstAnts.CheckedItems(bAnt) & " on instance " & x)

                    wb(x).Navigate("http://" & Me.chklstAnts.CheckedItems(bAnt) & "/cgi-bin/luci/;stok=/admin/status/minerstatus/", False)

                    bAnt += 1
                End If
            End If
        Next

        'If bAnt <> Me.chklstAnts.CheckedItems.Count Then
        '    If wb(1).IsBusy = False Then
        '        Debug.Print("1: submit " & Me.chklstAnts.CheckedItems(bAnt))

        '        wb(1).Navigate("http://" & Me.chklstAnts.CheckedItems(bAnt) & "/cgi-bin/luci/;stok=/admin/status/minerstatus/", False)

        '        bAnt += 1
        '    End If
        'End If

        'If bAnt <> Me.chklstAnts.CheckedItems.Count Then
        '    If wb(2).IsBusy = False Then
        '        Debug.Print("2: submit " & Me.chklstAnts.CheckedItems(bAnt))

        '        wb(2).Navigate("http://" & Me.chklstAnts.CheckedItems(bAnt) & "/cgi-bin/luci/;stok=/admin/status/minerstatus/", False)

        '        bAnt += 1
        '    End If
        'End If

        If bAnt = Me.chklstAnts.CheckedItems.Count Then
            Me.cmdPause.Enabled = True
            Me.TimerRefresh.Enabled = True
            Me.TimerWatchdog.Enabled = False
            Me.cmbBrowserInstances.Enabled = True
            bAnt = 0

            Me.Text = csVersion & " - Refreshed " & Now.ToString
        End If

    End Sub

    Private Sub dataGrid_ColumnWidthChanged(sender As Object, e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dataAnts.ColumnWidthChanged

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

    Private Sub frmMain_ResizeEnd(sender As Object, e As System.EventArgs) Handles Me.ResizeEnd

        With My.Computer.Registry
            .CurrentUser.CreateSubKey(csRegKey)
            .SetValue("HKEY_CURRENT_USER\" & csRegKey, "Width", Me.Width, Microsoft.Win32.RegistryValueKind.DWord)
            .SetValue("HKEY_CURRENT_USER\" & csRegKey, "Height", Me.Height, Microsoft.Win32.RegistryValueKind.DWord)
        End With

    End Sub

    Private Sub cmdScan_Click(sender As System.Object, e As System.EventArgs) Handles cmdScan.Click

        Dim sResponse, sLocalNet As String
        Dim x As Integer
        Dim wc As eWebClient

        Try
            If Me.cmbLocalIPs.Text.IsNullOrEmpty = True Then
                MsgBox("Please select your local IP address first.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

                Me.cmbLocalIPs.DroppedDown = True

                Exit Sub
            End If

            sLocalNet = Me.cmbLocalIPs.Text.Substring(0, InStrRev(Me.cmbLocalIPs.Text, "."))

            wc = New eWebClient

            Me.cmdScan.Enabled = False

            Me.ProgressBar1.Minimum = 1
            Me.ProgressBar1.Maximum = 255
            Me.ProgressBar1.Visible = True
            Me.lblScanning.Visible = True
            My.Application.DoEvents()

            For x = 1 To 255
                Me.ProgressBar1.Value = x
                Me.ToolTip1.SetToolTip(Me.ProgressBar1, sLocalNet & x.ToString)

                If sLocalNet & x.ToString <> Me.cmbLocalIPs.Text Then
                    Try
                        Debug.Print(x)

                        sResponse = wc.DownloadString("http://" & sLocalNet & x.ToString)

                        If sResponse.Contains("href=""/cgi-bin/luci"">LuCI - Lua Configuration Interface</a>") = True Then
                            wc.DownloadFile("http://" & sLocalNet & x.ToString & "/luci-static/resources/icons/antminer_logo.png", My.Computer.FileSystem.SpecialDirectories.Temp & "\ant.png")

                            My.Computer.FileSystem.DeleteFile(My.Computer.FileSystem.SpecialDirectories.Temp & "\ant.png")

                            Me.chklstAnts.SetItemChecked(Me.chklstAnts.Items.Add(sLocalNet & x.ToString), True)

                            My.Application.DoEvents()
                        End If

                        Debug.Print(sResponse)
                    Catch ex As Exception
                    End Try
                End If
            Next
        Catch ex As Exception When bErrorHandle = True
            MsgBox("The following error has occurred:" & vbCrLf & vbCrLf & ex.Message, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly)
        Finally
            Me.ToolTip1.SetToolTip(Me.ProgressBar1, "")
            Me.ProgressBar1.Visible = False
            Me.cmdScan.Enabled = True
            Me.lblScanning.Visible = False
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

        Call ctlsByKey.SetRegKeyByControl(Me.chkRebootIfXd)
        Call ctlsByKey.SetRegKeyByControl(Me.chkSavePassword)

        If Me.chkSavePassword.Checked = True Then
            Call ctlsByKey.SetRegKeyByControl(Me.txtPassword)
        Else
            Call ctlsByKey.SetRegKeyByControl(Me.txtPassword, "")
        End If

        Call ctlsByKey.SetRegKeyByControl(Me.txtUserName)
        Call ctlsByKey.SetRegKeyByControl(Me.chklstAnts)

        Call ctlsByKey.SetRegKeyByControl(Me.txtRefreshRate)
        Call ctlsByKey.SetRegKeyByControl(Me.cmbRefreshRate)

        Call ctlsByKey.SetRegKeyByControl(Me.cmbBrowserInstances)

        Call ctlsByKey.SetRegKeyByControl(Me.cmbFontSize)

    End Sub

    'will re-enable the normal countdown if it counts down to 0 
    'that should only happen if there are so many ants they can't be refreshed in 5 minutes
    'or something went wrong, like it's trying to reach an ant that is offline
    Private Sub TimerWatchdog_Tick(sender As Object, e As System.EventArgs) Handles TimerWatchdog.Tick

        iWatchDog -= 1

        If iWatchDog = 0 Then
            Me.TimerWatchdog.Enabled = False
            Me.TimerRefresh.Enabled = True
            Me.cmdPause.Enabled = True
        End If

    End Sub

    Private Sub cmdAddAnt_Click(sender As Object, e As System.EventArgs) Handles cmdAddAnt.Click

        If Me.txtAntAddress.Text.IsNullOrEmpty = False Then
            If Me.chklstAnts.Items.Contains(Me.txtAntAddress.Text) = False Then
                Me.chklstAnts.SetItemChecked(Me.chklstAnts.Items.Add(Me.txtAntAddress.Text), True)
                Me.txtAntAddress.Text = ""
            Else
                MsgBox("This address appears to already be in the list.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)
            End If
        End If

    End Sub

    Private Sub cmdDelAnt_Click(sender As System.Object, e As System.EventArgs) Handles cmdDelAnt.Click

        If Me.chklstAnts.SelectedItem Is Nothing Then
            MsgBox("Please select an item to remove first.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly)

            Exit Sub
        End If

        Me.chklstAnts.Items.RemoveAt(Me.chklstAnts.SelectedIndex)

    End Sub

    Private Sub cmbRefreshRate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cmbRefreshRate.KeyPress

        e.Handled = True

    End Sub

    Private Sub txtRefreshRate_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtRefreshRate.KeyPress

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

    Private Sub txtRefreshRate_LostFocus(sender As Object, e As System.EventArgs) Handles txtRefreshRate.LostFocus

        Call CalcRefreshRate()

    End Sub

    Private Sub cmbRefreshRate_LostFocus(sender As Object, e As System.EventArgs) Handles cmbRefreshRate.LostFocus

        Call CalcRefreshRate()

    End Sub

    Private Sub cmbBrowserInstances_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cmbBrowserInstances.KeyPress

        e.Handled = True

    End Sub

    Private Sub cmbBrowserInstances_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbBrowserInstances.SelectedIndexChanged

        Dim x As Byte

        bBrowserCount = Val(Me.cmbBrowserInstances.Text)

        If bBrowserCount = 0 OrElse bBrowserCount > 10 Then
            bBrowserCount = 3
        End If

        Array.Resize(wb, bBrowserCount)

        For x = 0 To bBrowserCount - 1
            If wb(x) Is Nothing Then
                wb(x) = New WebBrowser

                AddHandler wb(x).DocumentCompleted, AddressOf Me.wb_completed
            End If
        Next

    End Sub

    Private Sub AddToLog(ByVal sText As String)

        Me.txtLog.AppendText(Now.ToLocalTime & ": " & sText & vbCrLf)

    End Sub

    Private Sub cmbFontSize_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbFontSize.SelectedIndexChanged

        Dim x As Integer

        x = Val(Me.cmbFontSize.Text)

        If x = 0 Then x = 10

        Me.Font = New Font(Me.Font.Name, x)
        Me.Refresh()
        Me.dataAnts.Refresh()

    End Sub
End Class
