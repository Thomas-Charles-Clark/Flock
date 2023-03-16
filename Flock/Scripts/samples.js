// Here's a custom Knockout binding that makes elements shown/hidden via jQuery's fadeIn()/fadeOut() methods
// Could be stored in a separate utility library
ko.bindingHandlers.fadeVisible = {
    init: function (element, valueAccessor) {
        // Initially set the element to be instantly visible/hidden depending on the value
        var value = valueAccessor();
        $(element).toggle(ko.unwrap(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
    },
    update: function (element, valueAccessor) {
        // Whenever the value subsequently changes, slowly fade the element in or out
        var value = valueAccessor();
        ko.unwrap(value) ? $(element).fadeIn() : $(element).fadeOut();
    }
};

// Thanks to http://stackoverflow.com/questions/17048468/make-an-input-only-numeric-type-on-knockout
ko.bindingHandlers.numeric = {
    init: function (element, valueAccessor) {
        $(element).on("keydown", function (event) {

            // Allow: backspace, delete, tab, escape, and enter
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                // Allow: Ctrl+A
                (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: both . keys (comma is event.keyCode == 188) if the Europeans complain about formatting... 
                (event.keyCode == 190 || event.keyCode == 110) ||
                // Allow: home, end, left, right
                (event.keyCode >= 35 && event.keyCode <= 39)) {

                // let the keypress occur
                return;
            }
            else {
                // Ensure that it is a number and stop the keypress
                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }
            }
        });

        $(element).on("keyup", function (event) {
            if (isNaN($(element).val())) {
                RedFade($(this));
            } else {
                $(this).css("background-color", "transparent");
            }
        });
    }
};

//http://knockoutjs.com/documentation/extenders.html
ko.extenders.roundedNumeric = function (target, precision) {
    //create a writable computed observable to intercept writes to our observable
    var result = ko.pureComputed({
        read: target,  //always return the original observables value
        write: function (newValue) {


            if (newValue === "")
                newValue = null;

            var current = target(),
                roundingMultiplier = Math.pow(10, precision),
                newValueAsNum = isNaN(newValue) ? 0 : +newValue,
                valueToWrite = Math.round(newValueAsNum * roundingMultiplier) / roundingMultiplier;
            
            valueToWrite = Number(valueToWrite).toFixed(precision);

            // only write if it has changed

            if (newValue === null && current === null)
                return;

            if (newValue === null)
                valueToWrite = null;

            if (isNaN(newValue) && newValue !== null) {
                return;
            }

            // and if both new and current are not null
            //if (newValue !== null && current !== null) {
                if (valueToWrite !== current) {
                    target(valueToWrite);
                } else {
                    //if the rounded value is the same, but a different value was written, force a notification for the current field
                    if (newValue !== current) {
                        target.notifySubscribers(valueToWrite);
                    }
                }
            //}
        }
    }).extend({ notify: 'always' });

    //initialize with current value to make sure it is rounded appropriately
    result(target());

    //return the new computed observable
    return result;
};

function RedFade(ele) {
    var col = $(ele).css("background-color");
    $(ele).animate({ 'background-color': 'rgba(255, 0, 0, 0.2)' }, 200, 'linear', function () {
        //$(ele).animate({ 'background-color': col }, 400, 'linear', function () {

        //});
    });
}

var Address = function (address) {
    var self = this;

    self.PCASearchTerm = ko.observable(null);
    self.PCASearchId = ko.observable(null);
    self.PCAContainer = ko.observable(null);

    self.Name = ko.observable(address.Name);
    self.DeliveryReference = ko.observable(address.DeliveryReference);
    self.AddressLine1 = ko.observable(address.AddressLine1);
    self.AddressLine2 = ko.observable(address.AddressLine2);
    self.AddressLine3 = ko.observable(address.AddressLine3);
    self.AddressLine4 = ko.observable(address.AddressLine4);
    self.Postcode = ko.observable(address.Postcode);
    self.Country = ko.observable(address.Country);
    self.CountryCode = ko.observable(address.CountryCode);
    self.CountryID = ko.observable(address.CountryID);

    self.SelectListDisplayLine = ko.pureComputed(function () {

        var lineArray = [];
        // if last character is ',', remove it

        //if (self.DeliveryReference()) {
        //    var d = self.DeliveryReference();
        //    if (d)
        //      lineArray.push(d);
        //}

        if (self.AddressLine1()) {
            var a1 = self.AddressLine1();
            if (a1) {
                lineArray.push(a1);
            }
        }
        if (self.AddressLine2()) {
            var a2 = self.AddressLine2();
            if (a2) {
                lineArray.push(a2);
            }
        }
        //if (self.AddressLine3()) {
        //    var a3 = self.AddressLine3();
        //    if (a3)
        //         lineArray.push(a3);
        //}
        //if (self.AddressLine4()) {
        //    var a4 = self.AddressLine4();
        //    if (a4)
        //      lineArray.push(a4);
        //}
        if (self.Postcode()) {
            var p = self.Postcode();
            if (p) {
                lineArray.push(p);
            }
        }
        if (self.Country()) {
            var c = self.Country();
            if (c) {
                lineArray.push(c);
            }
        }

        return lineArray.join(', ');//self.DeliveryReference() + "\xa0\xa0\xa0\xa0\xa0" + lineArray.join(', ');
    }, self);

    self.DisplayLine = ko.pureComputed(function () {

        var lineArray = [];
        // if last character is ',', remove it

        if (self.Name()) {
            var n = self.Name();
            if (n)
                lineArray.push(n);
        }

        if (self.AddressLine1()) {
            var a1 = self.AddressLine1();
            if (a1)
                lineArray.push(a1);
        }
        if (self.AddressLine2()) {
            var a2 = self.AddressLine2();
            if (a2)
                lineArray.push(a2);
        }
        if (self.AddressLine3()) {
            var a3 = self.AddressLine3();
            if (a3)
                lineArray.push(a3);
        }
        if (self.AddressLine4()) {
            var a4 = self.AddressLine4();
            if (a4)
                lineArray.push(a4);
        }
        if (self.Postcode()) {
            var p = self.Postcode();
            if (p)
                lineArray.push(p);
        }
        if (self.Country()) {
            var c = self.Country();
            if (c)
                lineArray.push(c);
        }

        return lineArray.join(', ');
    }, self);


    self.SummaryDisplayLines = ko.pureComputed(function () {

        var lineArray = [];
        // if last character is ',', remove it

        if (self.Name()) {
            var n = self.Name();
            if (n)
                lineArray.push(n);
        }

        if (self.AddressLine1()) {
            var a1 = self.AddressLine1();
            if (a1)
                lineArray.push(a1);
        }
        if (self.AddressLine2()) {
            var a2 = self.AddressLine2();
            if (a2)
                lineArray.push(a2);
        }
        if (self.AddressLine3()) {
            var a3 = self.AddressLine3();
            if (a3)
                lineArray.push(a3);
        }
        if (self.AddressLine4()) {
            var a4 = self.AddressLine4();
            if (a4)
                lineArray.push(a4);
        }
        if (self.Postcode()) {
            var p = self.Postcode();
            if (p)
                lineArray.push(p);
        }
        if (self.Country()) {
            var c = self.Country();
            if (c)
                lineArray.push(c);
        }

        return lineArray; // returned as array; used via foreach
    }, self);



    self.QASAvailableClasses = ko.pureComputed(function () {
        if (self.Country() == null)
            return "col-sm-9";
        else {
            return self.Country().ThreeDigitCode == null ? "col-sm-9" : "col-sm-6";
        }
    }, self);

    self.QASAvailable = ko.pureComputed(function () {
        if (self.Country() == null)
            return false;
        else {
            return self.Country().ThreeDigitCode == null ? false : true;
        }
    }, self);

    self.toJSON = function () {
        var copy = ko.toJS(this); //easy way to get a clean copy

        delete copy.SelectListDisplayLine; //remove an extra property
        delete copy.DisplayLine;
        delete copy.SummaryDisplayLines;
        delete copy.QASAvailableClasses;
        delete copy.QASAvailable;

        return copy;
    };
}

function BlankAddress() {
    return new Address({
        Name: null,
        AddressLine1: null,
        AddressLine2: null,
        AddressLine3: null,
        AddressLine4: null,
        Country: null,
        Postcode: null,
        CountryCode: null,
        CountryID: null
    })
}

var DispatchMethod = function (dispatchMethod) {
    var self = this;

    self.Name = ko.observable(dispatchMethod.Name);
    self.HAULCODE = ko.observable(dispatchMethod.HAULCODE);
    self.CARRCODE = ko.observable(dispatchMethod.CARRCODE);

}

var Product = function (product) {
    var self = this;

    // do these need to be observables? they aren't going to change. So no.
    self.ProductID = product.ProductID;
    self.Name = product.Name;
    self.Code = product.Code;
    self.Status = product.Status;
    self.CuttingProductID = product.CuttingProductID;
    self.CuttingCode = product.CuttingCode;
    self.CSSID = product.CuttingSSID;
    self.CStock = product.CuttingStock;
    self.CuttingStatus = product.CuttingStatus;
    self.CuttingPrice = product.CuttingPrice == null ? null : product.CuttingPrice.toFixed(2);

    self.ReturnableProductID = product.ReturnableProductID;
    self.ReturnableCode = product.ReturnableCode;
    self.RSSID = product.ReturnableSSID;
    self.RStock = product.ReturnableStock;
    self.ReturnableStatus = product.ReturnableStatus;
    self.ReturnablePrice = product.ReturnablePrice == null ? null : product.ReturnablePrice.toFixed(2);

    if (viewModel.cuttingPriceOverride() != null) {
        self.CuttingPrice = viewModel.cuttingPriceOverride();
    }

    if (viewModel.returnablePriceOverride() != null) {
        self.ReturnablePrice = viewModel.returnablePriceOverride();
    }

    self.ImageID = product.ImageID;
    self.ProductPageID = product.ProductPageID;

    self.CuttingStockLine = ko.pureComputed(function () {
        switch (self.CSSID) {
            case 1:
                return self.CStock + " in stock";
            case 255:
                return "Out of stock";
            default:
                return self.CuttingCode == null ? "No cutting part number" : "No stock info";
        }
    }, self);

    self.ReturnableStockLine = ko.pureComputed(function () {
        switch (self.RSSID) {
            case 1:
                return self.RStock + " in stock";
            case 255:
                return "Out of stock";
            default:
                return self.ReturnableCode == null ? "No returnable part number" : "No stock info";
        }
    }, self);

    self.DisplayName = ko.pureComputed(function () {

        var lineArray = [];
        // if last character is ',', remove it

        if (self.Code) {
            var c = self.Code;
            lineArray.push(c);
        }
        if (self.Name) {
            var n = self.Name;
            lineArray.push(n);
        }

        return self.Code + "\xa0\xa0\xa0\xa0\xa0" + lineArray.join(', ');
    }, self);

    self.cuttingQuantityInBasket = ko.pureComputed({
        read: function () {
            return Enumerable.From(viewModel.basket()).Where(function (x) {
                var xCode = x.Code;
                var sCode = self.CuttingCode;
                if (xCode == sCode)
                    return x;
            }).ToArray().length || "";
        },
        write: function (value) {

        },
        owner: self
    }, self);

    self.returnableQuantityInBasket = ko.pureComputed({
        read: function () {
            return Enumerable.From(viewModel.basket()).Where(function (x) {
                var xCode = x.Code;
                var sCode = self.ReturnableCode;
                if (xCode == sCode)
                    return x;
            }).ToArray().length || "";
        },
        write: function (value) {

        },
        owner: self
    }, self);

    self.toJSON = function () {
        var copy = ko.toJS(this); //easy way to get a clean copy

        delete copy.CuttingStockLine; //remove an extra property
        delete copy.ReturnableStockLine;
        delete copy.DisplayName;
        delete copy.cuttingQuantityInBasket;
        delete copy.returnableQuantityInBasket;

        return copy;
    }
}

var BasketItem = function (code, type, status, price) {
    var self = this;

    self.Code = code;
    self.Type = type;
    self.Status = status;
    self.Price = ko.observable(price);
}

var BasketSummaryItem = function (code, quantityTotal, ref, status, price, unitPriceOverride) {
    var self = this;

    self.Code = code;
    self.QuantityTotal = quantityTotal;
    self.LineRef = ko.observable(ref);
    self.Status = status;
    self.Price = price;
    self.OriginalUnitPrice = price;

    self.UnitPrice = unitPriceOverride || price;

    self.UnitPriceOverride = ko.observable(unitPriceOverride).extend({ roundedNumeric: 2 });
    self.UnitPriceOverride.subscribe(function (newValue) {
        if (newValue === null) {
            self.UnitPrice = self.OriginalUnitPrice;
        }
        else {
            self.UnitPrice = newValue;
        }
    });
}

var Account = function (account) {
    var self = this;

    self.Label = account.label;
    self.Name = account.Name;
    self.AccountNumber = account.value;
    self.CountryCode = account.CountryCode;
    self.Email = ko.observable(account.Email);
    self.HAULCODE = account.HAULCODE;
    self.CARRCODE = account.CARRCODE;
    self.DispatchName = account.DispatchName;
    self.PriceList = account.PriceList;
    self.TradePriceListID = account.TradePriceListID;
    self.DefaultOrderConfirmationLanguage = account.OrderConfirmationLanguageID;

    self.defaultDispatch = ko.observable(null);

    self.loadDefaultDispatch = function () {
        if (self.HAULCODE != null && self.CARRCODE != null) {
            self.defaultDispatch = ko.observable(new DispatchMethod({
                'CARRCODE': self.CARRCODE,
                'HAULCODE': self.HAULCODE,
                'Name': self.DispatchName
            }))
        }
    };

    //self.toJSON = function () {
    //    var copy = ko.toJS(this); //easy way to get a clean copy

    //    delete copy.loadDefaultDispatch;

    //    return copy;
    //}
}

function SamplesViewModel() {
    var self = this;

    self.reset = function () {

        initProductCodeAutocomplete();

        $("#account-number").val(null);
        $("#product-code").val(null);
        $("#account-number").data("ui-autocomplete").term = null;
        $("#product-code").data("ui-autocomplete").term = null; //.data()["ui-autocomplete"].term = null;
        self.account(null);
        self.accountNumber(null);
        self.customerOrderReference(null);
        self.selectedAddress(null);
        self.selectedSecondaryAddress(null);
        self.primaryAddress(BlankAddress());
        self.secondaryAddressList([]);
        self.customAddress(BlankAddress());
        self.usingQAS(false);
        self.qasBusy(false);
        self.selectedQASResult(null);
        self.qasResults([{ moniker: "", text: "Select an Address" }]);
        self.selectedDispatchMethod(null);
        self.customerPayingForDispatch(false);
        self.productResultsList([]);
        self.basket([]);
        self.availableCountries(getAllCountries());
        self.availableCountriesWithout3D = ko.observable(getAllCountriesWithout3D());
        self.availableHauliers(getAllHauliers());
        self.allCarriages(getAllCarriages());
        self.availableLanguages(getAllLanguages());
        self.selectedOrderConfirmationLanguage(null);

        //self.orderTakerLabel(null);
        self.orderTaker(getDefaultOrderTaker()); // hmmmmmm. if one taker is placing order after order the default should be that taker? or retain whatever was set across to the next order placed... ugh.

        self.cuttingPriceOverride(null);
        self.returnablePriceOverride(null);
        self.currentBasketSummary(null);

        self.productSearchBusy(false);

        self.selectedHaulier(null);
        self.selectedCarriage(null);

        self.customerWantsOrderConfirmation(true);

        self.secondaryAddresses([]);
        self.usingQAS(false);
    };

    self.account = ko.observable(null);
    self.accountNumber = ko.observable(null);
    self.customerOrderReference = ko.observable(null);
    self.customerOrderReferenceRemainingChars = ko.pureComputed({
        read: function () {
            return (24 - (self.customerOrderReference() == null ? 0 : self.customerOrderReference().length))
        },
        owner: self
    }, self);

    self.cuttingPriceOverride = ko.observable(null).extend({ roundedNumeric: 2 });
    self.returnablePriceOverride = ko.observable(null).extend({ roundedNumeric: 2 });

    self.cuttingPriceOverride.subscribe(function (newValue) {
        $("#product-code").autocomplete("search");
    });

    self.returnablePriceOverride.subscribe(function (newValue) {
        $("#product-code").autocomplete("search");
    });

    self.selectedAddress = ko.observable(null);
    self.selectedSecondaryAddress = ko.observable(null);
    self.primaryAddress = ko.observable(BlankAddress());
    self.secondaryAddressList = ko.observable([]);
    self.customAddress = ko.observable(BlankAddress());
    self.usingQAS = ko.observable(false);
    self.qasBusy = ko.observable(false);
    self.selectedQASResult = ko.observable(null);
    self.qasResults = ko.observableArray([{ moniker: "", text: "Select an Address" }]);
    self.selectedDispatchMethod = ko.observable(null);
    self.customerPayingForDispatch = ko.observable(false);
    self.productSearchBusy = ko.observable(false);
    self.productResultsList = ko.observable([]);
    self.basket = ko.observableArray([]);
    self.availableCountries = ko.observable(getAllCountries());
    self.availableCountriesWithout3D = ko.observable(getAllCountriesWithout3D());
    self.availableHauliers = ko.observable(getAllHauliers());
    self.allCarriages = ko.observable(getAllCarriages());
    self.availableLanguages = ko.observable(getAllLanguages());
    self.selectedOrderConfirmationLanguage = ko.observable(null);

    self.customerWantsOrderConfirmation = ko.observable(false);
    self.customerWantsOrderConfirmation.subscribe(function (newValue) {
        if (newValue == false) {
            $("#email").css("background-color", "transparent");
        }
    });

    self.orderTaker = ko.observable(getDefaultOrderTaker());
    self.orderTaker.subscribe(function (newValue) {
        $("#order-taker").val(null);
        $("#address-opener").focus();
   });

    //self.orderTakerLabel = ko.observable(null);

    self.orderTakerLabel = ko.computed({
        read: function () {
            return self.orderTaker()
        }
    }, self);

    self.selectedHaulier = ko.observable(null);
    self.selectedCarriage = ko.observable(null);
    self.selectedCarriage.subscribe(function (newValue) {
        if (newValue != null) {

            var displayName = '<label style="font-weight: bold; margin-right: 8px;">Haulier:</label>';

            var haulier = Enumerable.From(getAllHauliers()).Where(function (x) {
                if (x.HAULCODE === newValue.HAULCODE)
                    return x;
            }).FirstOrDefault(null);

            if (haulier != null) {
                displayName += haulier.Name;


                var carriageService = Enumerable.From(getAllCarriages()).Where(function (x) {
                    if (x.CARRCODE === newValue.CARRCODE)
                        return x;
                }).FirstOrDefault(null);

                if (carriageService != null) {
                    displayName += '<label style="font-weight: bold; margin-left: 12px; margin-right: 8px;">Service:</label>' + (carriageService.Description || carriageService.CARRCODE);
                }
            }

            self.selectedDispatchMethod(new DispatchMethod({

                'Name': displayName,//newValue.DispatchName, // newValue.Description + ' - ' + newValue.HAULCODE + ' - ' + newValue.CARRCODE,
                'HAULCODE': newValue.HAULCODE,
                'CARRCODE': newValue.CARRCODE
            }));
        } else {
            self.selectedDispatchMethod(null);
        }
    });

    self.availableCarriages = ko.pureComputed(function () {
        if (self.selectedHaulier()) {
            return Enumerable.From(self.allCarriages()).Where(function (x) {
                if (x.HAULCODE == self.selectedHaulier().HAULCODE)
                    return x;
            }).ToArray();
        }
    }, self);

    self.account.subscribe(function (newValue) {
        if (newValue != null) {

            self.accountNumber(newValue.AccountNumber);
            self.selectedOrderConfirmationLanguage(newValue.DefaultOrderConfirmationLanguage);

            newValue.loadDefaultDispatch();
            var countries = Enumerable.From(self.availableCountries()).Where(function (x) {
                if (x.Code == newValue.CountryCode)
                    return x;
            }).ToArray();

            if (countries.length > 0) {
                self.customAddress().Country(countries[0]);
            } else {

            }

            window.setTimeout(function () {

                $("#address-collapse").on('shown.bs.collapse', function () {
                    window.setTimeout(function () {
                        $("#select-primary-address-btn").focus();
                    }, 0);
                });

                if (newValue.Email() != null) {
                    $("#address-opener").focus();
                } else {
                    $("#email").focus();
                }

                $(".white-on-focus").on('focus', function (event, ui) {
                    $(this).css("background-color", "transparent");
                });

                $(".numeric").on('focus', function (event, ui) {
                    if(isNaN($(this).val())) {
                        RedFade($(this));
                    }
                });

                $(".white-on-focusout").on('focusout', function (event, ui) {

                    if ($(this).hasClass('numeric')) {
                        if (!isNaN($(this).val())) {
                            $(this).css("background-color", "transparent");
                        }
                    } else {
                        $(this).css("background-color", "transparent");
                    }
                })

            }, 0);
        }
    })

    self.secondaryAddresses = ko.pureComputed({
        read: function () {
            return self.secondaryAddressList()
        },
        write: function (secondaryAddresses) {

            var addressItems = [];

            if (secondaryAddresses != null) {
                secondaryAddresses.forEach(function (address) {
                    addressItems.push(new Address(address));
                });
            }
            self.secondaryAddressList(addressItems);
        },
        owner: self
    }, self);

    self.blankAddress = function () {
        return new Address({
            Name: null,
            AddressLine1: null,
            AddressLine2: null,
            AddressLine3: null,
            AddressLine4: null,
            Country: null,
            Postcode: null,
            CountryID: null
        })
    }

    self.setSelectedAddress = function (address) {
        if (address != null) {
            //$("#dispatch-display-label").trigger('click');
            self.selectedAddress(address);

            self.selectDispatchMethod(self.account().defaultDispatch());
        }
    };

    self.setSecondaryAddressOptionsAfterRender = function (option, item) {
        option.title = item.DisplayLine();
    };

    self.CancelUsingQAS = ko.observable(function () {
        self.usingQAS(false);
        self.qasBusy(false);
    });

    self.initPCAAutocomplete = function () {
        $("#pca-find").autocomplete({
            appendTo: '#pca-find-wrapper',
            source: function (request, response) {

                $.ajax({
                    url: "Samples/PCAFind",
                    type: "POST",
                    dataType: "json",
                    data: {
                        id: self.customAddress().PCASearchId(),
                        text: self.customAddress().PCASearchTerm(),
                        container: self.customAddress().PCAContainer(),
                        origin: null,
                        countries: self.customAddress().Country().Code,
                    },
                    success: function (data) {
                        $("#pca-find").removeClass('ui-autocomplete-loading');
                        if (self.customAddress().PCASearchId() != null) {
                            if (data.length == 1) {
                                var county = data[0].Province || data[0].ProvinceName || data[0].ProvinceCode || data[0].AdminAreaName || data[0].AdminAreaCode;
                                if (data[0].City === "London" && self.customAddress().Country().Code == "GB")
                                    county = "London";
                                var line1 = data[0].Line1;
                                if (data[0].Company != "") {
                                    line1 = data[0].Company + ", " + line1;
                                }
                                self.customAddress(new Address({
                                    Name: self.customAddress().Name(),
                                    AddressLine1: line1,
                                    AddressLine2: data[0].Line2,
                                    AddressLine3: data[0].City,
                                    AddressLine4: county,
                                    Postcode: data[0].PostalCode,
                                    Country: self.customAddress().Country()
                                }));
                                $("#pca-find").autocomplete("close");

                                self.customAddress().PCASearchId(null);
                                self.customAddress().PCAContainer(null);
                                return false;
                            } else {
                                response($.map(data, function (item) {
                                    var county = item.Province || item.ProvinceName || item.ProvinceCode || item.AdminAreaName || item.AdminAreaCode;
                                    if (item.City === "London" && self.customAddress().Country().Code == "GB")
                                        county = "London";
                                    var line1 = data[0].Line1;
                                    if (item.Company != "") {
                                        line1 = data[0].Company + ", " + line1;
                                    }
                                    return {
                                        customCatch: true,
                                        label: item.Label,
                                        value: item.Id,
                                        addressLine1: line1,
                                        addressLine2: item.Line2,
                                        addressLine3: item.City,
                                        addressLine4: county,
                                        Postcode: item.PostalCode
                                    }
                                }));
                                return false;
                            }
                        }

                        self.customAddress().PCASearchId(null);
                        self.customAddress().PCAContainer(null);

                        response($.map(data, function (item) {
                            if (item.hasOwnProperty('Type')) {
                                if ((item.Type === "Address") || (item.Type === "Residential")) {
                                    return {
                                        id: item.Id,
                                        label: item.Text + ", " + item.Description,
                                        value: item.Id,
                                        type: item.Type,
                                        text: item.Text
                                        // highlight: item.Highlight
                                    };
                                } else {
                                    return {
                                        id: item.Id,
                                        label: item.Description,
                                        value: item.Id,
                                        type: item.Type,
                                        text: item.Text
                                        // highlight: item.Highlight
                                    };
                                }
                            } else {
                                return {
                                    label: item.Label,
                                    value: item.Id
                                }
                            }
                        }));
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        $("#pca-find").removeClass('ui-autocomplete-loading');
                        $.showError(xhr, textStatus, errorThrown);
                    }
                });
            },
            minLength: 1,
            select: function (event, ui) {
                if (ui.item.hasOwnProperty('type')) {
                    switch (ui.item.type) {
                        case 'Address':
                        case 'Residential':
                            jQuery.extend(event.originalEvent, { keepOpen: true });
                            self.customAddress().PCASearchId(ui.item.id);
                            self.customAddress().PCAContainer(null);
                            //$("#pca-find").autocomplete('search');
                            break;
                        default:
                            jQuery.extend(event.originalEvent, { keepOpen: true });
                            //modify value as required
                            self.customAddress().PCASearchId(null);
                            self.customAddress().PCAContainer(ui.item.id);
                            //$("#pca-find").autocomplete('search');
                            return false;
                    }
                } else if (ui.item.hasOwnProperty('customCatch')) {

                    self.customAddress(new Address({
                        Name: self.customAddress().Name(),
                        AddressLine1: ui.item.addressLine1,
                        AddressLine2: ui.item.addressLine2,
                        AddressLine3: ui.item.addressLine3,
                        AddressLine4: ui.item.addressLine4,
                        Postcode: ui.item.Postcode,
                        Country: self.customAddress().Country()
                    }));
                    $("#pca-find").autocomplete("close");
                    self.customAddress().PCASearchId(null);
                    self.customAddress().PCAContainer(null);
                }

                return false;
            },
            focus: function (event, ui) {
                event.preventDefault();
            },
            autoFocus: true
        });
    }

    self.selectedQASResult.subscribe(function (newValue) {
        if (newValue != null)
            selectQASAddress(newValue);
    });

    self.selectQASResult = ko.observable(function () {
        selectQASAddress(self.selectedQASResult());
    });
    self.verifyPrimaryAddress = ko.observable(function () {
        var valid = true;
        // Verify Name, Address Line 1, Country & Postcode exist.
        // That is all.
        if (self.primaryAddress().Name() == null ||
            self.primaryAddress().Name() == "") {
            RedFade($("#primary-name"));
            valid = false;
        }

        if (self.primaryAddress().AddressLine1() == null ||
            self.primaryAddress().AddressLine1() == "") {
            RedFade($("#primary-line-1"));
            valid = false;
        }

        if (self.primaryAddress().Country() == null ||
            self.primaryAddress().Country() == "") {
            RedFade($("#primary-country"));
            valid = false;
        }

        if (self.primaryAddress().Postcode() == null ||
            self.primaryAddress().Postcode() == "") {
            RedFade($("#primary-postcode"));
            valid = false;
        }

        if (valid) {
            self.setSelectedAddress(new Address({
                Name: viewModel.primaryAddress().Name(),
                AddressLine1: viewModel.primaryAddress().AddressLine1(),
                AddressLine2: viewModel.primaryAddress().AddressLine2(),
                AddressLine3: viewModel.primaryAddress().AddressLine3(),
                AddressLine4: viewModel.primaryAddress().AddressLine4(),
                Country: viewModel.primaryAddress().Country().Name,
                Postcode: viewModel.primaryAddress().Postcode(),
                CountryCode: viewModel.primaryAddress().Country().Code,
                CountryID: viewModel.primaryAddress().Country().CountryID
            }));
        }
    });

    self.verifyCustomAddress = ko.observable(function () {

        var valid = true;
        // Verify Name, Address Line 1, Country & Postcode exist.
        // That is all.
        if (self.customAddress().Name() == null ||
            self.customAddress().Name() == "") {
            RedFade($("#custom-name"));
            valid = false;
        }

        if (self.customAddress().AddressLine1() == null ||
            self.customAddress().AddressLine1() == "") {
            RedFade($("#custom-line-1"));
            valid = false;
        }

        if (self.customAddress().Country() == null ||
            self.customAddress().Country() == "") {
            RedFade($("#custom-country"));
            valid = false;
        }

        if (self.customAddress().Postcode() == null ||
            self.customAddress().Postcode() == "") {
            RedFade($("#custom-postcode"));
            valid = false;
        }

        if (valid) {
            self.setSelectedAddress(new Address({
                Name: viewModel.customAddress().Name(),
                AddressLine1: viewModel.customAddress().AddressLine1(),
                AddressLine2: viewModel.customAddress().AddressLine2(),
                AddressLine3: viewModel.customAddress().AddressLine3(),
                AddressLine4: viewModel.customAddress().AddressLine4(),
                Country: viewModel.customAddress().Country().Name,
                Postcode: viewModel.customAddress().Postcode(),
                CountryCode: viewModel.customAddress().Country().Code,
                CountryID: viewModel.customAddress().Country().CountryID
            }));
        }
    });

    self.selectDispatchMethod = ko.pureComputed({
        read: function () {
            return self.selectedDispatchMethod();
        },
        write: function (dispatchMethod) {
            if (dispatchMethod != null) {
                self.selectedDispatchMethod(new DispatchMethod(dispatchMethod));
                window.setTimeout(function () {
                    $("#product-display-label").trigger("click");
                    window.setTimeout(function () {
                        $("#product-code").focus();
                    }, 0);
                }, 0);
            } else if (self.account().defaultDispatch() != null) {
                self.selectedDispatchMethod(self.account().defaultDispatch());
                $("#product-display-label").trigger("click");
                window.setTimeout(function () {
                    $("#product-code").focus();
                }, 0);
            } else {
                //noop
            }
        },
        owner: self
    }, self);

    self.productResults = ko.pureComputed({
        read: function () {
            return self.productResultsList()
        },
        write: function (productResults) {

            var productItems = [];

            if (productResults != null) {
                productResults.forEach(function (product) {
                    productItems.push(new Product(product));
                });
            }
            self.productResultsList(productItems);
        },
        owner: self
    }, self);

    self.statusTitle = function (status) {
        switch (status) {
            case "C":
                return "Current";
                break;
            case "L":
                return "Limited";
                break;
            case "Y":
                return "Pre-Limited";
                break;
            case "D":
                return "Discontinued";
                break;
            default:
                return "Unrecognised!";
        }
    }

    self.loadLargeImage = function (result) {
        if (result.ImageID != null) {
            $('#large-image-container').html('');
            var url = 'https://www.designersguild.com/image/488/' + result.ImageID;
            $('#large-image-container').html('<img src="' + url + '"/>');
            $('#large-image-title').text(result.Code);
            $("#image-modal").modal('show');
        } else {
            $('#large-image-container').html('');
            $('#large-image-container').html('<img src="/content/images/not-found-large.png"/>');
            $('#large-image-title').text(result.Code);
            $("#image-modal").modal('show');
        }
    }

    //self.basket.subscribe(function () {
    //    if (self.basket().length == 0) {
    //        if ($("#basket-collapse").is(":visible"))
    //            $("#basket-collapse").collapse('hide');
    //    }
    //});

    self.basketItemDisplay = ko.pureComputed(function () {
        if (self.basket().length == 1)
            return '1 item'
        else
            return self.basket().length + ' items'
    }, self);

    self.removeCodeFromBasket = function (code) {
        if (code) {
            self.basket.remove(function (item) {
                return item.Code == code;
            });
        }
    };

    self.increaseCodeInBasket = function (code, status, price) {
        if (code) {

            // price being passed from the summary add buttons is the price listed on the SUMMARY ITEM.
            // which is why it doubles itself whenever you click on it.

            // to get around that... what do we need to do...
            // we need to see if the basket already contains this line and if it does use THAT price not the price sent to this function

            var extant = Enumerable.From(self.basket()).Where(function (x) {
                if (x.Code == code)
                    return x;
            }).FirstOrDefault(null)

            if (extant == null) {
                self.basket.push(new BasketItem(code, code.slice(-1), status, price));
            } else {
                self.basket.push(new BasketItem(code, code.slice(-1), status, extant.Price()));
            }
        }
    };

    self.decreaseCodeInBasket = function (code) {
        if (code) {
            var toRemove = Enumerable.From(self.basket()).Where(function (x) {
                if (x.Code == code)
                    return x;
            }).FirstOrDefault(null)
            if (toRemove)
                self.basket.remove(toRemove);
        }
    };

    self.addAllCuttingsToBasket = function () {
        self.productResultsList().forEach(function (item) {
            if (item.CuttingCode != null && item.CuttingPrice != null)
                self.increaseCodeInBasket(item.CuttingCode, item.Status, item.CuttingPrice);
        });
    };

    self.removeAllCuttingsFromBasket = function () {
        self.productResultsList().forEach(function (item) {
            self.decreaseCodeInBasket(item.CuttingCode);
        });
    };

    self.addAllReturnablesToBasket = function () {
        self.productResultsList().forEach(function (item) {
            if (item.ReturnableCode != null && item.ReturnablePrice != null)
                self.increaseCodeInBasket(item.ReturnableCode, item.Status, item.ReturnablePrice);
        });
    };

    self.removeAllReturnablesFromBasket = function () {
        self.productResultsList().forEach(function (item) {
            self.decreaseCodeInBasket(item.ReturnableCode);
        });
    };

    // Issue: the level at which we'd like to place line notes is currently the result of this pureComputed.
    // basketSummary returns an array... of JSON objects... as composed by this .Select operation.
    // so we can define an observable and tie it to an input for each basket line but as soon as this computable
    // is reevaluated the null returns.
    // so... preserve the running basketSummary instead of rebuilding it every time an action happens to the basket.
    // ah, christ...
    // hmmmmmm.

    self.currentBasketSummary = ko.observable(null);

    self.basketSummary = ko.computed({
        read: function () {

            var summary = Enumerable.From(self.basket())
                .GroupBy(function (item) { return item.Code; })
                .Select(function (item) {

                    var lineRef = self.customerOrderReference();

                    var existing = Enumerable.From(self.currentBasketSummary())
                                             .Where(function (x) {
                                                 if (x.Code == item.source[0].Code)
                                                     return x;
                                             }).FirstOrDefault(null);

                    var price = 0;
                    var unitPriceOverride = null;

                    if (existing != null) {
                        lineRef = existing.LineRef();
                        unitPriceOverride = existing.UnitPriceOverride();
                    }

                    for (var i = 0; i < item.source.length; i++) {

                        if (unitPriceOverride != null) {
                            price += Number(unitPriceOverride);

                            item.source[i].Price(Number(unitPriceOverride));
                        } else {

                            // this now poses a problem as we lose track of what the price was before mucking about with it in the basket summary
                            price += Number(item.source[i].Price());
                        }
                    }

                    // Great. Now the information needs to be written back to the basket items themselves.

                    return new BasketSummaryItem(item.source[0].Code, item.source.length, lineRef, item.source[0].Status, price.toFixed(2), unitPriceOverride)
                })
                .OrderBy(function (item) { return item.Code; })
                .ToArray();

            self.currentBasketSummary(summary);

            return summary;
        },
        write: function () {

        },
        owner: self
    }, self);

    self.cuttingTotalQuantity = ko.computed({
        read: function () {
            return Enumerable.From(self.basket()).Where(function (x) {
                if (x.Type == 'C')
                    return x;
            }).ToArray().length;
        },
        write: function () {

        },
        owner: self
    }, self)

    self.cuttingTotalPrice = ko.computed({
        read: function () {
            return Enumerable.From(self.basket()).Where(function (x) {
                if (x.Type == 'C')
                    return x;
            }).Select("Number($.Price())").Sum().toFixed(2);
        },
        write: function () {

        },
        owner: self
    }, self);

    self.returnableTotalQuantity = ko.computed({
        read: function () {
            return Enumerable.From(self.basket()).Where(function (x) {
                if (x.Type == 'R')
                    return x;
            }).ToArray().length;
        },
        write: function () {

        },
        owner: self
    }, self)

    self.returnableTotalPrice = ko.computed({
        read: function () {
            return Enumerable.From(self.basket()).Where(function (x) {
                if (x.Type == 'R')
                    return x;
            }).Select("Number($.Price())").Sum().toFixed(2);
        },
        write: function () {

        },
        owner: self
    }, self);

    self.totalQuantity = ko.pureComputed({
        read: function () {
            return Number(self.cuttingTotalQuantity()) + Number(self.returnableTotalQuantity());
        },
        write: function () {

        },
        owner: self
    }, self);

    self.totalPrice = ko.pureComputed({
        read: function () {

            return Enumerable.From(self.currentBasketSummary()).Where(function (x) { return x; }).Select("Number($.Price)").Sum().toFixed(2);

            //return (Number(self.cuttingTotalPrice()) + Number(self.returnableTotalPrice())).toFixed(2)

        },
        write: function () {

        },
        owner: self
    }, self);

    self.placeOrderBusy = ko.observable(false);

    self.placeOrder = function () {

        if (self.customerWantsOrderConfirmation() && (self.account().Email() == "" || self.account().Email() == null)) {
            $("#order-summary").modal("hide");
            if ($("#account-collapse").hasClass("in")) { }
            else {
                $("#account-name-label").trigger("click");
            }
            RedFade($("#email"));
        } else {
            self.placeOrderBusy(true);
            $.ajax({
                url: '/Samples/PlaceOrder',
                type: 'POST',
                dataType: 'json',
                cache: false,
                data: {
                    order: ko.toJSON(viewModel)
                },
                success: function (data) {

                    self.placeOrderBusy(false);

                    if (data.state == "success") {
                        self.reset();

                        window.setTimeout(function () {
                            $("#account-name-label").trigger("click");
                            window.setTimeout(function () {
                                $("#account-number").focus();
                                $("#new-order-number").text(data.message);
                                $("#summary-modal").modal('hide');
                                $("#success-modal").modal('show');
                                window.setTimeout(function () {
                                    $("#success-close").focus();
                                }, 200)
                            }, 0);
                        }, 0);
                    }

                    if (data.state == "needLogin") {
                        $("#failure-title").text('Warning');
                        $("#failure-status").text('Missing WebTex Login');
                        $("#failure-modal-error-text").html(data.message);
                        $("#failure-modal-iframe-wrapper").empty();
                        $("#failure-modal").modal('show');
                    }

                },
                error: function (xhr, e, s) {

                    self.placeOrderBusy(false);

                    $("#failure-status").text('Order Not Placed');
                    $("#failure-modal-error-text").text(s);
                    $("#failure-title").text('Error');
                    var $frame = $('<iframe>');
                    $("#failure-modal-iframe-wrapper").html($frame);
                    setTimeout(function () {
                        var doc = $frame[0].contentWindow.document;
                        var $body = $('body', doc);
                        $body.html(xhr.responseText);
                    }, 1);

                    $("#failure-modal").modal('show');
                }
            });
        }
    }

    self.toJSON = function () {
        var copy = ko.toJS(this);

        // for testing errors
        //delete copy.account;

        delete copy.primaryAddress;
        delete copy.productResultsList;
        delete copy.productResults;
        delete copy.availableLanguages;
        delete copy.availableCountries;
        delete copy.availableCountriesWithout3D;
        delete copy.availableHauliers;
        delete copy.allCarriages;
        delete copy.availableCarriages;
        delete copy.secondaryAddress;
        delete copy.selectedSecondaryAddress;
        delete copy.secondaryAddresses;
        delete copy.secondaryAddressList;
        delete copy.customAddress;
        delete copy.blankAddress;
        delete copy.setSelectedAddress;
        delete copy.setSecondaryAddressOptionsAfterRender;
        delete copy.CancelUsingQAS;
        delete copy.usingQAS;
        delete copy.qasBusy;
        delete copy.pcaFind;
        delete copy.qasResults;
        delete copy.selectedQASResult;
        delete copy.selectQASResult;
        delete copy.verifyPrimaryAddress;
        delete copy.verifyCustomAddress;
        delete copy.selectDispatchMethod;
        delete copy.productResults;
        delete copy.basketItemDisplay;
        delete copy.removeCodeFromBasket;
        delete copy.increaseCodeInBasket;
        delete copy.decreaseCodeInBasket;
        delete copy.addAllCuttingsToBasket;
        delete copy.removeAllCuttingsFromBasket;
        delete copy.addAllReturnablesToBasket;
        delete copy.removeAllReturnablesFromBasket;
        delete copy.basket;
        delete copy.statusTitle;
        delete copy.customerOrderReferenceRemainingChars;
        delete copy.cuttingPriceOverride;
        delete copy.returnablePriceOverride;
        delete copy.productSearchBusy;
        delete copy.currentBasketSummary;
        delete copy.cuttingTotalPrice;
        delete copy.returnableTotalPrice;
        delete copy.totalQuantity;
        delete copy.totalPrice;
        delete copy.placeOrderBusy;

        copy.basket = copy.basketSummary;

        delete copy.basketSummary;
        delete copy.cuttingTotalQuantity;
        delete copy.returnableTotalQuantity;
        delete copy.placeOrder;
        delete copy.selectedCarriage;

        return copy;
    }

}

var viewModel = null;

$("#dispatch-collapse").on('shown.bs.collapse', function () {
    if ($("#dispatch-select-button").length)
        window.setTimeout(function () {
            $("#dispatch-select-button").focus();
        }, 0);
    else window.setTimeout(function () {
        $("#dispatch-code").focus();
    }, 0);
});

$("#product-collapse").on('shown.bs.collapse', function () {
    window.setTimeout(function () {
        $("#product-code").focus();
    }, 0);
});

function selectQASAddress(address) {
    $.ajax({
        url: "/Samples/addressLookupDetail",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(address),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data.hasOwnProperty('error')) {
                //$("#error-display").show();
                //$("#error-text").text(data.error);
                //alert(data.error);
                console.log(data.errorEx);
            } else {
                selectFoundAddress(data);
            }

        },
        error: function (xhr, textStatus, errorThrown) {
            $.showError(xhr, textStatus, errorThrown);
        }
    });
}

// This is slightly insufficient... what more can I do?!
// Don't know how much data I'm getting back from QAS, don't have more than five fields... oh well.
function selectFoundAddress(data) {

    var unlabelled = Enumerable.From(data).Where(function (x) {
        if (x.label == "")
            return x;
    }).ToArray();

    if (unlabelled.length > 0) {
        viewModel.customAddress().AddressLine1(data[0].data);
    }
    if (unlabelled.length > 1) {
        viewModel.customAddress().AddressLine2(data[1].data);
    }

    var town = Enumerable.From(data).Where(function (x) {
        if (x.label == "Town" || x.label == "City name")
            return x
    }).Select(function (x) {
        return x.data;
    }).FirstOrDefault()

    if (town) {
        viewModel.customAddress().AddressLine3(town);
    }

    var county = Enumerable.From(data).Where(function (x) {
        if (x.label == "County" || x.label == "State code")
            return x;
    }).Select(function (x) {
        return x.data;
    }).FirstOrDefault();

    if (county)
        viewModel.customAddress().AddressLine4(county);

    var postcode = Enumerable.From(data).Where(function (x) {
        if (x.label == "Postcode")
            return x;
    }).Select(function (x) {
        return x.data;
    }).FirstOrDefault();

    if (postcode) {
        viewModel.customAddress().Postcode(postcode);
    } else {
        viewModel.customAddress().Postcode(data[5].data)
    }
}

function fireTestConfiguration() {

    $("#account-number").val('K0099');

    var downKeyEvent = $.Event("keydown");
    downKeyEvent.keyCode = $.ui.keyCode.DOWN;  // event for pressing "down" key

    var enterKeyEvent = $.Event("keydown");
    enterKeyEvent.keyCode = $.ui.keyCode.ENTER;  // event for pressing "enter" key

    $("#account-number").trigger(downKeyEvent);  // First downkey invokes search

    window.setTimeout(function () {
        //$("#account-number").trigger(downKeyEvent);  // Second downkey highlights first item
        $("#account-number").trigger(enterKeyEvent); // Enter key selects highlighted item

        window.setTimeout(function () {

            $("#address-display-label").trigger("click");

            window.setTimeout(function () {

                $("#select-primary-address-btn").trigger("click");

                window.setTimeout(function () {

                    //viewModel.selectDispatchMethod(viewModel.account().defaultDispatch());

                    //window.setTimeout(function () {

                        $("#product-code").focus();

                    //}, 0);

                }, 500);

            }, 500);

        }, 500);

    }, 500);

}


$("#account-number").autocomplete({
    classes: {
        'account-no': 'account-no'
    },
    appendTo: '#account-selector',
    source: function (request, response) {
        $.ajax({
            url: "/Samples/AccountNumberAutocomplete",
            type: "POST",
            dataType: "json",
            data: {
                term: request.term
            },
            success: function (data) {
                response(data);
            },
            error: function (xhr, textStatus, errorThrown) {
                $.showError(xhr, textStatus, errorThrown);
            }
        });
    },
    minLength: 2,
    select: function (event, ui) {
        viewModel.reset();
        testForWebtexLogin(ui.item);
    },
    autoFocus: true,
});

function testForWebtexLogin(item) {

    viewModel.placeOrderBusy(true);

    $.ajax({
        url: '/Samples/TestMertex',
        type: 'POST',
        dataType: 'json',
        cache: false,
        data: {
            accountNumber: item.value
        },
        success: function (data) {

            viewModel.placeOrderBusy(false);

            if (data.state == "needLogin") {
                $("#failure-title").text('Warning');
                $("#failure-status").text('Missing or Incorrect WebTex Login');
                $("#failure-modal-error-text").html(data.message);
                $("#failure-modal-iframe-wrapper").empty();
                $("#failure-modal").modal('show');
            }
            else {

                viewModel.account(new Account(item));

                window.setTimeout(function () {
                    initOrderTakerAutocomplete();
                    viewModel.initPCAAutocomplete();
                }, 0);

                $.ajax({
                    url: "/Samples/FetchAddresses",
                    type: "POST",
                    dataType: "json",
                    data: {
                        accountNumber: item.value
                    },
                    success: function (data) {
                        if (data) {
                            if (data.PrimaryAddresses) {
                                if (data.PrimaryAddresses.length > 0) {
                                    viewModel.primaryAddress(new Address(data.PrimaryAddresses[0]));

                                    var countries = Enumerable.From(viewModel.availableCountriesWithout3D()).Where(function (x) {
                                        if (x.Code == data.PrimaryAddresses[0].CountryCode)
                                            return x;
                                    }).ToArray();

                                    if (countries.length > 0) {
                                        viewModel.primaryAddress().Country(countries[0]);
                                    } else {

                                    }
                                } else {
                                    viewModel.primaryAddress(null);
                                }
                            }

                            if (data.SecondaryAddresses) {
                                if (data.SecondaryAddresses.length > 0) {
                                    viewModel.secondaryAddresses(data.SecondaryAddresses);
                                } else {
                                    viewModel.secondaryAddresses([]);
                                }
                            }
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        $.showError(xhr, textStatus, errorThrown);
                    }
                })
            }
        },
        error: function (xhr, e, s) {

            viewModel.placeOrderBusy(false);

            $("#failure-modal-error-text").text(s);
            $("#failure-title").text('Error');
            var $frame = $('<iframe>');
            $("#failure-modal-iframe-wrapper").html($frame);
            setTimeout(function () {
                var doc = $frame[0].contentWindow.document;
                var $body = $('body', doc);
                $body.html(xhr.responseText);
            }, 1);

            $("#failure-modal").modal('show');
        }
    });
}

function initProductCodeAutocomplete() {
    $("#product-code").autocomplete({
        source: function (request, response) {
            if (!viewModel.productSearchBusy()) {
                viewModel.productSearchBusy(true);
                $.ajax({
                    url: "/Samples/ProductCodeAutocomplete",
                    type: "POST",
                    dataType: "json",
                    cache: false,
                    data: {
                        tradePriceListId: viewModel.account().TradePriceListID,
                        term: request.term
                    },
                    success: function (data) {
                        $("#product-code").removeClass('ui-autocomplete-loading');
                        viewModel.productResults(data);
                        viewModel.productSearchBusy(false);
                        //response(data);
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        viewModel.productSearchBusy(false);
                        $.showError(xhr, textStatus, errorThrown);
                    }
                });
            }
        },
        delay: 500,
        minLength: 4,
        select: function (event, ui) {
            //console.log(ui.item);
        }
    });


    $("#product-code").keypress(function (e) {
        if (e.which == 13) {
            $("#product-code").autocomplete("search");
        }
    });
    //$("#product-code").on("focusout", function () { $("#product-code").autocomplete("search"); });
}

function initOrderTakerAutocomplete() {

    $("#order-taker").autocomplete({
        classes: {
            'account-no': 'account-no'
        },
        appendTo: '#order-taker-selector',
        source: function (request, response) {
            $.ajax({
                url: "/Samples/OperatorAutocomplete",
                type: "POST",
                dataType: "json",
                data: {
                    term: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Code + " - " + item.Name,
                            value: item.Code
                        };
                    }));
                },
                error: function (xhr, textStatus, errorThrown) {
                    $.showError(xhr, textStatus, errorThrown);
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            $("#order-taker").val(null);
            viewModel.orderTaker(ui.item.value);
            return false;
        },
        autoFocus: true,
    });

    viewModel.orderTaker(getDefaultOrderTaker());
}