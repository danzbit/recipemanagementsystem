import axios, { AxiosError, AxiosInstance, InternalAxiosRequestConfig, AxiosResponse, AxiosRequestConfig } from 'axios';
import { getToken } from './authService';

export interface ApiResponse<T = any> {
    data: T;
    message?: string;
    success?: boolean;
}

const api: AxiosInstance = axios.create({
    baseURL: process.env.REACT_APP_API_URL || 'http://localhost:8080/api',
    headers: {
        'Content-Type': 'application/json',
    },
    timeout: 10000,
});

api.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const token = getToken();
        if (token && config.headers) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error: AxiosError) => Promise.reject(error)
);

api.interceptors.response.use(
    (response: AxiosResponse<ApiResponse>) => response,
    (error: AxiosError) => {
        console.error('API Error:', error);
        return Promise.reject(error);
    }
);

export const get = <T>(url: string, config?: AxiosRequestConfig) =>
    api.get<T>(url, config).then((res) => res.data);

export const post = <T>(url: string, data?: any, config?: AxiosRequestConfig) =>
    api.post<T>(url, data, config).then((res) => res.data);

export const put = <T>(url: string, data?: any, config?: AxiosRequestConfig) =>
    api.put<T>(url, data, config).then((res) => res.data);

export const del = <T>(url: string, config?: AxiosRequestConfig) =>
    api.delete<T>(url, config).then((res) => res.data);

export default api;
