namespace PublicApi.Models.Settings
{
	public class BlogPostServiceSettings
	{
		/// <summary>
		/// The host which contains the data and actual blog posts
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// The path relative to the host which leads the JSON containing all blog post data
		/// </summary>
		public string DataPath { get; set; }

		/// <summary>
		/// The path relative to the host which leads to the blog posts when a slug is attached
		/// </summary>
		public string BasePostPath { get; set; }

		/// <summary>
		/// The default author when an author is not defined
		/// </summary>
		public string DefaultAuthor { get; set; }
	}
}
