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
    public partial class GestionCuisiniers : Form
    {
        private string chaineConnexion;
        private DataGridView dataGridViewCuisiniers;
        /// <summary>
        /// Constructeur de la classe GestionCuisiniers qui initialise la connexion à la base de données,
        /// initialise les composants de l'interface utilisateur et charge la liste des cuisiniers.
        /// </summary>
        /// <param name="connexion">Chaîne de connexion à la base de données</param>
        public GestionCuisiniers(string connexion)
        {
            chaineConnexion = connexion;
            InitialiserComposants();
            ChargerCuisiniers();
        }
        /// <summary>
        /// Initialise les composants de l'interface utilisateur pour la gestion des cuisiniers.
        /// Cela inclut la configuration du titre, de la taille de la fenêtre, l'ajout d'une grille de données
        /// pour afficher les cuisiniers, et l'ajout des boutons permettant d'ajouter, modifier et supprimer
        /// des cuisiniers.
        /// </summary>
        private void InitialiserComposants()
        {
            this.Text = "Gestion des Cuisiniers";
            this.Size = new Size(1750, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            dataGridViewCuisiniers = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 250
            };
            this.Controls.Add(dataGridViewCuisiniers);

            Button btnAjouter = new Button
            {
                Text = "Ajouter Cuisinier",
                Location = new Point(50, 300),
                Size = new Size(150, 30)
            };
            btnAjouter.Click += BtnAjouter_Click;
            AppliquerCoinsArrondis(btnAjouter, 10); // Appliquer coins arrondis au bouton
            btnAjouter.BackColor = Color.LightGreen;
            btnAjouter.ForeColor = Color.White;
            btnAjouter.FlatStyle = FlatStyle.Flat;
            btnAjouter.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnAjouter);

            Button btnModifier = new Button
            {
                Text = "Modifier Cuisinier",
                Location = new Point(220, 300),
                Size = new Size(150, 30)
            };
            btnModifier.Click += BtnModifier_Click;
            AppliquerCoinsArrondis(btnModifier, 10); // Appliquer coins arrondis au bouton
            btnModifier.BackColor = Color.LightBlue;
            btnModifier.ForeColor = Color.White;
            btnModifier.FlatStyle = FlatStyle.Flat;
            btnModifier.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnModifier);

            Button btnSupprimer = new Button
            {
                Text = "Supprimer Cuisinier",
                Location = new Point(390, 300),
                Size = new Size(150, 30),
                BackColor = Color.LightCoral,
                ForeColor = Color.White
            };
            btnSupprimer.Click += BtnSupprimer_Click;
            AppliquerCoinsArrondis(btnSupprimer, 10); // Appliquer coins arrondis au bouton
            btnSupprimer.FlatStyle = FlatStyle.Flat;
            btnSupprimer.FlatAppearance.BorderSize = 0;
            btnSupprimer.FlatAppearance.BorderColor = Color.LightCoral;
            btnSupprimer.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btnSupprimer);

            Button btnFermer = new Button
            {
                Text = "Fermer",
                Location = new Point(560, 300),
                Size = new Size(150, 30),
                BackColor = Color.LightGray,
                ForeColor = Color.Black
            };
            btnFermer.Click += (s, e) => this.Close();
            AppliquerCoinsArrondis(btnFermer, 10); // Appliquer coins arrondis au bouton
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
        /// <summary>
        /// Charge les cuisiniers depuis la base de données et les affiche dans la grille.
        /// </summary>
        private void ChargerCuisiniers()
        {
            using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
            {
                connexion.Open();
                string requete = @"
            SELECT c.IdCuisinier, c.Nom, c.Prenom, c.Rue, c.Numero, c.CodePostal, c.Ville, c.Tel, c.Email, c.MetroProche
            FROM Cuisinier c
            WHERE c.IdParticulier NOT IN (SELECT IdParticulier FROM Admin)";


                using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                {
                    using (MySqlDataAdapter adaptateur = new MySqlDataAdapter(commande))
                    {
                        DataTable tableCuisiniers = new DataTable();
                        adaptateur.Fill(tableCuisiniers);
                        dataGridViewCuisiniers.DataSource = tableCuisiniers;
                    }
                }
            }
        }
        /// <summary>
        /// Ouvre un formulaire permettant d'ajouter un nouveau cuisinier dans la base de données.
        /// Le formulaire collecte les informations personnelles et professionnelles du cuisinier 
        /// et les enregistre dans les tables 'Particulier' et 'Cuisinier' de la base de données.
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement.</param>
        /// <param name="e">Les arguments de l'événement.</param>
        private void BtnAjouter_Click(object sender, EventArgs e)
        {
            // Créer un formulaire de saisie pour un nouveau cuisinier
            Form formulaireAjout = new Form
            {
                Text = "Ajouter un Cuisinier",
                Size = new Size(400, 500),
                StartPosition = FormStartPosition.CenterScreen
            };

            // Créer les étiquettes et champs de saisie
            Label[] etiquettes = new Label[]
            {
        new Label { Text = "Nom :", Location = new Point(20, 20), Size = new Size(100, 25) },
        new Label { Text = "Prénom :", Location = new Point(20, 60), Size = new Size(100, 25) },
        new Label { Text = "Rue :", Location = new Point(20, 100), Size = new Size(100, 25) },
        new Label { Text = "Numéro :", Location = new Point(20, 140), Size = new Size(100, 25) },
        new Label { Text = "Code Postal :", Location = new Point(20, 180), Size = new Size(100, 25) },
        new Label { Text = "Ville :", Location = new Point(20, 220), Size = new Size(100, 25) },
        new Label { Text = "Téléphone :", Location = new Point(20, 260), Size = new Size(100, 25) },
        new Label { Text = "Email :", Location = new Point(20, 300), Size = new Size(100, 25) },
        new Label { Text = "Métro Proche :", Location = new Point(20, 340), Size = new Size(100, 25) }
            };

            TextBox[] champsSaisie = new TextBox[]
            {
        new TextBox { Location = new Point(150, 20), Size = new Size(200, 25), Name = "txtNom" },
        new TextBox { Location = new Point(150, 60), Size = new Size(200, 25), Name = "txtPrenom" },
        new TextBox { Location = new Point(150, 100), Size = new Size(200, 25), Name = "txtRue" },
        new TextBox { Location = new Point(150, 140), Size = new Size(200, 25), Name = "txtNumero" },
        new TextBox { Location = new Point(150, 180), Size = new Size(200, 25), Name = "txtCodePostal" },
        new TextBox { Location = new Point(150, 220), Size = new Size(200, 25), Name = "txtVille" },
        new TextBox { Location = new Point(150, 260), Size = new Size(200, 25), Name = "txtTel" },
        new TextBox { Location = new Point(150, 300), Size = new Size(200, 25), Name = "txtEmail" },
        new TextBox { Location = new Point(150, 340), Size = new Size(200, 25), Name = "txtMetroProche" }
            };

            // Ajouter les étiquettes et champs de saisie au formulaire
            for (int i = 0; i < etiquettes.Length; i++)
            {
                formulaireAjout.Controls.Add(etiquettes[i]);
                formulaireAjout.Controls.Add(champsSaisie[i]);
            }

            // Bouton Ajouter
            Button btnConfirmer = new Button
            {
                Text = "Ajouter",
                Location = new Point(150, 400),
                Size = new Size(100, 30)
            };

            // Événement du bouton Ajouter
            btnConfirmer.Click += (s, ev) =>
            {
                if (champsSaisie[0].Text == "" || champsSaisie[0].Text == null || 
                        champsSaisie[1].Text == "" || champsSaisie[1].Text == null)
                    {
                        MessageBox.Show("Le nom et le prénom sont obligatoires.", "Erreur", MessageBoxButtons.OK);
                        return;
                    }

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();

                        // Étape 1 : Vérifier si le particulier existe déjà
                        string requeteParticulier = @"INSERT INTO Particulier (Nom, Prenom, Adresse, NumTel, Mdp) 
                                          VALUES (@Nom, @Prenom, @Adresse, @Tel, 'defaultpassword');";
                        using (MySqlCommand cmdParticulier = new MySqlCommand(requeteParticulier, connexion))
                        {
                            cmdParticulier.Parameters.AddWithValue("@Nom", champsSaisie[0].Text);
                            cmdParticulier.Parameters.AddWithValue("@Prenom", champsSaisie[1].Text);
                            cmdParticulier.Parameters.AddWithValue("@Adresse", champsSaisie[2].Text);
                            cmdParticulier.Parameters.AddWithValue("@Tel", champsSaisie[6].Text);
                            cmdParticulier.ExecuteNonQuery();
                        }

                        // Récupérer l'IdParticulier ajouté
                        long idParticulier;
                        using (MySqlCommand cmdGetId = new MySqlCommand("SELECT LAST_INSERT_ID();", connexion))
                        {
                            idParticulier = Convert.ToInt64(cmdGetId.ExecuteScalar());
                        }

                        // Étape 2 : Insérer le cuisinier avec l'IdParticulier récupéré
                        string requeteCuisinier = @"INSERT INTO Cuisinier (IdParticulier, Nom, Prenom, Rue, Numero, CodePostal, Ville, Tel, Email, MetroProche) 
                                        VALUES (@IdParticulier, @Nom, @Prenom, @Rue, @Numero, @CodePostal, @Ville, @Tel, @Email, @MetroProche);";

                        using (MySqlCommand cmdCuisinier = new MySqlCommand(requeteCuisinier, connexion))
                        {
                            cmdCuisinier.Parameters.AddWithValue("@IdParticulier", idParticulier);
                            cmdCuisinier.Parameters.AddWithValue("@Nom", champsSaisie[0].Text);
                            cmdCuisinier.Parameters.AddWithValue("@Prenom", champsSaisie[1].Text);
                            cmdCuisinier.Parameters.AddWithValue("@Rue", champsSaisie[2].Text);
                            cmdCuisinier.Parameters.AddWithValue("@Numero", champsSaisie[3].Text);
                            cmdCuisinier.Parameters.AddWithValue("@CodePostal", champsSaisie[4].Text);
                            cmdCuisinier.Parameters.AddWithValue("@Ville", champsSaisie[5].Text);
                            cmdCuisinier.Parameters.AddWithValue("@Tel", champsSaisie[6].Text);
                            cmdCuisinier.Parameters.AddWithValue("@Email", champsSaisie[7].Text);
                            cmdCuisinier.Parameters.AddWithValue("@MetroProche", champsSaisie[8].Text);
                            cmdCuisinier.ExecuteNonQuery();
                        }

                        MessageBox.Show("Cuisinier ajouté avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formulaireAjout.Close();
                        ChargerCuisiniers();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ajout du cuisinier : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };



            formulaireAjout.Controls.Add(btnConfirmer);
            formulaireAjout.ShowDialog();
        }
        /// <summary>
        /// Ouvre un formulaire permettant de modifier les informations d'un cuisinier existant dans la base de données.
        /// Le formulaire charge les données actuelles du cuisinier et permet de les mettre à jour après modification.
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement.</param>
        /// <param name="e">Les arguments de l'événement.</param>
        private void BtnModifier_Click(object sender, EventArgs e)
        {
            // Vérifier si une ligne est sélectionnée
            if (dataGridViewCuisiniers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un cuisinier à modifier.", "Sélection requise", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Récupérer l'ID du cuisinier sélectionné
            int idCuisinier = Convert.ToInt32(dataGridViewCuisiniers.SelectedRows[0].Cells["IdCuisinier"].Value);

            // Créer un formulaire de modification
            Form formulaireModification = new Form
            {
                Text = "Modifier un Cuisinier",
                Size = new Size(400, 500),
                StartPosition = FormStartPosition.CenterScreen
            };

            // Créer les étiquettes et champs de saisie
            Label[] etiquettes = new Label[]
            {
        new Label { Text = "Nom :", Location = new Point(20, 20), Size = new Size(100, 25) },
        new Label { Text = "Prénom :", Location = new Point(20, 60), Size = new Size(100, 25) },
        new Label { Text = "Rue :", Location = new Point(20, 100), Size = new Size(100, 25) },
        new Label { Text = "Numéro :", Location = new Point(20, 140), Size = new Size(100, 25) },
        new Label { Text = "Code Postal :", Location = new Point(20, 180), Size = new Size(100, 25) },
        new Label { Text = "Ville :", Location = new Point(20, 220), Size = new Size(100, 25) },
        new Label { Text = "Téléphone :", Location = new Point(20, 260), Size = new Size(100, 25) },
        new Label { Text = "Email :", Location = new Point(20, 300), Size = new Size(100, 25) },
        new Label { Text = "Métro Proche :", Location = new Point(20, 340), Size = new Size(100, 25) }
            };

            TextBox[] champsSaisie = new TextBox[]
            {
        new TextBox { Location = new Point(150, 20), Size = new Size(200, 25), Name = "txtNom" },
        new TextBox { Location = new Point(150, 60), Size = new Size(200, 25), Name = "txtPrenom" },
        new TextBox { Location = new Point(150, 100), Size = new Size(200, 25), Name = "txtRue" },
        new TextBox { Location = new Point(150, 140), Size = new Size(200, 25), Name = "txtNumero" },
        new TextBox { Location = new Point(150, 180), Size = new Size(200, 25), Name = "txtCodePostal" },
        new TextBox { Location = new Point(150, 220), Size = new Size(200, 25), Name = "txtVille" },
        new TextBox { Location = new Point(150, 260), Size = new Size(200, 25), Name = "txtTel" },
        new TextBox { Location = new Point(150, 300), Size = new Size(200, 25), Name = "txtEmail" },
        new TextBox { Location = new Point(150, 340), Size = new Size(200, 25), Name = "txtMetroProche" }
            };

            // Ajouter les étiquettes et champs de saisie au formulaire
            for (int i = 0; i < etiquettes.Length; i++)
            {
                formulaireModification.Controls.Add(etiquettes[i]);
                formulaireModification.Controls.Add(champsSaisie[i]);
            }

            // Charger les données du cuisinier sélectionné
            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();
                    string requeteSelect = "SELECT * FROM Cuisinier WHERE IdCuisinier = @IdCuisinier";

                    using (MySqlCommand commande = new MySqlCommand(requeteSelect, connexion))
                    {
                        commande.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                        using (MySqlDataReader lecteur = commande.ExecuteReader())
                        {
                            if (lecteur.Read())
                            {
                                champsSaisie[0].Text = lecteur["Nom"].ToString();
                                champsSaisie[1].Text = lecteur["Prenom"].ToString();
                                champsSaisie[2].Text = lecteur["Rue"].ToString();
                                champsSaisie[3].Text = lecteur["Numero"].ToString();
                                champsSaisie[4].Text = lecteur["CodePostal"].ToString();
                                champsSaisie[5].Text = lecteur["Ville"].ToString();
                                champsSaisie[6].Text = lecteur["Tel"].ToString();
                                champsSaisie[7].Text = lecteur["Email"].ToString();
                                champsSaisie[8].Text = lecteur["MetroProche"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des données : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Bouton Modifier
            Button btnConfirmer = new Button
            {
                Text = "Modifier",
                Location = new Point(150, 400),
                Size = new Size(100, 30)
            };

            // Événement du bouton Modifier
            btnConfirmer.Click += (s, ev) =>
            {
                // Vérification des champs obligatoires
                if (champsSaisie[0].Text == "" || champsSaisie[0].Text == null || 
                        champsSaisie[1].Text == "" || champsSaisie[1].Text == null)
                    {
                        MessageBox.Show("Le nom et le prénom sont obligatoires.", "Erreur", MessageBoxButtons.OK);
                        return;
                    }

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();

                        // Requête de mise à jour
                        string requeteUpdate = @"UPDATE Cuisinier 
                    SET Nom = @Nom, 
                        Prenom = @Prenom, 
                        Rue = @Rue, 
                        Numero = @Numero, 
                        CodePostal = @CodePostal, 
                        Ville = @Ville, 
                        Tel = @Tel, 
                        Email = @Email, 
                        MetroProche = @MetroProche 
                    WHERE IdCuisinier = @IdCuisinier";

                        using (MySqlCommand commande = new MySqlCommand(requeteUpdate, connexion))
                        {
                            // Ajout des paramètres
                            commande.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
                            commande.Parameters.AddWithValue("@Nom", champsSaisie[0].Text);
                            commande.Parameters.AddWithValue("@Prenom", champsSaisie[1].Text);
                            commande.Parameters.AddWithValue("@Rue", champsSaisie[2].Text);
                            commande.Parameters.AddWithValue("@Numero", champsSaisie[3].Text);
                            commande.Parameters.AddWithValue("@CodePostal", champsSaisie[4].Text);
                            commande.Parameters.AddWithValue("@Ville", champsSaisie[5].Text);
                            commande.Parameters.AddWithValue("@Tel", champsSaisie[6].Text);
                            commande.Parameters.AddWithValue("@Email", champsSaisie[7].Text);
                            commande.Parameters.AddWithValue("@MetroProche", champsSaisie[8].Text);

                            // Exécution de la requête
                            int lignesModifiees = commande.ExecuteNonQuery();

                            if (lignesModifiees > 0)
                            {
                                // Message de succès
                                MessageBox.Show("Cuisinier modifié avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Fermeture du formulaire
                                formulaireModification.Close();

                                // Actualisation de la liste des cuisiniers
                                ChargerCuisiniers();
                            }
                            else
                            {
                                MessageBox.Show("Aucune modification n'a été effectuée.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la modification du cuisinier : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            formulaireModification.Controls.Add(btnConfirmer);
            formulaireModification.ShowDialog();
        }
        /// <summary>
        /// Ouvre une boîte de dialogue de confirmation pour supprimer un cuisinier sélectionné dans la base de données.
        /// Avant la suppression, il vérifie s'il existe des commandes associées au cuisinier et demande une confirmation supplémentaire si nécessaire.
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement.</param>
        /// <param name="e">Les arguments de l'événement.</param>
        private void BtnSupprimer_Click(object sender, EventArgs e)
        {
            // Vérifier si une ligne est sélectionnée
            if (dataGridViewCuisiniers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner un cuisinier à supprimer.", "Sélection requise", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int idCuisinier = Convert.ToInt32(dataGridViewCuisiniers.SelectedRows[0].Cells["IdCuisinier"].Value);
            string nomCuisinier = dataGridViewCuisiniers.SelectedRows[0].Cells["Nom"].Value.ToString();
            string prenomCuisinier = dataGridViewCuisiniers.SelectedRows[0].Cells["Prenom"].Value.ToString();

            DialogResult resultat = MessageBox.Show(
                $"Êtes-vous sûr de vouloir supprimer le cuisinier {prenomCuisinier} {nomCuisinier} ?",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (resultat == DialogResult.No)
            {
                return; 
            }

            try
            {
                int nombreCommandes = 0;
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();

                    string requeteVerification = "SELECT COUNT(*) FROM Commande WHERE IdCuisinier = @IdCuisinier";
                    using (MySqlCommand commandeVerification = new MySqlCommand(requeteVerification, connexion))
                    {
                        commandeVerification.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
                        nombreCommandes = Convert.ToInt32(commandeVerification.ExecuteScalar());
                    }

                    if (nombreCommandes > 0)
                    {
                        DialogResult resultatCommandes = MessageBox.Show(
                            $"Ce cuisinier a {nombreCommandes} commande(s) associée(s). Voulez-vous vraiment le supprimer ?",
                            "Attention",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (resultatCommandes == DialogResult.No)
                        {
                            return; 
                        }
                    }

                    string requeteSuppression = "DELETE FROM Cuisinier WHERE IdCuisinier = @IdCuisinier";
                    using (MySqlCommand commandeSuppression = new MySqlCommand(requeteSuppression, connexion))
                    {
                        commandeSuppression.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                        int lignesSupprimees = commandeSuppression.ExecuteNonQuery();

                        if (lignesSupprimees > 0)
                        {
                            string requeteSupprimerCommandes = "DELETE FROM Commande WHERE IdCuisinier = @IdCuisinier";
                            using (MySqlCommand commandeSupprimerCommandes = new MySqlCommand(requeteSupprimerCommandes, connexion))
                            {
                                commandeSupprimerCommandes.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
                                commandeSupprimerCommandes.ExecuteNonQuery();
                            }
                            MessageBox.Show(
                                $"Le cuisinier {prenomCuisinier} {nomCuisinier} a été supprimé avec succès.",
                                "Suppression réussie",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                            ChargerCuisiniers();
                        }
                        else
                        {
                            MessageBox.Show(
                                "La suppression du cuisinier a échoué.",
                                "Erreur",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(
                    "Erreur de suppression",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Une erreur est survenue : {ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }

}
