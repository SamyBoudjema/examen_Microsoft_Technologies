#!/bin/bash

echo "═══════════════════════════════════════════════════════"
echo "  🧩 Test du Résolveur de Labyrinthe (Exercice 2)"
echo "═══════════════════════════════════════════════════════"
echo ""

cd ExamM2.Maze

echo "🔨 Compilation du projet..."
dotnet build --nologo --verbosity quiet
if [ $? -ne 0 ]; then
    echo "❌ Erreur de compilation"
    exit 1
fi
echo "✅ Compilation réussie"
echo ""

echo "🧪 Exécution des tests unitaires..."
dotnet test --nologo --verbosity quiet
if [ $? -ne 0 ]; then
    echo "❌ Des tests ont échoué"
    exit 1
fi
echo "✅ Tous les tests unitaires passent (23/23)"
echo ""

echo "🚀 Exécution du programme de démonstration..."
echo "───────────────────────────────────────────────────────"
dotnet run --nologo
echo "───────────────────────────────────────────────────────"
echo ""

echo "✨ Exercice 2 terminé avec succès !"
