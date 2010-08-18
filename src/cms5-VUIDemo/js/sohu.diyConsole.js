/**
 * 可视化编辑控制台
 * @author levinhuang
 * @param {Object} opts 选项,如clSec:"sec",cssWsp:"#main"表示分栏的css类为sec,工作区域的css选择器为#main
 * @dependency sohu.diyEditor.js;sohu.diyArea.js
 * TODO:将顶部菜单部分逻辑移到单独的js文件diyMenuBar.js
 */
sohu.diyConsole=function(opts){
	//属性
	opts=$.extend({},{
		cssWsp:"#main",clSec:"sec",clSec0:"sec0",clSecSub:"subsec",
		clSecRoot:"col",clArea:"area",cssArea:".area",dfTop:100,
		clAreaStatic:"static",
		limitSec:390,
		scrollWrapMainginTop:0
		},opts);
	var _this=this;
	this.$Workspace=$(opts.cssWsp);
	this.$Layout=$("#areaTools");

	this.Areas=null;
	
	var p={opts:opts};
	p._$pageTip=$("#pageTip");
	p._$elmTool=$("#elmTool");
	/* 对话框jq对象 */
	p._$wAreaBG=$("#wAreaBG")
	p._$wPageBG=$("#wPageBG");
	p._$wCpkWrap=$("#cpkWrap");
	p._$wAddLink=$("#addLink");
	p._$wSecHead=$("#cfgSecHead");
	p._$wSec=$("#wCfgSec");
	p._$wCode=$("#wCode");
	/* /对话框jq对象 */

	p._$txtFontColor=$("#txtFontColor");
	p._opts=opts;
	/* =/顶部交互菜单= */
	
	p.getWorkspaceBoundary=function(){
		var lastArea=_this.$Workspace.find(opts.cssArea+":last");
		if (lastArea.size() == 0) {
			return {
				lbleft: -2000,
				ubleft: 2000,
				lbtop: -2000,
				ubtop: 2000
			};
		};
		
		var lbtop=_this.$Workspace.offset().top;
		var ubtop=lastArea.height()+lastArea.offset().top;
		var lbleft=lastArea.offset().left;
		var ubleft=lastArea.width()+lbleft;
		
		return {
			lbleft:lbleft,
			ubleft:ubleft,
			lbtop:lbtop,
			ubtop:ubtop
		};
	};
	//body标签的鼠标事件
	p.onMousemove=function(evt){
		if(!sohu.diyConsole.CurArea) return;
		if(!sohu.diyConsole.CurArea.IsActive) return;
		if(sohu.diyConsole.CurArea.IsEditing) return;
		if(sohu.diyConsole.Dragger.ing) return;
		//if(sohu.diyConsole.EditingSec!=null) return;
		
		var b=p.getWorkspaceBoundary();
		
		if(evt.pageX<b.lbleft||evt.pageX>b.ubleft||evt.pageY>b.ubtop){//||evt.pageY<lbtop
			//反激活横切
			sohu.diyConsole.CurArea.Deactive();
			//反激活分栏
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Deactive();
				
			//移除拖拽助手和内容蒙层
			sohu.diyConsole.Dragger.handle.hide();
			sohu.diyConsole.$EHolder.hide();	
		};
	};
	p.onBodyClick=function(evt){
		//用户是否点击#editMenu
		if($(evt.target).parents(".jqmWindow").length>0) return;
		
		var b=p.getWorkspaceBoundary();
		if(evt.pageX<b.lbleft||evt.pageX>b.ubleft||evt.pageY>b.ubtop){//||evt.pageY<lbtop
			//sohu.diyDialog.Hide(true);
			//sohu.diyConsole.Preview();			
		};
		return false;
	};
	p.setDocumentDim=function(){
		return;
		var fullheight, height;
		fullheight = sohu.diyConsole.InnerHeight();        
		height = fullheight - p.opts.scrollWrapMainginTop;
		
		sohu.diyConsole.$ScrollWrap.css("height",height);
		_this.$Workspace.css("minHeight",height);
	};
	p.onLoaded=function(){
		//文档高度适应处理
		p.setDocumentDim();
		//横切工具条位置
		_this.$Layout.css({"top":p.opts.scrollWrapMainginTop+30});
	};
	/**
	 * Save current document.selection to sohu.diyConsole.DocSelection
	 */
	p.saveSelection=function(){
		if(sohu.diyConsole.CurElm.InlineEditable){
			sohu.diySelection.snap(sohu.diyConsole.CurElm.i$frame[0].iDoc());
		}else{
			sohu.diySelection.snap(document,sohu.diyConsole.CurElm.$Layout[0]);
		};
	};
	p.onEHolderClick=function(evt){
		if((!sohu.diyConsole.$EHolder)||(!sohu.diyConsole.$EHolder.t)) return false;
		sohu.diyConsole.$EHolder.t.ForceEdit();
		return false; 
	};
	p.Init=function(){
		//公有属性引用
		sohu.diyConsole.$WinSec=p._$wSec;
		sohu.diyConsole.$WinCode=p._$wCode;
		//sohu.diyConsole.$WinPageBG=p._$wPageBG;
		sohu.diyConsole.$SecEditorModel=$("#area_editor");
		sohu.diyConsole.$ScrollWrap=$("#scrollWrap");
		sohu.diyConsole.$BodyBGA=$("#main .bodyBGA");
		sohu.diyConsole.$BodyBGB=$("#main .bodyBGB");
		sohu.diyConsole.$ifEditor=$("#ifEditor").iframeEX();	
		sohu.diyConsole.SecEditor=new sohu.diyEditor({bos:_this});
		sohu.diyConsole.$EHolder=$('#eHolder').click(p.onEHolderClick);	
		sohu.diyConsole.$AreaHolder=$("#areaHolder");
		//已有横切
		_this.Areas=_this.AreaList().map(function(i,o){
			var a=new sohu.diyArea({
				isNew:false,
				console:_this,
				onRemove:p.onAreaRemove,
				obj:$(o)
			});
			return a;
		});
		//body鼠标事件
		sohu.diyConsole.$Body=$("body").mousemove(p.onMousemove).click(p.onBodyClick);
		//window resize事件
		sohu.diyConsole.$Window=$(window).resize(function(evt){
			p.setDocumentDim();
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Editor.Reposition();
		});
		new sohu.diyMenuBar({});
		//弹框组件
		sohu.diyDialog.Init({console:_this,cssDragCTM:'window'});
		//on page loaded
		$(document).ready(p.onLoaded);
	};
	this.__p=p;
	//Init
	p.Init();
};
/**
 * 重定位
 */
sohu.diyConsole.prototype.RePosition=function(){
	sohu.diyDialog.wAreaTool.Reposition();
};
/**
 * 设定激活的横切对象
 * @param {Object} target
 */
sohu.diyConsole.prototype.ActiveArea=function(target){
	//将上一个横切反激活
	if(sohu.diyConsole.CurArea){
		if(target&&(target.ID==sohu.diyConsole.CurArea.ID)) 
			return this;
		
		sohu.diyConsole.CurArea.Deactive();
	};
	//激活当前的横切
	sohu.diyConsole.CurArea=target;
	this.CurArea=target;
	return this;
};
/**
 * 关闭内容选择对话框
 */
sohu.diyConsole.prototype.CloseCTDialog=function(){
	sohu.diyConsole.CurSec.Editor.CloseCTDialog();
};
/**
 * 获取所有横切jquery对象
 */
sohu.diyConsole.prototype.AreaList=function(){
	var _this=this;
	var items= this.$Workspace.find(this.__p.opts.cssArea);
	//剔除channelNav和indexNav等含有static类的横切
	items=$.grep(items,function(o,i){
		if($(o).hasClass(_this.__p.opts.clAreaStatic)) return false;
		return true;
	});
	
	return $(items);
};
/**
 * 弹出一个确认对话框
 * @param {Object} opts
 */
sohu.diyConsole.prototype.Confirm=function(opts){
	var _this=this;
	opts=$.extend({},{
		title:"确认操作?",
		ct:"",
		height:140,
		width:"",
		position:"center",
		resizable:false,
		modal:true,
		yes:null,
		no:null,
		close:null
	},opts);
	
	var dlOpt={
		title:opts.title,
		resizable:opts.resizable,
		height:opts.height,
		width:opts.width,
		modal:opts.modal,
		position:opts.position,
		buttons:{
			"取消":function(){
				if(opts.no){opts.no(this);};
				$(this).dialog("close");
			},
			"确认":function(){
				if(opts.yes){opts.yes(this);};
				$(this).dialog("close");
			}
		},
		close:function(evt,ui){
			_this.__p._$pageTip.removeClass("confirm");
			if(opts.close){
				opts.close(evt,ui);
			};
		}
	};
	this.__p._$pageTip.addClass("confirm").html(opts.ct).dialog(dlOpt);
};
/**
 * 移除.txtLoading
 */
sohu.diyConsole.toggleLoading=function(){
	$(".txtLoading").toggle();
};
/*静态方法、对象*/
sohu.diyConsole.Dragger={
	ing:false,
	obj:null,
	handle:$("#ctHandle"),
	cssHandle:'.dragHandle'
};
sohu.diyConsole.CurArea=null;
sohu.diyConsole.CurSec=null;		/* 当前鼠标所在的分栏 */
sohu.diyConsole.EditingSec=null;	/* 当前内联编辑的分栏 */
sohu.diyConsole.CurCT=null;
sohu.diyConsole.EditingCT=null;
sohu.diyConsole.CurElm=null;/* current editing element */
sohu.diyConsole.$SecEditorModel=null; /* 分栏编辑器dom模型 */
sohu.diyConsole.DocSelection='';
sohu.diyConsole.InnerHeight=function() {
    var x,y;
    if (self.innerHeight) // all except Explorer
    {
		return self.innerHeight;
    }
    else if (document.documentElement && document.documentElement.clientHeight)         
    {
		// Explorer 6 Strict Mode
        return document.documentElement.clientHeight;
    }
    else if (document.body) // other Explorers
    {
        return document.body.clientHeight;
    };

};
/**
 * 根据颜色值获取完整的上、右、下、左四个方向的颜色值
 * @param {Object} c
 */
sohu.diyConsole.GetBorderColor=function(c){
	c=$.trim(c);
	if(c=="") return null;
	//默认情况下用jquery的css方法获取的rgb颜色值rgb(9, 168, 139)中含有空格，需先将这些空格去掉
	var reg=/\b,\s\b/g;
	c=c.replace(reg,",");//将", "替换为","
	var cList=c.split(" "),retVal={};
	$.each(cList,function(i,o){
		if(o.indexOf("#")!=0&&o.indexOf("rgb")!=0)
			cList[i]="none";
			
	});
	switch(cList.length){
		case 0:
			retVal= null;
		break;
		case 1:
			retVal.top=retVal.right=retVal.bottom=retVal.left=cList[0];
		break;
		case 2:
			retVal.top=retVal.bottom=cList[0];
			retVal.left=retVal.right=cList[1];
		break;
		case 3:
			retVal.top=cList[0];
			retVal.left=retVal.right=cList[1];
			retVal.bottom=cList[2];	
		break;
		case 4:
			retVal.top=cList[0];
			retVal.right=cList[1];
			retVal.bottom=cList[2];
			retVal.left=cList[3];
		break;
		default:
			retVal=null;
		break;
	};//switch
	return retVal;
};
/**
 * 获取指定jq dom对象的第idx个css class
 * @param {Object} $dom
 * @param {Object} idx
 */
sohu.diyConsole.GetClassName=function($dom,idx){
	var cl=$.trim($dom.attr("class"));
	if(cl=="") return "";
	
	cl=cl.split(" ");
	cl=$.grep(cl,function(o,i){
		if(o=="") return false;
		return true;
	});
	idx=idx||0;
	if(idx<0) idx=0;
	if(idx>=cl.length) idx-=1;
	return cl[idx];
};
/**
 * 检测字符串是否符合ID规则：字母、数字、下划线
 * @param {Object} str
 */
sohu.diyConsole.IsValidID=function(str){
	if(!StringUtils.isAlphanumeric(str.replace("_",""))) return false;
	return true;
};
/**
 * 预览-退出编辑状态
 */
sohu.diyConsole.Preview=function(){
	if(!sohu.diyConsole.CurArea) return;
	/*
	if(!sohu.diyConsole.CurArea.IsActive) return;
	if(sohu.diyConsole.CurArea.IsEditing) return;
	*/
	if(sohu.diyConsole.Dragger.ing) return;
	//移除分栏编辑器
	if(sohu.diyConsole.CurSec)
		sohu.diyConsole.CurSec.Editor.Editing("off");
	//强制移除内联编辑器
	if(sohu.diyConsole.EditingSec!=null&&sohu.diyConsole.CurElm!=null){
		sohu.diyConsole.CurElm.HideEditor(false);
	};
	//反激活横切
	sohu.diyConsole.CurArea.Deactive();
	//反激活分栏
	if(sohu.diyConsole.CurSec)
		sohu.diyConsole.CurSec.Deactive();
};
/**
 * 元素选择快照
 */
sohu.diyConsole.SnapSelection=function(){
	if(sohu.diyConsole.CurElm.InlineEditable){
		sohu.diySelection.snap(sohu.diyConsole.CurElm.i$frame[0].iDoc());
	}else{
		sohu.diySelection.snap(document,sohu.diyConsole.CurElm.$Layout[0]);
	};
};
/**
 * 解析背景图地址
 * @param {Object} img
 */
sohu.diyConsole.ParseBGImg=function(img){
	img=img=="none"?"":img;
	img=img.replace('url("',"").replace('")',"");
	return img;
};
