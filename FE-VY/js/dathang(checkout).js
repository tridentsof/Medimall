$(".box-money").click(function () {
  $(".box-money").removeClass("clicked");
  $(this).addClass("clicked");
  return false;
});
$(".btn-location").click(function () {
  $(".btn-location").removeClass("location");
  $(this).addClass("location");
  return false;
});
$(function () {
  $("#coupon_question").on("click", function () {
    $(".answer").toggle(this.checked);
  });
});

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