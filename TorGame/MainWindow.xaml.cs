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

namespace TorGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int width=2;
        int height=2;
        public MainWindow()
        {
            InitializeComponent();
            SizeV.Content = height;
            SizeH.Content = width;
        }

        void SetPole(int h,int w)
        {
            w++;
            h++;
            do
            {
                if (GridGame.ColumnDefinitions.Count > w)   GridGame.ColumnDefinitions.RemoveAt(GridGame.ColumnDefinitions.Count - 1);

                else if (GridGame.ColumnDefinitions.Count < w)  GridGame.ColumnDefinitions.Add(new ColumnDefinition());

            } while (GridGame.ColumnDefinitions.Count == w);
            do
            {
                if (GridGame.RowDefinitions.Count > h) GridGame.RowDefinitions.RemoveAt(GridGame.RowDefinitions.Count - 1);

                else if (GridGame.RowDefinitions.Count < h) GridGame.RowDefinitions.Add(new RowDefinition());

            } while (GridGame.RowDefinitions.Count == h);

            for (int Wi=0;Wi<w;Wi++)
            {

            }
        }
        class WinTime 
        {
            int horisontalSize;
            int vertikalSize;
            string nick;
            DateTime time;

            WinTime(int HSize, int VSize, DateTime TimeToWin, string Nick)
            {
                horisontalSize = HSize;
                vertikalSize = VSize;
                nick = Nick;
                time = TimeToWin;
            }
            
            string ToString()
            {
                return horisontalSize + "/" + vertikalSize + "|" + time.ToString() + "|" + nick;
            }
        }


    }
}
