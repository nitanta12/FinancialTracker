import React, { useState } from 'react'
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import type { AppDispatch, RootState } from '../../redux/store';
import { clearState, resetPassword } from '../../redux/resetPasswordSlice';

import {
      Card,
      CardTitle,
      CardContent,
      CardFooter,
      CardHeader
}
from '../../Components/ui/card';

import {Input} from '../../Components/ui/input';
import { Button } from '../../Components/ui/button';
import { Loader2 } from 'lucide-react';
import Error from "../../Components/Error";


const ResetPassword = () => {
    const params = new URLSearchParams(window.location.search);
    const tokenId = params.get("token");
    const userId = params.get("verify");
    console.log(tokenId);
    console.log(userId);
    const[password,setPassword] = useState("");
    const[confirmPassword, setConfirmPassword] = useState("");
    const[pwdError, setPwdError] = useState("")
    const navigate = useNavigate();
     
    const dispatch = useDispatch<AppDispatch>();
    const {loading,message,error} = useSelector((state :RootState) => state.resetPassword);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        
        if (password !== confirmPassword) {
          setPwdError("Passwords do not match");
          return;
        }
     
        dispatch(resetPassword({userId:userId ?? "",tokenId:tokenId ?? "",password}));
    }
      const handlePasswordChange = (e : React.ChangeEvent<HTMLInputElement>)=>{
        setPassword(e.target.value);
        if(pwdError) setPwdError("");
      }
      const handleConfirmPasswordChange = (e : React.ChangeEvent<HTMLInputElement>)=>{
        setConfirmPassword(e.target.value);
        if(pwdError) setPwdError("");
      }
  return (
    <>
     <div className = "bg-gray-50 min-h-screen flex justify-center">
        <div className = "flex flex-col justify-center" style = {{minHeight : '70vh'}}>
           <Card className = "w-full max-w-sm py-4 shadow-lg">
              <CardHeader className = "pb-2">
                 <CardTitle className = "text-left text-3xl font-semibold">
                    New password
                    <hr className = "my-2 border-t-2 border-gray-200"/>
                 </CardTitle>
              </CardHeader>

              <form onSubmit={handleSubmit}>
                <CardContent className = "space-y-4 my-5">
                 <div>
                    <Input
                          id= "password"
                          type = "password"
                          placeholder='Enter new password'
                          value={password}
                          onChange={handlePasswordChange}
                          required
                           className="h-12"
                    />
                 </div>
                 <div>
                    <Input
                          id= "confirmpassword"
                          type = "password"
                          placeholder='Confirm password'
                          value={confirmPassword}
                          onChange={handleConfirmPasswordChange}
                          required
                          className="h-12"
                    />
                 </div>
                 {pwdError && <p className="text-red-500 text-sm">{pwdError}</p>}
                 </CardContent>

                 <CardFooter>
                    <Button
                        className = "bg-blue-500 text-white"
                        disabled={loading}
                        type="submit"
                    >
                    {loading ?
                    (<Loader2 className = "animate-spin mr-2 h-4 w-4"/>) : "New password" 
                    }
                    </Button>
                 </CardFooter>
              </form>
           </Card>
        </div>
     </div>

     {error && (
      <Error
        message={error}
        type="Error"
        show={!!error}
        onClose={() => dispatch(clearState())}
      />
    )}

    {message && (
      <Error
        message={message}
        type="Success"
        show={!!message}
        onClose={() => dispatch(clearState())}
      />
    )}
    </>
  )
}

export default ResetPassword