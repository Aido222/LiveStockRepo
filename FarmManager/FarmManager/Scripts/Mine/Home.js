jQuery(document).ready(function () {

    $.post("/Home/DeathData", {},
function (data) {


    var pieData = [
				{
				    value: parseInt(data[0]),
				    color: "#F38630"
				},
				{
				    value: parseInt(data[1]),
				    color: "#E0E4CC"
				},
				{
				    value: parseInt(data[2]),
				    color: "#69D2E7"
				},
                {
                    value: parseInt(data[3]),
                    color: "#9E0900"
                },
                {
                    value: parseInt(data[4]),
                    color: "#000FA7"
                },
                {
                    value: parseInt(data[5]),
                    color: "#FAFF00"
                },


    ];


    $("#li1").append(" (" + data[0] + ")");
    $("#li2").append(" (" + data[1] + ")");
    $("#li3").append(" (" + data[2] + ")");
    $("#li4").append(" (" + data[3] + ")");
    $("#li5").append(" (" + data[4] + ")");
    $("#li6").append(" (" + data[5] + ")");

    var myPie = new Chart(document.getElementById("canvas3").getContext("2d")).Pie(pieData);

});


    $.post("/Home/MotherNumbers", {},
       function (data) {



           
           var barChartData = {
               labels: [data[0].MotherTagNo, data[1].MotherTagNo, data[2].MotherTagNo, data[3].MotherTagNo, data[4].MotherTagNo],
               datasets: [
                   {
                       fillColor: "rgba(220,220,220,0.5)",
                       strokeColor: "rgba(220,220,220,1)",
                       data: [data[0].Count, data[1].Count, data[2].Count, data[3].Count, data[4].Count]
                   }
               ]

           }


           barOptions = {

               scaleOverride: true,

               scaleSteps: 10,
               //Number - The value jump in the hard coded scale
               scaleStepWidth: 1,
               //Number - The scale starting value
               scaleStartValue: 0,
           }

           var myLine = new Chart(document.getElementById("canvas4").getContext("2d")).Bar(barChartData, barOptions);

       });


    $.post("/Home/PurchData", {},
           function (data) {


               







    var lineChartData = {

        labels: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
        datasets: [
            {
                fillColor: "rgba(97,139,255,0.5)",
                strokeColor: "rgba(97,139,255,1)",
                pointColor: "rgba(255,255,255,1)",

                data: [data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11]]
            }
        ]




    }


    lineOptions = {

        scaleOverride: true,

        //** Required if scaleOverride is true **
        //Number - The number of steps in a hard coded scale
        scaleSteps: 15,
        //Number - The value jump in the hard coded scale
        scaleStepWidth: 500,
        //Number - The scale starting value
        scaleStartValue: 0,
        scaleGridLineColor: "rgba(3, 69, 252, 0.1)",
        bezierCurve: false,
        scaleFontColor: "#0059ae",
        pointStrokeColor: "#0059ae",

    }


    var myLine = new Chart(document.getElementById("canvas2").getContext("2d")).Line(lineChartData, lineOptions);










           });





    $.post("/Home/SaleData", {},
       function (data) {


           var lineChartData = {

               labels: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
               datasets: [
                   {
                       fillColor: "rgba(97,139,255,0.5)",
                       strokeColor: "rgba(97,139,255,1)",
                       pointColor: "rgba(255,255,255,1)",

                       data: [data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11]]
                   }
               ]




           }


           lineOptions = {

               scaleOverride: true,

               //** Required if scaleOverride is true **
               //Number - The number of steps in a hard coded scale
               scaleSteps: 15,
               //Number - The value jump in the hard coded scale
               scaleStepWidth: 500,
               //Number - The scale starting value
               scaleStartValue: 0,
               scaleGridLineColor: "rgba(3, 69, 252, 0.1)",
               bezierCurve: false,
               scaleFontColor: "#0059ae",
               pointStrokeColor: "#0059ae",

           }


           var myLine = new Chart(document.getElementById("canvas").getContext("2d")).Line(lineChartData, lineOptions);


       });







});