<%@ Page Title="" Language="C#" MasterPageFile="~/common.master" AutoEventWireup="true" CodeFile="FUReports.aspx.cs" Inherits="FUReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        h3 {
            color: #000 !important;
            font-family: Arial !important;
        }

        label {
            font: normal 14px Arial !important;
        }

        .gvPocReport {
            border: solid 2px black;
        }

        .header {
            background-color: #646464;
            font-family: Arial;
            color: White;
            border: none 0px transparent;
            height: 25px;
            text-align: center;
            font-size: 16px;
        }

        .rows {
            background-color: #fff;
            font-family: Arial;
            font-size: 14px;
            color: #000;
            min-height: 25px;
            text-align: left;
            border: none 0px transparent;
        }

            .rows:hover {
                background-color: #ff8000;
                font-family: Arial;
                color: #fff;
                text-align: left;
            }

        .selectedrow {
            background-color: #ff8000;
            font-family: Arial;
            color: #fff;
            font-weight: bold;
            text-align: left;
        }

        .gvPocReport a /** FOR THE PAGING ICONS  **/ {
            background-color: Transparent;
            padding: 5px 5px 5px 5px;
            color: #000;
            text-decoration: none;
            font-weight: bold;
        }

            .gvPocReport a:hover /** FOR THE PAGING ICONS  HOVER STYLES**/ {
                background-color: #000;
                color: #fff;
            }

        .gvPocReport span /** FOR THE PAGING ICONS CURRENT PAGE INDICATOR **/ {
            background-color: #c9c9c9;
            color: #000;
            padding: 5px 5px 5px 5px;
        }


        .gvPocReport td {
            padding: 5px;
        }

        .gvPocReport th {
            padding: 5px;
        }

        body {
            font-family: Arial;
            font-size: 10pt;
        }

        .Pager span {
            text-align: center;
            color: #999;
            display: inline-block;
            width: 20px;
            background-color: #A1DCF2;
            margin-right: 3px;
            line-height: 150%;
            border: 1px solid #3AC0F2;
        }

        .Pager a {
            text-align: center;
            display: inline-block;
            width: 20px;
            background-color: #3AC0F2;
            color: #fff;
            border: 1px solid #3AC0F2;
            margin-right: 3px;
            line-height: 150%;
            text-decoration: none;
        }
    </style>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" />
    <script src="js/ASPSnippets_Pager.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js"></script>
    <script type="text/javascript">
        $(function () {

            $('[id*=txtFromDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "mm/dd/yyyy",
                language: "tr"
            });
            $('[id*=txtToDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "mm/dd/yyyy",
                language: "tr"
            });
        });


    </script>
    <script type="text/javascript">
        $(function () {
            GetCustomers(1);
        });
        $(".Pager .page").live("click", function () {
            GetCustomers(parseInt($(this).attr('page')));
        });
        function GetCustomers(pageIndex) {
            $.ajax({
                type: "POST",
                url: "FUReports.aspx/GetFUReport",
                data: '{pageIndex: ' + pageIndex + '}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                },
                error: function (response) {
                    alert(response.d);
                }
            });
        }

        function OnSuccess(response) {
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var customers = xml.find("Customers");
            var row = $("[id*=gvPocReport] tr:last-child").clone(true);
            $("[id*=gvPocReport] tr").not($("[id*=gvPocReport] tr:first-child")).remove();
            $.each(customers, function () {
                var customer = $(this);
                $("td", row).eq(0).html($(this).find("PatientIE_ID").text());
                $("td", row).eq(1).html($(this).find("PtName").text());
                $("td", row).eq(2).html($(this).find("CaseType").text());
                //$("td", row).eq(3).html($(this).find("Provider").text());
                $("td", row).eq(3).html($(this).find("DOE").text());
                //$("td", row).eq(4).html($(this).find("DOB").text());
                //$("td", row).eq(5).html($(this).find("DOA").text());
                //$("td", row).eq(6).html($(this).find("location").text());
                //$("td", row).eq(7).html($(this).find("MCODE").text());
                $("[id*=gvPocReport]").append(row);
                row = $("[id*=gvPocReport] tr:last-child").clone(true);
            });
            var pager = xml.find("Pager");
            $(".Pager").ASPSnippets_Pager({
                ActiveCssClass: "current",
                PagerCssClass: "pager",
                PageIndex: parseInt(pager.find("PageIndex").text()),
                PageSize: parseInt(pager.find("PageSize").text()),
                RecordCount: parseInt(pager.find("RecordCount").text())
            });
        };
    </script>
    <div style="padding-top: 50px; padding-bottom: 50px;">
        <div class="container" style="border: 1px solid black; background-color: #e9dada; padding-bottom: 50px;">
            <div class="form-inline" style="padding-top: 50px; padding-bottom: 50px;">
                <div class="form-group" style="padding-left: 10px;">
                    <label for="txtFromDate">From Date</label>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group" style="padding-left: 10px;">
                    <label for="txtToDate">To Date</label>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="form-group">

                    <asp:Button ID="Search" runat="server" CssClass="btn btn-danger" Text="Search" OnClick="Search_Click" />
                    <%--                 <asp:Button ID="Search" runat="server" CssClass="btn btn-danger" Text="Search"/>--%>
                </div>

            </div>
            <div class="form-inline">
                <div class="form-group" style="padding-right: 800px;">
                    <h3>FU Reports</h3>
                </div>
                <div class="form-group">
                    <asp:Button ID="btnExportToExcel" runat="server" CssClass="btn-info" Text="ExportToExcel" OnClick="btnExportToExcel_Click" />
                </div>

            </div>

            <asp:GridView ID="gvPocReport" runat="server" CssClass="gvLatestVisit" PagerStyle-CssClass="pager" AutoGenerateColumns="false"
                HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Width="100%" PageSize="3">
                <Columns>
                    <asp:BoundField DataField="PatientIE_ID" HeaderText="ID" />
                    <asp:BoundField DataField="PtName" HeaderText="Pt Name" />
                    <asp:BoundField DataField="CaseType" HeaderText="Case Type" />
                    <%--<asp:BoundField DataField="Provider" HeaderText="Provider" />--%>
                    <asp:BoundField DataField="DOE" HeaderText="DOE" DataFormatString="{0:MM/dd/yyyy}" />

                </Columns>
            </asp:GridView>
            <br />
            <div class="Pager"></div>
        </div>
    </div>
</asp:Content>

