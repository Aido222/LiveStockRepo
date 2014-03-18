jQuery(document).ready(function () {

    var d = new Date();
    var curr_date = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();

    var fullYear = curr_month + "-" + curr_date + "-" + curr_year;

    var pageText = $('#withDrawalEnd').text();
    var day = pageText.substring(0, 2);
    var month = pageText.substring(3, 5);
    var year = pageText.substring(6, 10);


    var withDrawaldate = new Date(month + "-" + day + "-" + year);
    var todaysDate = new Date(fullYear);

    //alert(withDrawaldate);

    //alert(todaysDate);
    
    if (Date.parse(withDrawaldate) > Date.parse(todaysDate)) {
        $("#withdrawalWarn").slideDown("slow");
    }
    

});

