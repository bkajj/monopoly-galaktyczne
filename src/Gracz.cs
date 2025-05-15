namespace MonopolyGalaktyczneFull.src
{
    public class Gracz
    {
        public string nick;
        public int kasa;
        public Pole obecnePole;
        Gra gra;
        public int pominieteRuchyPodRzad = 0;
        public int iloscBiletowGalaktycznych = 0;
        public int iloscObronPrzedPiratami = 0;
        public int blokadaKolejki = 0;
        public int zyskStocznia = 0;
        public Gracz(string nick, Gra gra)
        {
            this.nick = nick;
            kasa = 1500;
            obecnePole = new Pole(Pole.TypPola.Start);
            this.gra = gra;
        }
        public void akcjaPola()
        {
            if (obecnePole.typPola == Pole.TypPola.Planeta)
            {
                if (obecnePole.planeta.wlasciciel == null)
                {
                    gra.gameForm.WybudujAction(obecnePole, this);
                }
                else if (obecnePole.planeta.wlasciciel == this)
                {
                    if(obecnePole.planeta.typ == Planeta.typKolonii.PortKosmiczny)
                    {
                        gra.gameForm.RozbudujPortAction(obecnePole, this);
                    }
                    else
                    {
                        gra.gameForm.RozbudujAction(obecnePole, this);
                    } 
                }
                else
                {
                    gra.gameForm.CzynszAction(obecnePole, this);
                }
            }
            else if (obecnePole.typPola == Pole.TypPola.Osobliwosc)
            {
                gra.gameForm.OsobliwoscAction(this);
            }
            else if (obecnePole.typPola == Pole.TypPola.Kolej)
            {
                gra.gameForm.KolejAction(this);
            }
            else if (obecnePole.typPola == Pole.TypPola.Piraci)
            {
                gra.gameForm.AtakPiratow(this);
            }
            else if(obecnePole.typPola == Pole.TypPola.Start)
            {
                gra.nastepnaTura(); 
            }
        }
        public void rzucKostka()
        {
            Random random = new Random();
            int wynikKostki = random.Next(1, 7);

            int starePoleIndeks = obecnePole.index;
            int nowePoleIndeks = obecnePole.index + wynikKostki;
            
            if (nowePoleIndeks > 23) //przejscie przez start
            {
                kasa += 200;
                nowePoleIndeks %= 24;
                gra.gameForm.UpdatePlayerInfo(this);
            }
            obecnePole = gra.plansza.pola[nowePoleIndeks];

            gra.gameForm.UpdatePlayerPositions(starePoleIndeks, nowePoleIndeks);
            pominieteRuchyPodRzad = 0;

            akcjaPola();
        }

        public void pominRuch()
        {
            pominieteRuchyPodRzad++;
            gra.nastepnaTura();
        }
        public void wykonajRuch()
        {
            gra.gameForm.UpdatePlayerInfo(this);

            if (blokadaKolejki > 0)
            {
                gra.gameForm.GraczZablokowany();
            }
            else
            {
                gra.gameForm.RzucKostkaAction(pominieteRuchyPodRzad);
            }
        }

        public double podatekImperatora()
        {
            double podatek = 0;
            for (int i = 0; i < gra.plansza.pola.Length; i++)
            {
                if(gra.plansza.pola[i].typPola == Pole.TypPola.Planeta)
                    if (gra.plansza.pola[i].planeta.wlasciciel == this)
                        podatek += gra.plansza.pola[i].planeta.cena * 0.1;
            }
            return podatek;
        }

        public bool czyPosiadaWszystkieWUkladzie(int planetaZUkladuIndex)
        {
            for(int i = 0; i < 4; i++)
            {
                //jesli planeta jest z i-tego ukladu
                if (planetaZUkladuIndex >= i+1 && planetaZUkladuIndex <= i+5 && planetaZUkladuIndex != i+3)
                {
                    //sprawdz wszystkie planety w tym ukladzie
                    if (gra.plansza.pola[i + 1].planeta.wlasciciel == this &&
                        gra.plansza.pola[i + 2].planeta.wlasciciel == this &&
                        gra.plansza.pola[i + 4].planeta.wlasciciel == this &&
                        gra.plansza.pola[i + 5].planeta.wlasciciel == this)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int iloscPosiadanychPlanet()
        {
            int ilosc = 0;
            for(int i = 0; i < 24; i++)
            {
                if (gra.plansza.pola[i].typPola == Pole.TypPola.Planeta)
                {
                    if (gra.plansza.pola[i].planeta.wlasciciel == this)
                        ilosc++;
                }
            }
            return ilosc;
        }
    }
}
