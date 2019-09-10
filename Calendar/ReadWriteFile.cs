using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
//using Calendar;

namespace Calendar
{
    class ReadWriteFile
    {
        //string FileName;
        List<EventStruct> EventsList;
        //private string WorkDirectory;
        private string fileName;
        private char[] typeFile = { 'c', 'l', 'd', '.' };
        private byte[] vers = { 0, 0, 0, 0 };

        
        // конструктор
        //public ReadWriteFile(string WorkDir, byte[] vr)
        public ReadWriteFile(string fName, byte[] vr)
        {
            //WorkDirectory = WorkDir;
            fileName = fName;

            for (int i = 0; i < vr.Length; i++)
                vers[i] = vr[i];
            
            EventsList = new List<EventStruct>();
            ReadFile();
            // создать компаратор
            EventStructComparer ESCmp = new EventStructComparer();
            // сравнить
            EventsList.Sort(ESCmp);
        }


        // добавить событие
        public void AddEvent(EventStruct ES)
        {
            EventsList.Add(ES);
            WriteFile();
        }

        
        // удалить событие номер
        public void DeleteEvent(int indx)
        {
            EventsList.RemoveAt(indx);
            WriteFile();
        }


        /// <summary>
        /// изменить событие
        /// </summary>
        /// <param name="indx"></param>
        /// <param name="ES"></param>
        public void EditEvent(int indx, EventStruct ES)
        {
            EventsList[indx] = ES;
            WriteFile();
        }



        /// <summary>
        /// возвращает событие с указанным номером
        /// </summary>
        /// <param name="indx">номер события</param>
        /// <returns>null - если такого индекса нет</returns>
        public EventStruct GetEvent(int indx)
        {
            EventStruct ES = new EventStruct();
            try
            {
                ES = EventsList[indx];
            }
            catch
            {
                ES.Date = DateTime.Today;
                ES.Caption = "Событие не найдено!!!";
                ES.BeforeInform = DateTime.Today;
                ES.CapLength = (byte)ES.Caption.Length;
                ES.Informed = false;
                ES.NeedInform = false;
                ES.IsEarly = false;
            }
            return (ES);
        }


        /// <summary>
        /// Заполняет список всех событий
        /// </summary>
        /// <param name="esl">список</param>
        public void FillEventslist(List<EventStruct> esl)
        {
            foreach (EventStruct ES in EventsList)
            {
                esl.Add(ES);
            }
        }

        
        /// <summary>
        /// Заполняет список событий для указанной даты
        /// </summary>
        /// <param name="esl">список</param>
        /// <param name="OrigIndex">список для оригинальных индексов</param>
        /// <param name="OneDay">дата</param>
        public void FillEventslist(List<EventStruct> esl, List<int> OrigIndex, DateTime OneDay)
        {
            for (int i = 0; i < EventsList.Count; i++)
            {
                if (EventsList[i].Date.Day == OneDay.Day)
                {
                    esl.Add(EventsList[i]);
                    OrigIndex.Add(i);
                }
            }
        }


        /// <summary>
        /// Заполняет список событий для диапазона
        /// </summary>
        /// <param name="esl">спиок</param>
        /// <param name="FirstDay">первый день</param>
        /// <param name="DaysCount">количество дней</param>
        public void FillEventslist(List<EventStruct> esl, DateTime FirstDay, int DaysCount)
        {


        }


        /// <summary>
        /// Возвращает список всех дат имеющих Событие начиная с указанной даты
        /// </summary>
        /// <param name="FirstDay">первый день</param>
        /// <returns></returns>
        public List<DateTime> GetAllDateWithEvents(DateTime FirstDay)
        {
            DateTime LastDay = FirstDay.AddDays(42);
            List<DateTime> LD = new List<DateTime>();
            foreach (EventStruct ES in EventsList)
            {
                if (ES.IsEarly == false)
                {
                    if (ES.Date.Date >= FirstDay && ES.Date.Date <= LastDay)
                        LD.Add(ES.Date.Date);
                }
            }
            return (LD);
        }



        /// <summary>
        /// Возвращает список всех дат имеющих годовщину начиная с указанной даты
        /// </summary>
        /// <param name="FirstDay">первый день</param>
        /// <returns></returns>
        public List<DateTime> GetAllDateWithEarlys(DateTime FirstDay)
        {
            DateTime LastDay = FirstDay.AddDays(42);
            List<DateTime> LD = new List<DateTime>();
            foreach (EventStruct ES in EventsList)
            {
                if (ES.IsEarly == true)
                {
                    if (ES.Date.Date >= FirstDay && ES.Date.Date <= LastDay)
                        LD.Add(ES.Date.Date);
                }
            }
            return (LD);
        }



        /// <summary>
        /// Возвращает индекс ближайшего (или первого из прошедших) события
        /// </summary>
        /// <returns></returns>
        public int NextEvent()
        {
            int rez = -1;
            // если события вообще есть
            if (EventsList.Count > 0)
            {
                DateTime today = DateTime.Now;
                // ищем первое событие с положительным NeedInform
                for (int i = 0; i < EventsList.Count; i++)
                {
                    if (EventsList[i].NeedInform == true)
                    {
                        // если это не годовщина и уже сигнализированное событие
                        if (EventsList[i].IsEarly == false && EventsList[i].Informed == true)
                            continue;   // пропускаем

                        // время сигнала
                        DateTime DT_i = EventsList[i].BeforeInform;
                        
                        // если это годовщина, надо привести к текущему году
                        if (EventsList[i].IsEarly == true)
                        {
                            DT_i = new DateTime(today.Year, DT_i.Month, DT_i.Day, DT_i.Hour, DT_i.Minute, 0);
                            // если годовщина, но позже сегодня, то пропускаем
                            if (DT_i < today)
                                continue;
                        }
                        
                        
                        // первое из подходящих событий
                        rez = i;
                        if (i == EventsList.Count)
                            return (rez);   // последнее событие в списке
                        
                        
                        // просматриваем остальные 
                        DateTime DT_j;
                        for (int j = i + 1; j < EventsList.Count; j++)
                        {
                            if (EventsList[j].NeedInform == true)
                            {
                                // если это не годовщина и уже сигнализированное событие
                                if (EventsList[j].IsEarly == false && EventsList[j].Informed == true)
                                    continue;   // пропускаем


                                // сравнить очередное событие с пока наираньшим
                                DT_j = EventsList[j].BeforeInform;

                                // если годовщина привести к текущему году
                                if (EventsList[j].IsEarly == true)
                                {
                                    DT_j = new DateTime(today.Year, DT_j.Month, DT_j.Day, DT_j.Hour, DT_j.Minute, 0);
                                    // если годовщина, но позже сегодня, то пропускаем
                                    if (DT_j < today)
                                        continue;
                                }   
                                // сравнить
                                if (DT_j < DT_i)
                                {
                                    rez = j;
                                    DT_i = EventsList[j].BeforeInform;
                                }
                            }
                        }
                        // если кончился цикл, значит другого не нашли
                        return (rez);
                    }
                }
                // rez так и останется -1
            }
            return (rez);
        }



        /// <summary>
        /// Экспорт списка событий
        /// </summary>
        /// <param name="IndList">список идексов необходимых событий</param>
        /// <returns>true в </returns>
        public bool ExportEvents(List<int> IndList)
        {
            string workfile;
            System.Windows.Forms.SaveFileDialog SFD = new System.Windows.Forms.SaveFileDialog();
            SFD.AddExtension = true;
            SFD.DefaultExt = "cln";
            SFD.Filter = "Calendar files (*.cln)|*.cln|All files (*.*)|*.*";
            SFD.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            //string SrcFile = WorkDirectory + "\\eventlist.dat";
            if (SFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                workfile = SFD.FileName;
            else
                return (false);
            SFD.Dispose();
            

            FileStream FS;
            try
            {
                FS = new FileStream(workfile, FileMode.Create, FileAccess.Write);
            }
            catch
            {
                return(false);
            }

            // тип
            for (int i = 0; i < 4; i++)
                FS.WriteByte((byte)typeFile[i]);

            // версия
            FS.Write(vers, 0, 4);

            // кол-во
            int count = IndList.Count;
            byte[] buf = BitConverter.GetBytes(count);
            FS.Write(buf, 0, 4);


            int PointsOffset = 12;  // начало перечисления смещений
            //int DataOffset = PointsOffset + 4 * count;

            // массив смещений от начала файла забиваем нолями
            for (int i = 0; i < count * 4; i++)
                FS.WriteByte(0);


            // перебераем все события списка
            for (int i = 0; i < count; i++)
            {
                // i-е смещение
                FS.Seek(PointsOffset + i * 4, SeekOrigin.Begin);
                buf = BitConverter.GetBytes((int)FS.Length);
                FS.Write(buf, 0, 4);

                // событие в массив байт
                //buf = EventStructToByteArray(i);
                buf = EventStructToByteArray(IndList[i]);

                // дописать
                FS.Seek(0, SeekOrigin.End);
                FS.Write(buf, 0, buf.Length);
            }

            FS.Close();

            return (true);
        }



        /// <summary>
        /// удалить все события указанного дня
        /// </summary>
        /// <param name="day"></param>
        public void DeleteAllEventFromDay(DateTime day)
        {
            bool f = true;
            int count = 0;
            while (f == true)
            {
                f = false;
                foreach (EventStruct ES in EventsList)
                {
                    if (ES.Date.Date == day)
                    {
                        EventsList.Remove(ES);
                        f = true;
                        count++;
                        break;
                    }
                }
            }
            if (count > 0)
                WriteFile();
        }





        // ========== PRIVATE =========

        // прочитать файл
        private void ReadFile()
        {
            //string fn = WorkDirectory + "\\eventlist.dat";
            //if (File.Exists(fn) == false)
            if (File.Exists(fileName) == false)
                return;
            
            FileStream FS;
            try
            {
                FS = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return;
            }

            // тип

            // версия

            // кол-во
            FS.Seek(8, SeekOrigin.Begin);

            byte[] buf = new byte[4];
            FS.Read(buf, 0, 4);
            int count = BitConverter.ToInt32(buf, 0);
            long offset;
            EventStruct ES;
            for (int i = 0; i < count; i++)
            {
                ES = new EventStruct();

                // смещение очердного события
                FS.Seek(12 + i * 4, SeekOrigin.Begin);
                FS.Read(buf, 0, 4);
                offset = Convert.ToInt64(BitConverter.ToInt32(buf, 0));

                // передвигаемся на начало i-й структуры
                FS.Seek(offset, SeekOrigin.Begin);
                
                // дата
                buf = new byte[8];
                FS.Read(buf, 0, 8);
                long tickcount = BitConverter.ToInt64(buf, 0);
                ES.Date = new DateTime(tickcount);

                // длинна заголовка
                byte B =(byte)FS.ReadByte();
                ES.CapLength = B;               // кол-во символов

                // заголовок
                buf = new byte[B * 2];          // т.к юникод 1 симв = 2 байт
                FS.Read(buf, 0, B * 2); ;
                string S = Encoding.Unicode.GetString(buf);
                ES.Caption = S;

                // необходимость оповещения
                B = (byte)FS.ReadByte();
                ES.NeedInform = Convert.ToBoolean(B);

                // время сигнала
                buf = new byte[8];
                FS.Read(buf, 0, 8);
                tickcount = BitConverter.ToInt64(buf, 0);
                ES.BeforeInform = new DateTime(tickcount);

                // был ли сигнал
                B = (byte)FS.ReadByte();
                ES.Informed = Convert.ToBoolean(B);

                // типо события
                B = (byte)FS.ReadByte();
                ES.IsEarly = Convert.ToBoolean(B);

                // добавить событие в список
                EventsList.Add(ES);
            }

            FS.Close();
        }


        // записать список событий в файл
        private void WriteFile()
        {
            //string workfile = WorkDirectory + "\\eventlist.dat";
            string workfile = fileName;
            //string tmpfile = WorkDirectory + "\\eventlist.dat.tmp";
            string tmpfile = fileName + ".tmp";

            FileStream FS;
            try
            {
                //FS = new FileStream(tmpfile, FileMode.Create, FileAccess.ReadWrite);
                FS = new FileStream(tmpfile, FileMode.Create, FileAccess.Write);
            }
            catch
            {
                return;
            }

            // тип
            for (int i = 0; i < 4; i++)
                FS.WriteByte((byte)typeFile[i]);

            // версия
            FS.Write(vers, 0, 4);

            // кол-во
            int count = EventsList.Count;
            byte[] buf = BitConverter.GetBytes(count);
            FS.Write(buf, 0, 4);

            
            int PointsOffset = 12;  // начало перечисления смещений
            //int DataOffset = PointsOffset + 4 * count;
            
            // массив смещений от начала файла забиваем нолями
            for (int i = 0; i < count * 4; i++)
                FS.WriteByte(0);


            // перебераем все события списка
            for (int i = 0; i < count; i++)
            {
                // i-е смещение
                FS.Seek(PointsOffset + i * 4, SeekOrigin.Begin);
                buf = BitConverter.GetBytes((int)FS.Length);
                FS.Write(buf, 0, 4);

                // событие в массив байт
                buf = EventStructToByteArray(i);

                // дописать
                FS.Seek(0, SeekOrigin.End);
                FS.Write(buf, 0, buf.Length);
            }

            FS.Close();

            try
            {
                File.Delete(workfile);
            }
            catch
            {
                return;
            }

            File.Move(tmpfile, workfile);
        }

        
        // i-ю структуру в массив байт
        private byte[] EventStructToByteArray(int indx)
        {
            int StructLen;
            int pos;

            // длинна структуры
            StructLen = 20 + EventsList[indx].CapLength * 2;
            // массив под структуру
            byte[] rezult = new byte[StructLen];
            pos = 0;
            // дата
            byte[] buf = BitConverter.GetBytes(EventsList[indx].Date.Ticks);
            Array.Copy(buf, rezult, 8);
            pos = 8;

            // длинна заголовка
            byte b = Convert.ToByte(EventsList[indx].CapLength);
            rezult[pos] = b;
            pos++;

            // заголовок
            //buf = Encoding.ASCII.GetBytes(EvStrList[indx].Caption);
            buf = Encoding.Unicode.GetBytes(EventsList[indx].Caption);
            Array.Copy(buf, 0, rezult, pos, buf.Length);
            pos += buf.Length;

            // необходимость оповещения
            b = Convert.ToByte(EventsList[indx].NeedInform);
            rezult[pos] = b;
            pos++;

            // дата оповещения
            buf = BitConverter.GetBytes(EventsList[indx].BeforeInform.Ticks);
            Array.Copy(buf, 0, rezult, pos, 8);
            pos += 8;

            // был ли сигнал
            b = Convert.ToByte(EventsList[indx].Informed);
            rezult[pos] = b;
            pos++;

            // тип события
            b = Convert.ToByte(EventsList[indx].IsEarly);
            rezult[pos] = b;

            return (rezult);
        }

       
    }
}
