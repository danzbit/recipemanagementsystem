import { FilterParams } from "../interfaces/FilterParams";
import { PagedRecipeListResponse } from "../interfaces/PagedRecipeListResponse";
import { Recipe } from "../types/Recipe";
import { del, get } from "./api";

export const fetchRecipes = async (
  filters: FilterParams
): Promise<PagedRecipeListResponse> => {
  const response = await get<PagedRecipeListResponse>("/api/recipe", { params: filters });

  return response;
};

export const fetchRecipe = async (id: string): Promise<Recipe | null> => {
  try {
    const response = await get<Recipe>(`/api/recipe/${id}`);
    return response;
  } catch (err: any) {
    return null;
  }
};

export const deleteRecipe = async (
  recipeId: string
): Promise<void> => {
  await del(`/api/recipe/${recipeId}`)
}