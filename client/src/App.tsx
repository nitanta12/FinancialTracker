import LoginPage from "./Pages/LoginPage";
import Dashboard from "./Pages/Dashboard";
import { BrowserRouter as Router , Routes, Route, Navigate } from "react-router-dom";
import ProtectedRoute from "./Components/ProtectedRoute";

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

        {/* Redirect to any unknown path*/}
        <Route path = "*" element ={<Navigate to = "/"/>}/>
      </Routes>
    </Router>
    </>
  );
}
// export default App
