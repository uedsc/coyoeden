/**
 * @author levinhuang
 * @desc	动态复制元素插件
 */
;(function($) {
    // Private functions.
    var p = {};
    p.clone = function(evt) {
		var opts=evt.data;
		opts.itemCnt=p.items(opts).size();
		//onPreAdd回调函数
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
		//更新元素数
		opts.itemCnt++;
		//用户的回调函数
		if(opts.onAdd){
			opts.onAdd(opts);
		}; 
		return false;
	};
	p.del=function(evt){
		var opts=evt.data;
		opts.itemCnt=p.items(opts).size();
		//onPreDel回调函数
		if(opts.onPreDel&&(!opts.onPreDel(opts))){
			return false;
		};
		
		if(opts.externalTrigger&&(!opts.now)){
			alert("未选中任何元素！");return false;
		};
		if(opts.externalTrigger){
			opts.now.remove();
		}else{
			$(this).parents("."+opts.cssClone).remove();
		};
		//更新元素数
		opts.itemCnt--;
		//用户的删除回调函数
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
			//元素自身事件注册
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
			//按钮事件注册
			var btnAdd=opts.externalTrigger?$(opts.btnAdd):$(opts.btnAdd,$this);
			var btnDel=opts.externalTrigger?$(opts.btnDel):$(opts.btnDel,$this);
			btnAdd.bind("click",opts,p.clone);
			btnDel.bind("click",opts,p.del);
			
        });
    };
    // Public defaults.
    $.fn.iDynamic.defaults = {
		cssItem:'.iDynamic',/*母板及克隆的元素的css选择器-必须*/
		cssModel:'iModel',/*母板元素的class-必须*/
		cssClone:'iClone',/*克隆元素的class*/
        btnAdd: '.btnAdd',/*添加按钮css selector*/
		btnDel:'.btnDel',/*删除按钮css selector*/
		on:'on',/*元素激活时的css类*/
		over:'over',/*鼠标移过时的css类*/
		externalTrigger:false,/*btnAdd和btnDel按钮是否在元素外部*/
		subItem:'>*',/*子元素*/
		onPreAdd:null,/*添加前的回调函数-返回值为false时将不执行添加操作*/
		onAdd:null,/*动态添加后的回调函数*/
		onPreDel:null,/*删除前的回调函数-返回值为false时将不执行删除操作*/
		onDel:null,/*动态删除后的回调函数*/
		mouseOver:null,
		mouseOut:null,
		click:null,
		cloneEvents:true,/*复制元素时是否复制事件处理函数*/
		after:true/*是否在母板元素末尾添加被复制的元素。特殊值-1表示在母板元素的父元素内追增被复制的元素*/
    };
    // Public functions.
    $.fn.iDynamic.method1 = function(skinName) {
        return;
    };
})(jQuery); 