var noJump	      = 0;

    // Windowing system in place allows a Pages.go('page', options) functionality
// -- The system takes care of showing or hiding specific pages
// Also "Fixes" browser back and forward buttons
// -------
var Pages = {
  Version        : '0.5',
  Author         : 'David Rusenko',
  Company        : 'Weebly, Inc.',
  openPages      : new Array('main'),
  currentNavPage : 'load',
  optConfirmLoad : 0,
  optAnimations  : 1,
  history        : [],
  lastPage       : '',

  load: function() {
  },
// List of pages, and their respective "start-up" functions
  pageConstructor: {
    main : {  }
  },

// List of pages, and their "shut down" or close functions
  pageDestructor: {
    main : {  }
  },
// List of windows that are allowed to be open for each page
  pageWindows: {
    main : ['main']
  },
  currentVar1: 'empty',
  currentVar2: 'empty',
  currentVar3: 'empty',

// go function... accepts up to three variables that it will pass on
// -- to the page creator function
  go: function(page, var1, var2, var3) {

Pages.updateHistory(page);
if (Weebly.Elements) { Weebly.Elements.unselectElement(); }

// Prevent people from navigating away from a current blog entry
if (typeof(currentBlog) != "undefined" && currentBlog.postId && currentBlog.postId == 1 && page != "goBlogPost" && Pages.pageWindows[page].indexOf('goBlogPost') == -1) {
  if (currentBlog.saving) {
    currentBlog.saving = 0;
  } else {
    var confirmLostPost = confirm(/*tl(*/"Warning: Your post is not saved. If you navigate away, you will lose your changes.\n\nTo keep your changes and continue editing, press Cancel.\n\nTo discard your post and continue your action, press OK."/*)tl*/);
    if (!confirmLostPost) {
      return;
    }
  }
}

    var v1 = typeof var1 == 'undefined' || var1 == '' || var1 == 'undefined' ? 0 : 1;
    var v2 = typeof var2 == 'undefined' || var2 == '' || var2 == 'undefined' ? 0 : 1;
    var v3 = typeof var3 == 'undefined' || var3 == '' || var3 == 'undefined' ? 0 : 1;

  if (page != Pages.currentNavPage || (var1 != Pages.currentVar1 && v1) || (var2 != Pages.currentVar2 && v2) || (var3 != Pages.currentVar3 && v3)) {

    //alert("openPages: "+Pages.openPages.join(","));
    //alert("Pages.go: "+page+" "+var1+" - CurrentPage: "+Pages.currentNavPage+" + "+Pages.currentVar1);

    Pages.handleMain(page);
    Pages.closeAll(page);

    if (noJump == 1) { noJump = 0 } else { window.scrollTo(0,0); }

    var thisElement = eval("Pages.pageConstructor."+page);
    if( thisElement && page != 'main') {           
  if( thisElement.element) {
    Pages.showElement(thisElement.element);
      }
      if( thisElement.go) { thisElement.go(var1, var2, var3); }
    }

    if (!window.litePageChange) {
    	// Automatically confirm page load if the optConfirmLoad setting is not set
	    if(!Pages.optConfirmLoad) Pages.confirmLoad(page, var1, var2, var3);
	}

    Pages.currentNavPage    = page;
    if(typeof var1 == 'undefined' || var1 == '' || var1 == 'undefined') { Pages.currentVar1 = 'empty'; } else { Pages.currentVar1 = var1; }
    if(typeof var2 == 'undefined' || var2 == '' || var2 == 'undefined') { Pages.currentVar2 = 'empty'; } else { Pages.currentVar2 = var2; }
    if(typeof var3 == 'undefined' || var2 == '' || var3 == 'undefined') { Pages.currentVar3 = 'empty'; } else { Pages.currentVar3 = var3; }

  }
  
  if (Pages.openPages.indexOf(page) == -1) {
  	Pages.openPages.push(page);
  }
  //console.log('--- Pages.currentNavPage: ' + Pages.currentNavPage);
  //console.log('--- Pages.openPages: ' + Pages.openPages);

},

// Shows pages according to rules
  showElement: function(showElement, animation) {

    if( Pages.optAnimations) { 
  setTimeout("Effect.Appear('"+showElement+"');", 1000); 
}
    else { Element.show(showElement); }

  },
// Special logic to handle the case of 'main' element, where the logic is reversed...
  handleMain: function(page) {

    if (eval ("Pages.pageWindows."+page+".indexOf('main');") > -1) {
      if( Pages.pageConstructor.main) {
        if( Pages.pageConstructor.main.element) {
          if( Pages.optAnimations) { setTimeout("Effect.Fade('"+Pages.pageConstructor.main.element+"')", 1000); }
          else { Element.hide(Pages.pageConstructor.main.element); }
        }
        if( Pages.pageConstructor.main.go) { Pages.pageConstructor.main.go(); }
      }
      if (Pages.openPages.indexOf('main') == -1) { Pages.openPages.push('main'); }
    } else {
      if( Pages.pageDestructor.main) {
        if( Pages.pageDestructor.main.element) {
          //var newHeight = Pages.pageDestructor.main.maxHeight ? eval("Math.max("+Pages.pageDestructor.main.maxHeight+");") : '600';
      var newHeight = 1000;
          Element.setStyle(Pages.pageDestructor.main.element, { height: newHeight });
          if( Pages.optAnimations) { Effect.Appear(Pages.pageDestructor.main.element); }
          else { Element.show(Pages.pageDestructor.main.element); }
        }
        if( Pages.pageDestructor.main.go) { Pages.pageDestructor.main.go(); }
      }
      if (Pages.openPages.indexOf('main') == -1) { Pages.openPages.push('main'); }
    }

// Hack to make sure main always shows when pressing back button and window.reload
// Not sure why Pages.pageConstructor.main.element isn't set in this case, weird
if (page == "main" && $('grayedOut').style.visibility == "visible") {
  Element.hide('grayedOut');
}

  },

// A page will confirm a successfull load through this function
  confirmLoad: function(pageName, var1, var2, var3) {

    if (Pages.openPages.indexOf(pageName) == -1) { Pages.openPages.push(pageName); }
    
    _goPageHistory("page="+pageName+"&1="+var1+"&2="+var2+"&3="+var3);

  },

// Close all windows except those specified in array pageWindows.currentWindow
  closeAll: function(currentWindow) {
    for ( j=Pages.openPages.length-1; j >= 0; j-- ) {
      var matches = 0;
      eval ("matches = Pages.pageWindows."+currentWindow+".indexOf(Pages.openPages[j]);");
      if (matches == -1 && Pages.openPages[j] != 'main') {
        var thisElement = eval("Pages.pageDestructor."+Pages.openPages[j]);
        if( thisElement) {
          if( thisElement.element) {
            if( Pages.optAnimations) { Effect.Fade($(thisElement.element)); }
            else { Element.hide($(thisElement.element)); }
          }
          if( thisElement.go) thisElement.go();
        }
        Pages.openPages.splice(j, 1);
      }
    }
  },

  returnToEditor: function(){
      if(Pages.openPages.indexOf('goBlogPost') > -1){
          Pages.go('goBlogPost');
      }
      else{
          Pages.go("main");
      }
  },
  
  updateHistory: function(page){
      if(page!=Pages.lastPage){
          Pages.history.push(page);
          if(Pages.history.size() > 10){
              Pages.history.shift();
          }
      }
      Pages.lastPage = page;
  }

};



var _goPageIframe;

function _goPageHistory(queryString) {
	var url = "/weebly/goPage.php?" + queryString;
	if (!_goPageIframe) {
		$(document.body).insert(_goPageIframe = new Element('iframe', {
			style: 'height:0;width:0;border:0;overflow:hidden;position:absolute;bottom:0px;left:0px;',
			frameBorder: 0
		}));
		_goPageIframe.src = url; // needs top be after creation, for webkit
	}else{
		_goPageIframe.contentWindow.location.href = url;
	}
}



//    Pages.load();

//------------
/// End of Pages module
////
