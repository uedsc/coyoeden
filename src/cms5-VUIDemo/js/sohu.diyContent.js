/**
 * 类-内容、碎片
 * @author levinhuang
 * @param {Object} opts 选项
 */
sohu.diyContent=function(opts){
	opts=$.extend({},{clOn:"ctOn"},opts||{});
	var _this=this;
	this.$Layout=opts.$obj;
	this.Sec=opts.sec;//分栏
	
	//private property
	var p={opts:opts};
	this.__p=p;
	//内容的鼠标事件
};
