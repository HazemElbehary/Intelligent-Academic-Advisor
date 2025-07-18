# 🎓 AI-Based Academic Advising System

An **AI-enhanced academic advising platform** for the Faculty of Computers and Artificial Intelligence (FCAI), providing intelligent course recommendations and bylaw assistance for students and advisors.

---

## 📑 Table of Contents
- [Project Overview](#project-overview)
- [Key Features](#key-features)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [AI System (Flask) Setup](#ai-system-flask-setup)
  - [Backend Setup](#backend-setup)
  - [Frontend Setup](#frontend-setup)
- [Configuration & Environment Variables](#configuration--environment-variables)
- [Usage Examples](#usage-examples)
- [Testing & Development](#testing--development)
- [Authentication](#authentication)
- [AI Components](#ai-components)
- [Troubleshooting](#troubleshooting)
- [Future Improvements](#future-improvements)
- [Contributing](#contributing)
- [Contributors](#contributors)
- [Contact](#contact)
- [License](#license)

---

## 📌 Project Overview
The AI-Based Academic Advising System helps:
- Students: Get valid course paths and instant bylaw answers.
- Advisors: Monitor, support, and identify at-risk students.

---

## 💡 Key Features
- **Course Recommendation Engine:** CSP modeling & topological sorting for optimal course paths.
- **Academic Bylaw Chatbot:** RAG-based, context-aware bylaw Q&A.
- **Advisor Dashboard:** Visualize student progress, recommendations, and risk alerts.

---

## 🛠️ Tech Stack
- **Back-End:** ASP.NET Core (Onion Architecture, JWT, REST API)
- **AI System:** Python Flask (CSP, Topological Sort, RAG, LangChain, FAISS)
- **Front-End:** Angular (Responsive dashboard)

---

## �� Project Structure

```
AI-Advising-System/
├── BackEnd/
│   └── FCAI_BackEnd/
│       ├── FCAI.APIs/                      # ASP.NET Web API (Controllers, JWT, config)
│       │   ├── Controllers/                 # API controllers (Student, Admin, Auth, etc.)
│       │   ├── Middlewares/                 # Custom middleware
│       │   ├── Extension/                   # Extension methods
│       │   ├── Error/                       # Error handling
│       │   └── ...                          # Config, Program.cs, etc.
│       ├── FCAI.Domain/                    # Domain models and logic
│       │   ├── Entities/                    # Core entities (Student, Course, Department, etc.)
│       │   ├── Enums/                       # Enumerations
│       │   ├── Specifications/              # Domain specifications
│       │   └── ...
│       ├── FCAI.Application/               # Business logic
│       │   ├── Services/                    # Application services (Student, Course, Auth, etc.)
│       │   │   ├── CourseServices/
│       │   │   ├── StudentServices/
│       │   │   ├── DeptService/
│       │   │   └── UniversityService/
│       │   └── ...
│       ├── FCAI.Persistence/               # Data access, repositories, migrations
│       │   ├── Data/
│       │   ├── GenericRepo/
│       │   ├── UOW/                         # Unit of Work
│       │   ├── Configurations/
│       │   └── ...
│       └── FCAI.Application.Abstraction/   # Business logic interfaces, DTOs, exceptions
│           ├── IServices/
│           ├── DTOs/
│           ├── Exceptions/
│           └── ...
├── UI/
│   └── src/
│       ├── app/                            # Main Angular application (components, services, guards, interceptors, interfaces, shared)
│       └── assets/                         # Static assets (images, styles, etc.)
├── GP/                                    # AI Flask app
│   ├── app.py, run_app.py                  # Flask entry points
│   ├── requirements.txt                    # Python dependencies
│   ├── recommendation/                     # Course recommendation module (routes, utils, templates)
│   ├── chatBot/                            # Chatbot module (routes, utils, data, templates)
│   ├── bylaws_vector_index/                # Vector index for bylaw retrieval (FAISS, pickle)
│   └── ...
├── ScreenShoots/                          # UI screenshots
└── README.md
```

---

## 🚀 Getting Started

### 🧩 Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Node.js & npm](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- [Python 3.8+](https://www.python.org/)
- [pip](https://pip.pypa.io/en/stable/)
- (Optional) [virtualenv](https://virtualenv.pypa.io/)
- SQL Server (or compatible DB)

### 🧠 AI System (Flask) Setup
```bash
cd GP
python -m venv venv
# On Windows:
venv\Scripts\activate
# On Linux/Mac:
source venv/bin/activate
pip install -r requirements.txt
flask run
```

### ⚙️ Backend Setup
```bash
cd BackEnd/FCAI_BackEnd/FCAI.APIs
# Edit appsettings.json for DB and JWT config
# Example connection string:
# "ConnectionStrings": {
#   "ApplicationConnection": "Server=.;DataBase=FCAIDB;Trusted_Connection=true;TrustServerCertificate=true"
# }
dotnet run
```

### 🌐 Frontend Setup
```bash
cd UI
npm install
ng serve
```

---

## ⚙️ Configuration & Environment Variables
- **Backend (appsettings.json):**
  - `ConnectionStrings:ApplicationConnection`: SQL Server connection string
  - `JwtSettings:Key`: Secret key for JWT
  - `JwtSettings:Audience`, `JwtSettings:Issure`, `JwtSettings:DurationInMinutes`
  - `AIRecommendation:Endpoint`: Flask AI API URL (default: `http://127.0.0.1:5000/recommendation/api/recommend`)
- **AI System:**
  - All dependencies in `GP/requirements.txt`
- **Frontend:**
  - API endpoints configured in Angular services (see `src/environments/` if present)

---

## 📊 Usage Examples
- **Course Recommendation:**
  - `POST /recommendation/api/recommend` (Flask)
  - Body: `{ "student_id": "12345", "current_courses": ["CS101", ...] }`
- **Bylaw Q&A:**
  - `POST /chatbot/api/ask` (Flask)
  - Body: `{ "question": "What are the prerequisites for CS201?" }`
- **Backend API:**
  - `POST /api/auth/login` (ASP.NET)
  - `GET /api/students/{id}/recommendations`
- **Frontend:**
  - Access via `http://localhost:4200` after running `ng serve`

---

## 🧪 Testing & Development
- **Backend:**
  - Use `dotnet test` for .NET projects
- **Frontend:**
  - `ng test` for unit tests
  - `ng e2e` for end-to-end tests
- **AI System:**
  - Run test scripts in `GP/` (e.g., `test_import.py`, `test_performance.py`)

---

## 🔐 Authentication
- JWT-based authentication
- Roles: `Student`, `Advisor`
- Configure JWT in `appsettings.json`

---

## 🤖 AI Components
- **Course Recommendation:** CSP solver + topological sort
- **Bylaw Chatbot:** RAG, semantic search, LangChain, FAISS

---

## 🛠️ Troubleshooting
- **Port Conflicts:** Ensure backend (default 5000), frontend (4200), and AI system (5000) run on different ports.
- **CORS Issues:** Configure CORS in both backend and Flask as needed.
- **Database:** Check SQL Server is running and connection string is correct.
- **Dependencies:** Use correct Python, Node, and .NET versions.

---

## 📈 Future Improvements
- Natural language course planning
- ML-based risk prediction
- Mobile app version

---

## 🤝 Contributing
1. Fork the repo & clone locally
2. Create a new branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -am 'Add new feature'`)
4. Push to your fork (`git push origin feature/your-feature`)
5. Open a Pull Request

---

## 🧑‍💻 Contributors
- Hazem Ahmed
- Ahmed Oraby
- Hazem Magdy
- Hagar Galal
- Omnia Hamada

---

## 📫 Contact
**Hazem Ahmed**  
📧 hazemelbehary19@gmail.com

---

## 📜 License
This project is for academic use and case study purposes under FCAI. Licensing may be added for future open-source releases.