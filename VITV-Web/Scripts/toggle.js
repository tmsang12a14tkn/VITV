$(function () {
    if ($(".navbar-toggle").is(":visible")) {
        $(".navbar-toggle").on("click", function () {
            $(this).toggleClass("active");
        });

        if ($('#buttonsearch').length) {
            $("#buttonsearch").on("click", function () {
                $("#all_reporter_filter").collapse("hide");
                $(".navbar-toggle").removeClass("active");
            });
        }

        if ($('#buttonfilter').length) {
            $("#buttonfilter").on("click", function () {
                $("#related_video_filter").collapse("hide");
                $(".navbar-toggle").removeClass("active");
            });
        }

        if ($('#allprograme_filter').length) {
            $(".navbar-nav").on("click", function () {
                $("#allprograme_filter").collapse("hide");
                $(".navbar-toggle").removeClass("active");
            });
        }

        if ($('#allvideo_filter').length) {
            $("#filterBtn").on("click", function () {
                $("#allvideo_filter").collapse("hide");
                $(".navbar-toggle").removeClass("active");
            });
        }

        if ($('.select_video_category').length) {
            $(".select_video_category").on("change", function () {
                var _targetId = $(this).attr("id").replace('select_video_category-', '');
                $('#related_video_filter-' + _targetId).collapse('hide');
                $(".navbar-toggle").removeClass("active");
            });
        }
    }
});