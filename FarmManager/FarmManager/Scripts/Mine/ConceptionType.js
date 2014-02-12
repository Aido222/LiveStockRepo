jQuery(document).ready(function () {
    $("[name='InseminationType']").change(function () {
        var selectedValue = $('input[name=InseminationType]:checked').val()
        alert(selectedValue);

        if (selectedValue == 0)
        {
            $(".Natural").slideDown("slow");
            //$(this).addClass("current");
            
        }
        else 
        {

        }
    });
});