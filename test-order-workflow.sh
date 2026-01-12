#!/bin/bash

echo "======================================"
echo "🧪 Test workflow commande Exercice 3"
echo "======================================"
echo ""

echo "📦 1. État initial de la BDD"
echo "-----------------------------------"
curl -s http://localhost:5149/api/debug/database | jq '.'
echo ""
echo ""

echo "🛒 2. Création d'une commande"
echo "-----------------------------------"
echo "Commande: 2x Product A avec DISCOUNT10"
curl -s -X POST http://localhost:5149/ordersdb \
  -H "Content-Type: application/json" \
  -d '{"products":[{"productId":1,"quantity":2}],"promoCode":"DISCOUNT10"}' | jq '.'
echo ""
echo ""

echo "📦 3. État de la BDD après commande"
echo "-----------------------------------"
curl -s http://localhost:5149/api/debug/database | jq '.'
echo ""
echo ""

echo "✅ Vérification: Le stock de Product A devrait avoir diminué de 2"
