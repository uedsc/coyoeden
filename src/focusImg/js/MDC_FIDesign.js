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
		$(this).parents(".entry").after(p._$entry.clone(true).addClass("entry1"));
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
	p.onValidate=function(){
		p._fmCfg._isValid=true;
		//validate configuration data
		var ok=p.assertIsInt([p._fmCfg.h,p._fmCfg.w,p._fmCfg.s,p._fmCfg.h0,p._fmCfg.w0,p._fmCfg.h1,p._fmCfg.w1]);
		p._fmCfg._isValid=p._fmCfg._isValid&&ok;
		if(!p._fmCfg._isValid) return false;
		//validate image data
		var objs=$("#designer .imgSrc");
		ok=p.assertIsUrl(objs);
		if(!ok) return false;
		objs=$("#designer .imgSrc1");
		ok=$.trim(objs.val())==""?true:p.assertIsUrl(objs);
		if(!ok) return false;		
		//validate href data
		objs=$("#designer .imgLnk");
		ok=p.assertIsUrl(objs);
		if(!ok) return false;
		
		//everything is ok
		var retVal={
			a:p._fmCfg._getData()
		};
		retVal.b=p.getEntryData();
		return retVal;
	};
	p.onGen=function(evt){
		var d=p.onValidate();
		if(!d){
			$(".alert:first").select();
			return false;
		};
		
		var id="MDCFI"+StringUtils.RdStr(8);
		d.a.id="#"+id;
		var s='<div id="'+id+'"></div>\r\n';
		s+='<script type="text/javascript">\r\n';
		s+='new MDC_FocusImage('+$.toJSON(d.a)+','+$.toJSON(d.b)+');\r\n';
		s+='</script>';

		p._$txtCode.val(s);
	
		p._$preview.show();
		//show the overlay
		var h0=p._$desiner.height(),h1=p._$preview.height();
		h0=h0>h1?h0:h1;
		p._$previewOvl.show().css("height",h0+20);
	};
	p.onSelect=function(evt){
		if(this.className=="on") return false;
		p._$fiItems.removeClass("on");
		$(this).addClass("on");
		p._$body.removeClass().addClass(this.rel);
		return false;
	};
    //private area
    p.initVar = function(opts) { 
		p._$entry=$("#contentA .entry");
		p._limit=opts.limit||10;
		p._fmCfg={
			h0:$("#txtH"),				/* 焦点图整体高 */	
			w0:$("#txtW"),				/* 焦点图整体宽 */
			h:$("#txtImgH"),			/* 大图高 */
			w:$("#txtImgW"),			/* 大图宽 */
			h1:$("#txtImgH1"),			/* 小图高 */
			w1:$("#txtImgW1"),			/* 小图宽 */
			s:$("#txtSpeed"),
			ddlPos:$("#ddlBtnPos"),
			cbxTxt:$("#cbxShowTxt"),
			color:$("#txtColor"),
			bgColor:$("#txtBGColor"),
			type_02:$("#ddlTypeFI02"),	/* 第2种焦点图的类型 */
			type_03:$("#ddlTypeFI03"),	/* 第3种焦点图的类型 */
			type_04:$("#ddlTypeFI04"),	/* 第3种焦点图的类型 */
			type_05:$("#ddlTypeFI05"),	/* 第3种焦点图的类型 */
			_isValid:true,
			_getData:function(){
				var d={},o;
				for(var c in p._fmCfg){
					if((c.indexOf("_")==0)||p._fmCfg[c].is(":hidden"))
						continue;
						
					o=p._fmCfg[c][0];
					if(p._fmCfg[c].is(".cbx")){
						d[o.name]=o.checked?"1":"0";
					}else{
						d[o.name]=p._fmCfg[c].val();
					};
				};	
				
				return d;
			}
		};
		p._$previewOvl=$("#preview_ovl").css("opacity",0.6);
		p._$preview=$("#preview");
		p._$txtCode=$("#txtCode");
		p._$btnGen=$("#btnGen");
		p._$desiner=$("#designer");
		
		//焦点图列表
		p._$fiItems=$("#fiList_ a");
		//body
		p._$body=$("body");
		//左侧菜单
		p._$fiList=$("#fiList");
		//左侧菜单开关
		p._$toggler=p._$fiList.find(".toggler");
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
		//代码框
		p._$txtCode.click(function(evt){$(this).select();});
		p._$preview.find(".close").click(function(evt){
			p._$previewOvl.hide();
			p._$preview.slideUp("fast");
			return false;
		});
		//焦点图选择列表
		p._$fiItems.click(p.onSelect);
		
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
		
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 