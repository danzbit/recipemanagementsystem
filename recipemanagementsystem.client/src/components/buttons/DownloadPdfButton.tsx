import { DownloadPdfButtonProps } from './types'
import { handleDownloadPDF } from '../../services/pdfService';
import { SignalRService } from '../../services/signalr';
import { toast } from 'react-toastify';
import { PdfStatusType } from '../../enums/PdfStatusType';
import { post } from '../../services/api';
import { useState } from 'react';

const DownloadPdfButton = ({ recipeId }: DownloadPdfButtonProps) => {
    const [loading, setLoading] = useState<boolean>(false);
    const signalRService = new SignalRService(`pdf-status?recipeId=${recipeId}`);

    const handleDownload = async () => {
        try {
            var response = await post<string>(`/api/pdf/generatePdf?recipeId=${recipeId}`)

            setLoading(true);
            await signalRService.connect();

            signalRService.onPdfStatusUpdated(async (model) => {
                if (model.status === PdfStatusType.Failed) {
                    toast.warn('Download pdf file was failed.');
                    await signalRService.disconnect();
                }
                if (model.status === PdfStatusType.Completed) {
                    await handleDownloadPDF(response)
                    await signalRService.disconnect();
                }
            });
        }
        catch (err: any) {
            toast.error(err.response?.errors ?? "Failed to submit recipe.");
        } finally {
            setLoading(false);
        }
    };

    if (loading) return <div>Loading...</div>;

    return (
        <button
            onClick={handleDownload}
            className="btn btn-primary mt-3">
            Download PDF
        </button>
    )
}

export default DownloadPdfButton
