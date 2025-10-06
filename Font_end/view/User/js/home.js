document.addEventListener("DOMContentLoaded", () => {
  /* =====================================================
     ⚙️ TIỆN ÍCH CHUNG
  ===================================================== */
  const $ = (sel, all = false) =>
    all ? document.querySelectorAll(sel) : document.querySelector(sel);

  /* =====================================================
     1️⃣ SLIDER ẢNH TRANG HOME
  ===================================================== */
  (() => {
    const swiper = $(".home_swiper");
    const slides = $(".home_article", true);
    if (!swiper || slides.length === 0) return;

    let index = 0;
    const showSlide = (i) => {
      swiper.style.transform = `translateX(-${i * 100}%)`;
      swiper.style.transition = "transform 1s ease";
    };

    setInterval(() => {
      index = (index + 1) % slides.length;
      showSlide(index);
    }, 3000);
  })();

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
     3️⃣ RANKING SECTION (TOP 5)
  ===================================================== */
  (() => {
    const books = [
      {
        img: "../img/bia_sach_1.jpg",
        title: "Tên Sách 1",
        description: "Mô tả sách top 1 — tác phẩm nổi bật nhất tuần."
      },
      {
        img: "../img/bia_sach_2.jpg",
        title: "Tên Sách 2",
        description: "Sách top 2 — hành trình khám phá tri thức và cảm xúc."
      },
      {
        img: "../img/bia_sach_3.jpg",
        title: "Tên Sách 3",
        description: "Tác phẩm nhẹ nhàng, sâu lắng về tình người và thời gian."
      },
      {
        img: "../img/bia_sach_4.jpg",
        title: "Tên Sách 4",
        description: "Sách top 4 — mang hơi thở hiện đại, trẻ trung và sáng tạo."
      },
      {
        img: "../img/bia_sach_5.jpg",
        title: "Tên Sách 5",
        description: "Sách top 5 — câu chuyện truyền cảm hứng và nhân văn."
      }
    ];

    // DOM phần chi tiết bên trái
    const bookImg = $("#book-img");
    const bookTitle = $("#book-title");
    const bookDesc = $("#book-description");
    const rankLeft = $(".rank-left");
    const rankCards = $(".rank-card", true);

    if (!bookImg || !bookTitle || !bookDesc || rankCards.length === 0) {
      console.warn("⚠️ Không tìm thấy phần tử rank-section hoặc rank-card.");
      return;
    }

    // Hàm hiển thị chi tiết
    function showDetails(index) {
      const book = books[index];
      if (!book) return;

      // Thêm hiệu ứng fade
      rankLeft.classList.add("fade");
      setTimeout(() => {
        bookImg.src = book.img;
        bookTitle.textContent = book.title;
        bookDesc.textContent = book.description;
        rankLeft.classList.remove("fade");
      }, 200);

      // Cập nhật active bên phải
      rankCards.forEach((card) => card.classList.remove("active"));
      if (rankCards[index]) rankCards[index].classList.add("active");
    }

    // Gán sự kiện click cho mỗi sách
    rankCards.forEach((card, i) => {
      card.addEventListener("click", () => showDetails(i));
    });

    // Mặc định hiển thị sách top 1
    showDetails(0);
  })();
});
