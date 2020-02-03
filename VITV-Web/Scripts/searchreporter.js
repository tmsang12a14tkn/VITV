
$(".all_reporter").on("click","#buttonsearch", function (e) {
    $("#browse-results").load("/Reporter/Search?searchText=" + $("#searchinput").val());
});
$("#searchinput").keyup(function (e) {
    if (e.keyCode == 13) {
        $("#browse-results").load("/Reporter/Search?searchText=" + $(this).val());
    }
});

$("#filters-sort").on("change", function () {
    $("#browse-results").load("/Reporter/Filter?type=" + $(this).val());
});