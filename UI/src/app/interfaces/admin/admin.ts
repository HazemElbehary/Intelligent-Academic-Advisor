export interface AdminDto {
  AdminID: number;
  UserName: string;
  FullName: string;
  Position: string;
  Department?: string;
  Token: string;
}

export interface LoginDto {
  FCAIID: number;
  Password: string;
} 