/**
 * ���ӻ��༭Ԫ��
 * @dependency sohu.iframeEX,sohu.iEditable,jquery.cssEX
 * @author levin
 */
sohu.diyElement=function(opts){
	opts=$.extend({},{cl:"elm",clOn:"elmOn",inlineEditable:true},opts||{});
	var _this=this;
	this.CT=opts.ct;
	this.$Context=this.CT.$Layout;
	this.$Layout=opts.$dom;
	if(opts.inlineEditable){
		this.$Layout.iEditable();
	};
	//����¼�
	this.$Layout.mouseenter(function(evt){
		_this.$Layout.addClass(opts.clOn);
	});
	this.$Layout.mouseleave(function(evt){
		_this.$Layout.removeClass(opts.clOn);
	});
	this.$Layout.click(function(evt){
		if(sohu.diyConsole.CurElm)
			sohu.diyConsole.CurElm.$Layout[0].iEdit("off");
			
		sohu.diyConsole.CurElm=_this;
		
	});
	//���γ�����
	this.$Context.find("a").css("cursor","text").click(function(evt){return false;});
};
