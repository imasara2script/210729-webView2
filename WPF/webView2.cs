using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CoreWebView2NavigationCompletedEventArgsの参照に使用
using Microsoft.Web.WebView2.Core;

namespace netHTA
{
    class webView2関係
    {
        private CoreWebView2 cwv2;
        private MainWindow ウィンドウ;

        public void 初期化(CoreWebView2 web, MainWindow win)
        {
            cwv2 = web;
            cwv2.NavigationCompleted += Web_NavigationCompleted;
            cwv2.WebMessageReceived  += MessageReceived;

            ウィンドウ = win;
        }

        private void Web_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            //読み込み結果を判定
            if (e.IsSuccess)
                Console.WriteLine("complete");
            else
                Console.WriteLine(e.WebErrorStatus);
        }

        //JavaScriptからメッセージを受信したときに実行します。
        private void MessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs args)
        {
            String text = args.TryGetWebMessageAsString();

            Func<string, Dictionary<string, string>> getDic = (string str) =>
            {
                return JSON.デシリアライズ(str);
            };

            Dictionary<string,string> dic = getDic(text);

            Dictionary<string, string> dic引数;
            string 文字列;
            Int32 int値;

            /*
            Func<string> JSに値を返す(string 返送先No, string 戻り値)
            {
                string str = "WPF.callBack(" + 返送先No + ", " + 戻り値 + ")";
                cwv2.ExecuteScriptAsync(str);
                return () => { return str; }; // 返す必要はないけど「Func<string>」
            }
            // 下の書き方とどちらが良いか悩む。どちらかと言えば下の方が無駄(return ()=> {})が少ない気がする。（return str;は余分だけど、上に比べればマシ）
            */

            Func<string, string, string> JSに値を返す = (string 返送先No, string 戻り値) => {
                string str = "WPF.callBack(" + 返送先No + ", " + 戻り値 + ")";
                ウィンドウ.Dispatcher.Invoke(()=> {
                    dynamic res = cwv2.ExecuteScriptAsync(str);
                });
                
                return str;
            };

            switch (dic["機能の名前"])
            {
                case "AppActivate":
                    return;
                case "icon":
                    var getBF = new getBitmapFrame();
                    ウィンドウ.ウィンドウのプロパティを変更("Icon", getBF.fromString(dic["引数"]));
                    return;
                case "Close":
                    return;
                case "CMD同期":
                    dic引数 = getDic(dic["引数"]);
                    文字列 = シェル.実行(dic引数["path"], dic引数["パラメータ"]);
                    文字列 = JSON.シリアライズ(文字列);
                    JSに値を返す(dic["callBack"], 文字列);
                    return;
                case "CMD非同期":
                    dic引数 = getDic(dic["引数"]);
                    // JSに値を返す_(dic["callBack"], "1"); // ここからだとブラウザ側のWPF.callBackは反応するが
                    // return;
                    シェル.非同期実行(dic引数["path"], dic引数["パラメータ"], (string str)=> {
                        str = JSON.シリアライズ(str);
                        JSに値を返す(dic["callBack"], str); // ここからだとWPF.callBackが反応しない。原因不明。
                    //    JSに値を返す(dic["callBack"], "1");
                        return str;
                    });
                    return;
                case "TextRead":
                    文字列 = TextRead.getAll(dic["引数"]);
                    文字列 = JSON.シリアライズ(文字列);
                    JSに値を返す(dic["callBack"], 文字列);
                    return;
                case "TextWrite":
                    dic引数 = getDic(dic["引数"]);
                    TextWrite.上書き(dic引数["path"], dic引数["value"]);
                    JSに値を返す(dic["callBack"], "null");
                    return;
                default:
                    if (dic["機能の名前"].IndexOf("get") == 0)
                    {
                        string 項目 = dic["機能の名前"].Substring(3);

                        switch (項目)
                        {
                            case "Args":
                                文字列 = "'" + string.Join("','", ウィンドウ.get引数()).Replace("\\", "/") + "'";
                                break;
                            case "Left":
                            case "Top":
                                文字列 = ウィンドウ.ウィンドウのプロパティ値を取得<Double>(項目).ToString();
                                break;
                            default:
                                文字列 = ウィンドウ.ウィンドウのプロパティ値を取得<String>(項目);
                                break;
                        }
                        JSに値を返す(dic["callBack"], 文字列);
                        return;
                    }

                    if (int.TryParse(dic["引数"], out int値))
                    {
                        ウィンドウ.ウィンドウのプロパティを変更(dic["機能の名前"], int値);
                    }
                    else
                    {
                        ウィンドウ.ウィンドウのプロパティを変更(dic["機能の名前"], dic["引数"]);
                    }
                    return;
            }
        }
    }
}
