namespace OOD_UML_FINAL
{

    public interface IAdder 
    {
        void Add(string[] arguments);
    }

    public abstract class Builder<T> 
    {
        protected Dictionary<string, Action<string>> fieldBuilders;
        public void BuildField(string fieldName, string value) 
        {
            fieldBuilders[fieldName](value);
        }

        public abstract T Build();
        public abstract void Reset();
    }

    public class BuildDirector<T> : IAdder 
    {
        protected Dictionary<string, Builder<T>> builders;
        protected ICollection<T> collection;
        protected List<string> field_names;

        public void Add(string[] arguments)
        {
            string class_name = arguments[0];
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Available fields:]");
            foreach(var item in field_names) 
            {
                Console.Write(item + " ");
            }
            Console.ForegroundColor = previousColor;
            Console.WriteLine();
            while (true)
            {
                string command_input = Console.ReadLine();
                if (command_input == "EXIT")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{class_name} creation abandoned]");
                    Console.ForegroundColor = previousColor;
                    return;
                }
                if (command_input == "DONE")
                {
                    break;
                }
                int equals_index = command_input.IndexOf('=');
                if (equals_index == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Usage: <name_of_field>=<value>]");
                    Console.ForegroundColor = previousColor;
                    continue;
                }
                string name_of_field = command_input.Substring(0, equals_index);
                string value = command_input.Substring(equals_index + 1);
                if (!field_names.Contains(name_of_field))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Incorrect field name]");
                    Console.ForegroundColor = previousColor;
                    continue;
                }
                try 
                {
                    builders[arguments[1]].BuildField(name_of_field, value);
                }
                catch (Exception ex) 
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = previousColor;
                    continue;
                }
            }
            collection.Add(builders[arguments[1]].Build());
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{class_name} created]");
            Console.ForegroundColor = previousColor;
            builders[arguments[1]].Reset();
        }
    }

    public class AuthorAdder : BuildDirector<IAuthor>
    {
        public AuthorAdder(ICollection<IAuthor> collection)
        {
            base.collection = collection;

            Dictionary<string, Builder<IAuthor>> builders = new Dictionary<string, Builder<IAuthor>>();
            builders.Add("base", new BaseAuthorBuilder());
            builders.Add("secondary", new SecondaryAuthorBuilder());
            base.builders = builders;

            List<string> field_names = new List<string> { "Name", "Surname", "BirthYear", "Awards" };
            base.field_names = field_names;

        }
    }

    public class MovieAdder : BuildDirector<IMovie>
    {
        public MovieAdder(ICollection<IMovie> collection)
        {
            base.collection = collection;

            Dictionary<string, Builder<IMovie>> builders = new Dictionary<string, Builder<IMovie>>();
            builders.Add("base", new BaseMovieBuilder());
            builders.Add("secondary", new SecondaryMovieBuilder());
            base.builders = builders;

            List<string> field_names = new List<string> { "Title", "Genre", "ReleaseYear", "Duration" };
            base.field_names = field_names;
        }
    }

    public class SeriesAdder : BuildDirector<ISeries>
    {
        public SeriesAdder(ICollection<ISeries> collection)
        {
            base.collection = collection;

            Dictionary<string, Builder<ISeries>> builders = new Dictionary<string, Builder<ISeries>>();
            builders.Add("base", new BaseSeriesBuilder());
            builders.Add("secondary", new SecondarySeriesBuilder());
            base.builders = builders;

            List<string> field_names = new List<string> { "Title", "Genre" };
            base.field_names = field_names;
        }
    }

    public class BaseAuthorBuilder : Builder<IAuthor> 
    {
        private Author author;
        public BaseAuthorBuilder() 
        {
            author = new Author();
            fieldBuilders = new Dictionary<string, Action<string>>
            {
                    { "Name", BuildName },
                    { "Surname", BuildSurname },
                    { "BirthYear", BuildBirthYear },
                    { "Awards", BuildAwards }
            };
        }
        public void BuildName(string name)
        {
            author.Name = name;
        }
        public void BuildSurname(string surname)
        {
            author.Surname = surname;
        }
        public void BuildBirthYear(string birthYear)
        {
            author.BirthYear = int.TryParse(birthYear, out int BirthYear) ? BirthYear : throw new Exception("[Incorrect value to set BirthYear!]");
        }

        public void BuildAwards(string awards)
        {
            author.Awards = int.TryParse(awards, out int Awards) ? Awards : throw new Exception("[Incorrect value to set Awards!]");
        }

        public override IAuthor Build() => author;

        public override void Reset() => author = new Author();
    }

    public class SecondaryAuthorBuilder : Builder<IAuthor> 
    {
        public GenericLT author;
        public SecondaryAuthorBuilder() 
        {
            author = new GenericLT();
            fieldBuilders = new Dictionary<string, Action<string>>
            {
                    { "Name", BuildName },
                    { "Surname", BuildSurname },
                    { "BirthYear", BuildBirthYear },
                    { "Awards", BuildAwards }
            };
        }
        public void BuildName(string name)
        {
            author.AddField("Name", name);
        }

        public void BuildSurname(string surname)
        {
            author.AddField("Surname", surname);
        }

        public void BuildBirthYear(string birthYear)
        {
            author.AddField("BirthYear", int.TryParse(birthYear, out int BirthYear) ? BirthYear : throw new Exception("[Incorrect value to set BirthYear!]"));
        }

        public void BuildAwards(string awards)
        {
            author.AddField("Awards", int.TryParse(awards, out int Awards) ? Awards : throw new Exception("[Incorrect value to set Awards!]"));
        }

        public override IAuthor Build() 
        {
            AuthorLTAdapter adapted = new AuthorLTAdapter(author);
            return adapted;
        }

        public override void Reset() => author = new GenericLT();
    }

    public class BaseMovieBuilder : Builder<IMovie>
    {
        private Movie movie;
        public BaseMovieBuilder()
        {
            movie = new Movie();
            fieldBuilders = new Dictionary<string, Action<string>>
            {
                    { "Title", BuildTitle },
                    { "Genre", BuildGenre },
                    { "Duration", BuildDuration },
                    { "ReleaseYear", BuildReleaseYear }
            };
        }
        public void BuildTitle(string title)
        {
            movie.Title = title;
        }
        public void BuildGenre(string genre)
        {
            movie.Genre = genre;
        }
        public void BuildDuration(string duration)
        {
            movie.Duration = int.TryParse(duration, out int Duration) ? Duration : throw new Exception("[Incorrect value to set Duration!]");
        }

        public void BuildReleaseYear(string releaseYear)
        {
            movie.ReleaseYear = int.TryParse(releaseYear, out int ReleaseYear) ? ReleaseYear : throw new Exception("[Incorrect value to set ReleaseYear!]");
        }

        public override IMovie Build() => movie;

        public override void Reset() => movie = new Movie();
    }


    public class SecondaryMovieBuilder : Builder<IMovie> 
    {
        private GenericLT movie;
        public SecondaryMovieBuilder() 
        {
            movie = new GenericLT();
            fieldBuilders = new Dictionary<string, Action<string>>
            {
                    { "Title", BuildTitle },
                    { "Genre", BuildGenre },
                    { "Duration", BuildDuration },
                    { "ReleaseYear", BuildReleaseYear }
            };
        }
        public void BuildTitle(string title)
        {
            movie.AddField("Title", title);
        }
        public void BuildGenre(string genre)
        {
            movie.AddField("Genre", genre);
        }
        public void BuildDuration(string duration)
        {
            movie.AddField("Durarion", int.TryParse(duration, out int Duration) ? Duration : throw new Exception("[Incorrect value to set Duration!]"));
        }

        public void BuildReleaseYear(string releaseYear)
        {
            movie.AddField("ReleaseYear", int.TryParse(releaseYear, out int ReleaseYear) ? ReleaseYear : throw new Exception("[Incorrect value to set ReleaseYear!]"));
        }

        public override IMovie Build() 
        {
            MovieLTAdapter movieLTAdapter = new MovieLTAdapter(movie, null);
            return movieLTAdapter;

        }

        public override void Reset() => movie = new GenericLT();
    }

    public class BaseSeriesBuilder : Builder<ISeries> 
    {
        private Series series;

        public BaseSeriesBuilder() 
        {
            series = new Series();
            fieldBuilders = new Dictionary<string, Action<string>>
            {
                    { "Title", BuildTitle },
                    { "Genre", BuildGenre }
            };
        }

        public void BuildTitle(string title)
        {
            series.Title = title;
        }
        public void BuildGenre(string genre)
        {
            series.Genre = genre;
        }

        public override ISeries Build() => series;

        public override void Reset() => series = new Series();
    }

    public class SecondarySeriesBuilder : Builder<ISeries> 
    {
        private GenericLT series;
        public SecondarySeriesBuilder() 
        {
            series = new GenericLT();
            fieldBuilders = new Dictionary<string, Action<string>>
            {
                    { "Title", BuildTitle },
                    { "Genre", BuildGenre }
            };
        }

        public void BuildTitle(string title)
        {
            series.AddField("Title", title);
        }
        public void BuildGenre(string genre)
        {
            series.AddField("Genre", genre);
        }

        public override ISeries Build()
        {
            SeriesLTAdapter seriesLTAdapter = new SeriesLTAdapter(series, null);
            return seriesLTAdapter;
        }

        public override void Reset() => series = new GenericLT();
    }
}
