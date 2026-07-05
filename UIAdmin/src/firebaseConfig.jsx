import { initializeApp } from 'firebase/app';
import { getAuth, GoogleAuthProvider } from 'firebase/auth';

const firebaseConfig = {
    apiKey: "AIzaSyDRIU0YobUBmrsyF_IOoZX1p6VcDsIv3HE",
    authDomain: "portfoliowebsite-admin-7f818.firebaseapp.com",
    projectId: "portfoliowebsite-admin-7f818",
    storageBucket: "portfoliowebsite-admin-7f818.firebasestorage.app",
    messagingSenderId: "1094411974932",
    appId: "1:1094411974932:web:df6258d3c27b05cfcf2a01"
};

const app = initializeApp(firebaseConfig);
const auth = getAuth(app);
const googleProvider = new GoogleAuthProvider();

export {auth, googleProvider}