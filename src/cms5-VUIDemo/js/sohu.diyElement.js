/**
 * 可视化编辑元素
 * @dependency sohu.iframeEX,sohu.iEditable,jquery.cssEX
 * @author levin
 */
sohu.diyElement=function(opts){
	opts=$.extend({},{cl:"elm",clOn:"elmOn",inlineEditable:true},opts||{});
	var _this=this;
	this.CT=opts.ct;
	this.$Context=this.CT.$Layout;
	this.$Layout=opts.$dom;
	
	/* private member variables */
	var p={opts:opts};
	p.onEditModeChange=function(dom){
		if (dom.iEditing) {
			sohu.diyConsole.$EditMenu.show();
			_this.CT.InlineEdit("on");
		}else{
			sohu.diyConsole.$EditMenu.hide();
			_this.CT.InlineEdit("off");
		}
			
	};
	p.initEditable=function(){
		if(opts.inlineEditable){
			_this.$Layout.iEditable({
				onModeChange:p.onEditModeChange
			});
		};
	};
	/* /private member variables */
	
	p.Init=function(){
		p.initEditable();
	};
	
	//初始化
	p.Init();
	
	//鼠标事件
	this.$Layout.mouseenter(function(evt){
		_this.$Layout.addClass(opts.clOn);
	});
	this.$Layout.mouseleave(function(evt){
		_this.$Layout.removeClass(opts.clOn);
	});
	this.$Layout.click(function(evt){
		if(sohu.diyConsole.CurElm)
			sohu.diyConsole.CurElm.HideEditor(true);
			
		sohu.diyConsole.CurElm=_this;
		
		
	});
	//屏蔽超链接
	this.$Context.find("a").css("cursor","text").click(function(evt){return false;});
};
sohu.diyElement.prototype.HideEditor=function(ignoreCbk){
	this.$Layout[0].iEdit("off",ignoreCbk||false);
	sohu.diyConsole.CurElm=null;
};
