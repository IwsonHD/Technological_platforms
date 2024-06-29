using System;
using System.Collections.Generic;

namespace labs10
{
	public class Car
	{
		public string Model { get; set; }
		public int Year { get; set; }
		public Engine Motor { get; set; }

		public Car(string model, Engine motor, int year)
		{
			this.Model = model;
			this.Year = year;
			this.Motor = motor;
		}
		public Car() { }
	}

	public class Engine : IComparable<Engine>
	{
		public double Displacement { get; set; }
		public double HorsePower { get; set; }
		public string Model { get; set; }

		public Engine(double displacement, double horsePower, string model)
		{
			this.Displacement = displacement;
			this.HorsePower = horsePower;
			this.Model = model;
		}
		public Engine() { }

		public int CompareTo(Engine other)
		{
			return this.HorsePower.CompareTo(other.HorsePower);
		}
	}

	public class CarList
	{
		public List<Car> Cars { get; set; }

		public CarList()
		{
			Cars = new List<Car>();
		}
		public CarList(List<Car> cars)
		{
			this.Cars = cars;
		}
	}
}
