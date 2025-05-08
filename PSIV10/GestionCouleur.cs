using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PSIV10
{
    public static class GestionCouleur
    {
        /// <summary>
        /// Retourne la couleur associée à une ligne donnée.
        /// Chaque ligne est associée à une couleur spécifique, sinon noir par défaut.
        /// </summary>
        /// <param name="ligne">Nom ou numéro de la ligne.</param>
        /// <returns>Couleur correspondant à la ligne.</returns>
        public static Color GetCouleurLigne(string ligne)
        {
            if (ligne == "1" || ligne == "Ligne 1")
                return Color.Yellow;
            else if (ligne == "2" || ligne == "Ligne 2")
                return Color.Blue;
            else if (ligne == "3" || ligne == "Ligne 3")
                return Color.Green;
            else if (ligne == "4" || ligne == "Ligne 4")
                return Color.Purple;
            else if (ligne == "5" || ligne == "Ligne 5")
                return Color.Orange;
            else if (ligne == "6" || ligne == "Ligne 6")
                return Color.LightGreen;
            else if (ligne == "7" || ligne == "Ligne 7")
                return Color.Pink;
            else if (ligne == "8" || ligne == "Ligne 8")
                return Color.BlueViolet;
            else if (ligne == "9" || ligne == "Ligne 9")
                return Color.Olive;
            else if (ligne == "10" || ligne == "Ligne 10")
                return Color.Gold;
            else if (ligne == "11" || ligne == "Ligne 11")
                return Color.Brown;
            else if (ligne == "12" || ligne == "Ligne 12")
                return Color.DarkGreen;
            else if (ligne == "13" || ligne == "Ligne 13")
                return Color.LightBlue;
            else if (ligne == "14" || ligne == "Ligne 14")
                return Color.Purple;
            else
                return Color.Black;
        }

    }
}
