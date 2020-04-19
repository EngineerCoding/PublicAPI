using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using PublicApi.Infrastructure;
using PublicApi.ViewModels;
using System;

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


			string currentUri = Request.GetDisplayUrl();
			if (currentUri.StartsWith(Uri.UriSchemeHttp))
			{
				currentUri = Uri.UriSchemeHttps + currentUri.Substring(Uri.UriSchemeHttp.Length);
			}

			return View(new BlogPostViewModel
			{
				Title = blogPost.Title,
				Slug = blogPost.Slug,
				Description = blogPost.Description,
				ImageUrl = _blogPostService.ToAbsoluteUrl(blogPost.ImageUrl),
				ImageDescriptionAlt = blogPost.ImageDescriptionAlt,
				Url = _blogPostService.ToAbsoluteUrl(blogPost.RelativeUrl),
				Author = blogPost.Author,
				CurrentUrl = currentUri,
			});
		}

		[EnableCors(Startup.AdminCorsPolicy)]
		[HttpPost("Refresh")]
		[Authorize]
		public IActionResult RefreshBlogPosts()
		{
			_blogPostService.RefreshBlogPosts();
			return Ok();
		}
	}
}
