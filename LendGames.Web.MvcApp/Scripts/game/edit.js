$('#GameForm').on('submit', function (e) {
    $('#SaveButton').attr('disabled', true);
    $('#SaveButton').children('span').text('Salvando...');
    $('#SaveButton').children('i').removeClass('hidden');
});

$('#GameForm').on('keypress', function (e) {
    if (e.keyCode === 13) {
        $('#GameForm').submit();
    }
});