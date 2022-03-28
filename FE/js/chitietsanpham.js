var btn_cong = document.querySelector('.plus')
var btn_tru = document.querySelector('.minus')


var i = document.getElementById('amount').value

function handle_Giam(){
    var value = parseInt(document.getElementById('amount').value)
      
        value--;
        console.log(value)
        if(value !== 1){
            btn_tru.style.cursor = "pointer"
            btn_tru.disabled = false
        }
        else{
            btn_tru.style.cursor = "not-allowed"
            btn_tru.disabled = true
        }
    document.getElementById('amount').value = value;
    
}


function handle_Tang(){
    var value = parseInt(document.getElementById('amount').value)
        value =isNaN(value) ? 0 : value;
        value++;
        console.log(value)
    document.getElementById('amount').value = value;
}

$(document).ready(function () {
	$('#carousel5').carousel({ interval: false });
	$('.item1').click(function () {
		$('#carousel5').carousel(0);
	});
	$('.item2').click(function () {
		$('#carousel5').carousel(1);
	});
	$('.item3').click(function () {
		$('#carousel5').carousel(2);
	});
	$('.carousel-control-prev').click(function () {
		$('#carousel5').carousel('prev');
	});
	$('.carousel-control-next').click(function () {
		$('#carousel5').carousel('next');
	});
});








