<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Departments.ascx.cs" Inherits="Help_Desk.Departments" %>
<asp:hiddenfield ID="hfDeparmentID" runat="server" /> 
<asp:Panel ID="pnlDepartment" runat="server">
       <asp:GridView id="gvDepartment" runat="server"  AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite" GridLines="Both" BorderColor="Black" OnRowCommand="gvDepartment_RowCommand" CssClass="fonts">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="btnDepartmentEdit" runat="server" CommandName="DepartmentEdit" ImageUrl="~\Images\edit-pencil-png.png" Height="30" CommandArgument='<%# Container.DataItemIndex %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField >
                <ItemTemplate>
                    <asp:ImageButton ID="btnDepartmentDelete" runat="server" CommandName="DepartmentDelete" ImageUrl="~/Images/RemoveButton.png" OnClientClick="return confirm('Are you sure you want to delete this computer?');" Height="30"  CommandArgument='<%# Container.DataItemIndex %>' />   
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="departmentID" HeaderText="ID" />
            <asp:boundfield DataField="DepartmentName" HeaderText="Department Name" />
        </Columns>
    </asp:GridView>
    <asp:ImageButton ID="ibAddDepartment" runat="server" ImageUrl="~/Images/AddButton.png" OnClick="ibAddDepartment_Click" height="30"/>
</asp:Panel>
<asp:Panel ID="pnlDepartmentEdit" runat="server" Visible="false">
    <asp:Label ID="lblDepartmentName" runat="server" Text="Department:" cssclass="labels" font-name="Calibri" />
    <asp:TextBox ID="tbDepartmentName" runat="server" CssClass="textbox" font-name="Calibri" OnTextChanged="tbDepartmentName_TextChanged"/> 
    <br />
    <br />
    <asp:Button ID="btnDepartmentSave" runat="server" Text="Save" cssclass="button" OnClick="btnDepartmentSave_Click" />
    <asp:Button ID="btnDepartmentCancel" Text="Cancel" runat="server" CssClass="button" OnClick="btnDepartmentCancel_Click" />
</asp:Panel>

