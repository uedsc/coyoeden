/**
 * @author levinhuang
 */
$.jqm.iframe={
	open:function(hash){
		if(hash.c.beforeOpen){hash.c.beforeOpen(hash);};
		var $trigger = $(hash.t);
        var $modalWindow = $(hash.w);
        var $modalContainer = $('iframe', $modalWindow);
        var myUrl = hash.ifOpt.url;
        var myTitle =hash.ifOpt.title;
		hash.refreshAfterClose=hash.ifOpt.jqmRefresh;
		
        var newWidth =hash.ifOpt.width; 
		var newHeight =hash.ifOpt.height;
		var newLeft = 0, newTop = 0;
        
		$modalContainer.html('').attr('src', myUrl);
        $('#jqmTitleText').text(myTitle);
        
		myUrl = (myUrl.lastIndexOf("#") > -1) ? myUrl.slice(0, myUrl.lastIndexOf("#")) : myUrl;
        
		var queryString = (myUrl.indexOf("?") > -1) ? myUrl.substr(myUrl.indexOf("?") + 1) : null;

		if(!queryString){$modalWindow.jqmShow();return;};	
		
        // let's run through all possible values: 90%, nothing or a value in pixel
        if (newHeight != 0)
        {
            if (newHeight.indexOf('%') > -1)
            {

                newHeight = Math.floor(parseInt($(window).height()) * (parseInt(newHeight) / 100));

            }
            newTop = Math.floor(parseInt($(window).height() - newHeight) / 2);
        }
        else
        {
            newHeight = $modalWindow.height();
        }
        if (newWidth != 0)
        {
            if (newWidth.indexOf('%') > -1)
            {
                newWidth = Math.floor(parseInt($(window).width() / 100) * parseInt(newWidth));
            }
            newLeft = Math.floor(parseInt($(window).width() / 2) - parseInt(newWidth) / 2);

        }
        else
        {
            newWidth = $modalWindow.width();
        }

        // do the animation so that the windows stays on center of screen despite resizing
        $modalWindow.css({
            width: newWidth,
            height: newHeight,
            opacity: 0
        }).jqmShow().animate({
            width: newWidth,
            height: newHeight,
            top: newTop,
            left: newLeft,
            marginLeft: 0,
            opacity: 1
        }, 'slow');

	},
	close:function(hash){
		var $modalWindow = $(hash.w);
		var iframeDoc=window.frames["jqmContent"].document;
        //$('#jqmContent').attr('src', 'blank.html');
        $modalWindow.fadeOut('2000', function()
        {
            hash.o.remove();
            //refresh parent

            if (hash.refreshAfterClose === 'true')
            {

                window.location.href = document.location.href;
            };
			if(hash.c.txtHiddenID){
				var val=iframeDoc.getElementById(hash.c.txtHiddenID).value;
				$("#"+hash.c.txtHiddenID_).val(val);
			};
			if(hash.c.onHideExt){
				hash.c.onHideExt(hash);
			};
        });

	}
};
