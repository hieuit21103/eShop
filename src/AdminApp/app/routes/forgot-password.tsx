import React, { useState } from 'react';
import { Eye, EyeOff, Lock, Mail, ArrowRight, User, UserPlus, LockKeyhole, KeyIcon, KeySquare, KeyRound } from 'lucide-react';

const RegisterPage: React.FC = () => {
    const [email, setEmail] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const handleSubmit = async () => {

        setIsLoading(true);

        // Simulate API call
        fetch('http://localhost:5295/api/auth/forgot-password/' + email, {   
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        })
        setTimeout(() => {
            setIsLoading(false);
            //   console.log('Register attempt:', { email, username, password });
        }, 2000);
    };

    return (
        <div className="min-h-screen bg-gradient-to-r from-blue-50 via-white to-purple-50 flex items-center justify-center p-4">
            <div className="max-w-md w-full bg-white rounded-3xl shadow-2xl border border-gray-100 p-8 backdrop-blur-sm space-y-8">
                {/* Header */}
                <div className="text-center">
                    <div className="mx-auto h-16 w-16 bg-gradient-to-r from-blue-500 to-purple-500 rounded-2xl flex items-center justify-center mb-6 shadow-lg">
                        <KeyRound className="h-8 w-8 text-white" />
                    </div>
                    <h2 className="text-3xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent">
                        Đặt lại mật khẩu
                    </h2>
                    <p className="mt-2 text-gray-600">
                        Vui lòng nhập email để đặt lại mật khẩu
                    </p>
                </div>

                {/* Register Form */}
                <div className="mt-8 space-y-6">
                    <div className="space-y-4">
                        {/* Email Field */}
                        <div>
                            <label htmlFor="email" className="block text-sm font-medium text-gray-700 mb-2">
                                Email
                            </label>
                            <div className="relative">
                                <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                    <Mail className="h-5 w-5 text-gray-400" />
                                </div>
                                <input
                                    id="email"
                                    name="email"
                                    type="email"
                                    required
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    className="text-black block w-full pl-10 pr-3 py-3 border border-gray-300 rounded-xl shadow-sm placeholder-gray-400 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all duration-200"
                                    placeholder="Nhập địa chỉ email của bạn"
                                />
                            </div>
                        </div>

                        {/* Submit Button */}
                        <div>
                            <button
                                type="button"
                                disabled={isLoading}
                                onClick={handleSubmit}
                                className="group relative w-full flex justify-center py-3 px-4 border border-transparent text-sm font-medium rounded-xl text-white bg-gradient-to-r from-blue-600 to-purple-600 hover:from-blue-600 hover:to-purple-600 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-all duration-200 shadow-lg hover:shadow-xl transform hover:-translate-y-0.5 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none"
                            >
                                {isLoading ? (
                                    <div className="flex items-center">
                                        <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white mr-2"></div>
                                        Đang gửi email...
                                    </div>
                                ) : (
                                    <div className="flex items-center">
                                        Gửi email đặt lại mật khẩu
                                        <ArrowRight className="ml-2 h-4 w-4 group-hover:translate-x-1 transition-transform" />
                                    </div>
                                )}
                            </button>
                        </div>

                        {/* Sign In Link */}
                        <div className="text-center">
                            <p className="text-sm text-gray-600">
                                Đã có tài khoản?{' '}
                                <a href="#" className="font-medium text-blue-600 hover:text-purple-600 transition-colors">
                                    Đăng nhập ngay
                                </a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RegisterPage;