/**
 * 可视化编辑元素
 * @dependency sohu.iframeEX,sohu.iEditable,jquery.cssEX
 * @author levin
 */
sohu.diyElement=function(opts){
	opts=$.extend({},{cl:"elm",clOn:"elmOn"},opts||{});
	var _this=this;
	this.CT=opts.ct;
	this.$Context=this.CT.$Layout;
	this.$Layout=opts.$dom;
	this.i$frame=null;/* Reference to the iframe editor */
	this.InlineEditable=true;
	this.tagName=this.$Layout[0].tagName.toLowerCase();
	
	/* private member variables */
	var p={opts:opts};
	this.__p=p;
	p.detectType=function(){
		if(_this.tagName=="img")
			_this.InlineEditable=false;
	};
	p.onEditModeChange=function(dom){
		if (dom.iEditing) {
			_this.i$frame=_this.$Layout[0].i$frame;
			sohu.diyConsole.$EditMenu.show();
			sohu.diyConsole.$MenuTxt.show();
			sohu.diyConsole.$MenuImg.hide();
			_this.CT.InlineEdit("on");
		}else{
			_this.i$frame=null;
			sohu.diyConsole.$CPKWrap.hide();
			sohu.diyConsole.$EditMenu.hide();
			_this.CT.InlineEdit("off");
		}
			
	};
	p.initEditable=function(){
		//inline editable element
		if(_this.InlineEditable){
			_this.$Layout.iEditable({
				onModeChange:p.onEditModeChange
			});
			return;
		};
		//img
		if(_this.tagName=="img"){
			_this.$Layout.click(function(evt){
				sohu.diyConsole.$EditMenu.show();
				sohu.diyConsole.$MenuTxt.hide();
				sohu.diyConsole.$MenuImg.show();
				_this.CT.InlineEdit("on");
			});
			return;
		}
	};
	/* /private member variables */
	
	p.Init=function(){
		p.detectType();
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
	this.$Layout.mousedown(function(evt){
		sohu.diyConsole.$PopWins.hide();
		sohu.diyConsole.$Menu.removeClass("on");
		if(sohu.diyConsole.CurElm)
			sohu.diyConsole.CurElm.HideEditor(true);
			
		sohu.diyConsole.CurElm=_this;
		
	});
	//屏蔽超链接
	this.$Context.find("a").css("cursor","text").click(function(evt){return false;});
};
sohu.diyElement.prototype.HideEditor=function(ignoreCbk){
	if(this.InlineEditable){
		this.$Layout[0].iEdit("off",ignoreCbk||false);
	}else{
		sohu.diyConsole.$EditMenu.hide();
		this.CT.InlineEdit("off");
	}
	
	sohu.diyConsole.CurElm=null;
};
