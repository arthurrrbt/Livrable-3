using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using MySql.Data.MySqlClient;
using System.Globalization;
using System.IO;

namespace PSIV10
{
    public class GrapheClientCuisinier
    {
        #region Propriétés
        // Propriétés d'analyse du graphe
        public bool EstBiparti { get; set; }
        public bool EstPlanaire { get; set; }
        public int NombreChromatique { get; private set; }
        public List<Sommet> Sommets { get; set; } = new List<Sommet>();
        public List<Arete> Aretes { get; set; } = new List<Arete>();
        public int IdClient { get; set; }
        public string NomClient { get; set; }
        public int IdCuisinier { get; set; }
        public string NomCuisinier { get; set; }
        private string chaineConnexion;
        #endregion
        public GrapheClientCuisinier(string connexion)
        {
            chaineConnexion = connexion;
        }


        public void AjouterSommet(Sommet sommet)
        {
            Sommets.Add(sommet);
        }
        public void AjouterArete(Arete arete)
        {
            Aretes.Add(arete);
        }
        public void ChargerClientsEtCuisiniers(string connectionString)
        {
            Sommets.Clear();
            var sommets = new List<Sommet>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                var tousLesPoints = new List<(int id, string nom, string prenom, string type)>();

                using (var cmd = new MySqlCommand(@"
                    SELECT c.IdClient, p.Nom, p.Prenom 
                    FROM Client c 
                    JOIN Particulier p ON c.IdParticulier = p.IdParticulier
                    WHERE NOT EXISTS (
                        SELECT 1 FROM Admin a 
                        WHERE a.IdParticulier = p.IdParticulier
                    )", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tousLesPoints.Add((
                            reader.GetInt32("IdClient"),
                            reader.GetString("Nom"),
                            reader.GetString("Prenom"),
                            "Client"
                        ));
                    }
                }

                using (var cmd = new MySqlCommand(@"
                    SELECT c.IdCuisinier, p.Nom, p.Prenom 
                    FROM Cuisinier c 
                    JOIN Particulier p ON c.IdParticulier = p.IdParticulier
                    WHERE NOT EXISTS (
                        SELECT 1 FROM Admin a 
                        WHERE a.IdParticulier = p.IdParticulier
                    )", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tousLesPoints.Add((
                            reader.GetInt32("IdCuisinier"),
                            reader.GetString("Nom"),
                            reader.GetString("Prenom"),
                            "Cuisinier"
                        ));
                    }
                }

                double rayon = 350;
                double centreX = 600;
                double centreY = 400;

                for (int i = 0; i < tousLesPoints.Count; i++)
                {
                    double angle = (2 * Math.PI * i) / tousLesPoints.Count;
                    double x = centreX + rayon * Math.Cos(angle);
                    double y = centreY + rayon * Math.Sin(angle);

                    var point = tousLesPoints[i];
                    sommets.Add(new Sommet
                    {
                        Id = point.id,
                        Type = point.type,
                        Nom = $"{point.prenom} {point.nom}",
                        X = x,
                        Y = y,
                        AutresIds = new List<int> { point.id }
                    });
                }

            }


            Sommets.AddRange(sommets);
        }

        public void ChargerCommandes(string connectionString)
        {
            List<Arete> aretes = new List<Arete>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT IdClient, IdCuisinier FROM Commande", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Arete a = new Arete
                        {
                            IdClient = reader.GetInt32("idClient"),
                            IdCuisinier = reader.GetInt32("idCuisinier"),
                            NbCommandes = 1
                        };
                        aretes.Add(a);
                    }
                }

                conn.Close();
            }

            GrapheCommandes(aretes);
        }


        public void GrapheCommandes(List<Arete> commandes)
        {
            Aretes.Clear();
            foreach (var commande in commandes)
            {
                Sommet client = null;
                foreach (var s in Sommets)
                {
                    if (s.Id == commande.IdClient && s.Type == "Client")
                    {
                        client = s;
                        break;
                    }
                }

                Sommet cuisinier = null;
                foreach (var s in Sommets) 
                {
                    if (s.Id == commande.IdCuisinier && s.Type == "Cuisinier")
                    {
                        cuisinier = s;
                        break;
                    }
                }
                if (client != null && cuisinier != null)
                {
                    Arete areteExistante = null;
                    foreach (Arete a in Aretes)
                    {
                        if (a.IdClient == commande.IdClient && a.IdCuisinier == commande.IdCuisinier)
                        {
                            areteExistante = a;
                            break;
                        }
                    }

                    if (areteExistante != null)
                    {
                        areteExistante.NbCommandes++;
                    }
                    else
                    {
                        var nouvelleArete = new Arete
                        {
                            IdClient = commande.IdClient,
                            IdCuisinier = commande.IdCuisinier,
                            NbCommandes = 1
                        };
                        Aretes.Add(nouvelleArete);
                    }
                }
            }
        }

        public void AfficherGraphe(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            const int TAILLE_CERCLE = 50; 
            Font fontNormale = new Font("Arial", 6);

            StringFormat centrerTexte = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            foreach (var arete in Aretes)
            {
                Sommet client = null;
                foreach (var sommet in Sommets)
                {
                    if (sommet.AutresIds.Contains(arete.IdClient))
                    {
                        client = sommet;
                        break;
                    }
                }

                Sommet cuisinier = null;
                foreach (var sommet in Sommets)
                {
                    if (sommet.AutresIds.Contains(arete.IdCuisinier))
                    {
                        cuisinier = sommet;
                        break;
                    }
                }

                if (client != null && cuisinier != null)
                {
                    using (Pen styloGris = new Pen(Color.Gray, 2))
                    {
                        g.DrawLine(styloGris,
                            (float)client.X, (float)client.Y,
                            (float)cuisinier.X, (float)cuisinier.Y);

                        float centreX = (float)((client.X + cuisinier.X) / 2);
                        float centreY = (float)((client.Y + cuisinier.Y) / 2);

                        g.FillEllipse(Brushes.White, centreX - 10, centreY - 10, 20, 20);
                        g.DrawString(arete.NbCommandes.ToString(),
                            fontNormale,
                            Brushes.Black,
                            new RectangleF(centreX - 10, centreY - 10, 20, 20),
                            centrerTexte);
                    }
                }
            }
            foreach (var point in Sommets)
            {
                Color couleur;
                if (point.Couleur == default)
                    couleur = Color.Gray;
                else
                    couleur = point.Couleur;
                using (Brush pinceau = new SolidBrush(couleur))
                {
                    g.FillEllipse(pinceau,
                        (float)point.X - TAILLE_CERCLE / 2,
                        (float)point.Y - TAILLE_CERCLE / 2,
                        TAILLE_CERCLE,
                        TAILLE_CERCLE);
                    g.DrawString(point.Nom,
                        fontNormale,
                        Brushes.White,
                        new RectangleF(
                            (float)point.X - TAILLE_CERCLE / 2,
                            (float)point.Y - TAILLE_CERCLE / 2,
                            TAILLE_CERCLE,
                            TAILLE_CERCLE),
                        centrerTexte);
                }
            }
        }
        public void ResetGraphe()
        {
            foreach (var sommet in Sommets)
            {
            sommet.Couleur = default;
            }
        }
        public void ColorerGraphe()
        {
            Dictionary<Sommet, int> degres = new Dictionary<Sommet, int>();
            foreach (var sommet in Sommets)
            {
              int degre = 0;
                foreach (var a in Aretes)
                {
                    if (a.IdClient == sommet.Id || a.IdCuisinier == sommet.Id)
                    {
                        degre++;
                    }
                }
                degres[sommet] = degre;
            }

            List<Sommet> sommetsTries = new List<Sommet>(Sommets);
            for (int i = 0; i < sommetsTries.Count - 1; i++)
            {
                for (int j = 0; j < sommetsTries.Count - 1 - i; j++)
                {
                    if (degres[sommetsTries[j]] < degres[sommetsTries[j + 1]])
                    {
                        Sommet temp = sommetsTries[j];
                        sommetsTries[j] = sommetsTries[j + 1];
                        sommetsTries[j + 1] = temp;
                    }
                }
            }

            Dictionary<int, int> couleurs = new Dictionary<int, int>();

            foreach (var sommet in sommetsTries)
            {
                List<int> couleursUtilisees = new List<int>();
                foreach (var arete in Aretes)
                {
                    if (arete.IdClient == sommet.Id && couleurs.ContainsKey(arete.IdCuisinier))
                    {
                        couleursUtilisees.Add(couleurs[arete.IdCuisinier]);
                    }
                    if (arete.IdCuisinier == sommet.Id && couleurs.ContainsKey(arete.IdClient))
                    {
                        couleursUtilisees.Add(couleurs[arete.IdClient]);
                    }
                }

                int couleur = 0;
                while (couleursUtilisees.Contains(couleur))
                {
                    couleur++;
                }
                couleurs[sommet.Id] = couleur;
            }
            Color[] paletteColors = {
                Color.Red, Color.Blue, Color.Green, Color.Orange,
                Color.Purple, Color.Yellow, Color.Pink, Color.Brown
            };

            foreach (var sommet in Sommets)
            {
                sommet.Couleur = paletteColors[couleurs[sommet.Id] % paletteColors.Length];
            }

            NombreChromatique = couleurs.Values.Max() + 1;
            EstBiparti = NombreChromatique == 2;
            EstPlanaire = NombreChromatique <= 4;
        }
        public bool VerifierBiparti()
        {
            int[] couleurs = new int[Sommets.Count];

            for (int i = 0; i < Sommets.Count; i++)
            {
                if (couleurs[i] == 0)
                {
                    couleurs[i] = 1;

                    for (int j = 0; j < Sommets.Count; j++)
                    {
                        if (SontVoisins(Sommets[i], Sommets[j]))
                        {
                            if (couleurs[j] == 0)
                                couleurs[j] = 2; 
                            else if (couleurs[j] == couleurs[i])
                                return false; 
                        }
                    }
                }
            }

            return true;
        }


        private bool SontVoisins(Sommet s1, Sommet s2)
        {
            for (int i = 0; i < Aretes.Count; i++)
            {
                if ((Aretes[i].IdClient == s1.Id && Aretes[i].IdCuisinier == s2.Id) ||
                    (Aretes[i].IdClient == s2.Id && Aretes[i].IdCuisinier == s1.Id))
                {
                    return true;
                }
            }
            return false;
        }
        public bool VerifierPlanaire()
        {
            for (int i = 0; i < Aretes.Count; i++)
            {
                for (int j = i + 1; j < Aretes.Count; j++)
                {
                    var arete1 = Aretes[i];
                    var arete2 = Aretes[j];
                    Sommet debut1 = null;
                    Sommet fin1 = null;
                    Sommet debut2 = null;
                    Sommet fin2 = null;

                    foreach (var s in Sommets)
                    {
                        if (s.Id == arete1.IdClient) debut1 = s;
                        if (s.Id == arete1.IdCuisinier) fin1 = s;
                        if (s.Id == arete2.IdClient) debut2 = s;
                        if (s.Id == arete2.IdCuisinier) fin2 = s;
                    }
                    if (SecroisentElles(
                        debut1.X, debut1.Y, fin1.X, fin1.Y,
                        debut2.X, debut2.Y, fin2.X, fin2.Y))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private bool SecroisentElles(double x1, double y1, double x2, double y2, 
                                    double x3, double y3, double x4, double y4)
        {
            bool directionsDifferentes = 
                DirectionDifferente(x1, y1, x2, y2, x3, y3) != DirectionDifferente(x1, y1, x2, y2, x4, y4) &&
                DirectionDifferente(x3, y3, x4, y4, x1, y1) != DirectionDifferente(x3, y3, x4, y4, x2, y2);
            bool pointsCommuns = 
                SontMemePoint(x1, y1, x3, y3) ||
                SontMemePoint(x1, y1, x4, y4) ||
                SontMemePoint(x2, y2, x3, y3) ||
                SontMemePoint(x2, y2, x4, y4);

            return directionsDifferentes && !pointsCommuns;
        }

        private bool SontMemePoint(double x1, double y1, double x2, double y2)
        {
            double marge = 0.0000000001;
            return Math.Abs(x1 - x2) < marge && Math.Abs(y1 - y2) < marge;
        }

        private bool DirectionDifferente(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            double resultat = (y2 - y1) * (x3 - x2) - (x2 - x1) * (y3 - y2);
            return resultat > 0;
        }

    }

}



