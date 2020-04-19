using Microsoft.Extensions.Options;

namespace PublicApi.Services
{
	public abstract class OptionsService<T>
		where T : class, new()
	{
		protected readonly T _options;

		public OptionsService(IOptions<T> options)
		{
			_options = options.Value;
		}
	}
}
