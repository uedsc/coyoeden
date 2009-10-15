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
var CategoryM = function() {

	return {
	Init: function() {
			CategoryM.Asmx = LocalApp.Asmx("Category");
		} //endof Init
	};
} ();     //endof CategoryM

CategoryM.InitFlexigrid = function() { 

};//endof CategoryM.InitFlexigrid