
    // Elements allows the system to fundamentally understand an element,
    // the properties associated with it, and how they affect the HTML element
    // -------
    Weebly.Elements = {
      Version        : '0.1',
      Author         : 'David Rusenko',
      Company        : 'Weebly, Inc.',
      propertyContainer : null,
      currentElement : null,
      currentBorder: '',
      currentValidateFunction: null,
      currentSaveValidateFunction: null,
      currentCustomProperties: Array(),
      editing: null,
      customHTML: Array(),
      upload: 0,

      init: function(options) {
	this.propertyContainer = options.propertyContainer;
      },

      selectElement: function(element) {

	if(element != this.currentElement) { 

	  if (this.currentElement) { Weebly.Elements.unselectElement(); }
	  //if (this.currentElement != null) { this.unselectElement(); }
	
	  this.currentElement = element;
	  this.currentBorder = element.style.border;
	  this.currentCustomProperties = Array();

	  //this.generateProperties(element);

	  this.generateProperties(this.currentElement);
      if(this.isFormElement() && this.highlightedElement != element.id){
          this.selectForm();
      }
	}

      },

      unselectElement: function(element) {

    if(this.currentElement) {

	  if (this.editing) hideCustomHTML(this.ucfid, this.editing, this.uproperty[this.editing].ucfpid);
	  hideFlashContainer();

	  if (this.currentContentFieldDef.hasimage == 1) {
            Weebly.ImageResize.endResize();
            Weebly.ImageResize.destroy();
          }

		if (this.idfield.eid == '22397704' || this.idfield.eid == '18362204') {
			Weebly.ImageGallery.destroy();
		}

	  // AdSense callback
	  if (this.idfield.eid == "36391960") {
	    var adUnitType;
	    if (this.uproperty[96630555]['propertyresult'] == 'text') {
	      adUnitType = 'TextOnly';
	    } else if (this.uproperty[96630555]['propertyresult'] == 'image') {
              adUnitType = 'ImageOnly';
            } else if (this.uproperty[96630555]['propertyresult'] == 'text_image') {
              adUnitType = 'TextAndImage';
            }
	    new Ajax.Request(ajax, {parameters:'pos=saveadsense&cookie='+document.cookie+'&cfid='+this.uproperty.cfid+'&backgroundColor='+this.uproperty[53687540]['propertyresult']+'&borderColor='+this.uproperty[52715841]['propertyresult']+'&textColor='+this.uproperty[60388297]['propertyresult']+'&titleColor='+this.uproperty[10290183]['propertyresult']+'&urlColor='+this.uproperty[34189825]['propertyresult']+'&adUnitType='+adUnitType+'&layout='+this.uproperty[81070942]['propertyresult']+'&elementid='+this.idfield.id, onFailure:errFunc, bgRequest: true});

	  }

	  Weebly.Linker.close();

	  if (this.currentSaveValidateFunction != '' && !this.closeOnValidate && this.textChange) { this.textChange = null; validateFunction(); this.closeOnValidate = 1;}
	  else {

	    this.closeOnValidate = null;

	    if (swfu.dontSave) { swfu.dontSave = null; }
	    else { this.saveProperties(); }

	    this.currentElement.style.border = this.currentBorder;
	    this.currentBorder = '';
	    this.currentElement = null;
	    this.currentValidateFunction = null;
	    this.currentSaveValidateFunction = null;

	    this.hideMenuBar();
	    clearElementBox();
	  }

	}

      },

      generateProperties: function(element) {

    if (!this.currentElement) { return; }

	var ucfid = element.id;
	var parentRef = $(element).up('.inside');

  var children = parentRef.childElements();
  for( var i=0; i<children.size(); i++ )
  {
    if( children[i].idfield )
    {
      var finalChild = children[i];
      break;
    }
  }
	try {
          eval("var idfield = " + finalChild.idfield.value);
          eval("var pfield = " + finalChild.pfield.value.replace(/\r\n/g, ''));
	} catch(e) { showError(/*tl(*/'This element is not compatible with a recent upgrade. Please delete this element and drag on a new one.'/*)tl*/); return; }
    this.idfield = idfield;
	this.idfieldNode = finalChild.idfield;
	this.pfield = pfield;
	this.pfieldNode = finalChild.pfield;
	this.ucfid = ucfid;
	this.uproperty = pfield[ucfid];
	this.upload = 0;

        Weebly.Cache.get(ajax, 'pos=elementdef&keys='+idfield.eid, function(t) { Weebly.Elements.handlerGenerateProperties(t, ucfid, idfield.eid, pfield[ucfid]); }, null, {asynchronous:false});

      },
      handlerGenerateProperties: function(responseText, ucfid, eid, properties) {

    if (!this.currentElement) { return; }

	responseText = responseText.replace(/\r\n/g, "");
	responseText = responseText.replace(/\n/g, "");
	
	//console.log("var elementDef = " + responseText);
	try {
	  //eval("var elementDef = " + responseText);
      var elementDef = responseText.evalJSON();
	  this.elementDef = elementDef;
	  var cfid = properties.cfid;
	} catch(e) { showError(/*tl(*/'This element is not compatible with a recent upgrade. Please delete this element and drag on a new one.'/*)tl*/); return; }
	this.currentContentFieldDef = elementDef.contentfields[cfid];
	this.currentValidateFunction = this.currentContentFieldDef.elementonchangejs.replace( new RegExp ( "%%ELEMENTID%%", "gi"), this.ucfid);
	this.currentSaveValidateFunction = this.currentContentFieldDef.elementjs.replace( new RegExp ( "%%ELEMENTID%%", "gi"), this.ucfid);

        //If an image, make it resizable
        if (this.currentContentFieldDef.hasimage == 1) {
			Weebly.ImageResize.init(this.getCurrentImageElement(), { callback: onResize, ucfid: this.ucfid });
        } else {
	// Else assign a border
	  this.currentElement.style.border = "1px dashed #4455aa";
	  this.currentElement.style.padding = "0";
        }

	var menuBarDivHTML = "<table id='menuBarItemContainer' spacing=0 padding=0><tr>";
	menuBarDivHTML += "<td style='position: relative; background: none;'><div id='menubar-l' style='position: absolute; width: 5px; height: 44px; top: 0px; margin-left: -10px; background: url(http://"+editorStatic+"/weebly/images/menubar-l.gif) no-repeat bottom left;'></div> </td>";
	var menuBarAdvancedDivHTML = "";
	
	for(var x =0; x < elementDef.contentfields[cfid].contentfieldproperties.length; ++x) {
	  var xReturn = this.generateProperty(elementDef.contentfields[cfid].contentfieldproperties[x], elementDef.contentfields[cfid].contentfieldproperties[x].cfpid, properties);
	  if (xReturn != "") {
	    if (elementDef.contentfields[cfid].contentfieldproperties[x].advanced == 1) {
	      menuBarAdvancedDivHTML += "<span>"+ xReturn +"</span>";
	    } else {
	      menuBarDivHTML += "<td><span class='menuBarSpan'>"+ xReturn +"</span></td>";
	    }
	  }
	}

	if (menuBarAdvancedDivHTML != "") {
	  menuBarDivHTML += "<td style='background: none;'><div id='menuBarAdvancedDiv'><a href='#' onclick='Weebly.Elements.showHideAdvanced(); return false;' style='color: #1C4671; text-decoration: none;'><img src='http://"+editorStatic+"/weebly/images/arrow_right.gif' style='border: 0;' id='advancedImg' /> "+/*tl(*/"Advanced"/*)tl*/+"</a> <br/><br/><div id='menuBarAdvancedContainer'>"+menuBarAdvancedDivHTML+"</div></div> <div id='menuBarAdvancedPlaceholder' style='height: 20px; width: 80px;'></div></td>";
	}

	menuBarDivHTML += "<td style='position: relative; background: none;'><div id='menubar-r' style='position: absolute; width: 5px; height: 44px; top: 0px; margin-left: 10px; background: url(http://"+editorStatic+"/weebly/images/menubar-r.gif) no-repeat bottom left;'></div> </td>";
	menuBarDivHTML += "</tr></table>";

	$('menuBarDiv').innerHTML = menuBarDivHTML;

	if(this.currentValidateFunction != '') { eval("this.currentValidateFunction = "+this.currentValidateFunction); }
	if(this.currentSaveValidateFunction != '') { eval("validateFunction = "+this.currentSaveValidateFunction); }

	this.showMenuBar();

	// Default actions
	if (this.idfield.eid == '69807794') {
	  editCustomHTML(this.ucfid, 38531508, this.uproperty[38531508].ucfpid);
	/*
 	// Don't work anymore due to Flash 10 - Adobe sucks, this was a horrible decision
  	} else if (this.idfield.eid == '36582085' && this.uproperty.cfid == "85675969") {
	  if (this.uproperty['59220138'] && this.uproperty['59220138'].propertyresult && this.uproperty['59220138'].propertyresult.match(/\/weebly\/images\/na\.jpg$/)) {
	    selectUpload("image", "250", this.uproperty.ucfid, this.uproperty['59220138'].ucfpid);
	  }
        } else if (this.idfield.eid == '18383940' && this.uproperty.cfid == "9839145") {
          if (this.uproperty['24196366'] && this.uproperty['24196366'].propertyresult && this.uproperty['24196366'].propertyresult.match(/\/weebly\/images\/na\.jpg$/)) {
            selectUpload("image", "250", this.uproperty.ucfid, this.uproperty['24196366'].ucfpid);
          }
        } else if (this.idfield.eid == '17854681' && this.uproperty.cfid == "72477774") {
          if (this.uproperty['97955873'] && this.uproperty['97955873'].propertyresult && this.uproperty['97955873'].propertyresult.match(/\/weebly\/images\/na\.jpg$/)) {
            selectUpload("image", "250", this.uproperty.ucfid, this.uproperty['97955873'].ucfpid);
          }
	} else if (this.idfield.eid == '46196121' && this.uproperty.cfid == "94384342") {
	  if (this.uproperty['4073443'] && this.uproperty['4073443'].propertyresult && this.uproperty['4073443'].propertyresult == "#") {
	    selectUpload("file", "", this.uproperty.ucfid, this.uproperty['4073443'].ucfpid);
	  }
        } else if (this.idfield.eid == '63260230' && this.uproperty.cfid == "37049277") {
          if (this.uproperty['30527226'] && typeof(this.uproperty['30527226'].propertyresult) != "undefined" && this.uproperty['30527226'].propertyresult == "") {
            selectUpload("audio", "", this.uproperty.ucfid, this.uproperty['30527226'].ucfpid);
          }
	*/
    } else if (this.idfield.eid == '22397704' && this.uproperty.cfid == "34873637") {
    
		  // Image Gallery v0.1
		  var newUcfpid = Weebly.Elements.getPropertyByReference('<!IMAGESET!>');
		  if (newUcfpid.propertyresult == "" || newUcfpid.propertyresult.match("'length': '0'")) {
			//selectUpload("gallery", "thumbnail:333-250", this.uproperty.ucfid, newUcfpid.ucfpid);
		  } else {
			Weebly.ImageGallery.setCurrent(this.ucfid);
			Weebly.ImageGallery.init();
		  }
		  
	} else if (this.idfield.eid == '18362204') {
	
			// Image Gallery v0.2
			Weebly.ImageGallery.setCurrent(this.ucfid);
			Weebly.ImageGallery.init();
	
	}

	//}// else if (this.idfield.eid == '36582085') {
	/**
	  //console.log(this.uproperty);
	  if (siteType == 'myspace') {
	    selectUpload("image", "370", this.ucfid, this.uproperty[59220138].ucfpid)
	  } else {
	    selectUpload("image", "250", this.ucfid, this.uproperty[59220138].ucfpid)
	  }
	}
	**/

      },

      getCurrentImageElement: function() {

	if (!this.currentElement) { return; }

          var imgElement = {};
          if (this.currentElement.tagName == "IMG") {
            imgElement = $(this.ucfid+"");
          } else {
            imgElement = this.currentElement.getElementsByTagName("IMG")[0];
          }
          return imgElement;

      },

      saveProperties: function() {

	if (!this.currentElement) { return; }

	var jsonPField = toJsonString(this.pfield);
	var jsonIDField = toJsonString(this.idfield);

        this.idfieldNode.value = jsonIDField;
        this.pfieldNode.value = jsonPField;

        new Ajax.Request(ajax, {parameters:'pos=saveproperties&cookie='+document.cookie+'&ucfid='+this.ucfid+'&json='+encodeURIComponent(jsonPField), onFailure:errFunc, bgRequest: true});


      },

      showHideAdvanced: function() {

	if (!this.currentElement) { return; }

	if ($("menuBarAdvancedDiv").getWidth() > 100) { 
	  this.hideAdvanced();
	} else { 
	  this.showAdvanced();
	}


      },

      hideAdvanced: function() {

	if (!this.currentElement) { return; }

	$("menuBarAdvancedDiv").style.width = "80px"; 
	$("menuBarAdvancedDiv").style.height = "15px"; 
	$("menuBarAdvancedPlaceholder").style.width = "80px"; 
	$('advancedImg').src = 'http://'+editorStatic+'/weebly/images/arrow_right.gif';
	Weebly.Elements.positionFlash();

      },

      showAdvanced: function() {

	if (!this.currentElement) { return; }

	$("menuBarAdvancedDiv").style.width = "200px"; 
	$("menuBarAdvancedDiv").style.height = ($('menuBarAdvancedContainer').getHeight()-(-40))+"px";
	$("menuBarAdvancedPlaceholder").style.width = "200px"; 
	$('advancedImg').src = 'http://'+editorStatic+'/weebly/images/arrow_down.gif';
	Weebly.Elements.positionFlash();

      },

      generateProperty: function(property, cfpid, uproperty) {

	if (!this.currentElement) { return; }

	var r = '';

	r += '<b>' + property.title + ":</b> ";

	if (property.markuptype == 'custom') {
	  this.currentCustomProperties.push(cfpid);
	}
	
	if(property.options == "%%fileUpload%%") {

	  // Make sure we overwrite an old value!
	  try {
	    if ($(this.ucfid) && uproperty[cfpid].propertyresult != $(this.ucfid).getElementsByTagName("IMG")[0].src) {
	      uproperty[cfpid].propertyresult = $(this.ucfid).getElementsByTagName("IMG")[0].src.replace("http://", "").replace(/^[^\/]*/, "");
	    }
	  } catch (e) { }

          var newImageWidth = '250';
          if(Weebly.Elements.isProductElement()){
            switch(Weebly.Elements.pfield[Weebly.Elements.ucfid][69650731].propertyresult){
                case 'grid':
                  newImageWidth = 164;
                  break;
                case 'large':
                  newImageWidth = 225;
                  break;
                case 'small':
                  newImageWidth = 125;
                  break;
                case 'long':
                  newImageWidth = 175;
                  break;
            }
          }
	  selectUpload("image", newImageWidth, this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"image\", \""+newImageWidth+"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new Image"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%fileUploadFile%%") {
	  selectUpload("file", "", this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"file\", \"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new File"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%fileUploadDocument%%") {
	  selectUpload("doc", "", this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"doc\", \"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new File"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%fileUploadVideo%%") {
	  selectUpload("video", "", this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"video\", \"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new File"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%fileUploadFlash%%") {
	  selectUpload("flash", "", this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"flash\", \"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new File"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%fileUploadAudio%%") {
	  selectUpload("audio", "", this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"audio\", \"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new File"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
        } else if(property.options == "%%fileUploadGallery%%") {
	  var newUcfpid = Weebly.Elements.getPropertyByReference('<!IMAGESET!>').ucfpid;
	  selectUpload("gallery", "thumbnail:333-250", this.ucfid, newUcfpid);
          r += "<a href='#' onclick='selectUpload(\"gallery\", \"thumbnail:333-250\", "+this.ucfid+", "+newUcfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new Image(s)"/*)tl*/+"</a>";
	} else if(property.options == "%%LINK%%") {
	  r += "<a href='#' onClick='showCreateLinkProperties(\""+uproperty[cfpid].ucfpid+"\")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/icon_link.gif' class='menuIconImage' alt='New Link Icon'/> "+/*tl(*/"Set Link"/*)tl*/+"</a> <i style='font-size: 10px;'>("+/*tl(*/"website, email, file, etc"/*)tl*/+")</i><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value=\""+uproperty[cfpid].propertyresult+"\" /><input type='hidden' id='linkTargetElement' value=''/>";
	} else if(property.options == "%%CONFLINK%%") {
	  r += "<a href='#' onClick='showCreateLinkProperties(\""+uproperty[cfpid].ucfpid+"\")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/icon_link.gif' class='menuIconImage' alt='New Link Icon'/> "+/*tl(*/"Set Link"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value=\""+uproperty[cfpid].propertyresult+"\" /><input type='hidden' id='linkTargetElement' value=''/>";
	} else if(property.options == "%%NUMBER%%") {
	  r += "<input type='text' id='"+uproperty[cfpid].ucfpid+"' value=\""+uproperty[cfpid].propertyresult.replace(/"/g, "&quot;")+"\" onBlur='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");' onKeyUp='Weebly.Elements.onTextChange(\""+uproperty[cfpid].ucfpid+"\");'/>";
	} else if(property.options == "%%ALPHANUMERIC%%") {
	  r += "<input type='text' id='"+uproperty[cfpid].ucfpid+"' value=\""+uproperty[cfpid].propertyresult.replace(/"/g, "&quot;")+"\" onBlur='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");' onKeyUp='Weebly.Elements.onTextChange(\""+uproperty[cfpid].ucfpid+"\");'/>";
	} else if(property.options == "%%EMAIL%%") {
      var emailValue = uproperty[cfpid].propertyresult ? uproperty[cfpid].propertyresult.replace(/"/g, "&quot;") : userEmail;
	  r += "<input type='text' id='"+uproperty[cfpid].ucfpid+"' value=\""+emailValue+"\" onBlur='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");' onKeyUp='Weebly.Elements.onTextChange(\""+uproperty[cfpid].ucfpid+"\");'/>";
	} else if(property.options == "%%CUSTOMHTML%%") {
	  r += "<a href='#' onclick='editCustomHTML(\""+this.ucfid+"\", \""+cfpid+"\", \""+uproperty[cfpid].ucfpid+"\")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/page_script.gif' class='menuIconImage' alt='Edit Custom HTML'/>  Edit Custom HTML</a><textarea class='customhtml_textarea' onBlur='hideCustomHTML(\""+this.ucfid+"\", \""+cfpid+"\", \""+uproperty[cfpid].ucfpid+"\")' id='"+uproperty[cfpid].ucfpid+"CustomHTML' rows='20' style='border: 1px solid #ccc; display: none; height: 150px; width: 100%;'>"+uproperty[cfpid].propertyresult+"</textarea>";
	} else if(property.options == "%%EMBEDCODE%%") {
	  r += "Custom HTML";
	} else if(property.options == "%%editPollDaddy%%") {
	  r += "<a href='#' onclick='editPollDaddy(\""+uproperty[cfpid].propertyresult.replace(/"/g, "&quot;")+"\", \""+this.ucfid+"\", \""+uproperty['89468692'].ucfpid+"\")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> Edit Poll</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%longText%%") {
      r += "<div class='menuBarExpand' id='longprop-"+uproperty[cfpid].ucfpid+"'><a href='#' onclick='Weebly.Elements.toggleLongProperty(\""+uproperty[cfpid].ucfpid+"\", \"250\"); return done();' class='menuIconAdvanced'><img src='http://"+editorStatic+"/weebly/images/arrow_right.gif' id='longprop-icon-"+uproperty[cfpid].ucfpid+"' style='border: none;'/> "+/*tl(*/"Edit Text"/*)tl*/+"</a><textarea style='width:230px; height:75px; border:1px solid #999999; margin: 7px 0 0 6px;' id='"+uproperty[cfpid].ucfpid+"' onBlur='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");' onKeyUp='Weebly.Elements.onTextChange(\""+uproperty[cfpid].ucfpid+"\");'>"+uproperty[cfpid].propertyresult+"</textarea><div style='position:relative; width:100%'><a href='#' onclick='Weebly.Elements.toggleLongProperty(\""+uproperty[cfpid].ucfpid+"\", \"250\"); return done();'><img src=\"http://"+editorStatic+"/weebly/images/options-save.gif\" alt=\"Save\" class=\"editmenu-save\" /></a></div></div><div class='menuBarExpandPlaceholder'></div>";
    } else if(property.options == "%%longTextInstructions%%") {
      r += "<div class='menuBarExpand' id='longprop-"+uproperty[cfpid].ucfpid+"'><a href='#' onclick='Weebly.Elements.toggleLongProperty(\""+uproperty[cfpid].ucfpid+"\", \"250\"); return done();' class='menuIconAdvanced' title='Field instructions are displayed next to the field to assist a website visitor.'><img src='http://"+editorStatic+"/weebly/images/arrow_right.gif' id='longprop-icon-"+uproperty[cfpid].ucfpid+"' style='border: none;'/> "+/*tl(*/"Edit Text"/*)tl*/+"</a><textarea style='width:230px; height:75px; border:1px solid #999999; margin: 7px 0 0 6px;' id='"+uproperty[cfpid].ucfpid+"' onBlur='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");' onKeyUp='Weebly.Elements.onTextChange(\""+uproperty[cfpid].ucfpid+"\");'>"+uproperty[cfpid].propertyresult+"</textarea><div style='position:relative; width:100%'><a href='#' onclick='Weebly.Elements.toggleLongProperty(\""+uproperty[cfpid].ucfpid+"\", \"250\"); return done();'><img src=\"http://"+editorStatic+"/weebly/images/options-save.gif\" alt=\"Save\" class=\"editmenu-save\" /></a></div></div><div class='menuBarExpandPlaceholder'></div>";
    } else if(property.options == "%%inputOptions%%") {
	  r += "<div class='menuBarExpand' id='longprop-"+uproperty[cfpid].ucfpid+"'><a href='#' onclick='Weebly.Elements.toggleLongProperty(\""+uproperty[cfpid].ucfpid+"\", \"250\"); Weebly.Elements.editInputOptions(\""+uproperty[cfpid].propertyresult.replace(/"/g, "&quot;").replace(/'/g, "&#039;")+"\", \""+this.ucfid+"\", \""+uproperty[cfpid].ucfpid+"\"); return done();' class='menuIconAdvanced'><img src='http://"+editorStatic+"/weebly/images/arrow_right.gif' id='longprop-icon-"+uproperty[cfpid].ucfpid+"' style='border: none;'/> "+/*tl(*/"Edit Options"/*)tl*/+"</a><div id='inputoptions-"+uproperty[cfpid].ucfpid+"'></div><div style='position:relative'><div class='predefined-options-container'><div class='predefined-options' style='display:none;'></div><div class='predefined-options-handle'><a href='#' onclick='Weebly.Elements.togglePredefinedOptions(\""+uproperty[cfpid].ucfpid+"\", \""+this.ucfid+"\"); return done();'>Predefined Options&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a><a href='#' onclick='Weebly.Elements.togglePredefinedOptions(\""+uproperty[cfpid].ucfpid+"\", \""+this.ucfid+"\"); return done();'><img src='http://"+editorStatic+"/weebly/images/blog-settings-arrow-down.gif' class='predefined-options-arrow' border='0' /></a></div></div></div><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult.replace(/'/g, "&#039;")+"'/></div><div class='menuBarExpandPlaceholder'></div>";
	} else {
	  r += "<select id='"+uproperty[cfpid].ucfpid+"' onChange='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");'>";
	  var options = property.options.split("/");
	  var results = property.result.split("/");
	  for (var x=0; x < options.length; x++) {
	    var selected = '';
	    if (results[x] == uproperty[cfpid].propertyresult.replace(/'/g, '"')) { selected = ' selected'; }
	    r += "<option value='"+results[x]+"'"+selected+">"+options[x]+"</option>";
	  }
	  r += "</select>";
	}
	//r += "<br/>";

	if (property.referenceproperty != '') {
	  this.currentValidateFunction = this.currentValidateFunction.replace( new RegExp( property.referenceproperty, "gi"), uproperty[cfpid].ucfpid);
	  this.currentSaveValidateFunction = this.currentSaveValidateFunction.replace( new RegExp( property.referenceproperty, "gi"), uproperty[cfpid].ucfpid);
	}

	if (property.hidden != 1) { return r; } else { return ''; }
      },

      onTextChange: function(ucfpid, lastValue) {

	if (!this.currentElement) { return; }

	this.textChange = 1;

	if (typeof(lastValue) == "undefined") {
	  // First iteration
	  this.onTextChangeTracking = ucfpid;
	  setTimeout("Weebly.Elements.onTextChange('"+ucfpid+"', '"+$(ucfpid+"").value+"');", 1500);
	} else if($(ucfpid+"") && $(ucfpid+"").value && $(ucfpid+"").value == lastValue || ($(ucfpid+"").value == '' && lastValue == '')) {
	  // User has stopped typing
	  this.onTextChangeTracking = null;
	  Weebly.Elements.onChange(ucfpid);
        } else if(ucfpid != this.onTextChangeTracking) {
	  // User is still typing
	  setTimeout("Weebly.Elements.onTextChange('"+ucfpid+"', '"+$(ucfpid+"").value+"');", 1500);
        }

      },

      onChange: function(ucfpid) {

	if (!this.currentElement) { return; }

	if (this.currentValidateFunction != '' && typeof(this.currentValidateFunction) == "function") { this.currentValidateFunction(ucfpid); }
	if (this.currentSaveValidateFunction != '') { validateFunction(); }
	else { this.continueOnChange(); }

      },

      continueOnChange: function() {

	if (!this.currentElement) { return; }

       var customContent = this.currentContentFieldDef.customcontent;
       customContent = customContent.replace( new RegExp ( "%%INEDITOR%%", "gi"), 1);

       for(var x = 0; x < this.currentContentFieldDef.contentfieldproperties.length; ++x) {
	var cfpid = this.currentContentFieldDef.contentfieldproperties[x].cfpid;

	if (!this.uproperty[cfpid]) { continue; }

	var ucfpid = this.uproperty[cfpid].ucfpid;
	var property = this.currentContentFieldDef.contentfieldproperties[x];
	
        var result = '';
	if ($(""+ucfpid) && ($(""+ucfpid).value || $(""+ucfpid).value == '')) { 
	  result   = $(""+ucfpid).value;
	  this.pfield[this.ucfid][cfpid].propertyresult = result;
	} else if (property.options == "%%CUSTOMHTML%%") {
	  result   = $(""+ucfpid+"CustomHTML").value;
	  this.pfield[this.ucfid][cfpid].propertyresult = "%%NOCHANGE%%";
	} else { 
	  result   = this.pfield[this.ucfid][cfpid].propertyresult;
	}

	if (property.markuptype == "html") {
	  this.currentElement[property.property] = result;
	} else if (property.markuptype == "css") {

	  var styles = {};
	  // cssFloat or styleFloat -- only quirky property
	  if (property.property == "float") {
	    //var styles = { cssFloat: result, styleFloat: result };
	    if (this.currentElement.style.styleFloat) {
	      var styles = { styleFloat: result };
	    } else {
	      var styles = { cssFloat: result };
	    }
	  } else {
	    eval("var styles = { '"+property.property+"': '"+result+"' };");
	  }
	  Element.setStyle(this.currentElement, styles);
	} else if (property.markuptype == "custom") {

	  // Don't update Custom HTML or upload src fields
	  if (!(property.property == "%%CUSTOM2%%" && result == "%%NOCHANGE%%")) {
	    if(property.property == '%%FEEDURL%%'){
	      customContent = customContent.replace( new RegExp( property.property, "gi"), encodeURIComponent(result));
	    } else {
	      customContent = customContent.replace( new RegExp( property.property, "gi"), result);
	    }
	  } else {
	    customContent = customContent.replace( new RegExp( property.property, "gi"), this.customHTML[ucfpid]);
	  }

	}
       }

       if (this.currentContentFieldDef.hascontent == 2) {

        customContent = customContent.replace( new RegExp ( "%%ELEMENTID%%", "gi"), this.ucfid);
        customContent = customContent.replace( new RegExp ( "%%PAGEELEMENTID%%", "gi"), this.idfield.id);
        //customContent = customContent.replace(/\n/g,"");
        customContent = customContent.replace(/<weebly only_export([^>]*)>([\S\s]*?)<\/weebly>/img, '');

        var subdomain = configSiteName.replace(/\.weebly\.com$/, "");
        customContent = customContent.safeReplace(/%%WEEBLYSUBDOMAIN%%/ig, subdomain);
        customContent = customContent.safeReplace(/%%WEEBLYCONFIGDOMAIN%%/ig, configSiteName);

	var isGallery = null;
        if (customContent.match(/<!WEEBLYGALLERY.*?!>/)) {
          Weebly.ImageGallery.destroy();
	  Weebly.ImageGallery.setCurrent(this.ucfid);
          var updateHtml = Weebly.ImageGallery.update();
          customContent = customContent.safeReplace(/<!WEEBLYGALLERY.*?!>/, updateHtml);
	  isGallery = 1;
        }
        
    var _match;

	// Replace external account IDs
	if (_match = customContent.match(/<!EXTERNAL:([^!]+)!>/)) {
	  var remoteSite = _match[1];
	  if (remoteAccounts[remoteSite]) {
	    customContent = customContent.safeReplace(/<!EXTERNAL:([^!]+)!>/, remoteAccounts[remoteSite].remoteId);
	  }
	}

    if(_match = customContent.match(/<!WEEBLYRADIO\-(.*?)\-(.*?)!>/)){
        customContent = customContent.replace('<!WEEBLYRADIO-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawRadioOptions(_match[2]));
    }
    if(_match = customContent.match(/<!WEEBLYSELECT\-(.*?)\-(.*?)!>/)){
        customContent = customContent.replace('<!WEEBLYSELECT-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawSelectOptions(_match[2]));
    }
    if(_match = customContent.match(/<!WEEBLYCHECKBOXES\-(.*?)\-(.*?)!>/)){
        customContent = customContent.replace('<!WEEBLYCHECKBOXES-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawCheckboxes(_match[2]));
    }

    if(_match = customContent.match(/<!(.*?)CONTENTS!>/)){
        customContent = customContent.safeReplace(/<!(.*?)CONTENTS!>/, $(_match[1]+'-form-list').innerHTML);
    }

	if(!this.isProductElement()){
          var writeContent = customContent.replace(/<weebly include_once(_noexport)? ([^>]*)>([\S\s]*?)<\/weebly>/im, '');
          this.currentElement.innerHTML = writeContent;
          if(this.isFormElement()){
              initDraggables();
              Weebly.Form.fieldInstructionsHandler();
          }
          if($(this.currentElement).down('.weebly-form-instructions')){
              Weebly.Form.setupFieldInstructions($(this.currentElement).down('.weebly-form-instructions'));
          }
    	}
    else{
        this.currentElement.innerHTML = this.currentElement.innerHTML;
    }

	if (isGallery) {
	  Weebly.ImageGallery.init();
	  isGallery = null;
	}

	this.tryRunScripts(customContent);
	makeIframesDraggable($('secondlist'));

       }

       if (this.currentContentFieldDef.hasimage == 1) {
	  Weebly.ImageResize.destroy();
	  var img = this.getCurrentImageElement();
    var ucfid = this.ucfid;
	  img.onload =  
	    function(){ 
	      Weebly.ImageResize.init(img, {callback: onResize, ucfid: ucfid});
	      Behaviour.apply();
	      img.onload = null;
	    };
       }

       if (this.closeOnValidate) {
	 this.unselectElement();
       }

      },

      showMenuBar: function() {

		var menuBarDiv = $('menuBarDiv');
		menuBarDiv.style.height = '40px';
		Effect.Queues.get('menuscope').each(function(e) { e.cancel(); });
		Effect.Appear(menuBarDiv, { duration: 0.3, queue: {position: 'end', scope: 'menuscope'}, afterFinish: function() {
			showEvent('showProperties');
			Weebly.Elements.positionFlash();
		}});

      },

      toggleLongProperty: function(ucfpid, width){
          var prop = $('longprop-'+ucfpid);
          if(prop){
              if(prop.getStyle('width') == width+'px'){
                  $('longprop-'+ucfpid).setStyle({'width':'', 'height':'', zIndex:'', paddingBottom:'', overflow:'hidden'});
                  $('longprop-icon-'+ucfpid).src = 'http://'+editorStatic+'/weebly/images/arrow_right.gif';
              }
              else{
                  $('longprop-'+ucfpid).setStyle({'width':width+'px', 'height':'auto', zIndex:'100', paddingBottom:'31px', overflow:'visible'});
                  $('longprop-icon-'+ucfpid).src = 'http://'+editorStatic+'/weebly/images/arrow_down.gif';
                  if(Prototype.Browser.Gecko){
                      $('longprop-'+ucfpid).setStyle({'top':'8px'});
                  }
              }
          }
      },
      
      togglePredefinedOptions: function(ucfpid, ucfid){
          var prop = $('longprop-'+ucfpid);
          var options = prop.down('.predefined-options');
          if(!options.visible()){
              if(options.childElements().size() == 0){
                  Weebly.Elements.createCommonFieldOptions(ucfpid, ucfid);
              }
              Effect.BlindDown(options, {afterFinish:function(effect){
                  effect.element.setStyle({overflow:''});
              }});
              prop.down('.predefined-options-arrow').src='http://'+editorStatic+'images/blog-settings-arrow-up.gif';
          }
          else{
              Effect.BlindUp(options);
              prop.down('.predefined-options-arrow').src='http://'+editorStatic+'images/blog-settings-arrow-down.gif';
          }
      },

      /*editLongText : function(current, ucfid, ucfpid){
          if($('longtext-'+ucfpid) && !$('longtext-open-'+ucfpid)){
              $(ucfpid).remove();
              var div = new Element('div', {'id':'longtext-open-'+ucfpid, 'class':'galleryCaptionForm'}).setStyle({'top':'35px'});
              div.update('<textarea id="longtext-value-'+ucfpid+'" onBlur="Weebly.Elements.onChange('+ucfpid+');">'+current+'</textarea>');
              $('longtext-'+ucfpid).insert({'bottom':div});
          }
      },

      saveLongText : function(ucfpid){
          if($('longtext-value-'+ucfpid)){
              $(ucfpid).value = $('longtext-value-'+ucfpid).value;
              Weebly.Elements.toggleLongProperty(ucfpid, "250");
          }
      },*/

      itemTemplate : new Template('<li style="position:relative;"><img src="http://'+editorStatic+'/weebly/images/options-move.gif" alt="move" class="options-handle editmenu-input-move" /><input type="text" value="#{value}" /><a href="#" onclick="Weebly.Elements.removeInputOption(this); return false;"><img src="http://'+editorStatic+'/weebly/images/options-delete.gif" alt="Delete" class="editmenu-input-delete" /></a></li>'),

      editInputOptions : function(current, ucfid, ucfpid){
          if($('inputoptions-'+ucfpid) && !$('inputoptions-open-'+ucfpid)){
              var values = $(ucfpid).value.split('||');
              var container = new Element('div');
              var div = new Element('div', {'id':'inputoptions-open-'+ucfpid, 'class':'editmenu-input-options'});
              container.insert({'top':div});
              container.insert({'bottom':'<div style="position:relative; width:100%;"><a href="#" onclick="Weebly.Elements.addInputOption(); return done();"><img src="http://'+editorStatic+'/weebly/images/options-plus.gif" alt="Add" class="editmenu-input-plus" /></a> <a href="#" onclick="Weebly.Elements.undoInputOptions(\''+ucfpid+'\', \''+ucfid+'\'); return done();"><img src="http://'+editorStatic+'/weebly/images/undo_button.gif" alt="Undo" class="editmenu-input-undo" style="display:none;" /></a> <a href="#" onclick="Weebly.Elements.saveInputOptions(\''+ucfpid+'\'); return done();"><img src="http://'+editorStatic+'/weebly/images/options-save.gif" alt="Save" class="editmenu-input-save" /></a></div>'});
              var list = new Element('ul', {'id':'inputoptions-list'});
              div.insert({top:list});
              var i = 0;
              values.each(function(value){
                  list.insert({bottom:Weebly.Elements.itemTemplate.evaluate({'value':value})});
              });

              $('inputoptions-'+ucfpid).insert({'bottom':container});
              Sortable.create('inputoptions-list',{handle:'options-handle'});
              if(typeof(predefinedOptionsUndoDisplay) !== 'undefined' && predefinedOptionsUndoDisplay){
                  Effect.Appear(container.down('.editmenu-input-undo'));
              }
          }
      },

      removeInputOption : function(el){
          $(el).up('li').remove();
      },

      addInputOption : function(){
          if($('inputoptions-list')){
              $('inputoptions-list').insert({bottom:Weebly.Elements.itemTemplate.evaluate({'value':''})});
              Sortable.create('inputoptions-list',{handle:'options-handle'});
          }
      },

      saveInputOptions : function(ucfpid){
          if($('inputoptions-list')){
              var values = new Array();
              $('inputoptions-list').select('input').each(function(input){
                  values.push(input.value);
              });
              $(ucfpid).value = values.join('||');
              Weebly.Elements.onChange(ucfpid);
              $('inputoptions-open-'+ucfpid).up().remove();
              Weebly.Elements.toggleLongProperty(ucfpid, '250');
          }
      },
      
      createCommonFieldOptions : function(ucfpid, ucfid){
          var prop = $('longprop-'+ucfpid);
          var options = prop.down('.predefined-options');
          $H(Weebly.Form.commonFieldOptions).keys().each(function(key){
              var div = new Element('div', {'class':'predefined-option', 'title':Weebly.Form.commonFieldOptions[key].join(', ')});
              div.update(key);
              div.observe('click', function(){
                  Weebly.Elements.insertCommonFieldOptions(ucfpid, ucfid, key);
              });
              options.insert({'bottom':div});
          });
      },
      
      insertCommonFieldOptions : function(ucfpid, ucfid, key){
          var prop = $('longprop-'+ucfpid);
          prop.down('.predefined-options').hide();
          predefinedOptionsUndoDisplay = true;
          predefinedOptionsUndo = $(ucfpid).value;
          prop.down('.predefined-options-arrow').src='http://'+editorStatic+'images/blog-settings-arrow-down.gif';
          var options = Weebly.Form.commonFieldOptions[key].join('||');
          $(ucfpid).value = options;
          $('inputoptions-open-'+ucfpid).up().remove();
          Weebly.Elements.editInputOptions(options, ucfid, ucfpid);
      },
      
      undoInputOptions : function(ucfpid, ucfid){
          var prop = $('longprop-'+ucfpid);
          predefinedOptionsUndoDisplay = false;
          $(ucfpid).value = predefinedOptionsUndo;
          $('inputoptions-open-'+ucfpid).up().remove();
          Weebly.Elements.editInputOptions(predefinedOptionsUndo, ucfid, ucfpid);
      },

      positionFlash: function() { // position over the menu bar. ALSO positions non-flash plain uploader
	if(!forceFlashPosition){
        if (this.upload && $('menuBarItemContainer')) {

          var el = $('menuBarItemContainer').down('td').nextSibling.down('.menuBarSpan');
          el.style.height = "30px";
          el.style.display = "block";

          //// for html-based file uploader
          //if (this.preferPlainUploader()) {
		  //  getPlainUploader('menu').show(el, el);
          //  hideFlashContainer();
          //}else{
          
		      if (Prototype.Browser.Gecko) {
		        Element.clonePosition($('flashContainer'), el, {offsetTop: 18});
		      } else {
		        $('flashContainer').style.position = 'relative';
		        Element.clonePosition($('flashContainer'), el);
		        $('flashContainer').style.position = 'absolute';
		      }
		      showFlashContainer(21); // position over the menu (which has zindex of 20)
		      
		  //}

        } else {

          hideFlashContainer();

        }
    }

      },

      hideMenuBar: function() {

	Effect.Queues.get('menuscope').each(function(e) { e.cancel(); });
	Effect.Fade('menuBarDiv', { duration: 0.3, queue: {scope: 'menuscope'}});
	$('editMenu').style.display = 'none';

	hideFlashContainer();

      },

      getPropertyByReference: function(reference) {

	if (!this.currentElement) { return {}; }

	for (var cfpid in this.uproperty) {
	  if (cfpid != 'cfid' && cfpid != 'ucfid' && this.uproperty[cfpid].referenceproperty == reference) {
	    return this.uproperty[cfpid];
	  }
	}

	return {};

      },

      tryRunScripts: function(customContent) {

	scriptSrc = new Array();
	scriptId  = new Array();
	scriptType= new Array();
	var runScripts= 0;

	while (customContent.indexOf('<weebly include_once') > -1 && scriptSrc.length < 30) {
	  customContent = customContent.replace(/<weebly include_once(_noexport)? ([^>]*)>([\s\S]*?)<\/weebly>/im, '');
	  scriptSrc.push(RegExp.$3);
	  scriptId.push(RegExp.$2);
	  runScripts = 1;
	}

	if( runScripts > 0 ) {
	  runIncludedScripts();
	}

      },

      isProductElement: function(){
          return $(this.currentElement).down('.product-image');
      },

      getCurrentProductID: function(){
          if(this.isProductElement()){
              var form = $(this.ucfid + '-product-data');
              return $F(form['productID']);
          }
          return false;
      },

      isFormElement : function(){
          return this.currentElement && $(this.currentElement.id+'-form-parent') ? true : false;
      },

      selectForm : function(){
          this.highlightElement();
          var listEl = $(this.currentElement).down('.formlist');
          if($(listEl.id+'-cover')){
              $(listEl.id+'-cover').remove();
          }
          elementsPage('form');
          if(!$H(userEvents).get('editAdvancedForm') && $('elements_container').visible()){
            showTip("These elements can be dragged into your form to create new fields", $('elementlist'), 'y', '101');
            showEvent('editAdvancedForm');
          }
          Behaviour.apply();
          Weebly.Elements.unselectElement();
      },

      highlightElement: function(){
          if(Weebly.Elements.highlightedElement){return;}
          Weebly.Elements.highlightedElement = this.currentElement.id;
          Sizzle.matches(':not(#secondlist .formlist .columnlist)', $$('#secondlist .columnlist')).each(function(column){
              Sortable.destroy(column.id);
              contentDraggables = contentDraggables.findAll(function(el){return el.id != column.id;});
          })
          Sortable.destroy('secondlist');
          contentDraggables = contentDraggables.findAll(function(el){return el.id != 'secondlist';});
          var cover_top = new Element('div', {'id':'cover-top','class':'editor-cover'});
          var cover_left = new Element('div', {'id':'cover-left','class':'editor-cover'});
          var cover_right = new Element('div', {'id':'cover-right','class':'editor-cover'});
          var cover_bottom = new Element('div', {'id':'cover-bottom','class':'editor-cover'});
          var buttons = new Element('div', {'id':'cover-buttons'}).update('<img src="http://'+editorStatic+/*tli(*/'/weebly/images/form-options.gif'/*)tli*/+'" style="margin-right:10px; cursor:pointer;" onclick="Weebly.Elements.selectElement($(\''+Weebly.Elements.highlightedElement+'\'))" /><img src="'+/*tli(*/'images/view-data.gif'/*)tli*/+'" style="margin-right:10px; cursor:pointer;" onclick="viewFormData(\''+this.currentElement.id+'\')" /><img src="'+/*tli(*/'images/form-close.gif'/*)tli*/+'" style="cursor:pointer;" onclick="Weebly.Elements.removeHighlight();" />');
          $('scroll_container').insert({bottom:cover_top});
          $('scroll_container').insert({bottom:cover_left});
          $('scroll_container').insert({bottom:cover_right});
          $('scroll_container').insert({bottom:cover_bottom});
          $('scroll_container').insert({bottom:buttons});
          Weebly.Elements.resizeHighlight();
          $$('.editor-cover').each(function(cover){
              cover.observe('click', Weebly.Elements.removeHighlight)
          });
      },

      removeHighlight : function(){
          Weebly.Elements.unselectElement();
          Weebly.Elements.highlightedElement = null;
          Weebly.Elements.highlightedHeight = 0;
          $$('.editor-cover').invoke('remove');
          $('cover-buttons').remove();
          if(destroySecondList){
              elementsPage('blog');
          }
          else{
              elementsPage('default');
          }
          updateList();
      },

      resizeHighlight : function(){
        if(Weebly.Elements.highlightedElement){
            var el = $(this.highlightedElement);
            var dimensions = el.getDimensions();
            if(Weebly.Elements.highlightedHeight != dimensions.height){
                Weebly.Elements.highlightedHeight = dimensions.height;
                var padding = 10;
                var start_top = parseInt($('scroll_container').getStyle('marginTop').replace(/[^\d]/g, ''));
                var offset = el.cumulativeOffset();
                var top_height = offset.top-(start_top+padding);
                var bottom_top = top_height + dimensions.height + (2*padding);
                var bottom_height = $('icontent_container').getHeight() - bottom_top;
                var side_height = dimensions.height + (2*padding);
                var right_edge = ($('scroll_container').getWidth() - (offset.left + dimensions.width)) - 2*padding;
                $('cover-top').setStyle({width:'100%',height:top_height+'px', top:'0px'});
                $('cover-left').setStyle({width:(offset.left-padding)+'px',height:side_height+'px', top:top_height+'px'});
                $('cover-right').setStyle({width:right_edge+'px',height:side_height+'px', top:top_height+'px', right:'0px'});
                $('cover-bottom').setStyle({width:'100%',height:bottom_height+'px', top:bottom_top+'px'});
                $('cover-buttons').setStyle({position:'absolute', zIndex:'10', top:(top_height - 45)+'px', right:right_edge+'px'});
            }
            setTimeout('Weebly.Elements.resizeHighlight()', 300);
        }
      }
      
      //// for html-based file uploader
      //,
      //preferPlainUploader: function() { // can only be called when an element is selected. for current element
	  //  return !(this.idfield.eid == '22397704' && this.uproperty.cfid == "34873637" || this.idfield.eid == '18362204');
      //  // is not an image gallery
	  //}

    };

    //------------
    /// End of Elements module
    ////

