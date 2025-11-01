import LoginPage from "./Pages/Auth/LoginPage";
import Dashboard from "./Pages/Dashboard";
import { BrowserRouter as Router , Routes, Route, Navigate } from "react-router-dom";
import ProtectedRoute from "./Components/ProtectedRoute";
import ForgetPassword from "./Pages/Auth/ForgetPassword";
import ResetPassword from "./Pages/Auth/ResetPassword";

export default function App() {
  return (
    <>
    <Router>
      <Routes>
        <Route path = "/" element = {<LoginPage/>}/>
        <Route path = "/dashboard" element = 
       {
       <ProtectedRoute>
         <Dashboard/>
         </ProtectedRoute>
         }
         />
        <Route path = "/forgetpassword" element = {<ForgetPassword/>}/>
        <Route path = "/resetpassword" element = {<ResetPassword/>}/>
        {/* Redirect to any unknown path*/}
        <Route path = "*" element ={<Navigate to = "/"/>}/>
      </Routes>
    </Router>
    </>
  );
}
// export default App
