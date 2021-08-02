<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryControl.ascx.cs" Inherits="Help_Desk.CategoryControl"  %>
<link href="CSS/Custom.css" rel="stylesheet" />
<script src="https://cdn.tiny.cloud/1/fmmdlrov3kr8wk93ddb7xo1za9ile9o0vrp6swklhgixzwc0/tinymce/5/tinymce.min.js"></script>
   <script src="https://cdn.tiny.cloud/1/fmmdlrov3kr8wk93ddb7xo1za9ile9o0vrp6swklhgixzwc0/tinymce/5/tinymce.min.js"></script>
    <script>
        tinymce.init({
            selector: 'textarea',
            plugins: 'advlist autolink lists link image charmap print preview hr anchor pagebreak',
            menubar: false,
            toolbar: 'undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | outdent indent'
        })
    </script>
        <asp:Label ID="lblText" runat="server" />
                  <asp:Panel ID="pnlCategory" runat="server">
                    <div class="row" >
                        <div class="Categories catLeft">
                            <asp:Label ID="lbltypecategory" runat="server" Text="Category :" cssclass="fonts" />
                            <br/>
                            <asp:TextBox ID="tbCategory" runat="server" cssclass="fonts" OnTextChanged="tbCategory_TextChanged"  AutoPostBack="true"/>
                            <asp:ImageButton ID="btnDeleteCategory" runat="server" height="30" Width="30" ImageUrl="~/Images/RemoveButton.png" OnClick="btnDeleteCategory_Click" />
                            <br/>
                            <asp:DropDownList ID="ddlCategory" runat="server" cssclass="fonts" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" />
                        </div>
                        <div class="Categories catMiddle">
                            <asp:label ID="lblSubCategory" runat="server" CssClass="fonts" Text="SubCategory"/>
                            <br/>
                            <asp:TextBox ID="tbSubCategory" runat="server" CssClass="fonts" OnTextChanged="tbSubCategory_TextChanged" AutoPostBack="true"/>
                            <asp:ImageButton ID="btnDeleteSubCategory" runat="server" height="30" Width="30" ImageUrl="~/Images/RemoveButton.png" OnClick="btnDeleteSubCategory_Click" />
                            <br />
                            <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="fonts" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged" AutoPostBack="true"/>
                        </div>
                        <div class="Categories catRight">
                            <asp:label ID="lblSelection" runat="server" CssClass="fonts" Text="Selection" />
                            <br/>
                            <asp:TextBox ID="tbSelection" runat="server" CssClass="fonts" OnTextChanged="tbSelection_TextChanged"  AutoPostBack="true"/>
                            <asp:ImageButton ID="btnDeleteSelection" runat="server" height="30" Width="30" ImageUrl="~/Images/RemoveButton.png" OnClick="btnDeleteSelection_Click" />
                            <br />
                            <asp:DropDownList ID="ddlSelection" runat="server" CssClass="fonts" OnSelectedIndexChanged="ddlSelection_SelectedIndexChanged" AutoPostBack="true" />
                         </div>
                        <br />
                    </div>
                    <div style="clear:both;"></div>
                        <hr />
                        <asp:Button ID="btnSaveEmailBody" runat="server" OnClick="btnSaveEmailBody_Click" text="Save" CssClass="button"/>
                        <br />
                        <textarea id="taEmail" runat="server" name="taEmail" rows="15" style=" width:85%"></textarea>
                </asp:Panel>
