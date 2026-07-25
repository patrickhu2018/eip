<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage3.master" AutoEventWireup="true" CodeFile="Meetingroom_calendar.aspx.cs" Inherits="Meetingroom_calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .modal-backdrop {
            display: none;
        }

        .bt {
            width: 100px;
            height: 40px;
            background: #4CAF1E 0% 0% no-repeat padding-box;
            border: 0px;
            border-radius: 5px;
            opacity: 1;
            text-align: center;
            color: #FFFFFF;
            margin-right: 10px;
        }

        .todaybt {
            padding: 3px 15px;
            background: #FFFFFF 0% 0% no-repeat padding-box;
            border: 1px solid #8F8F8F;
            border-radius: 5px;
            opacity: 1;
        }

        .addbtn {
            padding: 15px 30px 15px 50px;
            font-size: 1.1rem;
            font-weight: bold;
            text-align: center;
            background-image: url(image/icon_20.png);
            background-repeat: no-repeat;
            background-position-y: 50%;
            background-position-x: 6%;
            box-shadow: 0px 3px 6px #00000029;
            border: 2px solid #DBCEAE;
            border-radius: 49px;
            color: #643C19;
            opacity: 1;
            background-color: transparent;
        }

        .hourtd {
            position: absolute;
            top: -10px;
            left: 0;
        }

        .panel_cd {
            /*flex: 0 0 76%;*/
        }

        .left_1024
        {
            margin-right: 60px;
        }

        @media (min-width: 1440px) and (max-width: 1920px) {
            .panel_cd {
                /*flex: 0 0 75%;*/ /* 當螢幕寬度大於 768px 時，設置為 78% */
            }

            
        }

        @media (min-width: 1260px) and (max-width: 1439px) {
            .panel_cd {
                flex: 0 0 75%; /* 當螢幕寬度大於 768px 時，設置為 78% */
            }

            .hourtd {
                left: -20%;
            }

            .left_1024
            {
                margin-right: 30px !important;
            }
        }

        @media (min-width: 1100px) and (max-width: 1259px) {
            .panel_cd {
                flex: 0 0 75%; /* 當螢幕寬度大於 768px 時，設置為 78% */
            }

            .hourtd {
                left: -20%;
            }

            .left_1024
            {
                margin-right: 30px !important;
            }
        }

        @media (min-width: 768px) and (max-width: 1099px) {
            .panel_cd {
                flex: 0 0 70%; /* 當螢幕寬度大於 768px 時，設置為 78% */
            }

            .hourtd {
                left: -30%;
            }

            .left_1024
            {
                margin-right: 30px !important;
            }
        }



        .calendar {
            width: 100%;
            margin: 0 auto;
            border-collapse: collapse;
            margin-bottom: 30px;
            table-layout: fixed; /* 設置固定的表格布局 */
        }

            .calendar th {
                background-color: #FFFFFF;
                padding-top: 5px;
                text-align: center;
                border: 1px solid #ddd;
                border-bottom: 0px solid #ddd;
            }

            .calendar td {
                width: 14%;
                max-width: 150px;
                height: 100px;
                text-align: center;
                vertical-align: middle;
                border: 1px solid #ddd;
                border-top: 0px solid #ddd;
                background-color: #FFFFFF;
                box-sizing: border-box; /* 包括邊框和內邊距在內的寬度計算 */
            }

        .meetname {
            width: 20px;
            height: 20px;
            border: 0px;
            border-radius: 8px;
            opacity: 1;
            margin-right: 2px;
        }

        .datebt {
            width: 100%;
            max-width: 100%;
            padding-top: 2px;
            padding-bottom: 2px;
            font-size: 0.8rem;
            background-color: #524BB2;
            color: #FFFFFF;
            border-radius: 5px;
            text-align: left;
            display: block;
            border: 1px solid #FFFFFF;
            margin-bottom: 3px;
            cursor: pointer;
            box-sizing: border-box;
            white-space: nowrap; /* 不換行 */
            overflow: hidden !important; /* 隱藏超出範圍的部分 */
            text-overflow: ellipsis; /* 顯示省略號 */
            transition: z-index 0.2s, box-shadow 0.2s ease, background-color 0.3s ease;
        }

            .datebt:hover {
                z-index: 999 !important;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* 增加阴影效果 */
                box-shadow: 0 10px 15px rgba(0, 0, 0, 0.3); /* 添加阴影，使其看起来浮动 */
                transition: z-index 0.2s, box-shadow 0.3s ease, background-color 0.3s ease;
            }

        .week td {
            width: 14%;
            height: 50px;
            text-align: center;
            vertical-align: middle;
            border: 1px solid #ddd;
            border-top: 0px solid #ddd;
            /*overflow-x:  hidden;
            overflow-y: visible;*/ /* 允許超出 */
        }

        .day td {
            height: 50px;
            text-align: center;
            vertical-align: middle;
            border: 1px solid #ddd;
            border-top: 0px solid #ddd;
        }

        .calendar td:hover {
            background-color: #f0f0f0;
            /* cursor: pointer;*/
        }

        .calendar td.empty {
            pointer-events: none;
        }

        .month-nav {
            text-align: center;
            margin: 7px 0;
        }

            .month-nav a {
                display: inline-flex; /* 使用 flexbox 讓內容居中 */
                justify-content: center; /* 水平居中箭頭 */
                align-items: center; /* 垂直居中箭頭 */
                width: 30px; /* 設置圓形的寬度 */
                height: 30px; /* 設置圓形的高度 */
                border: 2px solid #ccc; /* 灰色邊框 */
                border-radius: 50%; /* 邊框圓角設為 50% 形成圓形 */
                font-size: 1rem; /* 設置箭頭字型大小 */
                color: #707070; /* 設置箭頭顏色 */
                background-color: #D9D9D9A6;
                text-decoration: none; /* 移除下劃線 */
                transition: background-color 0.3s ease, color 0.3s ease; /* 添加懸停效果過渡 */
            }

        .modal .form-control {
            width: 100%;
        }

        .write_textBox {
            border-top: 0px;
            padding: 0;
        }

        .write_Box hr {
            margin-top: 1rem;
            margin-bottom: 1rem;
        }

        .favoritebt {
            width: 100%;
            text-align: left;
            border: 0;
            background-color: #EFEFEC;
            border-radius: 3px;
            font-size: 1.1rem;
            font-weight: bold;
            margin-bottom: 15px;
            padding: 15px 20px;
        }

        input[readonly] {
            background-color: white !important; /* 讓背景保持白色 */
            color: black !important; /* 確保文字顏色是黑色 */
        }

        .ItemContent label {
            margin-bottom: 0;
            margin-right: 20px;
        }

        .ItemContent input[type=checkbox] {
            margin-right: 5px;
        }

        /* 日期時間區域設置 */
        .date-time-wrapper {
            display: flex;
            width: 45%; /* 可以調整寬度比例 */
            max-width: 45%;
        }
        /* 時間下拉選單的樣式 */
        .time-dropdown {
            display: none;
            position: absolute;
            z-index: 9999;
            width: 20%;
        }

            .time-dropdown select {
                width: 100%;
                height: auto;
                font-size: 14px;
                padding: 5px;
            }

        @media (max-width: 1680px) {
            .modal-lg {
                max-width: 70% !important; /* 屏幕較小時，讓元素垂直排列 */
            }
        }

        #floating-icon {
            position: fixed;
            bottom: 20px;
            right: 20px;
            cursor: pointer;
            z-index: 1000;
        }



        .gray_box {
            width: 80%;
            border-radius: 3px;
            background-color: #EFEFEC;
            border: 0;
            color: #000000;
            padding: 5px 15px;
            height: 50px;
            display: flex;
            align-items: center;
            font-size: 1rem;
            font-weight: bold;
            text-align: left;
            align-items: center;
        }

        @keyframes floatUp {
            0% {
                opacity: 0;
                transform: translateY(10px);
            }

            100% {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .more-meetings {
            display: none;
            position: absolute;
            background-color: white;
            border: 1px solid #ccc;
            padding: 10px;
            z-index: 100;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.1);
            max-width: 150px;
        }

        /* 當滑鼠懸停在包含更多會議的按鈕上時顯示浮窗 */
        td:hover .more-meetings {
            display: block;
            animation: floatUp 0.3s ease-out;
        }
    </style>
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
        function showModal3() {
            $('#modal3').modal('show');

            return false;
        }
        function hideModal3() {
            $('#modal3').modal('hide');

            return false;
        }
        // 在日曆格上添加點擊事件
        function addClickEventToCalendar() {
            const urlParams = new URLSearchParams(window.location.search);
            var cells = document.querySelectorAll('.calendar td');
            cells.forEach(function (cell) {
                cell.addEventListener('click', function () {
                    var u = '<%= Session["user_name"] %>';
                    var g = '<%= Session["group_name"] %>';
                    var m_no = document.getElementById('<%= m_no.ClientID %>');

                    var start_date = document.getElementById('<%= startdate.ClientID %>');
                    var start_hour = document.getElementById('<%= starthour.ClientID %>');
                    var end_date = document.getElementById('<%= enddate.ClientID %>');
                    var end_hour = document.getElementById('<%= endhour.ClientID %>');
                    var appr_group = document.getElementById('<%= appr_group.ClientID %>');
                    var appr_user = document.getElementById('<%= appr_user.ClientID %>');
                    var mtroom = document.getElementById('<%= mtroom.ClientID %>');
                    var host = document.getElementById('<%= host.ClientID %>');
                    var number = document.getElementById('<%= number.ClientID %>');
                    var meetclass = document.getElementById('<%= meetclass.ClientID %>');
                    var useclass = document.getElementById('<%= useclass.ClientID %>');
                    var note = document.getElementById('<%= note.ClientID %>');
                    var submitbt = document.getElementById('<%= submitbt.ClientID %>');
                    var delbt = document.getElementById('<%= del.ClientID %>');
                    var modifybt = document.getElementById('<%= modify.ClientID %>');
                    var mttitle = document.getElementById('<%= modtitle.ClientID %>');
                    var room_equipment = document.getElementById('<%= room_equipment.ClientID %>');
                    var room_number = document.getElementById('<%= room_number.ClientID %>');
                    var ck = document.getElementById('<%= ck_yes.ClientID %>');
                    var lunch_box = document.getElementById('<%= lunch_box.ClientID %>');
                    var Takeaway = document.getElementById('<%= Takeaway.ClientID %>');
                    var disposable = document.getElementById('<%= disposable.ClientID %>');
                    var reason_ck1 = document.getElementById('<%= reason_ck1.ClientID %>');
                    var reason_ck2 = document.getElementById('<%= reason_ck2.ClientID %>');
                    var reason_ck3 = document.getElementById('<%= reason_ck3.ClientID %>');
                    var other = document.getElementById('<%= other.ClientID %>');
                    var other_reason = document.getElementById('<%= other_reason.ClientID %>');
                    var other_reason_txt = document.getElementById('<%= other_reason_txt.ClientID %>');
                    room_equipment.textContent = ""
                    room_number.textContent = "";
                    mttitle.textContent = '建議會議室申請';
                    m_no.disabled = false;
                    mtroom.disabled = false;
                    host.disabled = false;
                    number.disabled = false;
                    meetclass.disabled = false;
                    useclass.disabled = false;
                    note.disabled = false;
                    start_date.disabled = false;
                    start_hour.disabled = false;
                    end_date.disabled = false;
                    end_hour.disabled = false;
                    ck.disabled = false;
                    lunch_box.disabled = false;
                    Takeaway.disabled = false;
                    disposable.disabled = false;
                    reason_ck1.disabled = false;
                    reason_ck2.disabled = false;
                    reason_ck3.disabled = false;
                    other_reason.disabled = false;
                    other_reason_txt.disabled = true;
                    if (useclass.value == "5") {
                        other.disabled = false;
                    } else {
                        other.disabled = true;
                    }
                    reason_ck1.checked = false;
                    reason_ck2.checked = false;
                    reason_ck3.checked = false;
                    other_reason.checked = false;

                    m_no.value = "";
                    start_hour.value = "";
                    end_hour.value = "";
                    appr_group.value = g;
                    appr_user.value = u;
                    mtroom.value = "0";
                    host.value = "";
                    number.value = "";
                    meetclass.value = "0";
                    useclass.value = "0";
                    other.value = "";
                    note.value = "";
                    delbt.style.display = 'none';
                    submitbt.style.display = 'inline-block';
                    modifybt.style.display = 'none';
                    if (urlParams.get('mod') === 'm') {
                        var hidden = cell.querySelector('input[type="hidden"]').value;
                        start_date.value = hidden;
                        end_date.value = hidden;
                    }

                    showModal1();

                });
            });

        }
        window.addEventListener('load', addClickEventToCalendar);


        /////////////日歷行程點擊
        document.addEventListener('DOMContentLoaded', function () {
            document.querySelectorAll('.datebt').forEach(function (button) {
                button.addEventListener('click', function () {
                    var buttonId = button.id;
                    var buttonIdParts = buttonId.split('_');
                    var numberPart = buttonIdParts[1];
                    $.ajax({
                        url: 'WebService.asmx/getdate',
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({
                            id: numberPart
                        }),
                        dataType: 'json',
                        success: function (response) {
                            var result = JSON.parse(response.d);
                            var m_no = document.getElementById('<%= m_no.ClientID %>');
                            var hf_id = document.getElementById('<%= hf_id.ClientID %>');
                            var start_date = document.getElementById('<%= startdate.ClientID %>');
                            var start_hour = document.getElementById('<%= starthour.ClientID %>');
                            var end_date = document.getElementById('<%= enddate.ClientID %>');
                            var end_hour = document.getElementById('<%= endhour.ClientID %>');
                            var appr_group = document.getElementById('<%= appr_group.ClientID %>');
                            var appr_user = document.getElementById('<%= appr_user.ClientID %>');
                            var mtroom = document.getElementById('<%= mtroom.ClientID %>');
                            var host = document.getElementById('<%= host.ClientID %>');
                            var number = document.getElementById('<%= number.ClientID %>');
                            var room_equipment = document.getElementById('<%= room_equipment.ClientID %>');
                            var room_number = document.getElementById('<%= room_number.ClientID %>');
                            var meetclass = document.getElementById('<%= meetclass.ClientID %>');
                            var useclass = document.getElementById('<%= useclass.ClientID %>');
                            var note = document.getElementById('<%= note.ClientID %>');
                            var submitbt = document.getElementById('<%= submitbt.ClientID %>');
                            var delbt = document.getElementById('<%= del.ClientID %>');
                            var modifybt = document.getElementById('<%= modify.ClientID %>');
                            var del_id = document.getElementById('<%= del_id.ClientID %>');
                            var mttitle = document.getElementById('<%= modtitle.ClientID %>');
                            var meet = document.getElementById('<%= mnbt.ClientID %>');
                            var hostbt = document.getElementById('<%= hostbt.ClientID %>');
                            var ck = document.getElementById('<%= ck_yes.ClientID %>');
                            var lunch_box = document.getElementById('<%= lunch_box.ClientID %>');
                            var Takeaway = document.getElementById('<%= Takeaway.ClientID %>');
                            var disposable = document.getElementById('<%= disposable.ClientID %>');
                            var reason_ck1 = document.getElementById('<%= reason_ck1.ClientID %>');
                            var reason_ck2 = document.getElementById('<%= reason_ck2.ClientID %>');
                            var reason_ck3 = document.getElementById('<%= reason_ck3.ClientID %>');
                            var other = document.getElementById('<%= other.ClientID %>');
                            var other_reason = document.getElementById('<%= other_reason.ClientID %>');
                            var other_reason_txt = document.getElementById('<%= other_reason_txt.ClientID %>');

                            hf_id.value = result.mid
                            mttitle.textContent = '會議室詳情';
                            del_id.value = numberPart;
                            m_no.value = result.meeting_name;
                            appr_group.value = result.appr_group;
                            appr_user.value = result.appr_user;
                            mtroom.value = result.appr_meet_id;
                            host.value = result.host;
                            number.value = result.number;
                            room_equipment.textContent = result.equipment;
                            room_number.textContent = "(可容納" + result.count + "人)";
                            meetclass.value = result.meetclass;
                            useclass.value = result.useclass;
                            if (result.useclass == "5") {
                                other.disabled = false
                            } else {
                                other.disabled = true
                            }
                            other.value = result.other;
                            if (result.note === "") {
                                note.value = "無備註";
                            } else {
                                note.value = result.note;
                            }
                            if (result.ck === "1") {
                                ck.checked = true;
                            } else {
                                ck.checked = false;  // 取消勾選
                            }
                            lunch_box.value = result.lunch_box === null ? "" : result.lunch_box;
                            Takeaway.value = result.Takeaway === null ? "" : result.Takeaway;
                            disposable.value = result.disposable === null ? "" : result.disposable;
                            var reasons = [];
                            if (result.other_reason) {
                                reasons = result.other_reason.split(",").map(function (item) {
                                    return item.trim(); // 去除每個選項的前後空格
                                });
                            }
                            reason_ck1.checked = false;
                            reason_ck2.checked = false;
                            reason_ck3.checked = false;
                            other_reason.checked = false;
                            var otherOptions = [];
                            other_reason_txt.value = "";
                            reasons.forEach(function (item) {
                                if (item === "訂購數量") {
                                    reason_ck1.checked = true; // 勾選第一個選項
                                } else if (item === "收送時間") {
                                    reason_ck2.checked = true; // 勾選第二個選項
                                } else if (item === "辦理場地") {
                                    reason_ck3.checked = true; // 勾選第三個選項
                                } else {
                                    // 其他選項（不屬於預設的三個）
                                    otherOptions.push(item); // 將其他選項加入

                                }
                            });
                            if (otherOptions.length > 0) {
                                other_reason_txt.value = otherOptions[otherOptions.length - 1]; // 將最後一個選項顯示在輸入框中
                                other_reason_txt.disabled = false; // 啟用輸入框（如果它是禁用的）
                                other_reason.checked = true;
                            } else {
                                other_reason_txt.disabled = true; // 如果沒有其他選項，禁用輸入框
                                other_reason.checked = false;
                            }

                            delbt.style.display = 'inline-block';
                            submitbt.style.display = 'none';
                            modifybt.style.display = 'inline-block';
                            /////////開始時間
                            var startparts = result.use_start.split(' ');
                            var startdate = startparts[0];
                            var starttime = startparts[1] + ' ' + startparts[2];
                            var startdateParts = startdate.split('/');
                            var startformattedDate = startdateParts[0] + '-' +
                                (startdateParts[1].padStart(2, '0')) + '-' +
                                (startdateParts[2].padStart(2, '0'));
                            var starthour = parseInt(starttime.split(' ')[1].split(':')[0]);
                            var startminute = starttime.split(' ')[1].split(':')[1];
                            var startperiod = starttime.split(' ')[0];
                            if (startperiod === '下午' && starthour !== 12) {
                                starthour += 12;
                            } else if (startperiod === '上午' && starthour === 12) {
                                starthour = 0;
                            }
                            var startformattedTime = (starthour).toString().padStart(2, '0') + ':' + (startminute).padStart(2, '0');
                            start_date.value = startformattedDate;
                            start_hour.value = startformattedTime;
                            ////////結束時間
                            var endparts = result.use_end.split(' ');
                            var enddate = endparts[0];
                            var endtime = endparts[1] + ' ' + endparts[2];
                            var enddateParts = enddate.split('/');
                            var endformattedDate = enddateParts[0] + '-' +
                                (enddateParts[1].padStart(2, '0')) + '-' +
                                (enddateParts[2].padStart(2, '0'));
                            var endhour = parseInt(endtime.split(' ')[1].split(':')[0]);
                            var endminute = endtime.split(' ')[1].split(':')[1];
                            var endperiod = endtime.split(' ')[0];
                            if (endperiod === '下午' && endhour !== 12) {
                                endhour += 12;
                            } else if (endperiod === '上午' && endhour === 12) {
                                endhour = 0;
                            }
                            var endformattedTime = (endhour).toString().padStart(2, '0') + ':' + (endminute).padStart(2, '0');
                            end_date.value = endformattedDate;
                            end_hour.value = endformattedTime;
                            var timeSelectStart = document.getElementById('<%= timeSelectStart.ClientID %>');
                            var timeSelectEnd = document.getElementById('<%= timeSelectEnd.ClientID %>');
                            timeSelectStart.value = startformattedTime;
                            timeSelectEnd.value = endformattedTime;
                            var uid = '<%= Session["user_right_id"] %>';
                            var name = '<%= Session["user_name"] %>';
                            if (uid != "1" && uid != "3" && appr_user.value != name) {
                                delbt.style.display = 'none';
                                modifybt.style.display = 'none';
                                ////禁用
                                if (name != appr_user) {
                                    m_no.disabled = true;
                                    mtroom.disabled = true;
                                    host.disabled = true;
                                    number.disabled = true;
                                    meetclass.disabled = true;
                                    useclass.disabled = true;
                                    note.disabled = true;
                                    start_date.disabled = true;
                                    start_hour.disabled = true;
                                    start_hour.style.setProperty('background-color', '#ECECEC', 'important');
                                    end_hour.style.setProperty('background-color', '#ECECEC', 'important');
                                    end_date.disabled = true;
                                    end_hour.disabled = true;
                                    ck.disabled = true;
                                    lunch_box.disabled = true;
                                    Takeaway.disabled = true;
                                    disposable.disabled = true;
                                    reason_ck1.disabled = true;
                                    reason_ck2.disabled = true;
                                    reason_ck3.disabled = true;
                                    other.disabled = true;
                                    other_reason.disabled = true;
                                    other_reason_txt.disabled = true;
                                    meet.style.display = 'none'
                                    hostbt.style.display = 'none'

                                } else {

                                }
                            } else {
                                start_hour.style.setProperty('background-color', '#FFFFFF', 'important');
                                end_hour.style.setProperty('background-color', '#FFFFFF', 'important');

                                meet.style.display = 'block'
                                hostbt.style.display = 'block'
                            }
                            showModal1();
                        },
                        error: function (xhr, status, error) {
                            console.error('Error fetching data:', status, error);
                        }
                    });
                });
            });
        });
 </script>
    <script>

        // 點擊開始時間 TextBox 時顯示下拉選單
        function toggleTimeDropdown() {
            let dropdown = document.getElementById('timeDropdownStart');
            let textBox = document.getElementById('<%= starthour.ClientID %>');

            // 如果下拉選單已顯示，則隱藏它，否則顯示
            if (dropdown.style.display === 'block') {
                dropdown.style.display = 'none';
            } else {
                dropdown.style.display = 'block';

                // 確保下拉選單的位置與 TextBox 一致
                dropdown.style.top = textBox.offsetTop + textBox.offsetHeight + 'px';
                dropdown.style.left = textBox.offsetLeft + 'px';
            }
        }

        // 點擊結束時間 TextBox 時顯示下拉選單
        function toggleTimeDropdown2() {
            let dropdown = document.getElementById('timeDropdownEnd');
            let textBox = document.getElementById('<%= endhour.ClientID %>');

            // 如果下拉選單已顯示，則隱藏它，否則顯示
            if (dropdown.style.display === 'block') {
                dropdown.style.display = 'none';
            } else {
                dropdown.style.display = 'block';

                // 確保下拉選單的位置與 TextBox 一致
                dropdown.style.top = textBox.offsetTop + textBox.offsetHeight + 'px';
                dropdown.style.left = textBox.offsetLeft + 'px';
            }
        }

        // 當用戶選擇一個時間選項時，將選中的時間填入開始時間 TextBox
        function setTimeFromDropdown() {
            let select = document.getElementById('<%= timeSelectStart.ClientID %>');
            let selectedTime = select.value;
            let textBox = document.getElementById('<%= starthour.ClientID %>');
            // 將選擇的時間填入 TextBox
            textBox.value = selectedTime;

            // 隱藏下拉選單
            document.getElementById('timeDropdownStart').style.display = 'none';
        }

        // 當用戶選擇一個時間選項時，將選中的時間填入結束時間 TextBox
        function setTimeFromDropdown2() {
            let select = document.getElementById('<%= timeSelectEnd.ClientID %>');
            let selectedTime = select.value;
            let textBox = document.getElementById('<%= endhour.ClientID %>');

            // 將選擇的時間填入 TextBox
            textBox.value = selectedTime;

            // 隱藏下拉選單
            document.getElementById('timeDropdownEnd').style.display = 'none';
        }

        // 點擊下拉選單外部區域時，隱藏下拉選單
        document.addEventListener('click', function (event) {
            let dropdownStart = document.getElementById('timeDropdownStart');
            let dropdownEnd = document.getElementById('timeDropdownEnd');
            let textBoxStart = document.getElementById('<%= starthour.ClientID %>');
            let textBoxEnd = document.getElementById('<%= endhour.ClientID %>');

            // 如果點擊的區域不在 TextBox 或下拉選單內，則隱藏下拉選單
            if (!textBoxStart.contains(event.target) && !dropdownStart.contains(event.target)) {
                dropdownStart.style.display = 'none';
            }
            if (!textBoxEnd.contains(event.target) && !dropdownEnd.contains(event.target)) {
                dropdownEnd.style.display = 'none';
            }
        });

        function fetchMeetingNames() {
            $.ajax({
                type: 'POST',
                url: 'Meetingroom_calendar.aspx/GetMeetingNames',
                data: '{}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var favoriteNames = response.d;
                    var namesArray = favoriteNames.split(',');
                    namesArray = namesArray.slice(0, 3);

                    var container = document.getElementById('favorite');
                    container.innerHTML = '';
                    var i = 0
                    namesArray.forEach(function (name) {
                        i++
                        var button = document.createElement('button');
                        button.innerHTML = i + ".　" + name.trim();
                        button.className = 'favoritebt';
                        button.onclick = function (event) {
                            setmeetValue(name.trim());
                            event.preventDefault();
                        };
                        container.appendChild(button);
                    });
                },
                error: function (xhr, status, error) {
                }
            });
            var mdtitle = document.getElementById('<%=md3span.ClientID%>');
            mdtitle.innerText = '常用會議名稱'

            showModal3();
            return false;
        }
        function fetchHostNames() {
            $.ajax({
                type: 'POST',
                url: 'Meetingroom_calendar.aspx/GetHostNames',
                data: '{}',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var favoriteNames = response.d;
                    var namesArray = favoriteNames.split(',');
                    namesArray = namesArray.slice(0, 3);

                    var container = document.getElementById('favorite');
                    container.innerHTML = '';
                    var i = 0
                    namesArray.forEach(function (name) {
                        i++
                        var button = document.createElement('button');
                        button.innerHTML = i + ".　" + name.trim();
                        button.className = 'favoritebt';
                        button.onclick = function (event) {
                            setHostValue(name.trim());
                            event.preventDefault();
                        };
                        container.appendChild(button);
                    });
                },
                error: function (xhr, status, error) {
                }
            });
            var mdtitle = document.getElementById('<%=md3span.ClientID%>');
            mdtitle.innerText = '常用主持人'

            showModal3();
            return false;
        }
        function setmeetValue(buttonText) {
            var textBox = document.getElementById('<%= m_no.ClientID %>');
            textBox.value = buttonText;
            hideModal3();
            return false;
        }
        function setHostValue(buttonText) {
            var textBox = document.getElementById('<%= host.ClientID %>');
            textBox.value = buttonText;
            hideModal3();
            return false;
        }
        document.addEventListener("DOMContentLoaded", function () {
            // 遍歷每個日曆單元格，顯示浮窗
            document.querySelectorAll("td").forEach(function (cell) {
                var moreMeetingsDiv = cell.querySelector(".more-meetings");

                if (moreMeetingsDiv) {
                    // 當滑鼠進入該單元格時顯示浮窗
                    cell.addEventListener("mouseover", function () {
                        moreMeetingsDiv.style.display = "block";
                    });

                    // 當滑鼠離開該單元格時隱藏浮窗
                    cell.addEventListener("mouseout", function () {
                        moreMeetingsDiv.style.display = "none";
                    });
                }
            });
        });


</script>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="write_Box">
        <div class="write_textBox d-flex" style="width: auto;padding-top:20px;">
            <div class="left_1024" style="flex: 0 0 18%; display: flex; flex-direction: column; align-items: center;">
                <div class="d-flex align-items-center  justify-content-left" style="margin: 20px 0;">
                    <asp:Button ID="addmeeting" runat="server" Text="建立會議室申請" CssClass="addbtn" OnClick="addmeeting_Click" UseSubmitBehavior="false" />
                </div>
                <asp:Calendar runat="server" ID="Calendar" BackColor="White" BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" DayNameFormat="Shortest" 
                    Font-Names="Verdana" Font-Size="8pt" ForeColor="#003399" Height="200px" Width="220px" Visible="false" OnSelectionChanged="Calendar_SelectionChanged">
                    <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                    <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                    <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                    <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                    <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                    <WeekendDayStyle BackColor="#CCCCFF" />
                </asp:Calendar>
                <div class="d-flex" style="flex-direction: column; margin-top: 50px; bottom: 30px; width: 83%">
                    <span style="color: #643C19; font-weight: bold;">呈現方式</span>
                    <asp:DropDownList ID="ddl1" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl1_SelectedIndexChanged">
                        <asp:ListItem Value="0">月</asp:ListItem>
                        <asp:ListItem Value="1">週</asp:ListItem>
                        <asp:ListItem Value="2">日</asp:ListItem>
                    </asp:DropDownList>
                    <span style="color: #643C19; font-weight: bold; margin-top: 20px;">申請人</span>
                    <asp:DropDownList ID="ddl2" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl2_SelectedIndexChanged">
                        <asp:ListItem Value="0">全部</asp:ListItem>
                        <asp:ListItem Value="1">我的申請</asp:ListItem>
                        <asp:ListItem Value="2">組室申請</asp:ListItem>
                    </asp:DropDownList>
                    <span style="color: #643C19; font-weight: bold; margin-top: 20px;">開會地點</span>
                    <asp:DropDownList ID="ddl3" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl3_SelectedIndexChanged"></asp:DropDownList>
                    <span style="color: #643C19; font-weight: bold; margin-top: 20px;">會議室顏色說明</span>
                    <div id="m_meet" class="d-flex align-items-center flex-wrap" runat="server"></div>
                </div>

            </div>

            <asp:Panel ID="panle_month" CssClass="panel_cd" runat="server">
                <div class="month-nav d-flex justify-content-between" style="gap: 25px;">
                    <div class=" d-flex align-items-center">
                        <asp:Button ID="today_month" CssClass="todaybt" runat="server" Text="今天" Style="margin-right: 10px;" OnClick="today_month_Click" />
                        <span style="font-size: 1.4rem; margin-right: 10px;"><%= CurrentMonth() %> </span>
                        <a style="margin-right: 10px;" href="<%= GetPreviousMonthUrl() %>" class="fa fa-chevron-left"></a>
                        <a href="<%= GetNextMonthUrl() %>" class="fa fa-chevron-right"></a>
                    </div>
                    <asp:Label runat="server" ID="Label3" class="note" style="margin-left:5.5rem;display: flex;align-items: center;">註：日曆檢視的日期可以帶到清單檢視的[使用日期]中</asp:Label>
                </div>
                <div id="calendar_month" runat="server" style="max-width: 100%">
                </div>
            </asp:Panel>
            <asp:Panel ID="panle_week" CssClass="panel_cd" style="padding-top:15px;" runat="server" Visible="false">
                <div class="month-nav d-flex justify-content-between" style="gap: 25px;">
                    <div class=" d-flex align-items-center">
                        <asp:Button ID="today_week" CssClass="todaybt" runat="server" Text="今天" Style="margin-right: 10px;" OnClick="today_week_Click" />
                        <span style="font-size: 1.4rem; margin-right: 10px;"><%= CurrentMonth() %> </span>
                        <a style="margin-right: 10px;" href="<%= PreviousWeek() %>" class="fa fa-chevron-left"></a>
                        <a href="<%= NextWeek() %>" class="fa fa-chevron-right"></a>
                    </div>
                    <asp:Label runat="server" ID="Label1" class="note" style="margin-left:5.5rem;display: flex;align-items: center;">註：日曆檢視的日期可以帶到清單檢視的[使用日期]中</asp:Label>
                    <%--<div class="d-flex align-items-center text-center">
                        <div style="width: 20px; height: 20px; background: #524BB2 0% 0% no-repeat padding-box; border: 1px solid #707070; border-radius: 5px; opacity: 1;">
                        </div>
                        <span>已申請會議室</span>
                    </div>--%>
                </div>
                <div id="calendar_week" style="position: relative;" runat="server">
                </div>
            </asp:Panel>
            <asp:Panel ID="panle_day" CssClass="panel_cd" runat="server" Visible="false">
                <div class="month-nav d-flex justify-content-between" style="gap: 25px;">
                    <div class=" d-flex align-items-center" style="justify-content:space-between">
                        <asp:Button ID="today" CssClass="todaybt" runat="server" Text="今天" Style="margin-right: 10px;" OnClick="today_Click" />
                        <span style="font-size: 1.4rem; margin-right: 10px;"><%= Currentday() %> </span>
                        <a style="margin-right: 10px;" href="<%= Previousday() %>" class="fa fa-chevron-left"></a>
                        <a href="<%= Nextday() %>" class="fa fa-chevron-right"></a>
                    </div>
                    <asp:Label runat="server" ID="Label2" class="note" style="margin-left:5.5rem;">註：日曆檢視的日期可以帶到清單檢視的[使用日期]中</asp:Label>
                    <%--<div class="d-flex align-items-center text-center">
                        <div style="width: 20px; height: 20px; background: #524BB2 0% 0% no-repeat padding-box; border: 1px solid #707070; border-radius: 5px; opacity: 1;">
                        </div>
                        <span>已申請會議室</span>
                    </div>--%>
                </div>
                <div id="calendar_day" style="position: relative;" runat="server">
                </div>
            </asp:Panel>


        </div>
        <%--        <div id="floating-icon">
            <img src="image/icon_25.png" alt="浮動圖示" />
        </div>
        <div id="popup" class="popup">
            <div class="popup-content">
                
            </div>
        </div>--%>
    </div>

    <%--    <script>
        const floatingIcon = document.getElementById('floating-icon');
        const popup = document.getElementById('popup');

        // 用來判斷圖片是否靠近右下角
        function checkPosition() {
            // 當滑鼠移到圖片上時顯示浮動視窗
            floatingIcon.addEventListener('mouseover', function () {
                popup.style.display = 'block';
                console.log('顯示');
            });

            // 當滑鼠離開圖片時隱藏浮動視窗
            floatingIcon.addEventListener('mouseout', function () {
                popup.style.display = 'none';
                console.log('隱藏');
            });
        }
        window.addEventListener('scroll', checkPosition());

        function checkPositiveNumber(input) {
            var value = input.value;
            if (value.trim() === "") {
                return; // 如果是空的，什麼都不做，防止誤刪空白時觸發警告
            }
            if (isNaN(value) || value <= 0) {
                input.value = "";
                alert("請輸入正數");
            }
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
    <div class="modal fade bs-NewUser-modal-lg" id="modal1" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);overflow-y: auto;">
        <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 55%">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                    <b><span id="modtitle" runat="server" style="color: #003168">建立會議室申請</span></b>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                        <image src="image/popup_close.png"></image>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="MainClass">
                        <div id="content" role="form" class="form-horizontal">
                            <div class="Box" style="padding: 0px 40px;">
                                <div class="row" style="margin-bottom: 10px;">
                                    <div class=" col-8 formItem title3">
                                        <div class="ItemTitle"><span>會議名稱<span style="color: #ff0000">*</span></span></div>
                                        <div class="ItemContent d-flex align-items-center">
                                            <asp:TextBox ID="m_no" runat="server" CssClass="form-control" Style="width: 85%; margin-right: 10px;" />
                                            <asp:HiddenField ID="hf_id" runat="server" />
                                            <asp:ImageButton ID="mnbt" ImageUrl="~/image/icon_24.png" ToolTip="常用會議名稱" runat="server" CommandArgument="name" OnClick="favorite_Click" />
                                            <%-- <asp:ImageButton ID="mnbt" ImageUrl="~/image/icon_24.png" ToolTip="常用會議名稱" runat="server" UseSubmitBehavior="false" OnClientClick="return fetchMeetingNames()" />--%>
                                            <%--<asp:Button ID="mnbt" runat="server" Text="常用會議名稱" CssClass="bt" Style="width: auto; height: auto; padding: 5px 10px; background-color: #1885C5" UseSubmitBehavior="false" OnClientClick="return fetchMeetingNames()" />--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="margin-bottom: 10px; width: 100%">
                                    <div class=" col-12 formItem title3">
                                        <div class="ItemTitle"><span>會議時間<span style="color: #ff0000">*</span></span></div>
                                        <div class="ItemContent " style="max-width: 100%;">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" class="d-flex justify-content-between align-items-center flex-wrap" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="date-time-wrapper">
                                                        <asp:TextBox ID="startdate" runat="server" CssClass="form-control" Style="width: 100%; margin-right: 5px;" TextMode="Date" />
                                                        <asp:TextBox ID="starthour" runat="server" CssClass="form-control" Style="width: 100%;" TextMode="Time" ReadOnly="true" onclick="toggleTimeDropdown()" />
                                                        <div id="timeDropdownStart" class="time-dropdown" style="display: none;">
                                                            <asp:DropDownList ID="timeSelectStart" runat="server" OnSelectedIndexChanged="timeSelectStart_SelectedIndexChanged" size="10" AutoPostBack="true" />
                                                        </div>
                                                    </div>
                                                    <span class="text-center">~</span>
                                                    <div class="date-time-wrapper">
                                                        <asp:TextBox ID="enddate" runat="server" CssClass="form-control" Style="width: 100%; margin-right: 5px;" TextMode="Date" />
                                                        <asp:TextBox ID="endhour" runat="server" CssClass="form-control" Style="width: 100%;" TextMode="Time" ReadOnly="true" onclick="toggleTimeDropdown2()" />
                                                        <div id="timeDropdownEnd" class="time-dropdown" style="display: none;">
                                                            <asp:DropDownList ID="timeSelectEnd" runat="server" OnSelectedIndexChanged="timeSelectEnd_SelectedIndexChanged" size="10" AutoPostBack="true" />
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="timeSelectStart" EventName="SelectedIndexChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="timeSelectEnd" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <hr>
                            </div>
                            <div class="Box" style="padding: 0px 40px;">
                                <div class="row" style="margin-bottom: 10px;">
                                    <div class=" col-5 formItem title3">
                                        <div class="ItemTitle"><span>申請組室<span style="color: #ff0000">*</span></span></div>
                                        <div class="ItemContent">
                                            <asp:TextBox ID="appr_group" CssClass="form-control" runat="server" Style="width: 100%;" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class=" col-5 formItem title3">
                                        <div class="ItemTitle"><span>申請人<span style="color: #ff0000">*</span></span></div>
                                        <div class="ItemContent">
                                            <asp:TextBox ID="appr_user" CssClass="form-control" runat="server" Style="width: 80%;" Enabled="false" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="margin-bottom: 10px;">
                                    <div class="col-8 formItem  title3">
                                        <div class="ItemTitle"><span>會議室<span style="color: #ff0000">*</span></span></div>
                                        <div class="ItemContent">
                                            <div class=" d-flex align-items-center" style="width: 100%;">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" style="width: 54%; display: flex; flex-direction: column">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="mtroom" runat="server" class="form-control" Style="width: 100%;" OnSelectedIndexChanged="mtroom_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <span style="font-size: 14px; margin-left: 5px; white-space: nowrap;">註：標註★表示是我的最愛會議室</span>

                                            </div>
                                        </div>
                                    </div>
                                    <div class=" col-4 formItem title3 ml-0">
                                        <div class="ItemTitle"><span>主持人<span style="color: #ff0000">*</span></span></div>
                                        <div class="ItemContent d-flex align-items-center">
                                            <asp:TextBox ID="host" class="form-control" runat="server" Style="width: 85%; margin-right: 15px;" />
                                            <asp:ImageButton ID="hostbt" ImageUrl="~/image/icon_24.png" ToolTip="常用主持人" runat="server" CommandArgument="host" OnClick="favorite_Click" />
                                            <%--   <asp:ImageButton ID="hostbt" ImageUrl="~/image/icon_24.png" ToolTip="常用主持人" runat="server" UseSubmitBehavior="false" OnClientClick="return fetchHostNames()" />--%>
                                            <%--  <asp:Button ID="hostbt" runat="server" Text="常用主持人" CssClass="bt" Style="width: auto; height: auto; padding: 5px 10px; background-color: #1885C5" UseSubmitBehavior="false" OnClientClick="return fetchHostNames()" />--%>
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel runat="server" ID="mrdevices" style="width:70%; padding-left:5.875rem; display:none;" ClientIDMode="Static">
                                                <asp:Label ID="room_equipment" runat="server" Text="" Style="text-align: left; letter-spacing: 0px; color: #375471; font-size: 14px; " />
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <hr>
                            </div>

                            <div class="Box" style="padding: 0px 40px;">
                                <asp:UpdatePanel runat="server">
                                <ContentTemplate>                              
                                <div class="row" style="margin-bottom: 10px;">
                                    <div class=" col-5 formItem title3">
                                        <div class="ItemTitle"><span>會議類型<span style="color: #ff0000">*</span></span></div>
                                        <div class="ItemContent d-flex align-items-center">
                                            <%--<asp:DropDownList ID="meetclass" runat="server" class="form-control" Style="width: 100%;">
                                                <asp:ListItem Value="0">請選擇</asp:ListItem>
                                                <asp:ListItem Value="1">研討會</asp:ListItem>
                                                <asp:ListItem Value="2">發表會</asp:ListItem>
                                                <asp:ListItem Value="3">股東大會</asp:ListItem>
                                                <asp:ListItem Value="4">訓練講習會</asp:ListItem>
                                                <asp:ListItem Value="5">聯誼活動</asp:ListItem>
                                                <asp:ListItem Value="6">記者會</asp:ListItem>
                                                <asp:ListItem Value="7">其他</asp:ListItem>
                                                <asp:ListItem Value="8">招(開)標</asp:ListItem>
                                                <asp:ListItem Value="9">電影欣賞</asp:ListItem>
                                                <asp:ListItem Value="10">例行性會議</asp:ListItem>
                                                <asp:ListItem Value="11">視訊會議</asp:ListItem>
                                            </asp:DropDownList>--%>
                                            <asp:DropDownList ID="meetclass" runat="server" CssClass="form-control" Style="width: 100%;"  DataSourceID="meetclass_sd"  DataTextField="meetingtype_name"  DataValueField="meeting_meetingtype_id"  AppendDataBoundItems="true"> 
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="meeting_type_bt" Visible="false" ImageUrl="~/image/icon_24.png" ToolTip="會議類型" runat="server" CommandArgument="meetingtype" OnClick="meeting_item_Click" />
                                            <asp:SqlDataSource runat="server" ID="meetclass_sd" ConnectionString="<%$ ConnectionStrings:eip %>" SelectCommand="SELECT 0 AS meeting_meetingtype_id, '請選擇' AS meetingtype_name UNION ALL select meeting_meetingtype_id,meetingtype_name from meeting_apply_meetingtype"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                    <div class=" col-5 formItem title3">
                                        <div class="ItemTitle"><span>電子會議</span></div>
                                        <div class="ItemContent">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:CheckBox ID="ck_yes" runat="server" Text="是" AutoPostBack="true" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                </ContentTemplate>
                                </asp:UpdatePanel>

                                <div class="">
                                    <asp:UpdatePanel ID="UpdatePanel5" class="row" runat="server" style="margin-bottom: 10px;">
                                        <ContentTemplate>
                                            <div class=" col-5 formItem title3">
                                                <div class="ItemTitle"><span>使用類型<span style="color: #ff0000">*</span></span></div>
                                                <div class="ItemContent d-flex align-items-center">
                                                    <asp:DropDownList ID="useclass" runat="server" CssClass="form-control" Style="width: 100%;" OnSelectedIndexChanged="useclass_SelectedIndexChanged" AutoPostBack="true" DataSourceID="useclass_sd"  DataTextField="usedtype_name"  DataValueField="meeting_usedtype_id"  AppendDataBoundItems="true">
                                                        <%--<asp:ListItem Value="0">請選擇</asp:ListItem>--%>
                                                       <%-- <asp:ListItem Value="1">自用</asp:ListItem>
                                                        <asp:ListItem Value="2">租用</asp:ListItem>
                                                        <asp:ListItem Value="3">共同辦理</asp:ListItem>
                                                        <asp:ListItem Value="4">協辦</asp:ListItem>
                                                        <asp:ListItem Value="5">其他</asp:ListItem>--%>
                                                    </asp:DropDownList>
                                                    <asp:ImageButton ID="used_type_bt" Visible="false" ImageUrl="~/image/icon_24.png" ToolTip="使用類型" runat="server" CommandArgument="usedtype" OnClick="meeting_item_Click" />
                                                    <asp:SqlDataSource runat="server" ID="useclass_sd" ConnectionString="<%$ ConnectionStrings:eip %>" SelectCommand="SELECT 0 AS meeting_usedtype_id, '請選擇' AS usedtype_name UNION ALL select meeting_usedtype_id,usedtype_name from meeting_apply_usedtype"></asp:SqlDataSource>
                                                </div>
                                            </div>
                                            <div class=" col-5 formItem title3">
                                                <div class="ItemTitle"><span>其他說明</span></div>
                                                <div class="ItemContent">
                                                    <asp:TextBox ID="other" CssClass="form-control" runat="server" Style="width: 85%;" Enabled="false" />
                                                </div>
                                            </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <hr />
                                </div>
                                <div class="Box" style="padding: 0px 40px;">
                                    <div class="row" style="margin-bottom: 10px;">
                                        <div class=" col-12 formItem">
                                            <div class="ItemTitle" style="width: auto;"><span>供餐</span></div>
                                            <div class="ItemContent d-flex align-items-center" style="padding-left: 2.4rem;">
                                                <span style="text-align: left; letter-spacing: 0px; color: #375471; font-size: 14px;">(請依實際是否供餐再做填寫；外帶為除便當以外，提供非塑膠包裝之餐點個數)</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 10px;">
                                        <div class=" col-12 formItem title3">
                                            <div class="ItemTitle"><span></span></div>
                                            <div class="ItemContent d-flex align-items-center">
                                                <div class="d-flex align-items-center"><span class="mr-2">使用環保餐盒</span><asp:TextBox runat="server" ID="lunch_box" CssClass="form-control mr-2" Style="width: 35%" TextMode="Number" oninput="checkPositiveNumber(this);" /><span>個</span></div>
                                                <div class="d-flex align-items-center"><span class="mr-2">外帶</span><asp:TextBox runat="server" ID="Takeaway" CssClass="form-control mr-2" Style="width: 35%" TextMode="Number" oninput="checkPositiveNumber(this);" /><span>個</span></div>
                                                <div class="d-flex align-items-center" style="margin-left: -2.5rem"><span class="mr-2" style="white-space: nowrap">使用一次性產品</span><asp:TextBox runat="server" ID="disposable" CssClass="form-control mr-2" Style="width: 35%" TextMode="Number" oninput="checkPositiveNumber(this);" /><span>個</span></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-bottom: 10px;">
                                        <div class=" col-12 formItem title3">
                                            <div class="ItemTitle"><span></span></div>
                                            <div class="ItemContent d-flex align-items-center">

                                                <asp:UpdatePanel ID="UpdatePanel6" runat="server" class="d-flex align-items-center">
                                                    <ContentTemplate>
                                                        <span style="white-space: nowrap; margin-right: 10px;">無法配合原因</span>
                                                        <asp:CheckBox Text="訂購數量" ID="reason_ck1" runat="server" Style="white-space: nowrap" AutoPostBack="true" />
                                                        <asp:CheckBox Text="收送時間" ID="reason_ck2" runat="server" Style="white-space: nowrap" AutoPostBack="true" />
                                                        <asp:CheckBox Text="辦理場地" ID="reason_ck3" runat="server" Style="white-space: nowrap" AutoPostBack="true" />
                                                        <asp:CheckBox ID="other_reason" Text="其他" runat="server" Style="white-space: nowrap" AutoPostBack="true" OnCheckedChanged="Unnamed_CheckedChanged" />
                                                        <asp:TextBox ID="other_reason_txt" runat="server" CssClass="form-control" Width="30%" Enabled="false"></asp:TextBox>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                </div>
                                <div class="Box" style="padding: 0px 40px;">
                                    <div class="row" style="margin-bottom: 10px;">
                                        <div class=" col-5 formItem title3">
                                            <div class="ItemTitle"><span>出席人數<span style="color: #ff0000">*</span></span></div>
                                            <div class="ItemContent d-flex align-items-center">
                                                <asp:TextBox ID="number" class="form-control" runat="server" Style="width: 70%;" />
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" style="width: 100%; display: flex; flex-direction: column">
                                                    <ContentTemplate>
                                                        <asp:Label ID="room_number" runat="server" Text="" Style="text-align: left; letter-spacing: 0px; color: #8F8F8F; font-size: 14px; width: 100%; flex-wrap: nowrap; margin-left: 5px;" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" style="margin-bottom: 10px;">
                                        <div class=" col-8  title3">
                                            <div class="ItemTitle"><span>備註</span></div>
                                            <div class="ItemContent">
                                                <asp:TextBox ID="note" class="form-control" runat="server" TextMode="MultiLine" Columns="150" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <hr>
                            <div style="text-align: center">
                                <asp:Button ID="cancel" runat="server" Text="取消" class="bt" Style="background: #777777"
                                    OnClientClick="return hideModal1();" />
                                <asp:Button ID="submitbt" runat="server" Text="儲存" class="bt" OnClick="submitbt_Click" />
                                <asp:Button ID="del" runat="server" Text="刪除此申請" class="bt" Style="background: #B83F1F" OnClick="del_Click" />
                                <asp:Button ID="modify" runat="server" Text="修改" class="bt" OnClick="modify_Click" />
                                <asp:HiddenField ID="del_id" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
       
    </div>
     <div class="modal fade bs-NewUser-modal-lg" id="modal2" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
     <div class="modal-dialog modal-lg modal-dialog-centered" style="width: 550px;">
         <div class="modal-content">
             <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                 <b><span id="Span2" runat="server" style="color: #003168">刪除確認</span></b>
                 <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                     <image src="image/popup_close.png"></image>
                 </button>
             </div>
             <div class="modal-body">
                 <div class="MainClass">
                     <div id="content2" role="form" class="form-horizontal text-center" style="margin-top: 50px; margin-bottom: 50px;">
                         <b>
                             <asp:Label ID="deltxt" runat="server" Text="" /></b>
                         <br>
                         <b>確認後無法修改</b>
                     </div>
                     <hr>
                     <div style="text-align: center">
                         <asp:Button ID="Button2" runat="server" Text="取消" class="bt" Style="background: #777777"
                             OnClientClick="return hideModal2();" />
                         <asp:Button ID="md2del_sing" runat="server" Text="確認" class="bt" OnClick="md2del_sing_Click" />
                         <asp:Button ID="md2del_let" runat="server" Text="全部刪除" class="bt" OnClick="md2del_let_Click" />
                         <asp:Button ID="md2del_other" runat="server" Text="僅刪除此筆" class="bt" OnClick="md2del_other_Click" Style="background: #B83F1F" />
                     </div>
                 </div>
             </div>
         </div>
     </div>
 </div>

    <%--常用modal--%>
    <div class="modal fade bs-NewUser-modal-lg" id="modal3" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
         <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 350px !important;">
             <div class="modal-content">
                 <asp:UpdatePanel ID="Updatepanel7" runat="server" UpdateMode="Conditional">
                     <ContentTemplate>
                         <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                             <b><span id="md3span" runat="server" style="color: #003168"></span></b>
                             <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                                 <image src="image/popup_close.png"></image>
                             </button>
                         </div>
                         <div class="modal-body">
                             <div class="MainClass">
                                 <div id="content3" role="form" class="form-horizontal text-center" style="">
                                     <span id="title_fav" runat="server" style="color: #145597; font-size: 0.875rem;"></span>
                                     <div style="padding: 0px 20px; display: flex; margin-top: 10px;">
                                         <div style="width: 80%;">
                                             <asp:TextBox runat="server" ID="add_favorite_tb" CssClass="form-control" MaxLength="25" AutoPostBack="true"></asp:TextBox>
                                         </div>
                                         <asp:Button ID="add_favorite_btn" runat="server" Text="新增" class="btn BT_orange ml-4" Style="padding: 2px 8px;" OnClick="add_favorite_btn_Click" />
                                     </div>
                                     <hr />
                                     <asp:Panel ID="favorite_pl" runat="server" CssClass="scroll-container"></asp:Panel>
                                 </div>
                             </div>
                         </div>
                     </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="mnbt" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="hostbt" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="add_favorite_btn" EventName="Click" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
         </div>
    </div>
    <%--常用modal--%>

    <%--會議類型&使用類型 modal--%>
    <div class="modal fade bs-NewUser-modal-lg" id="modal_item_add" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true" style="backdrop-filter: brightness(0.3);">
         <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 350px !important;">
             <div class="modal-content">
                 <asp:UpdatePanel ID="meeting_item_ul" runat="server" UpdateMode="Conditional">
                     <ContentTemplate>
                         <div class="modal-header" style="background-color: #C5ECFF; font-size: 22px; display: flex; justify-content: center; align-items: center; position: relative;">
                             <b><span id="item_tile" runat="server" style="color: #003168"></span></b>
                             <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="position: absolute; right: 10px;">
                                 <image src="image/popup_close.png"></image>
                             </button>
                         </div>
                         <div class="modal-body">
                             <div class="MainClass">
                                 <div id="content3" role="form" class="form-horizontal text-center" style="">
                                     <span id="Span3" runat="server" style="color: #145597; font-size: 0.875rem;"></span>
                                     <div style="padding: 0px 20px; display: flex; margin-top: 10px;">
                                         <div style="width: 80%;">
                                             <asp:TextBox runat="server" ID="item_tb" CssClass="form-control" MaxLength="25" AutoPostBack="true"></asp:TextBox>
                                         </div>
                                         <asp:Button ID="add_item_btn" runat="server" Text="新增" class="btn BT_orange ml-4" Style="padding: 2px 8px;" OnClick="add_meeting_item_Click" />
                                     </div>
                                     <hr />
                                     <asp:Panel ID="modal_item_pl" runat="server" CssClass="scroll-container" style="max-height: 300px; overflow-y: auto;"></asp:Panel>
                                 </div>
                             </div>
                         </div>
                     </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="meeting_type_bt" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="used_type_bt" EventName="Click" />
                         <asp:AsyncPostBackTrigger ControlID="add_item_btn" EventName="Click" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
         </div>
    </div>
    <%--會議類型&使用類型modal--%>
</asp:Content>

