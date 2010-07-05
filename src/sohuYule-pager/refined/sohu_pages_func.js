var this$ = function() {
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
      var pagerTip="<span><b>"+(p._cPage+1)+"/"+(p._mPage>p._limitPage?p._limitPage:p._mPage)+"</b></span>";
      $("#pagingIndex").html(pagerTip);
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
		var items=$(".autoPic li");
		if(items.size()<=p._iMax) return;
		//ͨ��class��������
		var classList=$(".autoPic").attr("class").split(" ");
		$.each(classList,function(i,obj){
			obj=$.trim(obj);
			if(obj[0]=='c'){
				p._iRow=parseInt(obj[1]);
				p._iRow=p._iRow||5;
			};
		});
		var gap=p._iRow-items.size()%p._iRow;
		var ul=items.parent();
		//�������
		for(var i=0;i<gap;i++){
			var iRandom=Math.random()*items.size();iRandom=parseInt(iRandom);
			ul.append(items.eq(iRandom).clone());
		};
	};
	p.initVar = function(opts) { 
      p._indexUrl=opts.indexUrl||"/s2008/9975/s260498149/index.shtml";
      p._indexUrlNoExt=opts.indexUrlNoExt||"/s2008/9975/s260498149/index";
      p._docUrl=document.location.href;
      p._mPage=opts.mPage||4;
      p._cPage=opts.cPage||0;
      p._limitPage=opts.limitPage||100;
      p._pagerFlag=opts.pagerFlag||"_";	
      p._urlExt=opts.urlExt||".shtml";
      p._prevID=opts.prevID||"#lnkPrev";
      p._nextID=opts.nextID||"#lnkNext";
	  p._txtPage=$("#pagenav");p._btnGo=$("#btnGo");
	  p._iRow=opts.iRow||5;/*ÿ����ʾ��*/
	  p._iMax=opts.iMax||15;/*ÿҳ�ƻ���ʾ��*/
    };
    p.onLoaded = function() { };
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
        p.parsePager();
		p.fixItems();
		//��ǰ���ť�¼�ע��
		$(p._prevID).click(function(evt){p.go(-1);return false;});
		$(p._nextID).click(function(evt){p.go(0);return false;});
		$("#lnkLast").click(function(evt){p.go(p._mPage);return false;});
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
} ();