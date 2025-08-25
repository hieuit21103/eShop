import axios from 'axios';

const AUTH_API_URL = import.meta.env.VITE_AUTH_API_URL + 'auth' || 'http://localhost:5003/api/auth';

export interface RegisterPayload {
    username: string;
    password: string;
    email: string;
}

export interface LoginPayload {
    username: string;
    password: string;
}

export interface ResetPasswordPayload {
    password: string;
    confirmPassword: string;
}

class AuthService {
    async register(payload: RegisterPayload): Promise<void> {
        const response = await axios.post(`${AUTH_API_URL}/register`, payload);
        return response.data;
    }

    async confirmEmail(userId: string, token: string): Promise<void> {
        const response = await axios.get(`${AUTH_API_URL}/confirm-email/${userId}/${token}`);
        return response.data;
    }

    async login(payload: LoginPayload): Promise<{ status: number, token: string }> {
        const response = await axios.post(`${AUTH_API_URL}/login`, payload);
        return {status: response.status, token: response.data.Token}; // Assuming the response contains a Token field
    }

    async logout(): Promise<void> {
        const response = await axios.post(`${AUTH_API_URL}/logout`, {}, {
            headers: {
                Authorization: `Bearer ${this.getToken()}`
            }
        });
        return response.data;
    }

    async getCurrentUser(): Promise<any> {
        const response = await axios.get(`${AUTH_API_URL}/me`, {
            headers: {
                Authorization: `Bearer ${this.getToken()}`
            }
        });
        return response.data;
    }

    async sendForgotPasswordEmail(email: string): Promise<void> {
        const response = await axios.get(`${AUTH_API_URL}/forgot-password/${email}`, {
            headers: {
                Authorization: `Bearer ${this.getToken()}`
            }
        });
        return response.data;
    }

    async getResetPasswordInfo(userId: string, token: string): Promise<void> {
        const response = await axios.get(`${AUTH_API_URL}/reset-password/${userId}/${token}`, {
            headers: {
                Authorization: `Bearer ${this.getToken()}`
            }
        });
        return response.data;
    }

    async resetPassword(userId: string, token: string, payload: ResetPasswordPayload): Promise<void> {
        const response = await axios.put(`${AUTH_API_URL}/reset-password/${userId}/${token}`, payload, {
            headers: {
                Authorization: `Bearer ${this.getToken()}`
            }
        });
        return response.data;
    }

    private getToken(): string | null {
        return localStorage.getItem('authToken'); // Replace with your token storage mechanism
    }
}

export default new AuthService();