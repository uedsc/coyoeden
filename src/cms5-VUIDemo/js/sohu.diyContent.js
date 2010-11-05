/**
 * 类-内容、碎片
 * @author levinhuang
 * @param {Object} opts 选项{$obj,sec}
 */
sohu.diyContent=function(opts){
	opts=$.extend({},{cl:"vstp_ct",clOn:"vstp_ctOn",scale:false,clElm:"vstp_elm",clElmOn:"vstp_elmOn",isNew:true,addingMode:0},opts||{});
	var _this=this;
	this.Meta=opts.ct;
	this.IsNew=opts.isNew;
	this.$Layout=null;							/*在Validate方法中构建*/
	this.Type=opts.ct.type;						/* 内容插件类型 */
	this.Sec=opts.sec;							/* 分栏 */
	this.Editor=this.Sec.Editor;				/* 分栏编辑器 */
	this.CTEditor=sohu.diyConsole.CTEditor;		/* 内容编辑器 */
	this.MaxWidth=this.Sec.Width;				/* 最大宽度 */
	this.onDomed=null;							/* 被添加到dom树后的回调函数 */
	this.IsFlash=false;							/* 是否flash焦点图内容 */
	this.FlashObj=null;							/* flash对象 */
	this.IsEditing=false;						/* 是否处于编辑状态 */
	this.IsActive=false;						/* 是否处于选中状态 */
	this._timerBlink=null;						/* 闪烁计时器 */
	this.AddingMode=opts.addingMode;			/* 0为当前分栏添加的内容；1为当前碎片下方添加的内容 */							
	
	//private property
	var p={opts:opts};
	this.__p=p;
	
	/* 验证内容的有效性 并且构建$Layout*/
	this.Validate();
	if(!this.Validation.valid) return;
	
	/* ID */
	if ((this.ID = this.$Layout.attr("id")) == "") {
		this.ID = "ct_" + this.Type + "_" + StringUtils.RdStr(8);
		this.$Layout.attr("id",this.ID);
	};
	
	//是否flash
	this.IsFlash=this.$Layout.flash;
	if(this.IsFlash){
		if(this.IsNew){
			this.onDomed=function(mode){
				this.FlashObj=window["F_"+this.FlashData.pid];
			};	
		}else{
			this.FlashObj=window["F_"+this.FlashData.pid];
		};
		
	};
	/* Persistence to the dom tree */
	if (this.IsNew) {
		if(this.AddingMode==0)
			this.Editor.UpdateCT(this, 1);
		else
			this.CTEditor.UpdateCT(this,1);
	};
	/* Load elements */
	this.LoadElements();
	
	this.BindEvts();
};
/**
 * 获取内容的维度信息
 * @return {Object} {x,y,w,h}
 */
sohu.diyContent.prototype.Dim=function(){
	return {
		x:this.$Layout.offset().left,
		y:this.$Layout.offset().top,
		w:this.$Layout.width(),
		h:this.$Layout.height(),
		w1:this.$Layout.outerWidth(),
		h1:this.$Layout.outerHeight()
	};
};
/**
 * 利用html编辑器编辑内容
 */
sohu.diyContent.prototype.DoEdit=function(){
	this.Editor.DialogCT("update");
};
/**
 * 编辑焦点图内容
 */
sohu.diyContent.prototype.EditFlash=function(){
	sohu.diyDialog.Show('wFlash');
};
/**
 * Notice related objects that current element is being edited.
 * TODO:Use event model to implement this,let related objects register the inline editing event.
 * @param {Object} state
 */
sohu.diyContent.prototype.InlineEdit=function(state){
	if(state=="on"){
		this.Sec.Active();
		this.Sec.InlineEditing=true;
		this.IsEditing=true;
		sohu.diyConsole.EditingSec=this.Sec;
		sohu.diyConsole.EditingCT=this;
	}else{
		this.Sec.Deactive();
		this.Sec.InlineEditing=false;
		this.IsEditing=false;
		//this.$Layout.trigger("mouseleave");
		sohu.diyConsole.EditingSec=null;
		sohu.diyConsole.EditingCT=null;
	}
};
/**
 * 验证当前内容是否有效
 */
sohu.diyContent.prototype.Validate=function(){
	var _this=this;
	this.SetValidation(true);
	
	var commonValidate=function(ct,msg){
		if(_this.IsNew){
			ct.$Layout=$(ct.Meta.html0).filter("."+ct.Type);
		}else{
			ct.$Layout=ct.Meta.$dom;
		};
		
		if(ct.$Layout.length==0||(!ct.$Layout.is("."+ct.__p.opts.cl))){
			ct.SetValidation(false,msg);
		};
	};
	
	switch(this.Type){
		case "ohmygod":
			this.SetValidation(false,"Html内容不符合可视化专题模板规范");
		break;
		case "shtable":
			this.$Layout=$(this.Meta.html0).filter("table");
			if(this.$Layout.length==0){
				this.SetValidation(false,"Html内容无表格标签");
			}else{
				//将$Layout替换成符合diy内容模板规范的对象
				this.$Layout=$("<div/>").addClass(this.__p.opts.cl+" "+this.Type+" clear").append(this.$Layout);
			}
		break;
		case "vstp_flash":
			this.$Layout=this.IsNew?$(this.Meta.html0):this.Meta.$dom;
			this.$FlashData=this.$Layout.find(".vstp_flashData");
			this.FlashData=$.evalJSON(this.$FlashData.html());
		break;
		default:
			commonValidate(this,"Html内容不符合模板规范");
		break;
	};
	if(this.Validation.valid){
		$.extend(this.$Layout,this.Meta);
	};
	return this;
};
/**
 * 设置验证信息
 * @param {Boolean} isValid 是否有效
 * @param {String} 验证结果提示信息
 */
sohu.diyContent.prototype.SetValidation=function(isValid,msg){
	this.Validation={
		valid:isValid,
		msg:msg||null
	};
	return this;
};
/**
 * 加载当前内容的元素
 */
sohu.diyContent.prototype.LoadElements=function(){
	var _this=this;
	//a标签的事件注册
	this.$Layout.find("a").bind("click.edit",function(e){
		//显示编辑器
		_this.ShowChipEditor($(this));
		return false;
	}).end()
		.find("."+this.__p.opts.clElm).unbind(".edit")//其他手工加vstp_elm的元素
		.bind("click.edit",function(e){
			_this.ShowChipEditor($(this));
			return false;
	});	
	return this;
	/*
	var _this=this;
	var items=this.$Layout.find("."+this.__p.opts.clElm);
	items.each(function(i,o){
		new sohu.diyElement({
			$dom:$(o),
			ct:_this
		});
	});
	return this;
	*/
};
/**
 * unload event handlers for the elements
 */
sohu.diyContent.prototype.UnloadElements=function(){
	this.$Layout.find("a").unbind(".edit").end()
		.find("."+this.__p.opts.clElm).unbind(".edit");
		
	return this;
};
/**
 * 移除可视化编辑注册的事件
 */
sohu.diyContent.prototype.UnbindEvts=function(){
	//移除内容事件
	this.$Layout.unbind(".edit");
	//移除元素事件
	//this.$Layout.find("."+this.__p.opts.clElm).trigger("evtUnbindEvt");
	return this;
};
/**
 * 绑定可视化编辑的事件
 */
sohu.diyContent.prototype.BindEvts=function(){
	var p={},_this=this;
	p.mouseEnter=function(evt){
		_this.Active();
		return false;//stop bubbling
	};
	p.mouseLeave=function(evt){
		_this.Deactive();
	};
	
	//内容的鼠标事件
	this.$Layout.bind("mouseenter.edit",p.mouseEnter);//.bind("mouseleave.edit",p.mouseLeave);
	//自定义事件
	this.$Layout.unbind("evtBindEvt").bind("evtBindEvt",function(e){
		_this.BindEvts();
		//_this.$Layout.find("."+_this.__p.opts.clElm).trigger("evtBindEvt");
		return false;//停止冒泡
	});
	this.$Layout.bind("evtUnbindEvt.edit",function(e){
		_this.UnbindEvts();
		return false;//停止冒泡
	});
	
	return this;
};
/**
 * 删除碎片内容
 */
sohu.diyContent.prototype.Cls=function(){
	this.Sec.RemoveCTByID(this.ID);
	this.$Layout.remove();
};
/**
 * highlight current content
 * @param {Object} opts {color:'red',speed:2000}
 */
sohu.diyContent.prototype.Blink=function(opts){
	clearInterval(this._timerBlink);
	if(arguments.length==1&&opts==false){return this;};
	opts=$.extend({color:'red',speed:2000},opts||{});
	var _i=this,b=function(){
		_i.$Layout.effect("highlight",{color:opts.color},opts.speed/2);
	};
	this._timerBlink=window.setInterval(b,opts.speed);
	b();
	return this;
};
/**
 * active current content
 */
sohu.diyContent.prototype.Active=function(force){
	force=force||false;
	var _i=this;
	//已经选中
	if(this.IsActive) return this;
	//如果自己处于编辑内容状态，则不显示编辑器
	if(this.IsEditing) return this;
	//如果处于拖拽状态 ，则不显示编辑器
	if(sohu.diyConsole.Dragger.ing) return this;
	//别人处于编辑状态，则不显示编辑器
	if(sohu.diyConsole.CurCT&&sohu.diyConsole.CurCT.IsEditing&&(!force)) return this;
	//反激活上一个内容对象
	if(sohu.diyConsole.CurCT){
		sohu.diyConsole.CurCT.Deactive();
	};
	
	//激活相关分栏对象
	if(force)
		this.Sec.Active(true);
	
	this.IsActive=true;
	this.Editor.CurCT=this;
	sohu.diyConsole.CurCT=this;
	//_this.ToggleDragger("on");
	this.CTEditor.AttachTo(this).Show();
	//Flash焦点图
	if(this.IsFlash){
		var d=this.Dim();
		sohu.diyConsole.$FlashHolder.CT=this;
		sohu.diyConsole.$FlashHolder.css({
			top:d.y-1,
			left:d.x-1,
			height:d.h+1,
			width:d.w+1,
			opacity:0.5,
			display:'block'
		}).unbind("click").bind("click",function(evt){
			_i.InlineEdit("on");
			sohu.diyConsole.CurCT=_i;
			_i.EditFlash();
		});
	};
	return this;	
};
/**
 * Deactive current content obj
 */
sohu.diyContent.prototype.Deactive=function(){
	this.Blink(false);
	this.IsActive=false;
	sohu.diyConsole.CTEditor.Detach();
	sohu.diyConsole.CurCT=null;
	return this;
};
/**
 * 添加内容-在内容的末尾添加内容
 * @param {Object} ct 待添加内容对象。如{html0:'xx',flash:false,type:'pp'}
 */
sohu.diyContent.prototype.AddContent=function(ct){
	var _this=this;
	if(ct.isNew){
		/* 新增 */
		//创建相应的diyContent实体
		ct=sohu.diyContent.New({sec:this.Sec,ct:ct,addingMode:1});
		if(!ct.Validation.valid){
			alert(ct.Validation.msg);
			return;
		};
		this.Sec.Contents.push(ct);
	}else{
		/* 更新 */
		var ct0=this.Sec.GetCTByID(ct.attr("id"));
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
 * 显示碎片编辑器
 * @param {Object} $t 当前a标签或者具有vstp_elm类的元素
 */
sohu.diyContent.prototype.ShowChipEditor=function($t){
	if($t.hasClass(this.__p.opts.clElmOn)) return this;
	
	var _this=this;
	sohu.diyChipEditor.Show({
		ct:this,
		tabs:[0],
		$elm:$t,
		onSave:function(dlg){
			dlg.Hide();
		},
		afterShow:function(hash,dlg){
			//是否隐藏元素的“增加、删除、上移、下移”按钮
			dlg.$ElmcActs.show();
			dlg.$Elm.addClass(_this.__p.opts.clElmOn);
			//确认激活内容对象
			dlg.CT.Active(true);
			dlg.CT.InlineEdit("on");			
		},
		afterHide:function(hash,dlg){		
			dlg.CT.InlineEdit("off");			
		}
	});
};
/*静态方法*/
/**
 * 从现有的dom元素构建一个diyContent 对象
 * @param {Object} opts 选项 {sec,ct}
 */
sohu.diyContent.New=function(opts){
	return new sohu.diyContent(opts);
};
