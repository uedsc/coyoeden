function watermarkEnter(obj, wm) {
	if (obj.value == wm) {
		obj.value = '';
		obj.style.color = '';
        
	}
	
}
function watermarkLeave(obj, wm) {
	if (obj.value == '') {
		obj.value = wm;
      
	}
}