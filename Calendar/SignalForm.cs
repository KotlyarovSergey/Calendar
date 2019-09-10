using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calendar
{
    public partial class SignalForm : Form
    {
        public SignalForm(string text, bool IsEarly, DateTime dt)
        {
            InitializeComponent();
            label1.Text = text;
            if (IsEarly == true)
                this.BackColor = Color.Green;
            else
                this.BackColor = Color.Orange;

            label2.Text = dt.ToShortDateString();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
