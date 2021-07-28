	WPF = function(){
		var arrCallBack = []
		return {
			send:(機能の名前, 引数, callBack)=>{
				// 引数は文字列、もしくは文字列型の値が入っているプロパティのみを持つオブジェクト。(WPF側で<string, string>型のdictionaryに入れるので、文字列型以外の値が入っているとエラーになる)
				
				// WPF側で、2段階に分けてデシリアライズしたい(階層構造のobjをデシリアライズする場合の型の指定方法が分からない…)
				// ので、引数プロパティにはシリアライズした文字列を入れる。
				const WPFに渡すobj = {
					機能の名前 : 機能の名前,
					引数       : (typeof 引数)=='string' ? 引数 : JSON.stringify(引数 || '')
				}
				
				const pM = () => window.chrome.webview.postMessage(JSON.stringify(WPFに渡すobj)) // postMessageで送れるのは文字列だけ。
				
				if(!callBack){return pM()}
				
				let i = 0
				while(arrCallBack[i]){ i++ }
				arrCallBack[i] = callBack
				
				WPFに渡すobj.callBack = i.toString()
				pM()
			},
			callBack:(ind, value)=>{
				// JSON.parseするかどうかはcallBack先に任せる。
				const res = arrCallBack[ind](value)
				
				// !resならcallBackを削除する。(res!=falseならcallBackが複数回呼べるという事)…という感じにしたいんだけどcallBackの戻り値がどうしてもundefindになっちゃう。。
				if(!res){ arrCallBack[ind] = 0 }
			},
			sendPromise:function(機能の名前, 引数){
				const this_ = this
				return new Promise((resolve, reject)=>{
					this_.send(機能の名前, 引数, (v)=>resolve(v))
				})
			}
		}
	}()
