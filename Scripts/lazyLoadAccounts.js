var ajaxCallUrl = 'AdminAccounts/AccountList',
    page = 0,
    inCallback = false,
    isReachedScrollEnd = false;


var scrollHandler = function () {
    if (isReachedScrollEnd == false &&
        ((window.innerHeight + window.scrollY) >= document.body.offsetHeight)) {
        loadProducts(ajaxCallUrl);
    }
}
function loadProducts(ajaxCallUrl) {
    if (page > -1 && !inCallback) {
        inCallback = true;
        page++;
        $("div#loading").show();
        $.ajax({
            type: 'GET',
            url: ajaxCallUrl,
            data: "pageNumber=" + page,
            success: function (data, textstatus) {
                if (data != '') {
                    $("table > tbody").append(data);
                }
                else {
                    page = -1;
                }

                inCallback = false;
                $("div#loading").hide();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
}