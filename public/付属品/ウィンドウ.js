ウィンドウ = (()=>{
	
	const WPF制御 = (項目, 値)=>{
		if(!値 || 値.match(/[^\d]/)){return}
		WPF.send(項目, 値.toString())
	}
	
	return {
		width : {
			set : (v)=> WPF制御('Width', v),
			get : ()=> window.outerWidth
		},
		height : {
			set : (v)=> WPF制御('Height', v),
			get : ()=> window.outerHeight
		},
		top : {
			set : (v)=> WPF制御('Top', v),
			get : async ()=> await WPF.sendPromise('getTop')
		},
		left : {
			set : (v)=> WPF制御('Left', v),
			get : async ()=> await WPF.sendPromise('getLeft')
		}
	}
})()

/*
	inpTop.value = await ウィンドウ.top.get()
	ウィンドウ.top.get().then(v =>{ inpTop.value = v})
*/
