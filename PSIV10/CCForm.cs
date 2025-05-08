using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PSIV10
{
    public partial class CCForm : Form
    {
        // Déclaration des variables de classe
        private GrapheClientCuisinier graphe;        
        private string chaineConnexion;              
        private Panel panelGraphe;                  
        private float zoomFactor = 1.0f;          
        private Point deplacement = new Point(0, 0); 
        private PointF translation = new PointF(0, 0); 
        private bool glissement = false;           
        private Point lastMousePosition;            
        private Label txtNbCouleurs;
        private Label txtBiparti;
        private Label txtPlanaire;
        private Button btnColorer;


        public CCForm(string connexion)
        {
            InitializeComponent();
            this.Text = "Carte des Clients et Cuisiniers";
            this.Size = new Size(1500, 850);
            this.StartPosition = FormStartPosition.CenterScreen;
            chaineConnexion = connexion;

            panelGraphe = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null, panelGraphe, new object[] { true });

            this.Controls.Add(panelGraphe);
            PanelInformations();
            BoutonColoration();
            graphe = new GrapheClientCuisinier(chaineConnexion);
            graphe.ChargerClientsEtCuisiniers(chaineConnexion);

            List<Arete> commandes = ChargerCommandesDepuisBDD(chaineConnexion);
            graphe.GrapheCommandes(commandes);

            zoomFactor = 1.0f;
            translation = new PointF(0, 0);

            panelGraphe.Paint += DessinerGraphe;
            panelGraphe.MouseWheel += Zoom;
            panelGraphe.MouseDown += SourisPress;
            panelGraphe.MouseMove += SourisMouv;
            panelGraphe.MouseUp += MouseReleased;

            CentrerGraphe();
        }
        private void PanelInformations()
        {
            Panel panelInfo = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(10)
            };

            Label lblTitre = new Label
            {
                Text = "Propriétés du Graphe",
                Location = new Point(10, 20),
                Size = new Size(270, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblNbCouleursTitle = new Label
            {
                Text = "Nombre de couleurs :",
                Location = new Point(10, 70),
                Size = new Size(180, 25),
                Font = new Font("Arial", 9)
            };

            Label lblBipartiTitle = new Label
            {
                Text = "Graphe biparti :",
                Location = new Point(10, 110),
                Size = new Size(140, 25),
                Font = new Font("Arial", 9)
            };

            Label lblPlanaireTitle = new Label
            {
                Text = "Graphe planaire :",
                Location = new Point(10, 150),
                Size = new Size(150, 25),
                Font = new Font("Arial", 9)
            };

            txtNbCouleurs = new Label
            {
                Location = new Point(190, 70),
                Size = new Size(60, 25),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle
            };

            txtBiparti = new Label
            {
                Location = new Point(170, 110),
                Size = new Size(120, 25),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle
            };

            txtPlanaire = new Label
            {
                Location = new Point(170, 150),
                Size = new Size(120, 25),
                Font = new Font("Arial", 9, FontStyle.Bold),
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                BorderStyle = BorderStyle.FixedSingle
            };

            Button btnAnalyser = new Button
            {
                Text = "Analyser le graphe",
                Location = new Point(20, 200),
                Size = new Size(270, 35),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            this.Controls.Add(btnAnalyser);
            btnAnalyser.Click += BtnAnalyser_Click;
            
            Button btnReset = new Button
            {
                Text = "Reset",
                Location = new Point(20, 250),
                Size = new Size(270, 35),
                Font = new Font("Arial", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            this.Controls.Add(btnReset);
            btnReset.Click += BtnReset_Click;

            panelInfo.Controls.AddRange(new Control[] {
        lblTitre,
        lblNbCouleursTitle, txtNbCouleurs,
        lblBipartiTitle, txtBiparti,
        lblPlanaireTitle, txtPlanaire,
        btnAnalyser
        });

            this.Controls.Add(panelInfo);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            graphe.ResetGraphe();
            txtNbCouleurs.Text = "0";
            txtBiparti.Text = "Non";
            txtPlanaire.Text = "Non";
            panelGraphe.Invalidate();
        }
        private void BtnAnalyser_Click(object sender, EventArgs e)
        {
                graphe.ColorerGraphe();
                graphe.EstBiparti = graphe.VerifierBiparti();
                graphe.EstPlanaire = graphe.VerifierPlanaire();
                txtNbCouleurs.Text = graphe.NombreChromatique.ToString();
                if (graphe.EstBiparti)
                    txtBiparti.Text = "Oui";
                else 
                    txtBiparti.Text = "Non";
                    
                if (graphe.EstPlanaire)
                    txtPlanaire.Text = "Oui";
                else
                    txtPlanaire.Text = "Non";
                panelGraphe.Invalidate();
        }
        private void BoutonColoration()
        {
            btnColorer = new Button
            {
                Text = "Colorer le graphe",
                Location = new Point(120, 10),
                Size = new Size(120, 30)
            };

            btnColorer.Click += (s, e) =>
            {
                graphe.ColorerGraphe();

                txtNbCouleurs.Text = $"Nombre de couleurs : {graphe.NombreChromatique}";
                if (graphe.EstBiparti)
                    txtBiparti.Text = "Oui";
                else
                    txtBiparti.Text = "Non";

                if (graphe.EstPlanaire)
                    txtPlanaire.Text = "Oui";
                else
                    txtPlanaire.Text = "Non";
                panelGraphe.Invalidate();
            };

            this.Controls.Add(btnColorer);
        }
        private void DessinerGraphe(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);
            e.Graphics.TranslateTransform(translation.X, translation.Y);
            e.Graphics.ScaleTransform(zoomFactor, zoomFactor);
            graphe.AfficherGraphe(e.Graphics);
        }
        #region Souris
        private void Zoom(object sender, MouseEventArgs e)
        {
            const float zoomStep = 0.1f;
            float oldZoomFactor = zoomFactor;
            float newZoomFactor = (e.Delta > 0) ? zoomFactor * (1 + zoomStep) : zoomFactor / (1 + zoomStep);

            if (newZoomFactor < 0.2f || newZoomFactor > 5f)
                return;

            float mouseXBefore = (e.X - translation.X) / oldZoomFactor;
            float mouseYBefore = (e.Y - translation.Y) / oldZoomFactor;

            zoomFactor = newZoomFactor;

            translation.X = e.X - (mouseXBefore * zoomFactor);
            translation.Y = e.Y - (mouseYBefore * zoomFactor);

            panelGraphe.Invalidate();
        }
        /// <summary>
        /// Gère l'événement de pression du bouton gauche de la souris.
        /// Si le bouton gauche de la souris est enfoncé, l'indicateur de "dragging" est activé et la position actuelle de la souris est enregistrée.
        /// Cette méthode permet de démarrer une opération de déplacement (glissement) sur l'élément lorsqu'on clique avec le bouton gauche.
        /// </summary>
        /// <param name="sender">L'objet source de l'événement.</param>
        /// <param name="e">L'argument de l'événement contenant les informations sur la souris, telles que la position et le bouton cliqué.</param>
        private void SourisPress(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                glissement = true;
                lastMousePosition = e.Location;
            }
        }
        /// <summary>
        /// Gère l'événement de déplacement de la souris.
        /// Si l'utilisateur a activé le mode (glissement) en maintenant le bouton gauche enfoncé, cette méthode déplace l'élément selon les mouvements de la souris.
        /// Elle met à jour la position de l'élément en fonction de la différence entre la position actuelle de la souris et la dernière position enregistrée.
        /// </summary>
        /// <param name="sender">L'objet source de l'événement.</param>
        /// <param name="e">L'argument de l'événement contenant les informations sur la souris, telles que la position actuelle de la souris.</param>
        private void SourisMouv(object sender, MouseEventArgs e)
        {
            if (glissement)
            {
                translation.X += e.X - lastMousePosition.X;
                translation.Y += e.Y - lastMousePosition.Y;
                lastMousePosition = e.Location;
                panelGraphe.Invalidate();
            }
        }
        /// <summary>
        /// Gère l'événement lorsque le bouton de la souris est relâché.
        /// Si le bouton gauche de la souris est relâché, cette méthode désactive l'état de "glissement" (dragging).
        /// </summary>
        /// <param name="sender">L'objet source de l'événement.</param>
        /// <param name="e">L'argument de l'événement contenant les informations sur l'état du bouton de la souris et sa position.</param>
        private void MouseReleased(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                glissement = false;
            }
        }
        #endregion

        private void CentrerGraphe()
        {
            if (graphe.Sommets.Count == 0) return;
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            foreach (var sommet in graphe.Sommets)
            {
                if ((float)sommet.X < minX) minX = (float)sommet.X;
                if ((float)sommet.X > maxX) maxX = (float)sommet.X;
                if ((float)sommet.Y < minY) minY = (float)sommet.Y; 
                if ((float)sommet.Y > maxY) maxY = (float)sommet.Y;
            }

            var centreGrapheX = (minX + maxX) / 2;
            var centreGrapheY = (minY + maxY) / 2;

            deplacement = new Point(
                (int)(panelGraphe.Width / 2 - centreGrapheX),
                (int)(panelGraphe.Height / 2 - centreGrapheY)
            );
            panelGraphe.Invalidate();
        }


        private List<Arete> ChargerCommandesDepuisBDD(string connectionString)
        {
            List<Arete> commandes = new List<Arete>();

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                SELECT IdClient, IdCuisinier, COUNT(*) as NbCommandes 
                FROM Commande 
                GROUP BY IdClient, IdCuisinier";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        commandes.Add(new Arete
                        {
                            IdClient = reader.GetInt32("IdClient"),
                            IdCuisinier = reader.GetInt32("IdCuisinier"),
                            NbCommandes = reader.GetInt32("NbCommandes")
                        });
                    }
                }
            }

            return commandes;
        }
    }

}