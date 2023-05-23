namespace OOD_UML_FINAL
{
    public static class CollectionAlgorithms
    {
        public static T Find<T>(ICollectionWithIterators<T> collection, Func<T, bool> predicate, bool searchFromStart)
        {
            IEnumerator<T> enumerator = searchFromStart ? collection.GetEnumerator() : collection.GetReverseEnumerator();
            var wrapper = new EnumeratorWrapper<T>(enumerator);

            foreach (T item in wrapper)
            {
                if (predicate(item))
                {
                    return item;
                }
            }

            return default(T);
        }

        public static void Print<T>(ICollectionWithIterators<T> collection, Func<T, bool> predicate, bool searchFromStart)
        {
            IEnumerator<T> enumerator = searchFromStart ? collection.GetEnumerator() : collection.GetReverseEnumerator();
            var wrapper = new EnumeratorWrapper<T>(enumerator);

            foreach (T item in wrapper)
            {
                if (predicate(item))
                {
                    Console.WriteLine(item);
                }
            }
        }

        public static T Find<T>(IEnumerator<T> iterator, Func<T, bool> predicate)
        {

            while (iterator.MoveNext())
            {
                if (predicate(iterator.Current))
                {
                    return iterator.Current;
                }
            }
            return default(T);
        }

        public static void ForEach<T>(IEnumerator<T> iterator, Action<T> function)
        {
            while (iterator.MoveNext()) 
            {
                function(iterator.Current);
            }
        }

        public static int CountIf<T>(IEnumerator<T> iterator, Func<T, bool> predicate)
        {
            int count = 0;

            while (iterator.MoveNext())
            {
                if (predicate(iterator.Current))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
