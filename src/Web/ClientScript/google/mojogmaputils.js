var geocoder = null;

function initializeCmaputils() {
if (GBrowserIsCompatible()) {
	geocoder = new GClientGeocoder();
	}
}

function showGMap(divMap, address, enableMapType, enableZoom, showInfoWindow, enableLocalSearch, mapType, zoomLevel) 
{
	if (geocoder == null) 
	{
	 initializeCmaputils();
	}
	
	if (geocoder) 
	{
	    geocoder.getLatLng(
	  address,
	  function(point) {
	      if (!point) {
	          //alert(address + " not found");
	          divMap.innerHTML = 'location not found';
	      }
	      else {
	          var map = new GMap2(divMap);
	          
	          map.setCenter(point, 13);

	          if (mapType)
	              map.setMapType(mapType);

	          var marker = new GMarker(point);
	          map.addOverlay(marker);

	          if (enableZoom)
	              map.addControl(new GLargeMapControl());

	          if (enableMapType)
	              map.addControl(new GMapTypeControl());

	          if (showInfoWindow)
	              marker.openInfoWindowHtml(address);

	          if (enableLocalSearch) {
	              map.addControl(new google.maps.LocalSearch(), new GControlPosition(G_ANCHOR_BOTTOM_RIGHT, new GSize(10, 20)));
	          }

	          if (zoomLevel)
	              map.setZoom(zoomLevel);
	          //alert(map.getZoom());

	      }
	  }
	);
	}
}

function showMapAndDirections(divMapId, divDirectionsId, txtFromAddressId, toAddress, locale) 
{
    if (!GBrowserIsCompatible()){return;}
    
    var map = new GMap2(document.getElementById(divMapId));
    
	if(! map){ return;}
	
	var divDir = document.getElementById(divDirectionsId);
	if(! divDir){ alert('divdir was null');return;}
	
	var gdir = new GDirections(map, divDir);
	
	var txtFrom = document.getElementById(txtFromAddressId);
	if(! txtFrom){ return;}
    
    var fromLocation = txtFrom.value;
	
	gdir.load("from: " + fromLocation + " to: " + toAddress,{ "locale": locale });
	
	
	/*
	
	var statuscode = gdir.getStatus().code;
	
	if (statuscode == G_GEO_UNKNOWN_ADDRESS)
	     divDir.innerHTML = 'No corresponding geographic location could be found for one of the specified addresses. This may be due to the fact that the address is relatively new, or it may be incorrect.';
	   else if (statuscode == G_GEO_SERVER_ERROR)
	    divDir.innerHTML = 'A geocoding or directions request could not be successfully processed, yet the exact reason for the failure is not known.';
	     
	   else if (statuscode == G_GEO_MISSING_QUERY)
	     divDir.innerHTML = 'The HTTP q parameter was either missing or had no value. For geocoder requests, this means that an empty address was specified as input. For directions requests, this means that no query was specified in the input.';

	   else if (statuscode == G_GEO_BAD_KEY)
	     divDir.innerHTML = 'The given key is either invalid or does not match the domain for which it was given.';

	   else if (statuscode == G_GEO_BAD_REQUEST)
	     divDir.innerHTML = 'A directions request could not be successfully parsed.';
	     */
	  
}
    
