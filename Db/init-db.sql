-- Drop and recreate the database
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'MovieVault')
DROP DATABASE MovieVault;
GO

CREATE DATABASE MovieVault;
GO

USE MovieVault;
GO

-- Table Movies
CREATE TABLE Movies (
    MovieId INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    ReleaseYear INT CHECK (ReleaseYear >= 1888),
    Director NVARCHAR(255),
    Duration INT CHECK (Duration > 0),
    Owned BIT DEFAULT 0
);
GO

-- Table Users
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL
);
GO

-- Table MovieStatus (Many-to-Many relationship between Movies & Users)
CREATE TABLE MovieStatus (
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    Status INT DEFAULT 0,
    PRIMARY KEY (UserId, MovieId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE
);
GO

-- Table Reviews (Users can leave a review for a movie)
CREATE TABLE Reviews (
    ReviewId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    ReviewText NVARCHAR(MAX),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    ReviewDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE
);
GO

-- Table Genres (Movie categories: action, comedy, etc.)
CREATE TABLE Genres (
    GenreId INT IDENTITY(1,1) PRIMARY KEY,
    GenreName NVARCHAR(50) UNIQUE NOT NULL
);
GO

-- Table MovieGenres (Many-to-Many relationship between Movies & Genres)
CREATE TABLE MovieGenres (
    MovieId INT NOT NULL,
    GenreId INT NOT NULL,
    PRIMARY KEY (MovieId, GenreId),
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE,
    FOREIGN KEY (GenreId) REFERENCES Genres(GenreId) ON DELETE CASCADE
);
GO

-- Table Actors (List of actors)
CREATE TABLE Actors (
    ActorId INT IDENTITY(1,1) PRIMARY KEY,
    ActorName NVARCHAR(255) NOT NULL,
    BirthDate DATE,
    Nationality NVARCHAR(100)
);
GO

-- Table MovieActors (Many-to-Many relationship between Movies & Actors)
CREATE TABLE MovieActors (
    MovieId INT NOT NULL,
    ActorId INT NOT NULL,
    PRIMARY KEY (MovieId, ActorId),
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE,
    FOREIGN KEY (ActorId) REFERENCES Actors(ActorId) ON DELETE CASCADE
);
GO

-- 📌 Insert initial data 📌
INSERT INTO Movies (Title, ReleaseYear, Director, Duration, Owned) VALUES
('Inception', 2010, 'Christopher Nolan', 148, 1),
('The Matrix', 1999, 'Lana & Lilly Wachowski', 136, 1),
('Interstellar', 2014, 'Christopher Nolan', 169, 1),
('The Godfather', 1972, 'Francis Ford Coppola', 175, 1);
GO

INSERT INTO Users (Name, Email) VALUES
('Alice', 'alice@example.com'),
('Bob', 'bob@example.com');
GO

INSERT INTO Genres (GenreName) VALUES
('Science-fiction'),
('Action'),
('Drama'),
('Thriller');
GO

INSERT INTO MovieGenres (MovieId, GenreId) VALUES
(1, 1), -- Inception -> Science-fiction
(2, 1), -- The Matrix -> Science-fiction
(3, 1), -- Interstellar -> Science-fiction
(3, 4), -- Interstellar -> Thriller
(4, 3); -- The Godfather -> Drama
GO

INSERT INTO Actors (ActorName, BirthDate, Nationality) VALUES
('Leonardo DiCaprio', '1974-11-11', 'American'),
('Keanu Reeves', '1964-09-02', 'Canadian'),
('Al Pacino', '1940-04-25', 'American');
GO

INSERT INTO MovieActors (MovieId, ActorId) VALUES
(1, 1), -- Inception -> Leonardo DiCaprio
(2, 2), -- The Matrix -> Keanu Reeves
(4, 3); -- The Godfather -> Al Pacino
GO

INSERT INTO MovieStatus (UserId, MovieId, Status) VALUES
(1, 1, 1), -- Alice watched Inception
(1, 3, 0), -- Alice has not watched Interstellar
(2, 2, 1); -- Bob watched The Matrix
GO

INSERT INTO Reviews (UserId, MovieId, ReviewText, Rating) VALUES
(1, 1, 'Amazing movie, very well directed!', 5),
(2, 2, 'A classic of cinema!', 5);
GO
