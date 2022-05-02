function usePowerPoint() {
    debugger;
    var powerPoint = $('#power-point').val();
    var totalMoney = $('#total-money').val();

    if (totalMoney < 100000) {
        alertify.error('Đơn hàng của bạn chưa đủ giá trị!')
    }
    else {
        if (powerPoint > 50000) {
            var finalMoney = totalMoney - 50000;
            var pointLeft = powerPoint - 50000;

            $('#show-money').empty();
            $('#total-money').val(finalMoney);
            $('#show-money').append(finalMoney);

            $('#show-powerpoint').empty();
            $('#show-powerpoint').append(pointLeft);
            $('#power-point').val(pointLeft);

            $('#is-use-point').val(1);
            $('#point-used').val(50000);
            alertify.success('Sử dụng điểm thành công!');
        }
        else if (powerPoint < 50000) {
            var finalMoney = totalMoney - powerPoint;

            $('#show-money').empty();
            $('#total-money').val(finalMoney);
            $('#show-money').append(finalMoney);

            $('#show-powerpoint').empty();
            $('#show-powerpoint').append(0);
            $('#power-point').val(0);

            $('#is-use-point').val(1);
            $('#point-used').val(powerPoint);
            alertify.success('Sử dụng điểm thành công!');
        }
    }
    
}

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