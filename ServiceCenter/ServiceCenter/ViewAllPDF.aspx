<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewAllPDF.aspx.cs" Inherits="ServiceCenter.ViewAllPDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:DataList ID="dl_AllPdfIFrame" runat="server" RepeatColumns="3" 
        RepeatLayout="Flow">
        <ItemTemplate>
            <iframe id='ifPDF_ID<%#Eval("PDF_ID")%>' src='PDFFileView.aspx?PDF_ID=<%#Eval("PDF_ID")%>'
                width="410px" height="465px"></iframe>
        </ItemTemplate>
    </asp:DataList>
    </form>
</body>
</html>
