import {authApi}  from '../Apis/AuthApi';
import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';


interface AuthState {
    user: any | null;
      token: string | null; 
    loading: boolean;
    error: string|null; 
}

const initialState : AuthState = {
    user : null,
    token : null,
    loading : false,
    error : null
}


export const signIn = createAsyncThunk(
    "auth/signin",
    async (
        {userName,password} :  { userName: string; password: string },
        {rejectWithValue}
    ) => {
        try{
           const response = await authApi.signin(userName,password);
             if(!response.status){
                return rejectWithValue(response.message[0] || "Login failed");
           }
           //set token in local storage
           const userData = response.data;
          

           localStorage.setItem("token",userData.token);
            
           return userData; 
        }
        catch(err :any){
              return rejectWithValue(err.message || "Network error");
        }
    }
);

export const signOut = createAsyncThunk(
    "auth/signout",
    async (_, {dispatch,rejectWithValue}) =>{
        try{
        await authApi.signout();
        dispatch(logout());
        }
        catch(err : any){
            return rejectWithValue(err.message || "Network error");
        }
    }
)

const authSlice = createSlice({
    name :"auth",
    initialState,
    reducers:{
        logout: (state) => {
            state.user = null;
            state.token = null;
            localStorage.removeItem("token");
        },
        clearError: (state) => {
            state.error = null;
        }
    },

    extraReducers: (builder) => {
        builder.addCase(signIn.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(signIn.fulfilled,(state,action) => {
            state.loading = false;
            state.user= action.payload.user;
            state.token = action.payload.token;
        })
        .addCase(signIn.rejected,(state,action)=>{
            state.loading=false;
            state.error = action.payload as string;
        });
    },
});

export const {logout,clearError } = authSlice.actions;
export default authSlice.reducer;