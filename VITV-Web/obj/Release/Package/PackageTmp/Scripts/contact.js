$(".navbar-toggle").on("click", function () {
    $(this).toggleClass("active");
});

$(".nav-tabs a").on("click", function () {
    $("#contact_filter").collapse("hide");
    $(".navbar-toggle").removeClass("active");
});

var dragFlag = false;
var start = 0, end = 0;

function thisTouchStart(e) {
    dragFlag = true;
    start = e.touches[0].pageY;
}

function thisTouchEnd() {
    dragFlag = false;
}

function thisTouchMove(e) {
    if (!dragFlag) return;
    end = e.touches[0].pageY;
    window.scrollBy(0, (start - end));
}

function FullMapControl(controlDiv, map, link) {
    // Set CSS for the control border
    var controlUI = document.createElement('div');
    controlUI.style.backgroundColor = '#d9534f';
    controlUI.style.border = '0';
    controlUI.style.borderRadius = '3px';
    controlUI.style.boxShadow = '0 2px 6px rgba(0,0,0,.3)';
    controlUI.style.cursor = 'pointer';
    controlUI.style.marginBottom = '22px';
    controlUI.style.textAlign = 'center';
    controlUI.title = 'Bấm để xem bản đồ lớn';
    controlDiv.appendChild(controlUI);

    // Set CSS for the control interior
    var controlText = document.createElement('div');
    controlText.style.color = '#fff';
    controlText.style.fontSize = '16px';
    controlText.style.lineHeight = '38px';
    controlText.style.paddingLeft = '5px';
    controlText.style.paddingRight = '5px';
    controlText.innerHTML = 'Xem bản đồ lớn';
    controlUI.appendChild(controlText);

    // Setup the click event listeners: simply set the map to
    // Chicago
    google.maps.event.addDomListener(controlUI, 'click', function () {
        window.open(link);
    });
}

$(function () {
    var hnLatlng = new google.maps.LatLng(21.029955, 105.811756);
    var hnOptions = {
        zoom: 18,
        center: hnLatlng
    }
    var hnmap = new google.maps.Map(document.getElementById("hn-canvas"), hnOptions);

    var hnLink = 'https://www.google.com/maps/place/Vit+Tower/@21.029955,105.811756,17z/data=!3m1!4b1!4m2!3m1!1s0x3135ab6c0ad7e03b:0x23e17a1890736d39';
    var fullMapControlDiv = document.createElement('div');
    var fullMapControl = new FullMapControl(fullMapControlDiv, hnmap, hnLink);
    fullMapControlDiv.index = 1;
    hnmap.controls[google.maps.ControlPosition.BOTTOM_CENTER].push(fullMapControlDiv);

    var hnmarker = new google.maps.Marker({
        position: hnLatlng
    });

    hnmarker.setMap(hnmap);
    hnmap.setOptions({ 'scrollwheel': false, draggable: false });

    document.getElementById("hn-canvas").addEventListener("touchstart", thisTouchStart, true);
    document.getElementById("hn-canvas").addEventListener("touchend", thisTouchEnd, true);
    document.getElementById("hn-canvas").addEventListener("touchmove", thisTouchMove, true);

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        var hcmLatlng = new google.maps.LatLng(10.768263, 106.700718);
        var hcmOptions = {
            zoom: 18,
            center: hcmLatlng
        }
        var hcmmap = new google.maps.Map(document.getElementById("hcm-canvas"), hcmOptions);

        var hcmLink = 'https://www.google.com/maps/place/6+Ph%C3%B3+%C4%90%E1%BB%A9c+Ch%C3%ADnh,+Nguy%E1%BB%85n+Th%C3%A1i+B%C3%ACnh,+Qu%E1%BA%ADn+1,+H%E1%BB%93+Ch%C3%AD+Minh,+Vietnam/@10.7684294,106.7006342,17z/data=!3m1!4b1!4m2!3m1!1s0x31752f40120da82f:0xf544cd7675cd22';
        var fullMapControlDiv = document.createElement('div');
        var fullMapControl = new FullMapControl(fullMapControlDiv, hcmmap, hcmLink);
        fullMapControlDiv.index = 1;
        hcmmap.controls[google.maps.ControlPosition.BOTTOM_CENTER].push(fullMapControlDiv);

        var hcmmarker = new google.maps.Marker({
            position: hcmLatlng
        });

        hcmmarker.setMap(hcmmap);
        hcmmap.setOptions({ 'scrollwheel': false, draggable: false });

        document.getElementById("hcm-canvas").addEventListener("touchstart", thisTouchStart, true);
        document.getElementById("hcm-canvas").addEventListener("touchend", thisTouchEnd, true);
        document.getElementById("hcm-canvas").addEventListener("touchmove", thisTouchMove, true);
    });
});