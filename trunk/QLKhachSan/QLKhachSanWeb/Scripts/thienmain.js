

function initSize() {
    var width = $(window).width();
    var numitem = $(".table-data tr:first-child th").length;
    var widthIt = parseInt(width / numitem);
    $(".table-data th,.table-data td").width(widthIt);


}

$(function () {
    $(".datepicker").datepicker({ format: "dd/MM/yyyy" });
    initSize();

});