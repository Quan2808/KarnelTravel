const element = document.querySelector('.dropdown-toggle');
const instance = new mdb.Dropdown(element);

let btnbtt = document.getElementById("btn-back-to-top");

window.onscroll = function () {
    scrollFunction();
};

document.addEventListener('DOMContentLoaded', function () {
    new mdb.Ripple(document.getElementById('btn-go-to-dashboard'));

    new mdb.Popover(document.getElementById('btn-go-to-dashboard'), {
        trigger: 'hover',
        content: 'Go to Dashboard'
    });
});

document.addEventListener('DOMContentLoaded', function () {
    var buttonElement = document.getElementById('btn-go-to-dashboard');

    buttonElement.addEventListener('click', function () {
        window.open('/admin', '_blank');
    });
});

document.addEventListener('DOMContentLoaded', function () {
    new mdb.Ripple(document.getElementById('btn-back-to-top'));

    new mdb.Popover(document.getElementById('btn-back-to-top'), {
        trigger: 'hover',
        content: 'Back to top'
    });
});

function scrollFunction() {
    if (
        document.body.scrollTop > 20 ||
        document.documentElement.scrollTop > 20
    ) {
        btnbtt.style.display = "block";
    } else {
        btnbtt.style.display = "none";
    }
}
btnbtt.addEventListener("click", backToTop);

function backToTop() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

function toggleIcon() {
    var icon = document.getElementById('toggleIcon');

    // Kiểm tra lớp của biểu tượng và thay đổi nó
    if (icon.classList.contains('fa-align-justify')) {
        icon.classList.remove('fa-align-justify');
        icon.classList.add('fa-x');
    } else {
        icon.classList.remove('fa-x');
        icon.classList.add('fa-align-justify');
    }
}

var myCarousel = document.querySelector('#introCarousel')
var carousel = new mdb.Carousel(myCarousel, {
    interval: 2,
    wrap: false
})

$(document).ready(function () {
    $(".toggle-collapse").on("click", function () {
        var target = $(this).data("target");

        // Close all other collapses
        $("[id^='_']").not(target).collapse("hide");

        // Toggle the target collapse
        $(target).collapse("toggle");
    });

    $("[id^='_']").on("hidden.bs.collapse", function () {
        var targetLink = $(".toggle-collapse[data-target='#" + this.id + "']");
        targetLink.removeClass("show");
    });

    // Event listener to handle shown.bs.collapse event
    $("[id^='_']").on("shown.bs.collapse", function () {
        // Add the "show" class to the toggle link when a collapse is shown
        var targetLink = $(".toggle-collapse[data-target='#" + this.id + "']");
        targetLink.addClass("show");
    });
});
