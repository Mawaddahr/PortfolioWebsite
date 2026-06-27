/* eslint-disable react-hooks/set-state-in-effect */
import { Link } from 'react-router-dom';
import { useState , useEffect, useRef} from 'react';
import { gql } from "@apollo/client";
import { useQuery, useMutation } from "@apollo/client/react";
import DefaultImage from './assets/uploadImage.jpg';
import './AboutMe.css';

const GETABTMETEXT = gql`query{
  readAbtMeText
  {
    text
  }
}`;
const GET_IMAGE = gql`
query{
    readAbtMeText
    {
        imageUrl
    }
}
`;

function useImageUpload() {
    const { data } = useQuery(GET_IMAGE);
    const [imgUrl, setImageUrl] = useState(DefaultImage);

    useEffect(() => {
        if (data?.readAbtMeText?.imageUrl) {
            setImageUrl(data.readAbtMeText.imageUrl);
        }
    }, [data?.readAbtMeText?.imageUrl]);

    return imgUrl;
}

function AboutMe() {
    const { data } = useQuery(GETABTMETEXT);
    const image = useImageUpload();
    const [aboutMeText, setAboutMeText] = useState("");
    const [abtMeImg, setAbtMeImg] = useState(image);
    const [showTextArea, setShowTextArea] = useState(false);
    const [showUploadButton, setShowUploadButton] = useState(true);
    const [buttonText, setButtonText] = useState("Edit text");
    const delay = async (ms) => {
        return new Promise((resolve) =>
            setTimeout(resolve, ms));
    };

    useEffect(() => {
        if (data?.readAbtMeText?.text) {
            setAboutMeText(data.readAbtMeText.text);
        }
    }, [data?.readAbtMeText?.text]);

    useEffect(() => {
        setAbtMeImg(image);
    }, [image]);

    const handleText = async () => {
        if (buttonText === "Edit text") {
            setButtonText("Save text");
            setShowTextArea(true);
        } else if (buttonText === "Save text") {
            await handleSubmit();
            setButtonText("Edit text");
            setShowTextArea(false);
        }
    };

    const fileUploadRef = useRef();
    const handleImageUpload = (event) => {
        event.preventDefault();
        fileUploadRef.current.click();
    }

    const handleImageDisplay = async () => {
        const uploadedFile = fileUploadRef.current.files[0];
        if (!uploadedFile) return;

        const formData = new FormData();
        formData.append("file", uploadedFile);
        const response = await fetch(
            "http://localhost:8081/api/img-upload",
            {
                method: "POST",
                body: formData
            }
        );

        const data = await response.json();
        setAbtMeImg(data.imageUrl);
        setShowUploadButton(false);
    }

    const INSERTABTMETEXT = gql`
        mutation PutTextInJson($abtMeText: AboutMeTextInput!) {
        putTextInJson(abtMeText: $abtMeText)
        }
    `;

    const [insertAbtMeText] = useMutation(INSERTABTMETEXT);

    const handleSubmit = async () => {
        await insertAbtMeText({
            variables: {
                abtMeText: {
                    text: aboutMeText,
                    imageUrl: abtMeImg
                }
            }
        });
        if (!showUploadButton) {
            delay(2000);
            setShowUploadButton(true);
        }
    };

    return (
        <>
            <div className="abt-me">
                <header className="abtme-header">
                    <div className="about-container">
                        <h2 id="about">About</h2>
                        <h2 id="about2">me</h2>
                    </div>
                    <nav className="abtme-nav-container">
                        <Link className="nav-btn" to="/">Home</Link>
                        <Link className="nav-btn" to="/Resume">Resumé</Link>
                        <Link className="nav-btn" to="/Projects">Projects</Link>
                    </nav>
                </header>
            <div className="abt-me-body">
            <main className='abt-me-main'>
            <div className="buttons">
            <button id="show-text-btn" onClick={handleText}>{buttonText}</button>
            <form>
                {showUploadButton ?
                    <button
                        id="edit-img-btn"
                        type="submit"
                        onClick={handleImageUpload}
                    >upload image
                    </button> :
                    <button
                        id="save-img-btn"
                        type="button"
                        onClick={handleSubmit}
                    >Save image
                    </button>}
                <input
                    type="file"
                    ref={fileUploadRef}
                    onChange={handleImageDisplay}
                    hidden>
                </input>
                </form>
            </div>
            <section>
                {showTextArea ? (
                    <article>
                        <textarea
                            id="about-me-text"
                            type="text"
                            value={aboutMeText}
                            onChange={(e) => setAboutMeText(e.target.value)}
                            maxLength={5000}
                        />
                    </article>
                ) : (
                    <article id="about-me-text-unedited">
                        {aboutMeText}
                    </article>
                )}
                    <img id="about-me-img" src={abtMeImg} alt="about me image."/>
                    </section>
                    </main>
            </div>
            </div>
        </>
    )
}

export default AboutMe;