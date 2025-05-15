using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyGalaktyczneFull
{
    public class Plansza
    {
        public Pole[] pola;
        public Plansza()
        {
            pola = new Pole[24];
            for(int i = 0; i < 24; i++)
            {
                Pole pole;

                if (i == 0)
                    pole = new Pole(Pole.TypPola.Start);
                else if (i == 6 || i == 18)
                    pole = new Pole(Pole.TypPola.Osobliwosc);
                else if (i == 12)
                    pole = new Pole(Pole.TypPola.Piraci);
                else if ((i + 3) % 6 == 0)
                    pole = new Pole(Pole.TypPola.Kolej);
                else
                    pole = new Pole(Pole.TypPola.Planeta);

                pole.index = i;
                pola[i] = pole;
            }

            pola[1].planeta = new Planeta("Merkury", "Układ słoneczny", 200);
            pola[2].planeta = new Planeta("Wenus", "Układ słoneczny", 200);
            pola[4].planeta = new Planeta("Ziemia", "Układ słoneczny", 200);
            pola[5].planeta = new Planeta("Mars", "Układ słoneczny", 200);

            pola[7].planeta = new Planeta("Eres", "Układ Kepler-31", 300);
            pola[8].planeta = new Planeta("Titan", "Układ Kepler-31", 300);
            pola[10].planeta = new Planeta("Callisto", "Układ Kepler-31", 300);
            pola[11].planeta = new Planeta("Rhea", "Układ Kepler-31", 300);

            pola[13].planeta = new Planeta("Rondo", "Układ Zeta Reticuli", 400);
            pola[14].planeta = new Planeta("Feros", "Układ Zeta Reticuli", 400);
            pola[16].planeta = new Planeta("Arcanis", "Układ Zeta Reticuli", 400);
            pola[17].planeta = new Planeta("Icarion", "Układ Zeta Reticuli", 400);

            pola[19].planeta = new Planeta("Eldran", "Układ Varkorath", 500);
            pola[20].planeta = new Planeta("Sirius X ", "Układ Varkorath", 500);
            pola[22].planeta = new Planeta("Draxion", "Układ Varkorath", 500);
            pola[23].planeta = new Planeta("Enara", "Układ Varkorath", 500);

        }
    }
}
