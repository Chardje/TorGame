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
        Label[,] pole;
        public MainWindow()
        {
            InitializeComponent();
            SizeV.Content = height;
            SizeH.Content = width;
            SetPole(width,height);
        }

        void SetPole(int w,int h)
        {
            pole = new Label[w,h];
            w+=2;
            h+=2;
            GridGame.Children.Clear();
            do
            {
                if (GridGame.ColumnDefinitions.Count > h)   GridGame.ColumnDefinitions.RemoveAt(GridGame.ColumnDefinitions.Count - 1);

                else if (GridGame.ColumnDefinitions.Count < h)  GridGame.ColumnDefinitions.Add(new ColumnDefinition());

            } while (GridGame.ColumnDefinitions.Count != h);
            do
            {
                if (GridGame.RowDefinitions.Count > w) GridGame.RowDefinitions.RemoveAt(GridGame.RowDefinitions.Count - 1);

                else if (GridGame.RowDefinitions.Count < w) GridGame.RowDefinitions.Add(new RowDefinition());

            } while (GridGame.RowDefinitions.Count != w);

            
            for (int Hi = 0; Hi < h; Hi++)
            {
                for (int Wi = 0; Wi < w; Wi++)
                {
                    if (Wi > 0 && Hi > 0 && Wi < w - 1 && Hi < h - 1)//центр
                    {
                        pole[Wi - 1, Hi - 1] = new Label();
                        ref Label p = ref pole[Wi - 1, Hi - 1];
                        p.Background = Brushes.Gray;
                        p.Foreground = Brushes.White;
                        p.Margin = new Thickness(2, 2, 2, 2);
                        p.Content = Wi+""+Hi;
                        p.FontSize = 20;
                        p.MinHeight = 30;
                        p.MinWidth = 30;

                        Grid.SetRow(pole[Wi - 1, Hi - 1], Wi);
                        Grid.SetColumn(pole[Wi - 1, Hi - 1], Hi);

                        GridGame.Children.Add(pole[Wi - 1, Hi - 1]);
                    }
                    else if (Wi == Hi || (Wi == 0 && Hi == h - 1) || (Wi == w - 1 && Hi == 0))//якщо кут
                    { }
                    else if (Wi == 0) 
                    {
                        Button b = CreButt("←", Hi);
                        Grid.SetRow(b, Hi);//
                        Grid.SetColumn(b, Wi);//

                        GridGame.Children.Add(b);
                    }
                    else if (Wi == w - 1) 
                    {
                        Button b = CreButt("→", Hi);
                        Grid.SetRow(b, Hi);//
                        Grid.SetColumn(b, Wi);//

                        GridGame.Children.Add(b);
                    }
                    else if (Hi == 0) 
                    {
                        Button b = CreButt("↑", Wi);
                        Grid.SetRow(b, Hi);//
                        Grid.SetColumn(b, Wi);//

                        GridGame.Children.Add(b);
                    }
                    else if (Hi == h - 1) 
                    {
                        Button b = CreButt("↓", Wi);
                        Grid.SetRow(b, Hi);//
                        Grid.SetColumn(b, Wi);//

                        GridGame.Children.Add(b);
                    }
                }
            }
            Button CreButt(string s,int num)
            {
                Button B=new Button();                
                B.Background = Brushes.Gray;
                B.Foreground = Brushes.White;
                B.Margin = new Thickness(2, 2, 2, 2);
                B.Content = s;
                B.Click += ButtonClickPole;
                B.FontSize = 20;
                B.MinHeight = 30;
                B.MinWidth = 30;

                if (s == "←")s = "l";
                else if(s == "→") s = "r";
                else if(s == "↑") s = "u";
                else if(s == "↓") s = "d";

                B.Name = s+num;

                return B;
            }
        }
        void ButtonClickPole(object sender, RoutedEventArgs e)
        {

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
            
            public override string ToString()
            {
                return horisontalSize + "/" + vertikalSize + "|" + time.ToString() + "|" + nick;
            }
        }

        private void VClick(object sender, RoutedEventArgs e)
        {
            
            if (((Button)e.Source).Content.ToString()== "↑") width++;
            else width--;

            SizeV.Content = height;
            SetPole(width, height);

        }
        private void HClick(object sender, RoutedEventArgs e)
        {
            if (((Button)e.Source).Content.ToString() == "→") height++;
            else height--;

            SizeH.Content = width;
            SetPole(width, height);

        }
        void Start()
        {

        }
        void Stop()
        {

        }
        void CheckWin()
        {

        }
        void Rest()
        {

        }
        private void Rest(object sender, RoutedEventArgs e)
        {

        }
    }
}
