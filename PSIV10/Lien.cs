using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PSIV10
{
    public class Lien
    {
        public int IdSource { get; }

        public int IdDestination { get; }

        public double Poids { get; }
        public string Ligne { get; }

        public Lien(int idSource, int idDestination, double poids, string ligne = "")
        {
            IdSource = idSource;
            IdDestination = idDestination;
            Poids = poids;
            Ligne = ligne;
        }
    }
}
