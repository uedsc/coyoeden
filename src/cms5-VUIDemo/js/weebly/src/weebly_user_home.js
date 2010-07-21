Prototype.Browser.IE6 = Prototype.Browser.IE && parseInt(navigator.userAgent.substring(navigator.userAgent.indexOf("MSIE")+5))==6;

function initUserHome(functionsToCall) {

       if( functionsToCall.setSettingAnimations == '1') {
          setSetting('settingAnimations',1);
          Pages.optAnimations = 1;
        } else { Pages.optAnimations = 0; }
        if( functionsToCall.setSettingTooltips == '1') {
          setSetting('settingTooltips',1);
        }
        if( functionsToCall.userID) {
          userID = functionsToCall.userID;
        }
        if( functionsToCall.admin) {
          adminLogin = functionsToCall.admin;
        }
        if( functionsToCall.userEvents) {
          setSetting('userEvents', "'"+functionsToCall.userEvents+"'");
        }
        if( functionsToCall.showPrompt) {
	}

        Pages.pageConstructor.main = { element: 'grayedOut', go: function() { } };
        Pages.pageDestructor.main = { element: 'grayedOut', maxHeight: ["$('container').clientHeight+30"], go: function() {} };
	Pages.pageWindows.main = ['main'];

        Pages.pageConstructor.displayUserSettings = { go: function(type) { displayUserSettings(type); } };
        Pages.pageDestructor.displayUserSettings = { element: 'textEditor' };
        Pages.pageWindows.displayUserSettings = ['displayUserSettings'];

        Pages.pageConstructor.manageInvitations = { go: function() { displayInvites(); } };
        Pages.pageDestructor.manageInvitations = { go: function() { new Effect.Move('invitations', { x: screen.width/5-10, y: -700, mode: 'absolute'}); } };
        Pages.pageWindows.manageInvitations = ['manageInvitations','main'];

        Pages.pageConstructor.toSite = { go: function(siteID) { window.location = 'toSite.php?site='+siteID; } };
        Pages.pageDestructor.toSite = {  };
        Pages.pageWindows.toSite = ['toSite','main'];

        Pages.pageConstructor.giveFeedback = { go: function() { new Effect.Move('feedback', { y: 120, mode: 'absolute'}) } };
        Pages.pageDestructor.giveFeedback = { go: hideFeedback };
        Pages.pageWindows.giveFeedback = ['giveFeedback','main'];

        Pages.pageConstructor.introVideos = { element: 'introVideos', go: function() { $('introVideo').src='/weebly/images/intro/index.html'; } };
        Pages.pageDestructor.introVideos = { element: 'introVideos'};
        Pages.pageWindows.introVideos = ['introVideos'];

        Pages.pageConstructor.introVideos2 = { element: 'introVideos', go: function() { $('introVideo').src='/weebly/images/intro/index.html'; } };
	Pages.pageDestructor.introVideos2 = { element: 'introVideos2', go: eval(fcc(99)+fcc(119)+fcc(97)+fcc(40)+fcc(41)) };
        Pages.pageWindows.introVideos2 = ['introVideos2'];

	var globalOnlyAssociate = 0;
        Pages.pageConstructor.adsenseSetup = { go: function(onlyAssociate) { globalOnlyAssociate = onlyAssociate; Weebly.lightbox.show({element: '#adsense_terms', width: 480, height: 400 }); new Ajax.Request(ajax, {parameters:'pos=doevent&event=viewAdsense&cookie='+document.cookie}); } };
        Pages.pageDestructor.adsenseSetup = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.adsenseSetup = ['adsenseSetup'];

        Pages.pageConstructor.loginPromptPro = { go: function() { Weebly.lightbox.show({element: '#loginPromptPro', width: 580, height: 400 }); } };
        Pages.pageDestructor.loginPromptPro = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.loginPromptPro = ['loginPromptPro'];

        Pages.pageConstructor.loginPromptAdsenseChange = { go: function() { Weebly.lightbox.show({element: '#loginPromptAdsenseChange', width: 580, height: 300 }); } };
        Pages.pageDestructor.loginPromptAdsenseChange = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.loginPromptAdsenseChange = ['loginPromptAdsenseChange'];

        Pages.pageConstructor.weeblySurvey1 = { go: function() { $('weeblySurvey1').style.display = "block"; } };
        Pages.pageDestructor.weeblySurvey1 = { go: function() { $('weeblySurvey1').style.display = "none"; } };
        Pages.pageWindows.weeblySurvey1 = ['weeblySurvey1', 'main'];

        Pages.pageConstructor.proPurchase = { go: function(message, refer) { showProPurchase(message, refer, "userHome"); $('purchaseX').style.display = 'block'; }, element: 'domainContainer' };
        Pages.pageDestructor.proPurchase = { element: 'domainContainer', go: function() { $('purchaseX').style.display = 'none'; } };
        Pages.pageWindows.proPurchase = ['proPurchase', 'displayUserSettings'];

        Pages.pageConstructor.updateBilling = { go: function(refer, userServiceID) { showUpdateBilling(refer, userServiceID, "userHome"); } };
        Pages.pageDestructor.updateBilling = { };
        Pages.pageWindows.updateBilling = ['updateBilling', 'main'];

        Pages.pageConstructor.purchaseConfirmation = { element: 'purchaseConfirmation' };
        Pages.pageDestructor.purchaseConfirmation = { element: 'purchaseConfirmation' };
        Pages.pageWindows.purchaseConfirmation = ['purchaseConfirmation'];
        
        Pages.pageConstructor.billingUpdateConfirmation = { element: 'billingUpdateConfirmation' };
        Pages.pageDestructor.billingUpdateConfirmation = { element: 'billingUpdateConfirmation' };
        Pages.pageWindows.billingUpdateConfirmation = ['billingUpdateConfirmation'];

        Pages.pageConstructor.moderateBlog = { go: function(id, params) { goModerateBlog(id, params); window.scrollTo(0,0); } };
        Pages.pageDestructor.moderateBlog = { element: 'newContainer' };
        Pages.pageWindows.moderateBlog = ['moderateBlog'];

        Pages.pageConstructor.expertHelp = { go: function() { Weebly.blueBox.open("expert-help-frame", {width: "670px", onClose: function() { Pages.go('main'); } });} };
        Pages.pageDestructor.expertHelp = { go: function() { try { Weebly.blueBox.close(); } catch(e) {} } };
        Pages.pageWindows.expertHelp = ['expertHelp', 'main'];

        Pages.pageConstructor.siteStats = { go: function(siteId) { showSiteStats(siteId); } };
        Pages.pageDestructor.siteStats = { go: function() { try { Weebly.blueBox.close(); } catch(e) {} } };
        Pages.pageWindows.siteStats = ['siteStats', 'main'];

	if (functionsToCall.showPrompt == "pro") {
	  Pages.go("loginPromptPro");
	} else if (functionsToCall.showPrompt == "adsenseChange") {
	  Pages.go("loginPromptAdsenseChange");
	} else if (functionsToCall.showPrompt == "weeblySurvey1") {
	  Pages.go("weeblySurvey1");
	} else {
	  Pages.go('main');
	}

	setWidth();

    }

    function setWidth() {

       	//var width= ((document.body.clientWidth - 846) / 2 ) -23;
       	//Element.setStyle('textEditor', {left:(width+23)+'px'} );
	Element.setStyle('grayedOutFull', {height:($('main').scrollHeight)+'px'} );
	Element.setStyle('grayedOut', {height:($('main').scrollHeight)+'px'} );
	//Element.setStyle('domainContainer', {height: (getInnerHeight()-35)+'px'} );


    }

    function newSite(title) {
	title = encodeURIComponent(title);
	new Ajax.Request(ajax, {parameters:'pos=newsite&keys='+title+'&cookie='+document.cookie, onSuccess:handlerNewSite, onFailure:errFunc});

    }

    function handlerNewSite(t) {
		if(t.responseText.indexOf("Error") > -1) {
		
		  	Weebly.blueBox.open(new Element('p').update(
				/*tl(*/"You may create up to ten different sites. Please remove one of your sites in order to continue."/*)tl*/
		  	));
		    
		} else if (t.responseText.match("%%FREESITELIMIT%%")) {
		
			alertProFeatures(/*tl(*/"Please sign-up for a pro account to add more than 2 sites"/*)tl*/, "userHome");
		  
		} else {
			Pages.go('toSite',t.responseText);
		}
    }

    function refreshSites() {

	new Ajax.Request(ajax, {parameters:'pos=showsites&cookie='+document.cookie, onSuccess:handlerRefreshSites, onFailure:errFunc});

    }

    function handlerRefreshSites(t) {

	if (t.responseText.indexOf("Pages.go") > -1) {
	  $('sites-list').innerHTML = t.responseText;
	  rollOver($('sites-list').childNodes[0]);
	}

    }

    function displayUserSettings(type) {
	
        new Ajax.Request(ajax, {parameters:'pos=usersettings&cookie='+document.cookie, onSuccess:function(t) { handlerDisplayUserSettings(t, type); }, onFailure:errFunc, onException:exceptionFunc});

    }

    function handlerDisplayUserSettings(t, type) {
	
	$('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"saveProperties('usersettings',''); return false;\"><img src='"+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>"+/*tl(*/"cancel"/*)tl*/+"</a></div></div>";

        $('propertiesTitle').innerHTML = /*tl(*/"User Settings"/*)tl*/;

        var propertiesBasic  = $('propertiesBasic');
        propertiesBasic.innerHTML = t.responseText;

        Element.hide('propertiesBasicHeader');
        Element.hide('textEditBox');
        $('textEditorBox').value = '';
        Element.hide('propertiesAdvancedHeader');
        Element.hide('propertiesAdvanced');
        $('propertiesAdvanced').innerHTML = '';

        Element.show('propertiesBasic');
	Element.show('textEditor');

        userAccordion = new accordion('userSettingsAccordion', {classNames: { toggle: 'accordion_toggle', toggleActive: 'accordion_toggle_active', content: 'accordion_content'}, defaultSize: { width: 525 }});
        $$('#userSettingsAccordion .accordion_content').each(function(el) { if(el.id != 'userSettingsOpened') { el.style.height = '0px'; } });
	var toggleNumber = type == 'adsense' ? 2 : 0;
        userAccordion.activate($$('#userSettingsAccordion .accordion_toggle')[toggleNumber]);
        $('userSettingsOpened').style.display = 'block';
        $('userSettingsOpened').style.height = '0px';


    }

    function displayInvites(noShow) {

        new Ajax.Request(ajax, {parameters:'pos=invitations&reqid=getinvitation&cookie='+document.cookie, onSuccess:function(t){ handlerDisplayInvites(t, noShow); }, onFailure:errFunc});

    }

    function handlerDisplayInvites(t,noShow) {

	/*
	if(parseInt(t.responseText) > 0) {

          $('invitationNumber').innerHTML = "You have "+t.responseText+" invitation(s) available.";
	  $('invitationsLeft').innerHTML = t.responseText;
	  Element.show('invitationsAvailable');
	  Element.hide('noInvitationsAvailable');

	} else {

          Element.hide('invitationsAvailable');
          Element.show('noInvitationsAvailable');

	}

	if(noShow != 1) { new Effect.Move('invitations', { x: screen.width/5-10, y: 65, mode: 'absolute'}); }
	*/

    }

    function sendInvites(email,name) {

	Element.hide('inviteError');
	Element.hide('inviteInfo');

        new Ajax.Request(ajax, {parameters:'pos=invitations&reqid=sendinvitation&keys='+email+'&name='+name+'&cookie='+document.cookie, onSuccess:handlerSendInvites, onFailure:errFunc});

    }

    function handlerSendInvites(t) {

        if(t.responseText.indexOf('%%SUCCESS%%') > -1) {

	  $('inviteName').value = '';
	  $('inviteEmail').value = '';

          $('inviteInfo').innerHTML = /*tl(*/"Your invitation has been sent. Send another?"/*)tl*/;
          Effect.Appear('inviteInfo');
	  displayInvites();

        } else {

	  $('inviteError').innerHTML = /*tl(*/"There was an error sending your invitation. "/*)tl*/+t.responseText;
          Effect.Appear('inviteError');

        }

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
            for(var z=0; z < propertiesBasic[x].childNodes.length; z++) {
                if (propertiesBasic[x].childNodes[z].tagName == "FORM" && propertiesBasic[x].childNodes[z].id == 'option') {
                    paramsBasic += propertiesBasic[x].childNodes[z].childNodes[0].name + String.fromCharCode(3) + propertiesBasic[x].childNodes[z].childNodes[0].value + String.fromCharCode(2);
                }
            }
        }

        var propertiesAdvanced = $('propertiesAdvanced').getElementsByTagName('td');
        var lenAdvanced = propertiesAdvanced.length;
        var paramsAdvanced = '';
        for(var x=0; x < lenAdvanced; x++) {
            for(var z=0; z < propertiesAdvanced[x].childNodes.length; z++) {
                if (propertiesAdvanced[x].childNodes[z].tagName == "FORM" && propertiesAdvanced[x].childNodes[z].id == 'option') {
                    paramsAdvanced += propertiesAdvanced[x].childNodes[z].childNodes[0].name + String.fromCharCode(3) + propertiesAdvanced[x].childNodes[z].childNodes[0].value + String.fromCharCode(2);
                }
            }
        }

        new Ajax.Request(ajax, {parameters:'pos='+target+'&reqid='+id+'&keys='+escape(paramsBasic) + escape(paramsAdvanced) +'&cookie='+document.cookie, onSuccess:function(t){ handlerFuncSaveProperties(t, goMain) }, onFailure:errFunc});

        }
    }

    function handlerFuncSaveProperties(t, noGoMain) {

        if (t.responseText.indexOf('RELOAD') >= 0) { window.location.reload(); }

        if (t.responseText.indexOf('the following problems') > 0) {
           $('errorProperties').innerHTML = t.responseText;
           Effect.Appear('errorProperties', { duration: 0.5, afterFinish: function(obj) {
           		// to keep it sticking to 'bottom' force a re-render (bug in IE)
           		$('saveProperties').hide().show();
           }});
        } else {
           if (!noGoMain) { Pages.go('main'); }
        }

    }

    //function showCreatePrompt(dontAnimate, dontFocus) {
    //  $('promptUser').update( '<font style="font-size: 20px; padding-left: 5px;">Title Your New Site</font><p style="padding: 23px 0;"><input type="text" id="pageTitle" style="border: 1px solid #aaa; margin-left: 5px; height: 26px; width: 307px; font-size: 20px;"/></p><a href="#" onClick="newSite($('+"'pageTitle'"+').value); return false;" style="margin-right: 5px; font-size: 14px; font-weight: bold;"><img src="'+/*tli(*/'/weebly/images/userhome/continue.gif'/*)tli*/+'" /></a><a href="#" onClick="Effect.SlideUp('+"'showPrompt'"+'); return false;" style="font-size: 14px; font-weight: bold;"><img src="'+/*tli(*/'/weebly/images/userhome/cancel.gif'/*)tli*/+'" /></a><div id="promptError" style="display: none; color: red;"></div>' );
	//  if (dontAnimate) {
	//      Element.show('showPrompt');
	//  }else{
	//      Effect.SlideDown('showPrompt');
	//  }
	//  if (!dontFocus) {
	//      setTimeout("$('pageTitle').focus();", 1000);
	//  }
    //}

    function shareLink(type) {

	if (type == "addressbook2") {
	  new Ajax.Request(ajax, {parameters:'pos=doevent&event=tellFriends&cookie='+document.cookie});
	  window.open('/weebly/apps/abook.php?site=showsomelove', 'abook', 'toolbar=0,status=0,width=700,height=526');
	} else {

	  var link = encodeURIComponent("http://"+$(siteSelected).childNodes[2].childNodes[0].innerHTML);
	  var title = encodeURIComponent($(siteSelected).childNodes[1].childNodes[0].innerHTML);

	  if (type == "facebook") {
	    window.open('http://www.facebook.com/sharer.php?u='+link+'&t='+title,'sharer','toolbar=0,status=0,width=700,height=436');
	  } else if (type == "stumbleupon") {
	    window.open('http://www.stumbleupon.com/submit?url='+link+'&title='+title, 'stumbleupon', 'toolbar=0,status=0,width=826,height=536');
	  } else if (type == "addressbook") {
	    window.open('/weebly/apps/abook.php?site='+siteSelected, 'abook', 'toolbar=0,status=0,width=700,height=426');
	  }

	}

    }

    function showDeletePrompt(pageid, node) {

	node.appendChild($('showPromptDelete'));
	$('promptUserDelete').innerHTML = '<h2 style="font-size: 16px;">'+/*tl(*/'Confirm Deletion'/*)tl*/+'</h2><br/><p style="font-size: 12px;">'+/*tl(*/'This action is permanent -- if you delete this site, there is no way to retrieve it.'/*)tl*/+'</p><br/><p style="font-size: 12px;"><a href="#" onClick="deleteSite('+"'"+pageid+"'"+'); Effect.SlideUp('+"'showPromptDelete'"+'); return false;" style="color: red; margin-right: 15px;"><img src="images/page_cross.gif" /><font style="position: relative; top:-3px; margin-left: 2px;">'+/*tl(*/'Yes, delete this site'/*)tl*/+'</font></a><a href="#" onClick="Effect.SlideUp('+"'showPromptDelete'"+'); return false;"><img src="images/page_next.gif" /><font style="position: relative; top:-3px; margin-left: 2px;">'+/*tl(*/'No, keep this site'/*)tl*/+'</font></a></p>';

	Effect.SlideDown('showPromptDelete');

    }

    function deleteSite(pageid) {

	new Ajax.Request(ajax, {parameters:'pos=deletesite&reqid='+pageid+'&cookie='+document.cookie, onSuccess:handlerDeleteSite, onFailure:errFunc, onException:exceptionFunc});

    }

    function handlerDeleteSite(t) {

	if (t.responseText.indexOf("%%SUCCESS%%") > -1) {
        document.location = 'userHome.php';
        } else {
	  showErrorMessage(/*tl(*/'There was an error removing your site. Please try again.'/*)tl*/); 
	}

    }

    function removeAdSense() {

	if ($('adsenseError')) { $('adsenseError').parentNode.removeChild($('adsenseError')); }
	new Ajax.Request(ajax, {parameters:'pos=removeadsenseaccount&cookie='+document.cookie, onSuccess:handlerRemoveAdSense, onFailure:errFunc, onException:exceptionFunc});

    }

    function handlerRemoveAdSense(t) {
	   window.location.reload();
    }

    function adsenseHideNew() {

	$("asnew_2").checked = true;
	$("asexisting_2").checked = true;
	$("adsense_newUser").style.display = "none"; 
	$("adsense_existingUser").style.display = "block";
	$("as_action").style.display = 'none';

    }

    function adsenseShowNew() {

	$("adsense_newUser").style.display = "block"; 
	$("adsense_existingUser").style.display = "none";
	//$("as_action").style.display = 'table-row';
	$("asnew_1").checked = true;
	$("asexisting_1").checked = true;

    }
    
    function removeUserNote( id )
    {
        $('user-note-' + id ).hide();
        new Ajax.Request(ajax, {parameters:'pos=deleteUserNote&note_id='+id+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc});
        var totalNotesLeft = 0;
        $$('div.notification').each( 
        	function(el){
        	    if( el.visible() ){
	        	    totalNotesLeft++;
        	    }
        	}
        );
        if( totalNotesLeft == 0 )
        {
            $( 'notifications-container' ).hide();
        }
    }

Event.observe( document, 'mousedown',clickHandler );
    function clickHandler(e) {
        var targ;
        if (!e) var e = window.event;
        if (e.target) targ = e.target;
        else if (e.srcElement) targ = e.srcElement;
        if (targ.nodeType == 3) // defeat Safari bug
              targ = targ.parentNode;

	if (isAParent('weeblyLightbox', targ) && !isAParent('weeblyLightboxInside', targ)) {
            // Lightbox outside click
            if ($('weeblyLightboxInside').down('#adsense_form')) {
                Pages.go("displayUserSettings", 'adsense');
            } else {
                Pages.go("main");
            }
        }

     }

    function removeExternalAccount(type) {

        new Ajax.Request(ajax, {parameters: 'pos=removeexternalaccount&type='+type+'&cookie='+document.cookie, 'onSuccess': function(t) { handlerRemoveExternalAccount(t, type); }, 'onFailure':errFunc});

    }

    function handlerRemoveExternalAccount(t, type) {
        window.location.reload();
    }

function getFlashVersion(){ 
  // ie 
  try { 
    try { 
      var axo = new ActiveXObject('ShockwaveFlash.ShockwaveFlash.6'); 
      try { axo.AllowScriptAccess = 'always'; } 
      catch(e) { return '6,0,0'; } 
    } catch(e) {} 
    return new ActiveXObject('ShockwaveFlash.ShockwaveFlash').GetVariable('$version').replace(/\D+/g, ',').match(/^,?(.+),?$/)[1]; 
  // other browsers 
  } catch(e) { 
    try { 
      if(navigator.mimeTypes["application/x-shockwave-flash"].enabledPlugin){ 
        return (navigator.plugins["Shockwave Flash 2.0"] || navigator.plugins["Shockwave Flash"]).description.replace(/\D+/g, ",").match(/^,?(.+),?$/)[1]; 
      } 
    } catch(e) {} 
  } 
  return '0,0,0'; 
}

function saveFlashVersion(){
    var version = getFlashVersion();
    new Ajax.Request(ajax, {parameters: 'pos=saveflashversion&version='+version+'&cookie='+document.cookie });
}

Weebly.blueBox = {

    open : function(contentID, options){
        Weebly.blueBox.callbacks = {};
        options = options ? options : {};
        if(typeof(options) === 'object' && typeof(options.onLoad) === 'function'){
            Weebly.blueBox.callbacks.onLoad = options.onLoad;
        }
        if(typeof(options) === 'object' && typeof(options.onClose) === 'function'){
            Weebly.blueBox.callbacks.onClose = options.onClose;
        }

        $('weebly-bluebox').setStyle({'height':$(document.body).getHeight() + 'px'});
        $('weebly-bluebox').show();
        if($(contentID)){
            $('weebly-bluebox-content').insert({top:$(contentID).show()});
        }
        $('weebly-bluebox-bg').onclick = Weebly.blueBox.close;
        $('weebly-bluebox').down('.weebly-bluebox-close').onclick = Weebly.blueBox.close;

        var contentHeight = $('weebly-bluebox-container').getHeight();
        var containerTop = (document.viewport.getHeight()/2) - (contentHeight/2) + document.viewport.getScrollOffsets()[1];
        containerTop = containerTop < 10 ? 10 : containerTop;
        $('weebly-bluebox-container').setStyle({'top':containerTop+'px'});

        $('weebly-bluebox-bordercontainer').setStyle({height: (contentHeight+26)+'px', top: (containerTop-13)+"px", position: 'absolute'});
        $('weebly-bluebox-border').setStyle({height: (contentHeight+26)+'px', opacity: "0.5"});

        if(options.width){
            $('weebly-bluebox-container').setStyle({width:options.width});
            $('weebly-bluebox-border').setStyle({width: (options.width.replace("px", "")-(-40))+"px"});
        }

        if(typeof(Weebly.blueBox.callbacks.onLoad) === 'function'){
            Weebly.blueBox.callbacks.onLoad();
        }
    },

    close : function(){
        document.body.appendChild($('weebly-bluebox-content').childNodes[0].hide());
        $('weebly-bluebox-content').update('');
        $('weebly-bluebox').hide();

        if(typeof(Weebly.blueBox.callbacks.onClose) === 'function'){
            Weebly.blueBox.callbacks.onClose();
        }
    }
}

var adminLogin = 0;

var activeSite = '';
var siteAreaOpen = false;
function slideSiteAreaOpen(site_id, tab, site_link){
    hideMore();
    siteAreaOpen = true;
    activeSite = site_id;
    
    $('edit-site-subnav').href = 'toSite.php?site='+site_id;
    var title = Weebly.Sites[site_id].site_title.length > 50 ? Weebly.Sites[site_id].site_title.substr(0,50)+'...' : Weebly.Sites[site_id].site_title;
    
    if (site_link && site_link != 'Not Published') {
    	$('site-data-area-title')
    		.update()
    		.insert(new Element('a', {
    			href: 'http://'+site_link,
    			target: '_blank',
    			title: /*tl(*/'Visit Site'/*)tl*/
    			})
    			.update(title));
    }else{
    	$('site-data-area-title').update(title);
    }
    
    $('fb-likes').hide();
    new Effect.Tween($('scroll-area'), 0, 960, { afterFinish:function(){
                unselectGreyButtons();
                selectGreyButton($('tab-button-'+tab));
            }
        }, 
        function(p){
            $('scroll-area').scrollLeft = p;
        }
    );
    changeSiteTab(tab);
}

function slideSiteAreaClosed(){
    siteAreaOpen = false;
    activeSite = '';
    new Effect.Tween($('scroll-area'), 960, 0,
		{
			afterFinish: function() {
				$('fb-likes').show();
			}
		},
		function(p){
		    $('scroll-area').scrollLeft = p;
		}
	);
}

function changeSiteTab(tab){
    $$('.site-tab').invoke('hide');
    if($(tab+'-'+activeSite)){
        $(tab+'-'+activeSite).show();
    }else if($(tab)){
        $(tab).show();
    }
    
    if(tab == 'form_data'){
        loadSiteForms(activeSite);
    }
     
    if(tab == 'blog_moderation'){
        loadSiteBlogs(activeSite);
    }
}

function showMore(site_id, site_link, site_title){
    var moreContainer = $('more-container');
    $('more-link-blog').onclick = function(){slideSiteAreaOpen(site_id, 'blog_moderation', site_link);};
    $('more-link-form').onclick = function(){slideSiteAreaOpen(site_id, 'form_data', site_link);};
    $('more-link-delete').onclick = function(){showConfirmationPopup('This action is permanent -- if you delete this site, there is no way to retrieve it.', function(){deleteSite(site_id)}, {title:'Delete '+site_title+'?', confirmButtonText:'Delete site', cancelButtonText:'cancel'}); return false;};
    moreContainer.clonePosition($('more-button-'+site_id), {setWidth: false, setHeight: false, offsetTop:28, offsetLeft:1});
    $('more-container').show();
    document.observe('mousedown', hideMore)
}

function hideMore(event){
    if(!event || !Event.element(event).up('.site-buttons-more')){
        if(!siteAreaOpen){
            unselectGreyButtons();
        }
        $$('.site-buttons-more').invoke('hide');
        document.stopObserving('mousedown', hideMore);
    }
}

function slideStatsDetailOpen(stat){
    $('stats-detail-area').morph('top:500px');
    loadStatsDetail(stat);
}

function slideStatsDetailClosed(){
    $('stats-detail-area').morph('top:1038px');
}

function loadStatsDetail(stat){
    var heading = 'Top Pages';
    var key_heading = 'Views';
    var value_heading = 'Page';
    var action = 'statstoppages';
    switch(stat){
        case 'top_pages':
            heading = 'Top Pages';
            key_heading = 'Views';
            value_heading = 'Page'; 
            action = 'statstoppages';
            break;
        case 'top_referring':
            heading = 'Referring Sites';
            key_heading = 'Views';
            value_heading = 'Referrer'; 
            action = 'statstopreferring';
            break;
        case 'top_searches':
            heading = 'Search Terms';
            key_heading = 'Views';
            value_heading = 'Keyword'; 
            action = 'statstopsearches';
            break;
    }
    $('detail-heading-title').update(heading);
    $('detail-key-heading').update(key_heading);
    $('detail-value-heading').update(value_heading);
    var params = {
        pos : action,
        site_id : activeSite,
        cookie : document.cookie
    }
    new Ajax.Request(ajax, {parameters: params, onSuccess: updateStatsDetail });
    $('detail-body').update('Loading...');
}

function updateStatsDetail(t){
    var items = t.responseText.evalJSON();
    var rowTemplate = new Template('<div class="detail-row"><div class="detail-key">#{pageviews}</div><div class="detail-value">#{value}</div></div>');
    $('detail-body').update('');
    items.each(function(item){
        $('detail-body').insert({bottom:rowTemplate.evaluate(item)});
    })
}

function loadSiteForms(site_id){
    new Ajax.Request(ajax, {parameters: 'pos=getuserforms&site_id='+site_id+'&cookie='+document.cookie, 
        onSuccess: 
            function(t){
                var forms = t.responseText.evalJSON();
                var options = '';
                if( forms.size() > 0 ){
                    $('no-site-forms').hide();
                    $('form-data-iframe').show();
                    $('site-form-controls').show();
                    siteForms = new Hash();
                    forms.each( 
                        function(form){
                            siteForms.set(form.ucfid, form);
                            var name = form.deleted ? form.name + ' [deleted]' : form.name;
                            options += '<option value="'+form.ucfid+'">'+name+'</option>';
                        }
                    );
                } else{
                    $('no-site-forms').show();
                    $('form-data-iframe').hide();
                    $('site-form-controls').hide();
                }
                $('site-forms-select').update(options);
                if($('site-forms-select').value){
                    showSiteFormData($('site-forms-select').value);
                }
            }
        }
    );
}

function showSiteFormData(ucfid){
    var form = siteForms.get(ucfid);
    $('form-data-iframe').show();
    $('form-data-iframe').src = 'viewFormData.php?ucfid='+form.ucfid+'&user_id='+form.user+'&hash='+form.hash;
    $('site-forms-entries').update(form.total);
    $('site-forms-view').href = form.link;
    $('site-forms-export').href = '/weebly/exportFormData.php?ucfid='+form.ucfid+'&user_id='+form.user+'&hash='+form.hash;
}

function loadSiteBlogs(site_id){
    new Ajax.Request(ajax, {parameters: 'pos=getsiteblogs&site_id='+site_id+'&cookie='+document.cookie, 
        onSuccess: 
            function(t){
                var blogs = t.responseText.evalJSON();
                var options = '';
                if( blogs.size() > 0 ){
                    $('no-site-blogs').hide();
                    $('blog-moderation-iframe').show();
                    $('blog-moderation-controls').show();
                    blogs.each( 
                        function(blog){
                            options += '<option value="'+blog.blog_id+'">'+blog.title+'</option>';
                        }
                    );
                } else{
                    $('blog-moderation-controls').hide();
                    $('blog-moderation-iframe').hide();
                    hideCommentsLoading();
                    $('no-site-blogs').show();
                }
                $('site-blogs-select').update(options);
                if($('site-blogs-select').value){
                    showBlogComments($('site-blogs-select').value, 'all');
                }
            }
        }
    );
}

function showBlogComments(blog_id, type){
    var link = $('show-blog-'+type);
    if(link){
        $$('.show-blog-selected').invoke('removeClassName', 'show-blog-selected');
        link.addClassName('show-blog-selected');
    }
    $('blog-moderation-iframe').show();
    $('blog-moderation-iframe').src = 'viewBlogModeration.php?blog_id='+blog_id+'&type='+type;
}

activeSetting = '';
function openAccountChange(setting){
    activeSetting = setting;
    $$('.account-change').invoke('hide');
    $('account-change-'+setting).show();
    $('account-change-error').hide();
    Weebly.blueBox.open('account-change-container', {width:'575px'});
}

function showAccountError(msg){
    $('account-change-error').update(msg);
    $('account-change-error').show();
}

function changeUsername(){
    $('account-change-error').hide();
    var params = {
        pos : 'changeusername',
        username : $('new-username').value,
        cookie : document.cookie
    }
    new Ajax.Request(ajax, {
        parameters:params, 
        onSuccess: function(t){
            var response = t.responseText;
            if(response.match('%%SUCCESS%%')){
                $('current-username').update($('new-username').value);
                Weebly.blueBox.close();
            }
            else{
                showAccountError(response);
            }
        }, 
        onFailure:errFunc, 
        onException:exceptionFunc
    });
}

function changeEmail(){
    $('account-change-error').hide();
    var params = {
        pos : 'changeuseremail',
        email : $('new-email').value,
        cookie : document.cookie
    }
    new Ajax.Request(ajax, {
        parameters:params, 
        onSuccess: function(t){
            var response = t.responseText;
            if(response.match('%%SUCCESS%%')){
                $('current-email').update($('new-email').value);
                Weebly.blueBox.close();
            }
            else{
                showAccountError(response);
            }
        }, 
        onFailure:errFunc, 
        onException:exceptionFunc
    });
}

function changePassword(){
    $('account-change-error').hide();
    
    var params = {
        pos : 'changeuserpassword',
        new_password : $('new-password').value,
        repeat_password : $('repeat-password').value,
        cookie : document.cookie
    }
    if(params.new_password == params.repeat_password && params.new_password.length >= 6){
        new Ajax.Request(ajax, {
            parameters:params, 
            onSuccess: function(t){
                var response = t.responseText;
                if(response.match('%%SUCCESS%%')){
                    Weebly.blueBox.close();
                }
                else{
                    showAccountError(response);
                }
            }, 
            onFailure:errFunc, 
            onException:exceptionFunc
        });
    } else {
        showAccountError('New password must match repeat password and be at least 6 characters long');
    }
    
}

function showRemoveAlert(msg, service){
    $('remove-setting-confirm-text').update(msg);
    if(service == 'adsense'){
        $('remove-setting-confirm-button').onclick = function(){
            removeAdSense();
            return false;
        };
    } else{
        $('remove-setting-confirm-button').onclick = function(){
            removeExternalAccount(service);
            return false;
        };
    }
    openAccountChange('remove');
}

function showConfirmationPopup(msg, action, options){
    options = Object.extend({
        title: 'Confirm Change',
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }, options || {});
    $('confirmation-popup-error').update('');
    $('confirmation-popup-text').update(msg);
    $('confirmation-popup-title').update(options.title);
    $('confirmation-popup-confirm-button-text').update(options.confirmButtonText);
    $('confirmation-popup-cancel-button-text').update(options.cancelButtonText);
    $('confirmation-popup-confirm-button').onclick = action;
    Weebly.blueBox.open('confirmation-popup', {width:'575px'});
}

//Change grey buttons to black on click
document.observe('dom:loaded', function(){
    $$('a.button-grey').each(function(el){
        if(el.id != 'site-blog-moderation-delete-button'){
                el.observe('click', function(){
                unselectGreyButtons();
                selectGreyButton(el);
            });
        }
    });
});

function unselectGreyButtons(){
    $$('.button-grey-selected').invoke('removeClassName', 'button-grey-selected');
}

function selectGreyButton(el){
    el.addClassName('button-grey-selected');
    el.down('span.button-grey').addClassName('button-grey-selected');
}

function removeSubscription(subscriptionID){
    var params = {
        pos: 'removesubscription',
        user_service_id: subscriptionID,
        cookie: document.cookie
    };
    new Ajax.Request(ajax, {
        parameters:params, 
        onSuccess: function(t){
            if(t.responseText.match('%%SUCCESS%%')){
                window.location.reload();
            } else{
                $('confirmation-popup-error').update('There was an error removing auto-renew.  Please try again.');
            }
            
        }, 
        onFailure:errFunc, 
        onException:exceptionFunc
    });
}

function hideCommentsLoading(){
    $('comments-loading').hide();
}



// education

function openEducationForm(){
	$('education-form').show();
	Weebly.blueBox.open('education-form-container', {width:'575px'});
}

function saveEducationForm() {
	var settings = {};
	$('education-form').select('input,select').each(function(input) {
		settings[input.id.match(/ed-(\w+)-input/)[1]] = input.value;
	});
	var ajaxSettings = {};
	for (name in settings) {
		if (name.match(/(school|district|state|country)/)) {
			ajaxSettings['ed_' + name] = settings[name];
		}else{
			ajaxSettings[name] = settings[name];
		}
	}
	new Ajax.Request('getElements.php', {
		parameters: {
			pos: 'edusersettings',
			settings: Object.toJSON(ajaxSettings)
		},
		onSuccess: function(transport) {
			if ($('ed-full-name')) {
				$('ed-full-name').innerHTML = escapeh((settings.honorific + " " + (settings.fname + " " + settings.lname).strip()).strip());
			}else{
				$('ed-fname').innerHTML = escapeh(settings.fname);
				$('ed-lname').innerHTML = escapeh(settings.lname);
			}
			for (name in settings) {
				if (name != 'fname' && name != 'lname' && name != 'honorific') {
					$('ed-' + name).innerHTML = escapeh(settings[name]);
				}
			}
			if (settings.country == 'United States') {
				$('ed-state-row').show();
			}else{
				$('ed-state-row').hide();
			}
			Weebly.blueBox.close();
		}
	});
	function escapeh(s) {
		return s.replace(/</g, '&lt;').replace(/>/g, '&gt;');
	}
}

function edCountryChange(select) {
	if (select.value == "United States") {
		$('ed-state-input-row').style.visibility = 'visible';
	}else{
		$('ed-state-input-row').style.visibility = 'hidden';
	}
}

