/**
 * ��-���ݡ���Ƭ
 * @author levinhuang
 * @param {Object} opts ѡ��
 */
sohu.diyContent=function(opts){
	opts=$.extend({},{clOn:"ctOn"},opts||{});
	var _this=this;
	this.$Layout=opts.$obj;
	this.Sec=opts.sec;//����
	
	//private property
	var p={opts:opts};
	this.__p=p;
	//���ݵ�����¼�
};
