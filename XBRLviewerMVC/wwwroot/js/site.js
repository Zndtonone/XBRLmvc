// Custom JS XBRLviewer

function onOpenXBRL() {
    document.getElementById("overlayOpenXBRL").style.display = "block";
}

function offOpenXBRL() {
    document.getElementById("overlayOpenXBRL").style.display = "none";
}

// Script that shows the tooltip when you hover over a text
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

// Add style to p tags
$(document).ready(function () {
    $("p").css("white-space", "pre-wrap");
});

// Changes the file chooser text to the filename
$(function () {
    $("input:file").change(function () {
        var fileName = $(this).val().split("\\").pop();
        $(".custom-file-label").html(fileName);
    });
});

// Changes between + and - depending on if the sidebar is toggled or not
$(function () {
    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");

        if ($("#wrapper").hasClass("toggled")) {
            $("#menu-icon").removeClass("fa-plus");
            $("#menu-icon").addClass("fa-minus");
        } else {
            $("#menu-icon").removeClass("fa-minus");
            $("#menu-icon").addClass("fa-plus");
        }
    });
});