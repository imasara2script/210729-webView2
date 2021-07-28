using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Web.WebView2.Core;

namespace netHTA
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly CountdownEvent condition = new CountdownEvent(1);

        private webView2関係 wv2;

        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();

            ウィンドウのプロパティを変更("Title", "abccc");
            ウィンドウのプロパティを変更("Top", 300);
            // MessageBox.Show(Class1.test("abc"));
        }

        async void InitializeAsync()
        {
            await web.EnsureCoreWebView2Async(null);

            string[] URL = get引数();
            if (URL.Length == 1) { URL = new string[] { URL[0], "c:/WPF/index.html" }; }
            URL変更(URL[1]);

            wv2 = new webView2関係();
            wv2.初期化(web.CoreWebView2, this);
        }

        public string[] get引数()
        {
            string[] args = Environment.GetCommandLineArgs();
            return args;
        }

        private async void URL変更(string URL)
        {
            web.CoreWebView2.Navigate(URL);

            //非同期実行
            string result = "";
            await Task.Run(() =>
            {
                //読み込み完了まで待機
                if (condition.Wait(1000 * 10))
                {
                    result = "ok";
                    URL変更後の初期化();
                }
                else
                {
                    result = "timeout";
                }
            });
            // MessageBox.Show("condition : " + result);
        }

        private void URL変更後の初期化()
        {
            // 非同期にすると別スレッドになり、別スレッドになるとメインスレッドが所有するコントロールにアクセスできないので
            // メインスレッドのDispatcher(キューを管理するクラス)に実行を依頼する。
            this.Dispatcher.Invoke(async () =>
            {
                var webTitle = await web.ExecuteScriptAsync("document.title");
                Title = webTitle;
            });
        }

        public void ウィンドウのプロパティを変更<Type>(string プロパティ名, Type 値)
        {
            // this.GetType().GetProperty(プロパティ名).SetValue(this, 値, null);
            // netHTA.MainWindow.GetType().GetProperty(プロパティ名).SetValue(netHTA.MainWindow, 値, null);
            this.GetType().GetProperty(プロパティ名).SetValue(this, 値, null);
        }

        public T ウィンドウのプロパティ値を取得 <T>(string プロパティ名)
        {
            var prop = this.GetType().GetProperty(プロパティ名);
            dynamic 値 = prop.GetValue(this);
            return 値;
        }

    }
}