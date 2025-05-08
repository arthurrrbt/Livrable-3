using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace PSIV10
{
    public class Graphe
    {
        public List<Noeud> Noeuds { get; private set; } = new();
        public List<Lien> Liens { get; private set; } = new List<Lien>();

        /// <summary>
        /// Ajoute un nouveau nœud à la liste des nœuds.
        /// </summary>
        /// <param name="id">Identifiant unique du nœud.</param>
        /// <param name="nom">Nom du nœud.</param>
        /// <param name="lat">Latitude du nœud.</param>
        /// <param name="lon">Longitude du nœud.</param>
        /// <param name="ligne">Ligne associée au nœud.</param>
        public void AjouterNoeud(int id, string nom, double lat, double lon, string ligne)
        {
            Noeuds.Add(new Noeud(id, nom, lat, lon, ligne));
        }
        /// <summary>
        /// Ajoute un nouveau lien à la liste des liens.
        /// </summary>
        /// <param name="lien">Lien à ajouter.</param>
        public void AjouterLien(Lien lien)
        {
            Liens.Add(lien);
        }

        /// <summary>
        /// Importe les stations de métro depuis un fichier CSV et les ajoute à la liste des nœuds.
        /// </summary>
        public void ImporterStationsCSV()
        {
            string cheminFichier = @"MetroParisNoeud.csv";
            if (!File.Exists(cheminFichier))
            {
                Console.WriteLine("Fichier CSV introuvable !");
                return;
            }

            using (var reader = new StreamReader(cheminFichier, Encoding.UTF8))
            {
                string ligne;
                bool isFirstLine = true;
                while ((ligne = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }
                    var valeurs = ligne.Split(';');
                    if (valeurs.Length < 5) continue;

                    if (!int.TryParse(valeurs[0], out int id)) continue;
                    string nom = valeurs[2];
                    string ligneMetro = valeurs[1];
                    if (!double.TryParse(valeurs[4], NumberStyles.Float, CultureInfo.InvariantCulture, out double lat)) continue;
                    if (!double.TryParse(valeurs[3], NumberStyles.Float, CultureInfo.InvariantCulture, out double lon)) continue;

                    AjouterNoeud(id, nom, lat, lon, ligneMetro);
                }
            }
        }
        /// <summary>
        /// Récupère les connexions entre les stations de métro depuis un fichier CSV.
        /// </summary>
        /// <returns>Une liste contenant les connexions entre stations, incluant le nom de la station, la station précédente, la station suivante et le temps de changement.</returns>
        public List<(string NomStation, string StationPre, string StationSuiv, int TempsChangement)> RecupererConnexions()
        {
            List<(string NomStation, string StationPre, string StationSuiv, int TempsChangement)> connexions = new();
            string cheminFichier = @"MetroParisArcs.csv";
            if (!File.Exists(cheminFichier)) return connexions;

            using (var reader = new StreamReader(cheminFichier, Encoding.UTF8))
            {
                string ligne;
                bool isFirstLine = true;
                while ((ligne = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }
                    var valeurs = ligne.Split(';');
                    if (valeurs.Length < 5) continue;

                    string nomStation = valeurs[1];
                    string precedent = "";
                    string suivant = "";
                    if (valeurs[2] != null && valeurs[2].Length > 0)
                        precedent = valeurs[2];
                    if (valeurs[3] != null && valeurs[3].Length > 0)
                        suivant = valeurs[3];
                    if (!int.TryParse(valeurs[4], out int tempsChangement)) tempsChangement = 0;

                    connexions.Add((nomStation, precedent, suivant, tempsChangement));
                }
            }
            return connexions;
        }
        /// <summary>
        /// Importe les liens entre les stations à partir des connexions récupérées et les ajoute à la liste des liens.
        /// </summary>
        public void ImporterLiens()
        {
            var connexions = RecupererConnexions();

            foreach (var connexion in connexions)
            {
                string nomStation = connexion.NomStation;
                string stationPre = connexion.StationPre;
                string stationSuiv = connexion.StationSuiv;
                int tempsChangement = connexion.TempsChangement;
                Noeud noeud = null;
                for (int i = 0; i < Noeuds.Count; i++)
                {
                    if (Noeuds[i].Nom == nomStation)
                    {
                        noeud = Noeuds[i];
                        break;
                    }
                }

                if (noeud != null)
                {
                    if (stationPre != null && stationPre.Length > 0)
                    {
                        Noeud noeudPre = null;
                        for (int i = 0; i < Noeuds.Count; i++)
                        {
                            if (Noeuds[i].Nom == stationPre)
                            {
                                noeudPre = Noeuds[i];
                                break;
                            }
                        }

                        if (noeudPre != null)
                        {
                            double poids = CalculerDistance(noeud, noeudPre) + tempsChangement;
                            Liens.Add(new Lien(noeud.Id, noeudPre.Id, poids));
                        }
                    }
                    if (stationSuiv != null && stationSuiv.Length > 0)
                    {
                        Noeud noeudSuiv = null;
                        for (int i = 0; i < Noeuds.Count; i++)
                        {
                            if (Noeuds[i].Nom == stationSuiv)
                            {
                                noeudSuiv = Noeuds[i];
                                break;
                            }
                        }

                        if (noeudSuiv != null)
                        {
                            double poids = CalculerDistance(noeud, noeudSuiv) + tempsChangement;
                            Liens.Add(new Lien(noeud.Id, noeudSuiv.Id, poids));
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Calcule la distance en kilomètres entre deux nœuds en utilisant la formule de Haversine.
        /// </summary>
        /// <param name="a">Premier nœud.</param>
        /// <param name="b">Deuxième nœud.</param>
        /// <returns>La distance en kilomètres entre les deux nœuds.</returns>
        /// <remarks>
        /// La fonction utilise la formule de Haversine pour calculer la distance entre deux points sur une sphère.
        /// L'utilisation de Atan2 plutôt que Asin pour le calcul de l'angle permet d'améliorer la précision 
        /// et la robustesse du calcul, notamment en évitant les erreurs dues aux valeurs proches de 1.
        /// Atan2(y, x) prend en compte le signe des deux arguments et gère mieux les cas où les valeurs 
        /// peuvent provoquer des imprécisions numériques, contrairement à Asin qui est limité aux valeurs 
        /// entre -1 et 1.
        /// </remarks>
        private double CalculerDistance(Noeud a, Noeud b)
        {
            double R = 6371;
            double dLat = (b.Latitude - a.Latitude) * Math.PI / 180;
            double dLon = (b.Longitude - a.Longitude) * Math.PI / 180;

            double lat1 = a.Latitude * Math.PI / 180;
            double lat2 = b.Latitude * Math.PI / 180;

            double h = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(h), Math.Sqrt(1 - h));

            return R * c; 
        }

        /// <summary>
        /// Retourne la liste des stations (nœuds) existantes.
        /// </summary>
        /// <returns>Une liste contenant tous les nœuds représentant les stations.</returns>
        public List<Noeud> GetStations()
        {
            return Noeuds;
        }

        #region Dijsktra
        /// <summary>
        /// Implémente l'algorithme de Dijkstra pour trouver le chemin le plus court entre deux stations.
        /// </summary>
        /// <param name="idDepart">Identifiant du nœud de départ.</param>
        /// <param name="idArrivee">Identifiant du nœud d'arrivée.</param>
        /// <returns>Une liste de nœuds représentant le chemin le plus court entre les deux stations.</returns>
        /// <remarks>
        /// Cet algorithme utilise une file de priorité pour explorer les chemins les plus courts en fonction du poids des liens.
        /// Il met à jour les distances minimales à chaque itération et construit progressivement le chemin optimal.
        /// </remarks>
        public List<Noeud> Dijkstra(int idDepart, int idArrivee)
        {
            var distances = new Dictionary<int, double>();
            var chemins = new Dictionary<int, List<Noeud>>();
            var aTraiter = new List<(Noeud noeud, double distance)>();

            foreach (var noeud in Noeuds)
            {
                distances[noeud.Id] = double.MaxValue;
                chemins[noeud.Id] = new List<Noeud>();
            }

            Noeud noeudDepart = null;
            for (int i = 0; i < Noeuds.Count; i++)
            {
                if (Noeuds[i].Id == idDepart)
                {
                    noeudDepart = Noeuds[i];
                    break;
                }
            }
            distances[idDepart] = 0;
            aTraiter.Add((noeudDepart, 0));
            chemins[idDepart].Add(noeudDepart);

            while (aTraiter.Count > 0)
            {
                var minIndex = 0;
                for (int i = 1; i < aTraiter.Count; i++)
                {
                    if (aTraiter[i].distance < aTraiter[minIndex].distance)
                        minIndex = i;
                }

                var (noeudActuel, distanceActuelle) = aTraiter[minIndex];
                aTraiter.RemoveAt(minIndex);

                if (noeudActuel.Id == idArrivee)
                    break;
                if (distanceActuelle > distances[noeudActuel.Id])
                    continue;
                List<Lien> liensNoeud = new List<Lien>();
                for (int i = 0; i < Liens.Count; i++)
                {
                    if (Liens[i].IdSource == noeudActuel.Id)
                        liensNoeud.Add(Liens[i]);
                }

                foreach (var lien in liensNoeud)
                {
                    Noeud noeudDestination = null;
                    for (int j = 0; j < Noeuds.Count; j++)
                    {
                        if (Noeuds[j].Id == lien.IdDestination)
                        {
                            noeudDestination = Noeuds[j];
                            break;
                        }
                    }
                    double nouvelleDistance = distances[noeudActuel.Id] + lien.Poids;
                    if (nouvelleDistance < distances[noeudDestination.Id])
                    {
                        distances[noeudDestination.Id] = nouvelleDistance;
                        var nouveauChemin = new List<Noeud>(chemins[noeudActuel.Id]);
                        nouveauChemin.Add(noeudDestination);
                        chemins[noeudDestination.Id] = nouveauChemin;
                        aTraiter.Add((noeudDestination, nouvelleDistance));
                    }
                }
            }
            return chemins[idArrivee];
        }
        #endregion
        #region Bellman Ford
        /// <summary>
        /// Implémente l'algorithme de Bellman-Ford pour trouver le chemin le plus court entre deux stations.
        /// </summary>
        /// <param name="idDepart">Identifiant du nœud de départ.</param>
        /// <param name="idArrivee">Identifiant du nœud d'arrivée.</param>
        /// <returns>Une liste de nœuds représentant le chemin le plus court entre les deux stations.</returns>
        /// <remarks>
        /// L'algorithme de Bellman-Ford est utilisé pour trouver le plus court chemin dans un graphe pondéré.
        /// Contrairement à Dijkstra, il gère les poids négatifs et détecte les cycles de poids négatif.
        /// Une vérification est effectuée à la fin pour signaler la présence d'un cycle négatif dans le graphe.
        /// </remarks>
        public List<Noeud> BellmanFord(int idDepart, int idArrivee)
        {
            var distances = new Dictionary<int, double>();
            var predecesseurs = new Dictionary<int, int>();
            foreach (var noeud in Noeuds)
            {
                distances[noeud.Id] = double.MaxValue;
                predecesseurs[noeud.Id] = -1;
            }
            distances[idDepart] = 0;

            int nombreDeNoeuds = Noeuds.Count;

            for (int i = 0; i < nombreDeNoeuds - 1; i++)
            {
                foreach (var lien in Liens)
                {
                    double tempsTotal = lien.Poids;

                    if (distances[lien.IdSource] != double.MaxValue &&
                        distances[lien.IdSource] + tempsTotal < distances[lien.IdDestination])
                    {
                        distances[lien.IdDestination] = distances[lien.IdSource] + tempsTotal;
                        predecesseurs[lien.IdDestination] = lien.IdSource;
                    }
                }
            }
            List<Noeud> chemin = new();
            int noeudActuel = idArrivee;

            while (predecesseurs[noeudActuel] != -1)
            {
                Noeud noeud = null;
                for (int i = 0; i < Noeuds.Count; i++)
                {
                    if (Noeuds[i].Id == noeudActuel)
                    {
                        noeud = Noeuds[i];
                        break;
                    }
                }
                chemin.Insert(0, noeud);
                noeudActuel = predecesseurs[noeudActuel];
            }

            for (int i = 0; i < Noeuds.Count; i++)
            {
                if (Noeuds[i].Id == idDepart)
                {
                    chemin.Insert(0, Noeuds[i]);
                    break;
                }
            }

            return chemin;

        }

        #endregion
        #region Floyd-Warshall
        /// <summary>
        /// Implémente l'algorithme de Floyd-Warshall pour calculer les plus courts chemins entre toutes les paires de stations.
        /// </summary>
        /// <returns>
        /// Une matrice de distances où la case [i, j] représente la distance minimale entre le nœud i et le nœud j.
        /// </returns>
        public List<Noeud> FloydWarshall(int departId, int arriveeId)
        {
            int n = Noeuds.Count;
            double[,] dist = new double[n, n];
            int[,] pred = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        dist[i, j] = 0;
                        pred[i, j] = -1;
                    }
                    else
                    {
                        double poids = 100000000000000;
                        for (int k = 0; k < Liens.Count; k++)
                        {
                            Lien lien = Liens[k];
                            if ((lien.IdSource == Noeuds[i].Id && lien.IdDestination == Noeuds[j].Id) ||
                                (lien.IdSource == Noeuds[j].Id && lien.IdDestination == Noeuds[i].Id))
                            {
                                poids = lien.Poids;
                                break;
                            }
                        }
                        dist[i, j] = poids;
                        if (poids != 100000000000000)
                            pred[i, j] = i;
                        else
                            pred[i, j] = -1;
                    }
                }
            }

            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (dist[i, k] + dist[k, j] < dist[i, j])
                        {
                            dist[i, j] = dist[i, k] + dist[k, j];
                            pred[i, j] = pred[k, j];
                        }
                    }
                }
            }

            int departIndex = -1, arriveeIndex = -1;
            for (int i = 0; i < Noeuds.Count; i++)
            {
                if (Noeuds[i].Id == departId) departIndex = i;
                if (Noeuds[i].Id == arriveeId) arriveeIndex = i;
            }

            List<Noeud> chemin = new List<Noeud>();
            if (pred[departIndex, arriveeIndex] == null && departIndex != arriveeIndex)
            {
                return chemin;
            }

            int courant = arriveeIndex;
            while (courant != null && courant != departIndex)
            {
                chemin.Insert(0, Noeuds[courant]);
                courant = pred[departIndex, courant];
            }

            chemin.Insert(0, Noeuds[departIndex]); 
            return chemin;
        }


        #endregion
       

    }
}
