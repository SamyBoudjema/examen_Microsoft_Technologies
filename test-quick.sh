#!/bin/bash

echo "======================================"
echo "🧪 Tests rapides Exercice 3"
echo "======================================"
echo ""

echo "1️⃣ GET /api/productsdb (liste produits DB)"
echo "-------------------------------------------"
curl -s http://localhost:5149/api/productsdb | jq '.'
echo ""
echo ""

echo "2️⃣ POST /api/ordersdb (créer commande)"
echo "-------------------------------------------"
curl -s -X POST http://localhost:5149/api/ordersdb \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":1,"quantity":2}],"promoCode":"DISCOUNT10"}' | jq '.'
echo ""
echo ""

echo "3️⃣ GET /api/productsdb (vérifier stock après)"
echo "-------------------------------------------"
curl -s http://localhost:5149/api/productsdb | jq '.'
echo ""
echo ""

echo "✅ Comparez le stock de Product A avant/après"
