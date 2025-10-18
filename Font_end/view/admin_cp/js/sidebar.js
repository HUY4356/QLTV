    // --- Tìm kiếm trong sidebar ---
    const searchInput = document.getElementById("searchInput");
    const items = document.querySelectorAll("#menuList li");

    searchInput.addEventListener("keyup", () => {
      const keyword = searchInput.value.toLowerCase();
      items.forEach(li => {
        const text = li.textContent.toLowerCase();
        li.style.display = text.includes(keyword) ? "block" : "none";
      });
    });

    // --- Gán class active khi click ---
    const links = document.querySelectorAll(".khung ul li a");

    links.forEach(link => {
      link.addEventListener("click", function() {
        links.forEach(l => l.classList.remove("active"));
        this.classList.add("active");
        localStorage.setItem("activeMenu", this.href);
      });
    });

    // --- Giữ lại mục active khi load lại trang ---
    const savedActive = localStorage.getItem("activeMenu");
    if (savedActive) {
      links.forEach(link => {
        if (link.href === savedActive) {
          link.classList.add("active");
        }
      });
    }