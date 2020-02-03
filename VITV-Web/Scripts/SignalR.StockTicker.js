
// Crockford's supplant method (poor man's templating)
if (!String.prototype.supplant) {
    String.prototype.supplant = function (o) {
        return this.replace(/{([^{}]*)}/g,
            function (a, b) {
                var r = o[b];
                return typeof r === 'string' || typeof r === 'number' ? r : a;
            }
        );
    };
}

// A simple background color flash effect that uses jQuery Color plugin
jQuery.fn.flash = function (color, duration) {
    var current = this.css('backgroundColor');
    this.animate({ backgroundColor: 'rgb(' + color + ')' }, duration / 2)
        .animate({ backgroundColor: current }, duration / 2);
};

function startHub(stockHub) {

    var up = '▲',
        down = '▼',
        $stockTable = $('#stockTable'),
        $stockTableBody = $stockTable.find('tbody'),
        rowTemplate = '<tr data-symbol="{Symbol}"><td class="text-uppercase text-left {CloseChange}"><a href="/chung-khoan/bieu-do/{Symbol}"><strong>{Symbol}</strong></a></td><td class="ceiling">{Ceiling}</td><td class="floor">{Floor}</td><td class="open">{Open}</td><td class={Gm3Change}>{Gm3}</td><td class={Gm3Change}>{Klm3}</td><td class={Gm2Change}>{Gm2}</td><td class={Gm2Change}>{Klm2}</td><td class={Gm1Change}>{Gm1}</td><td class={Gm1Change}>{Klm1}</td><td class={CloseChange}>{Close}</td><td class={CloseChange}>{CurrentVolumn}</td><td class={CloseChange}>{PriceChange}</td><td  class={Gb1Change}>{Gb1}</td><td  class={Gb1Change}>{Klb1}</td><td  class={Gb2Change}>{Gb2}</td><td  class={Gb2Change}>{Klb2}</td><td  class={Gb3Change}>{Gb3}</td><td class={Gb3Change}>{Klb3}</td><td>{Volumn}</td><td class={PriceOpenChange}>{PriceOpen}</td><td class={PriceHighestChange}>{PriceHighest}</td><td class={PriceLowestChange}>{PriceLowest}</td><td>{ForeignerBuyVolume}</td></tr>',
        $stockTicker = $('#stockTicker'),
        $stockTickerTable = $stockTicker.find('tbody'),
        marketTemplate = '<tr data-symbol="{Symbol}"><td>{Symbol}</td> <td>{Close}</td> <td class="change">{PriceChange}</td> <td>{PercentChange}</td> <td>{Volumn}</td> </tr>';

    function formatStock(stock) {
        if(stock.Open != '')
            stock.Open = parseFloat(stock.Open);
        if (stock.Close != '')
            stock.Close = parseFloat(stock.Close);

        if (stock.Gm3 != '' && stock.Gm3 != 'ATC')
            stock.Gm3 = parseFloat(stock.Gm3);
        else
            stock.Klm3 = '';
        if (stock.Gm2 != '' && stock.Gm2 != 'ATC')
            stock.Gm2 = parseFloat(stock.Gm2);
        else
            stock.Klm2 = '';
        if (stock.Gm1 != '' && stock.Gm1 != 'ATC')
            stock.Gm1 = parseFloat(stock.Gm1);
        else
            stock.Klm1 = '';

        if (stock.Gb3 != '' && stock.Gb3 != 'ATC')
            stock.Gb3 = parseFloat(stock.Gb3);
        else
            stock.Klb3 = '';
        if (stock.Gb2 != '' && stock.Gb2 != 'ATC')
            stock.Gb2 = parseFloat(stock.Gb2);
        else
            stock.Klb2 = '';
        if (stock.Gb1 != '' && stock.Gb1 != 'ATC')
            stock.Gb1 = parseFloat(stock.Gb1);
        else
            stock.Klb1 = '';
        if (stock.PriceOpen != '')
            stock.PriceOpen = parseFloat(stock.PriceOpen);
        if (stock.PriceHighest != '')
            stock.PriceHighest = parseFloat(stock.PriceHighest);
        if (stock.PriceLowest != '')
            stock.PriceLowest = parseFloat(stock.PriceLowest);
        if (stock.Close == 0) {
            stock.Close = '';
            stock.CurrentVolumn = '';
        }
        return $.extend(stock, {
            Gm3Change: stock.Gm3 > stock.Open ? "priceup" : (stock.Gm3 == stock.Open ? "pricestay" : "pricedown"),
            Gm2Change: stock.Gm2 > stock.Open ? "priceup" : (stock.Gm2 == stock.Open ? "pricestay" : "pricedown"),
            Gm1Change: stock.Gm1 > stock.Open ? "priceup" : (stock.Gm1 == stock.Open ? "pricestay" : "pricedown"),
            CloseChange: stock.Close > stock.Open ? "priceup" : (stock.Close == stock.Open || stock.Close == 0 ? "pricestay" : "pricedown"),
            Gb3Change: stock.Gb3 > stock.Open ? "priceup" : (stock.Gb3 == stock.Open ? "pricestay" : "pricedown"),
            Gb2Change: stock.Gb2 > stock.Open ? "priceup" : (stock.Gb2 == stock.Open ? "pricestay" : "pricedown"),
            Gb1Change: stock.Gb1 > stock.Open ? "priceup" : (stock.Gb1 == stock.Open ? "pricestay" : "pricedown"),
            PriceOpenChange: stock.PriceOpen > stock.Open ? "priceup" : (stock.PriceOpen == stock.Open ? "pricestay" : "pricedown"),
            PriceHighestChange: stock.PriceHighest > stock.Open ? "priceup" : (stock.PriceHighest == stock.Open ? "pricestay" : "pricedown"),
            PriceLowestChange: stock.PriceLowest > stock.Open ? "priceup" : (stock.PriceLowest == stock.Open ? "pricestay" : "pricedown"),
        });
    }

    function init() {
        return stockHub.server.getAllStocks().done(function (stocks) {
            $stockTableBody.empty();
            $stockTickerTable.empty();
            $.each(stocks, function () {
                //var stock = this;
                var stock = formatStock(this);
                if (stock.Symbol.indexOf('^')>-1) {
                    $stockTickerTable.append(marketTemplate.supplant(stock));
                }
                else {
                    $stockTableBody.append(rowTemplate.supplant(stock));
                }
            });
        });
    }

    // Add client-side hub methods that the server will call
    $.extend(stockHub.client, {
        updateStockPrice: function (stock) {
            bg = '154,240,117'; // green
            var displayStock = formatStock(stock);
             if (stock.Symbol.indexOf('^') > -1) {
                $li = $(marketTemplate.supplant(displayStock));
                 $stockTickerTable.find('li[data-symbol=\'' + displayStock.Symbol + '\']')
                    .replaceWith($li);
                $li.flash(bg, 1000);
            }
            else {
                 $row = $(rowTemplate.supplant(displayStock));
                 $stockTableBody.find('tr[data-symbol=' + displayStock.Symbol + ']')
                    .replaceWith($row);
                $row.flash(bg, 1000);
            }
        },

        marketOpened: function () {
            $("#marketState").html("MỞ");
        },

        marketClosed: function () {
            $("#marketState").html("ĐÓNG");
        },

    });

    // Start the connection
    $.connection.hub.start()
        .then(init)
        .then(function () {
            return stockHub.server.getMarketState();
        })
        .done(function (state) {
            if (state === 'Open') {
                stockHub.client.marketOpened();
            } else {
                stockHub.client.marketClosed();
            }
        });
}