/**
 * @author levinhuang
 * @desc 常用jquery插件和静态方法
 */
; (function($) {

	//Attach this new method to jQuery
	$.fn.extend({

		//This is where you write your plugin's name
		singleSelect: function(opts) {
			///<summary>single select plugin.说明:点击选中dom元素,同时赋予选中的元素指定的css class,其他的元素移除改css class</summary>
			///<param name="opts">{CssClass:'cur',before:null,after:null}</param>
			var defaultOpts = { CssClass: 'cur', before: null, after: null };
			opts = $.extend(defaultOpts, opts);
			//Iterate over the current set of matched elements
			var items = this;

			return this.each(function() {

				$(this).click(function() {
					//before callback（precondition method）
					if ($.isFunction(opts.before)) {
						if (!opts.before(this)) return false;
					};
					items.filter(":visible").removeClass(opts.CssClass);
					$(this).addClass(opts.CssClass);
					if ($.isFunction(opts.after)) {
						opts.after(this);
					};
				});

			});
		}, //endof singleSelect plugin
		anySelect: function(opts) {
			///<summary>multiple select plugin</summary>
			///<param name="opts">{ CssClass: 'cur', max: 0, after: null, before: null,fail:null}</param>
			var defaultOpts = { CssClass: 'cur', max: 0, after: null, before: null, fail: null };
			opts = $.extend(defaultOpts, opts || {});
			opts.max = opts.max || 0;
			var items = this;
			return this.each(function() {
				var $this = $(this);
				//validate assert

				$this.click(function() {
					if ($.isFunction(opts.before)) {
						if (!opts.before(this)) { opts.fail(this); return false; };
					};
					//toggle effect
					if ($this.hasClass(opts.CssClass)) {
						$this.removeClass(opts.CssClass);
						return true;
					};
					//validate max
					if (opts.max > 0 && items.filter("." + opts.CssClass).size() >= opts.max) {
						if ($.isFunction(opts.fail)) { opts.fail(this); };
						return false;
					};
					$this.addClass(opts.CssClass);
					if ($.isFunction(opts.after)) { opts.after(this); };
				}); //endof click
			}); //endof each
		}, //endof anySelect plugin
		preInput: function(opts) {
			///<summary>为文本框显示默认值</summary>
			var needEmpty = function(defaultVal, curVal) {
				if ($.trim(curVal) == "" || $.trim(curVal) == defaultVal) {
					return true;
				};
				return false;
			};
			opts.cssDefault=opts.cssDefault||'ipt_default';
			return this.each(function() {
				var defaultVal = opts.val || "",$i=$(this);
				$i.val(defaultVal)
				.focus(function() {
					$i.removeClass(opts.cssDefault);
					if (needEmpty(defaultVal, this.value)) { this.value=""; };
				})
				.blur(function() {
					var val =this.vlaue;
					if (needEmpty(defaultVal, val)) { $i.val(defaultVal).addClass(opts.cssDefault); };
					if (opts.afterblur) { opts.afterblur($i); };
				});
				if(needEmpty(defaultVal,this.value)){
					$i.addClass(opts.cssDefault);
				};
			});
		}//endof preInput plugin
	}); //endof $.fn.extend

	//Static methods
	$.ajaxJsonPost = function(url, data, success, error) {
		$.ajax({
			url: url,
			data: data || '{}',
			type: "POST",
			contentType: "application/json",
			timeout: 1000000,
			dataType: "json",
			success: success,
			error: error
		});
	}; //endof ajaxJsonPost
	$.preloadImages = function() {
		//<summary>reference:www.mattfarina.com/2007/02/01/preloading_images_with_jquery</summary>
		//<remarks>$.preloadImages('xx.gif','yy.png');</remarks>
		for (var i = 0; i < arguments.length; i++) {
			jQuery("<img>").attr("src", arguments[i]);
		}
	}; //endof preloadImages
	$.navTo = function(opts) {
		///<summary>将当前页面导航至指定路径</summary>
		///<param name="opts">{url:'www.vivasky.com',timeout:3000}</param>
		var timeout = 0;
		if (opts && opts.timeout) timeout = opts.timeout;
		var refresh = function() {
			if (opts && opts.url) {
				window.location.href = opts.url;
			} else {
				//refresh current page
				window.location.reload();
				//window.history.go(0);
				//window.location.href = window.location.href;
			};
		};
		setTimeout(refresh, timeout);
	}; //endof navTo
	$.noScript = function(text) {
		///<summary>判断指定的字符串没有有害的script字符</summary>
		///<return>bool</return>
		var flag = true;
		var scriptWord = "<|>|script|alert|{|}|(|)|#|$|'|\"|:|;|&|*|@|%|^|?";
		var words = scriptWord.split('|');
		for (var i = 0; i < words.length; i++) {
			if (text.indexOf(words[i]) != -1) {
				flag = false;
				break;
			};
		};
		return flag;
	}; //endof $.noScript
	$.clearSql = function(text) {
		///<summary>清除字符串中的sql关键字</summary>
		var repWord = "|and|exec|insert|select|delete|update|count|*|chr|mid|master|truncate|char|declare|set|;|from";
		var repWords = repWord.split('|');
		var appIndex;
		for (var i = 0; i < repWords.length; i++) {
			appIndex = text.indexOf(repWords[i]);
			if (appIndex != -1) {
				text = text.replace(repWords[i], "");
			}
		}
		return text;
	};
	$.hasQuote = function(text) {
		var yes = text.indexOf("'") > -1 || text.indexOf('"') > -1;
		return yes;
	}; //endof $.noQuote
	$.hasher = function(hash) {
		///<summary>设置或获取当前url地址的hash值</summary>
		if (hash || hash == "") {
			window.location.hash = hash;
		} else {
			var _hash = window.location.hash;
			return _hash.substr(1);
		};
	};
	/**
	 * dummy div
	 */
	$.dummyDiv=$("<div/>");
	/**
	 * outerHtml
	 * @param {Object} $dom jquery dom object
	 */
	$.outerHtml=function($dom){
		var r=$.dummyDiv.empty().append($dom);
		return r.html();
	};
	//pass jQuery to the function, 
	//So that we will able to use any valid Javascript variable name 
	//to replace "$" SIGN. But, we'll stick to $ (I like dollar sign: ) )		
})(jQuery);
