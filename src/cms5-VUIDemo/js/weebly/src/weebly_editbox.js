var currentBox    = String();
var currentTop    = 0;
var currentLeft   = 0;
var currentAlign  = '';
var colorBoxShown = 0;
var isPaste	  = 0;
var initBoxes     = new Array;
var timeOuts      = 0;
var currentSel    = new Object;
var initializingBox = null;
var lastSave	  = null;
var iframeEl 	  = null;
var editBoxSaveCallback  = null;
var editBoxRedrawOptions = false;
var editBoxShowOptions = true;

function init(nameOfBox) {

	var self = this;

	//setTimeout("document.getElementById('"+nameOfBox+"Edit').contentWindow.document.designMode = 'On'",500);

	setTimeout("init2('"+nameOfBox+"')", 500);

}

function init2(nameOfBox) {

	var activateCount = 0;
	var keepGoing = 0;

	// Crap out if these elements don't exist anymore, for whatever reason
	if (!(document.getElementById(nameOfBox+'Edit') && document.getElementById(nameOfBox+'Edit').contentWindow)) { return false; }

	var myEl = document.getElementById(nameOfBox+'Edit').contentWindow.document;

	while (keepGoing == 0 && activateCount < 3) {
	  try{
	   myEl.designMode = 'On';
	   keepGoing = 1;
	  } catch (e) {
	   keepGoing = 0;
	   activateCount++;
	  }
	}

	if (typeof document.addEventListener == "function") {
	   myEl.addEventListener("keyup", function(e){self.keyUp(); return true;}, false);
           myEl.addEventListener("keydown", function(e){self.keyDown(); self.detectPaste(e); return true;}, false);
	}
	//initBoxes.push(nameOfBox);

        // Firefox is "ready" for the runCommand here
        // otherwise, spits out a nsComponentNotReady error later
        if (navigator.appVersion.indexOf("MSIE") == -1) {
          if (myEl.body.innerHTML.indexOf('Click here to edit.') > -1 || myEl.body.innerHTML.indexOf('This is your new blog post.') > -1 || myEl.body.innerHTML.indexOf('100.00') > -1) {
            document.getElementById(nameOfBox+'Edit').contentWindow.focus();
            try {
              selectAll();
            } catch(e) { setTimeout("selectAll();", 500); }
          }
        }

	// The order in which things are done varies for IE and FF
	// IE wants continueShowEditBox here
        if (navigator.appVersion.indexOf("MSIE") > -1) {
          continueShowEditBox(nameOfBox);
        }

}
                                                                                                                             
function hideEditBox(name, tries) {

      // If box is still being initalized       
      if (initializingBox) { 
	if (!tries) { tries = 0; }
	if (tries < 5) {
	  setTimeout("hideEditBox('"+name+"', "+(tries+1)+");", 500); 
	  return;
	}
      }

      if (document.getElementById(name) && document.getElementById(name+'Edit') && currentBox) {

	// Check that the iframe is still the same before saving text
	// This helps an issue with Firefox and the back button where text gets "whacked"
	if(document.getElementById(name+'Edit') && document.getElementById(name+'Edit').contentWindow && document.getElementById(name+'Edit').contentWindow.document.body.id == 'icontent') {

	  var cleanText = clean();

	  document.getElementById(name).innerHTML = cleanText;

      if('function' === typeof(editBoxSaveCallback)){
          editBoxSaveCallback(name, cleanText, currentAlign);
      }
	  else if (name != 'weebly_site_title') {
	    saveContent(name,cleanText,currentAlign);

	  } else {
	    updateSiteTitle(cleanText);
	  }

	}

	var element = document.getElementById(name);
        Element.setStyle(document.getElementById(name+'Edit'), {'display':'none'} );
        //Element.setStyle(element, {'display':'block'} );
        Element.show(element);

	// If there's a picture before it, un-gray out picture
	if (element.previousSibling && element.previousSibling.previousSibling && element.previousSibling.previousSibling.childNodes[0] && element.previousSibling.previousSibling.childNodes[0].childNodes[0] && element.previousSibling.previousSibling.childNodes[0].childNodes[0].tagName == "IMG") {
	  if (!navigator.appVersion.match("MSIE")) {
            var imageElement = element.previousSibling.previousSibling.childNodes[0].childNodes[0];
            $(imageElement).removeClassName("element-grayed");
            imageElement.style.position = "static";
	    $(name+'Edit').style.minHeight = "auto";
	  }
	}

        var activateCount = 0;
        var keepGoing = 0;
        var myEl = document.getElementById(name+'Edit').contentWindow.document;

        while (keepGoing == 0 && activateCount < 3) {
          try{
           myEl.designMode = 'Off';
           keepGoing = 1;
          } catch (e) {
           keepGoing = 0;
           activateCount++;
          }
        }

      }

      currentBox = null;
      Weebly.Elements.hideMenuBar();
      //Element.setStyle('colorpicker', {'display':'none'} );                
      Weebly.Linker.close();
      Behaviour.apply();
      if(editBoxRedrawOptions){
          Weebly.Elements.generateProperties(Weebly.Elements.currentElement);
      }

	if (name == 'weebly_site_title') {
		afterUpdateSiteTitle();
	}
	
	//alert("Hide edit box!"+name);

}
                                                                                                                             
function showEditBox(name, elementid, options) {
        
	//setTimeout('alert("Show edit box! '+name+', "+currentBox);', 500);

	if (name == currentBox) return;
        if (currentBox) hideEditBox(currentBox);

	initializingBox = 1;
        currentBox   = name;
    if( $(name) )
    {
       currentAlign = $(name).getStyle('textAlign'); 
    }
    else
    {
        currentAlign = '';
    }

    editBoxSaveCallback = null;
    if(options){
        handleEditBoxOptions(options);
    }
       
	// Create iframe?
	createIframe(name);

	try {
	  if( document.getElementById(name+'Edit').contentWindow.document.designMode != "On") { init(name); }
        } catch (e) { currentBox = null; }     	   

	// The order in which things are done varies for IE and FF
	// Firefox wants continueShowEditBox here
	if (navigator.appVersion.indexOf("MSIE") == -1) {
	  continueShowEditBox(name);
	}

}

function handleEditBoxOptions(options){
    if('function' == typeof(options.saveCallback)){
        editBoxSaveCallback = options.saveCallback;
    }
    if('undefined' !== typeof(options.redrawOptions)){
        editBoxRedrawOptions = options.redrawOptions;
    }
    if('undefined' !== typeof(options.showOptions)){
        editBoxShowOptions = options.showOptions;
    }
}

function continueShowEditBox(name) {

	// Crap out if these elements don't exist anymore, for whatever reason
	if (!(document.getElementById(name) && document.getElementById(name+'Edit') && document.getElementById(name+'Edit').contentWindow)) { return false; }

	if(editBoxRedrawOptions){
        Weebly.Elements.hideMenuBar();
    }

    var origBox   = document.getElementById(name);
	var myEl    = document.getElementById(name+'Edit');
	var myFrame = document.getElementById(name+'Edit').contentWindow.document;
        var editBody = myFrame.getElementsByTagName('body');

        var boxSize   = Element.getDimensions(origBox);
	var fontSize  = Element.getStyle(origBox, 'font-size');

        // If there's a picture before it, gray out picture
	var element = origBox;
        if (element.previousSibling && element.previousSibling.previousSibling && element.previousSibling.previousSibling.childNodes[0] && element.previousSibling.previousSibling.childNodes[0].childNodes[0] && element.previousSibling.previousSibling.childNodes[0].childNodes[0].tagName == "IMG") {
	  if (!navigator.appVersion.match("MSIE")) {
	    var imageElement = element.previousSibling.previousSibling.childNodes[0].childNodes[0];
	    	$(imageElement).addClassName("element-grayed");
	    imageElement.style.position = "absolute";
	  }
        }


	//var textToWrite = "<html><head><meta http-equiv='content-type' content='text/html; charset=UTF-8' /></head><body id='icontent'><div id='placeholder'>"+"<"+tag+" id='contents'>"+document.getElementById(name).innerHTML+"</"+tag+"></div></body></html>";
	var textToWrite = "<html><head><meta http-equiv='content-type' content='text/html; charset=UTF-8' /></head><body id='icontent'>"+origBox.innerHTML+"</body></html>";
        
	//myFrame.clear();
	myFrame.open();
	myFrame.write(textToWrite);
	myFrame.close();

	duplicateStyle(origBox, myFrame.getElementById('icontent'), myEl);
        $(name+'Edit').style.display = "block";
        $(name+'Edit').style.zIndex = 11;
        if(!Prototype.Browser.IE){
            var fs = $(origBox).getStyle('fontSize').replace('px', '');
            var lh = $(origBox).getStyle('lineHeight').replace('px', '');
            if(fs && lh){
                var newHeight = ''+(lh/fs);
                $(name+'Edit').style.lineHeight = newHeight;
                myFrame.getElementById('icontent').style.lineHeight = newHeight;
            }
        }

	// Set the width and margin for the boxes
        Element.setStyle(name+'Edit', {'height':(boxSize.height-2)+'px', 'width':(boxSize.width)+'px', 'border':'1px dashed #4455AA'} );
	if (!navigator.appVersion.match("MSIE")) {
          Element.setStyle(name+'Edit', {'maxWidth':'inherit'} );
	}
	var margins = [Element.getStyle(origBox, 'margin-top'), Element.getStyle(origBox, 'margin-bottom'), Element.getStyle(origBox, 'margin-left'), Element.getStyle(origBox, 'margin-right')];
	Element.setStyle(name+'Edit', {'margin-top':margins[0], 'margin-bottom':margins[1], 'margin-left':margins[2], 'margin-right':margins[3]} );

	if (imageElement) {
	  $(name+'Edit').style.minHeight = imageElement.getHeight()+parseInt(imageElement.style.marginBottom)+"px";
	}

	// Position the menu
	/**
	var menuPos  = new Position.cumulativeOffset(myEl);
	var topPos   = (menuPos[1]-160) - document.getElementById('icontent_container').offsetHeight;
        Element.setStyle('editMenu', {'display':'block', 'top':(menuPos[1]-135)+'px', 'left':(menuPos[0]+0)+'px'} );
	currentTop  = menuPos[1]-33;
	currentLeft = menuPos[0]+5;
	**/

	if (name != 'weebly_site_title' && editBoxShowOptions) {

	  // If element isn't a DIV, hide bulleted and numbered list buttons
	  if ($(name).tagName != "DIV") {
	    $('menuitem-ul').style.display = 'none';
	    $('menuitem-ol').style.display = 'none';
	    $('editMenuChild').style.width = '397px';
	  } else {
	    $('menuitem-ul').style.display = 'block';
	    $('menuitem-ol').style.display = 'block';
	    $('editMenuChild').style.width = '460px';
	  }

          hideFlashContainer();
          $('editMenu').style.display = "block";
	} else {
	  var par = $(name+'Edit').parentNode; 
	  if (!par.style.width) {
	    //par.style.width = "100%";
	    //par.style.display = "block";
	  }
	  //$(name+'Edit').style.width = ($(name+'Edit').style.width.replace(/px/, "")-(-25))+"px";
	  $(name+'Edit').style.minWidth = $(name+'Edit').style.width;
	  $(name+'Edit').contentWindow.document.body.focus();
	  //$(name+'Edit').style.styleFloat = "none";
	  //$(name+'Edit').style.clear = "both";
	}

        if ((textToWrite.indexOf('Click here to edit.') > -1 || textToWrite.indexOf('This is your new blog post.') > -1) && navigator.appVersion.indexOf("MSIE") > -1) {
          document.getElementById(name+'Edit').contentWindow.focus();
          try {
            selectAll();
            setTimeout('selectAll()', 500);
          } catch(e) { }
        }

	// Show the iframe and hide the previous element
        Element.setStyle(origBox, {'display':'none'} );

	if (myFrame.attachEvent) {
	  myFrame.attachEvent("onkeyup", function(e){self.keyUp(); return true;}, false);
	  myFrame.attachEvent("onkeydown", function(e){self.keyDown(); self.detectPaste(e); return true;}, false);
	}
    Event.observe(myFrame, 'keydown', fixIENewLines);

	keyUp();

	// Add keyboard shortcuts
	shortcut.add("Ctrl+B", function() { runCommand(currentBox, 'Bold', null); }, {target: myFrame });
	shortcut.add("Ctrl+U", function() { runCommand(currentBox, 'Underline', null); }, {target: myFrame });
	shortcut.add("Ctrl+I", function() { runCommand(currentBox, 'Italic', null); }, {target: myFrame });

	// We'll call these 500ms "recovery time"
	setTimeout("initializingBox = null;", 500);

}
        
function createIframe(nameOfBox) {

	if (!$(nameOfBox)) { return; }

	// Create the iframe element if it's not there
	if (!$(nameOfBox+'Edit')) {

	  iframeEl = document.createElement('iframe');

	  if (navigator.appVersion.indexOf("MSIE") == -1) {
	    //iframeEl.src = 'javascript:void(0)';
	  }

	  iframeEl.id = nameOfBox+'Edit';
	  iframeEl.className = 'editable';

	  iframeEl.style.display = 'block';
	  iframeEl.style.cssFloat = 'none';
	  iframeEl.style.styleFloat = 'none';
	  iframeEl.style.clear = 'both';

          iframeEl.allowTransparency = true ;
          iframeEl.frameBorder = '0' ;
          iframeEl.scrolling = 'no' ;

	  Element.insert($(nameOfBox), { after: iframeEl});
	}

}


function selectAll() {

	var name = currentBox;
        if (document.getElementById(name+'Edit') && document.getElementById(name+'Edit').contentWindow && document.getElementById(name+'Edit').contentWindow.document) {
          runCommand(currentBox, "selectall", false);
        }

}

function saveSelection() {

	if (document.selection) { 
	  currentSel = document.getElementById(currentBox+'Edit').contentWindow.document.selection.createRange();
	}

}

function displayColorBox() {

	if (colorBoxShown) {

	   var colors = ColorSelector.value('colorpicker');
	   colors     = colors.split(',');
	   var color  = '#' + toHex(colors[0]) + toHex(colors[1]) + toHex(colors[2]);

	   runCommand(currentBox, 'forecolor', color);
	   Element.setStyle('colorpicker', {'display':'none'});
	   colorBoxShown = 0;

	} else {

	   var topPos  = currentTop  + 35;
	   var leftPos = currentLeft + 195;

	   Element.setStyle('colorpicker', {'display':'block', 'top':topPos+'px', 'left':leftPos+'px'} );
	   colorBoxShown = 1;

	}

}

function showNewColorChooser(){
    var pos = Position.cumulativeOffset($('menuitem-cc'));
    $('new-color-chooser').setStyle({top:(pos.top+34)+'px', left:pos.left+'px'});
    newColorChooser.draw();
}
                                                                                                                     
function setColor() {
    var color = '#' + $('currentColor').value;
    runCommand(currentBox, 'forecolor', color);
}

function runCommand(name, command, options) {
            
	if (!document.getElementById(currentBox+'Edit') || !document.getElementById(currentBox+'Edit').contentWindow || !document.getElementById(currentBox+'Edit').contentWindow.document) { return; }
	var editableDocument = document.getElementById(currentBox+'Edit').contentWindow.document;
	var origElement      = document.getElementById(currentBox);

	if (currentSel.text) {
	  currentSel.select();
	  currentSel = '';
	}

	switch (command) {

	case "increasefontsize":
	  try {
	    //if (!editableDocument.queryCommandSupported(command)) 
	    //{
          	var currentSize = editableDocument.queryCommandValue("FontSize");
	      	if (!currentSize) 
	      	{
              editableDocument.execCommand("FontSize", false, 3);
	        } 
	        else if (typeof(currentSize) == "string") 
	        {
	          if( currentSize.match("px") )
	          {
                var newSize = pxToFontSize(parseInt(currentSize.replace("px", "")));
	          }
	          else
	          {
	            var newSize = parseInt(currentSize);
	          }
	          newSize = newSize > 6 ? 7 : newSize + 1;
              editableDocument.execCommand("FontSize", false, newSize);
	        } 
	        else 
	        {
              editableDocument.execCommand("FontSize", false, currentSize > 6 ? 7 : currentSize + 1);
	        }
	    //}
	  } catch(e) { editableDocument.execCommand(command, false, options); }
	  break;

        case "decreasefontsize":
          try {
            //if (!editableDocument.queryCommandSupported(command)) 
            //{
              var currentSize = editableDocument.queryCommandValue("FontSize");
              if (!currentSize) 
              {
                editableDocument.execCommand("FontSize", false, 1);
              } 
              else if (typeof(currentSize) == "string")
              { 
                  if( currentSize.match("px")) 
                  {
                	var newSize = pxToFontSize(parseInt(currentSize.replace("px", "")));
                  }
                  else
                  {
                    var newSize = parseInt(currentSize);
                  }
                  newSize = newSize > 1 ? newSize - 1 : 1;
                  editableDocument.execCommand("FontSize", false, newSize);
              } 
              else 
              {
                editableDocument.execCommand("FontSize", false, currentSize < 2 ? 1 : currentSize - 1);
              }
            //}
          } catch(e) { editableDocument.execCommand(command, false, options); }
          break;

	case "justifyleft":
	  currentAlign = "left";
	  editableDocument.getElementById('icontent').style.textAlign = "left";
	  origElement.style.textAlign = "left";
	  break;

        case "justifycenter":
	  currentAlign = "center";
	  editableDocument.getElementById('icontent').style.textAlign = "center";
	  origElement.style.textAlign = "center";
          break;

        case "justifyright":
	  currentAlign = "right";
	  editableDocument.getElementById('icontent').style.textAlign = "right";
	  origElement.style.textAlign = "right";
          break;

        case "justifyfull":
	  currentAlign = "justify";
	  editableDocument.getElementById('icontent').style.textAlign = "justify";
	  origElement.style.textAlign = "justify";
          break;

	default:
          editableDocument.execCommand(command, false, options);
          break;                                                                                                                      
	}
}


function detectPaste(e) {

       var key; var eventPassed;
                                                                                                                             
        if (e) {
           eventPassed = e;
        } else {
           eventPassed = event;
        }
                                                                                                                             
        if (eventPassed.ctrlKey && eventPassed.keyCode == 86) {
                   
	  isPaste = 1;

	}

	return true;

}

function pxToFontSize(currentSize) {

	var newSize = 3;

	if (currentSize < 13) {
          newSize = 1;
        } else if (currentSize >= 13 && currentSize < 16) {
          newSize = 2;
        } else if (currentSize >= 16 && currentSize < 18) {
          newSize = 3;
        } else if (currentSize >= 18 && currentSize < 24) {
          newSize = 4;
        } else if (currentSize >= 24 && currentSize < 32) {
          newSize = 5;
        } else if (currentSize >= 32) {
          newSize = 6;
        }

	return newSize;

}

function keyDown() {

}

function keyUp() {

	if (document.getElementById(currentBox+'Edit')) {
          var box              = document.getElementById(currentBox+'Edit');
          var editableDocument = box.contentWindow.document;
          //alert("keyUp. body Height " + editableDocument.body.offsetHeight + " " + editableDocument.body.clientHeight + " " + editableDocument.body.scrollHeight);

          var myHeight;
          /** if (navigator.appVersion.indexOf("MSIE") == -1) { myHeight = editableDocument.body.offsetHeight; } else { myHeight = editableDocument.body.scrollHeight; } **/
	
	  myHeight = box.contentWindow.document.body.scrollHeight;
	
	  myHeight = myHeight < 25 ? 25 : myHeight;

          Element.setStyle(box,{height: myHeight+'px'});

	  if (isPaste == 1) { isPaste = 0; clean(); }

	  // Auto-save edited text
	  if (!lastSave || (new Date().getTime() > lastSave + (20 * 1000))) {
	    lastSave = new Date().getTime();
	    if (currentBox != 'weebly_site_title') {
	      var text = editableDocument.getElementsByTagName('body')[0].innerHTML;
          if('function' === typeof(editBoxSaveCallback)){
            editBoxSaveCallback(currentBox, text, currentAlign);
          }
          else{
            saveContent(currentBox,text,currentAlign, 1);
          }
	    }

	  }
	}

	return true;

}

function fixIENewLines(event){
    if (Prototype.Browser.IE && event.keyCode == 13 && document.selection) {
        var sel = document.selection.createRange();
        sel.pasteHTML('<br /><span></span>');  //empty span needed to advance cursor to next line
        event.cancelBubble = true;
        event.returnValue = false;
    }
}

function clean() {

      var box  = document.getElementById(currentBox+'Edit');

      if (box) {
	var text = box.contentWindow.document.getElementsByTagName('body')[0].innerHTML;

	//console.log("BeforeClean! "+text);

	//Remove comments...
	text = text.replace(/<!--.*?-->/ig, "");

	//Replace <div> with <br /> in Safari
	if (navigator.appVersion.match("Safari/")) {
	  text = text.replace(/<div>/ig, "<br />");
	}

	//Replace <b></b> tags with <strong></strong>
	text = text.replace(/<b(\s+|>)/ig, "<strong$1");
	text = text.replace(/<\/b(\s+|>)/ig, "</strong$1");
	//Replace <i></i> tags with <em></em>
	text = text.replace(/<i(\s+|>)/ig, "<em$1");
	text = text.replace(/<\/i(\s+|>)/ig, "</em$1");
	//Replace <br> with <br />
	text = text.replace(/<br ?>/ig, "<br />");
    //Replace <br style='...'> with <br />
	text = text.replace(/<br style=[\'\"].*?[\'\"]>/ig, "<br />");
	//Replace <p> with <br />
	text = text.replace(/<\/p[^>]*>/ig, "<br /><br />");
	text = text.replace(/<p[^>]*>/ig, "");
    //Remove trailing new lines
    text = text.replace(/<br \/><br \/>$/, "");
	//Replace <li>'s with <br /> -- don't want to do that anymore!
	//text = text.replace(/<\/li\s*>/ig, "<br />");
	//Add a <br /> to pasted in newlines
	//text = text.replace(/\n/ig, "<br />\n");

	//Clean <font color=#123123> tags (add style and "")
	//text = text.replace(/<font (.*) color=(#[0-9a-f]{6})/ig, "<font $1 color='$2'");
	//Clean <font size=x> tags (add style and "")
        //text = text.replace(/<font (.*) size=([0-9]{1})/ig, "<font $1 size='$2'");

	//Remove empty tags
	text = text.replace(/(<[^\/]>|<[^\/][^>]*[^\/]>)(\s|<br \/>)*<\/[^>]*>/ig, "");
/*
	//Transform <span style=" attributes into their HTML tag equivalents
	text = text.replace(/<[^>]*>([^<]*)<\/[^>]*>/ig, function (match, content) {
					if (match.indexOf("span") == 1) {
					   var replaceWith = '';
					   match = match.replace(/style=["']([^"']*)["']/g, function(inside, style)
						   {
							if (style == "font-weight: bold;") { replaceWith = "<strong>"+content+"</strong>"; }
							else { replaceWith = content; }
							return inside;
						   });
					   return replaceWith;
					} else { return match; }
				    });
*/
	//Remove any disallowed tags
	text = text.replace(/<\/?([^>]*)>/ig, function(match, tag) {
					if (/^span/i.test(tag) || /^strong/i.test(tag) || /^u$/i.test(tag) || /^em/i.test(tag) || /^br \//i.test(tag) || /^big/i.test(tag) || /^small/i.test(tag) || /^a/i.test(tag) || /^font/i.test(tag) || /^ul/i.test(tag) || /^ol/i.test(tag) || /^li/i.test(tag)) {
						return match;
					} else { return ""; }

				      });

	//Remove any disallowed attributes
	text = text.replace(/<[^>]*>/g, function(match) 
				      {
					match = match.replace(/ ([^=]+)=["'][^"']*["']/g, function(inside, attribute)
						{
							if (/alt/i.test(attribute) || /href/i.test(attribute) || /target/i.test(attribute) || /title/i.test(attribute) || /style/i.test(attribute) || /size/i.test(attribute) || /color/i.test(attribute)) { return inside; } else { return ""; }
						});

					return match;

				      });

	// What was this supposed to do?
	//text = text.replace(/(.*)<br \/>$/g, '$1');


	box.contentWindow.document.getElementsByTagName('body')[0].innerHTML = text;
	//box.contentWindow.document.getElementById('contents').innerHTML = text;

	//console.log("AfterClean! "+text);

	return text;
      }

}

function toHex(d) {

	var hD="0123456789ABCDEF";
	var h = hD.substr(d&15,1);
	while(d>15) {d>>=4;h=hD.substr(d&15,1)+h;}
	if (h.length < 2) { h = "0"+h; }
	return h;

}
