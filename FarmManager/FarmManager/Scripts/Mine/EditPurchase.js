jQuery(document).ready(function () {


    var TopResult;

    $('.datepicker').datepicker({

        format: 'yyyy/mm/dd'
    });


    /* $('.datepicker').datepicker();
 
     $('#DOB').datepicker()
   .on('changeDate', function (ev) {
       if (ev.date.valueOf() > new Date()) {
           $("#greaterDateWarning").slideDown("slow");
 
           DateOkCheck = false;
       }
       else {
           $("#greaterDateWarning").slideUp("slow");
 
           DateOkCheck = true;
       }
   });*/



    //Tag No check
    $("#TagNo").focusout(function () {

        var tagnumber = $("#TagNo").val();

        $.post("/Cows/CheckTagNo", { TagNumber: tagnumber },
            function (data) {
                TopResult = data;

                if (data == true) {
                    $("#SameTagWarning").slideDown("slow");

                } else {
                    $("#SameTagWarning").slideUp("slow");
                }

            });




    });
    //End of tag no check






    $("#btnSubmit").click(function () {

        $("#Sex option:selected").text();



        if (TopResult == true) {
            event.preventDefault();
        }




        


        if ($("#TagNo").val() == "") {
            $("#EnterTagWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#EnterTagWarning").slideUp("slow");

        }



        if ($("#Breed option:selected").text() == "Please select") {
            $("#selectBreedWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectBreedWarning").slideUp("slow");

        }


        if ($("#DOB").val() == "Please select") {
            $("#enterAgeWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#enterAgeWarning").slideUp("slow");

        }


        if ($("#Sex option:selected").text() == "Please select") {
            $("#selectSexWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectSexWarning").slideUp("slow");

        }









        if ($("#BoughtFrom").val() == "") {
            $("#selectBoughtFromWarning").slideDown("slow");

            event.preventDefault();


        }
        else {
            $("#selectBoughtFromWarning").slideUp("slow");

        }



        if ($("#Location").val() == "") {
            $("#selectLocationWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectLocationWarning").slideUp("slow");

        }


        var price = $("#Price").val();
        if (price.match(/^\d+$/)) {

            $("#selectPriceWarning").slideUp("slow");
        }
        else {
            $("#selectPriceWarning").slideDown("slow");
            event.preventDefault();
        }


    });

});