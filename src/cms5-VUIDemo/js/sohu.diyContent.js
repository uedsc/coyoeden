/**
 * ��-���ݡ���Ƭ
 * @author levinhuang
 * @param {Object} opts ѡ��{$obj,sec}
 */
sohu.diyContent=function(opts){
	opts=$.extend({},{cl:"ct",clOn:"ctOn",scale:true},opts||{});
	var _this=this;
	this.$Layout=opts.$obj;
	this.Sec=opts.sec;//����
	this.Editor=this.Sec.Editor;//�����༭��
	this.MaxWidth=this.Sec.Width;
	this.ID="ct_"+sohu.diyConsole.RdStr(8);
	//private property
	var p={opts:opts};
	p.mouseEnter=function(evt){
		_this.$Layout.addClass(opts.clOn);
		_this.Editor.CurCT=_this;
		
		//��ק�����¼�
		
		var dim=_this.Dim();
		sohu.diyConsole.Dragger.handle.show()
		.css({width:dim.w,height:dim.h,opacity:0.3})
		.unbind()
		.bind("mousedown",function(evt){
			sohu.diyConsole.Dragger.ing=true;
			sohu.diyConsole.Dragger.obj=_this;
			_this.Sec.Deactive();
			
		}).bind("mouseup",function(evt){
			sohu.diyConsole.Dragger.ing=false;
		});
		_this.$Layout.prepend(sohu.diyConsole.Dragger.handle);
		
	};
	p.mouseLeave=function(evt){
		_this.$Layout.removeClass(opts.clOn);
		sohu.diyConsole.Dragger.handle.remove();
	};
	this.__p=p;
	
	//���ݵ�����¼�
	this.$Layout.mouseenter(p.mouseEnter).mouseleave(p.mouseLeave);
	//�Ƿ�flash
	if(this.$Layout.flash){
		this.ID+="_fl";
		this.$Layout.attr("id",this.ID);
		//��flash������ֳ���
		var fOpt={tplID:this.$Layout.tplID};
		if(opts.scale){fOpt.w=this.MaxWidth;};
		this.$Layout.flashObj=new sohu.diyTp.Flash(fOpt);
		this.$Layout.flashObj.Render(this.$Layout);
	};
};
/**
 * ��ȡ���ݵ�ά����Ϣ
 * @return {Object} {x,y,w,h}
 */
sohu.diyContent.prototype.Dim=function(){
	return {
		x:this.$Layout.offset().left,
		y:this.$Layout.offset().top,
		w:this.$Layout.width(),
		h:this.$Layout.height()
	};
};
/*��̬����*/
/**
 * �����е�domԪ�ع���һ��diyContent ����
 * @param {Object} opts ѡ��
 */
sohu.diyContent.New=function(opts){
	return new sohu.diyContent(opts);
};
