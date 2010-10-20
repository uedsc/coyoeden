/**
 * 简单静态分页逻辑。指定每行多少列，以及每页最多显示的个数。
 * 如果不是最后一页并且li不足整页则自动随机补足
 * @param {Object} $
 * @author levinvan
 * @version 2010.10.20
 */
var sohu_pager = (function($) {
    var p={},pub={};
    //private area
    //从url中解析当前页码
    p.parsePager=function(){
      var ipagerFlag=p._docUrl.lastIndexOf(p._pagerFlag);
      var iSuffix=p._docUrl.lastIndexOf(p._urlExt);
      if((ipagerFlag>0)&&(iSuffix>ipagerFlag)){
		p._cPage=p._docUrl.substring(ipagerFlag+1,iSuffix);p._cPage=parseInt(p._cPage);
		p._cPage=p._mPage-p._cPage;
      };
	  if(p._$pagingTip.length>0){
	      var pagerTip="<span><b>"+(p._cPage+1)+"/"+(p._mPage>p._limitPage?p._limitPage:p._mPage)+"</b></span>";
	      p._$pagingTip.html(pagerTip);	  	
	  };

    };
	//改变url地址
	p.nav=function(p0){
		var url=p._indexUrl;
		if(p0!=1){url=p._indexUrlNoExt+p._pagerFlag+(p._mPage-p0+1)+p._urlExt;};
		document.location.href=url;
	};
	//验证用户页码
	p.validatePage=function(p0){
		p0=parseInt(p0);
		if(isNaN(p0)){return false;};
		return {isOk:true,page:p0};
	};
	//导航，-1表示向前0表示向后
    p.go=function(flag){
	  var retVal=p.validatePage(flag);
	  if(!retVal) return false;
	  //用户自己输入页码
	  if(flag>0){
		if(flag>p._mPage)
			return false;
		p.nav(flag);return false;
      };
	  //向后一页
      if(flag==0){
        if(p._cPage<(p._mPage-1)){
			p.nav(p._cPage+2);
		};
		return false;
      };
	  //向前一页
	  if(flag==-1){
		if(p._cPage!=0){
			p.nav(p._cPage);
		};
		return false;
	  };
	  //用户乱输负数
	  if(flag<-1){return false;};
	  return false;
    };
	p.fixItems=function(){
		if(p._$items.size()<=p._iMax) return;
		var gap=p._iRow-p._$items.size()%p._iRow;
		//随机补足
		for(var i=0;i<gap;i++){
			var iRandom=Math.random()*p._$items.size();iRandom=parseInt(iRandom);
			p._$itemWrap.append(p._$items.eq(iRandom).clone(true).addClass(p._clRDItem));
		};
	};
	p.initVar = function(opts) { 
		p._indexUrl=opts.indexUrl||"/s2008/9975/s260498149/index.shtml";		/* url模式 */
		p._indexUrlNoExt=opts.indexUrlNoExt||"/s2008/9975/s260498149/index";	/* url模式无扩展名 */
		p._docUrl=document.location.href;
		p._mPage=opts.mPage||4;								/* 总页 */
		p._cPage=opts.cPage||0;								/* 当前页 */
		p._limitPage=opts.limitPage||100;					/* 每页最多数 */
		p._pagerFlag=opts.pagerFlag||"_";					/* 页码分隔符 */	
		p._urlExt=opts.urlExt||".shtml";					/* url前缀 */
		p._$prev=$(opts.cssPrev||".lnkPrev");					/* 前一页 */
		p._$next=$(opts.cssNext||".lnkNext");					/* 下一页 */
		p._$last=$(opts.cssLast||".lnkLast");					/* 末页按钮 */
		p._txtPage=$(opts.txtPageID||"#pagenav");			/* 页码输入框 */
		p._btnGo=$(opts.btnGO||"#btnGo");					/* 转到按钮 */
		p._iRow=opts.iRow||4;								/*每行显示数*/
		p._iMax=opts.iMax||15;								/*每页计划显示数*/
	 	p._$pagingTip=$(opts.pagingTip||'.pagingIndex');	/* 分页提示框 */
		p._$itemWrap=$(opts.cssItemWrap||'.apTarget');		/* 待分页元素的老爸 */
		p._$items=$(opts.cssItem||'.apTarget li');			/* 待分页元素 */
		p._clRDItem=opts.clRDItem||'sp_auto_item';		    /* 随机补足元素的css class */
		if(opts.forceCol){									/* 用户提供一个函数设定列数 */
			p._iRow=opts.forceCol();
		};
    };
    p.onLoaded = function() { };
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
        p.parsePager();
		p.fixItems();
		//向前向后按钮事件注册
		p._$prev.click(function(evt){p.go(-1);return false;});
		p._$next.click(function(evt){p.go(0);return false;});
		p._$last.click(function(evt){p.go(p._mPage);return false;});
		//页码输入框回车事件注册
		p._txtPage.keypress(function(evt){
			if(evt.which!=13) return true;
			p._btnGo.trigger("click");
		});
		//按钮事件注册
		p._btnGo.click(function(evt){p.go(p._txtPage.val());return false;});
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
}) (jQuery);
