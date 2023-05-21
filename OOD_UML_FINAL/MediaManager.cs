namespace OOD_UML_FINAL
{
    public class MediaManager
    {
        public Dictionary<int, Author> _authorsById;
        public Dictionary<int, Movie> _moviesById;
        public Dictionary<int, Series> _seriesById;
        public Dictionary<int, Episode> _episodesById;

        public MediaManager()
        {
            _authorsById = new Dictionary<int, Author>();
            _moviesById = new Dictionary<int, Movie>();
            _seriesById = new Dictionary<int, Series>();
            _episodesById = new Dictionary<int, Episode>();
        }

        public void AddAuthor(int id, Author author)
        {
            _authorsById[id] = author;
        }

        public Author GetAuthorById(int id)
        {
            return _authorsById.TryGetValue(id, out Author author) ? author : null;
        }
        public int? GetIdByAuthor(Author author)
        {
            foreach (KeyValuePair<int, Author> entry in _authorsById)
            {
                if (entry.Value == author)
                {
                    return entry.Key;
                }
            }

            return null;
        }

        public void AddMovie(int id, Movie movie)
        {
            _moviesById[id] = movie;
        }

        public Movie GetMovieById(int id)
        {
            return _moviesById.TryGetValue(id, out Movie movie) ? movie : null;
        }

        public void AddSeries(int id, Series series)
        {
            _seriesById[id] = series;
        }

        public Series GetSeriesById(int id)
        {
            return _seriesById.TryGetValue(id, out Series series) ? series : null;
        }

        public void AddEpisode(int id, Episode episode)
        {
            _episodesById[id] = episode;
        }

        public Episode GetEpisodeById(int id)
        {
            return _episodesById.TryGetValue(id, out Episode episode) ? episode : null;
        }
        public int? GetIdByEpisode(Episode episode) 
        {
            foreach (KeyValuePair<int, Episode> entry in _episodesById)
            {
                if (entry.Value == episode)
                {
                    return entry.Key;
                }
            }

            return null;
        }

    }
}
