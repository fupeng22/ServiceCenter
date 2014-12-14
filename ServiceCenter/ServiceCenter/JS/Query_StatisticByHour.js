$(function () {
    var GetDataURL = "/" + pub_WhichLang + "/Query_StatisticByHour/GetData?dYMD=";

    $("#btnQuery").click(function () {
        LoadData();
    });

    $("#btnQuery").click();

    function LoadData() {
        $.ajax({
            url: GetDataURL + encodeURI($("#txtDate").val()) + "&Station=" + encodeURI($("#hid_DeviceName").val()) + "&EmpNO=" + "",
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
                        text: "[" + $("#txtDate").val() + ']' + "单位小时货物统计数据" + ''
                    },
                    subtitle: {
                        text: "统计方式,如:0时表示0时-1时时间段"
                    },
                    xAxis: {
                        categories: [
                    '0' + "时",
                    '1' + "时",
                    '2' + "时",
                    '3' + "时",
                    '4' + "时",
                    '5' + "时",
                    '6' + "时",
                    '7' + "时",
                    '8' + "时",
                    '9' + "时",
                    '10' + "时",
                    '11' + "时",
                    '12' + "时",
                    '13' + "时",
                    '14' + "时",
                    '15' + "时",
                    '16' + "时",
                    '17' + "时",
                    '18' + "时",
                    '19' + "时",
                    '20' + "时",
                    '21' + "时",
                    '22' + "时",
                    '23' + "时"
                ]
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
                        href: "", //点击文本时的链接  
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
    