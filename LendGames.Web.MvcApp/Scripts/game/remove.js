$('#GameForm').on('submit', function (e) {
    $('#RemoveButton').attr('disabled', true);
    $('#RemoveButton').children('span').text('Removendo...');
    $('#RemoveButton').children('i').removeClass('hidden');
});