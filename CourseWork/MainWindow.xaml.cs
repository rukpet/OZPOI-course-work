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
				byte[] source = Encoding.GetEncoding(1251).GetBytes(encoder.Input);
				byte[] encodedSource = Abramson.Code.Encode(source);
				encoder.Output = Encoding.GetEncoding(1251).GetString(encodedSource);
			};

			decoder.Click = (o, e) => {
				byte[] source = Encoding.GetEncoding(1251).GetBytes(decoder.Input);
				byte[] encodedSource = Abramson.Code.Decode(source);
				decoder.Output = Encoding.GetEncoding(1251).GetString(encodedSource);
			};
		}
	}
}
