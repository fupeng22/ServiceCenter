$(function () {
    var LoginURL = "/" + pub_WhichLang + "/Login/Login?strUserName=";
    var DefaultURL = "/" + pub_WhichLang + "/Default/Index";

    $("#btnLogin").click(function () {
        var userName = $("#txtUserName").val();
        var password = $("#txtUserPassword").val();
        if (userName == "" || password == "") {
            $.messager.alert(Public_Dialog_Title, Login_JS_ErrorTip1, "error");
            return false;
        }
        var bOK = false;
        $.ajax({
            type: "POST",
            url: LoginURL + encodeURI(userName) + "&strUserPwd=" + encodeURI(password),
            data: "",
            async: false,
            cache: false,
            beforeSend: function (XMLHttpRequest) {

            },
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    //reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                    bOK = true;
                } else {
                    $.messager.alert(Public_Dialog_Title, JSONMsg.message, 'error');
                }
            },
            complete: function (XMLHttpRequest, textStatus) {

            },
            error: function () {

            }
        });
        if (bOK) {
            if ($("#hidRetURL").val() != "") {
                DefaultURL = $("#hidRetURL").val();
            } else {
            }
            window.location = DefaultURL;
        }
    });

    $("#btnClear").click(function () {
        $("#txtUserName").val("");
        $("#txtUserPassword").val("");
    });

    $(document).keydown(function (event) {
        if (event.keyCode == 13) {
            $("#btnLogin").click();
        }
    });
});