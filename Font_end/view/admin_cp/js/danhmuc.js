
    let products = [

    ];
    renderTable();

    function renderTable() {
      const tbody = document.querySelector("#productTable tbody");
      tbody.innerHTML = "";
      products.forEach((p, i) => {
        tbody.innerHTML += `
          <tr>
            <td>${i + 1}</td>
            <td>${p.name}</td>
            <td>${p.desc}</td>
            <td>${p.price}</td>
            <td>${p.stock}</td>
            <td>${p.category}</td>
            <td><img src="${p.image}" alt=""></td>
            <td>
              <button class="btn btn-edit" onclick="editProduct(${i})">Edit</button>
              <button class="btn btn-del" onclick="deleteProduct(${i})">Delete</button>
            </td>
          </tr>
        `;
      });
    }

    function showForm(editIndex = null) {
      document.getElementById("formContainer").style.display = "block";
      if (editIndex !== null) {
        document.getElementById("formTitle").innerText = "Sửa sản phẩm";
        document.getElementById("editIndex").value = editIndex;
        let p = products[editIndex];
        document.getElementById("name").value = p.name;
        document.getElementById("desc").value = p.desc;
        document.getElementById("price").value = p.price;
        document.getElementById("stock").value = p.stock;
        document.getElementById("category").value = p.category;
        document.getElementById("image").value = p.image;
      } else {
        document.getElementById("formTitle").innerText = "Thêm sản phẩm";
        document.getElementById("editIndex").value = "";
        document.querySelectorAll("#formContainer input, #formContainer select").forEach(el => el.value = "");
      }
    }

    function hideForm() {
      document.getElementById("formContainer").style.display = "none";
    }

    function saveProduct() {
      let name = document.getElementById("name").value;
      let desc = document.getElementById("desc").value;
      let price = document.getElementById("price").value;
      let stock = parseInt(document.getElementById("stock").value);
      let category = document.getElementById("category").value;
      let image = document.getElementById("image").value;
      let editIndex = document.getElementById("editIndex").value;

      let newProduct = { name, desc, price, stock, category, image };

      if (editIndex === "") {
        products.push(newProduct);
      } else {
        products[editIndex] = newProduct;
      }
      hideForm();
      renderTable();
    }

    function editProduct(i) {
      showForm(i);
    }

    function deleteProduct(i) {
      if (confirm("Bạn có chắc muốn xóa?")) {
        products.splice(i, 1);
        renderTable();
      }
    }

    renderTable();