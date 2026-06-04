import { Link } from 'react-router-dom';
import { useState, useEffect, useRef } from 'react';
import { useNavigate } from "react-router-dom";
import { gql } from "@apollo/client";
import { useQuery, useMutation } from "@apollo/client/react";
import './Resume.css';

const GET_EDUCATION = gql`
query{
    allEducation(last: 3)
    {
        nodes
        {
            id,
            institution,
            studyProgram,
            startDate,
            endDate
        }
    }
}`;

const INSERT_EDUCATION = gql`
mutation 
  insertEducation($inputEducation: InputEducationInput!) {
     insertEducation(inputEducation: $inputEducation)
}`;

const DELETE_EDUCATION = gql`
mutation deleteEducation($id: String!){
    deleteEducation(id: $id)
  }`;

const GET_EXPERIENCE = gql`
query{
    experiences(last: 3)
    {
        nodes
        {
            id,
            company,
            role,
            description,
            startDate,
            endDate,
            location
        }
    }
}`;


const INSERT_EXPERIENCE = gql`
        mutation insertExperience($experience: InputExperienceInput!) {
            insertExperience(experience: $experience)
        }
    `;

const DELETE_EXPERIENCE = gql`mutation deleteExperience($id: String!)
  {
      deleteExperience(id: $id)
  }
`;

function EducationData(deleteButtonClicked, editButtonClicked) {
    const {data} = useQuery(GET_EDUCATION);
    const education = data?.allEducation.nodes || [{ institution: "you suck" }];
    const [deleteEducation] = useMutation(DELETE_EDUCATION);
    const navigate = useNavigate();

    async function handleDeleteEd(id) {
        if (window.confirm("Are you sure you want to delete this education?")) {
            const { errors, data } = await deleteEducation({
                variables: {
                    id: id
                }
            });

            alert(errors ? errors[0].message : data.deleteEducation);
        }
    }

    async function handleUpdateEd(id) {
        navigate(`/update_education/${id}`)
    }

    return (<>
        <div className="education-list">
            {
                education.map((ed, i) =>
                    !deleteButtonClicked && !editButtonClicked ? (
                        <div className="education-container" key={ed.id ?? i}>
                            <div id="institution">{ed.institution}</div>
                            <div id="study-program">{ed.studyProgram}</div>
                            <div id="ed-endDate">
                                {ed.startDate} - {ed.endDate}
                            </div>
                        </div>
                    ) : (
                        <button
                            className="education-button"
                            key={ed.id ?? i}
                            onClick={() => deleteButtonClicked ? handleDeleteEd(ed.id) : handleUpdateEd(ed.id)}
                            >
                                <div id="institution">{ed.institution}</div>
                            <div id="study-program">{ed.studyProgram}</div>
                            <div id="ed-endDate">
                                {ed.startDate} - {ed.endDate}
                            </div>
                        </button>
                    )
                )
            }</div></>)
}

function ExperienceData(deleteButtonClicked, editButtonClicked) {
    const { data } = useQuery(GET_EXPERIENCE);
    const experience = data?.experiences.nodes || [{ company: "you suck" }];
    const [deleteExperience] = useMutation(DELETE_EXPERIENCE);
    const navigate = useNavigate();

    async function handleDeleteEx(id) {
        if (window.confirm("Are you sure you want to delete this experience?")) {
            const { errors, data } = await deleteExperience({
                variables: {
                    id: id
                }
            });

            alert(errors ? errors[0].message : data.deleteExperience);
        }
    }

    async function handleUpdateEx(id) {
        navigate(`/update_experience/${id}`)
    }
    return (<>
        <div className="experience-list">
            {
                experience.map((ex, i) =>
                    !deleteButtonClicked && !editButtonClicked ? (
                        <div className="experience-container" key={ex.id ?? i}>
                            <div id="company">{ex.company}</div>
                            <div id="role">{ex.role}</div>
                            <div id="ex-endDate">
                                {ex.startDate} - {ex.endDate}
                            </div>
                        </div>
                    ) : (
                        <button
                                className="experience-button"
                                key={ex.id ?? i}
                                onClick={() => deleteButtonClicked ? handleDeleteEx(ex.id) : handleUpdateEx(ex.id)}
                            >
                                <div id="company">{ex.company}</div>
                                <div id="role">{ex.role}</div>
                                <div id="ex-endDate">
                                    {ex.startDate} - {ex.endDate}
                                    </div>
                        </button>
                    )
                )
            }</div></>)}
function Resume() {
    const experienceFormData = useRef();
    const [showExForm, setShowExForm] = useState(false);
    const [hideDeleteExButton, setHideDeleteExButton] = useState(false);
    const [hideEditExButton, setHideEditExButton] = useState(false);
    const [insertExperience] = useMutation(INSERT_EXPERIENCE);

    const educationFormData = useRef();
    const [showEdForm, setShowEdForm] = useState(false);
    const [hideDeleteEdButton, setHideDeleteEdButton] = useState(false);
    const [hideEditEdButton, setHideEditEdButton] = useState(false);
    const [insertEducation] = useMutation(INSERT_EDUCATION);



    const handleAddExForm = () => {
        showExForm == false ? setShowExForm(true) : setShowExForm(false);
    }

    const handleAddEdForm = () => {
        showEdForm == false ? setShowEdForm(true) : setShowEdForm(false);
    }

    const handleSetButtonBool = (deleteButton, editButton) =>
    {
        setHideDeleteExButton(deleteButton);
        setHideEditExButton(editButton);
    }

    const handleSetEdButtonBool = (deleteExButton, editEdButton) => {
        setHideDeleteEdButton(deleteExButton);
        setHideEditEdButton(editEdButton);
    }


    const handleSubmitExperience = async (e) => {
        e.preventDefault();
        const company = experienceFormData.current.company.value
        const role = experienceFormData.current.role.value
        const description = experienceFormData.current.description.value
        const startDate = experienceFormData.current.startdate.value
        const endDate = experienceFormData.current.enddate.value
        const location = experienceFormData.current.location.value
        alert("You submitted the form, yay!");

        await insertExperience({
            variables: {
                experience: {
                    company: company,
                    role: role,
                    description: description,
                    startDate: startDate,
                    endDate: endDate,
                    location: location
                }
            }
        });

        experienceFormData.current.company.value = null;
        experienceFormData.current.role.value = null;
        experienceFormData.current.description.value = null;
        experienceFormData.current.startdate.value = null;
        experienceFormData.current.enddate.value = null;
        experienceFormData.current.location.value = null;

        setShowExForm(false);
    }

    const handleSubmitEducation = async (e) => {
        e.preventDefault();
        const institution = educationFormData.current.institution.value
        const studyProgram = educationFormData.current.studyProgram.value
        const studyProgramType = educationFormData.current.studyProgramType.value
        const startDate = educationFormData.current.startDate.value
        const endDate = educationFormData.current.endDate.value
        const onGoing = educationFormData.current.onGoing.value === "true"
        alert("You submitted the form, yay!");

        await insertEducation({
            variables: {
                inputEducation: {
                    institution: institution,
                    studyProgram: studyProgram,
                    studyProgramType: studyProgramType,
                    startDate: startDate,
                    endDate: endDate,
                    onGoing: onGoing
                }
            }
        });

        educationFormData.current.institution.value = null;
        educationFormData.current.studyProgram.value = null;
        educationFormData.current.studyProgramType.value = null;
        educationFormData.current.startDate.value = null;
        educationFormData.current.endDate.value = null;
        educationFormData.current.onGoing.value = null;

        setShowEdForm(false);
    }
    const allExFormHandling = () => {
        handleAddExForm();
        scrollCallback();
    }

    const allEdFormHandling = () => {
        handleAddEdForm();
        scrollCallEdBack();
    }
    const inputExForm = useRef();
    const inputEdForm = useRef();

    const scrollCallEdBack = () => {
        inputEdForm.current.scrollIntoView({behavior: 'smooth'})
    }
    const scrollCallback = () => {
        inputExForm.current.scrollIntoView({ behavior: 'smooth' })
    }

    return(
        <>
        <div className="resume">
            <header className="resume-header">
                <h1 id="resume-title">Resume</h1>
                <nav className="resume-nav-container">
                    <Link className="resume-nav-btn" to="/">Home</Link>
                    <Link className="resume-nav-btn" to="/AboutMe">About me</Link>
                    <Link className="resume-nav-btn" to="/Projects">Projects</Link>
                </nav>
            </header>
        <div className="resume-body">
                <main className="resume-main">
                <div id="experience-title">Experience</div>
                <div className= "ex-container">
                        <div className= "ex-data-container">{ExperienceData(hideDeleteExButton, hideEditExButton)}</div>
                        <section className="submit-ex-container">
                    <div className="input-ex" ref={inputExForm}>
                        {showExForm == false ? <button id="add-exform-btn" type="button" onClick={allExFormHandling}>Add Experience</button> : <button id="hide-exform-btn" type="button" onClick={handleAddExForm}>Cancel</button>}
                        {showExForm == true ?
                            <form ref={experienceFormData} onSubmit={handleSubmitExperience}>
                                <input id="input-ex-company" name="company" type="text" placeholder="Company" />
                                <input id="input-ex-role" name="role" type="text" placeholder="Role" />
                                <input id="input-ex-location" name="location" type="text" placeholder="Location" />
                                <textarea id="input-ex-description" name="description" type="text" placeholder="Description" />
                                <input id="input-ex-startdate" name="startdate" type="date" placeholder="start date" />
                                <input id="input-ex-enddate" name="enddate" type="date" placeholder="(estimated) end date" />
                                <button id="submit-exform-btn" type="submit">Submit</button>
                            </form> : null}
                            </div>
                            {hideDeleteExButton == false ? <button id="del-ex-btn" type="button" onClick={() => handleSetButtonBool(true, false)}>Delete Experience</button> : <button id="cancel-del-ex-btn" type="button" onClick={() => setHideDeleteExButton(false)}>cancel</button>}
                            {hideEditExButton == false ? <button id="edit-ex-btn" type="button" onClick={() => handleSetButtonBool(false, true)}>Edit Experience</button> : <button id="cancel-edit-ex-btn" type="button" onClick={() => setHideEditExButton(false)}>cancel</button>}

                            </section>
                   </div>
                    <div id="education-title">Education</div>
                    <div className="ed-container">
                    <div className="ed-data-container">{EducationData(hideDeleteEdButton, hideEditEdButton)}</div>
                        <section className="submit-ed-container">
                            <div className="input-ed" ref={inputEdForm}>
                                {showEdForm == false ? <button id="add-exform-btn" type="button" onClick={allEdFormHandling}>Add Education</button> : <button id="hide-edform-btn" type="button" onClick={handleAddEdForm}>Cancel</button>}
                                {showEdForm == true ?
                                    <form ref={educationFormData} onSubmit={handleSubmitEducation}>
                                        <input id="input-ed-institution" name="institution" type="text" placeholder="Institution" />
                                        <input id="input-ed-studyprogram" name="studyProgram" type="text" placeholder="Studyprogram" />
                                        <input id="input-ed-studyprogramtype" name="studyProgramType" type="text" placeholder="studyprogram type" />
                                        <div id="ongoing-title">On going</div>
                                            <input
                                                id="input-ed-ongoing-true"
                                                type="radio"
                                                name="onGoing"
                                                value="true"
                                                onChange={() => {
                                                    educationFormData.current.onGoing.value = true;
                                                }}
                                            />
                                            <label for="input-ed-ongoing-true">True</label>

                                            <input
                                                id="input-ed-ongoing-false"
                                                type="radio"
                                                name="onGoing"
                                                value="false"
                                                onChange={() => {
                                                    educationFormData.current.onGoing.value = false;
                                                }}
                                            />
                                            <label for="input-ed-ongoing-false">False</label>
                                        <input id="input-ed-startdate" name="startDate" type="date" placeholder="start date" />
                                        <input id="input-ed-enddate" name="endDate" type="date" placeholder="(estimated) end date" />
                                        <button id="submit-edform-btn" type="submit">Submit</button>
                                    </form> : null}
                            </div>
                            {hideDeleteEdButton == false ? <button id="del-ed-btn" type="button" onClick={() => handleSetEdButtonBool(true, false)}>Delete Education</button> : <button id="cancel-del-ed-btn" type="button" onClick={() => setHideDeleteEdButton(false)}>cancel</button>}
                            {hideEditEdButton == false ? <button id="edit-ed-btn" type="button" onClick={() => handleSetEdButtonBool(false, true)}>Edit Education</button> : <button id="cancel-edit-ed-btn" type="button" onClick={() => setHideEditEdButton(false)}>cancel</button>}
                            </section>
                    </div>
            </main>
        </div>
        </div>
        </>
    )
}

export default Resume;