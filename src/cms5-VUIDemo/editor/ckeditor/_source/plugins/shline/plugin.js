/**
 * sohu empty line plugin
 * @author levinhuang
 */
CKEDITOR.plugins.add( 'shline',
{
	lang:['en','zh-cn'],
	beforeInit:function(editor){
		//contextMenu order
		if(editor.config.menu_groups)
			editor.config.menu_groups+=',shline,shlinedelete';
		else
			editor.config.menu_groups='shline,shlinedelete';	
	},
	init:function(editor){
		var lang=editor.lang.shline;
		//cmd
		editor.addCommand( 'shline', new CKEDITOR.dialogCommand( 'shline' ) );
		editor.addCommand( 'shlineEdit', new CKEDITOR.dialogCommand( 'shlineEdit' ) );
		editor.addCommand( 'shlineDelete',{
			exec : function( editor )
			{
				var selection = editor.getSelection();
				var startElement = selection && selection.getStartElement();
				var line = startElement && startElement.getAscendant( 'div', true );
				if(!line.hasClass("shline"))
					line=null;

				if ( !line )
					return;

				// Maintain the selection point at where the shline was deleted.
				selection.selectElement( line );
				var range = selection.getRanges()[0];
				range.collapse();
				selection.selectRanges( [ range ] );

				// If the shline's parent has only one child, remove it,except body,as well.( #5416 )
				var parent =line.getParent();
				if ( parent.getChildCount() == 1 && parent.getName() != 'body' )
					parent.remove();
				else
					line.remove();
			}
		});
		//ui on eidtor
		editor.ui.addButton( 'SHLine',
			{
				label : lang.toolbar,
				command : 'shline'
			});
		//dialog
		CKEDITOR.dialog.add( 'shline', this.path + 'dialogs/shline.js' );
		CKEDITOR.dialog.add( 'shlineEdit', this.path + 'dialogs/shline.js' );
		// If the "menu" plugin is loaded, register the menu items.
		if ( editor.addMenuItems )
		{
			editor.addMenuItems(
				{
					shline :
					{
						label : lang.menu,
						command : 'shlineEdit',
						group : 'shline',
						order : 5
					},

					shlinedelete :
					{
						label : lang.deleteMe,
						command : 'shlineDelete',
						group : 'shline',
						order : 1
					}
				} );
		}
		editor.on( 'doubleclick', function( evt )
			{
				var element = evt.data.element;

				if ( element.hasClass( 'shline' ) )
					evt.data.dialog = 'shlineEdit';
			});
		
		// If the "contextmenu" plugin is loaded, register the listeners.
		if ( editor.contextMenu )
		{
			editor.contextMenu.addListener( function( element, selection )
				{
					if ( !element )
						return null;

					var isIt = element.hasClass( 'shline' ) || $(element.$).parents(".shline").length>0;

					if ( isIt )
					{
						return {
							shlinedelete : CKEDITOR.TRISTATE_OFF,
							shline : CKEDITOR.TRISTATE_OFF
						};
					}

					return null;
				} );
		}
	
	}//init
}
);