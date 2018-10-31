using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            number.Add(LS.Count());
        }
        override public void SaveStatistic()
        {

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

        }
        override public void PrintStatistic()
        {

        }
    }
}
