using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LinkShorter;
using LinkShorter.Models;

namespace Tests
{
	class TestRepository : IRepositoryBase<Link>
	{
		private List<Link> db = new List<Link>();

		public void Create(Link entity)
		{
			db.Add(entity);
		}

		public void Delete(Link entity)
		{
			db.Remove(entity);
		}

		public Link Find(int id)
		{
			return db.Find(link => link.id == id);
		}

		public IQueryable<Link> FindByCondition(Expression<Func<Link, bool>> expression)
		{
			Func<Link, bool> predicate = expression.Compile();
			return db.Where(predicate).AsQueryable();
		}

		public void Save()
		{
			
		}

		public int Size()
		{
			return db.Count;
		}

		public void Update(Link entity)
		{
			int index = db.FindIndex(list => list.id == entity.id);
			db[index] = entity;
		}

		private bool disposed = false;
		public virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					
				}
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
