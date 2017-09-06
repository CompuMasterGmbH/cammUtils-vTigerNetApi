using System;
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
            formTitle();
            VTiger.IgnoreSslCertificateErrors = this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Checked;
            loginToolStripMenuItem_Click(null, null);
            ShowData(null);
        }

        private void toolStripButtonTableDescription_Click(object sender, EventArgs e)
        {
            try
            {
                ShowTablesMetaData(api.Describe_DataTable(api.RemoteTables[currentTable].ElementType));
            }
            catch (VTigerApiSessionTimedOutException ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server session timeout error: " + ex.Message;
                this.loginToolStripMenuItem_Click(null, null);
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
            formTitle();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void formTitle()
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

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
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
            formTitle();
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
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
                this.loginToolStripMenuItem_Click(null, null);
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

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
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
                this.loginToolStripMenuItem_Click(null, null);
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

        private void calendarToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void dataView_DataSourceChanged(object sender, EventArgs e)
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
                this.loginToolStripMenuItem_Click(null, null);
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

        private void ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Checked = !this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Checked;
            VTiger.IgnoreSslCertificateErrors = this.ignoreSSLCertificateErrorsOfRemoteServerToolStripMenuItem.Checked;
        }

        private void newRecordForEveryTypeToolStripMenuItem_Click(object sender, EventArgs e)
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

            // try to create new contact
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
            newRecordForEveryTypeToolStripMenuItem_AddResultRecord(results, "Contacts", newRecordID, newRecordNo, newRecordCreationException);

            // show results to GUI
            ShowData(results);
        }

        private void newRecordForEveryTypeToolStripMenuItem_AddResultRecord(System.Data.DataTable resultsTable, string typeName, string primaryKeyID, string primaryKeyNo, Exception ex)
        {
            System.Data.DataRow NewRow = resultsTable.NewRow();
            NewRow["TypeName"] = typeName;
            NewRow["PrimaryKeyID"] = primaryKeyID;
            NewRow["PrimaryKeyNo"] = primaryKeyNo;
            NewRow["Exception"] = ex;
            resultsTable.Rows.Add(NewRow);
        }

        private void bulkInsert1500ContactRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int myCounter = 0; myCounter < 1500; myCounter++)
                {
                    VTigerContact newContact = api.AddContact("TestFirstName #" + myCounter.ToString(), "TestFamilyName " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), api.UserID);
                }
                MessageBox.Show(this, "Bulk insert of 1500 contacts completed successfully", "Bulk insert - Contacts", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (VTigerApiSessionTimedOutException ex)
            {
                MessageBox.Show(this, ex.ToString(), "ERROR from remote server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StatusLabel.Text = "VTiger remote server session timeout error: " + ex.Message;
                this.loginToolStripMenuItem_Click(null, null);
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

        private void toolStripComboBoxPageSize_SelectedIndexChanged(object sender, EventArgs e)
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
    }
}
