const apiUrl = 'http://localhost:5000/list'; // Base API URL

// Fetch and display all profiles
function getData() {
  axios.get(apiUrl).then((response) => {
    if (response.data.success) {
      const profiles = response.data.data;
      const tableBody = document.querySelector('#profileTable tbody');
      tableBody.innerHTML = ''; // Clear existing table rows

      profiles.forEach((profile) => {
        const row = `
          <tr>
            <td>${profile.id}</td>
            <td>${profile.name}</td>
            <td>${profile.age}</td>
          </tr>
        `;
        tableBody.innerHTML += row;
      });
    }
  }).catch((error) => console.error('Error fetching data:', error));
}

// Add a new profile
function addData() {
  const name = document.getElementById('name').value;
  const age = parseInt(document.getElementById('age').value);

  if (name && age) {
    axios.post(apiUrl, { name, age }).then((response) => {
      if (response.data.success) {
        alert('Profile added successfully!');
        getData(); // Refresh data
        document.getElementById('name').value = '';
        document.getElementById('age').value = '';
      }
    }).catch((error) => console.error('Error adding data:', error));
  } else {
    alert('Please fill out all fields.');
  }
}

// Update an existing profile
function updateData() {
  const id = parseInt(document.getElementById('updateId').value);
  const name = document.getElementById('updateName').value;
  const age = parseInt(document.getElementById('updateAge').value);

  if (id && (name || age)) {
    axios.put(`${apiUrl}/${id}`, { name, age }).then((response) => {
      if (response.data.success) {
        alert('Profile updated successfully!');
        getData(); // Refresh data
        document.getElementById('updateId').value = '';
        document.getElementById('updateName').value = '';
        document.getElementById('updateAge').value = '';
      }
    }).catch((error) => console.error('Error updating data:', error));
  } else {
    alert('Please fill out at least one field to update.');
  }
}

// Delete a profile
function deleteData() {
  const id = parseInt(document.getElementById('deleteId').value);

  if (id) {
    axios.delete(`${apiUrl}/${id}`).then((response) => {
      if (response.data.success) {
        alert('Profile deleted successfully!');
        getData(); // Refresh data
        document.getElementById('deleteId').value = '';
      }
    }).catch((error) => console.error('Error deleting data:', error));
  } else {
    alert('Please provide a valid ID.');
  }
}

// Fetch data on page load
getData();
