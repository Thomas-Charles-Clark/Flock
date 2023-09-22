function InitRelatedPagesDragDrop() {
    $("#related-pages").sortable({
        placeholder: 'related-page-placeholder',
        helper: 'clone',
        connectWith: '.page',
        beforeStop: function (event, ui) {
            var pageid = ui.item.data("page-id");
            if (pageid == $("#selected-page-id").val()) {
                $(ui.helper).remove();
                $(ui.item).remove();
                window.cancelRelatedDrop = true;
            }
        },
        receive: function (event, ui) {
            if (window.cancelRelatedDrop) {
                window.cancelRelatedDrop = false;
                $(ui.sortable).sortable("cancel");
            }
        },
        update: function (ev, ui) {
            window.RelatedPages = new Array();
            $("#related-page-placeholder").remove();
            $("#related-pages").sortable("refresh");
            $("#related-pages").children("div").each(function (index) {
                window.RelatedPages.push({ ID: $(this).data("page-id"), SortOrder: index });
            });
            UpdateRelatedPages();
        },
        scroll: false
    });

    $(".page").draggable({
        helper: 'clone',
        start: function (e, ui) {
            $(ui.helper).css("z-index", "300000");
        },
        connectToSortable: "#related-pages"
    });
}


function UpdateRelatedPages() {
    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/updaterelatedpages',
        type: 'POST',
        traditional: true,
        data: { BasePageID: Number($("#selected-page-id").val()), Pages: JSON.stringify(window.RelatedPages) },
        success: function (data) {
            LoadingPanel.Hide();
            $("#related-pages-container").html(data);
        },
        timeout: 20000
    });
}

function RemoveRelatedPage(PageID) {
    LoadingPanel.Show();
    $(".related-page-sortable[data-page-id='" + PageID + "']").remove();
    $.ajax({
        url: '/admin/cms/removerelatedpage',
        type: 'POST',
        traditional: true,
        data: { BasePageID: Number($("#selected-page-id").val()), PageID: PageID },
        success: function (data) {
            LoadingPanel.Hide();
            $("#related-pages-container").html(data);
        },
        timeout: 20000
    });
}

function SaveFormDetails(FormID, PageSectionID) {
    LoadingPanel.Show();

    var compID = null;
    if (typeof window["Form" + FormID + "CompetitionID"] == "object")
        compID = window["Form" + FormID + "CompetitionID"].GetValue();

    var thirdPartyID = null;
    if (typeof window["Form" + FormID + "ThirdPartyID"] == "object")
        thirdPartyID = window["Form" + FormID + "ThirdPartyID"].GetValue();

    var brochID = null;
    if (typeof window["Form" + FormID + "BrochureID"] == "object")
        brochID = window["Form" + FormID + "BrochureID"].GetValue();

    var gst = null;
    if (typeof window["Form" + FormID + "GenericSharingText"] == "object")
        gst = window["Form" + FormID + "GenericSharingText"].GetValue();

    var pst = null;
    if (typeof window["Form" + FormID + "PartnerSharingText"] == "object")
        pst = window["Form" + FormID + "PartnerSharingText"].GetValue();

    var storeEventID = null;
    if (typeof window["Form" + FormID + "StoreEventID"] == "object")
        storeEventID = window["Form" + FormID + "StoreEventID"].GetValue();


    $.ajax({
        url: '/admin/cms/saveformdetails',
        type: 'POST',
        traditional: true,
        data: {
            FormID: FormID,
            RedirectPageID: window["Form" + FormID + "RedirectPageID"].GetValue(),
            PromotionCodeID: window["Form" + FormID + "PromotionCodeID"].GetValue(),
            ConfirmEmail: window["Form" + FormID + "ConfirmEmail"].GetValue(),
            SourceID: window["Form" + FormID + "SourceID"].GetValue(),
            Name: window["Form" + FormID + "Name"].GetValue(),
            Heading: window["Form" + FormID + "Heading"].GetValue(),
            SubHeading: window["Form" + FormID + "SubHeading"].GetValue(),
            UseCaptcha: window["Form" + FormID + "UseCaptcha"].GetValue(),
            DisplayAddressBlock: window["Form" + FormID + "DisplayAddressBlock"].GetValue(),
            CompetitionID: compID,
            ThirdPartyID: thirdPartyID,
            BrochureID: brochID,
            PageSectionID: PageSectionID,
            PageSectionName: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "PageSectionName']").val(),
            StoreEventID: storeEventID,
            OnDate: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "OnDate']").val(),
            OffDate: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "OffDate']").val(),
            PhoneNumberRequired: window["Form" + FormID + "PhoneNumberRequired"].GetValue()
        },
        success: function (data) {
            $("#page-section-" + PageSectionID + "-update-target").html(data);
            LoadingPanel.Hide();
            SuccessFade($("#page-section-" + PageSectionID + "-save-success"));
        },
        error: function (e, s, a) {
            LoadingPanel.Hide();
            $("#page-section-" + PageSectionID + "-update-target").html(e.responseText);
            FailureFade($("#page-section-" + PageSectionID + "-save-success"));
        },
        timeout: 20000
    });
}

function ShowAdvancedSettings(PageSectionID, PageSectionTypeID, ContentID) {
    if ($('#page-section-' + PageSectionID + '-advanced-settings').length > 0) {
        var i = $('#page-section-' + PageSectionID + '-advanced-settings').detach();
        $('#page-section-advanced-settings-dialog').html("");
        $('#page-section-advanced-settings-dialog').append(i);
        $('#page-section-' + PageSectionID + '-advanced-settings').show();
        $('#page-section-advanced-settings-dialog').data('pagesectionid', PageSectionID);
        $('#page-section-advanced-settings-dialog').data('type', PageSectionTypeID);
        $('#page-section-advanced-settings-dialog').dialog('open');
    }
    else {
        alert("This page section has no advanced settings.");
    }
}

function SavePageGridAdvancedSettings(PageSectionID) {
    $.ajax({
        url: '/admin/cms/savepagegridadvancedsettings',
        type: 'POST',
        traditional: true,
        data: {
            PageSectionID: PageSectionID,
            MultiplePodImages: window["PageSection" + PageSectionID + "MultiplePodImages"].GetValue(),
            Paging: window["PageSection" + PageSectionID + "Paging"].GetValue()
        },
        success: function (data) {
            ReloadSectionsForEdit();
        },
        error: function (xhr, status, error) {
            if (xhr.status == 403) {
                alert('Sorry, your session has expired. Please login again to continue.');
                window.location.href = '/Account/Login';
            }
            else {
                if (xhr.responseText)
                    alert('An error occurred: ' + status + '\r\nError: ' + error + '\r\nResponse: ' + xhr.responseText);
                else
                    alert('An error occurred: ' + status + '\r\nError: ' + error);
            }
        },
        timeout: 20000
    });
}

function SaveArticleGridAdvancedSettings(PageSectionID) {
    $.ajax({
        url: '/admin/cms/SaveArticleGridAdvancedSettings',
        type: 'POST',
        traditional: true,
        data: {
            PageSectionID: PageSectionID,
            MultiplePodImages: window["PageSection" + PageSectionID + "MultiplePodImages"].GetValue(),
            Paging: window["PageSection" + PageSectionID + "Paging"].GetValue(),
            ArticleAutoPopulate: window["PageSection" + PageSectionID + "AutoPopulate"].GetValue()
        },
        success: function (data) {
            ReloadSectionsForEdit();
        },
        error: function (xhr, status, error) {
            if (xhr.status == 403) {
                alert('Sorry, your session has expired. Please login again to continue.');
                window.location.href = '/Account/Login';
            }
            else {
                if (xhr.responseText)
                    alert('An error occurred: ' + status + '\r\nError: ' + error + '\r\nResponse: ' + xhr.responseText);
                else
                    alert('An error occurred: ' + status + '\r\nError: ' + error);
            }
        },
        timeout: 20000
    });
}

function SaveProductPageGridAdvancedSettings(PageSectionID) {
    $.ajax({
        url: '/admin/cms/SaveProductPageGridAdvancedSettings',
        type: 'POST',
        traditional: true,
        data: {
            PageSectionID: PageSectionID,
            MultiplePodImages: window["PageSection" + PageSectionID + "MultiplePodImages"].GetValue(),
            IsLifestyleGallery: window["PageSection" + PageSectionID + "IsLifestyle"].GetValue(),
            Paging: window["PageSection" + PageSectionID + "Paging"].GetValue(),
            Filter: window["PageSection" + PageSectionID + "DisplayFilters"].GetValue(),
            PreviewColourways: window["PageSection" + PageSectionID + "PreviewColourways"].GetValue(),
            PreviewColourwaysCircularClip: window["PageSection" + PageSectionID + "PreviewColourwaysCircularClip"].GetValue()
        },
        success: function (data) {
            ReloadSectionsForEdit();
        },
        error: function (xhr, status, error) {
            if (xhr.status == 403) {
                alert('Sorry, your session has expired. Please login again to continue.');
                window.location.href = '/Account/Login';
            }
            else {
                if (xhr.responseText)
                    alert('An error occurred: ' + status + '\r\nError: ' + error + '\r\nResponse: ' + xhr.responseText);
                else
                    alert('An error occurred: ' + status + '\r\nError: ' + error);
            }
        },
        timeout: 20000
    });
}

function SaveCollectionGridAdvancedSettings(PageSectionID) {
    $.ajax({
        url: '/admin/cms/SaveCollectionGridAdvancedSettings',
        type: 'POST',
        traditional: true,
        data: {
            PageSectionID: PageSectionID,
            MultiplePodImages: window["PageSection" + PageSectionID + "MultiplePodImages"].GetValue(),
            Paging: window["PageSection" + PageSectionID + "Paging"].GetValue(),
        },
        success: function (data) {
            ReloadSectionsForEdit();
        },
        error: function (xhr, status, error) {
            if (xhr.status == 403) {
                alert('Sorry, your session has expired. Please login again to continue.');
                window.location.href = '/Account/Login';
            }
            else {
                if (xhr.responseText)
                    alert('An error occurred: ' + status + '\r\nError: ' + error + '\r\nResponse: ' + xhr.responseText);
                else
                    alert('An error occurred: ' + status + '\r\nError: ' + error);
            }
        },
        timeout: 20000
    });
}

function SaveSlideshowAdvancedSettings(PageSectionID) {
    LoadingPanel.Show();
    var targ = $("#page-section-" + PageSectionID + "-advanced-settings").find(".slideshow-overlay").first();

    var col = targ.css("background-color");
    col = col.substring(col.indexOf('(') + 1, col.lastIndexOf(')')).split(/,\s*/);
    col = col.splice(0, 3);
    var rgb = col.join(',');

    var fCol = targ.css("color");
    fCol = fCol.substring(fCol.indexOf('(') + 1, fCol.lastIndexOf(')')).split(/,\s*/);
    var fontRGB = fCol.join(',');

    var opa = window["SlideShow" + PageSectionID + "OverlayOpacity"].GetValue();
    $(targ).parent().hide();
    var w = targ.width();
    var h = targ.height();
    var t = parseFloat(targ[0].style.top);
    var l = parseFloat(targ[0].style.left);
    $(targ).parent().show();

    var imageID = $("#page-section-" + PageSectionID + "-advanced-settings").find(".slideshow-overlay-display-container").first().data("image-id");

    $.ajax({
        url: '/admin/cms/SaveSlideshowImageOverlaySettings',
        type: 'POST',
        traditional: true,
        data: {
            SlideshowID: $("#page-section-" + PageSectionID).data("slideshow-id"),
            PageSectionID: PageSectionID,
            DisplayOverlay: window["SlideShow" + PageSectionID + "DisplayOverlay"].GetValue(),
            ShowHotspots: window["SlideShow" + PageSectionID + "ShowHotspots"].GetValue(),
            OverlayTop: t,
            OverlayLeft: l,
            OverlayWidth: w,
            OverlayHeight: h,
            OverlayBackgroundColour: rgb,
            OverlayBackgroundOpacity: opa,
            OverlayFontColour: fontRGB,
            SlideshowOverlayPositionID: $('#slideshow-' + PageSectionID + '-overlay-position').val(),
            AutoHideOverlay: window["SlideShow" + PageSectionID + "AutoHideOverlay"].GetValue(),
            PageID: $("#Slideshow" + PageSectionID + "page-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "page-id-hidden").val(),
            ProductPageID: $("#Slideshow" + PageSectionID + "product-page-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "product-page-id-hidden").val(),
            CollectionID: $("#Slideshow" + PageSectionID + "collection-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "collection-id-hidden").val(),
            ExternalURL: window["Slideshow" + PageSectionID + "ExternalURL"].GetValue(),
            NewWindow: window["Slideshow" + PageSectionID + "NewWindow"].GetValue(),
            OverlayHeading: window["Slideshow" + PageSectionID + "OverlayHeading"].GetValue(),
            OverlayText: window["Slideshow" + PageSectionID + "OverlayText"].GetValue(),
            ImageID: imageID,
            OverlayFade: window["SlideShow" + PageSectionID + "OverlayFade"].GetValue()
        },
        success: function (data) {

            //alert("Ok - now return the actual damned slideshow.");
            LoadingPanel.Hide();
            $("#slideshow-image-" + PageSectionID + "-overlay-container").html(data);
            //ReloadSectionsForEdit();
        },
        timeout: 20000
    });
}

function SaveBrochureSection(PageSectionID) {
    $.ajax({
        url: '/admin/cms/SaveBrochurePageSection',
        type: 'POST',
        traditional: true,
        data: {
            PageSectionID: PageSectionID,
            BrochureID: window["ComposePageBrochureComboBox" + PageSectionID].GetValue()
        },
        success: function (data) {
            SuccessFade($("#brochure-section-" + PageSectionID + "-save"));
        },
        error: function (e, s, a) {
            FailureFade($("#brochure-section-" + PageSectionID + "-save"));
        },
        timeout: 20000
    });
}

function SaveVideoSection(PageSectionID) {
    $.ajax({
        url: '/admin/cms/SaveVideoPageSection',
        type: 'POST',
        traditional: true,
        data: {
            PageSectionID: PageSectionID,
            VideoID: window["ComposePageVideoComboBox" + PageSectionID].GetValue()
        },
        success: function (data) {
            $('#video-section-' + PageSectionID).html(data);
            SuccessFade($("#video-section-" + PageSectionID + "-save"));
        },
        error: function (e, s, a) {
            FailureFade($("#video-section-" + PageSectionID + "-save"));
        },
        timeout: 20000
    });
}

function LoadAnimationEditor(AnimationID) {
    $("#animation-edit-container").empty();
    LoadingPanel.Show();
    $.get('/admin/animations/edit', { AnimationID: AnimationID }, function (data, status, jqxhr) {
        $("#animation-edit-container").html(data);
        let m = bootstrap.Modal.getOrCreateInstance(document.querySelector("#animation-modal"), {
            backdrop: false
        });
        m._element.style.width = 'auto';
        m._element.style.height = 'auto';
        LoadingPanel.Hide();
        m.show();
        $("#animation-modal").draggable({
            handle: ".modal-header"
        }); 
    });
}

function SaveAnimationSection(PageSectionID) {
    $.ajax({
        url: '/admin/cms/SaveAnimationPageSection',
        type: 'POST',
        traditional: true,
        data: {
            PageSectionID: PageSectionID,
            AnimationID: window["ComposePageAnimationComboBox" + PageSectionID].GetValue()
        },
        success: function (data) {
            $('#page-section-' + PageSectionID).html(data);
            SuccessFade($("#page-section-" + PageSectionID + "-save"));
        },
        error: function (e, s, a) {
            FailureFade($("#page-section-" + PageSectionID + "-save"));
        },
        timeout: 20000
    });
}

function AddPageSection(Position) {
    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/AddPageSection',
        type: 'POST',
        traditional: true,
        data: {
            PageID: Number($("#selected-page-id").val()),
            PageSectionTypeID: $("#page-section-type-select").val(),
            SlideshowHeight: $("#slideshow-height-select").val(),
            AnimationHeight: $("#animation-height-select").val(),
            FormType: $("#form-type-select").val(),
            Position: Position,
            BannerLocationGroup: $("#banner-location-group").val()
        },
        success: function (data) {
            LoadingPanel.Hide();
            $("#page-sections-container").html(data);
            RefreshPageOptions();

        },
        error: function (e, s, a) {
            LoadingPanel.Hide();
            alert(e.responseText);
        },
        timeout: 20000
    });
}

function RemovePageSection(PageSectionID) {
    var confirm = window.confirm("Are you sure?");
    if (confirm) {
        $.ajax({
            url: '/admin/cms/RemovePageSection',
            type: 'POST',
            traditional: true,
            data: {
                PageID: Number($("#selected-page-id").val()),
                PageSectionID: PageSectionID
            },
            success: function (data) {
                $("#page-sections-container").html(data);
                RefreshPageOptions();
            },
            error: function (e, s, a) {
                alert(e.responseText);
            },
            timeout: 20000
        });
    }
}

function ShowSecondarySelect() {
    var sel = $("#page-section-type-select").val();

    $(".secondary-select").fadeOut('fast');

    if (sel == "2")
        $("#slideshow-height-select").fadeIn("fast");
    else if (sel == "3")
            ;
        //$("#form-type-select").fadeIn("fast");
    else if (sel == "8")
        $("#banner-location-group").fadeIn("fast");
    else if (sel == "12")
        $("#animation-height-select").fadeIn('fast');
}

function RemoveImageFromPagePods(PageID, ImageID) {
    var count = $("div.pod-image").length;
    if (count == 1) {
        alert("You may not remove the final pod image from a page.");
        return false;
    }
    else {
        var confirm = window.confirm("Are you sure?");
        if (confirm) {

            $("#pod-list").find('[data-image-id=' + ImageID + ']').remove();

            $("#pod-list").sortable("refresh");
            window.podImages = new Array();
            if ($("#pod-image-placeholder").length > 0)
                $("#pod-image-placeholder").remove();
            $("#pod-list").children("div").each(function (index) {
                window.podImages.push({ ID: $(this).data("image-id"), SortOrder: index });
            });
            UpdatePodImages();
        }
    }
}

function RemoveImageFromSlideshow(PageSectionID, SlideshowID, ImageID) {
    var count = $("#page-section-" + PageSectionID + "-slideshow-images").data("images-count");
    if (count == 1) {
        alert("You may not remove the only image from a slideshow.");
        return false;
    }
    else {
        var confirm = window.confirm("Are you sure?");
        if (confirm) {
            $("#page-section-" + PageSectionID + "-slideshow-images").find('[data-image-id=' + ImageID + ']').remove();
            UpdateSlideshowImages(PageSectionID, SlideshowID);
        }
    }
}

function UpdateSlideshowImages(PageSectionID, SlideshowID) {
    var slideshowImages = new Array();

    $("#page-section-" + PageSectionID + "-slideshow-images").children("div").each(function (index) {
        slideshowImages.push({ ID: $(this).data("image-id"), SortOrder: index });
    });

    $.ajax({
        url: '/admin/cms/UpdateSlideshowImages',
        type: 'POST',
        traditional: true,
        data: { PageSectionID: PageSectionID, SlideshowID: SlideshowID, Images: JSON.stringify(slideshowImages) },
        success: function (data) {
            if (typeof (window.HotspotsConfigured) != "undefined" && typeof (window.HotspotsConfigured) != false)
                if (window.HotspotsConfigured.hasOwnProperty("PageSection" + PageSectionID))
                    delete window.HotspotsConfigured["PageSection" + PageSectionID];
            $("#page-section-" + PageSectionID).html(data);
        },
        error: function (e, s, a) {
            if (typeof (e.responseText) != 'undefined' && e.responseText != false)
                alert(e.responseText);
            else
                alert(e);
        },
        timeout: 20000
    });
}

function RemoveProductPageFromGrid(PageSectionID, ProductPageID) {
    // check count of images, refuse removal of final image.
    //var count = $("#page-section-" + PageSectionID + "-grid").children("div").not(".product-page-placeholder").length;
    //if (count == 1) {
    //    $.info("Warning", "You may not remove the final item from a grid.");
    //    return false;
    //}
    //else
    {
        // alert on N selected for deletion?

        var mult = $("#page-section-" + PageSectionID + "-grid").find(".multisortable-selected").length;

        if (mult > 0)
            msg = mult + " selected for deletion.";

        $.confirm(function () {
            $("#page-section-" + PageSectionID + "-grid").find(".multisortable-selected").remove();
            UpdateProductPageGrid(PageSectionID);
        }, function () {

        }, [], msg);

        //var confirm = window.confirm("Are you sure?");
        //if (confirm) {
        //    $("#page-section-" + PageSectionID + "-grid").find('[data-product-page-id=' + ProductPageID + ']').remove();
        //    UpdateProductPageGrid(PageSectionID);
        //}
    }
}

function RemoveArticleFromGrid(PageSectionID, ArticleID) {
    // check count of images, refuse removal of final image.
    var count = $("#page-section-" + PageSectionID + "-grid").children("div").length;
    if (count == 1) {
        alert("You may not remove the final item from a grid.");
        return false;
    }
    else {
        var confirm = window.confirm("Are you sure?");
        if (confirm) {
            $("#page-section-" + PageSectionID + "-grid").find('[data-article-id=' + ArticleID + ']').remove();
            UpdateArticleGrid(PageSectionID);
        }
    }
}

function RemoveCollectionFromGrid(PageSectionID, CollectionID) {
    // check count of images, refuse removal of final image.
    var count = $("#page-section-" + PageSectionID + "-grid").children("div").length;
    if (count == 1) {
        alert("You may not remove the final item from a grid.");
        return false;
    }
    else {
        var confirm = window.confirm("Are you sure?");
        if (confirm) {
            $("#page-section-" + PageSectionID + "-grid").find('[data-collection-id=' + CollectionID + ']').remove();
            UpdateCollectionGrid(PageSectionID);
        }
    }
}

function RemovePageFromGrid(PageSectionID, PageID) {
    // check count of images, refuse removal of final image.
    var count = $("#page-section-" + PageSectionID + "-grid").children("div").length;
    if (count == 1) {
        alert("You may not remove the final item from a grid.");
        return false;
    }
    else {
        var confirm = window.confirm("Are you sure?");
        if (confirm) {
            $("#page-section-" + PageSectionID + "-grid").find('[data-page-id=' + PageID + ']').remove();
            UpdatePageGrid(PageSectionID);
        }
    }
}

function UpdatePageGrid(PageSectionID) {
    var pages = new Array();

    $("#page-section-" + PageSectionID + "-grid").find(".page-placeholder").remove();

    $("#page-section-" + PageSectionID + "-grid").children("div").each(function (index) {
        pages.push({ ID: $(this).data("page-id"), SortOrder: index });
    });

    $.ajax({
        url: '/admin/cms/UpdatePageGrid',
        type: 'POST',
        traditional: true,
        data: { PageSectionID: PageSectionID, Pages: JSON.stringify(pages) },
        success: function (data) {
            $("#page-section-" + PageSectionID + "-grid").html(data);
        },
        timeout: 20000
    });
}

function UpdateArticleGrid(PageSectionID) {
    var articles = new Array();

    $("#page-section-" + PageSectionID + "-grid").find(".article-placeholder").remove();

    $("#page-section-" + PageSectionID + "-grid").children("div").each(function (index) {
        articles.push({ ID: $(this).data("article-id"), SortOrder: index });
    });

    $.ajax({
        url: '/admin/cms/UpdateArticleGrid',
        type: 'POST',
        traditional: true,
        data: { PageSectionID: PageSectionID, Articles: JSON.stringify(articles) },
        success: function (data) {
            $("#page-section-" + PageSectionID + "-grid").html(data);
        },
        timeout: 20000
    });
}

function UpdateProductPageGrid(PageSectionID, onComplete) {

    var productPages = new Array();

    $("#page-section-" + PageSectionID + "-grid").find(".product-page-placeholder").remove();

    $("#page-section-" + PageSectionID + "-grid").children("div").each(function (index) {
        productPages.push({ ID: $(this).data("product-page-id"), SortOrder: index });
    });

    LoadingPanel.Show();

    $.ajax({
        url: '/admin/cms/UpdateProductPageGrid',
        type: 'POST',
        traditional: true,
        async: true,
        data: { PageSectionID: PageSectionID, ProductPages: JSON.stringify(productPages) },
        success: function (data) {
            $("#page-section-" + PageSectionID + "-grid").html(data);
            if (window.ignoreUpdate = PageSectionID) {
                window.ignoreUpdate = null;
            }
            LoadingPanel.Hide();
            if (typeof onComplete === 'function')
                onComplete();
        },
        error: function (e, s, a) {
            LoadingPanel.Hide();
            alert(e.responseText);
        },
        timeout: 20000
    });
}

function UpdateCollectionGrid(PageSectionID) {
    var collections = new Array();

    LoadingPanel.Show();

    $("#page-section-" + PageSectionID + "-grid").find(".collection-placeholder").remove();

    $("#page-section-" + PageSectionID + "-grid").children("div").each(function (index) {
        collections.push({ ID: $(this).data("collection-id"), SortOrder: index });
    });

    $.ajax({
        url: '/admin/cms/UpdateCollectionGrid',
        type: 'POST',
        traditional: true,
        data: { PageSectionID: PageSectionID, Collections: JSON.stringify(collections) },
        success: function (data) {
            $("#page-section-" + PageSectionID + "-grid").html(data);

            LoadingPanel.Hide();
        },
        error: function (e, s, a) {
            LoadingPanel.Hide();
            alert(e.responseText);
        },
        timeout: 20000
    });
}

function ConfigureHotspots(PageSectionID) {
    // need to create a hashmap of pagesectionid -> hotspotsconfigured bools... or do we... yes, yes we do.

    if (typeof (window.HotspotsConfigured) == "undefined" || typeof (window.HotspotsConfigured) == false)
        window.HotspotsConfigured = {};


    if (!window.HotspotsConfigured.hasOwnProperty("PageSection" + PageSectionID.toString())) {
        $(".hotspot").mouseenter(function () {

            $(".hotspot-reveal[data-id=" + $(this).data('id') + "]").data('held', 'true');

            var reveal = $(".hotspot-reveal[data-id=" + $(this).data("id") + "]").detach();

            clearTimeout($(reveal).data('timeoutId'));

            $(reveal).appendTo($("#page-section-" + PageSectionID + "-slideshow-images"));
            $(reveal).css("position", "absolute");
            $(reveal).stop(true).fadeIn("slow");
            $(reveal).position({
                my: "left top",
                at: "bottom right",
                of: $(this)
            });

            $(reveal).mouseenter(function () {
                $(".hotspot-reveal[data-id=" + $(this).data('id') + "]").data('held', 'true');
                //$(".hotspot-reveal").animate("opacity", "0.5", "linear", function () { })
                $(".hotspot-reveal").css("z-index", "1000000");
                $(this).css("z-index", "1000001");
                $(".hotspot").css("display", "block");
            });

            $(reveal).mouseleave(function () {
                $(".hotspot-reveal[data-id=" + $(this).data('id') + "]").data('held', 'false');
                var hotspot = $(this),
                    timeoutId = setTimeout(function () {
                        if ($(".hotspot-reveal[data-id=" + $(hotspot).data('id') + "]").data('held') == 'false')
                            $(".hotspot-reveal[data-id=" + $(hotspot).data('id') + "]").stop(true).fadeOut("slow");
                    }, 2000);
                $(hotspot).data('timeoutId', timeoutId);
            });
        });

        $(".hotspot").mouseleave(function () {
            var hotspot = $(this);
            $(".hotspot-reveal[data-id=" + $(hotspot).data('id') + "]").data('held', 'false');
            var timeoutId = setTimeout(function () {
                if ($(".hotspot-reveal[data-id=" + $(hotspot).data('id') + "]").data('held') == 'false')
                    $(".hotspot-reveal[data-id=" + $(hotspot).data('id') + "]").stop(true).fadeOut("slow");
            }, 2000);
            $(hotspot).data('timeoutId', timeoutId);
        })

        $(".hotspot-reveal").mouseenter(function () {
            $(".hotspot-reveal").css("z-index", "1000000");
            $(this).css("z-index", "1000001");
        });

        window.HotspotsConfigured["PageSection" + PageSectionID.toString()] = "true";

    }

}

function BulkAddProductPages() {
    // must prompt for potential targets...
    var count = $(".product-page-grid-container").length;
    if (count == 0) {
        alert("No target found. Please add a Product Page Grid page section.");
        return false;
    }
    else if (count == 1) {
        //AutoSelectProductPageForBulk()

        var newOptions = {};

        $(".product-page-grid-container").each(function () {
            newOptions["Product Grid Section ID: " + $(this).data("page-section-id") + " - '" + $(this).data("page-section-name") + "'"] = $(this).data("page-section-id");
        });

        $('#product-page-section-select option').remove();
        var $el = $("#product-page-section-select");
        $.each(newOptions, function (key, value) {
            $el.append($("<option></option>")
                .attr("value", value).prop("selected", "selected").text(key));
        });

        $("#product-page-section-selector-dialog").dialog("open");
    }
    else {
        var newOptions = {};

        $(".product-page-grid-container").each(function () {
            newOptions["Product Grid Section ID: " + $(this).data("page-section-id") + " - '" + $(this).data("page-section-name") + "'"] = $(this).data("page-section-id");
        });

        $('#product-page-section-select option').remove();   //:gt(0)').remove(); // remove old options except 0th - WHY EXCEPT 0th?!
        var $el = $("#product-page-section-select");
        $.each(newOptions, function (key, value) {
            $el.append($("<option></option>")
                .attr("value", value).text(key));
        });

        $("#product-page-section-selector-dialog").dialog("open");
    }
}

function BulkAddCollections() {
    // must prompt for potential targets...
    var count = $(".collection-grid-container").length;
    if (count == 0) {
        alert("No target found. Please add a Collection Grid page section.");
        return false;
    }
    else if (count == 1) {
        var newOptions = {};

        $(".collection-grid-container").each(function () {
            newOptions["Collection Grid Section ID: " + $(this).data("page-section-id") + " - '" + $(this).data("page-section-name") + "'"] = $(this).data("page-section-id");
        });

        $('#collection-section-select option').remove();
        var $el = $("#collection-section-select");
        $.each(newOptions, function (key, value) {
            $el.append($("<option></option>")
                .attr("value", value).prop("selected", "selected").text(key));
        });

        $("#collection-section-selector-dialog").dialog("open");
    }
    else {
        var newOptions = {};

        $(".collection-grid-container").each(function () {
            newOptions["Collection Grid Section ID: " + $(this).data("page-section-id") + " - '" + $(this).data("page-section-name") + "'"] = $(this).data("page-section-id");
        });

        $('#collection-section-select option').remove(); //:gt(0)').remove(); // remove old options except 0th
        var $el = $("#collection-section-select");
        $.each(newOptions, function (key, value) {
            $el.append($("<option></option>")
                .attr("value", value).text(key));
        });

        $("#collection-section-selector-dialog").dialog("open");
    }
}

function BulkAddPages() {
    // must prompt for potential targets...
    var count = $(".page-grid-container").length;
    if (count == 0) {
        alert("No target found. Please add a Page Grid page section.");
        return false;
    }
    else if (count == 1) {
        var newOptions = {};

        $(".page-grid-container").each(function () {
            newOptions["Page Grid Section ID: " + $(this).data("page-section-id") + " - '" + $(this).data("page-section-name") + "'"] = $(this).data("page-section-id");
        });

        $('#page-section-select option').remove();
        var $el = $("#page-section-select");
        $.each(newOptions, function (key, value) {
            $el.append($("<option></option>")
                .attr("value", value).prop("selected", "selected").text(key));
        });

        $("#page-section-selector-dialog").dialog("open");
    }
    else {
        var newOptions = {};

        $(".page-grid-container").each(function () {
            newOptions["Page Grid Section ID: " + $(this).data("page-section-id") + " - '" + $(this).data("page-section-name") + "'"] = $(this).data("page-section-id");
        });

        $('#page-section-select option');   //:gt(0)').remove(); // remove old options except 0th
        var $el = $("#page-section-select");
        $.each(newOptions, function (key, value) {
            $el.append($("<option></option>")
                .attr("value", value).text(key));
        });

        $("#page-section-selector-dialog").dialog("open");
    }
}

function SelectProductPageForBulk() {

    if ($("#page-section-" + $("#product-page-section-select").val() + "-grid").length > 0) {

        var productPages = new Array();

        var bulkPagesToAdd = new Array();
        $('.productpage').each(function (index) {
            bulkPagesToAdd.push({ ID: $(this).data('product-page-id'), SortOrder: index });
        });

        //$('.product-page-cell').each(function (index) {
        //    bulkPagesToAdd.push({ ID: $(this).data('product-page-id'), SortOrder: index });
        //});

        $("#page-section-" + $("#product-page-section-select").val() + "-grid").find(".product-page-placeholder").remove();

        $("#page-section-" + $("#product-page-section-select").val() + "-grid").children("div").each(function (index) {
            productPages.push({ ID: $(this).data("product-page-id"), SortOrder: index });
        });

        var insertionPoint = $("#add-where-select").val();
        if (insertionPoint == 0) {
            var appendSortOrder = bulkPagesToAdd.length; // + index

            for (var i = 0; i < productPages.length; i++) {
                productPages[i].SortOrder = productPages[i].SortOrder + appendSortOrder;
            }
        }
        else {
            var appendSortOrder = productPages.length; // + index

            for (var i = 0; i < bulkPagesToAdd.length; i++) {
                bulkPagesToAdd[i].SortOrder = bulkPagesToAdd[i].SortOrder + appendSortOrder;
            }
        }



        productPages = productPages.concat(bulkPagesToAdd);

        var conf = window.confirm("About to commit: " + productPages.length + " product pages. Continue?");

        if (conf) {
            $.ajax({
                url: '/admin/cms/UpdateProductPageGrid',
                type: 'POST',
                traditional: true,
                data: { PageSectionID: $("#product-page-section-select").val(), ProductPages: JSON.stringify(productPages) },
                success: function (data) {
                    $("#page-section-" + $("#product-page-section-select").val() + "-grid").html(data);
                },
                error: function (e, s, a) {
                    alert(e.responseText);
                },
                timeout: 20000
            });
        }
    }
    else {
        alert("PageSection NOT FOUND ON PAGE.");
    }
}

function SelectCollectionForBulk() {

    var collections = new Array();

    var bulkCollectionsToAdd = new Array();
    $('.collection').each(function (index) {
        bulkCollectionsToAdd.push({ ID: $(this).data('collection-id'), SortOrder: index });
    });

    $("#page-section-" + $("#collection-section-select").val() + "-grid").find(".collection-placeholder").remove();

    $("#page-section-" + $("#collection-section-select").val() + "-grid").children("div").each(function (index) {
        collections.push({ ID: $(this).data("collection-id"), SortOrder: index });
    });

    var insertionPoint = $("#collection-add-where-select").val();
    if (insertionPoint == 0) {
        var appendSortOrder = bulkCollectionsToAdd.length; // + index

        for (var i = 0; i < collections.length; i++) {
            collections[i].SortOrder = collections[i].SortOrder + appendSortOrder;
        }
    }
    else {
        var appendSortOrder = collections.length; // + index

        for (var i = 0; i < bulkCollectionsToAdd.length; i++) {
            bulkCollectionsToAdd[i].SortOrder = bulkCollectionsToAdd[i].SortOrder + appendSortOrder;
        }
    }

    collections = collections.concat(bulkCollectionsToAdd);

    $.ajax({
        url: '/admin/cms/UpdateCollectionGrid',
        type: 'POST',
        traditional: true,
        data: { PageSectionID: $("#collection-section-select").val(), Collections: JSON.stringify(collections) },
        success: function (data) {
            $("#page-section-" + $("#collection-section-select").val() + "-grid").html(data);
        },
        error: function (e, s, a) {
            alert(e.responseText);
        },
        timeout: 20000
    });
}

function SelectPageForBulk() {

    var pages = new Array();

    var bulkPagesToAdd = new Array();
    $('.page').each(function (index) {
        bulkPagesToAdd.push({ ID: $(this).data('page-id'), SortOrder: index });
    });

    $("#page-section-" + $("#page-section-select").val() + "-grid").find(".page-placeholder").remove();

    $("#page-section-" + $("#page-section-select").val() + "-grid").children("div").each(function (index) {
        pages.push({ ID: $(this).data("page-id"), SortOrder: index });
    });

    var insertionPoint = $("#page-add-where-select").val();
    if (insertionPoint == 0) {
        var appendSortOrder = bulkPagesToAdd.length; // + index

        for (var i = 0; i < pages.length; i++) {
            pages[i].SortOrder = pages[i].SortOrder + appendSortOrder;
        }
    }
    else {
        var appendSortOrder = pages.length; // + index

        for (var i = 0; i < bulkPagesToAdd.length; i++) {
            bulkPagesToAdd[i].SortOrder = bulkPagesToAdd[i].SortOrder + appendSortOrder;
        }
    }

    pages = pages.concat(bulkPagesToAdd);

    $.ajax({
        url: '/admin/cms/UpdatePageGrid',
        type: 'POST',
        traditional: true,
        data: { PageSectionID: $("#page-section-select").val(), Pages: JSON.stringify(pages) },
        success: function (data) {
            $("#page-section-" + $("#page-section-select").val() + "-grid").html(data);
        },
        timeout: 20000
    });
}

function UpdateFormFailure(data, status, args) {
    //alert(data.responseText);

    var resp = $(data.responseText);
    $("#page-section-" + $(resp).data("pagesection-id") + "-update-target").html(data.responseText);
}

function UpdateFormSuccess(data, status, args) {
    var resp = $(data.responseText);
    $("#page-section-" + $(resp).data("pagesection-id") + "-update-target").html(data.responseText);
    //$("#details-save-success").fadeIn("fast", function (ev, ui) {
    //    var timer = window.setTimeout(function () { $("#details-save-success").fadeOut("fast") }, 2000);
    //});
}

function SaveForm(PageSectionID) {
    $.ajax({
        url: '/admin/cms/updateform',//'@Url.Action("UpdateForm", "CMS")',
        type: 'POST',
        traditional: true,
        data: {
            PageSectionID: PageSectionID,
            FormID: $("#page-section-" + PageSectionID).find("input[name='FormID']").val(),
            FormType: $("#page-section-" + PageSectionID).find("input[name='FormType']").val(),
            PageSectionName: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "PageSectionName']").val(),
            OnDate: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "OnDate']").val(),
            OffDate: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "OffDate']").val(),
            DisplayTitle: $("#page-section-" + PageSectionID).find("input[name='PageSection" + PageSectionID + "DisplayTitle']").val(),
            DescriptionText: $("#page-section-" + PageSectionID).find("textarea[name='PageSection" + PageSectionID + "DescriptionText']").val(),
            MarketingPartnerID: $("#page-section-" + PageSectionID).find("input[name='MarketingPartnerID']").val(),
            MarketingPartnerText: $("#page-section-" + PageSectionID).find("textarea[name='PageSection" + PageSectionID + "MarketingPartnerText']").val(),
        },
        success: function (data) {
            $("#page-section-" + PageSectionID + "-update-target").html(data);
            SuccessFade($("#form-" + PageSectionID + "-save-success"));
            //$("#form-" + PageSectionID + "-save-success").fadeIn("fast", function (ev, ui) {
            //    var timer = window.setTimeout(function () { $("#form-" + PageSectionID + "-save-success").fadeOut("fast") }, 2000);
            //});
        },
        error: function (e, s, a) {
            $("#page-section-" + PageSectionID + "-update-target").html(e.responseText);
            FailureFade($("#form-" + PageSectionID + "-save-success"));
        },
        timeout: 20000
    });
}

function saveHTMLSection(PageSectionID) {
    $.ajax({
        url: '/admin/cms/HTMLPageSectionSave',
        type: 'POST',
        dataType: 'json',
        data: {
            HTMLSectionID: window["PageSection" + PageSectionID + "HTMLEditor"].HTMLSectionID,
            Contents: window["PageSection" + PageSectionID + "HTMLEditor"].getValue()
        }
    });
}

function SavePageSectionDetails(PageSectionID, PageSectionTypeID) {

    var data = {
        PageSectionID: PageSectionID,
        PageSectionTypeID: PageSectionTypeID,
        PageSectionName: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "PageSectionName']").val(),
        Heading: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "PageSectionHeading']").val(),
        IsHeader: window["PageSection" + PageSectionID + "IsHeader"].GetValue(),
        HalfWidth: window["PageSection" + PageSectionID + "HalfWidth"].GetValue(),
        PlatformSwitch: window["PageSection" + PageSectionID + "PlatformSwitch"].GetValue(),
        TradeSwitch: window["PageSection" + PageSectionID + "TradeSwitch"].GetValue(),
        TradePartnerAvailability: window["PageSection" + PageSectionID + "TradePartnerAvailability"].GetValue(),
        OnDate: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "OnDate']").val(),
        OffDate: $("#page-section-" + PageSectionID + "-header-details").find("input[name='PageSection" + PageSectionID + "OffDate']").val()
    };

    $.ajax({
        url: '/admin/cms/SavePageSectionHeaderDetails',
        type: 'POST',
        traditional: true,
        data: data,
        success: function (data) {
            $("#page-section-" + PageSectionID + "-header-details").html(data);
            SuccessFade($("#page-section-" + PageSectionID + "-save"));
            //$("#page-section-" + PageSectionID + "-save").animate({ opacity: '0.0' }, 300, 'linear', function () {
            //    $(this).attr('src', '/images/icons/32/save-success.png').bind('onreadystatechange load', function () {
            //        $(this).unbind('onreadystatechange load');
            //        if (this.complete)
            //            $(this).animate({ opacity: '1.0' }, 300, 'linear', function () {
            //                window.setTimeout(function () {
            //                    $("#page-section-" + PageSectionID + "-save").animate({ opacity: '0.0' }, 1000, 'linear', function () {
            //                        $(this).attr('src', '/images/icons/32/save.png').bind('onreadystatechange load', function () {
            //                            $(this).unbind('onreadystatechange load');
            //                            $(this).animate({ opacity: '1.0' }, 300, 'linear');
            //                        });
            //                    });
            //                }, 2000);
            //            });
            //    });
            //});
        },
        error: function (e, s, a) {
            $("#page-section-" + PageSectionID + "-header-details").html(e.responseText);
            FailureFade($("#page-section-" + PageSectionID + "-save"));
            //$("#page-section-" + PageSectionID + "-save").animate({ opacity: '0.0' }, 300, 'linear', function () {
            //    $(this).attr('src', '/images/icons/32/save-failed.png').bind('onreadystatechange load', function () {
            //        $(this).unbind('onreadystatechange load');
            //        if (this.complete)
            //            $(this).animate({ opacity: '1.0' }, 300, 'linear', function () {
            //                window.setTimeout(function () {
            //                    $("#page-section-" + PageSectionID + "-save").animate({ opacity: '0.0' }, 1000, 'linear', function () {
            //                        $(this).attr('src', '/images/icons/32/save.png').bind('onreadystatechange load', function () {
            //                            $(this).unbind('onreadystatechange load');
            //                            $(this).animate({ opacity: '1.0' }, 300, 'linear');
            //                        });
            //                    });
            //                }, 2000);
            //            });
            //    });
            //});
        },
        timeout: 20000
    });
}

function SavePageDetails() {
    URLKeywords.Validate();
    if (URLKeywords.GetIsValid()) {
        $.ajax({
            url: '/admin/cms/SavePageDetails',
            type: 'POST',
            traditional: true,
            data: {
                PageID: Number($("#selected-page-id").val()),
                TitleTag: TitleTag.GetValue(),
                URLKeywords: URLKeywords.GetValue(),
                MetaDescription: MetaDescription.GetValue(),
                Heading: Heading.GetValue(),
                SubHeading: SubHeading.GetValue(),
                Description: Description.GetValue()
            },
            success: function (data) {
                $("#page-detail-container").html(data);
                SuccessFade($("#page-details-save-success"));
                //$("#page-details-save-success").fadeIn("fast", function (ev, ui) {
                //    var timer = window.setTimeout(function () { $("#page-details-save-success").fadeOut("fast") }, 2000);
                //});
            },
            error: function (e, s, a) {
                $("#page-detail-container").html(e.responseText);
                FailureFade($("#page-details-save-success"));
            },
            timeout: 20000
        });
    }
}

function OpenAddPageSectionDialog() {
    $('#add-page-section-position').val('0');
    $('#banner-location-group').val('0');
    $('#banner-location-group').hide();
    $('#slideshow-height-select').val('0');
    $('#slideshow-height-select').hide();
    $('#animation-height-select').val('0');
    $('#animation-height-select').hide();
    $('#page-section-type-select').val('0');
    $('#add-page-section-type-dialog').dialog('open');
}

function AlterSlideshowOverlay(PageSectionID) {
    var ele = $("#slideshow-" + PageSectionID + "-overlay-position :selected");
    var targ = $("#page-section-" + PageSectionID + "-advanced-settings").find(".slideshow-overlay").first();

    window["SlideShow" + PageSectionID + "OverlayTop"].SetValue(ele.data("top") + '%');
    window["SlideShow" + PageSectionID + "OverlayLeft"].SetValue(ele.data("left") + '%');
    window["SlideShow" + PageSectionID + "OverlayWidth"].SetValue(ele.data("width") + '%');
    window["SlideShow" + PageSectionID + "OverlayHeight"].SetValue(ele.data("height") + '%');

    targ.css("top", ele.data("top") + '%');
    targ.css("left", ele.data("left") + '%');
    targ.css("width", ele.data("width") + '%');
    targ.css("height", ele.data("height") + '%');
}

function InitPodDragDrop() {

    $(".page-cell").droppable({
        accept: ".ar2",
        activeClass: "ui-state-default",
        drop: function (e, u) {
            OverridePageImage(e, u, $(this).data('page-section-id'), $(this).data('page-id'));
        }
    });

    $('.ar2').draggable({
        helper: 'clone',
        start: function (e, ui) {
            $(ui.helper).css("z-index", "300000");
        },
        connectToSortable: '#pod-list, .page-cell'
    });

    $("#pod-list").sortable({
        helper: 'clone',
        placeholder: "ui-state-highlight-cms-page-pod-feature",
        connectWith: '.ar2',
        update: function (ev, ui) {
            //if ($(ui.item).hasClass("draggable")) {
            $("#pod-list").sortable("refresh");
            window.podImages = new Array();
            if ($(".pod-image-placeholder").length > 0)
                $(".pod-image-placeholder").remove();
            $("#pod-list").children("div").each(function (index) {
                window.podImages.push({ ID: $(this).data("image-id"), SortOrder: index });
            });
            UpdatePodImages();
            //}
        },
        scroll: false
    });
}

function UpdatePodImages() {
    $.ajax({
        url: '/admin/cms/UpdatePagePodImages',
        traditional: true,
        type: "POST",
        data: {
            PageID: Number($("#selected-page-id").val()),
            Images: JSON.stringify(window.podImages),
            TemplateImageID: 1
        },
        success: function (data) {
            $("#pod-busy").fadeOut("fast");
            $('#pod-list').html(data)
            return false;
        },
        error: function (e, s, a) {
            $("#pod-busy").fadeOut("fast");
            alert(e.responseText);
            return false;
        },
        timeout: 20000
    });
}

function OverridePageImage(ev, ui, pageSectionID, pageID) {
    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/overridepageimage',//'@Url.Action("OverridePageImage", "CMS")',
        type: 'POST',
        traditional: true,
        data: {
            pageSectionID: pageSectionID,
            pageID: pageID,
            imageID: $(ui.draggable).data("image-id")
        },
        success: function (data) {
            LoadingPanel.Hide();
            $("#page-section-" + pageSectionID + "-grid").html(data);
            InitPodDragDrop();
        },
        timeout: 20000
    });
}

function RemovePageImageOverride(pageSectionID, pageID) {
    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/RemovePageImageOverride',
        type: 'POST',
        traditional: true,
        data: {
            pageSectionID: pageSectionID,
            pageID: pageID
        },
        success: function (data) {
            LoadingPanel.Hide();
            $("#page-section-" + pageSectionID + "-grid").html(data);
            InitPodDragDrop();
        },
        timeout: 20000
    });
}

function InitCountryDragDrop() {
    $("#country-restriction-section").sortable({
        helper: "clone",
        connectWith: ".country-restriction-flag",
        placeholder: ".ui-state-highlight-country-restriction",
        update: function (ev, ui) {
            if ($(ui.item).hasClass('country-flag')) {
                $("#country-restriction-section").sortable("refresh");
                window.CountryRestrictions = new Array();
                if ($('.page-country-restriction-placeholder').length > 0)
                    $('.page-country-restriction-placeholder').remove();
                $("#country-restriction-section").children("div").each(function (index) {
                    window.CountryRestrictions.push({ ID: $(this).data("country-id"), SortOrder: 0 });
                });
                UpdatePageCountryRestrictions();
            }
        },
        scroll: false
    });

    $(".country-flag").draggable({
        helper: "clone",
        start: function (e, ui) {
            $(ui.helper).css("z-index", "300000");
        },
        connectToSortable: "#country-restriction-section"
    });
}

function InitCountryLanguageDragDrop() {
    $(".country-language-restriction-section").sortable({
        helper: "clone",
        connectWith: ".country-language-restriction-flag",
        placeholder: ".ui-state-highlight-country-restriction",
        update: function (ev, ui) {
            if ($(ui.item).hasClass('country-language-flag')) {
                $(".country-language-restriction-section").sortable("refresh");
                var countryLanguageRestrictions = new Array();

                $(this).find(".country-restriction-placeholder").first().remove();
                $(this).children("div").each(function (index) {
                    countryLanguageRestrictions.push({
                        countryId: $(this).data("country-id"),
                        languageId: $(this).data("language-id"),
                        sortOrder: 0
                    });
                });
                var targ = $(this).parent();
                $.ajax({
                    url: '/admin/cms/UpdatePageSectionCountryRestrictions',
                    type: "POST",
                    traditional: true,
                    data: { pageSectionID: $(this).parent().data("page-section-id"), countryLanguageRestrictions: JSON.stringify(countryLanguageRestrictions) },
                    success: function (data) {
                        $(targ).html(data);
                    },
                    timeout: 20000
                });
            }
        },
        scroll: false
    })

    $(".country-language-flag").draggable({
        helper: "clone",
        start: function (e, ui) {
            $(ui.helper).css("z-index", "300000");
        },
        connectToSortable: ".country-language-restriction-section"
    });
}

function UpdatePageSectionCountryRestrictions(container) {
    $("#add-all-country-language-dialog").dialog("close");
    $(".country-language-restriction-section").sortable("refresh");
    var countryLanguageRestrictions = new Array();

    $(container).find(".country-restriction-placeholder").first().remove();
    $(container).children(0).children("div").each(function (index) {
        countryLanguageRestrictions.push({ CountryID: $(this).data("country-id"), LanguageID: $(this).data("language-id"), SortOrder: 0 });
    });

    var targ = container;

    $.ajax({
        url: '/admin/cms/UpdatePageSectionCountryRestrictions',
        type: "POST",
        traditional: true,
        data: { pageSectionID: targ.data("page-section-id"), countryLanguageRestrictions: JSON.stringify(countryLanguageRestrictions) },
        success: function (data) {
            $(targ).html(data);
        },
        timeout: 20000
    });
}

function UpdatePageCountryRestrictions() {
    $("#country-restriction-busy").fadeIn("fast");
    $.ajax({
        url: '/admin/cms/UpdatePageCountryRestrictions',
        type: "POST",
        traditional: true,
        data: { PageID: Number($("#selected-page-id").val()), Countries: JSON.stringify(window.CountryRestrictions) },
        success: function (data) {
            $("#country-restriction-busy").fadeOut("fast");
            $("#page-country-restrictions-container").html(data);
        },
        error: function (e, s, a) {
            $("#country-restriction-busy").fadeOut("fast");
            alert(e.responseText);
        },
        timeout: 20000
    });
}

function RemoveCountryFromRestriction() {
    $("#country-restriction-section").sortable("refresh");
    window.CountryRestrictions = new Array();
    if ($("#country-restriction-placeholder").length > 0)
        $("#country-restriction-placeholder").remove();
    $("#country-restriction-section").children("div.country-restriction-flag").each(function (index) {
        window.CountryRestrictions.push({ ID: $(this).data("country-id"), SortOrder: 0 });
    });
    UpdateCountryRestrictions();
}

function RemoveAllCountryRestrictions() {
    var confirm = window.confirm("Are you sure?");
    if (confirm) {
        window.CountryRestrictions = new Array();
        UpdateCountryRestrictions();
    }
}

function UpdateCountryRestrictions() {
    $("#add-all-country-dialog").dialog("close");
    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/updatepagecountryrestrictions',//'@Url.Action("UpdatePageCountryRestrictions", "CMS")',
        type: "POST",
        traditional: true,
        data: { PageID: $("#selected-page-id").val(), Countries: JSON.stringify(window.CountryRestrictions) },
        success: function (data) {
            $("#page-country-restrictions-container").html(data);
            LoadingPanel.Hide();
        },
        timeout: 20000
    });
}

function InitSlideshowDragDrop() {

    for (var i = 1; i < 11; i++) { // ten aspect ratios...
        var ar = "ar" + i.toString();

        var connector = '.slideshow-list-' + ar + ',  #animation-slides-container';

        if (i == 2)
            connector = '.slideshow-list-' + ar + ', #pod-list, #animation-slides-container';

        $('.' + ar).draggable({
            helper: 'clone',
            start: function (e, ui) {
                $(ui.helper).css("z-index", "300000");
            },
            connectToSortable: connector
        });

        $(".slideshow-list-" + ar).sortable({
            helper: 'clone',
            placeholder: "ui-state-highlight-cms-slideshow-" + ar,
            connectWith: '.' + ar,
            update: function (ev, ui) {
                //if ($(ui.item).hasClass("draggable")) {
                $(".slideshow-list-" + ar).sortable("refresh");

                UpdateSlideshowImages($(this).data("pagesection-id"), $(this).data("slideshow-id"));
                //}
            },
            scroll: false
        });
    }
}

function InitProductPagesDragDrop(pageSectionId) {
    if (typeof pageSectionId === 'undefined') {
        $(".multisortable-container").multisortable();

        $(".product-page-grid-container").sortable({
            scroll: false
        });
    }
    else {
        $(".multisortable-container[data-page-section-id='" + pageSectionId + "']").multisortable();

        $(".product-page-grid-container[data-page-secton-id=" + pageSectionId + "]").sortable();
    }

    $(".productpage").draggable({
        helper: 'clone',
        start: function (e, ui) {
            $(ui.helper).css("z-index", "300000");
        },
        connectToSortable: '.product-page-grid-container'
    });
}

function InitCollectionDragDrop() {
    $(".collection").draggable({
        helper: 'clone',
        start: function (e, ui) {
            $(ui.helper).css("z-index", "300000");
        },
        connectToSortable: ".collection-grid-container"
    });

    $(".collection-grid-container").sortable({
        helper: 'clone',
        placeholder: "ui-state-highlight-cms-collection",
        connectWith: ".collection",
        scroll: false,
        update: function (ev, ui) {
            $(".collection-grid-container").sortable("refresh");
            UpdateCollectionGrid($(this).data("page-section-id"));
        }
    });
}

function InitPageDragDrop() {
    $(".page").draggable({
        helper: 'clone',
        start: function (ev, ui) {
            $(ui.helper).css("z-index", "300000");
        },
        stop: function (ev, ui) {
            console.log(ev, ui);
        },
        connectToSortable: ".page-grid-container, #related-pages"
    });

    $(".page-grid-container").sortable({
        helper: 'clone',
        placeholder: "ui-state-highlight-cms-page",
        connectWith: ".page",
        update: function (ev, ui) {
            $(".page-grid-container").sortable("refresh");
            UpdatePageGrid($(this).data("page-section-id"));
        },
        scroll: false
    });
}

function LoadPagePodAdvancedSettings(pageSectionId, pageId) {
    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/LoadPagePodAdvancedSettings',
        type: 'POST',
        traditional: true,
        data: {
            PageSectionID: pageSectionId,
            PageID: pageId
        },
        success: function (data) {
            LoadingPanel.Hide();
            $('#page-pod-advanced-settings').html(data);
            $('#page-pod-advanced-settings-modal').modal('show');
        },
        error: function (e, s, a) {
            LoadingPanel.Hide();
            alert(e.responseText);
        },
        timeout: 20000
    });
}
function LoadCanonicalPageAttributeSettings(pageId, parentPageId) {
    if (pageId && parentPageId) {
        LoadingPanel.Show();
        $.ajax({
            url: '/admin/cms/LoadCanonicalPageAttributeSettings',
            type: 'POST',
            traditional: true,
            data: {
                pageId: pageId,
                parentPageId: parentPageId
            },
            success: function (data) {
                $('#page-pod-advanced-settings-modal').modal('show');
                $('#page-pod-advanced-settings').html(data);
                LoadingPanel.Hide();
            },
            error: function (xhr, s, a) {
                LoadingPanel.Hide();
                window.showInfo('Sorry', 'You can only specify pages which have filter indexing enabled for use as canon');
                document.querySelector(`#page-${parentPageId}-canonical-name`).innerHTML = null;
                // this will be hit if the selected canon page isn't filter indexing
            }
        });
    }
}

function InitArticleDragDrop() {
    $(".article").draggable({
        helper: 'clone',
        start: function (e, ui) {
            $(ui.helper).css("z-index", "300000");
        },
        connectToSortable: ".article-grid-container"
    });

    $(".article-grid-container").sortable({
        helper: 'clone',
        placeholder: "ui-state-highlight-cms-article",
        connectWith: ".article",
        update: function (ev, ui) {
            $(".article-grid-container").sortable("refresh");
            UpdateArticleGrid($(this).data("page-section-id"));
        },
        scroll: false
    });
}

function ReloadSectionsForShuffle() {
    
    $(".page-section-hideable").slideUp("fast");

    $("#shuffle-button").animate({
        "opacity": "0.0"
    }, 250, "linear");
    $.ajax({
        url: '/admin/cms/ReloadSectionsForShuffle',
        type: 'POST',
        traditional: true,
        data: {
            PageID: Number($("#selected-page-id").val())
        },
        success: function (data) {
            $("#shuffle-label").html("Edit Page Sections");
            $("#shuffle-button").animate({
                "opacity": "1.0"
            }, 250, "linear");

            $("#shuffle-button").attr("onclick", "ReloadSectionsForEdit()");
            $("#page-sections-container").html(data);
        },
        error: function (e, s, a) {
            $("#shuffle-button").animate({
                "opacity": "1.0"
            }, 250, "linear");
            alert(e.responseText);
        },
        timeout: 20000
    });
    
}

function ReloadSectionsForEdit() {
    $("#shuffle-button").animate({
        "opacity": "0.0"
    }, 250, "linear");
    $.ajax({
        url: '/admin/cms/ReloadSectionsForEditing',
        type: 'POST',
        traditional: true,
        data: { PageID: Number($("#selected-page-id").val()) },
        success: function (data) {
            $("#shuffle-label").html("Shuffle Page Sections");
            $("#shuffle-button").animate({
                "opacity": "1.0"
            }, 250, "linear");
            $("#shuffle-button").attr("onclick", "ReloadSectionsForShuffle()");
            $("#page-sections-container").html(data);

            if($("#page-sections-container.ui-sortable").length > 0)
                $("#page-sections-container.ui-sortable").sortable("destroy");

            if (typeof (InitPodDragDrop) == "function")
                InitPodDragDrop();
            if (typeof (InitCountryDragDrop) == "function")
                InitCountryDragDrop();
            if (typeof (InitCountryLanguageDragDrop) == "function")
                InitCountryLanguageDragDrop();
            if (typeof (InitSlideshowDragDrop) == "function")
                InitSlideshowDragDrop();
            if (typeof (InitProductPagesDragDrop) == "function")
                InitProductPagesDragDrop();
            if (typeof (InitCollectionDragDrop) == "function")
                InitCollectionDragDrop();
            if (typeof (InitPageDragDrop) == "function")
                InitPageDragDrop();
            if (typeof (InitArticleDragDrop) == "function")
                InitArticleDragDrop();

            RefreshPageOptions();

            if (typeof LoadingPanel !== 'undefined')
                LoadingPanel.Hide();
        },
        error: function (e, s, a) {
            $("#shuffle-button").animate({
                "opacity": "1.0"
            }, 250, "linear");
            alert(e.responseText);
        },
        timeout: 20000
    });
}

function RefreshPageOptions()
{
    $("#add-all-country-dialog").html("");
    $("#pagesection-productpage-merge-dialog-buttons").html("");
    $("#add-all-country-dialog").html('<button type="button" onclick="AddAllCountriesToPage();">To Page</button>');

    $("#add-all-country-language-dialog").html("");

    $('.content-panel-container[data-page-section-type="1"]').each(function () {
        $("#add-all-country-language-dialog").append("<button style='display: block; margin-top: 8px;' type='button' onclick='AddAllCountryLanguagesToPageSection(" + $(this).data('page-section-id') + "); $(\"#add-all-country-language-dialog\").dialog(\"close\");'>HTML Section - " + window["PageSection" + $(this).data("page-section-id") + "PageSectionName"].GetValue() + " - (PageSectionID: " + $(this).data("page-section-id") + ")</button>");
    });

    $('.content-panel-container[data-page-section-type="2"]').each(function () {
        $("#add-all-country-language-dialog").append("<button style='display: block; margin-top: 8px;' type='button' onclick='AddAllCountryLanguagesToPageSection(" + $(this).data('page-section-id') + "); $(\"#add-all-country-language-dialog\").dialog(\"close\");'>Slideshow - " + window["PageSection" + $(this).data("page-section-id") + "PageSectionName"].GetValue() + " - (PageSectionID: " + $(this).data("page-section-id") + ")</button>");
    });

    $('.content-panel-container[data-page-section-type="3"]').each(function () {
        $("#add-all-country-language-dialog").append("<button style='display: block; margin-top: 8px;' type='button' onclick='AddAllCountryLanguagesToPageSection(" + $(this).data('page-section-id') + "); $(\"#add-all-country-language-dialog\").dialog(\"close\");'>Form - " + window["PageSection" + $(this).data("page-section-id") + "PageSectionName"].GetValue() + " - (PageSectionID: " + $(this).data("page-section-id") + ")</button>");
    });

    $('.content-panel-container[data-page-section-type="4"]').each(function () {
        $("#add-all-country-language-dialog").append("<button style='display: block; margin-top: 8px;' type='button' onclick='AddAllCountryLanguagesToPageSection(" + $(this).data('page-section-id') + "); $(\"#add-all-country-language-dialog\").dialog(\"close\");'>ProductPage Grid - " + window["PageSection" + $(this).data("page-section-id") + "PageSectionName"].GetValue() + " - (PageSectionID: " + $(this).data("page-section-id") + ")</button>");
        $("#pagesection-productpage-merge-dialog-buttons").append("<input type='button' data-page-section-id=" + $(this).data('page-section-id') + " style='display: block; margin-top: 8px;' onclick='$.confirm(function() { PageSectionProductPageMerge(" + $(this).data('page-section-id') + ") }, function() { });'  value='ProductPage Grid - " + (window["PageSection" + $(this).data("page-section-id") + "PageSectionName"].GetValue() == null ? "Unnamed" : window["PageSection" + $(this).data("page-section-id") + "PageSectionName"].GetValue()) + " - (PageSectionID: " + $(this).data("page-section-id") + ")'/>");
    });

    $('.content-panel-container[data-page-section-type="6"]').each(function () {
        $("#add-all-country-language-dialog").append("<button style='display: block; margin-top: 8px;' type='button' onclick='AddAllCountryLanguagesToPageSection(" + $(this).data('page-section-id') + "); $(\"#add-all-country-language-dialog\").dialog(\"close\");'>Page Grid - " + window["PageSection" + $(this).data("page-section-id") + "PageSectionName"].GetValue() + " - (PageSectionID: " + $(this).data("page-section-id") + ")</button>");
    });

}

    function UpdatePageSectionSortOrder() {

        $("#page-sections-container").sortable("refresh");
        var PageSections = new Array();

        $("#page-sections-container").children("div").each(function (index) {
            PageSections.push({ ID: $(this).data("page-section-id"), SortOrder: index });
        });

        $.ajax({
            url: '/admin/cms/UpdatePageSectionSortOrder',
            type: 'POST',
            traditional: true,
            data: {
                PageID: Number($("#selected-page-id").val()),
                PageSections: JSON.stringify(PageSections)
            },
            success: function (data) {
                $("#page-sections-container").html(data);
            },
            error: function (e, s, a) {
                alert(e.responseText);
            },
            timeout: 20000
        });
    }

function ExpandLHS() {
    $('#page-basic-details-section').slideDown('fast');
    $('#page-details-section').slideDown('fast');
    $('#country-restriction-section').slideDown('fast');
    $('.page-section-hideable').slideDown('fast');
    $("#related-pages").slideDown('fast');
}

function CollapseLHS() {
    $('#page-basic-details-section').slideUp('fast');
    $('#page-details-section').slideUp('fast');
    $('#country-restriction-section').slideUp('fast');
    $('.page-section-hideable').slideUp('fast');
    $("#related-pages").slideUp('fast');
}



function OpenSlideshowImageDialog(SlideshowID, ImageID, PageSectionID, ARID) {
    LoadingPanel.Show();

    $("#slideshow-image-dialog").data("slideshow-id", SlideshowID);
    $("#slideshow-image-dialog").data("image-id", ImageID);
    $("#slideshow-image-dialog").data("page-section-id", PageSectionID);
    $("#slideshow-image-dialog").data("arid", ARID);

    $("#slideshow-image-dialog").dialog("open");
}

function ChangeSlideshowDialogImage(ImageID, PageSectionID) {
    LoadingPanel.Show();

    $("#slideshow-image-dialog").data("image-id", ImageID);

    $.ajax({
        url: '/admin/cms/SlideshowImageOverlayPartial',
        type: 'GET',
        traditional: true,
        data: {
            imageID: ImageID,
            pageSectionID: PageSectionID
        },
        success: function (data) {
            $("#slideshow-image-" + PageSectionID + "-overlay-container").html(data);
            LoadingPanel.Hide();
        },
        timeout: 60000
    });
}

function SlideshowDialogNext(PageSectionID) {
    var imagesList = [];

    var nextIndex = -1;
    $("#page-section-" + PageSectionID + "-slideshow-images").children("div").each(function (index) {
        imagesList.push($(this).data("image-id"));
    });

    var currentIndex = imagesList.indexOf($("#page-section-" + PageSectionID + "-advanced-settings").find(".slideshow-overlay-display-container").data("image-id"));

    nextIndex = currentIndex + 1;

    if (nextIndex >= imagesList.length)
        nextIndex = 0;

    var nextID = imagesList[nextIndex];

    ChangeSlideshowDialogImage(nextID, PageSectionID);
}

function SlideshowDialogPrev(PageSectionID) {
    var imagesList = [];

    var prevIndex = -1;
    $("#page-section-" + PageSectionID + "-slideshow-images").children("div").each(function (index) {
        imagesList.push($(this).data("image-id"));
    });

    var currentIndex = imagesList.indexOf($("#page-section-" + PageSectionID + "-advanced-settings").find(".slideshow-overlay-display-container").data("image-id"));

    prevIndex = currentIndex - 1;

    if (prevIndex < 0)
        prevIndex = imagesList.length - 1;

    var prevID = imagesList[prevIndex];

    ChangeSlideshowDialogImage(prevID, PageSectionID);
}



function RevealLinkSelector(index, selectedValue, pageSectionID) {
    $("#page-section-" + pageSectionID + "-advanced-settings").find(".link-selector").hide();
    $("#link-selector-" + index + "-" + pageSectionID).show();
    switch (Number(index)) {
        case 1:
            $("#Slideshow" + pageSectionID + "page-id-hidden").val("nothing");
            $("#Slideshow" + pageSectionID + "product-page-id-hidden").val('nothing');
            $("#Slideshow" + pageSectionID + "collection-id-hidden").val('nothing');
            window["Slideshow" + pageSectionID + "ExternalURL"].SetText('');
            window["Slideshow" + pageSectionID + "ExternalURL"].SetValue(null);
            $("#link-" + pageSectionID + "-test").hide();
            break;
        case 2:
            window["Slideshow" + pageSectionID + "MenuBuilderPageSelectorGrid"].AdjustControl();
            //window.setTimeout(function () {
            //    if (selectedValue != null)
            //        window["Slideshow" + pageSectionID + "MenuBuilderPageSelectorGrid"].AutoFilterByColumn(0, selectedValue);
            //}, 2500);
            break;
        case 3:
            window["Slideshow" + pageSectionID + "MenuBuilderProductPageSelectorGrid"].AdjustControl();
            //window.setTimeout(function () {
            //    if (selectedValue != null)
            //        window["Slideshow" + pageSectionID + "MenuBuilderProductPageSelectorGrid"].AutoFilterByColumn(0, selectedValue);
            //}, 2500);
            break;
        case 4:
            window["Slideshow" + pageSectionID + "MenuBuilderCollectionSelectorGrid"].AdjustControl();
            //window.setTimeout(function () {
            //    if (selectedValue != null)
            //        window["Slideshow" + pageSectionID + "MenuBuilderCollectionSelectorGrid"].AutoFilterByColumn(0, selectedValue);
            //}, 2500);
            break;
        default: break;
    }
}

function MergeAcrossSlideshow(PageSectionID) {
    var targ = $("#page-section-" + PageSectionID + "-advanced-settings").find(".slideshow-overlay").first();

    var col = targ.css("background-color");
    col = col.substring(col.indexOf('(') + 1, col.lastIndexOf(')')).split(/,\s*/);
    col = col.splice(0, 3);
    var rgb = col.join(',');

    var fCol = targ.css("color");
    fCol = fCol.substring(fCol.indexOf('(') + 1, fCol.lastIndexOf(')')).split(/,\s*/);
    var fontRGB = fCol.join(',');

    var opa = window["SlideShow" + PageSectionID + "OverlayOpacity"].GetValue();
    $(targ).parent().hide();
    var w = targ.width();
    var h = targ.height();
    var t = parseFloat(targ[0].style.top);
    var l = parseFloat(targ[0].style.left);
    $(targ).parent().show();

    var imageID = $("#page-section-" + PageSectionID + "-advanced-settings").find(".slideshow-overlay-display-container").first().data("image-id");

    $.ajax({
        url: '/admin/cms/MergeSlideshowOverlaySetting',
        type: 'POST',
        traditional: true,
        data: {
            SlideshowID: $("#page-section-" + PageSectionID).data("slideshow-id"),
            PageSectionID: PageSectionID,
            DisplayOverlay: window["SlideShow" + PageSectionID + "DisplayOverlay"].GetValue(),
            ShowHotspots: window["SlideShow" + PageSectionID + "ShowHotspots"].GetValue(),
            OverlayTop: t,
            OverlayLeft: l,
            OverlayWidth: w,
            OverlayHeight: h,
            OverlayBackgroundColour: rgb,
            OverlayBackgroundOpacity: opa,
            OverlayFontColour: fontRGB,
            SlideshowOverlayPositionID: $('#slideshow-' + PageSectionID + '-overlay-position').val(),
            AutoHideOverlay: window["SlideShow" + PageSectionID + "AutoHideOverlay"].GetValue(),
            PageID: $("#Slideshow" + PageSectionID + "page-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "page-id-hidden").val(),
            ProductPageID: $("#Slideshow" + PageSectionID + "product-page-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "product-page-id-hidden").val(),
            CollectionID: $("#Slideshow" + PageSectionID + "collection-id-hidden").val() == "nothing" ? null : $("#Slideshow" + PageSectionID + "collection-id-hidden").val(),
            ExternalURL: window["Slideshow" + PageSectionID + "ExternalURL"].GetValue(),
            NewWindow: window["Slideshow" + PageSectionID + "NewWindow"].GetValue(),
            OverlayHeading: window["Slideshow" + PageSectionID + "OverlayHeading"].GetValue(),
            OverlayText: window["Slideshow" + PageSectionID + "OverlayText"].GetValue(),
            OverlayFade: window["SlideShow" + PageSectionID + "OverlayFade"].GetValue(),
            ImageID: imageID
        },
        success: function (data) {
            LoadingPanel.Hide();
            $("#complete-dialog").dialog('open');
        },
        error: function (e, s, a) {
            LoadingPanel.Hide();
            $("#slideshow-image-" + PageSectionID + "-overlay-container").html(e.responseText);
        },
        timeout: 20000
    });
}

function AddAllCountriesToPage() {
    window.CountryRestrictions = new Array();
    if ($('#country-restriction-placeholder').length > 0)
        $('#country-restriction-placeholder').remove();
    //$('.country-restriction-flag').each(function (index) {
    //    window.CountryRestrictions.push({ ID: $(this).data('country-id'), SortOrder: 0 });
    //});
    $('.country-flag').each(function (index) {
        window.CountryRestrictions.push({ ID: $(this).data('country-id'), SortOrder: 0 });
    });
    UpdateCountryRestrictions();
    if (typeof (InitCountryDragDrop) == 'function')
        InitCountryDragDrop();
}

function AddAllCountriesToSlideshow(SlideshowID, PageSectionID) {
    $(".slideshow-country-restriction-section").sortable("refresh");
    var SlideshowCountryRestrictions = new Array();

    $('.country-restriction-flag').each(function (index) {
        SlideshowCountryRestrictions.push({ ID: $(this).data("country-id"), SortOrder: 0 });
    });

    $("#slideshow-image-" + PageSectionID + "-overlay-container").find(".country-restriction-placeholder").first().remove();

    $.ajax({
        url: '/admin/cms/UpdateSlideshowCountryRestrictions',
        type: "POST",
        traditional: true,
        data: { SlideshowID: SlideshowID, SlideshowCountryRestrictions: JSON.stringify(SlideshowCountryRestrictions) },
        success: function (data) {
            $("#slideshow-image-" + PageSectionID + "-overlay-container").html(data);
        },
        timeout: 20000
    });
}

function AddAllCountryLanguagesToPageSection(PageSectionID) {
    $("div [data-page-section-id='" + PageSectionID + "']").find(".country-restriction-section").sortable("refresh");
    var restrictions = new Array();

    $('.country-language-flag').each(function (index) {
        restrictions.push({
            CountryID: $(this).data("country-id"),
            LanguageID: $(this).data("language-id"),
            SortOrder: 0
        });
    });

    $("div [data-page-section-id='" + PageSectionID + "']").find(".country-restriction-section .country-language-restriction-flag").each(function () {
        restrictions.push({
            CountryID: $(this).data("country-id"),
            LanguageID: $(this).data("language-id"),
            SortOrder: 0
        });
    });

    $("div [data-page-section-id='" + PageSectionID + "']").find(".country-restriction-placeholder").first().remove();

    LoadingPanel.Show();

    $.ajax({
        url: '/admin/cms/updatepagesectioncountryrestrictions',//'@Url.Action("UpdatePageSectionCountryRestrictions", "CMS")',
        type: "POST",
        traditional: true,
        data: { pageSectionID: PageSectionID, countryLanguageRestrictions: JSON.stringify(restrictions) },
        success: function (data) {
            $("#page-section-" + PageSectionID + "-country-restriction-container").html(data);
            LoadingPanel.Hide();
        },
        timeout: 20000
    });

    ReloadSectionsForEdit();
}

// Swapped around before passing to merge statement to reverse direction of copy. We've just created A and want to interact with it, instead of having to find B
function PageSectionProductPageMerge() {//int PageID, long SourcePageSectionID, long TargetPageSectionID)
    var SourcePageSectionID = $("#merge-productpages-page-section-id").val();

    var TargetPageSectionID = window["TargetPageSectionID"].GetValue();

    var productPageIDs = new Array();

    $("#page-section-" + SourcePageSectionID + "-grid").find(".multisortable-selected").each(function (index) {
        productPageIDs.push($(this).data("product-page-id"));
    });

    if (productPageIDs.length == 0) {
        $("#page-section-" + SourcePageSectionID + "-grid").children("div").not(".product-page-placeholder").each(function () {
            productPageIDs.push($(this).data("product-page-id"));
        });
    }

    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/PageSectionProductPageMerge',
        type: 'POST',
        traditional: true,
        data: {
            PageID: $("#selected-page-id").val(),
            SourcePageSectionID: SourcePageSectionID,
            TargetPageSectionID: TargetPageSectionID,
            keyString: productPageIDs.toString(),
            replace: ProductPageMergeReplace.GetValue()
        },
        success: function (data) {
            $("#pagesection-productpage-merge-source").data('source-page-section-id', '-1');
            $("#pagesection-productpage-merge-dialog").dialog("close");
            $("#page-sections-container").html(data);
            LoadingPanel.Hide();
            MergeGridPopup.Hide();
        },
        timeout: 20000
    });
}

function SortProductGridByBestSeller(PageSectionID, PriceListID) {
    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/PageSectionProductPageSortBestSeller',
        type: 'POST',
        traditional: true,
        data: {
            PageID: $("#selected-page-id").val(),
            PageSectionID: PageSectionID,
            PriceListOverride: PriceListID
        },
        success: function (data) {
            $("#page-sections-container").html(data);
            LoadingPanel.Hide();
        },
        timeout: 20000
    });
}

function SortProductGridByStock(PageSectionID) {
    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/PageSectionProductPageSortStock',
        type: 'POST',
        traditional: true,
        data: {
            PageID: $("#selected-page-id").val(),
            PageSectionID: PageSectionID
        },
        success: function (data) {
            $("#page-sections-container").html(data);
            LoadingPanel.Hide();
        },
        timeout: 20000
    });
}

function SortProductGridByClearance(PageSectionID) {
    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/PageSectionProductPageSortClearance',
        type: 'POST',
        traditional: true,
        data: {
            PageID: $("#selected-page-id").val(),
            PageSectionID: PageSectionID
        },
        success: function (data) {
            $("#page-sections-container").html(data);
            LoadingPanel.Hide();
        },
        timeout: 20000
    });
}

function SortProductGridByColour(PageSectionID, Direction) {
    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/PageSectionProductPageSortColour',
        type: 'POST',
        traditional: true,
        data: {
            PageID: $("#selected-page-id").val(),
            PageSectionID: PageSectionID,
            Direction: Direction
        },
        success: function (data) {
            $("#page-sections-container").html(data);
            LoadingPanel.Hide();
        },
        timeout: 20000
    });
}

function SortProductGridByLaunch(PageSectionID) {
    LoadingPanel.Show();
    $.ajax({
        url: '/admin/cms/PageSectionProductPageSortLaunch',
        type: 'POST',
        traditional: true,
        data: {
            PageID: $("#selected-page-id").val(),
            PageSectionID: PageSectionID
        },
        success: function (data) {
            $("#page-sections-container").html(data);
            LoadingPanel.Hide();
        },
        timeout: 20000
    });
}

function OpenClonePageSectionDialog(PageSectionID) {
    $("#page-section-clone-dialog").data("page-section-id", PageSectionID);
    PageSectionClonePageID.SetValue(null);
    PageSectionCloneOnDate.SetValue(null);
    PageSectionCloneCopyContents.SetChecked(false);
    $("#page-section-clone-dialog").dialog("open");
}

function ClonePageSection(PageSectionID) {
    if (!window.isCloningPageSection) {
        window.isCloningPageSection = true;
        $("#page-section-clone-dialog").dialog("close");
        var d = PageSectionCloneOnDate.GetValue() == null ? null : PageSectionCloneOnDate.GetValue().toGMTString();
        LoadingPanel.Show();
        $.ajax({
            url: '/CMS/ClonePageSection',
            type: 'POST',
            data: {
                PageID: $("#selected-page-id").val(),
                TargetPageID: PageSectionClonePageID.GetValue() == null ? $("#selected-page-id").val() : PageSectionClonePageID.GetValue(),
                PageSectionID: PageSectionID,
                OnDate: d,
                CopyContents: PageSectionCloneCopyContents.GetChecked()
            },
            success: function (data) {
                window.isCloningPageSection = false;
                $("#page-sections-container").html(data);
                LoadingPanel.Hide();
                ClonePageSectionPopup.Hide();
            },
            timeout: 30000,
            error: function (xhr, s, t) {
                window.isCloningPageSection = false;
                ClonePageSectionPopup.Hide();
                $.showError(xhr, s, t);
            }
        });
    }
}

$(document).ready(function () {

    $("#page-section-advanced-settings-dialog").dialog({
        title: "Page Section Advanced Settings",
        autoOpen: false,
        autoResize: true,
        height: "auto",
        width: "auto",
        modal: false,
        buttons: {
            "Save": function () {
                if ($(this).data("type") == 0) {
                    $.showError("Save Failure", "Meltdown", "Someone get Tom");
                }
                switch ($(this).data("type")) {
                    case 1:
                        break;
                    case 2:
                        SaveSlideshowAdvancedSettings($(this).data("pagesectionid"));
                        return;
                        break;
                    case 3:
                        break;
                    case 4:
                        SaveProductPageGridAdvancedSettings($(this).data("pagesectionid"));
                        break;
                    case 5:
                        SaveCollectionGridAdvancedSettings($(this).data("pagesectionid"));
                        break;
                    case 6:
                        SavePageGridAdvancedSettings($(this).data("pagesectionid"));
                        break;
                    case 9:
                        SaveArticleGridAdvancedSettings($(this).data("pagesectionid"));
                        break;
                    default: break;
                }

                $(this).dialog("close");
                var i = $("#page-section-advanced-settings-dialog").children(0).hide();
                var i = $("#page-section-advanced-settings-dialog").children(0).detach();
                $(".content-panel-container[data-page-section-id=" + $(this).data("pagesectionid") + "]").append(i[0]);
            }
        },
        close: function () {
            $(this).dialog("close");
            var i = $("#page-section-advanced-settings-dialog").children(0).hide();
            var i = $("#page-section-advanced-settings-dialog").children(0).detach();
            $(".content-panel-container[data-page-section-id=" + $(this).data("pagesectionid") + "]").append(i[0]);
        }
    });

    $("#add-page-section-type-dialog").dialog({
        title: "Select Page Section Type",
        autoOpen: false,
        height: "auto",
        width: 280,
        modal: true,
        buttons: {
            "Ok": function () {
                if ($("#page-section-type-select").val() != "0") {
                    if ($("#slideshow-height-select").is(":visible")) {
                        if ($("#slideshow-height-select").val() != "0") {
                            $(this).dialog("close");
                            AddPageSection($('#add-page-section-position').val());
                            return false;
                        }
                        else {
                            $("#slideshow-height-select").animate({
                                "background-color": "#FFA0A0"
                            }, 250, "linear", function () {
                                $("#slideshow-height-select").animate({
                                    "background-color": "white"
                                }, 250, "linear");
                            });
                            return false;
                        }
                    }
                    if ($("#animation-height-select").is(":visible")) {
                        if ($("#animation-height-select").val() != "0") {
                            $(this).dialog("close");
                            AddPageSection($('#add-page-section-position').val());
                            return false;
                        }
                        else {
                            $("#animation-height-select").animate({
                                "background-color": "#FFA0A0"
                            }, 250, "linear", function () {
                                $("#animation-height-select").animate({
                                    "background-color": "white"
                                }, 250, "linear");
                            });
                            return false;
                        }
                    }
                    //if ($("#form-type-select").is(":visible")) {
                    //    if ($("#form-type-select").val() != "0") {
                    //        $(this).dialog("close");
                    //        AddPageSection($('#add-page-section-position').val());
                    //        return false;
                    //    }
                    //    else {
                    //        $("#form-type-select").animate({
                    //            "background-color": "#FFA0A0"
                    //        }, 250, "linear", function () {
                    //            $("#form-type-select").animate({
                    //                "background-color": "white"
                    //            }, 250, "linear");
                    //        });
                    //    }
                    //}
                    if ($("#banner-location-group").is(":visible")) {
                        if ($("#banner-location-group").val() != "0") {
                            $(this).dialog("close");
                            AddPageSection($('#add-page-section-position').val());
                            return false;
                        }
                        else {
                            $("#banner-location-group").animate({
                                "background-color": "#FFA0A0"
                            }, 250, "linear", function () {
                                $("#banner-location-group").animate({
                                    "background-color": "white"
                                }, 250, "linear");
                            });
                        }
                    }
                    else {
                        $(this).dialog("close");
                        AddPageSection($('#add-page-section-position').val());
                        return false;
                    }
                }
                else {
                    $("#page-section-type-select").animate({
                        "background-color": "#FFA0A0"
                    }, 250, "linear", function () {
                        $("#page-section-type-select").animate({
                            "background-color": "white"
                        }, 250, "linear");
                    });
                }
            },
            Cancel: function () {
                $("#animation-height-select").fadeOut("fast");
                $("#slideshow-height-select").fadeOut("fast");
                $("#brochure-select-container").hide();
                $(this).dialog("close");
            }
        }
    });

    $("#product-page-section-selector-dialog").dialog({
        title: "Select Page Section",
        autoOpen: false,
        height: "auto",
        width: "auto",
        modal: true,
        buttons: {
            "Ok": function () {
                if ($("#product-page-section-select").val() != "0") {
                    SelectProductPageForBulk();
                    $(this).dialog("close");
                }
                else {
                    $("#product-page-section-select").animate({
                        "background-color": "#FFA0A0"
                    }, 250, "linear", function () {
                        $("#product-page-section-select").animate({
                            "background-color": "white"
                        }, 250, "linear");
                    });
                }
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    $("#collection-section-selector-dialog").dialog({
        title: "Select Page Section",
        autoOpen: false,
        height: "auto",
        width: 250,
        modal: true,
        buttons: {
            "Ok": function () {
                if ($("#collection-section-select").val() != "0") {
                    SelectCollectionForBulk();
                    $(this).dialog("close");
                }
                else {
                    $("#collection-section-select").animate({
                        "background-color": "#FFA0A0"
                    }, 250, "linear", function () {
                        $("#collection-section-select").animate({
                            "background-color": "white"
                        }, 250, "linear");
                    });
                }
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    $("#page-section-selector-dialog").dialog({
        title: "Select Page Section",
        autoOpen: false,
        resizable: false,
        modal: true,
        width: 380,
        buttons: {
            "Ok": function () {
                if ($("#page-section-select").val() != "0") {
                    SelectPageForBulk();
                    $(this).dialog("close");
                }
                else {
                    $("#page-section-select").animate({
                        "background-color": "#FFA0A0"
                    }, 250, "linear", function () {
                        $("#page-section-select").animate({
                            "background-color": "white"
                        }, 250, "linear");
                    });
                }
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    $("#add-all-country-dialog").dialog({
        title: 'Add All Countries',
        width: 'auto',
        height: 'auto',
        autoResize: true,
        autoOpen: false,
        buttons: {
            Cancel: function (ev, ui) {
                $(this).dialog("close");
            }
        }
    });

    $("#add-all-country-language-dialog").dialog({
        title: 'Add All Sites',
        width: 'auto',
        height: 'auto',
        autoResize: true,
        autoOpen: false,
        buttons: {
            Cancel: function (ev, ui) {
                $(this).dialog("close");
            }
        }
    });

    $("#slideshow-image-dialog").dialog({
        title: "Slideshow Image Details",
        autoOpen: false,
        height: "auto",
        width: "auto",
        autoResize: true,
        modal: true,
        buttons: {
            "Save": function () {
                LoadingPanel.Show();

                var PageID = null;
                var ProductPageID = null;
                var CollectionID = null;
                var ExternalURLValue = null;
                var NewWindowValue = null;

                if (NewWindow.GetStateInput() != null)
                    NewWindowValue = NewWindow.GetValue();

                if (LinkTypeComboBox.GetValue() == 5 && ExternalURL.GetInputElement() != null)
                    ExternalURLValue = ExternalURL.GetValue();
                else {
                    if (typeof (MenuBuilderPageSelectorGrid) == 'object') {
                        if ((MenuBuilderPageSelectorGrid.GetFocusedRowIndex() != -1 || $('#page-id-hidden').val() != 'nothing'))
                            PageID = $('#page-id-hidden').val();

                        if ((MenuBuilderProductPageSelectorGrid.GetFocusedRowIndex() != -1 || $('#product-page-id-hidden').val() != 'nothing'))
                            ProductPageID = $('#product-page-id-hidden').val();

                        if ((MenuBuilderCollectionSelectorGrid.GetFocusedRowIndex() != -1 || $('#collection-id-hidden').val() != 'nothing'))
                            CollectionID = $('#collection-id-hidden').val();
                    }
                }

                $.ajax({
                    url: '/admin/cms/saveslideshowimagetextdetails',
                    type: 'POST',
                    traditional: true,
                    data: {
                        SlideshowID: $("#slideshow-image-dialog").data("slideshow-id"),
                        ImageID: $("#slideshow-image-dialog").data("image-id"),
                        Heading: SlideshowImageHeading.GetValue(),
                        Text: SlideshowImageText.GetValue(),
                        PageID: PageID,
                        ProductPageID: ProductPageID,
                        CollectionID: CollectionID,
                        ExternalURL: ExternalURLValue,
                        NewWindow: NewWindowValue
                    },
                    success: function (data) {
                        LoadingPanel.Hide();
                        $("#slideshow-image-dialog").dialog("close")
                    },
                    error: function (e, s, a) {
                        $("#slideshow-image-dialog-inner").html(e.responseText);
                        LoadingPanel.Hide();
                    },
                    timeout: 20000
                });

                var i = $("#page-section-advanced-settings-dialog").children(0).hide();
                var i = $("#page-section-advanced-settings-dialog").children(0).detach();
                $(".content-panel-container[data-page-section-id=" + $(this).data("pagesectionid") + "]").append(i);
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    $("#animation-dialog").dialog({
        title: "Animation Details",
        autoOpen: false,
        height: "auto",
        width: "auto",
        resizable: false,
        autoResize: true,
        modal: false,
        buttons: {
            "Done": function () {
                $("#call-to-action-grid-dialog").dialog("close");
                $(this).dialog("close");
                ReloadSectionsForEdit();
            }
        },
        close: function (ev, ui) {
            //$("#call-to-action-grid-dialog").dialog("close");
            ReloadSectionsForEdit();
        }
    });

    RefreshPageOptions();
});