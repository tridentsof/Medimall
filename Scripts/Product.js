$(window).ready(function () {
    $("#txt-search-product").on('keypress', function (e) {

        if (e.which == 13 && $("#txt-search-product").val() != "") {

            var searchString = $.trim($('#txt-search-product').val());
            $('#main-content-medimall').hide();
            $('.wrap-header').show();
            $('.container-main').show();

            $('#search-key').empty();
            $('#search-key').append(searchString);
            $.ajax({
                url: "/Home/ListProductSearch",
                type: "POST",
                data: { searchString: searchString },
                async: false,
                cache: false,
                success: function (data) {
                    $('#list-products').empty();
                    $(data).find("#list-products .item").each(function () {
                        $("#list-products").append(this);
                    })
                },
                error: function () {
                    $('#error-search').append("Không tìm thấy")
                }
            });
        }
        else if (e.which == 13 && $("#txt-search-product").val() == "") {
            alertify.alert("Nhập từ khóa","Mời bạn nhập từ khóa", function () {

            })
        }
    })
});

function addToCart(productId) {
    window.location.href = 'Home/AddToCart/' + productId;
}



