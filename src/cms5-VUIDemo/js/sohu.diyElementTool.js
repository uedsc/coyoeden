/**
 * ����Ƶ��ɱ༭Ԫ��ʱ����Ԫ�ػ����һ�����Ԫ�ؽ��С���������ɾ���������ơ��������ơ��Ĺ�����
 * sohu.diyElementTool.js��װ�ù������Ľ����߼�
 * @author levinhuang
 */
sohu.diyElementTool=function(){
	var p={},pub={};
	
	p.onShow=function(evt){
		if(!(p.elm=evt.elm)) return;
		var pos=p.elm.$Layout.offset();
		var css={
			top:pos.top+'px',
			left:pos.left+p.elm.$Layout.width()+'px',
		};
		p.$Layout.css(css).show();
	};
	p.onHide=function(evt){
		p.$Layout.hide();
	};
	p.onAdd=function(evt){
		if(!p.elm) return;
		var $dom=p.elm.$Layout.clone(false).attr("id","").removeClass(p._opts.clElmOn);
		var $if=p.elm.$Layout.next("iframe");
		if($if.length>0){
			$if.after($dom);
		}else{
			p.elm.$Layout.after($dom);
		}	
		//apply diyElement features
		new sohu.diyElement({ct:p.elm.CT,$dom:$dom});
		p.elm.$Layout.trigger("mouseleave");
		p.elm=null;
	};
	p.onDel=function(evt){
		if(!p.elm) return;
		if(p.elm.$Layout.parent().find("."+p._opts.clElmCopyable).length==1){
			alert("����ɾ�����һ��ģ��Ԫ��");
			return;
		}
		//if(!window.confirm("ȷ��ɾ��ô?")) return;
		p.elm.$Layout.next("iframe").remove();
		p.elm.$Layout.remove();
		p.elm=null;
		p.onHide();
	};
	p.onMove=function(evt){
		if(!p.elm) return;
		var $obj=evt.data.up?p.elm.$Layout.prevAll("."+p._opts.clElmCopyable):p.elm.$Layout.nextAll("."+p._opts.clElmCopyable);
		if($obj.length==0) return;
		$obj=$obj.eq(0);
		var $iframe0=$obj.next("iframe");
		var $iframe1=p.elm.$Layout.next("iframe");
		if(evt.data.up){
			$obj.before(p.elm.$Layout);
		}else{
			if($iframe0.length>0)
				$iframe0.after(p.elm.$Layout);
			else
				$obj.after(p.elm.$Layout);
		};
		if($iframe1.length>0)
			p.elm.$Layout.after($iframe1);
		
		p.elm.$Layout.trigger("mouseleave");
		p.elm=null;
	};
	/**
	 * ��ʼ������diyConsole��Init�����е���
	 */
	pub.Init=function(opts){
		if(!(p.$Layout=sohu.diyConsole.$ElmTool)) return;
		p._opts=$.extend(opts,{clElmCopyable:"elmc",clElmOn:"elmOn"});
		p._evts={evtShow:1,evtHide:1,evtAdd:1,evtDel:1,evtMoveUp:1,evtMoveDwn:1};
		//�¼�ע��
		p.$Layout.bind("evtShow",p.onShow);
		p.$Layout.bind("evtHide",p.onHide);
		p.$Layout.bind("evtAdd",p.onAdd);
		p.$Layout.bind("evtDel",p.onDel);
		p.$Layout.bind("evtMoveUp",{up:true},p.onMove);
		p.$Layout.bind("evtMoveDwn",{up:false},p.onMove);
		p.$Layout.bind("mouseenter",function(evt){
			if(!p.elm) return;
			p.elm.$Layout.trigger("mouseenter");	
		});
		p.$Layout.bind("mouseleave",function(evt){
			if(!p.elm) return;
			p.elm.$Layout.trigger("mouseleave");	
		});
		//��ť�¼�ע��
		p.$Layout.find("a").click(function(evt){return false;});
		//����
		p.$Layout.find(".add").click(function(evt){	
			p.$Layout.trigger("evtAdd");
		});
		//ɾ��
		p.$Layout.find(".del").click(function(evt){
			p.$Layout.trigger("evtDel");
		});
		//����
		p.$Layout.find(".up").click(function(evt){
			p.$Layout.trigger("evtMoveUp");
		});
		//����
		p.$Layout.find(".dwn").click(function(evt){
			p.$Layout.trigger("evtMoveDwn");
		});
	};
	/**
	 * trigger a event using a jquery event object
	 * @param {Object} evtObj
	 */
	pub.Trigger=function(evtObj){
		if(!p._evts[evtObj.type]) return;
		p.$Layout.trigger(evtObj);
	};
	return pub;
}();
