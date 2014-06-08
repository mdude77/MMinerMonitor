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
        Me.cmdPause = New System.Windows.Forms.Button()
        Me.TimerWatchdog = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtPleaseSupport = New System.Windows.Forms.TextBox()
        CType(Me.dataAnts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.tabAnts.SuspendLayout()
        Me.tabConfig.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.tabLog.SuspendLayout()
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
        Me.dataAnts.Size = New System.Drawing.Size(1139, 284)
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
        'frmAntMonitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1043, 375)
        Me.Controls.Add(Me.txtPleaseSupport)
        Me.Controls.Add(Me.cmdPause)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.cmdRefresh)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.Name = "frmAntMonitor"
        Me.Text = "M's Ant Monitor v1.6"
        CType(Me.dataAnts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.tabAnts.ResumeLayout(False)
        Me.tabConfig.ResumeLayout(False)
        Me.tabConfig.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tabLog.ResumeLayout(False)
        Me.tabLog.PerformLayout()
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

End Class
