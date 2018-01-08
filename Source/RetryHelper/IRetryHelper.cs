using System;
using System.Collections.Generic;
using System.Text;

namespace RetryHelper
{
    public interface IRetryHelper
    {
		void RetryOperationOnFail(int retryAttempts, int timeDelay, Action operation);
		T RetryOperationOnFail<T>(int retryAttempts, int timeDelay, Func<T> operationToComplete);
	}
}
