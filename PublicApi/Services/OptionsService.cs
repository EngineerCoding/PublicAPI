using Microsoft.Extensions.Options;

namespace PublicApi.Services
{
	public abstract class OptionsService<T>
		where T : class, new()
	{
		/// <summary>
		/// The options
		/// </summary>
		protected readonly T options;

		/// <summary>
		/// Initializes a new instance of the <see cref="OptionsService{T}"/> class.
		/// </summary>
		/// <param name="options">The injected options.</param>
		public OptionsService(IOptions<T> options)
		{
			this.options = options.Value;
		}
	}
}
