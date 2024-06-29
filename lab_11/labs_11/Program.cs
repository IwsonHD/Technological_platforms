using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

class Program
{
	static async Task Main(string[] args)
	{
		// Zadanie 1 - Symbol Newtona
		int N = 5;
		int K = 3;
		int result1 = await CalculateNewtonSymbolAsync(N, K);
		Console.WriteLine($"Symbol Newtona dla N={N} i K={K} (async-await): {result1}");

		int result2 = await CalculateNewtonSymbolUsingTasks(N, K);
		Console.WriteLine($"Symbol Newtona dla N={N} i K={K} (Task<T>): {result2}");

		// Zadanie 2 - Ciąg Fibonacciego z użyciem BackgroundWorker
		int fibIndex = 10; // Numer wyrazu ciągu Fibonacciego do obliczenia
		BackgroundWorker fibWorker = new BackgroundWorker();
		fibWorker.DoWork += (sender, e) => { e.Result = Fibonacci(fibIndex, sender as BackgroundWorker); };
		fibWorker.ProgressChanged += (sender, e) => { Console.WriteLine($"Postęp: {e.ProgressPercentage}%"); };
		fibWorker.RunWorkerCompleted += (sender, e) => { Console.WriteLine($"Wynik ciągu Fibonacciego dla indeksu {fibIndex}: {e.Result}"); };
		fibWorker.WorkerReportsProgress = true;
		fibWorker.RunWorkerAsync();

		// Zadanie 3 - Kompresja plików współbieżnie z GZipStream
		string directoryPath = args[0].ToString() ;
		await CompressFilesAsync(directoryPath);
		Console.WriteLine("Kompresja zakończona.");

		Console.ReadLine(); // Zapobieganie zakończeniu programu przed zakończeniem działania worker'a
	}

	// Zadanie 1 - Symbol Newtona z użyciem async-await
	static async Task<int> CalculateNewtonSymbolAsync(int N, int K)
	{
		int numerator = await Task.Run(() => CalculateFactorial(N));
		int denominator = await Task.Run(() => CalculateFactorial(K) * CalculateFactorial(N - K));

		return numerator / denominator;
	}

	// Zadanie 1 - Symbol Newtona z użyciem Task<T>
	static async Task<int> CalculateNewtonSymbolUsingTasks(int N, int K)
	{
		Task<int> task1 = Task.Run(() => CalculateFactorial(N));
		Task<int> task2 = Task.Run(() => CalculateFactorial(K));
		Task<int> task3 = Task.Run(() => CalculateFactorial(N - K));

		int numerator = await task1;
		int denominator = await task2 * await task3;

		return numerator / denominator;
	}

	static int CalculateFactorial(int n)
	{
		int factorial = 1;
		for (int i = 2; i <= n; i++)
		{
			factorial *= i;
		}
		return factorial;
	}

	// Zadanie 2 - Obliczenie i-tego wyrazu ciągu Fibonacciego z użyciem BackgroundWorker
	static int Fibonacci(int n, BackgroundWorker worker)
	{
		int a = 0, b = 1, c = 0;
		for (int i = 2; i <= n; i++)
		{
			c = a + b;
			a = b;
			b = c;
			Thread.Sleep(5); // Spowolnienie pętli
			worker.ReportProgress((int)((i / (float)n) * 100));
		}
		return n == 0 ? 0 : b;
	}

	// Zadanie 3 - Kompresja plików współbieżnie z GZipStream
	static async Task CompressFilesAsync(string directoryPath)
	{
		string[] files = Directory.GetFiles(directoryPath);
		await Task.WhenAll(files.Select(file => Task.Run(() =>
		{
			string compressedFilePath = $"{file}.gz";
			using (FileStream originalFileStream = File.OpenRead(file))
			{
				using (FileStream compressedFileStream = File.Create(compressedFilePath))
				{
					using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
					{
						originalFileStream.CopyTo(compressionStream);
					}
				}
			}
			Console.WriteLine($"Plik {file} został skompresowany.");
		})));
	}
}