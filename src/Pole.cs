using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyGalaktyczneFull.src
{
    public class Pole
    {
        Planeta _planeta;
        public Planeta planeta
        {
            get
            {
                if (typPola == TypPola.Planeta)
                    return _planeta;
                else
                    throw new InvalidOperationException("To pole nie jest planetą.");
            }
            set
            {
                if (typPola == TypPola.Planeta)
                    _planeta = value;
                else
                    throw new InvalidOperationException("To pole nie jest planetą.");
            }
        }

        public int index;
        public enum TypPola
        {
            Start,
            Planeta,
            Osobliwosc,
            Piraci,
            Kolej
        }

        public TypPola typPola;
        public Pole(TypPola typPola, int index = 0)
        {
            this.typPola = typPola;
            this.index = index;
        }
    }
}
