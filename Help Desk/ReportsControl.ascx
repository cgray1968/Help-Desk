<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportsControl.ascx.cs" Inherits="Help_Desk.Reports" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<link href="CSS/Custom.css" rel="stylesheet" />

<asp:Panel ID="pnlTop" runat="server">
    <div class="mytitle">
        <asp:Label ID="lblTitle" runat="server" Text="Reports"/>
    </div>
    <br />
    <asp:GridView ID="gvReports" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanged="gvReports_SelectedIndexChanged" GridLines="None" >
        <Columns>
            <asp:BoundField DataField="Name" />
        </Columns>

    </asp:GridView>
    <br />
    <br />
    <br />
    <asp:Panel ID="pnlReports" runat="server">
        <rsweb:ReportViewer ID="rvReports" runat="server" ProcessingMode="Remote"></rsweb:ReportViewer>
    </asp:Panel>
</asp:Panel>