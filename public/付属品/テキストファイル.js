テキストファイル = (()=>{
	return {
		Read:async (path)=> await WPF.sendPromise('TextRead', path),
		Write:async (path, value)=> await WPF.sendPromise('TextWrite',{path,value})
	}
})()
