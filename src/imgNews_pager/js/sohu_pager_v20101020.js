/**
 * �򵥾�̬��ҳ�߼���ָ��ÿ�ж����У��Լ�ÿҳ�����ʾ�ĸ�����
 * ����������һҳ����li������ҳ���Զ��������
 * @param {Object} $
 * @author levinvan
 * @version 2010.10.20
 */
var sohu_pager = (function($) {
    var p={},pub={};
    //private area
    //��url�н�����ǰҳ��
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
	//�ı�url��ַ
	p.nav=function(p0){
		var url=p._indexUrl;
		if(p0!=1){url=p._indexUrlNoExt+p._pagerFlag+(p._mPage-p0+1)+p._urlExt;};
		document.location.href=url;
	};
	//��֤�û�ҳ��
	p.validatePage=function(p0){
		p0=parseInt(p0);
		if(isNaN(p0)){return false;};
		return {isOk:true,page:p0};
	};
	//������-1��ʾ��ǰ0��ʾ���
    p.go=function(flag){
	  var retVal=p.validatePage(flag);
	  if(!retVal) return false;
	  //�û��Լ�����ҳ��
	  if(flag>0){
		if(flag>p._mPage)
			return false;
		p.nav(flag);return false;
      };
	  //���һҳ
      if(flag==0){
        if(p._cPage<(p._mPage-1)){
			p.nav(p._cPage+2);
		};
		return false;
      };
	  //��ǰһҳ
	  if(flag==-1){
		if(p._cPage!=0){
			p.nav(p._cPage);
		};
		return false;
	  };
	  //�û����为��
	  if(flag<-1){return false;};
	  return false;
    };
	p.fixItems=function(){
		if(p._$items.size()<=p._iMax) return;
		var gap=p._iRow-p._$items.size()%p._iRow;
		//�������
		for(var i=0;i<gap;i++){
			var iRandom=Math.random()*p._$items.size();iRandom=parseInt(iRandom);
			p._$itemWrap.append(p._$items.eq(iRandom).clone(true).addClass(p._clRDItem));
		};
	};
	p.initVar = function(opts) { 
		p._indexUrl=opts.indexUrl||"/s2008/9975/s260498149/index.shtml";		/* urlģʽ */
		p._indexUrlNoExt=opts.indexUrlNoExt||"/s2008/9975/s260498149/index";	/* urlģʽ����չ�� */
		p._docUrl=document.location.href;
		p._mPage=opts.mPage||4;								/* ��ҳ */
		p._cPage=opts.cPage||0;								/* ��ǰҳ */
		p._limitPage=opts.limitPage||100;					/* ÿҳ����� */
		p._pagerFlag=opts.pagerFlag||"_";					/* ҳ��ָ��� */	
		p._urlExt=opts.urlExt||".shtml";					/* urlǰ׺ */
		p._$prev=$(opts.cssPrev||".lnkPrev");					/* ǰһҳ */
		p._$next=$(opts.cssNext||".lnkNext");					/* ��һҳ */
		p._$last=$(opts.cssLast||".lnkLast");					/* ĩҳ��ť */
		p._txtPage=$(opts.txtPageID||"#pagenav");			/* ҳ������� */
		p._btnGo=$(opts.btnGO||"#btnGo");					/* ת����ť */
		p._iRow=opts.iRow||4;								/*ÿ����ʾ��*/
		p._iMax=opts.iMax||15;								/*ÿҳ�ƻ���ʾ��*/
	 	p._$pagingTip=$(opts.pagingTip||'.pagingIndex');	/* ��ҳ��ʾ�� */
		p._$itemWrap=$(opts.cssItemWrap||'.apTarget');		/* ����ҳԪ�ص��ϰ� */
		p._$items=$(opts.cssItem||'.apTarget li');			/* ����ҳԪ�� */
		p._clRDItem=opts.clRDItem||'sp_auto_item';		    /* �������Ԫ�ص�css class */
		if(opts.forceCol){									/* �û��ṩһ�������趨���� */
			p._iRow=opts.forceCol();
		};
    };
    p.onLoaded = function() { };
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
        p.parsePager();
		p.fixItems();
		//��ǰ���ť�¼�ע��
		p._$prev.click(function(evt){p.go(-1);return false;});
		p._$next.click(function(evt){p.go(0);return false;});
		p._$last.click(function(evt){p.go(p._mPage);return false;});
		//ҳ�������س��¼�ע��
		p._txtPage.keypress(function(evt){
			if(evt.which!=13) return true;
			p._btnGo.trigger("click");
		});
		//��ť�¼�ע��
		p._btnGo.click(function(evt){p.go(p._txtPage.val());return false;});
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
}) (jQuery);
