//MarketPanel.prototype = {
//    createChart: function () {        
//    },
//    //Cap nhat du lieu lan dau cho chart
//    updateChartInit: function () {
//    }  
//};

var MarketPanel = function(mktPanel, s, url) {
    this.mktPanel = mktPanel;
    this.url = url;
    this.symbol = s;
    this.priceData = [];
    this.volumeData = [];
    this.hchart = null;
    this.startTime;
    this.endTime;
    this.breakingTime;
    this.refPrice;
    this.updateInterval = null;
    this.createChart();
    this.updateChartInit();    
}



MarketPanel.prototype.createChart = function () {
    this.hChart = new Highcharts.Chart({
        chart: {
            renderTo: this.mktPanel,
            margin: [5, 40, 20, 40],
            animation: false,
            reflow: false,
            backgroundColor: Conf.chartBackColor,
            defaultSeriesType: "line"
        },
        credits: {
            enabled: false
        },
        title: {
            text: null
        },
        legend: {
            enabled: false
        },
        plotOptions: {
            series: {
                marker: {
                    enabled: false
                },
                states: {
                    hover: {
                        enabled: false
                    }
                },
                shadow: false,
                animation: false
            },
            area: {
                fillOpacity: 0.5,
                lineWidth: 1
            }
        },
        xAxis: {
            title: {
                text: null
            },
            labels: {
                style: {
                    fontSize: "10px",
                    color: Conf.chartxAxisColor
                }
            },
            type: "datetime",
            dateTimeLabelFormat: "%H:%M",
            tickPixelInterval: 125
        },
        yAxis: [{
            title: {
                text: null
            },
            labels: {
                style: {
                    fontSize: "12px",
                    color: Conf.chartyAxisColor
                }
            },
            gridLineColor: Conf.chartGridLineColor,
            endOnTick: false,
            startOnTick: true
        }, {
            title: {
                text: null
            },
            labels: {
                style: {
                    fontSize: "9px",
                    color: "White"
                }
            },
            gridLineWidth: 0,
            endOnTick: false,
            startOnTick: false,
            opposite: true
        }],
        tooltip: {
            formatter: function () {
                var c = '<span style="font-size:10px;font-weight:bold;">' + Highcharts.dateFormat("%H:%M:%S", this.x) + "</span><br/>";
                $.each(this.points, function (e, d) {
                    c += '<span style="font-size:10px;">' + d.series.name + ':</span><span style="font-size:10px;font-weight:bold;">' + addCommas(d.y) + (d.series.name == "Vol" ? "k" : "") + "</span>"
                });
                return c
            },
            shared: true,
            shadow: false,
            crosshairs: {
                color: "#FF0000"
            }
        },
        series: [{
            name: this.symbol,
            color: Conf.chartIndexColor,
            data: [],
            zIndex: 10
        }, {
            name: 'Vol',
            type: "area",
            color: Conf.chartVolColor,
            data: [],
            yAxis: 1,
            zIndex: 5
        }, {
            tooltip: { valueDecimals: 2 }
        }]
    });

};

MarketPanel.prototype.updateChartInit = function () {
    var b = this;
    console.log("ok");
    $.getJSON(this.url, function (result) {
        var data = result.split("@");
        if (data.length > 0) {
            //Parse data header: StartTime|EndTime|RefPrice
            var strHeader = data[0].split('|');
            if (strHeader.length >= 3) {
                b.startTime = ConvertDateUTC(strHeader[0]);
                b.endTime = ConvertDateUTC(strHeader[1]);
                b.breakingTime = ConvertDateUTC(strHeader[2]);
                b.refPrice = parseFloat(strHeader[3]);
            }
            //Parse data body: DealTime|Price|Volumn
            var strData = data[1].split('#');
            for (var i = 0; i < strData.length - 1; i++) {
                var c = MarketIndexParse(strData[i]);
                b.priceData.push([c.DealTime, c.Price]);
                b.volumeData.push([c.DealTime, c.Volumn / 1000]);
            }
            //Nghi giua phien duong khoi luong bi ngat
            if (b.volumeData.length > 0 && b.volumeData[b.volumeData.length - 1][0] >= b.breakingTime) {
                b.volumeData.push([b.breakingTime, null]);
                b.volumeData.sort(SortChartData);
            }
        }
        //Create Ref Price Line
        b.hChart.yAxis[0].removePlotLine("ref");
        b.hChart.yAxis[0].addPlotLine({
            id: "ref",
            value: b.refPrice,
            color: "#339966",
            dashStyle: "Dash",
            width: 2,
            zIndex: 5,
            label: {
                text: addCommas(b.refPrice),
                align: "center",
                verticalAlign: "middle",
                style: {
                    color: "#339966",
                    fontSize: "20px"
                }
            }
        });
        //Update data init to chart
        var k = b.hChart.xAxis[0].getExtremes();
        if (k.min != b.startTime || k.max != b.endTime) {
            b.hChart.xAxis[0].setExtremes(b.startTime, b.endTime, false, false)
        }
        b.hChart.series[0].setData(b.priceData, false);
        b.hChart.series[1].setData(b.volumeData, false);
        b.hChart.redraw();
    });
};

MarketPanel.prototype.updateChart = function () {
    var b = this;
    $.getJSON(b.url, function (result) {
        var data = result.split("@");
        if (data.length > 0) {
            var strData = data[1].split('#');
            if (strData.length > b.priceData.length + 1)
            {
                for (var i = b.priceData.length + 1; i < strData.length; i++)
                {
                    var c = MarketIndexParse(strData[i]);
                    b.priceData.push([c.DealTime, c.Price]);
                    b.volumeData.push([c.DealTime, c.Volumn / 1000]);
                }
                b.volumeData.sort(SortChartData);

                //Nghi giua phien duong khoi luong bi ngat
                if (b.volumeData.length > 0 && b.volumeData[b.volumeData.length - 1][0] >= b.breakingTime) {
                    b.volumeData.push([b.breakingTime, null]);
                    b.volumeData.sort(SortChartData);
                }

                //Create Ref Price Line
                b.hChart.yAxis[0].removePlotLine("ref");
                b.hChart.yAxis[0].addPlotLine({
                    id: "ref",
                    value: b.refPrice,
                    color: "#339966",
                    dashStyle: "Dash",
                    width: 2,
                    zIndex: 5,
                    label: {
                        text: addCommas(b.refPrice),
                        align: "center",
                        verticalAlign: "middle",
                        style: {
                            color: "#339966",
                            fontSize: "20px"
                        }
                    }
                });
                //Update data to chart
                var k = b.hChart.xAxis[0].getExtremes();
                if (k.min != b.startTime || k.max != b.endTime) {
                    b.hChart.xAxis[0].setExtremes(b.startTime, b.endTime, false, false)
                }
                b.hChart.series[0].setData(b.priceData, false);
                b.hChart.series[1].setData(b.volumeData, false);
                b.hChart.redraw();
            }
                
            //if (c.Volumn <= 0) return;
            //if (c.DealTime < b.startTime || c.DealTime > b.endTime) {
            //    return;
            //}
            //else {
            //    var Exists = false;
            //    for (var i = 0; i < b.volumeData.length; i++) {
            //        if (b.volumeData[i][0] == c.DealTime) {
            //            Exists = true;
            //            break;
            //        }
            //    }
            //    if (Exists == true) return;
            //}
            //b.priceData.push([c.DealTime, c.Price]);
            //b.volumeData.push([c.DealTime, c.Volumn / 1000]);
            //b.volumeData.sort(SortChartData);

                
        }
    });
};
MarketPanel.prototype.setSize = function (w, h) {
    var b = this;
    b.hChart.setSize(w, h);
};



MarketIndexParse = function (str) {
    var r = {};
    c = str.split('|');
    if (c.length > 0) {
        if (c.length > 0) {
            r.DealTime = ConvertDateUTC(c[0]);
            r.Price = parseFloat(c[1]);
            r.Volumn = parseInt(c[2]);
        }
    }
    return r;
}
//.toUTCString()
ConvertUTCDate = function (str) {
    var time = new Date();
    time.setMilliseconds(parseInt(str));
    return time;
}

function ConvertDateUTC(TimeMili) {
    var v = new Date();
    v.setTime(TimeMili);
    var dateUTC = Date.UTC(v.getFullYear(), v.getMonth() + 1, v.getDate(), v.getHours(), v.getMinutes(), v.getSeconds());
    return dateUTC;
}

SortChartData = function (a, b) {
    if (a[0] > b[0]) return 1;
    else if (a[0] < b[0]) return -1;
    else return 0;
};

addCommas = function (nStr) {
    if (nStr != "") {
        if (isNaN(nStr) == false) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }
        else {
            return nStr;
        }
    }
    else {
        return "";
    }
}

removeCommas = function (nStr) {
    return nStr.replace(/,/g, "");
}