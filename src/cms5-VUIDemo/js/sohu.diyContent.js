/**
 * ��-���ݡ���Ƭ
 * @author levinhuang
 * @param {Object} opts ѡ��{$obj,sec}
 */
sohu.diyContent=function(opts){
	opts=$.extend({},{cl:"ct",clOn:"ctOn"},opts||{});
	var _this=this;
	this.$Layout=opts.$obj;
	this.Sec=opts.sec;//����
	this.MaxWidth=this.Sec.Width;
	this.ID="ct_"+sohu.diyConsole.RdStr(8);
	//private property
	var p={opts:opts};
	p.mouseEnter=function(evt){
		_this.$Layout.addClass(opts.clOn).css("opacity",0.6);
	};
	p.mouseLeave=function(evt){
		_this.$Layout.removeClass(opts.clOn).css("opacity",1);
	};
	this.__p=p;
	
	//���ݵ�����¼�
	this.$Layout.mouseenter(p.mouseEnter).mouseleave(p.mouseLeave);
	//�Ƿ�flash
	if(this.$Layout.flash){
		this.ID+="_fl";
		this.$Layout.attr("id",this.ID);
		//��flash������ֳ���
		this.$Layout.flashObj=new sohu.diyTp[this.$Layout.tplID]({tplID:this.$Layout.tplID,w:this.MaxWidth});
		this.$Layout.flashObj.Render(this.$Layout);
	};
};
