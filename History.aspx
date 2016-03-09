<%@ Page Title="History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="History.aspx.cs" Inherits="MBTimeSheetWebApp.History" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>

    <div class="row">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>Filter Table Contents:</h4>
            </div>
            <div class="panel-body">
                <div class="col-sm-2">
                    <dx:ASPxButton ID="LoadAll" runat="server" Text="View All" CssClass="btn btn-primary" OnClick="LoadAll_Click" BackgroundImage-ImageUrl='""' Font-Size="Medium" Width="100%"></dx:ASPxButton>
                </div>
                <div class="col-sm-2">
                    <dx:ASPxButton ID="LoadCr" runat="server" Text="Change Requests" CssClass="btn btn-primary" OnClick="LoadCr_Click" BackgroundImage-ImageUrl='""' Font-Size="Medium" Width="100%"></dx:ASPxButton>
                </div>
                <div class="col-sm-2">
                    <dx:ASPxButton ID="LoadWs" runat="server" Text="Site Visits" CssClass="btn btn-primary" OnClick="LoadWs_Click" BackgroundImage-ImageUrl='""' Font-Size="Medium" Width="100%"></dx:ASPxButton>
                </div>
                <div class="col-sm-2">
                    <dx:ASPxButton ID="LoadProjBtn" runat="server" Text="Load Users Projects" OnClick="RefreshBtn_Click" CssClass="btn btn-primary" BackgroundImage-ImageUrl='""' Font-Size="Medium" Visible="false" Width="100%"></dx:ASPxButton>
                </div>
            </div>
        </div>

    </div>
    <%-- <div class="row">
        <div class="form-horizontal" role="form">
            <div class="form-group">
                <label class="control-label col-sm-2" for="dateFilter">Date From/To:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server"></asp:TextBox>
                </div>                       
            </div>
        </div>     
   </div>
    <div class="row">
        <div class="form-horizontal" role="form">
            <div class="form-group">
                <label class="control-label col-sm-2" for="custFilter">Customer From/To:</label>
                <div class="col-sm-10">
                    <asp:TextBox ID="TextBox2" CssClass="form-control" runat="server"></asp:TextBox>
                </div>                       
            </div>
        </div>     
   </div>    --%>

    <div class="col-sm-12 voffset">
        <dx:ASPxGridView ID="UsersGridView" runat="server" Visible="False" SettingsBehavior-AllowFocusedRow="True" KeyFieldName="UserName" ClientInstanceName="grid" Theme="Moderno">
            <Columns>
                <dx:GridViewDataTextColumn VisibleIndex="0">
                    <DataItemTemplate>
                        <dx:ASPxCheckBox ID="userCheck" runat="server" OnInit="userCheck_Init"></dx:ASPxCheckBox>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataColumn FieldName="UserName" Caption="User Name" VisibleIndex="1">
                    <EditFormSettings Visible="true" />
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="Role" Caption="Role" VisibleIndex="2"></dx:GridViewDataColumn>
            </Columns>
        </dx:ASPxGridView>
    </div>


    <div class="section">
        <div class="form-group">
            <div class="col-sm-12 voffset">
                <div class="table-responsive">
                    <dx:ASPxGridView ID="HistoryGV" KeyFieldName="ID" runat="server" SettingsSearchPanel-Visible="True" SettingsSearchPanel-ShowClearButton="True" SettingsSearchPanel-ShowApplyButton="True" Settings-ShowFilterRow="True" Settings-ShowFilterRowMenu="False" Settings-ShowFilterBar="Hidden" SettingsPager-PageSize="15" SettingsBehavior-AllowSelectByRowClick="True" SettingsBehavior-AllowSelectSingleRowOnly="True" SettingsBehavior-ProcessFocusedRowChangedOnServer="True" SettingsBehavior-AllowFocusedRow="True" Width="90%" Theme="Moderno">
                        <Columns>
                            <dx:GridViewDataTextColumn VisibleIndex="0" Caption="ID" FieldName="ID" Visible="False"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="0" Caption="ObjectID" FieldName="ObjectID" Visible="False"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="1" Caption="UserName" FieldName="UserName"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Customer" FieldName="Customer"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="3" Caption="Project" FieldName="Project"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="4" Caption="ProjCode" FieldName="ProjCode"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="5" Caption="PM" FieldName="PM"></dx:GridViewDataTextColumn>
                           <dx:GridViewDataDateColumn VisibleIndex="6" Caption="Date" FieldName="Date">
                                <PropertiesDateEdit DisplayFormatString="d" EditFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="7" Caption="Cust Contact" FieldName="Cust Contact"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="8" Caption="Contact Email" FieldName="Contact Email"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="9" Caption="Signature" FieldName="Signature"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="10" Caption="Version" FieldName="Version"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="11" Caption="Type" FieldName="Type"></dx:GridViewDataTextColumn>
                        </Columns>
                        <%--  <ClientSideEvents BeginCallback="function(s, e) {alert(e.command); }" />--%>
                    </dx:ASPxGridView>
                    <dx:ASPxGridView ID="CrGridView" KeyFieldName="ID" runat="server" SettingsSearchPanel-Visible="True" SettingsSearchPanel-ShowClearButton="True" SettingsSearchPanel-ShowApplyButton="True" Settings-ShowFilterRow="True" Settings-ShowFilterRowMenu="False" Settings-ShowFilterBar="Hidden" SettingsPager-PageSize="15" SettingsBehavior-AllowSelectByRowClick="True" SettingsBehavior-AllowSelectSingleRowOnly="True" SettingsBehavior-ProcessFocusedRowChangedOnServer="True" SettingsBehavior-AllowFocusedRow="True" Width="90%" Visible="false" Theme="Moderno">
                        <Columns>
                            <dx:GridViewDataTextColumn VisibleIndex="0" Caption="ID" FieldName="ID" Visible="False"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="0" Caption="ObjectID" FieldName="ObjectID" Visible="False"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="1" Caption="UserName" FieldName="UserName"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Customer" FieldName="Customer"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="3" Caption="Project" FieldName="Project"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="4" Caption="ProjCode" FieldName="ProjCode"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="5" Caption="PM" FieldName="PM"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn VisibleIndex="6" Caption="Date" FieldName="Date">
                                <PropertiesDateEdit DisplayFormatString="d" EditFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="7" Caption="Cust Contact" FieldName="Cust Contact"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="8" Caption="Contact Email" FieldName="Contact Email"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="9" Caption="Signature" FieldName="Signature"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="10" Caption="Version" FieldName="Version"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="11" Caption="Type" FieldName="Type"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="12" Caption="Estimate" FieldName="Estimate"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="13" Caption="DeptMb" FieldName="DeptMb"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="14" Caption="Time" FieldName="Time"></dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <dx:ASPxGridView ID="SvGridView" KeyFieldName="ID" runat="server" SettingsSearchPanel-Visible="True" SettingsSearchPanel-ShowClearButton="True" SettingsSearchPanel-ShowApplyButton="True" Settings-ShowFilterRow="True" Settings-ShowFilterRowMenu="False" Settings-ShowFilterBar="Hidden" SettingsPager-PageSize="15" SettingsBehavior-AllowSelectByRowClick="True" SettingsBehavior-AllowSelectSingleRowOnly="True" SettingsBehavior-ProcessFocusedRowChangedOnServer="True" SettingsBehavior-AllowFocusedRow="True" Width="90%" Visible="false" Theme="Moderno">
                        <Columns>
                            <dx:GridViewDataTextColumn VisibleIndex="0" Caption="ID" FieldName="ID" Visible="False"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="0" Caption="ObjectID" FieldName="ObjectID" Visible="False"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="1" Caption="UserName" FieldName="UserName"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Customer" FieldName="Customer"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="3" Caption="Project" FieldName="Project"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="4" Caption="ProjCode" FieldName="ProjCode"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="5" Caption="PM" FieldName="PM"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn VisibleIndex="6" Caption="Date" FieldName="Date">
                                <PropertiesDateEdit DisplayFormatString="d" EditFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="7" Caption="Cust Contact" FieldName="Cust Contact"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="8" Caption="Contact Email" FieldName="Contact Email"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="9" Caption="Signature" FieldName="Signature"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="10" Caption="Version" FieldName="Version"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="11" Caption="Type" FieldName="Type"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="12" Caption="Deviation" FieldName="Deviation"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="13" Caption="Cr Required" FieldName="CrRequired"></dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn VisibleIndex="14" Caption="TimeSpend" FieldName="TimeSpend"></dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>

                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-sm-2">
            <dx:ASPxButton ID="LoadBtn" runat="server" Text="Load Project" CssClass="btn btn-primary" OnClick="LoadBtn_Click" BackgroundImage-ImageUrl='""' Font-Size="Medium" Width="100%"></dx:ASPxButton>
        </div>
        <div class="col-sm-2">
            <dx:ASPxButton ID="LoadDraftBtn" runat="server" Text="Load Draft" CssClass="btn btn-primary" OnClick="LoadDraftBtn_Click" BackgroundImage-ImageUrl='""' Font-Size="Medium" Width="100%"></dx:ASPxButton>
        </div>
        <div class="col-sm-2">
            <dx:ASPxButton ID="ExportToPdf" runat="server" Text="Export To PDF" OnClick="ExportToPdf_Click" CssClass="btn btn-primary" BackgroundImage-ImageUrl='""' Font-Size="Medium" Width="100%"></dx:ASPxButton>
        </div>
        <div class="col-sm-2">
            <dx:ASPxButton ID="LoadUsersBtn" runat="server" Text="Load Users" OnClick="LoadUsersBtn_Click" CssClass="btn btn-primary" BackgroundImage-ImageUrl='""' Font-Size="Medium" Width="100%" Visible="false" ClientVisible="false"></dx:ASPxButton>
        </div>
        <div class="col-sm-2">
            <dx:ASPxButton ID="DeleteBtn" runat="server" Text="Delete Record" OnClick="DeleteBtn_Click" CssClass="btn btn-primary" BackgroundImage-ImageUrl='""' Font-Size="Medium" Width="100%" Visible="false" ClientVisible="false" ClientSideEvents-Click="">
                <ClientSideEvents Click="function(s, e) {  
                    e.ProcessOnServer = confirm('Are you sure you wish to delete this entry?');
                    }" />
            </dx:ASPxButton>
        </div>


        <%--<div class="col-sm-2 voffset">
            <dx:ASPxButton ID="RefreshBtn" runat="server" Text="Refresh Grid" OnClick="RefreshBtn_Click" CssClass="btn btn-primary" BackgroundImage-ImageUrl='""' Font-Size="Medium"></dx:ASPxButton>
        </div>--%>
    </div>



</asp:Content>
