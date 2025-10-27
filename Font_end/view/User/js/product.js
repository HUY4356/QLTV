// Lấy các phần tử DOM
const sliderTrack = document.getElementById("slider_track");
const slides = document.querySelectorAll(".slide");
const nextBtn = document.getElementById("next_btn");
const prevBtn = document.getElementById("prev_btn");

let currentIndex = 0;
const totalSlides = slides.length;

// Cập nhật vị trí slider
function updateSlider() {
  sliderTrack.style.transform = `translateX(-${currentIndex * 100}%)`;
}

// Nút kế tiếp
nextBtn.addEventListener("click", () => {
  currentIndex = (currentIndex + 1) % totalSlides;
  updateSlider();
});

// Nút lùi lại
prevBtn.addEventListener("click", () => {
  currentIndex = (currentIndex - 1 + totalSlides) % totalSlides;
  updateSlider();
});

// ✅ Tuỳ chọn: Tự động chuyển slide sau 4s
let autoSlide = setInterval(() => {
  nextBtn.click();
}, 4000);

// Dừng tự động khi rê chuột vào slider
const sliderBox = document.querySelector(".book_image_slider");
sliderBox.addEventListener("mouseenter", () => clearInterval(autoSlide));
sliderBox.addEventListener("mouseleave", () => {
  autoSlide = setInterval(() => nextBtn.click(), 4000);
});
