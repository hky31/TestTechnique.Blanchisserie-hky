# BlanchisserieStart - Application de Base d'Authentification

Cette application fournit une base d'authentification complète avec un backend .NET et un frontend Angular pour un système de gestion de blanchisserie. Elle est conçue pour servir de point de départ pour des tests techniques.

## 🏗️ Architecture

- **Backend**: .NET 9.0 avec ASP.NET Core Web API
- **Base de données**: Entity Framework Core avec SQL Server LocalDB
- **Frontend**: Angular 20.3 avec PrimeNG
- **Authentification**: JWT Bearer tokens avec gestion des rôles
- **Sécurité**: Guards basés sur les rôles, intercepteurs HTTP

## 📋 Prérequis

### Environnement de développement
- **Visual Studio 2022** ou **VS Code** avec l'extension C#
- **.NET 9.0 SDK** - [Télécharger ici](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Node.js** (version 18 ou plus récente) - [Télécharger ici](https://nodejs.org/)
- **SQL Server LocalDB** (inclus avec Visual Studio ou SQL Server Express)

### Vérification des prérequis
```powershell
# Vérifier .NET
dotnet --version

# Vérifier Node.js
node --version
npm --version

# Vérifier Angular CLI (optionnel)
ng version
```

## 🚀 Installation et Configuration

### 1. Cloner le projet
```powershell
git clone <url-du-repo>
cd BlanchisserieStart_example
```

### 2. Configuration du Backend

#### a) Accéder au dossier backend
```powershell
cd backend/BlanchisserieAPI
```

#### b) Restaurer les packages NuGet
```powershell
dotnet restore
```

#### c) Configurer la base de données
```powershell
# Créer et appliquer les migrations
dotnet ef migrations add InitialCreate
dotnet ef database update

# Si la commande ef n'est pas disponible, installer l'outil global
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### d) Lancer le backend
```powershell
dotnet run
```

Le backend sera accessible sur `http://localhost:5172`

### 3. Configuration du Frontend

#### a) Accéder au dossier frontend
```powershell
cd ../frontend/blanchisserie-app
```

#### b) Installer les dépendances
```powershell
npm install
```

#### c) Lancer le frontend
```powershell
npm start
```

Le frontend sera accessible sur `http://localhost:4200`

## 👤 Comptes de Test

L'application crée automatiquement deux comptes de test lors du premier lancement :

### Compte Administrateur
- **Nom d'utilisateur**: `admin`
- **Mot de passe**: `admin123`
- **Rôle**: Admin
- **Accès**: Toutes les fonctionnalités

### Compte Utilisateur
- **Nom d'utilisateur**: `user`
- **Mot de passe**: `user123`
- **Rôle**: User
- **Accès**: Fonctionnalités de base uniquement

## 🔧 Structure du Projet

```
BlanchisserieStart_example/
├── backend/
│   └── BlanchisserieAPI/
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   └── TestController.cs
│       ├── Data/
│       │   └── ApplicationDbContext.cs
│       ├── Models/
│       │   ├── User.cs
│       │   ├── Role.cs
│       │   └── UserRole.cs
│       ├── DTOs/
│       │   ├── LoginRequestDto.cs
│       │   ├── AuthResponseDto.cs
│       │   └── UserDto.cs
│       ├── Services/
│       │   ├── IAuthService.cs
│       │   ├── AuthService.cs
│       │   ├── IJwtService.cs
│       │   └── JwtService.cs
│       └── Program.cs
└── frontend/
    └── blanchisserie-app/
        ├── src/app/
        │   ├── components/
        │   │   ├── login/
        │   │   ├── dashboard/
        │   │   └── admin/
        │   ├── guards/
        │   │   ├── auth.guard.ts
        │   │   └── admin.guard.ts
        │   ├── interceptors/
        │   │   └── auth.interceptor.ts
        │   ├── models/
        │   │   └── auth.models.ts
        │   ├── services/
        │   │   └── auth.service.ts
        │   └── app.routes.ts
        └── src/styles.scss
```

## 🛡️ Fonctionnalités d'Authentification

### Backend (.NET)
- **JWT Token Generation**: Génération de tokens sécurisés avec claims
- **Password Hashing**: Utilisation de BCrypt pour le hashage des mots de passe
- **Role-based Authorization**: Système de rôles avec attributs `[Authorize]`
- **CORS Configuration**: Configuration pour les requêtes cross-origin
- **Entity Framework**: Gestion des données avec migrations automatiques

### Frontend (Angular)
- **Reactive Forms**: Formulaires avec validation
- **JWT Management**: Stockage et gestion automatique des tokens
- **Route Guards**: Protection des routes selon les rôles
- **HTTP Interceptors**: Ajout automatique des tokens aux requêtes
- **State Management**: Gestion de l'état utilisateur avec BehaviorSubject

## 🔗 API Endpoints

### Authentification
```
POST /api/auth/login
Body: { "username": "string", "password": "string" }
Response: { "token": "string", "user": { ... } }

GET /api/auth/me
Headers: Authorization: Bearer <token>
Response: { "id": number, "username": "string", "roles": [...] }
```

### Test (protégé)
```
GET /api/test/admin
Headers: Authorization: Bearer <token>
Requires: Admin role

GET /api/test/user
Headers: Authorization: Bearer <token>
Requires: User or Admin role
```

## 🎨 Interface Utilisateur

### Pages disponibles
1. **Page de connexion** (`/login`)
   - Formulaire de connexion avec validation
   - Gestion des erreurs d'authentification
   - Redirection automatique selon le rôle

2. **Dashboard utilisateur** (`/dashboard`)
   - Vue d'ensemble des fonctionnalités
   - Menu de navigation adaptatif
   - Informations utilisateur

3. **Panel administrateur** (`/admin`)
   - Fonctionnalités réservées aux administrateurs
   - Gestion avancée du système
   - Accès restreint par guard

## 🧪 Tests et Développement

### Lancer en mode développement
```powershell
# Backend (Terminal 1)
cd backend/BlanchisserieAPI
dotnet watch run

# Frontend (Terminal 2)
cd frontend/blanchisserie-app
npm run dev
```

### Build de production
```powershell
# Backend
cd backend/BlanchisserieAPI
dotnet publish -c Release

# Frontend
cd frontend/blanchisserie-app
npm run build
```

## 🐛 Résolution de Problèmes

### Problèmes courants

#### "Database connection failed"
```powershell
# Vérifier LocalDB
sqllocaldb info
sqllocaldb start MSSQLLocalDB

# Recréer la base de données
dotnet ef database drop
dotnet ef database update
```

#### "CORS errors"
Vérifiez que le backend est configuré pour accepter les requêtes depuis `http://localhost:4200`

#### "JWT token invalid"
Les tokens expirent après 24 heures. Déconnectez-vous et reconnectez-vous.

### Logs de débogage
- **Backend**: Logs dans la console Visual Studio
- **Frontend**: Utilisez les outils de développement du navigateur (F12)

## 📝 Notes pour les Développeurs

### Extensibilité
Cette base peut être étendue avec :
- Gestion des clients et commandes
- Système de facturation
- Tracking des vêtements
- Notifications en temps réel
- Rapports et statistiques

### Bonnes Pratiques Implémentées
- ✅ Séparation des responsabilités
- ✅ Injection de dépendances
- ✅ Validation des données
- ✅ Gestion d'erreurs centralisée
- ✅ Sécurité par défaut
- ✅ Code TypeScript strict
- ✅ Responsive design

## 📧 Support

Pour toute question sur l'implémentation ou l'extension de cette base d'authentification, consultez la documentation du code ou créez une issue dans le repository.

---

**Version**: 1.0.0  
**Dernière mise à jour**: 2025-01-19  
**Compatibilité**: .NET 9.0, Angular 20, Node.js 18+
