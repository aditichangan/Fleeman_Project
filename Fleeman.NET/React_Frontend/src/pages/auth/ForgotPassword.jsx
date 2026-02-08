import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import AuthService from '../../services/authService';
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card";
import { AlertTriangle, CheckCircle2, Loader2, ArrowLeft } from "lucide-react";

const ForgotPassword = () => {
    const [email, setEmail] = useState('');
    const [status, setStatus] = useState({ type: '', message: '' });
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setStatus({ type: '', message: '' });
        setLoading(true);

        try {
            await AuthService.forgotPassword(email);
            setStatus({ type: 'success', message: 'If an account exists with this email, you will receive a reset link shortly.' });
        } catch (err) {
            setStatus({ type: 'error', message: 'Failed to process request. Please try again.' });
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
                            Forgot Password?
                        </CardTitle>
                        <CardDescription>
                            Enter your email to receive a reset link
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
                                <Label htmlFor="email">Email Address</Label>
                                <Input
                                    id="email"
                                    type="email"
                                    placeholder="name@example.com"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                />
                            </div>

                            <Button type="submit" className="w-full" disabled={loading}>
                                {loading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                                Send Reset Link
                            </Button>

                            <div className="text-center mt-4">
                                <Link to="/login" className="text-sm text-muted-foreground hover:text-primary flex items-center justify-center gap-1 transition-colors">
                                    <ArrowLeft className="h-3 w-3" /> Back to Login
                                </Link>
                            </div>
                        </form>
                    </CardContent>
                </Card>
            </div>
        </div>
    );
};

export default ForgotPassword;
