/**
 * 类-分栏,通过diyEditor添加分栏时产生
 * @author levinhuang
 * @param {Object} opts 选项对象
 */
sohu.diySection = function(opts) {
	var _this=this;
	opts=$.extend({},{
		cssHelper:".secTip",clSecSub:"subsec",
		limitSec:390,clSec:"sec",clSecOn:"secOn",
		clHasSub:"hasSub",clHelperHasSub:"secTip1",
		clHelperHot:"secTipHot",
		clSecRoot:"col",
		clArea:"area",
		clContent:"ct"
		},opts);
	var p={opts:opts};
	this.__p=p;
	//属性
	this.$Layout=opts.$obj;
	this.Width=this.Size();
	this.Divisible=(this.Width>=390);//可继续分栏
	this.$Helper=opts.secHelper.clone(false)
		.attr("style","")
		.removeClass(opts.clHelperHasSub+" "+opts.clHelperHot)
		.html("w:"+this.Width+"px");
	this.IsActive=false;
	this.IsAddingContent=false;
	this.Editor=opts.editor;
	this.CurArea=opts.curArea;//当前分栏所在的横切。调用LoadCurArea方法时更新该属性
	this.AttachHelper();
	
	var p={};
	p.mouseOver=function(evt){
		if(_this.HasSub()) return false;
		_this.Active();
		//return false;
	};
	p.mouseOut=function(evt){
		_this.Deactive();
		return false;
	};
	//鼠标事件-！！停止冒泡事件
	/*
	this.$Layout.hover(p.mouseOver,p.mouseOut);
	*/
	this.$Layout.mouseenter(p.mouseOver).mouseleave(p.mouseOut);
	//$Helper事件
	this.$Helper.click(function(evt){
		_this.Active();
	}).hover(function(evt){
		if(!_this.$Helper.hasClass(opts.clHelperHasSub)) return;
		_this.$Helper.switchClass(opts.clHelperHasSub,opts.clHelperHot);
	},function(evt){
		if(!_this.$Helper.hasClass(opts.clHelperHot)) return;
		_this.$Helper.switchClass(opts.clHelperHot,opts.clHelperHasSub);
	});
	//获取当前分栏的内容
	this.Contents=this.LoadContents();
	//获取当前横切
	//this.LoadCurArea();
	//排序事件处理
	this.$Layout.sortable({
		items:".ct",
		connectWith:".sec",
		placeholder:"ui-hl",
		handle:".dragHandle",
		receive:function(evt,ui){
			//console.log("receive!");
			sohu.diyConsole.Dragger.obj.Sec=_this;
			
		}
	});
}; 
/**
 * 激活分栏
 */
sohu.diySection.prototype.Active=function(){
	if(this.IsActive) return;
	//如果页面在拖拽则屏蔽UI
	if(sohu.diyConsole.Dragger.ing) return;
	this.Editor.AttachTo(this).Show();
	this.IsActive=true;
	this.$Layout.addClass(this.__p.opts.clSecOn);
	//看看当前横切是否激活，没激活的话激活
	if(!this.CurArea.IsActive){
		this.CurArea.Active();
	};
};
sohu.diySection.prototype.Deactive=function(){
	//如果页面在拖拽则屏蔽UI
	if(sohu.diyConsole.Dragger.ing) return;
	//test
	return;
	this.Editor.Remove();
	this.IsActive=false;
	this.$Layout.removeClass(this.__p.opts.clSecOn);
};
/**
 * 添加子分栏
 */
sohu.diySection.prototype.AddSub=function($secSub){
	var _this=this;
	var subSecs=$secSub.find("."+this.__p.opts.clSec);
	subSecs.each(function(i,sec){
		sohu.diySection.New({
			$obj:$(sec),
			editor:_this.Editor,
			secHelper:_this.$Helper,
			curArea:_this.CurArea
		});
	});
	this.Editor.UpdateCT($secSub.effect("highlight"),1);
	this.$Layout.addClass(this.__p.opts.clHasSub);
	this.$Helper.addClass(this.__p.opts.clHelperHasSub);
};
/**
 * 添加内容-在分栏的末尾添加内容
 * @param {Object} $ct 待添加到内容(jq dom)
 */
sohu.diySection.prototype.AddContent=function($ct){
	this.Editor.UpdateCT($ct.effect("highlight"),1);
	//创建相应的diyContent实体
	var ct=new sohu.diyContent({$obj:$ct,sec:this});
	this.Contents.push(ct);
};
/**
 * 获取当前分栏父分栏的宽度
 */
sohu.diySection.prototype.Size=function(){
	var width=0;
	var classes=this.$Layout.parent().attr("class").split(" ");
	$.each(classes,function(i1,o1){
		if(o1.indexOf("w")==0){
			width=parseInt(o1.substr(1));
			return false;
		};
	});//each
	return width;
};
/**
 * 附加分栏助手dom
 */
sohu.diySection.prototype.AttachHelper=function(){
	if(!(this.$Layout.children(this.__p.opts.cssHelper).size()>0)){
		this.$Layout.prepend(this.$Helper);
	};
};
/**
 * 移除分栏助手dom
 */
sohu.diySection.prototype.RemoveHelper=function(){
	this.$Layout.children(this.__p.opts.cssHelper).remove();
};
/**
 * 是否有子分栏
 */
sohu.diySection.prototype.HasSub=function(){
	var cnt=this.$Layout.find("."+this.__p.opts.clSecSub).size();
	return (cnt>0);
};
/**
 * 获取当前分栏所属的横切对象
 */
sohu.diySection.prototype.LoadCurArea=function(){
	var _this=this;
	if(this.CurArea!=null) return;
	var areaID=this.$Layout.parents("."+this.__p.opts.clArea).attr("id");
	$.each(this.Editor.Console.Areas,function(i,o){
		if(o.ID==areaID){
			_this.CurArea=o;
			return false;
		};
	});
};
/**
 * 返回当前分栏相当于window的x和y值,以及自身的高和宽
 * @return {Object} {x,y,w,h}
 */
sohu.diySection.prototype.Dim=function(){
	return {
		x:this.$Layout.offset().left,
		y:this.$Layout.offset().top,
		w:this.$Layout.width(),
		h:this.$Layout.height()
	};
};
/**
 * 获取该分栏的内容
 */
sohu.diySection.prototype.LoadContents=function(){
	var _this=this;
	var items=this.$Layout.find("."+this.__p.opts.clContent);
	items=items.map(function(i,ct){
		return sohu.diyContent.New({
			$obj:$(ct),
			sec:_this
		});
	});
	return items;
};
/*静态方法*/
/**
 * 从现有的dom元素新建一个diySection对象
 * @param {Object} opts 
 */
sohu.diySection.New=function(opts){
	return new sohu.diySection(opts);
};

