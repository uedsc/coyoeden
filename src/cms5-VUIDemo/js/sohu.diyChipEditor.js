/**
 * JS interactive logic for editing fragments
 * 碎片编辑器
 * @author levinhuang
 * @revision 2010.10.22 
 * 	所有class加vsp_前缀;加tinyMCE开关
 */

sohu.diyChipEditor = function() {
    var p={},pub={};
	p.editors={};
	/* 事件处理 */	
	p.initTinyMCE=function(){
		/*这种异步加载tiny_mce.js的方式在ie下有问题，需要移到chipEditor.js外面。否则需要用原生的tinymce.init方法。
		p._$mce=$("#tinymce1").tinymce({
			script_url:'editor/tiny_mce/tiny_mce.js',
			theme:'simple'
		}).hide();
		*/
		if(p._noTinyMCE) return;
		tinymce.init({
			mode:'specific_textareas',
			editor_selector:'vstp_mceEditor',
			theme:'simple',
			oninit:function(){
				tinymce.activeEditor.hide();
			}
		});
	};
    //private area
    p.initVar = function(opts) {
		p._cssBold=opts.cssBold||'vsp_bb';
		p._cssColor=opts.cssColor||'vsp_cc';
		sohu.diyChipEditor.aTplStr=opts.aTplStr||'<a href="http://sohu.com" title="">sohu</a>';
		sohu.diyChipEditor.vdTplStr=opts.vdTplStr||'<a href="http://sohu.com" title=""><img src="http://images.sohu.com/uiue/vd.gif" alt="Video"/></a>';
		sohu.diyChipEditor.moreStr=opts.moreStr||'<strong class="vstp_elm more"><a href="#">更多&gt;&gt;</a></strong>';
		p._dlgModel=$(".vstp_chipEdt");
		p._$body=$("body");
		p._singleton=opts.singleton||false;/* 采用单件模式显示碎片编辑器 */
		p._noTinyMCE=opts.noTinyMCE||false;
	};
    p.onLoaded = function() { 
		p.initTinyMCE();
	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		//$(".acts a,button").button();
		p._dlgModel.find("a").not(".vstp_cmdicon").bind('click.noNav',sohu.diyConsole.OnStopNav);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
	/**
	 * 显示碎片编辑器
	 * @param {Object} $chip 碎片的dom对象
	 * @param {Object} opts 其他选项
	 */
	pub.Show=function($chip,opts){
		opts=$.extend({tabs:[0,1,2]},opts||{});
		var chipID=$chip.attr("id");
		var dlg=null,id=p._singleton?"solo":chipID;
		opts.isNew=false;
		opts=$.extend(opts,{
			dlgModel:p._dlgModel,
			$body:p._$body,
			$chip:$chip,				/* 碎片 */
			$elm:opts.$elm,				/* 元素 */
			elm:opts.elm,				/* sohu.diyElement对象 */
			onUpPic:opts.onUpPic,
			onTest:opts.onTest,
			onSave:opts.onSave,
			onCancel:opts.onCancel,
			onGlobalRes:opts.onGlobalRes,
			onExternal:opts.onExternal,
			onFlashEdit:opts.onFlashEdit,
			onLoadHis:opts.onLoadHis								
		});		
		if(!(dlg=p.editors[id])){
			opts.isNew=true;
			dlg=new sohu.diyChipEditor.Dialog(opts);
			p.editors[id]=dlg;				
		};
		dlg.Edit(opts.$elm,opts);
		//编辑器定位在碎片上
		if(dlg.ChipID!=chipID){
			var of=$chip.offset();
			dlg.$Layout.css({
				top:of.top,
				left:of.left,
				"margin-left":"auto"
			});
			dlg.ChipID=chipID;					
		};		
	};
	pub.MCE=function(){
		if(p._noTinyMCE){
			//不用tinymce,为了减少改动代码,伪造一个tinyMCE
			if(!p._fakeMCE){
				p._fakeMCE={
					html:"",
					setContent:function(html){
						this.html=html;
					},
					getContent:function(){
						return this.html;
					}
				};
			};
			return p._fakeMCE; 
		};
		return tinymce.activeEditor;
	};
	pub.$ElmChipTpl=$("#vstp_elmChipTpl");
	pub.$ElmATpl=$("#vstp_elmATpl");
	pub.$ElmImgTpl=$("#vstp_elmImgTpl");
	pub.$ElmTxtTpl=$("#vstp_elmTxtTpl");
    return pub;
} ();
/**
 * 碎片编辑器弹框类
 * @param {Object} opts
 */
sohu.diyChipEditor.Dialog=function(opts){
	opts=$.extend({},{
		tabIDTpl:'wVsp_tab',
		lblSort:'排序状态',
		lblSort1:'结束排序',
		lblTab0:'<strong class="vstp_alert">行修改状态</strong><em>请选择行</em>',
		clBold:'vstp_bb',
		clColor:'vstp_cc',
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
	//设置编辑器的目标碎片
	this.SetTarget(opts.$chip,opts.$elm,opts);
	//当前排序元素
	this.SortElm=null;
	
	//DOM引用
	this.$Layout=opts.dlgModel.clone().appendTo(opts.$body);
	this.$CmdItems=this.$Layout.find(".vstp_cmdicon");
	this.$ElmcActs=this.$Layout.find(".vstp_elmcActs");
	this.$ElmTpl=this.$Layout.find(".vstp_elmTpl");
	this.$ChipTpl=this.$Layout.find(".vstp_chipTpl");
	this.$SecHDActs=this.$Layout.find(".vstp_secHDActs");
	//初始化文本命令菜单
	new sohu.diyMenuBar({$cmdItems:this.$CmdItems,onDel:function(){
		_this.Hide();
	}});
	sohu.diyChipEditor.MCE().setContent(this.$Chip.html());
	this.$Code=this.$Layout.find('.vstp_txtVspC').val(sohu.diyChipEditor.MCE().getContent());
	/*
	this.$Layout.find('.txtVspCS').textareaSearch({
		cssTextArea:_this.$Code,
		cssBtn:_this.$Layout.find('.btnVspCS')
	});
	*/
	this.$btnCode=this.$Layout.find(".vstp_external");//.globalRes,
	
	//事件处理
	this.InitSecHDTpl();

	//整体测试
	this.$BtnTest=this.$Layout.find(".vstp_test");
	//保存碎片
	this.$BtnSave=this.$Layout.find(".vstp_save");
	//取消
	this.$BtnCancel=this.$Layout.find(".vstp_cancel").click(function(evt){
		/* 注：如果页面中存在prototype库，用replaceWith可能会出现bug。到时需要参照chipEditor.js进行修改 */
		/*
		var c=_this.$Backup.clone(true);
		_this.$Chip.replaceWith(c);
		_this.CT.$Layout=_this.$Chip=c;
		*/
		sohu.diyConsole.CTEditor.Detach();
		_this.$Chip.html(_this.$Backup.html());
		_this.CT.LoadElements();		
		
		_this.Hide();
		//重定位分栏编辑器
		_this.CT.Editor.Reposition();		
		return false;
	});
	//元素删除按钮
	this.$BtnElmDel=this.$CmdItems.filter(".vstp_elmDel1").bind("click",function(e){
		if(!sohu.diyConsole.CurElm) return;
		//关闭编辑对话框
		_this.Hide();
		//删除元素
		sohu.diyConsole.CurElm.$CopyModel.remove();
		//重定位分栏编辑器
		sohu.diyConsole.CurElm.CT.Editor.Reposition();			
		return false;	
	});
	//统一资源库
	this.$BtnGlobalRes=this.$Layout.find(".vstp_globalRes");
	//外包
	this.$BtnExternal=this.$Layout.find(".vstp_external");
	//flash编辑
	this.$BtnFlashEdit=this.$Layout.find(".vstp_btnFlashEdit");
	//tab菜单
	var tabID="tab"+StringUtils.RdStr(8);
	this.$TabM=this.$Layout.find(".vstp_tabM").each(function(i,o){
		var $o=$(o);
		$o.attr("href",$o.attr("href").replace(opts.tabIDTpl,tabID));
	});
	//tab内容
	this.$TabC=this.$Layout.find(".vstp_tabC").each(function(i,o){
		var $o=$(o);
		$o.attr("id",$o.attr("id").replace(opts.tabIDTpl,tabID));
	});
	//排序按钮
	this.$Layout.find('.vstp_btnSort').click(function(evt){
		_this.ToggleSorting($(this));return false;
	});
	//tab
	this.$CT=this.$Layout.find(".vstp_jqmCT");
	/*
	this.$CT=this.$Layout.find(".vstp_jqmCT").tabs({
		select:function(evt,ui){
			//按钮的显隐
			if(ui.index==1){
				_this.$btnCode.removeClass("hide");
			}else{
				_this.$btnCode.addClass("hide");
			};
			//修改记录
			if(ui.index==2){
				if(opts.onLoadHis){
					opts.onLoadHis(_this);
				};
			};
		}
	});
	*/
	//上传浮层
	this.$UpPic=this.$CT.find(".vstp_uppic").draggable({handle:".vstp_upp_t",containment:'parent',axis:'y'});
	this.$BtnUpPic=this.$UpPic.find(".vstp_up");
	this.$UpPic.find(".vstp_cls").click(function(evt){
		_this.$UpPic.slideUp("fast");
	});
	//jqm options
	this.jqmOpts={trigger:false,modal:false,overlay:false,autoFocus:false,beforeShow:null,afterShow:null,beforeHide:null,afterHide:null,overlayClass:"cedt_jqmovl",closeClass:'cedt_jqmcls'};
	this.jqmOpts.onShow=function(hash){
		var doShow=true;
		if(_this.jqmOpts.beforeShow){
			doShow=_this.jqmOpts.beforeShow(hash,_this);
		};
		if(doShow){
			hash.w.show();
			_this.jqmHash=hash;
			//清除排序状态
			if(_this.Sorting){
				_this.$BtnSort.trigger("click");
			};
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
			_this.$Chip.removeClass("vstp_on");
			//重置第一个tab的内容
			_this.$ElmTpl.empty();
			//重置命令按钮样式
			_this.$CmdItems.removeClass("vstp_editBtn1");
			if(_this.jqmOpts.afterHide)
			{
				_this.jqmOpts.afterHide(hash,_this);
			};
		};	
	};
	
	//draggable
	this.$Layout.draggable({handle:".vstp_hd",containment:'document'});
	//margin-left
	this.$Layout.css("margin-left",-(this.$Layout.width()/2));
	//订阅diyConsole的evtPreview事件
	if(bos){
		$(bos).bind("evtPreview",function(e){_this.Hide();});
	};
	//订阅window的scroll事件
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
	
	//各功能按钮的回调
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
	
	this.$Chip.addClass("vstp_on");
	//显示弹框	
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
		$i.removeClass("vstp_btnSort1").find("span").html(this._opts.lblSort);
		this.$CT.tabs("option","disabled",[]);
		this.$CT.tabs("option","selected",0);
		
		this.Sorting=false;
		/*
		//移除click.sort事件
		this.$Chip.find("a").unbind("click.sort");
		*/
		//重置"可视化修改"和"代码修改"(由于元素发生了变化)
		this.$ElmA.empty().html(this._opts.lblTab0);
		this.$ElmImg.empty().html(this._opts.lblTab0);
	}else{
		$i.prev().trigger("click");
		$i.addClass("vstp_btnSort1").find("span").html(this._opts.lblSort1);
		this.$CT.tabs("option","disabled",[0,1,2]);
		this.Sorting=true;
		/*
		//绑定a标签的click.sort事件处理
		this.$Chip.find("a").bind("click.sort",function(evt){
			_this.Sort($(this));
		});
		*/
	};
};
/**
 * 编辑某个元素
 * @param {Object} $elm
 */
sohu.diyChipEditor.Dialog.prototype.Edit=function($elm,opts){
	opts=opts||{};
	var _this=this,_afterShow=opts.afterShow,_afterHide=opts.afterHide;
	//改元素是否在编辑中
	if(opts.elm.IsEditing) return;
	
	var $elmList=$elm.contents();
	//更新碎片目标
	this.SetTarget(opts.$chip,$elm,opts);

	opts.afterShow=function(hash,dlg){	
		//第一个tab
		if(!_this.$Chip.is(".vstp_sec_hd")){
			//非栏目标题
			_this.$ChipTpl.hide();
			_this.$SecHDActs.hide();
		}else{
			//栏目标题
			_this.LoadSecHDTpl();
		};
		

				
		dlg.$ElmTpl.hide().empty();
		
		$elmList.each(function(i,o){
			var $o=$(o);
			var tpl=null;
			if(o.nodeType!=1){
				//文本
				tpl=$(sohu.diyChipEditor.$ElmTxtTpl.html());
				dlg.$ElmTpl.append(tpl);
				var data={$obj:$o,$tpl:tpl};
				tpl.find(".vstp_txt").val($o.text()).bind("keyup",function(evt){
					var obj0=$(document.createTextNode(this.value));
					data.$obj.replaceWith(obj0);
					data.$obj=obj0;
					_this.onDomRefreshed();
				}).focus(_this.focusSelect);
				//删除按钮
				tpl.find(".vstp_btnDel").bind("click",function(evt){
					data.$obj.remove();
					tpl.remove();
					_this.onDomRefreshed();
					return false;
				});
			}else if($o.is("a")){
				var $img=$o.find("img");
				if($img.length>0){
					//图片链接
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
		//第二个tab
		dlg.UpdateCode();
		//上传按钮
		dlg.$Layout.find(".vstp_btnUpl").click(function(evt){
			dlg.$UpPic.show().effect("highlight");
			//设定当前上传的图片对象2010.10.15
			dlg.$UpPic.data=jQuery(this).data("data");			
			return false;
		});
		
		//afterShow用户回调
		if(_afterShow)
			_afterShow(hash,dlg);
					
	};//onShow
	opts.afterHide=function(hash,dlg){		
		dlg.Elm=null;
		dlg.$Elm=null;
		//afterHide用户回调
		if(_afterHide)
			_afterHide(hash,dlg);
	};//onHide
	this.Show(opts);
};
/**
 * 排序
 * @param {Object} $i
 */
sohu.diyChipEditor.Dialog.prototype.Sort=function($i){
	if($i.hasClass("vstp_sort")) return;
	if(!this.SortElm){
		this.SortElm=$i.addClass("vstp_sort");
		return;
	};
	//调换内容
	var temp=this.SortElm.html();
	this.SortElm.html($i.html());
	$i.html(temp);
	
	//结束本次操作
	this.SortElm.removeClass("vstp_sort");
	this.SortElm=null;

	//更新代码textarea
	this.UpdateCode();
};
/**
 * 图标事件处理
 * @param {Object} $tpl
 * @param {Object} data
 */
sohu.diyChipEditor.Dialog.prototype.BindIconEvts=function($tpl,data){
	var _this=this;
	//删除按钮
	$tpl.find(".vstp_btnDel").bind("click",function(evt){
		_this.DelElm(data);return false;
	});
	//复制按钮
	$tpl.find(".vstp_btnAdd").bind("click",function(evt){
		_this.AddElm(data);return false;
	});
	//加粗
	$tpl.find(".vstp_bold").bind("click",function(evt){
		_this.Bold(data);return false;
	});
	//下划线
	$tpl.find(".vstp_udl").bind("click",function(evt){
		_this.Underline(data);return false;
	});
	//颜色
	$tpl.find(".vstp_color").bind("click",function(evt){
		_this.Color(data);return false;
	});
	if(data.t==0){
		//标题输入框
		$tpl.find(".vstp_tt").val(data.$obj.html()).keyup(function(evt){
			data.$obj.html(this.value);
			_this.onDomRefreshed();
		}).focus(this.focusSelect);
		//链接输入框
		$tpl.find(".vstp_lnk").val(data.$obj.attr("href")).keyup(function(evt){
			data.$obj.attr("href",this.value);
		}).focus(this.focusSelect);
		//链接打开方式
		$tpl.find(".vstp_aTarget").val(data.$obj.attr("target")).change(function(evt){
			data.$obj.attr("target",$(this).val());
		});
		//其他图标
		$tpl.find('.vstp_icon').click(function(evt){
			$(this).next().toggle();
			return false;
		});
		$tpl.find(".vstp_others").mouseleave(function(evt){$(this).hide();});
		//视频前
		$tpl.find(".vstp_videoL").bind("click",function(evt){
			_this.InsertVDIcon({$obj:data.$obj,before:true});return false;
		});
		//视频后
		$tpl.find(".vstp_videoR").bind("click",function(evt){
			_this.InsertVDIcon({$obj:data.$obj,before:false});return false;
		});			
	}else{
		//图片地址输入框
		$tpl.find(".vstp_imgSrc").val(data.$img.attr("src")).change(function(evt){
			data.$img.attr("src",this.value);
		}).focus(this.focusSelect);
		//ALT输入框
		$tpl.find(".vstp_imgAlt").val(data.$img.attr("alt")).change(function(evt){
			data.$img.attr("alt",this.value);
		}).focus(this.focusSelect);
		//链接输入框
		$tpl.find(".vstp_imgLnk").val(data.$obj.attr("href")).change(function(evt){
			data.$obj.attr("href",this.value);
		}).focus(this.focusSelect);
		//链接打开方式
		$tpl.find(".vstp_imgTarget").val(data.$obj.attr("target")).change(function(evt){
			data.$obj.attr("target",$(this).val());
		});		
		//Title
		$tpl.find(".vstp_imgTitle").val(data.$img.attr("title")).change(function(evt){
			data.$img.attr("title",this.value);
		});
		//宽
		$tpl.find(".vstp_imgW").val(data.$img.width()).change(function(evt){
			if(this.value==""){
				data.$img.removeAttr("width");
				_this.onDomRefreshed();
				return;
			};	
			if(!StringUtils.isPlusInt(this.value)){
				$(this).addClass("vstp_alert");
				return;
			};
			$(this).removeClass("vstp_alert");
			data.$img.attr("width",this.value);
			_this.onDomRefreshed();
		});
		//高
		$tpl.find(".vstp_imgH").val(data.$img.height()).change(function(evt){
			if(this.value==""){
				data.$img.removeAttr("height");
				_this.onDomRefreshed();
				return;
			};	
			if(!StringUtils.isPlusInt(this.value)){
				$(this).addClass("vstp_alert");
				return;
			};
			$(this).removeClass("vstp_alert");
			data.$img.attr("height",this.value);
			_this.onDomRefreshed();
		});
		//边框色
		$tpl.find(".vstp_imgBColor").css("background",data.$img.css("border-color")).click(function(evt){
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
 * 删除元素
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
 * 加粗字体
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
 * 加下划线
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
 * 着色
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
 * 添加元素
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
	//文字输入框事件(闭包的应用)
	tpl.find("input").val(" ").bind("keyup",function(evt){
		var obj0=$(document.createTextNode(this.value));
		data1.$obj.replaceWith(obj0);
		data1.$obj=obj0;
		_this.onDomRefreshed();
	}).focus(this.focusSelect);
	tpl.find(".vstp_btnDel").bind("click",function(evt){
		data1.$obj.remove();
		tpl.remove();
		_this.onDomRefreshed();
	});
	var tpl1=$(sohu.diyChipEditor.$ElmATpl.html());
	var data2={$obj:a0,$tpl:tpl1,t:0};
	//图标事件处理
	this.BindIconEvts(tpl1,data2);
	
	data.$tpl.filter(":last").after(tpl1).after(tpl);
	this.onDomRefreshed();
	return false;
};
sohu.diyChipEditor.Dialog.prototype.focusSelect=function(evt){
	$(this).select();
};
/**
 * 插入视频小图片
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
 * 同步当前碎片的Html到代码编辑文本框
 */
sohu.diyChipEditor.Dialog.prototype.UpdateCode=function(){
	sohu.diyChipEditor.MCE().setContent(this.$Chip.html());
	this.$Code.val(sohu.diyChipEditor.MCE().getContent());
};
/**
 * 加载标题栏目编辑模板
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
 * 初始化标题栏目编辑模板
 */
sohu.diyChipEditor.Dialog.prototype.InitSecHDTpl=function(){
	var _this=this;
	//更多按钮
	this.$CbxSecHDMore=this.$SecHDActs.find(".vstp_secHDMore").click(function(evt){	
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
	//背景图设置
	this.$SecHDBG=this.$ChipTpl.find(".vstp_chipBG").change(function(evt){
		if(this.value==""){
			_this.$Chip.css("background-image","none");
			return;
		};
		_this.$Chip.css("background-image","url('"+this.value+"')");
	});
	this.$ChipTpl.find(".vstp_chipBGAlign").change(function(evt){
		_this.$Chip.css("background-repeat",this.value);
	});
	this.$ChipTpl.find(".vstp_btnUpl").click(function(evt){
		if(_this.onUpChipBG){
			_this.onUpChipBG(_this);
		};
		return false;
	});
};
/**
 * onDomRefreshed事件处理。碎片编辑器编辑时候更新碎片dom时触发
 */
sohu.diyChipEditor.Dialog.prototype.onDomRefreshed=function(){
	if(!this.Elm) return;
	this.Elm.CT.Editor.Reposition();
};
/**
 * 设置碎片编辑器的目标碎片
 * @param {Object} $chip
 * @param {Object} $elm
 * @param {Object} opts
 */
sohu.diyChipEditor.Dialog.prototype.SetTarget=function($chip,$elm,opts){
	this.$Elm=$elm;//当前元素
	this.Elm=opts.elm;//当前元素对应的sohu.diyElement对象
	this.CT=opts.elm.CT;//当前碎片对象
	this.$Chip=$chip;//当前碎片
	this.$Backup=this.$Chip.clone().removeClass("vstp_ctOn")
	.find(".vstp_elm").removeClass("vstp_elmOn").end()
	.find("#vstp_ctEditor").remove().end();
};
