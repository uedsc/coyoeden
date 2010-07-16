/**
 * flash plugin for sohu.com
 * @author levinhuang
 */
CKEDITOR.plugins.add( 'shflash',
{
	requires: [ 'iframedialog' ],
	lang:['zh-cn'],
	init:function(editor){
        var lang= editor.lang.shflash;
		//cmd
		editor.addCommand( 'shflash', new CKEDITOR.dialogCommand( 'shflash' ) );
		//ui on eidtor
		editor.ui.addButton( 'SHFlash',
			{
				label : lang.toolbar,
				command : 'shflash'
			});
		//dialog
		CKEDITOR.dialog.add( 'shflash', this.path + 'dialogs/shflash.js' );
	}
}
); 