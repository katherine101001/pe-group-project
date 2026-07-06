# 🏗️ Project Management System

> Full-stack enterprise project management platform built with **C# .NET Clean Architecture** + **React**, fully containerized with Docker.

---

## 📸 Screenshots

<!-- TODO: Add screenshots here -->
<!-- Recommended: Dashboard, Project Details, Analytics, Task Board, Dark Mode -->

| Dashboard | Project Analytics | Task Board |
|:---:|:---:|:---:|
| ![Dashboard](screenshots/dashboard.png) | ![Analytics](screenshots/analytics.png) | ![Tasks](screenshots/tasks.png) |

---

## 🧱 Architecture — Clean Architecture (5 Layers)

```
ProjectManagement.sln
├── 📁 ProjectManagement.Domain/          # Entities + Repository Interfaces
│   ├── Entities/
│   │   ├── Users/         (User, Role)
│   │   ├── Projects/      (Project, ProjectMember, ProjectGoal)
│   │   ├── ProjectTasks/  (ProjectTask, SubTask, TaskAttachment)
│   │   └── Collaborations/(Comment, Mention, Notification)
│   └── Interfaces/Repositories/          # IUserRepo, IProjectRepo, etc.
│
├── 📁 ProjectManagement.Application/     # Business Logic (Use Cases)
│   ├── DTOs/              # 20+ DTOs for request/response
│   ├── Services/          # User, Project, Task, Comment, Email, Notification, Dashboard
│   ├── Interfaces/Services/
│   └── Mapping/           # AutoMapper profiles
│
├── 📁 ProjectManagement.Infrastructure/  # Data Access + External Services
│   ├── Data/              # AppDbContext (EF Core + SQL Server)
│   ├── Repositories/      # 6 repository implementations
│   └── Migrations/        # EF Core migrations
│
├── 📁 ProjectManagement.API/             # ASP.NET Core Web API
│   └── Controllers/       # 12 controllers (RESTful endpoints)
│
└── 📁 ProjectManagement.Shared/          # Cross-cutting
    └── Exceptions/         # NotFoundException, ValidationException
```

---

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| **Backend Framework** | ASP.NET Core Web API (.NET 8) |
| **ORM** | Entity Framework Core (Code-First + Migrations) |
| **Database** | SQL Server |
| **Object Mapping** | AutoMapper |
| **API Docs** | Swagger / OpenAPI |
| **Email** | Mailjet (transactional emails for @mentions) |
| **Frontend** | React 18 + Vite |
| **State Management** | Redux Toolkit |
| **Routing** | React Router v6 (role-based protected routes) |
| **Styling** | Tailwind CSS (dark mode supported) |
| **Charts** | Recharts (bar, pie charts for analytics) |
| **Icons** | Lucide React |
| **Notifications** | react-hot-toast |
| **Dates** | date-fns |
| **Containerization** | Docker + Docker Compose |
| **CI/CD** | GitHub Actions → AWS EC2 auto-deploy |

---

## ✨ Features

### 🔐 Authentication & Authorization
- Register / Login with role-based access (**ADMIN**, **LEADER**, **MEMBER**)
- Protected routes — different pages visible per role
- User activation flow

### 📊 Dashboard
- Welcome greeting with dynamic user name
- **Stats Grid**: total projects, tasks, team members, completion rate
- **Project Overview**: all projects with progress bars, status badges
- **Recent Activity**: timeline of latest task updates
- **Tasks Summary**: personal task list with priority + due date

### 📁 Project Management
- **Create** projects with title, description, priority (LOW/MEDIUM/HIGH), status, date range
- Assign **Team Lead** and **Team Members** by email
- **Edit** project settings — add/remove members, update details
- **Archive & Unarchive** — soft-delete with full recovery
- **Delete** projects permanently
- **Search** across all projects by keyword

### ✅ Task Management
- Create tasks with type (**TASK / BUG / FEATURE / IMPROVEMENT**)
- Set priority, due date, assignee
- **SubTasks** — break down work into checklists
- **Attachments** — file uploads per task
- Status workflow: **TODO → IN_PROGRESS → DONE**
- Overdue detection (highlights expired tasks)

### 📈 Analytics (per project)
- **Bar Chart**: task distribution by status
- **Pie Chart**: task distribution by type
- **Priority Breakdown**: LOW / MEDIUM / HIGH with percentages
- **Completion Rate**: percentage + count
- **Overdue Tasks**: count with warning indicator

### 📅 Calendar
- Monthly calendar view showing task deadlines
- Task cards positioned at their due dates
- Click to navigate to task details

### 💬 Collaboration
- **Comments** on tasks (threaded discussion)
- **@mentions** — tag team members in comments
- **Email notifications** via Mailjet when mentioned
- **In-app notifications** for mentions, assignments, project updates

### 🎨 UX
- **Dark Mode** toggle (persisted in Redux state)
- Responsive design (mobile-friendly)
- Toast notifications for all actions
- Loading spinners and empty states

---

## 🐳 Docker Deployment

```yaml
# docker-compose.yml
services:
  backend-api:     # .NET Web API on port 5000
  frontend-ui:     # React (nginx) on port 80
```

```bash
docker compose up --build -d
```

---

## 🚀 CI/CD Pipeline

On every push to `main`:

```
GitHub push → GitHub Actions → SSH into AWS EC2 → git pull → docker compose up --build -d
```

Automated by `appleboy/ssh-action` with zero-downtime rebuild.

---

## 📡 API Endpoints

| Controller | Endpoints |
|-----------|----------|
| **User** | `POST /api/User/register`, `POST /api/User/login`, `GET /api/User`, `GET /api/User/{id}` |
| **Project** | `GET/POST /api/Project`, `GET/PUT/DELETE /api/Project/{id}`, `PUT archive/{id}`, `PUT unarchive/{id}` |
| **ProjectTask** | `GET/POST /api/ProjectTask`, `GET/PUT/DELETE /api/ProjectTask/{id}`, `GET task-analytics` |
| **Comment** | `GET/POST /api/Comment`, `PUT/DELETE /api/Comment/{id}` |
| **Dashboard** | `GET /api/Dashboard/stats`, `GET /api/Dashboard/tasks`, `GET /api/Dashboard/recent-activity` |
| **Notification** | `GET/POST /api/Notification`, `PUT/DELETE /api/Notification/{id}` |
| **Search** | `GET /api/Search?keyword=` |
| **Calendar** | `GET /api/ProjectCalendar/{projectId}` |

Full Swagger docs available at `/swagger` in dev mode.

---

## 🗄️ Database Schema

| Table | Key Fields |
|-------|-----------|
| **Users** | Id, Name, Email, Password, ProfilePicture, RoleId, IsActivated |
| **Roles** | Id, Name |
| **Projects** | Id, Title, Description, Status, Priority, Progress, StartDate, EndDate, LeaderId, IsArchived |
| **ProjectMembers** | ProjectId + UserId (composite key) |
| **ProjectGoals** | Id, ProjectId, Title, IsCompleted |
| **ProjectTasks** | Id, ProjectId, AssignToUserId, Title, Description, DueDate, Status, Priority, Type |
| **SubTasks** | Id, ProjectTaskId, Title, IsCompleted |
| **TaskAttachments** | Id, ProjectTaskId, FileName, FileUrl |
| **Comments** | Id, ProjectTaskId, AuthorId, Content |
| **Mentions** | Id, CommentId, MentionedUserId |
| **Notifications** | Id, UserId, ProjectId, ProjectTaskId, CommentId, Message, IsRead |

---

## 🗂️ Frontend Structure

```
src/
├── app/store.js              # Redux store
├── App.jsx                   # Routes (public + protected)
├── features/
│   ├── userSlice.js          # Auth state
│   ├── projectsSlice.js      # Project state
│   └── themeSlice.js         # Dark mode state
├── pages/
│   ├── Login.jsx / Register.jsx
│   ├── Dashboard.jsx          # Stats + overview + activity + tasks
│   ├── Projects.jsx           # Project listing + create dialog
│   ├── ProjectDetails.jsx     # Tabs: overview | tasks | analytics | calendar | settings
│   ├── TaskDetails.jsx        # Full task with subtasks, attachments, comments
│   ├── Team.jsx               # Team member management
│   ├── Archive.jsx            # Archived projects
│   └── Layout.jsx             # Sidebar + navbar shell
├── components/
│   ├── Navbar.jsx             # Top bar with search, dark mode toggle, notifications
│   ├── Sidebar.jsx            # Navigation sidebar
│   ├── StatsGrid.jsx          # 4-card stats overview
│   ├── ProjectCard.jsx        # Project card with progress bar
│   ├── ProjectOverview.jsx    # Project list on dashboard
│   ├── ProjectTasks.jsx       # Task board with filters
│   ├── ProjectAnalytics.jsx   # Charts (Recharts bar/pie)
│   ├── ProjectCalendar.jsx    # Monthly calendar with task dots
│   ├── ProjectSettings.jsx    # Edit project form
│   ├── RecentActivity.jsx     # Activity timeline
│   ├── TasksSummary.jsx       # Personal task list
│   ├── MyTasksSidebar.jsx     # Quick-access task panel
│   ├── CreateProjectDialog.jsx
│   ├── CreateTaskDialog.jsx
│   ├── InviteMemberDialog.jsx
│   └── ProtectedRoute.jsx     # Role-based route guard
└── services/
    ├── api.js                 # Axios instance with interceptors
    ├── Project/ProjectAPI.js
    ├── ProjectTask/ProjectTaskAPI.js
    ├── Comment/CommentAPI.js
    ├── Dashboard/DashboardAPI.js
    ├── Team/team.api.js
    └── Search/SearchAPI.js
```

---

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- Docker Desktop
- SQL Server (or use Docker)

### Backend

```bash
cd ProjectManagement.API
dotnet restore
dotnet run
# API running at https://localhost:5001
# Swagger at https://localhost:5001/swagger
```

### Frontend

```bash
cd ProjectManagement-FrontEnd
npm install
npm run dev
# UI running at http://localhost:5173
```

### Docker (full stack)

```bash
docker compose up --build -d
# Backend: http://localhost:5000
# Frontend: http://localhost:80
```

---

## 👥 Team

This project was developed as a collaborative group project for **Software Engineering Practice**.

---

<p align="center">
  <i>Built with Clean Architecture · Containerized · CI/CD Ready</i>
</p>
