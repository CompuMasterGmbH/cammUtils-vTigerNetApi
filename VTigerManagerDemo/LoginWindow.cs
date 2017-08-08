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
        }

        private void LoginWindow_Load(object sender, EventArgs e)
        {
            EdServiceUrl.Text = (string)Properties.Settings.Default["VTigerInstanceUrl"];
            EdUsername.Text = (string)Properties.Settings.Default["VTigerUser"];
            EdAuthKey.Text = (string)Properties.Settings.Default["VTigerAppKey"];
        }
    }
}
