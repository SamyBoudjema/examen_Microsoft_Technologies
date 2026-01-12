#!/bin/bash

echo "======================================"
echo "🧪 Tests ExamM2 - Tous les exercices"
echo "======================================"
echo ""

# Exercice 1 & 2 : Tests unitaires
echo "1️⃣ Tests unitaires (Exercices 1 & 2)"
echo "--------------------------------------"
dotnet test --no-build --verbosity quiet
echo ""

# Exercice 3 : Tests API (nécessite l'API en cours d'exécution)
echo "2️⃣ Tests API Exercice 3 (EF Core)"
echo "--------------------------------------"
echo "⚠️  L'API doit être démarrée (dotnet run)"
echo ""

# Test GET /api/productsdb
echo "📦 GET /api/productsdb"
curl -s http://localhost:5149/api/productsdb | jq '.'
echo ""

# Test POST /api/ordersdb
echo "🛒 POST /api/ordersdb"
curl -s -X POST http://localhost:5149/api/ordersdb \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":1,"quantity":2}],"promoCode":"DISCOUNT10"}' | jq '.'
echo ""

echo "✅ Tests terminés"
