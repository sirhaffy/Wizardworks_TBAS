
# Frontend
The frontend application is built with Vite and uses React as the framework. 
For styling, SASS is used, enabling powerful and modular CSS solutions.

Start the frontend:
```bash
yarn run dev
``` 

# Backend
The backend application is built with ASP.NET Core and implements the following:

- Dependency Injection: Manages singletons and transient services effectively.
- CORS: Enables communication between the frontend and backend.
- Swagger: Enabled to provide API documentation in the development environment.

**Starta backend**
Navigate to the backend project's folder.

Run the application:
```bash
dotnet run
```

Swagger documentation is available at:
```bash
http://localhost:5129/swagger
```

# To start MongoDB server.
In solution root (where the docker-compose.yml file exists), run.

Start docker-compose:
```bash
docker-compose up -d
```

# GitHub Actions CI/CD
Is set up but not used, and I don't have any server (Azure or AWS) to deploy the application to.

View GitHub Actions Workflow file:
```bash
nano .github/workflows/main.yml
```

# IaC (Infrastructure as Code)
I would use Terraform to deploy the application to Azure or AWS.

To automate infrastructure, the following tools would be used:
- Terraform: To define and create resources on Azure or AWS.
- Ansible: To configure the server after the resources have been provisioned.

Example script (Terraform):
```bash
provider "azurerm" {
  features = {}
}

resource "azurerm_resource_group" "example" {
  name     = "my-resource-group"
  location = "West Europe"
}

```

Example script (Ansible):
```bash
- hosts: webserver
  tasks:
    - name: Installera Nginx
      apt:
        name: nginx
        state: present

```

# API Key Generation
The API key is used for authentication and is stored in:

- Frontend: .env-file.
- Backend: appsettings.json.

If a server were available, the key would be moved to GitHub Secrets or Key Vault for secure storage.

Python script:
```python
import secrets

api_key = secrets.token_urlsafe(32)
print(api_key)
```
