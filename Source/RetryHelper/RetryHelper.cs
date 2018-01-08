using RetryHelper.Exceptions;
using System;
using System.Threading;

namespace RetryHelper
{
	public class RetryHelper : IRetryHelper
	{
		/// <summary>
		/// Retry operation that may possibly fail as a resilience for a void method
		/// </summary>
		/// <param name="retryAttempts">Amount of retry attempts</param>
		/// <param name="timeDelay">Time interval between retries</param>
		/// <param name="operation">Method to be executed</param>
		public void RetryOperationOnFail(int retryAttempts, int timeDelay, Action operationToComplete)
		{
			LoopOperation<dynamic>(retryAttempts, timeDelay, null, operationToComplete);
		}

		/// <summary>
		/// Retry operation that may possibly fail as a resilience for a generic return method
		/// </summary>
		/// <param name="retryAttempts">Amount of retry attempts</param>
		/// <param name="timeDelay">Time interval between retries</param>
		/// <param name="operation">Method to be executed</param>
		public TReturn RetryOperationOnFail<TReturn>(int retryAttempts, int timeDelay, Func<TReturn> operationToComplete)
		{
			var result = LoopOperation<TReturn>(retryAttempts, timeDelay, operationToComplete);
			return result;
		}

		private T LoopOperation<T>(int retryAttempts, int timeDelay, Func<T> funcOperation = null, Action actionOperation = null)
		{
			var tries = 0;
			T result;
			do
			{
				try
				{
					tries++;
					if (funcOperation != null)
					{
						result = funcOperation();
					}
					else
					{
						actionOperation?.Invoke();
						result = default(T);
					}
					break;
				}
				catch (Exception e)
				{
					if (tries == retryAttempts)
					{
						throw new RetryExcededException($"Exception thrown due to all retry attempts({retryAttempts}) failing" , e);
					}
					Thread.Sleep(timeDelay);
				}

			} while (true);

			return result;
		}
	}
}
