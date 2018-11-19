using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace war
{
    public partial class War : Form
    {
        int x = 880;
        int y = 839;
        bool started = false;
        int time = 2;
        bool error = false;
        private volatile bool _pause = false;

        public War()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Выбор типа размножения
        private void rbGOmo_CheckedChanged(object sender, EventArgs e)
        {

            Specimen.typeOfReproduse = 1;
        }

        private void rbGet_CheckedChanged(object sender, EventArgs e)
        {

            Specimen.typeOfReproduse = 2;
        }

        private void rbBoth_CheckedChanged(object sender, EventArgs e)
        {
            Specimen.typeOfReproduse = 3;
        }

        //Выбор типа передачи
        private void rbGen_CheckedChanged(object sender, EventArgs e)
        {
            Ill.typePass = 1;
        }

        private void rbAir_CheckedChanged(object sender, EventArgs e)
        {
            Ill.typePass = 3;
        }

        private void rbCont_CheckedChanged(object sender, EventArgs e)
        {
            Ill.typePass = 2;
        }

        private void rbSex_CheckedChanged(object sender, EventArgs e)
        {
            Ill.typePass = 4;
        }
        //Данная функция отвечает за работу кнопки Старт. Если можель уже запущена и была нажата кнопка Пауза - 
        // можель просто начинает работу, иначе она считывает данные, генерирует популяцию и заражает ее
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!started)
            {
                try
                { LoadData();
                    error = false;
                    started = true;
                }
                catch
                {
                    MessageBox.Show("Где-то неверные данные.");
                    error = true;
                }
            }

            if (!error)
            {
                Task.Run(() =>
                {
                    _pause = false;

                    Play();
                });
                btnDel.Enabled = false;
                btnStatistic.Enabled = false;
            }

        }
        //Данная функция считывает некоторые данные с экрана, генерирует популяцию и болезнь, и блокирует ввод новых данных
        void LoadData()
        {
            int nIll = Convert.ToInt32(tbIllSp.Text);
            int nSP = Convert.ToInt32(tbSize.Text);
            if (nIll > nSP)
            {
                MessageBox.Show("Число зараженных особей больше размера популяции. \n Будут заражены ВСЕ особи.");
            }
            GenereteSpecimen();
            GenereteIll();
            if (time != 0)
                time = Convert.ToInt32(1000 / time);
            else
                time = 200;

            Drugs.strong = Convert.ToInt32(tbDrugs.Text);           
            panel2.Enabled = false;
            tbSize.Enabled = false;
            tbIllSp.Enabled = false;

        }
        //Данная функция считывает данные о популяции с экрана и генерирует популяцию
        public void GenereteSpecimen()//Генерируем популяцию
        {
            int nSP = Convert.ToInt32(tbSize.Text);//размер популяции
            int immun = tbMidImunn.Value;//средний иммунитет
            int spImmun = Convert.ToInt32(tbi.Text);//разброс иммунитета
            int maxAge = tbMidMaxAge.Value;//максимальный возраст особи
            int spMaxAge = Convert.ToInt32(tbma.Text);//разброс максимального возраста
            int repDist = tbRepDist.Value;//расстояние для размножения
            int health = tbMidHealth.Value;//здоровье
            int spHealth = Convert.ToInt32(tbh.Text);//разброс здоровья
            int countRep = Convert.ToInt32(tbr.Text);//кол-во спариваний в год
            int muteChanse = tbMuteChansS.Value;//шанс мутации при появлении новой особи
            int spMuteChanse = Convert.ToInt32(tbmcs.Text);////разброс шанса мутаций
            int speed = Convert.ToInt32(tbspeed.Text);//скорость
            int spSbeed = Convert.ToInt32(tbspedrasb.Text);
            Life.fx = x;
            Life.fy = y;

            for (int i = 0; i < nSP; i++)
            {
                Specimen newS;
                Random r = new Random();
                int nI = immun + r.Next(-spImmun, spImmun + 1);
                int nMA = (maxAge + r.Next(-spHealth, spHealth + 1)) * 12;
                int nH = health + r.Next(-spHealth, spHealth + 1);
                int nMC = muteChanse + r.Next(-spMuteChanse, spMuteChanse + 1);
                int nSpeed = speed + r.Next(-spSbeed, spSbeed + 1);
                int sex;
                int nrd = tbRepDist.Value;
                if (Specimen.typeOfReproduse == 1)
                    sex = 3;
                else
                    sex = r.Next(1, 3);
                int nx = r.Next(0, x);
                int ny = r.Next(0, y);
                if (nI < 1)
                    nI = 1;
                if (nMA < 1)
                    nMA = 1;
                if (nH < 1)
                    nH = 1;
                if (nMC < 1)
                    nMC = 1;
                newS = new Specimen(nI, nMA, sex, nH, nx, ny, nMC, nrd, nSpeed);
                newS.maxWay = nrd;
                Life.LS.Add(newS);
                Random r1 = new Random();
                int t = r1.Next(1, 50);
                System.Threading.Thread.Sleep(t);//чтобы генератор успел змениться
            }
        }

        //Данная функция считывает данные о болезни с экрана и генерирует болезни и первые nIll особей заражаются болезнью)
        public void GenereteIll()//Генерируем болезни
        {
            int deadly = tbDead.Value;//смертоносность
            int spDeadly = Convert.ToInt32(tbd.Text);//разброс смртоносности
            int muteC = tbMuteChanseIll.Value;//мутация при передаче
            int spMuteC = Convert.ToInt32(tbmci.Text);//разброс мутации
            int con = tbCon.Value;//заразность
            int spCon = Convert.ToInt32(tbc.Text);//разброс заразности
            int strong = tbStrong.Value;//сила
            int spStrong = Convert.ToInt32(tbs.Text);//разброс силы
            int nIll = Convert.ToInt32(tbIllSp.Text);//кол-во зараженных
            int passD = tbDistInfect.Value;//расстояние передачи
            int spPassD = Convert.ToInt32(tbpd.Text);//разброс расстояния

            int nSP = Convert.ToInt32(tbSize.Text);//если пользователь хочет заразить больше особей, чем есть  - заражаем всех
            if (nIll > nSP)
                nIll = nSP;

            for (int i = 0; i < nIll; i++)
            {
                Random r = new Random();
                int ndeadly = deadly + r.Next(-spDeadly, spDeadly + 1);
                if (ndeadly < 0)
                    ndeadly = 0;
                int nMuteC = muteC + r.Next(-spMuteC, spMuteC + 1);
                if (nMuteC < 1)
                    nMuteC = 1;
                int ncon = con + r.Next(-spCon, spCon + 1);
                if (ncon < 1)
                    ncon = 1;
                int nstrong = strong + r.Next(-spStrong, spStrong + 1);
                if (nstrong < 1)
                    nstrong = 1;
                int nPassDist = passD + r.Next(-spPassD, spPassD + 1);
                if (nPassDist < 1)
                    nPassDist = 1;

                Ill newIll = new Ill();
                newIll.deadly = ndeadly;
                newIll.MutateChanse = nMuteC;
                newIll.contagation = ncon;
                newIll.strong = nstrong;
                newIll.passDict = nPassDist;
                Life.LI.Add(newIll);
                Life.LS[i].ill = newIll;
                Life.LS[i].isSick = true;
                Random r1 = new Random();
                int t = r1.Next(1, 50);
                System.Threading.Thread.Sleep(t);//чтобы генератор успел змениться
            }
        }
        private void Field_Paint(object sender, PaintEventArgs e)
        {

        }
        //Кнопка пауза, позволяет очистить модель и сохранить статистику
        private void btnPause_Click(object sender, EventArgs e)
        {
            _pause = true;
            btnDel.Enabled = true;
            btnStatistic.Enabled = true;
        }
        //Основная функция работы модели. Отрисовывает особей и вызывает итеративные изменения
        public void Play()
        {
            Brush black = System.Drawing.Brushes.Black;
            Brush green = System.Drawing.Brushes.Green;
            Brush red = System.Drawing.Brushes.Red;
            Brush browm = System.Drawing.Brushes.Brown;
            Brush blue = System.Drawing.Brushes.DodgerBlue;
            Brush mBlue = System.Drawing.Brushes.MidnightBlue;
            Brush magenta = System.Drawing.Brushes.Magenta;
            Brush dMagente = System.Drawing.Brushes.DarkMagenta;
            Graphics g = pictureBox1.CreateGraphics();
            if (time != 0)
                time = Convert.ToInt32(1000 / time);
            else
                time = 200;
            while (_pause == false)
            {
                g.Clear(Color.Black);
                for (int i = 0; i < Life.LS.Count; i++)
                {
                    if (Life.LS[i].live)
                    {
                        Brush rC = magenta;
                        int sex = Life.LS[i].sex;
                        switch (sex)
                        {
                            case 1:
                                if (Life.LS[i].isSick)
                                { rC = browm; }
                                else
                                { rC = red; }
                                break;
                            case 2:
                                if (Life.LS[i].isSick)
                                {
                                    rC = mBlue;
                                }
                                else
                                {
                                    rC = blue;
                                }
                                break;
                            case 3:
                                if (Life.LS[i].isSick)
                                {
                                    rC = magenta;
                                }
                                else
                                {
                                    rC = dMagente;
                                }
                                break;
                        }
                        int x1 = Life.LS[i].x - Specimen.radius;
                        int x2 = Life.LS[i].x + Specimen.radius;
                        int y1 = Life.LS[i].y - Specimen.radius;
                        int y2 = Life.LS[i].y + Specimen.radius;
                        int d = Specimen.radius * 2;
                        g.FillEllipse(rC, x1, y1, d, d);
                    }
                }
                Life.Iteration();
                PrintSt();
            }
        }
        //Кнопка, которая отвечает за лечение болезни лекарствами
        private void btnDrugs_Click(object sender, EventArgs e)
        {
            Drugs.strong = Convert.ToInt32(tbDrugs.Text);
            Life.TreatDrugs();
        }
        //Кнопка отвечающая за стирание данных
        private void btnDel_Click(object sender, EventArgs e)
        {
            _pause = true;
            panel1.Enabled = true;
            panel2.Enabled = true;
            tbSize.Enabled = true;
            tbIllSp.Enabled = true;
            Life.LI.Clear();
            Life.LS.Clear();
            Life.SI = new StatisticIll();
            Life.SS = new StatisticSpisemen();
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.Black);
            started = false;
            Life.SI = new StatisticIll();
            Life.SS= new StatisticSpisemen();
        }
        //Кнопка, которая сохраняет статистику в xslx файлы
        private void btnStatistic_Click(object sender, EventArgs e)
        {
            Life.SI.SaveStatistic();
            Life.SS.SaveStatistic();
        }
        //Функция, отвечающая за вывод текущих средних значений по популяции и болезни на экран
        public void PrintSt()
        {
            int n = Life.SS.health.Count() - 1;

            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                label19.Text = "Средний иммунитет = " + Life.SS.immun[n];
                label20.Text = "Среднее здоровье  = " + Life.SS.health[n];
                label21.Text = "Частота мутаций = " + Life.SS.MutateChanse[n];
                label27.Text = "Всего особей = " + Life.SS.number[n];
                label28.Text = "Заражено особей = " + Life.SS.isSick[n];

                n = Life.SI.deadly.Count() - 1;
                label22.Text = "Смертаность = " + Life.SI.deadly[n];
                label23.Text = "Заразность = " + Life.SI.contagation[n];
                label24.Text = "Сила = " + Life.SI.strong[n];
                label25.Text = "Частота мутаций = " + Life.SI.MutateChanse[n];
                label26.Text = "Расстояние передачи = " + Life.SI.passDict[n];
            }
            catch (IndexOutOfRangeException)
            { }
        }

        private void label26_Click(object sender, EventArgs e)
        {

        }
    }
}
