
var year_select;
var month_select;

$(document).ready(function () {

    year_select = $("#filter_year");
    month_select = $("#filter_month");
    saloon_select = $("#filter_saloon");
    device_select = $("#filter_device");
    productionLine_select = $("#filter_productionLine");
    //getSaloonsName();
    initializeYearSelect();
    bindEventToSelect_year();
    bindEventToClick();


//alert("dddd");

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        url: '/production/charts/Wastage',
        success: function (result) {
            google.charts.load('current', {
                'packages': ['corechart']
            });
            google.charts.setOnLoadCallback(function () {
                drawChart(result);
            });
        }
    });
});


function drawChart(result) {
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Date');
    data.addColumn('number', 'میزان مواد ');
    data.addColumn('number', 'میزان ضایعات');
    var dataArray = [];
    $.each(result, function (i, obj) {
        dataArray.push([obj.Date, obj.ConsumedAmount, obj.WastageAmount]);
    });

    data.addRows(dataArray);
    var columnChartsOptions = {
        title: "جدول ضایعات",
        width: 1000,
        height: 400,
        bar: { groupWidth: "20%" }
    }

    var chartType = $("#filter_chartType").find("option:selected").attr("value");

    if (chartType == "columnChart") {
        var columnChart = new google.visualization.ColumnChart(document.getElementById('columnchart_div'));
    } else if (chartType == "lineChart") {
        var columnChart = new google.visualization.LineChart(document.getElementById('columnchart_div'));
    }
    
    //alert();

    columnChart.draw(data, columnChartsOptions);

}


function initializeYearSelect() {
    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        url: '/production/charts/GetYearsOfProduction',
        success: function (result) {
            $.each(result, function (i, obj) {
                //alert(obj);
                year_select.append($("<option  value=" + obj + " >" + obj + "</option>"));
            });
        }
    });
}

function bindEventToClick() {
    $("#btn_drawChart").click(function () {
        var yearSelected = year_select.find("option:selected").attr("value");
        var monthSelected = month_select.find("option:selected").attr("value");
        var saloonSelected = saloon_select.find("option:selected").attr("value");
        var deviceSelected = device_select.find("option:selected").attr("value");
        var productionLineSelected = productionLine_select.find("option:selected").attr("value");
        //alert(yearSelected);
        //alert(monthSelected);
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            url: '/production/charts/DrawWastages?year=' + yearSelected + "&month=" + monthSelected + "&saloonId=" + saloonSelected + "&deviceId=" + deviceSelected + "&productionLineId=" + productionLineSelected,
            success: function (result) {
                setConsumedTextBoxValues(result[0].TotalMaterialConsumed, result[0].MaxConsumed, result[0].MinConsumed, result[0].AverageConsumed);
                setWastageTextBoxValues(result[0].TotalWastage, result[0].MaxWastageAmount, result[0].MinWastage, result[0].AverageWastage);
                google.charts.load('current', {
                    'packages': ['corechart']
                });
                google.charts.setOnLoadCallback(function () {
                    $("#columnchart_div").empty();
                    drawChart(result);
                });
            }
        });

    });
}


function bindEventToSelect_year() {
    year_select.change(function () {

        var yearSelected = $(this).find("option:selected").attr("value");
        var monthSelected = month_select.find("option:selected").attr("value");
        var saloonSelected = month_select.find("option:selected").attr("value");
        var deviceSelected = month_select.find("option:selected").attr("value");
        var productionLineSelected = month_select.find("option:selected").attr("value");

        //alert(yearSelected);
        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            url: '/production/charts/GetMonthOfProduction?year=' + yearSelected,
            success: function (result) {
                month_select.empty();
                month_select.append($("<option value=''>همه ماه ها</option>"));
                if (result.length > 0) {

                    $.each(result, function (i, obj) {
                        //alert(obj);
                        month_select.append($("<option value=" + obj.MonthNumber + ">" + obj.MonthName + "</option>"));
                    });
                }

            }
        });
    });
}

function setConsumedTextBoxValues(totalConsumed, maxConsumed, minConsumed, averageConsumed) {
    $("#total_consumed").val(totalConsumed);
    $("#max_consumed").val(maxConsumed);
    $("#min_consumed").val(minConsumed);
    $("#average_consumed").val(averageConsumed);
}

function setWastageTextBoxValues(totalWastaged, maxWastage, minWastage, averageWastage) {
    $("#total_wastage").val(totalWastaged);
    $("#max_wastage").val(maxWastage);
    $("#min_wastage").val(minWastage);
    $("#average_wastage").val(averageWastage);
}





