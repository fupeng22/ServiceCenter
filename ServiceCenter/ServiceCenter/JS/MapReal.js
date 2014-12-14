$(function () {
    var GetAllMapInfoURL = "/" + pub_WhichLang + "/MapDesign/GetAllData?page=1&rows=100";

    var LoadDefaulMapSeleURL = "/" + pub_WhichLang + "/MapReal/LoadDefaultSeleMap";
    var AddDefaultSeleMapURL = "/" + pub_WhichLang + "/MapReal/AddDefaultSeleMap?defaultMapSele=";

    $('#ddlMapSele').combogrid({
        panelWidth: 400,
        idField: 'Id',
        textField: 'MapDes',
        url: GetAllMapInfoURL,
        editable: false,
        mode: "remote",
        //multiple:true,
        pagination: false,
        columns: [[
					{ field: 'Id', title: MapReal_JS_Field1, width: 40 },
                    { field: 'MapDes', title: MapReal_JS_Field2, width: 320 }
				]],
        onSelect: function (rowIndex, rowData) {
            //console.info(rowData);
            $.ajax({
                type: "GET",
                url: AddDefaultSeleMapURL + encodeURI(rowData.Id),
                data: "",
                async: true,
                cache: false,
                beforeSend: function (XMLHttpRequest) {

                },
                success: function (msg) {

                },
                complete: function (XMLHttpRequest, textStatus) {

                },
                error: function () {

                }
            });
            $("#iframeMapPotView").attr("src", "/" + pub_WhichLang + "/MapPortView/Index?MapId=" + encodeURI(rowData.Id));
        },
        onLoadSuccess: function (data) {
            $.ajax({
                type: "GET",
                url: LoadDefaulMapSeleURL,
                data: "",
                async: true,
                cache: false,
                beforeSend: function (XMLHttpRequest) {

                },
                success: function (msg) {
                    $('#ddlMapSele').combogrid('setValue', msg);
                    $("#iframeMapPotView").attr("src", "/" + pub_WhichLang + "/MapPortView/Index?MapId=" + encodeURI(msg));
                },
                complete: function (XMLHttpRequest, textStatus) {

                },
                error: function () {

                }
            });
        }
    });
});