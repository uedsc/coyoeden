///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
///<reference path="../jquery/jquery.tools.min.js"/>
///<reference path="../jquery/jquery.jgrowl.js"/>
///<reference path="../jquery/jquery.json-1.3.min.js"/>
///<reference path="../jquery/jquery.dateentry.min.js"/>
///<reference path="../jquery/jquery.validate.js"/>
///<reference path="../date.js"/>
/*====================================================================================
 * javascript logic for PSEXPDailyBill.aspx
 *====================================================================================*/
var PSEXPBillApp = function() {
	//private area-begin
	//!!private area end
	var tipx = BosApp.jqMTipX;
	var setdata = function(name, val, key) {
		key = key || "cfg";
		var _data = $("#btnQuery").data(key) || {};
		_data[name] = val;
		$("#btnQuery").data(key, _data);
	}; //endof setdate
	var validatex = function(showTip) {
		showTip = showTip != false;
		var _data = $("#btnQuery").data("cfg");
		if (!_data) { if (showTip) { tipx("查询条件不完整"); }; return false; };
		if (!_data.DateFlag) { if (showTip) { tipx("请输入日期.(查询、对账单只针对月份)"); }; return false; };
		if ((!_data.ExpressID) || (_data.ExpressID == "-1")) { if (showTip) { tipx("请选择快递公司"); }; return false; };
		return _data;
	};
	var registerEditFormValidator = function() {
		$("#frmPSDailyBill").validate({
			rules: {
				ReturnNumAdjust: { required: true, integer: true },
				ReturnFeeAdjust: { required: true, number: true },
				OtherNumAdjust: { required: true, integer: true },
				OtherFeeAdjust: { required: true, number: true },
				FareDeduction: { required: true, number: true },
				CashTransfer: { required: true, number: true }
			}
		});
		$("#frmPSDailyBill").submit(function() { return false; });
	};
	var registerCfgFormValidator = function() {
		$("#frmExpressConfig").validate({
			rules: {
				UnitFare: { required: true, number: true, min: 0 },
				UnitFareAllReturn: { required: true, number: true, min: 0 },
				JudgeRateAllReturn: { required: true, number: true, min: 0, max: 100 },
				InitialBalance: { required: true, number: true },
				InitialDateStr: { required: true, dateISO: true },
				VerifyDay: { required: true, digits: true, min: 1, max: 28 }
			}
		});
		$("#frmExpressConfig").submit(function() { return false; });
	}; //endof registerCfgFormValidator
	var registerInputEventHandlers = function(opts) {
		$("#jqModal1 input.f_txt").singleSelect({
			after: function(obj) {
				if (opts.onClick) { opts.onClick(obj); };
			}
		}).blur(function() { if (opts.onBlur) { opts.onBlur(this); }; });
		if (PSEXPBillApp.Superable < 0) { $("#txtDateReturned").attr("disabled", true); } else {
			$("#txtDateReturned").blur(function() { setdata("DateReturned", this.value, "jqm"); }).dateEntry({ dateFormat: 'ymd-', maxDate: Date.today() });
		};
	}; //endof registerInputEventHandlers
	var validateReport = function() {
		var cfg = $("#btnQuery").data("cfg");
		if ((!cfg) || (!cfg.DateFlag)) {
			alert("请输入日期.(查询、对账单只针对月份)");
			return false;
		};
		return true;
	};
	var customRange = function(input) {
		return {
			minDate: (input.id == 'txtEndDate' ? $('#txtBeginDate').dateEntry('getDate') : Date.today().addDays(-30)),
			maxDate: (input.id == 'txtBeginDate' ? ($('#txtEndDate').dateEntry('getDate') || Date.today()) : Date.today())
		};
	};
	//Public area below
	return {
		Init: function() {
			PSEXPBillApp.InitLayOut({ roundBox: false });
			PSEXPBillApp.AsmxDefault = BosApp.WebRoot + "Services/Clop/PSEXPDailyBill.asmx/";
			PSEXPBillApp.AsmxExpress = BosApp.WebRoot + "Services/Clop/Express.asmx/";
			PSEXPBillApp.AsmxPSRLog = BosApp.WebRoot + "Services/Clop/PackingSlipReturnedLog.asmx/";

			//event handlers register
			$("#btnQuery").click(PSEXPBillApp.Query);
			$("#btnQuery_sub").live("click", function() { PSEXPBillApp.LoadUnrelateReturnData(); });
			$("#btnCfg").click(PSEXPBillApp.Config);
			$("#btnReport").click(PSEXPBillApp.Report);
			$("#btnReport1").click(PSEXPBillApp.Report1);
			$("#listExpress").change(function() {
				setdata("ExpressID", $(this).val());
				setdata("ExpressDesc", $("option:selected", this).text());
			});
			$("#txtDateFlag").blur(function() {
				setdata("DateFlag", $(this).val());
			}).dateEntry({ dateFormat: 'ymd-', minDate: Date.today().addDays(-50), maxDate: Date.today() });
			//init flexigrid
			PSEXPBillApp.InitFlexigrid(validatex);
		},
		InitLayOut: function(opts) {
			if (opts.roundBox && $.browser.msie) {
				var borderEffect = RUZEE.ShadedBorder.create({ corner: 10, border: 2 });
				$(".roundedBox").each(function() {
					borderEffect.render(this);
				});
			};
		},
		//Query to TPSEXPDailyBill table
		Query: function(opts) {
			var _data = validatex();
			if (!_data) return false;

			PSEXPBillApp.PSGrid.flexOptions({
				criterias: _data
			}).flexReload();
		}, //endof $Q
		Show: function(opts) {
			var id = opts.id || 0; //delete or add
			var row = (opts.rows) ? opts.rows[0] : null;
			if (row && row.id) { id = row.id.substring(3); }; //remove id prefix 'row'
			if (id == "0") return false;
			$.jqMExt.jqmAjaxPost(
				"#jqModal1",
				PSEXPBillApp.AsmxDefault + "Show",
				{ mode: '2', okClick: PSEXPBillApp.Update },
				$.toJSON({ data: { Id: id} }),
				function(msg, jqmApi) {
					if (msg.IsOk) {
						$("#btnQuery").data("jqm", msg.Data);
						registerInputEventHandlers({
							onBlur: function(obj) { setdata(obj.name, obj.value, "jqm"); },
							onClick: function(obj) { var jqmData = $("#btnQuery").data("jqm"); $("#txtMemo").val(jqmData.MemoData[obj.name] || "").attr("title", obj.name); }
						});
						registerEditFormValidator(); //form validation
						$("#txtMemo").blur(function(event) {
							var jqmData = $("#btnQuery").data("jqm");
							jqmData.MemoData[this.title] = this.value;
							setdata("MemoData", jqmData.MemoData, "jqm");
						});
						//restore current query condition
						var lastQ = { ExpressID: msg.Data.ExpressID, DateFlag: msg.Data.DateFlagStr };
						setdata("ExpressID", lastQ.ExpressID); setdata("DateFlag", lastQ.DateFlag);
						$.deserializeX(lastQ, { filter: 'name' });
					};
				}
			);
		}, //endof show
		//update a TPSEXPDailyBill record
		Update: function(event, jqmApi) {
			var isOk = $("#frmPSDailyBill").valid();
			if (!isOk) return false;
			var data1 = $("#btnQuery").data("jqm");
			$.jqMExt.jqmAjaxPost(
                "#jqModal1", PSEXPBillApp.AsmxDefault + "Update", null, $.toJSON({ data: data1 }),
                function(msg, jqmApi1) {
                	if (!msg.IsOk) return false;
                	var lastQueryData = $("#btnQuery").data("cfg");
                	PSEXPBillApp.PSGrid.flexOptions({
                		criterias: lastQueryData
                	}).flexReload();
                	jqmApi.w.jqmHide();
                }
            );
		}, //endof Update
		//get express config data
		Config: function() {
			var data1 = { ExpressID: $("#listExpress").val() };
			if (data1.ExpressID == "-1") { alert("请选择快递公司"); return false; };
			$.jqMExt.jqmAjaxPost(
				"#jqModal1", PSEXPBillApp.AsmxDefault + "GetCfg", { mode: '2', okClick: PSEXPBillApp.UpdateCfg }, $.toJSON({ data: data1 }),
				function(msg, jqmApi) {
					if (!msg.IsOk) return false;
					$("#btnQuery").data("jqm.cfg", msg.Data);
					registerCfgFormValidator();
					registerInputEventHandlers({
						onClick: null,
						onBlur: function(obj) { setdata(obj.name, obj.value, "jqm.cfg"); }
					});
					$("#txtInitialDateStr").dateEntry({
						spinnerImage: '',
						dateFormat: 'ymd-',
						maxDate: Date.today()
					});
				}
			);
		}, //endof Config
		//update config
		UpdateCfg: function(event, jqmApi) {
			var isOk = $("#frmExpressConfig").valid();
			if (!isOk) return false;
			var data1 = $("#btnQuery").data("jqm.cfg");
			$.jqMExt.jqmAjaxPost(
					"#jqModal1", PSEXPBillApp.AsmxDefault + "UpdateCfg", null, $.toJSON({ data: data1 }),
					function(msg, jqmApi1) {
						if (!msg.IsOk) return false;
						jqmApi.w.jqmHide();
					}
				);
		}, //endof update config
		//report
		Report: function(opts) {
			if (!validateReport()) return false;
			var cfg = $("#btnQuery").data("cfg");
			var okCallback = function() {
				$.jqMExt.jqmAjaxPost("#jqModal1", PSEXPBillApp.AsmxDefault + "Report", null, $.toJSON({ data: cfg }), function(msg, jqmApi) {
					//do nothing here
				});
			};
			PSEXPBillApp.LoadExpresses(okCallback, 5);
		}, //endof report
		Report1: function(opts) {
			if (!validateReport()) return false;
			var cfg = $("#btnQuery").data("cfg");
			var okCallback = function() {
				$.jqMExt.jqmAjaxPost("#jqModal1", PSEXPBillApp.AsmxDefault + "Report1", null, $.toJSON({ data: cfg }), function(msg, jqmApi) {
					//do nothing here
				});
			};
			PSEXPBillApp.LoadExpresses(okCallback, 10);
		}, //endof Report1
		EASARBill: function(event, jqmApi1) {
			var cfg = validatex();
			if (!cfg) return false;

			if (jqmApi1) {  //第一次生成应收单
				jqmApi1.w.jqmHide();
				if (jqmApi1.flag == 0) {
					return false;
				} else {
					//do nothing
				};
			};
			cfg.FlagX = jqmApi1 ? jqmApi1.flag : 0;

			$.jqMExt.jqmAjaxPost("#jqModal1", PSEXPBillApp.AsmxDefault + "EASARBill", { mode: '2', okClick: PSEXPBillApp.EASARBill },
				$.toJSON({ data: cfg }),
				function(msg, jqmApi) {
					jqmApi.flag = msg.Flag1;
				}
			);

		}, //endof EASARBill
		LoadUnrelateReturnData: function(opts) {
			if (opts) {
				var id = opts.id || 0; //delete or add
				var row = (opts.rowsData) ? opts.rowsData[0] : null;
				if (row && row.id) { id = row.id; };
				if (id == "0") return false;
				setdata("DateSelected", row.cell[0]);
			};
			var cfg = $("#btnQuery").data("cfg");

			$.jqMExt.jqmAjaxPost(
				"#jqModal1",
				PSEXPBillApp.AsmxPSRLog + "LoadUnrelateReturnData",
				{ mode: '2', okClick: PSEXPBillApp.RelateReturn },
				$.toJSON({ data: cfg }),
				function(msg, jqmApi) {
					if (msg.IsOk) {
						$("#txtBeginDate,#txtEndDate").blur(function() {
							setdata(this.name, this.value);
						}).dateEntry({ dateFormat: 'ymd-', beforeShow: customRange });
						$("#unrelatedData input:checkbox").anySelect({ max: 1 });
					};
				}
			);
		}, //endof LoadUnrelateReturnData
		RelateReturn: function(evt, jqmApi) {
			var selectedDates = $("#unrelatedData input:checked").serializeX();
			if ((!selectedDates.ReturnDates) || (selectedDates.ReturnDates.length == 0)) { return false; };
			setdata("ReturnDates", selectedDates.ReturnDates);
			jqmApi.w.jqmHide();

			var cfg = $("#btnQuery").data("cfg");
			$.jqMExt.jqmAjaxPost(
				"#jqModal1",
				PSEXPBillApp.AsmxDefault + "RelateReturn",
				null,
				$.toJSON({ data: { ExpressID: cfg.ExpressID, DateSelected: cfg.DateSelected, ReturnDates: cfg.ReturnDates} }),
				function(msg, jqmApi1) { }
			);

		}, //endof RelateReturn
		LoadExpresses: function(okCallback, maxSelected) {
			maxSelected = maxSelected || 0;
			var okClick = function(evt, jqmApi) {
				var _data = $("#cbxExpressList input:checked").serializeX();
				setdata("ExpressIDs", _data.ExpressIDs);
				jqmApi.w.jqmHide();
				if (okCallback != null) {
					okCallback(jqmApi);
				};
			};
			$.jqMExt.jqmAjaxPost(
				"#jqModal1",
				PSEXPBillApp.AsmxExpress + "LoadExpresses",
				{ title: '请选择快递公司-最多可选' + maxSelected + '个', mode: '2', okClick: okClick },
				null,
				function() {
					var _data = $("#btnQuery").data("cfg");
					if (_data.ExpressIDs) $.deserializeX({ ExpressIDs: _data.ExpressIDs }, { filter: 'name' });
					$("#cbxExpressList input:checkbox").anySelect({ max: maxSelected });
				}
			);
		} //endof LoadExpresses
	};
} ();
//InitFlexigrid
PSEXPBillApp.InitFlexigrid = function(validateCallback) {
	PSEXPBillApp.PSGrid = $("#fgItems").flexigrid({
		url: PSEXPBillApp.AsmxDefault + 'Query',
		urlType: "asmx",
		dataType: 'json',
		contentType: "application/json;charset=utf-8",
		colModel: [
  					{ display: '日期', name: 'DateFlagStr', width: 60, sortable: true, align: 'center' },
  					{ display: '快递公司', name: 'ExpressDesc', width: 120, sortable: false, align: 'left' },
  					{ display: '退包个数调整', name: 'ReturnNumAdjust', width: 80, sortable: false, align: 'left' },
  					{ display: '其他个数调整', name: 'OtherNumAdjust', width: 80, sortable: false, align: 'left' },
  					{ display: '退包金额调整', name: 'ReturnFeeAdjust', width: 80, sortable: false, align: 'left' },
  					{ display: '其他金额调整', name: 'OtherFeeAdjust', width: 80, sortable: false, align: 'left' },
  					{ display: '运费抵扣金额', name: 'FareDeduction', width: 80, sortable: false, align: 'left' },
  					{ display: '汇款金额', name: 'CashTransfer', width: 60, sortable: false, align: 'left' },
  					{ display: '关联退货', name: 'DateReturned', width: 60, sortable: false, align: 'left' }
							],
		buttons: [
  					{ name: 'Edit', display: '编辑', bclass: 'edit', onpress: function(_params) { PSEXPBillApp.Show(_params); } },
  					{ name: 'Config', display: '配置', bclass: 'cfg', onpress: function(_params) { PSEXPBillApp.Config(); } },
  					{ separator: true },
  					{ name: 'Connect', display: 'EAS应收单', bclass: 'connect', onpress: function(_params) { PSEXPBillApp.EASARBill(); } },
  					{ separator: true },
  					{ name: 'Connect', display: '关联退货', bclass: 'connect', onpress: function(_params) { PSEXPBillApp.LoadUnrelateReturnData(_params); } }
							],
		searchitems: [
  					{ display: '日期', name: 'DateFlagStr' }
							],
		sortname: "DateFlagStr",
		sortorder: "asc",
		usepager: true,
		title: '快递日对账记录',
		useRp: true,
		rp: 40,
		showTableToggleBtn: false,
		height: 220,
		preProcess: function(data) {
			return data.d;
		},
		criterias: null,
		autoload: false,
		onSubmit: function() { var isOk = validateCallback(false); return isOk != false; }
	});
	//fix png of flexigrid
	$("div.pButton,div.fButton span", "div.flexigrid").ifixpng();
};               //endof initFlexigrid


$(document).ready(function() {
    PSEXPBillApp.Init();
    //2,tabs
    $("ul#menu").tabs("div#content > div.tab_p");
});