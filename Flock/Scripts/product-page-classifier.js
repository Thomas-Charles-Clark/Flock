function ToggleProductPage(ProductPageID) {
    var t = Number($("#result-total").text());
    var t1 = t - 1;
    $("#result-total").text(t1.toString());

    $.ajax({
        url: '/admin/productpages/ToggleProductPageAttribute',
        type: 'POST',
        traditional: true,
        data: {
            ProductPageID: ProductPageID,
            AttributeID: AttributeListBox.GetSelectedItem().value
        }
    });
}

function ToggleInput(ele) {
    if (ele.is(':checked')) {
        ele.removeAttr('checked');
    } else {
        ele.attr('checked', 'checked');
    }
}

function SaveProductPageAttributes(ProductPageID, ele) {
    var t = Number($("#result-total").text());
    var t1 = t - 1;
    $("#result-total").text(t1.toString());

    var AttributeIDs = '';

    $(ele).find('input:checked').each(function() { 
        AttributeIDs +=  $(this).val() + ',';
    });


    $.ajax({
        url: '/admin/productpages/SaveProductPageAttributes',
        type: 'POST',
        traditional: true,
        data: {
            ProductPageID: ProductPageID,
            AttributeGroupID: AttributeGroupListBox.GetValue(),
            AttributeIDs: AttributeIDs
        }
    });
}

function SaveAll() {
    $.confirm(function () {
        var data = new Array();

        $(".result-cell").each(function () {
            if ($(this).find('input:checked').length > 0) {

                var Attributes = [];
                $(this).find('input:checked').each(function () {
                    Attributes.push($(this).val());
                });

                data.push({ ProductPageID: $(this).data("product-page-id"), Attributes: Attributes });

                $(this).remove();
            }
        });

        $.ajax({
            url: '/admin/productpages/SaveMultipleProductPageAttributes',
            type: 'POST',
            traditional: true,
            data: {
                AttributeGroupID: AttributeGroupListBox.GetValue(),
                Data: JSON.stringify(data)
            }
        });
    },
    function () {

    });
}