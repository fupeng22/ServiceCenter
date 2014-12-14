$(function () {
    var _$_datagrid = $("#DG_ExceptionWayBill");
    var QueryURL = "/" + pub_WhichLang + "/QueryExceptionWayBill/GetData";
    var DeleteURL = "/" + pub_WhichLang + "/QueryExceptionWayBill/Delete";

    var LoadStationsURL = "/" + pub_WhichLang + "/Default/LoadStations";
    var LoadEmpNOURL = "/" + pub_WhichLang + "/Default/LoadAllEmp";
    var LoadFeeTypeURL = "/" + pub_WhichLang + "/Default/LoadFeeTypeJSON";
    var LoadExceptionTypeURL = "/" + pub_WhichLang + "/Default/LoadExceptionTypeJSON";
    var _$_ddlStation = $("#ddlStation");
    var _$_ddlFeeType = $("#ddlFeeType");
    var _$_ddlEmpNO = $("#ddlEmpNO");
    var _$_ddlExceptionType = $("#ddlExceptionType");

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

    _$_ddlFeeType.combobox({
        url: LoadFeeTypeURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null,
        onLoadSuccess: function () {
            _$_ddlFeeType.combobox("setValue", "-99");
        }
    });

    _$_ddlExceptionType.combobox({
        url: LoadExceptionTypeURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null,
        onLoadSuccess: function () {
            _$_ddlExceptionType.combobox("setValue", "-99");
        }
    });

    var PrintURL = "";
    QueryURL = "/" + pub_WhichLang + "/QueryExceptionWayBill/GetData?dBegin=" + encodeURI($("#txtBeginD").val()) + "&dEnd=" + encodeURI($("#txtEndD").val()) + "&strOperator=" + encodeURI(_$_ddlStation.combobox("getValue")) + "&feeFlag=" + encodeURI(_$_ddlFeeType.combobox("getValue")) + "&exceptionFlag=" + encodeURI(_$_ddlExceptionType.combobox("getValue")) + "&EmpNO=" + encodeURI(_$_ddlEmpNO.combobox("getValue"));

    $("#btnQuery").click(function () {
        QueryURL = "/" + pub_WhichLang + "/QueryExceptionWayBill/GetData?dBegin=" + encodeURI($("#txtBeginD").val()) + "&dEnd=" + encodeURI($("#txtEndD").val()) + "&strOperator=" + encodeURI(_$_ddlStation.combobox("getValue")) + "&feeFlag=" + encodeURI(_$_ddlFeeType.combobox("getValue")) + "&exceptionFlag=" + encodeURI(_$_ddlExceptionType.combobox("getValue")) + "&EmpNO=" + encodeURI(_$_ddlEmpNO.combobox("getValue"));
        window.setTimeout(function () {
            $.extend(_$_datagrid.datagrid("options"), {
                url: QueryURL
            });
            _$_datagrid.datagrid("reload");
        }, 10); //延迟100毫秒执行，时间可以更短
    });

    _$_datagrid.datagrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'wbScanTime',
        sortOrder: 'desc',
        remoteSort: true,
        border: false,
        idField: 'wbId',
        columns: [[
					{ field: 'wbName', title: QueryExceptionWayBill_JS_Field1, width: 100, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
					{ field: 'wbActualWeight', title: QueryExceptionWayBill_JS_Field2, width: 100, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
                    { field: 'wbVolume', title: QueryExceptionWayBill_JS_Field3, width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                     { field: 'wbWeightByVolume', title: QueryExceptionWayBill_JS_Field4, width: 100, sortable: true,
                         sorter: function (a, b) {
                             return (a > b ? 1 : -1);
                         }
                     },
                    { field: 'wbOperator', title: QueryExceptionWayBill_JS_Field5, width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'EmpNO', title: QueryExceptionWayBill_JS_Field10, width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'EmpName', title: QueryExceptionWayBill_JS_Field11, width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'wbScanTime', title: QueryExceptionWayBill_JS_Field6, width: 130, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'FeeFlagDesc', title: QueryExceptionWayBill_JS_Field7, width: 100, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'GroupName', title: QueryExceptionWayBill_JS_Field8, width: 200, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'ExceptionFlagDesc', title: QueryExceptionWayBill_JS_Field9, width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    }
				]],
        pagination: true,
        pageSize: 15,
        pageList: [15, 20, 25, 30, 35, 40, 45, 50],
        toolbar: "#toolBar",
        onRowContextMenu: function (e, rowIndex, rowData) {
            e.preventDefault();
            _$_datagrid.datagrid("unselectAll");
            _$_datagrid.datagrid("selectRow", rowIndex);

            var cmenu = $('<div id="cmenu" style="width:100px;"></div>').appendTo('body');
            $('<div  id="mnuQuery" iconCls="icon-search"/>').html(QueryExceptionWayBill_JS_ToolBar1).appendTo(cmenu);
            $('<div  id="mnuPrint" iconCls="icon-print"/>').html(QueryExceptionWayBill_JS_ToolBar2).appendTo(cmenu);
            $('<div  id="mnuExcel" iconCls="icon-excel"/>').html(QueryExceptionWayBill_JS_ToolBar3).appendTo(cmenu);
            cmenu.menu({
                onClick: function (item) {
                    cmenu.remove();
                    switch (item.id.toLowerCase()) {
                        case "mnudelete":
                            Delete();
                            break;
                        case "mnuseleall":
                            SeleAll();
                            break;
                        case "mnuinversesele":
                            InverseSele();
                            break;
                        case "mnuprint":
                            Print();
                            break;
                        case "mnuexcel":
                            Excel();
                            break;
                    }
                }
            });

            $('#cmenu').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        },
        onHeaderContextMenu: function (e, field) {
            e.preventDefault();
            if (!$('#tmenu').length) {
                createColumnMenu();
            }
            $('#tmenu').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        },
        onSortColumn: function (sort, order) {
            // _$_datagrid.datagrid("reload");
        }
    });

    setTimeout(function () {
        $("#btnQuery").click();
    }, 100);


    $("#btnPrint").click(function () {
        Print();
    });

    $("#btnExcel").click(function () {
        Excel();
    });

    function Print() {
        PrintURL = "/" + pub_WhichLang + "/QueryExceptionWayBill/Print?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&dBegin=" + encodeURI($("#txtBeginD").val()) + "&dEnd=" + encodeURI($("#txtEndD").val()) + "&strOperator=" + encodeURI(_$_ddlStation.combobox("getValue")) + "&feeFlag=" + encodeURI(_$_ddlFeeType.combobox("getValue")) + "&exceptionFlag=" + encodeURI(_$_ddlExceptionType.combobox("getValue")) + "&EmpNO=" + encodeURI(_$_ddlEmpNO.combobox("getValue")) + "&page=1&rows=10000000";
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
            div_PrintDlg.show();
            var PrintDlg = null;
            div_PrintDlg.find("#frmPrintURL").attr("src", PrintURL);
            PrintDlg = div_PrintDlg.window({
                title: Public_Print_Title,
                href: "",
                modal: true,
                resizable: true,
                minimizable: false,
                collapsible: false,
                cache: false,
                closed: true,
                width: 900,
                height: 500
            });
            div_PrintDlg.window("open");

        } else {
            reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForPrint, "error");
            return false;
        }
    }

    function Excel() {
        var browserType = "";
        if ($.browser.msie) {
            browserType = "msie";
        }
        else if ($.browser.safari) {
            browserType = "safari";
        }
        else if ($.browser.mozilla) {
            browserType = "mozilla";
        }
        else if ($.browser.opera) {
            browserType = "opera";
        }
        else {
            browserType = "unknown";
        }

        PrintURL = "/" + pub_WhichLang + "/QueryExceptionWayBill/Excel?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&dBegin=" + encodeURI($("#txtBeginD").val()) + "&dEnd=" + encodeURI($("#txtEndD").val()) + "&strOperator=" + encodeURI(_$_ddlStation.combobox("getValue")) + "&feeFlag=" + encodeURI(_$_ddlFeeType.combobox("getValue")) + "&exceptionFlag=" + encodeURI(_$_ddlExceptionType.combobox("getValue")) + "&EmpNO=" + encodeURI(_$_ddlEmpNO.combobox("getValue")) + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            window.open(PrintURL);

        } else {
            reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForExcel, "error");
            return false;
        }
    }

    function SeleAll() {
        var rows = _$_datagrid.datagrid("getRows");
        for (var i = 0; i < rows.length; i++) {
            var m = _$_datagrid.datagrid("getRowIndex", rows[i]);
            _$_datagrid.datagrid("selectRow", m)
        }
    }

    function InverseSele() {
        var rows = _$_datagrid.datagrid("getRows");
        var selects = _$_datagrid.datagrid("getSelections");
        for (var i = 0; i < rows.length; i++) {
            var bSele = false;
            var m = _$_datagrid.datagrid("getRowIndex", rows[i]);
            for (var j = 0; j < selects.length; j++) {
                var n = _$_datagrid.datagrid("getRowIndex", selects[j]);
                if (m == n) {
                    bSele = true;
                }
            }
            if (bSele) {
                _$_datagrid.datagrid("unselectRow", m)
            } else {
                _$_datagrid.datagrid("selectRow", m)
            }
        }
    }

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:100px;"></div>').appendTo('body');
        var fields = _$_datagrid.datagrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.datagrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "cb":
                    break;
                case "urNum":
                    break;

                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "urNum") {

                } else {
                    if (item.iconCls == 'icon-ok') {
                        _$_datagrid.datagrid('hideColumn', $(item.text).attr("id"));
                        tmenu.menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-empty'
                        });
                    } else {
                        _$_datagrid.datagrid('showColumn', $(item.text).attr("id"));
                        tmenu.menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-ok'
                        });
                    }
                }
            }
        });
    }
});
