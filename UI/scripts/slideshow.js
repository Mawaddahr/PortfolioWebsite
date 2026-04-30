// slideshow.js
let slideIndex = 1;

function showSlides(n) {
    const slides = document.getElementsByClassName("slides");
    if (!slides || slides.length === 0) return;

    if (n > slides.length) { slideIndex = 1; }
    if (n < 1) { slideIndex = slides.length; }

    for (let i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";
    }
    slides[slideIndex - 1].style.display = "block";
}

// Make plusSlides available to inline onclick handlers
window.plusSlides = function (n) {
    showSlides(slideIndex += n);
};

// Initialize after DOM is ready (works with or without `defer`)
document.addEventListener('DOMContentLoaded', function () {
    showSlides(slideIndex);
});