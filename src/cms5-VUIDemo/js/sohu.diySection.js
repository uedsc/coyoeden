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
		clContent:"ct",
		clTip:"secTip"
		},opts);
	var p={opts:opts};
	this.__p=p;
	//����
	this.ID="sec_"+StringUtils.RdStr(8);
	this.$Layout=opts.$obj;
	this.Width=this.Size();
	this.Divisible=(this.Width>=390);//�ɼ�������
	this.IsActive=false;
	this.IsAddingContent=false;
	this.InlineEditing=false;
	this.CurArea=opts.curArea;//��ǰ�������ڵĺ��С�����LoadCurArea����ʱ���¸�����
	this.Editor=new sohu.diyEditor({
		curArea:this.CurArea,
		curSec:this	
	});

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
	this.$Layout.attr("id",this.ID).mouseenter(p.mouseOver);//.mouseleave(p.mouseOut);
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
			sohu.diyConsole.Dragger.obj.Sec.RemoveCTByID(sohu.diyConsole.Dragger.obj.ID);
			sohu.diyConsole.Dragger.obj.Sec=_this;
			_this.Contents.push(sohu.diyConsole.Dragger.obj);
		}
	});
	/*  */
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
	//������ǰ�����Ƿ񼤻û����Ļ�����
	if(!this.CurArea.IsActive){
		this.CurArea.Active();
	};
	/* ��������һ������ */
	if(sohu.diyConsole.CurSec){sohu.diyConsole.CurSec.Deactive();};
	this.IsActive=true;
	this.$Layout.addClass(this.__p.opts.clSecOn);
	//Show the toolbar
	this.Editor.Show();
	//Update sohu.diyConsole.CurSec
	sohu.diyConsole.CurSec=this;
};
sohu.diySection.prototype.Deactive=function(){
	if(this.IsAddingContent||this.InlineEditing) return;/* ���ڱ༭���� */
	this.Editor.Hide();
	this.IsActive=false;
	this.$Layout.removeClass(this.__p.opts.clSecOn);
};
/**
 * ����ӷ���
 */
sohu.diySection.prototype.AddSub=function($secSub){
	var _this=this;
	this.Editor.UpdateCT({$Layout: $secSub},1);
	var subSecs=$secSub.find("."+this.__p.opts.clSec);
	this.$Layout.addClass(this.__p.opts.clHasSub);
	subSecs.each(function(i,sec){
		sohu.diySection.New({
			$obj:$(sec),
			curArea:_this.CurArea
		});
	});
};
/**
 * �������-�ڷ�����ĩβ�������
 * @param {Object} ct ��������ݶ�����{html0:'xx',flash:false}
 */
sohu.diySection.prototype.AddContent=function(ct){
	var _this=this;
	if(ct.isNew){
		/* ���� */
		//������Ӧ��diyContentʵ��
		ct=sohu.diyContent.New({sec:this,ct:ct});
		if(!ct.Validation.valid){
			alert(ct.Validation.msg);
			return;
		};
		this.Editor.UpdateCT(ct,1);
		this.Contents.push(ct);
	}else{
		/* ���� */
		var ct0=this.GetCTByID(ct.attr("id"));
		if(!ct0) return;
		if(ct.html0==""){
			if (window.confirm("HTML����Ϊ�ջ��߲�����ģ��淶,�Ƿ�ȷ��ɾ��ԭ����?")) {
				ct0.$Layout.remove();
			};
		}else{
			ct0.$Layout.html(ct.html0);
		};//if1	
	};//if0
};
/**
 * �����������
 */
sohu.diySection.prototype.Cls=function(){
	this.$Layout.find("."+this.__p.opts.clContent).remove();
	this.$Layout.removeClass("hasSub");
	this.Contents=[];
};
/**
 * ��������id��ȡ���ݶ���
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
 * ��������id�������б����Ƴ����ݶ���ע��,�Ǵ�dom���Ƴ����˷�����������ʱ��
 * @param {String} ctID ����ID
 * @return {diyContent} ���Ƴ��Ķ���
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
		h:this.$Layout.height(),
		mw:this.Size()
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

