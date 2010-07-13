/**
 * ��-����,ͨ��diyEditor��ӷ���ʱ����
 * @author levinhuang
 * @param {Object} opts ѡ�����
 */
sohu.diySection = function(opts) {
	var _this=this;
	opts=$.extend({},{
		clSecSub:"subsec",
		limitSec:390,clSec:"sec",clSecOn:"secOn",
		clHasSub:"hasSub",
		clSecRoot:"col",
		clArea:"area",
		clContent:"ct"
		},opts);
	var p={opts:opts};
	this.__p=p;
	//����
	this.$Layout=opts.$obj;
	this.Width=this.Size();
	this.Divisible=(this.Width>=390);//�ɼ�������
	this.IsActive=false;
	this.IsAddingContent=false;
	this.Editor=opts.editor;
	this.CurArea=opts.curArea;//��ǰ�������ڵĺ��С�����LoadCurArea����ʱ���¸�����

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
	//����¼�-����ֹͣð���¼�
	this.$Layout.mouseenter(p.mouseOver);//.mouseleave(p.mouseOut);
	//��ȡ��ǰ����������
	this.Contents=this.LoadContents();
	//��ȡ��ǰ����
	//this.LoadCurArea();
	//�����¼�����
	this.$Layout.sortable({
		items:">.ct",
		connectWith:".sec",
		placeholder:"ui-hl",
		handle:".dragHandle",
		receive:function(evt,ui){
			sohu.diyConsole.Dragger.obj.Sec=_this;
			
		},
		start:function(evt,ui){
			_this.Editor.RePosition();
		},
		stop:function(evt,ui){
			_this.Editor.RePosition();
		}
	});
	//�Զ����¼�
	this.$Layout.bind("evtActive",function(e){
		_this.Active();
		return false;//ֹͣð��
	});
}; 
/**
 * �������
 */
sohu.diySection.prototype.Active=function(){
	if(this.IsActive) return;
	if(this.Editor.CurSec){this.Editor.CurSec.Deactive();};
	//������ǰ�����Ƿ񼤻û����Ļ�����
	if(!this.CurArea.IsActive){
		this.CurArea.Active();
	};
	this.Editor.AttachTo(this).Show();
	this.IsActive=true;
	this.$Layout.addClass(this.__p.opts.clSecOn);
};
sohu.diySection.prototype.Deactive=function(){
	this.Editor.Remove();
	this.IsActive=false;
	this.$Layout.removeClass(this.__p.opts.clSecOn);
};
/**
 * ����ӷ���
 */
sohu.diySection.prototype.AddSub=function($secSub){
	var _this=this;
	this.Editor.UpdateCT($secSub,1);
	var subSecs=$secSub.find("."+this.__p.opts.clSec);
	this.$Layout.addClass(this.__p.opts.clHasSub);
	subSecs.each(function(i,sec){
		sohu.diySection.New({
			$obj:$(sec),
			editor:_this.Editor,
			curArea:_this.CurArea
		});
	});
};
/**
 * �������-�ڷ�����ĩβ�������
 * @param {Object} ct ��������ݶ�����{html:'xx',flash:false}
 */
sohu.diySection.prototype.AddContent=function(ct){
	var $ct=$(ct.html);
	$.extend($ct,ct);
	this.Editor.UpdateCT($ct,1);
	//������Ӧ��diyContentʵ��
	var ct=sohu.diyContent.New({$obj:$ct,sec:this});
	this.Contents.push(ct);
};
/**
 * �����������
 */
sohu.diySection.prototype.Cls=function(){
	this.$Layout.find("."+this.__p.opts.clContent).remove();
	this.$Layout.removeClass("hasSub");
};
/**
 * ��ȡ��ǰ�����������Ŀ��
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
 * �Ƿ����ӷ���
 */
sohu.diySection.prototype.HasSub=function(){
	var cnt=this.$Layout.find("."+this.__p.opts.clSecSub).size();
	return (cnt>0);
};
/**
 * ��ȡ��ǰ���������ĺ��ж���
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
 * ���ص�ǰ�����൱��window��x��yֵ,�Լ�����ĸߺͿ�
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
 * ��ȡ�÷���������
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
/**
 * ���������
 */
sohu.diySection.prototype.ActiveParent=function(){
	var $psec=this.$Layout.parents("."+this.__p.opts.clSec+":first");
	$psec.trigger("evtActive");
};
/*��̬����*/
/**
 * �����е�domԪ���½�һ��diySection����
 * @param {Object} opts 
 */
sohu.diySection.New=function(opts){
	return new sohu.diySection(opts);
};

