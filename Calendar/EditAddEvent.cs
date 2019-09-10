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
    public partial class EditAddEvent : Form
    {
        private EventStruct ES;
        
        private DateTime DT;
        private string Caption;
        private bool Event;
        private bool signal;
        private int BeforeDay;
        private DateTime time;

        private int IndexOfEvent;

        //private string WorkDir;
        private string FileName;
        private byte[] vers = { 0, 1, 0, 0 };

        private MainForm mfr;

        public EditAddEvent(string fName, byte[] vr, MainForm fr)
        {
            InitializeComponent();

            DT = DateTime.Now;
            //time = new DateTime(
            comboBox1.SelectedItem = 0;

            FileName = fName;
            Array.Copy(vr, vers, 4);

            mfr = fr;
        }

        /// <summary>
        /// добавить событие в день
        /// </summary>
        /// <param name="fName">рабочая папка</param>
        /// <param name="vr">версия программы</param>
        /// <param name="day">день</param>
        public EditAddEvent(string fName, byte[] vr, DateTime day, MainForm fr)
        {
            InitializeComponent();

            FileName = fName;
            Array.Copy(vr, vers, 4);

            DT = day;
            Caption = "";
            Event = true;
            signal = false;
            IndexOfEvent = -1;

            mfr = fr;
        }

        
        /// <summary>
        /// Изменить
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="vr"></param>
        /// <param name="indx">индекс изменяемого события</param>
        public EditAddEvent(string fName, byte[] vr, int indx, MainForm fr)
        {
            InitializeComponent();

            FileName = fName;
            Array.Copy(vr, vers, 4);

            IndexOfEvent = indx;
            ReadWriteFile RWF = new ReadWriteFile(FileName, vers);
            //EventStruct ES = RWF.GetEvent(75);
            EventStruct ES = RWF.GetEvent(indx);

            DT = ES.Date;
            Caption = ES.Caption;
            Event = !ES.IsEarly;
            signal = ES.NeedInform;
            TimeSpan ts = DT.Date - ES.BeforeInform.Date;
            BeforeDay = ts.Days;
            ts = ES.BeforeInform.TimeOfDay;
            time = new DateTime(ts.Ticks);

            mfr = fr;
        }


        /*
        public EditAddEvent(string WDir, byte[] vr, DateTime day, string Capt, bool Ev)
        {
            InitializeComponent();
            
            WorkDir = WDir;
            Array.Copy(vr, vers, 4);

            DT = day;
            //ES.Date = day;
            Caption = Capt;
            //ES.Caption = Capt;
            Event = Ev;
            //ES.IsEarly = !Ev;
            signal = false;
            //ES.NeedInform = false;

            //ES.CapLength = (byte)Capt.Length;
            //ES.BeforeInform = day;
            //ES.Informed = true;

            //comboBox1.SelectedItem = 0;
        }
        */

        /*
        public EditAddEvent(string WDir, byte[] vr, DateTime day, string Capt, bool Ev, int BefDay, DateTime tm)
        {
            InitializeComponent();
            
            WorkDir = WDir;
            Array.Copy(vr, vers, 4);

            DT = day;
            //ES.Date = day;
            Caption = Capt;
            //ES.Caption = Capt;
            //ES.CapLength = (byte)Capt.Length;
            Event = Ev;
            //ES.IsEarly = !Ev;
            signal = true;
            //ES.NeedInform = true;
            BeforeDay = BefDay;
            //DateTime dt = new DateTime(day.AddDays(-BefDay).Ticks);

            //ES.BeforeInform = new DateTime(day.AddDays(-BefDay),
            time = tm;

            //comboBox1.SelectedItem = 0;


        }
        */

        private void EditAddEvent_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DT;
            //dateTimePicker1.Value = ES.Date;
            if (Event == true)
            //if (ES.IsEarly == false)
                comboBox1.SelectedIndex = 0;
            else
                comboBox1.SelectedIndex = 1;

            textBox1.Text = Caption;
            //textBox1.Text = ES.Caption;

            if (signal == true)
            //if (ES.NeedInform == true)
            {
                checkBox1.Checked = true;
                numericUpDown1.Value = Convert.ToDecimal(BeforeDay);
                /*
                int h = time.Hour;
                string s;
                if (h < 10)
                    s = "0" + h.ToString();
                else
                    s = h.ToString();
                
                int m = time.Minute;
                if (m < 10)
                    s = s + ":0" + m.ToString();
                else
                    s = s + ":" + m.ToString();
                maskedTextBox1.Text = s;
                //maskedTextBox1.Text = time.Hour + ":" + time.Minute;
                 */
                dateTimePicker2.Value = DateTime.Now.Date;
                //dateTimePicker2.Value = dateTimePicker2.Value.AddHours(Convert.ToDouble(time.Hour));
                dateTimePicker2.Value = dateTimePicker2.Value.AddHours(time.Hour);
                //dateTimePicker2.Value = dateTimePicker2.Value.AddMinutes(Convert.ToDouble(time.Minute));
                dateTimePicker2.Value = dateTimePicker2.Value.AddMinutes(time.Minute);
                
            }
            else
                checkBox1.Checked = false;
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = checkBox1.Checked;
        }



        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int AA = Convert.ToInt32(numericUpDown1.Value);
            string s = AA.ToString();
            s = s.Substring(s.Length - 1);
            AA = Convert.ToInt32(s);

            if (AA == 0)
                label2.Text = "дней,";
            else if (AA == 1)
                label2.Text = "день,";
            else if (AA < 5)
                label2.Text = "дня,";
            else
                label2.Text = "дней,";
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                this.BackColor = Color.Goldenrod;
            else
                this.BackColor = Color.OliveDrab;
        }




        private void btnOK_Click(object sender, EventArgs e)
        {
            DT = dateTimePicker1.Value;
            if (comboBox1.SelectedIndex == 0)
                Event = true;
            else
                Event = false;
            Caption = textBox1.Text;
            if (checkBox1.Checked)
            {
                signal = true;
                BeforeDay = Convert.ToInt32(numericUpDown1.Value);
                /*
                if (ChekTime() == false)
                {
                    MessageBox.Show("Время задано не верно!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    maskedTextBox1.Focus();
                    return;
                }
                */ 
            }
            else
                signal = false;

            
            // проверить уникальность события
            // -- нахэр, добавляйте скока влезет

            // сформировать структуру
            ES.Date = DT;
            ES.CapLength = (byte)Caption.Length;
            ES.Caption = Caption;
            ES.NeedInform = signal;
            if (signal == true)
            {
                /*
                DateTime d1 = DT.Date;
                
                // минус дней
                DateTime d2 = d1.AddDays(-Convert.ToDouble(BeforeDay));
                // плюс часы
                double hour = Convert.ToDouble(maskedTextBox1.Text.Substring(0,2));
                d1 = d2.AddHours(hour);
                // минуты
                double minute = Convert.ToDouble(maskedTextBox1.Text.Substring(3, 2));
                d2 = d1.AddMinutes(minute);
                
                ES.BeforeInform = d2;
                 */
                DateTime d1 = DT.Date.AddDays(-Convert.ToDouble(BeforeDay));
                ES.BeforeInform = new DateTime(d1.Year, d1.Month, d1.Day,
                                               dateTimePicker2.Value.Hour, dateTimePicker2.Value.Minute, 0);

                ES.Informed = false;
            }
            else
            {
                ES.BeforeInform = DT;
                ES.Informed = true;
            }
            ES.IsEarly = !Event;

            // произвести изменения
            ReadWriteFile RWF = new ReadWriteFile(FileName, vers);
            if (IndexOfEvent >= 0)      // изменить
                RWF.EditEvent(IndexOfEvent, ES);
            else                        // добавить
                RWF.AddEvent(ES);

            // переопределить таймер
            RestartTimer();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }


        // переопределить и перезап таймер
        private void RestartTimer()
        {
            mfr.Invoke(mfr.myDelegat);
        }

        /*
        // проверка времени ЧЧ:ММ
        private bool ChekTime()
        {
            string t = maskedTextBox1.Text;
            if (t.Length != 5)
                return (false);
            int h, m;
            try
            {
                h = Convert.ToInt32(t.Substring(0, 2));
                m = Convert.ToInt32(t.Substring(3));
            }
            catch
            {
                return (false);
            }

            if (h > 23 || m > 59)
                return (false);
            else
            {
                time = new DateTime(2000, 1, 1, h, m, 0);
                return (true);
            }
        }
        */
    }
}
