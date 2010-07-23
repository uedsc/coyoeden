
/*!
 * Sizzle CSS Selector Engine - v1.0
 *  Copyright 2009, The Dojo Foundation
 *  Released under the MIT, BSD, and GPL Licenses.
 *  More information: http://sizzlejs.com/
 */
(function(){

var chunker = /((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^[\]]*\]|['"][^'"]*['"]|[^[\]'"]+)+\]|\\.|[^ >+~,(\[\\]+)+|[>+~])(\s*,\s*)?((?:.|\r|\n)*)/g,
	done = 0,
	toString = Object.prototype.toString,
	hasDuplicate = false,
	baseHasDuplicate = true;

// Here we check if the JavaScript engine is using some sort of
// optimization where it does not always call our comparision
// function. If that is the case, discard the hasDuplicate value.
//   Thus far that includes Google Chrome.
[0, 0].sort(function(){
	baseHasDuplicate = false;
	return 0;
});

var Sizzle = function(selector, context, results, seed) {
	results = results || [];
	var origContext = context = context || document;

	if ( context.nodeType !== 1 && context.nodeType !== 9 ) {
		return [];
	}
	
	if ( !selector || typeof selector !== "string" ) {
		return results;
	}

	var parts = [], m, set, checkSet, extra, prune = true, contextXML = isXML(context),
		soFar = selector;
	
	// Reset the position of the chunker regexp (start from head)
	while ( (chunker.exec(""), m = chunker.exec(soFar)) !== null ) {
		soFar = m[3];
		
		parts.push( m[1] );
		
		if ( m[2] ) {
			extra = m[3];
			break;
		}
	}

	if ( parts.length > 1 && origPOS.exec( selector ) ) {
		if ( parts.length === 2 && Expr.relative[ parts[0] ] ) {
			set = posProcess( parts[0] + parts[1], context );
		} else {
			set = Expr.relative[ parts[0] ] ?
				[ context ] :
				Sizzle( parts.shift(), context );

			while ( parts.length ) {
				selector = parts.shift();

				if ( Expr.relative[ selector ] ) {
					selector += parts.shift();
				}
				
				set = posProcess( selector, set );
			}
		}
	} else {
		// Take a shortcut and set the context if the root selector is an ID
		// (but not if it'll be faster if the inner selector is an ID)
		if ( !seed && parts.length > 1 && context.nodeType === 9 && !contextXML &&
				Expr.match.ID.test(parts[0]) && !Expr.match.ID.test(parts[parts.length - 1]) ) {
			var ret = Sizzle.find( parts.shift(), context, contextXML );
			context = ret.expr ? Sizzle.filter( ret.expr, ret.set )[0] : ret.set[0];
		}

		if ( context ) {
			var ret = seed ?
				{ expr: parts.pop(), set: makeArray(seed) } :
				Sizzle.find( parts.pop(), parts.length === 1 && (parts[0] === "~" || parts[0] === "+") && context.parentNode ? context.parentNode : context, contextXML );
			set = ret.expr ? Sizzle.filter( ret.expr, ret.set ) : ret.set;

			if ( parts.length > 0 ) {
				checkSet = makeArray(set);
			} else {
				prune = false;
			}

			while ( parts.length ) {
				var cur = parts.pop(), pop = cur;

				if ( !Expr.relative[ cur ] ) {
					cur = "";
				} else {
					pop = parts.pop();
				}

				if ( pop == null ) {
					pop = context;
				}

				Expr.relative[ cur ]( checkSet, pop, contextXML );
			}
		} else {
			checkSet = parts = [];
		}
	}

	if ( !checkSet ) {
		checkSet = set;
	}

	if ( !checkSet ) {
		Sizzle.error( cur || selector );
	}

	if ( toString.call(checkSet) === "[object Array]" ) {
		if ( !prune ) {
			results.push.apply( results, checkSet );
		} else if ( context && context.nodeType === 1 ) {
			for ( var i = 0; checkSet[i] != null; i++ ) {
				if ( checkSet[i] && (checkSet[i] === true || checkSet[i].nodeType === 1 && contains(context, checkSet[i])) ) {
					results.push( set[i] );
				}
			}
		} else {
			for ( var i = 0; checkSet[i] != null; i++ ) {
				if ( checkSet[i] && checkSet[i].nodeType === 1 ) {
					results.push( set[i] );
				}
			}
		}
	} else {
		makeArray( checkSet, results );
	}

	if ( extra ) {
		Sizzle( extra, origContext, results, seed );
		Sizzle.uniqueSort( results );
	}

	return results;
};

Sizzle.uniqueSort = function(results){
	if ( sortOrder ) {
		hasDuplicate = baseHasDuplicate;
		results.sort(sortOrder);

		if ( hasDuplicate ) {
			for ( var i = 1; i < results.length; i++ ) {
				if ( results[i] === results[i-1] ) {
					results.splice(i--, 1);
				}
			}
		}
	}

	return results;
};

Sizzle.matches = function(expr, set){
	return Sizzle(expr, null, null, set);
};

Sizzle.find = function(expr, context, isXML){
	var set, match;

	if ( !expr ) {
		return [];
	}

	for ( var i = 0, l = Expr.order.length; i < l; i++ ) {
		var type = Expr.order[i], match;
		
		if ( (match = Expr.leftMatch[ type ].exec( expr )) ) {
			var left = match[1];
			match.splice(1,1);

			if ( left.substr( left.length - 1 ) !== "\\" ) {
				match[1] = (match[1] || "").replace(/\\/g, "");
				set = Expr.find[ type ]( match, context, isXML );
				if ( set != null ) {
					expr = expr.replace( Expr.match[ type ], "" );
					break;
				}
			}
		}
	}

	if ( !set ) {
		set = context.getElementsByTagName("*");
	}

	return {set: set, expr: expr};
};

Sizzle.filter = function(expr, set, inplace, not){
	var old = expr, result = [], curLoop = set, match, anyFound,
		isXMLFilter = set && set[0] && isXML(set[0]);

	while ( expr && set.length ) {
		for ( var type in Expr.filter ) {
			if ( (match = Expr.leftMatch[ type ].exec( expr )) != null && match[2] ) {
				var filter = Expr.filter[ type ], found, item, left = match[1];
				anyFound = false;

				match.splice(1,1);

				if ( left.substr( left.length - 1 ) === "\\" ) {
					continue;
				}

				if ( curLoop === result ) {
					result = [];
				}

				if ( Expr.preFilter[ type ] ) {
					match = Expr.preFilter[ type ]( match, curLoop, inplace, result, not, isXMLFilter );

					if ( !match ) {
						anyFound = found = true;
					} else if ( match === true ) {
						continue;
					}
				}

				if ( match ) {
					for ( var i = 0; (item = curLoop[i]) != null; i++ ) {
						if ( item ) {
							found = filter( item, match, i, curLoop );
							var pass = not ^ !!found;

							if ( inplace && found != null ) {
								if ( pass ) {
									anyFound = true;
								} else {
									curLoop[i] = false;
								}
							} else if ( pass ) {
								result.push( item );
								anyFound = true;
							}
						}
					}
				}

				if ( found !== undefined ) {
					if ( !inplace ) {
						curLoop = result;
					}

					expr = expr.replace( Expr.match[ type ], "" );

					if ( !anyFound ) {
						return [];
					}

					break;
				}
			}
		}

		// Improper expression
		if ( expr === old ) {
			if ( anyFound == null ) {
				Sizzle.error( expr );
			} else {
				break;
			}
		}

		old = expr;
	}

	return curLoop;
};

Sizzle.error = function( msg ) {
	throw "Syntax error, unrecognized expression: " + msg;
};

var Expr = Sizzle.selectors = {
	order: [ "ID", "NAME", "TAG" ],
	match: {
		ID: /#((?:[\w\u00c0-\uFFFF-]|\\.)+)/,
		CLASS: /\.((?:[\w\u00c0-\uFFFF-]|\\.)+)/,
		NAME: /\[name=['"]*((?:[\w\u00c0-\uFFFF-]|\\.)+)['"]*\]/,
		ATTR: /\[\s*((?:[\w\u00c0-\uFFFF-]|\\.)+)\s*(?:(\S?=)\s*(['"]*)(.*?)\3|)\s*\]/,
		TAG: /^((?:[\w\u00c0-\uFFFF\*-]|\\.)+)/,
		CHILD: /:(only|nth|last|first)-child(?:\((even|odd|[\dn+-]*)\))?/,
		POS: /:(nth|eq|gt|lt|first|last|even|odd)(?:\((\d*)\))?(?=[^-]|$)/,
		PSEUDO: /:((?:[\w\u00c0-\uFFFF-]|\\.)+)(?:\((['"]?)((?:\([^\)]+\)|[^\(\)]*)+)\2\))?/
	},
	leftMatch: {},
	attrMap: {
		"class": "className",
		"for": "htmlFor"
	},
	attrHandle: {
		href: function(elem){
			return elem.getAttribute("href");
		}
	},
	relative: {
		"+": function(checkSet, part){
			var isPartStr = typeof part === "string",
				isTag = isPartStr && !/\W/.test(part),
				isPartStrNotTag = isPartStr && !isTag;

			if ( isTag ) {
				part = part.toLowerCase();
			}

			for ( var i = 0, l = checkSet.length, elem; i < l; i++ ) {
				if ( (elem = checkSet[i]) ) {
					while ( (elem = elem.previousSibling) && elem.nodeType !== 1 ) {}

					checkSet[i] = isPartStrNotTag || elem && elem.nodeName.toLowerCase() === part ?
						elem || false :
						elem === part;
				}
			}

			if ( isPartStrNotTag ) {
				Sizzle.filter( part, checkSet, true );
			}
		},
		">": function(checkSet, part){
			var isPartStr = typeof part === "string";

			if ( isPartStr && !/\W/.test(part) ) {
				part = part.toLowerCase();

				for ( var i = 0, l = checkSet.length; i < l; i++ ) {
					var elem = checkSet[i];
					if ( elem ) {
						var parent = elem.parentNode;
						checkSet[i] = parent.nodeName.toLowerCase() === part ? parent : false;
					}
				}
			} else {
				for ( var i = 0, l = checkSet.length; i < l; i++ ) {
					var elem = checkSet[i];
					if ( elem ) {
						checkSet[i] = isPartStr ?
							elem.parentNode :
							elem.parentNode === part;
					}
				}

				if ( isPartStr ) {
					Sizzle.filter( part, checkSet, true );
				}
			}
		},
		"": function(checkSet, part, isXML){
			var doneName = done++, checkFn = dirCheck;

			if ( typeof part === "string" && !/\W/.test(part) ) {
				var nodeCheck = part = part.toLowerCase();
				checkFn = dirNodeCheck;
			}

			checkFn("parentNode", part, doneName, checkSet, nodeCheck, isXML);
		},
		"~": function(checkSet, part, isXML){
			var doneName = done++, checkFn = dirCheck;

			if ( typeof part === "string" && !/\W/.test(part) ) {
				var nodeCheck = part = part.toLowerCase();
				checkFn = dirNodeCheck;
			}

			checkFn("previousSibling", part, doneName, checkSet, nodeCheck, isXML);
		}
	},
	find: {
		ID: function(match, context, isXML){
			if ( typeof context.getElementById !== "undefined" && !isXML ) {
				var m = context.getElementById(match[1]);
				return m ? [m] : [];
			}
		},
		NAME: function(match, context){
			if ( typeof context.getElementsByName !== "undefined" ) {
				var ret = [], results = context.getElementsByName(match[1]);

				for ( var i = 0, l = results.length; i < l; i++ ) {
					if ( results[i].getAttribute("name") === match[1] ) {
						ret.push( results[i] );
					}
				}

				return ret.length === 0 ? null : ret;
			}
		},
		TAG: function(match, context){
			return context.getElementsByTagName(match[1]);
		}
	},
	preFilter: {
		CLASS: function(match, curLoop, inplace, result, not, isXML){
			match = " " + match[1].replace(/\\/g, "") + " ";

			if ( isXML ) {
				return match;
			}

			for ( var i = 0, elem; (elem = curLoop[i]) != null; i++ ) {
				if ( elem ) {
					if ( not ^ (elem.className && (" " + elem.className + " ").replace(/[\t\n]/g, " ").indexOf(match) >= 0) ) {
						if ( !inplace ) {
							result.push( elem );
						}
					} else if ( inplace ) {
						curLoop[i] = false;
					}
				}
			}

			return false;
		},
		ID: function(match){
			return match[1].replace(/\\/g, "");
		},
		TAG: function(match, curLoop){
			return match[1].toLowerCase();
		},
		CHILD: function(match){
			if ( match[1] === "nth" ) {
				// parse equations like 'even', 'odd', '5', '2n', '3n+2', '4n-1', '-n+6'
				var test = /(-?)(\d*)n((?:\+|-)?\d*)/.exec(
					match[2] === "even" && "2n" || match[2] === "odd" && "2n+1" ||
					!/\D/.test( match[2] ) && "0n+" + match[2] || match[2]);

				// calculate the numbers (first)n+(last) including if they are negative
				match[2] = (test[1] + (test[2] || 1)) - 0;
				match[3] = test[3] - 0;
			}

			// TODO: Move to normal caching system
			match[0] = done++;

			return match;
		},
		ATTR: function(match, curLoop, inplace, result, not, isXML){
			var name = match[1].replace(/\\/g, "");
			
			if ( !isXML && Expr.attrMap[name] ) {
				match[1] = Expr.attrMap[name];
			}

			if ( match[2] === "~=" ) {
				match[4] = " " + match[4] + " ";
			}

			return match;
		},
		PSEUDO: function(match, curLoop, inplace, result, not){
			if ( match[1] === "not" ) {
				// If we're dealing with a complex expression, or a simple one
				if ( ( chunker.exec(match[3]) || "" ).length > 1 || /^\w/.test(match[3]) ) {
					match[3] = Sizzle(match[3], null, null, curLoop);
				} else {
					var ret = Sizzle.filter(match[3], curLoop, inplace, true ^ not);
					if ( !inplace ) {
						result.push.apply( result, ret );
					}
					return false;
				}
			} else if ( Expr.match.POS.test( match[0] ) || Expr.match.CHILD.test( match[0] ) ) {
				return true;
			}
			
			return match;
		},
		POS: function(match){
			match.unshift( true );
			return match;
		}
	},
	filters: {
		enabled: function(elem){
			return elem.disabled === false && elem.type !== "hidden";
		},
		disabled: function(elem){
			return elem.disabled === true;
		},
		checked: function(elem){
			return elem.checked === true;
		},
		selected: function(elem){
			// Accessing this property makes selected-by-default
			// options in Safari work properly
			elem.parentNode.selectedIndex;
			return elem.selected === true;
		},
		parent: function(elem){
			return !!elem.firstChild;
		},
		empty: function(elem){
			return !elem.firstChild;
		},
		has: function(elem, i, match){
			return !!Sizzle( match[3], elem ).length;
		},
		header: function(elem){
			return /h\d/i.test( elem.nodeName );
		},
		text: function(elem){
			return "text" === elem.type;
		},
		radio: function(elem){
			return "radio" === elem.type;
		},
		checkbox: function(elem){
			return "checkbox" === elem.type;
		},
		file: function(elem){
			return "file" === elem.type;
		},
		password: function(elem){
			return "password" === elem.type;
		},
		submit: function(elem){
			return "submit" === elem.type;
		},
		image: function(elem){
			return "image" === elem.type;
		},
		reset: function(elem){
			return "reset" === elem.type;
		},
		button: function(elem){
			return "button" === elem.type || elem.nodeName.toLowerCase() === "button";
		},
		input: function(elem){
			return /input|select|textarea|button/i.test(elem.nodeName);
		}
	},
	setFilters: {
		first: function(elem, i){
			return i === 0;
		},
		last: function(elem, i, match, array){
			return i === array.length - 1;
		},
		even: function(elem, i){
			return i % 2 === 0;
		},
		odd: function(elem, i){
			return i % 2 === 1;
		},
		lt: function(elem, i, match){
			return i < match[3] - 0;
		},
		gt: function(elem, i, match){
			return i > match[3] - 0;
		},
		nth: function(elem, i, match){
			return match[3] - 0 === i;
		},
		eq: function(elem, i, match){
			return match[3] - 0 === i;
		}
	},
	filter: {
		PSEUDO: function(elem, match, i, array){
			var name = match[1], filter = Expr.filters[ name ];

			if ( filter ) {
				return filter( elem, i, match, array );
			} else if ( name === "contains" ) {
				return (elem.textContent || elem.innerText || getText([ elem ]) || "").indexOf(match[3]) >= 0;
			} else if ( name === "not" ) {
				var not = match[3];

				for ( var i = 0, l = not.length; i < l; i++ ) {
					if ( not[i] === elem ) {
						return false;
					}
				}

				return true;
			} else {
				Sizzle.error( "Syntax error, unrecognized expression: " + name );
			}
		},
		CHILD: function(elem, match){
			var type = match[1], node = elem;
			switch (type) {
				case 'only':
				case 'first':
					while ( (node = node.previousSibling) )	 {
						if ( node.nodeType === 1 ) { 
							return false; 
						}
					}
					if ( type === "first" ) { 
						return true; 
					}
					node = elem;
				case 'last':
					while ( (node = node.nextSibling) )	 {
						if ( node.nodeType === 1 ) { 
							return false; 
						}
					}
					return true;
				case 'nth':
					var first = match[2], last = match[3];

					if ( first === 1 && last === 0 ) {
						return true;
					}
					
					var doneName = match[0],
						parent = elem.parentNode;
	
					if ( parent && (parent.sizcache !== doneName || !elem.nodeIndex) ) {
						var count = 0;
						for ( node = parent.firstChild; node; node = node.nextSibling ) {
							if ( node.nodeType === 1 ) {
								node.nodeIndex = ++count;
							}
						} 
						parent.sizcache = doneName;
					}
					
					var diff = elem.nodeIndex - last;
					if ( first === 0 ) {
						return diff === 0;
					} else {
						return ( diff % first === 0 && diff / first >= 0 );
					}
			}
		},
		ID: function(elem, match){
			return elem.nodeType === 1 && elem.getAttribute("id") === match;
		},
		TAG: function(elem, match){
			return (match === "*" && elem.nodeType === 1) || elem.nodeName.toLowerCase() === match;
		},
		CLASS: function(elem, match){
			return (" " + (elem.className || elem.getAttribute("class")) + " ")
				.indexOf( match ) > -1;
		},
		ATTR: function(elem, match){
			var name = match[1],
				result = Expr.attrHandle[ name ] ?
					Expr.attrHandle[ name ]( elem ) :
					elem[ name ] != null ?
						elem[ name ] :
						elem.getAttribute( name ),
				value = result + "",
				type = match[2],
				check = match[4];

			return result == null ?
				type === "!=" :
				type === "=" ?
				value === check :
				type === "*=" ?
				value.indexOf(check) >= 0 :
				type === "~=" ?
				(" " + value + " ").indexOf(check) >= 0 :
				!check ?
				value && result !== false :
				type === "!=" ?
				value !== check :
				type === "^=" ?
				value.indexOf(check) === 0 :
				type === "$=" ?
				value.substr(value.length - check.length) === check :
				type === "|=" ?
				value === check || value.substr(0, check.length + 1) === check + "-" :
				false;
		},
		POS: function(elem, match, i, array){
			var name = match[2], filter = Expr.setFilters[ name ];

			if ( filter ) {
				return filter( elem, i, match, array );
			}
		}
	}
};

var origPOS = Expr.match.POS;

for ( var type in Expr.match ) {
	Expr.match[ type ] = new RegExp( Expr.match[ type ].source + /(?![^\[]*\])(?![^\(]*\))/.source );
	Expr.leftMatch[ type ] = new RegExp( /(^(?:.|\r|\n)*?)/.source + Expr.match[ type ].source.replace(/\\(\d+)/g, function(all, num){
		return "\\" + (num - 0 + 1);
	}));
}

var makeArray = function(array, results) {
	array = Array.prototype.slice.call( array, 0 );

	if ( results ) {
		results.push.apply( results, array );
		return results;
	}
	
	return array;
};

// Perform a simple check to determine if the browser is capable of
// converting a NodeList to an array using builtin methods.
try {
	Array.prototype.slice.call( document.documentElement.childNodes, 0 );

// Provide a fallback method if it does not work
} catch(e){
	makeArray = function(array, results) {
		var ret = results || [];

		if ( toString.call(array) === "[object Array]" ) {
			Array.prototype.push.apply( ret, array );
		} else {
			if ( typeof array.length === "number" ) {
				for ( var i = 0, l = array.length; i < l; i++ ) {
					ret.push( array[i] );
				}
			} else {
				for ( var i = 0; array[i]; i++ ) {
					ret.push( array[i] );
				}
			}
		}

		return ret;
	};
}

var sortOrder;

if ( document.documentElement.compareDocumentPosition ) {
	sortOrder = function( a, b ) {
		if ( !a.compareDocumentPosition || !b.compareDocumentPosition ) {
			if ( a == b ) {
				hasDuplicate = true;
			}
			return a.compareDocumentPosition ? -1 : 1;
		}

		var ret = a.compareDocumentPosition(b) & 4 ? -1 : a === b ? 0 : 1;
		if ( ret === 0 ) {
			hasDuplicate = true;
		}
		return ret;
	};
} else if ( "sourceIndex" in document.documentElement ) {
	sortOrder = function( a, b ) {
		if ( !a.sourceIndex || !b.sourceIndex ) {
			if ( a == b ) {
				hasDuplicate = true;
			}
			return a.sourceIndex ? -1 : 1;
		}

		var ret = a.sourceIndex - b.sourceIndex;
		if ( ret === 0 ) {
			hasDuplicate = true;
		}
		return ret;
	};
} else if ( document.createRange ) {
	sortOrder = function( a, b ) {
		if ( !a.ownerDocument || !b.ownerDocument ) {
			if ( a == b ) {
				hasDuplicate = true;
			}
			return a.ownerDocument ? -1 : 1;
		}

		var aRange = a.ownerDocument.createRange(), bRange = b.ownerDocument.createRange();
		aRange.setStart(a, 0);
		aRange.setEnd(a, 0);
		bRange.setStart(b, 0);
		bRange.setEnd(b, 0);
		var ret = aRange.compareBoundaryPoints(Range.START_TO_END, bRange);
		if ( ret === 0 ) {
			hasDuplicate = true;
		}
		return ret;
	};
}

// Utility function for retreiving the text value of an array of DOM nodes
function getText( elems ) {
	var ret = "", elem;

	for ( var i = 0; elems[i]; i++ ) {
		elem = elems[i];

		// Get the text from text nodes and CDATA nodes
		if ( elem.nodeType === 3 || elem.nodeType === 4 ) {
			ret += elem.nodeValue;

		// Traverse everything else, except comment nodes
		} else if ( elem.nodeType !== 8 ) {
			ret += getText( elem.childNodes );
		}
	}

	return ret;
}

// Check to see if the browser returns elements by name when
// querying by getElementById (and provide a workaround)
(function(){
	// We're going to inject a fake input element with a specified name
	var form = document.createElement("div"),
		id = "script" + (new Date).getTime();
	form.innerHTML = "<a name='" + id + "'/>";

	// Inject it into the root element, check its status, and remove it quickly
	var root = document.documentElement;
	root.insertBefore( form, root.firstChild );

	// The workaround has to do additional checks after a getElementById
	// Which slows things down for other browsers (hence the branching)
	if ( document.getElementById( id ) ) {
		Expr.find.ID = function(match, context, isXML){
			if ( typeof context.getElementById !== "undefined" && !isXML ) {
				var m = context.getElementById(match[1]);
				return m ? m.id === match[1] || typeof m.getAttributeNode !== "undefined" && m.getAttributeNode("id").nodeValue === match[1] ? [m] : undefined : [];
			}
		};

		Expr.filter.ID = function(elem, match){
			var node = typeof elem.getAttributeNode !== "undefined" && elem.getAttributeNode("id");
			return elem.nodeType === 1 && node && node.nodeValue === match;
		};
	}

	root.removeChild( form );
	root = form = null; // release memory in IE
})();

(function(){
	// Check to see if the browser returns only elements
	// when doing getElementsByTagName("*")

	// Create a fake element
	var div = document.createElement("div");
	div.appendChild( document.createComment("") );

	// Make sure no comments are found
	if ( div.getElementsByTagName("*").length > 0 ) {
		Expr.find.TAG = function(match, context){
			var results = context.getElementsByTagName(match[1]);

			// Filter out possible comments
			if ( match[1] === "*" ) {
				var tmp = [];

				for ( var i = 0; results[i]; i++ ) {
					if ( results[i].nodeType === 1 ) {
						tmp.push( results[i] );
					}
				}

				results = tmp;
			}

			return results;
		};
	}

	// Check to see if an attribute returns normalized href attributes
	div.innerHTML = "<a href='#'></a>";
	if ( div.firstChild && typeof div.firstChild.getAttribute !== "undefined" &&
			div.firstChild.getAttribute("href") !== "#" ) {
		Expr.attrHandle.href = function(elem){
			return elem.getAttribute("href", 2);
		};
	}

	div = null; // release memory in IE
})();

if ( document.querySelectorAll ) {
	(function(){
		var oldSizzle = Sizzle, div = document.createElement("div");
		div.innerHTML = "<p class='TEST'></p>";

		// Safari can't handle uppercase or unicode characters when
		// in quirks mode.
		if ( div.querySelectorAll && div.querySelectorAll(".TEST").length === 0 ) {
			return;
		}
	
		Sizzle = function(query, context, extra, seed){
			context = context || document;

			// Only use querySelectorAll on non-XML documents
			// (ID selectors don't work in non-HTML documents)
			if ( !seed && context.nodeType === 9 && !isXML(context) ) {
				try {
					return makeArray( context.querySelectorAll(query), extra );
				} catch(e){}
			}
		
			return oldSizzle(query, context, extra, seed);
		};

		for ( var prop in oldSizzle ) {
			Sizzle[ prop ] = oldSizzle[ prop ];
		}

		div = null; // release memory in IE
	})();
}

(function(){
	var div = document.createElement("div");

	div.innerHTML = "<div class='test e'></div><div class='test'></div>";

	// Opera can't find a second classname (in 9.6)
	// Also, make sure that getElementsByClassName actually exists
	if ( !div.getElementsByClassName || div.getElementsByClassName("e").length === 0 ) {
		return;
	}

	// Safari caches class attributes, doesn't catch changes (in 3.2)
	div.lastChild.className = "e";

	if ( div.getElementsByClassName("e").length === 1 ) {
		return;
	}
	
	Expr.order.splice(1, 0, "CLASS");
	Expr.find.CLASS = function(match, context, isXML) {
		if ( typeof context.getElementsByClassName !== "undefined" && !isXML ) {
			return context.getElementsByClassName(match[1]);
		}
	};

	div = null; // release memory in IE
})();

function dirNodeCheck( dir, cur, doneName, checkSet, nodeCheck, isXML ) {
	for ( var i = 0, l = checkSet.length; i < l; i++ ) {
		var elem = checkSet[i];
		if ( elem ) {
			elem = elem[dir];
			var match = false;

			while ( elem ) {
				if ( elem.sizcache === doneName ) {
					match = checkSet[elem.sizset];
					break;
				}

				if ( elem.nodeType === 1 && !isXML ){
					elem.sizcache = doneName;
					elem.sizset = i;
				}

				if ( elem.nodeName.toLowerCase() === cur ) {
					match = elem;
					break;
				}

				elem = elem[dir];
			}

			checkSet[i] = match;
		}
	}
}

function dirCheck( dir, cur, doneName, checkSet, nodeCheck, isXML ) {
	for ( var i = 0, l = checkSet.length; i < l; i++ ) {
		var elem = checkSet[i];
		if ( elem ) {
			elem = elem[dir];
			var match = false;

			while ( elem ) {
				if ( elem.sizcache === doneName ) {
					match = checkSet[elem.sizset];
					break;
				}

				if ( elem.nodeType === 1 ) {
					if ( !isXML ) {
						elem.sizcache = doneName;
						elem.sizset = i;
					}
					if ( typeof cur !== "string" ) {
						if ( elem === cur ) {
							match = true;
							break;
						}

					} else if ( Sizzle.filter( cur, [elem] ).length > 0 ) {
						match = elem;
						break;
					}
				}

				elem = elem[dir];
			}

			checkSet[i] = match;
		}
	}
}

var contains = document.compareDocumentPosition ? function(a, b){
	return !!(a.compareDocumentPosition(b) & 16);
} : function(a, b){
	return a !== b && (a.contains ? a.contains(b) : true);
};

var isXML = function(elem){
	// documentElement is verified for cases where it doesn't yet exist
	// (such as loading iframes in IE - #4833) 
	var documentElement = (elem ? elem.ownerDocument || elem : 0).documentElement;
	return documentElement ? documentElement.nodeName !== "HTML" : false;
};

var posProcess = function(selector, context){
	var tmpSet = [], later = "", match,
		root = context.nodeType ? [context] : context;

	// Position selectors must be done after the filter
	// And so must :not(positional) so we move all PSEUDOs to the end
	while ( (match = Expr.match.PSEUDO.exec( selector )) ) {
		later += match[0];
		selector = selector.replace( Expr.match.PSEUDO, "" );
	}

	selector = Expr.relative[selector] ? selector + "*" : selector;

	for ( var i = 0, l = root.length; i < l; i++ ) {
		Sizzle( selector, root[i], tmpSet );
	}

	return Sizzle.filter( later, tmpSet );
};

// EXPOSE

window.Sizzle = Sizzle;

})();

Selector.findChildElements = function(element, expression){  
    expression = expression.join(", ");  
    var results = Sizzle(expression, element);  
    if(results.length > 0){  
        for(var i=0; i < results.length; i++){  
            results[i] = Element.extend(results[i]);  
        }  
    }  
    return results;  
}; 

/*
Class: SplitPane
	Separates two divs with a draggable divider that you can use to resize the divs, kind of like
	a frame but without using frames! The two divs should be siblings, that is they should both
	have the same parent. You can have an arbitrary number of such siblings separated using different
	instances of this class, i.e. you can have any number of columns separated by a draggable divider
	that alows you to resize them at will.
	
	You can ask to be notified when the following events occur
	
	- A drag starts.
	- A drag occurs.
	- A drag ends.
	
	This allows you to perform any housekeepng not already performed for you.
	
	You can disable the	resizing.
	
	You can ask an instance to serialize itself as an HTTP POST argument list, this is useful in
	combination with onEnd hooking to save the current div sizes on the server.
	
NOTE:
	In IE all parent divs must have a height other than 'auto'.
	div1 and div2 should probably have overflow=hidden set.
*/
var SplitPane = Class.create();

/*
property: SplitPane.cache
	Holds all instances of SplitPane. Used to delay intialization until Window.onLoad().
*/
SplitPane.cache = new Array();
SplitPane.cacheIndex = 0;
SplitPane.handleWidth = 4;	// Width of the handle

SplitPane.prototype = {
	/*
	Constructor: intialize
	
	parameters:
		div1_id - a div, or the ID of a div notionally on the 'left' of the divider.
		div1_width - the initial width of div1 as a percentage of its parent's width
		div2_id - a div, or the ID of a div notionally on the 'right' of the divider.
		div2_left - the coordinate of the left edge of div2 relative to the parent div as a percentage.
		div2_width - the initial width of div2 as a percentage of its parent's width.
		options - an associative array of optional arguments which include
		
	options:
		onStart - a function to be called when a drag of the divider starts.
		onDrag - a function to be called when a drag occurs.
		onEnd - a function to call when a drag ends.
		active - if true then resizing can occur. If false then the two divs are set to
			the specified widths and that is that. Defaults to false.
	*/
	initialize: function(div1_id, div1_width, div2_id, div2_left, div2_width, options) {
		this.options = { 
			onStart:    Prototype.emptyFunction,
			onDrag:     Prototype.emptyFunction,
			onEnd:      Prototype.emptyFunction,
			active:		false
		}
		
		Object.extend(this.options, options || {});
		
		this.div1 = $(div1_id);
		this.div2 = $(div2_id);
		this.container = this.div1.parentNode;	// This had better be the same for both divs
		this.div1_width = div1_width > 0.5 ? div1_width : 0.5;	// as a percentage
		this.div2_left = div2_left;		// as a percentage
		this.div2_width = div2_width > 0.5 ? div2_width : 0.5;	// as a percentage
		
		SplitPane.cache[SplitPane.cacheIndex] = this;
		SplitPane.cacheIndex = SplitPane.cacheIndex+1;
	},

	/*
		function: set
			create a divider. If its marked as 'active' then wire it up for events.
	*/
	set: function() {
		Element.makePositioned(this.container);	// Fix IE
		
		// Change widths to percents so that window resizing works
		this.div1.style.width = (this.div1_width -0.1) + "%";
		this.div2.style.width = (this.div2_width -0.1) + "%";
		this.div2.style.left  = (this.div2_left -0.1) + "%";
		
		// David - Make sure 0 width isn't possible
		if (this.div1_width < 0.1) {
		  this.div1.style.width = "0.5%";
		}

                if (this.div2_width < 0.1) {
                  this.div2.style.width = "0.5%";
                }

		// Create a divider and make it a child of container
		this.divider = document.createElement("DIV");
		this.container.appendChild(this.divider);
		this.divider.className="splitpane-divider";
		this.divider.style.position="absolute";
		this.divider.style.width=SplitPane.handleWidth + "px";
		this.divider.style.top="0px";
		this.divider.style.zIndex=4;

		this.containerWidth = this.getWidth(this.container);
		
		this.setDividerX();
		this.setDividerHeight();

		if (this.options.active) {
			this.eventMouseDown = this.startDrag.bindAsEventListener(this);
			this.eventMouseUp   = this.endDrag.bindAsEventListener(this);
			this.eventChangeCursor = this.cursor.bindAsEventListener(this);
			this.eventMouseMove = this.update.bindAsEventListener(this);
	
			Event.observe(this.divider, "mousedown", this.eventMouseDown);
			Event.observe(document, "mouseup", this.eventMouseUp);
			Event.observe(this.divider, "mousemove", this.eventChangeCursor);
			Event.observe(document, "mousemove", this.eventMouseMove);
		}
	},
	
	/*
		function: serialize
			serialize the splitpane in a form suitable to be used in an HTTP request.
		
		serialized values:
			div1 - the id of div1
			div1_left - the left edge of div1 expressed as a percentage of the parent width
			div1_width - the width of div1 expressed as a percentage of the parent width
			div2 - the id of div2
			div1_left - the left edge of div2 expressed as a percentage of the parent width
			div1_width - the width of div2 expressed as a percentage of the parent width
	*/
	serialize: function() {
		return "div1=" + this.div1.id
		+ "&div1_left=" + this.getXPercent(this.div1)
		+ "&div1_width=" + this.getWidthPercent(this.div1)
		+ "&div2=" + this.div2.id
		+ "&div2_left=" + this.getXPercent(this.div2)
		+ "&div2_width=" + this.getWidthPercent(this.div2);
	},
	
	/*
		function: dispose
			unhook from events
	*/
	dispose: function() {
		Event.stopObserving(this.divider, "mousedown", this.eventMouseDown);
		Event.stopObserving(document, "mouseup", this.eventMouseUp);
		Event.stopObserving(this.divider, "mousemove", this.eventChangeCursor);
		Event.stopObserving(document, "mousemove", this.eventMouseMove);
	},
	
	cursor: function(event) {
		this.divider.style.cursor = "e-resize";		
	},

	startDrag: function(event) {
	    if(Event.isLeftClick(event)) {
			this.active = true;
			var offsets = Position.cumulativeOffset(this.divider); 
			
	        this.start_pointer  = [Event.pointerX(event), Event.pointerY(event)];
			this.inset = this.start_pointer[0] - offsets[0];
			this.containerWidth = this.getWidth(this.container);
			this.start_div1_width = this.getWidth(this.div1);
			this.start_div2_left = this.getX(this.div2);
			this.start_div2_width = this.getWidth(this.div2);
			this.start_divider_x = this.getX(this.divider);
			
			Event.stop(event);
			
			this.options.onStart(this, event);
		}
	},

	endDrag: function(event) {
		if (this.active) {
			this.active = false;
			Event.stop(event);
			this.setDividerX();
			this.setDividerHeight();
			this.options.onEnd(this, event);
		}
	},

	update: function(event) {
		if (this.active) {
	        var pointer  = [Event.pointerX(event), Event.pointerY(event)];
			var delta = pointer[0] - this.start_pointer[0];
			var delta_percent = delta * 100.0 / this.containerWidth;
			
			// Calculate new div1 width
			var new_div1_width = this.start_div1_width + delta;
			
			// Limit width of div1
			if (new_div1_width < 0.0) {
				new_div1_width = 0.0;
				delta = -this.start_div1_width;
			}
						
			// Calculate new div2 width (in %)
			var new_div2_width = this.start_div2_width - delta;
			
			// Limit width of div2
			if (new_div2_width < 0.0) {
				new_div2_width = 0.0;
				delta = this.start_div2_width;
				new_div1_width = this.start_div1_width + delta;
			}

			// resize/position the divs
			this.div1.style.width = (new_div1_width * 100.0 / this.containerWidth) + "%";			
			this.div2.style.left  = ((this.start_div2_left + delta) * 100.0 / this.containerWidth) + "%";
			this.div2.style.width = (new_div2_width * 100.0 / this.containerWidth) + "%";
			
			// Set absolute position of divider - fix it up to be a % in endDrag().
			this.divider.style.left = (this.start_divider_x + delta) + "px";
			
			Event.stop(event);
			
			this.options.onDrag(this, event);
		}
	},

	setDividerX: function() {
		// Place the center of 'divider' half way between div1 and div2
		var div1_right = this.getX(this.div1) + this.getWidth(this.div1);
		var l = (((this.getX(this.div2)- div1_right - SplitPane.handleWidth)/2 + div1_right) * 100.0 / this.containerWidth) + "%";
		this.divider.style.left = l;
	},

	setDividerHeight: function() {
		// Set the divider height to the greater of the heights of the two divs
		var h = Math.max(this.getHeight(this.div1), this.getHeight(this.div2));
		this.divider.style.height = h + "px";
	},

	getX: function(el) {
    	return el.x ? el.x : el.offsetLeft;
	},
  
	getXPercent: function(el) {
		var x = "0";
		x = Element.getStyle(el,"left");
		if (x) {
			x = x.replace("%","");	//moz
		}
		
		return x ? parseFloat(x) : 0.0;
	},
  
	getWidthPercent: function(el) {
		var w = "0";
		w = Element.getStyle(el,"width");
		if (w) {
			w = w.replace("%","");	//moz
		}
		
		return w ? parseFloat(w) : 0.0;
	},
  
	getWidth: function(el) {
    	return el.offsetWidth;
	},
  
	getHeight: function(el) {
		if (el.currentStyle){
			return el.offsetHeight;									//ie
		} else {
			return Element.getStyle(el,"height").replace("px","");	//moz
		}
	}
}

SplitPane.setAll = function () {
	for(i=0; i<SplitPane.cache.length; i++){
		SplitPane.cache[i].set();
	}
}

Event.observe(window, "load", SplitPane.setAll);


/*
Class: Scroller
	Adds a scrollbar to a specific div. The scrollbar is implemented using a Script.aculo.us slider.
	The class reparents the original div, creates a slider and ties the reparented div to the slider,
	setting any properties necessary on the divs to make it all work. The scrollbar can be styled using
	CSS. The track of the scrollbar has class 'scroll-track', the thumb has class 'scroll-handle'.
	
properties:
	myIndex - an integer used to generate a unique ID for use in, for example, div ids.
	outerBox - the div that holds the scrollpane + scrollbar
	innerBox - the div that holds the scrollpane
	innerHeight - the height of the inner box.
	viewportHeight - the height of the view onto the scrolled div.
	track - a div that holds the script.aculo.us slider (the scrollbar)
	trackHeight - the height of the slider
	handle - the div for the 'thumb' of the scrollbar
	handleHeight - the height of the thumb
	slider - the script.aculo.us slider itself
	ieDecreaseBy - a fudge factor used when calculating the width of innerBox
	
*/
var Scroller = Class.create();

/*
property: Scroller.ids
	A cache of Scrollers indexed by the ID of the original div.
 */
Scroller.ids = new Object();

/*
property: Scroller.i
	A unique ID generator.
 */
Scroller.i = 0;

Scroller.prototype = {
	/*
	constructor: initialize	
		Wrap the passed div in a scrollpane.
	
	parameters:	
		el - the div to add a scrollbar to.
	 */
  initialize: function(el) {
	  this.outerBox = el;
	  this.decorate();
  },
  
  /*
  function: decorate  
  	create the necessary elements to implement the scrollbar and wire up events.
   */
  decorate: function() {
	Element.makePositioned(this.outerBox); // Fix IE
	
	// Seed a unique ID
	Scroller.i = Scroller.i + 1;
	this.myIndex = Scroller.i;
	
	//wrap the existing content in an intermediate inner box
	this.innerBox = document.createElement("DIV");
	this.innerBox.className="scroll-innerBox";
	Element.makePositioned(this.innerBox);	// Fix IE
	this.innerBox.style.cssFloat=this.innerBox.style.styleFloat='left';	// Need the scrollbar to appear next to the scrollpane
	this.innerBox.innerHTML = this.outerBox.innerHTML;
	this.outerBox.innerHTML="";
	this.outerBox.appendChild(this.innerBox);
	
	//now build a slider, and put it next to the inner box
	this.track=document.createElement("DIV");
	this.track.className="scroll-track";
	Element.makePositioned(this.track);	// Fix IE
	this.track.style.cssFloat=this.track.style.styleFloat='left';	// Need the scrollbar to appear next to the scrollpane
	this.track.id="scroll-track"+Scroller.i;
	this.track.style.display = 'none';
	
	// Save the size of our little window onto the content
	this.viewportHeight = this.getHeight(this.outerBox);
	
	this.trackHeight = this.viewportHeight;
	this.track.style.height=this.trackHeight+"px";
	
	// Now create the 'thumb' of the scrollbar
	this.handle=document.createElement("DIV");
	this.handle.className="scroll-handle";
	this.handle.id="scroll-handle"+Scroller.i;
	
	// Height of thumb is proportional, but minimum height is 10px
	this.innerHeight=this.getHeight(this.innerBox);			
	if (this.innerHeight > 0)
		this.handleHeight = Math.round((this.trackHeight * this.viewportHeight) / this.innerHeight);
	else
		this.handleHeight = 10;
	if(this.handleHeight < 10) this.handleHeight = 10;
	this.handle.style.height = this.handleHeight + "px";
	
	this.track.appendChild(this.handle);
	this.outerBox.appendChild(this.track);
	
	//turn off scrolling on the outer div
	this.outerBox.style.overflow="hidden";
	
	//layout complete.  if you exit here, you get nice looking box with an inactive scroll bar.
	//create the slider functionality
	this.slider = new Control.Slider(this.handle.id, this.track.id, {axis:'vertical',
															minimum:0,
															maximum:this.trackHeight});
	
	//scroll set up is complete. Work through the actual scrolling fuctions
	//run the same function while scrollin, and at the end of scrolling (handles jumping up/down)
	this.slider.options.onSlide = this.slider.options.onChange = this.onChange.bind(this);
	
	// Give the browser 10ms to render the DIVs and resolve their geometry.
    setTimeout(this.resetScrollbar.bind(this, false), 10);
  },
  
  /*
  function: resetScrollbar  
  	Re-calculate the geometry of the scrollbar. Typically called from an event handler.
	
	args:	
		full - if true, re-calculate the geometry of the scrollpane as well as the scrollbar.
   */
  resetScrollbar: function(full) {
	// If its a full reset, set scrollbar to invisible.
	if (full)
		this.track.style.display='none';
	
	//need to get height of innerBox.
	this.innerHeight = this.getHeight(this.innerBox);
	
	this.viewportHeight = this.getHeight(this.outerBox);// Need to refetch height of outerbox too since it might've stretched.
	this.trackHeight = this.viewportHeight;				// One day trackHeight might be different than viewportHeight if we have scroll buttons too.
	this.slider.trackLength = this.trackHeight;			// Reset slider geometry
	this.track.style.height=this.trackHeight+"px";
	
	// Reset thumb geometry
	this.handleHeight = Math.round((this.trackHeight * this.viewportHeight) / this.innerHeight);
	if(this.handleHeight < 10) this.handleHeight = 10;
	this.handle.style.height = this.handleHeight + "px";
	
	// Reset handle height
	this.slider.handleLength = this.handleHeight;
	if (this.handleHeight < this.trackHeight) {
		 // Scrolbar should be displayed.
		 if (Element.getStyle(this.track, "display") == 'none') {
			 // If scrollbar was not previously displayed, we have to squeeze the viewport width by the width of the scrollbar
			this.track.style.display='inline';
			
			//now adjust the size of the inner box to make room for the slider
			//if the outer box has a border on it (common for scroll boxes) we need to compensate for different box models
			//fortunately, mozilla will work by default - so only if IE  has a border do we care.  Which is good, we can only check borders in IE...
			this.ieDecreaseBy=0;
			if (this.outerBox.currentStyle){
				var borderWidth = this.outerBox.currentStyle["borderWidth"].replace("px","");	//no way to isolate left and right border (which is all we care about) so we'll just assume consistent border width
				if(!isNaN(borderWidth)){
					this.ieDecreaseBy=(borderWidth)*2;	//compensate for left and right border
				}
			}
			this.setWidth();
		 }
			
	} else {
		this.track.style.display='none';
	}
  },
  
  /*
  function: setWidth  
  	Set the width of of the scrollpane (aka innerBox).
   */
  setWidth: function() {
	var newWidth = (this.getWidth(this.outerBox) - this.getWidth(this.track) - this.ieDecreaseBy) + "px";
	this.innerBox.style.width = newWidth;
	
	// The sad thing is that all of this might change innerHeight, so need to schedule a refresh
	setTimeout(this.resetScrollbar.bind(this, false), 10);
  },
  
  /*
  function: getHeight  
  	Get the height of the passed element.
	
	args:	
		el - the element to get the height of.
   */
  getHeight: function(el) {
	if (el.currentStyle){
		return el.offsetHeight;									//ie
	}else{
		return Element.getStyle(el,"height").replace("px","");	//moz
	}
  },
  
  /*
  function: getWidth  
  	Get the width of the passed element.
	
	args:	
		el - the element to get the width of.
   */
  getWidth: function(el) {
	var w = "0";
	if (el.currentStyle){
		w = el.offsetWidth;									//ie
	} else {
		w = Element.getStyle(el,"width");
		if (w) {
			w = w.replace("px","");	//moz
		}
	}
	
	return w;
  },
  
  /*
  function: onChange  
  	Called when the script.aculo.us slider has changed (i.e. when it has been dragged). Scroll the inner box.
	
	args:	
		val - not used.
   */
  onChange: function(val) {
	if(this.track){
		//assume 100 ticks in the scrollbar
		//for each tick need to move:  The amount the inner box overruns the outer box, divided by 100
		var moveRatio = (this.innerHeight - this.getHeight(this.outerBox))/100;
		//move the box up (negative) for every TickVal, move the box by moveRatio
		this.innerBox.style.top = (val*100*moveRatio*-1) + "px";
	}
  }
}

/*
function: Scroller.setAll
	Search for divs of the class 'makeScroll' and wrap them in a Scroller.
 */
Scroller.setAll = function () {
	//get all the boxes we want to scroll
	var sliderBoxes = document.getElementsByClassName("makeScroll");
	//build scroll functionality for each scrollable box
	for(i=0; i<sliderBoxes.length; i++){
		Scroller.ids[sliderBoxes[i].id] = new Scroller(sliderBoxes[i]);
	}
}

/*
function: Scroller.reset
	If the passed element has class 'makeScroll', wrap it in a Scroller.
 */
Scroller.reset = function (body_id) {
	if ($(body_id).className.match(new RegExp("(^|\\s)makeScroll(\\s|$)"))) {
		Scroller.ids[body_id] = new Scroller($(body_id));
	}
}

/*
property: Scroller.updateAll
	Reset all of the scrollbars.
 */
Scroller.updateAll = function () {
	for (var key in Scroller.ids) {
		Scroller.ids[key].resetScrollbar(true);
	}
}

/*
	Hook up some global event handlers.
 */
Event.observe(window, "load", Scroller.setAll);
Event.observe(window, "resize", Scroller.updateAll);

// accordion.js v2.0
//
// Copyright (c) 2007 stickmanlabs
// Author: Kevin P Miller | http://www.stickmanlabs.com
// 
// Accordion is freely distributable under the terms of an MIT-style license.
//
// I don't care what you think about the file size...
//   Be a pro: 
//	    http://www.thinkvitamin.com/features/webapps/serving-javascript-fast
//      http://rakaz.nl/item/make_your_pages_load_faster_by_combining_and_compressing_javascript_and_css_files
//

/*-----------------------------------------------------------------------------------------------*/

if (typeof Effect == 'undefined') 
	throw("accordion.js requires including script.aculo.us' effects.js library!");

var accordion = Class.create();
accordion.prototype = {

	//
	//  Setup the Variables
	//
	showAccordion : null,
	currentAccordion : null,
	duration : null,
	effects : [],
	animating : false,
	
	//  
	//  Initialize the accordions
	//
	initialize: function(container, options) {
	  if (!$(container)) {
	    throw(container+" doesn't exist!");
	    return false;
	  }
	  
		this.options = Object.extend({
			resizeSpeed : 8,
			classNames : {
				toggle : 'accordion_toggle',
				toggleActive : 'accordion_toggle_active',
				content : 'accordion_content'
			},
			defaultSize : {
				height : null,
				width : null
			},
			direction : 'vertical',
			onEvent : 'click'
		}, options || {});
		
		this.duration = ((11-this.options.resizeSpeed)*0.15);

		var accordions = $$('#'+container+' .'+this.options.classNames.toggle);
		accordions.each(function(accordion) {
			Event.observe(accordion, this.options.onEvent, this.activate.bind(this, accordion), false);
			if (this.options.onEvent == 'click') {
			  accordion.onclick = function() {return false;};
			}
			
			if (this.options.direction == 'horizontal') {
				var options = $({width: '0px', display: 'none'});
			} else {
				var options = $({height: '0px', display: 'none'});			
			}
			
			this.currentAccordion = $(accordion.next(0)).setStyle(options);			
		}.bind(this));
	},
	
	//
	//  Activate an accordion
	//
	activate : function(accordion) {
		if (this.animating) {
			return false;
		}
		
		this.effects = [];
	
		this.currentAccordion = $(accordion.next(0));
		this.currentAccordion.setStyle({
			display: 'block'
		});		
		
		this.currentAccordion.previous(0).addClassName(this.options.classNames.toggleActive);

		if (this.options.direction == 'horizontal') {
			this.scaling = $({
				scaleX: true,
				scaleY: false
			});
		} else {
			this.scaling = $({
				scaleX: false,
				scaleY: true
			});			
		}
			
		if (this.currentAccordion == this.showAccordion) {
		  this.deactivate();
		} else {
		  this._handleAccordion();
		}
	},
	// 
	// Deactivate an active accordion
	//
	deactivate : function() {
		var options = $({
		  duration: this.duration,
			scaleContent: false,
			scaleX: this.scaling.scaleX,
			scaleY: this.scaling.scaleY,
			transition: Effect.Transitions.sinoidal,
			queue: {
				position: 'end', 
				scope: 'accordionAnimation'
			},
			scaleMode: { 
				originalHeight: this.options.defaultSize.height ? this.options.defaultSize.height : this.currentAccordion.scrollHeight,
				originalWidth: this.options.defaultSize.width ? this.options.defaultSize.width : this.currentAccordion.scrollWidth
			},
			afterFinish: function() {
				this.showAccordion.setStyle({
          height: 'auto',
					display: 'none'
				});				
				this.showAccordion = null;
				this.animating = false;
			}.bind(this)
		});    

    this.showAccordion.previous(0).removeClassName(this.options.classNames.toggleActive);
    
		new Effect.Scale(this.showAccordion, 0, options);
	},

  //
  // Handle the open/close actions of the accordion
  //
	_handleAccordion : function() {
		var options = $({
			sync: true,
			scaleFrom: 0,
			scaleContent: false,
			scaleX: this.scaling.scaleX,
			scaleY: this.scaling.scaleY,
			transition: Effect.Transitions.sinoidal,
			scaleMode: { 
				originalHeight: this.options.defaultSize.height ? this.options.defaultSize.height : this.currentAccordion.scrollHeight,
				originalWidth: this.options.defaultSize.width ? this.options.defaultSize.width : this.currentAccordion.scrollWidth
			}
		});
		
		this.effects.push(
			new Effect.Scale(this.currentAccordion, 100, options)
		);

		if (this.showAccordion) {
			this.showAccordion.previous(0).removeClassName(this.options.classNames.toggleActive);
			
			options = $({
				sync: true,
				scaleContent: false,
				scaleX: this.scaling.scaleX,
				scaleY: this.scaling.scaleY,
				transition: Effect.Transitions.sinoidal
			});
			
			this.effects.push(
				new Effect.Scale(this.showAccordion, 0, options)
			);				
		}
		
    new Effect.Parallel(this.effects, {
			duration: this.duration, 
			queue: {
				position: 'end', 
				scope: 'accordionAnimation'
			},
			beforeStart: function() {
				this.animating = true;
			}.bind(this),
			afterFinish: function() {
				if (this.showAccordion) {
					this.showAccordion.setStyle({
						display: 'none'
					});				
				}
				$(this.currentAccordion).setStyle({
				  height: 'auto'
				});
				this.showAccordion = this.currentAccordion;
				this.animating = false;
			}.bind(this)
		});
	}
}
	

// Weebly Javascript base file
// Copyright 2005-2006 Weebly

var included_files = new Array();
var initFiles = new Array();
var buildTime = 0;
var Weebly = { };
var userID = '';

function init(footprint, build_time) {

   initFiles = footprint;
   buildTime = build_time;

  // Library Includes
  if( isIn(footprint, 'base')) {
    //include_once('/weebly/libraries/prototype.js');
    //include_once('/weebly/libraries/scriptaculous.js');
  }
  if( isIn(footprint, 'utils_ajax')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_utils_ajax.js'/*)tls*/);
  }
  if( isIn(footprint, 'utils')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_utils.js'/*)tls*/);
  }
  if( isIn(footprint, 'pages')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_pages.js'/*)tls*/);
  }
  if( isIn(footprint, 'signup')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_signup.js'/*)tls*/);
  }
  if( isIn(footprint, 'user_home')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_user_home.js'/*)tls*/);
    include_once('/weebly/libraries/behaviour.js');
    include_once(/*tls(*/'/weebly/libraries/weebly_user_home_style.js'/*)tls*/);
  }
  if( isIn(footprint, 'colorchooser')) {
    include_once('/weebly/libraries/yahoo.color.js');
    include_once('/weebly/libraries/colorpicker.js');
  }
  if( isIn(footprint, 'linker')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_linker.js'/*)tls*/);
  }
  if( isIn(footprint, 'cache')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_cache.js'/*)tls*/);
  }
  if( isIn(footprint, 'main')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_main.js'/*)tls*/);
    include_once(/*tls(*/'/weebly/libraries/weebly_initmain.js'/*)tls*/);
    include_once('/weebly/libraries/behaviour.js');
    include_once(/*tls(*/'/weebly/libraries/weebly_mainstyle.js'/*)tls*/);
    include_once(/*tls(*/'/weebly/libraries/weebly_custom_themes.js'/*)tls*/);
    include_once(/*tls(*/'/weebly/libraries/nested_sortable.js'/*)tls*/);
    include_once(/*tls(*/'/weebly/libraries/weebly_page_manager.js'/*)tls*/);
    include_once(/*tls(*/'/weebly/libraries/flyout_menus.js'/*)tls*/);
    include_once(/*tls(*/'/weebly/libraries/weebly_form.js'/*)tls*/);
    include_once('/weebly/libraries/weebly_uploader_plain.js');
  }

  if( isIn(footprint, 'editbox')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_editbox.js'/*)tls*/);
  }
  if( isIn(footprint, 'absolute')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_absolute.js'/*)tls*/);
  }
  if( isIn(footprint, 'corners')) {
    include_once('/weebly/libraries/corners.js');
  }
  if( isIn(footprint, 'feedback')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_feedback.js'/*)tls*/);
  }
  if( isIn(footprint, 'signup')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_signup.js'/*)tls*/);
  }
  if( isIn(footprint, 'elements')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_elements.js'/*)tls*/);
  }
  if( isIn(footprint, 'imageutils')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_image_utils.js'/*)tls*/);
  }
  if( isIn(footprint, 'uploads')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_uploads.js'/*)tls*/);
  }
  if( isIn(footprint, 'storage')) {
    include_once('/weebly/libraries/weebly_storage.js');
  }
  if( isIn(footprint, 'calendarview')) {
    include_once('/weebly/libraries/calendarview.js');
  }
  if( isIn(footprint, 'myspace')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_myspace.js'/*)tls*/);
  }
  if( isIn(footprint, 'adsense')) {
    include_once(/*tls(*/'/weebly/libraries/weebly_adsense.js'/*)tls*/);
  }

}

function initTwo() {

  if( isIn(initFiles, 'colorchooser')) {
  }
  if( isIn(initFiles, 'main')) {
  }

}

function include_dom(script_filename) {
    //var html_doc = document.getElementsByTagName('head').item(0);
    //var js = document.createElement('script');
    //js.setAttribute('language', 'javascript');
    //js.setAttribute('type', 'text/javascript');
    //js.setAttribute('src', script_filename);
    //html_doc.appendChild(js);
    document.write('<script type="text/javascript" src="'+script_filename+'?buildTime='+buildTime+'"></script>');
    return false;
}

function include_once(script_filename) {
    //alert("Including: "+script_filename);
    if (!isIn(included_files, script_filename)) {
        included_files[included_files.length] = script_filename;
        include_dom(script_filename);
    }
}

function isIn(myArray, matchVar) {

    for (i=0; i < myArray.length; i++) { 
      if (matchVar == myArray[i]) { 
	return i; 
      }
    }
    return false;

}


Weebly.Restrictions = {
    settings : {},
    services : [],
    defaultSource : 'main',
    defaultLevel : 'all',
    proLevel : 'Weebly.proAccount',
    ecommerceLevel : 'Weebly.eCommerce',

    hasAccess : function(id){
        if(!this.settings[id]){
            return true;
        }
        var currentSource = this.settings[id][source] ? source : this.defaultSource;
        var level = this.settings[id][currentSource]['required_level'];
        if(!level || level === this.defaultLevel){
            return true;
        }
        return this.hasService(level);
    },

    hasService : function(service){
        return this.services.indexOf(service) != -1;
    },

    accessValue : function(id){
        if(!this.settings[id]){
            return true;
        }
        var currentSource = this.settings[id][source] ? source : this.defaultSource;
        var value = this.settings[id][currentSource]['value'];
        if(!value){
            return '';
        }
        return value;
    },

    accessLevel : function(id){
        if(!this.settings[id]){
            return false;
        }
        var currentSource = this.settings[id][source] ? source : this.defaultSource;
        var level = this.settings[id][currentSource]['required_level'] ? this.settings[id][currentSource]['required_level'] : this.settings[id][this.defaultSource]['required_level'];
        return level;
    },

    isProElement : function(elId){
        if(!this.settings[elId]){
            return false;
        }
        var currentSource = this.settings[elId][source] ? source : this.defaultSource;
        //console.log(currentSource);
        var level = this.settings[elId][currentSource]['required_level'] ? this.settings[elId][currentSource]['required_level'] : this.settings[elId][this.defaultSource]['required_level'];
        //console.log(level && level === this.proLevel);
        return level && level === this.proLevel;
    },

    requiredService : function(id){
        if(!this.settings[id]){
            return 'all';
        }
        var currentSource = this.settings[id][source] ? source : this.defaultSource;
        var level = this.settings[id][currentSource]['required_level'];
        if(!level){
            return 'all';
        }
        return level;
    },

    requiresUpgrade : function(id){
        var level = this.accessLevel(id);
        return level && level !== this.defaultLevel;
    },

    addService: function(service){
        this.services.push(service);
    },

    hasNewElementAccess: function(){
        var newElements = $$('#secondlist .outside_top form');
        if(newElements.size() == 1){
            var elementID = newElements[0].idfield.value.replace(/[^\d]/g, '');
            return this.hasAccess(elementID);
        }
        return true;
    }
}

var settingAnimations = 0;
var settingQuickExport= 0;
var settingTooltips   = 0;
var uploadInProgress  = 0;
var currentTheme      = '';
var ajax 	      = '/weebly/getElements.php';
var ajaxStatusCheckTimeout = 4000;
var ajaxStatusTimeoutGrowthFactor = 1;
Weebly.ajaxLog = [];

    var myGlobalHandlers = {
        onCreate: function(ajax, t){
            t.request.times = {start:new Date().getTime()};
            if(window.swfu && swfu.currentUpload && $('upload'+swfu.currentUpload)){
                t.request.concurrentUpload = true;
            }
		if(!ajax.options.bgRequest) { startWait(); }
        if(ajax.parameters && ajax.parameters.pos){
            Weebly.ajaxLog.push(ajax.parameters.pos);
            if(Weebly.ajaxLog.size() > 10){
                Weebly.ajaxLog.shift();
            }
        }

                // Set Weebly-Site-ID header
                if (typeof(currentSite) != "undefined") { 
                  if (typeof(ajax.options.requestHeaders) == "object") {
                    ajax.options.requestHeaders['Weebly-Site-ID'] = currentSite; 
                  } else {
                    ajax.options.requestHeaders = {'Weebly-Site-ID': currentSite}; 
                  }
                }
                if(!ajax.options.requestHeaders['x-ajax-request-id']){
                    var ajax_request_id = new Date().getTime() + '' + Math.floor(Math.random()*999);
                    ajax.options.requestHeaders['x-ajax-request-id'] = ajax_request_id;
                }
                if(ajax.options.isRetry && ajax.options.previouslyAborted){
                    ajax.options.requestHeaders['x-ajax-abort-retry'] = ajaxStatusCheckTimeout;
                }
                
                if(!ajax.options.isRetry && !Prototype.Browser.IE6){
                    setTimeout(function(){checkAjaxRequestStatus(ajax)}, ajaxStatusCheckTimeout);
                }
        },
        
        onLoading: function(ajax, t){
            if(t.request.times && t.request.times.start){
                t.request.times.initialized = new Date().getTime() - t.request.times.start;
            }
        },
        
        onLoaded: function(ajax, t){
            if(t.request.times && t.request.times.start){
                t.request.times.sent = new Date().getTime() - t.request.times.start;
                if(ajax.options.isRetry && t.request.times.sent > (ajaxStatusCheckTimeout/2)){
                    ajaxStatusCheckTimeout = ajaxStatusCheckTimeout + ajaxStatusCheckTimeout * ajaxStatusTimeoutGrowthFactor;
                    ajaxStatusTimeoutGrowthFactor = ajaxStatusTimeoutGrowthFactor * .9;
                }
            }
        },
        
        onInteractive: function(ajax, t){
            if(t.request.times && t.request.times.start && !t.request.times.response){
                t.request.times.response = new Date().getTime() - t.request.times.start;
            }
        },

        onComplete: function(ajax, t) {
        
                if(t.request.times && t.request.times.start){
                    t.request.times.complete = new Date().getTime() - t.request.times.start;
                }
                if(Ajax.activeRequestCount == 0){
                        endWait();
                }
                if(ajax.isRetriable()){
                    ajax.retry();
                } else{
                    handleLogout(t);
                }
        }
    };

    Ajax.Responders.register(myGlobalHandlers);

    function startWait()
    {
                try {
		  $('pleaseWait').style.display = 'block';
                } catch(e) { }
    }

    function endWait()
    {
                try {
		  $('pleaseWait').style.display = 'none';
                } catch(e) { }
    }

    function setSetting(setting, value) {
	if (typeof(value) == "string" && value.match(/{/)) {
	  value = value.replace(/^'/, '');
	  value = value.replace(/'$/, '');
	}
	eval(setting+" = "+value+";");
    }

    function handleLogout(t) {

        // Check if user is logged in; if not, redirect.
        //---- Note: This javascript redirect is for convenience purposes for the user
        //---- At this point, the user is fully logged out of the system from the
        //---- Server's perspective, so it will refuse to furnish any additional data.

        var header = '';
        try {
          header = t.getHeader("Weebly-Auth-Msg");
        } catch (e) { }

        if (header.match("not-logged-in")) { window.onbeforeunload = null; document.location="/?session-expired=1"; }
        else if (header.match("database-error")) { window.onbeforeunload = null; document.location="/?difficulties=1"; }
        else if (header.match("account-deleted")) { window.onbeforeunload = null; document.location="logout.php"; }
        else if (header.match("refresh-build")) { 
	  if (!(typeof(currentBlog) != "undefined" && currentBlog.postId && currentBlog.postId == 1)) {
	    window.onbeforeunload = null;
	    refreshMe(); 
	  }
	} else if (header.match("maintenance-soon")) {
	  var maintLength = header.match(/maintenance-soon\(([^\)]+)\)/);
	  window.onbeforeunload = null;
	  maintenanceSoon(maintLength[1]);
	}

    }

    function maintenanceSoon(maintLength) {

	$('maintenanceLength').innerHTML = maintLength;
	Element.show('maintenanceDiv');

    }

    function refreshMe() { 

	Element.show('refreshingDiv');
	Element.show('grayedOut');
	setTimeout("window.location.reload();", 4000);

    }

    function errFunc(t) {
        if(t.request.isRetriable()) return;
        
        showError(/*tl(*/'Weebly encountered an error. Please try your last request again.'/*)tl*/, t);
    }
    
    function exceptionFunc(t, exception, xx) {
        if(t.isRetriable()) return;
        
		if (t && (!t.getStatus || !t.getStatus() || t.getStatus() < 100 || t.getStatus() > 500)) { // will only retry if xhr or network related problem
			try {
				var options = t.options || {},
					retryCount = options._retryCount || 0;
				if (retryCount <= 1) { // will retry a max of 2 times
					options._retryCount = ++retryCount;
					new Ajax.Request(t.url, options);
				}else{
					showError(/*tl(*/'Weebly encountered an exception. Please try your last request again.<br/><br/>'/*)tl*/ + exception.message);
					// ^ will also report error to stats
					setInterfaceEnabled(); // enable interface, in case it was disabled
				}
			}
			catch (e) { }
		}
		else if (window.console && console.log) {
			console.log(exception); // couldn't throw it for some reason, so console.log it if possible
		}
    }
    
    function checkAjaxRequestStatus(ajax){
        if(!ajax._complete && Prototype.Browser.IE && !ajax.times.sent){
            ajax.abort();
        }
    }
    
    Ajax.Request.addMethods({
        abort : function() {
            if(this._complete) return;
        
            // avoid MSIE/Mozilla calling other event handlers when aborted
            this.transport.onreadystatechange = Prototype.emptyFunction;
            this.transport.abort();
            this._complete = true;
            this.aborted = true;
        
            var response = new Ajax.Response(this);
        
            ['Abort', 'Complete'].each(function(state) {
              try {
                (this.options['on' + state] || Prototype.emptyFunction)(response, response.headerJSON);
                Ajax.Responders.dispatch('on' + state, this, response, response.headerJSON);
              } catch (e) {
                this.dispatchException(e);
              }
            }, this);
        },
        
        retry :  function(force){
            if (force || this.isRetriable()) {
                var options = this.options || {};
                options._retryCount = (options._retryCount || 0) + 1;
                options.isRetry = true;
                options.previouslyAborted = this.aborted ? true : false;
                new Ajax.Request(this.url, options);
            }
        },
        
        isRetriable: function(){
            if(!this._complete){
                return false;
            }
            
            var status = this.getStatus();
            var options = this.options || {};
            var maxRetries = options.maxRetries || 1;
            var retryCount = options._retryCount || 0;
            return (status == 408  || status > 10000 || this.aborted) && retryCount < maxRetries;
        }
    });



var currentBlog	      = {postId: 0, newPost: 0, title: '', categories: '', skipToComments: '', saving: 0};

function done() {

	if (!$('focusMe')) {
	  var fm = document.createElement('input');
	  fm.id = "focusMe";
	  fm.style.border = "0";
	  fm.style.height = "0px";
	  fm.style.width  = "0px";
	  fm.style.overflow = "hidden";
	  fm.style.position = "absolute";
	  fm.style.top = "0px";
	  fm.style.width = "0px";
	  document.body.appendChild(fm);
	}

	$('focusMe').focus();
	return false;

   }


   var preloadedImages = Array();

   var lastEventID;
   lastEventID = Array();
   var tipsShown = 20;
   function showTip(text, posElement, color, tipID, animate, width) {

	if (!tipID) { tipID = tipsShown; tipsShown++; }

	var element = $(posElement);
	if (element && typeof(element.style) == "undefined") { return; }

	if (animate ==1) { width = 200; } else if (!width) { width = 400; }
	
    var pos = Position.cumulativeOffset(element);
	var dimensions = Element.getDimensions(element);
	
	var left = pos[0]+10;
	var triangleLeft = 16;
	var bodyWidth = $(document.body).getWidth();
	if (left + width > bodyWidth - 10) {
		var leftDiff = (left + width) - (bodyWidth - 10);
		left -= leftDiff;
		triangleLeft += leftDiff;
	}

	if (color == 'y') {
		new Insertion.Top('tips',
			"<div id='tip"+tipID+"' style='position: absolute; display: none; z-index: 25; width: "+width+"px; text-align:left'>" +
			"<iframe class='hiddenIframe' style='z-index: -1; filter: mask(); position: absolute; width: "+width+"px; height: 46px;'></iframe>" +
			"<div style='position: absolute; top:12px; right:6px; cursor: pointer;'>x</div>" +
			"<div style=\"background: url('http://"+editorStatic+"/weebly/images/tooltip2.gif') no-repeat "+triangleLeft+"px 0; height: 12px; overflow: hidden; position:relative; top:1px\"> &nbsp; </div>" +
			"<div style='text-align:left; border:1px solid #ccc; background: #FFFFCC; padding: 10px; font-family: verdana; font-weight: bold; font-size: 12px;'>"+text+"</div>" +
			"</div>"
		);
	} else {
		new Insertion.Top('tips',
			"<div id='tip"+tipID+"' style='position: absolute; display: none; z-index: 25; width: "+width+"px; text-align:left'>" +
			"<div style='position: absolute; top:12px; right:6px; cursor: pointer;'>x</div>" +
			"<div style=\"background: url('http://"+editorStatic+"/weebly/images/tooltip2-w.gif') no-repeat "+triangleLeft+"px 0; height: 12px; overflow: hidden; position:relative; top:1px\"> &nbsp; </div>" +
			"<div style='text-align: left; border: 1px solid #ccc; background: #FFFFFF; padding: 10px; font-family: verdana; font-weight: bold; font-size: 12px;'>"+text+"</div>" +
			"</div>"
		);
	}
	
	var top = (pos[1]+dimensions.height);

        Element.setStyle($('tip'+tipID), { position: 'absolute', top: top+'px', left: left+'px' });

        if (animate == 1) {
	  Element.show('tip'+tipID); 
	} else if (animate == 2) {
	  Effect.Appear('tip'+tipID); 
	} else { 
	  setTimeout("appearTip("+tipID+", "+left+", "+top+", "+dimensions.height+");", 500);
	}
   }
   function appearTip(tipID, left, top, dimensionsHeight) {
	Element.setStyle($('tip'+tipID), { position: 'absolute', left: left+'px', top: ((getInnerHeight()/2)+dimensionsHeight)+'px' });
	Element.show('tip'+tipID);
	new Effect.Move('tip'+tipID, {x: left, y: top, mode: 'absolute', transition: Effect.Transitions.Bounce});
   }
   function hideTip(tipID, animate) {

	if (!tipID || !$(tipID)) { return; }
	var tipPointer = tipID;
	//console.log($(tipID).firstChild.tagName);
	if($(tipID).firstChild.tagName == "IFRAME") {
	  $(tipID).removeChild($(tipID).firstChild);
	}
        if (animate == 1) { Element.hide($(tipID)); $('tips').removeChild($(tipID)); } else { Effect.Fade($(tipID), {afterFinish: function() { if ($(tipPointer) && $(tipPointer).parentNode == $('tips')) { $('tips').removeChild($(tipPointer)); }} }); }

   }

   function hideAllTips() {

        // Hide all tips
        for (var x=0; x < $('tips').childNodes.length; x++) {
          hideTip($('tips').childNodes[x]);
        }

   }

   	function checkFlash() 
	{
		if ((navigator.appName == "Microsoft Internet Explorer" && navigator.appVersion.indexOf("Mac") == -1 && navigator.appVersion.indexOf("3.1") == -1) || (navigator.plugins && navigator.plugins["Shockwave Flash"]) || navigator.plugins["Shockwave Flash 2.0"]){
			//showError2("<div style=\"text-align:center;\">Adobe Flash Player is required in order to use Weebly.<br/><br/><a target='_new' href='http://www.adobe.com/go/getflashplayer'><img style='border:0;' onClick='Effect.Fade(\"error2\");' src='http://www.adobe.com/macromedia/style_guide/images/160x41_Get_Flash_Player.jpg'><a/></div>");
		} 
		else {
			showError("<div style=\"text-align:center;\">"+/*tl(*/"Adobe Flash Player is required in order to use Weebly."/*)tl*/+"<br/><br/><a target='_new' href='http://www.adobe.com/go/getflashplayer'><img style='border:0;' onClick='Effect.Fade(\"error2\");' src='http://"+editorStatic+"/weebly/images/get_flash.jpg'><a/></div>");
		}
   	}

   function showEvent(eventName, dontPush, eventElement, width) {
   
	if (userEvents[eventName]) { return; }

	var eventData = {};
	if (siteType == 'myspace') {

	  eventData = {
            'showThemeOptions': {
                text: /*tl(*/'To get started, choose a color palette.'/*)tl*/
            },

	    'tab_edit': {
		text: /*tl(*/'You can drag these widgets and applications on to your profile.'/*)tl*/,
		eventElement: $('elementlist').firstChild
	    }

	  }

	} else {

	  eventData = {
	    'first_tip': {
		text: /*tl(*/'Great! Now drag one of these elements on to your page.'/*)tl*/,
		eventElement: $('elementlist').firstChild
	    },

	    'tab_themes': {
		text: /*tl(*/'Hover over designs to get a preview, and click to select a design.'/*)tl*/,
		eventElement: $('themePictures').firstChild
	    },

	    'addElement': {
		text: /*tl(*/'Nice! Now, click here to change your design.'/*)tl*/,
		eventElement: $('weebly_tab_themes')
	    },

	    'showProperties': {
		text: /*tl(*/'This menu bar lets you change things about the element you clicked.'/*)tl*/,
		eventElement: $('menuBarDiv').childNodes[0]
	    },

	    'tab_pages': {
		text: /*tl(*/'Click on "New Page" to add a page, or "New Blog" to add a blog to your site.'/*)tl*/,
		eventElement: $('newPageButton')
	    },

	    'newPage': {
		text: /*tl(*/'Title your page here. Once you\'ve done that, click "Save" below.'/*)tl*/
	    },

	    'updatePages': {
		text: /*tl(*/'Nice job, Click "Publish" when you want to publish or update your site online.'/*)tl*/,
		eventElement: $('publishButton')
	    },

	    'selectTheme': {
		text: /*tl(*/'Looking good. Click here to add more pages.'/*)tl*/,
		eventElement: $('weebly_tab_pages')
	    },

	    'twoColumn' : {
		text: /*tl(*/'Drag elements into either column.'/*)tl*/
	    },
	    
	    'navMore' : {
	    text: "Your other pages have been placed here.<br />We recommend creating sub-pages under the <a href='#' onclick=\"Pages.go('pagesMenu');return false;\">Pages</a> tab or you can disable the \"more\" feature under <a href='#' onclick=\"Pages.go('displaySiteSettings',1);return false\">Settings</a>.",
	    eventElement: $('weebly-nav-more-a'),
	    width: 400
	    },
	    
	    'secondPage': {
	    text: "If you'd like, drag pages left and right to create a hierarchy.",
	    width: 300
	    }

	  }

	}

	if (eventName == 'navMore' || eventName == 'secondPage') {
	  setTimeout(function() {
	  	  var eventEl = eventElement ? eventElement : eventData[eventName].eventElement;
		  width = eventData[eventName].width ? eventData[eventName].width : width;
		  showTip(eventData[eventName].text, eventEl, 'y', null, null, width);
	  }, 500);
	}
	else if (eventData[eventName] && settingTooltips == 0) {
	  hideAllTips();
	  var eventEl = eventElement ? eventElement : eventData[eventName].eventElement;
	  width = eventData[eventName].width ? eventData[eventName].width : width;
	  showTip(eventData[eventName].text, eventEl, 'y', null, null, width);
	}
	
	userEvents[eventName] = 1;
	if (!dontPush) {
	  new Ajax.Request(ajax, {parameters:'pos=doevent&event='+escape(eventName)+'&cookie='+document.cookie});
	  fireTrackingEvent("Event", eventName);
	}

   }

   Weebly.trackingArray = Array();

   function fireTrackingEvent(category, action, optional_label, optional_value) {

	try {

	  optional_value = optional_value ? optional_value : 0;

	  if (!doTrackingEvent(category, action, optional_label, optional_value)) {
	    throw("Did not process tracking event"); 
	  }

	  if (Weebly.trackingArray.length > 0) {
	    for (var i = 0; i < Weebly.trackingArray.length; i++) {
	      var c = Weebly.trackingArray[i];
	      doTrackingEvent(c.category, c.action, c.optional_label, c.optional_value);
	    }

	    Weebly.trackingArray = Array();
	  }
	} catch (e) {
	  Weebly.trackingArray.push({'category': category, 'action': action, 'optional_label': optional_label, 'optional_value': optional_value});
	}

   }

   function doTrackingEvent(category, action, optional_label, optional_value) {

     try {

       pageTracker._trackEvent(category, action, optional_label, parseInt(optional_value));
       mpmetrics.track(category, {'type': action});

     } catch (e) { return false; }

     return true;

   }

   function fireTransactionEvent(orderId, sku, price, affiliation, category) {

	  try {
		pageTracker._addTrans(orderId, affiliation, price, "", "", "", "", "");
		pageTracker._addItem(orderId, sku, sku, category, price, "1");
		pageTracker._trackTrans();
                mpmetrics.track("Purchase "+category);
	  } catch (e) { }

   }



	var errorDialog;

    function showError(message, t, dontTrack) {
        $('red-error-text').update(message);
        errorDialog = new Weebly.Dialog($('red-error'), {inner_class:'weebly-dialog-inner-red'});
        errorDialog.open();
        if (!dontTrack) {
			fireTrackingEvent("WeeblyError", "Error", message);
		}
        var params = 'pos=oopserror&message='+message;
        if(typeof(t) === 'object'){
            params += '&ajax_request=' + encodeURIComponent(t.request.body.match(/^.*?&cookie/)[0]);
            params += '&ajax_response=' + encodeURIComponent(t.responseText);
            params += '&ajax_status='+t.status;
            params += '&server='+t.getHeader('X-Host');
            if(t.request.times){
                params += '&ajax_start='+t.request.times.start;
                params += '&ajax_initialized='+t.request.times.initialized;
                params += '&ajax_sent='+t.request.times.sent;
                params += '&ajax_response_start='+t.request.times.response;
                params += '&ajax_complete='+(new Date().getTime() - t.request.times.start);
                params += '&current_upload='+(t.request.concurrentUpload ? 1 : 0);
            }
        }
        new Ajax.Request(ajax, {parameters:params+'&cookie='+document.cookie, bgRequest:true});
    }
    
    function showError2(message) {
        $('errorText2').innerHTML = message;
        Effect.Appear('error2');
    }
    
    function hideError() {
    	if (errorDialog) {
    		errorDialog.close();
    	}
    }
    
    
    
    function showWarning(message) {
        $('warning-text').update( message );
        $('warning').show();
    }

    function hoverOn(hoverID, type) {
	if (isIn(lastEventID, hoverID)) { } else {
	  if (settingTooltips == 1) {
	    var text;
	    if (type == 1) { text = /*tl(*/"<b>Double click</b> to edit."/*)tl*/; }
	    if (type == 2) { text = /*tl(*/"<b>Click</b> to change pages.\n<b>Drag</b> to rearrange order.\n<b>Double Click</b> to edit."/*)tl*/; }
	    if (type == 3) { text = /*tl(*/"<b>Drag</b> to rearrange order.\n<b>Drag to Trash</b> to delete."/*)tl*/; }
	    showTip(text, hoverID, 'y', hoverID, 1);
	    lastEventID.push(hoverID);
	  }
	  //var element = document.getElementById(hoverID);
	  //element.className = element.className + "-hover";
	}
    }

    function hoverOff(hoverID) {
	if (settingTooltips == 1) {
	  hideTip('tip'+hoverID);
	}
        //var element = document.getElementById(hoverID);
        //element.className = element.className.replace("-hover","");
	if(isIn(lastEventID, hoverID)) { lastEventID.splice(isIn(lastEventID, hoverID), 1); }
    }

    function preloadImages(images) {

	//console.log("preloadImages: "+images);

	var imagesArray = images.split(",");
	for (var x = 0; x < imagesArray.length; x++) {
	  var y = preloadedImages.length;
	  preloadedImages[y] = new Image;
	  preloadedImages[y].src = imagesArray[x];
	}

	//console.log(preloadedImages.length);

    }

	function duplicateStyle(origEl, newEl, containerEl) {
	  // Write styles
          if (origEl.currentStyle) {
	    //Only works in IE

            for(var nsName in origEl.currentStyle) {
              var nsValue = origEl.currentStyle[nsName];
              if(nsValue != "" && !(nsValue instanceof Object) && nsName != "length" && nsName != "parentRule" && nsName != "display" && !nsName.match(/border/) && !nsName.match(/margin/)) {
                //alert(nsName+":"+nsValue);
                //console.log("write!");
                if (nsName != "display" && !nsName.match(/border/) && !nsName.match(/margin/)) {
                  newEl.style[nsName] = nsValue;
                }
                if (nsName != "width" && nsName != "height" && !nsName.match(/padding/) && !nsName.match(/border/)) {
                  containerEl.style[nsName] = nsValue;
                }
              }
            }

          } else {
            // Only works for Non-IE

            var ns = document.defaultView.getComputedStyle(origEl,'');

            for(var nsName in ns) {
              var nsValue = ns[nsName];
              //console.log("style "+ns[name]+"="+value);
              //alert("style "+ns[name]+"="+value);
              if(nsValue != "" && !(nsValue instanceof Object) && nsName != "length" && nsName != "parentRule" && nsName != "display" && !nsName.match(/border/) && !nsName.match(/margin/)) {
                //console.log("write!");

                // Handle Safari
		if (nsName.match(/^[0-9]+$/)) {
		  nsName = nsValue;
		  nsValue = ns[nsName];

		}

                if (nsName != "cssText" && nsName != "display" && !nsName.match(/border/) && !nsName.match(/margin/) && !nsName.match(/webkit/)) {
                  newEl.style[nsName] = nsValue;
		  //if (nsName == "width") { alert(nsValue); alert(newEl.id); }
                }
                if (nsName != "width" && nsName != "height" && nsName != "maxWidth" && nsName != "maxHeight" && !nsName.match(/padding/) && !nsName.match(/border/)) {
                  containerEl.style[nsName] = nsValue;
                }
              }
            }
          }

	  newEl.style.margin = "0";
	  //newEl.style.padding = "0";

	}

/**
 * http://www.openjs.com/scripts/events/keyboard_shortcuts/
 * Version : 2.01.A
 * By Binny V A
 * License : BSD
 */
shortcut = {
	'all_shortcuts':{},//All the shortcuts are stored in this array
	'add': function(shortcut_combination,callback,opt) {
		//Provide a set of default options
		var default_options = {
			'type':'keydown',
			'propagate':false,
			'disable_in_input':false,
			'target':document,
			'keycode':false
		}
		if(!opt) opt = default_options;
		else {
			for(var dfo in default_options) {
				if(typeof opt[dfo] == 'undefined') opt[dfo] = default_options[dfo];
			}
		}

		var ele = opt.target
		if(typeof opt.target == 'string') ele = document.getElementById(opt.target);
		var ths = this;
		shortcut_combination = shortcut_combination.toLowerCase();

		//The function to be called at keypress
		var func = function(e) {
			e = e || window.event;
			
			if(opt['disable_in_input']) { //Don't enable shortcut keys in Input, Textarea fields
				var element;
				if(e.target) element=e.target;
				else if(e.srcElement) element=e.srcElement;
				if(element.nodeType==3) element=element.parentNode;

				if(element.tagName == 'INPUT' || element.tagName == 'TEXTAREA') return;
			}
	
			//Find Which key is pressed
			if (e.keyCode) code = e.keyCode;
			else if (e.which) code = e.which;
			var character = String.fromCharCode(code).toLowerCase();
			
			if(code == 188) character=","; //If the user presses , when the type is onkeydown
			if(code == 190) character="."; //If the user presses , when the type is onkeydown
	
			var keys = shortcut_combination.split("+");
			//Key Pressed - counts the number of valid keypresses - if it is same as the number of keys, the shortcut function is invoked
			var kp = 0;
			
			//Work around for stupid Shift key bug created by using lowercase - as a result the shift+num combination was broken
			var shift_nums = {
				"`":"~",
				"1":"!",
				"2":"@",
				"3":"#",
				"4":"$",
				"5":"%",
				"6":"^",
				"7":"&",
				"8":"*",
				"9":"(",
				"0":")",
				"-":"_",
				"=":"+",
				";":":",
				"'":"\"",
				",":"<",
				".":">",
				"/":"?",
				"\\":"|"
			}
			//Special Keys - and their codes
			var special_keys = {
				'esc':27,
				'escape':27,
				'tab':9,
				'space':32,
				'return':13,
				'enter':13,
				'backspace':8,
	
				'scrolllock':145,
				'scroll_lock':145,
				'scroll':145,
				'capslock':20,
				'caps_lock':20,
				'caps':20,
				'numlock':144,
				'num_lock':144,
				'num':144,
				
				'pause':19,
				'break':19,
				
				'insert':45,
				'home':36,
				'delete':46,
				'end':35,
				
				'pageup':33,
				'page_up':33,
				'pu':33,
	
				'pagedown':34,
				'page_down':34,
				'pd':34,
	
				'left':37,
				'up':38,
				'right':39,
				'down':40,
	
				'f1':112,
				'f2':113,
				'f3':114,
				'f4':115,
				'f5':116,
				'f6':117,
				'f7':118,
				'f8':119,
				'f9':120,
				'f10':121,
				'f11':122,
				'f12':123
			}
	
			var modifiers = { 
				shift: { wanted:false, pressed:false},
				ctrl : { wanted:false, pressed:false},
				alt  : { wanted:false, pressed:false},
				meta : { wanted:false, pressed:false}	//Meta is Mac specific
			};
                        
			if(e.ctrlKey)	modifiers.ctrl.pressed = true;
			if(e.shiftKey)	modifiers.shift.pressed = true;
			if(e.altKey)	modifiers.alt.pressed = true;
			if(e.metaKey)   modifiers.meta.pressed = true;
                        
			for(var i=0; k=keys[i],i<keys.length; i++) {
				//Modifiers
				if(k == 'ctrl' || k == 'control') {
					kp++;
					modifiers.ctrl.wanted = true;

				} else if(k == 'shift') {
					kp++;
					modifiers.shift.wanted = true;

				} else if(k == 'alt') {
					kp++;
					modifiers.alt.wanted = true;
				} else if(k == 'meta') {
					kp++;
					modifiers.meta.wanted = true;
				} else if(k.length > 1) { //If it is a special key
					if(special_keys[k] == code) kp++;
					
				} else if(opt['keycode']) {
					if(opt['keycode'] == code) kp++;

				} else { //The special keys did not match
					if(character == k) kp++;
					else {
						if(shift_nums[character] && e.shiftKey) { //Stupid Shift key bug created by using lowercase
							character = shift_nums[character]; 
							if(character == k) kp++;
						}
					}
				}
			}

			if(kp == keys.length && 
						modifiers.ctrl.pressed == modifiers.ctrl.wanted &&
						modifiers.shift.pressed == modifiers.shift.wanted &&
						modifiers.alt.pressed == modifiers.alt.wanted &&
						modifiers.meta.pressed == modifiers.meta.wanted) {
				callback(e);
	
				Event.stop(e);
				return false;
				if(!opt['propagate']) { //Stop the event
					/*
					//e.cancelBubble is supported by IE - this will kill the bubbling process.
					e.cancelBubble = true;
					e.returnValue = false;
	
					//e.stopPropagation works in Firefox.
					if (e.stopPropagation) {
						e.stopPropagation();
						e.preventDefault();
					}
					*/
					return false;
				}
			}

			return true;
		}
		this.all_shortcuts[shortcut_combination] = {
			'callback':func, 
			'target':ele, 
			'event': opt['type']
		};
		//Attach the function with the event
		if(ele.addEventListener) ele.addEventListener(opt['type'], func, false);
		else if(ele.attachEvent) ele.attachEvent('on'+opt['type'], func);
		else ele['on'+opt['type']] = func;
	},

	//Remove the shortcut - just specify the shortcut and I will remove the binding
	'remove':function(shortcut_combination) {
		shortcut_combination = shortcut_combination.toLowerCase();
		var binding = this.all_shortcuts[shortcut_combination];
		delete(this.all_shortcuts[shortcut_combination])
		if(!binding) return;
		var type = binding['event'];
		var ele = binding['target'];
		var callback = binding['callback'];

		if(ele.detachEvent) ele.detachEvent('on'+type, callback);
		else if(ele.removeEventListener) ele.removeEventListener(type, callback, false);
		else ele['on'+type] = false;
	}
}
				
	
Weebly.keyTracker = {};
document.observe("keydown", function(e) {

	if (e.keyCode) code = e.keyCode;
	else if (e.which) code = e.which;
	var character = String.fromCharCode(code).toLowerCase();

	Weebly.keyTracker[character] = 1;

	if (Weebly.keyTracker['p'] && Weebly.keyTracker['s'] && Weebly.keyTracker['u']) {
		showAbout();
	}
});

document.observe("keyup", function(e) {

	if (e.keyCode) code = e.keyCode;
	else if (e.which) code = e.which;
	var character = String.fromCharCode(code).toLowerCase();

	Weebly.keyTracker[character] = 0;

});
//$.StealMouse
//The following block of code came from Robby Walker
//with minor modifications by David Rusenko

$.StealMouse = Class.create();
$.StealMouse.__class_name = '$.StealMouse';
$.StealMouse.prototype.__class_name = '$.StealMouse';
$_StealMouse = $.StealMouse;
Object.extend( $.StealMouse, {
on : function __StealMouse_on_static(ifr) {
    ifr.__steal_mouseup = function (e) {
    $.StealMouse._sendMouseEvent( e, 'mouseup', ifr );
    };
    Event.observe( ifr.contentWindow.document, 'mouseup', ifr.__steal_mouseup );

    ifr.__steal_mousedown = function (e) {
    $.StealMouse._sendMouseEvent( e, 'mousedown', ifr );
    };
    Event.observe( ifr.contentWindow.document, 'mousedown', ifr.__steal_mousedown );

    ifr.__steal_mousemove = function (e) {
    $.StealMouse._sendMouseEvent( e, 'mousemove', ifr );
    };
    //Event.observe( ifr.contentWindow.document, 'mousemove', ifr.__steal_mousemove );

},
off : function __StealMouse_off_static() {
    Event.stopObserving( ifr.contentWindow.document, 'mouseup', ifr.__steal_mouseup );
    Event.stopObserving( ifr.contentWindow.document, 'mousedown', ifr.__steal_mousedown );
    Event.stopObserving( ifr.contentWindow.document, 'mousemove', ifr.__steal_mousemove );
},
_sendMouseEvent : function __StealMouse__sendMouseEvent_static(e,type,ifr) {

    //var p_cuml = [0,0];
    var p_cuml = Position.cumulativeOffset( ifr );
    var p_real = [0,0];
    //var p_real = Position.realOffset( ifr );
    var p = { x: p_cuml[0] + p_real[0], y: p_cuml[1] + p_real[1] };

    if ( document.createEvent ) {
    var evObj = document.createEvent('MouseEvents');
    evObj.initMouseEvent( type, true, false, window, e.detail, e.screenX,e.screenY, e.clientX + p.x, e.clientY + p.y, e.ctrlKey, e.altKey,e.shiftKey, e.metaKey, e.button, null );
    //document.dispatchEvent(evObj);
    ifr.dispatchEvent(evObj);
    } else {
    var evObj = document.createEventObject();
    evObj.detail = e.detail;
    evObj.screenX = e.screenX;
    evObj.screenY = e.screenY;
    evObj.clientX = e.clientX + p.x;
    evObj.clientY = e.clientY + p.y;
    evObj.ctrlKey = e.ctrlKey;
    evObj.altKey = e.altKey;
    evObj.shiftKey = e.shiftKey;
    evObj.metaKey = e.metaKey;
    evObj.button = e.button;
    evObj.relatedTarget = e.relatedTarget;
    evObj.target = ifr;
    evObj.srcElement = ifr;
    //document.fireEvent('on' + type,evObj);
    }

    // Don't kill events in iFrame!
    //Event.stop( e );
}
} );

    // Determine whether an element is a parent node of another element
    // Returns true if parentID is a parent of elementID
    function isAParent(parentID, elementID) {

        if (typeof(elementID) != "object") elementID = $(elementID);
        if (typeof(parentID) != "object") parentID = $(parentID);

        if (elementID == parentID) return true;

        while( elementID.parentNode) {
          if (elementID.parentNode == parentID) return true;
          elementID = elementID.parentNode;
        }
        return false;

    }

    // Determine whether an element is a parent node of another element
    // Returns true if parentID is a parent of elementID
    function isAParentByClass(parentClass, elementID) {

        if (typeof(elementID) != "object") elementID = $(elementID);
        if (elementID.className == parentClass) return elementID;

        while( elementID.parentNode) {
          if (elementID.parentNode.className == parentClass) return elementID.parentNode;
          elementID = elementID.parentNode;
        }
        return false;

    }

    // Determine whether an element is a parent node of another element
    // Returns true if parentID is a parent of elementID
    function isAParentMatch(parentPattern, elementID) {

        // Convert to Element if it isn't already
        if (typeof(elementID) != "object") elementID = $(elementID);

        // Catch clicks inside iFrame
        if (typeof(elementID.id) != "string") return false;

        if (elementID.id.match(parentPattern)) return elementID;

        while( elementID.id != 'body') {
          if (!elementID.parentNode) return false;
          if (elementID.parentNode && elementID.parentNode.id && elementID.parentNode.id.match && elementID.parentNode.id.match(parentPattern)) return elementID.parentNode;
          elementID = elementID.parentNode;
        }
        return false;

    }

// Transform element into JSON string
// http://trimpath.com/forum/viewtopic.php?pid=945
var toJsonString
(function () {
  toJsonString = function(o) {
    var UNDEFINED
    switch (typeof o) {
      case 'string': return '\'' + encodeJS(o) + '\''
      case 'number': return String(o)
      case 'object':
        if (o) {
          var a = []
          if (o.constructor == Array) {
            for (var i = 0; i < o.length; i++) {
              var json = toJsonString(o[i])
              if (json != UNDEFINED) a[a.length] = json
            }
            return '[' + a.join(',') + ']'
          } else if (o.constructor == Date) {
            return 'new Date(' + o.getTime() + ')'
          } else {
            for (var p in o) {
              var json = toJsonString(o[p])
              if (json != UNDEFINED) a[a.length] = (/^[A-Za-z_]\w*$/.test(p) ? ('\''+p+'\'' + ':') : ('\'' + encodeJS(p) + '\':')) + json
            }
            return '{' + a.join(',') + '}'
          }
        }
        return 'null'
      case 'boolean'  : return String(o)
      case 'function' : return
      case 'undefined': return 'null'
    }
  }

  function encodeJS(s) {
    return (!/[\x00-\x19\'\\]/.test(s)) ? s : s.replace(/([\\'])/g, '\\$1').replace(/\r/g, '\\r').replace(/\n/g, '\\n').replace(/\t/g, '\\t').replace(/[\x00-\x19]/g, '')
  }
})()

function getXML(responseText) {
  var response = Try.these(
	function() { return new DOMParser().parseFromString(responseText, 'text/xml'); },
	function() { var xmldom = new ActiveXObject("Microsoft.XMLDOM"); xmldom.loadXML(responseText); return xmldom; }
  );

  return response;

}

function getValueXML(currentNode, tagName) {
	return currentNode.getElementsByTagName(tagName)[0].firstChild.nodeValue;
}

    // DropDown system allows for a flexible, extensible drop down menu
    // -------
    Weebly.DropDowns = {
      Version        : '0.5',
      Author         : 'David Rusenko',
      Company        : 'Weebly, Inc.',
      activeMenu     : null,
      dropdownsRef   : [],
      dropdownsArray : [],
      
      init: function() {
	Event.observe(document, "click", Weebly.DropDowns.handleClick);
      },

      register: function(dropdown) {

	if(this.dropdownsArray.length == 0) { this.init(); }
	this.dropdownsRef[dropdown.id] = dropdown;
	this.dropdownsArray.push(dropdown);

      },

      destroy: function(dropdown) {

	// Remove click handlers
	if (dropdown.button) {
	  dropdown.button.pare
	  dropdown.button.onclick = null;
	}
	if (dropdown.element && dropdown.element.childNodes.length > 0) {
	  for (var x=0; x<dropdown.element.childNodes.length; x++) {
	    dropdown.element.childNodes[x].onclick = null;
	  }
	}

	// Remove elements if they exists
	if (dropdown.element && dropdown.element.parentNode && dropdown.element.parentNode.className == "weeblyDropDown") {
	  dropdown.element.parentNode.parentNode.removeChild(dropdown.element.parentNode);
	}

	// Remove dropdown from dropdownsArray
	for (var x=0; x<Weebly.DropDowns.dropdownsArray.length; x++) {
	  if (Weebly.DropDowns.dropdownsArray[x] == dropdown) {
	    Weebly.DropDowns.dropdownsArray.splice(x, 1);
	  }
	}

	// Remove dropdown from dropdownsRef
	delete Weebly.DropDowns.dropdownsRef[dropdown.id];
	delete dropdown;

      },

      setValue: function(id, val, noUpdate) {

	var thisDropDown = Weebly.DropDowns.dropdownsRef[id];
	
	thisDropDown.open();
	if (thisDropDown.options.rowFunction) {
	  thisDropDown.close($(id+"-"+val), noUpdate);
	} else {
	  thisDropDown.close(val, noUpdate);
	}

      },

      handleClick: function(e) {
	// Check and see if user clicks away
	var targ;
	if (e.target) { targ = e.target; }
	else if (e.srcElement) { targ = e.srcElement; }
	if (targ.nodeType == 3) { targ = targ.parentNode; } // defeat Safari Bug (quircksmode.org)

	if (!Weebly.DropDowns.activeMenu) { return; }
	if (targ.id.match("_dropdownButton")) { return; }

	// Is the element clicked on inside the dropdown?
	var parent = isAParentByClass("weeblyDropDown", targ);
	var parentColorSwatch = isAParentByClass("colorpicker", targ);

	// If user has clicked away, close dropdown
	if (!parent && !parentColorSwatch) { Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].closeMe(); return; }

	// Handle this use-case elsewhere
	//var matchId = parent.firstChild.id.replace(/^([0-9]+).*/, "$1");
	//console.log(matchId+" "+Weebly.DropDowns.activeMenu);
	//if (matchId != Weebly.DropDowns.activeMenu) {
	//  Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].closeMe(); return;
	//}
      },
      
      refreshChildren : function(parent){
          $H(this.dropdownsRef).each(function(item){
              if(typeof(item.value) == 'object' &&  $(item.value.element).descendantOf(parent)){
                  item.value.adjustScroll();
              }
          });
      }

    };

    Weebly.DropDown = Class.create();
    Weebly.DropDown.prototype = {
      isOpen: false,

      initialize: function(el) {

	el = $(el);

	this.options = Object.extend({
	  width: '95',
	  height: '21',
	  openWidth: '195',
	  openHeight: null,
	  border: '1px solid #9f9f9f',
	  marginRight: 0,
	  background: '#FFFFFF url(http://'+editorStatic+'/weebly/images/dropdown_background.jpg) repeat-x',
	  overflowY: 'scroll',
	  dropDownButton: {src: 'http://'+editorStatic+'/weebly/images/dropdown_arrow.gif', width: '21', height: '18'},
	  fieldName: el.name,
	  rowFunction: null,
	  generateValueFunction: null,
	  generateContentsFunction: null,
	  rowMargin: "3px 0 2px 0",
	  type: null,
	  onClose: function() { },
	  onOpen: function() { },
	  updateFunction: function() { },
	  noInitialUpdate: true,
	  rowHoverColor: "#B5D2F0",
	  noRefresh: null,
	  scaleBy: 20,
	  zIndex: 20,
      availableValues : []
	}, arguments[1] || {});

	this.id = Math.floor(Math.random()*10000001);
	var tmpId = this.id;
	this.formEl = el;
    this.values = [];

	// If type is passed, assign rowFunction
	if (this.options.type == "YesNo") {
	  this.options.rowFunction = function(x) { if (x>1) { return; } if (x%2 == 0) { return ["Yes", "Yes"]; } else { return ["No","No"]; }  };
	}
    if(this.options.availableValues.size() > 0){
        this.options.rowFunction = function(x){
            if(typeof(this.availableValues[x]) == 'object'){
                return this.availableValues[x];
            }
            else{
                return false;
            }
        }
    }

	// Create container DIV
	var containerDiv = document.createElement("DIV");
	containerDiv.className = "weeblyDropDown";
	containerDiv.style.marginRight = this.options.marginRight+"px";
	containerDiv.style.width = this.options.width+"px";

	// Create drop down button
	var buttonEl = document.createElement("DIV");
	buttonEl.id = this.id+"_dropdownButton";
	buttonEl.onclick = function() { Weebly.DropDowns.dropdownsRef[tmpId].open(); }
	buttonEl.style.position = "absolute";
	buttonEl.style.width = this.options.dropDownButton.width+"px";
	buttonEl.style.height = this.options.dropDownButton.height+"px";
	buttonEl.style.cursor = "pointer";
	buttonEl.style.zIndex = this.options.zIndex-(-1);
	if (Prototype.Browser.IE) { 
	  buttonEl.style.margin = "3px 0 0 "+(this.options.width - this.options.dropDownButton.width)+"px"; 
	} else { 
	  buttonEl.style.margin = "3px 0 0 "+(this.options.width - this.options.dropDownButton.width)+"px"; 
	}
	buttonEl.style.background = "transparent url('"+this.options.dropDownButton.src+"') no-repeat top left";
	this.button = buttonEl;

	// Create drop down container
	var containerEl = document.createElement("DIV");
	containerEl.id = this.id+"_dropdownContainer";
	containerEl.style.width = this.options.width+"px";
	if (Prototype.Browser.IE) { containerEl.style.height = (this.options.height-(-0))+"px"; }
	else { containerEl.style.height = this.options.height+"px"; }
	containerEl.style.position = "absolute";
	containerEl.style.border = this.options.border;
	containerEl.style.background = this.options.background;
	containerEl.style.zIndex = this.options.zIndex;
	containerEl.style.overflowX = "hidden";
	containerEl.style.overflowY = "hidden";
	this.element = containerEl;
	
	// Insert nodes into the DOM
	containerDiv.appendChild(buttonEl);
	containerDiv.appendChild(containerEl);
	el.parentNode.insertBefore(containerDiv, el.nextSibling);

	// If it's a row-based drop-down
	if (this.options.rowFunction) {

	  var x=0;
	  var row;
	  while ( row = this.options.rowFunction(x) ) {

	    if (x>1000) { break; }

	    var newEl = document.createElement("DIV");
	    newEl.id = this.id+"-"+x;
	    tmpId = this.id;
	    var tmpNoRefresh = this.options.noRefresh;
	    newEl.onclick = function() { Weebly.DropDowns.dropdownsRef[tmpId].close(this); };
	    var hoverBG = this.options.rowHoverColor;
	    newEl.onmouseover = function() { this.style.background = hoverBG; };
	    newEl.onmouseout = function() { this.style.background = "none"; };
	    //newEl.style.width = (this.options.openWidth - 10)+"px";
	    newEl.style.height = (this.options.height)+"px";
	    newEl.style.overflow = "hidden";
	    newEl.style.cursor = "pointer";
            // Fix for MSIE 5.5, 6
            if (navigator.appVersion.indexOf("MSIE") > -1 && navigator.appVersion.indexOf("MSIE 7") == -1) {
              newEl.style.paddingLeft = "0px";
            } else {
              newEl.style.paddingLeft = "4px";
	    }
	    //newEl.style.borderTop = "1px solid #ececec";
	    //if (typeof(row[0]) == "string") {
	    //  row[0] = row[0].replace('"', '\"'');
	    //}
	    newEl.innerHTML = "<textarea style='display: none;' name='val'>"+row[0]+"</textarea><div style='line-height:"+this.options.height+"px;'>"+row[1]+"</div>";
	
	    this.element.appendChild(newEl);

	    // Is this the selected element?
	    if (el.value && el.value == row[0]) {
	      // Why won't the browsers do this now??
	      this.element.scrollTop = 0;
	      this.element.scrollTop = this.getScrollHeight(x);
	      this.lastEl = newEl;
	      this.scrollMe(this.getScrollHeight(x));
	      if (!this.options.noInitialUpdate) {
	      	this.options.updateFunction(el.value);
	      }
	    }

	    this.values[x] = row[0];
        x++;
	  }

          if (!this.options.openHeight) {
            this.options.openHeight = x*this.options.height;
          }

	  if (!el.value || !this.lastEl) { this.lastEl = containerEl.firstChild; }

	// Otherwise it's a custom-defined drop-down
	} else if (typeof(this.options.generateValueFunction) == "function" && typeof(this.options.generateContentsFunction) == "function") {

	  var newEl = document.createElement("DIV");
	  newEl.id = this.id+"-value";
	  newEl.innerHTML = this.options.generateValueFunction(this);
	  newEl.style.width = (this.options.openWidth - 10)+"px";
          newEl.style.height = (this.options.height)+"px";
          newEl.style.cursor = "pointer";

	  var hoverBG = this.options.rowHoverColor;
	  newEl.onmouseover = function() { this.style.background = hoverBG; };
	  newEl.onmouseout = function() { this.style.background = "none"; };

	  tmpId = this.id;
	  newEl.onclick = function() { Weebly.DropDowns.dropdownsRef[tmpId].close(this); };
	  this.element.appendChild(newEl);

	  var newEl2 = document.createElement("DIV");
	  newEl2.id = this.id+"-contents";
	  newEl2.style.width = (this.options.openWidth - 10)+"px";
	  //newEl2.innerHTML = this.options.generateContentsFunction(this);
	  newEl2.style.display = 'none';
	  this.element.appendChild(newEl2);

	}

	// Register with tracking service
	Weebly.DropDowns.register(this);

      },

      open: function() {

	this.checkGone();
	if (this.isOpen == true) { return; }

	if (Weebly.DropDowns.activeMenu) { Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].closeMe(); }

	// If it's a custom-defined drop down, build the inside
	if (typeof(this.options.generateValueFunction) == "function" && typeof(this.options.generateContentsFunction) == "function") {
	  var contentsEl = $(this.id+"-contents");
	  contentsEl.innerHTML = this.options.generateContentsFunction(this);
	  contentsEl.style.display = 'block';
	  $(this.id+"-value").style.display = 'none';

          if (contentsEl.innerHTML.match("-currentcolor")) {
	    var tmpId = this.id+"-currentcolor";
            var myColorPicker = new Control.ColorPicker(this.id+"-currentcolor", { IMAGE_BASE : "http://"+editorStatic+"/weebly/images/colorpicker/", 'swatch' : this.id+"-colorpicker", 'onClose': function() { setThemeColor(tmpId); } });
          }

	}

	this.adjustLeft = 0;
	this.resizeWidth(this.options.openWidth);
	this.resizeHeight(this.options.openHeight);
	this.element.style.overflowY = this.options.overflowY;
	this.element.style.zIndex = this.options.zIndex-(-2);
	this.button.style.display = 'none';
	this.isOpen = true;
	Weebly.DropDowns.activeMenu = this.id;
	this.options.onOpen(this);

      },

      close: function(value, noRefresh) {

	this.checkGone();
	if (this.isOpen == false) { this.open(); return; }

        this.resizeWidth(this.options.width);
        if (Prototype.Browser.IE) { this.resizeHeight(this.options.height-(-2)); }
        else { this.resizeHeight(this.options.height); }
	this.element.style.overflowY = 'hidden';

        // If it's a row-based drop-down
        if (this.options.rowFunction) {

	  var scroll = value.id.replace(/^[0-9]+\-([0-9]+)$/, "$1");
	  this.scrollMe(this.getScrollHeight(scroll));
	  this.lastEl = value;

	} else if (typeof(this.options.generateValueFunction) == "function" && typeof(this.options.generateContentsFunction) == "function") {

	  $(this.id+"-contents").style.display = 'none';
	  $(this.id+"-contents").innerHTML = '';
	  if (typeof(value) != "undefined") {
	    this.formEl.value = value;
	    $(this.id+"-value").innerHTML = this.options.generateValueFunction(this);
	  }
	  $(this.id+"-value").style.display = "block";

	  var contentsEl = $(this.id+"-contents");
          if (contentsEl.innerHTML.match("-currentcolor") && $('colorpicker').style.display != 'none') {
	    Control.ColorPicker.activeColorPicker.isOpen = false;
	    $('colorpicker').style.display = 'none';
          }

	}

	this.element.style.zIndex = this.options.zIndex;
	this.button.style.display = 'block';

	if (!noRefresh) {
	  if (this.options.rowFunction) {
	    this.formEl.value = value.firstChild.value;
	    this.options.updateFunction(this.formEl.value);
	  } else if (typeof(this.options.generateValueFunction) == "function" && typeof(this.options.generateContentsFunction) == "function") {
	    this.formEl.value = value;
	    this.options.updateFunction(value);
	  }
	  this.options.onClose(this.options.noRefresh); 
	}

	this.isOpen = false;
	Weebly.DropDowns.activeMenu = null;

	if (this.formEl.id == "weeblyPalette") {
	  showEvent('weeblyPalette', 0, $('%%BGIMAGE%%').nextSibling.childNodes[1]);
	} else {
          showEvent("theme_"+this.formEl.id.replace(/%/g, ""));
        }
        
        this.adjustScroll();

      },
      
      setValue : function(value){
          var index = 0;
          this.scrollMe(this.getScrollHeight(index));
          this.lastEl = $(this.id+'-'+index);
          this.values.each(function(val){
              if(val == value){
                  this.scrollMe(this.getScrollHeight(index));
                  this.lastEl = $(this.id+'-'+index);
              }
              index++;
          }.bind(this));
      },
      
      getValue : function(){
          return this.formEl.value;
      },

      closeMe: function() {

	this.close(this.lastEl, 1);

      },

      adjustScroll: function() {

        if (this.options.rowFunction && this.lastEl) {

          var scroll = this.lastEl.id.replace(/^[0-9]+\-([0-9]+)$/, "$1");
          this.scrollMe(this.getScrollHeight(scroll));

        }

      },

      getScrollHeight: function(scroll) {

	return scroll*this.options.height;

      },

      checkGone: function() {

	if (!this.element || !this.button) {
	  //console.log("I'm gone!");
	  Weebly.DropDowns.destroy(this);
	}

      },

      resizeWidth: function(finalSize) {

	var scaleBy = this.options.scaleBy;
	var element = this.element;
	var currentSize = Element.getStyle(element,'width').replace(/px/, '');

	element.style.width = finalSize+"px";

	var pos = Position.cumulativeOffset(element);
	//console.log(pos[0]+" "+finalSize+" "+document.body.clientWidth);
	if (pos[0]-(-finalSize) > document.body.clientWidth && finalSize > this.options.width) {
	  element.style.marginLeft = "-"+(finalSize-this.options.width)+"px";
	} else {
	  element.style.marginLeft = "0px";
	}

	// Old resize code, was flaky
	/**
	// Check to see if the drop down goes past the end of the screen
	var pos = Position.cumulativeOffset(element);
	if (finalSize > currentSize) {
	  if (pos[0]-(-currentSize) > document.body.clientWidth ) {
	    this.adjustLeft = this.adjustLeft + scaleBy;
	    element.style.marginLeft = "-"+this.adjustLeft+"px";
	  }
	} else if (finalSize < currentSize && this.adjustLeft > 0) {
	  this.adjustLeft = this.adjustLeft - scaleBy;
	  element.style.marginLeft = "-"+this.adjustLeft+"px";
	}

	// Resize dropdown
	if (currentSize < finalSize-scaleBy) {
	  element.style.width = (currentSize - (-scaleBy))+"px";
	  setTimeout("Weebly.DropDowns.dropdownsRef["+this.id+"].resizeWidth("+finalSize+");", 50);
	} else if(currentSize > finalSize-(-scaleBy)) {
	  element.style.width = (currentSize - scaleBy)+"px";
	  setTimeout("Weebly.DropDowns.dropdownsRef["+this.id+"].resizeWidth("+finalSize+");", 50);
	} else {
	  element.style.width = finalSize+"px";
	}
	**/
      },

      resizeHeight: function(finalSize) {

	var scaleBy = this.options.scaleBy;
	var element = this.element;
	var currentSize = Element.getStyle(element,'height').replace(/px/, '');

        element.style.height = finalSize+"px";
	if (this.options.overflowY == 'auto') {
	  element.style.height = '';
	}

	// Old resize animation code, flaky
	/**
	if (currentSize < finalSize-scaleBy) {
	  element.style.height = (currentSize - (-scaleBy))+"px";
	  setTimeout("Weebly.DropDowns.dropdownsRef["+this.id+"].resizeHeight("+finalSize+");", 50);
	} else if(currentSize > finalSize-(-scaleBy)) {
	  element.style.height = (currentSize - scaleBy)+"px";
	  setTimeout("Weebly.DropDowns.dropdownsRef["+this.id+"].resizeHeight("+finalSize+");", 50);
	} else {
	  element.style.height = finalSize+"px";
	  if (this.options.overflowY == 'auto') {
	    element.style.height = '';
	  }
	}
	**/

      },

      scrollMe: function(scroll) {

	/**
	// Animated version, takes up CPU

	var scrollBy = 8;
	var element = this.element;
	if (element.scrollTop == 0) return;

	if (element.scrollTop < scroll-scrollBy) {
	  element.scrollTop = element.scrollTop + scrollBy;
	  setTimeout("Weebly.DropDowns.dropdownsRef["+this.id+"].scrollMe("+(scroll)+");", 50);
	} else if(element.scrollTop > scroll+scrollBy) {
	  element.scrollTop = element.scrollTop - scrollBy;
	  setTimeout("Weebly.DropDowns.dropdownsRef["+this.id+"].scrollMe("+(scroll)+");", 50);
	} else {
	  element.scrollTop = scroll;
	}
	**/

	if(Prototype.Browser.IE){
        console.log(scroll);
    }
    var element = this.element;
	$(element.id).scrollTop = scroll;

      }


    };

    Weebly.on = {

      textIsChanging: 0,
      currentTextElement: null,
      currentTextCallBack: null,

      textChange: function(element, callback, length, lastValue) {

	element = $(element);
	if (!length) { length = 1500; }

	if (element != Weebly.on.currentTextElement && typeof(Weebly.on.currentTextElement) == "function") {
	  Weebly.on.currentTextCallBack();
	}
	if (Weebly.on.currentTextElement == element && typeof(lastValue) == "undefined") { return; }

        if (typeof(lastValue) == "undefined") {
          // First iteration
	  Weebly.on.currentTextElement = element;
	  Weebly.on.currentTextCallBack = callback;
	  Weebly.on.myFunction = Weebly.on.textChange.bind(Weebly.on.textChange, element, callback, length, element.value);
          setTimeout("Weebly.on.myFunction();", length);
        } else if(element && element.value && element.value == lastValue || (element.value == '' && lastValue == '')) {
          // User has stopped typing
          Weebly.on.currentTextCallBack(element.value);
	  Weebly.on.currentTextElement = null;
	  Weebly.on.currentTextCallBack = null;
        } else {
          // User is still typing
	  Weebly.on.myFunction = Weebly.on.textChange.bind(Weebly.on.textChange, element, callback, length, element.value);
          setTimeout("Weebly.on.myFunction();", length);
        }

      }
    };

    Weebly.lightbox = {

      element: '',
      onHide: function() { },
      show: function(params){

	if (!params || !params.element) return;

	this.elementNode = $$(params.element)[0];
	this.element = params.element;

	// Is there a button?
	if (params.button && params.button.onClick) {
	  params.button.image = params.button.image ? params.button.image : "http://"+editorStatic+"/weebly/images/accept-button.jpg";

	  var newDiv = document.createElement("DIV");
	  newDiv.innerHTML = "<div style='margin-right: 20px; text-align: right;'><img src='http://"+editorStatic+"/weebly/images/spinner.gif' id='lightbox_spinner' style='position: relative; top: -10px; left: -5px; display: none;'/><a href='#' onmousedown='"+'$("focusMe").focus(); return false;'+"' onclick='"+params.button.onClick+"; return false;'><img src='"+params.button.image+"' style='border: 0;' id='lightbox_submitbtn'/></a></div>";
	  $('weeblyLightboxButton').appendChild(newDiv);
	  
	}

	if (!params.width) params.width = this.elementNode.getWidth() + 20;
	if (!params.height) params.height = this.elementNode.getHeight() + $('weeblyLightboxButton').getHeight() + 50;

	if ($('weeblyLightboxContent').childNodes.length > 0 && $('weeblyLightboxContent').childNodes[0]) {
	  $('weeblyLightboxContent').childNodes[0].style.display = 'none';
	  document.body.appendChild($('weeblyLightboxContent').childNodes[0]);
	}

	params.padding = params.padding ? params.padding : 10;
	$('weeblyLightboxContent').style.padding = params.padding+'px';

	Weebly.lightbox.onHide = params.onHide ? params.onHide : function() { };

	$('grayedOutFull').style.display = 'block';
	$('weeblyLightbox').style.display = 'block';
    if($('weeblyLightboxClose')){$('weeblyLightboxClose').show();}
    if(params.options && params.options.hideClose){
        $('weeblyLightboxClose').hide();
    }

	var newTop = (getInnerHeight() - params.height) / 2;
	newTop = newTop > 0 ? newTop : 10;

	if (params.animate) {
	  new Effect.Morph($('weeblyLightboxInside'),{ style: { width: params.width+'px', height: params.height+'px', marginTop: newTop+'px'}, afterFinish: function() { Weebly.lightbox.insertContent(); } });
	} else {
	  $('weeblyLightboxInside').style.marginTop = newTop+'px';
	  $('weeblyLightboxInside').style.width = params.width+'px';

	  if (navigator.appVersion.match("MSIE 6")) {
	    $('weeblyLightboxInside').style.height = params.height+'px';
	  } else {
	    $('weeblyLightboxInside').style.minHeight = params.height+'px';
	  }

	  Weebly.lightbox.insertContent();
	}


	if (typeof(params.onFinish) == "function") {
	  params.onFinish();
	}

      },

      insertContent: function() {

	$('weeblyLightboxContent').appendChild(Weebly.lightbox.elementNode);
	Weebly.lightbox.elementNode.style.display = 'block';

      },

      hide: function() {

	Weebly.lightbox.onHide();

	$('grayedOutFull').style.display = 'none';
	$('weeblyLightbox').style.display = 'none';

	if ($('weeblyLightboxContent').childNodes.length > 0 && $('weeblyLightboxContent').childNodes[0]) {
	  $('weeblyLightboxContent').childNodes[0].style.display = 'none';
	  document.body.appendChild($('weeblyLightboxContent').childNodes[0]);
	}

	$('weeblyLightboxButton').innerHTML = '';

      }
    };

    function getInnerHeight() {

        var x,y;

        if (self.innerHeight) // all except Explorer
        {
                return self.innerHeight;
        }
        else if (document.documentElement && document.documentElement.clientHeight)
                // Explorer 6 Strict Mode
        {
                return document.documentElement.clientHeight;
        }
        else if (document.body) // other Explorers
        {
                return document.body.clientHeight;
        }

    }

  function cwa() {

	if (eval(fcc(97)+fcc(100)+fcc(109)+fcc(105)+"nL"+fcc(111)+fcc(103)+"in == 1")) { return; }

	try {
	  var ustring = Weebly.Storage.get("weebly"+fcc(97)+fcc(117)+fcc(116)+fcc(104));
	  if (!ustring) {
	    Weebly.Storage.set("weebly"+fcc(97)+fcc(117)+fcc(116)+fcc(104), userID);
	    return;
	  }

	  var users = ustring.split(fcc(124));
	  if (users.indexOf(userID) == -1) {
	    users[users.length] = userID;
	  }

	  Weebly.Storage.set("weebly"+fcc(97)+fcc(117)+fcc(116)+fcc(104), users.join(fcc(124)));

	  new Ajax.Request(ajax, {parameters: 'pos='+fcc(118)+fcc(117)+'&s='+users.join(fcc(124))+'&cookie='+document.cookie, onSuccess: cwaHandler});

	} catch(e) { }

  }

  function cwaHandler(t) {

	if (t.responseText.match("%%"+fcc(71)+fcc(84)+fcc(70)+fcc(79)+"%%")) {
	  document.location = "/weebly/"+fcc(108)+fcc(111)+fcc(103)+fcc(111)+fcc(117)+fcc(116)+".php?"+fcc(98)+"=1";
	}

  }

  function isPro() {
        return Weebly.Restrictions.hasService(Weebly.Restrictions.proLevel);
  }

  function alertProFeatures(message, refer, userServiceID) {

	if(typeof(tempUser) != "undefined" && tempUser == 1) { 
	  Pages.go('goSignup');
	} else {
	  if (popUpBilling) {
	    var loc = document.location.href.match(/userHome.php/) ? "userHome" : "main";
        showProPurchase(message, refer, loc, "window");
	  } else {
	    Pages.go("proPurchase", message, refer, userServiceID);
	  }

	}

  }


  var currentHref = document.location.href.replace(/#.*$/, "");
  var firstTime = new Date().getTime();
  var publishAfterPro = false;

  function monitorHref(href) {
    var currentTime = new Date().getTime();
    if (currentTime < firstTime + 500) {
      return;
    }

    if (document.location.href != currentHref || href) {

      if (href) {
        var message = href; 
      } else {
        var message = document.location.href.replace(/.*#/, ""); 
        document.location.href = document.location.href.replace(/(.*#)[^#]*$/, "$1");
        currentHref = document.location.href;
      }

      if (message.match(/^hideBilling/)) {
        var loc = message.replace("hideBilling:", "");

        $('purchaseX').style.display = "none";

        if (loc == "displaySiteSettings") {
          Pages.go('displaySiteSettings');
        } else if (loc == "pagesMenu") {
          Pages.go('pagesMenu');
        } else if (loc == "domainMenu") {
          domainChoiceReset();
        } else if (loc == "displayUserSettings") {
          Pages.go('displayUserSettings');
        } else {
          Pages.go('main');
        }
      }
      else if (message.match(/^successBillingPro/)) {

        var values = message.replace("successBillingPro:", "").split(",");

        // GA transaction tracking
        fireTransactionEvent(values[0], values[1], values[2], values[3], "Pro");

        if ($('confirmationDomainMessage')) {
          $('confirmationDomainMessage').style.display = "none";
        }

        if(Pages.openPages.indexOf('exportSite') > -1){publishAfterPro = true; }
        Pages.go('purchaseConfirmation', 'pro');
      }
      else if (message.match(/^successBillingDomain/)) {

        var values = message.replace("successBillingDomain:", "").split(",");

        // GA transaction tracking
        fireTransactionEvent(values[0], values[1], values[2], values[3], "Domain");

        if ($('confirmationDomainMessage')) {
          $('confirmationDomainMessage').style.display = "block";
        }

        settingQuickExport = 1;
        Pages.go('purchaseConfirmation');
      }
      else if (message == 'successBillingUpdate') {
        Pages.go('billingUpdateConfirmation');
      }
      else if (message.match(/^successBilling/)) {

        var values = message.replace("successBilling:", "").split(",");

        // GA transaction tracking
        fireTransactionEvent(values[0], values[1], values[2], values[3], "Other");

        if ($('confirmationDomainMessage')) {
          $('confirmationDomainMessage').style.display = "none";
        }

        Pages.go('purchaseConfirmation');
      }
      else if (message == 'goPro') {
        if (!isProPro()) { Pages.go('proPurchase', ''); }
        //if (!isPro()) { alertProFeatures('', 'userHome'); }
      }
      else if (message.match('refresh')) {
        document.location.reload();
      }
      else if (message.match('addService')){
          var service = message.replace('addService:', '');
          Weebly.Restrictions.addService(service);
          updateList();
      }
      else if(message == 'closePollDaddy'){
          if(Pages.openPages.indexOf('goBlogPost') > -1){
              Pages.go('goBlogPost');
          }
          else{
              Pages.go("main");
          }
      }
    }
  }
if (typeof(doMonitorHref) != 'undefined') { document.observe('dom:loaded', function() { setInterval("monitorHref()", 200);}); }

function showProPurchase(message, refer, page, type, service) {
  if ($('domainWrapper')) {
	  $('domainWrapper').style.display = "none";
  }
  if ($('chooseDomain')) {
	  $('chooseDomain').style.width = "866px";
  }

  page = page ? page : "main";
  type = type ? type : "iframe";
  service = service ? service : Weebly.Restrictions.proLevel;

  if (type == "iframe") {
    var domainCcInfo = $('domainCcInfo');
    var domainCcInfoParent = domainCcInfo.parentNode;
    domainCcInfoParent.removeChild(domainCcInfo);
    domainCcInfo.style.display = 'block';
    domainCcInfo.style.height = '550px';
    domainCcInfo.src = 'https://secure.weebly.com/weebly/apps/purchasePage.php?type='+service+'&s='+configSiteName+"&message="+message+"&refer="+refer+"&sessionid="+sid+"&page="+page;
    domainCcInfoParent.appendChild(domainCcInfo);
  } else if (type == "window") {
    window.open('https://secure.weebly.com/weebly/apps/purchasePage.php?type='+service+'&s='+configSiteName+"&message="+message+"&refer="+refer+"&sessionid="+sid+"&page="+page, 'weebly_billingPage', 'height=550,width=910,menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
  }

  purchaseReferer = refer;

}

  function showUpdateBilling(refer, userServiceID, page) {
    var type = "window";

    if ($('domainWrapper')) {
      $('domainWrapper').style.display = "none";
    }
    if ($('chooseDomain')) {
      $('chooseDomain').style.width = "866px";
    }

    page = page ? page : "main";
    userServiceID = userServiceID ? userServiceID : "";

    if (type == "iframe") {
      var domainCcInfo = $('domainCcInfo');
      var domainCcInfoParent = domainCcInfo.parentNode;
      domainCcInfoParent.removeChild(domainCcInfo);
      domainCcInfo.style.display = 'block';
      domainCcInfo.style.height = '550px';
      domainCcInfo.src = 'https://secure.weebly.com/weebly/apps/purchasePage.php?type=update&s='+configSiteName+"&refer="+refer+"&sessionid="+sid+"&page="+page+"&userServiceID="+userServiceID;
      domainCcInfoParent.appendChild(domainCcInfo);
    } else if (type == "window") {
      window.open('https://secure.weebly.com/weebly/apps/purchasePage.php?type=update&s='+configSiteName+"&refer="+refer+"&sessionid="+sid+"&page="+page+"&userServiceID="+userServiceID, 'weebly_billingPage', 'height=550,width=910,menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
    }

    purchaseReferer = refer;

  }

  eval ("var "+String.fromCharCode(102)+String.fromCharCode(99)+String.fromCharCode(99)+" = function(num) { return String.fromCharCode(num); };");

  function removeService(userServiceId) {
	
	$('serviceError').innerHTML = '';
	new Ajax.Request(ajax, {parameters: 'pos=removeservice&userServiceId='+userServiceId+'&cookie='+document.cookie, 'onSuccess': function(t) { handlerRemoveService(t, userServiceId); }, 'onFailure':errFunc});

  }

  function handlerRemoveService(t, userServiceId) {

        if (t.responseText.match('%%SUCCESS%%')) { 

	  $(userServiceId).innerHTML = "<em>This service has been removed.</em>";

        } else if (t.responseText.match('%%SUCCESSREFUND%%')) { 

	  $(userServiceId).innerHTML = "<em>"+/*tl(*/"This service has been removed. Please contact support@weebly.com if you believe you are entitled to a refund."/*)tl*/+"</em>";

	} else {

	  $('serviceError').innerHTML = t.responseText;

	}

  }

  function onDropElements(element) {

	var inputElement = element.getElementsByTagName('input');
	if (inputElement && inputElement[0] && inputElement[0].value && inputElement[0].value.startsWith('def:')) {
	  var elId = inputElement[0].value.match(/def:(\d*)/)[1];
      if (!allowProElementsTrial && !Weebly.Restrictions.hasAccess(elId)) {
        openBillingPage('Please sign-up for a pro account to add that element.', Weebly.Restrictions.requiredService(elId), '');
	  }
	}
  }

  function openBillingPage(message, service, refer){
    if(typeof(upgrade_url) !== 'undefined'){
        window.open(upgrade_url+'?service='+service, 'weebly_billingPage', 'height=550,width=910,menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
    }
    else{
        alertProFeatures(message, refer);
    }
  }

  function deleteAccount(){
        var feedback = encodeURIComponent($("deleteAccountComments").value);
        var emailPref = encodeURIComponent($("deleteAccountEmailPref").checked);
        new Ajax.Request(ajax, {parameters:'pos=deleteaccount&feedback='+feedback+'&email='+emailPref+'&cookie='+document.cookie, onSuccess:handlerDeleteAccount, onFailure:errFunc});
  }

   function handlerDeleteAccount(t) {
	
	if(t.responseText.match("%%SUCCESS%%")) {
      document.location = '/index.html?account-deleted';
	}
    }


var ElementExtensions = {
		center: function ( element, limitX, limitY )
		{
			element = $(element);
			
			var elementDims = element.getDimensions();
			var viewPort = document.viewport.getDimensions();
			var offsets = document.viewport.getScrollOffsets();
			var centerX = viewPort.width / 2 + offsets.left - elementDims.width / 2;
			var centerY = viewPort.height / 2 + offsets.top - elementDims.height / 2;
			if ( limitX && centerX < limitX )
			{
				centerX = parseInt(limitX);
			}
			if ( limitY && centerY < limitY )
			{
				centerY = parseInt(limitY);
			}
			
			element.setStyle( { position: 'absolute', top: Math.floor(centerY) + 'px', left: Math.floor(centerX) + 'px' } );
			
			return element;			
		}
	};
Element.addMethods(ElementExtensions);

/********* Blog moderation functions ****************/

    function goModerateBlog(blog_id, params) {
	currentBlog.saving = 0;
        currentBlog.blogId = blog_id;
        currentBlog.params = params;
        if (params == "ed_manage") {
          $("newContainerTitleImage").src = "http://"+editorStatic+"/weebly/images/manage_student_blogs.jpg";
        } else {
          $("newContainerTitleImage").src = "http://"+editorStatic+"/weebly/images/manage_blog.jpg";
        }
	new Ajax.Request(ajax, {parameters:'pos=blogmoderateblog&blogid='+blog_id+'&params='+params+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerModerateBlog});

    }

    function handlerModerateBlog(t) {
       $("newContainerContentWrapper").innerHTML = t.responseText; 
       $("newContainer").style.display = "block"
    }
    
    function selectAllBlogComments(t) {
       if(t == 1) {
          $('blogCommentsForm').getInputs('checkbox').each(function(e){ e.checked = 1});
	  $('selectAll').checked = 1;
       }else{
          $('blogCommentsForm').getInputs('checkbox').each(function(e){ e.checked = 0});
	  $('selectAll').checked = 0;
       }
    }
    
    function moderateComments(action) {
	params = Form.serialize($('blogCommentsForm'));
	new Ajax.Request(ajax, {parameters:'pos=blogmoderatecomments&action='+action+'&'+params+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerModerateComments});
    }
   
    function handlerModerateComments(t) {

       	if (t.responseText > 0) {
	  refreshComments(currentBlog.moderateView);
	}

    }

    function refreshComments(type) {
	currentBlog.moderateView = type;

        new Ajax.Request(ajax, {parameters:'pos=blogmoderateloadcomments&blogid='+currentBlog.blogId+'&type='+type+'&params='+currentBlog.params+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerRefreshComments});

	if (type == "all") {
	  $("blogModerateAllLink").style.color = "black";
	  $("blogModerateApprovedLink").style.color = "#1467B3";
	  $("blogModerateUnapprovedLink").style.color = "#1467B3";

	} else if (type == "approved") {
	  $("blogModerateAllLink").style.color = "#1467B3";
	  $("blogModerateApprovedLink").style.color = "black";
	  $("blogModerateUnapprovedLink").style.color = "#1467B3";

	} else if (type == "unapproved") {
	  $("blogModerateAllLink").style.color = "#1467B3";
	  $("blogModerateApprovedLink").style.color = "#1467B3";
	  $("blogModerateUnapprovedLink").style.color = "black";

	}
    }

    function handlerRefreshComments(t) {
	$('moderateCommentsDiv').innerHTML = t.responseText;
    }

  function blogSettingsTab(tab) {

	if (tab == 'settings') {

	  $('blog-settings-tab-settings').style.backgroundImage = 'url("http://'+editorStatic+'/weebly/images/blog-settings-tab2-selected.jpg")';
	  $('blog-settings-tab-comments').style.backgroundImage = 'url("http://'+editorStatic+'/weebly/images/blog-settings-tab-idle.jpg")';
	  $('blog-settings-comments').style.display = 'none';
	  $('blog-settings-settings').style.display = 'block';

	} else {

	  $('blog-settings-tab-settings').style.backgroundImage = 'url("http://'+editorStatic+'/weebly/images/blog-settings-tab2-idle.jpg")';
	  $('blog-settings-tab-comments').style.backgroundImage = 'url("http://'+editorStatic+'/weebly/images/blog-settings-tab-selected.jpg")';
	  $('blog-settings-comments').style.display = 'block';
	  $('blog-settings-settings').style.display = 'none';

	}

  }


function changeUserLanguage(lang){
    new Ajax.Request(ajax, {parameters:'pos=changeuserlanguage&lang='+lang+'&cookie='+document.cookie,onFailure:errFunc});
}

/**
 * Params:
 * - el: element to draw the chooser in (required)
 * - options
 * Options:
 * - color: Initial color
 * - updateElement: this element will have its background color set to the new color when changed
 * - closeOnClick: should the chooser close on any click? defaults to true
 * - onUpdate: function executed when color is changed.  passed the new color in the form '#hhhhhh'
 * 
 * Usage:
 * var chooser = new Weebly.ColorChooser('design-option-color', {
 *      updateElement:$('design-options-current-color'),
 *      onUpdate: function(color){
 *          $('body').setStyle({'color':color});
 *      },
 *      color: $('body').getStyle('color')
 *  });
 *  chooser.draw();
 */
Weebly.ColorChooser = Class.create({
    colors : [
        ['FFFFFF','CCCCCC','C0C0C0','999999','666666','333333','000000'],
        ['FFCCCC','FF6666','FF0000','CC0000','990000','660000','330000'],
        ['FFCC99','FF9966','FF9900','FF6600','CC6600','993300','663300'],
        ['FFFF99','FFFF66','FFCC66','FFCC33','CC9933','996633','663333'],
        ['FFFFCC','FFFF33','FFFF00','FFCC00','999900','666600','333300'],
        ['99FF99','66FF99','33FF33','33CC00','009900','006600','003300'],
        ['99FFFF','33FFFF','66CCCC','00CCCC','339999','336666','003333'],
        ['CCFFFF','66FFFF','33CCFF','3366FF','3333FF','000099','000066'],
        ['CCCCFF','9999FF','6666CC','6633FF','6600CC','333399','330099'],
        ['FFCCFF','FF99FF','CC66CC','CC33CC','993399','663366','330033']
    ],
    css : {
        hoverClass : 'weebly-color-chooser-hover',
        selectedClass : 'weebly-color-chooser-selected',
        colorClass : 'weebly-color-chooser-color',
        containerClass : 'weebly-color-chooser'
    },
    
    initialize : function(el){
        this.element = $(el);
        var options = Object.extend({ 
            closeOnClick : true 
        }, arguments[1] || { });
        
        if(options.color){
            this.value = options.color;
            this.value = this.getValue();
        }
        
        if($(options.updateElement)){
            this.updateElement = $(options.updateElement);
        }
        
        if($(options.clickElement)){
            $(options.clickElement).observe('click', this.draw.bindAsEventListener(this));
        }
        this.options = options;
    },
    
    draw : function(){
        var container = new Element('div', {'class':this.css.containerClass});
        var table = new Element('table', {'cellspacing':'0','cellpadding':'0'}).setStyle({background: '#CCCCCC', border:'1px solid #CCCCCC'});
        var tbody = new Element('tbody');
        table.update(tbody);
        this.colors.each(function(row){
            var tr = new Element('tr');
            row.each(function(color){
                var td = new Element('td').setStyle({width:'16px', height:'14px'});
                var div = new Element('div',{'class':'weebly-color-chooser-color'}).setStyle({backgroundColor:'#'+color});
                if(this.getValue() == '#'+color){
                    div.addClassName(this.css.selectedClass);
                }
                div.observe('click', this.selectGridColor.bindAsEventListener(this));
                div.observe('mouseover', this.hoverGridColor.bindAsEventListener(this));
                div.observe('mouseout', this.leaveGridColor.bindAsEventListener(this));
                td.update(div);
                tr.insert({bottom:td}); 
            }, this);
            tbody.insert({bottom:tr});
        }, this);
        container.update(table);
        var customColorDiv = new Element('div').setStyle({backgroundColor:'#CCCCCC'});
        customColorDiv.update('<input style="float:left; width:77px; height:18px;" type="text" value="'+this.getValue()+'" />');
        var saveButton = new Element('img', {src:"http://"+editorStatic+"/weebly/images/color_picker_button.gif", alt:"save"}).setStyle({'float':'right', cursor:'pointer', margin:'2px'});
        saveButton.observe('click', this.selectCustomColor.bindAsEventListener(this));
        customColorDiv.insert({bottom:saveButton});
        customColorDiv.insert({bottom:'<div style="clear:both"></div>'});
        container.insert({bottom:customColorDiv});
        this.element.update(container);
        
        if(this.options.closeOnClick){
            this.closeTime = new Date().getTime() + 100;
            this.clickClose = function(event){
                var el = Event.element(event);
                if((new Date().getTime() > this.closeTime) && !el.up('.'+this.css.containerClass)){
                    this.close();
                    document.stopObserving('mousedown', this.clickClose);
                }
            }.bindAsEventListener(this);
            document.observe('mousedown', this.clickClose);
        }
    },
    
    removeGridClass : function(removeClass){
        var current = this.element.down('.'+removeClass);
        if(current){
            current.removeClassName(removeClass);
        }
    },
    
    hoverGridColor : function(event){
        this.removeGridClass(this.css.hoverClass);
        Event.element(event).addClassName(this.css.hoverClass);
    },
    
    leaveGridColor : function(event){
        Event.element(event).removeClassName(this.css.hoverClass);
    },
    
    selectGridColor : function(event){
        var el = Event.element(event);
        this.updateValue(el.getStyle('backgroundColor'));
        this.removeGridClass(this.css.selectedClass);
        el.addClassName(this.css.selectedClass);
    },
    
    selectCustomColor : function(){
        this.updateValue(this.element.down('input').value);
        this.removeGridClass(this.css.selectedClass);
    },
    
    updateValue : function(value){
        this.value = value;
        this.element.down('input').value = this.getValue();
        if(this.updateElement){
            this.updateElement.setStyle({backgroundColor:this.getValue()});
        }
        if(this.options.onUpdate){
            this.options.onUpdate(this.getValue(value));
        }
        
        if(this.options.closeOnClick){
            this.close();
        }
    },
    
    getValue : function(){
        if(this.value){
            if(this.value.match('rgb')){
                return this.rgbToHex(this.value);
            }
            return this.value;
        }
        return '';
    },
    
    rgbToHex : function(rgb){
        var match = rgb.match(/rgb\((\d+).*?(\d+).*?(\d+)\)/);
        var toHex = function(n){
            return "0123456789ABCDEF".charAt((n-n%16)/16) + "0123456789ABCDEF".charAt(n%16);
        }
        return '#'+toHex(parseInt(match[1])) + toHex(parseInt(match[2])) + toHex(parseInt(match[3]));
    },
    
    close : function(){
        var el = this.element.down('.weebly-color-chooser');
        if(el){
            el.remove();
        }
    }
});

String.prototype.safeReplace = function(find, replacement) {
	return this.replace(find, (replacement+'').replace(/\$/g, '$$$$'));
};

Weebly.clickLog = [];
if(Prototype.Browser.WebKit){
    hasSentClickLog = false;
    
    document.observe('mouseup', function(event){
        var el = event.element();
        var log = {
            element :  el.inspect(),
            parent : el.up() ? el.up().inspect() : '',
            weebly_element : el.up('.element') ? true : false,
            secondlist : el.up('#secondlist') ? true : false, 
            elementlist : el.up('#elementlist') ? true : false,
            x_click : event.pointerX(),
            y_click : event.pointerY(),
            'time' : new Date().toString(),
            left_click : event.isLeftClick()
        };
        Weebly.clickLog.push(log);
        if(Weebly.clickLog.size() > 10){
            Weebly.clickLog.shift();
        }
        if(!hasSentClickLog && excessiveDefs()){
            var history = Pages.history || [];
            var params = {
                pos: 'logwebkitbusted',
                pageid: currentPage,
                clicks: Object.toJSON(Weebly.clickLog),
                ajax: Weebly.ajaxLog ? Weebly.ajaxLog.toJSON() : '',
                dimensions: Object.toJSON(document.viewport.getDimensions()),
                history: history.join(', '),
                cookie: document.cookie 
            }
            new Ajax.Request(ajax, {parameters:params, bgRequest: true});
            hasSentClickLog = true;
        }
    });
    
    function excessiveDefs(){
        try{
            var el_id = $$('.outside_top')[0].down('[name=idfield]').value.replace(/[^\d]/g, '');
            var img_id = $$('.outside_top')[0].down('img').src.replace(/[^\d]/g, '');
            return el_id != img_id;
        }
        catch(e){
            return false;
        }
    }
}
var noJump	      = 0;

    // Windowing system in place allows a Pages.go('page', options) functionality
// -- The system takes care of showing or hiding specific pages
// Also "Fixes" browser back and forward buttons
// -------
var Pages = {
  Version        : '0.5',
  Author         : 'David Rusenko',
  Company        : 'Weebly, Inc.',
  openPages      : new Array('main'),
  currentNavPage : 'load',
  optConfirmLoad : 0,
  optAnimations  : 1,
  history        : [],
  lastPage       : '',

  load: function() {
  },
// List of pages, and their respective "start-up" functions
  pageConstructor: {
    main : {  }
  },

// List of pages, and their "shut down" or close functions
  pageDestructor: {
    main : {  }
  },
// List of windows that are allowed to be open for each page
  pageWindows: {
    main : ['main']
  },
  currentVar1: 'empty',
  currentVar2: 'empty',
  currentVar3: 'empty',

// go function... accepts up to three variables that it will pass on
// -- to the page creator function
  go: function(page, var1, var2, var3) {

Pages.updateHistory(page);
if (Weebly.Elements) { Weebly.Elements.unselectElement(); }

// Prevent people from navigating away from a current blog entry
if (typeof(currentBlog) != "undefined" && currentBlog.postId && currentBlog.postId == 1 && page != "goBlogPost" && Pages.pageWindows[page].indexOf('goBlogPost') == -1) {
  if (currentBlog.saving) {
    currentBlog.saving = 0;
  } else {
    var confirmLostPost = confirm(/*tl(*/"Warning: Your post is not saved. If you navigate away, you will lose your changes.\n\nTo keep your changes and continue editing, press Cancel.\n\nTo discard your post and continue your action, press OK."/*)tl*/);
    if (!confirmLostPost) {
      return;
    }
  }
}

    var v1 = typeof var1 == 'undefined' || var1 == '' || var1 == 'undefined' ? 0 : 1;
    var v2 = typeof var2 == 'undefined' || var2 == '' || var2 == 'undefined' ? 0 : 1;
    var v3 = typeof var3 == 'undefined' || var3 == '' || var3 == 'undefined' ? 0 : 1;

  if (page != Pages.currentNavPage || (var1 != Pages.currentVar1 && v1) || (var2 != Pages.currentVar2 && v2) || (var3 != Pages.currentVar3 && v3)) {

    //alert("openPages: "+Pages.openPages.join(","));
    //alert("Pages.go: "+page+" "+var1+" - CurrentPage: "+Pages.currentNavPage+" + "+Pages.currentVar1);

    Pages.handleMain(page);
    Pages.closeAll(page);

    if (noJump == 1) { noJump = 0 } else { window.scrollTo(0,0); }

    var thisElement = eval("Pages.pageConstructor."+page);
    if( thisElement && page != 'main') {           
  if( thisElement.element) {
    Pages.showElement(thisElement.element);
      }
      if( thisElement.go) { thisElement.go(var1, var2, var3); }
    }

    if (!window.litePageChange) {
    	// Automatically confirm page load if the optConfirmLoad setting is not set
	    if(!Pages.optConfirmLoad) Pages.confirmLoad(page, var1, var2, var3);
	}

    Pages.currentNavPage    = page;
    if(typeof var1 == 'undefined' || var1 == '' || var1 == 'undefined') { Pages.currentVar1 = 'empty'; } else { Pages.currentVar1 = var1; }
    if(typeof var2 == 'undefined' || var2 == '' || var2 == 'undefined') { Pages.currentVar2 = 'empty'; } else { Pages.currentVar2 = var2; }
    if(typeof var3 == 'undefined' || var2 == '' || var3 == 'undefined') { Pages.currentVar3 = 'empty'; } else { Pages.currentVar3 = var3; }

  }
  
  if (Pages.openPages.indexOf(page) == -1) {
  	Pages.openPages.push(page);
  }
  //console.log('--- Pages.currentNavPage: ' + Pages.currentNavPage);
  //console.log('--- Pages.openPages: ' + Pages.openPages);

},

// Shows pages according to rules
  showElement: function(showElement, animation) {

    if( Pages.optAnimations) { 
  setTimeout("Effect.Appear('"+showElement+"');", 1000); 
}
    else { Element.show(showElement); }

  },
// Special logic to handle the case of 'main' element, where the logic is reversed...
  handleMain: function(page) {

    if (eval ("Pages.pageWindows."+page+".indexOf('main');") > -1) {
      if( Pages.pageConstructor.main) {
        if( Pages.pageConstructor.main.element) {
          if( Pages.optAnimations) { setTimeout("Effect.Fade('"+Pages.pageConstructor.main.element+"')", 1000); }
          else { Element.hide(Pages.pageConstructor.main.element); }
        }
        if( Pages.pageConstructor.main.go) { Pages.pageConstructor.main.go(); }
      }
      if (Pages.openPages.indexOf('main') == -1) { Pages.openPages.push('main'); }
    } else {
      if( Pages.pageDestructor.main) {
        if( Pages.pageDestructor.main.element) {
          //var newHeight = Pages.pageDestructor.main.maxHeight ? eval("Math.max("+Pages.pageDestructor.main.maxHeight+");") : '600';
      var newHeight = 1000;
          Element.setStyle(Pages.pageDestructor.main.element, { height: newHeight });
          if( Pages.optAnimations) { Effect.Appear(Pages.pageDestructor.main.element); }
          else { Element.show(Pages.pageDestructor.main.element); }
        }
        if( Pages.pageDestructor.main.go) { Pages.pageDestructor.main.go(); }
      }
      if (Pages.openPages.indexOf('main') == -1) { Pages.openPages.push('main'); }
    }

// Hack to make sure main always shows when pressing back button and window.reload
// Not sure why Pages.pageConstructor.main.element isn't set in this case, weird
if (page == "main" && $('grayedOut').style.visibility == "visible") {
  Element.hide('grayedOut');
}

  },

// A page will confirm a successfull load through this function
  confirmLoad: function(pageName, var1, var2, var3) {

    if (Pages.openPages.indexOf(pageName) == -1) { Pages.openPages.push(pageName); }
    
    _goPageHistory("page="+pageName+"&1="+var1+"&2="+var2+"&3="+var3);

  },

// Close all windows except those specified in array pageWindows.currentWindow
  closeAll: function(currentWindow) {
    for ( j=Pages.openPages.length-1; j >= 0; j-- ) {
      var matches = 0;
      eval ("matches = Pages.pageWindows."+currentWindow+".indexOf(Pages.openPages[j]);");
      if (matches == -1 && Pages.openPages[j] != 'main') {
        var thisElement = eval("Pages.pageDestructor."+Pages.openPages[j]);
        if( thisElement) {
          if( thisElement.element) {
            if( Pages.optAnimations) { Effect.Fade($(thisElement.element)); }
            else { Element.hide($(thisElement.element)); }
          }
          if( thisElement.go) thisElement.go();
        }
        Pages.openPages.splice(j, 1);
      }
    }
  },

  returnToEditor: function(){
      if(Pages.openPages.indexOf('goBlogPost') > -1){
          Pages.go('goBlogPost');
      }
      else{
          Pages.go("main");
      }
  },
  
  updateHistory: function(page){
      if(page!=Pages.lastPage){
          Pages.history.push(page);
          if(Pages.history.size() > 10){
              Pages.history.shift();
          }
      }
      Pages.lastPage = page;
  }

};



var _goPageIframe;

function _goPageHistory(queryString) {
	var url = "/weebly/goPage.php?" + queryString;
	if (!_goPageIframe) {
		$(document.body).insert(_goPageIframe = new Element('iframe', {
			style: 'height:0;width:0;border:0;overflow:hidden;position:absolute;bottom:0px;left:0px;',
			frameBorder: 0
		}));
		_goPageIframe.src = url; // needs top be after creation, for webkit
	}else{
		_goPageIframe.contentWindow.location.href = url;
	}
}



//    Pages.load();

//------------
/// End of Pages module
////

var signupAction = '';

   function showSignup(myAction) {

	if (myAction == "exit") {
	  $('signupNote').innerHTML = "<font style='color: red; font-size: 18px;'>"+/*tl(*/"Sign-up to save your website."/*)tl*/+"</font><br/>"+/*tl(*/"Otherwise, click Cancel to exit."/*)tl*/;
	} else if (myAction == "publish") {
	  $('signupNote').innerHTML = "<font style='color: red; font-size: 18px;'>"+/*tl(*/"Sign-up to publish your website."/*)tl*/+"</font><br/>"+/*tl(*/"Otherwise, click Cancel to continue working."/*)tl*/;
        } else if (myAction == "zip") {
          $('signupNote').innerHTML = "<font style='color: red; font-size: 18px;'>"+/*tl(*/"Sign-up to export your website."/*)tl*/+"</font><br/>"+/*tl(*/"Otherwise, click Cancel to continue working."/*)tl*/;
	} else {
	  $('signupNote').innerHTML = "<font style='font-size: 22px; font-weight: bold;'>"+/*tl(*/"Sign-up now"/*)tl*/+"</font>";
	}

        new Effect.Move('signup', { y: 65, mode: 'absolute'});
	signupAction = myAction;
        return false;
   }

   function hideSignup() {
        new Effect.Move('signup', { y: -900, mode: 'absolute'});
        if (signupAction == "adsense") {
	  onHideLightbox('adsense');
	}
   }

   function submitSignup() {

        Element.setStyle('signupUser', { border: '1px solid #DDD' });
        Element.setStyle('signupPass', { border: '1px solid #DDD' });
        //Element.setStyle('signupPass2', { border: '1px solid #DDD' });
        Element.setStyle('signupEmail', { border: '1px solid #DDD' });
        Element.setStyle('signupInvitationID', { border: '1px solid #DDD' });

        Element.hide('signupError');

        var proceedForm = 1;
        if( $('signupUser').value.match(/[^a-zA-Z0-9\-\_]/)) {
          Element.setStyle('signupUser', { border: '2px solid red' });
          showTip(/*tl(*/'Your username may only contain numbers, letters, a dash (-) or an underscore (_).'/*)tl*/, 'signupUser');
          proceedForm = 0;
        } else if( $('signupUser').value == '') {
          Element.setStyle('signupUser', { border: '2px solid red' });
          showTip(/*tl(*/'Please enter a username.'/*)tl*/, 'signupUser');
          proceedForm = 0;
        }
        if( $('signupPass').value.match(/.{17}/)) {
          Element.setStyle('signupPass', { border: '2px solid red' });
          showTip(/*tl(*/'Your password is too long. It can be a maximum of 16 characters long.'/*)tl*/, 'signupPass');
          proceedForm = 0;
        } else if( !$('signupPass').value.match(/.{4}/)) {
          Element.setStyle('signupPass', { border: '2px solid red' });
          showTip(/*tl(*/'Your password is not long enough. It must be at least 4 characters long.'/*)tl*/, 'signupPass');
          proceedForm = 0;
        } else if( $('signupPass').value == '') {
          Element.setStyle('signupPass', { border: '2px solid red' });
          showTip(/*tl(*/'Please enter a password.'/*)tl*/, 'signupPass');
          proceedForm = 0;
        }/* else if( $('signupPass').value != $('signupPass2').value) {
          Element.setStyle('signupPass2', { border: '2px solid red' });
          showTip('Your passwords to not match. Please try again.', 'signupPass2');
          proceedForm = 0;
        }*/
	if ($('signupEmail').value == '' || !$('signupEmail').value.match("@")) {
	  Element.setStyle('signupEmail', { border: '2px solid red' });
          showTip(/*tl(*/'Please enter an email.'/*)tl*/, 'signupEmail');
          proceedForm = 0;
	}

        if(proceedForm == 1) {

           new Ajax.Request(ajax, {parameters:'pos=signup&user='+encodeURIComponent($F('signupUser'))+'&pass='+encodeURIComponent($F('signupPass'))+'&email='+encodeURIComponent($F('signupEmail'))+'&cookie='+document.cookie, onSuccess: handlerSubmitSignup, onFailure:errFunc});

        }

   }

   function handlerSubmitSignup(t) {

        if (t.responseText.indexOf('%%SUCCESS%%') > -1) {

		tempUser = 0;

		if (signupAction == "exit") {
		  Pages.go('userHome'); 
		} else if (signupAction == "publish") {
		  Pages.go('exportSite');
		} else if (signupAction == "zip") {
		  $("exportSiteZipFrame").src = "downloadZip.php?"+Math.floor(Math.random()*1001);
		  Pages.go('main');
		} else {
		  Pages.go('main');
		}

		$('weebly-signup-button').innerHTML = '<a class="weebly-top-links" href="#" onClick="Pages.go(\'userHome\'); return false;"><img style="position: relative; top: -1px;" src="images/action_stop.gif" /> <font style="position: relative; top: -5px; color: white;">'+/*tl(*/'Close'/*)tl*/+'</font></a>';

	} else {

          $('signupError').innerHTML = t.responseText;
	  if (t.responseText == '') { $('signupError').innerHTML = /*tl(*/'There was an error creating your account. Please try again.'/*)tl*/; }
          Effect.Appear('signupError');

        }

   }

   function resetPassword() {

        Element.hide('signupError');

        new Ajax.Request('/weebly/publicBackend.php', {parameters:'pos=resetpassword&email='+$F('resetEmail'), onSuccess:handlerResetPassword, onFailure:errFunc});

   }

   function handlerResetPassword(t) {

        if (t.responseText.indexOf('%%SUCCESS%%') > -1) {

	  $('resetSuccess').innerHTML = /*tl(*/"Instructions for reseting your password have been sent. Please check your email. <a href='http://www.weebly.com/'>Click here</a> to return to the main page."/*)tl*/;
	  Effect.Appear('resetSuccess');
        } else {

          $('signupError').innerHTML = t.responseText;
          Effect.Appear('signupError');

        }

   }

   function changePassword()
   {
        Element.hide('signupError');
        new Ajax.Request('/weebly/publicBackend.php', {parameters:'pos=changepassword&new_password='+encodeURIComponent($F('resetPass2'))+'&v='+$F('resetKey'), onSuccess:handlerChangePassword, onFailure:errFunc});
   }

   function handlerChangePassword(t)
   {
        if (t.responseText.indexOf('%%SUCCESS%%') > -1)
        {
          $('resetSuccess').innerHTML = /*tl(*/"Your password has been succesfully changed. <a href='http://www.weebly.com/'>Click here</a> to return to the main page and login."/*)tl*/;
          Effect.Appear('resetSuccess');
        }
        else
        {
          $('signupError').innerHTML = t.responseText;
          if (t.responseText == '') { $('signupError').innerHTML = 'Error'; }
          Effect.Appear('signupError');
        }
   }

/* Copyright (c) 2006 Yahoo! Inc. All rights reserved. */

var YAHOO = function() { return {util: {}} } ();
YAHOO.util.Color = new function() {
  
  // Adapted from http://www.easyrgb.com/math.html
  // hsv values = 0 - 1
  // rgb values 0 - 255
  this.hsv2rgb = function (h, s, v) {
    var r, g, b;
    if ( s == 0 ) {
      r = v * 255;
      g = v * 255;
      b = v * 255;
    } else {

      // h must be < 1
      var var_h = h * 6;
      if ( var_h == 6 ) {
        var_h = 0;
      }

      //Or ... var_i = floor( var_h )
      var var_i = Math.floor( var_h );
      var var_1 = v * ( 1 - s );
      var var_2 = v * ( 1 - s * ( var_h - var_i ) );
      var var_3 = v * ( 1 - s * ( 1 - ( var_h - var_i ) ) );

      if ( var_i == 0 ) { 
        var_r = v; 
        var_g = var_3; 
        var_b = var_1;
      } else if ( var_i == 1 ) { 
        var_r = var_2;
        var_g = v;
        var_b = var_1;
      } else if ( var_i == 2 ) {
        var_r = var_1;
        var_g = v;
        var_b = var_3
      } else if ( var_i == 3 ) {
        var_r = var_1;
        var_g = var_2;
        var_b = v;
      } else if ( var_i == 4 ) {
        var_r = var_3;
        var_g = var_1;
        var_b = v;
      } else { 
        var_r = v;
        var_g = var_1;
        var_b = var_2
      }

      r = var_r * 255          //rgb results = 0 ?255
      g = var_g * 255
      b = var_b * 255

      }
    return [Math.round(r), Math.round(g), Math.round(b)];
  };

  // added by Matthias Platzer AT knallgrau.at 
  this.rgb2hsv = function (r, g, b) {
      var r = ( r / 255 );                   //RGB values = 0 ?255
      var g = ( g / 255 );
      var b = ( b / 255 );

      var min = Math.min( r, g, b );    //Min. value of RGB
      var max = Math.max( r, g, b );    //Max. value of RGB
      deltaMax = max - min;             //Delta RGB value

      var v = max;
      var s1 = 0; var h = 0;
      var deltaRed, deltaGreen, deltaBlue;

      if ( deltaMax == 0 )                     //This is a gray, no chroma...
      {
         h = 0;                               //HSV results = 0 ?1
         s1 = 0;
      }
      else                                    //Chromatic data...
      {
         s1 = deltaMax / max;

         deltaRed = ( ( ( max - r ) / 6 ) + ( deltaMax / 2 ) ) / deltaMax;
         deltaGreen = ( ( ( max - g ) / 6 ) + ( deltaMax / 2 ) ) / deltaMax;
         deltaBlue = ( ( ( max - b ) / 6 ) + ( deltaMax / 2 ) ) / deltaMax;

         if      ( r == max ) h = deltaBlue - deltaGreen;
         else if ( g == max ) h = ( 1 / 3 ) + deltaRed - deltaBlue;
         else if ( b == max ) h = ( 2 / 3 ) + deltaGreen - deltaRed;

         if ( h < 0 ) h += 1;
         if ( h > 1 ) h -= 1;
      }

      return [h, s1, v];
  }

  this.rgb2hex = function (r,g,b) {
    return this.toHex(r) + this.toHex(g) + this.toHex(b);
  };

  this.hexchars = "0123456789ABCDEF";

  this.toHex = function(n) {
    n = n || 0;
    n = parseInt(n, 10);
    if (isNaN(n)) n = 0;
    n = Math.round(Math.min(Math.max(0, n), 255));

    return this.hexchars.charAt((n - n % 16) / 16) + this.hexchars.charAt(n % 16);
  };

  this.toDec = function(hexchar) {
    return this.hexchars.indexOf(hexchar.toUpperCase());
  };

  this.hex2rgb = function(str) { 
    var rgb = [];
    rgb[0] = (this.toDec(str.substr(0, 1)) * 16) + 
            this.toDec(str.substr(1, 1));
    rgb[1] = (this.toDec(str.substr(2, 1)) * 16) + 
            this.toDec(str.substr(3, 1));
    rgb[2] = (this.toDec(str.substr(4, 1)) * 16) + 
            this.toDec(str.substr(5, 1));
    // gLogger.debug("hex2rgb: " + str + ", " + rgb.toString());
    return rgb;
  };

  this.isValidRGB = function(a) { 
    if ((!a[0] && a[0] !=0) || isNaN(a[0]) || a[0] < 0 || a[0] > 255) return false;
    if ((!a[1] && a[1] !=0) || isNaN(a[1]) || a[1] < 0 || a[1] > 255) return false;
    if ((!a[2] && a[2] !=0) || isNaN(a[2]) || a[2] < 0 || a[2] > 255) return false;

    return true;
  };
}


/* 
   colorPicker for script.aculo.us, version 0.9
   REQUIRES prototype.js, yahoo.color.js and script.aculo.us
   written by Matthias Platzer AT knallgrau.at
   for a detailled documentation go to http://www.knallgrau.at/code/colorpicker 
 */

if(!Control) var Control = {};
Control.colorPickers = [];
Control.ColorPicker = Class.create();
Control.ColorPicker.activeColorPicker;
Control.ColorPicker.CONTROL;
/**
 * ColorPicker Control allows you to open a little inline popUp HSV color chooser.
 * This control is bound to an input field, that holds a hex value.
 */
Control.ColorPicker.prototype = {
  initialize : function(field, options) {
    var colorPicker = this;
    Control.colorPickers.push(colorPicker);
    this.field = $(field);
    this.fieldName = this.field.name || this.field.id;
    this.options = Object.extend({
       IMAGE_BASE : "img/"
    }, options || {});
    this.swatch = $(this.options.swatch) || this.field;
    this.rgb = {};
    this.hsv = {};
    this.isOpen = false;

    // create control (popUp) if not already existing
    // all colorPickers on a page share the same control (popUp)
    if (!Control.ColorPicker.CONTROL) {
      Control.ColorPicker.CONTROL = {};
      if (!$("colorpicker")) {
        var control = Builder.node('div', {id: 'colorpicker', className: 'colorpicker'});
        control.innerHTML = 
	    '<div id="colorpicker-div">' + (
            // apply png fix for ie 5.5 and 6.0
            (/MSIE ((6)|(5\.5))/gi.test(navigator.userAgent) && /windows/i.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent)) ?
              '<img id="colorpicker-bg" src="' + this.options.IMAGE_BASE + 'blank.gif" style="filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(src=\'' + this.options.IMAGE_BASE + 'pickerbg.png\', sizingMethod=\'scale\')" alt="">' :
              '<img id="colorpicker-bg" src="' + this.options.IMAGE_BASE + 'pickerbg.png" alt="">'
             ) +
          '<div id="colorpicker-bg-overlay" style="z-index: 1002;"></div>' +
          '<div id="colorpicker-selector"><img src="' + this.options.IMAGE_BASE + 'select.gif" width="11" height="11" alt="" /></div></div>' +
          '<div id="colorpicker-hue-container"><img src="' + this.options.IMAGE_BASE + 'hue.png" id="colorpicker-hue-bg-img"><div id="colorpicker-hue-slider"><div id="colorpicker-hue-thumb"><img src="' + this.options.IMAGE_BASE + 'hline.png"></div></div></div>' + 
          '<div id="colorpicker-footer"><span id="colorpicker-value">#<input type="text" onclick="this.select()" id="colorpicker-value-input" name="colorpicker-value" value=""></input></span><button id="colorpicker-okbutton">OK</button></div>';
        $('colorChooserDiv').appendChild(control);
      }
      Control.ColorPicker.CONTROL = {
        popUp : $("colorpicker"),
        pickerArea : $('colorpicker-div'),
        selector : $('colorpicker-selector'),
        okButton : $("colorpicker-okbutton"),
        value : $("colorpicker-value"),
        input : $("colorpicker-value-input"),
        picker : new Draggable($('colorpicker-selector'), {
          snap: function(x, y) {
            return [
              Math.min(Math.max(x, 0), Control.ColorPicker.activeColorPicker.control.pickerArea.offsetWidth), 
              Math.min(Math.max(y, 0), Control.ColorPicker.activeColorPicker.control.pickerArea.offsetHeight)
            ];
          },
          zindex: 1009,
          change: function(draggable) {
            var pos = draggable.currentDelta();
            Control.ColorPicker.activeColorPicker.update(pos[0], pos[1]);
          }
        }),
        hueSlider: new Control.Slider('colorpicker-hue-thumb', 'colorpicker-hue-slider', {
          axis: 'vertical',
          onChange: function(v) {
            Control.ColorPicker.activeColorPicker.updateHue(v);
          }
        })
      };
      Element.hide($("colorpicker"));
    }
    this.control = Control.ColorPicker.CONTROL;

    // bind event listener to properties, so we can use them savely with Event[observe|stopObserving]
    this.toggleOnClickListener = this.toggle.bindAsEventListener(this);
    this.updateOnChangeListener = this.updateFromFieldValue.bindAsEventListener(this);
    // David - changed "close" to "toggle" -- seems to fix some obscure bug in IE 5.5/6.0
    this.closeOnClickOkListener = this.toggle.bindAsEventListener(this);
    this.updateOnClickPickerListener = this.updateSelector.bindAsEventListener(this);

    Event.observe(this.swatch, "click", this.toggleOnClickListener);
    Event.observe(this.field, "change", this.updateOnChangeListener);
    Event.observe(this.control.input, "change", this.updateOnChangeListener);

    this.updateSwatch();
  },
  toggle : function(event) {
    this[(this.isOpen) ? "close" : "open"](event);
    Event.stop(event);    
  },
  open : function(event) {
	/**
    var tmpThis = this;
    Control.colorPickers.each(function(colorPicker) {
      // David -- don't close the current colorPicker instance
      if (tmpThis != colorPicker) { colorPicker.close(); console.log("Closing!"); }
    });
	**/
    Control.ColorPicker.activeColorPicker = this;
    this.isOpen = true;
    Element.show(this.control.popUp);
    if (this.options.getPopUpPosition) {
      var pos = this.options.getPopUpPosition.bind(this)(event);
    } else {
      var pos = Position.cumulativeOffset(this.swatch || this.field);
      //pos[0] = (pos[0] + (this.swatch || this.field).offsetWidth + 10);
      pos[1] = (pos[1] + (this.swatch || this.field).offsetHeight + 15);
    }
    // David!
    /*if (this.options.position == "toolbar") {
      pos[0] = '345';
      pos[1] = '165';
      Weebly.Linker.close();
    }*/
    this.control.popUp.style.left = (pos[0]) + "px";
    this.control.popUp.style.top = (pos[1]) + "px";
    this.initial = 1;
    this.updateFromFieldValue();
    Event.observe(this.control.okButton, "click", this.closeOnClickOkListener);
    Event.observe(this.control.pickerArea, "mousedown", this.updateOnClickPickerListener);
    if (this.options.onOpen) this.options.onOpen.bind(this)(event);
  },
  close : function(event) {
    if (Control.ColorPicker.activeColorPicker == this) Control.ColorPicker.activeColorPicker = null;
    this.isOpen = false;
    Element.hide(this.control.popUp);
    Event.stopObserving(this.control.okButton, "click", this.closeOnClickOkListener);
    Event.stopObserving(this.control.pickerArea, "mousedown", this.updateOnClickPickerListener);
    if (this.options.onClose) this.options.onClose.bind(this)();
  },
  updateHue : function(v) {
    var h = (this.control.pickerArea.offsetHeight - v * 100) / this.control.pickerArea.offsetHeight;
    if (h == 1) h = 0;
    var rgb = YAHOO.util.Color.hsv2rgb( h, 1, 1 );
    if (!YAHOO.util.Color.isValidRGB(rgb)) return;
    this.control.pickerArea.style.backgroundColor = "rgb(" + rgb[0] + ", " + rgb[1] + ", " + rgb[2] + ")";
    this.update();
  },
  updateFromFieldValue : function(event) {
    if (!this.isOpen) return;
    var field = (event && Event.findElement(event, "input")) || this.field;
    var rgb = YAHOO.util.Color.hex2rgb( field.value );
    if (!YAHOO.util.Color.isValidRGB(rgb)) return;
    var hsv = YAHOO.util.Color.rgb2hsv( rgb[0], rgb[1], rgb[2] );
    this.control.selector.style.left = Math.round(hsv[1] * this.control.pickerArea.offsetWidth) + "px";
    this.control.selector.style.top = Math.round((1 - hsv[2]) * this.control.pickerArea.offsetWidth) + "px";
    this.control.hueSlider.setValue((1 - hsv[0]));
  },
  updateSelector : function(event) {
    var xPos = Event.pointerX(event);
    var yPos = Event.pointerY(event);
    var pos = Position.cumulativeOffset($("colorpicker-bg"));
    this.control.selector.style.left = (xPos - pos[0] - 6) + "px";
    this.control.selector.style.top = (yPos - pos[1] - 6) + "px";
    this.update((xPos - pos[0]), (yPos - pos[1]));
    this.control.picker.initDrag(event);
  },
  updateSwatch : function() {
    var rgb = YAHOO.util.Color.hex2rgb( this.field.value );
    if (!YAHOO.util.Color.isValidRGB(rgb)) return;
    this.swatch.style.backgroundColor = "rgb(" + rgb[0] + ", " + rgb[1] + ", " + rgb[2] + ")";
    var hsv = YAHOO.util.Color.rgb2hsv( rgb[0], rgb[1], rgb[2] );
    this.swatch.style.color = (hsv[2] > 0.65) ? "#000000" : "#FFFFFF";
  },
  update : function(x, y) {

    // David - Don't update the first time, can lose colors due to not being precise
    if (this.initial) { this.initial = null; this.control.input.value = this.field.value; return; }

    if (!x) x = this.control.picker.currentDelta()[0];
    if (!y) y = this.control.picker.currentDelta()[1];

    var h = (this.control.pickerArea.offsetHeight - this.control.hueSlider.value * 100) / this.control.pickerArea.offsetHeight;
    if (h == 1) { h = 0; };
    this.hsv = {
      hue: 1 - this.control.hueSlider.value,
      saturation: x / this.control.pickerArea.offsetWidth,
      brightness: (this.control.pickerArea.offsetHeight - y) / this.control.pickerArea.offsetHeight
    };
    var rgb = YAHOO.util.Color.hsv2rgb( this.hsv.hue, this.hsv.saturation, this.hsv.brightness );
    this.rgb = {
      red: rgb[0],
      green: rgb[1],
      blue: rgb[2]
    };
    this.field.value = YAHOO.util.Color.rgb2hex(rgb[0], rgb[1], rgb[2]);
    this.control.input.value = this.field.value;
    this.updateSwatch();
    if (this.options.onUpdate) this.options.onUpdate.bind(this)(this.field.value);
  }
}

    // Linker system allows for a flexible, extensible linker that enables users
    // to create links to various types of objects throughout the application.
    // -------
    Weebly.Linker = {
      Version        : '0.1',
      Author         : 'David Rusenko',
      Company        : 'Weebly, Inc.',
      optDefault : 0,
      callbackFunc : '',
      currentURL : '',
      currentContentFieldID: '',
      passValidation: 1,
      activeElement: '',
      modules: Array('linkerWebsite','linkerWeebly','linkerFile','linkerImage','linkerEmail'),

      show: function(callback, position, displayedLinks, activeLink, contentFieldID, showRemoveLink) {

	Weebly.Linker.callbackFunc = callback;
	Weebly.Linker.currentContentFieldID = contentFieldID;
	
	// Hide 'Remove Link' link
	if (showRemoveLink) {
		$('linkerRemoveLink').show();
	}else{
		$('linkerRemoveLink').hide();
	}

	// Hide color picker
        //Element.setStyle('colorpicker', {'display':'none'} );


	// Hide all modules
	for (var x=0; x < Weebly.Linker.modules.length; x++) {
	  Element.hide(Weebly.Linker.modules[x]);
	}

	// Show passed modules
	for (var x=0; x < displayedLinks.length; x++) {
	  Element.show(displayedLinks[x]);
	}

        var width= ((document.body.clientWidth - 550) / 2 )-5;

	Weebly.Linker.active(activeLink);

	//Element.setStyle('createLink', {'top':position.top+'px', 'left':position.left+'px'} );

	Element.setStyle('createLink', {'top':position.top+'px', 'left':width+'px'} );

	Element.show('createLink');

	// If Linker will be partially hidden, move up enough that it's completely visible
	// Dan, you're such a pain in my ass, but I'm sure our users thank you...
	var myScrollTop = getScrollTop();
	if (position.top + $('createLink').clientHeight > window.innerHeight + myScrollTop) {
	  var newPosition = window.innerHeight + myScrollTop - $('createLink').clientHeight - 5;
	  Element.setStyle('createLink', {'top': newPosition +'px'} );
	}

      },

      save: function(doRemove) {

	if (Weebly.Linker.gatherDataFunc[Weebly.Linker.activeElement]) {
	  Weebly.Linker.gatherDataFunc[Weebly.Linker.activeElement]();
	}

	Weebly.Linker.passValidation = 1;
	if (!doRemove && Weebly.Linker.validateFunc[Weebly.Linker.activeElement]) {
	  var returnVar = Weebly.Linker.validateFunc[Weebly.Linker.activeElement]($('createURL').value);
	  if (returnVar != 1) {
	    $('linkerError').innerHTML = returnVar;
	    Weebly.Linker.passValidation = 0;
	  } else {
	    $('linkerError').innerHTML = '';
	  }
	}

	if (Weebly.Linker.passValidation) {

	  Weebly.Linker.currentURL = doRemove ? '' : $('createURL').value;
	  eval(Weebly.Linker.callbackFunc+'("'+Weebly.Linker.currentURL+'");');
      if( Weebly.Linker.activeElement === 'linkerWebsite' && $('webpageNewWindow').checked && $(currentBox+'Edit') )
      {
        var anchors = document.getElementById(currentBox+'Edit').contentWindow.document.getElementsByTagName('a');
        for( var i=0; i<anchors.length; i++ )
        {
          if( anchors[i].href.match('weeblylink_new_window') )
          {
            anchors[i].href = anchors[i].href.replace('weeblylink_new_window','');
            anchors[i].target = '_blank';
          }
        }
      }

	}

      },	

      active: function(activeElement) {

	// Set active linker element
	Weebly.Linker.activeElement = activeElement;

        // Unselect all modules
        for (var x=0; x < Weebly.Linker.modules.length; x++) {
          $(Weebly.Linker.modules[x]).className = "linkerMenuItem";
        }

        // Select passed modules
        $(activeElement).className = "linkerMenuItem-selected";

	// Reset error text
	$('linkerError').innerHTML = '';

	$("linkerContents").innerHTML = Weebly.Linker.elementContents[activeElement];
	new Ajax.Request(ajax, {parameters:'pos=linker&keys='+activeElement+'&cookie='+document.cookie, onSuccess:Weebly.Linker.paste, onFailure:errFunc});

      },

    // Handle AJAX response by pasting in HTML code
      paste: function(t) {

	$("linkerContents").innerHTML += t.responseText;

      },

    // Close linker
      close: function() {

	Element.hide('createLink');

      },

      saveAndClose: function(doRemove) {

	Weebly.Linker.save(doRemove);

	if (Weebly.Linker.passValidation) {
	  Weebly.Linker.close();
	}

      },
      
      removeLink: function() {
      	Weebly.Linker.saveAndClose(true);
      },

      elementContents: {

	'linkerWebsite': /*tl(*/"Enter the webpage you would like to link below. <br/><i>(ie, www.meebo.com/)</i>"/*)tl*/+"<br/><br/><select id='webpageStart' style='width: 72px; margin-right: 5px;'><option value='http://'>http://</option><option value='https://'>https://</option><option value='ftp://'>ftp://</option><option value='ftps://'>ftps://</option><option value='aim:goim?screenname='>aim:goim?screenname=</option></select><input type='text' id='webpageURL' value='www.' style='width: 250px; color: #cccccc;' onclick='if (this.value == \"www.\") { this.value = \"\"; }' onkeyup='this.value = this.value.replace(/^https?:\\/\\//, \"\"); this.value = this.value.replace(/^ftps?:\\/\\//, \"\"); this.style.color = \"#303030\";' /><br /><input type='checkbox' id='webpageNewWindow' style='margin-left:77px; margin-top:7px;' /> "+/*tl(*/"Open link in new window"/*)tl*/+"<br/><br/>",

	'linkerWeebly': /*tl(*/"Select the page you would like to link to below.<br/><br/>"/*)tl*/,

	'linkerImage': /*tl(*/"Your picture will be linked to the full size version of this image.<br/><br/>"/*)tl*/,
	'linkerEmail': /*tl(*/"Enter the email address you would like to link below. <br/><i>(ie, feedback@weebly.com)</i><br/><br/>Email"/*)tl*/+": <input type='text' id='emailLink' value='' style='width: 250px;' /><br/><br/>",

	'linkerFile':  /*tl(*/"Select the file you would like to link to below.<br/><br/>"/*)tl*/

      },
      gatherDataFunc: {

	'linkerWeebly': function() {
	  $('createURL').value = $('weeblyPageID').value;
	},

	'linkerWebsite': function() {
	  $('webpageURL').value = $('webpageURL').value.replace(/^[^\?]*:\/\//, '');
	  $('createURL').value = $('webpageStart').value+$('webpageURL').value;
      if($('webpageNewWindow').checked)
      {
        $('createURL').value += 'weeblylink_new_window';
      }
	},
	
	'linkerEmail': function() {
	  $('createURL').value = 'mailto:'+$('emailLink').value;
	},

	'linkerFile': function() {
	  $('createURL').value = $('weeblyFileID').value;
	},
	'linkerImage': function() {
	  $('createURL').value = "http://weebly-image-full/"+Weebly.Elements.currentElement.id;
	}

      },
      validateFunc: {
	'linkerWebsite': function(validateMe) {

	  if (validateMe == 'http://asdf') {
	    return "Invalid URL.";
	  } else {
	    return 1;
	  }

	},

	'linkerEmail': function(validateMe) {

	  if (validateMe.match(/^mailto:[a-zA-Z0-9\-\_\.\+\~\#\*]+@[a-zA-Z0-9\-]+\.[a-zA-Z]+[a-z0-9A-Z\.\-\?\=\&]*$/)) {
	    return 1;
	  } else {
	    return /*tl(*/"Invalid email address. Please enter a correct email address."/*)tl*/;
	  }

	},

	'linkerFile': function(validateMe) {

	  new Ajax.Request(ajax, {parameters:'pos=linker&keys=linkresource:'+Weebly.Linker.currentContentFieldID+':'+validateMe+'&cookie='+document.cookie, onSuccess:function(t) { }, onFailure:errFunc});

	  return 1;

	}

      }


    };

    //------------
    /// End of Linker module
    ////


    // Linker system allows for a flexible, extensible linker that enables users
    // to create links to various types of objects throughout the application.
    // -------
    Weebly.Cache = {
      Version        : '0.1',
      Author         : 'David Rusenko',
      Company        : 'Weebly, Inc.',
      activeElement: '',
      inCache: Object(),

      get: function(call, url, callbackFunc, callbackVars, options) {

	if (typeof(Weebly.Cache.inCache[url]) != "undefined" && Weebly.Cache.inCache[url] != "empty") {

	  Weebly.Cache.makeCall(url, callbackFunc, callbackVars);

	} else {

	  var async = true;
      if(options && options.asynchronous === false){
          async = false;
      }
      new Ajax.Request(call, {parameters:url+'&cookie='+document.cookie, onSuccess: function(t) { Weebly.Cache.requestHandler(t, url, callbackFunc, callbackVars) }, onFailure:errFunc, asynchronous: async});

	}

      },

      requestHandler: function(t, url, callbackFunc, callbackVars) {

	Weebly.Cache.insert(url, t.responseText);
	Weebly.Cache.makeCall(url, callbackFunc, callbackVars);

      },

      makeCall: function(url, callbackFunc, callbackVars) {

	var pString = new Array(); var x = 0;
	for (myVar in callbackVars) {

	  var toWrite = callbackVars[myVar];

	  if (typeof(toWrite) == "string") {
	    toWrite = toWrite.replace(/\n/g, "");
	    toWrite = toWrite.replace(/\r/g, "");
	    toWrite = toWrite.replace(/'/g, "\\'");
	  }

	  pString[x] = "'" + toWrite + "'";
	  x++;
	}
	
	if (pString.length > 0) {
	  eval( "callbackFunc( Weebly.Cache.inCache[url], " + pString.join(", ") + ")" );
	} else {
	  callbackFunc(Weebly.Cache.inCache[url]);
	}

      },

      insert: function(url, content) {

	if (typeof url == 'string') {
	  //alert("Inserting into Weebly.Cache '"+url+"': "+content);
	  Weebly.Cache.inCache[url] = content;
	} else if (typeof url == 'object') {
	  for (cacheElement in url) {
	    Weebly.Cache.inCache[cacheElement] = url[cacheElement];
	  }
	}

      },	

    // Clear Cache
      clear: function(toClear) {

	if (typeof toClear == 'string') {
	  Weebly.Cache.inCache[toClear] = "empty";
	} else if (typeof toClear == 'undefined') {
	  Weebly.Cache.inCache = { };
	}

      },

    // Dump current Cache to an element
      dump: function(dumpElement) {

	var tmpCache = 'Weebly.Cache.inCache = (\n';

	for (cacheElement in Weebly.Cache.inCache) {
	  tmpCache += "    '"+cacheElement+"' : '"+Weebly.Cache.inCache[cacheElement]+"', \n";
	}

	tmpCache += "\n);";

	$(dumpElement).innerHTML = tmpCache;

      }

    };

    //------------
    /// End of Cache module
    ////


var previousItems     = 0;
var previousPages     = 0;
var currentSite	      = 0;
var siteType	      = null;
var currentPage       = 0;
var previousPage      = 0;
var recentDrag        = 0;
var currentElement    = 0;
var currentThemesPage = 0;
var imageUploadSize   = 0;
var userID            = 0;
var userIDLocation    = '';
var publishingWindow  = 0;
var publishingAnim    = 0;
var publishingLoc     = 'left';
var pageDblClick      = 0;
var uploadUpdater     = new Object();
var currentMenu       = '';
var oldSecondList     = '';
var oldPages	      = '';
var dontUpdateList    = 0;
var trashItem	      = '';
var deleting	      = 0;
var scriptSrc  	      = new Array();
var scriptId   	      = new Array();
var scriptType 	      = new Array();
var sitePages 	      = new Object();	// deprecated, use Weebly.PageManager.pages[].title
var originalSitePages = new Object();	// deprecated?
var pageOrder         = [];				// deprecated, use Weebly.PageManager.topLevelPages[].id
var tempUser	      = 0;
var tempDeleted	      = { };
var iFramesCache      = new Object();
var iFramesHeight     = new Object();
var originalPages     = '';
var documentWriteElement = 'body';
var currentThemesPerPage = 8;
var currentThemesCategory= '';
var validateFunction = function() { validateOK(); }; //saveProperties('properties','"+currentElement+"');};
var validateChangeFunction = function() {};
var headerSelected    = null;
var headerDimensions  = Array();
var uploadId	      = null;
var currentHeader     = null;
var resetScrollTop    = null;
var destroySecondList = null;
var interfaceActive   = null;
var editThemeMode     = null;
var updatedTheme      = null;
var friendRequests    = 0;
var adsenseID         = 0;
var currentImageNum   = Math.floor(Math.random()*10000000001);
var footerCodeShown   = false;
var newSitePassword   = "";
var hideTitle 	      = 0;
var purchaseReferer   = "";
var currentSiteLocation="";
var userEvents	      = {};
var forceFlashPosition = false;
Prototype.Browser.IE6 = Prototype.Browser.IE && parseInt(navigator.userAgent.substring(navigator.userAgent.indexOf("MSIE")+5))==6;

    function inEditor()
    {
	return true;
    }

    function setWidth() // actually sets both width and height in response to a user window resize
    {
        var width, doubleWidth, fullheight, height, textWidth;

	fullheight = getInnerHeight();        

        width= ((document.body.clientWidth - 600) / 2 ) -23;
	doubleWidth = width * 2 - 30;
        textWidth = (document.body.clientWidth - 600) / 2;
        //if (document.body.clientWidth < 750) document.location = "error.pl?resolution";
        
	Element.setStyle('emptyList', {left:(width+100)+'px'} );

	//Effect.Center('textEditor');
	//Element.setStyle('textEditor', {marginLeft:textWidth+'px'} );       

	height = fullheight - 133;
	
	Element.setStyle('scroll_container', {height: height+'px'} );
	Element.setStyle('scroll_container_properties', {height: height+'px'} );
	Element.setStyle('domainContainer', {height: (height+98)+'px'} );
	Element.setStyle('grayedOut', {height: height+'px'} );
	Element.setStyle('icontent_container', {'minHeight': height+'px'});
	Element.setStyle('newContainer', {height: (height)+'px'} );

	if( $('customThemeContainer' ) )
	{
		Element.setStyle('customThemeContainer', {height: (ENABLE_THEME_BROWSER?fullheight:height)+'px'} );
		Element.setStyle('themeEditBoxContainer', {height: ((ENABLE_THEME_BROWSER?fullheight:height)-36)+'px'} );
		if (typeof(themeEditBox) != "undefined") {
	          themeEditBox.style.height = $('themeEditBoxContainer').getHeight()+"px";
	          themeEditBox.style.width = $('themeEditBoxContainer').getWidth()+"px";
	          themeEditBox.style.border = "none";
		}
	}

	Element.setStyle('grayedOut', {height:(getInnerHeight()-35)+'px'} );
	Element.setStyle('grayedOutFull', {height:(getInnerHeight())+'px'} );
	Element.setStyle('helpFrame', {height:(getInnerHeight()-35)+'px'} );
	Element.setStyle('helpIframe', {height:(getInnerHeight()-55)+'px'} );
	Element.setStyle('pagesContainer', {height:(getInnerHeight()-35)+'px'} );

	Element.setStyle('menuBarDiv', {width:(document.body.clientWidth-27)+'px'} );

	if (siteType == 'myspace') {
	  Element.setStyle('elementlist', {width:(document.body.clientWidth)+'px'});
	} else {
	  Element.setStyle('elementlist', {width:(document.body.clientWidth-150)+'px'});
	}

	if (editThemeMode == 1) {
	  $('currentThemeOptions').style.width = (document.body.clientWidth-130)+"px";
          var moveTo = document.body.clientWidth <= 800 ? 215 : 305;
          $('themePictures').style.left = (document.body.clientWidth-moveTo)+"px";

	}
	
		if (window.resizeThemeBrowser) {
			window.resizeThemeBrowser(document.body.clientWidth, height, fullheight);
		}

    }

    function setHeight()
    {
        var height;
        height= (document.body.clientHeight - 100);

        Element.setStyle('initial_loading', {height:height+'px'} );

    }

    function setInterfaceEnabled() {
	
	interfaceActive = 1;
	Behaviour.apply();
	
    }

    function setInterfaceDisabled() {

	interfaceActive = null;
	initContentDraggables();

    }

    function handlerFuncMiddle(t) {
    
      Weebly.TimingTest.end(Weebly.TimingTest.updateListTest+"_ajax");
      var resText = t.responseText;

	//Clear out Sortables
	initContentDraggables();

        // Update icontent DIV's class name
        try {
          var pageLink = t.getHeader("Weebly-Page-Link");
          if (pageLink) {
            $('icontent').className = pageLink;
          }
        } catch (e) { }

	// Show AdSense setup if not configured
	if (resText.match(/serveAds\.php\?type=adsense/) && !adsenseID) {
	  Pages.go("adsenseSetup");
	}

        // Add "remove" button to footerCode span
        if ($('footerCode') && !$('footerCode').innerHTML.match("footer_remove.png") && Weebly.Restrictions.hasAccess("show_footer_removebutton")) {
          if (isPro()) {
            $('footerCode').innerHTML += "<a href='#' onclick='hideFooter(); return false;' style='border: none;'><img src='http://"+editorStatic+/*tli(*/"/weebly/images/footer_remove.png"/*)tli*/+"' style='position: relative; top: 3px; left: 6px; border: none;'/></a>";
          } else {
            $('footerCode').innerHTML += "<a href='#' onclick='alertProFeatures(\"Please sign-up for a pro account to remove the Weebly footer link\", \"main\"); return false;' style='border: none;'><img src='http://"+editorStatic+/*tli(*/"/weebly/images/footer_remove.png"/*)tli*/+"' style='position: relative; top: 3px; left: 6px; border: none;'/></a>";
          }
        }

	var imageGalleries = new Array(),
		_match;
	
	// v0.1 Image Galleries
	while (_match = resText.match(/<!WEEBLYGALLERY\-([^\-]*)\-([^\-]*)\-([^\-]*)\-(.*?)!>/)) {
		var updateHtml = Weebly.ImageGallery.update(_match[1], _match[2], _match[3], 0, 0, _match[4]);
		imageGalleries.push(_match[1]);
		resText = resText.safeReplace(/<!WEEBLYGALLERY\-.*?!>/, updateHtml);
	}
	
	// v0.2 Image Galleries
	while (_match = resText.match(/<!WEEBLYGALLERY2\-([^\-]*)\-([^\-]*)\-([^\-]*)\-([^\-]*)\-([^\-]*)\-(.*?)!>/)) {
		var updateHtml = Weebly.ImageGallery.update(_match[1], _match[2], _match[3], _match[4], _match[5], _match[6]);
		imageGalleries.push(_match[1]);
		resText = resText.safeReplace(/<!WEEBLYGALLERY2\-.*?!>/, updateHtml);
	}

    // Form Inputs
	while (_match = resText.match(/<!WEEBLYRADIO\-(.*?)\-(.*?)!>/)) {
		resText = resText.replace('<!WEEBLYRADIO-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawRadioOptions(_match[2]));
	}
    while (_match = resText.match(/<!WEEBLYSELECT\-(.*?)\-(.*?)!>/)) {
		resText = resText.replace('<!WEEBLYSELECT-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawSelectOptions(_match[2]));
	}
    while (_match = resText.match(/<!WEEBLYCHECKBOXES\-(.*?)\-(.*?)!>/)) {
		resText = resText.replace('<!WEEBLYCHECKBOXES-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawCheckboxes(_match[2]));
	}

	// Replace external account IDs
	var externalSites = 0;
        while ((_match = resText.match(/<!EXTERNAL:([^!]+)!>/)) && externalSites < 1000) {
          var remoteSite = _match[1];
          if (remoteAccounts[remoteSite]) {
            resText = resText.safeReplace(/<!EXTERNAL:[^!]+!>/, remoteAccounts[remoteSite].remoteId);
          }
	  externalSites++;
        }

	if (_match = resText.match(/<!EXTERNAL-SETUP:([^:]+):([^!]+)!>/)) {
	  Pages.go("externalSetup", _match[1], _match[2]);
	}

        scriptSrc = new Array();
        scriptId  = new Array();
        scriptType= new Array();
        var runScripts= 0;
	
	//Fanini - Replace new lines before text is output to page in design mode
	//resText = resText.replace(/\n/g,"");
	
        while (resText.indexOf('<weebly include_once') > -1 && scriptSrc.length < 30) {
          resText = resText.replace(/<weebly include_once(_noexport)? ([^>]*)>([\S\s]*?)<\/weebly>/im, '');
	  //alert("Pushing Id: '"+RegExp.$2+"', Src: '"+RegExp.$3+"'");
          scriptSrc.push(RegExp.$3);
          scriptId.push(RegExp.$2);
          runScripts = 1;
        }

        // Replace anything that starts with <weebly only_export
        resText = resText.replace(/<weebly only_export([^>]*)>([\S\s]*?)<\/weebly>/img, '');

        var secondList = $('secondlist');

        if (resText.match(/^%%EMPTY%%/)) {
           secondList.innerHTML = '<ul style="height: 380px; text-align: center; padding-top: 100px; font-size: 20px; color: #aaa;"> <span style="display: block; margin-bottom: 10px;">'+/*tl(*/'This page is empty. Drag Elements here.'/*)tl*/+'</span> <span style="font-size: 12px;"> ('+/*tl(*/'hint: Elements, like a Paragraph, are in the top bar'/*)tl*/+')</span></ul>';
           Element.setStyle('secondlist', {height:'380px'} );
           Element.show('emptyList');
        } else {
           secondList.innerHTML = resText.replace(/%%EMPTY%%/g, '');
           Element.setStyle('secondlist', {height:''} );
           Element.hide('emptyList');
        }

        if ( runScripts > 0 ) {
           try{ //webkit browsers fail here
               runIncludedScripts();
           }
           catch(e){}
        }

	makeIframesDraggable(secondList);

        finishFuncMiddle(resText);

    }

    function finishFuncMiddle(responseText) {
    
    	if (window.navFlyoutMenu) {
    		navFlyoutMenu.hideSubmenus();
    	}

	//console.log('finishFuncMiddle');
        if (responseText.match('<%%WEEBLY!disableSecondList!%%>')) {
	  // Editing a blog...
	  if (destroySecondList != 1) {
	    // First time on a blog page
	    elementsPage('blog');
	    currentBlog.editingBlog = 1;
	  }
	  destroySecondList = 1;

	  if ($('secondlist').innerHTML.match('%%HIGHLIGHT-TITLE%%')) {
	    new Effect.Highlight('blog-post-title',{duration: 2.0});
	  } else if ($('secondlist').innerHTML.match('%%HIGHLIGHT-DRAFTS%%')) {
	    new Effect.Highlight('blog_drafts',{duration: 2.0, startcolor:'#FFdd99'});
	  }

        } else if (siteType != 'myspace') {
	  if (destroySecondList == 1) {
	    // First time on a 'main' page
	    elementsPage('default');
	    currentBlog.editingBlog = 0;
	  }
	  destroySecondList = 0;
	}

	if (resetScrollTop) { resetScrollTop = null; $('scroll_container').scrollTop = 0; }
        var secondList = $('secondlist');

        if (responseText.match('<%%WEEBLY!disableSecondList!%%>')) {
          // Editing a blog...
	  if (currentBlog.skipToComments == 'comments' && $('comments')) {
	    var pos  = new Position.cumulativeOffset($('comments'));
	    $('scroll_container').scrollTop = (pos[1]-150);
	  }
	  if (currentBlog.showPostSettings) {
	    Effect.toggle('blogPostSettings', 'slide');
	    currentBlog.showPostSettings = 0;
	  }
	}
	
	updateActiveNavLink();
	disableFlyouts = false;
	
	if(!externalLibariesLoaded) {
		loadExternalLibraries();
	}

	setInterfaceEnabled();

        Weebly.TimingTest.end(Weebly.TimingTest.updateListTest);

        //Element.setStyle($('trashitem'), {height:$('container').clientHeight+30+'px'} );
        previousItems = secondList.getElementsByTagName('li').length;

	if (userEvents && (!userEvents.tab_themes) && userEvents.addElement && userEvents.addElement == 2) {
	  userEvents.addElement = 0;
	  showEvent('addElement', 1);
	}


    }
    
    
    
    
    
    
    function updateActiveNavLink(skipFlyoutUpdate) {

		if (typeof(currentThemeDefinition['menuActive']) != 'undefined' && currentThemeDefinition['menuActive'].strip()) {

			if (!skipFlyoutUpdate && window.navFlyoutMenu) {
				var itemHandle = $('pg'+currentPage);
				if (itemHandle) navFlyoutMenu.removeItem(itemHandle);
			}

			var thisLink = currentThemeDefinition['menuActive'];
			thisLink = thisLink.safeReplace(/%%MENUITEMACTIVETITLE%%/i, sitePages[currentPage]);
			thisLink = thisLink.replace(/%%MENUITEMACTIVELINK%%/i, "#");
			thisLink = thisLink.safeReplace(/<a/i, '<a onclick="'+"if (notBeenDragged()) { noJump = 1; goUpdateList('"+currentPage+"', 1); }"+'; return false;"');
			thisLink = thisLink.safeReplace(/<\/a>/i, '</a>' + Weebly.PageManager.subpagesHTML(currentPage));
			if($('pg'+currentPage) !== null) {
				$('pg'+currentPage).innerHTML = thisLink;
			}

			if (!skipFlyoutUpdate && window.navFlyoutMenu) {
				var itemHandle = $('pg'+currentPage);
				if (itemHandle) navFlyoutMenu.addItem(itemHandle);
			}

			if (previousPage != currentPage) {

				if (!skipFlyoutUpdate && window.navFlyoutMenu) {
					var itemHandle = $('pg'+previousPage);
					if (itemHandle) navFlyoutMenu.removeItem(itemHandle);
				}

				var thisLink = currentThemeDefinition['menuRegular'];
				thisLink = thisLink.safeReplace(/%%MENUITEMTITLE%%/i, sitePages[previousPage]);
				thisLink = thisLink.replace(/%%MENUITEMLINK%%/i, "#");
				thisLink = thisLink.safeReplace(/<a/i, '<a onclick="'+"if (notBeenDragged()) { noJump = 1; goUpdateList('"+previousPage+"', 1); }"+'; return false;"');
				thisLink = thisLink.safeReplace(/<\/a>/i, '</a>' + Weebly.PageManager.subpagesHTML(previousPage));
				if($('pg'+previousPage) !== null) {
					$('pg'+previousPage).innerHTML = thisLink;
				}

				if (!skipFlyoutUpdate && window.navFlyoutMenu) {
					var itemHandle = $('pg'+previousPage);
					if (itemHandle) navFlyoutMenu.addItem(itemHandle);
				}

			}
			
			if (window.navFlyoutMenu) {
				// update submenu 'current' class
				var oldLI = $('weebly-nav-' + previousPage);
				if (oldLI) {
					oldLI.removeClassName('weebly-nav-current');
				}
				var newLI = $('weebly-nav-' + currentPage);
				if (newLI) {
					newLI.addClassName('weebly-nav-current');
				}
			}

			if (!skipFlyoutUpdate && window.navFlyoutMenu) {
				refreshNavCondense();
			}
		}
    }
    
    
    
    
    
    

    function makeIframesDraggable(el) {

        // Steal all iframe's mouse events
        /*
 	// Stop stealing the mouse events, no longer needed with DIV overlays
 	
        var iFrames = el.getElementsByTagName('iframe');
        for (x=0; x < iFrames.length; x++) {

          // If the iFrame doesn't have an ID, give it one
          if (iFrames[x].id == "") {
            iFrames[x].id = Math.floor(Math.random()*100000001);
          }

          tryStealMouse(iFrames[x].id);
        }
	*/

    }

    function tryStealMouse(iFrame) {

	var iFrameEl = document.getElementById(iFrame);

	if (iFrameEl && iFrameEl.contentWindow && iFrameEl.contentWindow.document && iFrameEl.contentWindow.document.body && iFrameEl.contentWindow.document.body.innerHTML && iFrameEl.contentWindow.document.body.innerHTML != "") {
	  //alert(iFrameEl.contentWindow.document.body.innerHTML);
	  $.StealMouse.on(iFrameEl);
	} else {
	  setTimeout("tryStealMouse('"+iFrame+"');", 250);
	}

    }

    function runIncludedScripts() {

        thisScriptId = scriptId.pop();
        thisScriptSrc = scriptSrc.pop();

        thisScriptIdOnly = thisScriptId.replace(/scriptInclude/,'');

        //alert("runIncludedScripts: "+thisScriptId);

        var myFrame = document.getElementById(thisScriptId).contentWindow.document;
        var headScript = '';
        if (thisScriptSrc.indexOf('<%HEAD%>') > -1) {
          var splitElements = new Array();
          splitElements = thisScriptSrc.split('<%HEAD%>');
          thisScriptSrc = splitElements[1];
          headScript    = splitElements[0];
        }
	var bodyScript = '';
	if (thisScriptSrc.indexOf('<%BODY%>') > -1) {
	  var splitElements = new Array();
	  splitElements = thisScriptSrc.split('<%BODY%>');
	  thisScriptSrc = splitElements[1];
	  bodyScript    = splitElements[0];
	}

        var toWrite = '<html><head> ' + headScript + ' </head><body style="margin: 0px; padding: 0px; background-color: transparent;" onload="parent.resizeMe(); ' + bodyScript + '" onClick="parent.clickHandler(event, \''+thisScriptIdOnly+'\');"> '+ thisScriptSrc +' <script type="text/javascript"> parent.resizeMe("'+thisScriptId+'"); parent.setInterval("resizeMe('+"'"+thisScriptId+"'"+');", 250); </script></body></html>';

        //alert("Writing to '"+thisScriptId+"': '"+toWrite+"', "+scriptSrc.length+" scripts left.");

        /**
        if (navigator.appVersion.indexOf("Mozilla") > -1) {
          myFrame.write('<html><body style="margin: 0px; padding: 0px; background-color: transparent;"> '+ thisScriptSrc +'</body></html>');
          document.getElementById(thisScriptId).contentWindow.document.addEventListener("DOMContentLoaded", function(e){alert('asdf'); resizeMe(thisScriptId); return true;}, false);
        } else {
        **/
        //}
          myFrame.write(toWrite);

	  // Create transparent DIV on top of the iframe to allow smoother scrolling and clicking
	  if (!$(thisScriptId+"-coverbox")) {
	    var box = document.getElementById(thisScriptId);
	    var newDiv = document.createElement("DIV");
	    newDiv.id = thisScriptId+"-coverbox";
	    newDiv.style.position = 'absolute';
	    newDiv.style.overflow = 'hidden';
	    newDiv.style.background = 'url("http://'+editorStatic+'/weebly/images/clear.gif")';
	    newDiv.style.top = '17px';
	    newDiv.style.left = '0px';
	    newDiv.style.height = Element.getStyle(box, 'height');
	    newDiv.style.width = Element.getStyle(box, 'width');
	    //newDiv.style.background = 'yellow';
	    box.parentNode.appendChild(newDiv);
	  }

	  if (scriptId.length > 0) { runIncludedScripts(); }

    }

    function resizeMe(scriptId) {

	//console.log("resizeMe: "+scriptId);
        //var myFrame = document.getElementById(thisScriptId).contentWindow.document;
        //myFrame.close();

	if ((!tempDeleted.item || (tempDeleted.item && scriptId != tempDeleted.item.id)) && document.getElementById(scriptId)) {

          var box              = document.getElementById(scriptId);
          var editableDocument = box.contentWindow.document;

          var myHeight;
          if (navigator.appVersion.indexOf("MSIE") == -1) {  
	    myHeight = editableDocument.body.offsetHeight; 
	  } else { 
	    myHeight = editableDocument.body.scrollHeight; 
	  }
	  //myHeight = box.contentWindow.innerHeight;
	  if (myHeight != Element.getStyle(box, 'height').replace(/px/, '')) {
            Element.setStyle(box,{height: myHeight+'px'});
	    if ($(scriptId+"-coverbox")) {
              Element.setStyle($(scriptId+"-coverbox"),{height: myHeight+'px'});
	    }
	    $$("#secondlist .columnlist").each(resizeColumns);
	  }

          //var coverBox       = document.getElementById(scriptId.replace(/scriptInclude/, "scriptCover"));
          //Element.setStyle(coverBox,{height: myHeight+'px', top: ((400-myHeight)-400)+'px'});

          //alert("Resizing "+scriptId.replace(/scriptInclude/, "scriptCover")+" to "+myHeight+", top: "+((400-myHeight)-400));

	}

    }

    function resizeColumns(el) {

	// Make sure both columns are same height
        var oppEl = '';
        if (el.id.match(/lhs-list/)) {
          oppEl = el.id.replace(/lhs/, "rhs");

	  if ($(oppEl)) { 
	    el.style.height = 'auto';
	    $(oppEl).style.height = 'auto';
	    var elHeight = $(el).getHeight();
	    var oppElHeight = $(oppEl).getHeight();

	    if (elHeight < 100 && oppElHeight < 100) {
	      el.style.height = '100px';
	      $(oppEl).style.height = '100px';
	    } else if (oppElHeight >= elHeight) {
              el.style.height = oppElHeight + "px";
            } else {
              $(oppEl).style.height = elHeight + "px";
	    }
	  }
	}

    }

    function cacheIframe(el) {

        var iFrames = el.getElementsByTagName('iframe');
        for (x=0; x < iFrames.length; x++) {

	  if (iFrames[x] && iFrames[x].contentWindow && iFrames[x].contentWindow.document && iFrames[x].contentWindow.document.body) {
            iFramesCache[iFrames[x].id] = iFrames[x].contentWindow.document.body.innerHTML;
            iFramesHeight[iFrames[x].id] = Element.getStyle(iFrames[x], 'height');
          }

        }

    }

    function smokeOutIframe(el) {

        var iFrames = el.getElementsByTagName('iframe');
        for (x=0; x < iFrames.length; x++) {

	  if (iFramesCache[iFrames[x].id]) {
	    //iFrames[x].contentWindow.document.body.innerHTML = iFramesCache[iFrames[x].id];
	    //iFrames[x].style.height = iFramesHeight[iFrames[x].id];
	  }
          tryStealMouse(iFrames[x].id);
        }

    }

    dontRemoveElement = null;
    function removeElement(elementId) {

	if (dontRemoveElement) {
	  dontRemoveElement = null;
	  return;
	}

	if ($('inside_'+elementId)) {

	  $('inside_'+elementId).parentNode.removeChild($('inside_'+elementId));
	  new Ajax.Request(ajax, {parameters:'pos=deletepageelement&pei='+elementId+'&cookie='+document.cookie, onSuccess:handlerFuncDeletePageElement, onFailure:errFunc, onException: exceptionFunc, asynchronous: false});
	  updateList();

	}

    }

    function submitExternal(type) {

        if ($('lightbox_spinner').style.display == "inline") return;
	
        $(type+'ErrorDiv').style.display = 'none';
        $('lightbox_submitbtn').style.opacity = 0.5;
        $('lightbox_submitbtn').style.filter = 'alpha(opacity=50)';
        $('lightbox_spinner').style.display = 'inline';
        new Ajax.Request(ajax, {parameters: 'pos=externalaccount&type='+type+'&'+Form.serialize(type+'_form')+'&cookie='+document.cookie, 'onSuccess': function(t) { handlerSubmitExternal(t, type); }, 'onFailure':errFunc, bgRequest: true});
    }

    function handlerSubmitExternal(t, type) {

        if (t.responseText.match("SUCCESS")) {

          var externalID = t.responseText.replace("SUCCESS:", "");
	  remoteAccounts[type] = { remoteSite: type, remoteId: externalID };

	  dontRemoveElement = 1;
	  updateList();
	  Pages.go('main');

          $(type+'ErrorDiv').style.display = 'none';
          $('lightbox_submitbtn').style.opacity = 1;
          $('lightbox_submitbtn').style.filter = 'alpha(opacity=100)';
          $('lightbox_spinner').style.display = 'none';

        } else {

          if (!t.responseText.match("ERROR")) { t.responseText = /*tl(*/"ERROR: Temporary error. Please try again."/*)tl*/; }
          $(type+'ErrorDiv').innerHTML = t.responseText;
          $(type+'ErrorDiv').style.display = 'block';
          $(type+'ErrorDiv').setStyle({'color':'#FF0000'});
          $('lightbox_submitbtn').style.opacity = 1;
          $('lightbox_submitbtn').style.filter = 'alpha(opacity=100)';
          $('lightbox_spinner').style.display = 'none';
        }

    }

    function createMarker(element) {

	//removeMarker();

	if (element.parentNode.id == "elementlist") { return; }
  if (element.parentNode.id == "pages") { return; }
  if (element.parentNode.id == "inputoptions-list") { return; }
	if (element.parentNode.id.match(/\-gallery$/)) { return; }
	//console.log(element);
	var sortableMarker = '';
	if ($('sortableMarker')) {
	  sortableMarker = $('sortableMarker');
	} else {
          sortableMarker = document.createElement('DIV');
          sortableMarker.id = 'sortableMarker';
	  sortableMarker.className = 'sortableMarker';
          sortableMarker.style.position = 'absolute';
	  sortableMarker.style.zIndex = '100';
          $('scroll_container').insertBefore(sortableMarker, $('scroll_container').firstChild);
	}
	if (element.previousSibling) {
          sortableMarker.style.left = Position.cumulativeOffset(element.previousSibling)[0]+"px";
          sortableMarker.style.top = (Position.cumulativeOffset(element.previousSibling)[1]-125-(-Element.getStyle(element.previousSibling, 'height').replace(/px/, '')))+"px";
	} else {
	  sortableMarker.style.left = Position.cumulativeOffset(element.parentNode)[0]+"px";
          sortableMarker.style.top = (Position.cumulativeOffset(element.parentNode)[1]-131)+"px";
	}
	if (element.previousSibling) {
          sortableMarker.style.width = Element.getStyle(element.previousSibling, 'width');
          sortableMarker.style.height = Element.getStyle(element, 'height');
	} else if (element.nextSibling) {
          sortableMarker.style.width = Element.getStyle(element.nextSibling, 'width');
          sortableMarker.style.height = Element.getStyle(element, 'height');
	} else {
          sortableMarker.style.width = Element.getStyle(element, 'width');
          sortableMarker.style.height = Element.getStyle(element, 'height');
	}

    }

    function removeMarker() {

	if ($('sortableMarker')) {
          $('sortableMarker').parentNode.removeChild($('sortableMarker'));
        }

    }

    function controlDrop(element, dropon) {

      if(Element.hasClassName(element, 'controlledDrop')) {
        // David -- Controlled drop code
        // Allow elements to be dragged into containers by class names set on the element and container
        //console.log("onHover -- dropon.id: "+dropon.id+", element.className: "+element.className);
	//console.log(element);
        if ((dropon.id == "myspaceListRight" || (dropon.id.match(/^inside_/) && dropon.parentNode.id == "myspaceListRight")) && !Element.hasClassName(element, 'myspaceApplication')) {
          return false;
        } else if((dropon.id == "myspaceListRight" || (dropon.id.match(/^inside_/) && (dropon.parentNode.id == "myspaceAboutMeList" || dropon.parentNode.id == "myspaceLikeToMeetList"))) && !Element.hasClassName(element, 'myspaceAboutMe')) {
          return false;
        } else if(Element.hasClassName(element, 'myspaceApplication')) { // && (dropon.id.match(/^inside_/) && dropon.parentNode.id == "myspaceListRight")) {

	  if (nextSiblingCount(dropon) < 2) {
	    return false;
	  }
	}
      }

      return true;

    }

    function controlDrag(element) {

      //console.log(element);
      if(Element.hasClassName(element, 'controlledDrag')) {
        // David -- Controlled drag code
        // Allow elements to be draggable or not by the class name of the handle
        if (element && element.childNodes[1] && element.childNodes[1].firstChild && (Element.hasClassName(element.childNodes[1].firstChild, 'handlebar_right_nodrag') || Element.hasClassName(element.childNodes[1].firstChild, 'handlebar_left_nodrag'))) {
          return false;
        }
      }

      return true;

    }

    function nextSiblingCount(element, count) {

	element = $(element);
	if (!count) { count = 0; }

	if (element.nextSibling) {
	  count = nextSiblingCount(element.nextSibling, count+1);
	}

	return count;

    }

    function createIncludedScripts(scriptText) {

        //alert("createIncludedScripts start "+scriptText);

        var script = document.createElement('script');

        script.type = 'text/javascript';
        script.text = scriptText;
        script.id   = thisScriptId+"JS";

        if (scriptSrc.length > 0) { script.text += " runIncludedScripts();"; }
        else { script.text += " finishFuncMiddle();"; }

        documentWriteElement = thisScriptId;

        $(thisScriptId).appendChild(script);

    }

    function updateElements() {

	selectCategory(currentMenu);

    }

    function isFormElement(pe){
        var form = $(pe).down('.formlist');
        if(form){
            var columns = $(pe).down('.columnlist');
            if(!columns){
                return true;
            }
            else{
                return form.ancestors().size() < columns.ancestors().size();
            }
        }
        return false;
    }

    function deleteMe(myElement) {

	if( !containsElements(myElement ) || isFormElement(myElement) )
	{
        if(currentBox && $(currentBox)) hideEditBox(currentBox);
		tempDeleted.item = myElement;
		tempDeleted.myParent = myElement.parentNode;
		if (myElement.nextSibling) {
		  tempDeleted.type = 'nextSibling';
		  tempDeleted.refNode = myElement.nextSibling;
		} else {
		  tempDeleted.type = 'append';
		  tempDeleted.refNode = myElement.parentNode;
		}
	
		initContentDraggables();
		myElement.parentNode.removeChild(myElement);
	
		confirmDelete();
	}
	else
	{
		showWarning( /*tl(*/'Please remove all elements inside this element before deleting it.'/*)tl*/, myElement );
	}

    }
    
    //returns true if another element is contained inside this element
    function containsElements(element)
    {
        var children = $(element).childElements();
        for( var i = 0; i < children.length; i++ )
        {
            if( children[i].match( 'li.inside' ) || containsElements(children[i]) )
            {
                return true;
            }
        }
        return false;
    }

    function updateTrash() {

	//dontUpdateList = 1;
	trashItem = $('trashlist').innerHTML;
    	$('trashlist').innerHTML = '<li id="trashitem" class="outside" style="height: 75px;"></li>';
	confirmDelete();

    }

    function confirmDelete() {

	var height = getScrollTop() + 205;
        //$('typeOfEl1').innerHTML = 'element';
        //$('typeOfEl2').innerHTML = 'element';
        //$('typeOfEl3').innerHTML = 'element';
	Element.setStyle('deleteConfirmation', {top:height+'px'} );
	Element.show('deleteConfirmation');	

	//var testme = confirm("Do you want to delete this item?");
        //if (!testme) { $('secondlist').innerHTML = oldSecondList; Behaviour.apply(); }

    }

    function unDeleteElement() {

	if ( trashItem.match(/notBeenDragged/) ) {
	  //$('pages').innerHTML = oldPages;
	  //updatePages();
	} else {
	  if( tempDeleted.type == "append") {
	    tempDeleted.refNode.appendChild(tempDeleted.item);
	  } else {
	    tempDeleted.refNode.parentNode.insertBefore(tempDeleted.item, tempDeleted.refNode);
	  }
	}
	Behaviour.apply();

    }

    function goDeleteElement() {

	deleting = 1;
	if (tempDeleted.myParent.parentNode.parentNode.id.match(/-blog/)) {
	  // Delete separately
	  new Ajax.Request(ajax, {parameters:'pos=deleteblogelement&pei='+tempDeleted.item.id.replace('inside_', '')+'&cookie='+document.cookie, onSuccess:handlerFuncDeleteElement, onFailure:errFunc, onException: exceptionFunc});
	} else {
          if ( trashItem.match(/notBeenDragged/) ) {
            //updatePages(); // should this be uncommented??? what is its use??? was uncommented before PageManager refactoring ~ashaw
          } else if(isFormElement(tempDeleted.item)) {
            new Ajax.Request(ajax, {parameters:'pos=deleteformpageelement&pei='+tempDeleted.item.id.replace('inside_', '')+'&cookie='+document.cookie, onSuccess:handlerFuncDeletePageElement, onFailure:errFunc, onException: exceptionFunc});
          } else {
            //updateList();
            new Ajax.Request(ajax, {parameters:'pos=deletepageelement&pei='+tempDeleted.item.id.replace('inside_', '')+'&cookie='+document.cookie, onSuccess:handlerFuncDeletePageElement, onFailure:errFunc, onException: exceptionFunc});
          }
	}

	Element.hide('deleteConfirmation');

    }
    
    function handlerFuncDeletePageElement(t) {
        if(t.responseText.indexOf('ERROR') > -1)
        {
            unDeleteElement()
            showError(t.responseText, tempDeleted);
        }
        else
        {
			updateList();
        }
    }

    function handlerFuncDeleteElement(t) {
	updateList();
    }

    var skipDisableInterface = 0;
    function updateList(pageid, newPost)
    {
      
        if (!firstLoadMiddle) {
          Weebly.TimingTest.doUpdateListTest = Math.floor(Math.random()*10) == 9;
          if (Weebly.TimingTest.doUpdateListTest) {
            Weebly.TimingTest.updateListTest = deleting ? 'delete_element' : 'add_element';
            Weebly.TimingTest.start(Weebly.TimingTest.updateListTest);
          }
        }
	//console.log('updateList. pageID: '+pageid+' newpost: '+newPost+' postId: '+currentBlog.postId);
        //new Ajax.Request(ajax, {parameters:'pos=left&cookie='+document.cookie, onSuccess:handlerFuncLeft, onFailure:errFunc});
        
	//Disable dragging of elements while page loads
	if (skipDisableInterface) {
	  skipDisableInterface = 0;
	} else {
	  setInterfaceDisabled();
	}

	if (currentBlog.postId > 0) {
	  if ($('blog-post-categories')) { currentBlog.categories = $('blog-post-categories').value; }
	  if ($('blog-post-title')) { currentBlog.title = $('blog-post-title').value; }
	}
	if (typeof(newPost) == "undefined") { newPost = 0; }

	if (currentBox) hideEditBox(currentBox);
	Weebly.Elements.unselectElement();

	if (deleting) { deleting = 0; }
	if (dontUpdateList) { dontUpdateList = 0; }
    else {
        if (pageid > 0) {
            previousPage = currentPage;
            currentPage = pageid;
            if (firstLoadMiddle) {
                Weebly.Cache.get('', 'initialMiddle', function(response){
                    var t = {};
                    t.responseText = response.replace(/%%QUOTE%%/g, '"').replace(/%%NEWLINE%%/g, '\n');
                    t.getHeader = function(h){
                        if (h == "Weebly-Page-Link") {
                            return "index";
                        }
                        return false;
                    };
                    handlerFuncMiddle(t);
                    $('pleaseWait').style.display = 'none';
                });
                firstLoadMiddle = false;
            }
            else {
                if (Weebly.TimingTest.doUpdateListTest) { Weebly.TimingTest.start(Weebly.TimingTest.updateListTest+"_ajax"); }
                new Ajax.Request(ajax, {
                    parameters: 'pos=middle&pageid=' + pageid + '&blogPost=' + currentBlog.postId + '&newPost=' + newPost + '&categories=' + currentBlog.categories + '&title=' + currentBlog.title + '&cookie=' + document.cookie,
                    onSuccess: handlerFuncMiddle,
                    onFailure: errFunc,
                    onException: exceptionFunc
                });
            }
        }
        else {
            var listItems = $$('#secondlist .inside', '#secondlist .outside_top');
            var elements = [];
            var newElements = 0;
            var i = 0;
            listItems.each(function(el){
                var form = el.down('form');
                if (form && form.idfield) {
                    elements[i] = {};
                    elements[i].new_element = form.idfield.value.match('def:') ? true : false;
                    if (!elements[i].new_element) {
                        var temp = form.idfield.value.evalJSON();
                        elements[i].page_element_id = temp.id;
                        elements[i].element_id = temp.eid;
                    }
                    else {
                        var temp = form.idfield.value.split('|');
                        elements[i].element_id = temp[0].replace(/[^\d]/g, '');
                        if (temp[1]) {
                            elements[i].properties = temp[1].evalJSON();
                        }
                        newElements++;
                    }
                    if (el.parentNode.parentNode.parentNode.className.match("column")) {
                        elements[i].parent = el.parentNode.parentNode.parentNode.id;
                    }
                    else 
                        if (el.up('.formlist')) {
                            elements[i].parent = el.up('.formlist').id;
                        }
                    
                    // If an element's never been added before, hook here to throw the event later
                    if (userEvents && !userEvents.addElement && elements[i].new_element) {
                        userEvents.addElement = 2;
                        fireTrackingEvent("AddElement", elements[i].element_id);
                    }
                    i++;
                }
            });
            pageid = typeof(pageid) != "string" ? currentPage : pageid;
            var params = {
                pos: 'middle',
                pageid: pageid,
                elements: elements.toJSON(),
                blogPost: currentBlog.postId,
                newPost: newPost,
                categories: currentBlog.categories,
                title: currentBlog.title,
                cookie: document.cookie 
            }
            //Handle WebKit multiple new elements bug by refreshing content from db
            if(newElements > 1){
                logExcessElements();
                setElementsPageType();
                updateList(currentPage);
            }
            else{
                if (Weebly.TimingTest.doUpdateListTest) { Weebly.TimingTest.start(Weebly.TimingTest.updateListTest+"_ajax"); }
                new Ajax.Request(ajax, {parameters:params, onSuccess:handlerFuncMiddle, onFailure:errFunc, onException: exceptionFunc, bgRequest: true});
            }        
        }
	}
        
	//console.log("endUpdateList");
        
    }
    
    function logExcessElements(){
        var history = Pages.history || [];
        var params = {
            pos: 'logexcesselements',
            pageid: currentPage,
            secondlist: $('secondlist').innerHTML,
            elementlist: $('elementlist').innerHTML,
            history: history.join(', '),
            cookie: document.cookie 
        }
        new Ajax.Request(ajax, {parameters:params, bgRequest: true});
    }
   
    function setDrag() {
	recentDrag = 1;
    }

    function notBeenDragged() {
      if (navigator.appVersion.indexOf("MSIE") == -1) {
	if (recentDrag == 1) {
	   recentDrag = 0;
	   return false;
	} else {
	   return true;
	}
      } else { return true; }
    }

    function saveContent(id,saveContent,alignment, dontRefresh) {

        //console.log("Saving content: "+saveContent);

        var encodedHtml;         
	encodedHtml = encodeURIComponent(saveContent);         
	encodedHtml = encodedHtml.replace(/\//g,"%2F");
        encodedHtml = encodedHtml.replace(/\?/g,"%3F");
        encodedHtml = encodedHtml.replace(/=/g,"%3D");
        encodedHtml = encodedHtml.replace(/&/g,"%26");
        encodedHtml = encodedHtml.replace(/@/g,"%40"); 

	// Replace linefeed (%0A) with space (%20)
	encodedHtml = encodedHtml.replace(/%0A/g, "%20");

        new Ajax.Request(ajax, {parameters:'pos=content&reqid='+id+'&content='+encodedHtml+'&align='+alignment+'&cookie='+document.cookie, onSuccess:function(t) { handlerFuncSaveContent(t, dontRefresh); }, onFailure:errFunc, bgRequest: dontRefresh});

    } 

    function handlerDragEndSplitpane(splitPane, event, cfpids, elId) {

	// Grab serialized parameters into array 'widths'
	var width = splitPane.serialize();
	var widthArray = width.split(/&/);
	var widths = new Array();

	for (var x=0; x < widthArray.length; x++) {
	  var v = widthArray[x].split(/=/);
	  widths[v[0]] = v[1];
	}

	var cfps = cfpids.split(/-/);
	var params = new Array();

	for (var x=0; x < cfps.length; x++) {
	  params.push(cfps[x]+'-'+widths['div'+(x+1)+'_width']);
	}

	var pString = params.join(":");

	//console.log(elId);
	//console.log(pString);
	new Ajax.Request(ajax, {parameters:'pos=savecolumn&elementid='+elId+'&width='+pString+'&cookie='+document.cookie, onSuccess:handlerFuncDragEndSplitpane, onFailure:errFunc, bgRequest: true});

    }

    function handlerFuncDragEndSplitpane(t) {

    }

    function flashMouseUpHandler(e, obj) {

    	if ( document.createEvent ) {
	  var evObj = document.createEvent('MouseEvents');
 	  evObj.initMouseEvent( 'mouseup', true, false, window, e.detail, e.screenX,e.screenY, e.clientX, e.clientY, e.ctrlKey, e.altKey,e.shiftKey, e.metaKey, e.button, null );
    	  obj.parentNode.dispatchEvent(evObj);
    	}
	// We don't need to dispatch these events for IE (since the bug doesn't exist on IE!)

    }

    function twoColumnDepth(el){
        var depth = 0;
        var parent = el.up('.weebly-splitpane-2');
        while(parent){
            depth++;
            parent = parent.up('.weebly-splitpane-2');
        }
        return depth;
    }

    function pushTwoColumnsDraggables(){
        if(Weebly.Elements.highlightedElement){
            var els = $$('#secondlist .formlist .columnlist', '#secondlist .formlist');
        }
        else{
            var els = Sizzle.matches( ':not(#secondlist .formlist .columnlist)', $$('#secondlist .columnlist') )
        }
        var depths = els.collect(function(el){
            return {'element': el, 'depth': el.ancestors().size()};
        });
        depths.sort(function(a, b){return b.depth - a.depth});
        depths.each(function(obj){
           contentDraggables.push(obj.element);
        });
    }


	//
	// initDraggables SUCKS because Draggables.addObserver isn't working (element property is disregarded)
	// causing the observer to be fired every time ANY draggable finishes
	// ~ashaw
	//
	// UPDATE: reorderDropped() still gets called globally on drops, but now it only gets called ONCE
	//
    function initDraggables() {

	if (!interfaceActive) { return false; }

	Position.includeScrollOffsets = true;
	//console.log("initDraggables!");

	for(var x=0; x < contentDraggables.length; x++) {

	  //console.log(contentDraggables[x]);
	  if (contentDraggables[x].id == "elementlist") {
	  
	    Sortable.create("elementlist", {
	    	dropOnEmpty:true,
	    	constraint:false,
	    	scroll:$('scroll_container'),
	    	scrollSensitivity:100,
	    	scrollSpeed:5,
	    	onUpdate:updateElements
	    });
	    
        Draggables.addObserver({element:$('elementlist'), onStart:beginAddElement});
	  } else if(contentDraggables[x].id != "myspaceListRight" && contentDraggables[x].id != "myspaceListLeft") {
	    //console.log("not elementlist");
	    if ((contentDraggables[x].id == "secondlist" && destroySecondList != 1) || contentDraggables[x].id != "secondlist") {
	    
	      //console.log("Sortable.create("+contentDraggables[x]+", ");
	      //Sortable.create(contentDraggables[x], {hoverclass: 'hoverclass', dropOnEmpty:true,handle:'handle',scroll:'scroll_container',scrollSensitivity:50,scrollSpeed:5,containment:contentDraggables,constraint:false,onUpdate:singleUpdateList });
	      
	      Sortable.create(contentDraggables[x], {
	      	hoverclass: 'hoverclass',
	      	dropOnEmpty:true,
	      	scroll:$('scroll_container'),
	      	scrollSensitivity:50,
	      	scrollSpeed:5,
	      	containment:contentDraggables,
	      	constraint:false
	      });

	      // Manually provide DOM events for flash objects,
	      // since Mac FF flash client seems to eat them
	      var obj = $$("#"+contentDraggables[x].id+" object");
	      if (obj && obj[0] && obj[0].tagName) {
		Event.stopObserving(obj[0], 'mouseup');
		Event.observe(obj[0], 'mouseup', function(e) { flashMouseUpHandler(e, obj[0]); });
	      }
	    }
	    if (contentDraggables[x].id.match(/lhs-list/) && userEvents.addElement && userEvents.addElement != 2) {
		//console.log("twoColumn "+userEvents.addElement);
	      showEvent("twoColumn", 0, contentDraggables[x].id);
	    }
	  }

	}
			
		// for addObserver(), the element parameter doesn't seem to do much
		// onEnd gets called every time ANYTHING is dropped
		// however, 'element' still serves as a hash key for adding/removing observers
		// ..so remove any old observers before adding new
		// ..this ensures reorderDropped() is only triggered once
		var secondlist = $('secondlist');
		Draggables.removeObserver(secondlist);
	    Draggables.addObserver({element:secondlist, onEnd:reorderDropped});
    }

    function initContentDraggables() {

        for(var x=0; x < contentDraggables.length; x++) {
	  Sortable.destroy($(contentDraggables[x]));
	}
    Draggables.observers = [];
	contentDraggables = [];

    }

    function beginAddElement(eventName, draggable, event){
    	disableFlyouts = true;
        if(draggable.element.hasClassName('outside_top')){
            addElementStart = true;
        }
    }

    var lastDroppedTime = 0;
    function reorderDropped(eventName,draggable,event){
    	disableFlyouts = false;
    	
    	if (draggable && draggable.element && (draggable.element.hasClassName('highlightbox') || draggable.element.hasClassName('highlightbox-active'))) {
    		//
    		// Since the Draggables.addObserver call in initDraggables is faulty,
    		// and the reorderDropped handlers is called every time ANY draggable
    		// finishes, we want to STOP this behavior for draggables from the Manage Pages
    		// interface. Check for className (hacky and temporary) ~ashaw
    		//
    		return;
    	}
    	
        var dragged = draggable.element;
        var currentTime = new Date().getTime();
        if(dragged.hasClassName('outside_top')){
            singleUpdateList();
            updateElements();
        }
        else if(currentBlog.editingBlog === 1 || dragged.select('.element-box').size() > 1){
            updateList();
        }
        else if(dragged.hasClassName('inside') && (currentTime > lastDroppedTime + 500)){
            var parent = '';
			if (dragged.parentNode.parentNode.parentNode.className.match("column")) {
			  parent = dragged.parentNode.parentNode.parentNode.id;
			}
            else if (dragged.up('.formlist')) {
			  parent = dragged.up('.formlist').id;
			}
            var currentOrder = 1;
            var listItems = $$('#secondlist li.inside');
            for(var i=0; i<listItems.size(); i++){
                if(listItems[i].id===dragged.id){
                    break;
                }
                currentOrder++;
            }
            var peid = dragged.id.match(/[\d]+/);
            new Ajax.Request(ajax, {parameters:'pos=reorderelement&page_element_id='+peid+'&page_id='+currentPage+'&new_order='+currentOrder+'&parent='+parent+'&cookie='+document.cookie, onFailure:errFunc,
                onSuccess:function(){
                    $$("#secondlist .columnlist").each(
                        function(el){
                            resizeColumns(el);
                            updateTwoColumnDividerHeight(el.id.match(/[\d]+/));
                        }
                    );
                }
            });
            lastDroppedTime = currentTime;
        }
    }

    function updateTwoColumnDividerHeight(el){
        el = el+'';
        var height = $(el).down('#'+$(el).id+'-lhs-list').getHeight();
        $(el).down('.splitpane-divider').setStyle({'height':height+'px'});
    }

    var addElementStart = false;
    function singleUpdateList(container) {

	var currentTime = new Date().getTime();
	if (addElementStart) {

	  addElementStart = false;
      if(allowProElementsTrial && !Weebly.Restrictions.hasNewElementAccess()){
          if(!userEvents['pro_element_upsell']){
              openBillingPage('Please sign-up for a pro account to add that element.');
              removeMarker();
              userEvents['pro_element_upsell'] = 1;
              //new Ajax.Request(ajax, {parameters:'pos=doevent&event=pro_element_upsell&cookie='+document.cookie});
          }
          Pages.go('upgradeWarning');
      }
      else if(Weebly.Form.isNewFormElement() && Weebly.Form.isOverInputLimit()){
          //$('form-element-warning-message').update('You\'ve now reached the maximum number of fields for your form.  If you want to add more fields, please <a style="color:#0054CD; text-decoration:none; font-weight:bold;" href="#" onclick="alertProFeatures(\'Upgrade to publish Pro elements\'); return false;">upgrade</a> to the Weebly Pro service.');
          if(Weebly.Form.isOverInputLimit()){
              $('form-element-warning-message').update('A standard Weebly account allows for 5 fields in a form.  If you want to add this field, please <a style="color:#0054CD; text-decoration:none; font-weight:bold;" href="#" onclick="alertProFeatures(\'Upgrade to publish Pro elements\'); return false;">upgrade</a> to the Weebly Pro service, which allows unlimited fields.');
              $$('#secondlist .outside_top')[0].remove();
          }
          Pages.go('formElementWarning');
      }
      else{
          updateList();
      }

	} else {

	  //console.log("Skipped updateList() -- marked as duplicate");

	}

   }

   var lastWidth = 0;
   function hoverHandler(e) {

	if (Draggables.activeDraggable) return;

      	var selected = $(Event.findElement(e)).up('.inside') || document;
	clearElementBox(selected);
	if (selected != document) {
	
	  var el1 = selected.down('.handleContainer');
	  var el2 = selected.down('.element-box');
	  if (el1 && el2) {
	    var newWidth = el2.getWidth()-2;
	    if (Prototype.Browser.IE6) {
	      newWidth = newWidth + 2;
	      if (lastWidth == newWidth - 2) {
		newWidth = lastWidth;
	      }
	      lastWidth = newWidth;
	    }
	    el1.setStyle({ width: newWidth+"px"});
	  }

	  //var el2 = selected.up('.columnlist');
	  //if (el2) { selected.up('.columnlist').setStyle({ position: "static"}); }

	  //selected.setStyle({ position: "auto"});
	  //selected.down('.handleContainer').clonePosition(selected.down('.element-box'), {setWidth: false, setHeight: false, offsetTop: -27});
	  if(!Weebly.Elements.highlightedElement || !selected.down('.element') || Weebly.Elements.highlightedElement != selected.down('.element').id){
          selected.addClassName('inside-hover');
      }
      var up = selected.up('.column .element-box-contents');
      if(up){
          up.setStyle({'overflow':'visible'});
      }
	}

    }

    function clearElementBox(selected) {

	var currentEl = '';
	if (Weebly.Elements.currentElement && Weebly.Elements.currentElement.up) {
	  currentEl = Weebly.Elements.currentElement.up('.inside');
	}
    else if(currentBox && $(''+currentBox).up('.inside')){
        currentEl = $(''+currentBox).up('.inside');
    }

	$$('#secondlist .inside').each(function(el) {
	  if (el != selected && el != currentEl && el.hasClassName('inside-hover')) {
	    el.removeClassName('inside-hover');
        var up = el.up('.column .element-box-contents');
        if(up){
            up.setStyle({'overflow':'hidden'});
        }
        var options_id = 'dropdown'+el.id.match(/[\d]+/);
        if($(options_id)){
            $(options_id).remove();
        }
	  }
	});	

    }

// Single click handlers
Event.observe( document, 'mouseup',clickHandler );
    function clickHandler(e, ucfid) {

	// Hide all tips
	hideAllTips();

	var targ;
	if(ucfid) { targ = $(ucfid+""); }
	else {
	  if (!e) var e = window.event;
	  if (e.target) targ = e.target;
	  else if (e.srcElement) targ = e.srcElement;
	  if (targ.nodeType == 3) // defeat Safari bug
		targ = targ.parentNode;
	}
	if (targ && typeof targ.id == 'string') {
		if(targ.id.match(/coverbox/)){
		    targ = targ.up('.element');
		}
		if(targ.id.match(/form\-cover/)){
		    return;
		}
    }
	
	// Grab all editable iframe elements, but allow scrolling (scroll_container)
	var iframeEl = isAParentMatch(/[0-9]{8}/, targ);

	if (typeof(targ.id) == 'string' && iframeEl && iframeEl.ondblclick && (iframeEl.tagName == 'H2' || iframeEl.tagName == 'P' || iframeEl.tagName == 'DIV')) {
	  // True of editable text
	  iframeEl.ondblclick();
	  Weebly.Elements.unselectElement();
	  Event.stop( e );
	} else if(iframeEl && iframeEl.id.match(/[0-9]Edit/)) {
	  // Do nothing -- is a first click on an iframe editable document
	} else if(((typeof(targ.id) == 'string' && targ.id != 'scroll_container') || typeof(targ.id) != 'string') && !isAParent('editMenu', targ) && !isAParent('createLink', targ) && !isAParent('colorChooserDiv', targ) && !isAParent('new-color-chooser', targ) && !isAParentMatch(/menuBar(Advanced)?Div/, targ) && !isAParent('notifications', targ) && !isAParentByClass('customhtml_textarea', targ) && !isAParent('flashContainer', targ) && isAParent(document, targ)) {
	  // True if not editable text, not linker, not color chooser, not text menu, not "scroll_container", not the flash upload container, and the click did not happen within an iFrame
	  if(currentBox && $(currentBox)) hideEditBox(currentBox);
	  if(headerSelected) unselectHeader();
	  var elementNode = isAParentByClass('element', targ);
	  if (elementNode) {
	    Weebly.Elements.selectElement(elementNode);
	  } else if(isAParentByClass('weebly_header', targ)) {
	    Weebly.Elements.unselectElement();
	    selectHeader();
	  } else if (isAParent('weeblyLightbox', targ) && !isAParent('weeblyLightboxInside', targ)) {
	    // Lightbox outside click
	    Pages.go("main");
	  } else {
	    Weebly.Elements.unselectElement();
	  }
	} else if (typeof(targ.id) == 'string' && targ.id == 'colorSwatch') {
	  myColorPicker.toggle(e);
	} else if (!isAParent(document, targ)) {
	  //console.log('iFrame click!');
	} else {
	  // This is a click on "scroll_container", the linker, the color chooser, or the text menu
	}
	//console.log("click! "+targ+", "+iframeEl.tagName);

    }

    function handlerFuncSaveContent(t, dontRefresh) {

        if (t.responseText.indexOf('the following problems') > 0 && !dontRefresh) {
           showError(t.responseText);
	   updateList(currentPage);
        }

    }

    function hideFooter() {

        $("footerCode").style.display = "none"; 
        footerCodeShown = 1;

        new Ajax.Request(ajax, {parameters: 'pos=hidefooter&siteid='+currentSite+'&cookie='+document.cookie, 'onFailure':errFunc, bgRequest: true});

    }

    function saveProperties(target, id, goMain) {

	if( uploadInProgress > 0) {
		showError(/*tl(*/"Your image file is still uploading. Please wait until the upload is finished."/*)tl*/);
	} else {

	Element.setStyle('errorProperties', {display:'none'});

        var propertiesBasic = $('propertiesBasic').getElementsByTagName('td');
        var lenBasic = propertiesBasic.length;
        var paramsBasic = '';
        for(var x=0; x < lenBasic; x++) {
	    var thisForm = propertiesBasic[x].getElementsByTagName('form');
            for(var z=0; z < thisForm.length; z++) {
            	var inputname = thisForm[z].childNodes[0].name;
            	var inputval;
            	if (inputname == 'enable_nav_more') {
            		inputval = thisForm[z].childNodes[0].checked ? 1 : 0;
            	}else{
	                inputval = thisForm[z].childNodes[0].value;
	            }
	            
	            paramsBasic += inputname + String.fromCharCode(3) + inputval + String.fromCharCode(2);

		//Special property actions
		if (inputname == "enable_nav_more") {
			DISABLE_NAV_MORE = !inputval;
			// page will be rewritten anyway, so just chill..
		}
		if (inputname == "site_title") {
          if($('weebly_site_title')){
		    $('weebly_site_title').innerHTML = thisForm[z].childNodes[0].value;
          }
		}
		if (inputname == "copyright_text") {
            if($('footerCode')){
                $('footerCode').innerHTML = thisForm[z].childNodes[0].value;
            }
		}
		if (inputname == "site_password" && Weebly.Restrictions.hasAccess("editable_site_password")) {

				newSitePassword = thisForm[z].childNodes[0].value;
				if (newSitePassword == "") {
					for (var pid in Weebly.PageManager.pages) {
						Weebly.PageManager.pages[pid].pwprotected = 0;
					}
				}
				else if (oldSitePassword == "" && newSitePassword != "") {
					for (var pid in Weebly.PageManager.pages) {
						Weebly.PageManager.pages[pid].pwprotected = 1;
					}
				}
		  
		}
            }
        }

        var propertiesAdvanced = $('propertiesAdvanced').getElementsByTagName('td');
        var lenAdvanced = propertiesAdvanced.length;
        var paramsAdvanced = '';
        for(var x=0; x < lenAdvanced; x++) {
	    var thisForm = propertiesAdvanced[x].getElementsByTagName('form');
	    for(var z=0; z < thisForm.length; z++) {
		paramsAdvanced += thisForm[z].childNodes[0].name + String.fromCharCode(3) + thisForm[z].childNodes[0].value + String.fromCharCode(2);
	    }
        }

        new Ajax.Request(ajax, {parameters:'pos='+target+'&reqid='+id+'&keys='+encodeURIComponent(paramsBasic)+encodeURIComponent(paramsAdvanced)+'&cookie='+document.cookie, onSuccess:function(t){ handlerFuncSaveProperties(t, goMain) }, onFailure:errFunc, bgRequest: true});

	} 
    }

    function handlerFuncSaveProperties(t, noGoMain) {

	if (t.responseText.indexOf('RELOAD') >= 0) { window.location.reload(); }

	if (t.responseText.indexOf('the following problems') > 0) {
	   $('errorProperties').innerHTML = t.responseText;
	   Effect.Appear('errorProperties', { duration: 0.5 });
	} else {
	   writeTheme(currentTheme);
	   updateList(currentPage);
	   //updatePages();
	   if (!noGoMain) { Pages.go('main'); }
	}

    }

    function displayProperties(ucfid) {

	currentElement = ucfid;

	new Ajax.Request(ajax, {parameters:'pos=properties&reqid='+ucfid+'&cookie='+document.cookie, onSuccess:handlerDisplayProperties, onFailure:errFunc, onException:exceptionFunc});

    }

    function handlerDisplayProperties(t) {

        var responseElements = new Array();
	var numOfProperties  = 0;
	responseElements     = t.responseText.split('%%NEXT%%');

        var propertiesTitle  = $('propertiesTitle');
	propertiesTitle.innerHTML = responseElements[0];

        if (responseElements[2]) { 
           var propertiesBasic  = $('propertiesBasic');
           propertiesBasic.innerHTML = responseElements[2];
           Element.setStyle('propertiesBasicHeader', {display:'block'});
           Element.setStyle('propertiesBasic', {display:'block'});
	   numOfProperties++;
        } else {
           var propertiesBasic  = $('propertiesBasic');
           propertiesBasic.innerHTML = '';
           Element.setStyle('propertiesBasicHeader', {display:'none'});
           Element.setStyle('propertiesBasic', {display:'none'});
        }

        if (responseElements[3]) {
           var propertiesAdvanced  = $('propertiesAdvanced');
           propertiesAdvanced.innerHTML = contentDecode(responseElements[3]);
           Element.setStyle('propertiesAdvancedHeader', {display:'block'});
           Element.setStyle('propertiesAdvanced', {display:'none'});
	   collapseAdvanced();
           numOfProperties++;
	}else{
	Element.setStyle('propertiesAdvancedHeader', {display:'none'});
        Element.setStyle('propertiesAdvanced', {display:'none'});
	}
	// load element_js from DB if present
	// function must be defined at top of this page
	// remove newlines from data
	responseElements[4]=responseElements[4].replace(/\n/gi,"");
	if (responseElements[4]) {
           eval("validateFunction = " + responseElements[4]);
	   //if element_js is present, use validateFunction when saving
	   $('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"validateFunction('properties', '"+currentElement+"'); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>cancel</a></div></div>";
}
else
{
	   //if element_js is not present, use default saveProperties function
	   $('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"saveProperties('properties', '"+currentElement+"'); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>cancel</a></div></div>";
}
	// load element_onchange_js from DB if present
	responseElements[5]=responseElements[5].replace(/\n/gi,"");
        if (responseElements[5]) {
	   //alert(responseElement[5]);
	   
	   //set validateChangeFunction
           eval("validateChangeFunction = " + responseElements[5]);
	}
	if (numOfProperties == 0) {
           var propertiesBasic  = $('propertiesBasic');
           propertiesBasic.innerHTML = /*tl(*/"This element does not have any properties"/*)tl*/;
           Element.setStyle('propertiesBasicHeader', {display:'block'});
           Element.setStyle('propertiesBasic', {display:'block'});
	}

	showProperties(45);

    }

    function AddPage() {

	$('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"createPage(); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>"+/*tl(*/"cancel"/*)tl*/+"</a></div></div>";

	$('propertiesTitle').innerHTML = "Add Page";
        $('propertiesBasic').innerHTML = "<p style='width: 75%;'>"+/*tl(*/"When you add a page, it will show up in the pages tab, along with the rest of the pages on your site."/*)tl*/+"</p><br/><b>"+/*tl(*/"Title your page"/*)tl*/+"</b><br/><input type='text' style='width: 200px; border: 1px solid black;' id='pageTitle'/>";
        Element.setStyle('propertiesBasicHeader', {display:'none'});
        Element.setStyle('propertiesBasic', {display:'block'});
        Element.setStyle('propertiesAdvancedHeader', {display:'none'});
        Element.setStyle('propertiesAdvanced', {display:'none'});
        Element.setStyle('textEditBox', {display:'none'});

	showProperties(45);

    }

    function createPage() {
	var pageTitle = $('pageTitle').value;

	new Ajax.Request(ajax, {parameters:'pos=addpage&title='+encodeURIComponent(pageTitle)+'&cookie='+document.cookie, onSuccess: function(t) { handlerAddPage(t, pageTitle); }, onFailure:errFunc, onException:exceptionFunc});

    }

    function handlerAddPage(t, newTitle) {

	if (t.responseText) {
	   currentPage = t.responseText;

	   sitePages[t.responseText] = newTitle;
	   writeTheme(currentTheme);
	   updateList(currentPage);

	} else {
	   showError(/*tl(*/"There was an error adding your page. Please try again."/*)tl*/);
	}

	Pages.go('main');

    }

    function goUpdateList(pageID, run) {
	//console.log("goUpdateList. pageID: "+pageID+"; run: "+run);
	unselectHeader(1);
	if (run == 0) { setTimeout("goUpdateList('"+pageID+"', 1)", 500); }
	else if (pageDblClick == 0) {
	  Pages.go('updateList',pageID);
	} else {
	  pageDblClick = 0;
	}

    }

    function goBlogPage(href) {
	resetScrollTop = 1;
	href = href.startsWith( '/' ) ? href : '/' + href;
	var loc = currentPage.replace(/([0-9])+\/.+/, "$1")+href;
	loc     = loc.replace(/\/[0-9]+$/, "");
	Pages.go('updateList', loc);

    }

    function goNewBlogPost() {
	currentBlog.saving = 0;
	Pages.go('goBlogPost', 1, 1);
    }

    function goBlogPost(postid, comments) {
	currentBlog.skipToComments = comments;
	Pages.go('goBlogPost', postid);
    }

    function goDiscardDraft(pageID) {
	new Ajax.Request(ajax, {parameters:'pos=blogdiscard&pageid='+pageID+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerDiscardDraft});
    }

    function handlerDiscardDraft(t) {
	Pages.go('updateList', currentPage);
    }

    function goSaveDraft(blogID) {
	if (currentBlog.postId == 1) { currentBlog.saving = 1; }
	new Ajax.Request(ajax, {parameters:'pos=blogsavedraft&blogid='+blogID+'&title='+encodeURIComponent($('blog-post-title').value)+'&categories='+encodeURIComponent($('blog-post-categories').value)+'&comments='+$('draft-comments').value+'&date='+encodeURIComponent($('blog-date-field').value)+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerSaveDraft});
    }

    function handlerSaveDraft(t) {
	Pages.go('updateList', currentPage);
    }

    function goPublishPost(pageID, update) {
	if (currentBlog.postId == 1) { currentBlog.saving = 1; }
	new Ajax.Request(ajax, {parameters:'pos=blogpublish&pageid='+pageID+'&title='+encodeURIComponent($('blog-post-title').value)+'&categories='+encodeURIComponent($('blog-post-categories').value)+'&comments='+$('draft-comments').value+'&date='+encodeURIComponent($('blog-date-field').value)+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess: function(t) { handlerPublishPost(t, update)}});
    }
    function handlerPublishPost(t, update) {
	if (!update) { Pages.go('updateList', currentPage); }
    }

    function goSavePost(postID, update) {
        new Ajax.Request(ajax, {parameters:'pos=blogsave&postid='+postID+'&title='+encodeURIComponent($('blog-post-title').value)+'&categories='+encodeURIComponent($('blog-post-categories').value)+'&comments='+$('post-comments').value+'&date='+encodeURIComponent($('blog-date-field').value)+'&permalink='+encodeURIComponent($('permalink-url').value)+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess: function(t) { handlerSavePost(t, update)}});
    }
    function handlerSavePost(t, update) {
	if (!update) { Pages.go('updateList', currentPage); }
    }

    function goDeletePost(postID) {
	new Ajax.Request(ajax, {parameters:'pos=blogdeletepost&postid='+postID+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerDeletePost});
    }

    function handlerDeletePost(t) {
	updateList(currentPage);
    }

    function saveBlogSettings() {

	new Ajax.Request(ajax, {parameters:'pos=blogsettingssave&blogid='+currentBlog.blogId+'&allow-comments='+$('blog-settings-allow-comments').value+'&email-notify='+$('blog-settings-email-comments').value+'&notify-address='+$('blog-settings-notify-address').value+'&close-comments='+$('blog-settings-close-comments').value+'&date-format='+$('blog-settings-date-format').value+'&time-format='+$('blog-settings-time-format').value+'&time-zone='+$('blog-settings-time-zone').value+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:function() { Pages.go('main'); }});

    }

    function goDeleteComment(commentID) {
        new Ajax.Request(ajax, {parameters:'pos=blogdeletecomment&commentid='+commentID+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerDeleteComment});
    }

    function handlerDeleteComment(t) {
	t.responseText = t.responseText.replace(/[^0-9]/, '');
	if (t.responseText.match(/[0-9]+/)) {
	  new Effect.Fade($(t.responseText+''));
	}
    }

    function goTrackback() {
	if (!$('trackback-url') || !$('trackback-url').value) return;

	$('trackback-status').innerHTML = '';
	new Ajax.Request(ajax, {parameters:'pos=dotrackback&postid='+currentBlog.postId+'&url='+encodeURIComponent($('trackback-url').value)+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc, onSuccess:handlerTrackback});
    }

    function handlerTrackback(t) {

	$('trackback-url').value = '';
	if (t.responseText.match("%%SUCCESS%")) {
	  $('trackback-status').innerHTML = /*tl(*/'Success!'/*)tl*/;
	} else {
	  $('trackback-status').innerHTML = /*tl(*/'Trackback failed.'/*)tl*/;
	}


    }

    function calendarCallback(calendar, newDate) {

        $('blog-date').innerHTML = newDate;
        $('blog-date-field').value = newDate;
        new Effect.Highlight('blog-date',{duration: 2.0});

    }



    function displayPageProperties(pageID) {

	currentPage = pageID;

        new Ajax.Request(ajax, {parameters:'pos=editpage&reqid='+pageID+'&cookie='+document.cookie, onSuccess:handlerDisplayPageProperties, onFailure:errFunc, onException:exceptionFunc});
    
    }
                                                                                                                         
    function handlerDisplayPageProperties(t) {
        
	$('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"saveProperties('editpage', '"+currentPage+"'); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>cancel</a></div></div>";

        $('propertiesTitle').innerHTML = /*tl(*/"Edit Page Properties"/*)tl*/;

        var propertiesBasic  = $('propertiesBasic');
        propertiesBasic.innerHTML = "<br/><p>"+/*tl(*/"In this section you can make various changes to your page, including changing its title, and marking it for deletion."/*)tl*/+"</p><br/>"+t.responseText;

        Element.hide('propertiesBasicHeader');
        Element.hide('textEditBox');
	$('textEditorBox').value = '';
        Element.hide('propertiesAdvancedHeader');
        Element.hide('propertiesAdvanced');
	$('propertiesAdvanced').innerHTML = '';

        showProperties(45);	
	Element.show('propertiesBasic');

    }

    function displayUserSettings() {
             
        new Ajax.Request(ajax, {parameters:'pos=usersettings&cookie='+document.cookie, onSuccess:handlerDisplayUserSettings, onFailure:errFunc, onException:exceptionFunc});
                                                                                                                             
    }
                                                                                                                             
    function handlerDisplayUserSettings(t) {
                                                                                                                             
	$('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"saveProperties('usersettings', ''); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>"+/*tl(*/"cancel"/*)tl*/+"</a></div></div>";

        $('propertiesTitle').innerHTML = /*tl(*/"Edit Profile"/*)tl*/;
                                                                                                                             
        var propertiesBasic  = $('propertiesBasic');
        propertiesBasic.innerHTML = t.responseText;
                                                                                                                             
        Element.hide('propertiesBasicHeader');
        Element.hide('textEditBox');
        $('textEditorBox').value = '';
        Element.hide('propertiesAdvancedHeader');
        Element.hide('propertiesAdvanced');
        $('propertiesAdvanced').innerHTML = '';
                    
        showProperties();
        Element.show('propertiesBasic');
    }

    function displaySiteSettings(update) {

	if (update == 1) {
          var t = new Ajax.Request(ajax, {parameters:'pos=sitesettings&reqid='+currentSite+'&cookie='+document.cookie, onSuccess:function(t){ handlerDisplaySiteSettings(t, update)}, onFailure:errFunc, onException:exceptionFunc});

	} else { handlerDisplaySiteSettings({responseText: ""},2); }

    }

    var siteAccordion;
    var oldSitePassword;
    function handlerDisplaySiteSettings(t, update) {

	if (update == 1) {

	$('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"saveProperties('sitesettings',currentSite); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>"+/*tl(*/"cancel"/*)tl*/+"</a></div></div>";

          $('propertiesTitle').innerHTML = "Edit Site Settings";

          var propertiesBasic  = $('propertiesBasic');
          propertiesBasic.innerHTML = t.responseText;

          Element.hide('propertiesBasicHeader');
          Element.hide('textEditBox');
          $('textEditorBox').value = '';
          Element.hide('propertiesAdvancedHeader');
          Element.hide('propertiesAdvanced');
          $('propertiesAdvanced').innerHTML = '';

	}

	siteAccordion = new accordion('siteSettingsAccordion', {classNames: { toggle: 'accordion_toggle', toggleActive: 'accordion_toggle_active', content: 'accordion_content'}, defaultSize: { width: 525 }});
	$$('#siteSettingsAccordion .accordion_content').each(function(el) { if(el.id != 'siteSettingsOpened') { el.style.height = '0px'; } });
	siteAccordion.activate($$('#siteSettingsAccordion .accordion_toggle')[0]);
	$('siteSettingsOpened').style.display = 'block';
	$('siteSettingsOpened').style.height = '0px';

        showProperties(45);
        Element.show('propertiesBasic');

	if ($('site_password'))
	  oldSitePassword = $('site_password').value;

	//var verticalAccordion = new accordion('#siteSettingsAccordion');

	//if( responseText.indexOf('Auto-Generated') > -1) { setTimeout("showTip(\"We've generated this name for you, but you're more than welcome to change it to anything you'd like.\", 'userIdentifier', 'y', 'userIdentifier');", 1500); setTimeout("hideTip('tipuserIdentifier');", 10000); }

    }
   
   function showHideTitle() {

	if ($('show_title').checked) {
	  hideTitle = false;
	  $('hide_title').value = 0;
      $('weebly_site_title').setStyle({'visibility':'visible'});
	} else {
	  hideTitle = true;
	  $('hide_title').value = 1;
      $('weebly_site_title').setStyle({'visibility':'hidden'});
	}

   }

    function saveMetaInfo() {
    	 var meta_description = encodeURIComponent($('meta_description').value);
	 var meta_keywords = encodeURIComponent($('meta_keywords').value);
	 var footer_code = encodeURIComponent($('footer_code').value);
	 var header_code = encodeURIComponent($('header_code').value);
	 // ajax call to check domain
         new Ajax.Request(ajax, {parameters:'pos=savemetainfo&metaKeywords='+meta_keywords+'&metaDescription='+meta_description+'&footerCode='+footer_code+'&headerCode='+header_code+'&cookie='+document.cookie, onSuccess:handlerSaveMetaInfo, onFailure:errFunc, bgRequest: true, onException:exceptionFunc});

    }

    function handlerSaveMetaInfo(x) {
    	
	   if(x.responseText.match(/%%SUCCESS%%/)){
	   	//Effect.SlideUp("showMetaProperties");
		$('errorProperties').style.display="hidden";
 		$('errorProperties').innerHTML = "";
	   }else if(x.responseText.match(/%%DESCRIPTIONLENGTH%%/)){
		$('errorProperties').innerHTML = /*tl(*/"Description too long, please shorten to 150 characters"/*)tl*/;
		$('errorProperties').style.display="block";
	   }else if(x.responseText.match(/%%KEYWORDLENGTH%%/)){
	   	$('errorProperties').innerHTML = /*tl(*/"Too many keywords, please limit to 1024 characters"/*)tl*/; 
		$('errorProperties').style.display="block";
	   }else if(x.responseText.match(/%%ERROR%%/)){
	   	$('errorProperties').innerHTML = /*tl(*/"Error saving, please try again."/*)tl*/; 
		$('errorProperties').style.display="block";
	   }
    }  
 
    function verifyDomainForm() {
	
	// check all form values before submitting
    	if ($('CCName').value == "") {

		$('domainError').innerHTML = /*tl(*/"Error: Please enter Cardholder's Name"/*)tl*/;

	}else if ($('CreditCardNumber').value =="") {
		
		$('domainError').innerHTML = /*tl(*/"Error: Please enter Card Number"/*)tl*/;
	
	}else if ($('CVV2').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter CVV2"/*)tl*/;

	}else if ($('CCAddress').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter Address"/*)tl*/;
	
 	}else if ($('RegistrantCity').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter City/Town"/*)tl*/;

 	}else if ($('CCCountry').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter Country"/*)tl*/;
 	
	}else if ($('CCZip').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter Zip/Postal Code"/*)tl*/;
	
	}else if ($('CCPhone').value =="") {

                $('domainError').innerHTML = /*tl(*/"Error: Please enter Phone"/*)tl*/;
 
	}else {
		// purchase the domain
		$('domainError').innerHTML = "";
		var newEl = document.createElement('DIV');
		newEl.id = 'purchaseDomainLoading';
		newEl.innerHTML = "Loading...";
		newEl.style.padding = "8px 0 0 50px";
		$('purchaseDomain').parentNode.appendChild(newEl);
		$('purchaseDomain').style.display = "none";
		purchaseDomain();
	}
    }

    function purchaseDomain() {
	
	// Create register.com account for customer

        // Loop through form fields
        var thisform = document.forms.chooseDomainCreditForm;
        var form2 = document.forms.domainLength;
        var formdata = "";
        for (i=0; i < thisform.length; i++) {

           // Build post string
           if(thisform.elements[i].type == "text"){ // Textbox's
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }else if(thisform.elements[i].type == "textarea"){ // Textareas
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }else if(thisform.elements[i].type == "checkbox"){ // Checkbox's
                 formdata = formdata + thisform.elements[i].name + "=" + thisform.elements[i].checked + "&";
           }else if(thisform.elements[i].type == "radio"){ // Radio buttons
                  if(thisform.elements[i].checked==true){
                     formdata = formdata + thisform.elements[i].name + "=" + thisform.elements[i].value + "&";
                  }
           }else{
                  // select box is all thats left
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }
        }

        for (x=0; x< form2.length; x++) {
                if (form2.elements[x].type == "radio"){ // Radio buttons
                        if(form2.elements[x].checked==true){
                                formdata = formdata + form2.elements[x].name + "=" + form2.elements[x].value + "&";
                        }
                }
        }

        //console.log(formdata);

        // break apart CredCardExpiration into CreditCardExpMonth and CreditCardExpYear
        //var CCX = $('CreditCardExpiration').value.split("\/");
        //var CreditCardExpMonth = CCX[0];
        //var CreditCardExpYear = CCX[1];

 	// break down domain
        var finalDomain = $('finalDomainName').innerHTML.split(".");
        var sld = finalDomain[0];
        var tld = finalDomain[1];

        // https call
	var reqId = Math.floor(Math.random()*1000001);
	var req   = document.createElement('script');
	req.id    = reqId;
	req.type  = 'text/javascript';
	req.src   = 'https://secure.weebly.com/weebly/apps/registerHttps.php?reqid='+reqId+'&pos=CreateRegisterAccount&tld='+tld+'&sld='+sld+'&'+formdata+'&cookie='+document.cookie+'';
	
	//console.log(req.src);
	
	// Append to HEAD element
	document.childNodes[1].childNodes[0].appendChild(req);
	
	// PHP script returns:
	//handlerFunction(reqId, 'responseText.....');
	

	//new Ajax.Request(ajax, {parameters:'pos=CreateRegisterAccount&tld='+tld+'&sld='+sld+'&CreditCardExpMonth='+CreditCardExpMonth+'&CreditCardExpYear='+CreditCardExpYear+'&'+formdata+'&cookie='+document.cookie, onSuccess:handlerPurchaseDomain, onFailure:errFunc, bgRequest: false, onException:exceptionFunc});
    }

    function handlerFunctionHttps(x) {
	//alert(reqId);
	
	if (x.match(/%%SUCCESS%%/)) {
                var orderID = x.split("|");
                orderID = orderID[1];
                //alert('Success! Order ID: '+orderID);
		
		completeDomainPurchase(orderID);
        }else{
		if ($('purchaseDomainLoading') && $('purchaseDomainLoading').parentNode) {
		  $('purchaseDomainLoading').parentNode.removeChild($('purchaseDomainLoading'));
		}
		$('purchaseDomain').style.display = 'block';
                $('domainError').innerHTML = x;
        }	



	// Clean up request script tag
        //$('reqId').parentNode.removeChild($('reqId'));
     }
    
     function purchaseDomain2() {

	// Create register.com account for customer

	// Loop through form fields
	var thisform = document.forms.chooseDomainCreditForm;
	var form2 = document.forms.domainLength;
	var formdata = "";
	for (i=0; i < thisform.length; i++) {
        
	   // Build post string
           if(thisform.elements[i].type == "text"){ // Textbox's
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }else if(thisform.elements[i].type == "textarea"){ // Textareas
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }else if(thisform.elements[i].type == "checkbox"){ // Checkbox's
                 formdata = formdata + thisform.elements[i].name + "=" + thisform.elements[i].checked + "&";
           }else if(thisform.elements[i].type == "radio"){ // Radio buttons
                  if(thisform.elements[i].checked==true){
                     formdata = formdata + thisform.elements[i].name + "=" + thisform.elements[i].value + "&";
                  }
           }else{
                  // select box is all thats left
                  formdata = formdata + thisform.elements[i].name + "=" + escape(thisform.elements[i].value) + "&";
           }
        }

	for (x=0; x< form2.length; x++) {
		if (form2.elements[x].type == "radio"){ // Radio buttons
                  	if(form2.elements[x].checked==true){
                     		formdata = formdata + form2.elements[x].name + "=" + form2.elements[x].value + "&";
                  	}
		}
	}

	//console.log(formdata);
	
	// break apart CredCardExpiration into CreditCardExpMonth and CreditCardExpYear
	var CCX = $('CreditCardExpiration').value.split("\/");
	var CreditCardExpMonth = CCX[0];
	var CreditCardExpYear = CCX[1];
	
	// break down domain
	var finalDomain = $('finalDomainName').innerHTML.split(".");
	var sld = finalDomain[0];
	var tld = finalDomain[1];

	new Ajax.Request(ajax, {parameters:'pos=CreateRegisterAccount&tld='+tld+'&sld='+sld+'&CreditCardExpMonth='+CreditCardExpMonth+'&CreditCardExpYear='+CreditCardExpYear+'&'+formdata+'&cookie='+document.cookie, onSuccess:handlerPurchaseDomain, onFailure:errFunc, bgRequest: false, onException:exceptionFunc});
    }

    function handlerPurchaseDomain(x) {
	if (x.responseText.match(/%%SUCCESS%%/)) {
		var orderID = x.responseText.split("|");
		orderID = orderID[1];
		alert('Success! Order ID: '+orderID);
		Element.hide('chooseDomain');
	}else{
    		$('domainError').innerHTML = x.responseText;
	}
    }

    function updatePageTitle(theId, value) {

	sitePages[theId] = value.innerHTML;

    }

    function updateSiteTitle(value) {

	currentTitle = value;

	new Ajax.Request(ajax, {parameters:'pos=sitetitle&newtitle=' + encodeURIComponent(value)+'&cookie='+document.cookie, onFailure:errFunc, onException:exceptionFunc});

    }
    
    function afterUpdateSiteTitle() {
		if (window.refreshNavCondense) {
			refreshNavCondense();
		}
    }

    function doClick( elId) {

	var evt;
	var el = document.getElementById(elId);
	if (document.createEvent){
	  evt = document.createEvent("MouseEvents");
	  if (evt.initMouseEvent){
	    evt.initMouseEvent("click", true, true, window,0, 0, 0, 0, 0, false, false, false, false, 0, null);
	  } else {
	    evt = false;
	  }
	}
	(evt)? el.dispatchEvent(evt) : (el.click && el.click());

    }

    function exportSite() {

	// Is a regular site
	if (settingQuickExport == 0) {
	  Pages.go('domainMenu',1);
          //new Ajax.Request(ajax, {parameters:'pos=sitesettings&reqid='+currentSite+'&cookie='+document.cookie, onSuccess:handlerExportSite, onFailure:errFunc, onException:exceptionFunc});
	} else {
	  Pages.go('doExport');
	}

    }

    function handlerExportSite(t) {

	$('saveProperties').innerHTML = "<div style='height: 50px; margin-top: 20px;'><a href='#' style='float: left;' onClick=\"saveProperties('sitesettings', currentSite, 1); Pages.go('doExport'); return false;\"><img src='http://"+editorStatic+/*tli(*/"/weebly/images/savebtn.jpg"/*)tli*/+"' border='0'/></a><div style='margin: 23px 0 0 5px; font-size: 14px; float: left;'> or&nbsp;<a href='#' onClick=\"Pages.go('main'); return false;\" style='position: absolute; font-weight: bold; font-size: 14px; color: #CE2424; text-decoration: underline;'>cancel</a></div></div>";

        $('propertiesTitle').innerHTML = "Publish Site";

        var propertiesBasic  = $('propertiesBasic');
        propertiesBasic.innerHTML = t.responseText;

        Element.hide('propertiesBasicHeader');
        Element.hide('textEditBox');
        $('textEditorBox').value = '';
        Element.hide('propertiesAdvancedHeader');
        Element.hide('propertiesAdvanced');
        $('propertiesAdvanced').innerHTML = '';

	publishingWindow = 1;

        showProperties(45);
        Element.show('propertiesBasic');

    }

    function doExport() {

	var wait = 0;

	showPublishingAnimation();

	if (wait) {
          setTimeout("new Ajax.Request(ajax, {parameters:'pos=exportsite&cookie='+document.cookie, onSuccess:handlerDoExport, onFailure:errFunc, bgRequest: true});", 350);
	} else {
          new Ajax.Request(ajax, {parameters:'pos=exportsite&cookie='+document.cookie, onSuccess:handlerDoExport, onFailure:errFunc, bgRequest: true});
	}
	
    }

    function handlerDoExport(t) {

	if (t.responseText.indexOf('siteLocation') > 0) {

	  // Editing a blog post
	  //if ($('secondlist').innerHTML.match('%%EDITING-NEW-POST%%') || $('secondlist').innerHTML.match('%%EDITING-DRAFT-POST%%:') || $('secondlist').innerHTML.match('%%EDITING-EXISTING-POST%%:')) {
	  //  Pages.go('updateList', currentPage);
	  //} else {
	    $('exportText').innerHTML = t.responseText;
	    currentSiteLocation = $('siteLocation').innerHTML;
	    Pages.go('exportSuccess');
	  //}

	  // Site has been exported, don't show export options any more
	  settingQuickExport = 1;
	  // Change title from Publishing to congratulations
	  $('chooseAddress1').src = "http://"+editorStatic+/*tli(*/"/weebly/images/congratulations.jpg"/*)tli*/;
	} 
	if (t.responseText.match(/Error\s/i)) {
	  Element.hide('tip14');
	  showError(t.responseText);
	  Pages.go('main');
	}
    if(t.responseText.match(/%%UPGRADEELEMENTS%%/)){
        $('exportText').show().innerHTML = '<div style="font-family: \'Lucida Grande\', \'Lucida Sans\', \'Trebuchet MS\', Helvetica, Arial, Verdana, sans-serif; font-size: 16px; line-height:1.5; font-weight:bold;">Your site contains Weebly Pro elements. Your published site will not include these Pro elements unless you choose to upgrade.</div><div style="float:right; margin-top:35px; margin-right:85px;"><a style="font-size:14px; color:#0054CD; text-decoration:none; font-weight:bold;" href="#" onclick="exportSite(); return false;">Publish Anyway</a></div><div style="width:159px; margin:25px auto;"><a href="#" onclick="alertProFeatures(\'Upgrade to publish Pro elements\'); return false;"><img src="http://'+editorStatic+'/weebly/images/upgrade_large.gif" alt="Upgrade" style="border:none;" /></a></div>';
    }

    }

// Chris' code -- not formatted correctly! :) -David
function unPublishSite(siteId){
	//alert('siteId: '+siteId+' userId: '+userId);
	//Display warning
	$("unpublish").innerHTML="<img src='http://"+editorStatic+"/weebly/images/action_stop.gif' style='border: 0; margin-right: 3px;' alt='Red X' /><span style='font-weight: bold; color: #555;'>"+/*tl(*/"Are you sure?"/*)tl*/+" </span><a href='#' onClick='doUnPublishSite(\""+siteId+"\");' style='color: green;'>"+/*tl(*/"Yes, Un-Publish"/*)tl*/+"</a>";

}
function doUnPublishSite(siteId){
	new Ajax.Request(ajax, {parameters:'pos=unpublishsite&siteid='+siteId+'&cookie='+document.cookie, onSuccess:handlerunPublishSite, onFailure:errFunc, bgRequest: true});
}
function handlerunPublishSite(t) {
	if(t.responseText.match(/%%SUCCESS%%/)) {
		saveProperties('sitesettings',currentSite);
		//$("unpublish").style.display = "none";
		// be sure to show site settings before next publish
		//settingQuickExport = 0;
	}else{
		showError('Error deleting site: '+ t.responseText);
		}
	}
// End Chris' code
    
    function previewSite(theme) {

        new Ajax.Request(ajax, {parameters:'pos=previewsite&theme='+theme+'&cookie='+document.cookie, onSuccess:handlerPreviewSite, onFailure:errFunc});
                                                                                                                             
    }

    function handlerPreviewSite(t) {

	if (t.responseText) { window.open("http://"+configSiteName+"/weebly/previewSite.php?req_id="+t.responseText); }

    }

    function exportSiteZip() {

        new Ajax.Request(ajax, {parameters:'pos=exportsitezip&cookie='+document.cookie, onSuccess:handlerExportSiteZip, onFailure:errFunc});

    }

    function handlerExportSiteZip(t) {

        if (t.responseText.match("error")) { 
	  showError(t.responseText); 
	} else { 
	  window.open("http://"+configSiteName+"/weebly/exports/"+userID+"/"+t.responseText+".zip"); 
	}

    }

    function closeCreateLinkText(linkUrl) {

	runCommand(currentBox,'createlink', linkUrl);

    }

    function showCreateLink() {

        var menuPos  = new Position.cumulativeOffset(document.getElementById(currentBox+'Edit'));
	Weebly.Linker.show('closeCreateLinkText', {'top': 185, 'left': 250}, Array('linkerWebsite', 'linkerWeebly', 'linkerEmail', 'linkerFile'), 'linkerWeebly', currentBox);

    }

    function showCreateLinkProperties(ucfpid) {

	Weebly.Linker.ucfpid = ucfpid;
        var menuPos  = new Position.cumulativeOffset(document.getElementById('textEditor'));
	var linkerTypes = Array('linkerWebsite', 'linkerWeebly', 'linkerEmail', 'linkerFile');
	var linkerSelected = 'linkerWeebly';
	if (Weebly && Weebly.Elements && Weebly.Elements.currentContentFieldDef && Weebly.Elements.currentContentFieldDef.hasimage == 1) { linkerTypes.push('linkerImage'); linkerSelected = 'linkerImage'; }
    else if(Weebly && Weebly.Elements && Weebly.Elements.currentContentFieldDef && Weebly.Elements.isFormElement()) { linkerTypes = Array('linkerWebsite', 'linkerWeebly'); }
        Weebly.Linker.show('closeCreateLinkProperties', {'top': 185, 'left': 250}, linkerTypes, linkerSelected, currentElement);

    }

    function closeCreateLinkProperties(linkUrl) {

    if( $('webpageNewWindow') && $('webpageNewWindow').checked )
    {
       var link = linkUrl.replace('weeblylink_new_window','');
       $(Weebly.Linker.ucfpid+"").value = "<a href='"+link+"' target='_blank'>";
    }
    else
    {
        $(Weebly.Linker.ucfpid+"").value = "<a href='"+linkUrl+"'>";
    }
	Weebly.Elements.onChange(Weebly.Linker.ucfpid);
	Behaviour.apply();

    }

    function selectTheme(theme, makeFavorite, favoriteCallback) {

	$('focusMe').focus();
	Pages.go('themesMenu');
	
	$$('.themePictureImgActive').each(function(el){el.className = 'themePictureImg';});
	//$('theme_'+currentTheme).className = 'themePictureImg';

	currentTheme = theme;
	
	if (previewingThemes) {
		$('theme_' + currentPreviewTheme).removeClassName('themePictureImgPreview');
		previewingThemes = 0;
		currentPreviewTheme = '';
	}
	previewThemeEventCnt++;

	if ($('theme_'+currentTheme)) {
		$('theme_'+currentTheme).className = 'themePictureImgActive';
	}

	loadDesignOptions(currentTheme);
    writeTheme(currentTheme);
	new Ajax.Request(ajax, {
		parameters: 'pos=settheme&keys='+theme+(makeFavorite ? '&favorite=1' : '')+'&cookie='+document.cookie,
		onSuccess: function(t) {
			if (favoriteCallback) {
				favoriteCallback();
			}
			handlerSelectTheme(t);
		},
		onFailure: errFunc,
		bgRequest: true
	});

    }

    function handlerSelectTheme(t) {

	updatedTheme = 1;

	if (!userEvents.tab_pages) {
	  showEvent('selectTheme', 1);
	}

    }

    function showThemeOptions(noScroll) {

	// If not in the themes tab, bail
	if ($('weebly_tab_themes').className == "weebly-notcurrent") { return; }

	/**
	// Uncomment to enable theme switching
	if (noScroll) {
	  $('themeCategories').style.display = 'none';
	  $('themesBack').style.display = 'none';
	} else {
	  Effect.Fade('themeCategories');
	  Effect.Fade('themesBack');
	}
	$('themesShowThemes').style.display='block';
	$('themesForward').style.display='none';

	if (noScroll) {
	  var moveTo = document.body.clientWidth <= 800 ? 215 : 305;
	  $('themePictures').style.left = (document.body.clientWidth-moveTo)+"px";
	} else {
	  var moveTo = document.body.clientWidth <= 800 ? 215 : 305;
	  new Effect.Move('themePictures', {y: 30, x: (document.body.clientWidth-moveTo), mode: 'absolute'});
	}

	$('currentThemeOptions').style.width = (document.body.clientWidth-268)+"px";

	if (noScroll) {
	  $('themeSettings').style.display = 'block';
	} else {
	  Effect.Appear('themeSettings');
	}

	$('currentThemeImage').src = $("theme_"+currentTheme).src;
	$('currentThemeText').innerHTML = currentTheme;

	firstThemeOnClick = $('themePictures').firstChild.onclick;
	secondThemeOnClick = $('themePictures').childNodes[1].onclick;
	$('themePictures').firstChild.onclick = function() { hideThemeOptions(); };
	$('themePictures').childNodes[1].onclick = function() { hideThemeOptions(); };
	new Effect.Opacity($('themePictures').firstChild, {from: 1.0, to: 0.3});
	new Effect.Opacity($('themePictures').childNodes[1], {from: 1.0, to: 0.3});
	**/

	// Disable theme switching
	if (ENABLE_THEME_BROWSER) {
		$('theme-action-tabs').style.display = 'none';
	}else{
		$('themeCategories').style.display = 'none';
	}
	$('themesBack').style.display = 'none';
	$('themesForward').style.display='none';
	$('themesShowThemes').style.display='none';
	$('themePictures').style.display='none';
	$('currentThemeImage').style.display='none';
	
	$('themeSettings').style.display = 'block';
	$('currentThemeOptions').style.width = (document.body.clientWidth-115)+"px";

	editThemeMode = 1;	

    }

    function hideThemeOptions() {

	// If not in the themes tab, bail
	if ($('weebly_tab_themes').className == "weebly-notcurrent") { return; }

	/**
	// Uncomment to enable theme switching
	Effect.Appear('themeCategories');
	$('themesForward').style.display='block';
	$('themesShowThemes').style.display='none';
        new Effect.Move('themePictures', {y: 30, x: 0, mode: 'absolute'});

	Effect.Fade('themeSettings');

	$('themePictures').firstChild.onclick = firstThemeOnClick; 
	$('themePictures').childNodes[1].onclick = secondThemeOnClick; 
	new Effect.Opacity($('themePictures').firstChild, {from: 0.3, to: 1.0});
	new Effect.Opacity($('themePictures').childNodes[1], {from: 0.3, to: 1.0});

        editThemeMode = null;
	**/

    }
    

    function showThemes(themesCategory) {

		if (!themesCategory) {
			if (ENABLE_THEME_BROWSER) {
				themesCategory = "All";
			}else{
				themesCategory = $$('#themes_select div')[0].innerHTML;
			}
		}

		themesCategory = themesCategory == "All" ? "" : themesCategory;
		
	    if(themesCategory === 'custom') {
	        showCustomThemes();
	    }else{
			Weebly.Cache.get(ajax, 'pos=getthemes&keys='+themesCategory, handlerShowThemes);
		}
		
    }

    function handlerShowThemes(responseText) {

		//var themeReturn = responseText.split(/%%/);

		$('themePicturesInner').innerHTML = responseText;

		if ($('theme_'+currentTheme)) {
			$('theme_'+currentTheme).className = 'themePictureImgActive';
		}

		if (ENABLE_THEME_BROWSER) {
			$('themePicturesInner').childElements().each(function(a) {
				var id = a.down().id.match(/^theme_(.*)$/)[1];
				initThemeFavoriting(id, a.select('span.themeFavoriteIcon')[0], a, 'themeIsFavorite');
			});
		}

    }
    
    function highlightThemeCategory(e)
    {
    	if (!ENABLE_THEME_BROWSER) {
	        $$('a.elements_category_selected').each(function(el){el.writeAttribute('class', 'elements_category');});
    	    e.writeAttribute('class', 'elements_category_selected');
    	}
    }
    
    var isScrollingThemes = false;

	function advanceThemes() {
		var slideWidth = document.body.clientWidth - $('themePictures').offsetLeft - 95,
			inner = $('themePicturesInner'),
			innerLeft = inner.offsetLeft,
			innerWidth = inner.offsetWidth,
			themesBack = $('themesBack'),
			themesForward = $('themesForward');
		themesForward.blur();
		if (innerLeft + innerWidth > slideWidth) {
			themesBack.hide();
			themesForward.hide();
			isScrollingThemes = true;
			new Effect.Move(inner, {
				x: -slideWidth,
				afterFinish: function() {
					themesBack.show();
					if (innerLeft + innerWidth - slideWidth > slideWidth) {
						themesForward.show();
					}
					isScrollingThemes = false;
				}
			});
		}
	}

    function preceedThemes() {
		var slideWidth = document.body.clientWidth - $('themePictures').offsetLeft - 95,
			inner = $('themePicturesInner'),
			innerLeft = inner.offsetLeft,
			innerWidth = inner.offsetWidth,
			themesBack = $('themesBack'),
			themesForward = $('themesForward');
		themesBack.blur();
		if (innerLeft < 0) {
			themesBack.hide();
			themesForward.hide();
			isScrollingThemes = true;
			new Effect.Move(inner, {
				x: slideWidth,
				afterFinish: function() {
					if (innerLeft + slideWidth < 0) {
						themesBack.show();
					}
					themesForward.show();
					isScrollingThemes = false;
				}
			});
		}
    }
    
    function resetThemesMenuScrolling() {
    	$('themePicturesInner').style.left = 0;
    	$('themesBack').hide();
    	$('themesForward').show();
    }

    function showBar() {

        if (navigator.appVersion.indexOf("MSIE") == -1 || navigator.appVersion.indexOf("MSIE 7") > -1) {
          new Effect.Move("elements", {x: 0, y:0, mode: "absolute"});
        }
        $("elements").className="topend";
        //Element.hide("expand");
        //Element.show("shrink");

    }

    function hideBar() {

        if (navigator.appVersion.indexOf("MSIE") == -1 || navigator.appVersion.indexOf("MSIE 7") > -1) {
          new Effect.Move("elements", {x: 0, y:-100, mode: "absolute"});
        }
        $("elements").className="topstart";
        //Element.hide("shrink");
        //Element.show("expand");

    }

    function expandThemeSettings() {

        //Effect.Fade('expandThemes');
        if ($('themeSettings').style.display == "none") {
          Effect.SlideDown('themeSettings');
        } else {
          Effect.SlideUp('themeSettings');
        }

    }

    function editImage(oldImageLocation, imageId) {

	new Ajax.Request(ajax, {parameters:'pos=externalsite&to=snipshot&cookie='+document.cookie, onSuccess: function(t) { handlerEditImage(t, oldImageLocation, imageId) }, onFailure:errFunc, onException:exceptionFunc});


    }

    function handlerEditImage(t, oldImageLocation, imageId) {

	$('editImageFrame').style.height = (getInnerHeight() - 35) + "px";
	$('editImage').style.height = (getInnerHeight() - 35) + "px";
	Element.show("editImageFrame");
	$("editImage").src = "http://services.snipshot.com/?snipshot_input="+escape(oldImageLocation)+"&snipshot_callback="+escape("http://"+configSiteName+"/weebly/remoteCall.php")+"&snipshot_callback_agent=user&snipshot_output_type=url&reqid="+t.responseText+"&from=snipshot&imageLocation="+escape(oldImageLocation)+"&imageId="+imageId;

    }

    function hideEditImage() {

	Element.hide("editImageFrame");
	$("editImage").src = "";

    }

    function closeEditImage(newImageLoc, imageId, ucfid) {

	Pages.go('main');
        Weebly.Elements.selectElement($(""+ucfid));
        $(""+imageId).value = newImageLoc;
        Weebly.Elements.onChange(imageId);

    }

    /*** Element Chooser Functions ***/
/****************************************/

    function setElementsPageType(){
        if(currentBlog.editingBlog){ 
            elementsPage('blog'); 
        } 
        else{
            if(Weebly.Elements.highlightedElement){
                elementsPage('form');
            }
            else{
                elementsPage('default');
            }
        }
    }

    var currentCategoryList;
    function elementsPage(categoryType) {

	currentCategoryList = Weebly.elementCache[categoryType];
	$('elements_1').innerHTML = generateCategoryHTML(currentCategoryList.order);
	selectCategory($$('#elements_1 a')[0].id);

    }

    function generateCategoryHTML(categoryOrder) {

	var categoryHTML = '';
	for (var i = 0; i < categoryOrder.length; i++) {

	  var cc = currentCategoryList[categoryOrder[i]];
	  var sprite = cc['icon'] ? cc['icon'] : 'folder_images';

	  categoryHTML += "<a href=\"#\" onClick=\"selectCategory('ec_"+cc['category_id']+"'); return false;\" class=\"elements_category\" id=\"ec_"+cc['category_id']+"\"><span class=\"elements_folder sprite-small-icons\" id='sprite-"+sprite+"'></span> "+cc['category_name']+"</a>\n";

	}

	return categoryHTML;

    }

    function generateElementHTML(category) {

	var elementHTML = '';

	if (category.category_name == "Imported") {

	  new Ajax.Request(ajax, {parameters:'pos=customelements&cookie='+document.cookie, 'onSuccess':updateCustomElements, 'onFailure':errFunc});

	} else {

	  for (var i=0; i < category.elements.length; i++) {
	    elementHTML += outputElement(category.elements[i].element_id, category.elements[i].display_name, 0, category.elements[i].class_names, category.elements[i].defaults, category.elements[i].icon, category.elements[i].override_title);
	  }

	}

	return elementHTML;

    }

    function selectCategory(categoryName) {

	$('focusMe').focus();
	categoryName = categoryName.replace("ec_", "");

	var currentCategory = currentCategoryList[categoryName];

	if (!currentCategory) {
          Element.hide('elements_2');
	  $('elementlist').style.left  = "150px";
	  $('elementlist').style.width = (document.body.clientWidth-150)+'px';
	  $('elementlist').innerHTML = "<div style='color: black; padding: 5px 0 0 10px;'>"+/*tl(*/"Coming soon..."/*)tl*/+"</div>";
	  return;
	}

	var pos = 1;
	var lvl = 1;

	if (currentCategory.children.length > 0) {
	  $('elements_2').innerHTML = generateCategoryHTML(currentCategory.children);
	  pos = 2;
	} else if (currentCategory.parent > 0) {
	  pos = 2;
	  lvl = 2;
	}

	if (pos == 2) {
          Element.show('elements_2');
	  $('elementlist').style.left  = "300px";
	  $('elementlist').style.width = (document.body.clientWidth-300)+'px';
	} else {
          Element.hide('elements_2');
	  $('elementlist').style.left  = "150px";
	  $('elementlist').style.width = (document.body.clientWidth-150)+'px';
	}

	$('elementlist').innerHTML = generateElementHTML(currentCategory);

	showEvent('elcat_'+categoryName);
        activeElementCategory('ec_'+categoryName,lvl);

	//Behaviour.apply();
		//console.log('select cat name INITDRAGGABLES...');
    	initDraggables();
	currentMenu = categoryName;
    }

    function updateCustomElements(t) {

	if (t.responseText.indexOf(':') > -1) {

          var splitElements = new Array();
          splitElements = t.responseText.split(',');
	
	  var outputElements = '';

	  for(var x=0; x < splitElements.length; x++) {

	    var splitPairs = new Array();
	    splitPairs = splitElements[x].split(':');

	    outputElements += outputElement(splitPairs[0], splitPairs[1], splitPairs[2]);

	  }

	  $('elementlist').innerHTML = outputElements;
	  Behaviour.apply();
	} else {
	  $('elementlist').innerHTML = '';
	  Behaviour.apply();
	}

    }

    function outputElement(elementNumber, elementDescription, customElement, classNames, defaults, icon, overrideTitle) {

	var elementDef = customElement ? 'cdef:' : 'def:';
	var imageNumber= customElement ? customElement : elementNumber;

	elementDescription = overrideTitle ? overrideTitle : elementDescription;

	var elementsFolder = 'elements';
	var elementsFileType = 'gif';
	var elementsBorder = 'none';
	var elementsWidth = '60px';
	classNames = classNames ? "outside_top controlledDrop "+classNames : "outside_top";

	if (siteType == 'myspace') {
	  elementsFolder = 'newelements';
	  elementsFileType = 'gif';
	  elementsBorder = 'none';
	  elementsWidth = '60px';
	}

    var iconFile = 'e'+imageNumber+'.'+elementsFileType;
    if(icon){
        iconFile = 'e'+imageNumber+'_'+icon+'.'+elementsFileType
    }

	var myOutput = '<li class="'+classNames+'" title="'+/*tl(*/'Drag to page"'/*)tl*/+' style="width: 80px;">';
    if(defaults){
        myOutput += '  <form name="id_'+elementNumber+'" autocomplete="off"><input type="hidden" name="idfield" value="' + elementDef + elementNumber + '|'+defaults.replace(/"/g, "'")+'" /></form>';
    }
    else{
        myOutput += '  <form name="id_'+elementNumber+'" autocomplete="off"><input type="hidden" name="idfield" value="' + elementDef + elementNumber + '" /></form>';
    }
	myOutput += '  <img src="http://'+editorStatic+'/weebly/images/'+elementsFolder+'/'+iconFile+'" style="border: '+elementsBorder+'; width: '+elementsWidth+'; height: '+elementsWidth+';"><br/>';
	myOutput += '  ' + elementDescription;
    if(Weebly.Restrictions.requiresUpgrade(elementNumber)){
        var service = Weebly.Restrictions.requiredService(elementNumber);
        var serviceOverlay = Weebly.Restrictions.accessValue(service+'_element_overlay');
        var overlaySrc = typeof(proElementOverlaySrc) != 'undefined' ? proElementOverlaySrc : 'http://'+editorStatic+'/weebly/images/pro-element-overlay.png';
        if(typeof(serviceOverlay) == 'string' && serviceOverlay.length > 0){
            overlaySrc = serviceOverlay;
        }
        myOutput += '<img src="'+overlaySrc+'" class="pro-element-overlay">';
    }
	myOutput += '</li>';

	return myOutput;

    }

    function activeElementCategory(activeCategory, categoryLevel) {

	var categoryEl = $('elements_'+categoryLevel);

	$$('#elements_'+categoryLevel+' a').each( function(el) {
          el.className = "elements_category";
	});

        if(activeCategory != 'none') { $(activeCategory).className = "elements_category_selected"; }

    }

    function activeTab(activePage) {

	var Tabs = ['weebly_tab_edit', 'weebly_tab_pages', 'weebly_tab_themes', 'weebly_tab_settings'];

	$('focusMe').focus();

	for(var i = 0; i < Tabs.length; i++) {
          $(Tabs[i]).className = "weebly-notcurrent";
        }

	$(activePage).className = "weebly-current";

    }

    function activeContainer(activePage) {

	var Areas = ['elements', 'pages', 'themes','settings'];

        for(var i = 0; i < Areas.length; i++) {
          Element.hide($(Areas[i]+"_container"));
        }

	Element.show(activePage+"_container");

	if (activePage == "settings" || activePage == "pages") {
	  $('elements').style.height = '35px';
	  $('placeholderDiv').style.height = '35px';
	  $('scroll_container').style.marginTop = '35px';
	  $('scroll_container').style.height = (getInnerHeight() - 35) + "px";
          $('grayedOut').style.top = '35px';
          $('grayedOut').style.height = (getInnerHeight() - 35) + "px";
	  $('icontent_container').style.minHeight = (getInnerHeight() - 35)+'px';
          $('scroll_container_properties').style.top = '35px';
          $('scroll_container_properties').style.height = (getInnerHeight() - 35) + "px";

	} else {
          $('elements').style.height = '131px';
	  $('placeholderDiv').style.height = '133px';
          $('scroll_container').style.marginTop = '133px';
	  $('scroll_container').style.height = (getInnerHeight() - 133) + "px";
          $('grayedOut').style.top = '133px';
          $('grayedOut').style.height = (getInnerHeight() - 133) + "px";
	  $('icontent_container').style.minHeight = (getInnerHeight() - 133)+'px';
          $('scroll_container_properties').style.top = '133px';
          $('scroll_container_properties').style.height = (getInnerHeight() - 133) + "px";
	}

	if (activePage != "themes") {
	  Element.hide($('themePictures'));
	  Element.hide($('customizeTheme'));
	  Element.hide($('themeSettings'));
	  Element.hide($('themesShowThemes'));
	  if (ENABLE_THEME_BROWSER) {
		Element.hide($('theme-action-tabs'));
	  }else{
		Element.hide($('themeCategories'));
	  }
	  Element.hide($('themesForward'));
	  Element.hide($('themesBack'));
	  $('themePictures').style.left = "0px";
	} else {

	  if (siteType == 'myspace') {
	    showThemeOptions(1);
	  } else {

	    Element.show($('themePictures'));
	    Element.show($('customizeTheme'));
	    if (ENABLE_THEME_BROWSER) {
	        Element.show($('theme-action-tabs'));
	    }else{
	        Element.show($('themeCategories'));
	    }
	    Element.show($('themesForward'));

	  }

	}

        if (activePage == "elements") {
          showEvent('tab_edit');
        } else if (activePage == "pages") {
          showEvent('tab_pages');
        } else if (activePage == "themes") {
          showEvent('tab_themes');
        } else if (activePage == "settings") {
          showEvent('tab_settings');
        }


    }

    function selectThemeConfigCategory(activeCategory) {

        var categoriesLevel1 = ['tc_popular', 'tc_colors', 'tc_text_styles', 'tc_layout'];

	$('focusMe').focus();

        for(var i = 0; i < categoriesLevel1.length; i++) {
          $(categoriesLevel1[i]).className = "elements_category";
        }

        if(activeCategory != 'none') { $(activeCategory).className = "elements_category_selected"; }

	var themeChildNodes = $('themeOptions').childNodes;
	for (var x=0; x<themeChildNodes.length; x++) {
	  if (!Element.hasClassName(themeChildNodes[x],activeCategory.replace(/^tc_/,''))) {
	    themeChildNodes[x].style.display = 'none';
	  } else {
	    themeChildNodes[x].style.display = 'block';

	    // Adjust scroll on dropdowns
	    var dropdowns = document.getElementsByClassName('weeblyDropDown', themeChildNodes[x]);
	    if (dropdowns.length > 0) {
	      var tmpId = dropdowns[0].firstChild.id.replace(/[^0-9]/g, "");
	      Weebly.DropDowns.dropdownsRef[tmpId].adjustScroll();
	    }
	  }
	}

    }

    /*** Utility Functions ***/
/*******************************/

  shortcut.add("Ctrl+B", function() { runCommand(currentBox, 'Bold', null); });
  shortcut.add("Ctrl+U", function() { runCommand(currentBox, 'Underline', null); });
  shortcut.add("Ctrl+I", function() { runCommand(currentBox, 'Italic', null); });
  function showAbout() {

        showTip("Thanks for using Weebly!", $('weebly_tab_edit'), 'y', '101');
        setTimeout("hideTip('tip101'); showTip('Weebly was created by David Rusenko, Dan Veltri, and Chris Fanini.', $('weebly_tab_themes'), 'y', '102');", 4000);
        setTimeout("hideTip('tip102'); showTip('We are!', $('weebly_tab_pages'), 'y', '103');", 8000);
        setTimeout("hideTip('tip103'); showTip('Penn State!<br/><br/><img src=\"http://rusenko.weebly.com/drusenko/pennstate.jpg\" width=\"300\" height=\"220\"/>', $('weebly_tab_settings'), 'w', '104');", 14000);
        setTimeout("hideTip('tip104');", 20000);

  }

    function showPublishingAnimation() {

	publishingAnim = 1;
	publishingAnimation();

        var newHeight = getInnerHeight();

	//Element.setStyle($('publishingWait'), {top: (Position.realOffset(window)[1]+40)+'px'});
	//Element.setStyle($('grayedOut'), {height:(newHeight-35)+'px', display:'block', visibility:'visible'} );

	//Effect.Center('publishingWait');
	//load the publising window
	
	$('chooseAddress1').src = 'http://'+editorStatic+/*tli(*/"/weebly/images/publishing-site.jpg"/*)tli*/;
    if(Weebly.Restrictions.hasAccess('ftp_publish')){
        $('exportText').innerHTML = "<div style=\"text-align: center;\"><img src=\"http://"+editorStatic+"/weebly/images/ajax-loader.gif\"/><br/><br/>"+/*tl(*/"Please be patient. It may take a few minutes to update your site."/*)tl*/+"</div><br/><br/></div>";
    }
    else{
        $('exportText').innerHTML = "<div style=\"text-align: center;\"><img src=\"http://"+editorStatic+"/weebly/images/ajax-loader.gif\"/><br/><br/>"+/*tl(*/"Please wait while we publish your site."/*)tl*/+"</div><br/><br/></div>";
    }
	Element.show('tip14');
/**
	if (settingAnimations == 1) {
          Effect.Appear($('publishingWait'), { duration: 0.5 });
        } else {
          Element.setStyle($('publishingWait'), {display:'block', opacity:'1', visibility:'visible'} );
        }
**/
	

    }
    function hidePublishingAnimation() {

       publishingAnim = 0;

       if (settingAnimations == 1) {
         window.setTimeout("Effect.Fade('publishingWait')", 1000);
         window.setTimeout("Effect.Fade('grayedOut')", 2200);
       } else {
         Element.hide('grayedOut');
         Element.hide('publishingWait');
       }

    }
    function publishingAnimation() {

	var moveX; var moveY;

	if (publishingLoc == 'left') { 
	  publishingLoc = 'right';
	  for(x=1; x<11; x++) {
	    moveX = 30; moveY = 150;
	    setTimeout("new Effect.Move('publishing"+x+"', { x: "+moveX+", y: "+moveY+", mode: 'absolute' })", 100*x);
	  }
	} else {
	  publishingLoc = 'left';
          for(x=1; x<11; x++) {
            moveX = 470; moveY = 150;
            setTimeout("new Effect.Move('publishing"+x+"', { x: "+moveX+", y: "+moveY+", mode: 'absolute' })", 100*x);
	  }
	}

	if (publishingAnim == 1) { setTimeout("publishingAnimation()", 2000); }


    }

    function expandAdvanced() {

	$("propertiesAdvancedHeader").innerHTML = "<br/><h3><a href=\"#\" onClick=\"collapseAdvanced();\" style=\"color: black;\"><img border=\"0\" src=\"http://"+editorStatic+"/weebly/images/arrow_down.gif\"/> "+/*tl(*/"Advanced Properties"/*)tl*/+"</a></h3>";
	Element.setStyle('propertiesAdvanced', {display:'block'} );

    }
    function collapseAdvanced() {

        Element.setStyle('propertiesAdvanced', {display:'none'} );
	$("propertiesAdvancedHeader").innerHTML = "<br/><h3><a href=\"#\" onClick=\"expandAdvanced();\" style=\"color: black;\"><img border=\"0\" src=\"http://"+editorStatic+"/weebly/images/arrow_right.gif\"/> "+/*tl(*/"Advanced Properties"/*)tl*/+"</a></h3>";

    }
 
    function showProperties(topPos) {

      topPos = topPos ? topPos : 145;

      $('textEditor').style.top = topPos+'px';
      //Element.setStyle('textEditor', { top:Position.realOffset(window)[1]+topPos+'px'} );

      //$('scroll_container').style.overflowY = 'hidden';
      Element.show('scroll_container_properties');

      if (settingAnimations == 1) {
         window.setTimeout("Effect.Appear('textEditor')", 1000);
      } else {
         Element.show('textEditor');
      }


    }

    function fadeProperties() {

       //$('scroll_container').style.overflowY = 'scroll'; 
       Weebly.Linker.close();
       Element.hide('scroll_container_properties');

    }

    function fadeMain() {

      Element.setStyle('grayedOut', {height:(getInnerHeight()-35)+'px'} );

      if (settingAnimations == 1) {
        Effect.Appear('grayedOut');
      } else {
        Element.show('grayedOut');
      }

    }

    function showMain() {

       if (settingAnimations == 1) {
         setTimeout("Effect.Fade('grayedOut')", 1000);
	 Element.setStyle('grayedOut', { visibility: 'visible'});
       } else {
         Element.hide('grayedOut');
       }

    }

    function displayThemes() {

      Element.setStyle('themes', {top:Position.realOffset(window)[1]+30+'px'} );

       if (settingAnimations == 1) {
         Effect.Appear('themes');
	 Effect.Appear('themeChoices', { duration: 0.5 });
       } else {
         Element.show('themes');
	 Element.show('themeChoices');
       }

      currentThemesPage = 1;

    }

    function fadeThemes() {

       if (settingAnimations == 1) {
         window.setTimeout("Effect.Fade('themes')", 1000);
       } else {
         Element.hide('themes');
       }

       Element.setStyle('nextThemes', {display:'block'} );
       Element.setStyle('previousThemes', {display:'none'} );

       currentThemesPage = 0;

    }

    function selectHeader(go) {

	if (!go) { setTimeout("selectHeader(1)", 50); }
	else {
	  if (!headerSelected) {
	    var headerNode = document.getElementsByClassName('weebly_header')[0];

	    headerDimensions = [Element.getStyle(headerNode, 'height').replace(/px/,''), Element.getStyle(headerNode, 'width').replace(/px/,'')];
	    headerNode.style.height = (headerDimensions[0]-2) + "px";
	    headerNode.style.width  = (headerDimensions[1]-2) + "px";
	    headerNode.style.border = "1px dashed #4455aa";

	    selectUpload("header", "header:"+currentTheme+"-resize");

            var menuBarDivHTML = "<table id='menuBarItemContainer' spacing=0 padding=0><tr>";
            menuBarDivHTML += "<td style='position: relative; background: none;'><div id='menubar-l' style='position: absolute; width: 5px; height: 44px; top: 0px; margin-left: -10px; background: url(http://"+editorStatic+"/weebly/images/menubar-l.gif) no-repeat bottom left;'></div> </td>";

	    menuBarDivHTML += "<td><span class='menuBarSpan'><b>"+/*tl(*/"New Header Image"/*)tl*/+"</b><a id='newImage' href='#' onClick='selectUpload(\"header\", \"header:"+currentTheme+"-resize\"); return false;' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload"/*)tl*/+"</a></span></td>";
	    if(currentHeader && currentHeader.match(/\./)) {
	      menuBarDivHTML += "<td><span class='menuBarSpan'><b>Header Image Size</b><div style=\"font-size: 12px; background: none; padding-top: 4px; display: block;\"><b>"+(headerNode.style.width.replace("px", "")-(-2)+"px")+" x "+(headerNode.style.height.replace("px", "")-(-2)+"px")+"</b></div></span></td>";
	      menuBarDivHTML += "<td style='background: none;'><span class='menuBarSpan'><b>"+/*tl(*/"Remove Header Image"/*)tl*/+"</b><a id='removeImage' class='menuIconLink' href='#' onClick='removeHeader(); return false;'><img src='http://"+editorStatic+"/weebly/images/action_stop.gif' class='menuIconImage' style='margin-right: 2px; top: 4px;' alt='New Image Icon'/> "+/*tl(*/"Remove"/*)tl*/+"</a></span></td>";
	    } else {
	      menuBarDivHTML += "<td style='background: none;'><span class='menuBarSpan'><b>Header Image Size</b><div style=\"font-size: 12px; background: none; padding-top: 4px; display: block;\"><b>"+(headerNode.style.width.replace("px", "")-(-2)+"px")+" x "+(headerNode.style.height.replace("px", "")-(-2)+"px")+"</b></div></span></td>";
	    }
	    menuBarDivHTML += "<td style='position: relative; background: none;'><div id='menubar-r' style='position: absolute; width: 5px; height: 44px; top: 0px; margin-left: 10px; background: url(http://"+editorStatic+"/weebly/images/menubar-r.gif) no-repeat bottom left;'></div> </td>";
	    menuBarDivHTML += "</tr></table>";

	    $("menuBarDiv").innerHTML = menuBarDivHTML;

	    Weebly.Elements.showMenuBar();
	    headerSelected = 1;
	  } else if (headerSelected == 2) {
	    headerSelected = null;
	  }
	}

    }

    function unselectHeader(preempt) {
	
	if (headerSelected) {
	  var headerNode = document.getElementsByClassName('weebly_header')[0];
	  if (headerNode && headerNode.style && headerDimensions && headerDimensions[0]) { 
	    headerNode.style.border = 'none';
	    headerNode.style.height = (headerDimensions[0]) + "px";
	    headerNode.style.width  = (headerDimensions[1]) + "px";
	    headerDimensions 	  = Array();	    
	  }

	  hideFlashContainer();

	  Weebly.Elements.hideMenuBar();
	  $('menuBarDiv').innerHTML = '';
	  headerSelected = null;
	}
	if (preempt) { headerSelected = 2; }

    }

    function removeHeader() {
	unselectHeader();
	new Ajax.Request(ajax, {parameters:'pos=removeheader&cookie='+document.cookie, 'onSuccess' : handlerRemoveHeader, 'onFailure':errFunc});
	currentHeader = null;
    }

    function handlerRemoveHeader(t) {
	if (t.responseText == 1) {
	  currentStyleNum = Math.floor(Math.random()*10000000001);
	  writeTheme(currentTheme);
	}
    }

    function editCustomHTML(ucfid, cfpid, ucfpid) {

	var ucf 	= $(""+ucfid);
	var ucfParentNode  = ucf.parentNode;
	ucf.style.display = 'none';

	var ucfp 	= $(""+ucfpid+"CustomHTML");
	var ucfpParentNode = ucfp.parentNode;

	// Fill in the text box
	var ucfFrame = $("scriptInclude"+ucfid);
	if (ucfFrame && ucfFrame.contentWindow && ucfFrame.contentWindow.document && ucfFrame.contentWindow.document.getElementsByTagName('body')[0] && ucfFrame.contentWindow.document.getElementsByTagName('body')[0].childNodes[1]) {
	  if (navigator.appVersion.indexOf("MSIE") == -1) {
	    ucfp.value = ucfFrame.contentWindow.document.getElementsByTagName('body')[0].childNodes[1].innerHTML;
	  } else {
	    ucfp.value = ucfFrame.contentWindow.document.getElementsByTagName('body')[0].childNodes[0].innerHTML;
	  }
	}
  if( Weebly.Elements.customHTML[ucfpid] )
  {
    ucfp.value = Weebly.Elements.customHTML[ucfpid];
  }
	
	var dummyNode = document.createElement('div');
	dummyNode.style.display = "none";
	dummyNode.id		= "customHTMLDummy";

	ucfpParentNode.appendChild(dummyNode);
	ucfParentNode.appendChild(ucfp);
	ucfp.style.display = 'block';
	ucfp.focus();
	ucfp.select();

	Weebly.Elements.editing = cfpid;

    }

    function hideCustomHTML(ucfid, cfpid, ucfpid) {

	if (!Weebly.Elements.editing) return;
	Weebly.Elements.editing = null;

        var ucf         = $(""+ucfid);
        var ucfpParentNode  = $('customHTMLDummy').parentNode;

        var ucfp        = $(""+ucfpid+"CustomHTML");
        var ucfParentNode = ucfp.parentNode;

        ucfp.style.display = 'none';
        ucf.style.display = 'block';
        ucfpParentNode.appendChild(ucfp);
        ucfpParentNode.removeChild($('customHTMLDummy'));

	//ucfp.value = ucfp.value.replace(/<!--.*?-->/ig, "");

	new Ajax.Request(ajax, {parameters:'pos=savecustomhtml&ucfid='+ucfid+'&ucfpid='+ucfpid+'&customhtml='+encodeURIComponent(ucfp.value)+'&cookie='+document.cookie, onSuccess:handlerHideCustomHTML, onFailure:errFunc, bgRequest: true});
  
  //ucfp.value = ucfp.value.escapeHTML();

	Weebly.Elements.customHTML[ucfpid] = ucfp.value;
	Weebly.Elements.onChange(ucfpid, cfpid);

    }

    function handlerHideCustomHTML(t) {
    }

    function hideElement(elementID) {
	Element.hide(elementID);
    }

    function showElement(elementID) {
	Element.setStyle(elementID, {display:'block', visibility:'visible'} );
    }

    function validateOK(element_id) {

	Weebly.Elements.continueOnChange();
	//saveProperties('properties',element_id);

    }

    function validateNoGo(errorMsg) {

	//Define later
    }

    function contentDecode(str) {

	str = str.replace(new RegExp('\\+','g'),' ');
    	return unescape(str);

    }

    function contentEncode(str) {

    	str = escape(str);
    	str = str.replace(new RegExp('\\+','g'),'%2B');
    	return str.replace(new RegExp('%20','g'),'+');

    }

    function showUploader(size) {

	Element.hide('uploadSize');
	Element.show('imageUploader');
	imageUploadSize = size;

    }

    function getScrollTop() {

	var y;
	if (self.pageYOffset) // all except Explorer
	{
	  y = self.pageYOffset;
	}
	else if (document.documentElement && document.documentElement.scrollTop)
	// Explorer 6 Strict
	{
	  y = document.documentElement.scrollTop;
	}
	else if (document.body) // all other Explorers
	{
	  y = document.body.scrollTop;
	}

	return y;

    }

  function getThemeConfig(firstTime) {

	new Ajax.Request(ajax, {parameters: 'pos=getthemeconfig&theme='+currentTheme+'&cookie='+document.cookie, 'onSuccess': function(t) { handlerGetThemeConfig(t, firstTime); }, 'onFailure':errFunc});
	
  }

  function handlerGetThemeConfig(t, firstTime) {

	$('currentThemeOptions').innerHTML = t.responseText;
	selectThemeConfigCategory('tc_popular');

	// Create dropdowns here
        var elementList = document.getElementsByClassName('dropDownData', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
	  var el = elementList[x];
	  var updateFunc = function() { };
	  if ( $(el.nextSibling.name+"-updateFunction") && $(el.nextSibling.name+"-updateFunction").value) {
	    eval("updateFunc = "+$(el.nextSibling.name+"-updateFunction").value+";");
	  }
	  new Weebly.DropDown(el.nextSibling, {openWidth: 95, rowMargin: '3px 0 0 0', rowFunction: function(x) { return generateDropDownRow(x, el.innerHTML); }, noRefresh: 1, onClose: saveThemeConfig, updateFunction: updateFunc});
	}

	// Create font drop down
        var elementList = document.getElementsByClassName('fontDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 150, rowMargin: '0px 0 0 0', rowFunction: generateFontsWebsafeRows, noRefresh: 1, onClose: saveThemeConfig, rowHoverColor: '#FFFFFF'});
        }

        // Create border drop down
        var elementList = document.getElementsByClassName('borderDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 279, openHeight: 320, height: 70, width: 105, marginRight: 25, scaleBy: 40, rowMargin: '5px 0 0 2px', rowFunction: generateBorderStyleRows, onClose: saveThemeConfig, rowHoverColor: '#FFFFFF', overflowY: 'scroll'});
        }

        // Create center drop down
        var elementList = document.getElementsByClassName('centerDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 279, height: 70, width: 105, marginRight: 25, scaleBy: 40, rowMargin: '5px 0 0 2px', rowFunction: generateCenterStyleRows, onClose: saveThemeConfig, rowHoverColor: '#FFFFFF', overflowY: 'scroll'});
        }

        // Create color drop down
        var elementList = document.getElementsByClassName('colorDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 220, openHeight: 123, generateValueFunction: colorValueFunction, generateContentsFunction: colorContentsFunction, rowMargin: '0px 0 0 0', onClose: saveThemeConfig, overflowY: 'scroll'});
        }

        // Create image drop down
        var elementList = document.getElementsByClassName('imageDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 540, openHeight: 330, height: 70, width: 105, marginRight: 25, generateValueFunction: imageValueFunction, generateContentsFunction: imageContentsFunction, rowMargin: '0px 0 0 0', onOpen: function() { if (lastTab == "imgSearch") { searchImages(1); } else { selectImageTab($("imgPattern")); } }, onClose: saveThemeConfig, overflowY: 'auto', scaleBy: 60, rowHoverColor: '#FFFFFF', updateFunction: function(val) { $('icontent_container').style.backgroundImage = $('%%BGIMAGE%%').nextSibling.childNodes[1].firstChild.firstChild.style.backgroundImage.replace(/_s\./, "."); }, showTabs: {'search': 1, 'patterns': 1, 'upload': 1, 'remove': 1} });
        }

        // Create image upload drop down
        var elementList = document.getElementsByClassName('imageUploadDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
          new Weebly.DropDown(el.nextSibling, {openWidth: 340, openHeight: 330, height: 70, width: 105, marginRight: 25, generateValueFunction: imageValueFunction, generateContentsFunction: imageContentsFunction, rowMargin: '0px 0 0 0', onOpen: function() { selectImageTab($("imgUpload")); }, onClose: saveThemeConfig, overflowY: 'auto', scaleBy: 60, rowHoverColor: '#FFFFFF', updateFunction: function(val) { $('icontent_container').style.backgroundImage = $('%%BGIMAGE%%').nextSibling.childNodes[1].firstChild.firstChild.style.backgroundImage.replace(/_s\./, "."); }, showTabs: {'upload': 1, 'del': 1} });
        }

	// Dispose of old sliders
	for (var slider in Weebly.sliders) {
	  Weebly.sliders[slider].dispose();
	}

	// Create new sliders
	var elementList = document.getElementsByClassName('sliderData', $('themeOptions'));
	Weebly.sliders = {};
        for (var x=0; x < elementList.length; x++) {
          var el = elementList[x];
	  var data = {};
	  eval("data = "+el.innerHTML+";");
	  var updateFunc = function() { };
          if ( $(el.nextSibling.name+"-updateFunction") && $(el.nextSibling.name+"-updateFunction").value) {
            eval("updateFunc = "+$(el.nextSibling.name+"-updateFunction").value+";");
          }
	  var updateEl = el.nextSibling.name;
	  var newSlider = new Control.Slider($(el.nextSibling.name+'-handle'),$(el.nextSibling.name+'-track'), { element: updateEl, updateFunction: updateFunc, startValue: data.startValue, endValue: data.endValue, sliderValue: ($(el.nextSibling).value-data.startValue)/(data.endValue-data.startValue), onSlide: function(value) { value = value*(this.endValue-this.startValue)-(-this.startValue); $(this.element).value = value; this.updateFunction(value); }, onChange: function() { saveThemeConfig(2); } });
	  Weebly.sliders[$(el.nextSibling.name+'-handle')] = newSlider;
	  
        }

	// Create palette drop down
	new Weebly.DropDown($('weeblyPalette'), {width: 165, openWidth: 165, openHeight: 128, marginRight: 10, rowFunction: generatePaletteRows, rowMargin: '0 0 0 1px', onClose: saveThemeConfig, updateFunction: paletteUpdateFunction, noInitialUpdate: true });

	if (firstTime) { saveThemeConfig(0,1); }

	/**
	if (t.responseText.match("This theme is not configurable.")) {
	  $('currentThemeOptions').innerHTML = t.responseText;
	  return;
	}

	var themeRoot = getXML(t.responseText);
	var optionsRoot = themeRoot.getElementsByTagName('theme')[0].getElementsByTagName('options')[0];

	$('currentThemeOptions').innerHTML = '';
	for (var x=0; x<optionsRoot.childNodes.length; x++) {
	  var currentNode = optionsRoot.childNodes[x];
	  if (currentNode.tagName == "option") {
	    $('currentThemeOptions').innerHTML += "<div style='float: left; height: 40px; font-size: 11px; font-weight: bold; min-width: 105px; width: auto !important; width: 105px; padding: 5px 5px 0 5px;'>" + getValueXML(currentNode,'name') + "<br/><i style='font-weight: normal;'>" + getValueXML(currentNode,'property') + "</i></div>";
	  }
	}
	**/

	showThemeOptions(); 
	setTimeout("showEvent('showThemeOptions', 0, $('weeblyPalette').nextSibling.childNodes[1]);", 1500);

  }

  function paletteUpdateFunction(val) {

	currentPalette = val; 
	var elementList = document.getElementsByClassName('colorDropDown', $('themeOptions')); 
	for (var x=0; x < elementList.length; x++) { 
	  if (elementList[x].nextSibling.id == "%%BGCOLOR%%") {
	    elementList[x].nextSibling.value = "#1";
          } else if (elementList[x].nextSibling.id == "%%CENTERBORDERCOLOR%%") {
            elementList[x].nextSibling.value = "#2";
          } else if (elementList[x].nextSibling.id == "%%HEADERCOLOR%%") {
            elementList[x].nextSibling.value = "#2";
	  } else if (elementList[x].nextSibling.id == "%%PAGECOLOR%%") {
            elementList[x].nextSibling.value = "#3";
          } else if (elementList[x].nextSibling.id == "%%BOXCOLOR%%") {
            elementList[x].nextSibling.value = "#4";
          } else if (elementList[x].nextSibling.id == "%%BORDERCOLOR%%") {
            elementList[x].nextSibling.value = "#5";
          } else if (elementList[x].nextSibling.id == "%%LEAVESCOLOR%%") {
            elementList[x].nextSibling.value = "#6";
          } else if (elementList[x].nextSibling.id == "%%CONTACTCOLOR%%") {
            elementList[x].nextSibling.value = "#7";
          } else if (elementList[x].nextSibling.id == "%%FONTCOLOR%%") {
            elementList[x].nextSibling.value = "#8";
	  }
	  var thisDropDown = Weebly.DropDowns.dropdownsRef[elementList[x].nextSibling.nextSibling.firstChild.id.replace(/[^0-9]/g, '')]; 
	  $(thisDropDown.id+"-value").innerHTML = thisDropDown.options.generateValueFunction(thisDropDown); 
	  thisDropDown.options.generateContentsFunction(thisDropDown); 
	}

  }

  function colorValueFunction(obj) {

	var curValue = obj.formEl.value;
	if (!curValue.match(/^#[a-fA-F0-9]{6}$/)) {
	  curValue = colorPalettes[currentPalette][curValue.replace(/[^0-9]/, '')-1];
	}
	return "<div style='height: 21px; width: 74px; margin: 1px; background: "+curValue+";'></div>";

  }

  function colorContentsFunction(obj) {

	var returnHTML = '<div style="padding: 5px 5px 0 5px;">';
	returnHTML += '<div style="font-size: 11px; font-weight: bold; font-family: arial, verdana, sans-serif; border-bottom: 1px solid #ccc; margin-bottom: 5px;">'+/*tl(*/'Choose a color'/*)tl*/+':</div>';
	thisPalette = colorPalettes[currentPalette];
	for (var y in thisPalette) {
	  returnHTML += '<div style="margin: 3px; height: 17px; width: 17px; font-size: 1px; background: '+thisPalette[y]+' url(http://'+editorStatic+'/weebly/images/palette-color.gif) no-repeat top left; float: left; cursor: pointer;" onclick="Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close(\'#'+(y-(-1))+'\');">&nbsp;</div>';
	}
	
	returnHTML += '<div style="clear: both; font-size: 11px; font-weight: bold; font-family: arial, verdana, sans-serif; border-bottom: 1px solid #ccc; margin-bottom: 5px; padding-top: 20px;">'+/*tl(*/'Or click to select more colors'/*)tl*/+':</div>';

	var curValue = obj.options.generateValueFunction(obj).replace(/.*background: #(.{6});.*/, "$1");
	returnHTML += '<div id="'+obj.id+'-colorpicker" style="margin: 5px 5px 5px 0; border: 1px solid #ccc; height: 20px; cursor: pointer;"></div>';
	returnHTML += '</div>';
	returnHTML += '<input type="hidden" id="'+obj.id+'-currentcolor" value="'+curValue+'"/>';
	returnHTML += '<input type="hidden" id="'+obj.id+'-initialcolor" value="'+curValue+'"/>';

	return returnHTML;

  }

  function imageValueFunction(obj) {

	var curValue = obj.formEl.value;
	if (curValue.match(/^pattern:/)) {
	  curValue = "%%THEMEIMG%%/bga.png?"+Math.floor(Math.random()*10000000001);
	}
	if (curValue.match(/jpe?g$/)) {
	  curValue = curValue.replace(/\.(jpe?g)$/, "_s.$1");
	} else {
	  curValue = curValue.replace(/%%THEMEIMG%%/, "http://www.weebly.com/weebly/render/users/"+userIDLocation+"/"+currentSite+"/");
	}
        return "<div style='height: 68px; width: 103px; margin: 1px; background: url("+curValue+");'></div>"

  }

  var lastSearch = 'forest night';
  var lastPatternsPage = 0;
  var lastTab = 'imgPattern';
  function imageContentsFunction(obj) {

	var returnHTML = '<div style="padding: 5px;">';
	if (obj.options.showTabs.search) {
	  returnHTML += '<div id="imgSearch" style="cursor: pointer; font-size: 10px; border-left: 1px solid #E9E9E9; border-right: 1px solid #E9E9E9; border-top: 2px solid #FF9900; padding: 2px 7px; float: left;" onclick="selectImageTab(this);">Image Search</div>';
	}
	if (obj.options.showTabs.patterns) {
	  returnHTML += '<div id="imgPattern" style="cursor: pointer; font-size: 10px; border-left: 1px solid #E9E9E9; border-right: 1px solid #FFFFFF; border-top: 2px solid #E9E9E9; background: #E9E9E9; padding: 2px 7px; float: left;" onclick="selectImageTab(this);">Pattern</div>';
	}
	if (obj.options.showTabs.upload) {
	  returnHTML += '<div id="imgUpload" style="cursor: pointer; font-size: 10px; border-left: 1px solid #E9E9E9; border-right: 1px solid #FFFFFF; border-top: 2px solid #E9E9E9; background: #E9E9E9; padding: 2px 7px; float: left;" onclick="selectImageTab(this);">Upload</div>';
	}
	if (obj.options.showTabs.remove || obj.options.showTabs.del) {
	  returnHTML += '<div id="imgNone" style="cursor: pointer; font-size: 10px; border-left: 1px solid #E9E9E9; border-right: 1px solid #FFFFFF; border-top: 2px solid #E9E9E9; background: #E9E9E9; padding: 2px 7px; float: left;" onclick="selectImageTab(this);">None</div>';
	}
	returnHTML += '<div style="clear: both; border-bottom: 1px solid #E9E9E9; height: 3px; overflow: hidden;"></div>';

	// Image search DIV
	if (obj.options.showTabs.search) {
	  returnHTML += '<div id="imgSearchBox">';
	  returnHTML += '<div style="margin-top: 5px; font-size: 11px; font-weight: bold; font-family: arial, verdana, sans-serif; border-bottom: 1px solid #E9E9E9; margin-bottom: 5px;">Search for an image:</div>';
	  returnHTML += '<input type="text" style="border: 1px solid #BCCDF0; padding: 3px;" id="imageSearchBox" value="'+lastSearch+'"/><input type="submit" style="padding: 2px; margin-left: 10px; margin-right: 10px;" value="Search" onclick="searchImages(1); return false;"/><span id="imageSearchError" style="color: red;"></span>';
	  returnHTML += '<div style="height: 300px;" id="imageSearchReturn"></div>';
	  returnHTML += '</div>';
	}

        // Pattern search DIV
	if (obj.options.showTabs.patterns) {
          returnHTML += '<div id="imgPatternBox" style="display: none;">';
          returnHTML += '<div style="margin-top: 5px; font-size: 11px; font-weight: bold; font-family: arial, verdana, sans-serif; border-bottom: 1px solid #E9E9E9; margin-bottom: 5px;">Select a pattern:</div>';
	  returnHTML += '<div style="height: 300px;" id="imagePatternReturn"></div>';
          returnHTML += '</div>';
	}

        // Upload Image DIV
	if (obj.options.showTabs.upload) {
	  var display = (!obj.options.showTabs.search && !obj.options.showTabs.patterns) ? 'block' : 'none';
          returnHTML += '<div id="imgUploadBox" style="display: '+display+';">';
          returnHTML += '<div style="margin-top: 5px; font-size: 11px; font-weight: bold; font-family: arial, verdana, sans-serif; border-bottom: 1px solid #E9E9E9; margin-bottom: 5px;">Upload an image:</div>';
          returnHTML += '<input type="text" style="border: 1px solid #BCCDF0; padding: 3px;" id="imageSearchBox"/><input type="submit" style="padding: 2px; margin-left: 10px; margin-right: 10px;" value="Browse" onclick="selectUpload(\'bgImageUpload\', 2048, 0, 0); return false;"/><span id="imageSearchError" style="color: red;"></span>';
	  returnHTML += '<ul id="notificationsBgImage"></ul>';
          returnHTML += '</div>';
	}

        // No Image DIV
	if (obj.options.showTabs.remove) {
          returnHTML += '<div id="imgNoneBox" style="display: none;">';
          returnHTML += '<a style="display: block; margin: 15px 5px; font-size: 12px; font-weight: bold; font-family: arial, verdana, sans-serif;" href="#" onclick="Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close(\'%%THEMEIMG%%/bga.png\'); return false;"><img src="http://'+editorStatic+'/weebly/images/action_stop.gif" style="position: relative; top: 3px; border: none;"/> Remove background image</a>';
          returnHTML += '</div>';
	}

        // No Image DIV
        if (obj.options.showTabs.del) {
          returnHTML += '<div id="imgNoneBox" style="display: none;">';
          returnHTML += '<a style="display: block; margin: 15px 5px; font-size: 12px; font-weight: bold; font-family: arial, verdana, sans-serif;" href="#" onclick="Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close(\'\'); return false;"><img src="http://'+editorStatic+'/weebly/images/action_stop.gif" style="position: relative; top: 3px; border: none;"/> Remove image</a>';
          returnHTML += '</div>';
        }
	
	returnHTML += '</div>';

	return returnHTML;

  }

  function selectImageTab(tabName) {

	var imageTabDivs = ['imgSearch', 'imgPattern', 'imgUpload', 'imgNone'];

	for (var x=0; x<imageTabDivs.length; x++) {
	  if ($(imageTabDivs[x])) {
	    $(imageTabDivs[x]).style.background = "#E9E9E9";
	    $(imageTabDivs[x]).style.borderTop = "1px solid #E9E9E9";
	    $(imageTabDivs[x]).style.borderRight = "1px solid #FFFFFF";
	    $(imageTabDivs[x]+"Box").style.display = "none";
	  }
	}

	tabName = tabName.id;

	$(tabName+"Box").style.display = "block";
	$(tabName).style.background = "#FFFFFF";
	$(tabName).style.borderTop = "2px solid #FF9900";
	$(tabName).style.borderRight = "1px solid #E9E9E9";

	if (tabName == 'imgPattern') {
	  populatePatterns(lastPatternsPage);
	  lastTab = 'imgPattern';
	} else {
	  lastTab = 'imgSearch';
	}

  }

  function populatePatterns(start) {

	var myRet = '';

	for (var x=start; x<start+6; x++) {
	  
	  myRet += "<a href='#' onclick='Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close(\"pattern:"+themePatterns[x]+"\"); return false;' style='text-align: center; float: left; width: 160px; display: block; margin: 5px;'><div style='border: none; height: 123px; width: 165px; background: url(\"/weebly/themes/myspace-patterns/"+themePatterns[x]+"/"+themePatterns[x]+"\");'></div></a>\n";
	  if (x%3 == 0 && x != 0) {
	    //myRet += "<div style='width: 0px height: 0px; overflow: hidden; clear: both;'></div>";
	  }

	}

	myRet += "<div style='width: 0px height: 0px; overflow: hidden; clear: both;'></div>";
	myRet += "<div style='text-align: right; margin-right: 15px;'>";
	
	if (start != 0) {
	  myRet += "<a href='#' onclick='populatePatterns("+(start-6)+"); return false'>&lt; Back</a> | ";
	}
	if (start-(-6) < themePatterns.length) {
	  myRet += "<a href='#' onclick='populatePatterns("+(start-(-6))+"); return false'> More &gt;</a>";
	}

	myRet += "</div>";

	$('imagePatternReturn').innerHTML = myRet;
	lastPatternsPage = start;

  }

  function searchImages(start) {

	$("imageSearchError").innerHTML = "";
	lastSearch = $('imageSearchBox').value;
	new Ajax.Request(ajax, {parameters: 'pos=searchimages&query='+$('imageSearchBox').value+'&start='+start+'&cookie='+document.cookie, 'onSuccess': handlerSearchImages, 'onFailure':errFunc});

  }

  function handlerSearchImages(t) {

	$('imageSearchReturn').innerHTML = t.responseText;

  }

  function keepImage(url, ref) {

	$("imageSearchError").innerHTML = "";
	new Ajax.Request(ajax, {parameters: 'pos=keepimage&url='+encodeURIComponent(url)+'&ref='+encodeURIComponent(ref)+'&cookie='+document.cookie, 'onSuccess': handlerKeepImage, 'onFailure':errFunc});

  }

  function handlerKeepImage(t) {

	if (t.responseText.match(/Error/)) {
	  $('imageSearchError').innerHTML = t.responseText;
	} else {
	  t.responseText = t.responseText.replace(/\n/g, "");
	  t.responseText = t.responseText.replace(/\r/g, "");
	  Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close(t.responseText);
	}

  }

  function setThemeColor(el) {

	//console.log($(el));
	if ($(el).value != $(el).nextSibling.value) {
	  //console.log(Weebly.DropDowns.activeMenu);
	  Weebly.DropDowns.dropdownsRef[Weebly.DropDowns.activeMenu].close('#'+$(el).value);
	}

  }

  function generatePaletteRows(x) {
	
	var i=0;
	for (var z in colorPalettes) { if (colorPalettes.hasOwnProperty(z)) { i++; } }
	if (x+1 > i) { return; }

	var returnVar = '';
	var thisPalette = colorPalettes[x+1];
	for (var y in thisPalette) {
	  if (y>5) { break; }
	  returnVar += '<div style="margin: 3px; height: 17px; width: 17px; font-size: 1px; background: '+thisPalette[y]+' url(http://'+editorStatic+'/weebly/images/palette-color.gif) no-repeat top left; float: left;">&nbsp;</div>';
	}

	return [x+1, returnVar];

  }

  function generateFontsWebsafeRows(x) {

	var i=0;
	for (var y in fontsWebsafe) {
	  if (x == i) { return [y,'<img src="/weebly/images/fonts/'+fontsWebsafe[y]+'" alt="'+y+'"/>']; }
	  i++;
	}

  }

  function generateBorderStyleRows(x) {

	if (x < 6) { 
	  return [x+1,"<div style='height: 80px; width: 270px; overflow: hidden;'><img src='/weebly/themes/myspace-images/thumbnails/borders/"+(x+1)+".jpg' style='border: 0;'/></div>"];
	} else if (x == 6) {
	  return [0,"<div style='height: 80px; width: 270px; overflow: hidden;'><img src='/weebly/themes/myspace-images/thumbnails/borders/none.jpg' style='border: 0;'/></div>"];
	}

  }

  function generateCenterStyleRows(x) {

        if (x < 4) {
          return [x+1,"<div style='height: 80px; width: 270px; overflow: hidden;'><img src='/weebly/themes/myspace-images/thumbnails/center/"+(x+1)+".jpg' style='border: 0;'/></div>"];
        } else if (x == 4) {
          return [0,"<div style='height: 80px; width: 270px; overflow: hidden;'><img src='/weebly/themes/myspace-images/thumbnails/center/none.jpg' style='border: 0;'/></div>"];
        }

  }

  function generateDropDownRow(x, data) {

	eval("data = "+data+";");

	var i=0;
	for (var y in data) {
	  if (x == i) { return [y, data[y]]; }
	  i++;
	}

  }

  function saveThemeConfig(noImageRefresh, firstTime) {

	new Ajax.Request(ajax, {parameters: 'pos=savethemeconfig&firstTime='+firstTime+'&'+Form.serialize('themeOptions')+'&cookie='+document.cookie, 'onSuccess': function(t) { handlerSaveThemeConfig(t, noImageRefresh); }, 'onFailure':errFunc});

  }

  function handlerSaveThemeConfig(t, noImageRefresh) {
	
	if (t.responseText.match('Error')) { return; }

	if (!noImageRefresh) {
	  refreshThemeImages();
	}
	if (noImageRefresh != '2') {
	  //currentStyleNum = Math.floor(Math.random()*10000000001);
	  clearThemeImages();
	  setThemeStyle(currentTheme);
	}

  }

  function clearThemeImages() {

        $('icontent_container').style.backgroundImage = '';
        var elementList = document.getElementsByClassName('updateBackgroundImage', $('icontent'));
        for (var x=0; x < elementList.length; x++) {
          $(elementList[x]).style.backgroundImage = '';
        }

  }

  function refreshThemeImages() {

	if (siteType != 'myspace') { return; }

	currentImageNum = Math.floor(Math.random()*10000000001);
	refreshBackgroundImage('icontent_container');
	var elementList = document.getElementsByClassName('updateBackgroundImage', $('icontent'));
	for (var x=0; x < elementList.length; x++) {
	  refreshBackgroundImage(elementList[x]);
	}

	// Update image setting boxes
        var elementList = document.getElementsByClassName('imageDropDown', $('themeOptions'));
        for (var x=0; x < elementList.length; x++) {
          var thisDropDown = Weebly.DropDowns.dropdownsRef[elementList[x].nextSibling.nextSibling.firstChild.id.replace(/[^0-9]/g, '')];
          $(thisDropDown.id+"-value").innerHTML = thisDropDown.options.generateValueFunction(thisDropDown);
          thisDropDown.options.generateContentsFunction(thisDropDown);
        }

  }

  function refreshBackgroundImage(element) {

	element = $(element);
	var currentBackground = Element.getStyle(element, 'background-image');
	if (currentBackground == "none") { return; }
	currentBackground = currentBackground.replace(/url\(([^\)]+)\)/, "$1");
	currentBackground = currentBackground.replace(/"$/, "");
	currentBackground = currentBackground.replace(/^"/, "");
	currentBackground = currentBackground.replace(/^([^\?]+).*$/, "url('$1?"+currentImageNum+"')");
	Element.setStyle(element, {backgroundImage: currentBackground});
	//$(element).style.backgroundImage = currentBackground;

  }

  function setProperty(property, classNames, value) {

	// If it's only one class name
	if (typeof(classNames) == "string") {
	  // Set the property of all elements given that class name
	  document.getElementsByClassName(classNames).each( function(el) { $(el).style[property] = value; });
	// Else, if it's an array of class names
	} else if (typeof(classNames) == "object") {
	  for (var x=0; x < classNames.length; x++) {
	    // Call setOpacity on each individual class name
	    setProperty(property, classNames[x], value);
	  }
	}
  }

  function makePro() {

	$('editorLogo').src = 'http://'+editorStatic+'/weebly/images/newui/weebly-editor-logo-pro.jpg';

        hideAllTips();
        showTip(/*tl(*/"Thanks for signing up for a Weebly pro account! We hope you enjoy the extra features, and thank you for supporting us."/*)tl*/, $('editorLogo'), 'y');

	swfu.setFileSizeLimit(Math.floor(Weebly.Restrictions.accessValue('upload_limit_pro')/1000));

	updateElements();
    Weebly.Restrictions.addService(Weebly.Restrictions.proLevel);

  }
  
  function showElementOptions(pageElementID)
  {
    var el = $('options'+pageElementID);
    var offset = el.viewportOffset();
    var outerBox = new Element('div', {'id':'dropdown'+pageElementID,'class':'drop-down-options'}).setStyle( {top:(offset.top+5)+'px', left:(offset.left+20)+'px'} );
    var innerBox = new Element('div').setStyle( {'background':'#4485c9', 'color':'#FFFFFF', 'padding':'1px'} );
    innerBox.update('<img onclick="removeElementOptions(\'dropdown'+pageElementID+'\')" style="float:right; margin:1px; cursor:pointer;" src="http://'+editorStatic+'/weebly/images/close-options.jpg" /><div style="padding:8px 0px 7px 6px">Move to page:</div>');
    var optionsBox = new Element( 'ul' );
    var optionsCount = 0;
    $H(sitePages).each(
      function(pair){
      	// ashaw
      	// since PageManager refactoring, this hasn't been tested (couldn't figure out how to execute this shit)
      	//   old code: $H(blogPages).get(pair.key) === '0'
        if( pair.key != currentPage && !Weebly.PageManager.pages[pair.key].blog )
        {
            var pageName = pair.value.length > 15 ? pair.value.substr(0, 12)+'...' : pair.value;
            var li = new Element( 'li', {'id':'moveTo'+pair.key} );
            li.update( pageName );
            li.observe( 'click',
              function(e)
              {
                $('dropdown'+pageElementID).remove();
                var element = Event.element(e);
                new Ajax.Request(ajax, {parameters: 'pos=moveelement&pageElementID='+pageElementID+'&pageID='+element.id.replace('moveTo', '')+'&cookie='+document.cookie, 'onSuccess': handlerMovePageElement, 'onFailure':errFunc});
              } 
            );
            optionsBox.insert({'bottom':li});
            optionsCount++;
        }
      }
    );
    if(optionsCount > 0)
    {
        innerBox.insert({'bottom':optionsBox});
    }
    else
    {
        innerBox.insert({'bottom':'<ul><li>No destination pages available.</li></ul>'});
    }
    outerBox.insert(innerBox);
    $('body').insert( {'bottom':outerBox} );
  }

  function removeElementOptions(id)
  {
      if($(id)){
          $(id).remove();
      }
  }
  
  function handlerMovePageElement(t)
  {
    var pageID = t.responseText.replace('\n', '' );
    if( $H(sitePages).get(pageID) )
    {
        noJump = 1;
        goUpdateList(pageID, 1);
    }
  }
  
  function copyElement(pageElementID){
      new Ajax.Request(ajax, {parameters: 'pos=copyelement&pageElementID='+pageElementID+'&pageID='+currentPage+'&cookie='+document.cookie, 'onSuccess':function(){updateList(currentPage)},'onFailure':errFunc});
  }

  function saveSiteTitle() {

	new Ajax.Request(ajax, {parameters: 'pos=sitetitle&newtitle='+encodeURIComponent($('newSiteTitle').value)+'&cookie='+document.cookie, 'onSuccess': handlerSaveSiteTitle, 'onFailure':errFunc});
	$('weebly_site_title').innerHTML = $('newSiteTitle').value;

  }

  function handlerSaveSiteTitle(t) {

	if (!settingQuickExport && tempUser != 1) {
	  Pages.go('initialDomainMenu');
	} else {
	  Pages.go('main');
	}
	//Pages.go('editMenu');

  }

  var facebook_js_loaded = false;
  function load_feed() {
    fb_window = window.open('facebook_update.php?site_id='+currentSite, 'facebook_connect', "menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes,width=700,height=500");
    setTimeout("fb_window.focus()", 500);

  } 

  function load_twitter() {

	$('twitterMessage').value = "I just updated my "+Weebly.PROPERTY_NAME+" website, check it out: "+currentSiteLocation;
	$('twitter-publish').style.display = 'block';

  }

  function tweet() {

	new Ajax.Request(ajax, {parameters: 'pos=tweet&username='+encodeURIComponent($('twitterUser').value)+'&password='+encodeURIComponent($('twitterPass').value)+'&message='+encodeURIComponent($('twitterMessage').value)+'&cookie='+document.cookie, 'onSuccess': handlerTweet, 'onFailure':errFunc});

  }

  function handlerTweet(t) {

	$('twitter-publish').style.display = 'none';

  }

  function load_email() {
        new Ajax.Request(ajax, {parameters:'pos=doevent&event=tellFriendsPublish&cookie='+document.cookie});
	var message = "I just updated my website, "+$('weebly_site_title').innerHTML+", and thought you'd like to check it out. You can get to the site by visiting: \n\n"+currentSiteLocation+"\n\n"+Weebly.EMAIL_FOOTER;
        abookwin = window.open('/weebly/apps/abook.php?type=update&message='+encodeURIComponent(message), 'abook', 'toolbar=0,status=0,width=700,height=526');
        setTimeout( 'abookwin.focus()', 300 );
  }

  var faviconUploading = false;
  function uploadFaviconStart(){
      if(!faviconUploading && $('favicon-path').value.length > 0){
          var iframe = new Element('iframe',{'id':'favicon-iframe','name':'favicon-iframe', 'src':'about:blank'}).setStyle({'display':'none'});
          iframe.observe('load', uploadFaviconFinish);
          $('body').insert({'bottom':iframe})
          $('favicon-loading').show();
          $('favicon-display').show();
          faviconUploading = true;
          return true;
      }
      return false;
  }

  function uploadFaviconFinish()
  {
      var result = $('favicon-iframe').contentWindow.document.body.innerHTML;
      var data = result.evalJSON();
      if(data.status === 'success'){
          $('current-favicon').show().setAttribute('src',data.imagePath+'?'+Math.floor(Math.random()*10000000001));
          $('remove-favicon').show();
          $('favicon-upload-area').hide();
      }
      else{
          $('favicon-display').hide();
          alert(data.message);
      }
      $('favicon-iframe').remove();
      $('favicon-loading').hide();
      faviconUploading = false;
  }

  function removeFavicon(){
      new Ajax.Request(ajax, {parameters:'pos=removefavicon&cookie='+document.cookie, onSuccess:function(){$('favicon-display').hide();$('remove-favicon').hide();}, 'onFailure':errFunc});
  }

  function redrawProduct(){
      var pID = Weebly.Elements.pfield[Weebly.Elements.ucfid][69650731].ucfpid;
      var currentDisplayStyle = Weebly.Elements.pfield[Weebly.Elements.ucfid][69650731].propertyresult;
      var newDisplayStyle = $(''+pID) ? $(''+pID).value : null;
      var hideImage = $(''+Weebly.Elements.pfield[Weebly.Elements.ucfid][94961376].ucfpid).value;
      var hideDesc = $(''+Weebly.Elements.pfield[Weebly.Elements.ucfid][36726176].ucfpid).value;
      var buttonStyle = $(''+Weebly.Elements.pfield[Weebly.Elements.ucfid][45179529].ucfpid).value;
      var currentButtonStyle = Weebly.Elements.pfield[Weebly.Elements.ucfid][45179529].propertyresult;
      var align = $(''+Weebly.Elements.pfield[Weebly.Elements.ucfid][90426618].ucfpid).value;
      var ucfid = Weebly.Elements.ucfid;
      if(newDisplayStyle && (currentDisplayStyle !== newDisplayStyle)){
          var image = $(''+Weebly.Elements.ucfid).down('.product-image').src;
          new Ajax.Request(ajax, {parameters:'pos=redrawproduct&productID='+Weebly.Elements.getCurrentProductID()+'&ucfid='+ucfid+'&image='+encodeURIComponent(image)+'&displayStyle='+newDisplayStyle+'&hideImage='+hideImage+'&hideDescription='+hideDesc+'&buttonStyle='+buttonStyle+'&cookie='+document.cookie, onSuccess:function(t){$(''+ucfid).update(t.responseText); makeProductEditable(ucfid);}, 'onFailure':errFunc});
      }
      if(hideImage == 0){
          $(''+ucfid).down('.product-image').show();
      }
      else{
          $(''+ucfid).down('.product-image').hide();
      }
      var prodDesc = $(''+ucfid).down('.product-description');
      if(hideDesc == 0 && prodDesc){
          prodDesc.show();
      }
      else if(prodDesc){
          prodDesc.hide();
      }
      if(align === 'right'){
          $(''+ucfid).down('.product').setStyle({'marginRight':'0px', 'marginLeft':'auto'});
      }
      else if(align === 'center'){
          $(''+ucfid).down('.product').setStyle({'marginRight':'auto', 'marginLeft':'auto'});
      }
      else if(align === 'left'){
          $(''+ucfid).down('.product').setStyle({'marginRight':'0px', 'marginLeft':'0px'});
      }
      if(buttonStyle !== currentButtonStyle){
          updateProductButton(ucfid,buttonStyle);
          setUserPreference('productButtonStyle', buttonStyle);
      }
      
      Weebly.Elements.continueOnChange();
      Weebly.Elements.saveProperties();
      Weebly.Elements.generateProperties(Weebly.Elements.currentElement);
  }

  function updateProductButton(ucfid, buttonStyle){
      var image = $(''+ucfid).down('.product-button');
      var size = image.src.match(/small|big/);
      var style = 'buy_now';
      if(buttonStyle === 'add_to_cart'){
          style = 'add_to_cart';
      }
      image.src = 'http://'+editorStatic+'/weebly/images/'+style+'_'+size+'.gif';
  }

  function makeProductEditable(ucfid){
      var title = $(''+ucfid).down('.product-title');
      if(title){title.onclick = function(){showEditBox(title.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':true})};}
      var price = $(''+ucfid).down('.product-price');
      if(price){price.onclick = function(event){showEditBox(price.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':false})};}
      var desc = $(''+ucfid).down('.product-description');
      if(desc){desc.onclick = function(event){showEditBox(desc.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':true})};}
      var form = $(''+ucfid).down('form');
      if(form){form.onsubmit = function(){return false;}}
      var img = $(''+ucfid).down('.product-image');
      if(img){
        img.onload =
            function(){
              Weebly.ImageResize.init(img, {callback: onResize, ucfid: ucfid});
              img.onload = null;
            };
      }
  }

  function saveProductField(id, text, align){
      var el = $(id);
      var type = el.className.match(/price|title|description/);
      if(type){type = type[0];}
      if(type === 'price'){
          text = text.replace(/[^\d\.]/g, '');
      }
      if(type){
          var productID = el.id.replace(/[^\d]/g, '');
      } else{
          var productID = Weebly.Elements.getCurrentProductID();
      }
      new Ajax.Request(ajax, {parameters:'pos=updateproduct&productID='+productID+'&type='+type+'&value='+encodeURIComponent(text)+'&cookie='+document.cookie,'onFailure':errFunc});
  }

  function setUserPreference(key, value){
      new Ajax.Request(ajax, {parameters:'pos=setuserpreference&key='+key+'&value='+value+'&cookie='+document.cookie,'onFailure':errFunc});
  }

  function loadExternalLibraries(){
      externalLibariesLoaded = true;
      //CodePress.run();
      checkFlash();
  }

  function showMerchantSettingsOption(){
      if($('site_settings_merchantaccount')){
          $('site_settings_googlecheckout').hide();
          $('site_settings_paypal').hide();
          switch($F('site_settings_merchantaccount')){
              case 'google':
                  $('site_settings_googlecheckout').show();
                  $('site_settings_curr_warning').show();
                  $('site_settings_currency').value = 'USD';
                  $('site_settings_currency').disable();
                  break;
              case 'paypal':
                  $('site_settings_paypal').show();
                  $('site_settings_curr_warning').hide();
                  $('site_settings_currency').enable();
                  break;
          }
      }
  }

function setupFileUploader(el){ // used to place the flash uploader on top of the initial image (gallery/standalone/etc)
                                 // for both FLASH uploader and PLAIN uploader
	if (isUploaderImage(el)) {
	    $(el).stopObserving('mouseover');
	    $(el).observe('mouseover', function(e){
	        if(isUploaderImage(el)){
	        
	        	//if(isGalleryImage(el)){
	        	
			        showFlashContainer(19); // to be under menu bar (which has zindex of 20)
			        if (Prototype.Browser.Gecko) {
			            $('flashContainer').clonePosition($(el));
			        } else {
			            $('flashContainer').style.position = 'relative';
			            $('flashContainer').clonePosition($(el));
			            $('flashContainer').style.position = 'absolute';
			        }
			        forceFlashPosition = true;

			        $('flashContainer').stopObserving('mousedown');
			        $('flashContainer').observe('mousedown', function(){
			            selectDefaultImageUpload(el.up('.element').id);
			        });
			        document.observe('mousemove', removeFlashUploader);
	        	
	        	//// for html-based file uploader
	        	//}else{
	        	//	var uploader = getPlainUploader('content');
				//	uploader.beforeQueued = function() {
				//		selectDefaultImageUpload(el.up('.element').id);
				//	};
				//	uploader.show(el, null, 19, true, true); // below menubar (menubar's zindex is 20)
			    //}
			    
	        }
	    });
    }
}

function removeFlashUploader(event){
    var el = Event.element(event);
    if(!isUploaderImage(el) && el.id !== 'flashContainer' && !el.up('#flashContainer')){
        forceFlashPosition = false;
        Weebly.Elements.positionFlash();
        $('flashContainer').stopObserving('mousedown');
        document.stopObserving('mousemove', removeFlashUploader);
    }
}

function isUploaderImage(el){
    return el.src && isUploaderImageSrc(el.src);
}

function isUploaderImageSrc(src){
    return src.match(/na\.jpg/) || src.match(/upload_images_01\.jpg/) || src.match(/video_click_to_upload\.jpg/)
}

//// for html-based file uploader
//function isGalleryImage(el) {
//	var ul = $(el).up('ul');
//	return ul && ul.hasClassName('imageGallery');
//}

function editPollDaddy(pollId, ucfid, ucfpid){
    Pages.go('pollDaddy');
    var pro = isPro() ? '1' : '0';
    setTimeout("$('poll-daddy-iframe').writeAttribute( 'src', 'http://jr.polldaddy.com/auth-weebly.php?userid='+userID+'&proUser="+pro+"&ucfpid="+ucfpid+"&callback=test&finalUrl='+encodeURIComponent('http://'+configSiteName+'/weebly/main.php#closePollDaddy') );", 500 );
}

function viewFormData(ucfid){
    window.open('viewFormData.php?ucfid='+ucfid, 'weebly_view_form', 'height=650,width=960,menubar=yes,toolbar=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
}


Weebly.TimingTest = {

  data: {},
  currentTest: '',
  

  start: function(test) {

    Weebly.TimingTest.data[test] = {};
    Weebly.TimingTest.data[test].start = new Date().getTime();

  },

  end: function(test) {

    if (!Weebly.TimingTest.data[test] || (Weebly.TimingTest.data[test] && !Weebly.TimingTest.data[test].start)) { return; }

    Weebly.TimingTest.data[test].diff = new Date().getTime() - Weebly.TimingTest.data[test].start;

    // Disabled
    //new Ajax.Request(ajax, {parameters:'pos=speedtest&test='+test+'&time_elapsed='+Weebly.TimingTest.data[test].diff+'&cookie='+document.cookie, bgRequest: true});

  }

}

var myColorPicker;
var maxFileSize;

   function initMain(functionsToCall) {

	//console.log("initMain start");
	//Effect.Center('pleaseWait');
	//new PeriodicalExecuter(feedback, 1);

        if( functionsToCall.userID > 0) {
          userID = functionsToCall.userID;
        }
	if( functionsToCall.userIDLocation != '') {
	  userIDLocation = functionsToCall.userIDLocation;
	}
	if( functionsToCall.pageid[0] == '1') {
	  currentPage = functionsToCall.pageid[1];
	}
	if( functionsToCall.setSettingAnimations == '1') {
	  setSetting('settingAnimations',1);
	  Pages.optAnimations = 1;
	} else { Pages.optAnimations = 0; }
        if( functionsToCall.siteVersion == '1' || functionsToCall.siteVersion == '2' || functionsToCall.siteVersion == '3' || functionsToCall.customDomain != '') {
          setSetting('settingQuickExport',1);
        }
        if( functionsToCall.setSettingTooltips == '1') {
          setSetting('settingTooltips',1);
        }
	if( functionsToCall.currentTheme) {
	  setSetting('currentTheme', "'"+functionsToCall.currentTheme+"'");
	}
        if( functionsToCall.currentHeader) {
          setSetting('currentHeader', "'"+functionsToCall.currentHeader+"'");
        }
        if( functionsToCall.siteType) {
          setSetting('siteType', "'"+functionsToCall.siteType+"'");
        }
        if( functionsToCall.updatedTheme) {
          setSetting('updatedTheme', "'"+functionsToCall.updatedTheme+"'");
        }
        if( functionsToCall.friendRequests) {
          setSetting('friendRequests', "'"+functionsToCall.friendRequests+"'");
        }
        if( functionsToCall.adsenseID) {
          setSetting('adsenseID', "'"+functionsToCall.adsenseID+"'");
	}
	if( functionsToCall.tempUser) {
	  setSetting('tempUser', "'"+functionsToCall.tempUser+"'");
	  if(tempUser == 1) {
	    window.onbeforeunload = function() {
	      return /*tl(*/"If you close the window, all your changes will be lost."/*)tl*/;
	    }
	  }
	}
        if( functionsToCall.userEvents) {
	  setSetting('userEvents', "'"+functionsToCall.userEvents+"'");
	}
        if( functionsToCall.hideTitle) {
          setSetting('hideTitle', "'"+functionsToCall.hideTitle+"'");
        }

	setWidth();
	//Element.setStyle("elements", { top: "expression((dummy = document.documentElement.scrollTop) + 'px')"});
	//$('elements').style.top = "0px";

	if (navigator.appVersion.indexOf("MSIE") > -1) {
	  $('scroll_container').style.top = '133px';
	}

	currentSite = functionsToCall.currentSite;

	Pages.pageConstructor.main = { element: 'grayedOut', go: function() { } }; //alert(9); updateList(currentPage); } };
        Pages.pageDestructor.main = { element: 'grayedOut', maxHeight: ["$('left').clientHeight+135","$('container').clientHeight+30","$('right').clientHeight+135"], go: function() { if( document.getElementById(currentBox+'Edit')){ hideEditBox(currentBox);}} };
        Pages.pageWindows.main = ['main', 'themesMenu'];

	Pages.pageConstructor.showElements = { go: function() { showElements(); } };
	Pages.pageDestructor.showElements = { go: function() { hideElements(); } };
	Pages.pageWindows.showElements = ['showElements','main'];

        Pages.pageConstructor.save = { element: 'tip13' };
        Pages.pageDestructor.save = { element: 'tip13' };
	Pages.pageWindows.save = ['save','main'];

	Pages.pageConstructor.displayPageProperties = { go: function(var1) { pageDblClick = 1; displayPageProperties(var1); } };
        Pages.pageDestructor.displayPageProperties = { element: 'textEditor', go: function() { fadeProperties(); } };
	Pages.pageWindows.displayPageProperties = ['displayPageProperties'];

        Pages.pageConstructor.exportSite = { go: function() { if(tempUser == 1) { setTimeout("Pages.go('goSignup', 'publish');",100); } else { exportSite(); } } };
        Pages.pageDestructor.exportSite = { element: 'textEditor', go: function() { fadeProperties(); $('tip14').style.display = 'none'; } };
        Pages.pageWindows.exportSite = ['exportSite', 'themesMenu'];

        Pages.pageConstructor.doExport = { go: function() { doExport(); } };
        Pages.pageDestructor.doExport = { element: 'publishingWait', go: function() { publishingAnim = 0; $('tip14').style.display = 'none'; } };
        Pages.pageWindows.doExport = ['doExport', 'themesMenu'];

	Pages.pageConstructor.friendRequests = { element: 'friendRequests' };
	Pages.pageDestructor.friendRequests = { element: 'friendRequests' };
	Pages.pageWindows.friendRequests = ['friendRequests', 'themesMenu'];

	Pages.pageConstructor.showMyspaceLogin = { go: function() { showMyspaceLogin(); } };
	Pages.pageDestructor.showMyspaceLogin = { element: 'publishMyspaceDialog' };
	Pages.pageWindows.showMyspaceLogin = ['showMyspaceLogin', 'themesMenu'];

        Pages.pageConstructor.exportSuccess = { element: 'tip14' };
        Pages.pageDestructor.exportSuccess = { element: 'tip14' };
        Pages.pageWindows.exportSuccess = ['exportSuccess', 'themesMenu', 'main'];

        Pages.pageConstructor.addPage = { go: function() { AddPage(); } };
        Pages.pageDestructor.addPage = { element: 'textEditor', go: function() { fadeProperties(); } };
        Pages.pageWindows.addPage = ['addPage'];

        Pages.pageConstructor.displayUserSettings = { go: function() { displayUserSettings(); } };
        Pages.pageDestructor.displayUserSettings = { element: 'textEditor' };
        Pages.pageWindows.displayUserSettings = ['displayUserSettings'];

        Pages.pageConstructor.displaySiteSettings = { go: function(dontUpdate) { displaySiteSettings(dontUpdate); activeContainer('settings'); activeTab('weebly_tab_settings'); } };
        Pages.pageDestructor.displaySiteSettings = { element: 'textEditor', go: function() { activeContainer('elements'); activeTab('weebly_tab_edit'); fadeProperties(); } };
        Pages.pageWindows.displaySiteSettings = ['displaySiteSettings'];

	Pages.pageConstructor.domainMenu = { go: function(isPublish) { 

		if (isPublish == 1) { 
		  domainNextStep = "publish"; 
		} else if (isPublish == 2) { 
		  domainNextStep = "main" ;
		} else { 
		  domainNextStep = "sitesettings";
		}

		$('chooseDomain').style.top = "10px";
		$('chooseDomainClose').onclick = isPublish > 0 ? function(){ Pages.go('main'); return false; } : function(){ Pages.go('displaySiteSettings', 1); return false; }; 

		activeContainer('settings'); 
		activeTab('weebly_tab_settings'); 
		domainChoiceReset(); 

	}, element: 'domainContainer' };
	Pages.pageDestructor.domainMenu = { element: 'domainContainer', go: function() { activeContainer('elements'); activeTab('weebly_tab_edit'); } };
	Pages.pageWindows.domainMenu = ['displaySiteSettings', 'domainMenu'];

        Pages.pageConstructor.initialDomainMenu = { go: function(isPublish) { $('grayedOutTop').style.display = 'block'; $('chooseDomain').style.top = "26px"; domainNextStep = "main"; $('chooseDomainClose').onclick = function(){ Pages.go('main'); return false; }; domainChoiceReset(); }, element: 'domainContainer' };
        Pages.pageDestructor.initialDomainMenu = { element: 'domainContainer', go: function() { activeContainer('elements'); activeTab('weebly_tab_edit'); showEvent("first_tip"); $('grayedOutTop').style.display = 'none'; } };
        Pages.pageWindows.initialDomainMenu = ['initialDomainMenu'];

	Pages.pageConstructor.domainMenuPurchase = { go: function(domainName) { domainNextStep = "main"; activeContainer('settings'); activeTab('weebly_tab_settings'); $('domain_sld').value = domainName; }, element: 'domainContainer' };
        Pages.pageDestructor.domainMenuPurchase = { element: 'domainContainer', go: function() { activeContainer('elements'); activeTab('weebly_tab_edit'); } };
        Pages.pageWindows.domainMenuPurchase = ['domainMenuPurchase', 'domainMenu'];         

        Pages.pageConstructor.proPurchase = { go: function(message, refer) { if(tempUser == 1) { setTimeout("Pages.go('goSignup');",100); } else { showProPurchase(message, refer); $('purchaseX').style.display = 'block'; } }, element: 'domainContainer' };
        Pages.pageDestructor.proPurchase = { element: 'domainContainer', go: function() { $('purchaseX').style.display = 'none'; } };
        Pages.pageWindows.proPurchase = ['proPurchase', 'displaySiteSettings', 'pagesMenu'];

        if (ENABLE_THEME_BROWSER) {
	        Pages.pageConstructor.themesMenu = { go: function() {
	        	if (!window.litePageChange) {
	        		enteringDesignTab();
	        	}
	        	resetThemesMenuScrolling();
	        	activeTab('weebly_tab_themes');
	        	activeContainer('themes');
	        }};
            Pages.pageDestructor.themesMenu = { go: function() { leavingDesignTab(); }  };
        }else{
            Pages.pageConstructor.themesMenu = { go: function() { activeTab('weebly_tab_themes'); showThemes(); activeContainer('themes'); } };
            Pages.pageDestructor.themesMenu = { };
        }
        Pages.pageWindows.themesMenu = ['themesMenu', 'main'];

        Pages.pageConstructor.customThemeEditor = { go: function() { openCustomThemeEditor(); }, element: 'customThemeContainer' };
        Pages.pageDestructor.customThemeEditor = { go: function() { closeCustomThemeEditor();}, element: 'customThemeContainer' };
        Pages.pageWindows.customThemeEditor = ['customThemeEditor', 'themesMenu', 'main']; // for theme browser ~ashaw
        
        Pages.pageConstructor.importTheme = { go: function() { Weebly.lightbox.show({element: '#themeEditBoxImport', width: 600, height: 400}) } };
        Pages.pageDestructor.importTheme = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.importTheme = ['importTheme', 'themesMenu', 'main']; // for theme browser ~ashaw

	Pages.pageConstructor.pagesMenu = { go: function(newPage) { Weebly.PageManager.buildUI(newPage); activeContainer('pages'); activeTab('weebly_tab_pages'); } };
	Pages.pageDestructor.pagesMenu = { element: 'pagesContainer', go: function() { Weebly.PageManager.cleanup(); activeContainer('elements'); activeTab('weebly_tab_edit'); fadeProperties(); } };
	Pages.pageWindows.pagesMenu = ['pagesMenu', 'main'];
	
	Pages.pageConstructor.editMenu = { go: function() { setElementsPageType(); activeTab('weebly_tab_edit'); activeContainer('elements'); } };
	Pages.pageDestructor.editMenu = { };
	Pages.pageWindows.editMenu = ['editMenu', 'main'];

        Pages.pageConstructor.welcomeMenu = { element: 'welcomeContainer', go: function() { $('grayedOutTop').style.display = 'block'; $('newSiteTitle').focus(); } };
        Pages.pageDestructor.welcomeMenu = { element: 'welcomeContainer', go: function() { $('grayedOutTop').style.display = 'none';} };
        Pages.pageWindows.welcomeMenu = ['welcomeMenu'];

        Pages.pageConstructor.displayProperties = { go: function(ucfid) { displayProperties(ucfid); } };
        Pages.pageDestructor.displayProperties = { element: 'textEditor', go: function() { fadeProperties(); } };
        Pages.pageWindows.displayProperties = ['displayProperties'];

        Pages.pageConstructor.updateList = { go: function(pageID) { updateList(pageID); } };
        Pages.pageDestructor.updateList = {  }; 
        Pages.pageWindows.updateList = ['updateList', 'main', 'themesMenu'];

        Pages.pageConstructor.userHome = { go: function() { if(tempUser == 1) { setTimeout("Pages.go('goSignup', 'exit');",100); } else { window.onbeforeunload = null; setTimeout("window.location = 'userHome.php'", 400); } } };
        Pages.pageDestructor.userHome = {  };
        Pages.pageWindows.userHome = ['userHome'];

        Pages.pageConstructor.showHelp = { element: 'helpFrame', go: function() { $('helpFrame').style.height = (getInnerHeight() - 35) + "px"; $('helpIframe').src = helpLocation; } };
        Pages.pageDestructor.showHelp = { element: 'helpFrame' };
        Pages.pageWindows.showHelp = ['showHelp','themesMenu','main'];

        Pages.pageConstructor.giveFeedback = { go: showFeedback };
        Pages.pageDestructor.giveFeedback = { go: hideFeedback };
        Pages.pageWindows.giveFeedback = ['giveFeedback','main'];

        Pages.pageConstructor.goSignup = { go: function(action) { showSignup(action); } };
        Pages.pageDestructor.goSignup = { go: hideSignup };
        Pages.pageWindows.goSignup = ['goSignup'];

        Pages.pageConstructor.editImage = { go: function(oldImageLocation, imageId) { editImage(oldImageLocation, imageId); } };
        Pages.pageDestructor.editImage = { go: hideEditImage };
        Pages.pageWindows.editImage = ['editImage', 'displayProperties'];

        Pages.pageConstructor.goBlogPost = { go: function(postId, newPost) { currentBlog.postId = postId; updateList(currentPage, newPost);  } };
        Pages.pageDestructor.goBlogPost = { go: function() { currentBlog.postId = 0; currentBlog.newPost = 0; currentBlog.title = ''; currentBlog.categories = ''; } };
        Pages.pageWindows.goBlogPost = ['goBlogPost', 'themesMenu', 'main'];

	Pages.pageConstructor.domainEditor = { go: function() { Element.show('domainEditor'); } };
        Pages.pageDestructor.domainEditor = { go: function() { Element.hide('domainEditor'); } };
        Pages.pageWindows.domainEditor = ['domainEditor'];

        Pages.pageConstructor.adsenseSetup = { go: function() { if(tempUser == 1) { setTimeout("Pages.go('goSignup', 'adsense');",100); } else { Weebly.lightbox.show({element: '#adsense_terms', width: 480, height: 400, onHide: function() { onHideLightbox(); }});new Ajax.Request(ajax, {parameters:'pos=doevent&event=viewAdsense&cookie='+document.cookie}); } } };
        Pages.pageDestructor.adsenseSetup = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.adsenseSetup = ['adsenseSetup', 'main'];

        Pages.pageConstructor.externalSetup = { go: function(remoteSite, elementId) { if(tempUser == 1) { setTimeout("Pages.go('goSignup');",100); } else { Weebly.lightbox.show({element: '#'+remoteSite, button: {onClick: 'submitExternal("'+remoteSite+'");'}, onHide: function() { removeElement(elementId); }}); } } };
        Pages.pageDestructor.externalSetup = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.externalSetup = ['externalSetup', 'main'];

        Pages.pageConstructor.pollDaddy = { go: function() {Weebly.lightbox.show({element: '#edit-poll-daddy', onHide: function() { updateList(); }})}};
        Pages.pageDestructor.pollDaddy = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.pollDaddy = ['pollDaddy', 'goBlogPost', 'main'];

        Pages.pageConstructor.upgradeWarning = { go: function() {Weebly.lightbox.show({element: '#upgrade-warning', options:{hideClose:true}, onHide: function() { updateList(); }})}};
        Pages.pageDestructor.upgradeWarning = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.upgradeWarning = ['upgradeWarning', 'goBlogPost','main'];

        Pages.pageConstructor.formElementWarning = { go: function() {Weebly.lightbox.show({element: '#form-element-warning', options:{hideClose:true}, onHide: function() { updateList(); }})}};
        Pages.pageDestructor.formElementWarning = { go: function() { Weebly.lightbox.hide(); } };
        Pages.pageWindows.formElementWarning = ['formElementWarning', 'goBlogPost','main'];

	var signupIsPro = 0;
        Pages.pageConstructor.purchaseConfirmation = { element: 'purchaseConfirmation', go: function(pro) { if (pro) { signupIsPro = 1; } } };
        Pages.pageDestructor.purchaseConfirmation = { element: 'purchaseConfirmation', go: function() { if (signupIsPro) { signupIsPro = 0; makePro(); } if(publishAfterPro){publishAfterPro = false; Pages.go('exportSite');} } };
        Pages.pageWindows.purchaseConfirmation = ['purchaseConfirmation'];

        Pages.pageConstructor.moderateBlog = { go: function(id) { goModerateBlog(id); } };
        Pages.pageDestructor.moderateBlog = { element: 'newContainer' };
        Pages.pageWindows.moderateBlog = ['moderateBlog'];

	//myColorPicker = new Control.ColorPicker("currentColor", { IMAGE_BASE : "http://www.weebly.com/weebly/images/colorpicker/", 'swatch' : 'menuitem-cc', 'onClose': function() { setColor(); }, 'position': 'toolbar' });
    newColorChooser = new Weebly.ColorChooser($('new-color-chooser'), {onUpdate:function(color){runCommand(currentBox, 'forecolor', color);}});

    showBar();

	if (isPro()) {
	  maxFileSize = Math.floor(Weebly.Restrictions.accessValue('upload_limit_pro') / 1000);
	} else {
	  maxFileSize = Math.floor(Weebly.Restrictions.accessValue('upload_limit_free') / 1000); // 5120;
	}

        swfu = new SWFUpload({

          file_size_limit : maxFileSize,
          file_types : "*.*",
          file_types_description : "All files...",
          file_queue_limit : 20,

          file_dialog_start_handler : dialogStart,
          file_dialog_complete_handler : dialogComplete,
          upload_start_handler : uploadStart,
          file_queue_error_handler : queueError,
          file_queued_handler : uploadFileQueued,
          upload_error_handler : uploadError,
          upload_success_handler : uploadSuccess,
          upload_progress_handler : uploadProgress,
          upload_complete_handler : uploadFileComplete,
          swfupload_loaded_handler : swfUploadLoaded,

	  prevent_swf_caching: true,

	  button_placeholder_id: "flashButtonPlaceholder",
	  button_width: 600,
	  button_height: 600,
	  button_window_mode: SWFUpload.WINDOW_MODE.TRANSPARENT,
	  button_cursor: SWFUpload.CURSOR.HAND,

          flash_url : "/weebly/libraries/swfupload/swfupload.swf"

        });
        
	Pages.go('editMenu');
	if (functionsToCall.siteTitle == 'No Title') {
	  Pages.go('welcomeMenu');
	}
	else if (settingQuickExport == 0) { // apparently settingQuickExport==0 means they haven't set up a domain yet. ~ashaw
	  Pages.go('initialDomainMenu');
	}
	//setTimeout("Pages.go('editMenu');", 1000);

	// ashaw
    //updatePages(1);
    Weebly.PageManager.init();
    //

 	var editorLoadTime = (new Date().getTime() - loadTime['a0']) / 1000;
	loadTime['final'] = editorLoadTime;
	fireTrackingEvent('EditorLoadTime', 'Load', '', editorLoadTime);
	recordLoad();
    showEvent('first_tip');
	//console.log("initMain end");
   }


/*
   Behaviour v1.1 by Ben Nolan, June 2005. Based largely on the work
   of Simon Willison (see comments by Simon below).

   Description:
   	
   	Uses css selectors to apply javascript behaviours to enable
   	unobtrusive javascript in html documents.
   	
   Usage:   
   
	var myrules = {
		'b.someclass' : function(element){
			element.onclick = function(){
				alert(this.innerHTML);
			}
		},
		'#someid u' : function(element){
			element.onmouseover = function(){
				this.innerHTML = "BLAH!";
			}
		}
	};
	
	Behaviour.register(myrules);
	
	// Call Behaviour.apply() to re-apply the rules (if you
	// update the dom, etc).

   License:
   
   	This file is entirely BSD licensed.
   	
   More information:
   	
   	http://ripcord.co.nz/behaviour/
   
*/   

var Behaviour = {
	list : new Array,
	
	register : function(sheet){
		Behaviour.list.push(sheet);
	},
	
	start : function(){
		Behaviour.addLoadEvent(function(){
			//Behaviour.apply();
		});
	},
	
	apply : function(){
		for (h=0;sheet=Behaviour.list[h];h++){
			for (selector in sheet){
				list = document.getElementsBySelector(selector);
				
				if (!list){
					continue;
				}

				for (i=0;element=list[i];i++){
					sheet[selector](element);
				}
			}
		}
	},
	
	addLoadEvent : function(func){
		var oldonload = window.onload;
		
		if (typeof window.onload != 'function') {
			window.onload = func;
		} else {
			window.onload = function() {
				oldonload();
				func();
			}
		}
	}
}

Behaviour.start();

/*
   The following code is Copyright (C) Simon Willison 2004.

   document.getElementsBySelector(selector)
   - returns an array of element objects from the current document
     matching the CSS selector. Selectors can contain element names, 
     class names and ids and can be nested. For example:
     
       elements = document.getElementsBySelect('div#main p a.external')
     
     Will return an array of all 'a' elements with 'external' in their 
     class attribute that are contained inside 'p' elements that are 
     contained inside the 'div' element which has id="main"

   New in version 0.4: Support for CSS2 and CSS3 attribute selectors:
   See http://www.w3.org/TR/css3-selectors/#attribute-selectors

   Version 0.4 - Simon Willison, March 25th 2003
   -- Works in Phoenix 0.5, Mozilla 1.3, Opera 7, Internet Explorer 6, Internet Explorer 5 on Windows
   -- Opera 7 fails 
*/

function getAllChildren(e) {
  // Returns all children of element. Workaround required for IE5/Windows. Ugh.
  return e.all ? e.all : e.getElementsByTagName('*');
}

document.getElementsBySelector = function(selector) {
    return $$(selector);
  // Attempt to fail gracefully in lesser browsers
  if (!document.getElementsByTagName) {
    return new Array();
  }
  // Split selector in to tokens
  var tokens = selector.split(' ');
  var currentContext = new Array(document);
  for (var i = 0; i < tokens.length; i++) {
    token = tokens[i].replace(/^\s+/,'').replace(/\s+$/,'');;
    if (token.indexOf('#') > -1) {
      // Token is an ID selector
      var bits = token.split('#');
      var tagName = bits[0];
      var id = bits[1];
      var element = document.getElementById(id);
      if (tagName && element && element.nodeName.toLowerCase() != tagName) {
        // tag with that ID not found, return false
        return new Array();
      } else if (!element || (element && !element.nodeName)) {
	// Fix: David Rusenko (weebly.com) for MSIE
	// In this case, there is no element returned by document.getElementById()
	return new Array();
      }
      // Set currentContext to contain just this element
      currentContext = new Array(element);
      continue; // Skip to next token
    }
    if (token.indexOf('.') > -1) {
      // Token contains a class selector
      var bits = token.split('.');
      var tagName = bits[0];
      var className = bits[1];
      if (!tagName) {
        tagName = '*';
      }
      // Get elements matching tag, filter them for class selector
      var found = new Array;
      var foundCount = 0;
      for (var h = 0; h < currentContext.length; h++) {
        var elements;
        if (tagName == '*') {
            elements = getAllChildren(currentContext[h]);
        } else {
            elements = currentContext[h].getElementsByTagName(tagName);
        }
        for (var j = 0; j < elements.length; j++) {
          found[foundCount++] = elements[j];
        }
      }
      currentContext = new Array;
      var currentContextIndex = 0;
      for (var k = 0; k < found.length; k++) {
        if (found[k].className && found[k].className.match(new RegExp('\\b'+className+'\\b'))) {
          currentContext[currentContextIndex++] = found[k];
        }
      }
      continue; // Skip to next token
    }
    // Code to deal with attribute selectors
/**
// We don't need this...
    if (token.match(/^(\w*)\[(\w+)([=~\|\^\$\*]?)=?"?([^\]"]*)"?\]$/)) {
      var tagName = RegExp.$1;
      var attrName = RegExp.$2;
      var attrOperator = RegExp.$3;
      var attrValue = RegExp.$4;
      if (!tagName) {
        tagName = '*';
      }
      // Grab all of the tagName elements within current context
      var found = new Array;
      var foundCount = 0;
      for (var h = 0; h < currentContext.length; h++) {
        var elements;
        if (tagName == '*') {
            elements = getAllChildren(currentContext[h]);
        } else {
            elements = currentContext[h].getElementsByTagName(tagName);
        }
        for (var j = 0; j < elements.length; j++) {
          found[foundCount++] = elements[j];
        }
      }
      currentContext = new Array;
      var currentContextIndex = 0;
      var checkFunction; // This function will be used to filter the elements
      switch (attrOperator) {
        case '=': // Equality
          checkFunction = function(e) { return (e.getAttribute(attrName) == attrValue); };
          break;
        case '~': // Match one of space seperated words 
          checkFunction = function(e) { return (e.getAttribute(attrName).match(new RegExp('\\b'+attrValue+'\\b'))); };
          break;
        case '|': // Match start with value followed by optional hyphen
          checkFunction = function(e) { return (e.getAttribute(attrName).match(new RegExp('^'+attrValue+'-?'))); };
          break;
        case '^': // Match starts with value
          checkFunction = function(e) { return (e.getAttribute(attrName).indexOf(attrValue) == 0); };
          break;
        case '$': // Match ends with value - fails with "Warning" in Opera 7
          checkFunction = function(e) { return (e.getAttribute(attrName).lastIndexOf(attrValue) == e.getAttribute(attrName).length - attrValue.length); };
          break;
        case '*': // Match ends with value
          checkFunction = function(e) { return (e.getAttribute(attrName).indexOf(attrValue) > -1); };
          break;
        default :
          // Just test for existence of attribute
          checkFunction = function(e) { return e.getAttribute(attrName); };
      }
      currentContext = new Array;
      var currentContextIndex = 0;
      for (var k = 0; k < found.length; k++) {
        if (checkFunction(found[k])) {
          currentContext[currentContextIndex++] = found[k];
        }
      }
      // alert('Attribute Selector: '+tagName+' '+attrName+' '+attrOperator+' '+attrValue);
      continue; // Skip to next token
    }
**/    

    if (!currentContext[0]){
    	return;
    }
    
    // If we get here, token is JUST an element (not a class or ID selector)
    tagName = token;
    var found = new Array;
    var foundCount = 0;
    for (var h = 0; h < currentContext.length; h++) {
      var elements = currentContext[h].getElementsByTagName(tagName);
      for (var j = 0; j < elements.length; j++) {
        found[foundCount++] = elements[j];
      }
    }
    currentContext = found;
  }
  return currentContext;
}

/* That revolting regular expression explained 
/^(\w+)\[(\w+)([=~\|\^\$\*]?)=?"?([^\]"]*)"?\]$/
  \---/  \---/\-------------/    \-------/
    |      |         |               |
    |      |         |           The value
    |      |    ~,|,^,$,* or =
    |   Attribute 
   Tag
*/

Position.includeScrollOffsets = true;
var splitpanes = new Array();
var contentDraggables = new Array();

/***************Rules**********************/
var myrules = {
   
    '#body' : function(el) {

	//Initialize here
	contentDraggables = [];

    },
    '#error' : function(el){
	el.onclick = function(){
	    new Effect.Fade('error');
	}
    }, 
    '#tip11' : function(el){
        el.onclick = function(){
            new Effect.Fade('tip11');
        }
    },
    '#tip12' : function(el){
        el.onclick = function(){
            new Effect.Fade('tip12');
        }
    },
    '#tip13' : function(el){
        el.onclick = function(){
            Pages.go('main');
        }
    },
    '#help' : function(el){
        el.onclick = function(){
            Pages.go('main');
        }
    },
    '#warning' : function(el){
        el.onclick = function(){
            new $('warning').hide();
        }
    },

    '#elementlist' : function(el){
	    contentDraggables.push(el);
            //Sortable.create("elementlist", {dropOnEmpty:true,containment:["elementlist","secondlist"],constraint:false, onUpdate:updateElements});
    },
    '#secondlist .columnlist' : function(el){
	    //contentDraggables.push($(el).id);
	    resizeColumns(el);
    },
    '#secondlist .columnlist .element img' : function(el){
        el.onload = function(event){
            var colEl = el.up('#secondlist .columnlist');
            if(colEl){
                if(colEl.id.match(/rhs-list/)){
                    colEl = $(colEl.id.replace(/rhs-list/, 'lhs-list'));
                }
                resizeColumns(colEl);
                if(el.up('.element-panes')){
                    updateTwoColumnDividerHeight(el.up('.element-panes').id);
                }
            }
        };
    },
    '#secondlist .columnlist-blog' : function(el){
        contentDraggables.push(el);
	    if (el.childNodes.length == 0) {
		el.style.height = '400px';
	    } else {
		el.style.height = 'auto';
	    }
    },
    '#secondlist-blog' : function(el) {
            contentDraggables.push(el);
	    Sortable.create('secondlist-blog', {hoverclass: 'hoverclass', dropOnEmpty:true,handle:'handle',scroll:'scroll_container',scrollSensitivity:50,scrollSpeed:5,containment:contentDraggables,constraint:false,onUpdate:function() { updateList(); } });
    },
    // (Medium) Memory leak 1
    '#weebly_site_title' : function(el) {
    	if (Weebly.Restrictions.hasAccess('editable_site_title')) {
			if (! $('weebly_site_titleEdit')) {
			  var iframeEl = document.createElement('iframe');
			  iframeEl.id = 'weebly_site_titleEdit';
			  iframeEl.className = 'editable';
			  iframeEl.frameBorder = 0;
			  iframeEl.allowTransparency = true;
			  if ($('weebly_site_title').nextSibling) {
				$('weebly_site_title').parentNode.insertBefore(iframeEl, $('weebly_site_title').nextSibling);
			  } else {
				$('weebly_site_title').parentNode.appendChild(iframeEl);
			  }
			  $('weebly_site_title').onclick = function() { unselectHeader(1); showEditBox('weebly_site_title'); return false; };
			  $('weebly_site_title').style.display = 'block';
			  //new Ajax.InPlaceEditor('weebly_site_title', ajax, { ajaxOptions: { bgRequest: true}, callback: function(form, value) { return 'pos=sitetitle&newtitle=' + encodeURIComponent(value)+"&cookie="+document.cookie; }, onComplete: function(form, value) { updateSiteTitle(value); }, onEnterEditMode:function() { unselectHeader(1)}, okText: '', okButton: false, cancelLink: false, cancelText: ' ', submitOnBlur: true, border: '1px dashed #4455aa', highlightcolor: 'transparent', hoverClassName: 'element-hover'});
			  //alert( $($(el).id+'-inplaceeditor') );
			}
		}
    },

    //'#colorpicker' : function(el){
    //        ColorSelector.create('colorpicker', {value: '255,100,0', transforms:DefaultTransforms});
    //},

    // (Small) Memory leak 2
    '#createLink' : function(el){
	    new Draggable('createLink');
    },

    '#secondlist .weebly-splitpane-2' : function(el) {
	    var elId = $(el).id.replace(/-parent/, '');
	    if ($(el).parentNode) {
	      $(el).parentNode.className = 'element-panes';
	      $(el).parentNode.style.padding = '0';
	      $(el).parentNode.style.margin = '0';
	      /*
	      $(el).parentNode.parentNode.parentNode.childNodes[1].style.backgroundColor = '#25b82c';
	      $(el).parentNode.parentNode.parentNode.style.border = '1px solid #66b86a';
	      */
	      $(el).parentNode.ondblclick = function() { };
	    }

	    // Make sure that there isn't already a SplitPane created for this div
	    if ((!splitpanes[elId] || (splitpanes[elId] && splitpanes[elId].container != $(el))) && $(elId+'-lhs-width')) {
	      var sp = new SplitPane(elId+"-lhs", $(elId+'-lhs-width').value, elId+"-rhs", $(elId+'-lhs-width').value, $(elId+'-rhs-width').value, { 
	        onEnd: function(splitPane, event) { 
		  handlerDragEndSplitpane(splitPane, event, $(elId+'-lhs-cfpid').value+"-"+$(elId+'-rhs-cfpid').value, elId) 
	        }, 
	        active: true 
	      });
	      sp.set();
	      splitpanes[elId] = sp;
	    }
    },
    '#secondlist a' : function(el) {

	    //if (el.className == "keepLinkAlive") { return; }
	    if (!el.onclick) { el.onclick = function() { return false; } }
	    el.title = "Links active once published";

    },
    '#footerCode a' : function(el) {

            if (!el.onclick) { el.onclick = function() { return false; } }
            el.title = "Links active once published";
    },
    '#secondlist .disable-link' : function (el) {

	    if (!el.onclick) { el.onclick = function() { return false; } }
	    el.title = "";

    },

    '#secondlist img' : function(el) {

	var _match = el.src.match(/.+\/(.+)$/),
		imgSrc = _match[1];
	if (imageResizeId[imgSrc]) {
	  el.src.match(/.+\?(.+)$/);
	  var oldId = _match[1];
          var newId = imageResizeId[imgSrc];

	  if (oldId != newId) {
            el.src = el.src + "?" + newId;
	  }
	}

    },
    '#secondlist .blog-link' : function(el) {
	    el.onclick = function() { goBlogPage(el.pathname); return false; }
	    el.title   = "";
    },
    '#secondlist #blog-date' : function(el) {

	    if (currentBlog.postId != 0) {

              var currentDateFormat = "%m/%d/%Y";
              if ($('blogDateFormat') && $('blogDateFormat').innerHTML.match('eu')) {
                currentDateFormat = "%d/%m/%Y";
              }

	      Calendar.setup({dateField: 'blog-date', selectHandler: calendarCallback, dateFormat: currentDateFormat});
	    }

    },
    '#secondlist .no-title' : function (el) {
	    el.title   = "";
    },

    '#icontent .weebly_header' : function(el) {
	    el.title   = "Click to replace";
    },

    /**
    '#themeOptions .dropDownData' : function(el) {
	    if (el.nextSibling && el.nextSibling.nextSibling && el.nextSibling.nextSibling.className == "weeblyDropDown") { return; }
	    new Weebly.DropDown(el.nextSibling, {openWidth: 95, rowFunction: function(x) { return generateDropDownRow(x, el.innerHTML); }});
    },
    **/

// MySpace styles
    '#myspaceListLeft' : function (el) {
	    //contentDraggables.push('myspaceListLeft');
	    //Sortable.create('myspaceListLeft', {hoverclass: 'hoverclass', dropOnEmpty:true, handle: 'handlebar_left', scroll:'scroll_container', scrollSensitivity:50, scrollSpeed:5, containment: ['elementlist','myspaceListLeft','myspaceListRight','myspaceAboutMeList','myspaceLikeToMeetList'], onUpdate: updateList});
    },
    '#weebly_boxLeft .weebly_box_left_top' : function (el) {
            el.onmouseover = function() { myspace_mouseOver(el); }
            el.onmouseout  = function() { myspace_mouseOut(el); }
    },
    '#weebly_boxLeft .handlebar_left' : function (el) {
            el.onmouseover = function() { myspace_mouseOver(el.parentNode); }
    },

    '#myspaceListRight' : function (el) {
	    //contentDraggables.push('myspaceListRight');
	    Sortable.create('myspaceListRight', {hoverclass: 'hoverclass', dropOnEmpty:true, handle: 'handlebar_right', scroll:'scroll_container', scrollSensitivity:50, scrollSpeed:5, containment: ['elementlist','myspaceListRight'], onUpdate: updateList});
    },
    '#weebly_boxRight .weebly_box_right_top' : function (el) {
            el.onmouseover = function() { myspace_mouseOver(el); }
            el.onmouseout  = function() { myspace_mouseOut(el); }
    },
    '#weebly_boxRight .handlebar_right' : function (el) {
            el.onmouseover = function() { myspace_mouseOver(el.parentNode); }
    },
    '#secondlist .updateBackgroundImage' : function(el) {
	    //refreshBackgroundImage(el);
    },
    '#icontent_container' : function(el) {
	    //refreshBackgroundImage(el);
    },

    '#scroll_container' : function(el) {
	    Event.observe(el, 'mouseover', hoverHandler);
    },
    '.product-title' : function(el){
        el.onclick = function(){showEditBox(el.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':true})};
    },
    '.product-price' : function(el){
        el.onclick = function(event){showEditBox(el.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':false})};
    },
    '.product-description' : function(el){
        el.onclick = function(event){showEditBox(el.id, null, {'saveCallback':saveProductField, 'redrawOptions':true, 'showOptions':true})};
    },
    '.product form' : function(el){
        el.onsubmit = function(){return false;}
    },
    '.element img' : setupFileUploader,
    '.element iframe' : setupFileUploader,
    '.formlist' : function(el){
        if($(el.id+'-form-cover')){
            $(el.id+'-form-cover').remove();
        }
        if(!Weebly.Elements.highlightedElement && !$('blog-editor-post')){
            var cover = new Element('div', {'id':el.id+'-form-cover', 'title':'Click on the form to add or edit the fields'});
            cover.setStyle({position:'absolute', opacity:0, background:'#FFFFFF', top:'0px', left:'0px', height:(el.getHeight()+10)+'px', width:(el.getWidth()+10)+'px', cursor:'pointer', zIndex:'5'});
            el.insert({after:cover});
            cover.observe('click', function(){if(!deleting){Weebly.Elements.selectElement(el.up('.element'));}});
        }

        el.select('.show-element-options-image').invoke('remove');
    },

// Last statement
    '#secondlist' : function(el){
            pushTwoColumnsDraggables();
            if(!Weebly.Elements.highlightedElement){
                contentDraggables.push(el);
            }
            initDraggables();
            Weebly.Form.fieldInstructionsHandler();
            //Sortable.create("secondlist", {hoverclass: 'hoverclass', dropOnEmpty:true,handle:'handle',containment:["secondlist","elementlist","trashlist"],constraint:false,onUpdate:updateList});
    }
    
};

/***************Register Rules*************/
Behaviour.register(myrules);


/********Resgister additional events*******/
Behaviour.addLoadEvent(function() {
        //Effect.Appear('tip1');
        //updateList();
}); 

//Behaviour.apply();



var oldTheme, newTheme, forkThemeDialog, advancedEditingDoneCallback;

function editCustomTheme(fullCustomThemeID, makeCopy, doneCallback)
{
	advancedEditingDoneCallback = doneCallback;
	newTheme = fullCustomThemeID;
	if (!isDesigner || !newTheme) {
		makeCopy = false;
	}
	//
	makeCopy = false; // always disable forking (for now)
	//
	if (typeof makeCopy == 'undefined') {
		if (!forkThemeDialog) {
			forkThemeDialog = new Weebly.Dialog('theme-fork-dialog', {
				zIndex: 1000,
				modal: true
			});
		}
		forkThemeDialog.open();
	}else{
		oldTheme = currentTheme;
		if (!fullCustomThemeID) {
			currentThemeTemp = currentTheme;
			currentTheme = 'blank';
		}else{
			currentTheme = fullCustomThemeID;
		}
		advancedEditing(makeCopy, doneCallback);
	}
}

function continueEditCustomTheme(makeCopy) // called by forkThemeDialog
{
	oldTheme = currentTheme;
	currentTheme = newTheme;
	advancedEditing(makeCopy);
}


var _advancedEditingForceCopy;

function advancedEditing(forceCopy)
{
	if (isDesigner && typeof forceCopy == 'undefined' && isCustomTheme(currentTheme)) {
		editCustomTheme(currentTheme);
		return;
	}
	_advancedEditingForceCopy = forceCopy;
    if(Weebly.Restrictions.hasAccess('custom_themes')){
        if( !$H(userEvents).get('viewCustomThemeWarning') )
        {
            if( confirm(/*tl(*/'Advanced Editing allows you to customize the layout and style of your Weebly theme.  This requires that you are familiar with HTML and CSS.  Weebly does not provide support for the HTML and CSS coding aspects of the Advanced Editor.\n\nClick "OK" to proceed to the Advanced Editing screen.'/*)tl*/ ) )
            {
                Pages.go('customThemeEditor');
            }
            new Ajax.Request(ajax, {parameters:'pos=doevent&event=viewCustomThemeWarning&cookie='+document.cookie});
            userEvents['viewCustomThemeWarning'] = 1;
        }
        else
        {
            Pages.go('customThemeEditor');
        }
    }
    else{
        if(typeof(upgrade_url) !== 'undefined'){
            window.open(upgrade_url+'?service='+Weebly.Restrictions.requiredService('custom_themes'), 'weebly_billingPage', 'height=550,width=910,menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
        }
        else{
            alertProFeatures('Please upgrade to edit your theme');
        }
    }
}

function openCustomThemeEditor() {
    $$('div.tab').each(function(e){e.setStyle({top: '14px'}); updateRoundedTabColor(e, '#E9E9E9');});
	hasUsedThemeEditor = 1;
	if (!ENABLE_THEME_BROWSER) {
		showThemes('All');
		highlightThemeCategory($('tc_all'));
	}
	$('custom-theme-image-preview').update('');
	if (!ENABLE_THEME_BROWSER) {
		var cover = new Element( 'div', {'id': 'custom-designs-cover'} );
		cover.setStyle({background: '#222222', width: '100%', height: '134px', position: 'absolute', left: '0px', top: '0px', 'zIndex': 13, opacity: '0.8', borderTop: '1px solid black'});
		$('body').insert(cover);
	}
	new Ajax.Request(ajax, {parameters:'pos=getthemesrc&keys='+currentTheme+'&type=html&cookie='+document.cookie, onSuccess:handlerOpenCustomThemeEditor, onFailure:errFunc});
}

function continueOpenCustomThemeEditor()
{
    themeSelectTab($('themeEditHTML'));
	$('custom-theme-upload-box').hide();//Fix IE bug
	$('custom-theme-upload-box').show();
	if( _advancedEditingForceCopy || !isCustomTheme(currentTheme) )
	{
		new Ajax.Request(ajax, {parameters:'pos=setupnewcustomtheme&html='+encodeURIComponent(temphtml)+'&css='+encodeURIComponent(tempcss)+'&currentTheme='+currentTheme+'&cookie='+document.cookie, onSuccess:handlerSaveCustomTheme, onFailure:errFunc, asynchronous: false});
		//currentTheme = 'custom_'+currentEditingThemeName+'_'+currentEditingThemeID;
	}
	else
	{
		currentEditingThemeName = getCustomThemeName(currentTheme);
		currentEditingThemeID = getCustomThemeID(currentTheme);
	}
	//updateThemeRollbackPoint();
	
	new Ajax.Request(ajax, {parameters:'pos=getthemefiles&theme='+makeCustomThemeName(currentEditingThemeName, currentEditingThemeID)+'&cookie='+document.cookie, onSuccess:handlerGetThemeFiles, onFailure:errFunc});
	setupThemeFileUploader();
	var themeExportLink = $('theme-export-link');
	if (themeExportLink) {
		themeExportLink.writeAttribute( {href: '/weebly/exportThemeZip.php?id='+currentEditingThemeID} ); 
	}
    if( !$H(userEvents).get('viewCustomThemeHelpMessage') )
    {
        showTip('Click here for help customizing your theme', 'themeEditHelp', 'y');
        new Ajax.Request(ajax, {parameters:'pos=doevent&event=viewCustomThemeHelpMessage&cookie='+document.cookie});
        userEvents['viewCustomThemeHelpMessage'] = 1;
    }
}

function exitCustomThemeEditor()
{
	if( typeof(currentEditingThemeName) !== 'undefined' && currentEditingThemeName !== 'WEEBLY__UNSAVED' )
	{
		var custTheme = makeCustomThemeName(currentEditingThemeName, currentEditingThemeID);
		Weebly.Cache.insert( 'pos=gettheme&keys='+custTheme, 'empty' );
		Weebly.Cache.insert( 'pos=getthememenu&keys='+custTheme, 'empty' );
		currentStyleNum = Math.floor(Math.random()*10000000001);
		if (advancedEditingDoneCallback) {
			currentTheme = oldTheme;
		}else{
			if (oldTheme) {
				selectTheme(oldTheme);
			}else{
				selectTheme(custTheme);
			}
		}
	}
	else
	{
		if (!advancedEditingDoneCallback) {
			if (oldTheme) {
				currentTheme = oldTheme;
			}
			Pages.go('themesMenu');
		}
	}
	if (advancedEditingDoneCallback) {
		advancedEditingDoneCallback();
		advancedEditingDoneCallback = null;
	}
	oldTheme = null;
}

function saveAndExitThemeEditor(name)
{
	if( saveCustomTheme(name) )
	{
		exitCustomThemeEditor();
	}
}

function closeCustomThemeEditor()
{
	if( $('custom-designs-cover') )
	{
		$('custom-designs-cover').remove();
	}
	$('customizeTheme').show();
	delete currentEditingThemeName;
	delete currentEditingThemeID;
	delete pending_html;
	delete pending_css;
	delete rollback_html;
	delete rollback_css;
	if(currentTheme === 'blank')
	{
		currentTheme = currentThemeTemp;
	}
}

function updateThemeRollbackPoint()
{
	if( typeof(pending_html) === 'undefined' )
	{
		pending_html = themeEditBoxHTML.getCode();
		pending_css = themeEditBoxCSS.getCode();
		rollback_html = pending_html;
		rollback_css = pending_css;
	}
	else
	{
		rollback_html = pending_html;
		rollback_css = pending_css;
		pending_html = themeEditBoxHTML.getCode();
		pending_css = themeEditBoxCSS.getCode();
	}
}

function rollbackThemeChanges()
{
	themeEditBoxHTML.setCode(rollback_html);
	themeEditBoxHTML.editor.syntaxHighlight('init');
	themeEditBoxCSS.setCode(rollback_css);
	themeEditBoxCSS.editor.syntaxHighlight('init');
	delete pending_html;
	updateThemeRollbackPoint();
}

function addNewDesignPicture()
{
	if( typeof(currentEditingThemeName) !== 'undefined' && currentEditingThemeName !== 'WEEBLY__UNSAVED' )
	{
		var custTheme = makeCustomThemeName(currentEditingThemeName, currentEditingThemeID);
		var displayName = currentEditingThemeName;
		
		function htmlEscape(s) {
			return s
				.replace(/&/g, '&amp;')
				.replace(/</g, '&lt;')
				.replace(/>/g, '&gt;')
				.replace(/'/g, '&#039;')
				.replace(/"/g, '&quot;');
		}
		
		showThemes('All');
		
		// completely ripped from theme_browser/includes/templates/weebly_editor_list.php
		$('themePicturesInner').insert({
			top: "<a class='themePicture' href='#' onclick=\"selectTheme('" + custTheme + "');return false\" " +
				"style='float:left;border:0px;text-decoration:none' " +
				"onmouseover=\"this.className='themePicture themePicture-hover';themeOver('" + custTheme + "')\" " +
				"onmouseout=\"this.className='themePicture';themeOut('" + custTheme + "')\"> " +
					"<div class='themePictureImg' id='theme_" + custTheme + "'>" +
						"<div class='themePicturePlaceholder'>" +
							"<div><span>" + htmlEscape(displayName) + "</span></div>" +
						"</div>" +
					"</div>" +
					"<span class='themeFavoriteIcon themeIsFavorite' " +
						"onmouseover='mouseOverFavorite=true' " +
						"onmouseout='mouseOverFavorite=false'" +
						"></span>" +
				"</a>"
		});
        
		if (ENABLE_THEME_BROWSER) {
			var a = $$('#themePicturesInner a')[0];
			initThemeFavoriting(custTheme, a.select('span.themeFavoriteIcon')[0], a, 'themeIsFavorite');
		}else{
			Weebly.Cache.insert('pos=getthemes&keys=', $('themePicturesInner').innerHTML);
		}
		
	}
}

function isCustomTheme( themeName )
{
	return themeName.substr( 0, 6 ) === 'custom';
}

function makeCustomThemeName( name, id )
{
	return 'custom_'+name+'_'+id;
}

function getCustomThemeID( themeName )
{
	return themeName.replace( /custom_.*_(.*)$/, '$1' );
}

function getCustomThemeName( themeName )
{
	return themeName.replace( /custom_(.*)_.*$/, '$1' );
}

function handlerOpenCustomThemeEditor(t) {
    if(typeof(themeEditorHTML) === 'undefined'){
        $('themeEditBoxHTML').value = t.responseText;
        themeEditorHTML = CodeMirror.fromTextArea('themeEditBoxHTML', {
            height: "100%",
            parserfile: ["parsexml.js", "parsecss.js", "tokenizejavascript.js", "parsejavascript.js", "parsehtmlmixed.js"],
            stylesheet: "libraries/codemirror/css/xmlcolors.css",
            path: "libraries/codemirror/js/",
            continuousScanning: 500,
            lineNumbers: true,
            textWrapping: false
        });
    }
    else{
        themeEditorHTML.setCode(t.responseText);
    }

    temphtml = t.responseText;

	$('customizeTheme').hide();

	$('customThemeContainer').style.visibility = "visible";
	new Ajax.Request(ajax, {parameters:'pos=getthemesrc&keys='+currentTheme+'&type=css&cookie='+document.cookie, onSuccess:handlerOpenCustomThemeEditor2, onFailure:errFunc});
}

function handlerOpenCustomThemeEditor2(t) {
    if(typeof(themeEditorCSS) === 'undefined'){
        $('themeEditBoxCSS').value = t.responseText;
        themeEditorCSS = CodeMirror.fromTextArea('themeEditBoxCSS', {
            height: "100%",
            parserfile: ["parsecss.js"],
            stylesheet: "libraries/codemirror/css/csscolors.css",
            path: "libraries/codemirror/js/",
            continuousScanning: 500,
            lineNumbers: true,
            textWrapping: false
        });
    }
    else{
        themeEditorCSS.setCode(t.responseText);
    }
    
    themeEditorCSS.hide();
	
	tempcss = t.responseText;
	
	continueOpenCustomThemeEditor();
}

function handlerGetThemeFiles(t)
{
	$('theme-files').update('');
	var files = t.responseText.evalJSON();
	files.each( 
		function(file){
			var el = new Element( 'option', { 'class': 'theme-file' } ).update( file );
			$('theme-files').insert( {bottom: el} );
		}
	);
}

function themeSelectTab(tab) {
	$$('div.tab').each(function(e){e.setStyle({top: '14px'}); updateRoundedTabColor(e, '#E9E9E9');});
	$(tab).setStyle({top: '15px'});
	updateRoundedTabColor($(tab), '#FFFFFF');

    themeEditorHTML.hide();
    themeEditorCSS.hide();
	$('themeEditBoxFiles').hide();
    $('themeEditBoxPreview').hide();
    $('themeEditBoxHelp').hide();
	var id = 'themeEditBox' + $(tab).identify().substr(9);
	if( id === 'themeEditBoxHTML' )
	{
		themeEditorHTML.show();
	}
    else if( id === 'themeEditBoxCSS' )
	{
		themeEditorCSS.show();
	}
	else
	{
        if(id === 'themeEditBoxHelp' && !$(id).down('iframe')){
            $(id).update('<iframe frameborder="0" src="http://customthemes.weebly.com" style="width:100%; height:100%; border:none;"></iframe>');
        }
		$(id).show();
	}
}

function updateRoundedTabColor(tab, color)
{
	if( tab.getStyle('background') || ( tab.getStyle('backgroundColor') && tab.getStyle('backgroundColor')!='transparent' ) )
	{
		tab.setStyle({background: color});
	}
	tab.childElements().each( function(e){updateRoundedTabColor(e, color );} );
}

function saveCustomTheme(name)
{
	var newTheme = 0;
	if( currentEditingThemeName == 'WEEBLY__UNSAVED' )
	{
		newTheme = 1;
		if( typeof(name) == 'undefined' || name.length === 0 )
		{
			drawSaveThemeNameBox();
			return false;
		}
	}
	else
	{
		name = currentEditingThemeName;
	}
	
	if( $('theme-save-name-box') )
	{
		$('theme-save-name-box').remove();
	}
	var html = themeEditorHTML.getCode();
	var css = themeEditorCSS.getCode();
	
	new Ajax.Request(ajax, {parameters:'pos=savecustomtheme&name='+name+'&newTheme='+newTheme+'&html='+encodeURIComponent(html)+'&css='+encodeURIComponent(css)+'&currentTheme='+currentTheme+'&cookie='+document.cookie, onFailure:errFunc, onSuccess:handlerSaveCustomTheme, asynchronous: false});
	return true;
}

function handlerSaveCustomTheme(t)
{
	var response = t.responseText;
	if( response.isJSON() )
	{
		var themeData = response.evalJSON();
		if( typeof(currentEditingThemeName) !== 'undefined' && currentEditingThemeName != themeData.theme_name )
		{
			//new Ajax.Request(ajax, {parameters:'pos=settheme&keys=custom_'+themeData.theme_name+'_'+themeData.custom_theme_id+'&cookie='+document.cookie, onFailure:errFunc, bgRequest: true});
			currentEditingThemeName = themeData.theme_name;
			currentEditingThemeID = themeData.custom_theme_id;
			addNewDesignPicture();
		}
		else
		{
			currentEditingThemeName = themeData.theme_name;
			currentEditingThemeID = themeData.custom_theme_id;
		}
	}
}

function deleteCustomTheme(themeName, squareElement)
{
	if( confirm('Are you sure you want to delete this theme?') )
	{
		deletedThemeName = themeName;
		var themeID = getCustomThemeID( themeName );
		new Ajax.Request(ajax, {parameters:'pos=deletecustomtheme&customThemeID='+themeID+'&cookie='+document.cookie, onFailure:errFunc, onSuccess: handlerDeleteCustomTheme});
	}
}

function handlerDeleteCustomTheme(t)
{
	showThemes('All');
	highlightThemeCategory($('tc_all'));
	if( $('theme_'+deletedThemeName) ) 
	{
		var pic = $('theme_'+deletedThemeName).up();
		pic.remove();
		if (!ENABLE_THEME_BROWSER) {
			Weebly.Cache.insert( 'pos=getthemes&keys=', $('themePicturesInner').innerHTML );
		}
	}
	if( currentTheme == deletedThemeName )
	{
		currentTheme = Weebly.defaultTheme;
		selectTheme(Weebly.defaultTheme);
	}
}

function drawSaveThemeNameBox()
{
	var saveButtonPos = $('theme-save-button').cumulativeOffset();
	var box = new Element( 'div', {id: 'theme-save-name-box', 'class': 'theme-save-name-box-class'} );
	box.setStyle( {left: (saveButtonPos.left-265)+'px'} );
	box.update( '<form style="margin-top: 4px; margin-left:15px;" onsubmit="saveAndExitThemeEditor($F(\'save-theme-name\')); return false;"><table><tr><td>Save As: </td><td><input id="save-theme-name" style="border: 1px solid #9A9A9A; height:22px; width:150px; font-size:16px;" type="text" value="MyTheme" /></td><td><input type="image" src="images/save_theme_blue.jpg" value="Save" /></td><td><img src="images/cancel_button.jpg" style="cursor: pointer;" onclick="$(\'theme-save-name-box\').remove()" /></td></tr></table></form>' );
	$('customThemeContainer').insert( {bottom: box} );
	$('save-theme-name').select();
	
	var settings = {tl: { radius: 5 },tr: { radius: 5 },bl: { radius: 5 },br: { radius: 5 },antiAlias: true,autoPad: false};
	var corners = new curvyCorners(settings, "theme-save-name-box-class");
	corners.applyCornersToAll();
	$('theme-save-name-box').style.padding = "0 0 7px 0";
}

function drawRenameBox()
{
	var selected = getSelectedThemeFile();
	if( selected )
	{
		currentSelectedThemeFile = $F('theme-files');
		var topoffset = selected.viewportOffset().top - 257;
		var leftoffset = selected.positionedOffset().left;
        var width = selected.getWidth();
		topoffset = getSelectedThemeFileTopOffset();//selected.positionedOffset().top - selected.cumulativeScrollOffset().top;// + getSelectedThemeFileTopOffset() + 0;
        leftoffset = selected.positionedOffset().left;
		var input = new Element( 'input', {type: 'text', value: $F('theme-files') } );
		input.setStyle({border: '1px solid #000000', height: '15px', width: '206px', padding: '0px', position: 'absolute', top: topoffset+'px', left:leftoffset+'px', zIndex: '20'} );
		input.observe( 'blur', handleRenameEvent);
		input.observe( 'keypress', function(event){if(event.keyCode==Event.KEY_RETURN){handleRenameEvent(event);}});
		$('custom-theme-upload-box').insert( {bottom: input} );
		input.select();
	}
}

function handleRenameEvent(event)
{
	var el = Event.element(event);
	var newName = el.value;
	if( newName != currentSelectedThemeFile )
	{
		new Ajax.Request(ajax, {parameters:'pos=renamethemefile&customThemeID='+currentEditingThemeID+'&oldName='+currentSelectedThemeFile+'&newName='+newName+'&cookie='+document.cookie, onSuccess:function(t){handlerGetThemeFiles(t);el.remove();}, onFailure:errFunc});
	}
	else
	{
		el.remove();
	}
}

function getSelectedThemeFile()
{
	var options = $$('option.theme-file');
	for( var i=0; i<options.size(); i++ )
	{
		if( options[i].selected )
		{
			return options[i];
		}
	}
	return false;
}

function getSelectedThemeFileTopOffset()
{
	var options = $$('option.theme-file');
    var multiplier = Prototype.Browser.IE ? 16 : 17;
	for( var i=0; i<options.size(); i++ )
	{
		if( options[i].selected )
		{
			return (multiplier * (i+1)) - options[i].cumulativeScrollOffset().top + (Prototype.Browser.IE ? 1 : 0);
		}
	}
	return false;
}

function updateSavedCustomThemes()
{
	new Ajax.Request(ajax, {parameters:'pos=getsavedthemes&cookie='+document.cookie, onFailure:errFunc, onSuccess:handleUpdateSavedCustomThemes});
}

function handleUpdateSavedCustomThemes(t)
{
	$('savedCustomThemes').update('');
	var themes = t.responseText.evalJSON(true);
	for( var i=0; i<themes.size(); i++ )
	{
		var li = new Element('li', {'class': 'savedThemeName'}).update(themes[i].theme_name);
		li.observe('click', function(e){$('themeEditName').value=Event.element(e).innerHTML;} );
		$('savedCustomThemes').insert({bottom: li});
	}
}

function updateThemePreview()
{
	var html = themeEditorHTML.getCode();
	var css = themeEditorCSS.getCode();
	new Ajax.Request('/weebly/apps/preview.php', {parameters:'themeID='+currentEditingThemeID+'&userID='+userID+'&siteID='+currentSite+'&pageID='+currentPage+'&template='+encodeURIComponent(html)+'&css='+encodeURIComponent(css)+'&cookie='+document.cookie, onFailure:errFunc, onSuccess:handlerUpdateThemePreview});
}

function handlerUpdateThemePreview(t)
{
	var e = $('customThemePreview');
	e.contentWindow.document.close();
	e.contentWindow.document.open();
	e.contentWindow.document.write(t.responseText);
	updateThemeRollbackPoint();
}

function deleteThemeFile(fileName)
{
	new Ajax.Request(ajax, {parameters:'pos=deletethemefile&customThemeID='+currentEditingThemeID+'&file='+encodeURIComponent(fileName)+'&cookie='+document.cookie, onFailure:errFunc, onSuccess:handlerGetThemeFiles});
}

function updateCustomThemeImagePreview()
{
	if( $F('theme-files') )
	{
		var ext = $F('theme-files').slice(-3).toLowerCase();
		if( ext === 'gif' || ext === 'png' || ext === 'jpg' )
		{
			var imagePath = getCustomThemePath(currentEditingThemeID)+'files/'+$F('theme-files')+'?'+Math.ceil(Math.random()*100000);
			var img = new Image();
			$(img).observe( 'load', updateCustomThemeImagePreviewDimensions);
			img.src = imagePath;
			$('custom-theme-image-preview').update( '<div style="text-align:left; width: 225px; color:#666666; font-size:11px;"><b>'+/*tl(*/'Preview'/*)tl*/+':</b><br /><div style="text-align:center; margin-top:3px;"><a href="'+imagePath+'" target="_blank"><img id="theme-file-preview-image" style="max-width:213px; max-height:63px; min-width:1px; min-height:1px" src='+imagePath+' /></a></div><table style="margin-left:40px;"><tr><td style="color:#000000; font-weight:bold; text-align:right">'+/*tl(*/'Name'/*)tl*/+'</td><td>'+$F('theme-files')+'</td></tr><tr><td style="color:#000000; font-weight:bold; text-align:right">'+/*tl(*/'Dimensions'/*)tl*/+'</td><td id="theme-file-preview-dimensions"></td></tr></table></div>' );
		}
		else
		{
			$('custom-theme-image-preview').update( '<div style="text-align:left; clear:both; color:#666666; font-size:11px; font-weight: bold;">'+/*tl(*/'Preview not available for this file type.'/*)tl*/+'</div>' );
		}
	}
}

function updateCustomThemeImagePreviewDimensions(event)
{
	$('theme-file-preview-dimensions').update( this.width + ' x ' + this.height );
	if( !$('theme-file-preview-image').getStyle('maxHeight') )//Fix IE 6
	{
		if( $('theme-file-preview-image').getHeight() > 63 )
		{
			$('theme-file-preview-image').setStyle({'height':'63px'});
		}
		if( $('theme-file-preview-image').getWidth() > 213 )
		{
			$('theme-file-preview-image').setStyle({'width':'213px'});
		}
	}
}

function getCustomThemePath(themeID)
{
	var path = '/uploads/';
	for( var i = 0; i<4; i++ )
	{
		var charac = userID.charAt(i);
		if( charac )
		{
			path = path + charac + '/';
		}
		else
		{
			path = path + '_/';
			break;
		}
	}
	return path + userID + '/custom_themes/' + themeID + '/';
}

function showCustomThemes()
{
	if (!ENABLE_THEME_BROWSER) {
		showThemes('All');
		$('themePicturesInner').childElements().each(
			function(e){
				if(!e.down().id.match('custom'))
				{
					e.remove();
				}
			}
		);
	}

    var overlaySrc = typeof(proElementOverlaySrc) != 'undefined' ? proElementOverlaySrc : 'images/pro-element-overlay.png';
	var pic = new Element( 'a', 
		{href: '#', 
		'class': 'themePicture'} 
	);
	pic.observe('click', 
		function(event){
            if(Weebly.Restrictions.hasAccess('custom_themes')){
                currentThemeTemp=currentTheme;
                currentTheme='blank';
                Pages.go('customThemeEditor'); return false;
            }
            else{
                if(typeof(upgrade_url) !== 'undefined'){
                    window.open(upgrade_url+'?service='+Weebly.Restrictions.requiredService('custom_themes'), 'weebly_billingPage', 'height=550,width=910,menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
                }
                else{
                    alertProFeatures('Please upgrade to create a theme');
                }
            }
		}
	);
	pic.setStyle( {'float': 'left', border: '0px', textDecoration: 'none'} );
	var div = new Element( 'div', {id: 'theme_blank', 'class': 'themePictureImg'} );
	div.setStyle({width: '85px', height: '85px', background: 'url(/weebly/images/new_theme.jpg)', position:'relative'});
    if(Weebly.Restrictions.requiresUpgrade('custom_themes')){
        div.update('<img src="'+overlaySrc+'" class="pro-element-overlay">');
    }
	pic.insert( div );
	$('themePicturesInner').insert( {top: pic} );
	
	var pic = new Element( 'a', 
		{href: '#', 
		'class': 'themePicture'} 
	);
	pic.observe('click',
		function(event){
            if(Weebly.Restrictions.hasAccess('import_theme')){
                Pages.go('importTheme');
            }
            else{
                if(typeof(upgrade_url) !== 'undefined'){
                    window.open(upgrade_url+'?service='+Weebly.Restrictions.requiredService('import_theme'), 'weebly_billingPage', 'height=550,width=910,menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
                }
                else{
                    alertProFeatures('Please upgrade to import your theme');
                }
            }
			return false;
		}
	);
	pic.setStyle( {'float': 'left', border: '0px', textDecoration: 'none'} );
	var div = new Element( 'div', {id: 'theme_blank', 'class': 'themePictureImg'} );
	div.setStyle({width: '85px', height: '85px', background: 'url(/weebly/images/import.jpg)', position:'relative'});
    if(Weebly.Restrictions.requiresUpgrade('custom_themes')){
        div.update('<img src="'+overlaySrc+'" class="pro-element-overlay">');
    }
	pic.insert( div );
	$('themePicturesInner').insert( {top: pic} );
}

function setupThemeFileUploader()
{
    if(typeof(themeFileUploader) === 'undefined'){
        var uploadSettings = {
            upload_url: "/weebly/themeFileUpload.php?customThemeID="+currentEditingThemeID+"&cookies="+escape(document.cookie),
            flash_url : "/weebly/libraries/swfupload/swfupload.swf",
            file_size_limit : maxFileSize,
            file_types : "*.*",
            file_types_description : "All files...",
            button_image_url: "../.."+/*tli(*/"/weebly/images/upload_button.png"/*)tli*/,	// Relative to the Flash file
            button_width: "91",
            button_height: "24",
            button_placeholder_id: "theme-file-upload-button",
            file_dialog_complete_handler : themeFileDialogComplete,
            file_queued_handler : themeFileQueued,
            upload_start_handler: themeFileUploadStart,
            upload_progress_handler : themeFileUploadProgress,
            upload_success_handler : themeFileUploadSuccess,
            upload_error_handler : themeFileUploadError,
            upload_complete_handler : themeFileUploadComplete
        };
        themeFileUploader = new SWFUpload(uploadSettings);
    }
}

function themeFileDialogComplete()
{
	themeFileUploader.startUpload();
}

function themeFileQueued(file)
{
	var el = new Element( 'div', {'class': 'theme-queue-item', id: 'upload-item-'+file.id} ).update( file.name );
	el.insert( {bottom: ' <span id="upload-status-'+file.id+'"></span><div class="theme-progress-outer"><div class="theme-progress-inner" id="progress-inner-'+file.id+'"></div></div>'} );
	$('theme-upload-queue').insert( {bottom: el} );
}

function themeFileUploadStart(file)
{
	themeFileUploader.setUploadURL("/weebly/themeFileUpload.php?customThemeID="+currentEditingThemeID+"&cookies="+escape(document.cookie));
}

function themeFileUploadProgress(file, bytesComplete, totalBytes)
{
	var percentComplete = Math.floor( 100 * bytesComplete / file.size );
	$('progress-inner-'+file.id).setStyle( {width: percentComplete+'%'} );
}

function themeFileUploadError(file, errorCode, message)
{
	$('upload-status-'+file.id).update('Error: upload failed');
}

function themeFileUploadSuccess( file, data )
{
	//$('upload-status-'+file.id).update('Upload complete');
	if( data.isJSON() )
	{
		handlerGetThemeFiles({responseText: data});
	}
}

function themeFileUploadComplete(file)
{
	Effect.BlindUp('upload-item-'+file.id, {queue: 'end'});
	themeFileUploader.startUpload();
}

function displayCustomThemeImport()
{
	
}

function setupThemeImporter()
{
	var uploadSettings = {
		upload_url: "/weebly/themeImport.php?cookies="+escape(document.cookie),
		flash_url : "/weebly/libraries/swfupload/swfupload.swf",
		file_size_limit : maxFileSize,
        file_types : "*.zip",
        file_types_description : "Zip Archives",
        button_image_url: "../.."+/*tli(*/"/weebly/images/upload_theme_button_states.png"/*)tli*/,	// Relative to the Flash file
		button_width: "116",
		button_height: "30",
		button_placeholder_id: "theme-import-button",
		file_dialog_complete_handler : themeImportDialogComplete,
		upload_start_handler: themeImportUploadStart,
		upload_progress_handler : themeImportUploadProgress,
		upload_success_handler : themeImportUploadSuccess,
		upload_error_handler : themeImportUploadError,
		upload_complete_handler : themeImportUploadComplete
	};
	themeImportUploader = new SWFUpload(uploadSettings);
}

function themeImportDialogComplete()
{
	themeImportUploader.startUpload();
}

function themeImportUploadStart(file)
{
	$('theme-import-status').update('File: '+file.name+'<br />'+/*tl(*/'Status'/*)tl*/+': <span id="theme-import-status-short">'+/*tl(*/'Uploading'/*)tl*/+'</span><div class="theme-progress-outer"><div class="theme-progress-inner" id="import-progress-inner-'+file.id+'"></div></div><div id="theme-import-status-long"></div>');
}

function themeImportUploadProgress(file, bytesComplete, totalBytes)
{
	var percentComplete = Math.floor( 100 * bytesComplete / file.size );
	$('import-progress-inner-'+file.id).setStyle( {width: percentComplete+'%'} );
}

function themeImportUploadError(file, errorCode, message)
{
	$('theme-import-status-long').update(/*tl(*/'Error: upload failed'/*)tl*/);
}

function themeImportUploadSuccess( file, data )
{
	$('theme-import-status-short').update('Upload complete');
	if( data.isJSON() )
	{
		var response = data.evalJSON();
		if( response.status === 'Success' )
		{
			currentEditingThemeName = response.name;
			currentEditingThemeID = response.id;
			addNewDesignPicture();
			exitCustomThemeEditor();
		}
		$('theme-import-status-long').update(response.status+': '+response.message);
	}
}

function themeImportUploadComplete(file)
{
}

var currentBox    = String();
var currentTop    = 0;
var currentLeft   = 0;
var currentAlign  = '';
var colorBoxShown = 0;
var isPaste	  = 0;
var initBoxes     = new Array;
var timeOuts      = 0;
var currentSel    = new Object;
var initializingBox = null;
var lastSave	  = null;
var iframeEl 	  = null;
var editBoxSaveCallback  = null;
var editBoxRedrawOptions = false;
var editBoxShowOptions = true;
                                                                                                                             
function hideEditBox(name, tries) {

      // If box is still being initalized       
      if (initializingBox) { 
	if (!tries) { tries = 0; }
	if (tries < 5) {
	  setTimeout("hideEditBox('"+name+"', "+(tries+1)+");", 500); 
	  return;
	}
      }

      if (document.getElementById(name) && document.getElementById(name+'Edit') && currentBox) {

	// Check that the iframe is still the same before saving text
	// This helps an issue with Firefox and the back button where text gets "whacked"
	if(document.getElementById(name+'Edit') && document.getElementById(name+'Edit').contentWindow && document.getElementById(name+'Edit').contentWindow.document.body.id == 'icontent') {

	  var cleanText = clean();

	  document.getElementById(name).innerHTML = cleanText;

      if('function' === typeof(editBoxSaveCallback)){
          editBoxSaveCallback(name, cleanText, currentAlign);
      }
	  else if (name != 'weebly_site_title') {
	    saveContent(name,cleanText,currentAlign);

	  } else {
	    updateSiteTitle(cleanText);
	  }

	}

	var element = document.getElementById(name);
        Element.setStyle(document.getElementById(name+'Edit'), {'display':'none'} );
        //Element.setStyle(element, {'display':'block'} );
        Element.show(element);

	// If there's a picture before it, un-gray out picture
	if (element.previousSibling && element.previousSibling.previousSibling && element.previousSibling.previousSibling.childNodes[0] && element.previousSibling.previousSibling.childNodes[0].childNodes[0] && element.previousSibling.previousSibling.childNodes[0].childNodes[0].tagName == "IMG") {
	  if (!navigator.appVersion.match("MSIE")) {
            var imageElement = element.previousSibling.previousSibling.childNodes[0].childNodes[0];
            $(imageElement).removeClassName("element-grayed");
            imageElement.style.position = "static";
	    $(name+'Edit').style.minHeight = "auto";
	  }
	}

        var activateCount = 0;
        var keepGoing = 0;
        var myEl = document.getElementById(name+'Edit').contentWindow.document;

        while (keepGoing == 0 && activateCount < 3) {
          try{
           myEl.designMode = 'Off';
           keepGoing = 1;
          } catch (e) {
           keepGoing = 0;
           activateCount++;
          }
        }

      }

      currentBox = null;
      Weebly.Elements.hideMenuBar();
      //Element.setStyle('colorpicker', {'display':'none'} );                
      Weebly.Linker.close();
      Behaviour.apply();
      if(editBoxRedrawOptions){
          Weebly.Elements.generateProperties(Weebly.Elements.currentElement);
      }

	if (name == 'weebly_site_title') {
		afterUpdateSiteTitle();
	}
	
	//alert("Hide edit box!"+name);

}
                                                                                                                             
function showEditBox(name, elementid, options) {
        
	//setTimeout('alert("Show edit box! '+name+', "+currentBox);', 500);

	if (name == currentBox) return;
        if (currentBox) hideEditBox(currentBox);

	initializingBox = 1;
        currentBox   = name;
    if( $(name) )
    {
       currentAlign = $(name).getStyle('textAlign'); 
    }
    else
    {
        currentAlign = '';
    }

    editBoxSaveCallback = null;
    if(options){
        handleEditBoxOptions(options);
    }
       
	// Create iframe?
	createIframe(name);

	try {
	  if( document.getElementById(name+'Edit').contentWindow.document.designMode != "On") { init(name); }
        } catch (e) { currentBox = null; }     	   

	// The order in which things are done varies for IE and FF
	// Firefox wants continueShowEditBox here
	if (navigator.appVersion.indexOf("MSIE") == -1) {
	  continueShowEditBox(name);
	}

}
function init(nameOfBox) {

	var self = this;

	//setTimeout("document.getElementById('"+nameOfBox+"Edit').contentWindow.document.designMode = 'On'",500);

	setTimeout("init2('"+nameOfBox+"')", 500);

}

function init2(nameOfBox) {

	var activateCount = 0;
	var keepGoing = 0;

	// Crap out if these elements don't exist anymore, for whatever reason
	if (!(document.getElementById(nameOfBox+'Edit') && document.getElementById(nameOfBox+'Edit').contentWindow)) { return false; }

	var myEl = document.getElementById(nameOfBox+'Edit').contentWindow.document;

	while (keepGoing == 0 && activateCount < 3) {
	  try{
	   myEl.designMode = 'On';
	   keepGoing = 1;
	  } catch (e) {
	   keepGoing = 0;
	   activateCount++;
	  }
	}

	if (typeof document.addEventListener == "function") {
	   myEl.addEventListener("keyup", function(e){self.keyUp(); return true;}, false);
           myEl.addEventListener("keydown", function(e){self.keyDown(); self.detectPaste(e); return true;}, false);
	}
	//initBoxes.push(nameOfBox);

        // Firefox is "ready" for the runCommand here
        // otherwise, spits out a nsComponentNotReady error later
        if (navigator.appVersion.indexOf("MSIE") == -1) {
          if (myEl.body.innerHTML.indexOf('Click here to edit.') > -1 || myEl.body.innerHTML.indexOf('This is your new blog post.') > -1 || myEl.body.innerHTML.indexOf('100.00') > -1) {
            document.getElementById(nameOfBox+'Edit').contentWindow.focus();
            try {
              selectAll();
            } catch(e) { setTimeout("selectAll();", 500); }
          }
        }

	// The order in which things are done varies for IE and FF
	// IE wants continueShowEditBox here
        if (navigator.appVersion.indexOf("MSIE") > -1) {
          continueShowEditBox(nameOfBox);
        }

}
function continueShowEditBox(name) {

	// Crap out if these elements don't exist anymore, for whatever reason
	if (!(document.getElementById(name) && document.getElementById(name+'Edit') && document.getElementById(name+'Edit').contentWindow)) { return false; }

	if(editBoxRedrawOptions){
        Weebly.Elements.hideMenuBar();
    }

    var origBox   = document.getElementById(name);
	var myEl    = document.getElementById(name+'Edit');
	var myFrame = document.getElementById(name+'Edit').contentWindow.document;
        var editBody = myFrame.getElementsByTagName('body');

        var boxSize   = Element.getDimensions(origBox);
	var fontSize  = Element.getStyle(origBox, 'font-size');

        // If there's a picture before it, gray out picture
	var element = origBox;
        if (element.previousSibling && element.previousSibling.previousSibling && element.previousSibling.previousSibling.childNodes[0] && element.previousSibling.previousSibling.childNodes[0].childNodes[0] && element.previousSibling.previousSibling.childNodes[0].childNodes[0].tagName == "IMG") {
	  if (!navigator.appVersion.match("MSIE")) {
	    var imageElement = element.previousSibling.previousSibling.childNodes[0].childNodes[0];
	    	$(imageElement).addClassName("element-grayed");
	    imageElement.style.position = "absolute";
	  }
        }


	//var textToWrite = "<html><head><meta http-equiv='content-type' content='text/html; charset=UTF-8' /></head><body id='icontent'><div id='placeholder'>"+"<"+tag+" id='contents'>"+document.getElementById(name).innerHTML+"</"+tag+"></div></body></html>";
	var textToWrite = "<html><head><meta http-equiv='content-type' content='text/html; charset=UTF-8' /></head><body id='icontent'>"+origBox.innerHTML+"</body></html>";
        
	//myFrame.clear();
	myFrame.open();
	myFrame.write(textToWrite);
	myFrame.close();

	duplicateStyle(origBox, myFrame.getElementById('icontent'), myEl);
        $(name+'Edit').style.display = "block";
        $(name+'Edit').style.zIndex = 11;
        if(!Prototype.Browser.IE){
            var fs = $(origBox).getStyle('fontSize').replace('px', '');
            var lh = $(origBox).getStyle('lineHeight').replace('px', '');
            if(fs && lh){
                var newHeight = ''+(lh/fs);
                $(name+'Edit').style.lineHeight = newHeight;
                myFrame.getElementById('icontent').style.lineHeight = newHeight;
            }
        }

	// Set the width and margin for the boxes
        Element.setStyle(name+'Edit', {'height':(boxSize.height-2)+'px', 'width':(boxSize.width)+'px', 'border':'1px dashed #4455AA'} );
	if (!navigator.appVersion.match("MSIE")) {
          Element.setStyle(name+'Edit', {'maxWidth':'inherit'} );
	}
	var margins = [Element.getStyle(origBox, 'margin-top'), Element.getStyle(origBox, 'margin-bottom'), Element.getStyle(origBox, 'margin-left'), Element.getStyle(origBox, 'margin-right')];
	Element.setStyle(name+'Edit', {'margin-top':margins[0], 'margin-bottom':margins[1], 'margin-left':margins[2], 'margin-right':margins[3]} );

	if (imageElement) {
	  $(name+'Edit').style.minHeight = imageElement.getHeight()+parseInt(imageElement.style.marginBottom)+"px";
	}

	// Position the menu
	/**
	var menuPos  = new Position.cumulativeOffset(myEl);
	var topPos   = (menuPos[1]-160) - document.getElementById('icontent_container').offsetHeight;
        Element.setStyle('editMenu', {'display':'block', 'top':(menuPos[1]-135)+'px', 'left':(menuPos[0]+0)+'px'} );
	currentTop  = menuPos[1]-33;
	currentLeft = menuPos[0]+5;
	**/

	if (name != 'weebly_site_title' && editBoxShowOptions) {

	  // If element isn't a DIV, hide bulleted and numbered list buttons
	  if ($(name).tagName != "DIV") {
	    $('menuitem-ul').style.display = 'none';
	    $('menuitem-ol').style.display = 'none';
	    $('editMenuChild').style.width = '397px';
	  } else {
	    $('menuitem-ul').style.display = 'block';
	    $('menuitem-ol').style.display = 'block';
	    $('editMenuChild').style.width = '460px';
	  }

          hideFlashContainer();
          $('editMenu').style.display = "block";
	} else {
	  var par = $(name+'Edit').parentNode; 
	  if (!par.style.width) {
	    //par.style.width = "100%";
	    //par.style.display = "block";
	  }
	  //$(name+'Edit').style.width = ($(name+'Edit').style.width.replace(/px/, "")-(-25))+"px";
	  $(name+'Edit').style.minWidth = $(name+'Edit').style.width;
	  $(name+'Edit').contentWindow.document.body.focus();
	  //$(name+'Edit').style.styleFloat = "none";
	  //$(name+'Edit').style.clear = "both";
	}

        if ((textToWrite.indexOf('Click here to edit.') > -1 || textToWrite.indexOf('This is your new blog post.') > -1) && navigator.appVersion.indexOf("MSIE") > -1) {
          document.getElementById(name+'Edit').contentWindow.focus();
          try {
            selectAll();
            setTimeout('selectAll()', 500);
          } catch(e) { }
        }

	// Show the iframe and hide the previous element
        Element.setStyle(origBox, {'display':'none'} );

	if (myFrame.attachEvent) {
	  myFrame.attachEvent("onkeyup", function(e){self.keyUp(); return true;}, false);
	  myFrame.attachEvent("onkeydown", function(e){self.keyDown(); self.detectPaste(e); return true;}, false);
	}
    Event.observe(myFrame, 'keydown', fixIENewLines);

	keyUp();

	// Add keyboard shortcuts
	shortcut.add("Ctrl+B", function() { runCommand(currentBox, 'Bold', null); }, {target: myFrame });
	shortcut.add("Ctrl+U", function() { runCommand(currentBox, 'Underline', null); }, {target: myFrame });
	shortcut.add("Ctrl+I", function() { runCommand(currentBox, 'Italic', null); }, {target: myFrame });

	// We'll call these 500ms "recovery time"
	setTimeout("initializingBox = null;", 500);

}
        
function createIframe(nameOfBox) {

	if (!$(nameOfBox)) { return; }

	// Create the iframe element if it's not there
	if (!$(nameOfBox+'Edit')) {

	  iframeEl = document.createElement('iframe');

	  if (navigator.appVersion.indexOf("MSIE") == -1) {
	    //iframeEl.src = 'javascript:void(0)';
	  }

	  iframeEl.id = nameOfBox+'Edit';
	  iframeEl.className = 'editable';

	  iframeEl.style.display = 'block';
	  iframeEl.style.cssFloat = 'none';
	  iframeEl.style.styleFloat = 'none';
	  iframeEl.style.clear = 'both';

          iframeEl.allowTransparency = true ;
          iframeEl.frameBorder = '0' ;
          iframeEl.scrolling = 'no' ;

	  Element.insert($(nameOfBox), { after: iframeEl});
	}

}
function handleEditBoxOptions(options){
    if('function' == typeof(options.saveCallback)){
        editBoxSaveCallback = options.saveCallback;
    }
    if('undefined' !== typeof(options.redrawOptions)){
        editBoxRedrawOptions = options.redrawOptions;
    }
    if('undefined' !== typeof(options.showOptions)){
        editBoxShowOptions = options.showOptions;
    }
}


function selectAll() {

	var name = currentBox;
        if (document.getElementById(name+'Edit') && document.getElementById(name+'Edit').contentWindow && document.getElementById(name+'Edit').contentWindow.document) {
          runCommand(currentBox, "selectall", false);
        }

}

function saveSelection() {

	if (document.selection) { 
	  currentSel = document.getElementById(currentBox+'Edit').contentWindow.document.selection.createRange();
	}

}

function displayColorBox() {

	if (colorBoxShown) {

	   var colors = ColorSelector.value('colorpicker');
	   colors     = colors.split(',');
	   var color  = '#' + toHex(colors[0]) + toHex(colors[1]) + toHex(colors[2]);

	   runCommand(currentBox, 'forecolor', color);
	   Element.setStyle('colorpicker', {'display':'none'});
	   colorBoxShown = 0;

	} else {

	   var topPos  = currentTop  + 35;
	   var leftPos = currentLeft + 195;

	   Element.setStyle('colorpicker', {'display':'block', 'top':topPos+'px', 'left':leftPos+'px'} );
	   colorBoxShown = 1;

	}

}

function showNewColorChooser(){
    var pos = Position.cumulativeOffset($('menuitem-cc'));
    $('new-color-chooser').setStyle({top:(pos.top+34)+'px', left:pos.left+'px'});
    newColorChooser.draw();
}
                                                                                                                     
function setColor() {
    var color = '#' + $('currentColor').value;
    runCommand(currentBox, 'forecolor', color);
}

function runCommand(name, command, options) {
            
	if (!document.getElementById(currentBox+'Edit') || !document.getElementById(currentBox+'Edit').contentWindow || !document.getElementById(currentBox+'Edit').contentWindow.document) { return; }
	var editableDocument = document.getElementById(currentBox+'Edit').contentWindow.document;
	var origElement      = document.getElementById(currentBox);

	if (currentSel.text) {
	  currentSel.select();
	  currentSel = '';
	}

	switch (command) {

	case "increasefontsize":
	  try {
	    //if (!editableDocument.queryCommandSupported(command)) 
	    //{
          	var currentSize = editableDocument.queryCommandValue("FontSize");
	      	if (!currentSize) 
	      	{
              editableDocument.execCommand("FontSize", false, 3);
	        } 
	        else if (typeof(currentSize) == "string") 
	        {
	          if( currentSize.match("px") )
	          {
                var newSize = pxToFontSize(parseInt(currentSize.replace("px", "")));
	          }
	          else
	          {
	            var newSize = parseInt(currentSize);
	          }
	          newSize = newSize > 6 ? 7 : newSize + 1;
              editableDocument.execCommand("FontSize", false, newSize);
	        } 
	        else 
	        {
              editableDocument.execCommand("FontSize", false, currentSize > 6 ? 7 : currentSize + 1);
	        }
	    //}
	  } catch(e) { editableDocument.execCommand(command, false, options); }
	  break;

        case "decreasefontsize":
          try {
            //if (!editableDocument.queryCommandSupported(command)) 
            //{
              var currentSize = editableDocument.queryCommandValue("FontSize");
              if (!currentSize) 
              {
                editableDocument.execCommand("FontSize", false, 1);
              } 
              else if (typeof(currentSize) == "string")
              { 
                  if( currentSize.match("px")) 
                  {
                	var newSize = pxToFontSize(parseInt(currentSize.replace("px", "")));
                  }
                  else
                  {
                    var newSize = parseInt(currentSize);
                  }
                  newSize = newSize > 1 ? newSize - 1 : 1;
                  editableDocument.execCommand("FontSize", false, newSize);
              } 
              else 
              {
                editableDocument.execCommand("FontSize", false, currentSize < 2 ? 1 : currentSize - 1);
              }
            //}
          } catch(e) { editableDocument.execCommand(command, false, options); }
          break;

	case "justifyleft":
	  currentAlign = "left";
	  editableDocument.getElementById('icontent').style.textAlign = "left";
	  origElement.style.textAlign = "left";
	  break;

        case "justifycenter":
	  currentAlign = "center";
	  editableDocument.getElementById('icontent').style.textAlign = "center";
	  origElement.style.textAlign = "center";
          break;

        case "justifyright":
	  currentAlign = "right";
	  editableDocument.getElementById('icontent').style.textAlign = "right";
	  origElement.style.textAlign = "right";
          break;

        case "justifyfull":
	  currentAlign = "justify";
	  editableDocument.getElementById('icontent').style.textAlign = "justify";
	  origElement.style.textAlign = "justify";
          break;

	default:
          editableDocument.execCommand(command, false, options);
          break;                                                                                                                      
	}
}


function detectPaste(e) {

       var key; var eventPassed;
                                                                                                                             
        if (e) {
           eventPassed = e;
        } else {
           eventPassed = event;
        }
                                                                                                                             
        if (eventPassed.ctrlKey && eventPassed.keyCode == 86) {
                   
	  isPaste = 1;

	}

	return true;

}

function pxToFontSize(currentSize) {

	var newSize = 3;

	if (currentSize < 13) {
          newSize = 1;
        } else if (currentSize >= 13 && currentSize < 16) {
          newSize = 2;
        } else if (currentSize >= 16 && currentSize < 18) {
          newSize = 3;
        } else if (currentSize >= 18 && currentSize < 24) {
          newSize = 4;
        } else if (currentSize >= 24 && currentSize < 32) {
          newSize = 5;
        } else if (currentSize >= 32) {
          newSize = 6;
        }

	return newSize;

}

function keyDown() {

}

function keyUp() {

	if (document.getElementById(currentBox+'Edit')) {
          var box              = document.getElementById(currentBox+'Edit');
          var editableDocument = box.contentWindow.document;
          //alert("keyUp. body Height " + editableDocument.body.offsetHeight + " " + editableDocument.body.clientHeight + " " + editableDocument.body.scrollHeight);

          var myHeight;
          /** if (navigator.appVersion.indexOf("MSIE") == -1) { myHeight = editableDocument.body.offsetHeight; } else { myHeight = editableDocument.body.scrollHeight; } **/
	
	  myHeight = box.contentWindow.document.body.scrollHeight;
	
	  myHeight = myHeight < 25 ? 25 : myHeight;

          Element.setStyle(box,{height: myHeight+'px'});

	  if (isPaste == 1) { isPaste = 0; clean(); }

	  // Auto-save edited text
	  if (!lastSave || (new Date().getTime() > lastSave + (20 * 1000))) {
	    lastSave = new Date().getTime();
	    if (currentBox != 'weebly_site_title') {
	      var text = editableDocument.getElementsByTagName('body')[0].innerHTML;
          if('function' === typeof(editBoxSaveCallback)){
            editBoxSaveCallback(currentBox, text, currentAlign);
          }
          else{
            saveContent(currentBox,text,currentAlign, 1);
          }
	    }

	  }
	}

	return true;

}

function fixIENewLines(event){
    if (Prototype.Browser.IE && event.keyCode == 13 && document.selection) {
        var sel = document.selection.createRange();
        sel.pasteHTML('<br /><span></span>');  //empty span needed to advance cursor to next line
        event.cancelBubble = true;
        event.returnValue = false;
    }
}

function clean() {

      var box  = document.getElementById(currentBox+'Edit');

      if (box) {
	var text = box.contentWindow.document.getElementsByTagName('body')[0].innerHTML;

	//console.log("BeforeClean! "+text);

	//Remove comments...
	text = text.replace(/<!--.*?-->/ig, "");

	//Replace <div> with <br /> in Safari
	if (navigator.appVersion.match("Safari/")) {
	  text = text.replace(/<div>/ig, "<br />");
	}

	//Replace <b></b> tags with <strong></strong>
	text = text.replace(/<b(\s+|>)/ig, "<strong$1");
	text = text.replace(/<\/b(\s+|>)/ig, "</strong$1");
	//Replace <i></i> tags with <em></em>
	text = text.replace(/<i(\s+|>)/ig, "<em$1");
	text = text.replace(/<\/i(\s+|>)/ig, "</em$1");
	//Replace <br> with <br />
	text = text.replace(/<br ?>/ig, "<br />");
    //Replace <br style='...'> with <br />
	text = text.replace(/<br style=[\'\"].*?[\'\"]>/ig, "<br />");
	//Replace <p> with <br />
	text = text.replace(/<\/p[^>]*>/ig, "<br /><br />");
	text = text.replace(/<p[^>]*>/ig, "");
    //Remove trailing new lines
    text = text.replace(/<br \/><br \/>$/, "");
	//Replace <li>'s with <br /> -- don't want to do that anymore!
	//text = text.replace(/<\/li\s*>/ig, "<br />");
	//Add a <br /> to pasted in newlines
	//text = text.replace(/\n/ig, "<br />\n");

	//Clean <font color=#123123> tags (add style and "")
	//text = text.replace(/<font (.*) color=(#[0-9a-f]{6})/ig, "<font $1 color='$2'");
	//Clean <font size=x> tags (add style and "")
        //text = text.replace(/<font (.*) size=([0-9]{1})/ig, "<font $1 size='$2'");

	//Remove empty tags
	text = text.replace(/(<[^\/]>|<[^\/][^>]*[^\/]>)(\s|<br \/>)*<\/[^>]*>/ig, "");
/*
	//Transform <span style=" attributes into their HTML tag equivalents
	text = text.replace(/<[^>]*>([^<]*)<\/[^>]*>/ig, function (match, content) {
					if (match.indexOf("span") == 1) {
					   var replaceWith = '';
					   match = match.replace(/style=["']([^"']*)["']/g, function(inside, style)
						   {
							if (style == "font-weight: bold;") { replaceWith = "<strong>"+content+"</strong>"; }
							else { replaceWith = content; }
							return inside;
						   });
					   return replaceWith;
					} else { return match; }
				    });
*/
	//Remove any disallowed tags
	text = text.replace(/<\/?([^>]*)>/ig, function(match, tag) {
					if (/^span/i.test(tag) || /^strong/i.test(tag) || /^u$/i.test(tag) || /^em/i.test(tag) || /^br \//i.test(tag) || /^big/i.test(tag) || /^small/i.test(tag) || /^a/i.test(tag) || /^font/i.test(tag) || /^ul/i.test(tag) || /^ol/i.test(tag) || /^li/i.test(tag)) {
						return match;
					} else { return ""; }

				      });

	//Remove any disallowed attributes
	text = text.replace(/<[^>]*>/g, function(match) 
				      {
					match = match.replace(/ ([^=]+)=["'][^"']*["']/g, function(inside, attribute)
						{
							if (/alt/i.test(attribute) || /href/i.test(attribute) || /target/i.test(attribute) || /title/i.test(attribute) || /style/i.test(attribute) || /size/i.test(attribute) || /color/i.test(attribute)) { return inside; } else { return ""; }
						});

					return match;

				      });

	// What was this supposed to do?
	//text = text.replace(/(.*)<br \/>$/g, '$1');


	box.contentWindow.document.getElementsByTagName('body')[0].innerHTML = text;
	//box.contentWindow.document.getElementById('contents').innerHTML = text;

	//console.log("AfterClean! "+text);

	return text;
      }

}

function toHex(d) {

	var hD="0123456789ABCDEF";
	var h = hD.substr(d&15,1);
	while(d>15) {d>>=4;h=hD.substr(d&15,1)+h;}
	if (h.length < 2) { h = "0"+h; }
	return h;

}


    function showFeedback() {

	new Effect.Move('feedback', { y: 100, mode: 'absolute'});

    }

    function hideFeedback() {

	new Effect.Move('feedback', { y: -700, mode: 'absolute'});

    }

    function giveFeedback() {

        new Ajax.Request(ajax, {parameters:'pos=givefeedback&feedback='+$('feedbackText').value+'&referral='+$('referralText').value+'&cookie='+document.cookie, onFailure:errFunc});
	//Clear form
	$('feedbackText').value = "";
	$('referralText').value = "";
    	feedbackInit();
    }

function feedbackInit() {

	new Effect.Move('feedback', { y: -700, mode: 'absolute'});
}


    // Elements allows the system to fundamentally understand an element,
    // the properties associated with it, and how they affect the HTML element
    // -------
    Weebly.Elements = {
      Version        : '0.1',
      Author         : 'David Rusenko',
      Company        : 'Weebly, Inc.',
      propertyContainer : null,
      currentElement : null,
      currentBorder: '',
      currentValidateFunction: null,
      currentSaveValidateFunction: null,
      currentCustomProperties: Array(),
      editing: null,
      customHTML: Array(),
      upload: 0,

      init: function(options) {
	this.propertyContainer = options.propertyContainer;
      },

      selectElement: function(element) {

	if(element != this.currentElement) { 

	  if (this.currentElement) { Weebly.Elements.unselectElement(); }
	  //if (this.currentElement != null) { this.unselectElement(); }
	
	  this.currentElement = element;
	  this.currentBorder = element.style.border;
	  this.currentCustomProperties = Array();

	  //this.generateProperties(element);

	  this.generateProperties(this.currentElement);
      if(this.isFormElement() && this.highlightedElement != element.id){
          this.selectForm();
      }
	}

      },

      unselectElement: function(element) {

    if(this.currentElement) {

	  if (this.editing) hideCustomHTML(this.ucfid, this.editing, this.uproperty[this.editing].ucfpid);
	  hideFlashContainer();

	  if (this.currentContentFieldDef.hasimage == 1) {
            Weebly.ImageResize.endResize();
            Weebly.ImageResize.destroy();
          }

		if (this.idfield.eid == '22397704' || this.idfield.eid == '18362204') {
			Weebly.ImageGallery.destroy();
		}

	  // AdSense callback
	  if (this.idfield.eid == "36391960") {
	    var adUnitType;
	    if (this.uproperty[96630555]['propertyresult'] == 'text') {
	      adUnitType = 'TextOnly';
	    } else if (this.uproperty[96630555]['propertyresult'] == 'image') {
              adUnitType = 'ImageOnly';
            } else if (this.uproperty[96630555]['propertyresult'] == 'text_image') {
              adUnitType = 'TextAndImage';
            }
	    new Ajax.Request(ajax, {parameters:'pos=saveadsense&cookie='+document.cookie+'&cfid='+this.uproperty.cfid+'&backgroundColor='+this.uproperty[53687540]['propertyresult']+'&borderColor='+this.uproperty[52715841]['propertyresult']+'&textColor='+this.uproperty[60388297]['propertyresult']+'&titleColor='+this.uproperty[10290183]['propertyresult']+'&urlColor='+this.uproperty[34189825]['propertyresult']+'&adUnitType='+adUnitType+'&layout='+this.uproperty[81070942]['propertyresult']+'&elementid='+this.idfield.id, onFailure:errFunc, bgRequest: true});

	  }

	  Weebly.Linker.close();

	  if (this.currentSaveValidateFunction != '' && !this.closeOnValidate && this.textChange) { this.textChange = null; validateFunction(); this.closeOnValidate = 1;}
	  else {

	    this.closeOnValidate = null;

	    if (swfu.dontSave) { swfu.dontSave = null; }
	    else { this.saveProperties(); }

	    this.currentElement.style.border = this.currentBorder;
	    this.currentBorder = '';
	    this.currentElement = null;
	    this.currentValidateFunction = null;
	    this.currentSaveValidateFunction = null;

	    this.hideMenuBar();
	    clearElementBox();
	  }

	}

      },

      generateProperties: function(element) {

    if (!this.currentElement) { return; }

	var ucfid = element.id;
	var parentRef = $(element).up('.inside');

  var children = parentRef.childElements();
  for( var i=0; i<children.size(); i++ )
  {
    if( children[i].idfield )
    {
      var finalChild = children[i];
      break;
    }
  }
	try {
          eval("var idfield = " + finalChild.idfield.value);
          eval("var pfield = " + finalChild.pfield.value.replace(/\r\n/g, ''));
	} catch(e) { showError(/*tl(*/'This element is not compatible with a recent upgrade. Please delete this element and drag on a new one.'/*)tl*/); return; }
    this.idfield = idfield;
	this.idfieldNode = finalChild.idfield;
	this.pfield = pfield;
	this.pfieldNode = finalChild.pfield;
	this.ucfid = ucfid;
	this.uproperty = pfield[ucfid];
	this.upload = 0;

        Weebly.Cache.get(ajax, 'pos=elementdef&keys='+idfield.eid, function(t) { Weebly.Elements.handlerGenerateProperties(t, ucfid, idfield.eid, pfield[ucfid]); }, null, {asynchronous:false});

      },
      handlerGenerateProperties: function(responseText, ucfid, eid, properties) {

    if (!this.currentElement) { return; }

	responseText = responseText.replace(/\r\n/g, "");
	responseText = responseText.replace(/\n/g, "");
	
	//console.log("var elementDef = " + responseText);
	try {
	  //eval("var elementDef = " + responseText);
      var elementDef = responseText.evalJSON();
	  this.elementDef = elementDef;
	  var cfid = properties.cfid;
	} catch(e) { showError(/*tl(*/'This element is not compatible with a recent upgrade. Please delete this element and drag on a new one.'/*)tl*/); return; }
	this.currentContentFieldDef = elementDef.contentfields[cfid];
	this.currentValidateFunction = this.currentContentFieldDef.elementonchangejs.replace( new RegExp ( "%%ELEMENTID%%", "gi"), this.ucfid);
	this.currentSaveValidateFunction = this.currentContentFieldDef.elementjs.replace( new RegExp ( "%%ELEMENTID%%", "gi"), this.ucfid);

        //If an image, make it resizable
        if (this.currentContentFieldDef.hasimage == 1) {
			Weebly.ImageResize.init(this.getCurrentImageElement(), { callback: onResize, ucfid: this.ucfid });
        } else {
	// Else assign a border
	  this.currentElement.style.border = "1px dashed #4455aa";
	  this.currentElement.style.padding = "0";
        }

	var menuBarDivHTML = "<table id='menuBarItemContainer' spacing=0 padding=0><tr>";
	menuBarDivHTML += "<td style='position: relative; background: none;'><div id='menubar-l' style='position: absolute; width: 5px; height: 44px; top: 0px; margin-left: -10px; background: url(http://"+editorStatic+"/weebly/images/menubar-l.gif) no-repeat bottom left;'></div> </td>";
	var menuBarAdvancedDivHTML = "";
	
	for(var x =0; x < elementDef.contentfields[cfid].contentfieldproperties.length; ++x) {
	  var xReturn = this.generateProperty(elementDef.contentfields[cfid].contentfieldproperties[x], elementDef.contentfields[cfid].contentfieldproperties[x].cfpid, properties);
	  if (xReturn != "") {
	    if (elementDef.contentfields[cfid].contentfieldproperties[x].advanced == 1) {
	      menuBarAdvancedDivHTML += "<span>"+ xReturn +"</span>";
	    } else {
	      menuBarDivHTML += "<td><span class='menuBarSpan'>"+ xReturn +"</span></td>";
	    }
	  }
	}

	if (menuBarAdvancedDivHTML != "") {
	  menuBarDivHTML += "<td style='background: none;'><div id='menuBarAdvancedDiv'><a href='#' onclick='Weebly.Elements.showHideAdvanced(); return false;' style='color: #1C4671; text-decoration: none;'><img src='http://"+editorStatic+"/weebly/images/arrow_right.gif' style='border: 0;' id='advancedImg' /> "+/*tl(*/"Advanced"/*)tl*/+"</a> <br/><br/><div id='menuBarAdvancedContainer'>"+menuBarAdvancedDivHTML+"</div></div> <div id='menuBarAdvancedPlaceholder' style='height: 20px; width: 80px;'></div></td>";
	}

	menuBarDivHTML += "<td style='position: relative; background: none;'><div id='menubar-r' style='position: absolute; width: 5px; height: 44px; top: 0px; margin-left: 10px; background: url(http://"+editorStatic+"/weebly/images/menubar-r.gif) no-repeat bottom left;'></div> </td>";
	menuBarDivHTML += "</tr></table>";

	$('menuBarDiv').innerHTML = menuBarDivHTML;

	if(this.currentValidateFunction != '') { eval("this.currentValidateFunction = "+this.currentValidateFunction); }
	if(this.currentSaveValidateFunction != '') { eval("validateFunction = "+this.currentSaveValidateFunction); }

	this.showMenuBar();

	// Default actions
	if (this.idfield.eid == '69807794') {
	  editCustomHTML(this.ucfid, 38531508, this.uproperty[38531508].ucfpid);
	/*
 	// Don't work anymore due to Flash 10 - Adobe sucks, this was a horrible decision
  	} else if (this.idfield.eid == '36582085' && this.uproperty.cfid == "85675969") {
	  if (this.uproperty['59220138'] && this.uproperty['59220138'].propertyresult && this.uproperty['59220138'].propertyresult.match(/\/weebly\/images\/na\.jpg$/)) {
	    selectUpload("image", "250", this.uproperty.ucfid, this.uproperty['59220138'].ucfpid);
	  }
        } else if (this.idfield.eid == '18383940' && this.uproperty.cfid == "9839145") {
          if (this.uproperty['24196366'] && this.uproperty['24196366'].propertyresult && this.uproperty['24196366'].propertyresult.match(/\/weebly\/images\/na\.jpg$/)) {
            selectUpload("image", "250", this.uproperty.ucfid, this.uproperty['24196366'].ucfpid);
          }
        } else if (this.idfield.eid == '17854681' && this.uproperty.cfid == "72477774") {
          if (this.uproperty['97955873'] && this.uproperty['97955873'].propertyresult && this.uproperty['97955873'].propertyresult.match(/\/weebly\/images\/na\.jpg$/)) {
            selectUpload("image", "250", this.uproperty.ucfid, this.uproperty['97955873'].ucfpid);
          }
	} else if (this.idfield.eid == '46196121' && this.uproperty.cfid == "94384342") {
	  if (this.uproperty['4073443'] && this.uproperty['4073443'].propertyresult && this.uproperty['4073443'].propertyresult == "#") {
	    selectUpload("file", "", this.uproperty.ucfid, this.uproperty['4073443'].ucfpid);
	  }
        } else if (this.idfield.eid == '63260230' && this.uproperty.cfid == "37049277") {
          if (this.uproperty['30527226'] && typeof(this.uproperty['30527226'].propertyresult) != "undefined" && this.uproperty['30527226'].propertyresult == "") {
            selectUpload("audio", "", this.uproperty.ucfid, this.uproperty['30527226'].ucfpid);
          }
	*/
    } else if (this.idfield.eid == '22397704' && this.uproperty.cfid == "34873637") {
    
		  // Image Gallery v0.1
		  var newUcfpid = Weebly.Elements.getPropertyByReference('<!IMAGESET!>');
		  if (newUcfpid.propertyresult == "" || newUcfpid.propertyresult.match("'length': '0'")) {
			//selectUpload("gallery", "thumbnail:333-250", this.uproperty.ucfid, newUcfpid.ucfpid);
		  } else {
			Weebly.ImageGallery.setCurrent(this.ucfid);
			Weebly.ImageGallery.init();
		  }
		  
	} else if (this.idfield.eid == '18362204') {
	
			// Image Gallery v0.2
			Weebly.ImageGallery.setCurrent(this.ucfid);
			Weebly.ImageGallery.init();
	
	}

	//}// else if (this.idfield.eid == '36582085') {
	/**
	  //console.log(this.uproperty);
	  if (siteType == 'myspace') {
	    selectUpload("image", "370", this.ucfid, this.uproperty[59220138].ucfpid)
	  } else {
	    selectUpload("image", "250", this.ucfid, this.uproperty[59220138].ucfpid)
	  }
	}
	**/

      },

      getCurrentImageElement: function() {

	if (!this.currentElement) { return; }

          var imgElement = {};
          if (this.currentElement.tagName == "IMG") {
            imgElement = $(this.ucfid+"");
          } else {
            imgElement = this.currentElement.getElementsByTagName("IMG")[0];
          }
          return imgElement;

      },

      saveProperties: function() {

	if (!this.currentElement) { return; }

	var jsonPField = toJsonString(this.pfield);
	var jsonIDField = toJsonString(this.idfield);

        this.idfieldNode.value = jsonIDField;
        this.pfieldNode.value = jsonPField;

        new Ajax.Request(ajax, {parameters:'pos=saveproperties&cookie='+document.cookie+'&ucfid='+this.ucfid+'&json='+encodeURIComponent(jsonPField), onFailure:errFunc, bgRequest: true});


      },

      showHideAdvanced: function() {

	if (!this.currentElement) { return; }

	if ($("menuBarAdvancedDiv").getWidth() > 100) { 
	  this.hideAdvanced();
	} else { 
	  this.showAdvanced();
	}


      },

      hideAdvanced: function() {

	if (!this.currentElement) { return; }

	$("menuBarAdvancedDiv").style.width = "80px"; 
	$("menuBarAdvancedDiv").style.height = "15px"; 
	$("menuBarAdvancedPlaceholder").style.width = "80px"; 
	$('advancedImg').src = 'http://'+editorStatic+'/weebly/images/arrow_right.gif';
	Weebly.Elements.positionFlash();

      },

      showAdvanced: function() {

	if (!this.currentElement) { return; }

	$("menuBarAdvancedDiv").style.width = "200px"; 
	$("menuBarAdvancedDiv").style.height = ($('menuBarAdvancedContainer').getHeight()-(-40))+"px";
	$("menuBarAdvancedPlaceholder").style.width = "200px"; 
	$('advancedImg').src = 'http://'+editorStatic+'/weebly/images/arrow_down.gif';
	Weebly.Elements.positionFlash();

      },

      generateProperty: function(property, cfpid, uproperty) {

	if (!this.currentElement) { return; }

	var r = '';

	r += '<b>' + property.title + ":</b> ";

	if (property.markuptype == 'custom') {
	  this.currentCustomProperties.push(cfpid);
	}
	
	if(property.options == "%%fileUpload%%") {

	  // Make sure we overwrite an old value!
	  try {
	    if ($(this.ucfid) && uproperty[cfpid].propertyresult != $(this.ucfid).getElementsByTagName("IMG")[0].src) {
	      uproperty[cfpid].propertyresult = $(this.ucfid).getElementsByTagName("IMG")[0].src.replace("http://", "").replace(/^[^\/]*/, "");
	    }
	  } catch (e) { }

          var newImageWidth = '250';
          if(Weebly.Elements.isProductElement()){
            switch(Weebly.Elements.pfield[Weebly.Elements.ucfid][69650731].propertyresult){
                case 'grid':
                  newImageWidth = 164;
                  break;
                case 'large':
                  newImageWidth = 225;
                  break;
                case 'small':
                  newImageWidth = 125;
                  break;
                case 'long':
                  newImageWidth = 175;
                  break;
            }
          }
	  selectUpload("image", newImageWidth, this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"image\", \""+newImageWidth+"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new Image"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%fileUploadFile%%") {
	  selectUpload("file", "", this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"file\", \"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new File"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%fileUploadDocument%%") {
	  selectUpload("doc", "", this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"doc\", \"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new File"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%fileUploadVideo%%") {
	  selectUpload("video", "", this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"video\", \"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new File"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%fileUploadFlash%%") {
	  selectUpload("flash", "", this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"flash\", \"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new File"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%fileUploadAudio%%") {
	  selectUpload("audio", "", this.ucfid, uproperty[cfpid].ucfpid);
	  r += "<a href='#' onclick='selectUpload(\"audio\", \"\", "+this.ucfid+", "+uproperty[cfpid].ucfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new File"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
        } else if(property.options == "%%fileUploadGallery%%") {
	  var newUcfpid = Weebly.Elements.getPropertyByReference('<!IMAGESET!>').ucfpid;
	  selectUpload("gallery", "thumbnail:333-250", this.ucfid, newUcfpid);
          r += "<a href='#' onclick='selectUpload(\"gallery\", \"thumbnail:333-250\", "+this.ucfid+", "+newUcfpid+")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> "+/*tl(*/"Upload new Image(s)"/*)tl*/+"</a>";
	} else if(property.options == "%%LINK%%") {
	  r += "<a href='#' onClick='showCreateLinkProperties(\""+uproperty[cfpid].ucfpid+"\")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/icon_link.gif' class='menuIconImage' alt='New Link Icon'/> "+/*tl(*/"Set Link"/*)tl*/+"</a> <i style='font-size: 10px;'>("+/*tl(*/"website, email, file, etc"/*)tl*/+")</i><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value=\""+uproperty[cfpid].propertyresult+"\" /><input type='hidden' id='linkTargetElement' value=''/>";
	} else if(property.options == "%%CONFLINK%%") {
	  r += "<a href='#' onClick='showCreateLinkProperties(\""+uproperty[cfpid].ucfpid+"\")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/icon_link.gif' class='menuIconImage' alt='New Link Icon'/> "+/*tl(*/"Set Link"/*)tl*/+"</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value=\""+uproperty[cfpid].propertyresult+"\" /><input type='hidden' id='linkTargetElement' value=''/>";
	} else if(property.options == "%%NUMBER%%") {
	  r += "<input type='text' id='"+uproperty[cfpid].ucfpid+"' value=\""+uproperty[cfpid].propertyresult.replace(/"/g, "&quot;")+"\" onBlur='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");' onKeyUp='Weebly.Elements.onTextChange(\""+uproperty[cfpid].ucfpid+"\");'/>";
	} else if(property.options == "%%ALPHANUMERIC%%") {
	  r += "<input type='text' id='"+uproperty[cfpid].ucfpid+"' value=\""+uproperty[cfpid].propertyresult.replace(/"/g, "&quot;")+"\" onBlur='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");' onKeyUp='Weebly.Elements.onTextChange(\""+uproperty[cfpid].ucfpid+"\");'/>";
	} else if(property.options == "%%EMAIL%%") {
      var emailValue = uproperty[cfpid].propertyresult ? uproperty[cfpid].propertyresult.replace(/"/g, "&quot;") : userEmail;
	  r += "<input type='text' id='"+uproperty[cfpid].ucfpid+"' value=\""+emailValue+"\" onBlur='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");' onKeyUp='Weebly.Elements.onTextChange(\""+uproperty[cfpid].ucfpid+"\");'/>";
	} else if(property.options == "%%CUSTOMHTML%%") {
	  r += "<a href='#' onclick='editCustomHTML(\""+this.ucfid+"\", \""+cfpid+"\", \""+uproperty[cfpid].ucfpid+"\")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/page_script.gif' class='menuIconImage' alt='Edit Custom HTML'/>  Edit Custom HTML</a><textarea class='customhtml_textarea' onBlur='hideCustomHTML(\""+this.ucfid+"\", \""+cfpid+"\", \""+uproperty[cfpid].ucfpid+"\")' id='"+uproperty[cfpid].ucfpid+"CustomHTML' rows='20' style='border: 1px solid #ccc; display: none; height: 150px; width: 100%;'>"+uproperty[cfpid].propertyresult+"</textarea>";
	} else if(property.options == "%%EMBEDCODE%%") {
	  r += "Custom HTML";
	} else if(property.options == "%%editPollDaddy%%") {
	  r += "<a href='#' onclick='editPollDaddy(\""+uproperty[cfpid].propertyresult.replace(/"/g, "&quot;")+"\", \""+this.ucfid+"\", \""+uproperty['89468692'].ucfpid+"\")' class='menuIconLink'><img src='http://"+editorStatic+"/weebly/images/image_new.gif' class='menuIconImage' alt='New Image Icon'/> Edit Poll</a><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult+"'/>";
	} else if(property.options == "%%longText%%") {
      r += "<div class='menuBarExpand' id='longprop-"+uproperty[cfpid].ucfpid+"'><a href='#' onclick='Weebly.Elements.toggleLongProperty(\""+uproperty[cfpid].ucfpid+"\", \"250\"); return done();' class='menuIconAdvanced'><img src='http://"+editorStatic+"/weebly/images/arrow_right.gif' id='longprop-icon-"+uproperty[cfpid].ucfpid+"' style='border: none;'/> "+/*tl(*/"Edit Text"/*)tl*/+"</a><textarea style='width:230px; height:75px; border:1px solid #999999; margin: 7px 0 0 6px;' id='"+uproperty[cfpid].ucfpid+"' onBlur='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");' onKeyUp='Weebly.Elements.onTextChange(\""+uproperty[cfpid].ucfpid+"\");'>"+uproperty[cfpid].propertyresult+"</textarea><div style='position:relative; width:100%'><a href='#' onclick='Weebly.Elements.toggleLongProperty(\""+uproperty[cfpid].ucfpid+"\", \"250\"); return done();'><img src=\"http://"+editorStatic+"/weebly/images/options-save.gif\" alt=\"Save\" class=\"editmenu-save\" /></a></div></div><div class='menuBarExpandPlaceholder'></div>";
    } else if(property.options == "%%longTextInstructions%%") {
      r += "<div class='menuBarExpand' id='longprop-"+uproperty[cfpid].ucfpid+"'><a href='#' onclick='Weebly.Elements.toggleLongProperty(\""+uproperty[cfpid].ucfpid+"\", \"250\"); return done();' class='menuIconAdvanced' title='Field instructions are displayed next to the field to assist a website visitor.'><img src='http://"+editorStatic+"/weebly/images/arrow_right.gif' id='longprop-icon-"+uproperty[cfpid].ucfpid+"' style='border: none;'/> "+/*tl(*/"Edit Text"/*)tl*/+"</a><textarea style='width:230px; height:75px; border:1px solid #999999; margin: 7px 0 0 6px;' id='"+uproperty[cfpid].ucfpid+"' onBlur='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");' onKeyUp='Weebly.Elements.onTextChange(\""+uproperty[cfpid].ucfpid+"\");'>"+uproperty[cfpid].propertyresult+"</textarea><div style='position:relative; width:100%'><a href='#' onclick='Weebly.Elements.toggleLongProperty(\""+uproperty[cfpid].ucfpid+"\", \"250\"); return done();'><img src=\"http://"+editorStatic+"/weebly/images/options-save.gif\" alt=\"Save\" class=\"editmenu-save\" /></a></div></div><div class='menuBarExpandPlaceholder'></div>";
    } else if(property.options == "%%inputOptions%%") {
	  r += "<div class='menuBarExpand' id='longprop-"+uproperty[cfpid].ucfpid+"'><a href='#' onclick='Weebly.Elements.toggleLongProperty(\""+uproperty[cfpid].ucfpid+"\", \"250\"); Weebly.Elements.editInputOptions(\""+uproperty[cfpid].propertyresult.replace(/"/g, "&quot;").replace(/'/g, "&#039;")+"\", \""+this.ucfid+"\", \""+uproperty[cfpid].ucfpid+"\"); return done();' class='menuIconAdvanced'><img src='http://"+editorStatic+"/weebly/images/arrow_right.gif' id='longprop-icon-"+uproperty[cfpid].ucfpid+"' style='border: none;'/> "+/*tl(*/"Edit Options"/*)tl*/+"</a><div id='inputoptions-"+uproperty[cfpid].ucfpid+"'></div><div style='position:relative'><div class='predefined-options-container'><div class='predefined-options' style='display:none;'></div><div class='predefined-options-handle'><a href='#' onclick='Weebly.Elements.togglePredefinedOptions(\""+uproperty[cfpid].ucfpid+"\", \""+this.ucfid+"\"); return done();'>Predefined Options&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a><a href='#' onclick='Weebly.Elements.togglePredefinedOptions(\""+uproperty[cfpid].ucfpid+"\", \""+this.ucfid+"\"); return done();'><img src='http://"+editorStatic+"/weebly/images/blog-settings-arrow-down.gif' class='predefined-options-arrow' border='0' /></a></div></div></div><input type='hidden' id='"+uproperty[cfpid].ucfpid+"' value='"+uproperty[cfpid].propertyresult.replace(/'/g, "&#039;")+"'/></div><div class='menuBarExpandPlaceholder'></div>";
	} else {
	  r += "<select id='"+uproperty[cfpid].ucfpid+"' onChange='Weebly.Elements.onChange(\""+uproperty[cfpid].ucfpid+"\", \""+cfpid+"\");'>";
	  var options = property.options.split("/");
	  var results = property.result.split("/");
	  for (var x=0; x < options.length; x++) {
	    var selected = '';
	    if (results[x] == uproperty[cfpid].propertyresult.replace(/'/g, '"')) { selected = ' selected'; }
	    r += "<option value='"+results[x]+"'"+selected+">"+options[x]+"</option>";
	  }
	  r += "</select>";
	}
	//r += "<br/>";

	if (property.referenceproperty != '') {
	  this.currentValidateFunction = this.currentValidateFunction.replace( new RegExp( property.referenceproperty, "gi"), uproperty[cfpid].ucfpid);
	  this.currentSaveValidateFunction = this.currentSaveValidateFunction.replace( new RegExp( property.referenceproperty, "gi"), uproperty[cfpid].ucfpid);
	}

	if (property.hidden != 1) { return r; } else { return ''; }
      },

      onTextChange: function(ucfpid, lastValue) {

	if (!this.currentElement) { return; }

	this.textChange = 1;

	if (typeof(lastValue) == "undefined") {
	  // First iteration
	  this.onTextChangeTracking = ucfpid;
	  setTimeout("Weebly.Elements.onTextChange('"+ucfpid+"', '"+$(ucfpid+"").value+"');", 1500);
	} else if($(ucfpid+"") && $(ucfpid+"").value && $(ucfpid+"").value == lastValue || ($(ucfpid+"").value == '' && lastValue == '')) {
	  // User has stopped typing
	  this.onTextChangeTracking = null;
	  Weebly.Elements.onChange(ucfpid);
        } else if(ucfpid != this.onTextChangeTracking) {
	  // User is still typing
	  setTimeout("Weebly.Elements.onTextChange('"+ucfpid+"', '"+$(ucfpid+"").value+"');", 1500);
        }

      },

      onChange: function(ucfpid) {

	if (!this.currentElement) { return; }

	if (this.currentValidateFunction != '' && typeof(this.currentValidateFunction) == "function") { this.currentValidateFunction(ucfpid); }
	if (this.currentSaveValidateFunction != '') { validateFunction(); }
	else { this.continueOnChange(); }

      },

      continueOnChange: function() {

	if (!this.currentElement) { return; }

       var customContent = this.currentContentFieldDef.customcontent;
       customContent = customContent.replace( new RegExp ( "%%INEDITOR%%", "gi"), 1);

       for(var x = 0; x < this.currentContentFieldDef.contentfieldproperties.length; ++x) {
	var cfpid = this.currentContentFieldDef.contentfieldproperties[x].cfpid;

	if (!this.uproperty[cfpid]) { continue; }

	var ucfpid = this.uproperty[cfpid].ucfpid;
	var property = this.currentContentFieldDef.contentfieldproperties[x];
	
        var result = '';
	if ($(""+ucfpid) && ($(""+ucfpid).value || $(""+ucfpid).value == '')) { 
	  result   = $(""+ucfpid).value;
	  this.pfield[this.ucfid][cfpid].propertyresult = result;
	} else if (property.options == "%%CUSTOMHTML%%") {
	  result   = $(""+ucfpid+"CustomHTML").value;
	  this.pfield[this.ucfid][cfpid].propertyresult = "%%NOCHANGE%%";
	} else { 
	  result   = this.pfield[this.ucfid][cfpid].propertyresult;
	}

	if (property.markuptype == "html") {
	  this.currentElement[property.property] = result;
	} else if (property.markuptype == "css") {

	  var styles = {};
	  // cssFloat or styleFloat -- only quirky property
	  if (property.property == "float") {
	    //var styles = { cssFloat: result, styleFloat: result };
	    if (this.currentElement.style.styleFloat) {
	      var styles = { styleFloat: result };
	    } else {
	      var styles = { cssFloat: result };
	    }
	  } else {
	    eval("var styles = { '"+property.property+"': '"+result+"' };");
	  }
	  Element.setStyle(this.currentElement, styles);
	} else if (property.markuptype == "custom") {

	  // Don't update Custom HTML or upload src fields
	  if (!(property.property == "%%CUSTOM2%%" && result == "%%NOCHANGE%%")) {
	    if(property.property == '%%FEEDURL%%'){
	      customContent = customContent.replace( new RegExp( property.property, "gi"), encodeURIComponent(result));
	    } else {
	      customContent = customContent.replace( new RegExp( property.property, "gi"), result);
	    }
	  } else {
	    customContent = customContent.replace( new RegExp( property.property, "gi"), this.customHTML[ucfpid]);
	  }

	}
       }

       if (this.currentContentFieldDef.hascontent == 2) {

        customContent = customContent.replace( new RegExp ( "%%ELEMENTID%%", "gi"), this.ucfid);
        customContent = customContent.replace( new RegExp ( "%%PAGEELEMENTID%%", "gi"), this.idfield.id);
        //customContent = customContent.replace(/\n/g,"");
        customContent = customContent.replace(/<weebly only_export([^>]*)>([\S\s]*?)<\/weebly>/img, '');

        var subdomain = configSiteName.replace(/\.weebly\.com$/, "");
        customContent = customContent.safeReplace(/%%WEEBLYSUBDOMAIN%%/ig, subdomain);
        customContent = customContent.safeReplace(/%%WEEBLYCONFIGDOMAIN%%/ig, configSiteName);

	var isGallery = null;
        if (customContent.match(/<!WEEBLYGALLERY.*?!>/)) {
          Weebly.ImageGallery.destroy();
	  Weebly.ImageGallery.setCurrent(this.ucfid);
          var updateHtml = Weebly.ImageGallery.update();
          customContent = customContent.safeReplace(/<!WEEBLYGALLERY.*?!>/, updateHtml);
	  isGallery = 1;
        }
        
    var _match;

	// Replace external account IDs
	if (_match = customContent.match(/<!EXTERNAL:([^!]+)!>/)) {
	  var remoteSite = _match[1];
	  if (remoteAccounts[remoteSite]) {
	    customContent = customContent.safeReplace(/<!EXTERNAL:([^!]+)!>/, remoteAccounts[remoteSite].remoteId);
	  }
	}

    if(_match = customContent.match(/<!WEEBLYRADIO\-(.*?)\-(.*?)!>/)){
        customContent = customContent.replace('<!WEEBLYRADIO-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawRadioOptions(_match[2]));
    }
    if(_match = customContent.match(/<!WEEBLYSELECT\-(.*?)\-(.*?)!>/)){
        customContent = customContent.replace('<!WEEBLYSELECT-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawSelectOptions(_match[2]));
    }
    if(_match = customContent.match(/<!WEEBLYCHECKBOXES\-(.*?)\-(.*?)!>/)){
        customContent = customContent.replace('<!WEEBLYCHECKBOXES-'+_match[1]+'-'+_match[2]+'!>', Weebly.Form.drawCheckboxes(_match[2]));
    }

    if(_match = customContent.match(/<!(.*?)CONTENTS!>/)){
        customContent = customContent.safeReplace(/<!(.*?)CONTENTS!>/, $(_match[1]+'-form-list').innerHTML);
    }

	if(!this.isProductElement()){
          var writeContent = customContent.replace(/<weebly include_once(_noexport)? ([^>]*)>([\S\s]*?)<\/weebly>/im, '');
          this.currentElement.innerHTML = writeContent;
          if(this.isFormElement()){
              initDraggables();
              Weebly.Form.fieldInstructionsHandler();
          }
          if($(this.currentElement).down('.weebly-form-instructions')){
              Weebly.Form.setupFieldInstructions($(this.currentElement).down('.weebly-form-instructions'));
          }
    	}
    else{
        this.currentElement.innerHTML = this.currentElement.innerHTML;
    }

	if (isGallery) {
	  Weebly.ImageGallery.init();
	  isGallery = null;
	}

	this.tryRunScripts(customContent);
	makeIframesDraggable($('secondlist'));

       }

       if (this.currentContentFieldDef.hasimage == 1) {
	  Weebly.ImageResize.destroy();
	  var img = this.getCurrentImageElement();
    var ucfid = this.ucfid;
	  img.onload =  
	    function(){ 
	      Weebly.ImageResize.init(img, {callback: onResize, ucfid: ucfid});
	      Behaviour.apply();
	      img.onload = null;
	    };
       }

       if (this.closeOnValidate) {
	 this.unselectElement();
       }

      },

      showMenuBar: function() {

		var menuBarDiv = $('menuBarDiv');
		menuBarDiv.style.height = '40px';
		Effect.Queues.get('menuscope').each(function(e) { e.cancel(); });
		Effect.Appear(menuBarDiv, { duration: 0.3, queue: {position: 'end', scope: 'menuscope'}, afterFinish: function() {
			showEvent('showProperties');
			Weebly.Elements.positionFlash();
		}});

      },

      toggleLongProperty: function(ucfpid, width){
          var prop = $('longprop-'+ucfpid);
          if(prop){
              if(prop.getStyle('width') == width+'px'){
                  $('longprop-'+ucfpid).setStyle({'width':'', 'height':'', zIndex:'', paddingBottom:'', overflow:'hidden'});
                  $('longprop-icon-'+ucfpid).src = 'http://'+editorStatic+'/weebly/images/arrow_right.gif';
              }
              else{
                  $('longprop-'+ucfpid).setStyle({'width':width+'px', 'height':'auto', zIndex:'100', paddingBottom:'31px', overflow:'visible'});
                  $('longprop-icon-'+ucfpid).src = 'http://'+editorStatic+'/weebly/images/arrow_down.gif';
                  if(Prototype.Browser.Gecko){
                      $('longprop-'+ucfpid).setStyle({'top':'8px'});
                  }
              }
          }
      },
      
      togglePredefinedOptions: function(ucfpid, ucfid){
          var prop = $('longprop-'+ucfpid);
          var options = prop.down('.predefined-options');
          if(!options.visible()){
              if(options.childElements().size() == 0){
                  Weebly.Elements.createCommonFieldOptions(ucfpid, ucfid);
              }
              Effect.BlindDown(options, {afterFinish:function(effect){
                  effect.element.setStyle({overflow:''});
              }});
              prop.down('.predefined-options-arrow').src='http://'+editorStatic+'images/blog-settings-arrow-up.gif';
          }
          else{
              Effect.BlindUp(options);
              prop.down('.predefined-options-arrow').src='http://'+editorStatic+'images/blog-settings-arrow-down.gif';
          }
      },

      /*editLongText : function(current, ucfid, ucfpid){
          if($('longtext-'+ucfpid) && !$('longtext-open-'+ucfpid)){
              $(ucfpid).remove();
              var div = new Element('div', {'id':'longtext-open-'+ucfpid, 'class':'galleryCaptionForm'}).setStyle({'top':'35px'});
              div.update('<textarea id="longtext-value-'+ucfpid+'" onBlur="Weebly.Elements.onChange('+ucfpid+');">'+current+'</textarea>');
              $('longtext-'+ucfpid).insert({'bottom':div});
          }
      },

      saveLongText : function(ucfpid){
          if($('longtext-value-'+ucfpid)){
              $(ucfpid).value = $('longtext-value-'+ucfpid).value;
              Weebly.Elements.toggleLongProperty(ucfpid, "250");
          }
      },*/

      itemTemplate : new Template('<li style="position:relative;"><img src="http://'+editorStatic+'/weebly/images/options-move.gif" alt="move" class="options-handle editmenu-input-move" /><input type="text" value="#{value}" /><a href="#" onclick="Weebly.Elements.removeInputOption(this); return false;"><img src="http://'+editorStatic+'/weebly/images/options-delete.gif" alt="Delete" class="editmenu-input-delete" /></a></li>'),

      editInputOptions : function(current, ucfid, ucfpid){
          if($('inputoptions-'+ucfpid) && !$('inputoptions-open-'+ucfpid)){
              var values = $(ucfpid).value.split('||');
              var container = new Element('div');
              var div = new Element('div', {'id':'inputoptions-open-'+ucfpid, 'class':'editmenu-input-options'});
              container.insert({'top':div});
              container.insert({'bottom':'<div style="position:relative; width:100%;"><a href="#" onclick="Weebly.Elements.addInputOption(); return done();"><img src="http://'+editorStatic+'/weebly/images/options-plus.gif" alt="Add" class="editmenu-input-plus" /></a> <a href="#" onclick="Weebly.Elements.undoInputOptions(\''+ucfpid+'\', \''+ucfid+'\'); return done();"><img src="http://'+editorStatic+'/weebly/images/undo_button.gif" alt="Undo" class="editmenu-input-undo" style="display:none;" /></a> <a href="#" onclick="Weebly.Elements.saveInputOptions(\''+ucfpid+'\'); return done();"><img src="http://'+editorStatic+'/weebly/images/options-save.gif" alt="Save" class="editmenu-input-save" /></a></div>'});
              var list = new Element('ul', {'id':'inputoptions-list'});
              div.insert({top:list});
              var i = 0;
              values.each(function(value){
                  list.insert({bottom:Weebly.Elements.itemTemplate.evaluate({'value':value})});
              });

              $('inputoptions-'+ucfpid).insert({'bottom':container});
              Sortable.create('inputoptions-list',{handle:'options-handle'});
              if(typeof(predefinedOptionsUndoDisplay) !== 'undefined' && predefinedOptionsUndoDisplay){
                  Effect.Appear(container.down('.editmenu-input-undo'));
              }
          }
      },

      removeInputOption : function(el){
          $(el).up('li').remove();
      },

      addInputOption : function(){
          if($('inputoptions-list')){
              $('inputoptions-list').insert({bottom:Weebly.Elements.itemTemplate.evaluate({'value':''})});
              Sortable.create('inputoptions-list',{handle:'options-handle'});
          }
      },

      saveInputOptions : function(ucfpid){
          if($('inputoptions-list')){
              var values = new Array();
              $('inputoptions-list').select('input').each(function(input){
                  values.push(input.value);
              });
              $(ucfpid).value = values.join('||');
              Weebly.Elements.onChange(ucfpid);
              $('inputoptions-open-'+ucfpid).up().remove();
              Weebly.Elements.toggleLongProperty(ucfpid, '250');
          }
      },
      
      createCommonFieldOptions : function(ucfpid, ucfid){
          var prop = $('longprop-'+ucfpid);
          var options = prop.down('.predefined-options');
          $H(Weebly.Form.commonFieldOptions).keys().each(function(key){
              var div = new Element('div', {'class':'predefined-option', 'title':Weebly.Form.commonFieldOptions[key].join(', ')});
              div.update(key);
              div.observe('click', function(){
                  Weebly.Elements.insertCommonFieldOptions(ucfpid, ucfid, key);
              });
              options.insert({'bottom':div});
          });
      },
      
      insertCommonFieldOptions : function(ucfpid, ucfid, key){
          var prop = $('longprop-'+ucfpid);
          prop.down('.predefined-options').hide();
          predefinedOptionsUndoDisplay = true;
          predefinedOptionsUndo = $(ucfpid).value;
          prop.down('.predefined-options-arrow').src='http://'+editorStatic+'images/blog-settings-arrow-down.gif';
          var options = Weebly.Form.commonFieldOptions[key].join('||');
          $(ucfpid).value = options;
          $('inputoptions-open-'+ucfpid).up().remove();
          Weebly.Elements.editInputOptions(options, ucfid, ucfpid);
      },
      
      undoInputOptions : function(ucfpid, ucfid){
          var prop = $('longprop-'+ucfpid);
          predefinedOptionsUndoDisplay = false;
          $(ucfpid).value = predefinedOptionsUndo;
          $('inputoptions-open-'+ucfpid).up().remove();
          Weebly.Elements.editInputOptions(predefinedOptionsUndo, ucfid, ucfpid);
      },

      positionFlash: function() { // position over the menu bar. ALSO positions non-flash plain uploader
	if(!forceFlashPosition){
        if (this.upload && $('menuBarItemContainer')) {

          var el = $('menuBarItemContainer').down('td').nextSibling.down('.menuBarSpan');
          el.style.height = "30px";
          el.style.display = "block";

          //// for html-based file uploader
          //if (this.preferPlainUploader()) {
		  //  getPlainUploader('menu').show(el, el);
          //  hideFlashContainer();
          //}else{
          
		      if (Prototype.Browser.Gecko) {
		        Element.clonePosition($('flashContainer'), el, {offsetTop: 18});
		      } else {
		        $('flashContainer').style.position = 'relative';
		        Element.clonePosition($('flashContainer'), el);
		        $('flashContainer').style.position = 'absolute';
		      }
		      showFlashContainer(21); // position over the menu (which has zindex of 20)
		      
		  //}

        } else {

          hideFlashContainer();

        }
    }

      },

      hideMenuBar: function() {

	Effect.Queues.get('menuscope').each(function(e) { e.cancel(); });
	Effect.Fade('menuBarDiv', { duration: 0.3, queue: {scope: 'menuscope'}});
	$('editMenu').style.display = 'none';

	hideFlashContainer();

      },

      getPropertyByReference: function(reference) {

	if (!this.currentElement) { return {}; }

	for (var cfpid in this.uproperty) {
	  if (cfpid != 'cfid' && cfpid != 'ucfid' && this.uproperty[cfpid].referenceproperty == reference) {
	    return this.uproperty[cfpid];
	  }
	}

	return {};

      },

      tryRunScripts: function(customContent) {

	scriptSrc = new Array();
	scriptId  = new Array();
	scriptType= new Array();
	var runScripts= 0;

	while (customContent.indexOf('<weebly include_once') > -1 && scriptSrc.length < 30) {
	  customContent = customContent.replace(/<weebly include_once(_noexport)? ([^>]*)>([\s\S]*?)<\/weebly>/im, '');
	  scriptSrc.push(RegExp.$3);
	  scriptId.push(RegExp.$2);
	  runScripts = 1;
	}

	if( runScripts > 0 ) {
	  runIncludedScripts();
	}

      },

      isProductElement: function(){
          return $(this.currentElement).down('.product-image');
      },

      getCurrentProductID: function(){
          if(this.isProductElement()){
              var form = $(this.ucfid + '-product-data');
              return $F(form['productID']);
          }
          return false;
      },

      isFormElement : function(){
          return this.currentElement && $(this.currentElement.id+'-form-parent') ? true : false;
      },

      selectForm : function(){
          this.highlightElement();
          var listEl = $(this.currentElement).down('.formlist');
          if($(listEl.id+'-cover')){
              $(listEl.id+'-cover').remove();
          }
          elementsPage('form');
          if(!$H(userEvents).get('editAdvancedForm') && $('elements_container').visible()){
            showTip("These elements can be dragged into your form to create new fields", $('elementlist'), 'y', '101');
            showEvent('editAdvancedForm');
          }
          Behaviour.apply();
          Weebly.Elements.unselectElement();
      },

      highlightElement: function(){
          if(Weebly.Elements.highlightedElement){return;}
          Weebly.Elements.highlightedElement = this.currentElement.id;
          Sizzle.matches(':not(#secondlist .formlist .columnlist)', $$('#secondlist .columnlist')).each(function(column){
              Sortable.destroy(column.id);
              contentDraggables = contentDraggables.findAll(function(el){return el.id != column.id;});
          })
          Sortable.destroy('secondlist');
          contentDraggables = contentDraggables.findAll(function(el){return el.id != 'secondlist';});
          var cover_top = new Element('div', {'id':'cover-top','class':'editor-cover'});
          var cover_left = new Element('div', {'id':'cover-left','class':'editor-cover'});
          var cover_right = new Element('div', {'id':'cover-right','class':'editor-cover'});
          var cover_bottom = new Element('div', {'id':'cover-bottom','class':'editor-cover'});
          var buttons = new Element('div', {'id':'cover-buttons'}).update('<img src="http://'+editorStatic+/*tli(*/'/weebly/images/form-options.gif'/*)tli*/+'" style="margin-right:10px; cursor:pointer;" onclick="Weebly.Elements.selectElement($(\''+Weebly.Elements.highlightedElement+'\'))" /><img src="'+/*tli(*/'images/view-data.gif'/*)tli*/+'" style="margin-right:10px; cursor:pointer;" onclick="viewFormData(\''+this.currentElement.id+'\')" /><img src="'+/*tli(*/'images/form-close.gif'/*)tli*/+'" style="cursor:pointer;" onclick="Weebly.Elements.removeHighlight();" />');
          $('scroll_container').insert({bottom:cover_top});
          $('scroll_container').insert({bottom:cover_left});
          $('scroll_container').insert({bottom:cover_right});
          $('scroll_container').insert({bottom:cover_bottom});
          $('scroll_container').insert({bottom:buttons});
          Weebly.Elements.resizeHighlight();
          $$('.editor-cover').each(function(cover){
              cover.observe('click', Weebly.Elements.removeHighlight)
          });
      },

      removeHighlight : function(){
          Weebly.Elements.unselectElement();
          Weebly.Elements.highlightedElement = null;
          Weebly.Elements.highlightedHeight = 0;
          $$('.editor-cover').invoke('remove');
          $('cover-buttons').remove();
          if(destroySecondList){
              elementsPage('blog');
          }
          else{
              elementsPage('default');
          }
          updateList();
      },

      resizeHighlight : function(){
        if(Weebly.Elements.highlightedElement){
            var el = $(this.highlightedElement);
            var dimensions = el.getDimensions();
            if(Weebly.Elements.highlightedHeight != dimensions.height){
                Weebly.Elements.highlightedHeight = dimensions.height;
                var padding = 10;
                var start_top = parseInt($('scroll_container').getStyle('marginTop').replace(/[^\d]/g, ''));
                var offset = el.cumulativeOffset();
                var top_height = offset.top-(start_top+padding);
                var bottom_top = top_height + dimensions.height + (2*padding);
                var bottom_height = $('icontent_container').getHeight() - bottom_top;
                var side_height = dimensions.height + (2*padding);
                var right_edge = ($('scroll_container').getWidth() - (offset.left + dimensions.width)) - 2*padding;
                $('cover-top').setStyle({width:'100%',height:top_height+'px', top:'0px'});
                $('cover-left').setStyle({width:(offset.left-padding)+'px',height:side_height+'px', top:top_height+'px'});
                $('cover-right').setStyle({width:right_edge+'px',height:side_height+'px', top:top_height+'px', right:'0px'});
                $('cover-bottom').setStyle({width:'100%',height:bottom_height+'px', top:bottom_top+'px'});
                $('cover-buttons').setStyle({position:'absolute', zIndex:'10', top:(top_height - 45)+'px', right:right_edge+'px'});
            }
            setTimeout('Weebly.Elements.resizeHighlight()', 300);
        }
      }
      
      //// for html-based file uploader
      //,
      //preferPlainUploader: function() { // can only be called when an element is selected. for current element
	  //  return !(this.idfield.eid == '22397704' && this.uproperty.cfid == "34873637" || this.idfield.eid == '18362204');
      //  // is not an image gallery
	  //}

    };

    //------------
    /// End of Elements module
    ////


var imageResizeId = new Object();


    // ImageResize allows users to resize images inline
    // -------
    Weebly.ImageResize = {
      Version        : '0.1',
      Author         : 'David Rusenko',
      Company        : 'Weebly, Inc.',
      callbackFunc : '',
      resizer: null,

      init: function(el) {

	if (this.resizer || this.el) this.destroy();

	this.el = $(el);

	// Can't resize images that aren't local! Return if src doesn't start with http://*.weebly.com/uploads/ or /uploads/
	if (!this.el.src.match(/^http:\/\/[^\.]+\.weebly\.com\/uploads\//) && !this.el.src.match(/^\/uploads\//)) { this.el = null; return; }

	this.options = Object.extend({
	  minWidth: 30,
	  minHeight: 30,
	  maxWidth: 1000,
	  maxHeight: 2000,
	  appendElement: 'scroll_container',
	  callback: function() {},
	  ucfid: null
	  //callback: function(width, height, id) { alert(width+" "+height+" "+id); }
	}, arguments[1] || {});

	var resizer = document.createElement('DIV');
	resizer.style.border = '1px solid #000000';
	resizer.style.background = '#FF5544';
	resizer.style.position = 'absolute';
	resizer.style.display = 'none';
	resizer.style.fontSize = "3px";
	resizer.style.width = "7px";
	resizer.style.height = "7px";
	resizer.style.cursor = 'nw-resize';
	resizer.style.zIndex = '1000';
	resizer.id = "imageresizer";
	document.getElementById(this.options.appendElement).appendChild(resizer);
	this.resizer = resizer;

	this.calcHeight();

	this.eventMouseDown = this.initResize.bindAsEventListener(this);
	Event.observe(this.resizer, "mousedown", this.eventMouseDown);

	Weebly.ImageResizeArray.init(this);
	this.on();

      },

      destroy: function() {

	if (this.el && this.resizer) {
	  Event.stopObserving(this.resizer, "mousedown", this.eventMouseDown);
	  Weebly.ImageResizeArray.kill(this);
	  this.off();
	  this.resizer.parentNode.removeChild(this.resizer);
	  this.resizer = null;
	  this.el = null;
	}

      },

      calcHeight: function() {

        this.origWidth = this.el.width;
        this.origHeight = this.el.height;

      },

      on: function() {

	this.el = Weebly.Elements.getCurrentImageElement();

	this.calcHeight();
	this.positionResizer();

	// from old standalone images where dotted border for resizing
	//this.options.oldBorder = this.el.style.border;
	//this.el.style.border  = this.options.border;
	
	this.resizer.style.display = 'block';

      },

      off: function() {

	this.resizer.style.display = 'none';
	//this.el.style.border = this.options.oldBorder;

      },

      positionResizer: function() {
	if (!this.resizer || !this.el) return;

	// Need to cause browsers to re-calculate image dimensions
	// HERE

        var cumulPosition  = Position.cumulativeOffset(this.el);
        var offsetPosition = Position.realOffset(this.el);
        var sizePosition   = [this.el.width, this.el.height];
        
        if (!Prototype.Browser.IE) {
        	sizePosition[0] += (parseInt(this.el.style.borderLeftWidth) || 0) * 2 + (parseInt(this.el.style.paddingLeft) || 0) * 2;
        	sizePosition[1] += (parseInt(this.el.style.borderTopWidth) || 0) * 2 + (parseInt(this.el.style.paddingTop) || 0) * 2;
        }

		var appendElementMargin = Element.getStyle(this.options.appendElement, 'margin-top').replace(/px/, '');

        this.resizer.style.left = (cumulPosition[0] + sizePosition[0] - 4) + "px";
        this.resizer.style.top  = (cumulPosition[1] - appendElementMargin + sizePosition[1] - 4) + "px";

      },

      initResize: function(event) {
	if(Event.isLeftClick(event)) {
	  // abort on form elements, fixes a Firefox issue
	  var src = Event.element(event);
	  if(src.tagName && (src.tagName=='INPUT' || src.tagName=='SELECT' || src.tagName=='OPTION' || src.tagName=='BUTTON' || src.tagName=='TEXTAREA')) return;

	  var pointer = [Event.pointerX(event), Event.pointerY(event)];
	  var pos     = Position.cumulativeOffset(this.el);
	  this.offset = [0,1].map( function(i) { return (pointer[i] - pos[i]); });
	  this.pointer = pointer;

	  this.origWidth = this.el.width;
	  this.origHeight = this.el.height;

	  Weebly.ImageResizeArray.setCurrent(this);
	  Event.stop(event);

	}
      },

      startResize: function(event) {
	this.resizing = true;

      },

      updateResize: function(event, pointer) {
	if(!this.resizing) this.startResize(event);

	Position.prepare();
	this.draw(pointer);
	Event.stop(event);
      },

      endResize: function(event) {
	if(!this.resizing) return;

	this.resizing = false;
	Weebly.ImageResizeArray.removeCurrent();
	
	this.options.callback(this.el.width, this.el.height, this.options.ucfid, this.el);

    if(event){Event.stop(event);}
      },

      draw: function(p) {

	var newWidth = this.origWidth - (this.pointer[0] - p[0]);
	var newHeight = (newWidth/this.origWidth) * this.origHeight;
	var maxWidth = Math.min($('secondlist').getWidth()-40, this.options.maxWidth);
	var maxHeight = this.options.maxHeight;
	var minWidth = this.options.minWidth;
	var minHeight = this.options.minHeight;
	
	// max size
	if (newWidth/newHeight > maxWidth/maxHeight) {
		// wider image, limit width
		if (newWidth > maxWidth) {
			newHeight = Math.round(maxWidth/newWidth * newHeight);
			newWidth = maxWidth;
		}
	}else{
		// taller image, limit height
		if (newHeight > maxHeight) {
			newWidth = Math.round(maxHeight/newHeight * newWidth);
			newHeight = maxHeight;
		}
	}
	
	// min size
	if (newWidth/newHeight > minWidth/minHeight) {
		// wider image, expand width
		if (newWidth < minWidth) {
			newHeight = Math.round(minWidth/newWidth * newHeight);
			newWidth = minWidth;
		}
	}else{
		// taller image, expand height
		if (newHeight < minHeight) {
			newWidth = Math.round(minHeight/newHeight * newWidth);
			newHeight = minHeight;
		}
	}

	this.el.width = newWidth;
	this.el.height = newHeight;

	// b/c we rely on attributes, clear css width/height
	this.el.style.width = '';
	this.el.style.height = '';

	this.positionResizer();

      }

    };

    Weebly.ImageResizeArray = {

      images: [],

      init: function(el) {

	if(this.images.length == 0) {

	  this.eventMouseUp = this.stopResize.bindAsEventListener(this);
	  this.eventMouseMove = this.updateResize.bindAsEventListener(this);

	  Event.observe(document, "mouseup", this.eventMouseUp);
	  Event.observe(document.body, "mouseleave", this.eventMouseUp); // simulate a mouseup when mouse leaves browser window (for IE)
	  Event.observe(document, "mousemove", this.eventMouseMove);

	}
	this.images.push(el);

      },

      kill: function(el) {

	this.images = this.images.reject(function(d) { return d==el });

	if(this.images.length == 0) {
	  Event.stopObserving(document, "mouseup", this.eventMouseUp);
	  Event.stopObserving(document.body, "mouseleave", this.eventMouseUp);
	  Event.stopObserving(document, "mousemove", this.eventMouseMove);
	}

      },

      setCurrent: function(el) {
	this.currentImage = el;
      },

      removeCurrent: function() {
	this.currentImage = null;
      },

      updateResize: function(event) {
	if (!this.currentImage) return;

	var p = [Event.pointerX(event), Event.pointerY(event)];

	// Fix for Mozilla-based browsers
	if (this._lastP && (this._lastP.inspect() == p.inspect())) return;
	this._lastP = p;

	this.currentImage.updateResize(event, p);

      },

      stopResize: function(event) {
	if (!this.currentImage) return;

	this._lastP = null;
	this.currentImage.endResize(event);
	this.currentImage = null;

      }


    };

    //------------
    /// End of ImageResize module
    ////

    function onResize(width, height, ucfid, el) {
      new Ajax.Request(ajax, {parameters:'pos=resizeimage&ucfid='+ucfid+'&width='+width+"&height="+height+'&cookie='+document.cookie, onSuccess:function(t) { handlerFuncOnResize(t, el, width, height) }, onFailure:errFunc, asynchronous:false});
    }

    function handlerFuncOnResize(t, el, width, height) {

	t.responseText = t.responseText.replace(/\r\n/, '').replace(/\n/, '');
	t.responseText = t.responseText.replace(/\?[0-9x]*$/, '');

	var src = t.responseText + "?" + width; // + "x" + height;
	el.width = width;
	el.removeAttribute('height');
	el.src = src;

	var property = Weebly.Elements.getPropertyByReference('<!SRC!>');
	if (property.cfpid) {

	  property.propertyresult = src;
	  $(''+property.ucfpid).value = src;
	  //Weebly.Elements.saveProperties();

	}

	var identifier = t.responseText.replace(/.?uploads\/[0-9]+\//, "").replace(/\r\n/, '').replace(/\n/, '');
	imageResizeId[identifier] = newId;
    }

    // Weebly ImageGallery
    // -------
    Weebly.ImageGallery = {
      Version        : '0.1',
      Author         : 'David Rusenko',
      Company        : 'Weebly, Inc.',
      currentElement : null,
    
      init: function() {

	if (!this.currentElement) { return; }
	
	Sortable.create(this.currentElement+"-gallery", {
		constraint: false,
		onChange: Weebly.ImageGallery.onChange
	});
	
	$$("#"+this.currentElement+"-gallery .imageControls").each(function(el) { el.style.display = "block"; el.parentNode.style.cursor = "move"; });
	Behaviour.apply();
	
	// hack over a hack ~ashaw
	// before this workaround, dontSave was set to true the first time after uploading an image,
	//  voiding the first saveProperty to the imagegallery after the upload
	swfu.dontSave = 0;

      },

      destroy: function() {

	if (!this.currentElement) { return; }
    if($(this.currentElement+"-gallery")){Sortable.destroy(this.currentElement+"-gallery");}
	$$("#"+this.currentElement+"-gallery .imageControls").each(function(el) { el.style.display = "none"; el.parentNode.style.cursor = "auto"; });
	Behaviour.apply();
	this.currentElement = null;
	
		Weebly.ImageGallery.clearCurrentCaptionForm();

      },

      setCurrent: function(elementId) {

	Weebly.ImageGallery.currentElement = elementId;

      },

      onChange: function() {

			this.currentElement = Weebly.ImageGallery.currentElement;
			if (!this.currentElement || !Weebly.Elements.currentElement || !Weebly.Elements.getPropertyByReference('<!IMAGESET!>').cfpid) { return; }

			var thumbnailData = [];
			var thumbnailCnt = 0;
			$$("#"+this.currentElement+"-gallery img.galleryImage").each(function(thumbnailImg) {
				var url = thumbnailImg.src.replace(/^.*\/uploads\//, ""),
					width = thumbnailImg.readAttribute('_width'),
					height = thumbnailImg.readAttribute('_height'),
					caption = thumbnailImg.readAttribute('_caption'),
					link = thumbnailImg.readAttribute('_link'),
					o = { url: url };
				if (width) o.width = width;
				if (height) o.height = height;
				if (caption) o.caption = filterCaptionData(caption);
				if (link) o.link = link;
				thumbnailData.push(o);
				thumbnailCnt++;
			});

			Weebly.Elements.getPropertyByReference('<!IMAGESET!>').propertyresult = thumbnailData.toJSON();
			Weebly.Elements.saveProperties();

			if (thumbnailCnt == 0) {
				$(''+this.currentElement).innerHTML = Weebly.ImageGallery.update();
			}
			
			Weebly.ImageGallery.clearCurrentCaptionForm(true);

      },

      update: function(elementId, columns, spacing, border, crop, imageSet) {
		
			if (typeof imageSet == 'undefined') {
				var prop = Weebly.Elements.getPropertyByReference('<!IMAGESET!>');
				if (prop) imageSet = prop.propertyresult;
			}
			
			if (imageSet) {
				try {
					imageSet = imageSet.evalJSON();
				} catch (e) {
					imageSet = [];
				}
			}

			if (typeof columns == 'undefined') {
				var prop = Weebly.Elements.getPropertyByReference('<!COLUMNS!>');
				if (prop) {
					var columnInput = $(prop.ucfpid);
					columns = columnInput ? columnInput.value : '';
				}
			}
		
			if (typeof spacing == 'undefined') {
				var prop = Weebly.Elements.getPropertyByReference('<!SPACING!>');
				if (prop) {
					var spacingInput = $(prop.ucfpid);
					spacing = spacingInput ? spacingInput.value : '';
				}
			}
			
			if (typeof border == 'undefined') {
				var prop = Weebly.Elements.getPropertyByReference('<!BORDER!>');
				if (prop) {
					var borderInput = $(prop.ucfpid);
					border = borderInput ? parseInt(borderInput.value) : 0;
				}
			}else{
				border = parseInt(border);
			}
			
			var borderCss = border ? ('border-width:1px;' + (border > 1 ? 'padding:' + (border - 1) + 'px;' : '')) : '';
			
			if (typeof crop == 'undefined') {
				var prop = Weebly.Elements.getPropertyByReference('<!CROPPING!>');
				if (prop) {
					var croppingInput = $(prop.ucfpid);
					crop = croppingInput ? parseInt(croppingInput.value) : 0;
				}
			}else{
				crop = parseInt(crop);
			}
			
			if (!elementId) {
				elementId = Weebly.Elements.ucfid;
			}

			if (!imageSet || imageSet == "" || !imageSet.length) {
				imageSet = [{ url:'/weebly/images/upload_images_01.jpg', width:250, height:168 }];
			}

			this.currentElement = elementId;

			var toWrite = '<ul id="'+elementId+'-gallery" class="imageGallery" style="line-height: 0px; margin: 0; padding: 0; list-style: none;">';
			var width = (100 / columns - 0.05).toFixed(2);
			
			var frameRatio;
			if (crop) {
				frameRatio = 1;
			}else{
				frameRatio = 333/250;
			}
			
			var framePercentHeight = (1/frameRatio * 100).toFixed(2);
			var imageUrl, imageWidth, imageHeight, imageRatio, imageLink, imageCaption;

			for(var x = 0; x < imageSet.length; x++) {

				if (!imageSet[x]) continue;
				
				if (typeof imageSet[x] == 'string') {
					imageUrl = imageSet[x];
					imageWidth = null;
					imageHeight = null;
					imageLink = null;
					imageCaption = '';				
				}else{
					imageUrl = imageSet[x].url;
					imageWidth = parseInt(imageSet[x].width);
					imageHeight = parseInt(imageSet[x].height);
					imageLink = imageSet[x].link;
					imageCaption = filterCaptionData(imageSet[x].caption || '');
				}
				
				if (imageWidth && imageHeight) {
					imageRatio = imageWidth / imageHeight;
				}else{
					imageRatio = 333/250;
				}
				
				if (!imageUrl.match("/weebly/images/")) {
					imageUrl = '/uploads/' + imageUrl;
				}
				
				var imagePercentWidth, imagePercentTop, imagePercentLeft;
				if (crop) {
					if (imageRatio > frameRatio) {
						// wide
						imagePercentWidth = (imageRatio * 100).toFixed(2);
						imagePercentTop = 0;
						imagePercentLeft = -((imageRatio - 1) / 2 * 100).toFixed(2); // bad: only works with squares
					}else{
						// tall
						imagePercentWidth = 100;
						imagePercentTop = -((1/imageRatio - 1) / 2 * 100).toFixed(2); // bad: only works with squares
						imagePercentLeft = 0;
					}
				}else{
					if (imageRatio > frameRatio) {
						// wide
						imagePercentWidth = 100;
						imagePercentTop = ((1 - frameRatio/imageRatio) / 2 * 100).toFixed(2);
						imagePercentLeft = 0;
					}else{
						// tall
						imagePercentWidth = (imageRatio/frameRatio * 100).toFixed(2);
						imagePercentTop = 0;
						imagePercentLeft = ((1 - imageRatio/frameRatio) / 2 * 100).toFixed(2);
					}
				}

				toWrite +=
					"<li id='" + elementId + "-imageContainer" + x + "' style='float:left;width:" + width +
						"%;margin:0;list-style:none;position:relative;z-index:5'>";
					
				toWrite +=
					"<div id='" + elementId + "-insideImageContainer" + x + "' style='position:relative;margin:" + spacing + ";" +
						((crop || !border) ? '' : "padding:0 " + (border*2) + "px " + (border*2) + "px 0;") + "'>";
				
				if (crop && border)
					toWrite +=
						"<div class='galleryImageBorder' style='" + borderCss + "'>";
				
				toWrite +=
					"<div style='position:relative;width:100%;padding:0 0 " + framePercentHeight + "%;" +
						(crop ? "overflow:hidden;" : '') + "'>";
					
				toWrite +=
					"<img src='" + imageUrl + "' class='galleryImage" + ((crop || !border) ? '' : ' galleryImageBorder') + "' " +
						(imageLink ? "_link='" + imageLink + "' " : '') +
						(imageCaption ? "_caption='" + imageCaption + "' " : '') + 
						(imageWidth ? "_width='" + imageWidth + "' _height='" + imageHeight + "' " : '') +
						"style='position:absolute;" + ((crop || !border) ? 'border:0;' : borderCss) +
							"width:" + imagePercentWidth + "%;top:" + imagePercentTop + "%;left:" + imagePercentLeft + "%' />";
					
				toWrite += "</div>";
					
				if (imageUrl != '/weebly/images/upload_images_01.jpg') {
				
					toWrite += "<div class='imageControls' style='display:none;top:" +
						(crop ? "0;left:0;width:100" : imagePercentTop + "%;left:" + imagePercentLeft + "%;width:" + imagePercentWidth) +
						"%'><div class='imageControls-inner1'><div class='imageControls-inner2'>";
						
					toWrite += "<div class='imageControls-icon imageControls-link" + (imageLink ? ' glowing' : '') + "'>";
					toWrite += "<a href='#' onclick='Weebly.ImageGallery.chooseThumbnailLink(this);return false;'></a>";
					toWrite += "</div>";
					
					toWrite += "<div class='imageControls-icon imageControls-caption" + (imageCaption ? ' glowing' : '') + "'>";
					toWrite += "<a href='#' onclick='Weebly.ImageGallery.chooseThumbnailCaption(this);return false;'></a>";
					toWrite += "</div>";
				
					toWrite += "<div class='imageControls-icon imageControls-delete'>";
					toWrite += "<a href='#' onclick='Weebly.ImageGallery.removeNode(this);return false;'></a>";
					toWrite += "</div>";
				
					toWrite += "</div></div></div>";
				}
				
				if (crop && border) toWrite += "</div>";
				
				toWrite += "</div>";
				toWrite += "</li>";
				
			}

			toWrite += "<span style='display: block; clear: both; height: 0px; overflow: hidden;'></span>";
			toWrite += "</ul>";

			return toWrite;

      },

      removeNode: function(element) {
		$(element).up('li').remove();
		Weebly.ImageGallery.onChange();
      },
      
      chooseThumbnailLink: function(element) {
        Weebly.ImageGallery.currentLinkButton = $(element).up();
      	Weebly.ImageGallery.currentLinkThumbnail = $(element).up('li').down('img');
      	Weebly.Linker.show('Weebly.ImageGallery.setLink', {'top': 185, 'left': 250}, ['linkerWebsite', 'linkerWeebly', 'linkerEmail', 'linkerFile'], 'linkerWeebly', currentBox, true);
      },
      
      setLink: function(link) {
      	Weebly.ImageGallery.currentLinkThumbnail.writeAttribute('_link', link);
      	Weebly.ImageGallery.onChange();
      	if (link) {
      		Weebly.ImageGallery.currentLinkButton.addClassName('glowing');
      	}else{
      		Weebly.ImageGallery.currentLinkButton.removeClassName('glowing');
      	}
      },
      
      chooseThumbnailCaption: function(element) {
      	Weebly.ImageGallery.clearCurrentCaptionForm();
      	
      	var button = Weebly.ImageGallery.currentControlButton = $(element).up(),
      		buttonPosition = button.positionedOffset(),
      		controlsInner = button.up().up(),
      		imageWrap = Weebly.ImageGallery.currentCaptionImageWrap = controlsInner.up('li'),
      		image = imageWrap.down('img'),
      		captionForm,
      		captionInput;
      		
      	imageWrap.style.zIndex = 6;
      	button.addClassName('highlighted');
      	
      	var origCaptionData = image.readAttribute('_caption');
      	controlsInner.insert(captionForm = Weebly.ImageGallery.currentCaptionForm =
		  	new Element('form', { 'class':'galleryCaptionForm', style:'left:'+(buttonPosition.left+1)+'px' })
		  		.observe('submit', function(ev) {
		  			ev.stop();
		  		})
		  		.observe('click', function(ev) {
		  			ev.stop();
		  		})
		  		.insert(captionInput = new Element('textarea', {'class':origCaptionData?'':'empty'})
		  			.observe('focus', function() {
		  				if (captionInput.hasClassName('empty')) {
		  					captionInput.removeClassName('empty');
		  					captionInput.value = '';
		  				}
		  			})
		  			.observe('blur', function() {
		  				if (!captionInput.value.strip()) {
		  					captionInput.addClassName('empty');
		  					captionInput.value = 'enter a caption';
		  				}
		  			}))
		  		.insert(new Element('div', { 'class':'galleryCaptionForm-foot' })
		  			.insert(new Element('span')
		  				.update('Caption will appear under the full-sized image.'))
			  		.insert(new Element('input', { type:'submit', value:'Save' })
			  			.observe('click', function() {
			  				var captionData;
			  				if (captionInput.hasClassName('empty')) {
			  					captionData = '';
			  				}else{
				  				captionData = filterCaptionData(captionInput.value);
				  			}
			  				image.writeAttribute('_caption', captionData);
			  				Weebly.ImageGallery.clearCurrentCaptionForm();
			  				Weebly.ImageGallery.onChange();
			  				if (captionData) {
			  					button.addClassName('glowing');
			  				}else{
			  					button.removeClassName('glowing');
			  				}
			  			}))
			  		.insert(new Element('input', { type:'button', value:'Cancel' })
			  			.observe('click', function(ev) {
			  				Weebly.ImageGallery.clearCurrentCaptionForm();
			  			}))));
		  captionInput.value = origCaptionData ? unfilterCaptionData(origCaptionData) : 'enter a caption';
      },
      
      clearCurrentCaptionForm: function(dontChangeZ) {
      	if (Weebly.ImageGallery.currentCaptionForm) {
      		Weebly.ImageGallery.currentCaptionForm.remove();
      		Weebly.ImageGallery.currentCaptionForm = null;
      	}
      	if (Weebly.ImageGallery.currentControlButton) {
      		Weebly.ImageGallery.currentControlButton.removeClassName('highlighted');
      		Weebly.ImageGallery.currentControlButton = null;
      	}
      	if (!dontChangeZ && Weebly.ImageGallery.currentCaptionImageWrap) {
      		Weebly.ImageGallery.currentCaptionImageWrap.style.zIndex = 5;
      		Weebly.ImageGallery.currentCaptionImageWrap = null;
      	}
      }

    };
    
function filterCaptionData(s) {
	return s.strip()
		.replace(/\s+/g, ' ')
		.replace(/'/g, '&#039;')
		.replace(/"/g, '&quot;')
		.replace(/!>/g, '!')
}

function unfilterCaptionData(s) {
	return s.replace(/&#039;/g, "'").replace(/&quot;/g, '"');
}


var swfu = {};

function uploadSuccess(file, data, response) {

	if (data && data == "%%TOOLARGE%%") {
	  $('upload'+file.id).innerHTML = /*tl(*/"<span style=\"color: red;\">Image is too large. Please resize and try again.</span>"/*)tl*/;
  	  removeProgressBox(file.id);
	}else if(data && data == "%%LARGESITE%%") {
	  $('upload'+file.id).innerHTML = /*tl(*/"<span style=\"color: red;\">Your site is too large. Please delete a few elements, and try again.</span>"/*)tl*/;
          removeProgressBox(file.id);
	}else if(data && data == "%%LARGESITESOFTLIMIT%%") {
	  $('upload'+file.id).innerHTML = /*tl(*/"<span style=\"color: red;\">Your account has uploaded quite a few files. Please contact support to continue uploading.</span>"/*)tl*/;
          removeProgressBox(file.id);
	} else if (data && data.match(/%%QUEUEID:[0-9]+%%[^%]+%%/)) {
	  var matches = data.match(/%%QUEUEID:([0-9]+)%%([^%]+)%%/);
	  if (file._parameters) {
	  	swfu.uploadOptions[file.id] = file._parameters; // from plain file uploader
	  }
	  swfu.uploadOptions[file.id].qid = matches[1];
	  swfu.uploadOptions[file.id].video_name = matches[2];
	}

}

function uploadFileComplete(file, passedOpts) {

  var opt = passedOpts || swfu.uploadOptions[file.id];
  if (opt.done) { return; }

  $('upload'+file.id).innerHTML = "Upload Complete";

  if (opt.type != "video") {
    removeProgressBox();
  }

  if(opt.type == "image") {

    if ($(""+opt.ucfpid)) {
      $(""+opt.ucfpid).value = '/uploads/'+userIDLocation+'/'+opt.newid+'.'+opt.ext.toLowerCase();
    }

    if ($(opt.ucfid) && $(opt.ucfid).down) {
      var prodImage = $(opt.ucfid).down('.product-image');
    } else {
      var prodImage = null;
    }
    
	  var ext = opt.ext.toLowerCase();
	  if (ext == 'bmp') {
	  	ext = 'jpg'; // bmp's are always converted to jpegs
	  }

    if(prodImage){
    
        prodImage.src = '/uploads/'+userIDLocation+'/'+opt.newid+'.'+ext;
        
    } else {

		// Try to update the image!
		try {
		  var img = $(""+opt.ucfid).getElementsByTagName("IMG")[0];
		  img.src = '/uploads/'+userIDLocation+'/'+opt.newid+'.'+ext;
		  img.style.width = 'auto';
		  img.style.height = 'auto';
		} catch (e) { }

    }

  } else if(opt.type == "header") {
        currentStyleNum = Math.floor(Math.random()*10000000001);
        writeTheme(currentTheme);
	currentHeader = "not.null";
  } else if(opt.type == "file" || opt.type == "audio" || opt.type == "gallery" || opt.type == "doc" || opt.type == "flash") {
	swfu.dontSave = 1;
	updateList(currentPage);
  } else if(opt.type == "video") {
	$('upload'+file.id).innerHTML = /*tl(*/"Waiting for video to start encoding..."/*)tl*/;
	new monitorEncoding(file.id, opt.ucfid, opt.qid, opt.video_name);
  }

  var stats = swfu.getStats();
  if (stats && stats.files_queued > 0) {
    swfu.startUpload();
  }

}

function dialogStart(){
    if (!swfu.currentDialog) { swfu.currentDialog = {}; }
    swfu.currentDialog.ucfid = swfu.current.ucfid;
    swfu.currentDialog.ucfpid = swfu.current.ucfpid;
}

function dialogComplete(numFilesSelected, numFilesQueued) {

  var stats = swfu.getStats();

  if (stats && stats.files_queued > 0) {
    swfu.startUpload();
  }

}

function queueError(file, errcode, msg) { 

  if (errcode == SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT) {
    if (!isPro()) {
      alertProFeatures(/*tl(*/"Please sign-up for a pro account to upload files larger than 5 MB (up to 100MB)"/*)tl*/, "main");
    } else {
      showError(/*tl(*/'This file is too big! You can only upload files smaller than 100MB.'/*)tl*/);
    }
  }

}

function uploadError(file, errcode, msg) {
  if (errcode == SWFUpload.UPLOAD_ERROR.FILE_CANCELLED) {

    $('upload'+file.id).innerHTML = /*tl(*/"Upload cancelled."/*)tl*/;

  } else {

    //$('upload'+file.id).innerHTML = /*tl(*/"Upload Error. Please try again."/*)tl*/+" ("+errcode+")";
    
    // show oops dialog with option to use plain uploader
    showUploadError(errcode, msg);

  }
  fireTrackingEvent("WeeblyError", "Upload Error", errcode + ' - '+msg);

  removeProgressBox(file.id);

}


function uploadStart(file) { 

  swfu.currentUpload = file.id;
  
  if (!file.plainUploader) {
      swfu.uploadOptions[file.id].ext = file.name.replace(/.*\.([^\.]*)/, "$1");
	  var postParams = {'type':swfu.uploadOptions[file.id].type,'width':swfu.uploadOptions[file.id].size,'ucfid':swfu.uploadOptions[file.id].ucfid,'ucfpid':swfu.uploadOptions[file.id].ucfpid, 'newid':swfu.uploadOptions[file.id].newid, 'cookies':document.cookie.match(/WeeblySession=[^;]+(;|$)/)[0]};
	  swfu.setPostParams(postParams);
	  swfu.setUploadURL("/weebly/fileUpload.php");
  }

  var newEl = document.createElement('li');
  newEl.id = 'upload'+file.id;
  newEl.innerHTML =
  	(file.plainUploader ?
  		/*tl(*/"Uploading File. Please Wait..."/*)tl*/ + "<br/><br/>" : // dont display progress indicator for non-flash uploader
  		/*tl(*/"Uploading File"/*)tl*/ + "<br/><div id='uploadContainer"+file.id+"' style='font-size: 10px;'><div style='height: 5px; overflow: hidden; background: #ccc; margin: 5px 0;'><div style='height: 5px; overflow: hidden; background: #777; width: 0px;' id='"+file.id+"progress'></div></div></div>"
  		) +
  	"<a href='#' onclick=\"cancelFile('"+file.id+"'); return false;\">"+/*tl(*/"Cancel"/*)tl*/+"</a>";
  	
  $('notifications').appendChild(newEl);

}

function uploadProgress(file, bytesLoaded, totalBytes) {
	if (file.size == bytesLoaded) {
	  $('uploadContainer'+swfu.currentUpload).innerHTML = "<i style='color: red;'>"+/*tl(*/"Virus scanning..."/*)tl*/+"</i>";
	} else {
       _uploadProgress(bytesLoaded / file.size);
	}
}

function _uploadProgress(frac) {
	var progress = document.getElementById(swfu.currentUpload + "progress");
	progress.style.width = Math.ceil(frac * 180) + "px";
}

function uploadFileQueued(file) {

	if (!swfu.uploadOptions) { swfu.uploadOptions = {}; }
	if (!swfu.uploadOptions[file.id]) { swfu.uploadOptions[file.id] = {}; }

  	swfu.uploadOptions[file.id].ext = file.name.replace(/.*\.([^\.]*)/, "$1");
  	swfu.uploadOptions[file.id].newid = Math.floor(Math.random()*10000001);
        swfu.uploadOptions[file.id].size = swfu.current.size;
        swfu.uploadOptions[file.id].type = swfu.current.type;
        swfu.uploadOptions[file.id].ucfid = swfu.currentDialog.ucfid;
        swfu.uploadOptions[file.id].ucfpid = swfu.currentDialog.ucfpid;
        swfu.uploadOptions[file.id].fileid = file.id;
        swfu.uploadOptions[file.id].done = null;

	Weebly.Elements.unselectElement();
}

function selectDefaultImageUpload(ucfid){
    var el = $(''+ucfid).up('.inside');
    var props = el.down('form')['pfield'].value.evalJSON();
    var ucfpid = '';
    var type = '';
    var width = 250;
    for(property in props[ucfid]){
        if(typeof(props[ucfid][property]) === 'object'){
            if(props[ucfid][property].propertyresult &&  isUploaderImageSrc(props[ucfid][property].propertyresult)){
                ucfpid = props[ucfid][property].ucfpid;
                if(props[ucfid][property].propertyresult.match(/na\.jpg/)){
                    type = 'image';
                    if(el.down('.product-image')){
                        switch(props[ucfid][69650731].propertyresult){
                            case 'grid':
                              width = 164;
                              break;
                            case 'large':
                              width = 225;
                              break;
                            case 'small':
                              width = 125;
                              break;
                            case 'long':
                              width = 175;
                              break;
                        }
                    }
                }
            }
            if(props[ucfid][property].referenceproperty === '<!VIDEOFILE!>'){
                type = 'video';
                width = '';
                ucfpid = props[ucfid][property].ucfpid;
                break;
            }
            if(props[ucfid][property].referenceproperty === '<!IMAGESET!>'){
                ucfpid = props[ucfid][property].ucfpid;
                type = 'gallery';
            }
        }
    }
    selectUpload(type, width, ucfid, ucfpid);
}

function selectUpload(uploadType, size, ucfid, ucfpid) {
	//console.log(ucfpid);
	
	Weebly.Elements.upload = 1;

	var fileDescription = "All files...";
	var fileTypes = "*.*";

	if (uploadType == "audio") {
	  fileDescription = "MP3 files...";
	  fileTypes = "*.mp3";
	} else if (uploadType == "header" || uploadType == "image" || uploadType == "gallery") {
	  fileDescription = "Image files...";
	  fileTypes = "*.gif;*.jpg;*.jpeg;*.png;*.bmp";
	} else if (uploadType == "file") {
	  fileDescription = "All files...";
	  fileTypes = "*.*";
	} else if (uploadType == "doc") {
	  fileDescription = "Document files...";
	  fileTypes = "*.doc;*.docx;*.ppt;*.pptx;*.pps;*.xls;*.xlsx;*.pdf;*.ps;*.odt;*.odp;*.ods;*.odf;*.odg;*.sxw;*.sxi;*.sxc;*.sxd;*.tif;*.tiff;*.txt;*.rtf";
	} else if (uploadType == "video") {
	  fileDescription = "Video files...";
	  fileTypes = "*.*";
	} else if (uploadType == "flash") {
	  fileDescription = "SWF files...";
	  fileTypes = "*.swf";
	}

	if (!swfu.current) { swfu.current = {}; }
	swfu.current.size = size;
	swfu.current.type = uploadType;
	swfu.current.ucfid = ucfid;
	swfu.current.ucfpid = ucfpid;

	if(uploadType == "header" && headerSelected) {
	  unselectHeader();
	}
	
	// for html-base file uploader
	plainUploaderFileTypes(fileTypes, fileDescription);

	try {

	  swfu.setFileTypes(fileTypes, fileDescription);

	  if (uploadType == "gallery") {
	    swfu.setButtonAction(SWFUpload.BUTTON_ACTION.SELECT_FILES);
	  } else {
	    swfu.setButtonAction(SWFUpload.BUTTON_ACTION.SELECT_FILE);
	  }

	} catch (e) { }

}

function cancelFile(id) {
	$('upload'+id).innerHTML = "Upload Canceled";
	if (!cancelPlainUpload(id)) { // will return true of canceled successfully
		if (id == swfu.currentUpload) {
		  swfu.stopUpload();
		}
		swfu.cancelUpload(swfu.uploadOptions[id].fileid);
	}
	removeProgressBox(id);
}

function removeProgressBox(id) {
	id = id ? id : swfu.currentUpload;
	if (swfu.uploadOptions && swfu.uploadOptions[id]) {
  		swfu.uploadOptions[id].done = 1;
  		// might be using the plain image uploader
  	}
	setTimeout("Effect.Fade('upload"+id+"'); ", 2000);
	setTimeout("if($('upload"+id+"')) $('notifications').removeChild($('upload"+id+"'));", 5000);
}

function swfUploadLoaded() {

	try {
	  if (!swfu.containerShown) {
	    showFlashContainer();
	    hideFlashContainer();
	  }
	} catch (e) { }

}

function hideFlashContainer() {

	if ($('flashContainer') && !forceFlashPosition) {
	  $('flashContainer').style.zIndex = "-1";
      try{
          swfu.setButtonDimensions(1,1);
      }
      catch(e){}
	}

}

function showFlashContainer(zIndex) {

	swfu.containerShown = 1;

	if ($('flashContainer') && !forceFlashPosition) {
	  $('flashContainer').style.zIndex = (typeof zIndex === undefined) ? "21" : zIndex; // TODO: ask drew about zindex stuff
      try{
          swfu.setButtonDimensions(600,600);
      }
      catch(e){}
	}

}

var monitorEncoding = Class.create({

	initialize: function(id, ucfid, qid, video_name) {

	  this.id = id;
	  this.qid = qid;
	  this.ucfid = ucfid;
	  this.video_name = video_name;
	  this.timeoutID = window.setInterval(this.check.bind(this), 5000);

	},

	check: function() {

	  new Ajax.Request(ajax, {parameters:'pos=checkencoding&qid='+this.qid+'&cookie='+document.cookie, onSuccess:this.returnCheck.bind(this), bgRequest: true});

	},

	returnCheck: function(t) {

	  if (t.responseText.match("processing:")) {

	    if (!this.encoding) {
	  
	      $('upload'+this.id).innerHTML = "Encoding video... <br/><div style='height: 5px; overflow: hidden; background: #ccc; margin: 5px 0;'><div style='height: 5px; overflow: hidden; background: #777; width: 0px;' id='"+this.qid+"qprogress'></div></div>";

	      // Magic: this is where we guess at how long a video will take to encode
	      // In any case, we try to underpromise here...
	      this.length = (t.responseText.split(":")[1] - 5) * 0.75;

	      this.encodingStarted = new Date().getTime()/1000;
	      this.encodingTimeoutID = window.setInterval(this.updateProgress.bind(this), 200);
	      this.encoding = true;

	    }

	  } else if (t.responseText.match("success")) {
	    $('upload'+this.id).innerHTML = "Video encoding finished!";

	    if ($(this.ucfid)) {

	      try {

		videoFileName = this.video_name.replace(/\.[^\.]+$/, ".flv");
		imageFileName = this.video_name.replace(/\.[^\.]+$/, ".jpg");

		// If Element is currently open, update its definition
		if (Weebly.Elements.currentElement && Weebly.Elements.pfield[this.ucfid]) {
		  Weebly.Elements.getPropertyByReference('<!IMAGEFILE!>').propertyresult = imageFileName;
		  Weebly.Elements.getPropertyByReference('<!VIDEOFILE!>').propertyresult = videoFileName;
		} else {
		  swfu.dontSave = 1;
		  updateList(currentPage);
		}

		/*
 		// Nice thought, but old element pfield doesn't get updated
		var iframe = $(this.ucfid).down('iframe');
		var newSrc = iframe.src;
		newSrc = newSrc.safeReplace(/video=[^&]*&?/, "video="+videoFileName+"&");
		newSrc = newSrc.safeReplace(/image=[^&]*&?/, "image="+imageFileName+"&");
		iframe.src = newSrc;
		*/

	      } catch(e) { }

	    }

	    this.cleanup();

	  } else if (t.responseText.match("failed")) {
	    $('upload'+this.id).innerHTML = "Video encoding failed. The uploaded file may be corrupted or may not be a supported video format.";
	    this.cleanup();
	  }
	},

	updateProgress: function() {

	  var elapsed = new Date().getTime()/1000 - this.encodingStarted;

	  if ($(this.qid+'qprogress') && elapsed < this.length) {
	    $(this.qid+'qprogress').style.width = Math.ceil(elapsed / this.length * 180) + "px";
	  } else {
	    window.clearInterval(this.encodingTimeoutID);
	  }

	},

	cleanup: function() {

	  removeProgressBox(this.id);
	  window.clearInterval(this.timeoutID);

	}

});


/********* plain uploader *********/

var plainUploaderHash = {};

function getPlainUploader(name) {
	var res = plainUploaderHash[name];
	if (!res) {
		var options = {
			queued: function(fileInfo) {
				Weebly.Elements.unselectElement();
				fileInfo._parameters = { // store here, as opposed to using swfu.uploadOptions like in uploadFileQueued()
				  	ext: fileInfo.name.replace(/.*\.([^\.]*)/, "$1"),
				  	newid: fileInfo.id,
					width: swfu.current.size, // this stuff is stored by selectUpload()
					type: swfu.current.type,  // ... in the swfu object :(
					ucfid: swfu.current.ucfid,
					ucfpid: swfu.current.ucfpid
				};
				//if (name == 'error') {
					hideError();
					fireTrackingEvent("WeeblyAltUpload", "Start", "orig swfu error: " + this._swfu_msg + " - " + this._swfu_errcode);
				//}
			},
			start: function(fileInfo) {
				this.parameters = fileInfo._parameters; // will set additional POST params before sending out
				uploadStart(fileInfo); // will bring up progress box
			},
			progress: function(fileInfo, progress) {
				//_uploadProgress(progress); // will update progress box
			},
			complete: function(fileInfo, data) {
				uploadSuccess(fileInfo, data);
				uploadFileComplete(fileInfo, fileInfo._parameters);
				//if (name == 'error') {
					fireTrackingEvent("WeeblyAltUpload", "Complete", "orig swfu error: " + this._swfu_msg + " - " + this._swfu_errcode);
				//}
			}
		};
		plainUploaderHash[name] = res = new Weebly.PlainUploader(options);
	}
	return res;
}

function showUploadError(errcode, msg) {
	showError(
		"<p>" +
		/*tl(*/"There was an error uploading your file"/*)tl*/+" (error "+errcode+"). <br />" +
		/*tl(*/"Would you like to try again using our basic file uploader?"/*)tl*/ +
		"</p>" +
		"<div style='padding-bottom:10px'>" +
			"<span id='upload-fallback-button' style='position:relative;display:inline-block'>" +
				"<button style='font-size:16px'>" +
					/*tl(*/"Try Basic Uploader"/*)tl*/ +
				"</button>" +
			"</span>" +
		"</div>",
		'',  // no xhr, needs to be non-object
		true // dont track oops event in GA
	);
	var uploader = getPlainUploader('error');
	uploader._swfu_errcode = errcode;
	uploader._swfu_msg = msg;
	var buttonWrap = $('upload-fallback-button');
	uploader.show(buttonWrap, buttonWrap);
}




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


/*
 * Responsible for flyout menus within the editor + on a published page.
 * Also responsible for condensing overflowing nav and making a "more..." link.
 *
 * Author: Adam Shaw
 */
 
(function() {

	/****************************** publically available functions ****************************/
	
	var moreItemHTML;
	var activeLiId;
	var currentPageId;
	
	// called from a published page
	window.initPublishedFlyoutMenus = function(topLevelSummary, cpid, moreItemHTML, aLiId, isPreview) {
		currentPageId = cpid;
		if (topLevelSummary.length > 0) {
			var go = function() {
				activeLiId = aLiId;
				var container = document.createElement('div');
				container.id = 'weebly-menus';
				$(document.body).insert(container);
				var firstItem = navElm(topLevelSummary[0].id);
				if (firstItem) {
					window.navFlyoutMenu = new FlyoutMenu(firstItem.up(), {
						relocate: container,
						aLiId: aLiId
					});
					condenseNav(topLevelSummary, moreItemHTML);
				}
			}
			if (isPreview) {
				go(); // css has been written in html <style> tag, no need to check if loaded
			}else{
				whenThemeCSSLoaded(go);
			}
		}
	};
	
	// called from the editor
	window.initEditorFlyoutMenus = function() {
		whenThemeCSSLoaded(function() {
			function go() {
				var topLevelSummary = Weebly.PageManager.getTopLevelSummary();
				if (topLevelSummary.length > 0) {
					var listItem0 = navElm(topLevelSummary[0].id);
					if (listItem0) {
						var listElement = listItem0.up();
						if (listElement && listElement.nodeName && !listElement.nodeName.match(/(table|tbody|thead|tr)/i)) {
							window.navFlyoutMenu = new FlyoutMenu(listElement, {
								relocate: 'weebly-menus'
							});
							moreItemHTML = currentThemeDefinition['menuRegular'];
							moreItemHTML = moreItemHTML.replace('%%MENUITEMLINK%%', '#');
							moreItemHTML = moreItemHTML.replace('%%MENUITEMTITLE%%', /*tl(*/'more...'/*)tl*/);
							moreItemHTML = "<span class='weebly-nav-handle weebly-nav-more'>" + moreItemHTML + "</span>";
							condenseNav(topLevelSummary, moreItemHTML);
						}else{
							window.navFlyoutMenu = null;
						}
					}else{
						window.navFlyoutMenu = null;
					}
				}else{
					window.navFlyoutMenu = null;
				}
			}
			if (Prototype.Browser.WebKit) {
				// this solves a webkit bug where the <span>s within the <ul> are displayes as block
				// this problem has nothing to do with the flyout code, but this was the most convenient place to put it ~ashaw
				var handles = $$('#icontent span.weebly-nav-handle');
				handles.each(Element.hide);
				setTimeout(function() {
					handles.each(Element.show);
					go();
				},0);
			}else{
				go();
			}
		});
	};
	
	// called from the editor when nav positioning might have changed
	window.refreshNavCondense = function(callback) {
		if (window.navFlyoutMenu) {
			//console.log('refresh');
			condenseNav(Weebly.PageManager.getTopLevelSummary(), moreItemHTML);
		}
	};
	
	window.disableFlyouts = false;
	
	
	
	/*********************************** flyout menu class ************************************/

	window.FlyoutMenu = function(mainList, options) {

		mainList = $(mainList); // the element that contains all the nav elements
		options = options || {};
		
		// settings (an attempt at making FlyoutMenu portable)
		var listTag = options.listTag ? options.listTag.toLowerCase() : 'ul';
		var itemTag = options.itemTag ? options.itemTag.toLowerCase() : 'li';
		var delay = (options.delay || 0.5) * 1000;
		var slideDuration = options.slideDuration || 0.3;
		
		// if specified, all submenus will be detached from original place in DOM and put in here
		var relocate = options.relocate ? $(options.relocate) : false;
		
		// FYI
		// a 'handle' is an element that contains the templatable HTML for each page's nav link
		// a 'handle' may be a wrapping SPAN element (with className 'weebly-nav-handle')
		//   OR it may be the item itself (such as an LI)
		
		var allItems; // list of all nav items
		              // (the first child within a handle OR the handle itself)
		
		
		//
		// attach all event handlers and do state-keeping for flyout menus
		//
		
		function initItem(item) {
		
			item.style.position = 'relative'; // this gives more accurate offsets
			var innerAs = item.getElementsByTagName('a');
			if (innerAs.length) {
				innerAs[0].style.position = 'relative'; // more accurate offset (prevents IE bug)
				//innerAs[0].style.whiteSpace = 'nowrap'; // so an item doesn't wrap to a 2nd line and give us weird offsets/widths
			}
	
			// states
			var isSliding = false;
			var isExpanded = false;
			var isMouseoverItem = false;
			var mouseoverCnt = 0;
			
			var slidVertically = false;
			var slidRight = false;
		
			var sublistWrapper; // a DIV.weebly-menu-wrap OR null
			var sublist;        // a UL.weebly-menu OR null
			
			var currentEffect;
			
			
			//
			// expand a sublist on mouseover
			//
			
			function itemMouseover() {
				if (disableFlyouts) return false;
				mouseoverCnt++;
				isMouseoverItem = true;
				if (!isExpanded && !isSliding) {
					if (sublist) {
						// when a sublist is expanded, immediately contract all siblings' sublists
						getSiblings(item).each(function(sib) {
							if (sib._flyoutmenu_contract) {
								sib._flyoutmenu_contract();
							}
						});
						expandSublist();
					}
				}
			}
			
			
			//
			// contract sublist on mouseout (after delay)
			//
			
			function itemMouseout() {
				if (disableFlyouts) return false;
				isMouseoverItem = false;
				if (isExpanded) {
					var mouseoverCnt0 = mouseoverCnt;
					setTimeout(function() {
						if (mouseoverCnt == mouseoverCnt0 && isExpanded && !isSliding) {
							contractSublist();
						}
					}, delay);
				}
			}
			
			
			//
			// prevent contracting when sublist is moused over
			//
			
			function sublistWrapperMouseover() {
				if (disableFlyouts) return false;
				mouseoverCnt++;
			}
			
			
			//
			// do item's sublist's expand animation
			//
		
			function expandSublist() {
				isSliding = true;
				var opts = {
					duration: slideDuration,
					afterFinish: function() { // when animation has finished
						currentEffect = null;
						isSliding = false;
						isExpanded = true;
						if (!isMouseoverItem) {
							// if mouse was not over when animation finished, immediately contract
							contractSublist();
						}else{
							// attach methods for later hiding/contracting
							item._flyoutmenu_contract = contractSublist;
							item._flyoutmenu_hide = function() {
								isSliding = false;
								isExpanded = false;
								isMouseoverItem = false;
								item._flyoutmenu_contract = null;
								item._flyoutmenu_hide = null;
								sublistWrapper.hide();
							};
						}
					}
				};
				var massCoords = getItemMassCoords(item);
				var localOrigin = safeCumulativeOffset(sublistWrapper.getOffsetParent());
				if (inVerticalList(item, true, options.aLiId)) {
					// slide right on vertical nav
					slidVertically = false;
					sublistWrapper.style.top = -localOrigin.top + massCoords[0].top + 'px';
					var w = sublistWrapper.getWidth();
					if (massCoords[1].left + w > $(document.body).getWidth()) {
						slidRight = false;
						sublistWrapper.style.left = -localOrigin.left + massCoords[0].left - w + 'px';
						// currentEffect = Effect.SlideLeftIn(sublistWrapper, opts);
						// jakewent's SlideLeftIn sucks... just show it
						sublistWrapper.show();
						opts.afterFinish();
					}else{
						slidRight = true;
						sublistWrapper.style.left = -localOrigin.left + massCoords[1].left + 'px';
						currentEffect = Effect.SlideRightIn(sublistWrapper, opts);
					}
				}else{
					// slide down on horizontal nav
					slidVertically = true;
					sublistWrapper.style.top = -localOrigin.top + massCoords[1].top + 'px';
					var w = sublistWrapper.getWidth();
					if (massCoords[0].left + w > $(document.body).getWidth()) {
						slidRight = false;
						sublistWrapper.style.left = -localOrigin.left + massCoords[1].left - w + 'px';
					}else{
						slidRight = true;
						sublistWrapper.style.left = -localOrigin.left + massCoords[0].left + 'px';
					}
					currentEffect = Effect.SlideDown(sublistWrapper, opts);
				}
			}
			
			
			//
			// do item's sublist's contract animation
			//
		
			function contractSublist(mouseoverHack) {
				if (disableFlyouts || !item.parentNode) { // no parentNode?? removed from dom already? wtf!?
					// contractSublist is often called from a delay, might have been disabled in that time
					return;
				}
				if (mouseoverHack) {
					// IE6 wasn't registering the mouseout
					isMouseoverItem = false;
				}
				isSliding = true;
				item._flyoutmenu_contract = null;
				item._flyoutmenu_hide = null;
				var opts = {
					duration: slideDuration,
					afterFinish: function() {
						currentEffect = null;
						isSliding = false;
						isExpanded = false;
						if (isMouseoverItem) {
							// if mouseleft, but re-entered before animation finished
							// immediately expand sublist again
							expandSublist();
						}
					}
				}
				if (slidVertically) {
					currentEffect = Effect.SlideUp(sublistWrapper, opts);
				}else{
					if (slidRight) {
						currentEffect = Effect.SlideLeftOut(sublistWrapper, opts);
					}else{
						currentEffect = Effect.SlideRightOut(sublistWrapper, opts);
					}
				}
			}
			
			
			//
			// initialize submenu and attach events
			//
			
			sublist = getSublist(item, listTag);
			if (sublist) {
				sublistWrapper = sublist.up();
				sublistWrapper.style.position = 'absolute';
				sublistWrapper.hide(); // should already be display:none, but just in case
				if (relocate) {
					// since sublist is no longer a descendant of the item, mouse events
					// wont cascade. simulate this
					sublistWrapper.observe('mouseover', itemMouseover);
					sublistWrapper.observe('mouseout', itemMouseout);
				}else{
					// keep the submenu alive...
					sublistWrapper.observe('mouseover', sublistWrapperMouseover);
				}
			}
			item.observe('mouseover', itemMouseover);
			item.observe('mouseout', itemMouseout);
			
			
			//
			// attach a method for removing registered events
			// (returns the sublist wrapper)
			//
			
			item._flyoutmenu_destroy = function(removeSublist) {
				if (currentEffect) {
					// effect is still animating, kill it now
					currentEffect.cancel();
					currentEffect = null;
				}
				item.stopObserving('mouseover', itemMouseover);
				item.stopObserving('mouseout', itemMouseout);
				if (removeSublist) {
					if (sublistWrapper) {
						return sublistWrapper.remove(); // detach before returning
					}
				}
				else if (sublistWrapper) {
					if (relocate) {
						sublistWrapper.stopObserving('mouseover', itemMouseover);
						sublistWrapper.stopObserving('mouseout', itemMouseout);
					}else{
						sublistWrapper.stopObserving('mouseover', sublistWrapperMouseover);
					}
					return sublistWrapper;
				}
			};
				
		}
		
		
		//
		// methods for the FlyoutMenu object
		//
		
		// close all submenus with an animation
		this.contract = function() {
			allItems.each(function(item) {
				if (item._flyoutmenu_contract) {
					item._flyoutmenu_contract(true);
				}
			});
		};
		
		// hide all submenus immediately
		this.hideSubmenus = function() {
			allItems.each(function(item) {
				if (item._flyoutmenu_hide) {
					item._flyoutmenu_hide();
				}
			});
		};
		
		// detach all event handlers
		this.destroy = function() {
			allItems.each(function(item) {
				if (item._flyoutmenu_destroy) {
					item._flyoutmenu_destroy();
				}
			});
		};
		
		// initialize a top level item that has already been placed into mainList
		this.addItem = function(handle) { // todo: rename initTopLevelItem()
			var item = getRealTopLevelItem(handle);
			if (item) {
				initItem(item);
				var sublist = getSublist(item, listTag);
				if (sublist) {
					sublist.select(itemTag).each(initItem); // init all subitems
				}
				if (relocate && sublist) {
					relocate.insert(sublist.parentNode); // relocate sublist's wrap
				}
				allItems.push(item);
			}
		};
		
		// detach an item's event handlers and remove from DOM
		this.removeItem = function(handle) { // todo: rename
			var item = getRealTopLevelItem(handle);
			if (item) {
				if (item._flyoutmenu_destroy) {
					item._flyoutmenu_destroy(true);
				}
				item.remove();
				allItems = allItems.without(item);
			}
		};
		
		// accessor
		this.getMainList = function() {
			return mainList;
		};
		
		
		//
		// initialize allItems and relocate
		//
		
		allItems = getAllItems(mainList, itemTag);
		allItems.each(initItem);
		
		if (relocate) {
			getTopLevelItems(mainList).each(function(item) {
				var sublist = getSublist(item, listTag);
				if (sublist) {
					relocate.insert(sublist.parentNode);
				}
			});
		}

	};
	
	
	
	
	
	/****************************** more... link and menu *****************************/
	
	function condenseNav(topLevelSummary, moreItemHTML) { // can be called repeatedly for updating
		if (window.DISABLE_NAV_MORE) return;
		//console.log('condenseNav');
		var cpid = window.currentPage || currentPageId;
		var mainList = navFlyoutMenu.getMainList();
		var mainListChildren = mainList.childElements();
		var moreHandle;
		if (mainListChildren.length > 0) {
			moreHandle = mainListChildren[mainListChildren.length-1];
			if (!moreHandle.hasClassName('weebly-nav-more')) {
				moreHandle = null;
			}
		}
		var alreadyMore = false;
		if (moreHandle) {
			moreHandle.hide();
			alreadyMore = true;
		}
		var isVertical;
		var handles = []; // holds all the handles up til the breaking element
		var itemCoords = [];
		var breakingHandle;
		var breakingIndex;
		for (var i=0; i<topLevelSummary.length; i++) {
			var handle = navElm(topLevelSummary[i].id);
			if (alreadyMore) {
				handle.show();
			}
			var item = getRealTopLevelItem(handle);
			if (!item) continue;
			var coords = getItemMassCoords(item);
			if (i == 1) {
				isVertical = Math.abs(coords[0].top - itemCoords[0][0].top) > Math.abs(coords[0].left - itemCoords[0][0].left);
			}
			else if (i > 1 && !isVertical && Math.abs(coords[0].top - itemCoords[i-1][0].top) > 5) {
				breakingHandle = handle;
				breakingIndex = i;
				break;
			}
			handles.push(handle);
			itemCoords.push(coords);
		}
		if (breakingHandle) {
			if (moreHandle) {
				moreHandle.show();
			}else{
				var temp = $(document.createElement('div'));
				temp.innerHTML = moreItemHTML;
				moreHandle = temp.down();
				moreHandle.select('a').each(function(moreAnchor) {
					moreAnchor.onclick = function() { return false; };
					moreAnchor.style.position = 'relative'; // match what initItem does
					moreAnchor.id = 'weebly-nav-more-a';
				});
				mainList.insert(moreHandle);
			}
			var moreItem = getRealTopLevelItem(moreHandle);
			moreItem.style.position = 'relative'; // match what initItem does
			var hiddenItemIndices = [];
			for (var i=breakingIndex; i<topLevelSummary.length; i++) {
				navElm(topLevelSummary[i].id).hide();
				hiddenItemIndices.push(i);
			}
			for (var i=breakingIndex-1; i>=0; i--) {
				var moreCoords = getItemMassCoords(moreItem);
				if (Math.abs(moreCoords[0].top - itemCoords[i][0].top) > 5) {
					handles[i].hide();
					hiddenItemIndices.unshift(i);
				}else{
					break;
				}
			}
			if (hiddenItemIndices.length == 0) {
				// no items were hidden, no need for more...
				moreHandle.remove();
			}
			else if (hiddenItemIndices.length == topLevelSummary.length) {
				// all items were hidden, revert back
				for (var i=0; i<hiddenItemIndices.length; i++) {
					navElm(topLevelSummary[hiddenItemIndices[i]].id).show();
				}
				moreHandle.remove();
			}
			else {
				if (!alreadyMore) {
					var wrap = $(document.createElement('div'));
					wrap.addClassName('weebly-menu-wrap');
					var ul = $(document.createElement('ul'));
					ul.addClassName('weebly-menu');
					wrap.appendChild(ul);
					for (var j=0; j<hiddenItemIndices.length; j++) {
						var pageSummary = topLevelSummary[hiddenItemIndices[j]];
						var li = $(document.createElement('li'));
						li.id = 'weebly-nav-' + pageSummary.id;
						if (pageSummary.id == cpid) {
							li.addClassName('weebly-nav-current');
						}
						var a = $(document.createElement('a'));
						if (pageSummary.onclick) {
							a.href = '#';
							a.onclick = pageSummary.onclick;
						}else{
							a.href = '/' + pageSummary.url; // TODO: 'url' is misleading
						}
						li.appendChild(a);
						var submenu = getRealTopLevelItem(navElm(topLevelSummary[hiddenItemIndices[j]].id))._flyoutmenu_destroy();
						a.innerHTML =
							"<span class='weebly-menu-title'>" + pageSummary.title + "</span>" +
							(submenu ? "<span class='weebly-menu-more'>&gt;</span>" : '');
						if (submenu) {
							li.appendChild(submenu);
						}
						ul.appendChild(li);
					}
					moreItem.appendChild(wrap);
					navFlyoutMenu.addItem(moreItem);
					if (window.showEvent) {
						showEvent('navMore');
					}
				}
			}
		}
	}
	
	
	
	
	
	/************************ helpers for navigating and querying items/sublists/etc ********************/
	
	function inVerticalList(item, strict, aLiId) {
		var list = item.up();
		if (list.hasClassName('weebly-nav-handle')) {
			list = list.up();
		}
		var allItems = getTopLevelItems(list, strict, aLiId);
		if (allItems.length >= 2) {
			var o1 = allItems[0].positionedOffset();
			var o2 = allItems[1].positionedOffset();
			var diff = Math.abs(o1.left - o2.left) - Math.abs(o1.top - o2.top);
			if (diff != 0) {
				return diff < 0;
			}
		}
		return !isItemTopLevel(item);
			// default to returning false for top level user-defined css
			// and true for weebly-created submenus
	}
	
	function getTopLevelItems(list, strict, aLiId) {
		var res = [];
		list.childElements().each(function(handle) {
			if (!strict ||
				handle.hasClassName('weebly-nav-handle') ||
				handle.hasClassName('weebly-nav-more') ||
				handle.id.match(/^pg/) ||
				(aLiId && handle.id==aLiId)) {
					var item = getRealTopLevelItem(handle);
					if (item) {
						res.push(item);
					}
				}
		});
		return res;
	}
	
	function getRealTopLevelItem(item) { // todo: rename to getItemFromHandle()
		if (item.hasClassName('weebly-nav-handle')) {
			item = item.down();
		}
		if (item && !item.hasClassName('weebly-menu-wrap')) {
			// sometimes with SPAN handles, markup was invalid and DOM messed up
			// so make sure item is not a menu
			return item;
		}
	}
	
	function getAllItems(list, itemTag) {
		// get top level and all descendant items
		return list.select(itemTag).concat(getTopLevelItems(list)).uniq();
	}
	
	function getSiblings(item) {
		if (item.parentNode.hasClassName('weebly-nav-handle')) {
			var siblings = [];
			item.up().siblings().each(function(handle) {
				var sib = handle.down();
				if (sib) {
					siblings.push(sib);
				}
			});
			return siblings;
		}else{
			// items aren't wrapped by separate handles
			return item.siblings();
		}
	}
	
	function getSublist(item, listTag) {
		var sublist = item.down(listTag);
		if (!sublist) {
			var next = item.next();
			if (next && next.hasClassName('weebly-menu-wrap')) {
				// sometimes with SPAN handles, markup is invalid, and it
				// makes the sublist a sibling AFTER the item
				sublist = next.down();
			}
		}
		return sublist;
	}
	
	function isItemTopLevel(item) {
		var list = item.up();
		if (list.hasClassName('weebly-nav-handle')) {
			list = list.up();
		}
		return !list.hasClassName('weebly-menu');
	}
	
	function getItemMassCoords(item) {
		// look at the item and its A tag and return the largest rectangle of space it takes up
		var anchor = item.nodeName == 'A' ? item : $(item.getElementsByTagName('a')[0]);
		var p1 = safeCumulativeOffset(item);
		var p2 = { top:p1.top+item.getHeight(), left:p1.left+item.getWidth() };
		if (!anchor) {
			// messed up DOM (SPAN's around TD's and such) sometimes pushes A tag outside of item
			return [p1, p2];
		}
		var p3 = safeCumulativeOffset(anchor);
		var p4 = { top:p3.top+anchor.getHeight(), left:p3.left+anchor.getWidth() };
		var p5, p6;
		if (Math.abs(p1.left - p2.left) < 10) { // a tag is really small, doen't have any mass..
			// the inner A tag is probably floated and the LI isn't. lame. just use A tag's coords
			p5 = p3;
			p6 = p4;
		}else{
			p5 = { top:Math.min(p1.top, p3.top), left:Math.min(p1.left, p3.left) };
			p6 = { top:Math.max(p2.top, p4.top), left:Math.max(p2.left, p4.left) };
		}
		return [p5, p6];
	}
	
	function navElm(id) { // todo: rename to getHandle()
		var elm = $('pg'+id);
		if (elm) return elm;
		if (activeLiId) return $(activeLiId);
	}
	
	function safeCumulativeOffset(e) {
		if (e.getBoundingClientRect && e.nodeName != 'BODY' && e.nodeName != 'HTML') {
			// heavily inspired by jquery's offset method
			var rect = e.getBoundingClientRect(),
				body = document.body,
				docElem = document.documentElement,
				clientTop = docElem.clientTop || body.clientTop || 0,
				clientLeft = docElem.clientLeft || body.clientLeft || 0,
				scrollTop = window.pageYOffset || docElem.scrollTop || body.scrollTop,
				scrollLeft = window.pageXOffset || docElem.scrollLeft || body.scrollLeft,
				top = rect.top + scrollTop - clientTop,
				left = rect.left + scrollLeft - clientLeft,
				a = [left, top];
			a.left = left;
			a.top = top;
			return a;
		}else{
			return $(e).cumulativeOffset();
		}
	}
	
	
	
	
	/************************** helpers for theme-css-loaded detection ***********************/
	
	function isThemeCSSLoaded() {
		for (var i=0; i<document.styleSheets.length; i++) {
			try {
				if (document.styleSheets[i].title == 'weebly-theme-css') {
					var sheet = document.styleSheets[i];
					var rules = sheet.cssRules || sheet.rules;
					return rules && rules.length > 0;
				}
			}
			catch (err) {}
		}
		return false;
	}
	
	function whenThemeCSSLoaded(callback) {
		if (isThemeCSSLoaded()) {
			callback();
		}else{
			var iters = 0;
			var maxIters = 10;
			var intervalID = setInterval(function() {
				if (++iters > maxIters) {
					clearInterval(intervalID);
				}
				else if (isThemeCSSLoaded()) {
					clearInterval(intervalID);
					callback();
				}
			}, 200);
		}
	}

})();



/******************************* extra scriptaculous effects required for fly-out ****************************/
// http://scriptaculous.jakewendt.com/


Effect.SlideRightOut = function(element) {
/* 
	SlideRightOut need to have the content of the element wrapped in a container element with fixed width!
*/
	element = $(element).cleanWhitespace();
	var elementDimensions = element.getDimensions();
	return new Effect.Parallel ( [
		new Effect.Move(element, { x: element.getWidth(), sync: true, mode: 'relative' }),
		new Effect.Scale(element, window.opera ? 0 : 1, {	
			sync: true, 
			scaleContent: false, 
			scaleY: false,
			scaleFrom: 100,
			restoreAfterFinish: true
		})
		], Object.extend({ 
			beforeSetup: function(effect){
				effect.effects[0].element.makeClipping();
			},
			afterFinishInternal: function(effect){
				effect.effects[0].element.undoClipping().hide();
			}
		}, arguments[1] || {})
	);
}



/* from SlideUp */
Effect.SlideLeftOut = function(element) {
/*
	SlideLeftOut needs to have the content of the element wrapped in a container element with fixed width
	otherwise any text or images begin to wrap in stange ways!
*/
	element = $(element).cleanWhitespace();
	return new Effect.Scale(element, window.opera ? 0 : 1,
		Object.extend({ 
			scaleContent: false, 
			scaleY: false, 
			scaleMode: 'box',
			scaleFrom: 100,
			restoreAfterFinish: true,
			beforeStartInternal: function(effect) {
				effect.element.makePositioned();
				effect.element.down().makePositioned();
				if(window.opera) effect.element.setStyle({left: ''});
				effect.element.makeClipping().show();
			},  
			afterUpdateInternal: function(effect) {
				var down = effect.element.down();
				if (down) {
					// todo: add comment here
					down.setStyle(
						{right: (effect.dims[1] - effect.element.clientWidth) + 'px' }
					);
				}
			},
			afterFinishInternal: function(effect) {
				effect.element.hide().undoClipping().undoPositioned();
				var down = effect.element.down();
				if (down) {
					down.undoPositioned();
				}
			}
		}, arguments[1] || {})
	);
}


/* from SlideDown */
Effect.SlideRightIn = function(element) {
/*
	SlideRightIn needs to have the content of the element wrapped in a container element with fixed width!
*/
	element = $(element).cleanWhitespace();
	var elementDimensions = element.getDimensions();
	return new Effect.Scale(element, 100, 
		Object.extend({ 
			scaleContent: false, 
			scaleY: false, 
			scaleFrom: window.opera ? 0 : 1,
			scaleMode: {originalHeight: elementDimensions.height, originalWidth: elementDimensions.width},
			restoreAfterFinish: true,
			afterSetup: function(effect) {
				effect.element.makePositioned();
				effect.element.down().makePositioned();
				if(window.opera) effect.element.setStyle({left: ''});
				effect.element.makeClipping().setStyle({width: '0px'}).show(); 
			},
			afterUpdateInternal: function(effect) {
				effect.element.down().setStyle({right: (effect.dims[1] - effect.element.clientWidth) + 'px' }); 
			},
			afterFinishInternal: function(effect) {
				effect.element.undoClipping().undoPositioned();
				effect.element.down().undoPositioned();
			}
		}, arguments[1] || {})
	);
}



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

//
// CalendarView (for Prototype)
//
// Copyright 2007-2008 Singlesnet, Inc.
// Copyright 2002-2005 Mihai Bazon
//
// Maintained by Justin Mecham <justin@corp.singlesnet.com>
//
// This calendar is based very loosely on the Dynarch Calendar in that it was
// used as a base, but completely gutted and more or less rewritten in place
// to use the Prototype JavaScript library.
//
// As such, CalendarView is licensed under the terms of the GNU Lesser General
// Public License (LGPL). More information on the Dynarch Calendar can be
// found at:
//
//   www.dynarch.com/projects/calendar
//

var Calendar = Class.create()

//------------------------------------------------------------------------------
// Constants
//------------------------------------------------------------------------------

Calendar.VERSION = '1.1'

Calendar.DAY_NAMES = new Array(
  'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday',
  'Sunday'
)

Calendar.SHORT_DAY_NAMES = new Array(
  'S', 'M', 'T', 'W', 'T', 'F', 'S', 'S'
)

Calendar.MONTH_NAMES = new Array(
  'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August',
  'September', 'October', 'November', 'December'
)

Calendar.SHORT_MONTH_NAMES = new Array(
  'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov',
  'Dec' 
)

Calendar.NAV_PREVIOUS_YEAR  = -2
Calendar.NAV_PREVIOUS_MONTH = -1
Calendar.NAV_TODAY          =  0
Calendar.NAV_NEXT_MONTH     =  1
Calendar.NAV_NEXT_YEAR      =  2

//------------------------------------------------------------------------------
// Static Methods
//------------------------------------------------------------------------------

// This gets called when the user presses a mouse button anywhere in the
// document, if the calendar is shown. If the click was outside the open
// calendar this function closes it.
Calendar._checkCalendar = function(event) {
  if (!window._popupCalendar)
    return false
  if (Element.descendantOf(Event.element(event), window._popupCalendar.container))
    return
  window._popupCalendar.callCloseHandler()
  return Event.stop(event)
}

//------------------------------------------------------------------------------
// Event Handlers
//------------------------------------------------------------------------------

Calendar.handleMouseDownEvent = function(event)
{
  Event.observe(document, 'mouseup', Calendar.handleMouseUpEvent)
  Event.stop(event)
}

// XXX I am not happy with how clicks of different actions are handled. Need to
// clean this up!
Calendar.handleMouseUpEvent = function(event)
{
  var el        = Event.element(event)
  var calendar  = el.calendar
  var isNewDate = false

  // If the element that was clicked on does not have an associated Calendar
  // object, return as we have nothing to do.
  if (!calendar) return false

  // Clicked on a day
  if (typeof el.navAction == 'undefined')
  {
    if (calendar.currentDateElement) {
      Element.removeClassName(calendar.currentDateElement, 'selected')
      Element.addClassName(el, 'selected')
      calendar.shouldClose = (calendar.currentDateElement == el)
      if (!calendar.shouldClose) calendar.currentDateElement = el
    }
    calendar.date.setDateOnly(el.date)
    isNewDate = true
    calendar.shouldClose = !el.hasClassName('otherDay')
    var isOtherMonth     = !calendar.shouldClose
    if (isOtherMonth) calendar.update(calendar.date)
  }

  // Clicked on an action button
  else
  {
    var date = new Date(calendar.date)

    if (el.navAction == Calendar.NAV_TODAY)
      date.setDateOnly(new Date())

    var year = date.getFullYear()
    var mon = date.getMonth()
    function setMonth(m) {
      var day = date.getDate()
      var max = date.getMonthDays(m)
      if (day > max) date.setDate(max)
      date.setMonth(m)
    }
    switch (el.navAction) {

      // Previous Year
      case Calendar.NAV_PREVIOUS_YEAR:
        if (year > calendar.minYear)
          date.setFullYear(year - 1)
        break

      // Previous Month
      case Calendar.NAV_PREVIOUS_MONTH:
        if (mon > 0) {
          setMonth(mon - 1)
        }
        else if (year-- > calendar.minYear) {
          date.setFullYear(year)
          setMonth(11)
        }
        break

      // Today
      case Calendar.NAV_TODAY:
        break

      // Next Month
      case Calendar.NAV_NEXT_MONTH:
        if (mon < 11) {
          setMonth(mon + 1)
        }
        else if (year < calendar.maxYear) {
          date.setFullYear(year + 1)
          setMonth(0)
        }
        break

      // Next Year
      case Calendar.NAV_NEXT_YEAR:
        if (year < calendar.maxYear)
          date.setFullYear(year + 1)
        break

    }

    if (!date.equalsTo(calendar.date)) {
      calendar.setDate(date)
      isNewDate = true
    } else if (el.navAction == 0) {
      isNewDate = (calendar.shouldClose = true)
    }
  }

  if (isNewDate) event && calendar.callSelectHandler()
  if (calendar.shouldClose) event && calendar.callCloseHandler()

  Event.stopObserving(document, 'mouseup', Calendar.handleMouseUpEvent)

  return Event.stop(event)
}

Calendar.defaultSelectHandler = function(calendar)
{
  if (!calendar.dateField) return false

  // Update dateField value
  if (calendar.dateField.tagName == 'DIV')
    Element.update(calendar.dateField, calendar.date.print(calendar.dateFormat))
  else if (calendar.dateField.tagName == 'INPUT') {
    calendar.dateField.value = calendar.date.print(calendar.dateFormat) }

  // Trigger the onchange callback on the dateField, if one has been defined
  if (typeof calendar.dateField.onchange == 'function')
    calendar.dateField.onchange()

  // Call the close handler, if necessary
  if (calendar.shouldClose) calendar.callCloseHandler()
}

Calendar.defaultCloseHandler = function(calendar)
{
  calendar.hide()
}


//------------------------------------------------------------------------------
// Calendar Setup
//------------------------------------------------------------------------------

Calendar.setup = function(params)
{

  function param_default(name, def) {
    if (!params[name]) params[name] = def
  }

  param_default('dateField', null)
  param_default('triggerElement', null)
  param_default('parentElement', null)
  param_default('selectHandler',  null)
  param_default('closeHandler', null)

  // In-Page Calendar
  if (params.parentElement)
  {
    var calendar = new Calendar(params.parentElement)
    calendar.setSelectHandler(params.selectHandler || Calendar.defaultSelectHandler)
    if (params.dateFormat)
      calendar.setDateFormat(params.dateFormat)
    if (params.dateField) {
      calendar.setDateField(params.dateField)
      calendar.parseDate(calendar.dateField.innerHTML || calendar.dateField.value)
    }
    calendar.show()
    return calendar
  }

  // Popup Calendars
  //
  // XXX There is significant optimization to be had here by creating the
  // calendar and storing it on the page, but then you will have issues with
  // multiple calendars on the same page.
  else
  {
    var triggerElement = $(params.triggerElement || params.dateField)
    triggerElement.onclick = function() {
      var calendar = new Calendar()
      calendar.setSelectHandler(params.selectHandler || Calendar.defaultSelectHandler)
      calendar.setCloseHandler(params.closeHandler || Calendar.defaultCloseHandler)
      if (params.dateFormat)
        calendar.setDateFormat(params.dateFormat)
      if (params.dateField) {
        calendar.setDateField(params.dateField)
        calendar.parseDate(calendar.dateField.innerHTML || calendar.dateField.value)
      }
      if (params.dateField)
        Date.parseDate(calendar.dateField.value || calendar.dateField.innerHTML, calendar.dateFormat)
      calendar.showAtElement(triggerElement)
      return calendar
    }
  }

}



//------------------------------------------------------------------------------
// Calendar Instance
//------------------------------------------------------------------------------

Calendar.prototype = {

  // The HTML Container Element
  container: null,

  // Callbacks
  selectHandler: null,
  closeHandler: null,

  // Configuration
  minYear: 1990,
  maxYear: 2020,
  dateFormat: '%m/%d/%Y',

  // Dates
  date: new Date(),
  currentDateElement: null,

  // Status
  shouldClose: false,
  isPopup: true,

  dateField: null,


  //----------------------------------------------------------------------------
  // Initialize
  //----------------------------------------------------------------------------

  initialize: function(parent)
  {
    if (parent)
      this.create($(parent))
    else
      this.create()
  },



  //----------------------------------------------------------------------------
  // Update / (Re)initialize Calendar
  //----------------------------------------------------------------------------

  update: function(date)
  {
    var calendar   = this
    var today      = new Date()
    var thisYear   = today.getFullYear()
    var thisMonth  = today.getMonth()
    var thisDay    = today.getDate()
    var month      = date.getMonth();
    var dayOfMonth = date.getDate();

    // Ensure date is within the defined range
    if (date.getFullYear() < this.minYear)
      date.setFullYear(this.minYear)
    else if (date.getFullYear() > this.maxYear)
      date.setFullYear(this.maxYear)

    this.date = new Date(date)

    // Calculate the first day to display (including the previous month)
    date.setDate(1)
    date.setDate(-(date.getDay()) + 1)

    // Fill in the days of the month
    Element.getElementsBySelector(this.container, 'tbody tr').each(
      function(row, i) {
        var rowHasDays = false
        row.immediateDescendants().each(
          function(cell, j) {
            var day            = date.getDate()
            var dayOfWeek      = date.getDay()
            var isCurrentMonth = (date.getMonth() == month)

            // Reset classes on the cell
            cell.className = ''
            cell.date = new Date(date)
            cell.update(day)

            // Account for days of the month other than the current month
            if (!isCurrentMonth)
              cell.addClassName('otherDay')
            else
              rowHasDays = true

            // Ensure the current day is selected
            if (isCurrentMonth && day == dayOfMonth) {
              cell.addClassName('selected')
              calendar.currentDateElement = cell
            }

            // Today
            if (date.getFullYear() == thisYear && date.getMonth() == thisMonth && day == thisDay)
              cell.addClassName('today')

            // Weekend
            if ([0, 6].indexOf(dayOfWeek) != -1)
              cell.addClassName('weekend')

            // Set the date to tommorrow
            date.setDate(day + 1)
          }
        )
        // Hide the extra row if it contains only days from another month
        !rowHasDays ? row.hide() : row.show()
      }
    )

    this.container.getElementsBySelector('td.title')[0].update(
      Calendar.MONTH_NAMES[month] + ' ' + this.date.getFullYear()
    )
  },



  //----------------------------------------------------------------------------
  // Create/Draw the Calendar HTML Elements
  //----------------------------------------------------------------------------

  create: function(parent)
  {

    // If no parent was specified, assume that we are creating a popup calendar.
    if (!parent) {
      parent = document.getElementsByTagName('body')[0]
      this.isPopup = true
    } else {
      this.isPopup = false
    }

    // Calendar Table
    var table = new Element('table')

    // Calendar Header
    var thead = new Element('thead')
    table.appendChild(thead)

    // Title Placeholder
    var row  = new Element('tr')
    var cell = new Element('td', { colSpan: 7, className: 'title' })
    row.appendChild(cell)
    thead.appendChild(row)

    // Calendar Navigation
    row = new Element('tr')
    this._drawButtonCell(row, '&#x00ab;', 1, Calendar.NAV_PREVIOUS_YEAR)
    this._drawButtonCell(row, '&#x2039;', 1, Calendar.NAV_PREVIOUS_MONTH)
    this._drawButtonCell(row, 'Today',    3, Calendar.NAV_TODAY)
    this._drawButtonCell(row, '&#x203a;', 1, Calendar.NAV_NEXT_MONTH)
    this._drawButtonCell(row, '&#x00bb;', 1, Calendar.NAV_NEXT_YEAR)
    thead.appendChild(row)

    // Day Names
    row = new Element('tr')
    for (var i = 0; i < 7; ++i) {
      cell = new Element('th').update(Calendar.SHORT_DAY_NAMES[i])
      if (i == 0 || i == 6)
        cell.addClassName('weekend')
      row.appendChild(cell)
    }
    thead.appendChild(row)

    // Calendar Days
    var tbody = table.appendChild(new Element('tbody'))
    for (i = 6; i > 0; --i) {
      row = tbody.appendChild(new Element('tr', { className: 'days' }))
      for (var j = 7; j > 0; --j) {
        cell = row.appendChild(new Element('td'))
        cell.calendar = this
      }
    }

    // Calendar Container (div)
    this.container = new Element('div', { className: 'calendar' })
    if (this.isPopup) {
      this.container.setStyle({ position: 'absolute', display: 'none' })
      this.container.addClassName('popup')
    }
    this.container.appendChild(table)

    // Initialize Calendar
    this.update(this.date)

    // Observe the container for mousedown events
    Event.observe(this.container, 'mousedown', Calendar.handleMouseDownEvent)

    // Append to parent element
    parent.appendChild(this.container)

  },

  _drawButtonCell: function(parent, text, colSpan, navAction)
  {
    var cell          = new Element('td')
    if (colSpan > 1) cell.colSpan = colSpan
    cell.className    = 'button'
    cell.calendar     = this
    cell.navAction    = navAction
    cell.innerHTML    = text
    cell.unselectable = 'on' // IE
    parent.appendChild(cell)
    return cell
  },



  //------------------------------------------------------------------------------
  // Callbacks
  //------------------------------------------------------------------------------

  // Calls the Select Handler (if defined)
  callSelectHandler: function()
  {
    if (this.selectHandler)
      this.selectHandler(this, this.date.print(this.dateFormat))
  },

  // Calls the Close Handler (if defined)
  callCloseHandler: function()
  {
    if (this.closeHandler)
      this.closeHandler(this)
  },



  //------------------------------------------------------------------------------
  // Calendar Display Functions
  //------------------------------------------------------------------------------

  // Shows the Calendar
  show: function()
  {
    this.container.show()
    if (this.isPopup) {
      window._popupCalendar = this
      Event.observe(document, 'mousedown', Calendar._checkCalendar)
    }
  },

  // Shows the calendar at the given absolute position
  showAt: function (x, y)
  {
    this.container.setStyle({ left: x + 'px', top: y + 'px' })
    this.show()
  },

  // Shows the Calendar at the coordinates of the provided element
  showAtElement: function(element)
  {
    var pos = element.viewportOffset();//Drew 2009-04-06 Fixed position in scrolling containers
    this.showAt(pos[0], pos[1])
  },

  // Hides the Calendar
  hide: function()
  {
    if (this.isPopup)
      Event.stopObserving(document, 'mousedown', Calendar._checkCalendar)
    this.container.parentNode.removeChild(this.container);
    //this.container.hide()
  },



  //------------------------------------------------------------------------------
  // Miscellaneous
  //------------------------------------------------------------------------------

  // Tries to identify the date represented in a string.  If successful it also
  // calls this.setDate which moves the calendar to the given date.
  parseDate: function(str, format)
  {
    if (!format)
      format = this.dateFormat
    this.setDate(Date.parseDate(str, format))
  },



  //------------------------------------------------------------------------------
  // Getters/Setters
  //------------------------------------------------------------------------------

  setSelectHandler: function(selectHandler)
  {
    this.selectHandler = selectHandler
  },

  setCloseHandler: function(closeHandler)
  {
    this.closeHandler = closeHandler
  },

  setDate: function(date)
  {
    if (!date.equalsTo(this.date))
      this.update(date)
  },

  setDateFormat: function(format)
  {
    this.dateFormat = format
  },

  setDateField: function(field)
  {
    this.dateField = $(field)
  },

  setRange: function(minYear, maxYear)
  {
    this.minYear = minYear
    this.maxYear = maxYear
  }

}

// global object that remembers the calendar
window._popupCalendar = null





























//==============================================================================
//
// Date Object Patches
//
// This is pretty much untouched from the original. I really would like to get
// rid of these patches if at all possible and find a cleaner way of
// accomplishing the same things. It's a shame Prototype doesn't extend Date at
// all.
//
//==============================================================================

Date.DAYS_IN_MONTH = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31)
Date.SECOND        = 1000 /* milliseconds */
Date.MINUTE        = 60 * Date.SECOND
Date.HOUR          = 60 * Date.MINUTE
Date.DAY           = 24 * Date.HOUR
Date.WEEK          =  7 * Date.DAY

// Parses Date
Date.parseDate = function(str, fmt) {
  var today = new Date();
  var y     = 0;
  var m     = -1;
  var d     = 0;
  var a     = str.split(/\W+/);
  var b     = fmt.match(/%./g);
  var i     = 0, j = 0;
  var hr    = 0;
  var min   = 0;

  for (i = 0; i < a.length; ++i) {
    if (!a[i]) continue;
    switch (b[i]) {
      case "%d":
      case "%e":
        d = parseInt(a[i], 10);
        break;
      case "%m":
        m = parseInt(a[i], 10) - 1;
        break;
      case "%Y":
      case "%y":
        y = parseInt(a[i], 10);
        (y < 100) && (y += (y > 29) ? 1900 : 2000);
        break;
      case "%b":
      case "%B":
        for (j = 0; j < 12; ++j) {
          if (Calendar.MONTH_NAMES[j].substr(0, a[i].length).toLowerCase() == a[i].toLowerCase()) {
            m = j;
            break;
          }
        }
        break;
      case "%H":
      case "%I":
      case "%k":
      case "%l":
        hr = parseInt(a[i], 10);
        break;
      case "%P":
      case "%p":
        if (/pm/i.test(a[i]) && hr < 12)
          hr += 12;
        else if (/am/i.test(a[i]) && hr >= 12)
          hr -= 12;
        break;
      case "%M":
        min = parseInt(a[i], 10);
        break;
    }
  }
  if (isNaN(y)) y = today.getFullYear();
  if (isNaN(m)) m = today.getMonth();
  if (isNaN(d)) d = today.getDate();
  if (isNaN(hr)) hr = today.getHours();
  if (isNaN(min)) min = today.getMinutes();
  if (y != 0 && m != -1 && d != 0)
    return new Date(y, m, d, hr, min, 0);
  y = 0; m = -1; d = 0;
  for (i = 0; i < a.length; ++i) {
    if (a[i].search(/[a-zA-Z]+/) != -1) {
      var t = -1;
      for (j = 0; j < 12; ++j) {
        if (Calendar.MONTH_NAMES[j].substr(0, a[i].length).toLowerCase() == a[i].toLowerCase()) { t = j; break; }
      }
      if (t != -1) {
        if (m != -1) {
          d = m+1;
        }
        m = t;
      }
    } else if (parseInt(a[i], 10) <= 12 && m == -1) {
      m = a[i]-1;
    } else if (parseInt(a[i], 10) > 31 && y == 0) {
      y = parseInt(a[i], 10);
      (y < 100) && (y += (y > 29) ? 1900 : 2000);
    } else if (d == 0) {
      d = a[i];
    }
  }
  if (y == 0)
    y = today.getFullYear();
  if (m != -1 && d != 0)
    return new Date(y, m, d, hr, min, 0);
  return today;
};

// Returns the number of days in the current month
Date.prototype.getMonthDays = function(month) {
  var year = this.getFullYear()
  if (typeof month == "undefined")
    month = this.getMonth()
  if (((0 == (year % 4)) && ( (0 != (year % 100)) || (0 == (year % 400)))) && month == 1)
    return 29
  else
    return Date.DAYS_IN_MONTH[month]
};

// Returns the number of day in the year
Date.prototype.getDayOfYear = function() {
  var now = new Date(this.getFullYear(), this.getMonth(), this.getDate(), 0, 0, 0);
  var then = new Date(this.getFullYear(), 0, 0, 0, 0, 0);
  var time = now - then;
  return Math.floor(time / Date.DAY);
};

/** Returns the number of the week in year, as defined in ISO 8601. */
Date.prototype.getWeekNumber = function() {
  var d = new Date(this.getFullYear(), this.getMonth(), this.getDate(), 0, 0, 0);
  var DoW = d.getDay();
  d.setDate(d.getDate() - (DoW + 6) % 7 + 3); // Nearest Thu
  var ms = d.valueOf(); // GMT
  d.setMonth(0);
  d.setDate(4); // Thu in Week 1
  return Math.round((ms - d.valueOf()) / (7 * 864e5)) + 1;
};

/** Checks date and time equality */
Date.prototype.equalsTo = function(date) {
  return ((this.getFullYear() == date.getFullYear()) &&
   (this.getMonth() == date.getMonth()) &&
   (this.getDate() == date.getDate()) &&
   (this.getHours() == date.getHours()) &&
   (this.getMinutes() == date.getMinutes()));
};

/** Set only the year, month, date parts (keep existing time) */
Date.prototype.setDateOnly = function(date) {
  var tmp = new Date(date);
  this.setDate(1);
  this.setFullYear(tmp.getFullYear());
  this.setMonth(tmp.getMonth());
  this.setDate(tmp.getDate());
};

/** Prints the date in a string according to the given format. */
Date.prototype.print = function (str) {
  var m = this.getMonth();
  var d = this.getDate();
  var y = this.getFullYear();
  var wn = this.getWeekNumber();
  var w = this.getDay();
  var s = {};
  var hr = this.getHours();
  var pm = (hr >= 12);
  var ir = (pm) ? (hr - 12) : hr;
  var dy = this.getDayOfYear();
  if (ir == 0)
    ir = 12;
  var min = this.getMinutes();
  var sec = this.getSeconds();
  s["%a"] = Calendar.SHORT_DAY_NAMES[w]; // abbreviated weekday name [FIXME: I18N]
  s["%A"] = Calendar.DAY_NAMES[w]; // full weekday name
  s["%b"] = Calendar.SHORT_MONTH_NAMES[m]; // abbreviated month name [FIXME: I18N]
  s["%B"] = Calendar.MONTH_NAMES[m]; // full month name
  // FIXME: %c : preferred date and time representation for the current locale
  s["%C"] = 1 + Math.floor(y / 100); // the century number
  s["%d"] = (d < 10) ? ("0" + d) : d; // the day of the month (range 01 to 31)
  s["%e"] = d; // the day of the month (range 1 to 31)
  // FIXME: %D : american date style: %m/%d/%y
  // FIXME: %E, %F, %G, %g, %h (man strftime)
  s["%H"] = (hr < 10) ? ("0" + hr) : hr; // hour, range 00 to 23 (24h format)
  s["%I"] = (ir < 10) ? ("0" + ir) : ir; // hour, range 01 to 12 (12h format)
  s["%j"] = (dy < 100) ? ((dy < 10) ? ("00" + dy) : ("0" + dy)) : dy; // day of the year (range 001 to 366)
  s["%k"] = hr;   // hour, range 0 to 23 (24h format)
  s["%l"] = ir;   // hour, range 1 to 12 (12h format)
  s["%m"] = (m < 9) ? ("0" + (1+m)) : (1+m); // month, range 01 to 12
  s["%M"] = (min < 10) ? ("0" + min) : min; // minute, range 00 to 59
  s["%n"] = "\n";   // a newline character
  s["%p"] = pm ? "PM" : "AM";
  s["%P"] = pm ? "pm" : "am";
  // FIXME: %r : the time in am/pm notation %I:%M:%S %p
  // FIXME: %R : the time in 24-hour notation %H:%M
  s["%s"] = Math.floor(this.getTime() / 1000);
  s["%S"] = (sec < 10) ? ("0" + sec) : sec; // seconds, range 00 to 59
  s["%t"] = "\t";   // a tab character
  // FIXME: %T : the time in 24-hour notation (%H:%M:%S)
  s["%U"] = s["%W"] = s["%V"] = (wn < 10) ? ("0" + wn) : wn;
  s["%u"] = w + 1;  // the day of the week (range 1 to 7, 1 = MON)
  s["%w"] = w;    // the day of the week (range 0 to 6, 0 = SUN)
  // FIXME: %x : preferred date representation for the current locale without the time
  // FIXME: %X : preferred time representation for the current locale without the date
  s["%y"] = ('' + y).substr(2, 2); // year without the century (range 00 to 99)
  s["%Y"] = y;    // year with the century
  s["%%"] = "%";    // a literal '%' character

  return str.gsub(/%./, function(match) { return s[match] || match });
};

Date.prototype.__msh_oldSetFullYear = Date.prototype.setFullYear;
Date.prototype.setFullYear = function(y) {
  var d = new Date(this);
  d.__msh_oldSetFullYear(y);
  if (d.getMonth() != this.getMonth())
    this.setDate(28);
  this.__msh_oldSetFullYear(y);
}

  function onHideLightbox(type) {

        if ($$('#weeblyLightboxContent div#adsense_terms')[0] || type == 'adsense') {

          $$('#secondlist li iframe.adsenseIframe').each(function(el) {
            if (el && el.parentNode && el.parentNode.parentNode && el.parentNode.parentNode.parentNode && el.parentNode.parentNode.parentNode.parentNode && el.parentNode.parentNode.parentNode.parentNode.parentNode) {
              if (navigator.appVersion.indexOf("MSIE") == -1) {
                var thisLi = el.parentNode.parentNode.parentNode.parentNode.parentNode;
              } else {
                var thisLi = el.parentNode.parentNode.parentNode.parentNode;
              }
              thisLi.parentNode.removeChild(thisLi);
              new Ajax.Request(ajax, {parameters:'pos=deletepageelement&pei='+thisLi.id.replace('inside_', '')+'&cookie='+document.cookie, onSuccess:handlerFuncDeletePageElement, onFailure:errFunc, onException: exceptionFunc, asynchronous: false});
            }
          });

          updateList();

        }

  }

  function submitAdsense() {

        if ($('adsense_spinner').style.display == "inline") return;

        $('adsenseErrorDiv').style.display = 'none';
        $('adsenseAcceptTOS').style.display = 'block';
        $('adsenseAcceptTOS').innerHTML = '';
        $('adsense_submitbtn').style.opacity = 0.5;
        $('adsense_submitbtn').style.filter = 'alpha(opacity=50)';
        $('adsense_spinner').style.display = 'inline';
        new Ajax.Request(ajax, {parameters: 'pos=createadsenseaccount&'+Form.serialize('adsense_form')+'&cookie='+document.cookie, 'onSuccess': handlerSubmitAdsense, 'onFailure':errFunc, bgRequest: true});
  }

  function handlerSubmitAdsense(t) {

        if (t.responseText.match("SUCCESS")) {

          adsenseID = t.responseText.replace("SUCCESS:", "");
          Weebly.lightbox.show({element: '#adsense_finish', width: 475, height: 260, padding: 20, animate: true});

          $('adsenseErrorDiv').style.display = 'none';
          $('adsenseAcceptTOS').style.display = 'block';
	  $('adsenseAcceptTOS').innerHTML = /*tl(*/"By continuing, I accept Google's AdSense Terms and Conditions and Weebly's Advertising Terms and Conditions."/*)tl*/;
          $('adsense_submitbtn').style.opacity = 1;
          $('adsense_submitbtn').style.filter = 'alpha(opacity=100)';
          $('adsense_spinner').style.display = 'none';

        } else {
          if (!t.responseText.match("ERROR")) { t.responseText = /*tl(*/"ERROR: Temporary error. Please try again."/*)tl*/; }
          $('adsenseErrorDiv').innerHTML = t.responseText;
          $('adsenseAcceptTOS').style.display = 'none';
          $('adsenseErrorDiv').style.display = 'block';
          $('adsense_submitbtn').style.opacity = 1;
          $('adsense_submitbtn').style.filter = 'alpha(opacity=100)';
          $('adsense_spinner').style.display = 'none';
        }

  }


Weebly.Form = {
    drawRadioOptions : function(options){
        if(typeof(options) === 'string'){
            options = options.split('||');
        }
        var radio = '';
        var name = 'radio-'+Math.floor(Math.random()*999);
        options.each(function(option){
            radio += '<span class="form-radio-container"><input type="radio" name="'+name+'" /><label>'+option+'</label></span>';
        });
        return radio;
    },

    drawSelectOptions : function(options){
        if(typeof(options) === 'string'){
            options = options.split('||');
        }
        var select = '<select>';
        options.each(function(option){
            select += '<option>'+option+'</option>';
        });
        select += '</select>';
        return select;
    },

    drawCheckboxes : function(options){
        if(typeof(options) === 'string'){
            options = options.split('||');
        }
        var radio = '';
        options.each(function(option){
            radio += '<span class="form-radio-container"><input type="checkbox" /><label>'+option+'</label></span>';
        });
        return radio;
    },

    isOverInputWarningLimit : function(){
        if(Weebly.Restrictions.hasAccess('unlimited_form_inputs')){
            return false;
        }

        var newElements = $$('#secondlist .outside_top');
        if(newElements.size() == 1){
            var newEl = newElements[0];
            var form = newEl.up('.formlist')
            if(form){
                var totalInputs  = form.select('.weebly-form-field').size();
                return totalInputs >= (Weebly.Restrictions.accessValue('free_form_inputs') - 1);
            }
        }
        return false;
    },

    isOverInputLimit : function(){
        if(Weebly.Restrictions.hasAccess('unlimited_form_inputs')){
            return false;
        }

        var newElements = $$('#secondlist .outside_top');
        if(newElements.size() == 1){
            var newEl = newElements[0];
            var form = newEl.up('.formlist')
            if(form){
                var totalInputs  = form.select('.weebly-form-field').size();
                return totalInputs >= (Weebly.Restrictions.accessValue('free_form_inputs'));
            }
        }
        return false;
    },
    
    isNewFormElement : function(){
        var newElements = $$('#secondlist .outside_top');
        if (newElements.size() == 1) {
            if(newElements[0].down('input')){
                var def = newElements[0].down('input').value;
                var id = def.replace(/[^\d]/g, '');
                return Weebly.Form.elements.member(id);
            }
        }
        return false;
    },

    showFieldInstructions : function( msg, pointTo ){
        var image = false;
        var el = new Element( 'div', { 'class':'instructions-container', 'id':pointTo.id+'-instructions' } ).update( msg );
        currentVisibleError = el.identify();
        el.observe( 'click', function(e){ el.hide().remove() } );
        $('scroll_container').insert( {'bottom':el} );
        var dimensions = el.getDimensions();

        var target = $(pointTo);
        var offset = target.cumulativeOffset();
        var targetDimensions = target.getDimensions();
        var top = (offset.top + targetDimensions.height/2 - dimensions.height/2) - 133 + 'px';
        var left = ( offset.left + targetDimensions.width + 20 ) + 'px';

        el.setStyle( {top: top, left: left} );
        //set arrow position
        var imagetop  = Math.floor( dimensions.height / 2 ) - 10;
        var imageleft = '-13';
        el.insert( {'bottom':'<img src="http://www.weebly.com/images/error_arrow_left.gif" style="position: absolute; left:'+imageleft+'px; top: '+imagetop+'px;" />'} );
    },

    removeFieldInstructions : function(event){
        var el = Event.element(event);
        if(!el.up('.weebly-form-field')){
            document.stopObserving('mousemove', Weebly.Form.removeFieldInstructions);
            $$('.instructions-container').invoke('remove');
        }
    },

    fieldInstructionsHandler : function(){
        $$('.weebly-form-instructions').each(function(el){Weebly.Form.setupFieldInstructions(el);});
    },

    setupFieldInstructions : function(el){
        var pointTo = $(el.id.replace('instructions', 'input'));
        //select inputs
        if(!pointTo){
            pointTo = el.up('.weebly-form-field').down('.form-select');
        }
        //radio/checkbox inputs
        if(!pointTo){
            pointTo = el.up('.weebly-form-field').down('.weebly-form-label');
        }
        var container = pointTo.up('.weebly-form-field');
        if(pointTo.up('.weebly-form-input-container') && pointTo.up('.weebly-form-input-container').hasClassName('weebly-form-left')){
            pointTo = pointTo.up('.weebly-form-input-container').next('.weebly-form-right');
        }
        container.stopObserving('mouseover');
        container.observe('mouseover', function(event){
            if(this.hasClassName('weebly-form-field')){
                if(!$(pointTo.id+'-instructions') && !el.innerHTML.empty()){
                    Weebly.Form.showFieldInstructions(el.innerHTML, pointTo);
                }
                document.observe('mousemove', Weebly.Form.removeFieldInstructions);
            }
        });
    },
    
    commonFieldOptions : {
        'Gender' : ['Male', 'Female'],
        'Age' : ['Less than 13','13-18','19-25','26-35','36-50','Over 50','Prefer not to say'],
        'Income' : ['Less than $10,000','$10,001-$25,000','$25,001-$40,000','$70,001-$100,000','> $100,000', 'Prefer not to say'],
        'Marketing Reference' : ['Internet Search','Advertisment','Friend','Other'],
        'Answer' : ['Yes','No','Maybe'],
        'Education' : ['Some High School','Completed High School','Some College','Associate\'s Degree','Bachelor\'s Degree','Master\'s Degree','PhD'],
        'Employment' : ['Unemployed','Part-time','Full-time','Self-employed'],
        'Days of the Week' : ['Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday'],
        'Months of the Year' : ['January','February','March','April','May','June','July','August','September','October','November','December'],
        'U.S. States' : ['Alabama','Alaska','Arizona','Arkansas','California','Colorado','Connecticut','Delaware','Florida','Georgia','Hawaii','Idaho','Illinois','Indiana','Iowa','Kansas','Kentucky','Louisiana','Maine','Maryland','Massachusetts','Michigan','Minnesota','Mississippi','Missouri','Montana','Nebraska','Nevada','New Hampshire','New Jersey','New Mexico','New York','North Carolina','North Dakota','Ohio','Oklahoma','Oregon','Pennsylvania','Rhode Island','South Carolina','South Dakota','Tennessee','Texas','Utah','Vermont','Virginia','Washington','West Virginia','Wisconsin','Wyoming'],
        'How Often' : ['Everyday','Once per week','2 to 3 times per week','Once per month','Less than once per month'],
        'How Long' : ['Less than 1 month','1-6 months','1-3 years','Over 3 years','Never'],
        'Satisfaction' : ['Very Satisfied','Satisfied','Neutral','Unsatisfied','Very Unsatisfied'],
        'Importance' : ['Very Important','Important','Neutral','Somewhat Important','Not at all Important'],
        'Agreement' : ['Strongly Agree','Agree','Neutral','Disagree','Strongly Disagree'],
        'Comparison' : ['Much Better','Somewhat Better','About the Same','Somewhat Worse','Much Worse']
    }  

}

var domainChoiceSelected;
var domainNextStep;
function domainChoiceSelect(selection) {
    var options = Array('domainSubdomain', 'domainNewDomain', 'domainExistingDomain');
    if (selection == domainChoiceSelected) { return; }
    var e;

    for (var x=0; x<options.length; x++) { 
        e = $(options[x]);
        if (e) {
            if (options[x] == selection) {
                $(options[x]).className = 'domainChoiceSelected';
            } else {
                $(options[x]).className = 'domainChoice';
            }
            if (options[x] == selection || $(options[x]+'-2').style.display == 'none') { options.splice(x, 1); x--; }
        }
    }

    if (e = $(selection+'-2')) {
        e.style.display = 'block';
    }
    if (e = $(options[0]+'-2')) {
        e.style.display = 'none';
    }
    if (options[1]) {
        if (e = $(options[1]+'-2')) {
            e.style.display = 'none';
        }
    }

    domainChoiceSelected = selection;
}

function domainChoiceReset() {
    var options = Array('domainSubdomain', 'domainNewDomain', 'domainExistingDomain'),
        e;
    for (var x=0; x<options.length; x++) { 
        if (e = $(options[x])) {
            e.className = 'domainChoice';
        }
    }
    if (e = $('domainSubdomain-2')) {
        e.style.display = 'block';
    }
    if (e = $('domainNewDomain-2')) {
        e.style.display = 'block';
        $('domain_sld').value = '';
        $('domainStatus').innerHTML = '';
    }
    if (e = $('domainExistingDomain-2')) {
        e.style.display = 'none';
        $('weeblyExistingDomain').value = '';
    }
    domainChoiceSelected = '';

    $('chooseDomainDiv').style.display = 'block';
    $('domainCcInfo').style.display = 'none'; 
    $('domainWrapper').style.display = "block";
    $('domainRegisterComplete').style.display = 'none'; 
    $('domainExistingException').style.display = 'none'; 
    $('weeblyDomain').value = '';
    $('domainChoiceError').innerHTML = '';
    $('domainChoiceError').style.display = 'none';
    $('weeblyDomainStatus').innerHTML = '';
    $('chooseDomain').style.width = "566px";
    $('chooseAddress').src = 'http://'+editorStatic+'/weebly/images/choose-address.jpg';
}

function domainChoiceContinue() {
    $('domainChoiceError').display = 'none';

    if (domainChoiceSelected == 'domainSubdomain') {
        new Ajax.Request(ajax, {parameters:'pos=savedomain&type=subdomain&domain='+$('weeblyDomain').value+'&cookie='+document.cookie, onSuccess:handlerSaveSubdomain, onFailure:errFunc});
    } else if (domainChoiceSelected == 'domainNewDomain') {
        if ($('domainStatus').innerHTML.match(/Not Available/)) {
            $('domainChoiceError').innerHTML = "Please select an available domain.";
            $('domainChoiceError').style.display = 'block';
        } else {
            if (popUpBilling) {
                var return_page = typeof(purchase_return_page) == 'string' ? purchase_return_page : '';
                window.open('https://'+secureBase+'/weebly/apps/purchasePage.php?type=domain&s='+configSiteName+'&domain='+$('finalDomainName').innerHTML+'&sessionid='+sid+'&page='+return_page, 'weebly_billingPage', 'height=670,width=910,menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
            } else {
                // This is where we plug in the iframe !!
                $('chooseDomain').style.width = "866px";
                $('chooseAddress').src = 'http://'+editorStatic+'/weebly/images/register-domain.jpg';
                $('chooseDomainDiv').style.display = 'none';
                
                var domainCcInfo = $('domainCcInfo');
                var domainCcInfoParent = domainCcInfo.parentNode;
                domainCcInfoParent.removeChild(domainCcInfo);
                domainCcInfo.style.display = 'block';
                domainCcInfo.style.height = '615px';
                domainCcInfo.src = 'https://'+secureBase+'/weebly/apps/purchasePage.php?type=domain&s='+configSiteName+'&domain='+$('finalDomainName').innerHTML+'&sessionid='+sid;
                domainCcInfoParent.appendChild(domainCcInfo);
                
                $('purchaseX').style.display = "block";
                $('domainWrapper').style.display = "none";
            }
            purchaseReferer = "domainMenu";
            showEvent('customDomainS2');
        }
    } else if (domainChoiceSelected == 'domainExistingDomain') {
        new Ajax.Request(ajax, {parameters:'pos=savedomain&continue_not_configured=0&type=domain&domain='+$('weeblyExistingDomain').value+'&cookie='+document.cookie, onSuccess:handlerSaveExistingDomain, onFailure:errFunc});
    }
}

function handlerSaveExistingDomain(t) {
    if (t.responseText.indexOf('the following problems') > 0) {
        $('domainChoiceError').innerHTML = t.responseText;
        $('domainChoiceError').style.display = "block";
    } else if (t.responseText.indexOf('%%notconfigured%%') > -1) {
        t.responseText = t.responseText.replace(/%%notconfigured%%/, "");
        var responseElements = t.responseText.split("|");
        $('finalExistingDomainName').innerHTML = $('weeblyExistingDomain').value;
        $('domainExistingInfo').innerHTML = "<a href='http://"+responseElements[1]+".weebly.com/' target='_blank'>http://"+responseElements[1]+".weebly.com</a>";
        $('domainExistingException').style.display = 'block';
        $('chooseDomainDiv').style.display = 'none';
        // allow publish even if domain is thought to be incorrectly configured
        domainNextStep = domainNextStep ? domainNextStep : "displaySiteSettings";
        settingQuickExport = 1;
    } else {
        settingQuickExport = 1;
        domainChoiceFinish();
    }
}

function domainExistingExceptionContinue() {
    new Ajax.Request(ajax, {
        parameters:'pos=savedomain&continue_not_configured=1&type=domain&domain='+$('weeblyExistingDomain').value+'&cookie='+document.cookie,
        onSuccess: function() {
            settingQuickExport = 1;
            domainChoiceFinish();
        },
        onFailure:errFunc
    });
    return false;
}

function handlerSaveSubdomain(t) {
    if (t.responseText.indexOf('the following problems') > 0) {
        $('domainChoiceError').innerHTML = t.responseText;
        $('domainChoiceError').style.display = "block";
    } else {
        settingQuickExport = 1;
    domainChoiceFinish();
    }
}

function openCompetitorWindow() {
    if ($('competitorTotal').innerHTML == "$217.82") {
        window.open('http://'+editorStatic+'/weebly/images/godaddy1year.png', 'godaddy1', 'toolbar=0,status=0,width=746,height=549');
    } else {
        window.open('http://'+editorStatic+'/weebly/images/godaddy2years.png', 'godaddy2', 'toolbar=0,status=0,width=820,height=562');
    }
}

      function completeDomainPurchase(order_id) {

    $('finalDomainNameConfirmation').innerHTML = order_id;
    $('finalDomainNameComplete').innerHTML = $('finalDomainName').innerHTML;
    $('domainRegisterComplete').style.display = "block";
    $('domainCcInfo').style.display = "none";
    $('domainWrapper').style.display = "block";
    $('chooseDomain').style.width = "566px";

    showEvent('customDomainS3');
    
      }

function continueFromPublish() {
    // make sure settings page doesnt show up after domain purchase
    domainNextStep = "main";

    if($('domain_sld').value != "") {
    // verify domain available, continue to purchase domain
    if(!$('domainStatus').innerHTML.match(/Not Available/) && !$('domainStatus').innerHTML.match(/Checking/) && $('domainStatus').innerHTML !="") {
        // continue
        var domainName = filterDomain($('domain_sld').value);
        $('finalDomainName').innerHTML=domainName+"."+$('domain_tld').value;
        
        if (popUpBilling) {
            window.open('https://'+secureBase+'/weebly/apps/purchasePage.php?type=domain&s='+configSiteName+'&domain='+$('finalDomainName').innerHTML+'&sessionid='+sid, 'weebly_billingPage', 'height=670,width=910,menubar=yes,toolbar=yes,location=yes,status=yes,resizable=yes,scrollbars=yes,dependent=yes');
        } else {
            // This is where we plug in the iframe !!
            $('chooseDomain').style.width = "866px";
            $('chooseAddress').src = 'http://'+editorStatic+'/weebly/images/register-domain.jpg';
            $('chooseDomainDiv').style.display = 'none';
            
            var domainCcInfo = $('domainCcInfo');
            var domainCcInfoParent = domainCcInfo.parentNode;
            domainCcInfoParent.removeChild(domainCcInfo);
            domainCcInfo.style.display = 'block';
            domainCcInfo.style.height = '615px';
            domainCcInfo.src = 'https://'+secureBase+'/weebly/apps/purchasePage.php?type=domain&s='+configSiteName+'&domain='+$('finalDomainName').innerHTML+'&sessionid='+sid;
            domainCcInfoParent.appendChild(domainCcInfo);
            
            $('purchaseX').style.display = "block";
            $('domainWrapper').style.display = "none";

            // set close action of form
            $('chooseDomainClose').onclick = function(){Pages.go('main',1);};

            Pages.go('domainMenuPurchase',$('domain_sld').value);

        }

        purchaseReferer = "domainMenu";
        showEvent('customDomainS2');

        //console.log('Domain available, proceed to order');
        }else {
        // print error
        //console.log('Domain not available');
        }
    } else{  
        // go back to main screen
        //Element.hide('tip14');
        $('domainStatus').innerHTML="Please choose a Domain";
    }
}
      
function domainChoiceFinish(){
    if (domainNextStep == "publish") {
        Pages.go('doExport');
    } else if (domainNextStep == "main") {
        Pages.go('main');
    } else if (domainNextStep == "editor") {
        window.location = 'main.php';
    } else {
        Pages.go('displaySiteSettings', 1);
    }
}

var isUserTyping = 0;
function onTextChange(field_id, lastValue) {
    // fix hanging issue after field is cleared
    if ($(field_id).value == "") { 
        isUserTyping = 0; 
        $('domainStatus').innerHTML="";
        return;
    }
    if (!lastValue && isUserTyping != 0) { return; }
        
    if (!lastValue) {
        // First iteration
        isUserTyping = field_id;
        setTimeout("onTextChange('"+field_id+"', $('"+field_id+"').value)", 700);
    } else if($(field_id) && $(field_id).value && $(field_id).value == lastValue) {
        // User has stopped typing
        isUserTyping = 0;
    
        // Call your function here!!
        if(field_id == "domain_sld") {
            checkDomain();
        }else{
            checkWeeblyDomain();
        }
    } else {
      // User is still typing
      setTimeout("onTextChange('"+field_id+"', $('"+field_id+"').value)", 700);
    }
}



function checkWeeblyDomain() {
    var weeblySubDomain = $('weeblyDomain').value;
    var regx = new Array();
    
    if(weeblySubDomain != "") {
        regx.push(new RegExp('^.*:\\/\\/', ''));
        regx.push(new RegExp('^.*@', ''));
        regx.push(new RegExp('^\\-+', ''));
        regx.push(new RegExp('\\-+$', ''));
        regx.push(new RegExp('[^a-zA-Z0-9\\-]', 'g'));  
    
        var origText = weeblySubDomain;
        for(var x=0; x<regx.length; x++) {
            weeblySubDomain = weeblySubDomain.replace(regx[x],'');
        }
        weeblySubDomain = weeblySubDomain.substr(0,32);
        if (origText != weeblySubDomain) {
            $('weeblyDomain').value = weeblySubDomain;
        }
        
        $('weeblyDomainStatus').innerHTML="<a style=\"color: grey;\">Checking ..</a>";
        new Ajax.Request(ajax, {parameters:'pos=checkWeeblyDomain&weeblySubDomain='+weeblySubDomain+'&cookie='+document.cookie, onSuccess:handlerCheckWeeblyDomain, onFailure:errFunc, bgRequest: true, onException:exceptionFunc});
    }
}

function handlerCheckWeeblyDomain(x) {
    if(x.responseText.match(/%%SUCCESS%%/)){
        $('weeblyDomainStatus').innerHTML="<a style=\"color: green;\">"+/*tl(*/"Available"/*)tl*/+"</a>";
    }else{
        $('weeblyDomainStatus').innerHTML="<a style=\"color: red;\">"+/*tl(*/"Not Available"/*)tl*/+"</a>";
    }
}

function checkDomain(){
    domainName = $('domain_sld').value;
    domainNameTld = $('domain_tld').value;
    
    if(domainName != ""){
       var origText = domainName;
       domainName = filterDomain(domainName);
       if (origText != domainName) {
            $('domain_sld').value = domainName;
       }
       
       $('domainStatus').innerHTML="<a style=\"color: grey;\">"+/*tl(*/"Checking .."/*)tl*/+"</a>"; 
       
       // Hide
       //$('purchaseDomainName2').style.display='block'; 
       $('finalDomainName').innerHTML=domainName+"."+domainNameTld;
       
       // ajax call to check domain
       new Ajax.Request(ajax, {parameters:'pos=checkdomain&domainname='+domainName+'&domaintld='+domainNameTld+'&cookie='+document.cookie, onSuccess:handlerCheckDomain, onFailure:errFunc, bgRequest: true, onException:exceptionFunc});
    }
}

function filterDomain(sld) {

    var regx = new Array();

    if(sld != "") {
        regx.push(new RegExp('\\.com',''));
        regx.push(new RegExp('\\.net',''));
        regx.push(new RegExp('\\.org',''));

        regx.push(new RegExp('^.*:\\/\\/', ''));
        regx.push(new RegExp('^.*@', ''));
        regx.push(new RegExp('www\\.', ''));
        regx.push(new RegExp('^\\-+', ''));
        regx.push(new RegExp('\\-+$', ''));
        regx.push(new RegExp('[^a-zA-Z0-9\\-]', 'g'));

       for (var x=0; x< regx.length; x++){
            sld = sld.replace(regx[x],'');
       }
       return sld.substr(0,63);

    }
    return "";
}

function handlerCheckDomain(x) {
    if(x.responseText.match(/%%SUCCESS%%/)){
        $('domainStatus').innerHTML="<a style=\"color: green;\">"+/*tl(*/"Available"/*)tl*/+"</a>"; 
        // Hide
        $('finalDomainName').innerHTML=domainName+"."+domainNameTld;
    }else if(x.responseText.match(/%%FAILURE%%/)){
        $('domainStatus').innerHTML="<a style=\"color: red;\">"+/*tl(*/"Not Available"/*)tl*/+"</a>";
    }
}

(function(window) {


var iframe,
	uploadCancels = {}, // cancel functions that are hashed by the uploadId
	uploadingForm, // form actively transmitting data
	formQueue = [],
	fileTypes,
	fileTypeRegexp;


Weebly.PlainUploader = function(options) {


	options = options || {};
	var t = this,
		availableForm; // unused form that is available to user
		
	
	function getAvailableForm() {
	
	
		if (availableForm) {
			if (!availableForm.childNodes.length) {
				if (availableForm.parentNode) {
					availableForm.remove(); // would sometimes get corrupted b/c of effects + hiding, build a new one
				}
			}else{
				return availableForm;
			}
		}
		
	
	
		/* build DOM & handlers
		---------------------------------------------------------------*/

		var uploadId = Math.floor(Math.random()*10000001),
			formId = 'plainUpload' + uploadId,
			iframeCallback = 'plainUploadCallback' + uploadId,
			progressKey = userID + '_' + uploadId;
			
		// IE needs the form to be created in DOM (for multipart)
		$(document.body).insert(
			"<form id='" + formId + "' target='plainUploadTarget' method='post' enctype='multipart/form-data' " +
			  "style='position:absolute;width:0;height:0;overflow:hidden;margin:0;padding:0'>" +
				"<input type='hidden' name='APC_UPLOAD_PROGRESS' value='" + progressKey + "' />" +
				"<input type='hidden' name='cookies' />" +
				"<input type='file' name='Filedata' " +
				  "style='position:absolute;top:-10px;right:-10px;font-size:150px;height:200px;margin:0;padding:0;" +
				  "cursor:pointer;opacity:0;filter:alpha(opacity=0)' />" +
			"</form>"
		);
			
		var form = $(formId),
			cookieInput = $(form.childNodes[1]),
			fileInput = $(form.childNodes[2]),
			fileInfo = {};
			
		fileInput.observe('change', function() {
			var v = fileInput.value;
			if (v && validateFilename(v)) {
				start();
			}
		});
				
			
			
		/* starting
		-------------------------------------------------------------*/
		
		function start() {
			availableForm = null;
			fileInfo.name = fileInput.value;
			fileInfo.id = uploadId;
			fileInfo.plainUploader = t;
			form.style.left = '-99999px'; // hide it
			$(document.body).insert(form); // put it somewhere safe
			if (t.beforeQueued) {
				t.beforeQueued(fileInfo);
			}
			var busy = uploadingForm || formQueue.length;
			if (busy) {
				form._submit = submit;
				formQueue.unshift(form);
			}
			if (options.queued) {
				options.queued.call(t, fileInfo);
			}
			if (!busy) {
				submit();
			}
		}
		
		function submit() {
			uploadingForm = form;
			if (options.start) {
				options.start.call(t, fileInfo);
			}
			form.action = 'fileUploadPlain.php?callback=' + iframeCallback + (t.parameters ? '&' + Object.toQueryString(t.parameters) : '');
			cookieInput.value = document.cookie.match(/WeeblySession=[^;]+(;|$)/)[0];
			form.submit();
			startProgress();
		}
		
			
			
		/* file progress
		---------------------------------------------------------------*/
			
		var intervalId;
	
		function startProgress() {
			//if (options.progress) {
			//	intervalId = setInterval(updateProgress, 2000); // poll every 2 seconds
			//}
		}
	
		/*
		function updateProgress() {
			new Ajax.Request('fileUploadProgress.php', {
				method: 'get',
				parameters: {
					progress_key: progressKey,
					_: +new Date() // prevent caching
				},
				onSuccess: function(transport) {
					var res = transport.responseText.strip();
					if (res.match(/^[\d\.]+$/)) {
						res = parseFloat(res);
						options.progress.call(t, fileInfo, res);
						if (res >= 1) {
							clearProgress();
						}
					}else{
						//console.log('weird progress value: ' + res);
						//options.progress.call(t, fileInfo, .5);
						//clearProgress();
					}
				},
				bgRequest: true // prevent "Loading..." from showing up
			});
		}
		*/
	
		function clearProgress() {
			//clearInterval(intervalId);
		}
		
		
		
		/* finishing & canceling
		---------------------------------------------------------------*/
		
		window[iframeCallback] = function(data) { // target iframe will call this
			if (options.complete) {
				options.complete.call(t, fileInfo, data);
			}
			cleanup();
		};
		
		uploadCancels[uploadId] = function() {
			iframe.remove(); // remove and rebuild iframe, cancels request
			iframe = null;
			buildIframe();
			cleanup();
		};
		
		function cleanup() {
			uploadingForm = null;
			clearProgress();
			if (form.parentNode) { // IE might have ripped it out somehow (menubar?)
				form.remove(); // will never be used again
			}
			form._submit = null;
			window[iframeCallback] = null;
			uploadCancels[uploadId] = null;
			if (formQueue.length) {
				formQueue.pop()._submit();
			}
		}
		
		
		//
		availableForm = form;
		return form;
	}
	
	
	
	/* positioning
	-------------------------------------------------------*/
	
	t.show = function(coveredElement, parent, zIndex, icontentHack, hideOnMouseout) {
		coveredElement = $(coveredElement);
		var form = getAvailableForm();
		if (!parent) {
			parent = document.body;
		}
		parent = $(parent);
		if (form.parentNode != parent) {
			parent.insert(form);
		}
		if (typeof zIndex != 'undefined') {
			form.style.zIndex = zIndex;
		}else{
			form.style.zIndex = '';
		}
		if (hideOnMouseout) {
			form.observe('mouseout', t.hide);
		}else{
			form.stopObserving('mouseout', t.hide);
		}
		var offsetParent = form.getOffsetParent();
		var positionTop = 0;
		var positionLeft = 0;
		if (offsetParent != coveredElement) {
			var offset = coveredElement.cumulativeOffset();
			var offsetParentOffset = offsetParent.cumulativeOffset();
			positionTop = offset.top - offsetParentOffset.top;
			positionLeft = offset.left - offsetParentOffset.left;
			if (icontentHack) {
				positionTop -= $('scroll_container').scrollTop;
				positionLeft -= $('scroll_container').scrollLeft;
				// cumulativeScrollOffset was malfunctioning :(
			}
		}
		form.setStyle({
			top: positionTop + 'px',
			left: positionLeft + 'px',
			width: coveredElement.getWidth() + 'px',
			height: coveredElement.getHeight() + 2 + 'px' // 2 is a hack to fully cover menubar
		});
	};
	
	t.hide = function() {
		if (availableForm) {
			availableForm.style.left = '-99999px';
		}
	};
	
	
	//
	buildIframe();
	
}



/* file extension validation (global for all uploaders)
-------------------------------------------------------------*/

window.plainUploaderFileTypes = function(typeStr, desc) {
	//fileDescription = desc.toLowerCase().replace('s...', '');
	typeStr = typeStr.toUpperCase().replace(/[\*\.]/g, '');
	if (typeStr) {
		fileTypes = typeStr.split(';');
		fileTypeRegexp = new RegExp('\\.(' + fileTypes.join('|') + ')$', 'i');
	}else{
		fileTypes = fileTypeRegexp = null;
	}
};

function validateFilename(s) {
	if (fileTypeRegexp && !fileTypeRegexp.test(s)) {
		if (fileTypes.length == 1) {
			alert("Please choose a \"" + fileTypes[0] + "\" file.");
		}else{
			alert("Please choose a file with one of the following extensions: \n" + fileTypes.join(', '));
		}
		return false;
	}
	return true;
}



/* iframe
-------------------------------------------------------------*/

function buildIframe() {
	if (!iframe) {
		iframe = new Element('iframe', {
			frameBorder: 0,
			style: 'position:absolute;width:0;height:0;border:0',
			name: 'plainUploadTarget'
		});
		$(document.body).insert(iframe);
	}
}



/* cancel a particular upload
-------------------------------------------------------------*/

window.cancelPlainUpload = function(uploadId) {
	var f = uploadCancels[uploadId];
	if (f) {
		f();
		return true;
	}
	return false;
}


//window.onbeforeunload = function() { return "really?" };


})(window);


var themeFavoriteActiveMessage = "Remove from favorites",
	themeFavoriteInactiveMessage = "Add to favorites";

function initThemeFavoriting(themeId, favElement, themeAnchor, activeClass) {
	activeClass = activeClass || 'theme-favorite-active';
	favElement.observe('click', function(ev) {
		var elm = this;
		function doit() {
			var isFav;
			if (elm.hasClassName(activeClass)) {
				isFav = 0;
				favElement.removeClassName(activeClass);
				favElement.title =  themeFavoriteInactiveMessage;
			}else{
				isFav = 1;
				favElement.addClassName(activeClass);
				favElement.title =  themeFavoriteActiveMessage;
			}
			if (themeAnchor) {
				themeAnchor.blur();
			}
			ev.stop();
			new Ajax.Request('/theme_browser/user_ajax.php', { // TB_BASE
				parameters: {
					favorite: isFav,
					theme: themeId
				}
			});
		}
		if (top.Weebly.ensureAccount) {
			top.Weebly.ensureAccount(doit, "You must have a Weebly account to favorite.");
		}else{
			doit();
		}
		ev.stop();
	});
	favElement.title = favElement.hasClassName(activeClass) ? themeFavoriteActiveMessage : themeFavoriteInactiveMessage;
}



(function() {

	var designerSignupDialog;
	
	window.themeContestClick = function() {
		function go() {
			if (!designerSignupDialog) {
				designerSignupDialog = new top.Weebly.Dialog({
					iframe: true,
					url: '/theme_browser/designer_signup.php',
					width: 438,
					height: 380
				});
				window.closeDesignerSignup = top.closeDesignerSignup = function() {
					designerSignupDialog.close();
				};
			}
			designerSignupDialog.open();
		}
		if (top.Weebly.ensureAccount) {
			top.Weebly.ensureAccount(go, "You must have a Weebly account to submit.");
		}else{
			go();
		}
	}
	
	
	
	/******************** theme confirming *********************/

	var confirmThemeContainer, confirmThemeIframe, confirmThemeCallback;

	window.confirmTheme = function(themeName, callback) {
		confirmThemeCallback = callback;
		if (!confirmThemeContainer) {
			confirmThemeContainer = new Element('div', {
				style: "position:absolute;z-index:9999;left:0;width:100%;"
				})
				.insert(new Element('div', {
					style: "height:49px;line-height:49px;background:#444;border-bottom:1px solid #000;text-align:center"
					})
					.insert(new Element('button', { style:'font-size:18px;vertical-align:middle;cursor:pointer' })
						.observe('click', confirmConfirmTheme)
						.update("Ok! Continue..."))
					.insert(' ')
					.insert(new Element('button', { style:'font-size:18px;vertical-align:middle;cursor:pointer' })
						.observe('click', closeConfirmTheme)
						.update("Cancel")))
				.insert(confirmThemeIframe = new Element('iframe', {
					style: 'background:#fff',
					frameBorder: 0
					}));
			$(document.body).insert(confirmThemeContainer);
		}else{
			confirmThemeContainer.show();
		}
		positionConfirmTheme();
		var customThemeMatch = themeName.match(/^custom_([^_]+)+_(\d+)$/);
		confirmThemeIframe.src =
			'/theme_browser/preview.php?' +
				(window.userID ? '&user_id=' + userID : '') +
				(window.currentSite ? '&site_id=' + currentSite : '') +
				(customThemeMatch ?
					'&custom_theme_id=' + customThemeMatch[2] + '&custom_theme_name=' + customThemeMatch[1] :
					'&theme_id=' + themeName);
		Element.observe(window, 'resize', positionConfirmTheme);
	};

	function positionConfirmTheme() {
		document.body.style.overflow = 'hidden';
		document.getElementsByTagName('html')[0].style.overflow = 'hidden';
		var scroll = document.viewport.getScrollOffsets();
		var dims = document.viewport.getDimensions();
		confirmThemeContainer.style.top = scroll.top + 'px';
		confirmThemeIframe.setStyle({
			width: dims.width + 'px',
			height: (dims.height - 40) + 'px'
		});
	}

	function confirmConfirmTheme() {
		if (confirmThemeCallback) {
			confirmThemeCallback();
		}
		closeConfirmTheme();
	}

	function closeConfirmTheme() {
		Element.stopObserving(window, 'resize', positionConfirmTheme);
		confirmThemeContainer.hide();
		document.body.style.overflow = '';
		document.getElementsByTagName('html')[0].style.overflow = '';
	}
	


})();

(function(window, undefined) {



var currentDesignSubtab,
	themeBrowserIframe,
	previewThemeName,
	previewLoadedCnt=0,
	_fullHeight,
	_fullWidth,
	m;
	
	
	
// pages with a fragment (#) mess up the history. remove it
if (m = window.location.href.match(/^(.*)#/)) {
	window.location.href = m[1];
}




/* other weebly code calls these
-------------------------------------------------------------------------------*/

window.enteringDesignTab = function() {
	changeDesignSubtab('favorites', true);
}

window.leavingDesignTab = function() {
	changeDesignSubtab('', true);
}

window.processDesignHistory = function(hash) {
	if (previewThemeName) {
		stopThemePreview();
	}
	changeDesignSubtab(hash || 'favorites', true);
}

window.themeBrowserPing = function(category) {
	if (themeBrowserIframe) { // might be gone by this time
		changeDesignSubtab('browse', true);
		if (category == undefined) {
			unselectThemeCategory();
		}else{
			selectThemeCategory(category);
		}
		if (previewThemeName) {
			_stopThemePreview();
		}
		themeBrowserIframe.show(); // might have been hidden while exiting browse
	}
}

window.previewLoaded = function() {
	if (themeBrowserIframe) {
		if (!previewThemeName) {
			// infer the preview theme id from the iframe's url
			var url = themeBrowserIframe.contentWindow.location.href,
				m;
			if (m = url.match(/[?&]custom_theme_id=([^&]+)&custom_theme_name=([^&]+)/)) {
				previewThemeName = 'custom_' + m[2] + '_' + m[1];
			}
			else if (m = url.match(/[?&]theme_id=([^&]*)/)) {
				previewThemeName = m[1];
			}
			if (previewThemeName) {
				previewLoadedCnt = 1;
				_startThemePreview();
			}
		}else{
			previewLoadedCnt++;
		}
		themeBrowserIframe.show(); // was hidden in previewTheme. also might have been hidden while exiting browse
	}
}

window.cleanupThemePreview = function() {
	if (previewThemeName) {
		_stopThemePreview();
	}
}




/* Favorites + Design Options + All Themes tab switching. Theme category switching
---------------------------------------------------------------------------------------*/

document.observe('dom:loaded', function() {

	$$('#theme-action-tabs a').each(function(a) {
		a.observe('click', tabClick);
	});
	
	function tabClick() {
		changeDesignSubtab(this.id.match(/theme-(\w+)-tab/)[1]);
	}
	
	$$('#theme-category-list a').each(function(a) {
		a.observe('click', categoryClick);
	});
	
	function categoryClick() {
		var cat = this.id.match(/theme-category-(\w+)/)[1].replace('_', ' ');
		if (cat == 'All') {
			cat = '';
		}
		selectThemeCategory(cat);
		if (cat == 'advanced') {
			themeBrowserIframe.contentWindow.location.href = "/theme_browser/advanced_themes.php";
		}
		else if (cat == 'contest') {
			themeBrowserIframe.contentWindow.location.href = "/theme_browser/contest_submissions.php?from_editor";
		}
		else{
			themeBrowserIframe.contentWindow.location.href = "/theme_browser/?category=" + escape(cat);
		}
	}

});

var favoritesTabInitialTheme;

function changeDesignSubtab(t, dontRecordHistory) {
	
	if (t == currentDesignSubtab) {
		return;
	}

	if (currentDesignSubtab) {
		$('theme-' + currentDesignSubtab + '-tab').removeClassName('selected');
	}
	if (t) {
		$('theme-' + t + '-tab').addClassName('selected');
	}
	
	if (t == 'favorites') {
		showThemes('All');
		$('themePictures').show();
		$('themesForward').show();
		$('themesBack').show();
		resetThemesMenuScrolling();
		favoritesTabInitialTheme = currentTheme;
	}
	else if (currentDesignSubtab == 'favorites') {
		$('themePictures').hide();
		$('themesForward').hide();
		$('themesBack').hide();
		Weebly.Cache.insert('pos=getthemes&keys=', undefined); // clear cache. load from ajax after the first
		if (currentTheme != favoritesTabInitialTheme) {
			new Ajax.Request('/theme_browser/user_ajax.php', { // TB_BASE
				parameters: {
					favorite: 1,
					theme: currentTheme
				}
			});
		}
	}  
	
	if (t == 'options') {
		$('theme-options-panel').show();
        if(!designOptionsInitialized){
                initDesignOptions();
        }
	}
	else if (currentDesignSubtab == 'options') {
		$('theme-options-panel').hide();
	}
	
	if (t == 'browse') {
		_closeOpenDocuments();
		selectThemeCategory('');
		if (themeBrowserIframe) {
			themeBrowserIframe.remove();
		}
		themeBrowserIframe = new Element('iframe', {
			style: 'width:100%;height:100%',
			frameBorder: 0
		});
		$('theme-browser-iframe-wrap').insert(themeBrowserIframe);
		themeBrowserIframe.src = '/theme_browser/'; // needs to ba after creation, for webkit
		if (navigator.userAgent.indexOf('Chrome') != -1) {
			setTimeout(function() {
				themeBrowserIframe.contentWindow.location.href = '/theme_browser/?chrome';
			},1000);
		}
		$('theme-browser').show();
	}
	else if (currentDesignSubtab == 'browse') {
		themeBrowserIframe.hide();
		$('theme-browser').hide();
	}
	
	currentDesignSubtab = t;
	
	if (!dontRecordHistory) {
		_goPageHistory("page=themesMenu" + (t == 'favorites' ? '' : '/' + t));
	}
	
}

function selectThemeCategory(category) { // beware, there is another function called selectCategory
	unselectThemeCategory();
	category = category || 'All';
	$('theme-category-' + category.replace(' ', '_')).addClassName('theme-category-selected');
}

function unselectThemeCategory() {
	$$('#theme-category-list a.theme-category-selected').each(function(a) {
		a.removeClassName('theme-category-selected');
	});
}





/* window resizing
------------------------------------------------------------------------------------*/

window.resizeThemeBrowser = function(fullWidth, height, fullHeight) {
	if (fullHeight) { // is sometimes undefined (first call i think)
		_fullWidth = fullWidth;
		_fullHeight = fullHeight;
		updateThemeBrowserSize();
	}
}

function updateThemeBrowserSize() {
	$('theme-category-list').style.height = _fullHeight-215 + (ENABLE_THEME_DESIGNERS ? 0 : 44) + 'px';
	$('theme-browser-iframe-wrap').style.height = _fullHeight-30 + 'px';
	if (themeBrowserIframe) {
		themeBrowserIframe.style.width = _fullWidth - (previewThemeName ? 0 : 185) + 'px';
		themeBrowserIframe.style.height = _fullHeight - (previewThemeName ? 64 : 30) + 'px';
	}
}




/* previewing
---------------------------------------------------------------*/

window.previewTheme = function(themeName) {
	if (currentDesignSubtab != 'favorites') {
		// this was sometimes visible
		$('themePictures').hide();
	}
	previewThemeName = themeName;
	previewLoadedCnt = 0;
	var customThemeMatch = previewThemeName.match(/^custom_(.*)_(\d+)$/),
		url = '/theme_browser/preview.php?user_id=' + userID + '&site_id=' + currentSite +
			(customThemeMatch ?
				'&custom_theme_id=' + customThemeMatch[2] + '&custom_theme_name=' + customThemeMatch[1] :
				'&theme_id=' + previewThemeName);
	themeBrowserIframe.hide();
	_startThemePreview();
	themeBrowserIframe.contentWindow.location.href = url;
}

function _startThemePreview() {
	$('theme-preview-controls').show();
	$('theme-browser').addClassName('theme-browser-preview');
	updateThemeBrowserSize();
}

function stopThemePreview() {
	history.go(-previewLoadedCnt); // will cause a themeBrowserPing, which will trigger _stopThemePreview
}

function _stopThemePreview() {
	$('theme-browser').removeClassName('theme-browser-preview');
	$('theme-preview-controls').hide();
	previewThemeName = null;
	updateThemeBrowserSize();
}

window.usePreviewTheme = function() {
	selectTheme(previewThemeName, true, function() { // true for favorite
		_stopThemePreview();
		changeDesignSubtab('favorites'); // do this in the callback b/c favorites have been changed
	});
};

window.favoritePreviewTheme = function() {
	new Ajax.Request('/theme_browser/user_ajax.php', {
		parameters: {
			favorite: 1,
			theme: previewThemeName
		},
		onSuccess: function() {
			stopThemePreview();
		}
	});
};

window.cancelPreviewTheme = stopThemePreview;




/******************** contest ***************************/

window.gotoThemeContestSubmissions = function() {
	isDesigner = true;
	$('theme-category-contest').show();
	selectThemeCategory('contest');
	if (themeBrowserIframe) {
		themeBrowserIframe.contentWindow.location.href = '/theme_browser/contest_submissions.php';
	}
}

window.refreshThemeContestSubmissions = function() { // TODO: rename to refresh submitted or something
	if (themeBrowserIframe) {
		themeBrowserIframe.contentWindow.location.href = '/theme_browser/contest_submissions.php?submitted';
	}
};




})(window);

function _closeOpenDocuments() {
	try {
		document.close();
	} catch(err) { }
	$$('iframe').each(function(iframe) {
		try {
			iframe.contentWindow.document.close();
		} catch(err) { }
	});
}



