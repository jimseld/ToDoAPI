using System;

namespace ToDoAPI
{
	/// <summary>
	/// Error helper
	/// </summary>
	public static class ExceptionHelper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ex"></param>
		/// <returns></returns>
		public static object ProcessError(Exception ex)
		{
			return new
			{
				error = new
				{
					code = ex.HResult,
					message = ex.Message
				}
			};
		}

	} // class
} // namespace

