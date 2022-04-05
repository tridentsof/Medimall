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

    var quantitiy = 1;
    $('.quantity-right-plus').click(function (e) {
        // Stop acting like a button
        e.preventDefault();
        // Get the field name
        var quantity = parseInt($('#quantity').val());
        $('#quantity').val(quantity + 1);

    });

    $('.quantity-left-minus').click(function (e) {
        // Stop acting like a button
        e.preventDefault();
        // Get the field name
        var quantity = parseInt($('#quantity').val());
        // Increment
        if (quantity > 1) {
            $('#quantity').val(quantity - 1);
        }
        
    });
    $('#back2').click(function (e) {
        $('#popup2').removeClass("active-cart");
        $('#blur').removeClass("blur");
        var quantity = parseInt($('#quantity').val());
        $('#quantity').val(quantity);
    });

});