using System;
namespace labs_7
{
	class Program
	{
		static void Main(string[] args)
		{

			//FilesReader.listContent(args[0]);
			//FilesReader.recurisveListing(args[0]);
			//DirectoryInfo directoryInfo = new DirectoryInfo(args[0]);
			//FileInfo fileSystemInfo= new FileInfo(args[0]);
			//Console.WriteLine(directoryInfo.oldestDirectoryEntry());
			//Console.WriteLine(fileSystemInfo.getDosAtributes());
			FilesReader.extendedRecursiveFileListing(args[0]);

			//SortedDictionary<string, int>? entriesDictionary = FilesReader.directoryEntriesAsDictionary(args[0]);

			//if(entriesDictionary != null) {
			//	entriesDictionary.ToList().ForEach(kvp =>
			//	{
			//		Console.WriteLine($"name: {kvp.Key} size: {kvp.Value}");
			//	});

			//	byte[] serializedData = FilesReader.serializeDictionary(entriesDictionary);
			//	SortedDictionary<string, int> deserializedData = FilesReader.deserializeDictionary(serializedData);

			//	deserializedData.ToList().ForEach(kvp =>
			//	{
			//		Console.WriteLine($"name: {kvp.Key} size: {kvp.Value}");
			//	});
			//}
		}
	}
}