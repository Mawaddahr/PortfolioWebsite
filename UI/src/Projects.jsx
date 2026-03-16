import { Link } from "react-router-dom";
import { useState} from "react";
import "./Projects.css";
import leftArrow from "../img/l-arrow.png";
import rightArrow from "../img/r-arrow.png";

function Projects() {

    const slideshowImages = [
        "../img/slideshow-img1.avf",
        "../img/slideshow-img2.jpg",
        "../img/slideshow-img3.avf"
    ];

    const [nextIndex, setNextIndex] = useState(0);
    const arrayLength = slideshowImages.length;

    function nextSlide() {
        if (nextIndex < arrayLength - 1) {
            setNextIndex(nextIndex + 1);
        }
        else {
            setNextIndex(0);
        }
    }

    function previousSlide() {
        if (nextIndex > 0) {
            setNextIndex(nextIndex - 1);
        }
        else {
            setNextIndex(arrayLength - 1);
        }
    }
    return (
        <>
            <header>
                <div class="nav-container">
                    <Link className="nav-btn" to="/">Home</Link>
                    <Link className="nav-btn" to="/">About me</Link>
                    <Link className="nav-btn" to="/Resume">Resumé</Link>
                </div>
            </header>
            <main className="project-main">
                <div className="project-container">
                    <h2 className="sloop-script-h2">my</h2><h1 id="title">Projects</h1>
                </div>
                <div className="slideshow-container">
                    <img className="left-arrow" onClick={previousSlide} src={leftArrow}/>
                    <img className="slideshow-img" src={slideshowImages[nextIndex]} alt="project screenshot" />
                    <img className="right-arrow" onClick={nextSlide} src={rightArrow} />
                </div>
            </main>
        </>
    )
}

export default Projects;