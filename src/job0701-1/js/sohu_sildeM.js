/**
 * @author levinhuang
 */
var sohu_slideM = function() {
    var $=jQuery;
	var p={},pub={};
	/**
	 * 滚动指定的tab内容
	 * @param {Object} t
	 */
	p.slide=function(t){
		var loop=function(t){
			var items=t.find(".box");
			if(items.size()<=p._showNum) return;
			var items0=items.eq(0);
			t.append(items0.clone());
			t.animate({"margin-left":"-229px"},2500,function(){
				t.css({"margin-left":"0px"});
				items0.remove();
			});
		};
		loop(t);
		p._intervalID=window.setInterval(function(){
			loop(t);
		},2600);
	};
	/**
	 * 获取tab内容
	 * @param {Object} evt
	 */
	p.loadFile=function(evt){
		var target=evt.data.t;
		var _this=$(this);
		//清除timeoutID
		if(p._intervalID){window.clearInterval(p._intervalID);p._intervalID=null;};
		//是否已是加载过的菜单
		if (_this.hasClass("on")) {
			p.slide(target);
			return false;
		};
		//加载文件
		target.load(_this.attr("rel"),function(data,txtStatus,xhr){
			p.slide(target);
		});
		evt.data.subMenus.removeClass("on");
		_this.addClass("on");
		//ul
		var idx=$.inArray(this,evt.data.subMenus);
		evt.data.ulMenu.css({left: {0:250, 1:0, 2:-225}[idx]});
	};
	/**
	 * 为每个tab的菜单、按钮注册事件
	 * @param {Object} tabC
	 */
	p.initTabC=function(tabC){
		var $ulSubMenu=tabC.find(".subMenuBox");
		var $subMenus=tabC.find(".subMenu");
		$subMenus.bind("click",{t:tabC.find(".dataConInner"),subMenus:$subMenus,ulMenu:$ulSubMenu},p.loadFile);
		var lBtn=tabC.find(".lBtn");
		var rBtn=tabC.find(".rBtn");
		//右按钮
		rBtn.click(function(evt){
			var prev=$subMenus.filter(".on").prev();
			if(prev.size()==0){
				return false;
			};
			if(prev.prev().size()==0){
				rBtn.addClass("rabled");
			};
			
			prev.trigger("click");
			lBtn.removeClass("labled");
			/*
			//ul
			var idx=$.inArray(prev[0],$subMenus);
			$ulSubMenu.css({left: {0:250, 1:0, 2:-225}[idx]});
			*/
			return false;
		});
		//左按钮
		lBtn.click(function(evt){
			var next=$subMenus.filter(".on").next();
			if(next.size()==0){
				return false;
			};
			if(next.next().size()==0){
				lBtn.addClass("rabled");
			};
			next.trigger("click");
			rBtn.removeClass("labled");
			/*
			//ul
			var idx=$.inArray(next[0],$subMenus);
			$ulSubMenu.css({left: {0:250, 1:0, 2:-225}[idx]});
			*/
			return false;
		});
		
	};
	/**
	 * 国内上映、国外院线的切换
	 * @param {Object} evt
	 */
	p.showTab=function(evt){
		var _this=$(this);
		var id=_this.find("img").attr("id");
		var i=id.lastIndexOf("_");
		i=parseInt(id.substr(i+1));
		var imgTmp="images/m"+i+"2.gif";
		p._$tabC.hide().eq(i-1).show();
		//图片切换
		p._$tabs.each(function(j,o){
			var imgTmp0="images/m"+(j+1)+"1.gif";
			$(o).find("img").attr("src",imgTmp0);
		});
		_this.find("img").attr("src",imgTmp);
		
		//init subtab
		p._$tabC.eq(i-1).find(".subMenu").eq(p._initSubTabIndex).trigger("click");
		return false;		
	};
    //private area
    p.initVar = function(opts) {
		p._initSubTabIndex=opts.initSubTabIndex||1; 
		p._$tabs=$("#contentC .menuA li");
		p._$tabC=$("#contentC .tab");
		p._showNum=opts.showNum||3;
		p._intervalID=null;
	};
    p.onLoaded = function() { 
		p._$tabs.eq(0).trigger("mouseover");
	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		p._$tabs.bind("mouseover",p.showTab);
		p._$tabC.each(function(i,obj){
			p.initTabC($(obj));
		});
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 