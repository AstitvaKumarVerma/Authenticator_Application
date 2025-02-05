export class AuthInputModel {
    Email!: string;
    Pin!: number;
  }


  export class AuthProviderModel {
    Account!: string;
    ManualEntryKey!: string;
    QrCodeSetupImageUrl!: string;
  }