/**
 * 编辑菜单类-为编辑对话框的图标绑定事件
 * @author levinhuang
 * @dependency sohu.diyConsole、sohu.diyDialog
 */
sohu.diyMenuBar=function(opts){
	var p={};
	p._opts=$.extend(opts,{clElmCopyable:"elmc",clElmOn:"elmOn"});
	//文本图片命令
	p.txtCmd={};
	p.txtCmd.fontCbk=function(iframeEditor){sohu.diyConsole.CurElm.$Layout.css(iframeEditor.iCurCss);};
	p.txtCmd.Bold=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Bold",null);};
	p.txtCmd.Italic=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Italic",null);};
	p.txtCmd.Underline=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Underline",null);};
	p.txtCmd.SizeUp=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("increasefontsize",null);};
	p.txtCmd.SizeDown=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("decreasefontsize",null);};
	p.txtCmd.AlignLeft=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyleft",null,p.txtCmd.fontCbk);};
	p.txtCmd.AlignCenter=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifycenter",null,p.txtCmd.fontCbk);};
	p.txtCmd.AlignRight=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyright",null,p.txtCmd.fontCbk);};
	p.txtCmd.AlignFull=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyfull",null,p.txtCmd.fontCbk);};
	p.txtCmd.AddUL=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("insertunorderedlist",null);};
	p.txtCmd.AddOL=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("insertorderedlist",null);};
	p.txtCmd.Undo=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("undo",null);};
	p.txtCmd.Redo=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("redo",null);};
	p.txtCmd.AddVideoIcon=function(evt){
		sohu.diyConsole.CurElm.$Layout.append(sohu.diyMenuBar.Tpl.video);
		sohu.diyConsole.CurElm.i$frame[0].i$Body().append(sohu.diyMenuBar.Tpl.video);
	};
	p.txtCmd.SetColor=function(evt){
		var _this=$(this);
		sohu.diyDialog.showColorPicker({
			onSubmit:function(c){
				sohu.diyConsole.CurElm.i$frame[0].iDoCommand("foreColor",c,null,function($iframe){
				sohu.diyConsole.DocSelection.selectAndRelease();
				_this.find("img").css("border-color",c);
			});
			},
			onChange:function(c){
				sohu.diyConsole.DocSelection.select();
				_this.find("img").css("border-color",c);
			}
		});
	};
	//元素命令
	p.elmCmd={};
	p.elmCmd.AddCopy=function(evt){
		if(!sohu.diyConsole.CurElm) return;
		var $dom=sohu.diyConsole.CurElm.$Layout.clone(false).attr("id","").removeClass(p._opts.clElmOn).show().css("visibility","visible");
		var $if=sohu.diyConsole.CurElm.$Layout.next("iframe");
		if($if.length>0){
			$if.after($dom);
		}else{
			sohu.diyConsole.CurElm.$Layout.after($dom);
		}	
		//apply diyElement features
		new sohu.diyElement({ct:sohu.diyConsole.CurElm.CT,$dom:$dom});
	};
	p.elmCmd.Del=function(evt){
		if(!sohu.diyConsole.CurElm) return;
		var tempElm=sohu.diyConsole.CurElm;
		//关闭编辑对话框
		sohu.diyDialog.Hide();
		//删除元素
		tempElm.$Layout.next("iframe").remove();
		tempElm.$Layout.remove();
	};
	p.elmCmd.Move=function(evt){
		p.elm=sohu.diyConsole.CurElm;
		if(!p.elm) return;
		//移动
		var $obj=evt.data.up?p.elm.$Layout.prevAll("."+p._opts.clElmCopyable):p.elm.$Layout.nextAll("."+p._opts.clElmCopyable);
		if($obj.length==0) return;
		$obj=$obj.eq(0);
		//var $iframe0=$obj.next("iframe");
		//var $iframe1=p.elm.$Layout.next("iframe");
		if(evt.data.up){
			$obj.before(p.elm.$Layout);
		}else{
			/*
			if($iframe0.length>0)
				$iframe0.after(p.elm.$Layout);
			else
				$obj.after(p.elm.$Layout);
			*/
			$obj.after(p.elm.$Layout);
		};
		/*
		if($iframe1.length>0)
			p.elm.$Layout.after($iframe1);
		*/	
		var pos=p.elm.$Layout.position();
		p.elm.i$frame.css({top:pos.top,left:pos.left});
	};
	
	//图标的事件绑定
	p.$cmdItems=$(".cmdicon");
	p.yourDaddy=function(evt){
		p.$cmdItems.removeClass("editBtn1");
		$(this).addClass("editBtn1");
		return false;
	};
	p.$cmdItems.click(p.yourDaddy);
	p.$cmdItems.filter(".cmdBold").click(p.txtCmd.Bold);
	p.$cmdItems.filter(".cmdUdl").click(p.txtCmd.Underline);
	p.$cmdItems.filter(".cmdColor").click(p.txtCmd.SetColor);
	p.$cmdItems.filter(".cmdItalic").click(p.txtCmd.Italic);
	p.$cmdItems.filter(".cmdAL").click(p.txtCmd.AlignLeft);
	p.$cmdItems.filter(".cmdAC").click(p.txtCmd.AlignCenter);
	p.$cmdItems.filter(".cmdAR").click(p.txtCmd.AlignRight);
	p.$cmdItems.filter(".cmdAF").click(p.txtCmd.AlignFull);
	p.$cmdItems.filter(".cmdVideo").click(p.txtCmd.AddVideoIcon);
	p.$cmdItems.filter(".elmAdd").click(p.elmCmd.AddCopy);
	p.$cmdItems.filter(".elmDel").click(p.elmCmd.Del);
	p.$cmdItems.filter(".elmMove0").bind("click",{up:true},p.elmCmd.Move);
	p.$cmdItems.filter(".elmMove1").bind("click",{up:false},p.elmCmd.Move);

	this.IsInit=true;
};
sohu.diyMenuBar.Tpl={
	video:'<a target="_blank" href="#" title=""><img height="10" width="16" alt="" src="images/vd.gif"/></a>'
};
