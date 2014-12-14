$(function () {
    var _$_TVGroup = $("#TVGroup");
    var _$_datagrid = $("#DG_MapInfo");
    var QueryGroupURL = "/" + pub_WhichLang + "/GroupManagement/GetData?state=open";
    var QueryMapURL = "/" + pub_WhichLang + "/MapDesign/GetData?page=1&rows=100&GroupId=";

    var MapAdd_dlg = null;
    //返回所选区域ID
    function getGroupSele() {
        return (_$_TVGroup.tree("getSelected") ? _$_TVGroup.tree("getSelected").id : -1);
    }

    function getMapSele() {
        var obj = [4];
        obj[0] = -1;
        obj[1] = -1;
        obj[2] = "";
        obj[3] = "";
        var sele = _$_datagrid.datagrid("getSelected");
        if (sele) {
            obj[0] = sele.GroupId;
            obj[1] = sele.Id;
            obj[2] = sele.MapName;
            obj[3] = sele.MapPath;
        }
        return obj;
    }

    _$_TVGroup.tree({
        url: QueryGroupURL,
        onClick: function (node) {
            //console.info(node.id);
            $("#span_AreaName").html(node.text);
            QueryMapURL = "/" + pub_WhichLang + "/MapDesign/GetData?page=1&rows=100&GroupId=" + node.id;
            window.setTimeout(function () {
                $.extend(_$_datagrid.datagrid("options"), {
                    url: QueryMapURL
                });
                _$_datagrid.datagrid("reload");
            }, 100); //延迟100毫秒执行，时间可以更短
        }
    });

    QueryMapURL = "/" + pub_WhichLang + "/MapDesign/GetData?page=1&rows=100&GroupId=" + getGroupSele();

    _$_datagrid.datagrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryMapURL,
        sortName: 'MapName',
        sortOrder: 'desc',
        remoteSort: true,
        singleSelect: true,
        border: false,
        idField: 'Id',
        columns: [[
					{ field: 'MapName', title: MapDesign_JS_Field1, width: 90, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
                    { field: 'GroupName', title: MapDesign_JS_Field2, width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    }
				]],
        pagination: false,
        pageList: [15, 20, 25, 30, 35, 40, 45, 50],
        onSortColumn: function (sort, order) {
            //_$_datagrid.datagrid("reload");
        },
        onClickRow: function (rowIndex, rowData) {
            $("#iframeMapView").attr("src", "/" + pub_WhichLang + "/MapView/Index?MapId=" + encodeURI(rowData.Id) + "&MapName=" + encodeURI(rowData.MapName) + "&GroupName=" + encodeURI(_$_TVGroup.tree("getSelected").text) + "&MapPath=" + encodeURI(rowData.MapPath));
            $("#span_Area_Detail").html(rowData.GroupName);
            $("#span_MapName").html(rowData.MapName);
        }
    });

    $("#btnAddMap").click(function () {
        var GroupIdSele = getGroupSele();

        if (GroupIdSele == -1) {
            $.messager.alert(Public_Dialog_Title, MapDesign_JS_ErrorTip1, "error");
            return false;
        }

        $("#txtMapName_Add").val("");
        $("#mapFile_Add").val("");

        $("#hid_AreaId_MapAdd").val(GroupIdSele);
        $("#span_AreaName_MapAdd").html((_$_TVGroup.tree("getSelected") ? _$_TVGroup.tree("getSelected").text : ""));

        MapAdd_dlg = $('#div_MapAdd').dialog({
            buttons: [{
                text: Public_Dialog_Save,
                iconCls: 'icon-ok',
                handler: function () {
                    $('#form_MapAdd').form('submit', {
                        url: "/" + pub_WhichLang + "/MapDesign/AddMap",
                        onSubmit: function () {
                            if ($("#txtMapName_Add").val() == "" || $("#mapFile_Add").val() == "") {
                                $.messager.alert(Public_Dialog_Title, MapDesign_JS_ErrorTip2, "error");
                                return false;
                            }
                            var win = $.messager.progress({
                                title: MapDesign_JS_ErrorTip3,
                                msg: MapDesign_JS_ErrorTip4
                            });
                        },
                        success: function (data) {
                            $.messager.progress('close');
                            var msg = eval("(" + data + ")");
                            if (msg.result == "ok") {
                                _$_datagrid.datagrid("reload");
                                MapAdd_dlg.dialog("close");
                                $.messager.alert(Public_Dialog_Title, msg.message, "info");
                            } else {
                                $.messager.alert(Public_Dialog_Title, msg.message, "error");
                                return false;
                            }

                        }
                    });
                }
            }, {
                text: Public_Dialog_Close,
                iconCls: 'icon-cancel',
                handler: function () {
                    MapAdd_dlg.dialog('close');
                }
            }],
            title: MapDesign_JS_AddMap,
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            width: 500,
            height: 230
        });

        $('#div_MapAdd').dialog("open");
    });

    $("#btnUpdateMap").click(function () {
        var GroupIdSele = getGroupSele();
        if (GroupIdSele == -1) {
            $.messager.alert(Public_Dialog_Title, MapDesign_JS_ErrorTip5, "error");
            return false;
        }

        //获取所选择的的地图
        var obj = getMapSele();
        if (obj[1] == -1) {
            $.messager.alert(Public_Dialog_Title, MapDesign_JS_ErrorTip6, "error");
            return false;
        }

        $("#mapFilePath_Old").val("");
        $("#txtMapName_Update").val("")
        $("#mapFile_Update").val("");

        $("#hid_AreaId_MapUpdate").val(GroupIdSele);
        $("#span_AreaName_MapUpdate").html((_$_TVGroup.tree("getSelected") ? _$_TVGroup.tree("getSelected").text : ""));


        $("#hid_MapId_MapUpdate").val(obj[1]);
        $("#txtMapName_Update").val(obj[2]);
        $("#mapFilePath_Old").val(obj[3]);

        MapAdd_dlg = $('#div_MapUpdate').dialog({
            buttons: [{
                text: Public_Dialog_Save,
                iconCls: 'icon-ok',
                handler: function () {
                    $('#form_MapUpdate').form('submit', {
                        url:  "/" + pub_WhichLang + "/MapDesign/UpdateMap",
                        onSubmit: function () {
                            if ($("#txtMapName_Update").val() == "" || ($("#mapFilePath_Old").val() == "" && $("#mapFile_Update").val() == "")) {
                                $.messager.alert(Public_Dialog_Title, MapDesign_JS_ErrorTip7, "error");
                                return false;
                            }
                            var win = $.messager.progress({
                                title: MapDesign_JS_ErrorTip3,
                                msg: MapDesign_JS_ErrorTip4
                            });
                        },
                        success: function (data) {
                            $.messager.progress('close');
                            var msg = eval("(" + data + ")");
                            if (msg.result == "ok") {
                                _$_datagrid.datagrid("reload");
                                MapAdd_dlg.dialog("close");
                                $.messager.alert(Public_Dialog_Title, msg.message, "info");
                            } else {
                                $.messager.alert(Public_Dialog_Title, msg.message, "error");
                                return false;
                            }

                        }
                    });
                }
            }, {
                text: Public_Dialog_Close,
                iconCls: 'icon-cancel',
                handler: function () {
                    MapAdd_dlg.dialog('close');
                }
            }],
            title: MapDesign_JS_UpdateMap,
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            width: 600,
            height: 230
        });

        $('#div_MapUpdate').dialog("open");
    });

    $("#btnClearNewMapFile").click(function () {
        $("#mapFile_Update").val("");
    });

    $("#btnDeleteMap").click(function () {
        var GroupIdSele = getGroupSele();
        if (GroupIdSele == -1) {
            $.messager.alert(Public_Dialog_Title, MapDesign_JS_ErrorTip8, "error");
            return false;
        }

        //获取所选择的的地图
        var obj = getMapSele();
        if (obj[1] == -1) {
            $.messager.alert(Public_Dialog_Title, MapDesign_JS_ErrorTip9, "error");
            return false;
        }

        $.ajax({
            type: "POST",
            url: "/" + pub_WhichLang + "/MapDesign/DeleteMap?Id=" + encodeURI(obj[1]),
            data: "",
            async: true,
            cache: false,
            beforeSend: function (XMLHttpRequest) {

            },
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    $.messager.alert(Public_Dialog_Title, JSONMsg.message, 'info');
                    _$_datagrid.datagrid("reload");
                } else {
                    $.messager.alert(Public_Dialog_Title, JSONMsg.message, "error");
                }
            },
            complete: function (XMLHttpRequest, textStatus) {

            },
            error: function () {

            }
        });
    });
});