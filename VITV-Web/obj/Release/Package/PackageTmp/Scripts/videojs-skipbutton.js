(function (vjs) {
    // plugin initializer
    var skipbtn = function (options) {
        player = this;

        var divLink = document.createElement("div");
        divLink.className = "vjs-skip-ads";

        var divContent = document.createElement("div");
        divContent.className = "vjs-skip-content";

        var text = document.createElement("div");
        text.textContent = "Tới nội dung chính";

        divContent.appendChild(text);
        divLink.appendChild(divContent);
        player.el().appendChild(divLink);

        return this;
    };
    // register the plugin with video.js
    vjs.plugin('skipbtn', skipbtn);

}(window.videojs));