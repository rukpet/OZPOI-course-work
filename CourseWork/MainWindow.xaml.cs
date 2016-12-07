using System.Text;
using System.Windows;
using static CourseWork.Abramson.Utilities;

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
				int numberBits;
				byte[] encodedSource = Abramson.Code.Encode(source, out numberBits);
				encoder.Output = BitsToBinaryString(encodedSource, numberBits);
			};

			decoder.Click = (o, e) => {
				byte[] source = BinaryStringToBytes(decoder.Input);
				byte[] encodedSource = Abramson.Code.Decode(source);
				decoder.Output = Encoding.Default.GetString(encodedSource).TrimEnd('\0');
			};
		}
	}
}
