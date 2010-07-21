/**
 * ��-���ݡ���Ƭ
 * @author levinhuang
 * @param {Object} opts ѡ��{$obj,sec}
 */
sohu.diyContent=function(opts){
	opts=$.extend({},{cl:"ct",clOn:"ctOn",scale:true,clElm:"elm"},opts||{});
	var _this=this;
	this.Meta=opts.ct;
	this.$Layout=null;/*��Validate�����й���*/
	this.Type=opts.ct.type;/* ���ݲ������ */
	this.Sec=opts.sec;//����
	this.Editor=this.Sec.Editor;//�����༭��
	this.MaxWidth=this.Sec.Width;
	this.ID="ct_"+this.Type+"_"+sohu.diyConsole.RdStr(8);
	this.onDomed=null;/* ����ӵ�dom����Ļص����� */
	
	//private property
	var p={opts:opts};
	this.__p=p;
	
	/* ��֤���ݵ���Ч�� ���ҹ���$Layout*/
	this.Validate();
	if(!this.Validation.valid) return;
	
	/* Load elements */
	this.LoadElements();
	
	p.mouseEnter=function(evt){
		_this.$Layout.addClass(opts.clOn);
		_this.Editor.CurCT=_this;
		sohu.diyConsole.CurCT=_this;
		//��ק�����¼�
		
		var dim=_this.Dim();
		/*v20100722
		sohu.diyConsole.Dragger.handle.show()
		.css({width:dim.w,height:dim.h,opacity:0.3})
		.unbind()
		.bind("mousedown",function(evt){
			sohu.diyConsole.Dragger.ing=true;
			sohu.diyConsole.Dragger.obj=_this;
			_this.Sec.Deactive();
			
		}).bind("mouseup",function(evt){
			sohu.diyConsole.Dragger.ing=false;
		}).bind("dblclick",function(evt){
			_this.DoEdit();return false;
		});
		_this.$Layout.find(".dragHandle").remove().end().prepend(sohu.diyConsole.Dragger.handle);
		*/
	};
	p.mouseLeave=function(evt){
		if(_this.Editor.CurArea.IsEditing) return false;
		_this.$Layout.removeClass(opts.clOn);
		/*sohu.diyConsole.Dragger.handle.remove();v20100722*/
		sohu.diyConsole.CurCT=null;
	};
	
	//���ݵ�����¼�
	this.$Layout.mouseenter(p.mouseEnter).mouseleave(p.mouseLeave);
	//�Ƿ�flash
	if(this.$Layout.flash){
		this.ID+="_fl";
		//��flash������ֳ���
		var fOpt={tplID:this.$Layout.tplID};
		if(opts.scale){fOpt.w=this.MaxWidth;};
		this.onDomed=function(mode){
			this.$Layout.flashObj=new sohu.diyTp.Flash(fOpt);
			this.$Layout.flashObj.Render(this.$Layout);
		};
	};
	this.$Layout.attr("id",this.ID);
};
/**
 * ��ȡ���ݵ�ά����Ϣ
 * @return {Object} {x,y,w,h}
 */
sohu.diyContent.prototype.Dim=function(){
	return {
		x:this.$Layout.offset().left,
		y:this.$Layout.offset().top,
		w:this.$Layout.width(),
		h:this.$Layout.height()
	};
};
/**
 * ����html�༭���༭����
 */
sohu.diyContent.prototype.DoEdit=function(){
	this.Editor.DialogCT("update");
};
/**
 * ��֤��ǰ�����Ƿ���Ч
 */
sohu.diyContent.prototype.Validate=function(){
	var _this=this;
	this.SetValidation(true);
	
	var commonValidate=function(ct,msg){
		ct.$Layout=$(ct.Meta.html0).filter("."+ct.Type);
		if(ct.$Layout.length==0||(!ct.$Layout.is("."+ct.__p.opts.cl))){
			ct.SetValidation(false,msg);
		};
	};
	
	switch(this.Type){
		case "ohmygod":
			this.SetValidation(false,"Html���ݲ����Ͽ��ӻ�ר��ģ��淶");
		break;
		case "shtable":
			this.$Layout=$(this.Meta.html0).filter("table");
			if(this.$Layout.length==0){
				this.SetValidation(false,"Html�����ޱ���ǩ");
			}else{
				//��$Layout�滻�ɷ���diy����ģ��淶�Ķ���
				this.$Layout=$("<div/>").addClass(this.__p.opts.cl+" "+this.Type+" clear").append(this.$Layout);
			}
		break;
		case "shflash":
			this.$Layout=$(this.Meta.html0);
		break;
		case "shline":
			commonValidate(this,"Html���ݲ�����{����}ģ��淶");
		break;
		case "shimage":
			commonValidate(this,"Html���ݲ�����{ͼ��}ģ��淶");
		break;
		case "shtext":
			commonValidate(this,"Html���ݲ�����{�ı�}ģ��淶");
		break;
		default:
			this.SetValidation(false,"Html���ݲ����Ͽ��ӻ�ר��ģ��淶");
		break;
	};
	if(this.Validation.valid){
		$.extend(this.$Layout,this.Meta);
	};
	return this;
};
/**
 * ������֤��Ϣ
 * @param {Boolean} isValid �Ƿ���Ч
 * @param {String} ��֤�����ʾ��Ϣ
 */
sohu.diyContent.prototype.SetValidation=function(isValid,msg){
	this.Validation={
		valid:isValid,
		msg:msg||null
	};
	return this;
};
sohu.diyContent.prototype.LoadElements=function(){
	var _this=this;
	var items=this.$Layout.find("."+this.__p.opts.clElm);
	items.each(function(i,o){
		new sohu.diyElement({
			$dom:$(o),
			ct:_this
		});
	});
};
/*��̬����*/
/**
 * �����е�domԪ�ع���һ��diyContent ����
 * @param {Object} opts ѡ��
 */
sohu.diyContent.New=function(opts){
	return new sohu.diyContent(opts);
};
