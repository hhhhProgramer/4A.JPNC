using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AnimalSpawn.Domain.Entities;

namespace AnimalSpawn.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task Add(T entity);
        Task Delete(int id);
        Task<T> GetById(int id);
        IEnumerable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IEnumerable<T> GetAll();
        void Update(T entity);
    }
}
