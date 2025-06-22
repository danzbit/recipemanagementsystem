import React, { Suspense } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { routesConfig } from './routesConfig';
import { ProtectedRoute } from './ProtectedRoute';

const AppRouter = () => {
    return (
        <Router>
            <Suspense fallback={<div className="text-center mt-5">Loading...</div>}>
                <Routes>
                    {routesConfig.map(({ path, element, private: isPrivate }) => {
                        const routeElement = isPrivate ? (
                            <ProtectedRoute>{React.createElement(element)}</ProtectedRoute>
                        ) : (
                            React.createElement(element)
                        );
                        return <Route key={path} path={path} element={routeElement} />;
                    })}
                </Routes>
            </Suspense>
        </Router>
    );
};

export default AppRouter;
