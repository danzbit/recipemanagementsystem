import { toast } from "react-toastify";
import { get } from "./api";

export const handleDownloadPDF = async (id: string) => {
    try {
        const response = await get<Blob>(`/api/pdf/download/${id}`, {
            responseType: 'blob',
        });

        const blob = new Blob([response], { type: 'application/json' }); 
        const downloadUrl = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = downloadUrl;
        link.download = 'exported-file.json'; 
        document.body.appendChild(link);
        link.click();
        link.remove();
        window.URL.revokeObjectURL(downloadUrl);
    } catch (err) {
        toast.error('Failed to download PDF.');
    }
};