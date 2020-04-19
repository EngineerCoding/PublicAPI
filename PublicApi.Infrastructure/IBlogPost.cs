namespace PublicApi.Infrastructure
{
	public interface IBlogPost
	{
		string Title { get; }

		string Slug { get; }

		string Description { get; }

		string ImageUrl { get; }

		string ImageDescriptionAlt { get; }

		string RelativeUrl { get; }

		string Author { get; }
	}
}
