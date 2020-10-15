
        
$(document).ready(function(){

    $('.counter-count').each(function () {
        $(this).prop('Counter',0).animate({
            Counter: $(this).text()
        }, {
            duration: 5000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });



    $("#timer").hide();
    $(".confirm-btn").hide();
    $(".custom-btn").hide();
    $(".success-msg").hide();

    $("#user-form").hide();

    

    $("polygon").click(function(){
        $(this).toggleClass("bg-red");
    });
    $("rect").click(function(){
        $(this).toggleClass("bg-red");
    });
    $("path").click(function(){
        $(this).toggleClass("bg-red");
    });

    $(".book-btn").click(function(){
        $(this).hide();
        $(".confirm-btn").addClass("book-text-show");
        $(".cancel-btn").addClass("cancel-btn-show");
        $("#timer").show();
        $(".confirm-btn").show();
    });

    $(".booking-btn").click(function(){
        $(".plot-info").hide();
        $("#user-form").show();
    });
    $(".confirm-btn ").click(function(){
        $(".plot-info").hide();
        $("#user-form").show();
        $(".custom-btn").show();
        $(this).hide();

    });

    $(".custom-btn").click(function(){
        $(".plot-info").hide();
        $("#user-form").hide();
        $(".cancel-btn").hide();
        $(".success-msg").show();
        $(this).hide();
        
    });


    
   

    $(".hamburger").click(function(){
         $(this).toggleClass("is-active");
         $(".sidebar").toggleClass("open-sidebar");
    });

    $("#sidebar-menu").click(function () {
        $(".sidebar").toggleClass("open-sidebar");
    });
    $(".main-body").click(function () {
        if ($('.sidebar').hasClass('open-sidebar')) {
          $(".sidebar").toggleClass('open-sidebar')
          $(".hamburger").toggleClass("is-active");
        }
    });
    

    function makeTimer() {

           
        var endTime = new Date("29 April 2020 9:56:00 GMT+01:00");			
        endTime = (Date.parse(endTime) / 1000);

        var now = new Date();
        now = (Date.parse(now) / 1000);

        var timeLeft = endTime - now;

        var days = Math.floor(timeLeft / 86400); 
        var hours = Math.floor((timeLeft - (days * 86400)) / 3600);
        var minutes = Math.floor((timeLeft - (days * 86400) - (hours * 3600 )) / 60);
        var seconds = Math.floor((timeLeft - (days * 86400) - (hours * 3600) - (minutes * 60)));

        if (hours < "10") { hours = "0" + hours; }
        if (minutes < "10") { minutes = "0" + minutes; }
        if (seconds < "10") { seconds = "0" + seconds; }

        // $("#days").html(days + "<span>Days</span>");
        $("#hours").html(hours + "<span>Hours</span>");
        $("#minutes").html(minutes + "<span>Minutes</span>");
        $("#seconds").html(seconds + "<span>Seconds</span>");		

}

setInterval(function() { makeTimer(); }, 1000);
});
