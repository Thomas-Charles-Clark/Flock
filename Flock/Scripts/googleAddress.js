 
function setupGoogleAddressAutocomplete() {    
    var input = document.getElementById('address-search-field');
       
    autocomplete = new google.maps.places.Autocomplete(input);
   
    google.maps.event.addListener(autocomplete, 'place_changed', function() {
        var place = autocomplete.getPlace();
        var city;
        var state;
        var country;
        var postcode;
        var addressLineOne;
        var addressLineTwo;
        var latitude;
        var longitude;
           
        if (!place.geometry) {
            // Inform the user that the place was not found and return.
            input.className = 'notfound';
            return;
        }
           
        if (place && place.geometry){
            address_components = place.address_components;
            if (address_components){
                for (index in address_components){
                    var component = address_components[index];
                    if (component.types.indexOf('locality') > -1){
                        city = component.long_name;
                    }
                    if (component.types.indexOf('administrative_area_level_1') > -1){
                        state = component.long_name;
                        if (!state){
                            if (component.types.indexOf('administrative_area_level_2') > -1){
                                state = component.long_name;
                                if (!state){
                                    if (component.types.indexOf('administrative_area_level_3') > -1){
                                        state = component.long_name;
                                    }
                                }
                            }
                        }
                    }
                    if (component.types.indexOf('country') > -1) {
                        country = component.long_name;
                    }
                    if (component.types.indexOf('postal_code') > -1) {
                        postcode = component.long_name;
                    }
                    if (component.types.indexOf('street_number') > -1) {
                        addressLineOne = component.short_name;
                    }
                    if (component.types.indexOf('route') > -1) {
                        addressLineOne += " " + component.long_name;
                    }

                }
            }
            if (place.geometry.location) {
                latitude = place.geometry.location.lat();
                longitude = place.geometry.location.lng();
            }
        } else {
            // Inform the user that the place was not found and return.
            input.className = 'notfound';
            return;
        }

        $("#AddressLineOne_I").val(addressLineOne);
        $("#AddressLineTwo_I").val();
        //$("#country").val(country);
        $("#PostCode_I").val(postcode);
        $("#City_I").val(city);

    });
}
