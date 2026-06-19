import React from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter, Routes, Route } from 'react-router-dom'
import App from './App.jsx'
import Resume from './Resume.jsx'
import AboutMe from './AboutMe.jsx'
import Projects from './Projects.jsx'
import LogIn from './LogIn.jsx'

import { ApolloClient, HttpLink, InMemoryCache } from "@apollo/client";
import { ApolloProvider } from "@apollo/client/react";
import UpdateExperience from './UpdateExperience.jsx';
import UpdateEducation from './UpdateEducation.jsx';
import AddProject from './AddProject.jsx'
import UpdateProject from './UpdateProject.jsx'
import ProtectedRoute from './ProtectedRoutes.jsx'


createRoot(document.getElementById('root')).render(
    <BrowserRouter>
        <App />
    </BrowserRouter>
)
