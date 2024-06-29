using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

using System.Text.RegularExpressions;

namespace techPlats8
{
	public partial class CreateItemDialog : Window
	{
		private TreeViewItem parentItem;

		public CreateItemDialog(TreeViewItem parent)
		{
			InitializeComponent();
			parentItem = parent;
			// Domyślnie ustaw typ na "Folder"
			itemTypeComboBox.SelectedIndex = 1;
		}

		// Obsługa przycisku "Create"
		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			string itemName = itemNameTextBox.Text;
			string itemType = itemTypeComboBox.SelectedItem.ToString().Split(" ").Last();

			// Weryfikacja nazwy
			if (string.IsNullOrWhiteSpace(itemName))
			{
				System.Windows.MessageBox.Show("Name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				Close();
			}

			string parentPath = parentItem.Tag.ToString();
			string newPath = System.IO.Path.Combine(parentPath, itemName);

			bool isNameValid = false;
			if (itemType == "File")
			{
				// Sprawdzenie poprawności nazwy pliku
				string filePattern = @"^[a-zA-Z0-9_\-\~]{1,8}\..*$";
				isNameValid = Regex.IsMatch(itemName, filePattern);
			}
			else if (itemType == "Folder")
			{
				// Sprawdzenie, czy nazwa folderu nie zawiera kropki
				string folderPattern = @"^[a-zA-Z0-9_\-\~]{1,8}$";
				isNameValid = Regex.IsMatch(itemName, folderPattern);
			}

			if (!isNameValid)
			{
				System.Windows.MessageBox.Show("Invalid name format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				Close();

			}
			else
			{


				try
				{
					// Tworzenie pliku lub folderu
					if (itemType == "File")
					{
						// Tworzenie pliku
						File.Create(newPath).Close();
					}
					else if (itemType == "Folder")
					{
						// Tworzenie folderu
						Directory.CreateDirectory(newPath);
					}

					// Dodanie nowego elementu do TreeView
					TreeViewItem newItem = new TreeViewItem
					{
						Header = itemName,
						Tag = newPath
					};
					parentItem.Items.Add(newItem);

					// Ustawianie atrybutów DOS'owych dla nowego pliku lub folderu
					FileAttributes attributes = FileAttributes.Normal;

					// Sprawdź pola wyboru atrybutów i dodaj odpowiednie flagi
					if (readOnlyCheckBox.IsChecked == true)
					{
						attributes |= FileAttributes.ReadOnly;
					}
					if (hiddenCheckBox.IsChecked == true)
					{
						attributes |= FileAttributes.Hidden;
					}
					if (systemCheckBox.IsChecked == true)
					{
						attributes |= FileAttributes.System;
					}
					if (archiveCheckBox.IsChecked == true)
					{
						attributes |= FileAttributes.Archive;
					}

					// Ustaw atrybuty pliku lub folderu
					if (itemType == "File")
					{
						File.SetAttributes(newPath, attributes);
					}


					// Zamknięcie okna dialogowego
					Close();
				}
				catch (Exception ex)
				{
					System.Windows.MessageBox.Show($"Error creating item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}
	}
}
