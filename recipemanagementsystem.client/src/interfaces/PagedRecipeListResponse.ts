import { Recipe } from "../types/Recipe";

export interface PagedRecipeListResponse {
    recipes: Recipe[];
    totalPages: number;
}