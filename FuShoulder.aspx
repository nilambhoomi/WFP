﻿<%@ Page Title="" Language="C#" MasterPageFile="~/AddFollowUpMaster.master" AutoEventWireup="true" CodeFile="FuShoulder.aspx.cs" Inherits="FuShoulder" %>

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
                            <label class="control-label">The patient complains of</label>
                            <label class="control-label">left</label>
                            <label class="control-label">shoulder pain that is</label>
                            <asp:TextBox runat="server" ID="txtPainScaleLeft" Width="40px"></asp:TextBox>
                            <label class="control-label">/10, with 10 being the worst, which is,</label>
                            <asp:CheckBox ID="chkContentLeft" runat="server" Text="constant" />
                            <asp:CheckBox ID="chkIntermittentLeft" runat="server" Text="intermittent." />
                            <asp:CheckBox ID="chkSharpLeft" runat="server" Text="sharp  " Checked="true" />
                            <asp:CheckBox ID="chkElectricLeft" runat="server" Text="electric,  " />
                            <asp:CheckBox ID="chkShootingLeft" runat="server" Text="shooting,  " Checked="true" />
                            <asp:CheckBox ID="chkThrobblingLeft" runat="server" Text="throbbing,  " />
                            <asp:CheckBox ID="chkPulsatingLeft" runat="server" Text="pulsating,  " />
                            <asp:CheckBox ID="chkDullLeft" runat="server" Text="dull,  " />
                            <asp:CheckBox ID="chkAchyLeft" runat="server" Text="achy in nature.  " />
                            <label class="control-label">The shoulder pain worsens with</label>
                            <asp:CheckBox ID="chkWorseLyingLeft" runat="server" Text="lying,  " />
                            <asp:CheckBox ID="chkWorseMovementLeft" runat="server" Text="raising the arm,  " Checked="true" />
                            <%--<asp:CheckBox ID="chkWorseRaisingLeft" runat="server" Text="pulsating,  " />--%>
                            <asp:CheckBox ID="chkWorseLiftingLeft" runat="server" Text="lifting objects,  " Checked="true" />
                            <asp:CheckBox ID="chkWorseRotationLeft" runat="server" Text="rotation,  " />
                            <asp:CheckBox ID="chkWorseWorkingLeft" runat="server" Text="working,  " />
                            <asp:CheckBox ID="chkWorseActivitiesLeft" runat="server" Text="overhead activities. " />
                            <label class="control-label">The shoulder pain is improved with</label>
                            <asp:CheckBox ID="chkImprovedRestingLeft" runat="server" Text="resting,  " />
                            <asp:CheckBox ID="chkImprovedMedicationLeft" runat="server" Text="medication,  " />
                            <asp:CheckBox ID="chkImprovedTherapyLeft" runat="server" Text="therapy,  " />
                            <asp:CheckBox ID="chkImprovedSleepingLeft" runat="server" Text="sleeping." /><br />
                        </div>
                        <br />

                        <div id="wrpRight" runat="server">

                            <label class="control-label">The patient complains of</label>
                            <label class="control-label">right</label>
                            <label class="control-label">shoulder pain that is</label>
                            <asp:TextBox runat="server" ID="txtPainScaleRight" Width="40px"> </asp:TextBox>
                            <label class="control-label">/10, with 10 being the worst, which is,</label>
                            <asp:CheckBox ID="chkContentRight" runat="server" Text="constant" />
                            <asp:CheckBox ID="chkIntermittentRight" runat="server" Text="intermittent." />
                            <asp:CheckBox ID="chkSharpRight" runat="server" Text="sharp  " Checked="true" />
                            <asp:CheckBox ID="chkElectricRight" runat="server" Text="electric,  " />
                            <asp:CheckBox ID="chkShootingRight" runat="server" Text="shooting,  " Checked="true" />
                            <asp:CheckBox ID="chkThrobblingRight" runat="server" Text="throbbing,  " />
                            <asp:CheckBox ID="chkPulsatingRight" runat="server" Text="pulsating,  " />
                            <asp:CheckBox ID="chkDullRight" runat="server" Text="dull,  " />
                            <asp:CheckBox ID="chkAchyRight" runat="server" Text="achy in nature.  " />
                            <label class="control-label">The shoulder pain worsens with </label>
                            <asp:CheckBox ID="chkWorseLyingRight" runat="server" Text="lying,  " />
                            <asp:CheckBox ID="chkWorseMovementRight" runat="server" Text="raising the arm,  " Checked="true" />
                            <%--<asp:CheckBox ID="chkWorseRaisingRight" runat="server" Text="pulsating,  " />--%>
                            <asp:CheckBox ID="chkWorseLiftingRight" runat="server" Text="lifting objects,  " Checked="true" />
                            <asp:CheckBox ID="chkWorseRotationRight" runat="server" Text="rotation,  " />
                            <asp:CheckBox ID="chkWorseWorkingRight" runat="server" Text="working,  " />
                            <asp:CheckBox ID="chkWorseActivitiesRight" runat="server" Text="overhead activities. " />
                            <label class="control-label">The shoulder pain is improved with </label>
                            <asp:CheckBox ID="chkImprovedRestingRight" runat="server" Text="resting,  " />
                            <asp:CheckBox ID="chkImprovedMedicationRight" runat="server" Text="medication,  " />
                            <asp:CheckBox ID="chkImprovedTherapyRight" runat="server" Text="therapy,  " />
                            <asp:CheckBox ID="chkImprovedSleepingRight" runat="server" Text="sleeping." />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"></label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px;">
                        <label class="control-label">ROM</label><br />
                        <table style="width: 40%;">
                            <thead>
                                <tr>
                                    <td style="text-align: left;">Upper Extremities
                                    </td>
                                    <td style="">Left
                                    </td>
                                    <td style="">Right
                                    </td>
                                    <td style="">Normal
                                    </td>
                                </tr>
                            </thead>
                            <tbody>

                                <tr>
                                    <td style="text-align: left;">Abduction</td>
                                    <td>
                                        <asp:TextBox ID="txtAbductionLeftWas" Width="50px" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtAbductionLeft" runat="server" Width="50px"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtAbductionRightWas" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                </tr>

                                <tr>
                                    <td style="text-align: left;">Flexion</td>
                                    <td>
                                        <asp:TextBox ID="txtFlexionLeftWas" Width="50px" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtFlexionLeft" runat="server" Width="50px"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtFlexionRightWas" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">Ext. rotation</td>
                                    <td>
                                        <asp:TextBox ID="txtExtRotationLeftWas" Width="50px" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtExtRotationLeft" Width="50px" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtExtRotationRightWas" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">Int. rotation</td>
                                    <td>
                                        <asp:TextBox ID="txtIntRotationLeftWas" Width="50px" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtIntRotationLeft" Width="50px" runat="server"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtIntRotationRightWas" ReadOnly="true" Width="50px" runat="server"></asp:TextBox></td>
                                </tr>
                            </tbody>
                        </table>
                        <br />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeFormCC" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <button type="button" id="start_button1" onclick="startButton1(event)">
                            <img src="images/mic.gif" alt="start" /></button>
                        <div style="display: none"><span class="final" id="final_span1"></span><span class="interim" id="interim_span1"></span></div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label"><b><u>PHYSICAL EXAM:</u></b></label>
                    </div>

                    <%--      <div class="col-md-9" style="margin-top: 5px">

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
                            <label class="control-label">The shoulder exam reveals tenderness upon palpation of the left </label>
                            <asp:TextBox runat="server" ID="txtPalpationText1Left" Style="margin-left: 0px;"></asp:TextBox>
                            <%--<asp:TextBox runat="server" ID="txtPalpationText2Left" Style="margin-left: 0px;" Width="70px"> </asp:TextBox><br />--%>

                            <asp:CheckBox ID="chkACJointLeft" runat="server" Text="AC joint  " Checked="true" />
                            <asp:CheckBox ID="chkGlenohumeralLeft" runat="server" Text="glenohumeral region  " />
                            <asp:CheckBox ID="chkCorticoidLeft" runat="server" Text="corticoid region  " Checked="true" />
                            <asp:CheckBox ID="chkSupraspinatusLeft" runat="server" Text="supraspintus tendon  " />
                            <asp:CheckBox ID="chkScapularLeft" runat="server" Text="scapular region  " />
                            <asp:CheckBox ID="chkDeepLabralLeft" runat="server" Text="deep labral region with muscle spasm present at " />
                            <asp:CheckBox ID="chkDeltoidLeft" runat="server" Text="deltoid muscle  " Checked="true" />
                            <asp:CheckBox ID="chkTrapeziusLeft" runat="server" Text="trapezius muscule  " Checked="true" />
                            <asp:CheckBox ID="chkEccymosisLeft" runat="server" Text="ecchymosis  " />
                            <asp:CheckBox ID="chkEdemaLeft" runat="server" Text="edema  " />
                            <asp:CheckBox ID="chkRangeOfMotionLeft" runat="server" Text="severe limitation in range of motion." />
                        </div>
                        <br />
                        <div id="wrpPERight" runat="server">
                            <label class="control-label">The shoulder exam reveals tenderness upon palpation of the right</label>
                            <asp:TextBox runat="server" ID="txtPalpationText1Right" Style="margin-left: 0px;"></asp:TextBox>
                            <%--<asp:TextBox runat="server" ID="txtPalpationText2Right" Style="margin-left: 0px;" Width="70px"> </asp:TextBox><br />--%>
                            <asp:CheckBox ID="chkACJointRight" runat="server" Text="AC joint  " Checked="true" />
                            <asp:CheckBox ID="chkGlenohumeralRight" runat="server" Text="glenohumeral region  " />
                            <asp:CheckBox ID="chkCorticoidRight" runat="server" Text="corticoid region  " Checked="true" />
                            <asp:CheckBox ID="chkSupraspinatusRight" runat="server" Text="supraspintus tendon  " />
                            <asp:CheckBox ID="chkScapularRight" runat="server" Text="scapular region  " />
                            <asp:CheckBox ID="chkDeepLabralRight" runat="server" Text="deep labral region with muscle spasm present at " />
                            <asp:CheckBox ID="chkDeltoidRight" runat="server" Text="deltoid muscle  " Checked="true" />
                            <asp:CheckBox ID="chkTrapeziusRight" runat="server" Text="trapezius muscule  " Checked="true" />
                            <asp:CheckBox ID="chkEccymosisRight" runat="server" Text="ecchymosis  " />
                            <asp:CheckBox ID="chkEdemaRight" runat="server" Text="edema  " />
                            <asp:CheckBox ID="chkRangeOfMotionRight" runat="server" Text="severe limitation in range of motion." />
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
                                    <td style="">Neer's
                                    </td>
                                    <td style="">Hawkins
                                    </td>
                                    <td style="">Yergason's
                                    </td>
                                    <td style="">Drop arm test
                                    </td>
                                    <td style="">Reverse beer can test
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td style="text-align: left;">Left</td>
                                    <td>
                                        <asp:CheckBox ID="chkNeerLeft" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkHawkinLeft" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkYergasonsLeft" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkDropArmLeft" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkReverseBeerLeft" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">Right</td>
                                    <td>
                                        <asp:CheckBox ID="chkNeerRight" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkHawkinRight" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkYergasonsRight" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkDropArmRight" runat="server" /></td>
                                    <td>
                                        <asp:CheckBox ID="chkReverseBeerRight" runat="server" /></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div id="divTP" runat="server">
                    <div class="row">
                        <div class="col-md-3">
                            <label class="control-label"></label>
                        </div>
                        <div class="col-md-9" style="margin-top: 5px">
                            <asp:Label Style="" ID="Label6" runat="server" Text="There are palpable taut bands/trigger points with referral patterns as depicted below." Font-Bold="False"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <label class="control-label"><b><u>Trigger Point:</u></b></label>
                        </div>
                        <div class="col-md-9" style="margin-top: 5px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="cboTPSide1" Style="height: 30px; width: 200px" DataSourceID="TPside1XML" DataTextField="name" runat="server"></asp:DropDownList>
                                        <asp:XmlDataSource ID="TPside1XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                        <asp:TextBox ID="txtTPText1" Style="margin-left: 20px;" runat="server" Text="deltoid" Width="557px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="cboTPSide2" Style="height: 30px; width: 200px" DataSourceID="TPside2XML" DataTextField="name" runat="server"></asp:DropDownList>
                                        <asp:XmlDataSource ID="TPside2XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                        <asp:TextBox ID="txtTPText2" Style="margin-left: 20px;" runat="server" Text="supraspinatus" Width="558px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="cboTPSide3" Style="height: 30px; width: 200px" DataSourceID="TPside3XML" DataTextField="name" runat="server"></asp:DropDownList>
                                        <asp:XmlDataSource ID="TPside3XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                        <asp:TextBox ID="txtTPText3" Style="margin-left: 20px;" runat="server" Text="infraspinatus" Width="558px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="cboTPSide4" Style="height: 30px; width: 200px" DataSourceID="TPside4XML" DataTextField="name" runat="server"></asp:DropDownList>
                                        <asp:XmlDataSource ID="TPside4XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                        <asp:TextBox ID="txtTPText4" Style="margin-left: 20px;" runat="server" Text="teres minor" Width="558px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="cboTPSide5" Style="height: 30px; width: 200px" DataSourceID="TPside5XML" DataTextField="name" runat="server"></asp:DropDownList>
                                        <asp:XmlDataSource ID="TPside5XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                        <asp:TextBox ID="txtTPText5" Style="margin-left: 20px;" runat="server" Text="teres major" Width="559px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="cboTPSide6" Style="height: 30px; width: 200px" DataSourceID="TPside6XML" DataTextField="name" runat="server"></asp:DropDownList>
                                        <asp:XmlDataSource ID="TPside6XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                        <asp:TextBox ID="txtTPText6" Style="margin-left: 20px;" runat="server" Text="pectoralis major" Width="560px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="cboTPSide7" Style="height: 30px; width: 200px" DataSourceID="TPside7XML" DataTextField="name" runat="server"></asp:DropDownList>
                                        <asp:XmlDataSource ID="TPside7XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                        <asp:TextBox ID="txtTPText7" Style="margin-left: 20px;" runat="server" Text="biceps" Width="560px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="cboTPSide8" Style="height: 30px; width: 200px" DataSourceID="TPside8XML" DataTextField="name" runat="server"></asp:DropDownList>
                                        <asp:XmlDataSource ID="TPside8XML" runat="server" DataFile="~/xml/HSMData.xml" XPath="HSM/sTPSides/TPSide" />
                                        <asp:TextBox ID="txtTPText8" Style="margin-left: 20px;" runat="server" Text="triceps" Width="560px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" Style="" ID="txtFreeForm" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <button type="button" id="start_button" onclick="startButton(event)">
                            <img src="images/mic.gif" alt="start" /></button>
                        <div style="display: none"><span class="final" id="final_span"></span><span class="interim" id="interim_span"></span></div>
                    </div>
                </div>

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
                        <%--  <asp:CheckBox ID="chkCervicalSpine" Style="" Text="MRI" runat="server" />
                                <asp:DropDownList ID="cboScanType" Style=" height: 25px;" runat="server"></asp:DropDownList>
                                <asp:Label ID="Label7" Style="" Text=" of the cervical spine " runat="server"></asp:Label>
                                <asp:TextBox ID="txtToRuleOut" runat="server" Style=" " Text="to rule out herniated nucleus pulposus/soft tissue injury " Width="299px"></asp:TextBox>--%>
                        <%--OnClick="AddStd_Click"--%>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">
                        <label class="control-label">Notes:</label>
                    </div>
                    <div class="col-md-9" style="margin-top: 5px">
                        <asp:TextBox runat="server" ID="txtFreeFormP" Style="" TextMode="MultiLine" Width="700px" Height="100px"></asp:TextBox>
                        <asp:ImageButton ID="AddStd" Style="display: none" runat="server" Height="50px" Width="50px" ImageUrl="~/img/a1.png" PostBackUrl="~/AddStandards.aspx" OnClientClick="basicPopup();return false;" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3"></div>
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
                                <%-- <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HiddenField ID="hfFname" runat="server" Value='<%# Eval("ProcedureDetail_ID") %>' />
                    </ItemTemplate>
                                      </asp:TemplateField>--%>
                                <%--<asp:TemplateField HeaderText="BodyPart" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("BodyPart") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <%-- <asp:TemplateField HeaderText="Heading" ItemStyle-Width="450">
                                    <ItemTemplate>--%>
                                <%--<asp:Label ID="lblheading" runat="server" Text='<%# Eval("Heading") %>'></asp:Label>--%>
                                <%-- <asp:TextBox ID="txtHeading" runat="server" CssClass="form-control" Width="400px"  TextMode="MultiLine" Text='<%# Eval("Heading") %>'></asp:TextBox>
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
                        <%--<asp:ImageButton ID="LoadDV" Style="" runat="server" OnClick="LoadDV_Click" ImageUrl="~/img/edit.gif" />--%>
                        <div style="display: none">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn blue" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

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

    <%--   </ContentTemplate>
        </asp:UpdatePanel>--%>
    <script type="text/javascript">
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
            $('#ctl00_lnkbtn_Shoulder').addClass('active');
        });


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
