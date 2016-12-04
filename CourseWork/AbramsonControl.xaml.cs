using CourseWork.Abramson;
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

namespace CourseWork.GUI
{
	/// <summary>
	/// Interaction logic for AbramsonControl.xaml
	/// </summary>
	public partial class AbramsonControl : UserControl
	{
		public AbramsonControl()
		{
			InitializeComponent();
			Polinom = Abramson.Code.Polinom.ToString();
		}

		public string Input
		{
			get { return input.Text; }
			set { input.Text = value; }
		}

		public string Output
		{
			get { return output.Text; }
			set { output.Text = value; }
		}

		public string Polinom
		{
			get { return polinom.Text; }
			set { polinom.Text = value; }
		}

		private Action<object, RoutedEventArgs> clickAction;

		public Action<object, RoutedEventArgs> Click
		{
			get { return clickAction; }
			set { clickAction = value; }
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Click?.Invoke(sender, e);
		}
	}
}
