(function(window) {


var iframe,
	uploadCancels = {}, // cancel functions that are hashed by the uploadId
	uploadingForm, // form actively transmitting data
	formQueue = [],
	fileTypes,
	fileTypeRegexp;


Weebly.PlainUploader = function(options) {


	options = options || {};
	var t = this,
		availableForm; // unused form that is available to user
		
	
	function getAvailableForm() {
	
	
		if (availableForm) {
			if (!availableForm.childNodes.length) {
				if (availableForm.parentNode) {
					availableForm.remove(); // would sometimes get corrupted b/c of effects + hiding, build a new one
				}
			}else{
				return availableForm;
			}
		}
		
	
	
		/* build DOM & handlers
		---------------------------------------------------------------*/

		var uploadId = Math.floor(Math.random()*10000001),
			formId = 'plainUpload' + uploadId,
			iframeCallback = 'plainUploadCallback' + uploadId,
			progressKey = userID + '_' + uploadId;
			
		// IE needs the form to be created in DOM (for multipart)
		$(document.body).insert(
			"<form id='" + formId + "' target='plainUploadTarget' method='post' enctype='multipart/form-data' " +
			  "style='position:absolute;width:0;height:0;overflow:hidden;margin:0;padding:0'>" +
				"<input type='hidden' name='APC_UPLOAD_PROGRESS' value='" + progressKey + "' />" +
				"<input type='hidden' name='cookies' />" +
				"<input type='file' name='Filedata' " +
				  "style='position:absolute;top:-10px;right:-10px;font-size:150px;height:200px;margin:0;padding:0;" +
				  "cursor:pointer;opacity:0;filter:alpha(opacity=0)' />" +
			"</form>"
		);
			
		var form = $(formId),
			cookieInput = $(form.childNodes[1]),
			fileInput = $(form.childNodes[2]),
			fileInfo = {};
			
		fileInput.observe('change', function() {
			var v = fileInput.value;
			if (v && validateFilename(v)) {
				start();
			}
		});
				
			
			
		/* starting
		-------------------------------------------------------------*/
		
		function start() {
			availableForm = null;
			fileInfo.name = fileInput.value;
			fileInfo.id = uploadId;
			fileInfo.plainUploader = t;
			form.style.left = '-99999px'; // hide it
			$(document.body).insert(form); // put it somewhere safe
			if (t.beforeQueued) {
				t.beforeQueued(fileInfo);
			}
			var busy = uploadingForm || formQueue.length;
			if (busy) {
				form._submit = submit;
				formQueue.unshift(form);
			}
			if (options.queued) {
				options.queued.call(t, fileInfo);
			}
			if (!busy) {
				submit();
			}
		}
		
		function submit() {
			uploadingForm = form;
			if (options.start) {
				options.start.call(t, fileInfo);
			}
			form.action = 'fileUploadPlain.php?callback=' + iframeCallback + (t.parameters ? '&' + Object.toQueryString(t.parameters) : '');
			cookieInput.value = document.cookie.match(/WeeblySession=[^;]+(;|$)/)[0];
			form.submit();
			startProgress();
		}
		
			
			
		/* file progress
		---------------------------------------------------------------*/
			
		var intervalId;
	
		function startProgress() {
			//if (options.progress) {
			//	intervalId = setInterval(updateProgress, 2000); // poll every 2 seconds
			//}
		}
	
		/*
		function updateProgress() {
			new Ajax.Request('fileUploadProgress.php', {
				method: 'get',
				parameters: {
					progress_key: progressKey,
					_: +new Date() // prevent caching
				},
				onSuccess: function(transport) {
					var res = transport.responseText.strip();
					if (res.match(/^[\d\.]+$/)) {
						res = parseFloat(res);
						options.progress.call(t, fileInfo, res);
						if (res >= 1) {
							clearProgress();
						}
					}else{
						//console.log('weird progress value: ' + res);
						//options.progress.call(t, fileInfo, .5);
						//clearProgress();
					}
				},
				bgRequest: true // prevent "Loading..." from showing up
			});
		}
		*/
	
		function clearProgress() {
			//clearInterval(intervalId);
		}
		
		
		
		/* finishing & canceling
		---------------------------------------------------------------*/
		
		window[iframeCallback] = function(data) { // target iframe will call this
			if (options.complete) {
				options.complete.call(t, fileInfo, data);
			}
			cleanup();
		};
		
		uploadCancels[uploadId] = function() {
			iframe.remove(); // remove and rebuild iframe, cancels request
			iframe = null;
			buildIframe();
			cleanup();
		};
		
		function cleanup() {
			uploadingForm = null;
			clearProgress();
			if (form.parentNode) { // IE might have ripped it out somehow (menubar?)
				form.remove(); // will never be used again
			}
			form._submit = null;
			window[iframeCallback] = null;
			uploadCancels[uploadId] = null;
			if (formQueue.length) {
				formQueue.pop()._submit();
			}
		}
		
		
		//
		availableForm = form;
		return form;
	}
	
	
	
	/* positioning
	-------------------------------------------------------*/
	
	t.show = function(coveredElement, parent, zIndex, icontentHack, hideOnMouseout) {
		coveredElement = $(coveredElement);
		var form = getAvailableForm();
		if (!parent) {
			parent = document.body;
		}
		parent = $(parent);
		if (form.parentNode != parent) {
			parent.insert(form);
		}
		if (typeof zIndex != 'undefined') {
			form.style.zIndex = zIndex;
		}else{
			form.style.zIndex = '';
		}
		if (hideOnMouseout) {
			form.observe('mouseout', t.hide);
		}else{
			form.stopObserving('mouseout', t.hide);
		}
		var offsetParent = form.getOffsetParent();
		var positionTop = 0;
		var positionLeft = 0;
		if (offsetParent != coveredElement) {
			var offset = coveredElement.cumulativeOffset();
			var offsetParentOffset = offsetParent.cumulativeOffset();
			positionTop = offset.top - offsetParentOffset.top;
			positionLeft = offset.left - offsetParentOffset.left;
			if (icontentHack) {
				positionTop -= $('scroll_container').scrollTop;
				positionLeft -= $('scroll_container').scrollLeft;
				// cumulativeScrollOffset was malfunctioning :(
			}
		}
		form.setStyle({
			top: positionTop + 'px',
			left: positionLeft + 'px',
			width: coveredElement.getWidth() + 'px',
			height: coveredElement.getHeight() + 2 + 'px' // 2 is a hack to fully cover menubar
		});
	};
	
	t.hide = function() {
		if (availableForm) {
			availableForm.style.left = '-99999px';
		}
	};
	
	
	//
	buildIframe();
	
}



/* file extension validation (global for all uploaders)
-------------------------------------------------------------*/

window.plainUploaderFileTypes = function(typeStr, desc) {
	//fileDescription = desc.toLowerCase().replace('s...', '');
	typeStr = typeStr.toUpperCase().replace(/[\*\.]/g, '');
	if (typeStr) {
		fileTypes = typeStr.split(';');
		fileTypeRegexp = new RegExp('\\.(' + fileTypes.join('|') + ')$', 'i');
	}else{
		fileTypes = fileTypeRegexp = null;
	}
};

function validateFilename(s) {
	if (fileTypeRegexp && !fileTypeRegexp.test(s)) {
		if (fileTypes.length == 1) {
			alert("Please choose a \"" + fileTypes[0] + "\" file.");
		}else{
			alert("Please choose a file with one of the following extensions: \n" + fileTypes.join(', '));
		}
		return false;
	}
	return true;
}



/* iframe
-------------------------------------------------------------*/

function buildIframe() {
	if (!iframe) {
		iframe = new Element('iframe', {
			frameBorder: 0,
			style: 'position:absolute;width:0;height:0;border:0',
			name: 'plainUploadTarget'
		});
		$(document.body).insert(iframe);
	}
}



/* cancel a particular upload
-------------------------------------------------------------*/

window.cancelPlainUpload = function(uploadId) {
	var f = uploadCancels[uploadId];
	if (f) {
		f();
		return true;
	}
	return false;
}


//window.onbeforeunload = function() { return "really?" };


})(window);
