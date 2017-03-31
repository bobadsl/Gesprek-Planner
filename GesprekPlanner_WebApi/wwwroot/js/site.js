$(document).on("click",
    "#TriggerLogout",
    function() {
        $("#logoutForm").submit();
    });

$(document).on("click",
    ".GroupToggler",
    function () {
        $(this).parent().parent().parent().find(".GroupToggleThis").each(function () {
            $(this).toggle();
        });
        if ($(this).hasClass("fa-arrow-down"))
            $(this).attr("class", "fa fa-arrow-up GroupToggler");
        else
            $(this).attr("class", "fa fa-arrow-down GroupToggler");
        //                    $(this).parent().parent().parent().parent().find(".GroupToggleThis").each(function() {
        //                        $(this).hide();
        //                    });
        //                    $(this).parent().parent().parent().parent().find(".GroupToggler").each(function () {
        //                        $(this).attr("class", "fa fa-arrow-down GroupToggler");
        //                    });
        //                    $(this).parent().parent().parent().find(".GroupToggleThis").each(function() {
        //                        $(this).show();
        //                    });
        //                    $(this).attr("class", "fa fa-arrow-up GroupToggler");
    });