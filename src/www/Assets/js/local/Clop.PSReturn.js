///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
///<reference path="../jquery/jquery.tools.min.js"/>
///<reference path="../jquery/jquery.jgrowl.js"/>
///<reference path="../jquery/jquery.json-1.3.min.js"/>
///<reference path="../jquery/jquery.dateentry.min.js"/>
///<reference path="BosApp.Common.js"/>
///<reference path="../date.js"/>
/*====================================================================================
 * javascript logic for PSReturn.aspx
 *====================================================================================*/

var PSReturnApp = function() {
	var undefined;
	var tipx = BosApp.jqMTipX;
	var setdata = function(name, val, key) {
		key = key || "cfg";
		var _data = $("#btnQuery").data(key) || {};
		_data[name] = val;
		$("#btnQuery").data(key, _data);
	}; //endof setdata
	var validatex = function(showTip) {
		showTip = showTip != false;
		var _data = $("#btnQuery").data("cfg");
		if (!_data) { if (showTip) { tipx("查询条件不完整"); }; return false; };
		if (!_data.ReturnDate) { if (showTip) { tipx("请输入退包日期"); }; return false; };
		if (!_data.ExpressID) { if (showTip) { tipx("请选择快递公司"); }; return false; };
		return _data;
	};
	var customRange = function(input) {
		return {
			minDate: (input.id == 'txtDate2' ? $('#txtDate1').dateEntry('getDate') : Date.today().addDays(-30)),
			maxDate: (input.id == 'txtDate1' ? ($('#txtDate2').dateEntry('getDate') || Date.today()) : Date.today())
		};
	};
	var registerEditFormValidator = function() {
		$("#frmPSReturnItem").validate({
			rules: {
				AmountAdjust: { required: true, number: true },
				ReturnRemark: { required: true }
			}
		});
		$("#frmPSReturnItem").submit(function() { return false; });
	};
	var registerInputEventHandlers = function(opts) {
		$("#jqModal1 input.f_txt,#txtMemo").singleSelect({
			after: function(obj) {
				if (opts.onClick) { opts.onClick(obj); };
			}
		}).blur(function() { if (opts.onBlur) { opts.onBlur(this); }; });
	}; //endof registerInputEventHandlers
	var registerSingleImportEventHandlers = function() {
		$("ul#menu1").tabs("div#PSReturnImportWrapper > div.tab_p", {
			onClick: function(i) {
				$.jqMExt.ieBGFix(); $("input[type='text']:first", "#jqModal1").focus(); setdata("tab", i);
			}
		});
		//set default tab  and default batchid
		setdata("tab", 0); setdata("BatchID", "1");
		$("#listBatchSN").change(function() {
			setdata("BatchID", $(this).val());
			var cfg = $("#btnQuery").data("cfg");
			if (cfg.tab == 0) {
				BosApp.InitUpload("#swfuWrapper", { rel: 'psreturn', relval: $.toJSON(cfg), fileTypes: '*.txt' });
			};
		});
		$("#txtPackingSlipID").blur(function() { setdata("PackingSlipID", $(this).val()); });
		$("#txtDateBatch").val($("#txtDateReturned").val()).blur(function() {
			setdata("DateBatch", $(this).val());
		}).dateEntry({ dateFormat: 'ymd-', minDate: Date.today().addDays(-50), maxDate: Date.today() });
	};
	//!!private area end

	//Public area below
	return {
		Init: function() {
			PSReturnApp.InitLayOut({ roundBox: false });
			//event handlers register
			$("#btnQuery").click(PSReturnApp.Query); $("#btnQuery_tab2").click(PSReturnApp.Query1);
			$("#btnReport").click(PSReturnApp.Report); $("#btnReport1").click(PSReturnApp.ReportTemp);
			$("#listExpress").change(function() {
				setdata("ExpressID", $(this).val());
				setdata("ExpressDesc", $("option:selected", this).text());
			});
			$("#txtDateReturned").blur(function() {
				setdata("ReturnDate", $(this).val());
			}).dateEntry({ dateFormat: 'ymd-', minDate: Date.today().addDays(-50), maxDate: Date.today() });
			$("#txtDate1,#txtDate2").dateEntry({ dateFormat: 'ymd-', beforeShow: customRange });
			//2,tabs
			$("ul#menu").tabs("div#content > div.tab_p", {
				onClick: function(i) {
					if (i == 0) { PSReturnApp.InitGrid(validatex, PSReturnApp.Grids.Opts.PSList); return; };
					PSReturnApp.InitGrid(null, PSReturnApp.Grids.Opts.PSReturnStat);
				}
			});
		},
		InitLayOut: function(opts) {
			if (opts.roundBox && $.browser.msie) {
				var borderEffect = RUZEE.ShadedBorder.create({ corner: 10, border: 2 });
				$(".roundedBox").each(function() {
					borderEffect.render(this);
				});
			}
		},
		//Query
		Query: function(opts) {
			var _data = validatex();
			if (!_data) return false;

			PSReturnApp.Grids["PSList"].flexOptions({
				criterias: _data
			}).flexReload();
		}, //endof Query
		Query1: function(opts) {
			var _data = $("#txtDate1,#txtDate2").serializeX();

			PSReturnApp.Grids["PSReturnStat"].flexOptions({
				criterias: _data
			}).flexReload();
		}, //endof Query1
		PreImport: function(id, opts) {
			var data = validatex(true);
			if (!data) return false;
			var editUrl = BosApp.WebRoot + "Views/Clop/PSReturnImport.htm";
			$.jqMExt.jqmAjax(
				"#jqModal1",
				editUrl,
				{
					mode: '2',
					title: '退包单录入-' + data.ExpressDesc,
					okClick: PSReturnApp.Save,
					onLoad: function(_api) {
						registerSingleImportEventHandlers();
						BosApp.InitUpload("#swfuWrapper", { rel: 'psreturn', relval: $.toJSON(data), fileTypes: '*.txt' });

					} //endof onLoad
				}
			); //endof jqmAjax
		}, //endof PreImport
		Show: function(opts) {
			if (opts.rowsData.length == 0) {
				alert("未选中任何记录"); return false;
			};
			var row = opts.rowsData[0];
			$.jqMExt.jqmAjaxPost(
				"#jqModal1",
				PSReturnApp.AsmxDefault + "Show",
				{ mode: '2', okClick: PSReturnApp.Update },
				$.toJSON({ data: { Id: row.id} }),
				function(msg, jqmApi) {
					if (msg.IsOk) {
						$("#btnQuery").data("jqm", msg.Extra.Data);
						registerInputEventHandlers({
							onBlur: function(obj) { setdata(obj.name, obj.value, "jqm"); }
						});
						registerEditFormValidator(); //form validation
						//restore current query condition
						var lastQ = { ExpressID: msg.Extra.Data.ExpressID, ReturnDate: msg.Extra.Data.DateReturnedStr };
						setdata("ExpressID", lastQ.ExpressID); setdata("ReturnDate", lastQ.ReturnDate);
						$.deserializeX(lastQ, { filter: 'name' });
					};
				}
			);
		}, //endof Show
		Verify: function(opts) {
			if (opts.rowsData.length == 0) {
				alert("未选中任何记录"); return false;
			};
			var row = opts.rowsData[0];
			var obj = opts.grid.getModel(row.cell);
			if (obj.Flag == 1) {
				alert("该记录已经审核提交财务,无法重复审核"); return false;
			};
			if (!window.confirm("请确认清单报表无误，此报表只能生成一次，用于财务快递对账！")) {
				return false;
			};
			$.jqMExt.jqmAjaxPost("#jqModal1", PSReturnApp.AsmxDefault + "Report", null, $.toJSON({ data: obj }), function(msg, jqmApi) {
				if (!msg.IsOk) return false;
				PSReturnApp.Query1();
			});
		}, //endof Verify
		//update a record
		Update: function(event, jqmApi) {
			var isOk = $("#frmPSReturnItem").valid();
			if (!isOk) return false;
			var data1 = $("#btnQuery").data("jqm");
			$.jqMExt.jqmAjaxPost(
                "#jqModal1", PSReturnApp.AsmxDefault + "Update", null, $.toJSON({ data: data1 }),
                function(msg, jqmApi1) {
                	if (!msg.IsOk) return false;
                	var lastQueryData = $("#btnQuery").data("cfg");
                	PSReturnApp.Grids["PSList"].flexOptions({
                		criterias: lastQueryData
                	}).flexReload();
                	jqmApi.w.jqmHide();
                }
            );
		}, //endof Update
		Save: function(event, jqmApi) {
			var cfg = $("#btnQuery").data("cfg");
			if (cfg.tab == undefined) return false;
			if (cfg.tab == 0) {
				jqmApi.w.jqmHide(); return false;
			};
			//逐单导入
			if ($.trim(cfg.PackingSlipID) == "") {
				alert("请输入包裹单号"); return false;
			};
			$("#jqModal1").block({ message: '正在处理数据...' });
			$.ajaxJsonPost(
				PSReturnApp.AsmxDefault + "Add",
				$.toJSON({ data: cfg }),
				function(msg) {
					msg = msg.d || msg;
					$("#jqModal1").block({ message: msg.Title, timeout: 2000 });
					$("#txtPackingSlipID").select();
					if (!msg.IsOk) return false;
					var d = validatex(false);
					if (!d) return false;
					PSReturnApp.Grids["PSList"].flexOptions({
						criterias: d
					}).flexReload();
				},
				function() {
					alert("服务器通讯错误");
					$("#jqModal1").unblock();
				}
			);
		}, //endof Save
		Delete: function(opts) {
			var keys = $.map($.makeArray(opts.rows), function(node, i) {
				return node.id.substring(3); //remove id prefix 'row'
			});
			var cfg = validatex();
			if (!cfg) return false;
			cfg.ExpressDesc = $("option:selected", "#listExpress").text();
			//没选中任何行，则删除当天快递公司当前日期的所有包裹单
			var tip = "是否确定删除选中的" + keys.length + "行？";
			if (keys.length == 0) {
				tip = "没有选中任何行,是否删除[%ExpressDesc%]在%ReturnDate%的所有包裹单?".parseTpl(cfg);
			};
			if (!window.confirm(tip)) return false;
			//do delete
			cfg.Keys = keys;
			$.jqMExt.jqmAjaxPost("#jqModal1", PSReturnApp.AsmxDefault + "Delete", null, $.toJSON({ data: cfg }), function(msg, jqmApi) {
				if (!msg.IsOk) return false;
				var d = validatex(false);
				if (!d) return false;
				PSReturnApp.Grids["PSList"].flexOptions({
					criterias: d
				}).flexReload();
				//jqmApi.w.jqmHide();
			});
		}, //endof Delete
		ReportTemp: function() {
			///<summary>退包批次报表</summary>
			var cfg = validatex();
			if (!cfg) return false;
			$.jqMExt.jqmAjaxPost("#jqModal1", PSReturnApp.AsmxDefault + "Report1", null, $.toJSON({ data: cfg }), function(msg, jqmApi) {
				if (!msg.IsOk) return false;
			});
		}, //endof ReportTemp
		FileDialogDone: function(numFilesSelected, numFilesQueued) {
			var data = $("#btnQuery").data("cfg");
			//2,add the return date to the upload post parameters
			PSReturnApp.swfu.addPostParam('ReturnDate', data.ReturnDate);
			PSReturnApp.swfu.addPostParam("ExpressID", data.ExpressID);
			//invoke the fileDialogComplete handler defined in SWFUploadHandler.js
			fileDialogComplete(numFilesSelected, numFilesQueued, this);
		} //endof FileDialogDone
	};
} ();
PSReturnApp.AsmxDefault = BosApp.WebRoot + "Services/Clop/PackingSlipReturned.asmx/";
PSReturnApp.AsmxPSRLog = BosApp.WebRoot + "Services/Clop/PackingSlipReturnedLog.asmx/";
PSReturnApp.Grids = {
	Opts: {
		PSList: {
			gridName: 'PSList',
			url: PSReturnApp.AsmxDefault + 'Query',
			colModel: [
  					{ display: '批次号', name: 'PSReturnBatchNO', width: 120, sortable: true, align: 'center' },
  					{ display: '发包单号', name: 'PackingslipID', width: 120, sortable: false, align: 'left' },
  					{ display: '发包金额', name: 'ShipAmount', width: 120, sortable: false, align: 'left' },
  					{ display: '差额调整', name: 'AmountAdjust', width: 120, sortable: false, align: 'left' },
  					{ display: '录入人员', name: 'UserUpdate', width: 60, sortable: false, align: 'left' },
  					{ display: '清单日期', name: 'DateBatchStr', width: 60, sortable: false, align: 'left' }
							],
			buttons: [
  					{ name: 'Add', display: '录入', bclass: 'add', onpress: function(_params) { PSReturnApp.PreImport(-1, _params); } },
  					{ name: 'Edit', display: '编辑', bclass: 'edit', onpress: function(_params) { PSReturnApp.Show(_params); } },
  					{ separator: true },
  					{ name: 'Delete', display: '删除', bclass: 'delete', onpress: function(_params) { PSReturnApp.Delete(_params); } }
							],
			searchitems: [
  					{ display: '包裹单号', name: 'PackingSlipID' }, { display: '批次编号', name: 'PSReturnBatchNO' }
							],
			sortname: "PSReturnBatchNO",
			title: '退包单列表'
		},
		PSReturnStat: {
			gridName: 'PSReturnStat',
			url: PSReturnApp.AsmxPSRLog + 'Query',
			colModel: [
				{ display: '编号', name: 'ExpressID', width: 55, sortable: false, align: 'center',hide:true},
				{ display: '快递公司', name: 'ExpressDesc', width: 120, sortable: false, align: 'center' },
				{ display: '退包日期', name: 'DateReturned', width: 65, sortable: false, align: 'center' },
				{ display: '清单日期', name: 'DateBatch', width: 65, sortable: false, align: 'left' },
				{ display: '清单个数', name: 'PSNumber', width: 60, sortable: false, align: 'left' },
				{ display: '审核时间', name: 'VerifiedOn', width: 120, sortable: false, align: 'left' },
				{ display: '审核人', name: 'VerifiedBy', width: 60, sortable: false, align: 'left' },
				{ display: '审核个数', name: 'PSNumberX', width: 50, sortable: false, align: 'left' },
				{ display: '审核标志', name: 'Flag', width: 50, sortable: false, align: 'left'}
						],
			buttons: [
  						{ name: 'Verify', display: '审核', bclass: 'edit', onpress: function(_params) { PSReturnApp.Verify(_params); } }
  					  ],
			sortname: "ExpressDesc",
			searchitems: [{ display: '审核(1/0)', name: 'Flag' }, { display: '快递公司', name: 'ExpressDesc'}],  			
			title: '退包审核情况'
		}
	}//endof Opts
};
//InitFlexigrid
PSReturnApp.InitGrid = function(validateCallback, opts) {
	var before = function() {
	};
	var after = function(g) {
		PSReturnApp.Grids[opts.gridName] = g;
		if (opts.gridName == "PSReturnStat") {
			if (PSReturnApp.Verifyable < 0) { $("div.tDiv", "#tabPanelBody1").block({ message: null, css: { cursor: 'default'} }); };
		};
	};
	$.fGrid(validateCallback, opts, before, after);
};     //endof initFlexigrid


$(document).ready(function() {
	PSReturnApp.Init();
});
