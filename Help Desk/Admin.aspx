<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Help_Desk.Admin"  ValidateRequest="false"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/Departments.ascx" TagPrefix="uc" TagName="Departments"  %>
<%@ Register Src="~/ComputerControl.ascx" TagPrefix="uc" TagName="Computers" %>
<%@ Register Src="~/CategoryControl.ascx" TagPrefix="uc" TagName="Category" %>
<%@ Register Src="~/UsersControl.ascx" TagPrefix="uc" TagName="Users" %>
<%@ Register Src="~/MessagesControl.ascx" TagPrefix="uc" TagName="Messages" %>
<%@ Register Src="~/SystemInfo.ascx" TagPrefix="uc" TagName="SystemInfo" %>
<%@ Register Src="~/SettingsControl.ascx" TagPrefix="uc" TagName="Settings" %>
<%@ Register Src="~/ReportsControl.ascx" TagPrefix="uc" TagName="Reports" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <link rel="stylesheet" type="text/css" href="CSS/Custom.css" />
        <link rel="styelsheet" type="text/css" href="CSS/json.css" />
        <title>FBC Dallas Help Desk</title>
    </head>
    <body class="background">
        <form id="AdminPage" runat="server" title="Admin Page">
            <div>
                <asp:HiddenField ID="hfPersonID" runat="server" />
                <asp:HiddenField ID="hfLoginID" runat="server" />
                <asp:ScriptManager ID="asm" runat="server" />
                <asp:Panel ID="pnlTop" runat="server">
                   <asp:Label ID="lblTitle" CssClass="mytitle" runat="server" Text="First Baptist Dallas Help Desk" width="100%" />
                   <div class="divBackground"></div>
                <asp:Menu ID="menuAdmin" runat="server"  OnMenuItemClick="menuAdmin_MenuItemClick" CssClass="menu" orientation="horizontal" ForeColor="IndianRed"  >
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle BackColor="LightGray" ForeColor="IndianRed" />
                    </LevelSubMenuStyles>
                    <Items>
                        <asp:MenuItem Text="Return" Value="Return"/>
                        <asp:MenuItem Text="|" Selectable="false"/>
                        <asp:MenuItem Text="Users" Value="Users">
                        </asp:MenuItem>
                        <asp:MenuItem Text="|" Selected="false" />
                        <asp:MenuItem Text="System Info">
                            <asp:MenuItem Text="System Info" />
                            <asp:MenuItem Text="Messages" />
                        </asp:MenuItem>
                        <asp:MenuItem Text="|" Selectable="false" />
                        <asp:MenuItem Text="Department">
                        </asp:MenuItem>
                        <asp:MenuItem Text="|" Selectable="false" />
                        <asp:MenuItem Text="Computers"  >
                        </asp:MenuItem>
                        <asp:MenuItem Text="|" Selectable="false" />
                        <asp:MenuItem Text="Categories">
                        </asp:MenuItem>
                        <asp:MenuItem Text="|" Selectable="false" />
                        <asp:MenuItem Text ="Settings">
                        </asp:MenuItem>
                        <asp:MenuItem Text="|" Selectable="false" />
                        <asp:MenuItem Text ="Reports">
                        </asp:MenuItem>
                    </Items>
                </asp:Menu>           
                <asp:Label ID="lblMessage" runat="server" />
                <br />
                </asp:Panel>

                <asp:Panel ID="pnlComputers" runat="server">
                    <uc:Computers ID="ComputerControl" runat="server" />
                </asp:Panel>

                <asp:Panel ID="pnlUser" runat="server">
                    <uc:Users id="Usercontrol" runat="server" />
                </asp:Panel>
    
                <asp:Panel ID="pnlSystemInfo" runat="server">
                    <uc:Messages id="Systeminfo" runat="server" />
                </asp:Panel>

                <asp:Panel ID="pnlCategory" runat="server">
                    <uc:category id="Category" runat="server" />
                </asp:Panel>

                <asp:Panel ID="pnlDepartment" runat="server" Visible="false">
                   <uc:Departments id="DepartmentControl"  runat="server" />
                </asp:Panel>
    
                <asp:Panel ID="pnlMessages" runat="server" Visible="false">
                    <uc:Messages ID="MessagesControl" runat="server"/>
                </asp:Panel>

                <asp:Panel ID="pnlSettings" runat="server" Visible="false">
                    <uc:Settings id="SettingsControl" runat="server" />
                </asp:Panel>

                <asp:Panel ID="pnlReport" runat="server" Visible="false">
                    <uc:Reports id="ReprotsControl" runat="server" />
                </asp:Panel>

            </div>


        </form>
    </body>
</html>

