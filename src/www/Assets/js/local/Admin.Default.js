///<reference path="../jquery/jquery-1.3.2.js"/>
///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
///<reference path="../jquery/jquery.tools.min.js"/>
///<reference path="../jquery/jquery.jgrowl.js"/>
///<reference path="../jquery/jquery.json-1.3.min.js"/>
///<reference path="../jquery/jquery.dateentry.min.js"/>
///<reference path="../jquery/jquery.validate.js"/>
///<reference path="Local.Common.js"/>
///<reference path="../date.js"/>
var AdminApp = function() {
	//private area
	var undefined;
	var tipx = LocalApp.jqMTipX;
	var registerEventHandlers = function() {
		$("#fmAddWidget").validate({
			rules: {
				WidgetName: { required: true },
				WidgetZone: { required: true }
			},
			errorElement: 'em'
		});
		$("#fmAddWidget").submit(function() { return false; });
		$("#listWidget,#listZone").change(function() {
			LocalApp.SetData(this.name, this.value, "widget");
		});
		$("#btnAddWidget").click(AdminApp.AddWidget);
	};
	var onPreAddWidget = function() {
		if (!$("#fmAddWidget").valid()) { return false; };
		var _data = LocalApp.GetData("widget");
		if ((!_data.WidgetName) || (!_data.WidgetZone)) { $.growlUI("Please select a widget and a zone!"); return false; };
		if (!window.confirm("Are u sure to add a [" + _data.WidgetName + "] to the zone [" + _data.WidgetZone + "]?")) { return false; };
		return _data;
	}; //endof onPreAddWidget

	//public area
	var p = {};
	p.Init = function() {
		registerEventHandlers();
		LocalApp.SetData("WidgetTag", $("#txtPageTag").val(), "widget");
	}; //endof Init
	p.RemoveWidget = function() {
		if (!window.confirm("Are you sure to delete?")) return false;
		id =$(this).parents("li.widget").attr("id");
		$.jqMExt.jqmAjaxPost(
			"#jqModal1",
			LocalApp.Asmx("WidgetService", "RemoveWidget"),
			null, $.toJSON({ data: { Id: id} }),
			function(msg, jqmApi) {
				if (msg.IsOk) {
					$("#" + id).fadeOut('normal', function() { $("#" + id).remove(); });
				}
			}
		);
		return false;
	}; // /RemoveWidget
	p.AddWidget = function() {
		var _data = onPreAddWidget();
		if (!_data) return false;
		$.jqMExt.jqmAjaxPost(
			"#jqModal1",
			LocalApp.Asmx("WidgetService", "AddWidget"),
			null,
			$.toJSON({ data: _data }),
			function(msg, jqmApi) {
				if (msg.IsOk) {
					$("#widgetzone_" + _data.WidgetZone).append(msg.Extra.data);
					$("#" + msg.Extra.widgetID + " a.remove").click(p.RemoveWidget);
				};
			}
		);
	}; //endof AddWidget
	return p;
} ();

$(document).ready(function() {
	AdminApp.Init();
	$("#widgetZones a.remove").click(AdminApp.RemoveWidget);
});