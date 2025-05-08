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
    public partial class GestionCommandes : Form
    {
        private string chaineConnexion;
        private DataGridView dataGridViewCommandes;
        Dictionary<string, int> platsDict = new Dictionary<string, int>();
        Dictionary<string, int> cuisiniersDict = new Dictionary<string, int>();

        public GestionCommandes(string connexion)
        {
            chaineConnexion = connexion;
            InitialiserComposants();
            ChargerCommandes();
        }

        private void InitialiserComposants()
        {
            this.Text = "Gestion des Commandes";
            this.Size = new Size(1750, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            dataGridViewCommandes = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 250
            };
            this.Controls.Add(dataGridViewCommandes);

            Button btnModifierStatut = new Button
            {
                Text = "Modifier Statut Commande",
                Location = new Point(50, 400),
                Size = new Size(200, 30),
            };
            btnModifierStatut.Click += BtnModifierStatut_Click;
            AppliquerCoinsArrondis(btnModifierStatut, 15);
            btnModifierStatut.BackColor = Color.LightBlue;
            btnModifierStatut.FlatStyle = FlatStyle.Flat;
            btnModifierStatut.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnModifierStatut);

            Button btnNouvelleCommande = new Button
            {
                Text = "Nouvelle Commande",
                Location = new Point(300, 400),
                Size = new Size(200, 30),
            };
            btnNouvelleCommande.Click += BtnNouvelleCommande_Click;
            AppliquerCoinsArrondis(btnNouvelleCommande, 15);
            btnModifierStatut.BackColor = Color.LightBlue;
            btnModifierStatut.FlatStyle = FlatStyle.Flat;
            btnNouvelleCommande.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnNouvelleCommande);

            Button btnFermer = new Button
            {
                Text = "Fermer",
                Location = new Point(550, 400),
                Size = new Size(200, 30),
            };
            btnFermer.Click += (s, e) => this.Close();
            AppliquerCoinsArrondis(btnModifierStatut, 15);

            this.Controls.Add(btnFermer);
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
            using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
            {
                connexion.Open();
                string requete = "SELECT * FROM Commande";
                using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                {
                    using (MySqlDataAdapter adaptateur = new MySqlDataAdapter(commande))
                    {
                        DataTable tableCommandes = new DataTable();
                        adaptateur.Fill(tableCommandes);
                        dataGridViewCommandes.DataSource = tableCommandes;
                    }
                }
            }
        }
        private void BtnModifierStatut_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommandes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner une commande à modifier.",
                                "Aucune sélection",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            int idCommande = Convert.ToInt32(dataGridViewCommandes.SelectedRows[0].Cells["IdCommande"].Value);

            using (Form formStatut = new Form())
            {
                formStatut.Text = "Modifier le statut de la commande";
                formStatut.Size = new Size(300, 200);
                formStatut.StartPosition = FormStartPosition.CenterScreen;
                ComboBox comboStatuts = new ComboBox
                {
                    Location = new Point(50, 50),
                    Width = 200
                };
                comboStatuts.Items.AddRange(new string[]
                {
                    "En préparation",
                    "Prêt à livrer",
                    "En cours de livraison",
                    "Livré",
                    "Annulé"
                });

                Label lblStatut = new Label
                {
                    Text = "Sélectionnez le nouveau statut :",
                    Location = new Point(50, 20)
                };

                Button btnConfirmer = new Button
                {
                    Text = "Confirmer",
                    Location = new Point(50, 100),
                    DialogResult = DialogResult.OK
                };

                Button btnAnnuler = new Button
                {
                    Text = "Annuler",
                    Location = new Point(170, 100),
                    DialogResult = DialogResult.Cancel
                };

                formStatut.Controls.Add(lblStatut);
                formStatut.Controls.Add(comboStatuts);
                formStatut.Controls.Add(btnConfirmer);
                formStatut.Controls.Add(btnAnnuler);
                formStatut.AcceptButton = btnConfirmer;
                formStatut.CancelButton = btnAnnuler;
                if (formStatut.ShowDialog() == DialogResult.OK)
                {
                    if (comboStatuts.SelectedItem == null)
                    {
                        MessageBox.Show("Veuillez sélectionner un statut.",
                                        "Statut non sélectionné",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }

                    string nouveauStatut = comboStatuts.SelectedItem.ToString();

                    try
                    {
                        using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                        {
                            connexion.Open();
                            string requete = @"ALTER TABLE Commande ADD COLUMN Statut VARCHAR(50);
                                       UPDATE Commande 
                                       SET Statut = @Statut 
                                       WHERE IdCommande = @IdCommande";

                            using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                            {
                                commande.Parameters.AddWithValue("@Statut", nouveauStatut);
                                commande.Parameters.AddWithValue("@IdCommande", idCommande);

                                int lignesModifiees = commande.ExecuteNonQuery();

                                if (lignesModifiees > 0)
                                {
                                    MessageBox.Show($"Le statut de la commande {idCommande} a été mis à jour à : {nouveauStatut}",
                                                    "Mise à jour réussie",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information);
                                    ChargerCommandes();
                                }
                                else
                                {
                                    MessageBox.Show("Impossible de mettre à jour le statut de la commande.",
                                                    "Erreur",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la mise à jour : {ex.Message}",
                                        "Erreur de connexion",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnNouvelleCommande_Click(object sender, EventArgs e)
        {
            using (Form formNouvelleCommande = new Form())
            {
                formNouvelleCommande.Text = "Nouvelle Commande";
                formNouvelleCommande.Size = new Size(500, 600);
                formNouvelleCommande.StartPosition = FormStartPosition.CenterScreen;

                Label[] labels = new Label[]
                {
                    new Label { Text = "Nom du Plat:", Location = new Point(20, 20), Width = 150 },
                    new Label { Text = "Prix:", Location = new Point(20, 60), Width = 150 },
                    new Label { Text = "Moyen de paiement:", Location = new Point(20, 100), Width = 150 },
                    new Label { Text = "Quantité:", Location = new Point(20, 140), Width = 150 },
                    new Label { Text = "Type de Plat:", Location = new Point(20, 180), Width = 150 },
                    new Label { Text = "Date de Fabrication:", Location = new Point(20, 220), Width = 150 },
                    new Label { Text = "Date de Péremption:", Location = new Point(20, 260), Width = 150 },
                    new Label { Text = "Régime:", Location = new Point(20, 300), Width = 150 },
                    new Label { Text = "Nature:", Location = new Point(20, 340), Width = 150 },
                    new Label { Text = "Ingrédient 1:", Location = new Point(20, 380), Width = 150 },
                    new Label { Text = "Volume 1:", Location = new Point(20, 420), Width = 150 },
                    new Label { Text = "Cuisinier:", Location = new Point(20, 460), Width = 150 }
                };
                ComboBox cmbNomPlat = new ComboBox
                {
                    Location = new Point(200, 20),
                    Width = 250,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                TextBox txtPrix = new TextBox { Location = new Point(200, 60), Width = 250 };

                ComboBox cmbMoyenPaiement = new ComboBox
                {
                    Location = new Point(200, 100),
                    Width = 250,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Items = { "Carte", "Espèces" }
                };

                TextBox txtQuantite = new TextBox { Location = new Point(200, 140), Width = 250 };

                ComboBox cmbTypePlat = new ComboBox
                {
                    Location = new Point(200, 180),
                    Width = 250,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Items = { "Entrée", "Plat", "Dessert", "Boisson" }
                };

                DateTimePicker dtpDateFab = new DateTimePicker
                {
                    Location = new Point(200, 220),
                    Width = 250,
                    Format = DateTimePickerFormat.Short
                };

                DateTimePicker dtpDatePer = new DateTimePicker
                {
                    Location = new Point(200, 260),
                    Width = 250,
                    Format = DateTimePickerFormat.Short
                };

                ComboBox cmbRegime = new ComboBox
                {
                    Location = new Point(200, 300),
                    Width = 250,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Items = { "Standard", "Végétarien", "Végan", "Sans Gluten", "Autre" }
                };

                ComboBox cmbNature = new ComboBox
                {
                    Location = new Point(200, 340),
                    Width = 250,
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Items = { "Française", "Italienne", "Indifférent", "Asiatique", "Autre" }
                };

                TextBox txtIngredient1 = new TextBox { Location = new Point(200, 380), Width = 250 };
                TextBox txtVolume1 = new TextBox { Location = new Point(200, 420), Width = 250 };

                ComboBox cmbCuisinier = new ComboBox
                {
                    Location = new Point(200, 460),
                    Width = 250,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();

                        string requetePlats = "SELECT IdPlat, NomPlat FROM Plat";
                        using (MySqlCommand commande = new MySqlCommand(requetePlats, connexion))
                        {
                            using (MySqlDataReader lecteur = commande.ExecuteReader())
                            {
                                while (lecteur.Read())
                                {
                                    int id = lecteur.GetInt32("IdPlat");
                                    string nom = lecteur.GetString("NomPlat");
                                    platsDict[nom] = id;
                                    cmbNomPlat.Items.Add(nom);
                                }
                            }
                        }
                        cmbNomPlat.DisplayMember = "Nom";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur de chargement des plats : {ex.Message}",
                                   "Erreur",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                }

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();
                        string requete = "SELECT IdCuisinier, CONCAT(Nom, ' ', Prenom) AS NomComplet FROM Cuisinier";
                        using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                        {
                            using (MySqlDataReader lecteur = commande.ExecuteReader())
                            {
                                while (lecteur.Read())
                                {
                                    int id = lecteur.GetInt32("IdCuisinier");
                                    string nom = lecteur.GetString("NomComplet");
                                    cuisiniersDict[nom] = id;
                                    cmbCuisinier.Items.Add(nom);
                                }
                            }
                        }
                        cmbCuisinier.DisplayMember = "Nom";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur de chargement des cuisiniers : {ex.Message}",
                                    "Erreur",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }

                foreach (Label label in labels)
                {
                    formNouvelleCommande.Controls.Add(label);
                }

                formNouvelleCommande.Controls.AddRange(new Control[]
                {
                    cmbNomPlat, txtPrix, cmbMoyenPaiement, txtQuantite, cmbTypePlat,
                    dtpDateFab, dtpDatePer, cmbRegime, cmbNature,
                    txtIngredient1, txtVolume1, cmbCuisinier
                });

                Button btnEnregistrer = new Button
                {
                    Text = "Enregistrer",
                    Location = new Point(200, 500),
                    DialogResult = DialogResult.OK
                };

                Button btnAnnuler = new Button
                {
                    Text = "Annuler",
                    Location = new Point(320, 500),
                    DialogResult = DialogResult.Cancel
                };

                formNouvelleCommande.Controls.Add(btnEnregistrer);
                formNouvelleCommande.Controls.Add(btnAnnuler);
                formNouvelleCommande.AcceptButton = btnEnregistrer;
                formNouvelleCommande.CancelButton = btnAnnuler;

                if (formNouvelleCommande.ShowDialog() == DialogResult.OK)
                {
                   if (cmbNomPlat.SelectedItem == null ||
                        txtPrix.Text == "" || txtPrix.Text == null ||
                        txtQuantite.Text == "" || txtQuantite.Text == null ||
                        cmbCuisinier.SelectedItem == null)
                    {
                        MessageBox.Show("Veuillez remplir tous les champs obligatoires.",
                                        "Champs manquants",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                        {
                            connexion.Open();
                            string requete = @"INSERT INTO Commande 
                            (DateCommande, MoyenPaiement, IdCuisinier, Nom,Prix, Quantite, TypePlat, 
                            DateFab, DatePer, Regime, Nature, Ingredient1, Volume1,IdAdresse, IdClient) 
                            VALUES 
                            (@DateCommande, @MoyenPaiement, @IdCuisinier, @Nom,@Prix, @Quantite, @TypePlat, 
                            @DateFab, @DatePer, @Regime, @Nature, @Ingredient1, @Volume1,@IdAdresse, @IdClient)";

                            using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                            {
                                string nomPlat = cmbNomPlat.SelectedItem.ToString();
                                int idPlat = platsDict[nomPlat];

                                string nomCuisinier = cmbCuisinier.SelectedItem.ToString();
                                int idCuisinier = cuisiniersDict[nomCuisinier];

                                commande.Parameters.AddWithValue("@DateCommande", DateTime.Now);
                                string moyenPaiement = "";
                                if (cmbMoyenPaiement.SelectedItem != null)
                                {
                                    moyenPaiement = cmbMoyenPaiement.SelectedItem.ToString();
                                }
                                commande.Parameters.AddWithValue("@MoyenPaiement", moyenPaiement);
                                commande.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
                                commande.Parameters.AddWithValue("@Nom", nomPlat);
                                commande.Parameters.AddWithValue("@Prix", decimal.Parse(txtPrix.Text));
                                commande.Parameters.AddWithValue("@Quantite", int.Parse(txtQuantite.Text));
                                string typePlat = "";
                                if (cmbTypePlat.SelectedItem != null)
                                {
                                    typePlat = cmbTypePlat.SelectedItem.ToString();
                                }
                                commande.Parameters.AddWithValue("@TypePlat", typePlat);
                                commande.Parameters.AddWithValue("@DateFab", dtpDateFab.Value);
                                commande.Parameters.AddWithValue("@DatePer", dtpDatePer.Value);
                                string regime = "";
                                if (cmbRegime.SelectedItem != null)
                                {
                                    regime = cmbRegime.SelectedItem.ToString();
                                }
                                commande.Parameters.AddWithValue("@Regime", regime);

                                string nature = "";
                                if (cmbNature.SelectedItem != null)
                                {
                                    nature = cmbNature.SelectedItem.ToString();
                                }
                                commande.Parameters.AddWithValue("@Nature", nature);
                                commande.Parameters.AddWithValue("@Ingredient1", txtIngredient1.Text);
                                commande.Parameters.AddWithValue("@Volume1", txtVolume1.Text);
                                commande.Parameters.AddWithValue("@IdAdresse", 1);
                                commande.Parameters.AddWithValue("@IdClient", 1);
                                int lignesAjoutees = commande.ExecuteNonQuery();

                                if (lignesAjoutees > 0)
                                {
                                    MessageBox.Show("Nouvelle commande ajoutée avec succès !",
                                                    "Succès",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Information);
                                    ChargerCommandes();
                                }
                                else
                                {
                                    MessageBox.Show("Impossible d'ajouter la nouvelle commande.",
                                                    "Erreur",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'ajout : {ex.Message}",
                                        "Erreur de connexion",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}