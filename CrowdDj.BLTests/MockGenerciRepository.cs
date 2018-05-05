using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.PoCos;
using CrowdDj.DAL;

namespace CrowdDj.BLTests
{
    public class MockGenerciRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntityObject, new()
    {
        private List<TEntity> entities;

        public MockGenerciRepository()
        {
            entities = new List<TEntity>();
        }

        public virtual TEntity[] Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = entities.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            // alle gewünschten abhängigen Entitäten mitladen
            //foreach (string includeProperty in includeProperties.Split
            //    (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            //{
            //    query = query.Include(includeProperty);
            //}
            if (orderBy != null)
            {
                return orderBy(query).ToArray();
            }
            return query.ToArray();
        }

        public TEntity GetById(object id)
        {
            int i = (int) id;
            return entities.SingleOrDefault(e => e.Id == i);
        }

        public void Insert(TEntity entity)
        {
            entities.Add(entity);
            entity.Id = entities.Max(e => e.Id)+1;
        }

        public void Delete(TEntity entityToDelete)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entityToUpdate)
        {
            throw new NotImplementedException();
        }

        public int Count(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            throw new NotImplementedException();
        }

        public void InsertMany(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}
