const express = require('express');
const cors = require('cors');


const app = express();
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// 🟢 User Data (for login and profile management)
let id = 4;
let data = [
    { id: 1, name: "John", age: 25, password: "admin123", isEditable: true },
    { id: 2, name: "Jane", age: 30, password: "admin321", isEditable: true },
    { id: 3, name: "Bob", age: 35, password: "user420", isEditable: false }
];

let response = {
    message: '',
    success: true,
    data: []
}

let availablePermissions = [
    {
        "permissionId": 1,
        "permissionName": "User",
        "permissionAccess": "R-E-A-D",
        "child": [
            {
                "permissionId": 2,
                "permissionName": "Profile",
                "permissionAccess": "R-E",
                "child": []
            },
            {
                "permissionId": 3,
                "permissionName": "Password",
                "permissionAccess": "A-D",
                "child": []
            },
        ]
    }, {
        "permissionId": 4,
        "permissionName": "Settings",
        "permissionAccess": "R-E-A-D",
        "child": [
            {
                "permissionId": 5,
                "permissionName": "Misc",
                "permissionAccess": "R-E",
                "child": []
            },
            {
                "permissionId": 6,
                "permissionName": "Colors",
                "permissionAccess": "A-D",
                "child": [
                    {
                        "permissionId": 7,
                        "permissionName": "Theme",
                        "permissionAccess": "A-D",
                        "child": [
                            {
                                "permissionId": 8,
                                "permissionName": "Dark Theme",
                                "permissionAccess": "A-D",
                                "child": []
                            },
                        ]
                    },
                ]
            },
        ]
    }, {
        "permissionId": 9,
        "permissionName": "Dashboard",
        "permissionAccess": "R-E-A-D",
        "child": []
    }, 
]

let userPermissions = []

// 🟢 LOGIN: Authenticate user
app.post('/login', (req, res) => {
    const { name, password } = req.body;
    const user = data.find(profile => profile.name === name && profile.password === password);

    if (user) {
        res.json({ 
            message: "Login successful", 
            user: { id: user.id, name: user.name, age: user.age, isEditable: user.isEditable }, 
            success: true 
        });
    } else {
        res.status(401).json({ message: "Invalid name or password", success: false });
    }
});

// 🟢 GET: Retrieve all profiles
app.get('/list', (req, res) => {
    const updatedData = data.map(({ id, name, age, isEditable }) => ({ id, name, age, isEditable }));
    res.json({ message: "Retrieved list", data: updatedData, success: true });
});

// 🟢 PUT: Add a new profile
app.put('/list', (req, res) => {
    const newData = { id: id++, name: req.body.name, age: req.body.age, password: req.body.password, isEditable: req.body.isEditable ?? true };
    data.push(newData);
    res.json({ message: "Added to list", data: newData, success: true });
});

// 🟢 POST: Update an existing profile
app.post('/list', (req, res) => {
    const profile = data.find(d => d.id === req.body.id);
    if (profile) {
        profile.name = req.body.name;
        profile.age = req.body.age;
        profile.isEditable = req.body.isEditable ?? profile.isEditable;
        profile.password = req.body.password ?? profile.password;

        res.json({ message: "Updated profile", data: profile, success: true });
    } else {
        res.status(404).json({ message: "ID does not exist", success: false });
    }
});

// 🟢 DELETE: Remove a profile by ID
app.delete('/list', (req, res) => {
    const profileIndex = data.findIndex(d => d.id === req.body.id);
    if (profileIndex !== -1) {
        const deletedProfile = data.splice(profileIndex, 1)[0];
        res.json({ message: "Deleted profile", data: deletedProfile, success: true });
    } else {
        res.status(404).json({ message: "ID does not exist", success: false });
    }
});

app.get('/base-permissions', (req, res) => {
    response.message = 'Successfully fetching base permissions'
    response.success = true
    response.data = availablePermissions

    return res.status(200).json(response)
})

app.get('/user-permissions', (req, res) => {
    response.message = 'Successfully fetching user permissions'
    response.success = true
    response.data = userPermissions
    return res.status(200).json(response)
})

app.post('/update-permissions', (req, res) => {
    let perms = req.body.permissions
    let updatePermissions = []

    // 1~R-E-A-D_2~R-D_5~R-E
    let perm = perms.split('_')
    perm.map((p) => {
        let access = p.split('~')
        let permId = access[0]
        let a = access[1]

        updatePermissions.push(
            {
                "permissionId": permId,
                "permissionAccess": a
            }
        )
    })
    
    response.message = 'Successfully updated permissions'
    response.success = true
    response.data = []

    userPermissions = updatePermissions

    return res.status(200).json(response)
})


// 🟢 Start the server
app.listen(5000, () => {
    console.log("🚀 Server running on port 5000");
});
