using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PSIV10
{
    public partial class Admin : Form
    {
        #region Déclaration des variables
        private string chaineConnexion;
        private DataGridView dataGridViewUtilisateurs;
        private DataGridView dataGridViewCommandesClients;
        private DataGridView dataGridViewCommandesCuisiniers;
        private Button btnSupprimerUtilisateur;
        #endregion

        public Admin(string connexion)
        {
            chaineConnexion = connexion;
            InitialiserComposants();
            ChargerDonnees();
        }
        /// <summary>
        /// Initialise les composants de l'interface utilisateur pour l'écran d'administration.
        /// Cette méthode configure les labels, les DataGridViews pour afficher les utilisateurs, 
        /// les commandes des clients et des cuisiniers, ainsi que le bouton de suppression pour les utilisateurs.
        /// </summary>
        private void InitialiserComposants()
        {
            this.Text = "Admin";
            this.Size = new Size(1500, 1000);
            this.StartPosition = FormStartPosition.CenterScreen;
            Label lblTitre = new Label
            {
                Text = "Gestionnaire Admin",
                Location = new Point(50, 20),
                Size = new Size(250, 50),
                Font = new Font("Segoe UI", 12, FontStyle.Bold | FontStyle.Underline)

            };
            this.Controls.Add(lblTitre);

            Label lblUtilisateurs = new Label
            {
                Text = "Liste des utilisateurs",
                Location = new Point(50, 50),
                Size = new Size(200, 25)
            };
            this.Controls.Add(lblUtilisateurs);

            dataGridViewUtilisateurs = new DataGridView
            {
                Location = new Point(50, 80),
                Size = new Size(1380, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            };
            this.Controls.Add(dataGridViewUtilisateurs);

            btnSupprimerUtilisateur = new Button
            {
                Text = "Supprimer Utilisateur",
                Location = new Point(50, 340),
                Size = new Size(200, 50),
                BackColor = Color.LightCoral,
                ForeColor = Color.White
            };
            btnSupprimerUtilisateur.Click += BtnSupprimerUtilisateur_Click;
            this.Controls.Add(btnSupprimerUtilisateur);

            Button btnModifierUtilisateur = new Button
            {
                Text = "Modifier Utilisateur",
                Location = new Point(270, 340),
                Size = new Size(200, 50),
                BackColor = Color.LightBlue,
                ForeColor = Color.White
            };
            btnModifierUtilisateur.Click += BtnModifierUtilisateur_Click;
            this.Controls.Add(btnModifierUtilisateur);

            Label lblCommandesClients = new Label
            {
                Text = "Commandes par Clients",
                Location = new Point(50, 430),
                Size = new Size(300, 25)
            };
            this.Controls.Add(lblCommandesClients);

            dataGridViewCommandesClients = new DataGridView
            {
                Location = new Point(50, 460),
                Size = new Size(900, 150),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            };
            this.Controls.Add(dataGridViewCommandesClients);

            Label lblCommandesCuisiniers = new Label
            {
                Text = "Commandes par Cuisiniers",
                Location = new Point(50, 620),
                Size = new Size(300, 25)
            };
            this.Controls.Add(lblCommandesCuisiniers);

            dataGridViewCommandesCuisiniers = new DataGridView
            {
                Location = new Point(50, 650),
                Size = new Size(900, 150),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            };
            this.Controls.Add(dataGridViewCommandesCuisiniers);
        }
        /// <summary>
        /// Charge les données des utilisateurs et des commandes à partir de la base de données MySQL.
        /// Les données des utilisateurs sont extraites de la table "Particulier" et affichées dans un DataGridView,
        /// tandis que les données des commandes sont extraites de la table "Commande" et affichées dans deux DataGridViews
        /// (une pour les commandes clients et une pour les commandes cuisiniers).
        /// </summary>
        private void ChargerDonnees()
        {
            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();

                    MySqlDataAdapter adaptUtilisateurs = new MySqlDataAdapter("SELECT IdParticulier, Nom,Prenom, Adresse,Mail, NumTel,MetroProche,Mdp FROM Particulier", connexion);
                    DataTable tableUtilisateurs = new DataTable();
                    adaptUtilisateurs.Fill(tableUtilisateurs);
                    dataGridViewUtilisateurs.DataSource = tableUtilisateurs;

                    MySqlDataAdapter adaptCommandes = new MySqlDataAdapter("SELECT IdCommande, IdCuisinier, Nom, Prix, Quantite, TypePlat, DateFab, DatePer, Regime, Nature, Ingredient1, Volume1, Ingredient2, Volume2, Ingredient3, Volume3, Ingredient4, Volume4 FROM Commande", connexion);
                    DataTable tableCommandes = new DataTable();
                    adaptCommandes.Fill(tableCommandes);

                    dataGridViewCommandesClients.DataSource = tableCommandes;
                    dataGridViewCommandesCuisiniers.DataSource = tableCommandes;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des données : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Gère l'événement de suppression d'un utilisateur sélectionné dans le DataGridView.
        /// Si un utilisateur est sélectionné, il est supprimé de la base de données en utilisant son identifiant (ID),
        /// puis la liste des utilisateurs est rechargée. En cas d'échec de la suppression, un message d'erreur est affiché.
        /// </summary>
        private void BtnSupprimerUtilisateur_Click(object sender, EventArgs e)
        {
            if (dataGridViewUtilisateurs.SelectedRows.Count > 0)
            {
                int idParticulier = Convert.ToInt32(dataGridViewUtilisateurs.SelectedRows[0].Cells["IdParticulier"].Value);

                DialogResult confirmation = MessageBox.Show(
                    "Voulez-vous vraiment supprimer cet utilisateur ?",
                    "Confirmation de suppression",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirmation == DialogResult.Yes)
                {
                    try
                    {
                        using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                        {
                            connexion.Open();

                            string[] requetes = {
                                "DELETE FROM Commande WHERE IdClient IN (SELECT IdClient FROM Client WHERE IdParticulier = @id)",
                                "DELETE FROM Commande WHERE IdCuisinier IN (SELECT IdCuisinier FROM Cuisinier WHERE IdParticulier = @id)",
                                "DELETE FROM Client WHERE IdParticulier = @id",
                                "DELETE FROM Cuisinier WHERE IdParticulier = @id",
                                "DELETE FROM Admin WHERE IdParticulier = @id",
                                "DELETE FROM Particulier WHERE IdParticulier = @id"
                            };

                            foreach (string requete in requetes)
                            {
                                using (MySqlCommand cmd = new MySqlCommand(requete, connexion))
                                {
                                    cmd.Parameters.AddWithValue("@id", idParticulier);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            MessageBox.Show("Utilisateur supprimé avec succès");
                            ChargerDonnees();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à supprimer");
            }
        }
        private void BtnModifierUtilisateur_Click(object sender, EventArgs e)
        {
            if (dataGridViewUtilisateurs.SelectedRows.Count > 0)
            {
                Form formModification = new Form();
                formModification.Text = "Modifier utilisateur";
                formModification.Size = new Size(400, 500);
                formModification.StartPosition = FormStartPosition.CenterParent;

                DataGridViewRow row = dataGridViewUtilisateurs.SelectedRows[0];
                int idUtilisateur = Convert.ToInt32(row.Cells["IdParticulier"].Value);

                Label lblNom = new Label { Text = "Nom:", Location = new Point(20, 20) };
                TextBox txtNom = new TextBox { Text = row.Cells["Nom"].Value.ToString(), Location = new Point(120, 20), Width = 200 };

                Label lblPrenom = new Label { Text = "Prénom:", Location = new Point(20, 60) };
                TextBox txtPrenom = new TextBox { Text = row.Cells["Prenom"].Value.ToString(), Location = new Point(120, 60), Width = 200 };

                Label lblAdresse = new Label { Text = "Adresse:", Location = new Point(20, 100) };
                TextBox txtAdresse = new TextBox { Text = row.Cells["Adresse"].Value.ToString(), Location = new Point(120, 100), Width = 200 };

                Label lblTel = new Label { Text = "Téléphone:", Location = new Point(20, 140) };
                TextBox txtTel = new TextBox { Text = row.Cells["NumTel"].Value.ToString(), Location = new Point(120, 140), Width = 200 };

                Label lblMail = new Label { Text = "Mail:", Location = new Point(20, 180) };
                TextBox txtMail = new TextBox { Text = row.Cells["Mail"].Value.ToString(), Location = new Point(120, 180), Width = 200 };

                Label lblMetro = new Label { Text = "Métro Proche:", Location = new Point(20, 220) };
                TextBox txtMetro = new TextBox { Text = row.Cells["MetroProche"].Value.ToString(), Location = new Point(120, 220), Width = 200 };

                Label lblMdp = new Label { Text = "Mot de passe:", Location = new Point(20, 260) };
                TextBox txtMdp = new TextBox { Text = row.Cells["Mdp"].Value.ToString(), Location = new Point(120, 260), Width = 200 };

                Button btnValider = new Button
                {
                    Text = "Valider",
                    DialogResult = DialogResult.OK,
                    Location = new Point(120, 320),
                    Size = new Size(100, 30),
                    BackColor = Color.LightGreen,
                };

                formModification.Controls.AddRange(new Control[] { lblNom, txtNom, lblPrenom, txtPrenom,
                lblAdresse, txtAdresse, lblTel, txtTel, lblMail, txtMail, lblMetro, txtMetro, lblMdp, txtMdp, btnValider });

                if (formModification.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                        {
                            connexion.Open();
                            string query = @"UPDATE Particulier 
                           SET Nom = @nom, Prenom = @prenom, Adresse = @adresse, 
                               NumTel = @tel, Mail = @mail, MetroProche = @metro, Mdp = @mdp 
                           WHERE IdParticulier = @id";

                            MySqlCommand cmd = new MySqlCommand(query, connexion);
                            cmd.Parameters.AddWithValue("@id", idUtilisateur);
                            cmd.Parameters.AddWithValue("@nom", txtNom.Text);
                            cmd.Parameters.AddWithValue("@prenom", txtPrenom.Text);
                            cmd.Parameters.AddWithValue("@adresse", txtAdresse.Text);
                            cmd.Parameters.AddWithValue("@tel", txtTel.Text);
                            cmd.Parameters.AddWithValue("@mail", txtMail.Text);
                            cmd.Parameters.AddWithValue("@metro", txtMetro.Text);
                            cmd.Parameters.AddWithValue("@mdp", txtMdp.Text);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Utilisateur modifié avec succès!", "Succès",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ChargerDonnees();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de la modification : " + ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un utilisateur à modifier.",
                    "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
