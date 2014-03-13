/*Create QUANGNT 4/10/2012*/
var tooltip = {}, isLoad = 0;
function showToolTip(id) {
var strHTML=GetDataTooltip(id);
    Tip(strHTML, WIDTH, 300, ABOVE, true);
}

function GetDataTooltip(id) {
    var el = $("div[onmouseover=\"showToolTip('" + id + "')\"]");
    /*truntp modified 10/12/2012*/
    if (tooltip[id] == undefined || tooltip[id] == null) {
        el.unbind('onmouseover');
        if (isLoad != id) {
            isLoad = id;
            $.ajax({
                url: 'ajax/tooltip/'+id,
                dataType: 'json',
                cache: true,
                async: false,
                type: 'GET',
                success: function (data) {
                    tooltip[id] = data;
                    el.mousemove(function (e) {
                        el.bind('onmouseover', showToolTip(id));
                        el.unbind('mousemove');
                    });
                    isLoad = 0;
                }
            });
        }
    }
    return getStringTooltip(id);
}

function getStringTooltip(id) {
    if (tooltip[id] == undefined || tooltip[id] == null)
        return "";
    var data = tooltip[id];
    var strhtml = "<div id=\"mystickytooltip\" class=\"mytooltip\">";
    strhtml += "<div id=\"sticky" + data.id + "\" class=\"atip\">";
    strhtml += "<div class=\"tipname\">" + data.n + "</div>";
    strhtml += "<div class=\"tipprice\">";
    // strhtml += "<span class=\"giaol\">" +data.p+"</span>";
    if(data.p!=null)
        strhtml += "<span class=\"giaol\">" + data.p + "</span>";
    //    strhtml +=  data.pold == "null" || data.pold == "0" || data.pold == "" ? "" : "</br>Giá shop:<span class=\"giaol\">" +data.pold+ "</span>";
    //strhtml += data.sv == "" ? "" : "</br>Ti?t ki?m:<span class=\"giaol\">" +data.sv+ "</span>";
    strhtml += "</div>";
    // strhtml += "<span>" + data.st == "True" ? "Còn hàng" : "H?t hàng" + "</span>";
    //strhtml += data.st == "True" ? "<div class=\"status\">Còn hàng</div>" : "<div class=\"statusEnd\" >H?t hàng</div>";
    strhtml += data.st != "<div class=\"status\">Còn hàng</div>" ? "<br/>" + data.st : " ";
    strhtml += "<hr class=\"line\"/>";
    if (data.numdis > 0)
        strhtml += "Giảm giá: <span class=\"giaol\">" + data.numdis + "%</span>";
    if (data.dis != ""||data.sv!=null) {
        strhtml += "<div class=\"promotion-tooltip\"><span class=\"promotionText\">Khuyến mãi</span>"
        strhtml += "<ul class=\"listPromotion\">";
        if (data.dis != null) {
         strhtml+= data.dis;
        }
        if (data.sv != null)
            strhtml += "<li>Bạn tiết kiệm " + data.sv + " nếu mua hôm nay</li>";
        if (data.text != null)
            strhtml +="<li>"+ data.text+"</li>";
        strhtml += "</ul>";
       
        strhtml += "</div>";
    }
    strhtml += "<div class=\"content\">" + data.sdes + "</div>";
    strhtml += "</div>";
    strhtml += "</div>";
    strhtml += "</div>";

    return strhtml;
}