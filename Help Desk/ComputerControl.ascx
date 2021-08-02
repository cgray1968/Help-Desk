<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComputerControl.ascx.cs" Inherits="Help_Desk.ComputerControl" %>
    <link href="CSS/Custom.css" rel="stylesheet" />
    <asp:Panel ID="pnlComputers" runat="server"  CssClass="clear">
        <asp:HiddenField ID="hfComputerUserID" runat="server" />
        <asp:Label id="lblComputers" runat="server" Text="Computers" CssClass="fonts" />
        <br />
        <asp:GridView ID="gvComputers" runat="server"  AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  GridLines="Both" BorderColor="Black" OnRowCommand="gvComputers_RowCommand" CssClass="Fonts" AllowSorting="true" Width="100px">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnComputerEdit" CommandName="ComputerEdit" runat="server"  ImageUrl="~\images\edit-pencil-png.png" Height="30" CommandArgument='<%# Container.DataItemIndex %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnComputerDelete" runat="server" CommandName="ComputerDelete" ImageUrl="~\images\removebutton.png" Height="30" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this computer?');" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ComputerID" Visible="true" />
                <asp:BoundField datafield="ComputerName" HeaderText="Computer Name" />
                <asp:BoundField DataField="DepartmentID" Visible="false" />
                <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                <asp:BoundField DataField="Personid" Visible="false" />
                <asp:BoundField DataField="UserName" HeaderText="User Name" />
            </Columns>
        </asp:GridView>
        <asp:ImageButton ID="btnAddNewComputer" runat="server" ImageUrl="~/Images/AddButton.png" OnClick="btnAddNewComputer_Click"  height="30" Width="30" CssClass="labels"/>
        <br />
    </asp:Panel>
    <asp:panel id="pnlComputerEdit" runat="server" Visible="false">
        <asp:Label ID="lblComputerEdit" runat="server" CssClass="fonts" Text="Computer Edit" />
        <asp:HiddenField ID="hfComputerID" runat="server" />
        <br />
        <asp:Label ID="lblComputerName" runat="server" Text="Computer" CssClass="labels" />
        <asp:TextBox ID="tbComputerName" runat="server" CssClass="textbox" OnTextChanged="tbComputerName_TextChanged" AutoPostBack="true"/>
        <br />
        <br />
        <br />
        <asp:Label ID="lblComputerDepartment" runat="server" Text="Department" CssClass="labels" />
        <asp:DropDownList ID="ddlDepartments" runat="server" OnSelectedIndexChanged="ddlDepartments_SelectedIndexChanged" AutoPostBack="true" CssClass="fonts"/>
        <br />
        <br />
        <asp:Button id="btnComputerSave" runat="server" Text="Save" OnClick="btnComputerSave_Click" CssClass="button"/>
        <asp:Button id="BtnComputerCancel" runat="server" Text="Cancel" OnClick="BtnComputerCancel_Click" cssclass="button"/>
    </asp:panel>
    <asp:Panel ID="pnlComputerUsers" runat="server" Visible="false">
        <asp:Label ID="lblComputerUSers" runat="server" Text="Users" CssClass="fonts"/>
        <br />
        <asp:GridView ID="gvComputersUsers" runat="server" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  GridLines="None" BorderColor="Black" OnRowCommand="gvComputersUsers_RowCommand" EmptyDataText="No Users">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton id="btnDeleteUser" runat="server" CommandName="DeleteUser" ImageUrl="~\images\removebutton.png" Height="30" OnClientClick="return confirm('Are you sure you want to delete this user from the computer?');" CommandArgument='<%# Container.DataItemIndex %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ComputerUserId" Visible="false" />
                <asp:BoundField DataField="PersonName" HeaderText="Person Name" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
