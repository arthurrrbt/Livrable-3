using Xunit;
using PSIV10;
using System.Collections.Generic;

namespace PSIV10.TestsMétro
{
    public class TestsGraphe
    {
        [Fact]
        public void AjouterNoeud_AjouteCorrectementUneStation()
        {
            // Arrange
            Graphe graphe = new Graphe();

            // Act
            graphe.AjouterNoeud(1, "Station A", 48.8566, 2.3522, "Ligne 1");

            // Assert
            Assert.Single(graphe.GetStations());
            Assert.Equal("Station A", graphe.GetStations()[0].Nom);
        }

        [Fact]
        public void AjouterLien_AjouteCorrectementUnLien()
        {
            // Arrange
            Graphe graphe = new Graphe();
            graphe.AjouterNoeud(1, "Station A", 48.8566, 2.3522, "Ligne 1");
            graphe.AjouterNoeud(2, "Station B", 48.8570, 2.3530, "Ligne 1");

            // Act
            Lien lien = new Lien(1, 2, 5.0, "Ligne 1");
            graphe.AjouterLien(lien);

            // Assert
            Assert.Single(graphe.Liens);
            Assert.Equal(1, graphe.Liens[0].IdSource);
            Assert.Equal(2, graphe.Liens[0].IdDestination);
            Assert.Equal(5.0, graphe.Liens[0].Poids);
        }

        [Fact]
        public void Dijkstra_TrouveUnCheminValide()
        {
            // Arrange
            Graphe graphe = new Graphe();
            graphe.AjouterNoeud(1, "Station A", 48.8566, 2.3522, "Ligne 1");
            graphe.AjouterNoeud(2, "Station B", 48.8570, 2.3530, "Ligne 1");
            graphe.AjouterNoeud(3, "Station C", 48.8580, 2.3540, "Ligne 1");

            graphe.AjouterLien(new Lien(1, 2, 5.0, "Ligne 1"));
            graphe.AjouterLien(new Lien(2, 3, 3.0, "Ligne 1"));

            // Act
            List<Noeud> chemin = graphe.Dijkstra(1, 3);

            // Assert
            Assert.Equal(3, chemin.Count);
            Assert.Equal("Station A", chemin[0].Nom);
            Assert.Equal("Station B", chemin[1].Nom);
            Assert.Equal("Station C", chemin[2].Nom);
        }

        [Fact]
        public void BellmanFord_TrouveUnCheminSansCycleNegatif()
        {
            // Arrange
            Graphe graphe = new Graphe();
            graphe.AjouterNoeud(1, "Station A", 48.8566, 2.3522, "Ligne 1");
            graphe.AjouterNoeud(2, "Station B", 48.8570, 2.3530, "Ligne 1");
            graphe.AjouterNoeud(3, "Station C", 48.8580, 2.3540, "Ligne 1");

            graphe.AjouterLien(new Lien(1, 2, 5.0, "Ligne 1"));
            graphe.AjouterLien(new Lien(2, 3, 3.0, "Ligne 1"));

            // Act
            List<Noeud> chemin = graphe.BellmanFord(1, 3);

            // Assert
            Assert.Equal(3, chemin.Count);
            Assert.Equal("Station A", chemin[0].Nom);
            Assert.Equal("Station B", chemin[1].Nom);
            Assert.Equal("Station C", chemin[2].Nom);
        }
        [Fact]
        public void FloydWarshall_TrouveUnCheminValide()
        {
            // Arrange
            Graphe graphe = new Graphe();
            graphe.AjouterNoeud(1, "Station A", 48.8566, 2.3522, "Ligne 1");
            graphe.AjouterNoeud(2, "Station B", 48.8570, 2.3530, "Ligne 1");
            graphe.AjouterNoeud(3, "Station C", 48.8580, 2.3540, "Ligne 1");

            graphe.AjouterLien(new Lien(1, 2, 5.0, "Ligne 1"));
            graphe.AjouterLien(new Lien(2, 3, 3.0, "Ligne 1"));

            // Act
            List<Noeud> chemin = graphe.FloydWarshall(1, 3);

            // Assert
            Assert.Equal(3, chemin.Count);
            Assert.Equal("Station A", chemin[0].Nom);
            Assert.Equal("Station B", chemin[1].Nom);
            Assert.Equal("Station C", chemin[2].Nom);
        }

        [Fact]
        public void Aetoile_TrouveUnCheminValide()
        {
            // Arrange
            Graphe graphe = new Graphe();
            graphe.AjouterNoeud(1, "Station A", 48.8566, 2.3522, "Ligne 1");
            graphe.AjouterNoeud(2, "Station B", 48.8570, 2.3530, "Ligne 1");
            graphe.AjouterNoeud(3, "Station C", 48.8580, 2.3540, "Ligne 1");

            graphe.AjouterLien(new Lien(1, 2, 5.0, "Ligne 1"));
            graphe.AjouterLien(new Lien(2, 3, 3.0, "Ligne 1"));

            // Act
            List<Noeud> chemin = graphe.Aetoile(1, 3);

            // Assert
            Assert.Equal(3, chemin.Count);
            Assert.Equal("Station A", chemin[0].Nom);
            Assert.Equal("Station B", chemin[1].Nom);
            Assert.Equal("Station C", chemin[2].Nom);
        }

    }
}
