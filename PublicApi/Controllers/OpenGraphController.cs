using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Infrastructure;

namespace PublicApi.Controllers
{
	[Route("[controller]")]
	public class OpenGraphController : Controller
	{
		private readonly IBlogPostService _blogPostService;

		public OpenGraphController(IBlogPostService blogPostService)
		{
			_blogPostService = blogPostService;
		}

		[HttpGet("{slug}")]
		public IActionResult BlogPost(string slug)
		{
			IBlogPost blogPost = _blogPostService.GetBlogPostForSlug(slug);
			if (blogPost == null)
			{
				return NotFound();
			}

			IBlogPost enriched = _blogPostService.RelativeToAbsolute(blogPost);
			return View(enriched);
		}

		[HttpPost("Refresh")]
		[Authorize]
		public IActionResult RefreshBlogPosts()
		{
			_blogPostService.RefreshBlogPosts();
			return Ok();
		}
	}
}
