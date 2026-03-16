import {Link} from 'react-router-dom';
import { useRef } from 'react';
import './App.css';


function App() {
    const aboutRef = useRef(null);
    const scrollToAbout = () => {
        aboutRef.current.scrollIntoView({behavior: 'smooth'});
    };
    return (
      <>
        <header>
            <nav className="nav-container">
                    <button className="nav-btn" onClick={scrollToAbout}>About me</button>
                <Link className="nav-btn" to="/Resume">Resumé</Link>
                <Link className="nav-btn" to="/Projects">Projects</Link>
            </nav>
        </header>
            <main className="homepage-main">
                <h1 id="website-title">Mawadda's</h1>
                <h2 id="website-title2">development journey website</h2>
            </main>
            <section ref={aboutRef}>
                <div className="tag-row">
                <h2 id="about">About</h2>
                <h2 id="about2">me</h2>
            </div>
            <p className="about-me-text">
                {/*I'm Mawadda Al Khorchani, a 19-year-old third year computer science student at the Rotterdam University of Applied Sciences. I have chosen this major because as a highschool student I had a great fascination in engineering, tech and programming. A lot of people in tech at the time warned me that I should not choose computer science unless I genuinely enjoy it.*/}
                {/*<br>*/}
                {/*<br>*/}
                {/*I was taught the basics and I indeed found out through a series of events regarding my studies and personal life, that if I wanted to succeed as a software developer, I have to find my passion, interest and joy in what I do. After being lost for some time, I decided to start my personal software development jourey to improve my technical skills and find that passion. This website is my first hobby project and start of a journey to succeed in tech.*/}
                {/*<br>*/}
                {/*<br>*/}
                {/*I am excited and I hope you, dear reader, will enjoy following my journey through this website. I will describe my growth and lessons thrugh updates on this webiste. Let's see what the future has for me!*/}
                {/*<br>*/}
                {/*<br>*/}
                {/*PS: I am open to any feedback and suggestions, so please do not hesitate to contact me through the contact section of this website.*/}
                {/*<br>*/}
                {/*<br>*/}
                {/*PS2: Everything on the website is entirely made by me. I value authenticity nad uniqueness, so I have not used any templates or pre-made code for this website. I have designed and coded everything from scratch, which has been a great learning experience for me.*/}
                {/*<br>*/}
                {/*<br>*/}
                {/*<br>*/}
                {/*Mawadda, 1-3-2026.*/}
            </p>
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
