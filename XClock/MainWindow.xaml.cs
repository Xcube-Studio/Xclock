using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SourceChord.FluentWPF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace XClock
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        /// <summary>
        /// 得到当前活动的窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern System.IntPtr GetForegroundWindow();
        int buttom = 0;
        public class hweather{
            public class Location
            {
                /// <summary>
                /// 
                /// </summary>
                public string id { get; set; }
                /// <summary>
                /// 丽水
                /// </summary>
                public string name { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string country { get; set; }
                /// <summary>
                /// 丽水,丽水,浙江,中国
                /// </summary>
                public string path { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string timezone { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string timezone_offset { get; set; }
            }

            public class Now
            {
                /// <summary>
                /// 阴
                /// </summary>
                public string text { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string code { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string temperature { get; set; }
            }

            public class Results
            {
                /// <summary>
                /// 
                /// </summary>
                public Location location { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public Now now { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string last_update { get; set; }
            }

            public class Root
            {
                /// <summary>
                /// 
                /// </summary>
                public List<Results> results { get; set; }
            }

        }

        public class Location
        {
            /// <summary>
            /// 
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 丽水
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string country { get; set; }
            /// <summary>
            /// 丽水,丽水,浙江,中国
            /// </summary>
            public string path { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string timezone { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string timezone_offset { get; set; }
        }

        public class Daily
        {
            /// <summary>
            /// 
            /// </summary>
            public DateTime date { get; set; }
            /// <summary>
            /// 小雨
            /// </summary>
            public string text_day { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string code_day { get; set; }
            /// <summary>
            /// 多云
            /// </summary>
            public string text_night { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string code_night { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string high { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string low { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string rainfall { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string precip { get; set; }
            /// <summary>
            /// 北
            /// </summary>
            public string wind_direction { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string wind_direction_degree { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string wind_speed { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string wind_scale { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string humidity { get; set; }
        }

        public class Results
        {
            /// <summary>
            /// 
            /// </summary>
            public Location location { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Daily> daily { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string last_update { get; set; }
        }

        public class Root
        {
            /// <summary>
            /// 
            /// </summary>
            public List<Results> results { get; set; }
        }

        static int f1, f2, a = 0, b = 0,c=0;
        static string[] times = new string[10];
        static string[] timeclose = new string[10];
        static string[] timecmd = new string[10];
        static string[] cmd = new string[10];
        string weather;
        string weather_day, weather_night;
        string weather_high, weather_low;
        static void doingcmd(string cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(cmd+ "&exit");
            p.StandardInput.AutoFlush = true;
            p.WaitForExit();
            p.Close();
        }

        public MainWindow()
        {
            InitializeComponent();


        }
        private void Button_Mini(object sender, RoutedEventArgs e)
        {
            buttom++;
            if (buttom == 3) { WindowState = WindowState.Minimized; buttom = 0; }
            GC.Collect();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeLable.Content = DateTime.Now.ToLongTimeString().ToString();
            for (int i = 1; i <= a; i++) if (times[i] == DateTime.Now.ToLongTimeString().ToString()) WindowState = WindowState.Normal;
            for (int i = 1; i <= b; i++) if (timeclose[i] == DateTime.Now.ToLongTimeString().ToString()) WindowState = WindowState.Minimized;
            for (int i = 1; i <= c; i++) if (timecmd[i] == DateTime.Now.ToLongTimeString().ToString()) doingcmd(cmd[i]);

            WindowInteropHelper mianHanel = new WindowInteropHelper(this);
            WindowInteropHelper vedioWin = new WindowInteropHelper(this);
            WindowInteropHelper FrameWin = new WindowInteropHelper(this);
            FrameWin.Owner = IntPtr.Zero;
            mianHanel.Owner = vedioWin.Handle;
            vedioWin.Owner = FrameWin.Handle;
            this.Topmost = true;
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            SetWindowPos(handle, -1, 0, 0, 0, 0, 1 | 2);
        }

            private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                Byte[] pageData = MyWebClient.DownloadData("https://api.seniverse.com/v3/weather/now.json?key=SsqE832RAq3z3EfAE&location=lishui&language=zh-Hans");
                weather = Encoding.UTF8.GetString(pageData);

            }
            catch { };
           hweather.Root rt2 = JsonConvert.DeserializeObject<hweather.Root>(weather);
            string ab = " ";
            for (int i = 0; i < rt2.results.Count; i++)
            {
                
                    if (i == 0)
                    {
                        ab = rt2.results[i].now.code;
                    }
                
            }

            BitmapImage ImageSource2 = new BitmapImage(new Uri("/" + ab + "@2x.png", UriKind.Relative));
            _1h.Source = ImageSource2;
            _1hc.Content = rt2.results[0].now.temperature + "℃";

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] text = new string[4];
            text[0] = "XBB2";
            text[1] = "XINGXING";
            text[2] = "LIUSHU";
            text[3] = "Xcube";
            Random ra = new Random();
            TimeLable.Content = text[ra.Next(0,3)];
            string weather1h=" ";
            
            weathercardwindow.SetBinding(AcrylicPanel.TargetProperty, new Binding() { ElementName = "MainImage" });
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "timeopen.txt"))
            {
                using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "timeopen.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        a++;
                        times[a] = line;
                    }
                }
            }
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "timeclose.txt"))
            {
                using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "timeclose.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        b++;
                        timeclose[b] = line;
                    }
                }
            }
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "timedoing.txt"))
            {
                using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "timedoing.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        c++;
                        timecmd[c] = line;
                        line = sr.ReadLine();
                        cmd[c] = line;
                    }
                }
            }
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - Width;
            this.Top = 0;
            DayLable.Content = DateTime.Now.ToString("yyyy年MM月dd日");
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                Byte[] pageData = MyWebClient.DownloadData("https://api.seniverse.com/v3/weather/daily.json?key=SsqE832RAq3z3EfAE&location=lishui&language=zh-Hans&unit=c&start=0&days=2"); 
                weather = Encoding.UTF8.GetString(pageData);

            }
            catch { };
            Root rt = JsonConvert.DeserializeObject<Root>(weather);
            for (int i = 0; i < rt.results.Count; i++)
            {
                for(int j = 0; j < rt.results[i].daily.Count; j++)
                {
                    if (j == 0)
                    {
                        weather_day = rt.results[i].daily[j].code_day;
                        weather_night = rt.results[i].daily[j].code_night;
                        weather_low = rt.results[i].daily[j].low;
                        weather_high = rt.results[i].daily[j].high;
                    }
                }
            }
            try
            {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                Byte[] pageData = MyWebClient.DownloadData("https://api.seniverse.com/v3/weather/now.json?key=SsqE832RAq3z3EfAE&location=lishui&language=zh-Hans");
                weather1h = Encoding.UTF8.GetString(pageData);

            }
            catch { };
            hweather.Root rt2 = JsonConvert.DeserializeObject<hweather.Root>(weather1h);

            BitmapImage ImageSource = new BitmapImage(new Uri("/" +weather_day + "@2x.png", UriKind.Relative));
            day.Source = ImageSource;
            BitmapImage ImageSource1 = new BitmapImage(new Uri("/" +weather_night + "@2x.png", UriKind.Relative));
            night.Source = ImageSource1;
            high.Content = weather_high + "℃";
            low.Content = weather_low + "℃";
            string ab = " ";
            for (int i = 0; i < rt2.results.Count; i++)
            {

                    if (i == 0)
                    {
                        ab = rt2.results[i].now.code;
                    }
                
            }
            
            BitmapImage ImageSource2 = new BitmapImage(new Uri("/" + ab + "@2x.png", UriKind.Relative));
            _1h.Source = ImageSource2;
           _1hc.Content = rt2.results[0].now.temperature + "℃";
           DispatcherTimer timer = new DispatcherTimer();
           timer.Interval = TimeSpan.FromSeconds(1);
           timer.Tick += timer1_Tick;
           timer.Start();
            IntPtr handle = new WindowInteropHelper(this).Handle;

            SetWindowPos(handle, -1, 0, 0, 0, 0, 1 | 2);
            DispatcherTimer timer2 = new DispatcherTimer();
            timer2.Interval = TimeSpan.FromHours(1);
            timer2.Tick += timer2_Tick;
            timer2.Start();
            DispatcherTimer timer3 = new DispatcherTimer();
            timer3.Interval = TimeSpan.FromMilliseconds(17);
            timer3.Tick += timer3_Tick;
            timer3.Start();
        }
    }
}
                                                                                            