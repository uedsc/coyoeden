
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

