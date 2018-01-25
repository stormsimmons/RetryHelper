using System;
using RetryHelper;

namespace RetryHelper.Samples
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var sampleClass = new SamplesClass())
			{
				RetryHelper.RetryOnException(3, 10,
					// retry method setup to fail twice before setting a variable
					() => sampleClass.DoSomethingWithErrors(),
					(e) => {
						return e is NullReferenceException;
					});
				Console.WriteLine("Method with no return:");
				Console.WriteLine("If this method is successful then it should return a result of 50 and if not then the result will be 0.");
				Console.WriteLine($"The result is: {sampleClass.assertVariable}");
				Console.WriteLine();
			}

			using (var sampleClass = new SamplesClass())
			{
				var result = RetryHelper.RetryOnException<string>(3, 10,
					// retry method setup to fail twice before returning a value 
					() => sampleClass.DoSomethingWithErrorsWithReturn(),
					(e) => {
						return e is NullReferenceException;
					});
				Console.WriteLine("----------------------------------------");
				Console.WriteLine("Method with return:");
				Console.WriteLine(result);
			}

			Console.ReadKey();
		}
	}
}
