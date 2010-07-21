
    function showFeedback() {

	new Effect.Move('feedback', { y: 100, mode: 'absolute'});

    }

    function hideFeedback() {

	new Effect.Move('feedback', { y: -700, mode: 'absolute'});

    }

    function giveFeedback() {

        new Ajax.Request(ajax, {parameters:'pos=givefeedback&feedback='+$('feedbackText').value+'&referral='+$('referralText').value+'&cookie='+document.cookie, onFailure:errFunc});
	//Clear form
	$('feedbackText').value = "";
	$('referralText').value = "";
    	feedbackInit();
    }

function feedbackInit() {

	new Effect.Move('feedback', { y: -700, mode: 'absolute'});
}
