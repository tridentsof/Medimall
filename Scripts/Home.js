function loginHandle() {
    var userName = $('#uname').val();
    var passWord = $('#pwd').val();
    var dataSend = {
        userName: userName,
        passWord: passWord
    }
    $.ajax({
        type: "POST",
        url: "Home/LoginAjax",
        data: dataSend,
        success: function (result) {
            if (result == true) {
                alertify.success('Đăng nhập thành công');
                $('.modal-content center').hide();
                $('#myModal').hide();
                $('.modal-backdrop.fade').remove();
                $('#myModal').remove();
                $('body').removeClass('modal-open');
                $('#btn-login').hide();
                $('#user-session').show();

                $('#user-session').css('display','flex');
                $('#user-name').append(userName);
            }
            else {
                alertify.error('Sai tên đăng nhập hoặc mật khẩu');
            }
        }
    })


}