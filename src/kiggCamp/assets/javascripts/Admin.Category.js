/**
 * @author Levin
 */
var this$ = (function($) {
    var p={},pub={};
	//private area
    p.initVar = function(opts) { 
		p._lang=opts.lang||{
			lblTitle:'Title goes here...',
			lblTag:'Add New Tag'
		};
	};
    p.onLoaded = function() { 
		
	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);

    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
}) (jQuery); 