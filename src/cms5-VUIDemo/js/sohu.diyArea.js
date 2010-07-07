/**
 * ��-����
 * @author levin
 * @desc	������ؽ���
 * @param {Object} opts ѡ��
 * @dependency sohu.diy.js
 */
sohu.diyArea=function(opts){
///<summary>���ж���</summary>
	//����
	opts=$.extend({},{clArea:"area",clActive:"area_active",isNew:true,clSec:"sec",clHelper:"areaTip",clContent:"ct"},opts);
	var _this=this;
	this.TemplateID=null;//����ģ��id
	this.Console=opts.console;
	this.$Workspace=this.Console.$Workspace;
	this.$Helper=this.Console.__p._$areaHelper.clone();
	this.IsEditing=false;//�Ƿ��ڱ༭״̬:1,��ӷ���ʱΪtrue
	this.IsActive=false;//�����Ƿ񼤻�
	
	var p={
		opts:opts
	};
	p.addNew=function(){
		_this.$Layout=function(){
			return $(sohu.diyTp[opts.tplID]);
		}();
		_this.$Layout.attr("id",_this.ID);
		if(!_this.Console.CurArea){
			_this.$Workspace.append(_this.$Layout);
		}else{
			_this.Console.CurArea.$Layout.after(_this.$Layout);
		};		
	};
	//���¼�
	p.bindEvts=function(){
		_this.$Layout.mouseenter(function(evt){_this.Active();})
			.mouseleave(function(evt){_this.Deactive();});		
	};
	//�Ƴ��󶨵��¼�������
	p.unbindEvts=function(){
		_this.$Layout.unbind("mouseenter mouseleave");
	};
	//��Ϊ
	if(opts.isNew){
		this.TemplateID=opts.tplID;
		this.ID=this.TemplateID+"_"+sohu.diyConsole.RdStr(8);
		p.addNew();
		this.$Layout.prepend(this.$Helper.css("opacity",0.7));
	}else{
		this.$Layout=opts.obj;
		this.ID=this.$Layout.attr("id");
		this.TemplateID=this.ID.substr(0,this.ID.lastIndexOf("_"));
	};
	//�����¼�-ע�������о��з���ʱ���������������¼���������false��ֹͣ���¼���ð�ݣ����е�����¼�����������
	this.$Layout.effect("highlight",{easing:'easeInElastic'},'slow');
	p.bindEvts();
	
	this.__p=p;
	
	//���з�������
	this.LoadSections();
};
/**
 * ����
 */
sohu.diyArea.prototype.Active=function(){
	var _this=this;
	if(this.IsActive) return;
	if(this.$Layout.hasClass(this.__p.opts.clActive)) return;
	this.Console.AreaList().removeClass(this.__p.opts.clActive);
	this.$Layout.addClass(this.__p.opts.clActive);
	this.Console.ActiveArea(this).RePosition();
	this.IsActive=true;
	//��������dom
	this.__p.unbindEvts();
	_this.$Helper.slideUp("fast",this.__p.bindEvts);
};
/**
 * �Ƴ�����״̬
 */
sohu.diyArea.prototype.Deactive=function(){
	if(this.IsEditing||(!this.IsActive)) return;
	var _this=this;
	_this.$Layout.removeClass(_this.__p.opts.clActive);
	
	this.IsActive=false;
	
	this.__p.unbindEvts();
	//��ʾ����dom
	if(!_this.IsEmpty()){
		_this.$Helper.hide();
	}else{
		_this.$Helper.slideDown(500,_this.__p.bindEvts);
	};

};
/**
 * �ƶ�
 * @param {Object} isUp ����true������false
 */
sohu.diyArea.prototype.Move=function(isUp){
	var sibling=isUp?this.$Layout.prev():this.$Layout.next();
	var tip0=isUp? "�����������ƶ���...":"�����������ƶ���...";
	if (sibling.size() == 0) {
		alert(tip0);return false;
	};	
	if(isUp){sibling.before(this.$Layout);}else{sibling.after(this.$Layout)};
	return false;	
};
/**
 * �Ƴ�
 */
sohu.diyArea.prototype.Remove=function(){
	if(!window.confirm("ȷ��ɾ��ѡ�еĺ���ô?")) return false;
	this.$Layout.remove();
	if(this.__p.opts.onRemove){
		this.__p.opts.onRemove(this);
	};
	return false;	
};
/**
 * ���ص�ǰ�����ڵ����з���
 */
sohu.diyArea.prototype.LoadSections=function(){
	var _this=this;
	var items=this.$Layout.find("."+this.__p.opts.clSec);
	items=items.map(function(i,sec){
		return sohu.diySection.New({
			$obj:$(sec),
			editor:_this.Console.Editor,
			secHelper:_this.Console.__p._$secHelper,
			curArea:_this
		});
	});
	return items;
};
/**
 * ��ǰ�����Ƿ�������
 */
sohu.diyArea.prototype.IsEmpty=function(){
	var ct=this.$Layout.find("."+this.__p.opts.clContent);
	return (ct.length==0);
};
/**
 * ���ص�ǰ�����൱��window��x��yֵ,�Լ�����ĸߺͿ�
 * @return {Object} {x,y,w,h}
 */
sohu.diyArea.prototype.Dim=function(){
	return {
		x:this.$Layout.offset().left,
		y:this.$Layout.offset().top,
		w:this.$Layout.width(),
		h:this.$Layout.height()
	};
};
