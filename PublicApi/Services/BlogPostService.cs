using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PublicApi.Infrastructure;
using PublicApi.Models;
using PublicApi.Models.Settings;
using PublicApi.Utils;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace PublicApi.Services
{
	public class BlogPostService : OptionsService<BlogPostServiceSettings>, IBlogPostService
	{
		private readonly PublicApiContext _context;
		
		private HttpClient _httpClient;
		private HttpClient HttpClient
		{
			get
			{
				if (_httpClient == null)
				{
					_httpClient = new HttpClient();
				}
				return _httpClient;
			}
		}


		public BlogPostService(IOptions<BlogPostServiceSettings> options, PublicApiContext context) : base(options)
		{
			_context = context;
		}

		public IBlogPost GetBlogPostForSlug(string slug)
		{
			return _context.BlogPosts.Where(blogPost => blogPost.Slug == slug).FirstOrDefault();
		}

		public IBlogPost RelativeToAbsolute(IBlogPost blogPost)
		{
			if (!(blogPost is BlogPost blogPostModel))
			{
				return blogPost;
			}

			blogPostModel.RelativeUrl = ToAbsolute(blogPost.RelativeUrl);
			blogPostModel.ImageUrl = ToAbsolute(blogPost.ImageUrl);
			return blogPostModel;
		}

		private string ToAbsolute(string relative)
		{
			if (!string.IsNullOrEmpty(relative))
			{
				return _options.GetUrl(relative);
			}
			return relative;
		}

		public void RefreshBlogPosts()
		{
			HttpResponseMessage response = HttpClient.GetAsync(_options.DataUrl).GetAwaiter().GetResult();
			if (response.StatusCode == HttpStatusCode.OK)
			{
				_context.Truncate<BlogPost>();

				Models.Json.BlogPost[] blogPostsData = JsonConvert.DeserializeObject<Models.Json.BlogPost[]>(
					response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

				BlogPost[] blogPosts = new BlogPost[blogPostsData.Length];
				for (int i = 0; i < blogPostsData.Length; i++)
				{
					blogPosts[i] = Map(blogPostsData[i]);
				}
				_context.BlogPosts.AddRange(blogPosts);
				_context.SaveChanges();
			}
		}

		private BlogPost Map(Models.Json.BlogPost blogPostData)
		{
			string slug = TitleToSlug(blogPostData.Title);
			return new BlogPost
			{
				Title = blogPostData.Title,
				Slug = slug,
				Description = blogPostData.Description,
				ImageUrl = blogPostData.Banner,
				RelativeUrl = _options.BasePostPath + System.Web.HttpUtility.UrlEncode(slug),
				Author = blogPostData.Author ?? _options.DefaultAuthor,
			};
		}

		private static string TitleToSlug(string title)
		{
			string slugified = Regex.Replace(title.ToLowerInvariant(), @"\s", "-");
			return slugified;
		}
	}
}
