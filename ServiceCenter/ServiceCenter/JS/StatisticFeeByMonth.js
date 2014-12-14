$(function () {
    var LoadStationsURL = "/" + pub_WhichLang + "/Default/LoadStations";
    var LoadEmpNOURL = "/" + pub_WhichLang + "/Default/LoadAllEmp";
    var GetDataURL = "/" + pub_WhichLang + "/StatisticFeeByMonth/GetData?dYM_Begin=";
    var _$_ddlStation = $("#ddlStation");
    var _$_ddlEmpNO = $("#ddlEmpNO");

    _$_ddlStation.combobox({
        url: LoadStationsURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null,
        onLoadSuccess: function () {
            _$_ddlStation.combobox("setValue", "-99");
        }
    });

    _$_ddlEmpNO.combobox({
        url: LoadEmpNOURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null,
        onLoadSuccess: function () {
            _$_ddlEmpNO.combobox("setValue", "-99");
        }
    });

    $("#btnQuery").click(function () {
        LoadData();
    });

    $("#btnQuery").click();

    function LoadData() {
        $.ajax({
            url: GetDataURL + encodeURI($("#txtDate_Begin").val()) + "&dYM_End=" + encodeURI($("#txtDate_End").val()) + "&Station=" + _$_ddlStation.combobox("getValue") + "&EmpNO=" + encodeURI(_$_ddlEmpNO.combobox("getValue")),
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
                        text: "[" + $("#txtDate_Begin").val() + "__" + $("#txtDate_End").val() + ']' + StatisticFeeByMonth_JS_Label1
                    },
                    subtitle: {
                        text: ''
                    },
                    xAxis: {
                        categories: produceMonth()
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: StatisticFeeByMonth_JS_Label2
                        }
                    },
                    tooltip: {
                        headerFormat: '<center><span style="font-size:20px">{point.key}</span></center><table style="width:150px">',
                        pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                    '<td style="padding:0"><b>{point.y} </b></td></tr>',
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

    function produceMonth() {
        var strBeginD = $("#txtDate_Begin").val() + "-01";
        var strEndD = $("#txtDate_End").val() + "-01";
        var d1 = new Date(strBeginD);
        var d2 = new Date(strEndD);
        var strDayInfo = "";
        //console.info(getDateStr(d2));
        while (d1 <= d2) {
            //console.info(getDateStr(d1));
            strDayInfo = strDayInfo + getDateStrMonth(d1) + ","
            d1.setMonth(d1.getMonth() + 1);
            //d1 = DateAdd("d", 1, d1);
        }
        strDayInfo = strDayInfo.substring(0, strDayInfo.length - 1);
        var arrDays = strDayInfo.split(',');
        return arrDays;
    }

    function getDateStrMonth(date) {
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
        return year.toString().substring(1) + "-" + month;
    }
});
    