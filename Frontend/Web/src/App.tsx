import { BrowserRouter, Routes, Route } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import { ToastProvider } from "./context/ToastContext";
import Navbar from "./views/components/Navbar";
import ProtectedRoute from "./views/components/ProtectedRoute";
import HomePage from "./views/pages/HomePage";
import LoginPage from "./views/pages/LoginPage";
import RegisterPage from "./views/pages/RegisterPage";
import RoomsPage from "./views/pages/RoomsPage";
import NewBookingPage from "./views/pages/NewBookingPage";
import PaymentPage from "./views/pages/PaymentPage";
import MyBookingsPage from "./views/pages/MyBookingsPage";
import BookingDetailPage from "./views/pages/BookingDetailPage";
import ServicesPage from "./views/pages/ServicesPage";
import ContactPage from "./views/pages/ContactPage";
import AboutPage from "./views/pages/AboutPage";
import ProfilePage from "./views/pages/ProfilePage";
import GalleryPage from "./views/pages/GalleryPage";
import { NotificationProvider } from "./context/NotificationContext";
import { ThemeProvider } from "./context/ThemeContext";
import ForgotPasswordPage from "./views/pages/ForgotPasswordPage";
import BookingConfirmationPage from "./views/pages/BookingConfirmationPage";
import ErrorPage from "./views/pages/ErrorPage";

export default function App() {
  return (
    <AuthProvider>
      <ToastProvider>
        <NotificationProvider>
          <ThemeProvider>
            <BrowserRouter>
              <Navbar />
              <Routes>
                <Route path="/forgot-password" element={<ForgotPasswordPage />} />
                <Route path="/booking/confirmation" element={
                  <ProtectedRoute><BookingConfirmationPage /></ProtectedRoute>
                } />
                <Route path="/" element={<HomePage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/rooms" element={<RoomsPage />} />
                <Route path="/services" element={<ServicesPage />} />
                <Route path="/contact" element={<ContactPage />} />
                <Route path="/about" element={<AboutPage />} />
                <Route path="/gallery" element={<GalleryPage />} />
                <Route path="/bookings/new" element={<ProtectedRoute><NewBookingPage /></ProtectedRoute>} />
                <Route path="/bookings/:id/pay" element={<ProtectedRoute><PaymentPage /></ProtectedRoute>} />
                <Route path="/bookings/:id" element={<ProtectedRoute><BookingDetailPage /></ProtectedRoute>} />
                <Route path="/my-bookings" element={<ProtectedRoute><MyBookingsPage /></ProtectedRoute>} />
                <Route path="/profile" element={<ProtectedRoute><ProfilePage /></ProtectedRoute>} />
                <Route path="*" element={<ErrorPage />} />
              </Routes>
            </BrowserRouter>
          </ThemeProvider>
        </NotificationProvider>
      </ToastProvider>
    </AuthProvider>
  );
}