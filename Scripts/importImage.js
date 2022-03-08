//get file size

function GetFileSize(fileid) {

    try {

        var fileSize = 0;

        if ($.browser.msie) {


            var objFSO = new ActiveXObject('Scripting.FileSystemObject'); var filePath = $('#' + fileid)[0].value;

            var objFile = objFSO.getFile(filePath);

            var fileSize = objFile.size; //size in kb

            fileSize = fileSize / 1048576; //size in mb

        }

        else {

            fileSize = $('#' + fileid)[0].files[0].size //size in kb

            fileSize = fileSize / 1048576; //size in mb

        }

        return fileSize;
    }

    catch (e) {

        alert('Lỗi kiểm tra fileSize(làm biếng debug quá) kệ mẹ nó đi, ấn ok để tiếp tục');

    }

}

//get file path from client system

function getNameFromPath(strFilepath) {

    var objRE = new RegExp(/([^\/\\]+)$/);

    var strName = objRE.exec(strFilepath);

    if (strName == null) {

        return null;

    }

    else {

        return strName[0];

    }

}



$(function () {

    $('#Photo').change(function () {

        var file = getNameFromPath($(this).val());

        if (file != null) {

            var extension = file.substr((file.lastIndexOf('.') + 1));

            switch (extension) {

                case 'jpg':

                case 'png':

                    flag = true;

                    break;

                default:

                    flag = false;

            }

        }

        if (flag == false) {

            $('#validationTxt').text('Bạn chỉ có thể tải lên dạng jpg,png');

            return false;

        }

        else {

            var size = GetFileSize('file');

            if (size > 3) {

                $('#validationTxt').text('Bạn chỉ có thể tải lên với kích thước 1 MB');

            }

            else {

                $('#validationTxt').text('');

            }

        }

    });

});