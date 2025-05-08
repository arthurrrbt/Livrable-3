using System;
using System.Collections.Generic;
using System.Drawing;

namespace PSIV10
{
    public class AffichageGraphe
    {
        private Graphe graphe;
        private const int RayonNoeud = 25;
        private float minLongitude, maxLongitude;
        private float minLatitude, maxLatitude;
        private float echelle = 0.9f;

        public AffichageGraphe(Graphe graphe)
        {
            this.graphe = graphe;

            if (graphe.Noeuds.Count > 0)
            {
                minLongitude = float.MaxValue;
                maxLongitude = float.MinValue;
                minLatitude = float.MaxValue;
                maxLatitude = float.MinValue;

                foreach (var noeud in graphe.Noeuds)
                {
                    if (noeud.Longitude < minLongitude) minLongitude = (float)noeud.Longitude;
                    if (noeud.Longitude > maxLongitude) maxLongitude = (float)noeud.Longitude;
                    if (noeud.Latitude < minLatitude) minLatitude = (float)noeud.Latitude;
                    if (noeud.Latitude > maxLatitude) maxLatitude = (float)noeud.Latitude;
                }
            }
        }
        private List<string> lignesFermees = new List<string>();
        public void DefinirLignesFermees(List<string> lignes)
        {
            lignesFermees = lignes;
        }

        public void Dessiner(Graphics g, List<(string NomStation, string StationPre, string StationSuiv, int TempsChangement)> connexions, List<Lien> cheminDijkstra, List<Lien> cheminBellmanFord,
List<Lien> cheminFloydWarshall = null,string stationDepart = null)

        {
            List<Noeud> stations = graphe.GetStations();
            int largeur = 2500;
            int hauteur = 1100;

            foreach (var connexion in connexions)
            {
                Noeud stationActuelle = null;
                Noeud stationPre = null;
                Noeud stationSuiv = null;

                foreach (var station in stations)
                {
                    if (station.Nom == connexion.NomStation)
                    {
                        stationActuelle = station;
                        break;
                    }
                }

                if (stationActuelle == null) continue;
                if (connexion.StationPre != null)
                {
                    foreach (var station in stations)
                    {
                        if (station.Nom == connexion.StationPre)
                        {
                            stationPre = station;
                            break;
                        }
                    }
                }

                if (connexion.StationSuiv != null)
                {
                    foreach (var station in stations)
                    {
                        if (station.Nom == connexion.StationSuiv)
                        {
                            stationSuiv = station;
                            break;
                        }
                    }
                }

                PointF pActuelle = ConvertirCoordonnees(stationActuelle, largeur, hauteur);

                if (stationPre != null)
                {
                    if (lignesFermees.Contains(stationPre.Ligne))
                        continue;

                    string lignePre = "default";
                    foreach (var lien in graphe.Liens)
                    {
                        if ((lien.IdSource == stationActuelle.Id && lien.IdDestination == stationPre.Id) ||
                            (lien.IdSource == stationPre.Id && lien.IdDestination == stationActuelle.Id))
                        {
                            foreach (var noeud in graphe.Noeuds)
                            {
                                if (noeud.Id == lien.IdSource)
                                {
                                    lignePre = noeud.Ligne;
                                    break;
                                }
                            }
                            break;
                        }
                    }

                }

                if (stationSuiv != null)
                {
                    if (lignesFermees.Contains(stationSuiv.Ligne))
                        continue;

                    string ligneSuiv = "default";
                    foreach (var lien in graphe.Liens)
                    {
                        if ((lien.IdSource == stationActuelle.Id && lien.IdDestination == stationSuiv.Id) ||
                            (lien.IdSource == stationSuiv.Id && lien.IdDestination == stationActuelle.Id))
                        {
                            foreach (var noeud in graphe.Noeuds)
                            {
                                if (noeud.Id == lien.IdSource)
                                {
                                    ligneSuiv = noeud.Ligne;
                                    break;
                                }
                            }
                            break;
                        }
                    }

                    if (!lignesFermees.Contains(ligneSuiv))
                    {
                        Color couleurSuiv = GestionCouleur.GetCouleurLigne(ligneSuiv);
                        Pen penSuiv = new Pen(couleurSuiv, 2);

                        PointF pSuiv = ConvertirCoordonnees(stationSuiv, largeur, hauteur);
                        g.DrawLine(penSuiv, pActuelle, pSuiv);
                    }
                }
            }

            if (cheminDijkstra != null && cheminDijkstra.Count > 0)
            {
                Pen penChemin = new Pen(Color.Red, 6);
                penChemin.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                foreach (var lien in cheminDijkstra)
                {
                    Noeud station1 = null;
                    Noeud station2 = null;

                    foreach (var station in stations)
                    {
                        if (station.Id == lien.IdSource)
                        {
                            station1 = station;
                            if (station2 != null) break; 
                        }
                        else if (station.Id == lien.IdDestination)
                        {
                            station2 = station;
                            if (station1 != null) break; 
                        }
                    }

                    if (station1 != null && station2 != null)
                    {
                        PointF p1 = ConvertirCoordonnees(station1, largeur, hauteur);
                        PointF p2 = ConvertirCoordonnees(station2, largeur, hauteur);

                        g.DrawLine(penChemin, p1, p2);

                        using (Brush highlightBrush = new SolidBrush(Color.FromArgb(150, Color.Red)))
                        {
                            g.FillEllipse(highlightBrush,
                                p1.X - RayonNoeud,
                                p1.Y - RayonNoeud,
                                RayonNoeud * 2,
                                RayonNoeud * 2);
                            g.FillEllipse(highlightBrush,
                                p2.X - RayonNoeud,
                                p2.Y - RayonNoeud,
                                RayonNoeud * 2,
                                RayonNoeud * 2);
                        }
                    }
                }
            }

            if (cheminBellmanFord != null && cheminBellmanFord.Count > 0)
            {
                Pen penBellmanFord = new Pen(Color.Blue, 6);
                penBellmanFord.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                foreach (var lien in cheminBellmanFord)
                {
                    Noeud station1 = null;
                    Noeud station2 = null;

                    // Rechercher les stations par ID
                    foreach (var station in stations)
                    {
                        if (station.Id == lien.IdSource)
                        {
                            station1 = station;
                            if (station2 != null) break;
                        }
                        else if (station.Id == lien.IdDestination)
                        {
                            station2 = station;
                            if (station1 != null) break;
                        }
                    }

                    if (station1 != null && station2 != null)
                    {
                        PointF p1 = ConvertirCoordonnees(station1, largeur, hauteur);
                        PointF p2 = ConvertirCoordonnees(station2, largeur, hauteur);

                        g.DrawLine(penBellmanFord, p1, p2);

                        using (Brush highlightBrush = new SolidBrush(Color.FromArgb(150, Color.Blue)))
                        {
                            g.FillEllipse(highlightBrush,
                                p1.X - RayonNoeud,
                                p1.Y - RayonNoeud,
                                RayonNoeud * 2,
                                RayonNoeud * 2);
                            g.FillEllipse(highlightBrush,
                                p2.X - RayonNoeud,
                                p2.Y - RayonNoeud,
                                RayonNoeud * 2,
                                RayonNoeud * 2);
                        }
                    }
                }
            }
            if (cheminFloydWarshall != null && cheminFloydWarshall.Count > 0)
            {
                Pen penFloyd = new Pen(Color.Green, 6);
                penFloyd.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                foreach (var lien in cheminFloydWarshall)
                {
                    Noeud station1 = null;
                    Noeud station2 = null;
                    foreach (var station in graphe.Noeuds)
                    {
                        if (station.Id == lien.IdSource)
                        {
                            station1 = station;
                        }
                        if (station.Id == lien.IdDestination)
                        {
                            station2 = station;
                        }
                    }

                    if (station1 != null && station2 != null)
                    {
                        PointF p1 = ConvertirCoordonnees(station1, largeur, hauteur);
                        PointF p2 = ConvertirCoordonnees(station2, largeur, hauteur);

                        g.DrawLine(penFloyd, p1, p2);

                        using (Brush highlightBrush = new SolidBrush(Color.FromArgb(150, Color.Green)))
                        {
                            g.FillEllipse(highlightBrush, p1.X - RayonNoeud, p1.Y - RayonNoeud, RayonNoeud * 2, RayonNoeud * 2);
                            g.FillEllipse(highlightBrush, p2.X - RayonNoeud, p2.Y - RayonNoeud, RayonNoeud * 2, RayonNoeud * 2);
                        }
                    }
                }
            }


            Font font = new Font("Arial", 2, FontStyle.Bold);
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            foreach (Noeud station in stations)
            {
                PointF p = ConvertirCoordonnees(station, largeur, hauteur);
                Brush brush = new SolidBrush(station.Couleur);

                g.FillEllipse(brush, p.X - RayonNoeud / 2, p.Y - RayonNoeud / 2, RayonNoeud, RayonNoeud);
                g.DrawEllipse(Pens.Black, p.X - RayonNoeud / 2, p.Y - RayonNoeud / 2, RayonNoeud, RayonNoeud);

                RectangleF rect = new RectangleF(p.X - RayonNoeud / 2, p.Y - RayonNoeud / 2, RayonNoeud, RayonNoeud);
                g.DrawString(station.Nom, font, Brushes.White, rect, sf);
            }
        }

        private PointF ConvertirCoordonnees(Noeud station, int largeur, int hauteur)
        {
            float longitudeRange = maxLongitude - minLongitude;
            float latitudeRange = maxLatitude - minLatitude;

            if (longitudeRange == 0) longitudeRange = 1;
            if (latitudeRange == 0) latitudeRange = 1;

            float x = (float)((station.Longitude - minLongitude) / longitudeRange * largeur * echelle);
            float y = (float)((station.Latitude - minLatitude) / latitudeRange * hauteur * echelle);

            y = hauteur * echelle - y;

            float offsetX = (largeur * (1 - echelle)) / 2;
            float offsetY = (hauteur * (1 - echelle)) / 2;

            return new PointF(x + offsetX, y + offsetY);
        }
    }
}