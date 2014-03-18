jQuery(document).ready(function () {


    var TopResult;
    var DateOkCheck;

    //Date picker...

    $('.datepicker').datepicker({

        format: 'yyyy/mm/dd'
    });

    $('#TreatmentDate').datepicker()
   .on('changeDate', function (ev) {
       if (ev.date.valueOf() > new Date()) {
           $("#greaterDateWarn").slideDown("slow");

           DateOkCheck = false;
       }
       else {
           $("#greaterDateWarn").slideUp("slow");

           DateOkCheck = true;
       }
   });

    
    $("#submitBtn").click(function () {



        if (DateOkCheck == false) {
            event.preventDefault();
        }

        if ($("#TreatmentDate").val() == "Please select") {
            $("#enterDateWarn").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#enterDateWarn").slideUp("slow");

        }


        if ($("#medicineUsedText").val() == "") {
            $("#enterMedicineWarn").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#enterMedicineWarn").slideUp("slow");

        }


        if ($("#DosageAmount").val() == "") {
            $("#enterDosageWarn").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#enterDosageWarn").slideUp("slow");

        }




        var txtToCheck = $("#DosageAmount").val()
        var ans = isNumber(txtToCheck);

        if (ans == false) {
            $("#enterDosageWarn").slideDown("slow");

            event.preventDefault();

        }
        else {
            $("#enterDosageWarn").slideUp("slow");

        }


        
    }); 



    $(".ViewMed").click(function () {

        var medID = $(this).attr('data-id');


        $.post("/Treatment/RetrieveMedicine", { MedID: medID },
            function (data) {

               

                $("#overlay_form").fadeIn(1000);
                positionPopup();



                var date = data[1];


                
                var subdate = data[5].substring(0, 10);

                
                $('#modalHead').text(data[0] + "(" + data[3] + ")");
                $('#mainUse').text(data[1]);
                $('#withPeriod').text(data[2] + " days");
                $('#batchNo').text(data[3]);
                $('#bottleSize').text(data[4]);
                $('#DOP').text(subdate);
                $('#suppliedBy').text(data[6]);

            
                $("#btnDeleteNote").attr("href", "../DeleteNote/" + data[3]);
                $("#btnEditNote").attr("href", "../EditNote/" + data[3]);

            });

  
    });



    function positionPopup() {
        if (!$("#overlay_form").is(':visible')) {
            return;
        }
        $("#overlay_form").css({
            left: ($(window).width() - $('#overlay_form').width()) / 2,
            top: ($(window).width() - $('#overlay_form').width()) / 7,
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



    $(".Select").click(function () {

        var medID = $(this).attr('id');
        var medText = $(this).text();

        //alert(medText);

        $('#medicineUsedText').val(medText); 
        $('#UserMedicineID').val(medID);
        //medicineUsedText
    });


    function isNumber(n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }

});


