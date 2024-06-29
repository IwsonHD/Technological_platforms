using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace labs_7
{
	
	internal static class FilesReader
	{
		[Serializable]
		private class EntryComparer : IComparer<string>, ISerializable
		{
			public int Compare(string x, string y)
			{
				int lengthComaprison = x.Length.CompareTo(y.Length);
				if (lengthComaprison != 0)
				{
					return lengthComaprison;
				}
				else
				{
					return string.Compare(x, y);
				}
			}

			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{

			}

			public EntryComparer(SerializationInfo info, StreamingContext context)
			{

			}

			public EntryComparer()
			{

			}

		}



		public static void listContent(string directory)
		{
			try
			{
				if (Directory.Exists(directory))
				{
					Console.WriteLine("Contents:");
					string[] files = Directory.GetFileSystemEntries(directory);
					foreach (string file in files)
					{
						Console.WriteLine($"  {file}");
					}
				}
				else
				{
					Console.WriteLine("Given folder does not exist");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error has arised");
			}
		}

		public static void recurisveListing(string directory)
		{
			try
			{
				if (Directory.Exists(directory))
				{
					Console.WriteLine("Contents:");
					int depth = 1;
					foreach(string path in Directory.GetFileSystemEntries(directory))
					{
						Console.WriteLine($"\t{Path.GetFileName(path)}");
						FileAttributes attributes = File.GetAttributes(path);
						if((attributes & FileAttributes.Directory) == FileAttributes.Directory)
						{
							recuriveListingLogic(depth + 1, path);
						}
					}
				}
				else
				{
					Console.WriteLine("Given folder does not exist");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error has arised");
			}

		}

		private static void recuriveListingLogic(int depth, string directory)
		{
			string[] contents = Directory.GetFileSystemEntries(directory);
			foreach(string path in contents)
			{	
				string fileName = Path.GetFileName(path);
				string outputString = fileName.PadLeft(fileName.Length + depth,'\t');

				Console.WriteLine(outputString);

				FileAttributes attributes = File.GetAttributes(path);
				if((attributes & FileAttributes.Directory) == FileAttributes.Directory)
				{
					recuriveListingLogic(depth + 1, path);
				}
			}
		}
		
		public static DateTime? oldestDirectoryEntry(this DirectoryInfo directoryInfo)
		{
			DateTime oldestEntryDate = DateTime.MaxValue;
			try
			{
				FileAttributes attributes = File.GetAttributes(directoryInfo.FullName);
				if ((attributes & FileAttributes.Directory) != FileAttributes.Directory) Console.WriteLine("Given path is not a directory");
				if (!directoryInfo.Exists) return null;
				FileInfo[] files = directoryInfo.GetFiles();
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				return logicOldestFolderEntry(directoryInfo, oldestEntryDate);
			}catch(Exception ex){
				Console.WriteLine("An error has arised");
				return null;
			}
		}

		private static DateTime logicOldestFolderEntry(DirectoryInfo directoryInfo, DateTime soFarTheOldest)
		{
			FileInfo[] files = directoryInfo.GetFiles();
			DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
			DateTime oldestEntryDate = soFarTheOldest;


			foreach(FileInfo file in files)
			{
				if(oldestEntryDate > file.CreationTime)
				{
					oldestEntryDate= file.CreationTime;
				}
			}
			foreach(DirectoryInfo directory in directoryInfos)
			{
				if(oldestEntryDate > directory.CreationTime)
				{
					oldestEntryDate = directory.CreationTime;
				}
				DateTime oldestSubDirEntry = logicOldestFolderEntry(directory, oldestEntryDate);
				if(oldestEntryDate > oldestSubDirEntry)
				{
					oldestEntryDate= oldestSubDirEntry;
				}
			}
			return oldestEntryDate;
		}
		public static string getDosAtributes(this FileSystemInfo fileSystemInfo)
		{
			return ((int)fileSystemInfo.Attributes).ToString("X4");
		}

		public static void extendedRecursiveFileListing(string directory)
		{
			try
			{
				if (Directory.Exists(directory))
				{
					Console.WriteLine("Contents:");
					int depth = 1;
					foreach (string path in Directory.GetFileSystemEntries(directory))
					{
						string entryName = Path.GetFileName(path);
						string dosAttributes = (new FileInfo(directory)).getDosAtributes();


						FileAttributes attributes = File.GetAttributes(path);
						if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
						{
							string amountOfEntries = Directory.GetFileSystemEntries(path).Length.ToString();
							Console.WriteLine($"\tname: {entryName} dosAttrs: {dosAttributes} size: {amountOfEntries}");
							extendedRecursiveListingLogic(depth + 1, path);
						}
						else
						{
							string fileSize = (new FileInfo(path).Length).ToString();
							Console.WriteLine($"\tname: {entryName} dosAttrs: {dosAttributes} size: {fileSize}");

						}
					}
				}
				else
				{
					Console.WriteLine("Given folder does not exist");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error has arised");
			}

		}

		private static void extendedRecursiveListingLogic(int depth, string directory)
		{
			string[] contents = Directory.GetFileSystemEntries(directory);
			foreach (string path in contents)
			{
				string entryName = Path.GetFileName(path);
				//string outputString = fileName.PadLeft(fileName.Length + depth, '\t');
				string dosAttributes = (new FileInfo(directory)).getDosAtributes();
				

				FileAttributes attributes = File.GetAttributes(path);
				if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
				{
					string amountOfEntries = Directory.GetFileSystemEntries(path).Length.ToString();
					string outputString = $"\tname: {entryName} dosAttrs: {dosAttributes} size: {amountOfEntries}";
					string formattedOutputString = outputString.PadLeft(outputString.Length + depth, '\t');
					Console.WriteLine(formattedOutputString);
					extendedRecursiveListingLogic(depth + 1, path);
				}
				else
				{
					string fileSize = (new FileInfo(path).Length).ToString();
					string outputString = $"\tname: {entryName} dosAttrs: {dosAttributes} size: {fileSize}";
					string formattedOutputString = outputString.PadLeft(outputString.Length + depth, '\t');
					Console.WriteLine(formattedOutputString);

				}
			}
		}

		public static SortedDictionary<string,int>? directoryEntriesAsDictionary(string directory)
		{
			try
			{
				if (Directory.Exists(directory))
				{
					SortedDictionary<string,int> nameSizeDictionary = new SortedDictionary<string,int>(new EntryComparer());
					string[] entries = Directory.GetFileSystemEntries(directory);
					foreach (string entry in entries)
					{
						FileAttributes attributes = File.GetAttributes(entry);
						if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
						{
							int amountOfEntries = Directory.GetFileSystemEntries(entry).Length;
							nameSizeDictionary.Add(entry, amountOfEntries);
						}
						else
						{
							int fileSize = (int)(new FileInfo(entry).Length);
							nameSizeDictionary.Add(entry, fileSize);
						}
					}
					return nameSizeDictionary;
				}
				else
				{
					Console.WriteLine("Given folder does not exist");
					return null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error has arised");
				return null;
			}
		}
		public static byte[] serializeDictionary(SortedDictionary<string, int> dictionary)
		{
			using(MemoryStream memoryStream= new MemoryStream())
			{
				new BinaryFormatter().Serialize(memoryStream, dictionary);
				return memoryStream.ToArray();
			}
		}

		public static SortedDictionary<string, int> deserializeDictionary(byte[] serializedData)
		{
			using (MemoryStream memoryStream = new MemoryStream(serializedData))
			{
				return (SortedDictionary<string, int>)new BinaryFormatter().Deserialize(memoryStream);
			}
		}


	}





}
