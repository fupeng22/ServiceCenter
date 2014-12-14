$(function () {
    var _$_datagrid = $("#DG_Employee");
    var _$_ddlEmpDepartment = $("#ddlEmpDepartment");
    var QueryURL = "/" + pub_WhichLang + "/EmployeeManagement/GetData";
    var CreateURL = "/" + pub_WhichLang + "/EmployeeManagement/AddEmployee?EmpNO=";
    var UpdateURL = "/" + pub_WhichLang + "/EmployeeManagement/UpdateEmployee?EmpId=";
    var DeleteURL = "/" + pub_WhichLang + "/EmployeeManagement/Delete";
    var TestExist_EmpNO = "/" + pub_WhichLang + "/EmployeeManagement/ExistEmployeeNO?strEmpNo=";
    var TestExist_EmpNO_Update = "/" + pub_WhichLang + "/EmployeeManagement/ExistEmployeeNO_Update?id=";
    var CreateDlg = null;
    var CreateDlgForm = null;

    var LoadGroupURL = "/" + pub_WhichLang + "/GroupManagement/GetData?state=open";

    var PrintURL = "";

    _$_datagrid.datagrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'EmpNO',
        sortOrder: 'asc',
        remoteSort: true,
        border: false,
        idField: 'cId',
        columns: [[
                        { field: 'cb', width: 120, checkbox: true },
    					{ field: 'EmpNO', title: EmployeeManagement_JS_Field1, width: 120, sortable: true,
    					    sorter: function (a, b) {
    					        return (a > b ? 1 : -1);
    					    }
    					},
                        { field: 'EmpName', title: EmployeeManagement_JS_Field2, width: 120, sortable: true,
                            sorter: function (a, b) {
                                return (a > b ? 1 : -1);
                            }
                        },
                        { field: 'GroupName', title: EmployeeManagement_JS_Field3, width: 200, sortable: true,
                            sorter: function (a, b) {
                                return (a > b ? 1 : -1);
                            }
                        }
    				]],
        pagination: true,
        pageSize: 15,
        pageList: [15, 20, 25, 30, 35, 40, 45, 50],
        toolbar: [{
            id: 'btnQuery',
            text: EmployeeManagement_JS_btnQuery,
            iconCls: 'icon-search',
            handler: function () {
                _$_datagrid.datagrid("reload");
                _$_datagrid.datagrid("unselectAll");
            }
        }, '-', {
            id: 'btnAdd',
            text: EmployeeManagement_JS_btnAdd,
            iconCls: 'icon-add',
            handler: function () {
                Add();
            }
        }, '-', {
            id: 'btnUpdate',
            text: EmployeeManagement_JS_btnUpdate,
            iconCls: 'icon-edit',
            handler: function () {
                Update();
            }
        }, '-', {
            id: 'btnDelete',
            text: EmployeeManagement_JS_btnDelete,
            disabled: false,
            iconCls: 'icon-remove',
            handler: function () {
                Delete();
            }
        }, '-', {
            id: 'btnSeleAll',
            text: EmployeeManagement_JS_btnSeleAll,
            disabled: false,
            iconCls: 'icon-seleall',
            handler: function () {
                SeleAll();
            }
        }, '-', {
            id: 'btnInverseSele',
            text: EmployeeManagement_JS_btnInverseSele,
            disabled: false,
            iconCls: 'icon-inversesele',
            handler: function () {
                InverseSele();
            }
        }, '-', {
            id: 'btnPrint',
            text: EmployeeManagement_JS_btnPrint,
            disabled: false,
            iconCls: 'icon-print',
            handler: function () {
                Print();
            }
        }, '-', {
            id: 'btnExcel',
            text: EmployeeManagement_JS_btnExcel,
            disabled: false,
            iconCls: 'icon-excel',
            handler: function () {
                Excel();
            }
        }],
        onRowContextMenu: function (e, rowIndex, rowData) {
            e.preventDefault();
            _$_datagrid.datagrid("unselectAll");
            _$_datagrid.datagrid("selectRow", rowIndex);

            var cmenu = $('<div id="cmenu" style="width:100px;"></div>').appendTo('body');
            $('<div  id="mnuQuery" iconCls="icon-search"/>').html(EmployeeManagement_JS_btnQuery).appendTo(cmenu);
            $('<div  id="mnuAdd" iconCls="icon-add"/>').html(EmployeeManagement_JS_btnAdd).appendTo(cmenu);
            $('<div  id="mnuUpdate" iconCls="icon-edit"/>').html(EmployeeManagement_JS_btnUpdate).appendTo(cmenu);
            $('<div  id="mnuDelete" iconCls="icon-remove"/>').html(EmployeeManagement_JS_btnDelete).appendTo(cmenu);
            $('<div  id="mnuSeleAll" iconCls="icon-seleall"/>').html(EmployeeManagement_JS_btnSeleAll).appendTo(cmenu);
            $('<div  id="mnuInverseSele" iconCls="icon-inversesele"/>').html(EmployeeManagement_JS_btnInverseSele).appendTo(cmenu);
            $('<div  id="mnuPrint" iconCls="icon-print"/>').html(EmployeeManagement_JS_btnPrint).appendTo(cmenu);
            $('<div  id="mnuExcel" iconCls="icon-excel"/>').html(EmployeeManagement_JS_btnExcel).appendTo(cmenu);
            cmenu.menu({
                onClick: function (item) {
                    cmenu.remove();
                    switch (item.id.toLowerCase()) {
                        case "mnuadd":
                            Add();
                            break;
                        case "mnuupdate":
                            Update();
                            break;
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
        },
        onDblClickRow: function (rowIndex, rowData) {
            _$_datagrid.datagrid("unselectAll");
            _$_datagrid.datagrid("selectRow", rowIndex);
            Update(rowData.cId);
        }
    });

    function Print() {
        PrintURL = "/" + pub_WhichLang + "/EmployeeManagement/Print?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
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
            //reWriteMessagerAlert("提示", "没有数据，不可打印", "error");
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

        PrintURL = "/" + pub_WhichLang + "/EmployeeManagement/Excel?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            window.open(PrintURL);

        } else {
            //reWriteMessagerAlert("提示", "没有数据，不可导出", "error");
            reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForExcel, "error");
            return false;
        }
    }

    function Add() {
        $("#txtEmpNO").val("");
        $("#hd_EmpId").val("");
        $("#txtEmpName").val("");

        _$_ddlEmpDepartment.combotree({
            url: LoadGroupURL,
            valueField: 'id',
            panelHeight: null,
            textField: 'text'
        });

        CreateDlg = $('#dlg_Create_Employee').dialog({
            buttons: [{
                text: Public_Dialog_Save,
                iconCls: 'icon-ok',
                handler: function () {
                    var EmpNo = $("#txtEmpNO").val();
                    var EmpName = $("#txtEmpName").val();
                    var EmpDepartment = _$_ddlEmpDepartment.combotree("getValue");

                    if (EmpNo == "" || EmpName == "" || (!EmpDepartment)) {
                        reWriteMessagerAlert(Public_Dialog_Title, EmployeeManagement_JS_ErrorTip1 + '<br/>(' + EmployeeManagement_JS_ErrorTip2 + ')', "error");
                        return false;
                    }

                    var bExist = false;
                    $.ajax({
                        type: "GET",
                        url: TestExist_EmpNO + encodeURI(EmpNo),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'error') {
                                bExist = true;
                                //reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                                reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                return false;
                            } else {

                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {

                        },
                        error: function () {

                        }
                    });

                    if (!bExist) {
                        var bOK = false;
                        $.ajax({
                            type: "POST",
                            url: CreateURL + encodeURI(EmpNo) + "&EmpName=" + encodeURI(EmpName) + "&EmpDepartment=" + encodeURI(EmpDepartment),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                var JSONMsg = eval("(" + msg + ")");
                                if (JSONMsg.result.toLowerCase() == 'ok') {
                                    bOK = true;
                                    reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'info');
                                } else {
                                    reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });
                        if (bOK) {
                            CreateDlg.dialog('close');
                            _$_datagrid.datagrid("reload");
                            _$_datagrid.datagrid("unselectAll");
                        }
                    }
                }
            }, {
                text: Public_Dialog_Close,
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateDlg.dialog('close');
                }
            }],
            title: EmployeeManagement_JS_AddEmployee,
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 200
        });

        $('#dlg_Create_Employee').dialog("open");
    }

    function Update() {
        var selects = _$_datagrid.datagrid("getSelections");
        if (selects.length != 1) {
            reWriteMessagerAlert(Public_Dialog_Title, "<center>" + EmployeeManagement_JS_ErrorTip3 + "(<font style='color:red'>" + EmployeeManagement_JS_ErrorTip4 + "</font>)</center>", "error");
            return false;
        } else {
            $("#hd_EmpId").val(selects[0].cId);
            $("#txtEmpNO").val(selects[0].EmpNO);
            $("#txtEmpName").val(selects[0].EmpName);
            var empDepartment = selects[0].EmpDepartment;
            _$_ddlEmpDepartment.combotree({
                url: LoadGroupURL,
                valueField: 'id',
                textField: 'text',
                panelHeight: null,
                onLoadSuccess: function (node, data) {
                    _$_ddlEmpDepartment.combotree("setValue", empDepartment);
                }
            });

            CreateDlg = $('#dlg_Create_Employee').dialog({
                buttons: [{
                    text: Public_Dialog_Save,
                    iconCls: 'icon-ok',
                    handler: function () {
                        var EmpId = $("#hd_EmpId").val();
                        var EmpNo = $("#txtEmpNO").val();
                        var EmpName = $("#txtEmpName").val();
                        var EmpDepartment = _$_ddlEmpDepartment.combotree("getValue");

                        if (EmpId == "" || EmpNo == "" || EmpName == "" || (!EmpDepartment)) {
                            reWriteMessagerAlert(Public_Dialog_Title, EmployeeManagement_JS_ErrorTip1 + '<br/>(' + EmployeeManagement_JS_ErrorTip2 + ')', "error");
                            return false;
                        }

                        //验证此用户名是否已使用
                        var bExist = false;
                        $.ajax({
                            type: "GET",
                            url: TestExist_EmpNO_Update + encodeURI(EmpId) + "&strEmpNo=" + encodeURI(EmpNo),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                //console.info(msg);
                                var JSONMsg = eval("(" + msg + ")");
                                if (JSONMsg.result.toLowerCase() == 'error') {
                                    bExist = true;
                                    reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                } else {
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });

                        if (!bExist) {
                            var bOK = false;
                            $.ajax({
                                type: "POST",
                                url: UpdateURL + encodeURI(EmpId) + "&EmpNO=" + encodeURI(EmpNo) + "&EmpName=" + encodeURI(EmpName) + "&EmpDepartment=" + encodeURI(EmpDepartment),
                                data: "",
                                async: false,
                                cache: false,
                                beforeSend: function (XMLHttpRequest) {

                                },
                                success: function (msg) {
                                    var JSONMsg = eval("(" + msg + ")");
                                    if (JSONMsg.result.toLowerCase() == 'ok') {
                                        bOK = true;
                                        reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'info');
                                    } else {
                                        reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                    }
                                },
                                complete: function (XMLHttpRequest, textStatus) {

                                },
                                error: function () {

                                }
                            });
                            if (bOK) {
                                CreateDlg.dialog('close');
                                _$_datagrid.datagrid("reload");
                                _$_datagrid.datagrid("unselectAll");
                            }
                        }
                    }
                }, {
                    text: Public_Dialog_Close,
                    iconCls: 'icon-cancel',
                    handler: function () {
                        CreateDlg.dialog('close');
                    }
                }],
                title: EmployeeManagement_JS_UpdateEmployee,
                modal: true,
                resizable: true,
                cache: false,
                left: 50,
                top: 30,
                width: 400,
                height: 200,
                closed: true
            });
            _$_datagrid.datagrid("unselectAll");
        }
        $('#dlg_Create_Employee').dialog("open");
    }

    function Delete() {
        reWriteMessagerConfirm(Public_Dialog_Title, EmployeeManagement_JS_DeleteAsk,
        //reWriteMessagerConfirm(Public_Dialog_Title, User_JS_ErrorMessage5,
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.datagrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids.push(selects[i].cId);
                            }
                            if (selects.length == 0) {
                                $.messager.alert(Public_Dialog_Title, "<center>" + EmployeeManagement_JS_ErrorTip5 + "</center>", "error");
                                //$.messager.alert(Public_Dialog_Title, User_JS_ErrorMessage6, "error");
                                return false;
                            }

                            $.ajax({
                                url: DeleteURL + '?ids=' + ids.join(','),
                                type: "POST",
                                cache: false,
                                async: false,
                                success: function (msg) {
                                    var JSONMsg = eval("(" + msg + ")");
                                    if (JSONMsg.result.toLowerCase() == 'ok') {
                                        // $.messager.alert('操作提示', JSONMsg.message, 'info');
                                        $.messager.alert(Public_Dialog_Title, JSONMsg.message, 'info');
                                        _$_datagrid.datagrid("reload");
                                        _$_datagrid.datagrid("unselectAll");
                                    } else {
                                        // $.messager.alert('操作提示', JSONMsg.message, 'error');
                                        $.messager.alert(Public_Dialog_Title, JSONMsg.message, 'error');
                                        return false;
                                    }
                                }
                            });

                        } else {

                        }
                    }
                );
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
                case "empno":
                    break;

                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "EmpNO") {

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
