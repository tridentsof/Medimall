(function () {
	'use strict';
	window.addEventListener(
		'load',
		function () {
			var forms = document.getElementsByClassName('needs-validation');
			var validation = Array.prototype.filter.call(forms, function (form) {
				form.addEventListener(
					'submit',
					function (event) {
						if (form.checkValidity() === false) {
							event.preventDefault();
							event.stopPropagation();
						}
						form.classList.add('was-validated');
					},
					false
				);
			});
		},
		false
	);
})();

$(document).ready(function () {
	$('#carousel2').carousel({ interval: false });
	$('.item1').click(function () {
		$('#carousel2').carousel(0);
	});
	$('.item2').click(function () {
		$('#carousel2').carousel(1);
	});
	$('.item3').click(function () {
		$('#carousel2').carousel(2);
	});
	$('.carousel-control-prev').click(function () {
		$('#carousel2').carousel('prev');
	});
	$('.carousel-control-next').click(function () {
		$('#carousel2').carousel('next');
	});
});
$(document).ready(function () {
	$('#carousel3').carousel({ interval: false });
	$('.item1').click(function () {
		$('#carousel3').carousel(0);
	});
	$('.item2').click(function () {
		$('#carousel3').carousel(1);
	});
	$('.item3').click(function () {
		$('#carousel3').carousel(2);
	});
	$('.carousel-control-prev').click(function () {
		$('#carousel3').carousel('prev');
	});
	$('.carousel-control-next').click(function () {
		$('#carousel3').carousel('next');
	});
});
$(document).ready(function () {
	$('#carousel4').carousel({ interval: false });
	$('.item1').click(function () {
		$('#carousel4').carousel(0);
	});
	$('.item2').click(function () {
		$('#carousel4').carousel(1);
	});
	$('.item3').click(function () {
		$('#carousel4').carousel(2);
	});
	$('.carousel-control-prev').click(function () {
		$('#carousel4').carousel('prev');
	});
	$('.carousel-control-next').click(function () {
		$('#carousel4').carousel('next');
	});
});
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
$(document).ready(function () {
	$('#carousel6').carousel({ interval: false });
	$('.item1').click(function () {
		$('#carousel6').carousel(0);
	});
	$('.item2').click(function () {
		$('#carousel6').carousel(1);
	});
	$('.item3').click(function () {
		$('#carousel6').carousel(2);
	});
	$('.carousel-control-prev').click(function () {
		$('#carousel6').carousel('prev');
	});
	$('.carousel-control-next').click(function () {
		$('#carousel6').carousel('next');
	});
});
$(document).ready(function () {
	$('#carousel7').carousel({ interval: false });
	$('.item1').click(function () {
		$('#cacarousel7rousel2').carousel(0);
	});
	$('.item2').click(function () {
		$('#carousel7').carousel(1);
	});
	$('.item3').click(function () {
		$('#carousel7').carousel(2);
	});
	$('.carousel-control-prev').click(function () {
		$('#carousel7').carousel('prev');
	});
	$('.carousel-control-next').click(function () {
		$('#carousel7').carousel('next');
	});
});
$(document).ready(function () {
	$('#carousel8').carousel({ interval: false });
	$('.item1').click(function () {
		$('#carousel8').carousel(0);
	});
	$('.item2').click(function () {
		$('#carousel8').carousel(1);
	});
	$('.item3').click(function () {
		$('#carousel8').carousel(2);
	});
	$('.carousel-control-prev').click(function () {
		$('#carousel8').carousel('prev');
	});
	$('.carousel-control-next').click(function () {
		$('#carousel8').carousel('next');
	});
});
$(document).ready(function () {
	$('#carousel9').carousel({ interval: false });
	$('.item1').click(function () {
		$('#carousel9').carousel(0);
	});
	$('.item2').click(function () {
		$('#carousel9').carousel(1);
	});
	$('.item3').click(function () {
		$('#carousel9').carousel(2);
	});
	$('.carousel-control-prev').click(function () {
		$('#carousel9').carousel('prev');
	});
	$('.carousel-control-next').click(function () {
		$('#carousel9').carousel('next');
	});
});
