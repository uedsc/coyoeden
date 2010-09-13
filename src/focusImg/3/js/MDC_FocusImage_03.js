var MDC_FocusImage=function(cfg,data){
	this.$d=jQuery(cfg.id);
	this.id=this.$d[0].id;	
	this.imgW=parseInt(cfg.imgW);
	this.imgH=parseInt(cfg.imgH);
	this.speed=parseInt(cfg.speed)||5000;
	this.focusData=data;
	this.type=cfg.type;							/* ����1=V��,2=��Ӱ,3=����,4=����,5=Ϸ�� */
	this.autoPlay;
	//˽������-���ڻ���dom��������Ч��
	this._$tabC=null;							/* tab��ǩ����,init���������� */
	this._$tabs=null;							/* tab��ǩ */
	this._$titleC=null;							/* ��������,init���������� */
	this._$img=null;							/* ͼƬ���� */
	this._$transparentOvl=null;					/* ͸���ɲ� */
	this._$desc=null;							/* �������� */	
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
		//�������-ע:����html�ַ���Ȼ��һ���Թ���dom����ȶ��ʹ��$(html)��Ч
		var html='<div class="fi03_'+this.type+'"'+ 'id="'+this.id+'_'+this.type+'">';
		html+='<div class="content">';
		html+='<img width="'+this.imgW+'" height="'+this.imgH+'"/><div class="transparence"></div><h2></h2><p></p><div class="playButton"></div>';
		html+='</div>';
		html+='<div class="tab"></div>';
		html+='</div>';
		this.$d.css("width",this.imgW).append(html);
		//��ʽ����dom����
		this.$d.children().css("width",this.imgW);
		this._$transparentOvl=this.$d.find(".transparence").css("opacity",0.5);
		this._$img=this.$d.find("img");
		this._$titleC=this.$d.find("h2");
		this._$desc=this.$d.find("p");
		this._$tabC=this.$d.find(".tab");
		
		//�������ݴ���tab��ǩ-ע����̬�������domʱ���ȹ���������html����Ч��
		html="";
		for(var i=0;i<this.focusData.length;i++){
			html+="<span>"+(i+1)+"</span>";
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
		this.$d.find(".content").click(function(){
			window.open(that._$img.attr("link"));
		})
	},
	/**
		����(alternation):
			1����������ѡ���Ӧ�Ĺ�������;
			2�������л���ʵ��.
	*/
	alternation:function(i){			
		//��ǩ�л�����
		this._$tabs.filter(".now").removeClass().end().eq(i).addClass("now");
		
		//͸��������
		if(this.type==1){			
			this._$transparentOvl.css({
				left:-600
			}).stop().animate({"left":0},200);				
		};
		
		//�����ͼ·����������·��,������ͼ�ӽ��Զ���
		this._$img.attr({"src":this.focusData[i].p,"link":this.focusData[i].l}).css({"opacity":0}).stop().animate({opacity:1},300);
		
		//���ݻ���
		this._$titleC.html(this.focusData[i].t);
		this._$desc.html(this.focusData[i].t1);
	}
};