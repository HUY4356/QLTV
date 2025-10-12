// ===================== SLIDE SHOW =====================
const slide = document.querySelector(".slide_img");
const images = document.querySelectorAll(".slide_img img");
const prevBtn = document.querySelector(".prev");
const nextBtn = document.querySelector(".next");

let currentIndex = 0;
const totalSlides = images.length;

// Hàm hiển thị slide theo chỉ số
function showSlide(index) {
  if (index < 0) {
    currentIndex = totalSlides - 1;
  } else if (index >= totalSlides) {
    currentIndex = 0;
  } else {
    currentIndex = index;
  }

  slide.style.transform = `translateX(-${currentIndex * 100}%)`;
}

// Nút điều hướng
nextBtn.addEventListener("click", () => {
  showSlide(currentIndex + 1);
});

prevBtn.addEventListener("click", () => {
  showSlide(currentIndex - 1);
});

// Tự động chuyển ảnh mỗi 5 giây
setInterval(() => {
  showSlide(currentIndex + 1);
}, 5000);

