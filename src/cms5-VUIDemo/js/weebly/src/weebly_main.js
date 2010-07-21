var previousItems     = 0;
var previousPages     = 0;
var currentSite	      = 0;
var siteType	      = null;
var currentPage       = 0;
var previousPage      = 0;
var recentDrag        = 0;
var currentElement    = 0;
var currentThemesPage = 0;
var imageUploadSize   = 0;
var userID            = 0;
var userIDLocation    = '';
var publishingWindow  = 0;
var publishingAnim    = 0;
var publishingLoc     = 'left';
var pageDblClick      = 0;
var uploadUpdater     = new Object();
var currentMenu       = '';
var oldSecondList     = '';
var oldPages	      = '';
var dontUpdateList    = 0;
var trashItem	      = '';
var deleting	      = 0;
var scriptSrc  	      = new Array();
var scriptId   	      = new Array();
var scriptType 	      = new Array();
var sitePages 	      = new Object();	// deprecated, use Weebly.PageManager.pages[].title
var originalSitePages = new Object();	// deprecated?
var pageOrder         = [];				// deprecated, use Weebly.PageManager.topLevelPages[].id
var tempUser	      = 0;
var tempDeleted	      = { };
var iFramesCache      = new Object();
var iFramesHeight     = new Object();
var originalPages     = '';
var documentWriteElement = 'body';
var currentThemesPerPage = 8;
var currentThemesCategory= '';
var validateFunction = function() { validateOK(); }; //saveProperties('properties','"+currentElement+"');};
var validateChangeFunction = function() {};
var headerSelected    = null;
var headerDimensions  = Array();
var uploadId	      = null;
var currentHeader     = null;
var resetScrollTop    = null;
var destroySecondList = null;
var interfaceActive   = null;
var editThemeMode     = null;
var updatedTheme      = null;
var friendRequests    = 0;
var adsenseID         = 0;
var currentImageNum   = Math.floor(Math.random()*10000000001);
var footerCodeShown   = false;
var newSitePassword   = "";
var hideTitle 	      = 0;
var purchaseReferer   = "";
var currentSiteLocation="";
var userEvents	      = {};
var forceFlashPosition = false;
Prototype.Browser.IE6 = Prototype.Browser.IE && parseInt(navigator.userAgent.substring(navigator.userAgent.indexOf("MSIE")+5))==6;

    function inEditor()
    {
	return true;
    }

    function setWidth() // actually sets both width and height in response to a user window resize
    {
        var width, doubleWidth, fullheight, height, textWidth;

	fullheight = getInnerHeight();        

        width= ((document.body.clientWidth - 600) / 2 ) -23;
	doubleWidth = width * 2 - 30;
        textWidth = (document.body.clientWidth - 600) / 2;
        //if (document.body.clientWidth < 750) document.location = "error.pl?resolution";
        
	Element.setStyle('emptyList', {left:(width+100)+'px'} );

	//Effect.Center('textEditor');
	//Element.setStyle('textEditor', {marginLeft:textWidth+'px'} );       

	height = fullheight - 133;
	
	Element.setStyle('scroll_container', {height: height+'px'} );
	Element.setStyle('scroll_container_properties', {height: height+'px'} );
	Element.setStyle('domainContainer', {height: (height+98)+'px'} );
	Element.setStyle('grayedOut', {height: height+'px'} );
	Element.setStyle('icontent_container', {'minHeight': height+'px'});
	Element.setStyle('newContainer', {height: (height)+'px'} );

	if( $('customThemeContainer' ) )
	{
		Element.setStyle('customThemeContainer', {height: (ENABLE_THEME_BROWSER?fullheight:height)+'px'} );
		Element.setStyle('themeEditBoxContainer', {height: ((ENABLE_THEME_BROWSER?fullheight:height)-36)+'px'} );
		if (typeof(themeEditBox) != "undefined") {
	          themeEditBox.style.height = $('themeEditBoxContainer').getHeight()+"px";
	          themeEditBox.style.width = $('themeEditBoxContainer').getWidth()+"px";
	          themeEditBox.style.border = "none";
		}
	}

	Element.setStyle('grayedOut', {height:(getInnerHeight()-35)+'px'} );
	Element.setStyle('grayedOutFull', {height:(getInnerHeight())+'px'} );
	Element.setStyle('helpFrame', {height:(getInnerHeight()-35)+'px'} );
	Element.setStyle('helpIframe', {height:(getInnerHeight()-55)+'px'} );
	Element.setStyle('pagesContainer', {height:(getInnerHeight()-35)+'px'} );

	Element.setStyle('menuBarDiv', {width:(document.body.clientWidth-27)+'px'} );

	if (siteType == 'myspace') {
	  Element.setStyle('elementlist', {width:(document.body.clientWidth)+'px'});
	} else {
	  Element.setStyle('elementlist', {width:(document.body.clientWidth-150)+'px'});
	}

	if (editThemeMode == 1) {
	  $('currentThemeOptions').style.width = (document.body.clientWidth-130)+"px";
          var moveTo = document.body.clientWidth <= 800 ? 215 : 305;
          $('themePictures').style.left = (document.body.clientWidth-moveTo)+"px";

	}
	
		if (window.resizeThemeBrowser) {
			window.resizeThemeBrowser(document.body.clientWidth, height, fullheight);
		}

    }

    function setHeight()
    {
        var height;
        height= (document.body.clientHeight - 100);

        Element.setStyle('initial_loading', {height:height+'px'} );

    }

    function setInterfaceEnabled() {
	
	interfaceActive = 1;
	Behaviour.apply();
	
    }

    function setInterfaceDisabled() {

	interfaceActive = null;
	initContentDraggables();

    }

    function handlerFuncMiddle(t) {
    
      Weebly.TimingTest.end(Weebly.TimingTest.updateListTest+"_ajax");
      var resText = t.responseText;

	//Clear out Sortables
	initContentDraggables();

        // Update icontent DIV's class name
        try {
          var pageLink = t.getHeader("Weebly-Page-Link");
          if (pageLink) {
            $('icontent').className = pageLink;
          }
        } catch (e) { }

	// Show AdSense setup if not configured
	if (resText.match(/serveAds\.php\?type=adsense/) && !adsenseID) {
	  Pages.go("adsenseSetup");
	}

        // Add "remove" button to footerCode span
        if ($('footerCode') && !$('footerCode').innerHTML.match("footer_remove.png") && Weebly.Restrictions.hasAccess("show_footer_removebutton")) {
          if (isPro()) {
            $('footerCode').innerHTML += "<a href='#' onclick='hideFooter(); return false;' style='border: none;'><img src='http://"+editorStatic+/*tli(*/"/weebly/images/footer_remove.png"/*)tli*/+"' style='position: relative; top: 3px; left: 6px; border: none;'/></a>";
          } else {
            $('footerCode').innerHTML += "<a href='#' onclick='alertProFeatures(\"Please sign-up for a pro account to remove the Weebly footer link\", \"main\"); return false;' style='border: none;'><img src='http://"+editorStatic+/*tli(*/"/weebly/images/footer_remove.png"/*)tli*/+"' style='position: relative; top: 3px; left: 6px; border: none;'/></a>";
          }
        }

	var imageGalleries = new Array(),
		_match;
	
	// v0.1 Image Galleries
	while (_match = resText.match(/<!WEEBLYGALLERY\-([^\-]*)\-([^\-]*)\-([^\-]*)\-(.*?)!>/)) {
		var updateHtml = Weebly.ImageGallery.update(_match[1], _match[2], _match[3], 0, 0, _match[4]);
		imageGalleries.push(_match[1]);
		resText = resText.safeReplace(/<!WEEBLYGALLERY\-.*?!>/, updateHtml);
	}
	
	// v0.2 Image Galleries
	while (_match = resText.match(/<!WEEBLYGALLERY2\-([^\-]*)\-([^\-]*)\-([^\-]*)\-([^\-]*)\-([^\-]*)\-(.*?)!>/)) {
		var updateHtml = Weebly.ImageGallery.update(_match[1], _match[2], _match[3], _match[4], _match[5], _match[6]);
		imageGalleries.push(_match[1]);
		resText = resText.safeReplace(/<!WEEBLYGALLERY2\-.*?!>/, updateHtml);
	}

    // Form Inputs
	while (_match = resText.match(/<!WEEBLYRADIO\-(.*?)\-(.*?)!>/)) {
		resText = resText.replace('<!WEEBLYRADIO-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawRadioOptions(_match[2]));
	}
    while (_match = resText.match(/<!WEEBLYSELECT\-(.*?)\-(.*?)!>/)) {
		resText = resText.replace('<!WEEBLYSELECT-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawSelectOptions(_match[2]));
	}
    while (_match = resText.match(/<!WEEBLYCHECKBOXES\-(.*?)\-(.*?)!>/)) {
		resText = resText.replace('<!WEEBLYCHECKBOXES-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawCheckboxes(_match[2]));
	}

	// Replace external account IDs
	var externalSites = 0;
        while ((_match = resText.match(/<!EXTERNAL:([^!]+)!>/)) && externalSites < 1000) {
          var remoteSite = _match[1];
          if (remoteAccounts[remoteSite]) {
            resText = resText.safeReplace(/<!EXTERNAL:[^!]+!>/, remoteAccounts[remoteSite].remoteId);
          }
	  externalSites++;
        }

	if (_match = resText.match(/<!EXTERNAL-SETUP:([^:]+):([^!]+)!>/)) {
	  Pages.go("externalSetup", _match[1], _match[2]);
	}

        scriptSrc = new Array();
        scriptId  = new Array();
        scriptType= new Array();
        var runScripts= 0;
	
	//Fanini - Replace new lines before text is output to page in design mode
	//resText = resText.replace(/\n/g,"");
	
        while (resText.indexOf('<weebly include_once') > -1 && scriptSrc.length < 30) {
          resText = resText.replace(/<weebly include_once(_noexport)? ([^>]*)>([\S\s]*?)<\/weebly>/im, '');
	  //alert("Pushing Id: '"+RegExp.$2+"', Src: '"+RegExp.$3+"'");
          scriptSrc.push(RegExp.$3);
          scriptId.push(RegExp.$2);
          runScripts = 1;
        }

        // Replace anything that starts with <weebly only_export
        resText = resText.replace(/<weebly only_export([^>]*)>([\S\s]*?)<\/weebly>/img, '');

        var secondList = $('secondlist');

        if (resText.match(/^%%EMPTY%%/)) {
           secondList.innerHTML = '<ul style="height: 380px; text-align: center; padding-top: 100px; font-size: 20px; color: #aaa;"> <span style="display: block; margin-bottom: 10px;">'+/*tl(*/'This page is empty. Drag Elements here.'/*)tl*/+'</span> <span style="font-size: 12px;"> ('+/*tl(*/'hint: Elements, like a Paragraph, are in the top bar'/*)tl*/+')</span></ul>';
           Element.setStyle('secondlist', {height:'380px'} );
           Element.show('emptyList');
        } else {
           secondList.innerHTML = resText.replace(/%%EMPTY%%/g, '');
           Element.setStyle('secondlist', {height:''} );
           Element.hide('emptyList');
        }

        if ( runScripts > 0 ) {
           try{ //webkit browsers fail here
               runIncludedScripts();
           }
           catch(e){}
        }

	makeIframesDraggable(secondList);

        finishFuncMiddle(resText);

    }

    function finishFuncMiddle(responseText) {
    
    	if (window.navFlyoutMenu) {
    		navFlyoutMenu.hideSubmenus();
    	}

	//console.log('finishFuncMiddle');
        if (responseText.match('<%%WEEBLY!disableSecondList!%%>')) {
	  // Editing a blog...
	  if (destroySecondList != 1) {
	    // First time on a blog page
	    elementsPage('blog');
	    currentBlog.editingBlog = 1;
	  }
	  destroySecondList = 1;

	  if ($('secondlist').innerHTML.match('%%HIGHLIGHT-TITLE%%')) {
	    new Effect.Highlight('blog-post-title',{duration: 2.0});
	  } else if ($('secondlist').innerHTML.match('%%HIGHLIGHT-DRAFTS%%')) {
	    new Effect.Highlight('blog_drafts',{duration: 2.0, startcolor:'#FFdd99'});
	  }

        } else if (siteType != 'myspace') {
	  if (destroySecondList == 1) {
	    // First time on a 'main' page
	    elementsPage('default');
	    currentBlog.editingBlog = 0;
	  }
	  destroySecondList = 0;
	}

	if (resetScrollTop) { resetScrollTop = null; $('scroll_container').scrollTop = 0; }
        var secondList = $('secondlist');

        if (responseText.match('<%%WEEBLY!disableSecondList!%%>')) {
          // Editing a blog...
	  if (currentBlog.skipToComments == 'comments' && $('comments')) {
	    var pos  = new Position.cumulativeOffset($('comments'));
	    $('scroll_container').scrollTop = (pos[1]-150);
	  }
	  if (currentBlog.showPostSettings) {
	    Effect.toggle('blogPostSettings', 'slide');
	    currentBlog.showPostSettings = 0;
	  }
	}
	
	updateActiveNavLink();
	disableFlyouts = false;
	
	if(!externalLibariesLoaded) {
		loadExternalLibraries();
	}

	setInterfaceEnabled();

        Weebly.TimingTest.end(Weebly.TimingTest.updateListTest);

        //Element.setStyle($('trashitem'), {height:$('container').clientHeight+30+'px'} );
        previousItems = secondList.getElementsByTagName('li').length;

	if (userEvents && (!userEvents.tab_themes) && userEvents.addElement && userEvents.addElement == 2) {
	  userEvents.addElement = 0;
	  showEvent('addElement', 1);
	}


    }
    
    
    
    
    
    
    function updateActiveNavLink(skipFlyoutUpdate) {

		if (typeof(currentThemeDefinition['menuActive']) != 'undefined' && currentThemeDefinition['menuActive'].strip()) {

			if (!skipFlyoutUpdate && window.navFlyoutMenu) {
				var itemHandle = $('pg'+currentPage);
				if (itemHandle) navFlyoutMenu.removeItem(itemHandle);
			}

			var thisLink = currentThemeDefinition['menuActive'];
			thisLink = thisLink.safeReplace(/%%MENUITEMACTIVETITLE%%/i, sitePages[currentPage]);
			thisLink = thisLink.replace(/%%MENUITEMACTIVELINK%%/i, "#");
			thisLink = thisLink.safeReplace(/<a/i, '<a onclick="'+"if (notBeenDragged()) { noJump = 1; goUpdateList('"+currentPage+"', 1); }"+'; return false;"');
			thisLink = thisLink.safeReplace(/<\/a>/i, '</a>' + Weebly.PageManager.subpagesHTML(currentPage));
			if($('pg'+currentPage) !== null) {
				$('pg'+currentPage).innerHTML = thisLink;
			}

			if (!skipFlyoutUpdate && window.navFlyoutMenu) {
				var itemHandle = $('pg'+currentPage);
				if (itemHandle) navFlyoutMenu.addItem(itemHandle);
			}

			if (previousPage != currentPage) {

				if (!skipFlyoutUpdate && window.navFlyoutMenu) {
					var itemHandle = $('pg'+previousPage);
					if (itemHandle) navFlyoutMenu.removeItem(itemHandle);
				}

				var thisLink = currentThemeDefinition['menuRegular'];
				thisLink = thisLink.safeReplace(/%%MENUITEMTITLE%%/i, sitePages[previousPage]);
				thisLink = thisLink.replace(/%%MENUITEMLINK%%/i, "#");
				thisLink = thisLink.safeReplace(/<a/i, '<a onclick="'+"if (notBeenDragged()) { noJump = 1; goUpdateList('"+previousPage+"', 1); }"+'; return false;"');
				thisLink = thisLink.safeReplace(/<\/a>/i, '</a>' + Weebly.PageManager.subpagesHTML(previousPage));
				if($('pg'+previousPage) !== null) {
					$('pg'+previousPage).innerHTML = thisLink;
				}

				if (!skipFlyoutUpdate && window.navFlyoutMenu) {
					var itemHandle = $('pg'+previousPage);
					if (itemHandle) navFlyoutMenu.addItem(itemHandle);
				}

			}
			
			if (window.navFlyoutMenu) {
				// update submenu 'current' class
				var oldLI = $('weebly-nav-' + previousPage);
				if (oldLI) {
					oldLI.removeClassName('weebly-nav-current');
				}
				var newLI = $('weebly-nav-' + currentPage);
				if (newLI) {
					newLI.addClassName('weebly-nav-current');
				}
			}

			if (!skipFlyoutUpdate && window.navFlyoutMenu) {
				refreshNavCondense();
			}
		}
    }
    
    
    
    
    
    

    function makeIframesDraggable(el) {

        // Steal all iframe's mouse events
        /*
 	// Stop stealing the mouse events, no longer needed with DIV overlays
 	
        var iFrames = el.getElementsByTagName('iframe');
        for (x=0; x < iFrames.length; x++) {

          // If the iFrame doesn't have an ID, give it one
          if (iFrames[x].id == "") {
            iFrames[x].id = Math.floor(Math.random()*100000001);
          }

          tryStealMouse(iFrames[x].id);
        }
	*/

    }

    function tryStealMouse(iFrame) {

	var iFrameEl = document.getElementById(iFrame);

	if (iFrameEl && iFrameEl.contentWindow && iFrameEl.contentWindow.document && iFrameEl.contentWindow.document.body && iFrameEl.contentWindow.document.body.innerHTML && iFrameEl.contentWindow.document.body.innerHTML != "") {
	  //alert(iFrameEl.contentWindow.document.body.innerHTML);
	  $.StealMouse.on(iFrameEl);
	} else {
	  setTimeout("tryStealMouse('"+iFrame+"');", 250);
	}

    }

    function runIncludedScripts() {

        thisScriptId = scriptId.pop();
        thisScriptSrc = scriptSrc.pop();

        thisScriptIdOnly = thisScriptId.replace(/scriptInclude/,'');

        //alert("runIncludedScripts: "+thisScriptId);

        var myFrame = document.getElementById(thisScriptId).contentWindow.document;
        var headScript = '';
        if (thisScriptSrc.indexOf('<%HEAD%>') > -1) {
          var splitElements = new Array();
          splitElements = thisScriptSrc.split('<%HEAD%>');
          thisScriptSrc = splitElements[1];
          headScript    = splitElements[0];
        }
	var bodyScript = '';
	if (thisScriptSrc.indexOf('<%BODY%>') > -1) {
	  var splitElements = new Array();
	  splitElements = thisScriptSrc.split('<%BODY%>');
	  thisScriptSrc = splitElements[1];
	  bodyScript    = splitElements[0];
	}

        var toWrite = '<html><head> ' + headScript + ' </head><body style="margin: 0px; padding: 0px; background-color: transparent;" onload="parent.resizeMe(); ' + bodyScript + '" onClick="parent.clickHandler(event, \''+thisScriptIdOnly+'\');"> '+ thisScriptSrc +' <script type="text/javascript"> parent.resizeMe("'+thisScriptId+'"); parent.setInterval("resizeMe('+"'"+thisScriptId+"'"+');", 250); </script></body></html>';

        //alert("Writing to '"+thisScriptId+"': '"+toWrite+"', "+scriptSrc.length+" scripts left.");

        /**
        if (navigator.appVersion.indexOf("Mozilla") > -1) {
          myFrame.write('<html><body style="margin: 0px; padding: 0px; background-color: transparent;"> '+ thisScriptSrc +'</body></html>');
          document.getElementById(thisScriptId).contentWindow.document.addEventListener("DOMContentLoaded", function(e){alert('asdf'); resizeMe(thisScriptId); return true;}, false);
        } else {
        **/
        //}
          myFrame.write(toWrite);

	  // Create transparent DIV on top of the iframe to allow smoother scrolling and clicking
	  if (!$(thisScriptId+"-coverbox")) {
	    var box = document.getElementById(thisScriptId);
	    var newDiv = document.createElement("DIV");
	    newDiv.id = thisScriptId+"-coverbox";
	    newDiv.style.position = 'absolute';
	    newDiv.style.overflow = 'hidden';
	    newDiv.style.background = 'url("http://'+editorStatic+'/weebly/images/clear.gif")';
	    newDiv.style.top = '17px';
	    newDiv.style.left = '0px';
	    newDiv.style.height = Element.getStyle(box, 'height');
	    newDiv.style.width = Element.getStyle(box, 'width');
	    //newDiv.style.background = 'yellow';
	    box.parentNode.appendChild(newDiv);
	  }

	  if (scriptId.length > 0) { runIncludedScripts(); }

    }

    function resizeMe(scriptId) {

	//console.log("resizeMe: "+scriptId);
        //var myFrame = document.getElementById(thisScriptId).contentWindow.document;
        //myFrame.close();

	if ((!tempDeleted.item || (tempDeleted.item && scriptId != tempDeleted.item.id)) && document.getElementById(scriptId)) {

          var box              = document.getElementById(scriptId);
          var editableDocument = box.contentWindow.document;

          var myHeight;
          if (navigator.appVersion.indexOf("MSIE") == -1) {  
	    myHeight = editableDocument.body.offsetHeight; 
	  } else { 
	    myHeight = editableDocument.body.scrollHeight; 
	  }
	  //myHeight = box.contentWindow.innerHeight;
	  if (myHeight != Element.getStyle(box, 'height').replace(/px/, '')) {
            Element.setStyle(box,{height: myHeight+'px'});
	    if ($(scriptId+"-coverbox")) {
              Element.setStyle($(scriptId+"-coverbox"),{height: myHeight+'px'});
	    }
	    $$("#secondlist .columnlist").each(resizeColumns);
	  }

          //var coverBox       = document.getElementById(scriptId.replace(/scriptInclude/, "scriptCover"));
          //Element.setStyle(coverBox,{height: myHeight+'px', top: ((400-myHeight)-400)+'px'});

          //alert("Resizing "+scriptId.replace(/scriptInclude/, "scriptCover")+" to "+myHeight+", top: "+((400-myHeight)-400));

	}

    }

    function resizeColumns(el) {

	// Make sure both columns are same height
        var oppEl = '';
        if (el.id.match(/lhs-list/)) {
          oppEl = el.id.replace(/lhs/, "rhs");

	  if ($(oppEl)) { 
	    el.style.height = 'auto';
	    $(oppEl).style.height = 'auto';
	    var elHeight = $(el).getHeight();
	    var oppElHeight = $(oppEl).getHeight();

	    if (elHeight < 100 && oppElHeight < 100) {
	      el.style.height = '100px';
	      $(oppEl).style.height = '100px';
	    } else if (oppElHeight >= elHeight) {
              el.style.height = oppElHeight + "px";
            } else {
              $(oppEl).style.height = elHeight + "px";
	    }
	  }
	}

    }

    function cacheIframe(el) {

        var iFrames = el.getElementsByTagName('iframe');
        for (x=0; x < iFrames.length; x++) {

	  if (iFrames[x] && iFrames[x].contentWindow && iFrames[x].contentWindow.document && iFrames[x].contentWindow.document.body) {
            iFramesCache[iFrames[x].id] = iFrames[x].contentWindow.document.body.innerHTML;
            iFramesHeight[iFrames[x].id] = Element.getStyle(iFrames[x], 'height');
          }

        }

    }

    function smokeOutIframe(el) {

        var iFrames = el.getElementsByTagName('iframe');
        for (x=0; x < iFrames.length; x++) {

	  if (iFramesCache[iFrames[x].id]) {
	    //iFrames[x].contentWindow.document.body.innerHTML = iFramesCache[iFrames[x].id];
	    //iFrames[x].style.height = iFramesHeight[iFrames[x].id];
	  }
          tryStealMouse(iFrames[x].id);
        }

    }

    dontRemoveElement = null;
    function removeElement(elementId) {

	if (dontRemoveElement) {
	  dontRemoveElement = null;
	  return;
	}

	if ($('inside_'+elementId)) {

	  $('inside_'+elementId).parentNode.removeChild($('inside_'+elementId));
	  new Ajax.Request(ajax, {parameters:'pos=deletepageelement&pei='+elementId+'&cookie='+document.cookie, onSuccess:handlerFuncDeletePageElement, onFailure:errFunc, onException: exceptionFunc, asynchronous: false});
	  updateList();

	}

    }

    function submitExternal(type) {

        if ($('lightbox_spinner').style.display == "inline") return;
	
        $(type+'ErrorDiv').style.display = 'none';
        $('lightbox_submitbtn').style.opacity = 0.5;
        $('lightbox_submitbtn').style.filter = 'alpha(opacity=50)';
        $('lightbox_spinner').style.display = 'inline';
        new Ajax.Request(ajax, {parameters: 'pos=externalaccount&type='+type+'&'+Form.serialize(type+'_form')+'&cookie='+document.cookie, 'onSuccess': function(t) { handlerSubmitExternal(t, type); }, 'onFailure':errFunc, bgRequest: true});
    }

    function handlerSubmitExternal(t, type) {

        if (t.responseText.match("SUCCESS")) {

          var externalID = t.responseText.replace("SUCCESS:", "");
	  remoteAccounts[type] = { remoteSite: type, remoteId: externalID };

	  dontRemoveElement = 1;
	  updateList();
	  Pages.go('main');

          $(type+'ErrorDiv').style.display = 'none';
          $('lightbox_submitbtn').style.opacity = 1;
          $('lightbox_submitbtn').style.filter = 'alpha(opacity=100)';
          $('lightbox_spinner').style.display = 'none';

        } else {

          if (!t.responseText.match("ERROR")) { t.responseText = /*tl(*/"ERROR: Temporary error. Please try again."/*)tl*/; }
          $(type+'ErrorDiv').innerHTML = t.responseText;
          $(type+'ErrorDiv').style.display = 'block';
          $(type+'ErrorDiv').setStyle({'color':'#FF0000'});
          $('lightbox_submitbtn').style.opacity = 1;
          $('lightbox_submitbtn').style.filter = 'alpha(opacity=100)';
          $('lightbox_spinner').style.display = 'none';
        }

    }

    function createMarker(element) {

	//removeMarker();

	if (element.parentNode.id == "elementlist") { return; }
  if (element.parentNode.id == "pages") { return; }
  if (element.parentNode.id == "inputoptions-list") { return; }
	if (element.parentNode.id.match(/\-gallery$/)) { return; }
	//console.log(element);
	var sortableMarker = '';
	if ($('sortableMarker')) {
	  sortableMarker = $('sortableMarker');
	} else {
          sortableMarker = document.createElement('DIV');
          sortableMarker.id = 'sortableMarker';
	  sortableMarker.className = 'sortableMarker';
          sortableMarker.style.position = 'absolute';
	  sortableMarker.style.zIndex = '100';
          $('scroll_container').insertBefore(sortableMarker, $('scroll_container').firstChild);
	}
	if (element.previousSibling) {
          sortableMarker.style.left = Position.cumulativeOffset(element.previousSibling)[0]+"px";
          sortableMarker.style.top = (Position.cumulativeOffset(element.previousSibling)[1]-125-(-Element.getStyle(element.previousSibling, 'height').replace(/px/, '')))+"px";
	} else {
	  sortableMarker.style.left = Position.cumulativeOffset(element.parentNode)[0]+"px";
          sortableMarker.style.top = (Position.cumulativeOffset(element.parentNode)[1]-131)+"px";
	}
	if (element.previousSibling) {
          sortableMarker.style.width = Element.getStyle(element.previousSibling, 'width');
          sortableMarker.style.height = Element.getStyle(element, 'height');
	} else if (element.nextSibling) {
          sortableMarker.style.width = Element.getStyle(element.nextSibling, 'width');
          sortableMarker.style.height = Element.getStyle(element, 'height');
	} else {
          sortableMarker.style.width = Element.getStyle(element, 'width');
          sortableMarker.style.height = Element.getStyle(element, 'height');
	}

    }

    function removeMarker() {

	if ($('sortableMarker')) {
          $('sortableMarker').parentNode.removeChild($('sortableMarker'));
        }

    }

    function controlDrop(element, dropon) {

      if(Element.hasClassName(element, 'controlledDrop')) {
        // David -- Controlled drop code
        // Allow elements to be dragged into containers by class names set on the element and container
        //console.log("onHover -- dropon.id: "+dropon.id+", element.className: "+element.className);
	//console.log(element);
        if ((dropon.id == "myspaceListRight" || (dropon.id.match(/^inside_/) && dropon.parentNode.id == "myspaceListRight")) && !Element.hasClassName(element, 'myspaceApplication')) {
          return false;
        } else if((dropon.id == "myspaceListRight" || (dropon.id.match(/^inside_/) && (dropon.parentNode.id == "myspaceAboutMeList" || dropon.parentNode.id == "myspaceLikeToMeetList"))) && !Element.hasClassName(element, 'myspaceAboutMe')) {
          return false;
        } else if(Element.hasClassName(element, 'myspaceApplication')) { // && (dropon.id.match(/^inside_/) && dropon.parentNode.id == "myspaceListRight")) {

	  if (nextSiblingCount(dropon) < 2) {
	    return false;
	  }
	}
      }

      return true;

    }

    function controlDrag(element) {

      //console.log(element);
      if(Element.hasClassName(element, 'controlledDrag')) {
        // David -- Controlled drag code
        // Allow elements to be draggable or not by the class name of the handle
        if (element && element.childNodes[1] && element.childNodes[1].firstChild && (Element.hasClassName(element.childNodes[1].firstChild, 'handlebar_right_nodrag') || Element.hasClassName(element.childNodes[1].firstChild, 'handlebar_left_nodrag'))) {
          return false;
        }
      }

      return true;

    }

    function nextSiblingCount(element, count) {

	element = $(element);
	if (!count) { count = 0; }

	if (element.nextSibling) {
	  count = nextSiblingCount(element.nextSibling, count+1);
	}

	return count;

    }

    function createIncludedScripts(scriptText) {

        //alert("createIncludedScripts start "+scriptText);

        var script = document.createElement('script');

        script.type = 'text/javascript';
        script.text = scriptText;
        script.id   = thisScriptId+"JS";

        if (scriptSrc.length > 0) { script.text += " runIncludedScripts();"; }
        else { script.text += " finishFuncMiddle();"; }

        documentWriteElement = thisScriptId;

        $(thisScriptId).appendChild(script);

    }

    function updateElements() {

	selectCategory(currentMenu);

    }

    function isFormElement(pe){
        var form = $(pe).down('.formlist');
        if(form){
            var columns = $(pe).down('.columnlist');
            if(!columns){
                return true;
            }
            else{
                return form.ancestors().size() < columns.ancestors().size();
            }
        }
        return false;
    }

    function deleteMe(myElement) {

	if( !containsElements(myElement ) || isFormElement(myElement) )
	{
        if(currentBox && $(currentBox)) hideEditBox(currentBox);
		tempDeleted.item = myElement;
		tempDeleted.myParent = myElement.parentNode;
		if (myElement.nextSibling) {
		  tempDeleted.type = 'nextSibling';
		  tempDeleted.refNode = myElement.nextSibling;
		} else {
		  tempDeleted.type = 'append';
		  tempDeleted.refNode = myElement.parentNode;
		}
	
		initContentDraggables();
		myElement.parentNode.removeChild(myElement);
	
		confirmDelete();
	}
	else
	{
		showWarning( /*tl(*/'Please remove all elements inside this element before deleting it.'/*)tl*/, myElement );
	}

    }
    
    //returns true if another element is contained inside this element
    function containsElements(element)
    {
        var children = $(element).childElements();
        for( var i = 0; i < children.length; i++ )
        {
            if( children[i].match( 'li.inside' ) || containsElements(children[i]) )
            {
                return true;
            }
        }
        return false;
    }

    function updateTrash() {

	//dontUpdateList = 1;
	trashItem = $('trashlist').innerHTML;
    	$('trashlist').innerHTML = '<li id="trashitem" class="outside" style="height: 75px;"></li>';
	confirmDelete();

    }

    function confirmDelete() {

	var height = getScrollTop() + 205;
        //$('typeOfEl1').innerHTML = 'element';
        //$('typeOfEl2').innerHTML = 'element';
        //$('typeOfEl3').innerHTML = 'element';
	Element.setStyle('deleteConfirmation', {top:height+'px'} );
	Element.show('deleteConfirmation');	

	//var testme = confirm("Do you want to delete this item?");
        //if (!testme) { $('secondlist').innerHTML = oldSecondList; Behaviour.apply(); }

    }

    function unDeleteElement() {

	if ( trashItem.match(/notBeenDragged/) ) {
	  //$('pages').innerHTML = oldPages;
	  //updatePages();
	} else {
	  if( tempDeleted.type == "append") {
	    tempDeleted.refNode.appendChild(tempDeleted.item);
	  } else {
	    tempDeleted.refNode.parentNode.insertBefore(tempDeleted.item, tempDeleted.refNode);
	  }
	}
	Behaviour.apply();

    }

    function goDeleteElement() {

	deleting = 1;
	if (tempDeleted.myParent.parentNode.parentNode.id.match(/-blog/)) {
	  // Delete separately
	  new Ajax.Request(ajax, {parameters:'pos=deleteblogelement&pei='+tempDeleted.item.id.replace('inside_', '')+'&cookie='+document.cookie, onSuccess:handlerFuncDeleteElement, onFailure:errFunc, onException: exceptionFunc});
	} else {
          if ( trashItem.match(/notBeenDragged/) ) {
            //updatePages(); // should this be uncommented??? what is its use??? was uncommented before PageManager refactoring ~ashaw
          } else if(isFormElement(tempDeleted.item)) {
            new Ajax.Request(ajax, {parameters:'pos=deleteformpageelement&pei='+tempDeleted.item.id.replace('inside_', '')+'&cookie='+document.cookie, onSuccess:handlerFuncDeletePageElement, onFailure:errFunc, onException: exceptionFunc});
          } else {
            //updateList();
            new Ajax.Request(ajax, {parameters:'pos=deletepageelement&pei='+tempDeleted.item.id.replace('inside_', '')+'&cookie='+document.cookie, onSuccess:handlerFuncDeletePageElement, onFailure:errFunc, onException: exceptionFunc});
          }
	}

	Element.hide('deleteConfirmation');

    }
    
    function handlerFuncDeletePageElement(t) {
        if(t.responseText.indexOf('ERROR') > -1)
        {
            unDeleteElement()
            showError(t.responseText, tempDeleted);
        }
        else
        {
			updateList();
        }
    }

    function handlerFuncDeleteElement(t) {
	updateList();
    }

    var skipDisableInterface = 0;
    function updateList(pageid, newPost)
    {
      
        if (!firstLoadMiddle) {
          Weebly.TimingTest.doUpdateListTest = Math.floor(Math.random()*10) == 9;
          if (Weebly.TimingTest.doUpdateListTest) {
            Weebly.TimingTest.updateListTest = deleting ? 'delete_element' : 'add_element';
            Weebly.TimingTest.start(Weebly.TimingTest.updateListTest);
          }
        }
	//console.log('updateList. pageID: '+pageid+' newpost: '+newPost+' postId: '+currentBlog.postId);
        //new Ajax.Request(ajax, {parameters:'pos=left&cookie='+document.cookie, onSuccess:handlerFuncLeft, onFailure:errFunc});
        
	//Disable dragging of elements while page loads
	if (skipDisableInterface) {
	  skipDisableInterface = 0;
	} else {
	  setInterfaceDisabled();
	}

	if (currentBlog.postId > 0) {
	  if ($('blog-post-categories')) { currentBlog.categories = $('blog-post-categories').value; }
	  if ($('blog-post-title')) { currentBlog.title = $('blog-post-title').value; }
	}
	if (typeof(newPost) == "undefined") { newPost = 0; }

	if (currentBox) hideEditBox(currentBox);
	Weebly.Elements.unselectElement();

	if (deleting) { deleting = 0; }
	if (dontUpdateList) { dontUpdateList = 0; }
    else {
        if (pageid > 0) {
            previousPage = currentPage;
            currentPage = pageid;
            if (firstLoadMiddle) {
                Weebly.Cache.get('', 'initialMiddle', function(response){
                    var t = {};
                    t.responseText = response.replace(/%%QUOTE%%/g, '"').replace(/%%NEWLINE%%/g, '\n');
                    t.getHeader = function(h){
                        if (h == "Weebly-Page-Link") {
                            return "index";
                        }
                        return false;
                    };
                    handlerFuncMiddle(t);
                    $('pleaseWait').style.display = 'none';
                });
                firstLoadMiddle = false;
            }
            else {
                if (Weebly.TimingTest.doUpdateListTest) { Weebly.TimingTest.start(Weebly.TimingTest.updateListTest+"_ajax"); }
                new Ajax.Request(ajax, {
                    parameters: 'pos=middle&pageid=' + pageid + '&blogPost=' + currentBlog.postId + '&newPost=' + newPost + '&categories=' + currentBlog.categories + '&title=' + currentBlog.title + '&cookie=' + document.cookie,
                    onSuccess: handlerFuncMiddle,
                    onFailure: errFunc,
                    onException: exceptionFunc
                });
            }
        }
        else {
            var listItems = $$('#secondlist .inside', '#secondlist .outside_top');
            var elements = [];
            var newElements = 0;
            var i = 0;
            listItems.each(function(el){
                var form = el.down('form');
                if (form && form.idfield) {
                    elements[i] = {};
                    elements[i].new_element = form.idfield.value.match('def:') ? true : false;
                    if (!elements[i].new_element) {
                        var temp = form.idfield.value.evalJSON();
                        elements[i].page_element_id = temp.id;
                        elements[i].element_id = temp.eid;
                    }
                    else {
                        var temp = form.idfield.value.split('|');
                        elements[i].element_id = temp[0].replace(/[^\d]/g, '');
                        if (temp[1]) {
                            elements[i].properties = temp[1].evalJSON();
                        }
                        newElements++;
                    }
                    if (el.parentNode.parentNode.parentNode.className.match("column")) {
                        elements[i].parent = el.parentNode.parentNode.parentNode.id;
                    }
                    else 
                        if (el.up('.formlist')) {
                            elements[i].parent = el.up('.formlist').id;
                        }
                    
                    // If an element's never been added before, hook here to throw the event later
                    if (userEvents && !userEvents.addElement && elements[i].new_element) {
                        userEvents.addElement = 2;
                        fireTrackingEvent("AddElement", elements[i].element_id);
                    }
                    i++;
                }
            });
            pageid = typeof(pageid) != "string" ? currentPage : pageid;
            var params = {
                pos: 'middle',
                pageid: pageid,
                elements: elements.toJSON(),
                blogPost: currentBlog.postId,
                newPost: newPost,
                categories: currentBlog.categories,
                title: currentBlog.title,
                cookie: document.cookie 
            }
            //Handle WebKit multiple new elements bug by refreshing content from db
            if(newElements > 1){
                logExcessElements();
                setElementsPageType();
                updateList(currentPage);
            }
            else{
                if (Weebly.TimingTest.doUpdateListTest) { Weebly.TimingTest.start(Weebly.TimingTest.updateListTest+"_ajax"); }
                new Ajax.Request(ajax, {parameters:params, onSuccess:handlerFuncMiddle, onFailure:errFunc, onException: exceptionFunc, bgRequest: true});
            }        
        }
	}
        
	//console.log("endUpdateList");
        
    }
    
    function logExcessElements(){
        var history = Pages.history || [];
        var params = {
            pos: 'logexcesselements',
            pageid: currentPage,
            secondlist: $('secondlist').innerHTML,
            elementlist: $('elementlist').innerHTML,
            history: history.join(', '),
            cookie: document.cookie 
        }
        new Ajax.Request(ajax, {parameters:params, bgRequest: true});
    }
   
    function setDrag() {
	recentDrag = 1;
    }

    function notBeenDragged() {
      if (navigator.appVersion.indexOf("MSIE") == -1) {
	if (recentDrag == 1) {
	   recentDrag = 0;
	   return false;
	} else {
	   return true;
	}
      } else { return true; }
    }

    function saveContent(id,saveContent,alignment, dontRefresh) {

        //console.log("Saving content: "+saveContent);

        var encodedHtml;         
	encodedHtml = encodeURIComponent(saveContent);         
	encodedHtml = encodedHtml.replace(/\//g,"%2F");
        encodedHtml = encodedHtml.replace(/\?/g,"%3F");
        encodedHtml = encodedHtml.replace(/=/g,"%3D");
        encodedHtml = encodedHtml.replace(/&/g,"%26");
        encodedHtml = encodedHtml.replace(/@/g,"%40"); 

	// Replace linefeed (%0A) with space (%20)
	encodedHtml = encodedHtml.replace(/%0A/g, "%20");

        new Ajax.Request(ajax, {parameters:'pos=content&reqid='+id+'&content='+encodedHtml+'&align='+alignment+'&cookie='+document.cookie, onSuccess:function(t) { handlerFuncSaveContent(t, dontRefresh); }, onFailure:errFunc, bgRequest: dontRefresh});

    } 

    function handlerDragEndSplitpane(splitPane, event, cfpids, elId) {

	// Grab serialized parameters into array 'widths'
	var width = splitPane.serialize();
	var widthArray = width.split(/&/);
	var widths = new Array();

	for (var x=0; x < widthArray.length; x++) {
	  var v = widthArray[x].split(/=/);
	  widths[v[0]] = v[1];
	}

	var cfps = cfpids.split(/-/);
	var params = new Array();

	for (var x=0; x < cfps.length; x++) {
	  params.push(cfps[x]+'-'+widths['div'+(x+1)+'_width']);
	}

	var pString = params.join(":");

	//console.log(elId);
	//console.log(pString);
	new Ajax.Request(ajax, {parameters:'pos=savecolumn&elementid='+elId+'&width='+pString+'&cookie='+document.cookie, onSuccess:handlerFuncDragEndSplitpane, onFailure:errFunc, bgRequest: true});

    }

    function handlerFuncDragEndSplitpane(t) {

    }

    function flashMouseUpHandler(e, obj) {

    	if ( document.createEvent ) {
	  var evObj = document.createEvent('MouseEvents');
 	  evObj.initMouseEvent( 'mouseup', true, false, window, e.detail, e.screenX,e.screenY, e.clientX, e.clientY, e.ctrlKey, e.altKey,e.shiftKey, e.metaKey, e.button, null );
    	  obj.parentNode.dispatchEvent(evObj);
    	}
	// We don't need to dispatch these events for IE (since the bug doesn't exist on IE!)

    }

    function twoColumnDepth(el){
        var depth = 0;
        var parent = el.up('.weebly-splitpane-2');
        while(parent){
            depth++;
            parent = parent.up('.weebly-splitpane-2');
        }
        return depth;
    }

    function pushTwoColumnsDraggables(){
        if(Weebly.Elements.highlightedElement){
            var els = $$('#secondlist .formlist .columnlist', '#secondlist .formlist');
        }
        else{
            var els = Sizzle.matches( ':not(#secondlist .formlist .columnlist)', $$('#secondlist .columnlist') )
        }
        var depths = els.collect(function(el){
            return {'element': el, 'depth': el.ancestors().size()};
        });
        depths.sort(function(a, b){return b.depth - a.depth});
        depths.each(function(obj){
           contentDraggables.push(obj.element);
        });
    }


	//
	// initDraggables SUCKS because Draggables.addObserver isn't working (element property is disregarded)
	// causing the observer to be fired every time ANY draggable finishes
	// ~ashaw
	//
	// UPDATE: reorderDropped() still gets called globally on drops, but now it only gets called ONCE
	//
    function initDraggables() {

	if (!interfaceActive) { return false; }

	Position.includeScrollOffsets = true;
	//console.log("initDraggables!");

	for(var x=0; x < contentDraggables.length; x++) {

	  //console.log(contentDraggables[x]);
	  if (contentDraggables[x].id == "elementlist") {
	  
	    Sortable.create("elementlist", {
	    	dropOnEmpty:true,
	    	constraint:false,
	    	scroll:$('scroll_container'),
	    	scrollSensitivity:100,
	    	scrollSpeed:5,
	    	onUpdate:updateElements
	    });
	    
        Draggables.addObserver({element:$('elementlist'), onStart:beginAddElement});
	  } else if(contentDraggables[x].id != "myspaceListRight" && contentDraggables[x].id != "myspaceListLeft") {
	    //console.log("not elementlist");
	    if ((contentDraggables[x].id == "secondlist" && destroySecondList != 1) || contentDraggables[x].id != "secondlist") {
	    
	      //console.log("Sortable.create("+contentDraggables[x]+", ");
	      //Sortable.create(contentDraggables[x], {hoverclass: 'hoverclass', dropOnEmpty:true,handle:'handle',scroll:'scroll_container',scrollSensitivity:50,scrollSpeed:5,containment:contentDraggables,constraint:false,onUpdate:singleUpdateList });
	      
	      Sortable.create(contentDraggables[x], {
	      	hoverclass: 'hoverclass',
	      	dropOnEmpty:true,
	      	scroll:$('scroll_container'),
	      	scrollSensitivity:50,
	      	scrollSpeed:5,
	      	containment:contentDraggables,
	      	constraint:false
	      });

	      // Manually provide DOM events for flash objects,
	      // since Mac FF flash client seems to eat them
	      var obj = $$("#"+contentDraggables[x].id+" object");
	      if (obj && obj[0] && obj[0].tagName) {
		Event.stopObserving(obj[0], 'mouseup');
		Event.observe(obj[0], 'mouseup', function(e) { flashMouseUpHandler(e, obj[0]); });
	      }
	    }
	    if (contentDraggables[x].id.match(/lhs-list/) && userEvents.addElement && userEvents.addElement != 2) {
		//console.log("twoColumn "+userEvents.addElement);
	      showEvent("twoColumn", 0, contentDraggables[x].id);
	    }
	  }

	}
			
		// for addObserver(), the element parameter doesn't seem to do much
		// onEnd gets called every time ANYTHING is dropped
		// however, 'element' still serves as a hash key for adding/removing observers
		// ..so remove any old observers before adding new
		// ..this ensures reorderDropped() is only triggered once
		var secondlist = $('secondlist');
		Draggables.removeObserver(secondlist);
	    Draggables.addObserver({element:secondlist, onEnd:reorderDropped});
    }

    function initContentDraggables() {

        for(var x=0; x < contentDraggables.length; x++) {
	  Sortable.destroy($(contentDraggables[x]));
	}
    Draggables.observers = [];
	contentDraggables = [];

    }

    function beginAddElement(eventName, draggable, event){
    	disableFlyouts = true;
        if(draggable.element.hasClassName('outside_top')){
            addElementStart = true;
        }
    }

    var lastDroppedTime = 0;
    function reorderDropped(eventName,draggable,event){
    	disableFlyouts = false;
    	
    	if (draggable && draggable.element && (draggable.element.hasClassName('highlightbox') || draggable.element.hasClassName('highlightbox-active'))) {
    		//
    		// Since the Draggables.addObserver call in initDraggables is faulty,
    		// and the reorderDropped handlers is called every time ANY draggable
    		// finishes, we want to STOP this behavior for draggables from the Manage Pages
    		// interface. Check for className (hacky and temporary) ~ashaw
    		//
    		return;
    	}
    	
        var dragged = draggable.element;
        var currentTime = new Date().getTime();
        if(dragged.hasClassName('outside_top')){
            singleUpdateList();
            updateElements();
        }
        else if(currentBlog.editingBlog === 1 || dragged.select('.element-box').size() > 1){
            updateList();
        }
        else if(dragged.hasClassName('inside') && (currentTime > lastDroppedTime + 500)){
            var parent = '';
			if (dragged.parentNode.parentNode.parentNode.className.match("column")) {
			  parent = dragged.parentNode.parentNode.parentNode.id;
			}
            else if (dragged.up('.formlist')) {
			  parent = dragged.up('.formlist').id;
			}
            var currentOrder = 1;
            var listItems = $$('#secondlist li.inside');
            for(var i=0; i<listItems.size(); i++){
                if(listItems[i].id===dragged.id){
                    break;
                }
                currentOrder++;
            }
            var peid = dragged.id.match(/[\d]+/);
            new Ajax.Request(ajax, {parameters:'pos=reorderelement&page_element_id='+peid+'&page_id='+currentPage+'&new_order='+currentOrder+'&parent='+parent+'&cookie='+document.cookie, onFailure:errFunc,
                onSuccess:function(){
                    $$("#secondlist .columnlist").each(
                        function(el){
                            resizeColumns(el);
                            updateTwoColumnDividerHeight(el.id.match(/[\d]+/));
                        }
                    );
                }
            });
            lastDroppedTime = currentTime;
        }
    }

    function updateTwoColumnDividerHeight(el){
        el = el+'';
        var height = $(el).down('#'+$(el).id+'-lhs-list').getHeight();
        $(el).down('.splitpane-divider').setStyle({'height':height+'px'});
    }

    var addElementStart = false;
    function singleUpdateList(container) {

	var currentTime = new Date().getTime();
	if (addElementStart) {

	  addElementStart = false;
      if(allowProElementsTrial && !Weebly.Restrictions.hasNewElementAccess()){
          if(!userEvents['pro_element_upsell']){
              openBillingPage('Please sign-up for a pro account to add that element.');
              removeMarker();
              userEvents['pro_element_upsell'] = 1;
              //new Ajax.Request(ajax, {parameters:'pos=doevent&event=pro_element_upsell&cookie='+document.cookie});
          }
          Pages.go('upgradeWarning');
      }
      else if(Weebly.Form.isNewFormElement() && Weebly.Form.isOverInputLimit()){
          //$('form-element-warning-message').update('You\'ve now reached the maximum number of fields for your form.  If you want to add more fields, please <a style="color:#0054CD; text-decoration:none; font-weight:bold;" href="#" onclick="alertProFeatures(\'Upgrade to publish Pro elements\'); return false;">upgrade</a> to the Weebly Pro service.');
          if(Weebly.Form.isOverInputLimit()){
              $('form-element-warning-message').update('A standard Weebly account allows for 5 fields in a form.  If you want to add this field, please <a style="color:#0054CD; text-decoration:none; font-weight:bold;" href="#" onclick="alertProFeatures(\'Upgrade to publish Pro elements\'); return false;">upgrade</a> to the Weebly Pro service, which allows unlimited fields.');
              $$('#secondlist .outside_top')[0].remove();
          }
          Pages.go('formElementWarning');
      }
      else{
          updateList();
      }

	} else {

	  //console.log("Skipped updateList() -- marked as duplicate");

	}

   }

   var lastWidth = 0;
   function hoverHandler(e) {

	if (Draggables.activeDraggable) return;

      	var selected = $(Event.findElement(e)).up('.inside') || document;
	clearElementBox(selected);
	if (selected != document) {
	
	  var el1 = selected.down('.handleContainer');
	  var el2 = selected.down('.element-box');
	  if (el1 && el2) {
	    var newWidth = el2.getWidth()-2;
	    if (Prototype.Browser.IE6) {
	      newWidth = newWidth + 2;
	      if (lastWidth == newWidth - 2) {
		newWidth = lastWidth;
	      }
	      lastWidth = newWidth;
	    }
	    el1.setStyle({ width: newWidth+"px"});
	  }

	  //var el2 = selected.up('.columnlist');
	  //if (el2) { selected.up('.columnlist').setStyle({ position: "static"}); }

	  //selected.setStyle({ position: "auto"});
	  //selected.down('.handleContainer').clonePosition(selected.down('.element-box'), {setWidth: false, setHeight: false, offsetTop: -27});
	  if(!Weebly.Elements.highlightedElement || !selected.down('.element') || Weebly.Elements.highlightedElement != selected.down('.element').id){
          selected.addClassName('inside-hover');
      }
      var up = selected.up('.column .element-box-contents');
      if(up){
          up.setStyle({'overflow':'visible'});
      }
	}

    }

    function clearElementBox(selected) {

	var currentEl = '';
	if (Weebly.Elements.currentElement && Weebly.Elements.currentElement.up) {
	  currentEl = Weebly.Elements.currentElement.up('.inside');
	}
    else if(currentBox && $(''+currentBox).up('.inside')){
        currentEl = $(''+currentBox).up('.inside');
    }

	$$('#secondlist .inside').each(function(el) {
	  if (el != selected && el != currentEl && el.hasClassName('inside-hover')) {
	    el.removeClassName('inside-hover');
        var up = el.up('.column .element-box-contents');
        if(up){
            up.setStyle({'overflow':'hidden'});
        }
        var options_id = 'dropdown'+el.id.match(/[\d]+/);
        if($(options_id)){
            $(options_id).remove();
        }
	  }
	});	

    }

// Single click handlers
Event.observe( document, 'mouseup',clickHandler );
    function clickHandler(e, ucfid) {

	// Hide all tips
	hideAllTips();

	var targ;
	if(ucfid) { targ = $(ucfid+""); }
	else {
	  if (!e) var e = window.event;
	  if (e.target) targ = e.target;
	  else if (e.srcElement) targ = e.srcElement;
	  if (targ.nodeType == 3) // defeat Safari bug
		targ = targ.parentNode;
	}
	if (targ && typeof targ.id == 'string') {
		if(targ.id.match(/coverbox/)){
		    targ = targ.up('.element');
		}
		if(targ.id.match(/form\-cover/)){
		    return;
		}
    }
	
	// Grab all editable iframe elements, but allow scrolling (scroll_container)
	var iframeEl = isAParentMatch(/[0-9]{8}/, targ);

	if (typeof(targ.id) == 'string' && iframeEl && iframeEl.ondblclick && (iframeEl.tagName == 'H2' || iframeEl.tagName == 'P' || iframeEl.tagName == 'DIV')) {
	  // True of editable text
	  iframeEl.ondblclick();
	  Weebly.Elements.unselectElement();
	  Event.stop( e );
	} else if(iframeEl && iframeEl.id.match(/[0-9]Edit/)) {
	  // Do nothing -- is a first click on an iframe editable document
	} else if(((typeof(targ.id) == 'string' && targ.id != 'scroll_container') || typeof(targ.id) != 'string') && !isAParent('editMenu', targ) && !isAParent('createLink', targ) && !isAParent('colorChooserDiv', targ) && !isAParent('new-color-chooser', targ) && !isAParentMatch(/menuBar(Advanced)?Div/, targ) && !isAParent('notifications', targ) && !isAParentByClass('customhtml_textarea', targ) && !isAParent('flashContainer', targ) && isAParent(document, targ)) {
	  // True if not editable text, not linker, not color chooser, not text menu, not "scroll_container", not the flash upload container, and the click did not happen within an iFrame
	  if(currentBox && $(currentBox)) hideEditBox(currentBox);
	  if(headerSelected) unselectHeader();
	  var elementNode = isAParentByClass('element', targ);
	  if (elementNode) {
	    Weebly.Elements.selectElement(elementNode);
	  } else if(isAParentByClass('weebly_header', targ)) {
	    Weebly.Elements.unselectElement();
	    selectHeader();
	  } else if (isAParent('weeblyLightbox', targ) && !isAParent('weeblyLightboxInside', targ)) {
	    // Lightbox outside click
	    Pages.go("main");
	  } else {
	    Weebly.Elements.unselectElement();
	  }
	} else if (typeof(targ.id) == 'string' && targ.id == 'colorSwatch') {
	  myColorPicker.toggle(e);
	} else if (!isAParent(document, targ)) {
	  //console.log('iFrame click!');
	} else {
	  // This is a click on "scroll_container", the linker, the color chooser, or the text menu
	}
	//console.log("click! "+targ+", "+iframeEl.tagName);

    }

    function handlerFuncSaveContent(t, dontRefresh) {

        if (t.responseText.indexOf('the following problems') > 0 && !dontRefresh) {
           showError(t.responseText);
	   updateList(currentPage);
        }

    }

    function hideFooter() {

        $("footerCode").style.display = "none"; 
        footerCodeShown = 1;

        new Ajax.Request(ajax, {parameters: 'pos=hidefooter&siteid='+currentSite+'&cookie='+document.cookie, 'onFailure':errFunc, bgRequest: true});

    }

    function saveProperties(target, id, goMain) {

	if( uploadInProgress > 0) {
		showError(/*tl(*/"Your image file is still uploading. Please wait until the upload is finished."/*)tl*/);
	} else {

	Element.setStyle('errorProperties', {display:'none'});

        var propertiesBasic = $('propertiesBasic').getElementsByTagName('td');
        var lenBasic = propertiesBasic.length;
        var paramsBasic = '';
        for(var x=0; x < lenBasic; x++) {
	    var thisForm = propertiesBasic[x].getElementsByTagName('form');
            for(var z=0; z < thisForm.length; z++) {
            	var inputname = thisForm[z].childNodes[0].name;
            	var inputval;
            	if (inputname == 'enable_nav_more') {
            		inputval = thisForm[z].childNodes[0].checked ? 1 : 0;
            	}else{
	                inputval = thisForm[z].childNodes[0].value;
	            }
	            
	            paramsBasic += inputname + String.fromCharCode(3) + inputval + String.fromCharCode(2);

		//Special property actions
		if (inputname == "enable_nav_more") {
			DISABLE_NAV_MORE = !inputval;
			// page will be rewritten anyway, so just chill..
		}
		if (inputname == "site_title") {
          if($('weebly_site_title')){
		    $('weebly_site_title').innerHTML = thisForm[z].childNodes[0].value;
          }
		}
		if (inputname == "copyright_text") {
            if($('footerCode')){
                $('footerCode').innerHTML = thisForm[z].childNodes[0].value;
            }
		}
		if (inputname == "site_password" && Weebly.Restrictions.hasAccess("editable_site_password")) {

				newSitePassword = thisForm[z].childNodes[0].value;
				if (newSitePassword == "") {
					for (var pid in Weebly.PageManager.pages) {
						Weebly.PageManager.pages[pid].pwprotected = 0;
					}
				}
				else if (oldSitePassword == "" && newSitePassword != "") {
					for (var pid in Weebly.PageManager.pages) {
						Weebly.PageManager.pages[pid].pwprotected = 1;
					}
				}
		  
		}
            }
        }

        var propertiesAdvanced = $('propertiesAdvanced').getElementsByTagName('td');
        var lenAdvanced = propertiesAdvanced.length;
        var paramsAdvanced = '';
        for(var x=0; x < lenAdvanced; x++) {
	    var thisForm = propertiesAdvanced[x].getElementsByTagName('form');
	    for(var z=0; z < thisForm.length; z++) {
		paramsAdvanced += thisForm[z].childNodes[0].name + String.fromCharCode(3) + thisForm[z].childNodes[0].value + String.fromCharCode(2);
	    }
        }

        new Ajax.Request(ajax, {parameters:'pos='+target+'&reqid='+id+'&keys='+encodeURIComponent(paramsBasic)+encodeURIComponent(paramsAdvanced)+'&cookie='+document.cookie, onSuccess:function(t){ handlerFuncSaveProperties(t, goMain) }, onFailure:errFunc, bgRequest: true});

	} 
    }

    function handlerFuncSaveProperties(t, noGoMain) {

	if (t.responseText.indexOf('RELOAD') >= 0) { window.location.reload(); }

	if (t.responseText.indexOf('the following problems') > 0) {
	   $('errorProperties').innerHTML = t.responseText;
	   Effect.Appear('errorProperties', { duration: 0.5 });
	} else {
	   writeTheme(currentTheme);
	   updateList(currentPage);
	   //updatePages();
	   if (!noGoMain) { Pages.go('main'); }
	}

    }

    function displayProperties(ucfid) {

	currentElement = ucfid;

	new Ajax.Request(ajax, {parameters:'pos=properties&reqid='+ucfid+'&cookie='+document.cookie, onSuccess:handlerDisplayProperties, onFailure:errFunc, onException:exceptionFunc});

    }

    function handlerDisplayProperties(t) {

        var responseElements = new Array();
	var numOfProperties  = 0;
	responseElements     = t.responseText.split('%%NEXT%%');

        var propertiesTitle  = $('propertiesTitle');
	propertiesTitle.innerHTML = responseElements[0];

        if (responseElements[2]) { 
           var propertiesBasic  = $('propertiesBasic');
           propertiesBasic.innerHTML = responseElements[2];
           Element.setStyle('propertiesBasicHeader', {display:'block'});
           Element.setStyle('propertiesBasic', {display:'block'});
	   numOfProperties++;
        } else {
           var propertiesBasic  = $('propertiesBasic');
           propertiesBasic.innerHTML = '';
           Element.setStyle('propertiesBasicHeader', {display:'none'});
           Element.setStyle('propertiesBasic', {display:'none'});
        }

        if (responseElements[3]) {
           var propertiesAdvanced  = $('propertiesAdvanced');
           propertiesAdvanced.innerHTML = contentDecode(responseElements[3]);
           Element.setStyle('propertiesAdvancedHeader', {display:'block'});
           Element.setStyle('propertiesAdvanced', {display:'none'});
	   collapseAdvanced();
           numOfProperties++;
	}else{
	Element.setStyle('propertiesAdvancedHeader', {display:'none'});
        Element.setStyle('propertiesAdvanced', {display:'none'});
	}
	// load element_js from DB if present
	// function must be defined at top of this page
	// remove newlines from data
	responseElements[4]=responseElements[4].replace(/\n/gi,"");
	if (responseElements[4]) {
           eval("validateFunction = " + responseElements[4]);
	   //if element_js is present, use validateFunction when saving
	   $('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"validateFunction('properties', '"+currentElement+"'); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>cancel</a></div></div>";
}
else
{
	   //if element_js is not present, use default saveProperties function
	   $('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"saveProperties('properties', '"+currentElement+"'); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>cancel</a></div></div>";
}
	// load element_onchange_js from DB if present
	responseElements[5]=responseElements[5].replace(/\n/gi,"");
        if (responseElements[5]) {
	   //alert(responseElement[5]);
	   
	   //set validateChangeFunction
           eval("validateChangeFunction = " + responseElements[5]);
	}
	if (numOfProperties == 0) {
           var propertiesBasic  = $('propertiesBasic');
           propertiesBasic.innerHTML = /*tl(*/"This element does not have any properties"/*)tl*/;
           Element.setStyle('propertiesBasicHeader', {display:'block'});
           Element.setStyle('propertiesBasic', {display:'block'});
	}

	showProperties(45);

    }

    function AddPage() {

	$('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"createPage(); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>"+/*tl(*/"cancel"/*)tl*/+"</a></div></div>";

	$('propertiesTitle').innerHTML = "Add Page";
        $('propertiesBasic').innerHTML = "<p style='width: 75%;'>"+/*tl(*/"When you add a page, it will show up in the pages tab, along with the rest of the pages on your site."/*)tl*/+"</p><br/><b>"+/*tl(*/"Title your page"/*)tl*/+"</b><br/><input type='text' style='width: 200px; border: 1px solid black;' id='pageTitle'/>";
        Element.setStyle('propertiesBasicHeader', {display:'none'});
        Element.setStyle('propertiesBasic', {display:'block'});
        Element.setStyle('propertiesAdvancedHeader', {display:'none'});
        Element.setStyle('propertiesAdvanced', {display:'none'});
        Element.setStyle('textEditBox', {display:'none'});

	showProperties(45);

    }

    function createPage() {
	var pageTitle = $('pageTitle').value;

	new Ajax.Request(ajax, {parameters:'pos=addpage&title='+encodeURIComponent(pageTitle)+'&cookie='+document.cookie, onSuccess: function(t) { handlerAddPage(t, pageTitle); }, onFailure:errFunc, onException:exceptionFunc});

    }

    function handlerAddPage(t, newTitle) {

	if (t.responseText) {
	   currentPage = t.responseText;

	   sitePages[t.responseText] = newTitle;
	   writeTheme(currentTheme);
	   updateList(currentPage);

	} else {
	   showError(/*tl(*/"There was an error adding your page. Please try again."/*)tl*/);
	}

	Pages.go('main');

    }

    function goUpdateList(pageID, run) {
	//console.log("goUpdateList. pageID: "+pageID+"; run: "+run);
	unselectHeader(1);
	if (run == 0) { setTimeout("goUpdateList('"+pageID+"', 1)", 500); }
	else if (pageDblClick == 0) {
	  Pages.go('updateList',pageID);
	} else {
	  pageDblClick = 0;
	}

    }

    function goBlogPage(href) {
	resetScrollTop = 1;
	href = href.startsWith( '/' ) ? href : '/' + href;
	var loc = currentPage.replace(/([0-9])+\/.+/, "$1")+href;
	loc     = loc.replace(/\/[0-9]+$/, "");
	Pages.go('updateList', loc);

    }

    function goNewBlogPost() {
	currentBlog.saving = 0;
	Pages.go('goBlogPost', 1, 1);
    }

    function goBlogPost(postid, comments) {
	currentBlog.skipToComments = comments;
	Pages.go('goBlogPost', postid);
    }

    function goDiscardDraft(pageID) {
	new Ajax.Request(ajax, {parameters:'pos=blogdiscard&pageid='+pageID+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerDiscardDraft});
    }

    function handlerDiscardDraft(t) {
	Pages.go('updateList', currentPage);
    }

    function goSaveDraft(blogID) {
	if (currentBlog.postId == 1) { currentBlog.saving = 1; }
	new Ajax.Request(ajax, {parameters:'pos=blogsavedraft&blogid='+blogID+'&title='+encodeURIComponent($('blog-post-title').value)+'&categories='+encodeURIComponent($('blog-post-categories').value)+'&comments='+$('draft-comments').value+'&date='+encodeURIComponent($('blog-date-field').value)+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerSaveDraft});
    }

    function handlerSaveDraft(t) {
	Pages.go('updateList', currentPage);
    }

    function goPublishPost(pageID, update) {
	if (currentBlog.postId == 1) { currentBlog.saving = 1; }
	new Ajax.Request(ajax, {parameters:'pos=blogpublish&pageid='+pageID+'&title='+encodeURIComponent($('blog-post-title').value)+'&categories='+encodeURIComponent($('blog-post-categories').value)+'&comments='+$('draft-comments').value+'&date='+encodeURIComponent($('blog-date-field').value)+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess: function(t) { handlerPublishPost(t, update)}});
    }
    function handlerPublishPost(t, update) {
	if (!update) { Pages.go('updateList', currentPage); }
    }

    function goSavePost(postID, update) {
        new Ajax.Request(ajax, {parameters:'pos=blogsave&postid='+postID+'&title='+encodeURIComponent($('blog-post-title').value)+'&categories='+encodeURIComponent($('blog-post-categories').value)+'&comments='+$('post-comments').value+'&date='+encodeURIComponent($('blog-date-field').value)+'&permalink='+encodeURIComponent($('permalink-url').value)+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess: function(t) { handlerSavePost(t, update)}});
    }
    function handlerSavePost(t, update) {
	if (!update) { Pages.go('updateList', currentPage); }
    }

    function goDeletePost(postID) {
	new Ajax.Request(ajax, {parameters:'pos=blogdeletepost&postid='+postID+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerDeletePost});
    }

    function handlerDeletePost(t) {
	updateList(currentPage);
    }

    function saveBlogSettings() {

	new Ajax.Request(ajax, {parameters:'pos=blogsettingssave&blogid='+currentBlog.blogId+'&allow-comments='+$('blog-settings-allow-comments').value+'&email-notify='+$('blog-settings-email-comments').value+'&notify-address='+$('blog-settings-notify-address').value+'&close-comments='+$('blog-settings-close-comments').value+'&date-format='+$('blog-settings-date-format').value+'&time-format='+$('blog-settings-time-format').value+'&time-zone='+$('blog-settings-time-zone').value+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:function() { Pages.go('main'); }});

    }

    function goDeleteComment(commentID) {
        new Ajax.Request(ajax, {parameters:'pos=blogdeletecomment&commentid='+commentID+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerDeleteComment});
    }

    function handlerDeleteComment(t) {
	t.responseText = t.responseText.replace(/[^0-9]/, '');
	if (t.responseText.match(/[0-9]+/)) {
	  new Effect.Fade($(t.responseText+''));
	}
    }

    function goTrackback() {
	if (!$('trackback-url') || !$('trackback-url').value) return;

	$('trackback-status').innerHTML = '';
	new Ajax.Request(ajax, {parameters:'pos=dotrackback&postid='+currentBlog.postId+'&url='+encodeURIComponent($('trackback-url').value)+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerTrackback});
    }

    function handlerTrackback(t) {

	$('trackback-url').value = '';
	if (t.responseText.match("%%SUCCESS%")) {
	  $('trackback-status').innerHTML = /*tl(*/'Success!'/*)tl*/;
	} else {
	  $('trackback-status').innerHTML = /*tl(*/'Trackback failed.'/*)tl*/;
	}


    }

    function calendarCallback(calendar, newDate) {

        $('blog-date').innerHTML = newDate;
        $('blog-date-field').value = newDate;
        new Effect.Highlight('blog-date',{duration: 2.0});

    }



    function displayPageProperties(pageID) {

	currentPage = pageID;

        new Ajax.Request(ajax, {parameters:'pos=editpage&reqid='+pageID+'&cookie='+document.cookie, onSuccess:handlerDisplayPageProperties, onFailure:errFunc, onException:exceptionFunc});
    
    }
                                                                                                                         
    function handlerDisplayPageProperties(t) {
        
	$('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"saveProperties('editpage', '"+currentPage+"'); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>cancel</a></div></div>";

        $('propertiesTitle').innerHTML = /*tl(*/"Edit Page Properties"/*)tl*/;

        var propertiesBasic  = $('propertiesBasic');
        propertiesBasic.innerHTML = "<br/><p>"+/*tl(*/"In this section you can make various changes to your page, including changing its title, and marking it for deletion."/*)tl*/+"</p><br/>"+t.responseText;

        Element.hide('propertiesBasicHeader');
        Element.hide('textEditBox');
	$('textEditorBox').value = '';
        Element.hide('propertiesAdvancedHeader');
        Element.hide('propertiesAdvanced');
	$('propertiesAdvanced').innerHTML = '';

        showProperties(45);	
	Element.show('propertiesBasic');

    }

    function displayUserSettings() {
             
        new Ajax.Request(ajax, {parameters:'pos=usersettings&cookie='+document.cookie, onSuccess:handlerDisplayUserSettings, onFailure:errFunc, onException:exceptionFunc});
                                                                                                                             
    }
                                                                                                                             
    function handlerDisplayUserSettings(t) {
                                                                                                                             
	$('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"saveProperties('usersettings', ''); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>"+/*tl(*/"cancel"/*)tl*/+"</a></div></div>";

        $('propertiesTitle').innerHTML = /*tl(*/"Edit Profile"/*)tl*/;
                                                                                                                             
        var propertiesBasic  = $('propertiesBasic');
        propertiesBasic.innerHTML = t.responseText;
                                                                                                                             
        Element.hide('propertiesBasicHeader');
        Element.hide('textEditBox');
        $('textEditorBox').value = '';
        Element.hide('propertiesAdvancedHeader');
        Element.hide('propertiesAdvanced');
        $('propertiesAdvanced').innerHTML = '';
                    
        showProperties();
        Element.show('propertiesBasic');
    }

    function displaySiteSettings(update) {

	if (update == 1) {
          var t = new Ajax.Request(ajax, {parameters:'pos=sitesettings&reqid='+currentSite+'&cookie='+document.cookie, onSuccess:function(t){ handlerDisplaySiteSettings(t, update)}, onFailure:errFunc, onException:exceptionFunc});

	} else { handlerDisplaySiteSettings({responseText: ""},2); }

    }

    var siteAccordion;
    var oldSitePassword;
    function handlerDisplaySiteSettings(t, update) {

	if (update == 1) {

	$('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"saveProperties('sitesettings',currentSite); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>"+/*tl(*/"cancel"/*)tl*/+"</a></div></div>";

          $('propertiesTitle').innerHTML = "Edit Site Settings";

          var propertiesBasic  = $('propertiesBasic');
          propertiesBasic.innerHTML = t.responseText;

          Element.hide('propertiesBasicHeader');
          Element.hide('textEditBox');
          $('textEditorBox').value = '';
          Element.hide('propertiesAdvancedHeader');
          Element.hide('propertiesAdvanced');
          $('propertiesAdvanced').innerHTML = '';

	}

	siteAccordion = new accordion('siteSettingsAccordion', {classNames: { toggle: 'accordion_toggle', toggleActive: 'accordion_toggle_active', content: 'accordion_content'}, defaultSize: { width: 525 }});
	$$('#siteSettingsAccordion .accordion_content').each(function(el) { if(el.id != 'siteSettingsOpened') { el.style.height = '0px'; } });
	siteAccordion.activate($$('#siteSettingsAccordion .accordion_toggle')[0]);
	$('siteSettingsOpened').style.display = 'block';
	$('siteSettingsOpened').style.height = '0px';

        showProperties(45);
        Element.show('propertiesBasic');

	if ($('site_password'))
	  oldSitePassword = $('site_password').value;

	//var verticalAccordion = new accordion('#siteSettingsAccordion');

	//if( responseText.indexOf('Auto-Generated') > -1) { setTimeout("showTip(\"We've generated this name for you, but you're more than welcome to change it to anything you'd like.\", 'userIdentifier', 'y', 'userIdentifier');", 1500); setTimeout("hideTip('tipuserIdentifier');", 10000); }

    }
   
   function showHideTitle() {

	if ($('show_title').checked) {
	  hideTitle = false;
	  $('hide_title').value = 0;
      $('weebly_site_title').setStyle({'visibility':'visible'});
	} else {
	  hideTitle = true;
	  $('hide_title').value = 1;
      $('weebly_site_title').setStyle({'visibility':'hidden'});
	}

   }

    function saveMetaInfo() {
    	 var meta_description = encodeURIComponent($('meta_description').value);
	 var meta_keywords = encodeURIComponent($('meta_keywords').value);
	 var footer_code = encodeURIComponent($('footer_code').value);
	 var header_code = encodeURIComponent($('header_code').value);
	 // ajax call to check domain
         new Ajax.Request(ajax, {parameters:'pos=savemetainfo&metaKeywords='+meta_keywords+'&metaDescription='+meta_description+'&footerCode='+footer_code+'&headerCode='+header_code+'&cookie='+document.cookie, onSuccess:handlerSaveMetaInfo, onFailure:errFunc, bgRequest: true, onException:exceptionFunc});

    }

    function handlerSaveMetaInfo(x) {
    	
	   if(x.responseText.match(/%%SUCCESS%%/)){
	   	//Effect.SlideUp("showMetaProperties");
		$('errorProperties').style.display="hidden";
 		$('errorProperties').innerHTML = "";
	   }else if(x.responseText.match(/%%DESCRIPTIONLENGTH%%/)){
		$('errorProperties').innerHTML = /*tl(*/"Description too long, please shorten to 150 characters"/*)tl*/;
		$('errorProperties').style.display="block";
	   }else if(x.responseText.match(/%%KEYWORDLENGTH%%/)){
	   	$('errorProperties').innerHTML = /*tl(*/"Too many keywords, please limit to 1024 characters"/*)tl*/; 
		$('errorProperties').style.display="block";
	   }else if(x.responseText.match(/%%ERROR%%/)){
	   	$('errorProperties').innerHTML = /*tl(*/"Error saving, please try again."/*)tl*/; 
		$('errorProperties').style.display="block";
	   }
    }  
 
    function verifyDomainForm() {
	
	// check all form values before submitting
    	if ($('CCName').value == "") {

		$('domainError').innerHTML = /*tl(*/"Error: Please enter Cardholder's Name"/*)tl*/;

	}else if ($('CreditCardNumber').value =="") {
		
		$('domainError').innerHTML = /*tl(*/"Error: Please enter Card Number"/*)tl*/;
	
	}else if ($('CVV2').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter CVV2"/*)tl*/;

	}else if ($('CCAddress').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter Address"/*)tl*/;
	
 	}else if ($('RegistrantCity').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter City/Town"/*)tl*/;

 	}else if ($('CCCountry').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter Country"/*)tl*/;
 	
	}else if ($('CCZip').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter Zip/Postal Code"/*)tl*/;
	
	}else if ($('CCPhone').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter Phone"/*)tl*/;
 
	}else {
		// purchase the domain
		$('domainError').innerHTML = "";
		var newEl = document.createElement('DIV');
		newEl.id = 'purchaseDomainLoading';
		newEl.innerHTML = "Loading...";
		newEl.style.padding = "8px 0 0 50px";
		$('purchaseDomain').parentNode.appendChild(newEl);
		$('purchaseDomain').style.display = "none";
		purchaseDomain();
	}
    }

    function purchaseDomain() {
	
	// Create register.com account for customer

        // Loop through form fields
        var thisform = document.forms.chooseDomainCreditForm;
        var form2 = document.forms.domainLength;
        var formdata = "";
        for (i=0; i < thisform.length; i++) {

           // Build post string
           if(thisform.elements[i].type == "text"){ // Textbox's
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }else if(thisform.elements[i].type == "textarea"){ // Textareas
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }else if(thisform.elements[i].type == "checkbox"){ // Checkbox's
                 formdata = formdata + thisform.elements[i].name + "=" + thisform.elements[i].checked + "&";
           }else if(thisform.elements[i].type == "radio"){ // Radio buttons
                  if(thisform.elements[i].checked==true){
                     formdata = formdata + thisform.elements[i].name + "=" + thisform.elements[i].value + "&";
                  }
           }else{
                  // select box is all thats left
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }
        }

        for (x=0; x< form2.length; x++) {
                if (form2.elements[x].type == "radio"){ // Radio buttons
                        if(form2.elements[x].checked==true){
                                formdata = formdata + form2.elements[x].name + "=" + form2.elements[x].value + "&";
                        }
                }
        }

        //console.log(formdata);

        // break apart CredCardExpiration into CreditCardExpMonth and CreditCardExpYear
        //var CCX = $('CreditCardExpiration').value.split("\/");
        //var CreditCardExpMonth = CCX[0];
        //var CreditCardExpYear = CCX[1];

 	// break down domain
        var finalDomain = $('finalDomainName').innerHTML.split(".");
        var sld = finalDomain[0];
        var tld = finalDomain[1];

        // https call
	var reqId = Math.floor(Math.random()*1000001);
	var req   = document.createElement('script');
	req.id    = reqId;
	req.type  = 'text/javascript';
	req.src   = 'https://secure.weebly.com/weebly/apps/registerHttps.php?reqid='+reqId+'&pos=CreateRegisterAccount&tld='+tld+'&sld='+sld+'&'+formdata+'&cookie='+document.cookie+'';
	
	//console.log(req.src);
	
	// Append to HEAD element
	document.childNodes[1].childNodes[0].appendChild(req);
	
	// PHP script returns:
	//handlerFunction(reqId, 'responseText.....');
	

	//new Ajax.Request(ajax, {parameters:'pos=CreateRegisterAccount&tld='+tld+'&sld='+sld+'&CreditCardExpMonth='+CreditCardExpMonth+'&CreditCardExpYear='+CreditCardExpYear+'&'+formdata+'&cookie='+document.cookie, onSuccess:handlerPurchaseDomain, onFailure:errFunc, bgRequest: false, onException:exceptionFunc});
    }

    function handlerFunctionHttps(x) {
	//alert(reqId);
	
	if (x.match(/%%SUCCESS%%/)) {
                var orderID = x.split("|");
                orderID = orderID[1];
                //alert('Success! Order ID: '+orderID);
		
		completeDomainPurchase(orderID);
        }else{
		if ($('purchaseDomainLoading') && $('purchaseDomainLoading').parentNode) {
		  $('purchaseDomainLoading').parentNode.removeChild($('purchaseDomainLoading'));
		}
		$('purchaseDomain').style.display = 'block';
                $('domainError').innerHTML = x;
        }	



	// Clean up request script tag
        //$('reqId').parentNode.removeChild($('reqId'));
     }
    
     function purchaseDomain2() {

	// Create register.com account for customer

	// Loop through form fields
	var thisform = document.forms.chooseDomainCreditForm;
	var form2 = document.forms.domainLength;
	var formdata = "";
	for (i=0; i < thisform.length; i++) {
        
	   // Build post string
           if(thisform.elements[i].type == "text"){ // Textbox's
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }else if(thisform.elements[i].type == "textarea"){ // Textareas
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }else if(thisform.elements[i].type == "checkbox"){ // Checkbox's
                 formdata = formdata + thisform.elements[i].name + "=" + thisform.elements[i].checked + "&";
           }else if(thisform.elements[i].type == "radio"){ // Radio buttons
                  if(thisform.elements[i].checked==true){
                     formdata = formdata + thisform.elements[i].name + "=" + thisform.elements[i].value + "&";
                  }
           }else{
                  // select box is all thats left
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }
        }

	for (x=0; x< form2.length; x++) {
		if (form2.elements[x].type == "radio"){ // Radio buttons
                  	if(form2.elements[x].checked==true){
                     		formdata = formdata + form2.elements[x].name + "=" + form2.elements[x].value + "&";
                  	}
		}
	}

	//console.log(formdata);
	
	// break apart CredCardExpiration into CreditCardExpMonth and CreditCardExpYear
	var CCX = $('CreditCardExpiration').value.split("\/");
	var CreditCardExpMonth = CCX[0];
	var CreditCardExpYear = CCX[1];
	
	// break down domain
	var finalDomain = $('finalDomainName').innerHTML.split(".");
	var sld = finalDomain[0];
	var tld = finalDomain[1];

	new Ajax.Request(ajax, {parameters:'pos=CreateRegisterAccount&tld='+tld+'&sld='+sld+'&CreditCardExpMonth='+CreditCardExpMonth+'&CreditCardExpYear='+CreditCardExpYear+'&'+formdata+'&cookie='+document.cookie, onSuccess:handlerPurchaseDomain, onFailure:errFunc, bgRequest: false, onException:exceptionFunc});
    }

    function handlerPurchaseDomain(x) {
	if (x.responseText.match(/%%SUCCESS%%/)) {
		var orderID = x.responseText.split("|");
		orderID = orderID[1];
		alert('Success! Order ID: '+orderID);
		Element.hide('chooseDomain');
	}else{
    		$('domainError').innerHTML = x.responseText;
	}
    }

    function updatePageTitle(theId, value) {

	sitePages[theId] = value.innerHTML;

    }

    function updateSiteTitle(value) {

	currentTitle = value;

	new Ajax.Request(ajax, {parameters:'pos=sitetitle&newtitle=' + encodeURIComponent(value)+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc});

    }
    
    function afterUpdateSiteTitle() {
		if (window.refreshNavCondense) {
			refreshNavCondense();
		}
    }

    function doClick( elId) {

	var evt;
	var el = document.getElementById(elId);
	if (document.createEvent){
	  evt = document.createEvent("MouseEvents");
	  if (evt.initMouseEvent){
	    evt.initMouseEvent("click", true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);
	  } else {
	    evt = false;
	  }
	}
	(evt)? el.dispatchEvent(evt) : (el.click && el.click());

    }

    function exportSite() {

	// Is a regular site
	if (settingQuickExport == 0) {
	  Pages.go('domainMenu',1);
          //new Ajax.Request(ajax, {parameters:'pos=sitesettings&reqid='+currentSite+'&cookie='+document.cookie, onSuccess:handlerExportSite, onFailure:errFunc, onException:exceptionFunc});
	} else {
	  Pages.go('doExport');
	}

    }

    function handlerExportSite(t) {

	$('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"saveProperties('sitesettings', currentSite, 1); Pages.go('doExport'); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>cancel</a></div></div>";

        $('propertiesTitle').innerHTML = "Publish Site";

        var propertiesBasic  = $('propertiesBasic');
        propertiesBasic.innerHTML = t.responseText;

        Element.hide('propertiesBasicHeader');
        Element.hide('textEditBox');
        $('textEditorBox').value = '';
        Element.hide('propertiesAdvancedHeader');
        Element.hide('propertiesAdvanced');
        $('propertiesAdvanced').innerHTML = '';

	publishingWindow = 1;

        showProperties(45);
        Element.show('propertiesBasic');

    }

    function doExport() {

	var wait = 0;

	showPublishingAnimation();

	if (wait) {
          setTimeout("new Ajax.Request(ajax, {parameters:'pos=exportsite&cookie='+document.cookie, onSuccess:handlerDoExport, onFailure:errFunc, bgRequest: true});", 350);
	} else {
          new Ajax.Request(ajax, {parameters:'pos=exportsite&cookie='+document.cookie, onSuccess:handlerDoExport, onFailure:errFunc, bgRequest: true});
	}
	
    }

    function handlerDoExport(t) {

	if (t.responseText.indexOf('siteLocation') > 0) {

	  // Editing a blog post
	  //if ($('secondlist').innerHTML.match('%%EDITING-NEW-POST%%') || $('secondlist').innerHTML.match('%%EDITING-DRAFT-POST%%:') || $('secondlist').innerHTML.match('%%EDITING-EXISTING-POST%%:')) {
	  //  Pages.go('updateList', currentPage);
	  //} else {
	    $('exportText').innerHTML = t.responseText;
	    currentSiteLocation = $('siteLocation').innerHTML;
	    Pages.go('exportSuccess');
	  //}

	  // Site has been exported, don't show export options any more
	  settingQuickExport = 1;
	  // Change title from Publishing to congratulations
	  $('chooseAddress1').src = "http://"+editorStatic+/*tli(*/"/weebly/images/congratulations.jpg"/*)tli*/;
	} 
	if (t.responseText.match(/Error\s/i)) {
	  Element.hide('tip14');
	  showError(t.responseText);
	  Pages.go('main');
	}
    if(t.responseText.match(/%%UPGRADEELEMENTS%%/)){
        $('exportText').show().innerHTML = '<div style="font-family: \'Lucida Grande\', \'Lucida Sans\', \'Trebuchet MS\', Helvetica, Arial, Verdana, sans-serif; font-size: 16px; line-height:1.5; font-weight:bold;">Your site contains Weebly Pro elements. Your published site will not include these Pro elements unless you choose to upgrade.</div><div style="float:right; margin-top:35px; margin-right:85px;"><a style="font-size:14px; color:#0054CD; text-decoration:none; font-weight:bold;" href="#" onclick="exportSite(); return false;">Publish Anyway</a></div><div style="width:159px; margin:25px auto;"><a href="#" onclick="alertProFeatures(\'Upgrade to publish Pro elements\'); return false;"><img src="http://'+editorStatic+'/weebly/images/upgrade_large.gif" alt="Upgrade" style="border:none;" /></a></div>';
    }

    }

// Chris' code -- not formatted correctly! :) -David
function unPublishSite(siteId){
	//alert('siteId: '+siteId+' userId: '+userId);
	//Display warning
	$("unpublish").innerHTML="<img src='http://"+editorStatic+"/weebly/images/action_stop.gif' style='border: 0; margin-right: 3px;' alt='Red X' /><span style='font-weight: bold; color: #555;'>"+/*tl(*/"Are you sure?"/*)tl*/+" </span><a href='#' onClick='doUnPublishSite(\""+siteId+"\");' style='color: green;'>"+/*tl(*/"Yes, Un-Publish"/*)tl*/+"</a>";

}
function doUnPublishSite(siteId){
	new Ajax.Request(ajax, {parameters:'pos=unpublishsite&siteid='+siteId+'&cookie='+document.cookie, onSuccess:handlerunPublishSite, onFailure:errFunc, bgRequest: true});
}
function handlerunPublishSite(t) {
	if(t.responseText.match(/%%SUCCESS%%/)) {
		saveProperties('sitesettings',currentSite);
		//$("unpublish").style.display = "none";
		// be sure to show site settings before next publish
		//settingQuickExport = 0;
	}else{
		showError('Error deleting site: '+ t.responseText);
		}
	}
// End Chris' code
    
    function previewSite(theme) {

        new Ajax.Request(ajax, {parameters:'pos=previewsite&theme='+theme+'&cookie='+document.cookie, onSuccess:handlerPreviewSite, onFailure:errFunc});
                                                                                                                             
    }

    function handlerPreviewSite(t) {

	if (t.responseText) { window.open("http://"+configSiteName+"/weebly/previewSite.php?req_id="+t.responseText); }

    }

    function exportSiteZip() {

        new Ajax.Request(ajax, {parameters:'pos=exportsitezip&cookie='+document.cookie, onSuccess:handlerExportSiteZip, onFailure:errFunc});

    }

    function handlerExportSiteZip(t) {

        if (t.responseText.match("error")) { 
	  showError(t.responseText); 
	} else { 
	  window.open("http://"+configSiteName+"/weebly/exports/"+userID+"/"+t.responseText+".zip"); 
	}

    }

    function closeCreateLinkText(linkUrl) {

	runCommand(currentBox,'createlink', linkUrl);

    }

    function showCreateLink() {

        var menuPos  = new Position.cumulativeOffset(document.getElementById(currentBox+'Edit'));
	Weebly.Linker.show('closeCreateLinkText', {'top': 185, 'left': 250}, Array('linkerWebsite', 'linkerWeebly', 'linkerEmail', 'linkerFile'), 'linkerWeebly', currentBox);

    }

    function showCreateLinkProperties(ucfpid) {

	Weebly.Linker.ucfpid = ucfpid;
        var menuPos  = new Position.cumulativeOffset(document.getElementById('textEditor'));
	var linkerTypes = Array('linkerWebsite', 'linkerWeebly', 'linkerEmail', 'linkerFile');
	var linkerSelected = 'linkerWeebly';
	if (Weebly && Weebly.Elements && Weebly.Elements.currentContentFieldDef && Weebly.Elements.currentContentFieldDef.hasimage == 1) { linkerTypes.push('linkerImage'); linkerSelected = 'linkerImage'; }
    else if(Weebly && Weebly.Elements && Weebly.Elements.currentContentFieldDef && Weebly.Elements.isFormElement()) { linkerTypes = Array('linkerWebsite', 'linkerWeebly'); }
        Weebly.Linker.show('closeCreateLinkProperties', {'top': 185, 'left': 250}, linkerTypes, linkerSelected, currentElement);

    }

    function closeCreateLinkProperties(linkUrl) {

    if( $('webpageNewWindow') && $('webpageNewWindow').checked )
    {
       var link = linkUrl.replace('weeblylink_new_window','');
       $(Weebly.Linker.ucfpid+"").value = "<a href='"+link+"' target='_blank'>";
    }
    else
    {
        $(Weebly.Linker.ucfpid+"").value = "<a href='"+linkUrl+"'>";
    }
	Weebly.Elements.onChange(Weebly.Linker.ucfpid);
	Behaviour.apply();

    }

    function selectTheme(theme, makeFavorite, favoriteCallback) {

	$('focusMe').focus();
	Pages.go('themesMenu');
	
	$$('.themePictureImgActive').each(function(el){el.className = 'themePictureImg';});
	//$('theme_'+currentTheme).className = 'themePictureImg';

	currentTheme = theme;
	
	if (previewingThemes) {
		$('theme_' + currentPreviewTheme).removeClassName('themePictureImgPreview');
		previewingThemes = 0;
		currentPreviewTheme = '';
	}
	previewThemeEventCnt++;

	if ($('theme_'+currentTheme)) {
		$('theme_'+currentTheme).className = 'themePictureImgActive';
	}

	loadDesignOptions(currentTheme);
    writeTheme(currentTheme);
	new Ajax.Request(ajax, {
		parameters: 'pos=settheme&keys='+theme+(makeFavorite ? '&favorite=1' : '')+'&cookie='+document.cookie,
		onSuccess: function(t) {
			if (favoriteCallback) {
				favoriteCallback();
			}
			handlerSelectTheme(t);
		},
		onFailure: errFunc,
		bgRequest: true
	});

    }

    function handlerSelectTheme(t) {

	updatedTheme = 1;

	if (!userEvents.tab_pages) {
	  showEvent('selectTheme', 1);
	}

    }

    function showThemeOptions(noScroll) {

	// If not in the themes tab, bail
	if ($('weebly_tab_themes').className == "weebly-notcurrent") { return; }

	/**
	// Uncomment to enable theme switching
	if (noScroll) {
	  $('themeCategories').style.display = 'none';
	  $('themesBack').style.display = 'none';
	} else {
	  Effect.Fade('themeCategories');
	  Effect.Fade('themesBack');
	}
	$('themesShowThemes').style.display='block';
	$('themesForward').style.display='none';

	if (noScroll) {
	  var moveTo = document.body.clientWidth <= 800 ? 215 : 305;
	  $('themePictures').style.left = (document.body.clientWidth-moveTo)+"px";
	} else {
	  var moveTo = document.body.clientWidth <= 800 ? 215 : 305;
	  new Effect.Move('themePictures', {y: 30, x: (document.body.clientWidth-moveTo), mode: 'absolute'});
	}

	$('currentThemeOptions').style.width = (document.body.clientWidth-268)+"px";

	if (noScroll) {
	  $('themeSettings').style.display = 'block';
	} else {
	  Effect.Appear('themeSettings');
	}

	$('currentThemeImage').src = $("theme_"+currentTheme).src;
	$('currentThemeText').innerHTML = currentTheme;

	firstThemeOnClick = $('themePictures').firstChild.onclick;
	secondThemeOnClick = $('themePictures').childNodes[1].onclick;
	$('themePictures').firstChild.onclick = function() { hideThemeOptions(); };
	$('themePictures').childNodes[1].onclick = function() { hideThemeOptions(); };
	new Effect.Opacity($('themePictures').firstChild, {from: 1.0, to: 0.3});
	new Effect.Opacity($('themePictures').childNodes[1], {from: 1.0, to: 0.3});
	**/

	// Disable theme switching
	if (ENABLE_THEME_BROWSER) {
		$('theme-action-tabs').style.display = 'none';
	}else{
		$('themeCategories').style.display = 'none';
	}
	$('themesBack').style.display = 'none';
	$('themesForward').style.display='none';
	$('themesShowThemes').style.display='none';
	$('themePictures').style.display='none';
	$('currentThemeImage').style.display='none';
	
	$('themeSettings').style.display = 'block';
	$('currentThemeOptions').style.width = (document.body.clientWidth-115)+"px";

	editThemeMode = 1;	

    }

    function hideThemeOptions() {

	// If not in the themes tab, bail
	if ($('weebly_tab_themes').className == "weebly-notcurrent") { return; }

	/**
	// Uncomment to enable theme switching
	Effect.Appear('themeCategories');
	$('themesForward').style.display='block';
	$('themesShowThemes').style.display='none';
        new Effect.Move('themePictures', {y: 30, x: 0, mode: 'absolute'});

	Effect.Fade('themeSettings');

	$('themePictures').firstChild.onclick = firstThemeOnClick; 
	$('themePictures').childNodes[1].onclick = secondThemeOnClick; 
	new Effect.Opacity($('themePictures').firstChild, {from: 0.3, to: 1.0});
	new Effect.Opacity($('themePictures').childNodes[1], {from: 0.3, to: 1.0});

        editThemeMode = null;
	**/

    }
    

    function showThemes(themesCategory) {

		if (!themesCategory) {
			if (ENABLE_THEME_BROWSER) {
				themesCategory = "All";
			}else{
				themesCategory = $$('#themes_select div')[0].innerHTML;
			}
		}

		themesCategory = themesCategory == "All" ? "" : themesCategory;
		
	    if(themesCategory === 'custom') {
	        showCustomThemes();
	    }else{
			Weebly.Cache.get(ajax, 'pos=getthemes&keys='+themesCategory, handlerShowThemes);
		}
		
    }

    function handlerShowThemes(responseText) {

		//var themeReturn = responseText.split(/%%/);

		$('themePicturesInner').innerHTML = responseText;

		if ($('theme_'+currentTheme)) {
			$('theme_'+currentTheme).className = 'themePictureImgActive';
		}

		if (ENABLE_THEME_BROWSER) {
			$('themePicturesInner').childElements().each(function(a) {
				var id = a.down().id.match(/^theme_(.*)$/)[1];
				initThemeFavoriting(id, a.select('span.themeFavoriteIcon')[0], a, 'themeIsFavorite');
			});
		}

    }
    
    function highlightThemeCategory(e)
    {
    	if (!ENABLE_THEME_BROWSER) {
	        $$('a.elements_category_selected').each(function(el){el.writeAttribute('class', 'elements_category');});
    	    e.writeAttribute('class', 'elements_category_selected');
    	}
    }
    
    var isScrollingThemes = false;

	function advanceThemes() {
		var slideWidth = document.body.clientWidth - $('themePictures').offsetLeft - 95,
			inner = $('themePicturesInner'),
			innerLeft = inner.offsetLeft,
			innerWidth = inner.offsetWidth,
			themesBack = $('themesBack'),
			themesForward = $('themesForward');
		themesForward.blur();
		if (innerLeft + innerWidth > slideWidth) {
			themesBack.hide();
			themesForward.hide();
			isScrollingThemes = true;
			new Effect.Move(inner, {
				x: -slideWidth,
				afterFinish: function() {
					themesBack.show();
					if (innerLeft + innerWidth - slideWidth > slideWidth) {
						themesForward.show();
					}
					isScrollingThemes = false;
				}
			});
		}
	}

    function preceedThemes() {
		var slideWidth = document.body.clientWidth - $('themePictures').offsetLeft - 95,
			inner = $('themePicturesInner'),
			innerLeft = inner.offsetLeft,
			innerWidth = inner.offsetWidth,
			themesBack = $('themesBack'),
			themesForward = $('themesForward');
		themesBack.blur();
		if (innerLeft < 0) {
			themesBack.hide();
			themesForward.hide();
			isScrollingThemes = true;
			new Effect.Move(inner, {
				x: slideWidth,
				afterFinish: function() {
					if (innerLeft + slideWidth < 0) {
						themesBack.show();
					}
					themesForward.show();
					isScrollingThemes = false;
				}
			});
		}
    }
    
    function resetThemesMenuScrolling() {
    	$('themePicturesInner').style.left = 0;
    	$('themesBack').hide();
    	$('themesForward').show();
    }

    function showBar() {

        if (navigator.appVersion.indexOf("MSIE") == -1 || navigator.appVersion.indexOf("MSIE 7") > -1) {
          new Effect.Move("elements", {x: 0, y:0, mode: "absolute"});
        }
        $("elements").className="topend";
        //Element.hide("expand");
        //Element.show("shrink");

    }

    function hideBar() {

        if (navigator.appVersion.indexOf("MSIE") == -1 || navigator.appVersion.indexOf("MSIE 7") > -1) {
          new Effect.Move("elements", {x: 0, y:-100, mode: "absolute"});
        }
        $("elements").className="topstart";
        //Element.hide("shrink");
        //Element.show("expand");

    }

    function expandThemeSettings() {

        //Effect.Fade('expandThemes');
        if ($('themeSettings').style.display == "none") {
          Effect.SlideDown('themeSettings');
        } else {
          Effect.SlideUp('themeSettings');
        }

    }

    function editImage(oldImageLocation, imageId) {

	new Ajax.Request(ajax, {parameters:'pos=externalsite&to=snipshot&cookie='+document.cookie, onSuccess: function(t) { handlerEditImage(t, oldImageLocation, imageId) }, onFailure:errFunc, onException:exceptionFunc});


    }

    function handlerEditImage(t, oldImageLocation, imageId) {

	$('editImageFrame').style.height = (getInnerHeight() - 35) + "px";
	$('editImage').style.height = (getInnerHeight() - 35) + "px";
	Element.show("editImageFrame");
	$("editImage").src = "http://services.snipshot.com/?snipshot_input="+escape(oldImageLocation)+"&snipshot_callback="+escape("http://"+configSiteName+"/weebly/remoteCall.php")+"&snipshot_callback_agent=user&snipshot_output_type=url&reqid="+t.responseText+"&from=snipshot&imageLocation="+escape(oldImageLocation)+"&imageId="+imageId;

    }

    function hideEditImage() {

	Element.hide("editImageFrame");
	$("editImage").src = "";

    }

    function closeEditImage(newImageLoc, imageId, ucfid) {

	Pages.go('main');
        Weebly.Elements.selectElement($(""+ucfid));
        $(""+imageId).value = newImageLoc;
        Weebly.Elements.onChange(imageId);

    }

    /*** Element Chooser Functions ***/
/****************************************/

    function setElementsPageType(){
        if(currentBlog.editingBlog){ 
            elementsPage('blog'); 
        } 
        else{
            if(Weebly.Elements.highlightedElement){
                elementsPage('form');
            }
            else{
                elementsPage('default');
            }
        }
    }

    var currentCategoryList;
    function elementsPage(categoryType) {

	currentCategoryList = Weebly.elementCache[categoryType];
	$('elements_1').innerHTML = generateCategoryHTML(currentCategoryList.order);
	selectCategory($$('#elements_1 a')[0].id);

    }

    function generateCategoryHTML(categoryOrder) {

	var categoryHTML = '';
	for (var i = 0; i < categoryOrder.length; i++) {

	  var cc = currentCategoryList[categoryOrder[i]];
	  var sprite = cc['icon'] ? cc['icon'] : 'folder_images';

	  categoryHTML += "<a href=\"#\" onClick=\"selectCategory('ec_"+cc['category_id']+"'); return false;\" class=\"elements_category\" id=\"ec_"+cc['category_id']+"\"><span class=\"elements_folder sprite-small-icons\" id='sprite-"+sprite+"'></span> "+cc['category_name']+"</a>\n";

	}

	return categoryHTML;

    }

    function generateElementHTML(category) {

	var elementHTML = '';

	if (category.category_name == "Imported") {

	  new Ajax.Request(ajax, {parameters:'pos=customelements&cookie='+document.cookie, 'onSuccess':updateCustomElements, 'onFailure':errFunc});

	} else {

	  for (var i=0; i < category.elements.length; i++) {
	    elementHTML += outputElement(category.elements[i].element_id, category.elements[i].display_name, 0, category.elements[i].class_names, category.elements[i].defaults, category.elements[i].icon, category.elements[i].override_title);
	  }

	}

	return elementHTML;

    }

    function selectCategory(categoryName) {

	$('focusMe').focus();
	categoryName = categoryName.replace("ec_", "");

	var currentCategory = currentCategoryList[categoryName];

	if (!currentCategory) {
          Element.hide('elements_2');
	  $('elementlist').style.left  = "150px";
	  $('elementlist').style.width = (document.body.clientWidth-150)+'px';
	  $('elementlist').innerHTML = "<div style='color: black; padding: 5px 0 0 10px;'>"+/*tl(*/"Coming soon..."/*)tl*/+"</div>";
	  return;
	}

	var pos = 1;
	var lvl = 1;

	if (currentCategory.children.length > 0) {
	  $('elements_2').innerHTML = generateCategoryHTML(currentCategory.children);
	  pos = 2;
	} else if (currentCategory.parent > 0) {
	  pos = 2;
	  lvl = 2;
	}

	if (pos == 2) {
          Element.show('elements_2');
	  $('elementlist').style.left  = "300px";
	  $('elementlist').style.width = (document.body.clientWidth-300)+'px';
	} else {
          Element.hide('elements_2');
	  $('elementlist').style.left  = "150px";
	  $('elementlist').style.width = (document.body.clientWidth-150)+'px';
	}

	$('elementlist').innerHTML = generateElementHTML(currentCategory);

	showEvent('elcat_'+categoryName);
        activeElementCategory('ec_'+categoryName,lvl);

	//Behaviour.apply();
		//console.log('select cat name INITDRAGGABLES...');
    	initDraggables();
	currentMenu = categoryName;
    }

    function updateCustomElements(t) {

	if (t.responseText.indexOf(':') > -1) {

          var splitElements = new Array();
          splitElements = t.responseText.split(',');
	
	  var outputElements = '';

	  for(var x=0; x < splitElements.length; x++) {

	    var splitPairs = new Array();
	    splitPairs = splitElements[x].split(':');

	    outputElements += outputElement(splitPairs[0], splitPairs[1], splitPairs[2]);

	  }

	  $('elementlist').innerHTML = outputElements;
	  Behaviour.apply();
	} else {
	  $('elementlist').innerHTML = '';
	  Behaviour.apply();
	}

    }

    function outputElement(elementNumber, elementDescription, customElement, classNames, defaults, icon, overrideTitle) {

	var elementDef = customElement ? 'cdef:' : 'def:';
	var imageNumber= customElement ? customElement : elementNumber;

	elementDescription = overrideTitle ? overrideTitle : elementDescription;

	var elementsFolder = 'elements';
	var elementsFileType = 'gif';
	var elementsBorder = 'none';
	var elementsWidth = '60px';
	classNames = classNames ? "outside_top controlledDrop "+classNames : "outside_top";

	if (siteType == 'myspace') {
	  elementsFolder = 'newelements';
	  elementsFileType = 'gif';
	  elementsBorder = 'none';
	  elementsWidth = '60px';
	}

    var iconFile = 'e'+imageNumber+'.'+elementsFileType;
    if(icon){
        iconFile = 'e'+imageNumber+'_'+icon+'.'+elementsFileType
    }

	var myOutput = '<li class="'+classNames+'" title="'+/*tl(*/'Drag to page"'/*)tl*/+' style="width: 80px;">';
    if(defaults){
        myOutput += '  <form name="id_'+elementNumber+'" autocomplete="off"><input type="hidden" name="idfield" value="' + elementDef + elementNumber + '|'+defaults.replace(/"/g, "'")+'" /></form>';
    }
    else{
        myOutput += '  <form name="id_'+elementNumber+'" autocomplete="off"><input type="hidden" name="idfield" value="' + elementDef + elementNumber + '" /></form>';
    }
	myOutput += '  <img src="http://'+editorStatic+'/weebly/images/'+elementsFolder+'/'+iconFile+'" style="border: '+elementsBorder+'; width: '+elementsWidth+'; height: '+elementsWidth+';"><br/>';
	myOutput += '  ' + elementDescription;
    if(Weebly.Restrictions.requiresUpgrade(elementNumber)){
        var service = Weebly.Restrictions.requiredService(elementNumber);
        var serviceOverlay = Weebly.Restrictions.accessValue(service+'_element_overlay');
        var overlaySrc = typeof(proElementOverlaySrc) != 'undefined' ? proElementOverlaySrc : 'http://'+editorStatic+'/weebly/images/pro-element-overlay.png';
        if(typeof(serviceOverlay) == 'string' && serviceOverlay.length > 0){
            overlaySrc = serviceOverlay;
        }
        myOutput += '<img src="'+overlaySrc+'" class="pro-element-overlay">';
    }
	myOutput += '</li>';

	return myOutput;

    }

    function activeElementCategory(activeCategory, categoryLevel) {

	var categoryEl = $('elements_'+categoryLevel);

	$$('#elements_'+categoryLevel+' a').each( function(el) {
          el.className = "elements_category";
	});

        if(activeCategory != 'none') { $(activeCategory).className = "elements_category_selected"; }

    }

    function activeTab(activePage) {

	var Tabs = ['weebly_tab_edit', 'weebly_tab_pages', 'weebly_tab_themes', 'weebly_tab_settings'];

	$('focusMe').focus();

	for(var i = 0; i < Tabs.length; i++) {
          $(Tabs[i]).className = "weebly-notcurrent";
        }

	$(activePage).className = "weebly-current";

    }

    function activeContainer(activePage) {

	var Areas = ['elements', 'pages', 'themes','settings'];

        for(var i = 0; i < Areas.length; i++) {
          Element.hide($(Areas[i]+"_container"));
        }

	Element.show(activePage+"_container");

	if (activePage == "settings" || activePage == "pages") {
	  $('elements').style.height = '35px';
	  $('placeholderDiv').style.height = '35px';
	  $('scroll_container').style.marginTop = '35px';
	  $('scroll_container').style.height = (getInnerHeight() - 35) + "px";
          $('grayedOut').style.top = '35px';
          $('grayedOut').style.height = (getInnerHeight() - 35) + "px";
	  $('icontent_container').style.minHeight = (getInnerHeight() - 35)+'px';
          $('scroll_container_properties').style.top = '35px';
          $('scroll_container_properties').style.height = (getInnerHeight() - 35) + "px";

	} else {
          $('elements').style.height = '131px';
	  $('placeholderDiv').style.height = '133px';
          $('scroll_container').style.marginTop = '133px';
	  $('scroll_container').style.height = (getInnerHeight() - 133) + "px";
          $('grayedOut').style.top = '133px';
          $('grayedOut').style.height = (getInnerHeight() - 133) + "px";
	  $('icontent_container').style.minHeight = (getInnerHeight() - 133)+'px';
          $('scroll_container_properties').style.top = '133px';
          $('scroll_container_properties').style.height = (getInnerHeight() - 133) + "px";
	}

	if (activePage != "themes") {
	  Element.hide($('themePictures'));
	  Element.hide($('customizeTheme'));
	  Element.hide($('themeSettings'));
	  Element.hide($('themesShowThemes'));
	  if (ENABLE_THEME_BROWSER) {
		Element.hide($('theme-action-tabs'));
	  }else{
		Element.hide($('themeCategories'));
	  }
	  Element.hide($('themesForward'));
	  Element.hide($('themesBack'));
	  $('themePictures').style.left = "0px";
	} else {

	  if (siteType == 'myspace') {
	    showThemeOptions(1);
	  } else {

	    Element.show($('themePictures'));
	    Element.show($('customizeTheme'));
	    if (ENABLE_THEME_BROWSER) {
	        Element.show($('theme-action-tabs'));
	    }else{
	        Element.show($('themeCategories'));
	    }
	    Element.show($('themesForward'));

	  }

	}

        if (activePage == "elements") {
          showEvent('tab_edit');
        } else if (activePage == "pages") {
          showEvent('tab_pages');
        } else if (activePage == "themes") {
          showEvent('tab_themes');
        } else if (activePage == "settings") {
          showEvent('tab_settings');
        }


    }

    function selectThemeConfigCategory(activeCategory) {

        var categoriesLevel1 = ['tc_popular', 'tc_colors', 'tc_text_styles', 'tc_layout'];

	$('focusMe').focus();

        for(var i = 0; i < categoriesLevel1.length; i++) {
          $(categoriesLevel1[i]).className = "elements_category";
        }

        if(activeCategory != 'none') { $(activeCategory).className = "elements_category_selected"; }

	var themeChildNodes = $('themeOptions').childNodes;
	for (var x=0; x<themeChildNodes.length; x++) {
	  if (!Element.hasClassName(themeChildNodes[x],activeCategory.replace(/^tc_/,''))) {
	    themeChildNodes[x].style.display = 'none';
	  } else {
	    themeChildNodes[x].style.display = 'block';

	    // Adjust scroll on dropdowns
	    var dropdowns = document.getElementsByClassName('weeblyDropDown', themeChildNodes[x]);
	    if (dropdowns.length > 0) {
	      var tmpId = dropdowns[0].firstChild.id.replace(/[^0-9]/g, "");
	      Weebly.DropDowns.dropdownsRef[tmpId].adjustScroll();
	    }
	  }
	}

    }

    /*** Utility Functions ***/
/*******************************/

  shortcut.add("Ctrl+B", function() { runCommand(currentBox, 'Bold', null); });
  shortcut.add("Ctrl+U", function() { runCommand(currentBox, 'Underline', null); });
  shortcut.add("Ctrl+I", function() { runCommand(currentBox, 'Italic', null); });
  function showAbout() {

        showTip("Thanks for using Weebly!", $('weebly_tab_edit'), 'y', '101');
        setTimeout("hideTip('tip101'); showTip('Weebly was created by David Rusenko, Dan Veltri, and Chris Fanini.', $('weebly_tab_themes'), 'y', '102');", 4000);
        setTimeout("hideTip('tip102'); showTip('We are!', $('weebly_tab_pages'), 'y', '103');", 8000);
        setTimeout("hideTip('tip103'); showTip('Penn State!<br/><br/><img src=\"http://rusenko.weebly.com/drusenko/pennstate.jpg\" width=\"300\" height=\"220\"/>', $('weebly_tab_settings'), 'w', '104');", 14000);
        setTimeout("hideTip('tip104');", 20000);

  }

    function showPublishingAnimation() {

	publishingAnim = 1;
	publishingAnimation();

        var newHeight = getInnerHeight();

	//Element.setStyle($('publishingWait'), {top: (Position.realOffset(window)[1]+40)+'px'});
	//Element.setStyle($('grayedOut'), {height:(newHeight-35)+'px', display:'block', visibility:'visible'} );

	//Effect.Center('publishingWait');
	//load the publising window
	
	$('chooseAddress1').src = 'http://'+editorStatic+/*tli(*/"/weebly/images/publishing-site.jpg"/*)tli*/;
    if(Weebly.Restrictions.hasAccess('ftp_publish')){
        $('exportText').innerHTML = "<div style=\"text-align: center;\"><img src=\"http://"+editorStatic+"/weebly/images/ajax-loader.gif\"/><br/><br/>"+/*tl(*/"Please be patient. It may take a few minutes to update your site."/*)tl*/+"</div><br/><br/></div>";
    }
    else{
        $('exportText').innerHTML = "<div style=\"text-align: center;\"><img src=\"http://"+editorStatic+"/weebly/images/ajax-loader.gif\"/><br/><br/>"+/*tl(*/"Please wait while we publish your site."/*)tl*/+"</div><br/><br/></div>";
    }
	Element.show('tip14');
/**
	if (settingAnimations == 1) {
          Effect.Appear($('publishingWait'), { duration: 0.5 });
        } else {
          Element.setStyle($('publishingWait'), {display:'block', opacity:'1', visibility:'visible'} );
        }
**/
	

    }
    function hidePublishingAnimation() {

       publishingAnim = 0;

       if (settingAnimations == 1) {
         window.setTimeout("Effect.Fade('publishingWait')", 1000);
         window.setTimeout("Effect.Fade('grayedOut')", 2200);
       } else {
         Element.hide('grayedOut');
         Element.hide('publishingWait');
       }

    }
    function publishingAnimation() {

	var moveX; var moveY;

	if (publishingLoc == 'left') { 
	  publishingLoc = 'right';
	  for(x=1; x<11; x++) {
	    moveX = 30; moveY = 150;
	    setTimeout("new Effect.Move('publishing"+x+"', { x: "+moveX+", y: "+moveY+", mode: 'absolute' })", 100*x);
	  }
	} else {
	  publishingLoc = 'left';
          for(x=1; x<11; x++) {
            moveX = 470; moveY = 150;
            setTimeout("new Effect.Move('publishing"+x+"', { x: "+moveX+", y: "+moveY+", mode: 'absolute' })", 100*x);
	  }
	}

	if (publishingAnim == 1) { setTimeout("publishingAnimation()", 2000); }


    }

    function expandAdvanced() {

	$("propertiesAdvancedHeader").innerHTML = "<br/><h3><a href=\"#\" onClick=\"collapseAdvanced();\" style=\"color: black;\"><img border=\"0\" src=\"http://"+editorStatic+"/weebly/images/arrow_down.gif\"/> "+/*tl(*/"Advanced Properties"/*)tl*/+"</a></h3>";
	Element.setStyle('propertiesAdvanced', {display:'block'} );

    }
    function collapseAdvanced() {

        Element.setStyle('propertiesAdvanced', {display:'none'} );
	$("propertiesAdvancedHeader").innerHTML = "<br/><h3><a href=\"#\" onClick=\"expandAdvanced();\" style=\"color: black;\"><img border=\"0\" src=\"http://"+editorStatic+"/weebly/images/arrow_right.gif\"/> "+/*tl(*/"Advanced Properties"/*)tl*/+"</a></h3>";

    }
 
    function showProperties(topPos) {

      topPos = topPos ? topPos : 145;

      $('textEditor').style.top = topPos+'px';
      //Element.setStyle('textEditor', { top:Position.realOffset(window)[1]+topPos+'px'} );

      //$('scroll_container').style.overflowY = 'hidden';
      Element.show('scroll_container_properties');

      if (settingAnimations == 1) {
         window.setTimeout("Effect.Appear('textEditor')", 1000);
      } else {
         Element.show('textEditor');
      }


    }

    function fadeProperties() {

       //$('scroll_container').style.overflowY = 'scroll'; 
       Weebly.Linker.close();
       Element.hide('scroll_container_properties');

    }

    function fadeMain() {

      Element.setStyle('grayedOut', {height:(getInnerHeight()-35)+'px'} );

      if (settingAnimations == 1) {
        Effect.Appear('grayedOut');
      } else {
        Element.show('grayedOut');
      }

    }

    function showMain() {

       if (settingAnimations == 1) {
         setTimeout("Effect.Fade('grayedOut')", 1000);
	 Element.setStyle('grayedOut', { visibility: 'visible'});
       } else {
         Element.hide('grayedOut');
       }

    }

    function displayThemes() {

      Element.setStyle('themes', {top:Position.realOffset(window)[1]+30+'px'} );

       if (settingAnimations == 1) {
         Effect.Appear('themes');
	 Effect.Appear('themeChoices', { duration: 0.5 });
       } else {
         Element.show('themes');
	 Element.show('themeChoices');
       }

      currentThemesPage = 1;

    }

    function fadeThemes() {

       if (settingAnimations == 1) {
         window.setTimeout("Effect.Fade('themes')", 1000);
       } else {
         Element.hide('themes');
       }

       Element.setStyle('nextThemes', {display:'block'} );
       Element.setStyle('previousThemes', {display:'none'} );

       currentThemesPage = 0;

    }

    function selectHeader(go) {

	if (!go) { setTimeout("selectHeader(1)", 50); }
	else {
	  if (!headerSelected) {
	    var headerNode = document.getElementsByClassName('weebly_header')[0];

	    headerDimensions = [Element.getStyle(headerNode, 'height').replace(/px/,''), Element.getStyle(headerNode, 'width').replace(/px/,'')];
	    headerNode.style.height = (headerDimensions[0]-2) + "px";
	    headerNode.style.width  = (headerDimensions[1]-2) + "px";
	    headerNode.style.border = "1px dashed #4455aa";

	    selectUpload("header", "header:"+currentTheme+"-resize");

            var menuBarDivHTML = "<table id='menuBarItemContainer' spacing=0 padding=0><tr>";
            menuBarDivHTML += "<td style='position: relative; background: none;'><div id='menubar-l' style='position: absolute; width: 5px; height: 44px; top: 0px; margin-left: -10px; background: url(http://"+editorStatic+"/weebly/images/menubar-l.gif) no-repeat bottom left;'></div> </td>";

	    menuBarDivHTML += "<td><span class='menuBarSpan'><b>"+/*tl(*/"New Header Image"/*)tl*/+"</b><a id='newImage' href='#' onClick='selectUpload(\"header\", \"header:"+currentTheme+"-resize\"); return false;' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload"/*)tl*/+"</a></span></td>";
	    if(currentHeader && currentHeader.match(/\./)) {
	      menuBarDivHTML += "<td><span class='menuBarSpan'><b>Header Image Size</b><div style=\"font-size: 12px; background: none; padding-top: 4px; display: block;\"><b>"+(headerNode.style.width.replace("px", "")-(-2)+"px")+" x "+(headerNode.style.height.replace("px", "")-(-2)+"px")+"</b></div></span></td>";
	      menuBarDivHTML += "<td style='background: none;'><span class='menuBarSpan'><b>"+/*tl(*/"Remove Header Image"/*)tl*/+"</b><a id='removeImage' class='menuIconLink' href='#' onClick='removeHeader(); return false;'><img src='http://"+editorStatic+"/weebly/images/action_stop.gif' class='menuIconImage' style='margin-right: 2px; top: 4px;' alt='New Image Icon'/> "+/*tl(*/"Remove"/*)tl*/+"</a></span></td>";
	    } else {
	      menuBarDivHTML += "<td style='background: none;'><span class='menuBarSpan'><b>Header Image Size</b><div style=\"font-size: 12px; background: none; padding-top: 4px; display: block;\"><b>"+(headerNode.style.width.replace("px", "")-(-2)+"px")+" x "+(headerNode.style.height.replace("px", "")-(-2)+"px")+"</b></div></span></td>";
	    }
	    menuBarDivHTML += "<td style='position: relative; background: none;'><div id='menubar-r' style='position: absolute; width: 5px; height: 44px; top: 0px; margin-left: 10px; background: url(http://"+editorStatic+"/weebly/images/menubar-r.gif) no-repeat bottom left;'></div> </td>";
	    menuBarDivHTML += "</tr></table>";

	    $("menuBarDiv").innerHTML = menuBarDivHTML;

	    Weebly.Elements.showMenuBar();
	    headerSelected = 1;
	  } else if (headerSelected == 2) {
	    headerSelected = null;
	  }
	}

    }

    function unselectHeader(preempt) {
	
	if (headerSelected) {
	  var headerNode = document.getElementsByClassName('weebly_header')[0];
	  if (headerNode && headerNode.style && headerDimensions && headerDimensions[0]) { 
	    headerNode.style.border = 'none';
	    headerNode.style.height = (headerDimensions[0]) + "px";
	    headerNode.style.width  = (headerDimensions[1]) + "px";
	    headerDimensions 	  = Array();	    
	  }

	  hideFlashContainer();

	  Weebly.Elements.hideMenuBar();
	  $('menuBarDiv').innerHTML = '';
	  headerSelected = null;
	}
	if (preempt) { headerSelected = 2; }

    }

    function removeHeader() {
	unselectHeader();
	new Ajax.Request(ajax, {parameters:'pos=removeheader&cookie='+document.cookie, 'onSuccess' : handlerRemoveHeader, 'onFailure':errFunc});
	currentHeader = null;
    }

    function handlerRemoveHeader(t) {
	if (t.responseText == 1) {
	  currentStyleNum = Math.floor(Math.random()*10000000001);
	  writeTheme(currentTheme);
	}
    }

    function editCustomHTML(ucfid, cfpid, ucfpid) {

	var ucf 	= $(""+ucfid);
	var ucfParentNode  = ucf.parentNode;
	ucf.style.display = 'none';

	var ucfp 	= $(""+ucfpid+"CustomHTML");
	var ucfpParentNode = ucfp.parentNode;

	// Fill in the text box
	var ucfFrame = $("scriptInclude"+ucfid);
	if (ucfFrame && ucfFrame.contentWindow && ucfFrame.contentWindow.document && ucfFrame.contentWindow.document.getElementsByTagName('body')[0] && ucfFrame.contentWindow.document.getElementsByTagName('body')[0].childNodes[1]) {
	  if (navigator.appVersion.indexOf("MSIE") == -1) {
	    ucfp.value = ucfFrame.contentWindow.document.getElementsByTagName('body')[0].childNodes[1].innerHTML;
	  } else {
	    ucfp.value = ucfFrame.contentWindow.document.getElementsByTagName('body')[0].childNodes[0].innerHTML;
	  }
	}
  if( Weebly.Elements.customHTML[ucfpid] )
  {
    ucfp.value = Weebly.Elements.customHTML[ucfpid];
  }
	
	var dummyNode = document.createElement('div');
	dummyNode.style.display = "none";
	dummyNode.id		= "customHTMLDummy";

	ucfpParentNode.appendChild(dummyNode);
	ucfParentNode.appendChild(ucfp);
	ucfp.style.display = 'block';
	ucfp.focus();
	ucfp.select();

	Weebly.Elements.editing = cfpid;

    }

    function hideCustomHTML(ucfid, cfpid, ucfpid) {

	if (!Weebly.Elements.editing) return;
	Weebly.Elements.editing = null;

        var ucf         = $(""+ucfid);
        var ucfpParentNode  = $('customHTMLDummy').parentNode;

        var ucfp        = $(""+ucfpid+"CustomHTML");
        var ucfParentNode = ucfp.parentNode;

        ucfp.style.display = 'none';
        ucf.style.display = 'block';
        ucfpParentNode.appendChild(ucfp);
        ucfpParentNode.removeChild($('customHTMLDummy'));

	//ucfp.value = ucfp.value.replace(/<!--.*?-->/ig, "");

	new Ajax.Request(ajax, {parameters:'pos=savecustomhtml&ucfid='+ucfid+'&ucfpid='+ucfpid+'&customhtml='+encodeURIComponent(ucfp.value)+'&cookie='+document.cookie, onSuccess:handlerHideCustomHTML, onFailure:errFunc, bgRequest: true});
  
  //ucfp.value = ucfp.value.escapeHTML();

	Weebly.Elements.customHTML[ucfpid] = ucfp.value;
	Weebly.Elements.onChange(ucfpid, cfpid);

    }

    function handlerHideCustomHTML(t) {
    }

    function hideElement(elementID) {
	Element.hide(elementID);
    }

    function showElement(elementID) {
	Element.setStyle(elementID, {display:'block', visibility:'visible'} );
    }

    function validateOK(element_id) {

	Weebly.Elements.continueOnChange();
	//saveProperties('properties',element_id);

    }

    function validateNoGo(errorMsg) {

	//Define later
    }

    function contentDecode(str) {

	str = str.replace(new RegExp('\\+','g'),' ');
    	return unescape(str);

    }

    function contentEncode(str) {

    	str = escape(str);
    	str = str.replace(new RegExp('\\+','g'),'%2B');
    	return str.replace(new RegExp('%20','g'),'+');

    }

    function showUploader(size) {

	Element.hide('uploadSize');
	Element.show('imageUploader');
	imageUploadSize = size;

    }

    function getScrollTop() {

	var y;
	if (self.pageYOffset) // all except Explorer
	{
	  y = self.pageYOffset;
	}
	else if (document.documentElement && document.documentElement.scrollTop)
	// Explorer 6 Strict
	{
	  y = document.documentElement.scrollTop;
	}
	else if (document.body) // all other Explorers
	{
	  y = document.body.scrollTop;
	}

	return y;

    }

  function getThemeConfig(firstTime) {

	new Ajax.Request(ajax, {parameters: 'pos=getthemeconfig&theme='+currentTheme+'&cookie='+document.cookie, 'onSuccess': function(t) { handlerGetThemeConfig(t, firstTime); }, 'onFailure':errFunc});
	
  }

  function handlerGetThemeConfig(t, firstTime) {

	$('currentThemeOptions').innerHTML = t.responseText;
	selectThemeConfigCategory('tc_popular');

	// Create dropdowns here
        var elementList = document.getElementsByClassName('dropDownData', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
	  var el = elementList[x];
	  var updateFunc = function() { };
	  if ( $(el.nextSibling.name+"-updateFunction") && $(el.nextSibling.name+"-updateFunction").value) {
	    eval("updateFunc = "+$(el.nextSibling.name+"-updateFunction").value+";");
	  }
	  new Weebly.DropDown(el.nextSibling, {openWidth: 95, rowMargin: '3px 0 0 0', rowFunction: function(x) { return generateDropDownRow(x, el.innerHTML); }, noRefresh: 1, onClose: saveThemeConfig, updateFunction: updateFunc});
	}

	// Create font drop down
        var elementList = document.getElementsByClassName('fontDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 150, rowMargin: '0px 0 0 0', rowFunction: generateFontsWebsafeRows, noRefresh: 1, onClose: saveThemeConfig, rowHoverColor: '#FFFFFF'});
        }

        // Create border drop down
        var elementList = document.getElementsByClassName('borderDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 279, openHeight: 320, height: 70, width: 105, marginRight: 25, scaleBy: 40, rowMargin: '5px 0 0 2px', rowFunction: generateBorderStyleRows, onClose: saveThemeConfig, rowHoverColor: '#FFFFFF', overflowY: 'scroll'});
        }

        // Create center drop down
        var elementList = document.getElementsByClassName('centerDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 279, height: 70, width: 105, marginRight: 25, scaleBy: 40, rowMargin: '5px 0 0 2px', rowFunction: generateCenterStyleRows, onClose: saveThemeConfig, rowHoverColor: '#FFFFFF', overflowY: 'scroll'});
        }

        // Create color drop down
        var elementList = document.getElementsByClassName('colorDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 220, openHeight: 123, generateValueFunction: colorValueFunction, generateContentsFunction: colorContentsFunction, rowMargin: '0px 0 0 0', onClose: saveThemeConfig, overflowY: 'scroll'});
        }

        // Create image drop down
        var elementList = document.getElementsByClassName('imageDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 540, openHeight: 330, height: 70, width: 105, marginRight: 25, generateValueFunction: imageValueFunction, generateContentsFunction: imageContentsFunction, rowMargin: '0px 0 0 0', onOpen: function() { if (lastTab == "imgSearch") { searchImages(1); } else { selectImageTab($("imgPattern")); } }, onClose: saveThemeConfig, overflowY: 'auto', scaleBy: 60, rowHoverColor: '#FFFFFF', updateFunction: function(val) { $('icontent_container').style.backgroundImage = $('%%BGIMAGE%%').nextSibling.childNodes[1].firstChild.firstChild.style.backgroundImage.replace(/_s\./, "."); }, showTabs: {'search': 1, 'patterns': 1, 'upload': 1, 'remove': 1} });
        }

        // Create image upload drop down
        var elementList = document.getElementsByClassName('imageUploadDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 340, openHeight: 330, height: 70, width: 105, marginRight: 25, generateValueFunction: imageValueFunction, generateContentsFunction: imageContentsFunction, rowMargin: '0px 0 0 0', onOpen: function() { selectImageTab($("imgUpload")); }, onClose: saveThemeConfig, overflowY: 'auto', scaleBy: 60, rowHoverColor: '#FFFFFF', updateFunction: function(val) { $('icontent_container').style.backgroundImage = $('%%BGIMAGE%%').nextSibling.childNodes[1].firstChild.firstChild.style.backgroundImage.replace(/_s\./, "."); }, showTabs: {'upload': 1, 'del': 1} });
        }

	// Dispose of old sliders
	for (var slider in Weebly.sliders) {
	  Weebly.sliders[slider].dispose();
	}

	// Create new sliders
	var elementList = document.getElementsByClassName('sliderData', $('themeOptions'));
	Weebly.sliders = {};
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
	  var data = {};
	  eval("data = "+el.innerHTML+";");
	  var updateFunc = function() { };
          if ( $(el.nextSibling.name+"-updateFunction") && $(el.nextSibling.name+"-updateFunction").value) {
            eval("updateFunc = "+$(el.nextSibling.name+"-updateFunction").value+";");
          }
	  var updateEl = el.nextSibling.name;
	  var newSlider = new Control.Slider($(el.nextSibling.name+'-handle'),$(el.nextSibling.name+'-track'), { element: updateEl, updateFunction: updateFunc, startValue: data.startValue, endValue: data.endValue, sliderValue: ($(el.nextSibling).value-data.startValue)/(data.endValue-data.startValue), onSlide: function(value) { value = value*(this.endValue-this.startValue)-(-this.startValue); $(this.element).value = value; this.updateFunction(value); }, onChange: function() { saveThemeConfig(2); } });
	  Weebly.sliders[$(el.nextSibling.name+'-handle')] = newSlider;
	  
        }

	// Create palette drop down
	new Weebly.DropDown($('weeblyPalette'), {width: 165, openWidth: 165, openHeight: 128, marginRight: 10, rowFunction: generatePaletteRows, rowMargin: '0 0 0 1px', onClose: saveThemeConfig, updateFunction: paletteUpdateFunction, noInitialUpdate: true });

	if (firstTime) { saveThemeConfig(0,1); }

	/**
	if (t.responseText.match("This theme is not configurable.")) {
	  $('currentThemeOptions').innerHTML = t.responseText;
	  return;
	}

	var themeRoot = getXML(t.responseText);
	var optionsRoot = themeRoot.getElementsByTagName('theme')[0].getElementsByTagName('options')[0];

	$('currentThemeOptions').innerHTML = '';
	for (var x=0; x<optionsRoot.childNodes.length; x++) {
	  var currentNode = optionsRoot.childNodes[x];
	  if (currentNode.tagName == "option") {
	    $('currentThemeOptions').innerHTML += "<div style='float: left; height: 40px; font-size: 11px; font-weight: bold; min-width: 105px; width: auto !important; width: 105px; padding: 5px 5px 0 5px;'>" + getValueXML(currentNode,'name') + "<br/><i style='font-weight: normal;'>" + getValueXML(currentNode,'property') + "</i></div>";
	  }
	}
	**/

	showThemeOptions(); 
	setTimeout("showEvent('showThemeOptions', 0, $('weeblyPalette').nextSibling.childNodes[1]);", 1500);

  }

  function paletteUpdateFunction(val) {

	currentPalette = val; 
	var elementList = document.getElementsByClassName('colorDropDown', $('themeOptions')); 
	for (var x=0; x < elementList.length; x++) { 
	  if (elementList[x].nextSibling.id == "%%BGCOLOR%%") {
	    elementList[x].nextSibling.value = "#1";
          } else if (elementList[x].nextSibling.id == "%%CENTERBORDERCOLOR%%") {
            elementList[x].nextSibling.value = "#2";
          } else if (elementList[x].nextSibling.id == "%%HEADERCOLOR%%") {
            elementList[x].nextSibling.value = "#2";
	  } else if (elementList[x].nextSibling.id == "%%PAGECOLOR%%") {
            elementList[x].nextSibling.value = "#3";
          } else if (elementList[x].nextSibling.id == "%%BOXCOLOR%%") {
            elementList[x].nextSibling.value = "#4";
          } else if (elementList[x].nextSibling.id == "%%BORDERCOLOR%%") {
            elementList[x].nextSibling.value = "#5";
          } else if (elementList[x].nextSibling.id == "%%LEAVESCOLOR%%") {
            elementList[x].nextSibling.value = "#6";
          } else if (elementList[x].nextSibling.id == "%%CONTACTCOLOR%%") {
            elementList[x].nextSibling.value = "#7";
          } else if (elementList[x].nextSibling.id == "%%FONTCOLOR%%") {
            elementList[x].nextSibling.value = "#8";
	  }
	  var thisDropDown = Weebly.DropDowns.dropdownsRef[elementList[x].nextSibling.nextSibling.firstChild.id.replace(/[^0-9]/g, '')]; 
	  $(thisDropDown.id+"-value").innerHTML = thisDropDown.options.generateValueFunction(thisDropDown); 
	  thisDropDown.options.generateContentsFunction(thisDropDown); 
	}

  }

  function colorValueFunction(obj) {

	var curValue = obj.formEl.value;
	if (!curValue.match(/^#[a-fA-F0-9]{6}$/)) {
	  curValue = colorPalettes[currentPalette][curValue.replace(/[^0-9]/, '')-1];
	}
	return "<div style='height: 21px; width: 74px; margin: 1px; background: "+curValue+";'></div>";

  }

  function colorContentsFunction(obj) {

	var returnHTML = '<div style="padding: 5px 5px 0 5px;">';
	returnHTML += '<div style="font-size: 11px; font-weight: bold; font-family: arial, verdana, sans-serif; border-bottom: 1px solid #ccc; margin-bottom: 5px;">'+/*tl(*/'Choose a color'/*)tl*/+':</div>';
	thisPalette = colorPalettes[currentPalette];
	for (var y in thisPalette) {
	  returnHTML += '<div style="margin: 3px; height: 17px; width: 17px; font-size: 1px; background: '+thisPalette[y]+' url(http://'+editorStatic+'/weebly/images/palette-color.gif) no-repeat top left; float: left; cursor: pointer;" onclick="Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close(\'#'+(y-(-1))+'\');">&nbsp;</div>';
	}
	
	returnHTML += '<div style="clear: both; font-size: 11px; font-weight: bold; font-family: arial, verdana, sans-serif; border-bottom: 1px solid #ccc; margin-bottom: 5px; padding-top: 20px;">'+/*tl(*/'Or click to select more colors'/*)tl*/+':</div>';

	var curValue = obj.options.generateValueFunction(obj).replace(/.*background: #(.{6});.*/, "$1");
	returnHTML += '<div id="'+obj.id+'-colorpicker" style="margin: 5px 5px 5px 0; border: 1px solid #ccc; height: 20px; cursor: pointer;"></div>';
	returnHTML += '</div>';
	returnHTML += '<input type="hidden" id="'+obj.id+'-currentcolor" value="'+curValue+'"/>';
	returnHTML += '<input type="hidden" id="'+obj.id+'-initialcolor" value="'+curValue+'"/>';

	return returnHTML;

  }

  function imageValueFunction(obj) {

	var curValue = obj.formEl.value;
	if (curValue.match(/^pattern:/)) {
	  curValue = "%%THEMEIMG%%/bga.png?"+Math.floor(Math.random()*10000000001);
	}
	if (curValue.match(/jpe?g$/)) {
	  curValue = curValue.replace(/\.(jpe?g)$/, "_s.$1");
	} else {
	  curValue = curValue.replace(/%%THEMEIMG%%/, "http://www.weebly.com/weebly/render/users/"+userIDLocation+"/"+currentSite+"/");
	}
        return "<div style='height: 68px; width: 103px; margin: 1px; background: url("+curValue+");'></div>"

  }

  var lastSearch = 'forest night';
  var lastPatternsPage = 0;
  var lastTab = 'imgPattern';
  function imageContentsFunction(obj) {

	var returnHTML = '<div style="padding: 5px;">';
	if (obj.options.showTabs.search) {
	  returnHTML += '<div id="imgSearch" style="cursor: pointer; font-size: 10px; border-left: 1px solid #E9E9E9; border-right: 1px solid #E9E9E9; border-top: 2px solid #FF9900; padding: 2px 7px; float: left;" onclick="selectImageTab(this);">Image Search</div>';
	}
	if (obj.options.showTabs.patterns) {
	  returnHTML += '<div id="imgPattern" style="cursor: pointer; font-size: 10px; border-left: 1px solid #E9E9E9; border-right: 1px solid #FFFFFF; border-top: 2px solid #E9E9E9; background: #E9E9E9; padding: 2px 7px; float: left;" onclick="selectImageTab(this);">Pattern</div>';
	}
	if (obj.options.showTabs.upload) {
	  returnHTML += '<div id="imgUpload" style="cursor: pointer; font-size: 10px; border-left: 1px solid #E9E9E9; border-right: 1px solid #FFFFFF; border-top: 2px solid #E9E9E9; background: #E9E9E9; padding: 2px 7px; float: left;" onclick="selectImageTab(this);">Upload</div>';
	}
	if (obj.options.showTabs.remove || obj.options.showTabs.del) {
	  returnHTML += '<div id="imgNone" style="cursor: pointer; font-size: 10px; border-left: 1px solid #E9E9E9; border-right: 1px solid #FFFFFF; border-top: 2px solid #E9E9E9; background: #E9E9E9; padding: 2px 7px; float: left;" onclick="selectImageTab(this);">None</div>';
	}
	returnHTML += '<div style="clear: both; border-bottom: 1px solid #E9E9E9; height: 3px; overflow: hidden;"></div>';

	// Image search DIV
	if (obj.options.showTabs.search) {
	  returnHTML += '<div id="imgSearchBox">';
	  returnHTML += '<div style="margin-top: 5px; font-size: 11px; font-weight: bold; font-family: arial, verdana, sans-serif; border-bottom: 1px solid #E9E9E9; margin-bottom: 5px;">Search for an image:</div>';
	  returnHTML += '<input type="text" style="border: 1px solid #BCCDF0; padding: 3px;" id="imageSearchBox" value="'+lastSearch+'"/><input type="submit" style="padding: 2px; margin-left: 10px; margin-right: 10px;" value="Search" onclick="searchImages(1); return false;"/><span id="imageSearchError" style="color: red;"></span>';
	  returnHTML += '<div style="height: 300px;" id="imageSearchReturn"></div>';
	  returnHTML += '</div>';
	}

        // Pattern search DIV
	if (obj.options.showTabs.patterns) {
          returnHTML += '<div id="imgPatternBox" style="display: none;">';
          returnHTML += '<div style="margin-top: 5px; font-size: 11px; font-weight: bold; font-family: arial, verdana, sans-serif; border-bottom: 1px solid #E9E9E9; margin-bottom: 5px;">Select a pattern:</div>';
	  returnHTML += '<div style="height: 300px;" id="imagePatternReturn"></div>';
          returnHTML += '</div>';
	}

        // Upload Image DIV
	if (obj.options.showTabs.upload) {
	  var display = (!obj.options.showTabs.search && !obj.options.showTabs.patterns) ? 'block' : 'none';
          returnHTML += '<div id="imgUploadBox" style="display: '+display+';">';
          returnHTML += '<div style="margin-top: 5px; font-size: 11px; font-weight: bold; font-family: arial, verdana, sans-serif; border-bottom: 1px solid #E9E9E9; margin-bottom: 5px;">Upload an image:</div>';
          returnHTML += '<input type="text" style="border: 1px solid #BCCDF0; padding: 3px;" id="imageSearchBox"/><input type="submit" style="padding: 2px; margin-left: 10px; margin-right: 10px;" value="Browse" onclick="selectUpload(\'bgImageUpload\', 2048, 0, 0); return false;"/><span id="imageSearchError" style="color: red;"></span>';
	  returnHTML += '<ul id="notificationsBgImage"></ul>';
          returnHTML += '</div>';
	}

        // No Image DIV
	if (obj.options.showTabs.remove) {
          returnHTML += '<div id="imgNoneBox" style="display: none;">';
          returnHTML += '<a style="display: block; margin: 15px 5px; font-size: 12px; font-weight: bold; font-family: arial, verdana, sans-serif;" href="#" onclick="Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close(\'%%THEMEIMG%%/bga.png\'); return false;"><img src="http://'+editorStatic+'/weebly/images/action_stop.gif" style="position: relative; top: 3px; border: none;"/> Remove background image</a>';
          returnHTML += '</div>';
	}

        // No Image DIV
        if (obj.options.showTabs.del) {
          returnHTML += '<div id="imgNoneBox" style="display: none;">';
          returnHTML += '<a style="display: block; margin: 15px 5px; font-size: 12px; font-weight: bold; font-family: arial, verdana, sans-serif;" href="#" onclick="Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close(\'\'); return false;"><img src="http://'+editorStatic+'/weebly/images/action_stop.gif" style="position: relative; top: 3px; border: none;"/> Remove image</a>';
          returnHTML += '</div>';
        }
	
	returnHTML += '</div>';

	return returnHTML;

  }

  function selectImageTab(tabName) {

	var imageTabDivs = ['imgSearch', 'imgPattern', 'imgUpload', 'imgNone'];

	for (var x=0; x<imageTabDivs.length; x++) {
	  if ($(imageTabDivs[x])) {
	    $(imageTabDivs[x]).style.background = "#E9E9E9";
	    $(imageTabDivs[x]).style.borderTop = "1px solid #E9E9E9";
	    $(imageTabDivs[x]).style.borderRight = "1px solid #FFFFFF";
	    $(imageTabDivs[x]+"Box").style.display = "none";
	  }
	}

	tabName = tabName.id;

	$(tabName+"Box").style.display = "block";
	$(tabName).style.background = "#FFFFFF";
	$(tabName).style.borderTop = "2px solid #FF9900";
	$(tabName).style.borderRight = "1px solid #E9E9E9";

	if (tabName == 'imgPattern') {
	  populatePatterns(lastPatternsPage);
	  lastTab = 'imgPattern';
	} else {
	  lastTab = 'imgSearch';
	}

  }

  function populatePatterns(start) {

	var myRet = '';

	for (var x=start; x<start+6; x++) {
	  
	  myRet += "<a href='#' onclick='Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close(\"pattern:"+themePatterns[x]+"\"); return false;' style='text-align: center; float: left; width: 160px; display: block; margin: 5px;'><div style='border: none; height: 123px; width: 165px; background: url(\"/weebly/themes/myspace-patterns/"+themePatterns[x]+"/"+themePatterns[x]+"\");'></div></a>\n";
	  if (x%3 == 0 && x != 0) {
	    //myRet += "<div style='width: 0px height: 0px; overflow: hidden; clear: both;'></div>";
	  }

	}

	myRet += "<div style='width: 0px height: 0px; overflow: hidden; clear: both;'></div>";
	myRet += "<div style='text-align: right; margin-right: 15px;'>";
	
	if (start != 0) {
	  myRet += "<a href='#' onclick='populatePatterns("+(start-6)+"); return false'>&lt; Back</a> | ";
	}
	if (start-(-6) < themePatterns.length) {
	  myRet += "<a href='#' onclick='populatePatterns("+(start-(-6))+"); return false'> More &gt;</a>";
	}

	myRet += "</div>";

	$('imagePatternReturn').innerHTML = myRet;
	lastPatternsPage = start;

  }

  function searchImages(start) {

	$("imageSearchError").innerHTML = "";
	lastSearch = $('imageSearchBox').value;
	new Ajax.Request(ajax, {parameters: 'pos=searchimages&query='+$('imageSearchBox').value+'&start='+start+'&cookie='+document.cookie, 'onSuccess': handlerSearchImages, 'onFailure':errFunc});

  }

  function handlerSearchImages(t) {

	$('imageSearchReturn').innerHTML = t.responseText;

  }

  function keepImage(url, ref) {

	$("imageSearchError").innerHTML = "";
	new Ajax.Request(ajax, {parameters: 'pos=keepimage&url='+encodeURIComponent(url)+'&ref='+encodeURIComponent(ref)+'&cookie='+document.cookie, 'onSuccess': handlerKeepImage, 'onFailure':errFunc});

  }

  function handlerKeepImage(t) {

	if (t.responseText.match(/Error/)) {
	  $('imageSearchError').innerHTML = t.responseText;
	} else {
	  t.responseText = t.responseText.replace(/\n/g, "");
	  t.responseText = t.responseText.replace(/\r/g, "");
	  Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close(t.responseText);
	}

  }

  function setThemeColor(el) {

	//console.log($(el));
	if ($(el).value != $(el).nextSibling.value) {
	  //console.log(Weebly.DropDowns.activeMenu);
	  Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close('#'+$(el).value);
	}

  }

  function generatePaletteRows(x) {
	
	var i=0;
	for (var z in colorPalettes) { if (colorPalettes.hasOwnProperty(z)) { i++; } }
	if (x+1 > i) { return; }

	var returnVar = '';
	var thisPalette = colorPalettes[x+1];
	for (var y in thisPalette) {
	  if (y>5) { break; }
	  returnVar += '<div style="margin: 3px; height: 17px; width: 17px; font-size: 1px; background: '+thisPalette[y]+' url(http://'+editorStatic+'/weebly/images/palette-color.gif) no-repeat top left; float: left;">&nbsp;</div>';
	}

	return [x+1, returnVar];

  }

  function generateFontsWebsafeRows(x) {

	var i=0;
	for (var y in fontsWebsafe) {
	  if (x == i) { return [y,'<img src="/weebly/images/fonts/'+fontsWebsafe[y]+'" alt="'+y+'"/>']; }
	  i++;
	}

  }

  function generateBorderStyleRows(x) {

	if (x < 6) { 
	  return [x+1,"<div style='height: 80px; width: 270px; overflow: hidden;'><img src='/weebly/themes/myspace-images/thumbnails/borders/"+(x+1)+".jpg' style='border: 0;'/></div>"];
	} else if (x == 6) {
	  return [0,"<div style='height: 80px; width: 270px; overflow: hidden;'><img src='/weebly/themes/myspace-images/thumbnails/borders/none.jpg' style='border: 0;'/></div>"];
	}

  }

  function generateCenterStyleRows(x) {

        if (x < 4) {
          return [x+1,"<div style='height: 80px; width: 270px; overflow: hidden;'><img src='/weebly/themes/myspace-images/thumbnails/center/"+(x+1)+".jpg' style='border: 0;'/></div>"];
        } else if (x == 4) {
          return [0,"<div style='height: 80px; width: 270px; overflow: hidden;'><img src='/weebly/themes/myspace-images/thumbnails/center/none.jpg' style='border: 0;'/></div>"];
        }

  }

  function generateDropDownRow(x, data) {

	eval("data = "+data+";");

	var i=0;
	for (var y in data) {
	  if (x == i) { return [y, data[y]]; }
	  i++;
	}

  }

  function saveThemeConfig(noImageRefresh, firstTime) {

	new Ajax.Request(ajax, {parameters: 'pos=savethemeconfig&firstTime='+firstTime+'&'+Form.serialize('themeOptions')+'&cookie='+document.cookie, 'onSuccess': function(t) { handlerSaveThemeConfig(t, noImageRefresh); }, 'onFailure':errFunc});

  }

  function handlerSaveThemeConfig(t, noImageRefresh) {
	
	if (t.responseText.match('Error')) { return; }

	if (!noImageRefresh) {
	  refreshThemeImages();
	}
	if (noImageRefresh != '2') {
	  //currentStyleNum = Math.floor(Math.random()*10000000001);
	  clearThemeImages();
	  setThemeStyle(currentTheme);
	}

  }

  function clearThemeImages() {

        $('icontent_container').style.backgroundImage = '';
        var elementList = document.getElementsByClassName('updateBackgroundImage', $('icontent'));
        for (var x=0; x < elementList.length; x++) {
          $(elementList[x]).style.backgroundImage = '';
        }

  }

  function refreshThemeImages() {

	if (siteType != 'myspace') { return; }

	currentImageNum = Math.floor(Math.random()*10000000001);
	refreshBackgroundImage('icontent_container');
	var elementList = document.getElementsByClassName('updateBackgroundImage', $('icontent'));
	for (var x=0; x < elementList.length; x++) {
	  refreshBackgroundImage(elementList[x]);
	}

	// Update image setting boxes
        var elementList = document.getElementsByClassName('imageDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var thisDropDown = Weebly.DropDowns.dropdownsRef[elementList[x].nextSibling.nextSibling.firstChild.id.replace(/[^0-9]/g, '')];
          $(thisDropDown.id+"-value").innerHTML = thisDropDown.options.generateValueFunction(thisDropDown);
          thisDropDown.options.generateContentsFunction(thisDropDown);
        }

  }

  function refreshBackgroundImage(element) {

	element = $(element);
	var currentBackground = Element.getStyle(element, 'background-image');
	if (currentBackground == "none") { return; }
	currentBackground = currentBackground.replace(/url\(([^\)]+)\)/, "$1");
	currentBackground = currentBackground.replace(/"$/, "");
	currentBackground = currentBackground.replace(/^"/, "");
	currentBackground = currentBackground.replace(/^([^\?]+).*$/, "url('$1?"+currentImageNum+"')");
	Element.setStyle(element, {backgroundImage: currentBackground});
	//$(element).style.backgroundImage = currentBackground;

  }

  function setProperty(property, classNames, value) {

	// If it's only one class name
	if (typeof(classNames) == "string") {
	  // Set the property of all elements given that class name
	  document.getElementsByClassName(classNames).each( function(el) { $(el).style[property] = value; });
	// Else, if it's an array of class names
	} else if (typeof(classNames) == "object") {
	  for (var x=0; x < classNames.length; x++) {
	    // Call setOpacity on each individual class name
	    setProperty(property, classNames[x], value);
	  }
	}
  }

  function makePro() {

	$('editorLogo').src = 'http://'+editorStatic+'/weebly/images/newui/weebly-editor-logo-pro.jpg';

        hideAllTips();
        showTip(/*tl(*/"Thanks for signing up for a Weebly pro account! We hope you enjoy the extra features, and thank you for supporting us."/*)tl*/, $('editorLogo'), 'y');

	swfu.setFileSizeLimit(Math.floor(Weebly.Restrictions.accessValue('upload_limit_pro')/1000));

	updateElements();
    Weebly.Restrictions.addService(Weebly.Restrictions.proLevel);

  }
  
  function showElementOptions(pageElementID)
  {
    var el = $('options'+pageElementID);
    var offset = el.viewportOffset();
    var outerBox = new Element('div', {'id':'dropdown'+pageElementID,'class':'drop-down-options'}).setStyle( {top:(offset.top+5)+'px', left:(offset.left+20)+'px'} );
    var innerBox = new Element('div').setStyle( {'background':'#4485c9', 'color':'#FFFFFF', 'padding':'1px'} );
    innerBox.update('<img onclick="removeElementOptions(\'dropdown'+pageElementID+'\')" style="float:right; margin:1px; cursor:pointer;" src="http://'+editorStatic+'/weebly/images/close-options.jpg" /><div style="padding:8px 0px 7px 6px">Move to page:</div>');
    var optionsBox = new Element( 'ul' );
    var optionsCount = 0;
    $H(sitePages).each(
      function(pair){
      	// ashaw
      	// since PageManager refactoring, this hasn't been tested (couldn't figure out how to execute this shit)
      	//   old code: $H(blogPages).get(pair.key) === '0'
        if( pair.key != currentPage && !Weebly.PageManager.pages[pair.key].blog )
        {
            var pageName = pair.value.length > 15 ? pair.value.substr(0, 12)+'...' : pair.value;
            var li = new Element( 'li', {'id':'moveTo'+pair.key} );
            li.update( pageName );
            li.observe( 'click',
              function(e)
              {
                $('dropdown'+pageElementID).remove();
                var element = Event.element(e);
                new Ajax.Request(ajax, {parameters: 'pos=moveelement&pageElementID='+pageElementID+'&pageID='+element.id.replace('moveTo', '')+'&cookie='+document.cookie, 'onSuccess': handlerMovePageElement, 'onFailure':errFunc});
              } 
            );
            optionsBox.insert({'bottom':li});
            optionsCount++;
        }
      }
    );
    if(optionsCount > 0)
    {
        innerBox.insert({'bottom':optionsBox});
    }
    else
    {
        innerBox.insert({'bottom':'<ul><li>No destination pages available.</li></ul>'});
    }
    outerBox.insert(innerBox);
    $('body').insert( {'bottom':outerBox} );
  }

  function removeElementOptions(id)
  {
      if($(id)){
          $(id).remove();
      }
  }
  
  function handlerMovePageElement(t)
  {
    var pageID = t.responseText.replace('\n', '' );
    if( $H(sitePages).get(pageID) )
    {
        noJump = 1;
        goUpdateList(pageID, 1);
    }
  }
  
  function copyElement(pageElementID){
      new Ajax.Request(ajax, {parameters: 'pos=copyelement&pageElementID='+pageElementID+'&pageID='+currentPage+'&cookie='+document.cookie, 'onSuccess':function(){updateList(currentPage)},'onFailure':errFunc});
  }

  function saveSiteTitle() {

	new Ajax.Request(ajax, {parameters: 'pos=sitetitle&newtitle='+encodeURIComponent($('newSiteTitle').value)+'&cookie='+document.cookie, 'onSuccess': handlerSaveSiteTitle, 'onFailure':errFunc});
	$('weebly_site_title').innerHTML = $('newSiteTitle').value;

  }

  function handlerSaveSiteTitle(t) {

	if (!settingQuickExport && tempUser != 1) {
	  Pages.go('initialDomainMenu');
	} else {
	  Pages.go('main');
	}
	//Pages.go('editMenu');

  }

  var facebook_js_loaded = false;
  function load_feed() {
    fb_window = window.open('facebook_update.php?site_id='+currentSite, 'facebook_connect', "menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes,width=700,height=500");
    setTimeout("fb_window.focus()", 500);

  } 

  function load_twitter() {

	$('twitterMessage').value = "I just updated my "+Weebly.PROPERTY_NAME+" website, check it out: "+currentSiteLocation;
	$('twitter-publish').style.display = 'block';

  }

  function tweet() {

	new Ajax.Request(ajax, {parameters: 'pos=tweet&username='+encodeURIComponent($('twitterUser').value)+'&password='+encodeURIComponent($('twitterPass').value)+'&message='+encodeURIComponent($('twitterMessage').value)+'&cookie='+document.cookie, 'onSuccess': handlerTweet, 'onFailure':errFunc});

  }

  function handlerTweet(t) {

	$('twitter-publish').style.display = 'none';

  }

  function load_email() {
        new Ajax.Request(ajax, {parameters:'pos=doevent&event=tellFriendsPublish&cookie='+document.cookie});
	var message = "I just updated my website, "+$('weebly_site_title').innerHTML+", and thought you'd like to check it out. You can get to the site by visiting: \n\n"+currentSiteLocation+"\n\n"+Weebly.EMAIL_FOOTER;
        abookwin = window.open('/weebly/apps/abook.php?type=update&message='+encodeURIComponent(message), 'abook', 'toolbar=0,status=0,width=700,height=526');
        setTimeout( 'abookwin.focus()', 300 );
  }

  var faviconUploading = false;
  function uploadFaviconStart(){
      if(!faviconUploading && $('favicon-path').value.length > 0){
          var iframe = new Element('iframe',{'id':'favicon-iframe','name':'favicon-iframe', 'src':'about:blank'}).setStyle({'display':'none'});
          iframe.observe('load', uploadFaviconFinish);
          $('body').insert({'bottom':iframe})
          $('favicon-loading').show();
          $('favicon-display').show();
          faviconUploading = true;
          return true;
      }
      return false;
  }

  function uploadFaviconFinish()
  {
      var result = $('favicon-iframe').contentWindow.document.body.innerHTML;
      var data = result.evalJSON();
      if(data.status === 'success'){
          $('current-favicon').show().setAttribute('src',data.imagePath+'?'+Math.floor(Math.random()*10000000001));
          $('remove-favicon').show();
          $('favicon-upload-area').hide();
      }
      else{
          $('favicon-display').hide();
          alert(data.message);
      }
      $('favicon-iframe').remove();
      $('favicon-loading').hide();
      faviconUploading = false;
  }

  function removeFavicon(){
      new Ajax.Request(ajax, {parameters:'pos=removefavicon&cookie='+document.cookie, onSuccess:function(){$('favicon-display').hide();$('remove-favicon').hide();}, 'onFailure':errFunc});
  }

  function redrawProduct(){
      var pID = Weebly.Elements.pfield[Weebly.Elements.ucfid][69650731].ucfpid;
      var currentDisplayStyle = Weebly.Elements.pfield[Weebly.Elements.ucfid][69650731].propertyresult;
      var newDisplayStyle = $(''+pID) ? $(''+pID).value : null;
      var hideImage = $(''+Weebly.Elements.pfield[Weebly.Elements.ucfid][94961376].ucfpid).value;
      var hideDesc = $(''+Weebly.Elements.pfield[Weebly.Elements.ucfid][36726176].ucfpid).value;
      var buttonStyle = $(''+Weebly.Elements.pfield[Weebly.Elements.ucfid][45179529].ucfpid).value;
      var currentButtonStyle = Weebly.Elements.pfield[Weebly.Elements.ucfid][45179529].propertyresult;
      var align = $(''+Weebly.Elements.pfield[Weebly.Elements.ucfid][90426618].ucfpid).value;
      var ucfid = Weebly.Elements.ucfid;
      if(newDisplayStyle && (currentDisplayStyle !== newDisplayStyle)){
          var image = $(''+Weebly.Elements.ucfid).down('.product-image').src;
          new Ajax.Request(ajax, {parameters:'pos=redrawproduct&productID='+Weebly.Elements.getCurrentProductID()+'&ucfid='+ucfid+'&image='+encodeURIComponent(image)+'&displayStyle='+newDisplayStyle+'&hideImage='+hideImage+'&hideDescription='+hideDesc+'&buttonStyle='+buttonStyle+'&cookie='+document.cookie, onSuccess:function(t){$(''+ucfid).update(t.responseText); makeProductEditable(ucfid);}, 'onFailure':errFunc});
      }
      if(hideImage == 0){
          $(''+ucfid).down('.product-image').show();
      }
      else{
          $(''+ucfid).down('.product-image').hide();
      }
      var prodDesc = $(''+ucfid).down('.product-description');
      if(hideDesc == 0 && prodDesc){
          prodDesc.show();
      }
      else if(prodDesc){
          prodDesc.hide();
      }
      if(align === 'right'){
          $(''+ucfid).down('.product').setStyle({'marginRight':'0px', 'marginLeft':'auto'});
      }
      else if(align === 'center'){
          $(''+ucfid).down('.product').setStyle({'marginRight':'auto', 'marginLeft':'auto'});
      }
      else if(align === 'left'){
          $(''+ucfid).down('.product').setStyle({'marginRight':'0px', 'marginLeft':'0px'});
      }
      if(buttonStyle !== currentButtonStyle){
          updateProductButton(ucfid,buttonStyle);
          setUserPreference('productButtonStyle', buttonStyle);
      }
      
      Weebly.Elements.continueOnChange();
      Weebly.Elements.saveProperties();
      Weebly.Elements.generateProperties(Weebly.Elements.currentElement);
  }

  function updateProductButton(ucfid, buttonStyle){
      var image = $(''+ucfid).down('.product-button');
      var size = image.src.match(/small|big/);
      var style = 'buy_now';
      if(buttonStyle === 'add_to_cart'){
          style = 'add_to_cart';
      }
      image.src = 'http://'+editorStatic+'/weebly/images/'+style+'_'+size+'.gif';
  }

  function makeProductEditable(ucfid){
      var title = $(''+ucfid).down('.product-title');
      if(title){title.onclick = function(){showEditBox(title.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':true})};}
      var price = $(''+ucfid).down('.product-price');
      if(price){price.onclick = function(event){showEditBox(price.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':false})};}
      var desc = $(''+ucfid).down('.product-description');
      if(desc){desc.onclick = function(event){showEditBox(desc.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':true})};}
      var form = $(''+ucfid).down('form');
      if(form){form.onsubmit = function(){return false;}}
      var img = $(''+ucfid).down('.product-image');
      if(img){
        img.onload =
            function(){
              Weebly.ImageResize.init(img, {callback: onResize, ucfid: ucfid});
              img.onload = null;
            };
      }
  }

  function saveProductField(id, text, align){
      var el = $(id);
      var type = el.className.match(/price|title|description/);
      if(type){type = type[0];}
      if(type === 'price'){
          text = text.replace(/[^\d\.]/g, '');
      }
      if(type){
          var productID = el.id.replace(/[^\d]/g, '');
      } else{
          var productID = Weebly.Elements.getCurrentProductID();
      }
      new Ajax.Request(ajax, {parameters:'pos=updateproduct&productID='+productID+'&type='+type+'&value='+encodeURIComponent(text)+'&cookie='+document.cookie,'onFailure':errFunc});
  }

  function setUserPreference(key, value){
      new Ajax.Request(ajax, {parameters:'pos=setuserpreference&key='+key+'&value='+value+'&cookie='+document.cookie,'onFailure':errFunc});
  }

  function loadExternalLibraries(){
      externalLibariesLoaded = true;
      //CodePress.run();
      checkFlash();
  }

  function showMerchantSettingsOption(){
      if($('site_settings_merchantaccount')){
          $('site_settings_googlecheckout').hide();
          $('site_settings_paypal').hide();
          switch($F('site_settings_merchantaccount')){
              case 'google':
                  $('site_settings_googlecheckout').show();
                  $('site_settings_curr_warning').show();
                  $('site_settings_currency').value = 'USD';
                  $('site_settings_currency').disable();
                  break;
              case 'paypal':
                  $('site_settings_paypal').show();
                  $('site_settings_curr_warning').hide();
                  $('site_settings_currency').enable();
                  break;
          }
      }
  }

function setupFileUploader(el){ // used to place the flash uploader on top of the initial image (gallery/standalone/etc)
                                 // for both FLASH uploader and PLAIN uploader
	if (isUploaderImage(el)) {
	    $(el).stopObserving('mouseover');
	    $(el).observe('mouseover', function(e){
	        if(isUploaderImage(el)){
	        
	        	//if(isGalleryImage(el)){
	        	
			        showFlashContainer(19); // to be under menu bar (which has zindex of 20)
			        if (Prototype.Browser.Gecko) {
			            $('flashContainer').clonePosition($(el));
			        } else {
			            $('flashContainer').style.position = 'relative';
			            $('flashContainer').clonePosition($(el));
			            $('flashContainer').style.position = 'absolute';
			        }
			        forceFlashPosition = true;

			        $('flashContainer').stopObserving('mousedown');
			        $('flashContainer').observe('mousedown', function(){
			            selectDefaultImageUpload(el.up('.element').id);
			        });
			        document.observe('mousemove', removeFlashUploader);
	        	
	        	//// for html-based file uploader
	        	//}else{
	        	//	var uploader = getPlainUploader('content');
				//	uploader.beforeQueued = function() {
				//		selectDefaultImageUpload(el.up('.element').id);
				//	};
				//	uploader.show(el, null, 19, true, true); // below menubar (menubar's zindex is 20)
			    //}
			    
	        }
	    });
    }
}

function removeFlashUploader(event){
    var el = Event.element(event);
    if(!isUploaderImage(el) && el.id !== 'flashContainer' && !el.up('#flashContainer')){
        forceFlashPosition = false;
        Weebly.Elements.positionFlash();
        $('flashContainer').stopObserving('mousedown');
        document.stopObserving('mousemove', removeFlashUploader);
    }
}

function isUploaderImage(el){
    return el.src && isUploaderImageSrc(el.src);
}

function isUploaderImageSrc(src){
    return src.match(/na\.jpg/) || src.match(/upload_images_01\.jpg/) || src.match(/video_click_to_upload\.jpg/)
}

//// for html-based file uploader
//function isGalleryImage(el) {
//	var ul = $(el).up('ul');
//	return ul && ul.hasClassName('imageGallery');
//}

function editPollDaddy(pollId, ucfid, ucfpid){
    Pages.go('pollDaddy');
    var pro = isPro() ? '1' : '0';
    setTimeout("$('poll-daddy-iframe').writeAttribute( 'src', 'http://jr.polldaddy.com/auth-weebly.php?userid='+userID+'&proUser="+pro+"&ucfpid="+ucfpid+"&callback=test&finalUrl='+encodeURIComponent('http://'+configSiteName+'/weebly/main.php#closePollDaddy') );", 500 );
}

function viewFormData(ucfid){
    window.open('viewFormData.php?ucfid='+ucfid, 'weebly_view_form', 'height=650,width=960,menubar=yes,toolbar=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
}


Weebly.TimingTest = {

  data: {},
  currentTest: '',
  

  start: function(test) {

    Weebly.TimingTest.data[test] = {};
    Weebly.TimingTest.data[test].start = new Date().getTime();

  },

  end: function(test) {

    if (!Weebly.TimingTest.data[test] || (Weebly.TimingTest.data[test] && !Weebly.TimingTest.data[test].start)) { return; }

    Weebly.TimingTest.data[test].diff = new Date().getTime() - Weebly.TimingTest.data[test].start;

    // Disabled
    //new Ajax.Request(ajax, {parameters:'pos=speedtest&test='+test+'&time_elapsed='+Weebly.TimingTest.data[test].diff+'&cookie='+document.cookie, bgRequest: true});

  }

}
