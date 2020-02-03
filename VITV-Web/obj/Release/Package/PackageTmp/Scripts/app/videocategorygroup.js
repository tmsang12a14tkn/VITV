$(function () {
    var tabs = $("#tabserver").tabs();
    tabs.find(".ui-tabs-nav").sortable({
        items: "li:not(.btnfuncsortable, .itemvct)",
        axis: "x",
        update: function (event, ui) {
            tabs.tabs("refresh");
            $('#ordereditvcgroup').addClass("yellow");
        }
    });
});
$("#tabserver").tabs({
    create: function (event, ui) {
        console.log("create");
        var active = $('#tabserver').tabs('option', 'active');
        var namevcg = $("#tabserver ul>li a").eq(active).attr("data-namevcg");
        var vcgid = $("#tabserver ul>li a").eq(active).attr("data-vcgid");
        var type = $("#tabserver ul>li a").eq(active).attr("data-type");
        $('#editcategorytab').attr("data-type", type);
        $('#editcategorytab').attr("data-vcgid", vcgid);
        $('#editcategorytab').attr("data-namevcg", namevcg);
    },
    activate: function (event, ui) {
        var active = $('#tabserver').tabs('option', 'active');
        var namevcg = $("#tabserver ul>li a").eq(active).attr("data-namevcg");
        var vcgid = $("#tabserver ul>li a").eq(active).attr("data-vcgid");
        var type = $("#tabserver ul>li a").eq(active).attr("data-type");
        $('#editcategorytab').attr("data-type", type);
        $('#editcategorytab').attr("data-vcgid", vcgid);
        $('#editcategorytab').attr("data-namevcg", namevcg);
    }
});

$(".table .sortablecatgroup").sortable(
    {
        update: function (event, ui) {
            $('#saveordereditvc' + $(this).data('vcgid')).addClass("yellow");
        },
        change: function (event, ui) {
            
        }
    });

var parent = null;
var parentsave = null;
$(document).ready(function () {
    //Chú ý: trường hợp click tooltip 2 lần mới hiện lên là do chưa để thuộc tinh title trong selection
    $('#editcategorytab').tooltipster({
        theme: 'tooltipster-shadow',
        contentAsHTML: true,
        autoClose: true,
        trigger: 'click',
        interactive: true,
        animation: 'fade',
        delay: 0,
        maxWidth: 400,
        speed: 100,
        functionBefore: function (origin, continueTooltip) {

            // we'll make this function asynchronous and allow the tooltip to go ahead and show the loading notification while fetching our data
            continueTooltip();
            parent = $(this);
            var type = parent.attr("data-type");
            var vcgid = parent.attr("data-vcgid");
            var namevcg = parent.attr("data-namevcg");
            var content = "";
            if (type == 1)
                content = GetContentTooltip("Chỉnh sửa nhóm chuyên mục", "Tên nhóm chuyên mục", "Nhập tên nhóm chuyên mục", vcgid, namevcg, type);
            else
                content = GetContentTooltip("Chỉnh sửa thể loại chương trình", "Tên thể loại chương trình", "Nhập tên thể loại chương trình", vcgid, namevcg, type);
            origin.tooltipster('content', content);



        },
        functionReady: function (origin, tooltipEl) {
            $(tooltipEl).on('click', '#savevcg', function () {
                var type = $(this).data('type');
                var namevcg = $('#namevcg').val();
                var vcgid = $(this).data('vcgid');
                if (namevcg != "") {
                    $.post("/VideoCatGroups/Edit", { vcgid: vcgid, namevcg: namevcg, type: type }, function (result) {
                        if (result.success == 1) {
                            if (result.type == 1) {
                                $('#namevcg' + result.vcgid).empty().html("<b>" + result.namevcg + "</b>");
                                $('#namevcg' + result.vcgid).attr("data-vcgid", result.vcgid);
                                $('#namevcg' + result.vcgid).attr("data-namevcg", result.namevcg);
                            }
                            else {
                                $('#namevct' + result.vcgid).empty().html("<b>" + result.namevcg + "</b>");
                                $('#namevct' + result.vcgid).attr("data-vcgid", result.vcgid);
                                $('#namevct' + result.vcgid).attr("data-namevcg", result.namevcg);
                            }
                            parent.attr("data-vcgid", result.vcgid);
                            parent.attr("data-namevcg", result.namevcg);
                        } else {
                            alert(result.text);
                        }
                        origin.tooltipster("hide");
                    });
                } else {
                    if (type == 1)
                        alert("Nhóm chuyên mục không được phép trống !")
                    else
                        alert("Thể loại chương trình không được phép trống !")
                }

            });
        }
    });

    function GetContentTooltip(titlemodal, titlelabel, placeholder, vcgid, namevcg, type) {
        return '<div class="md-content">' +
                    '<div class="modal-header theadvideo">' +
                        '<h4 class="modal-title"><b><i class="fa fa-pencil fa-lg"></i> ' + titlemodal + '</b></h4>' +
                    '</div>' +
                    '<div class="modal-body">' +
                        '<div class="form-group">' +
                            '<label><b>' + titlelabel + '</b></label>' +
                            '<input type="text" class="form-control" id="namevcg" placeholder="' + placeholder + '" value="' + namevcg + '"/>' +
                        '</div>' +
                    '</div>' +
                    '<div class="modal-footer">' +
                        '<button type="button" id="savevcg" name="savevcg" class="btn btn-success" data-type="' + type + '" data-vcgid="' + vcgid + '"><span class="fa fa-save"></span> Lưu thay đổi</button>' +
                    '</div>' +
                '</div>';
    }

    $('#ordereditvc').tooltipster({
        content: "<b>...</b>",
        theme: 'tooltipster-shadow',
        contentAsHTML: true,
        autoClose: false,
        trigger: 'click',
        interactive: true,
        animation: 'fade',
        delay: 0,
        maxWidth: 400,
        speed: 100,
        timer: 3000,
        functionBefore: function (origin, continueTooltip) {

            // we'll make this function asynchronous and allow the tooltip to go ahead and show the loading notification while fetching our data
            continueTooltip();
            if (origin.data('ajax') !== 'cached') {
                parentsave = $(this);
                var vcgid = parentsave.data('vcgid');
                var data = new Array();
                $('#tbodyvcg' + vcgid + " tr").each(function () {
                    if ($.inArray($(this).attr("data-vcid")) == -1)
                        data.push($(this).attr("data-vcid"))
                });
                if (data.length > 1) {
                    $.post("/VideoCategory/ChangeOrder", { ids: data }, function (result) {
                        var content = '<div class="green"><i class="fa fa-check-circle fa-fw fa-lg"></i>' +
                            '<strong>' + result.text + '</strong></div>';
                        origin.tooltipster('content', content);
                        $('#saveordereditvc' + vcgid).removeClass("yellow");
                    });
                }

            }


        },
        functionAfter: function (origin, continueTooltip) {
            origin.tooltipster('content', "<b>...</b>");
        }
    });

    $('#ordereditvcgroup').tooltipster({
        content: "<b>...</b>",
        theme: 'tooltipster-shadow',
        contentAsHTML: true,
        autoClose: false,
        trigger: 'click',
        interactive: true,
        animation: 'fade',
        delay: 0,
        maxWidth: 400,
        speed: 100,
        timer: 3000,
        functionBefore: function (origin, continueTooltip) {

            // we'll make this function asynchronous and allow the tooltip to go ahead and show the loading notification while fetching our data
            continueTooltip();
            if (origin.data('ajax') !== 'cached') {
                var data = new Array();
                $('#tabvcg .itemvcg').each(function () {
                    if ($.inArray($(this).attr("data-vcgid")) == -1)
                        data.push($(this).attr("data-vcgid"))
                });
                if (data.length > 1) {
                    $.post("/VideoCatGroups/ChangeOrder", { ids: data }, function (result) {
                        var content = '<div class="green"><i class="fa fa-check-circle fa-fw fa-lg"></i>' +
                            '<strong>' + result.text + '</strong></div>';
                        origin.tooltipster('content', content);
                        $('#ordereditvcgroup').removeClass("yellow");
                    });
                }

            }


        },
        functionAfter: function (origin, continueTooltip) {
            origin.tooltipster('content', "<b>...</b>");
        }
    });

    $('.dayofweektooltip').tooltipster({
        theme: 'tooltipster-shadow',
        autoClose: true,
        trigger: 'hover',
        animation: 'fade'
    });
    
    $('.vtooltip').each(function () {
        var highest = $(this).data("highest");
        var lowest = $(this).data("lowest");
        var content =
                    '<div>'
                        + 'Cao nhất: ' + highest
                        + '</br>' + 'Thấp nhất: ' + lowest
                    + '</div>';
        $(this).tooltipster(
        {
            contentAsHTML: true,
            theme: 'tooltipster-shadow',
            content: content
        });
    });

    $("#ordereditvc").on("click", function (e) {
        e.preventDefault();
    });
    //$("#selectcategory").on("change", function () {
    //    var type = $(this).val();
    //    window.location.href = "/VideoCatGroups/Index?type=" + type;

    //});

    $("#addcategorytab").click(function (e) {
        e.preventDefault();
        //window.location.href = "/VideoCatGroups/Create";
        window.open('/VideoCatGroups/Create', '_blank');
    });

    $('#sidebar-nav #videoMng').toggleClass("active");
    $('#sidebar-nav #videoMng').siblings().removeClass("active");

    $('#sidebar-nav a#vd-li-1').toggleClass("active");
    $('#sidebar-nav a#vd-li-1').parent().siblings().removeClass("active");

})