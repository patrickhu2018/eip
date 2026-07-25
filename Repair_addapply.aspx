<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Repair_addapply.aspx.cs" Inherits="Repair_addapply" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        //function showModal1() {

        //    $('#modal1').modal('show');
        //    return false;
        //}
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

        document.addEventListener('DOMContentLoaded', function () {
            var uploadButton = document.getElementById("ContentPlaceHolder1_p1");
            if (uploadButton) {
                uploadButton.onclick = function () {
                    var fileUpload = document.getElementById('<%= FileUpload1.ClientID %>');
                    if (fileUpload) {
                        fileUpload.click();
                    }
                };
            }
        });
        document.addEventListener('DOMContentLoaded', function () {
            var uploadButton = document.getElementById("ContentPlaceHolder1_p2");
            if (uploadButton) {
                uploadButton.onclick = function () {
                    var fileUpload = document.getElementById('<%= FileUpload2.ClientID %>');
                    if (fileUpload) {
                        fileUpload.click();
                    }
                };
            }
        });
        document.addEventListener('DOMContentLoaded', function () {
            var submit = document.getElementById("ContentPlaceHolder1_Submit");
            if (submit) {
                submit.onclick = function () {
                    var floor = document.getElementById("ContentPlaceHolder1_floor");
                    var location = document.getElementById("ContentPlaceHolder1_location");
                    var reason = document.getElementById("ContentPlaceHolder1_reason");
                    if (floor.value === "0") {
                        alert('樓層尚未選擇')
                        return false;
                    }
                    else if (location.value === "0") {
                        alert('位置尚未選擇')
                        return false;
                    }
                    else if (reason.value === "") {
                        alert('事由尚未填寫')
                        return false;
                    } else {
                        showModal1();
                        return false;
                    }
                    console.log(floor.value);

                };
            }
        });
        let selectedFiles = []; // 用於存儲選擇的檔案
        function showPreview(fileUploadControl, previewId) {
            var previewElement = document.getElementById('ContentPlaceHolder1_preview' + previewId);
            var mdpreviewElement = document.getElementById('modalPreview' + previewId)
            var modalMessage = document.getElementById('modalMessage');

            if (fileUploadControl.files && fileUploadControl.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    previewElement.src = e.target.result;
                    mdpreviewElement.src = e.target.result;
                    previewElement.style.display = 'block'; // 隱藏圖片
                    mdpreviewElement.style.display = 'block';
                    modalMessage.style.display = 'none';
                };

                reader.readAsDataURL(fileUploadControl.files[0]);
            }


            var delpic = document.getElementById('Delpreview' + previewId);
            delpic.style.display = 'block';
            //var bt = document.getElementById('bt' + previewId);
            //bt.style.display = 'none';
            //var img = document.querySelector('.image-wrapper');
            //img.style.height = '100%';
        }
        window.onload = function () {
            checkUploadedFile('1');
            checkUploadedFile('2');
        };

        // 檢查是否已經有圖片上傳
        function checkUploadedFile(previewId) {
            var fileUploadControl = document.getElementById('ContentPlaceHolder1_FileUpload' + previewId);
            var previewElement = document.getElementById('modalPreview' + previewId);
            var modalMessage = document.getElementById('modalMessage');

            if (fileUploadControl.files && fileUploadControl.files[0]) {
                previewElement.style.display = 'block'; // 顯示圖片
                modalMessage.style.display = 'none'; // 隱藏"無檔案上傳"
            } else {
                previewElement.style.display = 'none'; // 隱藏圖片
            }

            updateModalMessage();
        }
        function updateModalMessage() {
            var preview1Uploaded = document.getElementById('ContentPlaceHolder1_FileUpload1').files.length > 0;
            var preview2Uploaded = document.getElementById('ContentPlaceHolder1_FileUpload2').files.length > 0;
            var modalMessage = document.getElementById('modalMessage');

            if (preview1Uploaded && preview2Uploaded) {
                modalMessage.style.display = 'none'; // 隱藏無檔案上傳訊息
            } else if (preview1Uploaded) {
                modalMessage.style.display = 'none'; // 顯示檔案1的訊息
            } else if (preview2Uploaded) {
                modalMessage.style.display = 'none'; // 顯示檔案2的訊息
            } else {
                modalMessage.textContent = '無檔案上傳';
                modalMessage.style.display = 'block'; // 顯示無檔案上傳訊息
            }
        }
        function removeImage(previewId, fileUploadId) {
            var previewImage = document.getElementById('ContentPlaceHolder1_preview' + previewId);
            var previewElement = document.getElementById('modalPreview' + previewId);
            var fileUpload = document.getElementById('ContentPlaceHolder1_' + fileUploadId);

            previewImage.src = 'image/image.png'; // 恢復為預設圖片
            previewElement.style.display = 'none'; // 隱藏圖片

            // 清除選擇的檔案
            fileUpload.value = null;

            var delpic = document.getElementById('Delpreview' + previewId);
            delpic.style.display = "none";
            //var bt = document.getElementById('bt' + previewId);
            //bt.style.display = 'block';
            //var img = document.querySelector('.image-wrapper');
            //img.style.height = '80%';
            // 更新訊息
            updateModalMessage();
        }
        function showModal1() {
            document.getElementById('<%= mdgroup.ClientID %>').innerText = document.getElementById('<%= apply_group.ClientID %>').value;
            document.getElementById('<%= mduser.ClientID %>').innerText = document.getElementById('<%= apply_user.ClientID %>').value;
            var rb1 = document.getElementById('<%= rb1.ClientID %>');
            var rb2 = document.getElementById('<%= rb2.ClientID %>');
            if (rb1.checked) {
                document.getElementById('<%= mdplace.ClientID %>').innerText = "行政大樓";
            } else if (rb2.checked) {
                document.getElementById('<%= mdplace.ClientID %>').innerText = "工商大樓";
            }
            document.getElementById('<%= mdfloor.ClientID %>').innerText = document.getElementById('<%= floor.ClientID %>').options[document.getElementById('<%= floor.ClientID %>').selectedIndex].text;
            document.getElementById('<%= mdlocation.ClientID %>').innerText = document.getElementById('<%= location.ClientID %>').options[document.getElementById('<%= location.ClientID %>').selectedIndex].text;
            document.getElementById('<%= mdreason.ClientID %>').innerText = document.getElementById('<%= reason.ClientID %>').value;
            $('#modal1').modal('show'); // 顯示模態框
            return false;
        }
        var referrer = document.referrer;

        if (referrer) {
            var url = new URL(referrer);
            var path = url.pathname.trim();
            if (path === "/Repair_listquery.aspx") {
                $(".menuIcon02").addClass("active");
            } else if (path === "/Repair_myapply.aspx") {
                $(".menuIcon01").addClass("active");
            } else if (path === "/Repair_inventory_list.aspx") {
                $(".menuIcon09").addClass("active");
                $(".menuIcon05").addClass("active");
            } else if (path === "/Repair_management_query.aspx") {
                $(".menuIcon03").addClass("active");
            }
        }

    </script>
    <style>
        .bt {
            width: 92px;
            height: 44px;
            background: #4CAF1E 0% 0% no-repeat padding-box;
            border: 0px;
            border-radius: 5px;
            opacity: 1;
            text-align: center;
            color: #FFFFFF;
            margin: 0px 5px;
            padding: 10px 30px;
        }

        .addbtn {
            width: 120px;
            height: auto;
            border: 0px;
            color: #333333;
            background: #FFFFFF;
            background-image: url("../image/icon_16_g.png");
            background-position: -5px center;
            background-repeat: no-repeat;
            opacity: 1;
        }

        .delbt {
            width: 120px;
            height: auto;
            border: 0px;
            color: #333333;
            background: #FFFFFF;
            background-image: url("../image/icon_17_g.png");
            background-position: 12px center;
            background-repeat: no-repeat;
            opacity: 1;
        }



        .modal-backdrop {
            z-index: 0;
            background-color: rgba(0, 0, 0, 1); /* 調整透明度 */
        }

        .upload-container {
            position: relative;
            width: calc(var(--diameter)* 3 + var(--tipset)* 2.6);
            max-width: calc(var(--diameter)* 3 + var(--tipset)* 2.6);
            min-width: calc(var(--diameter)* 3 + var(--tipset)* 2.6);
            height: calc(var(--tipset)* 2.6 + var(--tipset)* 2.6);
            max-height: calc(var(--tipset)* 2.6 + var(--tipset)* 2.6);
            min-height: calc(var(--tipset)* 2.6 + var(--tipset)* 2.6);
            border: 1px solid #ccc;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-direction: column;
            background-color: #f7f7f7;
        }

        .image-preview {
            width: auto;
            max-width: 100%;
            max-height: 100%;
            height: auto;
            object-fit: cover;
        }

        .image-wrapper {
            height: 80%;
            display: flex;
            align-items: center;
        }

        .remove-button {
            position: absolute;
            top: -20px;
            right: -17px;
            background: none;
            border: none;
            color: #fff;
            font-size: 20px;
            cursor: pointer;
            z-index: 10;
        }

            .remove-button:hover {
                color: #ff0000;
            }

        .upload-button {
            background-color: #00bfa5;
            color: white;
            padding: 5px 10px;
            border: 0px;
            border-radius: 5px;
            cursor: pointer;
            text-align: center;
            display: inline-block;
            white-space: nowrap;
            margin-top: 10px;
        }

        .containertitle {
            height: 68px;
            background: #F5F5F5 0% 0% no-repeat padding-box;
            border: 1px solid #B9B9B9;
            border-radius: 10px;
            text-align: center;
            color: #000470;
            display: flex;
            align-self: center;
            font-size: 20px;
            font: normal normal bold 20px/24px Microsoft JhengHei;
            opacity: 1;
        }

        .write_Box hr {
            margin-top: 0.8rem;
            margin-bottom: 0.8rem;
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

        @media (max-width: 1860px) {
            .louis_col_1_3 {
                flex: 0 0 33.333% !important;
                max-width: 33.333% !important;
            }

            .louis_col_2_3 {
                flex: 0 0 66.666% !important;
                max-width: 66.666% !important;
                margin-left: 0 !important;
            }
        }

        @media (max-width: 1440px) and (min-width: 768px) {
            #flor {
                display: flex;
                flex-direction: column;
            }

            .form-control {
                height: auto !important;
            }

            #receive {
                height: 97% !important;
            }
        }

        @media (max-width: 768px) {
            .upload-container {
                max-width: 200px; /* 在小螢幕上，容器寬度調整為200px */
                height: 140px; /* 高度調整為140px */
            }

            .image-preview {
                max-width: 180px; /* 最大寬度180px */
                height: 80px; /* 固定高度80px */
            }
        }

        @media (max-width: 1024px) {
            .ItemContent1024 {
                display: flex;
                align-items: center;
            }

            .ItemContent1024Class {
                white-space: nowrap;
            }

            .ItemContent1024formItem {
                margin-right: 150px;
            }

            .ItemContent1024formItemLeft {
                padding-left: 119px;
            }
        }

        .write_TitleMark {
            display: flex !important;
            justify-content: space-between !important;
        }

        @media (min-width: 1200px) {
            #smallTextAuto {
                font-size: 14px;
            }
        }
    </style>
    <div class="write_Box">
        <div class="write_Title write_TitleMark">
            <h5>
                <asp:Literal runat="server" ID="box_title">修繕申請</asp:Literal></h5>
            <span class="subTitleMarkText">
                <span>註：1. 灰底為系統自棟帶入的資料</span>
                <span class="leftDistance">2. <span class="noticeText">*</span> 為必填</span>
            </span>
        </div>
        <div class="write_textBox">
            <div class="dataBox row">
                <div class="col-2 formItem title3">
                    <div class="ItemTitle"><span>申請日期</span></div>
                    <div class="ItemContent">
                        <asp:TextBox ID="apply_date" runat="server" TextMode="Date" CssClass="form-control" Style="width: 200px; height: 35px; margin-left: 0;" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="dataBox row">
                <div class="col-4 formItem title3">
                    <div class="ItemTitle"><span>申請組室</span></div>
                    <div class="ItemContent">
                        <asp:TextBox ID="apply_group" CssClass="form-control" runat="server" Style="height: 35px; margin-left: 0px;" Enabled="false"></asp:TextBox>
                    </div>
                </div>
                <div class="col-2 formItem title3">
                    <div class="ItemTitle"><span>申請人</span></div>
                    <div class="ItemContent">
                        <asp:TextBox ID="apply_user" CssClass="form-control" runat="server" Style="width: 138px; height: 35px; margin-left: 10px;" Enabled="false"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="write_textBox" style="display: flex">
            <div style="flex: 0 0 50%">

                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="dataBox row">
                            <div class="col-8 formItem title3">
                                <div class="ItemTitle"><span>修繕地點</span><span style="color: red">*</span></div>
                                <div class="ItemContent">
                                    <asp:RadioButton runat="server" Checked="true" ID="rb1" GroupName="prb" CssClass="col-2 pl-0" Style="margin-left: 10px; padding-right: 5px!important;" OnCheckedChanged="rb_CheckedChanged" AutoPostBack="true" /><span>行政大樓</span>
                                    <asp:RadioButton runat="server" ID="rb2" GroupName="prb" CssClass="col-2 pl-0" Style="margin-left: 15px; padding-right: 5px!important;" OnCheckedChanged="rb_CheckedChanged" AutoPostBack="true" /><span>工商大樓</span>
                                </div>

                            </div>
                        </div>
                        <div id="flor" class="dataBox row d-flex ">
                            <div class="col-5 formItem title3">
                                <div class="ItemTitle"><span>修繕樓層</span><span style="color: red">*</span></div>
                                <div class="ItemContent">
                                    <asp:DropDownList ID="floor" runat="server" CssClass="form-control" Style="width: 125px;" />
                                </div>
                            </div>
                            <div class="col-5 formItem title3" style="display: flex; flex-wrap: wrap; margin-left: 0px">
                                <div class="ItemTitle"><span>修繕位置</span></div>
                                <div class="ItemContent">
                                    <asp:DropDownList ID="location" runat="server" CssClass="form-control" Style="width: 150px;" />
                                </div>
                            </div>
                        </div>
                        <div class="dataBox row">
                            <div class="col-10 formItem title3">
                                <div class="ItemTitle"><span>修繕事由</span><span style="color: red">*</span></div>
                                <div class="ItemContent">
                                    <asp:TextBox ID="reason" CssClass="form-control" runat="server" TextMode="MultiLine" Columns="100" Rows="4" placeholder="請填寫具體的修繕位置、修繕物品及目前狀況(限30字)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div style="flex: 0 0 50%">
                <div>照片上傳<span style="font-size: 14px; color: #707070">(限傳2張照片，格式限制jpg、png，檔案大小限制10mb以下)</span></div>
                <div style="display: flex; gap: 20px;">
                    <div class="upload-container" id="pic1" runat="server">
                        <div class="image-wrapper">
                            <img id="preview1" src="image/image.png" alt="Image Preview" class="image-preview" runat="server" />
                            <button type="button" id="Delpreview1" class="remove-button" onclick="removeImage('1', 'FileUpload1')" style="display: none">
                                <img src="image/icon_close_red.png" alt="close" />
                            </button>
                        </div>
                        <asp:FileUpload ID="FileUpload1" runat="server" OnChange="showPreview(this, '1')" Style="display: none;" />
                        <div id="bt1" style="height: 20%; display: flex; align-items: center;">
                            <button type="button" id="p1" runat="server" class="upload-button">選擇檔案</button>
                        </div>
                    </div>
                    <div class="upload-container" id="pic2" runat="server">
                        <div class="image-wrapper">
                            <img id="preview2" src="image/image.png" alt="Image Preview" class="image-preview" runat="server" />
                            <button type="button" id="Delpreview2" class="remove-button" onclick="removeImage('2', 'FileUpload2')" style="display: none">
                                <img src="image/icon_close_red.png" alt="close" />
                            </button>
                        </div>
                        <asp:FileUpload ID="FileUpload2" runat="server" OnChange="showPreview(this, '2')" Style="display: none;" />
                        <div id="bt2" style="height: 20%; display: flex; align-items: center;">
                            <button type="button" id="p2" runat="server" class="upload-button">選擇檔案</button>
                        </div>
                    </div>
                    <span id="Message" style="margin: 0; padding: 10px; width: 100%; display: block; font-size: 14px; background-color: #e7e7e7; color: #414141; text-align: center; border: 1px solid #cccccc" runat="server" visible="false">無上傳照片</span>
                </div>
            </div>
        </div>
    </div>
    <div class="d-flex  flex-column align-items-center" style="margin-bottom: 50px; display: none">
    </div>
    <asp:Panel ID="pl" runat="server" Visible="false">
        <div class="write_Box">
            <div class="write_Title">
                <h5>
                    <asp:Literal runat="server" ID="Literal1">完成資訊</asp:Literal></h5>

                <div style="float: right; color: #000">
                    <asp:Label ID="treattime" runat="server" Text="處理日期：" Visible="false" Style="font: normal normal normal 18px Microsoft JhengHei;"></asp:Label>
                </div>
            </div>
            <div class="write_textBox">
                <div class="dataBox row">
                    <div class="col-4 formItem title6">
                        <div class="ItemTitle"><span>完成者/承辦人</span></div>
                        <div class="ItemContent">
                            <asp:TextBox ID="Finish_user" CssClass="form-control" runat="server" Style="width: 100%; height: 35px; margin-left: 10px;" Enabled="false"></asp:TextBox>
                        </div>
                    </div>

                </div>
                <div class="dataBox row">
                    <div class="col-3 louis_col_1_3 formItem title6 ItemContent1024formItem">
                        <div class="ItemTitle"><span>修繕處理</span></div>
                        <div class="ItemContent ItemContent1024">
                            <span class="ItemContent1024Class" style="margin-left: 10px;">日期</span><asp:TextBox ID="Finish_date" runat="server" TextMode="Date" CssClass="form-control" Style="width: 150px; height: 35px; margin-left: 10px;"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-7 louis_col_2_3 formItem title6 ItemContent1024formItemLeft" <%--style="padding-left: 10.5rem;"--%>>
                        <div class="ItemTitle" style="font-weight: normal;"><span>說明</span></div>
                        <div class="ItemContent" style="padding-left: 2.5em;">
                            <asp:TextBox ID="repair_note" CssClass="form-control" runat="server" Style="height: 50px; margin-left: 10px;" TextMode="MultiLine" Columns="100" placeholder="(限80字)"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <hr>
                <div class="dataBox row" style="margin-top: 0px;">
                    <div class="col-12 formItem title6" style="padding-right: 0px; align-items: baseline;">
                        <div id="receive">
                            <span>
                                <span class="ItemTitle">領用物品</span>
                                <br />
                                <span id="smallTextAuto" style="color: #707070">
                                    <span style="white-space: nowrap;">(有領物料時，</span>
                                    <br />
                                    <span style="position: relative; top: -4px;">請填寫)</span>
                                </span>
                            </span>

                        </div>
                        <div class="ItemContent">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="add_pl" runat="server" class="d-flex flex-column" Style="margin-left: 8px;">
                                    </asp:Panel>
                                    <asp:HiddenField ID="hf_add" runat="server" Value="0" />
                                    <asp:Button ID="add" runat="server" Text="新增一筆" CssClass="addbtn" OnClick="add_Click" UseSubmitBehavior="False" />
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                </div>
            </div>
        </div>

    </asp:Panel>
    <div class="d-flex  justify-content-center">

        <asp:Button ID="Cancel" class="bt" runat="server" Text="取消" Style="background: #777777" OnClick="Cancel_Click" />
        <asp:Button ID="statechange" runat="server" Text="處理" class="bt" Style="width: 92px; height: 44px; background: #FF9900 0% 0% no-repeat padding-box; border: 0px; color: #FFF; opacity: 1; border-radius: 5px;" Visible="false" OnClick="statechange_Click" />

        <asp:Button ID="Submit" class="bt" runat="server" Text="提交" OnClientClick="showModal1(); return false;" />
        <asp:Button ID="TempSave" class="bt" runat="server" Text="暫存" Style="background: #AB5C00" OnClick="TempSave_Click" Visible="false" />
        <asp:Button ID="save" class="bt" runat="server" Text="提交" OnClientClick="showModal2(); return false;" Visible="false" />
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
                        <div class="containertitle justify-content-center align-items-center">
                            您的修繕單編號為 &nbsp
                            <asp:Label ID="repairno" runat="server" Style="font-size: 24px;"></asp:Label>
                        </div>

                        <div id="content" role="form" class="form-horizontal">

                            <div class="Box" style="padding: 30px 60px;">
                                <div style="font-size: 1.25rem; font-weight: bold; text-align: center">請確認以下資料是否正確</div>
                                <div class="timeline">
                                    <div class="item">
                                        <div class="marker"></div>
                                        <div class="text">
                                            <h5>申請人資訊</h5>
                                            <div class="d-flex justify-content-start flex-wrap row" style="padding: 15px 0">
                                                <div class="col-lg-6 col-md-12 col-sm-12">申請組室：<span id="mdgroup" runat="server"></span></div>
                                                <div class="col-lg-6 col-md-12 col-sm-12">申請人：<span id="mduser" runat="server"></span></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="item">
                                        <div class="marker"></div>
                                        <div class="text">
                                            <h5>修繕地點資訊</h5>
                                            <div class="d-flex  justify-content-start flex-wrap row" style="padding: 15px 0">
                                                <div class="col-lg-6 col-md-12 col-sm-12">修繕地點：<span id="mdplace" runat="server"></span></div>
                                                <div class="col-lg-6 col-md-12 col-sm-12">修繕樓層：<span id="mdfloor" runat="server"></span></div>
                                            </div>
                                            <div class="d-flex  justify-content-start flex-wrap row" style="padding: 15px 0;">
                                                <div class="col-lg-12 col-md-12 col-sm-12">修繕位置：<span id="mdlocation" runat="server"></span></div>
                                            </div>
                                            <div class="d-flex  justify-content-start flex-wrap row" style="padding: 15px 0;">
                                                <div class="col-lg-6 col-md-12 col-sm-12">修繕事由：<span id="mdreason" runat="server"></span></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="item">
                                        <div class="marker"></div>
                                        <div class="text">
                                            <h5>照片上傳</h5>
                                            <div class="d-flex justify-content-start row" style="padding: 15px; gap: 50px;">
                                                <img id="modalPreview1" src="image/image.png" alt="Image Preview" class="image-preview" style="width: 150px; height: 150px;" />
                                                <%--<span id="mdp1" style="align-content: center;">無上傳照片</span>--%>
                                                <img id="modalPreview2" src="image/image.png" alt="Image Preview" class="image-preview" style="width: 150px; height: 150px;" />
                                                <span id="modalMessage" style="margin: 0;">無上傳照片</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                        </div>

                        <hr>
                        <div style="text-align: center">
                            <asp:Button ID="md1cancel" runat="server" Text="取消" class="bt" Style="background: #777777"
                                OnClientClick="return hideModal1();" />
                            <asp:Button ID="md1submit" runat="server" Text="提交" class="bt"
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
                <div class="modal-header" style="background-color: #C5ECFF; justify-content: center; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span id="Span1" runat="server" style="color: #003168">提交確認</span></b>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content2" role="form" class="form-horizontal text-center" style="margin-top: 50px; margin-bottom: 50px;">
                            <b style="font-size: 1.125rem; font-weight: bold;">確定要提交嗎?</b>
                            <br>
                            <b style="font-size: 1.125rem; font-weight: bold; color: #B83F1F">提交後無法修改及刪除</b>
                            <br>
                            <b style="font-size: 1rem; font-weight: normal!important;">作業處理可能需要一些時間，敬請稍候，感謝您的耐心等候與體諒！</b>
                        </div>
                        <hr>
                        <div style="text-align: center">
                            <asp:Button ID="md2cancel" runat="server" Text="取消" class="bt" Style="background: #777777"
                                OnClientClick="return hideModal2();" />
                            <asp:Button ID="md2submit" runat="server" Text="提交" class="bt" OnClick="md2submit_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

