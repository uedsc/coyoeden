

var oldTheme, newTheme, forkThemeDialog, advancedEditingDoneCallback;

function editCustomTheme(fullCustomThemeID, makeCopy, doneCallback)
{
	advancedEditingDoneCallback = doneCallback;
	newTheme = fullCustomThemeID;
	if (!isDesigner || !newTheme) {
		makeCopy = false;
	}
	//
	makeCopy = false; // always disable forking (for now)
	//
	if (typeof makeCopy == 'undefined') {
		if (!forkThemeDialog) {
			forkThemeDialog = new Weebly.Dialog('theme-fork-dialog', {
				zIndex: 1000,
				modal: true
			});
		}
		forkThemeDialog.open();
	}else{
		oldTheme = currentTheme;
		if (!fullCustomThemeID) {
			currentThemeTemp = currentTheme;
			currentTheme = 'blank';
		}else{
			currentTheme = fullCustomThemeID;
		}
		advancedEditing(makeCopy, doneCallback);
	}
}

function continueEditCustomTheme(makeCopy) // called by forkThemeDialog
{
	oldTheme = currentTheme;
	currentTheme = newTheme;
	advancedEditing(makeCopy);
}


var _advancedEditingForceCopy;

function advancedEditing(forceCopy)
{
	if (isDesigner && typeof forceCopy == 'undefined' && isCustomTheme(currentTheme)) {
		editCustomTheme(currentTheme);
		return;
	}
	_advancedEditingForceCopy = forceCopy;
    if(Weebly.Restrictions.hasAccess('custom_themes')){
        if( !$H(userEvents).get('viewCustomThemeWarning') )
        {
            if( confirm(/*tl(*/'Advanced Editing allows you to customize the layout and style of your Weebly theme.  This requires that you are familiar with HTML and CSS.  Weebly does not provide support for the HTML and CSS coding aspects of the Advanced Editor.\n\nClick "OK" to proceed to the Advanced Editing screen.'/*)tl*/ ) )
            {
                Pages.go('customThemeEditor');
            }
            new Ajax.Request(ajax, {parameters:'pos=doevent&event=viewCustomThemeWarning&cookie='+document.cookie});
            userEvents['viewCustomThemeWarning'] = 1;
        }
        else
        {
            Pages.go('customThemeEditor');
        }
    }
    else{
        if(typeof(upgrade_url) !== 'undefined'){
            window.open(upgrade_url+'?service='+Weebly.Restrictions.requiredService('custom_themes'), 'weebly_billingPage', 'height=550,width=910,menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
        }
        else{
            alertProFeatures('Please upgrade to edit your theme');
        }
    }
}

function openCustomThemeEditor() {
    $$('div.tab').each(function(e){e.setStyle({top: '14px'}); updateRoundedTabColor(e, '#E9E9E9');});
	hasUsedThemeEditor = 1;
	if (!ENABLE_THEME_BROWSER) {
		showThemes('All');
		highlightThemeCategory($('tc_all'));
	}
	$('custom-theme-image-preview').update('');
	if (!ENABLE_THEME_BROWSER) {
		var cover = new Element( 'div', {'id': 'custom-designs-cover'} );
		cover.setStyle({background: '#222222', width: '100%', height: '134px', position: 'absolute', left: '0px', top: '0px', 'zIndex': 13, opacity: '0.8', borderTop: '1px solid black'});
		$('body').insert(cover);
	}
	new Ajax.Request(ajax, {parameters:'pos=getthemesrc&keys='+currentTheme+'&type=html&cookie='+document.cookie, onSuccess:handlerOpenCustomThemeEditor, onFailure:errFunc});
}

function continueOpenCustomThemeEditor()
{
    themeSelectTab($('themeEditHTML'));
	$('custom-theme-upload-box').hide();//Fix IE bug
	$('custom-theme-upload-box').show();
	if( _advancedEditingForceCopy || !isCustomTheme(currentTheme) )
	{
		new Ajax.Request(ajax, {parameters:'pos=setupnewcustomtheme&html='+encodeURIComponent(temphtml)+'&css='+encodeURIComponent(tempcss)+'&currentTheme='+currentTheme+'&cookie='+document.cookie, onSuccess:handlerSaveCustomTheme, onFailure:errFunc, asynchronous: false});
		//currentTheme = 'custom_'+currentEditingThemeName+'_'+currentEditingThemeID;
	}
	else
	{
		currentEditingThemeName = getCustomThemeName(currentTheme);
		currentEditingThemeID = getCustomThemeID(currentTheme);
	}
	//updateThemeRollbackPoint();
	
	new Ajax.Request(ajax, {parameters:'pos=getthemefiles&theme='+makeCustomThemeName(currentEditingThemeName, currentEditingThemeID)+'&cookie='+document.cookie, onSuccess:handlerGetThemeFiles, onFailure:errFunc});
	setupThemeFileUploader();
	var themeExportLink = $('theme-export-link');
	if (themeExportLink) {
		themeExportLink.writeAttribute( {href: '/weebly/exportThemeZip.php?id='+currentEditingThemeID} ); 
	}
    if( !$H(userEvents).get('viewCustomThemeHelpMessage') )
    {
        showTip('Click here for help customizing your theme', 'themeEditHelp', 'y');
        new Ajax.Request(ajax, {parameters:'pos=doevent&event=viewCustomThemeHelpMessage&cookie='+document.cookie});
        userEvents['viewCustomThemeHelpMessage'] = 1;
    }
}

function exitCustomThemeEditor()
{
	if( typeof(currentEditingThemeName) !== 'undefined' && currentEditingThemeName !== 'WEEBLY__UNSAVED' )
	{
		var custTheme = makeCustomThemeName(currentEditingThemeName, currentEditingThemeID);
		Weebly.Cache.insert( 'pos=gettheme&keys='+custTheme, 'empty' );
		Weebly.Cache.insert( 'pos=getthememenu&keys='+custTheme, 'empty' );
		currentStyleNum = Math.floor(Math.random()*10000000001);
		if (advancedEditingDoneCallback) {
			currentTheme = oldTheme;
		}else{
			if (oldTheme) {
				selectTheme(oldTheme);
			}else{
				selectTheme(custTheme);
			}
		}
	}
	else
	{
		if (!advancedEditingDoneCallback) {
			if (oldTheme) {
				currentTheme = oldTheme;
			}
			Pages.go('themesMenu');
		}
	}
	if (advancedEditingDoneCallback) {
		advancedEditingDoneCallback();
		advancedEditingDoneCallback = null;
	}
	oldTheme = null;
}

function saveAndExitThemeEditor(name)
{
	if( saveCustomTheme(name) )
	{
		exitCustomThemeEditor();
	}
}

function closeCustomThemeEditor()
{
	if( $('custom-designs-cover') )
	{
		$('custom-designs-cover').remove();
	}
	$('customizeTheme').show();
	delete currentEditingThemeName;
	delete currentEditingThemeID;
	delete pending_html;
	delete pending_css;
	delete rollback_html;
	delete rollback_css;
	if(currentTheme === 'blank')
	{
		currentTheme = currentThemeTemp;
	}
}

function updateThemeRollbackPoint()
{
	if( typeof(pending_html) === 'undefined' )
	{
		pending_html = themeEditBoxHTML.getCode();
		pending_css = themeEditBoxCSS.getCode();
		rollback_html = pending_html;
		rollback_css = pending_css;
	}
	else
	{
		rollback_html = pending_html;
		rollback_css = pending_css;
		pending_html = themeEditBoxHTML.getCode();
		pending_css = themeEditBoxCSS.getCode();
	}
}

function rollbackThemeChanges()
{
	themeEditBoxHTML.setCode(rollback_html);
	themeEditBoxHTML.editor.syntaxHighlight('init');
	themeEditBoxCSS.setCode(rollback_css);
	themeEditBoxCSS.editor.syntaxHighlight('init');
	delete pending_html;
	updateThemeRollbackPoint();
}

function addNewDesignPicture()
{
	if( typeof(currentEditingThemeName) !== 'undefined' && currentEditingThemeName !== 'WEEBLY__UNSAVED' )
	{
		var custTheme = makeCustomThemeName(currentEditingThemeName, currentEditingThemeID);
		var displayName = currentEditingThemeName;
		
		function htmlEscape(s) {
			return s
				.replace(/&/g, '&amp;')
				.replace(/</g, '&lt;')
				.replace(/>/g, '&gt;')
				.replace(/'/g, '&#039;')
				.replace(/"/g, '&quot;');
		}
		
		showThemes('All');
		
		// completely ripped from theme_browser/includes/templates/weebly_editor_list.php
		$('themePicturesInner').insert({
			top: "<a class='themePicture' href='#' onclick=\"selectTheme('" + custTheme + "');return false\" " +
				"style='float:left;border:0px;text-decoration:none' " +
				"onmouseover=\"this.className='themePicture themePicture-hover';themeOver('" + custTheme + "')\" " +
				"onmouseout=\"this.className='themePicture';themeOut('" + custTheme + "')\"> " +
					"<div class='themePictureImg' id='theme_" + custTheme + "'>" +
						"<div class='themePicturePlaceholder'>" +
							"<div><span>" + htmlEscape(displayName) + "</span></div>" +
						"</div>" +
					"</div>" +
					"<span class='themeFavoriteIcon themeIsFavorite' " +
						"onmouseover='mouseOverFavorite=true' " +
						"onmouseout='mouseOverFavorite=false'" +
						"></span>" +
				"</a>"
		});
        
		if (ENABLE_THEME_BROWSER) {
			var a = $$('#themePicturesInner a')[0];
			initThemeFavoriting(custTheme, a.select('span.themeFavoriteIcon')[0], a, 'themeIsFavorite');
		}else{
			Weebly.Cache.insert('pos=getthemes&keys=', $('themePicturesInner').innerHTML);
		}
		
	}
}

function isCustomTheme( themeName )
{
	return themeName.substr( 0, 6 ) === 'custom';
}

function makeCustomThemeName( name, id )
{
	return 'custom_'+name+'_'+id;
}

function getCustomThemeID( themeName )
{
	return themeName.replace( /custom_.*_(.*)$/, '$1' );
}

function getCustomThemeName( themeName )
{
	return themeName.replace( /custom_(.*)_.*$/, '$1' );
}

function handlerOpenCustomThemeEditor(t) {
    if(typeof(themeEditorHTML) === 'undefined'){
        $('themeEditBoxHTML').value = t.responseText;
        themeEditorHTML = CodeMirror.fromTextArea('themeEditBoxHTML', {
            height: "100%",
            parserfile: ["parsexml.js", "parsecss.js", "tokenizejavascript.js", "parsejavascript.js", "parsehtmlmixed.js"],
            stylesheet: "libraries/codemirror/css/xmlcolors.css",
            path: "libraries/codemirror/js/",
            continuousScanning: 500,
            lineNumbers: true,
            textWrapping: false
        });
    }
    else{
        themeEditorHTML.setCode(t.responseText);
    }

    temphtml = t.responseText;

	$('customizeTheme').hide();

	$('customThemeContainer').style.visibility = "visible";
	new Ajax.Request(ajax, {parameters:'pos=getthemesrc&keys='+currentTheme+'&type=css&cookie='+document.cookie, onSuccess:handlerOpenCustomThemeEditor2, onFailure:errFunc});
}

function handlerOpenCustomThemeEditor2(t) {
    if(typeof(themeEditorCSS) === 'undefined'){
        $('themeEditBoxCSS').value = t.responseText;
        themeEditorCSS = CodeMirror.fromTextArea('themeEditBoxCSS', {
            height: "100%",
            parserfile: ["parsecss.js"],
            stylesheet: "libraries/codemirror/css/csscolors.css",
            path: "libraries/codemirror/js/",
            continuousScanning: 500,
            lineNumbers: true,
            textWrapping: false
        });
    }
    else{
        themeEditorCSS.setCode(t.responseText);
    }
    
    themeEditorCSS.hide();
	
	tempcss = t.responseText;
	
	continueOpenCustomThemeEditor();
}

function handlerGetThemeFiles(t)
{
	$('theme-files').update('');
	var files = t.responseText.evalJSON();
	files.each( 
		function(file){
			var el = new Element( 'option', { 'class': 'theme-file' } ).update( file );
			$('theme-files').insert( {bottom: el} );
		}
	);
}

function themeSelectTab(tab) {
	$$('div.tab').each(function(e){e.setStyle({top: '14px'}); updateRoundedTabColor(e, '#E9E9E9');});
	$(tab).setStyle({top: '15px'});
	updateRoundedTabColor($(tab), '#FFFFFF');

    themeEditorHTML.hide();
    themeEditorCSS.hide();
	$('themeEditBoxFiles').hide();
    $('themeEditBoxPreview').hide();
    $('themeEditBoxHelp').hide();
	var id = 'themeEditBox' + $(tab).identify().substr(9);
	if( id === 'themeEditBoxHTML' )
	{
		themeEditorHTML.show();
	}
    else if( id === 'themeEditBoxCSS' )
	{
		themeEditorCSS.show();
	}
	else
	{
        if(id === 'themeEditBoxHelp' && !$(id).down('iframe')){
            $(id).update('<iframe frameborder="0" src="http://customthemes.weebly.com" style="width:100%; height:100%; border:none;"></iframe>');
        }
		$(id).show();
	}
}

function updateRoundedTabColor(tab, color)
{
	if( tab.getStyle('background') || ( tab.getStyle('backgroundColor') && tab.getStyle('backgroundColor')!='transparent' ) )
	{
		tab.setStyle({background: color});
	}
	tab.childElements().each( function(e){updateRoundedTabColor(e, color );} );
}

function saveCustomTheme(name)
{
	var newTheme = 0;
	if( currentEditingThemeName == 'WEEBLY__UNSAVED' )
	{
		newTheme = 1;
		if( typeof(name) == 'undefined' || name.length === 0 )
		{
			drawSaveThemeNameBox();
			return false;
		}
	}
	else
	{
		name = currentEditingThemeName;
	}
	
	if( $('theme-save-name-box') )
	{
		$('theme-save-name-box').remove();
	}
	var html = themeEditorHTML.getCode();
	var css = themeEditorCSS.getCode();
	
	new Ajax.Request(ajax, {parameters:'pos=savecustomtheme&name='+name+'&newTheme='+newTheme+'&html='+encodeURIComponent(html)+'&css='+encodeURIComponent(css)+'&currentTheme='+currentTheme+'&cookie='+document.cookie, onFailure:errFunc, onSuccess:handlerSaveCustomTheme, asynchronous: false});
	return true;
}

function handlerSaveCustomTheme(t)
{
	var response = t.responseText;
	if( response.isJSON() )
	{
		var themeData = response.evalJSON();
		if( typeof(currentEditingThemeName) !== 'undefined' && currentEditingThemeName != themeData.theme_name )
		{
			//new Ajax.Request(ajax, {parameters:'pos=settheme&keys=custom_'+themeData.theme_name+'_'+themeData.custom_theme_id+'&cookie='+document.cookie, onFailure:errFunc, bgRequest: true});
			currentEditingThemeName = themeData.theme_name;
			currentEditingThemeID = themeData.custom_theme_id;
			addNewDesignPicture();
		}
		else
		{
			currentEditingThemeName = themeData.theme_name;
			currentEditingThemeID = themeData.custom_theme_id;
		}
	}
}

function deleteCustomTheme(themeName, squareElement)
{
	if( confirm('Are you sure you want to delete this theme?') )
	{
		deletedThemeName = themeName;
		var themeID = getCustomThemeID( themeName );
		new Ajax.Request(ajax, {parameters:'pos=deletecustomtheme&customThemeID='+themeID+'&cookie='+document.cookie, onFailure:errFunc, onSuccess: handlerDeleteCustomTheme});
	}
}

function handlerDeleteCustomTheme(t)
{
	showThemes('All');
	highlightThemeCategory($('tc_all'));
	if( $('theme_'+deletedThemeName) ) 
	{
		var pic = $('theme_'+deletedThemeName).up();
		pic.remove();
		if (!ENABLE_THEME_BROWSER) {
			Weebly.Cache.insert( 'pos=getthemes&keys=', $('themePicturesInner').innerHTML );
		}
	}
	if( currentTheme == deletedThemeName )
	{
		currentTheme = Weebly.defaultTheme;
		selectTheme(Weebly.defaultTheme);
	}
}

function drawSaveThemeNameBox()
{
	var saveButtonPos = $('theme-save-button').cumulativeOffset();
	var box = new Element( 'div', {id: 'theme-save-name-box', 'class': 'theme-save-name-box-class'} );
	box.setStyle( {left: (saveButtonPos.left-265)+'px'} );
	box.update( '<form style="margin-top: 4px; margin-left:15px;" onsubmit="saveAndExitThemeEditor($F(\'save-theme-name\')); return false;"><table><tr><td>Save As: </td><td><input id="save-theme-name" style="border: 1px solid #9A9A9A; height:22px; width:150px; font-size:16px;" type="text" value="MyTheme" /></td><td><input type="image" src="images/save_theme_blue.jpg" value="Save" /></td><td><img src="images/cancel_button.jpg" style="cursor: pointer;" onclick="$(\'theme-save-name-box\').remove()" /></td></tr></table></form>' );
	$('customThemeContainer').insert( {bottom: box} );
	$('save-theme-name').select();
	
	var settings = {tl: { radius: 5 },tr: { radius: 5 },bl: { radius: 5 },br: { radius: 5 },antiAlias: true,autoPad: false};
	var corners = new curvyCorners(settings, "theme-save-name-box-class");
	corners.applyCornersToAll();
	$('theme-save-name-box').style.padding = "0 0 7px 0";
}

function drawRenameBox()
{
	var selected = getSelectedThemeFile();
	if( selected )
	{
		currentSelectedThemeFile = $F('theme-files');
		var topoffset = selected.viewportOffset().top - 257;
		var leftoffset = selected.positionedOffset().left;
        var width = selected.getWidth();
		topoffset = getSelectedThemeFileTopOffset();//selected.positionedOffset().top - selected.cumulativeScrollOffset().top;// + getSelectedThemeFileTopOffset() + 0;
        leftoffset = selected.positionedOffset().left;
		var input = new Element( 'input', {type: 'text', value: $F('theme-files') } );
		input.setStyle({border: '1px solid #000000', height: '15px', width: '206px', padding: '0px', position: 'absolute', top: topoffset+'px', left:leftoffset+'px', zIndex: '20'} );
		input.observe( 'blur', handleRenameEvent);
		input.observe( 'keypress', function(event){if(event.keyCode==Event.KEY_RETURN){handleRenameEvent(event);}});
		$('custom-theme-upload-box').insert( {bottom: input} );
		input.select();
	}
}

function handleRenameEvent(event)
{
	var el = Event.element(event);
	var newName = el.value;
	if( newName != currentSelectedThemeFile )
	{
		new Ajax.Request(ajax, {parameters:'pos=renamethemefile&customThemeID='+currentEditingThemeID+'&oldName='+currentSelectedThemeFile+'&newName='+newName+'&cookie='+document.cookie, onSuccess:function(t){handlerGetThemeFiles(t);el.remove();}, onFailure:errFunc});
	}
	else
	{
		el.remove();
	}
}

function getSelectedThemeFile()
{
	var options = $$('option.theme-file');
	for( var i=0; i<options.size(); i++ )
	{
		if( options[i].selected )
		{
			return options[i];
		}
	}
	return false;
}

function getSelectedThemeFileTopOffset()
{
	var options = $$('option.theme-file');
    var multiplier = Prototype.Browser.IE ? 16 : 17;
	for( var i=0; i<options.size(); i++ )
	{
		if( options[i].selected )
		{
			return (multiplier * (i+1)) - options[i].cumulativeScrollOffset().top + (Prototype.Browser.IE ? 1 : 0);
		}
	}
	return false;
}

function updateSavedCustomThemes()
{
	new Ajax.Request(ajax, {parameters:'pos=getsavedthemes&cookie='+document.cookie, onFailure:errFunc, onSuccess:handleUpdateSavedCustomThemes});
}

function handleUpdateSavedCustomThemes(t)
{
	$('savedCustomThemes').update('');
	var themes = t.responseText.evalJSON(true);
	for( var i=0; i<themes.size(); i++ )
	{
		var li = new Element('li', {'class': 'savedThemeName'}).update(themes[i].theme_name);
		li.observe('click', function(e){$('themeEditName').value=Event.element(e).innerHTML;} );
		$('savedCustomThemes').insert({bottom: li});
	}
}

function updateThemePreview()
{
	var html = themeEditorHTML.getCode();
	var css = themeEditorCSS.getCode();
	new Ajax.Request('/weebly/apps/preview.php', {parameters:'themeID='+currentEditingThemeID+'&userID='+userID+'&siteID='+currentSite+'&pageID='+currentPage+'&template='+encodeURIComponent(html)+'&css='+encodeURIComponent(css)+'&cookie='+document.cookie, onFailure:errFunc, onSuccess:handlerUpdateThemePreview});
}

function handlerUpdateThemePreview(t)
{
	var e = $('customThemePreview');
	e.contentWindow.document.close();
	e.contentWindow.document.open();
	e.contentWindow.document.write(t.responseText);
	updateThemeRollbackPoint();
}

function deleteThemeFile(fileName)
{
	new Ajax.Request(ajax, {parameters:'pos=deletethemefile&customThemeID='+currentEditingThemeID+'&file='+encodeURIComponent(fileName)+'&cookie='+document.cookie, onFailure:errFunc, onSuccess:handlerGetThemeFiles});
}

function updateCustomThemeImagePreview()
{
	if( $F('theme-files') )
	{
		var ext = $F('theme-files').slice(-3).toLowerCase();
		if( ext === 'gif' || ext === 'png' || ext === 'jpg' )
		{
			var imagePath = getCustomThemePath(currentEditingThemeID)+'files/'+$F('theme-files')+'?'+Math.ceil(Math.random()*100000);
			var img = new Image();
			$(img).observe( 'load', updateCustomThemeImagePreviewDimensions);
			img.src = imagePath;
			$('custom-theme-image-preview').update( '<div style="text-align:left; width: 225px; color:#666666; font-size:11px;"><b>'+/*tl(*/'Preview'/*)tl*/+':</b><br /><div style="text-align:center; margin-top:3px;"><a href="'+imagePath+'" target="_blank"><img id="theme-file-preview-image" style="max-width:213px; max-height:63px; min-width:1px; min-height:1px" src='+imagePath+' /></a></div><table style="margin-left:40px;"><tr><td style="color:#000000; font-weight:bold; text-align:right">'+/*tl(*/'Name'/*)tl*/+'</td><td>'+$F('theme-files')+'</td></tr><tr><td style="color:#000000; font-weight:bold; text-align:right">'+/*tl(*/'Dimensions'/*)tl*/+'</td><td id="theme-file-preview-dimensions"></td></tr></table></div>' );
		}
		else
		{
			$('custom-theme-image-preview').update( '<div style="text-align:left; clear:both; color:#666666; font-size:11px; font-weight: bold;">'+/*tl(*/'Preview not available for this file type.'/*)tl*/+'</div>' );
		}
	}
}

function updateCustomThemeImagePreviewDimensions(event)
{
	$('theme-file-preview-dimensions').update( this.width + ' x ' + this.height );
	if( !$('theme-file-preview-image').getStyle('maxHeight') )//Fix IE 6
	{
		if( $('theme-file-preview-image').getHeight() > 63 )
		{
			$('theme-file-preview-image').setStyle({'height':'63px'});
		}
		if( $('theme-file-preview-image').getWidth() > 213 )
		{
			$('theme-file-preview-image').setStyle({'width':'213px'});
		}
	}
}

function getCustomThemePath(themeID)
{
	var path = '/uploads/';
	for( var i = 0; i<4; i++ )
	{
		var charac = userID.charAt(i);
		if( charac )
		{
			path = path + charac + '/';
		}
		else
		{
			path = path + '_/';
			break;
		}
	}
	return path + userID + '/custom_themes/' + themeID + '/';
}

function showCustomThemes()
{
	if (!ENABLE_THEME_BROWSER) {
		showThemes('All');
		$('themePicturesInner').childElements().each(
			function(e){
				if(!e.down().id.match('custom'))
				{
					e.remove();
				}
			}
		);
	}

    var overlaySrc = typeof(proElementOverlaySrc) != 'undefined' ? proElementOverlaySrc : 'images/pro-element-overlay.png';
	var pic = new Element( 'a', 
		{href: '#', 
		'class': 'themePicture'} 
	);
	pic.observe('click', 
		function(event){
            if(Weebly.Restrictions.hasAccess('custom_themes')){
                currentThemeTemp=currentTheme;
                currentTheme='blank';
                Pages.go('customThemeEditor'); return false;
            }
            else{
                if(typeof(upgrade_url) !== 'undefined'){
                    window.open(upgrade_url+'?service='+Weebly.Restrictions.requiredService('custom_themes'), 'weebly_billingPage', 'height=550,width=910,menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
                }
                else{
                    alertProFeatures('Please upgrade to create a theme');
                }
            }
		}
	);
	pic.setStyle( {'float': 'left', border: '0px', textDecoration: 'none'} );
	var div = new Element( 'div', {id: 'theme_blank', 'class': 'themePictureImg'} );
	div.setStyle({width: '85px', height: '85px', background: 'url(/weebly/images/new_theme.jpg)', position:'relative'});
    if(Weebly.Restrictions.requiresUpgrade('custom_themes')){
        div.update('<img src="'+overlaySrc+'" class="pro-element-overlay">');
    }
	pic.insert( div );
	$('themePicturesInner').insert( {top: pic} );
	
	var pic = new Element( 'a', 
		{href: '#', 
		'class': 'themePicture'} 
	);
	pic.observe('click',
		function(event){
            if(Weebly.Restrictions.hasAccess('import_theme')){
                Pages.go('importTheme');
            }
            else{
                if(typeof(upgrade_url) !== 'undefined'){
                    window.open(upgrade_url+'?service='+Weebly.Restrictions.requiredService('import_theme'), 'weebly_billingPage', 'height=550,width=910,menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
                }
                else{
                    alertProFeatures('Please upgrade to import your theme');
                }
            }
			return false;
		}
	);
	pic.setStyle( {'float': 'left', border: '0px', textDecoration: 'none'} );
	var div = new Element( 'div', {id: 'theme_blank', 'class': 'themePictureImg'} );
	div.setStyle({width: '85px', height: '85px', background: 'url(/weebly/images/import.jpg)', position:'relative'});
    if(Weebly.Restrictions.requiresUpgrade('custom_themes')){
        div.update('<img src="'+overlaySrc+'" class="pro-element-overlay">');
    }
	pic.insert( div );
	$('themePicturesInner').insert( {top: pic} );
}

function setupThemeFileUploader()
{
    if(typeof(themeFileUploader) === 'undefined'){
        var uploadSettings = {
            upload_url: "/weebly/themeFileUpload.php?customThemeID="+currentEditingThemeID+"&cookies="+escape(document.cookie),
            flash_url : "/weebly/libraries/swfupload/swfupload.swf",
            file_size_limit : maxFileSize,
            file_types : "*.*",
            file_types_description : "All files...",
            button_image_url: "../.."+/*tli(*/"/weebly/images/upload_button.png"/*)tli*/,	// Relative to the Flash file
            button_width: "91",
            button_height: "24",
            button_placeholder_id: "theme-file-upload-button",
            file_dialog_complete_handler : themeFileDialogComplete,
            file_queued_handler : themeFileQueued,
            upload_start_handler: themeFileUploadStart,
            upload_progress_handler : themeFileUploadProgress,
            upload_success_handler : themeFileUploadSuccess,
            upload_error_handler : themeFileUploadError,
            upload_complete_handler : themeFileUploadComplete
        };
        themeFileUploader = new SWFUpload(uploadSettings);
    }
}

function themeFileDialogComplete()
{
	themeFileUploader.startUpload();
}

function themeFileQueued(file)
{
	var el = new Element( 'div', {'class': 'theme-queue-item', id: 'upload-item-'+file.id} ).update( file.name );
	el.insert( {bottom: ' <span id="upload-status-'+file.id+'"></span><div class="theme-progress-outer"><div class="theme-progress-inner" id="progress-inner-'+file.id+'"></div></div>'} );
	$('theme-upload-queue').insert( {bottom: el} );
}

function themeFileUploadStart(file)
{
	themeFileUploader.setUploadURL("/weebly/themeFileUpload.php?customThemeID="+currentEditingThemeID+"&cookies="+escape(document.cookie));
}

function themeFileUploadProgress(file, bytesComplete, totalBytes)
{
	var percentComplete = Math.floor( 100 * bytesComplete / file.size );
	$('progress-inner-'+file.id).setStyle( {width: percentComplete+'%'} );
}

function themeFileUploadError(file, errorCode, message)
{
	$('upload-status-'+file.id).update('Error: upload failed');
}

function themeFileUploadSuccess( file, data )
{
	//$('upload-status-'+file.id).update('Upload complete');
	if( data.isJSON() )
	{
		handlerGetThemeFiles({responseText: data});
	}
}

function themeFileUploadComplete(file)
{
	Effect.BlindUp('upload-item-'+file.id, {queue: 'end'});
	themeFileUploader.startUpload();
}

function displayCustomThemeImport()
{
	
}

function setupThemeImporter()
{
	var uploadSettings = {
		upload_url: "/weebly/themeImport.php?cookies="+escape(document.cookie),
		flash_url : "/weebly/libraries/swfupload/swfupload.swf",
		file_size_limit : maxFileSize,
        file_types : "*.zip",
        file_types_description : "Zip Archives",
        button_image_url: "../.."+/*tli(*/"/weebly/images/upload_theme_button_states.png"/*)tli*/,	// Relative to the Flash file
		button_width: "116",
		button_height: "30",
		button_placeholder_id: "theme-import-button",
		file_dialog_complete_handler : themeImportDialogComplete,
		upload_start_handler: themeImportUploadStart,
		upload_progress_handler : themeImportUploadProgress,
		upload_success_handler : themeImportUploadSuccess,
		upload_error_handler : themeImportUploadError,
		upload_complete_handler : themeImportUploadComplete
	};
	themeImportUploader = new SWFUpload(uploadSettings);
}

function themeImportDialogComplete()
{
	themeImportUploader.startUpload();
}

function themeImportUploadStart(file)
{
	$('theme-import-status').update('File: '+file.name+'<br />'+/*tl(*/'Status'/*)tl*/+': <span id="theme-import-status-short">'+/*tl(*/'Uploading'/*)tl*/+'</span><div class="theme-progress-outer"><div class="theme-progress-inner" id="import-progress-inner-'+file.id+'"></div></div><div id="theme-import-status-long"></div>');
}

function themeImportUploadProgress(file, bytesComplete, totalBytes)
{
	var percentComplete = Math.floor( 100 * bytesComplete / file.size );
	$('import-progress-inner-'+file.id).setStyle( {width: percentComplete+'%'} );
}

function themeImportUploadError(file, errorCode, message)
{
	$('theme-import-status-long').update(/*tl(*/'Error: upload failed'/*)tl*/);
}

function themeImportUploadSuccess( file, data )
{
	$('theme-import-status-short').update('Upload complete');
	if( data.isJSON() )
	{
		var response = data.evalJSON();
		if( response.status === 'Success' )
		{
			currentEditingThemeName = response.name;
			currentEditingThemeID = response.id;
			addNewDesignPicture();
			exitCustomThemeEditor();
		}
		$('theme-import-status-long').update(response.status+': '+response.message);
	}
}

function themeImportUploadComplete(file)
{
}
