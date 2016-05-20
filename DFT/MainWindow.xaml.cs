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

namespace DFT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            double[] BaseFunction = createSignal(1);
            double[] BaseFunction2 = createSignal(4);
            double[] BaseFunction3 = createSignal(20);
            List<double[]> signals = new List<double[]>();
            signals.Add(BaseFunction);
            signals.Add(BaseFunction2);
            signals.Add(BaseFunction3);

            double[] SyntheseSignal = Synthesise(signals);

            print_signal(BaseFunction, mypanel);
            print_signal(BaseFunction2, mypanel2);
            print_signal(BaseFunction3, mypanel3);
            print_signal(SyntheseSignal, mypanel4);

            double[] ReX = new double[128];
            for (int k = 0; k < ReX.Length; k++)
            {
                for (int n = 0; n< 256; n++)
                {
                    ReX[k] += SyntheseSignal[n] * Math.Sin(2.0 * Math.PI *k* n / 256);
                }
                ReX[k] *= -1; //nur für die visualisierung! 
            }

            print_signal(ReX, mypanel5);

        }


        private double[] Synthesise (List<double[]> signals)
        {
            double[] signal = new double[256];
            foreach (double[] s in signals)
                for (int i = 0; i < signal.Length;i++)
                {
                    signal[i] += s[i];
                }
            return signal;
        }

        private void print_signal(double[] signal, Canvas c)
        {
            Polyline p = getPolylineFrom(signal);
            c.Children.Add(p);
            Canvas.SetTop(p, c.Height / 2);
        }

        private double[] createSignal(int T)        //signaldurchläufe
        {
            double[] signal = new double[256];
            for (int i = 0; i < signal.Length; i++)
            {
                signal[i] = Math.Sin(2.0 * Math.PI * T / 256.0 * i);
            }
            return signal;
        }

        private double get_max_amplitude (double[] signal)
        {
            double max = 1;
            for (int i = 0; i < signal.Length; i++)
            {
                if (Math.Abs(signal[i]) > max)
                    max = Math.Abs(signal[i]);
            }
            return max;
        }

        private Polyline getPolylineFrom(double[] signal)
        {
            Polyline p = new Polyline();
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;
            p.StrokeThickness = 2;
            p.Stroke = blackBrush;

            double Wunit = mypanel.Width / signal.Length;
            double Hunit = mypanel.Height / (get_max_amplitude(signal)*3.2);

            for (int i = 0; i < signal.Length; i++)
            {
                p.Points.Add(new Point(Wunit * i, signal[i]*Hunit));

            }
            return p;
        }
    }
}
