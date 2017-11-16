$('#FriendForm').on('submit', function (e) {
    $('#SaveButton').attr('disabled', true);
    $('#SaveButton').children('span').text('Salvando...');
    $('#SaveButton').children('i').removeClass('hidden');
});

$('#FriendForm').on('keypress', function (e) {
    if (e.keyCode === 13) {
        $('#FriendForm').submit();
    }
});