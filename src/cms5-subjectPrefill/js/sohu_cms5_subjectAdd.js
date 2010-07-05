/**
 * @author levinhuang
 */
var sohu_cms5_subjectAdd = function() {
    var p={},pub={};
	p.prefill=function(evt){
		sohu_frameModal.Show({
			jqm:'#modalWindow',
			url:"subject_prefill.html",
			data:{p1:1,p2:2},
			onOpen:function(hash){
			},
			onHide:function(hash,opts){
				alert(opts.ifDoc.getElementById("txtHidden1").value);
			}
		});
		return false;
	};
    //private area
    p.initVar = function(opts) { 
	};
    p.initEvents = function(opts) {
		$("#btnPrefill").click(p.prefill);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 