/**
 * JS interactive logic for vsp.html
 * @author levinhuang
 */
var this$ = function() {
    var p={},pub={};
	/**
	 * 碎片编辑弹框
	 * @param {Object} dlg
	 */
	p.vspDlg=function(dlg){
		var _this=this;
		this.$Layout=$("#wVsp");
		this.$btnCode=this.$Layout.find(".globalRes,.external");
		//事件处理
		
		//事件注册
		this.$Layout.tabs({
			select:function(evt,ui){
				if(ui.index==1){
					_this.$btnCode.removeClass("hide");
				}else{
					_this.$btnCode.addClass("hide");
				};
			}
		});
		
		//jqm options
		this.$Layout.jqmOpts={
			title:"碎片编辑",
			hideActions:true,
			modal:false,
			overlay:false
		};
	};
	/* 事件处理 */
	p.onDialogInit=function(dlg){
		p._vspDlg0=new p.vspDlg(dlg);
		sohu.diyDialog.Register("wVsp",p._vspDlg0);
	};
	/**
	 * 删除元素
	 * @param {Object} evt
	 */
	p.onDelElm=function(evt){
		if(evt.data.$obj){
			evt.data.$obj.remove();
			evt.data.$tpl.remove();
		};
		return false;
	};
	/**
	 * 添加元素
	 * @param {Object} evt
	 */
	p.onAddElm=function(evt){
		if(!evt.data.$obj) return false;
		var txt0=$(document.createTextNode(" "));
		var a0=$(p._aTplStr);
		evt.data.$obj.after(a0).after(txt0);
		
		var tpl=p._$rowTxtA.clone();
		tpl.find("input").val(" ");
		tpl.find(".btnDel").bind("click",{$obj:txt0,$tpl:tpl},p.onDelElm);
		var tpl1=$([p._$rowTitleA.clone()[0],p._$rowLnkA.clone()[0]]);
		//输入框回填内容
		tpl1.eq(0).find("input").val(a0.html());
		tpl1.eq(1).find("input").val(a0.attr("href"));
		//删除按钮
		tpl1.find(".btnDel").bind("click",{$obj:a0,$tpl:tpl1},p.onDelElm);
		//复制按钮
		tpl1.find(".btnAdd").bind("click",{$obj:a0,$tpl:tpl1},p.onAddElm);
		evt.data.$tpl.eq(1).after(tpl1).after(tpl);
		
		return false;
	};
	/**
	 * 编辑碎片的元素
	 * @param {Object} evt
	 */
	p.onEditFlagElem=function(evt){
		p._$elmA.empty();
		//检查元素类型
		p.$curElm=$(this);
		if(p.$curElm.parent().is("li")){
			p._t=0;//列表
		}else if(p.$curElm.find("img").length>0){
			p._t=1;//图片
		}else{
			p._t=2;//段落
		};
		
		p.$elmList=p.$curElm.parent().contents();
		
		p._$elmTpls.hide();
		switch(p._t){
			case 0:
				p.$elmList.each(function(i,o){
					var $o=$(o);
					var tpl=null;
					if(o.nodeType!=1){
						//文本
						tpl=p._$rowTxtA.clone();
						tpl.find("input").val($o.text());
						p._$elmA.append(tpl);
						
					}else if($o.is("a")){
						tpl=$([p._$rowTitleA.clone()[0],p._$rowLnkA.clone()[0]]);
						tpl.eq(0).find("input").val($o.attr("title"));
						tpl.eq(1).find("input").val($o.attr("href"));
						p._$elmA.append(tpl);
						
					};
					//删除按钮
					tpl.find(".btnDel").bind("click",{$obj:$o,$tpl:tpl},p.onDelElm);
					//复制按钮
					tpl.find(".btnAdd").bind("click",{$obj:$o,$tpl:tpl},p.onAddElm);
					
				});
				p._$elmA.show();
			break;
			case 1:
				p._$elmImg.show();
			break;
			case 2:
				p.$elmList.each(function(i,o){
					var $o=$(o);
					if(o.nodeType!=1){
						//文本
						p._$elmA.append(p._$rowTxtA0.clone().find("input").val($o.text()).end());
					}else if($o.is("a")){
						p._$elmA.append(p._$rowTitleA0.clone().find("input").val($o.attr("title")).end())
						.append(p._$rowLnkA.clone().find("input").val($o.attr("href")).end());
					};
				});			
				p._$elmA.show();
			break;
		};//switch
		
		sohu.diyDialog.Show("wVsp");
	};
	/**
	 * 碎片事件注册
	 */
	p.initFlagEvts=function(){
		$(p._cssFlag+" a,#wVsp_acts a").click(function(evt){
			evt.preventDefault();
			return true;
		});
		$(p._cssFlag+" a").bind("click.edit",p.onEditFlagElem);
	};
	p.initElmTplEvts=function(){
		p._$elmA.find(".btnDelTxt").click();
	};
    //private area
    p.initVar = function(opts) { 
		p._cssFlag=opts.cssFlag||".flag";
		p._$elmTpls=$("#wVsp_tab1 .elmTpl");
		p._$elmA=p._$elmTpls.filter(".elmA");
		p._$elmImg=p._$elmTpls.filter(".elmImg");
		p._$rowTxtA=$("#elmATpl .rowTxt");
		p._$rowTitleA=$("#elmATpl .rowTitle");
		p._$rowTxtA0=$("#elmATpl .rowTxt0");
		p._$rowTitleA0=$("#elmATpl .rowTitle0");		
		p._$rowLnkA=$("#elmATpl .rowLnk");
		p._aTplStr=opts.aTplStr||'<a href="http://sohu.com" title="">sohu</a>';
	};
    p.onLoaded = function() { 

	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		sohu.diyDialog.Init({cssDragCTM:'window',onInit:p.onDialogInit});
		$("#wVsp_acts a").button();
		p.initFlagEvts();
		p.initElmTplEvts();
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 