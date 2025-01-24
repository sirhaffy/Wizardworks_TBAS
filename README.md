
# Frontend
The frontend is built with vite and runs React. CSS is implemented with SASS.

# Backend
Dependency Injection is implemented in the application and will handle singletons and transient services.

# To start MongoDB server.
In solution root (where the docker-compose.yml file exists), run.

```bash
docker-compose up -d
```

# GitHub Actions CI/CD
Is set up but not used, asn I don't have any server (Azure or AWS) to deploy the application to.

```bash
nano .github/workflows/main.yml
```

# JWT Authentication
Is implemented and secret key is stored in .env file (Frontend) and applicationsettings.cs (Backend). 
Generated with SSL keygen.

```bash
openssl rand -base64 32
```