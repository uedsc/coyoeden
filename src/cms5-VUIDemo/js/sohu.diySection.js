/**
 * ��-����,ͨ��diyEditor��ӷ���ʱ����
 * @author levinhuang
 * @param {Object} opts ѡ�����
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
	//����
	this.$Layout=opts.$obj;
	this.Width=this.Size();
	this.Divisible=(this.Width>=390);//�ɼ�������
	this.$Helper=opts.secHelper.clone(false)
		.attr("style","")
		.removeClass(opts.clHelperHasSub+" "+opts.clHelperHot)
		.html("w:"+this.Width+"px");
	this.IsActive=false;
	this.IsAddingContent=false;
	this.Editor=opts.editor;
	this.CurArea=opts.curArea;//��ǰ�������ڵĺ��С�����LoadCurArea����ʱ���¸�����
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
	//����¼�-����ֹͣð���¼�
	/*
	this.$Layout.hover(p.mouseOver,p.mouseOut);
	*/
	this.$Layout.mouseenter(p.mouseOver).mouseleave(p.mouseOut);
	//$Helper�¼�
	this.$Helper.click(function(evt){
		_this.Active();
	}).hover(function(evt){
		if(!_this.$Helper.hasClass(opts.clHelperHasSub)) return;
		_this.$Helper.switchClass(opts.clHelperHasSub,opts.clHelperHot);
	},function(evt){
		if(!_this.$Helper.hasClass(opts.clHelperHot)) return;
		_this.$Helper.switchClass(opts.clHelperHot,opts.clHelperHasSub);
	});
	//��ȡ��ǰ����������
	this.Contents=this.LoadContents();
	//��ȡ��ǰ����
	//this.LoadCurArea();
	//�����¼�����
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
 * �������
 */
sohu.diySection.prototype.Active=function(){
	if(this.IsActive) return;
	//���ҳ������ק������UI
	if(sohu.diyConsole.Dragger.ing) return;
	this.Editor.AttachTo(this).Show();
	this.IsActive=true;
	this.$Layout.addClass(this.__p.opts.clSecOn);
	//������ǰ�����Ƿ񼤻û����Ļ�����
	if(!this.CurArea.IsActive){
		this.CurArea.Active();
	};
};
sohu.diySection.prototype.Deactive=function(){
	//���ҳ������ק������UI
	if(sohu.diyConsole.Dragger.ing) return;
	//test
	return;
	this.Editor.Remove();
	this.IsActive=false;
	this.$Layout.removeClass(this.__p.opts.clSecOn);
};
/**
 * ����ӷ���
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
 * �������-�ڷ�����ĩβ�������
 * @param {Object} $ct ����ӵ�����(jq dom)
 */
sohu.diySection.prototype.AddContent=function($ct){
	this.Editor.UpdateCT($ct.effect("highlight"),1);
	//������Ӧ��diyContentʵ��
	var ct=new sohu.diyContent({$obj:$ct,sec:this});
	this.Contents.push(ct);
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
 * ���ӷ�������dom
 */
sohu.diySection.prototype.AttachHelper=function(){
	if(!(this.$Layout.children(this.__p.opts.cssHelper).size()>0)){
		this.$Layout.prepend(this.$Helper);
	};
};
/**
 * �Ƴ���������dom
 */
sohu.diySection.prototype.RemoveHelper=function(){
	this.$Layout.children(this.__p.opts.cssHelper).remove();
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
/*��̬����*/
/**
 * �����е�domԪ���½�һ��diySection����
 * @param {Object} opts 
 */
sohu.diySection.New=function(opts){
	return new sohu.diySection(opts);
};

