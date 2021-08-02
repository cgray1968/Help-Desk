<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsControl.ascx.cs" Inherits="Help_Desk.Settings" %>
<link href="CSS/Custom.css" rel="stylesheet" />
<asp:panel ID="pnlTechPassword" runat="server" BorderStyle="Double" CssClass="column" Width="350px">

    <asp:Label id="lblCurrentPassword" runat="server" Text="Current Password:" cssclass="fonts"/>
    <asp:TextBox ID="tbCurrentPassword" runat="server" ReadOnly="true" cssclass="textbox"/>
    <br />
    <br />
    <br />
    <asp:Label ID="lblNewPassword" runat="server" Text="New Password" CssClass="fonts" />
    <br />
    <asp:TextBox ID="tbNewPassword" runat="server" cssclass="textbox"/>
    <br />
    <br />
    <asp:Button ID="btnSavePassword" runat="server" Text="Save" CssClass="button" OnClick="btnSavePassword_Click" />
    <br />
    <asp:label ID="lblInfo" runat="server" Text="This password is used to open Tech Notes." CssClass="fonts" />
    <br />
</asp:panel>

