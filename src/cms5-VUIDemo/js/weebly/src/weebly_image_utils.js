var imageResizeId = new Object();


    // ImageResize allows users to resize images inline
    // -------
    Weebly.ImageResize = {
      Version        : '0.1',
      Author         : 'David Rusenko',
      Company        : 'Weebly, Inc.',
      callbackFunc : '',
      resizer: null,

      init: function(el) {

	if (this.resizer || this.el) this.destroy();

	this.el = $(el);

	// Can't resize images that aren't local! Return if src doesn't start with http://*.weebly.com/uploads/ or /uploads/
	if (!this.el.src.match(/^http:\/\/[^\.]+\.weebly\.com\/uploads\//) && !this.el.src.match(/^\/uploads\//)) { this.el = null; return; }

	this.options = Object.extend({
	  minWidth: 30,
	  minHeight: 30,
	  maxWidth: 1000,
	  maxHeight: 2000,
	  appendElement: 'scroll_container',
	  callback: function() {},
	  ucfid: null
	  //callback: function(width, height, id) { alert(width+" "+height+" "+id); }
	}, arguments[1] || {});

	var resizer = document.createElement('DIV');
	resizer.style.border = '1px solid #000000';
	resizer.style.background = '#FF5544';
	resizer.style.position = 'absolute';
	resizer.style.display = 'none';
	resizer.style.fontSize = "3px";
	resizer.style.width = "7px";
	resizer.style.height = "7px";
	resizer.style.cursor = 'nw-resize';
	resizer.style.zIndex = '1000';
	resizer.id = "imageresizer";
	document.getElementById(this.options.appendElement).appendChild(resizer);
	this.resizer = resizer;

	this.calcHeight();

	this.eventMouseDown = this.initResize.bindAsEventListener(this);
	Event.observe(this.resizer, "mousedown", this.eventMouseDown);

	Weebly.ImageResizeArray.init(this);
	this.on();

      },

      destroy: function() {

	if (this.el && this.resizer) {
	  Event.stopObserving(this.resizer, "mousedown", this.eventMouseDown);
	  Weebly.ImageResizeArray.kill(this);
	  this.off();
	  this.resizer.parentNode.removeChild(this.resizer);
	  this.resizer = null;
	  this.el = null;
	}

      },

      calcHeight: function() {

        this.origWidth = this.el.width;
        this.origHeight = this.el.height;

      },

      on: function() {

	this.el = Weebly.Elements.getCurrentImageElement();

	this.calcHeight();
	this.positionResizer();

	// from old standalone images where dotted border for resizing
	//this.options.oldBorder = this.el.style.border;
	//this.el.style.border  = this.options.border;
	
	this.resizer.style.display = 'block';

      },

      off: function() {

	this.resizer.style.display = 'none';
	//this.el.style.border = this.options.oldBorder;

      },

      positionResizer: function() {
	if (!this.resizer || !this.el) return;

	// Need to cause browsers to re-calculate image dimensions
	// HERE

        var cumulPosition  = Position.cumulativeOffset(this.el);
        var offsetPosition = Position.realOffset(this.el);
        var sizePosition   = [this.el.width, this.el.height];
        
        if (!Prototype.Browser.IE) {
        	sizePosition[0] += (parseInt(this.el.style.borderLeftWidth) || 0) * 2 + (parseInt(this.el.style.paddingLeft) || 0) * 2;
        	sizePosition[1] += (parseInt(this.el.style.borderTopWidth) || 0) * 2 + (parseInt(this.el.style.paddingTop) || 0) * 2;
        }

		var appendElementMargin = Element.getStyle(this.options.appendElement, 'margin-top').replace(/px/, '');

        this.resizer.style.left = (cumulPosition[0] + sizePosition[0] - 4) + "px";
        this.resizer.style.top  = (cumulPosition[1] - appendElementMargin + sizePosition[1] - 4) + "px";

      },

      initResize: function(event) {
	if(Event.isLeftClick(event)) {
	  // abort on form elements, fixes a Firefox issue
	  var src = Event.element(event);
	  if(src.tagName && (src.tagName=='INPUT' || src.tagName=='SELECT' || src.tagName=='OPTION' || src.tagName=='BUTTON' || src.tagName=='TEXTAREA')) return;

	  var pointer = [Event.pointerX(event), Event.pointerY(event)];
	  var pos     = Position.cumulativeOffset(this.el);
	  this.offset = [0,1].map( function(i) { return (pointer[i] - pos[i]); });
	  this.pointer = pointer;

	  this.origWidth = this.el.width;
	  this.origHeight = this.el.height;

	  Weebly.ImageResizeArray.setCurrent(this);
	  Event.stop(event);

	}
      },

      startResize: function(event) {
	this.resizing = true;

      },

      updateResize: function(event, pointer) {
	if(!this.resizing) this.startResize(event);

	Position.prepare();
	this.draw(pointer);
	Event.stop(event);
      },

      endResize: function(event) {
	if(!this.resizing) return;

	this.resizing = false;
	Weebly.ImageResizeArray.removeCurrent();
	
	this.options.callback(this.el.width, this.el.height, this.options.ucfid, this.el);

    if(event){Event.stop(event);}
      },

      draw: function(p) {

	var newWidth = this.origWidth - (this.pointer[0] - p[0]);
	var newHeight = (newWidth/this.origWidth) * this.origHeight;
	var maxWidth = Math.min($('secondlist').getWidth()-40, this.options.maxWidth);
	var maxHeight = this.options.maxHeight;
	var minWidth = this.options.minWidth;
	var minHeight = this.options.minHeight;
	
	// max size
	if (newWidth/newHeight > maxWidth/maxHeight) {
		// wider image, limit width
		if (newWidth > maxWidth) {
			newHeight = Math.round(maxWidth/newWidth * newHeight);
			newWidth = maxWidth;
		}
	}else{
		// taller image, limit height
		if (newHeight > maxHeight) {
			newWidth = Math.round(maxHeight/newHeight * newWidth);
			newHeight = maxHeight;
		}
	}
	
	// min size
	if (newWidth/newHeight > minWidth/minHeight) {
		// wider image, expand width
		if (newWidth < minWidth) {
			newHeight = Math.round(minWidth/newWidth * newHeight);
			newWidth = minWidth;
		}
	}else{
		// taller image, expand height
		if (newHeight < minHeight) {
			newWidth = Math.round(minHeight/newHeight * newWidth);
			newHeight = minHeight;
		}
	}

	this.el.width = newWidth;
	this.el.height = newHeight;

	// b/c we rely on attributes, clear css width/height
	this.el.style.width = '';
	this.el.style.height = '';

	this.positionResizer();

      }

    };

    Weebly.ImageResizeArray = {

      images: [],

      init: function(el) {

	if(this.images.length == 0) {

	  this.eventMouseUp = this.stopResize.bindAsEventListener(this);
	  this.eventMouseMove = this.updateResize.bindAsEventListener(this);

	  Event.observe(document, "mouseup", this.eventMouseUp);
	  Event.observe(document.body, "mouseleave", this.eventMouseUp); // simulate a mouseup when mouse leaves browser window (for IE)
	  Event.observe(document, "mousemove", this.eventMouseMove);

	}
	this.images.push(el);

      },

      kill: function(el) {

	this.images = this.images.reject(function(d) { return d==el });

	if(this.images.length == 0) {
	  Event.stopObserving(document, "mouseup", this.eventMouseUp);
	  Event.stopObserving(document.body, "mouseleave", this.eventMouseUp);
	  Event.stopObserving(document, "mousemove", this.eventMouseMove);
	}

      },

      setCurrent: function(el) {
	this.currentImage = el;
      },

      removeCurrent: function() {
	this.currentImage = null;
      },

      updateResize: function(event) {
	if (!this.currentImage) return;

	var p = [Event.pointerX(event), Event.pointerY(event)];

	// Fix for Mozilla-based browsers
	if (this._lastP && (this._lastP.inspect() == p.inspect())) return;
	this._lastP = p;

	this.currentImage.updateResize(event, p);

      },

      stopResize: function(event) {
	if (!this.currentImage) return;

	this._lastP = null;
	this.currentImage.endResize(event);
	this.currentImage = null;

      }


    };

    //------------
    /// End of ImageResize module
    ////

    function onResize(width, height, ucfid, el) {
      new Ajax.Request(ajax, {parameters:'pos=resizeimage&ucfid='+ucfid+'&width='+width+"&height="+height+'&cookie='+document.cookie, onSuccess:function(t) { handlerFuncOnResize(t, el, width, height) }, onFailure:errFunc, asynchronous:false});
    }

    function handlerFuncOnResize(t, el, width, height) {

	t.responseText = t.responseText.replace(/\r\n/, '').replace(/\n/, '');
	t.responseText = t.responseText.replace(/\?[0-9x]*$/, '');

	var src = t.responseText + "?" + width; // + "x" + height;
	el.width = width;
	el.removeAttribute('height');
	el.src = src;

	var property = Weebly.Elements.getPropertyByReference('<!SRC!>');
	if (property.cfpid) {

	  property.propertyresult = src;
	  $(''+property.ucfpid).value = src;
	  //Weebly.Elements.saveProperties();

	}

	var identifier = t.responseText.replace(/.?uploads\/[0-9]+\//, "").replace(/\r\n/, '').replace(/\n/, '');
	imageResizeId[identifier] = newId;
    }

    // Weebly ImageGallery
    // -------
    Weebly.ImageGallery = {
      Version        : '0.1',
      Author         : 'David Rusenko',
      Company        : 'Weebly, Inc.',
      currentElement : null,
    
      init: function() {

	if (!this.currentElement) { return; }
	
	Sortable.create(this.currentElement+"-gallery", {
		constraint: false,
		onChange: Weebly.ImageGallery.onChange
	});
	
	$$("#"+this.currentElement+"-gallery .imageControls").each(function(el) { el.style.display = "block"; el.parentNode.style.cursor = "move"; });
	Behaviour.apply();
	
	// hack over a hack ~ashaw
	// before this workaround, dontSave was set to true the first time after uploading an image,
	//  voiding the first saveProperty to the imagegallery after the upload
	swfu.dontSave = 0;

      },

      destroy: function() {

	if (!this.currentElement) { return; }
    if($(this.currentElement+"-gallery")){Sortable.destroy(this.currentElement+"-gallery");}
	$$("#"+this.currentElement+"-gallery .imageControls").each(function(el) { el.style.display = "none"; el.parentNode.style.cursor = "auto"; });
	Behaviour.apply();
	this.currentElement = null;
	
		Weebly.ImageGallery.clearCurrentCaptionForm();

      },

      setCurrent: function(elementId) {

	Weebly.ImageGallery.currentElement = elementId;

      },

      onChange: function() {

			this.currentElement = Weebly.ImageGallery.currentElement;
			if (!this.currentElement || !Weebly.Elements.currentElement || !Weebly.Elements.getPropertyByReference('<!IMAGESET!>').cfpid) { return; }

			var thumbnailData = [];
			var thumbnailCnt = 0;
			$$("#"+this.currentElement+"-gallery img.galleryImage").each(function(thumbnailImg) {
				var url = thumbnailImg.src.replace(/^.*\/uploads\//, ""),
					width = thumbnailImg.readAttribute('_width'),
					height = thumbnailImg.readAttribute('_height'),
					caption = thumbnailImg.readAttribute('_caption'),
					link = thumbnailImg.readAttribute('_link'),
					o = { url: url };
				if (width) o.width = width;
				if (height) o.height = height;
				if (caption) o.caption = filterCaptionData(caption);
				if (link) o.link = link;
				thumbnailData.push(o);
				thumbnailCnt++;
			});

			Weebly.Elements.getPropertyByReference('<!IMAGESET!>').propertyresult = thumbnailData.toJSON();
			Weebly.Elements.saveProperties();

			if (thumbnailCnt == 0) {
				$(''+this.currentElement).innerHTML = Weebly.ImageGallery.update();
			}
			
			Weebly.ImageGallery.clearCurrentCaptionForm(true);

      },

      update: function(elementId, columns, spacing, border, crop, imageSet) {
		
			if (typeof imageSet == 'undefined') {
				var prop = Weebly.Elements.getPropertyByReference('<!IMAGESET!>');
				if (prop) imageSet = prop.propertyresult;
			}
			
			if (imageSet) {
				try {
					imageSet = imageSet.evalJSON();
				} catch (e) {
					imageSet = [];
				}
			}

			if (typeof columns == 'undefined') {
				var prop = Weebly.Elements.getPropertyByReference('<!COLUMNS!>');
				if (prop) {
					var columnInput = $(prop.ucfpid);
					columns = columnInput ? columnInput.value : '';
				}
			}
		
			if (typeof spacing == 'undefined') {
				var prop = Weebly.Elements.getPropertyByReference('<!SPACING!>');
				if (prop) {
					var spacingInput = $(prop.ucfpid);
					spacing = spacingInput ? spacingInput.value : '';
				}
			}
			
			if (typeof border == 'undefined') {
				var prop = Weebly.Elements.getPropertyByReference('<!BORDER!>');
				if (prop) {
					var borderInput = $(prop.ucfpid);
					border = borderInput ? parseInt(borderInput.value) : 0;
				}
			}else{
				border = parseInt(border);
			}
			
			var borderCss = border ? ('border-width:1px;' + (border > 1 ? 'padding:' + (border - 1) + 'px;' : '')) : '';
			
			if (typeof crop == 'undefined') {
				var prop = Weebly.Elements.getPropertyByReference('<!CROPPING!>');
				if (prop) {
					var croppingInput = $(prop.ucfpid);
					crop = croppingInput ? parseInt(croppingInput.value) : 0;
				}
			}else{
				crop = parseInt(crop);
			}
			
			if (!elementId) {
				elementId = Weebly.Elements.ucfid;
			}

			if (!imageSet || imageSet == "" || !imageSet.length) {
				imageSet = [{ url:'/weebly/images/upload_images_01.jpg', width:250, height:168 }];
			}

			this.currentElement = elementId;

			var toWrite = '<ul id="'+elementId+'-gallery" class="imageGallery" style="line-height: 0px; margin: 0; padding: 0; list-style: none;">';
			var width = (100 / columns - 0.05).toFixed(2);
			
			var frameRatio;
			if (crop) {
				frameRatio = 1;
			}else{
				frameRatio = 333/250;
			}
			
			var framePercentHeight = (1/frameRatio * 100).toFixed(2);
			var imageUrl, imageWidth, imageHeight, imageRatio, imageLink, imageCaption;

			for(var x = 0; x < imageSet.length; x++) {

				if (!imageSet[x]) continue;
				
				if (typeof imageSet[x] == 'string') {
					imageUrl = imageSet[x];
					imageWidth = null;
					imageHeight = null;
					imageLink = null;
					imageCaption = '';				
				}else{
					imageUrl = imageSet[x].url;
					imageWidth = parseInt(imageSet[x].width);
					imageHeight = parseInt(imageSet[x].height);
					imageLink = imageSet[x].link;
					imageCaption = filterCaptionData(imageSet[x].caption || '');
				}
				
				if (imageWidth && imageHeight) {
					imageRatio = imageWidth / imageHeight;
				}else{
					imageRatio = 333/250;
				}
				
				if (!imageUrl.match("/weebly/images/")) {
					imageUrl = '/uploads/' + imageUrl;
				}
				
				var imagePercentWidth, imagePercentTop, imagePercentLeft;
				if (crop) {
					if (imageRatio > frameRatio) {
						// wide
						imagePercentWidth = (imageRatio * 100).toFixed(2);
						imagePercentTop = 0;
						imagePercentLeft = -((imageRatio - 1) / 2 * 100).toFixed(2); // bad: only works with squares
					}else{
						// tall
						imagePercentWidth = 100;
						imagePercentTop = -((1/imageRatio - 1) / 2 * 100).toFixed(2); // bad: only works with squares
						imagePercentLeft = 0;
					}
				}else{
					if (imageRatio > frameRatio) {
						// wide
						imagePercentWidth = 100;
						imagePercentTop = ((1 - frameRatio/imageRatio) / 2 * 100).toFixed(2);
						imagePercentLeft = 0;
					}else{
						// tall
						imagePercentWidth = (imageRatio/frameRatio * 100).toFixed(2);
						imagePercentTop = 0;
						imagePercentLeft = ((1 - imageRatio/frameRatio) / 2 * 100).toFixed(2);
					}
				}

				toWrite +=
					"<li id='" + elementId + "-imageContainer" + x + "' style='float:left;width:" + width +
						"%;margin:0;list-style:none;position:relative;z-index:5'>";
					
				toWrite +=
					"<div id='" + elementId + "-insideImageContainer" + x + "' style='position:relative;margin:" + spacing + ";" +
						((crop || !border) ? '' : "padding:0 " + (border*2) + "px " + (border*2) + "px 0;") + "'>";
				
				if (crop && border)
					toWrite +=
						"<div class='galleryImageBorder' style='" + borderCss + "'>";
				
				toWrite +=
					"<div style='position:relative;width:100%;padding:0 0 " + framePercentHeight + "%;" +
						(crop ? "overflow:hidden;" : '') + "'>";
					
				toWrite +=
					"<img src='" + imageUrl + "' class='galleryImage" + ((crop || !border) ? '' : ' galleryImageBorder') + "' " +
						(imageLink ? "_link='" + imageLink + "' " : '') +
						(imageCaption ? "_caption='" + imageCaption + "' " : '') + 
						(imageWidth ? "_width='" + imageWidth + "' _height='" + imageHeight + "' " : '') +
						"style='position:absolute;" + ((crop || !border) ? 'border:0;' : borderCss) +
							"width:" + imagePercentWidth + "%;top:" + imagePercentTop + "%;left:" + imagePercentLeft + "%' />";
					
				toWrite += "</div>";
					
				if (imageUrl != '/weebly/images/upload_images_01.jpg') {
				
					toWrite += "<div class='imageControls' style='display:none;top:" +
						(crop ? "0;left:0;width:100" : imagePercentTop + "%;left:" + imagePercentLeft + "%;width:" + imagePercentWidth) +
						"%'><div class='imageControls-inner1'><div class='imageControls-inner2'>";
						
					toWrite += "<div class='imageControls-icon imageControls-link" + (imageLink ? ' glowing' : '') + "'>";
					toWrite += "<a href='#' onclick='Weebly.ImageGallery.chooseThumbnailLink(this);return false;'></a>";
					toWrite += "</div>";
					
					toWrite += "<div class='imageControls-icon imageControls-caption" + (imageCaption ? ' glowing' : '') + "'>";
					toWrite += "<a href='#' onclick='Weebly.ImageGallery.chooseThumbnailCaption(this);return false;'></a>";
					toWrite += "</div>";
				
					toWrite += "<div class='imageControls-icon imageControls-delete'>";
					toWrite += "<a href='#' onclick='Weebly.ImageGallery.removeNode(this);return false;'></a>";
					toWrite += "</div>";
				
					toWrite += "</div></div></div>";
				}
				
				if (crop && border) toWrite += "</div>";
				
				toWrite += "</div>";
				toWrite += "</li>";
				
			}

			toWrite += "<span style='display: block; clear: both; height: 0px; overflow: hidden;'></span>";
			toWrite += "</ul>";

			return toWrite;

      },

      removeNode: function(element) {
		$(element).up('li').remove();
		Weebly.ImageGallery.onChange();
      },
      
      chooseThumbnailLink: function(element) {
        Weebly.ImageGallery.currentLinkButton = $(element).up();
      	Weebly.ImageGallery.currentLinkThumbnail = $(element).up('li').down('img');
      	Weebly.Linker.show('Weebly.ImageGallery.setLink', {'top': 185, 'left': 250}, ['linkerWebsite', 'linkerWeebly', 'linkerEmail', 'linkerFile'], 'linkerWeebly', currentBox, true);
      },
      
      setLink: function(link) {
      	Weebly.ImageGallery.currentLinkThumbnail.writeAttribute('_link', link);
      	Weebly.ImageGallery.onChange();
      	if (link) {
      		Weebly.ImageGallery.currentLinkButton.addClassName('glowing');
      	}else{
      		Weebly.ImageGallery.currentLinkButton.removeClassName('glowing');
      	}
      },
      
      chooseThumbnailCaption: function(element) {
      	Weebly.ImageGallery.clearCurrentCaptionForm();
      	
      	var button = Weebly.ImageGallery.currentControlButton = $(element).up(),
      		buttonPosition = button.positionedOffset(),
      		controlsInner = button.up().up(),
      		imageWrap = Weebly.ImageGallery.currentCaptionImageWrap = controlsInner.up('li'),
      		image = imageWrap.down('img'),
      		captionForm,
      		captionInput;
      		
      	imageWrap.style.zIndex = 6;
      	button.addClassName('highlighted');
      	
      	var origCaptionData = image.readAttribute('_caption');
      	controlsInner.insert(captionForm = Weebly.ImageGallery.currentCaptionForm =
		  	new Element('form', { 'class':'galleryCaptionForm', style:'left:'+(buttonPosition.left+1)+'px' })
		  		.observe('submit', function(ev) {
		  			ev.stop();
		  		})
		  		.observe('click', function(ev) {
		  			ev.stop();
		  		})
		  		.insert(captionInput = new Element('textarea', {'class':origCaptionData?'':'empty'})
		  			.observe('focus', function() {
		  				if (captionInput.hasClassName('empty')) {
		  					captionInput.removeClassName('empty');
		  					captionInput.value = '';
		  				}
		  			})
		  			.observe('blur', function() {
		  				if (!captionInput.value.strip()) {
		  					captionInput.addClassName('empty');
		  					captionInput.value = 'enter a caption';
		  				}
		  			}))
		  		.insert(new Element('div', { 'class':'galleryCaptionForm-foot' })
		  			.insert(new Element('span')
		  				.update('Caption will appear under the full-sized image.'))
			  		.insert(new Element('input', { type:'submit', value:'Save' })
			  			.observe('click', function() {
			  				var captionData;
			  				if (captionInput.hasClassName('empty')) {
			  					captionData = '';
			  				}else{
				  				captionData = filterCaptionData(captionInput.value);
				  			}
			  				image.writeAttribute('_caption', captionData);
			  				Weebly.ImageGallery.clearCurrentCaptionForm();
			  				Weebly.ImageGallery.onChange();
			  				if (captionData) {
			  					button.addClassName('glowing');
			  				}else{
			  					button.removeClassName('glowing');
			  				}
			  			}))
			  		.insert(new Element('input', { type:'button', value:'Cancel' })
			  			.observe('click', function(ev) {
			  				Weebly.ImageGallery.clearCurrentCaptionForm();
			  			}))));
		  captionInput.value = origCaptionData ? unfilterCaptionData(origCaptionData) : 'enter a caption';
      },
      
      clearCurrentCaptionForm: function(dontChangeZ) {
      	if (Weebly.ImageGallery.currentCaptionForm) {
      		Weebly.ImageGallery.currentCaptionForm.remove();
      		Weebly.ImageGallery.currentCaptionForm = null;
      	}
      	if (Weebly.ImageGallery.currentControlButton) {
      		Weebly.ImageGallery.currentControlButton.removeClassName('highlighted');
      		Weebly.ImageGallery.currentControlButton = null;
      	}
      	if (!dontChangeZ && Weebly.ImageGallery.currentCaptionImageWrap) {
      		Weebly.ImageGallery.currentCaptionImageWrap.style.zIndex = 5;
      		Weebly.ImageGallery.currentCaptionImageWrap = null;
      	}
      }

    };
    
function filterCaptionData(s) {
	return s.strip()
		.replace(/\s+/g, ' ')
		.replace(/'/g, '&#039;')
		.replace(/"/g, '&quot;')
		.replace(/!>/g, '!')
}

function unfilterCaptionData(s) {
	return s.replace(/&#039;/g, "'").replace(/&quot;/g, '"');
}

