namespace PublicApi.Infrastructure
{
	public interface IBlogPostService
	{
		string ToAbsoluteUrl(string path);

		IBlogPost GetBlogPostForSlug(string slug);

		void RefreshBlogPosts();
	}
}
