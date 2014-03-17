function showPopup(str) {
    $("#popup").modal();
    $("#popup").html(str);
    UnTip();
    $.validator.unobtrusive.parse($('form')[0]);

}
function showToolTip(strHTML) {
    Tip(strHTML, WIDTH, 300, ABOVE, true);
}
var tooltipBookingInfo = {};
function showInfoBooking(id) {
    var check = tooltipBookingInfo[id] == undefined;
    if (check) {
        $.get("/Booking/Detail/" + id, function (d) {

            tooltipBookingInfo[id] = d;
            showToolTip(tooltipBookingInfo[id]);
        })

    }
    if (!check)
        showToolTip(tooltipBookingInfo[id]);

}

var tooltipRoomInfo = {};
function showInfoRoom(id) {
    var check = tooltipRoomInfo[id] == undefined;
    if (check) {
        $.get("/Booking/RoomDetail/" + id, function (d) {
            tooltipRoomInfo[id] = d;
            showToolTip(tooltipRoomInfo[id]);
        })

    }
    if (!check)
        showToolTip(tooltipRoomInfo[id]);

}

function initSize()
{
    var width = $(window).width();
    var numitem = $(".table-data tr:first-child th").length;
    var widthIt = parseInt(width / numitem);
    $(".table-data th,.table-data td").width(widthIt);


}
function search(isToday)
{
    var fromDate = $("input[name=fromdate]").val(), endDate = $("input[name=enddaate]").val();
    if (isToday)
    {
        fromDate = null;
        endDate = null;
    }
    if (fromDate == "")
    {
        alert("Vui lòng chọn ngày");
        return;

    }
    if (endDate == "")
    {
        alert("Vui lòng chọn ngày");
        return;
    }

    
    $.post("/Home/ShowBookingInfo", { startDate: fromDate, endDate: endDate }, function (data) {
        if (data == "nologin")
            window.location.reload();
        $("#showBooking").hide();
        $("#showBooking").html(data);
       
        initSize();
        $("#showBooking").fadeIn();
    })
}
$(function () {
    $(".datepicker").datepicker();
    initSize();
});
$(document).ready(function () {
    $(document).ajaxStart(function () {
        $(".div-loading").show();
    }).ajaxComplete(function () {
        $(".div-loading").hide();
    })
    $(document).on("mouseenter", "#simplemodal-overlay", function () { UnTip(); })
    $(document).on("change", "#ddlRoom", function () {
        var isDango = $(this).attr("name") == "Room";
        var ddlbooking = $("#ddlListBooking");
        ddlbooking.html("");
        $.get("/Booking/GetListBookingInfo", { roomId: $(this).find("option:selected").val(),dango:isDango }, function (d) {

            if (d.length > 0)           {
              
               
                $.each(d, function (i, e) {
                    ddlbooking.append($("<option/>").html(e.Text).val(e.Value));
                })
                ddlbooking.change();
            }
        })
    })
    $(document).on("change", "#ddlListBooking", function () {
        $("#detail").html("");
        $.get("/Booking/Detail/" + $(this).find("option:selected").val(), function (d) {
            $("#detail").html(d);
           
        })
    })
    $(document).on("change", ".showbooking", function () {
        $.get("/Booking/GetListBookingInfoDetail/", { bookingId: $(this).find("option:selected").val() }, function (d) {
            $("#listService").html(d);
            $("#AddServiceModel_ServiceId").change();
                     $.modal.setPosition();
        })
    })
    $(document).on("click","#btnChangeStatus",function(){
        changeStatus($("#ddlListBooking option:selected").val(),3,false);
    })
  
    $(document).on("change", "#AddServiceModel_ServiceId", function () {
        $.get("/Booking/GetPrice", {serviceId:$(this).find("option:selected").val()},function(d){
            $("#AddServiceModel_Price").val(d);
            calculatorPrice();
        })
       
    })
    $(document).on("blur", "#AddServiceModel_Price", function () {
        calculatorPrice();
    })
    $(document).on("blur", "#AddServiceModel_Quatity", function () {
        calculatorPrice();
    })
  
    $(document).on("click","#btnCheckIn",function(){
        $.get("/Booking/CheckIn",function(d){
            showPopup(d);
            
        })
    });

    $(document).on("click", "#btnCheckOut", function () {
        $.get("/Booking/CheckOut", function (d) {
            showPopup(d);
            $("#ddlRoom").change();
        })
    });

    $(document).on("click","#btnTimKiem", function () {
        search(false);
    })
    $(document).on("click", "#btnGoToday", function () {
        search(true);
    })
    $(document).on("click", "#btnCancel", function () {
        $.modal.close();
    })
    $(document).on("mouseenter", "td[status]", function () {
        var id = $(this).attr("id");
        var els = $("td[status][id=" + id + "]");
        $.each(els, function (i) {
            var e = $(this);
            if (i == 0)
                e.css({
                    "border-left": "solid red"
                });
            if (i == els.length-1)
                e.css("border-right", "solid red");
            e.css({
                "border-top": "solid red",
                "border-bottom": "solid red"
            })
        })
    });
    $(document).on("mouseout", "td[status]", function () {
        var id = $(this).attr("id");
        var els = $("td[status][id=" + id + "]");
        $.each(els, function (i) {
            $(this).removeAttr("style");
        })
    });
    $("#btnDatPhong").bind("click",function () {
        $.get("/Booking/BookRoom", function (d) {

            showPopup(d);
            $(".datepicker").datepicker({format:"dd/MM/yyyy"});
        })
    })
    $(document).on("click","#btnPayment",function () {
        Payment();
    })
    $("#btnXemLog").bind("click", function () {
        $.get("/Admin/AddUsers", function (d) {

            showPopup(d);
        })
    })


    $("#btnxemdanhsach").bind("click", function () {
        $.get("/Admin/ListUsers", function (d) {

            showPopup(d);
        })
    })

  



    $(document).on("mouseover", "td[status]", function () {
        showInfoBooking(parseInt($(this).attr("id")));
    });
    $(document).on("mouseover", "td.room", function () {
        showInfoRoom(parseInt($(this).attr("id")));
    });
    $(document).on("mouseout", "td[status],td.room", function () {
        UnTip();
    });
    //$(document).on("click", "td[status]", function (e) {
    //    var strHtml = "<div class='tooltip'><select name=status roomid=" + $(this).attr("id") + " id='changeStatus'>";
    //    for (var i in ROOM_STATUS) {
    //        var j  = parseInt(i) + 1;
    //        strHtml += "<option " + (parseInt($(this).attr("status")) == j ? "selected=true" : "") + " value=" + j + ">" + ROOM_STATUS[i] + "</option>";
    //    }
    //    strHtml + "</select>";        
    //    strHtml += "</div>";
    //    var pos = $(this).position();
    //    if ($("#change").length > 0)
    //        $("#change").remove();
    //    $("body").append($("<div id='change'/>")
    //        .html(strHtml)
    //        .css({
    //            position: "absolute",
    //            top: pos.top,
    //            left:pos.left
    //        })
    //        );
        
    //});
    $(document).on("change", "select[name=status]", function () {
      
        changeStatus($(this).attr("roomid"), $(this).find("option:selected").val(),true);
    });
    $(document).on("click", "#btnCancelUpDate", function () {
      
        resetForm();
    })


})
function resetForm()
{
    $("#AddServiceModel_Id").val("");
    $("#btnInsertSV").val("Thêm");
    $("#btnCancelUpdate").hide();
}
function Payment()
{
    var id = $("#ddlListBooking option:selected").val();
    $.ajax({
        type: "POST",
        url: "/Booking/CheckOut",
        data: { bookingInfoId: id },
        success: function (d) {
            if (d.length < 50)
                alert(d);
            else
            {
                $("#popup").html(d);
            }
        }
    })
}
function calculatorPrice()
{
    var quatity = parseInt($("#AddServiceModel_Quatity").val());
    var price = parseInt($("#AddServiceModel_Price").val());
    $("#AddServiceModel_Total").val(quatity * price);
}
function changeStatus(id, status,select)
{
    $.post("/Booking/ChangeStatus", { id: id, status: status }, function (d) {
        if (d == 1) {
            alert("Cập nhật thành công!");
            if(select)
                $("select[name=status]").remove();
        }
        else
            alert("Cập nhật không thành công");
        window.location.reload();
    })
}
function loadDataEdit(id)
{
    var td = $("#data-" + id);
    $("#AddServiceModel_Id").val(id);
    $("#AddServiceModel_ServiceId").val($("td:eq(1)", td).attr("data-value"));
    $("#AddServiceModel_Quatity").val($("td:eq(2)", td).attr("data-value"));
    $("#AddServiceModel_Price").val($("td:eq(3)", td).attr("data-value"));
    $("#AddServiceModel_Total").val($("td:eq(4)", td).attr("data-value"));
    $("#btnInsertSV").val("Cập nhật");
    $("#btnCancelUpdate").show();
}
function deleteService(id)
{
    if (confirm("Bạn có chắc xóa ?")) {
        $.post("/Booking/DeleteService", { id: id }, function (d) {
            alert(d);
            $("#ddlListBooking").change();
        })
    }
}

$(document).on("submit", "form", function (event) {
    var form = $(this);
    var id = form.attr("id");
    if (form.valid()) {
        
        $.ajax({
            url: form.attr("action"), // Not available to 'form' variable
            type: form.attr("method"),
            dataType: "json",// Not available to 'form' variable
            data: form.serialize(),
            success: function (d) {
                // Do something with the returned html.  
                if (d == 'nologin')
                {
                    window.location.reload();
                    return;
                }
                if (d == "success") {
                    if (id == "frmInsertService")
                    {
                        if ($("#AddServiceModel_Id").val() != "") {
                            alert("Cập nhật thành công!");
                            resetForm();
                        }
                        else
                            alert("Thêm thành công!");
                        $("#ddlListBooking").change();
                        return;
                    }
                    $.modal.close();
                   window.location.reload();
                }
                else
                    alert(d);

            },
            error: function () {
                alert("Lỗi ! không thành công!bạn chưa đăng nhập");
                location.reload();
            }
        });
    }

    event.preventDefault();
});
var ROOM_STATUS = [
    /// dat phong dam bao
    /// </summary>
    "Đặt phòng đảm bảo",
    /// <summary>
    /// Dat phong khong dam bao
    /// </summary>
   "Đặt phòng không đảm bảo",
    /// <summary>
    /// phong dang o
    /// </summary>
  "Phòng đang ở",
    /// <summary>
    /// phong da checkout
    /// </summary>
   "Phòng đã trả"
]