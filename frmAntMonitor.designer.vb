<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAntMonitor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAntMonitor))
        Me.dataAnts = New System.Windows.Forms.DataGridView()
        Me.TimerRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabAnts = New System.Windows.Forms.TabPage()
        Me.tabConfig = New System.Windows.Forms.TabPage()
        Me.chkUseAPI = New System.Windows.Forms.CheckBox()
        Me.optAddS2 = New System.Windows.Forms.RadioButton()
        Me.optAddS1 = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkShowStale = New System.Windows.Forms.CheckBox()
        Me.chkShowRej = New System.Windows.Forms.CheckBox()
        Me.chkShowXCount = New System.Windows.Forms.CheckBox()
        Me.chkShowHighTemp = New System.Windows.Forms.CheckBox()
        Me.chkShowHighFan = New System.Windows.Forms.CheckBox()
        Me.chkShowFreqs = New System.Windows.Forms.CheckBox()
        Me.chkShowStatus = New System.Windows.Forms.CheckBox()
        Me.chkShowTemps = New System.Windows.Forms.CheckBox()
        Me.chkShowFans = New System.Windows.Forms.CheckBox()
        Me.chkShowPools = New System.Windows.Forms.CheckBox()
        Me.chkShowBestShare = New System.Windows.Forms.CheckBox()
        Me.cmdSaveConfig = New System.Windows.Forms.Button()
        Me.chkShowHWE = New System.Windows.Forms.CheckBox()
        Me.chkShowBlocks = New System.Windows.Forms.CheckBox()
        Me.chkShowGHsAvg = New System.Windows.Forms.CheckBox()
        Me.chkShowGHs5s = New System.Windows.Forms.CheckBox()
        Me.chkShowUptime = New System.Windows.Forms.CheckBox()
        Me.cmbRefreshRate = New System.Windows.Forms.ComboBox()
        Me.txtRefreshRate = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.lblScanning = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkWBSavePassword = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbLocalIPs = New System.Windows.Forms.ComboBox()
        Me.chkWBRebootIfXd = New System.Windows.Forms.CheckBox()
        Me.txtWBPassword = New System.Windows.Forms.TextBox()
        Me.lblWBPassword = New System.Windows.Forms.Label()
        Me.txtWBUserName = New System.Windows.Forms.TextBox()
        Me.lblWBUserName = New System.Windows.Forms.Label()
        Me.cmdDelAnt = New System.Windows.Forms.Button()
        Me.cmdAddAnt = New System.Windows.Forms.Button()
        Me.txtAntAddress = New System.Windows.Forms.TextBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.cmdScan = New System.Windows.Forms.Button()
        Me.chklstAnts = New System.Windows.Forms.CheckedListBox()
        Me.tabLog = New System.Windows.Forms.TabPage()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.tabAlerts = New System.Windows.Forms.TabPage()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.chkAlertIfS2XCount = New System.Windows.Forms.CheckBox()
        Me.txtAlertS2XCount = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS2Hash = New System.Windows.Forms.CheckBox()
        Me.txtAlertS2Hash = New System.Windows.Forms.TextBox()
        Me.txtAlertS2Temp = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS2Fan = New System.Windows.Forms.CheckBox()
        Me.chkAlertIfS2Temp = New System.Windows.Forms.CheckBox()
        Me.txtAlertS2Fan = New System.Windows.Forms.TextBox()
        Me.cmdSaveAlerts = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.chkAlertStartProcess = New System.Windows.Forms.CheckBox()
        Me.cmdAlertProcessFileFinder = New System.Windows.Forms.Button()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.txtAlertStartProcessName = New System.Windows.Forms.TextBox()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.txtAlertStartProcessParms = New System.Windows.Forms.TextBox()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.chkAlertHighlightField = New System.Windows.Forms.CheckBox()
        Me.chkAlertShowNotifyPopup = New System.Windows.Forms.CheckBox()
        Me.chkAlertShowAnnoyingPopup = New System.Windows.Forms.CheckBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.chkAlertIfS1XCount = New System.Windows.Forms.CheckBox()
        Me.txtAlertS1XCount = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS1Hash = New System.Windows.Forms.CheckBox()
        Me.txtAlertS1Hash = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS1Fan = New System.Windows.Forms.CheckBox()
        Me.txtAlertS1Fan = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS1Temp = New System.Windows.Forms.CheckBox()
        Me.txtAlertS1Temp = New System.Windows.Forms.TextBox()
        Me.cmdPause = New System.Windows.Forms.Button()
        Me.TimerWatchdog = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtPleaseSupport = New System.Windows.Forms.TextBox()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.menuStripNotifyIcon = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuShow = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuStripMain = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuMainExit = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.dataAnts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.tabAnts.SuspendLayout()
        Me.tabConfig.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.tabLog.SuspendLayout()
        Me.tabAlerts.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.menuStripNotifyIcon.SuspendLayout()
        Me.menuStripMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'dataAnts
        '
        Me.dataAnts.AllowUserToAddRows = False
        Me.dataAnts.AllowUserToDeleteRows = False
        Me.dataAnts.AllowUserToOrderColumns = True
        Me.dataAnts.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dataAnts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataAnts.Location = New System.Drawing.Point(7, 7)
        Me.dataAnts.Margin = New System.Windows.Forms.Padding(4)
        Me.dataAnts.Name = "dataAnts"
        Me.dataAnts.ReadOnly = True
        Me.dataAnts.RowHeadersVisible = False
        Me.dataAnts.RowTemplate.Height = 24
        Me.dataAnts.Size = New System.Drawing.Size(1007, 284)
        Me.dataAnts.TabIndex = 0
        '
        'TimerRefresh
        '
        Me.TimerRefresh.Interval = 1000
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.Location = New System.Drawing.Point(514, 340)
        Me.cmdRefresh.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(164, 29)
        Me.cmdRefresh.TabIndex = 1
        Me.cmdRefresh.Text = "Refresh"
        Me.cmdRefresh.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.tabAnts)
        Me.TabControl1.Controls.Add(Me.tabConfig)
        Me.TabControl1.Controls.Add(Me.tabAlerts)
        Me.TabControl1.Controls.Add(Me.tabLog)
        Me.TabControl1.Location = New System.Drawing.Point(2, 1)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1029, 331)
        Me.TabControl1.TabIndex = 2
        '
        'tabAnts
        '
        Me.tabAnts.Controls.Add(Me.dataAnts)
        Me.tabAnts.Location = New System.Drawing.Point(4, 4)
        Me.tabAnts.Name = "tabAnts"
        Me.tabAnts.Padding = New System.Windows.Forms.Padding(3)
        Me.tabAnts.Size = New System.Drawing.Size(1021, 298)
        Me.tabAnts.TabIndex = 0
        Me.tabAnts.Text = "Ants"
        Me.tabAnts.UseVisualStyleBackColor = True
        '
        'tabConfig
        '
        Me.tabConfig.Controls.Add(Me.chkUseAPI)
        Me.tabConfig.Controls.Add(Me.optAddS2)
        Me.tabConfig.Controls.Add(Me.optAddS1)
        Me.tabConfig.Controls.Add(Me.GroupBox1)
        Me.tabConfig.Controls.Add(Me.cmbRefreshRate)
        Me.tabConfig.Controls.Add(Me.txtRefreshRate)
        Me.tabConfig.Controls.Add(Me.Label16)
        Me.tabConfig.Controls.Add(Me.lblScanning)
        Me.tabConfig.Controls.Add(Me.Label5)
        Me.tabConfig.Controls.Add(Me.Label4)
        Me.tabConfig.Controls.Add(Me.chkWBSavePassword)
        Me.tabConfig.Controls.Add(Me.Label3)
        Me.tabConfig.Controls.Add(Me.cmbLocalIPs)
        Me.tabConfig.Controls.Add(Me.chkWBRebootIfXd)
        Me.tabConfig.Controls.Add(Me.txtWBPassword)
        Me.tabConfig.Controls.Add(Me.lblWBPassword)
        Me.tabConfig.Controls.Add(Me.txtWBUserName)
        Me.tabConfig.Controls.Add(Me.lblWBUserName)
        Me.tabConfig.Controls.Add(Me.cmdDelAnt)
        Me.tabConfig.Controls.Add(Me.cmdAddAnt)
        Me.tabConfig.Controls.Add(Me.txtAntAddress)
        Me.tabConfig.Controls.Add(Me.ProgressBar1)
        Me.tabConfig.Controls.Add(Me.cmdScan)
        Me.tabConfig.Controls.Add(Me.chklstAnts)
        Me.tabConfig.Location = New System.Drawing.Point(4, 4)
        Me.tabConfig.Name = "tabConfig"
        Me.tabConfig.Padding = New System.Windows.Forms.Padding(3)
        Me.tabConfig.Size = New System.Drawing.Size(1021, 298)
        Me.tabConfig.TabIndex = 1
        Me.tabConfig.Text = "Config"
        Me.tabConfig.UseVisualStyleBackColor = True
        '
        'chkUseAPI
        '
        Me.chkUseAPI.AutoSize = True
        Me.chkUseAPI.Checked = True
        Me.chkUseAPI.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUseAPI.Location = New System.Drawing.Point(347, 225)
        Me.chkUseAPI.Name = "chkUseAPI"
        Me.chkUseAPI.Size = New System.Drawing.Size(92, 24)
        Me.chkUseAPI.TabIndex = 53
        Me.chkUseAPI.Text = "Use API"
        Me.ToolTip1.SetToolTip(Me.chkUseAPI, "If unchecked, will use a browser control, that has mixed results for some users.")
        Me.chkUseAPI.UseVisualStyleBackColor = True
        '
        'optAddS2
        '
        Me.optAddS2.AutoSize = True
        Me.optAddS2.Location = New System.Drawing.Point(401, 158)
        Me.optAddS2.Name = "optAddS2"
        Me.optAddS2.Size = New System.Drawing.Size(50, 24)
        Me.optAddS2.TabIndex = 50
        Me.optAddS2.Text = "S2"
        Me.optAddS2.UseVisualStyleBackColor = True
        '
        'optAddS1
        '
        Me.optAddS1.AutoSize = True
        Me.optAddS1.Location = New System.Drawing.Point(345, 158)
        Me.optAddS1.Name = "optAddS1"
        Me.optAddS1.Size = New System.Drawing.Size(50, 24)
        Me.optAddS1.TabIndex = 49
        Me.optAddS1.Text = "S1"
        Me.optAddS1.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkShowStale)
        Me.GroupBox1.Controls.Add(Me.chkShowRej)
        Me.GroupBox1.Controls.Add(Me.chkShowXCount)
        Me.GroupBox1.Controls.Add(Me.chkShowHighTemp)
        Me.GroupBox1.Controls.Add(Me.chkShowHighFan)
        Me.GroupBox1.Controls.Add(Me.chkShowFreqs)
        Me.GroupBox1.Controls.Add(Me.chkShowStatus)
        Me.GroupBox1.Controls.Add(Me.chkShowTemps)
        Me.GroupBox1.Controls.Add(Me.chkShowFans)
        Me.GroupBox1.Controls.Add(Me.chkShowPools)
        Me.GroupBox1.Controls.Add(Me.chkShowBestShare)
        Me.GroupBox1.Controls.Add(Me.cmdSaveConfig)
        Me.GroupBox1.Controls.Add(Me.chkShowHWE)
        Me.GroupBox1.Controls.Add(Me.chkShowBlocks)
        Me.GroupBox1.Controls.Add(Me.chkShowGHsAvg)
        Me.GroupBox1.Controls.Add(Me.chkShowGHs5s)
        Me.GroupBox1.Controls.Add(Me.chkShowUptime)
        Me.GroupBox1.Location = New System.Drawing.Point(556, 75)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(451, 145)
        Me.GroupBox1.TabIndex = 48
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Output options"
        '
        'chkShowStale
        '
        Me.chkShowStale.AutoSize = True
        Me.chkShowStale.Checked = True
        Me.chkShowStale.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowStale.Location = New System.Drawing.Point(109, 89)
        Me.chkShowStale.Name = "chkShowStale"
        Me.chkShowStale.Size = New System.Drawing.Size(89, 24)
        Me.chkShowStale.TabIndex = 30
        Me.chkShowStale.Text = "Stale %"
        Me.chkShowStale.UseVisualStyleBackColor = True
        '
        'chkShowRej
        '
        Me.chkShowRej.AutoSize = True
        Me.chkShowRej.Checked = True
        Me.chkShowRej.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowRej.Location = New System.Drawing.Point(109, 68)
        Me.chkShowRej.Name = "chkShowRej"
        Me.chkShowRej.Size = New System.Drawing.Size(99, 24)
        Me.chkShowRej.TabIndex = 29
        Me.chkShowRej.Text = "Reject %"
        Me.chkShowRej.UseVisualStyleBackColor = True
        '
        'chkShowXCount
        '
        Me.chkShowXCount.AutoSize = True
        Me.chkShowXCount.Checked = True
        Me.chkShowXCount.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowXCount.Location = New System.Drawing.Point(347, 26)
        Me.chkShowXCount.Name = "chkShowXCount"
        Me.chkShowXCount.Size = New System.Drawing.Size(86, 24)
        Me.chkShowXCount.TabIndex = 28
        Me.chkShowXCount.Text = "XCount"
        Me.chkShowXCount.UseVisualStyleBackColor = True
        '
        'chkShowHighTemp
        '
        Me.chkShowHighTemp.AutoSize = True
        Me.chkShowHighTemp.Checked = True
        Me.chkShowHighTemp.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowHighTemp.Location = New System.Drawing.Point(217, 46)
        Me.chkShowHighTemp.Name = "chkShowHighTemp"
        Me.chkShowHighTemp.Size = New System.Drawing.Size(113, 24)
        Me.chkShowHighTemp.TabIndex = 27
        Me.chkShowHighTemp.Text = "High Temp"
        Me.chkShowHighTemp.UseVisualStyleBackColor = True
        '
        'chkShowHighFan
        '
        Me.chkShowHighFan.AutoSize = True
        Me.chkShowHighFan.Checked = True
        Me.chkShowHighFan.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowHighFan.Location = New System.Drawing.Point(109, 109)
        Me.chkShowHighFan.Name = "chkShowHighFan"
        Me.chkShowHighFan.Size = New System.Drawing.Size(99, 24)
        Me.chkShowHighFan.TabIndex = 26
        Me.chkShowHighFan.Text = "High Fan"
        Me.chkShowHighFan.UseVisualStyleBackColor = True
        '
        'chkShowFreqs
        '
        Me.chkShowFreqs.AutoSize = True
        Me.chkShowFreqs.Checked = True
        Me.chkShowFreqs.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowFreqs.Location = New System.Drawing.Point(217, 89)
        Me.chkShowFreqs.Name = "chkShowFreqs"
        Me.chkShowFreqs.Size = New System.Drawing.Size(74, 24)
        Me.chkShowFreqs.TabIndex = 25
        Me.chkShowFreqs.Text = "Freqs"
        Me.chkShowFreqs.UseVisualStyleBackColor = True
        '
        'chkShowStatus
        '
        Me.chkShowStatus.AutoSize = True
        Me.chkShowStatus.Checked = True
        Me.chkShowStatus.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowStatus.Location = New System.Drawing.Point(217, 109)
        Me.chkShowStatus.Name = "chkShowStatus"
        Me.chkShowStatus.Size = New System.Drawing.Size(79, 24)
        Me.chkShowStatus.TabIndex = 24
        Me.chkShowStatus.Text = "Status"
        Me.chkShowStatus.UseVisualStyleBackColor = True
        '
        'chkShowTemps
        '
        Me.chkShowTemps.AutoSize = True
        Me.chkShowTemps.Checked = True
        Me.chkShowTemps.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowTemps.Location = New System.Drawing.Point(218, 68)
        Me.chkShowTemps.Name = "chkShowTemps"
        Me.chkShowTemps.Size = New System.Drawing.Size(82, 24)
        Me.chkShowTemps.TabIndex = 23
        Me.chkShowTemps.Text = "Temps"
        Me.chkShowTemps.UseVisualStyleBackColor = True
        '
        'chkShowFans
        '
        Me.chkShowFans.AutoSize = True
        Me.chkShowFans.Checked = True
        Me.chkShowFans.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowFans.Location = New System.Drawing.Point(217, 26)
        Me.chkShowFans.Name = "chkShowFans"
        Me.chkShowFans.Size = New System.Drawing.Size(68, 24)
        Me.chkShowFans.TabIndex = 22
        Me.chkShowFans.Text = "Fans"
        Me.chkShowFans.UseVisualStyleBackColor = True
        '
        'chkShowPools
        '
        Me.chkShowPools.AutoSize = True
        Me.chkShowPools.Checked = True
        Me.chkShowPools.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowPools.Location = New System.Drawing.Point(109, 47)
        Me.chkShowPools.Name = "chkShowPools"
        Me.chkShowPools.Size = New System.Drawing.Size(73, 24)
        Me.chkShowPools.TabIndex = 21
        Me.chkShowPools.Text = "Pools"
        Me.chkShowPools.UseVisualStyleBackColor = True
        '
        'chkShowBestShare
        '
        Me.chkShowBestShare.AutoSize = True
        Me.chkShowBestShare.Checked = True
        Me.chkShowBestShare.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowBestShare.Location = New System.Drawing.Point(109, 26)
        Me.chkShowBestShare.Name = "chkShowBestShare"
        Me.chkShowBestShare.Size = New System.Drawing.Size(110, 24)
        Me.chkShowBestShare.TabIndex = 20
        Me.chkShowBestShare.Text = "BestShare"
        Me.chkShowBestShare.UseVisualStyleBackColor = True
        '
        'cmdSaveConfig
        '
        Me.cmdSaveConfig.Location = New System.Drawing.Point(324, 101)
        Me.cmdSaveConfig.Name = "cmdSaveConfig"
        Me.cmdSaveConfig.Size = New System.Drawing.Size(121, 32)
        Me.cmdSaveConfig.TabIndex = 15
        Me.cmdSaveConfig.Text = "Save Config"
        Me.cmdSaveConfig.UseVisualStyleBackColor = True
        '
        'chkShowHWE
        '
        Me.chkShowHWE.AutoSize = True
        Me.chkShowHWE.Checked = True
        Me.chkShowHWE.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowHWE.Location = New System.Drawing.Point(9, 109)
        Me.chkShowHWE.Name = "chkShowHWE"
        Me.chkShowHWE.Size = New System.Drawing.Size(86, 24)
        Me.chkShowHWE.TabIndex = 19
        Me.chkShowHWE.Text = "HWE%"
        Me.chkShowHWE.UseVisualStyleBackColor = True
        '
        'chkShowBlocks
        '
        Me.chkShowBlocks.AutoSize = True
        Me.chkShowBlocks.Checked = True
        Me.chkShowBlocks.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowBlocks.Location = New System.Drawing.Point(9, 89)
        Me.chkShowBlocks.Name = "chkShowBlocks"
        Me.chkShowBlocks.Size = New System.Drawing.Size(82, 24)
        Me.chkShowBlocks.TabIndex = 18
        Me.chkShowBlocks.Text = "Blocks"
        Me.chkShowBlocks.UseVisualStyleBackColor = True
        '
        'chkShowGHsAvg
        '
        Me.chkShowGHsAvg.AutoSize = True
        Me.chkShowGHsAvg.Checked = True
        Me.chkShowGHsAvg.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowGHsAvg.Location = New System.Drawing.Point(9, 68)
        Me.chkShowGHsAvg.Name = "chkShowGHsAvg"
        Me.chkShowGHsAvg.Size = New System.Drawing.Size(104, 24)
        Me.chkShowGHsAvg.TabIndex = 17
        Me.chkShowGHsAvg.Text = "GH/s Avg"
        Me.chkShowGHsAvg.UseVisualStyleBackColor = True
        '
        'chkShowGHs5s
        '
        Me.chkShowGHs5s.AutoSize = True
        Me.chkShowGHs5s.Checked = True
        Me.chkShowGHs5s.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowGHs5s.Location = New System.Drawing.Point(9, 47)
        Me.chkShowGHs5s.Name = "chkShowGHs5s"
        Me.chkShowGHs5s.Size = New System.Drawing.Size(94, 24)
        Me.chkShowGHs5s.TabIndex = 16
        Me.chkShowGHs5s.Text = "GH/s 5s"
        Me.chkShowGHs5s.UseVisualStyleBackColor = True
        '
        'chkShowUptime
        '
        Me.chkShowUptime.AutoSize = True
        Me.chkShowUptime.Checked = True
        Me.chkShowUptime.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowUptime.Location = New System.Drawing.Point(9, 26)
        Me.chkShowUptime.Name = "chkShowUptime"
        Me.chkShowUptime.Size = New System.Drawing.Size(84, 24)
        Me.chkShowUptime.TabIndex = 15
        Me.chkShowUptime.Text = "Uptime"
        Me.chkShowUptime.UseVisualStyleBackColor = True
        '
        'cmbRefreshRate
        '
        Me.cmbRefreshRate.FormattingEnabled = True
        Me.cmbRefreshRate.Items.AddRange(New Object() {"Seconds", "Minutes", "Hours"})
        Me.cmbRefreshRate.Location = New System.Drawing.Point(529, 258)
        Me.cmbRefreshRate.Name = "cmbRefreshRate"
        Me.cmbRefreshRate.Size = New System.Drawing.Size(94, 28)
        Me.cmbRefreshRate.TabIndex = 47
        Me.cmbRefreshRate.Text = "Seconds"
        '
        'txtRefreshRate
        '
        Me.txtRefreshRate.Location = New System.Drawing.Point(467, 259)
        Me.txtRefreshRate.Name = "txtRefreshRate"
        Me.txtRefreshRate.Size = New System.Drawing.Size(55, 27)
        Me.txtRefreshRate.TabIndex = 46
        Me.txtRefreshRate.Text = "300"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(346, 261)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(118, 20)
        Me.Label16.TabIndex = 45
        Me.Label16.Text = "Refresh every:"
        '
        'lblScanning
        '
        Me.lblScanning.AutoSize = True
        Me.lblScanning.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScanning.Location = New System.Drawing.Point(448, 14)
        Me.lblScanning.Name = "lblScanning"
        Me.lblScanning.Size = New System.Drawing.Size(411, 20)
        Me.lblScanning.TabIndex = 18
        Me.lblScanning.Text = "This could take a while and may appear unresponsive."
        Me.lblScanning.Visible = False
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(6, 238)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(234, 20)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Selected items can be deleted"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 263)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(229, 20)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Unchecked items are skipped"
        '
        'chkWBSavePassword
        '
        Me.chkWBSavePassword.AutoSize = True
        Me.chkWBSavePassword.Checked = True
        Me.chkWBSavePassword.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWBSavePassword.Location = New System.Drawing.Point(642, 258)
        Me.chkWBSavePassword.Name = "chkWBSavePassword"
        Me.chkWBSavePassword.Size = New System.Drawing.Size(147, 24)
        Me.chkWBSavePassword.TabIndex = 14
        Me.chkWBSavePassword.Text = "Save Password"
        Me.chkWBSavePassword.UseVisualStyleBackColor = True
        Me.chkWBSavePassword.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(341, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(137, 20)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Local IP Address"
        '
        'cmbLocalIPs
        '
        Me.cmbLocalIPs.FormattingEnabled = True
        Me.cmbLocalIPs.Location = New System.Drawing.Point(345, 95)
        Me.cmbLocalIPs.Name = "cmbLocalIPs"
        Me.cmbLocalIPs.Size = New System.Drawing.Size(183, 28)
        Me.cmbLocalIPs.TabIndex = 12
        '
        'chkWBRebootIfXd
        '
        Me.chkWBRebootIfXd.AutoSize = True
        Me.chkWBRebootIfXd.Checked = True
        Me.chkWBRebootIfXd.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkWBRebootIfXd.Location = New System.Drawing.Point(795, 257)
        Me.chkWBRebootIfXd.Name = "chkWBRebootIfXd"
        Me.chkWBRebootIfXd.Size = New System.Drawing.Size(183, 24)
        Me.chkWBRebootIfXd.TabIndex = 11
        Me.chkWBRebootIfXd.Text = "Reboot Ants with Xs"
        Me.chkWBRebootIfXd.UseVisualStyleBackColor = True
        Me.chkWBRebootIfXd.Visible = False
        '
        'txtWBPassword
        '
        Me.txtWBPassword.Location = New System.Drawing.Point(905, 225)
        Me.txtWBPassword.Name = "txtWBPassword"
        Me.txtWBPassword.Size = New System.Drawing.Size(100, 27)
        Me.txtWBPassword.TabIndex = 10
        Me.txtWBPassword.Visible = False
        '
        'lblWBPassword
        '
        Me.lblWBPassword.AutoSize = True
        Me.lblWBPassword.Location = New System.Drawing.Point(815, 228)
        Me.lblWBPassword.Name = "lblWBPassword"
        Me.lblWBPassword.Size = New System.Drawing.Size(88, 20)
        Me.lblWBPassword.TabIndex = 9
        Me.lblWBPassword.Text = "Password:"
        Me.lblWBPassword.Visible = False
        '
        'txtWBUserName
        '
        Me.txtWBUserName.Location = New System.Drawing.Point(709, 225)
        Me.txtWBUserName.Name = "txtWBUserName"
        Me.txtWBUserName.Size = New System.Drawing.Size(100, 27)
        Me.txtWBUserName.TabIndex = 8
        Me.txtWBUserName.Visible = False
        '
        'lblWBUserName
        '
        Me.lblWBUserName.AutoSize = True
        Me.lblWBUserName.Location = New System.Drawing.Point(621, 228)
        Me.lblWBUserName.Name = "lblWBUserName"
        Me.lblWBUserName.Size = New System.Drawing.Size(91, 20)
        Me.lblWBUserName.TabIndex = 7
        Me.lblWBUserName.Text = "Username:"
        Me.lblWBUserName.Visible = False
        '
        'cmdDelAnt
        '
        Me.cmdDelAnt.Location = New System.Drawing.Point(439, 188)
        Me.cmdDelAnt.Name = "cmdDelAnt"
        Me.cmdDelAnt.Size = New System.Drawing.Size(74, 32)
        Me.cmdDelAnt.TabIndex = 2
        Me.cmdDelAnt.Text = "&Delete"
        Me.cmdDelAnt.UseVisualStyleBackColor = True
        '
        'cmdAddAnt
        '
        Me.cmdAddAnt.Location = New System.Drawing.Point(345, 188)
        Me.cmdAddAnt.Name = "cmdAddAnt"
        Me.cmdAddAnt.Size = New System.Drawing.Size(74, 32)
        Me.cmdAddAnt.TabIndex = 1
        Me.cmdAddAnt.Text = "&Add"
        Me.cmdAddAnt.UseVisualStyleBackColor = True
        '
        'txtAntAddress
        '
        Me.txtAntAddress.Location = New System.Drawing.Point(345, 126)
        Me.txtAntAddress.Name = "txtAntAddress"
        Me.txtAntAddress.Size = New System.Drawing.Size(168, 27)
        Me.txtAntAddress.TabIndex = 0
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(345, 46)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(382, 23)
        Me.ProgressBar1.TabIndex = 3
        Me.ProgressBar1.Visible = False
        '
        'cmdScan
        '
        Me.cmdScan.Location = New System.Drawing.Point(345, 10)
        Me.cmdScan.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdScan.Name = "cmdScan"
        Me.cmdScan.Size = New System.Drawing.Size(94, 31)
        Me.cmdScan.TabIndex = 2
        Me.cmdScan.Text = "Scan"
        Me.cmdScan.UseVisualStyleBackColor = True
        '
        'chklstAnts
        '
        Me.chklstAnts.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chklstAnts.FormattingEnabled = True
        Me.chklstAnts.Location = New System.Drawing.Point(6, 8)
        Me.chklstAnts.Name = "chklstAnts"
        Me.chklstAnts.Size = New System.Drawing.Size(333, 224)
        Me.chklstAnts.TabIndex = 0
        '
        'tabLog
        '
        Me.tabLog.Controls.Add(Me.txtLog)
        Me.tabLog.Location = New System.Drawing.Point(4, 4)
        Me.tabLog.Name = "tabLog"
        Me.tabLog.Size = New System.Drawing.Size(1021, 298)
        Me.tabLog.TabIndex = 2
        Me.tabLog.Text = "Log"
        Me.tabLog.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLog.Location = New System.Drawing.Point(6, 7)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(1135, 288)
        Me.txtLog.TabIndex = 0
        '
        'tabAlerts
        '
        Me.tabAlerts.Controls.Add(Me.GroupBox4)
        Me.tabAlerts.Controls.Add(Me.cmdSaveAlerts)
        Me.tabAlerts.Controls.Add(Me.GroupBox3)
        Me.tabAlerts.Controls.Add(Me.GroupBox2)
        Me.tabAlerts.Location = New System.Drawing.Point(4, 4)
        Me.tabAlerts.Name = "tabAlerts"
        Me.tabAlerts.Size = New System.Drawing.Size(1021, 298)
        Me.tabAlerts.TabIndex = 3
        Me.tabAlerts.Text = "Alerts"
        Me.tabAlerts.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.chkAlertIfS2XCount)
        Me.GroupBox4.Controls.Add(Me.txtAlertS2XCount)
        Me.GroupBox4.Controls.Add(Me.chkAlertIfS2Hash)
        Me.GroupBox4.Controls.Add(Me.txtAlertS2Hash)
        Me.GroupBox4.Controls.Add(Me.txtAlertS2Temp)
        Me.GroupBox4.Controls.Add(Me.chkAlertIfS2Fan)
        Me.GroupBox4.Controls.Add(Me.chkAlertIfS2Temp)
        Me.GroupBox4.Controls.Add(Me.txtAlertS2Fan)
        Me.GroupBox4.Location = New System.Drawing.Point(212, 7)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(200, 151)
        Me.GroupBox4.TabIndex = 17
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "S2 Alert On"
        '
        'chkAlertIfS2XCount
        '
        Me.chkAlertIfS2XCount.AutoSize = True
        Me.chkAlertIfS2XCount.Location = New System.Drawing.Point(6, 116)
        Me.chkAlertIfS2XCount.Name = "chkAlertIfS2XCount"
        Me.chkAlertIfS2XCount.Size = New System.Drawing.Size(129, 24)
        Me.chkAlertIfS2XCount.TabIndex = 11
        Me.chkAlertIfS2XCount.Text = "XCount Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS2XCount, "Great than or equal to")
        Me.chkAlertIfS2XCount.UseVisualStyleBackColor = True
        '
        'txtAlertS2XCount
        '
        Me.txtAlertS2XCount.Location = New System.Drawing.Point(135, 112)
        Me.txtAlertS2XCount.Name = "txtAlertS2XCount"
        Me.txtAlertS2XCount.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS2XCount.TabIndex = 12
        Me.txtAlertS2XCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS2Hash
        '
        Me.chkAlertIfS2Hash.AutoSize = True
        Me.chkAlertIfS2Hash.Location = New System.Drawing.Point(6, 54)
        Me.chkAlertIfS2Hash.Name = "chkAlertIfS2Hash"
        Me.chkAlertIfS2Hash.Size = New System.Drawing.Size(114, 24)
        Me.chkAlertIfS2Hash.TabIndex = 9
        Me.chkAlertIfS2Hash.Text = "Hash Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS2Hash, "Avg Hash is equal to or less than")
        Me.chkAlertIfS2Hash.UseVisualStyleBackColor = True
        '
        'txtAlertS2Hash
        '
        Me.txtAlertS2Hash.Location = New System.Drawing.Point(135, 50)
        Me.txtAlertS2Hash.Name = "txtAlertS2Hash"
        Me.txtAlertS2Hash.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS2Hash.TabIndex = 10
        Me.txtAlertS2Hash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtAlertS2Temp
        '
        Me.txtAlertS2Temp.Location = New System.Drawing.Point(135, 81)
        Me.txtAlertS2Temp.Name = "txtAlertS2Temp"
        Me.txtAlertS2Temp.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS2Temp.TabIndex = 3
        Me.txtAlertS2Temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS2Fan
        '
        Me.chkAlertIfS2Fan.AutoSize = True
        Me.chkAlertIfS2Fan.Location = New System.Drawing.Point(6, 23)
        Me.chkAlertIfS2Fan.Name = "chkAlertIfS2Fan"
        Me.chkAlertIfS2Fan.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfS2Fan.TabIndex = 5
        Me.chkAlertIfS2Fan.Text = "Fan Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS2Fan, "Equal to or greater than")
        Me.chkAlertIfS2Fan.UseVisualStyleBackColor = True
        '
        'chkAlertIfS2Temp
        '
        Me.chkAlertIfS2Temp.AutoSize = True
        Me.chkAlertIfS2Temp.Location = New System.Drawing.Point(6, 85)
        Me.chkAlertIfS2Temp.Name = "chkAlertIfS2Temp"
        Me.chkAlertIfS2Temp.Size = New System.Drawing.Size(116, 24)
        Me.chkAlertIfS2Temp.TabIndex = 1
        Me.chkAlertIfS2Temp.Text = "Temp Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS2Temp, "Equal to or greater than")
        Me.chkAlertIfS2Temp.UseVisualStyleBackColor = True
        '
        'txtAlertS2Fan
        '
        Me.txtAlertS2Fan.Location = New System.Drawing.Point(135, 19)
        Me.txtAlertS2Fan.Name = "txtAlertS2Fan"
        Me.txtAlertS2Fan.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS2Fan.TabIndex = 7
        Me.txtAlertS2Fan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdSaveAlerts
        '
        Me.cmdSaveAlerts.Location = New System.Drawing.Point(141, 199)
        Me.cmdSaveAlerts.Name = "cmdSaveAlerts"
        Me.cmdSaveAlerts.Size = New System.Drawing.Size(121, 32)
        Me.cmdSaveAlerts.TabIndex = 16
        Me.cmdSaveAlerts.Text = "Save Config"
        Me.cmdSaveAlerts.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.chkAlertStartProcess)
        Me.GroupBox3.Controls.Add(Me.cmdAlertProcessFileFinder)
        Me.GroupBox3.Controls.Add(Me.Label77)
        Me.GroupBox3.Controls.Add(Me.txtAlertStartProcessName)
        Me.GroupBox3.Controls.Add(Me.Label39)
        Me.GroupBox3.Controls.Add(Me.txtAlertStartProcessParms)
        Me.GroupBox3.Controls.Add(Me.Label40)
        Me.GroupBox3.Controls.Add(Me.chkAlertHighlightField)
        Me.GroupBox3.Controls.Add(Me.chkAlertShowNotifyPopup)
        Me.GroupBox3.Controls.Add(Me.chkAlertShowAnnoyingPopup)
        Me.GroupBox3.Location = New System.Drawing.Point(426, 7)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(465, 237)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Alert Types"
        '
        'chkAlertStartProcess
        '
        Me.chkAlertStartProcess.AutoSize = True
        Me.chkAlertStartProcess.Location = New System.Drawing.Point(6, 116)
        Me.chkAlertStartProcess.Name = "chkAlertStartProcess"
        Me.chkAlertStartProcess.Size = New System.Drawing.Size(132, 24)
        Me.chkAlertStartProcess.TabIndex = 21
        Me.chkAlertStartProcess.Text = "Start process"
        Me.ToolTip1.SetToolTip(Me.chkAlertStartProcess, "Could be any valid file or app, such as a sound byte or program")
        Me.chkAlertStartProcess.UseVisualStyleBackColor = True
        '
        'cmdAlertProcessFileFinder
        '
        Me.cmdAlertProcessFileFinder.Location = New System.Drawing.Point(427, 145)
        Me.cmdAlertProcessFileFinder.Name = "cmdAlertProcessFileFinder"
        Me.cmdAlertProcessFileFinder.Size = New System.Drawing.Size(26, 27)
        Me.cmdAlertProcessFileFinder.TabIndex = 20
        Me.cmdAlertProcessFileFinder.Text = "?"
        Me.cmdAlertProcessFileFinder.UseVisualStyleBackColor = True
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label77.Location = New System.Drawing.Point(85, 204)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(171, 20)
        Me.Label77.TabIndex = 19
        Me.Label77.Text = "Use %A for Ant name"
        '
        'txtAlertStartProcessName
        '
        Me.txtAlertStartProcessName.Location = New System.Drawing.Point(89, 145)
        Me.txtAlertStartProcessName.Name = "txtAlertStartProcessName"
        Me.txtAlertStartProcessName.Size = New System.Drawing.Size(332, 27)
        Me.txtAlertStartProcessName.TabIndex = 15
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(6, 148)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(78, 20)
        Me.Label39.TabIndex = 17
        Me.Label39.Text = "Location:"
        '
        'txtAlertStartProcessParms
        '
        Me.txtAlertStartProcessParms.Location = New System.Drawing.Point(89, 174)
        Me.txtAlertStartProcessParms.Name = "txtAlertStartProcessParms"
        Me.txtAlertStartProcessParms.Size = New System.Drawing.Size(332, 27)
        Me.txtAlertStartProcessParms.TabIndex = 16
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(6, 177)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(63, 20)
        Me.Label40.TabIndex = 18
        Me.Label40.Text = "Parms:"
        '
        'chkAlertHighlightField
        '
        Me.chkAlertHighlightField.AutoSize = True
        Me.chkAlertHighlightField.Checked = True
        Me.chkAlertHighlightField.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAlertHighlightField.Location = New System.Drawing.Point(6, 85)
        Me.chkAlertHighlightField.Name = "chkAlertHighlightField"
        Me.chkAlertHighlightField.Size = New System.Drawing.Size(178, 24)
        Me.chkAlertHighlightField.TabIndex = 6
        Me.chkAlertHighlightField.Text = "Highlight Alert Field"
        Me.chkAlertHighlightField.UseVisualStyleBackColor = True
        '
        'chkAlertShowNotifyPopup
        '
        Me.chkAlertShowNotifyPopup.AutoSize = True
        Me.chkAlertShowNotifyPopup.Checked = True
        Me.chkAlertShowNotifyPopup.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAlertShowNotifyPopup.Location = New System.Drawing.Point(6, 23)
        Me.chkAlertShowNotifyPopup.Name = "chkAlertShowNotifyPopup"
        Me.chkAlertShowNotifyPopup.Size = New System.Drawing.Size(213, 24)
        Me.chkAlertShowNotifyPopup.TabIndex = 4
        Me.chkAlertShowNotifyPopup.Text = "Show Notification Popup"
        Me.chkAlertShowNotifyPopup.UseVisualStyleBackColor = True
        '
        'chkAlertShowAnnoyingPopup
        '
        Me.chkAlertShowAnnoyingPopup.AutoSize = True
        Me.chkAlertShowAnnoyingPopup.Location = New System.Drawing.Point(6, 54)
        Me.chkAlertShowAnnoyingPopup.Name = "chkAlertShowAnnoyingPopup"
        Me.chkAlertShowAnnoyingPopup.Size = New System.Drawing.Size(197, 24)
        Me.chkAlertShowAnnoyingPopup.TabIndex = 5
        Me.chkAlertShowAnnoyingPopup.Text = "Show Annoying Popup"
        Me.chkAlertShowAnnoyingPopup.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkAlertIfS1XCount)
        Me.GroupBox2.Controls.Add(Me.txtAlertS1XCount)
        Me.GroupBox2.Controls.Add(Me.chkAlertIfS1Hash)
        Me.GroupBox2.Controls.Add(Me.txtAlertS1Hash)
        Me.GroupBox2.Controls.Add(Me.chkAlertIfS1Fan)
        Me.GroupBox2.Controls.Add(Me.txtAlertS1Fan)
        Me.GroupBox2.Controls.Add(Me.chkAlertIfS1Temp)
        Me.GroupBox2.Controls.Add(Me.txtAlertS1Temp)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 7)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(200, 151)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "S1 Alert On"
        '
        'chkAlertIfS1XCount
        '
        Me.chkAlertIfS1XCount.AutoSize = True
        Me.chkAlertIfS1XCount.Location = New System.Drawing.Point(6, 116)
        Me.chkAlertIfS1XCount.Name = "chkAlertIfS1XCount"
        Me.chkAlertIfS1XCount.Size = New System.Drawing.Size(129, 24)
        Me.chkAlertIfS1XCount.TabIndex = 9
        Me.chkAlertIfS1XCount.Text = "XCount Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS1XCount, "Great than or equal to")
        Me.chkAlertIfS1XCount.UseVisualStyleBackColor = True
        '
        'txtAlertS1XCount
        '
        Me.txtAlertS1XCount.Location = New System.Drawing.Point(135, 112)
        Me.txtAlertS1XCount.Name = "txtAlertS1XCount"
        Me.txtAlertS1XCount.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS1XCount.TabIndex = 10
        Me.txtAlertS1XCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS1Hash
        '
        Me.chkAlertIfS1Hash.AutoSize = True
        Me.chkAlertIfS1Hash.Location = New System.Drawing.Point(6, 54)
        Me.chkAlertIfS1Hash.Name = "chkAlertIfS1Hash"
        Me.chkAlertIfS1Hash.Size = New System.Drawing.Size(114, 24)
        Me.chkAlertIfS1Hash.TabIndex = 7
        Me.chkAlertIfS1Hash.Text = "Hash Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS1Hash, "Avg Hash is equal to or less than")
        Me.chkAlertIfS1Hash.UseVisualStyleBackColor = True
        '
        'txtAlertS1Hash
        '
        Me.txtAlertS1Hash.Location = New System.Drawing.Point(135, 50)
        Me.txtAlertS1Hash.Name = "txtAlertS1Hash"
        Me.txtAlertS1Hash.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS1Hash.TabIndex = 8
        Me.txtAlertS1Hash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS1Fan
        '
        Me.chkAlertIfS1Fan.AutoSize = True
        Me.chkAlertIfS1Fan.Location = New System.Drawing.Point(6, 23)
        Me.chkAlertIfS1Fan.Name = "chkAlertIfS1Fan"
        Me.chkAlertIfS1Fan.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfS1Fan.TabIndex = 4
        Me.chkAlertIfS1Fan.Text = "Fan Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS1Fan, "Equal to or greater than")
        Me.chkAlertIfS1Fan.UseVisualStyleBackColor = True
        '
        'txtAlertS1Fan
        '
        Me.txtAlertS1Fan.Location = New System.Drawing.Point(135, 19)
        Me.txtAlertS1Fan.Name = "txtAlertS1Fan"
        Me.txtAlertS1Fan.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS1Fan.TabIndex = 6
        Me.txtAlertS1Fan.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS1Temp
        '
        Me.chkAlertIfS1Temp.AutoSize = True
        Me.chkAlertIfS1Temp.Location = New System.Drawing.Point(6, 85)
        Me.chkAlertIfS1Temp.Name = "chkAlertIfS1Temp"
        Me.chkAlertIfS1Temp.Size = New System.Drawing.Size(116, 24)
        Me.chkAlertIfS1Temp.TabIndex = 0
        Me.chkAlertIfS1Temp.Text = "Temp Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS1Temp, "Equal to or greater than")
        Me.chkAlertIfS1Temp.UseVisualStyleBackColor = True
        '
        'txtAlertS1Temp
        '
        Me.txtAlertS1Temp.Location = New System.Drawing.Point(135, 81)
        Me.txtAlertS1Temp.Name = "txtAlertS1Temp"
        Me.txtAlertS1Temp.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS1Temp.TabIndex = 2
        Me.txtAlertS1Temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdPause
        '
        Me.cmdPause.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdPause.Location = New System.Drawing.Point(692, 340)
        Me.cmdPause.Name = "cmdPause"
        Me.cmdPause.Size = New System.Drawing.Size(85, 28)
        Me.cmdPause.TabIndex = 3
        Me.cmdPause.Text = "Pause"
        Me.cmdPause.UseVisualStyleBackColor = True
        '
        'TimerWatchdog
        '
        Me.TimerWatchdog.Interval = 1000
        '
        'txtPleaseSupport
        '
        Me.txtPleaseSupport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtPleaseSupport.Location = New System.Drawing.Point(6, 341)
        Me.txtPleaseSupport.Name = "txtPleaseSupport"
        Me.txtPleaseSupport.ReadOnly = True
        Me.txtPleaseSupport.Size = New System.Drawing.Size(501, 27)
        Me.txtPleaseSupport.TabIndex = 100
        Me.txtPleaseSupport.TabStop = False
        Me.txtPleaseSupport.Text = "Please support this app: 1PA1sji28iztspKxDquwFrNjp5SksjkCHE"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.menuStripNotifyIcon
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'menuStripNotifyIcon
        '
        Me.menuStripNotifyIcon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShow, Me.mnuExit})
        Me.menuStripNotifyIcon.Name = "ContextMenuStrip1"
        Me.menuStripNotifyIcon.Size = New System.Drawing.Size(115, 52)
        '
        'mnuShow
        '
        Me.mnuShow.Name = "mnuShow"
        Me.mnuShow.Size = New System.Drawing.Size(114, 24)
        Me.mnuShow.Text = "Show"
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(114, 24)
        Me.mnuExit.Text = "Exit"
        '
        'menuStripMain
        '
        Me.menuStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMainExit})
        Me.menuStripMain.Name = "menuStripMain"
        Me.menuStripMain.Size = New System.Drawing.Size(103, 28)
        '
        'mnuMainExit
        '
        Me.mnuMainExit.Name = "mnuMainExit"
        Me.mnuMainExit.Size = New System.Drawing.Size(102, 24)
        Me.mnuMainExit.Text = "E&xit"
        '
        'frmAntMonitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1043, 375)
        Me.ContextMenuStrip = Me.menuStripMain
        Me.Controls.Add(Me.txtPleaseSupport)
        Me.Controls.Add(Me.cmdPause)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "frmAntMonitor"
        Me.Text = "M's Ant Monitor v1.9"
        CType(Me.dataAnts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.tabAnts.ResumeLayout(False)
        Me.tabConfig.ResumeLayout(False)
        Me.tabConfig.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tabLog.ResumeLayout(False)
        Me.tabLog.PerformLayout()
        Me.tabAlerts.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.menuStripNotifyIcon.ResumeLayout(False)
        Me.menuStripMain.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dataAnts As System.Windows.Forms.DataGridView
    Friend WithEvents TimerRefresh As System.Windows.Forms.Timer
    Friend WithEvents cmdRefresh As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabAnts As System.Windows.Forms.TabPage
    Friend WithEvents tabConfig As System.Windows.Forms.TabPage
    Friend WithEvents cmdScan As System.Windows.Forms.Button
    Friend WithEvents chklstAnts As System.Windows.Forms.CheckedListBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents cmdDelAnt As System.Windows.Forms.Button
    Friend WithEvents cmdAddAnt As System.Windows.Forms.Button
    Friend WithEvents txtAntAddress As System.Windows.Forms.TextBox
    Friend WithEvents cmdPause As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbLocalIPs As System.Windows.Forms.ComboBox
    Friend WithEvents chkWBRebootIfXd As System.Windows.Forms.CheckBox
    Friend WithEvents txtWBPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblWBPassword As System.Windows.Forms.Label
    Friend WithEvents txtWBUserName As System.Windows.Forms.TextBox
    Friend WithEvents lblWBUserName As System.Windows.Forms.Label
    Friend WithEvents chkWBSavePassword As System.Windows.Forms.CheckBox
    Friend WithEvents cmdSaveConfig As System.Windows.Forms.Button
    Friend WithEvents TimerWatchdog As System.Windows.Forms.Timer
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents lblScanning As System.Windows.Forms.Label
    Friend WithEvents txtPleaseSupport As System.Windows.Forms.TextBox
    Friend WithEvents cmbRefreshRate As System.Windows.Forms.ComboBox
    Friend WithEvents txtRefreshRate As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents tabLog As System.Windows.Forms.TabPage
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkShowStatus As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowTemps As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowFans As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowPools As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowBestShare As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowHWE As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowBlocks As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowGHsAvg As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowGHs5s As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowUptime As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowFreqs As System.Windows.Forms.CheckBox
    Friend WithEvents optAddS2 As System.Windows.Forms.RadioButton
    Friend WithEvents optAddS1 As System.Windows.Forms.RadioButton
    Friend WithEvents chkShowHighTemp As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowHighFan As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowXCount As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowStale As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowRej As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseAPI As System.Windows.Forms.CheckBox
    Friend WithEvents tabAlerts As System.Windows.Forms.TabPage
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents menuStripNotifyIcon As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuShow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents menuStripMain As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuMainExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtAlertS2Temp As System.Windows.Forms.TextBox
    Friend WithEvents txtAlertS1Temp As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfS2Temp As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertIfS1Temp As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents chkAlertHighlightField As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertShowNotifyPopup As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertShowAnnoyingPopup As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cmdSaveAlerts As System.Windows.Forms.Button
    Friend WithEvents chkAlertIfS1Fan As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertIfS2Fan As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS1Fan As System.Windows.Forms.TextBox
    Friend WithEvents txtAlertS2Fan As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents chkAlertIfS2Hash As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS2Hash As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfS1Hash As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS1Hash As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfS2XCount As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS2XCount As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfS1XCount As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS1XCount As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertStartProcess As System.Windows.Forms.CheckBox
    Friend WithEvents cmdAlertProcessFileFinder As System.Windows.Forms.Button
    Friend WithEvents Label77 As System.Windows.Forms.Label
    Friend WithEvents txtAlertStartProcessName As System.Windows.Forms.TextBox
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents txtAlertStartProcessParms As System.Windows.Forms.TextBox
    Friend WithEvents Label40 As System.Windows.Forms.Label

End Class
