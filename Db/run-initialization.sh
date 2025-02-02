#!/bin/bash

echo "⏳ Attente de SQL Server pour s'assurer qu'il est prêt..."
sleep 30s

echo "🚀 Exécution des scripts SQL d'initialisation..."

# Exécuter tous les fichiers SQL dans l'ordre
for f in /usr/src/app/*.sql; do
    echo "➡️ Exécution de $f..."
    /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U SA -P 'K]xr!9*a>sPw' -d master -i "$f"
done

echo "✅ Initialisation terminée !"
