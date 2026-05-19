import { Link } from 'react-router-dom';
import { useState, useEffect, useRef } from 'react';
import { gql } from "@apollo/client";
import { useQuery, useMutation } from "@apollo/client/react";
import './Resume.css';

const GET_EDUCATION = gql`
query{
    allEducation(last: 5)
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
    experiences(last: 5)
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
        <div id="education-title">Education</div>
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
        <div id="experience-title">Experience</div>
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
                        <EducationData />
                        <ExperienceData />
            </main>
        </div>
        </div>
        </>
    )
}

export default Resume;