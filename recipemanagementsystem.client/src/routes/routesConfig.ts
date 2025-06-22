import { lazy } from 'react';
import { RouteConfig } from '../types/RouteConfig';

const Home = lazy(() => import('../pages/Home'));
const Login = lazy(() => import('../features/login/Login'));
const Registration = lazy(() => import('../features/registration/Registration'));
const NewRecipeForm = lazy(() => import('../features/recipes/components/NewRecipeForm'));
const UpdateRecipeForm = lazy(() => import('../features/recipes/components/UpdateRecipeForm'));
const RecipeDetails = lazy(() => import('../features/recipes/components/RecipeDetails'));
const NotFound = lazy(() => import('../pages/NotFound'));

export const ROUTES = {
    HOME: '/',
    LOGIN: '/login',
    REGISTRATION: '/registration',
    NEW_RECIPE: '/create-recipe',
    UPDATE_RECIPE: '/update-recipe/:id',
    RECIPE_DETAILS: '/recipe/:id'
};

export const routesConfig: RouteConfig[] = [
    { path: ROUTES.HOME, element: Home, private: true },
    { path: ROUTES.LOGIN, element: Login, private: false },
    { path: ROUTES.REGISTRATION, element: Registration, private: false },
    { path: ROUTES.NEW_RECIPE, element: NewRecipeForm, private: true },
    { path: ROUTES.UPDATE_RECIPE, element: UpdateRecipeForm, private: true },
    { path: ROUTES.RECIPE_DETAILS, element: RecipeDetails, private: true },
    { path: '*', element: NotFound, private: false },
];
