CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.autoLanguage = false;
    // config.defaultLanguage = 'pt-br';
    // config.contentsLangDirection = 'rtl'
    //config.theme = 'default';
    
	config.toolbar_CKFull =
	[
		['Source','-','Save','NewPage','Preview','-','Templates'],
		['Cut','Copy','Paste','PasteText','PasteFromWord','-','Print', 'SpellChecker', 'Scayt'],
		['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],
		['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'],
		'/',
		['Bold','Italic','Underline','Strike','-','Subscript','Superscript'],
		['NumberedList','BulletedList','-','Outdent','Indent','Blockquote'],
		['JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],
		['Link','Unlink','Anchor'],
		['Image','Flash','Table','HorizontalRule','Smiley','SpecialChar','PageBreak'],
		'/',
		['Styles','Format','Font','FontSize'],
		['TextColor','BGColor'],
		['Maximize', 'ShowBlocks','-','About']
	];

	
	config.toolbar_Full =
    [
		['Source'],
		['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteWord', '-', 'Print', 'SpellChecker', 'Scayt'],
		['Undo','Redo','-','Find','Replace'],
		'/',
		['Blockquote','Bold','Italic','Underline','Strike','-','Subscript','Superscript'],
		['NumberedList','BulletedList','-','Outdent','Indent'],
		['JustifyLeft','JustifyCenter','JustifyRight','JustifyFull'],
		['Link','Unlink','Anchor'],
		['Image','Flash','Table','HorizontalRule','Smiley','SpecialChar'],
		'/',
		['Styles'],
		['Maximize']

    ];

	config.toolbar_Newsletter =
    [
		['Source'],
		['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteWord', '-', 'Print', 'SpellChecker', 'Scayt'],
		['Undo', 'Redo', '-', 'Find', 'Replace'],
		'/',
		['Blockquote', 'Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
		['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
		['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyFull'],
		['Link', 'Unlink', 'Anchor'],
		['Image', 'Table', 'HorizontalRule', 'SpecialChar'],
		'/',
		['Format', 'Font', 'FontSize'],
		['TextColor', 'BGColor'],
		['Maximize','Preview']

    ];
	
	config.toolbar_FullWithTemplates =
    [
		['Source'],
		['SelectAll', 'RemoveFormat', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteWord', '-', 'Print', 'SpellChecker', 'Scayt'],
		['Undo','Redo','-','Find','Replace'],
		'/',
		['Blockquote','Bold','Italic','Underline','Strike','-','Subscript','Superscript'],
		['NumberedList','BulletedList','-','Outdent','Indent'],
		['JustifyLeft','JustifyCenter','JustifyRight','JustifyFull'],
		['Link','Unlink','Anchor'],
		['Image','Flash','Table','HorizontalRule','Smiley','SpecialChar'],
		'/',
		['Templates','Styles'],
		['Maximize']
	
    ];
	
	config.toolbar_Group =
    [
		['Cut','Copy','PasteText','-'],
		['Undo', 'Redo', '-', 'Find', 'Replace', 'SpellChecker', 'Scayt', '-', 'SelectAll', 'RemoveFormat'],
		['Blockquote','Bold','Italic','Underline'],
		['NumberedList', 'BulletedList'],
		['Link','Unlink'],
		['SpecialChar','Smiley']
	];
	
	config.toolbar_GroupWithImages =
    [
		['Cut','Copy','PasteText','-'],
		['Undo','Redo','-','Find','Replace','SpellChecker', 'Scayt','-','SelectAll','RemoveFormat'],
		['Blockquote','Bold','Italic','Underline','Image'],
		['NumberedList', 'BulletedList'],
		['Link','Unlink'],
		['SpecialChar','Smiley']
	];
	
	config.toolbar_SimpleWithSource =
    [
		['Source','Cut','Copy','PasteText'],
		['Blockquote', 'Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', 'SpellChecker', 'Scayt', 'Smiley']
	];
	
	config.toolbar_AnonymousUser =
    [
		['Cut','Copy','PasteText'],
		['Blockquote', 'Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', 'Smiley']
	];
	
};
