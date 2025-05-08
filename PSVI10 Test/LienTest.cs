using Xunit;
using PSIV10;

namespace PSIV10.TestsMétro
{
    public class TestsLien
    {
        [Fact]
        public void CreerLien_AttribueCorrectementLesValeurs()
        {
            // Arrange (préparation des données)
            int idSource = 1;
            int idDestination = 2;
            double poids = 5.0;
            string ligne = "Ligne 1";

            // Act (création du lien)
            Lien lien = new Lien(idSource, idDestination, poids, ligne);

            // Assert (vérification des valeurs)
            Assert.Equal(idSource, lien.IdSource);
            Assert.Equal(idDestination, lien.IdDestination);
            Assert.Equal(poids, lien.Poids);
            Assert.Equal(ligne, lien.Ligne);
        }

        [Fact]
        public void CreerLien_SansLigne_AssigneValeurParDefaut()
        {
            // Arrange
            int idSource = 1;
            int idDestination = 2;
            double poids = 5.0;

            // Act
            Lien lien = new Lien(idSource, idDestination, poids);

            // Assert
            Assert.Equal("", lien.Ligne); // Vérifie que la ligne est bien vide par défaut
        }
    }
}
