<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginAD.aspx.cs" Inherits="Help_Desk.LoginAD" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
    </title>
    <link rel="stylesheet" type="text/css" href="CSS/Custom.css" />
    <link rel="icon" href="Images/help-desk-icon.ico" type="image/x-icon" />
</head>
<body class="background">
    <form id="form1" runat="server" >
        <div>
            <asp:Label ID="lblTitle" runat="server" Text="FBC Dallas Help Desk" CssClass="mytitle" width="100%"/>
            <asp:Label ID="lblSystemInfo" runat="server" CssClass="message" Width="100%" />
            <asp:Panel ID="pnlLogin" runat="server" >
                <asp:Login ID="login1" RememberMeSet="True" runat="server" CssClass="auto-style1" OnAuthenticate="loginad_Authenticate" BackColor="#EFF3FB" BorderColor="#B5C7DE" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" DestinationPageUrl="~/Tickets.aspx" Font-Names="Verdana" Font-Size="15" ForeColor="#333333">
                    <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                    <LoginButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
                    <TextBoxStyle Font-Size="0.8em" />
                    <TitleTextStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
                </asp:Login>
            </asp:Panel>

            <asp:Panel ID="pnlNewUser" runat="server" Visible="false" BorderColor="Black" BorderStyle="Solid" CssClass="" Width="100%">
                <h1>New User Creation</h1>
                <h2>Please complete the following to complete the registration for Helpdesk</h2>
                <br />
                <br />
                <asp:Label ID="lblFirstName" runat="server" Text="First Name:" CssClass="labels"  />
                <asp:TextBox ID="tbFirstName" runat="server" cssclass="textbox" AutoPostBack="true" />
                <br /> 
                <br />
                <asp:Label ID="lblLastName" runat="server" Text="Last Name:" CssClass="labels" />
                <asp:TextBox ID="tbLastName" runat="server" cssclass="textbox" AutoPostBack="true"/>
                <br />
                <br />

                <asp:Label ID ="lblEmailAddress" runat="server" Text="Email Address:" CssClass="labels" />
                <asp:TextBox ID="tbEmailAddress" runat="server" AutoPostBack="true" CssClass="textbox" />
                <br />
                <br />
                <asp:Label ID="lblDepartment" runat="server" Text="Department:" CssClass="labels" />
                <asp:DropDownList ID="ddlDepartment" runat="server" AutoPostBack="true" CssClass="fonts"/>
                <br />
                <br />
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button"/>
            </asp:Panel>


        </div>
        
        <div class="footer">
            <asp:Label ID="GUID" runat="server" Visible="false" />
            <asp:HiddenField id="hfid" runat="server" />
        </div>
    </form>
</body>
</html>
