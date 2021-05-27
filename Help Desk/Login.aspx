<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Help_Desk.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="CSS/Custom.css" />
</head>
<body class="background">
    <form id="form1" runat="server" >
        <div>
            <asp:Label ID="lblTitle" runat="server" Text="FBC Dallas Help Desk" CssClass="mytitle" width="100%"/>
            <asp:Label ID="lblSystemInfo" runat="server" CssClass="message" Width="100%" />
            <asp:Panel ID="pnlLogin" runat="server" >
                <asp:Login ID="login1" RememberMeSet="false"  runat="server" CssClass="auto-style1" OnAuthenticate="login1_Authenticate" BackColor="#EFF3FB" BorderColor="#B5C7DE" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" DestinationPageUrl="~/Tickets.aspx" Font-Names="Verdana" Font-Size="15" ForeColor="#333333">
                    <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                    <LoginButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
                    <TextBoxStyle Font-Size="0.8em" />
                    <TitleTextStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
                </asp:Login>
            </asp:Panel>

            <asp:Panel ID="pnlChangePassword" runat="server" Visible="false" BorderColor="Black" BorderStyle="Solid" CssClass="ChangePassword" Width="300px" >
                
                <asp:Label id="lblHeader1" runat="server" Text="Change Password" cssclass="ChangePasswordHeader" Width="100%"/>
                <br />
                <br />
                <asp:Label ID="lblNewPassword" runat="server" Text="New Password" CssClass="ChangePasswordLabel" Width="120px" />
                <asp:TextBox ID="tbNewPassword" runat="server" TextMode="Password" width="100px"/>
                <br />
                <asp:Label ID="lblNewPasswordConfirm" runat="server" Text="Confirm Password" CssClass="ChangePasswordLabel" Width="120px"/>
                <asp:TextBox ID="tbNewPasswordConfirm" runat="server" width="100px" TextMode="Password" />
                <br />
                <br />
                <asp:Button ID="btnChangePassword" runat="server" OnClick="btnChangePassword_Click" text="Change Password" CssClass="ChangePasswordButton" />
                <br />                
                <asp:Label ID="lblChangePasswordError" runat="server" />
            </asp:Panel> 
        </div>
        
        <div class="footer">
            <asp:Label ID="GUID" runat="server" Visible="false" />
            <asp:HiddenField id="hfid" runat="server" />
        </div>
    </form>
</body>
</html>
