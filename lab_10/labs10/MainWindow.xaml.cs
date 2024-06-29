using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace labs10
{
	public partial class MainWindow : Window
	{
		private SortableSearchableBindingList<Car> myCars;
		private ICollectionView carCollectionView;

		public MainWindow()
		{
			InitializeComponent();

			CarList carList = new CarList();
			carList.Cars.AddRange(new List<Car>{
				new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
				new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
				new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
				new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
				new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
				new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
				new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
				new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
				new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
			});

			myCars = new SortableSearchableBindingList<Car>(carList.Cars);
			carCollectionView = CollectionViewSource.GetDefaultView(myCars);
			dataGridCars.ItemsSource = carCollectionView;

			Comparison<Car> arg1 = delegate (Car car1, Car car2)
			{
				return car2.Motor.HorsePower.CompareTo(car1.Motor.HorsePower);
			};

			Predicate<Car> arg2 = delegate (Car car)
			{
				return car.Motor.Model.Contains("TDI");
			};

			Action<Car> arg3 = delegate (Car car)
			{
				MessageBox.Show(car.Model + " - " + car.Motor.Model + " - " + car.Motor.HorsePower + " HP");
			};

			// Użycie zmiennych arg1, arg2 i arg3
			List<Car> carListForSorting = new List<Car>(myCars);
			carListForSorting.Sort(arg1);
			carListForSorting.FindAll(arg2).ForEach(arg3);

			ExecuteLinqQueries();
		}

		private void ExecuteLinqQueries()
		{
			// Query Expression Syntax
			var queryExpressionSyntax = from car in myCars
										where car.Model == "A6"
										group car by car.Motor.Model.Contains("TDI") ? "diesel" : "petrol" into g
										let avgHPPL = g.Average(car => car.Motor.HorsePower / car.Motor.Displacement)
										orderby avgHPPL descending
										select new
										{
											engineType = g.Key,
											avgHPPL
										};

			string queryExpressionResult = "Query Expression Syntax:\n";
			foreach (var e in queryExpressionSyntax)
			{
				queryExpressionResult += $"{e.engineType}: {e.avgHPPL}\n";
			}

			// Method-Based Query Syntax
			var methodBasedQuerySyntax = myCars
				.Where(car => car.Model == "A6")
				.GroupBy(car => car.Motor.Model.Contains("TDI") ? "diesel" : "petrol")
				.Select(g => new
				{
					engineType = g.Key,
					avgHPPL = g.Average(car => car.Motor.HorsePower / car.Motor.Displacement)
				})
				.OrderByDescending(x => x.avgHPPL);

			string methodBasedQueryResult = "Method-Based Query Syntax:\n";
			foreach (var e in methodBasedQuerySyntax)
			{
				methodBasedQueryResult += $"{e.engineType}: {e.avgHPPL}\n";
			}

			txtLinqResults.Text = queryExpressionResult + "\n" + methodBasedQueryResult;
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new CarDialog();
			if (dialog.ShowDialog() == true)
			{
				myCars.Add(dialog.Car);
				ExecuteLinqQueries(); // Aktualizacja wyników zapytań LINQ
			}
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridCars.SelectedItem is Car selectedCar)
			{
				myCars.Remove(selectedCar);
				ExecuteLinqQueries(); // Aktualizacja wyników zapytań LINQ
			}
		}

		private void btnSearch_Click(object sender, RoutedEventArgs e)
		{
			string searchText = txtSearch.Text;
			var prop = TypeDescriptor.GetProperties(typeof(Car))["Model"];
			int index = myCars.Find(prop, searchText);
			if (index != -1)
			{
				var car = myCars[index];
				MessageBox.Show($"Found: {car.Model} - {car.Motor.Model} - {car.Motor.HorsePower} HP");
			}
			else
			{
				MessageBox.Show("No matching car found.");
			}
		}

		private void btnResetSearch_Click(object sender, RoutedEventArgs e)
		{
			txtSearch.Clear();
		}
	}
}
