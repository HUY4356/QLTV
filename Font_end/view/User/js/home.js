document.addEventListener("DOMContentLoaded", () => {
  /* ==========================
     1️⃣ SLIDER ẢNH TRANG HOME
     ========================== */
  const swiper = document.querySelector(".home_swiper");
  const slides = document.querySelectorAll(".home_article");
  let index = 0;

  function showSlide(i) {
    swiper.style.transform = `translateX(-${i * 100}%)`;
    swiper.style.transition = "transform 1s ease";
  }

  if (swiper && slides.length > 0) {
    setInterval(() => {
      index = (index + 1) % slides.length;
      showSlide(index);
    }, 3000); // đổi ảnh mỗi 3 giây
  }

  /* ==========================
     2️⃣ FEATURED SECTION LOOP
     ========================== */
  const track = document.querySelector(".featured_track");
  if (track) {
    track.innerHTML += track.innerHTML; // nhân đôi item để loop
  }

  /* ==========================
     3️⃣ RANKING SECTION (TOP 5)
     ========================== */

  // Mảng lưu trữ thông tin sách
  const books = [
    {
      img: "img/bia_sach_1.jpg",
      title: "Tên Sách 1",
      description: "Mô tả sách top 1 — tác phẩm nổi bật nhất tuần."
    },
    {
      img: "img/bia_sach_2.jpg",
      title: "Tên Sách 2",
      description: "Sách top 2 — hành trình khám phá tri thức và cảm xúc."
    },
    {
      img: "img/bia_sach_3.jpg",
      title: "Tên Sách 3",
      description: "Tác phẩm nhẹ nhàng, sâu lắng về tình người và thời gian."
    },
    {
      img: "img/bia_sach_4.jpg",
      title: "Tên Sách 4",
      description: "Sách top 4 — mang hơi thở hiện đại, trẻ trung và sáng tạo."
    },
    {
      img: "img/bia_sach_5.jpg",
      title: "Tên Sách 5",
      description: "Sách top 5 — câu chuyện truyền cảm hứng và nhân văn."
    }
  ];

  // DOM phần chi tiết bên trái
  const bookImg = document.getElementById("book-img");
  const bookTitle = document.getElementById("book-title");
  const bookDesc = document.getElementById("book-description");
  const rankLeft = document.querySelector(".rank-left");

  // DOM danh sách top 5 bên phải
  const rankCards = document.querySelectorAll(".rank-card");

  // Hàm hiển thị chi tiết sách
  function showDetails(index) {
    if (!books[index]) return;
    const book = books[index];
    bookImg.src = book.img;
    bookTitle.textContent = book.title;
    bookDesc.textContent = book.description;

    // Hiệu ứng đổi màu bên trái (sáng nhẹ)
    rankLeft.classList.add("active");
    setTimeout(() => rankLeft.classList.remove("active"), 200);

    // Cập nhật active bên phải
    rankCards.forEach(c => c.classList.remove("active"));
    if (rankCards[index]) rankCards[index].classList.add("active");
  }

  // Gán sự kiện click cho từng sách top 5
  rankCards.forEach((card, i) => {
    card.addEventListener("click", () => showDetails(i));
  });

  // Hiển thị mặc định sách top 1
  showDetails(0);
});
