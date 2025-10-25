/**
 * File: category.js
 * Chức năng: Xử lý logic chuyển đổi giữa các trang sản phẩm (Page 1, Page 2,...)
 */

document.addEventListener('DOMContentLoaded', () => {
    // 1. Lấy các phần tử cần thiết
    const pageLinks = document.querySelectorAll('.page_link[data-page]');
    const paginationContainer = document.querySelector('.pagination');
    const prevButton = paginationContainer.querySelector('.page_link:first-child');
    const nextButton = paginationContainer.querySelector('.page_link:last-child');
    const totalPages = pageLinks.length;
    let currentPage = 1;

    // 2. Hàm hiển thị trang cụ thể
    function showPage(pageNumber) {
        // Ẩn tất cả các trang
        document.querySelectorAll('.product_grid').forEach(grid => {
            grid.style.display = 'none';
        });

        // Hiển thị trang được chọn
        const targetPage = document.getElementById(`page_${pageNumber}`);
        if (targetPage) {
            targetPage.style.display = 'grid';
            currentPage = pageNumber;
        }

        // Cập nhật trạng thái active cho nút phân trang
        pageLinks.forEach(link => {
            link.classList.remove('active');
            if (parseInt(link.dataset.page) === currentPage) {
                link.classList.add('active');
            }
        });

        // Cập nhật trạng thái Disabled cho nút Trước/Sau
        prevButton.classList.toggle('disabled', currentPage === 1);
        nextButton.classList.toggle('disabled', currentPage === totalPages);
    }

    // 3. Gắn sự kiện cho các nút số trang
    pageLinks.forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault();
            const pageNumber = parseInt(e.currentTarget.dataset.page);
            showPage(pageNumber);
        });
    });

    // 4. Gắn sự kiện cho nút 'Trước'
    prevButton.addEventListener('click', (e) => {
        e.preventDefault();
        if (currentPage > 1) {
            showPage(currentPage - 1);
        }
    });

    // 5. Gắn sự kiện cho nút 'Sau'
    nextButton.addEventListener('click', (e) => {
        e.preventDefault();
        if (currentPage < totalPages) {
            showPage(currentPage + 1);
        }
    });

    // 6. Hiển thị trang đầu tiên khi tải trang
    showPage(1);
});