///<reference path="../jquery/jquery-1.3.2.js"/>
///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="Local.Common.js"/>
/*-----Note:This file Contains client logic for Page xxxx-----*/
var this$ = function() {
    //private area
    var p = {};

    p.initVar = function(opts) { };
    p.onLoaded = function() {
        LocalApp.Loading(0);
    };
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
    };
    //public area
    var pub = {};

    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 