using Linklives.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Linklives.DAL
{
    public abstract class DBRepository<T> where T : class
    {
        protected readonly LinklivesContext context;

        protected DBRepository(LinklivesContext context)
        {
            this.context = context;
        }
        public int Count()
        {
            return context.Set<T>().Count();
        }
        public IEnumerable<T> GetAll()
        {
            return context.Set<T>();
        }

        public void Insert(T entity)
        {
            context.Set<T>().Add(entity);
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
