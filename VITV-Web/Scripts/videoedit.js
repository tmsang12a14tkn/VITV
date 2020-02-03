
$(function () {
    //var sublimeplayer;
    ////SUBLIME
    //var prepareVideo = function ()
    //{
    //    // You can safely call the sublime API methods here.
    //    sublime.prepare('video', function (player) {
    //        sublimeplayer = player;
    //        player.play();
    //        player.on("pause", function() {
    //            console.log("pause");
    //            $("input[type=button]#setThumbnailBtn").removeAttr("disabled");

    //        });

    //        player.on("play", function() {
    //            console.log("play");
    //            $("input[type=button]#setThumbnailBtn").attr("disabled", "disabled");
    //        });

    //        $("#setThumbnailBtn").on("click", function() {
    //            if ($("#Url").length > 0 && player != null) {
    //                //console.log(videoplayer.currentTime);
    //                jQuery.get("/Video/GetThumbnail", { videoUrl: $("#Url").val(), time: player.playbackTime() })
    //                    .done(function(data) {
    //                        if (data.success) {
    //                            $("#Thumbnail").val(data.url);
    //                            $("#MobileThumbnail").val(data.mobile_url);
    //                            $("#thumbnailImg").attr("src", data.url);
    //                        } else {
    //                            alert(data.error);
    //                        }
    //                    });
    //            } else {
    //                alert("Không thể đặt ảnh đại diện");
    //            }
    //        });
    //    });
    //};

    //var unprepareVideo = function() {
    //    sublime.unprepare('video');
    //    sublimeplayer = null;
    //    $("#setThumbnailBtn").off("click");
    //};

    //sublime.ready(function() {
    //    prepareVideo();
    //});

    //END SUBLIME

    var videoplayer = document.getElementById("video");
    if (videoplayer != null) {
        if (videoplayer.currentSrc)
            $("input[type=button]#setThumbnailBtn").removeAttr("disabled");

        videoplayer.addEventListener("pause", function () {
            $("input[type=button]#setThumbnailBtn").removeAttr("disabled");
        });

        videoplayer.addEventListener("emptied", function () {
            console.log("emptied");
        });


        videoplayer.addEventListener("play", function () {
            $("input[type=button]#setThumbnailBtn").attr("disabled", "disabled");
        });

        $("#setThumbnailBtn").on("click", function () {
            if ($("#Url").length > 0 && videoplayer != null) {
                //console.log(videoplayer.currentTime);
                jQuery.get("/Video/GetThumbnail", { videoUrl: $("#Url").val(), time: videoplayer.currentTime })
                    .done(function (data) {
                        if (data.success) {
                            $("#MobileThumbnail").val(data.mobile_url);
                            $("#Thumbnail").val(data.url);
                            $("#thumbnailImg").attr("src", data.url);
                        } else {
                            alert(data.error);
                        }
                    })
            } else {
                alert("Không thể đặt ảnh đại diện");
            }
        });

        var replaceVideo = function (url, duration) {
            videoplayer.pause();
            videoplayer.src = url;

            videoplayer.load();
            videoplayer.controls = true;

            $("#Url").val(url);
            $("#Duration").val(duration);
        };

        //var replaceVideo = function (url) {
        //    if (sublimeplayer != null) {
        //        sublimeplayer.pause();
        //        unprepareVideo();
        //        videoplayer.src = url;

        //        prepareVideo();
        //        $("#Url").val(url);
        //    }
        //};

        $('#videoupload').fileupload({
            dataType: 'json',
            url: '/UploadHandler/VideoUploadHandler.ashx',
            done: function (e, data) {
                var result = data.result;
                if (result.success) {
                    replaceVideo(result.videourl, result.duration);
                } else {
                    if (result.errorcode == 1)//video ton tai
                    {
                        var tempVideoUrl = result.tempvideourl;
                        var videoUrl = result.videourl;

                        $('#existFileModal').modal();

                        $('#modalReuseBtn').on("click", function () {
                            $('#existFileModal').modal('hide');
                            jQuery.get("/Video/DeleteTempFile", { tempVideoUrl: tempVideoUrl })
                                .done(function (data) {
                                    if (data.success) {
                                        $("#Duration").val(result.duration);
                                        replaceVideo(videoUrl, result.duration);
                                        console.log("Xóa tập tin tạm và sử dụng tập tin có sẵn");
                                    }
                                });
                            $('#modalReuseBtn').off("click");
                            $('#modalOverrideBtn').off("click");
                            $('#modalCancelBtn').off("click");
                        });
                        $('#modalOverrideBtn').on("click", function () {
                            $('#existFileModal').modal('hide');
                            replaceVideo("", "");
                            jQuery.get("/Video/ReplaceVideo", { videoUrl: videoUrl, tempVideoUrl: tempVideoUrl })
                                .done(function (data) {
                                    if (data.success) {
                                        replaceVideo(videoUrl, result.duration);
                                        console.log("Ghi đè tập tin");
                                    }
                                });
                            $('#modalReuseBtn').off("click");
                            $('#modalOverrideBtn').off("click");
                            $('#modalCancelBtn').off("click");
                        });
                        $('#modalCancelBtn').on("click", function () {
                            $('#existFileModal').modal('hide');
                            jQuery.get("/Video/DeleteTempFile", { tempVideoUrl: tempVideoUrl })
                                .done(function (data) {
                                    if (data.success) {
                                        console.log("Hủy và xóa tập tin tạm");
                                    }
                                });
                            $('#modalReuseBtn').off("click");
                            $('#modalOverrideBtn').off("click");
                            $('#modalCancelBtn').off("click");
                        });
                    }
                    else if (result.errorcode == 2) {
                        console.log(result);
                    }
                }
            },
            fail: function (e, data) {
                console.log("Error", e, data);
                alert("Lỗi trong quá trình tải tập tin");
            },
            progressall: function (e, data) {
                var progress = parseInt(data.loaded / data.total * 100, 10);
                $('#video-progress .bar').css(
                    'width',
                    progress + '%'
                );
            }
        });

        //$('#photoupload').fileupload({
        //    dataType: 'json',
        //    url: '/UploadHandler/PhotoUploadHandler.ashx',
        //    done: function (e, data) {
        //        if (data.result.success) {
        //            console.log(data.result.videourl);
        //            $("#thumbnailImg").attr("src", data.result.videourl);
        //            $("#Thumbnail").val(data.result.videourl);
        //        }
        //        else {
        //            alert("Xảy ra lỗi trong quá trình tải ảnh: " + data.result.error);
        //        }

        //    },

        //    progressall: function (e, data) {
        //        var progress = parseInt(data.loaded / data.total * 100, 10);
        //        $('#photo-progress .bar').css(
        //            'width',
        //            progress + '%'
        //        );
        //    }
        //});

        function readURL(input) {

            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#thumbnailImg').attr('src', e.target.result);
                }

                reader.readAsDataURL(input.files[0]);
            }
        }

        $("#photoinput").change(function () {
            readURL(this);
        });
    }
})