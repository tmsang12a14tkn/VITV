//this function can remove a array element.
Array.remove = function (array, from, to) {
    var rest = array.slice((to || from) + 1 || array.length);
    array.length = from < 0 ? array.length + from : from;
    return array.push.apply(array, rest);
};

//this variable represents the total number of popups can be displayed according to the viewport width
var total_popups = 0;

//arrays of popups ids
var popups = [];
function openChatWindow(userId, displayName) {
    
    for (var iii = 0; iii < popups.length; iii++) {
        //already registered. Bring it to front.
        if (userId == popups[iii]) {
            Array.remove(popups, iii);

            popups.unshift(userId);

            calculate_popups();


            return true;
        }
    }
    
    if ($('.chat_window[data-userid=' + userId + ']').length == 0) {
        $.get('/Message/GetChatWindow?userId=' + userId, function (data) {
            //document.getElementsById("containerchat")[0].innerHTML = document.getElementsByTagName("containerchat")[0].innerHTML +data;
            $('#containerchat').append(data);
            popups.unshift(userId);
            calculate_popups();
            var $chatWindow = $('.chat_window[data-userid=' + userId + ']');
            $chatWindow.find('.dialogs').animate({ scrollTop: $chatWindow.find('.dialogs')[0].scrollHeight }, 500);
        });
        return false;
    }
    return true;
}



//this is used to close a popup
function close_popup(id) {
    for (var iii = 0; iii < popups.length; iii++) {
        if (id == popups[iii]) {
            Array.remove(popups, iii);

            document.getElementById(id).style.display = "none";

            calculate_popups();

            return;
        }
    }
}

//calculate the total number of popups suitable and then populate the toatal_popups variable.
function calculate_popups() {
    var width = window.innerWidth;
    if (width < 640) {
        total_popups = 0;
    }
    else {
        width = width - 300;
        //320 is width of a single popup box
        total_popups = parseInt(width / 420);
    }

    display_popups();

}

//displays the popups. Displays based on the maximum number of popups that can be displayed on the current viewport width
function display_popups() {
    var right = 320;

    var iii = 0;
    for (iii; iii < total_popups; iii++) {
        if (popups[iii] != undefined) {
            var element = document.getElementById(popups[iii]);
            element.style.right = right + "px";
            right = right + 420;
            element.style.display = "block";
        }
    }

    for (var jjj = iii; jjj < popups.length; jjj++) {
        var element = document.getElementById(popups[jjj]);
        element.style.display = "none";
    }
}

function filterChatList(name) {
    if (name === null || name.match(/^ *$/) !== null) {
        $('.chat-user-item').removeClass("hidden");
        }
        else {
        $('.chat-user-item').each(function (index) {
            if ($(this).data('displayname').toString().toLowerCase().indexOf(name.toLowerCase()) < 0)
                $(this).addClass("hidden");
            else
                $(this).removeClass("hidden");
                });
    }
}

$(document).ready(function () {
    var IsDataAvailable = true;
    //khi click vao nut thong bao tin nhan de xem
    $('#clicknotichat').on('click', function () {
        if (IsDataAvailable == true) {
            var ul = $('#listnotichat');
            ul.empty();
            ul.prepend('<i id="loader_noti_chat" class="ace-icon fa fa-spinner fa-spin red bigger-150" style="margin-left: 145px"></i>');
            IsDataAvailable = false;

            $.post('/Message/GetListNotiMessage', function (result) {
                if (result.success == -1) {
                    window.location = result.content;
                }
                else if (result.success == 1) {
                    if (result.count > 0)
                        $('#countnotichatsecond').replaceWith(result.count + " Tin nhắn");
                    else
                        $('#countnotichatsecond').replaceWith("Tin nhắn");
                    $('#countnotichat').empty();
                    IsDataAvailable = true;
                    ul.empty();
                    ul.prepend(result.content);
                }
            });
        }
    });

    var chat = $.connection.chatHub;
    getListUser();
    chat.client.notifyUserOnline = function (client) {
        $(".chat-user-item[data-userid=" + client.UserId + "]").find('i').removeClass('offline').addClass('online');
    }
    chat.client.notifyUserOffline = function (client) {
        console.log(client.UserId + "-offline");
        $(".chat-user-item[data-userid=" + client.UserId + "]").find('i').removeClass('online').addClass('offline');
    }

    chat.client.receiveMessage = function (otherId, otherDisplayName, message, avatar, time) {
        GetCountReceiveMessage();
        var existWindow = openChatWindow(otherId, otherDisplayName);
        if (existWindow == true) {
            appendOtherMessage(otherId, otherDisplayName, message, avatar, time);

            var $chatWindow = $('.chat_window[data-userid=' + otherId + ']');
            $chatWindow.find('.dialogs').animate({ scrollTop: $chatWindow.find('.dialogs')[0].scrollHeight }, 500);
            $chatWindow.data('viewed', false);
        }
    }
    chat.client.receiveGroupMessage = function (userId, userDisplayName, message) {
        appendGroupMessage(userId, userDisplayName, message);
        //scroll 
        var $chatWindow = $('#chat-group');
            //$chatWindow.find('.modal-body .scroller').animate({ scrollTop: $chatWindow.find('.modal-body .scroller')[0].scrollHeight }, 500);
    }
    chat.client.sendMessageComplete = function (otherId, myDisplayName, message, avatar, time) {
        console.log(time);
        appendMyMessage(otherId, myDisplayName, message, avatar, time);

        var $chatWindow = $('.chat_window[data-userid=' + otherId + ']');
        $chatWindow.find('.dialogs').animate({ scrollTop: $chatWindow.find('.dialogs')[0].scrollHeight }, 500);
    }
    chat.client.receiveOnlineUsers = function (clients) {
        console.log(clients);
        for (var i = 0; i < clients.length; i++) {
            $('.chat-user-item[data-userid=' + clients[i].UserId + '] i').removeClass('offline').addClass('online');
        }
    }

    if (window.hubReady != null) {
        window.hubReady.done(function () {
            chat.server.getOnlineUsers();
        //khi click tren thong bao tin nhan
        $('body').on('click', '.chat_user_noti_message', function () {
            var userId = $(this).data("userid");
            var displayName = $(this).data("displayname");
            console.log(displayName);
            SetAllMessageViewed(userId);
            openChatWindow(userId, displayName);
        });
        //khi click tren danh sach user
        $('.chat-user-item').on('click', function () {

            var userId = $(this).data("userid");
            var displayName = $(this).data("displayname");
            console.log(displayName);
            SetAllMessageViewed(userId);
            openChatWindow(userId, displayName);
        });
        $("#group-chat-input").keypress(function (e) {
            if (e.which == 13) {
                var message = $("#group-chat-input").val();
                $("#group-chat-input").val('');
                sendGroupMessage(message);
            }
        });

        $('body').on('click', '.btnSendMessage', function () {
            var messageInput = $(this).parent().parent().find('#ChatContent');
            if (messageInput != null && messageInput.val() != "") {
                var userid = $(this).parents('.chat_window').data('userid');
                sendMessage(userid, messageInput.val());
                messageInput.val('').focus();
            }
        });

        $('body').on('keypress', '#ChatContent', function (e) {
            if (e.keyCode == 13 && e.shiftKey) { }
            else if (e.keyCode == 10 || e.keyCode == 13) {
                e.preventDefault();
                var messageInput = $(this).val();
                if (messageInput != "") {
                    var userid = $(this).parents('.chat_window').data('userid');
                    sendMessage(userid, messageInput);
                    $(this).val('').focus();
                }
            }
        });

        $("body").on("click", "#ChatContent", function (e) {
            var userid = $(this).parents('.chat_window').data('userid');
            SetAllMessageViewed(userid);            
        });

        $('body').on('click', '#btnclose', function (data) {
            close_popup($(this).parents('.chat_window').data('userid'));
            $(this).parents('.chat_window').remove();
            return false;
        });
        
        $('body').on('click', '#chat_title', function () {
            var userid = $(this).parents('.chat_window').data('userid');
            console.log(userid);
            var chatwindow = $("#" + userid);
            EffectChatWindow(chatwindow);
        });
        $("#chat-filter").keyup(function () {
            filterChatList($("#chat-filter").val());
        });
    });
    }

    function GetCountReceiveMessage() {
        $.post('/Message/GetCountReceiveMessage', function (result) {
            console.log(result.count);
            if (result.count > 0)
            {
                $('#countnotichat').empty().append(result.count);
            } else {
                $('#countnotichat').empty();
            }
                
        });
    }

    function SetAllMessageViewed(receiver) {
        var chatwindow = $('.chat_window[data-userid=' + receiver + ']');
        if (chatwindow.data('viewed') == false)
        {
            console.log("ok");
            $.post('/Message/SetAllMessageViewed', { receiver: receiver }, function (result) {
                if (result.success == 1)
                {
                    chatwindow.data('viewed', true);
                    GetCountReceiveMessage()
                }
                    
            });
        }
        
    }

    var appendOtherMessage = function (userId, displayName, msg, avatar, time) {
        var messElement = '<div class="itemdiv dialogdiv">' +
                            '<div class="user">' +
                                '<img alt="' + displayName + '" src="' + avatar + '" />' +
                            '</div>' +
                            '<div class="body">' +
                                '<div class="time">' +
                                    '<i class="ace-icon fa fa-clock-o"></i>' +
                                    '<span class="green">' + time + '</span>' +
                                '</div>' +
                                '<div class="name">' +
                                    '<a href="#">' + displayName + '</a>' +
                                '</div>' +
                                '<div class="text">' + msg + '</div>' +
                                '<div class="tools">' +
                                    '<a href="#" class="btn btn-minier btn-info">' +
                                        '<i class="icon-only ace-icon fa fa-share"></i>' +
                                    '</a>' +
                                '</div>' +
                            '</div>' +
                        '</div>';
        //append message
        $('.chat_window[data-userid=' + userId + ']').find('.dialogs').append(messElement);
    }
    var appendMyMessage = function (userId, displayName, msg, avatar, time) {
        var messElement = '<div class="itemdiv dialogdiv">' +
                            '<div class="user">' +
                                '<img alt="' + displayName + '" src="' + avatar + '" />' +
                            '</div>' +
                            '<div class="body">' +
                                '<div class="time">' +
                                    '<i class="ace-icon fa fa-clock-o"></i>' +
                                    '<span class="green">' + time + '</span>' +
                                '</div>' +
                                '<div class="name">' +
                                    '<a href="#">' + displayName + '</a>' +
                                '</div>' +
                                '<div class="text">' + msg + '</div>' +
                                '<div class="tools">' +
                                    '<a href="#" class="btn btn-minier btn-info">' +
                                        '<i class="icon-only ace-icon fa fa-share"></i>' +
                                    '</a>' +
                                '</div>' +
                            '</div>' +
                        '</div>';
        //append message
        $('.chat_window[data-userid=' + userId + ']').find('.dialogs').append(messElement);

    }
    var appendGroupMessage = function (userId, displayName, msg) {
        var messElement = '<div class="itemdiv dialogdiv">'
                                + '<div class="user">'
                                    + '<img alt="' + displayName + '" src="/Content/Images/user.png">'
                                + '</div>'

                                + '<div class="body">'
                                    + '<div class="time">'
                                        + '<i class="ace-icon fa fa-clock-o"></i>'
                                        + '<span class="green">4 sec</span>'
                                    + '</div>'

                                    + '<div class="name">'
                                        + '<a href="#">' + displayName + '</a>'
                                    + '</div>'
                                    + '<div class="text">' + msg + '</div>'

                                + '</div>'
                            + '</div>'
        //append message
        $('#chat-group').find('.chat-messages').append(messElement);
    }
    var sendMessage = function (userId, msg) {
        chat.server.sendMessage(userId, msg);
    }
    var sendGroupMessage = function (msg) {
        chat.server.sendGroupMessage(msg);
    }
    function EffectChatWindow(chatwindow) {
        
        console.log(chatwindow.css("bottom"));
        if (chatwindow.css("bottom") >= '0px') {
            chatwindow.animate({
                bottom: "-=358px",
            }, 300);
        }
        else {
            chatwindow.animate({
                bottom: "+=358px",
            }, 300, function () {
            });
        }
    }
    function getListUser() {
        $.get('/Message/GetListUser', function (data) {
            $('.page-content').append(data.data);
        });
    }
    //recalculate when window is loaded and also when window is resized.
    window.addEventListener("resize", calculate_popups);
    window.addEventListener("load", calculate_popups);   
});