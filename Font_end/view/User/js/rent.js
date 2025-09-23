let slideIndex = 0;
const slides = document.querySelector(".slide_img");
const images = document.querySelectorAll(".slide_img img");
const totalSlides = images.length;

function updateSlide() {
  slides.style.transform = `translateX(-${slideIndex * 100}%)`;
  // reset animation for text
  const slideText = document.querySelector(".slide_text");
  slideText.style.animation = "none";
  slideText.offsetHeight; // trigger reflow
  slideText.style.animation = "fadeInUp 1s ease";
}

// Next button
document.querySelector(".next").addEventListener("click", () => {
  slideIndex = (slideIndex + 1) % totalSlides;
  updateSlide();
});

// Prev button
document.querySelector(".prev").addEventListener("click", () => {
  slideIndex = (slideIndex - 1 + totalSlides) % totalSlides;
  updateSlide();
});

// Auto slide every 5s
setInterval(() => {
  slideIndex = (slideIndex + 1) % totalSlides;
  updateSlide();
}, 5000);

// Rent button click event
document.querySelectorAll(".rent").forEach(btn => {
  btn.addEventListener("click", () => {
    alert("📅 Booking feature coming soon!");
  });
});
