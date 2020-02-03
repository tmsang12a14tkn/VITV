jQuery.fn.extend({
	chatwindow: function (options)
	{
		var hide = $(this).data('hide');
		var userId = $(this).data('userid');
		function toggleShow() {
			if (!hide) {
				$('.chat-window[data-userid=' + userId + ']').find(".panel-body").show();
				$('.chat-window[data-userid=' + userId + ']').find(".panel-footer").show();
			}
			else {
				$('.chat-window[data-userid=' + userId + ']').find(".panel-body").hide();
				$('.chat-window[data-userid=' + userId + ']').find(".panel-footer").hide();
			}
		}
		function sendMessage() {
			var $input = $('.chat-window[data-userid=' + userId + ']').find('.input-chat');
			var message = $input.val();
			if (message === null || message.match(/^ *$/) !== null) {
			    return;
			}
			$input.val('');
			if (options != undefined && options.onChat != undefined)
				options.onChat(userId, message);
		};

		$('#chat-windows').on('click', '.chat-window[data-userid=' + userId + '] .btn-hide', function () {
			hide = !hide;
			toggleShow();
		});

		$('#chat-windows').on('click', '.chat-window[data-userid=' + userId + '] .btn-chat', function () {
			sendMessage();
		});

		$("#chat-windows").on('click', '.chat-window[data-userid='+userId+'] .btn-close', function () {
			$('.chat-window[data-userid='+userId+']').remove();
		});

		$('.chat-window[data-userid=' + userId + '] .input-chat').keypress(function (e) {
			if (e.which == 13) {
				sendMessage();
			}
		});
	}
})