import React, { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom';
import { useAppSelector, useRecipe } from '../../../hooks/hooks';
import { Recipe } from '../../../types/Recipe';
import { ErrorMessage, Field, FieldArray, Form, Formik, FormikHelpers } from 'formik';
import { toast } from 'react-toastify';
import { put } from '../../../services/api';
import { initialFormData } from '../form/initialFormData';
import { validationUpdateRecipeSchema } from '../form/recipeSchema';

const UpdateRecipeForm = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { error, recipe } = useRecipe(id!);
    const ownerId = useAppSelector((state) => state.auth.userId);

    const [initialValues, setInitialValues] = useState<Recipe>(initialFormData);

    useEffect(() => {
        if (recipe) {
            setInitialValues(recipe);
        }
    }, [recipe]);

    const handleSubmit = async (
        values: Recipe,
        { setSubmitting, setStatus }: FormikHelpers<Recipe>
    ) => {
        if (!ownerId) {
            toast.error("User is not logged in.");
            return;
        }

        try {
            const payload = { ...values, ownerId };

            for (let ingredient of payload.ingredients) {
                ingredient.recipeId = payload.id;
                ingredient.productId = ingredient.productId ?? null;
            }
            await put<Recipe>(`/api/recipe/${payload.id}`, payload);

            toast.success('Recipe updated!');
            navigate('/');
        } catch (err: any) {
            const apiErrors = err.response?.data?.errors ?? ["Failed to update recipe."];
            setStatus({ submit: apiErrors });
        } finally {
            setSubmitting(false);
        }
    };

    if (error) return <div className="alert alert-danger">Failed to load recipe.</div>;
    if (!recipe) return <div>Loading...</div>;

    return (
        <>
            <div className="d-flex justify-content-between align-items-center mt-4 p-4">
                <h2 className="">Update Recipe</h2>

                <div className="">
                    <button className="btn btn-outline-secondary" onClick={() => navigate("/")}>
                        ← Back to Recipes
                    </button>
                </div>
            </div>
            <Formik<Recipe, { submit?: string[] }>
                enableReinitialize
                initialValues={initialValues}
                validationSchema={validationUpdateRecipeSchema}
                onSubmit={handleSubmit}
            >
                {({ values, isSubmitting, status }) => (
                    <Form className="border rounded bg-light p-4">
                        {/* Title */}
                        <div className="mb-3">
                            <label htmlFor="title" className="form-label">Title</label>
                            <Field name="title" className="form-control" />
                            <ErrorMessage name="title" component="div" className="text-danger" />
                        </div>

                        {/* Description */}
                        <div className="mb-3">
                            <label htmlFor="description" className="form-label">Description</label>
                            <Field as="textarea" name="description" rows={3} className="form-control" />
                        </div>

                        {/* Calorie */}
                        <div className="mb-3">
                            <label htmlFor="calorie" className="form-label">Calorie</label>
                            <Field as="textarea" name="calorie" rows={2} className="form-control" />
                            <ErrorMessage name="calorie" component="div" className="text-danger" />
                        </div>

                        {/* Category */}
                        <div className="mb-3">
                            <label htmlFor="recipeCategory" className="form-label">Category</label>
                            <Field as="select" name="recipeCategory" className="form-select">
                                <option value="">Select category</option>
                                <option value="Breakfast">Breakfast</option>
                                <option value="Lunch">Lunch</option>
                                <option value="Dinner">Dinner</option>
                                <option value="Snacks">Snacks</option>
                                <option value="Desserts">Desserts</option>
                            </Field>
                            <ErrorMessage name="recipeCategory" component="div" className="text-danger" />
                        </div>

                        {/* Ingredients */}
                        <div className="mb-3">
                            <label className="form-label">Ingredients</label>
                            <FieldArray name="ingredients">
                                {({ push, remove }) => (
                                    <>
                                        {values.ingredients.map((_, index) => (
                                            <div className="row mb-2" key={index}>
                                                <div className="col-md-4">
                                                    <Field
                                                        name={`ingredients[${index}].quantity`}
                                                        type="number"
                                                        step="any"
                                                        placeholder="Quantity"
                                                        className="form-control"
                                                    />
                                                    <ErrorMessage name={`ingredients[${index}].quantity`} component="div" className="text-danger" />
                                                </div>
                                                <div className="col-md-6">
                                                    <Field
                                                        name={`ingredients[${index}].product`}
                                                        placeholder="Product"
                                                        className="form-control"
                                                    />
                                                    <ErrorMessage name={`ingredients[${index}].product`} component="div" className="text-danger" />
                                                </div>
                                                <div className="col-md-2">
                                                    <button
                                                        type="button"
                                                        className="btn btn-danger"
                                                        onClick={() => remove(index)}
                                                    >
                                                        Remove
                                                    </button>
                                                </div>
                                            </div>
                                        ))}
                                        <button
                                            type="button"
                                            className="btn btn-secondary mt-2"
                                            onClick={() => push({ id: null, quantity: 0, product: ""  })}
                                        >
                                            Add Ingredient
                                        </button>
                                    </>
                                )}
                            </FieldArray>
                        </div>

                        {/* Submit Errors */}
                        {status?.submit && (
                            <div className="alert alert-danger">
                                {Array.isArray(status.submit)
                                    ? <ul>{status.submit.map((msg: any, i: any) => <li key={i}>{msg}</li>)}</ul>
                                    : <div>{status.submit}</div>}
                            </div>
                        )}

                        <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
                            {isSubmitting ? "Saving..." : "Update Recipe"}
                        </button>
                    </Form>
                )}
            </Formik>
        </>
    )
}


export default UpdateRecipeForm
