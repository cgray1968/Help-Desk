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
    <asp:Button ID="btnNewUser" runat="server" Text="New User" onclick="btnNewUser_Click" CssClass="button"/>
    <br />
    <br />
    <asp:Label ID="lblFilter" Text="Department:" runat="server" CssClass="labels"/>
    <asp:dropdownlist id="ddlNewDepartment" runat="server" OnSelectedIndexChanged="ddlNewDepartment_SelectedIndexChanged"   AutoPostBack="true" CssClass="textbox" />
    <br />
    <br />
    <asp:Label ID="lblDDLUser" runat="server" Text="Employee:" CssClass="labels"/>
    <asp:DropDownList ID="ddlGetUser" runat="server" OnSelectedIndexChanged="ddlGetUser_SelectedIndexChanged" AutoPostBack="true" CssClass="textbox"/>
</asp:Panel>
<hr />

<asp:Panel ID="pnlUserInfo" runat="server">
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:HelpDeskConnection %>" ProviderName="<%$ ConnectionStrings:HelpDeskConnection.ProviderName %>" SelectCommand="SELECT [Role], [RoleID] FROM [Role]"></asp:SqlDataSource>
    <br />
    <asp:Label ID="lblInfo" runat="server" cssclass="fonts" Text="Person Name" />
    <br />
    <asp:Label ID="lblNewUserFirstName" runat="server" CssClass="labels" Text="First Name" />
    <asp:TextBox ID="tbNewUserFirstName" runat="server" AutoPostBack="true" CssClass="textbox" OnTextChanged="tbNewUserFirstName_TextChanged" />
    <br />
    <br />
    <asp:Label ID="lblNewUserLastName" runat="server" CssClass="labels" Text="Last Name" />
    <asp:TextBox ID="tbNewUserLastName" runat="server" AutoPostBack="true" CssClass="textbox" OnTextChanged="tbNewUserLastName_TextChanged" />
    <br />
    <br />
    <asp:Label ID="lblNewUserEmail" runat="server" CssClass="labels" Text="Email" />
    <asp:TextBox ID="tbNewUserEmail" runat="server" AutoPostBack="true" CssClass="textbox" OnTextChanged="tbNewUserEmail_TextChanged" />
    <br />
    <br />
    <asp:Label ID="lblNewUserDepartment" runat="server" CssClass="labels" Text="Department" />
    <asp:DropDownList ID="ddlNewUserDepartment" runat="server" AutoPostBack="true" CssClass="textbox" OnSelectedIndexChanged="ddlNewUserDepartment_SelectedIndexChanged" />
    <br />
    <br />
    <asp:Label ID="lblNewUserRole" runat="server" CssClass="labels" Text="Role" />
    <asp:DropDownList ID="ddlUserRole" runat="server" AutoPostBack="True" CssClass="textbox"  OnSelectedIndexChanged="ddlUserRole_SelectedIndexChanged" />
    <br />
    <br />
    <asp:Label ID="lblNewUserLogin" runat="server" Text="Login Name" CssClass="labels" />
    <asp:TextBox ID="tbNewUserLogin" runat="server" CssClass="textbox" AutoPostBack="true" ReadOnly="true" />
    <br />
    <br />
    <asp:Button ID="btnSaveNewUser" runat="server" OnClick="btnSaveNewUser_Click" Text="Save New User" CssClass="button" Visible="false" />
    <asp:Button ID="btnCancelNewUser" runat="server" OnClick="btnCancelNewUser_Click" Text="Cancel" CssClass="button" Visible="false" />
    <br />
    <br />
    <br />
    <div class="row">
        <div class="column left">
            <asp:Label ID="lblNewUserTickets" runat="server" CssClass="fonts" text="Tickets" />
            <br />
            <asp:GridView ID="gvNewUserTickets" runat="server" AllowPaging="true" allowsorting="true" AlternatingRowStyle-BackColor="MintCream" AutoGenerateColumns="false" BackColor="AntiqueWhite" BorderColor="Black" CssClass="fonts" EmptyDataText="No Tickets found" GridLines="none" OnPageIndexChanging="gvNewUserTickets_PageIndexChanging" onrowcommand="gvNewUserTickets_RowCommand" PageSize="10" ShowFooter="false" ShowHeader="false" Width="100%">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="hlTicket" runat="server" NavigateUrl='<%# string.Format("~/thisticket.aspx?tid={0}&UserID={1}", 
                                HttpUtility.UrlEncode(Eval("Ticketid").ToString()), 
                                HttpUtility.UrlEncode(Eval("PersonID").ToString())) %>' Text='<%# Eval("TicketID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PersonID" visible="false" />
                    <asp:BoundField DataField="Description" HeaderStyle-Width="85%" HtmlEncode="false" />
                </Columns>
            </asp:GridView>
        </div>
        <div class="column right">
            <asp:Label ID="lblNewUserComputer" runat="server" CssClass="fonts" Text="Computers" />
            <br />
            <asp:GridView ID="gvNewUserComputer" runat="server" AlternatingRowStyle-BackColor="MintCream" AutoGenerateColumns="false" BackColor="AntiqueWhite" BorderColor="Black" CssClass="fonts" EmptyDataText="No Computers Assigned" GridLines="none" OnRowCommand="gvNewUserComputer_RowCommand" ShowHeader="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDeleteUserComputer" runat="server" CommandArgument="<%#Container.DataItemIndex  %>" CommandName="btnDeleteUserComputer_Command" CssClass="fonts" Height="30" ImageUrl="~/Images/RemoveButton.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ComputerID" Visible="false" />
                    <asp:BoundField DataField="ComputerName" ItemStyle-HorizontalAlign="Left" />
                </Columns>
            </asp:GridView>
            <asp:ImageButton ID="btnAddComputer" runat="server" CssClass="labels" height="30" ImageUrl="~/Images/AddButton.png" OnClick="btnAddComputer_Click" Width="30" />
        </div>
    </div>
    <div style="clear:both">
        <asp:Label ID="lblNewRecentActivity" runat="server" CssClass="fonts" Text="Recent Activity" />
        <br />
        <br />
        <asp:GridView ID="gvNewRecentActivity" runat="server" AlternatingRowStyle-BackColor="MintCream" AutoGenerateColumns="false" BackColor="AntiqueWhite" BorderColor="Black" CssClass="fonts" PageSize="5">
            <Columns>
                <asp:BoundField DataField="SecurityLogDate" HeaderText="Date" ItemStyle-Width="20%" />
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