# ExamM2 - Microsoft Technologies

![.NET CI/CD](https://github.com/SamyBoudjema/examen_Microsoft_Technologies/actions/workflows/dotnet-ci.yml/badge.svg)
![Tests](https://img.shields.io/badge/tests-41%2F41%20passing-brightgreen)
![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)

## 📋 Description

Projet d'examen M2 Informatique CYBER - 3 exercices en .NET 9.0 avec CI/CD.

---

## 🎯 Exercices

| # | Description | Tests | Endpoints |
|---|-------------|-------|-----------|
| **1** | API E-Commerce (Singleton) | 18/18 ✅ | `/api/products`, `/api/orders` |
| **2** | Résolution Labyrinthe (BFS) | 23/23 ✅ | - |
| **3** | EF Core InMemory | ✅ | `/api/productsdb`, `/api/ordersdb` |

**Total : 41/41 tests** 🎉

---

## 🚀 Démarrage rapide

### Prérequis
- .NET 9.0 SDK

### Lancer l'API
```bash
cd ExamM2.Api
dotnet run
```

**URLs disponibles :**
- `http://localhost:5149/` → Swagger UI
- `http://localhost:5149/health` → Health Check
- `http://localhost:5149/openapi/v1.json` → OpenAPI Spec

### Lancer les tests
```bash
dotnet test
```

---

## 📦 Exercice 1 : API E-Commerce

**Architecture :** Singleton + Services

**Endpoints :**
- `GET /api/products` - Liste des produits
- `POST /api/orders` - Créer une commande

**Règles métier :**
- Remise auto -10% si qty > 5 sur un produit
- Remise auto -5% si total > 100€
- Codes promo : `DISCOUNT10` (10%), `DISCOUNT20` (20%)

**Exemple :**
```bash
curl http://localhost:5149/api/products
curl -X POST http://localhost:5149/api/orders \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":1,"quantity":3}],"promo_code":"DISCOUNT10"}'
```

---

## 🧩 Exercice 2 : Labyrinthe

**Algorithme :** BFS (Breadth-First Search)

**Programme de démo :**
```bash
cd ExamM2.Maze
dotnet run
```

**Tests :**
- 4 labyrinthes de difficulté croissante
- Validation du chemin le plus court
- Labyrinthe annexe (distance = 18)

---

## 🗄️ Exercice 3 : EF Core InMemory

**Architecture :** EF Core + DbContext + Seed Data

**Endpoints :**
- `GET /api/productsdb` - Produits depuis DB
- `POST /api/ordersdb` - Commandes avec DB

**Produits IT (seed data) :**
- RAM Corsair Vengeance 32GB DDR5 - 1000€
- SSD Samsung 980 PRO 2TB NVMe - 250€
- iPhone 15 Pro 256GB - 1200€

**Exemple :**
```bash
curl http://localhost:5149/api/productsdb
curl -X POST http://localhost:5149/api/ordersdb \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":1,"quantity":2},{"id":3,"quantity":1}],"promoCode":"DISCOUNT10"}'
```

---

## 🛠️ Technologies

- **.NET 9.0** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core InMemory 9.0** - Base de données
- **xUnit** - Tests unitaires
- **Swagger UI** - Documentation (via CDN)
- **Health Checks** - Monitoring
- **GitHub Actions** - CI/CD

---

## 📊 CI/CD

Pipeline automatique à chaque push :
- ✅ Restore dependencies
- ✅ Build solution
- ✅ Run 41 unit tests
- ✅ Upload test results

---

## 👤 Auteur

**Samy Boudjema** - M2 Informatique CYBER
