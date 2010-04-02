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
			return this.each(function() {
				var defaultVal = opts.val || "";
				$(this).val(defaultVal)
				.focus(function() {
					if (needEmpty(defaultVal, this.value)) { $(this).val(""); };
					if (opts.afterfocus) { opts.afterfocus(this); };
				})
				.blur(function() {
					var val = $(this).val();
					if (needEmpty(defaultVal, val)) { $(this).val(defaultVal); };
					if (opts.afterblur) { opts.afterblur(this); };
				});
			});
		}, //endof preInput plugin
		serializeX: function(opts) {
			///<summary>serialize selected elements to a json object.用选定元素的name||id和value||innerHTML构造一个匿名json对象</summary>
			///<param>默认为{useid:false,prefix:""}，即使用id做匿名对象的属性</param>
			///<return>匿名json对象.{} if no selected elements</return>
			var retVal = {};
			var defaultOpts = { useid: false, prefix: "" };
			opts = $.extend(defaultOpts, opts);
			var isRadioOrCheckbox = function(elem) { if (elem.nodeType != 1) { return false; }; return /radio|checkbox/.test(elem.type); };
			this.each(function() {
				var nowVal = this.value || (this.innerHTML || "");
				if (opts.useid) {
					if ((!this.id) || $.string(this.id).blank()) return true;
					if (!$.string(opts.prefix).blank()) {
						if (!$.string(this.id).startsWith(opts.prefix)) {
							return true; //continue
						};
					};

					retVal[this.id] = isRadioOrCheckbox(this) ? [nowVal] : nowVal;
				} else {
					if ((!this.name) || $.string(this.name).blank()) return true;
					if (!$.string(opts.prefix).blank()) {
						if (!$.string(this.name).startsWith(opts.prefix)) {
							return true; //continue
						};
					};
					//we may have multiple objects with same name.like checkbox and radio
					var lastVal = retVal[this.name];
					if (!lastVal) {
						retVal[this.name] = isRadioOrCheckbox(this) ? [nowVal] : nowVal;
					} else {
						if ($.isArray(lastVal)) {
							lastVal.push(nowVal);
							retVal[this.name] = lastVal;
						} else if (typeof lastVal == 'string') {
							var tempArr = []; tempArr.push(lastVal); tempArr.push(nowVal);
							retVal[this.name] = tempArr;
						};
					};
				};
			}); //endof this.each
			return retVal;
		}, //endof serializeX
		thinTable: function(opts) {
			///<summary>Use vivasky.thintable.css to style a existing table.</summary>
			///<param name="opts">options.E.g:{caption:'table1'}</param>
			return this.each(function() {
				$(this).addClass(opts.cssClass || "ThinTable");
				//1,caption
				if (opts.caption) {
					$(this).append("<caption>" + opts.caption + "</caption>");
				};
				//2,title row
				var tempTdStr = null;
				$("tr:first>td", this).each(function(i) {
					if (i == 0) {
						tempTdStr = '<th class="nobg" scope="col">' + this.innerHTML + "</th>";
					} else {
						tempTdStr = '<th scope="col">' + this.innerHTML + "</th>"; ;
					};
					$(this).replaceWith(tempTdStr);
				});
				//3,content rows
				var rows = $("tr:not(:first)");
				//3.1 odd rows
				rows.filter(":odd").each(function() {
					$(">td", this).each(function(x) {
						if (x == 0) {
							tempTdStr = '<th scope="row" class="spec">' + this.innerHTML + "</th>";
						} else {
							tempTdStr = '<td>' + this.innerHTML + '</td>';
						};
						$(this).replaceWith(tempTdStr);
					});
				});
				//3.2 even rows
				rows.filter(":even").each(function() {
					$(">td", this).each(function(x) {
						if (x == 0) {
							tempTdStr = '<th scope="row" class="specalt">' + this.innerHTML + "</th>";
						} else {
							tempTdStr = '<td class="alt">' + this.innerHTML + '</td>';
						};
						$(this).replaceWith(tempTdStr);
					});
				});

			}); //endof return
		} //endof thinTable
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
	$.wcfJsonPost = function(url, data, callback, error) {
		$.ajaxJsonPost(url, data, callback, error);
	}; //endof $.wcfJsonPost
	$.asmxJsonPost = function(url, data, callback, error) {
		$.ajaxJsonPost(url, data, callback, error);
	}; //endof $.asmxJsonPost
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

	//pass jQuery to the function, 
	//So that we will able to use any valid Javascript variable name 
	//to replace "$" SIGN. But, we'll stick to $ (I like dollar sign: ) )		
})(jQuery);