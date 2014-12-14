$(function () {
    var GetDataURL = "/" + pub_WhichLang + "/Query_StatisticByDay/GetData?dYMD_Begin=";
   
    $("#btnQuery").click(function () {
        LoadData();
    });

    $("#btnQuery").click();

    function LoadData() {
        $.ajax({
            url: GetDataURL + encodeURI($("#txtDate_Begin").val()) + "&dYMD_End=" + encodeURI($("#txtDate_End").val()) + "&Station=" + encodeURI($("#hid_DeviceName").val()) + "&EmpNO=" + "",
            type: "POST",
            cache: false,
            async: false,
            success: function (msg) {
                var data = eval("(" + msg + ")");
                $('#container').highcharts({
                    chart: {
                        type: 'column'
                    },
                    title: {
                        text: "[" + $("#txtDate_Begin").val() + "__" + $("#txtDate_End").val() + ']' + "单位天货物统计数据"
                    },
                    subtitle: {
                        text: ''
                    },
                    xAxis: {
                        categories: produceDays()
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: "件数(件),体积(m³),重量(kg)"
                        }
                    },
                    tooltip: {
                        headerFormat: '<center><span style="font-size:20px">{point.key}</span></center><table style="width:130px">',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y:.1f} </b></td></tr>',
                        footerFormat: '</table>',
                        shared: true,
                        useHTML: true
                    },
                    plotOptions: {
                        column: {
                            pointPadding: 0.1,
                            borderWidth: 0
                        }
                    },
                    series: data,
                    credits: {//右下角的文本  
                        enabled: false,
                        position: {//位置设置  
                            align: 'center',
                            x: -5,
                            y: -5
                        },
                        href: "http://www.baidu.com", //点击文本时的链接  
                        style: {
                            color: 'blue'
                        },
                        text: ""//显示的内容  
                    }
                });

            }

        });
    }

    function produceDays() {
        var strBeginD = $("#txtDate_Begin").val();
        var strEndD = $("#txtDate_End").val();
        var d1 = new Date(strBeginD);
        var d2 = new Date(strEndD);
        var strDayInfo = "";
        //console.info(getDateStr(d2));
        while (d1 <= d2) {
            //console.info(getDateStr(d1));
            strDayInfo = strDayInfo + getDateStrDays(d1) + ","
            d1.setDate(d1.getDate() + 1);
            //d1 = DateAdd("d", 1, d1);
        }
        strDayInfo = strDayInfo.substring(0, strDayInfo.length - 1);
        var arrDays = strDayInfo.split(',');
        return arrDays;
    }

    function getDateStrDays(date) {
        var year = date.getYear();
        var month = date.getMonth() + 1;
        if (month < 10) {
            month = "0" + month;
        }
        var day = date.getDate();
        if (day < 10) {
            day = "0" + day;
        }
        var h = date.getHours();
        if (h < 10) {
            h = "0" + h;
        }
        var m = date.getMinutes();
        if (m < 10) {
            m = "0" + m;
        }
        var s = date.getSeconds();
        if (s < 10) {
            s = "0" + s;
        }
        //return year + "-" + month + "-" + day + " " + h + ":" + m + ":" + s;
        return month + "-" + day;
    }

    //    function DateAdd(interval, num, dateValue) {
    //        var newCom = new TimeCom(dateValue);
    //        switch (String(interval).toLowerCase()) {
    //            case "y": case "year": newCom.year += num; break;
    //            case "n": case "month": newCom.month += num; break;
    //            case "d": case "day": newCom.day += num; break;
    //            case "h": case "hour": newCom.hour += num; break;
    //            case "m": case "minute": newCom.minute += num; break;
    //            case "s": case "second": newCom.second += num; break;
    //            case "ms": case "msecond": newCom.msecond += num; break;
    //            case "w": case "week": newCom.day += num * 7; break;
    //            default: return ("invalid");
    //        }
    //        var now = newCom.year + "/" + newCom.month + "/" + newCom.day + " " + newCom.hour + ":" + newCom.minute + ":" + newCom.second;
    //        return (new Date(now));
    //    }
});
    