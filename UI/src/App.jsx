import {Link} from 'react-router-dom';
import { useRef } from 'react';
import './App.css';
import { gql } from "@apollo/client";
import { useQuery } from "@apollo/client/react";


function GetAbtMeText() {
    const { loading, error, data } = useQuery(GETABTMETEXT);
    const abtMeText = data?.readAbtMeText || {text: '', imageUrl: ''};

    return {abtMeText, loading, error};
}

function App() {
    const aboutRef = useRef(null);
    const scrollToAbout = () => {
        aboutRef.current.scrollIntoView({ behavior: 'smooth' });
    };
    const { abtMeText, loading, error } = GetAbtMeText();

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error : {error.message}</p>;

    return (
      <>
        <header className="home-header">
            <nav className="nav-container-home">
                    <button className="nav-btn" onClick={scrollToAbout}>About me</button>
                <Link className="nav-btn" to="/Resume">Resumé</Link>
                <Link className="nav-btn" to="/Projects">Projects</Link>
            </nav>
        </header>
            <main className="homepage-main">
                <h1 id="website-title">Mawadda's</h1>
                <h2 id="website-title2">development journey website</h2>
            </main>
            <section className="home-section" ref={aboutRef}>
                <div className="tag-row">
                <h2 id="about">About</h2>
                <h2 id="about2">me</h2>
            </div>
             <div className="about-me-content">
                    <div id= "about-me-text">{abtMeText.text}</div>
                    <img id="about-me-img" src={abtMeText.imageUrl} alt="about me image" />
             </div>
        </section>
        <footer>
            <div className="contact-row">
                <h2 id="contact">Contact</h2>
                <h2 id="contact2">me</h2>
            </div>
            <div className="contact-container">
                <a className="contact-btn" href="https://www.linkedin.com/in/mawadda-al-khorchani-3b4a55348/" target="_blank">LinkedIn</a>
                <a className="contact-btn" href="https://github.com/Mawaddahr" target="_blank">GitHub</a>
                <a className="contact-btn" href="mailto:alkhorchanimawadda@gmail.com">Email</a>
                <a className="contact-btn" href="tel:0683661885">Phone</a>
            </div>
          </footer>
          </>
  )
}

export default App;

const GETABTMETEXT = gql`query{
  readAbtMeText
  {
    text,
    imageUrl
  }
}`;