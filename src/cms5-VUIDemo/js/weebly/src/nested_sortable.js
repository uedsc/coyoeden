
/*
 * A scriptaculous plugin, analagous to Sortable, that allows
 * a hierarchical sortable list (with nested UL's and LI's)
 *
 * Author: Adam Shaw
 *
 */

var NestedSortable = {

	create: function(list, options) {
	
		list = $(list);
		options = options || {};
		
		var scrollParent = options.scroll ? $(options.scroll) : null;
		var placeholderOpacity = (typeof options.placeholderOpacity == 'undefined') ? 0.5 : options.placeholderOpacity;
		var allowIndent = typeof options.allowIndent == 'undefined' ? true : options.allowIndent;
		
		var placeholder;
		var itemMouseX, itemMouseY;
		var origScrollTop;
		var origParent, origPrevSibling;
		var currentParent, currentPrevSibling;
		
		var linesAbove, linesBelow, linesLeft, lineRight;
		var canOutdent, canIndent;
		
		var draggables = [];
		
		function initItems() {
			draggables = [];
			list.select(options.itemSelector || 'li').each(function(item) {
				draggables.push(
					new Draggable(item, {
						scroll: scrollParent,
						handle: options.handleSelector ? item.select(options.handleSelector)[0] : item,
						constraint: options.constraint,
						//ghosting: true,
							// doesn't work with 'scroll' option...
							// http://groups.google.co.uk/group/prototype-scriptaculous/browse_thread/thread/28bc3458097c5072
							// figured out workaround by manually inserting a 'placeholder', and setting position='absolute'
						onStart: function(draggableObj, mouseEvent) {
					
							placeholder = item.cloneNode(true);
							placeholder.style.position = 'absolute';
							placeholder.setOpacity(placeholderOpacity);
							item.insert({ before:placeholder });
					
							var itemOffset = item.cumulativeOffset();
							itemMouseX = mouseEvent.clientX - itemOffset.left;
							itemMouseY = mouseEvent.clientY - itemOffset.top;
							
							if (scrollParent) {
								origScrollTop = scrollParent.scrollTop;
							}
						
							updateLines(item, placeholder);
						
							origParent = currentParent;
							origPrevSibling = currentPrevSibling;
							
							if (options.onStart) {
								options.onStart.call(item);
							}
						
						},
						onDrag: function(draggableObj, mouseEvent) {
					
							if (!mouseEvent) return; // sometimes this is undefined
						
							var elementX = mouseEvent.clientX - itemMouseX;
							var elementY = mouseEvent.clientY - itemMouseY;
							
							if (scrollParent) {
								// scriptaculous's scroll option messes up coords. fix
								elementY += scrollParent.scrollTop - origScrollTop;
							}
						
							//console.log(elementX + ', ' + elementY);
					
							var i = 0;
							while (i < linesAbove.length && elementY < linesAbove[i][0]) i++;
							if (i > 0) {
								//console.log('up');
								var prevUL = $(item.parentNode);
								linesAbove[i-1][1].insert({ before: placeholder });
								linesAbove[i-1][1].insert({ before: item });
								if (prevUL.childElements().length == 0) {
									// delete the empty ul
									prevUL.remove();
								}
								updateLines(item, placeholder);
                                                                if (options.onPop) {
                                                                        options.onPop.call(item);
                                                                }
							}else{
								i = 0;
								while (i < linesBelow.length && elementY >= linesBelow[i][0]) i++;
								if (i > 0) {
									//console.log('down');
									var prevUL = $(item.parentNode);
									if (i < linesBelow.length) {
										linesBelow[i][1].insert({ before: placeholder });
										linesBelow[i][1].insert({ before: item });
									}else{
										linesBelow[linesBelow.length-1][1].insert({ after: item });
										linesBelow[linesBelow.length-1][1].insert({ after: placeholder });
									}
									if (prevUL.childElements().length == 0) {
										// delete the empty ul
										prevUL.remove();
									}
									updateLines(item, placeholder);
                                                                        if (options.onPop) {
                                                                                options.onPop.call(item);
                                                                        }
								}
							}
							if (canOutdent) {
								i = 0;
								while (i < linesLeft.length && elementX < linesLeft[i][0]) i++;
								if (i > 0) {
									//console.log('outdent');
									var prevUL = $(item.parentNode);
									linesLeft[i-1][1].parentNode.insert({ after: item });
									linesLeft[i-1][1].parentNode.insert({ after: placeholder });
									if (prevUL.childElements().length == 0) {
										// delete the empty ul
										prevUL.remove();
									}
									updateLines(item, placeholder);
                                                                        if (options.onPop) {
                                                                                options.onPop.call(item);
                                                                        }
								}
							}
							if (canIndent) {
								if (elementX > lineRight) {
									//console.log('indent');
									var prev = item.previous();
									if (prev == placeholder) prev = prev.previous();
									var subcontainer = prev.down('UL');
									if (!subcontainer) {
										subcontainer = $(document.createElement('UL'));
										prev.insert(subcontainer);
									}
									subcontainer.insert(placeholder);
									subcontainer.insert(item);
									updateLines(item, placeholder);
                                                                        if (options.onPop) {
                                                                                options.onPop.call(item);
                                                                        }
								}
							}
					
						},
						onEnd: function() {
						
							placeholder.remove();
							
							//new Effect.Move(item, { x: 0, y: 0, mode: 'absolute' });
								// in the future, would like to similate a nice 'revert' effect
							item.style.top = '';
							item.style.left = '';
							
							refreshItems();
							
							if (options.onChange && (currentParent != origParent || currentPrevSibling != origPrevSibling)) {
								options.onChange();
							}
							if (options.onEnd) {
								options.onEnd.call(item);
							}
						}
					})
				);
			});
		}
		
		function refreshItems() {
			for (var i=0; i<draggables.length; i++) {
				draggables[i].destroy();
			}
			initItems();
		}
		
		function updateLines(item, placeholder) {
			
			var prev = item.previous();
			if (prev == placeholder) prev = prev.previous();
			
			var next = item.next();
			
			canOutdent = !next && item.parentNode != list;
			canIndent = allowIndent && prev;
			
			if (canIndent) {
				placeholder = $(placeholder);
				lineRight = placeholder.cumulativeOffset().left + 10; // for now
			}
			
			linesAbove = [];
			var n = prevElement(placeholder);
			while (n && n != list) {
				if (n.nodeName == 'LI' && n != placeholder) {
					linesAbove.push([n.cumulativeOffset().top + 10, n]); // for now
				}
				n = prevElement(n);
			}
			
			linesBelow = [];
			n = nextElement(item, true);
			var stop = nextElement(list, true);
			while (n && n != stop) {
				if (n.nodeName == 'LI' && n != placeholder) {
					linesBelow.push([n.cumulativeOffset().top - 10, n]); // for now
				}
				n = nextElement(n);
			}
			
			linesLeft = [];
			if (canOutdent) {
				n = $(item.parentNode);
				while (n && n != list) {
					if (n.nodeName == 'UL') {
						linesLeft.push([n.cumulativeOffset().left - 0, n]); // for now
					}
					n = $(n.parentNode);
				}
			}
			
			currentParent = item.parentNode;
			currentPrevSibling = prev;
		}
		
		function prevElement(n) {
			while (true) {
				if (n.previousSibling) {
					n = n.previousSibling;
					while (n.lastChild)
						n = n.lastChild;
				}
				else if (n.parentNode) {
					n = n.parentNode;
					if (!n) return null;
				}
				if (n.nodeType == 1) return $(n);
			}
		}
		
		function nextElement(n, skip0) {
			while (true) {
				if (!skip0 && n.firstChild) n = n.firstChild;
				else if (n.nextSibling) n = n.nextSibling;
				else {
					do {
						n = n.parentNode;
						if (!n) return null;
					} while (!n.nextSibling);
					n = n.nextSibling;
				}
				if (n.nodeType == 1) return $(n);
				skip0 = false;
			}
		}
		
		initItems();
		
		this.refresh = refreshItems;
		return this;
		
	}

};
