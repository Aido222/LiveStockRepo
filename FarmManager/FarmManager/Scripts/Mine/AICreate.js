jQuery(document).ready(function () {




    $('.datepicker').datepicker({

        format: 'yyyy/mm/dd'
    });




    $("#btnSubmit").click(function () {


        if ($("#Date").val() == "Please select") {
            $("#enterDateWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#enterDateWarning").slideUp("slow");

        }



        if ($("#Breed option:selected").text() == "Please select") {
            $("#selectBreedWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectBreedWarning").slideUp("slow");

        }



        if ($("#AIOperator").val() == "") {
            $("#selectAIOperatorWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectAIOperatorWarning").slideUp("slow");

        }


        if ($("#BullID").val() == "") {
            $("#enterBullIDWarn").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#enterBullIDWarn").slideUp("slow");

        }

    });




});