<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.mnuAntMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuRebootAnt = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRebootMultiple = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRemoveAnt = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuShutdownS2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUpdatePools = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimerRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabMiners = New System.Windows.Forms.TabPage()
        Me.dataMiners = New MMinerMonitor.dgvWrapper()
        Me.tabMinerConfig = New System.Windows.Forms.TabPage()
        Me.cmbMinerType = New System.Windows.Forms.ComboBox()
        Me.txtIPToGetInfo = New System.Windows.Forms.TextBox()
        Me.cmdGetMinerInfo = New System.Windows.Forms.Button()
        Me.cmbAntScanStop = New System.Windows.Forms.ComboBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.cmbAntScanStart = New System.Windows.Forms.ComboBox()
        Me.cmdAntClear = New System.Windows.Forms.Button()
        Me.lblMinerID = New System.Windows.Forms.Label()
        Me.txtAntSSHPort = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.txtAntWebPort = New System.Windows.Forms.TextBox()
        Me.txtAntAPIPort = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.chkAntRebootViaSSH = New System.Windows.Forms.CheckBox()
        Me.chkAntUseAPI = New System.Windows.Forms.CheckBox()
        Me.chkAntActive = New System.Windows.Forms.CheckBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.dataAntConfig = New System.Windows.Forms.DataGridView()
        Me.txtAntName = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtIPRangeToScan = New System.Windows.Forms.TextBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.cmdScan = New System.Windows.Forms.Button()
        Me.cmdSaveAnt = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAntAddress = New System.Windows.Forms.TextBox()
        Me.txtAntSSHPassword = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmdAddAnt = New System.Windows.Forms.Button()
        Me.lblWBPassword = New System.Windows.Forms.Label()
        Me.txtAntWebPassword = New System.Windows.Forms.TextBox()
        Me.cmdDelAnt = New System.Windows.Forms.Button()
        Me.txtAntSSHUsername = New System.Windows.Forms.TextBox()
        Me.txtAntWebUsername = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblWBUserName = New System.Windows.Forms.Label()
        Me.tabConfig = New System.Windows.Forms.TabPage()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtDisplayRefreshInSecs = New System.Windows.Forms.TextBox()
        Me.cmdChangeThreads = New System.Windows.Forms.Button()
        Me.trackThreadCount = New System.Windows.Forms.TrackBar()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkShowACount = New System.Windows.Forms.CheckBox()
        Me.chkShowDifficulty = New System.Windows.Forms.CheckBox()
        Me.chkShowStale = New System.Windows.Forms.CheckBox()
        Me.chkShowRej = New System.Windows.Forms.CheckBox()
        Me.chkShowXCount = New System.Windows.Forms.CheckBox()
        Me.chkShowSelectionColumn = New System.Windows.Forms.CheckBox()
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
        Me.tabPools = New System.Windows.Forms.TabPage()
        Me.cmdPoolClear3 = New System.Windows.Forms.Button()
        Me.cmdPoolClear2 = New System.Windows.Forms.Button()
        Me.cmdPoolMake3 = New System.Windows.Forms.Button()
        Me.cmdPoolMake2 = New System.Windows.Forms.Button()
        Me.cmdPoolMake1 = New System.Windows.Forms.Button()
        Me.lblPools3 = New System.Windows.Forms.Label()
        Me.lblPools2 = New System.Windows.Forms.Label()
        Me.lblPools1 = New System.Windows.Forms.Label()
        Me.cmdPoolsImportFromAnts = New System.Windows.Forms.Button()
        Me.cmdPoolDelete = New System.Windows.Forms.Button()
        Me.cmdPoolChange = New System.Windows.Forms.Button()
        Me.cmdPoolAdd = New System.Windows.Forms.Button()
        Me.txtPoolPassword = New System.Windows.Forms.TextBox()
        Me.txtPoolUsername = New System.Windows.Forms.TextBox()
        Me.txtPoolURL = New System.Windows.Forms.TextBox()
        Me.txtPoolDesc = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lstPools = New MMinerMonitor.enListbox()
        Me.tabAlerts = New System.Windows.Forms.TabPage()
        Me.tabAlertsAndReboots = New System.Windows.Forms.TabControl()
        Me.tabC1Alerts = New System.Windows.Forms.TabPage()
        Me.chkAlertIfC1FanLow = New System.Windows.Forms.CheckBox()
        Me.txtAlertC1FanLow = New System.Windows.Forms.TextBox()
        Me.cmdSaveAlerts6 = New System.Windows.Forms.Button()
        Me.chkAlertIfC1XCount = New System.Windows.Forms.CheckBox()
        Me.txtAlertC1XCount = New System.Windows.Forms.TextBox()
        Me.chkAlertIfC1Temp = New System.Windows.Forms.CheckBox()
        Me.chkAlertIfC1Hash = New System.Windows.Forms.CheckBox()
        Me.txtAlertC1FanHigh = New System.Windows.Forms.TextBox()
        Me.txtAlertC1Hash = New System.Windows.Forms.TextBox()
        Me.chkAlertIfC1FanHigh = New System.Windows.Forms.CheckBox()
        Me.txtAlertC1Temp = New System.Windows.Forms.TextBox()
        Me.tabS1Alerts = New System.Windows.Forms.TabPage()
        Me.chkAlertIfS1FanLow = New System.Windows.Forms.CheckBox()
        Me.txtAlertS1FanLow = New System.Windows.Forms.TextBox()
        Me.cmdSaveAlerts1 = New System.Windows.Forms.Button()
        Me.chkAlertIfS1XCount = New System.Windows.Forms.CheckBox()
        Me.txtAlertS1XCount = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS1FanHigh = New System.Windows.Forms.CheckBox()
        Me.chkAlertIfS1Hash = New System.Windows.Forms.CheckBox()
        Me.txtAlertS1Temp = New System.Windows.Forms.TextBox()
        Me.txtAlertS1Hash = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS1Temp = New System.Windows.Forms.CheckBox()
        Me.txtAlertS1FanHigh = New System.Windows.Forms.TextBox()
        Me.tabS2Alerts = New System.Windows.Forms.TabPage()
        Me.chkAlertIfS2FanLow = New System.Windows.Forms.CheckBox()
        Me.txtAlertS2FanLow = New System.Windows.Forms.TextBox()
        Me.cmdSaveAlerts2 = New System.Windows.Forms.Button()
        Me.chkAlertIfS2XCount = New System.Windows.Forms.CheckBox()
        Me.txtAlertS2XCount = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS2Temp = New System.Windows.Forms.CheckBox()
        Me.chkAlertIfS2Hash = New System.Windows.Forms.CheckBox()
        Me.txtAlertS2FanHigh = New System.Windows.Forms.TextBox()
        Me.txtAlertS2Hash = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS2FanHigh = New System.Windows.Forms.CheckBox()
        Me.txtAlertS2Temp = New System.Windows.Forms.TextBox()
        Me.tabS3Alerts = New System.Windows.Forms.TabPage()
        Me.chkAlertIfS3FanLow = New System.Windows.Forms.CheckBox()
        Me.txtAlertS3FanLow = New System.Windows.Forms.TextBox()
        Me.cmdSaveAlerts5 = New System.Windows.Forms.Button()
        Me.chkAlertIfS3XCount = New System.Windows.Forms.CheckBox()
        Me.txtAlertS3XCount = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS3FanHigh = New System.Windows.Forms.CheckBox()
        Me.chkAlertIfS3Hash = New System.Windows.Forms.CheckBox()
        Me.txtAlertS3Temp = New System.Windows.Forms.TextBox()
        Me.txtAlertS3Hash = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS3Temp = New System.Windows.Forms.CheckBox()
        Me.txtAlertS3FanHigh = New System.Windows.Forms.TextBox()
        Me.tabS4Alerts = New System.Windows.Forms.TabPage()
        Me.chkAlertIfS4FanLow = New System.Windows.Forms.CheckBox()
        Me.txtAlertS4FanLow = New System.Windows.Forms.TextBox()
        Me.cmdSaveAlerts7 = New System.Windows.Forms.Button()
        Me.chkAlertIfS4XCount = New System.Windows.Forms.CheckBox()
        Me.txtAlertS4XCount = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS4FanHigh = New System.Windows.Forms.CheckBox()
        Me.chkAlertIfS4Hash = New System.Windows.Forms.CheckBox()
        Me.txtAlertS4Temp = New System.Windows.Forms.TextBox()
        Me.txtAlertS4Hash = New System.Windows.Forms.TextBox()
        Me.chkAlertIfS4Temp = New System.Windows.Forms.CheckBox()
        Me.txtAlertS4FanHigh = New System.Windows.Forms.TextBox()
        Me.tabSPAlerts = New System.Windows.Forms.TabPage()
        Me.cmdSaveAlerts8 = New System.Windows.Forms.Button()
        Me.chkAlertIfSPHash = New System.Windows.Forms.CheckBox()
        Me.txtAlertSPTemp = New System.Windows.Forms.TextBox()
        Me.txtAlertSPHash = New System.Windows.Forms.TextBox()
        Me.chkAlertIfSPTemp = New System.Windows.Forms.CheckBox()
        Me.tabAlertTypes = New System.Windows.Forms.TabPage()
        Me.cmbAlertEMailGovernor = New System.Windows.Forms.ComboBox()
        Me.txtAlertEMailGovernor = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkAlertSendEMail = New System.Windows.Forms.CheckBox()
        Me.cmdSaveAlerts3 = New System.Windows.Forms.Button()
        Me.chkAlertStartProcess = New System.Windows.Forms.CheckBox()
        Me.cmdAlertProcessFileFinder = New System.Windows.Forms.Button()
        Me.chkAlertShowNotifyPopup = New System.Windows.Forms.CheckBox()
        Me.Label77 = New System.Windows.Forms.Label()
        Me.chkAlertShowAnnoyingPopup = New System.Windows.Forms.CheckBox()
        Me.txtAlertStartProcessName = New System.Windows.Forms.TextBox()
        Me.chkAlertHighlightField = New System.Windows.Forms.CheckBox()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.txtAlertStartProcessParms = New System.Windows.Forms.TextBox()
        Me.tabReboots = New System.Windows.Forms.TabPage()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.cmdSaveReboots = New System.Windows.Forms.Button()
        Me.cmbRebootAllAntsAtAlso = New System.Windows.Forms.ComboBox()
        Me.txtRebootAllAntsAtAlso = New System.Windows.Forms.TextBox()
        Me.cmbRebootAllAntsAt = New System.Windows.Forms.ComboBox()
        Me.txtRebootAllAntsAt = New System.Windows.Forms.TextBox()
        Me.txtRebootAntsByUptime = New System.Windows.Forms.TextBox()
        Me.cmbRebootAntsByUptime = New System.Windows.Forms.ComboBox()
        Me.chkRebootAllAntsAtAlso = New System.Windows.Forms.CheckBox()
        Me.chkRebootAllAntsAt = New System.Windows.Forms.CheckBox()
        Me.chkRebootAntsByUptime = New System.Windows.Forms.CheckBox()
        Me.chkRebootAntOnError = New System.Windows.Forms.CheckBox()
        Me.txtAlertRebootGovernor = New System.Windows.Forms.TextBox()
        Me.chkAlertRebootIfXd = New System.Windows.Forms.CheckBox()
        Me.chkAlertRebootAntsOnHashAlert = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbAlertRebootGovernor = New System.Windows.Forms.ComboBox()
        Me.tabEmail = New System.Windows.Forms.TabPage()
        Me.cmdSendTestEMail = New System.Windows.Forms.Button()
        Me.txtSMTPAlertAddress = New System.Windows.Forms.TextBox()
        Me.Label92 = New System.Windows.Forms.Label()
        Me.txtSMTPAlertSubject = New System.Windows.Forms.TextBox()
        Me.Label93 = New System.Windows.Forms.Label()
        Me.txtSMTPAlertName = New System.Windows.Forms.TextBox()
        Me.Label96 = New System.Windows.Forms.Label()
        Me.Label94 = New System.Windows.Forms.Label()
        Me.Label95 = New System.Windows.Forms.Label()
        Me.Label85 = New System.Windows.Forms.Label()
        Me.cmdSaveAlerts4 = New System.Windows.Forms.Button()
        Me.txtSMTPServer = New System.Windows.Forms.TextBox()
        Me.txtSMTPPort = New System.Windows.Forms.TextBox()
        Me.Label101 = New System.Windows.Forms.Label()
        Me.Label89 = New System.Windows.Forms.Label()
        Me.Label102 = New System.Windows.Forms.Label()
        Me.txtSMTPUserName = New System.Windows.Forms.TextBox()
        Me.txtSMTPFromName = New System.Windows.Forms.TextBox()
        Me.Label90 = New System.Windows.Forms.Label()
        Me.Label103 = New System.Windows.Forms.Label()
        Me.txtSMTPPassword = New System.Windows.Forms.TextBox()
        Me.txtSMTPFromAddress = New System.Windows.Forms.TextBox()
        Me.Label91 = New System.Windows.Forms.Label()
        Me.chkSMTPSSL = New System.Windows.Forms.CheckBox()
        Me.Label100 = New System.Windows.Forms.Label()
        Me.Label99 = New System.Windows.Forms.Label()
        Me.Label98 = New System.Windows.Forms.Label()
        Me.Label97 = New System.Windows.Forms.Label()
        Me.tabLog = New System.Windows.Forms.TabPage()
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.cmbRefreshRate = New System.Windows.Forms.ComboBox()
        Me.txtRefreshRate = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.cmdPause = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtPleaseSupport = New System.Windows.Forms.TextBox()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.menuStripNotifyIcon = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuShow = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.menuStripMain = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuMainExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.timerDoStuff = New System.Windows.Forms.Timer(Me.components)
        Me.mnuAntMenu.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tabMiners.SuspendLayout()
        CType(Me.dataMiners, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabMinerConfig.SuspendLayout()
        CType(Me.dataAntConfig, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabConfig.SuspendLayout()
        CType(Me.trackThreadCount, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.tabPools.SuspendLayout()
        Me.tabAlerts.SuspendLayout()
        Me.tabAlertsAndReboots.SuspendLayout()
        Me.tabC1Alerts.SuspendLayout()
        Me.tabS1Alerts.SuspendLayout()
        Me.tabS2Alerts.SuspendLayout()
        Me.tabS3Alerts.SuspendLayout()
        Me.tabS4Alerts.SuspendLayout()
        Me.tabSPAlerts.SuspendLayout()
        Me.tabAlertTypes.SuspendLayout()
        Me.tabReboots.SuspendLayout()
        Me.tabEmail.SuspendLayout()
        Me.tabLog.SuspendLayout()
        Me.menuStripNotifyIcon.SuspendLayout()
        Me.menuStripMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'mnuAntMenu
        '
        Me.mnuAntMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRebootAnt, Me.mnuRebootMultiple, Me.mnuRemoveAnt, Me.mnuShutdownS2, Me.mnuUpdatePools})
        Me.mnuAntMenu.Name = "menuAntMenu"
        Me.mnuAntMenu.Size = New System.Drawing.Size(203, 144)
        '
        'mnuRebootAnt
        '
        Me.mnuRebootAnt.Name = "mnuRebootAnt"
        Me.mnuRebootAnt.Size = New System.Drawing.Size(202, 28)
        Me.mnuRebootAnt.Text = "Reboot"
        '
        'mnuRebootMultiple
        '
        Me.mnuRebootMultiple.Name = "mnuRebootMultiple"
        Me.mnuRebootMultiple.Size = New System.Drawing.Size(202, 28)
        Me.mnuRebootMultiple.Text = "Reboot Multiple"
        Me.mnuRebootMultiple.Visible = False
        '
        'mnuRemoveAnt
        '
        Me.mnuRemoveAnt.Name = "mnuRemoveAnt"
        Me.mnuRemoveAnt.Size = New System.Drawing.Size(202, 28)
        Me.mnuRemoveAnt.Text = "Remove"
        Me.mnuRemoveAnt.Visible = False
        '
        'mnuShutdownS2
        '
        Me.mnuShutdownS2.Name = "mnuShutdownS2"
        Me.mnuShutdownS2.Size = New System.Drawing.Size(202, 28)
        Me.mnuShutdownS2.Text = "Shutdown S2"
        Me.mnuShutdownS2.Visible = False
        '
        'mnuUpdatePools
        '
        Me.mnuUpdatePools.Name = "mnuUpdatePools"
        Me.mnuUpdatePools.Size = New System.Drawing.Size(202, 28)
        Me.mnuUpdatePools.Text = "Update Pools"
        Me.mnuUpdatePools.Visible = False
        '
        'TimerRefresh
        '
        Me.TimerRefresh.Interval = 1000
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.Location = New System.Drawing.Point(514, 366)
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
        Me.TabControl1.Controls.Add(Me.tabMiners)
        Me.TabControl1.Controls.Add(Me.tabMinerConfig)
        Me.TabControl1.Controls.Add(Me.tabConfig)
        Me.TabControl1.Controls.Add(Me.tabPools)
        Me.TabControl1.Controls.Add(Me.tabAlerts)
        Me.TabControl1.Controls.Add(Me.tabLog)
        Me.TabControl1.Location = New System.Drawing.Point(2, 1)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1161, 358)
        Me.TabControl1.TabIndex = 0
        '
        'tabMiners
        '
        Me.tabMiners.Controls.Add(Me.dataMiners)
        Me.tabMiners.Location = New System.Drawing.Point(4, 4)
        Me.tabMiners.Name = "tabMiners"
        Me.tabMiners.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMiners.Size = New System.Drawing.Size(1153, 325)
        Me.tabMiners.TabIndex = 0
        Me.tabMiners.Text = "Miner Output"
        Me.tabMiners.UseVisualStyleBackColor = True
        '
        'dataMiners
        '
        Me.dataMiners.AllowUserToAddRows = False
        Me.dataMiners.AllowUserToDeleteRows = False
        Me.dataMiners.AllowUserToOrderColumns = True
        Me.dataMiners.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dataMiners.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataMiners.Location = New System.Drawing.Point(7, 7)
        Me.dataMiners.Margin = New System.Windows.Forms.Padding(4)
        Me.dataMiners.Name = "dataMiners"
        Me.dataMiners.ReadOnly = True
        Me.dataMiners.RowHeadersVisible = False
        Me.dataMiners.RowTemplate.Height = 24
        Me.dataMiners.Size = New System.Drawing.Size(1139, 258)
        Me.dataMiners.TabIndex = 0
        '
        'tabMinerConfig
        '
        Me.tabMinerConfig.Controls.Add(Me.cmbMinerType)
        Me.tabMinerConfig.Controls.Add(Me.txtIPToGetInfo)
        Me.tabMinerConfig.Controls.Add(Me.cmdGetMinerInfo)
        Me.tabMinerConfig.Controls.Add(Me.cmbAntScanStop)
        Me.tabMinerConfig.Controls.Add(Me.Label24)
        Me.tabMinerConfig.Controls.Add(Me.Label22)
        Me.tabMinerConfig.Controls.Add(Me.cmbAntScanStart)
        Me.tabMinerConfig.Controls.Add(Me.cmdAntClear)
        Me.tabMinerConfig.Controls.Add(Me.lblMinerID)
        Me.tabMinerConfig.Controls.Add(Me.txtAntSSHPort)
        Me.tabMinerConfig.Controls.Add(Me.Label23)
        Me.tabMinerConfig.Controls.Add(Me.txtAntWebPort)
        Me.tabMinerConfig.Controls.Add(Me.txtAntAPIPort)
        Me.tabMinerConfig.Controls.Add(Me.Label21)
        Me.tabMinerConfig.Controls.Add(Me.chkAntRebootViaSSH)
        Me.tabMinerConfig.Controls.Add(Me.chkAntUseAPI)
        Me.tabMinerConfig.Controls.Add(Me.chkAntActive)
        Me.tabMinerConfig.Controls.Add(Me.Label20)
        Me.tabMinerConfig.Controls.Add(Me.Label19)
        Me.tabMinerConfig.Controls.Add(Me.dataAntConfig)
        Me.tabMinerConfig.Controls.Add(Me.txtAntName)
        Me.tabMinerConfig.Controls.Add(Me.Label15)
        Me.tabMinerConfig.Controls.Add(Me.Label3)
        Me.tabMinerConfig.Controls.Add(Me.txtIPRangeToScan)
        Me.tabMinerConfig.Controls.Add(Me.ProgressBar1)
        Me.tabMinerConfig.Controls.Add(Me.cmdScan)
        Me.tabMinerConfig.Controls.Add(Me.cmdSaveAnt)
        Me.tabMinerConfig.Controls.Add(Me.Label1)
        Me.tabMinerConfig.Controls.Add(Me.txtAntAddress)
        Me.tabMinerConfig.Controls.Add(Me.txtAntSSHPassword)
        Me.tabMinerConfig.Controls.Add(Me.Label7)
        Me.tabMinerConfig.Controls.Add(Me.cmdAddAnt)
        Me.tabMinerConfig.Controls.Add(Me.lblWBPassword)
        Me.tabMinerConfig.Controls.Add(Me.txtAntWebPassword)
        Me.tabMinerConfig.Controls.Add(Me.cmdDelAnt)
        Me.tabMinerConfig.Controls.Add(Me.txtAntSSHUsername)
        Me.tabMinerConfig.Controls.Add(Me.txtAntWebUsername)
        Me.tabMinerConfig.Controls.Add(Me.Label6)
        Me.tabMinerConfig.Controls.Add(Me.lblWBUserName)
        Me.tabMinerConfig.Location = New System.Drawing.Point(4, 4)
        Me.tabMinerConfig.Name = "tabMinerConfig"
        Me.tabMinerConfig.Size = New System.Drawing.Size(1153, 325)
        Me.tabMinerConfig.TabIndex = 5
        Me.tabMinerConfig.Text = "Miner Config"
        Me.tabMinerConfig.UseVisualStyleBackColor = True
        '
        'cmbMinerType
        '
        Me.cmbMinerType.FormattingEnabled = True
        Me.cmbMinerType.Location = New System.Drawing.Point(524, 43)
        Me.cmbMinerType.Name = "cmbMinerType"
        Me.cmbMinerType.Size = New System.Drawing.Size(155, 28)
        Me.cmbMinerType.TabIndex = 93
        '
        'txtIPToGetInfo
        '
        Me.txtIPToGetInfo.Location = New System.Drawing.Point(961, 271)
        Me.txtIPToGetInfo.Name = "txtIPToGetInfo"
        Me.txtIPToGetInfo.Size = New System.Drawing.Size(135, 27)
        Me.txtIPToGetInfo.TabIndex = 92
        '
        'cmdGetMinerInfo
        '
        Me.cmdGetMinerInfo.Location = New System.Drawing.Point(810, 271)
        Me.cmdGetMinerInfo.Name = "cmdGetMinerInfo"
        Me.cmdGetMinerInfo.Size = New System.Drawing.Size(135, 29)
        Me.cmdGetMinerInfo.TabIndex = 91
        Me.cmdGetMinerInfo.Text = "Get Miner Info"
        Me.ToolTip1.SetToolTip(Me.cmdGetMinerInfo, "Click this to get debug info for an unrecognized miner to send to M.")
        Me.cmdGetMinerInfo.UseVisualStyleBackColor = True
        '
        'cmbAntScanStop
        '
        Me.cmbAntScanStop.FormattingEnabled = True
        Me.cmbAntScanStop.Location = New System.Drawing.Point(1071, 235)
        Me.cmbAntScanStop.Name = "cmbAntScanStop"
        Me.cmbAntScanStop.Size = New System.Drawing.Size(52, 28)
        Me.cmbAntScanStop.TabIndex = 11
        Me.cmbAntScanStop.Text = "254"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(1020, 238)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(48, 20)
        Me.Label24.TabIndex = 88
        Me.Label24.Text = "Stop:"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(911, 238)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(50, 20)
        Me.Label22.TabIndex = 87
        Me.Label22.Text = "Start:"
        '
        'cmbAntScanStart
        '
        Me.cmbAntScanStart.FormattingEnabled = True
        Me.cmbAntScanStart.Location = New System.Drawing.Point(961, 235)
        Me.cmbAntScanStart.Name = "cmbAntScanStart"
        Me.cmbAntScanStart.Size = New System.Drawing.Size(52, 28)
        Me.cmbAntScanStart.TabIndex = 10
        Me.cmbAntScanStart.Text = "1"
        '
        'cmdAntClear
        '
        Me.cmdAntClear.Location = New System.Drawing.Point(707, 173)
        Me.cmdAntClear.Name = "cmdAntClear"
        Me.cmdAntClear.Size = New System.Drawing.Size(74, 32)
        Me.cmdAntClear.TabIndex = 85
        Me.cmdAntClear.Text = "&Clear"
        Me.cmdAntClear.UseVisualStyleBackColor = True
        '
        'lblMinerID
        '
        Me.lblMinerID.AutoSize = True
        Me.lblMinerID.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinerID.Location = New System.Drawing.Point(591, 273)
        Me.lblMinerID.Name = "lblMinerID"
        Me.lblMinerID.Size = New System.Drawing.Size(73, 20)
        Me.lblMinerID.TabIndex = 84
        Me.lblMinerID.Text = "Miner ID"
        '
        'txtAntSSHPort
        '
        Me.txtAntSSHPort.Location = New System.Drawing.Point(488, 192)
        Me.txtAntSSHPort.Name = "txtAntSSHPort"
        Me.txtAntSSHPort.Size = New System.Drawing.Size(60, 27)
        Me.txtAntSSHPort.TabIndex = 4
        Me.txtAntSSHPort.Text = "22"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(424, 194)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(45, 20)
        Me.Label23.TabIndex = 82
        Me.Label23.Text = "Port:"
        '
        'txtAntWebPort
        '
        Me.txtAntWebPort.Location = New System.Drawing.Point(594, 192)
        Me.txtAntWebPort.Name = "txtAntWebPort"
        Me.txtAntWebPort.Size = New System.Drawing.Size(60, 27)
        Me.txtAntWebPort.TabIndex = 7
        Me.txtAntWebPort.Text = "80"
        '
        'txtAntAPIPort
        '
        Me.txtAntAPIPort.Location = New System.Drawing.Point(594, 222)
        Me.txtAntAPIPort.Name = "txtAntAPIPort"
        Me.txtAntAPIPort.Size = New System.Drawing.Size(60, 27)
        Me.txtAntAPIPort.TabIndex = 8
        Me.txtAntAPIPort.Text = "4028"
        '
        'Label21
        '
        Me.Label21.Location = New System.Drawing.Point(541, 226)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(47, 27)
        Me.Label21.TabIndex = 78
        Me.Label21.Text = "Port:"
        '
        'chkAntRebootViaSSH
        '
        Me.chkAntRebootViaSSH.AutoSize = True
        Me.chkAntRebootViaSSH.Checked = True
        Me.chkAntRebootViaSSH.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAntRebootViaSSH.Location = New System.Drawing.Point(428, 249)
        Me.chkAntRebootViaSSH.Name = "chkAntRebootViaSSH"
        Me.chkAntRebootViaSSH.Size = New System.Drawing.Size(150, 24)
        Me.chkAntRebootViaSSH.TabIndex = 77
        Me.chkAntRebootViaSSH.Text = "Reboot via SSH"
        Me.chkAntRebootViaSSH.UseVisualStyleBackColor = True
        '
        'chkAntUseAPI
        '
        Me.chkAntUseAPI.AutoSize = True
        Me.chkAntUseAPI.Checked = True
        Me.chkAntUseAPI.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAntUseAPI.Location = New System.Drawing.Point(428, 225)
        Me.chkAntUseAPI.Name = "chkAntUseAPI"
        Me.chkAntUseAPI.Size = New System.Drawing.Size(92, 24)
        Me.chkAntUseAPI.TabIndex = 76
        Me.chkAntUseAPI.Text = "Use API"
        Me.chkAntUseAPI.UseVisualStyleBackColor = True
        '
        'chkAntActive
        '
        Me.chkAntActive.AutoSize = True
        Me.chkAntActive.Checked = True
        Me.chkAntActive.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAntActive.Location = New System.Drawing.Point(428, 272)
        Me.chkAntActive.Name = "chkAntActive"
        Me.chkAntActive.Size = New System.Drawing.Size(77, 24)
        Me.chkAntActive.TabIndex = 75
        Me.chkAntActive.Text = "Active"
        Me.chkAntActive.UseVisualStyleBackColor = True
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(424, 82)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(58, 20)
        Me.Label20.TabIndex = 74
        Me.Label20.Text = "Name:"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(424, 44)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(50, 20)
        Me.Label19.TabIndex = 72
        Me.Label19.Text = "Type:"
        '
        'dataAntConfig
        '
        Me.dataAntConfig.AllowUserToAddRows = False
        Me.dataAntConfig.AllowUserToDeleteRows = False
        Me.dataAntConfig.AllowUserToOrderColumns = True
        Me.dataAntConfig.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.dataAntConfig.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dataAntConfig.Location = New System.Drawing.Point(7, 7)
        Me.dataAntConfig.Name = "dataAntConfig"
        Me.dataAntConfig.ReadOnly = True
        Me.dataAntConfig.RowTemplate.Height = 24
        Me.dataAntConfig.Size = New System.Drawing.Size(411, 313)
        Me.dataAntConfig.TabIndex = 71
        '
        'txtAntName
        '
        Me.txtAntName.Location = New System.Drawing.Point(524, 79)
        Me.txtAntName.Name = "txtAntName"
        Me.txtAntName.Size = New System.Drawing.Size(155, 27)
        Me.txtAntName.TabIndex = 1
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(808, 168)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(96, 20)
        Me.Label15.TabIndex = 6
        Me.Label15.Text = "IP Address:"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(806, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(335, 154)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = resources.GetString("Label3.Text")
        '
        'txtIPRangeToScan
        '
        Me.txtIPRangeToScan.Location = New System.Drawing.Point(915, 165)
        Me.txtIPRangeToScan.Name = "txtIPRangeToScan"
        Me.txtIPRangeToScan.Size = New System.Drawing.Size(135, 27)
        Me.txtIPRangeToScan.TabIndex = 9
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(812, 198)
        Me.ProgressBar1.Maximum = 254
        Me.ProgressBar1.Minimum = 1
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(319, 23)
        Me.ProgressBar1.TabIndex = 3
        Me.ProgressBar1.Value = 1
        Me.ProgressBar1.Visible = False
        '
        'cmdScan
        '
        Me.cmdScan.Location = New System.Drawing.Point(811, 233)
        Me.cmdScan.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdScan.Name = "cmdScan"
        Me.cmdScan.Size = New System.Drawing.Size(94, 31)
        Me.cmdScan.TabIndex = 2
        Me.cmdScan.Text = "Scan"
        Me.cmdScan.UseVisualStyleBackColor = True
        '
        'cmdSaveAnt
        '
        Me.cmdSaveAnt.Location = New System.Drawing.Point(707, 63)
        Me.cmdSaveAnt.Name = "cmdSaveAnt"
        Me.cmdSaveAnt.Size = New System.Drawing.Size(74, 32)
        Me.cmdSaveAnt.TabIndex = 63
        Me.cmdSaveAnt.Text = "&Save"
        Me.cmdSaveAnt.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(424, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 20)
        Me.Label1.TabIndex = 54
        Me.Label1.Text = "IP Address:"
        '
        'txtAntAddress
        '
        Me.txtAntAddress.Location = New System.Drawing.Point(524, 10)
        Me.txtAntAddress.Name = "txtAntAddress"
        Me.txtAntAddress.Size = New System.Drawing.Size(155, 27)
        Me.txtAntAddress.TabIndex = 0
        '
        'txtAntSSHPassword
        '
        Me.txtAntSSHPassword.Location = New System.Drawing.Point(488, 163)
        Me.txtAntSSHPassword.Name = "txtAntSSHPassword"
        Me.txtAntSSHPassword.Size = New System.Drawing.Size(100, 27)
        Me.txtAntSSHPassword.TabIndex = 3
        Me.txtAntSSHPassword.Text = "root"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(591, 109)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(80, 20)
        Me.Label7.TabIndex = 62
        Me.Label7.Text = "Web Info:"
        '
        'cmdAddAnt
        '
        Me.cmdAddAnt.Location = New System.Drawing.Point(707, 10)
        Me.cmdAddAnt.Name = "cmdAddAnt"
        Me.cmdAddAnt.Size = New System.Drawing.Size(74, 32)
        Me.cmdAddAnt.TabIndex = 1
        Me.cmdAddAnt.Text = "&Add"
        Me.cmdAddAnt.UseVisualStyleBackColor = True
        '
        'lblWBPassword
        '
        Me.lblWBPassword.AutoSize = True
        Me.lblWBPassword.Location = New System.Drawing.Point(424, 166)
        Me.lblWBPassword.Name = "lblWBPassword"
        Me.lblWBPassword.Size = New System.Drawing.Size(52, 20)
        Me.lblWBPassword.TabIndex = 9
        Me.lblWBPassword.Text = "Pass:"
        '
        'txtAntWebPassword
        '
        Me.txtAntWebPassword.Location = New System.Drawing.Point(594, 163)
        Me.txtAntWebPassword.Name = "txtAntWebPassword"
        Me.txtAntWebPassword.Size = New System.Drawing.Size(100, 27)
        Me.txtAntWebPassword.TabIndex = 6
        Me.txtAntWebPassword.Text = "root"
        '
        'cmdDelAnt
        '
        Me.cmdDelAnt.Location = New System.Drawing.Point(707, 116)
        Me.cmdDelAnt.Name = "cmdDelAnt"
        Me.cmdDelAnt.Size = New System.Drawing.Size(74, 32)
        Me.cmdDelAnt.TabIndex = 2
        Me.cmdDelAnt.Text = "&Delete"
        Me.cmdDelAnt.UseVisualStyleBackColor = True
        '
        'txtAntSSHUsername
        '
        Me.txtAntSSHUsername.Location = New System.Drawing.Point(488, 134)
        Me.txtAntSSHUsername.Name = "txtAntSSHUsername"
        Me.txtAntSSHUsername.Size = New System.Drawing.Size(100, 27)
        Me.txtAntSSHUsername.TabIndex = 2
        Me.txtAntSSHUsername.Text = "root"
        Me.ToolTip1.SetToolTip(Me.txtAntSSHUsername, "If left empty, default is used")
        '
        'txtAntWebUsername
        '
        Me.txtAntWebUsername.Location = New System.Drawing.Point(594, 134)
        Me.txtAntWebUsername.Name = "txtAntWebUsername"
        Me.txtAntWebUsername.Size = New System.Drawing.Size(100, 27)
        Me.txtAntWebUsername.TabIndex = 5
        Me.txtAntWebUsername.Text = "root"
        Me.ToolTip1.SetToolTip(Me.txtAntWebUsername, "If left empty, default is used")
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(424, 109)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 20)
        Me.Label6.TabIndex = 57
        Me.Label6.Text = "SSH Info:"
        '
        'lblWBUserName
        '
        Me.lblWBUserName.AutoSize = True
        Me.lblWBUserName.Location = New System.Drawing.Point(424, 137)
        Me.lblWBUserName.Name = "lblWBUserName"
        Me.lblWBUserName.Size = New System.Drawing.Size(50, 20)
        Me.lblWBUserName.TabIndex = 7
        Me.lblWBUserName.Text = "User:"
        '
        'tabConfig
        '
        Me.tabConfig.Controls.Add(Me.Label18)
        Me.tabConfig.Controls.Add(Me.Label17)
        Me.tabConfig.Controls.Add(Me.Label5)
        Me.tabConfig.Controls.Add(Me.Label4)
        Me.tabConfig.Controls.Add(Me.Label14)
        Me.tabConfig.Controls.Add(Me.txtDisplayRefreshInSecs)
        Me.tabConfig.Controls.Add(Me.cmdChangeThreads)
        Me.tabConfig.Controls.Add(Me.trackThreadCount)
        Me.tabConfig.Controls.Add(Me.Label13)
        Me.tabConfig.Controls.Add(Me.GroupBox1)
        Me.tabConfig.Location = New System.Drawing.Point(4, 4)
        Me.tabConfig.Name = "tabConfig"
        Me.tabConfig.Padding = New System.Windows.Forms.Padding(3)
        Me.tabConfig.Size = New System.Drawing.Size(1153, 325)
        Me.tabConfig.TabIndex = 1
        Me.tabConfig.Text = "Config"
        Me.tabConfig.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(477, 237)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(343, 85)
        Me.Label18.TabIndex = 74
        Me.Label18.Text = "A properly matched Thread Count and Display Refresh Display can result in an extr" & _
    "emely fast refresh.  One user monitors 1200 Ants in 52 seconds!"
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(477, 130)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(343, 103)
        Me.Label17.TabIndex = 73
        Me.Label17.Text = resources.GetString("Label17.Text")
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(477, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(343, 104)
        Me.Label5.TabIndex = 72
        Me.Label5.Text = "The Thread Count is how many background workers run simultaneously to monitor you" & _
    "r Ants.  The higher the number, the faster it'll be, but the more resources (PC " & _
    "and network) that it'll consume."
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(7, 271)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(97, 24)
        Me.Label4.TabIndex = 71
        Me.Label4.Text = "In seconds:"
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(6, 245)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(182, 24)
        Me.Label14.TabIndex = 70
        Me.Label14.Text = "Display Refresh Delay"
        '
        'txtDisplayRefreshInSecs
        '
        Me.txtDisplayRefreshInSecs.Location = New System.Drawing.Point(106, 269)
        Me.txtDisplayRefreshInSecs.Name = "txtDisplayRefreshInSecs"
        Me.txtDisplayRefreshInSecs.Size = New System.Drawing.Size(34, 27)
        Me.txtDisplayRefreshInSecs.TabIndex = 69
        Me.txtDisplayRefreshInSecs.Text = "1"
        Me.txtDisplayRefreshInSecs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip1.SetToolTip(Me.txtDisplayRefreshInSecs, "How much time to wait in seconds before refreshing the display grid after initiat" & _
        "ing a refresh.  This value should probably be low if you have fast video or a lo" & _
        "w number of Ants.")
        '
        'cmdChangeThreads
        '
        Me.cmdChangeThreads.Location = New System.Drawing.Point(187, 261)
        Me.cmdChangeThreads.Name = "cmdChangeThreads"
        Me.cmdChangeThreads.Size = New System.Drawing.Size(153, 32)
        Me.cmdChangeThreads.TabIndex = 68
        Me.cmdChangeThreads.Text = "Change Threads"
        Me.ToolTip1.SetToolTip(Me.cmdChangeThreads, "Will cause the existing threads to be aborted and new ones started.  Probably bes" & _
        "t to only do when refreshing is not active.")
        Me.cmdChangeThreads.UseVisualStyleBackColor = True
        '
        'trackThreadCount
        '
        Me.trackThreadCount.Location = New System.Drawing.Point(187, 199)
        Me.trackThreadCount.Maximum = 64
        Me.trackThreadCount.Minimum = 2
        Me.trackThreadCount.Name = "trackThreadCount"
        Me.trackThreadCount.Size = New System.Drawing.Size(272, 56)
        Me.trackThreadCount.TabIndex = 67
        Me.trackThreadCount.Value = 5
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 201)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(146, 20)
        Me.Label13.TabIndex = 66
        Me.Label13.Text = "API Thread Count:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkShowACount)
        Me.GroupBox1.Controls.Add(Me.chkShowDifficulty)
        Me.GroupBox1.Controls.Add(Me.chkShowStale)
        Me.GroupBox1.Controls.Add(Me.chkShowRej)
        Me.GroupBox1.Controls.Add(Me.chkShowXCount)
        Me.GroupBox1.Controls.Add(Me.chkShowSelectionColumn)
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
        Me.GroupBox1.Location = New System.Drawing.Point(6, 13)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(451, 169)
        Me.GroupBox1.TabIndex = 48
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Output options"
        '
        'chkShowACount
        '
        Me.chkShowACount.AutoSize = True
        Me.chkShowACount.Checked = True
        Me.chkShowACount.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowACount.Location = New System.Drawing.Point(347, 68)
        Me.chkShowACount.Name = "chkShowACount"
        Me.chkShowACount.Size = New System.Drawing.Size(86, 24)
        Me.chkShowACount.TabIndex = 32
        Me.chkShowACount.Text = "ACount"
        Me.chkShowACount.UseVisualStyleBackColor = True
        '
        'chkShowDifficulty
        '
        Me.chkShowDifficulty.AutoSize = True
        Me.chkShowDifficulty.Checked = True
        Me.chkShowDifficulty.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowDifficulty.Location = New System.Drawing.Point(109, 47)
        Me.chkShowDifficulty.Name = "chkShowDifficulty"
        Me.chkShowDifficulty.Size = New System.Drawing.Size(97, 24)
        Me.chkShowDifficulty.TabIndex = 31
        Me.chkShowDifficulty.Text = "Difficulty"
        Me.chkShowDifficulty.UseVisualStyleBackColor = True
        '
        'chkShowStale
        '
        Me.chkShowStale.AutoSize = True
        Me.chkShowStale.Checked = True
        Me.chkShowStale.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowStale.Location = New System.Drawing.Point(109, 109)
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
        Me.chkShowRej.Location = New System.Drawing.Point(109, 89)
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
        Me.chkShowXCount.Location = New System.Drawing.Point(347, 47)
        Me.chkShowXCount.Name = "chkShowXCount"
        Me.chkShowXCount.Size = New System.Drawing.Size(86, 24)
        Me.chkShowXCount.TabIndex = 28
        Me.chkShowXCount.Text = "XCount"
        Me.chkShowXCount.UseVisualStyleBackColor = True
        '
        'chkShowSelectionColumn
        '
        Me.chkShowSelectionColumn.AutoSize = True
        Me.chkShowSelectionColumn.Location = New System.Drawing.Point(9, 141)
        Me.chkShowSelectionColumn.Name = "chkShowSelectionColumn"
        Me.chkShowSelectionColumn.Size = New System.Drawing.Size(208, 24)
        Me.chkShowSelectionColumn.TabIndex = 64
        Me.chkShowSelectionColumn.Text = "Show Selection Column"
        Me.chkShowSelectionColumn.UseVisualStyleBackColor = True
        '
        'chkShowHighTemp
        '
        Me.chkShowHighTemp.AutoSize = True
        Me.chkShowHighTemp.Checked = True
        Me.chkShowHighTemp.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowHighTemp.Location = New System.Drawing.Point(217, 68)
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
        Me.chkShowHighFan.Location = New System.Drawing.Point(217, 26)
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
        Me.chkShowFreqs.Location = New System.Drawing.Point(217, 109)
        Me.chkShowFreqs.Name = "chkShowFreqs"
        Me.chkShowFreqs.Size = New System.Drawing.Size(65, 24)
        Me.chkShowFreqs.TabIndex = 25
        Me.chkShowFreqs.Text = "Freq"
        Me.chkShowFreqs.UseVisualStyleBackColor = True
        '
        'chkShowStatus
        '
        Me.chkShowStatus.AutoSize = True
        Me.chkShowStatus.Checked = True
        Me.chkShowStatus.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowStatus.Location = New System.Drawing.Point(347, 26)
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
        Me.chkShowTemps.Location = New System.Drawing.Point(217, 89)
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
        Me.chkShowFans.Location = New System.Drawing.Point(217, 47)
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
        Me.chkShowPools.Location = New System.Drawing.Point(109, 68)
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
        Me.cmdSaveConfig.Location = New System.Drawing.Point(324, 131)
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
        'tabPools
        '
        Me.tabPools.Controls.Add(Me.cmdPoolClear3)
        Me.tabPools.Controls.Add(Me.cmdPoolClear2)
        Me.tabPools.Controls.Add(Me.cmdPoolMake3)
        Me.tabPools.Controls.Add(Me.cmdPoolMake2)
        Me.tabPools.Controls.Add(Me.cmdPoolMake1)
        Me.tabPools.Controls.Add(Me.lblPools3)
        Me.tabPools.Controls.Add(Me.lblPools2)
        Me.tabPools.Controls.Add(Me.lblPools1)
        Me.tabPools.Controls.Add(Me.cmdPoolsImportFromAnts)
        Me.tabPools.Controls.Add(Me.cmdPoolDelete)
        Me.tabPools.Controls.Add(Me.cmdPoolChange)
        Me.tabPools.Controls.Add(Me.cmdPoolAdd)
        Me.tabPools.Controls.Add(Me.txtPoolPassword)
        Me.tabPools.Controls.Add(Me.txtPoolUsername)
        Me.tabPools.Controls.Add(Me.txtPoolURL)
        Me.tabPools.Controls.Add(Me.txtPoolDesc)
        Me.tabPools.Controls.Add(Me.Label12)
        Me.tabPools.Controls.Add(Me.Label11)
        Me.tabPools.Controls.Add(Me.Label10)
        Me.tabPools.Controls.Add(Me.Label9)
        Me.tabPools.Controls.Add(Me.lstPools)
        Me.tabPools.Location = New System.Drawing.Point(4, 4)
        Me.tabPools.Name = "tabPools"
        Me.tabPools.Size = New System.Drawing.Size(1153, 325)
        Me.tabPools.TabIndex = 4
        Me.tabPools.Text = "Pools"
        Me.tabPools.UseVisualStyleBackColor = True
        '
        'cmdPoolClear3
        '
        Me.cmdPoolClear3.Location = New System.Drawing.Point(618, 242)
        Me.cmdPoolClear3.Name = "cmdPoolClear3"
        Me.cmdPoolClear3.Size = New System.Drawing.Size(129, 32)
        Me.cmdPoolClear3.TabIndex = 21
        Me.cmdPoolClear3.Text = "Clear Pool #3"
        Me.cmdPoolClear3.UseVisualStyleBackColor = True
        '
        'cmdPoolClear2
        '
        Me.cmdPoolClear2.Location = New System.Drawing.Point(447, 242)
        Me.cmdPoolClear2.Name = "cmdPoolClear2"
        Me.cmdPoolClear2.Size = New System.Drawing.Size(129, 32)
        Me.cmdPoolClear2.TabIndex = 20
        Me.cmdPoolClear2.Text = "Clear Pool #2"
        Me.cmdPoolClear2.UseVisualStyleBackColor = True
        '
        'cmdPoolMake3
        '
        Me.cmdPoolMake3.Location = New System.Drawing.Point(618, 194)
        Me.cmdPoolMake3.Name = "cmdPoolMake3"
        Me.cmdPoolMake3.Size = New System.Drawing.Size(129, 32)
        Me.cmdPoolMake3.TabIndex = 19
        Me.cmdPoolMake3.Text = "Make Pool #3"
        Me.cmdPoolMake3.UseVisualStyleBackColor = True
        '
        'cmdPoolMake2
        '
        Me.cmdPoolMake2.Location = New System.Drawing.Point(447, 194)
        Me.cmdPoolMake2.Name = "cmdPoolMake2"
        Me.cmdPoolMake2.Size = New System.Drawing.Size(129, 32)
        Me.cmdPoolMake2.TabIndex = 18
        Me.cmdPoolMake2.Text = "Make Pool #2"
        Me.cmdPoolMake2.UseVisualStyleBackColor = True
        '
        'cmdPoolMake1
        '
        Me.cmdPoolMake1.Location = New System.Drawing.Point(282, 194)
        Me.cmdPoolMake1.Name = "cmdPoolMake1"
        Me.cmdPoolMake1.Size = New System.Drawing.Size(129, 32)
        Me.cmdPoolMake1.TabIndex = 17
        Me.cmdPoolMake1.Text = "Make Pool #1"
        Me.cmdPoolMake1.UseVisualStyleBackColor = True
        '
        'lblPools3
        '
        Me.lblPools3.AutoSize = True
        Me.lblPools3.Location = New System.Drawing.Point(766, 70)
        Me.lblPools3.Name = "lblPools3"
        Me.lblPools3.Size = New System.Drawing.Size(65, 20)
        Me.lblPools3.TabIndex = 16
        Me.lblPools3.Text = "Pool #3"
        '
        'lblPools2
        '
        Me.lblPools2.AutoSize = True
        Me.lblPools2.Location = New System.Drawing.Point(766, 42)
        Me.lblPools2.Name = "lblPools2"
        Me.lblPools2.Size = New System.Drawing.Size(65, 20)
        Me.lblPools2.TabIndex = 15
        Me.lblPools2.Text = "Pool #2"
        '
        'lblPools1
        '
        Me.lblPools1.AutoSize = True
        Me.lblPools1.Location = New System.Drawing.Point(766, 14)
        Me.lblPools1.Name = "lblPools1"
        Me.lblPools1.Size = New System.Drawing.Size(65, 20)
        Me.lblPools1.TabIndex = 14
        Me.lblPools1.Text = "Pool #1"
        '
        'cmdPoolsImportFromAnts
        '
        Me.cmdPoolsImportFromAnts.Location = New System.Drawing.Point(591, 141)
        Me.cmdPoolsImportFromAnts.Name = "cmdPoolsImportFromAnts"
        Me.cmdPoolsImportFromAnts.Size = New System.Drawing.Size(156, 32)
        Me.cmdPoolsImportFromAnts.TabIndex = 13
        Me.cmdPoolsImportFromAnts.Text = "Import From Ants"
        Me.cmdPoolsImportFromAnts.UseVisualStyleBackColor = True
        '
        'cmdPoolDelete
        '
        Me.cmdPoolDelete.Location = New System.Drawing.Point(470, 141)
        Me.cmdPoolDelete.Name = "cmdPoolDelete"
        Me.cmdPoolDelete.Size = New System.Drawing.Size(87, 32)
        Me.cmdPoolDelete.TabIndex = 11
        Me.cmdPoolDelete.Text = "Delete"
        Me.cmdPoolDelete.UseVisualStyleBackColor = True
        '
        'cmdPoolChange
        '
        Me.cmdPoolChange.Location = New System.Drawing.Point(377, 141)
        Me.cmdPoolChange.Name = "cmdPoolChange"
        Me.cmdPoolChange.Size = New System.Drawing.Size(87, 32)
        Me.cmdPoolChange.TabIndex = 10
        Me.cmdPoolChange.Text = "Change"
        Me.cmdPoolChange.UseVisualStyleBackColor = True
        '
        'cmdPoolAdd
        '
        Me.cmdPoolAdd.Location = New System.Drawing.Point(282, 141)
        Me.cmdPoolAdd.Name = "cmdPoolAdd"
        Me.cmdPoolAdd.Size = New System.Drawing.Size(87, 32)
        Me.cmdPoolAdd.TabIndex = 9
        Me.cmdPoolAdd.Text = "Add"
        Me.cmdPoolAdd.UseVisualStyleBackColor = True
        '
        'txtPoolPassword
        '
        Me.txtPoolPassword.Location = New System.Drawing.Point(377, 92)
        Me.txtPoolPassword.Name = "txtPoolPassword"
        Me.txtPoolPassword.Size = New System.Drawing.Size(370, 27)
        Me.txtPoolPassword.TabIndex = 8
        '
        'txtPoolUsername
        '
        Me.txtPoolUsername.Location = New System.Drawing.Point(377, 63)
        Me.txtPoolUsername.Name = "txtPoolUsername"
        Me.txtPoolUsername.Size = New System.Drawing.Size(370, 27)
        Me.txtPoolUsername.TabIndex = 7
        '
        'txtPoolURL
        '
        Me.txtPoolURL.Location = New System.Drawing.Point(377, 35)
        Me.txtPoolURL.Name = "txtPoolURL"
        Me.txtPoolURL.Size = New System.Drawing.Size(370, 27)
        Me.txtPoolURL.TabIndex = 6
        '
        'txtPoolDesc
        '
        Me.txtPoolDesc.Location = New System.Drawing.Point(377, 7)
        Me.txtPoolDesc.Name = "txtPoolDesc"
        Me.txtPoolDesc.Size = New System.Drawing.Size(370, 27)
        Me.txtPoolDesc.TabIndex = 5
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(284, 95)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(88, 20)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Password:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(281, 66)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(91, 20)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "Username:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(324, 38)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(48, 20)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "URL:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(318, 10)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(54, 20)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Desc:"
        '
        'lstPools
        '
        Me.lstPools.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstPools.FormattingEnabled = True
        Me.lstPools.ItemHeight = 20
        Me.lstPools.Location = New System.Drawing.Point(6, 7)
        Me.lstPools.Name = "lstPools"
        Me.lstPools.Size = New System.Drawing.Size(269, 284)
        Me.lstPools.TabIndex = 12
        '
        'tabAlerts
        '
        Me.tabAlerts.Controls.Add(Me.tabAlertsAndReboots)
        Me.tabAlerts.Location = New System.Drawing.Point(4, 4)
        Me.tabAlerts.Name = "tabAlerts"
        Me.tabAlerts.Size = New System.Drawing.Size(1153, 325)
        Me.tabAlerts.TabIndex = 3
        Me.tabAlerts.Text = "Alerts & Reboots"
        Me.tabAlerts.UseVisualStyleBackColor = True
        '
        'tabAlertsAndReboots
        '
        Me.tabAlertsAndReboots.Alignment = System.Windows.Forms.TabAlignment.Bottom
        Me.tabAlertsAndReboots.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabAlertsAndReboots.Controls.Add(Me.tabC1Alerts)
        Me.tabAlertsAndReboots.Controls.Add(Me.tabS1Alerts)
        Me.tabAlertsAndReboots.Controls.Add(Me.tabS2Alerts)
        Me.tabAlertsAndReboots.Controls.Add(Me.tabS3Alerts)
        Me.tabAlertsAndReboots.Controls.Add(Me.tabS4Alerts)
        Me.tabAlertsAndReboots.Controls.Add(Me.tabSPAlerts)
        Me.tabAlertsAndReboots.Controls.Add(Me.tabAlertTypes)
        Me.tabAlertsAndReboots.Controls.Add(Me.tabReboots)
        Me.tabAlertsAndReboots.Controls.Add(Me.tabEmail)
        Me.tabAlertsAndReboots.Location = New System.Drawing.Point(6, 7)
        Me.tabAlertsAndReboots.Multiline = True
        Me.tabAlertsAndReboots.Name = "tabAlertsAndReboots"
        Me.tabAlertsAndReboots.SelectedIndex = 0
        Me.tabAlertsAndReboots.Size = New System.Drawing.Size(1144, 317)
        Me.tabAlertsAndReboots.TabIndex = 18
        '
        'tabC1Alerts
        '
        Me.tabC1Alerts.Controls.Add(Me.chkAlertIfC1FanLow)
        Me.tabC1Alerts.Controls.Add(Me.txtAlertC1FanLow)
        Me.tabC1Alerts.Controls.Add(Me.cmdSaveAlerts6)
        Me.tabC1Alerts.Controls.Add(Me.chkAlertIfC1XCount)
        Me.tabC1Alerts.Controls.Add(Me.txtAlertC1XCount)
        Me.tabC1Alerts.Controls.Add(Me.chkAlertIfC1Temp)
        Me.tabC1Alerts.Controls.Add(Me.chkAlertIfC1Hash)
        Me.tabC1Alerts.Controls.Add(Me.txtAlertC1FanHigh)
        Me.tabC1Alerts.Controls.Add(Me.txtAlertC1Hash)
        Me.tabC1Alerts.Controls.Add(Me.chkAlertIfC1FanHigh)
        Me.tabC1Alerts.Controls.Add(Me.txtAlertC1Temp)
        Me.tabC1Alerts.Location = New System.Drawing.Point(4, 4)
        Me.tabC1Alerts.Name = "tabC1Alerts"
        Me.tabC1Alerts.Size = New System.Drawing.Size(1136, 284)
        Me.tabC1Alerts.TabIndex = 6
        Me.tabC1Alerts.Text = "C1 Alerts"
        Me.tabC1Alerts.UseVisualStyleBackColor = True
        '
        'chkAlertIfC1FanLow
        '
        Me.chkAlertIfC1FanLow.AutoSize = True
        Me.chkAlertIfC1FanLow.Location = New System.Drawing.Point(12, 43)
        Me.chkAlertIfC1FanLow.Name = "chkAlertIfC1FanLow"
        Me.chkAlertIfC1FanLow.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfC1FanLow.TabIndex = 31
        Me.chkAlertIfC1FanLow.Text = "Fan Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfC1FanLow, "Equal to or greater than")
        Me.chkAlertIfC1FanLow.UseVisualStyleBackColor = True
        '
        'txtAlertC1FanLow
        '
        Me.txtAlertC1FanLow.Location = New System.Drawing.Point(141, 39)
        Me.txtAlertC1FanLow.Name = "txtAlertC1FanLow"
        Me.txtAlertC1FanLow.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertC1FanLow.TabIndex = 32
        Me.txtAlertC1FanLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdSaveAlerts6
        '
        Me.cmdSaveAlerts6.Location = New System.Drawing.Point(12, 216)
        Me.cmdSaveAlerts6.Name = "cmdSaveAlerts6"
        Me.cmdSaveAlerts6.Size = New System.Drawing.Size(121, 32)
        Me.cmdSaveAlerts6.TabIndex = 30
        Me.cmdSaveAlerts6.Text = "Save Config"
        Me.cmdSaveAlerts6.UseVisualStyleBackColor = True
        '
        'chkAlertIfC1XCount
        '
        Me.chkAlertIfC1XCount.AutoSize = True
        Me.chkAlertIfC1XCount.Location = New System.Drawing.Point(12, 136)
        Me.chkAlertIfC1XCount.Name = "chkAlertIfC1XCount"
        Me.chkAlertIfC1XCount.Size = New System.Drawing.Size(129, 24)
        Me.chkAlertIfC1XCount.TabIndex = 28
        Me.chkAlertIfC1XCount.Text = "XCount Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfC1XCount, "Great than or equal to")
        Me.chkAlertIfC1XCount.UseVisualStyleBackColor = True
        '
        'txtAlertC1XCount
        '
        Me.txtAlertC1XCount.Location = New System.Drawing.Point(141, 132)
        Me.txtAlertC1XCount.Name = "txtAlertC1XCount"
        Me.txtAlertC1XCount.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertC1XCount.TabIndex = 29
        Me.txtAlertC1XCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfC1Temp
        '
        Me.chkAlertIfC1Temp.AutoSize = True
        Me.chkAlertIfC1Temp.Location = New System.Drawing.Point(12, 105)
        Me.chkAlertIfC1Temp.Name = "chkAlertIfC1Temp"
        Me.chkAlertIfC1Temp.Size = New System.Drawing.Size(116, 24)
        Me.chkAlertIfC1Temp.TabIndex = 22
        Me.chkAlertIfC1Temp.Text = "Temp Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfC1Temp, "Equal to or greater than")
        Me.chkAlertIfC1Temp.UseVisualStyleBackColor = True
        '
        'chkAlertIfC1Hash
        '
        Me.chkAlertIfC1Hash.AutoSize = True
        Me.chkAlertIfC1Hash.Location = New System.Drawing.Point(12, 74)
        Me.chkAlertIfC1Hash.Name = "chkAlertIfC1Hash"
        Me.chkAlertIfC1Hash.Size = New System.Drawing.Size(114, 24)
        Me.chkAlertIfC1Hash.TabIndex = 26
        Me.chkAlertIfC1Hash.Text = "Hash Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfC1Hash, "Avg Hash is equal to or less than")
        Me.chkAlertIfC1Hash.UseVisualStyleBackColor = True
        '
        'txtAlertC1FanHigh
        '
        Me.txtAlertC1FanHigh.Location = New System.Drawing.Point(141, 8)
        Me.txtAlertC1FanHigh.Name = "txtAlertC1FanHigh"
        Me.txtAlertC1FanHigh.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertC1FanHigh.TabIndex = 25
        Me.txtAlertC1FanHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtAlertC1Hash
        '
        Me.txtAlertC1Hash.Location = New System.Drawing.Point(141, 70)
        Me.txtAlertC1Hash.Name = "txtAlertC1Hash"
        Me.txtAlertC1Hash.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertC1Hash.TabIndex = 27
        Me.txtAlertC1Hash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfC1FanHigh
        '
        Me.chkAlertIfC1FanHigh.AutoSize = True
        Me.chkAlertIfC1FanHigh.Location = New System.Drawing.Point(12, 12)
        Me.chkAlertIfC1FanHigh.Name = "chkAlertIfC1FanHigh"
        Me.chkAlertIfC1FanHigh.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfC1FanHigh.TabIndex = 24
        Me.chkAlertIfC1FanHigh.Text = "Fan Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfC1FanHigh, "Equal to or greater than")
        Me.chkAlertIfC1FanHigh.UseVisualStyleBackColor = True
        '
        'txtAlertC1Temp
        '
        Me.txtAlertC1Temp.Location = New System.Drawing.Point(141, 101)
        Me.txtAlertC1Temp.Name = "txtAlertC1Temp"
        Me.txtAlertC1Temp.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertC1Temp.TabIndex = 23
        Me.txtAlertC1Temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tabS1Alerts
        '
        Me.tabS1Alerts.Controls.Add(Me.chkAlertIfS1FanLow)
        Me.tabS1Alerts.Controls.Add(Me.txtAlertS1FanLow)
        Me.tabS1Alerts.Controls.Add(Me.cmdSaveAlerts1)
        Me.tabS1Alerts.Controls.Add(Me.chkAlertIfS1XCount)
        Me.tabS1Alerts.Controls.Add(Me.txtAlertS1XCount)
        Me.tabS1Alerts.Controls.Add(Me.chkAlertIfS1FanHigh)
        Me.tabS1Alerts.Controls.Add(Me.chkAlertIfS1Hash)
        Me.tabS1Alerts.Controls.Add(Me.txtAlertS1Temp)
        Me.tabS1Alerts.Controls.Add(Me.txtAlertS1Hash)
        Me.tabS1Alerts.Controls.Add(Me.chkAlertIfS1Temp)
        Me.tabS1Alerts.Controls.Add(Me.txtAlertS1FanHigh)
        Me.tabS1Alerts.Location = New System.Drawing.Point(4, 4)
        Me.tabS1Alerts.Name = "tabS1Alerts"
        Me.tabS1Alerts.Padding = New System.Windows.Forms.Padding(3)
        Me.tabS1Alerts.Size = New System.Drawing.Size(1136, 284)
        Me.tabS1Alerts.TabIndex = 0
        Me.tabS1Alerts.Text = "S1 Alerts"
        Me.tabS1Alerts.UseVisualStyleBackColor = True
        '
        'chkAlertIfS1FanLow
        '
        Me.chkAlertIfS1FanLow.AutoSize = True
        Me.chkAlertIfS1FanLow.Location = New System.Drawing.Point(12, 43)
        Me.chkAlertIfS1FanLow.Name = "chkAlertIfS1FanLow"
        Me.chkAlertIfS1FanLow.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfS1FanLow.TabIndex = 18
        Me.chkAlertIfS1FanLow.Text = "Fan Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS1FanLow, "Equal to or greater than")
        Me.chkAlertIfS1FanLow.UseVisualStyleBackColor = True
        '
        'txtAlertS1FanLow
        '
        Me.txtAlertS1FanLow.Location = New System.Drawing.Point(141, 39)
        Me.txtAlertS1FanLow.Name = "txtAlertS1FanLow"
        Me.txtAlertS1FanLow.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS1FanLow.TabIndex = 19
        Me.txtAlertS1FanLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdSaveAlerts1
        '
        Me.cmdSaveAlerts1.Location = New System.Drawing.Point(12, 216)
        Me.cmdSaveAlerts1.Name = "cmdSaveAlerts1"
        Me.cmdSaveAlerts1.Size = New System.Drawing.Size(121, 32)
        Me.cmdSaveAlerts1.TabIndex = 17
        Me.cmdSaveAlerts1.Text = "Save Config"
        Me.cmdSaveAlerts1.UseVisualStyleBackColor = True
        '
        'chkAlertIfS1XCount
        '
        Me.chkAlertIfS1XCount.AutoSize = True
        Me.chkAlertIfS1XCount.Location = New System.Drawing.Point(12, 136)
        Me.chkAlertIfS1XCount.Name = "chkAlertIfS1XCount"
        Me.chkAlertIfS1XCount.Size = New System.Drawing.Size(129, 24)
        Me.chkAlertIfS1XCount.TabIndex = 9
        Me.chkAlertIfS1XCount.Text = "XCount Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS1XCount, "Great than or equal to")
        Me.chkAlertIfS1XCount.UseVisualStyleBackColor = True
        '
        'txtAlertS1XCount
        '
        Me.txtAlertS1XCount.Location = New System.Drawing.Point(141, 132)
        Me.txtAlertS1XCount.Name = "txtAlertS1XCount"
        Me.txtAlertS1XCount.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS1XCount.TabIndex = 10
        Me.txtAlertS1XCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS1FanHigh
        '
        Me.chkAlertIfS1FanHigh.AutoSize = True
        Me.chkAlertIfS1FanHigh.Location = New System.Drawing.Point(12, 12)
        Me.chkAlertIfS1FanHigh.Name = "chkAlertIfS1FanHigh"
        Me.chkAlertIfS1FanHigh.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfS1FanHigh.TabIndex = 4
        Me.chkAlertIfS1FanHigh.Text = "Fan Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS1FanHigh, "Equal to or greater than")
        Me.chkAlertIfS1FanHigh.UseVisualStyleBackColor = True
        '
        'chkAlertIfS1Hash
        '
        Me.chkAlertIfS1Hash.AutoSize = True
        Me.chkAlertIfS1Hash.Location = New System.Drawing.Point(12, 74)
        Me.chkAlertIfS1Hash.Name = "chkAlertIfS1Hash"
        Me.chkAlertIfS1Hash.Size = New System.Drawing.Size(114, 24)
        Me.chkAlertIfS1Hash.TabIndex = 7
        Me.chkAlertIfS1Hash.Text = "Hash Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS1Hash, "Avg Hash is equal to or less than")
        Me.chkAlertIfS1Hash.UseVisualStyleBackColor = True
        '
        'txtAlertS1Temp
        '
        Me.txtAlertS1Temp.Location = New System.Drawing.Point(141, 101)
        Me.txtAlertS1Temp.Name = "txtAlertS1Temp"
        Me.txtAlertS1Temp.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS1Temp.TabIndex = 2
        Me.txtAlertS1Temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtAlertS1Hash
        '
        Me.txtAlertS1Hash.Location = New System.Drawing.Point(141, 70)
        Me.txtAlertS1Hash.Name = "txtAlertS1Hash"
        Me.txtAlertS1Hash.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS1Hash.TabIndex = 8
        Me.txtAlertS1Hash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS1Temp
        '
        Me.chkAlertIfS1Temp.AutoSize = True
        Me.chkAlertIfS1Temp.Location = New System.Drawing.Point(12, 105)
        Me.chkAlertIfS1Temp.Name = "chkAlertIfS1Temp"
        Me.chkAlertIfS1Temp.Size = New System.Drawing.Size(116, 24)
        Me.chkAlertIfS1Temp.TabIndex = 0
        Me.chkAlertIfS1Temp.Text = "Temp Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS1Temp, "Equal to or greater than")
        Me.chkAlertIfS1Temp.UseVisualStyleBackColor = True
        '
        'txtAlertS1FanHigh
        '
        Me.txtAlertS1FanHigh.Location = New System.Drawing.Point(141, 8)
        Me.txtAlertS1FanHigh.Name = "txtAlertS1FanHigh"
        Me.txtAlertS1FanHigh.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS1FanHigh.TabIndex = 6
        Me.txtAlertS1FanHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tabS2Alerts
        '
        Me.tabS2Alerts.Controls.Add(Me.chkAlertIfS2FanLow)
        Me.tabS2Alerts.Controls.Add(Me.txtAlertS2FanLow)
        Me.tabS2Alerts.Controls.Add(Me.cmdSaveAlerts2)
        Me.tabS2Alerts.Controls.Add(Me.chkAlertIfS2XCount)
        Me.tabS2Alerts.Controls.Add(Me.txtAlertS2XCount)
        Me.tabS2Alerts.Controls.Add(Me.chkAlertIfS2Temp)
        Me.tabS2Alerts.Controls.Add(Me.chkAlertIfS2Hash)
        Me.tabS2Alerts.Controls.Add(Me.txtAlertS2FanHigh)
        Me.tabS2Alerts.Controls.Add(Me.txtAlertS2Hash)
        Me.tabS2Alerts.Controls.Add(Me.chkAlertIfS2FanHigh)
        Me.tabS2Alerts.Controls.Add(Me.txtAlertS2Temp)
        Me.tabS2Alerts.Location = New System.Drawing.Point(4, 4)
        Me.tabS2Alerts.Name = "tabS2Alerts"
        Me.tabS2Alerts.Padding = New System.Windows.Forms.Padding(3)
        Me.tabS2Alerts.Size = New System.Drawing.Size(1136, 284)
        Me.tabS2Alerts.TabIndex = 1
        Me.tabS2Alerts.Text = "S2 Alerts"
        Me.tabS2Alerts.UseVisualStyleBackColor = True
        '
        'chkAlertIfS2FanLow
        '
        Me.chkAlertIfS2FanLow.AutoSize = True
        Me.chkAlertIfS2FanLow.Location = New System.Drawing.Point(12, 43)
        Me.chkAlertIfS2FanLow.Name = "chkAlertIfS2FanLow"
        Me.chkAlertIfS2FanLow.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfS2FanLow.TabIndex = 20
        Me.chkAlertIfS2FanLow.Text = "Fan Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS2FanLow, "Equal to or greater than")
        Me.chkAlertIfS2FanLow.UseVisualStyleBackColor = True
        '
        'txtAlertS2FanLow
        '
        Me.txtAlertS2FanLow.Location = New System.Drawing.Point(141, 39)
        Me.txtAlertS2FanLow.Name = "txtAlertS2FanLow"
        Me.txtAlertS2FanLow.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS2FanLow.TabIndex = 21
        Me.txtAlertS2FanLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdSaveAlerts2
        '
        Me.cmdSaveAlerts2.Location = New System.Drawing.Point(12, 216)
        Me.cmdSaveAlerts2.Name = "cmdSaveAlerts2"
        Me.cmdSaveAlerts2.Size = New System.Drawing.Size(121, 32)
        Me.cmdSaveAlerts2.TabIndex = 17
        Me.cmdSaveAlerts2.Text = "Save Config"
        Me.cmdSaveAlerts2.UseVisualStyleBackColor = True
        '
        'chkAlertIfS2XCount
        '
        Me.chkAlertIfS2XCount.AutoSize = True
        Me.chkAlertIfS2XCount.Location = New System.Drawing.Point(12, 136)
        Me.chkAlertIfS2XCount.Name = "chkAlertIfS2XCount"
        Me.chkAlertIfS2XCount.Size = New System.Drawing.Size(129, 24)
        Me.chkAlertIfS2XCount.TabIndex = 11
        Me.chkAlertIfS2XCount.Text = "XCount Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS2XCount, "Great than or equal to")
        Me.chkAlertIfS2XCount.UseVisualStyleBackColor = True
        '
        'txtAlertS2XCount
        '
        Me.txtAlertS2XCount.Location = New System.Drawing.Point(141, 132)
        Me.txtAlertS2XCount.Name = "txtAlertS2XCount"
        Me.txtAlertS2XCount.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS2XCount.TabIndex = 12
        Me.txtAlertS2XCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS2Temp
        '
        Me.chkAlertIfS2Temp.AutoSize = True
        Me.chkAlertIfS2Temp.Location = New System.Drawing.Point(12, 105)
        Me.chkAlertIfS2Temp.Name = "chkAlertIfS2Temp"
        Me.chkAlertIfS2Temp.Size = New System.Drawing.Size(116, 24)
        Me.chkAlertIfS2Temp.TabIndex = 1
        Me.chkAlertIfS2Temp.Text = "Temp Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS2Temp, "Equal to or greater than")
        Me.chkAlertIfS2Temp.UseVisualStyleBackColor = True
        '
        'chkAlertIfS2Hash
        '
        Me.chkAlertIfS2Hash.AutoSize = True
        Me.chkAlertIfS2Hash.Location = New System.Drawing.Point(12, 74)
        Me.chkAlertIfS2Hash.Name = "chkAlertIfS2Hash"
        Me.chkAlertIfS2Hash.Size = New System.Drawing.Size(114, 24)
        Me.chkAlertIfS2Hash.TabIndex = 9
        Me.chkAlertIfS2Hash.Text = "Hash Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS2Hash, "Avg Hash is equal to or less than")
        Me.chkAlertIfS2Hash.UseVisualStyleBackColor = True
        '
        'txtAlertS2FanHigh
        '
        Me.txtAlertS2FanHigh.Location = New System.Drawing.Point(141, 8)
        Me.txtAlertS2FanHigh.Name = "txtAlertS2FanHigh"
        Me.txtAlertS2FanHigh.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS2FanHigh.TabIndex = 7
        Me.txtAlertS2FanHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtAlertS2Hash
        '
        Me.txtAlertS2Hash.Location = New System.Drawing.Point(141, 70)
        Me.txtAlertS2Hash.Name = "txtAlertS2Hash"
        Me.txtAlertS2Hash.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS2Hash.TabIndex = 10
        Me.txtAlertS2Hash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS2FanHigh
        '
        Me.chkAlertIfS2FanHigh.AutoSize = True
        Me.chkAlertIfS2FanHigh.Location = New System.Drawing.Point(12, 12)
        Me.chkAlertIfS2FanHigh.Name = "chkAlertIfS2FanHigh"
        Me.chkAlertIfS2FanHigh.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfS2FanHigh.TabIndex = 5
        Me.chkAlertIfS2FanHigh.Text = "Fan Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS2FanHigh, "Equal to or greater than")
        Me.chkAlertIfS2FanHigh.UseVisualStyleBackColor = True
        '
        'txtAlertS2Temp
        '
        Me.txtAlertS2Temp.Location = New System.Drawing.Point(141, 101)
        Me.txtAlertS2Temp.Name = "txtAlertS2Temp"
        Me.txtAlertS2Temp.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS2Temp.TabIndex = 3
        Me.txtAlertS2Temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tabS3Alerts
        '
        Me.tabS3Alerts.Controls.Add(Me.chkAlertIfS3FanLow)
        Me.tabS3Alerts.Controls.Add(Me.txtAlertS3FanLow)
        Me.tabS3Alerts.Controls.Add(Me.cmdSaveAlerts5)
        Me.tabS3Alerts.Controls.Add(Me.chkAlertIfS3XCount)
        Me.tabS3Alerts.Controls.Add(Me.txtAlertS3XCount)
        Me.tabS3Alerts.Controls.Add(Me.chkAlertIfS3FanHigh)
        Me.tabS3Alerts.Controls.Add(Me.chkAlertIfS3Hash)
        Me.tabS3Alerts.Controls.Add(Me.txtAlertS3Temp)
        Me.tabS3Alerts.Controls.Add(Me.txtAlertS3Hash)
        Me.tabS3Alerts.Controls.Add(Me.chkAlertIfS3Temp)
        Me.tabS3Alerts.Controls.Add(Me.txtAlertS3FanHigh)
        Me.tabS3Alerts.Location = New System.Drawing.Point(4, 4)
        Me.tabS3Alerts.Name = "tabS3Alerts"
        Me.tabS3Alerts.Size = New System.Drawing.Size(1136, 284)
        Me.tabS3Alerts.TabIndex = 4
        Me.tabS3Alerts.Text = "S3 Alerts"
        Me.tabS3Alerts.UseVisualStyleBackColor = True
        '
        'chkAlertIfS3FanLow
        '
        Me.chkAlertIfS3FanLow.AutoSize = True
        Me.chkAlertIfS3FanLow.Location = New System.Drawing.Point(12, 43)
        Me.chkAlertIfS3FanLow.Name = "chkAlertIfS3FanLow"
        Me.chkAlertIfS3FanLow.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfS3FanLow.TabIndex = 29
        Me.chkAlertIfS3FanLow.Text = "Fan Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS3FanLow, "Equal to or greater than")
        Me.chkAlertIfS3FanLow.UseVisualStyleBackColor = True
        '
        'txtAlertS3FanLow
        '
        Me.txtAlertS3FanLow.Location = New System.Drawing.Point(141, 39)
        Me.txtAlertS3FanLow.Name = "txtAlertS3FanLow"
        Me.txtAlertS3FanLow.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS3FanLow.TabIndex = 30
        Me.txtAlertS3FanLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdSaveAlerts5
        '
        Me.cmdSaveAlerts5.Location = New System.Drawing.Point(12, 216)
        Me.cmdSaveAlerts5.Name = "cmdSaveAlerts5"
        Me.cmdSaveAlerts5.Size = New System.Drawing.Size(121, 32)
        Me.cmdSaveAlerts5.TabIndex = 28
        Me.cmdSaveAlerts5.Text = "Save Config"
        Me.cmdSaveAlerts5.UseVisualStyleBackColor = True
        '
        'chkAlertIfS3XCount
        '
        Me.chkAlertIfS3XCount.AutoSize = True
        Me.chkAlertIfS3XCount.Location = New System.Drawing.Point(12, 136)
        Me.chkAlertIfS3XCount.Name = "chkAlertIfS3XCount"
        Me.chkAlertIfS3XCount.Size = New System.Drawing.Size(129, 24)
        Me.chkAlertIfS3XCount.TabIndex = 26
        Me.chkAlertIfS3XCount.Text = "XCount Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS3XCount, "Great than or equal to")
        Me.chkAlertIfS3XCount.UseVisualStyleBackColor = True
        '
        'txtAlertS3XCount
        '
        Me.txtAlertS3XCount.Location = New System.Drawing.Point(141, 132)
        Me.txtAlertS3XCount.Name = "txtAlertS3XCount"
        Me.txtAlertS3XCount.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS3XCount.TabIndex = 27
        Me.txtAlertS3XCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS3FanHigh
        '
        Me.chkAlertIfS3FanHigh.AutoSize = True
        Me.chkAlertIfS3FanHigh.Location = New System.Drawing.Point(12, 12)
        Me.chkAlertIfS3FanHigh.Name = "chkAlertIfS3FanHigh"
        Me.chkAlertIfS3FanHigh.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfS3FanHigh.TabIndex = 22
        Me.chkAlertIfS3FanHigh.Text = "Fan Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS3FanHigh, "Equal to or greater than")
        Me.chkAlertIfS3FanHigh.UseVisualStyleBackColor = True
        '
        'chkAlertIfS3Hash
        '
        Me.chkAlertIfS3Hash.AutoSize = True
        Me.chkAlertIfS3Hash.Location = New System.Drawing.Point(12, 74)
        Me.chkAlertIfS3Hash.Name = "chkAlertIfS3Hash"
        Me.chkAlertIfS3Hash.Size = New System.Drawing.Size(114, 24)
        Me.chkAlertIfS3Hash.TabIndex = 24
        Me.chkAlertIfS3Hash.Text = "Hash Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS3Hash, "Avg Hash is equal to or less than")
        Me.chkAlertIfS3Hash.UseVisualStyleBackColor = True
        '
        'txtAlertS3Temp
        '
        Me.txtAlertS3Temp.Location = New System.Drawing.Point(141, 101)
        Me.txtAlertS3Temp.Name = "txtAlertS3Temp"
        Me.txtAlertS3Temp.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS3Temp.TabIndex = 21
        Me.txtAlertS3Temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtAlertS3Hash
        '
        Me.txtAlertS3Hash.Location = New System.Drawing.Point(141, 70)
        Me.txtAlertS3Hash.Name = "txtAlertS3Hash"
        Me.txtAlertS3Hash.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS3Hash.TabIndex = 25
        Me.txtAlertS3Hash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS3Temp
        '
        Me.chkAlertIfS3Temp.AutoSize = True
        Me.chkAlertIfS3Temp.Location = New System.Drawing.Point(12, 105)
        Me.chkAlertIfS3Temp.Name = "chkAlertIfS3Temp"
        Me.chkAlertIfS3Temp.Size = New System.Drawing.Size(116, 24)
        Me.chkAlertIfS3Temp.TabIndex = 20
        Me.chkAlertIfS3Temp.Text = "Temp Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS3Temp, "Equal to or greater than")
        Me.chkAlertIfS3Temp.UseVisualStyleBackColor = True
        '
        'txtAlertS3FanHigh
        '
        Me.txtAlertS3FanHigh.Location = New System.Drawing.Point(141, 8)
        Me.txtAlertS3FanHigh.Name = "txtAlertS3FanHigh"
        Me.txtAlertS3FanHigh.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS3FanHigh.TabIndex = 23
        Me.txtAlertS3FanHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tabS4Alerts
        '
        Me.tabS4Alerts.Controls.Add(Me.chkAlertIfS4FanLow)
        Me.tabS4Alerts.Controls.Add(Me.txtAlertS4FanLow)
        Me.tabS4Alerts.Controls.Add(Me.cmdSaveAlerts7)
        Me.tabS4Alerts.Controls.Add(Me.chkAlertIfS4XCount)
        Me.tabS4Alerts.Controls.Add(Me.txtAlertS4XCount)
        Me.tabS4Alerts.Controls.Add(Me.chkAlertIfS4FanHigh)
        Me.tabS4Alerts.Controls.Add(Me.chkAlertIfS4Hash)
        Me.tabS4Alerts.Controls.Add(Me.txtAlertS4Temp)
        Me.tabS4Alerts.Controls.Add(Me.txtAlertS4Hash)
        Me.tabS4Alerts.Controls.Add(Me.chkAlertIfS4Temp)
        Me.tabS4Alerts.Controls.Add(Me.txtAlertS4FanHigh)
        Me.tabS4Alerts.Location = New System.Drawing.Point(4, 4)
        Me.tabS4Alerts.Name = "tabS4Alerts"
        Me.tabS4Alerts.Size = New System.Drawing.Size(1136, 284)
        Me.tabS4Alerts.TabIndex = 8
        Me.tabS4Alerts.Text = "S4 Alerts"
        Me.tabS4Alerts.UseVisualStyleBackColor = True
        '
        'chkAlertIfS4FanLow
        '
        Me.chkAlertIfS4FanLow.AutoSize = True
        Me.chkAlertIfS4FanLow.Location = New System.Drawing.Point(12, 43)
        Me.chkAlertIfS4FanLow.Name = "chkAlertIfS4FanLow"
        Me.chkAlertIfS4FanLow.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfS4FanLow.TabIndex = 40
        Me.chkAlertIfS4FanLow.Text = "Fan Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS4FanLow, "Equal to or greater than")
        Me.chkAlertIfS4FanLow.UseVisualStyleBackColor = True
        '
        'txtAlertS4FanLow
        '
        Me.txtAlertS4FanLow.Location = New System.Drawing.Point(141, 39)
        Me.txtAlertS4FanLow.Name = "txtAlertS4FanLow"
        Me.txtAlertS4FanLow.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS4FanLow.TabIndex = 41
        Me.txtAlertS4FanLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmdSaveAlerts7
        '
        Me.cmdSaveAlerts7.Location = New System.Drawing.Point(12, 216)
        Me.cmdSaveAlerts7.Name = "cmdSaveAlerts7"
        Me.cmdSaveAlerts7.Size = New System.Drawing.Size(121, 32)
        Me.cmdSaveAlerts7.TabIndex = 39
        Me.cmdSaveAlerts7.Text = "Save Config"
        Me.cmdSaveAlerts7.UseVisualStyleBackColor = True
        '
        'chkAlertIfS4XCount
        '
        Me.chkAlertIfS4XCount.AutoSize = True
        Me.chkAlertIfS4XCount.Location = New System.Drawing.Point(12, 136)
        Me.chkAlertIfS4XCount.Name = "chkAlertIfS4XCount"
        Me.chkAlertIfS4XCount.Size = New System.Drawing.Size(129, 24)
        Me.chkAlertIfS4XCount.TabIndex = 37
        Me.chkAlertIfS4XCount.Text = "XCount Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS4XCount, "Great than or equal to")
        Me.chkAlertIfS4XCount.UseVisualStyleBackColor = True
        '
        'txtAlertS4XCount
        '
        Me.txtAlertS4XCount.Location = New System.Drawing.Point(141, 132)
        Me.txtAlertS4XCount.Name = "txtAlertS4XCount"
        Me.txtAlertS4XCount.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS4XCount.TabIndex = 38
        Me.txtAlertS4XCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS4FanHigh
        '
        Me.chkAlertIfS4FanHigh.AutoSize = True
        Me.chkAlertIfS4FanHigh.Location = New System.Drawing.Point(12, 12)
        Me.chkAlertIfS4FanHigh.Name = "chkAlertIfS4FanHigh"
        Me.chkAlertIfS4FanHigh.Size = New System.Drawing.Size(102, 24)
        Me.chkAlertIfS4FanHigh.TabIndex = 33
        Me.chkAlertIfS4FanHigh.Text = "Fan Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS4FanHigh, "Equal to or greater than")
        Me.chkAlertIfS4FanHigh.UseVisualStyleBackColor = True
        '
        'chkAlertIfS4Hash
        '
        Me.chkAlertIfS4Hash.AutoSize = True
        Me.chkAlertIfS4Hash.Location = New System.Drawing.Point(12, 74)
        Me.chkAlertIfS4Hash.Name = "chkAlertIfS4Hash"
        Me.chkAlertIfS4Hash.Size = New System.Drawing.Size(114, 24)
        Me.chkAlertIfS4Hash.TabIndex = 35
        Me.chkAlertIfS4Hash.Text = "Hash Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS4Hash, "Avg Hash is equal to or less than")
        Me.chkAlertIfS4Hash.UseVisualStyleBackColor = True
        '
        'txtAlertS4Temp
        '
        Me.txtAlertS4Temp.Location = New System.Drawing.Point(141, 101)
        Me.txtAlertS4Temp.Name = "txtAlertS4Temp"
        Me.txtAlertS4Temp.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS4Temp.TabIndex = 32
        Me.txtAlertS4Temp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtAlertS4Hash
        '
        Me.txtAlertS4Hash.Location = New System.Drawing.Point(141, 70)
        Me.txtAlertS4Hash.Name = "txtAlertS4Hash"
        Me.txtAlertS4Hash.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS4Hash.TabIndex = 36
        Me.txtAlertS4Hash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfS4Temp
        '
        Me.chkAlertIfS4Temp.AutoSize = True
        Me.chkAlertIfS4Temp.Location = New System.Drawing.Point(12, 105)
        Me.chkAlertIfS4Temp.Name = "chkAlertIfS4Temp"
        Me.chkAlertIfS4Temp.Size = New System.Drawing.Size(116, 24)
        Me.chkAlertIfS4Temp.TabIndex = 31
        Me.chkAlertIfS4Temp.Text = "Temp Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfS4Temp, "Equal to or greater than")
        Me.chkAlertIfS4Temp.UseVisualStyleBackColor = True
        '
        'txtAlertS4FanHigh
        '
        Me.txtAlertS4FanHigh.Location = New System.Drawing.Point(141, 8)
        Me.txtAlertS4FanHigh.Name = "txtAlertS4FanHigh"
        Me.txtAlertS4FanHigh.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertS4FanHigh.TabIndex = 34
        Me.txtAlertS4FanHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tabSPAlerts
        '
        Me.tabSPAlerts.Controls.Add(Me.cmdSaveAlerts8)
        Me.tabSPAlerts.Controls.Add(Me.chkAlertIfSPHash)
        Me.tabSPAlerts.Controls.Add(Me.txtAlertSPTemp)
        Me.tabSPAlerts.Controls.Add(Me.txtAlertSPHash)
        Me.tabSPAlerts.Controls.Add(Me.chkAlertIfSPTemp)
        Me.tabSPAlerts.Location = New System.Drawing.Point(4, 4)
        Me.tabSPAlerts.Name = "tabSPAlerts"
        Me.tabSPAlerts.Size = New System.Drawing.Size(1136, 284)
        Me.tabSPAlerts.TabIndex = 7
        Me.tabSPAlerts.Text = "SP Alerts"
        Me.tabSPAlerts.UseVisualStyleBackColor = True
        '
        'cmdSaveAlerts8
        '
        Me.cmdSaveAlerts8.Location = New System.Drawing.Point(12, 216)
        Me.cmdSaveAlerts8.Name = "cmdSaveAlerts8"
        Me.cmdSaveAlerts8.Size = New System.Drawing.Size(121, 32)
        Me.cmdSaveAlerts8.TabIndex = 50
        Me.cmdSaveAlerts8.Text = "Save Config"
        Me.cmdSaveAlerts8.UseVisualStyleBackColor = True
        '
        'chkAlertIfSPHash
        '
        Me.chkAlertIfSPHash.AutoSize = True
        Me.chkAlertIfSPHash.Location = New System.Drawing.Point(12, 12)
        Me.chkAlertIfSPHash.Name = "chkAlertIfSPHash"
        Me.chkAlertIfSPHash.Size = New System.Drawing.Size(114, 24)
        Me.chkAlertIfSPHash.TabIndex = 46
        Me.chkAlertIfSPHash.Text = "Hash Is <="
        Me.ToolTip1.SetToolTip(Me.chkAlertIfSPHash, "Avg Hash is equal to or less than")
        Me.chkAlertIfSPHash.UseVisualStyleBackColor = True
        '
        'txtAlertSPTemp
        '
        Me.txtAlertSPTemp.Location = New System.Drawing.Point(141, 39)
        Me.txtAlertSPTemp.Name = "txtAlertSPTemp"
        Me.txtAlertSPTemp.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertSPTemp.TabIndex = 43
        Me.txtAlertSPTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtAlertSPHash
        '
        Me.txtAlertSPHash.Location = New System.Drawing.Point(141, 8)
        Me.txtAlertSPHash.Name = "txtAlertSPHash"
        Me.txtAlertSPHash.Size = New System.Drawing.Size(54, 27)
        Me.txtAlertSPHash.TabIndex = 47
        Me.txtAlertSPHash.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'chkAlertIfSPTemp
        '
        Me.chkAlertIfSPTemp.AutoSize = True
        Me.chkAlertIfSPTemp.Location = New System.Drawing.Point(12, 43)
        Me.chkAlertIfSPTemp.Name = "chkAlertIfSPTemp"
        Me.chkAlertIfSPTemp.Size = New System.Drawing.Size(116, 24)
        Me.chkAlertIfSPTemp.TabIndex = 42
        Me.chkAlertIfSPTemp.Text = "Temp Is =>"
        Me.ToolTip1.SetToolTip(Me.chkAlertIfSPTemp, "Equal to or greater than")
        Me.chkAlertIfSPTemp.UseVisualStyleBackColor = True
        '
        'tabAlertTypes
        '
        Me.tabAlertTypes.Controls.Add(Me.cmbAlertEMailGovernor)
        Me.tabAlertTypes.Controls.Add(Me.txtAlertEMailGovernor)
        Me.tabAlertTypes.Controls.Add(Me.Label2)
        Me.tabAlertTypes.Controls.Add(Me.chkAlertSendEMail)
        Me.tabAlertTypes.Controls.Add(Me.cmdSaveAlerts3)
        Me.tabAlertTypes.Controls.Add(Me.chkAlertStartProcess)
        Me.tabAlertTypes.Controls.Add(Me.cmdAlertProcessFileFinder)
        Me.tabAlertTypes.Controls.Add(Me.chkAlertShowNotifyPopup)
        Me.tabAlertTypes.Controls.Add(Me.Label77)
        Me.tabAlertTypes.Controls.Add(Me.chkAlertShowAnnoyingPopup)
        Me.tabAlertTypes.Controls.Add(Me.txtAlertStartProcessName)
        Me.tabAlertTypes.Controls.Add(Me.chkAlertHighlightField)
        Me.tabAlertTypes.Controls.Add(Me.Label39)
        Me.tabAlertTypes.Controls.Add(Me.Label40)
        Me.tabAlertTypes.Controls.Add(Me.txtAlertStartProcessParms)
        Me.tabAlertTypes.Location = New System.Drawing.Point(4, 4)
        Me.tabAlertTypes.Name = "tabAlertTypes"
        Me.tabAlertTypes.Size = New System.Drawing.Size(1136, 284)
        Me.tabAlertTypes.TabIndex = 2
        Me.tabAlertTypes.Text = "Alert Types"
        Me.tabAlertTypes.UseVisualStyleBackColor = True
        '
        'cmbAlertEMailGovernor
        '
        Me.cmbAlertEMailGovernor.FormattingEnabled = True
        Me.cmbAlertEMailGovernor.Items.AddRange(New Object() {"Seconds", "Minutes", "Hours", "Days"})
        Me.cmbAlertEMailGovernor.Location = New System.Drawing.Point(306, 86)
        Me.cmbAlertEMailGovernor.Name = "cmbAlertEMailGovernor"
        Me.cmbAlertEMailGovernor.Size = New System.Drawing.Size(94, 28)
        Me.cmbAlertEMailGovernor.TabIndex = 7
        Me.cmbAlertEMailGovernor.Text = "Minutes"
        '
        'txtAlertEMailGovernor
        '
        Me.txtAlertEMailGovernor.Location = New System.Drawing.Point(244, 87)
        Me.txtAlertEMailGovernor.Name = "txtAlertEMailGovernor"
        Me.txtAlertEMailGovernor.Size = New System.Drawing.Size(55, 27)
        Me.txtAlertEMailGovernor.TabIndex = 6
        Me.txtAlertEMailGovernor.Text = "10"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(242, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(185, 43)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Don't send EMails more than once every"
        '
        'chkAlertSendEMail
        '
        Me.chkAlertSendEMail.AutoSize = True
        Me.chkAlertSendEMail.Location = New System.Drawing.Point(246, 12)
        Me.chkAlertSendEMail.Name = "chkAlertSendEMail"
        Me.chkAlertSendEMail.Size = New System.Drawing.Size(205, 24)
        Me.chkAlertSendEMail.TabIndex = 4
        Me.chkAlertSendEMail.Text = "Send Notification EMail"
        Me.chkAlertSendEMail.UseVisualStyleBackColor = True
        '
        'cmdSaveAlerts3
        '
        Me.cmdSaveAlerts3.Location = New System.Drawing.Point(12, 216)
        Me.cmdSaveAlerts3.Name = "cmdSaveAlerts3"
        Me.cmdSaveAlerts3.Size = New System.Drawing.Size(121, 32)
        Me.cmdSaveAlerts3.TabIndex = 22
        Me.cmdSaveAlerts3.Text = "Save Config"
        Me.cmdSaveAlerts3.UseVisualStyleBackColor = True
        '
        'chkAlertStartProcess
        '
        Me.chkAlertStartProcess.AutoSize = True
        Me.chkAlertStartProcess.Location = New System.Drawing.Point(12, 105)
        Me.chkAlertStartProcess.Name = "chkAlertStartProcess"
        Me.chkAlertStartProcess.Size = New System.Drawing.Size(132, 24)
        Me.chkAlertStartProcess.TabIndex = 3
        Me.chkAlertStartProcess.Text = "Start process"
        Me.ToolTip1.SetToolTip(Me.chkAlertStartProcess, "Could be any valid file or app, such as a sound byte or program")
        Me.chkAlertStartProcess.UseVisualStyleBackColor = True
        '
        'cmdAlertProcessFileFinder
        '
        Me.cmdAlertProcessFileFinder.Location = New System.Drawing.Point(433, 134)
        Me.cmdAlertProcessFileFinder.Name = "cmdAlertProcessFileFinder"
        Me.cmdAlertProcessFileFinder.Size = New System.Drawing.Size(26, 27)
        Me.cmdAlertProcessFileFinder.TabIndex = 20
        Me.cmdAlertProcessFileFinder.Text = "?"
        Me.cmdAlertProcessFileFinder.UseVisualStyleBackColor = True
        '
        'chkAlertShowNotifyPopup
        '
        Me.chkAlertShowNotifyPopup.AutoSize = True
        Me.chkAlertShowNotifyPopup.Checked = True
        Me.chkAlertShowNotifyPopup.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAlertShowNotifyPopup.Location = New System.Drawing.Point(12, 12)
        Me.chkAlertShowNotifyPopup.Name = "chkAlertShowNotifyPopup"
        Me.chkAlertShowNotifyPopup.Size = New System.Drawing.Size(213, 24)
        Me.chkAlertShowNotifyPopup.TabIndex = 0
        Me.chkAlertShowNotifyPopup.Text = "Show Notification Popup"
        Me.chkAlertShowNotifyPopup.UseVisualStyleBackColor = True
        '
        'Label77
        '
        Me.Label77.AutoSize = True
        Me.Label77.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label77.Location = New System.Drawing.Point(91, 193)
        Me.Label77.Name = "Label77"
        Me.Label77.Size = New System.Drawing.Size(171, 20)
        Me.Label77.TabIndex = 19
        Me.Label77.Text = "Use %A for Ant name"
        '
        'chkAlertShowAnnoyingPopup
        '
        Me.chkAlertShowAnnoyingPopup.AutoSize = True
        Me.chkAlertShowAnnoyingPopup.Location = New System.Drawing.Point(12, 43)
        Me.chkAlertShowAnnoyingPopup.Name = "chkAlertShowAnnoyingPopup"
        Me.chkAlertShowAnnoyingPopup.Size = New System.Drawing.Size(197, 24)
        Me.chkAlertShowAnnoyingPopup.TabIndex = 1
        Me.chkAlertShowAnnoyingPopup.Text = "Show Annoying Popup"
        Me.chkAlertShowAnnoyingPopup.UseVisualStyleBackColor = True
        '
        'txtAlertStartProcessName
        '
        Me.txtAlertStartProcessName.Location = New System.Drawing.Point(95, 134)
        Me.txtAlertStartProcessName.Name = "txtAlertStartProcessName"
        Me.txtAlertStartProcessName.Size = New System.Drawing.Size(332, 27)
        Me.txtAlertStartProcessName.TabIndex = 11
        '
        'chkAlertHighlightField
        '
        Me.chkAlertHighlightField.AutoSize = True
        Me.chkAlertHighlightField.Checked = True
        Me.chkAlertHighlightField.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAlertHighlightField.Location = New System.Drawing.Point(12, 74)
        Me.chkAlertHighlightField.Name = "chkAlertHighlightField"
        Me.chkAlertHighlightField.Size = New System.Drawing.Size(178, 24)
        Me.chkAlertHighlightField.TabIndex = 2
        Me.chkAlertHighlightField.Text = "Highlight Alert Field"
        Me.chkAlertHighlightField.UseVisualStyleBackColor = True
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(12, 137)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(78, 20)
        Me.Label39.TabIndex = 17
        Me.Label39.Text = "Location:"
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(12, 166)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(63, 20)
        Me.Label40.TabIndex = 18
        Me.Label40.Text = "Parms:"
        '
        'txtAlertStartProcessParms
        '
        Me.txtAlertStartProcessParms.Location = New System.Drawing.Point(95, 163)
        Me.txtAlertStartProcessParms.Name = "txtAlertStartProcessParms"
        Me.txtAlertStartProcessParms.Size = New System.Drawing.Size(332, 27)
        Me.txtAlertStartProcessParms.TabIndex = 12
        '
        'tabReboots
        '
        Me.tabReboots.Controls.Add(Me.Label27)
        Me.tabReboots.Controls.Add(Me.Label26)
        Me.tabReboots.Controls.Add(Me.Label25)
        Me.tabReboots.Controls.Add(Me.cmdSaveReboots)
        Me.tabReboots.Controls.Add(Me.cmbRebootAllAntsAtAlso)
        Me.tabReboots.Controls.Add(Me.txtRebootAllAntsAtAlso)
        Me.tabReboots.Controls.Add(Me.cmbRebootAllAntsAt)
        Me.tabReboots.Controls.Add(Me.txtRebootAllAntsAt)
        Me.tabReboots.Controls.Add(Me.txtRebootAntsByUptime)
        Me.tabReboots.Controls.Add(Me.cmbRebootAntsByUptime)
        Me.tabReboots.Controls.Add(Me.chkRebootAllAntsAtAlso)
        Me.tabReboots.Controls.Add(Me.chkRebootAllAntsAt)
        Me.tabReboots.Controls.Add(Me.chkRebootAntsByUptime)
        Me.tabReboots.Controls.Add(Me.chkRebootAntOnError)
        Me.tabReboots.Controls.Add(Me.txtAlertRebootGovernor)
        Me.tabReboots.Controls.Add(Me.chkAlertRebootIfXd)
        Me.tabReboots.Controls.Add(Me.chkAlertRebootAntsOnHashAlert)
        Me.tabReboots.Controls.Add(Me.Label8)
        Me.tabReboots.Controls.Add(Me.cmbAlertRebootGovernor)
        Me.tabReboots.Location = New System.Drawing.Point(4, 4)
        Me.tabReboots.Name = "tabReboots"
        Me.tabReboots.Size = New System.Drawing.Size(1136, 284)
        Me.tabReboots.TabIndex = 5
        Me.tabReboots.Text = "Reboots"
        Me.tabReboots.UseVisualStyleBackColor = True
        '
        'Label27
        '
        Me.Label27.Location = New System.Drawing.Point(508, 180)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(449, 90)
        Me.Label27.TabIndex = 39
        Me.Label27.Text = "- Rebooting at a certain time will trigger up to 15 minutes after the time entere" & _
    "d.  That means you can trigger an immediate reboot of all Ants by entering a val" & _
    "id time within the last 15 minutes."
        '
        'Label26
        '
        Me.Label26.Location = New System.Drawing.Point(508, 113)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(449, 67)
        Me.Label26.TabIndex = 38
        Me.Label26.Text = "- The reboot on schedule/time logic runs with the refresh logic.  If you click th" & _
    "e pause button, reboots will be paused as well as refreshes."
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(508, 86)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(58, 20)
        Me.Label25.TabIndex = 37
        Me.Label25.Text = "Notes:"
        '
        'cmdSaveReboots
        '
        Me.cmdSaveReboots.Location = New System.Drawing.Point(12, 216)
        Me.cmdSaveReboots.Name = "cmdSaveReboots"
        Me.cmdSaveReboots.Size = New System.Drawing.Size(121, 32)
        Me.cmdSaveReboots.TabIndex = 36
        Me.cmdSaveReboots.Text = "Save Config"
        Me.cmdSaveReboots.UseVisualStyleBackColor = True
        '
        'cmbRebootAllAntsAtAlso
        '
        Me.cmbRebootAllAntsAtAlso.FormattingEnabled = True
        Me.cmbRebootAllAntsAtAlso.Items.AddRange(New Object() {"AM", "PM"})
        Me.cmbRebootAllAntsAtAlso.Location = New System.Drawing.Point(353, 165)
        Me.cmbRebootAllAntsAtAlso.Name = "cmbRebootAllAntsAtAlso"
        Me.cmbRebootAllAntsAtAlso.Size = New System.Drawing.Size(56, 28)
        Me.cmbRebootAllAntsAtAlso.TabIndex = 35
        Me.cmbRebootAllAntsAtAlso.Text = "AM"
        '
        'txtRebootAllAntsAtAlso
        '
        Me.txtRebootAllAntsAtAlso.Location = New System.Drawing.Point(291, 166)
        Me.txtRebootAllAntsAtAlso.Name = "txtRebootAllAntsAtAlso"
        Me.txtRebootAllAntsAtAlso.Size = New System.Drawing.Size(55, 27)
        Me.txtRebootAllAntsAtAlso.TabIndex = 34
        Me.txtRebootAllAntsAtAlso.Text = "12:00"
        '
        'cmbRebootAllAntsAt
        '
        Me.cmbRebootAllAntsAt.FormattingEnabled = True
        Me.cmbRebootAllAntsAt.Items.AddRange(New Object() {"AM", "PM"})
        Me.cmbRebootAllAntsAt.Location = New System.Drawing.Point(353, 135)
        Me.cmbRebootAllAntsAt.Name = "cmbRebootAllAntsAt"
        Me.cmbRebootAllAntsAt.Size = New System.Drawing.Size(56, 28)
        Me.cmbRebootAllAntsAt.TabIndex = 33
        Me.cmbRebootAllAntsAt.Text = "AM"
        '
        'txtRebootAllAntsAt
        '
        Me.txtRebootAllAntsAt.Location = New System.Drawing.Point(291, 136)
        Me.txtRebootAllAntsAt.Name = "txtRebootAllAntsAt"
        Me.txtRebootAllAntsAt.Size = New System.Drawing.Size(55, 27)
        Me.txtRebootAllAntsAt.TabIndex = 32
        Me.txtRebootAllAntsAt.Text = "12:00"
        '
        'txtRebootAntsByUptime
        '
        Me.txtRebootAntsByUptime.Location = New System.Drawing.Point(291, 106)
        Me.txtRebootAntsByUptime.Name = "txtRebootAntsByUptime"
        Me.txtRebootAntsByUptime.Size = New System.Drawing.Size(55, 27)
        Me.txtRebootAntsByUptime.TabIndex = 30
        Me.txtRebootAntsByUptime.Text = "30"
        '
        'cmbRebootAntsByUptime
        '
        Me.cmbRebootAntsByUptime.FormattingEnabled = True
        Me.cmbRebootAntsByUptime.Items.AddRange(New Object() {"Seconds", "Minutes", "Hours", "Days"})
        Me.cmbRebootAntsByUptime.Location = New System.Drawing.Point(353, 105)
        Me.cmbRebootAntsByUptime.Name = "cmbRebootAntsByUptime"
        Me.cmbRebootAntsByUptime.Size = New System.Drawing.Size(94, 28)
        Me.cmbRebootAntsByUptime.TabIndex = 31
        Me.cmbRebootAntsByUptime.Text = "Minutes"
        '
        'chkRebootAllAntsAtAlso
        '
        Me.chkRebootAllAntsAtAlso.AutoSize = True
        Me.chkRebootAllAntsAtAlso.Location = New System.Drawing.Point(12, 169)
        Me.chkRebootAllAntsAtAlso.Name = "chkRebootAllAntsAtAlso"
        Me.chkRebootAllAntsAtAlso.Size = New System.Drawing.Size(201, 24)
        Me.chkRebootAllAntsAtAlso.TabIndex = 29
        Me.chkRebootAllAntsAtAlso.Text = "Also reboot all Ants at:"
        Me.chkRebootAllAntsAtAlso.UseVisualStyleBackColor = True
        '
        'chkRebootAllAntsAt
        '
        Me.chkRebootAllAntsAt.AutoSize = True
        Me.chkRebootAllAntsAt.Location = New System.Drawing.Point(12, 139)
        Me.chkRebootAllAntsAt.Name = "chkRebootAllAntsAt"
        Me.chkRebootAllAntsAt.Size = New System.Drawing.Size(169, 24)
        Me.chkRebootAllAntsAt.TabIndex = 28
        Me.chkRebootAllAntsAt.Text = "Reboot all Ants at:"
        Me.chkRebootAllAntsAt.UseVisualStyleBackColor = True
        '
        'chkRebootAntsByUptime
        '
        Me.chkRebootAntsByUptime.AutoSize = True
        Me.chkRebootAntsByUptime.Location = New System.Drawing.Point(12, 109)
        Me.chkRebootAntsByUptime.Name = "chkRebootAntsByUptime"
        Me.chkRebootAntsByUptime.Size = New System.Drawing.Size(281, 24)
        Me.chkRebootAntsByUptime.TabIndex = 27
        Me.chkRebootAntsByUptime.Text = "Reboot an Ant when Uptime hits: "
        Me.chkRebootAntsByUptime.UseVisualStyleBackColor = True
        '
        'chkRebootAntOnError
        '
        Me.chkRebootAntOnError.AutoSize = True
        Me.chkRebootAntOnError.Location = New System.Drawing.Point(12, 69)
        Me.chkRebootAntOnError.Name = "chkRebootAntOnError"
        Me.chkRebootAntOnError.Size = New System.Drawing.Size(449, 24)
        Me.chkRebootAntOnError.TabIndex = 9
        Me.chkRebootAntOnError.Text = "Attempt to reboot ants that get an ERROR on refreshing"
        Me.chkRebootAntOnError.UseVisualStyleBackColor = True
        '
        'txtAlertRebootGovernor
        '
        Me.txtAlertRebootGovernor.Location = New System.Drawing.Point(508, 37)
        Me.txtAlertRebootGovernor.Name = "txtAlertRebootGovernor"
        Me.txtAlertRebootGovernor.Size = New System.Drawing.Size(55, 27)
        Me.txtAlertRebootGovernor.TabIndex = 25
        Me.txtAlertRebootGovernor.Text = "30"
        '
        'chkAlertRebootIfXd
        '
        Me.chkAlertRebootIfXd.AutoSize = True
        Me.chkAlertRebootIfXd.Checked = True
        Me.chkAlertRebootIfXd.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAlertRebootIfXd.Location = New System.Drawing.Point(12, 12)
        Me.chkAlertRebootIfXd.Name = "chkAlertRebootIfXd"
        Me.chkAlertRebootIfXd.Size = New System.Drawing.Size(316, 24)
        Me.chkAlertRebootIfXd.TabIndex = 7
        Me.chkAlertRebootIfXd.Text = "Reboot Ants that trigger XCount alerts"
        Me.chkAlertRebootIfXd.UseVisualStyleBackColor = True
        '
        'chkAlertRebootAntsOnHashAlert
        '
        Me.chkAlertRebootAntsOnHashAlert.AutoSize = True
        Me.chkAlertRebootAntsOnHashAlert.Checked = True
        Me.chkAlertRebootAntsOnHashAlert.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAlertRebootAntsOnHashAlert.Location = New System.Drawing.Point(12, 39)
        Me.chkAlertRebootAntsOnHashAlert.Name = "chkAlertRebootAntsOnHashAlert"
        Me.chkAlertRebootAntsOnHashAlert.Size = New System.Drawing.Size(367, 24)
        Me.chkAlertRebootAntsOnHashAlert.TabIndex = 8
        Me.chkAlertRebootAntsOnHashAlert.Text = "Reboot Ants that trigger low/high Hash alerts"
        Me.chkAlertRebootAntsOnHashAlert.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(506, 12)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(320, 28)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Don't reboot more than once every"
        '
        'cmbAlertRebootGovernor
        '
        Me.cmbAlertRebootGovernor.FormattingEnabled = True
        Me.cmbAlertRebootGovernor.Items.AddRange(New Object() {"Seconds", "Minutes", "Hours", "Days"})
        Me.cmbAlertRebootGovernor.Location = New System.Drawing.Point(570, 36)
        Me.cmbAlertRebootGovernor.Name = "cmbAlertRebootGovernor"
        Me.cmbAlertRebootGovernor.Size = New System.Drawing.Size(94, 28)
        Me.cmbAlertRebootGovernor.TabIndex = 26
        Me.cmbAlertRebootGovernor.Text = "Minutes"
        '
        'tabEmail
        '
        Me.tabEmail.Controls.Add(Me.cmdSendTestEMail)
        Me.tabEmail.Controls.Add(Me.txtSMTPAlertAddress)
        Me.tabEmail.Controls.Add(Me.Label92)
        Me.tabEmail.Controls.Add(Me.txtSMTPAlertSubject)
        Me.tabEmail.Controls.Add(Me.Label93)
        Me.tabEmail.Controls.Add(Me.txtSMTPAlertName)
        Me.tabEmail.Controls.Add(Me.Label96)
        Me.tabEmail.Controls.Add(Me.Label94)
        Me.tabEmail.Controls.Add(Me.Label95)
        Me.tabEmail.Controls.Add(Me.Label85)
        Me.tabEmail.Controls.Add(Me.cmdSaveAlerts4)
        Me.tabEmail.Controls.Add(Me.txtSMTPServer)
        Me.tabEmail.Controls.Add(Me.txtSMTPPort)
        Me.tabEmail.Controls.Add(Me.Label101)
        Me.tabEmail.Controls.Add(Me.Label89)
        Me.tabEmail.Controls.Add(Me.Label102)
        Me.tabEmail.Controls.Add(Me.txtSMTPUserName)
        Me.tabEmail.Controls.Add(Me.txtSMTPFromName)
        Me.tabEmail.Controls.Add(Me.Label90)
        Me.tabEmail.Controls.Add(Me.Label103)
        Me.tabEmail.Controls.Add(Me.txtSMTPPassword)
        Me.tabEmail.Controls.Add(Me.txtSMTPFromAddress)
        Me.tabEmail.Controls.Add(Me.Label91)
        Me.tabEmail.Controls.Add(Me.chkSMTPSSL)
        Me.tabEmail.Controls.Add(Me.Label100)
        Me.tabEmail.Controls.Add(Me.Label99)
        Me.tabEmail.Controls.Add(Me.Label98)
        Me.tabEmail.Controls.Add(Me.Label97)
        Me.tabEmail.Location = New System.Drawing.Point(4, 4)
        Me.tabEmail.Name = "tabEmail"
        Me.tabEmail.Size = New System.Drawing.Size(1136, 284)
        Me.tabEmail.TabIndex = 3
        Me.tabEmail.Text = "EMail Config"
        Me.tabEmail.UseVisualStyleBackColor = True
        '
        'cmdSendTestEMail
        '
        Me.cmdSendTestEMail.Location = New System.Drawing.Point(647, 129)
        Me.cmdSendTestEMail.Name = "cmdSendTestEMail"
        Me.cmdSendTestEMail.Size = New System.Drawing.Size(153, 29)
        Me.cmdSendTestEMail.TabIndex = 53
        Me.cmdSendTestEMail.Text = "Send Test EMail"
        Me.cmdSendTestEMail.UseVisualStyleBackColor = True
        '
        'txtSMTPAlertAddress
        '
        Me.txtSMTPAlertAddress.Location = New System.Drawing.Point(647, 67)
        Me.txtSMTPAlertAddress.Name = "txtSMTPAlertAddress"
        Me.txtSMTPAlertAddress.Size = New System.Drawing.Size(332, 27)
        Me.txtSMTPAlertAddress.TabIndex = 46
        '
        'Label92
        '
        Me.Label92.AutoSize = True
        Me.Label92.Location = New System.Drawing.Point(517, 70)
        Me.Label92.Name = "Label92"
        Me.Label92.Size = New System.Drawing.Size(76, 20)
        Me.Label92.TabIndex = 48
        Me.Label92.Text = "Address:"
        '
        'txtSMTPAlertSubject
        '
        Me.txtSMTPAlertSubject.Location = New System.Drawing.Point(647, 96)
        Me.txtSMTPAlertSubject.Name = "txtSMTPAlertSubject"
        Me.txtSMTPAlertSubject.Size = New System.Drawing.Size(332, 27)
        Me.txtSMTPAlertSubject.TabIndex = 47
        '
        'Label93
        '
        Me.Label93.AutoSize = True
        Me.Label93.Location = New System.Drawing.Point(517, 99)
        Me.Label93.Name = "Label93"
        Me.Label93.Size = New System.Drawing.Size(70, 20)
        Me.Label93.TabIndex = 49
        Me.Label93.Text = "Subject:"
        '
        'txtSMTPAlertName
        '
        Me.txtSMTPAlertName.Location = New System.Drawing.Point(647, 38)
        Me.txtSMTPAlertName.Name = "txtSMTPAlertName"
        Me.txtSMTPAlertName.Size = New System.Drawing.Size(332, 27)
        Me.txtSMTPAlertName.TabIndex = 45
        '
        'Label96
        '
        Me.Label96.AutoSize = True
        Me.Label96.ForeColor = System.Drawing.Color.Red
        Me.Label96.Location = New System.Drawing.Point(981, 73)
        Me.Label96.Name = "Label96"
        Me.Label96.Size = New System.Drawing.Size(15, 20)
        Me.Label96.TabIndex = 52
        Me.Label96.Text = "*"
        '
        'Label94
        '
        Me.Label94.AutoSize = True
        Me.Label94.Location = New System.Drawing.Point(517, 41)
        Me.Label94.Name = "Label94"
        Me.Label94.Size = New System.Drawing.Size(58, 20)
        Me.Label94.TabIndex = 50
        Me.Label94.Text = "Name:"
        '
        'Label95
        '
        Me.Label95.AutoSize = True
        Me.Label95.ForeColor = System.Drawing.Color.Maroon
        Me.Label95.Location = New System.Drawing.Point(517, 11)
        Me.Label95.Name = "Label95"
        Me.Label95.Size = New System.Drawing.Size(109, 20)
        Me.Label95.TabIndex = 51
        Me.Label95.Text = "Send alert to:"
        '
        'Label85
        '
        Me.Label85.AutoSize = True
        Me.Label85.Location = New System.Drawing.Point(10, 12)
        Me.Label85.Name = "Label85"
        Me.Label85.Size = New System.Drawing.Size(112, 20)
        Me.Label85.TabIndex = 30
        Me.Label85.Text = "Server Name:"
        '
        'cmdSaveAlerts4
        '
        Me.cmdSaveAlerts4.Location = New System.Drawing.Point(12, 216)
        Me.cmdSaveAlerts4.Name = "cmdSaveAlerts4"
        Me.cmdSaveAlerts4.Size = New System.Drawing.Size(121, 32)
        Me.cmdSaveAlerts4.TabIndex = 16
        Me.cmdSaveAlerts4.Text = "Save Config"
        Me.cmdSaveAlerts4.UseVisualStyleBackColor = True
        '
        'txtSMTPServer
        '
        Me.txtSMTPServer.Location = New System.Drawing.Point(140, 9)
        Me.txtSMTPServer.Name = "txtSMTPServer"
        Me.txtSMTPServer.Size = New System.Drawing.Size(332, 27)
        Me.txtSMTPServer.TabIndex = 27
        '
        'txtSMTPPort
        '
        Me.txtSMTPPort.Location = New System.Drawing.Point(140, 38)
        Me.txtSMTPPort.Name = "txtSMTPPort"
        Me.txtSMTPPort.Size = New System.Drawing.Size(63, 27)
        Me.txtSMTPPort.TabIndex = 28
        '
        'Label101
        '
        Me.Label101.AutoSize = True
        Me.Label101.ForeColor = System.Drawing.Color.Red
        Me.Label101.Location = New System.Drawing.Point(474, 159)
        Me.Label101.Name = "Label101"
        Me.Label101.Size = New System.Drawing.Size(15, 20)
        Me.Label101.TabIndex = 44
        Me.Label101.Text = "*"
        '
        'Label89
        '
        Me.Label89.AutoSize = True
        Me.Label89.Location = New System.Drawing.Point(10, 41)
        Me.Label89.Name = "Label89"
        Me.Label89.Size = New System.Drawing.Size(99, 20)
        Me.Label89.TabIndex = 33
        Me.Label89.Text = "Server Port:"
        '
        'Label102
        '
        Me.Label102.AutoSize = True
        Me.Label102.Location = New System.Drawing.Point(10, 129)
        Me.Label102.Name = "Label102"
        Me.Label102.Size = New System.Drawing.Size(102, 20)
        Me.Label102.TabIndex = 43
        Me.Label102.Text = "From Name:"
        '
        'txtSMTPUserName
        '
        Me.txtSMTPUserName.Location = New System.Drawing.Point(140, 67)
        Me.txtSMTPUserName.Name = "txtSMTPUserName"
        Me.txtSMTPUserName.Size = New System.Drawing.Size(332, 27)
        Me.txtSMTPUserName.TabIndex = 29
        '
        'txtSMTPFromName
        '
        Me.txtSMTPFromName.Location = New System.Drawing.Point(140, 126)
        Me.txtSMTPFromName.Name = "txtSMTPFromName"
        Me.txtSMTPFromName.Size = New System.Drawing.Size(332, 27)
        Me.txtSMTPFromName.TabIndex = 32
        '
        'Label90
        '
        Me.Label90.AutoSize = True
        Me.Label90.Location = New System.Drawing.Point(10, 70)
        Me.Label90.Name = "Label90"
        Me.Label90.Size = New System.Drawing.Size(91, 20)
        Me.Label90.TabIndex = 35
        Me.Label90.Text = "Username:"
        '
        'Label103
        '
        Me.Label103.AutoSize = True
        Me.Label103.Location = New System.Drawing.Point(10, 158)
        Me.Label103.Name = "Label103"
        Me.Label103.Size = New System.Drawing.Size(120, 20)
        Me.Label103.TabIndex = 42
        Me.Label103.Text = "From Address:"
        '
        'txtSMTPPassword
        '
        Me.txtSMTPPassword.Location = New System.Drawing.Point(140, 96)
        Me.txtSMTPPassword.Name = "txtSMTPPassword"
        Me.txtSMTPPassword.Size = New System.Drawing.Size(332, 27)
        Me.txtSMTPPassword.TabIndex = 31
        '
        'txtSMTPFromAddress
        '
        Me.txtSMTPFromAddress.Location = New System.Drawing.Point(140, 155)
        Me.txtSMTPFromAddress.Name = "txtSMTPFromAddress"
        Me.txtSMTPFromAddress.Size = New System.Drawing.Size(332, 27)
        Me.txtSMTPFromAddress.TabIndex = 34
        '
        'Label91
        '
        Me.Label91.AutoSize = True
        Me.Label91.Location = New System.Drawing.Point(10, 99)
        Me.Label91.Name = "Label91"
        Me.Label91.Size = New System.Drawing.Size(88, 20)
        Me.Label91.TabIndex = 36
        Me.Label91.Text = "Password:"
        '
        'chkSMTPSSL
        '
        Me.chkSMTPSSL.AutoSize = True
        Me.chkSMTPSSL.Location = New System.Drawing.Point(374, 42)
        Me.chkSMTPSSL.Name = "chkSMTPSSL"
        Me.chkSMTPSSL.Size = New System.Drawing.Size(98, 24)
        Me.chkSMTPSSL.TabIndex = 41
        Me.chkSMTPSSL.Text = "Use SSL"
        Me.chkSMTPSSL.UseVisualStyleBackColor = True
        '
        'Label100
        '
        Me.Label100.AutoSize = True
        Me.Label100.ForeColor = System.Drawing.Color.Red
        Me.Label100.Location = New System.Drawing.Point(474, 13)
        Me.Label100.Name = "Label100"
        Me.Label100.Size = New System.Drawing.Size(15, 20)
        Me.Label100.TabIndex = 40
        Me.Label100.Text = "*"
        '
        'Label99
        '
        Me.Label99.AutoSize = True
        Me.Label99.ForeColor = System.Drawing.Color.Red
        Me.Label99.Location = New System.Drawing.Point(206, 42)
        Me.Label99.Name = "Label99"
        Me.Label99.Size = New System.Drawing.Size(15, 20)
        Me.Label99.TabIndex = 39
        Me.Label99.Text = "*"
        '
        'Label98
        '
        Me.Label98.AutoSize = True
        Me.Label98.ForeColor = System.Drawing.Color.Red
        Me.Label98.Location = New System.Drawing.Point(474, 71)
        Me.Label98.Name = "Label98"
        Me.Label98.Size = New System.Drawing.Size(15, 20)
        Me.Label98.TabIndex = 38
        Me.Label98.Text = "*"
        '
        'Label97
        '
        Me.Label97.AutoSize = True
        Me.Label97.ForeColor = System.Drawing.Color.Red
        Me.Label97.Location = New System.Drawing.Point(474, 99)
        Me.Label97.Name = "Label97"
        Me.Label97.Size = New System.Drawing.Size(15, 20)
        Me.Label97.TabIndex = 37
        Me.Label97.Text = "*"
        '
        'tabLog
        '
        Me.tabLog.Controls.Add(Me.txtLog)
        Me.tabLog.Location = New System.Drawing.Point(4, 4)
        Me.tabLog.Name = "tabLog"
        Me.tabLog.Size = New System.Drawing.Size(1153, 325)
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
        Me.txtLog.Size = New System.Drawing.Size(1208, 316)
        Me.txtLog.TabIndex = 0
        '
        'cmbRefreshRate
        '
        Me.cmbRefreshRate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmbRefreshRate.FormattingEnabled = True
        Me.cmbRefreshRate.Items.AddRange(New Object() {"Seconds", "Minutes", "Hours"})
        Me.cmbRefreshRate.Location = New System.Drawing.Point(995, 369)
        Me.cmbRefreshRate.Name = "cmbRefreshRate"
        Me.cmbRefreshRate.Size = New System.Drawing.Size(94, 28)
        Me.cmbRefreshRate.TabIndex = 47
        Me.cmbRefreshRate.Text = "Seconds"
        '
        'txtRefreshRate
        '
        Me.txtRefreshRate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtRefreshRate.Location = New System.Drawing.Point(933, 370)
        Me.txtRefreshRate.Name = "txtRefreshRate"
        Me.txtRefreshRate.Size = New System.Drawing.Size(55, 27)
        Me.txtRefreshRate.TabIndex = 46
        Me.txtRefreshRate.Text = "300"
        '
        'Label16
        '
        Me.Label16.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(812, 372)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(118, 20)
        Me.Label16.TabIndex = 45
        Me.Label16.Text = "Refresh every:"
        '
        'cmdPause
        '
        Me.cmdPause.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdPause.Location = New System.Drawing.Point(692, 366)
        Me.cmdPause.Name = "cmdPause"
        Me.cmdPause.Size = New System.Drawing.Size(85, 28)
        Me.cmdPause.TabIndex = 3
        Me.cmdPause.Text = "Pause"
        Me.cmdPause.UseVisualStyleBackColor = True
        '
        'txtPleaseSupport
        '
        Me.txtPleaseSupport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtPleaseSupport.Location = New System.Drawing.Point(6, 367)
        Me.txtPleaseSupport.Name = "txtPleaseSupport"
        Me.txtPleaseSupport.ReadOnly = True
        Me.txtPleaseSupport.Size = New System.Drawing.Size(501, 27)
        Me.txtPleaseSupport.TabIndex = 100
        Me.txtPleaseSupport.TabStop = False
        Me.txtPleaseSupport.Text = "Donations: 1DHT7jkmP7msxb3YbyGPettGm3VmJS9Wtk"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.ContextMenuStrip = Me.menuStripNotifyIcon
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "M's Pool Monitor"
        Me.NotifyIcon1.Visible = True
        '
        'menuStripNotifyIcon
        '
        Me.menuStripNotifyIcon.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShow, Me.mnuExit})
        Me.menuStripNotifyIcon.Name = "ContextMenuStrip1"
        Me.menuStripNotifyIcon.Size = New System.Drawing.Size(122, 60)
        '
        'mnuShow
        '
        Me.mnuShow.Name = "mnuShow"
        Me.mnuShow.Size = New System.Drawing.Size(121, 28)
        Me.mnuShow.Text = "Show"
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(121, 28)
        Me.mnuExit.Text = "Exit"
        '
        'menuStripMain
        '
        Me.menuStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMainExit})
        Me.menuStripMain.Name = "menuStripMain"
        Me.menuStripMain.Size = New System.Drawing.Size(108, 32)
        '
        'mnuMainExit
        '
        Me.mnuMainExit.Name = "mnuMainExit"
        Me.mnuMainExit.Size = New System.Drawing.Size(107, 28)
        Me.mnuMainExit.Text = "E&xit"
        '
        'timerDoStuff
        '
        Me.timerDoStuff.Interval = 10
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1175, 401)
        Me.ContextMenuStrip = Me.menuStripMain
        Me.Controls.Add(Me.txtPleaseSupport)
        Me.Controls.Add(Me.cmdPause)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Controls.Add(Me.cmbRefreshRate)
        Me.Controls.Add(Me.txtRefreshRate)
        Me.Controls.Add(Me.Label16)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(144, 180)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "M's Miner Monitor"
        Me.mnuAntMenu.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.tabMiners.ResumeLayout(False)
        CType(Me.dataMiners, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabMinerConfig.ResumeLayout(False)
        Me.tabMinerConfig.PerformLayout()
        CType(Me.dataAntConfig, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabConfig.ResumeLayout(False)
        Me.tabConfig.PerformLayout()
        CType(Me.trackThreadCount, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tabPools.ResumeLayout(False)
        Me.tabPools.PerformLayout()
        Me.tabAlerts.ResumeLayout(False)
        Me.tabAlertsAndReboots.ResumeLayout(False)
        Me.tabC1Alerts.ResumeLayout(False)
        Me.tabC1Alerts.PerformLayout()
        Me.tabS1Alerts.ResumeLayout(False)
        Me.tabS1Alerts.PerformLayout()
        Me.tabS2Alerts.ResumeLayout(False)
        Me.tabS2Alerts.PerformLayout()
        Me.tabS3Alerts.ResumeLayout(False)
        Me.tabS3Alerts.PerformLayout()
        Me.tabS4Alerts.ResumeLayout(False)
        Me.tabS4Alerts.PerformLayout()
        Me.tabSPAlerts.ResumeLayout(False)
        Me.tabSPAlerts.PerformLayout()
        Me.tabAlertTypes.ResumeLayout(False)
        Me.tabAlertTypes.PerformLayout()
        Me.tabReboots.ResumeLayout(False)
        Me.tabReboots.PerformLayout()
        Me.tabEmail.ResumeLayout(False)
        Me.tabEmail.PerformLayout()
        Me.tabLog.ResumeLayout(False)
        Me.tabLog.PerformLayout()
        Me.menuStripNotifyIcon.ResumeLayout(False)
        Me.menuStripMain.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    'Friend WithEvents dataMiners As System.Windows.Forms.DataGridView
    Friend WithEvents dataMiners As dgvWrapper
    Friend WithEvents TimerRefresh As System.Windows.Forms.Timer
    Friend WithEvents cmdRefresh As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabMiners As System.Windows.Forms.TabPage
    Friend WithEvents tabConfig As System.Windows.Forms.TabPage
    Friend WithEvents cmdDelAnt As System.Windows.Forms.Button
    Friend WithEvents cmdAddAnt As System.Windows.Forms.Button
    Friend WithEvents txtAntAddress As System.Windows.Forms.TextBox
    Friend WithEvents cmdPause As System.Windows.Forms.Button
    Friend WithEvents txtAntSSHPassword As System.Windows.Forms.TextBox
    Friend WithEvents lblWBPassword As System.Windows.Forms.Label
    Friend WithEvents txtAntSSHUsername As System.Windows.Forms.TextBox
    Friend WithEvents lblWBUserName As System.Windows.Forms.Label
    Friend WithEvents cmdSaveConfig As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
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
    Friend WithEvents chkShowHighTemp As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowHighFan As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowXCount As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowStale As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowRej As System.Windows.Forms.CheckBox
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
    Friend WithEvents chkAlertHighlightField As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertShowNotifyPopup As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertShowAnnoyingPopup As System.Windows.Forms.CheckBox
    Friend WithEvents cmdSaveAlerts4 As System.Windows.Forms.Button
    Friend WithEvents chkAlertIfS1FanHigh As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertIfS2FanHigh As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS1FanHigh As System.Windows.Forms.TextBox
    Friend WithEvents txtAlertS2FanHigh As System.Windows.Forms.TextBox
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
    Friend WithEvents tabAlertsAndReboots As System.Windows.Forms.TabControl
    Friend WithEvents tabS1Alerts As System.Windows.Forms.TabPage
    Friend WithEvents cmdSaveAlerts1 As System.Windows.Forms.Button
    Friend WithEvents tabS2Alerts As System.Windows.Forms.TabPage
    Friend WithEvents cmdSaveAlerts2 As System.Windows.Forms.Button
    Friend WithEvents tabAlertTypes As System.Windows.Forms.TabPage
    Friend WithEvents cmdSaveAlerts3 As System.Windows.Forms.Button
    Friend WithEvents tabEmail As System.Windows.Forms.TabPage
    Friend WithEvents cmdSendTestEMail As System.Windows.Forms.Button
    Friend WithEvents txtSMTPAlertAddress As System.Windows.Forms.TextBox
    Friend WithEvents Label92 As System.Windows.Forms.Label
    Friend WithEvents txtSMTPAlertSubject As System.Windows.Forms.TextBox
    Friend WithEvents Label93 As System.Windows.Forms.Label
    Friend WithEvents txtSMTPAlertName As System.Windows.Forms.TextBox
    Friend WithEvents Label96 As System.Windows.Forms.Label
    Friend WithEvents Label94 As System.Windows.Forms.Label
    Friend WithEvents Label95 As System.Windows.Forms.Label
    Friend WithEvents Label85 As System.Windows.Forms.Label
    Friend WithEvents txtSMTPServer As System.Windows.Forms.TextBox
    Friend WithEvents txtSMTPPort As System.Windows.Forms.TextBox
    Friend WithEvents Label101 As System.Windows.Forms.Label
    Friend WithEvents Label89 As System.Windows.Forms.Label
    Friend WithEvents Label102 As System.Windows.Forms.Label
    Friend WithEvents txtSMTPUserName As System.Windows.Forms.TextBox
    Friend WithEvents txtSMTPFromName As System.Windows.Forms.TextBox
    Friend WithEvents Label90 As System.Windows.Forms.Label
    Friend WithEvents Label103 As System.Windows.Forms.Label
    Friend WithEvents txtSMTPPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtSMTPFromAddress As System.Windows.Forms.TextBox
    Friend WithEvents Label91 As System.Windows.Forms.Label
    Friend WithEvents chkSMTPSSL As System.Windows.Forms.CheckBox
    Friend WithEvents Label100 As System.Windows.Forms.Label
    Friend WithEvents Label99 As System.Windows.Forms.Label
    Friend WithEvents Label98 As System.Windows.Forms.Label
    Friend WithEvents Label97 As System.Windows.Forms.Label
    Friend WithEvents chkAlertSendEMail As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbAlertEMailGovernor As System.Windows.Forms.ComboBox
    Friend WithEvents txtAlertEMailGovernor As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents chkShowDifficulty As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtAntWebPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtAntWebUsername As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmdSaveAnt As System.Windows.Forms.Button
    Friend WithEvents chkShowACount As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowSelectionColumn As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertRebootIfXd As System.Windows.Forms.CheckBox
    Friend WithEvents cmbAlertRebootGovernor As System.Windows.Forms.ComboBox
    Friend WithEvents txtAlertRebootGovernor As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents mnuAntMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuRebootAnt As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRebootMultiple As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkAlertIfS1FanLow As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS1FanLow As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfS2FanLow As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS2FanLow As System.Windows.Forms.TextBox
    Friend WithEvents mnuShutdownS2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabPools As System.Windows.Forms.TabPage
    Friend WithEvents cmdPoolDelete As System.Windows.Forms.Button
    Friend WithEvents cmdPoolChange As System.Windows.Forms.Button
    Friend WithEvents cmdPoolAdd As System.Windows.Forms.Button
    Friend WithEvents txtPoolPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtPoolUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtPoolURL As System.Windows.Forms.TextBox
    Friend WithEvents txtPoolDesc As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lstPools As MMinerMonitor.enListbox
    Friend WithEvents cmdPoolsImportFromAnts As System.Windows.Forms.Button
    Friend WithEvents cmdPoolClear3 As System.Windows.Forms.Button
    Friend WithEvents cmdPoolClear2 As System.Windows.Forms.Button
    Friend WithEvents cmdPoolMake3 As System.Windows.Forms.Button
    Friend WithEvents cmdPoolMake2 As System.Windows.Forms.Button
    Friend WithEvents cmdPoolMake1 As System.Windows.Forms.Button
    Friend WithEvents lblPools3 As System.Windows.Forms.Label
    Friend WithEvents lblPools2 As System.Windows.Forms.Label
    Friend WithEvents lblPools1 As System.Windows.Forms.Label
    Friend WithEvents mnuUpdatePools As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkAlertRebootAntsOnHashAlert As System.Windows.Forms.CheckBox
    Friend WithEvents tabS3Alerts As System.Windows.Forms.TabPage
    Friend WithEvents chkAlertIfS3FanLow As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS3FanLow As System.Windows.Forms.TextBox
    Friend WithEvents cmdSaveAlerts5 As System.Windows.Forms.Button
    Friend WithEvents chkAlertIfS3XCount As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS3XCount As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfS3FanHigh As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertIfS3Hash As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS3Temp As System.Windows.Forms.TextBox
    Friend WithEvents txtAlertS3Hash As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfS3Temp As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS3FanHigh As System.Windows.Forms.TextBox
    Friend WithEvents timerDoStuff As System.Windows.Forms.Timer
    Friend WithEvents cmdChangeThreads As System.Windows.Forms.Button
    Friend WithEvents trackThreadCount As System.Windows.Forms.TrackBar
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtDisplayRefreshInSecs As System.Windows.Forms.TextBox
    Friend WithEvents chkRebootAntOnError As System.Windows.Forms.CheckBox
    Friend WithEvents tabMinerConfig As System.Windows.Forms.TabPage
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtIPRangeToScan As System.Windows.Forms.TextBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents cmdScan As System.Windows.Forms.Button
    Friend WithEvents dataAntConfig As System.Windows.Forms.DataGridView
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents chkAntRebootViaSSH As System.Windows.Forms.CheckBox
    Friend WithEvents chkAntUseAPI As System.Windows.Forms.CheckBox
    Friend WithEvents chkAntActive As System.Windows.Forms.CheckBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtAntName As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents lblMinerID As System.Windows.Forms.Label
    Friend WithEvents txtAntSSHPort As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents txtAntWebPort As System.Windows.Forms.TextBox
    Friend WithEvents txtAntAPIPort As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents cmdAntClear As System.Windows.Forms.Button
    Friend WithEvents cmbAntScanStop As System.Windows.Forms.ComboBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents cmbAntScanStart As System.Windows.Forms.ComboBox
    Friend WithEvents mnuRemoveAnt As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabReboots As System.Windows.Forms.TabPage
    Friend WithEvents cmbRebootAllAntsAtAlso As System.Windows.Forms.ComboBox
    Friend WithEvents txtRebootAllAntsAtAlso As System.Windows.Forms.TextBox
    Friend WithEvents cmbRebootAllAntsAt As System.Windows.Forms.ComboBox
    Friend WithEvents txtRebootAllAntsAt As System.Windows.Forms.TextBox
    Friend WithEvents txtRebootAntsByUptime As System.Windows.Forms.TextBox
    Friend WithEvents cmbRebootAntsByUptime As System.Windows.Forms.ComboBox
    Friend WithEvents chkRebootAllAntsAtAlso As System.Windows.Forms.CheckBox
    Friend WithEvents chkRebootAllAntsAt As System.Windows.Forms.CheckBox
    Friend WithEvents chkRebootAntsByUptime As System.Windows.Forms.CheckBox
    Friend WithEvents cmdSaveReboots As System.Windows.Forms.Button
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents tabC1Alerts As System.Windows.Forms.TabPage
    Friend WithEvents chkAlertIfC1FanLow As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertC1FanLow As System.Windows.Forms.TextBox
    Friend WithEvents cmdSaveAlerts6 As System.Windows.Forms.Button
    Friend WithEvents chkAlertIfC1XCount As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertC1XCount As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfC1Temp As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertIfC1Hash As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertC1FanHigh As System.Windows.Forms.TextBox
    Friend WithEvents txtAlertC1Hash As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfC1FanHigh As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertC1Temp As System.Windows.Forms.TextBox
    Friend WithEvents txtIPToGetInfo As System.Windows.Forms.TextBox
    Friend WithEvents cmdGetMinerInfo As System.Windows.Forms.Button
    Friend WithEvents tabS4Alerts As System.Windows.Forms.TabPage
    Friend WithEvents tabSPAlerts As System.Windows.Forms.TabPage
    Friend WithEvents chkAlertIfS4FanLow As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS4FanLow As System.Windows.Forms.TextBox
    Friend WithEvents cmdSaveAlerts7 As System.Windows.Forms.Button
    Friend WithEvents chkAlertIfS4XCount As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS4XCount As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfS4FanHigh As System.Windows.Forms.CheckBox
    Friend WithEvents chkAlertIfS4Hash As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS4Temp As System.Windows.Forms.TextBox
    Friend WithEvents txtAlertS4Hash As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfS4Temp As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertS4FanHigh As System.Windows.Forms.TextBox
    Friend WithEvents cmdSaveAlerts8 As System.Windows.Forms.Button
    Friend WithEvents chkAlertIfSPHash As System.Windows.Forms.CheckBox
    Friend WithEvents txtAlertSPTemp As System.Windows.Forms.TextBox
    Friend WithEvents txtAlertSPHash As System.Windows.Forms.TextBox
    Friend WithEvents chkAlertIfSPTemp As System.Windows.Forms.CheckBox
    Friend WithEvents cmbMinerType As System.Windows.Forms.ComboBox

End Class
