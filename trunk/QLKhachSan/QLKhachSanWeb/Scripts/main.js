function showPopup(str) {
    $("#popup").modal();
    $("#popup").html(str);
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
    $(document).on("click", "td[status]", function (e) {
        var strHtml = "<div class='tooltip'><select name=status roomid=" + $(this).attr("id") + " id='changeStatus'>";
        for (var i in ROOM_STATUS) {
            var j  = parseInt(i) + 1;
            strHtml += "<option " + (parseInt($(this).attr("status")) == j ? "selected=true" : "") + " value=" + j + ">" + ROOM_STATUS[i] + "</option>";
        }
        strHtml + "</select>";        
        strHtml += "</div>";
        var pos = $(this).position();
        if ($("#change").length > 0)
            $("#change").remove();
        $("body").append($("<div id='change'/>")
            .html(strHtml)
            .css({
                position: "absolute",
                top: pos.top,
                left:pos.left
            })
            );
        
    });
    $(document).on("change", "select[name=status]", function () {
        $.post("/Booking/ChangeStatus",{ id: $(this).attr("roomid"), status: $(this).find("option:selected").val() }, function (d) {
            if (d == 1)
                $("select[name=status]").remove();
                window.location.reload();
        })
    });

})
$(document).on("submit", "form", function (event) {
    var form = $(this);

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
                   
                    $.modal.close();
                   window.location.reload();
                }
                else
                    alert(d);

            },
            error: function () {
                alert("Lỗi ! không thành công!");
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