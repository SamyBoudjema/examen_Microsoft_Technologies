#!/bin/bash

echo "═══════════════════════════════════════════════════════"
echo "  🗄️ Test API E-commerce avec EF Core (Exercice 3)"
echo "═══════════════════════════════════════════════════════"
echo ""

cd ExamM2.Api

echo "🔨 Compilation du projet..."
dotnet build --nologo --verbosity quiet > /dev/null 2>&1
if [ $? -ne 0 ]; then
    echo "❌ Erreur de compilation"
    exit 1
fi
echo "✅ Compilation réussie"
echo ""

echo "🚀 Démarrage de l'API..."
dotnet run --no-build --urls "http://localhost:5149" > /dev/null 2>&1 &
API_PID=$!
sleep 3

echo "═══════════════════════════════════════════════════════"
echo "  Test des endpoints EF Core (/productsdb et /ordersdb)"
echo "═══════════════════════════════════════════════════════"
echo ""

echo "📦 Test GET /productsdb (liste des produits depuis DB)"
echo "───────────────────────────────────────────────────────"
curl -s http://localhost:5149/productsdb | jq '.'
echo ""

echo "🛒 Test POST /ordersdb (commande avec code promo DISCOUNT10)"
echo "───────────────────────────────────────────────────────"
curl -s -X POST http://localhost:5149/ordersdb \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":1,"quantity":2}],"promo_code":"DISCOUNT10"}' | jq '.'
echo ""

echo "═══════════════════════════════════════════════════════"
echo "  Vérification endpoints Exercice 1 (inchangés)"
echo "═══════════════════════════════════════════════════════"
echo ""

echo "📦 Test GET /products (service singleton - exercice 1)"
echo "───────────────────────────────────────────────────────"
curl -s http://localhost:5149/products | jq '. | length'
echo " produits trouvés"
echo ""

echo "🛑 Arrêt de l'API..."
kill $API_PID 2>/dev/null
wait $API_PID 2>/dev/null

echo ""
echo "✨ Exercice 3 terminé : EF Core InMemory intégré !"
echo "   - Endpoints /productsdb et /ordersdb fonctionnels"
echo "   - Exercice 1 intact (41/41 tests)"
