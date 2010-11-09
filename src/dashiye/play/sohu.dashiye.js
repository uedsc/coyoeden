/**
 * @author levinhuang
 * @desc 搜狐娱乐大视野JS交互工厂类
 */
var Sohu_Dashiye=(function($){
	var p={modules:{}};
	
	pub.Init=function(opts){
		//初始化所有模块
		for(var m in p.modules){
			if(m.Init){
				m.Init(opts);
			};
		};
		
		//$(document).ready的注册
		$(document).ready(function(){
			for(var m in p.modules){
				if(m.OnLoad){
					m.OnLoad();
				};
			};
		});	
	};
	/**
	 * 添加一个模块
	 * @param {Object} key
	 * @param {Object} module
	 */
	pub.AddModule=function(key,module){
		if(p.modules[key]){
			alert("模块"+key+"已经存在！");
			return;
		};
	};
	/**
	 * 获取一个模块
	 * @param {Object} key
	 */
	pub.GetModule=function(key){
		return p.modules[key];
	};
	return pub;
})(jQuery);

/**
 * 首屏图片播放器
 * @param {Object} $
 */
(function($){
	//大图模块
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
		 * 获取服务器端数据
		 */
		LoadData:function(cbk){
			
		},
		/**
		 * 显示大图
		 * @param {Object} imgData 图片数据
		 */
		ZoomIn:function(imgData){
			alert("TODO：等待实现");
		},
		/**
		 * 初始化缩略图模块
		 */
		InitThumbView:function(){
			var m=Sohu_Dashiye.GetModule("ThumbView");
			if(m&&m.InitUI){
				m.InitUI(this,this.data);
			};
		}
	});
	//缩略图模块-具体实现由杜鹏负责	
	Sohu_Dashiye.AddModule("ThumbView",{
		
	});
})(jQuery);

