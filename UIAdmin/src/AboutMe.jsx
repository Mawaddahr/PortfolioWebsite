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
    const [buttonText, setButtonText] = useState("Edit text");

    useEffect(() => {
        if (data?.readAbtMeText?.text) {
            setAboutMeText(data.readAbtMeText.text);
        }
    }, [data?.readAbtMeText?.text]);

    useEffect(() => {
        setAbtMeImg(image);
    }, [image]);

    const handleText = () => {
        if (buttonText === "Edit text") {
            setButtonText("Save text");
            setShowTextArea(true);
        } else if (buttonText === "Save text") {
            handleSubmit();
            setButtonText("Edit text");
            setShowTextArea(false);
        }
    };

    const fileUploadRef = useRef();
    const handleImageUpload = (event) => {
        event.preventDefault();
        fileUploadRef.current.click();
    }

    const handleImageDisplay = () => {
        const uploadedFile = fileUploadRef.current.files[0];

        const cachedImg = URL.createObjectURL(uploadedFile);
        setAbtMeImg(cachedImg);
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
    };

    return (
        <>
            <header>
                <div className="about-container">
                    <h2 id="about">About</h2>
                    <h2 id="about2">me</h2>
                </div>
            <nav className="nav-container">
                <Link className="nav-btn" to="/">Home</Link>
                <Link className="nav-btn" to="/Resume">Resumé</Link>
                <Link className="nav-btn" to="/Projects">Projects</Link>
                </nav>
        </header>
            <section>
                <button id="show-text-btn" onClick={handleText}>{buttonText}</button>
                {showTextArea ? (
                    <article>
                        <textarea
                            id="about-me-text"
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
                <aside>
                    <img id="about-me-img" src={abtMeImg} alt="about me image."/>
                    <form>
                        <button
                            id="edit-img-btn"
                            type="submit"
                            onClick={handleImageUpload}
                        >Choose file</button>
                        <input
                            type="file"
                            ref={fileUploadRef}
                            onChange={handleImageDisplay}
                            hidden>
                        </input>
                    </form>
                </aside>
            </section>
        </>
    )
}

export default AboutMe;