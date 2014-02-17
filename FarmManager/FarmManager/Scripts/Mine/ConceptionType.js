jQuery(document).ready(function () {

    var TopResult;

    $("form").on('submit', function (e) {
        $("#Sex option:selected").text();


        var selectedValue = $('input[name=InseminationType]:checked').val()

        if (selectedValue == 0) {

            if ($("#Sex option:selected").text() == "Please select" || $("#Breed option:selected").text() == "Please select" || $("#MotherTagNo option:selected").text() == "Please select" || $("#SireTagNo option:selected").text() == "Please select" || $("#Difficult option:selected").text() == "Please select" || TopResult == true) {

                alert("No Good, Natural attempt");
                event.preventDefault();
            }
        }
        else {

            if ($("#Sex option:selected").text() == "Please select" || $("#Breed option:selected").text() == "Please select" || $("#AICow option:selected").text() == "Please select" || $("#Difficult option:selected").text() == "Please select" || TopResult == true) {

                alert("No Good, AI Attempt");
                event.preventDefault();
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