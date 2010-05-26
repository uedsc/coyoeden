/*
Javascript Module Pattern 模板v1.0
Author:Levin Van
Last Modified On 2010.05.25
此模板用于js客户端开发,发布时别忘压缩js以便去掉模板中的备注
*/
var this$ = function() {
	var p={},pub={};
    /*private area*/
	
	//画图
	p.drawPercentageBar=function(){
		var w=0;
		$.each(p._bars,function(i,obj){
			obj=$(obj);
			w=obj.find(".precent-num").html();
			obj.find(".precent").animate({width:w,"opacity":.7},p._interval,function(){
				$(this).animate({"opacity":1},"fast");
			});						
		});
	};
	/*
	initVar方法
	作用：用于引用重复使用的dom元素或引用服务器端生成到页面的js变量
	*/
    p.initVar = function(opts) { 
		p._interval=opts.interval||'slow';
		p._bars=$(".item");
	};
	/*
	onLoaded方法
	作用:统一管理页面加载完毕后的回调方法
	说明:onLoaded方法接管所有页面上注册到$(document).ready(callback)中的callback方法;
		如果你要新增一个$(callback)或$(document).ready,请将你的callback方法放在onLoaded方法体内
	*/
    p.onLoaded = function() { 
		p.drawPercentageBar();
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
