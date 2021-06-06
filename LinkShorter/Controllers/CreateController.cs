using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinkShorter.Models;
using Microsoft.AspNetCore.Mvc;

namespace LinkShorter.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CreateController : ControllerBase
	{

		private LinkContext db;

		public CreateController(LinkContext db)
		{
			this.db = db;
		}

		public static bool ValidHttpURL(string s, out Uri resultURI)
		{
			if (!Regex.IsMatch(s, @"^https?:\/\/", RegexOptions.IgnoreCase))
				s = "http://" + s;

			if (Uri.TryCreate(s, UriKind.Absolute, out resultURI))
				return (resultURI.Scheme == Uri.UriSchemeHttp ||
						resultURI.Scheme == Uri.UriSchemeHttps);

			return false;
		}

		[HttpPost]
		public async Task<ActionResult<string>> Post(string url)
		{
			Link exist = db.links.FirstOrDefault(link => link.fullLink == url);
			if (exist != null)
			{
				return Content("localhost:5000/g/" + exist.id);
			}
			else
			{
				Uri uriResult;

				if (!ValidHttpURL(url, out uriResult))
				{
					return Content("url unValid");
				}
				UriBuilder builder = new UriBuilder(url);
				Console.WriteLine(uriResult);
				Link link = new Link() { id = db.links.Count() + 1, fullLink = builder.Uri.AbsoluteUri };

				db.links.Add(link);
				await db.SaveChangesAsync();
				return Content("localhost:5000/g/" + link.id);
			}

		}

		[HttpGet]
		public ActionResult<string> Get(string url)
		{
			Console.WriteLine(url);
			return Content("test get");
		}
	}
}
