function onDrag(e) {
    var d = e.data;
    if (d.left < 0) { d.left = 0 }
    if (d.top < 0) { d.top = 0 }
    if (d.left + $(d.target).outerWidth() > $(d.parent).width()) {
        d.left = $(d.parent).width() - $(d.target).outerWidth();
    }
    if (d.top + $(d.target).outerHeight() > $(d.parent).height()) {
        d.top = $(d.parent).height() - $(d.target).outerHeight();
    }
}

function onStopDrag(e) {
    var txtMapPortXPos_Add_val = parseFloat($("#span_MapPotSet_" + e.target.id.replace("span_MapPotSet_", "").replace("img_MapPot_", "").replace("chk_mapPortCurSele", "").replace("span_Display", "")).css("left")); // e.pageX;
    var txtMapPortYPos_Add_val = parseFloat($("#span_MapPotSet_" + e.target.id.replace("span_MapPotSet_", "").replace("img_MapPot_", "").replace("chk_mapPortCurSele", "").replace("span_Display", "")).css("top")); //  e.pageY;
    //console.info("Left:" + $("#span_MapPotSet_" + e.target.id.replace("span_MapPotSet_", "").replace("img_MapPot_", "").replace("chk_mapPortCurSele", "").replace("span_Display", "")).css("left") + ",TOP:" + $("#span_MapPotSet_" + e.target.id.replace("span_MapPotSet_", "").replace("img_MapPot_", "").replace("chk_mapPortCurSele", "").replace("span_Display", "")).css("top"));
    //console.info("原来的:X:" + txtMapPortXPos_Add_val + ",Y:" + txtMapPortYPos_Add_val);
    //if (e.target.id.indexOf("img_MapPot_") > -1) {
    //console.info(parseFloat($("#" + e.target.id.replace("img_MapPot_", "span_Display")).css("width")));
    //  txtMapPortXPos_Add_val = txtMapPortXPos_Add_val - (parseFloat($("#" + e.target.id.replace("img_MapPot_", "span_Display")).css("width")) / 2 - parseFloat($("#" + e.target.id).css("width")) / 2);
    //}
    //console.info("后来的的:X:" + txtMapPortXPos_Add_val + ",Y:" + txtMapPortYPos_Add_val);

    var mapPotId = e.target.id.replace("span_MapPotSet_", "").replace("img_MapPot_", "").replace("chk_mapPortCurSele", "").replace("span_Display", "");
    $.ajax({
        type: "GET",
        url: "/" + pub_WhichLang + "/MapView/UpdateMapPot?posX=" + encodeURI(txtMapPortXPos_Add_val) + "&posY=" + encodeURI(txtMapPortYPos_Add_val) + "&MapPotId=" + encodeURI(mapPotId),
        data: "",
        async: true,
        cache: false,
        beforeSend: function (XMLHttpRequest) {

        },
        success: function (msg) {
            var JSONMsg = eval("(" + msg + ")");
            if (JSONMsg.result.toLowerCase() == 'ok') {
                //$.messager.alert('操作提示', JSONMsg.message, 'info');
                //window.location = window.location;
                //ReloadMapPortInfo();
            } else {
                $.messager.alert(Public_Dialog_Title, JSONMsg.message, "error");
            }
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function () {

        }
    });
}

function onStopResize(e) {
    var txtMapPortWidth_val = e.data.width;
    var txtMapPortHeight_val = e.data.height;
    var mapPotId = e.target.id.replace("span_MapPotSet_", "");
    $.ajax({
        type: "GET",
        url: "/" + pub_WhichLang + "/MapView/UpdateMapPotSize?Width=" + encodeURI(txtMapPortWidth_val) + "&Height=" + encodeURI(txtMapPortHeight_val) + "&MapPotId=" + encodeURI(mapPotId),
        data: "",
        async: true,
        cache: false,
        beforeSend: function (XMLHttpRequest) {

        },
        success: function (msg) {
            var JSONMsg = eval("(" + msg + ")");
            if (JSONMsg.result.toLowerCase() == 'ok') {

            } else {
                $.messager.alert(Public_Dialog_Title, JSONMsg.message, "error");
            }
        },
        complete: function (XMLHttpRequest, textStatus) {

        },
        error: function () {

        }
    });
}

$(function () {
    var MapAdd_dlg = null;
    var QueryAvialableEquipmentByMapIdURL = "";
    var imgs = ["iconA", "iconB", "iconC", "iconD", "iconE", "iconF", "iconG", "iconH", "iconI", "iconJ"];

    $("#img_Map").attr("src", "../../" + $("#hid_MapPath").val());
    $("#body_Main").css("background", "url('" + "../../" + $("#hid_MapPath").val() + "') no-repeat");

    setTimeout(function () {
        var width = $("#img_Map").css("width");
        var height = $("#img_Map").css("height");

        $("#img_Map").css("display", "none");
        $("#img_Map").remove();

        $("#body_Main").css("width", width);
        $("#body_Main").css("height", height);
        $("#body_Main").css("left", "0px");
        $("#body_Main").css("top", "0px");

        ReloadMapPortInfo();
    }, 300);

    MapAdd_dlg = $('#div_MapPotAdd').dialog({
        buttons: [{
            text: Public_Dialog_Save,
            iconCls: 'icon-ok',
            handler: function () {

            }
        }, {
            text: Public_Dialog_Close,
            iconCls: 'icon-cancel',
            handler: function () {
                MapAdd_dlg.dialog('close');
            }
        }],
        title: MapView_JS_AddMapPot,
        modal: true,
        resizable: true,
        cache: false,
        closed: true,
        width: 400,
        height: 230
    });

    $('#div_MapPotAdd').dialog("close");

    MapAdd_dlg = $('#div_MapPotUpdate').dialog({
        buttons: [{
            text: Public_Dialog_Save,
            iconCls: 'icon-ok',
            handler: function () {

            }
        }, {
            text: Public_Dialog_Close,
            iconCls: 'icon-cancel',
            handler: function () {
                MapAdd_dlg.dialog('close');
            }
        }],
        title: MapView_JS_UpdateMapPot,
        modal: true,
        resizable: true,
        cache: false,
        closed: true,
        width: 400,
        height: 230
    });

    $('#div_MapPotUpdate').dialog("close");

    $("#mnu_AddMapPort").click(function (e) {
        $("#txtMapPortXPos_Add").val(e.pageX - 15);
        $("#txtMapPortYPos_Add").val(e.pageY - 7);
        $("#txtMapPortWidth_Add").val(30);
        $("#txtMapPortHeight_Add").val(40);
        $("#span_MapNameForMapPort_Add").html($("#hid_MapName").val());
        $("#span_GroupName_Add").html($("#hid_GroupName").val());
        $("#txtMapPortCustomizeName_Add").val("");

        QueryAvialableEquipmentByMapIdURL = "/" + pub_WhichLang + "/MapView/GetAvailableEquipmentByGroupId?hid_MapId=" + encodeURI($("#hid_MapId").val());
        $('#CGEquipmentForMapPort_Add').combogrid({
            panelWidth: 340,
            idField: 'ID',
            textField: 'DeviceName',
            url: QueryAvialableEquipmentByMapIdURL + "&d=" + Date(),
            editable: false,
            mode: "remote",
            panelHeight: 200,
            method: 'get',
            columns: [[
					{ field: 'ID', title: MapView_JS_Field1, width: 40 },
					{ field: 'DeviceName', title: MapView_JS_Field2, width: 270 }
				]]
        });

        MapAdd_dlg = $('#div_MapPotAdd').dialog({
            buttons: [{
                text: Public_Dialog_Save,
                iconCls: 'icon-ok',
                handler: function () {
                    var txtMapPortXPos_Add_val = $("#txtMapPortXPos_Add").val();
                    var txtMapPortYPos_Add_val = $("#txtMapPortYPos_Add").val();
                    var txtMapPortWidth_Add = $("#txtMapPortWidth_Add").val();
                    var txtMapPortHeight_Add = $("#txtMapPortHeight_Add").val();
                    var txtMapPortCustomizeName_Add_val = $("#txtMapPortCustomizeName_Add").val();
                    var CGEquipmentForMapPort_Add_val = $("#CGEquipmentForMapPort_Add").combogrid("getValue");
                    if (txtMapPortXPos_Add_val == "" || txtMapPortYPos_Add_val == "" || txtMapPortCustomizeName_Add_val == "" || CGEquipmentForMapPort_Add_val == "" || txtMapPortWidth_Add == "" || txtMapPortHeight_Add == "") {
                        $.messager.alert(Public_Dialog_Title, MapView_JS_ErrorTip1, "error");
                        return false;
                    }
                    $.ajax({
                        type: "GET",
                        url: "/" + pub_WhichLang + "/MapView/MapPotAdd?MapEquipmentId=" + encodeURI(CGEquipmentForMapPort_Add_val) + "&MapId=" + encodeURI($("#hid_MapId").val()) + "&MapPotName=" + encodeURI(txtMapPortCustomizeName_Add_val) + "&posX=" + encodeURI(txtMapPortXPos_Add_val) + "&posY=" + encodeURI(txtMapPortYPos_Add_val) + "&Width=" + encodeURI(txtMapPortWidth_Add) + "&Height=" + encodeURI(txtMapPortHeight_Add),
                        data: "",
                        async: true,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                $.messager.alert(Public_Dialog_Title, JSONMsg.message, 'info');
                                MapAdd_dlg.dialog('close');
                                //window.location = window.location;
                                ReloadMapPortInfo();
                            } else {
                                $.messager.alert(Public_Dialog_Title, JSONMsg.message, "error");
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {

                        },
                        error: function () {

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
            title: MapView_JS_AddMapPot,
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            width: 500,
            height: 230
        });

        $('#div_MapPotAdd').dialog("open");
    });

    function ReloadMapPortInfo() {
        $("span[customize=1]").remove();
        $.ajax({
            type: "GET",
            url: "/" + pub_WhichLang + "/MapView/LoadMapPotDetail?MapId=" + encodeURI(encodeURI($("#hid_MapId").val())),
            data: "",
            async: true,
            cache: false,
            beforeSend: function (XMLHttpRequest) {

            },
            success: function (msg) {
                var iCurr = 0;
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    var mapPotsInfo = JSONMsg.data;
                    if (mapPotsInfo.length > 0) {
                        for (var i = 0; i < mapPotsInfo.length; i++) {
                            //$("#body_Main").append("<span  name=span_MapPotSet_" + mapPotsInfo[i].Id + " thisId=" + mapPotsInfo[i].Id + " customize='1' id='span_MapPotSet_" + mapPotsInfo[i].Id + "' style='position:absolute;text-align:center;left:" + mapPotsInfo[i].posX + "px;top:" + mapPotsInfo[i].posY + "px;'>" + "<img  class='mp " + imgs[iCurr] + "' id='img_MapPot_" + mapPotsInfo[i].Id + "' src='imgs/transparent.png' />" + "</br><input type='checkbox' curId=" + mapPotsInfo[i].Id + " id='chk_mapPortCurSele" + mapPotsInfo[i].Id + "' MapId=" + mapPotsInfo[i].MapId + " EquipmentId=" + mapPotsInfo[i].EquipmentId + " MapPotName='" + mapPotsInfo[i].MapPotName + "' posX=" + mapPotsInfo[i].posX + " posY='" + mapPotsInfo[i].posY + "' Width='" + mapPotsInfo[i].Width + "' Height='" + mapPotsInfo[i].Height + "' /><br/><span style='color:blue; font-weight:bold'  id='span_Display" + mapPotsInfo[i].Id + "'>" + mapPotsInfo[i].EquipmentName + "</span></span>");
                            //console.info("<span  name=span_MapPotSet_" + mapPotsInfo[i].Id + " thisId=" + mapPotsInfo[i].Id + " customize='1' id='span_MapPotSet_" + mapPotsInfo[i].Id + "' style='position:absolute;text-align:center;left:" + mapPotsInfo[i].posX + "px;top:" + mapPotsInfo[i].posY + "px;'>" + "<img  class='mp " + imgs[iCurr] + "' id='img_MapPot_" + mapPotsInfo[i].Id + "' src='../../images/map/imgs/transparent.png' />" + "</br><input type='checkbox' curId=" + mapPotsInfo[i].Id + " id='chk_mapPortCurSele" + mapPotsInfo[i].Id + "' MapId=" + mapPotsInfo[i].MapId + " EquipmentId=" + mapPotsInfo[i].EquipmentId + " MapPotName='" + mapPotsInfo[i].MapPotName + "' posX=" + mapPotsInfo[i].posX + " posY='" + mapPotsInfo[i].posY + "' Width='" + mapPotsInfo[i].Width + "' Height='" + mapPotsInfo[i].Height + "' /><br/><span style='color:blue; font-weight:bold'  id='span_Display" + mapPotsInfo[i].Id + "'>" + "</span></span>");
                            $("#body_Main").append("<span  name=span_MapPotSet_" + mapPotsInfo[i].Id + " thisId=" + mapPotsInfo[i].Id + " customize='1' id='span_MapPotSet_" + mapPotsInfo[i].Id + "' style='position:absolute;text-align:center;left:" + mapPotsInfo[i].posX + "px;top:" + mapPotsInfo[i].posY + "px;'>" + "<img  class='mp " + imgs[iCurr] + "' id='img_MapPot_" + mapPotsInfo[i].Id + "' src='../../images/map/imgs/transparent.png' />" + "</br><input type='checkbox' curId=" + mapPotsInfo[i].Id + " id='chk_mapPortCurSele" + mapPotsInfo[i].Id + "' MapId=" + mapPotsInfo[i].MapId + " EquipmentId=" + mapPotsInfo[i].EquipmentId + " MapPotName='" + mapPotsInfo[i].MapPotName + "' posX=" + mapPotsInfo[i].posX + " posY='" + mapPotsInfo[i].posY + "' Width='" + mapPotsInfo[i].Width + "' Height='" + mapPotsInfo[i].Height + "' /><br/><span style='color:blue; font-weight:bold'  id='span_Display" + mapPotsInfo[i].Id + "'>" + "</span></span>");
                            $('#span_MapPotSet_' + mapPotsInfo[i].Id).draggable({
                                onDrag: function (e) {
                                    onDrag(e);
                                },
                                onStopDrag: function (e) {
                                    onStopDrag(e);
                                }
                            }).bind('click', function (e) {
                                var currSeleMapPot = e.target.id.replace("span_MapPotSet_", "chk_mapPortCurSele");
                                var allMapPotsChk = $("input[curId]");
                                $.each(allMapPotsChk, function (i, item) {
                                    var thisId = $(item).attr("id");
                                    if (currSeleMapPot == thisId) {
                                        $("#" + thisId).attr("checked", true);
                                    } else {
                                        $("#" + thisId).attr("checked", false);
                                    }
                                });
                            }).bind('dblclick', function (e) {
                                $("#mnu_UpdateMapPort_Detail").click();
                            }).bind('contextmenu', function (e) {
                                e.preventDefault();
                                e.stopPropagation();
                                var currSeleMapPot = e.currentTarget.id.replace("span_MapPotSet_", "chk_mapPortCurSele");
                                var allMapPotsChk = $("input[curId]");
                                $.each(allMapPotsChk, function (i, item) {
                                    var thisId = $(item).attr("id");
                                    if (currSeleMapPot == thisId) {
                                        $("#" + thisId).attr("checked", true);
                                    } else {
                                        $("#" + thisId).attr("checked", false);
                                    }
                                });

                                $('#editMapPortMemu').menu('show', {
                                    left: e.pageX,
                                    top: e.pageY
                                });
                            }).tooltip({
                                content: function () {
                                    var bOK = false;
                                    var strRet = "";

                                    $.ajax({
                                        type: "GET",
                                        url: "/" + pub_WhichLang + "/MapView/LoadMapPotProperty?MapPotId=" + encodeURI(this.id.replace("span_MapPotSet_", "")),
                                        data: "",
                                        async: false,
                                        cache: false,
                                        beforeSend: function (XMLHttpRequest) {

                                        },
                                        success: function (msg) {
                                            var JSONmsg = eval("(" + msg + ")");
                                            //console.info(JSONmsg[0].MapPotName + "---" + JSONmsg[0].EquipmentName);
                                            strRet = "<table><tr><td>" + JSONmsg[0].MapPotName.toString() + "</td></tr><tr><td>" + JSONmsg[0].DeviceName + "</td></tr></table>";
                                            //console.info(strRet);
                                            bOK = true;
                                        },
                                        complete: function (XMLHttpRequest, textStatus) {

                                        },
                                        error: function () {

                                        }
                                    });
                                    return strRet;
                                },
                                trackMouse: true,
                                onShow: function () {
                                    var t = $(this);
                                    t.tooltip('tip').unbind().bind('mouseenter', function () {
                                        t.tooltip('show');
                                    }).bind('mouseleave', function () {
                                        t.tooltip('hide');
                                    });
                                }
                            });

                            iCurr = iCurr + 1;
                            if (iCurr == imgs.length) {
                                iCurr = 0;
                            }
                        }
                    }
                } else {
                    $.messager.alert(Public_Dialog_Title, JSONMsg.message, "error");
                }
            },
            complete: function (XMLHttpRequest, textStatus) {

            },
            error: function () {

            }
        });
    }

    $("#body_Main").dblclick(function (e) {
        var e = e || window.event;
        e.preventDefault();
        $('#menuForPots').menu('show', {
            left: e.pageX,
            top: e.pageY
        });
    });

    $("#mnu_UpdateMapPort_Detail").click(function () {
        var mapPotId = -1;
        var MapId = -1;
        var EquipmentId = -1;
        var MapPotName = "";
        var posX = 0.00;
        var posY = 0.00;
        var Width = 0;
        var Height = 0;
        var bSele = false;
        var allMapPotsChk = $("input[curId]");
        $.each(allMapPotsChk, function (i, item) {
            var checked = $(item).attr("checked");
            if (checked) {
                mapPotId = $(item).attr("curId");
                MapId = $(item).attr("MapId");
                EquipmentId = $(item).attr("EquipmentId");
                MapPotName = $(item).attr("MapPotName");
                posX = $(item).attr("posX");
                posY = $(item).attr("posY");
                Width = $(item).attr("Width");
                Height = $(item).attr("Height");
                bSele = true;
            }
        });
        if (!bSele) {
            $.messager.alert(Public_Dialog_Title, MapView_JS_ErrorTip2, "error");
            return false;
        }

        $("#txtMapPortXPos_Update").val(posX);
        $("#txtMapPortYPos_Update").val(posY);
        $("#txtMapPortWidth_Update").val(Width);
        $("#txtMapPortHeight_Update").val(Height);
        $("#span_MapNameForMapPort_Update").html($("#hid_MapName").val());
        $("#span_GroupName_Update").html($("#hid_GroupName").val());
        $("#txtMapPortCustomizeName_Update").val(MapPotName);
        $("#hid_MapPotIdForUpdate").val(mapPotId);

        QueryAvialableEquipmentByMapIdURL = "/" + pub_WhichLang + "/MapView/GetAvailableEquipmentByGroupId_Update?hid_MapId=" + encodeURI($("#hid_MapId").val()) + "&EquipmentId=" + encodeURI(EquipmentId);
        $('#CGEquipmentForMapPort_Update').combogrid({
            panelWidth: 340,
            idField: 'ID',
            textField: 'DeviceName',
            url: QueryAvialableEquipmentByMapIdURL + "&d=" + Date(),
            editable: false,
            mode: "remote",
            panelHeight: 200,
            method: 'get',
            columns: [[
        					{ field: 'ID', title: MapView_JS_Field1, width: 40 },
        					{ field: 'DeviceName', title: MapView_JS_Field2, width: 270 }
        				]],
            onLoadSuccess: function (data) {
                $('#CGEquipmentForMapPort_Update').combogrid("setValue", EquipmentId);
            }
        });

        MapAdd_dlg = $('#div_MapPotUpdate').dialog({
            buttons: [{
                text: Public_Dialog_Save,
                iconCls: 'icon-ok',
                handler: function () {
                    var mapPotId = $("#hid_MapPotIdForUpdate").val();
                    var txtMapPortXPos_Update_val = $("#txtMapPortXPos_Update").val();
                    var txtMapPortYPos_Update_val = $("#txtMapPortYPos_Update").val();
                    var txtMapPortWidth_Update_val = $("#txtMapPortWidth_Update").val();
                    var txtMapPortHeight_Update_val = $("#txtMapPortHeight_Update").val();
                    var txtMapPortCustomizeName_Update_val = $("#txtMapPortCustomizeName_Update").val();
                    var CGEquipmentForMapPort_Update_val = $("#CGEquipmentForMapPort_Update").combogrid("getValue");
                    if (txtMapPortXPos_Update_val == "" || txtMapPortYPos_Update_val == "" || txtMapPortCustomizeName_Update_val == "" || CGEquipmentForMapPort_Update_val == "" || txtMapPortWidth_Update_val == "" || txtMapPortHeight_Update_val == "") {
                        $.messager.alert(Public_Dialog_Title, MapView_JS_ErrorTip1, "error");
                        return false;
                    }
                    $.ajax({
                        type: "GET",
                        url: "/" + pub_WhichLang + "/MapView/MapPotUpdate?MapEquipmentId=" + encodeURI(CGEquipmentForMapPort_Update_val) + "&MapId=" + encodeURI($("#hid_MapId").val()) + "&MapPotName=" + encodeURI(txtMapPortCustomizeName_Update_val) + "&posX=" + encodeURI(txtMapPortXPos_Update_val) + "&posY=" + encodeURI(txtMapPortYPos_Update_val) + "&Width=" + encodeURI(txtMapPortWidth_Update_val) + "&Height=" + encodeURI(txtMapPortHeight_Update_val) + "&MapPotId=" + encodeURI(mapPotId),
                        data: "",
                        async: true,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'ok') {
                                $.messager.alert(Public_Dialog_Title, JSONMsg.message, 'info');
                                MapAdd_dlg.dialog('close');
                                //window.location = window.location;
                                ReloadMapPortInfo();
                            } else {
                                $.messager.alert(Public_Dialog_Title, JSONMsg.message, "error");
                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {

                        },
                        error: function () {

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
            title: MapView_JS_UpdateMapPot,
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            width: 500,
            height: 230
        });

        $('#div_MapPotUpdate').dialog("open");
    });

    $("#mnu_DeleMapPort_Detail").click(function () {
        var mapPotId = -1;
        var bSele = false;
        var allMapPotsChk = $("input[curId]");
        $.each(allMapPotsChk, function (i, item) {
            var checked = $(item).attr("checked");
            if (checked) {
                mapPotId = $(item).attr("curId");
                bSele = true;
            }
        });
        if (!bSele) {
            $.messager.alert(Public_Dialog_Title, MapView_JS_ErrorTip3, "error");
            return false;
        }

        $.ajax({
            type: "GET",
            url: "/" + pub_WhichLang + "/MapView/MapPotDele?mapPotId=" + encodeURI(mapPotId),
            data: "",
            async: true,
            cache: false,
            beforeSend: function (XMLHttpRequest) {

            },
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    $.messager.alert(Public_Dialog_Title, JSONMsg.message, 'info');
                    //window.location = window.location;
                    ReloadMapPortInfo();
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