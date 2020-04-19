using PublicApi.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace PublicApi.Models
{
	public class BlogPost : IBlogPost
	{
		[Key]
		public int PrimaryKey { get; set; }

		public string Title { get; set; }

		public string Slug { get; set; }

		public string Description { get; set; }

		public string ImageUrl { get; set; }

		public string ImageDescriptionAlt { get; set; }

		public string RelativeUrl { get; set; }

		public string Author { get; set; }
	}
}
