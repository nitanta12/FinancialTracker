import {configureStore} from "@reduxjs/toolkit";
import authReducer from "./authSlice";
import forgetReducer from "./forgetPasswordSlice";
import  resetReducer  from "./resetPasswordSlice";


export const store = configureStore({
    reducer:{
        auth:authReducer,
        forgetpassword: forgetReducer,
        resetPassword: resetReducer
    }
})


export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;