namespace OOD_UML_FINAL
{
    public interface IFilter
    {
        bool Filter(string[] arguments, bool display);
    }

    public interface IComp<T>
    {
        bool Compare(T toBeFiltered, string[] arguments);
    }


    public class StringComp<T> : IComp<T> 
    {
        private readonly Func<T, string> _stringSelector;
        public StringComp(Func<T, string> titleSelector)
        {
            _stringSelector = titleSelector;
        }
        public bool Compare(T toBeFiltered, string[] arguments) 
        {
            string item = _stringSelector(toBeFiltered);
            // Implement comparison logic here
            if (string.Compare(item, arguments[2]) < 0 && arguments[1] == "<")
            {
                return true;
            }
            else if (string.Compare(item, arguments[2]) > 0 && arguments[1] == ">")
            {
                return true;
            }
            else if (string.Compare(item, arguments[2]) == 0 && arguments[1] == "=")
            {
                return true;
            }
            else
                return false;
        }
    }

    public class IntComp<T> : IComp<T> 
    {
        private readonly Func<T, int> _intSelector;
        public IntComp(Func<T, int> intSelector)
        {
            _intSelector = intSelector;
        }

        public bool Compare(T toBeFiltered, string[] arguments) 
        {
            int item = _intSelector(toBeFiltered);

            if (int.TryParse(arguments[2], out int value)) 
            {
                if (item > value && arguments[1] == ">")
                {
                    return true;
                }
                else if (item < value && arguments[1] == "<")
                {
                    return true;
                }
                else if (item == value && arguments[1] == "=")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                throw new Exception("Incorrect value to compare!");
                // find Author BirthYear=abc should throw an exception which will be handled in line 129
            }
        }
    }
    public class FilterClass<T> : IFilter
    {
        public ICollection<T> collection;
        public Dictionary<string, IComp<T>> fieldComp;

        public List<T> fulfilled_collection;

        public bool Filter(string[] arguments, bool display) 
        {
            fulfilled_collection = new List<T>();
            

            int num_of_filter_condition = arguments.Count() - 1;
            string[] arguments_for_comparators = arguments.Skip(1).ToArray();
            try
            {
                foreach (T item in collection)
                {
                    bool flag = false;

                    for (int i = 0; i < num_of_filter_condition; i++)
                    {
                        int operatorIndex = arguments_for_comparators[i].IndexOfAny(new char[] { '<', '>', '=' });
                        if (operatorIndex == -1)
                        {
                            ConsoleColor previousColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Inappropriate/missing operator. Only `<` or `>` or `=` is allowed");
                            Console.ForegroundColor = previousColor;
                            return false; // unsuccessful filtering 
                        }
                        string op = arguments_for_comparators[i][operatorIndex].ToString();
                        string fieldName = arguments_for_comparators[i].Substring(0, operatorIndex);
                        string value = arguments_for_comparators[i].Substring(operatorIndex + 1);
                        string[] arguments_for_single_comparator = new string[] { fieldName, op, value };
                        // Basically what I do above is creating an array of arguments so that they will be handled easily in comparators.
                        if (fieldComp.TryGetValue(fieldName, out IComp<T> comp))
                            flag = comp.Compare(item, arguments_for_single_comparator);
                        else
                        {
                            ConsoleColor previousColor = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Field `{fieldName}` is not found");
                            Console.ForegroundColor = previousColor;
                            return false;
                        }
                    }
                    if (flag && item is IDisplay displayable && display) // because task says "Everything that was said about "list" is also applicable to "find"" regarding printing.
                    {
                        ConsoleColor previousColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        displayable.Display();
                        Console.ForegroundColor = previousColor;
                        fulfilled_collection.Add(item);
                    }
                    else if (flag && !display)
                    {
                        fulfilled_collection.Add(item);
                    }
                }
            }
            catch (Exception ex) 
            {
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = previousColor;
            }
            return true;
        }
    }

    public class AuthorFilter : FilterClass<IAuthor>
    {
        public AuthorFilter(ICollection<IAuthor> collection)
        {
            base.collection = collection;
            Dictionary<string, IComp<IAuthor>> fieldComp  = new Dictionary<string, IComp<IAuthor>>();
            fieldComp.Add("Name", new StringComp<IAuthor>(author => author.Name));
            fieldComp.Add("Surname", new StringComp<IAuthor>(author => author.Surname));
            fieldComp.Add("BirthYear", new IntComp<IAuthor>(author => author.BirthYear));
            fieldComp.Add("Awards", new IntComp<IAuthor>(author => author.Awards));
            base.fieldComp = fieldComp;
        }
    }

    public class MovieFilter : FilterClass<IMovie>
    {
        public MovieFilter(ICollection<IMovie> collection)
        {
            base.collection = collection;
            Dictionary<string, IComp<IMovie>> fieldComp = new Dictionary<string, IComp<IMovie>>();
            fieldComp.Add("Title", new StringComp<IMovie>(movie => movie.Title));
            fieldComp.Add("Genre", new StringComp<IMovie>(movie => movie.Genre));
            fieldComp.Add("ReleaseYear", new IntComp<IMovie>(movie => movie.ReleaseYear));
            fieldComp.Add("Duration", new IntComp<IMovie>(movie => movie.Duration));
            base.fieldComp = fieldComp;
        }
    }

    public class SeriesFilter : FilterClass<ISeries>
    {
        public SeriesFilter(ICollection<ISeries> collection)
        {
            base.collection = collection;
            Dictionary<string, IComp<ISeries>> fieldComp = new Dictionary<string, IComp<ISeries>>();
            fieldComp.Add("Title", new StringComp<ISeries>(series => series.Title));
            fieldComp.Add("Genre", new StringComp<ISeries>(series => series.Genre));
            base.fieldComp = fieldComp;
        }
    }
}