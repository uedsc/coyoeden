///<reference path="../jquery/jquery-1.3.2.js"/>
///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
///<reference path="../jquery/jquery.tools.min.js"/>
///<reference path="../jquery/jquery.jgrowl.js"/>
///<reference path="../jquery/jquery.json-1.3.min.js"/>
///<reference path="../jquery/jquery.dateentry.min.js"/>
///<reference path="Local.Common.js"/>
///<reference path="../date.js"/>
///<reference path="Utils.iDropMenu.js"/>
var App = function() {
	//private
	var p = {};
	p.cleanTip = function(condition, obj) {
		var $go = $('#globalsearch-go');
		if (obj.val() == obj.attr('title')) {
			obj.val('').removeClass('grey');
			$go.addClass("typing");
		} else if (obj.val() == "") {
			obj.val(obj.attr('title')).addClass("grey");
			$go.removeClass("typing");
		}
	};
	p.initSearch = function() {
		//搜索
		$(".search_menu .up").click(function() {
			$(this).hide().prev().show();
			$(this).next().hide();
			return false;
		});

		$(".search_menu .down").click(function() {
			$(this).hide().next().show().next().show();
			return false;
		});

		p.tbxQuery = $('#globalsearch-query');

		p.tbxQuery.blur(function() { p.cleanTip("blur", p.tbxQuery); }).click(function() { p.cleanTip("click", p.tbxQuery); });

		$("#search_menu_drop").find("a").click(function() {
			$("#globalsearch").attr("action", $(this).attr('href'));
			if (p.tbxQuery.val() == p.tbxQuery.attr('title')) p.tbxQuery.val('');
			$("#globalsearch")[0].submit();
			return false;
		});
	};
	p.initSysDropMenu = function() {
		$("#primary-others a").hover(
			function() {
				$('#sys_drop_inner').show();
			}, function() {
				window.setTimeout(function() { $('#sys_drop_inner:not(.mouseover)').hide(); }, 1000);
			}).click(function() {
				$('#sys_drop_inner').show(); return false;
			});
		$("#sys_drop_inner").hover(function() { $(this).addClass('mouseover'); }, function() { $(this).removeClass('mouseover'); $(this).hide(); });
	};
	p.initShowMoreLinks = function() {
		$('.showmore .more').mouseover(function() {
			$(this).next().show()
		}).next().bind('mouseleave', function() {
			$(this).hide();
		});
	};
	//public
	var pub = {};
	pub.Init = function(opts) {

		if ($.browser.msie) $("input[type='text'], input[type='password'], textarea").focus(function() { $(this).addClass("ie_focus") }).blur(function() { $(this).removeClass("ie_focus") });

		$('.personalDropDown').iDropMenu({ myUid: opts.uid, webRoot: opts.webRoot });

		p.initSearch(); p.initSysDropMenu(); p.initShowMoreLinks();

		$("#web_loading").remove();

	};
	pub.OnSearch = function() {
		p.cleanTip(null, p.tbxQuery);
	};
	return pub;
} ();