
  const slides = document.querySelectorAll(".slide");
  const thumb = document.getElementById("thumb");
  let index = 0;

  function showNextSlide() {
    slides[index].classList.remove("active");
    index = (index + 1) % slides.length;
    slides[index].classList.add("active");

    // đổi ảnh bìa theo slide hiện tại
    const img = slides[index].querySelector("img").src;
    thumb.src = img;
  }

  setInterval(showNextSlide, 3000);

