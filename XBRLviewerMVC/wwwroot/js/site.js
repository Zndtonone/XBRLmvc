﻿// Custom JS XBRLviewer

function on() {
    document.getElementById("overlay").style.display = "block";
}

function off() {
    document.getElementById("overlay").style.display = "none";
}

// Script that shows the tooltip when you hover over a text
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

// Add style to p tags
$(document).ready(function () {
    $("p").css("white-space", "pre-wrap");
});

$(function () {
    $("#menu-toggle").click(function (e) {
        e.preventDefault();
        $("#wrapper").toggleClass("toggled");
    });

    $(window).resize(function (e) {
        if ($(window).width() <= 768) {
            $("#wrapper").removeClass("toggled");
        } else {
            $("#wrapper").addClass("toggled");
        }
    });
});