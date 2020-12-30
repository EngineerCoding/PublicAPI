namespace PublicApi.Infrastructure
{
	public interface IBlogPostService
	{
		/// <summary>
		/// Converts the path to an absolute URL.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>
		/// The absolute URL
		/// </returns>
		string ToAbsoluteUrl(string path);

		/// <summary>
		/// Gets the blog post for slug.
		/// </summary>
		/// <param name="slug">The slug.</param>
		/// <returns>
		/// The blog post object
		/// </returns>
		IBlogPost GetBlogPostForSlug(string slug);

		/// <summary>
		/// Refreshes the blog posts.
		/// </summary>
		void RefreshBlogPosts();
	}
}
