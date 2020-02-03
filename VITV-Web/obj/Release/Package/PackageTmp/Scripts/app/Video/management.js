var parent = null;
$(document).ready(function () {
    $('.youtubeplay').tooltipster({
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

            parent = $(this).parent().parent();
            var videoid = parent.data('videoid');
            var youtubeurl = parent.data('youtubeurl');
            var videopiority = parent.data('videopiority');

            var content =
                '<div class="col-lg-12">' +
                        '<form role="form">' +
                            '<div class="row">' +
                                '<div class="input-group">' +
                                    '<span class="input-group-btn">' +
                                        '<button class="btn btn-primary" id="emptyyoutubeurl" type="button">Xoá</button>' +
                                    '</span>' +
                                    '<input type="text" class="form-control" id="youtubeurl" placeholder="Đường dẫn youtube" value="' + youtubeurl + '"/>' +
                                '</div>' +
                            '</div>' +
                            '<div class="row">' +
                                '<div class="radio">' +
                                    '<input type="radio" name="optionsRadios" id="optionsRadios1" value="0" ' + (videopiority == 0 ? "checked" : "") + '/>' +
                                    '<label for="optionsRadios1">Từ youtube</label>' +
                                '</div>' +
                                '<div class="radio">' +
                                    '<input type="radio" name="optionsRadios" id="optionsRadios2" value="1" ' + (videopiority == 1 ? "checked" : "") + '/>' +
                                    '<label for="optionsRadios2">Từ lưu trữ VITV</label>' +
                                '</div>' +
                            '</div>' +
                            '<div class="row" id="containeryoutube">' +
                                '<div class="col-lg-4">' +
                                    '<input type="button" id="saveyoutubeplay" name="saveyoutubeplay" class="btn btn-primary" data-videoid="' + videoid + '" value="Lưu thay đổi">' +
                                '</div>' +
                                '<div class="col-lg-4">' +
                                    '<input type="button" id="viewvideo" class="btn btn-primary" value="Xem video">' +
                                '</div>' +
                            '</div>' +
                        '</form>' +
                '</div>';
            origin.tooltipster('content', content);

        },
        functionReady: function (origin, tooltipEl) {
            $(tooltipEl).on('click', '#saveyoutubeplay', function () {
                var youtubeurl = $('#youtubeurl').val();
                var videopiority = $('input[name=optionsRadios]:checked').val();
                var videoid = $(this).data('videoid');

                $.post("/Video/UpdateYoutubePlay", { videoid: videoid, youtubeurl: youtubeurl, videopiority: videopiority }, function (result) {
                    if (result.code == 0) {
                        alert("Không tồn tại video này !");
                    } else if (result.code == 1) {
                        $("#youtubeplay" + videoid).removeClass("red").addClass("green");
                        $("#youtubeplay" + videoid + " span").attr("title", "Xem video từ youtube");
                        parent.data('youtubeurl', result.YoutubeUrl);
                        parent.data('videopiority', result.VideoPiority);
                    } else if (result.code == 2) {
                        $("#youtubeplay" + videoid).removeClass("red").removeClass("green");
                        $("#youtubeplay" + videoid + " span").attr("title", "Xem video từ lưu trữ VITV");
                        parent.data('youtubeurl', result.YoutubeUrl);
                        parent.data('videopiority', result.VideoPiority);
                    }
                    else if (result.code == 3) {
                        $("#youtubeplay" + videoid).removeClass("green").addClass("red");
                        $("#youtubeplay" + videoid + " span").attr("title", "Không có đường dẫn youtube");
                        parent.data('youtubeurl', result.YoutubeUrl);
                        parent.data('videopiority', result.VideoPiority);
                    }
                    origin.tooltipster("hide");
                });
            });

            $(tooltipEl).on('click', '#emptyyoutubeurl', function () {
                $('#youtubeurl').val("");
            });

            $(tooltipEl).on('click', '#viewvideo', function () {
                if ($('#youtubeurl').val() != "")
                    window.open($('#youtubeurl').val(), "_blank");
            });
        }
    });

    $('.videouploadtime').tooltipster({
        theme: 'tooltipster-shadow',
        contentAsHTML: true,
        autoClose: true,
        trigger: 'click',
        interactive: true,
        animation: 'fade',
        delay: 0,
        fixedWidth: 200,
        maxWidth: 800,
        speed: 100,
        functionBefore: function (origin, continueTooltip) {

            // we'll make this function asynchronous and allow the tooltip to go ahead and show the loading notification while fetching our data
            continueTooltip();

            var uploadtime = $(this).data('uploadtime');
            var publishtime = $(this).data('publishtime');
            var displaytime = $(this).data('displaytime');
            var uploader = $(this).data('uploader');
            var difference = $(this).data('difference');
            var type = $(this).data('type');
            var result = "";
            //vuot qua 60ph
            if (type == 1) {

                result = '<hr>' +
                    '<div class="row"  style="margin-bottom:15px">' +
                        '<div class="col-lg-6">' +
                            '<span><b>Chênh lệch</b></span>' +
                        '</div>' +
                        '<div class="col-lg-6">' +
                            '<span>Sau thời gian phát sóng là ' + difference + '</span>' +
                        '</div>' +
                    '</div>';
            //upload truoc thoi han
            } else if (type == 2) {
                result = '<hr>' +
                    '<div class="row"  style="margin-bottom:15px">' +
                        '<div class="col-lg-6">' +
                            '<span><b>Chênh lệch</b></span>' +
                        '</div>' +
                        '<div class="col-lg-6">' +
                            '<span>Trước thời gian phát sóng là ' + difference + '</span>' +
                        '</div>' +
                    '</div>';
            } else//upload dung gio
            {
                result = '<hr>' +
                    '<div class="row"  style="margin-bottom:15px">' +
                        '<div class="col-lg-6">' +
                            '<span><b>Chênh lệch</b></span>' +
                        '</div>' +
                        '<div class="col-lg-6">' +
                            '<span>' + difference + ' (đúng giờ)</span>' +
                        '</div>' +
                    '</div>';
            }

            var content =
                '<div class="col-lg-12">' +
                    '<div class="row"  style="margin-top:15px">' +
                        '<div class="col-lg-6">' +
                            '<span><b>Người tải lên</b></span>' +
                        '</div>' +
                        '<div class="col-lg-6">' +
                            '<span>' + uploader + '</span>' +
                        '</div>' +
                    '</div>' +
                    '<hr>' +
                    '<div class="row">' +
                        '<div class="col-lg-6">' +
                            '<span><b>Thời điểm hẹn giờ đăng</b></span>' +
                        '</div>' +
                        '<div class="col-lg-6">' +
                            '<span>' + publishtime + '</span>' +
                        '</div>' +
                    '</div>' +
                    '<hr>' +
                    '<div class="row">' +
                        '<div class="col-lg-6">' +
                            '<span><b>Thời điểm tải lên</b></span>' +
                        '</div>' +
                        '<div class="col-lg-6">' +
                            '<span>' + uploadtime + '</span>' +
                        '</div>' +
                    '</div>' +
                    '<hr>' +
                    '<div class="row"  style="margin-bottom:15px">' +
                        '<div class="col-lg-6">' +
                            '<span><b>Thời điểm phát sóng</b></span>' +
                        '</div>' +
                        '<div class="col-lg-6">' +
                            '<span>' + displaytime + '</span>' +
                        '</div>' +
                    '</div>' + result +
                '</div>';
            origin.tooltipster('content', content);

        },
    });

    $('.youtubeplay, .ispublished, .videotranscript, .unuseyoutube').click(function (e) {
        e.preventDefault();
    });

    $('.ispublished').click(function () {
        var videoid = $(this).data('videoid');
        $.post("/Video/ShowOrHideVideo", { videoid: videoid }, function (result) {
            $('#listvideoModalContent').empty().html(result);
            $('#listvideoModal').modal("show");
        })
    })

    $('#listvideoModal').on("click", '#closeshowhidevideo', function () {
        var videoid = $(this).data('videoid');
        $.post("/Video/UpdateIsPublishedVideo", { videoid: videoid }, function (result) {
            $('#listvideoModal').modal("hide");
            if (result.success) {
                if (result.ispublished) {
                    $('#ispublished' + videoid).removeClass('danger').addClass('gray');
                } else {
                    $('#ispublished' + videoid).removeClass('gray').addClass('danger');
                }
            } else {
                alert(result.content);
            }
        })
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

});


//function validYoutubeUrl(url) {
//    var p = /^(?:https?:\/\/)?(?:www\.)?youtube\.com\/watch\?(?=.*v=((\w|-){11}))(?:\S+)?$/;
//    var b = /^(?:https?:\/\/)?(?:www\.)?youtube\.com\/watch\?(?=.*v=((\w|-){11}))(?:\S+)?$/;
//    return url.match(p);
//}