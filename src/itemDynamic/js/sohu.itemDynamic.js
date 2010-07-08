/**
 * @author levinhuang
 * @desc	��̬����Ԫ�ز��
 */
;(function($) {
    // Private functions.
    var p = {};
    p.clone = function(evt) {
		var opts=evt.data;
		opts.itemCnt=p.items(opts).size();
		//onPreAdd�ص�����
		if(opts.onPreAdd&&(!opts.onPreAdd(opts))){
			return false;
		};
		
		var cloned=opts.iModel.clone(opts.cloneEvents).removeClass(opts.cssModel).addClass(opts.cssClone);
		if(opts.after==-1){
			opts.iModel.parent().append(cloned);
		}else{
			if(opts.after){
				if(!opts.externalTrigger){
					cloned.find(opts.btnAdd).hide();
				};
				opts.iModel.after(cloned);
			}else{
				opts.iModel.before(cloned);
			};
		};
		opts.i=cloned;
		opts.subItems=$(opts.subItem,cloned);
		//����Ԫ����
		opts.itemCnt++;
		//�û��Ļص�����
		if(opts.onAdd){
			opts.onAdd(opts);
		}; 
		return false;
	};
	p.del=function(evt){
		var opts=evt.data;
		opts.itemCnt=p.items(opts).size();
		//onPreDel�ص�����
		if(opts.onPreDel&&(!opts.onPreDel(opts))){
			return false;
		};
		
		if(opts.externalTrigger&&(!opts.now)){
			alert("δѡ���κ�Ԫ�أ�");return false;
		};
		if(opts.externalTrigger){
			opts.now.remove();
		}else{
			$(this).parents("."+opts.cssClone).remove();
		};
		//����Ԫ����
		opts.itemCnt--;
		//�û���ɾ���ص�����
		if(opts.onDel){
			opts.onDel(opts);
		};
		return false;
	};
	p.items=function(opts){
		return $(opts.cssItem);
	};
    //main plugin body
    $.fn.iDynamic = function(opts) {
        // Set the options.
        opts = $.extend({}, $.fn.iDynamic.defaults, opts);
		// set model item
		if(this.length>0){opts.iModel=this.eq(0).addClass(opts.cssModel);};
        // Go through the matched elements and return the jQuery object.
        return this.each(function() {
			//Ԫ�������¼�ע��
			var $this=$(this)
			$this.click(function(evt){
				p.items(opts).removeClass(opts.on);
				opts.now=$(this).addClass(opts.on);
				if(opts.click){opts.click(evt,opts);};
				//return false;
			}).hover(function(evt){
				$(this).addClass(opts.over);
				if(opts.mouseOver){opts.mouseOver(evt,opts);};
				return false;
			},function(evt){
				$(this).removeClass(opts.over);
				if(opts.mouseOut){opts.mouseOut(evt,opts);};
				return false;
			});
			//��ť�¼�ע��
			var btnAdd=opts.externalTrigger?$(opts.btnAdd):$(opts.btnAdd,$this);
			var btnDel=opts.externalTrigger?$(opts.btnDel):$(opts.btnDel,$this);
			btnAdd.bind("click",opts,p.clone);
			btnDel.bind("click",opts,p.del);
			
        });
    };
    // Public defaults.
    $.fn.iDynamic.defaults = {
		cssItem:'.iDynamic',/*ĸ�弰��¡��Ԫ�ص�cssѡ����-����*/
		cssModel:'iModel',/*ĸ��Ԫ�ص�class-����*/
		cssClone:'iClone',/*��¡Ԫ�ص�class*/
        btnAdd: '.btnAdd',/*��Ӱ�ťcss selector*/
		btnDel:'.btnDel',/*ɾ����ťcss selector*/
		on:'on',/*Ԫ�ؼ���ʱ��css��*/
		over:'over',/*����ƹ�ʱ��css��*/
		externalTrigger:false,/*btnAdd��btnDel��ť�Ƿ���Ԫ���ⲿ*/
		subItem:'>*',/*��Ԫ��*/
		onPreAdd:null,/*���ǰ�Ļص�����-����ֵΪfalseʱ����ִ����Ӳ���*/
		onAdd:null,/*��̬��Ӻ�Ļص�����*/
		onPreDel:null,/*ɾ��ǰ�Ļص�����-����ֵΪfalseʱ����ִ��ɾ������*/
		onDel:null,/*��̬ɾ����Ļص�����*/
		mouseOver:null,
		mouseOut:null,
		click:null,
		cloneEvents:true,/*����Ԫ��ʱ�Ƿ����¼�������*/
		after:true/*�Ƿ���ĸ��Ԫ��ĩβ��ӱ����Ƶ�Ԫ�ء�����ֵ-1��ʾ��ĸ��Ԫ�صĸ�Ԫ����׷�������Ƶ�Ԫ��*/
    };
    // Public functions.
    $.fn.iDynamic.method1 = function(skinName) {
        return;
    };
})(jQuery); 