<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Help_Desk.Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <link rel="stylesheet" type="text/css" href="CSS/Custom.css" />
        <title>FBC Dallas Help Desk</title>
        <style type="text/css">
        .auto-style1 {
            width: 486px;
        }
        </style>
    </head>
    <body class="background">
        <form id="AdminPage" runat="server" title="Admin Page">
            <div>
                <asp:ScriptManager ID="asm" runat="server" />
                <asp:Panel ID="pnlTop" runat="server">
                   <asp:Label ID="lblTitle" CssClass="mytitle" runat="server" Text="First Baptist Dallas Help Desk" width="100%" />
                   
                <asp:Menu ID="menuAdmin" runat="server"  OnMenuItemClick="menuAdmin_MenuItemClick" CssClass="menu" orientation="horizontal" ForeColor="IndianRed"  >
                    <LevelSubMenuStyles>
                        <asp:SubMenuStyle BackColor="LightGray" ForeColor="IndianRed" />
                    </LevelSubMenuStyles>
                    <Items>
                        <asp:MenuItem Text="Return" Value="Return"/>
                        <asp:MenuItem Text="|" Value="|1" Selectable="false"/>
                        <asp:MenuItem Text="Users" Value="Users">
                            <asp:MenuItem Text="New User" Value="NewUser" />
                            <asp:MenuItem Text="Import from A/D" />
                        </asp:MenuItem>
                        <asp:MenuItem Text="|" Selected="false" />
                        <asp:MenuItem Text="System Info">
                            <asp:MenuItem Text="System Logs" />
                            <asp:MenuItem Text="Create new Info" />
                        </asp:MenuItem>
                        <asp:MenuItem Text="|" Selectable="false" />
                        <asp:MenuItem Text="Department">
                            <asp:MenuItem Text="New Department" />
                        </asp:MenuItem>
                        <asp:MenuItem Text="|" Selectable="false" />
                        <asp:MenuItem Text="Computer">
                            <asp:MenuItem Text="New Computer" />
                            <asp:MenuItem Text="Improt from A/D" />
                        </asp:MenuItem>
                        <asp:MenuItem Text="|" Selectable="false" />
                        <asp:MenuItem Text="Categories">
                            <asp:MenuItem Text="Category">
                                <asp:MenuItem Text="New Category" />
                            </asp:MenuItem>
                            <asp:MenuItem Text="SubCategory">
                                <asp:MenuItem Text="New SubCategory" />
                            </asp:MenuItem>
                            <asp:MenuItem Text="Selection">
                                <asp:MenuItem Text="New Selection" />
                            </asp:MenuItem>
                        </asp:MenuItem>
                        </Items>
                </asp:Menu>           
                <asp:Label ID="lblMessage" runat="server" />
                <br />
                    
                </asp:Panel>
                <asp:Panel ID="pnlUser" runat="server" >
                    <asp:Hiddenfield ID="hflNewUserID" runat="server" />
                    <asp:Label ID="lblFilter" Text="Department:" runat="server" CssClass="labels"/>
                    <asp:dropdownlist id="ddlNewDepartment" runat="server" OnSelectedIndexChanged="ddlNewDepartment_SelectedIndexChanged"   AutoPostBack="true" CssClass="textbox" />
                    <br />
                    <br />
                    <asp:Label ID="lblDDLUser" runat="server" Text="Employee:" CssClass="labels"/>
                    <asp:DropDownList ID="ddlGetUser" runat="server" OnSelectedIndexChanged="ddlGetUser_SelectedIndexChanged" AutoPostBack="true" 
                    CssClass="textbox"/>
                    <br />
                    <br />
                    <hr />
                    <br />
                    <asp:Label ID="lblNewUserFirstName" runat="server" Text="First Name"  CssClass="labels" />
                    <asp:TextBox ID="tbNewUserFirstName" runat="server" CssClass="textbox" />
                    <br />
                    <br />
                    <asp:Label ID="lblNewUserLastName" runat="server" Text="Last Name" CssClass="labels" />
                    <asp:TextBox ID="tbNewUserLastName" runat="server" CssClass="textbox" />
                    <br />
                    <br />
                    <asp:Label ID="lblNewUserEmail" runat="server" Text="Email" CssClass="labels" />
                    <asp:TextBox ID="tbNewUserEmail" runat="server" CssClass="textbox" />
                    <br />
                    <br />
                    <asp:label id="lblNewUserDepartment" runat="server" Text="Department" CssClass="labels" />
                    <asp:DropDownList ID="ddlNewUserDepartment" runat="server" AutoPostBack="true" CssClass="textbox"/>
                    <br />
                    <br />
                    <div class="column2">
                        <div class="left">
                            <asp:label id="lblNewUserTickets" runat="server" text="Tickets" CssClass="fonts" />
                            <br />
                            <br />
                            <asp:GridView ID="gvNewUserTickets" runat="server" EmptyDataText="No Tickets found" allowsorting="true" AllowPaging="true" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  BorderColor="Black" onrowcommand="gvNewUserTickets_RowCommand" PageSize="5" AutoPostBack="true" OnPageIndexChanging="gvNewUserTickets_PageIndexChanging" CssClass="fonts" Width="1254%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Link">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlTicket" runat="server" 
                                                NavigateUrl='<%# string.Format("~/thisticket.aspx?tid={0}&UserID={1}", 
                                                    HttpUtility.UrlEncode(Eval("Ticketid").ToString()), 
                                                    HttpUtility.UrlEncode(Eval("PersonID").ToString())) %>' 
                                                Text='<%# Eval("TicketID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PersonID" visible="false"/>
                                    <asp:BoundField DataField="Description" HeaderStyle-Width="85%"/>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="right">
                            <asp:Label ID="lblNewRecentActivity" runat="server" Text="Recent Activity" CssClass="fonts" />
                            <br />
                            <br />
                            <asp:GridView ID="gvNewRecentActivity" runat="server" allowsorting="false" AllowPaging="true" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"   BorderColor="Black" onrowcommand="gvNewRecentActivity_RowCommand"  PageSize="5" AutoPostBack="true" CssClass="fonts" Width="100%">
                                <Columns>
                                    <asp:BoundField DataField="SecurityLogDate" HeaderText="Date" />
                                    <asp:BoundField DataField="SecurityLogInfo" HeaderText="Info" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
                <div style="clear"></div>
                <asp:Panel id="pnlUsersGrid" runat="server" Visible="false">
                    <br/>
                    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  GridLines="Both" BorderColor="Black" onrowcommand="gvUsers_RowCommand" >
                        <Columns>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:Button ID="btnUsersEdit" runat="server" CommandName="btnUserEdit" Text="Edit" CommandArgument='<%# Container.DataItemIndex %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:Button ID="btnUsersDelete" runat="server" CommandName="btnUserDelete" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PersonID" Visible="false" />
                            <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                            <asp:BoundField DataField="LastName" HeaderText="Last Name"/>
                            <asp:BoundField DataField="EmailAddress" HeaderText="Email Address"/>
                            <asp:BoundField DataField="LoginName" HeaderText="Login Name"/>
                            <asp:BoundField DataField="LastLogin" HeaderText="Last Login"/>
                            <asp:BoundField DataField="ComputerID" Visible="false" />
                            <asp:BoundField DataField="ComputerName" HeaderText="Computer Name" />
                            <asp:BoundField DataField="DepartmentID" Visible="false" />
                            <asp:BoundField DataField="DepartmentName" HeaderText="Department Name" />
                        </Columns>
                    </asp:GridView>
                    <asp:Panel ID="pnlUsersEdit" runat="server" Visible="false">
                        <asp:hiddenfield ID="hfPersonID" runat="server" />
                        <asp:HiddenField ID="hfLoginID" runat="server" />
                        <br />
                        <asp:label ID="lblUserFirstName" runat="server" Text="First Name:" CssClass="labels" />
                        <asp:TextBox ID="tbUserFirstName" runat="server" CssClass="textbox" />
                        <br />
                        <br />
                        <asp:Label ID="lblUserLastName" runat="server" Text="Last Name:" CssClass="labels" />
                        <asp:TextBox ID="tbUserLastName" runat="server" CssClass="textbox" />
                        <br />
                        <br />
                        <asp:Label ID="lblUserEmailAddress" runat="server" Text="Email : " CssClass="labels" />
                        <asp:TextBox ID="tbUserEmailAddress" runat="server" CssClass="textbox" />
                        <br />
                        <br />
                        <asp:Label ID="lblUserLoginName" runat="server" text="Login Name" CssClass="labels" />
                        <asp:TextBox  ID="tbUserLoginName" runat="server" CssClass="textbox"/>
                        <br />
                        <br />
                        <asp:label ID="lblUserPassword" runat="server" text="Password" CssClass="labels" />
                        <asp:TextBox ID="tbUserPassword" runat="server" CssClass="textbox" TextMode="password"/>
                        <asp:Label ID="lblUserPasswordConfirm" runat="server" text="Confirm" CssClass="labels" />
                        <asp:TextBox id="tbUserPasswordConfirm" runat="server" CssClass="textbox" TextMode="password"/>
                        <br />
                        <br />
                        <asp:Label id="lblUserDepartment" runat="server" Text="Department" CssClass="labels" />
                        <asp:DropDownList ID="ddlUserDepartment" runat="server" cssclass="textbox" />
                        <br />
                        <br />
                        <asp:Label ID="lblUserComputer" runat="server" Text="Computer:" CssClass="labels" />
                        <asp:DropDownList ID="ddlUserComputer" runat="server" CssClass="textbox" />
                        <br />
                        <br />
                        <asp:Button ID="btnUserSave" runat="server" Text="Save" OnClick="btnUserSave_Click" CssClass="labels" />
                        <asp:Button ID="btnUserCancel" runat="server" Text="Cancel" OnClick="btnUserCancel_Click" CssClass="labels" />
                    </asp:Panel>
                </asp:Panel>
                <br />
                <asp:Panel ID="pnlComputers" runat="server" Visible="false">
                    <br />
                    <asp:GridView ID="gvComputers" runat="server"  AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  GridLines="Both" BorderColor="Black" OnRowCommand="gvCompters_RowCommand" CssClass="Fonts">
                        <Columns>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:Button ID="btnComputerEdit" CommandName="ComputerEdit" runat="server" Text="Edit" CommandArgument='<%# Container.DataItemIndex %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:Button ID="btnComputerDelete" runat="server" CommandName="ComputerDelete" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ComputerID" Visible="false" />
                            <asp:BoundField datafield="ComputerName" HeaderText="Computer Name" />
                            <asp:BoundField DataField="OperatingSystem" HeaderText="Operating System" />
                            <asp:BoundField DataField="InstallDate" HeaderText="Install Date" />
                            <asp:BoundField DataField="DepartmentID" Visible="false" />
                            <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                            <asp:BoundField DataField="Personid" Visible="false" />
                            <asp:BoundField DataField="UserName" HeaderText="User Name" />
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Panel ID="pnlComputerEdit" runat="server" Visible="false">
                        <asp:HiddenField ID="hfComputerID" runat="server" />
                        <br />
                        <asp:Label ID="lblComputerName" runat="server" Text="Computer:" CssClass="labels" />
                        <asp:TextBox ID="tbComputerName" runat="server" CssClass="textbox" />
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="lblOperatingSystem" runat="server" Text="OS" CssClass="labels" />
                        <asp:TextBox ID="tbOperatingSystem" runat="server" cssclass="textbox" />
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="lblInstallDate" runat="server" Text="Install Date" CssClass="labels" />
                        <asp:TextBox ID="tbInstallDate" runat="server"  CssClass="textbox" />
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="lblComputerDepartment" runat="server" Text="Department" CssClass="labels" />
                        <asp:DropDownList ID="ddlComputerDepartment" runat="server" OnSelectedIndexChanged="ddlComputerDepartment_SelectedIndexChanged" cssclass="fonts"/>
                        <br />
                        <br />
                        <br />
                        <asp:Label ID="lblComputerPerson" runat="server" text="User:" CssClass="labels" />
                        <asp:DropDownList ID="ddlComputerPerson" runat="server" OnSelectedIndexChanged="ddlComputerPerson_SelectedIndexChanged" CssClass="fonts"/>
                        <br />
                        <br />
                        <asp:Button id="btnComputerSave" runat="server" Text="Save" OnClick="btnComputerSave_Click" CssClass="fonts" />
                        <asp:Button id="BtnComputerCancel" runat="server" Text="Cancel" OnClick="BtnComputerCancel_Click" cssclass="fonts"/>
                     </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="pnlDepartment" runat="server" Visible="false">
                    <br />
                    <asp:GridView id="gvDepartment" runat="server"  AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  GridLines="Both" BorderColor="Black" OnRowCommand="gvDepartment_RowCommand" CssClass="fonts">
                        <Columns>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:Button ID="btnDepartmentEdit" runat="server" CommandName="DepartmentEdit" Text="Edit" CommandArgument='<%# Container.DataItemIndex %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField headertext="Delete" >
                                <ItemTemplate>
                                    <asp:Button ID="btnDepartmentDelete" runat="server" CommandName="DepartmentDelete" text="Delete"  CommandArgument='<%# Container.DataItemIndex %>' />                            
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField datafield="DepartmentID" visible="false"/>
                            <asp:boundfield DataField="DepartmentName" HeaderText="Department Name" />
                        </Columns>
                    </asp:GridView>
                    <asp:Panel ID="pnlDepartmentEdit" runat="server" Visible="false">
                        <br />
                        <asp:hiddenfield ID="hfDeparmentID" runat="server" />
                        <asp:Label ID="lblDepartmentName" runat="server" Text="Department:" cssclass="labels" font-name="Calibri" />
                        <asp:TextBox ID="tbDepartmentName" runat="server" CssClass="textbox" font-name="Calibri"  /> 
                        <br />
                        <br />
                        <asp:Button ID="btnDepartmentSave" runat="server" Text="Save" cssclass="labels" OnClick="btnDepartmentSave_Click" />
                        <asp:Button ID="btnDepartmentCancel" Text="Cancel" runat="server" CssClass="labels" OnClick="btnDepartmentCancel_Click" />
                    </asp:Panel>
                </asp:Panel>
                <br />
                <br />
                <asp:Panel ID="pnlMessages" runat="server" Visible="false">
                    <br />
                    <asp:Label ID="lblNewMessage" runat="server" Text="Message:" CssClass="labels" />
                    <asp:TextBox ID="tbNewMessage" runat="server" CssClass="textbox" Width="600px"/>
                    <asp:CheckBox ID="cbNewMessage" runat="server"  Text="Enabled" AutoPostBack="True" cssclass="fonts"/>
                    <asp:Button ID="btnSaveMessage" runat="server" OnClick="btnSaveMessage_Click" text="Save" CssClass="fonts"/>
                    <br />
                    <br />
                    <asp:GridView ID="gvMessages" runat="server" OnRowCommand="gvMessages_RowCommand1" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  GridLines="Both" BorderColor="Black" CssClass="fonts">
                        <Columns>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:Button ID="btnMessageEdit" runat="server" Text="Edit" CommandName="MessageEdit" CommandArgument='<%# Container.DataItemIndex %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:Button ID="btnMessageDelete" runat="server" Text="Delete" CommandName="MessageDelete" CommandArgument='<%# Container.DataItemIndex %>'/> 
                                </ItemTemplate>
                             </asp:TemplateField>
                            <asp:BoundField DataField="SystemInfoID" visible="false" />
                            <asp:BoundField DataField="Information" HeaderText="Message" />
                            <asp:BoundField DataField="Active" HeaderText="Active" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                            <asp:BoundField DataField="DateCreated" HeaderText="Date Created" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="pnlCategory" runat="server" Visible="false">
                    <asp:Panel ID="pnlCategoryInput" runat="server" visible="false">
                        <br />
                        <asp:HiddenField ID="hfCategoryID" runat="server" />
                        <asp:HiddenField ID="HFType" runat="server" />
                        <asp:Label ID="lbltypecategory" runat="server" Text="Text :" cssclass="labels" />
                        <asp:TextBox ID="tbCategory" runat="server" cssclass="textbox" />
                        <asp:Button ID="btnEditSave" runat="server" OnClick="btnEditSave_Click"  Text="Save" />
                    </asp:Panel>
                    <br />
                    
                    <asp:GridView ID="gvcategory" runat="server" OnRowCommand="gvCategory_RowCommand" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  GridLines="Both" BorderColor="Black">
                        <Columns>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:button ID="btnCategoryEdit" runat="server" CommandName="EditCategory" text="Edit" CommandArgument='<%# Container.DataItemIndex %>'/> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:Button ID="btnCategorySave" runat="server" CommandName="DeleteCategory" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>'/> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:boundfield DataField="CategoryID" Visible="false" />
                            <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:GridView ID="gvSubCategory" runat="server" OnRowCommand="gvSubCategory_RowCommand" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  GridLines="Both" BorderColor="Black" >
                                        <Columns>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnSubCategoryEdit" runat="server" Text="Edit" CommandName="btnSubCategoryEdit" CommandArgument='<%# Container.DataItemIndex %>'/> 
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnSubCategoryDelete" runat="server" CommandName="btnSubCategoryDelete" CommandArgument='<%# Container.DataItemIndex %>'/> 
                                                 </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SubCategoryID" Visible="false" />
                                            <asp:BoundField DataField="CategoryID" Visible="false" />
                                            <asp:BoundField DataField="SubCategoryName" HeaderText="SubCategory Name" />
                                            <asp:TemplateField HeaderText="Selection">
                                                <EditItemTemplate>
                                                    <asp:GridView ID="gvSelection" runat="server" OnRowCommand="gvSelection_RowCommand" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  GridLines="Both" BorderColor="Black">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Edit">
                                                                <EditItemTemplate>
                                                                    <asp:Button ID="btnSelectionEdit" runat="server" CommandName="btnSelectionEdit" Text="Edit" CommandArgument='<%# Container.DataItemIndex %>'/>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delete">
                                                                <EditItemTemplate>
                                                                    <asp:Button ID="btnSelectionDelete" runat="server" CommandName="btnSelectionDelete" Text="Delete" CommandArgument='<%# Container.DataItemIndex %>'/>
                                                                </EditItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="SelectionID" Visible="false" />
                                                            <asp:BoundField DataField="SubCategoryID" Visible="false" />
                                                            <asp:BoundField DataField="Selectionname" HeaderText="Selection Name" />
                                                        </Columns>    
                                                    </asp:GridView>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </form>
    </body>
</html>

