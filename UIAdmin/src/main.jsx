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

const client = new ApolloClient({
    link: new HttpLink({ uri: "http://localhost:5142/graphql/" }),
    cache: new InMemoryCache(),
});

createRoot(document.getElementById('root')).render(
    <BrowserRouter>
        <Routes>
            <Route path="/" element={<App />} />
            <Route path="/LogIn" element={<LogIn />} />
            <Route path="/AboutMe" element={<ApolloProvider client={client}><AboutMe /></ApolloProvider>} />
            <Route path="/Resume" element={<ApolloProvider client= {client}><Resume/></ApolloProvider>}/>
            <Route path="/Projects" element={<ApolloProvider client={client}><Projects /></ApolloProvider>} />
            <Route path="/update_experience/:id" element={<ApolloProvider client= {client} ><UpdateExperience/></ApolloProvider> }/>
            <Route path="/update_education/:id" element={<ApolloProvider client={client} ><UpdateEducation /></ApolloProvider>} />
            <Route path="/add_project/:id" element={<ApolloProvider client={client} ><AddProject /></ApolloProvider>} />
            <Route path="/update_project/:id" element={<ApolloProvider client={client} ><UpdateProject /></ApolloProvider>} />

        </Routes>
    </BrowserRouter >
)
