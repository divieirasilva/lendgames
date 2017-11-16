$(function () {
    loadGamesList(1);
})

function loadGamesList(page) {
    $.ajax({
        url: '/Game/GamesList',
        data: {
            page,
            search: $('#Search').val()
        },
        async: true,        
        beforeSend: function () {
            $('#GamesListHolder').html('');
            $('#GamesListHolder').addClass('loading-box');
        },
        complete: function () {
            $('#GamesListHolder').removeClass('loading-box');
        }
    }).success(function (data) {
        if (data) {
            $('#GamesListHolder').html(data);
        }
    }).error(function (jqXHR, textStatus, errorThrown) {
        console.error(errorThrown);
    });
}