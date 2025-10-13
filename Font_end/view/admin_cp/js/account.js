
// let users = JSON.parse(localStorage.getItem("users")) || [];
// const tableBody = document.querySelector("#userTable tbody");
// const form = document.querySelector("#userForm");
// const addBtn = document.querySelector("#addUserBtn");
// const cancelBtn = document.querySelector("#cancelBtn");
// const formTitle = document.querySelector("#formTitle");
// const idField = document.querySelector("#userId");
// const nameField = document.querySelector("#userName");
// const emailField = document.querySelector("#userEmail");
// const roleField = document.querySelector("#userRole");
// const statusField = document.querySelector("#userStatus");


// function saveUsers() {
//   localStorage.setItem("users", JSON.stringify(users));
// }

// function renderTable() {
//   tableBody.innerHTML = "";
//   users.forEach((u, i) => {
//     const tr = document.createElement("tr");
//     tr.innerHTML = `
//       <td>${u.id}</td>
//       <td>${u.name}</td>
//       <td>${u.email}</td>
//       <td>${u.role}</td>
//       <td>${u.status}</td>
//       <td class="actions">
//         <button onclick="editUser(${i})">✏️ Sửa</button>
//         <button onclick="deleteUser(${i})">🗑️ Xóa</button>
//       </td>`;
//     tableBody.appendChild(tr);
//   });
// }

// addBtn.onclick = () => {
//   form.style.display = "block";
//   formTitle.textContent = "Thêm người dùng";
//   form.reset();
//   idField.value = "";
// };

// cancelBtn.onclick = () => form.style.display = "none";

// form.onsubmit = (e) => {
//   e.preventDefault();
//   const id = idField.value || Date.now();
//   const userData = {
//     id,
//     name: nameField.value.trim(),
//     email: emailField.value.trim(),
//     role: roleField.value,
//     status: statusField.value
//   };
//   if (idField.value) {
//     // cập nhật
//     const index = users.findIndex(u => u.id == id);
//     users[index] = userData;
//   } else {
//     users.push(userData);
//   }
//   saveUsers();
//   renderTable();
//   form.reset();
//   form.style.display = "none";
// };

// function editUser(index) {
//   const u = users[index];
//   form.style.display = "block";
//   formTitle.textContent = "Sửa người dùng";
//   idField.value = u.id;
//   nameField.value = u.name;
//   emailField.value = u.email;
//   roleField.value = u.role;
//   statusField.value = u.status;
// }

// function deleteUser(index) {
//   if (confirm("Bạn có chắc chắn muốn xóa người dùng này không?")) {
//     users.splice(index, 1);
//     saveUsers();
//     renderTable();
//   }
// }

// renderTable();
