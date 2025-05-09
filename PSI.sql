-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: localhost    Database: psi
-- ------------------------------------------------------
-- Server version	8.0.41

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `admin`
--

DROP TABLE IF EXISTS `admin`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `admin` (
  `IdAdmin` int NOT NULL AUTO_INCREMENT,
  `IdParticulier` int DEFAULT NULL,
  `Nom` varchar(50) NOT NULL,
  `Prenom` varchar(50) NOT NULL,
  `Mail` varchar(255) NOT NULL,
  `Adresse` varchar(50) NOT NULL,
  `NumTel` varchar(20) NOT NULL,
  `Mdp` varchar(255) NOT NULL,
  `MetroProche` varchar(50) NOT NULL,
  PRIMARY KEY (`IdAdmin`),
  UNIQUE KEY `IdParticulier` (`IdParticulier`),
  CONSTRAINT `admin_ibfk_1` FOREIGN KEY (`IdParticulier`) REFERENCES `particulier` (`IdParticulier`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `admin`
--

LOCK TABLES `admin` WRITE;
/*!40000 ALTER TABLE `admin` DISABLE KEYS */;
INSERT INTO `admin` VALUES (1,1,'root','Admin','root@test.com','Rue de Paris','0612345678','root','MetroProche');
/*!40000 ALTER TABLE `admin` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `adresse`
--

DROP TABLE IF EXISTS `adresse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `adresse` (
  `IdAdresse` int NOT NULL AUTO_INCREMENT,
  `Rue` varchar(255) NOT NULL DEFAULT 'Inconnu',
  `Ville` varchar(100) NOT NULL DEFAULT 'Paris',
  `CodePostal` varchar(10) NOT NULL,
  `Pays` varchar(50) NOT NULL DEFAULT 'France',
  PRIMARY KEY (`IdAdresse`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `adresse`
--

LOCK TABLES `adresse` WRITE;
/*!40000 ALTER TABLE `adresse` DISABLE KEYS */;
INSERT INTO `adresse` VALUES (1,'Rue de Paris','Paris','75001','France'),(2,'Rue de la paix','Paris','75002','France'),(3,'1 Rue de la Paix','Paris','75001','France'),(4,'2 Avenue des Champs-Élysées','Paris','75008','France'),(5,'3 Boulevard Haussmann','Paris','75009','France'),(6,'4 Rue Saint-Honoré','Paris','75001','France'),(7,'5 Place de la Bastille','Paris','75004','France'),(8,'6 Avenue de l\'Opéra','Paris','75002','France'),(9,'7 Rue de Rennes','Paris','75006','France'),(10,'8 Boulevard Saint-Michel','Paris','75005','France'),(11,'9 Place Vendôme','Paris','75001','France'),(12,'10 Rue La Fayette','Paris','75010','France'),(13,'15 Rue des Commandes','Paris','75020','France'),(14,'12 Rue Montorgueil','Paris','75002','France'),(15,'25 Avenue Montaigne','Paris','75008','France'),(16,'8 Rue des Rosiers','Paris','75004','France'),(17,'17 Rue Oberkampf','Paris','75011','France'),(18,'33 Rue de Belleville','Paris','75019','France'),(19,'5 Rue de Charonne','Paris','75011','France'),(20,'41 Rue des Martyrs','Paris','75009','France'),(21,'14 Rue du Commerce','Paris','75015','France'),(22,'23 Rue Daguerre','Paris','75014','France'),(23,'9 Rue de la Roquette','Paris','75011','France');
/*!40000 ALTER TABLE `adresse` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `avis`
--

DROP TABLE IF EXISTS `avis`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `avis` (
  `IdAvis` int NOT NULL AUTO_INCREMENT,
  `Commentaire` varchar(255) NOT NULL,
  `Note` decimal(2,1) NOT NULL,
  PRIMARY KEY (`IdAvis`),
  CONSTRAINT `avis_chk_1` CHECK (((`Note` >= 0) and (`Note` <= 5)))
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `avis`
--

LOCK TABLES `avis` WRITE;
/*!40000 ALTER TABLE `avis` DISABLE KEYS */;
INSERT INTO `avis` VALUES (1,'Livraison impeccable',1.0),(2,'Excellent',5.0),(3,'Je recommande !',4.0),(4,'Les plats n\'étaient pas assez chauds',2.0);
/*!40000 ALTER TABLE `avis` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client` (
  `IdClient` int NOT NULL AUTO_INCREMENT,
  `IdParticulier` int DEFAULT NULL,
  `IdEntreprise` int DEFAULT NULL,
  `IdAdresse` int DEFAULT NULL,
  PRIMARY KEY (`IdClient`),
  UNIQUE KEY `IdParticulier` (`IdParticulier`),
  UNIQUE KEY `IdEntreprise` (`IdEntreprise`),
  UNIQUE KEY `IdAdresse` (`IdAdresse`),
  CONSTRAINT `client_ibfk_1` FOREIGN KEY (`IdParticulier`) REFERENCES `particulier` (`IdParticulier`) ON DELETE SET NULL,
  CONSTRAINT `client_ibfk_2` FOREIGN KEY (`IdEntreprise`) REFERENCES `entreprise_locale` (`IdEntreprise`) ON DELETE SET NULL,
  CONSTRAINT `client_ibfk_3` FOREIGN KEY (`IdAdresse`) REFERENCES `adresse` (`IdAdresse`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES (1,1,1,1),(2,2,NULL,2),(3,5,NULL,3),(4,6,NULL,4),(5,7,NULL,5),(6,8,NULL,6),(7,9,NULL,7),(8,10,NULL,8),(9,11,NULL,9),(10,12,NULL,10),(11,13,NULL,11),(12,14,NULL,12);
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commande`
--

DROP TABLE IF EXISTS `commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `commande` (
  `IdCommande` int NOT NULL AUTO_INCREMENT,
  `DateCommande` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `MoyenPaiement` varchar(50) NOT NULL,
  `CodePromoUtilise` varchar(20) DEFAULT NULL,
  `Reduction` decimal(5,2) DEFAULT NULL,
  `IdAdresse` int NOT NULL,
  `IdClient` int NOT NULL,
  `IdCuisinier` int DEFAULT NULL,
  `IdPlat` int NOT NULL,
  `Nom` varchar(100) DEFAULT NULL,
  `Prix` decimal(10,2) DEFAULT NULL,
  `Quantite` int DEFAULT NULL,
  `TypePlat` varchar(50) DEFAULT NULL,
  `DateFab` date DEFAULT NULL,
  `DatePer` date DEFAULT NULL,
  `Regime` varchar(50) DEFAULT NULL,
  `Nature` varchar(50) DEFAULT NULL,
  `Ingredient1` varchar(50) DEFAULT NULL,
  `Volume1` varchar(20) DEFAULT NULL,
  `Ingredient2` varchar(50) DEFAULT NULL,
  `Volume2` varchar(20) DEFAULT NULL,
  `Ingredient3` varchar(50) DEFAULT NULL,
  `Volume3` varchar(20) DEFAULT NULL,
  `Ingredient4` varchar(50) DEFAULT NULL,
  `Volume4` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`IdCommande`),
  KEY `IdAdresse` (`IdAdresse`),
  KEY `IdClient` (`IdClient`),
  KEY `IdCuisinier` (`IdCuisinier`),
  KEY `IdPlat` (`IdPlat`),
  CONSTRAINT `commande_ibfk_1` FOREIGN KEY (`IdAdresse`) REFERENCES `adresse` (`IdAdresse`) ON DELETE CASCADE,
  CONSTRAINT `commande_ibfk_2` FOREIGN KEY (`IdClient`) REFERENCES `client` (`IdClient`) ON DELETE CASCADE,
  CONSTRAINT `commande_ibfk_3` FOREIGN KEY (`IdCuisinier`) REFERENCES `cuisinier` (`IdCuisinier`),
  CONSTRAINT `commande_ibfk_4` FOREIGN KEY (`IdPlat`) REFERENCES `plat` (`IdPlat`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commande`
--

LOCK TABLES `commande` WRITE;
/*!40000 ALTER TABLE `commande` DISABLE KEYS */;
INSERT INTO `commande` VALUES (1,'2025-03-03 00:00:00','Carte Bancaire','',0.00,13,1,15,1,'Lasagnes Végétariennes',16.00,2,'Plat Principal','2025-03-02','2025-03-06','Végétarien','Italienne','Pâtes','150g','Légumes grillés','200g','Béchamel','100g','Fromage râpé','50g'),(2,'2025-03-05 00:00:00','Espèces','',0.00,13,1,16,1,'Tiramisu Classique',6.00,1,'Dessert','2025-03-04','2025-03-08','Standard','Italienne','Mascarpone','100g','Biscuits','80g','Café','50ml','Cacao','10g'),(3,'2025-04-20 00:00:00','Carte Bancaire','',0.00,13,5,21,1,'Bruschetta Tomate-Basilic',7.00,3,'Entrée','2025-04-20','2025-04-23','Végétarien','Italienne','Pain ciabatta','100g','Tomates','150g','Basilic frais','20g','Huile d\'olive','15ml'),(4,'2025-04-20 00:00:00','PayPal','',0.00,13,12,17,7,'Boeuf Bourguignon',20.00,2,'Plat Principal','2025-04-20','2025-04-24','Standard','Française','Boeuf','300g','Carottes','100g','Vin rouge','150ml','Champignons','80g'),(5,'2025-04-20 00:00:00','Espèces','',0.00,13,11,14,10,'Tiramisu Classique',6.00,4,'Dessert','2025-04-20','2025-04-22','Standard','Italienne','Mascarpone','200g','Café','100ml','Biscuits','120g','Cacao','15g'),(6,'2025-04-20 00:00:00','Carte Bancaire','',0.00,13,7,20,8,'Risotto aux Champignons',17.00,2,'Plat Principal','2025-04-20','2025-04-23','Végane','Italienne','Riz arborio','200g','Champignons','150g','Vin blanc','50ml','Bouillon légumes','400ml'),(7,'2025-04-20 00:00:00','Virement bancaire','',0.00,13,11,22,11,'Mousse au Chocolat',5.00,3,'Dessert','2025-04-20','2025-04-22','Végétarien','Française','Chocolat noir','150g','Œufs','4p','Sucre','50g','Crème','50ml'),(8,'2025-05-08 23:19:07','Carte','',0.00,2,2,3,5,NULL,140.00,10,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cuisinier`
--

DROP TABLE IF EXISTS `cuisinier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cuisinier` (
  `IdCuisinier` int NOT NULL AUTO_INCREMENT,
  `IdParticulier` int NOT NULL,
  `Nom` varchar(50) NOT NULL,
  `Prenom` varchar(50) NOT NULL,
  `Rue` varchar(255) DEFAULT NULL,
  `Numero` int DEFAULT NULL,
  `CodePostal` varchar(10) DEFAULT NULL,
  `Ville` varchar(50) DEFAULT 'Paris',
  `Tel` varchar(20) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `MetroProche` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`IdCuisinier`),
  UNIQUE KEY `IdParticulier` (`IdParticulier`),
  CONSTRAINT `cuisinier_ibfk_1` FOREIGN KEY (`IdParticulier`) REFERENCES `particulier` (`IdParticulier`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cuisinier`
--

LOCK TABLES `cuisinier` WRITE;
/*!40000 ALTER TABLE `cuisinier` DISABLE KEYS */;
INSERT INTO `cuisinier` VALUES (1,1,'root','Admin','Rue de Paris',15,'75001','Paris','0612345678','root@test.com','Châtelet'),(2,3,'chef','Michel','Rue de la Gastronomie',23,'75000','Paris','0611223344','chef.michel@cuisine.fr','Bastille'),(3,4,'Dupond','Marie','Rue de la République',30,'75011','Paris','1234567890','dupondmarie@gmail.com','République'),(4,15,'Alice','Dupont','Rue de la Cuisine',12,'75000','Paris','0612345678','alice.dupont@cuisine.fr','Châtelet'),(5,16,'Bob','Martin','Avenue des Chefs',15,'75000','Paris','0623456789','bob.martin@cuisine.fr','Opéra'),(6,17,'Charlie','Durand','Boulevard des Saveurs',18,'75000','Paris','0634567890','charlie.durand@cuisine.fr','République'),(7,18,'David','Lefebvre','Rue des Gourmets',21,'75000','Paris','0645678901','david.lefebvre@cuisine.fr','Montparnasse'),(8,19,'Eve','Rousseau','Allée des Épices',24,'75000','Paris','0656789012','eve.rousseau@cuisine.fr','Nation'),(9,20,'Frank','Morel','Impasse des Chefs',27,'75000','Paris','0667890123','frank.morel@cuisine.fr','Bastille'),(10,21,'Grace','Noël','Passage des Saveurs',30,'75000','Paris','0678901234','grace.noel@cuisine.fr','Concorde'),(11,22,'Hector','Blanc','Ruelle des Plats',33,'75000','Paris','0689012345','hector.blanc@cuisine.fr','Invalides'),(12,23,'Isabelle','Petit','Boulevard des Chefs',36,'75000','Paris','0690123456','isabelle.petit@cuisine.fr','Saint-Lazare'),(13,24,'Jack','Grand','Avenue des Gourmets',39,'75000','Paris','0601234567','jack.grand@cuisine.fr','Gare de Lyon'),(14,35,'Roux','Mathilde','Rue du Faubourg Saint-Antoine',28,'75012','Paris','0601112233','mathilde.roux@cuisine.fr','Ledru-Rollin'),(15,36,'Bonnet','Julien','Rue Saint-Maur',19,'75011','Paris','0602223344','julien.bonnet@cuisine.fr','Rue Saint-Maur'),(16,37,'Lemoine','Claire','Rue Montmartre',42,'75002','Paris','0603334455','claire.lemoine@cuisine.fr','Grands Boulevards'),(17,38,'Garnier','Vincent','Rue de Buci',11,'75006','Paris','0604445566','vincent.garnier@cuisine.fr','Odéon'),(18,39,'Faure','Chloé','Rue des Petits Champs',31,'75001','Paris','0605556677','chloe.faure@cuisine.fr','Bourse'),(19,40,'Simon','Alexandre','Rue de Charenton',7,'75012','Paris','0606667788','alexandre.simon@cuisine.fr','Gare de Lyon'),(20,41,'Fournier','Aurélie','Rue des Archives',16,'75004','Paris','0607778899','aurelie.fournier@cuisine.fr','Rambuteau'),(21,42,'Vidal','Guillaume','Rue de la Pompe',29,'75016','Paris','0608889900','guillaume.vidal@cuisine.fr','Rue de la Pompe'),(22,43,'Morin','Pauline','Rue de Passy',38,'75016','Paris','0609990011','pauline.morin@cuisine.fr','Passy'),(23,44,'Lefevre','Romain','Rue de Vaugirard',22,'75015','Paris','0600112233','romain.lefevre@cuisine.fr','Convention');
/*!40000 ALTER TABLE `cuisinier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `entreprise_locale`
--

DROP TABLE IF EXISTS `entreprise_locale`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `entreprise_locale` (
  `IdEntreprise` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(100) NOT NULL,
  `NomReferent` varchar(100) NOT NULL,
  PRIMARY KEY (`IdEntreprise`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `entreprise_locale`
--

LOCK TABLES `entreprise_locale` WRITE;
/*!40000 ALTER TABLE `entreprise_locale` DISABLE KEYS */;
INSERT INTO `entreprise_locale` VALUES (1,'Durand','Marco');
/*!40000 ALTER TABLE `entreprise_locale` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fidelite_client`
--

DROP TABLE IF EXISTS `fidelite_client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fidelite_client` (
  `IdFidelite` int NOT NULL AUTO_INCREMENT,
  `IdClient` int NOT NULL,
  `PointsFidelite` int NOT NULL DEFAULT '0',
  `PointsUtilises` int NOT NULL DEFAULT '0',
  `DateDerniereMaj` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`IdFidelite`),
  UNIQUE KEY `UC_Client` (`IdClient`),
  CONSTRAINT `fidelite_client_ibfk_1` FOREIGN KEY (`IdClient`) REFERENCES `client` (`IdClient`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fidelite_client`
--

LOCK TABLES `fidelite_client` WRITE;
/*!40000 ALTER TABLE `fidelite_client` DISABLE KEYS */;
INSERT INTO `fidelite_client` VALUES (1,2,14,0,'2025-05-08 23:19:08');
/*!40000 ALTER TABLE `fidelite_client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `livraison`
--

DROP TABLE IF EXISTS `livraison`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livraison` (
  `IdLivraison` int NOT NULL AUTO_INCREMENT,
  `PositionLive` varchar(100) NOT NULL,
  `IdAvis` int NOT NULL,
  `IdNotification` int NOT NULL,
  `IdAdresse` int NOT NULL,
  `IdCommande` int NOT NULL,
  PRIMARY KEY (`IdLivraison`),
  KEY `IdAvis` (`IdAvis`),
  KEY `IdNotification` (`IdNotification`),
  KEY `IdAdresse` (`IdAdresse`),
  KEY `IdCommande` (`IdCommande`),
  CONSTRAINT `livraison_ibfk_1` FOREIGN KEY (`IdAvis`) REFERENCES `avis` (`IdAvis`) ON DELETE CASCADE,
  CONSTRAINT `livraison_ibfk_2` FOREIGN KEY (`IdNotification`) REFERENCES `notification` (`IdNotification`) ON DELETE CASCADE,
  CONSTRAINT `livraison_ibfk_3` FOREIGN KEY (`IdAdresse`) REFERENCES `adresse` (`IdAdresse`) ON DELETE CASCADE,
  CONSTRAINT `livraison_ibfk_4` FOREIGN KEY (`IdCommande`) REFERENCES `commande` (`IdCommande`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `livraison`
--

LOCK TABLES `livraison` WRITE;
/*!40000 ALTER TABLE `livraison` DISABLE KEYS */;
/*!40000 ALTER TABLE `livraison` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notification`
--

DROP TABLE IF EXISTS `notification`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notification` (
  `IdNotification` int NOT NULL AUTO_INCREMENT,
  `Message` text NOT NULL,
  `MoyenEnvoi` varchar(50) NOT NULL,
  `DateHeure` datetime NOT NULL,
  `Statut` varchar(50) NOT NULL,
  PRIMARY KEY (`IdNotification`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notification`
--

LOCK TABLES `notification` WRITE;
/*!40000 ALTER TABLE `notification` DISABLE KEYS */;
INSERT INTO `notification` VALUES (1,'Votre commande est en cours de préparation','Email','2025-05-07 11:04:32','Envoyé'),(2,'Votre commande est en cours de livraison','SMS','2025-05-07 11:04:32','Envoyé'),(3,'Votre commande est en cours de préparation. Notre chef s\'en occupe !','Application','2025-05-08 23:19:08','Non lu');
/*!40000 ALTER TABLE `notification` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `particulier`
--

DROP TABLE IF EXISTS `particulier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `particulier` (
  `IdParticulier` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(50) NOT NULL,
  `Prenom` varchar(50) NOT NULL,
  `Mail` varchar(255) NOT NULL,
  `Adresse` varchar(50) NOT NULL,
  `NumTel` varchar(20) NOT NULL,
  `Mdp` varchar(255) NOT NULL,
  `MetroProche` varchar(50) NOT NULL,
  PRIMARY KEY (`IdParticulier`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `particulier`
--

LOCK TABLES `particulier` WRITE;
/*!40000 ALTER TABLE `particulier` DISABLE KEYS */;
INSERT INTO `particulier` VALUES (1,'root','Admin','root@test.com','Rue de Paris','0612345678','root','Porte Maillot'),(2,'testclient','Utilisateur','test@example.com','Rue de la paix','0612345678','motdepasse','test'),(3,'chef','Michel','chef.michel@cuisine.fr','23 Rue de la Gastronomie, Paris','0611223344','123456','Argentine'),(4,'Dupond','Marie','dupondmarie@test.com','Rue de la République','1234567890','chef','Gare de Lyon'),(5,'Client1','Prenom1','client1@example.com','1 Rue de la Paix','0611111111','motdepasse','Opéra'),(6,'Client2','Prenom2','client2@example.com','2 Avenue des Champs-Élysées','0622222222','motdepasse','George V'),(7,'Client3','Prenom3','client3@example.com','3 Boulevard Haussmann','0633333333','motdepasse','Havre-Caumartin'),(8,'Client4','Prenom4','client4@example.com','4 Rue Saint-Honoré','0644444444','motdepasse','Palais Royal - Musée du Louvre'),(9,'Client5','Prenom5','client5@example.com','5 Place de la Bastille','0655555555','motdepasse','Bastille'),(10,'Client6','Prenom6','client6@example.com','6 Avenue de l\'Opéra','0666666666','motdepasse','Pyramides'),(11,'Client7','Prenom7','client7@example.com','7 Rue de Rennes','0677777777','motdepasse','Saint-Sulpice'),(12,'Client8','Prenom8','client8@example.com','8 Boulevard Saint-Michel','0688888888','motdepasse','Luxembourg'),(13,'Client9','Prenom9','client9@example.com','9 Place Vendôme','0699999999','motdepasse','Tuileries'),(14,'Client10','Prenom10','client10@example.com','10 Rue La Fayette','0600000000','motdepasse','Le Peletier'),(15,'Moreau','Camille','camille.moreau@email.com','12 Rue Montorgueil','0701234567','motdepasse','Étienne Marcel'),(16,'Lambert','Thomas','thomas.lambert@email.com','25 Avenue Montaigne','0712345678','motdepasse','Alma-Marceau'),(17,'Dubois','Sophie','sophie.dubois@email.com','8 Rue des Rosiers','0723456789','motdepasse','Saint-Paul'),(18,'Leroy','Antoine','antoine.leroy@email.com','17 Rue Oberkampf','0734567890','motdepasse','Parmentier'),(19,'Legrand','Émilie','emilie.legrand@email.com','33 Rue de Belleville','0745678901','motdepasse','Pyrénées'),(20,'Mercier','Lucas','lucas.mercier@email.com','5 Rue de Charonne','0756789012','motdepasse','Ledru-Rollin'),(21,'Fontaine','Julie','julie.fontaine@email.com','41 Rue des Martyrs','0767890123','motdepasse','Pigalle'),(22,'Brun','Nicolas','nicolas.brun@email.com','14 Rue du Commerce','0778901234','motdepasse','Commerce'),(23,'Perrin','Léa','lea.perrin@email.com','23 Rue Daguerre','0789012345','motdepasse','Denfert-Rochereau'),(24,'Girard','Maxime','maxime.girard@email.com','9 Rue de la Roquette','0790123456','motdepasse','Voltaire'),(25,'Alice','Dupont','alice.dupont@cuisine.fr','12 Rue de la Cuisine, Paris','0612345678','123456','Châtelet'),(26,'Bob','Martin','bob.martin@cuisine.fr','15 Avenue des Chefs, Paris','0623456789','123456','Opéra'),(27,'Charlie','Durand','charlie.durand@cuisine.fr','18 Boulevard des Saveurs, Paris','0634567890','123456','République'),(28,'David','Lefebvre','david.lefebvre@cuisine.fr','21 Rue des Gourmets, Paris','0645678901','123456','Montparnasse'),(29,'Eve','Rousseau','eve.rousseau@cuisine.fr','24 Allée des Épices, Paris','0656789012','123456','Nation'),(30,'Frank','Morel','frank.morel@cuisine.fr','27 Impasse des Chefs, Paris','0667890123','123456','Bastille'),(31,'Grace','Noël','grace.noel@cuisine.fr','30 Passage des Saveurs, Paris','0678901234','123456','Concorde'),(32,'Hector','Blanc','hector.blanc@cuisine.fr','33 Ruelle des Plats, Paris','0689012345','123456','Invalides'),(33,'Isabelle','Petit','isabelle.petit@cuisine.fr','36 Boulevard des Chefs, Paris','0690123456','123456','Saint-Lazare'),(34,'Jack','Grand','jack.grand@cuisine.fr','39 Avenue des Gourmets, Paris','0601234567','123456','Gare de Lyon'),(35,'Roux','Mathilde','mathilde.roux@cuisine.fr','28 Rue du Faubourg Saint-Antoine','0601112233','123456','Ledru-Rollin'),(36,'Bonnet','Julien','julien.bonnet@cuisine.fr','19 Rue Saint-Maur','0602223344','123456','Rue Saint-Maur'),(37,'Lemoine','Claire','claire.lemoine@cuisine.fr','42 Rue Montmartre','0603334455','123456','Grands Boulevards'),(38,'Garnier','Vincent','vincent.garnier@cuisine.fr','11 Rue de Buci','0604445566','123456','Odéon'),(39,'Faure','Chloé','chloe.faure@cuisine.fr','31 Rue des Petits Champs','0605556677','123456','Bourse'),(40,'Simon','Alexandre','alexandre.simon@cuisine.fr','7 Rue de Charenton','0606667788','123456','Gare de Lyon'),(41,'Fournier','Aurélie','aurelie.fournier@cuisine.fr','16 Rue des Archives','0607778899','123456','Rambuteau'),(42,'Vidal','Guillaume','guillaume.vidal@cuisine.fr','29 Rue de la Pompe','0608889900','123456','Rue de la Pompe'),(43,'Morin','Pauline','pauline.morin@cuisine.fr','38 Rue de Passy','0609990011','123456','Passy'),(44,'Lefevre','Romain','romain.lefevre@cuisine.fr','22 Rue de Vaugirard','0600112233','123456','Convention'),(45,'a','a','a','a','1','a','Porte Maillot'),(48,'a','a','a','a','1','a','Porte Maillot'),(49,'a','a','a','a','1','a','Argentine');
/*!40000 ALTER TABLE `particulier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `plat`
--

DROP TABLE IF EXISTS `plat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plat` (
  `IdPlat` int NOT NULL AUTO_INCREMENT,
  `NomPlat` varchar(255) NOT NULL,
  `Quantite` int NOT NULL,
  `Prix` decimal(10,2) DEFAULT NULL,
  `Categorie` varchar(50) NOT NULL,
  `TypePlat` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`IdPlat`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plat`
--

LOCK TABLES `plat` WRITE;
/*!40000 ALTER TABLE `plat` DISABLE KEYS */;
INSERT INTO `plat` VALUES (1,'Bruschetta Tomate-Basilic',14,7.00,'Entrée','Italienne'),(2,'Tartare de Betterave',10,9.00,'Entrée','Française'),(3,'Caprese à la Mozzarella di Bufala',10,8.00,'Entrée','Italienne'),(4,'Velouté de Potimarron',9,7.00,'Entrée','Française'),(5,'Ratatouille Maison',9,14.00,'Plat Principal','Française'),(6,'Lasagnes Végétariennes',7,16.00,'Plat Principal','Italienne'),(7,'Boeuf Bourguignon',6,20.00,'Plat Principal','Française'),(8,'Risotto aux Champignons',8,17.00,'Plat Principal','Italienne'),(9,'Poulet Basquaise',5,18.00,'Plat Principal','Standart'),(10,'Tiramisu Classique',12,6.00,'Dessert','Italienne'),(11,'Mousse au Chocolat',10,5.00,'Dessert','Française'),(12,'Panna Cotta Coco et Mangue',8,6.00,'Dessert','Italienne');
/*!40000 ALTER TABLE `plat` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recommendation_plat`
--

DROP TABLE IF EXISTS `recommendation_plat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recommendation_plat` (
  `IdRecommendation` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(100) NOT NULL,
  PRIMARY KEY (`IdRecommendation`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recommendation_plat`
--

LOCK TABLES `recommendation_plat` WRITE;
/*!40000 ALTER TABLE `recommendation_plat` DISABLE KEYS */;
/*!40000 ALTER TABLE `recommendation_plat` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-09  9:18:08
