/**
 * 可视化编辑控制台
 * @author levinhuang
 * @param {Object} opts 选项,如clSec:"sec",cssWsp:"#main"表示分栏的css类为sec,工作区域的css选择器为#main
 * @dependency sohu.diyEditor.js;sohu.diyArea.js
 * TODO:将顶部菜单部分逻辑移到单独的js文件diyMenuBar.js
 */
sohu.diyConsole=function(opts){
	//属性
	opts=$.extend({},{
		cssWsp:"#main",clSec:"sec",clSec0:"sec0",clSecSub:"subsec",
		clSecRoot:"col",clArea:"area",cssArea:".area",dfTop:100,
		limitSec:390,
		scrollWrapMainginTop:0
		},opts);
	var _this=this;
	this.$Workspace=$(opts.cssWsp);
	this.$Layout=$("#areaTools");

	sohu.diyConsole.$SecEditorModel=$("#area_editor");
	this.Areas=null;
	
	var p={opts:opts};
	p._$btnAdd=$("#lnkAreaAdd");
	p._$btnDel=$("#lnkAreaDel");
	p._$btnBG=$("#lnkAreaBG");
	p._$btnDown=$("#lnkAreaDown");
	p._$btnUp=$("#lnkAreaUp");
	p._$areaSelector=$("#area_selector");
	p._$wAreaBG=$("#wAreaBG")
	p._$pageTip=$("#pageTip");
	p._$elmTool=$("#elmTool");
	/* =顶部交互菜单= */
	p._$editMenu=$("#editMenu");
	p._$menus=$("#editMenuChild .menuitem");
	p._$menuTxt=p._$editMenu.find(".mtxt");
	p._$menuImg=p._$editMenu.find(".mimg");
	p._$menuSecHead=p._$editMenu.find(".msecHead");
	p._$popWins=p._$editMenu.find(".win");
	/* 对话框jq对象 */
	p._$wCpkWrap=$("#cpkWrap");
	p._$wAddLink=$("#addLink");
	p._$wImgPro=$("#imgPro");
	p._$wImgSwitch=$("#imgSwitch");
	p._$wSecHead=$("#cfgSecHead");
	/* /对话框jq对象 */
	
	/* 对话框表单元素 */
	p._fmAddLink={
		txtATitle:$("#txtATitle"),
		txtAHref:$("#txtAHref"),
		txtATarget:$("input[name='atarget']"),
		tipAHref:p._$wAddLink.find(".tipAHref"),
		reset:function(){p._$wAddLink.find(".tip").removeClass("alert").end().find("input[type='text']").removeClass("alert").val("");}
	};
	p._fmImgPro={
		txtImgH:$("#txtImgH"),
		txtImgW:$("#txtImgW"),
		ddlImgBStyle:$("#ddlImgBStyle"),
		txtImgBColor:$('#txtImgBColor'),
		txtImgPadding:$("#txtImgPadding"),
		cpk:p._$wImgPro.find(".cpk"),
		reset:function(){
			p._$wImgPro.find(".tip").removeClass("alert").end().find("input[type='text']").removeClass("alert").val("");
		}
	};
	p._fmImgSwitch={
		txtImgAlt:$("#txtImgAlt"),
		txtImgSrc:$("#txtImgSrc"),
		tipImgSrc:p._$wImgSwitch.find(".tipImgSrc"),
		btnOK:p._$wImgSwitch.find(".btnMWinOK"),
		btnNO:p._$wImgSwitch.find(".btnMWinNO"),
		reset:function(){p._$wImgSwitch.find(".tip").removeClass("alert").end().find("input[type='text']").removeClass("alert").val("");}
	};
	p._fmAreaBG={
		txtAreaBG:$("#txtAreaBG"),
		rbtnAreaBGAlign:p._$wAreaBG.find("input[name='areaBGAlign']"),
		rbtnAreaBGRepeat:p._$wAreaBG.find("input[name='areaBGRepeat']"),
		tipAreaBG:p._$wAreaBG.find(".tipAreaBG"),
		reset:function(){p._$wAreaBG.find("*").removeClass("alert").end().find(":text").val("");}
	};
	p._fmSecHead={
		txtBG:$("#txtSecHeadBGC"),
		txtBGImg:$("#txtSecHeadImg"),
		tipBGImg:p._$wSecHead.find(".tipSecHeadImg"),
		btnOK:p._$wSecHead.find(".btnMWinOK"),
		btnNO:p._$wSecHead.find(".btnMWinNO"),
		btnDel:p._$wSecHead.find(".btnMWinDel"),
		cpk:p._$wSecHead.find(".cpk"),
		reset:function(){p._$wSecHead.find(".tip").removeClass("alert").end().find("input[type='text']").removeClass("alert").val("");}
	};
	/* /对话框表单元素 */
	p._$txtFontColor=$("#txtFontColor");
	p._opts=opts;
	/* =/顶部交互菜单= */
	
	//横切删除时的回调函数
	p.onAreaRemove=function(area){
		_this.ActiveArea(null);
		_this.$Layout.animate({top:opts.dfTop});	
	};
	
	/**
	 * 显示横切选择器
	 */
	p.showSelector=function(){
		var _onClose=function(evt,ui){
			if(!p._curAreaTpl) return false;//未选中任何分栏
			var obj=new sohu.diyArea({
				tplID:p._curAreaTpl,
				console:_this,
				onRemove:p.onAreaRemove
			});
			_this.Areas.push(obj);
		};
		//显示选择框
		p._$areaSelector.dialog({
			title:"添加横切",
			resizable:false,
			modal:true,
			width:430,
			height:250,
			position:[700,50],
			close:_onClose
			}
		);
	};
	p.onAdd=function(evt){
	///<summary>添加横切</summary>
		p.showSelector();	
		return false;
	};
	p.onSelectAreaTpl=function(evt){
	///<summary>选择某个横切模板后</summary>
		p._curAreaTpl=this.id;p._$areaSelector.dialog("close");	return false;
	};
	p.onRemove=function(evt){
	///<summary>删除横切</summary>	
		if(!sohu.diyConsole.CurArea){alert("未选中任何横切!");return false;};
		_this.Areas=$.grep(_this.Areas,function(o,i){
			if(o.ID==_this.CurArea.ID) return false;
			return true;
		});
		sohu.diyConsole.CurArea.Remove();
		return false;	
	};
	p.onAddBG=function(evt){
	///<summary>添加横切背景</summary>	
		if(!sohu.diyConsole.CurArea){alert("未选中任何横切!");return false;};
		//对话框相关回调处理函数
		var _onOK=function(evt,cls){
			cls=cls||true;
			var url=p._fmAreaBG.txtAreaBG.val();
			if((url!="")&&(!StringUtils.isUrl(url))){
				p._fmAreaBG.txtAreaBG.addClass("alert").select();
				p._fmAreaBG.tipAreaBG.addClass("alert");
				return false;
			};
			if(url==""){
				sohu.diyConsole.CurArea.$Layout.css("background-image","none");
			}else{
				sohu.diyConsole.CurArea.$Layout.css("background-image","url('"+url+"')");
			};
			var al=p._fmAreaBG.rbtnAreaBGAlign.curVal;
			al=al=="center"?al:al+" top";
			sohu.diyConsole.CurArea.$Layout.css("background-position",al);
			
			var rp=p._fmAreaBG.rbtnAreaBGRepeat.curVal;
			sohu.diyConsole.CurArea.$Layout.css("background-repeat",rp);
			if(cls)
				$(this).dialog("close");
		};
		var _onNO=function(evt){
			$(this).dialog("close");
		};
		//显示对话框
		p._$wAreaBG.dialog(
		{
			title:"横切背景设置",
			resizable:false,
			modal:true,
			width:430,
			position:[700,50],
			buttons:{
				"关闭":_onNO,
				"确认":_onOK,
			},
			open:function(evt,ui){
				var img=sohu.diyConsole.CurArea.$Layout.css("background-image");
				img=img.replace('url("',"").replace('")',"");
				p._fmAreaBG.txtAreaBG.val(img).select();
				p._$wAreaBG.preview=function(){
					_onOK(evt,false);
				};
			},
			close:function(evt,ui){p._fmAreaBG.reset();}
		}
		);
		return false;
	};
	p.onMove=function(evt){
	///<summary>移动横切</summary>
		if(!sohu.diyConsole.CurArea){alert("未选中任何横切!");return false;};	
		var isUp=evt.data.up;
		sohu.diyConsole.CurArea.Move(isUp);
		_this.RePosition();
		return false;	
	};
	p.getWorkspaceBoundary=function(){
		var lastArea=_this.$Workspace.find(opts.cssArea+":last");
		if (lastArea.size() == 0) {
			return {
				lbleft: -2000,
				ubleft: 2000,
				lbtop: -2000,
				ubtop: 2000
			};
		};
		
		var lbtop=_this.$Workspace.offset().top;
		var ubtop=lastArea.height()+lastArea.offset().top;
		var lbleft=lastArea.offset().left;
		var ubleft=lastArea.width()+lbleft;
		
		return {
			lbleft:lbleft,
			ubleft:ubleft,
			lbtop:lbtop,
			ubtop:ubtop
		};
	};
	//body标签的鼠标事件
	p.onMousemove=function(evt){
		if(!sohu.diyConsole.CurArea) return;
		if(!sohu.diyConsole.CurArea.IsActive) return;
		if(sohu.diyConsole.CurArea.IsEditing) return;
		if(sohu.diyConsole.Dragger.ing) return;
		//if(sohu.diyConsole.EditingSec!=null) return;
		
		var b=p.getWorkspaceBoundary();
		
		if(evt.pageX<b.lbleft||evt.pageX>b.ubleft||evt.pageY>b.ubtop){//||evt.pageY<lbtop
			//反激活横切
			sohu.diyConsole.CurArea.Deactive();
			//反激活分栏
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Deactive();
		};
	};
	p.onBodyClick=function(evt){
		if(!sohu.diyConsole.CurArea) return;
		if(!sohu.diyConsole.CurArea.IsActive) return;
		if(sohu.diyConsole.CurArea.IsEditing) return;
		if(sohu.diyConsole.Dragger.ing) return;
		//用户是否点击#editMenu
		if($(evt.target).parents("#editMenu").length>0) return;
		
		var b=p.getWorkspaceBoundary();
		if(evt.pageX<b.lbleft||evt.pageX>b.ubleft||evt.pageY>b.ubtop){//||evt.pageY<lbtop
			//强制移除内联编辑器
			if(sohu.diyConsole.EditingSec!=null&&sohu.diyConsole.CurElm!=null){
				sohu.diyConsole.CurElm.HideEditor(false);
			};
			//反激活横切
			sohu.diyConsole.CurArea.Deactive();
			//反激活分栏
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Deactive();
			//重置添加链接对话框表单
			p._fmAddLink.reset();
		};
		return false;
	};
	p.setDocumentDim=function(){
		var fullheight, height;
		fullheight = sohu.diyConsole.InnerHeight();        
		height = fullheight - p.opts.scrollWrapMainginTop;
		
		$("#scrollWrap").css("height",height);
		$("#main").css("minHeight",height);
	};
	p.onLoaded=function(){
		//文档高度适应处理
		p.setDocumentDim();
		//编辑工具栏位置处理
		p._$editMenu.css("top",p.opts.scrollWrapMainginTop)
			.hide().find("#editMenuIn")
			.draggable({containment:'#mainWrap',scroll:false,handle:"#editMenuChild"});
		//横切工具条位置
		_this.$Layout.css({"top":p.opts.scrollWrapMainginTop+30});
	};
	p.initColorPicker=function(){
		//文字颜色拾色器
		var cbk=p._$wCpkWrap.ColorPicker({
			flat:true,
			color:"#000000",
			onSubmit:function(hsb,hex,rgb){
				var c="#"+hex;
				p._$txtFontColor.val(c);
				p._$popWins.slideUp();	
				sohu.diyConsole.CurElm.i$frame[0].iDoCommand("foreColor",c,null,function($iframe){
					sohu.diyConsole.DocSelection.selectAndRelease();
				});
			},
			onChange:function(hsb,hex,rgb){
				sohu.diyConsole.DocSelection.select();
			}
		});
		//图片边框拾色器
		p._fmImgPro.cpk.ColorPicker({
			flat:true,
			color:"#cccccc",
			onChange:function(hsb,hex,rgb){
				p._fmImgPro.txtImgBColor.css("backgroundColor","#"+hex).val("#"+hex);
			},
			onSubmit:function(hsb,hex,rgb){
				p._fmImgPro.cpk.hide();
			}
		});
		//栏目头背景色
		p._fmSecHead.cpk.ColorPicker({
			flat:true,
			color:"#cccccc",
			onChange:function(hsb,hex,rgb){
				p._fmSecHead.txtBG.css("backgroundColor","#"+hex).val("#"+hex);
				sohu.diyConsole.CurElm.$Layout.parent().css("backgroundColor","#"+hex).val("#"+hex);
			},
			onSubmit:function(hsb,hex,rgb){
				p._fmSecHead.cpk.hide();
			}
		});
	};
	p.initAddLink=function(){
		//链接对话框的确认按钮逻辑
		p._$wAddLink.find("#btnAddLink").click(function(evt){
			var url=p._fmAddLink.txtAHref.val();
			if(!StringUtils.isUrl(url)){
				p._fmAddLink.txtAHref.addClass("alert").select();
				p._fmAddLink.tipAHref.addClass("alert");
				return false;
			};
			p._$popWins.slideUp();
			if(p._fmAddLink.isNew){
				sohu.diyConsole.CurElm.i$frame[0].iDoCommand("createlink",url,null,function($iframe){
					sohu.diyConsole.DocSelection.selectAndRelease();
				});
			}else{
				p._fmAddLink.a.$obj.attr("title",p._fmAddLink.txtATitle.val())
					.attr("href",p._fmAddLink.txtAHref.val())
					.attr("target",p._fmAddLink.txtATarget.curVal);
			}

			p._fmAddLink.reset();
		});
		//关闭按钮
		p._$wAddLink.find("#btnCloseLink").click(function(evt){
			p._$popWins.slideUp();
		});
		p._$wAddLink.find("input[type='text']").keyup(function(evt){
			this.value=this.value.replace('"',"").replace("'","").replace('‘',"").replace('“',"").replace('”',"");
		});
		//打开位置单选框
		p._fmAddLink.txtATarget.click(function(evt){
			p._fmAddLink.txtATarget.curVal=this.value;
		});
		p._fmAddLink.txtATarget.curVal="_blank";
	};
	p.initImgPro=function(){
		//图片属性对话框的确认按钮逻辑
		p._$wImgPro.find("#btnAddLink1").click(function(evt){
			var isOk=true,i=[null,null,null];
			$.each([p._fmImgPro.txtImgH,p._fmImgPro.txtImgW,p._fmImgPro.txtImgPadding],function(j,o){
				var _v=o.val();
				i[j]=(_v==""?"auto":_v);
				if((_v!="")&&(!StringUtils.isPlusInt(_v))){
					$(o).addClass("alert");
					isOk=isOk&&false;
				}
			});
			if(!isOk) return;
			p._$popWins.slideUp();

			i[2]=i[2]=="auto"?"0":i[2];
			var $img=sohu.diyConsole.CurElm.$Layout;
			$img.attr("width",i[1]).attr("height",i[0]).css("padding",i[2]+'px');
			if(p._fmImgPro.ddlImgBStyle.val()!="none"){
				var b="1px "+p._fmImgPro.ddlImgBStyle.val()+" "+p._fmImgPro.txtImgBColor.val();
				$img.css("border",b);
			}else{
				$img.css("border","none");
			}

			p._fmImgPro.reset();
		});
		//关闭按钮
		p._$wImgPro.find("#btnCloseLink1").click(function(evt){
			p._$popWins.slideUp();
		});
		p._$wImgPro.find("input[type='text']").keyup(function(evt){
			this.value=this.value.replace('"',"").replace("'","").replace('‘',"").replace('“',"").replace('”',"");
		});
		//边框样式
		p._fmImgPro.ddlImgBStyle.change(function(evt){
			p._fmImgPro.ddlImgBStyle.curVal=this.value;
		});
		p._fmImgPro.ddlImgBStyle.curVal="none";
		//边框颜色拾色器

		p._fmImgPro.txtImgBColor
			.attr("readonly",true)
			.css("backgroundColor","#cccccc")
			.click(function(evt){
				p._fmImgPro.cpk.show();
			});
	};
	p.initImgSwitch=function(){
		//按钮行为
		p._fmImgSwitch.btnOK.click(function(evt){
			var url=p._fmImgSwitch.txtImgSrc.val();
			if(!StringUtils.isUrl(url)){
				p._fmImgSwitch.txtImgSrc.addClass("alert").select();
				p._fmImgSwitch.tipImgSrc.addClass("alert");
				return false;
			};
			//更改图片属性
			sohu.diyConsole.CurElm.$Layout.attr("src",url).attr("alt",p._fmImgSwitch.txtImgAlt.val());
			//收起对话框
			p._$popWins.slideUp();
			//重置表单
			p._fmImgSwitch.reset();	
		});
		p._fmImgSwitch.btnNO.click(function(evt){p._$popWins.slideUp();});
		
	};	
	p.initAreaBG=function(){
		p._fmAreaBG.rbtnAreaBGAlign.click(function(evt){
			p._fmAreaBG.rbtnAreaBGAlign.curVal=this.value;
			p._$wAreaBG.preview();
		});
		p._fmAreaBG.rbtnAreaBGRepeat.click(function(evt){
			p._fmAreaBG.rbtnAreaBGRepeat.curVal=this.value;
			p._$wAreaBG.preview();
		});		
		p._fmAreaBG.rbtnAreaBGAlign.curVal="center";
		p._fmAreaBG.rbtnAreaBGRepeat.curVal="no-repeat";
	};
	p.initSecHead=function(){
		//按钮行为
		p._fmSecHead.btnOK.click(function(evt){
			var url=$.trim(p._fmSecHead.txtBGImg.val());
			if( (url!="") && (!StringUtils.isUrl(url)) ){
				p._fmSecHead.txtBGImg.addClass("alert").select();
				p._fmSecHead.tipBGImg.addClass("alert");
				return false;
			};
			//更改图片属性
			if(url!="")
				sohu.diyConsole.CurElm.$Layout.parent().css("background-image","url('"+url+"')");
			//收起对话框
			p._$popWins.slideUp();
			//重置表单
			p._fmSecHead.reset();	
		});
		p._fmSecHead.btnNO.click(function(evt){p._$popWins.slideUp();});
		p._fmSecHead.btnDel.click(function(evt){
			if(!window.confirm("确定删除该栏目标题么")) return;
			p._$popWins.slideUp();
			var ct=sohu.diyConsole.CurElm.CT;
			var sec=sohu.diyConsole.EditingSec;
			sohu.diyConsole.CurElm.HideEditor(false);
			ct.$Layout.remove();
			sec.RemoveCTByID(ct.ID);
			
		});
		//背景色
		p._fmSecHead.txtBG
		.attr("readonly",true)
		.css("backgroundColor","#cccccc")
		.click(function(evt){
			p._fmSecHead.cpk.show();
		});
		//背景图
		p._fmSecHead.txtBGImg.change(function(evt){
			var url=p._fmSecHead.txtBGImg.val();
			if(!StringUtils.isUrl(url)){
				p._fmSecHead.txtBGImg.addClass("alert").select();
				p._fmSecHead.tipBGImg.addClass("alert");
				return false;
			};
			//更改图片属性
			sohu.diyConsole.CurElm.$Layout.parent().css("background-image","url('"+url+"')");
		});
	};
	/**
	 * Save current document.selection to sohu.diyConsole.DocSelection
	 */
	p.saveSelection=function(){
		if(sohu.diyConsole.CurElm.InlineEditable){
			sohu.diySelection.snap(sohu.diyConsole.CurElm.i$frame[0].iDoc());
		}else{
			sohu.diySelection.snap(document,sohu.diyConsole.CurElm.$Layout[0]);
		};
	};
	p.initEditMenu=function(){
		var fontStyleCbk=function(iframeEditor){
			sohu.diyConsole.CurElm.$Layout.css(iframeEditor.iCurCss);
		};
		p._$menus.click(function(evt){
			p._$menus.removeClass("on");
			var _this=$(this);
			_this.addClass("on");
			if(!_this.is(".popwin"))
				p._$popWins.hide();	
				
			return false;
		});
		$("#menuitem-b").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Bold",null);});
		$("#menuitem-i").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Italic",null);});
		$("#menuitem-u").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Underline",null);});
		$("#menuitem-if").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("increasefontsize",null);});
		$("#menuitem-df").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("decreasefontsize",null);});
		$("#menuitem-al").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyleft",null,fontStyleCbk);});
		$("#menuitem-ac").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifycenter",null,fontStyleCbk);});
		$("#menuitem-ar").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyright",null,fontStyleCbk);});
		$("#menuitem-af").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyfull",null,fontStyleCbk);});
		$("#menuitem-ul").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("insertunorderedlist",null);});
		$("#menuitem-ol").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("insertorderedlist",null);});
		$("#menuitem-z").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("undo",null);});
		$("#menuitem-r").click(function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("redo",null);});
		//colorpicker
		$("#menuitem-cc").mousedown(function(evt){
			p.saveSelection();
			p._$popWins.hide();	
			p._$wCpkWrap.slideDown();
		});
		//create link
		$("#menuitem-cl").mousedown(function(evt){
			p.saveSelection();
			p._$popWins.hide();
			p._$wAddLink.slideDown();
			p._fmAddLink.txtATitle.focus();
			//当前选中是否有a标签
			var a=sohu.diyConsole.DocSelection.getA();
			/*
			if(sohu.diyConsole.CurElm.InlineEditable){
				a=
			}else{
				if(sohu.diyConsole.CurElm.tagName=="img"){
					var $a=sohu.diyConsole.CurElm.$Layout.parents("a");
					a={
						isNull:false,
						$obj:$a,
						title:$a.attr()
					};	
				};
			};
			*/
			if(a.isNull){
				p._fmAddLink.isNew=true;
				p.a=null;
				return;
			};
			p._fmAddLink.txtATitle.val(a.title);
			p._fmAddLink.txtAHref.val(a.href);
			p._fmAddLink.txtATarget.filter("input[value='"+a.target+"']").trigger("click");
			p._fmAddLink.isNew=false;
			p._fmAddLink.a=a;
		});
		//图片属性
		$("#menuitem-imgp").mousedown(function(evt){
			p._$popWins.hide();
			p._$wImgPro.slideDown();
			var $img=sohu.diyConsole.CurElm.$Layout;
			p._fmImgPro.txtImgH.val(parseInt($img.css("height")));
			p._fmImgPro.txtImgW.val(parseInt($img.css("width")));
			p._fmImgPro.ddlImgBStyle.val($img.css("borderStyle"));
			p._fmImgPro.txtImgBColor.val($img.css("borderColor"));
			p._fmImgPro.txtImgPadding.val(parseInt($img.css("padding"))||"0");
		});
		//更换图片
		$("#menuitem-img").mousedown(function(evt){
			p._$popWins.hide();
			p._$wImgSwitch.slideDown();
			p._fmImgSwitch.txtImgAlt.val(sohu.diyConsole.CurElm.$Layout.attr("alt"));
			p._fmImgSwitch.txtImgSrc.val("");
		});
		//分栏标题
		$("#menuitem-secHead").mousedown(function(evt){
			p._$popWins.hide();
			p._$wSecHead.slideDown();
			var $img=sohu.diyConsole.CurElm.$Layout;
			p._fmSecHead.txtBG.val($img.css("backgroundColor"));
			var img=$img.css("background-image");
			img=img.replace('url("',"").replace('")',"");
			img=img=="none"?"":img;
			p._fmSecHead.txtBGImg.val(img);
		});
	};
	p.Init=function(){
		//公有属性引用
		sohu.diyConsole.$EditMenu=p._$editMenu;
		sohu.diyConsole.$MenuTxt=p._$menuTxt;
		sohu.diyConsole.$MenuImg=p._$menuImg;
		sohu.diyConsole.$MenuSecHead=p._$menuSecHead;
		sohu.diyConsole.$CPKWrap=p._$wCpkWrap;
		sohu.diyConsole.$PopWins=p._$popWins;
		sohu.diyConsole.$Menu=p._$menus;
		sohu.diyConsole.$ElmTool=p._$elmTool;
		//横切选择器
		$("li",p._$areaSelector).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");})
			.click(p.onSelectAreaTpl);
		
		//分栏选择器
		$('#hiddenTemplate .sec_selector li').click(function(evt){
			if(!sohu.diyConsole.CurSec) return;
			sohu.diyConsole.CurSec.Editor.SecSelector.Cur=this.id;
			sohu.diyConsole.CurSec.Editor.SecSelector.dialog("close");
			sohu.diyConsole.CurSec.AddSub($(sohu.diyTp[this.id]));
			return false;
		}).hover(function(){$(this).addClass("on");},function(){$(this).removeClass("on");});
		
		p._$btnAdd.bind("click",p.onAdd);
		p._$btnDel.click(p.onRemove);
		//隐藏删除按钮
		//p._$btnDel.parent().hide();
		p._$btnBG.click(p.onAddBG);
		p._$btnUp.bind("click",{up:true},p.onMove);
		p._$btnDown.bind("click",{up:false},p.onMove);
		
		//已有横切
		_this.Areas=_this.AreaList().map(function(i,o){
			o=new sohu.diyArea({
				isNew:false,
				console:_this,
				onRemove:p.onAreaRemove
			});
			return o;
		});
		//body鼠标事件
		$("body").mousemove(p.onMousemove).click(p.onBodyClick);
		//window resize事件
		$(window).resize(function(evt){
			p.setDocumentDim();
			if(sohu.diyConsole.CurSec)
				sohu.diyConsole.CurSec.Editor.Reposition();
		});
		//color picker
		p.initColorPicker();
		//create link popup window
		p.initAddLink();
		//图片属性对话框
		p.initImgPro();
		//更换图片对话框
		p.initImgSwitch();
		//横切背景对话框
		p.initAreaBG();
		//分栏标题对话框
		p.initSecHead();
		//editMenu的事件注册
		p.initEditMenu();
		//元素工具条
		sohu.diyElementTool.Init({});
		//on page loaded
		$(document).ready(p.onLoaded);
	};
	this.__p=p;
	//Init
	p.Init();
};
/**
 * 重定位
 */
sohu.diyConsole.prototype.RePosition=function(){
	if(!this.CurArea){this.$Layout.attr("style","");return;};
	this.$Layout.css("top",this.CurArea.$Layout.offset().top);
};
/**
 * 设定激活的横切对象
 * @param {Object} target
 */
sohu.diyConsole.prototype.ActiveArea=function(target){
	//将上一个横切反激活
	if(sohu.diyConsole.CurArea){
		if(target&&(target.ID==sohu.diyConsole.CurArea.ID)) 
			return this;
		
		sohu.diyConsole.CurArea.Deactive();
	};
	//激活当前的横切
	sohu.diyConsole.CurArea=target;
	this.CurArea=target;
	return this;
};
/**
 * 关闭内容选择对话框
 */
sohu.diyConsole.prototype.CloseCTDialog=function(){
	sohu.diyConsole.CurSec.Editor.CloseCTDialog();
};
/**
 * 获取所有横切jquery对象
 */
sohu.diyConsole.prototype.AreaList=function(){
	var items= this.$Workspace.find(this.__p.opts.cssArea);
	return items;
};
/**
 * 弹出一个确认对话框
 * @param {Object} opts
 */
sohu.diyConsole.prototype.Confirm=function(opts){
	var _this=this;
	opts=$.extend({},{
		title:"确认操作?",
		ct:"",
		height:140,
		width:"",
		position:"center",
		resizable:false,
		modal:true,
		yes:null,
		no:null,
		close:null
	},opts);
	
	var dlOpt={
		title:opts.title,
		resizable:opts.resizable,
		height:opts.height,
		width:opts.width,
		modal:opts.modal,
		position:opts.position,
		buttons:{
			"取消":function(){
				if(opts.no){opts.no(this);};
				$(this).dialog("close");
			},
			"确认":function(){
				if(opts.yes){opts.yes(this);};
				$(this).dialog("close");
			}
		},
		close:function(evt,ui){
			_this.__p._$pageTip.removeClass("confirm");
			if(opts.close){
				opts.close(evt,ui);
			};
		}
	};
	this.__p._$pageTip.addClass("confirm").html(opts.ct).dialog(dlOpt);
};
/**
 * 移除.txtLoading
 */
sohu.diyConsole.toggleLoading=function(){
	$(".txtLoading").toggle();
};
/*静态方法、对象*/
sohu.diyConsole.Dragger={
	ing:false,
	obj:null,
	handle:$("#ctHandle"),
	cssHandle:'.dragHandle'
};
sohu.diyConsole.CurArea=null;
sohu.diyConsole.CurSec=null;		/* 当前鼠标所在的分栏 */
sohu.diyConsole.EditingSec=null;	/* 当前内联编辑的分栏 */
sohu.diyConsole.CurCT=null;
sohu.diyConsole.EditingCT=null;
sohu.diyConsole.CurElm=null;/* current editing element */
sohu.diyConsole.$SecEditorModel=null; /* 分栏编辑器dom模型 */
sohu.diyConsole.DocSelection='';
sohu.diyConsole.InnerHeight=function() {
    var x,y;
    if (self.innerHeight) // all except Explorer
    {
		return self.innerHeight;
    }
    else if (document.documentElement && document.documentElement.clientHeight)         
    {
		// Explorer 6 Strict Mode
        return document.documentElement.clientHeight;
    }
    else if (document.body) // other Explorers
    {
        return document.body.clientHeight;
    };

};
/**
 * 显示顶部编辑菜单
 * @param {Object} group 菜单组名
 * @param {Boolean} showOthers 显示其他组菜单
 */
sohu.diyConsole.ShowEditMenu=function(group,showOthers){
	showOthers=showOthers||false;
	sohu.diyConsole.$EditMenu.show();
	if(!showOthers)
		sohu.diyConsole.$Menu.not(".mcom").hide();
	
	sohu.diyConsole[group].show();
};
