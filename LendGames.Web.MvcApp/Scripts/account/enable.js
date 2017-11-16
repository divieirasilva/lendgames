$('#AccountForm').on('submit', function (e) {
    $('#EnableButton').attr('disabled', true);
    $('#EnableButton').children('span').text('Ativando...');
    $('#EnableButton').children('i').removeClass('hidden');
});