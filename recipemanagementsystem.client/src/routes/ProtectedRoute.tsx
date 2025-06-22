import React, { JSX, ReactNode } from 'react';
import { Navigate } from 'react-router-dom';
import { isAuthenticated } from '../services/authService';

type ProtectedRouteProps = {
  children: ReactNode;
};

export const ProtectedRoute = ({ children }: ProtectedRouteProps): JSX.Element => {
  return isAuthenticated() ? <>{children}</> : <Navigate to="/login" replace />;
};
