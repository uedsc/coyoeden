/**
 * JS interactive logic for editing fragments
 * ��Ƭ�༭��
 * @author levinhuang
 */

sohu.diyChipEditor = function() {
    var p={},pub={};
	p.editors={};
	/* �¼����� */	
	/**
	 * ����a��ǩ��Ĭ�ϵ�����Ϊ
	 * @param {Object} evt
	 */
	p.stopNav=function(evt){
		evt.preventDefault();
		return true;		
	};
	p.initTinyMCE=function(){
		/*�����첽����tiny_mce.js�ķ�ʽ��ie�������⣬��Ҫ�Ƶ�chipEditor.js���档������Ҫ��ԭ����tinymce.init������
		p._$mce=$("#tinymce1").tinymce({
			script_url:'editor/tiny_mce/tiny_mce.js',
			theme:'simple'
		}).hide();
		*/
		tinymce.init({
			mode:'specific_textareas',
			editor_selector:'mceEditor',
			theme:'simple',
			oninit:function(){
				tinymce.activeEditor.hide();
			}
		});
	};
    //private area
    p.initVar = function(opts) {
		p._cssBold=opts.cssBold||'bb';
		p._cssColor=opts.cssColor||'cc';
		sohu.diyChipEditor.aTplStr=opts.aTplStr||'<a href="http://sohu.com" title="">sohu</a>';
		sohu.diyChipEditor.vdTplStr=opts.vdTplStr||'<a href="http://sohu.com" title=""><img src="http://images.sohu.com/uiue/vd.gif" alt="Video"/></a>';
		p._dlgModel=$(".chipEdt");
		p._$body=$("body");
		/* �û��Ļص����� */
		p.onUpPic=opts.onUpPic;
		p.onTest=opts.onTest;
		p.onSave=opts.onSave;
		p.onCancel=opts.onCancel;
		p.onGlobalRes=opts.onGlobalRes;
		p.onExternal=opts.onExternal;
		p.onFlashEdit=opts.onFlashEdit;
		p.onLoadHis=opts.onLoadHis;
		/* /�û��Ļص����� */
	};
    p.onLoaded = function() { 
		p.initTinyMCE();
	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		//$(".acts a,button").button();
		p._dlgModel.find("a").not(".cmdicon").bind('click.noNav',p.stopNav);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
	/**
	 * ��ʾ��Ƭ�༭��
	 * @param {Object} chip ��Ƭ��dom����
	 * @param {Object} opts ����ѡ��
	 */
	pub.Show=function(chip,opts){
		opts=$.extend({tabs:[0,1,2]},opts||{});
		var dlg=null,$chip=$(chip),id=$chip.attr("id");
		opts.isNew=false;
		if(!(dlg=p.editors[id])){
			opts.isNew=true;
			opts=$.extend({
				dlgModel:p._dlgModel,
				$body:p._$body,
				$chip:$chip,				/* ��Ƭ */
				$elm:opts.$elm,				/* Ԫ�� */
				onUpPic:p.onUpPic,
				onTest:p.onTest,
				onSave:p.onSave,
				onCancel:p.onCancel,
				onGlobalRes:p.onGlobalRes,
				onExternal:p.onExternal,
				onFlashEdit:p.onFlashEdit,
				onLoadHis:p.onLoadHis								
			},opts);
			dlg=new sohu.diyChipEditor.Dialog(opts);
			p.editors[id]=dlg;
			
		};
		//dlg.Show(opts);
		dlg.Edit(opts.$elm,{tabs:opts.tabs});
	};
	pub.MCE=function(){
		return tinymce.activeEditor;
	};
	pub.StopNav=p.stopNav;
	pub.$RowTxtA=$("#elmATpl .rowTxt");
	pub.$RowTitleA=$("#elmATpl .rowTitle");
	pub.$RowTxtA0=$("#elmATpl .rowTxt0");
	pub.$RowTitleA0=$("#elmATpl .rowTitle0");		
	pub.$RowLnkA=$("#elmATpl .rowLnk");
	pub.$RowImg=$("#elmImgTpl .rowImg");
	pub.$RowALT=$("#elmImgTpl .rowALT");
	pub.$RowLnkImg=$("#elmImgTpl .rowLnk");
    return pub;
} ();
/**
 * ��Ƭ�༭��������
 * @param {Object} opts
 */
sohu.diyChipEditor.Dialog=function(opts){
	opts=$.extend({},{
		tabIDTpl:'wVsp_tab',
		lblSort:'����״̬',
		lblSort1:'��������',
		lblTab0:'<strong class="alert">���޸�״̬</strong><em>��ѡ����</em>',
		clBold:'bb',
		clColor:'cc',
		onUpPic:null,
		onTest:null,
		onSave:null,
		onCancel:null,
		onGlobalRes:null,
		onExternal:null,
		onFlashEdit:null,
		onLoadHis:null
		},opts||{});
	var _this=this;
	this.Sorting=false;
	this._opts=opts;
	this.$Elm=opts.$elm;//��ǰԪ��
	this.$Chip=opts.$chip;//��ǰ��Ƭ
	this.SortElm=null;//��ǰ����Ԫ��
	
	//DOM����
	this.$Layout=opts.dlgModel.clone().appendTo(opts.$body);
	this.$CmdItems=this.$Layout.find(".cmdicon");
	//��ʼ���ı�����˵�
	new sohu.diyMenuBar({$cmdItems:this.$CmdItems,onDel:function(){
		_this.Hide();
	}});
	sohu.diyChipEditor.MCE().setContent(this.$Chip.html());
	this.$Code=this.$Layout.find('.txtVspC').val(sohu.diyChipEditor.MCE().getContent());
	/*
	this.$Layout.find('.txtVspCS').textareaSearch({
		cssTextArea:_this.$Code,
		cssBtn:_this.$Layout.find('.btnVspCS')
	});
	*/
	this.$btnCode=this.$Layout.find(".external");//.globalRes,
	this.$ElmTpl=this.$Layout.find(".elmTpl");
	
	//�¼�����
	this.$Backup=this.$Chip.clone(true);

	//�������
	this.$Layout.find(".test").click(function(evt){
		if(opts.onTest){
			opts.onTest(_this);
		};
		return false;
	});
	//������Ƭ
	this.$Layout.find(".save").click(function(evt){
		if(opts.onSave){
			opts.onSave(_this);
		};
		return false;
	});
	//ȡ��
	this.$Layout.find(".cancel").click(function(evt){
		var c=_this.$Backup.clone(true);
		_this.$Chip.replaceWith(c);
		_this.$Chip=c;
		_this.$Layout.jqmHide();
		if(opts.onCancel){
			opts.onCancel(_this);
		};
		return false;
	});
	//ͳһ��Դ��
	this.$Layout.find(".globalRes").click(function(evt){
		if(opts.onGlobalRes){
			opts.onGlobalRes(_this);
		};
		return false;
	});
	//���
	this.$Layout.find(".external").click(function(evt){
		if(opts.onExternal){
			opts.onExternal(_this);
		};
		return false;
	});
	//flash�༭
	this.$Layout.find(".btnFlashEdit").click(function(evt){
		if(opts.onFlashEdit){
			opts.onFlashEdit(_this);
		};
		return false;
	});
	//tab�˵�
	var tabID="tab"+StringUtils.RdStr(8);
	this.$TabM=this.$Layout.find(".tabM").each(function(i,o){
		var $o=$(o);
		$o.attr("href",$o.attr("href").replace(opts.tabIDTpl,tabID));
	});
	//tab����
	this.$TabC=this.$Layout.find(".tabC").each(function(i,o){
		var $o=$(o);
		$o.attr("id",$o.attr("id").replace(opts.tabIDTpl,tabID));
	});
	//����ť
	this.$Layout.find('.btnSort').click(function(evt){
		_this.ToggleSorting($(this));return false;
	});
	//tab
	this.$CT=this.$Layout.find(".jqmCT");
	/*
	this.$CT=this.$Layout.find(".jqmCT").tabs({
		select:function(evt,ui){
			//��ť������
			if(ui.index==1){
				_this.$btnCode.removeClass("hide");
			}else{
				_this.$btnCode.addClass("hide");
			};
			//�޸ļ�¼
			if(ui.index==2){
				if(opts.onLoadHis){
					opts.onLoadHis(_this);
				};
			};
		}
	});
	*/
	//�ϴ�����
	this.$UpPic=this.$CT.find(".uppic").draggable({handle:".upp_t",containment:'parent',axis:'y'});
	this.$UpPic.find(".up").click(function(evt){
		if(opts.onUpPic){
			opts.onUpPic(_this);
		};
	});
	this.$UpPic.find(".cls").click(function(evt){
		_this.$UpPic.slideUp("fast");
	});
	//jqm options
	this.jqmOpts={trigger:false,modal:false,overlay:false,autoFocus:false,beforeShow:null,afterShow:null,beforeHide:null,afterHide:null};
	this.jqmOpts.onShow=function(hash){
		var doShow=true;
		if(_this.jqmOpts.beforeShow){
			doShow=_this.jqmOpts.beforeShow(hash,_this);
		};
		if(doShow){
			hash.w.show();
			_this.jqmHash=hash;
			if(_this.jqmOpts.afterShow){
				_this.jqmOpts.afterShow(hash,_this);
			};
		};
	};
	this.jqmOpts.onHide=function(hash){
		var doHide=true;
		if(_this.jqmOpts.beforeHide){
			doHide=_this.jqmOpts.beforeHide(hash,_this);				
		};
		if(doHide){
			hash.w.hide();				
			if(_this.jqmOpts.modal){hash.o.remove();}; 
			//afterHide callback
			_this.$Chip.removeClass("on");
			//���õ�һ��tab������
			_this.$ElmTpl.empty();
			//�������ť��ʽ
			_this.$CmdItems.removeClass("editBtn1");
			if(_this.jqmOpts.afterHide)
			{
				_this.jqmOpts.afterHide(hash,_this);
			};
		};	
	};
	
	//draggable
	this.$Layout.draggable({handle:".hd",containment:'window'});
	//margin-left
	this.$Layout.css("margin-left",-(this.$Layout.width()/2));
}; 
sohu.diyChipEditor.Dialog.prototype.Show=function(opts){
	var _this=this;
	opts=$.extend({tabs:[0,1,2]},opts||{});
	$.extend(this.jqmOpts,{trigger:false,modal:false,overlay:false,autoFocus:false,beforeShow:null,afterShow:null,beforeHide:null,afterHide:null});//reset
	$.extend(this.jqmOpts,opts);
	this.IsNew=opts.isNew;
	if(!opts.isNew)
		this.Hide(opts);
	
	/*
	if((!opts.tabs)||opts.tabs.length==0){
		this.$CT.hide();
	}else{
		this.$TabM.hide();
		$.each(opts.tabs,function(i,o){
			_this.$TabM.eq(o).show();
		});
		this.$CT.tabs("select",opts.tabs[0]);
	};
	*/
	
	this.$Chip.addClass("on");
		
	this.$Layout.jqm(this.jqmOpts).jqmShow();
};
sohu.diyChipEditor.Dialog.prototype.Hide=function(opts){
	$.extend(this.jqmOpts,opts||{});
	this.$Layout.jqm(this.jqmOpts).jqmHide();
	return this;
};
sohu.diyChipEditor.Dialog.prototype.ToggleSorting=function($i){
	var _this=this;
	if(this.Sorting){
		$i.removeClass("btnSort1").find("span").html(this._opts.lblSort);
		this.$CT.tabs("option","disabled",[]);
		this.$CT.tabs("option","selected",0);
		
		this.Sorting=false;
		//��click.edit�¼�
		this.$Chip.find("a").unbind("click.sort").bind("click.edit",function(evt){
			_this.Edit($(this));
		});
		//����"���ӻ��޸�"��"�����޸�"(����Ԫ�ط����˱仯)
		this.$ElmA.empty().html(this._opts.lblTab0);
		this.$ElmImg.empty().html(this._opts.lblTab0);
	}else{
		$i.prev().trigger("click");
		$i.addClass("btnSort1").find("span").html(this._opts.lblSort1);
		this.$CT.tabs("option","disabled",[0,1,2]);
		this.Sorting=true;
		//�Ƴ���Ƭa��ǩ��click.edit�¼�����
		this.$Chip.find("a").unbind("click.edit").bind("click.sort",function(evt){
			_this.Sort($(this));
		});
	};
};
/**
 * �༭ĳ��Ԫ��
 * @param {Object} $elm
 */
sohu.diyChipEditor.Dialog.prototype.Edit=function($elm,opts){
	opts=opts||{};
	var _t=0,_this=this;
	//��Ԫ���Ƿ��ڱ༭��
	if($elm.hasClass("ing")) return;
	
	$elm.addClass("ing");
	
	var $elmList=$elm.contents();
	
	if(this.$Elm) this.$Elm.removeClass("ing");
	this.$Elm=$elm;
	this.$Chip.addClass("on");
	
	opts.afterShow=function(hash,dlg){	
		//��һ��tab
		dlg.$ElmTpl.hide().empty();
		$elmList.each(function(i,o){
			var $o=$(o);
			var tpl=null;
			if(o.nodeType!=1){
				//�ı�
				tpl=sohu.diyChipEditor.$RowTxtA.clone();
				dlg.$ElmTpl.append(tpl);
				var data={$obj:$o,$tpl:tpl};
				tpl.find("input").val($o.text()).bind("keyup",function(evt){
					var obj0=$(document.createTextNode(this.value));
					data.$obj.replaceWith(obj0);
					data.$obj=obj0;
				}).focus(_this.focusSelect);
				//ɾ����ť
				tpl.find(".btnDel").bind("click",function(evt){
					data.$obj.remove();
					tpl.remove();
				});
			}else if($o.is("a")){
				var $img=$o.find("img");
				if($img.length>0){
					//ͼƬ����
					tpl=$([sohu.diyChipEditor.$RowImg.clone()[0],sohu.diyChipEditor.$RowALT.clone()[0],sohu.diyChipEditor.$RowLnkImg.clone()[0]]);
					tpl.eq(0).find("input").val($img.attr("src"));
					tpl.eq(1).find("input").val($img.attr("alt"));
					tpl.eq(2).find("input").val($o.attr("href"));
					_this.$ElmTpl.append(tpl);
					_this.BindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:_t,$img:$img});								
				}else{
					tpl=$([sohu.diyChipEditor.$RowTitleA.clone()[0],sohu.diyChipEditor.$RowLnkA.clone()[0]]);
					tpl.eq(0).find("input").val($o.html());//attr("title")
					tpl.eq(1).find("input").val($o.attr("href"));
					dlg.$ElmTpl.append(tpl);
					_this.BindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:_t});							
				};
			};
	
		});
		_this.$ElmTpl.show();
		//�ڶ���tab
		sohu.diyChipEditor.MCE().setContent(dlg.$Chip.html());
		dlg.$Code.val(sohu.diyChipEditor.MCE().getContent());
		//�ϴ���ť
		dlg.$Layout.find(".btnUpl").click(function(evt){
			dlg.$UpPic.show().effect("highlight");
			return false;
		});
					
	};//onShow
	opts.afterHide=function(hash,dlg){
		if(dlg.$Elm){
			dlg.$Elm.removeClass("ing");
			dlg.$Elm=null;			
		};
		dlg.$Chip.removeClass("on");
	};//onHide
	this.Show(opts);
};
/**
 * ����
 * @param {Object} $i
 */
sohu.diyChipEditor.Dialog.prototype.Sort=function($i){
	if($i.hasClass("sort")) return;
	if(!this.SortElm){
		this.SortElm=$i.addClass("sort");
		return;
	};
	//��������
	var temp=this.SortElm.html();
	this.SortElm.html($i.html());
	$i.html(temp);
	
	//�������β���
	this.SortElm.removeClass("sort");
	this.SortElm=null;
	if(this.$Elm)
		this.$Elm.removeClass('ing');
	//���´���textarea
	sohu.diyChipEditor.MCE().setContent(this.$Chip.html());
	this.$Code.val(sohu.diyChipEditor.MCE().getContent());
};
/**
 * ͼ���¼�����
 * @param {Object} $tpl
 * @param {Object} data
 */
sohu.diyChipEditor.Dialog.prototype.BindIconEvts=function($tpl,data){
	var _this=this;
	//ɾ����ť
	$tpl.find(".btnDel").bind("click",function(evt){
		_this.DelElm(data);return false;
	});
	//���ư�ť
	$tpl.find(".btnAdd").bind("click",function(evt){
		_this.AddElm(data);return false;
	});
	//�Ӵ�
	$tpl.find(".bold").bind("click",function(evt){
		_this.Bold(data);return false;
	});
	//��ɫ
	$tpl.find(".color").bind("click",function(evt){
		_this.Color(data);return false;
	});
	if(data.t==0){
		//���������
		$tpl.eq(0).find("input").keyup(function(evt){
			data.$obj.html(this.value);
		}).focus(this.focusSelect);
		//���������
		$tpl.eq(1).find("input").keyup(function(evt){
			data.$obj.attr("href",this.value);
		}).focus(this.focusSelect);
		//����ͼ��
		$tpl.eq(0).find('.icon').click(function(evt){
			$(this).next().toggle();
			return false;
		});
		$tpl.eq(0).find(".others").mouseleave(function(evt){$(this).hide();});
		//��Ƶǰ
		$tpl.eq(0).find(".videoL").bind("click",function(evt){
			_this.InsertVDIcon({$obj:data.$obj,before:true});return false;
		});
		//��Ƶ��
		$tpl.eq(0).find(".videoR").bind("click",function(evt){
			_this.InsertVDIcon({$obj:data.$obj,before:false});return false;
		});			
	}else{
		//ͼƬ��ַ�����
		$tpl.eq(0).find("input").change(function(evt){
			data.$img.attr("src",this.value);
		}).focus(this.focusSelect);
		//ALT�����
		$tpl.eq(1).find("input").change(function(evt){
			data.$img.attr("alt",this.value);
		}).focus(this.focusSelect);
		//���������
		$tpl.eq(2).find("input").change(function(evt){
			data.$obj.attr("href",this.value);
		}).focus(this.focusSelect);
	};

};
/**
 * ɾ��Ԫ��
 * @param {Object} data
 */
sohu.diyChipEditor.Dialog.prototype.DelElm=function(data){
	if(data.$obj){
		data.$obj.remove();
		data.$tpl.remove();
	};
};
/**
 * �Ӵ�����
 * @param {Object} data
 */
sohu.diyChipEditor.Dialog.prototype.Bold=function(data){
	if(!data.$obj) return false;
	if(data.$obj.hasClass(this._opts.clBold)){
		data.$obj.removeClass(this._opts.clBold).css("font-weight","normal");
		data.$tpl.eq(0).find("input").removeClass(this._opts.clBold);
	}else{
		data.$obj.addClass(this._opts.clBold).css("font-weight","bold");
		data.$tpl.eq(0).find("input").addClass(this._opts.clBold);
	};
};
/**
 * ��ɫ
 * @param {Object} data
 */
sohu.diyChipEditor.Dialog.prototype.Color=function(data){
	if(!data.$obj) return false;
	sohu.diyDialog.showColorPicker({
		onSubmit:function(c){
			data.$obj.css("color",c);	
		},
		onChange:function(c){
			data.$obj.css("color",c);
		}
	});
};
/**
 * ���Ԫ��
 * @param {Object} data
 */
sohu.diyChipEditor.Dialog.prototype.AddElm=function(data){
	if(!data.$obj) return false;
	var txt0=$(document.createTextNode(" "));
	var a0=$(sohu.diyChipEditor.aTplStr);
	data.$obj.after(a0).after(txt0);
	a0.bind('click.noNav',sohu.diyChipEditor.StopNav);
	
	var tpl=sohu.diyChipEditor.$RowTxtA.clone();
	var data1={$obj:txt0,$tpl:tpl};
	//����������¼�(�հ���Ӧ��)
	tpl.find("input").val(" ").bind("keyup",function(evt){
		var obj0=$(document.createTextNode(this.value));
		data1.$obj.replaceWith(obj0);
		data1.$obj=obj0;
	}).focus(this.focusSelect);
	tpl.find(".btnDel").bind("click",function(evt){
		data1.$obj.remove();
		tpl.remove();
	});
	var tpl1=$([sohu.diyChipEditor.$RowTitleA.clone()[0],sohu.diyChipEditor.$RowLnkA.clone()[0]]);
	//������������
	tpl1.eq(0).find("input").val(a0.html());
	tpl1.eq(1).find("input").val(a0.attr("href"));
	var data2={$obj:a0,$tpl:tpl1,t:0};
	//ͼ���¼�����
	this.BindIconEvts(tpl1,data2);
	
	data.$tpl.eq(1).after(tpl1).after(tpl);
	return false;
};
sohu.diyChipEditor.Dialog.prototype.focusSelect=function(evt){
	$(this).select();
};
/**
 * ������ƵСͼƬ
 * @param {Object} data
 */
sohu.diyChipEditor.Dialog.prototype.InsertVDIcon=function(data){
	var _this=this;
	var a=$(sohu.diyChipEditor.vdTplStr).bind("click.noNav",sohu.diyChipEditor.StopNav).bind('click.edit',function(evt){
		_this.Edit($(this));
	});
	if(data.before){
		data.$obj.before(a);
	}else{
		data.$obj.after(a);
	};
	return false;
};
