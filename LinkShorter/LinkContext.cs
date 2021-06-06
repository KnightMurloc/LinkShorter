using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShorter.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkShorter
{
	public class LinkContext : DbContext
	{
		public DbSet<Link> links { get; set; }

		public LinkContext(DbContextOptions<LinkContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
	}
}
