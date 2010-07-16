/**
 * text-image plugin for sohu.com
 * @author levinhuang
 */
CKEDITOR.plugins.add( 'shimage',
{
	requires: [ 'iframedialog' ],
	lang:['zh-cn'],
	init:function(editor){
        var lang= editor.lang.shimage;
		//cmd
		editor.addCommand( 'shimage', new CKEDITOR.dialogCommand( 'shimage' ) );
		//ui on eidtor
		editor.ui.addButton( 'SHImage',
			{
				label : lang.toolbar,
				command : 'shimage'
			});
		//dialog
		CKEDITOR.dialog.add( 'shimage', this.path + 'dialogs/shimage.js' );
	}
}
); 