var MDC_FocusImage=function(cfg,data){
	this.$d=jQuery(cfg.id);						/* dom jquery���� */
	this.id=this.$d[0].id;						/* dom��� */	
	this.imgW=parseInt(cfg.imgW);
	this.imgH=parseInt(cfg.imgH);
	this.speed=parseInt(cfg.speed)||5000;
	this.place=cfg.place;
	this.text=cfg.text;
	this.txtColor=cfg.txtColor;
	this.bgColor=cfg.bgColor;
	this.focusData=data;
	this.autoPlay;
	//˽������-����dom����
	this._$tabC=null;							/* tab��ǩ����,init���������� */
	this._$tabs=null;							/* tab��ǩ */
	this._$titleC=null;							/* ��������,init���������� */
	this._$img=null;							/* ͼƬ���� */
	//��������
	this.$d.data("apple",{a:cfg,b:data});
	//init
	this.init();
};
MDC_FocusImage.prototype={
	/**
		��ʼ��(init):
			1���������ʹ�������ͼ���;
			2�������ݻ���ṹ��.
	*/
	init:function(){
		var that=this;
		//�������
		var html='<img height="'+this.imgH+'"/>';
		html+='<div class="tab '+this.place+'"></div>';
		html+='<div class="tt"></div>';
		this.$d.append(html);
		//��ʽ����dom����
		this._$tabC=this.$d.find(".tab");
		this._$titleC=this.$d.find(".tt").css({background:this.bgColor,color:this.txtColor});
		this._$img=this.$d.find("img");
		
		if(this.text){
			var h0=this._$titleC.height();
			this._$titleC.show();
			this.$d.css({width:this.imgW,height:this.imgH+h0});
			this._$tabC.css("bottom",h0);
		}else{
			this._$titleC.hide();
			this.$d.css({width:this.imgW,height:this.imgH});
		};
		
		//�������ݴ���tab��ǩ
		html="";
		for(var i=0;i<that.focusData.length;i++){
			html+='<span>'+(i+1)+'</span>';
		};
		this._$tabC.append(html);
		this._$tabs=this._$tabC.find("span");
		//ִ�д���
		this.spring();
		
	},
	/**
		����(spring):
			1��tab�л��¼��İ�;
			2����ʱ����ˢ��ʵ��.
	*/
	spring:function(){
		var that=this;
		
		//ִ�н�������ˢ
		var gogo=function(i){
			var _index=i||0;
			that.alternation(_index);
			that.autoPlay=setInterval(function(){
				that.alternation((++_index)==that.focusData.length?_index=0:_index);
			},that.speed);
		};
		gogo();
		
		//��tab��ǩ��Click�¼�,���ڽ���ͼ�л�
		this._$tabs.each(function(i){
			(function(i,o){
				jQuery(o).click(function(){
					if(this.className!="now"){
						clearTimeout(that.autoPlay);
						gogo(i);
					};
					return false;
				});
			})(i,this)
		});
		
		//����������Click�¼�,���ڵ�������
		this.$d.click(function(){
			window.open(that._$img.attr("link"));
		});
	},
	/**
		����(alternation):
			1����������ѡ���Ӧ�Ĺ�������;
			2�������л���ʵ��.
	*/
	alternation:function(i){
		//�����ͼ·����������·��,������ͼ�ӽ��Զ���
		this.$d.attr("href",this.focusData[i].l);
		this._$img.attr({"src":this.focusData[i].p,"link":this.focusData[i].l}).css({"opacity":0}).stop().animate({opacity:1},1000);
		
		//���ݻ���
		this._$titleC.html(this.focusData[i].t);
		//��ǩ�л�����
		this._$tabs.filter(".now").removeClass().end()
			.eq(i).addClass("now");		
	}
};
