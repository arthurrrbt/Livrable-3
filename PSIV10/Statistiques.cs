using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PSIV10
{
    public partial class Statistiques : Form
    {
        #region Declaration des variables
        private string chaineConnexion;
        private DataGridView dataGridViewLivraisonsCuisiniers;
        private DataGridView dataGridViewCommandesPeriode;
        private DataGridView dataGridViewMoyennePrix;
        private DataGridView dataGridViewMoyenneComptes;
        private DataGridView dataGridViewCommandesNationalite;
        private DataGridView dataGridViewMoyenneAvis;
        private DataGridView dataGridViewMoyenneRentables;
        private DataGridView dataGridViewPerfCodePromo;
        private DataGridView dataGridViewTopPlats;
        private DataGridView dataGridViewModesPaiement;
        private Button btnFermer;
        #endregion

        public Statistiques(string connexion)
        {
            chaineConnexion = connexion;
            InitializeComponent();
            InitialiserControles();
            ChargerStatistiques();
        }

        private void InitialiserControles()
        {
            this.Text = "Statistiques";
            this.Size = new Size(500, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
               Label lblTitre = new Label
            {
                Text = "Statistiques :",
                Location = new Point(50, 20),
                Size = new Size(250, 50),
                Font = new Font("Segoe UI", 12, FontStyle.Bold | FontStyle.Underline)
            };
            this.Controls.Add(lblTitre);
            // Créer les boutons pour chaque statistique
            Button btnLivraisonsCuisiniers = new Button
            {
                Text = "Livraisons par cuisinier",
                Location = new Point(50, 80),
                Size = new Size(400, 40)
            };
            btnLivraisonsCuisiniers.Click += (s, e) => AfficherStats("Livraisons par cuisinier", dataGridViewLivraisonsCuisiniers);
            btnLivraisonsCuisiniers.FlatStyle = FlatStyle.Flat;
            btnLivraisonsCuisiniers.BackColor = Color.LightGray;
            btnLivraisonsCuisiniers.ForeColor = Color.Black;
            btnLivraisonsCuisiniers.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnLivraisonsCuisiniers.FlatAppearance.BorderSize = 0; 
            AppliquerCoinsArrondis(btnLivraisonsCuisiniers, 20); // Appliquer coins arrondis
            this.Controls.Add(btnLivraisonsCuisiniers);

            Button btnCommandesPeriode = new Button
            {
                Text = "Commandes par période",
                Location = new Point(50, 140),
                Size = new Size(400, 40)
            };
            btnCommandesPeriode.Click += (s, e) => AfficherStats("Commandes par période", dataGridViewCommandesPeriode);
            btnCommandesPeriode.FlatStyle = FlatStyle.Flat;
            btnCommandesPeriode.BackColor = Color.LightGray;
            btnCommandesPeriode.ForeColor = Color.Black;
            btnCommandesPeriode.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnCommandesPeriode.FlatAppearance.BorderSize = 0;
            AppliquerCoinsArrondis(btnCommandesPeriode, 20); // Appliquer coins arrondis
            this.Controls.Add(btnCommandesPeriode);

            Button btnMoyennePrix = new Button
            {
                Text = "Moyenne des prix des commandes",
                Location = new Point(50, 200),
                Size = new Size(400, 40)
            };
            btnMoyennePrix.Click += (s, e) => AfficherStats("Moyenne des prix des commandes", dataGridViewMoyennePrix);
            btnMoyennePrix.FlatStyle = FlatStyle.Flat;
            btnMoyennePrix.BackColor = Color.LightGray;
            btnMoyennePrix.ForeColor = Color.Black;
            btnMoyennePrix.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnMoyennePrix.FlatAppearance.BorderSize = 0;
            AppliquerCoinsArrondis(btnMoyennePrix, 20); // Appliquer coins arrondis
            this.Controls.Add(btnMoyennePrix);

            Button btnMoyenneComptes = new Button
            {
                Text = "Moyenne des comptes",
                Location = new Point(50, 260),
                Size = new Size(400, 40)
            };
            btnMoyenneComptes.Click += (s, e) => AfficherStats("Moyenne des comptes", dataGridViewMoyenneComptes);
            btnMoyenneComptes.FlatStyle = FlatStyle.Flat;
            btnMoyenneComptes.BackColor = Color.LightGray;
            btnMoyenneComptes.ForeColor = Color.Black;
            btnMoyenneComptes.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnMoyenneComptes.FlatAppearance.BorderSize = 0;
            AppliquerCoinsArrondis(btnMoyenneComptes, 20); // Appliquer coins arrondis
            this.Controls.Add(btnMoyenneComptes);

            Button btnCommandesNationalite = new Button
            {
                Text = "Commandes par nationalité",
                Location = new Point(50, 320),
                Size = new Size(400, 40)
            };
            btnCommandesNationalite.Click += (s, e) => AfficherStats("Commandes par nationalité", dataGridViewCommandesNationalite);
            btnCommandesNationalite.FlatStyle = FlatStyle.Flat;
            btnCommandesNationalite.BackColor = Color.LightGray;
            btnCommandesNationalite.ForeColor = Color.Black;
            btnCommandesNationalite.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnCommandesNationalite.FlatAppearance.BorderSize = 0;
            AppliquerCoinsArrondis(btnCommandesNationalite, 20); // Appliquer coins arrondis
            this.Controls.Add(btnCommandesNationalite);

            Button btnMoyenneAvis = new Button
            {
                Text = "Moyenne des avis",
                Location = new Point(50, 380),
                Size = new Size(400, 40)
            };
            btnMoyenneAvis.Click += (s, e) => AfficherStats("Moyenne des avis", dataGridViewMoyenneAvis);
            btnMoyenneAvis.FlatStyle = FlatStyle.Flat;
            btnMoyenneAvis.BackColor = Color.LightGray;
            btnMoyenneAvis.ForeColor = Color.Black;
            btnMoyenneAvis.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnMoyenneAvis.FlatAppearance.BorderSize = 0;
            AppliquerCoinsArrondis(btnMoyenneAvis, 20); // Appliquer coins arrondis
            this.Controls.Add(btnMoyenneAvis);

            Button btnPlatRentables = new Button
            {
                Text = "Plats rentables",
                Location = new Point(50, 440),
                Size = new Size(400, 40)
            };
            btnPlatRentables.Click += (s, e) => AfficherStats("Plats rentables", dataGridViewMoyenneRentables);
            btnPlatRentables.FlatStyle = FlatStyle.Flat;
            btnPlatRentables.BackColor = Color.LightGray;
            btnPlatRentables.ForeColor = Color.Black;
            btnPlatRentables.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnPlatRentables.FlatAppearance.BorderSize = 0;
            AppliquerCoinsArrondis(btnPlatRentables, 20); // Appliquer coins arrondis
            this.Controls.Add(btnPlatRentables); 

            Button btnPerfCodePromo = new Button
            {
                Text = "Performance des codes promo",
                Location = new Point(50, 500),
                Size = new Size(400, 40)
            };
            btnPerfCodePromo.Click += (s, e) => AfficherStats("Performance des codes promo", dataGridViewPerfCodePromo);
            btnPerfCodePromo.FlatStyle = FlatStyle.Flat;
            btnPerfCodePromo.BackColor = Color.LightGray;
            btnPerfCodePromo.ForeColor = Color.Black;
            btnPerfCodePromo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnPerfCodePromo.FlatAppearance.BorderSize = 0;
            AppliquerCoinsArrondis(btnPerfCodePromo, 20); // Appliquer coins arrondis
            this.Controls.Add(btnPerfCodePromo);

            /// <summary>Bouton Top 5 plats</summary>
            Button btnTopPlats = new Button
            {
                Text = "Top 5 des plats les plus commandés",
                Location = new Point(50, 560),
                Size = new Size(400, 40),
            };
            btnTopPlats.Click += (s, e) => AfficherStats("Top 5 des plats les plus commandés", dataGridViewTopPlats);
            btnTopPlats.FlatStyle = FlatStyle.Flat;
            btnTopPlats.BackColor = Color.LightGray;
            btnTopPlats.ForeColor = Color.Black;
            btnTopPlats.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnTopPlats.FlatAppearance.BorderSize = 0;
            AppliquerCoinsArrondis(btnTopPlats, 20); // Appliquer coins arrondis
            this.Controls.Add(btnTopPlats);

            Button btnModesPaiement = new Button
            {
                Text = "Modes de paiement préférés",
                Location = new Point(50, 620),
                Size = new Size(400, 40)
            };
            btnModesPaiement.Click += (s, e) => AfficherStats("Modes de paiement préférés", dataGridViewModesPaiement);
            btnModesPaiement.FlatStyle = FlatStyle.Flat;
            btnModesPaiement.BackColor = Color.LightGray;
            btnModesPaiement.ForeColor = Color.Black;
            btnModesPaiement.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnModesPaiement.FlatAppearance.BorderSize = 0;
            AppliquerCoinsArrondis(btnModesPaiement, 20);
            this.Controls.Add(btnModesPaiement);
            btnFermer = new Button
            {
                Text = "Fermer",
                Location = new Point(50, 680),
                Size = new Size(400, 40)
            };
            btnFermer.Click += (s, e) => this.Close();
            btnFermer.FlatStyle = FlatStyle.Flat;
            btnFermer.BackColor = Color.LightGray;
            btnFermer.ForeColor = Color.Black;
            btnFermer.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnFermer.FlatAppearance.BorderSize = 0;
            AppliquerCoinsArrondis(btnFermer, 20); // Appliquer coins arrondis
            this.Controls.Add(btnFermer);
            // Initialiser les DataGridViews mais ne pas les ajouter au formulaire principal
            InitialiserDataGridViews();
        }

        private void InitialiserDataGridViews()
        {
            dataGridViewLivraisonsCuisiniers = new DataGridView
            {
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };

            dataGridViewCommandesPeriode = new DataGridView
            {
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };

            dataGridViewMoyennePrix = new DataGridView
            {
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };

            dataGridViewMoyenneComptes = new DataGridView
            {
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };

            dataGridViewCommandesNationalite = new DataGridView
            {
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };

            dataGridViewMoyenneAvis = new DataGridView
            {
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };

            dataGridViewMoyenneRentables = new DataGridView
            {
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };

            dataGridViewPerfCodePromo = new DataGridView
            {
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };

            dataGridViewTopPlats = new DataGridView
            {
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };

            dataGridViewModesPaiement = new DataGridView
            {
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };
        }

        public void AppliquerCoinsArrondis(Button bouton, int rayon)
        {
            GraphicsPath path = new GraphicsPath();
            int w = bouton.Width;
            int h = bouton.Height;

            path.AddArc(0, 0, rayon, rayon, 180, 90);
            path.AddArc(w - rayon, 0, rayon, rayon, 270, 90);
            path.AddArc(w - rayon, h - rayon, rayon, rayon, 0, 90);
            path.AddArc(0, h - rayon, rayon, rayon, 90, 90);
            path.CloseFigure();

            bouton.Region = new Region(path);
        }
        private void AfficherStats(string titre, DataGridView dgv)
        {
            Form form = new Form
            {
                Text = titre,
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterParent
            };

            DataGridView newDgv = new DataGridView
            {
                DataSource = dgv.DataSource,
                AllowUserToAddRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Dock = DockStyle.Fill
            };



            form.Controls.Add(newDgv);
            form.ShowDialog();
        }

        private void ChargerStatistiques()
        {
            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();

                    string requeteLivraisons = @"
                    SELECT p.Nom, p.Prenom, COUNT(c.IdCommande) as NombreLivraisons
                    FROM Cuisinier cu
                    JOIN Particulier p ON cu.IdParticulier = p.IdParticulier
                    LEFT JOIN Commande c ON cu.IdCuisinier = c.IdCuisinier
                    GROUP BY cu.IdCuisinier, p.Nom, p.Prenom";

                    using (MySqlCommand commande = new MySqlCommand(requeteLivraisons, connexion))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(commande))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridViewLivraisonsCuisiniers.DataSource = dt;
                        }
                    }

                    string requeteCommandes = @"
                    SELECT DATE_FORMAT(DateCommande, '%Y-%m') as Periode,
                           COUNT(*) as NombreCommandes
                    FROM Commande
                    GROUP BY DATE_FORMAT(DateCommande, '%Y-%m')
                    ORDER BY Periode DESC";

                    using (MySqlCommand commande = new MySqlCommand(requeteCommandes, connexion))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(commande))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridViewCommandesPeriode.DataSource = dt;
                        }
                    }

                    string requeteMoyennePrix = @"
                    SELECT AVG(p.Prix) as PrixMoyen
                    FROM Commande c
                    JOIN Plat p ON c.IdPlat = p.IdPlat";

                    using (MySqlCommand commande = new MySqlCommand(requeteMoyennePrix, connexion))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(commande))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridViewMoyennePrix.DataSource = dt;
                        }
                    }
                    string requeteMoyenneComptes = @"
                SELECT 
                    COUNT(DISTINCT c.IdClient) as NombreClients,
                    COUNT(co.IdCommande) as NombreCommandes,
                    CAST(COUNT(co.IdCommande) / COUNT(DISTINCT c.IdClient) AS DECIMAL(10,2)) as MoyenneCommandesParClient
                FROM Client c
                LEFT JOIN Commande co ON c.IdClient = co.IdClient";

                    using (MySqlCommand commande = new MySqlCommand(requeteMoyenneComptes, connexion))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(commande))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridViewMoyenneComptes.DataSource = dt;
                        }
                    }

                    string requeteCommandesNationalite = @"
                SELECT 
                    p.TypePlat,
                    COUNT(c.IdCommande) as NombreCommandes,
                    CAST(COUNT(c.IdCommande) * 100.0 / (SELECT COUNT(*) FROM Commande) AS DECIMAL(10,2)) as PourcentageDePart
                FROM Plat p
                JOIN Commande c ON p.IdPlat = c.IdPlat
                GROUP BY p.TypePlat
                ORDER BY NombreCommandes DESC";

                    using (MySqlCommand commande = new MySqlCommand(requeteCommandesNationalite, connexion))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(commande))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridViewCommandesNationalite.DataSource = dt;
                        }
                    }

                    string requeteMoyenneAvis = @"
                SELECT 
                    CAST(AVG(Note) AS DECIMAL(10,2)) as MoyenneGenerale,
                    COUNT(IdAvis) as NombreTotal,
                    MIN(Note) as NoteMinimum,
                    MAX(Note) as NoteMaximum
                FROM Avis";

                    using (MySqlCommand commande = new MySqlCommand(requeteMoyenneAvis, connexion))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(commande))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dataGridViewMoyenneAvis.DataSource = dt;
                        }
                    }
            string requetePlatRentables = @"
                SELECT 
                    p.NomPlat,
                    COUNT(c.IdCommande) as NombreVentes,
                    p.Prix as PrixUnitaire,
                    SUM(c.Prix) as ChiffreAffaires,
                    CAST(AVG(c.Prix) AS DECIMAL(10,2)) as PrixMoyen,
                    CAST((COUNT(c.IdCommande) * p.Prix) AS DECIMAL(10,2)) as RevenuTotal
                FROM Plat p
                LEFT JOIN Commande c ON p.IdPlat = c.IdPlat
                GROUP BY p.IdPlat, p.NomPlat, p.Prix
                ORDER BY RevenuTotal DESC
                LIMIT 10";

            using (MySqlCommand commande = new MySqlCommand(requetePlatRentables, connexion))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(commande))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewMoyenneRentables.DataSource = dt;
                }
            }
            string requetePerformancePromo = @"
                SELECT 
                    CodePromoUtilise,
                    COUNT(*) as NombreUtilisations,
                    CAST(AVG(Reduction * 100) AS DECIMAL(10,2)) as PourcentageReduction,
                    CAST(SUM(Prix) AS DECIMAL(10,2)) as ChiffreAffairesAvecPromo,
                    CAST(SUM(Prix / (1-Reduction)) AS DECIMAL(10,2)) as ChiffreAffairesSansPromo,
                    CAST(SUM(Prix / (1-Reduction) - Prix) AS DECIMAL(10,2)) as EconomieClientsTotale
                FROM Commande
                WHERE CodePromoUtilise IS NOT NULL
                GROUP BY CodePromoUtilise
                ORDER BY NombreUtilisations DESC";

            using (MySqlCommand commande = new MySqlCommand(requetePerformancePromo, connexion))
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(commande))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridViewPerfCodePromo.DataSource = dt;
                }
            }
                string requeteTopPlats = @"
                    SELECT 
                        p.NomPlat as Plat,
                        COUNT(c.IdCommande) as NombreCommandes,
                        SUM(c.Prix) as ChiffreAffaires,
                        CAST(AVG(c.Prix) AS DECIMAL(10,2)) as PrixMoyen
                    FROM Plat p
                    JOIN Commande c ON p.IdPlat = c.IdPlat
                    GROUP BY p.IdPlat, p.NomPlat
                    ORDER BY NombreCommandes DESC
                    LIMIT 5";
                using (MySqlCommand commande = new MySqlCommand(requeteTopPlats, connexion))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(commande))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridViewTopPlats.DataSource = dt;
                    }
            
                }
                string requeteModesPaiement = @"
                    SELECT 
                        MoyenPaiement as 'Mode de paiement',
                        COUNT(*) as 'Nombre d''utilisations',
                        CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Commande) AS DECIMAL(10,2)) as 'Pourcentage (%)',
                        CAST(SUM(Prix) AS DECIMAL(10,2)) as 'Montant total',
                        CAST(AVG(Prix) AS DECIMAL(10,2)) as 'Panier moyen'
                    FROM Commande
                    GROUP BY MoyenPaiement
                    ORDER BY COUNT(*) DESC";

                using (MySqlCommand commande = new MySqlCommand(requeteModesPaiement, connexion))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(commande))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridViewModesPaiement.DataSource = dt;
                    }
                }

            }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des statistiques : {ex.Message}",
                              "Erreur",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }
    }
}
