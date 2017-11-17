$('#LendForm').on('submit', function (e) {
    $('#LendButton').attr('disabled', true);
    $('#LendButton').children('span').text('Emprestando...');
    $('#LendButton').children('i').removeClass('hidden');
});