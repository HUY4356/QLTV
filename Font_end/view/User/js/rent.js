// Lấy tất cả nút rent
const rentButtons = document.querySelectorAll('.rent');

// Lấy modal
const modal = document.getElementById('rentModal');

// Lấy nút đóng
const closeBtn = modal.querySelector('.close');

// Lấy tất cả nút phòng trong modal
const roomBtns = modal.querySelectorAll('.room_btn');

// Mở modal khi bấm nút rent
rentButtons.forEach(button => {
  button.addEventListener('click', () => {
    // Lấy tên phòng từ card chứa nút
    const roomName = button.closest('.room').querySelector('.info').childNodes[0].textContent.trim();

    // Bật modal
    modal.classList.add('show');

    // Chọn phòng tương ứng trong modal
    roomBtns.forEach(btn => {
      if(btn.textContent.trim() === roomName) {
        btn.style.backgroundColor = '#7e6e59'; // màu nổi bật
      } else {
        btn.style.backgroundColor = '#A79277'; // màu mặc định
      }
    });
  });
});

// Đóng modal khi bấm nút X
closeBtn.addEventListener('click', () => {
  modal.classList.remove('show');
});

// Đóng modal khi bấm ra ngoài modal content
window.addEventListener('click', (e) => {
  if (e.target === modal) {
    modal.classList.remove('show');
  }
});