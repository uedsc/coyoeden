/**
 * @author levinhuang
 * @desc	CMS5���ӻ��༭��-js�����ඨ��
 * @dependence 1,sohu.diy.js
 */

/**
 * @class SubArea manager
 * @opts ѡ��
 */
sohu.diy.SubAreaManager=function(opts){
	var _this=this;
	/*BEGIN���Զ���*/
	//TODO:������
	this.cfg={
		colClass:"col",/*�շ���css class*/
		colClassActive:"subarea-active",
		areaWidth:950,
		editorBodyClass:"area_body"
	};
	
	this.area=null;/*Ŀ�����-���е�jq����*/
	this.areaIsEmpty=false;/*Ŀ������ǿպ���-��950����*/
	this.columns=null;
	this.columnsX=null;
	this.canSubArea=true;
	this.target=null;
	this.curTemplate=null;/*��ǰѡ�еķ���ģ��*/
	
	/*END���Զ���*/
	
	//����ѡ�����¼�ע��
	$('#hiddenTemplate .subarea_selector li').click(function(evt){
		_this.curTemplate=this.id;
		_this.areaSelector.dialog("close");
		return false;
	}).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");});
	
};
sohu.diy.SubAreaManager.prototype.SetArea=function(area){
	//���б仯�����������
	this.area=area;
	this.target=null;
	this.columnsX=[];
	this.columns=null;
	this.curTemplate=null;
	//��ȡ����
	this.LoadColumns();
};
/**
 * ��ȡ�����µ�ǰ�������ӷ���
 */
sohu.diy.SubAreaManager.prototype.LoadColumns=function(){	
	var _this=this;
	this.columnsX=[];
	this.columns=this.area.find("."+this.cfg.colClass);
	this.areaIsEmpty=(this.columns.length==0);
	//�պ��е����⴦��
	if(this.areaIsEmpty){
		this.columns=this.area.addClass(this.cfg.colClass);
	};
	//�������п��Ƿ���Է���
	this.columns.each(function(i,obj){
		var width=_this.getColSize($(obj));
		obj.canSubArea=false;
		if(width>=390){obj.canSubArea=true;obj._width=width;_this.columnsX.push(obj);};
	});//each
	this.columns.click(function(evt){
		_this.SetTarget($(this));
	});
	
	this.canSubArea=this.columnsX.length>0;
};
sohu.diy.SubAreaManager.prototype.SetTarget=function(colObj){
	$(this.columns).removeClass(this.cfg.colClassActive);
	if (colObj) {
		this.target = colObj.addClass(this.cfg.colClassActive);
	};
	return this;
};
sohu.diy.SubAreaManager.prototype.Dialog=function(){
	var _this=this;
	this.CheckTarget();
	//var width=this.getColSize(this.target);
	//if(width==390) return;
	if (!this.target[0].canSubArea) {
		alert("�ߴ�С��390���޷��ٽ��в��");
		return;
	};
	var templateID="#subarea_selector_"+this.target[0]._width;
	var _onClose=function(evt,ui){
		if(!_this.curTemplate) return;
		if(_this.areaIsEmpty){
			_this.target.find("."+_this.cfg.editorBodyClass).html(sohu.diyTp[_this.curTemplate]);
		}else{
			_this.target.html(sohu.diyTp[_this.curTemplate]);
		};
		_this.SetTarget(null);
		//�÷��������ǿշ���
		_this.target.removeClass(_this.cfg.colClass).unbind("click");
		//ˢ���ӷ���
		_this.LoadColumns();
	};
	this.areaSelector=$(templateID).dialog({
		title:"��ӷ���",
		resizable:false,
		modal:true,
		width:430,
		height:250,
		close:_onClose
	});
	
};
sohu.diy.SubAreaManager.prototype.getColSize=function(colObj){
	//�պ����������
	if(this.areaIsEmpty){
		return this.cfg.areaWidth;	
	};
	//�����������ĺ���
	var width=0;
	var classes=colObj.attr("class").split(" ");
	$.each(classes,function(i1,o1){
		if(o1.indexOf("w")==0){
			width=parseInt(o1.substr(1));
			return false;
		};
	});//each
	return width;
};
/**
 * ��鵱ǰ������Ŀ�����Ƿ���ڣ������������Ĭ��ָ����һ��
 */
sohu.diy.SubAreaManager.prototype.CheckTarget=function(){
	if(!this.target){this.SetTarget(this.columns.eq(0));};
};
sohu.diy.SubAreaManager.prototype.AddContent=function(ct){
	this.CheckTarget();
	if (this.areaIsEmpty) {
		this.target.find("." + this.cfg.editorBodyClass).append(ct);
	}else{
		this.target.append(ct);
	};
};

/**
 * @class AreaEditor
 */
sohu.diy.AreaEditor=function(opts){
	//����
	this.layoutModel=opts.layoutModel;
	this.target=null;//��ǰ���е�dom
	this.area=null;//��ǰ���ж���
	this.subAreaManager=new sohu.diy.SubAreaManager({});
	this.contentDialog=null;
};
sohu.diy.AreaEditor.prototype.AddSubArea=function(){
	if(!this.target) return;
	this.subAreaManager.Dialog();
};
sohu.diy.AreaEditor.prototype.Show=function(opts){
	var _this=this;
	if (!opts.target) {
		alert("AreaEditor����Show����ʧ��-����target��Ч");return false;
	};
	//prepare ui
	this.layout=this.layoutModel.clone();
	this.layout_body=this.layout.find(".area_body");
	this.layout_actions=this.layout.find(".actions").hide();
	this.btn={
		addContent:this.layout.find(".a_content"),
		addSubArea:this.layout.find(".a_subArea"),
		clear:this.layout.find(".a_clear"),
		editCode:this.layout.find(".a_code")
	};

	this.target=opts.target;
	this.area=opts.area;
	this.layout_body.append(this.target.children());
	this.layout.appendTo(this.target);
	this.layout_actions.show();
	
	//subAreaManager
	this.subAreaManager.SetArea(this.target);
	
	//��ť�¼�ע��
	this.btn.addContent.click(function(evt){_this.contentDialog=$("#content_selector").dialog({title:"�������",width:600,height:420,modal:true});return false;});
	this.btn.addSubArea.click(function(evt){_this.AddSubArea();return false;});
	this.btn.clear.click(function(evt){_this.area.Remove();return false;});
	this.btn.editCode.click(function(evt){alert("����");return false;});
};
sohu.diy.AreaEditor.prototype.Remove=function(){
	if(!this.target) return;
	if(!this.layout_body) return;
	this.subAreaManager.SetTarget(null);
	this.target.html(this.layout_body.html());
	this.layout_body=null;
	this.layout=null;this.layout_actions=null;
	if(this.area.hasContent){
		this.target.removeClass("area-empty");
	};
};
sohu.diy.AreaEditor.prototype.AddContent=function(ct){
	this.subAreaManager.AddContent(ct);
	this.area.hasContent=true;
};
/**
 * �ر�������ݵĶԻ���
 * @param {Object} opt ѡ��
 */
sohu.diy.AreaEditor.prototype.CloseContentDialog=function(opt){
	if(!this.contentDialog) return;
	this.contentDialog.dialog("close");
};
