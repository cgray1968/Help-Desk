<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportsControl.ascx.cs" Inherits="Help_Desk.Reports" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<link href="CSS/Custom.css" rel="stylesheet" />

<asp:Panel ID="pnlTop" runat="server">
    <div class="mytitle">
        <asp:Label ID="lblTitle" runat="server" Text="Reports"/>
    </div>
    <br />
    <asp:Menu ID="menu1" runat="server" CssClass="menu" OnMenuItemClick="menu1_MenuItemClick" Orientation="Horizontal" ForeColor="IndianRed">
        <Items>
            <asp:MenuItem Text="Graphics" Value="Graphics">
                <asp:MenuItem Text="Department" Value="Department"/>
                <asp:MenuItem Text="Category" Value="Category" />
                <asp:MenuItem Text="Techs" Value="Techs" />
            
            </asp:MenuItem>

            <asp:MenuItem Text="|" Enabled="false" />
            <asp:MenuItem Text="Summary" Value="Summary">
            </asp:MenuItem>
        </Items>
    </asp:Menu>
    <br />
    <br />
    <br />
    <asp:Panel ID="pnlGraphics" runat="server" Visible="false" Width="1714px">
        <h1 class="font"> Select the Chart type: </h1>
        <asp:DropDownList ID="ddlChartType" runat="server" OnSelectedIndexChanged="ddlChartType_SelectedIndexChanged" CssClass="fonts" AutoPostBack="true"/>
        <br />
        <ajaxToolkit:BarChart ID="BarChart1"  runat="server" ChartHeight="300" ChartWidth = "1500" 
                ChartType="Column" ChartTitleColor="#0E426C" Visible = "false"
                CategoryAxisLineColor="#D08AD9" ValueAxisLineColor="#D08AD9" BaseLineColor="#A156AB">
        </ajaxtoolkit:BarChart>
        <asp:Chart ID="Chart1" runat="server" Width="1500px" Palette="Bright">
            <Titles>
                <asp:Title ShadowOffset="3" Name="Tech" />
            </Titles>
   
            <ChartAreas>
                <asp:ChartArea Name="Chartarea1" BorderWidth="0" />
            </ChartAreas>
        </asp:Chart>
    </asp:Panel>
    <asp:Panel ID="pnlSummary" runat="server" Visible="false">
        <asp:label ID="lblSummaryTickets" runat="server" Text="Ticket Summary" Font-Name="Calibri" ForeColor="Navy" Font-Size="160%"/>
        <br />
        <br />
        <asp:label ID="lblNew" Text="New :" runat="server" CssClass="labels" />
        <asp:TextBox ID="tbNew" runat="server" ReadOnly="true" cssclass="textbox" BorderStyle="none" backcolor="Transparent"/>
        <br />
        <br />
        <asp:label ID="lblOpen" Text="Open :" runat="server" CssClass="labels" />
        <asp:textbox ID="tbOpen" runat="server" ReadOnly="true" CssClass="textbox" BorderStyle="none" backcolor="Transparent"/>
        <br />
        <br />
        <asp:label ID="lblInProgress" Text="In Progress :" runat="server" CssClass="labels" />
        <asp:textbox ID="tbInProgress" runat="server" ReadOnly="true" CssClass="textbox" BorderStyle="none" backcolor="Transparent"/>
        <br />
        <br />
        <asp:label ID="lblCompleted" Text="Completed :" runat="server" CssClass="labels" />
        <asp:textbox ID="tbCompleted" runat="server" ReadOnly="true" CssClass="textbox" BorderStyle="none" backcolor="Transparent"/>
        <br />
        <br />
        <asp:label ID="lblClosed" Text="Closed :" runat="server" CssClass="labels" />
        <asp:textbox ID="tbClosed" runat="server" ReadOnly="true" CssClass="textbox" BorderStyle="none" backcolor="Transparent" />
        <br />
        <br />
        <asp:label ID="lblCanceled" Text="Canceled :" runat="server" CssClass="labels" />
        <asp:textbox ID="tbCanceled" runat="server" ReadOnly="true" CssClass="textbox" BorderStyle="none" backcolor="Transparent"/>
        <br />
        <br />
        <asp:label ID="lblTotal" Text="Total :" runat="server" CssClass="labels" />
        <asp:textbox ID="tbTotal" runat="server" ReadOnly="true" backcolor="Transparent" CssClass="textbox" BorderStyle="none" />
        <br />
        <br />
        <br />
        <hr />
        <br />
        <asp:label ID="lblSummaryTechs" runat="server" Text="Tech Summary" Font-Name="Calibri" ForeColor="Navy" Font-Size="160%"/>
        <br />        
        <br />        
        <asp:GridView ID="gvSummaryTechs" runat="server" GridLines="vertical" AutoGenerateColumns="false" CssClass="fonts"  >
            <columns>
                <asp:BoundField DataField="Tech" HeaderText="Tech" ItemStyle-HorizontalAlign="Center" ItemStyle-forecolor="Purple"/>
                <asp:BoundField DataField="Open" HeaderText="Open" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="InProgress" HeaderText="In Progress"  ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="Completed" HeaderText ="Completed" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Closed" HeaderText="Closed" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="Canceled" HeaderText="Canceled" ItemStyle-HorizontalAlign="Center"/>
                <asp:BoundField DataField="Total" headertext="Total" ItemStyle-HorizontalAlign="Center"/>
            </columns>
        </asp:GridView>
            </asp:Panel>
</asp:Panel>