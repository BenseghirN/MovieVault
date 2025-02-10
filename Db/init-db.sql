-- Drop and recreate the database if it already exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'MovieVault')
DROP DATABASE MovieVault;
GO

-- Création de la base de données
CREATE DATABASE MovieVault;
GO
USE MovieVault;
GO

-- Table Users
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL
);
GO

-- Table Movies
CREATE TABLE Movies (
    MovieId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    ReleaseYear INT CHECK (ReleaseYear >= 1888),
    Duration INT CHECK (Duration > 0),
    Synopsis TEXT NULL,
    PosterUrl NVARCHAR(255) NULL -- Ajout de l'affiche du film
);
GO

-- Table People (Remplace Actors)
CREATE TABLE People (
    PersonId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    BirthDate DATE NULL,
    Nationality NVARCHAR(100) NULL,
    PhotoUrl NVARCHAR(255) NULL
);
GO

-- Table Genres
CREATE TABLE Genres (
    GenreId INT IDENTITY(1,1) PRIMARY KEY,
    GenreName NVARCHAR(50) NOT NULL UNIQUE
);
GO

-- Table MoviesGenres
CREATE TABLE MoviesGenres (
    MovieId INT NOT NULL,
    GenreId INT NOT NULL,
    PRIMARY KEY (MovieId, GenreId),
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE,
    FOREIGN KEY (GenreId) REFERENCES Genres(GenreId) ON DELETE CASCADE
);
GO

-- Table MoviesPeople (Remplace MoviesActors) avec RoleId en INT
CREATE TABLE MoviesPeople (
    MovieId INT NOT NULL,
    PersonId INT NOT NULL,
    Role TINYINT NOT NULL, -- Plus de table Roles, juste un INT
    PRIMARY KEY (MovieId, PersonId, Role),
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE,
    FOREIGN KEY (PersonId) REFERENCES People(PersonId) ON DELETE CASCADE
);
GO

-- Table UserMovies (Remplace MovieStatus)
CREATE TABLE UserMovies (
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    Status NVARCHAR(100) NULL, -- Vu, À voir, etc.
    Owned BIT DEFAULT 0, -- Indique si le film est possédé physiquement
    AddedDate DATE DEFAULT GETDATE(),
    LastWatched DATE NULL,
    PRIMARY KEY (UserId, MovieId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE
);
GO

-- Table Reviews
CREATE TABLE Reviews (
    ReviewId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    Comment TEXT NULL,
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    ReviewDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE
);
GO
