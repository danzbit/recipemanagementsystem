import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  LogLevel,
} from '@microsoft/signalr';
import { PdfStatus } from '../types/PdfStatus';

export class SignalRService {
  private connection: HubConnection | null = null;
  private readonly hubUrl: string;
  private onRecipePublishedCallback: ((message: string) => void) | null = null;
  private onPdfStatusUpdatedCallback: ((status: PdfStatus) => void) | null = null;

  constructor(hubPath: string) {
    this.hubUrl = `${process.env.REACT_APP_API_URL}/hubs/${hubPath}`;
  }

  public async connect(): Promise<void> {
    if (this.connection && this.connection.state === HubConnectionState.Connected) {
      return;
    }

    this.connection = new HubConnectionBuilder()
      .withUrl(this.hubUrl, {
        accessTokenFactory: () => localStorage.getItem('token') || '',
      })
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    this.connection.onreconnecting((error) => {
      console.warn('SignalR reconnecting...', error);
    });

    this.connection.onreconnected(() => {
      console.info('SignalR reconnected.');
    });

    this.connection.onclose((error) => {
      console.error('SignalR connection closed:', error);
    });

    this.connection.on('RecipeCreated', async (message: string) => {
      console.warn('Received RecipeCreated message:', message);

      if (this.onRecipePublishedCallback) {
        this.onRecipePublishedCallback(message);
      }

      setTimeout(async () => {
        await this.disconnect();
      }, 3000);
    });

    this.connection.on('PdfStatusUpdated', async (status: PdfStatus) => {
      console.warn('Received PdfStatusUpdated status:', status);

      if (this.onPdfStatusUpdatedCallback) {
        this.onPdfStatusUpdatedCallback(status);
      }

      setTimeout(async () => {
        await this.disconnect();
      }, 3000);
    });

    try {
      await this.connection.start();
      console.info('SignalR connected:', this.hubUrl);
    } catch (err) {
      console.error('SignalR connection failed:', err);
    }
  }

  public on<T>(event: string, callback: (data: T) => void): void {
    this.connection?.on(event, callback);
  }

  public off(event: string): void {
    this.connection?.off(event);
  }

  public async invoke(method: string, ...args: any[]): Promise<any> {
    if (!this.connection || this.connection.state !== HubConnectionState.Connected) {
      throw new Error('SignalR connection is not established.');
    }
    return await this.connection.invoke(method, ...args);
  }

  public async disconnect(): Promise<void> {
    if (this.connection && this.connection.state !== HubConnectionState.Disconnected) {
      await this.connection.stop();
    }
  }

  public onRecipePublished(callback: (message: string) => void): void {
    this.onRecipePublishedCallback = callback;
  }

  public onPdfStatusUpdated(callback: (status: PdfStatus) => void): void {
    this.onPdfStatusUpdatedCallback = callback;
  }
}
