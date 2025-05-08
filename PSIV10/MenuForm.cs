using System;
using System.Data;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PSIV10
{
    public partial class MenuForm : Form
    {
        #region Déclaration des contrôles
        public static int IdClientConnecte { get; set; }

        public static int IdParticulierConnecte { get; set; }
        public static int IdCuisinierConnecte { get; set; }
        public static string NomUtilisateurConnecte { get; set; }
        public static string PrenomUtilisateurConnecte { get; set; }
        public static bool estClient { get; set; }
        public static bool estAdmin { get; set; }
        public static bool estCuisinier { get; set; }
        private TextBox txtNomUtilisateur;
        private TextBox txtMotDePasse;
        private Button btnConnexionAdmin;
        private Button btnConnexionClients;
        private Button btnConnexionCuisiniers;
        private Button btnAnnuler;
        private Button btnInscription;
        private Button btnInscriptionAdmin;
        private Button btnInscriptionClients;
        private Button btnInscriptionCuisiniers;
        private Button btnlivraison;
        private Label lblNomUtilisateur;
        private Label lblMotDePasse;
        private Label lblTitre;
        private Label lblconnecter;
        private Label lblInscription;
        private DataGridView dataGridViewClients;
        private Button btnClients;
        private Button btnCuisiniers;
        private Button btnCommandes;
        private Button btnPlats;
        private Button btnAdmin;
        private Button btnGrapheform;
        private Button btnGestionPlats;
        private Button btnDeconnexion;
        private Button btnQuitter;
        private Button btnStats;
        private Button btnAvis;
        private Button btnCCForm;
        private Button btnFideliteClient;
        private Graphe graphe;
        private Button buttonAjouter;
        private PictureBox livinparis;
        private PictureBox logoAdmin;
        private PictureBox logoClient;
        private PictureBox logoCuisinier;
        private PictureBox imagefond;
        private Label lblinscription;

        private DataGridView dataGridViewPanier;
        private List<DataRow> panier = new List<DataRow>();
        private Label labelTotal;
        private decimal totalPrix = 0;


        #endregion
        private string chaineConnexion = "Server=localhost;Port=3306;Database=psi;User ID=root;Password=root;";
        #region Effet fondu pour les menus
        private System.Windows.Forms.Timer fonduTimer;
        private float opacityStep = 0.05f;

        private void InitialiserFondu()
        {
            // Initialiser l'opacité du formulaire à 0
            this.Opacity = 0;

            // Configurer le Timer
            fonduTimer = new System.Windows.Forms.Timer
            {
                Interval = 15
            };
            fonduTimer.Tick += Fondu_Tick;
            fonduTimer.Start();
        }

        private void Fondu_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += opacityStep;
            }
            else
            {
                fonduTimer.Stop();
            }
        }
        #endregion
        /// <summary>
        /// Constructeur de la classe MenuForm. 
        /// Il initialise les composants de l'interface utilisateur en appelant la méthode Initialiser().
        /// </summary>
        public MenuForm()
        {
            Initialiser();
            InitialiserFondu();
        }
        /// <summary>
        /// Initialise les composants de l'interface utilisateur pour la fenêtre de connexion.
        /// Configure le titre, les boutons de connexion pour différents types d'utilisateurs (Admin, Client, Cuisinier),
        /// et un bouton d'inscription. Ajoute également un logo à l'écran.
        /// </summary>
        private void Initialiser()
        {
            this.Text = "Liv'in Paris !";
            // this.WindowState = FormWindowState.Maximized;
            // this.TopMost = true;
            this.Size = new Size(1500, 1000);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTitre = new Label
            {
                Text = "Bienvenue !",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(900, 0), 
                Size = new Size(360, 90),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitre);
            lblTitre.SendToBack();
            // Image Liv'in Paris
            livinparis = new PictureBox
            {
                Image = Image.image5,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(0, 0),
                Size = new Size(750, 1000)
            };
            this.Controls.Add(livinparis);
            livinparis.SendToBack();
            // Logo Admin
            logoAdmin = new PictureBox
            {
                Image = Image.admin,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(850, 170),
                Size = new Size(88, 90)
            };
            this.Controls.Add(logoAdmin);
            logoAdmin.SendToBack();
            // Logo Client
            logoClient = new PictureBox
            {
                Image = Image.client,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(850, 250),
                Size = new Size(100, 100)
            };
            this.Controls.Add(logoClient);
            logoClient.SendToBack();
            // Logo cuisinier
            logoCuisinier = new PictureBox
            {
                Image = Image.cuisinier,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(850, 340),
                Size = new Size(100, 100)
            };
            this.Controls.Add(logoCuisinier);
            logoCuisinier.SendToBack();
            //imagefond = new PictureBox
            //{
            //    Image = Image.image10,
            //    SizeMode = PictureBoxSizeMode.StretchImage,
            //    Location = new Point(750, 0),
            //    Size = new Size(600, 300)
            //};
            //this.Controls.Add(imagefond);
            //imagefond.SendToBack();

            // Titre de connexion
            lblconnecter = new Label
            {
                Text = "--- Se connecter ---",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(900, 80),
                Size = new Size(360, 90),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblconnecter);
            lblconnecter.Visible = true;

            // Bouton de connexion pour les admins
            btnConnexionAdmin = new Button
            {
                Text = "Admin",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(950, 190),
                Size = new Size(250, 70),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnConnexionAdmin.Click += new EventHandler(this.btnConnexionAdmin_Click);
            btnConnexionAdmin.FlatStyle = FlatStyle.Flat;
            btnConnexionAdmin.FlatAppearance.BorderSize = 0;
            btnConnexionAdmin.BackColor = Color.LightGray;
            btnConnexionAdmin.ForeColor = Color.Black;
            this.Controls.Add(btnConnexionAdmin);
            // Bouton de connexion pour les clients
            btnConnexionClients = new Button
            {
                Text = "Client",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(950, 270),
                Size = new Size(250, 70),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnConnexionClients.Click += new EventHandler(this.btnConnexionClient_Click);
            btnConnexionClients.FlatStyle = FlatStyle.Flat;
            btnConnexionClients.FlatAppearance.BorderSize = 0;
            btnConnexionClients.BackColor = Color.LightGray;
            btnConnexionClients.ForeColor = Color.Black;
            this.Controls.Add(btnConnexionClients);
            // Bouton de connexion pour les cuisiniers
            btnConnexionCuisiniers = new Button
            {
                Text = "Cuisinier",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(950, 350),
                Size = new Size(250, 70),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnConnexionCuisiniers.Click += new EventHandler(this.btnConnexionCuisinier_Click);
            btnConnexionCuisiniers.FlatStyle = FlatStyle.Flat;
            btnConnexionCuisiniers.FlatAppearance.BorderSize = 0;
            btnConnexionCuisiniers.BackColor = Color.LightGray;
            btnConnexionCuisiniers.ForeColor = Color.Black;
            this.Controls.Add(btnConnexionCuisiniers);

            lblinscription = new Label
            {
                Text = "--- S'inscrire ---",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(900, 450),
                Size = new Size(360, 90),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblinscription);


            // Bouton d'Inscription
            btnInscription = new Button
            {
                Text = "Inscription",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(950, 550),
                Size = new Size(250, 70),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnInscription.Click += new EventHandler(this.btnInscription_Click);
            btnInscription.FlatStyle = FlatStyle.Flat;
            btnInscription.FlatAppearance.BorderSize = 0;
            btnInscription.BackColor = Color.LightGray;
            btnInscription.ForeColor = Color.Black;
            this.Controls.Add(btnInscription);

            btnQuitter = new Button
            {
                Text = "Quitter",
                Location = new Point(1300, 10),
                Size = new Size(150, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightCoral,
                ForeColor = Color.White
            };
            btnQuitter.Click += new EventHandler(this.btnQuitter_Click);
            btnQuitter.FlatStyle = FlatStyle.Flat;
            btnQuitter.FlatAppearance.BorderSize = 0;
            btnQuitter.FlatAppearance.MouseOverBackColor = Color.Red;
            btnQuitter.FlatAppearance.MouseDownBackColor = Color.DarkRed;
            btnQuitter.FlatAppearance.BorderColor = Color.Red;
            this.Controls.Add(btnQuitter);

            AppliquerCoinsArrondis(btnConnexionAdmin, 20);
            AppliquerCoinsArrondis(btnConnexionClients, 20);
            AppliquerCoinsArrondis(btnConnexionCuisiniers, 20);
            AppliquerCoinsArrondis(btnInscription, 20);
            AppliquerCoinsArrondis(btnQuitter, 20);

        }
        
        /// <summary>
        /// Applique des coins arrondis à un bouton avec un rayon spécifié.
        /// </summary>
        /// <param name="bouton">Le bouton auquel appliquer les coins arrondis</param>
        /// <param name="rayon">Le rayon de courbure des coins en pixels</param>
        /// <remarks>
        /// Cette méthode crée un chemin graphique (GraphicsPath) qui définit la forme du bouton 
        /// avec des coins arrondis. Elle utilise quatre arcs de cercle pour créer les coins 
        /// et applique cette forme au bouton via sa propriété Region.
        /// </remarks>
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

        #region Page d'accueil
        /// <summary>
        /// Initialise les composants de l'interface pour l'écran de gestion de l'admin.
        /// Crée et configure des boutons pour la gestion des clients, des cuisiniers, des commandes, des plats,
        /// l'accès à l'administration, l'affichage d'un graphique métro, et la déconnexion.
        /// Tous les boutons sont initialement invisibles.
        ///
        private void MenuAdmin()
        {
            InitialiserFondu();
            livinparis.Visible = false;
            logoAdmin.Visible = false;
            logoClient.Visible = false;
            logoCuisinier.Visible = false;
            lblinscription.Visible = false;
            lblconnecter.Visible = false;

            PictureBox logoPictureBox = new PictureBox
            {
                Image = Image.logo,
                SizeMode = PictureBoxSizeMode.Normal,
                Location = new Point(470, 150),
                Size = new Size(200, 200)
            };
            this.Controls.Add(logoPictureBox);

            btnClients = CreerBouton("Gestion des Clients", 50, 50);
            btnClients.Visible = false;
            btnClients.Click += (sender, e) => OuvrirGestionClients();
            btnClients.FlatStyle = FlatStyle.Flat;
            btnClients.FlatAppearance.BorderSize = 0;
            btnClients.BackColor = Color.LightGray;
            btnClients.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnClients, 20);

            btnCuisiniers = CreerBouton("Gestion des Cuisiniers", 50, 100);
            btnCuisiniers.Visible = false;
            btnCuisiniers.Click += (sender, e) => OuvrirGestionCuisiniers();
            btnCuisiniers.FlatStyle = FlatStyle.Flat;
            btnCuisiniers.FlatAppearance.BorderSize = 0;
            btnCuisiniers.BackColor = Color.LightGray;
            btnCuisiniers.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnCuisiniers, 20);

            btnCommandes = CreerBouton("Gestion des Commandes", 50, 150);
            btnCommandes.Visible = false;
            btnCommandes.Click += (sender, e) => OuvrirGestionCommandes();
            btnCommandes.FlatStyle = FlatStyle.Flat;
            btnCommandes.FlatAppearance.BorderSize = 0;
            btnCommandes.BackColor = Color.LightGray;
            btnCommandes.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnCommandes, 20);

            btnPlats = CreerBouton("Gestion des Plats", 50, 200);
            btnPlats.Visible = false;
            btnPlats.Click += (sender, e) => OuvrirGestionPlats();
            btnPlats.FlatStyle = FlatStyle.Flat;
            btnPlats.FlatAppearance.BorderSize = 0;
            btnPlats.BackColor = Color.LightGray;
            btnPlats.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnPlats, 20);

            btnAvis = CreerBouton("Gestion des Avis", 50, 250);
            btnAvis.Visible = false;
            btnAvis.Click += (sender, e) => OuvrirGestionAvis();
            btnAvis.FlatStyle = FlatStyle.Flat;
            btnAvis.FlatAppearance.BorderSize = 0;
            btnAvis.BackColor = Color.LightGray;
            btnAvis.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnAvis, 20);

            btnStats = CreerBouton("Statistiques", 50, 300);
            btnStats.Visible = false;
            btnStats.Click += (sender, e) => OuvrirStatistiques();
            btnStats.FlatStyle = FlatStyle.Flat;
            btnStats.FlatAppearance.BorderSize = 0;
            btnStats.BackColor = Color.LightGray;
            btnStats.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnStats, 20);

            btnAdmin = CreerBouton("Admin", 50, 350);
            btnAdmin.Visible = false;
            btnAdmin.Click += (sender, e) => OuvrirAdmin();
            btnAdmin.FlatStyle = FlatStyle.Flat;
            btnAdmin.FlatAppearance.BorderSize = 0;
            btnAdmin.BackColor = Color.LightGray;
            btnAdmin.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnAdmin, 20);

            btnGrapheform = CreerBouton("Graphe métro", 50, 400);
            btnGrapheform.Visible = false;
            btnGrapheform.Click += (sender, e) => OuvrirGrapheform();
            btnGrapheform.FlatStyle = FlatStyle.Flat;
            btnGrapheform.FlatAppearance.BorderSize = 0;
            btnGrapheform.BackColor = Color.LightGray;
            btnGrapheform.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnGrapheform, 20);

            btnCCForm = CreerBouton("Graphe Client/Cuisinier", 50, 450);
            btnCCForm.Visible = false;
            btnCCForm.Click += (sender, e) => OuvrirCCForm();
            btnCCForm.FlatStyle = FlatStyle.Flat;
            btnCCForm.FlatAppearance.BorderSize = 0;
            btnCCForm.BackColor = Color.LightGray;
            btnCCForm.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnCCForm, 20);

            if (btnDeconnexion == null)
            {
                btnDeconnexion = CreerBouton("Déconnexion", 50, 500);
                btnDeconnexion.Click += (sender, e) => Deconnecter();
                btnDeconnexion.FlatStyle = FlatStyle.Flat;
                btnDeconnexion.FlatAppearance.BorderSize = 0;
                btnDeconnexion.BackColor = Color.LightGray;
                btnDeconnexion.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnDeconnexion, 20);
            }
            btnDeconnexion.Visible = true;
            btnClients.Visible = true;
            btnCuisiniers.Visible = true;
            btnCommandes.Visible = true;
            btnPlats.Visible = true;
            btnAvis.Visible = true;
            btnStats.Visible = true;
            btnAdmin.Visible = true;
            btnGrapheform.Visible = true;
            btnCCForm.Visible = true;
        }
        /// <summary>
        /// Initialise l'interface utilisateur pour l'écran de gestion du client.
        /// Masque les composants de connexion et d'inscription, et affiche un message de bienvenue personnalisé.
        /// Crée et configure les boutons pour la gestion des plats, des commandes, l'affichage d'un graphique métro et la déconnexion.
        /// </summary>
        private void MenuClient()
        {
            InitialiserFondu();
            livinparis.Visible = false;
            logoAdmin.Visible = false;
            logoClient.Visible = false;
            logoCuisinier.Visible = false;
            lblinscription.Visible = false;
            lblconnecter.Visible = false;
            PictureBox logoPictureBox = new PictureBox
            {
                Image = Image.logo,
                SizeMode = PictureBoxSizeMode.Normal,
                Location = new Point(470, 150),
                Size = new Size(200, 200)
            };
            this.Controls.Add(logoPictureBox);

            lblTitre.Visible = false;
            lblNomUtilisateur.Visible = false;
            lblMotDePasse.Visible = false;
            btnAnnuler.Visible = false;
            txtNomUtilisateur.Visible = false;
            txtMotDePasse.Visible = false;
            btnConnexionClients.Visible = false;
            btnConnexionCuisiniers.Visible = false;
            btnInscription.Visible = false;
            btnConnexionAdmin.Visible = false;

            Label lblBienvenue = new Label
            {
                Text = $"Bienvenue, {PrenomUtilisateurConnecte} !",
                Location = new Point(50, 20),
                Size = new Size(200, 25)
            };
            this.Controls.Add(lblBienvenue);
            btnGestionPlats = CreerBouton("Mes plats", 50, 50);
            btnGestionPlats.Click += (sender, e) => ChargerPlats();
            btnGestionPlats.Visible = true;
            btnGestionPlats.FlatStyle = FlatStyle.Flat;
            btnGestionPlats.FlatAppearance.BorderSize = 0;
            btnGestionPlats.BackColor = Color.LightGray;
            btnGestionPlats.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnGestionPlats, 20);

            btnCommandes = CreerBouton("Mes Commandes", 50, 100);
            btnCommandes.Visible = true;
            btnCommandes.Click += (sender, e) => ChargerCommandes();
            btnCommandes.FlatStyle = FlatStyle.Flat;
            btnCommandes.FlatAppearance.BorderSize = 0;
            btnCommandes.BackColor = Color.LightGray;
            btnCommandes.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnCommandes, 20);

            btnAvis = CreerBouton("Avis", 50, 150);
            btnAvis.Visible = true;
            btnAvis.Click += (sender, e) => ChargerAvis();
            btnAvis.FlatStyle = FlatStyle.Flat;
            btnAvis.FlatAppearance.BorderSize = 0;
            btnAvis.BackColor = Color.LightGray;
            btnAvis.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnAvis, 20);

            btnFideliteClient = CreerBouton("Fidélité Client", 50, 200);
            btnFideliteClient.Visible = true;
            btnFideliteClient.Click += new EventHandler(FideliteClient);
            btnFideliteClient.FlatStyle = FlatStyle.Flat;
            btnFideliteClient.FlatAppearance.BorderSize = 0;
            btnFideliteClient.BackColor = Color.LightGray;
            btnFideliteClient.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnFideliteClient, 20);

            btnGrapheform = CreerBouton("Graphe métro", 50, 250);
            btnGrapheform.Click += (sender, e) => OuvrirGrapheform();
            btnGrapheform.FlatStyle = FlatStyle.Flat;
            btnGrapheform.FlatAppearance.BorderSize = 0;
            btnGrapheform.BackColor = Color.LightGray;
            btnGrapheform.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnGrapheform, 20);
            btnGrapheform.Visible = true;

            if (btnDeconnexion == null)
            {
                btnDeconnexion = CreerBouton("Déconnexion", 50, 300);
                btnDeconnexion.Click += (sender, e) => Deconnecter();
                btnDeconnexion.FlatStyle = FlatStyle.Flat;
                btnDeconnexion.FlatAppearance.BorderSize = 0;
                btnDeconnexion.BackColor = Color.LightGray;
                btnDeconnexion.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnDeconnexion, 20);
            }
            btnDeconnexion.Visible = true;


            Panel notificationPanel = new Panel
            {
                Size = new Size(300, 400),
                Location = new Point(50, 350),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Visible = true
            };

            Label titleLabel = new Label
            {
                Text = "Mes Notifications",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(280, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };
            notificationPanel.Controls.Add(titleLabel);
            FlowLayoutPanel notificationsContainer = new FlowLayoutPanel
            {
                Location = new Point(10, 40),
                Size = new Size(280, 380),
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };

            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();
                    string requete = @"
                        SELECT Message, MoyenEnvoi, DATE_FORMAT(DateHeure, '%d/%m/%Y %H:%i') as Date, Statut 
                        FROM Notification 
                        ORDER BY DateHeure DESC 
                        LIMIT 10";

                    using (MySqlCommand cmd = new MySqlCommand(requete, connexion))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Panel notifItem = new Panel
                                {
                                    Size = new Size(260, 100),
                                    BackColor = Color.WhiteSmoke,
                                    Margin = new Padding(0, 0, 0, 10)
                                };

                                Label messageLabel = new Label
                                {
                                    Text = reader["Message"].ToString(),
                                    Location = new Point(5, 5),
                                    Size = new Size(250, 40),
                                    Font = new Font("Arial", 9)
                                };

                                Label detailsLabel = new Label
                                {
                                    Text = $"Envoyé par : {reader["MoyenEnvoi"]}\n" +
                                        $"Date : {reader["Date"]}\n" +
                                        $"Statut : {reader["Statut"]}",
                                    Location = new Point(5, 50),
                                    Size = new Size(250, 100),
                                    Font = new Font("Arial", 8),
                                    ForeColor = Color.Gray
                                };

                                notifItem.Controls.Add(messageLabel);
                                notifItem.Controls.Add(detailsLabel);
                                notificationsContainer.Controls.Add(notifItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Label errorLabel = new Label
                {
                    Text = "Erreur lors du chargement des notifications",
                    Location = new Point(10, 10),
                    Size = new Size(280, 40),
                    ForeColor = Color.Red
                };
                notificationsContainer.Controls.Add(errorLabel);
            }

            notificationPanel.Controls.Add(notificationsContainer);
            this.Controls.Add(notificationPanel);

        }
        /// <summary>
        /// Initialise l'interface utilisateur pour l'écran de gestion du cuisinier.
        /// Masque les composants de connexion, affiche un message de bienvenue personnalisé pour le cuisinier, 
        /// et crée les boutons pour la gestion des commandes, des plats, un graphique métro et la déconnexion.
        /// </summary>
        /// <param name="idCuisinier">Identifiant du cuisinier connecté</param>
        private void MenuCuisinier(int idCuisinier)
        {
            InitialiserFondu();
            livinparis.Visible = false;
            logoAdmin.Visible = false;
            logoClient.Visible = false;
            logoCuisinier.Visible = false;
            lblinscription.Visible = false;
            lblconnecter.Visible = false;
            PictureBox logoPictureBox = new PictureBox
            {
                Image = Image.logo,
                SizeMode = PictureBoxSizeMode.Normal,
                Location = new Point(470, 150),
                Size = new Size(200, 200)
            };
            this.Controls.Add(logoPictureBox);
            btnConnexionCuisiniers.Visible = false;
            btnConnexionClients.Visible = false;
            btnConnexionAdmin.Visible = false;

            Label lblBienvenue = new Label
            {
                Text = $"Bienvenue, Chef {PrenomUtilisateurConnecte} !",
                Location = new Point(50, 20),
                Size = new Size(200, 25)
            };

            this.Controls.Add(lblBienvenue);
            btnCommandes = CreerBouton("Gérer mes Commandes", 50, 50);
            btnCommandes.Visible = true;
            btnCommandes.Click += (sender, e) => OuvrirGestionCommandes();
            btnCommandes.FlatStyle = FlatStyle.Flat;
            btnCommandes.FlatAppearance.BorderSize = 0;
            btnCommandes.BackColor = Color.LightGray;
            btnCommandes.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnCommandes, 20);

            btnGestionPlats = CreerBouton("Gérer mes plats", 50, 100);
            btnGestionPlats.Click += (sender, e) => OuvrirGestionPlats();
            btnGestionPlats.FlatStyle = FlatStyle.Flat;
            btnGestionPlats.FlatAppearance.BorderSize = 0;
            btnGestionPlats.BackColor = Color.LightGray;
            btnGestionPlats.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnGestionPlats, 20);
            btnGestionPlats.Visible = true;

            btnlivraison = CreerBouton("Livraison", 50, 150);
            btnlivraison.Visible = true;
            btnlivraison.Click += (sender, e) => OuvrirGestionLivraison();
            btnlivraison.FlatStyle = FlatStyle.Flat;
            btnlivraison.FlatAppearance.BorderSize = 0;
            btnlivraison.BackColor = Color.LightGray;
            btnlivraison.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnlivraison, 20);

            btnGrapheform = CreerBouton("Graphe métro", 50, 200);
            btnGrapheform.Visible = true;
            btnGrapheform.Click += (sender, e) => OuvrirGrapheform();
            btnGrapheform.FlatStyle = FlatStyle.Flat;
            btnGrapheform.FlatAppearance.BorderSize = 0;
            btnGrapheform.BackColor = Color.LightGray;
            btnGrapheform.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnGrapheform, 20);

            if (btnDeconnexion == null)
            {
                btnDeconnexion = CreerBouton("Déconnexion", 50, 250);
                btnDeconnexion.Click += (sender, e) => Deconnecter();
                btnDeconnexion.FlatStyle = FlatStyle.Flat;
                btnDeconnexion.FlatAppearance.BorderSize = 0;
                btnDeconnexion.BackColor = Color.LightGray;
                btnDeconnexion.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnDeconnexion, 20);
            }
            btnDeconnexion.Visible = true;
        }
        /// <summary>
        /// Initialise le graphe et ouvre un formulaire pour afficher le graphique.
        /// </summary>
        private void OuvrirGrapheform()
        {
            graphe = new Graphe();

            GrapheForm formGraphe = new GrapheForm();
            formGraphe.ShowDialog();
        }
        #region Connexion
        /// <summary>
        /// Gère la connexion de l'administrateur. Vérifie les informations de l'utilisateur (nom et mot de passe) et accorde l'accès si les informations sont valides et correspondent à un utilisateur administrateur (root ou admin). 
        /// Affiche l'interface principale de l'administrateur si la connexion est réussie.
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement (bouton de connexion admin).</param>
        /// <param name="e">Événement déclenché lors du clic sur le bouton.</param>
        private void btnConnexionAdmin_Click(object sender, EventArgs e)
        {
            if (lblinscription != null) lblinscription.Visible = false;
            if (logoAdmin != null) logoAdmin.Visible = false;
            if (logoClient != null) logoClient.Visible = false;
            if (logoCuisinier != null) logoCuisinier.Visible = false;
            if (txtNomUtilisateur == null || !txtNomUtilisateur.Visible)
            {
                btnConnexionAdmin.Visible = false;
                btnConnexionClients.Visible = false;
                btnConnexionCuisiniers.Visible = false;
                btnInscription.Visible = false;
                lblNomUtilisateur = new Label
                {
                    Text = "Nom d'utilisateur :",
                    Location = new Point(900, 200),
                    Size = new Size(250, 25)
                };
                this.Controls.Add(lblNomUtilisateur);

                txtNomUtilisateur = new TextBox
                {
                    Location = new Point(900, 230),
                    Size = new Size(250, 25)
                };
                this.Controls.Add(txtNomUtilisateur);

                lblMotDePasse = new Label
                {
                    Text = "Mot de passe :",
                    Location = new Point(900, 280),
                    Size = new Size(250, 25)
                };
                this.Controls.Add(lblMotDePasse);

                txtMotDePasse = new TextBox
                {
                    Location = new Point(900, 300),
                    Size = new Size(250, 25),
                    PasswordChar = '*'
                };
                this.Controls.Add(txtMotDePasse);

                btnConnexionAdmin = new Button
                {
                    Text = "Connexion",
                    Location = new Point(900, 355),
                    Size = new Size(120, 30)
                };
                btnConnexionAdmin.Click += new EventHandler(btnConnexionAdmin_Click);
                btnConnexionAdmin.FlatStyle = FlatStyle.Flat;
                btnConnexionAdmin.FlatAppearance.BorderSize = 0;
                btnConnexionAdmin.BackColor = Color.LightGray;
                btnConnexionAdmin.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnConnexionAdmin, 20);
                this.Controls.Add(btnConnexionAdmin);

                btnAnnuler = new Button
                {
                    Text = "Annuler",
                    Location = new Point(1030, 355),
                    Size = new Size(120, 30)
                };
                btnAnnuler.Click += new EventHandler(this.btnAnnuler_Click);
                btnAnnuler.FlatStyle = FlatStyle.Flat;
                btnAnnuler.FlatAppearance.BorderSize = 0;
                btnAnnuler.BackColor = Color.LightGray;
                btnAnnuler.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnAnnuler, 20);
                this.Controls.Add(btnAnnuler);
            }
            else
            {
                if (txtNomUtilisateur.Text == "" || txtNomUtilisateur.Text == null || 
                    txtMotDePasse.Text == "" || txtMotDePasse.Text == null)
                {
                    MessageBox.Show("Veuillez remplir tous les champs.",
                        "Champs requis",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                string nomUtilisateur = txtNomUtilisateur.Text;
                string motDePasse = txtMotDePasse.Text;

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();
                        string requete = @"
                            SELECT a.IdAdmin, p.Nom, p.Prenom 
                            FROM Admin a
                            JOIN Particulier p ON a.IdParticulier = p.IdParticulier
                            WHERE p.Nom = @NomUtilisateur AND p.Mdp = @MotDePasse";

                        using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                        {
                            commande.Parameters.AddWithValue("@NomUtilisateur", nomUtilisateur);
                            commande.Parameters.AddWithValue("@MotDePasse", motDePasse);

                            using (MySqlDataReader reader = commande.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int idAdmin = reader.GetInt32("IdAdmin");
                                    string nom = reader.GetString("Nom");
                                    string prenom = reader.GetString("Prenom");

                                    IdParticulierConnecte = idAdmin;
                                    NomUtilisateurConnecte = nom;
                                    PrenomUtilisateurConnecte = prenom;
                                    estAdmin = true;
                                    estCuisinier = false;
                                    estClient = false;

                                    MessageBox.Show($"Bienvenue dans l'espace administrateur, {nom} {prenom} !",
                                        "Connexion réussie",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                                    lblTitre.Visible = false;
                                    lblNomUtilisateur.Visible = false;
                                    lblMotDePasse.Visible = false;
                                    txtNomUtilisateur.Visible = false;
                                    txtMotDePasse.Visible = false;
                                    btnConnexionAdmin.Visible = false;
                                    btnAnnuler.Visible = false;
                                    btnInscription.Visible = false;

                                    MenuAdmin();
                                    btnClients.Visible = true;
                                    btnCuisiniers.Visible = true;
                                    btnCommandes.Visible = true;
                                    btnAdmin.Visible = true;
                                    btnPlats.Visible = true;
                                    btnGrapheform.Visible = true;
                                    btnAvis.Visible = true;
                                    btnStats.Visible = true;
                                    btnCCForm.Visible = true;

                                    this.Text = "LivinParis - Espace Administrateur";
                                }
                                else
                                {
                                    MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.",
                                        "Erreur de connexion",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Erreur de connexion à la base de données : {ex.Message}",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
        /// <summary>
        /// Gère la connexion des clients. Vérifie les informations d'authentification (email et mot de passe), 
        /// et accorde l'accès si les informations sont valides. Si l'utilisateur n'est pas trouvé ou 
        /// s'il n'est pas associé à un compte client, un message d'erreur est affiché.
        /// Si la connexion réussit, l'interface client est affichée.
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement (bouton de connexion client).</param>
        /// <param name="e">Événement déclenché lors du clic sur le bouton.</param>
        private void btnConnexionClient_Click(object sender, EventArgs e)
        {
            if (txtNomUtilisateur == null || !txtNomUtilisateur.Visible)
            {
                AfficherEcranConnexion();
                return;
            }

            string email = txtNomUtilisateur.Text;
            string motDePasse = txtMotDePasse.Text;

            if (email == "" || email == null || motDePasse == "" || motDePasse == null)
            {
                MessageBox.Show("Veuillez saisir un email et un mot de passe.",
                    "Erreur de connexion",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();
                    string requeteVerification = @"
                SELECT p.Nom, p.Prenom FROM Particulier p 
                WHERE p.Mail = @Email AND p.Mdp = @MotDePasse";

                    bool utilisateurExiste = false;
                    string nom = string.Empty;
                    string prenom = string.Empty;
                    int idParticulier = -1;
                    using (MySqlCommand commande = new MySqlCommand(requeteVerification, connexion))
                    {
                        commande.Parameters.AddWithValue("@Email", email);
                        commande.Parameters.AddWithValue("@MotDePasse", motDePasse);

                        using (MySqlDataReader reader = commande.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                utilisateurExiste = true;
                                nom = reader.GetString("Nom");
                                prenom = reader.GetString("Prenom");
                            }
                        }
                    }

                    if (!utilisateurExiste)
                    {
                        MessageBox.Show("Email ou mot de passe incorrect.",
                            "Erreur de connexion",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    string requeteIdParticulier = "SELECT IdParticulier FROM Particulier WHERE Mail = @Email";


                    using (MySqlCommand commande = new MySqlCommand(requeteIdParticulier, connexion))
                    {
                        commande.Parameters.AddWithValue("@Email", email);
                        idParticulier = Convert.ToInt32(commande.ExecuteScalar());
                    }

                    string requeteClient = "SELECT IdClient FROM Client WHERE IdParticulier = @IdParticulier";
                    int idClient = -1;

                    using (MySqlCommand commande = new MySqlCommand(requeteClient, connexion))
                    {
                        commande.Parameters.AddWithValue("@IdParticulier", idParticulier);
                        object resultat = commande.ExecuteScalar();

                        if (resultat != null)
                        {
                            idClient = Convert.ToInt32(resultat);
                        }
                    }

                    MessageBox.Show($"Bienvenue, {prenom} {nom} !",
                        "Connexion réussie",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    IdClientConnecte = idClient;
                    IdParticulierConnecte = idParticulier;
                    NomUtilisateurConnecte = nom;
                    PrenomUtilisateurConnecte = prenom;
                    MenuClient();
                    btnAvis.Visible = true;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Erreur de connexion à la base de données : {ex.Message}",
                    "Erreur",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Affiche l'écran de connexion en masquant les boutons principaux (Admin, Client, Cuisinier, Inscription),
        /// et en créant ou rendant visibles les contrôles nécessaires pour la connexion (email, mot de passe, boutons).
        /// Si les contrôles existent déjà, il suffit de les rendre visibles, sinon ils sont créés dynamiquement.
        /// </summary>
        private void AfficherEcranConnexion()
        {
            btnConnexionAdmin.Visible = false;
            btnConnexionClients.Visible = false;
            btnConnexionCuisiniers.Visible = false;
            btnInscription.Visible = false;
            logoAdmin.Visible = false;
            logoClient.Visible = false;
            logoCuisinier.Visible = false;
            lblinscription.Visible = false;
            if (lblNomUtilisateur == null)
            {
                lblNomUtilisateur = new Label
                {
                    Text = "Mail :",
                    Location = new Point(900, 200),
                    Size = new Size(250, 25)
                };
                this.Controls.Add(lblNomUtilisateur);

                txtNomUtilisateur = new TextBox
                {
                    Location = new Point(900, 230),
                    Size = new Size(250, 25)
                };
                this.Controls.Add(txtNomUtilisateur);

                lblMotDePasse = new Label
                {
                    Text = "Mot de passe :",
                    Location = new Point(900, 265),
                    Size = new Size(250, 25)
                };
                this.Controls.Add(lblMotDePasse);

                txtMotDePasse = new TextBox
                {
                    Location = new Point(900, 290),
                    Size = new Size(250, 25),
                    PasswordChar = '*'
                };
                this.Controls.Add(txtMotDePasse);

                btnConnexionClients = new Button
                {
                    Text = "Connexion",
                    Location = new Point(900, 335),
                    Size = new Size(120, 30)
                };
                btnConnexionClients.Click += new EventHandler(btnConnexionClient_Click);
                btnConnexionClients.FlatStyle = FlatStyle.Flat;
                btnConnexionClients.FlatAppearance.BorderSize = 0;
                btnConnexionClients.BackColor = Color.LightGray;
                btnConnexionClients.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnConnexionClients, 20);
                this.Controls.Add(btnConnexionClients);

                btnAnnuler = new Button
                {
                    Text = "Annuler",
                    Location = new Point(1030, 335),
                    Size = new Size(120, 30)
                };
                btnAnnuler.Click += new EventHandler(this.btnAnnuler_Click);
                btnAnnuler.FlatStyle = FlatStyle.Flat;
                btnAnnuler.FlatAppearance.BorderSize = 0;
                btnAnnuler.BackColor = Color.LightGray;
                btnAnnuler.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnAnnuler, 20);
                this.Controls.Add(btnAnnuler);
            }
            else
            {
                lblNomUtilisateur.Visible = true;
                txtNomUtilisateur.Visible = true;
                lblMotDePasse.Visible = true;
                txtMotDePasse.Visible = true;
                btnConnexionClients.Visible = true;
                btnAnnuler.Visible = true;
            }
        }
        /// <summary>
        /// Gère la connexion du cuisinier. Vérifie les informations d'authentification (nom d'utilisateur et mot de passe),
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnexionCuisinier_Click(object sender, EventArgs e)
        {
            logoAdmin.Visible = false;
            logoClient.Visible = false;
            logoCuisinier.Visible = false;
            lblinscription.Visible = false;
            if (txtNomUtilisateur == null || !txtNomUtilisateur.Visible)
            {
                btnConnexionAdmin.Visible = false;
                btnConnexionClients.Visible = false;
                btnConnexionCuisiniers.Visible = false;
                btnInscription.Visible = false;

                lblNomUtilisateur = new Label
                {
                    Text = "Nom d'utilisateur :",
                    Location = new Point(900, 200),
                    Size = new Size(250, 25)
                };
                this.Controls.Add(lblNomUtilisateur);


                txtNomUtilisateur = new TextBox
                {
                    Location = new Point(900, 230),
                    Size = new Size(250, 25)
                };
                this.Controls.Add(txtNomUtilisateur);

                lblMotDePasse = new Label
                {
                    Text = "Mot de passe :",
                    Location = new Point(900, 265),
                    Size = new Size(250, 25)
                };
                this.Controls.Add(lblMotDePasse);

                txtMotDePasse = new TextBox
                {
                    Location = new Point(900, 290),
                    Size = new Size(250, 25),
                    PasswordChar = '*'
                };
                this.Controls.Add(txtMotDePasse);

                btnConnexionAdmin = new Button
                {
                    Text = "Connexion",
                    Location = new Point(900, 335),
                    Size = new Size(120, 30)
                };
                btnConnexionAdmin.Click += new EventHandler(btnConnexionCuisinier_Click);
                btnConnexionAdmin.FlatStyle = FlatStyle.Flat;
                btnConnexionAdmin.FlatAppearance.BorderSize = 0;
                btnConnexionAdmin.BackColor = Color.LightGray;
                btnConnexionAdmin.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnConnexionAdmin, 20);
                this.Controls.Add(btnConnexionAdmin);

                btnAnnuler = new Button
                {
                    Text = "Annuler",
                    Location = new Point(1030, 335),
                    Size = new Size(120, 30)
                };
                btnAnnuler.Click += new EventHandler(this.btnAnnuler_Click);
                btnAnnuler.FlatStyle = FlatStyle.Flat;
                btnAnnuler.FlatAppearance.BorderSize = 0;
                btnAnnuler.BackColor = Color.LightGray;
                btnAnnuler.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnAnnuler, 20);
                this.Controls.Add(btnAnnuler);
                return;
            }

            string nomUtilisateur = txtNomUtilisateur.Text;
            string motDePasse = txtMotDePasse.Text;

            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();

                    string requete = @"
                SELECT c.IdCuisinier, p.Prenom, p.IdParticulier 
                FROM Cuisinier c 
                JOIN Particulier p ON c.IdParticulier = p.IdParticulier 
                WHERE p.Nom = @NomUtilisateur AND p.Mdp = @MotDePasse";

                    using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                    {
                        commande.Parameters.AddWithValue("@NomUtilisateur", nomUtilisateur);
                        commande.Parameters.AddWithValue("@MotDePasse", motDePasse);

                        using (MySqlDataReader lecteur = commande.ExecuteReader())
                        {
                            if (lecteur.Read())
                            {
                                int idCuisinier = lecteur.GetInt32("IdCuisinier");
                                int idParticulier = lecteur.GetInt32("IdParticulier");
                                string prenom = lecteur.GetString("Prenom");

                                IdCuisinierConnecte = idCuisinier;
                                IdParticulierConnecte = idParticulier;
                                NomUtilisateurConnecte = nomUtilisateur;
                                PrenomUtilisateurConnecte = prenom;
                                estCuisinier = true;
                                estAdmin = false;
                                estClient = false;

                                MessageBox.Show($"Bienvenue, Chef {prenom} !", "Connexion réussie",
                                               MessageBoxButtons.OK, MessageBoxIcon.Information);

                                this.Text = "LivinParis - Menu Cuisinier";

                                lblTitre.Visible = false;
                                lblNomUtilisateur.Visible = false;
                                lblMotDePasse.Visible = false;
                                txtNomUtilisateur.Visible = false;
                                txtMotDePasse.Visible = false;
                                btnConnexionCuisiniers.Visible = false;
                                btnAnnuler.Visible = false;
                                btnInscription.Visible = false;

                                MenuCuisinier(idCuisinier);
                            }
                            else
                            {
                                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.",
                                              "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Erreur de connexion à la base de données : {ex.Message}",
                              "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur inattendue s'est produite : {ex.Message}",
                              "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Permet de déconnecter l'utilisateur en réinitialisant les variables de session (ID, nom, prénom),
        /// en masquant les éléments du menu (commandes, gestion des plats, etc.), et en réaffichant l'écran de connexion.
        /// Un message de déconnexion est ensuite affiché.
        /// </summary>
        private void Deconnecter()
        {
            IdCuisinierConnecte = 0;
            IdParticulierConnecte = 0;
            NomUtilisateurConnecte = null;
            PrenomUtilisateurConnecte = null;
            btnInscription.Visible = true;
            this.Hide();
            MessageBox.Show("Vous avez été déconnecté.", "Déconnexion",
                         MessageBoxButtons.OK, MessageBoxIcon.Information);
            MenuForm menuForm = new MenuForm();
            menuForm.ShowDialog();
        }
        #endregion
        #region Inscription
        /// <summary>
        /// Gestionnaire d'événements pour l'inscription d'un utilisateur. Affiche les options d'inscription (Admin, Client, Cuisinier)
        /// et permet de naviguer vers l'écran d'inscription des différents rôles.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInscription_Click(object sender, EventArgs e)
        {
            lblTitre.Visible = false;
            lblconnecter.Visible = false;
            if (btnConnexionClients != null) btnConnexionClients.Visible = false;
            if (btnConnexionAdmin != null) btnConnexionAdmin.Visible = false;
            if (btnConnexionCuisiniers != null) btnConnexionCuisiniers.Visible = false;
            btnInscription.Visible = false;

            lblInscription = new Label
            {
                Text = "--- Inscription ---",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(900, 80),
                Size = new Size(360, 90),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblInscription);

            if (btnInscriptionAdmin == null)
            {
                btnInscriptionAdmin = new Button
                {
                    Text = "Admin",
                    Location = new Point(950, 190),
                    Size = new Size(250, 70),
                    Font = new Font("Seoge UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                btnInscriptionAdmin.Click += new EventHandler(this.btnInscriptionAdmin_Click);
                btnInscriptionAdmin.FlatStyle = FlatStyle.Flat;
                btnInscriptionAdmin.FlatAppearance.BorderSize = 0;
                btnInscriptionAdmin.BackColor = Color.LightGray;
                btnInscriptionAdmin.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnInscriptionAdmin, 20);
                this.Controls.Add(btnInscriptionAdmin);
            }
            else
            {
                btnInscriptionAdmin.Visible = true;
            }

            if (btnInscriptionClients == null)
            {
                btnInscriptionClients = new Button
                {
                    Text = "Client",
                    Location = new Point(950, 270),
                    Size = new Size(250, 70),
                    Font = new Font("Seoge UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                btnInscriptionClients.Click += new EventHandler(this.btnInscriptionClient_Click);
                btnInscriptionClients.FlatStyle = FlatStyle.Flat;
                btnInscriptionClients.FlatAppearance.BorderSize = 0;
                btnInscriptionClients.BackColor = Color.LightGray;
                btnInscriptionClients.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnInscriptionClients, 20);
                this.Controls.Add(btnInscriptionClients);
            }
            else
            {
                btnInscriptionClients.Visible = true;
            }

            if (btnInscriptionCuisiniers == null)
            {
                btnInscriptionCuisiniers = new Button
                {
                    Text = "Cuisinier",
                    Location = new Point(950, 350),
                    Size = new Size(250, 70),
                    Font = new Font("Seoge UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                btnInscriptionCuisiniers.Click += new EventHandler(this.btnInscriptionCuisinier_Click);
                btnInscriptionCuisiniers.FlatStyle = FlatStyle.Flat;
                btnInscriptionCuisiniers.FlatAppearance.BorderSize = 0;
                btnInscriptionCuisiniers.BackColor = Color.LightGray;
                btnInscriptionCuisiniers.ForeColor = Color.Black;
                AppliquerCoinsArrondis(btnInscriptionCuisiniers, 20);
                this.Controls.Add(btnInscriptionCuisiniers);
            }
            else
            {
                btnInscriptionCuisiniers.Visible = true;
            }

            Button btnRetour = new Button
            {
                Text = "Retour",
                Location = new Point(950, 550),
                Size = new Size(250, 40),
                Font = new Font("Seoge UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnRetour.Click += (s, args) =>
            {
                lblInscription.Visible = false;
                btnInscriptionAdmin.Visible = false;
                btnInscriptionClients.Visible = false;
                btnInscriptionCuisiniers.Visible = false;
                btnRetour.Visible = false;

                lblTitre.Visible = true;
                btnInscription.Visible = true;
                lblconnecter.Visible = true;
                if (btnConnexionClients != null) btnConnexionClients.Visible = true;
                if (btnConnexionAdmin != null) btnConnexionAdmin.Visible = true;
                if (btnConnexionCuisiniers != null) btnConnexionCuisiniers.Visible = true;
            };
            this.Controls.Add(btnRetour);
            btnRetour.FlatStyle = FlatStyle.Flat;
            btnRetour.FlatAppearance.BorderSize = 0;
            btnRetour.BackColor = Color.LightGray;
            btnRetour.ForeColor = Color.Black;
            AppliquerCoinsArrondis(btnRetour, 20);
        }
        /// <summary>
        /// Gère l'événement de clic du bouton d'inscription pour un administrateur. 
        /// Affiche un formulaire d'inscription avec des champs pour saisir le nom, prénom, adresse, numéro de téléphone et mot de passe. 
        /// Lors de la soumission, les informations sont vérifiées, et si valides, insérées dans la base de données. 
        /// Si l'inscription réussit, un message de confirmation est affiché, sinon un message d'erreur est montré.
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement.</param>
        /// <param name="e">Les arguments de l'événement.</param>
        private void btnInscriptionAdmin_Click(object sender, EventArgs e)
        {
            Form formInscription = new Form
            {
                Text = "Inscription Administrateur - Liv'in Paris",
                Size = new Size(500, 700),
                StartPosition = FormStartPosition.CenterScreen
            };

            Label lblTitreInscription = new Label
            {
                Text = "Inscription Administrateur",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(50, 20),
                Size = new Size(400, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            formInscription.Controls.Add(lblTitreInscription);

            Label lblMotDePasseAdmin = new Label
            {
                Text = "Mot de passe Admin existant :",
                Location = new Point(50, 80),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(lblMotDePasseAdmin);

            TextBox txtMotDePasseAdmin = new TextBox
            {
                Location = new Point(50, 105),
                Size = new Size(250, 25),
                PasswordChar = '*'
            };
            formInscription.Controls.Add(txtMotDePasseAdmin);


            Label lblNom = new Label
            {
                Text = "Nom :",
                Location = new Point(50, 140),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(lblNom);

            TextBox txtNom = new TextBox
            {
                Location = new Point(50, 165),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(txtNom);

            Label lblPrenom = new Label
            {
                Text = "Prénom :",
                Location = new Point(50, 200),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(lblPrenom);

            TextBox txtPrenom = new TextBox
            {
                Location = new Point(50, 225),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(txtPrenom);

            Label lblMail = new Label
            {
                Text = "Mail :",
                Location = new Point(50, 260),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(lblMail);

            TextBox txtMail = new TextBox
            {
                Location = new Point(50, 285),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(txtMail);

            Label lblAdresse = new Label
            {
                Text = "Adresse :",
                Location = new Point(50, 320),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(lblAdresse);

            TextBox txtAdresse = new TextBox
            {
                Location = new Point(50, 345),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(txtAdresse);

            Label lblNumTel = new Label
            {
                Text = "Numéro de téléphone :",
                Location = new Point(50, 380),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(lblNumTel);

            TextBox txtNumTel = new TextBox
            {
                Location = new Point(50, 405),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(txtNumTel);

            Label lblmetroproche = new Label
            {
                Text = "Métro proche :",
                Location = new Point(50, 440),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(lblmetroproche);

             ComboBox cmbMetroProche = new ComboBox
            {
                Location = new Point(50, 465),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            formInscription.Controls.Add(cmbMetroProche);
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
                        foreach (var item in cmbMetroProche.Items)
                        {
                            if (item.Equals(nomStation))
                            {
                                existeDeja = true;
                                break;
                            }
                        }

                        if (!existeDeja)
                        {
                            cmbMetroProche.Items.Add(nomStation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des stations de métro : {ex.Message}");
            }

            Label lblMotDePasse = new Label
            {
                Text = "Mot de passe :",
                Location = new Point(50, 500),
                Size = new Size(250, 25)
            };
            formInscription.Controls.Add(lblMotDePasse);

            TextBox txtMotDePasse = new TextBox
            {
                Location = new Point(50, 535),
                Size = new Size(250, 25),
                PasswordChar = '*'
            };
            formInscription.Controls.Add(txtMotDePasse);

            Button btnConfirmerInscription = new Button
            {
                Text = "S'inscrire",
                Location = new Point(50, 580),
                Size = new Size(120, 30)
            };
            formInscription.Controls.Add(btnConfirmerInscription);

            Button btnAnnulerInscription = new Button
            {
                Text = "Annuler",
                Location = new Point(180, 580),
                Size = new Size(120, 30)
            };
            formInscription.Controls.Add(btnAnnulerInscription);
            btnAnnulerInscription.Click += (s, ev) => formInscription.Close();
            btnConfirmerInscription.Click += (s, ev) =>
            {
                string motDePasseAdmin = txtMotDePasseAdmin.Text.Trim();
                string nom = txtNom.Text.Trim();
                string prenom = txtPrenom.Text.Trim();
                string mail = txtMail.Text.Trim();
                string adresse = txtAdresse.Text.Trim();
                string numTel = txtNumTel.Text.Trim();
                string metroProche = cmbMetroProche.Text.Trim();
                string motDePasse = txtMotDePasse.Text;
                if (metroProche == "" || metroProche == null)
                {
                    MessageBox.Show("Le champ 'Métro proche' est obligatoire.",
                                    "Erreur",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
                if (txtNom.Text == "" || txtNom.Text == null)
                {
                    MessageBox.Show("Le champ 'Nom' est obligatoire.", 
                                    "Erreur", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Warning);
                    return;
                }
                if (motDePasseAdmin == "" || motDePasseAdmin == null || 
                    nom == "" || nom == null ||
                    prenom == "" || prenom == null ||
                    mail == "" || mail == null ||
                    adresse == "" || adresse == null ||
                    numTel == "" || numTel == null ||
                    motDePasse == "" || motDePasse == null)
                {
                    MessageBox.Show("Veuillez remplir tous les champs.", 
                                "Erreur", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();
                        string requeteVerification = "SELECT COUNT(*) FROM Admin a JOIN Particulier p ON a.IdParticulier = p.IdParticulier WHERE p.Mdp = @MotDePasseAdmin";
                        using (MySqlCommand commandeVerification = new MySqlCommand(requeteVerification, connexion))
                        {
                            commandeVerification.Parameters.AddWithValue("@MotDePasseAdmin", motDePasseAdmin);
                            int adminExiste = Convert.ToInt32(commandeVerification.ExecuteScalar());

                            if (adminExiste == 0)
                            {
                                MessageBox.Show("Mot de passe administrateur incorrect.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        string requeteInsertionParticulier = "INSERT INTO Particulier (Nom, Prenom, Mail, Adresse, NumTel, Mdp,MetroProche) VALUES (@Nom, @Prenom, @Mail, @Adresse, @NumTel, @Mdp,@MetroProche)";
                        int idParticulier;
                        using (MySqlCommand commandeInsertionParticulier = new MySqlCommand(requeteInsertionParticulier, connexion))
                        {
                            commandeInsertionParticulier.Parameters.AddWithValue("@Nom", nom);
                            commandeInsertionParticulier.Parameters.AddWithValue("@Prenom", prenom);
                            commandeInsertionParticulier.Parameters.AddWithValue("@Mail", mail);
                            commandeInsertionParticulier.Parameters.AddWithValue("@Adresse", adresse);
                            commandeInsertionParticulier.Parameters.AddWithValue("@NumTel", numTel);
                            commandeInsertionParticulier.Parameters.AddWithValue("@Mdp", motDePasse);
                            commandeInsertionParticulier.Parameters.AddWithValue("@MetroProche", metroProche);

                            commandeInsertionParticulier.ExecuteNonQuery();
                            idParticulier = (int)commandeInsertionParticulier.LastInsertedId;
                        }
                        string requeteInsertionAdmin = "INSERT INTO Admin (IdParticulier) VALUES (@IdParticulier)";
                        using (MySqlCommand commandeInsertionAdmin = new MySqlCommand(requeteInsertionAdmin, connexion))
                        {
                            commandeInsertionAdmin.Parameters.AddWithValue("@IdParticulier", idParticulier);
                            commandeInsertionAdmin.ExecuteNonQuery();
                        }

                        MessageBox.Show("Inscription réussie !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formInscription.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Erreur de connexion à la base de données : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            formInscription.ShowDialog();
        }
        /// <summary>
        /// Gère l'événement de clic du bouton d'inscription pour un client.
        /// Crée un formulaire d'inscription avec des champs pour saisir les informations nécessaires : 
        /// nom, prénom, adresse, téléphone, email, etc. Lorsque l'utilisateur clique sur le bouton de confirmation, 
        /// les informations sont validées et envoyées à la base de données. Si l'inscription réussit, un message de confirmation est affiché.
        /// Si l'utilisateur annule, le formulaire est fermé.
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement.</param>
        /// <param name="e">Les arguments de l'événement.</param>

        private void btnInscriptionClient_Click(object sender, EventArgs e)
        {
            Form formInscriptionClient = new Form
            {
                Text = "Inscription Client - Liv'in Paris",
                Size = new Size(450, 650),
                StartPosition = FormStartPosition.CenterScreen
            };

            Label lblTitreInscription = new Label
            {
                Text = "Inscription Client - Liv'in Paris",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(50, 20),
                Size = new Size(350, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            formInscriptionClient.Controls.Add(lblTitreInscription);

            Label lblNom = new Label
            {
                Text = "Nom :",
                Location = new Point(50, 80),
                Size = new Size(150, 25)
            };
            formInscriptionClient.Controls.Add(lblNom);

            TextBox txtNom = new TextBox
            {
                Location = new Point(200, 80),
                Size = new Size(200, 25)
            };
            formInscriptionClient.Controls.Add(txtNom);

            Label lblPrenom = new Label
            {
                Text = "Prénom :",
                Location = new Point(50, 115),
                Size = new Size(150, 25)
            };
            formInscriptionClient.Controls.Add(lblPrenom);

            TextBox txtPrenom = new TextBox
            {
                Location = new Point(200, 115),
                Size = new Size(200, 25)
            };
            formInscriptionClient.Controls.Add(txtPrenom);

            Label lblRue = new Label
            {
                Text = "Rue :",
                Location = new Point(50, 150),
                Size = new Size(150, 25)
            };
            formInscriptionClient.Controls.Add(lblRue);

            TextBox txtRue = new TextBox
            {
                Location = new Point(200, 150),
                Size = new Size(200, 25)
            };
            formInscriptionClient.Controls.Add(txtRue);

            Label lblCodePostal = new Label
            {
                Text = "Code Postal :",
                Location = new Point(50, 185),
                Size = new Size(150, 25)
            };
            formInscriptionClient.Controls.Add(lblCodePostal);

            TextBox txtCodePostal = new TextBox
            {
                Location = new Point(200, 185),
                Size = new Size(200, 25)
            };
            formInscriptionClient.Controls.Add(txtCodePostal);

            Label lblVille = new Label
            {
                Text = "Ville :",
                Location = new Point(50, 220),
                Size = new Size(150, 25)
            };
            formInscriptionClient.Controls.Add(lblVille);

            TextBox txtVille = new TextBox
            {
                Location = new Point(200, 220),
                Size = new Size(200, 25)
            };
            formInscriptionClient.Controls.Add(txtVille);

            Label lblTelephone = new Label
            {
                Text = "Téléphone :",
                Location = new Point(50, 255),
                Size = new Size(150, 25)
            };
            formInscriptionClient.Controls.Add(lblTelephone);

            TextBox txtTelephone = new TextBox
            {
                Location = new Point(200, 255),
                Size = new Size(200, 25)
            };
            formInscriptionClient.Controls.Add(txtTelephone);

            Label lblEmail = new Label
            {
                Text = "Mail :",
                Location = new Point(50, 290),
                Size = new Size(150, 25)
            };
            formInscriptionClient.Controls.Add(lblEmail);

            TextBox txtEmail = new TextBox
            {
                Location = new Point(200, 290),
                Size = new Size(200, 25)
            };
            formInscriptionClient.Controls.Add(txtEmail);

            Label lblMetroProche = new Label
            {
                Text = "Métro proche :",
                Location = new Point(50, 325),
                Size = new Size(150, 25)
            };
            formInscriptionClient.Controls.Add(lblMetroProche);

            ComboBox cmbMetroProche = new ComboBox
            {
                Location = new Point(200, 325),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            formInscriptionClient.Controls.Add(cmbMetroProche);
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
                        foreach (var item in cmbMetroProche.Items)
                        {
                            if (item == nomStation)
                            {
                                existeDeja = true;
                                break;
                            }
                        }

                        if (!existeDeja)
                        {
                            cmbMetroProche.Items.Add(nomStation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des stations de métro : {ex.Message}");
            }

            Label lblMotDePasse = new Label
            {
                Text = "Mot de passe :",
                Location = new Point(50, 360),
                Size = new Size(150, 25)
            };
            formInscriptionClient.Controls.Add(lblMotDePasse);

            TextBox txtMotDePasse = new TextBox
            {
                Location = new Point(200, 360),
                Size = new Size(200, 25),
                PasswordChar = '*'
            };
            formInscriptionClient.Controls.Add(txtMotDePasse);

            Label lblConfirmMotDePasse = new Label
            {
                Text = "Confirmer mot de passe :",
                Location = new Point(50, 430),
                Size = new Size(150, 25)
            };
            formInscriptionClient.Controls.Add(lblConfirmMotDePasse);

            TextBox txtConfirmMotDePasse = new TextBox
            {
                Location = new Point(200, 430),
                Size = new Size(200, 25),
                PasswordChar = '*'
            };
            formInscriptionClient.Controls.Add(txtConfirmMotDePasse);

            Button btnConfirmerInscription = new Button
            {
                Text = "S'inscrire",
                Location = new Point(100, 480),
                Size = new Size(120, 30),
                BackColor = Color.LightGreen
            };
            formInscriptionClient.Controls.Add(btnConfirmerInscription);

            Button btnAnnulerInscription = new Button
            {
                Text = "Annuler",
                Location = new Point(230, 480),
                Size = new Size(120, 30)
            };
            formInscriptionClient.Controls.Add(btnAnnulerInscription);

            btnAnnulerInscription.Click += (s, ev) => formInscriptionClient.Close();

            btnConfirmerInscription.Click += (s, ev) =>
            {
                string nom = txtNom.Text.Trim();
                string prenom = txtPrenom.Text.Trim();
                string rue = txtRue.Text.Trim();
                string codePostal = txtCodePostal.Text.Trim();
                string ville = txtVille.Text.Trim();
                string telephone = txtTelephone.Text.Trim();
                string email = txtEmail.Text.Trim();
                string metroProche = cmbMetroProche.Text.Trim();
                string motDePasse = txtMotDePasse.Text;
                string confirmMotDePasse = txtConfirmMotDePasse.Text;

                if (nom == "" || nom == null ||
                    prenom == "" || prenom == null ||
                    rue == "" || rue == null ||
                    codePostal == "" || codePostal == null ||
                    ville == "" || ville == null ||
                    telephone == "" || telephone == null ||
                    motDePasse == "" || motDePasse == null)
                {
                    MessageBox.Show("Veuillez remplir tous les champs obligatoires.",
                        "Erreur d'inscription",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                if (motDePasse != confirmMotDePasse)
                {
                    MessageBox.Show("Les mots de passe ne correspondent pas.",
                        "Erreur d'inscription",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                string adresseComplete = $"{rue}, {codePostal} {ville}";

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();

                        using (MySqlTransaction transaction = connexion.BeginTransaction())
                        {
                            try
                            {
                                string requeteVerification = "SELECT COUNT(*) FROM Particulier WHERE Nom = @Nom";
                                using (MySqlCommand commandeVerification = new MySqlCommand(requeteVerification, connexion, transaction))
                                {
                                    commandeVerification.Parameters.AddWithValue("@Nom", nom);
                                    int nombreUtilisateurs = Convert.ToInt32(commandeVerification.ExecuteScalar());

                                    if (nombreUtilisateurs > 0)
                                    {
                                        MessageBox.Show("Ce nom d'utilisateur existe déjà. Veuillez en choisir un autre.",
                                            "Erreur d'inscription",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                                string requeteInsertionAdresse = @"
                                    INSERT INTO Adresse (Rue, CodePostal, Ville)
                                    VALUES (@Rue, @CodePostal, @Ville);
                                    SELECT LAST_INSERT_ID();";

                                int idAdresse;
                                using (MySqlCommand cmdInsertAdresse = new MySqlCommand(requeteInsertionAdresse, connexion, transaction))
                                {
                                    cmdInsertAdresse.Parameters.AddWithValue("@Rue", rue);
                                    cmdInsertAdresse.Parameters.AddWithValue("@CodePostal", codePostal);
                                    cmdInsertAdresse.Parameters.AddWithValue("@Ville", ville);

                                    idAdresse = Convert.ToInt32(cmdInsertAdresse.ExecuteScalar());
                                }

                                string requeteInsertionParticulier = @"
                                    INSERT INTO Particulier (Nom, Prenom, Mail, Adresse, NumTel, Mdp, MetroProche)
                                    VALUES (@Nom, @Prenom, @Mail, @Adresse, @NumTel, @Mdp, @MetroProche);
                                    SELECT LAST_INSERT_ID();";

                                int idParticulier;
                                using (MySqlCommand cmdInsertParticulier = new MySqlCommand(requeteInsertionParticulier, connexion, transaction))
                                {
                                    cmdInsertParticulier.Parameters.AddWithValue("@Nom", nom);
                                    cmdInsertParticulier.Parameters.AddWithValue("@Prenom", prenom);
                                    cmdInsertParticulier.Parameters.AddWithValue("@Mail", email);
                                    cmdInsertParticulier.Parameters.AddWithValue("@Adresse", adresseComplete);
                                    cmdInsertParticulier.Parameters.AddWithValue("@NumTel", telephone);
                                    cmdInsertParticulier.Parameters.AddWithValue("@Mdp", motDePasse);
                                    cmdInsertParticulier.Parameters.AddWithValue("@MetroProche", metroProche);

                                    idParticulier = Convert.ToInt32(cmdInsertParticulier.ExecuteScalar());
                                }
                                string requeteInsertionClient = @"
                                    INSERT INTO Client (IdParticulier, IdAdresse)
                                    VALUES (@IdParticulier, @IdAdresse);";

                                using (MySqlCommand cmdInsertClient = new MySqlCommand(requeteInsertionClient, connexion, transaction))
                                {
                                    cmdInsertClient.Parameters.AddWithValue("@IdParticulier", idParticulier);
                                    cmdInsertClient.Parameters.AddWithValue("@IdAdresse", idAdresse);
                                    cmdInsertClient.ExecuteNonQuery();
                                }

                                transaction.Commit();

                                MessageBox.Show("Inscription réussie ! Vous pouvez maintenant vous connecter.",
                                    "Inscription",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                                formInscriptionClient.Close();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Erreur de connexion à la base de données : {ex.Message}",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite : {ex.Message}",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };

            formInscriptionClient.ShowDialog();
        }
        /// <summary>
        /// Gère l'événement lorsqu'on clique sur le bouton "Inscription Cuisinier". 
        /// Cela ouvre un nouveau formulaire pour permettre à l'utilisateur de s'inscrire en tant que "Cuisinier". 
        /// Le formulaire collecte des informations personnelles, telles que le nom, l'adresse, le numéro de téléphone, l'email, etc. 
        /// Le formulaire valide les champs saisis et, si tout est correct, les données sont insérées dans les tables "Particulier" et "Cuisinier" de la base de données. 
        /// </summary>
        private void btnInscriptionCuisinier_Click(object sender, EventArgs e)
        {
            Form formInscriptionCuisinier = new Form
            {
                Text = "Inscription Cuisinier - Liv'in Paris",
                Size = new Size(450, 650),
                StartPosition = FormStartPosition.CenterScreen
            };
            Label lblTitreInscription = new Label
            {
                Text = "Inscription Cuisinier - Liv'in Paris",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(50, 20),
                Size = new Size(350, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
            formInscriptionCuisinier.Controls.Add(lblTitreInscription);

            Label lblNom = new Label
            {
                Text = "Nom :",
                Location = new Point(50, 80),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblNom);

            TextBox txtNom = new TextBox
            {
                Location = new Point(200, 80),
                Size = new Size(200, 25)
            };
            formInscriptionCuisinier.Controls.Add(txtNom);

            Label lblPrenom = new Label
            {
                Text = "Prénom :",
                Location = new Point(50, 115),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblPrenom);

            TextBox txtPrenom = new TextBox
            {
                Location = new Point(200, 115),
                Size = new Size(200, 25)
            };
            formInscriptionCuisinier.Controls.Add(txtPrenom);

            Label lblRue = new Label
            {
                Text = "Rue :",
                Location = new Point(50, 150),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblRue);

            TextBox txtRue = new TextBox
            {
                Location = new Point(200, 150),
                Size = new Size(200, 25)
            };
            formInscriptionCuisinier.Controls.Add(txtRue);

           Label lblNumero = new Label
            {
                Text = "Numéro :",
                Location = new Point(50, 185),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblNumero);

            TextBox txtNumero = new TextBox
            {
                Location = new Point(200, 185),
                Size = new Size(200, 25)
            };
            formInscriptionCuisinier.Controls.Add(txtNumero);

            Label lblCodePostal = new Label
            {
                Text = "Code Postal :",
                Location = new Point(50, 220),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblCodePostal);

            TextBox txtCodePostal = new TextBox
            {
                Location = new Point(200, 220),
                Size = new Size(200, 25)
            };
            formInscriptionCuisinier.Controls.Add(txtCodePostal);

            Label lblVille = new Label
            {
                Text = "Ville :",
                Location = new Point(50, 255),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblVille);

            TextBox txtVille = new TextBox
            {
                Location = new Point(200, 255),
                Size = new Size(200, 25)
            };
            formInscriptionCuisinier.Controls.Add(txtVille);

            Label lblTelephone = new Label
            {
                Text = "Téléphone :",
                Location = new Point(50, 290),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblTelephone);

            TextBox txtTelephone = new TextBox
            {
                Location = new Point(200, 290),
                Size = new Size(200, 25)
            };
            formInscriptionCuisinier.Controls.Add(txtTelephone);

            Label lblEmail = new Label
            {
                Text = "Email :",
                Location = new Point(50, 325),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblEmail);

            TextBox txtEmail = new TextBox
            {
                Location = new Point(200, 325),
                Size = new Size(200, 25)
            };
            formInscriptionCuisinier.Controls.Add(txtEmail);

            Label lblMetroProche = new Label
            {
                Text = "Métro proche :",
                Location = new Point(50, 360),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblMetroProche);

            ComboBox cmbMetroProche = new ComboBox
            {
                Location = new Point(200, 360),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            formInscriptionCuisinier.Controls.Add(cmbMetroProche);
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
                        foreach (var item in cmbMetroProche.Items)
                        {
                            if (item == nomStation)
                            {
                                existeDeja = true;
                                break;
                            }
                        }

                        if (!existeDeja)
                        {
                            cmbMetroProche.Items.Add(nomStation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des stations de métro : {ex.Message}");
            }

            Label lblSpecialite = new Label
            {
                Text = "Spécialité culinaire :",
                Location = new Point(50, 395),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblSpecialite);

            TextBox txtSpecialite = new TextBox
            {
                Location = new Point(200, 395),
                Size = new Size(200, 25)
            };
            formInscriptionCuisinier.Controls.Add(txtSpecialite);

            Label lblMotDePasse = new Label
            {
                Text = "Mot de passe :",
                Location = new Point(50, 430),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblMotDePasse);

            TextBox txtMotDePasse = new TextBox
            {
                Location = new Point(200, 430),
                Size = new Size(200, 25),
                PasswordChar = '*'
            };
            formInscriptionCuisinier.Controls.Add(txtMotDePasse);

            Label lblConfirmMotDePasse = new Label
            {
                Text = "Confirmer mot de passe :",
                Location = new Point(50, 465),
                Size = new Size(150, 25)
            };
            formInscriptionCuisinier.Controls.Add(lblConfirmMotDePasse);

            TextBox txtConfirmMotDePasse = new TextBox
            {
                Location = new Point(200, 465),
                Size = new Size(200, 25),
                PasswordChar = '*'
            };
            formInscriptionCuisinier.Controls.Add(txtConfirmMotDePasse);

            Button btnConfirmerInscription = new Button
            {
                Text = "S'inscrire",
                Location = new Point(100, 510),
                Size = new Size(120, 30)
            };
            formInscriptionCuisinier.Controls.Add(btnConfirmerInscription);

            Button btnAnnulerInscription = new Button
            {
                Text = "Annuler",
                Location = new Point(230, 510),
                Size = new Size(120, 30)
            };
            formInscriptionCuisinier.Controls.Add(btnAnnulerInscription);

            btnAnnulerInscription.Click += (s, ev) => formInscriptionCuisinier.Close();

            btnConfirmerInscription.Click += (s, ev) =>
            {
                string nom = txtNom.Text.Trim();
                string prenom = txtPrenom.Text.Trim();
                string rue = txtRue.Text.Trim();
                string numeroStr = txtNumero.Text.Trim();
                string codePostal = txtCodePostal.Text.Trim();
                string ville = txtVille.Text.Trim();
                string telephone = txtTelephone.Text.Trim();
                string email = txtEmail.Text.Trim();
                string metroProche = cmbMetroProche.Text.Trim();
                string specialite = txtSpecialite.Text.Trim();
                string motDePasse = txtMotDePasse.Text;
                string confirmMotDePasse = txtConfirmMotDePasse.Text;

                if (nom == "" || nom == null ||
                    prenom == "" || prenom == null ||
                    rue == "" || rue == null ||
                    numeroStr == "" || numeroStr == null ||
                    codePostal == "" || codePostal == null ||
                    ville == "" || ville == null ||
                    telephone == "" || telephone == null ||
                    specialite == "" || specialite == null ||
                    motDePasse == "" || motDePasse == null)
                {
                    MessageBox.Show("Veuillez remplir tous les champs obligatoires.",
                        "Erreur d'inscription",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Vérification que le mot de passe et sa confirmation correspondent
                if (motDePasse != confirmMotDePasse)
                {
                    MessageBox.Show("Les mots de passe ne correspondent pas.",
                        "Erreur d'inscription",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Conversion du numéro en entier
                if (!int.TryParse(numeroStr, out int numero))
                {
                    MessageBox.Show("Le numéro doit être un nombre entier.",
                        "Erreur d'inscription",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Construction de l'adresse complète pour la table Particulier
                string adresseComplete = $"{numero} {rue}, {codePostal} {ville}";

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();

                        using (MySqlTransaction transaction = connexion.BeginTransaction())
                        {
                            try
                            {
                                string requeteVerification = "SELECT COUNT(*) FROM Particulier WHERE Nom = @Nom";
                                using (MySqlCommand commandeVerification = new MySqlCommand(requeteVerification, connexion, transaction))
                                {
                                    commandeVerification.Parameters.AddWithValue("@Nom", nom);
                                    int nombreUtilisateurs = Convert.ToInt32(commandeVerification.ExecuteScalar());

                                    if (nombreUtilisateurs > 0)
                                    {
                                        MessageBox.Show("Ce nom d'utilisateur existe déjà. Veuillez en choisir un autre.",
                                            "Erreur d'inscription",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                                        return;
                                    }
                                }

                                string requeteInsertionParticulier = @"
                    INSERT INTO Particulier (Nom, Prenom,Mail, Adresse, NumTel, Mdp) 
                    VALUES (@Nom, @Prenom,@Mail, @Adresse, @NumTel, @Mdp);
                    SELECT LAST_INSERT_ID();";

                                int idParticulier;
                                using (MySqlCommand cmdInsertParticulier = new MySqlCommand(requeteInsertionParticulier, connexion, transaction))
                                {
                                    cmdInsertParticulier.Parameters.AddWithValue("@Nom", nom);
                                    cmdInsertParticulier.Parameters.AddWithValue("@Prenom", prenom);
                                    cmdInsertParticulier.Parameters.AddWithValue("@Mail", email);
                                    cmdInsertParticulier.Parameters.AddWithValue("@Adresse", adresseComplete);
                                    cmdInsertParticulier.Parameters.AddWithValue("@NumTel", telephone);
                                    cmdInsertParticulier.Parameters.AddWithValue("@Mdp", motDePasse);
                                    idParticulier = Convert.ToInt32(cmdInsertParticulier.ExecuteScalar());
                                }

                                string requeteInsertionCuisinier = @"
                    INSERT INTO Cuisinier (IdParticulier) 
                    VALUES (@IdParticulier);";

                                using (MySqlCommand cmdInsertCuisinier = new MySqlCommand(requeteInsertionCuisinier, connexion, transaction))
                                {
                                    cmdInsertCuisinier.Parameters.AddWithValue("@IdParticulier", idParticulier);
                                    cmdInsertCuisinier.ExecuteNonQuery();
                                }
                                string requeteVerifierStructure = @"
                    SELECT COUNT(*) 
                    FROM information_schema.COLUMNS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Cuisinier' 
                    AND COLUMN_NAME = 'Nom';";

                                bool cuisinierADesColonnesSupplementaires = false;
                                using (MySqlCommand cmdVerifierStructure = new MySqlCommand(requeteVerifierStructure, connexion, transaction))
                                {
                                    int nombreColonnes = Convert.ToInt32(cmdVerifierStructure.ExecuteScalar());
                                    cuisinierADesColonnesSupplementaires = (nombreColonnes > 0);
                                }

                                if (cuisinierADesColonnesSupplementaires)
                                {
                                    string requeteGetCuisinierID = "SELECT IdCuisinier FROM Cuisinier WHERE IdParticulier = @IdParticulier;";
                                    int idCuisinier;
                                    using (MySqlCommand cmdGetCuisinierID = new MySqlCommand(requeteGetCuisinierID, connexion, transaction))
                                    {
                                        cmdGetCuisinierID.Parameters.AddWithValue("@IdParticulier", idParticulier);
                                        idCuisinier = Convert.ToInt32(cmdGetCuisinierID.ExecuteScalar());
                                    }

                                    string requeteMiseAJourCuisinier = @"
                        UPDATE Cuisinier 
                        SET Nom = @Nom, Prenom = @Prenom, Rue = @Rue, Numero = @Numero, 
                            CodePostal = @CodePostal, Ville = @Ville, Tel = @Tel, 
                            Email = @Email, MetroProche = @MetroProche, Specialite = @Specialite
                        WHERE IdCuisinier = @IdCuisinier;";

                                    using (MySqlCommand cmdMiseAJourCuisinier = new MySqlCommand(requeteMiseAJourCuisinier, connexion, transaction))
                                    {
                                        cmdMiseAJourCuisinier.Parameters.AddWithValue("@Nom", nom);
                                        cmdMiseAJourCuisinier.Parameters.AddWithValue("@Prenom", prenom);
                                        cmdMiseAJourCuisinier.Parameters.AddWithValue("@Rue", rue);
                                        cmdMiseAJourCuisinier.Parameters.AddWithValue("@Numero", numero);
                                        cmdMiseAJourCuisinier.Parameters.AddWithValue("@CodePostal", codePostal);
                                        cmdMiseAJourCuisinier.Parameters.AddWithValue("@Ville", ville);
                                        cmdMiseAJourCuisinier.Parameters.AddWithValue("@Tel", telephone);
                                        cmdMiseAJourCuisinier.Parameters.AddWithValue("@Email", email);
                                        cmdMiseAJourCuisinier.Parameters.AddWithValue("@MetroProche", metroProche);
                                        cmdMiseAJourCuisinier.Parameters.AddWithValue("@Specialite", specialite);
                                        cmdMiseAJourCuisinier.Parameters.AddWithValue("@IdCuisinier", idCuisinier);

                                        cmdMiseAJourCuisinier.ExecuteNonQuery();
                                    }
                                }

                                transaction.Commit();

                                MessageBox.Show("Inscription réussie ! Vous pouvez maintenant vous connecter en tant que cuisinier.",
                                    "Inscription",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                                formInscriptionCuisinier.Close();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw ex; 
                            }
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Erreur de connexion à la base de données : {ex.Message}",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite : {ex.Message}",
                        "Erreur",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            };

            formInscriptionCuisinier.ShowDialog();
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="texte"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Button CreerBouton(string texte, int x, int y)
        {
            Button bouton = new Button
            {
                Text = texte,
                Location = new Point(x, y),
                Size = new Size(300, 40)
            };
            this.Controls.Add(bouton);

            return bouton;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            MenuForm menuForm = new MenuForm();
            menuForm.Show();
            this.Hide();
        }
        #endregion
        #region Page principal
        /// <summary>
        /// Ouvre le formulaire de gestion des clients. 
        /// Ce formulaire permet de visualiser et gérer les informations des clients dans l'application.
        /// Il crée une instance du formulaire `Clients` en passant la chaîne de connexion à la base de données 
        /// et affiche ce formulaire en mode modal (avec `ShowDialog()`).
        /// </summary>
        private void OuvrirGestionClients()
        {
            Clients formCuisiniers = new Clients(chaineConnexion);
            formCuisiniers.ShowDialog();
        }
        /// <summary>
        /// Ouvre le formulaire de gestion des cuisiniers.
        /// Ce formulaire permet de visualiser et gérer les informations des cuisiniers dans l'application.
        /// Il crée une instance du formulaire `GestionCuisiniers` en passant la chaîne de connexion à la base de données 
        /// et affiche ce formulaire en mode modal (avec `ShowDialog()`).
        /// </summary>
        private void OuvrirGestionCuisiniers()
        {
            GestionCuisiniers formCuisiniers = new GestionCuisiniers(chaineConnexion);
            formCuisiniers.ShowDialog();
        }
        /// <summary>
        /// Ouvre le formulaire de gestion des plats.
        /// Ce formulaire permet de visualiser et gérer les plats dans l'application.
        /// Avant d'ouvrir le formulaire, la méthode définit la variable `estCuisinier` à `false` pour indiquer que 
        /// l'utilisateur actuel n'est pas un cuisinier.
        /// Une instance du formulaire `GestionPlats` est créée en passant la chaîne de connexion à la base de données 
        /// et l'état `estCuisinier`, puis le formulaire est affiché en mode modal (avec `ShowDialog()`).
        /// </summary>
        private void OuvrirGestionPlats()
        {
            GestionPlats formPlats = new GestionPlats(chaineConnexion);
            formPlats.ShowDialog();
        }

        /// <summary>
        /// Ouvre le formulaire de gestion des commandes.
        /// Ce formulaire permet de visualiser et gérer les commandes dans l'application.
        /// Une instance du formulaire `GestionCommandes` est créée en passant la chaîne de connexion à la base de données, 
        /// puis le formulaire est affiché en mode modal (avec `ShowDialog()`).
        /// </summary>
        private void OuvrirGestionCommandes()
        {
            GestionCommandes formCommandes = new GestionCommandes(chaineConnexion);
            formCommandes.ShowDialog();
        }

        private void OuvrirStatistiques()
        {
            Statistiques formStatistiques = new Statistiques(chaineConnexion);
            formStatistiques.ShowDialog();
        }
        /// <summary>
        /// Ouvre le formulaire d'administration.
        /// Ce formulaire permet d'accéder à la gestion administrative de l'application.
        /// Une instance du formulaire `Admin` est créée en passant la chaîne de connexion à la base de données, 
        /// puis le formulaire est affiché en mode non modal (avec `Show()`).
        /// </summary>
        private void OuvrirAdmin()
        {
            Admin adminForm = new Admin(chaineConnexion);
            adminForm.Show();
        }
        /// <summary>
        /// Ouvre le formulaire de gestion des avis.
        /// </summary>
        private void OuvrirGestionAvis()
        {
            GestionAvis formAvis = new GestionAvis(chaineConnexion);
            formAvis.ShowDialog();
        }

        private void OuvrirCCForm()
        {
            CCForm formCCForm = new CCForm(chaineConnexion);
            formCCForm.ShowDialog();
        }

        private void OuvrirGestionLivraison()
        {
            GestionLivraison formLivraison = new GestionLivraison(chaineConnexion);
            formLivraison.ShowDialog();
        }

        #region Menu pour le client 
        // Mes plats pour le client
        private DataGridView dataGridViewPlats;
        private void ChargerPlats()
        {
            using (Form formPlats = new Form())
            {
                formPlats.Text = "Mes Plats";
                formPlats.Size = new Size(1550, 600);
                formPlats.StartPosition = FormStartPosition.CenterScreen;
                dataGridViewPlats = new DataGridView
                {
                    Location = new Point(30, 30),
                    Size = new Size(850, 300),
                    ReadOnly = true,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect
                };
                formPlats.Controls.Add(dataGridViewPlats);

                dataGridViewPanier = new DataGridView
                {
                    Location = new Point(900, 30),
                    Size = new Size(400, 300),
                    ReadOnly = true,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                };
                formPlats.Controls.Add(dataGridViewPanier);

                Button buttonAjouterPanier = new Button
                {
                    Text = "Ajouter au panier",
                    Location = new Point(30, 350),
                    Size = new Size(250, 30)
                };
                buttonAjouterPanier.Click += AjouterAuPanier;
                buttonAjouterPanier.FlatStyle = FlatStyle.Flat;
                buttonAjouterPanier.BackColor = Color.LightGreen;
                buttonAjouterPanier.FlatAppearance.BorderSize = 0;
                buttonAjouterPanier.ForeColor = Color.Black;
                buttonAjouterPanier.Font = new Font("Arial", 10, FontStyle.Bold);
                AppliquerCoinsArrondis(buttonAjouterPanier, 10);
                formPlats.Controls.Add(buttonAjouterPanier);

                labelTotal = new Label
                {
                    Text = "Total : 0 €",
                    Location = new Point(900, 350),
                    Size = new Size(200, 30),
                    Font = new Font("Arial", 10, FontStyle.Bold)
                };
                formPlats.Controls.Add(labelTotal);

                Button buttonValiderCommande = new Button
                {
                    Text = "Valider la commande",
                    Location = new Point(1100, 350),
                    Size = new Size(200, 30)
                };
                buttonValiderCommande.Click += ValiderCommande;
                buttonValiderCommande.FlatStyle = FlatStyle.Flat;
                buttonValiderCommande.FlatAppearance.BorderSize = 0;
                buttonValiderCommande.BackColor = Color.LightGreen;
                buttonValiderCommande.ForeColor = Color.Black;
                AppliquerCoinsArrondis(buttonValiderCommande, 10);
                formPlats.Controls.Add(buttonValiderCommande);

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();
                        string requete = "SELECT IdPlat, NomPlat, Categorie, Prix,TypePlat FROM Plat";
                        using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                        {
                            using (MySqlDataAdapter adaptateur = new MySqlDataAdapter(commande))
                            {
                                DataTable tablePlats = new DataTable();
                                adaptateur.Fill(tablePlats);
                                dataGridViewPlats.DataSource = tablePlats;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur de chargement des plats : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                formPlats.ShowDialog();
            }
        }
        private void AjouterAuPanier(object sender, EventArgs e)
        {
            if (dataGridViewPlats.SelectedRows.Count > 0)
            {
                var ligne = ((DataRowView)dataGridViewPlats.SelectedRows[0].DataBoundItem).Row;
                string input = Microsoft.VisualBasic.Interaction.InputBox("Choisissez la quantité :", "Quantité", "1");
                if (int.TryParse(input, out int quantiteChoisie) && quantiteChoisie > 0)
                {
                    int idPlat = Convert.ToInt32(ligne["IdPlat"]);
                    string nomPlat = ligne["NomPlat"].ToString();
                    decimal prix = Convert.ToDecimal(ligne["Prix"]);

                    decimal totalLigne = prix * quantiteChoisie;
                    totalPrix += totalLigne;
                    labelTotal.Text = $"Total : {totalPrix} €";

                    DataTable panierTable = dataGridViewPanier.DataSource as DataTable;
                    if (panierTable == null)
                    {
                        panierTable = new DataTable();
                        panierTable.Columns.Add("IdPlat");
                        panierTable.Columns.Add("NomPlat");
                        panierTable.Columns.Add("Quantité");
                        panierTable.Columns.Add("Prix unitaire");
                        panierTable.Columns.Add("Total");

                        dataGridViewPanier.DataSource = panierTable;
                    }

                    panierTable.Rows.Add(idPlat, nomPlat, quantiteChoisie, prix, totalLigne);
                }
                else
                {
                    MessageBox.Show("Quantité invalide.");
                }
            }
        }
        private string ChoisirMoyenPaiement()
        {
            using (Form paiementForm = new Form())
            {
                paiementForm.Text = "Choisir le moyen de paiement";
                paiementForm.Size = new Size(300, 200);
                paiementForm.StartPosition = FormStartPosition.CenterParent;

                RadioButton rbCarte = new RadioButton
                {
                    Text = "Carte",
                    Location = new Point(30, 30),
                    Checked = true
                };

                RadioButton rbEspece = new RadioButton
                {
                    Text = "Espèce",
                    Location = new Point(30, 60)

                };

                Button btnValider = new Button
                {
                    Text = "Valider",
                    Location = new Point(30, 100),
                    Size = new Size(100, 30),
                    DialogResult = DialogResult.OK
                };
                btnValider.FlatStyle = FlatStyle.Flat;
                btnValider.BackColor = Color.LightGreen;
                btnValider.ForeColor = Color.Black;
                btnValider.Font = new Font("Arial", 10, FontStyle.Bold);
                AppliquerCoinsArrondis(btnValider, 10);

                paiementForm.Controls.AddRange(new Control[] { rbCarte, rbEspece, btnValider });
                paiementForm.AcceptButton = btnValider;

                if (paiementForm.ShowDialog() == DialogResult.OK)
                {
                    if (rbCarte.Checked)
                    {
                        return "Carte";
                    }
                    else
                    {
                        return "Espèce";
                    }
                }
                return null;
            }
        }
        private void ValiderCommande(object sender, EventArgs e)
    {
        if (dataGridViewPanier.DataSource is not DataTable panierTable || panierTable.Rows.Count == 0)
        {
            MessageBox.Show("Le panier est vide.");
            return;
        }
        string moyenPaiement = ChoisirMoyenPaiement();
       if (moyenPaiement == "" || moyenPaiement == null)
        {
            MessageBox.Show("Vous devez choisir un moyen de paiement.");
            return;
        }
        string codePromo = Microsoft.VisualBasic.Interaction.InputBox("Entrez votre code promo (ou laissez vide) :", "Code Promo", "");
        decimal reduction = 0;
        int pointsNecessaires = 0;
    
        if (codePromo != "" && codePromo != null)
        {
            switch (codePromo.ToUpper())
            {
                case "FIDELITE10": reduction = 0.10m; pointsNecessaires = 10; break;
                case "FIDELITE15": reduction = 0.15m; pointsNecessaires = 20; break;
                case "FIDELITE20": reduction = 0.20m; pointsNecessaires = 30; break;
                case "FIDELITE25": reduction = 0.25m; pointsNecessaires = 50; break;
                case "FIDELITE30": reduction = 0.30m; pointsNecessaires = 100; break;
                default: MessageBox.Show("Code promo invalide.");
                    return;
            }

            try
            {
                using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                {
                    connexion.Open();
                    string requetePoints = "SELECT PointsFidelite FROM Fidelite_Client WHERE IdClient = @IdClient";
                    MySqlCommand cmd = new MySqlCommand(requetePoints, connexion);
                    cmd.Parameters.AddWithValue("@IdClient", IdClientConnecte);
                    
                    object result = cmd.ExecuteScalar();
                    int pointsDisponibles;
                    if (result != null)
                    {
                        pointsDisponibles = Convert.ToInt32(result);
                    }
                    else
                    {
                        pointsDisponibles = 0;
                    }
    
                    if (pointsDisponibles < pointsNecessaires)
                    {
                        MessageBox.Show($"Points insuffisants. Vous avez {pointsDisponibles} points et il en faut {pointsNecessaires}.");
                        return;
                    }

                    string requeteMaj = @"
                        UPDATE Fidelite_Client 
                        SET PointsFidelite = PointsFidelite - @Points,
                            PointsUtilises = PointsUtilises + @Points
                        WHERE IdClient = @IdClient";
    
                    cmd = new MySqlCommand(requeteMaj, connexion);
                    cmd.Parameters.AddWithValue("@Points", pointsNecessaires);
                    cmd.Parameters.AddWithValue("@IdClient", IdClientConnecte);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la vérification des points : " + ex.Message);
                return;
            }
        }
        try
        {
            using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
            {
                connexion.Open();
    
                List<int> cuisinierIds = new List<int>();
                MySqlCommand cmd = new MySqlCommand("SELECT IdCuisinier FROM Cuisinier", connexion);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cuisinierIds.Add(reader.GetInt32(0));
                    }
                }
    
                if (cuisinierIds.Count == 0)
                {
                    MessageBox.Show("Aucun cuisinier disponible.");
                    return;
                }
    
                cmd = new MySqlCommand("SELECT IdAdresse FROM Client WHERE IdClient = @IdClient", connexion);
                cmd.Parameters.AddWithValue("@IdClient", IdClientConnecte);
                int idAdresse = Convert.ToInt32(cmd.ExecuteScalar());
    
                Random rnd = new Random();
                foreach (DataRow ligne in panierTable.Rows)
                {
                    int idPlat = Convert.ToInt32(ligne["IdPlat"]);
                    int quantite = Convert.ToInt32(ligne["Quantité"]);
                    decimal prixTotal = Convert.ToDecimal(ligne["Total"]) * (1 - reduction);
                    int idCuisinier = cuisinierIds[rnd.Next(cuisinierIds.Count)];
    
                    string requete = @"
                        INSERT INTO Commande 
                        (IdAdresse, IdClient, IdCuisinier, IdPlat, Quantite, Prix, MoyenPaiement, CodePromoUtilise, Reduction)
                        VALUES 
                        (@IdAdresse, @IdClient, @IdCuisinier, @IdPlat, @Quantite, @Prix, @MoyenPaiement, @CodePromo, @Reduction)";
    
                    cmd = new MySqlCommand(requete, connexion);
                    cmd.Parameters.AddWithValue("@IdAdresse", idAdresse);
                    cmd.Parameters.AddWithValue("@IdClient", IdClientConnecte);
                    cmd.Parameters.AddWithValue("@IdCuisinier", idCuisinier);
                    cmd.Parameters.AddWithValue("@IdPlat", idPlat);
                    cmd.Parameters.AddWithValue("@Quantite", quantite);
                    cmd.Parameters.AddWithValue("@Prix", prixTotal);
                    cmd.Parameters.AddWithValue("@MoyenPaiement", moyenPaiement);
                    cmd.Parameters.AddWithValue("@CodePromo", codePromo);
                    cmd.Parameters.AddWithValue("@Reduction", reduction);
                    cmd.ExecuteNonQuery();
                }
            }
            decimal montantFinal = totalPrix * (1 - reduction);
            MessageBox.Show(
                $"Commande enregistrée avec succès !\n" +
                $"Montant total avant réduction : {totalPrix:C2}\n" +
                $"Réduction appliquée : {(reduction * 100):0}%\n" +
                $"Montant final : {montantFinal:C2}");
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();
                        string requeteInitPoints = @"
                            INSERT INTO Fidelite_Client (IdClient, PointsFidelite, PointsUtilises)
                            VALUES (@IdClient, FLOOR(@MontantFinal/10), 0)
                            ON DUPLICATE KEY UPDATE 
                            PointsFidelite = PointsFidelite + FLOOR(@MontantFinal/10)";
    
                        using (MySqlCommand cmdPoints = new MySqlCommand(requeteInitPoints, connexion))
                        {
                            cmdPoints.Parameters.AddWithValue("@IdClient", IdClientConnecte);
                            cmdPoints.Parameters.AddWithValue("@MontantFinal", montantFinal);
                            cmdPoints.ExecuteNonQuery();
                        }
                    }
            dataGridViewPanier.DataSource = null;
            totalPrix = 0;
            labelTotal.Text = "Total : 0 €";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de l'enregistrement de la commande : " + ex.Message);
        }
        try
        {
            using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
            {
                connexion.Open();
            
                string requeteNotification = @"
                    INSERT INTO Notification (Message, MoyenEnvoi, DateHeure, Statut)
                    VALUES (@Message, @MoyenEnvoi, NOW(), @Statut)";

                using (MySqlCommand cmdNotification = new MySqlCommand(requeteNotification, connexion))
                {
                    cmdNotification.Parameters.AddWithValue("@Message", "Votre commande est en cours de préparation. Notre chef s'en occupe !");
                    cmdNotification.Parameters.AddWithValue("@MoyenEnvoi", "Application");
                    cmdNotification.Parameters.AddWithValue("@Statut", "Non lu");
                    cmdNotification.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erreur lors de la création de la notification : " + ex.Message);
        }
    }
        private void FideliteClient(object sender, EventArgs e)
        {
            using (Form formFidelite = new Form())
            {
                formFidelite.Text = "Mes points de fidélité";
                formFidelite.StartPosition = FormStartPosition.CenterScreen;
                Label lbltitre = new Label
                {
                    Text = "Mes points de fidélité :",
                    Location = new Point(50, 0),
                    Size = new Size(500, 30),
                    Font = new Font("Arial", 16, FontStyle.Bold)
                };
                formFidelite.Controls.Add(lbltitre);
                formFidelite.Size = new Size(1000, 900);
                formFidelite.StartPosition = FormStartPosition.CenterParent;
                DataGridView dgvPointsFidelite = new DataGridView
                {
                    Location = new Point(50, 70),
                    Size = new Size(800, 200),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true
                };
                formFidelite.Controls.Add(dgvPointsFidelite);

                Label lblTotalPoints = new Label
                {
                    Location = new Point(50, 280),
                    Size = new Size(300, 30),
                    Font = new Font("Arial", 12, FontStyle.Bold)
                };
                formFidelite.Controls.Add(lblTotalPoints);

                DataGridView dgvRecompenses = new DataGridView
                {
                    Location = new Point(50, 370),
                    Size = new Size(800, 200),
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true
                };
                formFidelite.Controls.Add(dgvRecompenses);

                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();

                        string requeteCommandes = @"
                    SELECT 
                        c.IdCommande,
                        c.Prix,
                        DATE(c.DateCommande) as DateCommande,
                        FLOOR(c.Prix / 10) as PointsFidelite
                    FROM Commande c
                    WHERE c.IdClient = @IdClient
                    ORDER BY c.DateCommande DESC";

                        using (MySqlCommand cmd = new MySqlCommand(requeteCommandes, connexion))
                        {
                            cmd.Parameters.AddWithValue("@IdClient", IdClientConnecte);

                            DataTable tablePoints = new DataTable();
                            using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                            {
                                adapter.Fill(tablePoints);
                            }
                            dgvPointsFidelite.DataSource = tablePoints;

                            int totalPoints = tablePoints.AsEnumerable()
                                .Sum(row => Convert.ToInt32(row["PointsFidelite"]));
                            lblTotalPoints.Text = $"Total des points de fidélité : {totalPoints} points";
                            lblTotalPoints.Size = new Size(500, 30);

                            DataTable tableRecompenses = new DataTable();
                            tableRecompenses.Columns.Add("Points Requis", typeof(int));
                            tableRecompenses.Columns.Add("Réduction", typeof(string));
                            tableRecompenses.Columns.Add("Code Promo", typeof(string));
                            tableRecompenses.Columns.Add("Statut", typeof(string));

                            var recompenses = new[]
                            {
                        new { Points = 10, Reduction = "-10%", Code = "FIDELITE10" },
                        new { Points = 20, Reduction = "-15%", Code = "FIDELITE15" },
                        new { Points = 30, Reduction = "-20%", Code = "FIDELITE20" },
                        new { Points = 50, Reduction = "-25%", Code = "FIDELITE25" },
                        new { Points = 100, Reduction = "-30%", Code = "FIDELITE30" }
                    };

                            foreach (var r in recompenses)
                            {
                                string statut;
                                string code;
                                if (totalPoints >= r.Points) {
                                    statut = "Disponible";
                                    code = r.Code;
                                }
                                else {
                                    statut = $"Manque {r.Points - totalPoints} points";
                                    code = "---";
                                }

                                tableRecompenses.Rows.Add(r.Points, r.Reduction, code, statut);
                            }

                            dgvRecompenses.DataSource = tableRecompenses;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du chargement des points de fidélité : {ex.Message}",
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                formFidelite.ShowDialog();
            }
        }        
        private DataGridView dataGridViewCommandes;
        private void ChargerCommandes()
        {
            using (Form formCommandes = new Form())
            {
                Label lbltitre = new Label
                {
                    Text = "Mes commandes :",
                    Location = new Point(50, 0),
                    Size = new Size(500, 30),
                    Font = new Font("Segeo UI", 16, FontStyle.Bold)
                };
                formCommandes.Controls.Add(lbltitre);
                formCommandes.Text = "Mes Plats";
                formCommandes.Size = new Size(900, 500);
                formCommandes.StartPosition = FormStartPosition.CenterScreen;
                dataGridViewCommandes = new DataGridView
                {
                    Location = new Point(50, 100),
                    Size = new Size(800, 250)
                };
                formCommandes.Controls.Add(dataGridViewCommandes);
                try
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();
                        string requete = "SELECT * FROM Commande WHERE IdClient = @IdClient";
                        using (MySqlCommand commande = new MySqlCommand(requete, connexion))
                        {
                            commande.Parameters.AddWithValue("@IdClient", IdClientConnecte);
                            using (MySqlDataAdapter adaptateur = new MySqlDataAdapter(commande))
                            {
                                DataTable tableCommandes = new DataTable();
                                adaptateur.Fill(tableCommandes);
                                dataGridViewCommandes.DataSource = tableCommandes;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur de chargement des plats : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                formCommandes.ShowDialog();
            }
        }
        DataGridView dataGridViewAvis = new DataGridView();
        private void ButtonAjouter_Click(object sender, EventArgs e)
        {
            using (Form formNouveauAvis = new Form())
            {
                formNouveauAvis.Text = "Nouvel avis";
                formNouveauAvis.Size = new Size(600, 400);
                formNouveauAvis.StartPosition = FormStartPosition.CenterParent;

                Label[] labels = new Label[]
                {
                    new Label { Text = "Note", Location = new Point(50, 50) },
                    new Label { Text = "Commentaire", Location = new Point(50, 100), Size = new Size(100, 30) },
                };

                ComboBox Note = new ComboBox { Location = new Point(200, 50) };
                for (int i = 1; i <= 5; i++)
                {
                    Note.Items.Add(i);
                }

                TextBox Commentaire = new TextBox { Location = new Point(250, 100), Size = new Size(200, 100), Multiline = true };

                Button buttonValider = new Button { Text = "Valider", Location = new Point(50, 250), Size = new Size(100, 30) };
                buttonValider.Click += (s, args) => AjouterAvis(Note.SelectedItem.ToString(), Commentaire.Text, formNouveauAvis);
                buttonValider.FlatStyle = FlatStyle.Flat;
                buttonValider.BackColor = Color.LightGreen;
                buttonValider.ForeColor = Color.Black;
                buttonValider.Font = new Font("Arial", 10, FontStyle.Bold);
                AppliquerCoinsArrondis(buttonValider, 10);
                Button buttonAnnuler = new Button { Text = "Annuler", Location = new Point(200, 250),Size = new Size(100, 30) };
                buttonAnnuler.Click += (s, args) => formNouveauAvis.Close();
                buttonAnnuler.FlatStyle = FlatStyle.Flat;
                buttonAnnuler.BackColor = Color.LightCoral;
                buttonAnnuler.ForeColor = Color.Black;
                buttonAnnuler.Font = new Font("Arial", 10, FontStyle.Bold);
                AppliquerCoinsArrondis(buttonAnnuler, 10);
                formNouveauAvis.Controls.AddRange(labels);
                formNouveauAvis.Controls.Add(Note);
                formNouveauAvis.Controls.Add(Commentaire);
                formNouveauAvis.Controls.Add(buttonValider);
                formNouveauAvis.Controls.Add(buttonAnnuler);

                formNouveauAvis.ShowDialog();
            }
        }
        private void AjouterAvis(string note, string commentaire, Form form)
        {
            using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
            {
                connexion.Open();
                using (var commande = connexion.CreateCommand())
                {
                    commande.CommandText = "INSERT INTO Avis (Commentaire, Note) VALUES (@Commentaire, @Note)";
                    commande.Parameters.AddWithValue("@Commentaire", commentaire);
                    commande.Parameters.AddWithValue("@Note", note);
                    commande.ExecuteNonQuery();
                }
            }
            form.Close();
            ChargerAvis();
        }
        private void ChargerAvis()
        {
            
            using (Form formAvis = new Form())
            {
                Label lbltitre = new Label
                {
                    Text = "Avis :",
                    Location = new Point(50, 0),
                    Size = new Size(500, 30),
                    Font = new Font("Segeo UI", 16, FontStyle.Bold)
                };
                formAvis.Controls.Add(lbltitre);
                formAvis.Text = "Mes Plats";
                formAvis.Size = new Size(900, 500);
                formAvis.StartPosition = FormStartPosition.CenterScreen;
                dataGridViewAvis = new DataGridView
                {
                    Location = new Point(50, 80),
                    Size = new Size(800, 250)
                };
                formAvis.Controls.Add(dataGridViewAvis);
                buttonAjouter = new Button
                {
                    Text = "Ajouter un avis",
                    Location = new Point(50, 400),
                    Size = new Size(200, 30)
                };
                buttonAjouter.Click += ButtonAjouter_Click;
                buttonAjouter.FlatStyle = FlatStyle.Flat;
                buttonAjouter.FlatAppearance.BorderSize = 0;
                buttonAjouter.BackColor = Color.LightGreen;
                buttonAjouter.ForeColor = Color.Black;
                AppliquerCoinsArrondis(buttonAjouter, 10);
                formAvis.Controls.Add(buttonAjouter);

                try
                {


                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();
                        using (var commande = connexion.CreateCommand())
                        {
                            commande.CommandText = "SELECT IdAvis, Commentaire, Note FROM Avis";
                            using (var reader = commande.ExecuteReader())
                            {
                                DataTable table = new DataTable();
                                table.Load(reader);
                                dataGridViewAvis.DataSource = table;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur de chargement des avis : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                formAvis.ShowDialog();
            }
        }

        #endregion
        #endregion

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }

        private void btnQuitter_Click(object sender, EventArgs e)
        {
            // Ferme l'application
            Application.Exit();
        }
    }
}