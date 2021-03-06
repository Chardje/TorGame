using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        byte width = 2;
        byte height = 2;
        Label[,] pole;
        List<WinTime> wins = new List<WinTime>();//Список рекордів
        static Stopwatch stopwatch = new Stopwatch();//секундомір
        static Label TimeL;
        static TreeView RecordTable;

        static Thread time = new Thread(TimeOn);

        public MainWindow()
        {
            InitializeComponent();
            TimeL = TimeLabel;
            RecordTable = RecTable;
            if (File.Exists("rec.txt")) RecordTable.Items.Add(File.ReadAllText("rec.txt"));

            SizeV.Content = height;
            SizeH.Content = width;
            SetPole(width, height);
            time.Start();
        }
        /// <summary>
        /// create pole size w*h
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        void SetPole(int w, int h)
        {
            pole = new Label[w, h];
            w += 2;
            h += 2;

            GridGame.Children.Clear();
            //add or remove column
            do
            {
                if (GridGame.ColumnDefinitions.Count > h) GridGame.ColumnDefinitions.RemoveAt(GridGame.ColumnDefinitions.Count - 1);

                else if (GridGame.ColumnDefinitions.Count < h) GridGame.ColumnDefinitions.Add(new ColumnDefinition());

            } while (GridGame.ColumnDefinitions.Count != h);

            //add or remove row
            do
            {
                if (GridGame.RowDefinitions.Count > w) GridGame.RowDefinitions.RemoveAt(GridGame.RowDefinitions.Count - 1);

                else if (GridGame.RowDefinitions.Count < w) GridGame.RowDefinitions.Add(new RowDefinition());

            } while (GridGame.RowDefinitions.Count != w);


            for (byte y = 0; y < h; y++)
            {
                for (byte x = 0; x < w; x++)
                {
                    //create labels in center
                    if (x > 0 && y > 0 && x < w - 1 && y < h - 1)//Создание лейблов в центре поля
                    {
                        pole[x - 1, y - 1] = new Label();
                        ref Label p = ref pole[x - 1, y - 1];
                        byte c = (byte)(256 / (width * height) * (y + (x - 1) * height));
                        p.Background = new SolidColorBrush(Color.FromRgb(c, c, c));
                        p.Foreground = new SolidColorBrush(Color.FromRgb((byte)(128 + c), (byte)(128 + c), (byte)(128 + c)));
                        p.Margin = new Thickness(2, 2, 2, 2);
                        p.Content = y + (x - 1) * height;
                        p.FontSize = 20;
                        p.MinHeight = 30;
                        p.MinWidth = 30;

                        Grid.SetRow(pole[x - 1, y - 1], x);
                        Grid.SetColumn(pole[x - 1, y - 1], y);

                        GridGame.Children.Add(pole[x - 1, y - 1]);
                    }
                    //angles
                    else if ((x == 0 && y == 0) || (x == w - 1 && y == h - 1) || (x == 0 && y == h - 1) || (x == w - 1 && y == 0))
                    { }
                    //create buttons
                    else
                    {
                        Button b = new Button();
                        if (x == 0) b = CreButt("↑", y);
                        else if (x == w - 1) b = CreButt("↓", y);
                        else if (y == 0) b = CreButt("←", x);
                        else if (y == h - 1) b = CreButt("→", x);

                        Grid.SetRow(b, x);
                        Grid.SetColumn(b, y);

                        GridGame.Children.Add(b);
                    }

                }
            }
            Button CreButt(string s, int num)
            {
                Button B = new Button();
                B.Background = Brushes.Gray;
                B.Foreground = Brushes.White;
                B.Margin = new Thickness(2, 2, 2, 2);
                B.Content = s;
                B.Click += ButtonClickPole;
                B.FontSize = 20;
                B.MinHeight = 30;
                B.MinWidth = 30;

                if (s == "←") s = "l";
                else if (s == "→") s = "r";
                else if (s == "↑") s = "u";
                else if (s == "↓") s = "d";

                B.Name = s + num;

                return B;
            }
            Rest();
        }

        /// <summary>
        /// Rotate labels in pole when click button in pole
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">link to button</param>
        void ButtonClickPole(object sender, RoutedEventArgs e)
        {
            Button b = ((Button)e.Source);
            bool plus;
            bool vert;
            int z = Convert.ToInt32(b.Name.Remove(0, 1));

            switch (b.Name[0])
            {
                case 'l'://left
                    vert = false;
                    plus = true;
                    break;
                case 'r'://right
                    vert = false;
                    plus = false;
                    break;
                case 'u'://up
                    vert = true;
                    plus = true;
                    break;
                case 'd'://down
                    vert = true;
                    plus = false;
                    break;
                default:
                    vert = false;
                    plus = false;
                    break;
            }

            int f;
            Swich Swicher;
            if (vert)
            {
                //if vertical switch
                f = height;
                Swicher = SwichR;
            }
            else
            {
                //if horizontal swich
                f = width;
                Swicher = SwichC;
            }
            if (plus)
            {
                for (int i = 0; i < f - 1; i++)
                {
                    Swicher(ref pole[vert ? i : z - 1, vert ? z - 1 : i]
                          , ref pole[vert ? i + 1 : z - 1, vert ? z - 1 : i + 1]);
                    //swich in pole array
                    (pole[vert ? i : z - 1, vert ? z - 1 : i],
                     pole[vert ? i + 1 : z - 1, vert ? z - 1 : i + 1])
                  = (pole[vert ? i + 1 : z - 1, vert ? z - 1 : i + 1],
                     pole[vert ? i : z - 1, vert ? z - 1 : i]);
                }
            }
            else
            {
                for (int i = f - 1; i > 0; i--)
                {
                    Swicher(ref pole[vert ? i : z - 1, vert ? z - 1 : i]
                          , ref pole[vert ? i - 1 : z - 1, vert ? z - 1 : i - 1]);
                    //swich in pole array
                    (pole[vert ? i : z - 1, vert ? z - 1 : i],
                     pole[vert ? i - 1 : z - 1, vert ? z - 1 : i - 1])
                  = (pole[vert ? i - 1 : z - 1, vert ? z - 1 : i - 1],
                     pole[vert ? i : z - 1, vert ? z - 1 : i]);
                }
            }
            if (!stopwatch.IsRunning) Start();
            CheckWin();

            void SwichR(ref Label l1, ref Label l2)//swich row
            {                
                int x = Grid.GetRow(l2);
                Grid.SetRow(l2, Grid.GetRow(l1));
                Grid.SetRow(l1, x);
            }
            
            void SwichC(ref Label l1, ref Label l2)//swich column
            {
                int x = Grid.GetColumn(l2);
                Grid.SetColumn(l2, Grid.GetColumn(l1));
                Grid.SetColumn(l1, x);
            }
        }
        delegate void Swich(ref Label l1, ref Label l2);

        class WinTime
        {
            int horisontalSize;
            int vertikalSize;
            string nick;
            string time;

            public WinTime(int HSize, int VSize, string TimeToWin, string Nick)
            {
                horisontalSize = HSize;
                vertikalSize = VSize;
                nick = Nick;
                time = TimeToWin;
                RecordTable.Items.Add(this);
            }

            public override string ToString()
            {
                return horisontalSize + "/" + vertikalSize + "|" + time + "|" + nick;
            }
        }

        private void VClick(object sender, RoutedEventArgs e)
        {

            if (((Button)e.Source).Content.ToString() == "↑") { if (width < 10) width++; }
            else { if (width > 1) width--; }

            SizeV.Content = width;
            SetPole(width, height);
        }
        private void HClick(object sender, RoutedEventArgs e)
        {
            if (((Button)e.Source).Content.ToString() == "→") { if (height < 10) height++; }
            else { if (height > 1) height--; }

            SizeH.Content = height;
            SetPole(width, height);
        }
        void Start()
        {
            stopwatch.Restart();
        }
        void Stop()
        {
            stopwatch.Stop();
        }
        void CheckWin()
        {
            int i = 0;
            foreach (Label l in pole)
            {
                i++;
                if (l.Content.ToString() != i.ToString())//if 1 of all  labels no in hir palse code end
                    return;
            }
            Stop();
            wins.Add(new WinTime(width, height, stopwatch.Elapsed.ToString(), TextBoxNick.Text));
        }
        void Rest()
        {
            Random R = new Random();
            Swich S = Sich;
            int rx, ry;

            ///randomize labels
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    rx = R.Next(width - x) + x;
                    ry= R.Next(height - y) + y;
                    S(ref pole[x, y], ref pole[rx, ry]);
                    (pole[x, y], pole[rx, ry]) = (pole[rx, ry], pole[x, y]);//swich in pole
                }
            }
            void Sich(ref Label l1, ref Label l2)
            {
                int g = Grid.GetRow(l2);
                Grid.SetRow(l2, Grid.GetRow(l1));
                Grid.SetRow(l1, g);

                g = Grid.GetColumn(l2);
                Grid.SetColumn(l2, Grid.GetColumn(l1));
                Grid.SetColumn(l1, g);
            }
            Stop();
        }
        void Rest(object sender, RoutedEventArgs e) => Rest();


        static void TimeOn()
        {
            try
            {
                while (true)
                {
                    string sw = stopwatch.Elapsed.ToString();
                    TimeL.Dispatcher.Invoke(() => { TimeL.Content = "Time:" + sw; });
                    Thread.Sleep(10);

                }
            }
            catch (Exception ex) { }//ignor        
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!File.Exists("rec.txt")) File.Create("rec.txt");
            List<string> list = new List<string>();
            foreach (object i in RecordTable.Items) list.Add(i.ToString());
            File.WriteAllLines("rec.txt", list);
        }
    }
}
