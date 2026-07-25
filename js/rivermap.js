

function locate() {//搜尋地址
    var initialLocation;// = new google.maps.LatLng(25.09108, 121.5598);


    if (navigator.geolocation) {
        browserSupportFlag = true;
        navigator.geolocation.getCurrentPosition(function (position) {
            initialLocation = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
            //map.setCenter(initialLocation);
            alert(initialLocation);
        }, function () {
            handleNoGeolocation(browserSupportFlag);
        });
    }
        // Browser doesn't support Geolocation
    else {
        browserSupportFlag = false;
        handleNoGeolocation(browserSupportFlag);
    }


    function handleNoGeolocation(errorFlag) {
        if (errorFlag == true) {
            alert("地圖定位失敗");
        } else {
            alert("您的瀏覽器不支援定位服務");
        }
      //  initialLocation = taipei;
     
        //map.setCenter(initialLocation);
    }

    var geocoder = new google.maps.Geocoder();
    if (geocoder) {
        geocoder.geocode({ 'location': initialLocation }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
               


                if (results[0]) {
                //    window.alert(results[0].formatted_address);
                //    $("#address").val(results[0].formatted_address);
                    document.getElementById("address").innerHTML = results[1].formatted_address;
                 //   loczoon();
                } else {
                    window.alert('No results found');
                }

            }
            else {
                alert("查無資料");
            }
        });
    }
    else
        alert("查無資料");

    }

 
       
  

