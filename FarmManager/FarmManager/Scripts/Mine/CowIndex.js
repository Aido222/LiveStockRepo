$(document).ready(function () {


    $('#tabs div').hide();
    $('#tabs div:first').show();
    $('#tabs ul li:first').addClass('active');
    $('#tabs ul li a').click(function () {
        $('#tabs ul li').removeClass('active');
        $(this).parent().addClass('active');
        var currentTab = $(this).attr('href');
        $('#tabs div').hide();
        $(currentTab).show();
        return false;
    });




    $(".treatLink").click(function () {

        var id = $(this).attr('data-tag');


        $.post("/Cows/RetrieveTreatments", { ID: id },
            function (data) {


                $("#overlay_form").fadeIn(1000);
                positionPopup();

                $('#modalHead').text(data[2] + " (" + data[1] + ")");
                $('#batch').text(data[3]);
                $('#dosage').text(data[0]);





                $("#btnView").attr("href", "../Treatment/Details/" + id);

            });
    });


    function positionPopup() {
        if (!$(".overlay_form").is(':visible')) {
            return;
        }
        $(".overlay_form").css({
            left: ($(window).width() - $('.overlay_form').width()) / 2,
            top: ($(window).width() - $('.overlay_form').width()) / 7,
            position: 'absolute'
        });
    }


    $("#close").click(function () {
        $("#overlay_form").fadeOut(500);
    });

    $("#pop").click(function () {
        $("#overlay_form").fadeIn(1000);
        positionPopup();
    });
    $("#close").click(function () {
        $("#overlay_form").fadeOut(500);
    });




    $(".tabLinks").click(function () {

        var status = $(this).text();

        $('#cowStatusHead').text(status);


    });

});