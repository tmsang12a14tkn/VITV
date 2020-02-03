$(document).ready(function () {
    //Week
    $contentLoadTriggeredWeek = false;
    $("#week").scroll(
        function () {
            
            if ($("#week").scrollTop() >= ($("#wrapperDivWeek").height() - $("#week").height()) && $contentLoadTriggeredWeek == false) {
                $contentLoadTriggeredWeek = true;
                    
                var date = $("#wrapperDivWeek > p").last().data("date");
                $.post("/VideoAccess/GetScrollWeek", { date: date }, function (data) {
                        $contentLoadTriggeredWeek = false;
                        $("#wrapperDivWeek").append(data);
                    });
                }
        }
    );


    //Month
    $contentLoadTriggeredMonth = false;
    $("#month").scroll(
        function () {

            if ($("#month").scrollTop() >= ($("#wrapperDivMonth").height() - $("#month").height()) && $contentLoadTriggeredMonth == false) {
                $contentLoadTriggeredMonth = true;

                var date = $("#wrapperDivMonth > p").last().data("date");
                $.post("/VideoAccess/GetScrollMonth", { date: date }, function (data) {
                    $contentLoadTriggeredMonth = false;
                    $("#wrapperDivMonth").append(data);
                });
            }
        }
    );


    //Quarter
    $contentLoadTriggeredQuarter = false;
    $("#quarter").scroll(
        function () {

            if ($("#quarter").scrollTop() >= ($("#wrapperDivQuarter").height() - $("#quarter").height()) && $contentLoadTriggeredQuarter == false) {
                $contentLoadTriggeredQuarter = true;

                var date = $("#wrapperDivQuarter > p").last().data("date");
                $.post("/VideoAccess/GetScrollQuarter", { date: date }, function (data) {
                    $contentLoadTriggeredQuarter = false;
                    $("#wrapperDivQuarter").append(data);
                });
            }
        }
    );

    //Year
    $contentLoadTriggeredYear = false;
    $("#year").scroll(
        function () {

            if ($("#year").scrollTop() >= ($("#wrapperDivYear").height() - $("#year").height()) && $contentLoadTriggeredYear == false) {
                $contentLoadTriggeredYear = true;

                var date = $("#wrapperDivYear > p").last().data("date");
                $.post("/VideoAccess/GetScrollYear", { date: date }, function (data) {
                    $contentLoadTriggeredYear = false;
                    $("#wrapperDivYear").append(data);
                });
            }
        }
    );
});