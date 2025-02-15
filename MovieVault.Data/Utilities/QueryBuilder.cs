using Microsoft.Data.SqlClient;

namespace MovieVault.Data.Utilities
{
    public static class QueryBuilder
    {
        public static List<string> BuildJoins(string? genre, IEnumerable<string>? directors, IEnumerable<string>? actors)
        {
            var joins = new List<string>();

            if (!string.IsNullOrEmpty(genre))
            {
                joins.Add("LEFT JOIN MoviesGenres mg ON m.MovieId = mg.MovieId LEFT JOIN Genres g ON mg.GenreId = g.GenreId");
            }

            if ((directors != null && directors.Any()) || (actors != null && actors.Any()))
            {
                joins.Add("LEFT JOIN MoviePeople mp ON m.MovieId = mp.MovieId LEFT JOIN People p ON mp.PersonId = p.PersonId");
            }

            return joins;
        }

        public static List<string> BuildConditions(string? title, IEnumerable<int>? years, string? genre, IEnumerable<string>? directors, IEnumerable<string>? actors, List<SqlParameter> parameters)
        {
            var conditions = new List<string>();

            if (!string.IsNullOrEmpty(title))
            {
                conditions.Add("m.Title LIKE @Title");
                parameters.Add(new SqlParameter("@Title", $"%{title}%"));
            }

            if (years != null && years.Any())
            {
                var yearParams = years.Select((year, index) => new SqlParameter($"@Year{index}", year)).ToList();
                conditions.Add($"m.ReleaseYear IN ({string.Join(", ", yearParams.Select(p => p.ParameterName))})");
                parameters.AddRange(yearParams);
            }

            if (!string.IsNullOrEmpty(genre))
            {
                conditions.Add("g.GenreName = @Genre");
                parameters.Add(new SqlParameter("@Genre", genre));
            }

            if (directors != null && directors.Any())
            {
                var directorParams = directors.Select((director, index) => new SqlParameter($"@Director{index}", director)).ToList();
                conditions.Add($"p.FullName IN ({string.Join(", ", directorParams.Select(p => p.ParameterName))}) AND mp.RoleId = 1");
                parameters.AddRange(directorParams);
            }

            if (actors != null && actors.Any())
            {
                var actorParams = actors.Select((actor, index) => new SqlParameter($"@Actor{index}", actor)).ToList();
                conditions.Add($"p.FullName IN ({string.Join(", ", actorParams.Select(p => p.ParameterName))}) AND mp.RoleId = 2");
                parameters.AddRange(actorParams);
            }

            return conditions;
        }
    }
}