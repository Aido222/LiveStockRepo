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



    $('#btnDeleteNote').click(function () {
        var answer = confirm("Are you sure you want to delete this note?");
        if (answer) {
            return true;
        } else {
            return false;
        }
    });
});

