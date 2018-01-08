using System;
using System.Collections.Generic;
using System.Text;

namespace RetryHelper.Exceptions
{
	public class RetryExcededException : Exception
	{
		public RetryExcededException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
