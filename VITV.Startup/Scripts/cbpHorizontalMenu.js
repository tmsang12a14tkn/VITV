var cbpHorizontalMenu = (function() {

    var $listItems = $('.cbp-hrmenu .navbar-nav > li'),
		$menuItems = $listItems.children( 'a' ),
		$body = $( 'body' );
	current = -1;
    
	function init() {
		$menuItems.on( 'click', open );
		$listItems.on('click', function (event) { event.stopPropagation(); });
	}

	function open( event ) {
	    
		if( current !== -1 ) {
			$listItems.eq( current ).removeClass( 'cbp-hropen' );
			$listItems.eq( current ).find('.cbp-hrsub').stop(!0,!0).delay(50).slideUp(200);
		}

		var $item = $( event.currentTarget ).parent( 'li' ),
			$item_sub = $( event.currentTarget ).next('.cbp-hrsub'),
			idx = $item.index();
		
		if( current === idx ) {
			$item.removeClass( 'cbp-hropen' );
			$item_sub.stop(!0,!0).delay(50).slideUp(200);
			current = -1;
		}
		else {
			$item.addClass( 'cbp-hropen' );
			$item_sub.stop(!0,!0).delay(50).slideDown(250);
			current = idx;
			$body.off( 'click' ).on( 'click', close );
		}

		if (!$item.hasClass('nonsubmenu'))
		    return false;

	}

	function close( event ) {
		$listItems.eq( current ).removeClass( 'cbp-hropen' );
		$listItems.eq( current ).find('.cbp-hrsub').stop(!0,!0).delay(50).slideUp(200);
		current = -1;
	}

	return { init : init };

})();