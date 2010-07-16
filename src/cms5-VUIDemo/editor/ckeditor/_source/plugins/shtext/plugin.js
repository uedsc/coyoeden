/**
 * text plugin for sohu.com
 * @author levinhuang
 */
CKEDITOR.plugins.add( 'shtext',
{
	requires: [ 'iframedialog' ],
	lang:['zh-cn'],
	init:function(editor){
        var lang= editor.lang.shtext;
		//cmd
		editor.addCommand( 'shtext', new CKEDITOR.dialogCommand( 'shtext' ) );
		//ui on eidtor
		editor.ui.addButton( 'SHText',
			{
				label : lang.toolbar,
				command : 'shtext'
			});
		//dialog
		CKEDITOR.dialog.add( 'shtext', this.path + 'dialogs/shtext.js' );
	}
}
); 