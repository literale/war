using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace war
{
    class Life
    {
        static public List<Specimen> LS = new List<Specimen>();
        static public List<Ill> LI = new List<Ill>();
        static public StatisticSpisemen SS = new StatisticSpisemen();
        static public StatisticIll SI = new StatisticIll();
        static public int fx = 880;
        static public int fy = 839;


        static public void Iteration()
        {
            foreach ( Specimen ls in LS )
            {
                ls.Iteration();
                    ls.Move(fx, fy);
                Random r1 = new Random();
                int t = r1.Next(10, 25);
                System.Threading.Thread.Sleep(t);
            }
            //for (int i = 0; i < LS.Count; i++)
            //{
            //    if (LS[i].live == false)
            //        LS.Remove(LS[i]);
            //    i--;
            //}

            ReproduseFinal();
            Infect();

            foreach (Ill ill in LI)
            {
                if (ill == null)
                    LI.Remove(ill);
            }
            SS.GetStatistic();
            SI.GetStatistic();
            



        }

       static void ReproduseFinal() //размножаем всех
        {
            Specimen newS = null;

            int n = LS.Count();
            if (Specimen.typeOfReproduse > 1)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (LS[i].wantReproduse && LS[j].wantReproduse)
                            newS = LS[i].TryReproduseGetero(LS[j]);

                        if (newS != null)
                        {
                            LS.Add(newS);
                            LS[i].wantReproduse = false;
                            LS[j].wantReproduse = false;
                        }
                    }

                    if (LS[i].wantReproduse && LS[i].sex==1 && Specimen.typeOfReproduse ==3 )
                        newS = LS[i].ReproduseGomo();

                    if (newS != null)
                    {
                        LS.Add(newS);
                        LS[i].wantReproduse = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (LS[i].wantReproduse)
                        newS = LS[i].ReproduseGomo();

                    if (newS != null)
                    {
                        LS.Add(newS);
                        LS[i].wantReproduse = false;
                    }
                }
            }

        }

       static void Infect()//Заражение популяции
        {
            switch (Ill.typePass)
            {
                case 1://наследственный, не тут
                    break;
                case 2://контактный         
                    InfectCont();
                    break;
                case 3://воздушно-капельный
                    InfectAir();
                    break;
                case 4://половой. не тут
                    break;

            }
        }

        static void InfectCont()//Заразить контактно
        {
            for (int i = 0; i < LS.Count(); i++)
            {
                if (LS[i].isSick)
                {
                    for (int j = 0; j < LS.Count(); j++)
                    {
                        if (!LS[j].isSick)
                        {
                            int realDist = 0;
                            realDist = Convert.ToInt32(Math.Sqrt(((LS[i].x - LS[j].x) * (LS[i].x - LS[j].x)) + ((LS[i].y - LS[j].y) * (LS[i].y - LS[j].y))));

                            if (realDist <= Specimen.reproduseDistanse)
                            {
                                LS[j].TryBeginSick(LS[i].ill);

                            }

                        }

                    }

                }
            }

        }

        static void InfectAir()//Заразить воздушно-капедльным путем
        {
            for (int i = 0; i < LS.Count(); i++)
            {
                if (LS[i].isSick)
                {
                    for (int j = 0; j < LS.Count(); j++)
                    {
                        if (!LS[j].isSick)
                        {
                            int realDist = 0;
                            realDist = Convert.ToInt32(Math.Sqrt(((LS[i].x - LS[j].x) * (LS[i].x - LS[j].x)) + ((LS[i].y - LS[j].y) * (LS[i].y - LS[j].y))));

                            if (realDist <= LS[i].ill.passDict)
                            {
                                LS[j].TryBeginSick(LS[i].ill);

                            }

                        }

                    }

                }
            }
        }

        static public void TreatDrugs()//Лечим все лекарствами
        {
            foreach (Specimen ls in LS)
            {
                if (ls.isSick)
                    ls.TreatDrugs();
            }

            foreach (Ill ill in LI)
            {
                if (ill == null)
                    LI.Remove(ill);
            }
        }

    }
}
