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

document.addEventListener("DOMContentLoaded", () => {
  const leftItems = document.querySelectorAll(".category-left li");
  const subcategories = document.querySelectorAll(".subcategory");

  leftItems.forEach(item => {
    item.addEventListener("mouseenter", () => {
      // Reset active
      leftItems.forEach(li => li.classList.remove("active"));
      subcategories.forEach(sub => sub.classList.remove("active"));

      // Thêm active mới
      item.classList.add("active");
      const id = item.getAttribute("data-category");
      if (id) {
        document.getElementById(id).classList.add("active");
      }
    });
  });

  // Khi load trang tự mở submenu đầu tiên
  if (leftItems[0]) {
    leftItems[0].classList.add("active");
    const id = leftItems[0].getAttribute("data-category");
    if (id) {
      document.getElementById(id).classList.add("active");
    }
  }
});

