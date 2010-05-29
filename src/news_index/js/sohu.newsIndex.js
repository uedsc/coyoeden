/*
Javascript Module Pattern 模板v1.0
Author:Levin Van
Last Modified On 2010.05.25
此模板用于js客户端开发,发布时别忘压缩js以便去掉模板中的备注
*/
var this$ = function() {
	var p={},pub={};
    /*private area*/
	
	//tabHeadlines
	p.initTabHL=function(){
		$("#tabHL0 ul.secHead").imgNav({mode:'hover',navc:"#tabC01,#tabC02"});	
		$("#tabHL1 ul.secHead").imgNav({mode:'hover',navc:"#tabC03,#tabC04"});
	};
	//fancyHL
	p.initFancyHL=function(){
		var items=$("#fancyHL .secBody");
		var effect0=function(i){
			items.removeClass("on");
			i.addClass("on");
		};
		items.mouseover(function(evt){
			var i=$(this);
			if(i.hasClass("on")) return false;
			effect0(i);		
		});
	};
	//fancyPics
	p.initFancyPics=function(){
		var t=$("#fancyPics_S");
		//放大图x坐标值的计算公式：(i-1)×小图大小含留白边距69-放大图右边距10+放大图大小的一半84/2-箭头图标宽度一半9+调整值5
		t.imgNav({mode:'hover',navc:"#fancyPics_B li",callback:function(opts){
			t.find("li").removeClass("on");	
			opts._i.parent().addClass("on");
			//箭头位置
			var l=opts._index*69-10+84/2-9+5;
			$("#fancyPics .arrow").stop(true,true).animate({left:l+'px'},"normal");
		}});	
	};
	
	/*
	initVar方法
	作用：用于引用重复使用的dom元素或引用服务器端生成到页面的js变量
	*/
    p.initVar = function(opts) { 
	};
	/*
	onLoaded方法
	作用:统一管理页面加载完毕后的回调方法
	说明:onLoaded方法接管所有页面上注册到$(document).ready(callback)中的callback方法;
		如果你要新增一个$(callback)或$(document).ready,请将你的callback方法放在onLoaded方法体内
	*/
    p.onLoaded = function() { 
		p.initTabHL();
		p.initFancyHL();
		p.initFancyPics();
	};
	/*
	initEvents方法
	作用:用于为页面dom元素注册各种事件!
	说明:Html页面仅用于表现，任何时候应在标签里面直接注册事件。即避免如<a onclick="xx"/>
	*/
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		
    };
	
	/*/private area*/
	
    /*public area*/
	
    /*
	Init方法
	作用:页面js逻辑的唯一入口
	说明：理想状态下每个页面对应一个交互用的js文件，在页面末尾通过下面代码初始化js交互逻辑
	<script type="text/javascript">
	//<![CDATA[
	this$.Init({x:'kk',y:'zz'});
	//]]>
	</script> 
	*/
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
	
	/*/public area*/
    return pub;
} (); 