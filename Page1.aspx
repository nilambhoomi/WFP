﻿<%@ Page Title="" Language="C#" MasterPageFile="~/PageMainMaster.master" AutoEventWireup="true" CodeFile="Page1.aspx.cs" Inherits="Page1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>


<asp:Content ID="Content1" style="margin-top: 90px;" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <style>
        .capitalize {
            text-transform: capitalize;
        }

        select::after {
            pointer-events: none;
        }
    </style>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <link href="css/jquery-ui-timepicker-addon.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.2.min.js"></script>

    <script src="https://cdn.rawgit.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script src="js/jquery-mask-1.14.8.min.js"></script>
    <script src="js/jquery.maskedinput.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-3-typeahead/4.0.2/bootstrap3-typeahead.js"></script>
    <script src="https://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <script src="js/jquery-ui-timepicker-addon.js"></script>


    <%--  <link href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.11.4/jquery-ui.css" rel="stylesheet" />
    <script src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script src="https://cdn.rawgit.com/igorescobar/jQuery-Mask-Plugin/master/src/jquery.mask.js"></script>
    <script src="js/jquery-mask-1.14.8.min.js"></script>
    <script src="js/jquery.maskedinput.js"></script>
    <script src="http://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>
    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <link href="css/jquery-ui-timepicker-addon.css" rel="stylesheet" />
    <script src="js/jquery-ui-timepicker-addon.js"></script>--%>

    <script>
        function getInTime() {
            var d = new Date();
            var n = d.toLocaleString([], { hour12: true });
            $('#<%= PatientIntime.ClientID %>').val(n.replace(/,/g, ""));
        }
        function getOutTime() {
            var d = new Date();
            var n = d.toLocaleString([], { hour12: true });
            $('#<%= PatientOuttime.ClientID %>').val(n.replace(/,/g, ""));
        }
    </script>
    <script>

        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
                //alert();
                //var currentURL = window.location.href;
                document.getElementById('<%=pageHDN.ClientID%>').value = $('#ctl00_' + page).attr('href');
                document.getElementById('<%= btnSave.ClientID %>').click();
            }
            else {

                window.location.href = $('#ctl00_' + page).attr('href');
            }
        }
        function saveall() {
            document.getElementById('<%= btnSave.ClientID %>').click();
        }
        function setFocus(obj) {

            // document.getElementById(obj).focus();
            $('#' + obj + '').focus();
        }
        function openPopup(divid) {

            $('#' + divid + '').modal('show');

        }
        $(function () {
            if (!$.fn.bootstrapDP && $.fn.datepicker && $.fn.datepicker.noConflict) {
                var datepicker = $.fn.datepicker.noConflict();
                $.fn.bootstrapDP = datepicker;
            }

        });
        $(document).ready(function ($) {
            var $j = jQuery.noConflict();;

            //document.getElementById("aspnetForm").reset();
            $.fn.typeahead.Constructor.prototype.select = function () {
                var val = this.$menu.find('.active').attr('data-value');
                if (val) {
                    this.$element
                        .val(this.updater(val))
                        .change();
                }
                return this.hide()
            };
            var newRender = function (items) {
                var that = this

                items = $(items).map(function (i, item) {
                    i = $(that.options.item).attr('data-value', item);
                    i.find('a').html(that.highlighter(item));
                    return i[0];
                });

                this.$menu.html(items);
                return this;
            };
            $.fn.typeahead.Constructor.prototype.render = newRender;
            $('#<%=txt_mobile.ClientID%>').mask("999-999-9999");
            $('#<%=txt_home_ph.ClientID%>').mask("999-999-9999");
            $('#<%=txt_work_ph.ClientID%>').mask("999-999-9999");
            $('#<%=txt_attorney_ph.ClientID%>').mask("999-999-9999");
            $('#<%=txt_SSN.ClientID%>').mask("999-99-9999");
            $('#<%=txt_adjuster_no.ClientID%>').mask("999-999-9999");

            //$('[id*=txt_mobile]').mask("999-999-9999")
            //$('[id*=txt_home_ph]').mask("999-999-9999")
            //$('[id*=txt_work_ph]').mask("999-999-9999")

            //$('[id*=txt_attorney_ph]').mask("999-999-9999")
            //$('[id*=txt_SSN]').mask("999-99-9999")

            //$('[id*=txt_DOB]').mask("99/99/9999")

            $j('#<%=txt_DOV.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=txt_DOB.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                yearRange: "-100:+0",
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=txt_DOA.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=PatientIntime.ClientID%>').datetimepicker({
                controlType: 'select',
                oneLine: true,
                timeFormat: 'hh:mm tt'
            });
            $j('#<%=PatientOuttime.ClientID%>').datetimepicker({
                controlType: 'select',
                oneLine: true,
                timeFormat: 'hh:mm tt'
            });
           <%-- $j('#<%=PatientIntime.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });
            $j('#<%=PatientOuttime.ClientID%>').datepicker({
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    $(this).focus();
                }
            });--%>
            $('#<%=txt_DOV.ClientID%>').mask("99/99/9999");
            $('#<%=txt_DOB.ClientID%>').mask("99/99/9999");
            $('#<%=txt_DOA.ClientID%>').mask("99/99/9999");

            $('#<%=txt_fname.ClientID%>').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
                , source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Page1.aspx/getFirstName") %>',
                        data: "{ 'prefix': '" + request + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            items = [];
                            map = {};
                            $.each(data.d, function (i, item) {
                                var id = item.split('-')[1];
                                var name = item.split('-')[0];
                                map[name] = { id: id, name: name };
                                items.push(name);
                            });
                            response(items);
                            $(".dropdown-menu").css("height", "auto");
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                updater: function (item) {
                    // $('[id*=hfpatientId]').val(map[item].id);
                    return item;
                }
            });

            $('[id*=txt_lname]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
                , source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Page1.aspx/getLastName") %>',
                        data: "{ 'prefix': '" + request + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            items = [];
                            map = {};
                            $.each(data.d, function (i, item) {
                                var id = item.split('-')[1];
                                var name = item.split('-')[0];
                                // name = name.split('~')[0];
                                map[name] = { id: id, name: name };
                                items.push(name);
                            });
                            response(items);
                            $(".dropdown-menu").css("height", "auto");
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                updater: function (item) {
                    return item;
                }
            });

            $('[id*=txt_ins_co]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
                , source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Page1.aspx/getInsComp") %>',
                        data: "{ 'prefix': '" + request + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            items = [];
                            map = {};
                            $.each(data.d, function (i, item) {
                                var id = item.split('-')[1];
                                var name = item.split('-')[0];
                                map[name] = { id: id, name: name };
                                items.push(name);
                            });
                            response(items);
                            $(".dropdown-menu").css("height", "auto");
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                updater: function (item) {
                    $('[id*=hfinccmp]').val(map[item].id);
                    return item;
                }
            });

            $('[id*=txt_pharmacy_name]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
                , source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Page1.aspx/getPharmacy") %>',
                        data: "{ 'prefix': '" + request + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            items = [];
                            map = {};
                            $.each(data.d, function (i, item) {
                                var id = item.split('-')[1];
                                var name = item.split('-')[0];
                                map[name] = { id: id, name: name };
                                items.push(name);
                            });
                            response(items);
                            $(".dropdown-menu").css("height", "auto");
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                updater: function (item) {
                    $('[id*=hpharmacy]').val(map[item].id);
                    return item;
                }
            });


            $('[id*=txt_attorney]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
                , source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("~/Page1.aspx/getAttorney") %>',
                        data: "{ 'prefix': '" + request + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            items = [];
                            map = {};
                            $.each(data.d, function (i, item) {
                                var id = item.split('-')[1];
                                var name = item.split('-')[0];
                                map[name] = { id: id, name: name };
                                items.push(name);
                            });
                            response(items);
                            $(".dropdown-menu").css("height", "auto");
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                updater: function (item) {
                    $('[id*=hattorney]').val(map[item].id);
                    $.ajax({
                        url: '<%=ResolveUrl("~/Page1.aspx/txt_attorney_TextChanged") %>',
                        data: "{ 'prefix': '" + map[item].id + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            $('#<%=txt_attorney_ph.ClientID%>').val('');
                            $('#<%=txt_attorney_ph.ClientID%>').text('');
                            $('#<%=txt_attorney_ph.ClientID%>').text(data.d);
                            $('#<%=txt_attorney_ph.ClientID%>').val(data.d);
                        }
                    });
                    $('#<%=txt_attorney_ph.ClientID%>').focus();
                    return item;
                }
            });

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {

                $('#<%=txt_mobile.ClientID%>').mask("999-999-9999");
                $('#<%=txt_home_ph.ClientID%>').mask("999-999-9999");
                $('#<%=txt_work_ph.ClientID%>').mask("999-999-9999");
                $('#<%=txt_attorney_ph.ClientID%>').mask("999-999-9999");
                $('#<%=txt_SSN.ClientID%>').mask("999-99-9999");
                $('#<%=txt_adjuster_no.ClientID%>').mask("999-999-9999");

                //$('[id*=txt_DOB]').mask("99/99/9999")

                //$('[id*=txt_DOV]').datepicker();
                //$('[id*=txt_DOB]').datepicker();
                $('#<%=txt_DOA.ClientID%>').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    onSelect: function (dateText, inst) {
                        $(this).focus();
                    }
                });
                $('#<%=txt_DOV.ClientID%>').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    onSelect: function (dateText, inst) {
                        $(this).focus();
                    }
                });
                $('#<%=txt_DOB.ClientID%>').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    yearRange: "-100:+0",
                    onSelect: function (dateText, inst) {
                        $(this).focus();
                    }
                });
                $j('#<%=PatientIntime.ClientID%>').datetimepicker({
                    controlType: 'select',
                    oneLine: true,
                    timeFormat: 'hh:mm tt'
                });
                $j('#<%=PatientOuttime.ClientID%>').datetimepicker({
                    controlType: 'select',
                    oneLine: true,
                    timeFormat: 'hh:mm tt'
                });
                <%--$j('#<%=PatientIntime.ClientID%>').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    onSelect: function (dateText, inst) {
                        $(this).focus();
                    }
                });
                $j('#<%=PatientOuttime.ClientID%>').datepicker({
                    changeMonth: true,
                    changeYear: true,
                    onSelect: function (dateText, inst) {
                        $(this).focus();
                    }
                });--%>
                $('#<%=txt_DOV.ClientID%>').mask("99/99/9999");
                $('#<%=txt_DOB.ClientID%>').mask("99/99/9999");
                $('#<%=txt_DOA.ClientID%>').mask("99/99/9999");


                $('#<%=txt_fname.ClientID%>').typeahead({
                    hint: true,
                    highlight: true,
                    minLength: 1
                    , source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/Page1.aspx/getFirstName") %>',
                            data: "{ 'prefix': '" + request + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                items = [];
                                map = {};
                                $.each(data.d, function (i, item) {
                                    var id = item.split('-')[1];
                                    var name = item.split('-')[0];
                                    map[name] = { id: id, name: name };
                                    items.push(name);
                                });
                                response(items);
                                $(".dropdown-menu").css("height", "auto");
                            },
                            error: function (response) {
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        });
                    },
                    updater: function (item) {
                        // $('#<%=hfpatientId.ClientID%>').val(map[item].id);
                        return item;
                    }
                });

                $('#<%=txt_lname.ClientID%>').typeahead({
                    hint: true,
                    highlight: true,
                    minLength: 1
                    , source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/Page1.aspx/getLastName") %>',
                            data: "{ 'prefix': '" + request + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                items = [];
                                map = {};
                                $.each(data.d, function (i, item) {
                                    var id = item.split('-')[1];
                                    var name = item.split('-')[0];
                                    map[name] = { id: id, name: name };
                                    items.push(name);
                                });
                                response(items);
                                $(".dropdown-menu").css("height", "auto");
                            },
                            error: function (response) {
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        });
                    },
                    updater: function (item) {
                        //alert(item);
                        //var parts = item.split('~');
                        //$('[id*=hfpatientId]').val(map[item].id);
                        return item;
                    }
                });
                $(document).on('mousedown', 'ul.typeahead', function (e) {
                    e.preventDefault();
                });

                $('#<%=txt_ins_co.ClientID%>').typeahead({
                    hint: true,
                    highlight: true,
                    minLength: 1
                    , source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/Page1.aspx/getInsComp") %>',
                            data: "{ 'prefix': '" + request + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                items = [];
                                map = {};
                                $.each(data.d, function (i, item) {
                                    var id = item.split('-')[1];
                                    var name = item.split('-')[0];
                                    map[name] = { id: id, name: name };
                                    items.push(name);
                                });
                                response(items);
                                $(".dropdown-menu").css("height", "auto");
                            },
                            error: function (response) {
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        });
                    },
                    updater: function (item) {
                        $('#<%=hfinccmp.ClientID%>').val(map[item].id);
                        return item;
                    }
                });
                $('#<%=txt_pharmacy_name.ClientID%>').typeahead({
                    hint: true,
                    highlight: true,
                    minLength: 1
                    , source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/Page1.aspx/getPharmacy") %>',
                            data: "{ 'prefix': '" + request + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                items = [];
                                map = {};
                                $.each(data.d, function (i, item) {
                                    var id = item.split('-')[1];
                                    var name = item.split('-')[0];
                                    map[name] = { id: id, name: name };
                                    items.push(name);
                                });
                                response(items);
                                $(".dropdown-menu").css("height", "auto");
                            },
                            error: function (response) {
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        });
                    },
                    updater: function (item) {
                        $('#<%=hpharmacy.ClientID%>').val(map[item].id);
                            $('#<%=txt_pharmacy_address.ClientID%>').focus();
                        return item;
                    }
                });

                $('#<%=txt_attorney.ClientID%>').typeahead({
                    hint: true,
                    highlight: true,
                    minLength: 1
                    , source: function (request, response) {
                        $.ajax({
                            url: '<%=ResolveUrl("~/Page1.aspx/getAttorney") %>',
                            data: "{ 'prefix': '" + request + "'}",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                items = [];
                                map = {};
                                $.each(data.d, function (i, item) {
                                    var id = item.split('-')[1];
                                    var name = item.split('-')[0];
                                    map[name] = { id: id, name: name };
                                    items.push(name);
                                });
                                response(items);
                                $(".dropdown-menu").css("height", "auto");
                            },
                            error: function (response) {
                                alert(response.responseText);
                            },
                            failure: function (response) {
                                alert(response.responseText);
                            }
                        });
                    },
                    updater: function (item) {
                        $('[id*=hattorney]').val(map[item].id);
                        $.ajax({
                            url: '<%=ResolveUrl("~/Page1.aspx/txt_attorney_TextChanged") %>',
                                data: "{ 'prefix': '" + map[item].id + "'}",
                                dataType: "json",
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    $('#<%=txt_attorney_ph.ClientID%>').val('');
                        $('#<%=txt_attorney_ph.ClientID%>').text('');
                        $('#<%=txt_attorney_ph.ClientID%>').text(data.d);
                        $('#<%=txt_attorney_ph.ClientID%>').val(data.d);
                                }
                            });
                            $('#<%=txt_attorney_ph.ClientID%>').focus();
                        return item;
                    }
                });


            }
        });

        function Validation(e) {

            var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
            if (!(date_regex.test($('#<%=txt_DOA.ClientID%>').val()))) {
                // alert("Date of Appointment Should be MM/dd/yyyy format");
                return false;
            }
            if (!(date_regex.test($('#<%=txt_DOV.ClientID%>').val()))) {
                // alert("Date of Visit Should be MM/dd/yyyy format");
                return false;
            }
            if (!(date_regex.test($('#<%=txt_DOB.ClientID%>').val()))) {
                //alert("Date of Birth Should be MM/dd/yyyy format");
                return false;
            }
        }

    </script>

    <%--</asp:Content>
   
<asp:Content ID="Content2" ContentPlaceHolderID="cpTitle" runat="Server">--%>
    <%--<div>
        <ul class="breadcrumb">
            <li>
                <i class="icon-home"></i>
                <a href="Page1.aspx"><span class="label label-success">Page1</span></a>
            </li>
            <li id="lipage2">
                <i class="icon-edit"></i>
                <a href="Page2.aspx"><span class="label">Page2</span></a>
            </li>
            <li id="li1" runat="server" enable="false">
                <i class="icon-edit"></i>
                <a href="Page3.aspx"><span class="label">Page3</span></a>
            </li>
            <li id="li2" runat="server" enable="false">
                <i class="icon-edit"></i>
                <a href="Page4.aspx"><span class="label">Page4</span></a>
            </li>
        </ul>
        <asp:LinkButton ID="lbtnProcedureDetails" CssClass="procDetail" runat="server" OnClick="lbtnProcedureDetails_Click">Procedure Details</asp:LinkButton>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpMain" runat="Server">--%>

    <div id="mymodelmessage" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Message</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="upMessage" UpdateMode="Conditional">
                        <ContentTemplate>
                            <label runat="server" id="lblMessage"></label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="pageHDN" runat="server" />
    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField ID="hfpatientId" runat="server" />
            <asp:HiddenField ID="hfinccmp" runat="server" />
            <asp:HiddenField ID="hpharmacy" runat="server" />
            <asp:HiddenField ID="hattorney" runat="server" />
            <div class="form-horizontal" style="display: none">
                <div class="control-group span6">
                    <label class="control-label">Patient In Time: </label>
                    <div class="controls">
                        <asp:TextBox ID="PatientIntime" TabIndex="28" runat="server"></asp:TextBox>
                        <button type="button" class="btn btn-primary btn-small" onclick="getInTime()">Time In</button>
                    </div>
                </div>
                <div class="control-group span6">
                    <label class="control-label">Patient Out Time: </label>
                    <div class="controls">
                        <asp:TextBox ID="PatientOuttime" TabIndex="29" runat="server"></asp:TextBox>
                        <button type="button" class="btn btn-primary btn-small" onclick="getOutTime()">Time Out</button>
                    </div>
                </div>
            </div>
            <div class="form-horizontal">

                <div class="control-group span3">
                    <label class="control-label" style="font-weight: bold; color: red">Location: </label>
                    <div class="controls">
                        <asp:DropDownList runat="server" ID="ddl_location" Width="190px" TabIndex="1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" InitialValue="0" runat="server" ControlToValidate="ddl_location" Display="Dynamic" ErrorMessage="Please select Location" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="control-group span3">
                    <label class="control-label" style="font-weight: bold; color: red">DOS: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_DOV" OnTextChanged="txt_DOV_TextChanged" AutoPostBack="true" placeholder="MM/dd/yyyy" TabIndex="2"></asp:TextBox>
                        <asp:CustomValidator runat="server" ControlToValidate="txt_DOV" Display="Dynamic"
                            SetFocusOnError="True" ValidationGroup="save" ErrorMessage="DOV should be MM/dd/yyyy format" OnServerValidate="CustomValidator3_ServerValidate" />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txt_DOV"
                            ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d"
                            ErrorMessage="Invalid date format." ValidationGroup="save" />
                        <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"     
                                ErrorMessage="Please enter valid date" 
                                ControlToValidate="txt_DOV"     
                                ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" />--%>
                    </div>
                </div>

                <div class="control-group span3">
                    <label class="control-label" style="font-weight: bold; color: red">DOA: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_DOA" AutoPostBack="true" placeholder="MM/dd/yyyy" OnTextChanged="txt_DOA_TextChanged" TabIndex="3"></asp:TextBox>
                        <asp:CustomValidator runat="server" ControlToValidate="txt_DOA" Display="Dynamic" SetFocusOnError="True"
                            ValidationGroup="save" ErrorMessage="DOA should be MM/dd/yyyy format" OnServerValidate="CustomValidator2_ServerValidate" />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txt_DOA"
                            ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d"
                            ErrorMessage="Invalid date format." ValidationGroup="save" />
                        <%--<asp:RegularExpressionValidator ID="regexpName" runat="server"     
                                ErrorMessage="Please enter valid date" 
                                ControlToValidate="txt_DOA"     
                                ValidationExpression="^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$" />--%>
                    </div>
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="form-horizontal">
                <div class="control-group span3">
                    <label class="control-label" style="font-weight: bold; color: red">Last Name: </label>
                    <div class="controls">

                        <asp:TextBox runat="server" ID="txt_lname" CssClass="capitalize" OnTextChanged="txt_lname_TextChanged" TabIndex="4" AutoCompleteType="None"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_lname" Display="Dynamic" ErrorMessage="Please enter patient details" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="control-group span3">
                    <label class="control-label" style="font-weight: bold; color: red">First Name: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_fname" CssClass="capitalize" OnTextChanged="txt_fname_TextChanged" TabIndex="5" AutoCompleteType="None"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_fname" Display="Dynamic" ErrorMessage="Please enter patient details" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="control-group span3">
                    <label class="control-label">MI: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_mname" CssClass="capitalize" TabIndex="6"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group span3">
                    <label class="control-label" style="font-weight: bold; color: red">Sex: </label>
                    <div class="controls">
                        <asp:DropDownList runat="server" ID="ddl_gender" Width="90px" TabIndex="7">
                            <asp:ListItem Value="0">-- Sex --</asp:ListItem>
                            <asp:ListItem Value="Mr." Text="M"></asp:ListItem>
                            <asp:ListItem Value="Ms." Text="F"></asp:ListItem>

                        </asp:DropDownList><br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddl_gender" Display="Dynamic" ErrorMessage="Please select sex" SetFocusOnError="True" ValidationGroup="save" InitialValue="0"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>

            <div style="clear: both"></div>
            <div class="form-horizontal">
                <div class="control-group span3">
                    <label class="control-label" style="font-weight: bold; color: red">DOB: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" AutoPostBack="true" OnTextChanged="txt_DOB_TextChanged" ID="txt_DOB" placeholder="MM/dd/yyyy" TabIndex="8"></asp:TextBox>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_DOB" Display="Dynamic" ErrorMessage="Please enter DOB" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                    </div>
                    <asp:RegularExpressionValidator runat="server" ControlToValidate="txt_DOB" ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d"
                        ErrorMessage="Invalid date format." ValidationGroup="save" />
                    <asp:CustomValidator runat="server" ControlToValidate="txt_DOB" ErrorMessage="DOB should be MM/dd/yyyy format" Display="Dynamic" SetFocusOnError="True" ValidationGroup="save" OnServerValidate="CustomValidator1_ServerValidate" />
                    <%--<asp:RegularExpressionValidator runat="server" ID="reg11" ValidationExpression="/^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d+$/" ErrorMessage="Please enter valid date" Display="Dynamic" ForeColor="Red" ControlToValidate="txt_DOB"></asp:RegularExpressionValidator>--%>
                </div>
                <div class="control-group span3">
                    <label class="control-label" style="font-weight: bold; color: red">Handedness: </label>
                    <div class="controls">
                        <asp:DropDownList runat="server" ID="ddlHandedness" Width="120px" TabIndex="9">
                            <asp:ListItem Text="right-handed"></asp:ListItem>
                            <asp:ListItem Text="left-handed"></asp:ListItem>
                            <asp:ListItem Text="ambidextrous"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="control-group span3">
                    <label class="control-label">SSN: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_SSN" placeholder="xxx-xx-xxxx" TabIndex="10" MaxLength="9"></asp:TextBox>
                        <%--placeholder="xxx-xx-xxxx"  ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{2}[\s.-]\d{4}$"--%>
                        <asp:RegularExpressionValidator runat="server" ID="reg1" ForeColor="Red" ControlToValidate="txt_SSN" Display="Dynamic" ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{2}[\s.-]\d{4}$" ErrorMessage="SSN should be of 9 digit" ValidationGroup="save"></asp:RegularExpressionValidator>
                    </div>
                </div>

            </div>
            <div style="clear: both"></div>
            <div class="form-horizontal">

                <div class="control-group span3">
                    <label class="control-label">Home Ph: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_home_ph" placeholder="xxx-xxx-xxxx" TabIndex="11"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ForeColor="Red" ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$" ControlToValidate="txt_home_ph" Display="Dynamic" ErrorMessage="invalid format for Home Ph." ValidationGroup="save"></asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="control-group span3">
                    <label class="control-label">Work Ph: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_work_ph" placeholder="xxx-xxx-xxxx" TabIndex="12"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2" ForeColor="Red" ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$" ControlToValidate="txt_work_ph" Display="Dynamic" ErrorMessage="invalid format for Work Ph." ValidationGroup="save"></asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="control-group span3">
                    <label class="control-label">Mobile: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_mobile" placeholder="xxx-xxx-xxxx" TabIndex="13"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator3" ForeColor="Red" ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$" ControlToValidate="txt_mobile" Display="Dynamic" ErrorMessage="invalid format for mobile." ValidationGroup="save"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
            <div style="clear: both"></div>
            <div class="form-horizontal">
                <div class="control-group span3">
                    <label class="control-label">Address: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_add" TabIndex="14"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group span3">
                    <label class="control-label">City: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_city" TabIndex="15"></asp:TextBox>
                    </div>
                </div>
                <div class="control-group span3">
                    <label class="control-label">State: </label>
                    <div class="controls">
                        <asp:DropDownList ID="ddState" TabIndex="16" runat="server"></asp:DropDownList>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddState" Display="Dynamic" ErrorMessage="Please Select State" SetFocusOnError="True" ValidationGroup="save" InitialValue='0'></asp:RequiredFieldValidator>--%>
                        <asp:Button ID="def_State" runat="server" CssClass="btn btn-primary btn-sm" OnClick="def_State_Click" Text="Default" />
                    </div>
                </div>
                <div class="control-group span3">
                    <label class="control-label">Zip: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_zip" Width="100px" TabIndex="17"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div style="clear: both"></div>
            <div class="form-horizontal">

                <div class="control-group span3">
                    <label class="control-label">Insurance Co.: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_ins_co" CssClass="capitalize" TabIndex="18"></asp:TextBox>
                    </div>
                </div>

                <div class="control-group span3">
                    <label class="control-label">Claim #: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_claim" TabIndex="19"></asp:TextBox>
                    </div>
                </div>

                <div class="control-group span3">
                    <label class="control-label">Policy #: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" ID="txt_policy" TabIndex="20"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div style="clear: both"></div>
            <asp:UpdatePanel runat="server" ID="upattorny">
                <ContentTemplate>
                    <div class="form-horizontal">

                        <div class="control-group span3">
                            <label class="control-label">Attorney Name: </label>
                            <div class="controls">
                                <asp:TextBox runat="server" Width="200px" ID="txt_attorney" CssClass="capitalize" AutoCompleteType="None" onblur="setFocus('txt_attorney_ph');" TabIndex="21"></asp:TextBox>
                            </div>
                        </div>
                        <%--OnTextChanged="txt_attorney_TextChanged"--%>
                        <div class="control-group span3">
                            <label class="control-label">Attorney Ph: </label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="txt_attorney_ph" placeholder="xxx-xxx-xxxxx" TabIndex="22"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator4" ForeColor="Red" ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$" ControlToValidate="txt_attorney_ph" Display="Dynamic" ErrorMessage="invalid format for Attorney Ph." ValidationGroup="save"></asp:RegularExpressionValidator>
                            </div>
                        </div>

                        <div class="control-group span3">
                            <label class="control-label" style="font-weight: bold; color: red">Case Type: </label>
                            <div class="controls">

                                <asp:DropDownList runat="server" ID="ddl_casetype" Width="150px" TabIndex="23">
                                </asp:DropDownList>
                                <br />
                                <asp:RequiredFieldValidator ID="rfvCaseType" runat="server" ControlToValidate="ddl_casetype" Display="Dynamic" ErrorMessage="Please Select Case Type" SetFocusOnError="True" ValidationGroup="save" InitialValue=''></asp:RequiredFieldValidator>
                            </div>
                        </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="clear: both"></div>

            <asp:UpdatePanel runat="server" ID="uppharmacy">
                <ContentTemplate>
                    <div class="form-horizontal">

                        <div class="control-group span3">
                            <label class="control-label">Pharmacy Name: </label>
                            <div class="controls">
                                <asp:TextBox runat="server" Width="200px" ID="txt_pharmacy_name" CssClass="capitalize" TabIndex="24"></asp:TextBox>
                                <%--OnTextChanged="txt_pharmacy_name_TextChanged"  AutoPostBack="true" --%>
                            </div>
                        </div>

                        <div class="control-group span3">
                            <label class="control-label">Address: </label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="txt_pharmacy_address" TabIndex="25"></asp:TextBox>
                            </div>
                        </div>
                        <div class="control-group span3">
                            <label class="control-label">WCBGroup: </label>
                            <div class="controls">
                                <asp:TextBox runat="server" ID="txt_WCBGroup" TabIndex="26"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="clear: both"></div>
            <div class="form-horizontal">
                <div class="control-group span3">
                    <label class="control-label">MA & Providers: </label>
                    <div class="controls">
                        <%-- <asp:TextBox ID="txtMAProviders" runat="server" Enabled="true"></asp:TextBox>--%>
                       <%-- <asp:DropDownList ID="txtMAProviders" runat="server" ></asp:DropDownList>--%>
                        <asp:DropDownList runat="server" ID="txtMAProviders" Width="190px">
                        </asp:DropDownList>
                        <%--txtMAProviders--%>
                    </div>
                </div>
            </div>
            <div class="form-horizontal">
                <div class="control-group span4">
                    <label class="control-label">Adjuster:</label>
                    <div class="controls">
                        <asp:TextBox runat="server" Width="100px" ID="txt_adjuster" TabIndex="30"></asp:TextBox>
                    </div>
                </div>
                <div style="margin-left: -250px;" class="control-group span3">
                    <label class="control-label">Adjuster Ph: </label>
                    <div class="controls">
                        <asp:TextBox runat="server" Width="100px" ID="txt_adjuster_no" TabIndex="31"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator6" ForeColor="Red" ValidationExpression="^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$" ControlToValidate="txt_adjuster_no" Display="Dynamic" ErrorMessage="invalid format for Adjusterno." ValidationGroup="save"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div style="margin-left: -160px;" class="control-group span3">
                    <label class="control-label">Ext.: </label>
                    <div class="controls">
                        <asp:TextBox Width="100px" runat="server" ID="txtAdjusterExtension" TabIndex="32"></asp:TextBox>
                    </div>
                </div>
            </div>

            </div>
           
            <br />
            <div style="clear: both"></div>
            <div class="form-horizontal" style="height: 250px;">
                <div class="controls">
                    <div style="display: none">
                        <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" TabIndex="33" ValidationGroup="save" />
                    </div>
                    <asp:Button runat="server" ID="Button1" PostBackUrl="~/PatientIntakeList.aspx" Text="Back to List" TabIndex="34" CssClass="btn btn-default" UseSubmitBehavior="False" />
                </div>
                <div class="controls">
                    <asp:Label runat="server" ID="lblmess"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>

