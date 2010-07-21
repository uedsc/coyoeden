Position.includeScrollOffsets = true;
var splitpanes = new Array();
var contentDraggables = new Array();

/***************Rules**********************/
var myrules = {
   
    '#body' : function(el) {

	//Initialize here
	contentDraggables = [];

    },
    '#error' : function(el){
	el.onclick = function(){
	    new Effect.Fade('error');
	}
    }, 
    '#tip11' : function(el){
        el.onclick = function(){
            new Effect.Fade('tip11');
        }
    },
    '#tip12' : function(el){
        el.onclick = function(){
            new Effect.Fade('tip12');
        }
    },
    '#tip13' : function(el){
        el.onclick = function(){
            Pages.go('main');
        }
    },
    '#help' : function(el){
        el.onclick = function(){
            Pages.go('main');
        }
    },
    '#warning' : function(el){
        el.onclick = function(){
            new $('warning').hide();
        }
    },

    '#elementlist' : function(el){
	    contentDraggables.push(el);
            //Sortable.create("elementlist", {dropOnEmpty:true,containment:["elementlist","secondlist"],constraint:false, onUpdate:updateElements});
    },
    '#secondlist .columnlist' : function(el){
	    //contentDraggables.push($(el).id);
	    resizeColumns(el);
    },
    '#secondlist .columnlist .element img' : function(el){
        el.onload = function(event){
            var colEl = el.up('#secondlist .columnlist');
            if(colEl){
                if(colEl.id.match(/rhs-list/)){
                    colEl = $(colEl.id.replace(/rhs-list/, 'lhs-list'));
                }
                resizeColumns(colEl);
                if(el.up('.element-panes')){
                    updateTwoColumnDividerHeight(el.up('.element-panes').id);
                }
            }
        };
    },
    '#secondlist .columnlist-blog' : function(el){
        contentDraggables.push(el);
	    if (el.childNodes.length == 0) {
		el.style.height = '400px';
	    } else {
		el.style.height = 'auto';
	    }
    },
    '#secondlist-blog' : function(el) {
            contentDraggables.push(el);
	    Sortable.create('secondlist-blog', {hoverclass: 'hoverclass', dropOnEmpty:true,handle:'handle',scroll:'scroll_container',scrollSensitivity:50,scrollSpeed:5,containment:contentDraggables,constraint:false,onUpdate:function() { updateList(); } });
    },
    // (Medium) Memory leak 1
    '#weebly_site_title' : function(el) {
    	if (Weebly.Restrictions.hasAccess('editable_site_title')) {
			if (! $('weebly_site_titleEdit')) {
			  var iframeEl = document.createElement('iframe');
			  iframeEl.id = 'weebly_site_titleEdit';
			  iframeEl.className = 'editable';
			  iframeEl.frameBorder = 0;
			  iframeEl.allowTransparency = true;
			  if ($('weebly_site_title').nextSibling) {
				$('weebly_site_title').parentNode.insertBefore(iframeEl, $('weebly_site_title').nextSibling);
			  } else {
				$('weebly_site_title').parentNode.appendChild(iframeEl);
			  }
			  $('weebly_site_title').onclick = function() { unselectHeader(1); showEditBox('weebly_site_title'); return false; };
			  $('weebly_site_title').style.display = 'block';
			  //new Ajax.InPlaceEditor('weebly_site_title', ajax, { ajaxOptions: { bgRequest: true}, callback: function(form, value) { return 'pos=sitetitle&newtitle=' + encodeURIComponent(value)+"&cookie="+document.cookie; }, onComplete: function(form, value) { updateSiteTitle(value); }, onEnterEditMode:function() { unselectHeader(1)}, okText: '', okButton: false, cancelLink: false, cancelText: ' ', submitOnBlur: true, border: '1px dashed #4455aa', highlightcolor: 'transparent', hoverClassName: 'element-hover'});
			  //alert( $($(el).id+'-inplaceeditor') );
			}
		}
    },

    //'#colorpicker' : function(el){
    //        ColorSelector.create('colorpicker', {value: '255,100,0', transforms:DefaultTransforms});
    //},

    // (Small) Memory leak 2
    '#createLink' : function(el){
	    new Draggable('createLink');
    },

    '#secondlist .weebly-splitpane-2' : function(el) {
	    var elId = $(el).id.replace(/-parent/, '');
	    if ($(el).parentNode) {
	      $(el).parentNode.className = 'element-panes';
	      $(el).parentNode.style.padding = '0';
	      $(el).parentNode.style.margin = '0';
	      /*
	      $(el).parentNode.parentNode.parentNode.childNodes[1].style.backgroundColor = '#25b82c';
	      $(el).parentNode.parentNode.parentNode.style.border = '1px solid #66b86a';
	      */
	      $(el).parentNode.ondblclick = function() { };
	    }

	    // Make sure that there isn't already a SplitPane created for this div
	    if ((!splitpanes[elId] || (splitpanes[elId] && splitpanes[elId].container != $(el))) && $(elId+'-lhs-width')) {
	      var sp = new SplitPane(elId+"-lhs", $(elId+'-lhs-width').value, elId+"-rhs", $(elId+'-lhs-width').value, $(elId+'-rhs-width').value, { 
	        onEnd: function(splitPane, event) { 
		  handlerDragEndSplitpane(splitPane, event, $(elId+'-lhs-cfpid').value+"-"+$(elId+'-rhs-cfpid').value, elId) 
	        }, 
	        active: true 
	      });
	      sp.set();
	      splitpanes[elId] = sp;
	    }
    },
    '#secondlist a' : function(el) {

	    //if (el.className == "keepLinkAlive") { return; }
	    if (!el.onclick) { el.onclick = function() { return false; } }
	    el.title = "Links active once published";

    },
    '#footerCode a' : function(el) {

            if (!el.onclick) { el.onclick = function() { return false; } }
            el.title = "Links active once published";
    },
    '#secondlist .disable-link' : function (el) {

	    if (!el.onclick) { el.onclick = function() { return false; } }
	    el.title = "";

    },

    '#secondlist img' : function(el) {

	var _match = el.src.match(/.+\/(.+)$/),
		imgSrc = _match[1];
	if (imageResizeId[imgSrc]) {
	  el.src.match(/.+\?(.+)$/);
	  var oldId = _match[1];
          var newId = imageResizeId[imgSrc];

	  if (oldId != newId) {
            el.src = el.src + "?" + newId;
	  }
	}

    },
    '#secondlist .blog-link' : function(el) {
	    el.onclick = function() { goBlogPage(el.pathname); return false; }
	    el.title   = "";
    },
    '#secondlist #blog-date' : function(el) {

	    if (currentBlog.postId != 0) {

              var currentDateFormat = "%m/%d/%Y";
              if ($('blogDateFormat') && $('blogDateFormat').innerHTML.match('eu')) {
                currentDateFormat = "%d/%m/%Y";
              }

	      Calendar.setup({dateField: 'blog-date', selectHandler: calendarCallback, dateFormat: currentDateFormat});
	    }

    },
    '#secondlist .no-title' : function (el) {
	    el.title   = "";
    },

    '#icontent .weebly_header' : function(el) {
	    el.title   = "Click to replace";
    },

    /**
    '#themeOptions .dropDownData' : function(el) {
	    if (el.nextSibling && el.nextSibling.nextSibling && el.nextSibling.nextSibling.className == "weeblyDropDown") { return; }
	    new Weebly.DropDown(el.nextSibling, {openWidth: 95, rowFunction: function(x) { return generateDropDownRow(x, el.innerHTML); }});
    },
    **/

// MySpace styles
    '#myspaceListLeft' : function (el) {
	    //contentDraggables.push('myspaceListLeft');
	    //Sortable.create('myspaceListLeft', {hoverclass: 'hoverclass', dropOnEmpty:true, handle: 'handlebar_left', scroll:'scroll_container', scrollSensitivity:50, scrollSpeed:5, containment: ['elementlist','myspaceListLeft','myspaceListRight','myspaceAboutMeList','myspaceLikeToMeetList'], onUpdate: updateList});
    },
    '#weebly_boxLeft .weebly_box_left_top' : function (el) {
            el.onmouseover = function() { myspace_mouseOver(el); }
            el.onmouseout  = function() { myspace_mouseOut(el); }
    },
    '#weebly_boxLeft .handlebar_left' : function (el) {
            el.onmouseover = function() { myspace_mouseOver(el.parentNode); }
    },

    '#myspaceListRight' : function (el) {
	    //contentDraggables.push('myspaceListRight');
	    Sortable.create('myspaceListRight', {hoverclass: 'hoverclass', dropOnEmpty:true, handle: 'handlebar_right', scroll:'scroll_container', scrollSensitivity:50, scrollSpeed:5, containment: ['elementlist','myspaceListRight'], onUpdate: updateList});
    },
    '#weebly_boxRight .weebly_box_right_top' : function (el) {
            el.onmouseover = function() { myspace_mouseOver(el); }
            el.onmouseout  = function() { myspace_mouseOut(el); }
    },
    '#weebly_boxRight .handlebar_right' : function (el) {
            el.onmouseover = function() { myspace_mouseOver(el.parentNode); }
    },
    '#secondlist .updateBackgroundImage' : function(el) {
	    //refreshBackgroundImage(el);
    },
    '#icontent_container' : function(el) {
	    //refreshBackgroundImage(el);
    },

    '#scroll_container' : function(el) {
	    Event.observe(el, 'mouseover', hoverHandler);
    },
    '.product-title' : function(el){
        el.onclick = function(){showEditBox(el.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':true})};
    },
    '.product-price' : function(el){
        el.onclick = function(event){showEditBox(el.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':false})};
    },
    '.product-description' : function(el){
        el.onclick = function(event){showEditBox(el.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':true})};
    },
    '.product form' : function(el){
        el.onsubmit = function(){return false;}
    },
    '.element img' : setupFileUploader,
    '.element iframe' : setupFileUploader,
    '.formlist' : function(el){
        if($(el.id+'-form-cover')){
            $(el.id+'-form-cover').remove();
        }
        if(!Weebly.Elements.highlightedElement && !$('blog-editor-post')){
            var cover = new Element('div', {'id':el.id+'-form-cover', 'title':'Click on the form to add or edit the fields'});
            cover.setStyle({position:'absolute', opacity:0, background:'#FFFFFF', top:'0px', left:'0px', height:(el.getHeight()+10)+'px', width:(el.getWidth()+10)+'px', cursor:'pointer', zIndex:'5'});
            el.insert({after:cover});
            cover.observe('click', function(){if(!deleting){Weebly.Elements.selectElement(el.up('.element'));}});
        }

        el.select('.show-element-options-image').invoke('remove');
    },

// Last statement
    '#secondlist' : function(el){
            pushTwoColumnsDraggables();
            if(!Weebly.Elements.highlightedElement){
                contentDraggables.push(el);
            }
            initDraggables();
            Weebly.Form.fieldInstructionsHandler();
            //Sortable.create("secondlist", {hoverclass: 'hoverclass', dropOnEmpty:true,handle:'handle',containment:["secondlist","elementlist","trashlist"],constraint:false,onUpdate:updateList});
    }
    
};

/***************Register Rules*************/
Behaviour.register(myrules);


/********Resgister additional events*******/
Behaviour.addLoadEvent(function() {
        //Effect.Appear('tip1');
        //updateList();
}); 

//Behaviour.apply();
