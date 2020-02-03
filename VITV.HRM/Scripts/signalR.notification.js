var notificationHub = $.connection.notificationHub;

notificationHub.client.reloadNotification = function ()
{

	$.ajax({
		url: '/Notification/Get',
		success: function (result) {
			var notifiCountDiv = $('#notifiCentered');
			notifiCountDiv.html(result);
		}
	});


};
notificationHub.client.popupNotification = function(employeeId, notificationId, content)
{
    //$("#notification-modal .modal-content").load('/Notification/Popup?employeeId=' + employeeId + '&notifiId=' + notificationId, function ()
    //{
        //$(this).find('.blockquote-content').html(content);
    //});
	//$("#notification-modal").modal('show');
    $.get('/Notification/Popup?employeeId=' + employeeId + '&notifiId=' + notificationId, function (data)
    {
        $.gritter.add({
            title: data.Content,
            text: content + '<div><a href="'+data.Url+'">Xem thêm</a></div>',
            image: content.ProfilePicture, //in Ace demo dist will be replaced by correct assets path
            sticky: false,
            time: '',
            class_name: 'gritter-light'

        });
    })
	
	document.getElementById('sound').play();
	setTimeout(function () {
		document.getElementById('sound').play();
	}, 1000);
	
}