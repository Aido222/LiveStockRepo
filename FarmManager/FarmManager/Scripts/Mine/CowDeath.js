jQuery(document).ready(function () {


    var TopResult;
    var DateOkCheck;

    //Date picker...

    $('.datepicker').datepicker({

        format: 'yyyy/mm/dd'
    });

    $('#DOD').datepicker()
   .on('changeDate', function (ev) {
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



      

        if (DateOkCheck == false) {
            event.preventDefault();

        }



      

        if ($("#DOD").val() == "Please select") {
            $("#selectDateWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectDateWarning").slideUp("slow");

        }



        if ($("#Cause option:selected").text() == "Please select") {
            $("#selectCauseWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectCauseWarning").slideUp("slow");

        }





        
    });


    $("[name='InseminationType']").change(function () {
        var selectedValue = $('input[name=InseminationType]:checked').val()

        $(".expandingForm").slideDown("slow");

        if (selectedValue == 0) {
            $(".Natural").slideDown("slow");
            $(".AI").slideUp("slow");
            $('.hTop').text('New Calf Birth By Bull');

            //$(this).addClass("current");

        }
        else {
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