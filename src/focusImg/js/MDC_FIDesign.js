/**
 * @author levinhuang
 */
var MDC_FIDesign= function() {
    var p={},pub={};
	p.onDel=function(evt){
		$(this).parents(".entry").fadeTo("normal",0.33,function(){$(this).remove();});
		return false;
	};
	p.onAdd=function(evt){
		if($("#contentA .entry").length>=p._limit){
			alert("图片数目超出上限"+p._limit);
			return false;
		};
		p._$entry1=p._$entry.clone(true).addClass("entry1").find(".fld").val("").end();
		$(this).parents(".entry").after(p._$entry1);
		return false;
	};
	p.assertIsInt=function(doms){
		var ok=true;
		for(var i=0;i<doms.length;i++){
			if(doms[i].is(":hidden"))
				continue;

			if(!StringUtils.isPlusInt(doms[i].val())){
				ok=ok&&false;
				doms[i].addClass("alert");
			}else{
				ok=ok&&true;
				doms[i].removeClass("alert");
			};			
		};
		return ok;
	};
	p.assertIsUrl=function(doms){
		var ok=true;
		for(var i=0;i<doms.length;i++){				
			if(!StringUtils.isUrl(doms.eq(i).val())){
				ok=ok&&false;
				doms.eq(i).addClass("alert");
			}else{
				ok=ok&&true;
				doms.eq(i).removeClass("alert");
			};			
		};
		return ok;
	};
	p.getEntryData=function(){
		var r=[];
		var $o,d;
		$("#designer .entry").each(function(i,o){
			$o=$(o);
			d={};
			$o.find(".fld").each(function(i1,o1){
				if($(o1).is(":hidden")) return true;
				d[o1.name]=o1.value;
			});
			r.push(d);
		});
		return r;
	};
	p.fillEntryData=function($entry,data){
		$entry.find(".fld").each(function(i,o){
			if(data[o.name])
				$(o).val(data[o.name]);
		});
	};
	p.onValidate=function(){
		p._fmCfg._isValid=true;
		//validate configuration data
		var ok=p.assertIsInt([p._fmCfg.speed]);
		p._fmCfg._isValid=p._fmCfg._isValid&&ok;
		if(!p._fmCfg._isValid) return false;
		//validate image data
		var objs=$("#designer .imgSrc");
		ok=p.assertIsUrl(objs);
		if(!ok){
			return false;
		}; 
		objs=$("#designer .imgSrc1");
		ok=$.trim(objs.val())==""?true:p.assertIsUrl(objs);
		if (!ok) {
			return false;
		};		
		//validate href data
		objs=$("#designer .imgLnk");
		ok=p.assertIsUrl(objs);
		if (!ok) {
			return false;
		};	
		
		//everything is ok
		var retVal={
			a:p._fmCfg._getData()
		};
		retVal.b=p.getEntryData();
		return retVal;
	};
	p.showCodePreview=function(){
		p._$preview.show();
		//show the overlay
		var h0=p._$desiner.height(),h1=p._$preview.height();
		h0=h0>h1?h0:h1;
		p._$previewOvl.show().css("height",h0+20);	
	};
	p.onGen=function(evt){
		p._$btnRGen.removeClass();
		p._$btnGen.addClass("on");
		p._$preview.removeClass("rgen");
		var d=p.onValidate();
		if(!d){
			$(".alert:first").select();
			return false;
		};
		d.a.flag=p._fiSN;
		d.a.hoverStop=true;
		
		if((p._fiSN=='fi03'||p._fiSN=='fi04')&&d.a.type=="1"){
			d.a.myHtml='<div class="fi_btnplay"></div>';
		};
		if(p._fiSN=='fi06'&&d.a.type=="1"){
			d.a.ptStepY=90;
			d.a.clickTabToNav=true;
		};
		
		var id="MDCFI"+StringUtils.RdStr(8);
		var s='<div id="'+id+'" class="'+p._fiSN+'"></div>\r\n';
		s+='<script type="text/javascript">\r\n';
		s+='jQuery("#'+id+'").focusImg('+$.toJSON(d.a)+','+$.toJSON(d.b)+');\r\n';
		s+='</script>';

		p._$txtCode.val(s);
	
		p.showCodePreview();
	};
	/**
	 * 分析代码按钮处理
	 * @param {Object} evt
	 */
	p.onRevertGen=function(evt){
		p._$btnGen.removeClass();
		p._$btnRGen.addClass("on");
		p._$preview.addClass("rgen");
		p.showCodePreview();
		p._$txtCode.val("").focus();
	};
	/**
	 * 反向分析焦点图代码
	 * @param {Object} evt
	 */
	p.onDoRGen=function(evt){
		var c=$.trim(p._$txtCode.val());
		if(c==""){
			alert("请输入要分析的焦点图代码");
			p._$txtCode.focus();
			return;
		};
		c=c.substring(c.indexOf("{"),c.indexOf("]")+1);
		if(c==""){alert("代码不合法");return;};
		//配置json数据
		var d=c.substring(0,c.indexOf("[")-1);
		if(d==""){alert("代码不合法");return;};
		p._fiData={};
		try{p._fiData.a=$.evalJSON(d);}catch(e){alert("代码不合法:"+d);return;};
		if(!p._fiData.a.flag){alert("代码不合法(缺少焦点图类型flag属性):"+d);return;};
		//图片json数据
		d=c.substr(c.indexOf("["));
		try{p._fiData.b=$.evalJSON(d);}catch(e){alert("代码不合法:"+d);return;};
		
		p.loadEditData(true);
		p._$closePreview.trigger("click");
	};
	/**
	 * 选择焦点图
	 * @param {Object} evt
	 */
	p.onSelect=function(evt){
		if(this.className=="on") return false;
		if(!p._$previewOvl.is(":hidden"))
			p._$closePreview.trigger("click");
			
		p._$fiItems.removeClass("on");
		$(this).addClass("on");
		p._$body.removeClass().addClass(this.rel);
		p._fiSN=this.rel;
		p._demoUrl=this.href;
		return false;
	};
	p.loadEditData=function(keepLMenu){
		if(!p._fiData) return;
		//隐藏左边菜单
		if(!keepLMenu)
			p._$fiList.hide();
		//焦点图种类
		p._$fiItems.each(function(i,o){
			if(o.rel==p._fiData.a.flag){
				$(o).trigger("click");
				return false;
			};
		});
		//清空表单
		p._$desiner.find(".entry1").remove();
		//根据数据建立编辑ui
		var $o=null;
		for(var c in p._fiData.a){
			if(c=="type"){c=c+"_"+p._fiData.a.flag;};
			$o=p._fmCfg[c];
			if(!$o)
				continue;
			
			if($o.is(".cbx")){
				$o[0].checked=p._fiData.a[c];
			}else{
				$o.val(p._fiData.a[c]);
			};
		};
		
		for(var i=0;i<p._fiData.b.length;i++){
			if(i==0){
				p.fillEntryData(p._$entry,p._fiData.b[i]);
			}else if(i==1){
				p._$btnAdd.trigger("click");
				p.fillEntryData(p._$entry1,p._fiData.b[i]);
			}else{
				p._$entry1.find(".add").trigger("click");
				p.fillEntryData(p._$entry1,p._fiData.b[i]);
			};
		};
	};
    //private area
    p.initVar = function(opts) { 
		p._$entry=$("#contentA .entry");
		p._$entry1=null;					/* 引用并缓存p._$entry的克隆实体 */
		p._$btnAdd=p._$entry.find(".add");
		p._limit=opts.limit||10;
		p._fmCfg={
			speed:$("#txtSpeed"),
			place:$("#ddlBtnPos"),
			text:$("#cbxShowTxt"),
			type_fi02:$("#ddlTypeFI02"),	/* 第2种焦点图的类型 */
			type_fi03:$("#ddlTypeFI03"),	/* 第3种焦点图的类型 */
			type_fi04:$("#ddlTypeFI04"),	/* 第4种焦点图的类型 */
			type_fi05:$("#ddlTypeFI05"),	/* 第5种焦点图的类型 */
			type_fi06:$("#ddlTypeFI06"),	/* 第5种焦点图的类型 */
			_isValid:true,
			_getData:function(){
				var d={},o;
				for(var c in p._fmCfg){
					if((c.indexOf("_")==0)||p._fmCfg[c].is(":hidden"))
						continue;
						
					o=p._fmCfg[c][0];
					if(p._fmCfg[c].is(".cbx")){
						d[o.name]=o.checked;
					}else{
						d[o.name]=p._fmCfg[c].val();
					};
				};	
				
				return d;
			}
		};
		p._$previewOvl=$("#preview_ovl").css("opacity",0.6);
		p._$preview=$("#preview");
		p._$closePreview=p._$preview.find(".close");
		p._$txtCode=$("#txtCode");
		p._$btnGen=$("#btnGen");
		p._$btnRGen=$("#btnAnalytics");
		p._$desiner=$("#designer");
		p._$btnViewDemo=$("#btnViewDemo");
		p._demoUrl="http://i.mdc.sohu.com/levinhuang/focusImg/1/";
		
		//焦点图列表
		p._$fiItems=$("#fiList_ a");
		//body
		p._$body=$("body");
		//左侧菜单
		p._$fiList=$("#fiList");
		//左侧菜单开关
		p._$toggler=p._$fiList.find(".toggler");
		//焦点图类型
		p._fiSN='fi01';
		//预览图
		p._$thumbView=$("#thumbView");
		p._$thumbImg=p._$thumbView.find("img");
		if(window.opener)
			p._fiData=window.opener.MDCFI_DATA;
		else
			p._fiData=null;
		
	};
    p.onLoaded = function() { };
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		p._$entry.find(".del").click(p.onDel).end().find(".add").click(p.onAdd);
		p._$entry.find(".imgSrc,.imgLnk").change(function(evt){
			p.onValidate();
		});
		//生成代码
		p._$btnGen.click(p.onGen);
		//反向分析代码
		p._$btnRGen.click(p.onRevertGen);
		$("#btnDoRGen").click(p.onDoRGen);
		p._$btnViewDemo.click(function(){
			window.open(p._demoUrl);
		});
		//代码框
		p._$txtCode.click(function(evt){$(this).select();});
		p._$closePreview.click(function(evt){
			p._$previewOvl.hide();
			p._$preview.slideUp("fast");
			return false;
		});
		//焦点图选择列表
		p._$fiItems.click(p.onSelect);
		p._$fiList.find("li").hover(function(e){
			p._$thumbImg.attr("src",($(this).index()+1)+"/images/preview.gif");
			p._$thumbView.show();
		},function(e){
			p._$thumbView.hide();
		});
		p._$thumbView.find("a").click(function(){p._$thumbView.fadeTo("fast",0);});
		
		//左侧菜单关闭和打开
		p._$toggler.find(".op").click(function(evt){
			p._$toggler.addClass("open");
			p._$fiList.animate({left:0},"slow");
			return false;
		});
		p._$toggler.find(".cl").click(function(evt){
			p._$toggler.removeClass("open");
			p._$fiList.animate({left:-80},"slow");
			return false;
		});
		//排序效果
		p._$desiner.find(".sec").sortable();
		//加载编辑数据
		p.loadEditData();
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 