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
			d={
				p:$o.find(".imgSrc").val(),
				l:$o.find(".imgLnk").val(),
				t:$o.find(".imgTt").val(),
				p1:$o.find(".imgSrc1").val()
			};
			d.p1=d.p1==""?d.p:d.p1;
			r.push(d);
		});
		return r;
	};
	p.onValidate=function(){
		p._fmCfg.isValid=true;
		//validate configuration data
		var ok=p.assertIsInt([p._fmCfg.h,p._fmCfg.w,p._fmCfg.s]);
		p._fmCfg.isValid=p._fmCfg.isValid&&ok;
		if(!p._fmCfg.isValid) return false;
		//validate image data
		var objs=$("#designer .imgSrc");
		ok=p.assertIsUrl(objs);
		if(!ok) return false;
		//validate href data
		objs=$("#designer .imgLnk");
		ok=p.assertIsUrl(objs);
		if(!ok) return false;
		
		//everything is ok
		var retVal={
			a:p._fmCfg.getData()
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
    //private area
    p.initVar = function(opts) { 
		p._$entry=$("#contentA .entry");
		p._limit=opts.limit||10;
		p._fmCfg={
			h:$("#txtImgH"),
			w:$("#txtImgW"),
			s:$("#txtSpeed"),
			ddlPos:$("#ddlBtnPos"),
			cbxTxt:$("#cbxShowTxt"),
			color:$("#txtColor"),
			bgColor:$("#txtBGColor"),
			isValid:true,
			getData:function(){
				return {
					imgW:p._fmCfg.w.val(),
					imgH:p._fmCfg.h.val(),
					speed:p._fmCfg.s.val(),
					place:p._fmCfg.ddlPos.val(),
					text:p._fmCfg.cbxTxt[0].checked?"1":"0",
					txtColor:p._fmCfg.color.val(),
					bgColor:p._fmCfg.bgColor.val()
				};
			}
		};
		p._$previewOvl=$("#preview_ovl").css("opacity",0.6);
		p._$preview=$("#preview");
		p._$txtCode=$("#txtCode");
		p._$btnGen=$("#btnGen");
		p._$desiner=$("#designer");
	};
    p.onLoaded = function() { };
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		p._$entry.find(".del").click(p.onDel).end().find(".add").click(p.onAdd);
		p._$entry.find(".imgSrc,.imgLnk").change(function(evt){
			p.onValidate();
		});
		p._$btnGen.click(p.onGen);
		p._$txtCode.click(function(evt){$(this).select();});
		p._$preview.find(".close").click(function(evt){
			p._$previewOvl.hide();
			p._$preview.slideUp("fast");
		});
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 