/**
 * JS interactive logic for editing fragments
 * ��Ƭ�༭��
 * @author levinhuang
 */

sohu.diyChipEditor = function() {
    var p={},pub={};
	p.editors={};
	/* �¼����� */	
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
		sohu.diyChipEditor.moreStr=opts.moreStr||'<strong class="elm more"><a href="#">����&gt;&gt;</a></strong>';
		p._dlgModel=$(".chipEdt");
		p._$body=$("body");
		p._singleton=opts.singleton||false;/* ���õ���ģʽ��ʾ��Ƭ�༭�� */
	};
    p.onLoaded = function() { 
		p.initTinyMCE();
	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		//$(".acts a,button").button();
		p._dlgModel.find("a").not(".cmdicon").bind('click.noNav',sohu.diyConsole.OnStopNav);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
	/**
	 * ��ʾ��Ƭ�༭��
	 * @param {Object} $chip ��Ƭ��dom����
	 * @param {Object} opts ����ѡ��
	 */
	pub.Show=function($chip,opts){
		opts=$.extend({tabs:[0,1,2]},opts||{});
		var dlg=null,id=p._singleton?"solo":$chip.attr("id");
		opts.isNew=false;
		opts=$.extend({
			dlgModel:p._dlgModel,
			$body:p._$body,
			$chip:$chip,				/* ��Ƭ */
			$elm:opts.$elm,				/* Ԫ�� */
			elm:opts.elm,				/* sohu.diyElement���� */
			onUpPic:opts.onUpPic,
			onTest:opts.onTest,
			onSave:opts.onSave,
			onCancel:opts.onCancel,
			onGlobalRes:opts.onGlobalRes,
			onExternal:opts.onExternal,
			onFlashEdit:opts.onFlashEdit,
			onLoadHis:opts.onLoadHis								
		},opts);		
		if(!(dlg=p.editors[id])){
			opts.isNew=true;
			dlg=new sohu.diyChipEditor.Dialog(opts);
			p.editors[id]=dlg;
			
		};
		//dlg.Show(opts);
		dlg.Edit(opts.$elm,opts);
	};
	pub.MCE=function(){
		return tinymce.activeEditor;
	};
	pub.$ElmChipTpl=$("#elmChipTpl");
	pub.$ElmATpl=$("#elmATpl");
	pub.$ElmImgTpl=$("#elmImgTpl");
	pub.$ElmTxtTpl=$("#elmTxtTpl");
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
	this.Elm=opts.elm;//��ǰԪ�ض�Ӧ��sohu.diyElement����
	this.$Chip=opts.$chip;//��ǰ��Ƭ
	this.SortElm=null;//��ǰ����Ԫ��
	
	//DOM����
	this.$Layout=opts.dlgModel.clone().appendTo(opts.$body);
	this.$CmdItems=this.$Layout.find(".cmdicon");
	this.$ElmcActs=this.$Layout.find(".elmcActs");
	this.$ElmTpl=this.$Layout.find(".elmTpl");
	this.$ChipTpl=this.$Layout.find(".chipTpl");
	this.$SecHDActs=this.$Layout.find(".secHDActs");
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
	
	//�¼�����
	this.$Backup=this.$Chip.clone(true);
	
	this.InitSecHDTpl();

	//�������
	this.$BtnTest=this.$Layout.find(".test");
	//������Ƭ
	this.$BtnSave=this.$Layout.find(".save");
	//ȡ��
	this.$BtnCancel=this.$Layout.find(".cancel").click(function(evt){
		var c=_this.$Backup.clone(true);
		_this.$Chip.replaceWith(c);
		_this.$Chip=c;
		_this.$Layout.jqmHide();
		return false;
	});
	//Ԫ��ɾ����ť
	this.$BtnElmDel=this.$CmdItems.filter(".elmDel1").bind("click",function(e){
		if(!sohu.diyConsole.CurElm) return;
		//�رձ༭�Ի���
		_this.Hide();
		//ɾ��Ԫ��
		sohu.diyConsole.CurElm.$CopyModel.remove();	
		return false;	
	});
	//ͳһ��Դ��
	this.$BtnGlobalRes=this.$Layout.find(".globalRes");
	//���
	this.$BtnExternal=this.$Layout.find(".external");
	//flash�༭
	this.$BtnFlashEdit=this.$Layout.find(".btnFlashEdit");
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
	this.$BtnUpPic=this.$UpPic.find(".up");
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
	this.$Layout.draggable({handle:".hd",containment:'document'});
	//margin-left
	this.$Layout.css("margin-left",-(this.$Layout.width()/2));
	//����diyConsole��evtPreview�¼�
	if(bos){
		$(bos).bind("evtPreview",function(e){_this.Hide();});
	};
	//����window��scroll�¼�
	sohu.diyConsole.FixDraggable(this.$Layout);
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
	
	//�����ܰ�ť�Ļص�
	if(opts.onSave){
		this.$BtnSave.unbind("click.a").bind("click.a",function(evt){
			opts.onSave(_this);
			return false;
		});
	};
	if(opts.onCancel){
		this.$BtnCancel.unbind("click.a").bind("click.a",function(evt){
			opts.onCancel(_this);
			return false;
		});
	};	
	if(opts.onUpPic){
		this.$BtnUpPic.unbind("click.a").bind("click.a",function(evt){
			opts.onUpPic(_this);
			return false;
		});
	};
	
	this.$Chip.addClass("on");
	//��ʾ����	
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
		/*
		//�Ƴ�click.sort�¼�
		this.$Chip.find("a").unbind("click.sort");
		*/
		//����"���ӻ��޸�"��"�����޸�"(����Ԫ�ط����˱仯)
		this.$ElmA.empty().html(this._opts.lblTab0);
		this.$ElmImg.empty().html(this._opts.lblTab0);
	}else{
		$i.prev().trigger("click");
		$i.addClass("btnSort1").find("span").html(this._opts.lblSort1);
		this.$CT.tabs("option","disabled",[0,1,2]);
		this.Sorting=true;
		/*
		//��a��ǩ��click.sort�¼�����
		this.$Chip.find("a").bind("click.sort",function(evt){
			_this.Sort($(this));
		});
		*/
	};
};
/**
 * �༭ĳ��Ԫ��
 * @param {Object} $elm
 */
sohu.diyChipEditor.Dialog.prototype.Edit=function($elm,opts){
	opts=opts||{};
	var _this=this,_afterShow=opts.afterShow,_afterHide=opts.afterHide;
	//��Ԫ���Ƿ��ڱ༭��
	if(opts.elm.IsEditing) return;
	
	var $elmList=$elm.contents();
	//�������õĵ�ǰԪ��
	this.Elm=opts.elm;
	this.$Elm=$elm;
	this.$Chip=opts.$chip;

	opts.afterShow=function(hash,dlg){	
		//��һ��tab
		if(!_this.$Chip.is(".sec_hd")){
			//����Ŀ����
			_this.$ChipTpl.hide();
			_this.$SecHDActs.hide();
		}else{
			//��Ŀ����
			_this.LoadSecHDTpl();
		};
		

				
		dlg.$ElmTpl.hide().empty();
		
		$elmList.each(function(i,o){
			var $o=$(o);
			var tpl=null;
			if(o.nodeType!=1){
				//�ı�
				tpl=$(sohu.diyChipEditor.$ElmTxtTpl.html());
				dlg.$ElmTpl.append(tpl);
				var data={$obj:$o,$tpl:tpl};
				tpl.find(".txt").val($o.text()).bind("keyup",function(evt){
					var obj0=$(document.createTextNode(this.value));
					data.$obj.replaceWith(obj0);
					data.$obj=obj0;
					_this.onDomRefreshed();
				}).focus(_this.focusSelect);
				//ɾ����ť
				tpl.find(".btnDel").bind("click",function(evt){
					data.$obj.remove();
					tpl.remove();
					_this.onDomRefreshed();
				});
			}else if($o.is("a")){
				var $img=$o.find("img");
				if($img.length>0){
					//ͼƬ����
					tpl=$(sohu.diyChipEditor.$ElmImgTpl.html());
					_this.$ElmTpl.append(tpl);
					_this.BindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:1,$img:$img});								
				}else{
					tpl=$(sohu.diyChipEditor.$ElmATpl.html());
					dlg.$ElmTpl.append(tpl);
					_this.BindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:0});							
				};
			};
	
		});
		_this.$ElmTpl.show();
		//�ڶ���tab
		dlg.UpdateCode();
		//�ϴ���ť
		dlg.$Layout.find(".btnUpl").click(function(evt){
			dlg.$UpPic.show().effect("highlight");
			return false;
		});
		
		//afterShow�û��ص�
		if(_afterShow)
			_afterShow(hash,dlg);
					
	};//onShow
	opts.afterHide=function(hash,dlg){		
		dlg.Elm=null;
		dlg.$Elm=null;
		//afterHide�û��ص�
		if(_afterHide)
			_afterHide(hash,dlg);
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

	//���´���textarea
	this.UpdateCode();
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
	//�»���
	$tpl.find(".udl").bind("click",function(evt){
		_this.Underline(data);return false;
	});
	//��ɫ
	$tpl.find(".color").bind("click",function(evt){
		_this.Color(data);return false;
	});
	if(data.t==0){
		//���������
		$tpl.find(".tt").val(data.$obj.html()).keyup(function(evt){
			data.$obj.html(this.value);
			_this.onDomRefreshed();
		}).focus(this.focusSelect);
		//���������
		$tpl.find(".lnk").val(data.$obj.attr("href")).keyup(function(evt){
			data.$obj.attr("href",this.value);
		}).focus(this.focusSelect);
		//���Ӵ򿪷�ʽ
		$tpl.find(".aTarget").val(data.$obj.attr("target")).change(function(evt){
			data.$obj.attr("target",$(this).val());
		});
		//����ͼ��
		$tpl.find('.icon').click(function(evt){
			$(this).next().toggle();
			return false;
		});
		$tpl.find(".others").mouseleave(function(evt){$(this).hide();});
		//��Ƶǰ
		$tpl.find(".videoL").bind("click",function(evt){
			_this.InsertVDIcon({$obj:data.$obj,before:true});return false;
		});
		//��Ƶ��
		$tpl.find(".videoR").bind("click",function(evt){
			_this.InsertVDIcon({$obj:data.$obj,before:false});return false;
		});			
	}else{
		//ͼƬ��ַ�����
		$tpl.find(".imgSrc").val(data.$img.attr("src")).change(function(evt){
			data.$img.attr("src",this.value);
		}).focus(this.focusSelect);
		//ALT�����
		$tpl.find(".imgAlt").val(data.$img.attr("alt")).change(function(evt){
			data.$img.attr("alt",this.value);
		}).focus(this.focusSelect);
		//���������
		$tpl.find(".imgLnk").val(data.$obj.attr("href")).change(function(evt){
			data.$obj.attr("href",this.value);
		}).focus(this.focusSelect);
		//���Ӵ򿪷�ʽ
		$tpl.find(".imgTarget").val(data.$obj.attr("target")).change(function(evt){
			data.$obj.attr("target",$(this).val());
		});		
		//Title
		$tpl.find(".imgTitle").val(data.$img.attr("title")).change(function(evt){
			data.$img.attr("title",this.value);
		});
		//��
		$tpl.find(".imgW").val(data.$img.width()).change(function(evt){
			if(this.value==""){
				data.$img.removeAttr("width");
				_this.onDomRefreshed();
				return;
			};	
			if(!StringUtils.isPlusInt(this.value)){
				$(this).addClass("alert");
				return;
			};
			$(this).removeClass("alert");
			data.$img.attr("width",this.value);
			_this.onDomRefreshed();
		});
		//��
		$tpl.find(".imgH").val(data.$img.height()).change(function(evt){
			if(this.value==""){
				data.$img.removeAttr("height");
				_this.onDomRefreshed();
				return;
			};	
			if(!StringUtils.isPlusInt(this.value)){
				$(this).addClass("alert");
				return;
			};
			$(this).removeClass("alert");
			data.$img.attr("height",this.value);
			_this.onDomRefreshed();
		});
		//�߿�ɫ
		$tpl.find(".imgBColor").css("background",data.$img.css("border-color")).click(function(evt){
			var $i=$(this);
			sohu.diyDialog.showColorPicker({
				onSelect:function(c){
					$i.css("background",c);
					data.$img.css("border-color",c);
				}
			});
		});		
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
		this.onDomRefreshed();
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
 * ���»���
 * @param {Object} data
 */
sohu.diyChipEditor.Dialog.prototype.Underline=function(data){
	if(!data.$obj) return false;
	if (data.$obj.css("text-decoration") == "underline") {
		data.$obj.css("text-decoration", "none");
	}else {
		data.$obj.css("text-decoration", "underline");
	};
};
/**
 * ��ɫ
 * @param {Object} data
 */
sohu.diyChipEditor.Dialog.prototype.Color=function(data){
	if(!data.$obj) return false;
	sohu.diyDialog.showColorPicker({
		onSelect:function(c){
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
	var _this=this;
	var a0=$(sohu.diyChipEditor.aTplStr);
	data.$obj.after(a0).after(txt0);
	a0.bind('click.noNav',sohu.diyConsole.OnStopNav);
	
	var tpl=$(sohu.diyChipEditor.$ElmTxtTpl.html());
	var data1={$obj:txt0,$tpl:tpl};
	//����������¼�(�հ���Ӧ��)
	tpl.find("input").val(" ").bind("keyup",function(evt){
		var obj0=$(document.createTextNode(this.value));
		data1.$obj.replaceWith(obj0);
		data1.$obj=obj0;
		_this.onDomRefreshed();
	}).focus(this.focusSelect);
	tpl.find(".btnDel").bind("click",function(evt){
		data1.$obj.remove();
		tpl.remove();
		_this.onDomRefreshed();
	});
	var tpl1=$(sohu.diyChipEditor.$ElmATpl.html());
	var data2={$obj:a0,$tpl:tpl1,t:0};
	//ͼ���¼�����
	this.BindIconEvts(tpl1,data2);
	
	data.$tpl.filter(":last").after(tpl1).after(tpl);
	this.onDomRefreshed();
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
	var a=$(sohu.diyChipEditor.vdTplStr).bind("click.noNav",sohu.diyConsole.OnStopNav);
	if(data.before){
		data.$obj.before(a);
	}else{
		data.$obj.after(a);
	};
	this.onDomRefreshed();
	return false;
};
/**
 * ͬ����ǰ��Ƭ��Html������༭�ı���
 */
sohu.diyChipEditor.Dialog.prototype.UpdateCode=function(){
	sohu.diyChipEditor.MCE().setContent(this.$Chip.html());
	this.$Code.val(sohu.diyChipEditor.MCE().getContent());
};
/**
 * ���ر�����Ŀ�༭ģ��
 */
sohu.diyChipEditor.Dialog.prototype.LoadSecHDTpl=function(){
	this.$ChipTpl.show();
	this.$SecHDActs.show();
	this.$CbxSecHDMore[0].checked=(this.$Chip.find(".more").length>0);
	
	var bgimg=this.$Chip.css("background-image");
	if(bgimg!=""&&bgimg!="none"){
		this.$SecHDBG.val(bgimg.replace('url("',"").replace('")',""));
	};
};
/**
 * ��ʼ��������Ŀ�༭ģ��
 */
sohu.diyChipEditor.Dialog.prototype.InitSecHDTpl=function(){
	var _this=this;
	//���ఴť
	this.$CbxSecHDMore=this.$SecHDActs.find(".secHDMore").click(function(evt){
		if(this.checked){
			if(sohu.diyConsole.CurElm.CT.$Layout.find(".more").length==0){
				var $more=$(sohu.diyChipEditor.moreStr);
				sohu.diyConsole.CurElm.CT.$Layout.append($more);
				new sohu.diyElement({ct:sohu.diyConsole.CurElm.CT,$dom:$more});
			};
		}else{
			sohu.diyConsole.CurElm.CT.$Layout.find(".more").remove();
		};
	});
	//����ͼ����
	this.$SecHDBG=this.$ChipTpl.find(".chipBG").change(function(evt){
		if(this.value==""){
			_this.$Chip.css("background-image","none");
			return;
		};
		_this.$Chip.css("background-image","url('"+this.value+"')");
	});
	this.$ChipTpl.find(".chipBGAlign").change(function(evt){
		_this.$Chip.css("background-repeat",this.value);
	});
	this.$ChipTpl.find(".btnUpl").click(function(evt){
		if(_this.onUpChipBG){
			_this.onUpChipBG(_this);
		};
		return false;
	});
};
/**
 * onDomRefreshed�¼�������Ƭ�༭���༭ʱ�������Ƭdomʱ����
 */
sohu.diyChipEditor.Dialog.prototype.onDomRefreshed=function(){
	if(!this.Elm) return;
	this.Elm.CT.Editor.Reposition();
};
