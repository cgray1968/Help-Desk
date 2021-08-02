<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessagesControl.ascx.cs" Inherits="Help_Desk.MessagesControl" %>
               <asp:Panel ID="pnlMessages" runat="server" Visible="true">
                   <asp:HiddenField ID="hfMessageID" runat="server" />
                    <br />
                    <asp:Label ID="lblNewMessage" runat="server" Text="System Messages:" CssClass="labels" />
                    <asp:TextBox ID="tbNewMessage" runat="server" CssClass="textbox" Width="600px"/>
                    <asp:CheckBox ID="cbNewMessage" runat="server"  Text="Enabled" AutoPostBack="True" cssclass="fonts"/>
                    <asp:Button ID="btnSaveMessage" runat="server" OnClick="btnSaveMessage_Click" text="Save" CssClass="button"/>
                   <asp:Button ID="btnClearMessage" runat="server" OnClick="btnClearMessage_Click" Text="Clear" CssClass="button" />
                     <br />
                    <br />
                    <asp:GridView ID="gvMessages" runat="server" OnRowCommand="gvMessages_RowCommand" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="MintCream" BackColor="AntiqueWhite"  GridLines="Both" BorderColor="Black" CssClass="fonts">
                        <Columns>
                            <asp:TemplateField >
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnMessageEdit" runat="server" ImageURL="~/Images/Edit-pencil-png.png" Height="30" CommandName="MessageEdit" CommandArgument='<%# Container.DataItemIndex %>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnMessageDelete" runat="server" ImageURL="~/Images/RemoveButton.png" Height="30" CommandName="MessageDelete" CommandArgument='<%# Container.DataItemIndex %>'/> 
                                </ItemTemplate>
                             </asp:TemplateField>
                            <asp:BoundField DataField="SystemInfoID" />
                            <asp:BoundField DataField="Information" HeaderText="Message" />
                            <asp:BoundField DataField="Active" HeaderText="Active" />
                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                            <asp:BoundField DataField="DateCreated" HeaderText="Date Created" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>