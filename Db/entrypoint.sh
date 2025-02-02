#!/bin/bash

echo "🚀 Démarrage de SQL Server et exécution des scripts..."
/usr/src/app/run-initialization.sh & /opt/mssql/bin/sqlservr
