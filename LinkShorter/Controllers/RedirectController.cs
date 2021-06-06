using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShorter.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinkShorter.Controllers
{
	[Controller]
	public class RedirectController : ControllerBase
	{
		LinkContext db;

		public RedirectController(LinkContext db)
		{
			this.db = db;
		}

		[Route("g/{id:int}")]
		public RedirectResult Get(int id)
		{

			Link link = db.links.Find(id);
			if (link == null)
			{
				return Redirect("http://localhost:5000/");
			}
			else
			{
				Console.WriteLine(link.fullLink);
				return Redirect(link.fullLink);
			}

		}
	}
}
