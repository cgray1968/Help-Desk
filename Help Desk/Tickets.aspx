<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="Help_Desk.Tickets" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="CSS/Custom.css" />
    <title>FBC Dallas Help Desk</title>
    </head>
<body class="background">

    <form id="TicketsHome" runat="server" title="Tickets">
        <div>
            <asp:ScriptManager ID="asm" runat="server" />
            <asp:Panel id="PnlTop" runat="server">
                <asp:Label ID="lblTitle" CssClass="mytitle" runat="server" Text="First Baptist Dallas Help Desk" width="100%" />
                <br />
                <asp:Label ID="lblMessage" runat="server" CssClass="message" Width="100%"/>
                <br />
                <br />
                <asp:Menu ID="menuTicket" runat="server" CssClass="menu" OnMenuItemClick="menuTicket_MenuItemClick" StaticDisplayLevels="2" StaticSubMenuIndent="10"  dynamichorizontaloffset="10" orientation="Horizontal"  >
                    <Items>
                        <asp:MenuItem Text="New Ticket" Value="NewTicket"  />
                        <asp:MenuItem Text="|" Value="|1" Selectable="false"/>
                        <asp:MenuItem Text="All Tickets" Value="AssignTicket"></asp:MenuItem>
                        <asp:MenuItem Text="|" Value="|2" Selectable="false" />
                        <asp:MenuItem Text="Admin" Value="Admin" />
                        <asp:MenuItem Text="|" Value ="|3" Selectable="false" />
                        <asp:MenuItem Text="Logoff" Value="logoff"></asp:MenuItem>
                    </Items>
                </asp:Menu>           
                <br />
                <br />
                <div class="row">
                    <div class="column left">
                        <asp:label ID="lblDDLStatus" runat="server" Text="Status:" CssClass="fonts"  />
                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" CssClass="fonts" />
                    </div>
                    <div class="column right">
                        <asp:Label ID="lblDDLTickets" runat="server" Text="Ticket by ID:" cssclass="fonts" />
                        <asp:DropDownList ID ="ddlTickets" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTickets_SelectedIndexChanged" CssClass="fonts" />
                    </div>
                 </div>
                <br />
             <asp:Label ID="lblAdminNotes" runat="server" Visible="false" CssClass="fonts" />
            </asp:Panel>
            <asp:Panel ID="pnlTickets" runat="server" >
                <br />
                <asp:GridView ID="gvTickets" runat="server" AllowSorting="true"  AutoGenerateColumns="false" BackColor="White" 
                    GridLines="both" BorderColor="Black" OnRowCommand="gvTickets_RowCommand" CssClass="Grids" HeaderStyle-BorderStyle="Solid" EmptyDataText="No Tickets Found">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" CommandName="editTicket" runat="server" ImageURL="~/Images/Edit-pencil-png.png" Height="30" CommandArgument='<%# Container.DataItemIndex %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TicketID" HeaderText="Ticket ID"/>
                        <asp:BoundField DataField="DateCreated" HeaderText="Date Created" />
                        <asp:BoundField DataField="createdby" HeaderText="Created by" />
                        <asp:BoundField DataField="TicketFor" HeaderText="Ticket For" />
                        <asp:BoundField DataField="Description" HeaderText="Description" HtmlEncode="false" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="Category" HeaderText="Category" />
                        <asp:BoundField datafield="SubCategory" HeaderText="SubCategory" />
                        <asp:Boundfield datafield="Selection" headerText ="Selection" />
                        <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" />
                        <asp:BoundField DataField="StatusID" ItemStyle-ForeColor="white" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
