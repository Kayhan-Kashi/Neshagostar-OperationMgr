$(function () {

    $("form").validate({   /// binding validation properties to form
        rules: {
            Customer_name: "required",
            ProductInput: "required",
            AmountInMeteres: "required",
            pricePerMeter: "required",
            NominalWeightPerMeter: "required",
            hDPEPrice: "required",
        },
        messages: {
            Customer_name: "لطفا نام خریدار را وارد فرمائید",
            ProductInput: "لطفا نام محصول را وارد فرمائید",
            AmountInMeteres: "لطفا متراژ را وارد فرمائید",
            pricePerMeter: "لطفا قیمت یک متر را وارد فرمائید",
            NominalWeightPerMeter: "لطفا وزن یک متر را وارد فرمائید",
            hDPEPrice: "لطفا قیمت مواد اولیه بروز را وارد فرمائید",
        }
        //submitHandler: function (form) {

        //    //form.submit();
        //}
    });
    //$("#btnSubmit").on("click", function () {  ///  binds 2 condition to this button
    //    if (orderItems.length == 0) {   /// if orderItems list is empty it means no product has been selected so the button will not submit
    //        alert("هیچ آیتمی برای سفارش وارد نشده است");
    //    } else {  /// if orderItems is not empty it means some products hass been chosen so the form will be submited without doing any validation hence if input for products properties has some value or has n't any value they will be discarded
    //        $('#form').data('validator', null);

    //        return false;

    //        //$("form")[0].submit();
    //    }
    //});
});

function validateForm() {
    if ($("form").valid()) { /// checks the validation rules for form
        addItem();  /// if products entries all pass validation rules it gathers all the entries and create a product and adds it to orderitems list and show it it orderItems table.
    }

}


var products = "";

function getProductsJson() {
    return $.ajax({
        type: "GET",
        url: "/commerce/products/getProducts"
    });
};

var productsPromise = getProductsJson();

productsPromise.success(function (data) {
    products = data;
    //alert(data[0].id)
});

var parentProductsDialog;

function popUpProducts() {
    //alert("popup ");

    var parentDiv = $('<div id="divPopupProducts" style="display:none"></div>').addClass("table-responsive");
    //alert();
    var table = $("<table></table>").addClass("table table-hover");
    var headers = $("<tr></tr>");
    headers.append(
        `<th>نام محصول</th>
         <th>نوع</th>
        `);

    headers.appendTo(table);
    var tableDatas = $("<tr></tr>");
    for (var productNum = 0; productNum < products.length; productNum++) {
        $(`<td style="font-family:B Nazanin" onclick="showProduct('${products[productNum].id}','${products[productNum].name}')">${products[productNum].name}</td>`).appendTo(tableDatas);
        $(`<td style="font-family:B Nazanin" onclick="showProduct('${products[productNum].id}','${products[productNum].name}')">${products[productNum].category}</td>`).appendTo(tableDatas);
        tableDatas.appendTo(table);
        tableDatas = $("<tr></tr>");
    }
    tableDatas.appendTo(table);
    table.appendTo(parentDiv);
    parentProductsDialog = true;


    parentDiv.dialog({
        height: 400,
        width: 500,
        modal: true,
        buttons: {
            "OK": function () {
                $(this).dialog("close");
                //alert("You selected OK!!")
            },
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    })
    parentProductsDialog = parentDiv;
}

function showProduct(id, name) {
    //alert(name);
    //alert("showproduct");
    //alert(id + "   "  +  name);
    if (checkIfCurrentProductHasBeenChosenBefore(id)) {
        $("#productId").val(productId);
        $("#productName").val(productName);
        alert("محصول از قبل انتخاب شده است");
    }
    else {
        $("#productName").val(name);
        $("#productId").val(id);
    }
    parentProductsDialog.dialog("close");


}

function removeNullOrderItems() {
    var orderItemsWithoutNull = [];
    for (var i = 0; i < orderItems.length; i++) {
        if (orderItems[i] !== null) {
            //orderItems[i].amountInMeters.re
            orderItemsWithoutNull.push(orderItems[i]);
        }
    }
    //alert("remove null finished.");
    return orderItemsWithoutNull;
}


function calculateTotalWeight() {
    var nominalWeightPerMeter = $("#nominalWeightPerMeter").val();
    var amountInMeters = $("#amountInMeters").val();
    //alert();
    if (nominalWeightPerMeter != null && amountInMeters != null) {
        var total = nominalWeightPerMeter * amountInMeters;
        $("#totalWeight").val(total);
    }

}

var orderItems = [];


function addItem() {
    var orderItem = getItemsFromInputs();
    if (checkIfProductIdIsDouplicated(orderItem)) {
        alert("نوع کالا از قبل انتخاب شده است");
    }
    else {
        orderItems.push(orderItem);
        transportToTable(orderItem);
        cleanInputs();
        calculateFinalPrice(orderItems);
    }
}

function checkIfProductIdIsDouplicated(orderItem) {

    for (var i = 0; i < orderItems.length; i++) {
        if (orderItems[i] !== null && orderItems[i].productId == orderItem.productId) {
            return true;
        }
    }
    return false;
}



function getItemsFromInputs() {

    var totalWeight = $("#totalWeight");

    var productName = $("#productInput").val();
    var productId = $("#productInputHidden").val();
    var amountInMeters = $("#amountInMeters").val();
    var pricePerMeter = $("#pricePerMeter").val();
    var nominalWeightPerMeter = $("#nominalWeightPerMeter").val();
    var hDPEPrice = $("#hDPEPrice").val();
    var washerPrice = $("#washerPrice").val();
    var totalWeight = $("#totalWeight").val();
    //var isUsedFromInventory = $("#isUsedFromInventory").val();  
    var orderItemTotalPrice = amountInMeters * pricePerMeter;
    var pricePerKilo = Math.ceil(orderItemTotalPrice / totalWeight);
    //var pricePerKiloCal = pricePerMeter / nominalWeightPerMeter;
    var ordItemDynamic = {
        productName: productName, productId: productId, amount: amountInMeters, pricePerUnit: pricePerMeter, nominalWeightPerMeter: nominalWeightPerMeter,
        hDPEPrice: hDPEPrice, washerPrice: washerPrice, totalPrice: orderItemTotalPrice, pricePerKilo: pricePerKilo, totalWeight: totalWeight

    }

    return ordItemDynamic;
}


function transportToTable(orderItem) {
    //alert(orderItem.nominalWeightPerMeter);
    var orderItemClmns = $("#orderItemsClmns");
    var row = $(`<tr id="orderItemNo${orderItems.length - 1}"></tr>`);
    $(`<td>${orderItem.productName}</td>`).appendTo(row);
    $(`<td>${Math.trunc(orderItem.amount).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')}</td>`).appendTo(row);
    $(`<td>${Math.trunc(orderItem.pricePerUnit).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')}</td>`).appendTo(row);
    $(`<td>${orderItem.nominalWeightPerMeter.toString()}</td>`).appendTo(row);
    $(`<td>${(Math.trunc(orderItem.pricePerKilo)).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')}</td>`).appendTo(row);
    $(`<td>${Math.trunc(orderItem.hDPEPrice).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')}</td>`).appendTo(row);
    $(`<td>${Math.trunc(orderItem.washerPrice).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')}</td>`).appendTo(row);


    $(`<td>${(Math.trunc(orderItem.amount * orderItem.pricePerUnit)).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')}</td>`).appendTo(row);
    $(`<td>${(Math.trunc(orderItem.amount * orderItem.pricePerUnit)).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')}</td>`).appendTo(row);
    $(`<td><button class="btn btn-warning" type="button" onclick="removeOrderItem(${orderItems.length - 1})">حذف</button></td>`).appendTo(row);
    row.appendTo(orderItemClmns);

}

var finalSum = 0;
var finalPrice = 0;
var addedCost = 0;
var finalOrderPrice = 0;

function calculateFinalPrice(orderItems) {
    finalSum = 0;
    finalPrice = 0;
    addedCost = 0;

    //alert("calculateFinalPrice");
    for (var ordNum = 0; ordNum < orderItems.length; ordNum++) {
        if ((orderItems[ordNum]) !== null) {
            var ordItem = orderItems[ordNum];
            //ordItem.totalPrice = ordItem.amountInMeters * ordItem.pricePerMeter;
            finalSum += ordItem.totalPrice;
        }
    }
    //orderItems.forEach(calculate(item));
    addedCost = finalSum * 0.09;
    finalPrice = addedCost + finalSum;
    showOrderPriceDetail(finalSum, addedCost, finalPrice);
    //$("#priceWithoutAddedCost").text(Math.trunc(finalSum).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,'));
    //$("#addedCost").text(Math.trunc(addedCost).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,'));
    //$("#finalPrice").text(Math.trunc(finalPrice).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,'));
    finalOrderPrice = finalPrice;
}

//function calculate(item) {
//    alert("calculate");
//    finalSum += (item.amountInMeters * item.pricePerMeter);

//}
///////////////////////////////////////////////////////////////// 

function cleanInputs() {
    $("#productInput").val("");
    $("#productInputHidden").val("");
    $("#amountInMeters").val("");
    $("#pricePerMeter").val("");
    $("#nominalWeightPerMeter").val("");
    $("#hDPEPrice").val("");
    $("#washerPrice").val("");
    $("#amountUsedFromInventory").val("");
    $("#totalWeight").val("");
}

///////////////////////////////////////////////////////////////// 

function removeOrderItem(orderItemIndex) {
    orderItems[orderItemIndex] = null;
    $(`#orderItemNo${orderItemIndex}`).remove();
    calculateFinalPrice(orderItems);

}


///////////////////////////////////////////////////////////////// 



function showOrderPriceDetail(finalSum, addedCost, finalPrice) {
    $("#priceWithoutAddedCost").text(Math.trunc(finalSum).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,'));
    $("#addedCost").text(Math.trunc(addedCost).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,'));
    $("#finalPrice").text(Math.trunc(finalPrice).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,'));
}

var storedAddedCost;
var storedFinalPrice;

function addedCostToggle(chekBox) {
    if (chekBox.checked) {
        addedCost = storedAddedCost;
        finalPrice = storedFinalPrice;
        calculateFinalPrice(orderItems);
        //storedAddedCost = addedCost;
        //showOrderPriceDetail(finalSum, addedCost, finalPrice);
    }
    else {
        storedAddedCost = addedCost;
        storedFinalPrice = finalPrice;
        addedCost = 0;
        finalPrice = finalSum;
        finalOrderPrice = finalPrice;
        showOrderPriceDetail(finalSum, addedCost, finalOrderPrice);
    }

}

function createBindingObject() {
    var orderItems = removeNullOrderItems();
    var customerId = $("#customerIdHidden").val();
    var customerName = $("#customer_name").val();
    var contactNumber = $("#customer_tel").val();
    var address = $("#customer_address").val();
    var comments = $("#comments").val();
    //var addedCostAmount = $("#addedCost").text().replace(',', '');
    var hasAddedCost = $("#hasAddedCost").is(":checked");
    var addedCostAmount = addedCost;
    //var priceWithoutAddedCost = $("#priceWithoutAddedCost").text().replace(',', '');
    var priceWithoutAddedCost = finalSum;
    var finalPrice = finalOrderPrice;
    //var finalPrice = $("#finalPrice").text().replace(',', '');

    var obj = {
        inquiryItems: orderItems,
        customerId: customerId,
        customerName: customerName,
        contactNumber: contactNumber,
        address: address,
        addedCostAmount: addedCostAmount,
        hasAddedCost: hasAddedCost,
        priceWithoutAddedCost: priceWithoutAddedCost,
        finalPrice: finalPrice,
        comments: comments
    };
    //alert(obj.customerId);
    return obj;
}

function sendOrderItemsJson() {
    var item = createBindingObject();
    //alert(item);
    //alert();
    $.ajax({
        type: "POST",
        url: "/Commerce/Inquiries/Create",
        async: false,
        data: { inquiry: item },
        dataType: "json",
        content: "application/json; charset=utf-8",
        success: function (data) {
            window.location.href = data;
        }
    });
    return products;
}


var productsChosen = "";

function getProductsChosenBefore(inquiryId) {
    return $.ajax({
        type: "GET",
        url: "/commerce/Inquiries/getProductsChosen",
        data: { inquiryId: inquiryId }
    });
};


function collectProductHasBeenChosenBefore(id) {
  
    var productsChosenPromise = getProductsChosenBefore(id);
    productsChosenPromise.success(function (data) {
        productsChosen = data;
        //alert(data[0].id)
    });
}

function checkIfCurrentProductHasBeenChosenBefore(id) {

    for (var i = 0; i < productsChosen.length; i++) {
        if (productsChosen[i].id == id) {
            return true;
        }
    }
    return false;
}