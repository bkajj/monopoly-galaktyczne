using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//w zad 10 wykorzsytac system.speech
//gui
//czytanie z pliku albo textbox
//mozliwosc wyboru glosu itp
//pauzowanie czytania + przerwanie
//znaczniki w tekscie dotyczace lektora - glos meski, zenski, itp
namespace MonopolyGalaktyczneFull.src
{
    public class Gra
    {
        public GameForm gameForm;
        public List<Gracz> gracze;
        public Plansza plansza;
        int iloscTur;
        public int turaGracza;
        public Gra()
        {
            plansza = new Plansza();
        }

        public void dodajGraczy(int liczbaGraczy)
        {
            gameForm = Program.gameForm;
            gracze = new List<Gracz>(liczbaGraczy);
            iloscTur = liczbaGraczy;
            turaGracza = 0;
        }

        private void ustawTure(int turaGracza)
        {
            gameForm.UpdatePlayerInfo(gracze[turaGracza]);
        }

        public void stworzGraczy(string[] nicknames)
        {
            for (int i = 0; i < nicknames.Length; i++)
            {
                gracze.Add(new Gracz(nicknames[i], this));
            }
        }

        public void nastepnaTura()
        {
            if(gracze[turaGracza].kasa < 0)
                gameForm.KillPlayer(gracze[turaGracza]);

            if(gracze.Count == 1)
            {
                MessageBox.Show($"Gracz {gracze[0].nick} wygrał!");
                gameForm.Close();
            }

            dodajZyskCykliczny(); //wszystkim graczom co <iloscTur> tury
            turaGracza++;
            turaGracza %= iloscTur;
            gameForm.UpdatePlayerInfo(gracze[turaGracza]);
            gracze[turaGracza].wykonajRuch();
        }

        private void dodajZyskCykliczny()
        {
            if (turaGracza >= iloscTur)
            {
                int dodatkowaKasa = 0;
                for (int i = 0; i < iloscTur; i++)
                {
                    dodatkowaKasa = gracze[i].iloscPosiadanychPlanet() * 50 + gracze[i].zyskStocznia;
                    gracze[i].kasa += dodatkowaKasa;
                }
            }
        }

        public void start()
        {
            gameForm.HideAddPlayerSection();
            gameForm.CreatePlayerInfo(iloscTur);

            var random = new Random();
            gracze = gracze.OrderBy(x => random.Next()).ToList();

            ustawTure(turaGracza);
            gracze[turaGracza].wykonajRuch();
        }

        public void usunGracza(Gracz gracz)
        {
            gracze.Remove(gracz);
            iloscTur--;
        }
    }
}
