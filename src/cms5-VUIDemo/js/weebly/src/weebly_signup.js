var signupAction = '';

   function showSignup(myAction) {

	if (myAction == "exit") {
	  $('signupNote').innerHTML = "<font style='color: red; font-size: 18px;'>"+/*tl(*/"Sign-up to save your website."/*)tl*/+"</font><br/>"+/*tl(*/"Otherwise, click Cancel to exit."/*)tl*/;
	} else if (myAction == "publish") {
	  $('signupNote').innerHTML = "<font style='color: red; font-size: 18px;'>"+/*tl(*/"Sign-up to publish your website."/*)tl*/+"</font><br/>"+/*tl(*/"Otherwise, click Cancel to continue working."/*)tl*/;
        } else if (myAction == "zip") {
          $('signupNote').innerHTML = "<font style='color: red; font-size: 18px;'>"+/*tl(*/"Sign-up to export your website."/*)tl*/+"</font><br/>"+/*tl(*/"Otherwise, click Cancel to continue working."/*)tl*/;
	} else {
	  $('signupNote').innerHTML = "<font style='font-size: 22px; font-weight: bold;'>"+/*tl(*/"Sign-up now"/*)tl*/+"</font>";
	}

        new Effect.Move('signup', { y: 65, mode: 'absolute'});
	signupAction = myAction;
        return false;
   }

   function hideSignup() {
        new Effect.Move('signup', { y: -900, mode: 'absolute'});
        if (signupAction == "adsense") {
	  onHideLightbox('adsense');
	}
   }

   function submitSignup() {

        Element.setStyle('signupUser', { border: '1px solid #DDD' });
        Element.setStyle('signupPass', { border: '1px solid #DDD' });
        //Element.setStyle('signupPass2', { border: '1px solid #DDD' });
        Element.setStyle('signupEmail', { border: '1px solid #DDD' });
        Element.setStyle('signupInvitationID', { border: '1px solid #DDD' });

        Element.hide('signupError');

        var proceedForm = 1;
        if( $('signupUser').value.match(/[^a-zA-Z0-9\-\_]/)) {
          Element.setStyle('signupUser', { border: '2px solid red' });
          showTip(/*tl(*/'Your username may only contain numbers, letters, a dash (-) or an underscore (_).'/*)tl*/, 'signupUser');
          proceedForm = 0;
        } else if( $('signupUser').value == '') {
          Element.setStyle('signupUser', { border: '2px solid red' });
          showTip(/*tl(*/'Please enter a username.'/*)tl*/, 'signupUser');
          proceedForm = 0;
        }
        if( $('signupPass').value.match(/.{17}/)) {
          Element.setStyle('signupPass', { border: '2px solid red' });
          showTip(/*tl(*/'Your password is too long. It can be a maximum of 16 characters long.'/*)tl*/, 'signupPass');
          proceedForm = 0;
        } else if( !$('signupPass').value.match(/.{4}/)) {
          Element.setStyle('signupPass', { border: '2px solid red' });
          showTip(/*tl(*/'Your password is not long enough. It must be at least 4 characters long.'/*)tl*/, 'signupPass');
          proceedForm = 0;
        } else if( $('signupPass').value == '') {
          Element.setStyle('signupPass', { border: '2px solid red' });
          showTip(/*tl(*/'Please enter a password.'/*)tl*/, 'signupPass');
          proceedForm = 0;
        }/* else if( $('signupPass').value != $('signupPass2').value) {
          Element.setStyle('signupPass2', { border: '2px solid red' });
          showTip('Your passwords to not match. Please try again.', 'signupPass2');
          proceedForm = 0;
        }*/
	if ($('signupEmail').value == '' || !$('signupEmail').value.match("@")) {
	  Element.setStyle('signupEmail', { border: '2px solid red' });
          showTip(/*tl(*/'Please enter an email.'/*)tl*/, 'signupEmail');
          proceedForm = 0;
	}

        if(proceedForm == 1) {

           new Ajax.Request(ajax, {parameters:'pos=signup&user='+encodeURIComponent($F('signupUser'))+'&pass='+encodeURIComponent($F('signupPass'))+'&email='+encodeURIComponent($F('signupEmail'))+'&cookie='+document.cookie, onSuccess: handlerSubmitSignup, onFailure:errFunc});

        }

   }

   function handlerSubmitSignup(t) {

        if (t.responseText.indexOf('%%SUCCESS%%') > -1) {

		tempUser = 0;

		if (signupAction == "exit") {
		  Pages.go('userHome'); 
		} else if (signupAction == "publish") {
		  Pages.go('exportSite');
		} else if (signupAction == "zip") {
		  $("exportSiteZipFrame").src = "downloadZip.php?"+Math.floor(Math.random()*1001);
		  Pages.go('main');
		} else {
		  Pages.go('main');
		}

		$('weebly-signup-button').innerHTML = '<a class="weebly-top-links" href="#" onClick="Pages.go(\'userHome\'); return false;"><img style="position: relative; top: -1px;" src="images/action_stop.gif" /> <font style="position: relative; top: -5px; color: white;">'+/*tl(*/'Close'/*)tl*/+'</font></a>';

	} else {

          $('signupError').innerHTML = t.responseText;
	  if (t.responseText == '') { $('signupError').innerHTML = /*tl(*/'There was an error creating your account. Please try again.'/*)tl*/; }
          Effect.Appear('signupError');

        }

   }

   function resetPassword() {

        Element.hide('signupError');

        new Ajax.Request('/weebly/publicBackend.php', {parameters:'pos=resetpassword&email='+$F('resetEmail'), onSuccess:handlerResetPassword, onFailure:errFunc});

   }

   function handlerResetPassword(t) {

        if (t.responseText.indexOf('%%SUCCESS%%') > -1) {

	  $('resetSuccess').innerHTML = /*tl(*/"Instructions for reseting your password have been sent. Please check your email. <a href='http://www.weebly.com/'>Click here</a> to return to the main page."/*)tl*/;
	  Effect.Appear('resetSuccess');
        } else {

          $('signupError').innerHTML = t.responseText;
          Effect.Appear('signupError');

        }

   }

   function changePassword()
   {
        Element.hide('signupError');
        new Ajax.Request('/weebly/publicBackend.php', {parameters:'pos=changepassword&new_password='+encodeURIComponent($F('resetPass2'))+'&v='+$F('resetKey'), onSuccess:handlerChangePassword, onFailure:errFunc});
   }

   function handlerChangePassword(t)
   {
        if (t.responseText.indexOf('%%SUCCESS%%') > -1)
        {
          $('resetSuccess').innerHTML = /*tl(*/"Your password has been succesfully changed. <a href='http://www.weebly.com/'>Click here</a> to return to the main page and login."/*)tl*/;
          Effect.Appear('resetSuccess');
        }
        else
        {
          $('signupError').innerHTML = t.responseText;
          if (t.responseText == '') { $('signupError').innerHTML = 'Error'; }
          Effect.Appear('signupError');
        }
   }
