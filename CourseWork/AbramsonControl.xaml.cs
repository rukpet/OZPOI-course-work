using System;
using System.Windows;
using System.Windows.Controls;

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
			Polinom = Abramson.Code.Polinom.ToString("b");
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
