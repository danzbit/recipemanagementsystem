import React, { useEffect, useState } from 'react'
import { useAppDispatch, useAppSelector } from '../../hooks/hooks';
import { login } from './loginSlice';
import { Link, useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';

const Login = () => {
    const dispatch = useAppDispatch();
    const authState = useAppSelector(state => state.auth);

    const navigate = useNavigate();

    const [usernameOrEmail, setUsernameOrEmail] = useState('');
    const [password, setPassword] = useState('');

    useEffect(() => {
        if (authState.token) {
            navigate('/');
        }
    }, [authState.token, navigate]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        dispatch(login({ usernameOrEmail, password }));
        toast.success('You signed in successfully! You can now log in.');
    };

    return (
        <div className="container d-flex align-items-center justify-content-center flex-column vh-100">
            <div className="card shadow-sm p-4" style={{ maxWidth: '400px', width: '100%' }}>
                <h2 className="mb-4 text-center">Login</h2>
                <form onSubmit={handleSubmit}>
                    <div className="mb-3">
                        <label htmlFor="username" className="form-label">
                            Username or Email
                        </label>
                        <input
                            id="username"
                            type="text"
                            className="form-control"
                            value={usernameOrEmail}
                            onChange={(e) => setUsernameOrEmail(e.target.value)}
                            placeholder="Enter username"
                            required
                            disabled={authState.status === 'loading'}
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="password" className="form-label">
                            Password
                        </label>
                        <input
                            id="password"
                            type="password"
                            className="form-control"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            placeholder="Enter password"
                            required
                            disabled={authState.status === 'loading'}
                        />
                    </div>
                    {authState.error && (
                        <div className="alert alert-danger" role="alert">
                            {authState.error}
                        </div>
                    )}
                    <button
                        type="submit"
                        className="btn btn-primary w-100"
                        disabled={authState.status === 'loading'}
                    >
                        {authState.status === 'loading' ? (
                            <>
                                <span
                                    className="spinner-border spinner-border-sm me-2"
                                    role="status"
                                    aria-hidden="true"
                                ></span>
                                Logging in...
                            </>
                        ) : (
                            'Login'
                        )}
                    </button>
                </form>
            </div>
            <div className="text-center mt-3">
                <span>Don't have an account? </span>
                <Link to="/registration">Sign up</Link>
            </div>
        </div>
    );
}

export default Login
