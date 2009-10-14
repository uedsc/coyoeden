///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
///<reference path="../jquery/jquery.tools.min.js"/>
///<reference path="../jquery/jquery.jgrowl.js"/>
///<reference path="../jquery/jquery.json-1.3.min.js"/>
///<reference path="../jquery/jquery.dateentry.min.js"/>
///<reference path="../date.js"/>
/*====================================================================================
 * javascript logic for AgingReportII.aspx
 *====================================================================================*/
var AgingReportII = function() {
	//private area-begin
	//!!private area end
	var tipx = BosApp.jqMTipX;
	var validatex = function() {
		var _data = $("#btnCfg").data("cfg");
		if (!_data) { tipx("查询条件不完整,请确保运输组已经选择"); return false; };
		if (!(_data.BeginDate && _data.EndDate)) { tipx("请输入日期"); return false; };
		if (!_data.CarrierGroups) { tipx("请通过配置按钮选择运输组"); return false; };
		return _data;
	};
	//Public area below
	return {
		Init: function() {
			AgingReportII.Notice();
			AgingReportII.InitLayOut({ roundBox: false }); //don't use roundBox,or the jquery.tools.expose won't work
			AgingReportII.AsmxCarrier = BosApp.WebRoot + "Services/Clop/CarrierGroupSV.asmx/";
			AgingReportII.AsmxDefault = BosApp.WebRoot + "Services/Clop/AgingReportIISV.asmx/";
			//event handler register
			$("#btnCfg").click(AgingReportII.loadCarriers);
			$("#btnOK").click(AgingReportII.Query);
			$("#CarrierGroupsWrap input:checkbox").live("click", function() {
				var items = $("#CarrierGroupsWrap input:checked");
				if (items.size() > 2) {
					this.checked = false; return false;
				};
			}); //只能选两个
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
			}).change(AgingReportII.OnInputChanged);
			//report types
			$("#lstReportTypes").change(AgingReportII.OnInputChanged);
		},
		InitLayOut: function(opts) {
			if (opts.roundBox&&$.browser.msie) {
				var borderEffect = RUZEE.ShadedBorder.create({ corner: 10, border: 2 });
				$(".roundedBox").each(function() {
					borderEffect.render(this);
				});
			};
		},
		OnInputChanged: function(event) {
			var _data = $("#btnCfg").data("cfg");
			if (_data) {
				_data[this.name] = this.value;
				$("#btnCfg").data("cfg", _data);
			};
		}, //endof onInputChange
		//Query
		Query: function() {
			var _data = validatex();
			if (!_data) return false;

			//begin Query logic
			$.jqMExt.jqmAjaxPost(
				"#jqModal1",
				AgingReportII.AsmxDefault + "Query",
				null,
				$.toJSON({ data: _data }),
				function(msg, jqmApi) { if (!msg.IsOk) { return; }; jqmApi.w.jqmHide(); $("#tabPanelBody").html(msg.ReportData || msg.Body); }
			);
		}, //endof Query
		Notice: function() {
			$.jGrowl("注：运输组最多可以同时选择2个", {
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
		} //endof Notice
	};
} ();

//Load carrier groups
AgingReportII.loadCarriers = function() {
	var okClick = function(evt, jqmApi) {
		var _data = $("input:checked,input[type='text'],#lstReportTypes").serializeX();
		$("#btnCfg").data("cfg", _data);
		jqmApi.w.jqmHide();
	};

	$.jqMExt.jqmAjaxPost(
		"#jqModal1",
		AgingReportII.AsmxCarrier + "LoadCarriers",
		{ title: '请选择运输商', mode: '2', okClick: okClick },
		null,
		function() {
			var _data = $("#btnCfg").data("cfg");
			if (_data) $.deserializeX(_data, { filter: 'name' });
		}
	);
};



$(document).ready(function() {
	AgingReportII.Init();
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