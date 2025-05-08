using Xunit;
using System.Drawing;
using PSIV10;

namespace PSIV10.TestsMétro
{
    public class TestsNoeud
    {
        [Fact]
        public void CreerNoeud_AttribueCorrectementLesValeurs()
        {
            // Arrange (préparation des données)
            int id = 1;
            string nom = "Station Exemple";
            double latitude = 48.8566;
            double longitude = 2.3522;
            string ligne = "Ligne 1";

            // Act (création du noeud)
            Noeud noeud = new Noeud(id, nom, latitude, longitude, ligne);

            // Assert (vérification des valeurs)
            Assert.Equal(id, noeud.Id);
            Assert.Equal(nom, noeud.Nom);
            Assert.Equal(latitude, noeud.Latitude);
            Assert.Equal(longitude, noeud.Longitude);
            Assert.Equal(ligne, noeud.Ligne);
        }

        [Fact]
        public void CreerNoeud_AssigneBonneCouleurSelonLigne()
        {
            // Arrange
            string ligne = "Ligne 1";
            Color couleurAttendue = GestionCouleur.GetCouleurLigne(ligne);

            // Act
            Noeud noeud = new Noeud(1, "Station Exemple", 48.8566, 2.3522, ligne);

            // Assert
            Assert.Equal(couleurAttendue, noeud.Couleur);
        }
    }
}
