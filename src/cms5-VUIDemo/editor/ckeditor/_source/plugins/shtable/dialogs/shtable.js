/*
Table plugin for sohu.com
@author Levinhuang
*/
(function()
{
	var widthPattern = /^(\d+(?:\.\d+)?)(px|%)$/,
		heightPattern = /^(\d+(?:\.\d+)?)px$/;

	var commitValue = function( data )
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
	
	
	function tableDialog( editor, command )
	{
		var $e = function( name ){ return new CKEDITOR.dom.element( name, editor.document ); };

		return {
			title : editor.lang.shtable.title,
			minWidth : 510,
			minHeight : CKEDITOR.env.ie ? 310 : 280,
			onShow : function()
			{
				/* 定义editor目前的插件的元信息 Meta*/
				editor.M={
					type:'shtable',
					dom:null
				};
				// Detect if there's a selected table.
				var selection = editor.getSelection(),
					ranges = selection.getRanges(),
					selectedTable = null;

				var rowsInput = this.getContentElement( 'info', 'txtRows' ),
					colsInput = this.getContentElement( 'info', 'txtCols' ),
					widthInput = this.getContentElement( 'info', 'txtWidth' );
				if ( command == 'shtableProperties' )
				{
					if ( ( selectedTable = editor.getSelection().getSelectedElement() ) )
					{
						if ( selectedTable.getName() != 'table' )
							selectedTable = null;
					}
					else if ( ranges.length > 0 )
					{
						// Webkit could report the following range on cell selection (#4948):
						// <table><tr><td>[&nbsp;</td></tr></table>]
						if ( CKEDITOR.env.webkit )
							ranges[ 0 ].shrink( CKEDITOR.NODE_ELEMENT );

						var rangeRoot = ranges[0].getCommonAncestor( true );
						selectedTable = rangeRoot.getAscendant( 'table', true );
					}

					// Save a reference to the selected table, and push a new set of default values.
					this._.selectedElement = selectedTable;
				}

				// Enable, disable and select the row, cols, width fields.
				if ( selectedTable )
				{
					this.setupContent( selectedTable );
					rowsInput && rowsInput.disable();
					colsInput && colsInput.disable();
					widthInput && widthInput.select();
				}
				else
				{
					rowsInput && rowsInput.enable();
					colsInput && colsInput.enable();
					rowsInput && rowsInput.select();
				}
			},
			onOk : function()
			{
				if ( this._.selectedElement )
				{
					var selection = editor.getSelection(),
						bms = editor.getSelection().createBookmarks();
				}

				var table = this._.selectedElement || $e( 'table' ),
					me = this,
					data = {};

				this.commitContent( data, table );

				if ( data.info )
				{
					var info = data.info;
					//table outer border style
					if(info.txtOBorder!='0'){
						var css = CKEDITOR.tools.cssLength( info.txtOBorder )+" solid "+info.txtOBorderColor;
						$(table.$).css("border",css);
					}else{
						$(table.$).css("border","none");
					}
					// caption bg
					if(data.caption){
						$(table.$).find("caption").css("background-color",info.txtCaptionBGColor);
					}
					// Generate the rows and cols.
					if ( !this._.selectedElement )
					{
						var tbody = table.append( $e( 'tbody' ) ),
							rows = parseInt( info.txtRows, 10 ) || 0,
							cols = parseInt( info.txtCols, 10 ) || 0;

						for ( var i = 0 ; i < rows ; i++ )
						{
							var row = tbody.append( $e( 'tr' ) );
							for ( var j = 0 ; j < cols ; j++ )
							{
								var cell = row.append( $e( 'td' ) );
								if ( !CKEDITOR.env.ie )
									cell.append( $e( 'br' ) );
							}
						}
					}

					// Modify the table headers. Depends on having rows and cols generated
					// correctly so it can't be done in commit functions.

					// Should we make a <thead>?
					var headers = info.selHeaders;
					if ( !table.$.tHead && ( headers == 'row' || headers == 'both' ) )
					{
						var thead = new CKEDITOR.dom.element( table.$.createTHead() );
						tbody = table.getElementsByTag( 'tbody' ).getItem( 0 );
						var theRow = tbody.getElementsByTag( 'tr' ).getItem( 0 );

						// Change TD to TH:
						for ( i = 0 ; i < theRow.getChildCount() ; i++ )
						{
							var th = theRow.getChild( i );
							if ( th.type == CKEDITOR.NODE_ELEMENT )
							{
								th.renameNode( 'th' );
								th.setAttribute( 'scope', 'col' );
							}
						}
						thead.append( theRow.remove() );
					}

					if ( table.$.tHead !== null && !( headers == 'row' || headers == 'both' ) )
					{
						// Move the row out of the THead and put it in the TBody:
						thead = new CKEDITOR.dom.element( table.$.tHead );
						tbody = table.getElementsByTag( 'tbody' ).getItem( 0 );

						var previousFirstRow = tbody.getFirst();
						while ( thead.getChildCount() > 0 )
						{
							theRow = thead.getFirst();
							for ( i = 0; i < theRow.getChildCount() ; i++ )
							{
								var newCell = theRow.getChild( i );
								if ( newCell.type == CKEDITOR.NODE_ELEMENT )
								{
									newCell.renameNode( 'td' );
									newCell.removeAttribute( 'scope' );
								}
							}
							theRow.insertBefore( previousFirstRow );
						}
						thead.remove();
					}

					// Should we make all first cells in a row TH?
					if ( !this.hasColumnHeaders && ( headers == 'col' || headers == 'both' ) )
					{
						for ( row = 0 ; row < table.$.rows.length ; row++ )
						{
							newCell = new CKEDITOR.dom.element( table.$.rows[ row ].cells[ 0 ] );
							newCell.renameNode( 'th' );
							newCell.setAttribute( 'scope', 'row' );
						}
					}

					// Should we make all first TH-cells in a row make TD? If 'yes' we do it the other way round :-)
					if ( ( this.hasColumnHeaders ) && !( headers == 'col' || headers == 'both' ) )
					{
						for ( i = 0 ; i < table.$.rows.length ; i++ )
						{
							row = new CKEDITOR.dom.element( table.$.rows[i] );
							if ( row.getParent().getName() == 'tbody' )
							{
								newCell = new CKEDITOR.dom.element( row.$.cells[0] );
								newCell.renameNode( 'td' );
								newCell.removeAttribute( 'scope' );
							}
						}
					}

					// Set the width and height.
					var styles = [];
					if ( info.txtHeight )
						table.setStyle( 'height', CKEDITOR.tools.cssLength( info.txtHeight ) );
					else
						table.removeStyle( 'height' );

					if ( info.txtWidth )
					{
						var type = info.cmbWidthType || 'pixels';
						table.setStyle( 'width', info.txtWidth + ( type == 'pixels' ? 'px' : '%' ) );
					}
					else
						table.removeStyle( 'width' );

					if ( !table.getAttribute( 'style' ) )
						table.removeAttribute( 'style' );
				}

				// Insert the table element if we're creating one.
				if ( !this._.selectedElement )
					//editor.insertElement( table );
					editor.setData(table.getOuterHtml());
				// Properly restore the selection inside table. (#4822)
				else
					selection.selectBookmarks( bms );

				return true;
			},
			contents : [
				{
					id : 'info',
					label : editor.lang.shtable.title,
					elements :
					[
						{
							type : 'hbox',
							widths : [ null, null ],
							styles : [ 'vertical-align:top' ],
							children :
							[
								{
									type : 'vbox',
									padding : 0,
									children :
									[
										{
											type : 'text',
											id : 'txtRows',
											'default' : 3,
											label : editor.lang.table.rows,
											//style : 'width:5em',
											labelLayout : 'horizontal',
											widths : [ '40%', '60%'],
											validate : function()
											{
												var pass = true,
													value = this.getValue();
												pass = pass && CKEDITOR.dialog.validate.integer()( value )
													&& value > 0;
												if ( !pass )
												{
													alert( editor.lang.table.invalidRows );
													this.select();
												}
												return pass;
											},
											setup : function( selectedElement )
											{
												this.setValue( selectedElement.$.rows.length );
											},
											commit : commitValue
										},
										{
											type : 'text',
											id : 'txtCols',
											'default' : 2,
											label : editor.lang.table.columns,
											//style : 'width:5em',
											labelLayout : 'horizontal',
											widths : [ '40%', '60%' ],
											validate : function()
											{
												var pass = true,
													value = this.getValue();
												pass = pass && CKEDITOR.dialog.validate.integer()( value )
													&& value > 0;
												if ( !pass )
												{
													alert( editor.lang.table.invalidCols );
													this.select();
												}
												return pass;
											},
											setup : function( selectedTable )
											{
												this.setValue( selectedTable.$.rows[0].cells.length);
											},
											commit : commitValue
										},
										{
											type : 'html',
											html : '&nbsp;'
										},
										{
											type : 'select',
											id : 'selHeaders',
											'default' : '',
											label : editor.lang.table.headers,
											labelLayout : 'horizontal',
											widths : [ '40%', '60%' ],
											items :
											[
												[ editor.lang.table.headersNone, '' ],
												[ editor.lang.table.headersRow, 'row' ],
												[ editor.lang.table.headersColumn, 'col' ],
												[ editor.lang.table.headersBoth, 'both' ]
											],
											setup : function( selectedTable )
											{
												// Fill in the headers field.
												var dialog = this.getDialog();
												dialog.hasColumnHeaders = true;

												// Check if all the first cells in every row are TH
												for ( var row = 0 ; row < selectedTable.$.rows.length ; row++ )
												{
													// If just one cell isn't a TH then it isn't a header column
													if ( selectedTable.$.rows[row].cells[0].nodeName.toLowerCase() != 'th' )
													{
														dialog.hasColumnHeaders = false;
														break;
													}
												}

												// Check if the table contains <thead>.
												if ( ( selectedTable.$.tHead !== null) )
													this.setValue( dialog.hasColumnHeaders ? 'both' : 'row' );
												else
													this.setValue( dialog.hasColumnHeaders ? 'col' : '' );
											},
											commit : commitValue
										},
										{
											type : 'text',
											id : 'txtBorder',
											'default' : 1,
											label : editor.lang.table.border,
											//style : 'width:3em',
											labelLayout : 'horizontal',
											widths : [ '40%', '60%' ],
											validate : CKEDITOR.dialog.validate['number']( editor.lang.table.invalidBorder ),
											setup : function( selectedTable )
											{
												this.setValue( selectedTable.getAttribute( 'border' ) || '' );
											},
											commit : function( data, selectedTable )
											{
												if ( this.getValue() )
													selectedTable.setAttribute( 'border', this.getValue() );
												else
													selectedTable.removeAttribute( 'border' );
											}
										},
										{
											id:"txtOBorder",
											type:"text",
											'default':1,
											label:editor.lang.shtable.oborder,
											labelLayout : 'horizontal',
											widths : [ '40%', '60%' ],
											validate : CKEDITOR.dialog.validate['number']( editor.lang.table.invalidBorder ),						
											setup:function(tb){
												var bMatch = heightPattern.exec( tb.$.style.borderWidth );
												if ( bMatch )
													this.setValue( bMatch[1] );
											},
											commit:commitValue
										},
										{
											id : 'cmbAlign',
											type : 'select',
											'default' : '',
											labelLayout : 'horizontal',
											widths : [ '40%', '60%' ],
											label : editor.lang.table.align,
											items :
											[
												[ editor.lang.common.notSet , ''],
												[ editor.lang.table.alignLeft , 'left'],
												[ editor.lang.table.alignCenter , 'center'],
												[ editor.lang.table.alignRight , 'right']
											],
											setup : function( selectedTable )
											{
												this.setValue( selectedTable.getAttribute( 'align' ) || '' );
											},
											commit : function( data, selectedTable )
											{
												if ( this.getValue() )
													selectedTable.setAttribute( 'align', this.getValue() );
												else
													selectedTable.removeAttribute( 'align' );
											}
										}
									]
								},
								{
									type : 'vbox',
									padding : 0,
									children :
									[
										{
											type : 'hbox',
											//widths : [ '5em' ],
											children :
											[
												{
													type : 'text',
													id : 'txtWidth',
													//style : 'width:5em',
													labelLayout : 'horizontal',
													widths : [ '40%', '60%' ],
													label : editor.lang.table.width,
													'default' : 200,
													validate : CKEDITOR.dialog.validate['number']( editor.lang.table.invalidWidth ),

													// Extra labelling of width unit type.
													onLoad : function()
													{
														var widthType = this.getDialog().getContentElement( 'info', 'cmbWidthType' ),
															labelElement = widthType.getElement(),
															inputElement = this.getInputElement(),
															ariaLabelledByAttr = inputElement.getAttribute( 'aria-labelledby' );

														inputElement.setAttribute( 'aria-labelledby', [ ariaLabelledByAttr, labelElement.$.id ].join( ' ' ) );
													},

													setup : function( selectedTable )
													{
														var widthMatch = widthPattern.exec( selectedTable.$.style.width );
														if ( widthMatch )
															this.setValue( widthMatch[1] );
														else
															this.setValue( '' );
													},
													commit : commitValue
												},
												{
													id : 'cmbWidthType',
													type : 'select',
													//label : editor.lang.table.widthUnit,
													//labelLayout : 'horizontal',
													//widths : [ '0%', '100%' ],
													'default' : 'pixels',
													items :
													[
														[ editor.lang.table.widthPx , 'pixels'],
														[ editor.lang.table.widthPc , 'percents']
													],
													setup : function( selectedTable )
													{
														var widthMatch = widthPattern.exec( selectedTable.$.style.width );
														if ( widthMatch )
															this.setValue( widthMatch[2] == 'px' ? 'pixels' : 'percents' );
													},
													commit : commitValue
												}
											]
										},
										{
											type : 'hbox',
											//widths : [ '5em' ],
											children :
											[
												{
													type : 'text',
													id : 'txtHeight',
													//style : 'width:5em',
													labelLayout : 'horizontal',
													widths : [ '40%', '60%' ],													
													label : editor.lang.table.height,
													'default' : '',
													validate : CKEDITOR.dialog.validate['number']( editor.lang.table.invalidHeight ),

													// Extra labelling of height unit type.
													onLoad : function()
													{
														var heightType = this.getDialog().getContentElement( 'info', 'htmlHeightType' ),
															labelElement = heightType.getElement(),
															inputElement = this.getInputElement(),
															ariaLabelledByAttr = inputElement.getAttribute( 'aria-labelledby' );

														inputElement.setAttribute( 'aria-labelledby', [ ariaLabelledByAttr, labelElement.$.id ].join( ' ' ) );
													},

													setup : function( selectedTable )
													{
														var heightMatch = heightPattern.exec( selectedTable.$.style.height );
														if ( heightMatch )
															this.setValue( heightMatch[1] );
													},
													commit : commitValue
												},
												{
													id : 'htmlHeightType',
													type : 'html',
													html : editor.lang.table.widthPx
												}
											]
										},
										{
											type : 'html',
											html : '&nbsp;'
										},
										{
											type : 'text',
											id : 'txtCellSpace',
											//style : 'width:3em',
											labelLayout : 'horizontal',
											widths : [ '19%', '81%' ],
											label : editor.lang.table.cellSpace,
											'default' : 1,
											validate : CKEDITOR.dialog.validate['number']( editor.lang.table.invalidCellSpacing ),
											setup : function( selectedTable )
											{
												this.setValue( selectedTable.getAttribute( 'cellSpacing' ) || '' );
											},
											commit : function( data, selectedTable )
											{
												if ( this.getValue() )
													selectedTable.setAttribute( 'cellSpacing', this.getValue() );
												else
													selectedTable.removeAttribute( 'cellSpacing' );
											}
										},
										{
											type : 'text',
											id : 'txtCellPad',
											//style : 'width:3em',
											labelLayout : 'horizontal',
											widths : [ '19%', '81%' ],											
											label : editor.lang.table.cellPad,
											'default' : 1,
											validate : CKEDITOR.dialog.validate['number']( editor.lang.table.invalidCellPadding ),
											setup : function( selectedTable )
											{
												this.setValue( selectedTable.getAttribute( 'cellPadding' ) || '' );
											},
											commit : function( data, selectedTable )
											{
												if ( this.getValue() )
													selectedTable.setAttribute( 'cellPadding', this.getValue() );
												else
													selectedTable.removeAttribute( 'cellPadding' );
											}
										},
										{
											id:'txtOBorderColor',
											type:'text',
											labelLayout : 'horizontal',
											widths : [ '19%', '81%' ],
											label : editor.lang.shtable.obordercolor,
											'default':'#ffffff',
											setup:function(tb){
												var cl=$(tb.$).css("borderColor")||'#ffffff';
												$('#'+this._.inputId).val(cl).css("background-color",cl).attr("readonly",true);
											},
											commit:commitValue,
											onClick:function(e){
												var _this=this;
												var _close=function(evt){
													colorDialogHandler.close(evt,this,_this,_close);
												};
												var _onShow=function(e){
													colorDialogHandler.show(e,this,_this);
												};
												editor.openDialog( 'colordialog', function()
												{
													this.on( 'ok', _close);
													this.on( 'cancel', _close);
													this.on('show',_onShow)
												} );
											}
										},
										{
											id:'txtCaptionBGColor',
											type:'text',
											labelLayout : 'horizontal',
											widths : [ '19%', '81%' ],
											label : editor.lang.shtable.captionbgcolor,
											'default':'#ffffff',
											setup:function(tb){
												var cl=$(tb.$).find("caption").css("background-color")||'#ffffff';
												$('#'+this._.inputId).val(cl).css("background-color",cl).attr("readonly",true);
											},
											commit:commitValue,
											onClick:function(e){
												var _this=this;
												var _close=function(evt){
													colorDialogHandler.close(evt,this,_this,_close);
												};
												var _onShow=function(e){
													colorDialogHandler.show(e,this,_this);
												};
												editor.openDialog( 'colordialog', function()
												{
													this.on( 'ok', _close);
													this.on( 'cancel', _close);
													this.on('show',_onShow)
												} );
											}											
										}										
									]
								}
							]
						},
						{
							type : 'html',
							align : 'right',
							html : ''
						},
						{
							type : 'vbox',
							padding : 0,
							children :
							[
								{
									type : 'text',
									id : 'txtCaption',
									label : editor.lang.table.caption,
									labelLayout : 'horizontal',
									widths : [ '17%', '83%' ],
									setup : function( selectedTable )
									{
										var nodeList = selectedTable.getElementsByTag( 'caption' );
										if ( nodeList.count() > 0 )
										{
											var caption = nodeList.getItem( 0 );
											caption = ( caption.getChild( 0 ) && caption.getChild( 0 ).getText() ) || '';
											caption = CKEDITOR.tools.trim( caption );
											this.setValue( caption );
										}
									},
									commit : function( data, table )
									{
										var caption = this.getValue(),
											captionElement = table.getElementsByTag( 'caption' );
										data[this.id]=caption;
										data.caption=false;
										if ( caption )
										{
											data.caption=true;
											if ( captionElement.count() > 0 )
											{
												captionElement = captionElement.getItem( 0 );
												captionElement.setHtml( '' );
											}
											else
											{
												captionElement = new CKEDITOR.dom.element( 'caption', editor.document );
												if ( table.getChildCount() )
													captionElement.insertBefore( table.getFirst() );
												else
													captionElement.appendTo( table );
											}
											captionElement.append( new CKEDITOR.dom.text( caption, editor.document ) );
										}
										else if ( captionElement.count() > 0 )
										{
											for ( var i = captionElement.count() - 1 ; i >= 0 ; i-- )
												captionElement.getItem( i ).remove();
										}
									}
								},
								{
									type : 'text',
									id : 'txtSummary',
									label : editor.lang.table.summary,
									labelLayout : 'horizontal',
									widths : [ '17%', '83%' ],
									setup : function( selectedTable )
									{
										this.setValue( selectedTable.getAttribute( 'summary' ) || '' );
									},
									commit : function( data, selectedTable )
									{
										if ( this.getValue() )
											selectedTable.setAttribute( 'summary', this.getValue() );
										else
											selectedTable.removeAttribute( 'summary' );
									}
								}
							]
						}
					]
				}
			]
		};
	}

	CKEDITOR.dialog.add( 'shtable', function( editor )
		{
			return tableDialog( editor, 'shtable' );
		} );
	CKEDITOR.dialog.add( 'shtableProperties', function( editor )
		{
			return tableDialog( editor, 'shtableProperties' );
		} );
})();
