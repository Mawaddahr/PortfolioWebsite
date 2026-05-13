/* eslint-disable react-hooks/set-state-in-effect */
import { Link } from 'react-router-dom';
import { useState , useEffect} from 'react';
import { gql } from "@apollo/client";
import { useQuery, useMutation} from "@apollo/client/react";
import './AboutMe.css';

const GETABTMETEXT = gql`query{
  readAbtMeText
  {
    text
  }
}`;

const EditAboutMeText = (props) =>
{
    const { data } = useQuery(GETABTMETEXT);
    const [aboutMeText, setAboutMeText] = useState();
    const [aboutMeImg, setAboutMeImg] = useState(
        "https://i.pinimg.com/1200x/00/a8/bd/00a8bdcdb6a65ba1dc9a2f325e1390fc.jpg"
    );

    useEffect(() => {
        if (data?.readAbtMeText?.text) {
            setAboutMeText(data.readAbtMeText.text);
        }
    }, [data?.readAbtMeText?.text]);
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
                    imageUrl: aboutMeImg
                }
            }
        });
    };

    if (props.showTextArea)
    {
        return(<>
        <section>
            <textarea
                id="about-me-text"
                value={aboutMeText}
                onChange={e => setAboutMeText(e.target.value)}
                maxLength={1000}>
            </textarea>
            <button
                id="submit-btn"
                onClick={handleSubmit}
            >Save</button>
            </section></>
            )
    }
}

const ShowTextUnedited = (props) => {
    const { data } = useQuery(GETABTMETEXT);
    const [aboutMeText, setAboutMeText] = useState("Huh");
    const [aboutMeImg, setAboutMeImg] = useState(
        "https://i.pinimg.com/1200x/00/a8/bd/00a8bdcdb6a65ba1dc9a2f325e1390fc.jpg"
    );

    useEffect(() => {
        if (data?.readAbtMeText?.text) {
            setAboutMeText(data.readAbtMeText.text);
        }
    }, [data?.readAbtMeText?.text]);
    if (!props.showTextArea) {
        return (<article id= "about-me-text-unedited">{aboutMeText}</article>)
    }
}
function AboutMe() {
    const [showTextArea, setShowTextArea] = useState(false);

    return (
        <>
        <header>
            <nav className="nav-container">
                <Link className="nav-btn" to="/">Home</Link>
                <Link className="nav-btn" to="/Resume">Resumé</Link>
                <Link className="nav-btn" to="/Projects">Projects</Link>
            </nav>
                <h1>About me</h1>
        </header>
            <section>
                <button onClick={() => showTextArea == true ? setShowTextArea(false) : setShowTextArea(true)}>Edit text</button>
                <ShowTextUnedited showTextArea={showTextArea} />
                <EditAboutMeText showTextArea={showTextArea} />
        </section>
        <aside>
        </aside>
        </>
    )
}


export default AboutMe;