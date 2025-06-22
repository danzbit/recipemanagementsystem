import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { User } from '../../types/User';
import { post } from '../../services/api';

interface AuthState {
    token: string | null;
    userId: string | null;
    status: 'idle' | 'loading' | 'failed';
    error: string | null;
}

const initialState: AuthState = {
    token: localStorage.getItem('token'),
    userId: localStorage.getItem('userId'),
    status: 'idle',
    error: null,
};

export const login = createAsyncThunk<
    { token: string; id: string },
    { usernameOrEmail: string; password: string },
    { rejectValue: string }
>(
    '/auth/sign-in',
    async (credentials, thunkAPI) => {
        try {
            const response = await post<User>('/api/auth/sign-in', credentials);
            const { token, id } = response;

            if (!token || !id) {
                return thunkAPI.rejectWithValue('Invalid login response');
            }

            localStorage.setItem('token', token);
            localStorage.setItem('userId', id);
            return { token, id };
        } catch (error: any) {
            return thunkAPI.rejectWithValue(error.response?.data?.message || error.message);
        }
    }
);

const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        logout(state) {
            console.log('Logout action triggered, resetting userId to null');
            state.token = null;
            state.userId = null;
            localStorage.removeItem('token');
            localStorage.removeItem('userId');
            state.status = 'idle';
            state.error = null;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(login.pending, (state) => {
                state.status = 'loading';
                state.error = null;
            })
            .addCase(login.fulfilled, (state, action) => {
                state.status = 'idle';
                state.token = action.payload.token;
                state.userId = action.payload.id;
            })
            .addCase(login.rejected, (state, action) => {
                state.status = 'failed';
                state.error = action.payload || 'Login failed';
            });
    },
});

export const { logout } = authSlice.actions;

export default authSlice.reducer;
