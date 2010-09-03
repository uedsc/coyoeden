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
		/*
		var vd=null;
		if((vd=sohu.diyConsole.CurElm.$Layout.find('.vdIcon')).length==0){
			sohu.diyConsole.CurElm.$Layout.append(sohu.diyMenuBar.Tpl.video);
			sohu.diyConsole.CurElm.i$frame[0].i$Body().append(sohu.diyMenuBar.Tpl.video);			
		}else{
			vd.remove();
			sohu.diyConsole.CurElm.i$frame[0].i$Body().find('.vdIcon').remove();
		};
		*/
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
		var $dom=sohu.diyConsole.CurElm.$CopyModel.clone(false).removeClass(p._opts.clElmOn).show().css("visibility","visible");
		if($dom.attr("id")!="") $dom.attr("id","");
		
		if(sohu.diyConsole.CurElm.IsSelfCopyable){
			var $if=sohu.diyConsole.CurElm.$CopyModel.next("iframe");
			if($if.length>0){
				$if.after($dom);
			}else{
				sohu.diyConsole.CurElm.$Layout.after($dom);
			};
			//apply diyElement features
			new sohu.diyElement({ct:sohu.diyConsole.CurElm.CT,$dom:$dom});
		}else{
			//清楚iframe.editable
			$dom.find(".editable").remove();
			//清楚元素的id
			$dom.find(".elm").attr("id","").css("visibility","visible");
			
			sohu.diyConsole.CurElm.$CopyModel.after($dom);
			//apply diyElement features
			$dom.find(".elm").each(function(i,o){
				new sohu.diyElement({ct:sohu.diyConsole.CurElm.CT,$dom:$(o)});
			});
		};
		//重定位分栏编辑器
		sohu.diyConsole.CurElm.CT.Editor.Reposition();
	};
	p.elmCmd.Del=function(evt){
		if(!sohu.diyConsole.CurElm) return;
		var tempElm=sohu.diyConsole.CurElm;
		//关闭编辑对话框
		sohu.diyDialog.Hide();
		//删除元素
		if(tempElm.IsSelfCopyable){
			tempElm.$Layout.next("iframe").remove();
			tempElm.$Layout.remove();		
		}else{
			tempElm.$CopyModel.remove();
		};

		//重定位分栏编辑器
		//sohu.diyConsole.CurElm.CT.Editor.Reposition();
	};
	p.elmCmd.Move=function(evt){
		if(!sohu.diyConsole.CurElm) return;
		//移动
		sohu.diyConsole.CurElm.Move(evt.data.up);
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
	video:'<a class="vdIcon" target="_blank" href="#" title=""><img height="10" width="16" alt="" src="images/vd.gif"/></a>'
};
