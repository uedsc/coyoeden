///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
///<reference path="../jquery/jquery.tools.min.js"/>
///<reference path="../jquery/jquery.jgrowl.js"/>
///<reference path="../jquery/jquery.json-1.3.min.js"/>
///<reference path="../jquery/jquery.dateentry.min.js"/>
///<reference path="../date.js"/>
/*====================================================================================
 * javascript logic for Shop/JiXiaoJiBu.aspx
 *====================================================================================*/
var JiXiaoJiBu = function() {
	//!!private area end
	var tipx = BosApp.jqMTipX;
	var validatex = function() {
		var _data = $("#btnOK").data("cfg");
		if (!_data) { tipx("查询条件不完整,请输入日期"); return false; };
		if (!(_data.BeginDate && _data.EndDate)) { tipx("请输入日期"); return false; };
		return _data;
	};
	//public area
	return {
		Init: function() {
			JiXiaoJiBu.Notice();
			JiXiaoJiBu.InitLayout({ roundBox: false });
			JiXiaoJiBu.AsmxDefault = BosApp.WebRoot + "Services/Shop/CatsAndDogs.asmx/";
			$("#btnOK").click(JiXiaoJiBu.Query);
			//daterange
			$(".dateRange").dateEntry({
				dateFormat: 'ymd-',
				beforeShow: function(input) {
					var d = new Date(2008, 0, 1);
					return {
						minDate: (input.id == 'txtDate2' ? $('#txtDate1').dateEntry('getDate').addDays(1) : d),
						maxDate: (input.id == 'txtDate1' ? $('#txtDate2').dateEntry('getDate') : null)
					};
				} //endof beforeShow
			}).change(JiXiaoJiBu.OnInputChanged);
		},
		InitLayout: function(opts) {
			if (opts.roundBox) {
				var borderEffect = RUZEE.ShadedBorder.create({ corner: 10, border: 2 });
				$(".roundedBox").each(function() {
					borderEffect.render(this);
				});
			}
		},
		Query: function() {
			var _data = validatex();
			if (!_data) return false;

			//begin Query logic
			$.jqMExt.jqmAjaxPost(
					"#jqModal1",
					JiXiaoJiBu.AsmxDefault + "JiXiaoJiBu",
					null,
					$.toJSON({ data: _data }),
					function(msg, jqmApi) { if (!msg.IsOk) { return; }; jqmApi.w.jqmHide(); $("#tabPanelBody").html(msg.ReportData || msg.Body); }
				);
		}, //endof Query
		Notice: function() {
			$.jGrowl("注：即销即补每天只能执行一次，任何问题请联系IT部", {
				sticky: true,
				beforeOpen: function(e, m, o) {
					var cheader = $("#header");
					var offset = cheader.offset();
					$(".default", e).css({
						'margin-top': cheader.height() + 'px',
						'margin-right': (offset.left - 5) + 'px'
					});
				}
			});
		}, //endof Notice
		OnInputChanged: function(event) {
			var _data = $("#btnOK").data("cfg");
			_data = _data || {};
			if (_data) {
				_data[this.name] = this.value;
				$("#btnOK").data("cfg", _data);
			};
		} //endof onInputChange
	};
} ();
$(document).ready(function() {
	JiXiaoJiBu.Init();
	//2,tabs
	$("ul#menu").tabs("div#content > div.tab_p");
	//3,expose tab panel's title bar
	var exposeApi = $("#tab_p_title").expose({
		api: true,
		onLoad: function() {
			var obj = this.getExposed();
			obj.addClass("exposed");
			$("input:first", obj).focus();
		},
		onClose: function() {
			this.getExposed().removeClass("exposed");
		}
	});
	$("#tab_p_title input[type='text']").mouseover(function() {
		exposeApi.load();
	});
	$("#tab_p_title .noexpose").click(exposeApi.close);
});