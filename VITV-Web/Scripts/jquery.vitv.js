(function ( $ ) {
 
	$.fn.rangeDropdown = function (options) {

		var inputRange = $(this).children("input[name='rangeType']");
		var inputBegin = $(this).children("input[name='begin']");
		var inputEnd = $(this).children("input[name='end']");

		var btn = $(this).children("button")[0];
		var btnText = $(btn).children("span.text-value")[0];

		var list = $(this).children("ul")[0];
		if (!options)
		    options = {};
        options.range = options.range || inputRange.val();
		
		if (options && options.range)
		{
		    var rangeOption = $(list).children("li[data-range='" + options.range + "']");
		    var rangeText = rangeOption.html();
		    if (options.range == 'Custom')
		    {
		        if (inputBegin.val() != '' && inputEnd.val()!='')
		            $(btnText).html("Từ " + inputBegin.val() + " tới " + inputEnd.val());
		        else if(inputBegin.val() != '')
		            $(btnText).html("Sau " + inputBegin.val());
		        else if(inputEnd.val()!='')
		            $(btnText).html("Trước " + inputEnd.val());
		    }
		    else
		    {
		        $(btnText).html(rangeText);
		        inputBegin.val('');
		        inputEnd.val('');
		    }
		        
		    //inputRange.val(range);
		}
        
		$(list).children("li.fixed-range").each(function () {
			$(this).click(function () {
				var range = $(this).data("range");
				var rangeText = $(this).html();

				inputRange.val(range);
				$(btnText).html(rangeText);
				inputBegin.val('');
				inputEnd.val('');
				if (options && options.onSelectComplete)
				    options.onSelectComplete(range);
			});
		});

		$(list).children("li.custom-range").each(function () {
			$(this).click(function ()
			{
			    var range = $(this).data("range");
				var modalId = $(this).data("target");
				var modal = $(modalId);
				var btnModalOK = modal.find("button.btn-ok");
				btnModalOK.click(function () {
					var inputBeginModal = modal.find("#rangeBegin");
					var inputEndModal = modal.find("#rangeEnd");

					var beginValue = inputBeginModal.val();
					var endValue = inputEndModal.val();
					inputBegin.val(beginValue);
					inputEnd.val(endValue);
					if (beginValue && endValue)
					{
					    inputBegin.val(beginValue);
					    inputEnd.val(endValue);
					    $(btnText).html("Từ " + beginValue + " tới " + endValue);
					    inputRange.val(range);
					    if (options && options.onSelectComplete)
					        options.onSelectComplete(range);
					}
					else if (beginValue)
					{
					    inputBegin.val(beginValue);
					    $(btnText).html("Sau " + beginValue);
					    inputRange.val(range);
					    if (options && options.onSelectComplete)
					        options.onSelectComplete(range);
					}
					else if (endValue)
					{
					    inputEnd.val(endValue);
					    $(btnText).html("Trước " + endValue);
					    inputRange.val(range);
					    if (options && options.onSelectComplete)
					        options.onSelectComplete(range);
					}
				});

				
			});
		});

   };
 
}( jQuery ));