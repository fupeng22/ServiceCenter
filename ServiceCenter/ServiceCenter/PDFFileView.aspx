<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PDFFileView.aspx.cs" Inherits="XRayPictureView.PDFFileView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PDF file view</title>
    <script src="FlexPaper/js/jquery.js" type="text/javascript"></script>
    <script src="FlexPaper/js/flexpaper_flash.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="viewerPlaceHolder" style="width: 400px; height: 440px; display: block;">
    </div>
    <script type="text/javascript">
        var fp = new FlexPaperViewer(
        'FlexPaper/FlexPaperViewer',
        'viewerPlaceHolder',
        { config: {
            SwfFile: escape('files/tmp/' + "<%=pub_SWFFileName%>"),
            Scale: 1,
            ZoomTransition: 'easeOut',
            ZoomTime: 1,
            ZoomInterval: 0.4,
            FitPageOnLoad: true,
            FitWidthOnLoad:true,
            PrintEnabled: false,
            FullScreenAsMaxWindow: false,
            ProgressiveLoading: true,
            MinZoomSize: 1,
            MaxZoomSize: 2,
            SearchMatchAll: false,
            InitViewMode: 'Portrait',
            ViewModeToolsVisible: false,
            ZoomToolsVisible: true,
            NavToolsVisible: false,
            CursorToolsVisible: false,
            SearchToolsVisible: false,
            localeChain: 'en_US'
        }
        }
        );
    </script>
    </form>
</body>
</html>
