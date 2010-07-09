/**
 * @author levinhuang qianwang
 * @desc	搜狐通用弹出对话框
 * @dependence jqModal.js;
 */
var sohu_frameModal = function() {
	var p = {},pub = {};
	p.onOpenIF = function(hash) {
		var $win = $(hash.w);
		var $if = $('iframe', $win);
		hash.$if=$if;
		var url = p._opts.url;
		if (p._opts.data) {
			if (url.indexOf("?") > 0) {
				url += "&" + $.param(p._opts.data);
			} else {
				url += "?" + $.param(p._opts.data);
			}

		}
		var title = p._opts.title || "";
		hash.refreshAfterClose = p._opts.jqmRefresh || false;

		var newWidth = p._opts.width || "90%";
		var newHeight = p._opts.height || "50%";
		var newLeft = 0, newTop = 0;

		$if.html('').attr('src', url);
		$('#jqmTitleText').text(title);

		// let's run through all possible values: 90%, nothing or a value in pixel
		if (newHeight != 0) {
			if (newHeight.indexOf('%') > -1)
			{
				newHeight = Math.floor(parseInt($(window).height()) * (parseInt(newHeight) / 100));
			}

			if (parseInt($(window).height()) > newHeight) {
				newTop = Math.floor(parseInt($(window).height() - newHeight) / 2);
			} else {
				newTop = 0;
				newHeight = parseInt($(window).height()) - 40;
			}
		} else {
			newHeight = $win.height();
		}

		if (newWidth != 0)
		{
			if (newWidth.indexOf('%') > -1)
			{
				newWidth = Math.floor(parseInt($(window).width() / 100) * parseInt(newWidth));
			}
			newLeft = Math.floor(parseInt($(window).width() / 2) - parseInt(newWidth) / 2);
		} else {
			newWidth = $win.width();
		}


		// do the animation so that the windows stays on center of screen despite resizing
		$win.css({
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
		}, 'fast');
		if (p._onOpen) {
			p._onOpen(hash);
		}
		;
	};
	p.onCloseIF = function(hash) {
		var $win = $(hash.w), iframeDoc;
		if (!p._opts.dm) {
			iframeDoc = window.frames["jqmContent"].document;
		}
		$win.fadeOut('2000', function() {
			hash.o.remove();
			//refresh parent
			if (hash.refreshAfterClose === 'true') {
				window.location.href = document.location.href;
			}
			hash.$if.attr("src","");//clear cache
			if (p._onHide) {
				p._opts.ifDoc = iframeDoc;
				p._onHide(hash, p._opts);
			}

		});
	};
	p.showIframe = function(opts) {
		sohu_frameModal.Jqm = p._jqmHolder.jqm({
			overlay:opts.overlay || 0,
			modal:opts.modal || true,
			trigger:false,
			onHide: p.onCloseIF,
			onShow: p.onOpenIF
		}).jqmShow();
	};
	//public area
	pub.Show = function(opts) {
		if (!opts.jqm) return;
		if (!opts.url) return;
		p._opts = opts;
		p._jqmHolder = $(opts.jqm);
		p._onOpen = opts.onOpen;
		p._onHide = opts.onHide;
		p.showIframe(opts);
	};
	return pub;
}();