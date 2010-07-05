/**
 * @author levinhuang
 */
var sohu_cms5_treeNav= function() {
    var p={},pub={};
	p.resetMenu=function(opts){
	///<summary>����һ���˵�</summary>
		if(opts.t.hasClass("collapsable")) return;
		$("#leftNav .panel").filter(":visible").slideUp("slow");
		$("#leftNav a.root").removeClass("collapsable");
	};
	p.createNode=function(parent) {
	///<summary>ȫ��Ƶ��-�������β˵�</summary>
		var meta={id:this.id||"",layer:this.layer||0,channelid:this.channelid||0};
		var current = $("<li/>").attr("id", $.toJSON(meta)).html("<a title='' href='#'>" + this.text + "</a>").appendTo(parent);
		if (this.classes) {
			current.children("span").addClass(this.classes);
		};
		if (this.expanded) {
			current.addClass("open");
		};
		if(this.href){
			current.find("a").attr("href",this.href);
		};
		current.find("a").click(function(evt){$(this).prev().trigger("click");return false;});
		if (this.hasChildren || this.children && this.children.length) {
			var branch = $("<ul/>").appendTo(current);
			if (this.hasChildren) {
				current.addClass("hasChildren");
				p.createNode.call({
					text:"placeholder",
					id:"placeholder",
					children:[]
				}, branch);
			}
			if (this.children && this.children.length) {
				$.each(this.children, p.createNode, [branch])
			}
		}
	};
	p.onClickAllChannel=function(evt){
	///<summary>ȫ��Ƶ��</summary>	
		var _this=$(this);
		p.resetMenu({t:_this}); 
		_this.addClass("collapsable");
		$("#nav_allCList").empty().treeview({url:p._treeDataUrl,root:null,createNode:p.createNode,json:true}).show();
		return false;
	};
	p.onClickFav=function(evt){
	///<summary>�����ղؼ�</summary>
		var _this=$(this);
		p.resetMenu({t:_this}); 

		return false;
	};
	p.onClickHis=function(evt){
	///<summary>��ʷ��¼</summary>
		var _this=$(this);
		p.resetMenu({t:_this});
		_this.addClass("collapsable");
		_this.parent().next().show(); 
		return false;
	};
	p.onClickFavC=function(evt){
	///<summary>Ƶ���ղؼ�</summary>
		var _this=$(this);
		p.resetMenu({t:_this}); 
		return false;
	};
	p.loadHis=function(opts){
	///<summary>��ʷ��¼����</summary>
		var d=opts.d||Date.today().toString("yyyy-MM-dd");
		var data={d:d};
		$.getJSON(p._hisDataUrl+"?callback=?",data,function(json){
			p._hisList.empty();
			if((!json.tips)||(json.tips.length==0)){
				p._hisList.append(p._hisItemTpl0);
				return;
			};
			$(json.tips).each(function(i,tip){
				p._hisList.append(StringUtils.parseTpl(p._hisItemTpl,{time:tip.lastvist.substring(11,16),url:tip.url||"#",title:tip.name}));
			});
		});
	};
	p.onClickHisNav=function(evt){
	///<summary>��ʷ��¼����</summary>
		var flag=evt.data.flag;//-1ǰһ��;1��һ��
		var f0="yyyy-MM-dd";
		var d1=p._txtHisDate.val();
		var d2=Date.today();
		if(p._hisDate0==d1){d1=d2;}else{d1=Date.parse(d1);};
		d1=d1.addDays(flag);
		var val=d1.is().today()?p._hisDate0:d1.toString(f0);
		p._txtHisDate.val(val);	
		//load data from server
		p.loadHis({d:d1.toString(f0)});
		return false;
	};
    //private area
    p.initVar = function(opts) {
		p._treeDataUrl=opts.treeDataUrl;	
		p._hisDataUrl=opts.hisDataUrl;
		p._txtHisDate=$("#txtHisDate");
		p._hisDate0="��  ��";
		p._hisList=$("#nav_hisList");
		p._hisItemTpl='<li><span class="time">%time%</span><span class="icon">&nbsp;&nbsp;</span><a href="%url%" title="">%title%</a></li>';
		p._hisItemTpl0='<li class="empty"><span class="icon">&nbsp;&nbsp;</span></li>';
	};
    p.onLoaded = function() { 
		p._txtHisDate.val(p._hisDate0);
		p.loadHis({});
	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		$("#lnkAllC").click(p.onClickAllChannel);
		$("#lnkFav").click(p.onClickFav);
		$("#lnkHis").click(p.onClickHis);
		$("#lnkFavC").click(p.onClickFavC);
		$("#btnHisPrev").bind("click",{flag:-1},p.onClickHisNav);
		$("#btnHisNext").bind("click",{flag:1},p.onClickHisNav);
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
    return pub;
} (); 