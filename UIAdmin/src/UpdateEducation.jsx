/* eslint-disable react-hooks/set-state-in-effect */
import { useParams, useNavigate } from "react-router-dom";
import { gql } from "@apollo/client";
import { useQuery, useMutation} from "@apollo/client/react";
import {useRef, useState, useEffect} from 'react';
import './UpdateExperience.css'
import './Resume.css' 

const GET_EDUCATION = gql`
query educationById($id: String!)
{
  educationById(id: $id)
  {
    institution,
    studyProgram,
    studyProgramType,
    onGoing,
    startDate,
    endDate
  }
}
`

const UPDATE_EDUCATION = gql`
mutation updateEducation($education: EducationInput!){
  updateEducation(education: $education)
}`;


const UpdateExperience = () =>
{
    const navigate = useNavigate()
    const educationFormData = useRef();
    const inputEdForm = useRef();
    const { id } = useParams({ id: String });

    const { loading, errors, data } = useQuery(GET_EDUCATION, {
        variables: {
            id: id
        }
    });

    const [updateEducation] = useMutation(UPDATE_EDUCATION);

    const [institution, setInstitution] = useState();
    const [studyprogram, setStudyProgram] = useState();
    const [studyprogramtype, setStudyProgramType] = useState();
    const [onGoing, setOnGoing] = useState();
    const [startDate, setStartDate] = useState();
    const [endDate, setEndDate] = useState();

    useEffect(() => {
        if (data?.educationById) {
            setInstitution(data.educationById.institution);
            setStudyProgram(data.educationById.studyProgram);
            setStudyProgramType(data.educationById.studyProgramType);
            setOnGoing(data.educationById.onGoing);
            setStartDate(data.educationById.startDate);
            setEndDate(data.educationById.endDate);
        }
    }, [data]);

    const handleUpdateEducation = async (e) => {
        e.preventDefault();
        const institution = educationFormData.current.institution.value
        const studyprogram = educationFormData.current.studyprogram.value
        const studyprogramtype = educationFormData.current.studyprogramtype.value
        const startDate = educationFormData.current.startdate.value
        const endDate = educationFormData.current.enddate.value
        const ongoing = educationFormData.current.onGoing.value === "true"

        await updateEducation({
            variables: {
                education: {
                    id: id,
                    institution: institution,
                    studyProgram: studyprogram,
                    studyProgramType: studyprogramtype,
                    startDate: startDate,
                    endDate: endDate,
                    onGoing: ongoing
                }
            }
        });

        educationFormData.current.institution.value = null;
        educationFormData.current.studyprogram.value = null;
        educationFormData.current.studyprogramtype.value = null;
        educationFormData.current.startdate.value = null;
        educationFormData.current.enddate.value = null;
        educationFormData.current.onGoing.value = null;

        alert("yay updated successfully!");
        navigate(-1);
    }
    if (errors) return (<><h1>error! {errors.message}</h1></>)
    if (loading) return (<><h1>loading...</h1></>)
    return (<><div className="update-project">
        <div className="update-project-body">
            <div className="update-project-navigation">
                <button onClick={() => navigate(-1)} id="update-project-back-btn">Back</button>
            </div>
            <div className="update-project-main">
                <h1 id="update-project-title">Update Education {id}</h1>
                <div className="edit-ex" ref={inputEdForm}>
                    <form className="edit-form" ref={educationFormData} onSubmit={handleUpdateEducation}>
                    <input id="edit-ed-institution" name="institution" value={institution} onChange={(e) => setInstitution(e.currentTarget.value)} type="text" placeholder="Institution" />
                        <input id="edit-ed-studyprogram" name="studyprogram" value={studyprogram} onChange={(e) => setStudyProgram(e.currentTarget.value)} type="text" placeholder="Study program" />
                        <input id="edit-ed-studyprogramtype" name="studyprogramtype" value={studyprogramtype} onChange={(e) => setStudyProgramType(e.currentTarget.value)} type="text" placeholder="Study program type" />
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
                        <input id="edit-ed-startdate" name="startdate" value={startDate} onChange={(e) => setStartDate(e.currentTarget.value)} type="date" placeholder="start date" />
                        <input id="edit-ed-enddate" name="enddate" value={endDate} onChange={(e) => setEndDate(e.currentTarget.value)} type="date" placeholder="(estimated) end date" />
                    <button id="edit-edform-btn" type="submit">Submit</button>
                </form>
                </div>
            </div>
        </div>
    </div></>)
}

export default UpdateExperience;