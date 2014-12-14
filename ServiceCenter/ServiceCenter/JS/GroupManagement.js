$(function () {
    var _$_TVDepart = $("#TVDepart");
    var dlg_CreateCompany = $("#dlg_CreateCompany");
    var dlg_Create = $("#dlg_Create");
    var dlg_Update = $("#dlg_Update");

    var CreateCompany_dlg = null;
    var Create_dlg = null;
    var Update_dlg = null;

    var QueryURL = "/" + pub_WhichLang + "/GroupManagement/GetData?state=open";

    var TestExist = "/" + pub_WhichLang + "/GroupManagement/TestExistDepartName?departName=";
    var InsertCompany = "/" + pub_WhichLang + "/GroupManagement/InsertTopDepartment?departName=";
    var InsertDepart = "/" + pub_WhichLang + "/GroupManagement/InsertDepartment";
    var UpdateDepart = "/" + pub_WhichLang + "/GroupManagement/UpdateDepartment";
    var TestExistInOther = "/" + pub_WhichLang + "/GroupManagement/TestAlreadyUsedDepartName?departName=";
    var DeleteDepart = "/" + pub_WhichLang + "/GroupManagement/DeleDepartment?departId=";
    var ExportURL = "/" + pub_WhichLang + "/GroupManagement/SaveExcel";
    var IsDepartHasEmployee = "/" + pub_WhichLang + "/GroupManagement/TestHasUsed?departId=";

    _$_TVDepart.tree({
        url: QueryURL,
        onClick: function (node) {
            _$_TVDepart.tree('toggle', node.target);
        },
        onDblClick: function (node) {
            Update();
        },
        onContextMenu: function (e, node) {
            e.preventDefault();
            _$_TVDepart.tree('select', node.target);
            $('#mm').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        }
    });

    CreateCompany_dlg = dlg_CreateCompany.dialog({
        buttons: [{
            text: Public_Dialog_Save,
            iconCls: 'icon-ok',
            handler: function () {
                var newCompanyName = $("#txtCompanyName").val();
                newCompanyName = $.trim(newCompanyName);
                if (newCompanyName && newCompanyName != "") {
                    $.ajax({
                        type: "POST",
                        url: TestExist + encodeURI(newCompanyName),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var jsonMsg = eval("(" + msg + ")");
                            if (jsonMsg.result == "ok") {
                                $.ajax({
                                    type: "POST",
                                    url: InsertCompany + encodeURI(newCompanyName),
                                    data: "",
                                    async: true,
                                    cache: false,
                                    beforeSend: function (XMLHttpRequest) {

                                    },
                                    success: function (msg) {
                                        var JSONMsg = eval("(" + msg + ")");
                                        if (JSONMsg.result.toLowerCase() == 'ok') {
                                            reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'info');
                                            CreateCompany_dlg.dialog('close');
                                            Query();
                                        } else {
                                            reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                        }
                                    },
                                    complete: function (XMLHttpRequest, textStatus) {

                                    },
                                    error: function () {

                                    }
                                });
                            } else {
                                reWriteMessagerAlert(Public_Dialog_Title, jsonMsg.message, "error");
                                return false;
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {

                        },
                        error: function () {

                        }
                    });
                } else {
                    reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip1, 'error');
                    return false;
                }
            }
        }, {
            text: Public_Dialog_Close,
            iconCls: 'icon-cancel',
            handler: function () {
                CreateCompany_dlg.dialog('close');
            }
        }],
        title: GroupManagement_JS_DialogTitle1,
        modal: true,
        resizable: true,
        cache: false,
        closed: true,
        left: 50,
        top: 30,
        width: 400,
        height: 150
    });

    Create_dlg = $('#dlg_Create').dialog({
        buttons: [{
            text: Public_Dialog_Save,
            iconCls: 'icon-ok',
            handler: function () {
                var subDepartName = $("#txtSubDepart_Create").val();
                var ParentDepartName = $("#span_ParentDepart_Create").html();
                var ParentDepartId = $("#hid_ParentDepartId").val();
                subDepartName = $.trim(subDepartName);
                if (subDepartName && subDepartName != "") {
                    $.ajax({
                        type: "POST",
                        url: TestExist + encodeURI(subDepartName),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var jsonMsg = eval("(" + msg + ")");
                            if (jsonMsg.result == "ok") {
                                $.ajax({
                                    type: "POST",
                                    url: InsertDepart + "?departName=" + encodeURI(subDepartName) + "&parentDepartId=" + encodeURI(ParentDepartId),
                                    data: "",
                                    async: false,
                                    cache: false,
                                    beforeSend: function (XMLHttpRequest) {

                                    },
                                    success: function (msg) {
                                        var JSONMsg = eval("(" + msg + ")");
                                        if (JSONMsg.result.toLowerCase() == 'ok') {
                                            reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'info');
                                            Create_dlg.dialog('close');
                                            Query();
                                        } else {
                                            reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                        }
                                    },
                                    complete: function (XMLHttpRequest, textStatus) {

                                    },
                                    error: function () {

                                    }
                                });
                            } else {
                                reWriteMessagerAlert(Public_Dialog_Title, jsonMsg.message, "error");
                                return false;
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {

                        },
                        error: function () {

                        }
                    });
                } else {
                    reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip2, 'error');
                    return false;
                }
            }
        }, {
            text: Public_Dialog_Close,
            iconCls: 'icon-cancel',
            handler: function () {
                Create_dlg.dialog('close');
            }
        }],
        title: GroupManagement_JS_DialogTitle2,
        modal: true,
        resizable: true,
        cache: false,
        closed: true,
        left: 50,
        top: 30,
        width: 400,
        height: 150
    });

    Update_dlg = $('#dlg_Update').dialog({
        buttons: [{
            text: Public_Dialog_Save,
            iconCls: 'icon-ok',
            handler: function () {
                var newDepartName = $("#txtNewDepartName").val();
                var oldDepartName = $("#span_OldDepartName").html();
                var departId = $("#hid_DepartId").val();
                newDepartName = $.trim(newDepartName);
                if (newDepartName == oldDepartName) {
                    reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip3, 'error');
                    return false;
                } else {
                    if (newDepartName && newDepartName != "") {
                        $.ajax({
                            type: "POST",
                            url: TestExistInOther + encodeURI(newDepartName) + "&departId=" + encodeURI(departId),
                            data: "",
                            async: true,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                var jsonMsg = eval("(" + msg + ")");
                                if (jsonMsg.result == "ok") {
                                    $.ajax({
                                        type: "POST",
                                        url: UpdateDepart + "?newDepartName=" + encodeURI(newDepartName) + "&departId=" + encodeURI(departId),
                                        data: "",
                                        async: true,
                                        cache: false,
                                        beforeSend: function (XMLHttpRequest) {

                                        },
                                        success: function (msg) {
                                            var JSONMsg = eval("(" + msg + ")");
                                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                                reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'info');
                                                Create_dlg.dialog('close');
                                                Query();
                                            } else {
                                                reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                            }
                                        },
                                        complete: function (XMLHttpRequest, textStatus) {

                                        },
                                        error: function () {

                                        }
                                    });
                                } else {
                                    reWriteMessagerAlert(Public_Dialog_Title, jsonMsg.message, "error");
                                    return false;
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });
                    } else {
                        reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip4, 'error');
                        return false;
                    }
                }
            }
        }, {
            text: Public_Dialog_Close,
            iconCls: 'icon-cancel',
            handler: function () {
                Update_dlg.dialog('close');
            }
        }],
        title: GroupManagement_JS_DialogTitle3,
        modal: true,
        resizable: true,
        cache: false,
        closed: true,
        left: 50,
        top: 30,
        width: 400,
        height: 150
    });

    dlg_CreateCompany.dialog("close");
    dlg_Create.dialog("close");
    dlg_Update.dialog("close");

    $("#btnQuery").click(function () {
        Query();
    });

    $("#btnAddCompany").click(function () {
        AppendCompany();
    });

    $("#btnAdd").click(function () {
        Append();
    });

    $("#btnUpdate").click(function () {
        Update();
    });

    $("#btnDelete").click(function () {
        Remove();
    });

    $("#btnExpandAll").click(function () {
        expandAll();
    });

    $("#btnCollapseAll").click(function () {
        collapseAll();
    });

    $("#btnExport").click(function () {
        ExportExcel();
    });

    $("#btnPrint").click(function () {
        print();
    });

    $("#divQuery").click(function () {
        Query();
    });

    $("#divAddCompany").click(function () {
        AppendCompany();
    });

    $("#divAdd").click(function () {
        Append();
    });

    $("#divUpdate").click(function () {
        Update();
    });

    $("#divDelete").click(function () {
        Remove();
    });

    $("#divExpand").click(function () {
        expandAll();
    });

    $("#divCollapse").click(function () {
        collapseAll();
    });

    $("#divExport").click(function () {
        ExportExcel();
    });

    $("#divPrint").click(function () {
        print();
    });

    function Query() {
        var node = null;
        if (node) {
            _$_TVDepart.tree('reload', node.target);
        } else {
            _$_TVDepart.tree('reload');
        }
    }

    function AppendCompany() {
        $("#txtCompanyName").val("");
        $("#txtCompanyName").focus();
        CreateCompany_dlg = dlg_CreateCompany.dialog({
            buttons: [{
                text: Public_Dialog_Save,
                iconCls: 'icon-ok',
                handler: function () {
                    var newCompanyName = $("#txtCompanyName").val();
                    newCompanyName = $.trim(newCompanyName);
                    if (newCompanyName && newCompanyName != "") {
                        $.ajax({
                            type: "POST",
                            url: TestExist + encodeURI(newCompanyName),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                var jsonMsg = eval("(" + msg + ")");
                                if (jsonMsg.result == "ok") {
                                    $.ajax({
                                        type: "POST",
                                        url: InsertCompany + encodeURI(newCompanyName),
                                        data: "",
                                        async: true,
                                        cache: false,
                                        beforeSend: function (XMLHttpRequest) {

                                        },
                                        success: function (msg) {
                                            var JSONMsg = eval("(" + msg + ")");
                                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                                reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'info');
                                                CreateCompany_dlg.dialog('close');
                                                Query();
                                            } else {
                                                reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                            }
                                        },
                                        complete: function (XMLHttpRequest, textStatus) {

                                        },
                                        error: function () {

                                        }
                                    });
                                } else {
                                    reWriteMessagerAlert(Public_Dialog_Title, jsonMsg.message, "error");
                                    return false;
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });
                    } else {
                        reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip5, 'error');
                        return false;
                    }
                }
            }, {
                text: Public_Dialog_Close,
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateCompany_dlg.dialog('close');
                }
            }],
            title: GroupManagement_JS_DialogTitle4,
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 150
        });

        dlg_CreateCompany.dialog("open");
    }

    function Append() {
        var node = _$_TVDepart.tree('getSelected');
        if (node) {

        } else {
            reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip6, 'error');
            return false;
        }

        $("#span_ParentDepart_Create").html(node.text);
        $("#hid_ParentDepartId").val(node.id);
        $("#txtSubDepart_Create").val("");

        $("#txtSubDepart_Create").focus();

        Create_dlg = $('#dlg_Create').dialog({
            buttons: [{
                text: Public_Dialog_Save,
                iconCls: 'icon-ok',
                handler: function () {
                    var subDepartName = $("#txtSubDepart_Create").val();
                    var ParentDepartName = $("#span_ParentDepart_Create").html();
                    var ParentDepartId = $("#hid_ParentDepartId").val();
                    subDepartName = $.trim(subDepartName);
                    if (subDepartName && subDepartName != "") {
                        $.ajax({
                            type: "POST",
                            url: TestExist + encodeURI(subDepartName),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                var jsonMsg = eval("(" + msg + ")");
                                if (jsonMsg.result == "ok") {
                                    $.ajax({
                                        type: "POST",
                                        url: InsertDepart + "?departName=" + encodeURI(subDepartName) + "&parentDepartId=" + encodeURI(ParentDepartId),
                                        data: "",
                                        async: false,
                                        cache: false,
                                        beforeSend: function (XMLHttpRequest) {

                                        },
                                        success: function (msg) {
                                            var JSONMsg = eval("(" + msg + ")");
                                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                                reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'info');
                                                Create_dlg.dialog('close');
                                                Query();
                                            } else {
                                                reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                            }
                                        },
                                        complete: function (XMLHttpRequest, textStatus) {

                                        },
                                        error: function () {

                                        }
                                    });
                                } else {
                                    reWriteMessagerAlert(Public_Dialog_Title, jsonMsg.message, "error");
                                    return false;
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });
                    } else {
                        reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip7, 'error');
                        return false;
                    }
                }
            }, {
                text: Public_Dialog_Close,
                iconCls: 'icon-cancel',
                handler: function () {
                    Create_dlg.dialog('close');
                }
            }],
            title: GroupManagement_JS_DialogTitle5,
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 150
        });

        $('#dlg_Create').dialog("open");
    }

    function Update() {
        var node = _$_TVDepart.tree('getSelected');
        if (node) {

        } else {
            reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip8, 'error');
            return false;
        }

        $("#span_OldDepartName").html(node.text);
        $("#hid_DepartId").val(node.id);
        $("#txtNewDepartName").val(node.text);

        $("#txtNewDepartName").focus();

        Update_dlg = $('#dlg_Update').dialog({
            buttons: [{
                text: Public_Dialog_Save,
                iconCls: 'icon-ok',
                handler: function () {
                    var newDepartName = $("#txtNewDepartName").val();
                    var oldDepartName = $("#span_OldDepartName").html();
                    var departId = $("#hid_DepartId").val();
                    newDepartName = $.trim(newDepartName);
                    if (newDepartName == oldDepartName) {
                        reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip9, 'error');
                        return false;
                    } else {
                        if (newDepartName && newDepartName != "") {
                            $.ajax({
                                type: "POST",
                                url: TestExistInOther + encodeURI(newDepartName) + "&departId=" + encodeURI(departId),
                                data: "",
                                async: true,
                                cache: false,
                                beforeSend: function (XMLHttpRequest) {

                                },
                                success: function (msg) {
                                    var jsonMsg = eval("(" + msg + ")");
                                    if (jsonMsg.result == "ok") {
                                        $.ajax({
                                            type: "POST",
                                            url: UpdateDepart + "?newDepartName=" + encodeURI(newDepartName) + "&departId=" + encodeURI(departId),
                                            data: "",
                                            async: true,
                                            cache: false,
                                            beforeSend: function (XMLHttpRequest) {

                                            },
                                            success: function (msg) {
                                                var JSONMsg = eval("(" + msg + ")");
                                                if (JSONMsg.result.toLowerCase() == 'ok') {
                                                    reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'info');
                                                    Update_dlg.dialog('close');
                                                    Query();
                                                } else {
                                                    reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                                }
                                            },
                                            complete: function (XMLHttpRequest, textStatus) {

                                            },
                                            error: function () {

                                            }
                                        });
                                    } else {
                                        reWriteMessagerAlert(Public_Dialog_Title, jsonMsg.message, "error");
                                        return false;
                                    }
                                },
                                complete: function (XMLHttpRequest, textStatus) {

                                },
                                error: function () {

                                }
                            });
                        } else {
                            reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip10, 'error');
                            return false;
                        }
                    }
                }
            }, {
                text: Public_Dialog_Close,
                iconCls: 'icon-cancel',
                handler: function () {
                    Update_dlg.dialog('close');
                }
            }],
            title: GroupManagement_JS_DialogTitle6,
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 150
        });

        $('#dlg_Update').dialog("open");
    }

    function Remove() {
        var node = _$_TVDepart.tree('getSelected');
        if (node) {
            var b = _$_TVDepart.tree('isLeaf', node.target);
            if (!b) {
                reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip11, "操作提示", 'error');
                return false;
            } else {
                reWriteMessagerConfirm(Public_Dialog_Title, GroupManagement_JS_ErrorTip12, function (r) {
                    if (r) {
                        //                        $.ajax({
                        //                            type: "POST",
                        //                            url: IsDepartHasEmployee + encodeURI(node.id),
                        //                            data: "",
                        //                            async: false,
                        //                            cache: false,
                        //                            beforeSend: function (XMLHttpRequest) {

                        //                            },
                        //                            success: function (msg) {
                        //var jsonMsg = eval("(" + msg + ")");
                        //if (jsonMsg.result == "ok") {
                        $.ajax({
                            type: "POST",
                            url: DeleteDepart + encodeURI(node.id),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                var JSONMsg = eval("(" + msg + ")");
                                if (JSONMsg.result.toLowerCase() == 'ok') {
                                    reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'info');
                                    Query();
                                } else {
                                    reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });
                        //} else {
                        //  alert(jsonMsg.message);
                        //reWriteMessagerAlert("提示", jsonMsg.message, "error");
                        //return false;
                        //}
                        //                            },
                        //                            complete: function (XMLHttpRequest, textStatus) {

                        //                            },
                        //                            error: function () {

                        //                            }
                        // });
                    }
                });
            }
        } else {
            reWriteMessagerAlert(Public_Dialog_Title, GroupManagement_JS_ErrorTip13 + '<br>(<font style="color:red;font-weight:bold">' + GroupManagement_JS_ErrorTip14 + '</font>)!', 'error');
            return false;
        }
    }

    function collapseAll() {
        var node = _$_TVDepart.tree('getSelected');
        if (node) {
            _$_TVDepart.tree('collapseAll', node.target);
        } else {
            _$_TVDepart.tree('collapseAll');
        }
    }

    function expandAll() {
        var node = _$_TVDepart.tree('getSelected');
        if (node) {
            _$_TVDepart.tree('expandAll', node.target);
        } else {
            _$_TVDepart.tree('expandAll');
        }
    }
});