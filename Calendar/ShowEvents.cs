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
    public partial class ShowEvents : Form
    {
        //private string WorkDir;
        private string FileName;
        private byte[] vers = { 0, 1, 0, 0 };

        private DateTime Date;
        List<EventStruct> EventList;

        private struct EvPanell
        {
            public Panel MainPanel;
            public CheckBox chb;
            public Label date;
            public Label type;
            //public TextBox Caption;
            public Label Caption;
            public Label befor;
            public Label time;
        };
        List<EvPanell> EvPnlList;
        List<int> OriginalIndex;

        private bool ShowOneDay;

        private string[] month = new string[12] {"Январь", "Февраль", "Март", "Апрель", "Март", "Июнь", "Июль", "Август",
                                   "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"};

        private MainForm MnFrm;

        private ShowEventsObjective obj;
        private string ImpFlName;

        // ----------------------------------------------------
        /*
        public ShowEvents()
        {
            InitializeComponent();
        }
        */
        
        /// <summary>
        /// Импорт событий
        /// </summary>
        /// <param name="WorkFileName"></param>
        /// <param name="ImportFileName"></param>
        /// <param name="vr"></param>
        /// <param name="fr"></param>
        /// <param name="objective"></param>
        public ShowEvents(string WorkFileName, string ImportFileName, byte[] vr, MainForm fr, ShowEventsObjective objective)
        {
            InitializeComponent();
            FileName = WorkFileName;
            ImpFlName = ImportFileName;
            Array.Copy(vr, vers, 4);
            lblCaption.Text = "Импорт событий";
            MnFrm = fr;
            obj = objective;
            btnExpImpAdd.Text = "Импорт";
            btnExpImpAdd.Click += new EventHandler(btnImp_Click);
            btnDelete.Visible = false;
            btnEdit.Visible = false;
        }


        /// <summary>
        /// просморт/экспорт
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="vr"></param>
        /// <param name="DT"></param>
        /// <param name="dpz"></param>
        /// <param name="fr"></param>
        /// <param name="objective"></param>
        public ShowEvents(string fName, byte[] vr, DateTime DT, Diapazon dpz, MainForm fr, ShowEventsObjective objective)
        {
            InitializeComponent();
            FileName = fName;
            Date = DT;
            //WorkDir = WDir;

            Array.Copy(vr, vers, 4);

            // заголовок
            switch (dpz)
            {
                case Diapazon.Day:      // день
                    lblCaption.Text = DT.ToShortDateString();
                    ShowOneDay = true;
                    lblDate.Visible = false;
                    break;
                case Diapazon.Month:    // месяц
                    lblCaption.Text = month[DT.Month] + " " + DT.Year.ToString();
                    ShowOneDay = false;
                    lblDate.Visible = true;
                    break;
                default:                // все события
                    lblCaption.Text = "Список всех событий";
                    ShowOneDay = false;
                    lblDate.Visible = true;
                    break;
            }
            //pnlMain.Dispose();
            //panel4.Dispose();

            MnFrm = fr;
            obj = objective;
            switch (objective)
            {
                case ShowEventsObjective.toExport:
                    btnExpImpAdd.Text = "Экспорт";
                    btnExpImpAdd.Click += new EventHandler(btnExp_Click);
                    btnDelete.Visible = false;
                    btnEdit.Visible = false;
                    break;
                case ShowEventsObjective.toImport:
                    btnExpImpAdd.Text = "Импорт";
                    btnExpImpAdd.Click += new EventHandler(btnImp_Click);
                    btnDelete.Visible = false;
                    btnEdit.Visible = false;
                    break;
                default:
                    btnExpImpAdd.Click += new EventHandler(btnAdd_Click);
                    break;
            }
        }



        // загрузка формы
        private void ShowEvents_Load(object sender, EventArgs e)
        {
            // список событий
            EventList = new List<EventStruct>();
            // список оригинальных индексов
            OriginalIndex = new List<int>();
            
            // заполнить списки
            
            if (obj != ShowEventsObjective.toImport)
            {
                ReadWriteFile RWF = new ReadWriteFile(FileName, vers);
                if (ShowOneDay == true)         // если один день
                    RWF.FillEventslist(EventList, OriginalIndex, Date);
                else                            // если нужны все события
                {
                    RWF.FillEventslist(EventList);
                    for (int i = 0; i < EventList.Count; i++)
                        OriginalIndex.Add(i);
                }
            }
            else    // для импорта
            {
                ReadWriteFile RWF = new ReadWriteFile(ImpFlName, vers);
                RWF.FillEventslist(EventList);
                for (int i = 0; i < EventList.Count; i++)
                    OriginalIndex.Add(i);
            }

            // подогнать размеры
            ChangeFormWidth();

            if (EventList.Count > 0)
            {
                // добавить панели
                EvPnlList = new List<EvPanell>();
                for (int i = 0; i < EventList.Count; i++)
                {
                    // изменить форму и остальные элменты
                    OtherElements(i);
                    // создать i-ю панель
                    CreateEventPanel(i);

                }
                // поместить форму в сердину экрана
                this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width)/2,
                                          (Screen.PrimaryScreen.Bounds.Height - this.Height)/2);
            }
            else
            {
                OtherElements(0);
                lblUnFind.Visible = true;
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
            }

            
            if (ShowOneDay == false)
            {
                lblDate.Left = 25;
                lblType.Left += 37;
                lblEventCapt.Left += 27;
                lblSignal.Left += 27;
                
            }
        }


        // добавить очередную панель
        private void CreateEventPanel(int indx)
        {
            EvPanell EP = new EvPanell();
            int pnlHeight = 46;
            
            // панель
            Panel pn = new Panel();
            pn.Parent = pnlMain;
            pn.Location = new Point(0, indx * (pnlHeight-1));
            if (ShowOneDay == true)
                pn.Size = new Size(350, pnlHeight);
            else
                pn.Size = new Size(379, pnlHeight);
            if (indx % 2 == 0)
                pn.BackColor = SystemColors.Menu;
            else
                //pn.BackColor = SystemColors.Control;
                //pn.BackColor = SystemColors.Info;
                pn.BackColor = SystemColors.MenuBar;
            //pn.BorderStyle = BorderStyle.FixedSingle;
            pn.BorderStyle = BorderStyle.None;
            //pn.Paint += new PaintEventHandler(Panel_Paint);
            EP.MainPanel = pn;
            
            // ChekBox
            CheckBox chb = new CheckBox();
            chb.Parent = pn;
            chb.Location = new Point(8, 16);
            chb.Size = new System.Drawing.Size(15, 14);
            chb.CheckedChanged += new EventHandler(AnyCheckBox_CheckedChanged);
            EP.chb = chb;

            
            Label lbl;
            int baseLeft = 29; //

            // дата
            if (ShowOneDay == true)
                EP.date = null;
            else
            {
                baseLeft += 37;
                lbl = new Label();
                lbl.Parent = pn;
                lbl.Location = new Point(29, 1);
                lbl.Size = new Size(35, 42);
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Text = DateTwoLines(indx);
                EP.date = lbl;
            }


            // тип
            lbl = new Label();
            lbl.Parent = pn;
            lbl.Location = new Point(baseLeft, 16);
            lbl.Size = new Size(14, 13);
            if (EventList[indx].IsEarly == true)
            {
                lbl.Text = "Г";
                lbl.ForeColor = Color.Green;
            }
            else
            {
                lbl.Text = "С";
                lbl.ForeColor = Color.Orange;
            }
            EP.type = lbl;
            

            // текст
            lbl = new Label();
            lbl.Parent = pn;
            lbl.Size = new Size(241, 42);
            lbl.Location = new Point(baseLeft + 20, 1);
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            //lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.Text = EventList[indx].Caption;
            EP.Caption = lbl;

            // время синала
            if (EventList[indx].NeedInform == true)
            {
                lbl = new Label();
                lbl.Parent = pn;
                lbl.Location = new Point(baseLeft + 263, 8);
                lbl.Size = new System.Drawing.Size(58, 13);
                lbl.Text = "за " + BeforeDateToDay(indx).ToString() + " дн.";
                EP.befor = lbl;

                lbl = new Label();
                lbl.Parent = pn;
                lbl.Location = new Point(baseLeft + 263, 24);
                lbl.Size = new System.Drawing.Size(43, 13);
                lbl.Text = "в " + TimeSignal(indx);
                EP.time = lbl;
            }
            else
            {
                lbl = new Label();
                lbl.Parent = pn;
                lbl.Location = new Point(baseLeft + 263, 16);
                lbl.Size = new System.Drawing.Size(58, 13);
                lbl.Text = "нет";
                EP.befor = lbl;

                EP.time = null;
            }
            
            EvPnlList.Add(EP);
        }


        // дата в две строки
        private string DateTwoLines(int indx)
        {
            DateTime DT = EventList[indx].Date;
            string rez = DT.Day.ToString() + ".";
            int M = DT.Month;
            if (M < 10)
                rez += "0";
            rez += M.ToString();
            rez += "\r\n";
            M = DT.Year;
            rez += M.ToString();

            return (rez);
        }

        // остальные элменнты формы
        private void OtherElements(int indx)
        {
            int FormHeightMin = 146;
            int pnlHeight = 46;
            int ButtonTop = 96;

            int k;
            if (indx < 9)
                k = indx;
            else
                k = 8;

            this.Height = FormHeightMin + k * pnlHeight;
            btnExpImpAdd.Top = ButtonTop + k * pnlHeight;
            btnDelete.Top = ButtonTop + k * pnlHeight;
            btnEdit.Top = ButtonTop + k * pnlHeight;
            pnlMain.Height = 46 + k * pnlHeight;

        }


        /// <summary>
        /// возвращает количество дней до напоминания
        /// </summary>
        /// <param name="indx">индекс события</param>
        /// <returns></returns>
        private int BeforeDateToDay(int indx)
        {
            DateTime evDt = EventList[indx].Date.Date;
            DateTime befDt = EventList[indx].BeforeInform.Date;
            TimeSpan ts = evDt - befDt;
            return (ts.Days);
        }


        // время напоминания
        private string TimeSignal(int indx)
        {
            string s;
            s = EventList[indx].BeforeInform.Hour.ToString();
            s += ":";
            int m = EventList[indx].BeforeInform.Minute;
            if (m < 10)
                s += "0";
            s += m.ToString();
            return (s);
        }


        // подогнать размеры формы и элементов
        private void ChangeFormWidth()
        {
            // стандартная ширина формы
            //int frWidth = 354;
            int frWidth = 354;

            // если события не одного дня
            if (ShowOneDay == false)
                frWidth += 30;  // добавить ширину для даты

            // если событий > 8 расширяем под скролл
            if (EventList.Count > 8)
            {
                // использовать сролл
                int scrolwidth = 17;
                this.Width = frWidth + scrolwidth;
                //pnlCaption.Width = frWidth - 5 + scrolwidth;
                pnlCaption.Width = frWidth - 3 + scrolwidth;
                lblCaption.Width = frWidth - 13 + scrolwidth;
                //pnlMain.Width = frWidth - 5 + scrolwidth;
                pnlMain.Width = frWidth - 4 + scrolwidth;
            }
            else
            {
                this.Width = frWidth;
                //pnlCaption.Width = frWidth - 5;
                pnlCaption.Width = frWidth - 3;
                lblCaption.Width = frWidth - 13;
                //pnlMain.Width = frWidth - 5;
                pnlMain.Width = frWidth - 4;
            }
        }


        // выбор/отмена выбора события
        private void AnyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chb = sender as CheckBox;
            if (chb.Checked == true)
            {
                if (obj != ShowEventsObjective.toShow)
                    btnExpImpAdd.Enabled = true;
                else
                {
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
            else
            {
                // если нет отмеченных событий
                if (SomeCheked() == false)
                {

                    if (obj != ShowEventsObjective.toShow)
                        btnExpImpAdd.Enabled = false;
                    else
                    {
                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                }
            }
        }


        // есть ли отмеченные события
        private bool SomeCheked()
        {
            foreach (EvPanell EP in EvPnlList)
            {
                if (EP.chb.Checked == true)
                    return (true);
            }
            return (false);
        }


        // изменить.клик
        private void btnEdit_Click(object sender, EventArgs e)
        {
            List<int> LI = ListOfChekedEvents();
            if (LI.Count > 0)
            {
                // изменяем только одну запись
                int indx = OriginalIndex[LI[0]];
                EditAddEvent newForm = new EditAddEvent(FileName, vers, indx, MnFrm);
                if (newForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    RefreshForm();
                }
            }
        }

        

        // удалить.клик
        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> LI = ListOfChekedEvents();
            if (LI.Count > 0)
            {
                LI.Sort();
                LI.Reverse();
                ReadWriteFile RWF = new ReadWriteFile(FileName, vers);
                for (int i = 0; i < LI.Count; i++)
                    RWF.DeleteEvent(OriginalIndex[LI[i]]);
                
                // обновить форму
                RefreshForm();

                // переопределить таймер
                RestartTimer();
            }
        }

        
        // переопределить и перезап таймер
        private void RestartTimer()
        {
            MnFrm.Invoke(MnFrm.myDelegat);
        }


        // индексы выбранных событий
        private List<int> ListOfChekedEvents()
        {
            List<int> L = new List<int>();
            for(int i = 0; i<EvPnlList.Count; i++)
            {
                if (EvPnlList[i].chb.Checked == true)
                    L.Add(i);
            }
            
            return (L);
        }

        
        // экспорт.клик
        private void btnExp_Click(object sender, EventArgs e)
        {
            // Экспорт
            // индексы выбранных событий
            List<int> ChkLst = ListOfChekedEvents();
            int KolVo = ChkLst.Count;

            // соответствие с оригинальными
            List<int> IndLst = new List<int>();
            foreach (int i in ChkLst)
                IndLst.Add(OriginalIndex[i]);


            ReadWriteFile RWF = new ReadWriteFile(FileName, vers);
            if (RWF.ExportEvents(IndLst) == true)
            {
                //MessageBox.Show("Экспорт успешно завершен.", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show("Экспортировано " + KolVo.ToString() + " события.", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
        
        // иморт.клик
        private void btnImp_Click(object sender, EventArgs e)
        {
            ReadWriteFile RWF = new ReadWriteFile(FileName, vers);
            int KolVo = 0;
            // перебераем все панели
            for (int i = 0; i < EvPnlList.Count; i++)
            {
                // если событие отмечено
                if (EvPnlList[i].chb.Checked == true)
                {
                    RWF.AddEvent(EventList[i]);     // добавляем
                    KolVo++;
                }
            }
            MessageBox.Show("Добавлено " + KolVo.ToString() + " события.", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // переопределить таймер
            RestartTimer();
            this.Close();
        }

        // добавить.клик
        private void btnAdd_Click(object sender, EventArgs e)
        {
            EditAddEvent fr = new EditAddEvent(FileName, vers, Date, MnFrm);
            if (fr.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // обновить форму
                RefreshForm();

                // переопределить таймер
                RestartTimer();
            }
        }


        // обновить форму
        private void RefreshForm()
        {
            // удалить все панели
            if (EvPnlList != null)
            {
                foreach (EvPanell EP in EvPnlList)
                    EP.MainPanel.Dispose();
                EvPnlList.Clear();
                EvPnlList = null;
            }



            // очистить список событий
            EventList.Clear();
            EventList = null;

            // оч сп ориг индексов
            OriginalIndex.Clear();
            OriginalIndex = null;

            // деактивировать кнопки
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            lblUnFind.Visible = false;

            // обновить форму
            ShowEvents_Load(this, new EventArgs());
        }


      

        private void chbAllChek_CheckedChanged(object sender, EventArgs e)
        {
            bool chek = chbAllChek.Checked;
            foreach (EvPanell EP in EvPnlList)
                EP.chb.Checked = chek;
            
        }

      
        

        

        

        


    }
}
