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
using MukuBase;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        ValueAnimator animator = ValueAnimator.Empty;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            animator = ValueAnimator.Animate(0, 1000, 500, (o,f) => { Dispatcher.Invoke(() => { pbar.Value=f; }); },1);
        }

        private void End(object sender, RoutedEventArgs e)
        {
            animator.End();
        }

        private void Abort(object sender, RoutedEventArgs e)
        {
            animator.Abort();
        }
    }
}
