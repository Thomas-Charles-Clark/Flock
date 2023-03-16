function init() {
    $(".layout-control").draggable({
        helper: "clone",
        start: function (e, ui) {
            $(ui.helper).css("z-index", "300000");
        }
    });

    $(".section-type").draggable({
        helper: "clone",
        start: function (e, ui) {
            $(ui.helper).css("z-index", "300000");
        }
    });

    $(".page-section").droppable({
        accept: ".layout-control",
        drop: function (e, u) {
            InsertLayoutControl(e, u);
        }
    });
}

function ClearPage() {
    alert("ClearPage() Error: Not Implemented");
}

//function SavePage() {

//    if (typeof (window.slideshow) != "undefined") {
//        window.slideshow.Enabled = false;
//        window.slideshow.ClientEnabled = false;
//    }

//    $("#settings-arrow").remove();
//    $("#settings-pane").remove();
    
//    var sending = [];

//    var obj = $("#page-chassis");
//    var firstChildren = $("> div", obj);//.clone();
//    var counter = 0;

//    //$("#status-div").load("/admin/CMS/ClearPage", { 'pageID': window.pageID.toString() }, function () {
//        var id = $("#page-id-div").html();

//        for (var i = 0; i < firstChildren.length; i++) {
//            // assign a page-section-id
//            firstChildren[i].id = "page-section-" + (i + 1);
//            counter += 1;

//            //var thisResult = $("<div id='result-" + counter + "'></div>");

//            //$("#status-div").append(thisResult);

//            var justTag = $(firstChildren[i].outerHTML);
//            $(justTag).html("");

//            // must push PSID if extant
//            var psid = $(firstChildren[i]).attr("psid");
//            if (typeof (psid) !== "undefined" && psid !== false) {
//                sending.push(
//                    {
//                        'pageID': id.toString(),
//                        'pageSectionTypeID': '4',
//                        'pageSectionID': psid.toString(),
//                        'sortOrder': counter.toString(),
//                        'parentID': firstChildren[i].parentNode.id.toString(),
//                        'contents': htmlEncode($(justTag)[0].outerHTML),
//                        'eventID': '0'
//                    }
//                );
//            }
//            else {
//                sending.push(
//                  {
//                      'pageID': id.toString(),
//                      'pageSectionTypeID': '4',
//                      'sortOrder': counter.toString(),
//                      'parentID': firstChildren[i].parentNode.id.toString(),
//                      'contents': htmlEncode($(justTag)[0].outerHTML),
//                      'eventID': '0'
//                  }
//              );
//            }

//            //$("#status-div").load("/Admin/CMSSavePageSection", {
//            //    'pageID': id.toString(),
//            //    'pageSectionTypeID': '4',
//            //    'sortOrder': counter.toString(),
//            //    'parentID': firstChildren[i].parentNode.id.toString(),
//            //    'contents': htmlEncode($(justTag)[0].outerHTML),
//            //    'eventID': '0'
//            //});
//        }


//        for (var i = 0; i < firstChildren.length; i++) {
//            var t = $(firstChildren[i]);
//            var secondChildren = $("> div", t);
//            for (var f = 0; f < secondChildren.length; f++) {
//                var q = $(secondChildren[f].children(0));

//                //secondChildren.remove(".removable");

//                counter += 1;
//                secondChildren[f].id = "page-section-" + counter;

//                var sectionContents = $(secondChildren[f].outerHTML).filter(":not('script')");

//                // in here needs to be the check for if we're dealing with grids within grids

//                if ($(sectionContents[0]).children().length > 0) {
//                    if ($(sectionContents[0]).children().children().length > 0)
//                        if ($(sectionContents[0]).children(0)[0].id != "category-framework-grid")
//                            $(sectionContents[0]).children().children().remove();
//                }

//                //.find(':not(.removable)');
                
//                // need for form JSON array for each page section, then send the whole thing to the server at once.

//                // must push PSID if extant
//                // var psid = $(secondChildren)[f].attr("psid") if typeof(psid) !== "undefined" && psid !== false
//                var psid = $(secondChildren[f]).attr("psid");
//                if (typeof (psid) !== "undefined" && psid !== false) {
//                    sending.push(
//                        {
//                            'pageID': id.toString(),
//                            'pageSectionTypeID': '4',
//                            'pageSectionID': psid.toString(),
//                            'sortOrder': counter.toString(),
//                            'parentID': secondChildren[f].parentNode.id.toString(),
//                            'contents': htmlEncode(sectionContents[0].outerHTML),
//                            'eventID': '0'
//                        }
//                    );
//                }
//                else {
//                    sending.push(
//                      {
//                          'pageID': id.toString(),
//                          'pageSectionTypeID': '4',
//                          'sortOrder': counter.toString(),
//                         'parentID': secondChildren[f].parentNode.id.toString(),
//                          'contents': htmlEncode(sectionContents[0].outerHTML),
//                          'eventID': '0'
//                      }
//                  );
//                }

//                //$("#status-div").load("/Admin/CMSSavePageSection", {
//                //    'pageID': id.toString(),
//                //    'pageSectionTypeID': '4',
//                //    'sortOrder': counter.toString(),
//                //    'parentID': secondChildren[f].parentNode.id.toString(),
//                //    'contents': htmlEncode(sectionContents[0].outerHTML),
//                //    'eventID': '0'
//                //});
//            }
//        }

        

//        $.ajax({
//            url: '/admin/CMS/SavePageSections',
//            type: 'POST',
//            datatype: "json",
//            traditional: true,
//            data: {
//                "pageSections": JSON.stringify(sending)
//            },
//            success: function (d) {
//                $("#status-div").append("<div>" + d + "</div>");

//                //while (tempRemovals.length) {
//                //    var o = tempRemovals.pop();
//                //    $(o.obj).appendTo(o.loc);
//                //}

//                for (var i = 0; i < d.length; i++) {
//                    $("#page-section-" + d[i].SortOrder).attr("psid", d[i].PageSectionID);
//                }
//            },
//            error: function (xhr, status, error) {
//                if (xhr.status == 403) {
//                    alert("Sorry, your session has expired. Please login again to continue");
//                    window.location.href = "/Account/Login";
//                }
//                else {
//                    if (xhr.responseText)
//                        alert("An error occurred: " + status + "\r\nError: " + error + "\r\nResponse: " + xhr.responseText);
//                    else
//                        alert("An error occurred: " + status + "\r\nError: " + error);
//                }
//            },
//            timeout: 20000
//        });

//    //});

//   //ReloadSections();
//}

function InsertLayoutControl(event, ui) {
    // what type of layout control do we insert

    // need to insert a BlankContentItem for each so that they're retained...
    switch (ui.draggable[0].id) {
        case "full-width-layout-control-2-0":
            var t = $("<div class='pure-u-1 two-zero new-layout'></div>");
            $(event.target).append(t);
            //function AddContentItem(type, destinationtype, classname, section, sortorder)
            AddContentItem("blank", "page-section", $(t).attr("class"), $(event.target).attr("id").replace("page-section-", ""), $(t).index());
            ConfigureDragDrop();
            break;

        case "full-width-layout-control-2-6":
            var t = $("<div class='pure-u-1 two-six new-layout'></div>");
            $(event.target).append(t);
            AddContentItem("blank", "page-section", $(t).attr("class"), $(event.target).attr("id").replace("page-section-", ""), $(t).index());
            ConfigureDragDrop();
            break;

        case "one-half-layout-control":
            var t = $("<div class='pure-u-1-2 new-layout'></div>");
            $(event.target).append(t);
            AddContentItem("blank", "page-section", $(t).attr("class"), $(event.target).attr("id").replace("page-section-", ""), $(t).index());
            ConfigureDragDrop();
            break;

        case "one-third-layout-control":
            var t = $("<div class='pure-u-1-3 new-layout'></div>");
            $(event.target).append(t);
            AddContentItem("blank", "page-section", $(t).attr("class"), $(event.target).attr("id").replace("page-section-", ""), $(t).index());
            ConfigureDragDrop();
            break;

        case "one-quarter-layout-control":
            var t = $("<div class='pure-u-1-4 new-layout'></div>");
            $(event.target).append(t);
            AddContentItem("blank", "page-section", $(t).attr("class"), $(event.target).attr("id").replace("page-section-", ""), $(t).index());
            ConfigureDragDrop();
            break;

        case "one-sixth-layout-control":
            var t = $("<div class='pure-u-1-6 new-layout'></div>");
            $(event.target).append(t);
            AddContentItem("blank", "page-section", $(t).attr("class"), $(event.target).attr("id").replace("page-section-", ""), $(t).index());
            ConfigureDragDrop();
            break;

        case "content-grid-layout-control":
            if ($(event.target).attr("id").indexOf("page-section") == -1)
                alert("Error: Content Grids may not contain further Content Grids");
            else {
                var t = $("<div class='pure-u-1-2 new-layout' style='height: 200px; background-color: #C0FAC9;'></div>");
                $(event.target).append(t);
                AddContentItem("contents-grid", "page-section", "pure-u-1-2", $(event.target).attr("id").replace("page-section-", ""), $(event.target).index());
                ConfigureDragDrop();
            }
            break;

        case "search-grid-layout-control":
            var t = $("<div class='pure-u-1-4 new-layout' style='min-height: 600px; background-color: #001122;'></div><div style='min-height: 600px; background-color: #112233;' class='pure-u-3-4'></div>");
            $(event.target).append(t);

            // DRY: what the fuck at these first three parameters being the same, Tom?
            AddContentItem("results-grid", "results-grid", "results-grid", $(event.target).attr("id").replace("page-section-", ""), $(event.target).index());

            ConfigureDragDrop();
            break;

        default:
            break;
    }
}

function InsertSiteSection(event, ui) {
    if ($(event.target).data("type") == "blank") {
        RemoveBlankContentItem($(event.target).data("content-id"));
    }

    switch (ui.draggable[0].id) {
        case "section-type-new-product":
            var newProduct = $("<h4>[product]</h4>");    
            $(event.target).append(newProduct);
            $(event.target).attr("type", "product");
            ConfigureSection(event.target);
            break;
        case "section-type-new-image":
            var newImage = $("<h4>[image]</h4>");
            $(event.target).append(newImage);
            $(event.target).attr("type", "image");
            ConfigureSection(event.target);
            break;
        case "section-type-new-html":
            var newHtml = $("<h4>HTML</h4>");
            $(event.target).html("");
            $(event.target).append(newHtml);
            $(event.target).attr("type", "html");

            if (typeof ($(event.target).parent().attr("id")) !== "undefined" && $(event.target).parent().attr("id") !== false)
                if ($(event.target).parent().attr("id").indexOf("page-section-") != -1)
                    AddContentItem("html", "page-section", event.target.classList[0], $(event.target).parent().attr("id").replace("page-section-", ""), $(event.target).index());
                else
                    AddContentItem("html", "grid-section", event.target.classList[0], $(event.target).parent().attr("id").replace("grid-section-", ""), $(event.target).index());
            else {
                if (typeof ($(event.target).parent().attr("data-content-id")) !== "undefined" && $(event.target).parent().attr("data-content-id") !== false) {
                    AddContentItem("html", "grid-section", event.target.classList[0], $(event.target).parent().attr("data-content-id"), $(event.target).index());
                }
                else
                    alert("InsertSiteSection Error (Line 288): Couldn't fetch PageSectionID or DataContentID.\r\n"
                        + "Either: \r\n" 
                        + "$(event.target).parent().attr('id')" 
                        + "or, "
                        + "$(event.target).parent().attr('data-content-id')"
                        + "was undefined.");
            }
            break;
        case "section-type-new-slideshow":
            var newSlideshow = $("<h4>[slideshow]</h4>");
            $(event.target).html("");
            $(event.target).append(newSlideshow)
            $(event.target).attr("type", "slideshow");

            if (typeof ($(event.target).parent().attr("id")) !== "undefined" && $(event.target).parent().attr("id") !== false)
                if ($(event.target).parent().attr("id").indexOf("page-section-") != -1)
                    AddContentItem("slideshow", "page-section", event.target.classList[0] + ' ' + event.target.classList[1], $(event.target).parent().attr("id").replace("page-section-", ""), $(event.target).index());
                else
                    AddContentItem("slideshow", "grid-section", event.target.classList[0] + ' ' + event.target.classList[1], $(event.target).parent().attr("id").replace("grid-section-", ""), $(event.target).index());
            else {
                if (typeof ($(event.target).parent().attr("data-content-id")) !== "undefined" && event.target.parent().attr("data-content-id") !== false) {
                    AddContentItem("slideshow", "grid-section", event.target.classList[0] + ' ' + event.target.classList[1], $(event.target).parent().attr("data-content-id"), $(event.target).index());
                }
                else
                    alert("InsertSiteSection Error (Line 312): Couldn't fetch PageSectionID or DataContentID.\r\n"
                        + "Either: \r\n"
                        + "$(event.target).parent().attr('id')"
                        + "or, "
                        + "$(event.target).parent().attr('data-content-id')"
                        + "was undefined.");
            }

            //$(event.target).append(newSlideshow);
            //$(event.target).parent().attr("type", "slideshow");

            //$(event.target).parent().css("height", "auto");
            //$(event.target).css("height", "auto");

            //var ss = $("<div style='width: 100%; height: auto;' class='removable'></div>");
            //$.ajax({
            //    url: '/admin/CMS/ImageSliderPartial',
            //    type: 'POST',
            //    datatype: "json",
            //    data: {
            //        "pageSectionID": $(event.target).parent().attr("id").replace("page-section-", "").toString()
            //    },
            //    success: function (d) {
            //        ss.append(d);
            //        $(event.target).append(ss);
            //    },
            //    error: function (event, request, settings) {
            //        window.alert('@Resource.AjaxError' + ' : ' + settings);
            //    },
            //    timeout: 20000
            //});

            //SavePage();
            //ss.load('/admin/CMS/ImageSliderPartial', function () {
            //    $(event.target).append(ss);
            //});
            break;

        case "section-type-content-grid":

            break;

        case "section-type-new-banner":
            var newBanner = $("<h4>[banner]</h4>");
            $(event.target).append(newBanner);
            $(event.target).parent().attr("type", "banner");
            break;
        case "section-type-new-other":
            var newOther = $("<h4>[etc]</h4>");
            $(event.target).append(newOther);
            $(event.target).parent().attr("type", "other");
            break;

        case "section-type-form":
            // should open up a selector to choose form with.
            // $(event.target).parent().attr("id").replace("page-section-", "")
            $("#dialog-form-select").dialog("open");
            $("#dialog-form-hidden-psid").val($(event.target).parent().attr("id").replace("page-section-", ""));
            $("#dialog-form-hidden-classname").val($(event.target)[0].classList[0]);
            break;
        default:
            break;
    }
}


function ConfigureSection(element) {



    // parentNode will be the pure-u-M-N element
    // which holds data-type, data-content-id
    
    var pane = 0;
    var arrow = 0;

    if ($("#settings-pane").length > 0)
        pane = $("#settings-pane");
    else
        pane = $("<div id='settings-pane'><div id='settings-pane-inner'><div id='slideshow-settings-container'></div><div id='html-editor-container'></div></div></div>");


    if ($("#settings-arrow").length > 0)
        arrow = $("#settings-arrow");
    else
        arrow = $("<div id='settings-arrow'></div>");

    ////////////////////////////////
    //
    // For posterity: this function should check the calling pagesectiontype and render the appropriate control panel.
    //
    ////////////////////////////////
    
    var targetElement = $(element).closest("[data-sort-order]");

    var Type = $(targetElement).attr("data-type");
    var ContentID = $(targetElement).attr("data-content-id");

    $(element).closest(".section-inner").append(arrow);


    $(arrow).show();
    //$(pane).css('left', ($("#page-chassis").position().left - $(arrow).offset().left) + 73  + 'px');
    $(pane).show();


    $(arrow).position({
        my: 'center top',
        at: 'center bottom',
        of: $(element)
    });

    $(arrow).css("position", "relative");
    $(arrow).css("z-index", "10");
    
    //$(arrow).append(pane);

    $(pane).css('position', 'relative');
    //$(pane).css('top', '20px');
    //$(arrow).css('position', 'absolute');
    //$(arrow).css('top', $(targetElement).css('height'));
    //$(arrow).css('left', ($(targetElement).width() / 2 - 10) + 'px');

    //$(arrow).show();
    $(pane).position({
        my: 'center',
        at: 'center',
        of: '#page-chassis'
    });

    $(pane).position({
        my: 'top',
        at: 'bottom',
        of: '#settings-arrow'
    });

    //$(pane).show();
    //var addition = $("#slideshow-settings-container");


    if (typeof Type !== 'undefined' && Type !== false) {
        if (typeof (ContentID) !== 'undefined' && ContentID !== false) {
            switch (Type) {
                case "slideshow":
                    {
                        var ss = $("<div style='width: 100%; height: auto' class='removable'></div>");

                        $.ajax({
                            url: '/admin/CMS/ImageSliderSettingsPartial',
                            type: 'POST',
                            data: {
                                "slideShowID": ContentID
                            },
                            success: function (data) {
                                $("#slideshow-settings-container").html("");
                                $("#slideshow-settings-container").append(data);
                                return; 
                            },
                            error: function (event, request, settings) {
                                window.alert('@Resource.AjaxError' + ' : ' + event.responseText);
                            },
                            timeout: 20000
                        });
                    }
                    //alert("ConfigureSection() Error - Slideshow: Not Implemented");
                    break;

                case "html":
                    var ht = $("<div style='width: 100%; height: auto' class='removable'></div>");
                    $.ajax({
                        url: "/admin/CMS/HtmlEditorPartial",
                        type: "POST",
                        data: {
                            htmlSectionID: ContentID
                        },
                        success: function (data) {
                            ht.append(data);
                            $("#html-editor-container").html("");
                            $("#html-editor-container").append(ht);
                        },
                        error: function (event, request, settings) {
                            alert("Error loading HtmlEditorPartial: " + event.responseText);
                        },
                        timeout: 20000
                    });
                    break;
                default:
                    break;
            }
        }
        else {
            alert("Error loading configuration panel: 'PageSectionID' was not defined.");
        }
    }
    else {
        //alert("Error loading configuration panel: 'Type' was not defined.");
    }
    //if (typeof (attrPSID) !== 'undefined' && attrPSID !== false) {

    //    var pss = $("<div style='width: 100%; height: auto' class='removable'></div>");

    //    $.ajax({
    //        url: "/admin/CMS/PageSectionBasicSettings",
    //        type: "POST",
    //        datatype: "json",
    //        data: {
    //            "pageSectionID": attrPSID.toString()
    //        },
    //        success: function (data) {
    //            pss.append(data);
    //            $("#pagesection-settings-container").html("");
    //            $("#pagesection-settings-container").append(pss);
    //        },
    //        error: function (event, request, settings) {
    //            alert("Error loading PageSectionBasicSettings: " + event.responseText);
    //        },
    //        timeout: 20000
    //    });
    //}

    $(arrow).click(function (event) {
        event.preventDefault();
        event.stopImmediatePropagation();
        if ($(event.target).attr('id') == $(arrow).attr('id')) {
            $("#slideshow-settings-container").html("");
            $("#html-editor-container").html("");
            arrow.hide();
            pane.hide();
            return false;
        }
        return false;
    });
    
    //$(targetElement).children(0).append(arrow);

    ////$(pane).insertAfter($(element).parent());
    //$(arrow).append(pane);

    //$(pane).css('position', 'relative');
    //$(pane).css('top', '20px');
    //$(arrow).css('position', 'absolute');
    //$(arrow).css('top', $(targetElement).css('height'));
    //$(arrow).css('left', ($(targetElement).width() / 2 - 10) + 'px');

    ////$(pane).css("position", "relative");
    ////$(pane).css("top", ($(arrow).position().top + 20) + 'px');

}


function ConfigureDragDrop() {
    $("#page-chassis").sortable();

    $(".pure-g").sortable({
        update: function (e, u) {
            // need to check if we're sorting within a page-section or if we're sorting whole page-sections: if parent hasclass page-section we're within a section and must sort the contentitems 
            if ($(u.item).parent().hasClass("page-section"))
                SortContentItems($(u.item).parent());
            else 
                SortPageSections();
            // must update sort order of contentitems in the ui.element body 
        },
        scroll: false
    });

    $(".pure-g").droppable({
        accept: ".layout-control",
        greedy: true,
        drop: function (e, u) {
            // if event.target.id == ui.id...
            
            InsertLayoutControl(e, u); // we want to prevent duplication of addition... dropping in a gridsection does not create on pagesection
            return false;
        }
    });

    $(".content-grid").droppable({
        accept: ".layout-control",
        greedy: true,
        drop: function (e, u) {
            InsertLayoutControl(e, u);
        }
    });

    $(".new-layout").sortable({
        helper: "clone"
    });
    //$(".section-type").draggable({
    //    helper: "clone",
    //start: function (e, ui) {
    //    $(ui.helper).css("z-index", "300000");
    //}
    //});
    $(".new-layout").droppable({
        accept: ".section-type, .grid-control",
        drop: function (e, u) {
            InsertSiteSection(e, u);
        }
    });


    $(".section-inner").click(function (event) {
        ConfigureSection(event.target);
        return false;
    });
}

function htmlEncode(value) {
    return String(value)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
}

function htmlDecode(value) {
    return String(value)
        .replace(/&quot;/g, '"')
        .replace(/&#39;/g, "'")
        .replace(/&lt;/g, '<')
        .replace(/&gt;/g, '>')
        .replace(/&amp;/g, '&');
}