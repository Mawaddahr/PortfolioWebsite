import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter, Router, Routes, Link, Route } from 'react-router-dom'
import App from './App.jsx';
import Projects from './Projects.jsx';
import Resume from './Resume.jsx';

import { ApolloClient, HttpLink, InMemoryCache} from "@apollo/client";
import { ApolloProvider } from "@apollo/client/react";

const client = new ApolloClient({
    link: new HttpLink({ uri: "http://localhost:8081/graphql/" }),
  cache: new InMemoryCache(),
});

ReactDOM.createRoot(document.getElementById('root')).render(
    <BrowserRouter>
        <Routes>
            <Route path="/" element={<ApolloProvider client={client}><App /></ApolloProvider>} />
            <Route path="/Resume" element={<ApolloProvider client={client}><Resume /></ApolloProvider>} />
            <Route path="/Projects" element={<ApolloProvider client={client}><Projects /></ApolloProvider>} />
    </Routes>
     </BrowserRouter >
)
