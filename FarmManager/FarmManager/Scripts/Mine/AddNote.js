jQuery(document).ready(function () {


   

    $("form").on('submit', function (e) {
        $("#Sex option:selected").text();






        if ($("#Description").val() == "") {
            $("#RequiredWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#RequiredWarning").slideUp("slow");

        }


        if ($("#Note").val() == "") {
            $("#RequiredWarningNote").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#RequiredWarningNote").slideUp("slow");

        }






      




    });


});