﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using VTigerApi;
using System.IO;

namespace VTigerManager
{
    public partial class VTigerMan : Form
    {
        public bool closed = false;
        private VTiger api;

        private int currentPage;
        private string currentTable;
        private int pageLimit;

        private bool editMode;
        private bool EditMode
        {
            get
            {
                return editMode;
            }
            set
            {
                if (editMode != value)
                {
                    editMode = value;
                    //if (value)
                    //dataView.ShowKeyValueTable = editMode;
                }
            }
        }
        private bool changed;
        private bool creatingEntry;

        public VTigerMan()
        {
            InitializeComponent();
        }

        private void VTigerMan_Load(object sender, EventArgs e)
        {
            this.toolStripComboBoxPageSize.Text = AssignNewPagingSize((string)Properties.Settings.Default["PagingSize"]);
            FormTitle();
            VTiger.IgnoreSslCertificateErrors = this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Checked;
            LoginToolStripMenuItem_Click(null, null);
            ShowData(null);
        }

        private void ToolStripButtonTableDescription_Click(object sender, EventArgs e)
        {
            try
            {
                ShowTablesMetaData(api.Describe_DataTable(api.RemoteTables[currentTable].ElementType));
            }
            catch (VTigerApiSessionTimedOutException ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server session timeout error: " + ex.Message;
                this.LoginToolStripMenuItem_Click(null, null);
            }
            catch (VTigerApiException ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server error: " + ex.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
            }
        }

        #region Events

        private void VTigerMan_FormClosed(object sender, FormClosedEventArgs e)
        {
            closed = true;
        }

        //========== mainMenu ==========

        private void FillRemoteTableList()
        {
            //fill remote table list
            List<string> zlk = new List<string>(api.RemoteTables.Keys); // Create a list of keys
            zlk.Sort(); // and then sort it.
            tableList.Nodes.Clear();
            foreach (string remoteTable in zlk)
            {
                tableList.Nodes.Add(remoteTable);
            }
        }

        public void Login(string Url, string Username, string AccessKey)
        {
            try
            {
                if (api != null)
                    try { api.Logout(); }
                    catch { }

                StatusLabel.Text = "Logging in...";
                this.Refresh();

                api = new VTiger(Url);
                api.Login(Username, AccessKey);

                StatusLabel.Text = "Successfully logged in";
                MainPanel.Enabled = true;
                logoutToolStripMenuItem.Visible = true;
                loginToolStripMenuItem.Visible = false;
                textBoxSessionID.Text = api.SessionName;
                FillRemoteTableList();
            }
            catch (Exception ex)
            {
                if (api != null)
                    try { api.Logout(); }
                    catch { }
                api = null;
                MainPanel.Enabled = false;
                logoutToolStripMenuItem.Visible = false;
                loginToolStripMenuItem.Visible = true;
                tableList.Nodes.Clear();
                textBoxSessionID.Text = "N/A";
                StatusLabel.Text = "Failed to login: " + ex.Message;
                this.Refresh();
                System.Windows.Forms.MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            FormTitle();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (LoginWindow login = new LoginWindow())
            {
                if (login.ShowDialog(this) == DialogResult.OK)
                {
                    Login(login.EdServiceUrl.Text, login.EdUsername.Text, login.EdAuthKey.Text);
                }
            }
        }

        /// <summary>
        /// Update the form's title
        /// </summary>
        /// <param name="instanceDescription"></param>
        private void FormTitle()
        {
            if (api == null || api.SessionName == null || api.SessionName == "")
            {
                this.Text = this.Text = "VTiger Demo";
            }
            else
            {
                this.Text = this.Text = "VTiger Demo - connected as user ID " + api.UserID + " to VTiger V" + api.VTigerVersion.ToString() + " at " + api.ServiceUrl;
            }
        }

        private void LogoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                api.Logout();
            }
            catch { }
            api = null;
            MainPanel.Enabled = false;
            logoutToolStripMenuItem.Visible = false;
            loginToolStripMenuItem.Visible = true;
            dataView.DataSource = null;
            ShowData(null);
            textBoxSessionID.Text = "N/A";
            tableList.Nodes.Clear();
            FormTitle();
        }

        //========== treeView1 ==========

        #endregion

        #region Data display

        //public bool LoadTreeNode(TreeNode node)
        //{
        //    try
        //    {
        //        node.Nodes.Insert(0, "Loading...");
        //        node.Expand();
        //        StatusLabel.Text = "Loading nodes...";
        //        this.Refresh();

        //        VTiger.TitleFields titleField;
        //        try { titleField = api.RemoteTables[node.Text]; }
        //        catch { titleField = new VTiger.TitleFields(); }

        //        string formatString;
        //        string query;
        //        if (titleField.DefaultTitleField1 != null)
        //            if (titleField.DefaultTitleField2 != null)
        //            {
        //                formatString = "[{0}] {1} - {2}";
        //                query = String.Format("select id,{1},{2} from {0};", node.Text, titleField.DefaultTitleField1, titleField.DefaultTitleField2);
        //                DataTable dt = api.Query(query);
        //                node.Nodes.Clear();
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    string id = (string)dr["id"];
        //                    node.Nodes.Add(id, String.Format(
        //                        formatString, id, (string)dr[titleField.DefaultTitleField1], (string)dr[titleField.DefaultTitleField2]));
        //                }
        //            }
        //            else
        //            {
        //                formatString = "[{0}] {1}";
        //                query = String.Format("select id,{1} from {0};", node.Text, titleField.DefaultTitleField1);
        //                DataTable dt = api.Query(query);
        //                node.Nodes.Clear();
        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    string id = (string)dr["id"];
        //                    node.Nodes.Add(id, String.Format(
        //                        formatString, id, (string)dr[titleField.DefaultTitleField1]));
        //                }
        //            }
        //        else
        //        {
        //            formatString = "[{0}]";
        //            query = String.Format("select id from {0};", node.Text);
        //            DataTable dt = api.Query(query);
        //            node.Nodes.Clear();
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                string id = (string)dr["id"];
        //                node.Nodes.Add(id, String.Format(formatString, id));
        //            }
        //        }
        //        node.Expand();

        //        StatusLabel.Text = "Successfully loaded nodes...";
        //        this.Refresh();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        StatusLabel.Text = "Error: " + ex.Message;
        //        return false;
        //    }
        //}

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            currentTable = e.Node.Text;
            toolStripLabelQueryTable.Text = currentTable;
            ShowPage(0);

        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            ShowPage(currentPage);
        }

        private void ShowData(System.Data.DataTable data)
        {
            EditMode = false;
            dataView.Enabled = false;
            toolStripSwitchTableStyle.Visible = false;
            toolStripQueryOptions.Visible = false;
            SwitchVisibilityEditAndViewControls(false);
            this.Refresh();
            dataView.DataSource = data;
        }

        private void SwitchVisibilityEditAndViewControls(bool visible)
        {
            this.BtnDelete.Visible = visible;
            this.BtnExportTable.Visible = visible;
            this.BtnNew.Visible = visible;
            this.BtnUpdate.Visible = visible;
            this.BtnViewMode.Visible = visible;
            this.BtnQuery.Visible = visible;
            this.BtnPagePrev.Visible = visible;
            this.BtnPageNext.Visible = visible;
            this.BtnPageFirst.Visible = visible;
            this.BtnPageLast.Visible = visible;
            this.toolStripLabelPage.Visible = visible;
            this.toolStripComboBoxPageSize.Visible = visible;
            this.toolStripLabelSelectFrom.Visible = visible;
            this.toolStripLabelWhereClause.Visible = visible;
            this.toolStripLabelOrderBy.Visible = visible;
            this.toolStripLabelPagingSize.Visible = visible;
            this.toolStripSeparatorPaging.Visible = visible;
            this.toolStripSeparatorExport.Visible = visible;
            this.toolStripSeparatorPaging.Visible = visible;
        }

        private void ShowTablesMetaData(System.Data.DataTable data)
        {
            //EditMode = false;
            //dataView.Enabled = false;
            toolStripSwitchTableStyle.Visible = true;
            toolStripQueryOptions.Visible = false;
            SwitchVisibilityEditAndViewControls(false);
            this.Refresh();
            dataView.DataSource = data;
        }

        private void ShowPage(int pageNum)
        {
            if (!CheckChangedStatus())
                return;
            try
            {
                if ((api == null) || (currentTable == null)) { throw new InvalidOperationException("API or table object null - bugfix required"); }
                StatusLabel.Text = "Retriving elements...";
                currentPage = pageNum;
                EditMode = false;
                UpdatePageCaption();
                dataView.Enabled = false;
                toolStripSwitchTableStyle.Visible = true;
                toolStripQueryOptions.Visible = true;
                SwitchVisibilityEditAndViewControls(true);
                this.Refresh();
                string query = String.Format("select * from {0} limit {1},{2};", currentTable, currentPage * pageLimit, pageLimit);
                DataTable dt = api.Query(query);
                dataView.DataSource = dt;
                StatusLabel.Text = "Successfully retrived elements";
                dataView.Enabled = true;
            }
            catch (VTigerApiSessionTimedOutException ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server session timeout error: " + ex.Message;
                this.LoginToolStripMenuItem_Click(null, null);
                ShowTablesMetaData(null);
            }
            catch (VTigerApiException ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server error: " + ex.Message;
                ShowTablesMetaData(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
                ShowTablesMetaData(null);
            }
        }

        public void UpdateOderByList()
        {
            try
            {
            toolStripComboboxOrderBy.Items.Clear();
            if (dataView.DataSource != null)
                foreach (DataColumn col in (dataView.DataSource as DataTable).Columns)
                {
                    toolStripComboboxOrderBy.Items.Add(col.Caption);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
            }
        }

        private void UpdatePageCaption()
        {
            toolStripLabelPage.Text = String.Format("Page {0} / ?", currentPage + 1);
        }

        private void BtnPageNext_Click(object sender, EventArgs e)
        {
            ShowPage(currentPage + 1);
        }

        private void BtnPagePrev_Click(object sender, EventArgs e)
        {
            if (currentPage > 0)
                currentPage--;
            ShowPage(currentPage);
        }

        private void BtnPageFirst_Click(object sender, EventArgs e)
        {
            ShowPage(0);
        }

        #endregion

        #region Data editing

        private bool CheckChangedStatus(bool askForUpdate = true)
        {
            if (!changed)
                return true;
            if (askForUpdate)
                if (MessageBox.Show(
                    "Values have been changed. Commit changes?",
                    "Update?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    changed = false;
                    creatingEntry = false;
                    return true;
                }
            try
            {
                if (creatingEntry)
                {
                    StatusLabel.Text = "Creating entry...";
                    this.Refresh();

                    DataTable dt = (dataView.DataSource as DataTable);
                    dt = api.Create(VTiger.VTigerTypeParse(currentTable), dt.Rows[0]);
                    dataView.DataSource = dt;

                    StatusLabel.Text = "Successfully created entry";
                }
                else
                {
                    StatusLabel.Text = "Updating entry...";
                    this.Refresh();

                    DataTable dt = (dataView.DataSource as DataTable);
                    //dt = api.UpdateTable(dt);
                    dt = api.Update(dt.Rows[dataView.SelectedCells[0].RowIndex]);
                    dataView.DataSource = dt;

                    StatusLabel.Text = "Successfully updated entry";
                }
                changed = false;
                EditMode = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
                return false;
            }
            return true;
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            changed = true;
        }

        //========== ToolBar ==========

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            changed = true;
            CheckChangedStatus(false);
        }

        private void BtnViewMode_Click(object sender, EventArgs e)
        {
            if (creatingEntry)
            {
                dataView.ShowKeyValueTable = !dataView.ShowKeyValueTable;
                return;
            }
            if (!CheckChangedStatus())
                return;
            if (dataView.ShowKeyValueTable)
            {
                pageLimit = 100;
                currentPage /= 100 / 5;
            }
            else
            {
                pageLimit = 5;
                currentPage *= 100 / 5;
            }
            ShowPage(currentPage);
            dataView.ShowKeyValueTable = !dataView.ShowKeyValueTable;
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            if (!CheckChangedStatus())
                return;

            /* code optimized for slow or offline connections to VTiger server
            try
            {
                creatingEntry = true;
                changed = false;
                EditMode = true;
                DataTable dt = api.NewElement(api.RemoteTables[currentTable].ElementType);
                dataView.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
            }
            */

            try
            {
                StatusLabel.Text = "Retriving elements...";
                creatingEntry = true;
                changed = false;
                EditMode = true;
                DataTable dt = api.NewElementFromRemoteServerScheme(currentTable);
                dataView.DataSource = dt;
                StatusLabel.Text = "Successfully retrived elements";
            }
            catch (VTigerApiSessionTimedOutException ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server session timeout error: " + ex.Message;
                this.LoginToolStripMenuItem_Click(null, null);
            }
            catch (VTigerApiException ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server error: " + ex.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dataView.DataSource as DataTable;
                int selRow;
                if (dataView.ShowKeyValueTable)
                    selRow = dataView.SelectedCells[0].ColumnIndex - 1;
                else
                    selRow = dataView.SelectedCells[0].RowIndex;
                string id = (string)dt.Rows[selRow]["id"];

                if (MessageBox.Show(
                    String.Format("Delete element with ID [{0}]?", id),
                    "Delete?", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                StatusLabel.Text = "Deleting element";
                api.Delete(id);
                StatusLabel.Text = "Successfully deleted element";
                ShowPage(currentPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
            }
        }

        #endregion

        #region New-menu

        public string GetInput(string name)
        {
            string value = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter " + name, "Input");
            if (value == "")
                throw new Exception("Aborted");
            return value;
        }

        private void MenuItemNewContact_Click(object sender, EventArgs e)
        {
            try
            {
                string firstname = GetInput("firstname");
                string lastname = GetInput("lastname");
                StatusLabel.Text = "Creating element";
                VTigerContact newContact = api.AddContact(firstname, lastname, api.UserID);
                StatusLabel.Text = "Successfully created element with ID " + newContact.id.ToString();
                MessageBox.Show(this, "Successfully created element with ID " + newContact.id.ToString() + " / contact no. " + newContact.contact_no, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowPage(currentPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
            }
        }

        private void CalendarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string subject = GetInput("subject");

                StatusLabel.Text = "Creating element";
                api.AddCalendar(api.UserID, subject, DateTime.Now, DateTime.Now,
                    TaskStatus.In_Progress);
                StatusLabel.Text = "Successfully created element";
                ShowPage(currentPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
            }
        }

        #endregion

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (!CheckChangedStatus())
                return;
            try
            {
                StatusLabel.Text = "Executing query...";
                currentPage = 0;
                EditMode = false;
                UpdatePageCaption();
                this.Refresh();

                string query;
                if (toolStripComboboxOrderBy.Text == "")
                    query = String.Format("SELECT * FROM {0} WHERE {1} LIMIT {2},{3};",
                        currentTable,
                        toolStripTextboxQueryWhereClause.Text,
                        currentPage * pageLimit, pageLimit);
                else
                    query = String.Format("SELECT * FROM {0} WHERE {1} ORDER BY {2} LIMIT {3},{4};",
                        currentTable,
                        toolStripTextboxQueryWhereClause.Text,
                        toolStripComboboxOrderBy.Text,
                        currentPage * pageLimit, pageLimit);
                DataTable dt = api.Query(query);
                dataView.DataSource = dt;

                StatusLabel.Text = "Finished";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
            }
        }

        private void DataView_DataSourceChanged(object sender, EventArgs e)
        {
            UpdateOderByList();
        }

        private void BtnExportTable_Click(object sender, EventArgs e)
        {
            DataTable dt = dataView.DataSource as DataTable;
            WriteDataTable(currentTable + ".csv", dt, ';');
        }

        private void WriteDataTable(string fileName, DataTable datatable, char seperator)
        {
            try
            {
                CompuMaster.Data.Csv.WriteDataTableToCsvFile(fileName, datatable, true, CompuMaster.Data.Csv.WriteLineEncodings.Default, System.Globalization.CultureInfo.CurrentCulture, "UTF-8");
                DialogResult UserChoiceOpenFileImmediately = MessageBox.Show(this, "Export to " + fileName + " succeeded.\r\n\r\nWould you like to open the file, now?", "Export", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (UserChoiceOpenFileImmediately == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
            }
        }

        private void CompareLocalVsRemoteTableButton_Click(object sender, EventArgs e)
        {
            if (!CheckChangedStatus())
                return;

            try
            {
                DataTable localDataTable = api.NewElement(api.RemoteTables[currentTable].ElementType);

                //Query remote table structure
                StatusLabel.Text = "Retriving elements...";
                this.Refresh();
                creatingEntry = true;
                changed = false;
                EditMode = true;
                DataTable remoteDataTable = api.NewElementFromRemoteServerScheme(currentTable);
                StatusLabel.Text = "Successfully retrived elements";
                this.Refresh();

                //Identify the additional columns from remote server
                DataTable diffDataTable = remoteDataTable.Clone();
                for (int myCounter=0; myCounter < remoteDataTable.Columns.Count; myCounter++)
                {
                    DataColumn col = remoteDataTable.Columns[myCounter];
                    if (localDataTable.Columns.Contains(col.ColumnName))
                        diffDataTable.Columns.Remove(diffDataTable.Columns[col.ColumnName]);
                }

                //Show results to user
                DataTable diffDescriptionDataTable = new DataTable();
                diffDescriptionDataTable.Columns.Add("ColumnName", typeof(string));
                diffDescriptionDataTable.Columns.Add("DataType", typeof(string));
                for (int myCounter = 0; myCounter < diffDataTable.Columns.Count; myCounter++)
                {
                    DataColumn col = diffDataTable.Columns[myCounter];
                    DataRow row = diffDescriptionDataTable.NewRow();
                    row["ColumnName"] = col.ColumnName;
                    row["DataType"] = col.DataType.ToString();
                    diffDescriptionDataTable.Rows.Add(row);
                }
                ShowTablesMetaData(diffDescriptionDataTable);
            }
            catch (VTigerApiSessionTimedOutException ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server session timeout error: " + ex.Message;
                this.LoginToolStripMenuItem_Click(null, null);
            }
            catch (VTigerApiException ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server error: " + ex.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
            }
        }

        private void IgnoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Checked = !this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Checked;
            VTiger.IgnoreSslCertificateErrors = this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Checked;
        }

        private void NewRecordForEveryTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // prepare result table
            System.Data.DataTable results = new DataTable("root");
            results.Columns.Add("TypeName", typeof(string));
            results.Columns.Add("PrimaryKeyID", typeof(string));
            results.Columns.Add("PrimaryKeyNo", typeof(string));
            results.Columns.Add("Exception", typeof(string));

            Exception newRecordCreationException;
            string newRecordID;
            string newRecordNo;
            this.Cursor = Cursors.WaitCursor;

            // try to create new account
            StatusLabel.Text = "Creating new account . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerAccount newAcount = api.AddAccount("TestAccount " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), api.UserID);
                newRecordID = newAcount.id;
                newRecordNo = newAcount.account_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Account", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new asset
            StatusLabel.Text = "Creating new asset . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerAsset newAsset = api.AddAsset("TestProduct", "s/n 0000", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), Assetstatus.Outofservice, "TestAsset " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), "TestAccount", api.UserID);
                newRecordID = newAsset.id;
                newRecordNo = newAsset.asset_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Asset", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new calendar
            StatusLabel.Text = "Creating new calendar entry . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerCalendar newCalendar = api.AddCalendar(api.UserID, "TestCalendarEntry", DateTime.Now, DateTime.Now.AddDays(3), TaskStatus.Planned);
                newRecordID = newCalendar.id;
                newRecordNo = "N/A";
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Calendar", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new campaign
            StatusLabel.Text = "Creating new campaign . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerCampaign newCampaign = api.AddCampaign("TestCampaign " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now, api.UserID);
                newRecordID = newCampaign.id;
                newRecordNo = newCampaign.campaign_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Campaigns", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new contact
            StatusLabel.Text = "Creating new contact . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerContact newContact = api.AddContact("TestFirstName", "TestFamilyName " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), api.UserID);
                newRecordID = newContact.id;
                newRecordNo = newContact.contact_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Contacts", newRecordID, newRecordNo, newRecordCreationException);

            // don't try to create new currency
            
            // don't try to create new documentFolders

            // try to create new document
            StatusLabel.Text = "Creating new document . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerDocument newDocument = api.AddDocument("TestNote " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), api.UserID);
                newRecordID = newDocument.id;
                newRecordNo = "N/A";
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Documents", newRecordID, newRecordNo, newRecordCreationException);

            // don't try to create new email

            // try to create new event
            StatusLabel.Text = "Creating new event . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerEvent newEvent = api.AddEvent("TestEvent " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("HH:mm:ss"),"23:59:59", 4, Eventstatus.Planned, Activitytype.Meeting, api.UserID);
                newRecordID = newEvent.id;
                newRecordNo = "N/A";
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Events", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new faq
            StatusLabel.Text = "Creating new faq entry . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerFaq newFaq = api.AddFaq(Faqstatus.Draft, "TestFaq " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), "TestAnswer");
                newRecordID = newFaq.id;
                newRecordNo = newFaq.faq_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Faq", newRecordID, newRecordNo, newRecordCreationException);

            // don't try to create new group

            // try to create new helpdesk
            StatusLabel.Text = "Creating new helpdesk entry . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerHelpDesk newHelpdesk = api.AddHelpDesk(api.UserID, Ticketstatus.Open, "TestHelpDesk " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                newRecordID = newHelpdesk.id;
                newRecordNo = newHelpdesk.ticket_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "HelpDesk", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new invoice
            StatusLabel.Text = "Creating new invoice . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerInvoice newInvoice = api.AddInvoice("TestInvoice " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), "TestBillStreet", "TestShippingStreet", "TestAccount", api.UserID);
                newRecordID = newInvoice.id;
                newRecordNo = newInvoice.invoice_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Invoice", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new lead
            StatusLabel.Text = "Creating new lead . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerLead newLead = api.AddLead("TestLeadFamilyName " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), "TestLeadCompany", api.UserID);
                newRecordID = newLead.id;
                newRecordNo = newLead.lead_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Leads", newRecordID, newRecordNo, newRecordCreationException);

            // don't try to create new ModComment

            // try to create new PBXManager
            StatusLabel.Text = "Creating new PBXManager . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerPBXManager newPBXManager = api.AddPBXManager("TestCustomerNo", "TestPBXManagerCallFrom " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), "TestPBXManagerCallTo", api.UserID);
                newRecordID = newPBXManager.id;
                newRecordNo = "N/A";
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "PBXManager", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new potential
            StatusLabel.Text = "Creating new potential . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerPotential newPotential = api.AddPotential("TestPotential " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), "RelatedTo", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Sales_stage.Needs_Analysis, api.UserID);
                newRecordID = newPotential.id;
                newRecordNo = newPotential.potential_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Potentials", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new PriceBook
            StatusLabel.Text = "Creating new pricebook . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                //VTigerCurrency currency = api.Query<VTigerCurrency>("SELECT * FROM Currency LIMIT 0, 1")[0];
                VTigerCurrency currency = api.Query<VTigerCurrency>(0, 1)[0];
                VTigerPriceBook newPriceBook = api.AddPriceBook("TestPriceBook " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), currency.id);
                newRecordID = newPriceBook.id;
                newRecordNo = newPriceBook.pricebook_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "PriceBooks", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new Product
            StatusLabel.Text = "Creating new product . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerProduct newProduct = api.AddProduct("TestProduct " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), api.UserID);
                newRecordID = newProduct.id;
                newRecordNo = newProduct.product_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Products", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new project
            // BUT: AddProject not available!!!
            //StatusLabel.Text = "Creating new project . . ."; this.Refresh();
            //newRecordCreationException = null;
            //newRecordID = null;
            //newRecordNo = null;
            //try
            //{
            //    VTigerProject newProject = api.AddProject("TestProject " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), api.UserID);
            //    newRecordID = newProject.id;
            //    newRecordNo = newProject.project_no;
            //}
            //catch (Exception ex) { newRecordCreationException = ex; }
            //newRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Project", newRecordID, newRecordNo, newRecordCreationException);

            // don't try to create new ProjectMileStone
            // don't try to create new ProjectTask

            // try to create new PurchaseOrder
            StatusLabel.Text = "Creating new purchase order . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerPurchaseOrder newPurchaseOrder = api.AddPurchaseOrder("TestPurchaseOrder " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), "VendorID", PoStatus.Created, "TestBillStreet", "TestShippingStreet", api.UserID);
                newRecordID = newPurchaseOrder.id;
                newRecordNo = newPurchaseOrder.purchaseorder_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "PurchaseOrder", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new quote
            StatusLabel.Text = "Creating new quote . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerQuote newQuote = api.AddQuote("TestQuote " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), Quotestage.Created, "TestBillStreet", "TestShippingStreet", "TestAccountID", api.UserID);
                newRecordID = newQuote.id;
                newRecordNo = newQuote.quote_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Quotes", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new salesorder
            StatusLabel.Text = "Creating new salesorder . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerSalesOrder newSalesOrder = api.AddSalesOrder("TestFamilyName " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), SoStatus.Created, "TestBillStreet", "TestShippingStreet", Invoicestatus.Created, "TestAccountID", api.UserID);
                newRecordID = newSalesOrder.id;
                newRecordNo = newSalesOrder.salesorder_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "SalesOrders", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new ServiceContract
            //usually times out with vtiger V7.0 --> temporarily disabled code
            //StatusLabel.Text = "Creating new service contract . . ."; this.Refresh();
            //newRecordCreationException = null;
            //newRecordID = null;
            //newRecordNo = null;
            //try
            //{
            //    VTigerServiceContract newServiceContract = api.AddServiceContract("TestServiceContract " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), api.UserID);
            //    newRecordID = newServiceContract.id;
            //    newRecordNo = newServiceContract.contract_no;
            //}
            //catch (Exception ex) { newRecordCreationException = ex; }
            //newRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "ServiceContracts", newRecordID, newRecordNo, newRecordCreationException);

            // try to create new Service
            StatusLabel.Text = "Creating new service . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerService newService = api.AddService("TestService " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                newRecordID = newService.id;
                newRecordNo = newService.service_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Services", newRecordID, newRecordNo, newRecordCreationException);

            // don't try to create new SMSNotifier

            // try to create new Vendor
            StatusLabel.Text = "Creating new vendor . . ."; this.Refresh();
            newRecordCreationException = null;
            newRecordID = null;
            newRecordNo = null;
            try
            {
                VTigerVendor newVendor = api.AddVendor("TestVendor " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), api.UserID);
                newRecordID = newVendor.id;
                newRecordNo = newVendor.vendor_no;
            }
            catch (Exception ex) { newRecordCreationException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Vendors", newRecordID, newRecordNo, newRecordCreationException);

            // show results to GUI
            ShowData(results);
            this.Cursor = Cursors.Default;
            StatusLabel.Text = "Bulk insert completed, see results for success status";
            MessageBox.Show(this, "Bulk insert completed, see results for success status", "Bulk insert - all available item types", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(System.Data.DataTable resultsTable, string typeName, string primaryKeyID, string primaryKeyNo, Exception ex)
        {
            System.Data.DataRow NewRow = resultsTable.NewRow();
            NewRow["TypeName"] = typeName;
            NewRow["PrimaryKeyID"] = primaryKeyID;
            NewRow["PrimaryKeyNo"] = primaryKeyNo;
            NewRow["Exception"] = ex;
            resultsTable.Rows.Add(NewRow);
        }

        private void BulkInsert1500ContactRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                for (int myCounter = 0; myCounter < 1500; myCounter++)
                {
                    StatusLabel.Text = "Bulk creation of new contacts " + (myCounter + 1).ToString() + " / 1500";
                    VTigerContact newContact = api.AddContact("TestFirstName #" + myCounter.ToString(), "TestFamilyName " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), api.UserID);
                }
                this.Cursor = Cursors.Default;
                StatusLabel.Text = "Bulk insert of 1500 contacts completed successfully";
                MessageBox.Show(this, "Bulk insert of 1500 contacts completed successfully", "Bulk insert - Contacts", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (VTigerApiSessionTimedOutException ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server session timeout error: " + ex.Message;
                this.LoginToolStripMenuItem_Click(null, null);
            }
            catch (VTigerApiException ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server error: " + ex.Message;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(this, ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "Error: " + ex.Message;
            }
        }

        private void ToolStripComboBoxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignNewPagingSize(this.toolStripComboBoxPageSize.Text);
            if (!(api == null) && !(currentTable == null)) { ShowPage(currentPage); } // refresh current's table view
        }

        /// <summary>
        /// Assign the new paging size to the toolbar control + save as user setting for next application launch + return as valid value for toolbar combobox
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string AssignNewPagingSize(string size)
        {
            string result;
            switch (size)
            {
                case "":
                case null:
                    this.pageLimit = 50;
                    result = "50";
                    break;
                case "∞":
                    this.pageLimit = System.Int32.MaxValue;
                    result = "∞";
                    break;
                default:
                    this.pageLimit = System.Int32.Parse(size);
                    result = this.pageLimit.ToString();
                    break;
            }
            Properties.Settings.Default["PagingSize"] = size;
            Properties.Settings.Default.Save();
            return result;
        }

        private void QueryFromAllRemoteTablesWithoutErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // prepare result table
            System.Data.DataTable results = new DataTable("root");
            results.Columns.Add("TypeName", typeof(string));
            results.Columns.Add("PrimaryKeyID", typeof(string));
            results.Columns.Add("PrimaryKeyNo", typeof(string));
            results.Columns.Add("Exception", typeof(string));

            Exception loadException;
            this.Cursor = Cursors.WaitCursor;

            // try to load first row of Account
            StatusLabel.Text = "Loading 1 row of Account . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerAccount>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Account", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Asset
            StatusLabel.Text = "Loading 1 row of Asset . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerAsset>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Asset", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Calendar
            StatusLabel.Text = "Loading 1 row of Calendar . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerCalendar>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Calendar", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Campaign
            StatusLabel.Text = "Loading 1 row of Campaign . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerCampaign>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Campaign", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Contact
            StatusLabel.Text = "Loading 1 row of Contact . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerContact>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Contact", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Currency
            StatusLabel.Text = "Loading 1 row of Currency . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerCurrency>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Currency", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Document
            StatusLabel.Text = "Loading 1 row of Document . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerDocument>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Document", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of DocumentFolders
            StatusLabel.Text = "Loading 1 row of DocumentFolders . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerDocumentFolder>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "DocumentFolders", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of EMail
            StatusLabel.Text = "Loading 1 row of EMail . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerEmail>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "EMail", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Event
            StatusLabel.Text = "Loading 1 row of Event . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerEvent>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Event", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Faq
            StatusLabel.Text = "Loading 1 row of Faq . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerFaq>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Faq", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Group
            StatusLabel.Text = "Loading 1 row of Group . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerGroup>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Group", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of HelpDesk
            StatusLabel.Text = "Loading 1 row of HelpDesk . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerHelpDesk>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "HelpDesk", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Invoice
            StatusLabel.Text = "Loading 1 row of Invoice . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerInvoice>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Invoice", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Lead
            StatusLabel.Text = "Loading 1 row of Lead . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerLead>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Lead", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of ModComment
            StatusLabel.Text = "Loading 1 row of ModComment . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerModComment>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "ModComment", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of PBXManager
            StatusLabel.Text = "Loading 1 row of PBXManager . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerPBXManager>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "PBXManager", "", (loadException == null ? "OK" : "Error"), loadException);
            
            // try to load first row of Potential
            StatusLabel.Text = "Loading 1 row of Potential . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerPotential>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Potential", "", (loadException == null ? "OK" : "Error"), loadException);
            
            // try to load first row of PriceBook
            StatusLabel.Text = "Loading 1 row of PriceBook . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerPriceBook>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "PriceBook", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Product
            StatusLabel.Text = "Loading 1 row of Product . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerProduct>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Product", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Project
            StatusLabel.Text = "Loading 1 row of Project . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerProject>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Project", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of ProjectMileStone
            StatusLabel.Text = "Loading 1 row of ProjectMileStone . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerProjectMilestone>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "ProjectMileStone", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of ProjectTask
            StatusLabel.Text = "Loading 1 row of ProjectTask . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerProjectTask>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "ProjectTask", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of PurchaseOrder
            StatusLabel.Text = "Loading 1 row of PurchaseOrder . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerPurchaseOrder>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "PurchaseOrder", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Quote
            StatusLabel.Text = "Loading 1 row of Quote . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerQuote>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Quote", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of SalesOrder
            StatusLabel.Text = "Loading 1 row of SalesOrder . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerSalesOrder>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "SalesOrder", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of ServiceContract
            StatusLabel.Text = "Loading 1 row of ServiceContract . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerServiceContract>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "ServiceContract", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Service
            StatusLabel.Text = "Loading 1 row of Service . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerService>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Service", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of SMSNotifier
            StatusLabel.Text = "Loading 1 row of SMSNotifier . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerSMSNotifier>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "SMSNotifier", "", (loadException == null ? "OK" : "Error"), loadException);

            // try to load first row of Vendor
            StatusLabel.Text = "Loading 1 row of Vendor . . ."; this.Refresh();
            loadException = null;
            try
            {
                api.Query<VTigerVendor>(0, 1);
            }
            catch (Exception ex) { loadException = ex; }
            NewRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Vendor", "", (loadException == null ? "OK" : "Error"), loadException);
            
            // show results to GUI
            results.Columns["PrimaryKeyID"].ColumnName = "Status";
            results.Columns.Remove("PrimaryKeyNo");
            ShowData(results);
            this.Cursor = Cursors.Default;
            StatusLabel.Text = "Query test completed, see results for success status";
        }
    }
}
