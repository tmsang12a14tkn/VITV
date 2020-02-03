(function ($) {
    // Add an option for your plugin.
    $.FroalaEditor.DEFAULTS = $.extend($.FroalaEditor.DEFAULTS, {
        commentDisplay: { "show": "Hiển thị bình luận", "hide": "Ẩn bình luận" },
        commentDisplayDefaultSelection: "show",
        commentDisplaySelection: !1
    });

    
    $.FroalaEditor.RegisterCommand("commentDisplay",
        {
            type: "dropdown",
            title: "Comment Display",
            displaySelection: function (a)
            { return a.opts.commentDisplaySelection },
            displaySelectionWidth: 30,
            defaultSelection: function (a) { return a.opts.commentDisplaySelection },
            html: function () {
                for (var a = '<ul class="fr-dropdown-list">', b = this.opts.commentDisplay, c = 0; c < b.length; c++)
                {
                    var d = b[c];
                    a += '<li><a class="fr-command" data-cmd="commentDisplay" data-param1="' + d + '" title="' + d + '">' + d + "</a></li>"
                }
                return a += "</ul>"
            },
            callback: function (a, b) {  },
            refresh: function (a) {  },
            refreshOnShow: function (a, b)
            {  },
            plugin: "comment"
        });
    $.FroalaEditor.DefineIcon("commentDisplay", { NAME: "eye" })
    // Define the plugin.
    // The editor parameter is the current instance.
    $.FroalaEditor.PLUGINS.comment = function (editor) {
       
        // Public method that is visible in the instance scope.
        function show() {
            var style = document.createElement('style')
            style.type = 'text/css'
            style.id = 'showcommentcss'
            style.innerHTML = '.comment{color:red !important;}'
            document.getElementsByTagName('head')[0].appendChild(style)

            $('.articlecontent, .description-wrapper').removeClass("col-md-12").addClass("col-md-9");
            $("#comments").removeClass("hidden");
        }
        function hide()
        {
            var element = document.getElementById('showcommentcss');
            if (element != null)
                element.parentNode.removeChild(element);

            $('.articlecontent, .description-wrapper').removeClass("col-md-9").addClass("col-md-12")
            $("#comments").addClass("hidden");
        }
        function checkSelection()
        {
            var selectedText = editor.selection.text();
            if (selectedText != "") {
                var $selectedElement = $(editor.selection.element());
                
                console.log($selectedElement.offset().top, $selectedElement.offset().left, editor.$box.offset().top, editor.$box.offset().left);

                $btnComment.css("top", $selectedElement.offset().top - editor.$box.offset().top);
                $btnComment.css("left", editor.$box.width() - 20);
                $btnComment.removeClass("hidden");
            }
            else
            {
                setTimeout(function () { $btnComment.addClass("hidden"); }, 100);
            }
        }

        
        function S4() {
            return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        }
        function guid() {
            return (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
        }
        function selectedText() {
            var text = "";
            if (window.getSelection) {
                text = window.getSelection().toString();
            } else if (document.selection && document.selection.type != "Control") {
                text = document.selection.createRange().text;
            }
            return text != "";
        }
        var highlight = function () {
            var range, sel;
            if (window.getSelection) {
                // IE9 and non-IE
                return makeEditableAndHighlight();

            } else if (document.selection && document.selection.createRange) {
                // IE <= 8 case
                range = document.selection.createRange();
                range.execCommand("BackColor", false, "red");
            }
        }
        $("#articleForm").submit(function (event) {

            saveComments();
            deleteComments();
        });
        $.get("/ArticleComment/GetList?articleId="+editor.opts.articleId, function(data)
        {
            var hasComment = false;
            $(".comment").each(function (i, cmt) {
                hasComment = true;
                var id = $(cmt).data("id");
                for (var i = 0; i < data.length; i++) {
                    var cmt = data[i];
                    if (cmt.Id == id)
                    {
                        var $cmtEl = createComment(cmt.Id, cmt.Avatar, cmt.UserName, cmt.Content);

                        $("#cmtList").append($cmtEl);

                        for (var j = 0; j < cmt.Replies.length; j++) {
                            var reply = cmt.Replies[j];
                            var replyEl = "<li class='clearfix'>"
                                + "<div class='img'>"
                                    + "<img src='" + reply.Avatar + "' width='50'>"
                                + "</div>"
                                + "<div class='reply-author'>" + reply.UserName + "</div>"
                                + reply.Content
                                + "</li>";
                            $cmtEl.find(".reply-list").append(replyEl);
                        }
                    }
                }
            });

            if (hasComment == true) {
                show();
                $("#comment-display-ctrl").val("show");
            }
            //for(var i=0;i<data.length;i++)
            //{
            //    var cmt = data[i];
            //    if($(".articlecontent ."+cmt.Id).length > 0)
            //    {
            //        var $cmtEl = createComment(cmt.Id, cmt.Avatar, cmt.UserName, cmt.Content);

            //        $("#cmtList").append($cmtEl);

            //        for (var j = 0; j < cmt.Replies.length; j++) {
            //            var reply = cmt.Replies[j];
            //            var replyEl = "<li class='clearfix'>"
            //                + "<div class='img'>"
            //                    + "<img src='"+reply.Avatar+"' width='50'>"
            //                + "</div>"
            //                + "<div class='reply-author'>"+reply.UserName+"</div>"
            //                + reply.Content
            //                + "</li>";
            //            $cmtEl.find(".reply-list").append(replyEl);
            //        }
            //    }
            //}
        });
        function saveComments()
        {
            $.post('/ArticleComment/AddComments', {comments: newComments}, function (data)
            {
                saveReplies();
                console.log(data);
            });
        }
        function deleteComments()
        {
            $.post('/ArticleComment/DeleteComments', { comments: deletedComments }, function (data) {
                console.log(data);
            });
        }
        function saveReplies() {
            $.post('/ArticleComment/AddReplies', { replies: newReplies }, function (data) {
                console.log(data);
            });
        }
        var removeComment = function (id) {
            //remove from doc
            var $span = $(document).find('.' + id);
            //$span.tooltip('disable');
            //$span.tooltip('destroy');
            $span.popover('destroy');
            if ($span.prop("tagName") == 'SPAN')
                $span.contents().unwrap();
            else if ($span.prop("tagName") == 'IMG')
                $span.removeClass("bordered-image");

        }
       
        function makeEditableAndHighlight() {

            var range, sel = window.getSelection();
            var uuid = guid();
            if (selectedText() != "" && sel.rangeCount && sel.getRangeAt)
            {
                var styles = null;
                range = sel.getRangeAt(0);
                console.log(range.commonAncestorContainer);
                if (range.commonAncestorContainer.parentNode.nodeName == "SPAN")
                {
                    styles = range.commonAncestorContainer.parentNode.style.cssText;
                }
                editor.commands.applyProperty('color', '');

                sel = window.getSelection();
                range = sel.getRangeAt(0);
                if (range.commonAncestorContainer.nodeName == "#text") {
                    var span = range.commonAncestorContainer.parentNode;
                    $(span).addClass("comment");
                    $(span).addClass(uuid);
                    $(span).attr("data-id", uuid);
                    if (styles != null) $(span).attr("style", styles);
                }
                else if(range.commonAncestorContainer.nodeName == "SPAN")
                {
                    var span = range.commonAncestorContainer;
                    $(span).addClass("comment");
                    $(span).addClass(uuid);
                    $(span).attr("data-id", uuid);
                    if (styles != null) $(span).attr("style", styles);
                }
                else
                {
                    var allWithinRangeParent = range.commonAncestorContainer.getElementsByTagName("span");

                    var allSelected = [];
                    for (var i = 0, el; el = allWithinRangeParent[i]; i++) {
                        if (sel.containsNode(el, true)) {
                            allSelected.push(el);
                            var span = el;
                            var siblings = $(span).siblings("span");
                            if (siblings.length > 0)
                            {
                                styles = siblings.get(0).style.cssText;
                            }
                            $(span).addClass("comment");
                            $(span).addClass(uuid);
                            $(span).attr("data-id", uuid);
                            if (styles != null) $(span).attr("style", styles);
                        }
                    }
                    console.log('All selected =', allSelected);
                }

            }
            return uuid;
        }
        $("#cmtList").on("mouseenter", "li.cmt-wrapper", function () {
            $(this).addClass("highlighted");
            var id = $(this).data("id");
            $cmtElement = $("." + id);
            $cmtElement.addClass("highlighted");
        });
        $("#cmtList").on("mouseleave", "li.cmt-wrapper", function () {
            $(this).removeClass("highlighted");
            var id = $(this).data("id");
            $cmtElement = $("." + id);
            $cmtElement.removeClass("highlighted");
        });

        var newComments = [];
        var newReplies = [];
        var deletedComments = [];

        $("#cmtList").on("keypress", "input.input-reply", function (e) {
            //var $replyInput = $(this).closest("div.input-group").children('input');
            if (e.which == 13) {
                var $replyInput = $(this);
                var replyText = $replyInput.val();
                $replyInput.val('');

                var $replyList = $(this).closest("div.main-box-body").children('ul.reply-list');
                $replyList.append("<li class='clearfix'>"
                    + "<div class='img'>"
                        + "<img src='" + editor.opts.userAvatar + "' width='30'>"
                    + "</div>"
                    + "<div class='reply-author'>" + editor.opts.userName + "</div>"
                    + replyText
                    + "</li>");
                var cmtId = $(this).closest("li.cmt-wrapper").data("id");
                var newReply = { CommentId: cmtId, Content: replyText }
                newReplies.push(newReply);
                return false;
            }
        });
        $("#cmtList").on("click", "button.btn-delcmt", function () {
            //delete comment
            console.log("delete comment");
            var $parentLi = $(this).closest('li');
            var id = $parentLi.data("id");

            removeComment(id);

            for (var i = 0; i < newComments.length; i++)
            {
                if(newComments[i].Id == id)
                {
                    newComments.splice(i, 1);
                    break;
                }
            }
            deletedComments.push(id);
            //remove item on right panel
            $parentLi.remove();
        });
        $("#btn-submitreview").on("click", function ()
        {
            var cmtText = $("#if-comment").contents().find("#txt-articlecomment").val();

            //highlight
            var uuid = highlight();
               
            //add comment to right box
            var cmtPointer = createComment(uuid, editor.opts.userAvatar, editor.opts.userName, cmtText);

            cmtPointer.appendTo("#cmtList");

            //reset and close comment box
            //if (iwin.getSelection)
            //    iwin.getSelection().removeAllRanges();
            //else
            //    idoc.selection.empty();
            $("#if-comment").contents().find("#txt-articlecomment").val('');
            $('#comment-box').addClass('closed');

            var newComment = {
                Id: uuid, Content: cmtText, articleId: editor.opts.articleId
            }
            newComments.push(newComment);

        });
        function createComment(uuid, avatar, userName, cmtText)
        {
            return $("<li class='clearfix cmt-wrapper' data-id=" + uuid + ">"
                    + "<div class='main-box'>"
                    + "<div class='main-box-header clearfix'>"
                        + "<div class='img'>"
                            + "<img src='" + avatar + "' width='30'/>"
                        + "</div>"
                         + "<div class='comment-author'>" + userName + "</div>"
                        + "<div class='comment-content'>" + cmtText + "</div>"
                        + "<div class='icon-box'><button class='btn btn-sm btn-delcmt'><i class='fa fa-trash'></i></button></div>"
                    + "</div>"
                    + "<div class='main-box-body'>"
                    + "<ul class='list-unstyled reply-list'></ul><div class='form-group'>"
                        + "<div class='input-group'>"
                            + "<input type='text' class='form-control input-reply'>"
                            //+ "<span class='input-group-btn'>"
                            //    + "<button class='btn btn-primary btn-replycmt' type='button'><i class='fa fa-comment'></i></button>"
                            //+ "</span>"
                        + "</div>"
                    + "</div>"
                + "</div></div>"
                + "</li>");
        }
        $("#btn-cancelreview").on("click", function () {
            $("#txt-articlecomment").val('');
            $("#comment-box").addClass('closed');
        });

        $("#comment-display-ctrl").on("change", function () {
            var value = $(this).val();
            if (value == 'show') {
                show();
            }
            else if (value == 'hide') {
                hide();
                
            }
        });

        // The start point for your plugin.
        function _init() {
            $btnComment = $("<button id='btn-comment' class='fr-comment-btn' type='buton'></button");
            $btnComment.html("<i class='fa fa-comment'></i>");
            $btnComment.addClass("hidden");
            editor.$box.append($btnComment);
            $btnComment.on("click", function () {
                $('#comment-box').removeClass('closed');
                $btnComment.addClass("hidden");
                return false;
            });

            editor.events.on("keyup", checkSelection);
            editor.events.on("mouseup", checkSelection);
            //editor.o_doc.addEventListener("selectionchange", checkSelection, false)
            // You can access any option from documentation or your custom options.
           
        }
        var $btnComment;
        // Expose public methods. If _init is not public then the plugin won't be initialized.
        return {
            _init: _init,
            show: show,
            hide: hide
        }
    }
})(jQuery);