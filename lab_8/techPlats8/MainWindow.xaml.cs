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








	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		// Funkcja otwierania folderu
		private void OpenFolder(object sender, RoutedEventArgs e)
		{
			// Tworzenie FolderBrowserDialog
			using (FolderBrowserDialog dlg = new FolderBrowserDialog())
			{
				dlg.Description = "Select directory to open";
				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					// Ładowanie struktury plików
					LoadDirectoryStructure(dlg.SelectedPath);
				}
			}
		}

		// Funkcja zamykania aplikacji
		private void ExitApplication(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}

		// Funkcja ładowania struktury plików do TreeView
		private void LoadDirectoryStructure(string path)
		{
			// Tworzenie korzenia TreeView
			FilesTreeView.Items.Clear();
			TreeViewItem root = CreateTreeViewItem(path);
			FilesTreeView.Items.Add(root);
		}

		// Tworzenie elementu TreeViewItem
		private TreeViewItem CreateTreeViewItem(string path)
		{
			TreeViewItem item = new TreeViewItem
			{
				Header = System.IO.Path.GetFileName(path),
				Tag = path
			};

			// Dodawanie menu kontekstowego
			item.ContextMenu = CreateContextMenu(item);

			// Dodawanie elementów podrzędnych (folderów i plików)
			if (Directory.Exists(path))
			{
				foreach (var dir in Directory.GetDirectories(path))
				{
					item.Items.Add(CreateTreeViewItem(dir));
				}

				foreach (var file in Directory.GetFiles(path))
				{
					item.Items.Add(CreateTreeViewItem(file));
				}
			}

			return item;
		}

		// Tworzenie menu kontekstowego
		private ContextMenu CreateContextMenu(TreeViewItem item)
		{
			ContextMenu menu = new ContextMenu();

			// Opcja Delete
			MenuItem deleteItem = new MenuItem
			{
				Header = "Delete"
			};
			deleteItem.Click += (sender, e) => DeleteItem(item);
			menu.Items.Add(deleteItem);

			// Opcja Create (dla folderów)
			if (Directory.Exists(item.Tag.ToString()))
			{
				MenuItem createItem = new MenuItem
				{
					Header = "Create"
				};
				createItem.Click += (sender, e) => CreateItem(item);
				menu.Items.Add(createItem);
			}

			// Opcja Open (dla plików)
			if (File.Exists(item.Tag.ToString()))
			{
				MenuItem openItem = new MenuItem
				{
					Header = "Open"
				};
				openItem.Click += (sender, e) => OpenFile(item);
				menu.Items.Add(openItem);
			}

			return menu;
		}

		// Funkcja usuwania elementu
		private void DeleteItem(TreeViewItem item)
		{
			string path = item.Tag.ToString();

			// Usunięcie pliku lub folderu z dysku
			if (File.Exists(path))
			{
				File.SetAttributes(path, FileAttributes.Normal);
				File.Delete(path);
			}
			else if (Directory.Exists(path))
			{
				Directory.Delete(path, true);
			}

			// Usunięcie elementu z TreeView
			TreeViewItem parent = item.Parent as TreeViewItem;
			parent?.Items.Remove(item);
		}

		// Funkcja otwierania pliku
		private void OpenFile(TreeViewItem item)
		{
			string path = item.Tag.ToString();

			if (File.Exists(path))
			{
				// Odczyt zawartości pliku do TextBlock
				FileContentTextBlock.Text = File.ReadAllText(path);
			}
		}

		// Funkcja tworzenia nowego elementu
		private void CreateItem(TreeViewItem parent)
		{
			// Wyświetl formularz do wprowadzenia nowego pliku lub folderu
			CreateItemDialog dialog = new CreateItemDialog(parent);
			dialog.ShowDialog();
		}

		// Funkcja wyświetlania atrybutów DOS w pasku stanu
		private void DisplayAttributes(string path)
		{
			if (File.Exists(path) || Directory.Exists(path))
			{
				FileAttributes attributes = File.GetAttributes(path);
				string attrText = string.Empty;

				// Tworzenie tekstu atrybutów
				attrText += (attributes.HasFlag(FileAttributes.ReadOnly) ? "r" : "-");
				attrText += (attributes.HasFlag(FileAttributes.Archive) ? "a" : "-");
				attrText += (attributes.HasFlag(FileAttributes.System) ? "s" : "-");
				attrText += (attributes.HasFlag(FileAttributes.Hidden) ? "h" : "-");

				// Ustawienie tekstu w StatusBar
				FileAttributesStatusBar.Text = $"Attributes: {attrText}";
			}
		}

		// Funkcja obsługi menu kontekstowego
		private void FilesTreeView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			// Pobranie zaznaczonego elementu
			TreeViewItem selectedItem = FilesTreeView.SelectedItem as TreeViewItem;

			// Wyświetlenie atrybutów dla zaznaczonego elementu
			if (selectedItem != null)
			{
				DisplayAttributes(selectedItem.Tag.ToString());
			}
		}
	}


}
