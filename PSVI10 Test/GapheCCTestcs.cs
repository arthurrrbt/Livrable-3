using Xunit;
using System.Collections.Generic;
using PSIV10;
using System.Drawing;

namespace PSIV10.TestsClientCuisinier
{
    public class GrapheClientCuisinierTests
    {
        [Fact]
        public void TestAjouterSommetEtArete()
        {
            var graphe = new GrapheClientCuisinier("fake-connection");

            var client = new Sommet { Id = 1, Type = "Client", Nom = "Client A" };
            var cuisinier = new Sommet { Id = 2, Type = "Cuisinier", Nom = "Cuisinier B" };

            graphe.AjouterSommet(client);
            graphe.AjouterSommet(cuisinier);

            var arete = new Arete { IdClient = 1, IdCuisinier = 2, NbCommandes = 1 };
            graphe.AjouterArete(arete);

            Assert.Contains(client, graphe.Sommets);
            Assert.Contains(cuisinier, graphe.Sommets);
            Assert.Contains(arete, graphe.Aretes);
        }

        [Fact]
        public void TestColorerGrapheEtVerifierBiparti()
        {
            var graphe = new GrapheClientCuisinier("fake-connection");

            var client1 = new Sommet { Id = 1, Type = "Client", Nom = "Client A" };
            var cuisinier1 = new Sommet { Id = 2, Type = "Cuisinier", Nom = "Cuisinier A" };
            var client2 = new Sommet { Id = 3, Type = "Client", Nom = "Client B" };
            var cuisinier2 = new Sommet { Id = 4, Type = "Cuisinier", Nom = "Cuisinier B" };

            graphe.Sommets.AddRange(new[] { client1, cuisinier1, client2, cuisinier2 });

            graphe.GrapheCommandes(new List<Arete>
            {
                new Arete { IdClient = 1, IdCuisinier = 2, NbCommandes = 1 },
                new Arete { IdClient = 3, IdCuisinier = 4, NbCommandes = 1 }
            });

            graphe.ColorerGraphe();

            Assert.True(graphe.EstBiparti);
            Assert.True(graphe.EstPlanaire);
            Assert.True(graphe.NombreChromatique <= 2);
        }

        [Fact]
        public void TestVerifierPlanaire_True()
        {
            var graphe = new GrapheClientCuisinier("fake-connection");

            var s1 = new Sommet { Id = 1, X = 0, Y = 0 };
            var s2 = new Sommet { Id = 2, X = 1, Y = 0 };
            var s3 = new Sommet { Id = 3, X = 0, Y = 1 };
            var s4 = new Sommet { Id = 4, X = 1, Y = 1 };

            graphe.Sommets.AddRange(new[] { s1, s2, s3, s4 });

            graphe.Aretes.AddRange(new[]
            {
                new Arete { IdClient = 1, IdCuisinier = 2 },
                new Arete { IdClient = 3, IdCuisinier = 4 }
            });

            bool result = graphe.VerifierPlanaire();
            Assert.True(result);
        }

        [Fact]
        public void TestVerifierPlanaire_False()
        {
            var graphe = new GrapheClientCuisinier("fake-connection");

            var s1 = new Sommet { Id = 1, X = 0, Y = 0 };
            var s2 = new Sommet { Id = 2, X = 1, Y = 1 };
            var s3 = new Sommet { Id = 3, X = 1, Y = 0 };
            var s4 = new Sommet { Id = 4, X = 0, Y = 1 };

            graphe.Sommets.AddRange(new[] { s1, s2, s3, s4 });

            graphe.Aretes.AddRange(new[]
            {
                new Arete { IdClient = 1, IdCuisinier = 2 },
                new Arete { IdClient = 3, IdCuisinier = 4 }
            });

            bool result = graphe.VerifierPlanaire();
            Assert.False(result); // car les arêtes se croisent
        }
    }
}
