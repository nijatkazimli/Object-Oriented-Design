using System.Text.RegularExpressions;

namespace OOD_UML_FINAL
{
    public class MovieTAdapter : IMovie, IDisplay
    {
        public string Title
        {
            get
            {
                var parts = movieText.data.Split(new char[] { ';', '(', '/', ')', '@' }, StringSplitOptions.RemoveEmptyEntries);
                return parts[0];
            } 
            set
            {
                movieText.data = Regex.Replace(movieText.data, @"^[^;]+", value);
            }
        }
        public string Genre
        {
            get 
            {
                var parts = movieText.data.Split(new char[] { ';', '(', '/', ')', '@' }, StringSplitOptions.RemoveEmptyEntries);
                return parts[1];
            }
            set 
            {
                movieText.data = Regex.Replace(movieText.data, @";([^()]+)\(",";" + value+ ";(");
            }
        }
        public int ReleaseYear
        { 
            get
            {
                var parts = movieText.data.Split(new char[] { ';', '(', '/', ')', '@' }, StringSplitOptions.RemoveEmptyEntries);
                return int.Parse(parts[2]);
            }
            set
            {
                movieText.data = Regex.Replace(movieText.data, @"\((\d+)\/", $"({value}/");
            }
        }
        public Author Director
        {
            get
            {
                var parts = movieText.data.Split(new char[] { ';', '(', '/', ')', '@' }, StringSplitOptions.RemoveEmptyEntries);
                return mediaManager.GetAuthorById(int.Parse(parts[4]));
            }
            set
            {
                int id = (int)mediaManager.GetIdByAuthor(value);
                movieText.data = Regex.Replace(movieText.data, @"@\d+", "@" + id.ToString());
            }
        }
        public int Duration
        { 
            get
            {
                var parts = movieText.data.Split(new char[] { ';', '(', '/', ')', '@' }, StringSplitOptions.RemoveEmptyEntries);
                return int.Parse(parts[3]);
            } 
            set
            {
                movieText.data = Regex.Replace(movieText.data, @"\((\d+)\/\d+\)", $"($1/{value})");
            }
        }
        MovieT movieText { get; set; }
        MediaManager mediaManager;
        public MovieTAdapter(MovieT movieText, MediaManager mediaManager)
        {
            this.mediaManager = mediaManager;
            this.movieText = movieText;
        }

        public override string ToString()
        {
            return $"{Title}, {Genre}, {Director.Name} {Director.Surname}, {Duration}m, {ReleaseYear}";
        }
        public void Task2()
        {
            if (Director.BirthYear > 1970)
                Console.WriteLine($"{Title}, {Genre}, {Director.Name} {Director.Surname}, {ReleaseYear}");
        }
        public void Display()
        {
            Console.WriteLine("{0,-25} {1,-10} {2,-7} {3,-7}", Title, Genre, ReleaseYear, Duration);
        }
    }

    public class SeriesTAdapter : ISeries, IDisplay
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public Author ShowRunner { get; set; }
        public List<Episode> Episodes { get; set; }
        public SeriesT seriesText { get; set; }
        MediaManager mediaManager;
        public SeriesTAdapter(SeriesT seriesText, MediaManager mediaManager)
        {
            Episodes = new List<Episode>();
            this.mediaManager = mediaManager;
            this.seriesText = seriesText;
            var parts = seriesText.data.Split(new char[] { ';', '@', '(', ')', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);
            Title = parts[0];
            Genre = parts[1];
            ShowRunner = mediaManager.GetAuthorById(int.Parse(parts[2]));
            List<int> episodeIds = parts.Skip(3).Select(int.Parse).ToList();
            List<Episode> episodes = new List<Episode>(); // Create a new list of episodes
            foreach (int episodeId in episodeIds)
            {
                episodes.Add(mediaManager.GetEpisodeById(episodeId)); // Add episodes to the new list
            }
            Episodes = episodes; // Return the populated list
        }

        public override string ToString()
        {
            var result = $"{Title}, {Genre}, {ShowRunner.Name} {ShowRunner.Surname}";
            int i = 0;
            foreach (Episode episode in Episodes)
            {
                result += "\n";
                result += "\t" + ++i + ". ";
                result += episode.ToString();
            }
            return result;
        }

        public void Task2()
        {
            bool flag = false;
            foreach (Episode episode in Episodes)
            {
                if (episode.Director.BirthYear > 1970)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                Console.WriteLine($"{Title}, {Genre}, {ShowRunner.Name} {ShowRunner.Surname}");
                foreach (Episode episode in Episodes)
                {
                    Console.WriteLine($"\t{Title}, {episode.Director.Name} {episode.Director.Surname}, {episode.ReleaseYear}");
                }
            }

        }

        public void Display()
        {
            Console.WriteLine("{0,-25} {1,-10}", Title, Genre);
        }
    }

    public class EpsiodeTAdapter : IEpisode, IDisplay
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public int ReleaseYear { get; set; }
        public Author Director { get; set; }
        //public EpisodeT episodeText { get; set; }
        public EpsiodeTAdapter(EpisodeT episodeText, MediaManager mediaManager)
        {
            var parts = episodeText.data.Split(new char[] { '/', '(', ';', ')' }, StringSplitOptions.RemoveEmptyEntries);
            Title = parts[0];
            Duration = int.Parse(parts[1]);
            ReleaseYear = int.Parse(parts[2]);
            Director = mediaManager.GetAuthorById(int.Parse(parts[3]));
        }

        public void Display()
        {
            Console.WriteLine($"{Title}, {Duration}, {ReleaseYear}");
        }

        public override string ToString()
        {
            return $"{Title}, {Duration}, {ReleaseYear}, {Director.Name} {Director.Surname}";
        }
    }

    public class AuthorTAdapter : IAuthor, IDisplay
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BirthYear { get; set; }
        public int Awards { get; set; }
        //public AuthorT authorText { get; set; }
        public AuthorTAdapter(AuthorT authorText)
        {
            var parts = authorText.data.Split(new char[] { '+', '^' }, StringSplitOptions.RemoveEmptyEntries);
            Name = parts[0];
            Surname = parts[1];
            BirthYear = int.Parse(parts[2]);
            Awards = int.Parse(parts[3]);
        }

        public override string ToString()
        {
            return $"{Name}, {Surname}, {BirthYear}, {Awards}";
        }

        public void Display()
        {
            Console.WriteLine("{0,-15} {1,-15} {2,-7} {3,-5}", Name, Surname, BirthYear, Awards);
        }
    }

    public class MovieLTAdapter : IMovie, IDisplay
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public Author Director { get; set; }
        public int Duration { get; set; }
        //public GenericLT movieLT { get; set; }

        public MovieLTAdapter(GenericLT movieLT, MediaManager mediaManager)
        {
            Title = (string)movieLT.GetFieldValue("Title");
            Genre = (string)movieLT.GetFieldValue("Genre");
            ReleaseYear = Convert.ToInt32(movieLT.GetFieldValue("ReleaseYear"));
            Duration = Convert.ToInt32(movieLT.GetFieldValue("Duration"));
            if(mediaManager is not null)
                Director = mediaManager.GetAuthorById((int)movieLT.GetFieldValue("DirectorId"));
        }

        public override string ToString()
        {
            return $"{Title}, {Genre}, {Director.Name} {Director.Surname}, {Duration}m, {ReleaseYear}";
        }
        public void Task2()
        {
            if (Director.BirthYear > 1970)
                Console.WriteLine($"{Title}, {Genre}, {Director.Name} {Director.Surname}, {ReleaseYear}");
        }
        public void Display()
        {
            Console.WriteLine("{0,-25} {1,-10} {2,-7} {3,-7}", Title, Genre, ReleaseYear, Duration);
        }
    }

    public class SeriesLTAdapter : ISeries, IDisplay
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public Author ShowRunner { get; set; }
        public List<Episode> Episodes { get; set; }
        //public GenericLT seriesLT { get; set; }
        public SeriesLTAdapter(GenericLT seriesLT, MediaManager mediaManager)
        {
            Episodes = new List<Episode>();
            Title = (string)seriesLT.GetFieldValue("Title");
            Genre = (string)seriesLT.GetFieldValue("Genre");
            if (mediaManager is not null)
            {
                ShowRunner = mediaManager.GetAuthorById((int)seriesLT.GetFieldValue("ShowRunnerId"));
                List<int> episodeIds = (List<int>)seriesLT.GetFieldValue("EpisodeIds");
                foreach (int id in episodeIds)
                {
                    Episodes.Add(mediaManager.GetEpisodeById(id));
                }
            }
        }


        public void Display()
        {
            Console.WriteLine("{0,-25} {1,-10}", Title, Genre);
        }

        public override string ToString()
        {
            var result = $"{Title}, {Genre}, {ShowRunner.Name} {ShowRunner.Surname}";
            int i = 0;
            foreach (Episode episode in Episodes)
            {
                result += "\n";
                result += "\t" + ++i + ". ";
                result += episode.ToString();
            }
            return result;
        }

        public void Task2()
        {
            bool flag = false;
            foreach (Episode episode in Episodes)
            {
                if (episode.Director.BirthYear > 1970)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                Console.WriteLine($"{Title}, {Genre}, {ShowRunner.Name} {ShowRunner.Surname}");
                foreach (Episode episode in Episodes)
                {
                    Console.WriteLine($"\t{Title}, {episode.Director.Name} {episode.Director.Surname}, {episode.ReleaseYear}");
                }
            }

        }
    }

    public class EpisodeLTAdapter : IEpisode, IDisplay
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public int ReleaseYear { get; set; }
        public Author Director { get; set; }
        //public GenericLT episodeLT { get; set; }

        public EpisodeLTAdapter(GenericLT episodeLT, MediaManager mediaManager)
        {
            Title = (string)episodeLT.GetFieldValue("Title");
            Duration = Convert.ToInt32(episodeLT.GetFieldValue("Duration"));
            ReleaseYear = Convert.ToInt32(episodeLT.GetFieldValue("ReleaseYear"));
            Director = mediaManager.GetAuthorById((int)episodeLT.GetFieldValue("DirectorId"));
        }
        public void Display()
        {
            Console.WriteLine($"{Title}, {Duration}, {ReleaseYear}");
        }

        public override string ToString()
        {
            return $"{Title}, {Duration}, {ReleaseYear}, {Director.Name} {Director.Surname}";
        }
    }

    public class AuthorLTAdapter : IAuthor, IDisplay
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BirthYear { get; set; }
        public int Awards { get; set; }
        //public GenericLT authorLT { get; set; }

        public AuthorLTAdapter(GenericLT authorLT)
        {
            Name = (string)authorLT.GetFieldValue("Name");
            Surname = (string)authorLT.GetFieldValue("Surname");
            BirthYear = Convert.ToInt32(authorLT.GetFieldValue("BirthYear"));
            Awards = Convert.ToInt32(authorLT.GetFieldValue("Awards"));
        }
        public override string ToString()
        {
            return $"{Name}, {Surname}, {BirthYear}, {Awards}";
        }

        public void Display()
        {
            Console.WriteLine("{0,-15} {1,-15} {2,-7} {3,-5}", Name, Surname, BirthYear, Awards);
        }
    }
}
