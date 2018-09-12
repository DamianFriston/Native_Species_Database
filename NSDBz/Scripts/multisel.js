$('#btnRight').click(function (e) {
    var selectedOpts = $('#selectedCountries option:selected');
    if (selectedOpts.length == 0) {
        alert("Please select a Country.");
        e.preventDefault();
    }

    $('#availCountries').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#btnLeft').click(function (e) {
    var selectedOpts = $('#availCountries option:selected');
    if (selectedOpts.length == 0) {
        alert("Please select a Country.");
        e.preventDefault();
    }

    $('#selectedCountries').append($(selectedOpts).clone());
    $(selectedOpts).remove();
    e.preventDefault();
});

$('#btnSubmit').click(function (e) {
    $('#selectedCountries option').prop('selected', true);
});
