$(function () {
    loadAccountsList(1);
})

function loadAccountsList(page) {
    $.ajax({
        url: '/Account/AccountsList',
        data: {
            page,
            search: $('#Search').val()
        },
        async: true,
        beforeSend: function () {
            $('#AccountsListHolder').html('');
            $('#AccountsListHolder').addClass('loading-box');
        },
        complete: function () {
            $('#AccountsListHolder').removeClass('loading-box');
        }
    }).success(function (data) {
        if (data) {
            $('#AccountsListHolder').html(data);
        }
    }).error(function (jqXHR, textStatus, errorThrown) {
        console.error(errorThrown);
    });
}