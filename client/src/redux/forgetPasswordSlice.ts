import { forgetPasswordApi } from "../Apis/AuthApi";
import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";



export const sendMail = createAsyncThunk(
    "auth/forgetpassword",
    async({email}: {email: string}, { rejectWithValue }) => {
        try {
            const data = await forgetPasswordApi.forgetPassword(email);
            return data; 
        } catch(err: any) {
            return rejectWithValue(err.response?.data?.message || err.message || "Network Error");
        }
    }
);

const forgetSlice = createSlice({
    name: "forgetpassword",
    initialState: {
        loading: false,
        message: "",
        error: ""
    },
    reducers: {
        clear : (state) => {
            state.message = "";
            state.error = "";
        }
    },
    extraReducers: (builder) => {
        builder
            .addCase(sendMail.pending, (state) => {
                state.loading = true;
                state.message = "";
                state.error = "";
            })
            .addCase(sendMail.fulfilled, (state, action) => {
                state.loading = false;
                state.message = action.payload.message; 
            })
            .addCase(sendMail.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload as string;
            });
    }
});

export const {clear} = forgetSlice.actions;

export default forgetSlice.reducer;