import { resetPasswordApi } from "../Apis/AuthApi";
import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";


export const resetPassword = createAsyncThunk(
    "user/resetpassword",
    async( {userId,tokenId,password} : {userId:string,tokenId:string,password:string}, 
        {rejectWithValue}
    ) => {
       try {
          var response = await resetPasswordApi.resetPassword(userId,tokenId,password);

          return response;
       } 
       catch (err:any) {
           return rejectWithValue(err.response?.data?.message || "Network error");
       }
    }
)


const resetSlice = createSlice({
    name : "reset",
    initialState :{
       loading:false,
       message:"",
       error:"",
    },

    reducers: {
        clearState : (state)=>{
            state.message = "";
            state.error = "";
        }
    },

    extraReducers:(builder) => {
        builder.addCase(resetPassword.pending, (state) => {
            state.loading = true;
            state.error = ""
        })
        .addCase(resetPassword.fulfilled, (state,action) => {
            state.loading = false;
            state.error = "",
            state.message = action.payload.message
        })
        .addCase(resetPassword.rejected, (state, action)=>{
            state.loading = false;
            state.error = action.payload as string
        })
    }

})


export const {clearState} = resetSlice.actions;

export default resetSlice.reducer;