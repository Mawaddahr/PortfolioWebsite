import './App.css'
import { Link } from 'react-router-dom';
function App() {

    return (<>
        <div className="app">
        <div className='app-body'>
        <header className="app-header">
            <nav className="app-nav-container">
                <Link className="app-nav-btn" to="/AboutMe">About me</Link>
                <Link className="app-nav-btn" to="/Resume">Resumé</Link>
                <Link className="app-nav-btn" to="/Projects">Projects</Link>
            </nav>
        </header>
        <main className="homepage-main">
        <h1 id= "website-title"> Admin panel</h1>
            <div id="website-title2">mawadda-alkhorchani.nl</div>
        </main>
        </div>
        </div>
    </>)
}
export default App
