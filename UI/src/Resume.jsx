import "./resume.css";
import { Link } from 'react-router-dom';

function Resume() {
    return(
    <>
        <header>
            <div className="nav-container">
                    <Link className="nav-btn" to="/">Home</Link>
                <Link className="nav-btn" to="/Projects">Projects</Link>
            </div>
        </header>
        <div className="resume-container">
            <h2 className="sloop-script-h2">my</h2><h1 id="title">Resumé</h1><h2 className="sloop-script-h2">so far</h2>
        </div>
    </>
    )
}

export default Resume;