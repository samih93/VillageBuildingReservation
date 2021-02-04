var DataTablesLang = {
    "lengthMenu": "عرض _MENU_ سجل في كل صفحة",
    "zeroRecords": "عذراً لا يوجد نتائج",
    "info": "عرض الصفحة رقم _PAGE_ من أصل _PAGES_",
    "infoEmpty": "لا يوجد نتائج",
    "infoFiltered": "(مصفاة من _MAX_ سجل)",
    "paginate": {
        "first": "الأول",
        "previous": "السابق",
        "next": "التالي",
        "last": "الأخير"
    },
    "sSearch": "",
    "sSearchPlaceholder": " بحث "
};

//$(document).ready(function () {
//    //hide by default
//    var tournamentContent = jQuery(".tournamentContent");
//    var missionOfficersWrapper = jQuery(".missionOfficersWrapper");
//    tournamentContent.hide();
//    if ($("#Mission_Istournament").is(":checked") == true) {
//        tournamentContent.fadeIn();
//        missionOfficersWrapper.fadeOut();
//    }
//    $('#Mission_Istournament').change(function () {
//        if (this.checked) {
//            tournamentContent.fadeIn();
//            missionOfficersWrapper.fadeOut();
//        } else {
//            tournamentContent.fadeOut();
//            missionOfficersWrapper.fadeIn();
//        }
//    });

//});


$(document).ready(function () {
    if ($(".datepicker").length > 0) {
        $(".datepicker").each(function () {
            var id = $(this).attr("id");

            $("#" + id).datepicker({
                dateFormat: "yy-mm-dd",
                changeYear: true,
                changeMonth: true,
                stepMonths: 12,
                yearRange: "2000:2060",
                showAnim: "fadeIn",
                language: 'pt-BR',
                showButtonPanel: true,
                currentText: 'اليوم',
                nextText: '>>',
                prevText: '<<',
                closeText: 'إغلاق',
                autoSize: false,
                buttonImage: "/images/datepicker.gif",
                monthNames: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"],
                monthNamesShort: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"],
                dayNamesMin: ["أحد", "إثنين", "ثلاثاء", "أربعاء", "خميس", "جمعة", "سبت"],
                firstDay: 1,
                beforeShow: function (input, inst) {
                    $(".ui-datepicker-month").addClass('attet');
                    $(".ui-datepicker-year").addClass('attet2');
                }
            });
        })
    }

    if ($(".datepicker").length > 0) {
        //$('.datepicker').datetimepicker({
        //    language: 'pt-BR'
        //});

    }
})

$(document).ready(function () {
    if ($(".material-timepicker").length > 0) {
        jQuery(".material-timepicker").bootstrapMaterialDatePicker(
            {
                format: "HH:mm",
                year: false,
                date: false,
                okText: "تأكيد",
                clearButton: false,
                switchOnClick: true,
                cancelText: "إلغاء"
            }
        );
    }


    if ($(".material-timepicker-date").length > 0) {
        jQuery(".material-timepicker-date").bootstrapMaterialDatePicker(
            {
                format: "YYYY-MM-DD",
                year: true,
                date: true,
                lang: "en",
                time: false,
                okText: "تأكيد",
                clearButton: true,
                cancelText: "إلغاء",
                clearText: "مسح",
                weekStart: 1,
                switchOnClick: true
            }
        )
    };
})


//checks if strDate is in between strFrom and strTo
function dateBetween(date, from, to) {
    return from <= date && date <= to;
}
function treatAsUTC(date) {
    var result = date;
    result.setMinutes(result.getMinutes() - result.getTimezoneOffset());
    result.setHours(0, 0, 0, 0);
    return result;
}
//returns int days between 2 dates
function daysBetween(startDate, endDate) {
    var millisecondsPerDay = 24 * 60 * 60 * 1000;
    return ((treatAsUTC(endDate) - treatAsUTC(startDate)) / millisecondsPerDay) + 1;
}



$(document).ready(function () {
    $(".btn-pref .btn").click(function () {
        $(".btn-pref .btn").removeClass("btn-primary").addClass("btn-default");
        // $(".tab").addClass("active"); // instead of this do the below 
        $(this).removeClass("btn-default").addClass("btn-primary");
    });
});

//dropdown with grouping and search

function selectOptionGroup(selectBoxID) {
    if ($(selectBoxID).length > 0) {
        var groups = {};
        $(selectBoxID + " option[optiongroup]").each(function () {
            groups[$.trim($(this).attr("optiongroup"))] = true;
        });
        $.each(groups, function (c) {
            $(selectBoxID + " option[optiongroup='" + c + "']").wrapAll('<optgroup label="' + c + '">');
        });
    }
}

$(document).ready(function () {
    selectOptionGroup(".ddl_Blocks");
});

$(document).ready(function () {
    if ($('.selectpicker').length > 0) {
        $('.selectpicker').selectpicker({
            liveSearch: true,
            showSubtext: true
        });
    }
});

$(document).ready(function () {
    if ($(".fancybox").length > 0) {
        $(".fancybox").fancybox();
    }
});



$(document).ready(function () {
    $("#chkReserveAll").change(function () {
        if (this.checked) {
            $('.selectpicker.ddl_Blocks').selectpicker('selectAll');
        } else {
            $('.selectpicker.ddl_Blocks').selectpicker('deselectAll');
        }
    });

});