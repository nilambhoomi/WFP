﻿<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="PatientIntakeList.aspx.cs" Inherits="PatientIntakeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Scripts/jquery-1.8.2.min.js"></script>
     <script src="js/images/bootstrap.min.js"></script>

    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <script src="https://cdn.rawgit.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script src="js/jquery-mask-1.14.8.min.js"></script>
    <script src="js/jquery.maskedinput.js"></script>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>


    <style>
        /*a.btn {
            text-decoration: none;
        }

        table {
            text-align: center;
        }

        .main-container {
            min-height: 900px;
        }*/

        .pager::before {
            display: none;
        }

        .pager table {
            margin: 0 auto;
        }

            .pager table tbody tr td a,
            .pager table tbody tr td span {
                position: relative;
                float: left;
                padding: 6px 12px;
                margin-left: -1px;
                line-height: 1.42857143;
                color: #337ab7;
                text-decoration: none;
                background-color: #fff;
                border: 1px solid #ddd;
            }

            .pager table > tbody > tr > td > span {
                z-index: 3;
                color: #fff;
                cursor: default;
                background-color: #337ab7;
                border-color: #337ab7;
            }

            .pager table > tbody > tr > td:first-child > a,
            .pager table > tbody > tr > td:first-child > span {
                margin-left: 0;
                border-top-left-radius: 4px;
                border-bottom-left-radius: 4px;
            }

            .pager table > tbody > tr > td:last-child > a,
            .pager table > tbody > tr > td:last-child > span {
                border-top-right-radius: 4px;
                border-bottom-right-radius: 4px;
            }

            .pager table > tbody > tr > td > a:hover,
            .pager table > tbody > tr > td > span:hover,
            .pager table > tbody > tr > td > a:focus,
            .pager table > tbody > tr > td > span:focus {
                z-index: 2;
                color: #23527c;
                background-color: #eee;
                border-color: #ddd;
            }

        .modal {
            width: 100%;
        }

        .modal-dialog {
            width: 1000px;
            overflow-y: initial !important;
        }

        .modal-body {
            width: 1000px;
            height: 650px;
            overflow-y: auto;
        }

        .chkChoice input {
            margin-right: 8px;
        }

        .chkChoice td {
            padding-left: 5px;
        }
        /*input[type="radio"] {*/
        /*-webkit-appearance: checkbox;*/ /* Chrome, Safari, Opera */
        /*-moz-appearance: checkbox;*/ /* Firefox */
        /*-ms-appearance: checkbox;*/ /* not currently supported */
        /*}*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">
    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Patient Details								
									<i class="ace-icon fa fa-angle-double-right"></i>

                    </small>
                </h1>
            </div>


            <div class="">

                <div class="row">
                    <div class="col-xs-12">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtSearch" CssClass="form-control" placeholder="Search" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-success" Text="Search" OnClick="btnSearch_Click" />
                                        <asp:Button ID="btnRefresh" runat="server" CssClass="btn btn-success" Text="Refresh" OnClick="btnRefresh_Click" />
                                        <asp:HiddenField ID="hfPatientId" runat="server"></asp:HiddenField>
                                    </div>
                                    <div class="col-sm-6" style="float: right">
                                        <asp:DropDownList runat="server" ID="ddlPage" AutoPostBack="true" Style="float: right; width: 70px" CssClass="form-control" OnSelectedIndexChanged="ddlPage_SelectedIndexChanged">
                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="50" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="100" Value="40"></asp:ListItem>
                                            <%--   <asp:ListItem Text="All" Value="0"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="space"></div>
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvPatientDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" DataKeyNames="PatientIE_ID" OnRowDataBound="OnRowDataBound" AllowPaging="True" OnPageIndexChanging="gvPatientDetails_PageIndexChanging1" PagerStyle-CssClass="pager">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Image alt="" title='<%# Eval("PatientIE_ID") %>' runat="server" ID="plusimg" Style="cursor: pointer" ImageUrl="img/plus.png" />
                                                    <%-- <img alt="" title='<%# Eval("PatientIE_ID") %>' runat="server" id="plusimg" style="cursor: pointer" src="img/plus.png" />--%>
                                                    <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                                                        <asp:GridView ID="gvPatientFUDetails" BorderStyle="None" CssClass="table table-bordered" Width="100%" runat="server" AllowPaging="True" OnPageIndexChanging="gvPatientFUDetails_PageIndexChanging" AutoGenerateColumns="False" EmptyDataText="No Records Found" PagerStyle-CssClass="pager">
                                                            <Columns>
                                                                <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:d}" />
                                                                <asp:BoundField DataField="Location" HeaderText="Location" />
                                                                <asp:BoundField DataField="MAProviders" HeaderText="MA & Providers" />
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>

                                                                        <%--<asp:HyperLink runat="server" CssClass="btn btn-info" ID="hlAddFU" NavigateUrl='<%# "~/TimeSheet.aspx?PId="+Eval("PatientIEId")+"&FID="+Eval("PatientFUId")  %>' Text="Procedure Details"></asp:HyperLink>--%>
                                                                        <asp:HyperLink runat="server" CssClass="btn btn-info" ID="HyperLink1" NavigateUrl='<%# "~/EditFU.aspx?FUID="+Eval("PatientFUId") %>' Text="Edit"></asp:HyperLink>
                                                                        |
                                                                        <asp:HyperLink runat="server" CssClass="btn btn-link PrintClick" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="lkbtnReprint" Text="Print"></asp:HyperLink>
                                                                        | 
																<%--<asp:HyperLink runat="server" CssClass="btn btn-info" ID="HyperLink3" NavigateUrl='<%# "~/EditFU.aspx?FUID="+Eval("PatientFUId") %>' Text="Edit"></asp:HyperLink>--%>
                                                                        <asp:HyperLink runat="server" CssClass="btn btn-link PrintClick" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="HyperLink2" Text='<%# Eval("PrintStatus").ToString() %>'></asp:HyperLink>
                                                                        |
                                                                         <asp:LinkButton runat="server" ID="lnkfurod" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientFUId")+ "-" +Eval("PatientIEId")+"~" +Convert.ToString(Eval("MAProviders")) %>' Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("rodid")))?"Create RoD":"Edit RoD" %>' OnClick="lnkfurod_Click"></asp:LinkButton>


                                                                        <%--<asp:LinkButton runat="server" ID="lnkfusoap" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientFUId")+ "-" +Eval("PatientIEId") %>' Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("SoapID")))?"SOAP":"SOAP" %>' OnClick="lnkfusoap_Click"></asp:LinkButton>--%>
                                                                        <asp:LinkButton runat="server" ID="lnkfusoap" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientFUId")+ "|" +Eval("PatientIEId") %>' Text="SOAP" OnClick="lnkfusoap_Click1"></asp:LinkButton>

                                                                        <%-- <asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="HyperLink3" Text="Print ROD"></asp:HyperLink>--%>

                                                                        <%--<asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="HyperLink4" Text='DownloadRoD'></asp:HyperLink>--%>
                                                                        <asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="HyperLink3" Text='<%# Eval("PrintStatusRod").ToString().Equals("Print Requested")? "" : Eval("PrintStatusRod").ToString().Equals("Printing")?"Printing Rod":Eval("PrintStatusRod").ToString().Equals("Download")? "dl RoD":Eval("PrintStatusRod").ToString()  %>'></asp:HyperLink>
                                                                        <%-- <asp:HyperLink runat="server" CssClass="btn btn-link PrintClickSoap" data-id='<%# Eval("PatientFUId") %>' data-FUIE="FU" ID="HyperLink5" Text='<%# Eval("PrintStatusSoap").ToString().Equals("Print Requested")? "" : Eval("PrintStatusSoap").ToString().Equals("Printing")?"Printing Soap":Eval("PrintStatusSoap").ToString().Equals("Download")? "dl Soap":Eval("PrintStatusSoap").ToString()  %>'></asp:HyperLink>--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>

                                                            <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                                            <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                                        </asp:GridView>
                                                        <%--<asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" CssClass = "ChildGrid">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="150px" DataField="OrderId" HeaderText="Order Id" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="OrderDate" HeaderText="Date" />
                        </Columns>
                    </asp:GridView>--%>
                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="Sex" HeaderText="Title" />
                                            <asp:BoundField DataField="lastname" HeaderText="LastName" />
                                            <asp:BoundField DataField="firstname" HeaderText="FirstName" />
                                            <asp:BoundField DataField="DOB" HeaderText="DOB" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="DOA" HeaderText="DOA" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:d}" />
                                            <asp:BoundField DataField="Compensation" HeaderText="Case Type" />
                                            <asp:BoundField DataField="location" HeaderText="Location" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HyperLink runat="server" CssClass="btn btn-link" ID="hlEdit" NavigateUrl='<%# "~/Page1.aspx?id="+Eval("PatientIE_ID") %>' Text="Edit IE">
                                      
                                                    </asp:HyperLink>
                                                    | 
              <asp:HyperLink runat="server" CssClass="btn btn-link" ID="hlAddFU" NavigateUrl='<%# "~/AddFU.aspx?PID="+Eval("PatientIE_ID") %>' Text="AddFU"></asp:HyperLink>
                                                    | 
              <asp:HyperLink runat="server" CssClass="btn btn-link PrintClick" data-id='<%# Eval("PatientIE_ID") %>' data-FUIE="IE" ID="lkbtnReprint" Text="Print"></asp:HyperLink>
                                                    |
              <asp:HyperLink runat="server" CssClass="btn btn-link PrintClick" data-id='<%# Eval("PatientIE_ID") %>' data-FUIE="IE" ID="HyperLink2" Text='<%# Eval("PrintStatus").ToString() %>'></asp:HyperLink>
                                                    |
                                                    
                                                    <asp:LinkButton runat="server" ID="lnkierod" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientIE_ID")+"-"+Convert.ToString(Eval("MA_Providers")) %>' Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("rodid")))?"Create RoD":"Edit RoD" %>' OnClick="lnkierod_Click"></asp:LinkButton>
                                                    <%--<asp:LinkButton runat="server" ID="lnkiesoap" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientIE_ID") %>' OnClick="lnkiesoap_Click" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("SoapID")))?"SOAP":"SOAP" %>'></asp:LinkButton>--%>

                                                    <asp:LinkButton runat="server" ID="lnkiesoap" CssClass="btn btn-link" CommandArgument='<%# Eval("PatientIE_ID") %>' OnClick="lnkiesoap_Click1" Text="SOAP"></asp:LinkButton>

                                                    <%--<asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientIE_ID") %>' data-FUIE="IE" ID="HyperLink5" Text="Print RoD"></asp:HyperLink>--%>

                                                    <%--<asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientIE_ID") %>' data-FUIE="IE" ID="HyperLink6" Text='DownloadRoD'></asp:HyperLink>--%>
                                                    <asp:HyperLink runat="server" CssClass="btn btn-link PrintClickRod" data-id='<%# Eval("PatientIE_ID") %>' data-FUIE="IE" ID="HyperLink4" Text='<%# Eval("PrintStatusRod").ToString().Equals("Print Requested")? "" : Eval("PrintStatusRod").ToString().Equals("Printing")?"Printing Rod":Eval("PrintStatusRod").ToString().Equals("Download")? "dl RoD":Eval("PrintStatusRod").ToString()  %>'></asp:HyperLink>


                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%-- <asp:BoundField ItemStyle-Width="150px" DataField="ContactName" HeaderText="Contact Name" />
            <asp:BoundField ItemStyle-Width="150px" DataField="City" HeaderText="City" />--%>
                                        </Columns>
                                        <PagerSettings PageButtonCount="5" />

                                        <PagerStyle CssClass="pager"></PagerStyle>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <div class="modal fade" id="RodPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; max-height: 750px;" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog" style="background: white">
                                <div class="modal-content">
                                    <div class="modal-header" style="display: inline-block; width: 100%;">
                                        Select ROD 
                                        <div style="display: inline-block; width: 80%; text-align: center;">
                                            <asp:Button ID="btnrodsave" CssClass="btn btn-success" Style="margin-left: 15px; display: none" runat="server" OnClick="btnrodsave_Click" Text="Save" />
                                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                            <asp:Literal ID="ltrprint" runat="server"></asp:Literal>
                                            <asp:Literal ID="ltrdownload" runat="server"></asp:Literal>
                                            <asp:Button ID="btnRODDelete" Text="Delete" OnClick="btnRODDelete_Click" runat="server" CssClass="btn btn-danger" />
                                        </div>
                                        <button type="button" class="close" style="float: right" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    </div>
                                    <div class="modal-body">
                                        <asp:UpdatePanel runat="server" ID="upRod">
                                            <ContentTemplate>
                                                <div class="col-md-9 inline">
                                                    Create Date:
                                                    <asp:TextBox ID="txtrodcreatedate" Style="display: inline-block" Width="100px" OnServerValidate="CustomValidator1_ServerValidate" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:Repeater runat="server" ID="repRoD">
                                                        <HeaderTemplate>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td colspan="2"><b>ROD:</b> </td>
                                                                </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <tr>
                                                                <td style="width: 20px">
                                                                    <asp:CheckBox runat="server" OnCheckedChanged="chk_CheckedChanged" ID="chk" AutoPostBack="true" Checked='<%# Convert.ToBoolean(Eval("isChecked")) %>' />
                                                                </td>
                                                                <td>
                                                                    <asp:HiddenField ID="bodypart" Value='<%# Eval("bodypart") %>' runat="server" />
                                                                    <asp:HiddenField ID="isnewline" Value='<%# Eval("isnewline") %>' runat="server" />
                                                                    <%--<asp:TextBox runat="server" OnTextChanged="txtRod_TextChanged" TextMode="MultiLine" AutoPostBack="true" ID="txtRod" Text='<%# Eval("name") %>' Width='138%' /></td>--%>
                                                                    <%--iCounter == 14 ||--%>
                                                                    <% if (iCounter == 1 || iCounter == 16)
                                                                       { %>
                                                                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txtRod" OnTextChanged="txtRod_TextChanged" AutoPostBack="true" Text='<%# Eval("name") %>' Columns="100" Rows="7" />
                                                                    <%}
                                                                       else
                                                                       { %>
                                                                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txtRod1" OnTextChanged="txtRod_TextChanged" AutoPostBack="true" Text='<%# Eval("name") %>' Columns="100" Rows="1" />
                                                                    <%}
                                                                       iCounter++;
                                                                    %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                    <div style="display: none">
                                                        <asp:TextBox runat="server" ID="txtrodFulldetails" Style="display: none"></asp:TextBox>
                                                        <asp:HiddenField runat="server" ID="hdbodyparts" />
                                                        <asp:HiddenField runat="server" ID="hdnewline" />
                                                    </div>
                                                </div>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:Label Columns="20" runat="server">Provider:</asp:Label>
                                         <%--  <asp:TextBox runat="server" ID="txtMAProviderrod"></asp:TextBox>--%>
                                        <asp:DropDownList runat="server" ID="txtMAProviderrod" Width="190px">
                                        </asp:DropDownList>
                                        <div class="modal-footer" style="display: inline-block; width: 100%; text-align: center;">
                                            <asp:Button ID="Button1" CssClass="btn btn-success" Style="margin-left: 15px" runat="server" OnClick="btnrodsave_Click" Text="Save" />
                                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="hdnrodieid" runat="server" />
                        <asp:HiddenField ID="hdnrodeditedfuid" runat="server" />
                        <asp:HiddenField ID="hdnrodeditedfuieid" runat="server" />
                        <asp:HiddenField ID="hfCurrentlyOpened" runat="server"></asp:HiddenField>
                        <asp:HiddenField ID="hdnSoapId" runat="server" />

                        <div class="modal fade" id="SoapPopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; max-height: 850px;" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog" style="background: white">
                                <div class="modal-content" style="width: 1200px;">
                                    <div class="modal-header" style="display: inline-block; width: 100%;">
                                        <center><b>SOAP</b></center>
                                        <button type="button" class="close" style="float: right" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    </div>
                                    <div class="modal-body" style="width: 100%;">
                                        <div class="col-md-12 inline">
                                            <div class="row">
                                                <div class="col-lg-4">
                                                    <b>Name :</b><asp:Label ID="lblName" runat="server"></asp:Label>

                                                </div>
                                                <div class="col-lg-4">
                                                    <b>DOI :</b><asp:Label ID="lblDOI" runat="server"></asp:Label>
                                                </div>
                                                <div class="col-lg-4">
                                                    <b>DOS :</b><asp:TextBox ID="txtCreateSoapDate" Style="display: inline-block" Width="100px" OnServerValidate="CustomValidator1_ServerValidate" runat="server" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-lg-12">
                                                <b>Subjective:</b><br />
                                                <asp:TextBox TextMode="MultiLine" Rows="7" Columns="100" runat="server" ID="txtAdjective"></asp:TextBox>
                                            </div>

                                            <div><b>Objective :</b></div>
                                            <%--Body Parts Start--%>
                                            <div class="col-lg-4" id="dvbp_Neck" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>C/S :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseflexionNeck" runat="server" onclick="fnCheckOne(this);" groupname="flexionNeck" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseflexionNeck" runat="server" onclick="fnCheckOne(this);" groupname="flexionNeck" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>extension</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseextensionNeck" runat="server" onclick="fnCheckOne(this);" groupname="extensionNeck" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseextensionNeck" runat="server" onclick="fnCheckOne(this);" groupname="extensionNeck" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>tilt to left</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasetilttoleftNeck" runat="server" onclick="fnCheckOne(this);" groupname="tiltToLeftNeck" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasetilttoleftNeck" runat="server" onclick="fnCheckOne(this);" groupname="tiltToLeftNeck" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>tilt to right</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasetilttorightNeck" runat="server" onclick="fnCheckOne(this);" groupname="tilttoright" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasetilttorightNeck" runat="server" onclick="fnCheckOne(this);" groupname="tilttoright" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>left rotation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseleftrotationNeck" runat="server" onclick="fnCheckOne(this);" groupname="leftrotation" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseleftrotationNeck" runat="server" onclick="fnCheckOne(this);" groupname="leftrotation" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Right rotation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseRightrotationNeck" runat="server" onclick="fnCheckOne(this);" groupname="Rightrotation" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseRightrotationNeck" runat="server" onclick="fnCheckOne(this);" groupname="Rightrotation" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkspasmNeck" runat="server" Checked="true" Text="spasm present" /></td>
                                                                <td colspan="2"></td>
                                                                <%--<td>
                                                                    <asp:CheckBox ID="chkDecreasespasmNeck" runat="server" onclick="fnCheckOne(this);" groupname="spasm" /></td>--%>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_LowBack" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>L/S :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseflexionLumber" runat="server" onclick="fnCheckOne(this);" groupname="flexionLumber" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseflexionLumber" runat="server" onclick="fnCheckOne(this);" groupname="flexionLumber" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>extension</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseextensionLumber" runat="server" onclick="fnCheckOne(this);" groupname="extensionLumber" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseextensionLumber" runat="server" onclick="fnCheckOne(this);" groupname="extensionLumber" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>tilt to left</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasetilttoleftLumber" runat="server" onclick="fnCheckOne(this);" groupname="tilttoleftLumber" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasetilttoleftLumber" runat="server" onclick="fnCheckOne(this);" groupname="tilttoleftLumber" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>tilt to right</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasetilttorightLumber" runat="server" onclick="fnCheckOne(this);" groupname="tilttorightLumber" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasetilttorightLumber" runat="server" onclick="fnCheckOne(this);" groupname="tilttorightLumber" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkspasmLumber" runat="server" Checked="true" Text="spasm present" />
                                                                </td>
                                                                <td colspan="2"></td>
                                                                <%--<td>
                                                                    <asp:CheckBox ID="chkDecreasespasmLumber" runat="server" onclick="fnCheckOne(this);" groupname="spasmLumber" /></td>--%>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_MidBack" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>T/S :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>mildly</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasemildlyTrousic" runat="server" onclick="fnCheckOne(this);" groupname="mildlyTrousic" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasemildlyTrousic" runat="server" onclick="fnCheckOne(this);" groupname="mildlyTrousic" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>moderately</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasemoderatelyTrousic" runat="server" onclick="fnCheckOne(this);" groupname="moderatelyTrousic" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasemoderatelyTrousic" runat="server" onclick="fnCheckOne(this);" groupname="moderatelyTrousic" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>severely</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkincreaseseverelyTrousic" runat="server" onclick="fnCheckOne(this);" groupname="severelyTrousic" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseseverelyTrousic" runat="server" onclick="fnCheckOne(this);" groupname="severelyTrousic" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="chkspasmTrousic" runat="server" Checked="true" Text="spasm present" /></td>
                                                                <td colspan="2"></td>
                                                                <%-- <td>
                                                                    <asp:CheckBox ID="chkDecreasepasmTrousic" runat="server" onclick="fnCheckOne(this);" groupname="spasmTrousic" /></td>--%>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_RightElbow" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>R. Elbow :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseflexionRE" runat="server" onclick="fnCheckOne(this);" groupname="flexionRE" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseflexionRE" runat="server" onclick="fnCheckOne(this);" groupname="flexionRE" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>extension</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseextensionRE" runat="server" onclick="fnCheckOne(this);" groupname="extensionRE" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseextensionRE" runat="server" onclick="fnCheckOne(this);" groupname="extensionRE" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_LeftElbow" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>L. Elbow :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseflexionLE" runat="server" onclick="fnCheckOne(this);" groupname="flexionLE" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseflexionLE" runat="server" onclick="fnCheckOne(this);" groupname="flexionLE" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>extension</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseextensionLE" runat="server" onclick="fnCheckOne(this);" groupname="extensionLE" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseextensionLE" runat="server" onclick="fnCheckOne(this);" groupname="extensionLE" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_RightKnee" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>R. Knee :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseflexionRK" runat="server" onclick="fnCheckOne(this);" groupname="flexionRK" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseflexionRK" runat="server" onclick="fnCheckOne(this);" groupname="flexionRK" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>extension</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseextensionRK" runat="server" onclick="fnCheckOne(this);" groupname="extensionRK" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseextensionRK" runat="server" onclick="fnCheckOne(this);" groupname="extensionRK" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_LeftKnee" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>L. Knee :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseflexionLK" runat="server" onclick="fnCheckOne(this);" groupname="flexionLK" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseflexionLK" runat="server" onclick="fnCheckOne(this);" groupname="flexionLK" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>extension</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseextensionLK" runat="server" onclick="fnCheckOne(this);" groupname="extensionLK" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseextensionLK" runat="server" onclick="fnCheckOne(this);" groupname="extensionLK" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_RightShoulder" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>R. Shoulder :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseflexionRSh" runat="server" onclick="fnCheckOne(this);" groupname="flexionRSh" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseflexionRSh" runat="server" onclick="fnCheckOne(this);" groupname="flexionRSh" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>abduction</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseabductionRSh" runat="server" onclick="fnCheckOne(this);" groupname="abductionRSh" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseabductionRSh" runat="server" onclick="fnCheckOne(this);" groupname="abductionRSh" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>int. rotation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseintrotationRSh" runat="server" onclick="fnCheckOne(this);" groupname="introtationRSh" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseintrotationRSh" runat="server" onclick="fnCheckOne(this);" groupname="introtationRSh" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>ext. rotation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseextrotationRSh" runat="server" onclick="fnCheckOne(this);" groupname="extrotationRSh" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseextrotationRSh" runat="server" onclick="fnCheckOne(this);" groupname="extrotationRSh" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_LeftShoulder" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>L. Shoulder :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseflexionLSh" runat="server" onclick="fnCheckOne(this);" groupname="flexionLSh" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseflexionLSh" runat="server" onclick="fnCheckOne(this);" groupname="flexionLSh" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>abduction</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseabductionLSh" runat="server" onclick="fnCheckOne(this);" groupname="abductionLSh" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseabductionLSh" runat="server" onclick="fnCheckOne(this);" groupname="abductionLSh" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>int. rotation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseintrotationLSh" runat="server" onclick="fnCheckOne(this);" groupname="introtationLSh" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseintrotationLSh" runat="server" onclick="fnCheckOne(this);" groupname="introtationLSh" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>ext. rotation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseextrotationLSh" runat="server" onclick="fnCheckOne(this);" groupname="extrotationLSh" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseextrotationLSh" runat="server" onclick="fnCheckOne(this);" groupname="extrotationLSh" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_RightWrist" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>R. Wrist :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>palmar flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasepalmarflexionRW" runat="server" onclick="fnCheckOne(this);" groupname="palmarflexionRW" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasepalmarflexionRW" runat="server" onclick="fnCheckOne(this);" groupname="palmarflexionRW" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>dorsiflexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasedorsiflexionRW" runat="server" onclick="fnCheckOne(this);" groupname="dorsiflexionRW" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasedorsiflexionRW" runat="server" onclick="fnCheckOne(this);" groupname="dorsiflexionRW" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>ulnar deviation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseulnardeviationRW" runat="server" onclick="fnCheckOne(this);" groupname="ulnardeviationRW" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseulnardeviationRW" runat="server" onclick="fnCheckOne(this);" groupname="ulnardeviationRW" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>radial deviation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseradialdeviationRW" runat="server" onclick="fnCheckOne(this);" groupname="radialdeviationRW" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseradialdeviationRW" runat="server" onclick="fnCheckOne(this);" groupname="radialdeviationRW" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_LeftWrist" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>L. Wrist :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>palmar flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasepalmarflexionLW" runat="server" onclick="fnCheckOne(this);" groupname="palmarflexionLW" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasepalmarflexionLW" runat="server" onclick="fnCheckOne(this);" groupname="palmarflexionLW" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>dorsiflexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasedorsiflexionLW" runat="server" onclick="fnCheckOne(this);" groupname="dorsiflexionLW" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasedorsiflexionLW" runat="server" onclick="fnCheckOne(this);" groupname="dorsiflexionLW" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>ulnar deviation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseulnardeviationLW" runat="server" onclick="fnCheckOne(this);" groupname="ulnardeviationLW" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseulnardeviationLW" runat="server" onclick="fnCheckOne(this);" groupname="ulnardeviationLW" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>radial deviation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseradialdeviationLW" runat="server" onclick="fnCheckOne(this);" groupname="radialdeviationLW" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseradialdeviationLW" runat="server" onclick="fnCheckOne(this);" groupname="radialdeviationLW" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_RightHip" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>R. Hip :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseflexionRHIP" runat="server" onclick="fnCheckOne(this);" groupname="flexionRHIP" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseflexionRHIP" runat="server" onclick="fnCheckOne(this);" groupname="flexionRHIP" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>extension</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseextensionRHIP" runat="server" onclick="fnCheckOne(this);" groupname="extensionRHIP" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseextensionRHIP" runat="server" onclick="fnCheckOne(this);" groupname="extensionRHIP" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>abduction</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseabductionRHIP" runat="server" onclick="fnCheckOne(this);" groupname="abductionRHIP" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseabductionRHIP" runat="server" onclick="fnCheckOne(this);" groupname="abductionRHIP" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>int. rotation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseintrotationRHIP" runat="server" onclick="fnCheckOne(this);" groupname="introtationRHIP" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseintrotationRHIP" runat="server" onclick="fnCheckOne(this);" groupname="introtationRHIP" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_LeftHip" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>L. Hip :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseflexionLHIP" runat="server" onclick="fnCheckOne(this);" groupname="flexionLHIP" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseflexionLHIP" runat="server" onclick="fnCheckOne(this);" groupname="flexionLHIP" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>extension</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseextensionLHIP" runat="server" onclick="fnCheckOne(this);" groupname="extensionLHIP" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseextensionLHIP" runat="server" onclick="fnCheckOne(this);" groupname="extensionLHIP" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>abduction</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseabductionLHIP" runat="server" onclick="fnCheckOne(this);" groupname="abductionLHIP" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseabductionLHIP" runat="server" onclick="fnCheckOne(this);" groupname="abductionLHIP" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>int. rotation</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseintrotationLHIP" runat="server" onclick="fnCheckOne(this);" groupname="introtationLHIP" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseintrotationLHIP" runat="server" onclick="fnCheckOne(this);" groupname="introtationLHIP" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_RightAnkle" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>R. Ankle :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>dorsiflexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasedorsiflexionRA" runat="server" onclick="fnCheckOne(this);" groupname="dorsiflexionRA" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasedorsiflexionRA" runat="server" onclick="fnCheckOne(this);" groupname="dorsiflexionRA" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>plantar flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseplantarflexionRA" runat="server" onclick="fnCheckOne(this);" groupname="plantarflexionRA" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseplantarflexionRA" runat="server" onclick="fnCheckOne(this);" groupname="plantarflexionRA" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>inversion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseinversionRA" runat="server" onclick="fnCheckOne(this);" groupname="inversionRA" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseinversionRA" runat="server" onclick="fnCheckOne(this);" groupname="inversionRA" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>eversion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseeversionRA" runat="server" onclick="fnCheckOne(this);" groupname="eversionRA" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseeversionRA" runat="server" onclick="fnCheckOne(this);" groupname="eversionRA" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="col-lg-4" id="dvbp_LeftAnkle" runat="server" style="display: none">
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th>L. Ankle :</th>
                                                                <th><i class="ace-icon fa fa-arrow-up green"></i></th>
                                                                <th><i class="ace-icon fa fa-arrow-down green"></i></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td>dorsiflexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreasedorsiflexionLA" runat="server" onclick="fnCheckOne(this);" groupname="dorsiflexionLA" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreasedorsiflexionLA" runat="server" onclick="fnCheckOne(this);" groupname="dorsiflexionLA" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>plantar flexion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseplantarflexionLA" runat="server" onclick="fnCheckOne(this);" groupname="plantarflexionLA" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseplantarflexionLA" runat="server" onclick="fnCheckOne(this);" groupname="plantarflexionLA" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>inversion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseinversionLA" runat="server" onclick="fnCheckOne(this);" groupname="inversionLA" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseinversionLA" runat="server" onclick="fnCheckOne(this);" groupname="inversionLA" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>eversion</td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIncreaseeversionLA" runat="server" onclick="fnCheckOne(this);" groupname="eversionLA" /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkDecreaseeversionLA" runat="server" onclick="fnCheckOne(this);" groupname="eversionLA" /></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <%--Body Parts End--%>
                                            <div class="clearfix"></div>
                                            <div class="col-lg-12" id="dvTreatment">
                                                <b><input type="checkbox" id="treatment" name="treatment" onclick="unchecktreatment();"><label for="treatment">UnSelect All </label>Treatment:</b>
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 400px">Modalities :</th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Modalities1" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkModalities1" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Modalities2" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkModalities2" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Modalities3" runat="server"></asp:Literal>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkModalities3" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Modalities4" runat="server"></asp:Literal>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkModalities4" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <div class="clearfix"></div>
                                                <%--<div class="col-lg-12">--%>
                                                <div class="col-lg-12">
                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 400px">Manual treatment:</th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Manualtreatment1" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkManualtreatment1" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Manualtreatment2" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkManualtreatment2" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Manualtreatment3" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkManualtreatment3" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Manualtreatment4" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkManualtreatment4" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                                <%--</div>--%>
                                                <div class="clearfix"></div>
                                                <%-- <div class="col-lg-12">--%>

                                                <div class="col-lg-12">

                                                    <table class="table">
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 400px">Other:</th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Other1" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkOther1" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Other2" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkOther2" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Literal ID="Other8" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkOther8" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Other3" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkOther3" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Other4" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkOther4" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Literal ID="Other9" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkOther9" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>

                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Other5" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkOther5" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Other6" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkOther6" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Other7" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:CheckBoxList ID="chkOther7" runat="server" RepeatDirection="Horizontal" CssClass="chkChoice"></asp:CheckBoxList></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 400px">
                                                                    <asp:Literal ID="Other10" runat="server"></asp:Literal></td>
                                                                <td>
                                                                    <asp:TextBox ID="txtothertext" runat="server" Width="100%" Rows="2" TextMode="MultiLine"></asp:TextBox></td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <%--Assestement Start--%>
                                            <asp:Repeater runat="server" ID="repAssestment">
                                                <HeaderTemplate>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td colspan="2"><b>Assessment:</b> </td>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="width: 20px">
                                                            <%--OnCheckedChanged="chk_CheckedAssestment" AutoPostBack="true"--%>
                                                            <asp:CheckBox runat="server" ID="chk" Checked='<%# Convert.ToBoolean(Eval("isChecked")) %>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtAssestment" Text='<%# Eval("name") %>' Columns="100" Rows="1" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                            <asp:HiddenField ID="hdnAssestment" runat="server" />
                                            <%--Assestement End--%>
                                            <%--Plan Start--%>
                                            <asp:Repeater runat="server" ID="repPlan">
                                                <HeaderTemplate>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td colspan="2"><b>Plan:</b> </td>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="width: 20px">
                                                            <%--OnCheckedChanged="chk_CheckedPlan"--%>
                                                            <asp:CheckBox runat="server" ID="chk" Checked='<%# Convert.ToBoolean(Eval("isChecked")) %>' />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtPlan" Text='<%# Eval("name") %>' Columns="100" Rows="1" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                            <div class="col-lg-12">
                                                <b>Physical Therapist :</b>
                                                <asp:TextBox Columns="100" runat="server" ID="txtMAProvider"></asp:TextBox>
                                            </div>

                                            <asp:HiddenField ID="hdnPlan" runat="server" />
                                            <%--Plan End--%>
                                            <div style="display: none">
                                                <asp:TextBox runat="server" ID="TextBox2" Style="display: none"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="HiddenField1" />
                                                <asp:HiddenField runat="server" ID="HiddenField2" />
                                            </div>
                                            <%--   </div>--%>
                                        </div>
                                        <div class="modal-footer" style="display: inline-block; width: 100%; text-align: center;">
                                            <asp:Button ID="btnsavesoap" CssClass="btn btn-success" Style="margin-left: 15px" runat="server" OnClick="btnsavesoap_Click" Text="Save" />
                                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="modal fade" id="askPopupsoap" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-backdrop="static" data-keyboard="false">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <asp:LinkButton ID="btnCreatnew" runat="server" OnClick="btnCreatnew_Click" Visible="false" Text="New" Style="font-size: x-large; font-weight: bold;"></asp:LinkButton>
                                        <asp:LinkButton ID="btnCreatnewFu" runat="server" OnClick="btnCreatnewFu_Click" Visible="false" Text="New" Style="font-size: x-large; font-weight: bold;"></asp:LinkButton>
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    </div>
                                    <div class="modal-body" style="height: 100%!important;">
                                        <%--OnPageIndexChanging="gvPatientFUDetails_PageIndexChanging"--%>
                                        <asp:GridView ID="gvEditSoap" BorderStyle="None" CssClass="table table-bordered" runat="server" AutoGenerateColumns="False" EmptyDataText="No Records Found" PagerStyle-CssClass="pager">
                                            <Columns>
                                                <asp:BoundField DataField="DOI" HeaderText="DOI" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="DOS" HeaderText="DOS" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="MAProvider" HeaderText="Physical Therapist" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkiesoap" CssClass="btn btn-link" CommandArgument='<%# Convert.ToString(Eval("PatientIE_ID")) + "|" + Convert.ToString(Eval("ID"))  %>' Style='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PatientFU_ID"))) ? "display:inline-block": "display:none" %>' OnClick="lnkiesoap_Click" Text="Edit"></asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="lnkfuSoap" CssClass="btn btn-link" CommandArgument='<%# Convert.ToString(Eval("PatientFU_ID")) + "|" + Convert.ToString(Eval("PatientIE_ID")) +"|"+Convert.ToString(Eval("ID")) %>' Style='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PatientFU_ID"))) ? "display:none": "display:inline-block" %>' OnClick="lnkfusoap_Click" Text="Edit"></asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="soapdelete" CssClass="btn btn-link" OnClientClick="return confirm('Are you sure you want to delete this record ?')" CommandArgument='<%# "IE-"+Convert.ToString(Eval("PatientIE_ID")) + "|" + Convert.ToString(Eval("ID")) %>' OnClick="soapdelete_Click" Style='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PatientFU_ID"))) ? "display:inline-block": "display:none" %>' Text="Delete"></asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="soapdeletefu" CssClass="btn btn-link" OnClientClick="return confirm('Are you sure you want to delete this record ?')" CommandArgument='<%# "FU-" + Convert.ToString(Eval("PatientIE_ID")) + "|" + Convert.ToString(Eval("PatientFU_ID")) +"|"+Convert.ToString(Eval("ID")) %>' OnClick="soapdelete_Click" Style='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PatientFU_ID"))) ? "display:none": "display:inline-block" %>' Text="Delete"></asp:LinkButton>
                                                         <%--<asp:HyperLink runat="server" CssClass="btn btn-link PrintClickSoap" data-id='<%# Eval("PatientIE_ID") %>' data-SoapID='<%# Eval("ID") %>' data-FUIE="IE" ID="HyperLink6" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PrintStatus")))?"Print":Convert.ToString(Eval("PrintStatus")).Equals("Print Requested")?"RePrint":Convert.ToString(Eval("PrintStatus")).Equals("Downloaded")?"RePrint":Convert.ToString(Eval("PrintStatus")).Equals("Download")?"RePrint":Convert.ToString(Eval("PrintStatus")) %>' Style='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PatientFU_ID"))) ? "display:inline-block": "display:none" %>'></asp:HyperLink>
                                                        <asp:HyperLink runat="server" CssClass="btn btn-link PrintClickSoap" data-id='<%# Eval("PatientFU_ID") %>' data-SoapID='<%# Eval("ID") %>' data-FUIE="FU" ID="HyperLink5" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PrintStatus")))?"Print":Convert.ToString(Eval("PrintStatus")).Equals("Print Requested")?"RePrint":Convert.ToString(Eval("PrintStatus")).Equals("Downloaded")?"RePrint":Convert.ToString(Eval("PrintStatus")).Equals("Download")?"RePrint":Convert.ToString(Eval("PrintStatus")) %>' Style='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PatientFU_ID"))) ? "display:none": "display:inline-block" %>'></asp:HyperLink>
                                                        <asp:HyperLink runat="server" CssClass="btn btn-link PrintClickSoap" data-id='<%# Eval("PatientIE_ID") %>' data-SoapID='<%# Eval("ID") %>' data-FUIE="IE" ID="HyperLink7" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PrintStatus")))?" ":Convert.ToString(Eval("PrintStatus")).Equals("Print Requested")?" ":Convert.ToString(Eval("PrintStatus")).Equals("Downloaded")?"Downloaded":Convert.ToString(Eval("PrintStatus")).Equals("Download")?"Download":Convert.ToString(Eval("PrintStatus")) %>' Style='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PatientFU_ID"))) ? "display:inline-block": "display:none" %>'></asp:HyperLink>
                                                        <asp:HyperLink runat="server" CssClass="btn btn-link PrintClickSoap" data-id='<%# Eval("PatientFU_ID") %>' data-SoapID='<%# Eval("ID") %>' data-FUIE="FU" ID="HyperLink8" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PrintStatus")))?" ":Convert.ToString(Eval("PrintStatus")).Equals("Print Requested")?" ":Convert.ToString(Eval("PrintStatus")).Equals("Downloaded")?"Downloaded":Convert.ToString(Eval("PrintStatus")).Equals("Download")?"Download":Convert.ToString(Eval("PrintStatus")) %>' Style='<%# string.IsNullOrEmpty(Convert.ToString(Eval("PatientFU_ID"))) ? "display:none": "display:inline-block" %>'></asp:HyperLink>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:GridView>
                                        <asp:Label ID="lblRecordnotfound" runat="server" Visible="false" ForeColor="Green" Text="No Existing Soap found."></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <script src="Scripts/jquery-1.8.2.js"></script>
                <script src="Scripts/jquery-ui-1.8.24.js"></script>
                <link href="Style/jquery-ui.css" rel="stylesheet" />
                <script type="text/javascript">
                    $.noConflict();
                    function unchecktreatment() {
                        $('#dvTreatment').find('input[type=checkbox]:checked').removeAttr('checked');
                    }
                    function openModelPopupSoapEditSoap() {
                        $('#askPopupsoap').modal('show');
                    }
                    function openModelPopupSoap() {
                        $('#SoapPopup').modal('show');
                    }
                    function closeModelPopupSoap() {
                        $('#SoapPopup').modal('hide');
                    }

                    function openModelPopup() {
                        $('#RodPopup').modal('show');
                    }

                    function closeModelPopup() {
                        //jQuery.noConflict();
                        //(function ($) {

                        $('#RodPopup').modal('hide');

                        //})(jQuery);
                    }

                    var downloadPath = '<%=ConfigurationSettings.AppSettings["downloadpath"]%>';
                    $(document).ready(function () {
                        if ($('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]')) {
                            $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').closest("tr").after("<tr><td></td><td colspan = '999'>" + $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').next().html() + "</td></tr>");
                            $('[title="' + $("#<%=hfCurrentlyOpened.ClientID %>").val() + '"]').attr("src", "img/minus.png");
                        }
                        $("[src*=plus]").live("click", function () {
                            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>");
                            $(this).attr("src", "img/minus.png");
                        });

                        $("[src*=minus]").live("click", function () {
                            $(this).attr("src", "img/plus.png");
                            $(this).closest("tr").next().remove();
                        });

                        $("#<%=txtSearch.ClientID %>").autocomplete({
                            source: function (request, response) {
                                $.ajax({
                                    url: 'Search.aspx/GetPatients',
                                    data: "{ 'prefix': '" + request.term + "'}",
                                    dataType: "json",
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    success: function (data) {
                                        response($.map(data.d, function (item) {
                                            return {
                                                label: item.split('_')[0],
                                                val: item.split('_')[1]
                                            }
                                        }))
                                    },
                                    error: function (response) {
                                        alert(response.responseText);
                                    },
                                    failure: function (response) {
                                        alert(response.responseText);
                                    }
                                });
                            },
                            select: function (e, i) {
                                $("#<%=hfPatientId.ClientID %>").val(i.item.val);
                                $('#<%= txtSearch.ClientID %>').val(i.item.label);
                                $('#<%= btnSearch.ClientID %>').click();
                            },
                            minLength: 1
                        });
                    });


                    $(document).on("click", ".PrintClick", function () {
                        var currentID = this.id;
                        var obj = $(this);
                        var flag = $(this).attr("data-FUIE");
                        var id = $(this).attr("data-id");
                        var isdownload = 0;
                        if ($(this).html().toLowerCase() == "print") {
                            isdownload = 0;
                        }
                        else if ($(this).html().toLowerCase() == "print requested") {
                            alert("You already given print request.");
                            return false;
                        }
                        else if ($(this).html().toLowerCase() == "download" || $(this).html().toLowerCase() == "downloaded") {
                            isdownload = 1;
                        }
                        if (isdownload == 0) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/UpdatePrintStatus',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (currentID.indexOf("lkbtnReprint") != -1) {
                                        alert("Print Request received.")
                                        location.reload();
                                    }
                                    else {
                                        $(obj).html("Print Requested");
                                    }
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                        if (isdownload == 1) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/CheckDownload',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "") {
                                        alert("No files found to download.");
                                        return false;
                                    }
                                    var link = document.createElement("a");
                                    link.download = data.d;
                                    link.href = downloadPath + "/" + data.d + ".zip";
                                    link.click();
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                    })

                    $(document).on("click", ".PrintClickRod", function () {


                        var currentID = this.id;
                        var obj = $(this);
                        var flag = $(this).attr("data-FUIE");
                        var id = $(this).attr("data-id");
                        var isdownload = 0;
                        debugger;
                        if ($(this).html().toLowerCase() == "print") {
                            isdownload = 0;
                        }
                        else if ($(this).html().toLowerCase() == "print requested") {
                            alert("You already given print request.");
                            return false;
                        }
                        else if ($(this).html().toLowerCase() == "dl rod" || $(this).html().toLowerCase() == "download") {
                            isdownload = 1;
                        }
                        if (isdownload == 0) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/UpdatePrintStatusRod',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (currentID.indexOf("lkbtnReprint") != -1) {
                                        alert("Print Request received.")
                                        location.reload();
                                    }
                                    else {
                                        $(obj).html("Print Requested");
                                    }
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                        if (isdownload == 1) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/CheckDownloadRod',
                                data: '{"flag": "' + flag + '", "id": ' + id + '}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "") {
                                        alert("No files found to download.");
                                        return false;
                                    }
                                    var link = document.createElement("a");
                                    link.download = data.d;
                                    link.href = downloadPath + "/" + data.d + ".zip";
                                    link.click();
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                    })

                    $(document).on("click", ".PrintClickSoap", function () {

                        debugger;
                        var currentID = this.id;
                        var obj = $(this);
                        var flag = $(this).attr("data-FUIE");
                        var id = $(this).attr("data-id");
                        var Soapid = $(this).attr("data-SoapID");
                        var isdownload = 0;
                        debugger;
                        if ($(this).html().toLowerCase() == "print") {
                            isdownload = 0;
                        }
                        else if ($(this).html().toLowerCase() == "reprint") {
                            isdownload = 0;
                        }
                        else if ($(this).html().toLowerCase() == "download") {
                            isdownload = 1;
                        }
                        else if ($(this).html().toLowerCase() == "dl Soap" || $(this).html().toLowerCase() == "downloaded") {
                            isdownload = 1;
                        }
                        if (isdownload == 0) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/UpdatePrintStatusSoap',
                                data: '{"flag": "' + flag + '", "id": "' + id + '", "soapid": "' + Soapid + '"}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    //if (currentID.indexOf("lkbtnReprint") != -1) {
                                    //    alert("Print Request received.")
                                    //    location.reload();
                                    //}
                                    //else {

                                    $(obj).html("RePrint");
                                    alert("Print Request received.")
                                    location.reload();
                                    //}
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                        if (isdownload == 1) {
                            $.ajax({
                                url: 'PatientIntakeList.aspx/CheckDownloadSoap',
                                data: '{"flag": "' + flag + '", "id": "' + id + '", "soapid": "' + Soapid + '"}',
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    if (data.d == "") {
                                        alert("No files found to download.");
                                        return false;
                                    }
                                    var link = document.createElement("a");
                                    link.download = data.d;
                                    link.href = downloadPath + "/" + data.d + ".zip";
                                    link.click();
                                },
                                error: function (response) {
                                    alert(response.responseText);
                                },
                                failure: function (response) {
                                    alert(response.responseText);
                                }
                            });
                        }
                    })

                    $(document).ready(function ($) {

                        $('#<%=txtrodcreatedate.ClientID%>').mask("99/99/9999");

                        $('#<%=txtrodcreatedate.ClientID%>').datepicker({
                            changeMonth: true,
                            changeYear: true,
                            yearRange: "-100:+0",
                            onSelect: function (dateText, inst) {
                                $(this).focus();
                            }
                        });

                        $('#<%=txtCreateSoapDate.ClientID%>').mask("99/99/9999");

                        $('#<%=txtCreateSoapDate.ClientID%>').datepicker({
                            changeMonth: true,
                            changeYear: true,
                            yearRange: "-100:+0",
                            onSelect: function (dateText, inst) {
                                $(this).focus();
                            }
                        });
                    })
                    function fnCheckOne(me) {
                        var chkary = document.getElementsByTagName('input');
                        for (i = 0; i < chkary.length; i++) {

                            if (chkary[i].type == 'checkbox') {
                                debugger;
                                if (chkary[i].parentElement.getAttribute("groupname") == me.parentElement.getAttribute("groupname")) {
                                    if (chkary[i].id != me.id && chkary[i].parentElement.getAttribute("groupname") == me.parentElement.getAttribute("groupname")) {
                                        if (chkary[i].checked) {
                                            chkary[i].checked = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                </script>
            </div>
        </div>
    </div>
</asp:Content>

