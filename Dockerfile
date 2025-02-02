# Utiliser SQL Server 2022 sur Ubuntu
FROM mcr.microsoft.com/mssql/server:2022-latest

# Définir le répertoire de travail
WORKDIR /usr/src/app

# Copier le dossier Db contenant les scripts SQL
COPY ./Db /usr/src/app

# Définir les variables d'environnement pour SQL Server
ENV SA_PASSWORD=K]xr!9*a>sPw
ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Developer

# Exposer le port SQL Server
EXPOSE 1433

# Lancer SQL Server et les scripts d'initialisation en parallèle
CMD /bin/bash ./entrypoint.sh
