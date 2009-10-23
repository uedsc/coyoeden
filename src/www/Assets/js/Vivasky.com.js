///<reference path="jquery/jquery-1.3.2.js"/>
///<reference path="jquery/jquery.string.1.0.js"/>
///<reference path="jquery/jquery.event.drag-1.5.js"/>
var __undefined;
var Vivasky = {};
Vivasky.undefined = __undefined;
/*=============================1,jquery plugin of Vivasky=================================*/
//You need an anonymous function to wrap around your function to avoid conflict
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

/*==============================2,other common methods================================*/
$.writeExcel = function(content) {
	///<summary>write sth to excel.Works in IE only</summary>
	if (!$.browser.msie) {
		alert("$.writeExcel only works in IE...");
		return false;
	};
	var isOk = false;
	var d = new Date();
	var fileName = d.getFullYear() + "" + (d.getMonth() + 1) + d.getDate() + d.getHours() + d.getMinutes() + ".xls";
	var xlsWindow = window.open("", "_blank", "width=2,height=2,scrollbars=no,toolbar=no,status=no,menubar=no,location=no,left=780,top=1000");
	xlsWindow.document.write(content);
	xlsWindow.document.close();
	isOk = xlsWindow.document.execCommand('SaveAs', true, fileName); // '%homeDrive%\\Data.xls'
	xlsWindow.close();
	//据目前测试，上面方法不适用于windows2003
	//试用ActiveXObject控件
	var xlsApp;
	var xlSheet;
	if (!isOk) {
		try {
			xlsApp = new ActiveXObject("Excel.Application");
			xlSheet = new ActiveXObject("Excel.Sheet");
		} catch (e) {
			alert("您必须安装Excel软件，同时浏览器须允许使用ActiveX 控件。 请点击【帮助】了解浏览器设置方法！");
			return;
		}
		//复制内容到clipboard
		try {
			var txtInput = document.createElement("input");
			txtInput.type = "hidden";
			txtInput.value = content;
			document.appendChild(txtInput);
			var txtRange = txtInput.createTextRange();
			txtRange.select();
			//复制到clipboard
			txtRange.execCommand("Copy");
			//粘帖到excel中
			xlSheet.ActiveSheet.Paste();
		} catch (e) {
			alert(e.description);
			return;
		};
		//移除临时元素
		document.removeChild(txtInput);
		txtInput = null;
		txtRange = null;

		//另存excel文件
		if (window.confirm("点击确定保存到D:\盘，点击取消保存到C:\盘！")) {
			xlSheet.Application.Visible = true;
			xlSheet.SaveAs("D:\\" + fileName);
		} else {
			xlSheet.Application.Visible = true;
			xlSheet.SaveAs("C:\\" + fileName);
		}
		xlSheet.Application.Quit();
	}; //endof if(!isOk)
};
$.cnZodiac = function(yyyy) {
///<summary>Chinese Zodiac</summary>
///<param name="yyyy">year</param>
//by Go_Rush(阿舜) from http://ashun.cnblogs.com/    
	var arr = ['猴', '鸡', '狗', '猪', '鼠', '牛', '虎', '兔', '龙', '蛇', '马', '羊'];
	return /^\d{4}$/.test(yyyy) ? arr[yyyy % 12] : null
};
$.cnConstellation = function(mm, dd) {
	///<summary>Chinese constellation</summary>
	///<param name="mm">month</param>
	///<param name="dd">day</param>
	//by Go_Rush(阿舜) from http://ashun.cnblogs.com/        
	var d = new Date(1999, month - 1, day, 0, 0, 0);
	var arr = [];
	arr.push(["魔羯座", new Date(1999, 0, 1, 0, 0, 0)]);
	arr.push(["水瓶座", new Date(1999, 0, 20, 0, 0, 0)]);
	arr.push(["双鱼座", new Date(1999, 1, 19, 0, 0, 0)]);
	arr.push(["牡羊座", new Date(1999, 2, 21, 0, 0, 0)]);
	arr.push(["金牛座", new Date(1999, 3, 21, 0, 0, 0)]);
	arr.push(["双子座", new Date(1999, 4, 21, 0, 0, 0)]);
	arr.push(["巨蟹座", new Date(1999, 5, 22, 0, 0, 0)]);
	arr.push(["狮子座", new Date(1999, 6, 23, 0, 0, 0)]);
	arr.push(["处女座", new Date(1999, 7, 23, 0, 0, 0)]);
	arr.push(["天秤座", new Date(1999, 8, 23, 0, 0, 0)]);
	arr.push(["天蝎座", new Date(1999, 9, 23, 0, 0, 0)]);
	arr.push(["射手座", new Date(1999, 10, 22, 0, 0, 0)]);
	arr.push(["魔羯座", new Date(1999, 11, 22, 0, 0, 0)]);
	for (var i = arr.length - 1; i >= 0; i--) {
		if (d >= arr[i][1]) return arr[i][0];
	};

};

$.parseTpl = function(str, data) {
	///<summary>
	///simple js template parser
	///E.G,IF:str="<a href=/u/%uid%>%username%</a>"
	///data={uid:1,username:'xiami'}
	///Then:str = "<a href=/u/1>xiami</a>"
	///</summary>
	var result;
	var patt = new RegExp("%([a-zA-z0-9]+)%");
	while ((result = patt.exec(str)) != null) {
		var v = data[result[1]] || '';
		str = str.replace(new RegExp(result[0], "g"), v);
	};
	return str;
};
String.prototype.parseTpl = function(data) { return $.parseTpl(this, data); };

$.deserializeX = function(data, opts) {
	///<summary>
	///Fill elements' value or innerHTML by a json data,the properties of which will be used to map the elements to be filled.
	///Opposite operation of $(xx).serializeX
	///</summary>
	///<param name="data">json data like {txt1:'xxx',cbx1:['zzz','yyy']}</param>
	///<param name="opts">option.Default OP is {filter:'class',prefix:'',context:'body'}</param>
	var defaultOpt = { filter: 'class', prefix: '', context: 'body' };
	opts = $.extend(defaultOpt, opts);
	var undefined;
	data = data || {};
	$.each(data, function(k, v) {
		var s = null;
		var s0 = (opts.prefix) ? (opts.prefix + k) : k;
		switch (opts.filter) {
			case 'class':
				s = "." + s0;
				break;
			case 'id':
				s = "#" + s0;
				break;
			case 'name':
				s = "[name='" + s0 + "']";
				break;
		}; //endof swith
		if (s) {
			var obj = $(s, opts.context);
			if (obj.size() > 0&&v!=null) { if (!(obj[0].value === undefined)) { obj.val(v); } else { obj.html(v); }; };
		};
	}); //endof $.each
};   //endof $.deserializeX
$.isEnter = function(event) {
	///<summary>whether was ENTER pressed </summary>
	return event.which == 13;
}; //endof $.isEnter
/*===============================3,extension for jqModal===================================*/
$.jqMExt = Vivasky.jqMExt = {
	initUIData: function(uidata) {
		uidata = uidata || { mode: '-1' };
		var tpData = { title: '', content: '', btn_ok: '确 认', btn_close: '取 消', onHide: null };
		tpData = $.extend(tpData, uidata);
		tpData.content = Vivasky.jqMExt.ajaxText;
		return tpData;
	}, //endof initUIData
	jqMCssClass: '.dialog_jqm',
	ajaxText: '<p class="loading">正为您在处理数据, 请稍候...</p>',
	alertHtml: '<div class="alert %Flag%"><strong>%Title%</strong><p>%Body%</p></div>',
	prepareUI: function(jqmApi) {
		///<summary>update the layout of the jqModal according to the layout mode</summary>
		///<param name="jqmApi">jqm api;jqmApi.mode:'-1':only content;'0':title+content+ok;'1':title+content+close;'2':all</param>
		var w = jqmApi.w;
		var mode = '-1';
		if (jqmApi.uiData) {
			if (jqmApi.uiData.okClick) $(".dialog_btn_ok", w).unbind("click").click(function(evt) { jqmApi.uiData.okClick(evt, jqmApi); });
			$.deserializeX(jqmApi.uiData, { prefix: 'dialog_', context: w });
			mode = jqmApi.uiData.mode || '-1';
		};
		switch (mode) {
			case '-1':
				$(".dialog_title,.dialog_acts", w).hide();
				break;
			case '0':
				$(".dialog_title,.dialog_acts", w).show().children(".dialog_btn_close").hide();
				break;
			case '1':
				$(".dialog_title,.dialog_acts", w).show().children(".dialog_btn_ok").hide();
				break;
			case '2':
				$(".dialog_title,.dialog_acts", w).show();
				break;
		}; //endof switch
	}, //endof prepareUI
	ieBGFix: function() {
		if ($.browser.msie) {
			$('.dialog_sharp').height($('.dialog_main').height());
		};
	},
	onShow: function(c) {
		Vivasky.jqMExt.prepareUI(c);
		c.w.show();
		Vivasky.jqMExt.ieBGFix();
		$(".jqmClose", c.w[0]).live("click", function() { c.w.jqmHide(); return false; }); //our close trigger may be dynamically generated.
	},
	onLoad: function(c) {
		Vivasky.jqMExt.ieBGFix();
	},
	jqm: function(jqmSelector, uiData, modal, toTop) {
		///<summary>simply show a dom using jqModal.If wanna use ajax,pls use jqmAjax or jqmAjaxPost</summary>
		///<param name="jqmSelector">jqm which?</param>
		///<param name="uiData">Default is:{title:'',content:'',flag:'alert_ok',btn_ok:'确 认',btn_close:'取 消',mode:'-1'}</param>
		var tpData = { title: '', content: '', btn_ok: '确 认', btn_close: '取 消', onHide: null, mode: '-1'};
		uiData = $.extend(tpData, uiData);
		if (uiData.flag) {
			uiData.mode = "-1"; //no title,no buttons
			uiData.content = Vivasky.jqMExt.alertHtml.parseTpl({ Title: uiData.title, Flag: uiData.flag, Body: uiData.content });
		};

		$(jqmSelector).jqm({
			ajax: null, modal: modal || true, toTop: toTop || true,
			onShow: function(_api) { _api.uiData = uiData; Vivasky.jqMExt.onShow(_api); },
			onHide: uiData.onHide
		}).drag(
		 	function(event) { return $(event.target).is('.jqDrag'); },
			function(event) { var pos = { top: event.offsetY, left: event.offsetX }; $(this).css(pos); }
		).jqmShow();
	}, //endof jqm
	jqmAjax: function(jqmSelector, url, uiData, responseTarget) {
		///<summary>ajax get request via jqModal's internal ajax feature</summary>
		///<param name="jqmSelector">modal which?</param>
		///<param name="url">ajax url</param>
		///<param name="responseTarget">response target element.default is div.dialog_main</param>
		var tpData = $.jqMExt.initUIData(uiData);
		var rawMode = tpData.mode; tpData.mode = '-1';

		var jqmApi = null;
		$(jqmSelector).jqm({
			ajax: url,
			modal: true,
			toTop: true,
			target: responseTarget || 'div.dialog_content',
			ajaxText: Vivasky.jqMExt.ajaxText,
			onHide: uiData.onHide,
			onShow: function(_api) { _api.uiData = tpData; jqmApi = _api; Vivasky.jqMExt.onShow(_api); },
			onLoad: function(_api) { Vivasky.jqMExt.prepareUI({ uiData: { mode: rawMode} }); if (uiData.onLoad) { uiData.onLoad(_api); }; Vivasky.jqMExt.onLoad(_api); }
		}).drag(
			function(event) { return $(event.target).is('.jqDrag'); },
			function(event) { var pos = { top: event.offsetY, left: event.offsetX }; $(this).css(pos); }
		).jqmShow();
	}, //endof jqmAjax
	jqmAjaxPost: function(jqmSelector, url, uiData, data, callback) {
		///<summary>ajax post request.Prepare the UI using jqModal when the request is finished.</summary>
		///<param name="jqmSelector">modal which?</param>
		///<param name="url">ajax url</param>
		///<param name="uiData">ui data for jqModal.{title:'',content:'',btn_ok:'',btn_close:'',okClick:null,mode:'-1'}</param>
		///<param name="data">data to be posted to server.Should be a json string like "{Name:'xxx',Age:11}"</param>
		///<param name="callback">ajax request callback</param>
		var tpData = $.jqMExt.initUIData(uiData);
		var rawMode = tpData.mode; tpData.mode = '-1';
		var jqmApi = {};
		$(jqmSelector).jqm({
			ajax: null,
			onShow: function(_api) { _api.uiData = tpData; jqmApi = _api; Vivasky.jqMExt.onShow(_api); }, //we get the jqModal api here,
			modal: true,
			toTop: true,
			onHide: tpData.onHide
		}).drag(
		 	function(event) { return $(event.target).is('.jqDrag'); },
			function(event) { var pos = { top: event.offsetY, left: event.offsetX }; $(this).css(pos); }
		).jqmShow();

		var callback1 = function(msg) {//msg is server response data
			if (msg.d) msg = msg.d; //keep compatible to asp.net asmx/svc
			if (msg.IsOk) {//data contract:{IsOk:true,Body:'asdfsd',others:'others data'}
				tpData.mode = msg.IsAlert?"-1":rawMode;
				tpData.content = msg.IsAlert ? Vivasky.jqMExt.alertHtml.parseTpl(msg) : msg.Body;
				if (msg.Title && msg.Title != "") { tpData.title = msg.Title; };
			} else { //endof msg.IsOk. if !msg.IsOk,we should process manually in the onLoad callback
				tpData.content = Vivasky.jqMExt.alertHtml.parseTpl(msg);
			};
			jqmApi.uiData = tpData;
			Vivasky.jqMExt.prepareUI(jqmApi);
			//ie bg fix
			Vivasky.jqMExt.onLoad();
			if (callback && $.isFunction(callback)) { callback(msg, jqmApi); };
		}; //endof jqmLocalCallback

		$.ajaxJsonPost(url, data, callback1, function() {
			tpData.content = Vivasky.jqMExt.alertHtml.parseTpl({ Flag: 'alert_error', Title: '服务器通讯发生错误,请稍候重试' });
			jqmApi.uiData = tpData;
			Vivasky.jqMExt.prepareUI(jqmApi);
			Vivasky.jqMExt.onLoad();
		});
	} //endof jqmAjaxPost
};
/*=============================flexigrid ext===========================*/
$.fGrid = function(validateCallback, opts, beforeCallback, afterCallback) {
	var defaultOpts = {
		urlType: "asmx",
		dataType: 'json',
		contentType: "application/json;charset=utf-8",
		sortorder: "asc",
		usepager: true,
		useRp: true,
		rp: 40,
		showTableToggleBtn: false,
		height: 220,
		preProcess: function(data) {
			return data.d;
		},
		criterias: null,
		autoload: false,
		onSubmit: function(p) { if (!validateCallback) { return true; }; var isOk = validateCallback(false); p.criterias = isOk; return isOk != false; }
	};
	opts = $.extend(defaultOpts, opts || {});
	if (beforeCallback) {
		beforeCallback(opts);
	};
	var grid = $("#fg_" + opts.gridName).flexigrid(opts);
	//fix png of flexigrid
	$("div.pButton,div.fButton span", "div.flexigrid").ifixpng();
	if (afterCallback) {
		afterCallback(grid);
	};
};    //endof initFlexigrid

/*=============================validator ext===========================*/
if (jQuery.validator) {
	jQuery.validator.addMethod('notEqualTo', function(value, element, param) { return value != param; }, 'Must not be equal to {0}.');
	jQuery.validator.addMethod('greaterThan', function(value, element, param) { return /^[0-9]+$/.test(value)&&(value > param); }, 'Must be greater than {0}.');
	jQuery.validator.addMethod('lesserThan', function(value, element, param) { return /^[0-9]+$/.test(value)&&(value < param); }, 'Must be lesser than {0}.');
	jQuery.validator.addMethod('numberNative', function(value, element, param) { return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:\,\d+)?$/.test(value); }, 'Not a valid number.');
	jQuery.validator.addMethod('simpleDate', function(value, element, param) { return this.optional(element) || /^\d{1,2}\-\d{1,2}\-\d{4}$/.test(value); }, 'Not a valid date.');
	jQuery.validator.addMethod('integer', function(value, element, param) { var isOk = /^[1-9]\d*$/.test(value) || /^-[1-9]\d*$/.test(value)||(value=="0"); return isOk; }, 'Not a valid int.');
};