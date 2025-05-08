using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSIV10
{
    public partial class GrapheForm : Form
    {
        #region Déclaration des contrôles
        private Graphe graphe;
        private AffichageGraphe affichageGraphe;
        private List<(string NomStation, string StationPre, string StationSuiv, int TempsChangement)> connexions;
        private float zoomFactor = 1.0f;
        private PointF translation = new PointF(0, 0);
        private Panel panelAffichage;
        public ComboBox txtDepart;
        public ComboBox txtArrivee;
        public Button btnDijkstra;
        private Button btnBellmanFord;
        private Button btnFloydWarshall;
        private Button btnReset;
        private List<Lien> cheminDijkstra;
        private List<Lien> cheminBellmanFord;
        private List<Lien> cheminFloyd;
        private List<Lien> cheminAetoile;
        private Label lblChemin;
        private Label lblTitre;
        private bool glissement = false;
        private Point lastMousePosition;
        private Button btnRetour;

        private bool clique = false;
        #endregion
        /// <summary>
        /// Initialise une nouvelle instance de la classe GrapheForm.
        /// Configure l'interface graphique, charge les données du graphe et associe les événements nécessaires.
        /// </summary>
        public GrapheForm()
        {

            InitializeComponent();
            lblTitre = new Label
            {
                Text = "Liv'In Paris\nArthur BIET Noé WALLNER",
                Font = new Font("Times New Roman", 16, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(lblTitre);
            PositionnerLabel();
            this.Resize += (s, e) => PositionnerLabel();

            panelAffichage = new Panel { Dock = DockStyle.Fill };
            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, panelAffichage, new object[] { true });

            this.Controls.Add(panelAffichage);

            graphe = new Graphe();

            graphe.ImporterStationsCSV();
            connexions = graphe.RecupererConnexions();
            graphe.ImporterLiens();
            affichageGraphe = new AffichageGraphe(graphe);
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Affichage du Graphe";

            panelAffichage.Paint += new PaintEventHandler(Dessine);
            panelAffichage.MouseWheel += new MouseEventHandler(Zoom);
            panelAffichage.MouseDown += new MouseEventHandler(SourisPress);
            panelAffichage.MouseMove += new MouseEventHandler(SourisMouv);
            panelAffichage.MouseUp += new MouseEventHandler(MouseReleased);
            cheminDijkstra = new List<Lien>();

            txtDepart = new ComboBox { Top = 50, Left = 10, Width = 150, Height = 30 };
            txtArrivee = new ComboBox { Top = 100, Left = 10, Width = 150, Height = 30 };
            btnDijkstra = new Button { Top = 10, Left = 10, Width = 150, Height = 30, Text = "Dijkstra" };
            btnBellmanFord = new Button { Top = 10, Left = 170, Width = 150, Height = 30, Text = "Bellman-Ford" };
            btnFloydWarshall = new Button { Top = 10, Left = 330, Width = 150, Height = 30, Text = "Floyd-Warshall" };

            btnRetour = new Button { Top = 10, Left = 650, Width = 150, Height = 30, Text = "Retour" };


            btnDijkstra.Click += BtnDijkstra_Click;
            btnBellmanFord.Click += BtnBellmanFord_Click;
            btnFloydWarshall.Click += BtnFloydWarshall_Click;
            btnRetour.Click += (s, e) => { this.Hide(); };

            btnReset = new Button { Top = 150, Left = 10, Width = 150, Height = 30, Text = "Reset" };
            btnReset.Click += BtnReset_Click;
            txtDepart.DataSource = graphe.Noeuds.Select(n => n.Nom).ToList();
            txtArrivee.DataSource = graphe.Noeuds.Select(n => n.Nom).ToList();
            lblChemin = new Label
            {
                Top = 190,
                Left = 10,
                Width = 300,
                Height = 100,
                Text = "Chemin :",
            };

            panelAffichage.Controls.Add(txtDepart);
            panelAffichage.Controls.Add(txtArrivee);
            panelAffichage.Controls.Add(btnDijkstra);
            panelAffichage.Controls.Add(btnBellmanFord);
            panelAffichage.Controls.Add(btnFloydWarshall);
            panelAffichage.Controls.Add(btnReset);
            panelAffichage.Controls.Add(lblChemin);
            panelAffichage.Controls.Add(btnRetour);
        }
        /// <summary>
        /// Gère l'événement de clic sur le bouton Dijkstra.
        /// Trouve et affiche le chemin le plus court entre deux stations sélectionnées en utilisant l'algorithme de Dijkstra.
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement.</param>
        /// <param name="e">Données de l'événement.</param>
        private void BtnDijkstra_Click(object sender, EventArgs e)
        {
            if (!clique)
            {
                Noeud depart = null;
                foreach (Noeud noeud in graphe.Noeuds)
                {
                    if (noeud.Nom.ToLower() == txtDepart.Text.ToLower())
                    {
                    depart = noeud;
                    break;
                    }
                }
                Noeud arrivee = null;
                foreach (Noeud noeud in graphe.Noeuds)
                {
                    if (noeud.Nom.ToLower() == txtArrivee.Text.ToLower())
                    {
                    arrivee = noeud;
                    break;
                    }
                }
                if (depart == null || arrivee == null)
                {
                    MessageBox.Show("Station(s) introuvable(s), vérifiez les noms.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<Noeud> cheminNoeuds = graphe.Dijkstra(depart.Id, arrivee.Id);
                if (cheminNoeuds == null || cheminNoeuds.Count == 0)
                {
                    MessageBox.Show("Aucun chemin trouvé entre ces stations.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cheminDijkstra = new List<Lien>();
                for (int i = 0; i < cheminNoeuds.Count - 1; i++)
                {
                    foreach (Lien lien in graphe.Liens)
                    {
                        if ((lien.IdSource == cheminNoeuds[i].Id && lien.IdDestination == cheminNoeuds[i + 1].Id) ||
                            (lien.IdSource == cheminNoeuds[i + 1].Id && lien.IdDestination == cheminNoeuds[i].Id))
                        {
                            cheminDijkstra.Add(lien);
                            break;
                        }
                    }
                }
                string cheminTexte = cheminNoeuds[0].Nom;
                for (int i = 1; i < cheminNoeuds.Count; i++)
                {
                    cheminTexte += " -> " + cheminNoeuds[i].Nom;
                }
                lblChemin.Text = "Chemin : " + cheminTexte;
                panelAffichage.Invalidate();
            }
            clique = !clique;
        }

        /// <summary>
        /// Gère l'événement de clic sur le bouton Bellman-Ford.
        /// Trouve et affiche le chemin le plus court entre deux stations sélectionnées en utilisant l'algorithme de Bellman-Ford.
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement.</param>
        /// <param name="e">Données de l'événement.</param>
        private void BtnBellmanFord_Click(object sender, EventArgs e)
        {
            Noeud depart = null;
            foreach (Noeud noeud in graphe.Noeuds)
            {
                if (noeud.Nom.ToLower() == txtDepart.Text.ToLower())
                {
                    depart = noeud;
                    break;
                }
            }
            Noeud arrivee = null;
            foreach (Noeud noeud in graphe.Noeuds)
            {
                if (noeud.Nom.ToLower() == txtArrivee.Text.ToLower())
                {
                    arrivee = noeud;
                    break;
                }
            }

            if (depart == null || arrivee == null)
            {
                MessageBox.Show("Station(s) introuvable(s), vérifiez les noms.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<Noeud> cheminNoeuds = graphe.BellmanFord(depart.Id, arrivee.Id);

            if (cheminNoeuds == null || cheminNoeuds.Count == 0)
            {
                MessageBox.Show("Aucun chemin trouvé entre ces stations.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            cheminBellmanFord = new List<Lien>();
            for (int i = 0; i < cheminNoeuds.Count - 1; i++)
            {
                foreach (Lien lien in graphe.Liens)
                {
                    if ((lien.IdSource == cheminNoeuds[i].Id && lien.IdDestination == cheminNoeuds[i + 1].Id) ||
                        (lien.IdSource == cheminNoeuds[i + 1].Id && lien.IdDestination == cheminNoeuds[i].Id))
                    {
                        cheminBellmanFord.Add(lien);
                        break;
                    }
                }
            }

            string cheminTexte = cheminNoeuds[0].Nom;
            for (int i = 1; i < cheminNoeuds.Count; i++)
            {
                cheminTexte += " -> " + cheminNoeuds[i].Nom;
            }
            lblChemin.Text = "Chemin : " + cheminTexte;

            panelAffichage.Invalidate();
        }
        private void BtnFloydWarshall_Click(object sender, EventArgs e)
        {
            Noeud depart = null;
            foreach (Noeud noeud in graphe.Noeuds)
            {
                if (noeud.Nom.ToLower() == txtDepart.Text.ToLower())
                {
                    depart = noeud;
                    break;
                }
            }

            Noeud arrivee = null;
            foreach (Noeud noeud in graphe.Noeuds)
            {
                if (noeud.Nom.ToLower() == txtArrivee.Text.ToLower())
                {
                    arrivee = noeud;
                    break;
                }
            }

            if (depart == null || arrivee == null)
            {
                MessageBox.Show("Station(s) introuvable(s), vérifiez les noms.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<Noeud> cheminNoeuds = graphe.FloydWarshall(depart.Id, arrivee.Id);

            if (cheminNoeuds == null || cheminNoeuds.Count == 0)
            {
                MessageBox.Show("Aucun chemin trouvé entre ces stations.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            cheminFloyd = new List<Lien>();
            for (int i = 0; i < cheminNoeuds.Count - 1; i++)
            {
                foreach (Lien lien in graphe.Liens)
                {
                    if ((lien.IdSource == cheminNoeuds[i].Id && lien.IdDestination == cheminNoeuds[i + 1].Id) ||
                        (lien.IdSource == cheminNoeuds[i + 1].Id && lien.IdDestination == cheminNoeuds[i].Id))
                    {
                        cheminFloyd.Add(lien);
                        break;
                    }
                }
            }

            string cheminTexte = cheminNoeuds[0].Nom;
            for (int i = 1; i < cheminNoeuds.Count; i++)
            {
                cheminTexte += " -> " + cheminNoeuds[i].Nom;
            }
            lblChemin.Text = "Chemin : " + cheminTexte;

            panelAffichage.Invalidate();
        }
        
        /// <summary>
        /// Gère l'événement de dessin du panneau d'affichage.
        /// Applique les transformations de zoom et de translation avant d'afficher le graphe avec ses connexions et chemins calculés.
        /// </summary>
        /// <param name="sender">Objet déclencheur de l'événement.</param>
        /// <param name="e">Données de l'événement contenant l'objet Graphics.</param>
        private void Dessine(object sender, PaintEventArgs e)
        {

            Graphics g = e.Graphics;
            g.TranslateTransform(translation.X, translation.Y);
            g.ScaleTransform(zoomFactor, zoomFactor);
            var connexions = graphe.RecupererConnexions();
            affichageGraphe.Dessiner(g, connexions, cheminDijkstra, cheminBellmanFord, cheminFloyd, txtDepart.Text);

        }
        /// <summary>
        /// Positionne le label du titre au centre horizontal de la fenêtre.
        /// Ajuste sa position en fonction de la taille actuelle de la fenêtre.
        /// </summary>
        private void PositionnerLabel()
        {
            lblTitre.Left = (this.ClientSize.Width - lblTitre.Width) / 2;
            lblTitre.Top = 10;
        }
        /// <summary>
        /// Gère l'événement de clic sur le bouton Reset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       private void BtnReset_Click(object sender, EventArgs e)
        {
        try 
        {
        zoomFactor = 1.0f;
        translation = new PointF(0, 0);

        if (cheminDijkstra != null) cheminDijkstra.Clear();
        if (cheminBellmanFord != null) cheminBellmanFord.Clear();
        if (cheminFloyd != null) cheminFloyd.Clear();

        if (lblChemin != null) lblChemin.Text = "Chemin :";
        if (txtDepart != null) txtDepart.SelectedIndex = -1;
        if (txtArrivee != null) txtArrivee.SelectedIndex = -1;

        if (panelAffichage != null) panelAffichage.Invalidate();
        }
        catch (Exception ex)
        {
        MessageBox.Show($"Erreur lors de la réinitialisation : {ex.Message}", 
                       "Erreur", 
                       MessageBoxButtons.OK, 
                       MessageBoxIcon.Error);
        }
}

        #region Souris
        /// <summary>
        /// Gère le zoom sur le panel d'affichage lors d'un événement de molette de souris.
        /// La méthode ajuste le facteur de zoom en fonction du mouvement de la molette de la souris (en avant ou en arrière).
        /// Le zoom est effectué autour du point sous la souris, avec une plage de zoom limitée entre 0.2 et 5.
        /// Le zoom est appliqué en ajustant le facteur de zoom et la position de translation de l'affichage.
        /// </summary>
        /// <param name="sender">L'objet source de l'événement.</param>
        /// <param name="e">L'argument de l'événement contenant les informations sur la molette de la souris (delta, position, etc.).</param>
        private void Zoom(object sender, MouseEventArgs e)
        {
            const float zoomStep = 0.1f;
            float oldZoomFactor = zoomFactor;
            float newZoomFactor;
            if (e.Delta > 0)
                newZoomFactor = zoomFactor * (1 + zoomStep);
            else
                newZoomFactor = zoomFactor / (1 + zoomStep);

            if (newZoomFactor < 0.2f || newZoomFactor > 5f)
                return;

            float mouseXBefore = (e.X - translation.X) / oldZoomFactor;
            float mouseYBefore = (e.Y - translation.Y) / oldZoomFactor;

            zoomFactor = newZoomFactor;

            translation.X = e.X - (mouseXBefore * zoomFactor);
            translation.Y = e.Y - (mouseYBefore * zoomFactor);

            panelAffichage.Invalidate();
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
                panelAffichage.Invalidate();
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

    }
}
