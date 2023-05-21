using OOD_UML_FINAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_PROJECT
{
    public interface IDelete 
    {
        public void Delete(string[] arguments);
    }

    public class DeleteDirector<T> : IDelete
    {
        protected FilterClass<T> filter;

        public DeleteDirector(FilterClass<T> filter) 
        {
            this.filter = filter;
        }

        public void Delete(string[] arguments) 
        {
            bool isSuccessful = filter.Filter(arguments, false);
            if (isSuccessful) 
            {
                foreach(T item in filter.fulfilled_collection) 
                {
                    filter.collection.Remove(item);
                }
            }
        }
    }
}
