import React, { useEffect, useState } from "react";

//UI component
import {
  Card,
  CardHeader,
  CardFooter,
  CardContent,
  CardTitle,
} from "../../Components/ui/card";
import { Input } from "../../Components/ui/input";
import { Button } from "../../Components/ui/button";

import { Loader2 } from "lucide-react";
import { useDispatch, useSelector } from "react-redux";
import type { AppDispatch, RootState } from "../../redux/store";
//import { useNavigate } from "react-router-dom";
import { clear, sendMail } from "../../redux/forgetPasswordSlice";
import Error from "../../Components/Error";
import ForgetPasswordSuccess from "./ForgetPasswordSuccess";


const ForgetPassword = () => {
  const [email, setEmail] = useState("");

  const dispatch = useDispatch<AppDispatch>();
  const { loading, message, error } = useSelector(
    (state: RootState) => state.forgetpassword
  );

  //const navigate = useNavigate();
  
   
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    dispatch(sendMail({ email }));
  };
 
  return (
    <>
       {message.length > 0 ? (
      // Render success component if message exists
      <ForgetPasswordSuccess />
    ) : (
      // Otherwise, render the forget password form
      <div className="bg-gray-50 min-h-screen flex justify-center">
        <div
          className="flex flex-col justify-center"
          style={{ minHeight: "70vh" }}
        >
          <Card className="w-full max-w-sm shadow-md py-4">
            <CardHeader className="pb-2">
              <CardTitle className="text-left text-3xl font-semibold">
                Forget Password
                <hr className="my-3 border-t-2 border-gray-200" />
              </CardTitle>
              <div className="py-10 bg-yellow-100 px-5">
                Forgotten your password? Enter your e-mail address below, and
                we'll send you an e-mail allowing you to reset it.
              </div>
            </CardHeader>
            <form onSubmit={handleSubmit}>
              <CardContent className="space-y-4 my-5">
                <div>
                  <Input
                    id="email"
                    placeholder="E-mail address"
                    type="text"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                    className="h-12"
                  />
                </div>
              </CardContent>

              <CardFooter>
                <Button
                  className="bg-lime-500 text-white"
                  disabled={loading}
                  type="submit"
                >
                  {loading ? (
                    <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                  ) : (
                    " Reset My Password"
                  )}
                </Button>
              </CardFooter>
            </form>
          </Card>
        </div>
      </div>
    )}
      { error && (
      <Error
        message = {error}
        type = {"Error"}
        show = {!!error}
        onClose = {() => dispatch(clear())}
      />)}
    </>
  );
};

export default ForgetPassword;
