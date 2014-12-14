$(function () {
    var LoadStationsURL = "/" + pub_WhichLang + "/Default/LoadStations";
    var LoadEmpNOURL = "/" + pub_WhichLang + "/Default/LoadAllEmp";
    var GetDataURL = "/" + pub_WhichLang + "/StatisticFeeByMonth_Pie/GetData?dYM_Begin=";
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
            url: GetDataURL + encodeURI($("#txtDate").val()) + "&dYM_End=" + encodeURI($("#txtDate").val()) + "&Station=" + _$_ddlStation.combobox("getValue") + "&EmpNO=" + encodeURI(_$_ddlEmpNO.combobox("getValue")),
            type: "POST",
            cache: false,
            async: false,
            success: function (msg) {
                var data = eval("(" + msg + ")");
                $('#container').highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotBorderWidth: 1, //null,
                        plotShadow: false
                    },
                    title: {
                        text: "[" + $("#txtDate").val() + ']' + StatisticFeeByMonth_Pie_JS_Label1
                    },
                    subtitle: {
                        text: ''
                    },
                    plotOptions: {
                        pie: {
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                style: {
                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            },
                            showInLegend: true
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
    