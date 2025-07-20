# 🧩 Headless CMS (Angular + .NET)

A **fullstack Headless CMS** with a modern Angular frontend and a flexible .NET backend.  
Supports dynamic creation of content types, structured content storage, and clean API delivery.

---

## 🛠️ Tech Stack

### 🖥️ Frontend

- **Angular** (Standalone APIs, Signals, Reactive Forms)
- **PrimeNG** for UI components
- **NX Monorepo** for scalable workspace

### ⚙️ Backend

- **ASP.NET Core Web API**
- **SQLite** for local development
- **Dapper** (planned) for flexible querying

---

## 🚀 Features

### ✅ Core Functionality

- [x] Create custom content types (e.g., Blog, Products, Pages)
- [x] Add custom fields (e.g., text, number, image)
- [x] Store records per content type
- [x] RESTful API to fetch content
- [x] Admin panel to manage models and records

### 🧱 Planned Architecture Features

- [ ] Create content types dynamically via API + UI
- [ ] Separate DB table for each content type
- [ ] Validation rules per field
- [ ] Field types:
    - [ ] Boolean
    - [ ] Date
    - [ ] Image
    - [ ] Relation (foreign keys)

---

## 🔐 Upcoming Capabilities

- [ ] Authentication & Role-based access control
- [ ] Dapper-based filtering/sorting for entries
- [ ] File upload & Media Library
- [ ] Multi-language support (i18n for fields and records)
- [ ] Entry versioning / audit history

---