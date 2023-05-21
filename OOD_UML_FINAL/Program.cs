using OOD_PROJECT;
using System.Collections;
using System.Diagnostics;

namespace OOD_UML_FINAL
{
    class Program
    {
        static void Main(string[] args)
        {
            // MediaManager for ID management
            MediaManager mediaManager = new MediaManager();

            // Authors
            Author author1 = new Author { Name = "Francis",     Surname = "Coppola",        BirthYear = 1939,       Awards = 32 };
            Author author2 = new Author { Name = "Steven",      Surname = "Spielberg",      BirthYear = 1956,       Awards = 73 };
            Author author3 = new Author { Name = "Charlie",     Surname = "Chaplin",        BirthYear = 1889,       Awards = 6 };
            Author author4 = new Author { Name = "Vince",       Surname = "Gilligan",       BirthYear = 1967,       Awards = 17 };
            Author author5 = new Author { Name = "Rian",        Surname = "Johnson",        BirthYear = 1973,       Awards = 29 };
            Author author6 = new Author { Name = "Greg",        Surname = "Daniels",        BirthYear = 1963,       Awards = 5 };
            Author author7 = new Author { Name = "Troy",        Surname = "Miller",         BirthYear = 1960,       Awards = 0 };
            Author author8 = new Author { Name = "Victor",      Surname = "Nelli Jr.",      BirthYear = 1960,       Awards = 0 };
            Author author9 = new Author { Name = "Charles",     Surname = "McDougall",      BirthYear = 1960,       Awards = 0 };

            mediaManager.AddAuthor(1, author1);
            mediaManager.AddAuthor(2, author2);
            mediaManager.AddAuthor(3, author3);
            mediaManager.AddAuthor(4, author4);
            mediaManager.AddAuthor(5, author5);
            mediaManager.AddAuthor(6, author6);
            mediaManager.AddAuthor(7, author7);
            mediaManager.AddAuthor(8, author8);
            mediaManager.AddAuthor(9, author9);

            // Episodes
            Episode breakingBadFly = new Episode {          Title = "Fly",                                  Duration = 45,      ReleaseYear = 2010,     Director = author5 };
            Episode breakingBadOzymandias = new Episode {   Title = "Ozymandias",                           Duration = 50,      ReleaseYear = 2013,     Director = author5 };
            Episode breakingBadPilot = new Episode {        Title = "Pilot",                                Duration = 43,      ReleaseYear = 2008,     Director = author4 };

            Episode theOfficeUsDwightK = new Episode {      Title = "Dwight K. Schrute, (Acting) Manager",  Duration = 22,      ReleaseYear = 2011,     Director = author7 };
            Episode theCarpet = new Episode {               Title = "The Carpet",                           Duration = 23,      ReleaseYear = 2006,     Director = author8 };
            Episode dwightsSpeech = new Episode {           Title = "Dwight's Speech",                      Duration = 22,      ReleaseYear = 2006,     Director = author9 };

            mediaManager.AddEpisode(1, breakingBadFly);
            mediaManager.AddEpisode(2, breakingBadOzymandias);
            mediaManager.AddEpisode(3, breakingBadPilot);
            mediaManager.AddEpisode(4, theOfficeUsDwightK);
            mediaManager.AddEpisode(5, theCarpet);
            mediaManager.AddEpisode(6, dwightsSpeech);

            // Series
            Series breakingBad = new Series {   Title = "Breaking Bad",     Genre = "drama",    ShowRunner = author4,   Episodes = new List<Episode> { breakingBadFly, breakingBadOzymandias, breakingBadPilot } };
            Series theOfficeUS = new Series {   Title = "The Office US",    Genre = "horror",   ShowRunner = author6,   Episodes = new List<Episode> { theOfficeUsDwightK, theCarpet, dwightsSpeech } };

            mediaManager.AddSeries(1, breakingBad);
            mediaManager.AddSeries(2, theOfficeUS);

            // Movies
            Movie movie1 = new Movie {  Title = "Apocalypse Now",           Genre = "war film",     Director = author1,     ReleaseYear = 1979,         Duration = 174 };
            Movie movie2 = new Movie {  Title = "The Godfather",            Genre = "criminal",     Director = author1,     ReleaseYear = 1972,         Duration = 175 };
            Movie movie3 = new Movie {  Title = "Raiders of the lost ark",  Genre = "adventure",    Director = author2,     ReleaseYear = 1981,         Duration = 115 };
            Movie movie4 = new Movie {  Title = "The Great Dictator",       Genre = "comedy",       Director = author3,     ReleaseYear = 1940,         Duration = 125 };

            mediaManager.AddMovie(1, movie1);
            mediaManager.AddMovie(2, movie2);
            mediaManager.AddMovie(3, movie3);
            mediaManager.AddMovie(4, movie4);


            MovieT movie1t = new MovieT { data = "Apocalypse Now;war film(1979/174)@1" };
            MovieTAdapter movie1t_adapted = new MovieTAdapter(movie1t, mediaManager);

            SeriesT series1t = new SeriesT { data = "Breaking Bad;drama@4-(1)(2)(3)" };
            SeriesTAdapter series1t_adapted = new SeriesTAdapter(series1t, mediaManager);

            AuthorT author1t = new AuthorT { data = "Francis+Coppola+1939^32^" };
            AuthorTAdapter author1t_adapted = new AuthorTAdapter(author1t);

            GenericLT movie1lt = new GenericLT();
            movie1lt.AddField("Title", "Apocalypse Now");
            movie1lt.AddField("Genre", "war film");
            movie1lt.AddField("ReleaseYear", 1979);
            movie1lt.AddField("Duration", 174);
            movie1lt.AddField("DirectorId", 1);
            MovieLTAdapter movie1lt_adapted = new MovieLTAdapter(movie1lt, mediaManager);


            GenericLT series1lt = new GenericLT();
            series1lt.AddField("Title", "Breaking Bad");
            series1lt.AddField("Genre", "drama");
            series1lt.AddField("ShowRunnerId", 4);
            series1lt.AddField("EpisodeIds", new List<int> { 1, 2, 3 });
            SeriesLTAdapter series1lt_adapted = new SeriesLTAdapter(series1lt, mediaManager);

            GenericLT author1lt = new GenericLT();
            author1lt.AddField("Name", "Francis");
            author1lt.AddField("Surname", "Coppola");
            author1lt.AddField("BirthYear", 1939);
            author1lt.AddField("Awards", 32);
            AuthorLTAdapter author1lt_adapted = new AuthorLTAdapter(author1lt);


            List<IAuthor> authors_both = new List<IAuthor> { author1t_adapted, author1lt_adapted, author2, author3, author4, author5, author6, author7, author8, author9 };
            List<IMovie> movies_both = new List<IMovie> { movie1t_adapted, movie1lt_adapted, movie2, movie3, movie4 };
            List<ISeries> series_both = new List<ISeries> { series1t_adapted, series1lt_adapted, theOfficeUS };

            Dictionary<string, ICollection> collections = new Dictionary<string, ICollection>
            {
                { "Author", authors_both },
                { "Movie", movies_both },
                { "Series", series_both }
            };

            AuthorFilter authorFilter = new AuthorFilter(authors_both);
            MovieFilter movieFilter = new MovieFilter(movies_both);
            SeriesFilter seriesFilter = new SeriesFilter(series_both);

            Dictionary<string, IFilter> filters = new Dictionary<string, IFilter>()
            {
                { "Author", authorFilter },
                { "Movie", movieFilter },
                { "Series", seriesFilter }
            };

            AuthorAdder authorAdder = new AuthorAdder(authors_both);
            MovieAdder movieAdder = new MovieAdder(movies_both);
            SeriesAdder seriesAdder = new SeriesAdder(series_both);

            Dictionary<string, IAdder> adders = new Dictionary<string, IAdder>()
            {
                { "Author", authorAdder },
                { "Movie", movieAdder },
                { "Series", seriesAdder }
            };

            AuthorEditor authorEditor = new AuthorEditor(authors_both, authorFilter);
            MovieEditor movieEditor = new MovieEditor(movies_both, movieFilter);
            SeriesEditor seriesEditor = new SeriesEditor(series_both, seriesFilter);

            Dictionary<string, IEditor> editors = new Dictionary<string, IEditor>()
            {
                { "Author", authorEditor },
                { "Movie", movieEditor },
                { "Series", seriesEditor }
            };

            DeleteDirector<IAuthor> authorDelete = new DeleteDirector<IAuthor>(authorFilter);
            DeleteDirector<IMovie> movieDelete = new DeleteDirector<IMovie>(movieFilter);
            DeleteDirector<ISeries> seriesDelete = new DeleteDirector<ISeries>(seriesFilter);

            Dictionary<string, IDelete> deletes = new Dictionary<string, IDelete>
            {
                { "Author", authorDelete },
                { "Movie", movieDelete },
                { "Series", seriesDelete }
            };

            ListCommand listCommand = new ListCommand(collections);
            FindCommand findCommand = new FindCommand(filters);
            ExitCommand exitCommand = new ExitCommand();
            ClearCommand clearCommand = new ClearCommand();
            AddCommand addCommand = new AddCommand(adders);
            EditCommand editCommand = new EditCommand(editors);
            DeleteCommand deleteCommand = new DeleteCommand(deletes);

            CommandInvoker commandInvoker = new CommandInvoker();
            Queue<ICommand> commandQueue = new Queue<ICommand>();
            QueuePrintCommand queuePrintCommand = new QueuePrintCommand(commandQueue);
            QueueCommitCommand queueCommitCommand = new QueueCommitCommand(commandQueue);
            QueueExportCommand queueExportCommand = new QueueExportCommand(commandQueue);
            QueueDismissCommand queueDismissCommand = new QueueDismissCommand(commandQueue);
            QueueLoadCommand queueLoadCommand = new QueueLoadCommand(commandQueue, commandInvoker.commands);

            commandInvoker.RegisterCommand("list", listCommand);
            commandInvoker.RegisterCommand("exit", exitCommand);
            commandInvoker.RegisterCommand("find", findCommand);
            commandInvoker.RegisterCommand("clear", clearCommand);
            commandInvoker.RegisterCommand("add", addCommand);
            commandInvoker.RegisterCommand("edit", editCommand);
            commandInvoker.RegisterCommand("delete", deleteCommand);
            commandInvoker.RegisterCommand("queue print", queuePrintCommand);
            commandInvoker.RegisterCommand("queue commit", queueCommitCommand);
            commandInvoker.RegisterCommand("queue export", queueExportCommand);
            commandInvoker.RegisterCommand("queue dismiss", queueDismissCommand);
            commandInvoker.RegisterCommand("queue load", queueLoadCommand);
            commandInvoker.RegisterQueue(commandQueue);

            while (true) 
            {
                string projectName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().ProcessName);
                Console.Write($"./{projectName}:$ ");
                string input = Console.ReadLine();
                commandInvoker.ExecuteCommand(input);
            }

        }
    }
}