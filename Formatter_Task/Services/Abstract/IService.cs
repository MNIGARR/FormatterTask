﻿using System.Linq.Expressions;

namespace Formatter_Task.Services.Abstract
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
