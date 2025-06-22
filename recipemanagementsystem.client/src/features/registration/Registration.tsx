import React, { useState } from 'react'
import { post } from '../../services/api';
import { toast } from 'react-toastify';
import { Link, useNavigate } from 'react-router-dom';

const Registration = () => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({ username: '', email: '', password: '' });
    const [loading, setLoading] = useState(false);
    const [errors, setErrors] = useState<string[] | null>(null);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData((prev) => ({ ...prev, [e.target.name]: e.target.value }));
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setErrors(null);
        try {
            await post('/api/auth/sign-up', formData);
            toast.success('Account created successfully! You can now log in.');
            setTimeout(() => navigate('/login'), 1500);
        } catch (err: any) {
            setErrors(err.response?.data?.errors || ['Signup failed']);
            errors?.forEach((err) => {
                toast.error(err)
            })
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container d-flex align-items-center justify-content-center flex-column vh-100">
            <div className="card shadow-sm p-4" style={{ maxWidth: '400px', width: '100%' }}>
                <h2 className="mb-4 text-center">Sign Up</h2>
                <form onSubmit={handleSubmit}>
                    <input
                        name="username"
                        className="form-control mb-3"
                        placeholder="Username"
                        onChange={handleChange}
                        required
                    />
                    <input
                        name="email"
                        className="form-control mb-3"
                        type="email"
                        placeholder="Email"
                        onChange={handleChange}
                        required
                    />
                    <input
                        name="password"
                        className="form-control mb-3"
                        type="password"
                        placeholder="Password"
                        onChange={handleChange}
                        required
                    />

                    {errors?.length === 1 && <div className="alert alert-danger">{errors[0]}</div>}

                    <button className="btn btn-primary w-100" type="submit" disabled={loading}>
                        {loading ? 'Signing up...' : 'Sign Up'}
                    </button>
                </form>
            </div>
            <div className="text-center mt-3">
                Already have an account? <Link to="/login">Login</Link>
            </div>
        </div>
    )
}

export default Registration
