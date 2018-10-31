using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace war
{
    class Ill
    {
        static public int typePass = 3;//тип передачи 2 - контактный, 3 - воздушнокапельный, 4 - половой
        public int deadly=10;//смертоносность, шанс убить носителя (сколько здоровья отнимает на итерации)
        public int contagation=25;//заразность, шанс передасться
        public int strong=25;//Сила, сопротивление лечению и выздоровлению
        public int passDict=25;//расстояние. на котором передается болезнь при передаче воздушно-капельным путем
        public int MutateChanse =50;//шанс мутации болезни
        static int MV = 2;//величина мутации

        public void Mutate()//мутация болезни
        {
            Random r = new Random();
            Random val = new Random();
            int iV;
            int MC = r.Next(1, 101);
            if (MC <= MutateChanse)
            {
                int rand = r.Next(0, 5);
                switch (rand)
                {
                    case 0:
                        iV = val.Next(-MV, MV + 1);
                        deadly = deadly + iV;
                        if (deadly < 1)
                            deadly = 1;
                        break;
                    case 1:
                        iV = val.Next(-MV, MV + 1);
                        contagation = contagation + iV;
                        if (contagation < 1)
                            contagation = 1;
                        break;
                    case 2:
                        iV = val.Next(-MV, MV + 1);
                        strong = strong + iV;
                        if (strong < 1)
                            strong = 1;
                        break;
                    case 3:
                        iV = val.Next(-MV, MV + 1);
                        passDict = passDict + iV;
                        if (passDict < 1)
                            passDict = 1;
                        break;
                    case 4:
                        iV = val.Next(-MV, MV + 1);
                        MutateChanse = MutateChanse + iV;
                        if (MutateChanse < 1)
                            MutateChanse = 1;
                        break;
                }
            }
        }

        public Ill GetIll()//передать болезнь другой особи, при передае болезнь мутирует
        {
            Ill newIll = new Ill();
            newIll.deadly = this.deadly;
            newIll.contagation = this.contagation;
            newIll.strong = this.strong;
            newIll.passDict = this.passDict;
            newIll.MutateChanse = this.MutateChanse;
            newIll.Mutate();

            return newIll;
        }

    }
}
