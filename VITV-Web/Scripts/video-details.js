$(document).ready(function () {
    var social_selected;
    var iOS = (navigator.userAgent.match(/(iPad|iPhone|iPod)/i) ? true : false);
    var mainUri = $('#mainUri').val().split('?')[0];
    var arr_time = [];

    $(".single_transcript").each(function () {
        arr_time.push($(this).data('seektime'));
    });

    $(window).load(function () {
        $.mCustomScrollbar.defaults.scrollButtons.enable = true; //enable scrolling buttons by default
        $(".transcript_timeline").mCustomScrollbar({ theme: "rounded", autoDraggerLength: false });

        stylingVideo();
    });

    $(window).on('resize', function () {
        stylingVideo();
    });

    $(document).on('click', '#full-transcript', function (e) {
        e.preventDefault();
        $('.transcript_content').fadeOut(200, function () {
            $('.transcript_full').show();
            $('.transcript_content').fadeIn("slow");
            $('#full-transcript').hide();
        });
    });

    $(document).on('click', '#transcript-return-btn', function (e) {
        e.preventDefault();
        $('.transcript_content').fadeOut(200, function () {
            $('.transcript_full').hide();
            $('.transcript_content').fadeIn("slow");
            $('#full-transcript').show();
        });
    });

    var myPlayer = videojs("video", {}, function () {
       
        var player = this;

        var timeParam = $('#timeParam').val();
        var isSeek = parseInt(timeParam) != 0;
        if (player.techName_ == "Html5" && !isSeek) {
            player.preroll({
                data: jQuery.parseJSON(player.el().dataset.preroll),
                adsOptions: { debug: false }
            });
        }
        //this.skipbtn();

        
        //var skipIntroTime = $('#skipIntroTime').val();

        //if (skipIntroTime <= 0) {
        //    $('.vjs-skip-ads').hide();
        //}

        if (isSeek) {
            player.play();
            player.pause();
            player.currentTime(timeParam);
            player.play();
        }

        $('.jumptotime').on("click", function (e) {
            e.preventDefault();

            var seconds = $(this).data('time');
            player.play();

            var playLength = player.duration();
            if (seconds > playLength) {
                seconds = playLength - 1;
            }

            player.pause();
            player.currentTime(seconds);
            player.play();
        });

        player.on('timeupdate', function () {
            var cur_time = this.currentTime();
            if ($('.view-count .dropdown-menu').hasClass('in')) {
                $('#cur-time').val(formatTime(cur_time));

                if ($('#share-time').is(':checked'))
                    $('#share-link').val(mainUri + '?t=' + Math.round(this.currentTime()));
                else
                    $('#share-link').val(mainUri);
            }

            if ($(".single_transcript[data-seektime='" + Math.floor(cur_time) + "']").length > 0) {
                $(".single_transcript[data-seektime='" + Math.floor(cur_time) + "']").addClass('active');
                $(".single_transcript[data-seektime='" + Math.floor(cur_time) + "']").siblings().removeClass('active');
            } else {
                var closest_time = findClosestTime(Math.floor(cur_time));
                $(".single_transcript[data-seektime='" + closest_time + "']").addClass('active');
                $(".single_transcript[data-seektime='" + closest_time + "']").siblings().removeClass('active');
            }

            if ($(".transcript-single-row[data-seektime='" + Math.floor(cur_time) + "']").length > 0) {
                $(".transcript-single-row[data-seektime='" + Math.floor(cur_time) + "']").addClass('active');
                $(".transcript-single-row[data-seektime='" + Math.floor(cur_time) + "']").siblings().removeClass('active');
            } else {
                var closest_time = findClosestTime(Math.floor(cur_time));
                $(".transcript-single-row[data-seektime='" + closest_time + "']").addClass('active');
                $(".transcript-single-row[data-seektime='" + closest_time + "']").siblings().removeClass('active');
            }

            //if (this.currentTime() >= skipIntroTime) {
            //    $('.vjs-skip-ads').hide();
            //} else {
            //    $('.vjs-skip-ads').show();
            //}
        });

        //$('.vjs-skip-ads').on('click', function (e) {
        //    e.preventDefault();
        //    player.play();
        //    player.pause();
        //    player.currentTime(skipIntroTime);
        //    player.play();
        //    $(this).hide();
        //});
    });

    $(document).on('click', '.single_transcript', function (e) {
        e.preventDefault();
        if (myPlayer.preroll.adDone != false) {
            myPlayer.play();
            myPlayer.pause();
            myPlayer.currentTime($(this).data('seektime'));
            myPlayer.play();

            var className = $(this).attr('class');
            if (className == 'btn-seek-time') {
                $(this).parent('li').addClass('active');
                $(this).parent('li').siblings().removeClass('active');
            } else {
                $(this).addClass('active');
                $(this).siblings().removeClass('active');
            }
        }
    });

    $(document).on('click', '.transcript-single-row .content', function (e) {
        e.preventDefault();
        if (myPlayer.preroll.adDone != false) {
            myPlayer.play();
            myPlayer.pause();
            myPlayer.currentTime($(this).parent('div').data('seektime'));
            myPlayer.play();

            $(this).parent('div').addClass('active');
            $(this).parent('div').siblings().removeClass('active');
        }
    });

    $('.social-btn').on('click', function (e) {
        e.preventDefault();
        var $collapse = $(this).closest('.collapse-group').find('.collapse');
        social_selected = $(this).data('social');
        $collapse.collapse('toggle');
        $('#cur-time').val(formatTime(myPlayer.currentTime()));
        $('#share-time').prop('checked', false);
        $('#share-link').val(mainUri);
    });

    $('#share-time').on('click', function () {
        if ($(this).is(':checked'))
            $('#share-link').val(mainUri + '?t=' + parseStringTime($('#cur-time').val()));
        else
            $('#share-link').val(mainUri);
    });

    $('#cur-time').on('input', function () {
        var time = $(this).val();
        if ($('#share-time').is(':checked')) {
            if (checkRegex(time))
                $('#share-link').val(mainUri + '?t=' + parseStringTime(time));
            else
                $('#share-link').val(mainUri);
        }
    });

    $('#btn-share').on('click', function () {
        var link = $('#share-link').val();
        if (social_selected == 'fb') {
            var fb = 'https://www.facebook.com/sharer/sharer.php?u=' + link;
            window.open(fb, 'VITV | vitv.vn', 'width=600,resizable=yes,height=650');
        } else if (social_selected == 'gp') {
            var gp = 'https://plus.google.com/share?url=' + link;
            window.open(gp, 'VITV | vitv.vn', 'menubar=no,toolbar=no,resizable=yes,scrollbars=yes,height=600,width=600');
        } else if (social_selected == 'twitter') {
            var tw = 'https://twitter.com/share?url=' + link + '&text=@(Model.Title)';
            window.open(tw, 'popupwindow', 'scrollbars=yes,width=600,height=400');
        }

        $('.collapse-group').find('.collapse').removeClass('in');

        return false;
    });

    function stylingVideo() {
        var oWidth = $(".video_box").data('owidth');
        var oHeight = $(".video_box").data('oheight');
        var windowW = $(window).width();
        var rate = width = height = 0;

        if (windowW >= 1200) {
            width = oWidth;
            height = oHeight;
        }
        else if (windowW <= 1199 && windowW > 665) {
            rate = (340 * 100) / oHeight;
            height = Math.ceil((oHeight * rate) / 100);
            width = Math.ceil((oWidth * rate) / 100);
        }
        else if (windowW <= 665 && windowW > 578) {
            rate = (300 * 100) / oHeight;
            height = Math.ceil((oHeight * rate) / 100);
            width = Math.ceil((oWidth * rate) / 100);
        }
        else if (windowW < 578 && windowW >= 520) {
            rate = (260 * 100) / oHeight;
            height = Math.ceil((oHeight * rate) / 100);
            width = Math.ceil((oWidth * rate) / 100);
        }
        else if (windowW < 520 && windowW >= 420) {
            rate = (220 * 100) / oHeight;
            height = Math.ceil((oHeight * rate) / 100);
            width = Math.ceil((oWidth * rate) / 100);
        }
        else if (windowW < 420 && windowW >= 380) {
            rate = (180 * 100) / oHeight;
            height = Math.ceil((oHeight * rate) / 100);
            width = Math.ceil((oWidth * rate) / 100);
        }
        else if (windowW < 380) {
            rate = (150 * 100) / oHeight;
            height = Math.ceil((oHeight * rate) / 100);
            width = Math.ceil((oWidth * rate) / 100);
        }

        $(".video_box").height(height);
        $(".transcript_box").height(height);
        $(".transcript_timeline").height(height - 45);

        $('.vjs-control-bar').width(width);
        $('.vjs-skip-ads').width(width);
    }

    function checkRegex(val) {
        var regex = /(?:(?:([01]?\d|2[0-3]):)?([0-5]?\d):)?([0-5]?\d)/;
        return regex.test(val);
    }

    function parseStringTime(val) {
        var times = val.split(':');
        var result = 0;

        if (times.length == 3)
            result = (+times[0]) * 60 * 60 + (+times[1]) * 60 + (+times[2]);
        else
            result = (+times[0]) * 60 + (+times[1]);

        return result;
    }

    function formatTime(s) {
        var hours = Math.floor(s / 60 / 60),
            minutes = Math.floor((s - (hours * 60 * 60)) / 60),
            seconds = Math.floor(s - (hours * 60 * 60) - (minutes * 60));
        return ((hours > 0) ? hours + ':' : '') + ((minutes < 10) ? '0' + minutes : minutes) + ':' + ((seconds < 10) ? '0' + seconds : seconds);
    }

    function findClosestTime(s) {
        var i = arr_time.length;
        while (arr_time[--i] > s);
        return arr_time[i];
    }
});