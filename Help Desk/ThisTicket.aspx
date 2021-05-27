<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ThisTicket.aspx.cs" Inherits="Help_Desk.ThisTicket" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="https://cdn.tiny.cloud/1/fmmdlrov3kr8wk93ddb7xo1za9ile9o0vrp6swklhgixzwc0/tinymce/5/tinymce.min.js"></script>
    <link rel="stylesheet" type="text/css" href="CSS/Custom.css" />
    <title>FBC Dallas Help Desk</title>
</head>
<body class="background">
    <form id="ThisTicket" runat="server" title="ThisTicket">
        <div>
            <asp:ScriptManager ID="asm" runat="server" />
            <asp:Panel ID="pnlTop" runat="server">
                  <asp:Label ID="lblTitle" CssClass="mytitle" runat="server" Text="First Baptist Dallas Help Desk" width="100%" />
                <asp:Menu ID="menuTicket" runat="server" CssClass="menu" OnMenuItemClick="menuTicket_MenuItemClick" StaticDisplayLevels="2" StaticSubMenuIndent="10" DynamicHorizontalOffset="10" Orientation="Horizontal" ForeColor="IndianRed">
                    <Items>
                        <asp:MenuItem Text="Save" Value="Save" />
                        <asp:MenuItem Text="Cancel" Value="Cancel" />
                    </Items>
                </asp:Menu>
                <asp:Panel ID="pnlMessage" runat="server">
                    <asp:Label ID="lblMessage" runat="server" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlTicketInfo" runat="server">
                <asp:HiddenField id="hfcreatedid" runat="server" />
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
                <hr />
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
                <hr />    
                <br />
                <asp:label ID="lblDescription" runat="server" Text="Description" CssClass="labels"  />
                <asp:TextBox ID="tbNotes" runat="server"  TextMode="MultiLine" Height="93px" Width="375px" OnTextChanged="tbNotes_TextChanged"/>
                <asp:TextBox ID="tbNotesHidden" runat="server" Visible="false" TextMode="MultiLine"></asp:TextBox>
            </asp:Panel>
            <hr />
            <asp:panel id="pnlTicketNotes" runat="server">
                <br />
                <asp:Button ID="bnAddNote" runat="server" Text="Add Note"  OnClick="bnAddNote_Click" CssClass="labels"/>
                <br />
                <br />
                <br />

                <script>
                    tinymce.init({
                            selector: 'textarea',
                            plugins: 'advlist autolink lists link image charmap print preview hr anchor pagebreak',
                            toolbar_mode: 'floating'
                    })
                </script>
                <asp:TextBox id="tbNewNote" runat="server"   AutoPostBack="true" Width="500px" TextMode="MultiLine" Height="140px" OnTextChanged="tbNewNote_TextChanged" />
                <asp:Gridview runat="server" ID="gvNotes" AutoGenerateColumns="false" ShowHeader="false" ShowFooter="false" CssClass="fonts" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="Info" HtmlEncode="false" />
                    </Columns>
                </asp:Gridview>
            </asp:panel>
            <asp:Panel ID="hiddendata" runat="server">
                <asp:HiddenField ID="lblisTech" runat="server" />
                <asp:HiddenField id="lblisAdmin" runat="server" />
                <asp:HiddenField ID="lblNewTicket" runat="server" />
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
            </asp:Panel>
            
        </div>
    </form>
</body>
</html>
