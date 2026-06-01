/* eslint-disable react-hooks/set-state-in-effect */
import { useParams, useNavigate } from "react-router-dom";
import { gql } from "@apollo/client";
import { useQuery, useMutation, useLazyQuery} from "@apollo/client/react";
import {useRef, useState, useEffect} from 'react';
import './UpdateExperience.css'
import './Resume.css' 

const GET_EXPERIENCE = gql`
query
  experienceById($id: String!){
    experienceById(id: $id){
      company,
      role,
      location,
      description,
      startDate,
      endDate
    }
}`;

const UPDATE_EXPERIENCE = gql`
mutation updateExperience($experience: ExperienceInput!){
  updateExperience(experience: $experience)
}`

const UpdateExperience = () =>
{
    const navigate = useNavigate()
    const experienceFormData = useRef();
    const inputExForm = useRef();
    const { id } = useParams({ id: String });

    const {loading, errors, data} = useQuery(GET_EXPERIENCE, {
        variables: {
            id: id
        }
    });

    const [updateExperience] = useMutation(UPDATE_EXPERIENCE);

    const [company, setCompany] = useState();
    const [role, setRole] = useState();
    const [location, setLocation] = useState();
    const [description, setDescription] = useState();
    const [startDate, setStartDate] = useState();
    const [endDate, setEndDate] = useState();

    useEffect(() => {
        if (data?.experienceById) {
            setCompany(data.experienceById.company);
            setRole(data.experienceById.role);
            setLocation(data.experienceById.location);
            setDescription(data.experienceById.description);
            setStartDate(data.experienceById.startDate);
            setEndDate(data.experienceById.endDate);
        }
    }, [data]);

    const handleUpdateExperience = async (e) => {
        e.preventDefault();
        const company = experienceFormData.current.company.value
        const role = experienceFormData.current.role.value
        const description = experienceFormData.current.description.value
        const startDate = experienceFormData.current.startdate.value
        const endDate = experienceFormData.current.enddate.value
        const location = experienceFormData.current.location.value

        await updateExperience({
            variables: {
                experience: {
                    id: id,
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

        alert("yay updated successfully!")
    }
    if (errors) return (<><h1>error! {errors.message}</h1></>)
    if (loading) return (<><h1>loading...</h1></>)
    return (<><div className="update-project">
        <div className="update-project-body">
            <div className="update-project-navigation">
                <button onClick={() => navigate(-1)} id="update-project-back-btn">Back</button>
            </div>
            <div className="update-project-main">
                <h1 id="update-project-title">Update Experience {id}</h1>
                <div className="edit-ex" ref={inputExForm}>
                <form className="edit-form" ref={experienceFormData} onSubmit={handleUpdateExperience}>
                    <input id="edit-ex-company" name="company" value={company} onChange={(e) => setCompany(e.currentTarget.value)} type="text" placeholder="Company" />
                        <input id="edit-ex-role" name="role" value={role} onChange={(e) => setRole(e.currentTarget.value)} type="text" placeholder="Role" />
                        <input id="edit-ex-location" name="location" value={location} onChange={(e) => setLocation(e.currentTarget.value)} type="text" placeholder="Location" />
                        <textarea id="edit-ex-description" name="description" value={description} onChange={(e) => setDescription(e.currentTarget.value)} type="text" placeholder="Description" />
                        <input id="edit-ex-startdate" name="startdate" value={startDate} onChange={(e) => setStartDate(e.currentTarget.value)} type="date" placeholder="start date" />
                        <input id="edit-ex-enddate" name="enddate" value={endDate} onChange={(e) => setEndDate(e.currentTarget.value)} type="date" placeholder="(estimated) end date" />
                    <button id="edit-exform-btn" type="submit">Submit</button>
                </form>
                </div>
            </div>
        </div>
    </div></>)
}

export default UpdateExperience;