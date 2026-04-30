import "./resume.css";
import { Link } from 'react-router-dom';
import { gql } from "@apollo/client";
import { useQuery } from "@apollo/client/react";

function DisplayExperiences() {
    const { loading, error, data } = useQuery(GET_EXPERIENCES);

    if (loading) return <p>loading</p>;
    if (error) return <p>{error.message}</p>;

    const experiencelist = data?.experiences.nodes || [];
    return (
        <>
            {experiencelist.map(({ id, company, role, startDate, endDate }, index) => (
                <div className="experience-item" key={id ?? index}>
                    <div className="experience-main">
                        <strong className= "experience-company">{company}</strong>
                        <div className="experience-role">{role}</div>
                        <div className="experience-dates">{startDate} — {endDate}</div>
                    </div>
                </div>
            ))}
        </>
    );
}

function DisplayEducation() {
    const { loading, error, data } = useQuery(GET_EDUCATION);

    if (loading) return <p>loading</p>;
    if (error) return <p>{error.message}</p>;

    const educationlist = data?.allEducation.nodes || [];
    return (
        <>
            {educationlist.map(({ id, institution, studyProgram,startDate, endDate }, index) => (
                <div className="education-item" key={id ?? index}>
                    <div className="education-main">
                        <strong className="experience-company">{institution}</strong>
                        <div className="experience-role">{studyProgram}</div>
                        <div className="experience-dates">{startDate} — {endDate}</div>
                    </div>
                </div>
            ))}
        </>
    );
}

const GET_EDUCATION = gql`{
  allEducation
  {
    nodes{
        institution,
        studyProgram,
        onGoing,
        startDate,
        endDate
    }
  }
}
`;

const GET_EXPERIENCES = gql`
{
  experiences {
    nodes{
        id,
        company,
        role,
        startDate,
        endDate
    }
  }
}
`;

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
        <div className= "title-container">
            <h2 className="sloop-script-h2">my</h2><h1 id="title">Resumé</h1><h2 className="sloop-script-h2">so far</h2>
                </div>
                <label>Experience</label>
                <div className="experiences-list"><DisplayExperiences /></div>
                <label>Education</label>
                <div className="experiences-list"><DisplayEducation /></div>
        </div>

    </>
    )
}

export default Resume;