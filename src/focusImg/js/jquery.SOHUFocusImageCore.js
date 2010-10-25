/**
 * �Ѻ�����ͼ������Ĳ��
 * @author levinhuang
 * @version 2010.10.12
 * @desc �Ѻ�����7�ֽ���ͼ��ʹ��MDC_FocusImage.js.
 * �����Ľ���ͼ�����ǵ���������Զ�̬���ɵ�dom���ݲ��ɼ�������õ�8�ֽ���ͼ�ķ�ʽ,������json���ݶ�̬����
 */
;(function($) {
    // Private functions.
    var p = {cache:{}};
	p.M=function(cfg){
		this.$d=cfg.$d;								/* JQuery���� */
		this.flag=cfg.flag;							/* ����ͼ���ͱ�ʶ����fi01��ʾ����1�Ľ���ͼ */	
		this.id=this.$d[0].id;						/* dom��� */
		this.text=cfg.text||true;					/* �Ƿ���ʾ���� */
		this.speed=parseInt(cfg.speed)||5000;		/* ��ˢ�ٶ� */
		this.hoverStop=cfg.hoverStop;				/* ����Ƶ�tab��ʱ�Ƿ�ֹͣ���� */
		this.type=cfg.type||1;						/* ������ */
		this.place=cfg.place||'fi_tabRB';				/* tab��ǩ��λ�� */
		this.myHtml=cfg.myHtml||"";					/* �û������html.��䵽class='fi_ct'�������� */
		this.ptStepX=cfg.ptStepX||92;				/* tabָ��ˮƽλ�Ʋ��� */
		this.ptStepY=cfg.ptStepY||81;				/* tabָ�봹ֱλ�Ʋ��� */
		this.ptStepX_=cfg.ptStepX_||4;				/* tabָ��ˮƽλ�Ʋ���ƫ��ֵ */
		this.ptStepY_=cfg.ptStepY_||-1;				/* tabָ�봹ֱλ�Ʋ��� ƫ��ֵ*/			
		this.clickTabToNav=cfg.clickTabToNav||false;/* ���tab��ǩ�Ƿ񵼺� */
		this.autoPlay=null;							/* �Զ����ż�ʱ��id */
		this.autoPlay1=null;						/* ����tab������ʱ������һ�β��ŵļ�ʱ��id */
		
		//˽������-_$��ͷ�ı������ڻ���dom��������Ч��
		this._fiObj=null;							/* ����ͼ���ƶ��� */
		this._$tabC=null;							/* tab��ǩ����,init���������� */
		this._$tabs=null;							/* tab��ǩ */
		this._$curTab=null;							/* ��ǰtab��ǩ */
		this._$titleC=null;							/* ��������,init���������� */
		this._$img=null;							/* ͼƬ���� */
		this._$transparentOvl=null;					/* ͸���ɲ� */
		this._$desc=null;							/* �������� */
		this._$pointer=null;						/* ����ͼָ�� */
		this._cfg=cfg;
		this._curLink=null;
		this._$curImg=null;							/* ��ǰСͼ */
        this._tabNum=0;                             /* tab���� */	
		//��������
		this.$d.data("apple",{a:cfg});	
		//��ʼ��
		this.init();
			
	};
	p.M.prototype={
		/**
			��ʼ��(init):
				1���������ʹ�������ͼ���;
				2�������ݻ���ṹ��.
		*/
		init:function(){
			//��ȡ����ͼ���ƶ���
			this._fiObj=p.cache[this.flag];
			if(!this._fiObj) return;
			//�������-������dom
			this._fiObj.init(this);
			//�����html����
			this.$d.find(".fi_ct").append(this.myHtml);
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
					that.alternation((++_index)==that._tabNum?_index=0:_index);
				},that.speed);
			};
			gogo();
			
			//��tab��ǩ��Click�¼�,���ڽ���ͼ�л�
			this._$tabs.each(function(i){
				(function(i,o){
					o=jQuery(o).click(function(){
						if(this.className!="now"){
							clearTimeout(that.autoPlay1);
							clearInterval(that.autoPlay);
							gogo(i);
						};
						if(that.clickTabToNav){
							window.open(that._curLink);
						};
						return false;
					});
					if(that.hoverStop){
						o.mouseenter(function(){
							clearInterval(that.autoPlay);
							clearTimeout(that.autoPlay1);
							if (this.className != "now") {that.alternation(i);};
							return false;
						}).mouseleave(function(){
							clearInterval(that.autoPlay);
							clearTimeout(that.autoPlay1);
							var j=(i+1)==that._tabNum?0:(i+1);
							that.autoPlay1=window.setTimeout(function(){gogo(j);},that.speed);
						});
						
					};
				})(i,this)
			});
						
			if(this._fiObj.initEvts){
				this._fiObj.initEvts(this,gogo);
			};
		},
		/**
			����(alternation):
				1����������ѡ���Ӧ�Ĺ�������;
				2�������л���ʵ��.
		*/
		alternation:function(i){			
			var continueCommon=true;
			//���ƵĽ���Ч��
			if(this._fiObj.alt)
				continueCommon=this._fiObj.alt(this,i);
				
			if(!continueCommon) return;
			//�����ͼ·����������·��,������ͼ�ӽ��Զ���
			this._$img.attr("src",this._$curTab.find("img").attr("src")).css({opacity:0}).stop().animate({opacity:1},500);		
			//��ǩ�л�����
			this._$tabs.filter(".now").removeClass().end().eq(i).addClass("now");	
			
		}
	};
    //main plugin body
	/**
	 * focusImg���
	 * @param {Object} cfg ����ͼ����������
	 */
    $.fn.focusImg = function(cfg) {
        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			cfg.$d=$(this);
        	new p.M(cfg);
		});
    };
    // Public functions.
	/**
	 * ע�ό��ͼ���ƶ���
	 * @param {Object} key ������fi01��ʾ����ͼ����1
	 * @param {Object} fiObj ��{init,initEvts,alt}
	 */
    $.fn.focusImg.Register = function(key,fiObj) {
        p.cache[key]=fiObj;
    };
})(jQuery);