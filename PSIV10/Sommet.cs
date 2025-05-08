using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PSIV10
{
    public class Sommet
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Type { get; set; }
        public string MetroProche { get; set; }
        public Color Couleur { get; set; } = Color.Gray;
        public double X { get; set; }
        public double Y { get; set; }
        public List<int> AutresIds { get; set; } = new List<int>();
    }
}
