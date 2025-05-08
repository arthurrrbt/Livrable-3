using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;

namespace PSIV10
{
    public partial class GestionAvis : Form
    {
        private string chaineConnexion;
        private DataGridView dataGridViewAvis;
        private Button buttonAjouter;
        private Button buttonModifier;
        private Button buttonSupprimer;
        private Button btnFermer;
        public GestionAvis(string connexion)
        {
            InitializeComponent();
            chaineConnexion = connexion;
            Initialiser();
            ChargerAvis();
        }

        private void Initialiser()
        {
            this.Text = "Gestion des Avis";
            this.Size = new Size(1100, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            dataGridViewAvis = new DataGridView
            {
                Location = new Point(50, 80),
                Size = new Size(800, 250)
            };
            this.Controls.Add(dataGridViewAvis);

            Label lblTitre = new Label
            {
                Text = "Gestion des avis :",
                Location = new Point(50, 20),
                Size = new Size(250, 50),
                Font = new Font("Segoe UI", 12, FontStyle.Bold | FontStyle.Underline)
            };
            this.Controls.Add(lblTitre);

            buttonAjouter = new Button
            {
                Text = "Ajouter un avis",
                Location = new Point(50, 400),
                Size = new Size(200, 30),
            };
            buttonAjouter.Click += ButtonAjouter_Click;
            buttonAjouter.FlatStyle = FlatStyle.Flat;
            buttonAjouter.FlatAppearance.BorderSize = 0;
            buttonAjouter.BackColor = Color.LightGreen;
            buttonAjouter.ForeColor = Color.White;
            AppliquerCoinsArrondis(buttonAjouter, 10);
            this.Controls.Add(buttonAjouter);

            buttonModifier = new Button
            {
                Text = "Modifier un avis",
                Location = new Point(270, 400),
                Size = new Size(200, 30),
            };
            buttonModifier.Click += ButtonModifier_Click;
            buttonModifier.FlatStyle = FlatStyle.Flat;
            buttonModifier.FlatAppearance.BorderSize = 0;
            buttonModifier.BackColor = Color.LightBlue;
            buttonModifier.ForeColor = Color.White;
            AppliquerCoinsArrondis(buttonModifier, 10);
            this.Controls.Add(buttonModifier);

            buttonSupprimer = new Button
            {
                Text = "Supprimer un avis",
                Location = new Point(490, 400),
                Size = new Size(200, 30),
                BackColor = Color.LightCoral,
                ForeColor = Color.White
            };
            buttonSupprimer.Click += ButtonSupprimer_Click;
            buttonSupprimer.FlatStyle = FlatStyle.Flat;
            buttonSupprimer.FlatAppearance.BorderSize = 0;
            AppliquerCoinsArrondis(buttonSupprimer, 10);
            this.Controls.Add(buttonSupprimer);

            btnFermer = new Button
            {
                Text = "Fermer",
                Location = new Point(800, 400),
                Size = new Size(200, 30),
            };
            btnFermer.Click += (s, e) => this.Close();
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
        private void ChargerAvis()
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
                        if (dataGridViewAvis.Columns.Count > 0)
                        {
                            dataGridViewAvis.Columns["IdAvis"].HeaderText = "ID";
                            dataGridViewAvis.Columns["IdAvis"].Width = 50;
                            dataGridViewAvis.Columns["IdAvis"].MinimumWidth = 50;

                            dataGridViewAvis.Columns["Note"].HeaderText = "Note";
                            dataGridViewAvis.Columns["Note"].Width = 100;
                            dataGridViewAvis.Columns["Note"].MinimumWidth = 100;

                            dataGridViewAvis.Columns["Commentaire"].HeaderText = "Commentaire";
                            dataGridViewAvis.Columns["Commentaire"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }

                        dataGridViewAvis.AutoResizeColumns();
                    }
                }
            }
        }

        private void ButtonAjouter_Click(object sender, EventArgs e)
        {
            using (Form formNouveauAvis = new Form())
            {
                formNouveauAvis.Text = "Nouvel avis";
                formNouveauAvis.Size = new Size(400, 300);
                formNouveauAvis.StartPosition = FormStartPosition.CenterParent;

                Label[] labels = new Label[]
                {
                    new Label { Text = "Note", Location = new Point(50, 50) },
                    new Label { Text = "Commentaire", Location = new Point(50, 100) }
                };

                ComboBox Note = new ComboBox { Location = new Point(200, 50) };
                for (int i = 1; i <= 5; i++)
                {
                    Note.Items.Add(i);
                }

                TextBox Commentaire = new TextBox { Location = new Point(200, 100), Size = new Size(200, 100), Multiline = true };

                Button buttonValider = new Button { Text = "Valider", Location = new Point(50, 200) };
                buttonValider.Click += (s, args) => AjouterAvis(Note.SelectedItem.ToString(), Commentaire.Text, formNouveauAvis);

                Button buttonAnnuler = new Button { Text = "Annuler", Location = new Point(200, 200) };
                buttonAnnuler.Click += (s, args) => formNouveauAvis.Close();

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

        private void ButtonModifier_Click(object sender, EventArgs e)
        {
            if (dataGridViewAvis.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewAvis.SelectedRows[0];
                int idAvis = (int)selectedRow.Cells["IdAvis"].Value;
                string commentaire = selectedRow.Cells["Commentaire"].Value.ToString();
                decimal note = (decimal)selectedRow.Cells["Note"].Value;

                using (Form formModifierAvis = new Form())
                {
                    formModifierAvis.Text = "Modifier un avis";
                    formModifierAvis.Size = new Size(400, 300);
                    formModifierAvis.StartPosition = FormStartPosition.CenterParent;

                    Label[] labels = new Label[]
                    {
                        new Label { Text = "Note", Location = new Point(50, 50) },
                        new Label { Text = "Commentaire", Location = new Point(50, 100) }
                    };

                    ComboBox Note = new ComboBox { Location = new Point(200, 50) };
                    for (int i = 1; i <= 5; i++)
                    {
                        Note.Items.Add(i);
                    }
                    Note.SelectedItem = note;

                    TextBox Commentaire = new TextBox { Location = new Point(200, 100), Size = new Size(200, 100), Multiline = true, Text = commentaire };

                    Button buttonValider = new Button { Text = "Valider", Location = new Point(50, 200) };
                    buttonValider.Click += (s, args) => ModifierAvis(idAvis, Note.SelectedItem.ToString(), Commentaire.Text, formModifierAvis);

                    Button buttonAnnuler = new Button { Text = "Annuler", Location = new Point(200, 200) };
                    buttonAnnuler.Click += (s, args) => formModifierAvis.Close();

                    formModifierAvis.Controls.AddRange(labels);
                    formModifierAvis.Controls.Add(Note);
                    formModifierAvis.Controls.Add(Commentaire);
                    formModifierAvis.Controls.Add(buttonValider);
                    formModifierAvis.Controls.Add(buttonAnnuler);

                    formModifierAvis.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un avis à modifier.");
            }
        }

        private void ModifierAvis(int idAvis, string note, string commentaire, Form form)
        {
            using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
            {
                connexion.Open();
                using (var commande = connexion.CreateCommand())
                {
                    commande.CommandText = "UPDATE Avis SET Commentaire = @Commentaire, Note = @Note WHERE IdAvis = @IdAvis";
                    commande.Parameters.AddWithValue("@Commentaire", commentaire);
                    commande.Parameters.AddWithValue("@Note", note);
                    commande.Parameters.AddWithValue("@IdAvis", idAvis);
                    commande.ExecuteNonQuery();
                }
            }
            form.Close();
            ChargerAvis();
        }

        private void ButtonSupprimer_Click(object sender, EventArgs e)
        {
            if (dataGridViewAvis.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewAvis.SelectedRows[0];
                int idAvis = (int)selectedRow.Cells["IdAvis"].Value;

                DialogResult result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cet avis ?", "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection connexion = new MySqlConnection(chaineConnexion))
                    {
                        connexion.Open();
                        using (var commande = connexion.CreateCommand())
                        {
                            commande.CommandText = "DELETE FROM Avis WHERE IdAvis = @IdAvis";
                            commande.Parameters.AddWithValue("@IdAvis", idAvis);
                            commande.ExecuteNonQuery();
                        }
                    }
                    ChargerAvis();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un avis à supprimer.");
            }
        }
    }
}
