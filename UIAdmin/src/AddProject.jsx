import { useNavigate } from 'react-router-dom';
import { gql } from "@apollo/client";
import { useQuery, useMutation, useLazyQuery } from "@apollo/client/react";
import { useRef, useState, useEffect } from 'react';


const AddProject = () => {
    const navigate = useNavigate();
    const projectFormData = useRef();

    return (<>
        <div className="add-project">
            <div className="add-project-body">
                <div className="add-project-header">
                    <button onClick={() => navigate(-1) }>back</button>
                </div>
                <div className="add-project-main">
                    <form ref={projectFormData}>
                    <input />
                    </form>

                </div>
            </div>
        </div>
    </>)
}

export default AddProject;