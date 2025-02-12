db = db.getSiblingDB('admin');
db.auth('admin', 'securepassword');

db = db.getSiblingDB('tbas_db');
db.createUser({
    user: "appuser",
    pwd: "apppassword",
    roles: [
        { role: "dbOwner", db: "tbas_db" },
        { role: "readWrite", db: "tbas_db" }
    ]
});