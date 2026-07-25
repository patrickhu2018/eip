<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="item_addapply.aspx.cs" Inherits="item_addapply" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/0.4.1/html2canvas.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.2/jspdf.min.js"></script>
    <script>
        function showModal1() {
            $('#modal1').modal('show');
            return false;
        }
        function hideModal1() {
            $('#modal1').modal('hide');
            return false;
        }
        function showModal2() {
            $('#modal2').modal('show');
            return false;
        }
        function hideModal2() {
            $('#modal2').modal('hide');
            return false;
        }
        function showusermd() {
            $('#usermd').modal('show');
            return false;
        }
        function hideusermd() {
            $('#usermd').modal('hide');
            return false;
        }
        function showcustodianmd() {
            $('#custodianmd').modal('show');
            return false;
        }
        function hidecustodianmd() {
            $('#custodianmd').modal('hide');
            return false;
        }
        function captureAndPrint() {
            html2canvas(document.body).then(function (canvas) {
                var imgData = canvas.toDataURL('image/png');

                // 創建一個隱藏的 iframe
                var iframe = document.createElement('iframe');
                iframe.style.position = 'absolute';
                iframe.style.width = '0';
                iframe.style.height = '0';
                iframe.style.border = 'none';
                document.body.appendChild(iframe);

                // 將圖片插入到 iframe 中
                var iframeDoc = iframe.contentWindow.document;
                iframeDoc.open();
                iframeDoc.write('<html><head><title>Print</title></head><body>');
                iframeDoc.write('<img src="' + imgData + '" style="width:100%" id="printImage"/>');
                iframeDoc.write('</body></html>');
                iframeDoc.close();

                // 確保圖片加載完成後再列印
                var printImage = iframe.contentWindow.document.getElementById('printImage');
                printImage.onload = function () {
                    // 確保圖片加載完畢後再進行列印
                    iframe.contentWindow.focus();
                    iframe.contentWindow.print();

                    // 刪除 iframe
                    document.body.removeChild(iframe);
                };

                // 如果圖片已經加載完成，手動觸發 onload
                if (printImage.complete) {
                    printImage.onload();
                }
            });
        }
        var referrer = document.referrer;

        if (referrer) {
            var url = new URL(referrer);
            var path = url.pathname.trim();
            if (path === "/item_listquery.aspx") {
                $(".menuIcon02").addClass("active");
            } else if (path === "/item_myapply.aspx") {
                $(".menuIcon01").addClass("active");
            }
        }



        function getUrlParameter(name) {
            var url = window.location.href; // 獲取當前頁面的 URL
            var regex = new RegExp("[?&]" + name + "=([^&]*)");
            var result = regex.exec(url);
            return result === null ? "" : decodeURIComponent(result[1]);
        }


        function checkPositiveNumber(input) {
            var value = input.value;
            if (value <= 0) {
                input.value = "";
                alert("請輸入正數");
            }
        }

        function printScreen() {
            var content = document.getElementById('ContentPlaceHolder1_print_all').innerHTML;
            var printWindow = window.open('', '', 'height=600,width=800');
            printWindow.document.write('<html><head><title></title>');
            printWindow.document.write('<style>body { font-family: Arial, sans-serif; }</style>');
            printWindow.document.write('</head><body>');
            printWindow.document.write(content);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.print();
        }
    </script>
    <style>
        .bt {
            padding: 10px 30px;
            background: #4CAF1E 0% 0% no-repeat padding-box;
            border: 0px;
            border-radius: 5px;
            opacity: 1;
            text-align: center;
            color: #FFFFFF;
            margin-right: 10px;
            padding: 10px 30px;
        }

        .printbt {
            padding: 10px 30px 10px 50px;
            border: 0px;
            border-radius: 5px;
            background-image: url(image/icon_19.png);
            background-color: #37A4C7;
            background-repeat: no-repeat;
            background-position-y: 50%;
            background-position-x: 9%;
            opacity: 1;
            text-align: center;
            color: #FFFFFF;
            margin-right: 10px;
        }


        .addbt {
            height: auto;
            padding: 10px 0;
            max-width: 60px;
            border: 0px solid #ECECEC;
            background: #F7931E 0% 0% no-repeat padding-box;
            border-radius: 5px;
            opacity: 1;
            text-align: center;
            font: normal normal normal 16px/12px Microsoft JhengHei;
            letter-spacing: 0px;
            color: #FFFFFF;
        }

        .del_btn {
            background-image: url(image/icon_17.png);
            background-repeat: no-repeat;
            background-position-y: 50%;
            background-position-x: 9%;
            padding-left: 26px;
            color: #FFFFFF;
            border-radius: 3px;
            opacity: 1;
            border: 0px;
        }

        .modal-backdrop {
            z-index: 0;
            background-color: rgba(0, 0, 0, 1); /* 調整透明度 */
        }

        .timeline {
            position: relative;
            margin: 20px 0;
            border-left: 2px solid #8B4513; /* 棕色線條 */
        }

        /* 每個項目 */
        .item {
            position: relative;
            margin-bottom: 20px;
        }

            /* 圓圈標記 */
            .item .marker {
                width: 12px;
                height: 12px;
                background-color: #643C19; /* 棕色 */
                border-radius: 50%;
                position: absolute;
                left: -7px; /* 將圓圈移到線條中心 */
                top: 0;
            }

            /* 文字內容 */
            .item .text {
                margin-left: 20px;
                font-size: 1.125rem;
                color: #333;
            }

        .text h5 {
            font-weight: bold;
            font-size: 1.25rem;
            color: #643C19;
        }

        .text span {
            margin-left: 3px;
        }

        @media(max-width:1360px) {
            #sub {
                margin-left: 71px;
            }
        }


        .write_TitleMark {
            display: flex !important;
            justify-content: space-between !important;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div runat="server" id="print_all">
        <div class="write_Box">
            <div class="write_Title write_TitleMark">
                <h5>
                    <asp:Literal runat="server" ID="box_title">申請登錄</asp:Literal></h5>
                <span class="subTitleMarkText">
                    <span>註：1. 灰底為系統自動帶入的資料</span>
                    <span class="leftDistance">2. 全部為必填</span>
                </span>
            </div>
            <div class="write_textBox">
                <div class="dataBox row">
                    <div class="col-2 formItem title3">
                        <div class="ItemTitle"><span>申請日期</span></div>
                        <div class="ItemContent">
                            <asp:TextBox ID="apply_date" runat="server" TextMode="Date" CssClass="form-control" Style="width: 200px; height: 35px; margin-left: 10px;" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="dataBox row">
                    <div class="col-4 formItem title3">
                        <div class="ItemTitle"><span>申請組室</span></div>
                        <div class="ItemContent">
                            <asp:TextBox ID="apply_group" CssClass="form-control" runat="server" Style="height: 35px; margin-left: 10px;" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-2 formItem title3">
                        <div class="ItemTitle"><span>申請人</span></div>
                        <div class="ItemContent" style="padding-left: 3em;">
                            <asp:TextBox ID="apply_user" CssClass="form-control" runat="server" Style="width: 138px; height: 35px; margin-left: 10px;" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="write_textBox">
                <div class="dataBox row pb-3">
                    <div class="col-12 formItem title3" style="position: relative;">
                        <div class="ItemTitle"><span>品名</span></div>
                        <div class="ItemContent d-flex  align-items-baseline">
                            <asp:DropDownList ID="product" runat="server" CssClass="form-control" Style="height: 35px; width: 250px; margin-left: 10px; margin-right: 15px;" AutoPostBack="true" OnSelectedIndexChanged="product_SelectedIndexChanged" />
                            <asp:Label ID="lastbuy" runat="server" Text="" Style="font-size: 14px; color: #777777; padding-right: 1em"></asp:Label>

                            <div id="myDiv" style="position: absolute; top: 100%; width: 140%; font-size: 14px; color: #145597; margin-left: 10px">
                                註：選擇所購買物品的品名，如果選不到品名請洽秘書室承辦人員
                            </div>
                        </div>
                    </div>
                </div>

                <%--<label Style="font-size: 14px; color: #145597;margin-left:5.5em">註：選擇所購買物品的品名，如果選不到品名請洽秘書室承辦人員</label>--%>

                <div class="dataBox row">
                    <div class="col-3  col-lg-3 col-xl-3 formItem title3">
                        <div class="ItemTitle"><span>單價</span></div>
                        <div class="ItemContent" style="white-space: nowrap">
                            <asp:TextBox ID="txtPrice" CssClass="form-control" runat="server" Style="height: 35px; margin-left: 10px; width: 80px;" OnTextChanged="txtPrice_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <span style="margin-left: 20px;">元</span>
                        </div>

                    </div>
                    <div class="col-3 formItem title3">
                        <div class="ItemTitle"><span>數量</span></div>
                        <div class="ItemContent" style="padding-left: 3em;">
                            <%--<asp:DropDownList ID="txtCount" runat="server" CssClass="form-control" Style="width: 120px;" OnSelectedIndexChanged="count_SelectedIndexChanged" AutoPostBack="true" />--%>
                            <asp:TextBox ID="txtCount" runat="server" CssClass="form-control" TextMode="Number" Style="width: 120px;" oninput="checkPositiveNumber(this);" OnTextChanged="count_TextChanged" AutoPostBack="true" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="write_textBox">
                <div class="dataBox row">
                    <div class="col-5 formItem title3">
                        <div class="ItemTitle"><span>合計</span></div>
                        <div class="ItemContent" style="white-space: nowrap">
                            <asp:TextBox ID="total" CssClass="form-control" runat="server" Style="height: 35px; margin-left: 10px; width: 120px;"></asp:TextBox>
                            <span style="margin-left: 5px;">元</span>
                        </div>
                        <span style="font-size: 14px; color: #145597; flex-wrap: wrap; white-space: nowrap; padding-left: 1em">自動計算：數量x單價，可自行更改合計數字</span>
                    </div>
                </div>
            </div>
        </div>
        <asp:Panel ID="pl" runat="server" Visible="false">
            <div class="write_Box">
                <div class="write_Title">
                    <h5>
                        <asp:Literal runat="server" ID="Literal1">採購登錄</asp:Literal></h5>
                </div>
                <div class="write_textBox">
                    <div class="dataBox row d-flex" style="flex; align-items: center;">
                        <div class="col-4 col-sm-4 col-md-4 col-lg-3 formItem title3">
                            <div class="ItemTitle"><span>採購日期</span><span style="color: red">*</span></div>
                            <div class="ItemContent">
                                <asp:TextBox ID="pass_date" runat="server" TextMode="Date" CssClass="form-control" Style="min-width: 150px; height: 35px; margin-left: 0px;"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-3 formItem title3" style="position: relative;" id="sub">
                            <div class="ItemTitle"><span>預算科目</span><span style="color: red">*</span></div>
                            <div class="ItemContent">
                                <asp:DropDownList ID="Budget" runat="server" CssClass="form-control" Style="min-width: 300px" AutoPostBack="true" />

                                <div style="position: absolute; top: 100%; width: 140%; font-size: 14px; color: #145597; flex-wrap: wrap; white-space: nowrap;">
                                    註：如選不到合適科目請洽秘書室承辦人員
                                </div>
                            </div>

                        </div>
                    </div>

                    <%-- <span style="font-size: 14px; color: #145597; flex-wrap: wrap; white-space: nowrap;margin-left:26rem">註：如選不到合適科目請洽秘書室承辦人員</span>--%>
                </div>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="write_textBox">
                            <div class="dataBox row" style="display: flex; align-items: center;">
                                <!-- 使用人欄位 -->
                                <div class="col-6 formItem title3">
                                    <div class="ItemTitle"><span>使用人</span></div>
                                    <div runat="server" class="ItemContent d-flex align-items-center">
                                        <asp:TextBox ID="user_list" runat="server" CssClass="form-control" Enabled="false" />
                                    </div>
                                </div>
                                <asp:Button ID="checkuser" runat="server" Text="編輯" CssClass="addbt" OnClientClick="return showusermd();" UseSubmitBehavior="false" />


                            </div>
                            <div style="font-size: 14px; margin-left: 5rem; color: #145597; flex-wrap: wrap; white-space: nowrap; display: flex; align-items: center">註：請點
                                <div class="addbt" style="zoom: 80%; margin: 0px 0.5em; padding: 8px 0; max-width: 40px">編輯   </div>
                                填寫資料</div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="write_textBox">
                            <div class="dataBox row" style="display: flex; align-items: center;">
                                <!-- 保管人欄位 -->
                                <div class="col-6 formItem title3">
                                    <div class="ItemTitle"><span>保管人</span></div>
                                    <div class="ItemContent d-flex  align-items-center">
                                        <asp:TextBox ID="custodian_list" runat="server" CssClass="form-control" Enabled="false" />
                                    </div>
                                </div>
                                <asp:Button ID="checkcustodian" runat="server" Text="編輯" CssClass="addbt" OnClientClick="return showcustodianmd();" UseSubmitBehavior="false" />
                            </div>
                            <div style="font-size: 14px; margin-left: 5rem; color: #145597; flex-wrap: wrap; white-space: nowrap; display: flex; align-items: center">註：請點
                                <div class="addbt" style="zoom: 80%; margin: 0px 0.5em; padding: 8px 0; max-width: 40px">編輯   </div>
                                填寫資料</div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="write_textBox">
                    <div class="dataBox row">
                        <div class="col-6 formItem title3">
                            <div class="ItemTitle" style="height: 100%"><span>備註</span></div>
                            <div class="ItemContent">
                                <asp:TextBox ID="note" runat="server" CssClass="form-control" TextMode="MultiLine" Columns="90" placeholder="請填寫"></asp:TextBox>
                            </div>
                            <span style="font-size: 14px; color: #145597; flex-wrap: wrap; white-space: nowrap; margin-left: 1em;">註：限100字以內</span>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="d-flex  justify-content-center">
        <asp:Button ID="Cancel" class="bt" runat="server" Text="取消" Style="background: #777777" OnClick="Cancel_Click" />
        <asp:Button ID="Submit" class="bt" runat="server" Text="提送" OnClick="Submit_Click" />
    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <div class="modal fade bs-NewUser-modal-lg" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 50%;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span id="Span3" runat="server" style="color: #003168">提交確認</span></b>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content" role="form" class="form-horizontal">
                            <div class="Box" style="padding: 30px 60px;">
                                <div class="timeline">
                                    <div class="item">
                                        <div class="marker"></div>
                                        <div class="text">
                                            <h5>申請人資訊</h5>
                                            <div class="d-flex justify-content-start flex-wrap row" style="padding: 15px 0">
                                                <div class="col-lg-6 col-md-12 col-sm-12 ">申請組室：<span id="mdgroup" runat="server"></span></div>
                                                <div class="col-lg-6 col-md-12 col-sm-12 ">申請人：<span id="mduser" runat="server"></span></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="item">
                                        <div class="marker"></div>
                                        <div class="text">
                                            <h5>申請品項</h5>
                                            <div class="d-flex  justify-content-start flex-wrap" style="padding: 10px 0">
                                                <div class="col-4 pl-0">品名：<span id="mdproduct" runat="server"></span></div>
                                            </div>
                                            <div class="d-flex  justify-content-start flex-wrap row" style="padding: 15px 0;">
                                                <div class="col-6 ">單價：<span id="mdprice" runat="server" style="color: #145597; margin-right: 10px;"></span>元</div>
                                                <div class="col-6 ">數量：<span id="mdcount" runat="server" style="color: #145597;"></span></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="item">
                                        <div class="marker"></div>
                                        <div class="text">
                                            <h5>合計價錢<span style="font-size: 0.875rem; color: #707070; margin-left: 5px;">*註數量x單價或是您填寫的合計金額</span></h5>
                                            <div class="d-flex  justify-content-start row" style="padding: 15px 0; white-space: nowrap">
                                                <div class="col-4">合計：<span id="mdtotal" runat="server" style="color: #145597; margin-right: 10px;"></span>元</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <hr>
                        </div>

                        <div style="text-align: center">
                            <asp:Button ID="print" runat="server" Text="列印" class="printbt" OnClientClick="captureAndPrint(); return false;" />
                            <asp:Button ID="Btclose" runat="server" Text="取消" class="bt" Style="background-color: #777777"
                                OnClientClick="return hideModal1();" />
                            <asp:Button ID="Butsubmit" runat="server" Text="提交" class="bt"
                                OnClientClick="return showModal2();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade bs-NewUser-modal-lg" id="modal2" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 380px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span id="Span4" runat="server" style="color: #003168">提交確認</span></b>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content2" role="form" class="form-horizontal text-center" style="margin-top: 50px; margin-bottom: 50px;">
                            <b style="font-weight: bold; font-size: 1.125rem">確定要提交嗎?</b>
                            <br>
                            <b style="color: #B83F1F; font-weight: bold; font-size: 1.125rem">提交後無法修改及刪除</b>
                        </div>
                        <hr>
                        <div style="text-align: center">
                            <asp:Button ID="Button1" runat="server" Text="列印" class="printbt" Visible="false" OnClientClick="printScreen(); return false;" />
                            <asp:Button ID="Button2" runat="server" Text="取消" class="bt" Style="background: #777777"
                                OnClientClick="return hideModal2();" />
                            <asp:Button ID="Button3" runat="server" Text="提交" class="bt mr-0"
                                OnClick="Butsubmit_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade bs-NewUser-modal-lg" id="usermd" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 500px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span id="Span2" runat="server" style="color: #003168">編輯</span></b>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content3" role="form" class="form-horizontal text-center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="d-flex align-items-center flex-wrap justify-content-center">
                                        <span class="mr-3">使用人</span>
                                        <asp:TextBox ID="user_name" runat="server" CssClass="form-control" Style="margin-right: 20px; width: 20%" />
                                        <span class="mr-3">使用數量</span>
                                        <asp:TextBox ID="user_quantity" runat="server" CssClass="form-control" TextMode="Number" Style="margin-right: 10px; width: 20%" oninput="checkPositiveNumber(this);" />
                                        <asp:Button ID="adduser" runat="server" Text="新增" CssClass="addbt" Style="margin-right: 5px" OnClick="adduser_Click" />
                                    </div>
                                    <hr style="margin-bottom: 30px;" />
                                    <asp:Label ID="stocks" Text="" runat="server" Style="color: #145597; font-size: 0.8rem; position: absolute; top: 70px; left: 15px;" />
                                    <asp:GridView ID="user_gv" class="table table-bordered mt-2 position-relative" runat="server" AutoGenerateColumns="False" OnRowCommand="user_gv_RowCommand" PageSize="10" AllowPaging="true" OnPageIndexChanging="user_gv_PageIndexChanging" PagerSettings-Visible="false">
                                        <EmptyDataTemplate>
                                            無資料
                                        </EmptyDataTemplate>
                                        <HeaderStyle CssClass="table-topbar" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="序列" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:Label ID="serial" runat="server"
                                                        Text='<%# (Container.DataItemIndex + 1) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="使用者" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="name" runat="server" Text='<%# Eval("user_name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="使用數量" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="user_quantity" runat="server" Text='<%# Eval("user_quantity") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="1%">
                                                <HeaderTemplate>
                                                    <span>功能</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" CssClass="del_btn" ID="del" CommandName="del" CommandArgument='<%# Eval("id") %>' Text="刪除" Style="background-color: #B83F1F" OnClientClick="return confirm('確定要刪除嗎？');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div id="down" runat="server" class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups" style="justify-content: center;">
                                        <div class="btn-group pull-right" role="group" aria-label="First group" style="align-items: center;">

                                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                                第<asp:Label ID="lblPageIndex" runat="server" Text="0" CssClass="Number" Style="color: #0071BC"></asp:Label>頁    
                                            </div>

                                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                                總計<asp:Label ID="lblDataCount" runat="server" Text="0" CssClass="Number" Style="color: #0071BC"></asp:Label>筆
                                            </div>

                                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                                <asp:LinkButton ID="lkbPagePrev" runat="server" OnClick="lkbPagePrev_Click" Style="color: #0071BC; text-decoration: underline;">上一頁</asp:LinkButton>
                                                <asp:LinkButton ID="lkbPageNext" runat="server" OnClick="lkbPageNext_Click" Style="color: #0071BC; text-decoration: underline;">下一頁</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <hr>
                        <div style="text-align: center">
                            <asp:Button runat="server" Text="返回" class="bt" Style="background: #777777"
                                OnClientClick="return hideusermd();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade bs-NewUser-modal-lg" id="custodianmd" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 500px;">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span id="Span1" runat="server" style="color: #003168">編輯</span></b>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content4" role="form" class="form-horizontal text-center">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <div class="d-flex align-items-center flex-wrap justify-content-center">
                                        <span class="mr-3">保管人</span>
                                        <asp:TextBox ID="custodian_name" runat="server" CssClass="form-control" Style="margin-right: 20px; width: 20%" />
                                        <span class="mr-3">保管數量</span>
                                        <asp:TextBox ID="custodian_quantity" runat="server" CssClass="form-control" TextMode="Number" Style="margin-right: 10px; width: 20%" oninput="checkPositiveNumber(this);" />
                                        <asp:Button ID="addquantity" runat="server" Text="新增" CssClass="addbt" Style="margin-right: 5px" OnClick="addquantity_Click" />
                                    </div>
                                    <hr style="margin-bottom: 30px;" />
                                    <asp:Label ID="custodian_stocks" Text="" runat="server" Style="color: #145597; font-size: 0.8rem; position: absolute; top: 70px; left: 15px;" />
                                    <asp:GridView ID="custodian_gv" class="table table-bordered mt-2 position-relative" runat="server" AutoGenerateColumns="False" OnRowCommand="custodian_gv_RowCommand" PageSize="10" AllowPaging="true" OnPageIndexChanging="custodian_gv_PageIndexChanging" PagerSettings-Visible="false">
                                        <EmptyDataTemplate>
                                            無資料
                                        </EmptyDataTemplate>
                                        <HeaderStyle CssClass="table-topbar" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="序列" ItemStyle-HorizontalAlign="center" ItemStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:Label ID="serial" runat="server"
                                                        Text='<%# (Container.DataItemIndex + 1) %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="id" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="id" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="保管人" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="name" runat="server" Text='<%# Eval("custodian_name") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="保管數量" ItemStyle-HorizontalAlign="center" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="user_quantity" runat="server" Text='<%# Eval("custodian_quantity") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="1%">
                                                <HeaderTemplate>
                                                    <span>功能</span>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Button runat="server" CssClass="del_btn" ID="del" CommandName="del" CommandArgument='<%# Eval("id") %>' Text="刪除" Style="background-color: #B83F1F" OnClientClick="return confirm('確定要刪除嗎？');" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div id="down2" runat="server" class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups" style="justify-content: center;">
                                        <div class="btn-group pull-right" role="group" aria-label="First group" style="align-items: center;">

                                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                                第<asp:Label ID="lblPageIndex2" runat="server" Text="0" CssClass="Number" Style="color: #0071BC"></asp:Label>頁    
                                            </div>

                                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                                總計<asp:Label ID="lblDataCount2" runat="server" Text="0" CssClass="Number" Style="color: #0071BC"></asp:Label>筆
                                            </div>

                                            <div class="NumberOfPages" style="padding: 5px; float: left">
                                                <asp:LinkButton ID="lkbPagePrev2" runat="server" OnClick="lkbPagePrev2_Click" Style="color: #0071BC; text-decoration: underline;">上一頁</asp:LinkButton>
                                                <asp:LinkButton ID="lkbPageNext2" runat="server" OnClick="lkbPageNext2_Click" Style="color: #0071BC; text-decoration: underline;">下一頁</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                        <hr>
                        <div style="text-align: center">
                            <asp:Button runat="server" Text="返回" class="bt" Style="background: #777777"
                                OnClientClick="return hidecustodianmd();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

