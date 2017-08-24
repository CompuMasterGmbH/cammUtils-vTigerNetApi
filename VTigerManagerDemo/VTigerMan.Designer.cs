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
            this.MainPanel = new System.Windows.Forms.Panel();
            this.dataView = new VTigerUserControls.KeyValueDataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.LQueryTable = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.EdQuery = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.EdOrderBy = new System.Windows.Forms.ToolStripComboBox();
            this.BtnQuery = new System.Windows.Forms.ToolStripButton();
            this.BtnSwitchTableStyle = new System.Windows.Forms.ToolStrip();
            this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
            this.BtnNew = new System.Windows.Forms.ToolStripButton();
            this.BtnUpdate = new System.Windows.Forms.ToolStripButton();
            this.BtnDelete = new System.Windows.Forms.ToolStripButton();
            this.BtnViewMode = new System.Windows.Forms.ToolStripButton();
            this.BtnPageFirst = new System.Windows.Forms.ToolStripButton();
            this.BtnPagePrev = new System.Windows.Forms.ToolStripButton();
            this.LPage = new System.Windows.Forms.ToolStripLabel();
            this.BtnPageNext = new System.Windows.Forms.ToolStripButton();
            this.BtnPageLast = new System.Windows.Forms.ToolStripButton();
            this.BtnExportTable = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.textBoxSessionID = new System.Windows.Forms.ToolStripTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tableList = new System.Windows.Forms.TreeView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.loadThread = new System.ComponentModel.BackgroundWorker();
            this.CompareLocalVsRemoteTableButton = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataView)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.BtnSwitchTableStyle.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.newElementToolStripMenuItem});
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
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loginToolStripMenuItem.Text = "Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Visible = false;
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // newElementToolStripMenuItem
            // 
            this.newElementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemNewContact,
            this.calendarToolStripMenuItem});
            this.newElementToolStripMenuItem.Name = "newElementToolStripMenuItem";
            this.newElementToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.newElementToolStripMenuItem.Text = "New element";
            // 
            // MenuItemNewContact
            // 
            this.MenuItemNewContact.Name = "MenuItemNewContact";
            this.MenuItemNewContact.Size = new System.Drawing.Size(121, 22);
            this.MenuItemNewContact.Text = "Contact";
            this.MenuItemNewContact.Click += new System.EventHandler(this.MenuItemNewContact_Click);
            // 
            // calendarToolStripMenuItem
            // 
            this.calendarToolStripMenuItem.Name = "calendarToolStripMenuItem";
            this.calendarToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.calendarToolStripMenuItem.Text = "Calendar";
            this.calendarToolStripMenuItem.Click += new System.EventHandler(this.calendarToolStripMenuItem_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPanel.Controls.Add(this.dataView);
            this.MainPanel.Controls.Add(this.toolStrip1);
            this.MainPanel.Controls.Add(this.BtnSwitchTableStyle);
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
            this.dataView.Location = new System.Drawing.Point(159, 70);
            this.dataView.Name = "dataView";
            this.dataView.RowHeadersVisible = false;
            this.dataView.ShowKeyValueTable = false;
            this.dataView.Size = new System.Drawing.Size(785, 336);
            this.dataView.TabIndex = 8;
            this.dataView.DataSourceChanged += new System.EventHandler(this.dataView_DataSourceChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.LQueryTable,
            this.toolStripLabel2,
            this.EdQuery,
            this.toolStripLabel3,
            this.EdOrderBy,
            this.BtnQuery});
            this.toolStrip1.Location = new System.Drawing.Point(159, 45);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(785, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(90, 22);
            this.toolStripLabel1.Text = "SELECT * FROM";
            // 
            // LQueryTable
            // 
            this.LQueryTable.Name = "LQueryTable";
            this.LQueryTable.Size = new System.Drawing.Size(51, 22);
            this.LQueryTable.Text = "<Table>";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(46, 22);
            this.toolStripLabel2.Text = "WHERE";
            // 
            // EdQuery
            // 
            this.EdQuery.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.EdQuery.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.EdQuery.Name = "EdQuery";
            this.EdQuery.Size = new System.Drawing.Size(150, 25);
            this.EdQuery.Text = "firstname like \'m%\'";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(61, 22);
            this.toolStripLabel3.Text = "ORDER BY";
            // 
            // EdOrderBy
            // 
            this.EdOrderBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.EdOrderBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.EdOrderBy.Name = "EdOrderBy";
            this.EdOrderBy.Size = new System.Drawing.Size(80, 25);
            this.EdOrderBy.Text = "id";
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
            // BtnSwitchTableStyle
            // 
            this.BtnSwitchTableStyle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnRefresh,
            this.BtnNew,
            this.BtnUpdate,
            this.BtnDelete,
            this.BtnViewMode,
            this.BtnPageFirst,
            this.BtnPagePrev,
            this.LPage,
            this.BtnPageNext,
            this.BtnPageLast,
            this.BtnExportTable,
            this.toolStripButton2,
            this.textBoxSessionID,
            this.CompareLocalVsRemoteTableButton});
            this.BtnSwitchTableStyle.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.BtnSwitchTableStyle.Location = new System.Drawing.Point(159, 0);
            this.BtnSwitchTableStyle.Name = "BtnSwitchTableStyle";
            this.BtnSwitchTableStyle.Size = new System.Drawing.Size(785, 45);
            this.BtnSwitchTableStyle.TabIndex = 6;
            this.BtnSwitchTableStyle.Text = "Switch table-style";
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
            // LPage
            // 
            this.LPage.Name = "LPage";
            this.LPage.Size = new System.Drawing.Size(58, 15);
            this.LPage.Text = "Page 1 / ?";
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
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(85, 19);
            this.toolStripButton2.Text = "Describe table";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
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
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.BtnSwitchTableStyle.ResumeLayout(false);
            this.BtnSwitchTableStyle.PerformLayout();
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
        private System.Windows.Forms.ToolStrip BtnSwitchTableStyle;
        private System.Windows.Forms.ToolStripButton BtnRefresh;
        private System.Windows.Forms.ToolStripButton BtnNew;
        private System.Windows.Forms.ToolStripButton BtnUpdate;
        private System.Windows.Forms.ToolStripButton BtnDelete;
        private System.Windows.Forms.ToolStripButton BtnViewMode;
        private System.Windows.Forms.ToolStripButton BtnPageFirst;
        private System.Windows.Forms.ToolStripButton BtnPagePrev;
        private System.Windows.Forms.ToolStripLabel LPage;
        private System.Windows.Forms.ToolStripButton BtnPageNext;
        private System.Windows.Forms.ToolStripButton BtnPageLast;
        private System.Windows.Forms.TreeView tableList;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox EdQuery;
        private System.Windows.Forms.ToolStripButton BtnQuery;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel LQueryTable;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private VTigerUserControls.KeyValueDataGridView dataView;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox EdOrderBy;
        private System.Windows.Forms.ToolStripButton BtnExportTable;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripTextBox textBoxSessionID;
        private System.Windows.Forms.ToolStripButton CompareLocalVsRemoteTableButton;
    }
}

