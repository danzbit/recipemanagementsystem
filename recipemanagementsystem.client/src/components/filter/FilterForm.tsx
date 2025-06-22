import React, { ChangeEvent, FormEvent, useState } from 'react'
import { FilterParams } from '../../interfaces/FilterParams';

interface FilterFormProps {
    filters: FilterParams;
    onApplyFilters: (filters: FilterParams) => void;
}

const FilterForm: React.FC<FilterFormProps> = ({ filters, onApplyFilters }) => {
    const [localFilters, setLocalFilters] = useState<FilterParams>(filters);

    const handleChange = (key: keyof FilterParams, value: string) => {
        setLocalFilters((prev) => ({ ...prev, [key]: value }));
    };

    const handleInputChange = (e: ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        handleChange(name as keyof FilterParams, value);
    };

    const handleSubmit = (e: FormEvent) => {
        e.preventDefault();
        onApplyFilters(localFilters);
    };

    return (
        <form onSubmit={handleSubmit} className="mb-4 d-flex gap-3 align-items-center">
            <div className="flex-grow-1" style={{ maxWidth: '200px' }}>
                <label htmlFor="searchInput" className="form-label visually-hidden">
                    Search Recipes
                </label>
                <input
                    type="search"
                    name="search"
                    value={localFilters.search}
                    onChange={handleInputChange}
                    className="form-control"
                    placeholder="Search..."
                    autoComplete="off"
                />
            </div>

            <div style={{ minWidth: '150px' }}>
                <label htmlFor="categorySelect" className="form-label visually-hidden">
                    Category
                </label>
                <select
                    name="category"
                    id="categorySelect"
                    className="form-select"
                    value={localFilters.category}
                    onChange={handleInputChange}
                >
                    <option value="">All</option>
                    <option value="Breakfast">Breakfast</option>
                    <option value="Lunch">Lunch</option>
                    <option value="Dinner">Dinner</option>
                    <option value="Snacks">Snacks</option>
                    <option value="Desserts">Desserts</option>
                </select>
            </div>

            <button type="submit" className="btn btn-primary">
                Apply
            </button>
        </form>
    )
}

export default FilterForm
