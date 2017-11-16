$(function () {
    loadDashboardInfo();
})

function loadDashboardInfo() {
    $.ajax({
        url: '/Account/DashboardInfo',
        async: true,
        beforeSend: function () {
            $('#DashboardInfoHolder').html('');
            $('#DashboardInfoHolder').addClass('loading-box');
        },
        complete: function () {
            $('#DashboardInfoHolder').removeClass('loading-box');
        }
    }).success(function (data) {
        if (data) {
            $('#DashboardInfoHolder').html(data);
        }
    }).error(function (jqXHR, textStatus, errorThrown) {
        console.error(errorThrown);
    });
}