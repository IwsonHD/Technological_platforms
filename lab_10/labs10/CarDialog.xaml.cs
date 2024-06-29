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
using System.Windows.Shapes;


namespace labs10
{
	public partial class CarDialog : Window
	{
		public Car Car { get; private set; }

		public CarDialog(Car car = null)
		{
			InitializeComponent();

			if (car != null)
			{
				Car = car;
				txtModel.Text = Car.Model;
				txtYear.Text = Car.Year.ToString();
				txtDisplacement.Text = Car.Motor.Displacement.ToString();
				txtHorsePower.Text = Car.Motor.HorsePower.ToString();
				txtEngineModel.Text = Car.Motor.Model;
			}
			else
			{
				Car = new Car();
			}
		}

		private void btnOK_Click(object sender, RoutedEventArgs e)
		{
			Car.Model = txtModel.Text;
			Car.Year = int.Parse(txtYear.Text);
			Car.Motor = new Engine
			{
				Displacement = double.Parse(txtDisplacement.Text),
				HorsePower = double.Parse(txtHorsePower.Text),
				Model = txtEngineModel.Text
			};

			DialogResult = true;
			Close();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}
