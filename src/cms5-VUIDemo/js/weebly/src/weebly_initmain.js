var myColorPicker;
var maxFileSize;

   function initMain(functionsToCall) {

	//console.log("initMain start");
	//Effect.Center('pleaseWait');
	//new PeriodicalExecuter(feedback, 1);

        if( functionsToCall.userID > 0) {
          userID = functionsToCall.userID;
        }
	if( functionsToCall.userIDLocation != '') {
	  userIDLocation = functionsToCall.userIDLocation;
	}
	if( functionsToCall.pageid[0] == '1') {
	  currentPage = functionsToCall.pageid[1];
	}
	if( functionsToCall.setSettingAnimations == '1') {
	  setSetting('settingAnimations',1);
	  Pages.optAnimations = 1;
	} else { Pages.optAnimations = 0; }
        if( functionsToCall.siteVersion == '1' || functionsToCall.siteVersion == '2' || functionsToCall.siteVersion == '3' || functionsToCall.customDomain != '') {
          setSetting('settingQuickExport',1);
        }
        if( functionsToCall.setSettingTooltips == '1') {
          setSetting('settingTooltips',1);
        }
	if( functionsToCall.currentTheme) {
	  setSetting('currentTheme', "'"+functionsToCall.currentTheme+"'");
	}
        if( functionsToCall.currentHeader) {
          setSetting('currentHeader', "'"+functionsToCall.currentHeader+"'");
        }
        if( functionsToCall.siteType) {
          setSetting('siteType', "'"+functionsToCall.siteType+"'");
        }
        if( functionsToCall.updatedTheme) {
          setSetting('updatedTheme', "'"+functionsToCall.updatedTheme+"'");
        }
        if( functionsToCall.friendRequests) {
          setSetting('friendRequests', "'"+functionsToCall.friendRequests+"'");
        }
        if( functionsToCall.adsenseID) {
          setSetting('adsenseID', "'"+functionsToCall.adsenseID+"'");
	}
	if( functionsToCall.tempUser) {
	  setSetting('tempUser', "'"+functionsToCall.tempUser+"'");
	  if(tempUser == 1) {
	    window.onbeforeunload = function() {
	      return /*tl(*/"If you close the window, all your changes will be lost."/*)tl*/;
	    }
	  }
	}
        if( functionsToCall.userEvents) {
	  setSetting('userEvents', "'"+functionsToCall.userEvents+"'");
	}
        if( functionsToCall.hideTitle) {
          setSetting('hideTitle', "'"+functionsToCall.hideTitle+"'");
        }

	setWidth();
	//Element.setStyle("elements", { top: "expression((dummy = document.documentElement.scrollTop) + 'px')"});
	//$('elements').style.top = "0px";

	if (navigator.appVersion.indexOf("MSIE") > -1) {
	  $('scroll_container').style.top = '133px';
	}

	currentSite = functionsToCall.currentSite;

	Pages.pageConstructor.main = { element: 'grayedOut', go: function() { } }; //alert(9); updateList(currentPage); } };
        Pages.pageDestructor.main = { element: 'grayedOut', maxHeight: ["$('left').clientHeight+135","$('container').clientHeight+30","$('right').clientHeight+135"], go: function() { if( document.getElementById(currentBox+'Edit')){ hideEditBox(currentBox);}} };
        Pages.pageWindows.main = ['main', 'themesMenu'];

	Pages.pageConstructor.showElements = { go: function() { showElements(); } };
	Pages.pageDestructor.showElements = { go: function() { hideElements(); } };
	Pages.pageWindows.showElements = ['showElements','main'];

        Pages.pageConstructor.save = { element: 'tip13' };
        Pages.pageDestructor.save = { element: 'tip13' };
	Pages.pageWindows.save = ['save','main'];

	Pages.pageConstructor.displayPageProperties = { go: function(var1) { pageDblClick = 1; displayPageProperties(var1); } };
        Pages.pageDestructor.displayPageProperties = { element: 'textEditor', go: function() { fadeProperties(); } };
	Pages.pageWindows.displayPageProperties = ['displayPageProperties'];

        Pages.pageConstructor.exportSite = { go: function() { if(tempUser == 1) { setTimeout("Pages.go('goSignup', 'publish');",100); } else { exportSite(); } } };
        Pages.pageDestructor.exportSite = { element: 'textEditor', go: function() { fadeProperties(); $('tip14').style.display = 'none'; } };
        Pages.pageWindows.exportSite = ['exportSite', 'themesMenu'];

        Pages.pageConstructor.doExport = { go: function() { doExport(); } };
        Pages.pageDestructor.doExport = { element: 'publishingWait', go: function() { publishingAnim = 0; $('tip14').style.display = 'none'; } };
        Pages.pageWindows.doExport = ['doExport', 'themesMenu'];

	Pages.pageConstructor.friendRequests = { element: 'friendRequests' };
	Pages.pageDestructor.friendRequests = { element: 'friendRequests' };
	Pages.pageWindows.friendRequests = ['friendRequests', 'themesMenu'];

	Pages.pageConstructor.showMyspaceLogin = { go: function() { showMyspaceLogin(); } };
	Pages.pageDestructor.showMyspaceLogin = { element: 'publishMyspaceDialog' };
	Pages.pageWindows.showMyspaceLogin = ['showMyspaceLogin', 'themesMenu'];

        Pages.pageConstructor.exportSuccess = { element: 'tip14' };
        Pages.pageDestructor.exportSuccess = { element: 'tip14' };
        Pages.pageWindows.exportSuccess = ['exportSuccess', 'themesMenu', 'main'];

        Pages.pageConstructor.addPage = { go: function() { AddPage(); } };
        Pages.pageDestructor.addPage = { element: 'textEditor', go: function() { fadeProperties(); } };
        Pages.pageWindows.addPage = ['addPage'];

        Pages.pageConstructor.displayUserSettings = { go: function() { displayUserSettings(); } };
        Pages.pageDestructor.displayUserSettings = { element: 'textEditor' };
        Pages.pageWindows.displayUserSettings = ['displayUserSettings'];

        Pages.pageConstructor.displaySiteSettings = { go: function(dontUpdate) { displaySiteSettings(dontUpdate); activeContainer('settings'); activeTab('weebly_tab_settings'); } };
        Pages.pageDestructor.displaySiteSettings = { element: 'textEditor', go: function() { activeContainer('elements'); activeTab('weebly_tab_edit'); fadeProperties(); } };
        Pages.pageWindows.displaySiteSettings = ['displaySiteSettings'];

	Pages.pageConstructor.domainMenu = { go: function(isPublish) { 

		if (isPublish == 1) { 
		  domainNextStep = "publish"; 
		} else if (isPublish == 2) { 
		  domainNextStep = "main" ;
		} else { 
		  domainNextStep = "sitesettings";
		}

		$('chooseDomain').style.top = "10px";
		$('chooseDomainClose').onclick = isPublish > 0 ? function(){ Pages.go('main'); return false; } : function(){ Pages.go('displaySiteSettings', 1); return false; }; 

		activeContainer('settings'); 
		activeTab('weebly_tab_settings'); 
		domainChoiceReset(); 

	}, element: 'domainContainer' };
	Pages.pageDestructor.domainMenu = { element: 'domainContainer', go: function() { activeContainer('elements'); activeTab('weebly_tab_edit'); } };
	Pages.pageWindows.domainMenu = ['displaySiteSettings', 'domainMenu'];

        Pages.pageConstructor.initialDomainMenu = { go: function(isPublish) { $('grayedOutTop').style.display = 'block'; $('chooseDomain').style.top = "26px"; domainNextStep = "main"; $('chooseDomainClose').onclick = function(){ Pages.go('main'); return false; }; domainChoiceReset(); }, element: 'domainContainer' };
        Pages.pageDestructor.initialDomainMenu = { element: 'domainContainer', go: function() { activeContainer('elements'); activeTab('weebly_tab_edit'); showEvent("first_tip"); $('grayedOutTop').style.display = 'none'; } };
        Pages.pageWindows.initialDomainMenu = ['initialDomainMenu'];

	Pages.pageConstructor.domainMenuPurchase = { go: function(domainName) { domainNextStep = "main"; activeContainer('settings'); activeTab('weebly_tab_settings'); $('domain_sld').value = domainName; }, element: 'domainContainer' };
        Pages.pageDestructor.domainMenuPurchase = { element: 'domainContainer', go: function() { activeContainer('elements'); activeTab('weebly_tab_edit'); } };
        Pages.pageWindows.domainMenuPurchase = ['domainMenuPurchase', 'domainMenu'];         

        Pages.pageConstructor.proPurchase = { go: function(message, refer) { if(tempUser == 1) { setTimeout("Pages.go('goSignup');",100); } else { showProPurchase(message, refer); $('purchaseX').style.display = 'block'; } }, element: 'domainContainer' };
        Pages.pageDestructor.proPurchase = { element: 'domainContainer', go: function() { $('purchaseX').style.display = 'none'; } };
        Pages.pageWindows.proPurchase = ['proPurchase', 'displaySiteSettings', 'pagesMenu'];

        if (ENABLE_THEME_BROWSER) {
	        Pages.pageConstructor.themesMenu = { go: function() {
	        	if (!window.litePageChange) {
	        		enteringDesignTab();
	        	}
	        	resetThemesMenuScrolling();
	        	activeTab('weebly_tab_themes');
	        	activeContainer('themes');
	        }};
            Pages.pageDestructor.themesMenu = { go: function() { leavingDesignTab(); }  };
        }else{
            Pages.pageConstructor.themesMenu = { go: function() { activeTab('weebly_tab_themes'); showThemes(); activeContainer('themes'); } };
            Pages.pageDestructor.themesMenu = { };
        }
        Pages.pageWindows.themesMenu = ['themesMenu', 'main'];

        Pages.pageConstructor.customThemeEditor = { go: function() { openCustomThemeEditor(); }, element: 'customThemeContainer' };
        Pages.pageDestructor.customThemeEditor = { go: function() { closeCustomThemeEditor();}, element: 'customThemeContainer' };
        Pages.pageWindows.customThemeEditor = ['customThemeEditor', 'themesMenu', 'main']; // for theme browser ~ashaw
        
        Pages.pageConstructor.importTheme = { go: function() { Weebly.lightbox.show({element: '#themeEditBoxImport', width: 600, height: 400}) } };
        Pages.pageDestructor.importTheme = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.importTheme = ['importTheme', 'themesMenu', 'main']; // for theme browser ~ashaw

	Pages.pageConstructor.pagesMenu = { go: function(newPage) { Weebly.PageManager.buildUI(newPage); activeContainer('pages'); activeTab('weebly_tab_pages'); } };
	Pages.pageDestructor.pagesMenu = { element: 'pagesContainer', go: function() { Weebly.PageManager.cleanup(); activeContainer('elements'); activeTab('weebly_tab_edit'); fadeProperties(); } };
	Pages.pageWindows.pagesMenu = ['pagesMenu', 'main'];
	
	Pages.pageConstructor.editMenu = { go: function() { setElementsPageType(); activeTab('weebly_tab_edit'); activeContainer('elements'); } };
	Pages.pageDestructor.editMenu = { };
	Pages.pageWindows.editMenu = ['editMenu', 'main'];

        Pages.pageConstructor.welcomeMenu = { element: 'welcomeContainer', go: function() { $('grayedOutTop').style.display = 'block'; $('newSiteTitle').focus(); } };
        Pages.pageDestructor.welcomeMenu = { element: 'welcomeContainer', go: function() { $('grayedOutTop').style.display = 'none';} };
        Pages.pageWindows.welcomeMenu = ['welcomeMenu'];

        Pages.pageConstructor.displayProperties = { go: function(ucfid) { displayProperties(ucfid); } };
        Pages.pageDestructor.displayProperties = { element: 'textEditor', go: function() { fadeProperties(); } };
        Pages.pageWindows.displayProperties = ['displayProperties'];

        Pages.pageConstructor.updateList = { go: function(pageID) { updateList(pageID); } };
        Pages.pageDestructor.updateList = {  }; 
        Pages.pageWindows.updateList = ['updateList', 'main', 'themesMenu'];

        Pages.pageConstructor.userHome = { go: function() { if(tempUser == 1) { setTimeout("Pages.go('goSignup', 'exit');",100); } else { window.onbeforeunload = null; setTimeout("window.location = 'userHome.php'", 400); } } };
        Pages.pageDestructor.userHome = {  };
        Pages.pageWindows.userHome = ['userHome'];

        Pages.pageConstructor.showHelp = { element: 'helpFrame', go: function() { $('helpFrame').style.height = (getInnerHeight() - 35) + "px"; $('helpIframe').src = helpLocation; } };
        Pages.pageDestructor.showHelp = { element: 'helpFrame' };
        Pages.pageWindows.showHelp = ['showHelp','themesMenu','main'];

        Pages.pageConstructor.giveFeedback = { go: showFeedback };
        Pages.pageDestructor.giveFeedback = { go: hideFeedback };
        Pages.pageWindows.giveFeedback = ['giveFeedback','main'];

        Pages.pageConstructor.goSignup = { go: function(action) { showSignup(action); } };
        Pages.pageDestructor.goSignup = { go: hideSignup };
        Pages.pageWindows.goSignup = ['goSignup'];

        Pages.pageConstructor.editImage = { go: function(oldImageLocation, imageId) { editImage(oldImageLocation, imageId); } };
        Pages.pageDestructor.editImage = { go: hideEditImage };
        Pages.pageWindows.editImage = ['editImage', 'displayProperties'];

        Pages.pageConstructor.goBlogPost = { go: function(postId, newPost) { currentBlog.postId = postId; updateList(currentPage, newPost);  } };
        Pages.pageDestructor.goBlogPost = { go: function() { currentBlog.postId = 0; currentBlog.newPost = 0; currentBlog.title = ''; currentBlog.categories = ''; } };
        Pages.pageWindows.goBlogPost = ['goBlogPost', 'themesMenu', 'main'];

	Pages.pageConstructor.domainEditor = { go: function() { Element.show('domainEditor'); } };
        Pages.pageDestructor.domainEditor = { go: function() { Element.hide('domainEditor'); } };
        Pages.pageWindows.domainEditor = ['domainEditor'];

        Pages.pageConstructor.adsenseSetup = { go: function() { if(tempUser == 1) { setTimeout("Pages.go('goSignup', 'adsense');",100); } else { Weebly.lightbox.show({element: '#adsense_terms', width: 480, height: 400, onHide: function() { onHideLightbox(); }});new Ajax.Request(ajax, {parameters:'pos=doevent&event=viewAdsense&cookie='+document.cookie}); } } };
        Pages.pageDestructor.adsenseSetup = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.adsenseSetup = ['adsenseSetup', 'main'];

        Pages.pageConstructor.externalSetup = { go: function(remoteSite, elementId) { if(tempUser == 1) { setTimeout("Pages.go('goSignup');",100); } else { Weebly.lightbox.show({element: '#'+remoteSite, button: {onClick: 'submitExternal("'+remoteSite+'");'}, onHide: function() { removeElement(elementId); }}); } } };
        Pages.pageDestructor.externalSetup = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.externalSetup = ['externalSetup', 'main'];

        Pages.pageConstructor.pollDaddy = { go: function() {Weebly.lightbox.show({element: '#edit-poll-daddy', onHide: function() { updateList(); }})}};
        Pages.pageDestructor.pollDaddy = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.pollDaddy = ['pollDaddy', 'goBlogPost', 'main'];

        Pages.pageConstructor.upgradeWarning = { go: function() {Weebly.lightbox.show({element: '#upgrade-warning', options:{hideClose:true}, onHide: function() { updateList(); }})}};
        Pages.pageDestructor.upgradeWarning = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.upgradeWarning = ['upgradeWarning', 'goBlogPost','main'];

        Pages.pageConstructor.formElementWarning = { go: function() {Weebly.lightbox.show({element: '#form-element-warning', options:{hideClose:true}, onHide: function() { updateList(); }})}};
        Pages.pageDestructor.formElementWarning = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.formElementWarning = ['formElementWarning', 'goBlogPost','main'];

	var signupIsPro = 0;
        Pages.pageConstructor.purchaseConfirmation = { element: 'purchaseConfirmation', go: function(pro) { if (pro) { signupIsPro = 1; } } };
        Pages.pageDestructor.purchaseConfirmation = { element: 'purchaseConfirmation', go: function() { if (signupIsPro) { signupIsPro = 0; makePro(); } if(publishAfterPro){publishAfterPro = false; Pages.go('exportSite');} } };
        Pages.pageWindows.purchaseConfirmation = ['purchaseConfirmation'];

        Pages.pageConstructor.moderateBlog = { go: function(id) { goModerateBlog(id); } };
        Pages.pageDestructor.moderateBlog = { element: 'newContainer' };
        Pages.pageWindows.moderateBlog = ['moderateBlog'];

	//myColorPicker = new Control.ColorPicker("currentColor", { IMAGE_BASE : "http://www.weebly.com/weebly/images/colorpicker/", 'swatch' : 'menuitem-cc', 'onClose': function() { setColor(); }, 'position': 'toolbar' });
    newColorChooser = new Weebly.ColorChooser($('new-color-chooser'), {onUpdate:function(color){runCommand(currentBox, 'forecolor', color);}});

    showBar();

	if (isPro()) {
	  maxFileSize = Math.floor(Weebly.Restrictions.accessValue('upload_limit_pro') / 1000);
	} else {
	  maxFileSize = Math.floor(Weebly.Restrictions.accessValue('upload_limit_free') / 1000); // 5120;
	}

        swfu = new SWFUpload({

          file_size_limit : maxFileSize,
          file_types : "*.*",
          file_types_description : "All files...",
          file_queue_limit : 20,

          file_dialog_start_handler : dialogStart,
          file_dialog_complete_handler : dialogComplete,
          upload_start_handler : uploadStart,
          file_queue_error_handler : queueError,
          file_queued_handler : uploadFileQueued,
          upload_error_handler : uploadError,
          upload_success_handler : uploadSuccess,
          upload_progress_handler : uploadProgress,
          upload_complete_handler : uploadFileComplete,
          swfupload_loaded_handler : swfUploadLoaded,

	  prevent_swf_caching: true,

	  button_placeholder_id: "flashButtonPlaceholder",
	  button_width: 600,
	  button_height: 600,
	  button_window_mode: SWFUpload.WINDOW_MODE.TRANSPARENT,
	  button_cursor: SWFUpload.CURSOR.HAND,

          flash_url : "/weebly/libraries/swfupload/swfupload.swf"

        });
        
	Pages.go('editMenu');
	if (functionsToCall.siteTitle == 'No Title') {
	  Pages.go('welcomeMenu');
	}
	else if (settingQuickExport == 0) { // apparently settingQuickExport==0 means they haven't set up a domain yet. ~ashaw
	  Pages.go('initialDomainMenu');
	}
	//setTimeout("Pages.go('editMenu');", 1000);

	// ashaw
    //updatePages(1);
    Weebly.PageManager.init();
    //

 	var editorLoadTime = (new Date().getTime() - loadTime['a0']) / 1000;
	loadTime['final'] = editorLoadTime;
	fireTrackingEvent('EditorLoadTime', 'Load', '', editorLoadTime);
	recordLoad();
    showEvent('first_tip');
	//console.log("initMain end");
   }

