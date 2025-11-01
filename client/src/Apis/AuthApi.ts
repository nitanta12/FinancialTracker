import axios from "axios";
import {API_Routes} from "./Endpoints";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;


export const authApi = {
    signin : async (userName: string, password:string)=>{
         const response = await axios.post(`${API_BASE_URL}${API_Routes.AUTH.SIGNIN}`,{userName,password},{ withCredentials: true });

         return response.data //this returns users token.
    },

    signout : async () => {
        const response = await axios.post(`${API_BASE_URL}${API_Routes.AUTH.SIGNOUT}`,{},{ withCredentials: true });

        return response.data;
    }
}


export const forgetPasswordApi = {
     forgetPassword : async (email : string) => {
        const response = await axios.post(`${API_BASE_URL}${API_Routes.AUTH.FORGETPASSWORD}?email=${encodeURIComponent(email)}`,{},{withCredentials : true});

        return response.data;
     }
}


export const resetPasswordApi = {
    resetPassword : async(userId : string, tokenId: string, password: string) => {
        var response = await axios.post(`${API_BASE_URL}${API_Routes.RESET.RESETPASSWORD}`,{userId,tokenId,password},{withCredentials : true});

        return response.data;
    }
}



