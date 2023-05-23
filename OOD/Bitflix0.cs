namespace OOD_UML_FINAL
{
    public interface IMovie
    {
        string Title { get; set; }
        string Genre { get; set; }
        int ReleaseYear { get; set; }
        Author Director { get; set; }
        int Duration { get; set; }
        void Task2();
    }

    public interface ISeries
    {
        string Title { get; set; }
        string Genre { get; set; }
        Author ShowRunner { get; set; }
        List<Episode> Episodes { get; set; }
        void Task2();
    }

    public interface IEpisode
    {
        string Title { get; set; }
        int Duration { get; set; }
        int ReleaseYear { get; set; }
        Author Director { get; set; }
    }

    public interface IAuthor
    {
        string Name { get; set; }
        string Surname { get; set; }
        int BirthYear { get; set; }
        int Awards { get; set; }
    }

    public interface IDisplay 
    {
        void Display();
    }


    public class Movie : IMovie, IDisplay
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public Author Director { get; set; }
        public int Duration { get; set; }

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

    public class Series : ISeries, IDisplay
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public Author ShowRunner { get; set; }
        public List<Episode> Episodes  { get; set; }
        public override string ToString()
        {
            var result = $"{Title}, {Genre}, {ShowRunner.Name} {ShowRunner.Surname}";
            int i = 0;
            foreach(Episode episode in Episodes)
            {
                result += "\t" + ++i + ". ";
                result += episode.ToString();
            }
            return result;
        }

        public void Task2()
        {
            bool flag = false;
            foreach(Episode episode in Episodes)
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
                foreach(Episode episode in Episodes)
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

    public class Episode : IEpisode, IDisplay
    {
        public string Title { get; set; }
        public int Duration { get; set; }
        public int ReleaseYear { get; set; }
        public Author Director { get; set; }

        public void Display()
        {
            Console.WriteLine($"{Title}, {Duration}, {ReleaseYear}");
        }

        public override string ToString()
        {
            return $"{Title}, {Duration}, {ReleaseYear}, {Director.Name} {Director.Surname}";
        }
    }

    public class Author : IAuthor, IDisplay
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BirthYear { get; set; }
        public int Awards { get; set; }

        public void Display()
        {
            Console.WriteLine("{0,-15} {1,-15} {2,-7} {3,-5}", Name, Surname, BirthYear, Awards);
        }

        public override string ToString()
        {
            return $"{Name}, {Surname}, {BirthYear}, {Awards}";
        }
    }
}
