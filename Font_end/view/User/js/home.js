document.addEventListener("DOMContentLoaded", () => {
  /* =====================================================
     ⚙️ TIỆN ÍCH CHUNG
  ===================================================== */
  const $ = (sel, all = false) =>
    all ? document.querySelectorAll(sel) : document.querySelector(sel);

  /* =====================================================
     2️⃣ FEATURED SECTION LOOP
  ===================================================== */
  (() => {
    const track = $(".featured_track");
    if (track && !track.dataset.duplicated) {
      track.innerHTML += track.innerHTML;
      track.dataset.duplicated = "true";
    }
  })();

  /* =====================================================
     4️⃣ CATEGORY SECTION
  ===================================================== */
  (() => {
    const leftItems = document.querySelectorAll(".category-left li");
    const subcategories = document.querySelectorAll(".subcategory");

    if (leftItems.length === 0 || subcategories.length === 0) return;

    // Mặc định: hiển thị phần đầu tiên
    subcategories.forEach((sub, i) => {
      if (i === 0) sub.classList.add("active");
      else sub.classList.remove("active");
    });
    leftItems[0].classList.add("active");

    // Khi click vào danh mục bên trái
    leftItems.forEach((item) => {
      item.addEventListener("click", () => {
        const targetId = item.getAttribute("data-target");

        // Reset trạng thái
        leftItems.forEach((li) => li.classList.remove("active"));
        subcategories.forEach((sub) => sub.classList.remove("active"));

        // Kích hoạt danh mục được chọn
        item.classList.add("active");
        const targetSub = document.getElementById(targetId);
        if (targetSub) targetSub.classList.add("active");
      });
    });
  })();

  /* =====================================================
     5️⃣ REVIEW FORM
  ===================================================== */
  (() => {
    const form = document.getElementById("reviewForm");
    if (!form) return;

    form.addEventListener("submit", (e) => {
      e.preventDefault();

      // Lấy dữ liệu
      const name = document.getElementById("nameInput").value.trim();
      const text = document.getElementById("textInput").value.trim();
      const rating =
        document.querySelector('input[name="rating"]:checked')?.value || 0;

      const stars = "★".repeat(rating) + "☆".repeat(5 - rating);

      const formCard = document.querySelector(".review_form_card");
      if (!formCard) return;

      // Đổi giao diện
      formCard.classList.remove("review_form_card");
      formCard.classList.add("review_card");

      formCard.innerHTML = `
        <div class="review_user">
          <img src="img/default_user.jpg" alt="${name}">
          <h4>${name}</h4>
        </div>
        <div class="review_stars">${stars}</div>
        <p>${text}</p>
      `;
    });
  })();
});

// FORM ĐẶT PHÒNG
document.addEventListener("DOMContentLoaded", () => {
  const bookingForm = document.getElementById("bookingForm");
  if (!bookingForm) return;

  bookingForm.addEventListener("submit", (e) => {
    e.preventDefault();
    const roomType = document.getElementById("roomType").value;
    const date = document.getElementById("date").value;
    const session = document.getElementById("session").value;

    if (!roomType || !date || !session) {
      alert("⚠️ Vui lòng điền đầy đủ thông tin!");
      return;
    }

    alert(`✅ Đặt phòng thành công!\n\nPhòng: ${roomType}\nNgày: ${date}\nBuổi: ${session}`);
    bookingForm.reset();
  });
});

// Khi trang tải xong
document.addEventListener("DOMContentLoaded", () => {
  const bookButtons = document.querySelectorAll(".room_card_btn");
  const roomSelect = document.getElementById("roomType");
  const bookingSection = document.querySelector(".booking_section_room");
  const bookingForm = document.querySelector(".booking_room_form");

  bookButtons.forEach(button => {
    button.addEventListener("click", (e) => {
      e.preventDefault();

      const roomValue = button.getAttribute("data-room");

      // Cuộn xuống phần đặt phòng
      bookingSection.scrollIntoView({ behavior: "smooth" });

      // Gán giá trị tương ứng cho select
      if (roomSelect) {
        roomSelect.value = roomValue;
      }

      // Thêm hiệu ứng highlight
      bookingForm.classList.add("highlight-form");
      setTimeout(() => bookingForm.classList.remove("highlight-form"), 1500);
    });
  });
});


