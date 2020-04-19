namespace PublicApi.Infrastructure
{
	public interface IBlogPostService
	{
		IBlogPost RelativeToAbsolute(IBlogPost blogPost);

		IBlogPost GetBlogPostForSlug(string slug);

		void RefreshBlogPosts();
	}
}
