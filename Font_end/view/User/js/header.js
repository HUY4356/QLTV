document.addEventListener("DOMContentLoaded", () => {
  const searchBtn = document.querySelector(".search-btn");
  const searchBox = document.querySelector(".header-search");

  searchBtn.addEventListener("click", (e) => {
    // Ngăn submit khi bấm icon
    if (window.innerWidth <= 768) {
      e.preventDefault();
      searchBox.classList.toggle("active");
    }
  });
});

