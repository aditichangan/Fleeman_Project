import React, { useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import AuthService from '../../services/authService';
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { AlertTriangle, Loader2, CheckCircle2 } from "lucide-react";

const ResetPassword = () => {
    const [searchParams] = useSearchParams();
    const token = searchParams.get('token');
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        password: '',
        confirmPassword: ''
    });
    const [status, setStatus] = useState({ type: '', message: '' });
    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setStatus({ type: '', message: '' });

        if (formData.password !== formData.confirmPassword) {
            setStatus({ type: 'error', message: 'Passwords do not match.' });
            return;
        }

        if (!token) {
            setStatus({ type: 'error', message: 'Invalid or missing reset token.' });
            return;
        }

        setLoading(true);
        try {
            await AuthService.resetPassword(token, formData.password);
            setStatus({ type: 'success', message: 'Password reset successfully!' });
            setTimeout(() => {
                navigate('/login', { state: { message: 'Password reset successfully! Please login with your new password.' } });
            }, 2000);
        } catch (err) {
            setStatus({ type: 'error', message: err.response?.data || 'Failed to reset password. Token may be expired.' });
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen py-12 flex items-center justify-center bg-muted/30">
            <div className="w-full max-w-md px-4">
                <Card className="border-none shadow-xl bg-card/80 backdrop-blur-sm animate-in fade-in slide-in-from-bottom-10 duration-500">
                    <CardHeader className="space-y-1 text-center">
                        <CardTitle className="text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-primary to-blue-600">
                            Reset Password
                        </CardTitle>
                        <CardDescription>
                            Create a new strong password for your account
                        </CardDescription>
                    </CardHeader>
                    <CardContent>
                        {status.message && (
                            <div className={`text-sm p-3 rounded-md mb-4 flex items-center gap-2 ${status.type === 'error' ? 'bg-destructive/15 text-destructive' : 'bg-emerald-500/15 text-emerald-600'}`}>
                                {status.type === 'error' ? <AlertTriangle className="h-4 w-4" /> : <CheckCircle2 className="h-4 w-4" />}
                                {status.message}
                            </div>
                        )}

                        <form onSubmit={handleSubmit} className="space-y-4">
                            <div className="space-y-2">
                                <Label htmlFor="password">New Password</Label>
                                <Input
                                    id="password"
                                    name="password"
                                    type="password"
                                    placeholder="Enter new password"
                                    value={formData.password}
                                    onChange={handleChange}
                                    required
                                    minLength={8}
                                />
                            </div>
                            <div className="space-y-2">
                                <Label htmlFor="confirmPassword">Confirm Password</Label>
                                <Input
                                    id="confirmPassword"
                                    name="confirmPassword"
                                    type="password"
                                    placeholder="Confirm new password"
                                    value={formData.confirmPassword}
                                    onChange={handleChange}
                                    required
                                    minLength={8}
                                />
                            </div>

                            <Button type="submit" className="w-full" disabled={loading}>
                                {loading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                Reset Password
                            </Button>
                        </form>
                    </CardContent>
                </Card>
            </div>
        </div>
    );
};

export default ResetPassword;
