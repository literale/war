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

        //Данная функция отвечает за всеобщие итаративные изменения.
        //Сначала она удаляет из списка мертвых особей
        //Затем для каждой особи она вызывает личные итеративные изменения, а так же заставляет каждую особь двигаться
        //Затем она вызывает функцию всеобщего размножения
        //Затем - общую функцию заражения
        //Затем из списка болезней удаляются обнуленные болезни
        //Затем собирается статистика
        static public void Iteration()
        {
            for (int i = 0; i < LS.Count; i++)
            {
                if (LS[i].live == false)
                    LS.Remove(LS[i]);
                i--;
            }

            foreach ( Specimen ls in LS )
            {
                ls.Iteration();
                ls.Move(fx, fy);
                Random r1 = new Random();
                int t = r1.Next(10, 25);
                System.Threading.Thread.Sleep(t);
            }

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
        //Данная функция отвечает за всеобщее размножение. В зависимости от способа размножения она поступает по разному
        // Если способ размножения половой или смешенный, а размер популяции не больше максимального - 
        //она проходится по списку всех особей
        //и если обе особи хотят размножаться - она пытается спарить из вызывая функцию TryReproduseGetero,
        // и если размножение прошло успешно - новая особь добавляется в список популяции
        // Если способ размножения смешанный, особи не нашлось подходящего партнера, она женского пола и она хочет спариваться - 
        // Вызывается метод ReproduseGomo, который создает копию материнской особи с некоторыми мутациями
        //Если способ размножения - бесполый и особь хочет размножаться - так же вызывается функция ReproduseGomo
       static void ReproduseFinal() //размножаем всех
        {
            Specimen newS = null;
            
            int n = LS.Count();
            int pop = Specimen.CountLived();
            if (pop < Specimen.maxPop)
            {
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
                                if (Specimen.CountLived() > Specimen.maxPop)
                                    break;
                            }
                        }

                        if (LS[i].wantReproduse && LS[i].sex == 1 && Specimen.typeOfReproduse == 3)
                            newS = LS[i].ReproduseGomo();

                        if (newS != null)
                        {
                            LS.Add(newS);
                            LS[i].wantReproduse = false;
                            if (Specimen.CountLived() > Specimen.maxPop)
                                break;
                        }
                        if (Specimen.CountLived() > Specimen.maxPop)
                            break;
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
                        if (Specimen.CountLived() > 200)
                            break;
                    }
                }
            }
        }
        
        //Данная функция - функция общего заражения, которая вызывает нужную функцию в зависимости от типа передачи болезни
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
        
        //Данная функция отвечает за контактный тип передачи болезни
        //Она дважды проходится по списку особей и для каждой пары проверяет,
        //заражена ли одна (только одна) из особей и находятся ли они на расстоянии заражения
        //Расстоянием заражения для болезни, передающейся половым путем, является расстояние,
        //на котором особи могут спариться, поскольку будем считать, что тогда они обитают достаточно близко
        //и могут пересечься на итерации (учитываем, что за каждую итерацию проходит месяц и мы не рассматриваем, 
        //как именно особи двигаются весь месяц). Если все условия выполнены - она пытается передать болезнь
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
        //Данная функция предназначена для распространения болезни воздушно-капельным путем,
        //Она дваждый проходится по листу особей и для каждой пары смотрит, болеет ли только одна из них,
        //И если болеет - смотрит, находятся ли они на расстоянии заражения. И если все условия выполнены - 
        //она пытается передать болезнь
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
        //Данная функция предназначена для излесчения всех больных особей лекарствами
        //Она проходит по списку особей, и для каждой особи если она больна - пытается излечить
        //После она проходится по списку болезней и если болезнь обнулена - болезнь удаляетсся из списка
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
