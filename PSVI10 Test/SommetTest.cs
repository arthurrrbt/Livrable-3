using Xunit;
using PSIV10; // Ton espace de noms contenant la classe Sommet
using System.Drawing; // Pour la classe Color

namespace PSIV10.TestsClientCuisinier
{
    public class SommetTest
    {
        [Fact]
        public void ConstructeurParDefaut_DoitInitialiserLesProprietesCorrectement()
        {
            // Arrange & Act
            var sommet = new Sommet();

            // Assert
            Assert.Equal(0, sommet.Id);
            Assert.Null(sommet.Nom);
            Assert.Null(sommet.Type);
            Assert.Null(sommet.MetroProche);
            Assert.Equal(Color.Gray, sommet.Couleur);
            Assert.Equal(0, sommet.X);
            Assert.Equal(0, sommet.Y);
            Assert.Empty(sommet.AutresIds);
        }

        [Fact]
        public void AffectationDesProprietes_DoitFonctionnerCorrectement()
        {
            // Arrange
            var sommet = new Sommet
            {
                Id = 1,
                Nom = "Tour Eiffel",
                Type = "Monument",
                MetroProche = "Bir-Hakeim",
                Couleur = Color.Blue,
                X = 48.8584,
                Y = 2.2945,
                AutresIds = new List<int> { 2, 3 }
            };

            // Act & Assert
            Assert.Equal(1, sommet.Id);
            Assert.Equal("Tour Eiffel", sommet.Nom);
            Assert.Equal("Monument", sommet.Type);
            Assert.Equal("Bir-Hakeim", sommet.MetroProche);
            Assert.Equal(Color.Blue, sommet.Couleur);
            Assert.Equal(48.8584, sommet.X);
            Assert.Equal(2.2945, sommet.Y);
            Assert.Equal(new List<int> { 2, 3 }, sommet.AutresIds);
        }
    }
}
