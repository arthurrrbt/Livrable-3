using Xunit;
using PSIV10; 

namespace PSIV10.TestsClientCuisinier
{
    public class TestsArete
    {
        [Fact]
        public void ConstructeurParDefaut_DoitInitialiserLesProprietesCorrectement()
        {
            // Arrange & Act
            var arete = new Arete();

            // Assert
            Assert.Equal(0, arete.IdClient);
            Assert.Equal(0, arete.IdCuisinier);
            Assert.Equal(0, arete.NbCommandes);
        }

        [Fact]
        public void AffectationDesProprietes_DoitFonctionnerCorrectement()
        {
            // Arrange
            var arete = new Arete
            {
                IdClient = 5,
                IdCuisinier = 10,
                NbCommandes = 3
            };

            // Act & Assert
            Assert.Equal(5, arete.IdClient);
            Assert.Equal(10, arete.IdCuisinier);
            Assert.Equal(3, arete.NbCommandes);
        }
    }
}
