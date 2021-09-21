using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LinkShorter
{
	public interface IRepositoryBase<T> : IDisposable
	{
		IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
		void Create(T entity);
		void Update(T entity);
		void Delete(T entity);
		void Save();
		public int Size();
		public T Find(int id);
	}
}
