jQuery(document).ready(function () {


    var TopResult;
    var DateOkCheck;

    //Date picker...

    $('.datepicker').datepicker();

    $('#BirthDate').datepicker()
   .on('changeDate', function(ev){
       if (ev.date.valueOf() > new Date()) {
           $("#greaterDateWarning").slideDown("slow");

           DateOkCheck = false;
       }
       else {
           $("#greaterDateWarning").slideUp("slow");

           DateOkCheck = true;
       }
   });

    
    $("form").on('submit', function (e) {
        $("#Sex option:selected").text();

        

        if (TopResult == true)
        {
            event.preventDefault();
        }

        if (DateOkCheck == false) {
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


        if ($("#BirthDate").val() == "Please select") {
            $("#selectDateWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectDateWarning").slideUp("slow");

        }




        



       /* if ($("#BirthDate option:selected").text() == "") {
            $("#selectDOBWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectDOBWarning").slideUp("slow");

        }*/

        

        if ($("#Sex option:selected").text() == "Please select") {
            $("#selectSexWarning").slideDown("slow");

            event.preventDefault();


        }
        else {
            $("#selectSexWarning").slideUp("slow");

        } 



        if ($("#Difficult option:selected").text() == "Please select") {
            $("#selectDifficultyWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectDifficultyWarning").slideUp("slow");

        }

        var selectedValue = $('input[name=InseminationType]:checked').val()

        if (selectedValue == 0) {

           // if ($("#Sex option:selected").text() == "Please select" || $("#Breed option:selected").text() == "Please select" || $("#MotherTagNo option:selected").text() == "Please select" || $("#SireTagNo option:selected").text() == "Please select" || $("#Difficult option:selected").text() == "Please select" || TopResult == true) {

             //   alert("No Good, Natural attempt");
               // event.preventDefault();
            //}

            if ($("#MotherTagNo option:selected").text() == "Please select") {
                $("#selectMotherTagBWarning").slideDown("slow");

                event.preventDefault();

            }
            else {
                $("#selectMotherTagBWarning").slideUp("slow");

            }


            if ($("#SireTagNo option:selected").text() == "Please select") {
                $("#selectSireTagWarning").slideDown("slow");

                event.preventDefault();

            }
            else {
                $("#selectSireTagWarning").slideUp("slow");

            }
            






            //if ($("#Sex option:selected").text() == "Please select" || $("#Breed option:selected").text() == "Please select" || $("#MotherTagNo option:selected").text() == "Please select" || $("#SireTagNo option:selected").text() == "Please select" || $("#Difficult option:selected").text() == "Please select" || TopResult == true) {

            //    alert("No Good, Natural attempt");
            //    event.preventDefault();
           // }
        }
        else {

            if ($("#AICow option:selected").text() == "Please select") {

                $("#selectAIWarning").slideDown("slow");
                event.preventDefault();
            }
            else {
                $("#selectAIWarning").slideUp("slow");

            }
            

        }
    });


    $("[name='InseminationType']").change(function () {
        var selectedValue = $('input[name=InseminationType]:checked').val()

        $(".expandingForm").slideDown("slow");

        if (selectedValue == 0)
        {
            $(".Natural").slideDown("slow");
            $(".AI").slideUp("slow");
            $('.hTop').text('New Calf Birth By Bull');

            //$(this).addClass("current");
            
        }
        else 
        {
            $(".AI").slideDown("slow");
            $(".Natural").slideUp("slow");
            $('.hTop').text('New Calf Birth By A.I');
        }
    });



    
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


});