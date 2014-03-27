jQuery(document).ready(function () {

    var DateOkCheck;
    var DateOkCheck2;


    $('.datepicker').datepicker({

        format: 'yyyy/mm/dd'
    });

    $('#ExpiryDate').datepicker()
   .on('changeDate', function (ev) {
       if (ev.date.valueOf() < new Date()) {
           $("#greaterDateWarning").slideDown("slow");

           DateOkCheck = false;
       }
       else {
           $("#greaterDateWarning").slideUp("slow");

           DateOkCheck = true;
       }
   });


    $('#DateOfPurchase').datepicker()
.on('changeDate', function (ev) {
    if (ev.date.valueOf() > new Date()) {
        $("#purchgreaterDateWarning").slideDown("slow");

        DateOkCheck2 = false;
    }
    else {
        $("#purchgreaterDateWarning").slideUp("slow");

        DateOkCheck2 = true;
    }
});



    $("#btnSubmit").click(function () {





        if ($("#BatchNo").val() == "") {
            $("#enterBatchWarn").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#enterBatchWarn").slideUp("slow");

        }


        if ($("#ExpiryDate").val() == "Please select") {
            $("#enterDateWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#enterDateWarning").slideUp("slow");

        }




        if ($("#BottleSize").val() == "") {
            $("#enterBottleWarn").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#enterBottleWarn").slideUp("slow");

        }


        var txtToCheck = $("#BottleSize").val()
        var ans = isNumber(txtToCheck);


        if (ans == false) {
            $("#mlOnlyWarn").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#mlOnlyWarn").slideUp("slow");

        }



        if ($("#SuppliedBy").val() == "") {
            $("#suppliedByWarn").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#suppliedByWarn").slideUp("slow");

        }



        if ($("#medicineName").val() == "") {
            $("#enterMedWarn").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#enterMedWarn").slideUp("slow");

        }

        


        if ($("#DateOfPurchase").val() == "Please select") {
            $("#purchenterDateWarning").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#purchenterDateWarning").slideUp("slow");

        }





        function isNumber(n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }

    });



    $(".Select").click(function () {

        var medID = $(this).attr('id');
        var medText = $(this).text();

        //alert(medText);

        $('#medicineName').val(medText);
        $('#MedicineID').val(medID);
        //medicineUsedText
    });



});