$(function () {
    var LoadStationsURL = "/" + pub_WhichLang + "/Default/LoadStations";
    var LoadEmpNOURL = "/" + pub_WhichLang + "/Default/LoadAllEmp";
    var GetDataURL = "/" + pub_WhichLang + "/StatisticByHour_WayBill/GetData?dYMD=";
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
            url: GetDataURL + encodeURI($("#txtDate").val()) + "&Station=" + _$_ddlStation.combobox("getValue") + "&EmpNO=" + encodeURI(_$_ddlEmpNO.combobox("getValue")),
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
                        text: "[" + $("#txtDate").val() + ']' + StatisticByHour_WayBill_JS_Tip1 + ''
                    },
                    subtitle: {
                        text: StatisticByHour_WayBill_JS_Tip2
                    },
                    xAxis: {
                        categories: [
                    '0' + StatisticByHour_WayBill_JS_Label1,
                    '1' + StatisticByHour_WayBill_JS_Label1,
                    '2' + StatisticByHour_WayBill_JS_Label1,
                    '3' + StatisticByHour_WayBill_JS_Label1,
                    '4' + StatisticByHour_WayBill_JS_Label1,
                    '5' + StatisticByHour_WayBill_JS_Label1,
                    '6' + StatisticByHour_WayBill_JS_Label1,
                    '7' + StatisticByHour_WayBill_JS_Label1,
                    '8' + StatisticByHour_WayBill_JS_Label1,
                    '9' + StatisticByHour_WayBill_JS_Label1,
                    '10' + StatisticByHour_WayBill_JS_Label1,
                    '11' + StatisticByHour_WayBill_JS_Label1,
                    '12' + StatisticByHour_WayBill_JS_Label1,
                    '13' + StatisticByHour_WayBill_JS_Label1,
                    '14' + StatisticByHour_WayBill_JS_Label1,
                    '15' + StatisticByHour_WayBill_JS_Label1,
                    '16' + StatisticByHour_WayBill_JS_Label1,
                    '17' + StatisticByHour_WayBill_JS_Label1,
                    '18' + StatisticByHour_WayBill_JS_Label1,
                    '19' + StatisticByHour_WayBill_JS_Label1,
                    '20' + StatisticByHour_WayBill_JS_Label1,
                    '21' + StatisticByHour_WayBill_JS_Label1,
                    '22' + StatisticByHour_WayBill_JS_Label1,
                    '23' + StatisticByHour_WayBill_JS_Label1
                ]
                    },
                    yAxis: {
                        min: 0,
                        title: {
                            text: StatisticByHour_WayBill_JS_Tip3
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
                            pointPadding: 0.2,
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
});
    