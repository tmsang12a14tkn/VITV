function analyticPage(thumbnail, title, vId, vCatId) {
    var loc = window.location;
    var url = loc.pathname + loc.search + loc.hash;

    var parser = new UAParser();
    var result = parser.getResult();
    
    var os = result.os.name;
    var deviceType = result.device.type;
    var deviceVendor = result.device.vendor;
    var deviceModel = result.device.model;    
    
    var params = "/home/analyticpages?url=" + url + "&thumbnail=" + thumbnail + "&title=" + title;
    if (vId != null) params = params + "&vId=" + vId;
    if (vCatId != null) params = params + "&vcatId=" + vCatId;
    params = params + "&os=" + os + "&dvmodel=" + deviceModel + "&dvtype=" + deviceType + "&dvvendor=" + deviceVendor
    
    $.post(params, function () { });
}