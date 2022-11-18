<%@ Page Title="" Language="C#" MasterPageFile="~/AddFollowUpMaster.master" AutoEventWireup="true" CodeFile="FuKnee.aspx.cs" Inherits="FuKnee" %>

<%@ Register Assembly="EditableDropDownList" Namespace="EditableControls" TagPrefix="editable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script src="Scripts/jquery-1.8.2.min.js"></script>
    <script src="js/images/bootstrap.min.js"></script>

    <style>
        /*.table{
    display:table;
    width:100%;
    table-layout:fixed;
}*/
        .table_cell {
            /*display:table-cell;*/
            width: 100px;
            /*border:solid black 1px;*/
        }
    </style>
    <script type="text/javascript">
        function Confirmbox(e, page) {
            e.preventDefault();
            var answer = confirm('Do you want to save the data?');
            if (answer) {
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
    </script>
    <asp:HiddenField ID="pageHDN" runat="server" />
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
    <!-- start: Header -->
    <%--    <asp:UpdatePanel runat="server" ID="upMain">
            <ContentTemplate>--%>
    <div class="container">
        <div class="row">
            <div class="col-lg-10" id="content">
                <%--    <ul class="breadcrumb">
                                <li>
                                    <i class="icon-home"></i>
                                    <a href="Page1.aspx"><span class="label">Page1</span></a>
                                </li>
                                <li id="lipage2">
                                    <i class="icon-edit"></i>
                                    <a href="Page2.aspx"><span class="label label-success">Page2</span></a>
                                </li>
                                <li id="li1" runat="server" enable="false">
                                    <i class="icon-edit"></i>
                                    <a href="Page3.aspx"><span class="label">Page3</span></a>
                                </li>
                                <li id="li2" runat="server" enable="false">
                                    <i class="icon-edit"></i>
                                    <a href="Page4.aspx"><span class="label">Page4</span></a>
                                </li>
                            </ul>--%>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"><b><u>CHIEF COMPLAINT</u></b></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <div id="WrapLeft" runat="server">
                            <label class="control-label">The patient complains of </label>
                            <label class="control-label">&nbsp; Left knee pain that is &nbsp;</label>
                            <asp:TextBox runat="server" ID="txtPainScaleLeft" Style="margin-left: 0px;" Width="40px"></asp:TextBox>
                            <label class="control-label">&nbsp; /10, with 10 being the worst, which is ,&nbsp;</label>
                            <asp:CheckBox ID="chkContentLeft" Style="" runat="server" Text="constant" />
                            <asp:CheckBox ID="chkIntermittentLeft" Style="" runat="server" Text="intermittent." />
                            <asp:CheckBox ID="chkSharpLeft" Style="" runat="server" Text="sharp  " Checked="true" />
                            <asp:CheckBox ID="chkElectricLeft" Style="" runat="server" Text="electric,  " />
                            <asp:CheckBox ID="chkShootingLeft" Style="" runat="server" Text="shooting,  " Checked="true" />
                            <asp:CheckBox ID="chkThrobblingLeft" Style="" runat="server" Text="throbbing,  " />
                            <asp:CheckBox ID="chkPulsatingLeft" Style="" runat="server" Text="pulsating,  " />
                            <asp:CheckBox ID="chkDullLeft" Style="" runat="server" Text="&nbsp;dull,&nbsp;" />
                            <asp:CheckBox ID="chkAchyLeft" Style="" runat="server" Text="&nbsp;achy in nature.&nbsp;" />
                            <label class="control-label">The knee pain worsens with </label>
                            <asp:CheckBox ID="chkWorseMovementLeft" Style="" runat="server" Text="movement,  " />
                            <asp:CheckBox ID="chkWorseWalkingLeft" Style="" runat="server" Text="walking,  " Checked="true" />
                            <asp:CheckBox ID="chkWorseStairsLeft" Style="" runat="server" Text="climbing stairs,  " Checked="true" />
                            <asp:CheckBox ID="chkWorseSquattingLeft" Style="" runat="server" Text="squatting,  " Checked="true" />
                            <asp:CheckBox ID="chkWorseActivitiesLeft" Style="" runat="server" Text="activities  " />
                            <asp:CheckBox ID="chkWorseOtherLeft" Style="" runat="server" Text="and&nbsp; " />
                            <asp:TextBox ID="txtWorseOtherTextLeft" runat="server" Width="200"> </asp:TextBox>
                            <label class="control-label">The knee pain is improved with&nbsp; </label>
                            <asp:CheckBox ID="chkImprovedRestingLeft" Style="" runat="server" Text="resting,  " />
                            <asp:CheckBox ID="chkImprovedMedicationLeft" Style="" runat="server" Text="medication,  " />
                            <asp:CheckBox ID="chkImprovedTherapyLeft" Style="" runat="server" Text="therapy,  " />
                            <asp:CheckBox ID="chkImprovedSleepingLeft" Style="" runat="server" Text="sleeping." />
                        </div>
                        <br />
                        <div id="wrpRight" runat="server">
                            <label class="control-label">The patient complains of</label>
                            <label class="control-label">&nbsp; right knee pain that is &nbsp;</label>
                            <asp:TextBox runat="server" ID="txtPainScaleRight" Style="margin-left: 0px;" Width="40px"> </asp:TextBox>
                            <label class="control-label">&nbsp; /10, with 10 being the worst, which is ,&nbsp;</label>
                            <asp:CheckBox ID="chkContentRight" Style="" runat="server" Text="constant" />
                            <asp:CheckBox ID="chkIntermittentRight" Style="" runat="server" Text="intermittent." />
                            <asp:CheckBox ID="chkSharpRight" Style="" runat="server" Text="sharp  " Checked="true" />
                            <asp:CheckBox ID="chkElectricRight" Style="" runat="server" Text="electric,  " />
                            <asp:CheckBox ID="chkShootingRight" Style="" runat="server" Text="shooting,  " Checked="true" />
                            <asp:CheckBox ID="chkThrobblingRight" Style="" runat="server" Text="throbbing,  " />
                            <asp:CheckBox ID="chkPulsatingRight" Style="" runat="server" Text="pulsating,  " />
                            <asp:CheckBox ID="chkDullRight" Style="" runat="server" Text="dull,  " />
                            <asp:CheckBox ID="chkAchyRight" Style="" runat="server" Text="achy in nature.  " />
                            <label class="control-label">&nbsp;The knee pain worsens with&nbsp; </label>
                            <asp:CheckBox ID="chkWorseMovementRight" Style="" runat="server" Text="movement,  " />
                            <asp:CheckBox ID="chkWorseWalkingRight" Style="" runat="server" Text="walking,  " Checked="true" />
                            <asp:CheckBox ID="chkWorseStairsRight" Style="" runat="server" Text="climbing stairs,  " />
                            <asp:CheckBox ID="chkWorseSquattingRight" Style="" runat="server" Text="squatting,  " Checked="true" />
                            <asp:CheckBox ID="chkWorseActivitiesRight" Style="" runat="server" Text="activities  " />
                            <asp:CheckBox ID="chkWorseOtherRight" Style="" runat="server" Text="and&nbsp;&nbsp; " />
                            <asp:TextBox ID="txtWorseOtherTextRight" Style="" runat="server" Width="200"> </asp:TextBox>
                            <label class="control-label">The knee pain is improved with &nbsp;</label>
                            <asp:CheckBox ID="chkImprovedRestingRight" Style="" runat="server" Text="resting,  " />
                            <asp:CheckBox ID="chkImprovedMedicationRight" Style="" runat="server" Text="medication,  " />
                            <asp:CheckBox ID="chkImprovedTherapyRight" Style="" runat="server" Text="therapy,  " />
                            <asp:CheckBox ID="chkImprovedSleepingRight" Style="" runat="server" Text="sleeping." />
                        </div>
                        <br />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox ID="txtFreeFormCC" runat="server" Style="" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <button type="button" id="start_button1" onclick="startButton1(event)">
                            <img src="images/mic.gif" alt="start" /></button>
                        <div style="display: none"><span class="final" id="final_span1"></span><span class="interim" id="interim_span1"></span></div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px;">
                        <table style="width: 40%;">
                            <thead>
                                <tr>
                                    <td style="text-align: left;">Lower Extremities
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td style="">ROM
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td style="">Left
                                    </td>
                                    <%--<td style="">IS
                                            </td>--%>
                                    <td style="">Right
                                    </td>
                                    <%--<td style="">IS
                                            </td>--%>
                                    <td style="">Normal
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="text-align: left;">Flexion</td>
                                    <%--<td>
                                                <asp:TextBox ID="txtLEFlexionLeftWas" Text="30" Width="50px" runat="server"></asp:TextBox></td>--%>
                                    <td>
                                        <asp:TextBox ID="txtLEFlexionLeft" runat="server" Width="50px" Text="30"></asp:TextBox></td>
                                    <%-- <td>
                                                <asp:TextBox ID="txtLEFlexionRightWas" Width="50px" runat="server"></asp:TextBox></td>--%>
                                    <td>
                                        <asp:TextBox ID="txtLEFlexionRight" Width="50px" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtFwdFlexNormal" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                </tr>

                                <tr>
                                    <td style="text-align: left;">Extension</td>
                                    <%--<td>
                                                <asp:TextBox ID="txtLEExtensionLeftWas" Text="30" Width="50px" runat="server"></asp:TextBox></td>--%>
                                    <td>
                                        <asp:TextBox ID="txtLEExtensionLeft" runat="server" Width="50px" Text="30"></asp:TextBox></td>
                                    <%--<td>
                                                <asp:TextBox ID="txtLEExtensionRightWas" Width="50px" runat="server"></asp:TextBox></td>--%>
                                    <td>
                                        <asp:TextBox ID="txtLEExtensionRight" Width="50px" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtExtensionNormal" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                </tr>

                            </tbody>
                        </table>
                        <br />
                    </div>
                </div>
                <%--<tr>
                            <th style="width: 10%;"><label class="control-label">PHYSICAL EXAM:</label></th>
                            <th style="width: 90%;">--%>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"><b><u>PHYSICAL EXAM:</u></b></label>
                    </div>

                    <%--     <div class="col-md-9" style="margin-top: 5px">

                        <asp:Repeater runat="server" ID="repROM" OnItemDataBound="repROM_ItemDataBound">
                            <HeaderTemplate>
                                <table style="width: 40%;">

                                    <thead>
                                        <tr>
                                            <td style="text-align: left;">ROM
                                            </td>
                                            <td></td>
                                            <td></td>
                                            <td style=""></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </thead>
                                    <tr>
                                        <td></td>
                                        <td style="">Left
                                        </td>
                                       
                                        <td style="">Right
                                        </td>
                                      
                                        <td style="">Normal
                                        </td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: left;">
                                        <asp:Label runat="server" ID="lblname" Text='<%# Eval("name") %>'></asp:Label></td>
                                   
                                    <td>
                                        <asp:TextBox ID="txtleft" runat="server" Width="50px" onkeypress="return onlyNumbers(event);" Text='<%# Eval("left") %>'></asp:TextBox></td>
                                   
                                    <td>
                                        <asp:TextBox ID="txtright" Width="50px" Text='<%# Eval("right") %>' onkeypress="return onlyNumbers(event);" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtnormal" ReadOnly="true" Text='<%# Eval("normal") %>' Width="50px" runat="server"></asp:TextBox></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>


                    </div>--%>
                    <div class="col-md-9" style="margin-top: 5px">
                        <div id="wrpPELeft" runat="server">
                            <label class="control-label">The knee exam reveals tenderness upon palpation of the Left&nbsp;</label>
                            <asp:TextBox runat="server" ID="txtPalpationText1Left" Style="margin-right: 5px" Width="100px"></asp:TextBox>&nbsp;&nbsp;
                                    <asp:TextBox runat="server" ID="txtPalpationText2Left" Style="" Width="100px"> </asp:TextBox>
                            <asp:CheckBox ID="chkMedialLeft" Style="" runat="server" Text="medial  " Checked="true" />
                            <asp:CheckBox ID="chkLateralLeft" Style="" runat="server" Text="lateral " />
                            <asp:CheckBox ID="chkSuperiorLeft" runat="server" Style="" Text="superior  " Checked="true" />
                            <asp:CheckBox ID="chkInferiorLeft" runat="server" Style="" Text="inferior  " />
                            <asp:CheckBox ID="chkSupermedialLeft" runat="server" Style="" Text="superomedial  " />
                            <asp:CheckBox ID="chkSuperoLateralLeft" runat="server" Style="" Text="superolateral " />
                            <asp:CheckBox ID="chkInferomedialLeft" runat="server" Style="" Text="inferomedial  " Checked="true" />
                            <asp:CheckBox ID="chkInferoLateralLeft" Style="" runat="server" Text="inferolateral  " />
                            <asp:CheckBox ID="chkPeripatellarLeft" Style="" runat="server" Text="peripatellar.  " Checked="true" />

                        </div>
                        <br />
                        <div id="wrpPERight" runat="server">
                            <label class="control-label">The knee exam reveals tenderness upon palpation of the Right&nbsp;</label>
                            <asp:TextBox runat="server" ID="txtPalpationText1Right" Style="margin-right: 5px" Width="100px"></asp:TextBox>&nbsp;&nbsp;
                                    <asp:TextBox runat="server" ID="txtPalpationText2Right" Style="" Width="100px"> </asp:TextBox>
                            <asp:CheckBox ID="chkMedialRight" Style="" runat="server" Text="medial  " Checked="true" />
                            <asp:CheckBox ID="chkLateralRight" Style="" runat="server" Text="lateral " />
                            <asp:CheckBox ID="chkSuperiorRight" runat="server" Style="" Text="superior  " Checked="true" />
                            <asp:CheckBox ID="chkInferiorRight" runat="server" Style="" Text="inferior  " />
                            <asp:CheckBox ID="chkSupermedialRight" runat="server" Style="" Text="superomedial  " />
                            <asp:CheckBox ID="chkSuperoLateralRight" runat="server" Style="" Text="superolateral " />
                            <asp:CheckBox ID="chkInferomedialRight" runat="server" Style="" Text="inferomedial  " Checked="true" />
                            <asp:CheckBox ID="chkInferoLateralRight" Style="" runat="server" Text="inferolateral  " />
                            <asp:CheckBox ID="chkPeripatellarRight" Style="" runat="server" Text="peripatellar.  " Checked="true" />
                            <br />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <br />
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <td style="text-align: left"></td>
                                    <td>McMurray's
                                    </td>
                                    <td>Lachman's
                                    </td>
                                    <td>Anterior drawer
                                    </td>
                                    <td>Posterior drawer
                                    </td>
                                    <td>Varus stress test
                                    </td>
                                    <td>Valgus stress test
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="text-align: left;">Left</td>
                                    <td>
                                        <asp:CheckBox ID="chkMcMurrayLeft" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkLachmanLeft" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkAnteriorLeft" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkPosteriorLeft" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkVarusLeft" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkValgusLeft" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">Right</td>
                                    <td>
                                        <asp:CheckBox ID="chkMcMurrayRight" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkLachmanRight" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkAnteriorRight" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkPosteriorRight" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkVarusRight" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkValgusRight" runat="server" /></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeForm" Style="" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <button type="button" id="start_button" onclick="startButton(event)">
                            <img src="images/mic.gif" alt="start" /></button>
                        <div style="display: none"><span class="final" id="final_span"></span><span class="interim" id="interim_span"></span></div>

                    </div>
                </div>
                <%--<tr>
                            <th style="width: 10%;"><label class="control-label">ASSESSMENT/DIAGNOSIS</label></th>
                            <th style="width: 90%; margin-left: 40px;">--%>
                <asp:UpdatePanel runat="server" ID="upMedicine">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-3">
                                <label class="control-label"><b><u>ASSESSMENT/DIAGNOSIS:</u></b></label>
                                <div class="col-md-9" style="margin-top: 5px">
                                    <%--<asp:CheckBox ID="chkSprainStrain" Style="float: left;" runat="server" Text="Cervical muscle sprain/strain." Checked="true" /><br />
                                <asp:CheckBox ID="chkHerniation" Style="float: left; margin-left: -18.5%" runat="server" Text="Possible cervical disc herniation." Checked="true" /><br />--%>
                                    <%-- <asp:CheckBox ID="chkSyndrome" runat="server"  Text="Possible cervical radiculopathy vs. plexopathy vs. entrapment syndrome." Checked="true" />
                                    --%>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-3">
                                    <label class="control-label">Notes:</label>
                                </div>
                                <div class="col-md-9" style="margin-top: 5px">
                                    <asp:TextBox runat="server" Style="float: left;" ID="txtFreeFormA" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                                    <%-- <asp:ImageButton ID="AddDiag" Style="float: left; text-align: left;" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="basicPopup();" OnClick="AddDiag_Click" />--%>
                                    <asp:ImageButton ID="AddDiag" Style="float: left; text-align: left;" ImageUrl="~/img/a1.png" Height="50px" Width="50px" runat="server" OnClientClick="openModelPopup();" OnClick="AddDiag_Click" />
                                    <asp:GridView ID="dgvDiagCodes" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="100">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcc" ReadOnly="true" runat="server" Width="100" Text='<%# Eval("DiagCode") %>'></asp:TextBox>
                                                    <asp:HiddenField runat="server" ID="hidDiagCodeID" Value='<%# Eval("Diag_Master_ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="700">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtpe" runat="server" Width="700" Text='<%# Eval("Description") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="50">
                                                <ItemTemplate>
                                                    <%--    <asp:HiddenField runat="server" ID="hidDiagCodeDetailID" Value='<%# Eval("DiagCodeDetail_ID") %>' />--%>
                                                    <asp:CheckBox runat="server" ID="chkRemove" Checked="true" />

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>


                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"><b><u>PLAN:</u></b></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <%--  <asp:CheckBox ID="chkCervicalSpine" Style=";" Text="MRI" runat="server" />
                                <asp:ListBox ID="cboScanType" Style="; height: 25px;" runat="server"></asp:ListBox>
                                <asp:Label ID="Label7" Style=";" Text=" of the cervical spine " runat="server"></asp:Label>
                                <asp:TextBox ID="txtToRuleOut" runat="server" Style="; " Text="to rule out herniated nucleus pulposus/soft tissue injury " Width="299px"></asp:TextBox>--%>
                        <%--OnClick="AddStd_Click"--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeFormP" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <asp:ImageButton ID="AddStd" Style="display: none" runat="server" Height="50px" Width="50px" ImageUrl="~/img/a1.png" PostBackUrl="~/AddStandards.aspx" OnClientClick="basicPopup();return false;" />
                    </div>
                </div>


                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:GridView ID="dgvStandards" runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfFname" runat="server" Value='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Heading" ItemStyle-Width="450">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtHeading" runat="server" CssClass="form-control" Width="400px" TextMode="MultiLine" Text='<%# Eval("Heading") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PDesc" ItemStyle-Width="600">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPDesc" runat="server" CssClass="form-control" Width="600px" TextMode="MultiLine" Text='<%# Eval("PDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="IsChkd">

                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox2" runat="server" value='<%# Convert.ToBoolean(Eval("IsChkd")) %>' AutoPostBack="true" />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                <%-- <asp:TemplateField HeaderText="MCODE" ItemStyle-Width="150">
                                            <ItemTemplate>
                                                <asp:Label ID="mcode" runat="server" Text='<%# Eval("MCODE") %>'></asp:Label>
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                <%--  <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HiddenField ID="hfFname" runat="server" Value='<%# Eval("ProcedureDetail_ID") %>' />
                    </ItemTemplate>
                                      </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="BodyPart" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("BodyPart") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%--   <asp:TemplateField HeaderText="Heading" ItemStyle-Width="450">
                                    <ItemTemplate>--%>
                                <%--<asp:Label ID="lblheading" runat="server" Text='<%# Eval("Heading") %>'></asp:Label>--%>
                                <%--    <asp:TextBox ID="txtHeading" runat="server" CssClass="form-control" Width="400px"  TextMode="MultiLine" Text='<%# Eval("Heading") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <%-- <asp:TemplateField HeaderText="CC" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtcc" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("CCDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PE" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpe" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("PEDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AD" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtadesc" Width="48" ReadOnly="true" runat="server" Text='<%# Eval("ADesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="PD" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtpdesc" Width="95" ReadOnly="true" runat="server" Text='<%# Eval("PDesc") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%-- <asp:TemplateField HeaderText="PN" ItemStyle-Width="20">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox3" Enabled="false" runat="server" value='<%# Eval("PN") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                <%--<asp:TemplateField HeaderText="IsChkd">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox4" Enabled="false" runat="server" value='<%# Eval("PN") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="row"></div>
                <div class="row" style="margin-top: 15px">
                    <div class="col-md-3"></div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <%--<asp:ImageButton ID="LoadDV" Style=";" runat="server" OnClick="LoadDV_Click" ImageUrl="~/img/edit.gif" />--%>
                        <div style="display: none">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn blue" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--   </ContentTemplate>
        </asp:UpdatePanel>--%>


    <div class="modal fade" id="MedicinePopup" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none; width: 950px; margin-right: 20%">
        <div class="modal-dialog" style="width: 950px;">
            <div class="modal-content">
                <div class="modal-header">
                    Select Diag 
                    <b id="CatHeading"></b>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>

                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="upMedice">
                        <ContentTemplate>

                            <div class="row" style="margin: 5px">
                                <div class="col-md-3">
                                    <asp:TextBox ID="txDesc" runat="server" Style="margin-bottom: 0px" />
                                    &nbsp;
                                    <asp:Button runat="server" ID="btnSearch" Text="Filter" CssClass="btn btn-info" />
                                    &nbsp;
                                    <asp:Button runat="server" ID="btnDaigSave" Text="Save & Close" CssClass="btn btn-primary" OnClick="btnDaigSave_Click" />
                                </div>
                                <br />

                                <div class="col-md-12">
                                    <asp:GridView ID="dgvDiagCodesPopup" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false" DataKeyNames="DiagCode_ID">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Select">

                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsChkd")) %>' value='<%# Eval("IsChkd") %>' AutoPostBack="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="BodyPart" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("BodyPart") %>'></asp:Label>
                                                    <%--<asp:TextBox ID="txtbodypart" runat="server" Text='<%# Eval("BodyPart") %>'></asp:TextBox>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="DiagCode" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("DiagCode") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="550">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtDescription" Width="550" runat="server" Text='<%# Eval("Description") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PN" ItemStyle-Width="150">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox3" Enabled="false" runat="server" value='<%# Eval("PreSelect") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <script type="text/javascript">

        $.noConflict();
        function openModelPopup() {
            jQuery.noConflict();
            (function ($) {

                $('#MedicinePopup').modal('show');

            })(jQuery);
        }

        function closeModelPopup() {
            jQuery.noConflict();
            (function ($) {

                $('#MedicinePopup').modal('hide');

            })(jQuery);
        }

        var $j = jQuery.noConflict();
        $j('#MedicinePopup').on('hidden.bs.modal', function (e) {
            $('#ctl00_lnkbtn_Keen').addClass('active');
        });

        function OnSuccess(response) {
            //debugger;
            popupWindow = window.open("AddStandards.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');
        }
        function OnSuccess_q(response) {
            popupWindow = window.open("AddDiagnosis.aspx", 'popUpWindow', 'height=500,width=1200,left=100,top=30,resizable=No,scrollbars=Yes,toolbar=no,menubar=no,location=no,directories=no, status=No');

        }
        function basicPopup() {
            document.forms[0].target = "_blank";
        };
    </script>
    <script>
        $(document).ready(function () {
            $('#rbl_in_past input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_injur_past_bp").prop('disabled', true);
                    $("#txt_injur_past_how").prop('disabled', true);
                }
                else {
                    $("#txt_injur_past_bp").prop('disabled', false);
                    $("#txt_injur_past_how").prop('disabled', false);
                }
            });
        });

        $(document).ready(function () {
            $('#rbl_seen_injury input').change(function () {
                if ($(this).val() == 'False') {
                    $("#txt_docname").prop('disabled', true);
                }
                else {
                    $("#txt_docname").prop('disabled', false);
                }
            });
        });

        $(document).ready(function () {
            $('#rep_wenttohospital input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_day").prop('disabled', true);
                    $("#txt_day").prop('value', "0");
                }
                else {
                    $("#txt_day").prop('disabled', false);
                    $("#txt_day").select();
                    $("#txt_day").focus();
                }
            });
        });

        $(document).ready(function () {
            $('#rep_hospitalized input').change(function () {
                if ($(this).val() == '0') {
                    $("#txt_hospital").prop('disabled', true);
                    $("#txt_day").prop('disabled', true);
                    $("#chk_mri").prop('disabled', true);
                    $("#txt_mri").prop('disabled', true);
                    $("#chk_CT").prop('disabled', true);
                    $("#txt_CT").prop('disabled', true);
                    $("#chk_xray").prop('disabled', true);
                    $("#txt_x_ray").prop('disabled', true);
                    $("#txt_prescription").prop('disabled', true);
                    $("#txt_which_what").prop('disabled', true);
                }
                else {
                    $("#txt_hospital").prop('disabled', false);
                    $("#ddl_via").prop('disabled', false);
                    $("#txt_day").prop('disabled', false);
                    $("#chk_mri").prop('disabled', false);
                    $("#txt_mri").prop('disabled', false);
                    $("#chk_CT").prop('disabled', false);
                    $("#txt_CT").prop('disabled', false);
                    $("#chk_xray").prop('disabled', false);
                    $("#txt_x_ray").prop('disabled', false);
                    $("#txt_prescription").prop('disabled', false);
                    $("#txt_which_what").prop('disabled', false);
                }
            });
        });
    </script>
    <script>
        var controlname = null;
        var final_transcript = '';
        var recognizing = false;
        var ignore_onend;
        var start_timestamp;

        if (!('webkitSpeechRecognition' in window)) {
            // upgrade();
        } else {
            start_button.style.display = 'inline-block';
            var recognition = new webkitSpeechRecognition();
            recognition.continuous = true;
            recognition.interimResults = true;

            recognition.onstart = function () {
                recognizing = true;
            };

            recognition.onerror = function (event) {
                if (event.error == 'no-speech') {
                    ignore_onend = true;
                }
                if (event.error == 'audio-capture') {
                    //showInfo('info_no_microphone');
                    ignore_onend = true;
                }
                if (event.error == 'not-allowed') {
                    if (event.timeStamp - start_timestamp < 100) {
                        //showInfo('info_blocked');
                    } else {
                        //showInfo('info_denied');
                    }
                    ignore_onend = true;
                }
            };

            recognition.onend = function () {
                recognizing = false;
                if (ignore_onend) {
                    return;
                }
                if (!final_transcript) {
                    //showInfo('info_start');
                    return;
                }
                if (!final_transcript1) {
                    //showInfo('info_start');
                    return;
                }

            };

            recognition.onresult = function (event) {
                var interim_transcript = '';
                if (typeof (event.results) == 'undefined') {
                    recognition.onend = null;
                    recognition.stop();
                    //upgrade();
                    return;
                }
                for (var i = event.resultIndex; i < event.results.length; ++i) {
                    if (event.results[i].isFinal) {
                        final_transcript += event.results[i][0].transcript;
                    } else {
                        interim_transcript += event.results[i][0].transcript;
                    }
                }
                final_transcript = capitalize(final_transcript);
                //finalrecord = linebreak(final_transcript);
                //$('#ctl00_ContentPlaceHolder1_txtFreeForm').text(linebreak(final_transcript));
                $(controlname).text(linebreak(final_transcript));
                interim_span.innerHTML = linebreak(interim_transcript);
            };
        }



        var two_line = /\n\n/g;
        var one_line = /\n/g;
        function linebreak(s) {
            return s.replace(two_line, '<p></p>').replace(one_line, '<br>');
        }

        var first_char = /\S/;
        function capitalize(s) {
            return s.replace(first_char, function (m) { return m.toUpperCase(); });
        }

        function startButton(event) {
            controlname = "#ctl00_ContentPlaceHolder1_txtFreeForm";
            if (recognizing) {
                recognition.stop();
                return;
            }
            final_transcript = '';
            recognition.lang = 'en';
            recognition.start();
            ignore_onend = false;
            final_span.innerHTML = '';
            interim_span.innerHTML = '';
            //showInfo('info_allow');
            //showButtons('none');
            start_timestamp = event.timeStamp;
        }

        function startButton1(event) {
            controlname = "#ctl00_ContentPlaceHolder1_txtFreeFormCC";
            if (recognizing) {
                recognition.stop();
                return;
            }
            final_transcript = '';
            recognition.lang = 'en';
            recognition.start();
            ignore_onend = false;
            final_span1.innerHTML = '';
            interim_span1.innerHTML = '';
            //showInfo('info_allow');
            //showButtons('none');
            start_timestamp = event.timeStamp;
        }
    </script>
</asp:Content>
