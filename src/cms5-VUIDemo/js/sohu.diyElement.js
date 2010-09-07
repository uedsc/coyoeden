/**
 * ���ӻ��༭Ԫ��
 * @dependency sohu.iframeEX,sohu.iEditable,jquery.cssEX,sohu.diyElementTool.js
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
	p.onEditModeChange=function(dom){
		if (dom.iEditing) {
			_this.i$frame=_this.$Layout[0].i$frame;
			if(_this.$Layout.parent().is(".sec_hd")){
				sohu.diyDialog.Show("wSecHead");
			}else{
				sohu.diyDialog.Show("wText");
			};
			_this.Overlay("on");
			sohu.diyConsole.$EHolder.IsBusy=_this.IsEditing=true;
			_this.CT.InlineEdit("on");
			
		}else{
			_this.i$frame=null;
			_this.CT.InlineEdit("off");
			sohu.diyConsole.$EHolder.IsBusy=_this.IsEditing=false;
			
		}
			
	};
	p.initEditable=function(){
		_this.$Layout.click(function(evt){
			sohu.diyChipEditor.Show(_this.$Context,{
				tabs:[0],
				$elm:_this.$Layout,
				onSave:function(dlg){
					dlg.Hide();
				},
				afterShow:function(hash,dlg){
					if(_this.Copyable){
						dlg.$ElmcActs.show();
					}else{
						dlg.$ElmcActs.hide();
					};
				}
			});
			return false;
		});
		/*
		//img
		if(_this.tagName=="img"){
			_this.$Layout.click(function(evt){
				sohu.diyDialog.Show("wImage");
				_this.Overlay("on");
				sohu.diyConsole.$EHolder.IsBusy=_this.IsEditing=true;
				_this.CT.InlineEdit("on");
			});
			return;
		};
		*/
		
	};
	/* /private member variables */
	
	p.Init=function(){
		p.detectType();
		p.initEditable();
	};
	
	//��ʼ��
	p.Init();
	
	//����¼�
	this.$Layout.mouseenter(function(evt){
		_this.$Layout.addClass(opts.clOn);
	});
	this.$Layout.mouseleave(function(evt){
		_this.$Layout.removeClass(opts.clOn);
	});
	
	this.$Layout.mousedown(function(evt){
		if (sohu.diyConsole.CurElm) {
			sohu.diyConsole.CurElm.HideEditor(true);
			sohu.diyDialog.Hide(true);
			//sohu.diyDialog.Hide();
		};	
		sohu.diyConsole.CurElm=_this;
		return false;
	});
	//���γ�����
	this.$Context.find("a").css("cursor","text").click(function(evt){
		evt.preventDefault();
		return true;
	});
};
/**
 * �ر�Ԫ�صı༭״̬
 * @param {Object} ignoreCbk
 */
sohu.diyElement.prototype.HideEditor=function(ignoreCbk){
	/*
	if(this.InlineEditable){
		this.IFEdit("off",ignoreCbk);
	}else{
		this.CT.InlineEdit("off");
	}
	*/
	sohu.diyConsole.CurElm=null;
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
 * Force the element to switch on the edit state
 */
sohu.diyElement.prototype.ForceEdit=function(){
	this.$Layout.trigger("mousedown");
	this.$Layout.trigger("click");
};
