import { useNavigate } from 'react-router-dom';
import { gql } from "@apollo/client";
import { useQuery, useMutation, useLazyQuery } from "@apollo/client/react";
import { useRef, useState, useEffect } from 'react';
import './AddProject.css'

const ADD_PROJECT = gql`
mutation insertProject($inputProject: InputProjectInput!){
  insertProject(inputProject: $inputProject)
}`
const AddProject = () => {
    const navigate = useNavigate();
    const projectFormData = useRef();
    const [addProject] = useMutation(ADD_PROJECT);

    const handleAddProject = async (e) => {
        e.preventDefault();
        const name = projectFormData.current.name.value;
        const description = projectFormData.current.description.value;
        const link = projectFormData.current.link.value;
        const imageUrl = projectFormData.current.imageurl.value;

        const result = await addProject({
            variables: {
                inputProject: {
                    name: name,
                    description: description,
                    link: link,
                    imageUrl: imageUrl
                }
            }
        })

        if (result?.data?.insertProject) {
            alert("Project successfully added!");
        }
        projectFormData.current.name.value = null
        projectFormData.current.description.value = null
        projectFormData.current.link.value = null
        projectFormData.current.imageurl.value = null

    }
    return (<>
        <div className="add-project">
            <div className="add-project-body">
                <div className="add-project-header">
                    <button id="back-btn" onClick={() => navigate(-1) }>back</button>
                </div>
                <div className="add-project-main">
                    <div className="title-container"><h2 className="sloop-script-h2">Add</h2><h1 id="title">Project</h1></div>
                    <form ref={projectFormData} onSubmit={handleAddProject}>
                        <input id="input-project-name" type="text" name="name" placeholder="project name" />
                        <textarea id="project-description" type="text" name="description" placeholder="description" />
                        <input id="project-link" type="text" name="link" placeholder="github link" />
                        <input id="project-imageurl" type="text" name="imageurl" placeholder="image url"/>
                        <button id="submit-btn" type="submit">Submit</button> 
                    </form>
                </div>
            </div>
        </div>
    </>)
}

export default AddProject;