using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShorter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LinkShorter.Controllers
{
	[Controller]
	public class RedirectController : ControllerBase
	{
		//private LinkContext db;
		private IRepositoryBase<Link> repository;
		private IConfiguration _configuration;
		private string addres;

		public RedirectController(IRepositoryBase<Link> repository, IConfiguration configuration)
		{
			//this.db = db;
			this.repository = repository;
			_configuration = configuration;
			addres = configuration.GetValue<String>("addres");
		}

		[Route("g/{id:int}")]
		public RedirectResult Get(int id)
		{
			Console.WriteLine("test: " + id);
			Link link = repository.Find(id);
			//Link link = db.links.Find(id);
			//Console.WriteLine(link);
			if (link == null || link.hash == 0)
			{
				return Redirect(addres + "/notfound");
			}
			else
			{
				Console.WriteLine(link.fullLink);
				return Redirect(link.fullLink);
			}

		}
	}
}
