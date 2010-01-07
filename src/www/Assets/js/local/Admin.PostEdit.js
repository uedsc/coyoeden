///<reference path="../jquery/jquery-1.3.2.js"/>
///<reference path="../Vivasky.StringUtils.js"/>
///<reference path="../Vivasky.com.js"/>
///<reference path="../jquery/jquery.blockUI.js"/>
///<reference path="../jquery/jquery.tools.min.js"/>
///<reference path="../jquery/jquery.jgrowl.js"/>
///<reference path="../jquery/jquery.json-1.3.min.js"/>
///<reference path="../jquery/jquery.dateentry.min.js"/>
///<reference path="Local.Common.js"/>
///<reference path="../date.js"/>
var PostEditApp = function() {
	//private area
	var p = {};
	p.applySlug = function(arg, context) { p.txtSlug.val(arg); };
	p.getSlug = function() { WebForm_DoCallback('__Page', p.txtTitle.val(), ApplySlug, 'slug', null, false); return false; };
	p.autoSave = function() {
		var content = p.txtRawContent.size() > 0 ? p.txtRawContent.val() : tinyMCE.activeEditor.getContent();
		var data = { content: content, title: p.txtTitle.val(), desc: p.txtDescription.val(), slug: p.txtSlug.val(), tags: p.txtTags.val() };

		if (content.length > 10) {
			WebForm_DoCallback('__Page', '_autosave' + $.toJSON(data), null, 'autosave', null, false);
		};

		setTimeout(p.autoSave, 5000);
	};
	p.onKeypressBody = function(evt) {
		if (evt.which == 27) { p.tagselector.hide(); };
	};
	//public area
	var pub = {};
	pub.Init = function(opts) {
		p.ulDrafts = $("#" + opts.ulDraftsID);
		p.txtTitle = $("#" + opts.txtTitleID);
		p.txtSlug = $("#" + opts.txtSlugID);
		p.txtRawContent = $("#" + opts.txtRawContentID);
		p.txtDescription = $("#" + opts.txtDescriptionID);
		p.txtTags = $("#" + opts.txtTagsID);
		p.tagselector = $("#" + opts.tagselectorID);
		p.aDrafts = $("#" + opts.aDraftsID);
		p.lnkGetSlug = $("#" + opts.lnkGetSlugID);
		p.postID = opts.id;
		//event registers
		p.aDrafts.click(function() { p.ulDrafts.toggle(); return false; });
		p.lnkGetSlug.click(p.getSlug);
		if (p.postID && p.postID.length > 0 && p.postID != "null") { p.autoSave(); };
		$("#lnkShowTags").click(function() { p.tagselector.toggle(); return false; });
		$("#lnkTagsCloser").click(function() { p.tagselector.hide(); return false; });
		$("body").keypress(p.onKeypressBody);
	};
	pub.AddTag=function(obj) {
		p.txtTags[0].value += obj.innerHTML + ', ';
	}
	return pub;
} ();