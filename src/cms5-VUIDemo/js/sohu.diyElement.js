/**
 * ���ӻ��༭Ԫ��
 * @dependency sohu.diyMenuBar.js,sohu.diyConsole.js
 * @author levin
 */
sohu.diyElement=function(opts){
	opts=$.extend({},{cl:"elm",clOn:"elmOn",clCopyable:"elmc"},opts||{});
	var _this=this;
	this.CT=opts.ct;
	this.$Context=this.CT.$Layout;
	this.$Layout=opts.$dom;
	this.i$frame=null;/* Reference to the iframe editor */
	this.InlineEditable=true;
	this.tagName=this.$Layout[0].tagName.toLowerCase();
	this.Copyable=false;
	this.IsSelfCopyable=false;
	this.$CopyModel=null;
	this.IsEditing=false;
	
	/* private member variables */
	var p={opts:opts};
	this.__p=p;
	p.detectType=function(){
		if(_this.tagName=="img")
			_this.InlineEditable=false;
		
		//Copyable
		if(_this.$Layout.is(".elmc")){
			_this.Copyable=true;
			_this.IsSelfCopyable=true;
			_this.$CopyModel=_this.$Layout;
		}else{
			var o0=_this.$Layout.parents(".elmc");
			if(o0.length>0){
				o0=o0.eq(0);
				_this.Copyable=true;
				_this.IsSelfCopyable=false;
				_this.$CopyModel=o0;
			};
		};
	};
	/* /private member variables */
	
	p.Init=function(){
		p.detectType();
	};
	
	//��ʼ��
	p.Init();
	
	this.BindEvts();
	//����diyConsole�����evtPreview�¼�
	if(bos){
		$(bos).bind("evtPreview",function(e){
			_this.IsEditing=false;
		});
	};
};
/**
 * �Ƴ����ӻ��༭ʱ���¼�
 */
sohu.diyElement.prototype.UnbindEvts=function(){
	this.$Layout.unbind(".edit");
	this.$Context.find("a").unbind("click.noNav");
};
/**
 * �󶨿��ӻ��༭ʱ���¼�
 */
sohu.diyElement.prototype.BindEvts=function(){
	var _this=this;
	//����¼�
	this.$Layout.bind("mouseenter.edit",function(evt){
		_this.$Layout.addClass(_this.__p.opts.clOn);
	});
	this.$Layout.bind("mouseleave.edit",function(evt){
		_this.$Layout.removeClass(_this.__p.opts.clOn);
	});
	//�����������ݱ༭
	this.$Layout.bind("click.edit",$.proxy(this.EditView,this));	
	//���γ�����
	//this.$Context.find("a").bind("click.noNav",sohu.diyConsole.OnStopNav);
	//�Զ����¼�
	this.$Layout.unbind("evtBindEvt").bind("evtBindEvt",function(e){
		_this.BindEvts();
		return false;//ֹͣð��
	});
	this.$Layout.bind("evtUnbindEvt.edit",function(e){
		_this.UnbindEvts();
		return false;//ֹͣð��
	});	
};
/**
 * dimension info of the element
 */
sohu.diyElement.prototype.Dim=function(){
	var of=this.$Layout.offset();
	var pl=0,pr=0;
	pl=parseInt(this.$Layout.css("padding-left"));
	pl=isNaN(pl)?0:pl;
	pr=parseInt(this.$Layout.css("padding-right"));
	pr=isNaN(pr)?0:pr;
	var d={
		x:of.left,
		y:of.top,
		h:this.$Layout.height(),
		w:this.$Layout.width(),
		pl:pl,	/* ������ */
		pr:pr,	/* ������ */
		fl:0,	/* ��߸���Ԫ�صĿ�� */
		fr:0	/* �ұ߸���Ԫ�صĿ�� */
	};
	
	//��ȡ��߸�����Ԫ��
	var fl=$.grep(this.$Layout.prevAll(),function(o,i){
		if($(o).css("float")!="none") return true;
		return false;
	});
	if(fl.length==0) return d;
	
	fl=$(fl[0]);
	//���
	var w0=fl.width();
	//�߾�
	var ml=parseInt(fl.css("margin-left"));
	ml=isNaN(ml)?0:ml;
	var mr=parseInt(fl.css("margin-right"));
	mr=isNaN(mr)?0:ml;
	//����
	pl=parseInt(fl.css("padding-left"));
	pl=isNaN(pl)?0:pl;
	pr=parseInt(fl.css("padding-right"));
	pr=isNaN(pr)?0:pr;
	//ʵ�ʿ��
	w0+=ml+mr+pl+pr;
	
	if(fl.css("float")=="left")
		d.fl=w0;
	else
		d.fr=w0;
		
	return d;
	 
};
/**
 * ���ƻ�������
 * @param {Object} isUp �Ƿ�����
 */
sohu.diyElement.prototype.Move=function(isUp){
	//�ƶ�
	var $obj=isUp?this.$CopyModel.prevAll("."+this.__p.opts.clCopyable):this.$CopyModel.nextAll("."+this.__p.opts.clCopyable);
	if($obj.length==0) return;
	$obj=$obj.eq(0);
	if(isUp){
		$obj.before(this.$CopyModel);
	}else{
		$obj.after(this.$CopyModel);
	};
	var d=this.Dim();
	if(this.i$frame){
		this.i$frame.css({top:d.y,left:d.x});			
	};
};
/**
 * Force the element to switch to the edit state
 */
sohu.diyElement.prototype.EditView=function(){
	var _i=this;
	if(this.IsEditing) return;// false;
	if (sohu.diyConsole.CurElm) {
		sohu.diyConsole.CurElm.IsEditing=false;
		sohu.diyDialog.Hide(true);
		//sohu.diyDialog.Hide();
	};
	sohu.diyConsole.CurElm=this;
	//��ʾ��Ƭ�༭��		
	sohu.diyChipEditor.Show(this.$Context,{
		tabs:[0],
		$elm:this.$Layout,
		elm:this,
		onSave:function(dlg){
			dlg.Hide();
		},
		afterShow:function(hash,dlg){
			_i.CT.InlineEdit("on");
			_i.IsEditing=true;
			//�Ƿ�����Ԫ�صġ����ӡ�ɾ�������ơ����ơ���ť
			if(_i.Copyable){
				dlg.$ElmcActs.show();
			}else{
				dlg.$ElmcActs.hide();
			};
			//������ק����
			sohu.diyConsole.Dragger.handle.hide();
		},
		afterHide:function(hash,dlg){
			_i.IsEditing=false;
			_i.CT.InlineEdit("off");
		}
	});
};
