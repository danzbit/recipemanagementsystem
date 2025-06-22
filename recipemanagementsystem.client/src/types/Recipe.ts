import { Ingredeint } from "./Ingredient";
import { RecipeCollaborator } from "./RecipeCollaborator";

export type Recipe = {
    id?: string | null;
    title: string;
    description?: string;
    calorie: number;
    recipeCategory: string;
    ingredients: Ingredeint[];
    collaborators?: RecipeCollaborator[];
    ownerId?: string;
    scheduledTime: number;
}