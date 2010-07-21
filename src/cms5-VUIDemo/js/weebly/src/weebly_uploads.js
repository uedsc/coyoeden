var swfu = {};

function uploadSuccess(file, data, response) {

	if (data && data == "%%TOOLARGE%%") {
	  $('upload'+file.id).innerHTML = /*tl(*/"<span style=\"color: red;\">Image is too large. Please resize and try again.</span>"/*)tl*/;
  	  removeProgressBox(file.id);
	}else if(data && data == "%%LARGESITE%%") {
	  $('upload'+file.id).innerHTML = /*tl(*/"<span style=\"color: red;\">Your site is too large. Please delete a few elements, and try again.</span>"/*)tl*/;
          removeProgressBox(file.id);
	}else if(data && data == "%%LARGESITESOFTLIMIT%%") {
	  $('upload'+file.id).innerHTML = /*tl(*/"<span style=\"color: red;\">Your account has uploaded quite a few files. Please contact support to continue uploading.</span>"/*)tl*/;
          removeProgressBox(file.id);
	} else if (data && data.match(/%%QUEUEID:[0-9]+%%[^%]+%%/)) {
	  var matches = data.match(/%%QUEUEID:([0-9]+)%%([^%]+)%%/);
	  if (file._parameters) {
	  	swfu.uploadOptions[file.id] = file._parameters; // from plain file uploader
	  }
	  swfu.uploadOptions[file.id].qid = matches[1];
	  swfu.uploadOptions[file.id].video_name = matches[2];
	}

}

function uploadFileComplete(file, passedOpts) {

  var opt = passedOpts || swfu.uploadOptions[file.id];
  if (opt.done) { return; }

  $('upload'+file.id).innerHTML = "Upload Complete";

  if (opt.type != "video") {
    removeProgressBox();
  }

  if(opt.type == "image") {

    if ($(""+opt.ucfpid)) {
      $(""+opt.ucfpid).value = '/uploads/'+userIDLocation+'/'+opt.newid+'.'+opt.ext.toLowerCase();
    }

    if ($(opt.ucfid) && $(opt.ucfid).down) {
      var prodImage = $(opt.ucfid).down('.product-image');
    } else {
      var prodImage = null;
    }
    
	  var ext = opt.ext.toLowerCase();
	  if (ext == 'bmp') {
	  	ext = 'jpg'; // bmp's are always converted to jpegs
	  }

    if(prodImage){
    
        prodImage.src = '/uploads/'+userIDLocation+'/'+opt.newid+'.'+ext;
        
    } else {

		// Try to update the image!
		try {
		  var img = $(""+opt.ucfid).getElementsByTagName("IMG")[0];
		  img.src = '/uploads/'+userIDLocation+'/'+opt.newid+'.'+ext;
		  img.style.width = 'auto';
		  img.style.height = 'auto';
		} catch (e) { }

    }

  } else if(opt.type == "header") {
        currentStyleNum = Math.floor(Math.random()*10000000001);
        writeTheme(currentTheme);
	currentHeader = "not.null";
  } else if(opt.type == "file" || opt.type == "audio" || opt.type == "gallery" || opt.type == "doc" || opt.type == "flash") {
	swfu.dontSave = 1;
	updateList(currentPage);
  } else if(opt.type == "video") {
	$('upload'+file.id).innerHTML = /*tl(*/"Waiting for video to start encoding..."/*)tl*/;
	new monitorEncoding(file.id, opt.ucfid, opt.qid, opt.video_name);
  }

  var stats = swfu.getStats();
  if (stats && stats.files_queued > 0) {
    swfu.startUpload();
  }

}

function dialogStart(){
    if (!swfu.currentDialog) { swfu.currentDialog = {}; }
    swfu.currentDialog.ucfid = swfu.current.ucfid;
    swfu.currentDialog.ucfpid = swfu.current.ucfpid;
}

function dialogComplete(numFilesSelected, numFilesQueued) {

  var stats = swfu.getStats();

  if (stats && stats.files_queued > 0) {
    swfu.startUpload();
  }

}

function queueError(file, errcode, msg) { 

  if (errcode == SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT) {
    if (!isPro()) {
      alertProFeatures(/*tl(*/"Please sign-up for a pro account to upload files larger than 5 MB (up to 100MB)"/*)tl*/, "main");
    } else {
      showError(/*tl(*/'This file is too big! You can only upload files smaller than 100MB.'/*)tl*/);
    }
  }

}

function uploadError(file, errcode, msg) {
  if (errcode == SWFUpload.UPLOAD_ERROR.FILE_CANCELLED) {

    $('upload'+file.id).innerHTML = /*tl(*/"Upload cancelled."/*)tl*/;

  } else {

    //$('upload'+file.id).innerHTML = /*tl(*/"Upload Error. Please try again."/*)tl*/+" ("+errcode+")";
    
    // show oops dialog with option to use plain uploader
    showUploadError(errcode, msg);

  }
  fireTrackingEvent("WeeblyError", "Upload Error", errcode + ' - '+msg);

  removeProgressBox(file.id);

}


function uploadStart(file) { 

  swfu.currentUpload = file.id;
  
  if (!file.plainUploader) {
      swfu.uploadOptions[file.id].ext = file.name.replace(/.*\.([^\.]*)/, "$1");
	  var postParams = {'type':swfu.uploadOptions[file.id].type,'width':swfu.uploadOptions[file.id].size,'ucfid':swfu.uploadOptions[file.id].ucfid,'ucfpid':swfu.uploadOptions[file.id].ucfpid, 'newid':swfu.uploadOptions[file.id].newid, 'cookies':document.cookie.match(/WeeblySession=[^;]+(;|$)/)[0]};
	  swfu.setPostParams(postParams);
	  swfu.setUploadURL("/weebly/fileUpload.php");
  }

  var newEl = document.createElement('li');
  newEl.id = 'upload'+file.id;
  newEl.innerHTML =
  	(file.plainUploader ?
  		/*tl(*/"Uploading File. Please Wait..."/*)tl*/ + "<br/><br/>" : // dont display progress indicator for non-flash uploader
  		/*tl(*/"Uploading File"/*)tl*/ + "<br/><div id='uploadContainer"+file.id+"' style='font-size: 10px;'><div style='height: 5px; overflow: hidden; background: #ccc; margin: 5px 0;'><div style='height: 5px; overflow: hidden; background: #777; width: 0px;' id='"+file.id+"progress'></div></div></div>"
  		) +
  	"<a href='#' onclick=\"cancelFile('"+file.id+"'); return false;\">"+/*tl(*/"Cancel"/*)tl*/+"</a>";
  	
  $('notifications').appendChild(newEl);

}

function uploadProgress(file, bytesLoaded, totalBytes) {
	if (file.size == bytesLoaded) {
	  $('uploadContainer'+swfu.currentUpload).innerHTML = "<i style='color: red;'>"+/*tl(*/"Virus scanning..."/*)tl*/+"</i>";
	} else {
       _uploadProgress(bytesLoaded / file.size);
	}
}

function _uploadProgress(frac) {
	var progress = document.getElementById(swfu.currentUpload + "progress");
	progress.style.width = Math.ceil(frac * 180) + "px";
}

function uploadFileQueued(file) {

	if (!swfu.uploadOptions) { swfu.uploadOptions = {}; }
	if (!swfu.uploadOptions[file.id]) { swfu.uploadOptions[file.id] = {}; }

  	swfu.uploadOptions[file.id].ext = file.name.replace(/.*\.([^\.]*)/, "$1");
  	swfu.uploadOptions[file.id].newid = Math.floor(Math.random()*10000001);
        swfu.uploadOptions[file.id].size = swfu.current.size;
        swfu.uploadOptions[file.id].type = swfu.current.type;
        swfu.uploadOptions[file.id].ucfid = swfu.currentDialog.ucfid;
        swfu.uploadOptions[file.id].ucfpid = swfu.currentDialog.ucfpid;
        swfu.uploadOptions[file.id].fileid = file.id;
        swfu.uploadOptions[file.id].done = null;

	Weebly.Elements.unselectElement();
}

function selectDefaultImageUpload(ucfid){
    var el = $(''+ucfid).up('.inside');
    var props = el.down('form')['pfield'].value.evalJSON();
    var ucfpid = '';
    var type = '';
    var width = 250;
    for(property in props[ucfid]){
        if(typeof(props[ucfid][property]) === 'object'){
            if(props[ucfid][property].propertyresult &&  isUploaderImageSrc(props[ucfid][property].propertyresult)){
                ucfpid = props[ucfid][property].ucfpid;
                if(props[ucfid][property].propertyresult.match(/na\.jpg/)){
                    type = 'image';
                    if(el.down('.product-image')){
                        switch(props[ucfid][69650731].propertyresult){
                            case 'grid':
                              width = 164;
                              break;
                            case 'large':
                              width = 225;
                              break;
                            case 'small':
                              width = 125;
                              break;
                            case 'long':
                              width = 175;
                              break;
                        }
                    }
                }
            }
            if(props[ucfid][property].referenceproperty === '<!VIDEOFILE!>'){
                type = 'video';
                width = '';
                ucfpid = props[ucfid][property].ucfpid;
                break;
            }
            if(props[ucfid][property].referenceproperty === '<!IMAGESET!>'){
                ucfpid = props[ucfid][property].ucfpid;
                type = 'gallery';
            }
        }
    }
    selectUpload(type, width, ucfid, ucfpid);
}

function selectUpload(uploadType, size, ucfid, ucfpid) {
	//console.log(ucfpid);
	
	Weebly.Elements.upload = 1;

	var fileDescription = "All files...";
	var fileTypes = "*.*";

	if (uploadType == "audio") {
	  fileDescription = "MP3 files...";
	  fileTypes = "*.mp3";
	} else if (uploadType == "header" || uploadType == "image" || uploadType == "gallery") {
	  fileDescription = "Image files...";
	  fileTypes = "*.gif;*.jpg;*.jpeg;*.png;*.bmp";
	} else if (uploadType == "file") {
	  fileDescription = "All files...";
	  fileTypes = "*.*";
	} else if (uploadType == "doc") {
	  fileDescription = "Document files...";
	  fileTypes = "*.doc;*.docx;*.ppt;*.pptx;*.pps;*.xls;*.xlsx;*.pdf;*.ps;*.odt;*.odp;*.ods;*.odf;*.odg;*.sxw;*.sxi;*.sxc;*.sxd;*.tif;*.tiff;*.txt;*.rtf";
	} else if (uploadType == "video") {
	  fileDescription = "Video files...";
	  fileTypes = "*.*";
	} else if (uploadType == "flash") {
	  fileDescription = "SWF files...";
	  fileTypes = "*.swf";
	}

	if (!swfu.current) { swfu.current = {}; }
	swfu.current.size = size;
	swfu.current.type = uploadType;
	swfu.current.ucfid = ucfid;
	swfu.current.ucfpid = ucfpid;

	if(uploadType == "header" && headerSelected) {
	  unselectHeader();
	}
	
	// for html-base file uploader
	plainUploaderFileTypes(fileTypes, fileDescription);

	try {

	  swfu.setFileTypes(fileTypes, fileDescription);

	  if (uploadType == "gallery") {
	    swfu.setButtonAction(SWFUpload.BUTTON_ACTION.SELECT_FILES);
	  } else {
	    swfu.setButtonAction(SWFUpload.BUTTON_ACTION.SELECT_FILE);
	  }

	} catch (e) { }

}

function cancelFile(id) {
	$('upload'+id).innerHTML = "Upload Canceled";
	if (!cancelPlainUpload(id)) { // will return true of canceled successfully
		if (id == swfu.currentUpload) {
		  swfu.stopUpload();
		}
		swfu.cancelUpload(swfu.uploadOptions[id].fileid);
	}
	removeProgressBox(id);
}

function removeProgressBox(id) {
	id = id ? id : swfu.currentUpload;
	if (swfu.uploadOptions && swfu.uploadOptions[id]) {
  		swfu.uploadOptions[id].done = 1;
  		// might be using the plain image uploader
  	}
	setTimeout("Effect.Fade('upload"+id+"'); ", 2000);
	setTimeout("if($('upload"+id+"')) $('notifications').removeChild($('upload"+id+"'));", 5000);
}

function swfUploadLoaded() {

	try {
	  if (!swfu.containerShown) {
	    showFlashContainer();
	    hideFlashContainer();
	  }
	} catch (e) { }

}

function hideFlashContainer() {

	if ($('flashContainer') && !forceFlashPosition) {
	  $('flashContainer').style.zIndex = "-1";
      try{
          swfu.setButtonDimensions(1,1);
      }
      catch(e){}
	}

}

function showFlashContainer(zIndex) {

	swfu.containerShown = 1;

	if ($('flashContainer') && !forceFlashPosition) {
	  $('flashContainer').style.zIndex = (typeof zIndex === undefined) ? "21" : zIndex; // TODO: ask drew about zindex stuff
      try{
          swfu.setButtonDimensions(600,600);
      }
      catch(e){}
	}

}

var monitorEncoding = Class.create({

	initialize: function(id, ucfid, qid, video_name) {

	  this.id = id;
	  this.qid = qid;
	  this.ucfid = ucfid;
	  this.video_name = video_name;
	  this.timeoutID = window.setInterval(this.check.bind(this), 5000);

	},

	check: function() {

	  new Ajax.Request(ajax, {parameters:'pos=checkencoding&qid='+this.qid+'&cookie='+document.cookie, onSuccess:this.returnCheck.bind(this), bgRequest: true});

	},

	returnCheck: function(t) {

	  if (t.responseText.match("processing:")) {

	    if (!this.encoding) {
	  
	      $('upload'+this.id).innerHTML = "Encoding video... <br/><div style='height: 5px; overflow: hidden; background: #ccc; margin: 5px 0;'><div style='height: 5px; overflow: hidden; background: #777; width: 0px;' id='"+this.qid+"qprogress'></div></div>";

	      // Magic: this is where we guess at how long a video will take to encode
	      // In any case, we try to underpromise here...
	      this.length = (t.responseText.split(":")[1] - 5) * 0.75;

	      this.encodingStarted = new Date().getTime()/1000;
	      this.encodingTimeoutID = window.setInterval(this.updateProgress.bind(this), 200);
	      this.encoding = true;

	    }

	  } else if (t.responseText.match("success")) {
	    $('upload'+this.id).innerHTML = "Video encoding finished!";

	    if ($(this.ucfid)) {

	      try {

		videoFileName = this.video_name.replace(/\.[^\.]+$/, ".flv");
		imageFileName = this.video_name.replace(/\.[^\.]+$/, ".jpg");

		// If Element is currently open, update its definition
		if (Weebly.Elements.currentElement && Weebly.Elements.pfield[this.ucfid]) {
		  Weebly.Elements.getPropertyByReference('<!IMAGEFILE!>').propertyresult = imageFileName;
		  Weebly.Elements.getPropertyByReference('<!VIDEOFILE!>').propertyresult = videoFileName;
		} else {
		  swfu.dontSave = 1;
		  updateList(currentPage);
		}

		/*
 		// Nice thought, but old element pfield doesn't get updated
		var iframe = $(this.ucfid).down('iframe');
		var newSrc = iframe.src;
		newSrc = newSrc.safeReplace(/video=[^&]*&?/, "video="+videoFileName+"&");
		newSrc = newSrc.safeReplace(/image=[^&]*&?/, "image="+imageFileName+"&");
		iframe.src = newSrc;
		*/

	      } catch(e) { }

	    }

	    this.cleanup();

	  } else if (t.responseText.match("failed")) {
	    $('upload'+this.id).innerHTML = "Video encoding failed. The uploaded file may be corrupted or may not be a supported video format.";
	    this.cleanup();
	  }
	},

	updateProgress: function() {

	  var elapsed = new Date().getTime()/1000 - this.encodingStarted;

	  if ($(this.qid+'qprogress') && elapsed < this.length) {
	    $(this.qid+'qprogress').style.width = Math.ceil(elapsed / this.length * 180) + "px";
	  } else {
	    window.clearInterval(this.encodingTimeoutID);
	  }

	},

	cleanup: function() {

	  removeProgressBox(this.id);
	  window.clearInterval(this.timeoutID);

	}

});


/********* plain uploader *********/

var plainUploaderHash = {};

function getPlainUploader(name) {
	var res = plainUploaderHash[name];
	if (!res) {
		var options = {
			queued: function(fileInfo) {
				Weebly.Elements.unselectElement();
				fileInfo._parameters = { // store here, as opposed to using swfu.uploadOptions like in uploadFileQueued()
				  	ext: fileInfo.name.replace(/.*\.([^\.]*)/, "$1"),
				  	newid: fileInfo.id,
					width: swfu.current.size, // this stuff is stored by selectUpload()
					type: swfu.current.type,  // ... in the swfu object :(
					ucfid: swfu.current.ucfid,
					ucfpid: swfu.current.ucfpid
				};
				//if (name == 'error') {
					hideError();
					fireTrackingEvent("WeeblyAltUpload", "Start", "orig swfu error: " + this._swfu_msg + " - " + this._swfu_errcode);
				//}
			},
			start: function(fileInfo) {
				this.parameters = fileInfo._parameters; // will set additional POST params before sending out
				uploadStart(fileInfo); // will bring up progress box
			},
			progress: function(fileInfo, progress) {
				//_uploadProgress(progress); // will update progress box
			},
			complete: function(fileInfo, data) {
				uploadSuccess(fileInfo, data);
				uploadFileComplete(fileInfo, fileInfo._parameters);
				//if (name == 'error') {
					fireTrackingEvent("WeeblyAltUpload", "Complete", "orig swfu error: " + this._swfu_msg + " - " + this._swfu_errcode);
				//}
			}
		};
		plainUploaderHash[name] = res = new Weebly.PlainUploader(options);
	}
	return res;
}

function showUploadError(errcode, msg) {
	showError(
		"<p>" +
		/*tl(*/"There was an error uploading your file"/*)tl*/+" (error "+errcode+"). <br />" +
		/*tl(*/"Would you like to try again using our basic file uploader?"/*)tl*/ +
		"</p>" +
		"<div style='padding-bottom:10px'>" +
			"<span id='upload-fallback-button' style='position:relative;display:inline-block'>" +
				"<button style='font-size:16px'>" +
					/*tl(*/"Try Basic Uploader"/*)tl*/ +
				"</button>" +
			"</span>" +
		"</div>",
		'',  // no xhr, needs to be non-object
		true // dont track oops event in GA
	);
	var uploader = getPlainUploader('error');
	uploader._swfu_errcode = errcode;
	uploader._swfu_msg = msg;
	var buttonWrap = $('upload-fallback-button');
	uploader.show(buttonWrap, buttonWrap);
}


