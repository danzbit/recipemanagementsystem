import { Link } from 'react-router-dom'
import { RecipeItemProps } from '../types/types'
import UpdateRecipeButton from '../../../components/buttons/UpdateRecipeButton'
import DeleteRecipeButton from '../../../components/buttons/DeleteRecipeButton'

const RecipeItem = ({ recipe }: RecipeItemProps) => {
    return (
        <div className="card mb-4 shadow-sm border-0">
            <div className="card-body">
                <div className="d-flex justify-content-between align-items-start mb-2">
                    <div>
                        <h5 className="fw-bold mb-1">{recipe.title}</h5>
                        <p className="text-muted mb-0">
                            {recipe.description?.substring(0, 120) || 'No description provided.'}
                        </p>
                    </div>

                    <div className="d-flex flex-column align-items-end gap-2 ms-3">
                        <UpdateRecipeButton
                            recipeId={recipe.id!}
                        />
                        <DeleteRecipeButton
                            recipeId={recipe.id!}
                        />
                    </div>
                </div>

                <Link
                    to={`recipe/${recipe.id}`}
                    className="btn btn-link text-decoration-none ps-0"
                >
                    View Full Recipe →
                </Link>
            </div>
        </div>
    )
}

export default RecipeItem
