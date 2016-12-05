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

			decode.Click = (o, e) => {
				byte[] source = Encoding.ASCII.GetBytes(decode.Input);
				byte[] decodedSource = Abramson.Code.Decode(source);
				decode.Output = Encoding.ASCII.GetString(decodedSource);
			};
		}
	}
}
