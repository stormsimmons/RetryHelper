using RetryHelper.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RetryHelper.Tests
{
	[TestClass]
	public class RetryHelperTests
	{
		private TestMethodsToVerify _testMethodsToVerify;
		public RetryHelperTests()
		{
			_testMethodsToVerify = new TestMethodsToVerify();
		}

		public void Dispose()
		{
			_testMethodsToVerify.Dispose();
		}


		[TestMethod]
		public void RetryOnException_MethodThrowExceptions_CallsMethodOnce()
		{
			RetryHelper.RetryOnException(3, 10,
				() => _testMethodsToVerify.DoSomethingWithErrors(),
				(e) => { return false; });

			Assert.AreEqual(50, _testMethodsToVerify.assertVariable);
		}
		[TestMethod]
		[ExpectedException(typeof(AggregateException))]
		public void RetryOnException_MethodThrowExceptions_ThrowsException()
		{
			RetryHelper.RetryOnException(2, 10,
				() => _testMethodsToVerify.DoSomethingWithErrors(),
				(e) => { return false; });
		}

		[TestMethod]
		public void RetryOnException_MethodThrowExceptions_CallsMethodReturnsResult()
		{
			var result = RetryHelper.RetryOnException<string>(3, 10,
					() => _testMethodsToVerify.DoSomethingWithErrorsWithReturn(),
					(e) => { return false; });

			Assert.AreEqual("this is a test string", result);
		}

		[TestMethod]
		[ExpectedException(typeof(AggregateException))]
		public void RetryOnException_MethodThrowExceptions_ThrowsExceptionForReturnMethod()
		{
			var result = RetryHelper.RetryOnException<string>(2, 10,
					() => _testMethodsToVerify.DoSomethingWithErrorsWithReturn(),
					(e) => { return false; });
		}

		[TestMethod]
		[ExpectedException(typeof(AggregateException))]
		public void RetryOnException_MethodAbortWithConfigurableExceptions_ThrowsExceptionForReturnMethod()
		{
			var result = RetryHelper.RetryOnException<string>(3, 10,
					() => _testMethodsToVerify.DoSomethingWithErrorsWithReturn(),
					(e) => {
						if (e is Exception)
						{
							return true;
						}
						return false;
					});
		}

		[TestMethod]
		[ExpectedException(typeof(AggregateException))]
		public void RetryOnException_MethodAbortWithConfigurableExceptions_ThrowsException()
		{
			RetryHelper.RetryOnException(3, 10,
				() => _testMethodsToVerify.DoSomethingWithErrors(),
				(e) => {
					if (e is Exception)
					{
						return true;
					}
					return false;
				});
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

