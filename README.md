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
./test-api.sh
```

---

## 🧩 Exercice 2 : Résolution de Labyrinthe (TDD)

> 🚧 À implémenter

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
```

---

## 👤 Auteur

Samy Boudjema - Master 2 Informatique CYBER

---

## 📝 Notes de développement

### Bonnes pratiques respectées
- Code lisible avec accolades systématiques
- Architecture en couches (Controllers, Services, Models, DTOs)
- Injection de dépendances
- Services singleton et scoped appropriés
- Tests unitaires exhaustifs
- Gestion propre des erreurs
- Validation complète des données
