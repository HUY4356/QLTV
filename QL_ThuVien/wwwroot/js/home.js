document.addEventListener("DOMContentLoaded", () => {
    const swiper = document.querySelector(".home_swiper");
    const slides = document.querySelectorAll(".home_article");
    let index = 0;

    function showSlide(i) {
        swiper.style.transform = `translateX(-${i * 100}%)`;
    }

    setInterval(() => {
        index = (index + 1) % slides.length;
        showSlide(index);
    }, 3000); // đổi ảnh mỗi 3 giây
});

document.addEventListener("DOMContentLoaded", () => {
    const track = document.querySelector(".featured_track");
    track.innerHTML += track.innerHTML; // nhân đôi item để loop
});