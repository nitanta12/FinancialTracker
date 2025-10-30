import React, { useEffect, useState } from "react";
//store
import { useDispatch, useSelector } from "react-redux";
import type { AppDispatch, RootState } from "../redux/store";
import { useNavigate } from "react-router-dom";
import { Button } from "../Components/ui/button";
import {
  Card,
  CardContent,
  //CardDescription,
  CardHeader,
  CardFooter,
  CardTitle,
} from "../Components/ui/card";
import { Input } from "../Components/ui/input";

import { Loader2 } from "lucide-react";
//api
import  {clearError, signIn}  from "../redux/authSlice";
import Error from "../Components/Error";

const LoginPage = () => {
  const dispatch = useDispatch<AppDispatch>(); 
  const {token,loading,error} = 
  useSelector((state:RootState) => state.auth); 

  const navigate = useNavigate();

  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  //const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
     dispatch(signIn({userName,password}));
  };
  
  useEffect(() => {
      if(token){
        navigate("/dashboard");
      }
  },[token,navigate]);
 
  return (
    <>
      <div className="bg-gray-50 min-h-screen flex justify-center">
  <div className="flex flex-col justify-center" style={{ minHeight: '70vh' }}>
        <Card className="w-full max-w-sm shadow-md p-4">
          <CardHeader className="pb-2">
            <CardTitle className="text-center text-3xl font-semibold">
              Financial Tracker Login
              <div className="py-10 font-bold text-2xl">Log in to continue</div>
            </CardTitle>
          </CardHeader>
          <form onSubmit={handleSubmit}>
            <CardContent className="space-y-4">
              <div>
                {/* <label htmlFor='userName' className = "block text-sm font-medium mb-1">
                        Username:
                     </label> */}
                <Input
                  id="username"
                  placeholder="Enter your username or e-mail"
                  type="text"
                  value={userName}
                  onChange={(e) => setUserName(e.target.value)}
                  required
                  className=""
                />
              </div>

              <div>
                {/* <label htmlFor='password' className = "block text-sm font-medium mb-1">
                        Password:
                     </label> */}
                <Input
                  id="password"
                  placeholder="Password"
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                />

              
              </div>
             
            </CardContent>

            <CardFooter className="flex justify-center ">
              <Button
                type="submit"
                disabled = {loading}
                className="w-full bg-blue-600 text-white font-semibold  font-medium 
                   hover:bg-blue-700  transform hover: scale-[1.02] transition-all duration-200"
              >
                {loading ? 
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" /> 
                 : 
                  "Login"
                }
              </Button>
            </CardFooter>
            <a
              href="/dashboard"
              className="w-full text-center py-2 px-4 rounded hover:shadow-lg hover:bg-gray-100 underline transition-all"
            >
              Forgot your password?
            </a>
          </form>
        </Card>
      </div>
      </div>
        {/* Error popup */}
    { error && (
      <Error
        message={error}
        type="Error"
        show={!!error}
        onClose={() => dispatch(clearError())} // clears Redux error
      />
    )}
    </>
  );
};

export default LoginPage;
