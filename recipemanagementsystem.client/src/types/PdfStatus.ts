import { PdfStatusType } from "../enums/PdfStatusType";

export type PdfStatus = {
    id: string;
    status: PdfStatusType;
    recipeId: string;
}