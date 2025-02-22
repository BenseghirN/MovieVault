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
    PasswordHash NVARCHAR(255) NOT NULL,
    isAdmin BIT DEFAULT 0 
);
GO

-- Table Movies
CREATE TABLE Movies (
    MovieId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    ReleaseYear INT CHECK (ReleaseYear >= 1888),
    Duration INT CHECK (Duration > 0),
    Synopsis TEXT NULL,
    PosterUrl NVARCHAR(255) NULL, -- Ajout de l'affiche du film
    TMDBId INT UNIQUE NULL
    -- CONSTRAINT UQ_Movies_Title_ReleaseYear UNIQUE (Title, ReleaseYear)
);
GO

-- Table People
CREATE TABLE People (
    PersonId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    BirthDate DATE NULL,
    Nationality NVARCHAR(100) NULL,
    PhotoUrl NVARCHAR(255) NULL,
    TMDBId INT UNIQUE NULL
);
GO

-- Table Genres
CREATE TABLE Genres (
    GenreId INT IDENTITY(1,1) PRIMARY KEY,
    GenreName NVARCHAR(50) NOT NULL UNIQUE,
    TMDBId INT UNIQUE NULL
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

-- Table MoviesPeople
CREATE TABLE MoviesPeople (
    MovieId INT NOT NULL,
    PersonId INT NOT NULL,
    Role TINYINT NOT NULL,
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
    Rating DECIMAL(3,1) CHECK (Rating BETWEEN 1 AND 5),
    ReviewDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE
);
GO
-- Insertion d'un utilisateur administrateur par défaut
INSERT INTO Users (UserName, Email, PasswordHash, isAdmin)
VALUES ('admin', 'admin', '$2a$12$.d3g5XfZHGkfgI5phqGUc.cWJeriV6VwkKz2DaAWMcCL5o0ELeR.y', 1);
GO