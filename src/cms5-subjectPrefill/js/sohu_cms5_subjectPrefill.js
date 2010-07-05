/**
 * @author levinhuang
 */
var sohu_cms5_subjectPrefill = function() {
    var p={},pub={};
	p.onOk=function(evt){
		var data=[];
		$("#tbPrefillData tr").not(":first").each(function(i,o){
			data.push({id:$("td:first",o).html(),seid:$("select",o).val()});
		});
		p._txtHidden.val($.toJSON(data));
		parent.sohu_frameModal.Jqm.jqmHide();
		return false;
	};
	p.onClose=function(evt){
		parent.sohu_frameModal.Jqm.jqmHide();
		return false;		
	};
	p.fillData=function(){
		var row=null,d=null,sl=null,opt=null;
		for(var i=0;i<p._jsonData.length;i++){
			d=p._jsonData[i];
			row=$(StringUtils.parseTpl(p._rowTpl,d));
			if(d.entityList){
				$.each(d.entityList,function(i1,o1){
					sl=row.find("select");
					sl.append($(StringUtils.parseTpl(p._optTpl,o1))).change(function(evt){
						opt=$("option:selected",this);
						$(this).next().attr("href",opt.attr("meta"));
					});
					p._row0.after(row);
				});
			};			
		};//for
	};
    //private area
    p.initVar = function(opts) {
		p._row0=$("#tbPrefillData tr:first"); 
		p._rowTpl='<tr><td>%id%</td><td>%name%</td><td>%desc%</td><td><select><option value="default">默认</option><option value="none">不使用</option></select><a href="#">指定专题(内部简称)</a></td></tr>';
		p._optTpl='<option value="%id%" meta="%staticUrl%">%name%</option>';
		p._txtHidden=$("#txtHidden1");
	};
    p.onLoaded = function() { 
		p._jsonData=init_data.tc;
		p.fillData();
	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		$("#btnOK").click(p.onOk);
		$("#btnCls").click(p.onClose);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 