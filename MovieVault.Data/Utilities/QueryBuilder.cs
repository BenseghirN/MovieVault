using Microsoft.Data.SqlClient;

namespace MovieVault.Data.Utilities
{
    public static class QueryBuilder
    {
        public static List<string> BuildJoins(IEnumerable<string>? genres, IEnumerable<string>? directors, IEnumerable<string>? actors)
        {
            var joins = new List<string>();

            if (genres != null && genres.Any())
            {
                joins.Add("LEFT JOIN MoviesGenres mg ON m.MovieId = mg.MovieId LEFT JOIN Genres g ON mg.GenreId = g.GenreId");
            }

            if (directors != null && directors.Any())
            {
                joins.Add("LEFT JOIN MoviePeople mp_director ON m.MovieId = mp_director.MovieId AND mp_director.Role = 1 LEFT JOIN People p_director ON mp_director.PersonId = p_director.PersonId");
            }

            if (actors != null && actors.Any())
            {
                joins.Add("LEFT JOIN MoviePeople mp_actor ON m.MovieId = mp_actor.MovieId AND mp_actor.Role = 2 LEFT JOIN People p_actor ON mp_actor.PersonId = p_actor.PersonId");
            }

            return joins;
        }

        public static List<string> BuildConditions(string? title, IEnumerable<int>? years, IEnumerable<string>? genres, IEnumerable<string>? directors, IEnumerable<string>? actors, List<SqlParameter> parameters)
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

            if (genres != null && genres.Any())
            {
                conditions.Add($"g.GenreName IN ({string.Join(",", genres.Select((g, i) => $"@Genre{i}"))})");
                for (int i = 0; i < genres.Count(); i++)
                {
                    parameters.Add(new SqlParameter($"@Genre{i}", genres.ElementAt(i)));
                }
            }

            if (directors != null && directors.Any())
            {
                conditions.Add($"p_director.FirstName + ' ' + p_director.LastName IN ({string.Join(",", directors.Select((d, i) => $"@Director{i}"))}) AND mp_director.Role = 1");
                for (int i = 0; i < directors.Count(); i++)
                {
                    parameters.Add(new SqlParameter($"@Director{i}", directors.ElementAt(i)));
                }
            }

            if (actors != null && actors.Any())
            {
                conditions.Add($"p_actor.FirstName + ' ' + p_actor.LastName IN ({string.Join(",", actors.Select((a, i) => $"@Actor{i}"))}) AND mp_actor.Role = 2");
                for (int i = 0; i < actors.Count(); i++)
                {
                    parameters.Add(new SqlParameter($"@Actor{i}", actors.ElementAt(i)));
                }
            }

            return conditions;
        }
    }
}