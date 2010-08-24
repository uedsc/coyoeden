/**
 * JS interactive logic for editing fragments
 * 碎片编辑器
 * @author levinhuang
 */
var chipEditor = function() {
    var p={},pub={};
	/* 事件处理 */
	/**
	 * 删除元素
	 * @param {Object} evt
	 */
	p.onDelElm=function(evt){
		if(evt.data.$obj){
			evt.data.$obj.remove();
			evt.data.$tpl.remove();
		};
		return false;
	};
	/**
	 * 加粗字体
	 * @param {Object} evt
	 */
	p.onBold=function(evt){
		if(!evt.data.$obj) return false;
		if(evt.data.$obj.hasClass(p._cssBold)){
			evt.data.$obj.removeClass(p._cssBold).css("font-weight","normal");
			evt.data.$tpl.eq(0).find("input").removeClass(p._cssBold);
		}else{
			evt.data.$obj.addClass(p._cssBold).css("font-weight","bold");
			evt.data.$tpl.eq(0).find("input").addClass(p._cssBold);
		};
		return false;
	};
	/**
	 * 着色
	 * @param {Object} evt
	 */
	p.onColor=function(evt){
		if(!evt.data.$obj) return false;
		if(evt.data.$obj.hasClass(p._cssColor)){
			evt.data.$obj.removeClass(p._cssColor).css("color",p._rawColor);
			evt.data.$tpl.eq(0).find("input").removeClass(p._cssColor);
		}else{
			if(!p._rawColor)
				p._rawColor=evt.data.$obj.css("color");
			
			evt.data.$obj.addClass(p._cssColor).css("color","red");
			evt.data.$tpl.eq(0).find("input").addClass(p._cssColor);			
		};
		return false;
	};
	/**
	 * 添加元素
	 * @param {Object} evt
	 */
	p.onAddElm=function(evt){
		if(!evt.data.$obj) return false;
		var txt0=$(document.createTextNode(" "));
		var a0=$(p._aTplStr);
		evt.data.$obj.after(a0).after(txt0);
		a0.bind('click.noNav',p.stopNav);
		
		var tpl=p._$rowTxtA.clone();
		var data={$obj:txt0,$tpl:tpl};
		//文字输入框事件(闭包的应用)
		tpl.find("input").val(" ").bind("keyup",function(evt){
			var obj0=$(document.createTextNode(this.value));
			data.$obj.replaceWith(obj0);
			data.$obj=obj0;
		}).focus(p.focusSelect);
		tpl.find(".btnDel").bind("click",function(evt){
			data.$obj.remove();
			tpl.remove();
		});
		var tpl1=$([p._$rowTitleA.clone()[0],p._$rowLnkA.clone()[0]]);
		//输入框回填内容
		tpl1.eq(0).find("input").val(a0.html());
		tpl1.eq(1).find("input").val(a0.attr("href"));
		var data1={$obj:a0,$tpl:tpl1,t:0};
		//图标事件处理
		p.bindIconEvts(tpl1,data1);
		
		evt.data.$tpl.eq(1).after(tpl1).after(tpl);
		return false;
	};
	/**
	 * 图标事件处理
	 * @param {Object} $tpl
	 * @param {Object} data
	 */
	p.bindIconEvts=function($tpl,data){
		//删除按钮
		$tpl.find(".btnDel").bind("click",data,p.onDelElm);
		//复制按钮
		$tpl.find(".btnAdd").bind("click",data,p.onAddElm);
		//加粗
		$tpl.find(".bold").bind("click",data,p.onBold);
		//颜色
		$tpl.find(".color").bind("click",data,p.onColor);
		if(data.t==0){
			//标题输入框
			$tpl.eq(0).find("input").keyup(function(evt){
				data.$obj.html(this.value);
			}).focus(p.focusSelect);
			//链接输入框
			$tpl.eq(1).find("input").keyup(function(evt){
				data.$obj.attr("href",this.value);
			}).focus(p.focusSelect);
			//其他图标
			$tpl.eq(0).find('.icon').click(function(evt){
				$(this).next().toggle();
				return false;
			});
			//视频前
			$tpl.eq(0).find(".videoL").bind("click",{$obj:data.$obj,before:true},p.onInsertVDIcon);
			//视频后
			$tpl.eq(0).find(".videoR").bind("click",{$obj:data.$obj,before:false},p.onInsertVDIcon);			
		}else{
			//图片地址输入框
			$tpl.eq(0).find("input").change(function(evt){
				data.$img.attr("src",this.value);
			}).focus(p.focusSelect);
			//ALT输入框
			$tpl.eq(1).find("input").change(function(evt){
				data.$img.attr("alt",this.value);
			}).focus(p.focusSelect);
			//链接输入框
			$tpl.eq(2).find("input").change(function(evt){
				data.$obj.attr("href",this.value);
			}).focus(p.focusSelect);
		};

	};
	/**
	 * 插入视频小图片
	 * @param {Object} evt
	 */
	p.onInsertVDIcon=function(evt){
		var a=$(p._vdTplStr).bind("click.noNav",p.stopNav).bind('click.edit',p.onEditFlagElem);
		if(evt.data.before){
			evt.data.$obj.before(a);
		}else{
			evt.data.$obj.after(a);
		};
		return false;
	};
	/**
	 * 编辑碎片的元素
	 * @param {Object} evt
	 */
	p.onEditFlagElem=function(evt){
		var $curElm=$(this),_t=0;
		//已处于编辑状态
		if($curElm.hasClass("ing")) return false;
		
		$curElm.addClass("ing");
		//检查元素类型
		if($curElm.find("img").length>0){
			_t=1;//图片
		}else if($curElm.parent().is("li")){
			_t=0;//列表
		}else{
			_t=2;//段落
		};
		
		var $elmList=$curElm.parent().contents();
		
		var onShow=function(hash,dlg){
			//第一个tab
			dlg.$Layout.find(".elmTpl").hide();
			var _$elmA=dlg.$Layout.find(".elmA");
			var _$elmImg=dlg.$Layout.find(".elmImg");
			switch(_t){
				case 0:
					$elmList.each(function(i,o){
						var $o=$(o);
						var tpl=null;
						if(o.nodeType!=1){
							//文本
							tpl=p._$rowTxtA.clone();
							_$elmA.append(tpl);
							var data={$obj:$o,$tpl:tpl};
							tpl.find("input").val($o.text()).bind("keyup",function(evt){
								var obj0=$(document.createTextNode(this.value));
								data.$obj.replaceWith(obj0);
								data.$obj=obj0;
							}).focus(p.focusSelect);
							//删除按钮
							tpl.find(".btnDel").bind("click",function(evt){
								data.$obj.remove();
								tpl.remove();
							});
						}else if($o.is("a")){
							tpl=$([p._$rowTitleA.clone()[0],p._$rowLnkA.clone()[0]]);
							tpl.eq(0).find("input").val($o.html());//attr("title")
							tpl.eq(1).find("input").val($o.attr("href"));
							_$elmA.append(tpl);
							p.bindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:_t});
						};
	
					});
					_$elmA.show();
				break;
				case 1:
					$elmList.each(function(i,o){
						var $o=$(o),tpl=null;
						if(o.nodeType!=1){
							//文本
							tpl=p._$rowTxtA.clone();
							_$elmImg.append(tpl);
							var data={$obj:$o,$tpl:tpl,t:_t};
							tpl.find("input").val($o.text()).bind("keyup",function(evt){
								var obj0=$(document.createTextNode(this.value));
								data.$obj.replaceWith(obj0);
								data.$obj=obj0;
							}).focus(p.focusSelect);
							//删除按钮
							tpl.find(".btnDel").bind("click",function(evt){
								data.$obj.remove();
								tpl.remove();
								return false;
							});								
						}else if($o.is("a")){
							var $img=$o.find("img");
							if($img.length>0){
								//图片链接
								tpl=$([p._$rowImg.clone()[0],p._$rowALT.clone()[0],p._$rowLnkImg.clone()[0]]);
								tpl.eq(0).find("input").val($img.attr("src"));
								tpl.eq(1).find("input").val($img.attr("alt"));
								tpl.eq(2).find("input").val($o.attr("href"));
								_$elmImg.append(tpl);
								p.bindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:_t,$img:$img});								
							}else{
								//文字链接
								tpl=$([p._$rowTitleA.clone()[0],p._$rowLnkA.clone()[0]]);
								tpl.eq(0).find("input").val($o.html());
								tpl.eq(1).find("input").val($o.attr("href"));
								_$elmImg.append(tpl);
								p.bindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:0});								
							};
						};
					});//each
					_$elmImg.show();
				break;
				case 2:
					$elmList.each(function(i,o){
						var $o=$(o),tpl=null;
						if(o.nodeType!=1){
							//文本
							tpl=p._$rowTxtA0.clone();
							_$elmA.append(tpl);
							var data={$obj:$o,$tpl:tpl,t:_t};
							tpl.find("input").val($o.text()).bind("keyup",function(evt){
								var obj0=$(document.createTextNode(this.value));
								data.$obj.replaceWith(obj0);
								data.$obj=obj0;
							}).focus(p.focusSelect);						
						}else if($o.is("a")){
							tpl=$([p._$rowTitleA0.clone()[0],p._$rowLnkA.clone()[0]]);
							_$elmA.append(tpl);
							//输入框事件处理
							tpl.eq(0).find("input").val($o.html()).keyup(function(evt){
								$o.html(this.value);
							}).focus(p.focusSelect);
							tpl.eq(1).find("input").val($o.attr("href")).keyup(function(evt){
								$o.attr("href",this.value);
							}).focus(p.focusSelect);
							
							
						};
					});			
					_$elmA.show();
				break;
			};//switch
			//第二个tab
			dlg.$Layout.find('.txtVspC').val($curElm.parents(p._cssFlag).html());
			dlg.$Layout.find('.txtVspCS').textareaSearch({
				cssTextArea:dlg.$Layout.find('textarea'),
				cssBtn:dlg.$Layout.find('.btnVspCS')
			});			
		};//onShow
		var onHide=function(hash,dlg){
			$curElm.removeClass("ing");
		};//onHide
		chipEditor.Show({afterShow:onShow,afterHide:onHide});
	};
	/**
	 * 禁用a标签的默认导航行为
	 * @param {Object} evt
	 */
	p.stopNav=function(evt){
		evt.preventDefault();
		return true;		
	};
	p.focusSelect=function(evt){
		$(this).select();
	};
	/**
	 * 碎片事件注册
	 */
	p.initFlagEvts=function(){
		$(p._cssFlag+" a,.jqmCT a").bind('click.noNav',p.stopNav);
		$(p._cssFlag+" a").bind("click.edit",p.onEditFlagElem);
	};
    //private area
    p.initVar = function(opts) { 
		p._cssFlag=opts.cssFlag||".flag";
		p._cssBold=opts.cssBold||'bb';
		p._cssColor=opts.cssColor||'cc';
		p._$rowTxtA=$("#elmATpl .rowTxt");
		p._$rowTitleA=$("#elmATpl .rowTitle");
		p._$rowTxtA0=$("#elmATpl .rowTxt0");
		p._$rowTitleA0=$("#elmATpl .rowTitle0");		
		p._$rowLnkA=$("#elmATpl .rowLnk");
		p._$rowImg=$("#elmImgTpl .rowImg");
		p._$rowALT=$("#elmImgTpl .rowALT");
		p._$rowLnkImg=$("#elmImgTpl .rowLnk");
		p._aTplStr=opts.aTplStr||'<a href="http://sohu.com" title="">sohu</a>';
		p._vdTplStr=opts.vdTplStr||'<a href="http://sohu.com" title=""><img src="http://images.sohu.com/uiue/vd.gif" alt="Video"/></a>';
		p._dlgModel=$(".chipEdt");
		p._$body=$("body");
	};
    p.onLoaded = function() { 

	};
    p.initEvents = function(opts) {
        $(document).ready(p.onLoaded);
		$(".acts a,button").button();
		p.initFlagEvts();
    };
    //public area
    pub.Init = function(opts) {
        p.initVar(opts);
        p.initEvents(opts);
    };
	pub.Show=function(opts){
		var dlg=new chipEditor.Dialog({dlgModel:p._dlgModel,$body:p._$body});
		dlg.Show(opts);
	};
    return pub;
} ();
/**
 * 碎片编辑器弹框类
 * @param {Object} opts
 */
chipEditor.Dialog=function(opts){
	opts=$.extend({},{tabIDTpl:'wVsp_tab'},opts||{});
	var _this=this;
	//setup layout
	this.$Layout=opts.dlgModel.clone().appendTo(opts.$body);
	this.$btnCode=this.$Layout.find(".globalRes,.external");
	//事件处理
	
	//内容tab菜单
	var tabID="tab"+StringUtils.RdStr(8);
	this.$Layout.find(".tabM").each(function(i,o){
		var $o=$(o);
		$o.attr("href",$o.attr("href").replace(opts.tabIDTpl,tabID));
	});
	this.$Layout.find(".tabC").each(function(i,o){
		var $o=$(o);
		$o.attr("id",$o.attr("id").replace(opts.tabIDTpl,tabID));
	});
	this.$Layout.find(".jqmCT").tabs({
		select:function(evt,ui){
			if(ui.index==1){
				_this.$btnCode.removeClass("hide");
			}else{
				_this.$btnCode.addClass("hide");
			};
		}
	});
	//jqm options
	this.jqmOpts={trigger:false,modal:false,overlay:false,autoFocus:false,beforeShow:null,afterShow:null,beforeHide:null,afterHide:null};
	this.jqmOpts.onShow=function(hash){
		var doShow=true;
		if(_this.jqmOpts.beforeShow){
			doShow=_this.jqmOpts.beforeShow(hash,_this);
		};
		if(doShow){
			hash.w.show();
			_this.jqmHash=hash;
			if(_this.jqmOpts.afterShow){
				_this.jqmOpts.afterShow(hash,_this);
			};
		};
	};
	this.jqmOpts.onHide=function(hash){
		var doHide=true;
		if(_this.jqmOpts.beforeHide){
			doHide=_this.jqmOpts.beforeHide(hash,_this);				
		};
		if(doHide){
			hash.w.hide();				
			if(_this.jqmOpts.modal){hash.o.remove();}; 
			//afterHide callback
			if(_this.jqmOpts.afterHide)
			{
				_this.jqmOpts.afterHide(hash,_this);
			};
		};	
	};
	
	//draggable
	this.$Layout.draggable({handle:".hd",containment:'window'});
}; 
chipEditor.Dialog.prototype.Show=function(opts){
	$.extend(this.jqmOpts,opts||{});
	this.$Layout.jqm(this.jqmOpts).jqmShow();
};
chipEditor.Dialog.prototype.Hide=function(opts){
	$.extend(this.jqmOpts,opts||{});
	this.$Layout.jqm(this.jqmOpts).jqmHide();
};