using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PSIV10
{
    public class Noeud
    {
        public int Id { get; private set; }
        public string Nom { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string Ligne { get; private set; }
        public Color Couleur { get; private set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe Noeud avec les informations spécifiées.
        /// </summary>
        /// <param name="id">Identifiant unique du nœud.</param>
        /// <param name="nom">Nom du nœud.</param>
        /// <param name="lat">Latitude du nœud.</param>
        /// <param name="lon">Longitude du nœud.</param>
        /// <param name="ligne">Ligne associée au nœud.</param>
        public Noeud(int id, string nom, double lat, double lon, string ligne)
        {
            Id = id;
            Nom = nom;
            Latitude = lat;
            Longitude = lon;
            Ligne = ligne;
            Couleur = GestionCouleur.GetCouleurLigne(ligne);
        }
    }
}
