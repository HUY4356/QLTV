// Slider ảnh chạy vô tận, điều khiển bằng nút, tự động chuyển mỗi 5s

const sliderList = document.getElementById('slider-list');
const items = document.querySelectorAll('#slider-list .item');
const prevBtn = document.getElementById('prev');
const nextBtn = document.getElementById('next');
let currentIndex = 1; // Bắt đầu từ 1 vì sẽ clone thêm ảnh
const total = items.length;

// Clone ảnh đầu và cuối để tạo hiệu ứng vô tận
const firstClone = items[0].cloneNode(true);
const lastClone = items[total - 1].cloneNode(true);

sliderList.appendChild(firstClone);
sliderList.insertBefore(lastClone, items[0]);

const allItems = document.querySelectorAll('#slider-list .item');
const slideCount = allItems.length;

// Đặt chiều rộng cho slider-list
sliderList.style.width = `${slideCount * 100}%`;

// Hiển thị slide tại vị trí index
function showSlide(index, animate = true) {
    if (animate) {
        sliderList.style.transition = 'transform 0.5s ease';
    } else {
        sliderList.style.transition = 'none';
    }
    sliderList.style.transform = `translateX(-${index * 100}%)`;
}

// Chuyển tới slide tiếp theo
function nextSlide() {
    currentIndex++;
    showSlide(currentIndex);
    if (currentIndex === slideCount - 1) {
        setTimeout(() => {
            currentIndex = 1;
            showSlide(currentIndex, false);
        }, 500);
    }
}

// Chuyển tới slide trước
function prevSlide() {
    currentIndex--;
    showSlide(currentIndex);
    if (currentIndex === 0) {
        setTimeout(() => {
            currentIndex = slideCount - 2;
            showSlide(currentIndex, false);
        }, 500);
    }
}

nextBtn.addEventListener('click', () => {
    nextSlide();
    resetAuto();
});
prevBtn.addEventListener('click', () => {
    prevSlide();
    resetAuto();
});

let autoSlide = setInterval(nextSlide, 5000);

function resetAuto() {
    clearInterval(autoSlide);
    autoSlide = setInterval(nextSlide, 5000);
}

// Khởi tạo slide đầu tiên (sau clone)