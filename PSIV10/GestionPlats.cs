using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;

namespace PSIV10
{
    public partial class GestionPlats : Form
    {
        #region Declaration des variables
        private string chaineConnexion;
        private MySqlConnection connexion;
        private DataGridView dataGridViewEntrees;
        private DataGridView dataGridViewPlatsPrincipaux;
        private DataGridView dataGridViewDesserts;
        private Button btnAjouter;
        private Button btnModifier;
        private Button btnSupprimer;
        private Button btnFermer;
        private TextBox txtNom;
        private TextBox txtQuantite;
        private TextBox txtCategorie;
        #endregion
        /// <summary>
        /// Constructeur de la classe GestionPlats.
        /// Initialise la connexion et charge les plats disponibles.
        /// </summary>
        /// <param name="connexion">Chaîne de connexion à la base de données.</param>
        /// <param name="estCuisinier">Indique si l'utilisateur est un cuisinier.</param>
        public GestionPlats(string connexion)
        {
            //InitializeComponent();
            Initialiser();
            this.chaineConnexion = connexion;
            this.connexion = new MySqlConnection(chaineConnexion);
            ChargerEntrees();
            ChargerPlatsPrincipaux();
            ChargerDesserts();

        }

        /// <summary>
        /// Initialise les composants de l'interface de gestion des plats.
        /// Configure la fenêtre, les contrôles et les événements associés.
        /// </summary>
        private void Initialiser()
        {
            this.Text = "Gestion des Plats";
            this.Size = new Size(1500, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            Label lblTitre = new Label
            {
                Text = "Gestion des Plats :",
                Location = new Point(50, 20),
                Size = new Size(250, 50),
                Font = new Font("Segoe UI", 12, FontStyle.Bold | FontStyle.Underline)
            };
            this.Controls.Add(lblTitre);
            Label lblentree = new Label
            {
                Text = "Entrée :",
                Location = new Point(50, 120),
                Size = new Size(200, 25)
            };
            this.Controls.Add(lblentree);

            dataGridViewEntrees = new DataGridView
            {
                Location = new Point(50, 150),
                Size = new Size(800, 150),
                //SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };
            this.Controls.Add(dataGridViewEntrees);
            Label lblplat = new Label
            {
                Text = "Plat :",
                Location = new Point(50, 320),
                Size = new Size(200, 25)
            };
            this.Controls.Add(lblplat);
            dataGridViewPlatsPrincipaux = new DataGridView
            {
                Location = new Point(50, 350),
                Size = new Size(800, 150),
                // SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            this.Controls.Add(dataGridViewPlatsPrincipaux);
            Label lbldessert = new Label
            {
                Text = "Dessert :",
                Location = new Point(50, 520),
                Size = new Size(200, 25)
            };
            this.Controls.Add(lbldessert);
            dataGridViewDesserts = new DataGridView
            {
                Location = new Point(50, 550),
                Size = new Size(800, 150),
                //SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            this.Controls.Add(dataGridViewDesserts);

            btnAjouter = new Button
            {
                Text = "Ajouter",
                Location = new Point(980, 150),
                Size = new Size(200, 30),
            };
            btnAjouter.Click += BtnAjouter_Click;
            AppliquerCoinsArrondis(btnAjouter, 10);
            btnAjouter.BackColor = Color.LightGreen;
            btnAjouter.ForeColor = Color.White;
            btnAjouter.FlatStyle = FlatStyle.Flat;
            btnAjouter.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnAjouter);
            btnModifier = new Button
            {
                Text = "Modifier",
                Location = new Point(980, 200),
                Size = new Size(200, 30),
            };
            btnModifier.Click += BtnModifier_Click;
            AppliquerCoinsArrondis(btnModifier, 10);
            btnModifier.BackColor = Color.LightBlue;
            btnModifier.ForeColor = Color.White;
            btnModifier.FlatStyle = FlatStyle.Flat;
            btnModifier.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnModifier);
            btnSupprimer = new Button
            {
                Text = "Supprimer",
                Location = new Point(980, 250),
                Size = new Size(200, 30),
                BackColor = Color.LightCoral,
                ForeColor = Color.White
            };
            btnSupprimer.Click += btnSupprimer_Click;
            AppliquerCoinsArrondis(btnSupprimer, 10);
            btnSupprimer.FlatStyle = FlatStyle.Flat;
            btnSupprimer.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnSupprimer);

            btnFermer = new Button
            {
                Text = "Fermer",
                Location = new Point(980, 300),
                Size = new Size(200, 30),
                BackColor = Color.LightGray,
                ForeColor = Color.Black
            };
            btnFermer.Click += (s, e) => this.Close();
            AppliquerCoinsArrondis(btnFermer, 10);
            btnFermer.FlatStyle = FlatStyle.Flat;
            btnFermer.FlatAppearance.BorderSize = 0;
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
    
            private DataGridView GetSelectedDataGridView()
        {
            // Vérifier les entrées
            if (dataGridViewEntrees.SelectedRows.Count > 0)
                return dataGridViewEntrees;

            // Vérifier les plats principaux
            if (dataGridViewPlatsPrincipaux.SelectedRows.Count > 0)
                return dataGridViewPlatsPrincipaux;

            // Vérifier les desserts
            if (dataGridViewDesserts.SelectedRows.Count > 0)
                return dataGridViewDesserts;

            // Aucune sélection trouvée
            return null;
        }
        #region ChargerPlats
        /// <summary>
        /// Charge les plats depuis la base de données et les affiche dans le DataGridView.
        /// Cette méthode établit une connexion, exécute la requête et remplit un DataTable.
        /// </summary>
        private void ChargerEntrees()
        {
            using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
            {
                connexion.Open();
                string requete = "SELECT IdPlat, NomPlat, Quantite, Categorie,Prix FROM Plat WHERE Categorie = 'Entrée'";
                using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                {
                    using (MySqlDataAdapter adaptateur = new MySqlDataAdapter(commande))
                    {
                        DataTable tableEntrees = new DataTable();
                        adaptateur.Fill(tableEntrees);
                        dataGridViewEntrees.DataSource = tableEntrees;
                        dataGridViewEntrees.ClearSelection();
                    }
                }
            }
        }
        private void ChargerPlatsPrincipaux()
        {
            using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
            {
                connexion.Open();
                string requete = "SELECT IdPlat, NomPlat, Quantite, Categorie,Prix FROM Plat WHERE Categorie = 'Plat Principal'";
                using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                {
                    using (MySqlDataAdapter adaptateur = new MySqlDataAdapter(commande))
                    {
                        DataTable tablePlats = new DataTable();
                        adaptateur.Fill(tablePlats);
                        dataGridViewPlatsPrincipaux.DataSource = tablePlats;
                    }
                }
            }
        }
        private void ChargerDesserts()
        {
            using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
            {
                connexion.Open();
                string requete = "SELECT IdPlat, NomPlat, Quantite, Categorie,Prix FROM Plat WHERE Categorie = 'Dessert'";
                using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                {
                    using (MySqlDataAdapter adaptateur = new MySqlDataAdapter(commande))
                    {
                        DataTable tableDesserts = new DataTable();
                        adaptateur.Fill(tableDesserts);
                        dataGridViewDesserts.DataSource = tableDesserts;
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Ouvre un formulaire permettant de saisir un nouveau plat avec ses informations essentielles :
        /// Nom, Quantité et Catégorie. Vérifie les champs obligatoires et la validité de la quantité.
        /// Si les données sont valides, le plat est ajouté à la base de données. Si l'insertion est réussie,
        /// la liste des plats est rechargée dans le DataGridView.
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement, ici un bouton.</param>
        /// <param name="e">Les arguments de l'événement, ici de type EventArgs.</param>
        private void BtnAjouter_Click(object sender, EventArgs e)
        {
            using (Form formNouveauPlat = new Form())
            {
                formNouveauPlat.Text = "Nouveau Plat";
                formNouveauPlat.Size = new Size(400, 300);
                formNouveauPlat.StartPosition = FormStartPosition.CenterScreen;

                Label lblNom = new Label { Text = "Nom du Plat:", Location = new Point(20, 20), Width = 150 };
                TextBox txtNomPlat = new TextBox { Location = new Point(180, 20), Width = 180 };

                Label lblQuantite = new Label { Text = "Quantité:", Location = new Point(20, 60), Width = 150 };
                TextBox txtQuantite = new TextBox { Location = new Point(180, 60), Width = 180 };

                Label lblCategorie = new Label { Text = "Catégorie:", Location = new Point(20, 100), Width = 150 };
                Label lblPrix = new Label { Text = "Prix:", Location = new Point(20, 140), Width = 150 };
                TextBox txtPrix = new TextBox { Location = new Point(180, 140), Width = 180 };
                ComboBox comboCategorie = new ComboBox
                {
                    Location = new Point(180, 100),
                    Width = 180,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                comboCategorie.Items.AddRange(new string[] { "Entrée", "Plat Principal", "Dessert" });

                Button btnEnregistrer = new Button
                {
                    Text = "Enregistrer",
                    Location = new Point(100, 200),
                    Size = new Size(120, 30),
                    DialogResult = DialogResult.OK
                };

                Button btnAnnuler = new Button
                {
                    Text = "Annuler",
                    Location = new Point(240, 200),
                    Size = new Size(120, 30),
                    DialogResult = DialogResult.Cancel
                };

                formNouveauPlat.Controls.AddRange(new Control[]
                {
            lblNom, txtNomPlat,
            lblQuantite, txtQuantite,
            lblCategorie, comboCategorie,
            lblPrix, txtPrix,
            btnEnregistrer, btnAnnuler
                });

                formNouveauPlat.AcceptButton = btnEnregistrer;
                formNouveauPlat.CancelButton = btnAnnuler;

                if (formNouveauPlat.ShowDialog() == DialogResult.OK)
                {
                    if (txtNomPlat.Text == "" || txtNomPlat.Text == null ||
                        txtQuantite.Text == "" || txtQuantite.Text == null ||
                        comboCategorie.SelectedItem == null)
                    {
                        MessageBox.Show("Veuillez remplir tous les champs.", "Champs manquants", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(txtQuantite.Text, out int quantite) || quantite < 0)
                    {
                        MessageBox.Show("Quantité invalide. Entrez un nombre positif.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string categorie = comboCategorie.SelectedItem.ToString();

                    try
                    {
                        using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                        {
                            connexion.Open();
                            string requete = @"INSERT INTO Plat (NomPlat, Quantite, Categorie,Prix) 
                                       VALUES (@Nom, @Quantite, @Categorie,@Prix)";
                            using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                            {
                                commande.Parameters.AddWithValue("@Nom", txtNomPlat.Text);
                                commande.Parameters.AddWithValue("@Quantite", quantite);
                                commande.Parameters.AddWithValue("@Categorie", categorie);
                                commande.Parameters.AddWithValue("@Prix", txtPrix.Text);


                                int result = commande.ExecuteNonQuery();

                                if (result > 0)
                                {
                                    MessageBox.Show("Plat ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    ChargerEntrees();
                                    ChargerPlatsPrincipaux();
                                    ChargerDesserts();
                                }
                                else
                                {
                                    MessageBox.Show("L'ajout a échoué.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de l'ajout : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Ouvre un formulaire permettant de modifier un plat sélectionné dans le DataGridView.
        /// Récupère les données du plat sélectionné et pré-remplit les champs du formulaire.
        /// Après modification, valide les informations saisies et met à jour le plat dans la base de données.
        /// Si la mise à jour est réussie, recharge la liste des plats.
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement, ici un bouton.</param>
        /// <param name="e">Les arguments de l'événement, ici de type EventArgs.</param>
        private void BtnModifier_Click(object sender, EventArgs e)
        {
            // Vérifier si une ligne est sélectionnée dans le DataGridView
            DataGridView dgv = GetSelectedDataGridView();
            if (dgv == null || dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un plat à modifier.");
                return;
            }
            int idPlat = Convert.ToInt32(dgv.SelectedRows[0].Cells["IdPlat"].Value);
            string nomPlat = "";
            string quantite = "";
            string categorie = "";
            string prix = "";

            if (dgv.SelectedRows[0].Cells["NomPlat"].Value != null)
            {
                nomPlat = dgv.SelectedRows[0].Cells["NomPlat"].Value.ToString();
            }

            if (dgv.SelectedRows[0].Cells["Quantite"].Value != null)
            {
                quantite = dgv.SelectedRows[0].Cells["Quantite"].Value.ToString();
            }

            if (dgv.SelectedRows[0].Cells["Categorie"].Value != null)
            {
                categorie = dgv.SelectedRows[0].Cells["Categorie"].Value.ToString();
            }

            if (dgv.SelectedRows[0].Cells["Prix"].Value != null)
            {
                prix = dgv.SelectedRows[0].Cells["Prix"].Value.ToString();
            }



            // Créer un formulaire simplifié pour la modification du plat
            using (Form formModifierPlat = new Form())
            {
                formModifierPlat.Text = "Modifier un Plat";
                formModifierPlat.Size = new Size(400, 400);
                formModifierPlat.StartPosition = FormStartPosition.CenterScreen;

                // Étiquettes et champs de saisie (uniquement les essentiels)
                Label[] labels = new Label[]
                {
            new Label { Text = "Nom du Plat:", Location = new Point(20, 20), Width = 150 },
            new Label { Text = "Quantité:", Location = new Point(20, 60), Width = 150 },
            new Label { Text = "Catégorie:", Location = new Point(20, 100), Width = 150 },
            new Label { Text = "Prix:", Location = new Point(20, 140), Width = 150 }
                };

                TextBox txtNomPlat = new TextBox
                {
                    Location = new Point(180, 20),
                    Width = 180,
                    Text = nomPlat
                };

                TextBox txtQuantite = new TextBox
                {
                    Location = new Point(180, 60),
                    Width = 180,
                    Text = quantite
                };

                ComboBox comboCategorie = new ComboBox
                {
                    Location = new Point(180, 100),
                    Width = 180,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };
                comboCategorie.Items.AddRange(new string[] { "Entrée", "Plat Principal", "Dessert" });

                TextBox txtPrix = new TextBox
                {
                    Location = new Point(180, 140),
                    Width = 180,
                    Text = prix
                };

                foreach (Label label in labels)
                {
                    formModifierPlat.Controls.Add(label);
                }

                formModifierPlat.Controls.AddRange(new Control[]
                {
            txtNomPlat, txtQuantite, comboCategorie,txtPrix
                });

                Button btnEnregistrer = new Button
                {
                    Text = "Enregistrer",
                    Location = new Point(100, 240),
                    Size = new Size(120, 30),
                    DialogResult = DialogResult.OK
                };

                Button btnAnnuler = new Button
                {
                    Text = "Annuler",
                    Location = new Point(240, 240),
                    Size = new Size(120, 30),
                    DialogResult = DialogResult.Cancel
                };

                formModifierPlat.Controls.Add(btnEnregistrer);
                formModifierPlat.Controls.Add(btnAnnuler);
                formModifierPlat.AcceptButton = btnEnregistrer;
                formModifierPlat.CancelButton = btnAnnuler;

                if (formModifierPlat.ShowDialog() == DialogResult.OK)
                {
                    if (txtNomPlat.Text == "" || txtNomPlat.Text == null ||
                        txtQuantite.Text == "" || txtQuantite.Text == null ||
                        comboCategorie.Text == "" || comboCategorie.Text == null ||
                        txtPrix.Text == "" || txtPrix.Text == null)
                    {
                        MessageBox.Show("Veuillez remplir tous les champs obligatoires (Nom, Quantité, Catégorie).",
                                        "Champs manquants",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }

                    if (!int.TryParse(txtQuantite.Text, out int quantiteVal) || quantiteVal < 0)
                    {
                        MessageBox.Show("Veuillez entrer une quantité valide (nombre entier positif).",
                                        "Valeur incorrecte",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                        {
                            connexion.Open();

                            string requete = @"UPDATE Plat 
                                      SET NomPlat = @Nom, Quantite = @Quantite, Categorie = @Categorie, Prix = @Prix 
                                      WHERE IdPlat = @IdPlat";

                            using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                            {
                                commande.Parameters.AddWithValue("@IdPlat", idPlat);
                                commande.Parameters.AddWithValue("@Nom", txtNomPlat.Text);
                                commande.Parameters.AddWithValue("@Quantite", quantiteVal);
                                commande.Parameters.AddWithValue("@Categorie", comboCategorie.Text);
                                commande.Parameters.AddWithValue("@Prix", txtPrix.Text);

                                int lignesModifiees = commande.ExecuteNonQuery();

                                if (lignesModifiees > 0)
                                {
                                    MessageBox.Show("Plat modifié avec succès !",
                                                   "Succès",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Information);

                                    if (categorie == "Entrée")
                                        ChargerEntrees();
                                    else if (categorie == "Plat Principal")
                                        ChargerPlatsPrincipaux();
                                    else if (categorie == "Dessert")
                                        ChargerDesserts();

                                }
                                else
                                {
                                    MessageBox.Show("Impossible de modifier le plat.",
                                                   "Erreur",
                                                   MessageBoxButtons.OK,
                                                   MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de la modification : {ex.Message}",
                                       "Erreur de connexion",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Error);
                    }
                }
            }
        }
        /// <summary>
        /// Supprime le plat sélectionné dans le DataGridView de la base de données.
        /// Si aucune ligne n'est sélectionnée ou si l'utilisateur est un cuisinier (pas autorisé à supprimer), la suppression n'a pas lieu.
        /// La liste des plats est rechargée après la suppression.
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement, ici un bouton.</param>
        /// <param name="e">Les arguments de l'événement, ici de type EventArgs.</param>
        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            DataGridView dgv = GetSelectedDataGridView();
            if (dgv == null || dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un plat à supprimer.");
                return;
            }

            if (MessageBox.Show("Voulez-vous vraiment supprimer ce plat ?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            int idPlat = Convert.ToInt32(dgv.SelectedRows[0].Cells["IdPlat"].Value);
            string categorie;
            if (dgv.SelectedRows[0].Cells["Categorie"].Value != null)
            {
                categorie = dgv.SelectedRows[0].Cells["Categorie"].Value.ToString();
            }
            else
            {
                categorie = "";
            }

            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();
                    string requete = "DELETE FROM Plat WHERE IdPlat = @IdPlat";
                    using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                    {
                        commande.Parameters.AddWithValue("@IdPlat", idPlat);
                        int lignesSupprimees = commande.ExecuteNonQuery();

                        if (lignesSupprimees > 0)
                        {
                            MessageBox.Show("Plat supprimé avec succès !", "Succès",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ChargerEntrees();
                            ChargerPlatsPrincipaux();
                            ChargerDesserts();
                        }
                        else
                        {
                            MessageBox.Show("Impossible de supprimer le plat.", "Erreur",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
