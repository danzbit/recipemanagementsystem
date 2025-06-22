import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "../app/store";
import { useEffect, useState } from "react";
import { fetchRecipe, fetchRecipes } from "../services/recipeService";
import { FilterParams } from "../interfaces/FilterParams";
import { PagedRecipeListResponse } from "../interfaces/PagedRecipeListResponse";
import { Recipe } from "../types/Recipe";

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;

export const useRecipes = (initialFilters: FilterParams = {}) => {
    const [currentPage, setCurrentPage] = useState(initialFilters.pageNumber ?? 0);
    const [filters, setFilters] = useState<FilterParams>({ ...initialFilters, pageNumber: 0 });
    const [data, setData] = useState<PagedRecipeListResponse | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const params = { ...filters, pageNumber: currentPage };
        setLoading(true);
        fetchRecipes(params)
            .then((res) => {
                setData(res);
                setError(null);
            })
            .catch(() => setError("Failed to fetch recipes"))
            .finally(() => setLoading(false));
    }, [filters, currentPage]);

    const updateFilters = (filter: FilterParams) => {
        setFilters(filter);
        setCurrentPage(0);
    };

    const setPage = (page: number) => {
        setCurrentPage(page);
    };

    return {
        filters,
        data,
        loading,
        error,
        currentPage,
        totalPages: data?.totalPages ?? 1,
        updateFilters,
        setPage,
    };
}

export const useRecipe = (id: string) => {
    const [recipe, setRecipe] = useState<Recipe | null>(null);
    const [error, setError] = useState('');


    useEffect(() => {
        const fetchRecipeById = async () => {
            const result = await fetchRecipe(id!);

            if (result) {
                setRecipe(result)
            }
            else {
                setError('Failed to load recipe.')
            }
        }

        if (id) fetchRecipeById();
    }, [id]);

    return { recipe, error }
}