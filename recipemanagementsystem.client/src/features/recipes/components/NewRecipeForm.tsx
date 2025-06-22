import { Recipe } from '../../../types/Recipe';
import { post } from '../../../services/api';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { useAppSelector } from '../../../hooks/hooks';
import { Form, ErrorMessage, Field, FieldArray, Formik, FormikErrors } from 'formik';
import { validationCreateNewRecipeSchema } from '../form/recipeSchema';
import { initialFormData } from '../form/initialFormData';
import { SignalRService } from '../../../services/signalr';

const NewRecipeForm = () => {
    const navigate = useNavigate();
    const ownerId = useAppSelector((state) => state.auth.userId);
    const signalRService = new SignalRService(`recipe?userId=${ownerId}`);

    const handleSubmit = async (
        values: Recipe,
        {
            setSubmitting,
            setErrors,
        }: {
            setSubmitting: (isSubmitting: boolean) => void;
            setErrors: (errors: Partial<FormikErrors<Recipe>> & { submit?: string[] }) => void;
        }
    ) => {
        if (!ownerId) {
            toast.error('User is not logged in.');
            return;
        }

        try {
            const payload = { ...values, ownerId };

            if (Number(payload.scheduledTime) > 0) {
                await post<Recipe>('/api/recipe/schedule', payload);
                toast.success('Recipe scheduled!');

                await signalRService.connect();

                signalRService.onRecipePublished((message) => {
                    toast.warn(message || 'You were disconnected by the server.');

                    setTimeout(async () => {
                        await signalRService.disconnect();
                    }, 3000);
                });
            } else {
                await post<Recipe>('/api/recipe', payload);
                toast.success('Recipe created!');
            }

            navigate('/');
        } catch (err: any) {
            setErrors({ submit: err.response?.errors ?? ["Failed to submit recipe."] });
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <>
            <div className="d-flex justify-content-between align-items-center mt-4 p-4">
                <h2 className="">Create New Recipe</h2>

                <div className="">
                    <button className="btn btn-outline-secondary" onClick={() => navigate("/")}>
                        ← Back to Recipes
                    </button>
                </div>
            </div>
            <Formik
                initialValues={initialFormData}
                validationSchema={validationCreateNewRecipeSchema}
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
                            <Field as="textarea" name="calorie" rows={3} className="form-control" />
                            <ErrorMessage name="calorie" component="div" className="text-danger" />
                        </div>

                        {/* Scheduled Time */}
                        <div className="mb-3">
                            <label htmlFor="scheduledTime" className="form-label">Time for Scheduling in Minutes (Optional)</label>
                            <Field as="textarea" name="scheduledTime" rows={3} className="form-control" />
                            <ErrorMessage name="scheduledTime" component="div" className="text-danger" />
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
                                                        disabled={values.ingredients.length === 1}
                                                    >
                                                        Remove
                                                    </button>
                                                </div>
                                            </div>
                                        ))}
                                        <button type="button" className="btn btn-secondary mt-2" onClick={() => push({ quantity: 0, product: "" })}>
                                            Add Ingredient
                                        </button>
                                    </>
                                )}
                            </FieldArray>
                        </div>

                        {/* Submit Error */}
                        {status?.submit && (
                            <div className="alert alert-danger">
                                {Array.isArray(status.submit)
                                    ? <ul>{status.submit.map((msg: string, i: number) => <li key={i}>{msg}</li>)}</ul>
                                    : <div>{status.submit}</div>}
                            </div>
                        )}

                        <button type="submit" className="btn btn-primary" disabled={isSubmitting}>
                            {isSubmitting ? "Submitting..." : "Create Recipe"}
                        </button>
                    </Form>
                )}
            </Formik>
        </>
    )
}

export default NewRecipeForm
