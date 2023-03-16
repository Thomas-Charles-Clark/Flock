/**
* Amended by Thomas Clark 2015
* Intention is to permit multiple elements from multiple dom elements deep inside a container
* to be multiple selected and dragged to a target location.
* 
* jquery.multidraggable.js - v0.1
* https://github.com/shvetsgroup/jquery.multisortable
*
* Author: Ethan Atlakson, Jay Hayes, Gabriel Such, Alexander Shvets, Thomas Clark
* Last Revision 31/12/2015
* multi-selectable, multi-draggable jQuery plugin
*/
!function ($) {
    $.fn.multidragselectable = function (options) {
        if (!options) {
            options = {}
        }
        options = $.extend({}, $.fn.multidragselectable.defaults, options);
        function mouseDown(e) {
            var item = $(this),
            parent = item.parent(),
            myIndex = item.index();
            var prev = parent.find('.multidragselectable-previous');
            // If no previous selection found, start selecting from first selected item.
            prev = prev.length ? prev : $(parent.find('.' + options.selectedClass)[0]).addClass('multidragselectable-previous');
            var prevIndex = prev.index();
            if (e.ctrlKey || e.metaKey) {
                if (item.hasClass(options.selectedClass)) {
                    item.removeClass(options.selectedClass).removeClass('multidragselectable-previous')
                    if (item.not('.child').length) {
                        item.nextUntil(':not(.child)').removeClass(options.selectedClass);
                    }
                }
                else {
                    parent.find('.multidragselectable-previous').removeClass('multidragselectable-previous');
                    item.addClass(options.selectedClass).addClass('multidragselectable-previous')
                    if (item.not('.child').length) {
                        item.nextUntil(':not(.child)').addClass(options.selectedClass);
                    }
                }
            }
            if (e.shiftKey) {
                var last_shift_range = parent.find('.multidragselectable-shift');
                last_shift_range.removeClass(options.selectedClass).removeClass('multidragselectable-shift');
                var shift_range;
                if (prevIndex < myIndex) {
                    shift_range = item.prevUntil('.multidragselectable-previous').add(prev).add(item);
                }
                else if (prevIndex > myIndex) {
                    shift_range = item.nextUntil('.multidragselectable-previous').add(prev).add(item);
                }
                shift_range.addClass(options.selectedClass).addClass('multidragselectable-shift');
            }
            else {
                parent.find('.multidragselectable-shift').removeClass('multidragselectable-shift');
            }
            if (!e.ctrlKey && !e.metaKey && !e.shiftKey) {
                parent.find('.multidragselectable-previous').removeClass('multidragselectable-previous');
                if (!item.hasClass(options.selectedClass)) {
                    parent.find('.' + options.selectedClass).removeClass(options.selectedClass);
                    item.addClass(options.selectedClass).addClass('multidragselectable-previous');
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
            var item = $(this), parent = item.closest(".multidraggable-container");
            // If item wasn't draged and is not multiselected, it should reset selection for other items.
            if (!e.ctrlKey && !e.metaKey && !e.shiftKey) {
                parent.find('.multidragselectable-previous').removeClass('multidragselectable-previous');
                parent.find('.' + options.selectedClass).removeClass(options.selectedClass);
                item.addClass(options.selectedClass).addClass('multidragselectable-previous');
                if (item.not('.child').length) {
                    item.nextUntil(':not(.child)').addClass(options.selectedClass);
                }
            }
            options.click(e, item);
        }

        return this.each(function () {
            var list = $(this);
            if (!list.data('multidragselectable')) {
                list.data('multidragselectable', true)
                .delegate(options.items, 'mousedown', mouseDown)
                .delegate(options.items, 'click', click)
                .disableSelection();
            }
        });
    };
    $.fn.multidragselectable.defaults = {
        click: function (event, elem) { },
        mousedown: function (event, elem) { },
        selectedClass: 'multidragselectable-selected',
        items: 'li'
    };
    $.fn.multidraggable = function (options) {
        if (!options) {
            options = {}
        }
        var settings = $.extend({}, $.fn.multidraggable.defaults, options);
        function regroup(item, list, update, performCallback) {
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
            if (update) {
                var RelatedProductPages = new Array();

                list.find(".placeholder").not('.ui-draggable-dragging').remove();

                list.multidraggable("refresh");

                list.find(".productpage").each(function (index) {
                    RelatedProductPages.push({ ID: $(this).data("product-page-id"), SortOrder: index });
                });

                console.log('ProductPageID: ' + list.data("product-page-id") + ' regroup calling update');

                console.log(RelatedProductPages);

                if (performCallback == null) {
                    UpdateRelatedProductPages(list.data("product-page-id"), RelatedProductPages);
                } else {
                    UpdateRelatedProductPages(list.data("product-page-id"), RelatedProductPages, performCallback);
                }
            }
        }
        return this.each(function () {
            var list = $(this);
            //enable multi-selection
            list.multidragselectable({
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

                var parent = ui.helper.closest(".multidraggable-container");

                console.log("ProductPageID: " + parent.data("product-page-id") + " - multidraggable options.start hit");

                if (ui.helper.hasClass(settings.selectedClass)) {

                    var allIDs = [];
                    allIDs.push(ui.helper.data("product-page-id"));
                    
                    //assign indexes to all selected items
                    parent.find('.' + settings.selectedClass).each(function (i) {
                        $(this).data('i', i);
                        allIDs.push($(this).data("product-page-id"));
                        $(this).parent().data("original-index", i);
                    });

                    ui.helper.data("dragging-ids", allIDs);
                }
                settings.start(event, ui);
            };
            options.stop = function (event, ui) {
                console.log(ui.item);
                var parent = ui.item.closest(".multidraggable-container");

                console.log("ProductPageID: " + parent.data("product-page-id") + " - multidraggable options.stop hit");

                regroup(ui.item, parent, true, null);

                settings.stop(event, ui);
            };
            options.sort = function (event, ui) {
                if (!ui.helper.hasClass("ui-draggable")) {

                    var parent = ui.helper.parent(),
                    myIndex = ui.helper.data('i'),
                    top = parseInt(ui.helper.css('top').replace('px', '')),
                    left = parseInt(ui.helper.css('left').replace('px', ''));
                    // fix to keep compatibility using prototype.js and jquery together
                    $.fn.reverse = Array.prototype._reverse || Array.prototype.reverse
                    var height = 0;
                    var width = 0;
                    var ctr = 0;
                    var rowCtr = 0;

                    var cellWidth = ui.helper.outerWidth();
                    var cellHeight = ui.helper.outerHeight();
                    var colOffset = -(myIndex) * cellWidth;
                    var rowOffset = -(Math.floor(myIndex)) * cellHeight;

                    if (colOffset >= 0)
                        colOffset = Math.abs(colOffset);
                    if (rowOffset >= 0)
                        rowOffset = Math.abs(rowOffset);

                    $('.' + settings.selectedClass, parent).each(function () {

                        var item = $(this);

                        item.css({
                            left: colOffset + (width + left),
                            top: (top + height),
                            position: 'absolute',
                            zIndex: 1000
                        });

                        ctr++;

                        width += item.outerWidth();

                        if (width > 800) {
                            rowCtr++;
                            width = 0;
                            height += $(this).outerHeight() + (11);
                            ctr = 0;
                        }
                    });
                }

                settings.sort(event, ui);
            };
            options.receive = function (event, ui) {
                var parent = (ui.helper || ui.item).closest(".multidraggable-container");
               
                console.log("ProductPageID: " +  $(this).data("product-page-id") + " - multidraggable options.receive hit");

                // need to include ui.sender.find(".ui-multiselect-highlight-product-page");

                parent.append(ui.sender.find(".ui-multiselect-highlight-product-page"))

                regroup((ui.helper || ui.item), parent, false, false);

                settings.receive(event, ui);
            };
            options.update = function (event, ui) {
                var parent = ui.item.closest(".multidraggable-container");

                console.log("ProductPageID: " + parent.data("product-page-id") + " - multidraggable options.update hit");

                regroup(ui.item, parent, false, false);
                

                settings.update(event, ui);
            };
            list.sortable(options);
        })
    };
    $.fn.multidraggable.defaults = {
        start: function (event, ui) { },
        stop: function (event, ui) { },
        sort: function (event, ui) { },
        receive: function (event, ui) { },
        click: function (event, elem) { },
        mousedown: function (event, elem) { },
        update: function (event, ui) { },
        helper: "original",
        selectedClass: 'multidraggable-selected',
        placeholder: 'ui-state-highlight-cms-product-page',
        items: '.multidraggable',
        tolerance: 0,
        distance: 20
    };
}(jQuery);