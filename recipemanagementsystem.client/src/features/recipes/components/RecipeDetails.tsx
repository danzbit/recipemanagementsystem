import { useNavigate, useParams } from 'react-router-dom';
import { useRecipe } from '../../../hooks/hooks';
import DownloadPdfButton from '../../../components/buttons/DownloadPdfButton';

const RecipeDetails = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const { error, recipe } = useRecipe(id!);

    if (error) return <div className="alert alert-danger mt-4 text-center">{error}</div>;
    if (!recipe) return <div className="text-center mt-5">Loading...</div>;

    return (
        <div className="container mt-5">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2 className="text-primary">{recipe.title}</h2>
                <button className="btn btn-outline-secondary" onClick={() => navigate('/')}>
                    ← Back to Recipes
                </button>
            </div>

            <div className="card shadow-lg border-0 mb-5">
                <div className="card-body p-4">
                    <div className="d-flex justify-content-between align-items-start mb-3">
                        <h2 className="card-title">{recipe.title}</h2>
                        <span className="badge bg-secondary fs-6">{recipe.recipeCategory}</span>
                    </div>

                    {recipe.description && (
                        <div className="mb-3">
                            <h5 className="text-muted">📝 Description</h5>
                            <p className="mb-0">{recipe.description}</p>
                        </div>
                    )}

                    <div className="mb-3">
                        <h5 className="text-muted">🔥 Calories</h5>
                        <p className="mb-0">{recipe.calorie} kcal</p>
                    </div>

                    <div className="mb-4">
                        <h5 className="text-muted">🥕 Ingredients</h5>
                        <ul className="list-group">
                            {recipe.ingredients.map((ingredient, index) => (
                                <li
                                    key={index}
                                    className="list-group-item d-flex justify-content-between align-items-center"
                                >
                                    <span>{ingredient.product}</span>
                                    <span className="badge bg-primary rounded-pill">
                                        {ingredient.quantity}
                                    </span>
                                </li>
                            ))}
                        </ul>
                    </div>

                    {recipe.collaborators && recipe.collaborators.length > 0 && (
                        <div className="mb-3">
                            <h5 className="text-muted">🤝 Collaborators</h5>
                            <ul className="list-group">
                                {recipe.collaborators.map((collab, idx) => (
                                    <li key={idx} className="list-group-item d-flex justify-content-between">
                                        <span>{collab.collaboratorEmail}</span>
                                        <small className="text-muted">
                                            Joined: {new Date(collab.joinedAt).toLocaleDateString()}
                                        </small>
                                    </li>
                                ))}
                            </ul>
                        </div>
                    )}

                    <DownloadPdfButton recipeId={recipe.id!} />
                </div>
            </div>
        </div>
    )
}

export default RecipeDetails
