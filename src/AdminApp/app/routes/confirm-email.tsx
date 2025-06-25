import React, { useEffect, useState } from 'react';
import { useSearchParams } from "react-router";
import { Mail, CheckCircle, XCircle, AlertCircle, ArrowRight, RefreshCw } from 'lucide-react';
import AuthService from '../services/auth-service';

const ConfirmEmail: React.FC = () => {
    const [searchParams] = useSearchParams();
    const [confirmationStatus, setConfirmationStatus] = useState<string | null>(
        null
    );
    const [isRetrying, setIsRetrying] = useState(false);

    useEffect(() => {
        const userId = searchParams.get("userId");
        const token = searchParams.get("token");
        if (userId && token) {
            // Simulate API call to confirm email
            const encodeToken = encodeURIComponent(token);
            confirmEmail(userId, encodeToken)
                .then(() => setConfirmationStatus("success"))
                .catch(() => setConfirmationStatus("error"));
        } else {
            setConfirmationStatus("invalid");
        }
    }, [searchParams]);

    const confirmEmail = async (userId: string, token: string) => {
        try {
            await AuthService.confirmEmail(userId, token);
        } catch (error) {
            console.error("Email confirmation failed:", error);
            throw new Error("Email confirmation failed");
        }
    };

    const handleRetry = async () => {
        setIsRetrying(true);
        const userId = searchParams.get("userId");
        const token = searchParams.get("token");

        if (userId && token) {
            const encodeToken = encodeURIComponent(token);
            try {
                await confirmEmail(userId, encodeToken);
                setConfirmationStatus("success");
            } catch {
                setConfirmationStatus("error");
            }
        }
        setIsRetrying(false);
    };

    const handleGoToLogin = () => {
        // Replace with actual navigation
        window.location.href = "/login"; // or use react-router's navigate function
        console.log("Navigate to login page");
    };

    const renderContent = () => {
        switch (confirmationStatus) {
            case "success":
                return (
                    <>
                        <div className="mx-auto h-16 w-16 bg-gradient-to-r from-green-500 to-emerald-500 rounded-2xl flex items-center justify-center mb-6 shadow-lg">
                            <CheckCircle className="h-8 w-8 text-white" />
                        </div>
                        <h2 className="text-3xl font-bold bg-gradient-to-r from-green-600 to-emerald-600 bg-clip-text text-transparent mb-4">
                            Email đã được xác nhận!
                        </h2>
                        <p className="text-gray-600 mb-8 text-center">
                            Tài khoản của bạn đã được kích hoạt thành công. Bạn có thể đăng nhập ngay bây giờ.
                        </p>
                        <button
                            onClick={handleGoToLogin}
                            className="group relative w-full flex justify-center py-3 px-4 border border-transparent text-sm font-medium rounded-xl text-white bg-gradient-to-r from-green-600 to-emerald-600 hover:from-green-700 hover:to-emerald-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500 transition-all duration-200 shadow-lg hover:shadow-xl transform hover:-translate-y-0.5"
                        >
                            <div className="flex items-center">
                                Đăng nhập ngay
                                <ArrowRight className="ml-2 h-4 w-4 group-hover:translate-x-1 transition-transform" />
                            </div>
                        </button>
                    </>
                );

            case "error":
                return (
                    <>
                        <div className="mx-auto h-16 w-16 bg-gradient-to-r from-red-500 to-rose-500 rounded-2xl flex items-center justify-center mb-6 shadow-lg">
                            <XCircle className="h-8 w-8 text-white" />
                        </div>
                        <h2 className="text-3xl font-bold bg-gradient-to-r from-red-600 to-rose-600 bg-clip-text text-transparent mb-4">
                            Xác nhận thất bại
                        </h2>
                        <p className="text-gray-600 mb-8 text-center">
                            Có lỗi xảy ra khi xác nhận email của bạn. Vui lòng thử lại hoặc liên hệ hỗ trợ.
                        </p>
                        <div className="space-y-3">
                            <button
                                onClick={handleRetry}
                                disabled={isRetrying}
                                className="group relative w-full flex justify-center py-3 px-4 border border-transparent text-sm font-medium rounded-xl text-white bg-gradient-to-r from-red-600 to-rose-600 hover:from-red-700 hover:to-rose-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500 transition-all duration-200 shadow-lg hover:shadow-xl transform hover:-translate-y-0.5 disabled:opacity-50 disabled:cursor-not-allowed disabled:transform-none"
                            >
                                {isRetrying ? (
                                    <div className="flex items-center">
                                        <div className="animate-spin rounded-full h-4 w-4 border-b-2 border-white mr-2"></div>
                                        Đang thử lại...
                                    </div>
                                ) : (
                                    <div className="flex items-center">
                                        <RefreshCw className="mr-2 h-4 w-4" />
                                        Thử lại
                                    </div>
                                )}
                            </button>
                            <button
                                onClick={handleGoToLogin}
                                className="w-full py-3 px-4 border border-gray-300 text-sm font-medium rounded-xl text-gray-700 bg-white hover:bg-gray-50 hover:border-gray-400 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 transition-all duration-200 shadow-sm"
                            >
                                Quay lại đăng nhập
                            </button>
                        </div>
                    </>
                );

            case "invalid":
                return (
                    <>
                        <div className="mx-auto h-16 w-16 bg-gradient-to-r from-yellow-500 to-orange-500 rounded-2xl flex items-center justify-center mb-6 shadow-lg">
                            <AlertCircle className="h-8 w-8 text-white" />
                        </div>
                        <h2 className="text-3xl font-bold bg-gradient-to-r from-yellow-600 to-orange-600 bg-clip-text text-transparent mb-4">
                            Link không hợp lệ
                        </h2>
                        <p className="text-gray-600 mb-8 text-center">
                            Link xác nhận email không hợp lệ hoặc đã hết hạn. Vui lòng kiểm tra lại email của bạn.
                        </p>
                        <div className="space-y-3">
                            <button
                                className="w-full py-3 px-4 border border-gray-300 text-sm font-medium rounded-xl text-gray-700 bg-white hover:bg-gray-50 hover:border-gray-400 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-gray-500 transition-all duration-200 shadow-sm"
                            >
                                Gửi lại email xác nhận
                            </button>
                            <button
                                onClick={handleGoToLogin}
                                className="w-full py-3 px-4 border border-transparent text-sm font-medium rounded-xl text-white bg-gradient-to-r from-blue-600 to-purple-600 hover:from-blue-700 hover:to-purple-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 transition-all duration-200 shadow-lg hover:shadow-xl transform hover:-translate-y-0.5"
                            >
                                Về trang đăng nhập
                            </button>
                        </div>
                    </>
                );

            default:
                return (
                    <>
                        <div className="mx-auto h-16 w-16 bg-gradient-to-r from-blue-500 to-purple-500 rounded-2xl flex items-center justify-center mb-6 shadow-lg">
                            <Mail className="h-8 w-8 text-white animate-pulse" />
                        </div>
                        <h2 className="text-3xl font-bold bg-gradient-to-r from-blue-600 to-purple-600 bg-clip-text text-transparent mb-4">
                            Đang xác nhận email...
                        </h2>
                        <p className="text-gray-600 mb-8 text-center">
                            Vui lòng chờ trong giây lát, chúng tôi đang xác nhận email của bạn.
                        </p>
                        <div className="flex justify-center">
                            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
                        </div>
                    </>
                );
        }
    };

    return (
        <div className="min-h-screen bg-gradient-to-r from-blue-50 via-white to-purple-50 flex items-center justify-center p-4">
            <div className="max-w-md w-full bg-white rounded-3xl shadow-2xl border border-gray-100 p-8 backdrop-blur-sm">
                <div className="text-center">
                    {renderContent()}
                </div>

                {/* Footer */}
                <div className="mt-8 pt-6 border-t border-gray-100">
                    <p className="text-xs text-gray-500 text-center">
                        Nếu bạn gặp khó khăn, vui lòng liên hệ{' '}
                        <a href="#" className="text-blue-600 hover:text-purple-600 transition-colors">
                            hỗ trợ khách hàng
                        </a>
                    </p>
                </div>
            </div>
        </div>
    );
};

export default ConfirmEmail;