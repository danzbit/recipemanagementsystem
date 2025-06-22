import { toast } from 'react-toastify';
import { deleteRecipe } from '../../services/recipeService';
import { RecipeButtonProps } from './types';

const DeleteRecipeButton = ({ recipeId }: RecipeButtonProps) => {
    const handleDelete = async () => {
        if (!window.confirm('Are you sure you want to delete this recipe?')) return;

        try {
            await deleteRecipe(recipeId);
            toast.success('Recipe deleted successfully');
            setTimeout(() => {
                window.location.reload();
            }, 1000);
        } catch (err) {
            toast.error('Failed to delete recipe');
        }
    };
    return (
        <button
            className="btn btn-sm btn-outline-danger"
            onClick={handleDelete}
        >
            🗑 Delete
        </button>
    )
}

export default DeleteRecipeButton
