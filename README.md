# ManuTrack — Manufacturing Operations Platform

ManuTrack is a full-stack manufacturing operations management platform built with a **Microservices Architecture**. It helps manufacturers manage products, BOMs, work orders, inventory, quality inspections, compliance, analytics, and notifications — all through a single unified Angular dashboard.

---

## 📋 Table of Contents

- [Architecture Overview](#architecture-overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [User Roles](#user-roles)
- [Prerequisites](#prerequisites)
- [Running with Docker (Recommended)](#running-with-docker-recommended)
- [Running Locally (Without Docker)](#running-locally-without-docker)
- [Running on GitHub Codespaces](#running-on-github-codespaces)
- [SonarQube Code Analysis](#sonarqube-code-analysis)
- [Default Credentials](#default-credentials)
- [Port Reference](#port-reference)
- [Environment Variables](#environment-variables)

---

## 🏗️ Architecture Overview

```
                        ┌──────────────────────────────┐
                        │   Angular Frontend (Port 4200) │
                        │   Served by nginx              │
                        └──────────────┬───────────────┘
                                       │ /api/*
                        ┌──────────────▼───────────────┐
                        │   API Gateway / Ocelot (5000)  │
                        │   Routes all API requests      │
                        └──┬───┬───┬───┬───┬───┬───┬──┘
                           │   │   │   │   │   │   │
              ┌────────────┘   │   │   │   │   │   └────────────────┐
              │            ┌───┘   │   │   │   └──────┐             │
              ▼            ▼       ▼   ▼   ▼          ▼             ▼
         AuthService  ProductSvc  WO  Inv  QC   Analytics  Compliance  Notification
          (5001)       (5004)    (5002)(5003)(5006) (5005)   (5007)      (5008)
              │            │       │   │               │
              └────────────┴───────┴───┘               │
                           │                           │
              ┌────────────▼────────────────────────────▼──┐
              │              SQL Server (Port 1433)          │
              │  GovernanceDB │ OperationsDB │ QualityDB     │
              └──────────────────────────────────────────── ┘
```

### Key Patterns
| Pattern | Implementation |
|---------|---------------|
| **Microservices** | 8 independent .NET services, each with its own DB |
| **API Gateway** | Ocelot routes all frontend requests to correct service |
| **Database-per-Service** | 3 SQL Server databases across 8 services |
| **Repository Pattern** | `IXxxRepository` + `XxxRepository` in every service |
| **Service Layer** | `IXxxService` + `XxxServiceImpl` in every service |
| **JWT Authentication** | Bearer tokens validated at API Gateway level |
| **Dependency Injection** | .NET built-in DI with `AddScoped` registrations |
| **Hierarchical DI (Angular)** | `providedIn: 'root'` services + NgModule providers |

---

## 🛠️ Tech Stack

### Backend
| Technology | Version | Purpose |
|-----------|---------|---------|
| .NET | 10.0 | All microservices runtime |
| ASP.NET Core | 10.0 | REST API framework |
| Entity Framework Core | 10.0 | ORM for database access |
| SQL Server | 2022 | Relational database |
| Ocelot | Latest | API Gateway |
| JWT Bearer | 10.0 | Authentication |
| BCrypt.Net | 4.1.0 | Password hashing |
| Swashbuckle (Swagger) | 6.9.0 | API documentation |

### Frontend
| Technology | Version | Purpose |
|-----------|---------|---------|
| Angular | 21.2 | SPA framework |
| TypeScript | 5.9 | Language |
| RxJS | 7.8 | Reactive programming |
| Chart.js | 4.5 | Analytics charts |
| nginx | Alpine | Static file serving + API proxy |

### DevOps
| Technology | Purpose |
|-----------|---------|
| Docker | Containerisation |
| Docker Compose | Multi-container orchestration |
| SonarQube LTS | Code quality analysis |
| GitHub Codespaces | Cloud development environment |

---

## 📁 Project Structure

```
ManuTrackF1/
├── ApiGateway/                  ← Ocelot API Gateway (Port 5000)
│   ├── ocelot.json              ← Route configuration
│   └── Dockerfile
├── AuthService/                 ← User auth & JWT (Port 5001)
├── WorkOrderService/            ← Work orders & tasks (Port 5002)
├── InventoryService/            ← Stock & suppliers (Port 5003)
├── ProductService/              ← Products, BOM, components (Port 5004)
├── AnalyticsService/            ← KPIs & reports (Port 5005)
├── QualityService/              ← Inspections & defects (Port 5006)
├── ComplianceService/           ← Compliance & audit logs (Port 5007)
├── NotificationService/         ← Notifications (Port 5008)
├── ManuTrack.SharedKernel/      ← Shared filters, middleware, helpers
├── ManuTrack.UI/                ← Angular 21 frontend (Port 4200)
│   ├── src/app/
│   │   ├── core/
│   │   │   ├── services/        ← All Angular injectable services
│   │   │   ├── guards/          ← AuthGuard, NoAuthGuard
│   │   │   └── interceptors/    ← JWT AuthInterceptor
│   │   └── features/            ← Role-based feature components
│   ├── Dockerfile
│   └── nginx.conf               ← SPA routing + API proxy
└── docker-compose.yml           ← Full stack orchestration
```

---

## 👥 User Roles

| Role | Access |
|------|--------|
| **Admin** | Full system access — manage users, products, all data |
| **Planner** | Create products, BOMs, work orders, assign tasks |
| **Operator** | View and update assigned work order tasks |
| **Inspector** | Create quality inspections and log defects |
| **InventoryManager** | Manage stock levels, purchase orders, suppliers |
| **ComplianceOfficer** | View compliance reports and audit logs |

---

## ✅ Prerequisites

### For Docker (Recommended)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) — v24+
- [Git](https://git-scm.com/)
- 8 GB RAM minimum (SQL Server + 8 services)

### For Local Development (Without Docker)
- [.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 22+](https://nodejs.org/)
- [SQL Server 2022](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express edition is fine)
- [Angular CLI 21](https://angular.io/cli): `npm install -g @angular/cli`

---

## 🐳 Running with Docker (Recommended)

This is the easiest way to run the full stack on any machine.

### Step 1 — Clone the repository

```bash
git clone https://github.com/darshanvdevamane-pixel/ManuTrack_latest.git
cd ManuTrack_latest
```

### Step 2 — Build and start all services

```bash
docker-compose up --build
```

> ⏳ **First run takes 5–10 minutes** — it pulls base images, builds all services, and waits for SQL Server to initialise. Subsequent runs are much faster.

### Step 3 — Access the application

| Service | URL |
|---------|-----|
| Angular UI | http://localhost:4200 |
| API Gateway | http://localhost:5000 |
| SonarQube | http://localhost:9000 |

### Step 4 — Login

```
Email:    admin@manutrack.com
Password: Admin@123
```

### Useful Docker commands

```bash
# Start in background (detached mode)
docker-compose up -d --build

# Stop all containers
docker-compose down

# Stop and wipe all data (fresh start)
docker-compose down -v

# View logs of a specific service
docker-compose logs -f auth-service

# Restart a single service
docker-compose restart auth-service

# Check running containers
docker ps
```

---

## 💻 Running Locally (Without Docker)

### Step 1 — Clone the repository

```bash
git clone https://github.com/darshanvdevamane-pixel/ManuTrack_latest.git
cd ManuTrack_latest
```

### Step 2 — Set up SQL Server

Ensure SQL Server is running locally. The `appsettings.json` in each service uses:
```
Server=(localdb)\MSSQLLocalDB
```
Each service will auto-create its database on first run via EF Core migrations.

### Step 3 — Run backend services

Open separate terminals for each service:

```bash
# Terminal 1 - API Gateway
cd ApiGateway && dotnet run

# Terminal 2 - Auth Service
cd AuthService && dotnet run

# Terminal 3 - Product Service
cd ProductService && dotnet run

# Terminal 4 - WorkOrder Service
cd WorkOrderService && dotnet run

# Terminal 5 - Inventory Service
cd InventoryService && dotnet run

# Terminal 6 - Quality Service
cd QualityService && dotnet run

# Terminal 7 - Compliance Service
cd ComplianceService && dotnet run

# Terminal 8 - Analytics Service
cd AnalyticsService && dotnet run

# Terminal 9 - Notification Service
cd NotificationService && dotnet run
```

### Step 4 — Run the Angular frontend

```bash
cd ManuTrack.UI
npm install
ng serve
```

Open http://localhost:4200

---

## ☁️ Running on GitHub Codespaces

ManuTrack is fully compatible with GitHub Codespaces — no local setup required.

### Step 1 — Open in Codespaces

1. Go to https://github.com/darshanvdevamane-pixel/ManuTrack_latest
2. Click **Code** → **Codespaces** → **Create codespace on main**

### Step 2 — Build and start

Once the Codespace loads, run in the terminal:

```bash
docker-compose up --build
```

### Step 3 — Access the app

Codespaces auto-forwards ports. Click the forwarded URL for port **4200** in the **Ports** tab.

> ⚠️ Make sure ports are set to **Public** visibility in the Ports panel if you want to share the URL.

### Step 4 — Login

```
Email:    admin@manutrack.com
Password: Admin@123
```

---

## 🔍 SonarQube Code Analysis

SonarQube is included in `docker-compose.yml` and runs automatically with the stack.

### Step 1 — Start the full stack (including SonarQube)

```bash
docker-compose up -d
```

Wait ~60 seconds for SonarQube to initialise.

### Step 2 — Access SonarQube dashboard

**Local:** http://localhost:9000

**Codespaces:** Use `localhost:9000` from the terminal (not the forwarded URL — it requires GitHub auth).

Login: `admin` / `admin` → you will be prompted to change password.

### Step 3 — Create a project and get a token

1. Click **Create Project** → **Manually**
2. Project Key: `ManuTrack`
3. Go to **My Account** → **Security** → **Generate Token**
4. Copy the token

### Step 4 — Install the SonarScanner (if not installed)

```bash
dotnet tool install --global dotnet-sonarscanner
```

### Step 5 — Run the analysis

```bash
# Begin scan
dotnet sonarscanner begin \
  /k:"ManuTrack" \
  /d:sonar.host.url="http://localhost:9000" \
  /d:sonar.login="YOUR_TOKEN_HERE"

# Build all projects
dotnet build ManuTrack.slnx

# End scan and upload results
dotnet sonarscanner end /d:sonar.login="YOUR_TOKEN_HERE"
```

> ✅ **Important for Codespaces:** Always use `http://localhost:9000` as the host URL — the external forwarded URL is blocked by GitHub auth.

---

## 🔑 Default Credentials

### Application Login

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@manutrack.com | Admin@123 |

> New users can be created by the Admin from the Admin dashboard.

### SQL Server (Docker)

| Field | Value |
|-------|-------|
| Server | `localhost,1433` |
| Username | `sa` |
| Password | `YourStrong@Passw0rd` |

### SonarQube

| Field | Value |
|-------|-------|
| URL | http://localhost:9000 |
| Username | `admin` |
| Password | `admin` (change on first login) |

---

## 🌐 Port Reference

| Port | Service | Description |
|------|---------|-------------|
| 4200 | Angular UI | Frontend (nginx) |
| 5000 | API Gateway | Ocelot — single entry point for all APIs |
| 5001 | AuthService | Login, register, user management |
| 5002 | WorkOrderService | Work orders and tasks |
| 5003 | InventoryService | Stock, purchase orders, suppliers |
| 5004 | ProductService | Products, BOM, raw materials |
| 5005 | AnalyticsService | KPI reports, production metrics |
| 5006 | QualityService | Inspections and defects |
| 5007 | ComplianceService | Compliance reports and audit logs |
| 5008 | NotificationService | User notifications |
| 1433 | SQL Server | Database |
| 9000 | SonarQube | Code quality dashboard |

---

## ⚙️ Environment Variables

All services are configured via `docker-compose.yml` environment variables which override `appsettings.json`. Key variables:

```yaml
# Database connections (override appsettings.json)
ConnectionStrings__GovernanceDb=Server=sqlserver,1433;Database=ManuTrackGovernanceDB;...
ConnectionStrings__OperationsDb=Server=sqlserver,1433;Database=ManuTrackOperationsDB;...
ConnectionStrings__QualityDb=Server=sqlserver,1433;Database=ManuTrackQualityDB;...

# Inter-service communication (Docker container names)
ServiceUrls__ComplianceService=http://compliance-service:8080
ServiceUrls__NotificationService=http://notification-service:8080
ServiceUrls__QualityService=http://quality-service:8080
```

> To change the SA password, update `SA_PASSWORD` in `docker-compose.yml` under the `sqlserver` service **and** all `ConnectionStrings__*` values across all services.

---

## 🗄️ Database Structure

| Database | Services Using It |
|----------|------------------|
| `ManuTrackGovernanceDB` | AuthService, NotificationService, ComplianceService (audit) |
| `ManuTrackOperationsDB` | WorkOrderService, InventoryService, ProductService |
| `ManuTrackQualityDB` | QualityService, AnalyticsService, ComplianceService (reports) |

Databases and tables are **auto-created on first startup** via EF Core migrations — no manual SQL setup needed.

---

## 🔒 Security Notes

- JWT tokens expire after **60 minutes**
- Passwords are hashed using **BCrypt** (work factor 4)
- All API routes require a valid JWT except `POST /api/v1/auth/login`
- Change default credentials before deploying to production
- Replace `YourStrong@Passw0rd` with a secure password in production

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/your-feature`
3. Commit: `git commit -m "Add your feature"`
4. Push: `git push origin feature/your-feature`
5. Open a Pull Request

---

*Built with ❤️ using .NET 10, Angular 21, Docker, and SonarQube*
