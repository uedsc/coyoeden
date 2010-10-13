/**
 * @author levinhuang
 * @搜狐高清-电视剧首页焦点图
 * @version 2010.10.12
 * @dependency jquery.SOHUFocusImageCore.js
 */
/* 焦点图8 */
$.fn.focusImg.Register("fi08", {
    init: function (fi) {
		fi.ptStepY_=fi.ptStepY_||50;
		fi.ptStepY=fi.ptStepY||180;
        //dom references and cache
        fi._$tabC = fi.$d.find(".fi_tab");
		fi._$transparentOvl = fi.$d.find(".fi_ovl").css("opacity", fi._cfg.opacity || 0.5);
        //tab列表
		fi._$list = fi._$tabC.find("ul");
        fi._$tabs = fi._$list.find("li");
        fi._tabNum = fi._$tabs.length;		
		//右侧文字列表
		fi._$list1=fi.$d.find(".fi_note ul");
		//设置文字列表li的索引
		fi._$list1.find("li").each(function(i,o){
			$(o).data("id",i);
		});
		//tab按钮
        fi._$btnL = fi._$tabC.find(".fi_left a");
        fi._$btnR = fi._$tabC.find(".fi_right a");
		//大图
        fi._$img = fi.$d.find(".fi_player");
        //水平方向步长
        fi.ptStepX = fi._$tabs.eq(0).width();
        //设置tab的总长度
        fi._$list.css("width", fi.ptStepX * fi._tabNum);
        //根据tab个数，设定按钮是否可点击
        fi._cfg.displayNum = fi._cfg.displayNum || 4;
        if (fi._tabNum <= fi._cfg.displayNum) {
            fi._$btnR.addClass("fi_btn_off");
        } else {
            fi._$btnL.addClass("fi_btn_on");
        };
        //所有tab图片半透明
        fi._$tabC.find("img").css("opacity", fi._cfg.opacity || 0.5);
    },
    initEvts: function (fi, gogo) {
        var _this = this;
        //绑定各种元素的事件
        fi._$tabC.find("a").click(function (e) { /*失焦以便移除虚线*/this.blur(); e.preventDefault(); return true; });
        //左按钮
        fi._$btnL.mousedown(function () {
            var s = fi._$tabs.index(fi._$tabs.filter(".now")[0]);
            if ((s==fi._tabNum-1)||fi._$list.is(":animated") || $(this).hasClass("fi_btn_off")) return;
            clearInterval(fi.autoPlay);
            clearTimeout(fi.autoPlay1);

			s=(s+1==fi._tabNum)?0:(s+1);
			_this.alt(fi,s,function(){
				fi.autoPlay1=window.setTimeout(function(){
					gogo(s-1);
				},fi.speed);				
			});	

        });
        //右按钮
        fi._$btnR.mousedown(function () {
            var s = fi._$tabs.index(fi._$tabs.filter(".now")[0]);
            if ((s ==0) || fi._$list.is(":animated") || $(this).hasClass("fi_btn_off")) return;
            clearInterval(fi.autoPlay);
            clearTimeout(fi.autoPlay1);
			
			s=(s-1<0)?0:(s-1);
			_this.alt(fi,s,function(){
				fi.autoPlay1=window.setTimeout(function(){
					gogo(s+1);
				},fi.speed);				
			});
        });

        fi.$d.find(".fi_player_c").click(function (e) {
            window.open(fi._curLink);
        });
    },
	/**
	 * 右侧文字列表的逐步滚动
	 */
	roll:function(fi,i){
		var i1=0;
		var liNow=$.grep(fi._$list1.find("li"),function(o,j){
			if($(o).data("id")==i){
				i1=j;
				return true;
			};
			return false;
		});		
		if(i1==0) return;
		var step=fi.ptStepY+(i1-1)*fi.ptStepY_;
		fi._$list1.stop(true,true).animate({top:-step},200,function(){
			fi._$list1
				.append($(liNow).prevAll().removeClass("fi_note_on").stop(true,true).height(fi.ptStepY_))
				.css("top",0)
				.find("li:first").stop(true,true)
				.addClass("fi_note_on")
				.css("height",fi.ptStepY_)
				.animate({height:fi.ptStepY},300);
		});		
	},
	tabOffset:function(fi,i){
		var r={visible:false};
		//计算当前tab是否在显示区域
        r.val = (i + 1) * fi.ptStepX + fi._$list.position().left;
        //当前tab在显示区
        if (r.val > 0 && r.val <= fi._cfg.displayNum * fi.ptStepX){
			r.visible=true;
		};
		return r;
	},
	/**
	 * 按钮状态管理
	 * @param {Object} fi
	 * @param {Object} i
	 */
	btnTrack:function(fi,i){
        //左右按钮控制
        if (i == 0) {
            //右边暗左边暗
            fi._$btnR.removeClass("fi_btn_on").addClass("fi_btn_off");
            fi._$btnL.removeClass("fi_btn_off").addClass("fi_btn_on");
        } else if (i == (fi._tabNum - 1)) {
            //左边暗右边亮
            fi._$btnL.removeClass("fi_btn_on").addClass("fi_btn_off");
            fi._$btnR.removeClass("fi_btn_off").addClass("fi_btn_on");
        } else {
            //都亮
            fi._$btnL.removeClass("fi_btn_off").addClass("fi_btn_on");
            fi._$btnR.removeClass("fi_btn_off").addClass("fi_btn_on");
        };		
	},
	/**
	 * 选中指定tab
	 * @param {Object} i
	 */
	setTab:function(fi,i,cbk){
		if (fi._$curImg) {
            fi._$curImg.stop(true, true).animate({ opacity: fi._cfg.opacity || 0.5 }, 200);
        };
        fi._$curTab = fi._$tabs.removeClass("now").eq(i).addClass("now");
        fi._$curImg = fi._$curTab.find("img").stop(true, true).animate({ opacity: 1 }, 200,cbk);
		/* 右侧文字列表滚动交互 */
		this.roll(fi,i);		
        fi._curLink = fi._$curTab.find("a").attr("href");
        //回填数据
        fi._$img.attr("src", fi._$curImg.attr("src")).css("opacity", 0).stop(true, true).animate({ opacity: 1 }, 200);		
		
	},
    alt: function (fi, i, cbk) {
		//选中指定tab
		this.setTab(fi,i,cbk);
		
        /* 图片tab列表交互 */
        //tab总数小于等于显示数
        if (fi._tabNum <= fi._cfg.displayNum) return false;
		//按钮状态
		this.btnTrack(fi,i);
        //计算当前tab是否在显示区域
        var offset =this.tabOffset(fi,i);
        //当前tab在显示区
        if (offset.visible) return false;
        //当前tab不在显示区，需要移动tab列表
        fi._$list.stop(true, true).animate({ left: (offset.val <= 0 ? "+=" : "-=") + fi.ptStepX }, 200);
        //返回false，不执行jquery.SOHUFocusImageCore.js中定义的通用的交互流程
        return false;
    }
});