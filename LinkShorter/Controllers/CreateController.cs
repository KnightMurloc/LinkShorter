using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinkShorter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LinkShorter.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CreateController : ControllerBase
	{
		private IRepositoryBase<Link> repository;
		private IConfiguration _configuration;
		private string addres;

		public CreateController(IRepositoryBase<Link> repository, IConfiguration configuration)
		{
			this.repository = repository;
			_configuration = configuration;
			addres = configuration.GetValue<String>("addres");
		}

		public static bool ValidHttpURL(ref string s)
		{
			if (s == null)
			{
				return false;
			}
			if (!Regex.IsMatch(s, @"^https?:\/\/", RegexOptions.IgnoreCase))
			{
				s = "http://" + s;
			}

			string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
			Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			return Rgx.IsMatch(s);
		}

		[HttpPost]
		public ActionResult<string> Post(string url)
		{

			if (!ValidHttpURL(ref url))
			{
				return Content("url unValid");
			}

			Link exist = repository.
				FindByCondition(link => link.hash == url.GetHashCode()).
				Where(link => link.fullLink == url).FirstOrDefault();
			if (exist != null)
			{
				return Content(addres + "/g/" + exist.id);
			}
			else
			{
				Link free = repository.FindByCondition(link => link.hash == 0).FirstOrDefault();
				if (free != null)
				{
					Link newLink = new Link() { id = free.id, hash = url.GetHashCode(), fullLink = url };
					repository.Update(newLink);
					repository.Save();
					return Content(addres + "/g/" + newLink.id);
				}

				Link link = new Link() { id = repository.Size() + 1, hash = url.GetHashCode(), fullLink = url };
				repository.Create(link);
				repository.Save();
				return Content(addres + "/g/" + link.id);
			}

		}

		[HttpDelete]
		public ActionResult Delete(int id)
		{
			Link link = repository.Find(id);
			if (link == null || link.hash == 0)
			{
				return NotFound();
			}
			Link clearLink = new Link() { id = link.id, hash = 0, fullLink = null };
			repository.Update(clearLink);
			repository.Save();

			return Ok();
		}

	}
}
