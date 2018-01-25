using RetryHelper.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RetryHelper
{
	public static class RetryHelper
	{
		private const int MaxRetry = 100;
		/// <summary>
		/// Retry operation that may possibly fail as a resilience for a void method
		/// </summary>
		/// <param name="retryAttempts">Amount of retry attempts</param>
		/// <param name="timeDelay">Time interval between retries</param>
		/// <param name="operation">Method to be executed</param>
		/// <param name="cancelRetry">A function to test whether retry should be canceled. If the function returns true, the operation will not be retried</param>
		public static void RetryOnException(int retryAttempts, int timeDelay, Action operationToComplete, Func<Exception, bool> cancelRetry)
		{
			LoopOperation<bool>(cancelRetry, retryAttempts, timeDelay, null, operationToComplete).Wait();
		}

		/// <summary>
		/// Retry operation that may possibly fail as a resilience for a generic return method
		/// </summary>
		/// <param name="retryAttempts">Amount of retry attempts</param>
		/// <param name="timeDelay">Time interval between retries</param>
		/// <param name="operation">Method to be executed</param>
		/// <param name="cancelRetry">A function to test whether retry should be canceled. If the function returns true, the operation will not be retried</param>
		public static TReturn RetryOnException<TReturn>(int retryAttempts, int timeDelay, Func<TReturn> operationToComplete, Func<Exception, bool> cancelRetry)
		{
			var result = LoopOperation(cancelRetry, retryAttempts, timeDelay, operationToComplete).Result;
			return result;
		}

		private async static Task<T> LoopOperation<T>(Func<Exception, bool> cancelRetry, int retryAttempts, int timeDelay, Func<T> funcOperation = null, Action actionOperation = null)
		{
			if (retryAttempts <= 0)
			{
				throw new ArgumentOutOfRangeException("retryAttempts", "retryAttempts must be greater than zero");
			}

			var tries = 0;
			do
			{
				try
				{
					if (funcOperation != null)
					{
						return funcOperation();
					}
					actionOperation?.Invoke();
					return default(T);
				}
				catch (Exception e)
				{
					tries++;

					if (cancelRetry(e))
					{
						throw e;
					}

					if (tries == retryAttempts || tries == MaxRetry)
					{
						throw new RetryExceededException($"Exception thrown due to all retry attempts({tries}) failing", e);
					}
					await Task.Delay(timeDelay).ConfigureAwait(false);
				}

			} while (true);
		}
	}
}
