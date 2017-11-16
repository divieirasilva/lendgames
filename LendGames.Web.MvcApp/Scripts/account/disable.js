$('#AccountForm').on('submit', function (e) {
    $('#DisableButton').attr('disabled', true);
    $('#DisableButton').children('span').text('Desativando...');
    $('#DisableButton').children('i').removeClass('hidden');
});