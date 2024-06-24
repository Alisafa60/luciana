import axios, { AxiosResponse, InternalAxiosRequestConfig, AxiosHeaders } from "axios";

const BaseURL = "http://localhost:5103";

const apiService = axios.create({
    baseURL: BaseURL,
});

interface LoginResponse {
    token: string;
}

export const login = async (email: string, password: string): Promise<LoginResponse> => {
    try {
        const response: AxiosResponse<LoginResponse> = await apiService.post('auth/login', { email, password });
        return response.data;
    } catch (error) {
        console.log("error logging in");
        throw error;
    }
}

apiService.interceptors.request.use(
    (config: InternalAxiosRequestConfig): InternalAxiosRequestConfig => {
        const token = localStorage.getItem('token');
        if (token) {
            if (config.headers) {
                if (config.headers instanceof AxiosHeaders) {
                    config.headers.set('Authorization', `Bearer ${token}`);
                } else {
                    (config.headers as Record<string, string>)['Authorization'] = `Bearer ${token}`;
                }
            } else {
                config.headers = new AxiosHeaders();
                config.headers.set('Authorization', `Bearer ${token}`);
            }
        }
        return config;
    },
    error => {
        return Promise.reject(error);
    }
);


