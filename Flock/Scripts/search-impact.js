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
        ko.unwrap(value) ? $(element).fadeIn('fast') : $(element).fadeOut('fast');
    }
};

var searchImpactViewModel = function () {
    var self = this;

    self.availableProductTypes = ko.observableArray(getAvailableProductTypes());
    self.availableLaunches = ko.observableArray(getAvailableLaunches());

    self.searchResults = ko.observableArray([]);

    self.selectedProductTypeID = ko.observable(null);
    self.selectedProductTypeID.subscribe(function (newValue) {
        if (self.selectedLaunchID() != null) {
            self.performSearch();
        }
    });

    self.selectedLaunchID = ko.observable(null);
    self.selectedLaunchID.subscribe(function (newValue) {
        if (self.selectedProductTypeID() != null) {
            self.performSearch();
        }
    });

    self.updatingProductPageIDs = ko.observableArray([]);

    self.busy = ko.observable(false);

    self.reset = function () {
        if (!self.busy()) {
            $.confirm(function() { 
                viewModel.busy(true);
                $.ajax({
                    url: '/admin/searchimpact/reset',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify({
                        objReset: viewModel.toJSON()
                    }),
                    success: function (data) {
                        viewModel.busy(false);
                        self.searchResults(data.results);
                        self.initialiseMultiselectDragDrop();
                    },
                    error: function (xhr, e, s) {
                        viewModel.busy(false);
                        $.showError(xhr, e, s);
                    }
                });
            }, null, null);
        }
    }

    self.performSearch = function () {
        if (!self.busy()) {
            self.busy(true);

            var objSearch = {};
            objSearch.selectedProductTypeID = 1;
            objSearch.selectedLaunchID = 122;

            $.ajax({
                url: '/admin/searchimpact/performsearch',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({
                    objSearch: viewModel.toJSON()
                }),
                success: function (data) {
                    self.busy(false);
                    self.searchResults(data.results);
                    self.initialiseMultiselectDragDrop();
                },
                error: function (xhr, e, s) {
                    self.busy(false);
                    $.showError(xhr, e, s);
                }
            });
        }
    }

    self.performUpdate = function () {
        if (!self.busy()) {
            self.busy(true);

            $("#search-results-container").find(".col-xs-4").each(function (i) {
                var thisProductPageID = $(this).data('product-page-id');
                var t = Enumerable.From(self.searchResults()).Where(function (x) {
                    if (x.ProductPageID == thisProductPageID) {
                        return x;
                    }
                }).Single();
                t.SortOrder = i + 1;
            });

            $.ajax({
                url: '/admin/searchimpact/update',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                data: JSON.stringify({
                    objUpdate: viewModel.searchResults(),
                    selectedProductTypeID: viewModel.selectedProductTypeID(),
                    selectedLaunchID: viewModel.selectedLaunchID()
                }),
                success: function (data) {
                    self.busy(false);
                    self.searchResults(data.results);
                    self.initialiseMultiselectDragDrop();
                },
                error: function (xhr, e, s) {
                    self.busy(false);
                    $.showError(xhr, e, s);
                }
            });
        }
    }

    self.initialiseMultiselectDragDrop = function () {
        $("#search-results-container").multisortable({
            placeholder: 'col-xs-4 placeholder',
            items: 'div.col-xs-4',
            tolerance: "pointer",
            distance: 15
        });
    }

    self.debugStart = function () {
        self.selectedLaunchID(126);
        self.selectedProductTypeID(45);
    }

    self.toJSON = function () {
        var copy = ko.toJS(this);

        delete copy.initialise;
        delete copy.performUpdate;
        delete copy.searchResults;
        delete copy.initialiseMultiselectDragDrop();

        delete copy.availableProductTypes;
        delete copy.availableLaunches;
        delete copy.busy;
        delete copy.performSearch;

        return copy;
    }
}