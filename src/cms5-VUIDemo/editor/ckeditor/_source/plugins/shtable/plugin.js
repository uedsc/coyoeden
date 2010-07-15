/*
*	Table plugin for Sohu.com.Stop the offical table plugin via 'removePlugins' config option before using this plugin!
*	@author: levinhuang
*/

CKEDITOR.plugins.add( 'shtable',
{
	lang : [ 'en' ,'zh-cn'],
	init : function( editor )
	{
		var lang = editor.lang.shtable,
			lang0 = editor.lang.table;

		editor.addCommand( 'shtable', new CKEDITOR.dialogCommand( 'shtable' ) );
		editor.addCommand( 'shtableProperties', new CKEDITOR.dialogCommand( 'shtableProperties' ) );

		editor.ui.addButton( 'SHTable',
			{
				label : lang.toolbar,
				command : 'shtable'
			});

		CKEDITOR.dialog.add( 'shtable', this.path + 'dialogs/shtable.js' );
		CKEDITOR.dialog.add( 'shtableProperties', this.path + 'dialogs/shtable.js' );

		// If the "menu" plugin is loaded, register the menu items.
		if ( editor.addMenuItems )
		{
			editor.addMenuItems(
				{
					table :
					{
						label : lang0.menu,
						command : 'shtableProperties',
						group : 'shtable',
						order : 5
					},

					tabledelete :
					{
						label : lang0.deleteTable,
						command : 'tableDelete',
						group : 'shtable',
						order : 1
					}
				} );
		}
		editor.on( 'doubleclick', function( evt )
			{
				var element = evt.data.element;

				if ( element.is( 'table' ) )
					evt.data.dialog = 'shtableProperties';
			});
		
		// If the "contextmenu" plugin is loaded, register the listeners.
		if ( editor.contextMenu )
		{
			editor.contextMenu.addListener( function( element, selection )
				{
					if ( !element )
						return null;

					var isTable	= element.is( 'table' ) || element.hasAscendant( 'table' );

					if ( isTable )
					{
						return {
							tabledelete : CKEDITOR.TRISTATE_OFF,
							table : CKEDITOR.TRISTATE_OFF
						};
					}

					return null;
				} );
		}
	}
} );
