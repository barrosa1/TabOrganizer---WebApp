// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//create container scripts

$(function () {
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = 'https://localhost:12345/Container/Create';
        $.get(url).done(function (data) {
            $('#modal-placeholder').html(data);
            $('#modal-placeholder > .modal').modal('show');
        });
    });
});

$("#modal-placeholder").on('click', '[data-save="modal"]', function (event) {
    event.preventDefault();


    var form = $(this).parents('.modal').find('form');
    var actionUrl = form.attr('action');
    var dataToSend = form.serialize();

    $.post(actionUrl, dataToSend).done(function (data) {
        var newBody = $('.modal-body', data);
        $("#modal-placeholder").find('.modal-body').replaceWith(newBody);

        // find IsValid input field and check it's value
        // if it's valid then hide modal window
        var isValid = newBody.find('[name="IsValid"]').val() == 'True';
        if (isValid) {
            $("#modal-placeholder").find('.modal').modal('hide');


            $.ajax({
                type: 'GET',
                url: '/Container/IndexPartial',
                success: function (data) {
                    $(".row").html(data);
                }
            });
        }
    });
});


//edit container scripts
//$("#modal-placeholder button[data-save='modal2']")
//"#modal-placeholder").on('click', '[data-save="modal"]'
$("#modal-placeholder").on('click', '[data-save="modal2"]', function (event) {
    event.preventDefault();


    var form = $(this).parents('.modal').find('form');
    var actionUrl = form.attr('action');
    var dataToSend = form.serialize();

    $.post(actionUrl, dataToSend).done(function (data) {
        var newBody = $('.modal-body', data);
        $("#modal-placeholder").find('.modal-body').replaceWith(newBody);

        // find IsValid input field and check it's value
        // if it's valid then hide modal window
        var isValid = newBody.find('[name="IsValid"]').val() == 'True';
        if (isValid) {
            $("#modal-placeholder").find('.modal').modal('hide');


            $.ajax({
                type: 'GET',
                url: '/Container/IndexPartial',
                success: function (data) {
                    $(".row").html(data);
                }
            });
        }
    });
});


//edit website scripts


$("#modal-placeholder").on('click', '[data-save="modal3"]', function (event) {
    event.preventDefault();


    var form = $(this).parents('.modal').find('form');
    var actionUrl = form.attr('action');
    var dataToSend = form.serialize();

    $.post(actionUrl, dataToSend).done(function (data) {
        var newBody = $('.modal-body', data);
        $("#modal-placeholder").find('.modal-body').replaceWith(newBody);

        // find IsValid input field and check it's value
        // if it's valid then hide modal window
        var isValid = newBody.find('[name="IsValid"]').val() == 'True';
        if (isValid) {
            $("#modal-placeholder").find('.modal').modal('hide');


            location.reload();

        }
    });
});


$(function () {
    $('button[data-toggle="ajax_modal_4"]').click(function (event) {

        //var containerId = @Context.Request.Query["id"];

        var url = '/Website/Create';
        $.get(url).done(function (data) {
            $('#modal-placeholder').html(data);
            $('#modal-placeholder > .modal').modal('show');
        });
    });
});


$("#modal-placeholder").on('click', '[data-save="modal4"]', function (event) {
    event.preventDefault();


    var form = $(this).parents('.modal').find('form');
    var actionUrl = form.attr('action');
    var dataToSend = form.serialize();

    $.post(actionUrl, dataToSend).done(function (data) {
        var newBody = $('.modal-body', data);
        $("#modal-placeholder").find('.modal-body').replaceWith(newBody);

        // find IsValid input field and check it's value
        // if it's valid then hide modal window
        var isValid = newBody.find('[name="IsValid"]').val() == 'True';
        if (isValid) {
            $("#modal-placeholder").find('.modal').modal('hide');


            location.reload();

        }
    });
});