<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Computers.aspx.cs" Inherits="Help_Desk.Computers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <link rel="stylesheet" type="text/css" href="CSS/Custom.css" />
    <link rel="stylesheet" type="text/css" href="CSS/JSON.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-modal/0.9.1/jquery.modal.min.css" />
</head>
 
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-modal/0.9.1/jquery.modal.min.js"></script>
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>  
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script> 
     <!-- jQuery Modal -->
    <script type="text/javascript">
        function openModal()
        {
            $('#myModal').modal('show');
        }
    </script>
<body>
    <form id="fmComputers" runat="server">
        <asp:ScriptManager ID="asm" runat="server" />
        <div>
             <asp:Panel ID="pnlComputers" runat="server" Visible="false" CssClass="clear">
                <br />
                <asp:Label id="lblComputers" runat="server" Text="Computers" CssClass="labels" />
                <br />
                <br />

                <asp:GridView ID="gvComputers" runat="server"  AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  GridLines="Both" BorderColor="Black" OnRowCommand="gvComputers_RowCommand" CssClass="Fonts" >
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnComputerEdit" CommandName="ComputerEdit" runat="server"  ImageUrl="~\images\edit-pencil-png.png" Height="30" CommandArgument='<%# Container.DataItemIndex %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnComputerDelete" runat="server" CommandName="ComputerDelete" ImageUrl="~\images\removebutton.png" Height="30" CommandArgument='<%# Container.DataItemIndex %>' />
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
            <div style="clear:both" />
            <div id="OpenModalComputerEdit" class="modalDialog">
                <div class="PopUp">
                    <div class="modal-body">
                        <asp:Label ID="lblComputerEdit" runat="server" CssClass="fonts" Text="Computer Edit" />
                        <asp:HiddenField ID="hfComputerID" runat="server" />
                        <br />
                        <asp:Label ID="lblComputerName" runat="server" Text="Computer" CssClass="labels" />
                        <asp:TextBox ID="tbComputerName" runat="server" CssClass="textbox" OnTextChanged="tbComputerName_TextChanged"/>
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="lblComputerDepartment" runat="server" Text="Department" CssClass="labels" />
                        <asp:DropDownList ID="ddlComputerDepartment" runat="server" OnSelectedIndexChanged="ddlComputerDepartment_SelectedIndexChanged"   CssClass="fonts"/>
                        <br />
                        <br />
                    </div>
                    <div class="modal-footer">
                        <asp:Button id="btnComputerSave" runat="server" Text="Save" OnClick="btnComputerSave_Click" CssClass="fonts" />
                        <asp:Button id="BtnComputerCancel" runat="server" Text="Cancel" OnClick="BtnComputerCancel_Click" cssclass="fonts"/>
                    </div>
                </div>

            </div>
        </div>
   </form>
</body>
</html>
