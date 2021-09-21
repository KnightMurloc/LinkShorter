using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinkShorter.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkShorter
{
	public class LinkRepository : IRepositoryBase<Link>
	{
		private LinkContext db;

		public LinkRepository(LinkContext context) {
			db = context;
		}

		public void Create(Link entity)
		{
			db.links.Add(entity);
		}

		public void Delete(Link entity)
		{
			db.links.Remove(entity);
		}

		public IQueryable<Link> FindByCondition(Expression<Func<Link, bool>> expression)
		{
			return db.Set<Link>().Where(expression).AsNoTracking();
		}

		public void Update(Link entity)
		{
			Link old = Find(entity.id);
			db.Entry(old).CurrentValues.SetValues(entity);
		}

		public async void Save() {
			await db.SaveChangesAsync();
		}

		public int Size() {
			return db.links.Count();
		}

		public Link Find(int id) {
			return db.links.Find(id);
		}

		private bool disposed = false;
		public virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					db.Dispose();
				}
			}
			disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
