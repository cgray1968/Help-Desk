<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UsersControl.ascx.cs" Inherits="Help_Desk.UsersControl" %>
<link href="CSS/Custom.css" rel="stylesheet" />
<style type="text/css">
    .auto-style3 {
        margin-bottom: 0px;
    }
</style>
<asp:Label ID="lblMessage" runat="server" Visible="false" cssclass="fonts"/>
<asp:Panel ID="pnlUser" runat="server" CssClass="auto-style3" >
    <asp:HiddenField ID="hfComputerID" runat="server" />
    <asp:Hiddenfield ID="hflNewUserID" runat="server" />
    <asp:HiddenField ID="hfLoginID" runat="server" />
    <asp:HiddenField ID="hfPersonID" runat="server" />
    <asp:HiddenField ID="hfRecordChange" runat="server" />
    <asp:HiddenField ID="hfNewRecord" runat="server" />
    <asp:Label ID="lblFilter" Text="Department:" runat="server" CssClass="labels"/>
    <asp:dropdownlist id="ddlNewDepartment" runat="server" OnSelectedIndexChanged="ddlNewDepartment_SelectedIndexChanged"   AutoPostBack="true" CssClass="textbox" />
    <br />
    <br />
    <asp:Label ID="lblDDLUser" runat="server" Text="Employee:" CssClass="labels"/>
    <asp:DropDownList ID="ddlGetUser" runat="server" OnSelectedIndexChanged="ddlGetUser_SelectedIndexChanged" AutoPostBack="true" CssClass="textbox"/>
</asp:Panel>
<hr />

<asp:Panel ID="pnlUserInfo" runat="server">
    <br />
    <asp:Label ID="lblInfo" runat="server" Text="Person Name" cssclass="fonts"/>
    <br />
    <asp:Label ID="lblNewUserFirstName" runat="server" Text="First Name"  CssClass="labels" />
    <asp:TextBox ID="tbNewUserFirstName" runat="server" CssClass="textbox" OnTextChanged="tbNewUserFirstName_TextChanged" AutoPostBack="true"/>
    <br />
    <br />
    <asp:Label ID="lblNewUserLastName" runat="server" Text="Last Name" CssClass="labels" />
    <asp:TextBox ID="tbNewUserLastName" runat="server" CssClass="textbox" AutoPostBack="true" OnTextChanged="tbNewUserLastName_TextChanged"/>
    <br />
    <br />
    <asp:Label ID="lblNewUserEmail" runat="server" Text="Email" CssClass="labels" />
    <asp:TextBox ID="tbNewUserEmail" runat="server" CssClass="textbox" AutoPostBack="true" OnTextChanged="tbNewUserEmail_TextChanged" />
    <br />
    <br />
    <asp:label id="lblNewUserDepartment" runat="server" Text="Department" CssClass="labels" />
    <asp:DropDownList ID="ddlNewUserDepartment" runat="server" AutoPostBack="true" CssClass="textbox" OnSelectedIndexChanged="ddlNewUserDepartment_SelectedIndexChanged"/>
    <br />
    <br />
    <asp:Label ID="lblNewUserRole" runat="server" Text="Role" CssClass="labels" />
    <asp:DropDownList ID="ddlNewUserRole" runat="server" OnSelectedIndexChanged="ddlNewUserRole_SelectedIndexChanged" CssClass="textbox" AutoPostBack="true" />
    <br />
    <br />
    <asp:Label ID="lblNewUserLogin" runat="server" Text="Login Name" CssClass="labels" />
    <asp:TextBox ID="tbNewUserLogin" runat="server" CssClass="textbox" AutoPostBack="true" ReadOnly="true" />
    <br />
    <br />
    <br />
    <div class="row" >
        <div class="column left" >
            <asp:label id="lblNewUserTickets" runat="server" text="Tickets" CssClass="fonts" />
            <br />
            <asp:GridView ID="gvNewUserTickets" runat="server" EmptyDataText="No Tickets found" allowsorting="true" AllowPaging="true" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite" BorderColor="Black" onrowcommand="gvNewUserTickets_RowCommand" PageSize="10" OnPageIndexChanging="gvNewUserTickets_PageIndexChanging" CssClass="fonts" Width="100%" GridLines="none" ShowHeader="false" ShowFooter="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="hlTicket" runat="server" 
                                NavigateUrl='<%# string.Format("~/thisticket.aspx?tid={0}&UserID={1}", 
                                HttpUtility.UrlEncode(Eval("Ticketid").ToString()), 
                                HttpUtility.UrlEncode(Eval("PersonID").ToString())) %>' 
                                Text='<%# Eval("TicketID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PersonID" visible="false"/>
                    <asp:BoundField DataField="Description" HeaderStyle-Width="85%" HtmlEncode="false"/>
                </Columns>
            </asp:GridView>
        </div>
        <div class="column right">
            <asp:Label ID="lblNewUserComputer" runat="server" Text="Computers" CssClass="fonts" />
            <br />
            <asp:GridView ID="gvNewUserComputer" runat="server" EmptyDataText="No Computers Assigned" AutoGenerateColumns="false" CssClass="fonts" ShowHeader ="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite" BorderColor="Black" GridLines="none" OnRowCommand="gvNewUserComputer_RowCommand" >
                <Columns>
                    
                    <asp:TemplateField >
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDeleteUserComputer" runat="server" CssClass="fonts" Height="30" CommandName="btnDeleteUserComputer_Command" ImageUrl="~/Images/RemoveButton.png" CommandArgument="<%#Container.DataItemIndex  %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ComputerID" Visible="false" />
                    <asp:BoundField DataField="ComputerName" ItemStyle-HorizontalAlign="Left" />
                </Columns>
            </asp:GridView>
            <asp:ImageButton ID="btnAddComputer" runat="server" ImageUrl="~/Images/AddButton.png" OnClick="btnAddComputer_Click" height="30" Width="30" CssClass="labels"/>
        </div>
    </div>
    <div style="clear:both">
        <asp:Label ID="lblNewRecentActivity" runat="server" Text="Recent Activity" CssClass="fonts" />
        <br />
        <br />
        <asp:GridView ID="gvNewRecentActivity" runat="server"  AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite" BorderColor="Black" PageSize="5" CssClass="fonts">
            <Columns>
                <asp:BoundField DataField="SecurityLogDate" HeaderText="Date" ItemStyle-Width="20%"/>
                <asp:BoundField DataField="SecurityInfo" HeaderText="Info" ItemStyle-Width="80%" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Panel>
<asp:Panel ID="pnlSelectComputer" runat="server">
    <asp:label ID="lblSelectComputer" runat="server" Text="Select Computer:" CssClass="fonts" />
    <asp:DropDownList ID="ddlSelectComputer" runat="server" CssClass="fonts" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Button ID="btnOKSelecteComputer" runat="server" onclick="btnOKSelecteComputer_Click" Text="OK" CssClass="button" />
    <asp:Button ID="btnCancelSelectComputer" runat="server" OnClick="btnCancelSelectComputer_Click" Text="Cancel" CssClass="button" />
</asp:Panel>