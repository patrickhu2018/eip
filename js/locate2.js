function locate2()
{

    if(location.protocol != 'https:') 
    {
        if(window.chrome) {
            var position = {
                coords: {
                    latitude: '',
                    longitude: ''
                }
            };
            $.getJSON("http://ip-api.com/json", function (data, status) {
            if(status === "success") {
                if(data) {
                    $.getJSON("http://maps.googleapis.com/maps/api/geocode/json?address=" + res.zip, function (data, status) {
                    if (status === "success") {
                        position.coords.latitude = data.results[0].geometry.location.lat;
                        position.coords.longitude = data.results[0].geometry.location.lng;
                        locationOnSuccess(position);
                    } else {
                        locationOnError();
                    }
                });        
            } else {
                if(!data.zip && data.lat && data.lon) {
                    //if there's not zip code but we have a latitude and longitude, let's use them
                    position.coords.latitude = data.lat;
                    position.coords.longitude = data.lon;
                    locationOnSuccess(position);
                } else {
                    //if there's an error 
                    locationOnError();
                }
            }
        } else {
            locationOnError();
        }
    });
        }
        else 
        {
      navigator.geolocation.getCurrentPosition(locationOnSuccess, locationOnError, geo_options);
         }
    }
       else 
            navigator.geolocation.getCurrentPosition(locationOnSuccess, locationOnError, geo_options);
        


}

function locationOnSuccess(position)
{
    alert(position);

}

function locationOnError(errorFlag) {
    if (errorFlag == true) {
        alert("地圖定位失敗");
    } else {
        alert("您的瀏覽器不支援定位服務");
    }
}