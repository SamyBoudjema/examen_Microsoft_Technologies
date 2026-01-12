# ExamM2 - Microsoft Technologies

![.NET CI/CD](https://github.com/SamyBoudjema/examen_Microsoft_Technologies/actions/workflows/dotnet-ci.yml/badge.svg)
![Tests](https://img.shields.io/badge/tests-41%2F41%20passing-brightgreen)
![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)
![EF Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4)

## 📋 Description

Projet d'examen Master 2 Informatique CYBER - Microsoft Technologies.  
Ce projet contient 3 exercices distincts implémentés dans une solution .NET unique avec CI/CD.

### 📊 Statut des exercices

| Exercice | Description | Tests | Statut |
|----------|-------------|-------|--------|
| 1 | API E-Commerce | 18/18 ✅ | Complet |
| 2 | Résolution de Labyrinthe (BFS) | 23/23 ✅ | Complet |
| 3 | Intégration EF Core InMemory | - | Complet |
| **TOTAL** | | **41/41 ✅** | **100%** |

---

## 🏗️ Structure de la solution

```
ExamM2.sln                          # Solution principale
├── ExamM2.Api/                     # API e-commerce (Exercices 1 & 3)
│   ├── Controllers/
│   │   ├── ProductsController.cs      # Exercice 1
│   │   ├── OrdersController.cs        # Exercice 1
│   │   ├── ProductsDbController.cs    # Exercice 3
│   │   └── OrdersDbController.cs      # Exercice 3
│   ├── Services/
│   │   ├── ProductStockService.cs     # Exercice 1
│   │   ├── OrderService.cs            # Exercice 1
│   │   ├── ProductStockDbService.cs   # Exercice 3
│   │   ├── PromoCodeDbService.cs      # Exercice 3
│   │   └── OrderDbService.cs          # Exercice 3
│   ├── Models/
│   │   └── Entities/                  # Exercice 3 (EF Core)
│   ├── Data/
│   │   └── ECommerceDbContext.cs      # Exercice 3
│   └── DTOs/
├── ExamM2.Api.Tests/               # Tests de l'API (18 tests)
├── ExamM2.Maze/                    # Résolution de labyrinthe (Exercice 2)
├── ExamM2.Maze.Tests/              # Tests du labyrinthe (23 tests)
└── .gitignore                      # Exclusion /bin et /obj
```

---

## 🎯 Exercice 1 : API E-Commerce

### Description

API RESTful pour la gestion de commandes e-commerce avec système de remises automatiques et codes promotionnels.

### Endpoints

#### GET /products
Liste tous les produits disponibles avec leur stock.

**Réponse :**
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "price": 999.99,
    "stock": 10
  }
]
```

#### POST /orders
Crée une commande avec application automatique des remises.

**Requête :**
```json
{
  "products": [
    {"id": 1, "quantity": 3}
  ],
  "promo_code": "DISCOUNT20"
}
```

**Réponse succès (200) :**
```json
{
  "products": [
    {
      "id": 1,
      "name": "Laptop",
      "quantity": 3,
      "pricePerUnit": 999.99,
      "total": 2999.97
    }
  ],
  "discounts": [
    {"type": "order", "value": 5},
    {"type": "promo", "value": 20}
  ],
  "total": 2249.98
}
```

**Réponse erreur (400) :**
```json
{
  "errors": [
    "le produit avec l'identifiant 999 n'existe pas",
    "il ne reste que 5 exemplaire pour le produit Smartphone"
  ]
}
```

### Règles métier

#### Remises automatiques
- **10%** sur un produit si quantité > 5
- **5%** sur le total si montant > 100€ (type "order")

#### Codes promo
- `DISCOUNT20` : -20%
- `DISCOUNT10` : -10%
- Valides uniquement si commande > 50€ (avant remises)
- Cumul additif avec la remise "order"

#### Validation
- Vérification de l'existence des produits
- Contrôle du stock disponible
- Mise à jour du stock après commande validée
- Remontée de toutes les erreurs simultanément

### Tests

**18 tests unitaires** couvrant tous les cas :
- ✅ Tests du service ProductStockService (7 tests)
- ✅ Tests du service OrderService (11 tests)
- ✅ Cas valides et invalides
- ✅ 100% de réussite

**Exécuter les tests :**
```bash
dotnet test ExamM2.Api.Tests/ExamM2.Api.Tests.csproj
```

### Lancer l'API

```bash
dotnet run --project ExamM2.Api/ExamM2.Api.csproj
```

L'API sera accessible sur : `http://localhost:5149`

### Tester l'API

**Avec curl :**
```bash
# Liste des produits
curl http://localhost:5149/products

# Créer une commande
curl -X POST http://localhost:5149/orders \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":1,"quantity":2}],"promo_code":"DISCOUNT10"}'
```

**Avec le script de test :**
```bash
./test-api_exo1.sh
```

---

## 🧩 Exercice 2 : Résolution de Labyrinthe (TDD)

### Objectif
Créer un résolveur de labyrinthe en suivant une approche TDD stricte (les tests valent plus de points que l'algorithme).

### Description
Le programme résout un labyrinthe représenté par une chaîne de caractères :
- `D` : Départ
- `S` : Sortie
- `.` : Case vide (chemin)
- `#` : Mur

**Exemple de labyrinthe :**
```
D..#.
##...
.#.#.
...#.
####S
```

### Fonctionnalités implémentées

#### 1. Parser de labyrinthe
- Parse la chaîne en grille 2D
- Identifie le départ (D) et la sortie (S)
- Détecte les cases valides et les murs

#### 2. GetNeighbours(x, y)
- Retourne les voisins orthogonaux valides
- Exclut : murs, cases hors limites, départ

#### 3. Fill()
- Algorithme BFS (Breadth-First Search)
- Traite une cellule de la queue
- Calcule les distances depuis le départ
- Retourne `true` quand la sortie est atteinte

#### 4. GetDistance()
- Retourne la distance minimale départ → sortie
- Appelle Fill() en boucle jusqu'à atteindre la sortie

#### 5. GetShortestPath()
- Reconstruit le chemin optimal
- Remonte depuis la sortie jusqu'au départ
- Retourne la liste des coordonnées du chemin

### Tests unitaires

- ✅ **Parser** : 5 tests (identification départ/sortie, dimensions, grille)
- ✅ **GetNeighbours** : 7 tests (voisins valides, murs, limites, départ)
- ✅ **Fill** : 5 tests (queue, sortie, distances, duplicatas)
- ✅ **GetDistance** : 3 tests (simple, murs, complexe)
- ✅ **GetShortestPath** : 3 tests (chemin valide, murs, séquentiel)
- ✅ **Total : 23/23 tests** 🎉

**Exécuter les tests :**
```bash
dotnet test ExamM2.Maze.Tests/ExamM2.Maze.Tests.csproj
```

### Tester le programme

**Exécution directe :**
```bash
dotnet run --project ExamM2.Maze/ExamM2.Maze.csproj
```

**Avec le script de test complet :**
```bash
./test-maze_exo2.sh
```

**Résultats attendus :**
```
Test 1 : Labyrinthe simple 3x3
Distance: 4
Chemin: (0,0) -> (1,0) -> (2,0) -> (2,1) -> (2,2)

Test 2 : Labyrinthe avec murs
Distance: 4
Chemin: (0,0) -> (1,0) -> (1,1) -> (1,2) -> (2,2)

Test 3 : Labyrinthe complexe 5x5
Distance: 8
Chemin: (0,0) -> (1,0) -> (2,0) -> (2,1) -> (3,1) -> (4,1) -> (4,2) -> (4,3) -> (4,4)
```

---

## 🗄️ Exercice 3 : Ajout de Base de Données (EF Core InMemory)

### 📋 Objectif

Intégrer **Entity Framework Core InMemory** à l'API e-commerce de l'exercice 1 en ajoutant :
- Une base de données avec **Produits** et **Codes Promo**
- Des **nouveaux endpoints** séparés pour préserver l'exercice 1 (noté)
- Une architecture non-invasive

### 🏗️ Architecture

**Approche choisie : Option 1 - Nouveaux endpoints séparés**

```
EXERCICE 1 (préservé) :
- /api/products → ProductStockService (singleton)
- /api/orders   → OrderService

EXERCICE 3 (nouveaux) :
- /api/productsdb → ProductStockDbService (EF Core)
- /api/ordersdb   → OrderDbService (EF Core)
```

### 📦 Composants ajoutés

#### 1. Entities (EF Core)
- `ProductEntity.cs` : Id, Name, Price, Stock
- `PromoCodeEntity.cs` : Id, Code, DiscountPercentage, IsActive

#### 2. DbContext
- `ECommerceDbContext.cs` avec seed data :
  - **3 produits IT** : RAM Corsair 32GB (150€), SSD Samsung 2TB (250€), iPhone 15 Pro (1200€)
  - **3 codes promo** : DISCOUNT10 (10%), DISCOUNT20 (20%), EXPIRED (inactif)

#### 3. Services
- `ProductStockDbService.cs` : Gestion stock via DB
- `PromoCodeDbService.cs` : Validation codes promo DB
- `OrderDbService.cs` : Traitement commandes avec DB

#### 4. Controllers
- `ProductsDbController.cs` : GET /api/productsdb
- `OrdersDbController.cs` : POST /api/ordersdb

### 🧪 Endpoints

#### GET /api/productsdb
Récupère tous les produits depuis la DB.

```bash
curl http://localhost:5149/api/productsdb
```

**Réponse** :
```json
[
  { "id": 1, "name": "RAM Corsair Vengeance 32GB DDR5", "price": 150.00, "stock": 25 },
  { "id": 2, "name": "SSD Samsung 980 PRO 2TB NVMe", "price": 250.00, "stock": 15 },
  { "id": 3, "name": "iPhone 15 Pro 256GB", "price": 1200.00, "stock": 8 }
]
```

#### POST /api/ordersdb
Crée une commande avec codes promo DB.

```bash
curl -X POST http://localhost:5149/api/ordersdb \
  -H "Content-Type: application/json" \
  -d '{
    "products": [
      { "id": 1, "quantity": 2 },
      { "id": 3, "quantity": 1 }
    ],
    "promoCode": "DISCOUNT10"
  }'
```

**Réponse** :
```json
{
  "products": [
    { "id": 1, "name": "RAM Corsair Vengeance 32GB DDR5", "quantity": 2, "pricePerUnit": 150.00, "total": 300.00 },
    { "id": 3, "name": "iPhone 15 Pro 256GB", "quantity": 1, "pricePerUnit": 1200.00, "total": 1200.00 }
  ],
  "discounts": [
    { "type": "auto", "value": 75.00 },
    { "type": "promo_code", "value": 150.00 }
  ],
  "total": 1275.00
}
```

### 🎯 Règles métier (identiques à Exercice 1)

✅ **Remise automatique quantité** : -10% si qty > 5 sur un produit  
✅ **Remise automatique montant** : -5% si sous-total > 100€  
✅ **Codes promo DB** : DISCOUNT10 (10%), DISCOUNT20 (20%)  
✅ **Validation stock** : Impossible si stock insuffisant  
✅ **Codes promo inactifs** : EXPIRED refusé

### ✅ Statut : COMPLET

- [x] Entities créées
- [x] DbContext avec seed data
- [x] Services DB implémentés
- [x] Controllers séparés
- [x] Tests préservés (41/41 ✅)
- [x] API fonctionnelle

### 📝 Notes importantes

⚠️ **Exercice 1 préservé** : Les endpoints `/api/products` et `/api/orders` originaux sont intacts pour l'évaluation.

🔍 **Version EF Core** : 9.0.0 (compatible .NET 9.0)

---

## 🛠️ Technologies utilisées

- .NET 9.0
- ASP.NET Core Web API
- **Entity Framework Core InMemory 9.0.0**
- xUnit (Tests unitaires)
- C# avec Nullable enabled

---

## 📦 Installation

### Prérequis
- .NET 9.0 SDK
- Git

### Cloner le projet
```bash
git clone https://github.com/SamyBoudjema/examen_Microsoft_Technologies.git
cd ExamM2
```

### Compiler la solution
```bash
dotnet build
```

### Exécuter tous les tests
```bash
dotnet test
# Résultat attendu : 41/41 tests réussis (18 Exo1 + 23 Exo2)
```

### Tests rapides par exercice
```bash
# Exercice 1 : API E-commerce (avec démarrage automatique de l'API)
./test-api_exo1.sh

# Exercice 2 : Résolveur de labyrinthe
./test-maze_exo2.sh
```

---

## � Documentation & Monitoring

### Swagger UI

**URL** : `http://localhost:5149/` (après `dotnet run`)

Interface interactive pour tester tous les endpoints :
- 📦 **Exercice 1** : `/api/products`, `/api/orders` (Singleton)
- 🗄️ **Exercice 3** : `/api/productsdb`, `/api/ordersdb` (EF Core)
- 🐛 **Debug** : `/api/debug/database` (État de la DB)

### Health Checks

**URL** : `http://localhost:5149/health`

```bash
curl http://localhost:5149/health
```

**Réponse** :
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0010000"
}
```

### CI/CD Pipeline

**GitHub Actions** : Build + Tests automatiques à chaque push

- ✅ Restore dependencies
- ✅ Build solution (.NET 9.0)
- ✅ Run 41 unit tests
- ✅ Upload test results
- ✅ Badge de statut dans le README

---

## �📊 Récapitulatif des exercices

| Exercice | Description | Tests | Statut |
|----------|-------------|-------|--------|
| 1 | API E-commerce | 18/18 ✅ | Complet |
| 2 | Labyrinthe BFS | 23/23 ✅ | Complet |
| 3 | EF Core InMemory | - | Complet |
| **Bonus** | Swagger + Health + CI/CD | - | Complet |

**Total actuel : 41/41 tests** 🎉

---

## 👤 Auteur

Samy Boudjema - Master 2 Informatique CYBER

---

---

## 🧪 Tests

### Tests unitaires (Exercices 1 & 2)

```bash
# Tous les tests
dotnet test

# Résultat attendu : 41/41 tests ✅
```

### Tests API manuels (Exercice 3)

**1. Démarrer l'API :**
```bash
cd ExamM2.Api
dotnet run
```

**2. Tester les endpoints :**
```bash
# Liste des produits (EF Core)
curl http://localhost:5149/api/productsdb

# Créer une commande
curl -X POST http://localhost:5149/api/ordersdb \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":1,"quantity":2}],"promoCode":"DISCOUNT10"}'
```

**3. Script de test complet :**
```bash
chmod +x test-all.sh
./test-all.sh
```

---

## 📝 Notes de développement

### Bonnes pratiques respectées
- **Code propre** : Commentaires brefs et pertinents uniquement
- **Architecture** : Séparation claire (Controllers, Services, Models, DTOs)
- **TDD** : Tests écrits avant l'implémentation (surtout Exo2)
- **Injection de dépendances** : Services singleton et scoped appropriés
- **Tests exhaustifs** : 41 tests unitaires couvrant tous les cas
- **Gestion d'erreurs** : Validation complète avec messages clairs
- **Documentation** : README complet avec exemples d'utilisation

### Technologies & outils

**Backend**
- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core InMemory 9.0.0
- C# avec Nullable enabled

**Documentation & Monitoring**
- Swagger/OpenAPI (Swashbuckle 10.1.0)
- Health Checks API
- GitHub Actions CI/CD

**Tests**
- xUnit (41 tests unitaires)
- Test coverage : 100% des fonctionnalités métier

**DevOps**
- Git & GitHub
- Automated CI/CD pipeline
- Status badges
