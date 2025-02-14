db = db.getSiblingDB('admin');
db.auth('admin', 'securepassword');

// Skapa appuser i admin databasen
db.createUser({
    user: "appuser",
    pwd: "apppassword",
    roles: [
        { role: "dbOwner", db: "tbas_db" },
        { role: "readWrite", db: "tbas_db" }
    ]
});

// Byt till tbas_db och skapa collection
db = db.getSiblingDB('tbas_db');
db.createCollection('rectangles');