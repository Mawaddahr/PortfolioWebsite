import './App.css'
import { useState } from 'react'
import { Link } from 'react-router-dom';
import { Routes, Route } from 'react-router-dom'
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
import useLogInWithGoogle from './Auth.jsx';

const client = new ApolloClient({
    link: new HttpLink({ uri: "http://localhost:5142/graphql/" }),
    cache: new InMemoryCache(),
});

function Home({SignIn}) {
    return (
        <div className='app-body'>
            <header className="app-header">
                <nav className="app-nav-container">
                    <button className="app-nav-btn" onClick={SignIn}>
                        Sign in
                    </button>

                    <Link className="app-nav-btn" to="/AboutMe">About me</Link>
                    <Link className="app-nav-btn" to="/Resume">Resumé</Link>
                    <Link className="app-nav-btn" to="/Projects">Projects</Link>
                </nav>
            </header>

            <main className="homepage-main">
                <h1 id="website-title">Admin panel</h1>
                <div id="website-title2">mawadda-alkhorchani.nl</div>
            </main>
        </div>
    );
}
function App() {
    const [isAuth, SignIn] = useLogInWithGoogle(false);
    return(<>
            <Routes>
            <Route path="/" element={<Home SignIn={SignIn} />}/>
                <Route element={<ProtectedRoute auth={isAuth} />}>
                    <Route path="/AboutMe" element={<ApolloProvider client={client}><AboutMe /></ApolloProvider>} />
                    <Route path="/Resume" element={<ApolloProvider client={client}><Resume /></ApolloProvider>} />
                    <Route path="/Projects" element={<ApolloProvider client={client}><Projects /></ApolloProvider>} />
                    <Route path="/update_experience/:id" element={<ApolloProvider client={client} ><UpdateExperience /></ApolloProvider>} />
                    <Route path="/update_education/:id" element={<ApolloProvider client={client} ><UpdateEducation /></ApolloProvider>} />
                    <Route path="/add_project" element={<ApolloProvider client={client} ><AddProject /></ApolloProvider>} />
                    <Route path="/edit_project/:id" element={<ApolloProvider client={client} ><UpdateProject /></ApolloProvider>} />
                </Route>
            </Routes>
    </>)
}
export default App
