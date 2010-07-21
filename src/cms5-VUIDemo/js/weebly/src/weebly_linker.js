    // Linker system allows for a flexible, extensible linker that enables users
    // to create links to various types of objects throughout the application.
    // -------
    Weebly.Linker = {
      Version        : '0.1',
      Author         : 'David Rusenko',
      Company        : 'Weebly, Inc.',
      optDefault : 0,
      callbackFunc : '',
      currentURL : '',
      currentContentFieldID: '',
      passValidation: 1,
      activeElement: '',
      modules: Array('linkerWebsite','linkerWeebly','linkerFile','linkerImage','linkerEmail'),

      show: function(callback, position, displayedLinks, activeLink, contentFieldID, showRemoveLink) {

	Weebly.Linker.callbackFunc = callback;
	Weebly.Linker.currentContentFieldID = contentFieldID;
	
	// Hide 'Remove Link' link
	if (showRemoveLink) {
		$('linkerRemoveLink').show();
	}else{
		$('linkerRemoveLink').hide();
	}

	// Hide color picker
        //Element.setStyle('colorpicker', {'display':'none'} );


	// Hide all modules
	for (var x=0; x < Weebly.Linker.modules.length; x++) {
	  Element.hide(Weebly.Linker.modules[x]);
	}

	// Show passed modules
	for (var x=0; x < displayedLinks.length; x++) {
	  Element.show(displayedLinks[x]);
	}

        var width= ((document.body.clientWidth - 550) / 2 )-5;

	Weebly.Linker.active(activeLink);

	//Element.setStyle('createLink', {'top':position.top+'px', 'left':position.left+'px'} );

	Element.setStyle('createLink', {'top':position.top+'px', 'left':width+'px'} );

	Element.show('createLink');

	// If Linker will be partially hidden, move up enough that it's completely visible
	// Dan, you're such a pain in my ass, but I'm sure our users thank you...
	var myScrollTop = getScrollTop();
	if (position.top + $('createLink').clientHeight > window.innerHeight + myScrollTop) {
	  var newPosition = window.innerHeight + myScrollTop - $('createLink').clientHeight - 5;
	  Element.setStyle('createLink', {'top': newPosition +'px'} );
	}

      },

      save: function(doRemove) {

	if (Weebly.Linker.gatherDataFunc[Weebly.Linker.activeElement]) {
	  Weebly.Linker.gatherDataFunc[Weebly.Linker.activeElement]();
	}

	Weebly.Linker.passValidation = 1;
	if (!doRemove && Weebly.Linker.validateFunc[Weebly.Linker.activeElement]) {
	  var returnVar = Weebly.Linker.validateFunc[Weebly.Linker.activeElement]($('createURL').value);
	  if (returnVar != 1) {
	    $('linkerError').innerHTML = returnVar;
	    Weebly.Linker.passValidation = 0;
	  } else {
	    $('linkerError').innerHTML = '';
	  }
	}

	if (Weebly.Linker.passValidation) {

	  Weebly.Linker.currentURL = doRemove ? '' : $('createURL').value;
	  eval(Weebly.Linker.callbackFunc+'("'+Weebly.Linker.currentURL+'");');
      if( Weebly.Linker.activeElement === 'linkerWebsite' && $('webpageNewWindow').checked && $(currentBox+'Edit') )
      {
        var anchors = document.getElementById(currentBox+'Edit').contentWindow.document.getElementsByTagName('a');
        for( var i=0; i<anchors.length; i++ )
        {
          if( anchors[i].href.match('weeblylink_new_window') )
          {
            anchors[i].href = anchors[i].href.replace('weeblylink_new_window','');
            anchors[i].target = '_blank';
          }
        }
      }

	}

      },	

      active: function(activeElement) {

	// Set active linker element
	Weebly.Linker.activeElement = activeElement;

        // Unselect all modules
        for (var x=0; x < Weebly.Linker.modules.length; x++) {
          $(Weebly.Linker.modules[x]).className = "linkerMenuItem";
        }

        // Select passed modules
        $(activeElement).className = "linkerMenuItem-selected";

	// Reset error text
	$('linkerError').innerHTML = '';

	$("linkerContents").innerHTML = Weebly.Linker.elementContents[activeElement];
	new Ajax.Request(ajax, {parameters:'pos=linker&keys='+activeElement+'&cookie='+document.cookie, onSuccess:Weebly.Linker.paste, onFailure:errFunc});

      },

    // Handle AJAX response by pasting in HTML code
      paste: function(t) {

	$("linkerContents").innerHTML += t.responseText;

      },

    // Close linker
      close: function() {

	Element.hide('createLink');

      },

      saveAndClose: function(doRemove) {

	Weebly.Linker.save(doRemove);

	if (Weebly.Linker.passValidation) {
	  Weebly.Linker.close();
	}

      },
      
      removeLink: function() {
      	Weebly.Linker.saveAndClose(true);
      },

      elementContents: {

	'linkerWebsite': /*tl(*/"Enter the webpage you would like to link below. <br/><i>(ie, www.meebo.com/)</i>"/*)tl*/+"<br/><br/><select id='webpageStart' style='width: 72px; margin-right: 5px;'><option value='http://'>http://</option><option value='https://'>https://</option><option value='ftp://'>ftp://</option><option value='ftps://'>ftps://</option><option value='aim:goim?screenname='>aim:goim?screenname=</option></select><input type='text' id='webpageURL' value='www.' style='width: 250px; color: #cccccc;' onclick='if (this.value == \"www.\") { this.value = \"\"; }' onkeyup='this.value = this.value.replace(/^https?:\\/\\//, \"\"); this.value = this.value.replace(/^ftps?:\\/\\//, \"\"); this.style.color = \"#303030\";' /><br /><input type='checkbox' id='webpageNewWindow' style='margin-left:77px; margin-top:7px;' /> "+/*tl(*/"Open link in new window"/*)tl*/+"<br/><br/>",

	'linkerWeebly': /*tl(*/"Select the page you would like to link to below.<br/><br/>"/*)tl*/,

	'linkerImage': /*tl(*/"Your picture will be linked to the full size version of this image.<br/><br/>"/*)tl*/,
	'linkerEmail': /*tl(*/"Enter the email address you would like to link below. <br/><i>(ie, feedback@weebly.com)</i><br/><br/>Email"/*)tl*/+": <input type='text' id='emailLink' value='' style='width: 250px;' /><br/><br/>",

	'linkerFile':  /*tl(*/"Select the file you would like to link to below.<br/><br/>"/*)tl*/

      },
      gatherDataFunc: {

	'linkerWeebly': function() {
	  $('createURL').value = $('weeblyPageID').value;
	},

	'linkerWebsite': function() {
	  $('webpageURL').value = $('webpageURL').value.replace(/^[^\?]*:\/\//, '');
	  $('createURL').value = $('webpageStart').value+$('webpageURL').value;
      if($('webpageNewWindow').checked)
      {
        $('createURL').value += 'weeblylink_new_window';
      }
	},
	
	'linkerEmail': function() {
	  $('createURL').value = 'mailto:'+$('emailLink').value;
	},

	'linkerFile': function() {
	  $('createURL').value = $('weeblyFileID').value;
	},
	'linkerImage': function() {
	  $('createURL').value = "http://weebly-image-full/"+Weebly.Elements.currentElement.id;
	}

      },
      validateFunc: {
	'linkerWebsite': function(validateMe) {

	  if (validateMe == 'http://asdf') {
	    return "Invalid URL.";
	  } else {
	    return 1;
	  }

	},

	'linkerEmail': function(validateMe) {

	  if (validateMe.match(/^mailto:[a-zA-Z0-9\-\_\.\+\~\#\*]+@[a-zA-Z0-9\-]+\.[a-zA-Z]+[a-z0-9A-Z\.\-\?\=\&]*$/)) {
	    return 1;
	  } else {
	    return /*tl(*/"Invalid email address. Please enter a correct email address."/*)tl*/;
	  }

	},

	'linkerFile': function(validateMe) {

	  new Ajax.Request(ajax, {parameters:'pos=linker&keys=linkresource:'+Weebly.Linker.currentContentFieldID+':'+validateMe+'&cookie='+document.cookie, onSuccess:function(t) { }, onFailure:errFunc});

	  return 1;

	}

      }


    };

    //------------
    /// End of Linker module
    ////
