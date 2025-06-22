import { useNavigate } from 'react-router-dom';

const CreateRecipeButton = () => {
    const navigate = useNavigate();

    const handleClick = () => {
        navigate("/create-recipe");
    };

    return (
        <button className="btn btn-success" onClick={handleClick}>
            + Create New Recipe
        </button>
    );
}

export default CreateRecipeButton
