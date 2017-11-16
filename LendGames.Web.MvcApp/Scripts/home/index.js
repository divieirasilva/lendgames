$('#connectForm').on('submit', function (e) {
    $('#connectButton').attr('disabled', true);
    $('#connectButton').children('span').text('Conectando...');
    $('#connectButton').children('i').removeClass('hidden');
});

$('#connectForm').on('keypress', function (e) {
    if (e.keyCode === 13) {
        $('#connectForm').submit();
    }
});