/**
 * 可视化编辑控制台
 * @author levinhuang
 * @param {Object} opts 选项,如clSec:"sec",cssWsp:"#main"表示分栏的css类为sec,工作区域的css选择器为#main
 * @dependency sohu.diyEditor.js;sohu.diyArea.js
 */
sohu.diyConsole=function(opts){
	//属性
	opts=$.extend({},{
		cssWsp:"#main",clSec:"sec",clSec0:"sec0",clSecSub:"subsec",
		clSecRoot:"col",clArea:"area",cssArea:".area",dfTop:100,
		limitSec:390
		},opts);
	var _this=this;
	this.$Workspace=$(opts.cssWsp);
	this.$Layout=$("#areaTools");
	this.CurArea=null;//当前激活的横切对象
	this.Editor=new sohu.diyEditor({
		$layoutModel:$("#hiddenTemplate .area_editor"),
		console:_this
	});
	this.Areas=null;
	
	var p={opts:opts};
	p._$btnAdd=$("#lnkAreaAdd");
	p._$btnDel=$("#lnkAreaDel");
	p._$btnBG=$("#lnkAreaBG");
	p._$btnDown=$("#lnkAreaDown");
	p._$btnUp=$("#lnkAreaUp");
	p._$areaSelector=$("#area_selector");
	p._$areaHelper=$("#hiddenTemplate .areaTip")
	p._$secHelper=$("#hiddenTemplate .secTip");
	p._opts=opts;
	
	//横切删除时的回调函数
	p.onAreaRemove=function(area){
		_this.ActiveArea(null);
		_this.$Layout.animate({top:opts.dfTop});	
	};
	
	/**
	 * 显示横切选择器
	 */
	p.showSelector=function(){
		var _onClose=function(evt,ui){
			if(!p._curAreaTpl) return false;//未选中任何分栏
			var obj=new sohu.diyArea({
				tplID:p._curAreaTpl,
				console:_this,
				onRemove:p.onAreaRemove
			});
			_this.Areas.push(obj);
		};
		//显示选择框
		p._$areaSelector.dialog({
			title:"添加横切",
			resizable:false,
			modal:true,
			width:430,
			height:250,
			position:[700,50],
			close:_onClose
			}
		);
	};
	p.onAdd=function(evt){
	///<summary>添加横切</summary>
		p.showSelector();	
		return false;
	};
	p.onSelectAreaTpl=function(evt){
	///<summary>选择某个横切模板后</summary>
		p._curAreaTpl=this.id;p._$areaSelector.dialog("close");	return false;
	};
	p.onRemove=function(evt){
	///<summary>删除横切</summary>	
		if(!_this.CurArea){alert("未选中任何横切!");return false;};
		_this.Areas=$.grep(_this.Areas,function(o,i){
			if(o.ID==_this.CurArea.ID) return false;
			return true;
		});
		_this.CurArea.Remove();
		return false;	
	};
	p.onAddBG=function(evt){
	///<summary>添加横切背景</summary>	
		if(!_this.CurArea){alert("未选中任何横切!");return false;};
		alert("背景");
		return false;
	};
	p.onMove=function(evt){
	///<summary>移动横切</summary>
		if(!_this.CurArea){alert("未选中任何横切!");return false;};	
		var isUp=evt.data.up;
		_this.CurArea.Move(isUp);
		_this.RePosition();
		return false;	
	};
	p.Init=function(){
		//横切选择器
		$("li",p._$areaSelector).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");})
			.click(p.onSelectAreaTpl);
		
		p._$btnAdd.bind("click",p.onAdd);
		p._$btnDel.click(p.onRemove);
		//隐藏删除按钮
		//p._$btnDel.parent().hide();
		p._$btnBG.click(p.onAddBG);
		p._$btnUp.bind("click",{up:true},p.onMove);
		p._$btnDown.bind("click",{up:false},p.onMove);
		
		//已有横切
		_this.Areas=_this.AreaList().map(function(i,o){
			o=new sohu.diyArea({
				isNew:false,
				console:_this,
				onRemove:p.onAreaRemove
			});
			return o;
		});
	};
	this.__p=p;
	//Init
	p.Init();
};
/**
 * 重定位
 */
sohu.diyConsole.prototype.RePosition=function(){
	if(!this.CurArea){this.$Layout.attr("style","");return;};
	this.$Layout.css("top",this.CurArea.$Layout.offset().top);
};
/**
 * 激活指定横切对象
 * @param {Object} target
 */
sohu.diyConsole.prototype.ActiveArea=function(target){
	//将上一个横切凡激活
	if(this.CurArea){
		this.CurArea.Deactive();
	};
	//激活当前的横切
	this.CurArea=target;
	this.Editor.CurArea=target;
	return this;
};
/**
 * 关闭内容选择对话框
 */
sohu.diyConsole.prototype.CloseContentDialog=function(){
	this.areaEditor.CloseContentDialog();
	
};
/**
 * 获取所有横切jquery对象
 */
sohu.diyConsole.prototype.AreaList=function(){
	var items= this.$Workspace.find(this.__p.opts.cssArea);
	return items;
};
/*静态方法-TODO:移到sohu.stringUtils.js中*/
/**
 * 获取指定长度的随机字符串。注意：仅仅由数字和字母组成
 * @param {Object} size 随机字符串的长度
 * @param {Boolean} plusTimeStamp 是否加上当前时间戳
 */
sohu.diyConsole.RdStr=function(size,plusTimeStamp){
	var size0=8;
	var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";
	size=size||size0;size=size<1?size0:size;size=size>chars.length?size0:size;
	var s = '';
	for (var i=0; i<size; i++) {
		var rnum = Math.floor(Math.random() * chars.length);
		s += chars.substring(rnum,rnum+1);
	};
	if(plusTimeStamp){
		s+=new Date().getTime();
	};
	return s;
};
