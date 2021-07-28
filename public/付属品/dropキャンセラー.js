	// ページ上に何かがドロップされた際にページ偏移しないようにする。
	window.addEventListener('dragover',function(e){e.preventDefault();},false)
	window.addEventListener('drop',function(e){
		e.preventDefault()
		console.log(e.dataTransfer)
		console.log(e.dataTransfer.files[0])
	}, false)
