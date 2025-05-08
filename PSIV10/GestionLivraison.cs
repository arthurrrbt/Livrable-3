using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;

namespace PSIV10
{
    public partial class GestionLivraison : Form
    {
        private string chaineConnexion;
        private DataGridView dgvCommandesEnCours;
        private DataGridView dgvCommandesLivrees;
        private Button btnLivrer;

        public GestionLivraison(string connexion)
        {
            this.chaineConnexion = connexion;
            InitializeComponent();
            InitialiserInterface();
            ChargerCommandes();
        }

        private void InitialiserInterface()
        {
            this.Text = "Gestion des livraisons";
            this.Size = new Size(1400, 1200);
            this.StartPosition = FormStartPosition.CenterScreen;
            Label lblCommandesEnCours = new Label
            {
                Text = "Commandes à livrer",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(50, 20),
                Size = new Size(400, 30)
            };
            this.Controls.Add(lblCommandesEnCours);

            dgvCommandesEnCours = new DataGridView
            {
                Location = new Point(50, 60),
                Size = new Size(600, 600),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            this.Controls.Add(dgvCommandesEnCours);

            Label lblCommandesLivrees = new Label
            {
                Text = "Historique des livraisons",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(700, 20),
                Size = new Size(400, 30)
            };
            this.Controls.Add(lblCommandesLivrees);

            dgvCommandesLivrees = new DataGridView
            {
                Location = new Point(700, 60),
                Size = new Size(600, 600),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true
            };
            this.Controls.Add(dgvCommandesLivrees);

            btnLivrer = new Button
            {
                Text = "Marquer comme livré",
                Location = new Point(50, 680),
                Size = new Size(400, 40),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Font = new Font("Segeo UI", 12, FontStyle.Bold)
            };
            btnLivrer.Click += MarquerCommeLivre;
            AppliquerCoinsArrondis(btnLivrer, 20);
            this.Controls.Add(btnLivrer);

            Panel panelItineraire = new Panel
            {
                Location = new Point(50, 730),
                Size = new Size(800, 100),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblDepart = new Label
            {
                Text = "Station de départ :",
                Location = new Point(10, 15),
                Size = new Size(220, 25)
            };
            ComboBox cmbDepart = new ComboBox
            {
                Location = new Point(230, 10),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Label lblArrivee = new Label
            {
                Text = "Station d'arrivée :",
                Location = new Point(10, 45),
                Size = new Size(220, 25)
            };
            ComboBox cmbArrivee = new ComboBox
            {
                Location = new Point(230, 40),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Button btnCalculer = new Button
            {
                Text = "Calculer l'itinéraire",
                Location = new Point(400, 25),
                Size = new Size(250, 30),
                BackColor = Color.LightBlue,
                FlatStyle = FlatStyle.Flat
            };

            try
            {
                string[] lignes = File.ReadAllLines("MetroParisNoeud.csv");
                for (int i = 1; i < lignes.Length; i++)
                {
                    string[] colonnes = lignes[i].Split(';');
                    if (colonnes.Length >= 3)
                    {
                        string nomStation = colonnes[2];
                        bool existeDeja = false;
                        foreach (var item in cmbDepart.Items)
                        {
                            if (item.Equals(nomStation))
                            {
                                existeDeja = true;
                                break;
                            }
                        }

                        if (!existeDeja)
                        {
                            cmbDepart.Items.Add(nomStation);
                            cmbArrivee.Items.Add(nomStation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des stations : {ex.Message}");
            }

            btnCalculer.Click += (s, e) =>
            {
                if (cmbDepart.SelectedItem == null || cmbArrivee.SelectedItem == null)
                {
                    MessageBox.Show("Veuillez sélectionner les stations de départ et d'arrivée.");
                    return;
                }

                GrapheForm grapheForm = new GrapheForm();
                grapheForm.Show();
            
            };

            panelItineraire.Controls.AddRange(new Control[] { 
                lblDepart, cmbDepart,
                lblArrivee, cmbArrivee,
                btnCalculer
            });

            this.Controls.Add(panelItineraire);
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
        private void ChargerCommandes()
        {
            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();

                    string requeteEnCours = @"
                        SELECT 
                            c.IdCommande,
                            c.Nom as 'Plat',
                            p.Adresse as 'Adresse de livraison',
                            p.MetroProche as 'Métro le plus proche'
                        FROM Commande c
                        JOIN Client cl ON c.IdClient = cl.IdClient
                        JOIN Particulier p ON cl.IdParticulier = p.IdParticulier
                        WHERE c.IdCommande NOT IN (SELECT COALESCE(IdCommande,0) FROM Livraison)
                        ORDER BY c.DateCommande DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(requeteEnCours, connexion))
                    {
                        DataTable dtEnCours = new DataTable();
                        adapter.Fill(dtEnCours);
                        dgvCommandesEnCours.DataSource = dtEnCours;
                    }

                    string requeteLivrees = @"
                        SELECT 
                            l.IdLivraison as 'N° Livraison',
                            c.Nom as 'Plat',
                            p.Adresse as 'Adresse livrée',
                            p.MetroProche as 'Métro proche',
                            DATE_FORMAT(c.DateCommande, '%d/%m/%Y %H:%i') as 'Date de commande'
                        FROM Livraison l
                        JOIN Commande c ON l.IdCommande = c.IdCommande
                        JOIN Client cl ON c.IdClient = cl.IdClient
                        JOIN Particulier p ON cl.IdParticulier = p.IdParticulier
                        ORDER BY c.DateCommande DESC";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(requeteLivrees, connexion))
                    {
                        DataTable dtLivrees = new DataTable();
                        adapter.Fill(dtLivrees);
                        dgvCommandesLivrees.DataSource = dtLivrees;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des commandes : " + ex.Message);
            }
        }

        private void MarquerCommeLivre(object sender, EventArgs e)
        {
            if (dgvCommandesEnCours.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande à livrer.");
                return;
            }

            try
            {
                int idCommande = Convert.ToInt32(dgvCommandesEnCours.SelectedRows[0].Cells["IdCommande"].Value);

                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();
                    using (MySqlTransaction transaction = connexion.BeginTransaction())
                    {
                        try
                        {
                            string requeteLivraison = @"
                                INSERT INTO Livraison (PositionLive, IdAvis, IdNotification, IdAdresse, IdCommande)
                                VALUES ('Livré', 
                                        (SELECT MAX(IdAvis) FROM Avis), 
                                        (SELECT MAX(IdNotification) FROM Notification), 
                                        (SELECT IdAdresse FROM Commande WHERE IdCommande = @IdCommande),
                                        @IdCommande)";

                            using (MySqlCommand cmd = new MySqlCommand(requeteLivraison, connexion, transaction))
                            {
                                cmd.Parameters.AddWithValue("@IdCommande", idCommande);
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("Commande marquée comme livrée avec succès!");
                            ChargerCommandes();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Erreur lors de la livraison : {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }
    }
}
