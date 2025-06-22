import { useRecipes } from '../../../hooks/hooks';
import Pagination from '../../../components/pagination/Pagination';
import FilterForm from '../../../components/filter/FilterForm';
import RecipeItem from './RecipeItem';

const RecipeItems = () => {
    const {
        data,
        loading,
        error,
        currentPage,
        totalPages,
        filters,
        updateFilters,
        setPage, } = useRecipes({
            pageSize: 5,
        });

    if (loading) return <div>Loading...</div>;
    if (error) return <div>{error}</div>;

    return (
        <div className="p-4">
            <FilterForm filters={filters} onApplyFilters={updateFilters} />
            {data?.recipes.map((recipe) => (
                <RecipeItem key={recipe.id} recipe={recipe} />
            ))}

            {data && (
                <Pagination
                    currentPage={currentPage}
                    totalPages={totalPages}
                    onPageChange={setPage}
                />
            )}
        </div>
    );
}

export default RecipeItems
