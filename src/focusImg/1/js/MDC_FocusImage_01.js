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
		this.text==1?this.$d.css({"height":parseInt(that.imgH)+20+"px"}):this.$d.css({"height":that.imgH+"px"});
		this.$d.css({"width":that.imgW+"px","position":"relative","overflow":"hidden","cursor":"pointer"});
		var _imgBg,_place;
		switch(that.place){
			case "1":
				that.text==1?_place="left:2px;bottom:20px":_place="left:2px;bottom:0";
				_imgBg="height:13px;padding-top:3px;background:url(images/MDC_Focus_bg01.gif) no-repeat bottom";
				break;
			case "2":
				that.text==1?_place="right:2px;bottom:20px":_place="right:2px;bottom:0";
				_imgBg="height:13px;padding-top:3px;background:url(images/MDC_Focus_bg01.gif) no-repeat bottom";
				break;
			case "3":
				_place="left:2px;top:0";
				_imgBg="height:16px;padding-top:0;background:url(images/MDC_Focus_bg02.gif) no-repeat bottom";
				break;
			case "4":
				_place="right:2px;top:0";
				_imgBg="height:16px;padding-top:0;background:url(images/MDC_Focus_bg02.gif) no-repeat bottom";
				break;
		}		
		this._$tabC=jQuery('<div style="height:16px;display:inline;position:absolute;'+_place+'"></div>');
		this._$titleC=jQuery("<div style='height:20px;width:100%;position:absolute;bottom:0;left:0;background:"+that.bgColor+";color:"+that.txtColor+";font:12px/20px \"����\"'></div>");
		this._$img=jQuery('<img style="position:absolute;top:0;left:0;display:block;width:100%;height:'+that.imgH+'px" />');
		this.$d.append(this._$img).append(this._$tabC).append(this._$titleC);			

		//�Ƿ���ʾ�ı�
		this.text==1?this._$titleC.show():this._$titleC.hide();
		
		//�������ݴ���tab��ǩ
		for(var i=0;i<that.focusData.length;i++){
			this._$tabC.append('<span style="'+_imgBg+';width:23px;font:12px/12px \'arial\';color:#fff;float:left;margin-left:-1px">'+(i+1)+'</span>');
		};
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
		this._$tabs.filter(".now").removeClass().css("backgroundPosition","bottom").end()
			.eq(i).addClass("now").css("backgroundPosition","top");
	}
};
