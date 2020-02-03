/*! videojs-preroll - v0.2.0 - 2014-12-08
* Copyright (c) 2014 Sano Webdevelopment;
* Copyright (c) 2014 The Onion
* Licensed MIT */
(function (window, videojs) {
    'use strict';

    var defaults = {
        srcs: [], //Advertisement source, can also be an object like {src:"file.mp4",type:"video/mp4"}
        href: '', //Advertised url
        target: '_blank', //Target to open the ad url in
        allowSkip: true, //Allow skipping of the ad after a certain period
        repeatAd: false, //Show the ad only once or after every conten
        adsOptions: {}, //Options passed to the ads plugin
        lang: {
            'skip': 'Bỏ qua',
            'skip in': 'Còn '
        } //Language entries for translation
    }, prerollPlugin;

    /**
     * Initialize the plugin.
     *
     * @param options
     *            (optional) {object} configuration for the plugin
     */
    prerollPlugin = function (options) {
        var settings = videojs.mergeOptions(defaults, options), player = this;
        player.ads(settings.adsOptions);
        player.preroll = { adDone: false };
        player.on('contentupdate', function () {
            if (!player.preroll.shouldPlayPreroll()) {
                player.trigger('adscanceled');
            } else {
                player.trigger('adsready');
            }
        });
        function changeSrc()
        {
            if (player.preroll.skipButton != null && (' ' + player.preroll.skipButton.className + ' ').indexOf(' enabled ') >= 0)
                player.preroll.skipButton.className = player.preroll.skipButton.className.replace(' enabled', '');
            if (settings.data.length > 0) {
                player.preroll.currentSource = settings.data[0];
                settings.data.shift();
                player.src(player.preroll.currentSource.source);

                //if (settings.href !== '') {
                //    // link overlay
                //    var blocker = document.createElement('a');
                //    blocker.className = 'preroll-blocker';
                //    blocker.href = settings.href;
                //    blocker.target = settings.target || '_blank';
                //    blocker.onclick = function () {
                //        player.trigger('adclick');
                //    };
                //    player.preroll.blocker = blocker;
                //    player.el().insertBefore(blocker, player.controlBar.el());
                //}

                if (player.preroll.currentSource.allowSkip !== false) {
                    var divLink = document.createElement("div");
                    divLink.className = "vjs-preroll-contain";

                    var skipButton = document.createElement('div');
                    skipButton.className = 'preroll-skip-button';

                    divLink.appendChild(skipButton);
                    player.preroll.skipButton = skipButton;
                    player.el().appendChild(divLink);

                    skipButton.onclick = function (e) {
                        var Event = Event || window.Event;
                        if ((' ' + player.preroll.skipButton.className + ' ').indexOf(' enabled ') >= 0) {
                            //player.preroll.exitPreroll();
                            changeSrc();
                        }
                        if (Event.prototype.stopPropagation !== undefined) {
                            e.stopPropagation();
                        } else {
                            return false;
                        }
                    };
                    player.on('adtimeupdate', player.preroll.timeupdate);
                }
                else
                {
                    if (typeof player.preroll.skipButton !== 'undefined') {
                        player.preroll.skipButton.parentNode.removeChild(player.preroll.skipButton);
                        player.preroll.skipButton = undefined;
                    }
                    if (typeof player.preroll.blocker !== 'undefined') {
                        player.preroll.blocker.parentNode.removeChild(player.preroll.blocker);
                        player.preroll.blocker = undefined;
                    }
                    player.off('adtimeupdate', player.preroll.timeupdate);
                }
            }
            else
            {
                player.preroll.exitPreroll();
            }
        }
        player.on('readyforpreroll', function () {
            // No video? No ad.
            if (!player.preroll.shouldPlayPreroll()) {
                player.trigger('adscanceled');
                return;
            }

            // Initialize ad mode
            player.ads.startLinearAdMode();

            // Change player src to ad src
            changeSrc();
            
            player.one('durationchange', function () {
                player.play();
            });

            //Fallback in case preload = none
            player.one('progress', function () {
                player.play();
            });
            player.one('adloadstart', function () {
                player.play();
            });

           
            player.on('adended', changeSrc);
            player.one('error', player.preroll.prerollError);
        });
        player.preroll.shouldPlayPreroll = function () {
            if (settings.src === '') {
                return false;
            }
            if (player.preroll.adDone === true) {
                return false;
            }
            return true;
        };
        player.preroll.exitPreroll = function () {
            if (typeof player.preroll.skipButton !== 'undefined') {
                player.preroll.skipButton.parentNode.removeChild(player.preroll.skipButton);
                player.preroll.skipButton = undefined;
            }
            if (typeof player.preroll.blocker !== 'undefined') {
                player.preroll.blocker.parentNode.removeChild(player.preroll.blocker);
                player.preroll.blocker = undefined;
            }
            player.off('adtimeupdate', player.preroll.timeupdate);
            player.off('adended', player.preroll.exitPreroll);
            player.off('error', player.preroll.prerollError);
            if (settings.repeatAd !== true) {
                player.preroll.adDone = true;
            }
            player.ads.endLinearAdMode();
        };
        player.preroll.timeupdate = function (e) {
            player.loadingSpinner.el().style.display = 'none';
            var timeLeft = Math.ceil(player.preroll.currentSource.skipTime - player.currentTime());
            if (timeLeft > 0) {
                player.preroll.skipButton.innerHTML = settings.lang['skip in'] + timeLeft + 's...';
            } else {
                if ((' ' + player.preroll.skipButton.className + ' ').indexOf(' enabled ') === -1) {
                    player.preroll.skipButton.className += ' enabled';
                    player.preroll.skipButton.innerHTML = settings.lang.skip;
                }
            }
        };
        player.preroll.prerollError = function (e) {
            player.preroll.exitPreroll();
        };
        if (player.currentSrc()) {
            if (player.preroll.shouldPlayPreroll()) {
                player.trigger('adsready');
            } else {
                player.trigger('adscanceled');
            }
        }
    };

    // register the plugin
    videojs.plugin('preroll', prerollPlugin);
})(window, window.videojs);
