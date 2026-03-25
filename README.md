# 🏨 HotelMind: AI-Powered Travel Assistant

> **A smart, context-aware travel companion that prepares you for your journey using Real-Time Data and Local AI.**

This project was developed at **The Paisii Hilendarski University of Plovdiv** for the **Software Quality Assurance (Q.A.)** course.

## 🤖 Local AI Configuration

Unlike standard cloud-based apps, HotelMind is designed to run with **Local AI** to ensure privacy and customizability.

* **Supported Engines:** [Ollama](https://ollama.com/) or [LM Studio](https://lmstudio.ai/).
* **Model Requirements:** The selected AI model **must support Tool Calling / Function Calling** (e.g., `mistral-nemo`, `qwen3.5:4b`, or `llama3-groq-tool-use`) to interact with the search and weather APIs.
* **Endpoint:** Ensure your local server is running (usually `localhost:11434` for Ollama) so the Semantic Kernel can orchestrate the requests.


## 🌟 Key Features

### 🔍 Smart Hotel Discovery
Find stays within your specific budget. The AI searches the web in real-time to find current prices and availability, summarizing the best options for you.

### 🌤️ Dynamic Climate Dashboard
Get a live 7-day forecast for your destination. It doesn't just show numbers; it uses current conditions to power the rest of the app's intelligence.

### 🧳 Intelligent Packing Assistant
The AI analyzes the **High** and **Low** temperatures of your destination to suggest the perfect gear.

### ✨ Local Vibe & Activities
Get an AI-generated "feel" for the city. Discover the local atmosphere and top-rated activities summarized in a clean, readable widget.


## 🚀 Tech Stack

| Layer | Technology |
| :--- | :--- |
| **Backend** | ASP.NET Core MVC (C#) |
| **AI Orchestration** | Microsoft Semantic Kernel |
| **Local AI** | Ollama / LM Studio |
| **Search API** | Serper.dev (Google Search) |
| **Weather** | WeatherAPI.com |
| **Frontend** | CSS3 Glass-morphism, JavaScript, Bootstrap 5 |


## 📸 Screenshots

### **Register**
<p float="left">
  <img width="30%" alt="Regiser Light" src="https://github.com/user-attachments/assets/2c513cd1-6d5b-4e1b-8d68-15e5fd43ac5d" />
  <img width="30%" alt="Register Dark" src="https://github.com/user-attachments/assets/37afc896-e67d-40aa-a974-96b2a155a14c" />
</p>

### **Login**
<p float="left">
  <img width="30%" alt="Login Light" src="https://github.com/user-attachments/assets/16792664-6271-4a3a-8065-8fa4661436b8" />
  <img width="30%" alt="Login Dark" src="https://github.com/user-attachments/assets/d7d6fe03-d717-4c07-92d6-4e96ce597714" />
</p>

### **Main Menu**
<p float="left">
  <img width="30%" alt="Main Page Light" src="https://github.com/user-attachments/assets/d5874271-70fc-432a-af29-52c312334e3e" />
  <img width="30%" alt="Main Page Dark" src="https://github.com/user-attachments/assets/d8fa88d8-8caa-48b8-8f23-81625b0d7c89" />
</p>

### **The AI Insights Dashboard**
<p float="left">
  <img width="30%" alt="AI Light" src="https://github.com/user-attachments/assets/568575d7-271d-4c0c-a486-7956ad452736" />
  <img width="30%" alt="AI Dark" src="https://github.com/user-attachments/assets/168d29fb-4f14-4bae-ad79-36f7c2d53386" />
</p>
