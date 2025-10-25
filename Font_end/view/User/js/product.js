// ==== SLIDER IMAGE LOGIC ====
const track = document.getElementById("slider_track");
const slides = document.querySelectorAll(".slide");
const prevBtn = document.getElementById("prev_btn");
const nextBtn = document.getElementById("next_btn");

let index = 0;

function updateSlider() {
    track.style.transform = `translateX(-${index * 100}%)`;
}

nextBtn.addEventListener("click", () => {
    index = (index + 1) % slides.length;
    updateSlider();
});

prevBtn.addEventListener("click", () => {
    index = (index - 1 + slides.length) % slides.length;
    updateSlider();
});
