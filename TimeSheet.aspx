<%@ Page Title="Time Sheet" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TimeSheet.aspx.cs" EnableEventValidation="false" Inherits="MBTimeSheetWebApp.TimeSheet" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Data.v15.1, Version=15.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        #wsSig {
            width: 100%;
            height: 200px;
            border: 1px solid silver;
            background-color: white;
        }

        #crSig {
            width: 100%;
            height: 200px;
            border: 1px solid silver;
            background-color: white;
        }
    </style>

    <div class="jumbotron" style="font-family: Akzidenz Grotesk">
        <div class="row voffset">
            <div class="col-sm-3">
            </div>
            <div class="col-sm-6">
                <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true" ImageUrl="~/Content/Images/Milner_Browne_Logo_Full_Colour_Black.png" Height="100%" Width="100%"></dx:ASPxImage>
            </div>
            <div class="col-sm-3">
            </div>
        </div>
        <div class="row">
            <div class="form-group">
                <div class="form-inline" role="form">
                    <div class="col-sm-3">
                        <%-- <asp:RequiredFieldValidator ID="CustCodeRfv" runat="server"
                            ControlToValidate="CustCode"
                            ErrorMessage="Please Enter Customer Code "
                            ForeColor="Red" ViewStateMode="Inherit" Visible="True" Display="Dynamic" Font-Size="Small" Height="20px" SetFocusOnError="True">
                        </asp:RequiredFieldValidator>--%>
                    </div>
                    <div class="col-sm-3">
                        <label class="sr-only" for="CustCode">Customer:</label>
                        <asp:TextBox ID="CustCode" CssClass="form-control" runat="server" placeholder="Customer Code"></asp:TextBox>
                    </div>
                    <div class="col-sm-6">
                        <label class="sr-only" for="CustName">Customer Name</label>
                        <asp:TextBox ID="CustName" CssClass="form-control" runat="server" placeholder="Customer Name"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-group">
                <div class="form-inline" role="form">
                    <div class="col-sm-3">
                        <%-- <asp:RequiredFieldValidator ID="ProjCodeRfv" runat="server"
                            ControlToValidate="ProjCode"
                            ErrorMessage="Please Enter Project Code"
                            ForeColor="Red" Font-Size="Small" Display="Dynamic" Width="100%">
                        </asp:RequiredFieldValidator>--%>
                    </div>
                    <div class="col-sm-3">
                        <label class="sr-only" for="ProjCode">Project Code:</label>
                        <asp:TextBox ID="ProjCode" CssClass="form-control" runat="server" placeholder="Project Code"></asp:TextBox>
                    </div>
                    <div class="col-sm-6">
                        <label class="sr-only" for="ProjName">Project Name:</label>
                        <asp:TextBox ID="ProjName" CssClass="form-control" runat="server" placeholder="Project Name"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-group">
                <div class="form-inline" role="form">
                    <div class="col-sm-3">
                    </div>
                    <div class="col-sm-3">
                        <label class="sr-only" for="Pm">PM:</label>
                        <asp:TextBox ID="PmName" CssClass="form-control" runat="server" placeholder="PM Name"></asp:TextBox>
                    </div>
                    <div class="col-sm-6">
                        <label class="sr-only" for="ProjName">Pm Email:</label>
                        <asp:TextBox ID="PmEmail" TextMode="Email" CssClass="form-control" runat="server" placeholder="PM Email"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator id="pmRfv" runat="server"
                              ControlToValidate="PmEmail"
                              ErrorMessage="PM Email is a required field."
                              ForeColor="Red" Display="Dynamic" Font-Size="Small">
                            </asp:RequiredFieldValidator>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="form-group">
                <div class="form-inline" role="form">
                    <div class="col-sm-3">
                    </div>
                    <div class="col-sm-3">
                        <label class="sr-only" for="Date">Date</label>
<%--                        <asp:TextBox ID="DateTxt1" CssClass="form-control" runat="server"></asp:TextBox>--%>
                        <dx:ASPxDateEdit runat="server" CssClass="form-control" ID="DateTxt" DateOnError="Undo">
                            
                        </dx:ASPxDateEdit>
                    </div>
                    <div class="col-sm-7">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <section id="Tabs" role="tabpanel">
        <ul class="nav nav-tabs">
            <li class="active"><a href="#home" role="tab" data-toggle="tab">Worksheet</a></li>
            <li><a href="#crForm" role="tab" data-toggle="tab">Change Request</a></li>
            <li><a href="#checkList" role="tab" data-toggle="tab">Checklist</a></li>
        </ul>
    </section>
    <!--nav tabs-->

    <div class="tab-content" style="margin-top: 4%">
        <div id="home" class="tab-pane fade in active">
            <!--WorkSheet tab-->
            <div class="row">
                <div class="form-horizontal" role="form">
                    <div class="form-group">
                        <label class="control-label col-sm-2" for="timeSpend">Time Spend (hrs):</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="timeSpendTxt" CssClass="form-control" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="timeSpendTxt" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[0-9]\d*(\.\d+)?$" ForeColor="Red"></asp:RegularExpressionValidator>
                        </div>
                        <div class="col-sm-8">
                        </div>
                    </div>
                </div>
            </div>

            <section>
                <!--agenda Table-->
                <div class="row voffset">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label class="control-label col-sm-2" for="agendaTable">Agenda:</label>
                            <div class="col-sm-10">
                                <div class="table-bordered form control">
                                    <dx:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="100%" OnCallback="ASPxCallbackPanel1_Callback">
                                        <PanelCollection>
                                            <dx:PanelContent>
                                                <dx:ASPxGridView ID="AgendaGrid" runat="server" Width="100%" KeyFieldName="ID" SettingsBehavior-AllowSelectByRowClick="True" SettingsBehavior-AllowSelectSingleRowOnly="True" OnRowUpdating="RowUpdating" OnRowInserted="AgendaGrid_RowInserted" OnRowInserting="AgendaGrid_RowInserting" OnRowDeleted="AgendaGrid_RowDeleted" SettingsCommandButton-DeleteButton-ButtonType="Button" SettingsCommandButton-NewButton-ButtonType="Button" Theme="Moderno" SettingsCommandButton-EditButton-ButtonType="Button">
                                                    <Columns>
                                                        <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowDeleteButton="true" ShowEditButton="true" />
                                                        <dx:GridViewDataTextColumn VisibleIndex="0" Caption="ID" FieldName="ID" Visible="False"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn VisibleIndex="1" Caption="Point" FieldName="Point"></dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Description" FieldName="Description"></dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <ClientSideEvents RowClick="function(s,e) { s.StartEditRow(e.visibleIndex); }" />
                                                    <ClientSideEvents EndCallback="function(s, e) {grid.Refresh();}" />
                                                </dx:ASPxGridView>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxCallbackPanel>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </section>
            <!--agenda table-->
            <section>
                <div class="row">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label class="control-label col-sm-2" for="deviation">Deviation:</label>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="deviationList" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="0" Selected="False">Yes</asp:ListItem>
                                    <asp:ListItem Value="1" Selected="True">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label class="control-label col-sm-2" for="cr">CR Required:</label>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="crSelect" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="0" Selected="False">Yes</asp:ListItem>
                                    <asp:ListItem Value="1" Selected="True">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <!--select buttons-->
            <section>
                <div class="row voffset">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label class="control-label col-sm-2" for="outcomeTable">Outcome:</label>
                            <div class="col-sm-10">
                                <div class="table-bordered form control">
                                    <dx:ASPxGridView ID="outcomeGrid" runat="server" Width="100%" KeyFieldName="ID" SettingsBehavior-AllowSelectByRowClick="True" SettingsBehavior-AllowSelectSingleRowOnly="True" OnRowUpdating="outcomeGrid_RowUpdating" OnRowInserting="outcomeGrid_RowInserting" OnRowDeleted="outcomeGrid_RowDeleted" SettingsCommandButton-DeleteButton-ButtonType="Button" SettingsCommandButton-NewButton-ButtonType="Button" Theme="Moderno" SettingsCommandButton-EditButton-ButtonType="Button">
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowDeleteButton="true" ShowEditButton="true" />
                                            <dx:GridViewDataTextColumn VisibleIndex="0" Caption="ID" FieldName="ID" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn VisibleIndex="1" Caption="Point" FieldName="Point"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Description" FieldName="Description"></dx:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents RowClick="function(s,e) { s.StartEditRow(e.visibleIndex); }" />

                                    </dx:ASPxGridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <!--outcome table-->
            <section>
                <!--sign off section-->
                <div class="row">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label class="control-label col-sm-2" for="signOff">Sign Off</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="contactName" CssClass="form-control" runat="server" placeholder="Contact Name"></asp:TextBox>
                                <%-- <input id="contactName" class="form-control" type="text" placeholder="Contact Name" />--%>
                            </div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="contactEmail" CssClass="form-control" TextMode="email" placeholder="Contact Email" runat="server"></asp:TextBox>
                                <%-- <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                              ControlToValidate="contactEmail"
                              ErrorMessage="Contact Email is a required field."
                              ForeColor="Red" SetFocusOnError="True">
                            </asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-horizontal" role="form">
                        <div class="col-sm-2">
                        </div>
                        <div class="col-sm-8">
                            <div id="wsSig" class="form-group">
                            </div>
                            <asp:HiddenField ID="hdfSignatureDataNative1" runat="server" Value="" />
                            <asp:HiddenField ID="hdfSignatureDataBitmap1" runat="server" Value="" />
                        </div>
                        <div class="col-sm-1">
                            <dx:ASPxButton ID="ResetBtn1" CssClass="btn btn-primary voffset" runat="server" Text="Reset" BackgroundImage-ImageUrl='""' Font-Size="Medium" HorizontalAlign="Center">
                                <ClientSideEvents Click="function(s, e) { Reset(); e.processOnServer = false;}" />
                            </dx:ASPxButton>
                        </div>
                        <div class="col-sm-1">
                        </div>
                    </div>
                </div>
            </section>
            <!--sign off section -->

        </div>
        <!--home tab-->
        <div id="crForm" class="tab-pane fade">
            <section>
                <!--Estimate-->
                <div class="row">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label class="control-label col-sm-2" for="estimateSelect">Estimate Available:</label>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="estimateDropDown" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="0" Selected="False" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="1" Selected="True" Text="No"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <section>
                <!-- deptMB-->
                <div class="row">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label class="control-label col-sm-2" for="deptMb">Department MB:</label>
                            <div class="col-sm-2">
                                <%-- <asp:TextBox ID="deptTxtBox" CssClass="form-control" runat="server"></asp:TextBox>--%>
                                <asp:DropDownList ID="DeptDropDown" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="0" Selected="False" Text=""></asp:ListItem>
                                    <asp:ListItem Value="1" Selected="False" Text="Consultant"></asp:ListItem>
                                    <asp:ListItem Value="2" Selected="False" Text="Development"></asp:ListItem>
                                    <asp:ListItem Value="3" Selected="False" Text="Project Manager"></asp:ListItem>
                                    <asp:ListItem Value="4" Selected="False" Text="BI"></asp:ListItem>
                                </asp:DropDownList>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label class="control-label col-sm-2" for="timeTxt">Time (hrs):</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="timeTxt" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="timeTxt" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="^[0-9]\d*(\.\d+)?$" ForeColor="Red"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <section>
                <!--Reason Table-->
                <div class="row voffset">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label class="control-label col-sm-2" for="reasonTable">Reason:</label>
                            <div class="col-sm-10">
                                <div class="table-bordered form control">
                                    <dx:ASPxGridView ID="reasonGrid" runat="server" Width="100%" KeyFieldName="ID" SettingsBehavior-AllowSelectByRowClick="True" SettingsBehavior-AllowSelectSingleRowOnly="True" OnRowUpdating="reasonGrid_RowUpdating" OnRowInserting="reasonGrid_RowInserting" OnRowDeleted="reasonGrid_RowDeleted" SettingsCommandButton-DeleteButton-ButtonType="Button" SettingsCommandButton-NewButton-ButtonType="Button" Theme="Moderno" SettingsCommandButton-EditButton-ButtonType="Button">
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowDeleteButton="true" ShowEditButton="true" />
                                            <dx:GridViewDataTextColumn VisibleIndex="0" Caption="ID" FieldName="ID" Visible="false"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn VisibleIndex="1" Caption="Point" FieldName="Point"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn VisibleIndex="2" Caption="Description" FieldName="Description"></dx:GridViewDataTextColumn>
                                        </Columns>
                                        <ClientSideEvents RowClick="function(s,e) { s.StartEditRow(e.visibleIndex); }" />
                                    </dx:ASPxGridView>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </section>
            <!-- reason table -->

            <section>
                <div class="col-sm-1">
                </div>
                <div class="col-sm-11">
                    <div class="panel panel-danger">
                        <div class="panel-heading"><b>Please Note:</b></div>
                        <div class="panel-body"><b>Any CR will have a likely impact on project timeline and will need to be evaluated against project plan</b></div>
                    </div>
                </div>
            </section>
            <!-- notification panel section -->
            <section>
                <div class="row">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label class="control-label col-sm-2" for="custAuthSelect">Customer Authorisation:</label>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="custAuthSelect" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="0" Selected="False">Yes</asp:ListItem>
                                    <asp:ListItem Value="1" Selected="True">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <section>
                <!--sign off section-->
                <div class="row">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <label class="control-label col-sm-2" for="crSignOff">Sign Off</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="crContactName" CssClass="form-control" runat="server" placeholder="Contact Name"></asp:TextBox>
                                <%-- <input id="contactName" class="form-control" type="text" placeholder="Contact Name" />--%>
                            </div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="crContactEmail" CssClass="form-control" TextMode="email" placeholder="Contact Email" runat="server"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server"
                              ControlToValidate="crContactEmail"
                              ErrorMessage="Contact Email is a required field."
                              ForeColor="Red" SetFocusOnError="True">
                            </asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="form-horizontal" role="form">
                        <div class="col-sm-2">
                        </div>
                        <div class="col-sm-8">
                            <div id="crSig" class="form-group">
                            </div>
                            <asp:HiddenField ID="crSigDataNative" runat="server" Value="" />
                            <asp:HiddenField ID="crSigDataBitmap" runat="server" Value="" />
                        </div>
                        <div class="col-sm-1">
                            <dx:ASPxButton ID="ResetBtn2" CssClass="btn btn-primary voffset" runat="server" Text="Reset" BackgroundImage-ImageUrl='""' Font-Size="Medium" HorizontalAlign="Center">
                                <ClientSideEvents Click="function(s, e) { Reset2(); e.processOnServer = false;}" />
                            </dx:ASPxButton>
                        </div>
                        <div class="col-sm-1">
                        </div>
                    </div>
                </div>
            </section>
            <!--sign off section -->
        </div>
        <!--CR Form tab-->
        <div id="checkList" class="tab-pane fade">
            <section>
                <h2>Checklist</h2>
                <div class="row voffset">
                    <div class=" col-sm-3">
                        <div class="panel panel-default">
                            <div class="panel-heading" align="center">
                                <asp:FileUpload ID="FileUpload" runat="server" />
                            </div>
                            <div class="panel-body">
                                <dx:ASPxButton runat="server" ID="UploadBtn" CssClass="btn btn-link" BackgroundImage-ImageUrl='""' Text="Upload Excel File" OnClick="UploadBtn_Click"></dx:ASPxButton>
                                <br />
                                <dx:ASPxLabel runat="server" ID="StatusLbl" Text="Upload Status: " Visible="false"></dx:ASPxLabel>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row voffset">
                    <div class="form-horizontal" role="form">
                        <div class="form-group">
                            <div class="col-sm-12">
                                <dx:ASPxGridView ID="ChecklistGV" runat="server" KeyFieldName="ID" Theme="Moderno" SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectSingleRowOnly="true" SettingsBehavior-AllowSort="true" SettingsDataSecurity-AllowEdit="true" Width="100%" SettingsBehavior-AllowSelectByRowClick="True" SettingsSearchPanel-ShowClearButton="True" Settings-ShowFilterRow="True" SettingsCommandButton-NewButton-ButtonType="Button" OnRowInserting="ChecklistGV_RowInserting" OnRowUpdating="ChecklistGV_RowUpdating" OnRowDeleted="ChecklistGV_RowDeleted">
                                    <Columns>
                                        <dx:GridViewCommandColumn ShowDeleteButton="true" ShowEditButton="true" ShowNewButtonInHeader="true"></dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn FieldName="ID" VisibleIndex="-1" Visible="false"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Area" VisibleIndex="0"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Paragraph" VisibleIndex="1"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Action" VisibleIndex="2"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Priority" VisibleIndex="3"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ClientAssign" Caption="Client" VisibleIndex="4"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="MBAssign" Caption="Employee" VisibleIndex="5"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Complete" VisibleIndex="6"></dx:GridViewDataTextColumn>
                                        <%--<dx:GridViewDataTextColumn Caption="Complete" FieldName="Complete" VisibleIndex="6">
                                            <DataItemTemplate>
                                                <dx:ASPxCheckBox ID="cb" runat="server" OnInit="cb_Init" Checked='<%# GetChecked(Eval("Complete").ToString()) %>' ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false"  OnCheckedChanged="cb_CheckedChanged" />
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>--%>
                                        <dx:GridViewDataTextColumn FieldName="DateFinished" Caption="Finished Date" VisibleIndex="7"></dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Comments" VisibleIndex="8"></dx:GridViewDataTextColumn>

                                    </Columns>
                                </dx:ASPxGridView>

                            </div>
                        </div>
                    </div>
                </div>
            </section>
            <div class="row">
                <div class="col-sm-2">
                    <dx:ASPxButton ID="ExportBtn" runat="server" Text="Export to Excel" CssClass="btn btn-primary" BackgroundImage-ImageUrl='""' OnClick="ExportBtn_Click"></dx:ASPxButton>
                </div>
            </div>
            <!-- checklist table -->
        </div>
    </div>
    <section>
        <!--Save Buttons-->

        <div class="row">
            <div class="col-sm-7">
            </div>
            <div class="col-sm-2">
                <dx:ASPxButton ID="SaveDraftBtn" CssClass="btn btn-primary" runat="server" Text="Save Draft" OnClick="SaveDraft_Click" BackgroundImage-ImageUrl='""' Font-Size="Medium" Width="100%">
                    <ClientSideEvents Click="function (s, e) {e.processOnServer = saveSig(true);}" />
                </dx:ASPxButton>
            </div>
            <div class="col-sm-2">
                <dx:ASPxButton ID="SaveButton" CssClass="btn btn-primary" runat="server" Text="Save & Email" OnClick="SaveButton_Click" BackgroundImage-ImageUrl='""' Font-Size="Medium" Width="100%">
                    <ClientSideEvents Click="function (s, e) {e.processOnServer = saveSig(false);}" />
                </dx:ASPxButton>
            </div>
            <div class="col-sm-1">
                <dx:ASPxButton ID="ClearBtn" CssClass="btn btn-primary" runat="server" Text="Clear" OnClick="ClearButton_Click" BackgroundImage-ImageUrl='""' Font-Size="Medium" BackColor="#91268F" Width="100%"></dx:ASPxButton>
            </div>

        </div>
    </section>
    <!--save buttons-->

    <asp:HiddenField ID="agendaField" runat="server" Value="" />
    <asp:HiddenField ID="outcomeField" runat="server" Value="" />
    <asp:HiddenField ID="reasonField" runat="server" Value="" />
    <asp:HiddenField ID="TabName" runat="server" />

    <script type="text/javascript">
        $(document).ready(function () {
            $("#wsSig").jSignature({ 'UndoButton': true, color: "#000", lineWidth: 5 });
            //$("#crSig").jSignature({ 'UndoButton': true, color: "#000", lineWidth: 5 });
            ImportJsignature();

        });
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "home";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });

        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            e.target // newly activated tab
            var getUrl = window.location;
            var baseUrl = getUrl.protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[1];

            if (e.target == baseUrl + "#crForm") {
                if ($('#crSig canvas').length < 1) {
                    $("#crSig").jSignature({ 'UndoButton': true, color: "#000", lineWidth: 5 });
                    ImportJsignature();
                }

            }

        })

        var isWs = false;
        var isCr = false;

        $("#wsSig").bind('change', function (e) {
            if ($("#wsSig").jSignature('getData', 'native').length > 0) {

                isWs = true;
            }
            else {

                isWs = false;
            }

        });
        $("#crSig").bind('change', function (e) {
            if ($("#crSig").jSignature('getData', 'native').length > 0) {

                isCr = true;
            }
            else {

                isCr = false;
            }
        });


        function saveSig(draft) {
            var signaturePad = $("#wsSig");
            var signaturePad2 = $("#crSig");
            var crContactEmail = $("#crContactEmail");
            var contactEmail = $("#contactEmail")

            if (isWs || isCr) {
                if (isWs) {
                    //var data = $("#wsSig").jSignature("getData", "base30");
                    var hiddenField = document.getElementById('<%=hdfSignatureDataNative1.ClientID %>');
                    hiddenField.value = "data:" + signaturePad.jSignature("getData", "base30").join(",");
                    hiddenField = document.getElementById('<%=hdfSignatureDataBitmap1.ClientID %>');
                    hiddenField.value = signaturePad.jSignature("getData");
                }
                if (isCr) {
                    //var data2 = $("#crSig").jSignature("getData", "base30");
                    var hiddenField2 = document.getElementById('<%=crSigDataNative.ClientID %>');
                    hiddenField2.value = "data:" + signaturePad2.jSignature("getData", "base30").join(",");
                    hiddenField2 = document.getElementById('<%=crSigDataBitmap.ClientID %>');
                    hiddenField2.value = signaturePad2.jSignature("getData");
                }

                confirmation(draft);
                return true;
            }
            else {
                if (noSigConfirm(draft)) {
                    return true;
                }
            }

        }

        function ImportJsignature() {
            var wsSignaturePad = $("#wsSig");
            var crSignaturePad = $("#crSig");

            var wsHiddenField = document.getElementById('<%=hdfSignatureDataBitmap1.ClientID %>');
            if (wsHiddenField != "")
                wsSignaturePad.jSignature("importData", wsHiddenField.value);


            var crHiddenField = document.getElementById('<%=crSigDataBitmap.ClientID %>');
             if (crHiddenField != "")
                 crSignaturePad.jSignature("importData", crHiddenField.value);
         }

         function Reset() {
             console.log('ClearSignaturePad');
             signaturePad = $("#wsSig");
             signaturePad.jSignature("clear");

             var hiddenField = document.getElementById('<%=hdfSignatureDataNative1.ClientID %>');
             hiddenField.value = "";
             hiddenField = document.getElementById('<%=hdfSignatureDataBitmap1.ClientID %>');
             hiddenField.value = "";

         }
         function Reset2() {
             console.log('ClearSignaturePad');
             signaturePad = $("#crSig");
             signaturePad.jSignature("clear");

             var hiddenField = document.getElementById('<%=crSigDataNative.ClientID %>');
            hiddenField.value = "";
            hiddenField = document.getElementById('<%=crSigDataBitmap.ClientID %>');
            hiddenField.value = "";

        }


        function noSigConfirm(draft) {
            if (draft == true) {
                if (confirm("Are you sure you want to save draft"))
                    return true;
                else return false;
            }
            else {
                if (confirm("Are you sure you want to save & send email without a Signature?"))
                    return true;
                else return false;
            }

        }

        function PopulateTable(tableVals, tableID, cellId) {
            var table = document.getElementById(tableID);
            var cellCount = 0;
            var newVals = [];
            for (var i = 0; i < tableVals.length; i++) {
                if (tableVals[i] === '[' || tableVals[i] === ']' || tableVals[i] === ',') {
                    delete tableVals[i];
                }
                else {
                    newVals.push(tableVals[i]);
                }
            }
            for (var j = 0; j < newVals.length; j++) {

                var newRow = table.insertRow(-1);
                var cell1 = newRow.insertCell(0);
                var cell2 = newRow.insertCell(1);

                cell1.innerHTML = '<span id=' + cellId + (cellCount + 1) + ' contenteditable>' + newVals[cellCount] + '</span>';
                cellCount++;
                cell2.innerHTML = '<span id=' + cellId + (cellCount + 1) + ' contenteditable>' + newVals[cellCount] + '</span>';
                cellCount++;
            }
        }
        function confirmation(draft) {
            if (draft == true) {
                if (confirm("Save data?"))
                    return true;
                else return false;
            }
            else {
                if (confirm("Save data and send email?"))
                    return true;
                else return false;
            }
        }

    </script>


</asp:Content>


