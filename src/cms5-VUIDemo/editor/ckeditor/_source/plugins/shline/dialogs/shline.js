/**
 * @author levinhuang
 */
(function(){
	//Internal methods used by the dialog definition
	//1,utils functions
	/**
	 * commit field data
	 * @param {Object} data
	 */
	var commit= function( data )
	{
		var id = this.id;
		if ( !data.info )
			data.info = {};
		data.info[id] = this.getValue();
	};
	var colorDialogHandler={
		close:function(evt,dialog,inputElm,caller){
			dialog.removeListener( 'ok', caller );
			dialog.removeListener( 'cancel', caller );
			if(evt.name=='cancel') return;
			var c=dialog.getContentElement( 'picker', 'selectedColor' ).getValue();
			$('#'+inputElm._.inputId).val(c).css("background-color",c);
		},
		show:function(evt,dialog,inputElm){
			dialog.getContentElement( 'picker', 'selectedColor' ).setValue(inputElm.getValue());
		}
	};
	
	//2,dialog callback functions
	var dlgcbk={};
	/**
	 * callback for showing the dialog
	 * @param {Object} editor
	 * @param {Object} dlg
	 * @param {Object} command name for the dialog
	 */
	dlgcbk.show=function(editor,dlg,cmd){
		/* 定义editor目前的插件的元信息 Meta*/
		editor.M={
			type:'shline',
			dom:null
		};
		//detect if there is a  selected shline 
		editor.M.selection = editor.getSelection();
		editor.M.startElm=editor.M.selection && editor.M.selection.getStartElement();
		editor.M.selectedElm=editor.M.selection.getSelectedElement();
		editor.M.ranges = editor.M.selection.getRanges();
		
		if (cmd == "shlineEdit") {
			if ((editor.M.dom = editor.M.selectedElm)) {
				if (!editor.M.dom.hasClass("shline"))
				{
					var line=editor.M.startElm && editor.M.startElm.getAscendant('div',true);
					if(!line.hasClass('shline')){line=null;};
					editor.M.dom=line;
				}
			}
			else
			{
				if (editor.M.ranges.length > 0) {
					// Webkit could report the following range on cell selection (#4948):
					// <table><tr><td>[&nbsp;</td></tr></table>]
					if (CKEDITOR.env.webkit) 
						editor.M.ranges[0].shrink(CKEDITOR.NODE_ELEMENT);
					
					var rangeRoot = editor.M.ranges[0].getCommonAncestor(true);
					editor.M.dom = rangeRoot.getAscendant('div', true);
				}//if
			}//else 

			if (editor.M.dom && !editor.M.dom.hasClass('shline')) 
				editor.M.dom = null;
			
			// Save a reference to the selected obj, and push a new set of default values.
			dlg._.selectedElement = editor.M.dom;
		}//cmd=="shlineEdit"
		
		if( editor.M.dom){
			//select the dom
			editor.M.selection.selectElement(editor.M.dom);
			//setup content
			dlg.setupContent( editor.M.dom );
		}
	};
	dlgcbk.ok=function(editor,dlg,cmd){
		var $e = function( name ){ return new CKEDITOR.dom.element( name, editor.document ); };
		if ( dlg._.selectedElement )
		{
				editor.M.bms = editor.M.selection.createBookmarks();
		}else
			editor.M.isNew=true;

		editor.M.dom = dlg._.selectedElement || $e( 'div' ),
			me = dlg,
			data = {};
		
		editor.M.$obj=$(editor.M.dom.$).addClass("ct vspace shline");

		dlg.commitContent( data, editor.M.dom );
		
		if(data.info){
			//set the height
			editor.M.$obj.css({
				height:data.info.txtHeight+'px'
			});
			//shall we show the line
			var line=editor.M.$obj.find("hr");
			if(data.info.ddlLine!=''){
				var css={
					width:data.info.txtWidth+'%',
					border:'1px '+data.info.ddlLine+' '+data.info.txtLineColor,
					left:(100-data.info.txtWidth)/2+'%'
				};
				if(line.length==0){
					$("<hr/>").css( css ).appendTo( editor.M.$obj.empty() );
				}else{
					line.css(css);
				}
			}else{
				line.remove();
				editor.M.$obj.html("&nbsp;&nbsp;");	
			};
				
		};
		
		// Insert the shline element if we're creating one.
		if (!dlg._.selectedElement) {
			//editor.insertElement( edInfo.shline );
			editor.setData(editor.M.dom.getOuterHtml());
			/*
			 * Q:How to register event handlers?Switch between different modes of the editor will reset the elements.
			 */
			//editor.M.dom.on("click", function(evt){alert("hi");});
			//editor.M.$obj.click(function(evt){alert("hi");});
		}else// Properly restore the selection inside shline. (#4822) 
			editor.M.selection.selectBookmarks(editor.M.bms);

		return true;
	};
	//3,dialog definition factory
	var dlgDef=function(editor,cmd){
		return {
			title : editor.lang.shline.title /* title in string*/,
			minWidth : 400/*number of pixels*/,
			minHeight : 120/*number of pixels*/,
			onOk: function(){dlgcbk.ok(editor,this,cmd);}/*function*/ ,
			onShow: function(){dlgcbk.show(editor,this,cmd); }/*function*/,
			contents:
			[
				{
					id: 'info',
					label: editor.lang.shline.title,
					elements: 
					[
						{/*line1*/
							type: 'hbox',
							width: [null, null],
							children: 
							[
								{
									type: 'vbox',
									children: [{
										type: 'text',
										id: 'txtHeight',
										'default': '10',
										width: ['20%', '80%'],
										label: editor.lang.shline.height,
										labelLayout: 'horizontal',
										validate: function(){
											var pass = true, value = this.getValue();
											pass = pass && CKEDITOR.dialog.validate.integer()(value) &&
											value > 0;
											if (!pass) {
												alert(editor.lang.shline.invalidHeight);
												this.select();
											}
											return pass;
										},
										setup: function(shline){
											var h = $(shline.$).height();
											this.setValue(h);
										},
										commit: commit
									}, {
										type: 'text',
										id: 'txtWidth',
										'default': '100',
										width: ['20%', '80%'],
										label: editor.lang.shline.width,
										labelLayout: 'horizontal',
										validate: function(){
											var pass = true, value = this.getValue();
											pass = pass && CKEDITOR.dialog.validate.integer()(value) &&
											value > 0&&value<=100;
											if (!pass) {
												alert(editor.lang.shline.invalidWidth);
												this.select();
											}
											return pass;
										},
										setup: function(shline){
											var w=$(shline.$).find("hr").css("width")||"";
											w=w.indexOf("%")<1?false:parseInt(w,10);
											w=w||100;
											this.setValue(w);
										},
										commit: commit
									}]
								}, /* line1-col1 */ 
								{
									type: 'vbox',
									children: [{
										type: 'select',
										id: 'ddlLine',
										'default': '',
										width: ['20%', '80%'],
										label: editor.lang.shline.linestyle,
										labelLayout: 'horizontal',
										items: [[editor.lang.shline.linestyleNone, ''], [editor.lang.shline.linestyleSolid, 'solid'], [editor.lang.shline.linestyleDotted, 'dotted']],
										setup: function(shline){
											var h = $(shline.$).find("hr").css("borderStyle");
											if (h && h != '' && h != 'dotted') 
												h = 'dotted';
											else
												h=h||'';
												
											this.setValue(h);
										},
										commit: commit
									}, {
										type: 'text',
										id: 'txtLineColor',
										'default': '#000000',
										width: ['20%', '80%'],
										label: editor.lang.shline.linecolor,
										labelLayout: 'horizontal',
										setup: function(shline){
											var cl = $(shline.$).find("hr").css("borderColor") || '#000000';
											$('#' + this._.inputId).val(cl).css("background-color", cl).attr("readonly", true);
										},
										commit: commit,
										onClick: function(e){
											var _this = this;
											var _close = function(evt){
												colorDialogHandler.close(evt, this, _this, _close);
											};
											var _onShow = function(e){
												colorDialogHandler.show(e, this, _this);
											};
											editor.openDialog('colordialog', function(){
												this.on('ok', _close);
												this.on('cancel', _close);
												this.on('show', _onShow)
											});
										}
									}]
								} /* line1-col2 */
							]
						} /* line1 */
					]
				} /*elements*/			
			]/*content definition, basically the UI of the dialog*/			
		};
	};
	//5,add the dialog
	CKEDITOR.dialog.add('shline',function(editor){return dlgDef(editor,"shline");});
	CKEDITOR.dialog.add('shlineEdit',function(editor){return dlgDef(editor,"shlineEdit");});
})();
