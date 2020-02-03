(function () {

    "use strict";

    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////
    //
    // H E L P E R    F U N C T I O N S
    //
    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////

    /**
     * Function to check if we clicked inside an element with a particular class
     * name.
     * 
     * @param {Object} e The event
     * @param {String} className The class name to check against
     * @return {Boolean}
     */

    init();
  

    function clickInsideElement(e, className) {
        var el = e.srcElement || e.target;

        if (el.classList.contains(className)) {
            return el;
        } else {
            while (el = el.parentNode) {
                if (el.classList && el.classList.contains(className)) {
                    return el;
                }
            }
        }

        return false;
    }

    /**
     * Get's exact position of event.
     * 
     * @param {Object} e The event passed in
     * @return {Object} Returns the x and y position
     */
    function getPosition(e) {
        var posx = 0;
        var posy = 0;

        if (!e) var e = window.event;

        if (e.pageX || e.pageY) {
            posx = e.pageX;
            posy = e.pageY;
        } else if (e.clientX || e.clientY) {
            posx = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
            posy = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;
        }

        return {
            x: posx,
            y: posy
        }
    }

    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////
    //
    // C O R E    F U N C T I O N S
    //
    //////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////

    /**
     * Variables.
     */
    var commentClassName = "article-comment";
    var contextMenuActive = "context-menu--active";

    var articeContentClassName = "articlecontent";
    var taskItemInContext;

    var clickCoords;
    var clickCoordsX;
    var clickCoordsY;

    var menu = document.querySelector("#context-menu");
    var menuState = 0;
    var menuWidth;
    var menuHeight;
    var menuPosition;
    var menuPositionX;
    var menuPositionY;

    var windowWidth;
    var windowHeight;

    var selectedImg;
    /**
     * Initialise our application's code.
     */
    function init() {
        contextListener();
        clickListener();
        keyupListener();
        resizeListener();
    }

    /**
     * Listens for contextmenu events.
     */
    function contextListener() {
        document.addEventListener("contextmenu", function (e) {
            console.log(e);
            //taskItemInContext = true; //clickInsideElement(e, articeContentClassName);
            e.preventDefault();
            
           
            if(e.target.tagName == "IMG")
            {
                selectedImg = e.target;
                if (window.getSelection)
                    window.getSelection().removeAllRanges();
                else
                    document.selection.empty();
                toggleMenuOn();
                positionMenu(e);
            }
            else if (selectedText() != "") {
                selectedImg = null;
                toggleMenuOn();
                positionMenu(e);
            }
            else
            {
                //taskItemInContext = null;
                toggleMenuOff();
            }
        });
    }

    /**
     * Listens for click events.
     */
    function clickListener() {
        document.addEventListener("click", function (e) {
            if (e.target.className != "context-menu__link")
                selectedImg = null;
            toggleMenuOff();
       });
       //var imgs = document.getElementsByTagName("img");
       ////console.log(imgs);
       //for (var i = 0; i < imgs.length;i++)
       //{
       //    var img = imgs[i];
       //    img.oncontextmenu = function (e) {
       //        toggleMenuOn();
       //    };
       //}
      
    }

    
    /**
     * Listens for keyup events.
     */
    function keyupListener() {
        window.onkeyup = function (e) {
            if (e.keyCode === 27) {
                toggleMenuOff();
                selectedImg = null;
            }
        }
    }

    /**
     * Window resize event listener
     */
    function resizeListener() {
        window.onresize = function (e) {
            toggleMenuOff();
            selectedImg = null;
        };
    }

    /**
     * Turns the custom context menu on.
     */
    function toggleMenuOn() {
        if (menuState !== 1) {
            menuState = 1;
            menu.classList.add(contextMenuActive);
        }
    }

    /**
     * Turns the custom context menu off.
     */
    function toggleMenuOff() {
        
        if (menuState !== 0) {
            menuState = 0;
            menu.classList.remove(contextMenuActive);
        }
    }

    /**
     * Positions the menu properly.
     * 
     * @param {Object} e The event
     */
    function positionMenu(e) {
        
        clickCoords = getPosition(e);
        console.log(e, clickCoords);
        clickCoordsX = clickCoords.x;
        clickCoordsY = clickCoords.y;

        menuWidth = menu.offsetWidth + 4;
        menuHeight = menu.offsetHeight + 4;

        windowWidth = window.innerWidth;
        windowHeight = window.innerHeight;

        if ((windowWidth - clickCoordsX) < menuWidth) {
            menu.style.left = windowWidth - menuWidth + "px";
        } else {
            menu.style.left = clickCoordsX + "px";
        }

        if ((windowHeight - clickCoordsY) < menuHeight) {
            menu.style.top = windowHeight - menuHeight + "px";
        } else {
            menu.style.top = clickCoordsY + "px";
        }
    }

    /**
     * Dummy action function that logs an action when a menu item link is clicked
     * 
     * @param {HTMLElement} link The link that was clicked
    */

    document.getElementById("cmenu-comment").addEventListener("click", function () {
        $(document.parent).find('#comment-box').removeClass('closed');
    });

    document.highlight = function(colour, cmtText)
    {
        var range, sel;
        if (window.getSelection) {
            // IE9 and non-IE
            return makeEditableAndHighlight(colour, cmtText);

        } else if (document.selection && document.selection.createRange) {
            // IE <= 8 case
            range = document.selection.createRange();
            range.execCommand("BackColor", false, colour);
        }
    }
    document.removeComment = function (id)
    {
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
    function makeEditableAndHighlight(colour, cmtText)
    {

        var range, sel = window.getSelection();
        var uuid;

        if (sel.rangeCount && sel.getRangeAt) {
            range = sel.getRangeAt(0);
        }
        //select text
        if (selectedText()!="")
        {
            if (!document.execCommand("BackColor", false, colour)) {
                document.designMode = "on";
                if (range) {
                    sel.removeAllRanges();
                    sel.addRange(range);
                }
                // Use HiliteColor since some browsers apply BackColor to the whole block
                if (!document.execCommand("HiliteColor", false, colour)) {
                    document.execCommand("BackColor", false, colour);
                }
                document.designMode = "off";
            }

            var span = sel.focusNode.parentNode;

            uuid = guid();
            $(span).addClass(uuid);
            $(span).attr("data-toggle", "popover");
            $(span).attr("title", "Bình luận");
            $(span).attr("data-content", cmtText);
            $(span).attr("data-trigger", "hover");
            $(span).popover();
        }
        //select image
        else if(selectedImg != null)
        {
            $(selectedImg).addClass("bordered-image");
            //var span = selectedImg.parentNode;

            uuid = guid();
            $(selectedImg).addClass(uuid);
            $(selectedImg).attr("data-toggle", "popover");
            $(selectedImg).attr("title", "Bình luận");
            $(selectedImg).attr("data-content", cmtText);
            $(selectedImg).attr("data-trigger", "hover");
            $(selectedImg).popover();
        }
        return uuid;
    }

    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }

    // then to call it, plus stitch in '4' in the third group
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
    /**
     * Run the app.
     */
    

})();