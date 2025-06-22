import React from 'react'

interface PaginationProps {
    currentPage: number;
    totalPages: number;
    onPageChange: (page: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({
    currentPage,
    totalPages,
    onPageChange,
}) => {
    if (totalPages <= 1) return null;

    return (
        <div className="d-flex gap-2 mt-4 justify-content-center">
            <button
                disabled={currentPage === 0}
                onClick={() => onPageChange(currentPage - 1)}
                className="btn btn-outline-secondary"
            >
                Prev
            </button>
            {[...Array(totalPages)].map((_, i) => (
                <button
                    key={i}
                    onClick={() => onPageChange(i)}
                    className={`btn ${currentPage === i ? "btn-primary" : "btn-outline-secondary"}`}
                >
                    {i + 1}
                </button>
            ))}
            <button
                disabled={currentPage === totalPages}
                onClick={() => onPageChange(currentPage + 1)}
                className="btn btn-outline-secondary"
            >
                Next
            </button>
        </div>
    )
}

export default Pagination
