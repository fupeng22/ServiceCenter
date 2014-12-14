$(function () {
    var _$_datagrid = $("#DG_Equipment");
    var _$_ddlConnType = $("#ddlConnType");
    var _$_ddlGroupId = $("#ddlGroupId");

    var QueryURL = "/" + pub_WhichLang + "/EquipmentManagement/GetData";
    var CreateURL = "/" + pub_WhichLang + "/EquipmentManagement/AddEquipment?DeviceServiceID=";
    var UpdateURL = "/" + pub_WhichLang + "/EquipmentManagement/UpdateEquipment?ID=";
    var DeleteURL = "/" + pub_WhichLang + "/EquipmentManagement/Delete";
    var TestExist_EuipmentName = "/" + pub_WhichLang + "/EquipmentManagement/ExistEquipmentName?strEquipmentName=";
    var TestExist_EuipmentName_Update = "/" + pub_WhichLang + "/EquipmentManagement/ExistEquipmentName_Update?id=";
    var CreateDlg = null;
    var CreateDlgForm = null;

    var LoadGroupURL = "/" + pub_WhichLang + "/GroupManagement/GetData?state=open";

    var PrintURL = "";

    QueryURL = "/" + pub_WhichLang + "/EquipmentManagement/GetData?DeviceName=" + encodeURI($("#txtDeviceName_Q").val());

    $("#btnQuery").click(function () {
        Query();
    });

    $("#btnAdd").click(function () {
        Add();
    });

    $("#btnUpdate").click(function () {
        Update();
    });

    $("#btnDelete").click(function () {
        Delete();
    });

    $("#btnSeleAll").click(function () {
        SeleAll();
    });

    $("#btnInverseSele").click(function () {
        InverseSele();
    });

    $("#btnPrint").click(function () {
        Print();
    });

    $("#btnExcel").click(function () {
        Excel();
    });

    _$_datagrid.datagrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'DeviceName',
        sortOrder: 'asc',
        remoteSort: true,
        border: false,
        idField: 'ID',
        columns: [[
                        { field: 'cb', width: 120, checkbox: true },
    					{ field: 'DeviceServiceID', title: EquipmentManagement_JS_Field1, width: 120, sortable: true,
    					    sorter: function (a, b) {
    					        return (a > b ? 1 : -1);
    					    }
    					},
                        { field: 'DeviceName', title: EquipmentManagement_JS_Field2, width: 200, sortable: true,
                            sorter: function (a, b) {
                                return (a > b ? 1 : -1);
                            }
                        },
                        { field: 'GroupName', title: EquipmentManagement_JS_Field3, width: 300, sortable: true,
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
            $('<div  id="mnuQuery" iconCls="icon-search"/>').html(EquipmentManagement_JS_Menu1).appendTo(cmenu);
            $('<div  id="mnuAdd" iconCls="icon-add"/>').html(EquipmentManagement_JS_Menu2).appendTo(cmenu);
            $('<div  id="mnuUpdate" iconCls="icon-edit"/>').html(EquipmentManagement_JS_Menu3).appendTo(cmenu);
            $('<div  id="mnuDelete" iconCls="icon-remove"/>').html(EquipmentManagement_JS_Menu4).appendTo(cmenu);
            $('<div  id="mnuSeleAll" iconCls="icon-seleall"/>').html(EquipmentManagement_JS_Menu5).appendTo(cmenu);
            $('<div  id="mnuInverseSele" iconCls="icon-inversesele"/>').html(EquipmentManagement_JS_Menu6).appendTo(cmenu);
            $('<div  id="mnuPrint" iconCls="icon-print"/>').html(EquipmentManagement_JS_Menu7).appendTo(cmenu);
            $('<div  id="mnuExcel" iconCls="icon-excel"/>').html(EquipmentManagement_JS_Menu8).appendTo(cmenu);
            cmenu.menu({
                onClick: function (item) {
                    cmenu.remove();
                    switch (item.id.toLowerCase()) {
                        case "mnuquery":
                            Query();
                            break;
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
            Update(rowData.ID);
        }
    });

    function Query() {
        QueryURL = "/" + pub_WhichLang + "/EquipmentManagement/GetData?DeviceName=" + encodeURI($("#txtDeviceName_Q").val());
        window.setTimeout(function () {
            $.extend(_$_datagrid.datagrid("options"), {
                url: QueryURL
            });
            _$_datagrid.datagrid("reload");
        }, 20); //延迟100毫秒执行，时间可以更短
    }

    function Print() {
        PrintURL = "/" + pub_WhichLang + "/EquipmentManagement/Print?DeviceName=" + encodeURI($("#txtDeviceName_Q").val()) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
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

        PrintURL = "/" + pub_WhichLang + "/EquipmentManagement/Excel?DeviceName=" + encodeURI($("#txtDeviceName_Q").val()) + "&order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            window.open(PrintURL);

        } else {
            //reWriteMessagerAlert("提示", "没有数据，不可导出", "error");
            reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForExcel, "error");
            return false;
        }
    }

    function Add() {
        $("#txtDeviceId").val("");
        $("#hd_ID").val("");
        $("#txtDeviceName").val("");

        _$_ddlGroupId.combotree({
            url: LoadGroupURL,
            valueField: 'id',
            panelHeight: null,
            textField: 'text',
            onLoadSuccess: function () {
                _$_ddlGroupId.combotree("reset");
            }
        });

        CreateDlg = $('#dlg_Create_Equipment').dialog({
            buttons: [{
                text: Public_Dialog_Save,
                iconCls: 'icon-ok',
                handler: function () {
                    var DeviceId = $("#txtDeviceId").val();
                    var DeviceName = $("#txtDeviceName").val();
                    var Group_Id = _$_ddlGroupId.combotree("getValue");
                    if (DeviceId == "" || DeviceName == "" || (!Group_Id)) {
                        reWriteMessagerAlert(Public_Dialog_Title, EquipmentManagement_JS_ErrorTip1 + '<br/>(' + EquipmentManagement_JS_ErrorTip2 + ')', "error");
                        return false;
                    }

                    var bExist = false;
                    $.ajax({
                        type: "GET",
                        url: TestExist_EuipmentName + encodeURI(DeviceName),
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
                            url: CreateURL + encodeURI(DeviceId) + "&DeviceName=" + encodeURI(DeviceName)  + "&Group_ID=" + encodeURI(Group_Id) ,
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
            title: EquipmentManagement_JS_DialogAdd,
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 500,
            height: 250
        });

        $('#dlg_Create_Equipment').dialog("open");
    }

    function Update() {
        var selects = _$_datagrid.datagrid("getSelections");
        if (selects.length != 1) {
            reWriteMessagerAlert(Public_Dialog_Title, "<center>" + EquipmentManagement_JS_ErrorTip3 + "(<font style='color:red'>" + EquipmentManagement_JS_ErrorTip4 + "</font>)</center>", "error");
            return false;
        } else {
            $("#hd_ID").val(selects[0].ID);
            $("#txtDeviceId").val(selects[0].DeviceServiceID);
            $("#txtDeviceName").val(selects[0].DeviceName);
            var groupId = selects[0].Group_ID;
            _$_ddlGroupId.combotree({
                url: LoadGroupURL,
                valueField: 'id',
                textField: 'text',
                panelHeight: null,
                onLoadSuccess: function (node, data) {
                    _$_ddlGroupId.combotree("setValue", groupId);
                }
            });

            CreateDlg = $('#dlg_Create_Equipment').dialog({
                buttons: [{
                    text: Public_Dialog_Save,
                    iconCls: 'icon-ok',
                    handler: function () {
                        var DeviceId = $("#txtDeviceId").val();
                        var DeviceName = $("#txtDeviceName").val();
                        var Group_Id = _$_ddlGroupId.combotree("getValue");
                        var ID = $("#hd_ID").val();

                        if (ID == "" || DeviceId == "" || DeviceName == ""|| (!Group_Id)) {
                            reWriteMessagerAlert(Public_Dialog_Title, EquipmentManagement_JS_ErrorTip1 + '<br/>(' + EquipmentManagement_JS_ErrorTip2 + ')', "error");
                            return false;
                        }

                        //验证此用户名是否已使用
                        var bExist = false;
                        $.ajax({
                            type: "GET",
                            url: TestExist_EuipmentName_Update + encodeURI(ID) + "&strEquipmentName=" + encodeURI(DeviceName),
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
                                url: UpdateURL + encodeURI(ID) + "&DeviceServiceID=" + encodeURI(DeviceId) + "&DeviceName=" + encodeURI(DeviceName) + "&Group_ID=" + encodeURI(Group_Id) ,
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
                title: EquipmentManagement_JS_DialogUpdate,
                modal: true,
                resizable: true,
                cache: false,
                left: 50,
                top: 30,
                width: 500,
                height: 250,
                closed: true
            });
            _$_datagrid.datagrid("unselectAll");
        }
        $('#dlg_Create_Equipment').dialog("open");
    }

    function Delete() {
        reWriteMessagerConfirm(Public_Dialog_Title, EquipmentManagement_JS_ErrorTip5,
        //reWriteMessagerConfirm(Public_Dialog_Title, User_JS_ErrorMessage5,
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.datagrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids.push(selects[i].ID);
                            }
                            if (selects.length == 0) {
                                $.messager.alert(Public_Dialog_Title, "<center>" + EquipmentManagement_JS_ErrorTip6 + "</center>", "error");
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
                                        Query();
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
                case "equipmentname":
                    break;

                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "EquipmentName") {

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
