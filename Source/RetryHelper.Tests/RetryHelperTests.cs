using RetryHelper.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RetryHelper.Tests
{
	[TestClass]
	public class RetryHelperTests
	{
		private RetryHelper _retryHelper;
		private TestMethodsToVerify _testMethodsToVerify;
		public RetryHelperTests()
		{
			_testMethodsToVerify = new TestMethodsToVerify();
			_retryHelper = new RetryHelper();
		}

		public void Dispose()
		{
			_testMethodsToVerify.Dispose();
		}

		[TestMethod]
		public void RetryOperationOnFail_MethodThrowExceptions_CallsMethodOnce()
		{
			_retryHelper.RetryOperationOnFail(3, 10,
				() => _testMethodsToVerify.DoSomethingWithErrors());

			Assert.AreEqual(50, _testMethodsToVerify.assertVariable);
		}
		[TestMethod]
		[ExpectedException(typeof(RetryExcededException))]
		public void RetryOperationOnFail_MethodThrowExceptions_ThrowsException()
		{
			_retryHelper.RetryOperationOnFail(2, 10,
				() => _testMethodsToVerify.DoSomethingWithErrors());
		}

		[TestMethod]
		public void RetryOperationOnFail_MethodThrowExceptions_CallsMethodReturnsResult()
		{
			var result = _retryHelper.RetryOperationOnFail<string>(3, 10,
					() => _testMethodsToVerify.DoSomethingWithErrorsWithReturn());

			Assert.AreEqual("this is a test string", result);
		}

		[TestMethod]
		[ExpectedException(typeof(RetryExcededException))]
		public void RetryOperationOnFail_MethodThrowExceptions_ThrowsExceptionForReturnMethod()
		{
			var result = _retryHelper.RetryOperationOnFail<string>(2, 10,
					() => _testMethodsToVerify.DoSomethingWithErrorsWithReturn());
		}

	}

	public class TestMethodsToVerify : IDisposable
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
			string testVariable = "this is a test string";

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

