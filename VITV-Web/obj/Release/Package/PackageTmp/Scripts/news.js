var $container = $('#container');
var skip = 0;
var currentFilter = '*';
var hasMore = true;
	
$(window).load( function() {
	$container.isotope({
		itemSelector: '.grid',
		layoutMode: 'masonry'
	});
});

$( window ).resize(function() {
	$container.isotope('layout');
});

//$('#loadmore').on('click', function(e){
//    e.preventDefault();
//    skip += 5;
//    $.get('/Article/MoreArticle?skip=' + skip, function (data) {
//        if (data.cntArticle > 0) {
//            setTimeout(function () {
//                $container.isotope('insert', $(data.rendered));
//                if (currentFilter != '*' && data.rendered.indexOf(currentFilter) < 0) {
//                    $('#loadmore').hide();
//                }
//            }, 200);
//        } else {
//            $('#loadmore').hide();
//            hasMore = false;
//        }
//    });
//});

$('#loadmore').on('click', function(e){
    e.preventDefault();
    skip += 5;
    $.get('/Article/MoreArticle?skip=' + skip, function (data) {
        if (data.cntArticle > 0) {
            var moreGrid = '';            
            $.each(data.result, function (id, article) {
                console.log(article);
                moreGrid += "<div class='grid " + article.CategoryUniqueTitle + "'>"
						            + "<div class='imgholder'>"
							            + "<a href='/tin-tuc/" + article.UniqueTitle + "/" + article.Id + "'><img src='" + article.Thumbnail + "' /><span class='hover'></span></a>"
						            + "</div>"
						            + "<h5><a href='#'>Diễn đàn CEO</a>, " + new Date(parseInt(article.PublishedTime.substr(6))).toString("dd/MM/yyyy") + "</h5>"
						            + "<strong><a href='/tin-tuc/" + article.UniqueTitle + "/" + article.Id + "'>" + article.Title + "</a></strong>"
						            + "<p>" + article.ShortBrief + "</p>"
						            + "<p class='read_btn'><a href='/tin-tuc/" + article.UniqueTitle + "/" + article.Id + "'>Xem thêm...</a></p>"
                                + "</div>";
            });

            $container.isotope('insert', $(moreGrid));
        } else {
            $('#loadmore').hide();
            hasMore = false;
        }
    });
});

$('.news_filter_large ul li').on('click', function() {
    $('#loadmore').hide();

    var filterValue = $(this).attr('data-filter');
    currentFilter = filterValue;
	$( this ).siblings().removeClass('active');
	$( this ).addClass('active');
	$container.isotope({ filter: filterValue });

	setTimeout(function () {
	    if ($('.grid:visible').length > 0 && hasMore) $('#loadmore').show();
    }, 500);
	
});

$('.news_filter_small').on('change', function () {
    $('#loadmore').hide();

    var filterValue = $(this).val();
    $container.isotope({ filter: filterValue });

    setTimeout(function () {
        if ($('.grid:visible').length > 0 && hasMore) $('#loadmore').show();
    }, 500);
})