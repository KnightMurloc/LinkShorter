using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LinkShorter.Models
{
	[Index(nameof(id))]
	[Index(nameof(hash))]
	public class Link
	{
		public int id { get; set; }
		public int hash { get; set; }
		public string fullLink { get; set; }
	}
}
