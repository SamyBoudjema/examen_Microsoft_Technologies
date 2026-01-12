#!/bin/bash

echo "======================================"
echo "Tests de l'API E-Commerce"
echo "======================================"
echo ""

echo "1️⃣  GET /products - Liste des produits"
curl -s http://localhost:5149/products | jq '.'
echo ""
echo ""

echo "2️⃣  POST /orders - Commande simple (remise 5% auto)"
curl -s -X POST http://localhost:5149/orders \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":1,"quantity":2}]}' | jq '.'
echo ""
echo ""

echo "3️⃣  POST /orders - Avec remise 10% sur produit (qty > 5)"
curl -s -X POST http://localhost:5149/orders \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":5,"quantity":6}]}' | jq '.'
echo ""
echo ""

echo "4️⃣  POST /orders - Avec code promo DISCOUNT20"
curl -s -X POST http://localhost:5149/orders \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":3,"quantity":1}],"promo_code":"DISCOUNT20"}' | jq '.'
echo ""
echo ""

echo "5️⃣  POST /orders - Remises cumulées (5% + 10% promo)"
curl -s -X POST http://localhost:5149/orders \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":1,"quantity":1}],"promo_code":"DISCOUNT10"}' | jq '.'
echo ""
echo ""

echo "❌ 6️⃣  POST /orders - Erreur: Produit inexistant"
curl -s -X POST http://localhost:5149/orders \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":999,"quantity":1}]}' | jq '.'
echo ""
echo ""

echo "❌ 7️⃣  POST /orders - Erreur: Stock insuffisant"
curl -s -X POST http://localhost:5149/orders \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":1,"quantity":100}]}' | jq '.'
echo ""
echo ""

echo "❌ 8️⃣  POST /orders - Erreur: Code promo invalide"
curl -s -X POST http://localhost:5149/orders \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":3,"quantity":1}],"promo_code":"INVALID"}' | jq '.'
echo ""
echo ""

echo "❌ 9️⃣  POST /orders - Erreur: Promo < 50€"
curl -s -X POST http://localhost:5149/orders \
  -H "Content-Type: application/json" \
  -d '{"products":[{"id":5,"quantity":1}],"promo_code":"DISCOUNT20"}' | jq '.'
echo ""
echo ""

echo "======================================"
echo "Tests terminés !"
echo "======================================"
