var $this=function(){
	
	var p={},pub={};
	p.showNum=5;
	p.slideNum=0;
	
	//����
	p.onNavL=function(evt){
		p.slider.animate({left:'+=135',opacity: 0.5},"normal",function(){
			p.slider.css({opacity:1});
		});
		p.slideNum++;
		
		if(p.slideNum>=(p.total-p.showNum)){
			//��߰�ť��ң�ͬʱ�Ƴ��󶨵��¼�
			p.btnL.addClass("noNav").unbind("click");
			//�ұ߱�����ͬʱ���¼�
			p.btnR.removeClass("noNav").bind("click",p.onNavR);
			//return false;
			//����������
			p.slideNum=0;
		};
		
		return false;
	};
	
	p.onNavR=function(evt){

		p.slider.animate({left:'-=135',opacity: 0.5},"normal",function(){
			p.slider.css({opacity:1});
		});
		p.slideNum++;
		
		if(p.slideNum>=(p.total-p.showNum)){
			//�ұ߰�ť��ң�ͬʱ�Ƴ��󶨵��¼�
			p.btnR.addClass("noNav").unbind("click");
			//��߱�����ͬʱ���¼�
			p.btnL.removeClass("noNav").bind("click",p.onNavL);
			//return false;
			//����������
			p.slideNum=0;
		};
		
		return false;
	};
	p.onLoad=function(){
		var items=jQuery("#tab_10_1 .team");
		p.total=items.length;
		p.btnL=jQuery("#tab_10_1 .btnL"),p.btnR=jQuery("#tab_10_1 .btnR");
		p.slider=jQuery("#tab_10_1 .slider");
		
		if(p.total<=p.showNum){
			p.btnL.addClass("noNav");
			p.btnR.addClass("noNav");
			return;
		};
		
		//�ұ߰�ť
		p.btnR.click(p.onNavR);	
	};
	pub.Init=function(){
		
		jQuery(document).ready(p.onLoad);
	};
	
	return pub;
}();



