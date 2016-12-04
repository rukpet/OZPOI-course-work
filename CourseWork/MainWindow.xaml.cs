﻿using System;
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
