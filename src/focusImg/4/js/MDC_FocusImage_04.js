var MDC_FocusImage=function(cfg,data){
	this.$d=jQuery(cfg.id);	
	this.id=this.$d[0].id;
	this.imgW=parseInt(cfg.imgW);				/* ��ͼ�� */
	this.imgH=parseInt(cfg.imgH);				/* ��ͼ�� */
	this.imgW1=parseInt(cfg.imgW1);				/* Сͼ�� */
	this.imgH1=parseInt(cfg.imgH1);				/* Сͼ�� */
	this.W=parseInt(cfg.W);						/* �� */
	this.H=parseInt(cfg.H);						/* �� */
	this.speed=parseInt(cfg.speed)||5000;
	this.type=cfg.type;							/* ����1=����,2=��¼Ƭ */
	this.focusData=data;
	this.autoPlay;
	
	//˽������-���ڻ���dom��������Ч��
	this._$tabC=null;							/* tab��ǩ����,init���������� */
	this._$tabs=null;							/* tab��ǩ */
	this._$titleC=null;							/* ��������,init���������� */
	this._$img=null;							/* ͼƬ���� */
	this._$transparentOvl=null;					/* ͸���ɲ� */
	this._$desc=null;							/* �������� */
	this._$pointer=null;						/* ����ͼָ�� */	
	//��������
	this.$d.data("apple",{a:cfg,b:data});	
	//��ʼ��
	this.init();
		
};
MDC_FocusImage.prototype={
	/**
		��ʼ��(init):
			1���������ʹ�������ͼ���;
			2�������ݻ���ṹ��.
	*/
	init:function(){
		
		//�������-�ȹ���html������$(html)����dom����,������ʹ��$(html)
		var html='<div class="fi04_'+this.type+'"'+ 'id="'+this.id+'_'+this.type+'">';
		html+='<div class="content">';
		html+='<img style="display:block;"/><div class="transparence"></div><h2></h2><p></p><div class="playButton"></div>';
		html+='</div>';
		html+='<div class="tab"><span class="pointer"></span><dl></dl></div>';
		html+='</div>';
		this.$d.css("width",this.W).append(html);
		
		//��ʽ����dom����
		this.$d.children().css({width:this.W,height:this.H});
		this._$transparentOvl=this.$d.find(".transparence").css("opacity",0.5);
		this._$img=this.$d.find("img").css({width:this.imgW,height:this.imgH});
		this._$titleC=this.$d.find("h2");
		this._$desc=this.$d.find("p");
		this._$tabC=this.$d.find("dl");
		this._$pointer=this.$d.find(".pointer");
		
		//�������ݴ���tab��ǩ
		html="";
		for(var i=0;i<this.focusData.length;i++){
			this.focusData[i].p1=((!this.focusData[i].p1)||(this.focusData[i].p1==""))?this.focusData[i].p:this.focusData[i].p1;
			html+='<dd><img src="'+this.focusData[i].p1+'" width="'+this.imgW1+'" height="'+this.imgH1+'" style="display:block;"/></dd>';
		};
		this._$tabC.append(html).find("img").css("opacity",0.5);
		this._$tabs=this._$tabC.find("dd");
				
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
				jQuery(o).mouseenter(function(){
					if(this.className!="now"){
						clearTimeout(that.autoPlay);
						gogo(i);
					};
					return false;
				});
			})(i,this)
		});
					
		//����������Click�¼�,���ڵ�������
		this.$d.find(".content").click(function(){
			window.open(that._$img.attr("link"));
		});
		this._$pointer.click(function(){
			window.open(that._$img.attr("link"));
		})
	},
	/**
		����(alternation):
			1����������ѡ���Ӧ�Ĺ�������;
			2�������л���ʵ��.
	*/
	alternation:function(i){			
		//��ǩ�л�����,Сͼ���Զ���
		this._$tabs.filter(".now").removeClass().children().animate({"opacity":.5},200);
		this._$tabs.eq(i).addClass("now").children().animate({"opacity":1},200);
		//�����ͼ·����������·��,������ͼ�ӽ��Զ���
		this._$img.attr({"src":this.focusData[i].p,"link":this.focusData[i].l}).css({opacity:0}).stop().animate({opacity:1},300);
		
		//ָ���������				
		switch(this.type){
			case "1":
				this._$pointer.stop().animate({"left":i*92+4},200);
				break;
			case "2":
				this._$pointer.stop().animate({"top":i*81-1},200);
				break;
		};		
				
		//���ݻ���
		this._$titleC.html(this.focusData[i].t);
		this._$desc.html(this.focusData[i].t1);
	}
};