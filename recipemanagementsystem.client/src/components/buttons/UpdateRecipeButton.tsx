import React from 'react'
import { RecipeButtonProps } from './types'
import { useNavigate } from 'react-router-dom'

const UpdateRecipeButton = ({ recipeId }: RecipeButtonProps) => {
    const navigate = useNavigate();

    return (
        <button
            className="btn btn-sm btn-outline-primary"
            onClick={() => navigate(`/update-recipe/${recipeId}`)}
        >
            ✏️ Edit
        </button>
    )
}

export default UpdateRecipeButton
