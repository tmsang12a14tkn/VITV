$.get("/exchangerates/vndexchangeratedata?code=USD", function (data) {
    defaultChartConfig("view_chart", data, true, true, { forceY: false });
});


function defaultChartConfig(containerid, data, guideline, useDates, auxOptions) {
    if (auxOptions === undefined) auxOptions = {};
    if (guideline === undefined) guideline = true;
    nv.addGraph(function () {
        var chart;
        chart = nv.models.lineChart().useInteractiveGuideline(guideline);

        chart
            .x(function (d, i) {
                return d.x;
            });

        if (auxOptions.width)
            chart.width(auxOptions.width);

        if (auxOptions.height)
            chart.height(auxOptions.height);

        if (auxOptions.forceY)
            chart.forceY([0]);

        var formatter;
        if (useDates !== undefined) {
            formatter = function (d, i) {
                return d3.time.format('%x')(new Date(d*1000));
            }
        }
        else {
            formatter = d3.format(",.1f");
        }
        chart.margin({ right: 40 });
        chart.xAxis // chart sub-models (ie. xAxis, yAxis, etc) when accessed directly, return themselves, not the parent chart, so need to chain separately
            .tickFormat(
                formatter
              );

        chart.yAxis
            .axisLabel('Voltage (v)')
            .tickFormat(d3.format(',.2f'));


        d3.select('#' + containerid + ' svg')
            .datum(data)
          .transition().duration(500)
            .call(chart);

        nv.utils.windowResize(chart.update);

        return chart;
    });
}
