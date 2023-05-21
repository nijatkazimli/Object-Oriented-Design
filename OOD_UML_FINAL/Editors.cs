using OOD_UML_FINAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOD_PROJECT
{
    public interface IEditor 
    {
        public void Edit(string[] arguments);
    }
    public abstract class Editor<T>
    {
        public ICollection<T> collection;
        protected Dictionary<string, Action<string>> fieldEditors;
        public void EditField(string fieldName, string value) 
        {
            fieldEditors[fieldName](value);
        }

        public void EditAtOnce(Dictionary<string, string> fieldValuePairs) 
        {
            foreach (var entry in fieldValuePairs) 
            {
                EditField(entry.Key, entry.Value);
            }
        }
    }

    public class EditDirector<T> : IEditor 
    {
        protected Editor<T> editor;
        protected FilterClass<T> filter;
        protected List<string> fieldNames;

        public void Edit(string[] arguments) 
        {
            Dictionary<string, string> newFieldValuePairs = new Dictionary<string, string>();
            string class_name = arguments[0];
            bool isSuccessful = filter.Filter(arguments, false);
            if (!isSuccessful)
            {
                return;
            }
            editor.collection = filter.fulfilled_collection;
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Available fields:]");
            foreach (var item in fieldNames)
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
                    Console.WriteLine($"[{class_name} edit abandoned]");
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
                if (!fieldNames.Contains(name_of_field))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Incorrect field name]");
                    Console.ForegroundColor = previousColor;
                    continue;
                }
                UpdateOrAdd.AddOrUpdate(newFieldValuePairs, name_of_field, value);
            }
            try
            {
                editor.EditAtOnce(newFieldValuePairs);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = previousColor;
            }
        }
    }

    public class AuthorEditor : EditDirector<IAuthor> 
    {
        public AuthorEditor(ICollection<IAuthor> authors, AuthorFilter authorFilter) 
        {
            base.filter = authorFilter;
            base.fieldNames = new List<string> { "Name", "Surname", "BirthYear", "Awards" };
            base.editor = new AuthorEditorHelper(authors);
        }
    }

    public class MovieEditor : EditDirector<IMovie> 
    {
        public MovieEditor(ICollection<IMovie> movies, MovieFilter movieFilter) 
        {
            base.filter = movieFilter;
            base.fieldNames = new List<string> { "Title", "Genre", "Duration", "ReleaseYear" };
            base.editor = new MovieEditorHelper(movies);
        }
    }
    
    public class SeriesEditor : EditDirector<ISeries> 
    {
        public SeriesEditor(ICollection<ISeries> series, SeriesFilter seriesFilter) 
        {
            base.filter = seriesFilter;
            base.fieldNames = new List<string> { "Title", "Genre" };
            base.editor = new SeriesEditorHelper(series);
        }
    }

    public class AuthorEditorHelper : Editor<IAuthor>
    {
        public AuthorEditorHelper(ICollection<IAuthor> authors) 
        {
            this.collection = authors;
            fieldEditors = new Dictionary<string, Action<string>>
            {
                { "Name", EditName },
                { "Surname", EditSurname },
                { "BirthYear", EditBirthYear },
                { "Awards", EditAwards }
            };
        }

        public void EditName(string name)
        {
            foreach(IAuthor author in collection) 
            {
                author.Name = name;
            }
        }
        public void EditSurname(string surname)
        {
            foreach (IAuthor author in collection)
            {
                author.Surname = surname;
            }
        }
        public void EditBirthYear(string birthYear)
        {
            int newBirthYear = int.TryParse(birthYear, out newBirthYear) ? newBirthYear : throw new Exception("[Incorrect value to set BirthYear!]");
            foreach (IAuthor author in collection)
            {
                author.BirthYear = newBirthYear;
            }
        }

        public void EditAwards(string awards)
        {
            int newAwards = int.TryParse(awards, out int Awards) ? Awards : throw new Exception("[Incorrect value to set Awards!]");
            foreach (IAuthor author in collection)
            {
                author.Awards = newAwards;
            }
        }
    }

    public class MovieEditorHelper : Editor<IMovie> 
    {
        public MovieEditorHelper(ICollection<IMovie> movies) 
        {
            this.collection = movies;
            fieldEditors = new Dictionary<string, Action<string>>
            {
                { "Title", EditTitle },
                { "Genre", EditGenre },
                { "ReleaseYear", EditReleaseYear },
                { "Duration", EditDuration }
            };
        }

        public void EditTitle(string title) 
        {
            foreach (IMovie movie in collection)
            {
                movie.Title = title;
            }
        }

        public void EditGenre(string genre) 
        {
            foreach (IMovie movie in collection) 
            {
                movie.Genre = genre;
            } 
        }

        public void EditReleaseYear(string releaseYear) 
        {
            int newReleaseYear = int.TryParse(releaseYear, out newReleaseYear) ? newReleaseYear : throw new Exception("[Incorrect value to set ReleaseYear!]");
            foreach (IMovie movie in collection) 
            {
                movie.ReleaseYear = newReleaseYear;
            }
        }

        public void EditDuration(string duration) 
        {
            int newDuration = int.TryParse(duration, out newDuration) ? newDuration : throw new Exception("[Incorrect value to set Duration!]");
            foreach (IMovie movie in collection) 
            {
                movie.Duration = newDuration;
            }
        }
    }

    public class SeriesEditorHelper : Editor<ISeries> 
    {
        public SeriesEditorHelper(ICollection<ISeries> series) 
        {
            this.collection = series;
            fieldEditors = new Dictionary<string, Action<string>>
            {
                { "Title", EditTitle },
                { "Genre", EditGenre }
            };
        }

        public void EditTitle(string title)
        {
            foreach (ISeries series in collection)
            {
                series.Title = title;
            }
        }

        public void EditGenre(string genre)
        {
            foreach (ISeries series in collection)
            {
                series.Genre = genre;
            }
        }

    }

    public static class UpdateOrAdd 
    {
        public static void AddOrUpdate(Dictionary<string, string> myDictionary,  string key, string value) 
        {
            if (myDictionary.ContainsKey(key)) 
            {
                myDictionary[key] = value;
            }
            else 
            {
                myDictionary.Add(key, value);
            }
        }
    }
}
