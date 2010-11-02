/**
 * �༭�˵���-Ϊ�༭�Ի����ͼ����¼�
 * @author levinhuang
 * @dependency sohu.diyConsole��sohu.diyDialog
 */
sohu.diyMenuBar=function(opts){
	var p={};
	p._opts=$.extend(opts,{clElmCopyable:"vstp_elmc",clElmOn:"vstp_elmOn",clElm:'vstp_elm'});
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
			onSelect:function(c){
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
			$dom.find("."+p._opts.clElm).removeAttr("id");
			sohu.diyConsole.CurElm.$CopyModel.after($dom);
			//apply diyElement features
			$dom.find("."+p._opts.clElm).each(function(i,o){
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
		//�ض�λ�����༭��
		sohu.diyConsole.CurElm.CT.Editor.Reposition();		
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
	p.$cmdItems=opts.$cmdItems||$(".vstp_cmdicon");
	p.yourDaddy=function(evt){
		p.$cmdItems.removeClass("vstp_editBtn1");
		$(this).addClass("vstp_editBtn1");
		return false;
	};
	p.$cmdItems.click(p.yourDaddy);
	p.$cmdItems.filter(".vstp_cmdBold").click(p.txtCmd.Bold);
	p.$cmdItems.filter(".vstp_cmdUdl").click(p.txtCmd.Underline);
	p.$cmdItems.filter(".vstp_cmdColor").click(p.txtCmd.SetColor);
	p.$cmdItems.filter(".vstp_cmdItalic").click(p.txtCmd.Italic);
	p.$cmdItems.filter(".vstp_cmdAL").click(p.txtCmd.AlignLeft);
	p.$cmdItems.filter(".vstp_cmdAC").click(p.txtCmd.AlignCenter);
	p.$cmdItems.filter(".vstp_cmdAR").click(p.txtCmd.AlignRight);
	p.$cmdItems.filter(".vstp_cmdAF").click(p.txtCmd.AlignFull);
	p.$cmdItems.filter(".vstp_cmdVideo").click(p.txtCmd.AddVideoIcon);
	p.$cmdItems.filter(".vstp_elmAdd").click(p.elmCmd.AddCopy);
	p.$cmdItems.filter(".vstp_elmDel").click(p.elmCmd.Del);
	p.$cmdItems.filter(".vstp_elmMove0").bind("click",{up:true},p.elmCmd.Move);
	p.$cmdItems.filter(".vstp_elmMove1").bind("click",{up:false},p.elmCmd.Move);

	this.IsInit=true;
};
sohu.diyMenuBar.Tpl={
	video:'<a class="vstp_vdIcon" target="_blank" href="#" title=""><img height="10" width="16" alt="" src="http://images.sohu.com/uiue/vd.gif"/></a>'
};
