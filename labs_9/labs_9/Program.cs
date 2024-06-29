using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace labs_9
{
	public class Program
	{
		public static void Main(string[] args) {

			CarList carList= new CarList();

			carList.cars.AddRange(new List<Car>(){
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


			var query1 = carList.cars
			.Where(car => car.model == "A6")
			.Select(car => new
			{
				engineType = (car.motor.model == "TDI") ? "diesel" : "petrol",
				hppl = car.motor.horsePower / car.motor.displacement
			});

			var query2 = query1
				.GroupBy(item => item.engineType)
				.Select(group => new
				{
					engineType = group.Key,
					AverageHppl = group.Average(item => item.hppl)
				});

			foreach(var group in query2)
			{
				Console.WriteLine($"{group.engineType}: {group.AverageHppl}");
			}

			string xmlFilePath =  @"cars.xml";
			SerializeToXml(carList, xmlFilePath);
			Console.WriteLine($"Zserializowano kolekcję myCars do pliku {xmlFilePath}.");

			
			CarList deserializedCarList = DeserializeFromXml(xmlFilePath);
			Console.WriteLine($"Zdeserializowano kolekcję myCars z pliku {xmlFilePath}.");

			foreach (var car in deserializedCarList.cars)
			{
				Console.WriteLine($"Model: {car.model}, Year: {car.year}, Engine: {car.motor.model}, HP: {car.motor.model}, Displacement: {car.motor.model}");
			}
			

			
			
			var rootNode = new XPathDocument(xmlFilePath);
			var nav = rootNode.CreateNavigator();

			string expressionAvgHorsePower = @"sum(/cars/carList/Car[engine/@model != 'TDI']/engine/horsePower) div count(/cars/carList/Car[engine/@model != 'TDI'])";





			// Wyświetl wyniki
			Console.WriteLine("Average horse power of non TDI engines: " + nav.Evaluate(expressionAvgHorsePower));

			XPathExpression expression = nav.Compile("/cars/carList/Car/model[not(. = preceding::Car/model)]");
			XPathNodeIterator iterator = nav.Select(expression);

			// Wyświetl wyniki
			Console.WriteLine("Car models no repetitions: ");
			while (iterator.MoveNext())
			{
				Console.WriteLine(iterator.Current.Value);
			}

			createXmlFromLinq(carList);


			XDocument template = XDocument.Load("template.html");

			// Generowanie tabeli na podstawie kolekcji myCars
			XElement table = new XElement("table",
				new XElement("tr",
					new XElement("th", "Model"),
					new XElement("th", "Year"),
					new XElement("th", "Engine Model"),
					new XElement("th", "Displacement"),
					new XElement("th", "Horse Power")
				),
				// Dodawanie wierszy do tabeli
				from car in carList.cars
				select new XElement("tr",
					new XElement("td", car.model),
					new XElement("td", car.year),
					new XElement("td", car.motor.model),
					new XElement("td", car.motor.displacement),
					new XElement("td", car.motor.horsePower)
				)
			);

			// Dodawanie tabeli do szablonu
			template.Root.Element("body").Add(table);

			// Zapisywanie dokumentu XHTML do pliku
			template.Save("output.html");

			XDocument doc = XDocument.Load(xmlFilePath);

			// Zmiana nazwy elementu horsePower na hp
			foreach (var car in doc.Descendants("Car"))
			{
				var horsePowerElement = car.Element("engine")?.Element("horsePower");
				if (horsePowerElement != null)
				{
					horsePowerElement.Name = "hp";
				}
			}

			// Zamiast elementu year utwórz atrybut o tej samej nazwie w elemencie model
			foreach (var car in doc.Descendants("Car"))
			{
				var yearElement = car.Element("year");
				var modelElement = car.Element("model");
				if (yearElement != null && modelElement != null)
				{
					modelElement.Add(new XAttribute("year", yearElement.Value));
					yearElement.Remove();
				}
			}

			// Zapisz zmodyfikowany dokument XML
			doc.Save("modified_cars.xml");

			Console.WriteLine("Dokument XML został pomyślnie zmodyfikowany i zapisany.");
		


	}

		private static void SerializeToXml(CarList carList, string filePath)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CarList));

			using (FileStream fs = new FileStream(filePath,FileMode.Create))
			{
				serializer.Serialize(fs, carList);
			}

		} 

		private static CarList DeserializeFromXml(string filePath)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CarList));

			using (FileStream fs = new FileStream(filePath, FileMode.Open))
			{
				return (CarList) serializer.Deserialize(fs);
			}

		}

		private static void createXmlFromLinq(CarList myCars)
		{
			// Zapytanie LINQ do mapowania obiektów Car na elementy XML
			IEnumerable<XElement> nodes =
				from car in myCars.cars
				select new XElement("Car",
							new XElement("model", car.model),
							new XElement("year", car.year),
							new XElement("engine",
								new XAttribute("model", car.motor.model),
								new XElement("displacement", car.motor.displacement),
								new XElement("horsePower", car.motor.horsePower)
							)
						);

			// Tworzymy węzeł zawierający wyniki zapytania
			XElement rootNode = new XElement("cars",
									new XElement("carList", nodes)
								);

			// Zapisujemy wyniki do pliku XML
			rootNode.Save("CarsFromLinq.xml");
		}




	}





}