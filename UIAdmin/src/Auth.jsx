import { useState } from 'react'
import { auth, googleProvider } from './firebaseConfig.jsx';
import { signInWithPopup } from 'firebase/auth'


function useLogInWithGoogle(initialValue) {
    const [isAuth, setIsAuth] = useState(initialValue)
    async function SignIn() {
        try {
            await signInWithPopup(auth, googleProvider)
            setIsAuth(true)
        }
        catch (error) {
            alert(error.message)
            setIsAuth(false)
        }
    }
    return [isAuth, SignIn]
}

export default useLogInWithGoogle