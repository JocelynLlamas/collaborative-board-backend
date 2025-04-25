# 🧠 Collaborative Board - Backend (ASP.NET Core)

This is the backend for a real-time collaborative whiteboard built with **ASP.NET Core**, **SignalR**, and **MessagePack**. It powers a shared drawing canvas, sticky notes, connected users list, and real-time updates between multiple clients.

---

## 🚀 Features

- 🎨 Real-time drawing synchronization
- 📝 Sticky notes with live updates and drag-and-drop positioning
- 👥 Connected users tracking
- 🕘 Action history broadcasting (user joined, drew, added notes, etc.)
- ⚡ Uses **SignalR** with **MessagePack** for high-performance message exchange

---

## 🛠️ Tech Stack

- **.NET 8**
- **SignalR** (real-time communication)
- **MessagePack** (efficient binary serialization)
- **CORS** support for frontend integration (Angular)
- In-memory state storage (users, notes, actions)
