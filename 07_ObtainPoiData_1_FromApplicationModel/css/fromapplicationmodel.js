//var aa;
//function passData(id) { 
//    aa=JSON.stringify(id); 
//    alert(aa);
//}
// implementation of AR-Experience (aka "World")
var World = {

    initiallyLoadedData: false,
	// different POI-Marker assets
	markerDrawable_idle: null,
	markerDrawable_selected: null,
	markerDrawable_directionIndicator: null,

	// list of AR.GeoObjects that are currently shown in the scene / World
	markerList: [],
	// The last selected marker
	currentMarker: null,

	/*
		Have a look at the native code to get a better understanding of how data can be injected to JavaScript.
		Besides loading data from assets it is also possible to load data from a database, or to create it in native code. Use the platform common way to create JSON Objects of your data and use native 'architectView.callJavaScript()' to pass them to the ARchitect World's JavaScript.
		'World.loadPoisFromJsonData' is called directly in native code to pass over the poiData JSON, which then creates the AR experience.
	*/
	// called to inject new POI data
	loadPoisFromJsonData: function loadPoisFromJsonDataFn(poiData) {
	//function loadPoisFromJsonData(poiData) {
		World.markerList = [];

		// start loading marker assets
		World.markerDrawable_idle = new AR.ImageResource("assets/marker_idle.png");
		World.markerDrawable_selected = new AR.ImageResource("assets/marker_selected.png");
		World.markerDrawable_directionIndicator = new AR.ImageResource("assets/indi.png");

		// loop through POI-information and create an AR.GeoObject (=Marker) per POI
		for (var currentPlaceNr = 0; currentPlaceNr < poiData.length; currentPlaceNr++) {
			var singlePoi = {
				"id": poiData[currentPlaceNr].id,
				"latitude": parseFloat(poiData[currentPlaceNr].latitude),
				"longitude": parseFloat(poiData[currentPlaceNr].longitude),
				"altitude": parseFloat(poiData[currentPlaceNr].altitude),
				"title": poiData[currentPlaceNr].name,
				"description": poiData[currentPlaceNr].description
			};

			World.markerList.push(new Marker(singlePoi));
		}

		World.updateStatusMessage(currentPlaceNr + ' places loaded');
	},

	// updates status message shon in small "i"-button aligned bottom center
	updateStatusMessage: function updateStatusMessageFn(message, isWarning) {

		var themeToUse = isWarning ? "e" : "c";
		var iconToUse = isWarning ? "alert" : "info";

		$("#status-message").html(message);
		$("#popupInfoButton").buttonMarkup({
			theme: themeToUse
		});
		$("#popupInfoButton").buttonMarkup({
			icon: iconToUse
		});
	},

	// location updates, fired every time you call architectView.setLocation() in native environment
	locationChanged: function locationChangedFn(lat, lon, alt, acc) {
		if (!World.initiallyLoadedData) {
		    World.requestDataFromLocal(lat, lon);
			World.initiallyLoadedData = true;
		}
	},

	// fired when user pressed maker in cam
	onMarkerSelected: function onMarkerSelectedFn(marker) {

		// deselect previous marker
		if (World.currentMarker) {
			if (World.currentMarker.poiData.id == marker.poiData.id) {
				return;
			}
			World.currentMarker.setDeselected(World.currentMarker);
		}

		// highlight current one
		marker.setSelected(marker);
		World.currentMarker = marker;
	},

	// screen was clicked but no geo-object was hit
	onScreenClick: function onScreenClickFn() {
		if (World.currentMarker) {
			World.currentMarker.setDeselected(World.currentMarker);
		}
	},

	// request POI data
	requestDataFromLocal: function requestDataFromLocalFn(centerPointLatitude, centerPointLongitude) {
	//requestDataFromLocal: function requestDataFromLocalFn() {

		var poiData = [];

		poiData.push({
            "id": 17,
		    "latitude": 42.3918572037939,
			"longitude": -72.5266739353538,
			"description": ("East Entrance"),
			"altitude": "3.0",
			"name": "East Entrance" 
		});
		//poiData.push({
  //          "id": 00,
		//    "latitude": centerPointLatitude,
		//	"longitude": centerPointLongitude5,
		//	"description": ("Here"),
		//	"altitude": "3.0",
		//	"name": "Here" 
		//});
		poiData.push({
            "id": 19,
		    "latitude": 42.3918886513449,
			"longitude": -72.5268563255668,
			"description": ("Harvest Market"),
			"altitude": "3.0",
			"name": "Harvest Market" 
		});
		poiData.push({
            "id": 21,
		    "latitude": 42.3918282324141,
			"longitude": -72.5271138176322,
			"description": ("Information Desk"),
			"altitude": "3.0",
			"name": "Information Desk" 
		});
		poiData.push({
            "id": 22,
		    "latitude": 42.3917900991245,
			"longitude": -72.5272868201137,
			"description": ("UStore"),
			"altitude": "3.0",
			"name": "UStore" 
		});
		poiData.push({
            "id": 23,
		    "latitude": 42.3917031848521,
			"longitude": -72.5273525342345,
			"description": ("West Entrance"),
			"altitude": "3.0",
			"name": "West Entrance" 
		});
		poiData.push({
            "id": 26,
		    "latitude": 42.3917002134218,
			"longitude": -72.526957243681,
			"description": ("Men's Restroom Main Floor"),
			"altitude": "3.0",
			"name": "Men's Restroom Main Floor" 
		});
		poiData.push({
            "id": 27,
		    "latitude": 42.3916895657956,
			"longitude": -72.5270045176148,
			"description": ("Women's Restroom Main Floor"),
			"altitude": "3.0",
			"name": "Women's Restroom Main Floor" 
		});
		poiData.push({
            "id": 28,
		    "latitude": 42.3916650514865,
			"longitude": -72.5269686430693,
			"description": ("Stairs Main Floor"),
			"altitude": "3.0",
			"name": "Stairs Main Floor" 
		});
		poiData.push({
            "id": 29,
		    "latitude": 42.39177128009,
			"longitude": -72.526867389679,
			"description": ("Cafe"),
			"altitude": "3.0",
			"name": "Cafe" 
		});
		poiData.push({
            "id": 30,
		    "latitude": 42.3917836610344,
			"longitude": -72.5267047807574,
			"description": ("Blue Wall"),
			"altitude": "3.0",
			"name": "Blue Wall" 
		});
		poiData.push({
            "id": 526,
		    "latitude": 42.3918767656582,
			"longitude": -72.5270564854145,
			"description": ("Escalator Main Floor"),
			"altitude": "3.0",
			"name": "Escalator Main Floor" 
		});
		poiData.push({
            "id": 49,
		    "latitude": 42.3917311658133,
			"longitude": -72.527086995542,
			"description": ("Elevator Main Floor"),
			"altitude": "3.0",
			"name": "Elevator Main Floor" 
		});
		World.loadPoisFromJsonData(poiData);
	}

};

AR.context.onLocationChanged = World.locationChanged;
AR.context.onScreenClick = World.onScreenClick;