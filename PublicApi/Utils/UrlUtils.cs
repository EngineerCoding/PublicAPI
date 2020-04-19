﻿namespace PublicApi.Utils
{
	public static class UrlUtils
	{
		private const string PathSeparator = "/";

		public static string Join(string host, string path)
		{
			bool hostEndsWith = host.EndsWith(PathSeparator);
			bool pathStartsWith = path.StartsWith(PathSeparator);

			if (hostEndsWith && pathStartsWith)
			{
				return host + path.Substring(0);
			}
			else if (!hostEndsWith && !pathStartsWith)
			{
				path = PathSeparator + path;
			}
			return host + path;
		}
	}
}
