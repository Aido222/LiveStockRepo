jQuery(document).ready(function () {


    $(".noteLinks").click(function () {

        var noteID = $(this).attr('id');

        $.post("/Cows/RetrieveNote", { NoteID: noteID },
            function (data) {



                $("#overlay_form").fadeIn(1000);
                positionPopup();

                $('#modalHead').text(data[1]);
                $('#noteBody').text(data[0]);


                var date = data[2];


                var sub = date.substring(0, 10);

                $('#noteDate').text(sub);



                $("#btnDeleteNote").attr("href", "../DeleteNote/" + data[3]);
                $("#btnEditNote").attr("href", "../EditNote/" + data[3]);

            });

    });


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


    //close sale modal
    $("#closeSale").click(function () {
        $("#overlay_form_Sale").fadeOut(500);
    });

    $("#pop").click(function () {
        $("#overlay_form_Sale").fadeIn(1000);
        positionPopup();
    });
    $("#closeSale").click(function () {
        $("#overlay_form_Sale").fadeOut(500);
    });

    //close death
    $("#closeDeath").click(function () {
        $("#overlay_form_Death").fadeOut(500);
    });

    $("#pop").click(function () {
        $("#overlay_form_Death").fadeIn(1000);
        positionPopup();
    });
    $("#closeDeath").click(function () {
        $("#overlay_form_Death").fadeOut(500);
    });


    $("#closePurchase").click(function () {
        $("#overlay_form_Purchase").fadeOut(500);
    });

    $("#pop").click(function () {
        $("#overlay_form_Purchase").fadeIn(1000);
        positionPopup();
    });
    $("#closePurchase").click(function () {
        $("#overlay_form_Purchase").fadeOut(500);
    });


    $("#closeBirth").click(function () {
        $("#overlay_form_Birth").fadeOut(500);
    });

    $("#pop").click(function () {
        $("#overlay_form_Birth").fadeIn(1000);
        positionPopup();
    });
    $("#closeBirth").click(function () {
        $("#overlay_form_Birth").fadeOut(500);
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



    $('#btnDeleteNote').click(function () {
        var answer = confirm("Are you sure you want to delete this note?");
        if (answer) {
            return true;
        } else {
            return false;
        }
    });


    var statusCheck = $('#OwnershipStatus').val();
    var tagNo = $('#TagNo').val();


    if (statusCheck == 2) {

        $('#statusMessage').text("This animal has been ");
        // $("#statusLink").attr("href", "../SaleDetails/" +tagNo);

        $("#statusLink").text("Sold")


        $("#statusWarn").slideDown("slow");
    }
    else if (statusCheck == 3) {

        $('#statusMessage').text("This animal is ");
        //$("#statusLink").attr("href", "../DeathDetails/" + tagNo);

        $("#statusLink").text("Deceased")


        $("#statusWarn").slideDown("slow");
    }






    //retrieve sale data
    $("#statusLink").click(function () {

        var tagNo = $('#TagNo').val();
        var statusCheck = $('#OwnershipStatus').val();

        if (statusCheck == 2) {

            $.post("/Cows/RetrieveSale", { TagNo: tagNo },
                 function (data) {



                     $("#overlay_form_Sale").fadeIn(1000);
                     positionPopup();

                     $('#modalHeadSale').text("Sale of " + data[0]);
                     $('#modalTagNo').text(data[0]);
                     $('#modalLocationSold').text(data[1]);


                     var sub = data[2].substring(0, 10);


                     $('#dateSold').text(sub);
                     $('#soldTo').text(data[3]);
                     $('#saleNotes').text(data[4]);









                 });
        }
        else {



            $.post("/Cows/RetrieveDeath", { TagNo: tagNo },
                function (data) {
                    


                    $("#overlay_form_Death").fadeIn(1000);
                    positionPopup();

                    $('#modalHeadDeath').text("Death of " + data[0]);
                    $('#modalTagNoDeath').text(data[0]);

                    var sub = data[1].substring(0, 10);

                    $('#modalDOD').text(sub);

                    $('#deathCause').text(data[3]);

                    $('#deathNotes').text(data[2]);




                });


        }

    });



    if ($('#withDrawPara').is(':empty')) {

    }
    else {
        $("#withDrawWarn").slideDown("slow");
        
    }





    $("#purchLink").click(function () {

        var tagNo = $('#TagNo').val();

        $.post("/Cows/RetrieveCowPurch", { TagNo: tagNo },
           function (data) {



                $("#overlay_form_Purchase").fadeIn(1000);
                positionPopup();

               $('#modalHeadPurchase').text("Purchase details for " + data[0]);
               $('#modalTagNoPurchase').text(data[0]);
               $('#modalDOP').text(data[1].substring(0, 10));
               $('#purchasedFrom').text(data[5]);
               $('#pricePaid').text(data[4]);


               $('#purchNotes').text(data[2]);
               $('#purchasedLocation').text(data[3]);
               


               // $('#noteBody').text(data[0]);


               // var date = data[2];


               // var sub = date.substring(0, 10);






            });

    });





    //$("#bornLink").click(function () {

    //    var tagNo = $('#TagNo').val();

    //    //$.post("/Cows/RetrieveCowBirth", { TagNo: tagNo },
    //    //   function (data) {

  

    //           $("#overlay_form_Birth").fadeIn(1000);
    //           positionPopup();

    //           //$('#modalHeadPurchase').text("Purchase details for " + data[0]);
    //           //$('#modalTagNoPurchase').text(data[0]);
    //           //$('#modalDOP').text(data[1].substring(0, 10));
    //           //$('#purchasedFrom').text(data[5]);
    //           //$('#pricePaid').text(data[4]);


    //           //$('#purchNotes').text(data[2]);
    //           //$('#purchasedLocation').text(data[3]);



    //           // $('#noteBody').text(data[0]);


    //           // var date = data[2];


    //           // var sub = date.substring(0, 10);






    //       //});

    //});

});