import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter, Router, Routes, Link, Route } from 'react-router-dom'
import App from './App.jsx';
import Projects from './Projects.jsx';
import Resume from './Resume.jsx';

ReactDOM.createRoot(document.getElementById('root')).render(
    <BrowserRouter>
    <Routes>
        <Route path="/" element={<App />} />
        <Route path="/Resume" element={<Resume />} />
        <Route path="/Projects" element={<Projects />} />
    </Routes>
     </BrowserRouter >
)
