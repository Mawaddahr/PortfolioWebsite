import { Link } from "react-router-dom";
import { useState, useEffect } from "react";
import "./Projects.css";
import leftArrow from "../img/l-arrow.png";
import rightArrow from "../img/r-arrow.png";
import { gql } from "@apollo/client";
import { useQuery } from "@apollo/client/react";

const GET_PROJECTS = gql`
{
  projects
  {
    nodes
    {
      name,
      description,
      link,
      imageUrl
    }
  }
}
`;

function DisplayProjects() {
    const { loading, error, data } = useQuery(GET_PROJECTS);
    const projects = data?.projects?.nodes || [];

    return { projects, loading, error };
}

function Projects() {
    const [addButtonClicked, setAddButtonClicked] = useState(false)
    const [editButtonClicked, setEditButtonClicked] = useState(false)
    const [deleteButtonClicked, setDeleteButtonClicked] = useState(false)
    const { projects, loading, error } = DisplayProjects();
    const [index, setIndex] = useState(0);
    const arrayLength = projects.length;

    const handleButtonsClicked = (add, edit, deleteB) => {
        if (add) {
            setAddButtonClicked(true);
            setEditButtonClicked(false);
            setDeleteButtonClicked(false);
        }
        else if (deleteB) {
            setDeleteButtonClicked(true);
            setAddButtonClicked(false);
            setEditButtonClicked(false);
        }

        else if (edit) {
            setEditButtonClicked(true);
            setAddButtonClicked(false);
            setDeleteButtonClicked(false);
        }
    }

    useEffect(() => {
        if (index >= arrayLength) {
            // eslint-disable-next-line react-hooks/set-state-in-effect
            setIndex(0);
        }
    }, [arrayLength, index]);

    function nextSlide() {
        setIndex(prev => (prev < arrayLength - 1 ? prev + 1 : 0));
    }

    function previousSlide() {
        setIndex(prev => (prev > 0 ? prev - 1 : arrayLength - 1));
    }

    if (loading) return <div>Loading...</div>;
    if (error) return <div>Error {error.message}</div>;
    if (arrayLength === 0) return <div>No projects found.</div>;

    return (
        <>
            <header>
                <div className="nav-container">
                    <Link className="nav-btn" to="/">Home</Link>
                    <Link className="nav-btn" to="/AboutMe">About me</Link>
                    <Link className="nav-btn" to="/Resume">Resumé</Link>
                </div>
            </header>
            <div className= "project-body">
            <main className="project-main">
                <div className="project-container">
                    <h2 className="sloop-script-h2">my</h2><h1 id="title">Projects</h1>
                    <div className="edit-btns">
                            {addButtonClicked == false ? <button id="add-project-btn" onClick={() => handleButtonsClicked(true, false, false)}>Add Project</button> :
                                <button id="add-project-btn" onClick={() => setAddButtonClicked(false)}>Cancel</button>}
                            {editButtonClicked == false ? <button id="edit-project-btn" onClick={() => handleButtonsClicked(false, true, false)}>Edit Project</button> :
                                <button id="edit-project-btn" onClick={() => setEditButtonClicked(false)}>Cancel</button>}
                            {deleteButtonClicked == false ? <button id="delete-project-btn" onClick={() => handleButtonsClicked(false, false, true)}>Delete Project</button> :
                                <button id="delete-project-btn" onClick={() => setDeleteButtonClicked(false)}>Cancel</button> }
                    </div>
                </div>
                <div className="slideshow-container">
                    <img className="left-arrow" onClick={previousSlide} src={leftArrow} />
                        <button id="slideshow-img-btn" onClick={() => alert("Are you sure you want to edit this project?")} target="_blank">
                        <img className="slideshow-img" src={projects[index].imageUrl} alt="project image" />
                    </button>
                    <img className="right-arrow" onClick={nextSlide} src={rightArrow} />
                </div>
                <div id="project-name">{projects[index].name}</div>
                <div id="instruction-text">Click to edit project!</div>
                </main>
            </div>
        </>
    )
}

export default Projects;