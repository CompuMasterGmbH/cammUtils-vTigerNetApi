namespace VTigerManager
{
    partial class VTigerMan
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VTigerMan));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemNewContact = new System.Windows.Forms.ToolStripMenuItem();
            this.calendarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorMenuNewElementBulkInsert = new System.Windows.Forms.ToolStripSeparator();
            this.newRecordForEveryTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bulkInsert1500ContactRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.dataView = new VTigerUserControls.KeyValueDataGridView();
            this.toolStripQueryOptions = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelSelectFrom = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabelQueryTable = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabelWhereClause = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextboxQueryWhereClause = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelOrderBy = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboboxOrderBy = new System.Windows.Forms.ToolStripComboBox();
            this.BtnQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripSwitchTableStyle = new System.Windows.Forms.ToolStrip();
            this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
            this.BtnNew = new System.Windows.Forms.ToolStripButton();
            this.BtnUpdate = new System.Windows.Forms.ToolStripButton();
            this.BtnDelete = new System.Windows.Forms.ToolStripButton();
            this.BtnViewMode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorPaging = new System.Windows.Forms.ToolStripSeparator();
            this.BtnPageFirst = new System.Windows.Forms.ToolStripButton();
            this.BtnPagePrev = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelPage = new System.Windows.Forms.ToolStripLabel();
            this.BtnPageNext = new System.Windows.Forms.ToolStripButton();
            this.BtnPageLast = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelPagingSize = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxPageSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparatorExport = new System.Windows.Forms.ToolStripSeparator();
            this.BtnExportTable = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorMetaInfos = new System.Windows.Forms.ToolStripSeparator();
            this.CompareLocalVsRemoteTableButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTableDescription = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorServerSession = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelSessionID = new System.Windows.Forms.ToolStripLabel();
            this.textBoxSessionID = new System.Windows.Forms.ToolStripTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tableList = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.loadThread = new System.ComponentModel.BackgroundWorker();
            this.testsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryFromAllRemoteTablesWithoutErrorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
            this.toolStripQueryOptions.SuspendLayout();
            this.toolStripSwitchTableStyle.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.newElementToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.testsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(944, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.logoutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.loginToolStripMenuItem.Text = "Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Visible = false;
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // newElementToolStripMenuItem
            // 
            this.newElementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemNewContact,
            this.calendarToolStripMenuItem,
            this.toolStripSeparatorMenuNewElementBulkInsert,
            this.newRecordForEveryTypeToolStripMenuItem,
            this.bulkInsert1500ContactRecordsToolStripMenuItem});
            this.newElementToolStripMenuItem.Name = "newElementToolStripMenuItem";
            this.newElementToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.newElementToolStripMenuItem.Text = "&New element";
            // 
            // MenuItemNewContact
            // 
            this.MenuItemNewContact.Name = "MenuItemNewContact";
            this.MenuItemNewContact.Size = new System.Drawing.Size(269, 22);
            this.MenuItemNewContact.Text = "Contact";
            this.MenuItemNewContact.Click += new System.EventHandler(this.MenuItemNewContact_Click);
            // 
            // calendarToolStripMenuItem
            // 
            this.calendarToolStripMenuItem.Name = "calendarToolStripMenuItem";
            this.calendarToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.calendarToolStripMenuItem.Text = "Calendar";
            this.calendarToolStripMenuItem.Click += new System.EventHandler(this.calendarToolStripMenuItem_Click);
            // 
            // toolStripSeparatorMenuNewElementBulkInsert
            // 
            this.toolStripSeparatorMenuNewElementBulkInsert.Name = "toolStripSeparatorMenuNewElementBulkInsert";
            this.toolStripSeparatorMenuNewElementBulkInsert.Size = new System.Drawing.Size(266, 6);
            // 
            // newRecordForEveryTypeToolStripMenuItem
            // 
            this.newRecordForEveryTypeToolStripMenuItem.Name = "newRecordForEveryTypeToolStripMenuItem";
            this.newRecordForEveryTypeToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.newRecordForEveryTypeToolStripMenuItem.Text = "Bulk insert: new record for every type";
            this.newRecordForEveryTypeToolStripMenuItem.Click += new System.EventHandler(this.newRecordForEveryTypeToolStripMenuItem_Click);
            // 
            // bulkInsert1500ContactRecordsToolStripMenuItem
            // 
            this.bulkInsert1500ContactRecordsToolStripMenuItem.Name = "bulkInsert1500ContactRecordsToolStripMenuItem";
            this.bulkInsert1500ContactRecordsToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.bulkInsert1500ContactRecordsToolStripMenuItem.Text = "Bulk insert: 1500 contact records";
            this.bulkInsert1500ContactRecordsToolStripMenuItem.Click += new System.EventHandler(this.bulkInsert1500ContactRecordsToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem
            // 
            this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Checked = true;
            this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Name = "ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem";
            this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Size = new System.Drawing.Size(306, 22);
            this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Text = "Ignore SSL certificate errors of remote server";
            this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Click += new System.EventHandler(this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPanel.Controls.Add(this.dataView);
            this.MainPanel.Controls.Add(this.toolStripQueryOptions);
            this.MainPanel.Controls.Add(this.toolStripSwitchTableStyle);
            this.MainPanel.Controls.Add(this.splitter1);
            this.MainPanel.Controls.Add(this.tableList);
            this.MainPanel.Enabled = false;
            this.MainPanel.Location = new System.Drawing.Point(0, 24);
            this.MainPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(944, 406);
            this.MainPanel.TabIndex = 1;
            // 
            // dataView
            // 
            this.dataView.AllowUserToAddRows = false;
            this.dataView.AllowUserToDeleteRows = false;
            this.dataView.AllowUserToResizeColumns = false;
            this.dataView.AllowUserToResizeRows = false;
            this.dataView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataView.Location = new System.Drawing.Point(159, 71);
            this.dataView.Name = "dataView";
            this.dataView.RowHeadersVisible = false;
            this.dataView.ShowKeyValueTable = false;
            this.dataView.Size = new System.Drawing.Size(785, 335);
            this.dataView.TabIndex = 8;
            this.dataView.DataSourceChanged += new System.EventHandler(this.dataView_DataSourceChanged);
            // 
            // toolStripQueryOptions
            // 
            this.toolStripQueryOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelSelectFrom,
            this.toolStripLabelQueryTable,
            this.toolStripLabelWhereClause,
            this.toolStripTextboxQueryWhereClause,
            this.toolStripLabelOrderBy,
            this.toolStripComboboxOrderBy,
            this.BtnQuery});
            this.toolStripQueryOptions.Location = new System.Drawing.Point(159, 46);
            this.toolStripQueryOptions.Name = "toolStripQueryOptions";
            this.toolStripQueryOptions.Size = new System.Drawing.Size(785, 25);
            this.toolStripQueryOptions.TabIndex = 7;
            this.toolStripQueryOptions.Text = "toolStrip1";
            // 
            // toolStripLabelSelectFrom
            // 
            this.toolStripLabelSelectFrom.Name = "toolStripLabelSelectFrom";
            this.toolStripLabelSelectFrom.Size = new System.Drawing.Size(90, 22);
            this.toolStripLabelSelectFrom.Text = "SELECT * FROM";
            // 
            // toolStripLabelQueryTable
            // 
            this.toolStripLabelQueryTable.Name = "toolStripLabelQueryTable";
            this.toolStripLabelQueryTable.Size = new System.Drawing.Size(51, 22);
            this.toolStripLabelQueryTable.Text = "<Table>";
            // 
            // toolStripLabelWhereClause
            // 
            this.toolStripLabelWhereClause.Name = "toolStripLabelWhereClause";
            this.toolStripLabelWhereClause.Size = new System.Drawing.Size(46, 22);
            this.toolStripLabelWhereClause.Text = "WHERE";
            // 
            // toolStripTextboxQueryWhereClause
            // 
            this.toolStripTextboxQueryWhereClause.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.toolStripTextboxQueryWhereClause.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.toolStripTextboxQueryWhereClause.Name = "toolStripTextboxQueryWhereClause";
            this.toolStripTextboxQueryWhereClause.Size = new System.Drawing.Size(150, 25);
            this.toolStripTextboxQueryWhereClause.Text = "firstname like \'m%\'";
            // 
            // toolStripLabelOrderBy
            // 
            this.toolStripLabelOrderBy.Name = "toolStripLabelOrderBy";
            this.toolStripLabelOrderBy.Size = new System.Drawing.Size(61, 22);
            this.toolStripLabelOrderBy.Text = "ORDER BY";
            // 
            // toolStripComboboxOrderBy
            // 
            this.toolStripComboboxOrderBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.toolStripComboboxOrderBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.toolStripComboboxOrderBy.Name = "toolStripComboboxOrderBy";
            this.toolStripComboboxOrderBy.Size = new System.Drawing.Size(80, 25);
            this.toolStripComboboxOrderBy.Text = "id";
            // 
            // BtnQuery
            // 
            this.BtnQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnQuery.Image = ((System.Drawing.Image)(resources.GetObject("BtnQuery.Image")));
            this.BtnQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnQuery.Name = "BtnQuery";
            this.BtnQuery.Size = new System.Drawing.Size(43, 22);
            this.BtnQuery.Text = "Query";
            this.BtnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // toolStripSwitchTableStyle
            // 
            this.toolStripSwitchTableStyle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnRefresh,
            this.BtnNew,
            this.BtnUpdate,
            this.BtnDelete,
            this.BtnViewMode,
            this.toolStripSeparatorPaging,
            this.BtnPageFirst,
            this.BtnPagePrev,
            this.toolStripLabelPage,
            this.BtnPageNext,
            this.BtnPageLast,
            this.toolStripLabelPagingSize,
            this.toolStripComboBoxPageSize,
            this.toolStripSeparatorExport,
            this.BtnExportTable,
            this.toolStripSeparatorMetaInfos,
            this.CompareLocalVsRemoteTableButton,
            this.toolStripButtonTableDescription,
            this.toolStripSeparatorServerSession,
            this.toolStripLabelSessionID,
            this.textBoxSessionID});
            this.toolStripSwitchTableStyle.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripSwitchTableStyle.Location = new System.Drawing.Point(159, 0);
            this.toolStripSwitchTableStyle.Name = "toolStripSwitchTableStyle";
            this.toolStripSwitchTableStyle.Size = new System.Drawing.Size(785, 46);
            this.toolStripSwitchTableStyle.TabIndex = 6;
            this.toolStripSwitchTableStyle.Text = "Switch table-style";
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("BtnRefresh.Image")));
            this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(50, 19);
            this.BtnRefresh.Text = "Refresh";
            this.BtnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // BtnNew
            // 
            this.BtnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnNew.Image = ((System.Drawing.Image)(resources.GetObject("BtnNew.Image")));
            this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnNew.Name = "BtnNew";
            this.BtnNew.Size = new System.Drawing.Size(35, 19);
            this.BtnNew.Text = "New";
            this.BtnNew.Click += new System.EventHandler(this.BtnNew_Click);
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("BtnUpdate.Image")));
            this.BtnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(49, 19);
            this.BtnUpdate.Text = "Update";
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnDelete.Image = ((System.Drawing.Image)(resources.GetObject("BtnDelete.Image")));
            this.BtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(44, 19);
            this.BtnDelete.Text = "Delete";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnViewMode
            // 
            this.BtnViewMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnViewMode.Image = ((System.Drawing.Image)(resources.GetObject("BtnViewMode.Image")));
            this.BtnViewMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnViewMode.Name = "BtnViewMode";
            this.BtnViewMode.Size = new System.Drawing.Size(115, 19);
            this.BtnViewMode.Text = "Change view-mode";
            this.BtnViewMode.Click += new System.EventHandler(this.BtnViewMode_Click);
            // 
            // toolStripSeparatorPaging
            // 
            this.toolStripSeparatorPaging.Name = "toolStripSeparatorPaging";
            this.toolStripSeparatorPaging.Size = new System.Drawing.Size(6, 23);
            // 
            // BtnPageFirst
            // 
            this.BtnPageFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnPageFirst.Image = ((System.Drawing.Image)(resources.GetObject("BtnPageFirst.Image")));
            this.BtnPageFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnPageFirst.Name = "BtnPageFirst";
            this.BtnPageFirst.Size = new System.Drawing.Size(27, 19);
            this.BtnPageFirst.Text = "<<";
            this.BtnPageFirst.Click += new System.EventHandler(this.BtnPageFirst_Click);
            // 
            // BtnPagePrev
            // 
            this.BtnPagePrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnPagePrev.Image = ((System.Drawing.Image)(resources.GetObject("BtnPagePrev.Image")));
            this.BtnPagePrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnPagePrev.Name = "BtnPagePrev";
            this.BtnPagePrev.Size = new System.Drawing.Size(23, 19);
            this.BtnPagePrev.Text = "<";
            this.BtnPagePrev.Click += new System.EventHandler(this.BtnPagePrev_Click);
            // 
            // toolStripLabelPage
            // 
            this.toolStripLabelPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabelPage.Name = "toolStripLabelPage";
            this.toolStripLabelPage.Size = new System.Drawing.Size(58, 15);
            this.toolStripLabelPage.Text = "Page 1 / ?";
            // 
            // BtnPageNext
            // 
            this.BtnPageNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnPageNext.Image = ((System.Drawing.Image)(resources.GetObject("BtnPageNext.Image")));
            this.BtnPageNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnPageNext.Name = "BtnPageNext";
            this.BtnPageNext.Size = new System.Drawing.Size(23, 19);
            this.BtnPageNext.Text = ">";
            this.BtnPageNext.Click += new System.EventHandler(this.BtnPageNext_Click);
            // 
            // BtnPageLast
            // 
            this.BtnPageLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnPageLast.Enabled = false;
            this.BtnPageLast.Image = ((System.Drawing.Image)(resources.GetObject("BtnPageLast.Image")));
            this.BtnPageLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnPageLast.Name = "BtnPageLast";
            this.BtnPageLast.Size = new System.Drawing.Size(27, 19);
            this.BtnPageLast.Text = ">>";
            // 
            // toolStripLabelPagingSize
            // 
            this.toolStripLabelPagingSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabelPagingSize.Name = "toolStripLabelPagingSize";
            this.toolStripLabelPagingSize.Size = new System.Drawing.Size(60, 15);
            this.toolStripLabelPagingSize.Text = "Page limit";
            // 
            // toolStripComboBoxPageSize
            // 
            this.toolStripComboBoxPageSize.Items.AddRange(new object[] {
            "5",
            "10",
            "50",
            "100",
            "1000",
            "10000",
            "∞"});
            this.toolStripComboBoxPageSize.Name = "toolStripComboBoxPageSize";
            this.toolStripComboBoxPageSize.Size = new System.Drawing.Size(75, 23);
            this.toolStripComboBoxPageSize.Text = "20";
            this.toolStripComboBoxPageSize.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxPageSize_SelectedIndexChanged);
            // 
            // toolStripSeparatorExport
            // 
            this.toolStripSeparatorExport.Name = "toolStripSeparatorExport";
            this.toolStripSeparatorExport.Size = new System.Drawing.Size(6, 23);
            // 
            // BtnExportTable
            // 
            this.BtnExportTable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BtnExportTable.Image = ((System.Drawing.Image)(resources.GetObject("BtnExportTable.Image")));
            this.BtnExportTable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnExportTable.Name = "BtnExportTable";
            this.BtnExportTable.Size = new System.Drawing.Size(44, 19);
            this.BtnExportTable.Text = "Export";
            this.BtnExportTable.Click += new System.EventHandler(this.BtnExportTable_Click);
            // 
            // toolStripSeparatorMetaInfos
            // 
            this.toolStripSeparatorMetaInfos.Name = "toolStripSeparatorMetaInfos";
            this.toolStripSeparatorMetaInfos.Size = new System.Drawing.Size(6, 23);
            // 
            // CompareLocalVsRemoteTableButton
            // 
            this.CompareLocalVsRemoteTableButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.CompareLocalVsRemoteTableButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CompareLocalVsRemoteTableButton.Name = "CompareLocalVsRemoteTableButton";
            this.CompareLocalVsRemoteTableButton.Size = new System.Drawing.Size(172, 19);
            this.CompareLocalVsRemoteTableButton.Text = "Compare local vs remote table";
            this.CompareLocalVsRemoteTableButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.CompareLocalVsRemoteTableButton.Click += new System.EventHandler(this.CompareLocalVsRemoteTableButton_Click);
            // 
            // toolStripButtonTableDescription
            // 
            this.toolStripButtonTableDescription.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonTableDescription.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTableDescription.Image")));
            this.toolStripButtonTableDescription.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTableDescription.Name = "toolStripButtonTableDescription";
            this.toolStripButtonTableDescription.Size = new System.Drawing.Size(85, 19);
            this.toolStripButtonTableDescription.Text = "Describe table";
            this.toolStripButtonTableDescription.Click += new System.EventHandler(this.toolStripButtonTableDescription_Click);
            // 
            // toolStripSeparatorServerSession
            // 
            this.toolStripSeparatorServerSession.Name = "toolStripSeparatorServerSession";
            this.toolStripSeparatorServerSession.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripLabelSessionID
            // 
            this.toolStripLabelSessionID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabelSessionID.Name = "toolStripLabelSessionID";
            this.toolStripLabelSessionID.Size = new System.Drawing.Size(89, 15);
            this.toolStripLabelSessionID.Text = "Remote session";
            // 
            // textBoxSessionID
            // 
            this.textBoxSessionID.Name = "textBoxSessionID";
            this.textBoxSessionID.Size = new System.Drawing.Size(100, 23);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(156, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 406);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // tableList
            // 
            this.tableList.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableList.Location = new System.Drawing.Point(0, 0);
            this.tableList.Name = "tableList";
            this.tableList.Size = new System.Drawing.Size(156, 406);
            this.tableList.TabIndex = 0;
            this.tableList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 430);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(944, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(10, 17);
            this.StatusLabel.Text = " ";
            // 
            // testsToolStripMenuItem
            // 
            this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queryFromAllRemoteTablesWithoutErrorsToolStripMenuItem});
            this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
            this.testsToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.testsToolStripMenuItem.Text = "Tests";
            // 
            // queryFromAllRemoteTablesWithoutErrorsToolStripMenuItem
            // 
            this.queryFromAllRemoteTablesWithoutErrorsToolStripMenuItem.Name = "queryFromAllRemoteTablesWithoutErrorsToolStripMenuItem";
            this.queryFromAllRemoteTablesWithoutErrorsToolStripMenuItem.Size = new System.Drawing.Size(302, 22);
            this.queryFromAllRemoteTablesWithoutErrorsToolStripMenuItem.Text = "Query from all remote tables without errors";
            this.queryFromAllRemoteTablesWithoutErrorsToolStripMenuItem.Click += new System.EventHandler(this.queryFromAllRemoteTablesWithoutErrorsToolStripMenuItem_Click);
            // 
            // VTigerMan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 452);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "VTigerMan";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VTigerMan_FormClosed);
            this.Load += new System.EventHandler(this.VTigerMan_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).EndInit();
            this.toolStripQueryOptions.ResumeLayout(false);
            this.toolStripQueryOptions.PerformLayout();
            this.toolStripSwitchTableStyle.ResumeLayout(false);
            this.toolStripSwitchTableStyle.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.Splitter splitter1;
        private System.ComponentModel.BackgroundWorker loadThread;
        private System.Windows.Forms.ToolStripMenuItem newElementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuItemNewContact;
        private System.Windows.Forms.ToolStripMenuItem calendarToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStripSwitchTableStyle;
        private System.Windows.Forms.ToolStripButton BtnRefresh;
        private System.Windows.Forms.ToolStripButton BtnNew;
        private System.Windows.Forms.ToolStripButton BtnUpdate;
        private System.Windows.Forms.ToolStripButton BtnDelete;
        private System.Windows.Forms.ToolStripButton BtnViewMode;
        private System.Windows.Forms.ToolStripButton BtnPageFirst;
        private System.Windows.Forms.ToolStripButton BtnPagePrev;
        private System.Windows.Forms.ToolStripLabel toolStripLabelPage;
        private System.Windows.Forms.ToolStripButton BtnPageNext;
        private System.Windows.Forms.ToolStripButton BtnPageLast;
        private System.Windows.Forms.TreeView tableList;
        private System.Windows.Forms.ToolStrip toolStripQueryOptions;
        private System.Windows.Forms.ToolStripTextBox toolStripTextboxQueryWhereClause;
        private System.Windows.Forms.ToolStripButton BtnQuery;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSelectFrom;
        private System.Windows.Forms.ToolStripLabel toolStripLabelQueryTable;
        private System.Windows.Forms.ToolStripLabel toolStripLabelWhereClause;
        private VTigerUserControls.KeyValueDataGridView dataView;
        private System.Windows.Forms.ToolStripLabel toolStripLabelOrderBy;
        private System.Windows.Forms.ToolStripComboBox toolStripComboboxOrderBy;
        private System.Windows.Forms.ToolStripButton BtnExportTable;
        private System.Windows.Forms.ToolStripButton toolStripButtonTableDescription;
        private System.Windows.Forms.ToolStripTextBox textBoxSessionID;
        private System.Windows.Forms.ToolStripButton CompareLocalVsRemoteTableButton;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorMenuNewElementBulkInsert;
        private System.Windows.Forms.ToolStripMenuItem newRecordForEveryTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel toolStripLabelPagingSize;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxPageSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorExport;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSessionID;
        private System.Windows.Forms.ToolStripMenuItem bulkInsert1500ContactRecordsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorPaging;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorMetaInfos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorServerSession;
        private System.Windows.Forms.ToolStripMenuItem testsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryFromAllRemoteTablesWithoutErrorsToolStripMenuItem;
    }
}

