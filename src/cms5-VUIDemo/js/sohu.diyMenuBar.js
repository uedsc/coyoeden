/**
 * �༭�˵���-Ϊ�༭�Ի����ͼ����¼�
 * @author levinhuang
 * @dependency sohu.diyConsole��sohu.diyDialog
 */
sohu.diyMenuBar=function(opts){
	var p={};
	p._opts=$.extend(opts,{clElmCopyable:"elmc",clElmOn:"elmOn"});
	//�ı�ͼƬ����
	p.txtCmd={};
	p.txtCmd.Bold=function(evt){
		if(sohu.diyConsole.CurElm.$Layout.css("font-weight")=="bold")
			sohu.diyConsole.CurElm.$Layout.css("font-weight","normal");
		else
			sohu.diyConsole.CurElm.$Layout.css("font-weight","bold");
		
	};
	p.txtCmd.Italic=function(evt){
		if(sohu.diyConsole.CurElm.$Layout.css("font-style")=="italic")
			sohu.diyConsole.CurElm.$Layout.css("font-style","normal");
		else
			sohu.diyConsole.CurElm.$Layout.css("font-style","italic");
	};
	p.txtCmd.Underline=function(evt){
		if(sohu.diyConsole.CurElm.$Layout.css("text-decoration")=="underline")
			sohu.diyConsole.CurElm.$Layout.css("text-decoration","none")
		else
			sohu.diyConsole.CurElm.$Layout.css("text-decoration","underline")
	};
	p.txtCmd.AlignLeft=function(evt){
		if(sohu.diyConsole.CurElm.$Layout.css("text-align")=="left")
			sohu.diyConsole.CurElm.$Layout.css("text-align","");
		else
			sohu.diyConsole.CurElm.$Layout.css("text-align","left");
	};
	p.txtCmd.AlignCenter=function(evt){
		if(sohu.diyConsole.CurElm.$Layout.css("text-align")=="center")
			sohu.diyConsole.CurElm.$Layout.css("text-align","");
		else
			sohu.diyConsole.CurElm.$Layout.css("text-align","center");
	};
	p.txtCmd.AlignRight=function(evt){
		if(sohu.diyConsole.CurElm.$Layout.css("text-align")=="right")
			sohu.diyConsole.CurElm.$Layout.css("text-align","");
		else
			sohu.diyConsole.CurElm.$Layout.css("text-align","right");
	};
	p.txtCmd.AddVideoIcon=function(evt){
		sohu.diyConsole.CurElm.$Layout.append(sohu.diyMenuBar.Tpl.video);	
	};
	p.txtCmd.SetColor=function(evt){
		var _this=$(this);
		sohu.diyDialog.showColorPicker({
			onSubmit:function(c){
				sohu.diyConsole.CurElm.$Layout.css("color",c);
			},
			onChange:function(c){
				sohu.diyConsole.CurElm.$Layout.css("color",c);
			}
		});
	};
	//Ԫ������
	p.elmCmd={};
	p.elmCmd.AddCopy=function(evt){
		if(!sohu.diyConsole.CurElm) return;
		var $dom=sohu.diyConsole.CurElm.$CopyModel.clone(false).removeClass(p._opts.clElmOn).show();
		$dom.removeAttr("id");
		
		if(sohu.diyConsole.CurElm.IsSelfCopyable){
			sohu.diyConsole.CurElm.$Layout.after($dom);
			//apply diyElement features
			new sohu.diyElement({ct:sohu.diyConsole.CurElm.CT,$dom:$dom});
		}else{
			//���Ԫ�ص�id
			$dom.find(".elm").removeAttr("id");
			sohu.diyConsole.CurElm.$CopyModel.after($dom);
			//apply diyElement features
			$dom.find(".elm").each(function(i,o){
				new sohu.diyElement({ct:sohu.diyConsole.CurElm.CT,$dom:$(o)});
			});
		};
		//�ض�λ�����༭��
		sohu.diyConsole.CurElm.CT.Editor.Reposition();
	};
	p.elmCmd.Del=function(evt){
		if(!sohu.diyConsole.CurElm) return;
		var tempElm=sohu.diyConsole.CurElm;
		//�رձ༭�Ի���
		sohu.diyDialog.Hide();
		//ɾ��Ԫ��
		tempElm.$CopyModel.remove();
		if(opts.onDel){
			opts.onDel();
		};
	};
	p.elmCmd.Move=function(evt){
		if(!sohu.diyConsole.CurElm) return;
		//�ƶ�
		sohu.diyConsole.CurElm.Move(evt.data.up);
	};
	
	//ͼ����¼���
	p.$cmdItems=opts.$cmdItems||$(".cmdicon");
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
