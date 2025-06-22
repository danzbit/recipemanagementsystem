import { Recipe } from "../../../types/Recipe";

export const initialFormData: Recipe = {
    title: "",
    description: "",
    calorie: 0,
    recipeCategory: "",
    ingredients: [{ quantity: 0, product: '' }],
    scheduledTime: 0,
}