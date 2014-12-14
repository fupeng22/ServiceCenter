<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewAllPic_New.aspx.cs"
    Inherits="ServiceCenter.ViewAllPic_New" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:DataList ID="dl_AllPicIFrame" runat="server" RepeatColumns="3" RepeatLayout="Flow">
        <ItemTemplate>
            <iframe id='ifPic_ID<%#Eval("PIC_ID")%>' src='PicFileView_New.aspx?PIC_ID=<%#Eval("PIC_ID")%>'
                width="410px" height="465px"></iframe>
        </ItemTemplate>
    </asp:DataList>
    </form>
</body>
</html>
