using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace war
{
    class Specimen
    {
        public bool live = true;
        static public int radius = 5;//радиус круга, обозначающего особь
        static public int typeOfReproduse = 2; //1-бесполое, 2 - половое, 3 - и то и то
        public static int reproduseDistanse = 15; // расстояние, на котором особи могут спариться. так же при таком расстонии считается что особи соприкасались и болезнь может передаться контактным путем
        public int immun=50;//иммунитет. Влияет на шанс не заболеть и вылечиться
        public int dopimmun = 0;
        public int maxAge=120;//Максимальный возраст особи (в месяцах)
        public int age=0;//Текущий возраст особи (в месяцах)
        public int sex=1;//Пол особи 1-женский, 2-мужской, 3-средний
        public int health=100;//здоровье. при достижении нуля - умирает
        public bool wantReproduse=false;//хочет размножиться
        public int freqReproduse = 2;//cколько раз в год хочет размножаться
        public bool isSick=false;//болеет
        public int maxWay = 5;//максимальный путь, который пройдет особь за итерацию
        public int x=0;
        public int y=0;
        int quality=50;
        public Ill ill = null;//болеет этой болезнью
        static int MV = 2;//величина мутации
        public int MutateChanse=50;//шанс мутации новой особи
        public static int maxPop = 200; //Максимальный размер популяции

        public static int CountLived()//счтаем жывых
        {
            int lived = 0;
            for(int i= 0; i< Life.LS.Count(); i++ )
            {
                if (Life.LS[i].live)
                    lived++;
            }

            return lived;
            
        }

        public Specimen(int immun, int maxAge, int sex, int health, int x, int y, int MutateChanse, int freqReproduse, int speed )//конструктор
        {
            this.immun = immun;
            this.maxAge = maxAge;
            this.sex = sex;
            this.health = health;
            this.x = x;
            this.y = y;
            this.MutateChanse = MutateChanse;
            maxWay = speed;
        }

        public void Move(int fX, int fY)//движение особи
        {
            Random way = new Random();
            Random direct = new Random();
            int iWay = way.Next(-maxWay, maxWay + 1);
            int iDir = direct.Next(1, 3);
            
            switch (iDir)
            {
                case 1://движение по х
                    x = x + iWay;
                    if (x <= 0)
                        x = fX-1;
                    if (x > fX)
                        x = 1;


                    break;
                case 2://движение по у
                    y = y + iWay;
                    if (y <= 0)
                        y = fY - 1;
                    if (y > fY)
                        y = 1;
                    break;

            }
            getWantReproduse();
        }

        public void Mutate()//мутация новой особи
        {
            Random r = new Random();
            int MC = r.Next(1, 101);
            if (MC <= MutateChanse)
            {
                int rand = r.Next(0, 826);
                int cRand;
                int tmp=0;
                if (rand <= 150)
                    tmp = 0;
                if (rand > 150 && rand <= 450)
                    tmp = 1;
                if (rand > 150 && rand <= 450)
                    tmp = 2;
                if (rand > 450 && rand <= 750)
                    tmp = 3;
                if (rand > 750 && rand <= 800)
                    tmp = 4;
                if (rand > 800 && rand <= 825)
                    tmp = 5;


                switch (tmp)
                {
                    case 0:
                        cRand = r.Next(-MV, MV + 1);
                        immun = immun + cRand;
                        if (immun < 1)
                            immun = 1;
                        break;
                    case 1:
                        cRand = r.Next(-MV, MV + 1);
                        maxAge = maxAge + cRand;
                        if (maxAge < 1)
                            maxAge = 1;
                        break;
                    case 2:
                        cRand = r.Next(-MV, MV + 1);
                        health = health + cRand;
                        if (health < 1)
                            health = 1;
                        break;
                    case 3:
                        cRand = r.Next(-MV, MV + 1);
                        maxWay = maxWay + cRand;
                        if (maxWay < 1)
                            maxWay = 1;
                        break;
                    case 4:
                        cRand = r.Next(-MV, MV + 1);
                        MutateChanse = MutateChanse + cRand;
                        if (MutateChanse < 1)
                            MutateChanse = 1;
                        break;
                    case 5:
                        cRand = r.Next(-MV, MV + 1);
                        freqReproduse = freqReproduse + cRand;
                        if (freqReproduse < 1)
                            freqReproduse = 1;
                        break;


                }
            }  
        }

        public void TreatDrugs()//излечить особь лекарством
        {        
            int tmp = GetProb(Drugs.strong, this.ill.strong);
            if (tmp>0)
            {
                dopimmun += 5;
                this.isSick = false;
                this.ill = null;
            }
            
            if (tmp == 0)
            {
                Random r = new Random();
                int rand = r.Next(0, 2);
                if (rand==0)
                {
                    dopimmun += 5;
                    this.isSick = false;
                    this.ill = null;
                }
            }
        }

        void TreatImun()//излечиться силами иммунитета
        {
            if (Ill.typePass != 1)
            { 
            int tmp = GetProb(this.immun + this.dopimmun, this.ill.strong);
            if (tmp > 0)
            {
                dopimmun += 5;
                this.isSick = false;
                this.ill = null;
            }

                if (tmp == 0)
                {
                    Random r = new Random();
                    int rand = r.Next(0, 2);
                    if (rand == 0)
                    {
                        dopimmun += 5;
                        this.isSick = false;
                        this.ill = null;
                    }
                }
            }
        }

        public void TryBeginSick(Ill ill)//заразить особь
        {
            int tmp = GetProb(ill.contagation, this.immun);
            if (tmp > 0)
            {
                this.isSick = true;
                this.ill = ill.GetIll();
                Life.LI.Add(this.ill);
            }

            if (tmp == 0)
            {
                Random r = new Random();
                int rand = r.Next(0, 2);
                if (rand == 0)
                {
                    this.isSick = true;
                    this.ill = ill.GetIll();
                    Life.LI.Add(this.ill);
                }
            }
        }

        static public int GetProb(int wantIT, int DontWantIT)//вспомогательная функция подсчета вероятности, сверху вероятность события, которого мы ожидаем
        {
            int tmp = 0;
            int prob = 0;
            Random r = new Random();

            if (wantIT == 0 || DontWantIT == 0)//обработка исключения, на 0 не делим
            {
                if (wantIT == 0)
                    tmp = -1;
                else
                    tmp = wantIT;
            }
            else
            {
                if (wantIT == DontWantIT)
                {
                    int rand = r.Next(0, 2);
                    if (rand == 0)
                        tmp = -1;
                    else
                        tmp = 1;

                }
                if (wantIT > DontWantIT)
                {
                  
                    prob = Convert.ToInt32((wantIT / DontWantIT) * 50);
                    int rand = r.Next(0, 101);
                    tmp = prob - rand;
                    int t = r.Next(1, 10);
                    System.Threading.Thread.Sleep(t);//чтобы генератор успел замениться


                }
                if (DontWantIT > wantIT)
                {                    
                    prob = Convert.ToInt32((DontWantIT / wantIT) * 50);
                    int rand = r.Next(0, 101);
                    tmp = rand - prob;
                    int t = r.Next(1, 10);
                    System.Threading.Thread.Sleep(t);//чтобы генератор успел замениться
                }
            }
            
            return tmp;
        }

        Specimen ReproduseGetero(ref Specimen secondParent)//размножиться с партнером
        {
            this.wantReproduse = false;
            secondParent.wantReproduse = false;
            Random r = new Random();

            int immun = Convert.ToInt32((this.immun + secondParent.immun) / 2);
            int maxAge = Convert.ToInt32((this.maxAge + secondParent.maxAge) / 2);
            int sex = r.Next(1, 3);
            int health = Convert.ToInt32((this.health + secondParent.health) / 2);
            int x = Convert.ToInt32((this.x + secondParent.x) / 2);
            int y = Convert.ToInt32((this.y + secondParent.y) / 2);
            int MutateChanse = Convert.ToInt32((this.MutateChanse + secondParent.MutateChanse) / 2);
            int freqReproduse = Convert.ToInt32((this.freqReproduse + secondParent.freqReproduse) / 2);
            int speed = Convert.ToInt32((this.maxWay + secondParent.maxWay) / 2);
            Specimen newS = new Specimen(immun, maxAge,sex, health, x, y, MutateChanse, freqReproduse, speed);
            newS.maxWay = freqReproduse;
            newS.Mutate();
            if (this.isSick)//заражение партнера
            {
                secondParent.TryBeginSick(this.ill);
            }
            else
            {
                if (secondParent.isSick)
                    this.TryBeginSick(secondParent.ill);
            }  
            //заражение ребенка
            if (this.sex==1&&this.isSick)
            {
                newS.TryBeginSick(this.ill);
            }
            if (secondParent.sex == 1 && secondParent.isSick)
            {
                newS.TryBeginSick(secondParent.ill);
            }


            return newS;
        }

        public Specimen ReproduseGomo()//создать свою копию
        {
            this.wantReproduse = false;
            Random r = new Random();

            int immun = this.immun;
            int maxAge = this.maxAge;
            int sex = this.sex;
            int health = this.health;
            int x = this.x;
            int y = this.y;
            int MutateChanse = this.MutateChanse;
            int freqReproduse = this.freqReproduse;
            int speed = this.maxWay;
            Specimen newS = new Specimen(immun, maxAge, sex, health, x, y, MutateChanse, freqReproduse, speed);
            newS.maxWay = freqReproduse;
            newS.Mutate();
               if (this.isSick)
               {

                newS.TryBeginSick(this.ill);
            }

            return newS;
        }

        public void getWantReproduse()//начать сезон спаривания
        {
           // int tmp = GetProb(this.freqReproduse, 12);
            Random r = new Random();
            int rand = r.Next(0, 101);
            int prob = (this.freqReproduse / 12) * 100;
            int tmp = prob - rand;
            if (tmp >= 0)
                this.wantReproduse = true;
        }

        public Specimen TryReproduseGetero(Specimen secondParent)//попытаться размножиться с партнером
        {
            Specimen nSpecimen = null;
            int realDist = 0;
            realDist = Convert.ToInt32(Math.Sqrt(((this.x-secondParent.x) * (this.x - secondParent.x))+( (this.y - secondParent.y) * (this.y - secondParent.y))));
            
            if (realDist<=reproduseDistanse && this.sex!=secondParent.sex)
            {
                int tmp = 1;
               //int tmp = GetProb(this.quality, secondParent.quality);//отбор партнера по качеству
                if (tmp > 0)
                {
                    nSpecimen = this.ReproduseGetero(ref secondParent);
                    this.wantReproduse = false;
                    secondParent.wantReproduse = false;
                }

            }
            return nSpecimen;
        }

        public void Iteration()//итеративные изменения
        {
            this.age = age + 1;
            if (age > maxAge)
            {
                live = false;
            }
            if (isSick)
            {
                TreatImun();
                if (isSick)
                {
                    health = health - ill.deadly;
                    Random r = new Random();//С шансом 25% уменьшаем максимальный путь особи, она же болеет
                    int tmp = r.Next(0, 4);
                    if (tmp == 0)
                        this.maxWay = maxWay - 1;
                    if (maxWay < 1)
                        maxWay = 1;

                    
                    tmp = this.ill.deadly;//Понижение привлекательности зависит от болезни
                    int n = Convert.ToInt32(tmp/2);

                    this.quality = quality - n;
                    if (this.quality < 0)
                        this.quality = 0;

                }
            }
            if (health <= 0)
            {
                live = false;
            }
            if (live)
            {
                getWantReproduse();
            }

        }



    }
}
