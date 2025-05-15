using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyGalaktyczneFull
{
    public class Planeta
    {
        
        public enum typKolonii
        {
            PortKosmiczny,
            Posterunek,
            Kopalnia,
            Farma
        }
        public string nazwa;
        public string uklad;
        public typKolonii typ;
        public int cena;
        public int czynsz;
        public int poziom;
        public int maxPoziom;
        public Gracz wlasciciel;
        public Planeta(string nazwa, string uklad, int cena)
        {
            this.nazwa = nazwa;
            this.uklad = uklad;
            this.cena = cena;
            this.czynsz = 0;
            this.poziom = 0;
            this.wlasciciel = null;
        }

        public string nazwaAglomeracji(int poziomAglomeracji)
        {
            if (typ == typKolonii.Posterunek)
            {
                if (poziom == 1)
                    return "Posterunek";
                else if (poziom == 2)
                    return "Habitat Mieszkalny";
                else if (poziom == 3)
                    return "Kolonia";
                else if (poziom == 4)
                    return "Hotel Galaktyczny";
                else if (poziom == 5)
                    return "Sieć Hoteli Planetarnych";
            }
            else if (typ == typKolonii.Kopalnia)
                return "Kopalnia, poziom " + poziom;
            else if (typ == typKolonii.Farma)
                return "Farma żywności, poziom " + poziom;
            return "";
        }
        public void osiedl(Gracz kupujacy)
        {
            this.wlasciciel = kupujacy;
            this.typ = typKolonii.PortKosmiczny;
            this.czynsz = 100;

            kupujacy.kasa -= cena;
        }
        public void wybuduj(typKolonii typ, Gracz kupujacy)
        {
            this.typ = typ;

            if(typ == typKolonii.Posterunek)
            {
                this.czynsz = 200;
                this.maxPoziom = 5;
                this.poziom = 1;
            }
            else if (typ == typKolonii.Kopalnia)
            {
                this.czynsz = 300;
                this.maxPoziom = 3;
                this.poziom = 1;
            }
            else if (typ == typKolonii.Farma)
            {
                this.czynsz = 300;
                this.maxPoziom = 5;
                this.poziom = 1;
            }

            kupujacy.kasa -= cena;
        }  
    }
}
