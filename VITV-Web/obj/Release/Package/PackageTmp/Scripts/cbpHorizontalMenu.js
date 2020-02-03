var cbpHorizontalMenu = (function() {

    var $listItems = $('.cbp-hrmenu .navbar-nav > li'),
		$menuItems = $listItems.children( 'a' ),
		$body = $( 'body' ),
		current = -1;
    
	function init() {
		$menuItems.on( 'click', open );
		$listItems.on('click', function (event) { event.stopPropagation(); });
	}

	function open(event) {

	    $('.special_annouce').css('z-index', 'auto');
	    
		if( current !== -1 ) {
		    $listItems.eq(current).removeClass('cbp-hropen');
		    $('.special_annouce').css('z-index', 1030);
		}

		var $item = $( event.currentTarget ).parent( 'li' ),
			idx = $item.index();
		
		if( current === idx ) {
			$item.removeClass( 'cbp-hropen' );
			current = -1;
			$('.special_annouce').css('z-index', 1030);
		}
		else {
			$item.addClass( 'cbp-hropen' );
			current = idx;
			$body.off( 'click' ).on( 'click', close );
		}

		if (!$item.hasClass('nonsubmenu'))
		    return false;

	}

	function close(event) {
	    $('.special_annouce').css('z-index', 1030);
		$listItems.eq( current ).removeClass( 'cbp-hropen' );
		current = -1;
	}

	return { init : init };

})();