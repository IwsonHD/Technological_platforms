using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace labs_9
{
	public class Car
	{
		[XmlElement("model")]
		public string model { get; set; }
		[XmlElement("year")]
		public int year { get; set; }

		[XmlElement("engine")]
		public Engine motor { get; set; }

		public Car(string model,Engine motor, int year)
		{
			this.model = model;
			this.year = year;
			this.motor = motor;
		}
		public Car() { }
	}

	public class Engine
	{
		[XmlElement("displacement")]
		public double displacement { get; set; }
		[XmlElement("horsePower")]
		public double horsePower { get; set; }
		[XmlAttribute("model")]
		public string model { get; set; }

		public Engine(double displacement, double horsePower, string model)
		{
			this.displacement = displacement;
			this.horsePower = horsePower;
			this.model = model;
		}
		public Engine() { }
	}

	[XmlRoot("cars")]
	public class CarList
	{
		[XmlArray("carList")]
		[XmlArrayItem("Car")]
		public List<Car> cars { get; set;}

		public CarList()
		{
			cars = new List<Car>();
		}

	}





}
