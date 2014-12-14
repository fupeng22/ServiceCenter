$(function () {
    var _$_datagrid = $("#DG_UserMaintain");
    var _$_ddlGroup = $("#ddlGroup");
    var QueryURL = "/" + pub_WhichLang + "/UserManagement/GetData";
    var CreateURL = "/" + pub_WhichLang + "/UserManagement/AddUsers?UserName=";
    var UpdateURL = "/" + pub_WhichLang + "/UserManagement/UpdateUsers?UserId=";
    var DeleteURL = "/" + pub_WhichLang + "/UserManagement/Delete";
    var TestExist_UserName = "/" + pub_WhichLang + "/UserManagement/ExistUserNum?strUserNum=";
    var TestExist_UserName_Update = "/" + pub_WhichLang + "/UserManagement/ExistUserNum_Update?id=";
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
        sortName: 'Username',
        sortOrder: 'asc',
        remoteSort: true,
        border: false,
        idField: 'cId',
        columns: [[
                        { field: 'cb', width: 120, checkbox: true },
    					{ field: 'Username', title: UserManagement_JS_Field1, width: 120, sortable: true,
    					    sorter: function (a, b) {
    					        return (a > b ? 1 : -1);
    					    }
    					},
                        { field: 'GroupName', title: UserManagement_JS_Field2, width: 400, sortable: true,
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
            text: UserManagement_JS_Toolbar1,
            iconCls: 'icon-search',
            handler: function () {
                _$_datagrid.datagrid("reload");
                _$_datagrid.datagrid("unselectAll");
            }
        }, '-', {
            id: 'btnAdd',
            text: UserManagement_JS_Toolbar2,
            iconCls: 'icon-add',
            handler: function () {
                Add();
            }
        }, '-', {
            id: 'btnUpdate',
            text: UserManagement_JS_Toolbar3,
            iconCls: 'icon-edit',
            handler: function () {
                Update();
            }
        }, '-', {
            id: 'btnDelete',
            text: UserManagement_JS_Toolbar4,
            disabled: false,
            iconCls: 'icon-remove',
            handler: function () {
                Delete();
            }
        }, '-', {
            id: 'btnSeleAll',
            text: UserManagement_JS_Toolbar5,
            disabled: false,
            iconCls: 'icon-seleall',
            handler: function () {
                SeleAll();
            }
        }, '-', {
            id: 'btnInverseSele',
            text: UserManagement_JS_Toolbar6,
            disabled: false,
            iconCls: 'icon-inversesele',
            handler: function () {
                InverseSele();
            }
        }, '-', {
            id: 'btnPrint',
            text: UserManagement_JS_Toolbar7,
            disabled: false,
            iconCls: 'icon-print',
            handler: function () {
                Print();
            }
        }, '-', {
            id: 'btnExcel',
            text: UserManagement_JS_Toolbar8,
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
            $('<div  id="mnuQuery" iconCls="icon-search"/>').html(UserManagement_JS_Toolbar1).appendTo(cmenu);
            $('<div  id="mnuAdd" iconCls="icon-add"/>').html(UserManagement_JS_Toolbar2).appendTo(cmenu);
            $('<div  id="mnuUpdate" iconCls="icon-edit"/>').html(UserManagement_JS_Toolbar3).appendTo(cmenu);
            $('<div  id="mnuDelete" iconCls="icon-remove"/>').html(UserManagement_JS_Toolbar4).appendTo(cmenu);
            $('<div  id="mnuSeleAll" iconCls="icon-seleall"/>').html(UserManagement_JS_Toolbar5).appendTo(cmenu);
            $('<div  id="mnuInverseSele" iconCls="icon-inversesele"/>').html(UserManagement_JS_Toolbar6).appendTo(cmenu);
            $('<div  id="mnuPrint" iconCls="icon-print"/>').html(UserManagement_JS_Toolbar7).appendTo(cmenu);
            $('<div  id="mnuExcel" iconCls="icon-excel"/>').html(UserManagement_JS_Toolbar8).appendTo(cmenu);
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
        PrintURL = "/" + pub_WhichLang + "/UserManagement/Print?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
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

        PrintURL = "/" + pub_WhichLang + "/UserManagement/Excel?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            window.open(PrintURL);

        } else {
            //reWriteMessagerAlert("提示", "没有数据，不可导出", "error");
            reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForExcel, "error");
            return false;
        }
    }

    function Add() {
        $("#txtUserName").val("");
        $("#hd_UserId").val("");
        $("#txtPassword").val("");
        $("#txtRePassword").val("");

        _$_ddlGroup.combotree({
            url: LoadGroupURL,
            valueField: 'id',
            panelHeight: null,
            textField: 'text'
        });

        CreateDlg = $('#dlg_Create_User').dialog({
            buttons: [{
                text: Public_Dialog_Save,
                iconCls: 'icon-ok',
                handler: function () {
                    var _$_txtUserName = $("#txtUserName").val();
                    var _$_txtPassword = $("#txtPassword").val();
                    var _$_txtRePassword = $("#txtRePassword").val();
                    var Group_Id = _$_ddlGroup.combotree("getValue");

                    if (_$_txtUserName == "" || _$_txtPassword == "" || _$_txtRePassword == "" || (!Group_Id)) {
                        reWriteMessagerAlert(Public_Dialog_Title, UserManagement_JS_ErrorTip1 + '<br/>(' + UserManagement_JS_ErrorTip2 + ')', "error");
                        return false;
                    }

                    if (_$_txtPassword != _$_txtRePassword) {
                        reWriteMessagerAlert(Public_Dialog_Title, UserManagement_JS_ErrorTip3, "error");
                        return false;
                    }

                    var bExist = false;
                    $.ajax({
                        type: "GET",
                        url: TestExist_UserName + encodeURI(_$_txtUserName),
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
                            url: CreateURL + encodeURI(_$_txtUserName) + "&Password=" + encodeURI(_$_txtPassword) + "&Group_Id=" + encodeURI(Group_Id),
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
            title: UserManagement_JS_DialogAddTitle,
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 200
        });

        $('#dlg_Create_User').dialog("open");
    }

    function Update() {
        var selects = _$_datagrid.datagrid("getSelections");
        if (selects.length != 1) {
            reWriteMessagerAlert(Public_Dialog_Title, "<center>" + UserManagement_JS_ErrorTip4 + "(<font style='color:red'>" + UserManagement_JS_ErrorTip5 + "</font>)</center>", "error");
            return false;
        } else {
            $("#hd_UserId").val(selects[0].cId);
            $("#txtUserName").val(selects[0].Username);
            $("#txtPassword").val("");
            $("#txtRePassword").val("");
            var groupId = selects[0].Group_ID;
            _$_ddlGroup.combotree({
                url: LoadGroupURL,
                valueField: 'id',
                textField: 'text',
                panelHeight: null,
                onLoadSuccess: function (node, data) {
                    _$_ddlGroup.combotree("setValue", groupId);
                }
            });

            CreateDlg = $('#dlg_Create_User').dialog({
                buttons: [{
                    text: Public_Dialog_Save,
                    iconCls: 'icon-ok',
                    handler: function () {
                        var _$_hd_UserId = $("#hd_UserId").val();
                        var _$_txtUserName = $("#txtUserName").val();
                        var _$_txtPassword = $("#txtPassword").val();
                        var _$_txtRePassword = $("#txtRePassword").val();
                        var Group_Id = _$_ddlGroup.combotree("getValue");

                        if (_$_hd_UserId == "" || _$_txtUserName == "" || _$_txtPassword == "" || _$_txtRePassword == "" || (!Group_Id)) {
                            reWriteMessagerAlert(Public_Dialog_Title, UserManagement_JS_ErrorTip1 + '<br/>(' + UserManagement_JS_ErrorTip2 + ')', "error");
                            return false;
                        }

                        if (_$_txtPassword != _$_txtRePassword) {
                            reWriteMessagerAlert(Public_Dialog_Title, UserManagement_JS_ErrorTip3, "error");
                            return false;
                        }

                        //验证此用户名是否已使用
                        var bExist = false;
                        $.ajax({
                            type: "GET",
                            url: TestExist_UserName_Update + encodeURI(_$_hd_UserId) + "&strUserNum=" + encodeURI(_$_txtUserName),
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
                                url: UpdateURL + encodeURI(_$_hd_UserId) + "&UserName=" + encodeURI(_$_txtUserName) + "&Password=" + encodeURI(_$_txtPassword) + "&Group_Id=" + encodeURI(Group_Id),
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
                title: UserManagement_JS_DialogUpdateTitle,
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
        $('#dlg_Create_User').dialog("open");
    }

    function Delete() {
        reWriteMessagerConfirm(Public_Dialog_Title, UserManagement_JS_ErrorTip6,
        //reWriteMessagerConfirm(Public_Dialog_Title, User_JS_ErrorMessage5,
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.datagrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids.push(selects[i].cId);
                            }
                            if (selects.length == 0) {
                                $.messager.alert(Public_Dialog_Title, "<center>" + UserManagement_JS_ErrorTip7 + "</center>", "error");
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
                case "urNum":
                    break;

                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "userID") {

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
