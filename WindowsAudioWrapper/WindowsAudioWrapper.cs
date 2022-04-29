using WindowsAudioWrapper.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsAudioWrapper
{
    public partial class WindowsAudioWrapper : Form
    {
        private WindowsAudioManager manager;

        public WindowsAudioWrapper(WindowsAudioManager manager)
        {
            this.manager = manager;

            InitializeComponent();
        }

        public void ToggleForm(bool show)
        {
            if (show)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => { this.Show(); }));
                }
                else Show();
            }
            else
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => { this.Hide(); }));
                }
                else Hide();
            }
        }

        private void HueCommandSender_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(CommandInputTextBox.Text))
            {
                string response = manager.ProcessMessage(this.CommandInputTextBox.Text);

                this.CommandResultLabel.Text = response;
            }
        }
    }
}
