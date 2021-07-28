async get引数(){
	//	WPF.send('getArgs', (v)=>{ inpArgs.value = v })
	//	WPF.sendPromise('getArgs').then((v)=>{ inpArgs.value = v })
	return await WPF.sendPromise('getArgs')
}
