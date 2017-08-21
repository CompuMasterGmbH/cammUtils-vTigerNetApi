using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VTigerManager
{
    public partial class LoginWindow : Form
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel; //general return value if window has been closed
        }

        private void LoginWindow_Load(object sender, EventArgs e)
        {
            EdServiceUrl.Text = (string)Properties.Settings.Default["VTigerInstanceUrl"];
            EdUsername.Text = (string)Properties.Settings.Default["VTigerUser"];
            EdAuthKey.Text = (string)Properties.Settings.Default["VTigerAppKey"];
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            if (this.CheckBoxSaveCredentials.Checked)
            {
                Properties.Settings.Default["VTigerInstanceUrl"] = this.EdServiceUrl.Text;
                Properties.Settings.Default["VTigerUser"] = this.EdUsername.Text;
                Properties.Settings.Default["VTigerAppKey"] = this.EdAuthKey.Text;
                Properties.Settings.Default.Save();
            }
        }

    }
}
