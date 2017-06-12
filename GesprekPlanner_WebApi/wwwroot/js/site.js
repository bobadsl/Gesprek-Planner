$(document).on("click",
    "#TriggerLogout",
    function() {
        $("#logoutForm").submit();
    });

$(document).on("click",
    ".GroupToggler",
    function () {
        if ($(this).hasClass("multiToggle")) {
            $(this).parent().parent().parent().find(".GroupToggleThis").each(function () {
                $(this).toggle();
            });
            if ($(this).hasClass("fa-arrow-down")) {
                $(this).removeClass("fa-arrow-down");
                $(this).addClass("fa-arrow-up");
            } else {
                $(this).removeClass("fa-arrow-up");
                $(this).addClass("fa-arrow-down");
            }
        }
        else if ($(this).hasClass("singleToggle")) {

            if ($(this).hasClass("fa-arrow-up")) {
                $(this).parent().parent().parent().find(".GroupToggleThis").each(function() {
                    $(this).hide();
                });
                $(this).removeClass("fa-arrow-up");
                $(this).addClass("fa-arrow-down");
                return;
            }
            $(this).parent().parent().parent().parent().find(".GroupToggleThis").each(function() {
                $(this).hide();
            });
            $(this).parent().parent().parent().parent().find(".GroupToggler").each(function () {
                $(this).removeClass("fa-arrow-up");
                $(this).addClass("fa-arrow-down");
            });
            $(this).parent().parent().parent().find(".GroupToggleThis").each(function() {
                $(this).show();
            });
            $(this).removeClass("fa-arrow-down");
            $(this).addClass("fa-arrow-up");
        }
    });