const rooms = [
  { id: 1, name: "Phòng 101", type: "Đơn", price: 300000, status: "Trống", description: "Phòng có điều hòa" },
  { id: 2, name: "Phòng 102", type: "Đôi", price: 500000, status: "Đang thuê", description: "Phòng có ban công" },
];

const roomTable = document.querySelector("#roomTable tbody");
const roomFormContainer = document.querySelector("#roomFormContainer");
const roomForm = document.querySelector("#roomForm");
const cancelBtn = document.querySelector("#cancelBtn");
const searchRoom = document.querySelector("#searchRoom");

function renderRooms(list = rooms) {
  roomTable.innerHTML = "";
  list.forEach(room => {
    const row = document.createElement("tr");
    row.innerHTML = `
      <td>${room.id}</td>
      <td>${room.name}</td>
      <td>${room.type}</td>
      <td>${room.price.toLocaleString()}</td>
      <td>${room.status}</td>
      <td>${room.description}</td>
      <td>
        <button class="edit" onclick="editRoom(${room.id})">Sửa</button>
      </td>
    `;
    roomTable.appendChild(row);
  });
}

function showForm(edit = false, room = null) {
  roomFormContainer.classList.remove("hidden");
  document.getElementById("formTitle").textContent = edit ? "Sửa Phòng" : "Thêm Phòng";
  
  if (edit && room) {
    document.getElementById("roomId").value = room.id;
    document.getElementById("roomName").value = room.name;
    document.getElementById("roomType").value = room.type;
    document.getElementById("roomPrice").value = room.price;
    document.getElementById("roomStatus").value = room.status;
    document.getElementById("roomDescription").value = room.description;
  } else {
    roomForm.reset();
    document.getElementById("roomId").value = "";
  }
}

cancelBtn.onclick = () => roomFormContainer.classList.add("hidden");

roomForm.onsubmit = (e) => {
  e.preventDefault();
  const id = document.getElementById("roomId").value;
  const roomData = {
    id: Number(id),
    name: document.getElementById("roomName").value,
    type: document.getElementById("roomType").value,
    price: Number(document.getElementById("roomPrice").value),
    status: document.getElementById("roomStatus").value,
    description: document.getElementById("roomDescription").value,
  };
  const index = rooms.findIndex(r => r.id == id);
  if (index !== -1) {
    rooms[index] = roomData;
    renderRooms();
  }
  roomFormContainer.classList.add("hidden");
};

function editRoom(id) {
  const room = rooms.find(r => r.id === id);
  showForm(true, room);
}

searchRoom.addEventListener("input", e => {
  const keyword = e.target.value.toLowerCase();
  const filtered = rooms.filter(r => r.name.toLowerCase().includes(keyword));
  renderRooms(filtered);
});

renderRooms();