jQuery(document).ready(function () {

    var TopResult;
    var DateOkCheck;

    //Date picker...

    $('.datepicker').datepicker({

        format: 'yyyy/mm/dd'
    });

    $('#DateSold').datepicker()
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
  

        




        if (DateOkCheck == false) {
            event.preventDefault();

        }





        if ($("#DateSold").val() == "Please select") {
            $("#selectDateWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectDateWarning").slideUp("slow");

        }



        if ($("#LocationSold").val() == "") {
            $("#selectLocationWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectLocationWarning").slideUp("slow");

        } 

        if ($("#SoldTo").val() == "") {
            $("#selectBuyerWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#selectBuyerWarning").slideUp("slow");

        }


       



    });


});