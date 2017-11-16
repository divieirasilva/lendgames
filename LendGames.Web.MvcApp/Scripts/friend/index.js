$(function () {
    loadFriendsList(1);
})

function loadFriendsList(page) {
    $.ajax({
        url: '/Friend/FriendsList',
        data: {
            page,
            search: $('#Search').val()
        },
        async: true,
        beforeSend: function () {
            $('#FriendsListHolder').html('');
            $('#FriendsListHolder').addClass('loading-box');
        },
        complete: function () {
            $('#FriendsListHolder').removeClass('loading-box');
        }
    }).success(function (data) {
        if (data) {
            $('#FriendsListHolder').html(data);
        }
    }).error(function (jqXHR, textStatus, errorThrown) {
        console.error(errorThrown);
        });
}