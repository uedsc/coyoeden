///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
///<reference path="../jquery/jquery.tools.min.js"/>
///<reference path="../jquery/jquery.jgrowl.js"/>
///<reference path="../jquery/jquery.json-1.3.min.js"/>
///<reference path="../jquery/jquery.dateentry.min.js"/>
///<reference path="BosApp.Common.js"/>
///<reference path="../date.js"/>
///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
///<reference path="../jquery/jquery.tools.min.js"/>
///<reference path="../jquery/jquery.jgrowl.js"/>
///<reference path="../jquery/jquery.json-1.3.min.js"/>
///<reference path="../jquery/jquery.dateentry.min.js"/>
///<reference path="BosApp.Common.js"/>
///<reference path="../date.js"/>

var EMSBillApp = function() {
	var undefined;
	var tipx = BosApp.jqMTipX;
	var setdata = function(name, val) {
		var _data = $("#btnQuery").data("cfg") || {};
		_data[name] = val;
		$("#btnQuery").data("cfg", _data);
	}; //endof setdate
	var validatex = function(showTip) {
		showTip = showTip != false;
		var _data = $("#btnQuery").data("cfg");
		if (!_data) { if (showTip) { tipx("查询条件不完整"); }; return false; };
		if (!_data.BeginDate) { if (showTip) { tipx("请输入开始日期"); }; return false; };
		if (!_data.EndDate) { if (showTip) { tipx("请输入结束日期"); }; return false; };
		return _data;
	};
	var customRange = function(input) {
		return {
			minDate: (input.id == 'txtDate2' ? $('#txtDate1').dateEntry('getDate') : Date.today().addDays(-30)),
			maxDate: (input.id == 'txtDate1' ? ($('#txtDate2').dateEntry('getDate') || Date.today()) : Date.today())
		};
	};
	var showGrid = function(gridName) {
		$("#tabPanelBody .flexigrid").hide().filter(function() {
			return $("#fg_" + gridName, this).size() > 0;
		}).show();
	};
	//!!private area end

	//Public area below
	return {

		Init: function() {
			//event handlers register
			$("#btnQuery").click(EMSBillApp.Query);
			$("#btnReport").click(EMSBillApp.Report);
			$("#btnReport1").click(EMSBillApp.Report1);
			$("#btnImport").click(EMSBillApp.Import);

			$("#txtDate1,#txtDate2").blur(function() {
				setdata(this.name, this.value);
			}).dateEntry({ dateFormat: 'ymd-', beforeShow: customRange });
		},
		Import: function() {
			BosApp.PopUpload({ rel: 'emsbill', relval: '{}', fileTypes: '*.xls' });
		}, //endof Import
		//Query
		Query: function(opts) {
			var _data = validatex();
			if (!_data) return false;

			if (EMSBillApp.Grids.EMSBillList == undefined) {
				EMSBillApp.InitGrid(validatex, EMSBillApp.Grids.Opts.EMSBillList);
			};
			showGrid(EMSBillApp.Grids.Opts.EMSBillList.gridName);
			EMSBillApp.Grids.Opts.EMSBillList.criterias = _data;
			EMSBillApp.Grids.EMSBillList.flexOptions(EMSBillApp.Grids.Opts.EMSBillList).flexReload();
		}, //endof Query
		Delete: function(opts) {
			var keys = $.map($.makeArray(opts.rows), function(node, i) {
				return node.id.substring(3); //remove id prefix 'row'
			});
			var tip = "是否确定删除选中的" + keys.length + "行？";
			if (!window.confirm(tip)) return false;
			//do delete
			var cfg = {};
			cfg.Keys = keys;
			$.jqMExt.jqmAjaxPost("#jqModal1", EMSBillApp.AsmxDefault + "Delete", null, $.toJSON({ data: cfg }), function(msg, jqmApi) {
				if (!msg.IsOk) return false;
				var d = validatex(false);
				if (!d) return false;
				EMSBillApp.Grids.EMSBillList.flexOptions({
					criterias: d
				}).flexReload();
				//jqmApi.w.jqmHide();
			});
		}, //endof Delete
		Report: function() {
			var _data = validatex();
			if (!_data) return false;

			if (EMSBillApp.Grids.EMSBillReport == undefined) {
				EMSBillApp.InitGrid(validatex, EMSBillApp.Grids.Opts.EMSBillReport);
			};
			showGrid(EMSBillApp.Grids.Opts.EMSBillReport.gridName);
			EMSBillApp.Grids.Opts.EMSBillReport.criterias = _data;
			EMSBillApp.Grids.EMSBillReport.flexOptions(EMSBillApp.Grids.Opts.EMSBillReport).flexReload();
		}, //endof Report
		Report1: function() {
			///<summary>未结算EMS报表</summary>
			var _data = validatex();
			if (!_data) return false;

			if (EMSBillApp.Grids.UnsettledBillList == undefined) {
				EMSBillApp.InitGrid(validatex, EMSBillApp.Grids.Opts.UnsettledBillList);
			};
			showGrid(EMSBillApp.Grids.Opts.UnsettledBillList.gridName);
			EMSBillApp.Grids.Opts.UnsettledBillList.criterias = _data;
			EMSBillApp.Grids.UnsettledBillList.flexOptions(EMSBillApp.Grids.Opts.UnsettledBillList).flexReload();
		}, //endof Report1
		Save: function() {
			var excel = $("#btnReport").data("excel");
			if (excel) {
				$.jqMExt.jqm("#jqModal1", { title: "右单击另存为Excel", content:excel, mode: '1' });
			};
		} //endof Save
	};
} ();
EMSBillApp.AsmxDefault = BosApp.WebRoot + "Services/Clop/EMSBill.asmx/";
//InitFlexigrid
EMSBillApp.Grids = {
	Opts: {
		EMSBillList: {
			gridName: 'EMSBillList',
			url: EMSBillApp.AsmxDefault + 'Query',
			colModel: [
  							{ display: 'EMS单号', name: 'EMSID', width: 120, sortable: true, align: 'center' },
  							{ display: '代收金', name: 'CodAmount', width: 80, sortable: false, align: 'left' },
  							{ display: '结算服务费', name: 'ServiceSettledAmount', width: 100, sortable: false, align: 'left' },
  							{ display: '结算费用', name: 'SettledFee', width: 80, sortable: false, align: 'left' },
  							{ display: '退包结算费用', name: 'SettledFeeReturn', width: 120, sortable: false, align: 'left' },
  							{ display: '退包服务费', name: 'ServiceSettledAmountReturn', width: 100, sortable: false, align: 'left' }
									],
			buttons: [
  							{ name: 'Delete', display: '删除', bclass: 'delete', onpress: function(_params) { EMSBillApp.Delete(_params); } }
									],
			searchitems: [
  							{ display: 'EMS单号', name: 'EMSID' }
									],
			sortname: "EMSID",
			title: 'EMS导入清单'
		},
		EMSBillReport: {
			gridName: 'EMSBillReport',
			url: EMSBillApp.AsmxDefault + 'Report',
			colModel: [
  								{ display: '日期', name: 'ScanDate', width: 70, sortable: true, align: 'center' },
  								{ display: '发货数', name: 'PSNumber', width: 50, sortable: false, align: 'left' },
  								{ display: '代收金', name: 'CodAmount', width: 50, sortable: false, align: 'left' },
  								{ display: '妥投个数', name: 'PSNumberProper', width: 50, sortable: false, align: 'left' },
  								{ display: '服务费', name: 'ServiceAmount', width: 50, sortable: false, align: 'left' },
  								{ display: '结算费用', name: 'SettledFee', width: 50, sortable: false, align: 'left' },
  								{ display: '全退个数', name: 'PSNumberX', width: 50, sortable: false, align: 'left' },
  								{ display: '全退金额', name: 'ReturnFeeX', width: 50, sortable: false, align: 'left' },
  								{ display: '退回邮费', name: 'ReturnPostFee', width: 50, sortable: false, align: 'left' },
  								{ display: '汇款金额', name: 'CashTransfer', width: 50, sortable: false, align: 'left' },
  								{ display: '未结算包裹数', name: 'PSNumberUnsettled', width: 75, sortable: false, align: 'left' },
  								{ display: '未结算代收金', name: 'CodAmountUnsettled', width: 75, sortable: false, align: 'left' }
										],
			buttons: [
  						{ name: 'Save', display: '下载', bclass: 'save', onpress: function(_params) { EMSBillApp.Save(_params); } }
  					  ],
			sortname: "ScanDate",
			title: 'EMS对账单',
			onSuccess: function(data) {
				$("#btnReport").data("excel",data.ExtraData.Excels);
			}
		},
		UnsettledBillList: {
			gridName: 'UnsettledBillList',
			url: EMSBillApp.AsmxDefault + 'Report1',
			colModel: [
  									{ display: 'EMS单', name: 'EMSID', width: 120, sortable: false, align: 'left' },
  									{ display: '发货单', name: 'PSNumber', width:120, sortable: false, align: 'left' },
  									{ display: '发货日期', name: 'ScanDate1', width:120, sortable: true, align: 'center' },
  									{ display: '代收金', name: 'CodAmount', width: 120, sortable: false, align: 'left' }
											],
			buttons: [
  							{ name: 'Save', display: '下载', bclass: 'save', onpress: function(_params) { EMSBillApp.Save(_params); } }
  						  ],
			sortname: "EMSID",
			title: '未结算EMS清单',
			onSuccess: function(data) {
				$("#btnReport").data("excel", data.ExtraData.Excels);
			}
		}
}//endof Opts
	};

	EMSBillApp.InitGrid = function(validateCallback, opts) {
		var before = function() {
			$("#tabPanelBody .flexigrid").hide(); //hide all grids firstly
		};
		var after = function(g) {
			EMSBillApp.Grids[opts.gridName] = g;
		};
		$.fGrid(validateCallback, opts, before, after);
	};//endof initFlexigrid


$(document).ready(function() {
	EMSBillApp.Init();
	//2,tabs
	$("ul#menu").tabs("div#content > div.tab_p");
});
