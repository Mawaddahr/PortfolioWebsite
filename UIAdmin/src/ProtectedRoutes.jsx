import React from 'react'
import {Route, Outlet, Navigate} from 'react-router-dom'

function ProtectedRoute(auth){
    return auth === true ? <Outlet /> : <Navigate to="/" />

}

export default ProtectedRoute