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
    public partial class Clients : Form
    {
        private string chaineConnexion;
        private DataGridView dataGridViewClients;
        /// <summary>
        /// Constructeur de la classe Clients. 
        /// Initialise la connexion à la base de données et appelle la méthode de gestion des clients.
        /// </summary>
        /// <param name="connexion">Chaîne de connexion à la base de données.</param>
        public Clients(string connexion)
        {
            chaineConnexion = connexion;
            InitializeComponent();
            GestionClients();
        }

        /// <summary>
        /// Initialise l'interface de gestion des clients. 
        /// Crée et configure les composants nécessaires tels que la DataGridView et les boutons (Ajouter, Modifier, Supprimer).
        /// Charge la liste des clients dans la DataGridView.
        /// </summary>
        private void GestionClients()
        {
            this.Text = "Gestion des Clients";
            this.Size = new Size(1200, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            dataGridViewClients = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 250
            };
            this.Controls.Add(dataGridViewClients);

            Button btnAjouter = new Button
            {
                Text = "Ajouter Client",
                Location = new Point(50, 300),
                Size = new Size(150, 30)
            };
            btnAjouter.Click += BtnAjouter_Click;
            AppliquerCoinsArrondis(btnAjouter, 15);
            btnAjouter.BackColor = Color.LightGreen;
            btnAjouter.ForeColor = Color.Black;
            btnAjouter.FlatStyle = FlatStyle.Flat;
            btnAjouter.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnAjouter);

            Button btnModifier = new Button
            {
                Text = "Modifier Client",
                Location = new Point(220, 300),
                Size = new Size(150, 30)
            };
            btnModifier.Click += BtnModifier_Click;
            AppliquerCoinsArrondis(btnModifier, 15); 
            btnModifier.BackColor = Color.LightBlue;
            btnModifier.ForeColor = Color.Black;
            btnModifier.FlatStyle = FlatStyle.Flat;
            btnModifier.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnModifier);

            Button btnSupprimer = new Button
            {
                Text = "Supprimer Client",
                Location = new Point(390, 300),
                Size = new Size(150, 30),
                BackColor = Color.LightCoral,
                ForeColor = Color.White
            };
            btnSupprimer.Click += BtnSupprimer_Click;
            AppliquerCoinsArrondis(btnSupprimer, 15);
            btnSupprimer.FlatStyle = FlatStyle.Flat;
            btnSupprimer.FlatAppearance.BorderSize = 0;
            btnSupprimer.FlatAppearance.BorderColor = Color.Red;
            this.Controls.Add(btnSupprimer);

            Button btnFermer = new Button
            {
                Text = "Fermer",
                Location = new Point(560, 300),
                Size = new Size(150, 30)
            };
            btnFermer.Click += (s, e) => this.Close();
            AppliquerCoinsArrondis(btnFermer, 15); 
            btnFermer.BackColor = Color.LightGray;
            btnFermer.ForeColor = Color.Black;
            btnFermer.FlatStyle = FlatStyle.Flat;
            btnFermer.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnFermer);
            ChargerClients();
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

        /// <summary>
        /// Charge les informations des clients depuis la base de données.
        /// Exécute une requête pour récupérer les données des clients et les affiche dans un DataGridView.
        /// En cas d'erreur, un message est affiché.
        /// </summary>
        private void ChargerClients()
        {
            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();
                    // Récupérer les clients via la table Particulier et Client
                    string requete = @"
                        SELECT p.IdParticulier,c.IdClient,  p.Nom, p.Prenom,p.Mail,p.Adresse, p.NumTel,p.MetroProche
                        FROM Particulier p
                        LEFT JOIN Client c ON p.IdParticulier = c.IdParticulier
                        WHERE c.IdClient IS NOT NULL
                        AND p.IdParticulier NOT IN (SELECT IdParticulier FROM Admin)";

                    using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                    {
                        using (MySqlDataAdapter adaptateur = new MySqlDataAdapter(commande))
                        {
                            DataTable tableClients = new DataTable();
                            adaptateur.Fill(tableClients);
                            dataGridViewClients.DataSource = tableClients;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Erreur lors du chargement des clients : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Ouvre un formulaire pour ajouter un nouveau client.
        /// Le formulaire permet de saisir les informations du client (Nom, Prénom, Adresse, Téléphone, Mot de passe).
        /// Lorsque le bouton Enregistrer est cliqué, les informations sont insérées dans la base de données.
        /// Une validation est effectuée pour s'assurer que tous les champs sont remplis. En cas de succès, la liste des clients est mise à jour.
        /// En cas d'erreur, une transaction est annulée et un message d'erreur est affiché.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAjouter_Click(object sender, EventArgs e)
        {
            Form formAjoutClient = new Form
            {
                Text = "Ajouter un nouveau client",
                Size = new Size(400, 550),
                StartPosition = FormStartPosition.CenterScreen
            };
            Label[] labels = new Label[]
            {
                new Label { Text = "Nom", Location = new Point(20, 20), Size = new Size(150, 25) },
                new Label { Text = "Prénom", Location = new Point(20, 70), Size = new Size(150, 25) },
                new Label { Text = "Mail", Location = new Point(20, 120), Size = new Size(150, 25) },
                new Label { Text = "Adresse", Location = new Point(20, 150), Size = new Size(150, 25) },
                new Label { Text = "Téléphone", Location = new Point(20, 190), Size = new Size(150, 25) },
                new Label { Text = "Mot de passe", Location = new Point(20, 240), Size = new Size(150, 25) }
            };
            TextBox[] textBoxes = new TextBox[]
            {
                new TextBox { Location = new Point(180, 20), Size = new Size(200, 25) }, 
                new TextBox { Location = new Point(180, 70), Size = new Size(200, 25) },  
                new TextBox { Location = new Point (180, 120), Size = new Size(200, 25) }, 
                new TextBox { Location = new Point(180, 150), Size = new Size(200, 25) }, 
                new TextBox { Location = new Point(180, 190), Size = new Size(200, 25) }, 
                new TextBox { Location = new Point(180, 240), Size = new Size(200, 25), UseSystemPasswordChar = true } 
            };

            foreach (var label in labels) formAjoutClient.Controls.Add(label);
            foreach (var textBox in textBoxes) formAjoutClient.Controls.Add(textBox);

            Button btnEnregistrer = new Button
            {
                Text = "Enregistrer",
                Location = new Point(100, 400),
                Size = new Size(150, 30)
            };

            btnEnregistrer.Click += (s, ev) =>
            {
                foreach (var textBox in textBoxes)
                {
                    if (textBox.Text == "" || textBox.Text == null)
                    {
                            MessageBox.Show("Veuillez remplir tous les champs obligatoires.", 
                          "Champs manquants", 
                          MessageBoxButtons.OK, 
                          MessageBoxIcon.Warning);
                    return;
                    }
                }

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();
                        MySqlTransaction transaction = connexion.BeginTransaction();

                        try
                        {
                            string requeteParticulier = @"
                                INSERT INTO Particulier (Nom, Prenom,Mail, Adresse, NumTel, Mdp) 
                                VALUES (@Nom, @Prenom,@Mail, @Adresse, @NumTel, @Mdp);
                                SELECT LAST_INSERT_ID();";

                            int idParticulier;
                            using (MySqlCommand cmd = new MySqlCommand(requeteParticulier, connexion, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Nom", textBoxes[0].Text);
                                cmd.Parameters.AddWithValue("@Prenom", textBoxes[1].Text);
                                cmd.Parameters.AddWithValue("@Mail", textBoxes[2].Text);
                                cmd.Parameters.AddWithValue("@Adresse", textBoxes[3].Text);
                                cmd.Parameters.AddWithValue("@NumTel", textBoxes[4].Text);
                                cmd.Parameters.AddWithValue("@Mdp", textBoxes[5].Text);
                                idParticulier = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            string requeteClient = @"
                                INSERT INTO Client (IdParticulier) 
                                VALUES (@IdParticulier)";

                            using (MySqlCommand cmd = new MySqlCommand(requeteClient, connexion, transaction))
                            {
                                cmd.Parameters.AddWithValue("@IdParticulier", idParticulier);
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                            MessageBox.Show("Client ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            formAjoutClient.Close();
                            ChargerClients();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Erreur lors de l'ajout du client : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Erreur de connexion à la base de données : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            Button btnAnnuler = new Button
            {
                Text = "Annuler",
                Location = new Point(260, 400),
                Size = new Size(150, 30)
            };
            btnAnnuler.Click += (s, ev) => formAjoutClient.Close();
            formAjoutClient.Controls.Add(btnEnregistrer);
            formAjoutClient.Controls.Add(btnAnnuler);
            formAjoutClient.ShowDialog();
        }
        /// <summary>
        /// Ouvre un formulaire pour modifier les informations d'un client sélectionné.
        /// Le formulaire permet de modifier les informations du client (Nom, Prénom, Adresse, Téléphone, Mot de passe).
        /// Si le client souhaite modifier le mot de passe, une case à cocher est utilisée pour l'activer.
        /// Le formulaire permet de sauvegarder les modifications dans la base de données.
        /// En cas d'erreur, une transaction est annulée et un message d'erreur est affiché.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnModifier_Click(object sender, EventArgs e)
        {
            if (dataGridViewClients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un client à modifier.", "Sélection requise",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int particulierId = Convert.ToInt32(dataGridViewClients.SelectedRows[0].Cells["IdParticulier"].Value);
            Form formModifierClient = new Form
            {
                Text = "Modifier un client",
                Size = new Size(400, 500),
                StartPosition = FormStartPosition.CenterScreen
            };
            Label[] labels = new Label[]
            {
                new Label { Text = "Nom", Location = new Point(20, 20), Size = new Size(150, 25) },
                new Label { Text = "Prénom", Location = new Point(20, 70), Size = new Size(150, 25) },
                new Label { Text = "Mail", Location = new Point(20, 120), Size = new Size(150, 25) },
                new Label { Text = "Adresse", Location = new Point(20, 170), Size = new Size(150, 25) },
                new Label { Text = "Téléphone", Location = new Point(20, 220), Size = new Size(150, 25) },
                new Label { Text = "Nouveau mot de passe", Location = new Point(20, 266), Size = new Size(150, 25) }
            };

            TextBox[] textBoxes = new TextBox[]
            {
                new TextBox { Location = new Point(180, 20), Size = new Size(200, 25) }, 
                new TextBox { Location = new Point(180, 70), Size = new Size(200, 25) }, 
                new TextBox { Location = new Point(180, 120), Size = new Size(200, 25) }, 
                new TextBox { Location = new Point(180, 170), Size = new Size(200, 25) }, 
                new TextBox { Location = new Point(180, 220), Size = new Size(200, 25) }, 
                new TextBox { Location = new Point(180, 260), Size = new Size(200, 25), UseSystemPasswordChar = true } 
            };
            CheckBox chkChangerMdp = new CheckBox
            {
                Text = "Changer le mot de passe",
                Location = new Point(180, 280),
                Size = new Size(200, 25)
            };
            chkChangerMdp.CheckedChanged += (s, ev) => textBoxes[4].Enabled = chkChangerMdp.Checked;
            foreach (var label in labels)
                formModifierClient.Controls.Add(label);
            foreach (var textBox in textBoxes)
                formModifierClient.Controls.Add(textBox);
            formModifierClient.Controls.Add(chkChangerMdp);
            textBoxes[4].Enabled = false;
            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();
                    string requeteSelection = "SELECT * FROM Particulier WHERE IdParticulier = @IdParticulier";

                    using (MySqlCommand commande = new MySqlCommand(requeteSelection, connexion))
                    {
                        commande.Parameters.AddWithValue("@IdParticulier", particulierId);

                        using (MySqlDataReader lecteur = commande.ExecuteReader())
                        {
                            if (lecteur.Read())
                            {
                                textBoxes[0].Text = lecteur["Nom"].ToString();
                                textBoxes[1].Text = lecteur["Prenom"].ToString();
                                textBoxes[2].Text = lecteur["Mail"].ToString();
                                textBoxes[3].Text = lecteur["Adresse"].ToString();
                                textBoxes[4].Text = lecteur["NumTel"].ToString();
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Button btnEnregistrer = new Button
            {
                Text = "Enregistrer les modifications",
                Location = new Point(100, 320),
                Size = new Size(200, 30)
            };
            btnEnregistrer.Click += (s, ev) =>
            {
                bool champVide = false;
                for (int i = 0; i < textBoxes.Length - 1; i++)
                {
                    if (textBoxes[i].Text == "" || textBoxes[i].Text == null)
                    {
                        champVide = true;
                        break;
                    }
                }
                if (!champVide && chkChangerMdp.Checked)
                {
                    champVide = textBoxes[5].Text == "" || textBoxes[5].Text == null;
                }

                if (champVide)
                {
                    MessageBox.Show("Veuillez remplir tous les champs requis.", "Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();
                        string requeteMaj;
                        if (chkChangerMdp.Checked)
                        {
                            requeteMaj = @"
                                UPDATE Particulier 
                                SET Nom = @Nom, 
                                    Prenom = @Prenom, 
                                    Mail = @Mail,
                                    Adresse = @Adresse, 
                                    NumTel = @NumTel, 
                                    Mdp = @Mdp
                                WHERE IdParticulier = @IdParticulier";
                        }
                        else
                        {
                            requeteMaj = @"
                                UPDATE Particulier 
                                SET Nom = @Nom, 
                                    Prenom = @Prenom, 
                                    Mail = @Mail,
                                    Adresse = @Adresse, 
                                    NumTel = @NumTel
                                WHERE IdParticulier = @IdParticulier";
                        }

                        using (MySqlCommand commande = new MySqlCommand(requeteMaj, connexion))
                        {
                            commande.Parameters.AddWithValue("@IdParticulier", particulierId);
                            commande.Parameters.AddWithValue("@Nom", textBoxes[0].Text);
                            commande.Parameters.AddWithValue("@Prenom", textBoxes[1].Text);
                            commande.Parameters.AddWithValue("@Mail", textBoxes[2].Text);
                            commande.Parameters.AddWithValue("@Adresse", textBoxes[3].Text);
                            commande.Parameters.AddWithValue("@NumTel", textBoxes[4].Text);
                            if (chkChangerMdp.Checked)
                            {
                                commande.Parameters.AddWithValue("@Mdp", textBoxes[5].Text);
                            }
                            int resultat = commande.ExecuteNonQuery();

                            if (resultat > 0)
                            {
                                MessageBox.Show("Client modifié avec succès !", "Succès",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                formModifierClient.Close();
                                ChargerClients();
                            }
                            else
                            {
                                MessageBox.Show("Échec de la modification du client.", "Erreur",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur : {ex.Message}", "Erreur",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            Button btnAnnuler = new Button
            {
                Text = "Annuler",
                Location = new Point(100, 370),
                Size = new Size(200, 30)
            };
            btnAnnuler.Click += (s, ev) => formModifierClient.Close();
            formModifierClient.Controls.Add(btnEnregistrer);
            formModifierClient.Controls.Add(btnAnnuler);
            formModifierClient.ShowDialog();
        }
        /// <summary>
        /// Ouvre un processus de suppression pour un client sélectionné dans la grille.
        /// Vérifie que l'utilisateur a sélectionné un client et demande une confirmation avant de supprimer.
        /// Si l'utilisateur confirme, le client et le particulier associés sont supprimés de la base de données via une transaction.
        /// En cas d'erreur, la transaction est annulée et un message d'erreur est affiché.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSupprimer_Click(object sender, EventArgs e)
        {
            if (dataGridViewClients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un client à supprimer.", "Sélection requise",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult confirmation = MessageBox.Show(
                "Êtes-vous sûr de vouloir supprimer ce client ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (confirmation != DialogResult.Yes)
            {
                return;
            }
            int clientId = Convert.ToInt32(dataGridViewClients.SelectedRows[0].Cells["IdClient"].Value);
            int particulierId = Convert.ToInt32(dataGridViewClients.SelectedRows[0].Cells["IdParticulier"].Value);
            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();
                    MySqlTransaction transaction = connexion.BeginTransaction();

                    try
                    {
                        string requeteSuppressionClient = "DELETE FROM Client WHERE IdClient = @IdClient";
                        using (MySqlCommand cmd = new MySqlCommand(requeteSuppressionClient, connexion, transaction))
                        {
                            cmd.Parameters.AddWithValue("@IdClient", clientId);
                            cmd.ExecuteNonQuery();
                        }
                        string requeteSuppressionParticulier = "DELETE FROM Particulier WHERE IdParticulier = @IdParticulier";
                        using (MySqlCommand cmd = new MySqlCommand(requeteSuppressionParticulier, connexion, transaction))
                        {
                            cmd.Parameters.AddWithValue("@IdParticulier", particulierId);
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        MessageBox.Show("Client supprimé avec succès !", "Succès",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ChargerClients();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Erreur lors de la suppression : {ex.Message}", "Erreur",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Erreur de connexion à la base de données : {ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}