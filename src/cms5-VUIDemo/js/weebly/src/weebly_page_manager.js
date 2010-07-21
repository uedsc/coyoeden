
/*
 *
 * PageManager serves 2 purposes:
 *   - an improved js api for accessing information about a site's pages
 *   - rendering for "Manage Pages" interface
 *
 * To be backwards-compatible w/ other code, PageManager always keeps the
 * the globals 'sitePages' and 'pageOrder' up to date
 *
 * Author: Adam Shaw
 *
 */


Weebly.PageManager = {



	pages: {},			// hash of page_id's to page objects
	topLevelPages: [],  // only the top level, ordered
	
	nestedSortable: null,
	
	currentPageId: null,
	currentPageInDB: false,
		// used when a new page is created, and is reordered before the DB knows about it
		// when reordering stops, if this flag is true, save the page object also
	
	cancelDelayedSave: false,
		// cancel the 'save' the ensues when a text field is unfocused
		
	pageLimit: 0, // 0 means no limit
	
	fakeID: 0,
	
	
	
	//
	// initialize 'pages' and 'topLevelPages' and sync to 'pageOrder' and 'sitePages'
	// then, start the rendering of the first page
	//
	
	init: function() {
		Weebly.Cache.get('', 'initialRight', function(topLevel) {
		
			Weebly.PageManager.topLevelPages = topLevel;
			Weebly.PageManager.pages = Weebly.PageManager.getPageIdHash();
			
			var limit = Weebly.Restrictions.accessValue('page_limit');
			if (!Weebly.Restrictions.hasService(Weebly.Restrictions.proLevel) && typeof limit !== true) { // according to js API, a value of true means there is no limit
				Weebly.PageManager.pageLimit = parseInt(limit);
			}
			
			pageOrder = [];
			for (var i=0; i<topLevel.length; i++) {
				var p = topLevel[i];
				pageOrder.push(p.id);
			}
			
			sitePages = {};
			for (var pid in Weebly.PageManager.pages) {
				sitePages[pid] = Weebly.PageManager.pages[pid].title;
			}
			
			writeTheme(currentTheme, 1); // start the rendering of theme
				// this probably shouldn't be a responsibility of PageManager, but oh well
			
		});
	},



	//
	// initialize the Manage Pages screen w/ info from current pages, then show the interface
	//

	buildUI: function(newPage) {
	
		$('savePageSettings').innerHTML =
			"<div style='height: 50px; margin-top: 10px; width: 320px;'>" +
				"<a href='#' style='float: left;' onmousedown='Weebly.PageManager.cancelDelayedSave=true' onclick=\"Weebly.PageManager.save(true);showEvent('updatePages', 0, null, 300);return false;\"><img src='"+/*tli(*/"/weebly/images/savebtn-gray.jpg"/*)tli*/+"' border='0'/></a>" +
			"</div>";
		
		var pageListHTML =
			'<div id="pagelist-top"></div>' +
			'<div id="pagelist-middle">' +
				'<div id="instructions">' + /*tl(*/'Drag to reorder.'/*)tl*/ + '</div>' +
				'<div id="pages-wrap">' +
				'<ul class="sortabledemo" id="pages">';
			
		pageListHTML += Weebly.PageManager.buildHierarchyHTML();
		pageListHTML += '</ul></div>';
		var limit = Weebly.PageManager.pageLimit;
		if (limit) {
			pageListHTML +=
				"<div class='limit-notice'>(" +
				/*tl(*/"Only your first"/*)tl*/ + " " +
				(limit==1 ? /*tl(*/"page"/*)tl*/ : limit + " " + /*tl(*/"pages"/*)tl*/) +
				" " + /*tl(*/"will be published."/*)tl*/ +
                " " + /*tl(*/"To add more, upgrade to <a style='display:inline;' href='#' onclick='openBillingPage(\"Please upgrade to remove the page limit\", Weebly.Restrictions.proLevel, \"page_limit\"); return false;'>Premium</a>"/*)tl*/ +
				")</div>";
		}
		pageListHTML += '</div><div id="pagelist-bottom"></div>';
		$('pageListArea').innerHTML = pageListHTML;

                Weebly.PageManager.updateHomeIcon();
                Weebly.PageManager.updateLockIcons();
                Weebly.PageManager.updateIconPosition();
		
		$('pagesContainer').show();

		Weebly.PageManager.nestedSortable = NestedSortable.create('pages', {
			scroll: 'pages-wrap',
			handleSelector: 'div.pageBuffer',
			allowIndent: ENABLE_PAGE_HIERARCHY,
			constraint: ENABLE_PAGE_HIERARCHY ? null : 'vertical',
			onStart: function() {
				Weebly.PageManager.cancelDelayedSave = true;
				$(this).select('div.pagesBuffer')[0].style.cursor = 'move';
			},
			onChange: function() {
				Weebly.PageManager.save(false, true);
				Weebly.PageManager.updateHierarchyLimit();
                                Weebly.PageManager.updateHomeIcon();
                        },
                        onPop: function() {
                                Weebly.PageManager.updateHomeIcon();
			},
			onEnd: function() {
				$(this).select('div.pagesBuffer')[0].style.cursor = '';
			}
		});
		
		if (newPage) {
			Weebly.PageManager.addNewPage(); // will call updateHierarchyLimit
		}else{
			Weebly.PageManager.updateHierarchyLimit();
			Weebly.PageManager.displayPage(pageOrder[0]); // display first page
		}
		
		$('pagesContainer').scrollTop = 0;
		$('pages-wrap').scrollTop = 0;
		
		if (Weebly.PageManager.topLevelPages.length > 2) {
			showEvent('secondPage', 0, $('pages_'+Weebly.PageManager.topLevelPages[2].id));
		}
		
	},
	
	
	
	
	//
	// generate markup for the sortable list of pages
	//
	
	buildHierarchyHTML: function() {
		return Weebly.PageManager._buildHierarchyHTML(Weebly.PageManager.topLevelPages);
	},
	
	_buildHierarchyHTML: function(pgs) {
		var h = '';
		for (var i=0; i<pgs.length; i++) {
			var pageId = pgs[i].id.replace(/\n/, ''); // dont know why we do replace
			h +=
				"<li class='highlightbox' id='pages_"+pageId+"' title='"+/*tl(*/"Drag page to re-order"/*)tl*/+"' />" +
					"<div class='tick'></div>" +
					"<div class='pagesBuffer' onclick='Weebly.PageManager.displayPage(\""+pageId+"\")'><div style='position: relative;'>" +
                                                "<div class='pagesDragHandle'></div>" +
						"<span id='"+pageId+"title'>"+pgs[i].title.replace(/(.{20}).+/, "$1...")+"</span>" +
						"<span id='"+pageId+"hidden'>" + (pgs[i].hidden ? "&nbsp;(Hidden)" : '') + "</span>" +
                                                "<div class='iconContainer'>" + 
                                                        "<div class='lockIcon' title='" + /*tl(*/"This page is password protected"/*)tl*/ +"'></div>" +
                                                        "<div class='homeIcon' title='" + /*tl(*/"This is the homepage"/*)tl*/ + "'></div>" +
                                                "</div>" +
					"</div></div>" +
					(pgs[i].children.length > 0 ?
						"<ul>" + Weebly.PageManager._buildHierarchyHTML(pgs[i].children) + "</ul>" :
						'') +
				"</li>";
		}
		return h;
	},
	
	
	//
	// code to update the page icons
	//
	
	updateHomeIcon: function() {
                var x = 0;
                var parentID = '';
                $$('#pages .highlightbox').each(function(el) {

                        if (x == 0) {
                                parentID = el.id;
                                el.down(".homeIcon").style.display = 'block';
                        } else if (el.id == parentID) {
                                el.down(".homeIcon").style.display = 'block';
                        } else {
                                el.down(".homeIcon").style.display = 'none';
                        }
                        x++;
                });
	},
	
        updateIconPosition: function() {
	
                var x = 0;
                for(thisPage in Weebly.PageManager.pages) { 
                        x++;
                }

                for(thisPage in Weebly.PageManager.pages) { 
                        if (x > 9) { $('pages_'+thisPage).down(".iconContainer").addClassName("iconContainerScroll"); }
                        else { $('pages_'+thisPage).down(".iconContainer").removeClassName("iconContainerScroll"); }
                }

        },

	updateLockIcons: function() {
                for(thisPage in Weebly.PageManager.pages) { 
                        Weebly.PageManager.updateLockIcon(thisPage, Weebly.PageManager.pages[thisPage].pwprotected);
                }
        },

	updateLockIcon: function(page, pwprotected) {
                $("pages_"+page).down(".lockIcon").style.display = (pwprotected ? "block" : "none");
	},
	
	
	
	//
	// code for graying out the pages that are over the page # limit
	//
	
	updateHierarchyLimit: function() {
		// when pageLimit is set to 0, the hierarchy will NEVER be updated
		if (Weebly.PageManager.pageLimit) {
			Weebly.PageManager._updateHierarchyLimit($('pages'), 0, Weebly.PageManager.pageLimit);
		}
	},
	
	_updateHierarchyLimit: function(ul, cnt, limit) {
		ul.childElements().each(function(li) {
			cnt++;
			if (cnt > limit) {
				li.addClassName('overlimit');
			}else{
				li.removeClassName('overlimit');
			}
			var uls = $$('#' + li.id + ' > ul');
			if (uls.length) {
				cnt = Weebly.PageManager._updateHierarchyLimit(uls[0], cnt, limit);
			}
		});
		return cnt;
	},
	
	
	
	
	
	//
	// starts editing the properties of a given page. fills #pageSettings with markup
	//
	
	displayPage: function(pageId, notInDB) {
	
		if (pageId == Weebly.PageManager.currentPageId) {
			return;
		}
	
		if (Weebly.PageManager.currentPageId) {
			// save the previous page that was being edited
			Weebly.PageManager.save();
		}
	
		Weebly.PageManager.currentPageInDB = !notInDB;
		Weebly.PageManager.cancelDelayedSave = false;
			
		// update current page in list
		$$('#pages li').each(function(el) {
			el.removeClassName('highlightbox-active');
		});
		var activeLI = $('pages_'+pageId);
		if (activeLI) {
			activeLI.addClassName('highlightbox-active');
		}
		
		var page = Weebly.PageManager.pages[pageId];
		var hidden = page.hidden;
		
		var pageSettingsHTML =
                        "<div style='padding-left: 50px; width: 400px;'>" +
                                "<span style='font-weight: bold; font-size: 13px; color: #333333;'>" + /*tl(*/"Page Name"/*)tl*/ + "</span>" + 
                                "<div style='padding: 2px 0 20px 0;'>" +
				        "<div id='pageTitleEdit'><input id=\"page_name\" class=\"input\" style=\"width: 325px; padding: 5px;\" name=\"page_name\" value=\""+page.title.gsub('"',"&quot;")+"\" onblur=\"Weebly.PageManager.implicitSave(true);\"type=\"text\"></div>" +
					"<a id='edit_link_a' href=\"#\" class='button-blue button-left' onclick=\"Weebly.PageManager.editThisPage();\" style='margin: 5px 15px 0 0;'><span class='button-blue button-right' style='font-weight: bold;'>" + /*tl(*/"Edit Page"/*)tl*/ + "</span></a>";

                if (!page.blog) {
		        pageSettingsHTML += "<a id='copy_link_a' href=\"#\" class='button-grey button-left' onclick=\"Weebly.PageManager.copyPage();\" style='margin: 5px 15px 0 0;'><span class='button-grey button-right' style='font-weight: bold;'>" + /*tl(*/"Copy Page"/*)tl*/ + "</span></a>"; 
                }

		if ($H(Weebly.PageManager.pages).keys().length > 1) {
			pageSettingsHTML +=
				"<a onclick='Effect.SlideDown(\"confirmDeletePage\"); return false;' onmousedown='Weebly.PageManager.cancelDelayedSave=true' href=\"#\" class='button-grey button-left' style='margin: 5px 15px 0 0;'><span class='button-grey button-right' style='font-weight: bold;'>" + /*tl(*/"Delete Page"/*)tl*/ + "</span></a>" +
				"<div style='margin: 5px 0 0 0; border: 1px solid #ccc; background: #FFFFCC; width:270px; display: none;' id='confirmDeletePage'>" +
					"<div id='promptUser' style='padding: 10px'>"+
						/*tl(*/"Are you sure you want to delete this page? This action is permanent."/*)tl*/+" "+
						"<span style='display: block; padding-top: 10px;'>" +
							"<a href='#' onClick='Weebly.PageManager.deleteCurrentPage(); return false;' style='color: red; margin-right: 15px;'>" +
								"<img src='/weebly/images/page_cross.gif' style='border: 0;' /><font style='position: relative; top:-3px; margin-left: 2px;'>"+/*tl(*/"Yes, delete page"/*)tl*/+"</font>" +
							"</a>" +
							"<a href='#' onClick='Effect.SlideUp(\"confirmDeletePage\"); return false;'>" +
								"<img src='/weebly/images/page_next.gif' style='border: 0;' />" +
								"<font style='position: relative; top:-3px; margin-left: 2px;'>"+/*tl(*/"No, keep page"/*)tl*/+"</font>" +
							"</a>" +
						"</span>" +
					"</div>" +
				"</div>";
		}
		
                pageSettingsHTML += "</div>" +
				"<span style='font-weight: bold; font-size: 13px; color: #333333;'>" + /*tl(*/"Show in Navigation Menu?"/*)tl*/ + "</span>" +
                                "<div style='padding: 2px 0 20px 0;'>" +
					"<select class=\"input\" style='border: 1px solid #ccc; vertical-align:middle' onchange=\"Weebly.PageManager.implicitSave();\" id=\"page_hidden\">" +
						"<option value=\"0\" " + (!hidden ? "selected" : "") + ">" + /*tl(*/"Yes"/*)tl*/ +
						"<option value=\"1\" " + (hidden ? "selected" : "") + ">" + /*tl(*/"No"/*)tl*/ +
					"</select>" +
                                "</div>";

                if (Weebly.Restrictions.accessLevel('editable_site_password') != "none") {

                  var proIconSrc = typeof(proElementOverlaySrc) != 'undefined' ? proElementOverlaySrc : 'images/pro-element-overlay.png';
                  pageSettingsHTML +=  "<span style='font-weight: bold; font-size: 13px; color: #333333;'>" + /*tl(*/"Password Protect?"/*)tl*/ + "</span>";
					
		  if (Weebly.Restrictions.hasAccess('editable_site_password')) {
			pageSettingsHTML += Weebly.Restrictions.isProElement('editable_site_password') ? "<img src='"+proIconSrc+"' border='0'/>" : "";
		  }else {
			pageSettingsHTML += "<a href='#' onclick='alertProFeatures(\""+/*tl(*/"Please sign-up for a pro account to use password protected pages"/*)tl*/+"\", \"pagesMenu\"); return false;'><img src='"+proIconSrc+"' border='0'/></a>";
		  }
			
		  pageSettingsHTML += "<div style='padding: 2px 0 20px 0;'>";
		
		  if (Weebly.Restrictions.hasAccess('editable_site_password')) {
		
			pageSettingsHTML +=
				"<input type='checkbox' name='page_protected' id='page_protected' value='1' onclick=\"Weebly.PageManager.implicitSave();\" " +
				(page.pwprotected ? "checked" : "") +  " />";
				
			if (newSitePassword == "") {
				pageSettingsHTML +=
					"<br/><i>"+/*tl(*/"This option will work once you set a site-wide password in <br/>the Settings tab."/*)tl*/+"</i>";
			}
			
		  }else{
		
			pageSettingsHTML +=
				"<input type='hidden' name='page_protected' id='page_protected' value='0'/><input type='checkbox' id='mycheck' onclick='alertProFeatures(\""+/*tl(*/"Please sign-up for a pro account to use password protected pages"/*)tl*/+"\", \"pagesMenu\"); $(\"mycheck\").checked = 0; return false;'/>";
		
		  }
		
		  pageSettingsHTML += "</div>";

                }
		
                pageSettingsHTML += "<div style='padding: 2px 0 20px 0;'>";
			
                pageSettingsHTML += "<a href='#open' onclick='Effect.toggle(\"advancedPageSettings\", \"slide\"); if ($(\"pageSettingsAdvancedImg\").src.match(/arrow_right/)) { $(\"pageSettingsAdvancedImg\").src = \"/weebly/images/arrow_down.gif\"; } else { $(\"pageSettingsAdvancedImg\").src = \"/weebly/images/arrow_right.gif\"; } return false;' style='color: #0066CC; text-decoration: none; font-weight: bold;'>"+/*tl(*/"Advanced Settings"/*)tl*/+" <img id='pageSettingsAdvancedImg' src='/weebly/images/arrow_right.gif' style='border: none; position: relative; top: 1px; left: 2px;'/></a>" +
				"<div style='width:325px; display: none;' id='advancedPageSettings'>" +
                                        "<div>" +
                                                "<div style='font-weight: bold; font-size: 13px; color: #333333; margin: 10px 0 0 0;'>"+/*tl(*/"Page Description"/*)tl*/+"</div>" +
                                                "<div style='padding: 2px 0 5px 0;'>" +
                                                        "<input type='text' id='page_description' style='border: 1px solid #CCCCCC; width: 100%;' value=\""+page.description.replace(/"/g, "&quot;")+"\" onblur=\"Weebly.PageManager.implicitSave(true);\"/>" +
                                                "</div>" +
                                                "<div style='font-weight: bold; font-size: 13px; color: #333333; margin: 10px 0 0 0;'>"+/*tl(*/"Meta Keywords"/*)tl*/+" <span style='font-size: 11px;'>(separate each with a comma)"/*)tl*/+"</span></div>" +
                                                "<div style='padding: 2px 0 5px 0;'>" +
                                                        "<input type='text' id='page_keywords' style='border: 1px solid #CCCCCC; width: 100%;' value=\""+page.keywords.replace(/"/g, "&quot;")+"\" onblur=\"Weebly.PageManager.implicitSave(true);\"/>" +
                                                "</div>" +
                                                "<div style='font-weight: bold; font-size: 13px; color: #333333; margin: 10px 0 0 0;'>"+/*tl(*/"Footer Code"/*)tl*/+"</div>" +
                                                "<div style='padding: 2px 0 5px 0;'>" +
                                                        "<textarea id='page_footer' autocomplete='off' style='border: 1px solid #CCCCCC; width: 100%;' onblur=\"Weebly.PageManager.implicitSave(true);\">"+page.footer.replace(/\</g, "&lt;")+"</textarea>" +
                                                "</div>" +
                                                "<div style='font-weight: bold; font-size: 13px; color: #333333; margin: 10px 0 0 0;'>"+/*tl(*/"Header Code"/*)tl*/+"</div>" +
                                                "<div style='padding: 2px 0 5px 0;'>" +
                                                        "<textarea id='page_header' autocomplete='off' style='border: 1px solid #CCCCCC; width: 100%;' onblur=\"Weebly.PageManager.implicitSave(true);\">"+page.header.replace(/\</g, "&lt;")+"</textarea>" +
                                                "</div>" +
                                        "</div>" +
				"</div>";

		pageSettingsHTML += "</div>";
		pageSettingsHTML += "</div>";
		$('pageSettings').innerHTML = pageSettingsHTML;
		
		this.currentPageId = pageId;
		
		setTimeout(function() {
			var textInput = $('page_name');
			if (textInput) { // if clicking fast, sometimes current page has already switched
				textInput.select();
			}
		}, 50); // DOM usually isn't ready, so add delay
		
	},
	
	
	
	

	//
	// saves a page's properties to the server. also sends information on hierarchy
	// updates the global variables 'pageOrder' and 'sitePages'
	//
	
	save: function(exit, forceSave, callback) {
		
		var pid = Weebly.PageManager.currentPageId;
		var p = Weebly.PageManager.pages[pid];
		var oldp = Object.clone(p);
		var oldPageOrder = pageOrder;
		
		Weebly.PageManager.updateHierarchy(); // updates pageOrder
		
		p.title = sitePages[pid] = $F('page_name').strip();
		p.hidden = parseInt($F('page_hidden'));
		p.pwprotected = $('page_protected') ? ($F('page_protected') ? parseInt($F('page_protected')) : 0) : 0;
		p.header = $('page_header') ? ($F('page_header') != "null" ? $F('page_header') : '') : '';
		p.footer = $('page_footer') ? ($F('page_footer') != "null" ? $F('page_footer') : '') : '';
		p.keywords = $('page_keywords') ? ($F('page_keywords') != "null" ? $F('page_keywords') : '') : '';
		p.description = $('page_description') ? ($F('page_description') != "null" ? $F('page_description') : '') : '';
		
		Weebly.PageManager.updateCurrentPageUI(pid, p.title, p.hidden);
		
		if (forceSave || !Weebly.PageManager.pageSettingsEqual(oldp, p)) {
		
			Weebly.PageManager.lock();
			new Ajax.Request(ajax, {
				parameters: {
					pos: 'right',
					site_id: currentSite,
					cookie: document.cookie,
					pageid: pid.indexOf('new')==0 ? '' : pid,
					newtitle: p.title,
					hidden: p.hidden,
					pwprotected: p.pwprotected,
					header: p.header,
					footer: p.footer,
					keywords: p.keywords,
					description: p.description,
					blog: p.blog ? 1 : 0,
					hierarchy: Weebly.PageManager.getAbbreviatedHierarchy().toJSON()
				},
				onSuccess: function(transport) {
					Weebly.PageManager.unlock();
					var res = transport.responseText.evalJSON(),
						newid = res.id,
						newtitle = res.title;
					p.title = newtitle;
					if (pid != newid) {
						p.id = newid;
						Weebly.PageManager.pages[newid] = p;
						delete Weebly.PageManager.pages[pid];
						$('pages_' + pid).id = 'pages_' + newid;
						var pb = $$('#pages_' + newid + ' div.pagesBuffer')[0];
						pb.removeAttribute('onclick');
						pb.onclick = function() {
							Weebly.PageManager.displayPage(newid);
						};
						$(pid + 'title').id = newid + 'title';
						$(pid + 'hidden').id = newid + 'hidden';
						if (Weebly.PageManager.currentPageId == pid) {
							Weebly.PageManager.currentPageInDB = true;
							Weebly.PageManager.currentPageId = newid;
							$('edit_link_a').removeAttribute('onclick');
							$('edit_link_a').onclick = function() {
								goUpdateList(newid, 1);
							}
						}
						pageOrder[pageOrder.indexOf(pid)] = newid;
						sitePages[newid] = newtitle;
						delete sitePages[pid];
						Weebly.PageManager.cancelDelayedSave = false;
					}else{
						sitePages[pid] = newtitle;
					}
					if (callback) {
						callback();
					}
					if (exit) {
						Weebly.PageManager.exit();
					}
				},
				onFailure: function() {
					Weebly.PageManager.unlock();
					pageOrder = oldPageOrder;
					sitePages[pid] = oldp.title;
					Weebly.PageManager.pages[pid] = oldp;
					Weebly.PageManager.updateCurrentPageUI(pid, oldp.title, oldp.hidden);
					errFunc.apply(this, arguments);
				},
				onException: function() {
					Weebly.PageManager.unlock();
					exceptionFunc.apply(this, arguments);
				}
			});
		
		}else{
		
			if (exit) {
				Weebly.PageManager.exit();
			}
		
		}
	
	},
	
	delayedSave: function() {
		// due to event-firing order, cancelDelayedSave is reset every time displayPage is called, not here
		var pid = Weebly.PageManager.currentPageId;
		setTimeout(function() {
			if (!Weebly.PageManager.cancelDelayedSave) {
				// we need to be editing same page (and current page can't be null, meaning done editing)
				if (pid == Weebly.PageManager.currentPageId) {
					Weebly.PageManager.save();
				}
			}
			Weebly.PageManager.cancelDelayedSave = false; // also reset when done
		}, 50);
	},
	
	implicitSave: function(delayed) {
                Weebly.PageManager.updateLockIcon(Weebly.PageManager.currentPageId, $("page_protected").checked);
		if (Weebly.PageManager.currentPageInDB) {
			if (delayed) {
				Weebly.PageManager.delayedSave();
			}else{
				Weebly.PageManager.save();
			}
		}
	},
	
	
	
	
	
	
	//
	// update other misc UI components that are dependent upon page title/hidden
	//
	
	updateCurrentPageUI: function(pid, title, hidden) {
	
		if (title && $(pid+'title')) {
			$(pid+'title').innerHTML = title.replace(/(.{20}).+/, "$1..."); // update title in list
		}
		
		if (hidden) {
			if ($(pid+'hidden')) {
				$(pid+'hidden').innerHTML = "&nbsp;(Hidden)";
			}
		}else{
			if ($(pid+'hidden')) {
				$(pid+'hidden').innerHTML = "";
			}
		}
		
	},
	
	
	
	
	
	//
	// create new page, add to topLevelPages, pages, sitePages, and pageOrder
	// also, add to page list and call displayPage
	//
	
	addNewPage: function(isBlog, pageData) {
	
		pageData = typeof(pageData) == 'object' ? pageData : {};
        var newId = pageData.id || 'new' + Weebly.PageManager.fakeID++;
		var newTitle = isBlog ? /*tl(*/"New Blog"/*)tl*/ : /*tl(*/"New Page"/*)tl*/;
        newTitle = pageData.title || newTitle;
		var li = document.createElement('li');
		li.className = 'highlightbox';
		li.id = 'pages_'+newId;
		li.innerHTML =
			"<div class='tick'>&nbsp;</div>" +
			"<div class='pagesBuffer' onclick='Weebly.PageManager.displayPage(\""+newId+"\")'><div style='position: relative;'>" +
                                "<div class='pagesDragHandle'></div>" +
				"<span id='"+newId+"title'>" + newTitle.replace(/(.{20}).+/, "$1...") + "</span>" +
				"<span id='"+newId+"hidden'></span>" +
                                "<div class='iconContainer'>" +
                                        "<div class='lockIcon' title='" + /*tl(*/"This page is password protected"/*)tl*/ +"'></div>" +
                                        "<div class='homeIcon' title='" + /*tl(*/"This is the homepage"/*)tl*/ + "'></div>" +
                                "</div>" +
			"</div></div>";
	
		$('pages').appendChild(li);
		Weebly.PageManager.nestedSortable.refresh();
		
		pageOrder.push(newId);
		sitePages[newId] = newTitle;
		var newObj = {
			id: newId,
			title: newTitle,
			blog: isBlog,
			hidden: pageData.hidden || false,
			pwprotected: pageData.pwprotected || false,
			header: '',
			footer: '',
			keywords: '',
			description: '',
			children: []
		};
		Weebly.PageManager.pages[newId] = newObj;
		Weebly.PageManager.topLevelPages.push(newObj);
		
		var notInDB = pageData.id ? false : true;
        Weebly.PageManager.displayPage(newId, notInDB);
		Weebly.PageManager.updateHierarchyLimit();
                Weebly.PageManager.updateIconPosition();
		
		setTimeout(function() {
		
			$('pages-wrap').scrollTop = 10000000; // scroll to bottom of page list
			
			if (Weebly.PageManager.topLevelPages.length > 2) {
				showEvent('secondPage', 0, li);
			}else{
				showEvent('newPage', 0, $('pageTitleEdit').firstChild);
			}
			
		}, 200); // need delay for IE
		
	},
    
    copyPage: function(){
        var params = {
            'pos': 'copypage',
            'page_id': Weebly.PageManager.currentPageId,
            'cookie': document.cookie
        };
        new Ajax.Request(ajax, {
            parameters: params,
            onSuccess: function(t){
                var currentPage = Weebly.PageManager.pages[params.page_id];
                var copiedPage = {
                    'id'    : t.responseText.replace(/[^\d]/g, ''),
                    'title' : currentPage.title,
                    'hidden': currentPage.hidden,
                    'pwprotected' : currentPage.pwprotected
                };
                Weebly.PageManager.addNewPage(currentPage.blog, copiedPage);
                Weebly.PageManager.updateLockIcons();
                //Weebly.PageManager.save();
            },
            onFailure:errFunc
        });
    },
	
	
	
	
	
	//
	// delete currently selected page from UI, and if necessary, server
	//
	
	deleteCurrentPage: function() {
	
		if (!Weebly.PageManager.currentPageInDB) {
		
			Weebly.PageManager._deleteCurrentPage();
			
		}else{
		
			Weebly.PageManager.lock();
			new Ajax.Request(ajax, {
				parameters: {
					pos: 'removepage',
					page_id: Weebly.PageManager.currentPageId,
					site_id: currentSite,
					cookie: document.cookie
				},
				onSuccess: function() {
					Weebly.PageManager.unlock();
					Weebly.PageManager._deleteCurrentPage();
				},
				onFailure: function() {
					Weebly.PageManager.unlock();
					errFunc.apply(this, arguments);
				},
				onException: function() {
					Weebly.PageManager.unlock();
					exceptionFunc.apply(this, arguments);
				}
			});
			
		}
		
	},
	
	_deleteCurrentPage: function() { // make changes in client data structures, update UI
	
		var pid = Weebly.PageManager.currentPageId;
		Weebly.PageManager.currentPageId = null;
		
		// update DOM
		var doomedItem = $('pages_'+pid);
		var prevItem = doomedItem;
		$$('#pages_' + pid + ' > ul > li').each(function(e) { // promote children
			prevItem.insert({ after:e });
			prevItem = e;
		});
		doomedItem.remove();
	
		// update client data structures
		Weebly.PageManager.updateHierarchy(); // update pageOrder, topLevelPages (from DOM)
		delete sitePages[pid];
		delete Weebly.PageManager.pages[pid];
		
		Weebly.PageManager.displayPage(pageOrder[0]); // select first page
		Weebly.PageManager.updateHierarchyLimit();
                Weebly.PageManager.updateIconPosition();
		$('pages-wrap').scrollTop = 0; // scroll to top of pages list
		
	},
	
	
	
	
	
	//
	// leave the Manage Pages interface
	//
	
	exit: function() {
		Pages.go('editMenu'); // automatically calls cleanup (via destructor in weebly_initmain.js)
	},
	
	cleanup: function() {
	
		if (!Weebly.PageManager.currentPageInDB) {
			Weebly.PageManager._deleteCurrentPage(); // new page wasn't saved, clear it
		}
	
		Weebly.PageManager.currentPageId = null;
		if (!Weebly.PageManager.pages[currentPage]) {
			// current page has been deleted, switch to the homepage
		
			currentPage = Weebly.PageManager.topLevelPages[0].id;
			writeTheme(currentTheme, 1);
				// '1' will rewrite nav AND middle content
				
		}else{
		
			writeTheme(currentTheme);
				// will just rewrite nav
			
		}
	},
	
	
	
	
	//
	// a safe way to make sure the current page is saved before editing it in the editor
	//
	
	editThisPage: function() {
		if (Weebly.PageManager.currentPageInDB) {
			goUpdateList(Weebly.PageManager.currentPageId, 1);
		}else{
			Weebly.PageManager.save(false, true, function() {
				goUpdateList(Weebly.PageManager.currentPageId, 1);
			});
		}
	},
	
	
	
	
	//
	// stop editing current page and exit Manage Pages interface
	//
	
	cancelChanges: function() {
		if (!Weebly.PageManager.currentPageInDB) {
			Weebly.PageManager._deleteCurrentPage(); // if server doesn't know about page, delete on client
		}
		Weebly.PageManager.exit();
	},
	
	
	

	
	//
	// generate the HTML for submenus TO BE INSERTED INTO THE REAL NAV **IN THE EDITOR**
	// this function should probably be in flyout_menus.js, but it is not needed on published pages, so put it here
	//
	
	subpagesHTML: function(pageID, visibleSubpages) {
		if (!Weebly.PageManager.pages[pageID]) {
			//console.log('page not in Weebly.PageManager.pages: ' + pageID);
			return "";
		}
		if (typeof visibleSubpages == 'undefined') {
			visibleSubpages = Weebly.PageManager.filterHiddenPages(Weebly.PageManager.pages[pageID].children);
		}
		if (visibleSubpages.length > 0) {
			var s = "<div class='weebly-menu-wrap' style='display:none'><ul class='weebly-menu'>";
			var currID = Weebly.PageManager.currentPageId;
			visibleSubpages.each(function(page) {
				var vsub = Weebly.PageManager.filterHiddenPages(page.children);
				s += "<li id='weebly-nav-" + page.id + "'" + (page.id==currID ? " class='weebly-nav-current'" : '') +
					"><a href='#' onclick='if (notBeenDragged()) { noJump = 1; goUpdateList(\"" + page.id + "\",1); }; return false;'>" +
					"<span class='weebly-menu-title'>" + page.title + "</span>" +
						(vsub.length > 0 ?
							"<span class='weebly-menu-more'>&gt;</span></a>" + Weebly.PageManager.subpagesHTML(page.id, vsub) :
							"</a>") +
					"</li>";
			});
			s += "</ul></div>";
			return s;
		}else{
			return "";
		}
	},
	
	filterHiddenPages: function(pages) {
		var res = [];
		pages.each(function(page) {
			if (!page.hidden) res.push(page);
		});
		return res;
	},
	
	
	
	
	//
	// crawl the page hierarchy DOM and update pageOrder and topLevelPages
	//
	
	updateHierarchy: function() {
		var pgs = Weebly.PageManager._getHierarchy($('pages'));
		pageOrder = [];
		for (var i=0; i<pgs.length; i++) {
			pageOrder.push(pgs[i].id);
		}
		Weebly.PageManager.topLevelPages = pgs;
	},
	
	_getHierarchy: function(list) {
		var res = [];
		list.childElements().each(function(item) {
			var sublist,
				itemElm = $(item.id),
				itemChildren = itemElm.childElements();
			for (var i=0; i<itemChildren.length; i++) { // optimization for large # of pages
				if (itemChildren[i].nodeName == 'UL') {
					sublist = itemChildren[i];
					break;
				}
			}
			//var sublists = $$('#' + item.id + ' > ul'); // item.select('> ul') doesn't work
			var pg = Weebly.PageManager.pages[item.id.match(/pages_(.*)$/)[1]];
			pg.children = sublist ? Weebly.PageManager._getHierarchy(sublist) : [];
			res.push(pg);
		});
		return res;
	},
	
	
	
	
	
	//
	// return flattened hash of pages
	//
	
	getPageIdHash: function() {
		var hash = {};
		Weebly.PageManager._getPageIdHash(hash, Weebly.PageManager.topLevelPages);
		return hash;
	},
	
	_getPageIdHash: function(hash, pgs) {
		for (var i=0; i<pgs.length; i++) {
			hash[pgs[i].id] = pgs[i];
			Weebly.PageManager._getPageIdHash(hash, pgs[i].children);
		}
	},
	
	
	
	
	//
	// do 2 page objects have the same properties? (disregarding order/children)
	//
	
	pageSettingsEqual: function(pg1, pg2) {
		for (var prop in pg1) {
			if (prop == 'children' || prop == 'order' || prop == 'children') continue;
			if (pg1[prop] !== pg2[prop]) {
				return false;
			}
		}
		return true;
	},
	
	
	
	
	
	//
	// Get info about the hierarchy, but only include info on 'id' and 'children'
	//
	
	getAbbreviatedHierarchy: function() {
		return Weebly.PageManager._getAbbreviatedHierarchy(Weebly.PageManager.topLevelPages);
	},
	
	_getAbbreviatedHierarchy: function(pgs) {
		var res = [];
		for (var i=0; i<pgs.length; i++) {
			res.push({
				id: pgs[i].id.indexOf('new')==0 ? '' : pgs[i].id,
				children: Weebly.PageManager._getAbbreviatedHierarchy(pgs[i].children)
			});
		}
		return res;
	},
	

	
	
	
	//
	// Get id/title/onclick for visible top level pages only
	//
	
	getTopLevelSummary: function() {
		var res = [];
		Weebly.PageManager.topLevelPages.each(function(page) {
			if (!page.hidden) {
				res.push({
					id: page.id,
					title: page.title,
					onclick: function() {
						if (notBeenDragged()) {
							noJump = 1;
							goUpdateList(page.id, 1);
						}
						return false;
					}
				});
			}
		});
		return res;
	},
	
	
	
	
	lock: function() {
		var c = $('managePagesCover');
		if (c) {
			c.show();
		}
	},
	
	unlock: function() {
		var c = $('managePagesCover');
		if (c) {
			c.hide();
		}
	}


}
