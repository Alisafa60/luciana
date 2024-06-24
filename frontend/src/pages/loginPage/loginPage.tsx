import React, { useState, FormEvent, ChangeEvent } from "react";
import { login } from "../../services/api";
import Button from "../../components/buttonComponent/buttonComponent";

const Login: React.FC = () => {
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [error, setError] = useState<string>('');

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        try {
            const response = await login(email, password);
            const token = response.token;
            localStorage.setItem('token', token);
            console.log(`token: ${token}`)
        } catch (error) {
            setError('Invalid Credentials');
        }
    }

    const handleEmailChange = (e: ChangeEvent<HTMLInputElement>) => {
        setEmail(e.target.value);
    };

    const handlePasswordChange = (e: ChangeEvent<HTMLInputElement>) => {
        setPassword(e.target.value);
    }

    return (
        <div className="login-container">
            <h2>Login</h2>
            <form onSubmit={handleSubmit}>
                <input
                    type="text"
                    placeholder="Email"
                    value={email}
                    onChange={handleEmailChange}
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={handlePasswordChange}
                />
                <Button type="submit">Login</Button>
            </form>
            {error && <p>{error}</p>}
        </div>
    );
}

export default Login;