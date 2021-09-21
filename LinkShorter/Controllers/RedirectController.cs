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
		private IRepositoryBase<Link> repository;
		private IConfiguration _configuration;
		private string addres;

		public RedirectController(IRepositoryBase<Link> repository, IConfiguration configuration)
		{
			this.repository = repository;
			_configuration = configuration;
			addres = configuration.GetValue<String>("addres");
		}

		[Route("g/{id:int}")]
		public RedirectResult Get(int id)
		{
			Link link = repository.Find(id);
			if (link == null || link.hash == 0)
			{
				return Redirect(addres + "/notfound");
			}
			else
			{
				return Redirect(link.fullLink);
			}

		}
	}
}
