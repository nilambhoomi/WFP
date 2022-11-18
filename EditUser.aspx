<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/site.master" CodeFile="EditUser.aspx.cs" Inherits="EditUser" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .lblstyle {
            float: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">

    <div class="main-content-inner">
        <div class="page-content">
            <div class="page-header">
                <h1>
                    <small>Users Details								
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
                                    <div class="col-sm-2">
                                        <label class="lblstyle">First Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtFirstName" CssClass="form-control" placeholder="First Name" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtFirstName" Display="Dynamic" ErrorMessage="Please enter patient first name" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Middle Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtMiddleName" CssClass="form-control" placeholder="Middle Name" runat="server"></asp:TextBox>
                                    </div>
                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />
                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Last Name</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtLastName" CssClass="form-control" placeholder="Last Name" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Email</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtEmail" CssClass="form-control" placeholder="Email" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator runat="server" ID="regEmail" ControlToValidate="txtEmail" Display="Dynamic" ForeColor="Red" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />

                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">LoginID</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtLoginID" CssClass="form-control" placeholder="LoginId" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Password</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtPassowrd" TextMode="Password" CssClass="form-control" placeholder="Password" runat="server"></asp:TextBox>
                                        <asp:RegularExpressionValidator runat="server" ID="regPassword" ValidationExpression="(?=^.{8,15}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{:;'?/>.<,])(?!.*\s).*$" ControlToValidate="txtPassowrd" Display="Dynamic" 
                                            ErrorMessage="Password shoud be at least 8 chars with caps, low, nos & spl char."></asp:RegularExpressionValidator>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />

                            <div class="col-xs-12">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <label class="lblstyle">Designation</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtDesignation" CssClass="form-control" placeholder="Designation" runat="server"></asp:TextBox>
                                    </div>

                                </div>


                            </div>
                            <div class="clearfix"></div>
                            <br />

                            <div class="col-xs-12">
                                <div class="row">

                                    <div class="col-sm-2">
                                        <label class="lblstyle">&nbsp;</label>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                                            &nbsp;
                                                <asp:Button ID="btnBack" PostBackUrl="~/ManageUser.aspx" CausesValidation="false" runat="server" CssClass="btn btn-default" Text="Back" />
                                        </div>
                                    </div>

                                </div>


                            </div>
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
        $(document).ready(function () {

            $("#<%=txtDesignation.ClientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: 'EditUser.aspx/GetDesignations',
                        data: "{ 'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item,
                                    val: item
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
                               <%-- $("#<%=hfPatientId.ClientID %>").val(i.item.val);
                                $('#<%= txtSearch.ClientID %>').val(i.item.label);
                                $('#<%= btnSearch.ClientID %>').click();--%>
                },
                minLength: 1
            });

        });
    </script>

</asp:Content>
