using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyGalaktyczneFull.src
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
            czynsz = 0;
            poziom = 0;
            wlasciciel = null;
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
            wlasciciel = kupujacy;
            typ = typKolonii.PortKosmiczny;
            czynsz = 100;

            kupujacy.kasa -= cena;
        }
        public void wybuduj(typKolonii typ, Gracz kupujacy)
        {
            this.typ = typ;

            if(typ == typKolonii.Posterunek)
            {
                czynsz = 200;
                maxPoziom = 5;
                poziom = 1;
            }
            else if (typ == typKolonii.Kopalnia)
            {
                czynsz = 300;
                maxPoziom = 3;
                poziom = 1;
            }
            else if (typ == typKolonii.Farma)
            {
                czynsz = 300;
                maxPoziom = 5;
                poziom = 1;
            }

            kupujacy.kasa -= cena;
        }  
    }
}
