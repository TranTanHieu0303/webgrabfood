$(document).ready(function () {
    var listdiv = $('.item-link').toArray();
    var ite;
    var osetsheader = $('.header').height();
    $(window).scroll(function (event) {
        var pos = $('html,body').scrollTop();
        var osets = $('#footer').offset();
        var topmenu = $('.menu').offset().top;
        var he = $('.menu').height();
        var op = osets.top;
        console.log(osetsheader);
        if (pos > 470 && ((pos + he) < op)) {
            if (osetsheader == null) {
                $('.menu').css({ "position": "fixed", "top": "0" });

            }

            else {
                
                $('.menu').css({ "position": "fixed", "top": osetsheader });
            }

        }
        else
            if (((pos + he) > op)) {
                $('.menu').css({ "position": "absolute", "top": "auto" });
               /* $('#giohang').css({ "position": "absolute", "top": "auto" });*/
            }
                
            else {
                $('.menu').css({ "position": "absolute", "top": "auto" });
               /* $('#giohang').css({ "position": "absolute", "top": "auto" });*/
            }
       
        listdiv.forEach(function (item, index, array) {
            var offsets = $('#section-' + item.id).offset();
            var top = offsets.top;
            if (osetsheader != null)
                top -= osetsheader;
            console.log(top);
            if (ite == null) { 
                if (pos >= top) {
                    
                    ite = $('#' + item.id);
                    ite.addClass('active');
                }
            }
            else {
                if (ite != $('#' + item.id)) {
                    if (pos >= top) {
                        ite.removeClass('active');
                        ite = $('#' + item.id);
                        ite.addClass('active');
                    }
                }
            }
        });
    })
    
    listdiv.forEach(function (item, index, array) {
        var ofsets = $('#section-' + item.id).offset();
        var top1 = ofsets.top;
        if (osetsheader != null)
            top1 -= osetsheader;
        console.log(top1);
        $('#' + item.id).click(function (event) {
            $('html,body').animate({ scrollTop: top1 }, 400);
        });
    })
});
