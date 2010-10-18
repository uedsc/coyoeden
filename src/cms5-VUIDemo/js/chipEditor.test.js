/**
 * @author levin
 */
var test = function() {
	var p={},pub={};
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
	p.onFlagMEnter=function(evt){	
		var $i=jQuery(this);
		if($i.hasClass("cedt_on")) return;/* �ڱ༭ĳ��Ԫ��ʱ��Ƭ�����on */
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
			//p._$chipCover_t.removeClass("cedt_on");
			//$i.bind("mouseenter",p.onFlagMEnter);
		});
		
		
		p._$chipCover_t=$i;
		//p._$chipCover_t.addClass("cedt_on");
	};	
	//private area
	p.initVar = function(opts) { 
		p._$chipCover=jQuery("#chipCover");
		p._cssFlash=".flash";
	};
	p.onLoaded = function() { };
	p.initEvents = function(opts) {
		jQuery(document).ready(p.onLoaded);
		jQuery(".chip").live("mouseenter",p.onFlagMEnter).mouseleave(function(evt){
			p._$chipCover_t.unbind("mouseenter").bind("mouseenter",p.onFlagMEnter);
		});
		//�ɲ�ĵ����¼�
		p._$chipCover.click(function(evt){
			if(p._$chipCover_t.is(p._cssFlash)){
			//���뽹��ͼ�༭����
			chipEditor.Show(p._$chipCover_t,{
				tabs:[1,2],
				afterShow:function(hash,dlg){
					//show the flash tip
					dlg.$TabC.eq(1).find(".cedt_row").hide().filter(".cedt_flashTip").show();
					//content of tab no2
					chipEditor.MCE().setContent(dlg.$Chip.html());
					dlg.$Code.val(chipEditor.MCE().getContent());
					dlg.$CT.addClass("cedt_jqmFlash");/* ��������ť����ʾ */
					//��ť��
					dlg.$BtnTest.click(function(evt){
						alert("�������");return false;
					});
					//��λ�༭��λ��
					if(dlg.$Chip){
						var of=dlg.$Chip.offset();
						dlg.$Layout.css({
							top:of.top,
							left:of.left,
							"margin-left":"auto"
						});					
					};
				},
				onSave:function(dlg){alert("onSave");},
				onExternal:function(dlg){alert("onExternal");}
			});
				return;
			};
			
			p._$chipCover.trigger("mouseleave");
			p._$chipCover_t.unbind("mouseenter");
			
			//��ʾ��Ƭ�༭��
			chipEditor.Show(p._$chipCover_t,{
				/* �ϴ�ͼƬ */
				onUpPic:function(dlg){alert("onUpPic");},
				/* �޸ļ�¼ */
				onLoadHis:function(dlg){alert("onLoadHis");},
				/* ������� */
				onTest:function(dlg){alert("onTest");},
				/* ���� */
				onSave:function(dlg){alert("onSave");},
				/* ȡ�� */
				onCancel:function(dlg){},
				/* ��� */
				onExternal:function(dlg){alert("onExternal");},
				/* ����ͼ�༭ */
				onFlashEdit:function(dlg){alert("onFlashEdit");},
				afterShow:function(hash,dlg){
					//��λ�༭��λ��
					if(dlg.$Chip){
						var of=dlg.$Chip.offset();
						dlg.$Layout.css({
							top:of.top,
							left:of.left,
							"margin-left":"auto"
						});					
					};
				}
			});
		});
	};
	//public area
	pub.Init = function(opts) {
		p.initVar(opts);
		p.initEvents(opts);
	};
	return pub;
} (); 

test.Init({});
