// header.js
function initHeaderScripts() {
  /* =====================================================
     🔍 SEARCH BUTTON (CHO MOBILE)
  ===================================================== */
  const searchBtn = document.querySelector(".search-btn");
  const searchBox = document.querySelector(".header-search");

  if (searchBtn && searchBox) {
    searchBtn.addEventListener("click", (e) => {
      if (window.innerWidth <= 768) {
        e.preventDefault(); // Ngăn submit form khi bấm icon
        searchBox.classList.toggle("active");
      }
    });
  }

  /* =====================================================
     📚 CATEGORY LEFT MENU INTERACTION
  ===================================================== */
  const leftItems = document.querySelectorAll(".category-left li");
  const subcategories = document.querySelectorAll(".subcategory");

  if (leftItems.length > 0 && subcategories.length > 0) {
    // Mặc định: hiển thị danh mục đầu tiên
    leftItems[0].classList.add("active");
    const firstTarget = leftItems[0].getAttribute("data-target");
    if (firstTarget) {
      document.getElementById(firstTarget)?.classList.add("active");
    }

    // Khi hover hoặc click danh mục bên trái
    leftItems.forEach((item) => {
      const activate = () => {
        const targetId = item.getAttribute("data-target");

        leftItems.forEach((li) => li.classList.remove("active"));
        subcategories.forEach((sub) => sub.classList.remove("active"));

        item.classList.add("active");
        const targetSub = document.getElementById(targetId);
        if (targetSub) targetSub.classList.add("active");
      };

      item.addEventListener("mouseenter", activate);
      item.addEventListener("click", (e) => {
        e.preventDefault();
        activate();
      });
    });
  }
}

/* =====================================================
   🧠 Đảm bảo script khởi chạy đúng lúc
   (vì header được load động qua fetch trong index.js)
===================================================== */
if (document.readyState === "loading") {
  document.addEventListener("DOMContentLoaded", initHeaderScripts);
} else {
  initHeaderScripts();
}
