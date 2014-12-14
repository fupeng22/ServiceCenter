$(function () {
    var MapAdd_dlg = null;
    var imgs = ["iconA", "iconB", "iconC", "iconD", "iconE", "iconF", "iconG", "iconH", "iconI", "iconJ"];

    var QueryStatisticByHourURL = "/" + pub_WhichLang + "/Query_StatisticByHour/Index?MapPotId=";
    var QueryStatisticByDayURL = "/" + pub_WhichLang + "/Query_StatisticByDay/Index?MapPotId=";
    var QueryStatisticByMonthURL = "/" + pub_WhichLang + "/Query_StatisticByMonth/Index?MapPotId=";

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
                            //                            if ($("#hid_MapPotId").val() == mapPotsInfo[i].Id) {
                            //                                $("#body_Main").append("<span  name=span_MapPotSet_" + mapPotsInfo[i].Id + " thisId=" + mapPotsInfo[i].Id + " customize='1' id='span_MapPotSet_" + mapPotsInfo[i].Id + "' style='position:absolute;text-align:center;left:" + mapPotsInfo[i].posX + "px;top:" + mapPotsInfo[i].posY + "px;'>" + "<img  class='mpCur iconMpCur' src='imgs/transparent.png' />" + "</br><span style='color:Red; font-weight:bold' id='span_Display" + mapPotsInfo[i].Id + "'>" + "</span></span>");
                            //                            } else {
                            //                                $("#body_Main").append("<span  name=span_MapPotSet_" + mapPotsInfo[i].Id + " thisId=" + mapPotsInfo[i].Id + " customize='1' id='span_MapPotSet_" + mapPotsInfo[i].Id + "' style='position:absolute;text-align:center;left:" + mapPotsInfo[i].posX + "px;top:" + mapPotsInfo[i].posY + "px;'>" + "<img  class='mp " + imgs[iCurr] + "' src='imgs/transparent.png' />" + "</br><span style='color:blue; font-weight:bold' id='span_Display" + mapPotsInfo[i].Id + "'>" + "</span></span>");
                            //                            }
                            //console.info("<span  name=span_MapPotSet_" + mapPotsInfo[i].Id + " thisId=" + mapPotsInfo[i].Id + " customize='1' id='span_MapPotSet_" + mapPotsInfo[i].Id + "' style='position:absolute;text-align:center;left:" + mapPotsInfo[i].posX + "px;top:" + mapPotsInfo[i].posY + "px;'>" + "<img class='mp " + imgs[iCurr] + "' src='../../images/map/imgs/transparent.png' id='imgAlarmPot_" + mapPotsInfo[i].Id + "' normalCls='mp " + imgs[iCurr] + "' ReceiveMsgId=0/>" + "</br><span style='color:blue; font-weight:bold' id='span_Display" + mapPotsInfo[i].Id + "'>" + "</span></span>");
                            $("#body_Main").append("<span  name=span_MapPotSet_" + mapPotsInfo[i].Id + " thisId=" + mapPotsInfo[i].Id + " customize='1' id='span_MapPotSet_" + mapPotsInfo[i].Id + "' style='position:absolute;text-align:center;left:" + mapPotsInfo[i].posX + "px;top:" + mapPotsInfo[i].posY + "px;'>" + "<img class='mp " + imgs[iCurr] + "' src='../../images/map/imgs/transparent.png' id='imgAlarmPot_" + mapPotsInfo[i].Id + "' normalCls='mp " + imgs[iCurr] + "' ReceiveMsgId=0/>" + "</br><span style='color:blue; font-weight:bold' id='span_Display" + mapPotsInfo[i].Id + "'>" + "</span></span>");
                            //$("#body_Main").append("<span  name=span_MapPotSet_" + mapPotsInfo[i].Id + " thisId=" + mapPotsInfo[i].Id + " customize='1' id='span_MapPotSet_" + mapPotsInfo[i].Id + "' style='position:absolute;left:" + mapPotsInfo[i].posX + "px;top:" + mapPotsInfo[i].posY + "px;'>" + "<img style='width:19px;height:29px;cursor: pointer;background: transparent url(\"../../images/map/imgs/curMapPot.png\") repeat scroll -24px -230px;'  src='../../images/map/imgs/transparent.png' class='mp " + imgs[iCurr] + "' id='imgAlarmPot_" + mapPotsInfo[i].Id + "'/>" + "</br><span style='color:blue; font-weight:bold' id='span_Display" + mapPotsInfo[i].Id + "'>" + "</span></span>");
                            $('#span_MapPotSet_' + mapPotsInfo[i].Id).bind('click', function (e) {
                                //                                var currSeleMapPot = e.target.id.replace("span_MapPotSet_", "chk_mapPortCurSele");
                                //                                var allMapPotsChk = $("input[curId]");
                                //                                $.each(allMapPotsChk, function (i, item) {
                                //                                    var thisId = $(item).attr("id");
                                //                                    if (currSeleMapPot == thisId) {
                                //                                        $("#" + thisId).attr("checked", true);
                                //                                    } else {
                                //                                        $("#" + thisId).attr("checked", false);
                                //                                    }
                                //                                });




                                var currSeleMapPot = e.currentTarget.id.replace("span_MapPotSet_", "");
                                var cmenu = $('<div id="cmenu" style="width:200px;"></div>').appendTo('body');
                                $('<div  id="mnuQueryByHour' + currSeleMapPot + '" iconCls="icon-search"/>').html(MapPortView_JS_Menu1).appendTo(cmenu);
                                $('<div  id="mnuQueryByDay' + currSeleMapPot + '"  iconCls="icon-search"/>').html(MapPortView_JS_Menu2).appendTo(cmenu);
                                $('<div  id="mnuQueryByMonth' + currSeleMapPot + '" iconCls="icon-search"/>').html(MapPortView_JS_Menu3).appendTo(cmenu);
                                cmenu.menu({
                                    onClick: function (item) {
                                        cmenu.remove();
                                        //                                        switch (item.id.toLowerCase()) {
                                        //                                            case "mnuquerybyhour":
                                        //                                                console.info(item);
                                        //                                                break;
                                        //                                            case "mnuquerybyday":
                                        //                                                console.info(item);
                                        //                                                break;
                                        //                                            case "mnuquerybymonth":
                                        //                                                console.info(item);
                                        //                                                break;
                                        //                                        }
                                        //console.info(item.id);
                                        var ItemId = item.id;
                                        var _MapPotId = -1;
                                        if (ItemId.indexOf("mnuQueryByHour") != -1) {
                                            _MapPotId = ItemId.replace("mnuQueryByHour", "");
                                            OpenQuery(QueryStatisticByHourURL + encodeURI(_MapPotId), MapPortView_JS_Menu1);
                                        } else if (ItemId.indexOf("mnuQueryByDay") != -1) {
                                            _MapPotId = ItemId.replace("mnuQueryByDay", "");
                                            OpenQuery(QueryStatisticByDayURL + encodeURI(_MapPotId), MapPortView_JS_Menu2);
                                        } else if (ItemId.indexOf("mnuQueryByMonth") != -1) {
                                            _MapPotId = ItemId.replace("mnuQueryByMonth", "");
                                            OpenQuery(QueryStatisticByMonthURL + encodeURI(_MapPotId), MapPortView_JS_Menu3);
                                        }
                                    }
                                });

                                $('#cmenu').menu('show', {
                                    left: e.pageX,
                                    top: e.pageY
                                });
                            }).bind('contextmenu', function (e) {
                                e.preventDefault();
                                e.stopPropagation();

                                //                                var currSeleMapPot = e.currentTarget.id.replace("span_MapPotSet_", "");
                                //                                var cmenu = $('<div id="cmenu" style="width:100px;"></div>').appendTo('body');
                                //                                $('<div  id="mnuQueryByHour' + currSeleMapPot + '" iconCls="icon-search"/>').html("按小时统计查询").appendTo(cmenu);
                                //                                $('<div  id="mnuQueryByDay' + currSeleMapPot + '"  iconCls="icon-print"/>').html("按天统计查询").appendTo(cmenu);
                                //                                $('<div  id="mnuQueryByMonth' + currSeleMapPot + '" iconCls="icon-excel"/>').html("按月统计查询").appendTo(cmenu);
                                //                                cmenu.menu({
                                //                                    onClick: function (item) {
                                //                                        cmenu.remove();
                                //                                        //                                        switch (item.id.toLowerCase()) {
                                //                                        //                                            case "mnuquerybyhour":
                                //                                        //                                                console.info(item);
                                //                                        //                                                break;
                                //                                        //                                            case "mnuquerybyday":
                                //                                        //                                                console.info(item);
                                //                                        //                                                break;
                                //                                        //                                            case "mnuquerybymonth":
                                //                                        //                                                console.info(item);
                                //                                        //                                                break;
                                //                                        //                                        }
                                //                                        console.info(item.id);
                                //                                    }
                                //                                });

                                //                                $('#cmenu').menu('show', {
                                //                                    left: e.pageX,
                                //                                    top: e.pageY
                                //                                });
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

    function OpenQuery(Url, dlgTitle) {
        var div_PrintDlg = self.parent.parent.$("#dlg_GlobalPrint");
        div_PrintDlg.show();
        var PrintDlg = null;
        div_PrintDlg.find("#frmPrintURL").attr("src", Url);
        PrintDlg = div_PrintDlg.window({
            title: dlgTitle,
            href: "",
            modal: true,
            resizable: true,
            minimizable: false,
            collapsible: false,
            cache: false,
            closed: true,
            width: 800,
            height: 600
        });
        div_PrintDlg.window("open");
    }
});