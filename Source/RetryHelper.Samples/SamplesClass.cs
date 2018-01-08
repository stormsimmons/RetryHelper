using System;
using System.Collections.Generic;
using System.Text;

namespace RetryHelper.Samples
{
	public class SamplesClass : IDisposable
	{
		public int timesCalled = 0;
		public int assertVariable = 0;

		public void DoSomethingWithErrors()
		{
			if (timesCalled < 2)
			{
				timesCalled++;
				throw new Exception();
			}
			else
			{
				assertVariable = 50;
			}
		}

		public string DoSomethingWithErrorsWithReturn()
		{
			string testVariable = "This string will only be returned on the first successful attempt of this method";

			if (timesCalled < 2)
			{
				timesCalled++;
				throw new Exception();
			}
			else
			{
				return testVariable;
			}
		}

		public void Dispose()
		{
			assertVariable = 0;
			timesCalled = 0;
		}
	}
}
