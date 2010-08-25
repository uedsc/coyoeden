/**
 * JS interactive logic for editing fragments
 * ��Ƭ�༭��
 * @author levinhuang
 */
var chipEditor = function() {
    var p={},pub={};
	p.editors={};
	/* �¼����� */
	/**
	 * ɾ��Ԫ��
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
	 * �Ӵ�����
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
	 * ��ɫ
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
	 * ���Ԫ��
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
		//����������¼�(�հ���Ӧ��)
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
		//������������
		tpl1.eq(0).find("input").val(a0.html());
		tpl1.eq(1).find("input").val(a0.attr("href"));
		var data1={$obj:a0,$tpl:tpl1,t:0};
		//ͼ���¼�����
		p.bindIconEvts(tpl1,data1);
		
		evt.data.$tpl.eq(1).after(tpl1).after(tpl);
		return false;
	};
	/**
	 * ͼ���¼�����
	 * @param {Object} $tpl
	 * @param {Object} data
	 */
	p.bindIconEvts=function($tpl,data){
		//ɾ����ť
		$tpl.find(".btnDel").bind("click",data,p.onDelElm);
		//���ư�ť
		$tpl.find(".btnAdd").bind("click",data,p.onAddElm);
		//�Ӵ�
		$tpl.find(".bold").bind("click",data,p.onBold);
		//��ɫ
		$tpl.find(".color").bind("click",data,p.onColor);
		if(data.t==0){
			//���������
			$tpl.eq(0).find("input").keyup(function(evt){
				data.$obj.html(this.value);
			}).focus(p.focusSelect);
			//���������
			$tpl.eq(1).find("input").keyup(function(evt){
				data.$obj.attr("href",this.value);
			}).focus(p.focusSelect);
			//����ͼ��
			$tpl.eq(0).find('.icon').click(function(evt){
				$(this).next().toggle();
				return false;
			});
			//��Ƶǰ
			$tpl.eq(0).find(".videoL").bind("click",{$obj:data.$obj,before:true},p.onInsertVDIcon);
			//��Ƶ��
			$tpl.eq(0).find(".videoR").bind("click",{$obj:data.$obj,before:false},p.onInsertVDIcon);			
		}else{
			//ͼƬ��ַ�����
			$tpl.eq(0).find("input").change(function(evt){
				data.$img.attr("src",this.value);
			}).focus(p.focusSelect);
			//ALT�����
			$tpl.eq(1).find("input").change(function(evt){
				data.$img.attr("alt",this.value);
			}).focus(p.focusSelect);
			//���������
			$tpl.eq(2).find("input").change(function(evt){
				data.$obj.attr("href",this.value);
			}).focus(p.focusSelect);
		};

	};
	/**
	 * ��ȡ��Ƭ��ŵ��߼�
	 * @param {Object} $flag
	 */
	p.getFlagID=function($flag){
		var id=$flag.attr("id");
		if(id==""){
			id="flag"+StringUtils.RdStr(8);
			$flag.attr("id",id);
		};
		return id;	
	};
	/**
	 * ������ƵСͼƬ
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
	 * �༭��Ƭ��Ԫ��
	 * @param {Object} evt
	 */
	p.onEditFlagElem=function(evt){
		var $curElm=$(this),_t=0;
		
		//��Ԫ���Ƿ��ڱ༭��
		if($curElm.hasClass("ing")) return;
		
		var $curFlag=$curElm.parents(p._cssFlag);
		var flagID=p.getFlagID($curFlag);
		
		$curElm.addClass("ing");
		$curFlag.addClass("on");
		//���Ԫ������
		if($curElm.find("img").length>0){
			_t=1;//ͼƬ
		}else if($curElm.parent().is("li")){
			_t=0;//�б�
		}else{
			_t=2;//����
		};
		
		var $elmList=$curElm.parent().contents();
		
		var onShow=function(hash,dlg){
			if(dlg.curElm)
				dlg.curElm.removeClass("ing");
				
			dlg.CurElm=$curElm;
			dlg.CurFlag=$curFlag;
			//��һ��tab
			dlg.$Layout.find(".elmTpl").hide();
			var _$elmA=dlg.$Layout.find(".elmA").empty();
			var _$elmImg=dlg.$Layout.find(".elmImg").empty();
			switch(_t){
				case 0:
					$elmList.each(function(i,o){
						var $o=$(o);
						var tpl=null;
						if(o.nodeType!=1){
							//�ı�
							tpl=p._$rowTxtA.clone();
							_$elmA.append(tpl);
							var data={$obj:$o,$tpl:tpl};
							tpl.find("input").val($o.text()).bind("keyup",function(evt){
								var obj0=$(document.createTextNode(this.value));
								data.$obj.replaceWith(obj0);
								data.$obj=obj0;
							}).focus(p.focusSelect);
							//ɾ����ť
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
							//�ı�
							tpl=p._$rowTxtA.clone();
							_$elmImg.append(tpl);
							var data={$obj:$o,$tpl:tpl,t:_t};
							tpl.find("input").val($o.text()).bind("keyup",function(evt){
								var obj0=$(document.createTextNode(this.value));
								data.$obj.replaceWith(obj0);
								data.$obj=obj0;
							}).focus(p.focusSelect);
							//ɾ����ť
							tpl.find(".btnDel").bind("click",function(evt){
								data.$obj.remove();
								tpl.remove();
								return false;
							});								
						}else if($o.is("a")){
							var $img=$o.find("img");
							if($img.length>0){
								//ͼƬ����
								tpl=$([p._$rowImg.clone()[0],p._$rowALT.clone()[0],p._$rowLnkImg.clone()[0]]);
								tpl.eq(0).find("input").val($img.attr("src"));
								tpl.eq(1).find("input").val($img.attr("alt"));
								tpl.eq(2).find("input").val($o.attr("href"));
								_$elmImg.append(tpl);
								p.bindIconEvts(tpl,{$obj:$o,$tpl:tpl,t:_t,$img:$img});								
							}else{
								//��������
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
							//�ı�
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
							//������¼�����
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
			//�ڶ���tab
			dlg.$Layout.find('.txtVspC').val($curElm.parents(p._cssFlag).html());
			dlg.$Layout.find('.txtVspCS').textareaSearch({
				cssTextArea:dlg.$Layout.find('textarea'),
				cssBtn:dlg.$Layout.find('.btnVspCS')
			});			
		};//onShow
		var onHide=function(hash,dlg){
			dlg.CurElm.removeClass("ing");
			dlg.CurFlag.removeClass("on");
			p._$chipCover_t.bind("mouseenter",p.onFlagMEnter).trigger("mouseenter");
		};//onHide
		chipEditor.Show({afterShow:onShow,afterHide:onHide,flagID:flagID});
	};
	/**
	 * ����ͼԪ�ر༭
	 */
	p.onEditFlashElm=function(){
		//if(p._$chipCover_t.hasClass("on")) return;
		var flagID=p.getFlagID(p._$chipCover_t);
		var $curFlag=p._$chipCover_t;
		var onShow=function(hash,dlg){
			dlg.CurFlag=$curFlag;
			//hide the first tab
			dlg.$CT.tabs("select",1);
			dlg.$TabM.eq(0).hide();
			//show the flash tip
			dlg.$TabC.eq(1).find(".row").hide().filter(".flashTip").show();
			//content of tab no2
			dlg.$Layout.find('.txtVspC').val(dlg.CurFlag.html());
		};
		var onHide=function(hash,dlg){
			//p._$chipCover_t.removeClass("on");
		};
		chipEditor.Show({afterShow:onShow,afterHide:onHide,flagID:flagID});
	};
	/**
	 * ��ƬԪ������
	 * @param {Object} evt
	 */
	p.onSortFlagElm=function(evt){
		var $i=$(this);
		var dlg=evt.data.dlg;
		if($i.hasClass("sort")) return;
		if(!dlg.SortElm){
			dlg.SortElm=$i.addClass("sort");
			return;
		};
		//��������
		var temp=dlg.SortElm.html();
		dlg.SortElm.html($i.html());
		$i.html(temp);
		
		//�������β���
		dlg.SortElm.removeClass("sort");
		dlg.SortElm=null;
		dlg.CurElm.removeClass('ing');
		//����CurFlag����
		dlg.CurFlag=$i.parents(p._cssFlag);
		//���´���textarea
		dlg.$Layout.find("textarea").val(dlg.CurFlag.html());
	};
	p.onFlagMEnter=function(evt){	
		var $i=$(this);
		if($i.hasClass("on")) return;/* �ڱ༭ĳ��Ԫ��ʱ��Ƭ�����on */
		var d=p.getDim($i);
		p._$chipCover.css({
			opacity:0.5,
			top:d.y-1,
			left:d.x-1,
			height:d.h+d.pt+d.pb,
			width:d.w+d.pl+d.pr,
			display:'block'
		}).one("mouseleave",function(evt){
			p._$chipCover.hide();
			//p._$chipCover_t.removeClass("on");
			//$i.bind("mouseenter",p.onFlagMEnter);
		});
		
		
		p._$chipCover_t=$i;
		//p._$chipCover_t.addClass("on");
	};
	/**
	 * ����a��ǩ��Ĭ�ϵ�����Ϊ
	 * @param {Object} evt
	 */
	p.stopNav=function(evt){
		evt.preventDefault();
		return true;		
	};
	p.focusSelect=function(evt){
		$(this).select();
	};
	p.getDim=function($i){
		var of=$i.offset();
		var pt=parseInt($i.css("padding-top"));
		pt=isNaN(pt)?0:pt;
		var pb=parseInt($i.css("padding-bottom"));
		pb=isNaN(pb)?0:pb;
		var pl=parseInt($i.css("padding-left"));
		pl=isNaN(pl)?0:pl;
		var pr=parseInt($i.css("padding-right"));
		pr=isNaN(pr)?0:pr;
		
		var dim={
			y:of.top,
			x:of.left,
			h:$i.height(),
			w:$i.width(),
			pt:pt,
			pb:pb,
			pl:pl,
			pr:pr
		};
		
		return dim;
	};
	
	/**
	 * ��Ƭ�¼�ע��
	 */
	p.initFlagEvts=function(){
		$(p._cssFlag+" a,.jqmCT a").bind('click.noNav',p.stopNav);
		$(p._cssFlag+" a").bind("click.edit",p.onEditFlagElem);
		$(p._cssFlag).bind("mouseenter",p.onFlagMEnter).mouseleave(function(evt){
			p._$chipCover_t.unbind("mouseenter").bind("mouseenter",p.onFlagMEnter);
		});
		
		p._$chipCover.click(function(evt){
			if(p._$chipCover_t.is(p._cssFlash)){
				//���뽹��ͼ�༭����
				p.onEditFlashElm();	
				return;
			};
			
			p._$chipCover.trigger("mouseleave");
			p._$chipCover_t.unbind("mouseenter");
			/*
			window.setTimeout(function(){
				p._$chipCover_t.bind("mouseenter",p.onFlagMEnter);
			},500);
			*/
			
		});
	};
    //private area
    p.initVar = function(opts) { 
		p._cssFlag=opts.cssFlag||".chip";
		p._cssFlash=opts.cssFlash||'.flash';
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
		p._$chipCover=$("#chipCover");
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
		var dlg=null;
		opts.isNew=false;
		if(!(dlg=p.editors[opts.flagID])){
			dlg=new chipEditor.Dialog({dlgModel:p._dlgModel,$body:p._$body});
			p.editors[opts.flagID]=dlg;
			opts.isNew=true;
		};
		dlg.Show(opts);
	};
	pub.OnEditFlagElm=p.onEditFlagElem;
	pub.OnSortFlagElm=p.onSortFlagElm;
    return pub;
} ();
/**
 * ��Ƭ�༭��������
 * @param {Object} opts
 */
chipEditor.Dialog=function(opts){
	opts=$.extend({},{
		tabIDTpl:'wVsp_tab',
		lblSort:'����״̬',
		lblSort1:'��������',
		lblTab0:'<strong class="alert">���޸�״̬</strong><em>��ѡ����</em>'
		},opts||{});
	var _this=this;
	this.Sorting=false;
	this._opts=opts;
	this.CurElm=null;//��ǰԪ��
	this.CurFlag=null;//��ǰ��Ƭ
	this.SortElm=null;//��ǰ����Ԫ��
	//setup layout
	this.$Layout=opts.dlgModel.clone().appendTo(opts.$body);
	this.$btnCode=this.$Layout.find(".globalRes,.external");
	//�¼�����
	
	//����tab�˵�
	var tabID="tab"+StringUtils.RdStr(8);
	this.$TabM=this.$Layout.find(".tabM").each(function(i,o){
		var $o=$(o);
		$o.attr("href",$o.attr("href").replace(opts.tabIDTpl,tabID));
	});
	this.$TabC=this.$Layout.find(".tabC").each(function(i,o){
		var $o=$(o);
		$o.attr("id",$o.attr("id").replace(opts.tabIDTpl,tabID));
	});
	this.$Layout.find('.btnSort').click(function(evt){
		_this.ToggleSorting($(this));return false;
	});
	this.$CT=this.$Layout.find(".jqmCT").tabs({
		select:function(evt,ui){
			//��ť������
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
	//margin-left
	this.$Layout.css("margin-left",-(this.$Layout.width()/2));
}; 
chipEditor.Dialog.prototype.Show=function(opts){
	$.extend(this.jqmOpts,opts||{});
	if(!opts.isNew)
		this.Hide(opts);
		
	this.$Layout.jqm(this.jqmOpts).jqmShow();
};
chipEditor.Dialog.prototype.Hide=function(opts){
	$.extend(this.jqmOpts,opts||{});
	this.$Layout.jqm(this.jqmOpts).jqmHide();
	return this;
};
chipEditor.Dialog.prototype.ToggleSorting=function($i){
	if(this.Sorting){
		$i.removeClass("btnSort1").find("span").html(this._opts.lblSort);
		this.$CT.tabs("option","disabled",[]);
		this.$CT.tabs("option","selected",0);
		
		this.Sorting=false;
		//��click.edit�¼�
		this.CurFlag.find("a").unbind("click.sort").bind("click.edit",chipEditor.OnEditFlagElm);
		
	}else{
		$i.prev().trigger("click");
		$i.addClass("btnSort1").find("span").html(this._opts.lblSort1);
		this.$CT.tabs("option","disabled",[0,1,2]);
		this.Sorting=true;
		//�Ƴ���Ƭa��ǩ��click.edit�¼�����
		this.CurFlag.find("a").unbind("click.edit").bind("click.sort",{dlg:this},chipEditor.OnSortFlagElm);
		//����"���ӻ��޸�"��"�����޸�"(����Ԫ�ط����˱仯)
		this.$TabC.find(".elmA,.elmImg").empty().html(this._opts.lblTab0);
	};
};
