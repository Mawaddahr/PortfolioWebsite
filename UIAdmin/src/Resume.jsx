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
            company,
            role,
            startDate,
            endDate
        }
    }
}`;
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

function ExperienceData() {
    const { data } = useQuery(GET_EXPERIENCE);
    const experience = data?.experiences.nodes || [{ company: "you suck" }];
    return (<>
        <div className="experience-list">
        {
            experience.map((ex, i) => (
                <div className="experience-container" key={i }>
                    <div id="company">{ex.company}</div>
                    <div id="role">{ex.role}</div>
                    <div id="ex-endDate">{ex.startDate} - {ex.endDate}</div>
                </div>))
        }</div></>);
}
function Resume() {
    const educationFormData = useRef();
    const experienceFormData = useRef();

    const handleSubmitExperience = (e) => {
        e.preventDefault();
        const company = experienceFormData.current.company.value
        const role = experienceFormData.current.role.value
        const description = experienceFormData.current.description.value
        const startDate = experienceFormData.current.startdate.value
        const endDate = experienceFormData.current.enddate.value
        const location = experienceFormData.current.location.value
        alert("You submitted the form, yay!");
        console.log(company, role, description, startDate, endDate, location);
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
                    <ExperienceData />
                            <div className="input-ex">
                                <form ref={experienceFormData} onSubmit={handleSubmitExperience}>Add experience
                                <input id="input-ex-company" name="company" type="text" placeholder="Company" />
                                <input id="input-ex-role" name="role" type="text" placeholder="Role"/>
                                <input id="input-ex-location" name="location" type="text" placeholder="Location"/>
                                <input id="input-ex-description" name="description" type="text" placeholder="Description"/>
                                <input id="input-ex-startdate" name="startdate" type="date" placeholder="start date"/>
                                <input id="input-ex-enddate" name="enddate" type="date" placeholder="(estimated) end date"/>
                                <button type="submit">Submit</button>
                            </form>
                            </div>
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