  function onHideLightbox(type) {

        if ($$('#weeblyLightboxContent div#adsense_terms')[0] || type == 'adsense') {

          $$('#secondlist li iframe.adsenseIframe').each(function(el) {
            if (el && el.parentNode && el.parentNode.parentNode && el.parentNode.parentNode.parentNode && el.parentNode.parentNode.parentNode.parentNode && el.parentNode.parentNode.parentNode.parentNode.parentNode) {
              if (navigator.appVersion.indexOf("MSIE") == -1) {
                var thisLi = el.parentNode.parentNode.parentNode.parentNode.parentNode;
              } else {
                var thisLi = el.parentNode.parentNode.parentNode.parentNode;
              }
              thisLi.parentNode.removeChild(thisLi);
              new Ajax.Request(ajax, {parameters:'pos=deletepageelement&pei='+thisLi.id.replace('inside_', '')+'&cookie='+document.cookie, onSuccess:handlerFuncDeletePageElement, onFailure:errFunc, onException: exceptionFunc, asynchronous: false});
            }
          });

          updateList();

        }

  }

  function submitAdsense() {

        if ($('adsense_spinner').style.display == "inline") return;

        $('adsenseErrorDiv').style.display = 'none';
        $('adsenseAcceptTOS').style.display = 'block';
        $('adsenseAcceptTOS').innerHTML = '';
        $('adsense_submitbtn').style.opacity = 0.5;
        $('adsense_submitbtn').style.filter = 'alpha(opacity=50)';
        $('adsense_spinner').style.display = 'inline';
        new Ajax.Request(ajax, {parameters: 'pos=createadsenseaccount&'+Form.serialize('adsense_form')+'&cookie='+document.cookie, 'onSuccess': handlerSubmitAdsense, 'onFailure':errFunc, bgRequest: true});
  }

  function handlerSubmitAdsense(t) {

        if (t.responseText.match("SUCCESS")) {

          adsenseID = t.responseText.replace("SUCCESS:", "");
          Weebly.lightbox.show({element: '#adsense_finish', width: 475, height: 260, padding: 20, animate: true});

          $('adsenseErrorDiv').style.display = 'none';
          $('adsenseAcceptTOS').style.display = 'block';
	  $('adsenseAcceptTOS').innerHTML = /*tl(*/"By continuing, I accept Google's AdSense Terms and Conditions and Weebly's Advertising Terms and Conditions."/*)tl*/;
          $('adsense_submitbtn').style.opacity = 1;
          $('adsense_submitbtn').style.filter = 'alpha(opacity=100)';
          $('adsense_spinner').style.display = 'none';

        } else {
          if (!t.responseText.match("ERROR")) { t.responseText = /*tl(*/"ERROR: Temporary error. Please try again."/*)tl*/; }
          $('adsenseErrorDiv').innerHTML = t.responseText;
          $('adsenseAcceptTOS').style.display = 'none';
          $('adsenseErrorDiv').style.display = 'block';
          $('adsense_submitbtn').style.opacity = 1;
          $('adsense_submitbtn').style.filter = 'alpha(opacity=100)';
          $('adsense_spinner').style.display = 'none';
        }

  }

