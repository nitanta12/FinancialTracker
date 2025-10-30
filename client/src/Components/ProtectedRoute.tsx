
import { Navigate } from "react-router-dom";
//import React from 'react'

const ProtectedRoute = ({children} : any) => {
    const token = localStorage.getItem("token");
    if(!token){
        return <Navigate to = '/login' replace/>
    }
    return children;
}

export default ProtectedRoute