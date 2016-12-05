using System.Text;
using System.Windows;

namespace CourseWork.GUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			encoder.Click = (o, e) => {
				byte[] source = Encoding.ASCII.GetBytes(encoder.Input);
				byte[] encodedSource = Abramson.Code.Encode(source);
				encoder.Output = Encoding.ASCII.GetString(encodedSource);
			};
		}
	}
}
