$(function () {
    var _$_datagrid = $("#DG_XRayInfo");
    var _$_ddlNVRId = $("#ddlNVRId");
    var _$_ddlXRayType = $("#ddlXRayType");
    var _$_ddlXRunDirection = $("#ddlXRunDirection");
    var _$_ddlUseFlag = $("#ddlUseFlag");
    var QueryURL = "/" + pub_WhichLang + "/ChannelManagement/GetData";
    var CreateURL = "/" + pub_WhichLang + "/ChannelManagement/AddChannel?XRAY_NVR_ID=";
    var UpdateURL = "/" + pub_WhichLang + "/ChannelManagement/UpdateChannel?XRAY_ID=";
    var DeleteURL = "/" + pub_WhichLang + "/ChannelManagement/Delete";
    var TestExist_XRayName = "/" + pub_WhichLang + "/ChannelManagement/ExistXRayName?strXRayName=";
    var TestExist_XRayName_Update = "/" + pub_WhichLang + "/ChannelManagement/ExistXRayName_Update?XRayId=";
    var CreateDlg = null;
    var CreateDlgForm = null;

    var LoadAllNVRUrl = "/" + pub_WhichLang + "/Default/LoadAllNVR";
    var LoadXRayTypeURL = "/" + pub_WhichLang + "/Default/LoadXRayTypeJSON";
    var LoadXRayDirectionURL = "/" + pub_WhichLang + "/Default/LoadXRayDirectionJSON";
    var LoadXRayUseFlagURL = "/" + pub_WhichLang + "/Default/LoadXRayUseFlagJSON";

    var PrintURL = "";

    _$_datagrid.datagrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'NVR_NAME',
        sortOrder: 'asc',
        remoteSort: true,
        border: false,
        idField: 'XRAY_ID',
        columns: [[
                        { field: 'cb', width: 120, checkbox: true },
    					{ field: 'XRAY_NAME', title: "X光机名称", width: 200, sortable: true,
    					    sorter: function (a, b) {
    					        return (a > b ? 1 : -1);
    					    }
    					},
                        { field: 'NVR_NAME', title: "NVR名称", width: 200, sortable: true,
                            sorter: function (a, b) {
                                return (a > b ? 1 : -1);
                            }
                        },
                        { field: 'XRAY_TYPE_Desc', title: "X光机类型", width: 300, sortable: true,
                            sorter: function (a, b) {
                                return (a > b ? 1 : -1);
                            }
                        },
                        { field: 'XRAY_CHANNEL_NO', title: "通道号", width: 50, sortable: true,
                            sorter: function (a, b) {
                                return (a > b ? 1 : -1);
                            }
                        },
                        { field: 'USE_FLAG_Desc', title: "启用与否", width: 90, sortable: true,
                            sorter: function (a, b) {
                                return (a > b ? 1 : -1);
                            }
                        },
                        { field: 'LastCheckTime', title: "上次检测时间", width: 170, sortable: true,
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
            text: "查询",
            iconCls: 'icon-search',
            handler: function () {
                _$_datagrid.datagrid("reload");
                _$_datagrid.datagrid("unselectAll");
            }
        }, '-', {
            id: 'btnAdd',
            text: "添加",
            iconCls: 'icon-add',
            handler: function () {
                Add();
            }
        }, '-', {
            id: 'btnUpdate',
            text: "修改",
            iconCls: 'icon-edit',
            handler: function () {
                Update();
            }
        }, '-', {
            id: 'btnDelete',
            text: "删除",
            disabled: false,
            iconCls: 'icon-remove',
            handler: function () {
                Delete();
            }
        }, '-', {
            id: 'btnSeleAll',
            text: "全选",
            disabled: false,
            iconCls: 'icon-seleall',
            handler: function () {
                SeleAll();
            }
        }, '-', {
            id: 'btnInverseSele',
            text: "反选",
            disabled: false,
            iconCls: 'icon-inversesele',
            handler: function () {
                InverseSele();
            }
        }, '-', {
            id: 'btnPrint',
            text: "打印",
            disabled: false,
            iconCls: 'icon-print',
            handler: function () {
                Print();
            }
        }, '-', {
            id: 'btnExcel',
            text: "导出",
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
            $('<div  id="mnuQuery" iconCls="icon-search"/>').html("查询").appendTo(cmenu);
            $('<div  id="mnuAdd" iconCls="icon-add"/>').html("添加").appendTo(cmenu);
            $('<div  id="mnuUpdate" iconCls="icon-edit"/>').html("修改").appendTo(cmenu);
            $('<div  id="mnuDelete" iconCls="icon-remove"/>').html("删除").appendTo(cmenu);
            $('<div  id="mnuSeleAll" iconCls="icon-seleall"/>').html("全选").appendTo(cmenu);
            $('<div  id="mnuInverseSele" iconCls="icon-inversesele"/>').html("反选").appendTo(cmenu);
            $('<div  id="mnuPrint" iconCls="icon-print"/>').html("打印").appendTo(cmenu);
            $('<div  id="mnuExcel" iconCls="icon-excel"/>').html("导出").appendTo(cmenu);
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
        PrintURL = "/" + pub_WhichLang + "/ChannelManagement/Print?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
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

        PrintURL = "/" + pub_WhichLang + "/ChannelManagement/Excel?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            window.open(PrintURL);

        } else {
            //reWriteMessagerAlert("提示", "没有数据，不可导出", "error");
            reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForExcel, "error");
            return false;
        }
    }

    function Add() {
        $("#txtXRayName").val("");
        $("#hd_XRayId").val("");
        $("#txtXRayChannelNO").val("");
        $("#txtXBegin").val("");
        $("#txtYBegin").val("");
        $("#txtYEnd").val("");
        $("#txtCriticalValue").val("");
        $("#txtBeginCheckTime").val("");
        $("#txtLastCheckTime").val("");
        $("#txtInterval").val("");

        _$_ddlNVRId.combobox({
            url: LoadAllNVRUrl,
            valueField: 'id',
            textField: 'text',
            editable: false,
            panelHeight: null,
            onLoadSuccess: function () {
                _$_ddlNVRId.combobox("setValue", "-99");
            }
        });

        _$_ddlXRayType.combobox({
            url: LoadXRayTypeURL,
            valueField: 'id',
            textField: 'text',
            editable: false,
            panelHeight: null,
            onLoadSuccess: function () {
                _$_ddlXRayType.combobox("setValue", "-99");
            }
        });

        _$_ddlXRunDirection.combobox({
            url: LoadXRayDirectionURL,
            valueField: 'id',
            textField: 'text',
            editable: false,
            panelHeight: null,
            onLoadSuccess: function () {
                _$_ddlXRunDirection.combobox("setValue", "-99");
            }
        });

        _$_ddlUseFlag.combobox({
            url: LoadXRayUseFlagURL,
            valueField: 'id',
            textField: 'text',
            editable: false,
            panelHeight: null,
            onLoadSuccess: function () {
                _$_ddlUseFlag.combobox("setValue", "-99");
            }
        });

        CreateDlg = $('#dlg_Create_XRayInfo').dialog({
            buttons: [{
                text: Public_Dialog_Save,
                iconCls: 'icon-ok',
                handler: function () {
                    var _XRayName = $("#txtXRayName").val();
                    var _XRayChannelNO = $("#txtXRayChannelNO").val();
                    var _XBegin = $("#txtXBegin").val();
                    var _YBegin = $("#txtYBegin").val();
                    var _YEnd = $("#txtYEnd").val();
                    var _CriticalValue = $("#txtCriticalValue").val();
                    var _BeginCheckTime = $("#txtBeginCheckTime").val();
                    var _LastCheckTime = $("#txtLastCheckTime").val();
                    var _Interval = $("#txtInterval").val();
                    var _NVRId = _$_ddlNVRId.combobox("getValue");
                    var _ddlXRayType = _$_ddlXRayType.combobox("getValue");
                    var _ddlXRunDirection = _$_ddlXRunDirection.combobox("getValue");
                    var _ddlUseFlag = _$_ddlUseFlag.combobox("getValue");

                    if (_XRayName == "" || _XRayChannelNO == "" || _XBegin == "" || _YBegin == "" || _YEnd == "" || _CriticalValue == "" || _BeginCheckTime == "" || _LastCheckTime == "" || _Interval == "" || _NVRId == "-99" || _ddlXRayType == "-99" || _ddlXRunDirection == "-99" || _ddlUseFlag == "-99") {
                        reWriteMessagerAlert(Public_Dialog_Title, "请填写完整信息" + '<br/>(' + "所有信息必须填写" + ')', "error");
                        return false;
                    }

                    var bExist = false;
                    $.ajax({
                        type: "GET",
                        url: TestExist_XRayName + encodeURI(_XRayName),
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
                            url: CreateURL + encodeURI(_NVRId) + "&XRAY_TYPE=" + encodeURI(_ddlXRayType) + "&BEGIN_X=" + encodeURI(_XBegin) + "&BEGIN_Y=" + encodeURI(_YBegin) + "&END_Y=" + encodeURI(_YEnd) + "&RUN_DIRECTOR=" + encodeURI(_ddlXRunDirection) + "&CRITICAL_VALUE=" + encodeURI(_CriticalValue) + "&USE_FLAG=" + encodeURI(_ddlUseFlag) + "&XRAY_CHANNEL_NO=" + encodeURI(_XRayChannelNO) + "&XRAY_NAME=" + encodeURI(_XRayName) + "&BeginCheckTime=" + encodeURI(_BeginCheckTime) + "&LastCheckTime=" + encodeURI(_LastCheckTime) + "&EVERY_INTERNAL=" + encodeURI(_Interval),
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
            title: "添加通道信息",
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 500,
            height: 420
        });

        $('#dlg_Create_XRayInfo').dialog("open");
    }

    function Update() {
        var selects = _$_datagrid.datagrid("getSelections");
        if (selects.length != 1) {
            reWriteMessagerAlert(Public_Dialog_Title, "<center>" + "请选择需要修改的通道信息" + "(<font style='color:red'>" + "每次只可选择一行记录" + "</font>)</center>", "error");
            return false;
        } else {
            $("#txtXRayName").val(selects[0].XRAY_NAME);
            $("#hd_XRayId").val(selects[0].XRAY_ID);
            $("#txtXRayChannelNO").val(selects[0].XRAY_CHANNEL_NO);
            $("#txtXBegin").val(selects[0].BEGIN_X);
            $("#txtYBegin").val(selects[0].BEGIN_Y);
            $("#txtYEnd").val(selects[0].END_Y);
            $("#txtCriticalValue").val(selects[0].CRITICAL_VALUE);
            $("#txtBeginCheckTime").val(selects[0].BeginCheckTime);
            $("#txtLastCheckTime").val(selects[0].LastCheckTime);
            $("#txtInterval").val(selects[0].EVERY_INTERNAL);

            var NVRId_tmp = selects[0].XRAY_NVR_ID;
            var XRayType_tmp = selects[0].XRAY_TYPE;
            var XRunDirection_tmp = selects[0].RUN_DIRECTOR;
            var UseFlag_tmp = selects[0].USE_FLAG;

            _$_ddlNVRId.combobox({
                url: LoadAllNVRUrl,
                valueField: 'id',
                textField: 'text',
                editable: false,
                panelHeight: null,
                onLoadSuccess: function () {
                    _$_ddlNVRId.combobox("setValue", NVRId_tmp);
                }
            });

            _$_ddlXRayType.combobox({
                url: LoadXRayTypeURL,
                valueField: 'id',
                textField: 'text',
                editable: false,
                panelHeight: null,
                onLoadSuccess: function () {
                    _$_ddlXRayType.combobox("setValue", XRayType_tmp);
                }
            });

            _$_ddlXRunDirection.combobox({
                url: LoadXRayDirectionURL,
                valueField: 'id',
                textField: 'text',
                editable: false,
                panelHeight: null,
                onLoadSuccess: function () {
                    _$_ddlXRunDirection.combobox("setValue", XRunDirection_tmp);
                }
            });

            _$_ddlUseFlag.combobox({
                url: LoadXRayUseFlagURL,
                valueField: 'id',
                textField: 'text',
                editable: false,
                panelHeight: null,
                onLoadSuccess: function () {
                    _$_ddlUseFlag.combobox("setValue", UseFlag_tmp);
                }
            });

            CreateDlg = $('#dlg_Create_XRayInfo').dialog({
                buttons: [{
                    text: Public_Dialog_Save,
                    iconCls: 'icon-ok',
                    handler: function () {
                        var _XRayName = $("#txtXRayName").val();
                        var _XRayChannelNO = $("#txtXRayChannelNO").val();
                        var _XBegin = $("#txtXBegin").val();
                        var _YBegin = $("#txtYBegin").val();
                        var _YEnd = $("#txtYEnd").val();
                        var _CriticalValue = $("#txtCriticalValue").val();
                        var _BeginCheckTime = $("#txtBeginCheckTime").val();
                        var _LastCheckTime = $("#txtLastCheckTime").val();
                        var _Interval = $("#txtInterval").val();
                        var _NVRId = _$_ddlNVRId.combobox("getValue");
                        var _ddlXRayType = _$_ddlXRayType.combobox("getValue");
                        var _ddlXRunDirection = _$_ddlXRunDirection.combobox("getValue");
                        var _ddlUseFlag = _$_ddlUseFlag.combobox("getValue");
                        var _XRayId = $("#hd_XRayId").val();

                        if (_XRayId == "" || _XRayName == "" || _XRayChannelNO == "" || _XBegin == "" || _YBegin == "" || _YEnd == "" || _CriticalValue == "" || _BeginCheckTime == "" || _LastCheckTime == "" || _Interval == "" || _NVRId == "-99" || _ddlXRayType == "-99" || _ddlXRunDirection == "-99" || _ddlUseFlag == "-99") {
                            reWriteMessagerAlert(Public_Dialog_Title, "请填写完整信息" + '<br/>(' + "所有信息必须填写" + ')', "error");
                            return false;
                        }

                        //验证此用户名是否已使用
                        var bExist = false;
                        $.ajax({
                            type: "GET",
                            url: TestExist_XRayName_Update + encodeURI(_XRayId) + "&strXRayName=" + encodeURI(_XRayName),
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
                                url: UpdateURL + encodeURI(_XRayId) + "&XRAY_NVR_ID=" + encodeURI(_NVRId) + "&XRAY_TYPE=" + encodeURI(_ddlXRayType) + "&BEGIN_X=" + encodeURI(_XBegin) + "&BEGIN_Y=" + encodeURI(_YBegin) + "&END_Y=" + encodeURI(_YEnd) + "&RUN_DIRECTOR=" + encodeURI(_ddlXRunDirection) + "&CRITICAL_VALUE=" + encodeURI(_CriticalValue) + "&USE_FLAG=" + encodeURI(_ddlUseFlag) + "&XRAY_CHANNEL_NO=" + encodeURI(_XRayChannelNO) + "&XRAY_NAME=" + encodeURI(_XRayName) + "&BeginCheckTime=" + encodeURI(_BeginCheckTime) + "&LastCheckTime=" + encodeURI(_LastCheckTime) + "&EVERY_INTERNAL=" + encodeURI(_Interval),
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
                title: "修改通道信息",
                modal: true,
                resizable: true,
                cache: false,
                left: 50,
                top: 30,
                width: 500,
                height: 420,
                closed: true
            });
            _$_datagrid.datagrid("unselectAll");
        }
        $('#dlg_Create_XRayInfo').dialog("open");
    }

    function Delete() {
        reWriteMessagerConfirm(Public_Dialog_Title, "您确定需要删除所选择的的通道信息吗",
        //reWriteMessagerConfirm(Public_Dialog_Title, User_JS_ErrorMessage5,
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.datagrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids.push(selects[i].XRAY_ID);
                            }
                            if (selects.length == 0) {
                                $.messager.alert(Public_Dialog_Title, "<center>" + "请选择需要删除的通道" + "</center>", "error");
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
                case "xray_name":
                    break;

                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "XRAY_NAME") {

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
