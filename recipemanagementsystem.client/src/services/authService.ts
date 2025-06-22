const TOKEN_KEY = 'token';

export const isAuthenticated = (): boolean => {
    return !!localStorage.getItem(TOKEN_KEY);
};

export const getToken = (): string | null => {
    return localStorage.getItem(TOKEN_KEY);
};