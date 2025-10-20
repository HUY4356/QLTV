
  const totalPages = 3; // 👈 số trang bạn muốn hiển thị
  let currentPage = 1;

  const pageNumbers = document.getElementById("pageNumbers");
  const prevBtn = document.getElementById("prevBtn");
  const nextBtn = document.getElementById("nextBtn");

  // === TẠO NÚT SỐ TRANG ===
  function renderPageNumbers() {
    pageNumbers.innerHTML = ""; // xóa cũ
    for (let i = 1; i <= totalPages; i++) {
      const btn = document.createElement("button");
      btn.textContent = i;
      if (i === currentPage) btn.classList.add("active");
      btn.addEventListener("click", () => {
        currentPage = i;
        renderPageNumbers();
        console.log("Trang:", currentPage);
      });
      pageNumbers.appendChild(btn);
    }
  }

  // === NÚT TRƯỚC / SAU ===
  prevBtn.addEventListener("click", () => {
    if (currentPage > 1) {
      currentPage--;
      renderPageNumbers();
    }
  });

  nextBtn.addEventListener("click", () => {
    if (currentPage < totalPages) {
      currentPage++;
      renderPageNumbers();
    }
  });

  renderPageNumbers();


  