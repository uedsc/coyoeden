;(function($) {
	// Private functions.
	var p = {};
	p.resetSelectedRow = function(tbObj) {
		$("td", tbObj).removeClass("row-select"); return false;
	};
	p.rowOver = function(e) {
		$("td", this).addClass("row-over").eq(0).addClass("column0-over"); return false;
	};
	p.rowOut = function(e) {
		$("td", this).removeClass("row-over").eq(0).removeClass("column0-over"); return false;
	};
	p.rowClick = function(e, tbObj, rowObj) {
		if (!p.opts.mselect) {
			p.resetSelectedRow(tbObj);
		};
		$("td", rowObj).toggleClass("row-select"); return false;
	};

	//main plugin body
	$.fn.xgridview = function(options, newSkin) {
		// Set the options.
		options = $.extend({}, $.fn.xgridview.defaults, options);
		p.opts = options;
		// Go through the matched elements and return the jQuery object.
		return this.each(function() {
			var tbObj = this;
			if (newSkin) {
				$.fn.xgridview.applySkin(newSkin, tbObj);
			} else {
				var rows = $("tr.data-row,tr.alt-data-row", tbObj);
				rows.hover(p.rowOver, p.rowOut)
					.click(function(e) { p.rowClick(e, tbObj, this); });
			};

		});
	};
	// Public defaults.	convenient for global overrides.
	$.fn.xgridview.defaults = {
		skin: 'afternoon',
		skinPath: 'assets/css/',
		mselect: false, //multiple selection
		webRoot: "",
		skins: ['summer', 'afternoon', 'busstop', 'classic', 'night']
	};
	// Public functions.
	$.fn.xgridview.applySkin = function(skinName, tbObj) {
		if ($.inArray(skinName, p.opts.skins) == -1) {
			skinName = $.fn.xgridview.randomSkin();
		};
		var path = p.opts.webRoot + p.opts.skinPath + "gv-" + skinName + ".css";
		var cssLinks = $("head>link[type='text/css']").filter("[href='" + path + "']");
		if (cssLinks.length == 0) {
			$('<link rel="stylesheet" type="text/css" href="' + path + '"></link>').appendTo($("head"));
		};
		$(tbObj).removeClass(skinName).addClass(skinName);
	};
	$.fn.xgridview.randomSkin = function() {
		var skins = p.opts.skins;
		var skin = skins[Math.floor(Math.random() * skins.length)];
		return skin;
	};
})(jQuery);
