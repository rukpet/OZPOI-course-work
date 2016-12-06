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
				byte[] source = Encoding.Default.GetBytes(encoder.Input);
				byte[] encodedSource = Abramson.Code.Encode(source);
				encoder.Output = Encoding.Default.GetString(encodedSource);
			};

			decoder.Click = (o, e) => {
				byte[] source = Encoding.Default.GetBytes(decoder.Input);
				byte[] encodedSource = Abramson.Code.Decode(source);
				decoder.Output = Encoding.Default.GetString(encodedSource);
			};
		}
	}
}
