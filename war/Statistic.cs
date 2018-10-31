using System.Collections.Generic;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace war
{
    abstract class Statistic
    {
        public List<int> number = new List<int>();
        abstract public void GetStatistic();
        abstract public void SaveStatistic();
        abstract public void PrintStatistic();
    }

    class StatisticSpisemen : Statistic
    {
        public List<double> immun = new List<double>();//иммунитет. Влияет на шанс не заболеть и вылечиться       
        public List<double> maxAge = new List<double>();//Максимальный возраст особи (в месяцах)
        public List<double> age = new List<double>();//Текущий возраст особи (в месяцах)
        public List<double> health = new List<double>();//здоровье. при достижении нуля - умирает
        public List<int> isSick = new List<int>();//болеет
        public List<double> maxWay = new List<double>();//максимальный путь, который пройдет особь за итерацию
        public List<double> MutateChanse = new List<double>();//шанс мутации новой особи

        
        override public void  GetStatistic()
        {
            List<Specimen> LS = Life.LS;
            double mImmun = 0;
            double mMaxAge = 0 ;
            double mAge = 0;
            double mHealth = 0;
            int cSick = 0 ;
            double mMaxWay = 0;
            double mMutaeChanse = 0;
            //считываем данные
            int n = LS.Count();
            foreach (Specimen o in LS)
            {
                
                if (o.live == true)
                {
                    mImmun += o.immun + o.dopimmun;
                    mMaxAge += o.maxAge;
                    mAge += o.age;
                    mHealth += o.health;
                    if (o.isSick)
                        cSick++;
                    mMaxWay += o.maxWay;
                    mMutaeChanse += o.MutateChanse;
                }
                else
                { n--; }
            }
            
            //считаем среднее
            mImmun = mImmun / n;
            mMaxAge = mMaxAge / n;
            mAge = mAge / n;
            mHealth = mHealth/n;
            mMaxWay = mMaxWay / n;
            mMutaeChanse = mMutaeChanse / n;
            //сохраняем данные
            immun.Add(mImmun);
            maxAge.Add(mMaxAge);
            age.Add(mAge);
            health.Add(mHealth);
            isSick.Add(cSick);
            maxWay.Add(mMaxWay);
            MutateChanse.Add(mMutaeChanse);
            number.Add(n);
        }
        override public void SaveStatistic()
        {
            // Создаём экземпляр нашего приложения
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = false;
            excelApp.Workbooks.Add();
            excelApp.DisplayAlerts = false;
            Excel.Worksheet workSheet = excelApp.ActiveSheet;
            workSheet.EnableSelection = Microsoft.Office.Interop.Excel.XlEnableSelection.xlNoSelection;
            //Создаём экземпляр листа Excel

                workSheet.Cells[1, 1] = "Размер популяции";
                for (int j = 2; j <= number.Count() + 1; j++)
                {
                    workSheet.Cells[1, j] = number[j - 2];
                }

                workSheet.Cells[2, 1] = "Ср иммунитет";
                for (int j = 2; j <= immun.Count() + 1; j++)
                {
                    workSheet.Cells[2, j] = immun[j - 2];
                }

                workSheet.Cells[3, 1] = "Ср Максимальный возраст";
                for (int j = 2; j <= maxAge.Count() + 1; j++)
                {
                    workSheet.Cells[3, j] = maxAge[j - 2];
                }

                workSheet.Cells[4, 1] = "Ср возраст";
                for (int j = 2; j <= age.Count() + 1; j++)
                {
                    workSheet.Cells[4, j] = age[j - 2];
                }
                workSheet.Cells[5, 1] = "Ср здоровье";
                for (int j = 2; j <= health.Count() + 1; j++)
                {
                    workSheet.Cells[5, j] = health[j - 2];
                }
                workSheet.Cells[6, 1] = "Больных";
                for (int j = 2; j <= isSick.Count() + 1; j++)
                {
                    workSheet.Cells[6, j] = isSick[j - 2];
                }
                workSheet.Cells[7, 1] = "Ср макс путь";
                for (int j = 2; j <= maxWay.Count() + 1; j++)
                {
                    workSheet.Cells[7, j] = maxWay[j - 2];
                }
                workSheet.Cells[8, 1] = "Ср шан мутации";
                for (int j = 2; j <= MutateChanse.Count() + 1; j++)
                {
                    workSheet.Cells[8, j] = MutateChanse[j - 2];
                }


            excelApp.DisplayAlerts = false;
            workSheet.SaveAs(string.Format(@"{0}\Статистика_популяция.xlsx", System.Environment.CurrentDirectory));

            excelApp.Quit();


        }
        override public void PrintStatistic()
        {
            
        }
    }

    class StatisticIll : Statistic
    {
        public List<double> deadly = new List<double>();//смертоносность, шанс убить носителя (сколько здоровья отнимает на итерации)
        public List<double> contagation = new List<double>();//заразность, шанс передасться
        public List<double> strong = new List<double>();//Сила, сопротивление лечению и выздоровлению
        public List<double> passDict = new List<double>();//расстояние. на котором передается болезнь при передаче воздушно-капельным путем
        public List<double> MutateChanse = new List<double>();//шанс мутации болезни

        override public void GetStatistic()
        {
            List<Ill> LI = Life.LI;
            double mDeadly = 0;
            double mCont = 0;
            double mStrong = 0;
            double mPassDict = 0;
            double mMuteChanse = 0;
            //считываем данные
            foreach (Ill o in LI)
            {
                mDeadly += o.deadly;
                mCont  +=o.contagation;
                mStrong += o.strong;
                mPassDict += o.passDict;
                mMuteChanse += o.MutateChanse;
            }
            int n = LI.Count();
            //считаем среднее
            mDeadly = mDeadly / n;
            mCont = mCont / n;
            mStrong = mStrong / n;
            mPassDict = mPassDict / n;
            mMuteChanse = mMuteChanse / n;
            //сохраняем данные
            deadly.Add(mDeadly);
            contagation.Add(mCont);
            strong.Add(mStrong);
            passDict.Add(mPassDict);
            MutateChanse.Add(mMuteChanse);

        }

        override public void SaveStatistic()
        {

            System.Threading.Thread.Sleep(10000);
            //// Создаём экземпляр нашего приложения
            Excel.Application excelApp2 = new Excel.Application();
            excelApp2.Visible = true;
            excelApp2.Workbooks.Add();
            excelApp2.DisplayAlerts = false;
            Excel._Worksheet workSheet2 = excelApp2.ActiveSheet;
            //// Создаём экземпляр листа Excel

            workSheet2.Cells[1, 1] = "Ср Смерт";
            for (int j = 2; j <= deadly.Count() + 1; j++)
            {
                workSheet2.Cells[1, j] = deadly[j - 2];
            }

            workSheet2.Cells[2, 1] = "Ср заразность";
            for (int j = 2; j <= contagation.Count() + 1; j++)
            {
                workSheet2.Cells[2, j] = contagation[j - 2];
            }

            workSheet2.Cells[3, 1] = "Ср сила";
            for (int j = 2; j <= strong.Count() + 1; j++)
            {
                workSheet2.Cells[3, j] = strong[j - 2];
            }

            workSheet2.Cells[4, 1] = "Ср расстояние передачи";
            for (int j = 2; j <= passDict.Count() + 1; j++)
            {
                workSheet2.Cells[4, j] = passDict[j - 2];
            }

            workSheet2.Cells[5, 1] = "Ср шан мутации";
            for (int j = 2; j <= MutateChanse.Count() + 1; j++)
            {
                workSheet2.Cells[5, j] = MutateChanse[j - 2];
            }

            excelApp2.DisplayAlerts = false;
            workSheet2.SaveAs(string.Format(@"{0}\Статистика_болезнь.xlsx", System.Environment.CurrentDirectory));

            excelApp2.Quit();
        }
        override public void PrintStatistic()
        {

        }
    }
}
