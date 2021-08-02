<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThisTicket.aspx.cs" Inherits="Help_Desk.ThisTicket" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="https://cdn.tiny.cloud/1/fmmdlrov3kr8wk93ddb7xo1za9ile9o0vrp6swklhgixzwc0/tinymce/5/tinymce.min.js"></script>
    <script>
        tinymce.init({
        selector: 'textarea',
            plugins: 'paste advlist autolink lists link image charmap print preview hr anchor pagebreak',
            menubar: 'edit',
            toolbar: 'Paste patestext | undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | outdent indent'
        })
    </script>
    <link rel="stylesheet" type="text/css" href="CSS/Custom.css" />
    <title>FBC Dallas Help Desk</title>
</head>
<body class="background">
    <form id="ThisTicket" runat="server" title="ThisTicket">
        <div>
            <asp:ScriptManager ID="asm" runat="server" />
            <asp:Panel ID="pnlTop" runat="server">
                <asp:Label ID="lblTitle" CssClass="mytitle" runat="server" Text="First Baptist Dallas Help Desk" width="100%" />
                <br />
                <asp:Panel ID="pnlMessage" runat="server">
                    <asp:Label ID="lblMessage" runat="server" />
                </asp:Panel>
                <br />
                <asp:Menu ID="menuTicket" runat="server" CssClass="menu" OnMenuItemClick="menuTicket_MenuItemClick" StaticDisplayLevels="2" StaticSubMenuIndent="10" DynamicHorizontalOffset="10" Orientation="Horizontal">

                    <Items>
                        <asp:MenuItem Text="Save Ticket" Value="Save" />
                        <asp:MenuItem Text="|" Selectable="false" />
                        <asp:MenuItem Text="Cancel" Value="Cancel" />
                    </Items>
                </asp:Menu>
            </asp:Panel>
            <asp:Panel ID="pnlTicketInfo" runat="server">
                
                <br />
                <asp:Label ID="lblTicketID" runat="server" Text="Ticket ID" cssclass="labels" />
                <asp:TextBox ID="tbTicketID" runat="server" ReadOnly="true" BackColor="LightGray" CssClass="textbox"/>
                <asp:label ID="lblDateCreated" runat="server" Text="Date Created" CssClass="labels" />
                <asp:TextBox ID="tbDateCreated" runat="server" ReadOnly="true" CssClass="textbox" backcolor="LightGray"/>
                <asp:Label ID="lblCreatedby" runat="server" Text="Created by" CssClass="labels" />
                <asp:Textbox ID="tbCreatedby" runat="server" CssClass="textbox" backcolor="LightGray" />
                <br />
                <br />
                <asp:Label ID ="lblLastUpdate" runat="server" Text="Last Updated" CssClass="labels" />
                <asp:textbox ID="tbLastUpdate" runat="server" readonly="true" CssClass="textbox" BackColor="LightGray" />
                <asp:Label ID ="lblUpdateby" runat="server" Text="Updated by" CssClass="labels"  />
                <asp:TextBox ID="tbupdatedby" runat="server" ReadOnly="true"  CssClass="textbox"  BackColor="LightGray" />
                <br />
                <br />
                <asp:Label ID="lbldateClosed" runat="server" Text="Date Closed" CssClass="labels" />
                <asp:TextBox ID="tbDateClosed" runat="server" ReadOnly="true" CssClass="textbox"  BackColor="LightGray" />   
                <asp:Label ID="lblclosedBy" runat="server" Text="Closed by"  CssClass="labels" />
                <asp:TextBox ID="tbClosedby" runat="server" ReadOnly="true" CssClass="textbox"  BackColor="LightGray" />   
                <br />
                <br />
                <hr runat="server" id="hr3" style="border-color:darkslateblue" />
                <br />
                <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="labels"  />
                <asp:DropDownList ID="ddlTicketStatus" runat="server" CssClass="textbox"  AutoPostBack="true" OnSelectedIndexChanged="ddlTicketStatus_SelectedIndexChanged"/>
                <asp:Label ID="lblAssignedTo" runat="server" Text="Assigned To" CssClass="labels"  />
                <asp:DropDownList ID="ddlTechs" runat="server" CssClass="textbox"  AutoPostBack="true" OnSelectedIndexChanged="ddlTechs_SelectedIndexChanged"/>
                <br />
                <br />
                <asp:label id="lblPersonTicket" runat="server" Text="Person Ticket" CssClass="labels"  />
                <asp:DropDownList ID="ddlTicketPersons" runat="server" CssClass="textbox"  AutoPostBack="true" OnSelectedIndexChanged="ddlTicketPersons_SelectedIndexChanged"/>
                <asp:label id="lblComputer" runat="server" text="Computer" cssclass="labels"  />
                <asp:DropDownList ID="ddlComputers" runat="server"  CssClass="textbox"  AutoPostBack="true" OnSelectedIndexChanged="ddlComputers_SelectedIndexChanged"/>
                <br />
                <br />
                <asp:Label ID ="Category" runat="server" Text="Category" CssClass="labels"  />
                <asp:DropDownList ID="ddlCategory" runat="server"  OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" CssClass="textbox" AutoPostBack="true"/>
                <asp:Label ID ="lblsubCategory" runat="server" Text="SubCategory" CssClass="labels"  />
                <asp:dropdownlist id="ddlSubCategory" runat="server" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged" CssClass="textbox" AutoPostBack="true" />
                <asp:Label ID="lblSelection" runat="server" Text="Selection" CssClass="labels"  />
                <asp:DropDownList ID="ddlSelection" runat="server" CssClass="textbox"  AutoPostBack="true"/>
                <br />
                <br />
                <hr runat="server" id="hr1" style="border-color:darkslateblue" />    
                <br />
                <asp:label ID="lblDescription" runat="server" Text="Description" CssClass="labels"  />         
                <br />
                <h5 style="color:darkblue; font-family:Calibri; font-size:100%"> To Paste images or text press the CTRL-V button</h5>
                <textarea id="taDescription" runat="server" name="taDescription" rows="15" style=" width:85%" onchange="taDescription_TextChanged"></textarea>
                <br />
                <asp:TextBox ID="tbDescriptionHidden" runat="server" Visible="false" TextMode="MultiLine" Height="16px" Width="63px"></asp:TextBox>
            </asp:Panel>
            <hr runat="server" id="hr2" style="border-color:darkslateblue" />
            <asp:panel id="pnlTicketNotes" runat="server">
    
                <asp:Button ID="bnAddNote" runat="server" Text="Save Note"  OnClick="bnAddNote_Click" CssClass="button"/>
                <br />
                <br />
                <br />
                <textarea id="taNewNote" name="taNewNote" rows="15" style="width:80%" runat="server" onchange="taNewNote_textChanged"></textarea>
                <br />
                <asp:Gridview runat="server" ID="gvNotes" AutoGenerateColumns="false" ShowHeader="false" ShowFooter="false" CssClass="fonts" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="Info" HtmlEncode="false" />
                    </Columns>
                </asp:Gridview>
 
                <asp:label ID="lblTechNotespassword" runat="server" Text="Tech Password" cssclass="fonts"/>
                <asp:TextBox ID="tbTechNotespassword" runat="server" cssclass="fonts" TextMode="Password" />
                <asp:Button ID="btnTechNotes" runat="server" Text="OK" CssClass="fonts" OnClick="btnTechNotes_Click" />

                <asp:Panel ID="pnlTechNotes" runat="server" Visible="false">
                    <asp:Button ID="btnSaveTechNotes" runat="server" Text="Save Tech Notes" CssClass="button" OnClick="btnSaveTechNotes_Click" />
                    <br />
                    <textarea id="taTechNotes" runat="server" name="taTechNotes"  rows="15" style=" width:85%"></textarea>
                     <br />
                    <asp:GridView ID="gvTechNotes" runat="server" EmptyDataText="No tech notes found" GridLines="None" AutoGenerateColumns="false" OnRowCommand="gvTechNotes_RowCommand" AlternatingRowStyle-BackColor="MintCream"
                        BackColor="AntiqueWhite" BorderColor="Black" CssClass="Fonts" Width="100%" HeaderStyle-HorizontalAlign="Left">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnTechNoteEdit" CommandName="TechNoteEdit" runat="server"  ImageUrl="~\images\edit-pencil-png.png" Height="30" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnTechNoteDelete" runat="server" CommandName="TechNoteDelete" ImageUrl="~\images\removebutton.png" Height="30" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick="return confirm('Are you sure you want to delete this note?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TechNotesID" headertext ="Tech Notes ID"/>
                            <asp:BoundField DataField="DateCreated" HeaderText ="Date Created" />
                            <asp:BoundField DataField="Tech" HeaderText ="Tech" />
                            <asp:BoundField datafield="TechNotes" HeaderText="Tech Notes" HtmlEncode="false"/>
                         </Columns>
                    </asp:GridView>
                 </asp:Panel>
             </asp:panel>
            <asp:HiddenField ID ="lblIsTech" runat="server" />
            <asp:HiddenField ID ="lblIsAdmin" runat="server" />
            <asp:HiddenField ID="lblNewTicket" runat="server" />
            <asp:HiddenField ID="hfTechNotes" runat="server" />
            <asp:HiddenField ID="lblMyTicketID" runat="server" />
            <asp:HiddenField ID="lbluserID" runat="server" />
            <asp:HiddenField ID="lbluserName" runat="server" />
            <asp:HiddenField ID="lblComputerName" runat="server"/>
            <asp:HiddenField ID="lblComputerID" runat="server" />
            <asp:HiddenField ID ="lblchangeMade" runat="server" />
            <asp:HiddenField ID="lblticketClosed" runat="server" />
            <asp:HiddenField ID="hfReturn" runat="server" />
            <asp:HiddenField ID="hfChanges" runat="server" />
            <asp:HiddenField ID="hfOldNotes" runat="server" />
            <asp:HiddenField ID="hfTechNotesID" runat="server" />
            <asp:HiddenField ID="hfCreatedBy" runat="server" />
            <asp:HiddenField id="hfcreatedid" runat="server" />
            <asp:HiddenField ID="hfClosedbyID" runat="server" />
            <asp:HiddenField ID="hfIsTicketNew" runat="server" />
        </div>
    </form>
</body>
</html>
