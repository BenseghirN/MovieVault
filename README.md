# 🎬 MovieVault - Gestion de Cinémathèque

## 📌 Introduction
MovieVault est une application de gestion de cinémathèque permettant de gérer les films possédés, ceux vus et ceux à voir.
Elle repose sur **WinForms** pour l'interface utilisateur, **SQL Server** pour la base de données et **ADO.NET** pour l'accès aux données.

## 🚀 Technologies Utilisées
- **.NET 9** (WinForms, Console, Core, Data)
- **SQL Server 2022** (via Docker)
- **ADO.NET** (pour le CRUD sans ORM)
- **IConfiguration** (via `appsettings.json` pour gérer la connexion à la base de données)
- **C# (Architecture en couches : UI, Core, Data)**
- **Docker Compose** (pour l'automatisation de la base de données)

## 📂 Structure du Projet
```
📂 MovieVault-Solution/
│── 📂 MovieVault.UI/       <-- Interface utilisateur WinForms
│   │── 📜 appsettings.json <-- Fichier de configuration (Connection String, API Keys...)
│── 📂 MovieVault.Core/      <-- Contient les modèles et services
│   │── 📂 Models/          <-- Les entités du projet (Movies, Users...)
│── 📂 MovieVault.Data/      <-- Accès aux données via ADO.NET
│   │── 📂 Repositories/    <-- CRUD sera ici !
│── 📂 MovieVault.Tests/     <-- Tests unitaires et d'intégration
│── 📂 Db/                  <-- Scripts SQL d'initialisation de la base
│── 📜 Dockerfile           <-- Configuration du conteneur SQL Server
│── 📜 docker-compose.yml   <-- Automatisation de la base SQL Server
```

## 🔥 Fonctionnalités
✅ Gestion des films (CRUD : Ajouter, Modifier, Supprimer, Lire)  
✅ Gestion des utilisateurs  
✅ Attribution d'un statut aux films (Vu, Non vu, À voir)  
✅ Ajout de critiques et de notes aux films  
✅ Gestion des genres et des acteurs associés aux films  
✅ Base de données SQL Server gérée via Docker  
✅ Chargement des configurations via `appsettings.json`  

## ⚙️ Installation & Configuration
### 1️⃣ Prérequis
- **.NET 9 SDK**
- **Docker & Docker Compose**
- **SQL Server 2022 (via Docker ou installé localement)**
- **Visual Studio 2022**

### 2️⃣ Cloner le projet
```bash
git clone https://github.com/username/MovieVault.git
cd MovieVault-Solution
```

### 3️⃣ Lancer la base de données avec Docker
```bash
docker-compose up --build -d
```

### 4️⃣ Vérifier la connexion SQL
```bash
docker exec -it movievault-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P 'K]xr!9*a>sPw' -Q "SELECT name FROM sys.databases;"
```

### 5️⃣ Lancer l'application
```bash
dotnet build
cd MovieVault.UI
dotnet run
```

## 🛠️ Développement & Structure du Code
### 📜 Modèles (`Entities`)
Les entités se trouvent dans `MovieVault.Core/Models/` et incluent :
- `Movie.cs` 🎬 (Films)
- `User.cs` 👤 (Utilisateurs)
- `Genre.cs` 🎭 (Genres)
- `Actor.cs` 🎭 (Acteurs)
- `Review.cs` ✍️ (Critiques et notes)
- `MovieStatus.cs` 👀 (Statut des films : Vu, Non vu, À voir)

### 📚 Accès aux Données (ADO.NET)
Les opérations CRUD sont définies dans `MovieVault.Data/Repositories/`, incluant `MovieRepository.cs` pour la gestion des films.

### 🌐 Configuration avec `appsettings.json`
Le fichier **`appsettings.json`** stocke la connexion à la base de données et d'autres configurations

## 🧪 Tests & Débogage
- Tests unitaires et d'intégration dans `MovieVault.Tests`
- Mode debug avec `dotnet run --project MovieVault.UI`
- Commandes utiles pour recréer la base :
```bash
docker-compose down -v
docker-compose up --build -d
```

## 🔮 Améliorations Futures
- 📡 Intégration d'une API externe (ex: **TMDB**) pour enrichir les fiches films
- 🎨 Amélioration de l'interface WinForms
- 📈 Ajout de statistiques et d'un dashboard utilisateur
- 🔐 Gestion avancée des utilisateurs et permissions

