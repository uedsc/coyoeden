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
	opts=$.extend({},{clArea:"area",clActive:"area_active",clEmpty:"area_empty",isNew:true,clSec:"sec",clHelper:"areaTip",clContent:"ct"},opts);
	var _this=this;
	this.TemplateID=null;//����ģ��id
	this.Console=opts.console;
	this.$Workspace=this.Console.$Workspace;
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
	/*
	 ���еļ����ɷ�������
	//���¼�
	p.bindEvts=function(){
		_this.$Layout.mouseenter(function(evt){_this.Active();});
			//.mouseleave(function(evt){_this.Deactive();});		
	};
	//�Ƴ��󶨵��¼�������
	p.unbindEvts=function(){
		//_this.$Layout.unbind("mouseenter mouseleave");
		_this.$Layout.unbind("mouseenter");
	};
	*/
	//��Ϊ
	if(opts.isNew){
		this.TemplateID=opts.tplID;
		this.ID="content_"+StringUtils.RdStr(8);
		p.addNew();
	}else{
		this.$Layout=opts.obj;
		this.ID=this.$Layout.attr("id");
		this.TemplateID=this.ID.substr(0,this.ID.lastIndexOf("_"));
	};
	//this.$Layout.effect("highlight",null,"fast");
	//p.bindEvts();
	
	this.__p=p;
	
	//���з�������
	this.LoadSections();
};
/**
 * ����
 */
sohu.diyArea.prototype.Active=function(){
	if(this.IsActive) return;
	if(sohu.diyConsole.CurSec&&(sohu.diyConsole.CurSec.IsAddingContent||sohu.diyConsole.CurSec.InlineEditing)) return;
	var _this=this;
	//if(this.IsActive) return;
	//if(this.$Layout.hasClass(this.__p.opts.clActive)) return;
	//this.Console.AreaList().removeClass(this.__p.opts.clActive);
	this.$Layout.addClass(this.__p.opts.clActive);
	this.Console.ActiveArea(this).RePosition();
	this.IsActive=true;
	//��ʱ�Ƴ��¼�������
	//this.__p.unbindEvts();
};
/**
 * �Ƴ�����״̬
 */
sohu.diyArea.prototype.Deactive=function(){
	if(this.IsEditing||(!this.IsActive)||(sohu.diyConsole.EditingSec!=null)) return;
	this.$Layout.removeClass(this.__p.opts.clActive);
	this.IsActive=false;
	
	//this.__p.unbindEvts();
	//�����Ƿ�Ϊ��
	if(!this.IsEmpty()){
		this.$Layout.removeClass(this.__p.opts.clEmpty);
	}else{
		//û�����ݵĻ�������ʾ���еĿ���Ա��û��鿴
		this.$Layout.addClass(this.__p.opts.clEmpty);
	};
	//this.__p.bindEvts();

};
/**
 * ���º��е�ID��ͬʱ����sohu.diyConsoleʵ���Areas����
 * @param {Object} newID
 */
sohu.diyArea.prototype.UpdateID=function(newID){
	if((!newID)||(newID==this.ID)||(!sohu.diyConsole.IsValidID(newID))) return;
	//�����жϸ�ID�Ƿ��Ѿ�����
	var isOk=true;
	$.each(this.Console.Areas,function(i,o){
		if(o.ID==newID){
			return (isOk=false);
		};
		return true;
	});
	if (!isOk) {
		alert("���" + newID + "�Ѿ���ռ�ã�");
		return false;
	};
	
	this.ID=newID;
	this.$Layout.attr("id",this.ID);
	
	return true;
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
	if(isUp){sibling.before(this.$Layout);}else{sibling.after(this.$Layout);};
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
