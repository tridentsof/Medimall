$(document).ready(function () {  
    $('.nav-tabs li').click(function(e){
        $('.nav-tabs li').removeClass('active')
        $(this).addClass('active');
     });
});