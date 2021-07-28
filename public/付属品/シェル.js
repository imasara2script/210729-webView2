シェル = (()=>{
	return {
		// 実行:async (path, パラメータ="", bln非同期)=>{
		実行:(path, パラメータ="", bln非同期)=>{
			// return await WPF.sendPromise('CMD' + (bln非同期 ? '非' : '') + '同期', {path, パラメータ})
			return WPF.sendPromise('CMD' + (bln非同期 ? '非' : '') + '同期', {path, パラメータ})
		}
	}
})()

/*
	ta.value = await シェル.実行('cmd', '/C dir') // 「/C」が無いとプロセスが閉じないのでフリーズする。
*/
