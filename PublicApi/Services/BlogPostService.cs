using Microsoft.Extensions.Options;
using PublicApi.Infrastructure;
using PublicApi.Models;
using PublicApi.Models.Settings;
using PublicApi.Utils;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PublicApi.Services
{
	public class BlogPostService : OptionsService<BlogPostServiceSettings>, IBlogPostService
	{
		/// <summary>
		/// The context
		/// </summary>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogPostService"/> class.
		/// </summary>
		/// <param name="options">The injected options.</param>
		/// <param name="context">The injected context.</param>
		public BlogPostService(IOptionsSnapshot<BlogPostServiceSettings> options, PublicApiContext context)
			: base(options)
		{
			_context = context;
		}

		/// <inheritdoc/>
		public IBlogPost GetBlogPostForSlug(string slug)
		{
			return _context.BlogPosts.Where(blogPost => blogPost.Slug == slug).FirstOrDefault();
		}

		/// <inheritdoc/>
		public string ToAbsoluteUrl(string path)
		{
			return UrlUtils.Join(options.Host, path);
		}

		/// <inheritdoc/>
		public void RefreshBlogPosts()
		{
			string dataUrl = ToAbsoluteUrl(options.DataPath);
			HttpResponseMessage response = HttpClient.GetAsync(dataUrl).GetAwaiter().GetResult();
			if (response.StatusCode == HttpStatusCode.OK)
			{
				_context.Truncate<BlogPost>();

				Models.Json.BlogPost[] blogPostsData = JsonSerializer.Deserialize<Models.Json.BlogPost[]>(
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

		/// <summary>
		/// Maps the specified blog post data to database blog post data.
		/// </summary>
		/// <param name="blogPostData">The blog post data.</param>
		/// <returns></returns>
		private BlogPost Map(Models.Json.BlogPost blogPostData)
		{
			string slug = TitleToSlug(blogPostData.Title);
			return new BlogPost
			{
				Title = blogPostData.Title,
				Slug = slug,
				Description = blogPostData.Description,
				ImageUrl = blogPostData.Banner,
				RelativeUrl = options.BasePostPath + System.Web.HttpUtility.UrlEncode(slug),
				Author = blogPostData.Author ?? options.DefaultAuthor,
			};
		}

		/// <summary>
		/// Converts the title to a slug.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <returns>
		/// The generated slug
		/// </returns>
		private static string TitleToSlug(string title)
		{
			string slugified = Regex.Replace(title.ToLowerInvariant(), @"\s", "-");
			slugified = Regex.Replace(slugified, @"[^\w\-]", string.Empty);
			return slugified;
		}
	}
}
