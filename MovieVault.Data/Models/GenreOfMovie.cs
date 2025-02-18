public static class GenreOfMovie
{
    public static readonly Dictionary<int, string> Genres = new()
    {
        { 28, "Action" },
        { 12, "Aventure" },
        { 16, "Animation" },
        { 35, "Comédie" },
        { 80, "Crime" },
        { 99, "Documentaire" },
        { 18, "Drame" },
        { 10751, "Famille" },
        { 14, "Fantaisie" },
        { 36, "Histoire" },
        { 27, "Horreur" },
        { 10402, "Musique" },
        { 9648, "Mystère" },
        { 10749, "Romance" },
        { 878, "Science-Fiction" },
        { 10770, "Téléfilm" },
        { 53, "Thriller" },
        { 10752, "Guerre" },
        { 37, "Western" }
    };

    public static string GetGenreName(int genreId) =>
        Genres.TryGetValue(genreId, out var genreName) ? genreName : "Inconnu";

    public static bool GenreExists(int genreId) =>
        Genres.ContainsKey(genreId);

    public static string GetGenreNameByTMDBId(int TMDBId) =>
Genres.TryGetValue(TMDBId, out var genreName) ? genreName : "Inconnu";
}