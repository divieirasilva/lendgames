$('#GiveBackForm').on('submit', function (e) {
    $('#GiveBackButton').attr('disabled', true);
    $('#GiveBackButton').children('span').text('Emprestando...');
    $('#GiveBackButton').children('i').removeClass('hidden');
});