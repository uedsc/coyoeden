/**
 * 编辑菜单类-为编辑对话框的图标绑定事件
 * @author levinhuang
 * @dependency sohu.diyConsole
 */
sohu.diyMenuBar=function(opts){
	var p={};
	p._opts=$.extend(opts,{clElmCopyable:"elmc",clElmOn:"elmOn"});
	//文本图片命令
	var txtCmd={};
	txtCmd.fontCbk=function(iframeEditor){sohu.diyConsole.CurElm.$Layout.css(iframeEditor.iCurCss);};
	txtCmd.Bold=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Bold",null);};
	txtCmd.Italic=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Italic",null);}};
	txtCmd.Underline=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("Underline",null);};
	txtCmd.SizeUp=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("increasefontsize",null);};
	txtCmd.SizeDown=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("decreasefontsize",null);};
	txtCmd.AlignLeft=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyleft",null,txtCmd.fontCbk);};
	txtCmd.AlignCenter=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifycenter",null,txtCmd.fontCbk);};
	txtCmd.AlignRight=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyright",null,txtCmd.fontCbk);};
	txtCmd.AlignFull=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("justifyfull",null,txtCmd.fontCbk);};
	txtCmd.AddUL=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("insertunorderedlist",null);};
	txtCmd.AddOL=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("insertorderedlist",null);};
	txtCmd.Undo=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("undo",null);};
	txtCmd.Redo=function(evt){sohu.diyConsole.CurElm.i$frame[0].iDoCommand("redo",null);};
	txtCmd.AddVideoIcon=function(evt){
		sohu.diyConsole.CurElm.$Layout.append(sohu.diyMenuBar.Tpl.video);
		sohu.diyConsole.CurElm.i$frame[0].i$Body.append(sohu.diyMenuBar.Tpl.video);
	};
	//元素命令
	var elmCmd={};
	elmCmd.AddCopy=function(evt){
		if(!sohu.diyConsole.CurElm) return;
		var $dom=sohu.diyConsole.CurElm.$Layout.clone(false).attr("id","").removeClass(p._opts.clElmOn);
		var $if=sohu.diyConsole.CurElm.$Layout.next("iframe");
		if($if.length>0){
			$if.after($dom);
		}else{
			sohu.diyConsole.CurElm.$Layout.after($dom);
		}	
		//apply diyElement features
		new sohu.diyElement({ct:sohu.diyConsole.CurElm.CT,$dom:$dom});
	};
	elmCmd.Del=function(evt){
		if(!sohu.diyConsole.CurElm) return;
		//关闭编辑模式
		sohu.diyConsole.CurElm.HideEditor(false);
		//删除元素
		sohu.diyConsole.CurElm.$Layout.next("iframe").remove();
		sohu.diyConsole.CurElm.$Layout.remove();
	};
	elmCmd.Move=function(evt){
		p.elm=sohu.diyConsole.CurElm;
		if(!p.elm) return;
		
		//关闭编辑模式
		sohu.diyConsole.CurElm.HideEditor(false);
		//移动
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
			
		//打开编辑模式
		sohu.diyConsole.CurElm.$Layout.trigger("click");
	};
	
	//图标的事件绑定
	var yourDaddy=function(evt){return false;};
	$(".cmdicon").click(yourDaddy);
	$(".cmdBold").click(txtCmd.Bold);
	$(".cmdUdl").click(txtCmd.Underline);
	$(".cmdItalic").click(txtCmd.Italic);
	$(".cmdAL").click(txtCmd.AlignLeft);
	$(".cmdAC").click(txtCmd.AlignCenter);
	$(".cmdAR").click(txtCmd.AlignRight);
	$(".cmdAF").click(txtCmd.AlignFull);
	$(".cmdVideo").click(txtCmd.AddVideoIcon);
	$(".elmAdd").click(elmCmd.AddCopy);
	$(".elmDel").click(elmCmd.Del);
	$(".elmMove0").bind("click",{up:true},elmCmd.Move);
	$(".elmMove1").bind("click",{up:false},elmCmd.Move);
	
};
sohu.diyMenuBar.Tpl={
	video:'<a target="_blank" href="#" title=""><img height="10" width="16" alt="" src="images/vd.gif"/></a>'
};
