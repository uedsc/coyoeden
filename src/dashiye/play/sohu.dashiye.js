/**
 * @author levinhuang
 * @desc �Ѻ����ִ���ҰJS����������
 */
var Sohu_Dashiye=(function($){
	var p={modules:{}};
	
	pub.Init=function(opts){
		//��ʼ������ģ��
		for(var m in p.modules){
			if(m.Init){
				m.Init(opts);
			};
		};
		
		//$(document).ready��ע��
		$(document).ready(function(){
			for(var m in p.modules){
				if(m.OnLoad){
					m.OnLoad();
				};
			};
		});	
	};
	/**
	 * ���һ��ģ��
	 * @param {Object} key
	 * @param {Object} module
	 */
	pub.AddModule=function(key,module){
		if(p.modules[key]){
			alert("ģ��"+key+"�Ѿ����ڣ�");
			return;
		};
	};
	/**
	 * ��ȡһ��ģ��
	 * @param {Object} key
	 */
	pub.GetModule=function(key){
		return p.modules[key];
	};
	return pub;
})(jQuery);

/**
 * ����ͼƬ������
 * @param {Object} $
 */
(function($){
	//��ͼģ��
	Sohu_Dashiye.AddModule("ZoomInView",{
		Init:function(opts){
			this.o=opts;
			this.data=[];	
		},
		OnLoad:function(){
			var _i=this;
			this.LoadData(function(){
				_i.InitThumbView();
			});
		},
		/**
		 * ��ȡ������������
		 */
		LoadData:function(cbk){
			
		},
		/**
		 * ��ʾ��ͼ
		 * @param {Object} imgData ͼƬ����
		 */
		ZoomIn:function(imgData){
			alert("TODO���ȴ�ʵ��");
		},
		/**
		 * ��ʼ������ͼģ��
		 */
		InitThumbView:function(){
			var m=Sohu_Dashiye.GetModule("ThumbView");
			if(m&&m.InitUI){
				m.InitUI(this,this.data);
			};
		}
	});
	//����ͼģ��-����ʵ���ɶ�������	
	Sohu_Dashiye.AddModule("ThumbView",{
		
	});
})(jQuery);

