import CreateRecipeButton from '../components/buttons/CreateRecipeButton'
import RecipeItems from '../features/recipes/components/RecipeItems'

const Home = () => {
    return (
        <div className="container">
            <div className="d-flex justify-content-between align-items-center p-4">
                <h2>Recipes</h2>
                <CreateRecipeButton />
            </div>
            <RecipeItems />
        </div>
    )
}

export default Home
