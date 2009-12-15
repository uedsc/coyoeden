; (function($) {
	//dependency js:jquery.sortable;jquery.draggable
	// Private functions.
	var p = {};
	p.onRemove = function() {
		if (p.opts.onRemove) {
			var id = $(this).parents(p.opts.css_w).attr("id");
			alert("Your are removing " + id);
			p.opts.onRemove(this);
		};
		return false;
	};
	p.onEdit = function() {
		var id = $(this).parents(p.opts.css_w).attr("id");
		alert("You are editting " + id);
		return false;
	};
	p.onCollapseOn = function() {
		$(this).css({ backgroundPosition: '-38px 0' })
			.parents(p.opts.css_w)
			.find(p.opts.css_c).hide();
		return false;
	};
	p.onCollapseOff = function() {
		$(this).css({ backgroundPosition: '' })
			.parents(p.opts.css_w)
			.find(p.opts.css_c).show();
		return false;
	};
	p.makeSortable = function() {
		p.items_m.find(p.opts.css_h).mousedown(function(e) {
			p.items_m.css({ width: '' });
			$(this).parent().css({
				width: $(this).parent().width() + 'px'
			});
		}).mouseup(function() {
			if (!$(this).parent().hasClass('dragging')) {
				$(this).parent().css({ width: '' });
			} else {
				$(p.zones).sortable('disable');
			}
		});

		p.zones.sortable({
			items: p.items_m,
			connectWith: p.zones,
			handle: p.opts.css_h,
			placeholder: 'widget-placeholder',
			forcePlaceholderSize: true,
			revert: false,
			delay: 100,
			opacity: 0.9,
			containment: p.opts.wrapper || 'document',
			start: function(e, ui) {
				$(ui.helper).addClass('dragging');
				p.iZone0 = ui.item.parents(p.opts.zone).attr("id");
				p.iItem = ui.item.attr("id");
			},
			stop: function(e, ui) {
				$(ui.item).css({ width: '' }).removeClass('dragging');
				p.zones.sortable('enable');
			},
			update: function(e, ui) {
				var newIds = $.map(p.zones.find(p.opts.css_m), function(obj,i) { return obj.id; });
				var newI = $.inArray(p.iItem,newIds);
				p.iZone1 = ui.item.parents(p.opts.zone).attr("id");
				var data = {Items:newIds, /*sortdata*/sdata: { Id: p.iItem, NewIndex: newI, Zone0: p.iZone0, Zone1: p.iZone1} };
				if (p.opts.onSort) {
					p.opts.onSort(data);
				};
			}
		});
	};
	p.onHeadOver = function() {
		$(p.opts.css_a, this).show();
	};
	p.onHeadOut = function() { $(p.opts.css_a, this).hide(); };
	//main plugin body
	$.fn.xwidget = function(options) {
		// Set the options.
		options = $.extend({}, $.fn.xwidget.defaults, options);
		p.opts = options;
		// Go through the matched elements and return the jQuery object.
		p.items = this;
		p.items_m = p.items.filter(p.opts.css_m);
		p.zones = $(p.opts.zone);
		//make sortable
		p.makeSortable();
		//action register
		return this.each(function() {
			//remove action
			$(options.css_remove, this).click(p.onRemove);
			//edit action
			$(options.css_edit, this).click(p.onEdit);
			//collapse action
			$(options.css_h, this).hover(p.onHeadOver, p.onHeadOut);
			$(options.css_collapse, this).toggle(p.onCollapseOn, p.onCollapseOff);
		});
	};
	// Public defaults.	convenient for global overrides.
	$.fn.xwidget.defaults = {
		zone: '.widgetzone',
		css_w: '.widget',
		css_h: '.widget-head',
		css_c: '.widget-content',
		css_remove: '.remove',
		css_edit: '.edit',
		css_collapse: '.collapse',
		css_m: '.widget-m',
		css_a: '.action'
	};
	// Public functions.
	$.fn.xwidget.onWidgetRemoved = function(obj) {
		$(obj).parents(p.opts.css_w).animate({
			opacity: 0
		}, function() {
			$(obj).remove();
		});
	};
})(jQuery);