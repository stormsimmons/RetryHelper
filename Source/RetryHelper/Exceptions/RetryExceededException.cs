using System;
using System.Collections.Generic;
using System.Text;

namespace RetryHelper.Exceptions
{
	public class RetryExceededException : Exception
	{
		public RetryExceededException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
