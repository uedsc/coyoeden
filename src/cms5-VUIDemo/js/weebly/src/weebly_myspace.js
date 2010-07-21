var deleteElement = null;

function myspace_mouseOver(el) {

	if(!Element.hasClassName(el.childNodes[0], 'nodrag')) { 
	  el.childNodes[0].style.display = 'block';
	  if (Prototype.Browser.IE) { el.childNodes[1].style.marginTop = '-18px'; }
	}

}

function myspace_mouseOut(el) {

	el.childNodes[0].style.display = 'none';
	if (Prototype.Browser.IE) { el.childNodes[1].style.marginTop = '0px'; }

}

function myspace_hide(el) {

	deleteElement = el;
	Effect.Fade(el.parentNode.parentNode, { afterFinish: function () { deleteElement.parentNode.parentNode.parentNode.parentNode.removeChild(deleteElement.parentNode.parentNode.parentNode); updateList(); deleteElement = null; } });

}
