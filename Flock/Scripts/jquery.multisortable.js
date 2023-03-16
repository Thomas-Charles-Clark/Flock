/**
* jquery.multisortable.js - v0.2
* https://github.com/shvetsgroup/jquery.multisortable
*
* Author: Ethan Atlakson, Jay Hayes, Gabriel Such, Alexander Shvets
* Last Revision 3/16/2012
* multi-selectable, multi-sortable jQuery plugin
*/
!function ($) {
    $.fn.multiselectable = function (options) {
        if (!options) {
            options = {}
        }
        options = $.extend({}, $.fn.multiselectable.defaults, options);
        function mouseDown(e) {
            var item = $(this),
            parent = item.parent(),
            myIndex = item.index();
            var prev = parent.find('.multiselectable-previous');
            // If no previous selection found, start selecting from first selected item.
            prev = prev.length ? prev : $(parent.find('.' + options.selectedClass)[0]).addClass('multiselectable-previous');
            var prevIndex = prev.index();
            if (e.ctrlKey || e.metaKey) {
                if (item.hasClass(options.selectedClass)) {
                    item.removeClass(options.selectedClass).removeClass('multiselectable-previous')
                    if (item.not('.child').length) {
                        item.nextUntil(':not(.child)').removeClass(options.selectedClass);
                    }
                }
                else {
                    parent.find('.multiselectable-previous').removeClass('multiselectable-previous');
                    item.addClass(options.selectedClass).addClass('multiselectable-previous')
                    if (item.not('.child').length) {
                        item.nextUntil(':not(.child)').addClass(options.selectedClass);
                    }
                }
            }
            if (e.shiftKey) {
                var last_shift_range = parent.find('.multiselectable-shift');
                last_shift_range.removeClass(options.selectedClass).removeClass('multiselectable-shift');
                var shift_range;
                if (prevIndex < myIndex) {
                    shift_range = item.prevUntil('.multiselectable-previous').add(prev).add(item);
                }
                else if (prevIndex > myIndex) {
                    shift_range = item.nextUntil('.multiselectable-previous').add(prev).add(item);
                }
                shift_range.addClass(options.selectedClass).addClass('multiselectable-shift');
            }
            else {
                parent.find('.multiselectable-shift').removeClass('multiselectable-shift');
            }
            if (!e.ctrlKey && !e.metaKey && !e.shiftKey) {
                parent.find('.multiselectable-previous').removeClass('multiselectable-previous');
                if (!item.hasClass(options.selectedClass)) {
                    parent.find('.' + options.selectedClass).removeClass(options.selectedClass);
                    item.addClass(options.selectedClass).addClass('multiselectable-previous');
                    if (item.not('.child').length) {
                        item.nextUntil(':not(.child)').addClass(options.selectedClass);
                    }
                }
            }
            options.mousedown(e, item);
        }
        function click(e) {
            if ($(this).is('.ui-draggable-dragging')) {
                return;
            }
            var item = $(this), parent = item.parent();
            // If item wasn't draged and is not multiselected, it should reset selection for other items.
            if (!e.ctrlKey && !e.metaKey && !e.shiftKey) {
                parent.find('.multiselectable-previous').removeClass('multiselectable-previous');
                parent.find('.' + options.selectedClass).removeClass(options.selectedClass);
                item.addClass(options.selectedClass).addClass('multiselectable-previous');
                if (item.not('.child').length) {
                    item.nextUntil(':not(.child)').addClass(options.selectedClass);
                }
            }
            options.click(e, item);
        }
        return this.each(function () {
            var list = $(this);
            if (!list.data('multiselectable')) {
                list.data('multiselectable', true)
                .delegate(options.items, 'mousedown', mouseDown)
                .delegate(options.items, 'click', click)
                .disableSelection();
            }
        })
    };
    $.fn.multiselectable.defaults = {
        click: function (event, elem) { },
        mousedown: function (event, elem) { },
        selectedClass: 'multiselectable-selected',
        items: 'li'
    };
    $.fn.multisortable = function (options) {
        if (!options) {
            options = {}
        }
        var settings = $.extend({}, $.fn.multisortable.defaults, options);
        function regroup(item, list) {
            console.log("regroup hit");
            var now = Date.now();
            console.log("timer started: " + now);
            if (list.find('.' + settings.selectedClass).length > 0) {
                var myIndex = item.data('i');
                var itemsBefore = list.find('.' + settings.selectedClass).filter(function () {
                    return $(this).data('i') < myIndex
                }).css({
                    position: '',
                    width: '',
                    left: '',
                    top: '',
                    zIndex: ''
                });
                item.before(itemsBefore);
                var itemsAfter = list.find('.' + settings.selectedClass).filter(function () {
                    return $(this).data('i') > myIndex
                }).css({
                    position: '',
                    width: '',
                    left: '',
                    top: '',
                    zIndex: ''
                });
                item.after(itemsAfter);
                setTimeout(function () {
                    itemsAfter.add(itemsBefore).addClass(settings.selectedClass);
                }, 0);
            }
            console.log("regroup finished");
            console.log("time elapsed: " + (Date.now() - now));
        }
        return this.each(function () {
            var list = $(this);
            //enable multi-selection
            list.multiselectable({
                selectedClass: settings.selectedClass,
                click: settings.click,
                items: settings.items,
                mousedown: settings.mousedown
            });
            //enable sorting
            options.distance = settings.distance;
            options.tolerance = settings.tolerance;
            options.cancel = settings.items + ':not(.' + settings.selectedClass + ')';
            options.placeholder = settings.placeholder;
            options.start = function (event, ui) {

                if (ui.item.hasClass(settings.selectedClass)) {

                    var parent = ui.item.parent();

                    //assign indexes to all selected items
                    parent.find('.' + settings.selectedClass).each(function (i) {
                        $(this).data('i', i);
                        //$(this).css("opacity", "0.3");
                        $(this).data("original", $(this).position());
                    });
                    // adjust placeholder size to be size of items
                    // this was bollocks, leave it out.
                    var height = (parent.find('.' + settings.selectedClass).length) * (ui.item.outerWidth() + 11);
                    var width = (parent.find('.' + settings.selectedClass).length - 2) * (ui.item.outerHeight() + 11);

                    //ui.placeholder.height(width);
                    //ui.placeholder.width (height)
                }
                settings.start(event, ui);
            };
            options.stop = function (event, ui) {

                //$("." + settings.selectedClass).each(function () {
                //    $(this).animate({ 'position': $(this).data("original") }, 1000, "linear", function () { });
                //});

                regroup(ui.item, ui.item.parent());
                settings.stop(event, ui);
            };
            options.sort = function (event, ui) {
                var parent = ui.item.parent(),
                myIndex = ui.item.data('i'),
                top = parseInt(ui.item.css('top').replace('px', '')),
                left = parseInt(ui.item.css('left').replace('px', ''));
                // fix to keep compatibility using prototype.js and jquery together
                $.fn.reverse = Array.prototype._reverse || Array.prototype.reverse
                var height = 0;
                var width = 0;//ui.item.outerWidth();
                var ctr = 0;
                var rowCtr = 0;

                var cellWidth = ui.item.outerWidth() + 11;
                var cellHeight = ui.item.outerHeight() + 11;
                var colOffset = -(myIndex % 3) * cellWidth;
                var rowOffset = -(Math.floor(myIndex / 3)) * cellHeight;

                //console.log("column: ", (myIndex % 3));
                //console.log("row: ", Math.floor(myIndex / 3));

                if (colOffset >= 0)
                    colOffset = Math.abs(colOffset);
                if (rowOffset >= 0)
                    rowOffset = Math.abs(rowOffset);

                //console.log("colOffset: ", colOffset);
                //console.log("rowOffset: ", rowOffset);

                $('.' + settings.selectedClass, parent).each(function () {

                    var item = $(this);

                    //(ctr == 0) && (rowCtr == 0) ? width += 11 : width = width ;

                    item.css({
                        left: colOffset + (width + left + (11 * (ctr))),
                        top: rowOffset + (top + height),
                        position: 'absolute',
                        zIndex: 1000
                        //,width: ui.item.width()
                    });

                    ctr++;

                    width += item.outerWidth();

                    //console.log("width: " + width);

                    if (width > 800) {
                        rowCtr++;
                        width = 0;
                        height += $(this).outerHeight() + (11);
                        ctr = 0;
                    }
                });

                settings.sort(event, ui);
            };
            options.receive = function (event, ui) {

                console.log("multisortable options.receive hit");

                regroup(ui.item, ui.sender);
                settings.receive(event, ui);
            };
            options.update = function (event, ui) {
                console.log("multisortable options.update hit");

                if ($("#collection-contents-sortable").length) {
                    regroup(ui.item, $(this));
                    $("#collection-contents-sortable").sortable("refresh");
                    UpdateCollection();
                    settings.update(event, ui);
                } else {

                    if (ui.item.data("dragging-ids")) {

                        //can we not just regroup for each item...
                        //console.log(ui.item.data("dragging-ids")); // array, hooray!

                        for (var i = 0; i < ui.item.data("dragging-ids").length; i++) {
                            var t = ui.item.data("dragging-ids")[i];
                            var thisDraggable = $("div.multidraggable-selected").filter(function () {
                                return $(this).data("product-page-id") === t;
                            });

                            $(this).append(thisDraggable)
                        }

                        $(this).sortable("refresh");
                        UpdateProductPageGrid($(this).data("page-section-id"));

                        //if (typeof ProductPageSearch_DataView === "object")
                        //    ProductPageSearch_DataView.PerformCallback();

                        settings.update(event, ui);

                    } else {
                        if (typeof UpdateProductPageGrid === 'function') {


                            regroup(ui.item, $(this));

                            $(".product-page-grid-container").sortable("refresh");
                            UpdateProductPageGrid($(this).data("page-section-id"));

                            //if (typeof ProductPageSearch_DataView === "object")
                            //    ProductPageSearch_DataView.PerformCallback();

                        }
                        settings.update(event, ui);
                    }
                }
            };
            options.beforeStop = function (event, ui) {
                console.log("multisortable options.beforeStop hit");

                //if (ui.sender.hasClass("product-page-grid-container")) {
                //    window.ignoreUpdate = ui.sender.data("page-section-id");
                //}
            };
            list.sortable(options);//.disableSelection();
        })
    };
    $.fn.multisortable.defaults = {
        start: function (event, ui) { },
        stop: function (event, ui) { },
        sort: function (event, ui) { },
        receive: function (event, ui) { },
        click: function (event, elem) { },
        mousedown: function (event, elem) { },
        update: function (event, ui) { },
        selectedClass: 'multisortable-selected',
        placeholder: 'ui-state-highlight-cms-product-page',
        items: 'div.product-page-cell',
        tolerance: 20,
        distance: 60
    };
}(jQuery);