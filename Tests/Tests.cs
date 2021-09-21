using System;
using System.IO;
using LinkShorter;
using LinkShorter.Controllers;
using LinkShorter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Tests
{
	public class CreateTests
	{
		[Fact]
		public void CreateTestValid() {

			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			CreateController controller = new CreateController(repository, configuration);
			for (int i = 1; i <= 10; i++) {
				ContentResult result = (ContentResult)controller.Post($"example{i}.com").Result;

				Assert.Equal($"http://localhost:5000/g/{i}", result.Content);

			}

		}

		[Fact]
		public void CreateTestEmpty()
		{

			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			CreateController controller = new CreateController(repository, configuration);
			ContentResult result = (ContentResult)controller.Post("").Result;

			Assert.Equal("url unValid", result.Content);
		}

		[Fact]
		public void CreateTestNull()
		{

			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			CreateController controller = new CreateController(repository, configuration);
			ContentResult result = (ContentResult)controller.Post(null).Result;

			Assert.Equal("url unValid", result.Content);
		}

		[Fact]
		public void CreateTestRandomUnValidUrl()
		{

			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			CreateController controller = new CreateController(repository, configuration);
			ContentResult result = (ContentResult)controller.Post("dsamdklsamdklasmdklasmdklasmdkalsmdkalsd").Result;

			Assert.Equal("url unValid", result.Content);
		}

		[Fact]
		public void CreateTestValidDublicate()
		{

			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			CreateController controller = new CreateController(repository, configuration);
			ContentResult result = (ContentResult)controller.Post("example.com").Result;

			Assert.Equal("http://localhost:5000/g/1", result.Content);

			ContentResult result2 = (ContentResult)controller.Post("example.com").Result;

			Assert.Equal("http://localhost:5000/g/1", result2.Content);
		}

		[Fact]
		public void CreateTestAfterRemove()
		{

			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			CreateController controller = new CreateController(repository, configuration);
			controller.Post("example.com");
			controller.Post("example2.com");

			controller.Delete(1);

			ContentResult result = (ContentResult) controller.Post("example3.com").Result;

			Assert.Equal("http://localhost:5000/g/1", result.Content);
		}
	}

	public class RedirectTests
	{
		[Fact]
		public void RedirectTestValid() {
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			repository.Create(new Link() {id = 1,hash = "example.com".GetHashCode(),fullLink = "example.com" });

			RedirectController controller = new RedirectController(repository, configuration);

			RedirectResult result = controller.Get(1);

			Assert.Equal("example.com", result.Url);
		}

		[Fact]
		public void RedirectTestUnValid()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			repository.Create(new Link() { id = 1, hash = "example.com".GetHashCode(), fullLink = "example.com" });

			RedirectController controller = new RedirectController(repository, configuration);

			RedirectResult result = controller.Get(10);

			Assert.Equal("http://localhost:5000/notfound", result.Url);
		}

		[Fact]
		public void RedirectTestUnValidNegative()
		{
			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			repository.Create(new Link() { id = 1, hash = "example.com".GetHashCode(), fullLink = "example.com" });

			RedirectController controller = new RedirectController(repository, configuration);

			RedirectResult result = controller.Get(-10);

			Assert.Equal("http://localhost:5000/notfound", result.Url);
		}
	}

	public class RemoveTests {
		[Fact]
		public void RemoveTestValid()
		{

			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			for (int i = 1; i <= 10; i++)
			{
				repository.Create(new Link() { id = i, hash = $"example{i}.com".GetHashCode(), fullLink = $"example{i}.com" });
			}

			CreateController controller = new CreateController(repository, configuration);

			ActionResult result = controller.Delete(2);

			Assert.Equal(typeof(OkResult), result.GetType());
		}

		[Fact]
		public void RemoveTestFromEmpty()
		{

			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			CreateController controller = new CreateController(repository, configuration);

			ActionResult result = controller.Delete(2);

			Assert.Equal(typeof(NotFoundResult), result.GetType());
		}

		[Fact]
		public void RemoveTestFromOutOfRange()
		{


			ConfigurationBuilder builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory())
				   .AddJsonFile("appsettings.json");

			IConfiguration configuration = builder.Build();

			TestRepository repository = new TestRepository();

			for (int i = 1; i <= 10; i++)
			{
				repository.Create(new Link() { id = i, hash = $"example{i}.com".GetHashCode(), fullLink = $"example{i}.com" });
			}

			CreateController controller = new CreateController(repository, configuration);

			ActionResult result = controller.Delete(20);

			Assert.Equal(typeof(NotFoundResult), result.GetType());
		}
	}
}
