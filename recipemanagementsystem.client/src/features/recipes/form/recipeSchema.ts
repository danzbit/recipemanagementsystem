import * as Yup from "yup";

export const validationCreateNewRecipeSchema = Yup.object({
    title: Yup.string().required("Title is required"),
    description: Yup.string(),
    calorie: Yup.number().required("Calorie information is required"),
    scheduledTime: Yup.number().min(0, "Must be ≥ 0"),
    recipeCategory: Yup.string().required("Category is required"),
    ingredients: Yup.array()
        .of(
            Yup.object({
                quantity: Yup.number().min(0, "Must be ≥ 0").required("Required"),
                product: Yup.string().required("Product is required"),
            })
        )
        .min(1, "At least one ingredient is required"),
});

export const validationUpdateRecipeSchema = Yup.object({
    title: Yup.string().required("Title is required"),
    description: Yup.string(),
    calorie: Yup.number().required("Calorie information is required"),
    scheduledTime: Yup.number(),
    recipeCategory: Yup.string().required("Category is required"),
    ingredients: Yup.array()
        .of(
            Yup.object({
                quantity: Yup.number().min(0, "Must be ≥ 0").required("Required"),
                product: Yup.string().required("Product is required"),
            })
        )
});