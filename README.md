# ExamM2 - Microsoft Technologies

## 📋 Description

Projet d'examen Master 2 Informatique CYBER - Microsoft Technologies.  
Ce projet contient 3 exercices distincts implémentés dans une solution .NET unique.

---

## 🏗️ Structure de la solution

```
ExamM2.sln                          # Solution principale
├── ExamM2.Api/                     # API e-commerce (Exercice 1)
├── ExamM2.Api.Tests/               # Tests de l'API
├── ExamM2.Maze/                    # Résolution de labyrinthe (Exercice 2)
├── ExamM2.Maze.Tests/              # Tests du labyrinthe
└── .gitignore                      # Exclusion /bin et /obj
```

---

## 🎯 Exercice 1 : API E-Commerce

### Description

API RESTful pour la gestion de commandes e-commerce avec système de remises automatiques et codes promotionnels.

### Architecture

```
ExamM2.Api/
├── Controllers/
│   ├── ProductsController.cs      # GET /products
│   └── OrdersController.cs        # POST /orders
├── Services/
│   ├── ProductStockService.cs     # Gestion du stock (Singleton)
│   └── OrderService.cs            # Logique métier des commandes
├── Models/
│   ├── Product.cs                 # Modèle produit
│   └── Discount.cs                # Modèle remise
└── DTOs/
    ├── ProductDto.cs
    ├── OrderRequestDto.cs
    ├── OrderResponseDto.cs
    └── ErrorResponseDto.cs
```

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

## 🗄️ Exercice 3 : Ajout de Base de Données

> 🚧 À implémenter

---

## 🛠️ Technologies utilisées

- .NET 9.0
- ASP.NET Core Web API
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

## 📊 Récapitulatif des exercices

| Exercice | Description | Tests | Statut |
|----------|-------------|-------|--------|
| 1 | API E-commerce | 18/18 ✅ | Complet |
| 2 | Labyrinthe TDD | 23/23 ✅ | Complet |
| 3 | Base de données | - | À faire |

**Total actuel : 41/41 tests** 🎉

---

## 👤 Auteur

Samy Boudjema - Master 2 Informatique CYBER

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
