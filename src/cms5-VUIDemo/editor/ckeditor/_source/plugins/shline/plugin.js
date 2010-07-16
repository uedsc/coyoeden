/**
 * sohu empty line plugin
 * @author levinhuang
 */
CKEDITOR.plugins.add( 'shline',
{
	lang:['en','zh-cn'],
	init:function(editor){
		var lang=editor.lang.shline;
		//cmd
		editor.addCommand( 'shline', new CKEDITOR.dialogCommand( 'shline' ) );
		//ui on eidtor
		editor.ui.addButton( 'SHLine',
			{
				label : lang.toolbar,
				command : 'shline'
			});
		//dialog
		CKEDITOR.dialog.add( 'shline', this.path + 'dialogs/shline.js' );
	}
}
);