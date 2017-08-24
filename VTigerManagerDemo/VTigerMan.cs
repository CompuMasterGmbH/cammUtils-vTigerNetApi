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
        private int pageLimit = 20;

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
            formTitle();
            loginToolStripMenuItem_Click(null, null);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            dataView.DataSource = api.Describe_DataTable(api.RemoteTables[currentTable].ElementType);
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
                textBoxSessionID.Text = "";
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
                this.Text = this.Text = "VTiger Demo - connected to VTiger V" + api.VTigerVersion.ToString() + " at " + api.ServiceUrl;
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
            LQueryTable.Text = currentTable;
            ShowPage(0);

        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            ShowPage(currentPage);
        }

        private void ShowPage(int pageNum)
        {
            if (!CheckChangedStatus())
                return;
            try
            {
                StatusLabel.Text = "Retriving elements...";
                currentPage = pageNum;
                EditMode = false;
                UpdatePageCaption();
                dataView.Enabled = false;
                this.Refresh();
                string query = String.Format("select * from {0} limit {1},{2};", currentTable, currentPage * pageLimit, pageLimit);
                DataTable dt = api.Query(query);
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
            dataView.Enabled = true;
        }

        public void UpdateOderByList()
        {
            try
            {
            EdOrderBy.Items.Clear();
            if (dataView.DataSource != null)
                foreach (DataColumn col in (dataView.DataSource as DataTable).Columns)
                {
                    EdOrderBy.Items.Add(col.Caption);
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
            LPage.Text = String.Format("Page {0} / ?", currentPage + 1);
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
             */
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
                string account_id = GetInput("account_id");

                StatusLabel.Text = "Creating element";
                api.AddContact(firstname, lastname, api.UserID);
                StatusLabel.Text = "Successfully created element";
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
                if (EdOrderBy.Text == "")
                    query = String.Format("SELECT * FROM {0} WHERE {1} LIMIT {2},{3};",
                        currentTable,
                        EdQuery.Text,
                        currentPage * pageLimit, pageLimit);
                else
                    query = String.Format("SELECT * FROM {0} WHERE {1} ORDER BY {2} LIMIT {3},{4};",
                        currentTable,
                        EdQuery.Text,
                        EdOrderBy.Text,
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
                using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
                {
                    int numberOfColumns = datatable.Columns.Count;

                    for (int i = 0; i < numberOfColumns; i++)
                    {
                        sw.Write(datatable.Columns[i]);
                        if (i < numberOfColumns - 1)
                            sw.Write(seperator);
                    }
                    sw.Write(sw.NewLine);

                    foreach (DataRow dr in datatable.Rows)
                    {
                        for (int i = 0; i < numberOfColumns; i++)
                        {
                            sw.Write(dr[i].ToString());

                            if (i < numberOfColumns - 1)
                                sw.Write(seperator);
                        }
                        sw.Write(sw.NewLine);
                    }
                }
            }
            catch
            {
            }
        }

    }
}
