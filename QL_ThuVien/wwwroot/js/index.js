// Hàm dùng để nhúng nội dung HTML vào phần tử có id tương ứng
function includeHTML(id, filePath) {
    const target = document.getElementById(id);
    if (!target) return;

    const resolvedPath = resolvePath(filePath);

    fetch(resolvedPath)
        .then(response => {
            if (!response.ok) throw new Error(`Không thể tải ${resolvedPath} (status ${response.status})`);
            return response.text();
        })
        .then(data => {
            target.innerHTML = data;
        })
        .catch(error => {
            console.error("Lỗi khi include:", error);
            target.innerHTML = `<p style="color:red;">Không thể tải nội dung từ ${resolvedPath}</p>`;
        });
}

// Giải quyết đường dẫn: nếu không phải đường dẫn tuyệt đối thì nối với window.appRoot
function resolvePath(path) {
    if (!path) return path;
    if (path.startsWith('http://') || path.startsWith('https://') || path.startsWith('/')) {
        return path;
    }
    const root = (window.appRoot || '/');
    return root.endsWith('/') ? root + path : root + '/' + path;
}

// Nhúng header và footer (giữ như cũ nếu bạn có pages/Header.html và pages/Footer.html trong wwwroot)
includeHTML("header-placeholder", "pages/Header.html");
includeHTML("footer-placeholder", "pages/Footer.html");

// Xác định nội dung chính cần nhúng dựa trên tham số URL
const params = new URLSearchParams(window.location.search);
let page = "QLThuVien/HomeContent"; // mặc định: gọi endpoint controller trả về partial view

// Map tham số URL với file HTML / endpoint tương ứng
const pageMap = {
    rent: "pages/rent.html",
    Home: "QLThuVien/HomeContent",
    category: "pages/category.html",
    Login: "pages/Login.html",
    Account: "pages/Account.html",
    Admin: "pages/Admin.html",
    get_help: "pages/help/get_help.html",
    returns: "pages/help/returns.html",
    payment_options: "pages/help/payment_options.html",
    contact_us: "pages/help/contact_us.html",
};

// Map tham số URL với CSS tương ứng
const cssMap = {
    rent: "css/rent.css",
    Home: "css/home.css",
    category: "css/category.css",
    Login: "css/Login.css",
    Account: "css/account.css",
    Admin: "css/admin.css",
    get_help: "css/help.css",
    returns: "css/help.css",
    payment_options: "css/help.css",
    contact_us: "css/help.css",
};

// Map tham số URL với JS tương ứng
const jsMap = {
    rent: "js/rent.js",
    Home: "js/home.js",
    category: "js/category.js",
    Login: "js/Login.js",
    Account: "js/account.js",
    Admin: "js/admin.js",
    get_help: "js/help.js",
    returns: "js/help.js",
    payment_options: "js/help.js",
    contact_us: "js/help.js",
};

// Kiểm tra tham số URL
let matched = false;
for (const key in pageMap) {
    if (params.has(key)) {
        matched = true;
        page = pageMap[key];

        if (cssMap[key]) addCSS(resolvePath(cssMap[key]));
        if (jsMap[key]) addJS(resolvePath(jsMap[key]));
        break;
    }
}

// Nếu không có tham số nào -> load Home mặc định + CSS/JS
if (!matched) {
    addCSS(resolvePath(cssMap.Home));
    addJS(resolvePath(jsMap.Home));
}

// Nhúng nội dung chính
includeHTML("main-placeholder", page);

// Hàm thêm CSS an toàn
function addCSS(href) {
    if (!href) return;
    if ([...document.styleSheets].some(sheet => sheet.href && sheet.href.includes(href))) {
        return;
    }
    const link = document.createElement("link");
    link.rel = "stylesheet";
    link.href = href;
    document.head.appendChild(link);
}

// Hàm thêm JS an toàn
function addJS(src) {
    if (!src) return;
    if ([...document.scripts].some(script => script.src && script.src.includes(src))) {
        return;
    }
    const script = document.createElement("script");
    script.src = src;
    script.defer = true;
    document.body.appendChild(script);
}