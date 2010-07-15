tinyMCEPopup.requireLangPack();

var diyTableDialog = {
	init : function() {
		tinyMCEPopup.resizeToInnerSize();
		$('#bordercolor_pickcontainer').html(getColorPickerHTML('bordercolor_pick','bordercolor'));
		$('#hdbgcolor_pickcontainer').html(getColorPickerHTML('hdbgcolor_pick','hdbgcolor'));
		
		/*table attribute data object*/
		var d={
			action:"insert",
			id:"",
			cols:2,rows:2,
			border:'1',bordercolor:"#000",
			bgcolor:"#fff",hdbgcolor:"#fff",
			align:""
		};
		/*editor references*/
		var f = document.forms[0], ed=tinyMCEPopup.editor, dom = ed.dom;
		var elm = dom.getParent(ed.selection.getNode(), "table");
		d.action = tinyMCEPopup.getWindowArg('action');

		if (!d.action)
			d.action = elm ? "update" : "insert";

		if (elm && d.action != "insert") {/*read current table's styles and attributes*/
			var rowsAr = elm.rows;
			var cols = 0;
			for (var i=0; i<rowsAr.length; i++)
				if (rowsAr[i].cells.length > cols)
					cols = rowsAr[i].cells.length;
	
			d.cols = cols;
			d.rows = rowsAr.length;
	
			d.st = dom.parseStyle(dom.getAttrib(elm, "style"));
			d.border = trimSize(getStyle(elm, 'border', 'borderWidth'));
			d.cellpadding = dom.getAttrib(elm, 'cellpadding', "");
			d.cellspacing = dom.getAttrib(elm, 'cellspacing', "");
			d.width = trimSize(getStyle(elm, 'width', 'width'));
			d.height = trimSize(getStyle(elm, 'height', 'height'));
			d.bordercolor = convertRGBToHex(getStyle(elm, 'bordercolor', 'borderLeftColor'));
			d.hdbgcolor=convertRGBToHex(getStyle($(elm).find("tr:first")[0],'bgcolor', 'backgroundColor'));
			d.bgcolor = convertRGBToHex(getStyle(elm, 'bgcolor', 'backgroundColor'));
			d.align = dom.getAttrib(elm, 'align', d.align);
			d.frame = dom.getAttrib(elm, 'frame');
			d.rules = dom.getAttrib(elm, 'rules');
			d.className = tinymce.trim(dom.getAttrib(elm, 'class').replace(/mceItem.+/g, ''));
			d.id = dom.getAttrib(elm, 'id');
			d.summary = dom.getAttrib(elm, 'summary');
			d.style = dom.serializeStyle(d.st);
			d.dir = dom.getAttrib(elm, 'dir');
			d.lang = dom.getAttrib(elm, 'lang');
			d.background = getStyle(elm, 'background', 'backgroundImage').replace(new RegExp("url\\(['\"]?([^'\"]*)['\"]?\\)", 'gi'), "$1");
			//f.caption.checked = elm.getElementsByTagName('caption').length > 0;
	
			d.orgTableWidth = d.width;
			d.orgTableHeight = d.height;
	
			d.action = "update";
			f.insert.value = ed.getLang('update');
		}
	
		//addClassesToList('class', "table_styles");
		TinyMCE_EditableSelects.init();
	
		// Update form
		/*
		selectByValue(f, 'align', d.align);
		selectByValue(f, 'tframe', d.frame);
		selectByValue(f, 'rules', d.rules);
		selectByValue(f, 'class', d.className, true, true);
		*/
		f.cols.value = d.cols;
		f.rows.value = d.rows;
		/*
		f.border.value = d.border;
		f.cellpadding.value = d.cellpadding;
		f.cellspacing.value = d.cellspacing;
		f.width.value = d.width;
		f.height.value = d.height;
		*/
		f.bordercolor.value = d.bordercolor;
		f.hdbgcolor.value=d.hdbgcolor;
		/*
		f.bgcolor.value = d.bgcolor;
		f.id.value = d.id;
		f.summary.value = d.summary;
		f.style.value = d.style;
		f.dir.value = d.dir;
		f.lang.value = d.lang;
		f.backgroundimage.value = d.background;
		*/
		updateColor('bordercolor_pick', 'bordercolor');
		updateColor('hdbgcolor_pick', 'hdbgcolor');
		
		// Disable some fields in update mode
		if (d.action == "update") {
			f.cols.disabled = true;
			f.rows.disabled = true;
		}
		
		diyTableDialog.d=d;
	},

	insert : function() {
		var formObj = document.forms[0];
		var inst = tinyMCEPopup.editor, dom = inst.dom;
		var d={cellpadding:"",cellspacing:"",style:""};
		$.extend(d,diyTableDialog.d);
		var html = '', capEl, elm;
		var cellLimit, rowLimit, colLimit;
	
		tinyMCEPopup.restoreSelection();
	
		if (!AutoValidator.validate(formObj)) {
			tinyMCEPopup.alert(inst.getLang('invalid_data'));
			return false;
		}
	
		elm = dom.getParent(inst.selection.getNode(), 'table');
	
		// Get form data
		d.cols = formObj.elements['cols'].value;
		d.rows = formObj.elements['rows'].value;
		/*
		d.border = formObj.elements['border'].value != "" ? formObj.elements['border'].value  : 0;
		d.cellpadding = formObj.elements['cellpadding'].value != "" ? formObj.elements['cellpadding'].value : "";
		d.cellspacing = formObj.elements['cellspacing'].value != "" ? formObj.elements['cellspacing'].value : "";
		d.align = getSelectValue(formObj, "align");
		d.frame = getSelectValue(formObj, "tframe");
		d.rules = getSelectValue(formObj, "rules");
		*/
		d.width = 300;//formObj.elements['width'].value;
		d.height = 100;//formObj.elements['height'].value;
		d.bordercolor = formObj.elements['bordercolor'].value;
		d.hdbgcolor=formObj.elements['hdbgcolor'].value;;
		/*
		d.bgcolor = formObj.elements['bgcolor'].value;
		d.className = getSelectValue(formObj, "class");
		d.id = formObj.elements['id'].value;
		d.summary = formObj.elements['summary'].value;
		d.style = formObj.elements['style'].value;
		d.dir = formObj.elements['dir'].value;
		d.lang = formObj.elements['lang'].value;
		d.background = formObj.elements['backgroundimage'].value;
		d.caption = formObj.elements['caption'].checked;
		*/
		cellLimit = tinyMCEPopup.getParam('table_cell_limit', false);
		rowLimit = tinyMCEPopup.getParam('table_row_limit', false);
		colLimit = tinyMCEPopup.getParam('table_col_limit', false);
	
		// Validate table size
		if (colLimit && d.cols > colLimit) {
			tinyMCEPopup.alert(inst.getLang('diyTable_dlg.col_limit').replace(/\{\$cols\}/g, colLimit));
			return false;
		} else if (rowLimit && d.rows > rowLimit) {
			tinyMCEPopup.alert(inst.getLang('diyTable_dlg.row_limit').replace(/\{\$rows\}/g, rowLimit));
			return false;
		} else if (cellLimit && d.cols * d.rows > cellLimit) {
			tinyMCEPopup.alert(inst.getLang('diyTable_dlg.cell_limit').replace(/\{\$cells\}/g, cellLimit));
			return false;
		}
	
		// Update table
		if (d.action == "update") {
			inst.execCommand('mceBeginUndoLevel');
			/*	
			dom.setAttrib(elm, 'cellPadding', d.cellpadding, true);
			dom.setAttrib(elm, 'cellSpacing', d.cellspacing, true);
			dom.setAttrib(elm, 'border', d.border);
			dom.setAttrib(elm, 'align', d.align);
			dom.setAttrib(elm, 'frame', d.frame);
			dom.setAttrib(elm, 'rules', d.rules);
			dom.setAttrib(elm, 'class', d.className);
			dom.setAttrib(elm, 'style', d.style);
			dom.setAttrib(elm, 'id', d.id);
			dom.setAttrib(elm, 'summary', d.summary);
			dom.setAttrib(elm, 'dir', d.dir);
			dom.setAttrib(elm, 'lang', d.lang);
			*/
			capEl = inst.dom.select('caption', elm)[0];
	
			if (capEl && !d.caption)
				capEl.parentNode.removeChild(capEl);
	
			if (!capEl && d.caption) {
				capEl = elm.ownerDocument.createElement('caption');
	
				if (!tinymce.isIE)
					capEl.innerHTML = '<br _mce_bogus="1"/>';
	
				elm.insertBefore(capEl, elm.firstChild);
			}
	
			if (d.width && inst.settings.inline_styles) {
				dom.setStyle(elm, 'width', d.width);
				dom.setAttrib(elm, 'width', '');
			} else {
				dom.setAttrib(elm, 'width', d.width, true);
				dom.setStyle(elm, 'width', '');
			}
	
			// Remove these since they are not valid XHTML
			dom.setAttrib(elm, 'borderColor', '');
			dom.setAttrib(elm, 'bgColor', '');
			dom.setAttrib(elm, 'background', '');
	
			if (d.height && inst.settings.inline_styles) {
				dom.setStyle(elm, 'height', d.height);
				dom.setAttrib(elm, 'height', '');
			} else {
				dom.setAttrib(elm, 'height', d.height, true);
				dom.setStyle(elm, 'height', '');
	 		}
	
			if (d.background != '')
				elm.style.backgroundImage = "url('" + d.background + "')";
			else
				elm.style.backgroundImage = '';
	
	/*		if (tinyMCEPopup.getParam("inline_styles")) {
				if (width != '')
					elm.style.width = getCSSSize(width);
			}*/
	
			if (d.bordercolor != "") {
				elm.style.borderColor = d.bordercolor;
				elm.style.borderStyle = elm.style.borderStyle == "" ? "solid" : elm.style.borderStyle;
				elm.style.borderWidth = d.border == "" ? "1px" : d.border;
			} else
				elm.style.borderColor = '';
	
			if(d.hdbgcolor!=""){
				$(elm).find("tr:first").css("background-color",d.hdbgcolor);
			}
	
			elm.style.backgroundColor = d.bgcolor||"#fff";
			elm.style.height = getCSSSize(d.height+"");
	
			inst.addVisual();
	
			// Fix for stange MSIE align bug
			//elm.outerHTML = elm.outerHTML;
	
			inst.nodeChanged();
			inst.execCommand('mceEndUndoLevel');
	
			// Repaint if dimensions changed
			/*
			if (formObj.width.value != orgTableWidth || formObj.height.value != orgTableHeight)
				inst.execCommand('mceRepaint');
			*/
			tinyMCEPopup.close();
			return true;
		}
	
		// Create new table
		html += '<table';
	
		html += this.makeAttrib('id', d.id);
		html += this.makeAttrib('border', d.border);
		html += this.makeAttrib('cellpadding', d.cellpadding);
		html += this.makeAttrib('cellspacing', d.cellspacing);
		html += this.makeAttrib('_mce_new', '1');
	
		if (d.width && inst.settings.inline_styles) {
			if (d.style)
				d.style += '; ';
	
			// Force px
			if (/^[0-9\.]+$/.test(d.width))
				d.width += 'px';
	
			d.style += 'width: ' + d.width;
		} else
			html += this.makeAttrib('width', d.width);
	
		if(d.bordercolor&&inst.settings.inline_styles){
			d.style+=";border-color:"+d.bordercolor;
		}else{
			html += this.makeAttrib('bordercolor', d.bordercolor);
		}
	
	/*	if (height) {
			if (style)
				style += '; ';
	
			style += 'height: ' + height;
		}*/
	
		//html += makeAttrib('height', height);
		//html += makeAttrib('bgcolor', bgcolor);
		/*
		html += makeAttrib('align', d.align);
		html += makeAttrib('frame', d.frame);
		html += makeAttrib('rules', d.rules);
		html += makeAttrib('class', d.className);
		*/
		html += this.makeAttrib('style', d.style);
		/*
		html += makeAttrib('summary', d.summary);
		html += makeAttrib('dir', d.dir);
		html += makeAttrib('lang', d.lang);
		*/
		html += '>';
	
		if (d.caption) {
			if (!tinymce.isIE)
				html += '<caption><br _mce_bogus="1"/></caption>';
			else
				html += '<caption></caption>';
		}
	
		for (var y=0; y<d.rows; y++) {
			if(d.hdbgcolor&&y==0){
				html+='<tr style="background-color:'+d.hdbgcolor+'">';
			}else{
				html += "<tr>";	
			}
			
			for (var x=0; x<d.cols; x++) {
				if (!tinymce.isIE)
					html += '<td><br _mce_bogus="1"/></td>';
				else
					html += '<td></td>';
			}
	
			html += "</tr>";
		}
	
		html += "</table>";
	
		inst.execCommand('mceBeginUndoLevel');
	
		// Move table
		if (inst.settings.fix_table_elements) {
			var patt = '';
	
			inst.focus();
			inst.selection.setContent('<br class="_mce_marker" />');
	
			tinymce.each('h1,h2,h3,h4,h5,h6,p'.split(','), function(n) {
				if (patt)
					patt += ',';
	
				patt += n + ' ._mce_marker';
			});
	
			tinymce.each(inst.dom.select(patt), function(n) {
				inst.dom.split(inst.dom.getParent(n, 'h1,h2,h3,h4,h5,h6,p'), n);
			});
	
			dom.setOuterHTML(dom.select('br._mce_marker')[0], html);
		} else
			inst.execCommand('mceInsertContent', false, html);
	
		tinymce.each(dom.select('table[_mce_new]'), function(node) {
			var td = dom.select('td', node);
	
			inst.selection.select(td[0], true);
			inst.selection.collapse();
	
			dom.setAttrib(node, '_mce_new', '');
		});
	
		inst.addVisual();
		inst.execCommand('mceEndUndoLevel');
	
		tinyMCEPopup.close();
	},
	changedColor:function(){
		var f = document.forms[0],ed=tinyMCEPopup.editor, dom = ed.dom;
		var st = dom.parseStyle(f.style.value);
	
		st['background-color'] = f.bgcolor.value;
	
		if (f.bordercolor.value != "") {
			st['border-color'] = f.bordercolor.value;
	
			// Add border-width if it's missing
			if (!st['border-width'])
				st['border-width'] = "1px";
		}
	
		f.style.value = dom.serializeStyle(st);
	},
	makeAttrib:function(attrib, value) {
		var formObj = document.forms[0];
		var valueElm = formObj.elements[attrib];
	
		if (typeof(value) == "undefined" || value == null) {
			value = "";
	
			if (valueElm)
				value = valueElm.value;
		}
	
		if (value == "")
			return "";
	
		// XML encode it
		value = value.replace(/&/g, '&amp;');
		value = value.replace(/\"/g, '&quot;');
		value = value.replace(/</g, '&lt;');
		value = value.replace(/>/g, '&gt;');
	
		return ' ' + attrib + '="' + value + '"';
	}
};

tinyMCEPopup.onInit.add(diyTableDialog.init, diyTableDialog);
