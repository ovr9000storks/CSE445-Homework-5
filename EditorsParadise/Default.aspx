<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EditorsParadise.Default" %>
<%@ OutputCache Duration="180" VaryByParam="*" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" style="border-radius:0; width:100%;">
    <title>Editor's Paradise</title>
    <style type="text/css">
        #PageHeader {
            text-align: center;
            width: 1465px;
        }
        #PageTextArea {
            text-align: center;
            width: 1100px;
        }
        #PageFileUpload {
            text-align: center;
        }
        #PageTextOptions {
            text-align: center;
            width: 1100px;
        }
        #Top10WordsArea {
            text-align: center;
            width: 1100px;
        }
        div{
            background-color:#04293A;
            border-radius:15px;
            margin:auto;
        }
    </style>


    
    <asp:Image ID="PageLogo" runat="server" ImageUrl="~/PageContent/Images/LogoPlaceholder.png" Height="146px" Width="185px" />
    <br /><br /><br /><br />
</head>
<body style="background-color:#041C32; text-align: center; color:azure">
    <form id="form1" runat="server">
        <div id="PageFileUpload" runat="server" style="width: 1100px"><br />
            Select a text file, or paste your plain text in the box below<br /><br />
            <asp:FileUpload ID="InputTextFileUpload" runat="server" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="TextUploadButton" runat="server" OnClick="TextUploadButton_Click" Text="Upload" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="ResetTextButton" runat="server" OnClick="ResetTextButton_Click" Text="Reset text" />
            <br />
            <br />
            <asp:Label ID="LoadedLabel" runat="server" Text="Loaded File:" style="text-decoration: underline"></asp:Label>
&nbsp;<asp:Label ID="LoadedFileLabel" runat="server" Text=" "></asp:Label>
            <br /><br />
        </div>
        <br /><br />
        <div id="PageTextOptions" runat="server"><br />
            Select an operation to perform<br /><br />
            <asp:DropDownList ID="SelectedOptionList" runat="server">
                <asp:ListItem Selected="True" Value="top10">Top 10 Words</asp:ListItem>
                <asp:ListItem Value="replace">Word Replacement</asp:ListItem>
                <asp:ListItem Value="filter">Word Filter</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            <div id="WordReplacementParameters" runat="server">
                Replace this:&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="TextToReplace" runat="server" Width="200px"></asp:TextBox>
                <br />
                With this:&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="ReplacementText" runat="server" Width="200px"></asp:TextBox>
                <br /><br />
            </div>
            <asp:Button ID="ActionButton" runat="server" Text="Ship it!" OnClick="ActionButton_Click" />
            <br /><br />
        </div>
        <br /><br />
        <div id="PageTextArea" runat="server">
            <div id="Top10WordsArea" runat="server" visible="false"><br />
                Top 10 words<br />
                <br />
                <textarea id="Top10WordsOut" runat="server" cols="60" name="Top10WordsOut" rows="10"></textarea><br />
            </div>
            <br />Displayed text:<br /><br />
            <textarea id="MainTextArea" runat="server" cols="120" name="MainTextArea" rows="25"></textarea><br /><br />
        </div>
        <textarea id="SavedText" runat="server" hidden="hidden" ></textarea>
        <textbox id="SavedFileName" runat="server" hidden="hidden"></textbox>
        <br /><br />
    </form>
</body>
</html>
