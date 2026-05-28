import { Link } from 'react-router-dom';
import { useState, useEffect, useRef } from 'react';
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

const UPDATE_EXPERIENCE = gql`
        mutation updateExperience($experience: InputExperienceInput!){
            updateExperience(experience: $experience)
            }
        `;

const DELETE_EXPERIENCE = gql`mutation deleteExperience($id: String!)
  {
      deleteExperience(id: $id)
  }
`;

function EducationData() {
    const {data} = useQuery(GET_EDUCATION);
    const education = data?.allEducation.nodes || [{ institution: "you suck" }];
    return (<>
        <div className="education-list">
        {
            education.map((ed, i) => (
                <div className="education-container" key={i}>
                    <div id="institution">{ed.institution}</div>
                    <div id="studyProgram">{ed.studyProgram}</div>
                    <div id="ed-startDate">{ed.startDate} - {ed.endDate}</div>
                </div>))
            }</div >
        </>);
}

function ExperienceData(deleteButtonClicked, editButtonClicked) {
    const { data } = useQuery(GET_EXPERIENCE);
    const experience = data?.experiences.nodes || [{ company: "you suck" }];
    const [deleteExperience] = useMutation(DELETE_EXPERIENCE);
    const [editExperience] = useMutation(UPDATE_EXPERIENCE);

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
    const educationFormData = useRef();
    const experienceFormData = useRef();
    const [showExForm, setShowExForm] = useState(false);
    const [hideDeleteExButton, setHideDeleteExButton] = useState(false);
    const [hideEditExButton, setHideEditExButton] = useState(false)
    const [insertExperience] = useMutation(INSERT_EXPERIENCE);

    const handleAddExForm = () => {
        showExForm == false ? setShowExForm(true) : setShowExForm(false);
    }

    const handleSetButtonBool = (deleteButton, editButton) =>
    {
        setHideDeleteExButton(deleteButton);
        setHideEditExButton(editButton);
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
    const allExFormHandling = () => {
        handleAddExForm();
        scrollCallback();
    }
    const inputExForm = useRef();
    const scrollCallback = () => {
        inputExForm.current.scrollIntoView({ behavior: 'smooth' })
    }

    return(
        <>
        <div className="resume">
        <div className="resume-body">
            <header>
                <h1 id="resume-title">Resume</h1>
            <nav className="nav-container">
                <Link className="nav-btn" to="/">Home</Link>
                <Link className="nav-btn" to="/AboutMe">About me</Link>
                <Link className="nav-btn" to="/Projects">Projects</Link>
            </nav>
            </header>
                <main className="resume-main">
                <div id="experience-title">Experience</div>
                        <section className="submit-ex-container">
                            {ExperienceData(hideDeleteExButton, hideEditExButton)}
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
                <div id="education-title">Education</div>
                <section className="submit-ed-container">
                    <EducationData />
                    <div className="input-ed">
                    <form>Add education
                        <input id="input-ed-company" type="text" />
                        <input id="input-ed-role" type="text"/>
                        <input id="input-ed-description" type="text" />
                        <input id="input-ed-location" type="text"/>
                        <input id="input-ed-startdate" type="date" />
                        <input id="input-ex-enddate" type="date"/>
                        <button type="submit">Submit</button>
                    </form>
                    </div>
                </section>
            </main>
        </div>
        </div>
        </>
    )
}

export default Resume;