var $this=function(){
	
	var p={},pub={};
	//元素名称：m_n; tab_m_n
	//count:交换的个数
	p.showToptab=function(id){
	  var $=jQuery;
		for(var i=1;i<=count;i++){
			if (i==n){
				$("#td_"+id).addClass("libg");
				$("#tab_"+id).show();
				}
			else {
				$("#td_"+m+"_"+i).removeClass("libg");
				$("#tab_"+m+"_"+i).hide();
				}
		}
	};
	p.clickTopNav=function(evt){
		p._$btnSportsList.removeClass("on");
		var $this=jQuery(this).addClass("on");
		
		//找到li标签的id
		p._$liSportsList.removeClass("libg");
		var id=$this.parent().addClass("libg").attr("id");
		id=id.replace("td","");
		p._$tabs.hide().filter("#tab_"+id).show();
		return false;
	};
	p.initFocusSlide=function(){
		var $s=jQuery("#focusSlide");
		var $items=$s.find(".item");
		var $btns=$s.find(".num").find("a");
		
		var _curIdx=0,_t=null;
		
		//显示单张
		var slide=function(idx0){
			if(idx0){
				_curIdx=idx0-1;	
			}else{
				_curIdx++;
				if(_curIdx>=$items.length){
					_curIdx=0;
				};
			};
			
			$items.removeClass("on").hide().eq(_curIdx).show().addClass("on");
			$btns.removeClass("on").eq(_curIdx).addClass("on");
		};
		//自动滚动
		var cycle=function(){
			window.clearInterval(_t);
			_t=window.setInterval(function(){slide(null);},2000);
		};
		
		//a标签事件注册
		$btns.mouseover(function(evt){
			var _idx=parseInt(jQuery(this).html());
			window.clearInterval(_t);
			slide(_idx);
			return false;
		}).mouseover(function(evt){
			jQuery(this).addClass("on");
		}).mouseout(function(evt){
			
			cycle();
		});
		
		cycle();
	};
	
	pub.Init=function(){
		//头部tab切换-调用sohu.abcdSlider插件
		p._$tabs=jQuery(".skMsg").abcdSlider({cssItem:'.team'});
		//头部tab切换-屏蔽a标签的导航
		p._$liSportsList=jQuery("#sportsList");
		p._$btnSportsList=p._$liSportsList.find("a").mouseover(p.clickTopNav);
		//焦点图
		p.initFocusSlide();
	};
	
	return pub;
}();



