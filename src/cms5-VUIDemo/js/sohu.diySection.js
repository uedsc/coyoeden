/**
 * 类-分栏,通过diyEditor添加分栏时产生
 * @author levinhuang
 * @param {Object} opts 选项对象
 */
sohu.diySection = function(opts) {
	var _this=this;
	opts=$.extend({},{
		clSecSub:"subsec",
		limitSec:190,clSec:"sec",clSecOn:"secOn",
		clHasSub:"hasSub",
		clSecRoot:"col",
		clArea:"area",
		clContent:"ct",
		clTip:"secTip",
		pSec:null
		},opts);
	var p={opts:opts};
	this.__p=p;
	//属性
	this.ID="sec_"+StringUtils.RdStr(8);
	this.$Layout=opts.$obj;
	this.$Holder=this.$Layout.children(".secHolder");
	this.Width=this.Size();
	this.Divisible=(this.Width>=190);		/* 可继续分栏 */
	this.IsActive=false;
	this.IsAddingContent=false;
	this.InlineEditing=false;
	this.CurArea=opts.curArea;				/* 当前分栏所在的横切。调用LoadCurArea方法时更新该属性 */
	this.Editor=sohu.diyConsole.SecEditor;
	this.PSec=opts.pSec||null;							/* parent section */

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
	this.$Layout.attr("id",this.ID).mouseenter(p.mouseOver);//.mouseleave(p.mouseOut);
	//获取当前分栏的内容
	this.LoadContents();
	//获取当前横切
	//this.LoadCurArea();
	//排序事件处理
	this.$Layout.sortable({
		items:">.ct",
		connectWith:".sec",
		placeholder:"ui-hl",
		handle:".dragHandle",
		receive:function(evt,ui){
			sohu.diyConsole.Dragger.obj.Sec.RemoveCTByID(sohu.diyConsole.Dragger.obj.ID);
			sohu.diyConsole.Dragger.obj.Sec=_this;
			_this.Contents.push(sohu.diyConsole.Dragger.obj);
		}
	});
	/*  */
	//自定义事件
	this.$Layout.bind("evtActive",function(e){
		_this.Active();
		return false;//停止冒泡
	});
}; 
/**
 * 激活分栏
 */
sohu.diySection.prototype.Active=function(){
	if(sohu.diyConsole.CurSec&&(sohu.diyConsole.CurSec.IsAddingContent||sohu.diyConsole.CurSec.InlineEditing)) return;
	if(this.IsActive) return;
	//看看当前横切是否激活，没激活的话激活
	if(sohu.diyConsole.CurArea&&sohu.diyConsole.CurArea.IsActive){
		sohu.diyConsole.CurArea.Deactive();
	};
	this.CurArea.Active();
	/* 反激活上一个分栏 */
	if(sohu.diyConsole.CurSec){sohu.diyConsole.CurSec.Deactive();};
	this.IsActive=true;
	this.$Layout.addClass(this.__p.opts.clSecOn);
	//Update sohu.diyConsole.CurSec
	sohu.diyConsole.CurSec=this;
	//Show the toolbar
	this.Editor.Show();
};
sohu.diySection.prototype.Deactive=function(){
	if((!this.IsActive)||this.IsAddingContent||this.InlineEditing) return;/* 正在编辑内容 */
	this.Editor.Hide();
	this.IsActive=false;
	this.$Layout.removeClass(this.__p.opts.clSecOn);
};
/**
 * 添加子分栏
 */
sohu.diySection.prototype.AddSub=function($secSub){
	var _this=this;
	this.Editor.UpdateCT({$Layout: $secSub},1);
	var subSecs=$secSub.find("."+this.__p.opts.clSec);
	this.$Layout.addClass(this.__p.opts.clHasSub);
	subSecs.each(function(i,sec){
		sohu.diySection.New({
			$obj:$(sec),
			curArea:_this.CurArea,
			pSec:_this
		});
	});
};
/**
 * 添加内容-在分栏的末尾添加内容
 * @param {Object} ct 待添加内容对象。如{html0:'xx',flash:false,type:'pp'}
 */
sohu.diySection.prototype.AddContent=function(ct){
	var _this=this;
	if(ct.isNew){
		/* 新增 */
		//创建相应的diyContent实体
		ct=sohu.diyContent.New({sec:this,ct:ct});
		if(!ct.Validation.valid){
			alert(ct.Validation.msg);
			return;
		};
		this.Editor.UpdateCT(ct,1);
		this.Contents.push(ct);
	}else{
		/* 更新 */
		var ct0=this.GetCTByID(ct.attr("id"));
		if(!ct0) return;
		if(ct.html0==""){
			if (window.confirm("HTML内容为空或者不符合模板规范,是否确定删除原内容?")) {
				ct0.$Layout.remove();
			};
		}else{
			ct0.$Layout.html(ct.html0);
		};//if1	
	};//if0
};
/**
 * 清楚分栏内容
 */
sohu.diySection.prototype.Cls=function(){
	//内容为空，删除该分栏和同级分栏
	if(this.IsEmpty()){
		var p=this.$Layout.closest("."+this.__p.opts.clSecSub);
		if(p.length>0){
			p.remove();
		};
		//循环看父级分栏是否为空，为空则移除hasSub类
		var me=this;
		while(me&&me.PSec&&me.PSec.IsEmpty()){
			me.PSec.SetAsEmpty();
			me=me.PSec.PSec;
		};
		return;
	};
	//内容不为空则删除内容和子分栏
	//删除内容
	this.$Layout.children(":not(.secHolder)").remove();
	this.SetAsEmpty();
	this.Contents=[];
};
/**
 * 根据内容id获取内容对象
 * @param {String} ctID
 * @return {diyContent}
 */
sohu.diySection.prototype.GetCTByID=function(ctID){
	if(this.Contents.length==0) return null;
	var ct=null;
	$.each(this.Contents,function(i,o){
		if(o.ID==ctID){
			ct=o;return false;
		};
	});
	return ct;
};
/**
 * 根据内容id从内容列表中移除内容对象。注意,非从dom中移除。此方法用在排序时後
 * @param {String} ctID 内容ID
 * @return {diyContent} 被移除的对象
 */
sohu.diySection.prototype.RemoveCTByID=function(ctID){
	if(this.Contents.length==0) return null;
	var ct=null;
	this.Contents=$.grep(this.Contents,function(o,i){
		if(o.ID==ctID){
			ct=o;return false;
		}
		return true;
	});
	return ct;
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
 * 是否有子分栏
 */
sohu.diySection.prototype.HasSub=function(){
	var cnt=this.$Layout.find("."+this.__p.opts.clSecSub).size();
	return (cnt>0);
};
/**
 * 是否有父级分栏
 */
sohu.diySection.prototype.HasParent=function(){
	var cnt=this.$Layout.parents("."+this.__p.opts.clSec).size();
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
		h:this.$Layout.height(),
		mw:this.Size()
	};
};
/**
 * 获取该分栏的内容
 */
sohu.diySection.prototype.LoadContents=function(){
	var _this=this;
	var items=this.$Layout.find("."+this.__p.opts.clContent);
	items=items.map(function(i,ctdom){
		var $dom=$(ctdom);
		var ct={
			type:sohu.diyConsole.GetClassName($dom),
			$dom:$dom
		};
		if(ct.type=="flash"){
			ct.flash=true;
		};
		return sohu.diyContent.New({
			ct:ct,
			sec:_this,
			isNew:false
		});
	});
	this.Contents=items;
	if(items.length>0){
		this.CurArea.$Layout.removeClass("area_empty");
	};
	//return items;
};
/**
 * 激活父级分栏
 */
sohu.diySection.prototype.ActiveParent=function(){
	//var $psec=this.$Layout.parents("."+this.__p.opts.clSec);//.closest("."+this.__p.opts.clSec);//this.$Layout.parents("."+this.__p.opts.clSec+":last");
	if(this.PSec)
		this.PSec.$Layout.trigger("evtActive");
};
/**
 * 当前分栏是否为空
 */
sohu.diySection.prototype.IsEmpty=function(){
	return (this.$Layout.children(":not(.secHolder)").length==0);
};
/**
 * 设置当前分栏是空分栏
 * @param {Function} cbk callback function
 */
sohu.diySection.prototype.SetAsEmpty=function(cbk){
	this.$Layout.removeClass("hasSub");
	if(cbk){
		cbk(this);
	};
};
/**
 * switch the overlay of the section
 * @param {Object} mode possible values may be "on" or "off"
 */
sohu.diySection.prototype.Overlay=function(mode){
	mode=mode||"on";
	mode=="on"?mode:"off";
	if(mode=="on"){
		var css={
			display:"block",
			border:"1px solid red",
			background:"#f9bfc0",
			position:"absolute",
			top:0,
			left:0,
			height:this.$Layout.height(),
			width:"100%",
			"z-index":50,
			opacity:0.7
		};
		this.$Holder.css(css);
	}else{
		this.$Holder.attr("style","");
	};
};
/*静态方法*/
/**
 * 从现有的dom元素新建一个diySection对象
 * @param {Object} opts 
 */
sohu.diySection.New=function(opts){
	return new sohu.diySection(opts);
};

