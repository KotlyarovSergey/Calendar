using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using Microsoft.Win32;

namespace Calendar
{

    public struct EventStruct
    {
        public DateTime Date;               // дата
        public byte CapLength;              // длинна описания
        public string Caption;              // описание
        public bool NeedInform;             // необходимость оповещения
        public DateTime BeforeInform;       // дата оповещения
        public bool Informed;               // было ли оповещено
        public bool IsEarly;                // это годовщина
    }

    public enum Diapazon { Day, Month, All };
    public enum ShowEventsObjective { toShow, toExport, toImport };

    public partial class MainForm : Form
    {
        public delegate void RefreshTimer();
        public RefreshTimer myDelegat;

        private List<Label> LabelList;
        private DateTime CheckedDate;
        private DateTime FirstDay;
        private int CurCheckedLabelIndex = -1;

        private List<DateTime> EventsList;      // список дат видимого месяца в которых есть событие
        private List<DateTime> EarlyList;       // список дат видимого месяца в которых есть годовщина
       // private List<EventStruct> EventList;

        //private List<DateTime> EventsListOfMonth;

        private Color ChekColor = Color.LightCoral;
        private string[] month = new string[12] {"Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август",
                                   "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"};

        private string WorkDir;
        private string EventListFile = "eventlist.dat";
        private byte[] vers = { 0, 1, 0, 0 };

        private object CotextMenuControl = null;

        private bool NormalIcon = true;
        
        private DateTime NextSignaltime; // время следующего сигнала
        private int NextEventIndex;      // индекс следующего события

        private bool NeedSignalize = false;
        private bool needBell = false;

        private string SignalText = string.Empty;
        private bool SignalType = false;
        private DateTime EventDay = DateTime.Now;
        private string IconText = string.Empty;

        private int TudeyLabelIndex = -1;

        private bool SkrivatSvernutoe = true;
        


        // ================================================================================
        // ================================================================================

        public MainForm()
        {
            InitializeComponent();
            WorkDir = Application.StartupPath;

            LabelList = new List<Label>(42);
            CheckedDate = DateTime.Now.Date;        // 0:00:00
            
            label8.Text = "      Сегодня: " + CheckedDate.Date.ToShortDateString();

            CreateLabelList();
            CurCheckedLabelIndex = CalcIndexLabelOfDate(CheckedDate);
            LabelList[CurCheckedLabelIndex].BackColor = ChekColor;

            // подписка на именения питания системы
            //SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

            // метод делегата
            myDelegat = new RefreshTimer(ReloadTimer);

            ReloadTimer();
        }



        // перерисовка панели
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawLine(new Pen(new SolidBrush(Color.Black)),
                       new Point(4, 57), new Point(panel1.Width-4, 57));
            //g.DrawRectangle(new Pen(new SolidBrush(Color.Red)),
              //  new Rectangle(3, 192, 25, 18));
            g.Dispose();
        }



        // ================ создать коллекцию лейблов
        private void CreateLabelList()
        {
            Label lbl;
            int wdth = 25;
            int hght = 18;
            Size sz = new Size(wdth, hght);
            for (int h = 0; h < 6; h++)
            {
                for (int w = 0; w < 7; w++)
                {
                    lbl = new Label();
                    lbl.Parent = this.panel1;
                    //lbl.Text = w.ToString() + h.ToString();
                    lbl.Size = sz;
                    lbl.Location = new Point(2 + wdth * w, 60 + (hght + 2) * h);
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Click += new EventHandler(label_Click);
                    lbl.DoubleClick += new EventHandler(Label_DoubleClick);
                    lbl.Cursor = Cursors.Hand;
                    lbl.ContextMenuStrip = contextMenuStrip1;
                    LabelList.Add(lbl);
                }
            }
            
            RefreshMonth();
            CaptionMonth();
        }



        // =============== присвоить коллекции правильные даты
        private void RefreshMonth()
        {
            CalcFirstDay();         // высчитать первый день

            //ReadWriteFile RWF = new ReadWriteFile(WorkDir, vers);
            ReadWriteFile RWF = new ReadWriteFile(WorkDir + "\\" + EventListFile, vers);
            // получить список дат с событиями
            EventsList = RWF.GetAllDateWithEvents(FirstDay);
            // получить список дат с годовщинами
            EarlyList = RWF.GetAllDateWithEarlys(FirstDay);

            DateTime dt;
            int d;
            for (int i = 0; i < 42; i++)
            {
                dt = FirstDay.AddDays(i);
                d = dt.Day;
                LabelList[i].Text = d.ToString();
                if (i < 6)  // первые пн-сб
                {
                    if (d > 6)
                        LabelList[i].ForeColor = Color.Gray;
                    else
                        LabelList[i].ForeColor = Color.Black;
                }
                else if (i > 27)
                {
                    if (d < 15)
                        LabelList[i].ForeColor = Color.Gray;
                    else
                        LabelList[i].ForeColor = Color.Black;
                }

                // есть ли событие
                bool ev = IsEventPresent(dt);
                bool er = IsEarlyPresent(dt);
                if (ev == true && er == true)   
                {
                    // оба
                    LabelList[i].Image = Rsc.event_end_early_img;
                }
                else if (ev == true && er == false)
                {
                    // событие
                    LabelList[i].Image = Rsc.event_img;
                }
                else if (ev == false && er == true)
                {
                    // годовщина
                    LabelList[i].Image = Rsc.early_img;
                }
                else
                {
                    // ничего
                    LabelList[i].Image = null;
                }

            }

            // отметить сегодняшнюю дату
            // раскрасить предыдущий
            if (TudeyLabelIndex >= 0)
                LabelList[TudeyLabelIndex].Image = null;
            // если в отбражаемом месяце есть сегодня
            GetTudeyLabelIndex();
            if (TudeyLabelIndex >= 0)
                LabelList[TudeyLabelIndex].Image = Rsc.redRect;
        }



        // ================ есть ли событие у даты
        private bool IsEventPresent(DateTime dt)
        {
            for (int i = 0; i < EventsList.Count; i++)
            {
                if (EventsList[i] == dt)
                    return (true);
                //if (EventsList[i] > dt)
                  //  return (false);
            }
            return (false);
        }



        // ================ есть ли годовщина у даты
        private bool IsEarlyPresent(DateTime dt)
        {
            // привести к одному году
            dt = new DateTime(2012, dt.Month, dt.Day);

            DateTime dti;
            for (int i = 0; i < EarlyList.Count; i++)
            {
                dti = new DateTime(2012, EarlyList[i].Month, EarlyList[i].Day);
                if (dti == dt)
                    return (true);
               // if (dti > dt)
                 //   return (false);
            }
            return (false);
        }



        // =============== текст месяц год
        private void CaptionMonth()
        {
            lblMonth.Text = month[CheckedDate.Month-1] + " " + CheckedDate.Year.ToString() + "г.";
        }

        
        
        // =============== высчитать первый день
        private void CalcFirstDay()
        {

            DateTime FDThisMonth = new DateTime(CheckedDate.Year, CheckedDate.Month, 1);
            int decrement = 0;
            switch (FDThisMonth.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    decrement = 0;
                    break;
                case DayOfWeek.Tuesday:
                    decrement = -1;
                    break;
                case DayOfWeek.Wednesday:
                    decrement = -2;
                    break;
                case DayOfWeek.Thursday:
                    decrement = -3;
                    break;
                case DayOfWeek.Friday:
                    decrement = -4;
                    break;
                case DayOfWeek.Saturday:
                    decrement = -5;
                    break;
                default:
                    decrement = -6;
                    break;
            }

            FirstDay = FDThisMonth.AddDays(decrement);
        }



        // ================ клик на лейбле
        private void label_Click(object sender, EventArgs e)
        {
            // !!!!!!!! если пред или след мясяц, перескочить
            
            //LabelList.ForEach(labelRePaint);
            // раскрасить предыдущий
            if (CurCheckedLabelIndex >= 0)
                LabelList[CurCheckedLabelIndex].BackColor = 
                    LabelList[CurCheckedLabelIndex].Parent.BackColor;
            
            // определить индекс
            CurCheckedLabelIndex = FindIndexCurentLabel((Label)sender);

            
            // определить дату
            CheckedDate = FirstDay.AddDays(CurCheckedLabelIndex);
            RefreshMonth();
            CaptionMonth();

            // переопределить индекс
            CurCheckedLabelIndex = CalcIndexLabelOfDate(CheckedDate);

            // закрасить
            LabelList[CurCheckedLabelIndex].BackColor = ChekColor;

            CotextMenuControl = LabelList[CurCheckedLabelIndex];
            //((Label)sender).BackColor = Color.DarkOrange;
            //((Label)sender).Text = "f";
        }


        
        // ============= найти индекс лейбла в коллекции
        private int FindIndexCurentLabel(Label lbl)
        {
            for (int i = 0; i < 42; i++)
            {
                if (lbl.Equals(LabelList[i]) == true)
                {
                    return (i);
                }
            }

            return (-1);
        }
        
         
        // ============= нати индекс лейбла для кокретной даты
        private int CalcIndexLabelOfDate(DateTime dt)
        {
            int d = dt.Day;
            // найти первое число
            for (int i = 0; i < 42; i++)
            {
                if (Convert.ToInt32(LabelList[i].Text) == 1)
                {
                    // найти нужное число
                    for (int k = i; k < 42; k++)
                    {
                        if (Convert.ToInt32(LabelList[k].Text) == d)
                            return(k);
                    }
                }
            }
            return (-1);
        }


        // ============= след месяц клик
        private void btnMonthNext_Click(object sender, EventArgs e)
        {
            CheckedDate = CheckedDate.AddMonths(1);
            OtherMonth();

        }



        // ============= предыдущ месяц клик
        private void btnMonthBack_Click(object sender, EventArgs e)
        {
            CheckedDate = CheckedDate.AddMonths(-1);
            OtherMonth();
        }


        // ============= выбор другой даты
        private void OtherMonth()
        {
            RefreshMonth();
            CaptionMonth();

            // раскрасить предыдущий лейбл
            if (CurCheckedLabelIndex >= 0)
                LabelList[CurCheckedLabelIndex].BackColor =
                    LabelList[CurCheckedLabelIndex].Parent.BackColor;

            // выделить тот же день в новом месяце
            TimeSpan ts = CheckedDate - FirstDay;
            int ind = ts.Days;

            LabelList[ind].BackColor = ChekColor;
            CurCheckedLabelIndex = ind;

        }


        // если в отбражаемом месяце есть сегодня
        private void GetTudeyLabelIndex()
        {
            TudeyLabelIndex = -1;
            DateTime DT_Now = DateTime.Now.Date;
            // если первый день месяца больше сегодня, то и остальные больше
            // если последний день месяца меньше сегодня, то и остальные меньше
            if ( FirstDay > DT_Now ||  FirstDay.AddDays(28) < DT_Now)
                return;
            for (int i = 0; i < 42; i++)
            {
                if (FirstDay.AddDays(i) == DT_Now)
                {
                    TudeyLabelIndex = i;
                    break;
                }
            }
            
        }


        // ============= просмотр событий клик
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            // есть ли владелец
            if (CotextMenuControl != null)
            {
                // тип владельца
                Type tp = CotextMenuControl.GetType();
                if (tp == typeof(Label))
                {
                    Label lbl = CotextMenuControl as Label;

                    // определить чего нажали
                    int ind = FindIndexCurentLabel(lbl);
                    if (ind >= 0)
                    {
                        // открыть форму
                        ShowEvents fr = new ShowEvents(WorkDir + "\\" + EventListFile, vers, FirstDay.AddDays(ind), Diapazon.Day, this, ShowEventsObjective.toShow);
                        fr.ShowDialog();
                        RefreshMonth();
                    }
                }

                CotextMenuControl = null;
            }
        }



        // ============= добавить событие клик
        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            // родитлеь
            //ContextMenuStrip cm = sender as ContextMenuStrip;
            //Type tp = cm.Parent.GetType();
            if (CotextMenuControl != null)
            {
                Type tp = CotextMenuControl.GetType();
                if (tp == typeof(Label))
                {
                    //Label lbl = cm.Parent as Label;
                    Label lbl = CotextMenuControl as Label;

                    // определить чего нажали
                    int ind = FindIndexCurentLabel(lbl);
                    if (ind >= 0)
                    {
                        // открыть форму добавления/изменения события
                        //EditAddEvent fr = new EditAddEvent(WorkDir, vers, FirstDay.AddDays(ind));
                        //EditAddEvent fr = new EditAddEvent(WorkDir, vers, FirstDay.AddDays(ind), "", true);
                        EditAddEvent fr = new EditAddEvent(WorkDir + "\\" + EventListFile, vers, FirstDay.AddDays(ind), this);
                        fr.ShowDialog();
                        RefreshMonth();
                    }
                }
                CotextMenuControl = null;
            }
            //EditAddEvent fr = new EditAddEvent(
        }

        
        // ============= удалить событие клик
        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            if (CotextMenuControl != null)
            {
                Type tp = CotextMenuControl.GetType();
                if (tp == typeof(Label))
                {
                    //Label lbl = cm.Parent as Label;
                    Label lbl = CotextMenuControl as Label;

                    // определить чего нажали
                    int ind = FindIndexCurentLabel(lbl);
                    if (ind >= 0)
                    {
                        if (MessageBox.Show("Удалить все события этого дня?", "Удалить", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            ReadWriteFile RWF = new ReadWriteFile(WorkDir + "\\" + EventListFile, vers);
                            RWF.DeleteAllEventFromDay(FirstDay.AddDays(ind));
                            RefreshMonth();
                            ReloadTimer();
                        }
                    }
                }
            }
        }


        // ============ открытие контекстного меню
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip cm = sender as ContextMenuStrip;
            // определить владельца
            CotextMenuControl = cm.SourceControl; //cm.Parent;
            
            label_Click(cm.SourceControl, new EventArgs());
        }


        // Календарь -> просмотр всех.клик
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            ShowEvents nFr = new ShowEvents(WorkDir + "\\" + EventListFile, vers, DateTime.Today, Diapazon.All, this, ShowEventsObjective.toShow);
            nFr.ShowDialog();
            RefreshMonth();
        }


        // определить время следующего сигнала
        private bool DetectNextSignal()
        {
            ReadWriteFile RWF = new ReadWriteFile(WorkDir + "\\" + EventListFile, vers);
            // индекс ближайшего события
            int indx = RWF.NextEvent();
            if (indx >= 0)
            {
                NextEventIndex = indx;
                EventStruct ES = RWF.GetEvent(indx);
                NextSignaltime = ES.BeforeInform;
                //label10.Text = ES.BeforeInform.ToShortDateString() + " " + ES.BeforeInform.ToShortTimeString() + "\r\n" + ES.Caption;
                IconText = NextSignaltime.ToShortDateString() + " " +
                           NextSignaltime.ToShortTimeString() + "\r\n" + ES.Caption;
                if (IconText.Length > 63)
                    IconText = IconText.Substring(0, 63);
                notifyIcon1.Text = IconText;
                return (true);
            }
            return (false);
        }


        // переопределение и перезапуск таймера
        private void ReloadTimer()
        {
            // определить время следующего сигнала
            if (DetectNextSignal() == true)
                // запустить таймер
                MainTimer.Start();
        }
        

        // таймер
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            // если текущее время >= времени события
            DateTime DT_Now = DateTime.Now;
            if (DT_Now >= NextSignaltime)
            {
                // остановить таймер
                MainTimer.Stop();

                // получить структуру события
                ReadWriteFile RWF = new ReadWriteFile(WorkDir + "\\" + EventListFile, vers);
                EventStruct ES = RWF.GetEvent(NextEventIndex);


                SignalText = ES.Caption;
                SignalType = ES.IsEarly;
                EventDay = ES.Date;
                NeedSignalize = true;
                // сигализировать
                //Signalize(ES.Caption, ES.IsEarly, ES.Date);
                needBell = true;
                Signalize();

                // изменить статус события
                ES.Informed = true;
                RWF.EditEvent(NextEventIndex, ES);

                // перезапустить таймер
                ReloadTimer();
            }
            
        }


        // сигнализировать
        private void Signalize()
        {
            // если форма на экране
            if (this.WindowState != FormWindowState.Minimized)
            {
                // показать форму
                SignalForm sf = new SignalForm(SignalText, SignalType, EventDay);
                sf.Show();
                NeedSignalize = false;
                if (needBell == true)
                    Bell();
            }
            else
            {
                if (SkrivatSvernutoe == false)
                {
                    // показать форму
                    SignalForm sf = new SignalForm(SignalText, SignalType, EventDay);
                    sf.Show();
                    NeedSignalize = false;
                    Bell();
                }
                else
                {
                    // поморгать значком
                    TrayTimer.Start();
                    notifyIcon1.ShowBalloonTip(3000, "", " Дзынь! ", ToolTipIcon.None);
                    needBell = false;
                    Bell();
                }
            }

        }


        // звуковой сигнал
        private void Bell()
        {
            System.Media.SoundPlayer pleer = new System.Media.SoundPlayer(Rsc.bell);
            pleer.Play();
            pleer.Dispose();
        }


        // таймер смены картинки в трее
        private void TrayTimer_Tick(object sender, EventArgs e)
        {
            if (NormalIcon == true)
                //notifyIcon1.Icon = Rsc.TrayIcoRed;
                notifyIcon1.Icon = Rsc.TrayIconRight;
            else
                //notifyIcon1.Icon = Rsc.TrayIcoNorm;
                notifyIcon1.Icon = Rsc.TrayIconLeft;

            NormalIcon = !NormalIcon;
        }


        // изменения размеров формы
        private void Form1_Resize(object sender, EventArgs e)
        {
            // свернуть окно
            if (SkrivatSvernutoe == true)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    notifyIcon1.Visible = true;
                    notifyIcon1.Text = IconText;
                    this.Hide();
                }
            }
        }


        // даблклик по иконке
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            TrayTimer.Stop();
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
            notifyIcon1.Icon = Rsc.TrayIconNorm;
            if (NeedSignalize == true)
                Signalize();
            // текущий месяц
            CheckedDate = DateTime.Now.Date;
            OtherMonth();
        }


        // клик по всплывающему окну
        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            // тоже что и даблклик
            notifyIcon1_DoubleClick(sender, e);
        }


        // Экспорт всех событий
        private void ExportAllEvenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.AddExtension = true;
            SFD.DefaultExt = "cln";
            SFD.Filter = "Calendar files (*.cln)|*.cln|All files (*.*)|*.*";
            SFD.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            string SrcFile = WorkDir + "\\" + EventListFile;
            if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                System.IO.File.Copy(SrcFile, SFD.FileName, true);
            SFD.Dispose();
        }

        
        // Экспорт некоторых событий
        private void ExportAnyEventsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowEvents nFr = new ShowEvents(WorkDir + "\\" + EventListFile, vers, DateTime.Today, Diapazon.All, this, ShowEventsObjective.toExport);
            nFr.ShowDialog();
            //RefreshMonth();
        }

        
        // импорт
        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            // выбрать файл
            string InpFile;
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Calendar files (*.cln)|*.cln|All files (*.*)|*.*";
            OFD.FilterIndex = 0;
            OFD.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            if (OFD.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            else
                InpFile = OFD.FileName;
            OFD.Dispose();

            // отразить список событий
            ShowEvents nFr = new ShowEvents(WorkDir + "\\" + EventListFile, InpFile, vers, this, ShowEventsObjective.toImport);
            nFr.ShowDialog();

            RefreshMonth();
            //ReloadTimer(); делается из ShowEvents формы
        }


        // скрывать свернутое.клик
        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            SkrivatSvernutoe = toolStripMenuItem7.Checked;
        }


        // поверх остальных окон.клик
        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            this.TopMost = toolStripMenuItem8.Checked;
        }



        // двойной клик по лейблу
        private void Label_DoubleClick(object sender, EventArgs e)
        {
            CotextMenuControl = sender as Control;
            // показать
            toolStripMenuItem9_Click(toolStripMenuItem9, new EventArgs());
        }


       
        

        // =========================================================================================
        // =========================================================================================
        // =========================================================================================

        
        
        
        
        
        

        

       
        


        

    }
}
