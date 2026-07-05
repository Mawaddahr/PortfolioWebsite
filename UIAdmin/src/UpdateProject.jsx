import { useParams, useNavigate } from "react-router-dom";
import { gql } from "@apollo/client";
import { useQuery, useMutation, useLazyQuery } from "@apollo/client/react";
import { useRef, useState, useEffect } from 'react';
import './AddProject.css'

const GET_PROJECT = gql`
query projectById($id: String!){
    projectById(id: $id)
    {
        name,
        description,
        link,
        imageUrl
    }
}`;

const UPDATE_PROJECT = gql`
mutation updateProject($project: ProjectInput!){
  updateProject(project: $project)
}`;

const UpdateProject = () => {
    const navigate = useNavigate();
    const projectFormData = useRef();
    const [editProject] = useMutation(UPDATE_PROJECT);
    const { id } = useParams({ id: String });
    const { data } = useQuery(GET_PROJECT, {
        variables: {
            id: id
        }
    });

    const [name, setName] = useState();
    const [description, setDescription] = useState();
    const [link, setLink] = useState();
    const [imageUrl, setImageUrl] = useState();

    useEffect(() => {
        if (data?.projectById) {
            setName(data.projectById.name);
            setLink(data.projectById.link);
            setDescription(data.projectById.description);
            setImageUrl(data.projectById.imageUrl);
        }
    }, [data]);

    const handleAddProject = async (e) => {
        e.preventDefault();
        const name = projectFormData.current.name.value;
        const description = projectFormData.current.description.value;
        const link = projectFormData.current.link.value;
        const imageUrl = projectFormData.current.imageurl.value;

        const result = await editProject({
            variables: {
                project: {
                    id: id,
                    name: name,
                    description: description,
                    link: link,
                    imageUrl: imageUrl
                }
            }
        })

        if (result?.data?.updateProject) {
            alert("Project successfully updated!")
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
                    <button id="back-btn" onClick={() => navigate(-1)}>back</button>
                </div>
                <div className="add-project-main">
                    <div className="title-container"><h2 className="sloop-script-h2">Edit</h2><h1 id="title">Project</h1></div>
                    <form ref={projectFormData} onSubmit={handleAddProject}>
                        <input id="edit-project-name" type="text" name="name" value={name} onChange={(e) => setName(e.currentTarget.value)} placeholder="project name" />
                        <textarea id="project-description" type="text" name="description" value={description} onChange={ (e) => setDescription(e.currentTarget.value)} placeholder="description" />
                        <input id="project-link" type="text" name="link" value={link} onChange={(e) => setLink(e.currentTarget.value)} placeholder="github link" />
                        <input id="project-imageurl" type="text" name="imageurl" value={imageUrl} onChange={(e) => setImageUrl(e.currentTarget.value)} placeholder="image url" />
                        <button id="submit-btn" type="submit">Submit</button>
                    </form>
                </div>
            </div>
        </div>
    </>)
}

export default UpdateProject;